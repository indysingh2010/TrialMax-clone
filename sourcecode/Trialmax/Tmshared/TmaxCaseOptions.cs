using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the options used for printing</summary>
	public class CTmaxCaseOptions
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME			= "global";
		private const string XMLINI_VERSION					= "version";
		private const string XMLINI_ALLOW_EDIT_ALIASES		= "allowEditAliases";
		private const string XMLINI_CREATED_ON				= "createdOn";
		private const string XMLINI_PATH_MAPS_LOCKED		= "pathMapsLocked";
		private const string XMLINI_ALIASES_LOCKED			= "aliasesLocked";
		private const string XMLINI_SYNC_XML_DESIGNATIONS	= "syncXmld";
		
		private const int ERROR_OPEN_EX						= 0;
		private const int ERROR_SAVE_NO_FILE				= 1;
		private const int ERROR_SAVE_EX						= 2;
		private const int ERROR_INITIALIZE_EX				= 3;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to manage the configuration file</summary>
		private FTI.Shared.Xml.CXmlIni m_xmlIni = null;
		
		/// <summary>Local member bound to Machine property</summary>
		private FTI.Shared.Trialmax.CTmaxMachine m_tmaxMachine = new CTmaxMachine();
		
		/// <summary>Local member bound to PathMaps property</summary>
		private CTmaxPathMaps m_tmaxPathMaps = new CTmaxPathMaps();

		/// <summary>The active case path map</summary>
		private CTmaxPathMap m_tmaxPathMap = null;

		/// <summary>Local member bound to Aliases property</summary>
		private CTmaxAliases m_tmaxAliases = new CTmaxAliases();

		/// <summary>Local member bound to CaseFolder property</summary>
		private string m_strCaseFolder = "";

		/// <summary>Local member bound to Version property</summary>
		private string m_strVersion = "";

		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";

		/// <summary>Local member bound to CreatedOn property</summary>
		private string m_strCreatedOn = "";

		/// <summary>Local member bound to AliasesLocked property</summary>
		private string m_strAliasesLocked = "";

		/// <summary>Local member bound to PathMapsLocked property</summary>
		private string m_strPathMapsLocked = "";

		/// <summary>Local member bound to AllowEditAliases property</summary>
		private bool m_bAllowEditAliases = true;
		
		/// <summary>Local member bound to SyncXmlDesignations property</summary>
		private bool m_bSyncXmlDesignations = true;
		
		#endregion Private Members
	
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxCaseOptions()
		{
			//	Populate the error builder collection
			SetErrorStrings();

			this.EventSource.Attach(m_tmaxPathMaps.EventSource);
			this.EventSource.Attach(m_tmaxAliases.EventSource);
			
		}// public CTmaxCaseOptions()
		
		/// <summary>This method is called to initialize the options when the user opens the database</summary>
		/// <param name="strFileSpec">The fully qualified path to the file</param>
		/// <param name="strMachineName">The name of the machine opening the file</param>
		/// <returns>true if successful</returns>
		public bool Initialize(string strFileSpec, string strMachineName)
		{
			bool				bSuccessful = false;
			CTmsharedVersion	ver = null;
			
			try
			{
				m_tmaxMachine.Initialize(strMachineName);
				m_strFileSpec = strFileSpec;

				//	Initialize the class members
				m_strVersion = "";
				m_strCreatedOn = "";
				m_strAliasesLocked = "";
				m_strPathMapsLocked = "";
				m_bAllowEditAliases = false;
				m_bSyncXmlDesignations = true;
				m_tmaxAliases.Clear();
				m_tmaxPathMaps.Clear();

				//	Load the values stored in the configuration file
				while(bSuccessful == false)
				{
					//	Open the configuration file
					if(Open(true, true) == false)
						break;
						
					//	Are we creating a new file?
					if(CreatedOn.Length == 0)
					{
						//	Line up on the correct section
						if(m_xmlIni.SetSection(XMLINI_SECTION_NAME) == false)
							break;

						//	Write the persistant case options						
						ver = new CTmsharedVersion();
						m_strVersion = ver.Version;
						m_strCreatedOn = System.DateTime.Now.ToString();
						m_xmlIni.Write(XMLINI_CREATED_ON, CreatedOn);
						m_xmlIni.Write(XMLINI_VERSION, Version);
						m_xmlIni.Write(XMLINI_ALLOW_EDIT_ALIASES, AllowEditAliases);
						m_xmlIni.Write(XMLINI_SYNC_XML_DESIGNATIONS, SyncXmlDesignations);
						m_xmlIni.Write(XMLINI_ALIASES_LOCKED, AliasesLocked);
						m_xmlIni.Write(XMLINI_PATH_MAPS_LOCKED, PathMapsLocked);

						//	Is this one of the older case options files?
						ConvertSingleUser();
						
						//	Write the default aliases
						m_tmaxAliases.Save(m_xmlIni);
						
						//	Write the default path maps
						m_tmaxPathMaps.Save(m_xmlIni);
					
					}// if(CreatedOn.Length == 0)
					else
					{
						//	Retrieve the machine information from the file
						m_tmaxMachine.Load(m_xmlIni);
						m_tmaxMachine.Opened = System.DateTime.Now.ToString();
					}
						
					//	Save the file
					//
					//	NOTE:	This automatically saves the machine information
					Save();

					//	Close the XML
					Close();
					
					//	Use the default path map if not defined for this machine
					if(m_tmaxMachine.PathMap <= 0)
						m_tmaxMachine.PathMap = 1;
						
					//	Set the path map for this machine
					if(SetPathMap(m_tmaxMachine.PathMap, false, true) == false)
					{
						//	Assign the default map
						Debug.Assert(m_tmaxPathMaps.Count > 0);
						SetPathMap(1, false, false);
					}
						
					bSuccessful = true;
				
				}// while(bSuccessful == false)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX, strFileSpec), Ex);
			}

			return bSuccessful;
		
		}// public bool Initialize(string strFileSpec, string strMachineName)
		
		/// <summary>This method closes the XML file and resets the class members</summary>
		public void Terminate()
		{
			//	Do we need to save the machine information?
			if((m_tmaxMachine != null) && (m_strFileSpec.Length > 0))
			{
				//	Set the shut-down time
				m_tmaxMachine.Closed = System.DateTime.Now.ToString();
				
				//	Open the file but don't refresh any of the class members
				if(Open(false, false) == true)
				{
					//	Save the machine information
					Save();
				}
			
			}// if((m_tmaxMachine != null) && (m_strFileSpec.Length > 0))
			
			//	Make sure the file is closed
			Close();
			
			//	Reset the class members 
			m_bAllowEditAliases = false;
			m_bSyncXmlDesignations = true;
			m_strVersion = "";
			m_strCreatedOn = "";
			m_strFileSpec = "";
			m_strAliasesLocked = "";
			m_strPathMapsLocked = "";
			m_tmaxPathMap = null;
			if(m_tmaxAliases != null)
				m_tmaxAliases.Clear();
			if(m_tmaxPathMaps != null)
				m_tmaxPathMaps.Clear();
			if(m_tmaxMachine != null)
				m_tmaxMachine.Initialize("");
				
		}// public void Terminate()
		
		/// <summary>This method is called to get the path assigned to the specified folder</summary>
		/// <param name="eFolder">The enumerator that identifies the desired folder</param>
		/// <returns>The path assigned to the folder</returns>
		public string GetCasePath(TmaxCaseFolders eFolder)
		{
			if(m_tmaxPathMap != null)
				return m_tmaxPathMap.GetCasePath(eFolder);
			else
				return "";
			
		}// public string GetCasePath(TmaxCaseFolders eFolder)
		
		/// <summary>This method is called to set the path assigned to the specified folder</summary>
		/// <param name="eFolder">The enumerator that identifies the desired folder</param>
		/// <param name="strPath">The path to be assigned to the folder</param>
		public void SetCasePath(TmaxCaseFolders eFolder, string strPath)
		{
			if(m_tmaxPathMap != null)
				m_tmaxPathMap.SetCasePath(eFolder, strPath);
			
		}// public void SetCasePath(TmaxCaseFolders eFolder, string strPath)
		
		/// <summary>This method is called to save the values to the XML configuration file</summary>
		/// <param name="bPathMaps">True to save the path maps</param>
		/// <param name="bAliases">True to save the aliases</param>
		/// <returns>True if successful</returns>
		public bool Save(bool bPathMaps, bool bAliases)
		{
			bool bSuccessful = false;
			
			//	Make sure the file is opened
			if(m_xmlIni == null)
			{
				if(Open(!bPathMaps, !bAliases) == false)
					return false;
			}
			
			//	Write the requested collections to file
			if((bPathMaps == true) && (m_tmaxPathMaps != null))
				m_tmaxPathMaps.Save(m_xmlIni);
			if((bAliases == true) && (m_tmaxAliases != null))
				m_tmaxAliases.Save(m_xmlIni);
			
			//	Save the configuration file
			bSuccessful = Save();
			
			//	Close the file
			Close();
			
			return bSuccessful;
			
		}// public bool Save(bool bPathMaps, bool bAliases) 
		
		/// <summary>This method will add an alias to the collection</summary>
		/// <param name="strAlias">The aliased path to be added</param>
		/// <param name="bSave">True to update the configuration file</param>
		/// <returns>True if successful</returns>
		public CTmaxAlias AddAlias(string strAlias, bool bSave)
		{
			CTmaxAlias tmaxAlias = null;
			
			//	Make sure we have the latest set of aliases
			Open(false, true);
			
			//	Create the new alias
			tmaxAlias = m_tmaxAliases.Add(strAlias);
			
			//	Should we save the alias?
			if((tmaxAlias != null) && (bSave == true))
				SaveAliases();
			else
				Close(); // Make sure the file is closed
			
			return tmaxAlias;
			
		}// public CTmaxAlias AddAlias(string strAlias, bool bSave)
		
		/// <summary>This method is called to save the aliases to file without disrupting the other class members</summary>
		/// <returns>True if successful</returns>
		public bool SaveAliases()
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxAliases != null);
			if(m_tmaxAliases == null) return false;
			
			//	Make sure the file is opened
			if(m_xmlIni == null)
			{
				if(Open(false, false) == false)
					return false;
			}
			
			//	Write the aliases to the file
			m_tmaxAliases.Save(m_xmlIni);
			
			//	Save the configuration file
			bSuccessful = Save();
			
			//	Close the file
			Close();
			
			return bSuccessful;
			
		}// private bool SaveAliases() 
		
		/// <summary>This method is called to save the path maps to file without disrupting the other class members</summary>
		/// <returns>True if successful</returns>
		public bool SavePathMaps()
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxPathMaps != null);
			if(m_tmaxPathMaps == null) return false;
			
			//	Make sure the file is opened
			if(m_xmlIni == null)
			{
				if(Open(false, false) == false)
					return false;
			}
			
			//	Write the path maps to the file
			m_tmaxPathMaps.Save(m_xmlIni);
			
			//	Save the configuration file
			bSuccessful = Save();
			
			//	Close the file
			Close();
			
			return bSuccessful;
			
		}// private bool SavePathMaps() 
		
		/// <summary>This method is called to get control of the PathMaps collection</summary>
		/// <param name="bRefresh">true to refresh the collection from file</param>
		/// <param name="bLock">true to lock the collection for editing</param>
		/// <returns>The path maps collection if available</returns>
		public CTmaxPathMaps GetPathMaps(bool bRefresh, bool bLock)
		{
			string	strMsg = "";
			bool	bLockFailed = false;
			
			//	Make sure we have the maps collection
			Debug.Assert(m_tmaxPathMaps != null);
			if(m_tmaxPathMaps == null) return null;
			
			//	Do we have a valid file specification?
			if(m_strFileSpec.Length > 0)
			{
				//	Should we open the file?
				if((bRefresh == true) || (bLock == true))
				{
					//	Open the file and refresh the maps if requested
					Open(bRefresh, false);

					//	Did we refresh the path maps collection?
					if(bRefresh == true)
					{
						//	Reset the active Path Map
						if(m_tmaxPathMap != null)
							SetPathMap(m_tmaxPathMap.Id, false, false);
							
						//	Assign the default if necessary
						if(m_tmaxPathMap == null)
						{
							Debug.Assert(m_tmaxPathMaps.Count > 0, "No Path Maps");
							SetPathMap(1, false, false);
						}
					
					}// if(bRefresh == true)
			
					//	Does the user want to lock the collection?
					if(bLock == true)
					{
						//	Is it locked by somebody else?
						if((m_strPathMapsLocked.Length > 0) && (String.Compare(m_strPathMapsLocked, GetLockId(), true) != 0))
						{
							//	Prompt the user
							strMsg = String.Format("{0} has locked the path maps for editing. If you want to override the lock it's possible that one of you may loose your changes. Do you want to override?", m_strPathMapsLocked);
							if(MessageBox.Show(strMsg, "Locked", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							{
								bLockFailed = true;
							}
							
						}
						
						//	Should we set the lock?
						if(bLockFailed == false)
						{
							m_xmlIni.SetSection(XMLINI_SECTION_NAME);
							m_xmlIni.Write(XMLINI_PATH_MAPS_LOCKED, GetLockId());
							Save();
						}
						
					}// if(bLock == true)
					
					//	Make sure the file is closed
					Close();
				
				}// if((bRefresh == true) || (bLock == true))
				
			}// if((bRefresh == true) && (m_strFileSpec.Length > 0))
			
			if(bLockFailed == false)
				return m_tmaxPathMaps;
			else
				return null;
		
		}// public CTmaxPathMaps GetPathMaps(bool bRefresh, bool bLock)
		
		/// <summary>This method is called to get control of the Aliases collection</summary>
		/// <param name="bRefresh">true to refresh the collection from file</param>
		/// <param name="bLock">true to lock the collection for editing</param>
		/// <returns>The the aliases collection if available</returns>
		public CTmaxAliases GetAliases(bool bRefresh, bool bLock)
		{
			string	strMsg = "";
			bool	bLockFailed = false;
			
			//	Make sure we have the collection
			Debug.Assert(m_tmaxAliases != null);
			if(m_tmaxAliases == null) return null;
			
			//	Do we have a valid file specification?
			if(m_strFileSpec.Length > 0)
			{
				//	Should we open the file?
				if((bRefresh == true) || (bLock == true))
				{
					//	Open the file and refresh the collection if requested
					Open(false, bRefresh);

					//	Does the user want to lock the collection?
					if(bLock == true)
					{
						//	Is it locked by somebody else?
						if((m_strAliasesLocked.Length > 0) && (String.Compare(m_strAliasesLocked, GetLockId(), true) != 0))
						{
							//	Prompt the user
							strMsg = String.Format("{0} has locked the aliases for editing. If you want to override the lock it's possible that one of you may loose your changes. Do you want to override?", m_strAliasesLocked);
							if(MessageBox.Show(strMsg, "Locked", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							{
								bLockFailed = true;
							}
							
						}
						
						//	Should we set the lock?
						if(bLockFailed == false)
						{
							m_xmlIni.SetSection(XMLINI_SECTION_NAME);
							m_xmlIni.Write(XMLINI_ALIASES_LOCKED, GetLockId());
							Save();
						}
						
					}// if(bLock == true)
					
					//	Make sure the file is closed
					Close();
				
				}// if((bRefresh == true) || (bLock == true))
				
			}// if((bRefresh == true) && (m_strFileSpec.Length > 0))
			
			if(bLockFailed == false)
				return m_tmaxAliases;
			else
				return null;
		
		}// public CTmaxAliases GetAliases(bool bRefresh, bool bLock)
		
		/// <summary>This method is called to get the collection of machines stored in the file</summary>
		/// <returns>The the aliases collection if available</returns>
		public ArrayList GetTmaxMachines()
		{
			ArrayList		tmaxMachines = null;
			CTmaxMachine	tmaxMachine = null;
			string			strPrefix = CTmaxMachine.GetXmlSectionPrefix();
			string			strName = "";
			
			//	Do we have a valid file specification?
			if(m_strFileSpec.Length > 0)
			{
				//	Open the file without refreshing any of the collections
				Open(false, false);

				if((m_xmlIni != null) && (m_xmlIni.XMLSections != null))
				{
					tmaxMachines = new ArrayList();
					
					//	Search for all machine sections
					foreach(XmlNode O in m_xmlIni.XMLSections)
					{
						//	Get the section name for this node
						strName = m_xmlIni.GetSectionName(O);
						
						//	Is this a machine section?
						if(strName.StartsWith(strPrefix) == true)
						{
							//	Strip the prefix
							strName = strName.Substring(strPrefix.Length);
							
							if(strName.Length > 0)
							{
								tmaxMachine = new CTmaxMachine(strName);
								tmaxMachine.Load(m_xmlIni);
								
								tmaxMachines.Add(tmaxMachine);
							}
						
						}// if(strName.StartsWith(strPrefix) == true)
					
					}// foreach(XmlNode O in m_xmlIni.XMLSections)

				}// if((m_xmlIni != null) && (m_xmlIni.XMLSections != null))
				
				//	Make sure the file is closed
				Close();
				
			}// if(m_strFileSpec.Length > 0)
			
			if((tmaxMachines != null) && (tmaxMachines.Count > 0))
				return tmaxMachines;
			else
				return null;
		
		}// public ArrayList GetTmaxMachines()
		
		/// <summary>This method is called to release the lock on the Aliases collection</summary>
		/// <param name="bSave">true to save the Alias values</param>
		/// <returns>true if successful</returns>
		public bool ReleaseAliases(bool bSave)
		{
			//	Open the XML configuration file
			if(Open(false, false) == false) return false;
			
			//	Should we save the Aliases?
			if(bSave == true)
				m_tmaxAliases.Save(m_xmlIni);
				
			//	Is the collection locked?
			if(m_strAliasesLocked.Length > 0)
			{
				//	Is it OK for us to clear the lock?
				if(String.Compare(GetLockId(), m_strAliasesLocked, true) == 0)
				{
					m_xmlIni.SetSection(XMLINI_SECTION_NAME);
					m_xmlIni.Write(XMLINI_ALIASES_LOCKED, "");
					Save();
					
					//	No need to save any more
					bSave = false;
				}
				
			}// if(m_strAliasesLocked.Length > 0)
			
			//	Do we need to save the file
			if(bSave)
				Save();
				
			//	Make sure the file is closed
			Close();
				
			return true;
			
		}// public bool ReleaseAliases(bool bSave)
		
		/// <summary>This method is called to release the lock on the PathMaps collection</summary>
		/// <param name="bSave">true to save the PathMap values</param>
		/// <returns></returns>
		public bool ReleasePathMaps(bool bSave)
		{
			//	Open the XML configuration file
			if(Open(false, false) == false) return false;
			
			//	Should we save the PathMaps?
			if(bSave == true)
				m_tmaxPathMaps.Save(m_xmlIni);
				
			//	Is the collection locked?
			if(m_strPathMapsLocked.Length > 0)
			{
				//	Is it OK for us to clear the lock?
				if(String.Compare(GetLockId(), m_strPathMapsLocked, true) == 0)
				{
					m_xmlIni.SetSection(XMLINI_SECTION_NAME);
					m_xmlIni.Write(XMLINI_PATH_MAPS_LOCKED, "");
					Save();
					
					//	No need to save any more
					bSave = false;
				}
				
			}// if(m_strPathMapsLocked.Length > 0)
			
			//	Do we need to save the file
			if(bSave)
				Save();
				
			//	Make sure the file is closed
			Close();
				
			return true;
			
		}// public bool ReleasePathMaps(bool bSave)
		
		/// <summary>This method will set the active path map</summary>
		/// <param name="iId">The id of the new path map</param>
		/// <param name="bSave">True to save the change to file</param>
		/// <param name="bWarn">True to warn the user if the map does not exist</param>
		/// <returns>True if successful</returns>
		public bool SetPathMap(int iId, bool bSave, bool bWarn)
		{
			CTmaxPathMap	tmaxPathMap = null;
			string			strWarning = "";
			
			//	Locate the requested path map
			if(m_tmaxPathMaps != null)
				tmaxPathMap = m_tmaxPathMaps.Find(iId);
				
			if(tmaxPathMap == null)
			{
				//	Should we warn the user?
				if(bWarn == true)
				{
					strWarning = String.Format("Unable to locate path map #{0}. Use Tools->Case Options to set the path map for this case.", iId);
					MessageBox.Show(strWarning, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				return false;
				
			}
			else if(tmaxPathMap.IsDeleted == true)
			{
				//	Warn the user without cancelling the operation
				if(bWarn == true)
				{
					strWarning = String.Format("The path map named {0} has been deleted by {1}. Use Tools->Case Options to select a new path map for this case.", tmaxPathMap.Name, tmaxPathMap.DeletedBy);
					MessageBox.Show(strWarning, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}

			}
			
			//	Assign the new path map
			m_tmaxPathMap = tmaxPathMap;
			
			//	Update the active machine
			if(m_tmaxMachine != null)
				m_tmaxMachine.PathMap = m_tmaxPathMap.Id;
				
			//	Should we save the change?
			if(bSave == true)
			{
				//	Make sure the file is open
				Open(false, false);
				
				//	Save the changes (this automatically updates the machine)
				Save();
				
			}
			
			return true;
		
		}// public bool SetPathMap(int iId, bool bSave)
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method saves the current values to the XML file</summary>
		private bool Save()
		{
			bool bSuccessful = false;
			
			//	Make sure we have a valid XML file
			if(m_xmlIni == null)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_NO_FILE));
				return false;
			}
			
			try
			{
				//	Save the options to file
				while(bSuccessful == false)
				{
					//	Always update the machine information
					if(m_tmaxMachine != null)
						m_tmaxMachine.Save(m_xmlIni);
						
					//	Save the file
					if(m_xmlIni.Save() == false)
						break;
						
					bSuccessful = true;
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_EX, FileSpec), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Save()
		
		/// <summary>This method is called to open the specified XML configuration file</summary>
		/// <param name="bMaps">True to populate the path maps collection</param>
		/// <param name="bAliases">True to populate the aliases collection</param>
		/// <returns>true if successful</returns>
		private bool Open(bool bMaps, bool bAliases)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Make sure the existing file is closed
				Close();
			
				Debug.Assert(m_strFileSpec.Length > 0);

				//	Allocate and initialize a new XML file object
				m_xmlIni = new CXmlIni();
				m_xmlIni.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
				m_xmlIni.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
				
				m_xmlIni.XMLComments.Add("TrialMax .NET XML Case Configuration File");
				m_xmlIni.XMLComments.Add("Copyright FTI Consulting");
				
				//	Load the values stored in the configuration file
				while(bSuccessful == false)
				{
					//	Open the file
					if(m_xmlIni.Open(m_strFileSpec) == false)
						break;
						
					//	Line up on the appropriate section
					if(m_xmlIni.SetSection(XMLINI_SECTION_NAME) == false) 
						break;
			
					//	Read the static case options
					m_strCreatedOn = m_xmlIni.Read(XMLINI_CREATED_ON, "");
					m_strVersion = m_xmlIni.Read(XMLINI_VERSION, Version);
					m_strAliasesLocked = m_xmlIni.Read(XMLINI_ALIASES_LOCKED, AliasesLocked);
					m_strPathMapsLocked = m_xmlIni.Read(XMLINI_PATH_MAPS_LOCKED, PathMapsLocked);
					m_bAllowEditAliases = m_xmlIni.ReadBool(XMLINI_ALLOW_EDIT_ALIASES, AllowEditAliases);
					m_bSyncXmlDesignations = m_xmlIni.ReadBool(XMLINI_SYNC_XML_DESIGNATIONS, SyncXmlDesignations);
			
					//	Retrieve the drive aliases if requested
					if((bAliases == true) && (m_tmaxAliases != null))
						m_tmaxAliases.Load(m_xmlIni);
						
					//	Retrieve the path maps if requested
					if((bMaps == true) && (m_tmaxPathMaps != null))
						m_tmaxPathMaps.Load(m_xmlIni);
						
					bSuccessful = true;
				
				}// while(bSuccessful == false)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_EX, m_strFileSpec), Ex);
				
				//	Make sure the file is closed
				Close();
			}
					
			return bSuccessful;
		
		}// private bool Open(bool bMaps, bool bAliases)
		
		/// <summary>This method closes the XML file</summary>
		private void Close()
		{
			//	Has the XML file been opened?
			if(m_xmlIni != null)
			{
				m_xmlIni.Close();
				m_xmlIni = null;
			}
			
		}// public void Close()
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to open the XML case options: filename = %1");
			aStrings.Add("Unable to save the case options. The XML file has not been opened.");
			aStrings.Add("An exception was raised while attempting to save the XML case options: filename = %1");
			aStrings.Add("An exception was raised while attempting to initialize the case options: filename = %1");

		}// private void SetErrorStrings()
		
		/// <summary>This method is called to convert an older single-user options file</summary>
		private bool ConvertSingleUser()
		{
			XmlElement	xmlCaseOptions = null;
			XmlNode		xmlAliases = null;
			XmlNode		xmlCasePaths = null;
			
			Debug.Assert(m_xmlIni != null);
			if(m_xmlIni == null) return false;
			Debug.Assert(m_xmlIni.XMLDocument != null);
			if(m_xmlIni.XMLDocument == null) return false;

			try
			{
				//	Could this be an old style single user configuration file
				if(m_xmlIni.XMLRoot == null) return false;
				if(m_xmlIni.XMLRoot.Name != "trialMax") return false;

				//	Is this one of the older single-user options files?
				xmlCaseOptions = (XmlElement)m_xmlIni.XMLDocument.SelectSingleNode("trialMax/caseOptions");
				if(xmlCaseOptions == null) return false;
				
				//	Get the drive aliases node
				xmlAliases = m_xmlIni.XMLDocument.SelectSingleNode("trialMax/caseOptions/driveAliases");
				if((xmlAliases != null) && (m_tmaxAliases != null))
				{
					m_tmaxAliases.Clear();
					m_tmaxAliases.FromXmlNode(xmlAliases);
				}
			
				//	Does it have a casePaths node?
				xmlCasePaths = m_xmlIni.XMLDocument.SelectSingleNode("trialMax/caseOptions/casePaths");
				if((xmlCasePaths != null) && (m_tmaxPathMaps != null) && (m_tmaxPathMaps[0] != null))
				{
					if(m_tmaxPathMaps[0].CasePaths != null)
					{
						m_tmaxPathMaps[0].CasePaths.Clear();
						m_tmaxPathMaps[0].CasePaths.FromXmlNode(xmlCasePaths);
					}	
				
				}
			
				return true;
			
			}
			catch
			{
				return false;
			}
		
		}// private bool ConvertSingleUser()
		
		/// <summary>This method is called to id used to lock one of the collections for editing</summary>
		/// <returns>The lock identifier</returns>
		private string GetLockId()
		{
			if(m_tmaxMachine != null)
				return (m_tmaxMachine.User + " ON " + m_tmaxMachine.Name);
			else
				return "Unknown User ON Unknown Machine";
		
		}// private string GetLockId()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource;	}
		}
		
		/// <summary>Machine information associated with this case</summary>
		public CTmaxMachine Machine
		{
			get { return m_tmaxMachine; }
		}

		/// <summary>Collection of driver/server aliases</summary>
		public CTmaxAliases Aliases
		{
			get { return m_tmaxAliases; }
		}

		/// <summary>Collection of path maps aliases</summary>
		public CTmaxPathMaps PathMaps
		{
			get { return m_tmaxPathMaps; }
		}

		/// <summary>The active path map</summary>
		public CTmaxPathMap PathMap
		{
			get 
			{ 
				return m_tmaxPathMap;  
			}
			set 
			{ 
				//	Make sure all related members are properly set
				SetPathMap(value == null ? -1 : value.Id, false, false);
			}
		
		}

		/// <summary>Allow the user to edit the drive aliases</summary>
		public bool AllowEditAliases
		{
			get { return m_bAllowEditAliases; }
			set { m_bAllowEditAliases = value; }
		}
		
		/// <summary>Synchronize SplitText option for XML designations</summary>
		public bool SyncXmlDesignations
		{
			get { return m_bSyncXmlDesignations; }
			set { m_bSyncXmlDesignations = value; }
		}
		
		/// <summary>Path to the configuration file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; } 
		}
		
		/// <summary>Path to the folder containing the database file</summary>
		public string CaseFolder
		{
			get { return m_strCaseFolder; }
			set { m_strCaseFolder = value; }
		}
		
		/// <summary>Date that the case options file was created</summary>
		public string CreatedOn
		{
			get { return m_strCreatedOn; }
		}
		
		/// <summary>TrialMax shared library version used to create the file</summary>
		public string Version
		{
			get { return m_strVersion; }
			set { m_strVersion = value; }
		}

		/// <summary>Name of user who has the Aliases collection locked</summary>
		public string AliasesLocked
		{
			get { return m_strAliasesLocked; }
			set { m_strAliasesLocked = value; }
		}

		/// <summary>Name of user who has the PathMaps collection locked</summary>
		public string PathMapsLocked
		{
			get { return m_strPathMapsLocked; }
			set { m_strPathMapsLocked = value; }
		}

		#endregion Properties
	
	}// public class CTmaxCaseOptions

}// namespace FTI.Shared.Trialmax
