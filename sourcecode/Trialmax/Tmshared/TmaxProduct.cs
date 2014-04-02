using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Xml;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
    public class KeyCollection
    {
        public string Key;
        public int Duration;
        public string DataBaseId;
    }
	/// <summary>This class manages information required to perform on-line updates</summary>
	public class CTmaxProductManager
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				= "ProductManager";
		private const string XMLINI_ACTIVATE_SITE_KEY			= "ActivateSite";
		private const string XMLINI_UPDATE_SITE_KEY				= "UpdateSite";
		private const string XMLINI_UPDATE_ALTERNATE_SITE_KEY	= "UpdateAlternateSite";
		private const string XMLINI_UPDATE_FILENAME_KEY			= "UpdateFilename";
		private const string XMLINI_UPDATE_USERNAME_KEY			= "UpdateUserName";
		private const string XMLINI_UPDATE_PASSWORD_KEY			= "UpdatePassword";
		private const string XMLINI_UPDATE_GROUP_ID_KEY			= "UpdateGroupId";
		private const string XMLINI_PROXY_SERVER_NAME_KEY		= "ProxyServerName";
		private const string XMLINI_PROXY_PORT_KEY				= "ProxyPort";
		private const string XMLINI_PROXY_USER_NAME_KEY			= "ProxyUserName";
		private const string XMLINI_PROXY_PASSWORD_KEY			= "ProxyPassword";
		private const string XMLINI_USE_IE_PROXY_KEY			= "UseIEProxy";
		private const string XMLINI_ONLINE_SITE_KEY				= "OnlineSite";
		private const string XMLINI_MASTER_ACTIVATION_CODE_KEY	= "MAC";
		private const string XMLINI_VERSIONS_FILESPEC_KEY		= "VersionsFileSpec";
		
		private const string XML_VERSIONS_DESCRIPTION			= "Description";
		private const string XML_VERSIONS_VERSION				= "Version";
		private const string XML_VERSIONS_FOLDER				= "Folder";
		private const string XML_VERSIONS_FILENAME				= "Filename";
		private const string XML_VERSIONS_ACTIVATION_CODE		= "Internal";
        private const string XML_VERSIONS_ACTIVATION_DATE       = "InternalLimit";

		private const int ERROR_GET_INSTALLED_EX				= 0;
		private const int ERROR_FILL_COMPONENTS_EX				= 1;
		private const int ERROR_VERIFY_EX						= 2;
		private const int ERROR_FTIP2I_NOT_FOUND				= 3;
		private const int ERROR_FTIOBREP_NOT_FOUND				= 4;

		private const string DEFAULT_UPDATE_SITE				= "http://tools.trialmax.com/";
		private const string DEFAULT_ALTERNATE_UPDATE_SITE		= "http://www.trialmax.com/";
		private const string DEFAULT_ACTIVATE_SITE				= "http://tools.trialmax.com/activate/default.aspx";
		private const string DEFAULT_ONLINE_SITE				= "http://www.trialmax.com";

        private const string DEFAULT_MASTER_ACTIVATION_CODE     = "053004";
        private const string MASTER_ACTIVATION_CODE_SECOND      = "053007";
        private const int    MASTER_ACTIVATION_CODE_SECOND_DURATION = 12;
	    
        private List<KeyCollection> _lKeyCollection = new List<KeyCollection>(); 
        KeyCollection keyCollection = new KeyCollection();
        
		private const string DEFAULT_VERSIONS_FILESPEC			= "_tmax_versions.xml";
		
		private const string MASTER_ACTIVATION_DB_ID			= "908";
        private const string MASTER_ACTIVATION_DB_ID_SECOND     = "909";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Components property</summary>
		private FTI.Shared.Trialmax.CTmaxComponents m_tmaxComponents = new CTmaxComponents();
		
		/// <summary>Local member bound to Registry property</summary>
		private FTI.Shared.Trialmax.CTmaxRegistry m_tmaxRegistry = null;

		/// <summary>Local member bound to XmlVersions property</summary>
		private FTI.Shared.Xml.CXmlIni m_xmlVersions = null;

		/// <summary>Local member bound to EventSource property</summary>
		protected CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		protected CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to OnlineSite property</summary>
		private string m_strOnlineSite = DEFAULT_ONLINE_SITE;
		
		/// <summary>Local member bound to ActivateSite property</summary>
		private string m_strActivateSite = DEFAULT_ACTIVATE_SITE;
		
		/// <summary>Local member bound to UpdateSite property</summary>
		private string m_strUpdateSite = DEFAULT_UPDATE_SITE;
		
		/// <summary>Local member bound to UpdateAlternateSite property</summary>
		private string m_strUpdateAlternateSite = DEFAULT_ALTERNATE_UPDATE_SITE;
		
		/// <summary>Local member bound to UpdateUserName property</summary>
		private string m_strUpdateUserName = "tm7up";
		
		/// <summary>Local member bound to UpdatePassword property</summary>
        private string m_strUpdatePassword = "$$UpTM#";
		
		/// <summary>Local member bound to UpdateGroupId property</summary>
		private string m_strUpdateGroupId = "";
		
		/// <summary>Local member bound to UpdateFilename property</summary>
		private string m_strUpdateFilename = "update7/files/_tmax_updates.xml";

		/// <summary>Local member bound to VersionsFileSpec property</summary>
		private string m_strVersionsFileSpec = DEFAULT_VERSIONS_FILESPEC;

		/// <summary>Local member for displaying non-modal error message</summary>
		private string m_strErrorMessage = "";
		
		/// <summary>Local member bound to ProxyServerName property</summary>
		private string m_strProxyServerName = "";
		
		/// <summary>Local member bound to ProxyUserName property</summary>
		private string m_strProxyUserName = "";
		
		/// <summary>Local member bound to ProxyPassword property</summary>
		private string m_strProxyPassword = "";
		
		/// <summary>Local member bound to ProxyPort property</summary>
		private int m_iProxyPort = 80;

		/// <summary>Local member bound to UseIEProxy property</summary>
		private bool m_bUseIEProxy = true;

        /// <summary>Local member bound to DaysAllowed property</summary>
        private int m_intDaysAllowed = 90;
		
		/// <summary>Local member bound to Activated property</summary>
		private object m_bActivated = null;
		
		/// <summary>Local member bound to HostApplication property</summary>
		private TmaxComponents m_eHostApplication = TmaxComponents.TrialMax;

		/// <summary>Local member bound to MasterActivationCode property</summary>
		private string m_strMasterActivationCode = DEFAULT_MASTER_ACTIVATION_CODE;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxProductManager()
		{
			//	Populate the error builder collection
			SetErrorStrings();

			//	Initialize the event source
			m_tmaxEventSource.Name = "Product Manager Events";
			m_tmaxEventSource.Attach(m_tmaxComponents.EventSource);

		    LoadMasterKeys();



		}
		/// <summary>
		/// Load key collection list 
		/// </summary>
        public void LoadMasterKeys()
		{
            KeyCollection keyCollection = new KeyCollection();
            keyCollection.Key = DEFAULT_MASTER_ACTIVATION_CODE;
            keyCollection.Duration = 0;
		    keyCollection.DataBaseId = MASTER_ACTIVATION_DB_ID;

            _lKeyCollection.Add(keyCollection);

            keyCollection = new KeyCollection();
            keyCollection.Key = MASTER_ACTIVATION_CODE_SECOND;
            keyCollection.Duration = MASTER_ACTIVATION_CODE_SECOND_DURATION;
            keyCollection.DataBaseId = MASTER_ACTIVATION_DB_ID_SECOND;
            
            _lKeyCollection.Add(keyCollection);
		}
		
		/// <summary>This method is called to determine if the product has been activated</summary>
		///	<param name="bVerify">True to verify that the registry value is valid</param>
		/// <returns>True if activated</returns>
		public bool GetActivated()
		{
			//	Have we checked the registry yet?
			if(m_bActivated == null)
			{
                if (m_eHostApplication == TmaxComponents.VideoViewer)
                    m_bActivated = true;
                else
                {
                    m_bActivated = VerifyActivationCode(GetActivationCode(true));
                }
			}
				
			Debug.Assert(m_bActivated != null);
			
			return (bool)m_bActivated;
			
		}// public bool GetActivated()
		
		/// <summary>This method is called to get the seed used to construct the product activation code</summary>
		/// <returns>The seed used to construct the code</returns>
		public string GetActivationSeed()
		{
			return System.Environment.MachineName;
		}
		
		/// <summary>This method is called to get the fully qualified link to the activation site</summary>
		/// <returns>The parameterized link to get the activation code</returns>
		public string GetActivationLink()
		{
			string	strApplication = this.Name;
			string	strVersion = this.ShortVersion;
			string	strMachine = this.GetActivationSeed();
			string	strLink = "";
			
			strApplication = System.Web.HttpUtility.UrlEncode(strApplication);
			strVersion = System.Web.HttpUtility.UrlEncode(strVersion);
			strMachine = System.Web.HttpUtility.UrlEncode(strMachine);
		
			strLink = this.ActivateSite;
			strLink += "?MN=" + strMachine;
			strLink += "&AN=" + strApplication;
			strLink += "&AR=" + strVersion;
			
			return strLink;
			
		}// public string GetActivationLink()
				
		/// <summary>This method is called to get the product activation code</summary>
		///	<param name="bRegistered">True to get the registered key, False to get the computed key</param>
		/// <returns>The key used to activate the local machine</returns>
		public string GetActivationCode(bool bRegistered)
		{
			CTmaxComponent	tmaxComponent = null;
			string			strCode = "";
			
			//	Does the caller want the registered code?
			if(bRegistered == true)
			{
				if((tmaxComponent = m_tmaxComponents.Find(TmaxComponents.TrialMax)) != null)
				{
					strCode = tmaxComponent.ActivationCode;
				}
				
				//	Attempt to retrieve from the registry if not in the versions file. Prior
				//	to version 7.0.0, activation codes were stored in the system registry
				if(strCode.Length == 0)
				{
					strCode = m_tmaxRegistry.GetActivationCode();
					
					if((strCode.Length > 0) && (tmaxComponent != null))
					{
						//	Store the actual value since we are no longer using the registry
						tmaxComponent.ActivationCode = CreateActivationCode(MASTER_ACTIVATION_DB_ID);
						Save(tmaxComponent, m_xmlVersions);
					}

				}// if(strCode.Length == 0)
				
				return strCode;
			}
			else
			{
				//	Return the actual code that should be used for this machine
				return CreateActivationCode(null);
			}

		}// public string GetActivationCode(bool bRegistered)

        /// <summary>This method is called to get the product activation code</summary>
        /// <returns>The key used to activate the local machine</returns>
        public bool IsDefaultActivationCode(string activationCode)
        {
            
            foreach(KeyCollection keyCollection in _lKeyCollection)
            {
                if(activationCode == CreateActivationCode(keyCollection.DataBaseId))
                {
                    MasterKeyDefaultDuration = keyCollection.Duration;
                    return true;
                }
            }
            return false;
		}// public string GetDefaultActivationCode()


        public bool IsActivationExpired()
        {
            CTmaxComponent tmaxComponent = null;
            DateTime datetimeCheck = new DateTime();
            bool IsActivated = false;
            
                if ((tmaxComponent = m_tmaxComponents.Find(TmaxComponents.TrialMax)) != null)
                {
                    if ((tmaxComponent.ActivationDate != "" && DateTime.TryParse(tmaxComponent.ActivationDate, out datetimeCheck) && datetimeCheck.AddDays(DaysAllowed) >= DateTime.Now))
                    {
                        IsActivated = true;
                    }
                }

               
                

                return IsActivated;
                     
        }// public string GetActivationCode(bool bRegistered)
        /// <summary>
        /// To check whether key passed in the parameter is valid master key 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
	    public bool IsValidMasterKey(string strKey)
        {
            foreach (KeyCollection kc in _lKeyCollection)
            {
                if ((strKey.Length > 0) && (strKey == kc.Key))
                {
                    MasterKeyDefaultDuration = kc.Duration;
                    return true; // Use the master code to activate
                }
			 }
			return false;
        }

	    /// <summary>This method is called to make sure the specified activation code is valid</summary>
		///	<param name="strCode">The code to be stored in the registry</param>
		/// <returns>True if the code is valid</returns>
        public bool VerifyActivationCode(string strActivation)
		{
		    string strCompare = "";
		    string strCode = "";

		    //	The caller has to provide a code
		    if (strActivation == null) return false;
		    if (strActivation.Length <= 3) return false;

		    //	Is this the master code?
		    if (IsValidMasterKey(strActivation))
		        return true; // Use the master code to activate

		    //	Strip the last three characters. These should be the database id
		    strCode = strActivation.Substring(0, strActivation.Length - 3);

		    //	Get the true code for comparison
		    strCompare = CreateActivationCode(null);

		    //	Returned by activation site: 9530892 for MARS
		    m_tmaxEventSource.FireDiagnostic(this, "VerifyActivationCode",
		                                     String.Format("User Supplied: {0}  System Generated: {1}  Seed: {2}",
		                                                   strActivation, strCompare, GetActivationSeed()));

		    //	Do a case sensitive comparison
		    return (String.Compare(strCompare, strCode, false) == 0);

		} // public bool VerifyActivationCode(string strCode)
		
		/// <summary>This method is called to set the product activation code</summary>
		///	<param name="strCode">The code to be stored in the registry</param>
		///	<param name="bVerify">True to test the code before storage</param>
		/// <returns>True if successful</returns>
		public bool SetActivationCode(string strCode, bool bVerify)
		{
			CTmaxComponent	tmaxComponent = null;
			bool			bSuccessful = false;

			//	Should we verify the code first?
			if((strCode != null) && (strCode.Length > 0) && (bVerify == true))
			{
				if(VerifyActivationCode(strCode) == false)
				{
					return false;
				}
			}
			
			//	Force a refresh of the Activated property
			m_bActivated = null;

			if((tmaxComponent = m_tmaxComponents.Find(TmaxComponents.TrialMax)) != null)
			{
				// Make sure we store the hashed value
				if(IsValidMasterKey(strCode))
				{
                    strCode = CreateActivationCode(GetMasterActivationDBId(strCode));
				}
				
				tmaxComponent.ActivationCode = strCode;
                tmaxComponent.ActivationDate = DateTime.Now.ToString();
			    bSuccessful = Save(tmaxComponent, m_xmlVersions);
			}

			return bSuccessful;

		}
        /// <summary>
        /// Get Master Activation DB Id 
        /// </summary>
        /// <param name="strCode">Parameter strCode to compare it with the key collection</param>
        /// <returns></returns>
        private string GetMasterActivationDBId(string strCode)
        {
            foreach(KeyCollection keyCollection in _lKeyCollection)
            {
                if (strCode == keyCollection.Key)
                {
                    return keyCollection.DataBaseId;
                }
            }
            return MASTER_ACTIVATION_DB_ID;
        }// public bool SetActivationCode(string strCode, bool bVerify)
		
		/// <summary>This method is called to get the default port for the proxy server</summary>
		/// <returns>the default proxy port (80)</returns>
		public int GetDefaultProxyPort()
		{
			return 80;
		}
		
		/// <summary>This method is called to get the default product name</summary>
		/// <returns>the product name</returns>
		public string GetProductName()
		{
			return "TrialMax";
		}
		
		/// <summary>This method is called to get the product version identifier</summary>
		/// <param name="bShortVer">true to return the 3-part short version</param>
		/// <returns>the product version</returns>
		public string GetProductVersion(bool bShortVer)
		{
			try
			{
				CTmsharedVersion ver = new CTmsharedVersion();
				
				if(bShortVer == true)
					return ver.ShortVersion;
				else
					return ver.Version;
					
			}
			catch
			{
				return "";
			}
			
		}// public string GetProductVersion(bool bShortVer)
		
		/// <summary>This method is called to load the update settings from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the values</param>
		public void Load(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_strActivateSite = xmlIni.Read(XMLINI_ACTIVATE_SITE_KEY, DEFAULT_ACTIVATE_SITE);
			m_strOnlineSite = xmlIni.Read(XMLINI_ONLINE_SITE_KEY, DEFAULT_ONLINE_SITE);
			m_strUpdateSite = xmlIni.Read(XMLINI_UPDATE_SITE_KEY, DEFAULT_UPDATE_SITE);
			m_strUpdateAlternateSite = xmlIni.Read(XMLINI_UPDATE_ALTERNATE_SITE_KEY, DEFAULT_ALTERNATE_UPDATE_SITE);
			m_strUpdateFilename = xmlIni.Read(XMLINI_UPDATE_FILENAME_KEY, m_strUpdateFilename);
			m_strUpdateUserName = xmlIni.Read(XMLINI_UPDATE_USERNAME_KEY, m_strUpdateUserName);
			m_strUpdatePassword = xmlIni.Read(XMLINI_UPDATE_PASSWORD_KEY, m_strUpdatePassword);
			m_strUpdateGroupId = xmlIni.Read(XMLINI_UPDATE_GROUP_ID_KEY, m_strUpdateGroupId);
			m_strProxyServerName = xmlIni.Read(XMLINI_PROXY_SERVER_NAME_KEY, m_strProxyServerName);
			m_strProxyUserName = xmlIni.Read(XMLINI_PROXY_USER_NAME_KEY, m_strProxyUserName);
			m_strProxyPassword = xmlIni.Read(XMLINI_PROXY_PASSWORD_KEY, m_strProxyPassword);
			m_iProxyPort = xmlIni.ReadInteger(XMLINI_PROXY_PORT_KEY, m_iProxyPort);
			m_bUseIEProxy = xmlIni.ReadBool(XMLINI_USE_IE_PROXY_KEY, m_bUseIEProxy);
			m_strMasterActivationCode = xmlIni.Read(XMLINI_MASTER_ACTIVATION_CODE_KEY, DEFAULT_MASTER_ACTIVATION_CODE);
			m_strVersionsFileSpec = xmlIni.Read(XMLINI_VERSIONS_FILESPEC_KEY, m_strVersionsFileSpec);
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the update settings in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the current values in</param>
		public void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_ACTIVATE_SITE_KEY, m_strActivateSite);
			xmlIni.Write(XMLINI_ONLINE_SITE_KEY, m_strOnlineSite);
			xmlIni.Write(XMLINI_UPDATE_SITE_KEY, m_strUpdateSite);
			xmlIni.Write(XMLINI_UPDATE_ALTERNATE_SITE_KEY, m_strUpdateAlternateSite);
			xmlIni.Write(XMLINI_UPDATE_FILENAME_KEY, m_strUpdateFilename);
			//xmlIni.Write(XMLINI_UPDATE_USERNAME_KEY, m_strUpdateUserName);
			//xmlIni.Write(XMLINI_UPDATE_PASSWORD_KEY, m_strUpdatePassword);
			xmlIni.Write(XMLINI_UPDATE_GROUP_ID_KEY, m_strUpdateGroupId);
			xmlIni.Write(XMLINI_PROXY_SERVER_NAME_KEY, m_strProxyServerName);
			xmlIni.Write(XMLINI_PROXY_USER_NAME_KEY, m_strProxyUserName);
			xmlIni.Write(XMLINI_PROXY_PASSWORD_KEY, m_strProxyPassword);
			xmlIni.Write(XMLINI_PROXY_PORT_KEY, m_iProxyPort);
			xmlIni.Write(XMLINI_USE_IE_PROXY_KEY, m_bUseIEProxy);
			
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to get the component descriptor for the specified component using the installation information</summary>
		/// <param name="eComponent">The enumerated component identifier</param>
		/// <returns>The component descriptor if it's installed</returns>
		public CTmaxComponent GetInstalled(FTI.Shared.Trialmax.TmaxComponents eComponent)
		{
			CBaseVersion	baseVer = null;
			CTmaxComponent	tmaxComponent = null;
			long			lVerPacked = 0;
			string			strFileSpec = "";
			
			try
			{
				//	Initialize a new component
				tmaxComponent = new CTmaxComponent();
				tmaxComponent.Name = eComponent.ToString();
				
				//	Which component are we looking for?
				switch(eComponent)
				{
					case TmaxComponents.TrialMax:
					case TmaxComponents.VideoViewer:
					
						//	Is this component the active application?
						if(m_eHostApplication == eComponent)
						{
							tmaxComponent.Folder = CTmaxToolbox.GetApplicationFolder();
						}
						else
						{
							//	Try to get the registered path
							if(m_tmaxRegistry.GetComponent(tmaxComponent) == true)
							{
								//	Make sure the file exists
								if(System.IO.File.Exists(tmaxComponent.FileSpec) == false)
								{
									//	Override with the application folder
									tmaxComponent.Folder = CTmaxToolbox.GetApplicationFolder();
								}
								
							}
							
						}// if(m_eHostApplication == TmaxComponents.VideoViewer)
						
						//	Does the file exist?
						if(System.IO.File.Exists(tmaxComponent.FileSpec) == true)
						{
							baseVer = new CBaseVersion();
							baseVer.InitFromFile(tmaxComponent.FileSpec, true);
							tmaxComponent.Version = baseVer.Version;
						}
						else
						{
							//	Unable to locate
							tmaxComponent = null;
						}

						break;
						
					case TmaxComponents.FTIP2I:
					
						tmaxComponent.Folder = m_tmaxRegistry.GetKeyValue("Software\\FTIP2I", "");
						
						//	Is this component registered?
						if(tmaxComponent.Folder.Length == 0)
						{
							tmaxComponent = null;
							break;
						}
						
						//	Make sure the executable exists
						if(System.IO.File.Exists(tmaxComponent.GetFileSpec()) == false)
						{
							//	Display an error message
							ShowError(m_tmaxErrorBuilder.Message(ERROR_FTIP2I_NOT_FOUND, tmaxComponent.GetFileSpec()));
							
							//	Force the user to reinstall
							try
							{
								m_tmaxRegistry.RemoveSubKey("Software", "FTIP2I");
							}
							catch
							{
							}
							
							tmaxComponent = null;
							break;
						}
						
						try
						{
							//	Extract the version information from the executable file
							FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(tmaxComponent.FileSpec);
							tmaxComponent.Version = fvi.FileVersion;
									
							//	Set this to version one if this is the original release
							//	before we started version stamping
							if((tmaxComponent.Version.Length == 0) || (tmaxComponent.Version == "5,6,1,626"))
								tmaxComponent.Version = "1.0.11.0";
							else if((lVerPacked = CBaseVersion.GetPackedVersion(tmaxComponent.Version)) < 1011)
								tmaxComponent.Version = "1.0.11.0";
							
						}
						catch
						{
							tmaxComponent = null;
						}
						break;

					case TmaxComponents.FTIORE:

						tmaxComponent.Folder = m_tmaxRegistry.GetKeyValue("Software\\FTIORE", "");

						//	Is this component registered?
						if(tmaxComponent.Folder.Length == 0)
						{
							tmaxComponent = null;
							break;
						}

						//	Make sure the executable exists
						if(System.IO.File.Exists(tmaxComponent.GetFileSpec()) == false)
						{
							//	Display an error message
							ShowError(m_tmaxErrorBuilder.Message(ERROR_FTIOBREP_NOT_FOUND, tmaxComponent.GetFileSpec()));

							//	Force the user to reinstall
							try
							{
								m_tmaxRegistry.RemoveSubKey("Software", "FTIOBREP");
							}
							catch
							{
							}

							tmaxComponent = null;
							break;
						}

						try
						{
							//	Extract the version information from the executable file
							FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(tmaxComponent.FileSpec);
							if((fvi != null) && (fvi.FileVersion != null))
								tmaxComponent.Version = fvi.FileVersion;

							if(tmaxComponent.Version.Length == 0)
								tmaxComponent.Version = "1.0.0.0";
							else if((lVerPacked = CBaseVersion.GetPackedVersion(tmaxComponent.Version)) < 100)
								tmaxComponent.Version = "1.0.0.0";

						}
						catch
						{
							tmaxComponent = null;
						}
						break;

					case TmaxComponents.WMEncoder:
						
						//EXTERN_GUID( LIBID_WMEncoderLib,            0x632B6060, 0xBBC6, 0x11D2, 0xA3, 0x29, 0x00, 0x60, 0x97, 0xC4, 0xE4, 0x76 );
						//EXTERN_GUID( DIID__IWMEncoderEvents,        0x632B6062, 0xBBC6, 0x11D2, 0xA3, 0x29, 0x00, 0x60, 0x97, 0xC4, 0xE4, 0x76 );
						//EXTERN_GUID( DIID__IWMEncoderAppEvents,     0x32B8ECC9, 0x2901, 0x11D3, 0x8F, 0xB8, 0x00, 0xC0, 0x4F, 0x61, 0x09, 0xB7 );
						//EXTERN_GUID( DIID__IWMEncBasicEditEvents,   0xAB5AF3CC, 0x9347, 0x4DC1, 0x92, 0xE3, 0xB9, 0x65, 0x37, 0xB8, 0xC4, 0x46 );
						//EXTERN_GUID( CLSID_WMEncoder,               0x632B606A, 0xBBC6, 0x11D2, 0xA3, 0x29, 0x00, 0x60, 0x97, 0xC4, 0xE4, 0x76 );

						System.Guid guid = new System.Guid(0x632B606A, 0xBBC6, 0x11D2, 0xA3, 0x29, 0x00, 0x60, 0x97, 0xC4, 0xE4, 0x76);

						//	Get the path to the encoder engine
						strFileSpec = m_tmaxRegistry.GetRegisteredPath(guid);
						
						//	Is this component registered?
						if((strFileSpec == null) || (strFileSpec.Length == 0))
						{
							tmaxComponent = null;
							break;
						}
						
						//	Make sure the executable exists
						if(System.IO.File.Exists(strFileSpec) == false)
						{
							//	Force the user to reinstall
							try { m_tmaxRegistry.RemoveSubKey("Software", "WMEncoder"); }
							catch { }
							tmaxComponent = null;
							break;
						}
						
						tmaxComponent.Filename = System.IO.Path.GetFileName(strFileSpec);
						tmaxComponent.Folder = System.IO.Path.GetDirectoryName(strFileSpec);
						
						try
						{
							//	Extract the version information from the executable file
							FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(tmaxComponent.FileSpec);
							tmaxComponent.Version = fvi.FileVersion;
						}
						catch
						{
							tmaxComponent = null;
						}
						break;
						
					default:
					
						tmaxComponent = null;
						break;
						
				}// switch(eComponent)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetInstalled", m_tmaxErrorBuilder.Message(ERROR_GET_INSTALLED_EX, eComponent), Ex);
				tmaxComponent = null;
			}
			
			return tmaxComponent;
			
		}// public CTmaxComponent GetInstalled(FTI.Shared.Trialmax.TmaxComponents eComponent)
		
		/// <summary>This method is called to populate the components collection</summary>
		/// <returns>true if successful</returns>
		public bool FillComponents()
		{
			bool			bSuccessful = false;
			string			strAppFolder = "";
			CTmaxComponent	tmaxComponent = null;

			Debug.Assert(m_tmaxComponents != null);

			try
			{
				//	Clear the existing components
				m_tmaxComponents.Clear();

				//	Construct the full path to the file containing the version descriptions
				if ((m_strVersionsFileSpec.Contains(":") == false) &&
				   (m_strVersionsFileSpec.StartsWith("\\\\") == false) &&
				   (m_strVersionsFileSpec.StartsWith("//") == false))
				{
					strAppFolder = CTmaxToolbox.GetApplicationFolder();
					if (strAppFolder.EndsWith("\\") == false)
						strAppFolder += "\\";

					if ((m_strVersionsFileSpec.StartsWith("\\") == true) ||
					   (m_strVersionsFileSpec.StartsWith("/") == true))
					{
						m_strVersionsFileSpec = m_strVersionsFileSpec.Substring(1);
					}
					else if (m_strVersionsFileSpec.Length == 0)
					{
						m_strVersionsFileSpec = DEFAULT_VERSIONS_FILESPEC;
					}

					m_strVersionsFileSpec = strAppFolder + m_strVersionsFileSpec;
				}

				m_xmlVersions = new CXmlIni();
				m_xmlVersions.Open(m_strVersionsFileSpec, "");

				//	Add a component for each section in the file
				if((m_xmlVersions.XMLSections != null) && (m_xmlVersions.XMLSections.ChildNodes != null))
				{
					foreach (XmlNode O in m_xmlVersions.XMLSections.ChildNodes)
					{
						tmaxComponent = new CTmaxComponent();
						tmaxComponent.Name = m_xmlVersions.GetSectionName(O);
						m_tmaxComponents.Add(tmaxComponent);
						Load(tmaxComponent, m_xmlVersions);
					}					
				}

				//	Check the known components to make sure they have the correct information
				Verify(TmaxComponents.TrialMax, false);
				Verify(TmaxComponents.VideoViewer, false);
				Verify(TmaxComponents.FTIP2I, true);
				Verify(TmaxComponents.FTIORE, true);
				Verify(TmaxComponents.WMEncoder, true);
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillComponents", m_tmaxErrorBuilder.Message(ERROR_FILL_COMPONENTS_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool FillComponents()
		
		/// <summary>This method is called to get the address of the update site</summary>
		/// <param name="bAlternate">true for alternate site</param>
		/// <returns>the address of the requested site</returns>
		public string GetUpdateSite(bool bAlternate)
		{
			if(bAlternate == true)
			{
				if(m_strUpdateAlternateSite.Length == 0)
					m_strUpdateAlternateSite = DEFAULT_ALTERNATE_UPDATE_SITE;
				return m_strUpdateAlternateSite;
			}
			else
			{
				if(m_strUpdateSite.Length == 0)
					m_strUpdateSite = DEFAULT_UPDATE_SITE;
				return m_strUpdateSite;
			}
			
		}// public string GetUpdateSite(bool bAlternate)
		
		/// <summary>This method is called to set the address of the update site</summary>
		/// <param name="strAddress">the new site address</param>
		/// <param name="bAlternate">true for alternate site</param>
		public void SetUpdateSite(string strAddress, bool bAlternate)
		{
			if(bAlternate == true)
			{
				if((strAddress == null) || (strAddress.Length == 0))
					m_strUpdateAlternateSite = DEFAULT_ALTERNATE_UPDATE_SITE;
				else
					m_strUpdateAlternateSite = strAddress;
			}
			else
			{
				if((strAddress == null) || (strAddress.Length == 0))
					m_strUpdateSite = DEFAULT_UPDATE_SITE;
				else
					m_strUpdateSite = strAddress;
			}
			
		}// public void SetUpdateSite(string strAddress, bool bAlternate)
		
		/// <summary>This method is called to get the address of the activate site</summary>
		/// <returns>the address of the requested site</returns>
		public string GetActivateSite()
		{
			if(m_strActivateSite.Length == 0)
				m_strActivateSite = DEFAULT_ACTIVATE_SITE;
			return m_strActivateSite;
			
		}// public string GetActivateSite()
		
		/// <summary>This method is called to set the address of the activate site</summary>
		/// <param name="strAddress">the new site address</param>
		public void SetActivateSite(string strAddress)
		{
			if((strAddress == null) || (strAddress.Length == 0))
				m_strActivateSite = DEFAULT_ACTIVATE_SITE;
			else
				m_strActivateSite = strAddress;
			
		}// public void SetActivateSite(string strAddress)
		
		/// <summary>This method is called to get the address of the activate site</summary>
		/// <returns>the address of the requested site</returns>
		public string GetOnlineSite()
		{
			if(m_strOnlineSite.Length == 0)
				m_strOnlineSite = DEFAULT_ONLINE_SITE;
			return m_strOnlineSite;
			
		}// public string GetOnlineSite()
		
		/// <summary>This method is called to set the address of the activate site</summary>
		/// <param name="strAddress">the new site address</param>
		public void SetOnlineSite(string strAddress)
		{
			if((strAddress == null) || (strAddress.Length == 0))
				m_strOnlineSite = DEFAULT_ONLINE_SITE;
			else
				m_strOnlineSite = strAddress;
			
		}// public void SetOnlineSite(string strAddress)
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to retrieve the component installation information: component = %1");
			aStrings.Add("An exception was raised while attempting to fill the product components collection");
			aStrings.Add("An exception was raised while attempting to verify the component installation information: component = %1");
			aStrings.Add("Unable to locate the TrialMax PDF converter:\n\n%1\n\nIt appears to have been installed and then deleted. The installation information will be removed from the registry. You will need to run on-line updates (Help->Check For Updates) to reinstall the converter.");
			aStrings.Add("Unable to locate the TrialMax Objections Report Generator:\n\n%1\n\nIt appears to have been installed and then deleted. The installation information will be removed from the registry. You will need to run on-line updates (Help->Check For Updates) to reinstall the generator.");

		}// private virtual void SetErrorStrings()
		
		/// <summary>This method is called to display a non-modal error message</summary>
		///	<param name="strMessage">the error message to be displayed</param>
		/// <remarks>We have to use a non-modal message box because errors displayed during initialization mess the application up</remarks>
		private void ShowError(string strMessage)
		{
			if(strMessage.Length > 0)
			{
				m_strErrorMessage = strMessage;
				
				System.Threading.Thread nonModal = new System.Threading.Thread(new ThreadStart(this.ShowErrorProc));
				nonModal.Start();
			}
			
		}//	private void ShowError(string strMessage)
		
		/// <summary>This method is the thread service procedure for a non-modal error message</summary>
		private void ShowErrorProc()
		{
			if(m_strErrorMessage.Length > 0)
			{
				MessageBox.Show(m_strErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			
		}//	private void ShowErrorProc()
		
		/// <summary>This method is called to verify the information stored in the registry for the specified component</summary>
		/// <param name="eComponent">The unique component identifier</param>
		/// <param name="bRemove">true to remove from registry if unable to locate the installation</param>
		/// <returns>true if successful</returns>
		private bool Verify(TmaxComponents eComponent, bool bRemove)
		{
			bool			bSuccessful = false;
			CTmaxComponent	tmaxInstalled = null;
			CTmaxComponent	tmaxRegistered = null;
			
			Debug.Assert(this.Components != null);
			Debug.Assert(this.Registry != null);
				
			try
			{
				//	Get the component descriptors
				tmaxInstalled  = GetInstalled(eComponent);
				tmaxRegistered = this.Components.Find(eComponent);

				//	Were we able to locate the installation?
				if(tmaxInstalled != null)
				{
					//	Make sure we have the right information in the versions file
					if(m_xmlVersions != null)
						Save(tmaxInstalled, m_xmlVersions);	
						
					if(tmaxRegistered == null)
					{
						this.Components.Add(tmaxInstalled);							
					}
					else
					{
						tmaxRegistered.Version = tmaxInstalled.Version;
						tmaxRegistered.Folder = tmaxInstalled.Folder;
						tmaxRegistered.Filename = tmaxInstalled.Filename;

						if ((tmaxRegistered.Description.Length == 0) && (tmaxInstalled.Description.Length > 0))
							tmaxRegistered.Description = tmaxInstalled.Description;
					}
				}
				else
				{
					//	Is this component registered?
					if(tmaxRegistered != null)
					{
						//	Should we remove the component?
						if(bRemove == true)
						{
							m_xmlVersions.DeleteSection(tmaxRegistered.Name);
							m_xmlVersions.Save();

							this.Components.Remove(tmaxRegistered);
						}
						
					}// if(tmaxRegistered != null)
					
				}// if(tmaxInstalled != null)
			
			}	
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Verify", m_tmaxErrorBuilder.Message(ERROR_VERIFY_EX, eComponent), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Verify(TmaxComponents eComponent, bool bRemove)
		
		/// <summary>This method will create the activation key required for the machine</summary>
		/// <param name="strDatabaseId">Database id to be appended to the end of the code</param>
		/// <returns>The key used to activate the local machine</returns>
		private string CreateActivationCode(string strDatabaseId)
		{
			string		strSeed = "";
			string		strActivate = "";
			ushort		usCRC = 0;
			CTmaxCRC16	crcCalculator = null;
			
			try
			{
				//	Get the activation seed
				strSeed = GetActivationSeed();
				
				crcCalculator = new CTmaxCRC16();
				usCRC = crcCalculator.GetCRC16(strSeed);

				if(usCRC > 0)
					strActivate = usCRC.ToString();
					
				if((strDatabaseId != null) && (strDatabaseId.Length > 0))
					strActivate += strDatabaseId;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "CreateActivationCode", "Exception raised getting code for: " + strSeed, Ex);
			}
		
			return strActivate;
			
		}// private string CreateActivationCode()

		/// <summary>This method will create the activation key used prior to .NET 2.0</summary>
		/// <returns>The key used to activate the local machine</returns>
		private string CreateOldActivationCode()
		{
			string strSeed = "";
			long lHash = 0;

			try
			{
				//	Get the activation seed
				strSeed = GetActivationSeed();

				//	Use the absolute value of the seed's hash code
				//
				//	Windows changed the hashing algorithm in .NET 2.0 so we
				//	switched to CRC for the activation code
				lHash = System.Math.Abs(strSeed.GetHashCode());

				return lHash.ToString();

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "CreateOldActivationCode", "Exception raised getting code for: " + strSeed, Ex);
				return "";
			}

		}// private string CreateOldActivationCode()

		/// <summary>This method is called to load the version information for the specified component</summary>
		/// <param name="tmaxComponent">The desired product component</param>
		/// <param name="xmlVersions">The XML versions file containing the component descriptions</param>
		/// <returns>true if successful</returns>
		public bool Load(CTmaxComponent tmaxComponent, CXmlIni xmlVersions)
		{
			bool bSuccessful = false;
			
			while(bSuccessful == false)
			{
				if(xmlVersions.SetSection(tmaxComponent.Name) == false)
					break;
				
				tmaxComponent.Version = xmlVersions.Read(XML_VERSIONS_VERSION);
				tmaxComponent.Folder = xmlVersions.Read(XML_VERSIONS_FOLDER);
				tmaxComponent.Filename = xmlVersions.Read(XML_VERSIONS_FILENAME);
				tmaxComponent.Description = xmlVersions.Read(XML_VERSIONS_DESCRIPTION);
				tmaxComponent.ActivationCode = xmlVersions.Read(XML_VERSIONS_ACTIVATION_CODE);
                tmaxComponent.ActivationDate = EncryptDecrypt.Decrypt(xmlVersions.Read(XML_VERSIONS_ACTIVATION_DATE),true);
				
				bSuccessful = true;

			}
			return bSuccessful;
			
		}// public bool Load(CTmaxComponent tmaxComponent, CXmlIni xmlVersions)

		/// <summary>This method is called to store the update settings in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the current values in</param>
		public bool Save(CTmaxComponent tmaxComponent, CXmlIni xmlVersions)
		{
			bool bSuccessful = false;
			
			if(xmlVersions.SetSection(tmaxComponent.Name) == true)
			{
				if(tmaxComponent.Description.Length > 0)
					xmlVersions.Write(XML_VERSIONS_DESCRIPTION, tmaxComponent.Description);
				if (tmaxComponent.Version.Length > 0)
					xmlVersions.Write(XML_VERSIONS_VERSION, tmaxComponent.Version);
				if (tmaxComponent.Folder.Length > 0)
					xmlVersions.Write(XML_VERSIONS_FOLDER, tmaxComponent.Folder);
				if (tmaxComponent.Filename.Length > 0)
					xmlVersions.Write(XML_VERSIONS_FILENAME, tmaxComponent.Filename);
				if (tmaxComponent.ActivationCode.Length > 0)
					xmlVersions.Write(XML_VERSIONS_ACTIVATION_CODE, tmaxComponent.ActivationCode);
                if (tmaxComponent.ActivationDate.Length > 0)
                    xmlVersions.Write(XML_VERSIONS_ACTIVATION_DATE, EncryptDecrypt.Encrypt(tmaxComponent.ActivationDate, true));
				
				bSuccessful = xmlVersions.Save();
			}
			
			return bSuccessful;

		}// public bool Save(CTmaxComponent tmaxComponent, CXmlIni xmlVersions)


		#endregion Private Methods
		
		#region Properties

		/// <summary>The XML INI file containing the product components version information</summary>
		public FTI.Shared.Xml.CXmlIni XmlVersions
		{
			get { return m_xmlVersions; }
		}
		
		/// <summary>The collection of components that make up the product</summary>
		public FTI.Shared.Trialmax.CTmaxComponents Components
		{
			get { return m_tmaxComponents; }
			set { m_tmaxComponents = value; }
		}
		
		/// <summary>The application's registry interface</summary>
		public FTI.Shared.Trialmax.CTmaxRegistry Registry
		{
			get 
			{ 
				//	Has a registry interface been assigned?
				if(m_tmaxRegistry == null)
					m_tmaxRegistry = new CTmaxRegistry();
					
				return m_tmaxRegistry; 
			
			}
			set { m_tmaxRegistry = value; }
		}
		
		/// <summary>Event source used to fire system events</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The default product name</summary>
		public string Name
		{
			get { return GetProductName(); }
		}

        /// <summary>Days Allowed after trial installed</summary>
        public int DaysAllowed
        {
            get { return m_intDaysAllowed; }

        }

		/// <summary>The 4 part product version</summary>
		public string Version
		{
			get { return GetProductVersion(false); }
		}

		/// <summary>The 3 part product version</summary>
		public string ShortVersion
		{
			get { return GetProductVersion(true); }
		}

		/// <summary>True if the product has been activated</summary>
		public bool Activated
		{
			get { return GetActivated(); }
		}

		/// <summary>URL to online support site</summary>
		public string OnlineSite
		{
			get { return GetOnlineSite(); }
			set { SetOnlineSite(value); }
		}

		/// <summary>Primary site used to activate the product</summary>
		public string ActivateSite
		{
			get { return GetActivateSite(); }
			set { SetActivateSite(value); }
		}

		/// <summary>Parameterized link to get the activation code</summary>
		public string ActivationLink
		{
			get { return GetActivationLink(); }
		}

		/// <summary>Primary site used to retrieve product updates</summary>
		public string UpdateSite
		{
			get { return GetUpdateSite(false); }
			set { SetUpdateSite(value, false); }
		}

		/// <summary>Alternate site used to retrieve product updates</summary>
		public string UpdateAlternateSite
		{
			get { return GetUpdateSite(true); }
			set { SetUpdateSite(value, true); }
		}

		/// <summary>Name of the file containing the list of available updates</summary>
		public string UpdateFilename
		{
			get { return m_strUpdateFilename; }
			set { m_strUpdateFilename = value; }
		}
		
		/// <summary>User name for connecting to the update site</summary>
		public string UpdateUserName
		{
			get { return m_strUpdateUserName; }
			set { m_strUpdateUserName = value; }
		}
		
		/// <summary>Password for connecting to the update site</summary>
		public string UpdatePassword
		{
			get { return m_strUpdatePassword; }
			set { m_strUpdatePassword = value; }
		}
		
		/// <summary>Identifier used to filter on-line updates</summary>
		public string UpdateGroupId
		{
			get { return m_strUpdateGroupId; }
			set { m_strUpdateGroupId = value; }
		}
		
		/// <summary>True to user proxy settings in Internet Explorer</summary>
		public bool UseIEProxy
		{
			get { return m_bUseIEProxy; }
			set { m_bUseIEProxy = value; }
		}

		/// <summary>Proxy server name</summary>
		public string ProxyServerName
		{
			get { return m_strProxyServerName; }
			set { m_strProxyServerName = value; }
		}

		/// <summary>Proxy server port identifier</summary>
		public int ProxyPort
		{
			get { return m_iProxyPort; }
			set { m_iProxyPort = value; }
		}

		/// <summary>Proxy server user name</summary>
		public string ProxyUserName
		{
			get { return m_strProxyUserName; }
			set { m_strProxyUserName = value; }
		}

		/// <summary>Proxy server password</summary>
		public string ProxyPassword
		{
			get { return m_strProxyPassword; }
			set { m_strProxyPassword = value; }
		}

		/// <summary>Host application identifier</summary>
		public TmaxComponents HostApplication
		{
			get { return m_eHostApplication; }
			set { m_eHostApplication = value; }
		}

		/// <summary>Fully qualified path of the file containing the product version information</summary>
		public string VersionsFileSpec
		{
			get { return m_strVersionsFileSpec; }
			set { m_strVersionsFileSpec = value; }
		}

	    /// <summary>Fully qualified path of the file containing the product version information</summary>
	    public int MasterKeyDefaultDuration
	    {get; set;}
        
		#endregion Properties

	}// public class CTmaxProductManager

}// namespace FTI.Shared.Trialmax
