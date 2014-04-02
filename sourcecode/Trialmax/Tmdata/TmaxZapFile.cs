using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This class is used to manage the information associated with a zap file (treatment) created by TmaxPresentation</summary>
	public class CTmaxZapFile
	{
		#region Constants

		/// <summary>Packed flag identifiers</summary>
		private const int ZAP_FLAG_HORIZONTAL = 1;

		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX = 0;
		private const int ERROR_SOURCE_NOT_FOUND = 1;
		private const int ERROR_PARSE_EX = 2;
		private const int ERROR_INVALID_ZAP_FILENAME = 3;
		private const int ERROR_SIBLING_NOT_FOUND = 4;
	
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Private member bound to SourceFolder property</summary>
		private string m_strSourceFolder = "";

		/// <summary>Private member bound to SourceFileSpec propert</summary>
		private string m_strSourceFileSpec = "";

		/// <summary>Private member bound to SourcePageId propert</summary>
		private string m_strSourcePageId = "";

		/// <summary>Private member bound to SiblingFileSpec propert</summary>
		private string m_strSiblingFileSpec = "";

		/// <summary>Private member bound to SiblingPageId propert</summary>
		private string m_strSiblingPageId = "";

		/// <summary>Private member bound to Flags property</summary>
		private int m_iFlags = 0;

		/// <summary>Private member bound to SplitScreen property</summary>
		private bool m_bSplitScreen = false;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxZapFile()
		{
			m_tmaxEventSource.Name = "TmaxZapFile";
			
			//	Populate the error builder
			SetErrorStrings();
		}
		
		/// <summary>Called to initialize the object for the split operation</summary>
		/// <param name="strSourceFileSpec">The fully qualified path to the treatment created by TmaxPresentation</param>
		/// <returns>true if successful</returns>
		public bool Initialize(string strSourceFileSpec)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Clear the existing values
				this.m_strSourceFolder = "";
				this.m_strSourceFileSpec = "";
				this.m_strSourcePageId = "";
				this.m_strSiblingFileSpec = "";
				this.m_strSiblingPageId = "";
				this.m_bSplitScreen = false;
				this.m_iFlags = 0;
				
				while(bSuccessful == false)
				{
					//	Save the path to the source file
					m_strSourceFileSpec = strSourceFileSpec;

					//	Extract the components from the name of the zap file
					if(Parse(strSourceFileSpec) == false)
					{
						m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INVALID_ZAP_FILENAME, System.IO.Path.GetFileName(this.m_strSourceFileSpec)));
						break;
					}
						
					//	Verify that the source file exists
					if(System.IO.File.Exists(this.m_strSourceFileSpec) == false)
					{
						m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_SOURCE_NOT_FOUND, this.m_strSourceFileSpec));
						break;
					}

					//	Verify that the sibling file exists
					if((SplitScreen == true) && (SiblingFileSpec.Length > 0))
					{
						if(System.IO.File.Exists(this.SiblingFileSpec) == false)
						{
							m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_SIBLING_NOT_FOUND, this.SiblingFileSpec));
							break;
						}
					}

					//	All done
					bSuccessful = true;

				}// while(bSuccessful == false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
			}

			return bSuccessful;
			
		}// public bool Initialize(string strSourceFileSpec)

		/// <summary>Called to show the values assigned to the objec</summary>
		/// <param name="strTitle">The title to be displayed in the message box</param>
		/// <returns>OK or Cancel</returns>
		public DialogResult Show(string strTitle)
		{
			string strMessage = "";

			strMessage += String.Format("SourceFileSpec: {0}\n", this.m_strSourceFileSpec);
			strMessage += String.Format("SourceFolder: {0}\n", this.m_strSourceFolder);
			strMessage += String.Format("SourcePageId: {0}\n", this.m_strSourcePageId);
			strMessage += String.Format("SiblingFileSpec: {0}\n", this.m_strSiblingFileSpec);
			strMessage += String.Format("SiblingPageId: {0}\n", this.m_strSiblingPageId);
			strMessage += String.Format("SplitScreen: {0}\n", this.SplitScreen);
			strMessage += String.Format("Flags: {0}\n", this.m_iFlags);
			strMessage += String.Format("Horizontal: {0}\n", this.Horizontal);

			return MessageBox.Show(strMessage, strTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

		}// public DialogResult Show(string strTitle)

		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the TmaxPresentation treatment.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the source file: \n\n%1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to parse the zap file specification.");
			m_tmaxErrorBuilder.FormatStrings.Add("%1 is not a properly formatted zap file specification.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the sibling file: \n\n%1");

		}// private void SetErrorStrings()

		/// <summary>Called to extract the components identified in the specified file path</summary>
		/// <param name="strFileSpec">The fully qualified path containing the components needed to register the treatment</param>
		/// <returns>true if successful</returns>
		/// <remarks>The file should be formatted according to: uniqueid_##.zap_</remarks>
		private bool Parse(string strFileSpec)
		{
			char[]	acDelimiter = { '_' };
			string	strFilename = "";
			string	strSibling = "";
			int		iSplitScreen = -1;

			Debug.Assert(strFileSpec.Length > 0);
			if(strFileSpec.Length == 0) return false;

			try
			{
				m_strSourceFolder = System.IO.Path.GetDirectoryName(strFileSpec);
				if(m_strSourceFolder.EndsWith("\\") == false)
					m_strSourceFolder += "\\";

				//	Get the filename
				strFilename = System.IO.Path.GetFileNameWithoutExtension(strFileSpec);

				//	Is this a split screen treatment?
				if((iSplitScreen = strFilename.IndexOf('-')) >= 0)
				{
					//	Separate the filename and the sibling filename
					strSibling = strFilename.Substring(iSplitScreen + 1);
					strFilename = strFilename.Substring(0, iSplitScreen);

					m_strSiblingFileSpec = String.Format("{0}{1}.ssz_", m_strSourceFolder, strSibling);
					
					m_bSplitScreen = true;
				}
				else
				{
					m_bSplitScreen = false;
				}

				//	Separate out the unique id of the parent page
				string[] aSplit = strFilename.Split(acDelimiter);
				if((aSplit != null) && (aSplit.Length > 0))
				{
					m_strSourcePageId = (string)aSplit.GetValue(0);
					
					//	Get the split screen flags
					if(m_bSplitScreen == true)
					{
						if(aSplit.Length > 1)
							m_iFlags = System.Convert.ToInt32((string)aSplit.GetValue(1));
						else
							return false;

					}// if(m_bSplitScreen == true)
					
				}
				else
				{
					return false;
				}
				
				//	Get the id of the parent page for the sibling treatment
				string[] aSibling = strSibling.Split(acDelimiter);
				if((aSibling != null) && (aSibling.Length > 0))
				{
					m_strSiblingPageId = (string)aSibling.GetValue(0);
				}
				else
				{
					return false;
				}				

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Parse", m_tmaxErrorBuilder.Message(ERROR_PARSE_EX), Ex);
			}

			return true;

		}// private bool Parse(string strFileSpec)

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
			set { Initialize(value); }
		}

		/// <summary>Fully qualified path to the folder containing the source file</summary>
		public string SourceFolder
		{
			get { return m_strSourceFolder; }
		}

		/// <summary>Unique database identifier of the source parent page</summary>
		public string SourcePageId
		{
			get { return m_strSourcePageId; }
		}

		/// <summary>Fully qualified path to the zap file for the sibling if split screen</summary>
		public string SiblingFileSpec
		{
			get { return m_strSiblingFileSpec; }
		}

		/// <summary>Unique database identifier of the sibling's parent page</summary>
		public string SiblingPageId
		{
			get { return m_strSiblingPageId; }
		}

		/// <summary>True if this file represents a split screen treatment</summary>
		public bool SplitScreen
		{
			get { return m_bSplitScreen; }
		}

		/// <summary>Packed flags for split screen treatments</summary>
		public int Flags
		{
			get { return m_iFlags; }
		}

		/// <summary>True if this is a horizontal split screen zap file</summary>
		public bool Horizontal
		{
			get
			{
				return ((m_iFlags & ZAP_FLAG_HORIZONTAL) != 0);
			}
			set
			{
				if(value == true)
					m_iFlags |= ZAP_FLAG_HORIZONTAL;
				else
					m_iFlags &= ~ZAP_FLAG_HORIZONTAL;
			}

		}

		#endregion Properties
	
	}// public class CTmaxZapFile

}// namespace FTI.Trialmax.Database
