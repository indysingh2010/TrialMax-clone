using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class encapsulates the options used to register media in a Trialmax database</summary>
	public class CTmaxVideoOptions
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				= "ApplicationOptions";
		private const string XMLINI_SHOW_ERROR_MESSAGES_KEY		= "ShowErrorMessages";
		private const string XMLINI_ENABLE_DIAGNOSTICS_KEY		= "EnableDiagnostics";
		private const string XMLINI_LOG_DIAGNOSTICS_KEY			= "LogDiagnostics";
		private const string XMLINI_LAST_DEPOSITION_KEY			= "LastDeposition";
		private const string XMLINI_LAST_SCRIPT_KEY				= "LastScript";
		private const string XMLINI_PAUSE_THRESHOLD_KEY			= "PauseThreshold";
		private const string XMLINI_LOAD_LAST_KEY				= "LoadLast";
		private const string XMLINI_LEFT_KEY					= "Left";
		private const string XMLINI_TOP_KEY						= "Top";
		private const string XMLINI_WIDTH_KEY					= "Width";
		private const string XMLINI_HEIGHT_KEY					= "Height";
		private const string XMLINI_MAXIMIZED_KEY				= "Maximized";
		private const string XMLINI_LAST_HIGHLIGHTER_KEY		= "LastHighlighter";
		private const string XMLINI_VIDEO_FOLDER_KEY			= "VideoFolder";
		private const string XMLINI_USERS_MANUAL_KEY			= "UsersManual";
		private const string XMLINI_CONTACTS_FILENAME_KEY		= "ContactsFilename";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to SearchOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxSearchOptions m_tmaxSearchOptions = new CTmaxSearchOptions();
		
		/// <summary>Local member bound to ImportOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxImportOptions m_tmaxImportOptions = new CTmaxImportOptions();
		
		/// <summary>Local member bound to ExportOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxExportOptions m_tmaxExportOptions = new CTmaxExportOptions();
		
		/// <summary>Local member bound to ShowErrorMessages property</summary>
		private bool m_bShowErrorMessages = true; 
		
		/// <summary>Local member bound to EnableDiagnostics property</summary>
		private bool m_bEnableDiagnostics = false; 
		
		/// <summary>Local member bound to LogDiagnostics property</summary>
		private bool m_bLogDiagnostics = false; 
		
		/// <summary>Local member bound to LoadLast property</summary>
		private bool m_bLoadLast = false; 
		
		/// <summary>Local member bound to RecentlyUsed property</summary>
		private ArrayList m_aRecentlyUsed = new ArrayList();
		
		/// <summary>Local member bound to LastHighlighter property</summary>
		private long m_lLastHighlighter = -1;
		
		/// <summary>Local member bound to MaxRecentlyUsed property</summary>
		private int m_iMaxRecentlyUsed = 5;
		
		/// <summary>Local member bound to Top property</summary>
		private int m_iTop = 0;
		
		/// <summary>Local member bound to Left property</summary>
		private int m_iLeft = 0;
		
		/// <summary>Local member bound to Width property</summary>
		private int m_iWidth = 0;
		
		/// <summary>Local member bound to Height property</summary>
		private int m_iHeight = 0;
		
		/// <summary>Local member bound to Maximized property</summary>
		private bool m_bMaximized = false;
		
		/// <summary>Fully qualified path to the last source deposition file</summary>
		private string m_strLastDeposition = "";
		
		/// <summary>Fully qualified path to the last active script</summary>
		private string m_strLastScript = "";
		
		/// <summary>Fully qualified path to the folder containing the video files</summary>
		private string m_strVideoFolder = "";
		
		/// <summary>Local member bound to UsersManual property</summary>
		private string m_strUsersManual = "TrialMax Video Viewer Manual.pdf";
		
		/// <summary>Local member bound to ContactsFilename property</summary>
		private string m_strContactsFilename = "_tmax_contact_information.rtf";
		
		/// <summary>Number of seconds used to determine when Pause Indicator gets displayed</summary>
		private double m_dPauseThreshold = 12.0;
		
		/// <summary>Private member bound to Highlighters property</summary>
		private CTmaxHighlighters m_tmaxHighlighters = new CTmaxHighlighters();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoOptions()
		{
		
		}// CTmaxVideoOptions()
		
		/// <summary>This method is called to add a case folder to the collection of recently used folders</summary>
		/// <param name="strSource">The path to the source file</param>
		/// <returns>true if successful</returns>
		public bool AddRecentlyUsed(string strSource)
		{
			string strRecent = null;
			
			try
			{
				//	See if this file already exists in the collection
				for(int i = 0; i < m_aRecentlyUsed.Count; i++)
				{
					if(String.Compare(m_aRecentlyUsed[i].ToString(), strSource, true) == 0)
					{
						//	Is it already at the top of the list?
						if(i == 0)
						{
							//	Nothing to do
							return true;
						}
						else
						{
							//	Remove from the collection 
							m_aRecentlyUsed.RemoveAt(i);
						}
					}
					
				}
				
				//	Do we need to make room?
				while(m_aRecentlyUsed.Count > m_iMaxRecentlyUsed)
					m_aRecentlyUsed.RemoveAt(m_aRecentlyUsed.Count - 1);
					
				strRecent = new string(strSource.ToCharArray());
				
				//	Insert at the top of the collection
				if(m_aRecentlyUsed.Count > 0)
					m_aRecentlyUsed.Insert(0, strRecent);
				else
					m_aRecentlyUsed.Add(strRecent);
					
				return true;
			
			}
			catch
			{
				return false;
			}
		
		}// public bool AddRecentlyUsed(string strSource)
		
		/// <summary>This method is called to remove a case folder from the collection of recently used folders</summary>
		/// <param name="strSource">The source file to be removed from the local collection</param>
		public void RemoveRecentlyUsed(string strSource)
		{
			try
			{
				//	See if this file exists in the collection
				for(int i = 0; i < m_aRecentlyUsed.Count; i++)
				{
					if(String.Compare(m_aRecentlyUsed[i].ToString(), strSource, true) == 0)
					{
						//	Remove from the collection
						m_aRecentlyUsed.RemoveAt(i);
						break;
					}
					
				}
				
			}
			catch
			{
			}
		
		}// public void RemoveRecentlyUsed(string strSource)
		
		/// <summary>This method is called to get the fully qualified path to the application's users manual</summary>
		/// <returns>The path to the user's manual</returns>
		public string GetUsersManualFileSpec()
		{
			string strFileSpec = "";
			
			if(m_strUsersManual.Length > 0)
			{
				//	Is this a fully qualified path?
				if(strFileSpec.IndexOfAny("\\:/".ToCharArray()) >= 0)
				{
					strFileSpec = m_strUsersManual;
				}
				else
				{
					strFileSpec = CTmaxToolbox.GetApplicationFolder();
					if(strFileSpec.EndsWith("\\") == false)
						strFileSpec += "\\";
					strFileSpec += m_strUsersManual;
				}
				
			}// if(m_strUsersManual.Length > 0)
			
			return strFileSpec;
			
		}// public string GetUsersManualFileSpec()
		
		/// <summary>This method is called to get the fully qualified path to the application's users manual</summary>
		/// <returns>The path to the user's manual</returns>
		public string GetContactsFileSpec()
		{
			string strFileSpec = "";
			
			if(m_strContactsFilename.Length > 0)
			{
				//	Is this a fully qualified path?
				if(strFileSpec.IndexOfAny("\\:/".ToCharArray()) >= 0)
				{
					strFileSpec = m_strContactsFilename;
				}
				else
				{
					strFileSpec = CTmaxToolbox.GetApplicationFolder();
					if(strFileSpec.EndsWith("\\") == false)
						strFileSpec += "\\";
					strFileSpec += m_strContactsFilename;
				}
				
			}// if(m_strContactsFilename.Length > 0)
			
			return strFileSpec;
			
		}// public string GetContactsFileSpec()
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
		public void Load(CXmlIni xmlIni)
		{
			string strKey;
			string strRecent;
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_bShowErrorMessages = xmlIni.ReadBool(XMLINI_SHOW_ERROR_MESSAGES_KEY, m_bShowErrorMessages);
			m_bEnableDiagnostics = xmlIni.ReadBool(XMLINI_ENABLE_DIAGNOSTICS_KEY, m_bEnableDiagnostics);
			m_bLogDiagnostics = xmlIni.ReadBool(XMLINI_LOG_DIAGNOSTICS_KEY, m_bLogDiagnostics);
			m_bLoadLast = xmlIni.ReadBool(XMLINI_LOAD_LAST_KEY, m_bLoadLast);
			m_strLastDeposition = xmlIni.Read(XMLINI_LAST_DEPOSITION_KEY, m_strLastDeposition);
			m_strLastScript = xmlIni.Read(XMLINI_LAST_SCRIPT_KEY, m_strLastScript);
			m_strVideoFolder = xmlIni.Read(XMLINI_VIDEO_FOLDER_KEY, m_strVideoFolder);
			m_dPauseThreshold = xmlIni.ReadDouble(XMLINI_PAUSE_THRESHOLD_KEY, m_dPauseThreshold);
			m_iLeft = xmlIni.ReadInteger(XMLINI_LEFT_KEY, m_iLeft);
			m_iTop = xmlIni.ReadInteger(XMLINI_TOP_KEY, m_iTop);
			m_iWidth = xmlIni.ReadInteger(XMLINI_WIDTH_KEY, m_iWidth);
			m_iHeight = xmlIni.ReadInteger(XMLINI_HEIGHT_KEY, m_iHeight);
			m_bMaximized = xmlIni.ReadBool(XMLINI_MAXIMIZED_KEY, m_bMaximized);
			m_lLastHighlighter = xmlIni.ReadLong(XMLINI_LAST_HIGHLIGHTER_KEY, m_lLastHighlighter);
			m_strUsersManual = xmlIni.Read(XMLINI_USERS_MANUAL_KEY, m_strUsersManual);
			m_strContactsFilename = xmlIni.Read(XMLINI_CONTACTS_FILENAME_KEY, m_strContactsFilename);
			
			//	Retrieve the recently used scripts / depositions
			for(int i = 1; i <= m_iMaxRecentlyUsed; i++)
			{
				strKey = ("RecentlyUsed" + i.ToString());
				strRecent = xmlIni.Read(strKey);
				
				if(strRecent != null && (strRecent.Length > 0))
				{
					//	Make sure the file still exists
					if(System.IO.File.Exists(strRecent))
						m_aRecentlyUsed.Add(new string(strRecent.ToCharArray()));
				}
			}
			
			//	Load the nested objects
			m_tmaxHighlighters.Load(xmlIni);
			m_tmaxSearchOptions.Load(xmlIni);
			m_tmaxImportOptions.Load(xmlIni);
			m_tmaxExportOptions.Load(xmlIni);
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the application option values</param>
		public void Save(CXmlIni xmlIni)
		{
			string strKey = "";

			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_SHOW_ERROR_MESSAGES_KEY, m_bShowErrorMessages);
			xmlIni.Write(XMLINI_ENABLE_DIAGNOSTICS_KEY, m_bEnableDiagnostics);
			xmlIni.Write(XMLINI_LOG_DIAGNOSTICS_KEY, m_bLogDiagnostics);
			xmlIni.Write(XMLINI_LOAD_LAST_KEY, m_bLoadLast);
			xmlIni.Write(XMLINI_LAST_DEPOSITION_KEY, m_strLastDeposition);
			xmlIni.Write(XMLINI_LAST_SCRIPT_KEY, m_strLastScript);
			xmlIni.Write(XMLINI_VIDEO_FOLDER_KEY, m_strVideoFolder);
			xmlIni.Write(XMLINI_PAUSE_THRESHOLD_KEY, m_dPauseThreshold);
			xmlIni.Write(XMLINI_USERS_MANUAL_KEY, m_strUsersManual);
			xmlIni.Write(XMLINI_CONTACTS_FILENAME_KEY, m_strContactsFilename);

			xmlIni.Write(XMLINI_LAST_HIGHLIGHTER_KEY, m_lLastHighlighter);
			xmlIni.Write(XMLINI_LEFT_KEY, m_iLeft);
			xmlIni.Write(XMLINI_TOP_KEY, m_iTop);
			xmlIni.Write(XMLINI_WIDTH_KEY, m_iWidth);
			xmlIni.Write(XMLINI_HEIGHT_KEY, m_iHeight);
			xmlIni.Write(XMLINI_MAXIMIZED_KEY, m_bMaximized);

			//	Write the recently used source files
			for(int i = 0; i < m_iMaxRecentlyUsed; i++)
			{
				strKey = ("RecentlyUsed" + (i + 1).ToString());
				
				if(i < m_aRecentlyUsed.Count)
					xmlIni.Write(strKey, m_aRecentlyUsed[i].ToString());
				else
					xmlIni.Write(strKey, "");
			}
		
			//	Save the nested objects
			m_tmaxHighlighters.Save(xmlIni);
			m_tmaxSearchOptions.Save(xmlIni);
			m_tmaxImportOptions.Save(xmlIni);
			m_tmaxExportOptions.Save(xmlIni);
			
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>Called to retrieve the path to the video file associated with the specified segment</summary>
		/// <param name="xmlScript">The XML script that owns the segment</param>
		/// <param name="xmlSegment">The desired XML segment</param>
		/// <param name="bConfirm">true to confirm it's existance</param>
		/// <param name="bSilent">true to inhibit warning messages</param>
		/// <returns>The path to the video file</returns>
		public string GetVideoFileSpec(CXmlScript xmlScript, CXmlSegment xmlSegment, bool bConfirm, bool bSilent)
		{
			string		strFileSpec = "";
			string		strFolder = "";
			
			Debug.Assert(xmlScript != null);
			if(xmlScript == null) return "";
			Debug.Assert(xmlSegment != null);
			if(xmlSegment == null) return "";
			
			//	Do we have a default video folder
			if(this.VideoFolder.Length > 0)
			{
				strFolder = this.VideoFolder;
			}
			else
			{
				//	Use the folder containing the script
				try
				{
					strFolder = System.IO.Path.GetDirectoryName(xmlScript.FileSpec);
				}
				catch
				{
				}
				
			}// if((m_tmaxAppOptions != null) && (m_tmaxAppOptions.VideoFolder.Length > 0))
			
			if((strFolder.Length > 0) && (strFolder.EndsWith("\\") == false))
				strFolder += "\\";
				
			//	Add the segment filename
			strFileSpec = strFolder + xmlSegment.Filename;
			
			//	Should we confirm?
			if(bConfirm == true)
			{
				if(System.IO.File.Exists(strFileSpec) == false)
				{
					if(bSilent == false)
						MessageBox.Show("Unable to locate " + strFileSpec, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						
					strFileSpec = "";
				}
				
			}// if(bConfirm == true)
			
			return strFileSpec;
			
		}// public string GetVideoFileSpec(CXmlScript xmlScript, CXmlSegment xmlSegment, bool bConfirm, bool bSilent)
		
		/// <summary>Called to retrieve the path to the video file associated with the specified designation</summary>
		/// <param name="xmlScript">The XML script that owns the designation</param>
		/// <param name="xmlDesignation">The desired XML designation</param>
		/// <param name="bConfirm">true to confirm it's existance</param>
		/// <param name="bSilent">true to inhibit warning messages</param>
		/// <returns>The path to the video file</returns>
		public string GetVideoFileSpec(CXmlScript xmlScript, CXmlDesignation xmlDesignation, bool bConfirm, bool bSilent)
		{
			CXmlSegment	xmlSegment = null;
			
			Debug.Assert(xmlScript != null);
			if(xmlScript == null) return "";
			Debug.Assert(xmlDesignation != null);
			if(xmlDesignation == null) return "";
			
			if((xmlSegment = xmlScript.GetSegment(xmlDesignation.Segment)) == null)
			{
				if(bSilent == false)
					MessageBox.Show("Unable to locate the video segment bound to the specified designation", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return "";
			}
			
			
			return GetVideoFileSpec(xmlScript, xmlSegment, bConfirm, bSilent);
			
		}// public string GetVideoFileSpec(CXmlScript xmlScript, CXmlDesignation xmlDesignation, bool bConfirm, bool bSilent)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>List of most recently used case folders</summary>
		public ArrayList RecentlyUsed
		{
			get { return m_aRecentlyUsed; }
		
		}
		
		/// <summary>Maximum number of recently used selections</summary>
		public int MaxRecentlyUsed
		{
			get 
			{ 
				return m_iMaxRecentlyUsed; 
			}
			set 
			{ 
				m_iMaxRecentlyUsed = value; 
				
				//	Do we need to remove folders from the collection
				if(m_aRecentlyUsed != null)
				{
					while(m_aRecentlyUsed.Count > m_iMaxRecentlyUsed)
						m_aRecentlyUsed.RemoveAt(m_aRecentlyUsed.Count - 1);
				}
				
			}
			
		}// MaxRecentlyUsed Property
		
		/// <summary>User's manual filename</summary>
		public string UsersManual
		{
			get { return m_strUsersManual; }
			set { m_strUsersManual = value; }
		}
		
		/// <summary>FTI contacts filename</summary>
		public string ContactsFilename
		{
			get { return m_strContactsFilename; }
			set { m_strContactsFilename = value; }
		}
		
		/// <summary>True to show error messages</summary>
		public bool ShowErrorMessages
		{
			get { return m_bShowErrorMessages; }
			set { m_bShowErrorMessages = value; }
		}

		/// <summary>True to enable diagnostic messages</summary>
		public bool EnableDiagnostics
		{
			get { return m_bEnableDiagnostics; }
			set { m_bEnableDiagnostics = value; }
		}

		/// <summary>True to log diagnostic messages</summary>
		public bool LogDiagnostics
		{
			get { return m_bLogDiagnostics; }
			set { m_bLogDiagnostics = value; }
		}

		/// <summary>True to load last file on startup</summary>
		public bool LoadLast
		{
			get { return m_bLoadLast; }
			set { m_bLoadLast = value; }
		}

		/// <summary>Path to the last deposition source file</summary>
		public string LastDeposition
		{
			get { return m_strLastDeposition; }
			set { m_strLastDeposition = value; }
		}

		/// <summary>Path to the last active script file</summary>
		public string LastScript
		{
			get { return m_strLastScript; }
			set { m_strLastScript = value; }
		}

		/// <summary>Path to the folder containing the video files</summary>
		public string VideoFolder
		{
			get { return m_strVideoFolder; }
			set { m_strVideoFolder = value; }
		}

		/// <summary>Last highlighter selected by the user</summary>
		public long LastHighlighter
		{
			get { return m_lLastHighlighter; }
			set { m_lLastHighlighter = value; }
		}

		/// <summary>Number of seconds used to determine when Pause Indicators should be displayed</summary>
		public double PauseThreshold
		{
			get { return m_dPauseThreshold; }
			set { m_dPauseThreshold = value; }
		}

		/// <summary>The collection of designation highlighters</summary>
		public CTmaxHighlighters Highlighters
		{
			get { return m_tmaxHighlighters; }
		}
		
		/// <summary>Left coordinate of application's main window position</summary>
		public int Left
		{
			get { return m_iLeft; }
			set { m_iLeft = value; }
		}
		
		/// <summary>Top coordinate of application's main window position</summary>
		public int Top
		{
			get { return m_iTop; }
			set { m_iTop = value; }
		}
		
		/// <summary>Width of application's main window position</summary>
		public int Width
		{
			get { return m_iWidth; }
			set { m_iWidth = value; }
		}
		
		/// <summary>Height of application's main window position</summary>
		public int Height
		{
			get { return m_iHeight; }
			set { m_iHeight = value; }
		}
		
		/// <summary>True if application should be maximized on startup</summary>
		public bool Maximized
		{
			get { return m_bMaximized; }
			set { m_bMaximized = value; }
		}
		
		/// <summary>Collection of options used for searching</summary>
		public CTmaxSearchOptions SearchOptions
		{
			get { return m_tmaxSearchOptions; }
		}
		
		/// <summary>Collection of options used for import operations</summary>
		public CTmaxImportOptions ImportOptions
		{
			get { return m_tmaxImportOptions; }
		}
		
		/// <summary>Collection of options used for export operations</summary>
		public CTmaxExportOptions ExportOptions
		{
			get { return m_tmaxExportOptions; }
		}
		
		#endregion Properties

	}//	public class CTmaxVideoOptions

}// namespace FTI.Trialmax.TMVV.Tmvideo
