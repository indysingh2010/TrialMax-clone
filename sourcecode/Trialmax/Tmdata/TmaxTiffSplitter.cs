using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.ActiveX;

using Interop.TiffPageSplitDLL; // Sherroden

namespace FTI.Trialmax.Database
{
	/// <summary>This class is used to split a multipage TIFF file into individual pages</summary>
	public class CTmaxTiffSplitter
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX					= 0;
		private const int ERROR_NO_TMX_VIEW						= 1;
		private const int ERROR_SOURCE_NOT_FOUND				= 2;
		private const int ERROR_CREATE_FOLDER_FAILED			= 3;
		private const int ERROR_CREATE_SHERROD_FAILED			= 4;
		private const int ERROR_SPLIT_EX						= 5;
		
		/// <summary>Sherrod library error levels</summary>
		private const int SHERROD_SUCCESS			= 1;
		private const int SHERROD_FILE_NOT_FOUND	= 0;
		private const int SHERROD_GENERAL_ERROR		= -1;
		private const int SHERROD_INVALID_TIFF		= -2;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to TmxView property</summary>
		private FTI.Trialmax.ActiveX.CTmxView m_tmxView = null;
		
		/// <summary>Private member used to perform splitting with Sherrod library</summary>
		Interop.TiffPageSplitDLL.TiffPageSplitterDLLClass m_sherrodSplitter = null;

		/// <summary>Local member to store files created by the split operation</summary>
		private CTmaxSourceFolder m_tmaxPageFolder = new CTmaxSourceFolder();
		
		/// <summary>Local member bound to SourceFileSpec property</summary>
		private string m_strSourceFileSpec = "";
		
		/// <summary>Local member bound to PageFolderPath property</summary>
		private string m_strPageFolderPath = "";
		
		/// <summary>Private member to indicate that the operation is in progress</summary>
		private bool m_bInProgress = false;
		
		/// <summary>Private member to keep track of the page numbers</summary>
		private int m_iPage = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This is the delegate used to handle progress events fired by this control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="strSourceFileSpec">The path to the source file</param>
		/// <param name="strPageFileSpec">The path to the page file</param>
		/// <param name="iPageNumber">The number of the saved page</param>
		public delegate void SavedPageHandler(object sender, string strSourceFileSpec, string strPageFileSpec, int iPageNumber);
		
		/// <summary>Fired each time a page has been saved</summary>
		public event SavedPageHandler SavedPage;		

		/// <summary>Constructor</summary>
		public CTmaxTiffSplitter()
		{
			m_tmaxEventSource.Name = "TmaxTiffSplitter";
			
			//	Populate the error builder
			SetErrorStrings();
		}
		
		/// <summary>Called to initialize the object for the split operation</summary>
		/// <param name="strSourceFileSpec">The fully qualified path to the source file</param>
		/// <param name="strPageFolderPath">The fully qualified path to the output folder</param>
		/// <param name="bUseTmxView">true to use the TmxView control to perform the operation</param>
		/// <returns>true if successful</returns>
		public bool Initialize(string strSourceFileSpec, string strPageFolderPath, bool bUseTmxView)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Verify that the source file exists
				if(System.IO.File.Exists(strSourceFileSpec) == false)
				{
					m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_SOURCE_NOT_FOUND, strSourceFileSpec));
					return false;
				}
				
				//	Verify that the folder exists
				if(System.IO.Directory.Exists(strPageFolderPath) == false)
				{
					//	Attempt to create the folder
					try { System.IO.Directory.CreateDirectory(strPageFolderPath); }
					catch {};
				}
				if(System.IO.Directory.Exists(strPageFolderPath) == false)
				{
					//	Unable to create the folder so cancel the operation
					m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_CREATE_FOLDER_FAILED, strPageFolderPath));
					return false;
				}
				
				//	Save the paths
				m_strSourceFileSpec = strSourceFileSpec;
				m_strPageFolderPath = strPageFolderPath;
				
				//	Initialize the pages collection
				m_tmaxPageFolder.Initialize(m_strPageFolderPath);
				m_tmaxPageFolder.Files.Clear();
				
				//	Make sure we have the required splitter
				if(bUseTmxView == true)
				{
					if(m_tmxView == null)
					{
						//	Unable to create the folder so cancel the operation
						m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_NO_TMX_VIEW));
						return false;
					}
				
				}
				else
				{
					try
					{
						m_sherrodSplitter = new Interop.TiffPageSplitDLL.TiffPageSplitterDLLClass();
					}
					catch(System.Exception Ex)
					{
						//	Unable to create the folder so cancel the operation
						m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_CREATE_SHERROD_FAILED), Ex);
						return false;
					}
					
				}// if(bUseTmxView == true)
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX, bUseTmxView), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Initialize(string strSourceFileSpec, string strPageFolderPath, bool bUseTmxView)
		
		/// <summary>Called to perform the page splitting operation</summary>
		/// <returns>true if successful</returns>
		public bool Split()
		{
			bool bSuccessful = false;
			
			try
			{
				m_tmaxEventSource.InitElapsed();
				
				//	Are we using the Sherrod library?
				if(m_sherrodSplitter != null)
				{
					//	Spawn a thread to monitor the progress of the operation
					m_bInProgress = true;
					Thread progressThread = new Thread(new ThreadStart(this.ProgressThreadProc));
					progressThread.Start();
					
					bSuccessful = (m_sherrodSplitter.Tiff_PageSplit(ref m_strSourceFileSpec, ref m_strPageFolderPath) == SHERROD_SUCCESS);
				}
				else
				{
					//	NOTE:	We don't need a progress thread when we use the TmxView control because
					//			if fires progress events that are trapped by the database (OnTmxEvent)
					bSuccessful = (m_tmxView.SavePages(m_strSourceFileSpec, m_strPageFolderPath, "") > 0);
				}
				
				//	Populate the PageFiles collection if successful
				if(bSuccessful == true)
				{
					GetPageFiles();
					
					m_tmaxEventSource.FireElapsed(this, "Split", "Split " + this.PageFiles.Count.ToString() + " pages in " + System.IO.Path.GetFileName(this.SourceFileSpec));
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Split", m_tmaxErrorBuilder.Message(ERROR_SPLIT_EX, this.SourceFileSpec, this.PageFolderPath), Ex);
			}
			finally
			{
				m_bInProgress = false;
				Thread.Sleep(250);	//	Give the progress thread time to terminate
			}
			
			return (m_tmaxPageFolder.Files.Count > 0);
			
		}// public bool Split()
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>Thread service procedure to monitor the operation's progress in a separate thread</summary>
		private void ProgressThreadProc()
		{
			try
			{
				//	Initialize the page counter
				m_iPage = 1;
				
				//	Loop until the operation finishes
				while(m_bInProgress == true)
				{
					//	Check for the split files
					CheckForFiles();
					
					System.Threading.Thread.Sleep(200);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SplitThreadProc", m_tmaxErrorBuilder.Message(ERROR_SPLIT_EX, this.SourceFileSpec, this.PageFolderPath), Ex);
			}
			
		}// private void ProgressThreadProc()
		
		/// <summary>Called to check for files saved to the split folder</summary>
		private void CheckForFiles()
		{
			string strFilename = "";
			
			while(true)
			{
				//	Format the name of the output file
				if(m_strPageFolderPath.EndsWith("\\") == true)
					strFilename = String.Format("{0}{1:00000}.tif", m_strPageFolderPath, m_iPage);
				else
					strFilename = String.Format("{0}\\{1:00000}.tif", m_strPageFolderPath, m_iPage);

				if(System.IO.File.Exists(strFilename) == true)
				{
					m_iPage++;
				}
				else
				{
					//	Update the progress form
					if(m_iPage > 1)
					{
						try
						{
							if(SavedPage != null)
								SavedPage(this, m_strSourceFileSpec, strFilename, m_iPage);
						}
						catch(System.Exception Ex)
						{
							m_tmaxEventSource.FireDiagnostic(this, "CheckProgress", Ex);
						}
					}
					break;
				}
						
			}// while(true)
		
		}// private void CheckForFiles()
		
		/// <summary>Called to populate the collection of page files</summary>
		private bool GetPageFiles()
		{
			try
			{
				//	Get the exported files
				m_tmaxPageFolder.Files.KeepSorted = false;
				m_tmaxPageFolder.GetFiles("*.tif,*.tiff", true);
				m_tmaxPageFolder.Files.Sort(true);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetPageFiles", Ex);
			}
			
			return (m_tmaxPageFolder.Files.Count > 0);
			
		}// private bool GetPageFiles()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the TIFF splitter: UseTmxView = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to perform the TIFF splitting operation. The TmxView control is not available.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the source file for the TIFF splitting operation: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the page folder for the TIFF splitting operation: path = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create an instance of the TIFF splitter library.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to split the TIFF file: filename = %1  page folder = %2");

		}// private void SetErrorStrings()

		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>Fully qualified path to the source file</summary>
		public string SourceFileSpec
		{
			get { return m_strSourceFileSpec; }
		}
		
		/// <summary>Reference to application instance of TmxView control</summary>
		public FTI.Trialmax.ActiveX.CTmxView TmxView
		{
			get { return m_tmxView; }
			set { m_tmxView = value; }
		}
		
		/// <summary>Fully qualified path to the output folder</summary>
		public string PageFolderPath
		{
			get { return m_strPageFolderPath; }
		}
		
		/// <summary>Collection of files created during the operation</summary>
		public CTmaxSourceFiles PageFiles
		{
			get { return m_tmaxPageFolder.Files; }
		}
		
		
		#endregion Properties
	
	}// public class CTmaxTiffSplitter

}// namespace FTI.Trialmax.Database
