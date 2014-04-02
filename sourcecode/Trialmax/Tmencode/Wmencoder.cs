using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

using WMEncoderLib;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Encode
{
	/// <summary>This class is used to as a wrapper for Windows Media Encoder services</summary>
	public class CWMEncoder
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME			= "MediaEncoderOptions";
		private const string XMLINI_LAST_PROFILE_KEY		= "LastProfile";
		private const string XMLINI_USE_PREFERRED_KEY		= "UsePreferred";
		private const string XMLINI_PROFILE_KEY_PREFIX		= "Profile";

		/// <summary>Error message identifiers</summary>
		private const int ERROR_FILL_PROFILES				= 0;
		private const int ERROR_INITIALIZE_SOURCE_GROUPS	= 1;
		private const int ERROR_NO_WINDOWS_MEDIA			= 2;
		private const int ERROR_INITIALIZE_EX				= 3;
		private const int ERROR_ADD_SOURCE_FAILED			= 4;
		private const int ERROR_ADD_SOURCE_EX				= 5;
		private const int ERROR_CREATE_STATUS_FORM_EX		= 6;
		private const int ERROR_ENCODE_EX					= 7;
		private const int ERROR_PREPARE_SOURCE_GROUPS_EX	= 9;
		private const int ERROR_START_EX					= 10;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member used to retrieve version information</summary>
		private FTI.Shared.CBaseVersion m_wmVersion = null;

		/// <summary>Local member bound to IWMEncoderApp property</summary>
		public WMEncoderLib.WMEncoderApp m_IWMEncoderApp = null;

		/// <summary>Local member bound to IWMEncoder property</summary>
		public WMEncoderLib.WMEncoder m_IWMEncoder = null;
		
		/// <summary>Local member bound to Profiles property</summary>
		private CWMProfiles m_aProfiles = null;
		
		/// <summary>Local member bound to SourceGroups property</summary>
		private CWMSourceGroups m_aSourceGroups = null;
		
		/// <summary>Local member to keep track of active source group</summary>
		private CWMSourceGroup m_sourceGroup = null;
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		/// <summary>Local member bound to WMProfile property</summary>
		private CWMProfile m_WMProfile = null;
		
		/// <summary>Local member bound to Status property</summary>
		private CFEncoderStatus m_wndStatus = null;
		
		/// <summary>Local member to store status of active source while encoding is taking place</summary>
		private string m_strSourceStatus = "";
		
		/// <summary>Flag to indicate that the operation has been cancelled</summary>
		private bool m_bCancelled = false;
		
		/// <summary>Total time required for the encoded file</summary>
		private double m_dTotalTime = 0;
		
		/// <summary>Local member bound to ShowCancel property</summary>
		private bool m_bShowCancel = true;

		/// <summary>Local member bound to Completed property</summary>
		private long m_lCompleted = 0;
		
		/// <summary>Elapsed time represented by all encodedd source groups</summary>
		private double m_dElapsedTime = 0;
		
		/// <summary>True to indicate that the source group information has changed</summary>
		private bool m_bSourceChanged = false;
		
		/// <summary>True to filter invalid profiles</summary>
		private bool m_bFilterProfiles = true;
		
		/// <summary>Local member bound to LastProfile property</summary>
		private string m_strLastProfile = "";
		
		/// <summary>Local member bound to UsePreferred property</summary>
		private bool m_bUsePreferred = true;
		
		/// <summary>Local member bound to PreferredProfiles property</summary>
		private ArrayList m_aPreferredProfiles = new ArrayList();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This delegate prototypes the callback for Win32 EnumWindows API call</summary>
		/// <param name="hwnd">The enumerated window handle</param>
		/// <param name="lParam">Caller provided data</param>
		public delegate bool EncoderStatusHandler(object sender, string strStatus);
		
		/// <summary>Fired by the control to set the active link</summary>
		public event EncoderStatusHandler EncoderStatusUpdate;		
	
		/// <summary>Constructor</summary>
		public CWMEncoder()
		{
			m_tmaxEventSource.Name = "WMEncoder Events";
			SetErrorStrings();		
		}
		
		/// <summary>Called to get the version descriptor</summary>
		/// <returns>The version descriptor if available</returns>
		public CBaseVersion GetVersion()
		{
			//EXTERN_GUID( LIBID_WMEncoderLib,            0x632B6060, 0xBBC6, 0x11D2, 0xA3, 0x29, 0x00, 0x60, 0x97, 0xC4, 0xE4, 0x76 );
			//EXTERN_GUID( DIID__IWMEncoderEvents,        0x632B6062, 0xBBC6, 0x11D2, 0xA3, 0x29, 0x00, 0x60, 0x97, 0xC4, 0xE4, 0x76 );
			//EXTERN_GUID( DIID__IWMEncoderAppEvents,     0x32B8ECC9, 0x2901, 0x11D3, 0x8F, 0xB8, 0x00, 0xC0, 0x4F, 0x61, 0x09, 0xB7 );
			//EXTERN_GUID( DIID__IWMEncBasicEditEvents,   0xAB5AF3CC, 0x9347, 0x4DC1, 0x92, 0xE3, 0xB9, 0x65, 0x37, 0xB8, 0xC4, 0x46 );
			//EXTERN_GUID( CLSID_WMEncoder,               0x632B606A, 0xBBC6, 0x11D2, 0xA3, 0x29, 0x00, 0x60, 0x97, 0xC4, 0xE4, 0x76 );

			//	Don't bother if already retrieved versions
			if(m_wmVersion != null) return m_wmVersion;
			
			try
			{
				System.Guid		guid = new System.Guid(0x632B606A, 0xBBC6, 0x11D2, 0xA3, 0x29, 0x00, 0x60, 0x97, 0xC4, 0xE4, 0x76);
				CTmaxRegistry	tmaxRegistry = new CTmaxRegistry();
				string			strFileSpec = "";
				
				//	Get the path to the registered encoder library
				tmaxRegistry = new CTmaxRegistry();
				strFileSpec = tmaxRegistry.GetRegisteredPath(guid);
				
				if((strFileSpec != null) && (strFileSpec.Length > 0))
				{
					//	Verify that the file exists
					if(System.IO.File.Exists(strFileSpec) == true)
					{
						//	Get the version information
						m_wmVersion = new CBaseVersion();
						m_wmVersion.InitFromFile(strFileSpec, true);
					}
					
				}// if((strFileSpec != null) && (strFileSpec.Length > 0))
						
			}
			catch
			{
			}
			
			return m_wmVersion;
			
		}// public CBaseVersion GetVersion()
		
		/// <summary>This method is called to initialize the object</summary>
		/// <param name="strFileSpec">Fully qualified path to the output file to be encoded</param>
		/// <returns>true if successful</returns>
		public bool Initialize(string strFileSpec)
		{
			bool bSuccessful = false;
			
			//	Clear the previous instance
			Clear();
			m_bCancelled = false;
			
			try
			{
				// Create a WMEncoder object.
				m_IWMEncoderApp = new WMEncoderApp();
				m_IWMEncoder = (WMEncoder)(m_IWMEncoderApp.Encoder);
  			
				//	Get the collection of available profiles
				if(FillProfiles() == false)
				{
					m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_FILL_PROFILES));
					return false;
				}
				
				//	Does the user want to encode a new file?
				if((strFileSpec != null) && (strFileSpec.Length > 0))
				{
					//	Set the name of the encoder's output file
					m_IWMEncoder.File.LocalFileName = strFileSpec;
					m_strFileSpec = strFileSpec;
					
					//	Initialize the source groups collection
					m_aSourceGroups = new CWMSourceGroups();
					
					if(m_aSourceGroups.Initialize(this) == false)
					{
						m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_SOURCE_GROUPS));
						return false;
					}
					
				}// if((strFileSpec != null) && (strFileSpec.Length > 0))
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				if(m_IWMEncoder == null)
					m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_NO_WINDOWS_MEDIA));
				else
					m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
				return false;
			}
			
			return bSuccessful;
			
		}// public bool Initialize(string strFileSpec)
		
		/// <summary>This method is called to set the profile for the operation</summary>
		/// <param name="strProfile">Registered name of the profile</param>
		/// <returns>true if successful</returns>
		public bool SetProfile(string strProfile)
		{
			CWMProfile profile = null;
			
			//	Must have profiles available
			if(m_aProfiles == null) return false;
			if(m_aProfiles.Count == 0) return false;
			
			try
			{
				//	Did the caller provide a valid name
				if((strProfile != null) && (strProfile.Length > 0))
					profile = m_aProfiles.Find(strProfile);
				else
					profile = GetDefaultProfile();
			}
			catch
			{
			}
			
			//	Did we find the profile?
			if(profile != null)
			{
				m_WMProfile = profile;
				return true;
			}
			else
			{
				return false;
			}
			
		}// public bool SetProfile(string strProfile)
		
		/// <summary>This method is called to get the default encoder profile</summary>
		/// <returns>The default profile</returns>
		public CWMProfile GetDefaultProfile()
		{
			CWMProfile profile = null;
			
			//	Must have profiles available
			if(m_aProfiles == null) return null;
			if(m_aProfiles.Count == 0) return null;
			
			try
			{
				//	Assume the default is the one with lowest device index
				foreach(CWMProfile O in m_aProfiles)
				{
					//	Lowest device index is assumed to be the default
					if((profile == null) || (profile.DeviceIndex > O.DeviceIndex))
						profile = O;
				}

			}
			catch
			{
			}
			
			return profile;
			
		}// public CWMProfile GetDefaultProfile(string strProfile)
		
		/// <summary>Called to add a new source descriptor to the encoder project</summary>
		/// <param name="strName">The unique name used to identify the group</param>
		/// <param name="strFileSpec">The fully qualified path to the source file</param>
		/// <param name="dStart">The start time in seconds</param>
		/// <param name="dStop">The stop time in seconds</param>
		/// <returns>true if successful</returns>
		public bool AddSource(string strName, string strFileSpec, double dStart, double dStop)
		{
			try
			{
				//	The encoder should have been initialized
				if(m_aSourceGroups == null) return false;

				//	Add to the collection
				if(m_aSourceGroups.Add(strName, strFileSpec, dStart, dStop) != null)
				{
					return true;
				}
				else
				{
					m_tmaxEventSource.FireError(this, "AddSource", m_tmaxErrorBuilder.Message(ERROR_ADD_SOURCE_FAILED, strFileSpec));
					return false;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSource", m_tmaxErrorBuilder.Message(ERROR_ADD_SOURCE_EX, strFileSpec), Ex);
				return false;
			}
			
		}// public bool AddSource(string strFileSpec, double dStart, double dStop)
		
		/// <summary>This method is called to clear the existing interfaces and reset the object</summary>
		public void Clear()
		{
			try
			{
				//	Destroy the status form
				if(m_wndStatus != null)
				{
					if(m_wndStatus.IsDisposed == false)
						m_wndStatus.Dispose();
					m_wndStatus = null;
				}
			
				if(m_aProfiles != null)
				{
					m_aProfiles.Clear();
					m_aProfiles = null;
				}
				
				if(m_aSourceGroups != null)
				{
					m_aSourceGroups.Clear();
					m_aSourceGroups = null;
				}
				
				if(m_IWMEncoder != null)
				{
					m_IWMEncoder.Reset();
					m_IWMEncoder = null;
				}
				
				m_WMProfile = null;
				m_bSourceChanged = false;
				m_strFileSpec = "";	
				m_strSourceStatus = "";	
				m_dTotalTime = 0;
				m_dElapsedTime = 0;	
				m_sourceGroup = null;	
				
				//	NOTE:	We intentionally do NOT reset m_bCancelled
				//			here. That way the owner can tell if the
				//			operation was cancelled when Encode() returns
			}
			catch
			{
			}
			
		}// public void Close()
		
		/// <summary>This method is called to cancel the encoding operation</summary>
		public void Cancel()
		{
			m_bCancelled = true;
			
		}// public void Cancel()
		
		/// <summary>This method is called to encode the specified source groups</summary>
		/// <param name="bCancelled">Flag to indicate if the operation was cancelled by the user</param>
		/// <returns>true if successful</returns>
		public bool Encode()
		{
			bool bSuccessful = false;
			
			//	Make sure we have all the required objects
			if(m_aProfiles == null) return false;
			if(m_aProfiles.Count == 0) return false;
			if(m_aSourceGroups == null) return false;
			if(m_aSourceGroups.Count == 0) return false;
			
			try
			{
				//	Make sure we have a profile for the job
				if(m_WMProfile == null)
					SetProfile(""); // Try for default
				if(m_WMProfile == null) return false;
				
				//	Store the total time required for the resultant file
				m_dTotalTime = m_aSourceGroups.GetTotalTime();
				m_tmaxEventSource.FireDiagnostic(this, "Encode", "Preparing to encode " + m_dTotalTime.ToString() + " seconds");
				
				//	Clear the counter
				m_lCompleted = 0;
				
				if(this.CreateStatusForm(m_IWMEncoder.File.LocalFileName) == false) 
					return false;
				
				//	Prepare the source groups
				if(PrepareSourceGroups() == true)
				{
					//	Start the operation
					if(Start() == true)
					{
						while(GetCancelled() == false)
						{
							//	Is the encoder finished?
							if(GetComplete() == true)
							{
								bSuccessful = (m_aSourceGroups.Count == m_lCompleted);
								break;
							}
							else
							{
								if((EncoderStatusUpdate != null) && (m_wndStatus != null))
								{
									if(EncoderStatusUpdate(this, m_wndStatus.Status) == false)
										Cancel();
								}

								Thread.Sleep(500);
							}
					
						}// while(GetCancelled() == false)
						
						//	Did the user cancel?
						if(GetCancelled() == true)
						{
							//	Make sure to stop the encoder
							try { m_IWMEncoder.Stop(); }
							catch {}
						}
					
					}// if(Start() == true)
					
				}// if(PrepareSourceGroups() == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Encode", m_tmaxErrorBuilder.Message(ERROR_ENCODE_EX, m_strFileSpec), Ex);
			}
			finally
			{
				Clear();
			}
			
			return bSuccessful;
			
		}// public bool Encode()
		
		/// <summary>This method is called to load the encoder options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the values</param>
		public void Load(CXmlIni xmlIni)
		{
			string		strKey = "";
			string		strProfile = "";
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
		
			m_strLastProfile = xmlIni.Read(XMLINI_LAST_PROFILE_KEY, m_strLastProfile);
			m_bUsePreferred  = xmlIni.ReadBool(XMLINI_USE_PREFERRED_KEY, m_bUsePreferred);
			
			//	Retrieve the preferred profiles
			for(int i = 1; i < 500; i++)
			{
				strKey = (XMLINI_PROFILE_KEY_PREFIX + i.ToString());
				strProfile = xmlIni.Read(strKey);
				
				if(strProfile != null && (strProfile.Length > 0))
				{
					m_aPreferredProfiles.Add(strProfile);
				}
				else
				{
					//	Out of preferred profiles
					break;
				}
				
			}// for(int i = 1; i < 500; i++)
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the application option values</param>
		public void Save(CXmlIni xmlIni)
		{
			string	strKey = "";
			int		i = 1;
		
			if(xmlIni.SetSection(XMLINI_SECTION_NAME,true,true) == false) return;

			xmlIni.Write(XMLINI_LAST_PROFILE_KEY, m_strLastProfile);
			xmlIni.Write(XMLINI_USE_PREFERRED_KEY, m_bUsePreferred);

			//	Store the name of each preferred profile in the XML file
			foreach(string O in m_aPreferredProfiles)
			{
				if(O.Length > 0)
				{
					strKey = (XMLINI_PROFILE_KEY_PREFIX + i.ToString());
					xmlIni.Write(strKey, O);
					i++;
				}
				
			}// foreach(string O in m_aPreferredProfiles)
		
		}// public void Save(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>StateChange event handler during an encoding operation</summary>
		/// <param name="eState">The new encoder state</param>
		public void OnStateChange(WMENC_ENCODER_STATE eState)
		{
			//MessageBox.Show(StateToString(eState));
		}

		/// <summary>SourceStateChange event handler during an encoding operation</summary>
		/// <param name="enumState">Enumerated source state identifier</param>
		/// <param name="enumType">Enumerated source type identifier</param>
		/// <param name="iIndex">Index of the source in the Sources collection</param>
		/// <param name="strSourceGroup">The source group name</param>
		private void OnSourceStateChange(WMENC_SOURCE_STATE eState, WMENC_SOURCE_TYPE eType, short iIndex, string strSourceGroup)
		{
			switch(eState)
			{
				case WMENC_SOURCE_STATE.WMENC_SOURCE_START:
					
					switch(eType)
					{
						//case WMENC_SOURCE_TYPE.WMENC_AUDIO:
						//case WMENC_SOURCE_TYPE.WMENC_SCRIPT:
						case WMENC_SOURCE_TYPE.WMENC_VIDEO:
							
							//	Were we processing a group?
							if(m_sourceGroup != null)
								m_dElapsedTime += (m_sourceGroup.Stop - m_sourceGroup.Start);
							
							m_strSourceStatus = String.Format("Encoding {0}", strSourceGroup);
							
							m_sourceGroup = m_aSourceGroups.Find(strSourceGroup);

							//	Request an update of the status form
							m_bSourceChanged = true;
							m_lCompleted++;
							
							if(m_sourceGroup != null)
								m_tmaxEventSource.FireDiagnostic(this, "OnSourceStateChange", "Source Changed: Name = " + m_sourceGroup.Name);
							else
								m_tmaxEventSource.FireDiagnostic(this, "OnSourceStateChange", "Source Changed: Name = NULL");
							
							break;
							
					}// switch(eType)
					break;
				
				case WMENC_SOURCE_STATE.WMENC_SOURCE_STOP:
					
					switch(eType)
					{
						case WMENC_SOURCE_TYPE.WMENC_AUDIO:
						case WMENC_SOURCE_TYPE.WMENC_VIDEO:
						case WMENC_SOURCE_TYPE.WMENC_SCRIPT:
						default:
							m_strSourceStatus = String.Format("Stopped {0}", strSourceGroup);
							break;
					}
					break;
				
				case WMENC_SOURCE_STATE.WMENC_SOURCE_PREPARE:
				case WMENC_SOURCE_STATE.WMENC_SOURCE_UNPREPARE:
					break;
			
			}//	switch(eState)

		}//	private void OnSourceStateChange(WMENC_SOURCE_STATE eState, WMENC_SOURCE_TYPE eType, short iIndex, string strSourceGroup)

		/// <summary>Converts the enumerated state identifier to a display string</summary>
		/// <param name="eState">the state identifier</param>
		/// <returns>the associated display string</returns>
		public string StateToString(WMENC_ENCODER_STATE eState)
		{
			switch(eState)
			{
				case WMENC_ENCODER_STATE.WMENC_ENCODER_RUNNING:
					return "Running";

				case WMENC_ENCODER_STATE.WMENC_ENCODER_STOPPED:
					return "Stopped";

				case WMENC_ENCODER_STATE.WMENC_ENCODER_STARTING:
					return "Starting";

				case WMENC_ENCODER_STATE.WMENC_ENCODER_PAUSING:
					return "Pausing";

				case WMENC_ENCODER_STATE.WMENC_ENCODER_STOPPING:
					return "Stopping";

				case WMENC_ENCODER_STATE.WMENC_ENCODER_PAUSED:
					return "Paused";

				case WMENC_ENCODER_STATE.WMENC_ENCODER_END_PREPROCESS:
					return "End Preprocessing";
			
				default:
					return "Unkown State";
				
			}// switch(eState)
			
		}// public string StateToString(WMENC_ENCODER_STATE eState)
		
		/// <summary>This method prepares the source groups to be encoded</summary>
		/// <returns>true if successful</returns>
		private bool PrepareSourceGroups()
		{
			CWMSourceGroup	prevGroup = null;
			bool			bSuccessful = false;
			
			try
			{
				SetStatus("Preparing source groups ...", 0);
				
				//	Prepare all the source groups
				foreach(CWMSourceGroup O in m_aSourceGroups)
				{
					m_tmaxEventSource.FireDiagnostic(this, "PrepareSourceGroups", "Preparing Group: " + O.Name + " [" + O.Start.ToString() + " -> " + O.Stop.ToString() + "]");

					//	Set the profile for this group
					O.SetProfile(m_WMProfile);
					
					//	Daisy-chain the groups together
					if(prevGroup != null)
						prevGroup.Cascade(O);
					
					prevGroup = O;

				}// foreach(CWMSourceGroup O in m_aSourceGroups)
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "PrepareSourceGroups", m_tmaxErrorBuilder.Message(ERROR_PREPARE_SOURCE_GROUPS_EX), Ex);
				m_tmaxEventSource.FireDiagnostic(this, "PrepareSourceGroups", "Ex: -> " + Ex.Message, Ex);
			}

			return bSuccessful;
			
		}// private bool PrepareSourceGroups()
		
		/// <summary>This method is called to create the status form for the operation</summary>
		/// <param name="strFileSpec">The path to the output file</param>
		/// <returns>true if successful</returns>
		private bool CreateStatusForm(string strFileSpec)
		{
			//	Clear the cancellation flag
			m_bCancelled = false;
			
			try
			{
				//	Make sure the previous instance is disposed
				if(m_wndStatus != null) 
				{
					if(m_wndStatus.IsDisposed == false)
						m_wndStatus.Dispose();
					m_wndStatus = null;
				}
				
				//	Create a new instance
				m_wndStatus = new CFEncoderStatus();
			
				m_wndStatus.ShowCancel = m_bShowCancel;
				
				//	Set the initial status message
				m_wndStatus.FileSpec = strFileSpec;
				SetStatus("Initializing encoder ...", 0);
				
				m_wndStatus.Show();
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateStatusForm", m_tmaxErrorBuilder.Message(ERROR_CREATE_STATUS_FORM_EX, strFileSpec), Ex);
				m_wndStatus = null;
			}
			
			return (m_wndStatus != null);
		
		}// private bool CreateStatusForm()
		
		/// <summary>This method is called to start the operation</summary>
		/// <returns>true if successful</returns>
		private bool Start()
		{
			bool bSuccessful = false;
			
			try
			{
				// Listen for state changes in the source. 
				m_tmaxEventSource.FireDiagnostic(this, "Start", "Connecting event handlers");
				//m_IWMEncoder.OnStateChange += new _IWMEncoderEvents_OnStateChangeEventHandler(OnStateChange);
				m_IWMEncoder.OnSourceStateChange += new _IWMEncoderEvents_OnSourceStateChangeEventHandler(OnSourceStateChange);
					
				SetStatus("Preparing encoder engine ...", 0);
				
				m_tmaxEventSource.FireDiagnostic(this, "Start", "Preparing encoder");
				m_IWMEncoder.PrepareToEncode(true);
				m_tmaxEventSource.FireDiagnostic(this, "Start", "Encoder prepared");
					
				//	Start the operation
				m_tmaxEventSource.FireDiagnostic(this, "Start", "Starting encoder");
				m_IWMEncoder.Start();
				m_tmaxEventSource.FireDiagnostic(this, "Start", "Encoder started");
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Start", m_tmaxErrorBuilder.Message(ERROR_START_EX), Ex);
				m_tmaxEventSource.FireDiagnostic(this, "Start", "Ex: -> " + Ex.Message, Ex);
			}
			
			return bSuccessful;
		
		}// private bool Start()
		
		/// <summary>This method is called to update the status text on the status form</summary>
		/// <param name="strStatus">The new status message</param>
		private void SetStatus(string strStatus, int iProgress)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					m_wndStatus.Status = strStatus;
					m_wndStatus.SetProgress(iProgress);
					m_wndStatus.Refresh();
				}
				
			}
			catch
			{
			}
			
		}// private void SetStatus(string strStatus)
		
		/// <summary>Called to check the flag to see if the user has cancelled</summary>
		/// <returns>true if cancelled</returns>
		private bool GetCancelled()
		{
			if(m_bCancelled == false)
			{
				Application.DoEvents();
				
				if(m_wndStatus != null)
					m_bCancelled = m_wndStatus.Cancelled;
			}
				
			return m_bCancelled;
		
		}// private bool GetCancelled()
		
		/// <summary>This method is called to determine if the operation is complete</summary>
		/// <returns>true if the operation is complete</returns>
		private bool GetComplete()
		{
			WMENC_ENCODER_STATE	eState = WMENC_ENCODER_STATE.WMENC_ENCODER_STOPPED;
			
			try
			{
				//	Has the encoder stopped?
				if((eState = m_IWMEncoder.RunState) == WMENC_ENCODER_STATE.WMENC_ENCODER_STOPPED)
				{
					m_tmaxEventSource.FireDiagnostic(this, "GetComplete", "Encoder STOPPED - Error Level = " + m_IWMEncoder.ErrorState.ToString());
					return true;
				}
							
				m_tmaxEventSource.FireDiagnostic(this, "GetComplete", "Encoder eState = " + eState.ToString());

				//	Update the status information
				if((m_wndStatus != null) && (GetCancelled() == false))
				{
					try 
					{ 
						//	Are we still waiting for source updates?
						if(m_strSourceStatus.Length == 0)
						{
							//	Use the current encoder state
							SetStatus(StateToString(eState), 0);
						}
						else if(m_bSourceChanged == true)
						{
							m_bSourceChanged = false;
									
							//	Update the status message
							if(m_dTotalTime > 0)
								SetStatus(m_strSourceStatus, (int)(m_dElapsedTime / m_dTotalTime * 100.0));
						}
						
					}
					catch(System.Exception Ex) 
					{
						m_tmaxEventSource.FireDiagnostic(this, "EncodeThreadProc", "Inner Ex: ", Ex);
					}
						
				}// if(m_wndStatus != null)
				
			}
			catch
			{
			}

			return false;
			
		}// private bool GetComplete()
		
		/// <summary>Called to populate the local encoder profiles collection</summary>
		/// <returns>true if successful</returns>
		private bool FillProfiles()
		{
			bool					bSuccessful = false;
			IWMEncProfileCollection IWMProfiles = null;
			
			//	We have to have the interface to the encoder
			Debug.Assert(m_IWMEncoder != null, "NO ENCODER OBJECT");
			if(m_IWMEncoder == null) return false;
			
			try
			{
				//	Get the interface to the profile collection
				if((IWMProfiles = m_IWMEncoder.ProfileCollection) == null)
					return false;
					
				//	Allocate a new collection
				m_aProfiles = new CWMProfiles();
				
				//	Fill the new collection
				m_aProfiles.Fill(IWMProfiles, m_bFilterProfiles);
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FillProfiles", "Ex:", Ex);
			}
			
			return bSuccessful;
			
		}// private bool FillProfiles()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve the registered encoder profiles");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to initialize the source groups for the encoder operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to initialize the encoder. Windows Media Encoder may not be installed.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the encoder operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An error was encountered while attempting to add the source descriptor for the encoder operation: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the source descriptor for the encoder operation: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the status form for the export operation: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to encode the specified file: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while preparing the source groups to be encoded.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while starting the encoder.");

		}// private void SetErrorStrings()

		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>Version descriptor for WM Encoder libraries</summary>
		public FTI.Shared.CBaseVersion WMVersion
		{
			get { return GetVersion(); }
		}
		
		/// <summary>The output file specification</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
		}

		/// <summary>The profile to be used for the operation</summary>
		public CWMProfile WMProfile
		{
			get { return m_WMProfile; }
		}

		/// <summary>COM interface to WMEncoderApp object</summary>
		public WMEncoderLib.WMEncoderApp IWMEncoderApp
		{
			get { return m_IWMEncoderApp; }
		}

		/// <summary>COM interface to WM Encoder object</summary>
		public WMEncoderLib.WMEncoder IWMEncoder
		{
			get { return m_IWMEncoder; }
		}
		
		/// <summary>The collection of registered encoder profiles</summary>
		public CWMProfiles Profiles
		{
			get { return m_aProfiles; }
		}
		
		/// <summary>The collection of source groups to be encoded</summary>
		public CWMSourceGroups SourceGroups
		{
			get { return m_aSourceGroups; }
		}
		
		/// <summary>True if encode operation cancelled by the user</summary>
		public bool Cancelled
		{
			get { return m_bCancelled; }
		}
		
		/// <summary>True to filter out invalid profiles</summary>
		public bool FilterProfiles
		{
			get { return m_bFilterProfiles; }
			set { m_bFilterProfiles = value; }
		}
		
		/// <summary>True to show the Cancel button on the status form</summary>
		public bool ShowCancel
		{
			get { return m_bShowCancel; }
			set { m_bShowCancel = value; }
		}
		
		/// <summary>Name of Windows Media profile used to encode a WMV file</summary>
		public string LastProfile
		{
			get { return m_strLastProfile; }
			set { m_strLastProfile = value; }
		}
		
		/// <summary>Collection of preferred profile names</summary>
		public ArrayList PreferredProfiles
		{
			get { return m_aPreferredProfiles; }
			set { m_aPreferredProfiles = value; }
		}
		
		/// <summary>True to use only preferred profiles</summary>
		public bool UsePreferred
		{
			get { return m_bUsePreferred; }
			set { m_bUsePreferred = value; }
		}
		
		/// <summary>The total number of completed source groups</summary>
		public long Completed
		{
			get { return m_lCompleted; }
		}
		
		#endregion Properties

	}// public class CWMEncoder
	
}// namespace FTI.Trialmax.Encoder
