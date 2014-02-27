using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;

using FTI.Shared;
using FTI.Shared.Text;
using FTI.Shared.Xml;
using System.IO;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the options used for printing</summary>
	public class CTmaxCodesManager
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME		= "codesManager";
		private const string XMLINI_CASE_ID				= "caseId";
		private const string XMLINI_CASE_VERSION		= "caseVersion";
		private const string XMLINI_CREATED_ON			= "createdOn";
		private const string XMLINI_CREATED_BY			= "createdBy";
		private const string XMLINI_MODIFIED_ON			= "modifiedOn";
		private const string XMLINI_MODIFIED_BY			= "modifiedBy";
		private const string XMLINI_LOCK_HOLDER			= "caseCodesLocked";
		
		private const int ERROR_OPEN_EX					= 0;
		private const int ERROR_SAVE_NO_FILE			= 1;
		private const int ERROR_SAVE_EX					= 2;
		private const int ERROR_LOAD_EX			= 3;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to manage the configuration file</summary>
		private FTI.Shared.Xml.CXmlIni m_xmlIni = null;

        /// <summary>Local member used to manage the configuration file</summary>
        private FTI.Shared.Text.CTextIni  m_textIni = null;
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCodes m_tmaxCaseCodes = new CTmaxCaseCodes();
		
		/// <summary>Local member bound to PickLists property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickLists = new CTmaxPickItem();
		
		/// <summary>Local member bound to DxCaseCodes property</summary>
		private object m_dxCaseCodes = null;
		
		/// <summary>Local member bound to UserName property</summary>
		private string m_strUserName = "";
		
		/// <summary>Local member bound to CaseId property</summary>
		private string m_strCaseId = "";

		/// <summary>Local member bound to CaseVersion property</summary>
		private string m_strCaseVersion = "";

		/// <summary>Local member bound to MachineName property</summary>
		private string m_strMachineName = "";

		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";

		/// <summary>Local member bound to CreatedOn property</summary>
		private string m_strCreatedOn = "";

		/// <summary>Local member bound to CreatedBy property</summary>
		private string m_strCreatedBy = "";

		/// <summary>Local member bound to ModifiedOn property</summary>
		private string m_strModifiedOn = "";

		/// <summary>Local member bound to ModifiedBy property</summary>
		private string m_strModifiedBy = "";

		/// <summary>Local member bound to CaseCodesLocked property</summary>
		private string m_strLockHolder = "";

		/// <summary>Local member bound to Modified property</summary>
		private bool m_bModified = false;

		/// <summary>Local member bound to Locked property</summary>
		private bool m_bLocked = false;

		/// <summary>Local member bound to PickListsEnabled property</summary>
		private bool m_bPickListsEnabled = false;

		#endregion Private Members
	
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxCodesManager()
		{
			//	Populate the error builder collection
			SetErrorStrings();

			this.EventSource.Name = "Case Codes Manager";
			this.EventSource.Attach(m_tmaxCaseCodes.EventSource);

			//	Initialize the application's pick lists
			//
			//	NOTE:	Because pick lists are heirarchial, we use a root-level
			//			parent item to wrap the child collection
			m_tmaxPickLists.UniqueId = 0;
			m_tmaxPickLists.ParentId = 0;
			m_tmaxPickLists.Name = "_root_pick_list_";
			m_tmaxPickLists.Type = TmaxPickItemTypes.MultiLevel;
				
			//	Attach the event handlers
			//
			//	NOTE:	This must be done after setting the Type property
			//			to be sure that the Children collection is valid
			m_tmaxEventSource.Attach(m_tmaxPickLists.Children.EventSource);
			
		}// public CTmaxCodesManager()
		
		/// <summary>This method resets the class members and clears the collections</summary>
		public void Clear()
		{
			//	Reset the class members
			m_strCaseId = "";
			m_strCaseVersion = "";
			m_strUserName = "";
			m_strCreatedOn = "";
			m_strCreatedBy = "";
			m_strModifiedOn = "";
			m_strModifiedBy = "";
			m_strLockHolder = "";
			m_strFileSpec = "";
			m_tmaxCaseCodes.Clear();
			m_tmaxPickLists.Clear();
				
		}// public void Clear()
		
		/// <summary>This method is called to load the file specified by the caller</summary>
		/// <param name="strFileSpec">The fully qualified path to the file being loaded</param>
		/// <returns>true if successful</returns>
		public bool Load(string strFileSpec)
		{
			bool bSuccessful = false;
			
			//	Make sure the existing values have been reset
			Terminate();
			
			try
			{
				//	Load the values stored in the configuration file
				while(bSuccessful == false)
				{
					//	Set the path to the configuration file
					m_strFileSpec = strFileSpec;
					
					//	Open the configuration file and load the collections
					if(Open(true) == false)
						break;
						
					//	Read the static case options
					m_strCaseId = m_xmlIni.Read(XMLINI_CASE_ID, "");
					m_strCaseVersion = m_xmlIni.Read(XMLINI_CASE_VERSION, "");
					m_strCreatedOn = m_xmlIni.Read(XMLINI_CREATED_ON, "");
					m_strCreatedBy = m_xmlIni.Read(XMLINI_CREATED_BY, "");
					m_strModifiedOn = m_xmlIni.Read(XMLINI_MODIFIED_ON, "");
					m_strModifiedBy = m_xmlIni.Read(XMLINI_MODIFIED_BY, "");
					m_strLockHolder = m_xmlIni.Read(XMLINI_LOCK_HOLDER, this.LockHolder);
			
					//	Close the XML
					Close();
					
					bSuccessful = true;
				
				}// while(bSuccessful == false)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Load", m_tmaxErrorBuilder.Message(ERROR_LOAD_EX, strFileSpec), Ex);
			}
					
			return bSuccessful;
		
		}// public bool Load(string strFileSpec)
		
		/// <summary>This method closes the XML file and resets the class members</summary>
		public void Terminate()
		{
			//	Make sure the file is closed
			Close();
			
			//	Reset the class members
			Clear();
				
		}// public void Terminate()
		
		/// <summary>This method is called to save the values to the XML configuration file</summary>
		/// <param name="bCollections">True to save the collections</param>
		/// <returns>True if successful</returns>
		public bool Save(bool bCollections)
		{
			bool bSuccessful = false;
			
			//	Make sure the file is opened
			if(m_xmlIni == null)
			{
				if(Open(!bCollections) == false)
					return false;
			}
			
			//	Write the collections to the file if requested
			if(bCollections == true)
			{
				//	Write the case codes to file
				if(m_tmaxCaseCodes != null)
					m_tmaxCaseCodes.Save(m_xmlIni);
					
				if((this.PickLists != null) && (this.PickLists.Children != null))
					this.PickLists.Children.Save(m_xmlIni);
					
			}// if(bCollections == true)
			
			//	Save the configuration file
			bSuccessful = Save();
			
			//	Close the file
			Close();
			
			//	Clear the modified flag if we saved the collections
			if(bCollections == true)
				m_bModified = false;
				
			return bSuccessful;
			
		}// public bool Save(bool bCollections)
		
        /// <summary>
        /// This method is called to save the values to the Txt configuration file
        /// </summary>
        /// <param name="bCollections">True to save the collections</param>
        /// <returns>returns true if successful</returns>
        public bool SaveToTextFile(bool bCollections)
        {
            bool bSuccessful = true;
            if (bCollections == true)
            {
                //	Write the case codes to file
                if (m_tmaxCaseCodes != null)
                {
                    m_textIni = (m_textIni ?? new CTextIni());
                    m_textIni.fileName = m_strFileSpec;
                    m_tmaxCaseCodes.SaveToTextFile(m_textIni);
                }

            }
            //	Close the file
            Close();

            //	Clear the modified flag if we saved the collections
            if (bCollections == true)
                m_bModified = false;

            return bSuccessful;
        }


	    /// <summary>This method is called to save the values to the specified file</summary>
		/// <param name="strFileSpec">The fully qualified path to the file</param>
		/// <param name="bCollections">True to save the collections</param>
		/// <param name="bSetFileSpec">True to change the file specification after saving</param>
		/// <returns>True if successful</returns>
		public bool SaveAs(string strFileSpec, bool bCollections, bool bSetFileSpec)
		{
			string	strCurrentFileSpec = "";
			bool	bSuccessful = false;
			
			//	Just in case this is already the file specification
			if(String.Compare(m_strFileSpec, strFileSpec, true) == 0)
				return Save(bCollections);
				
			//	Delete the specified file if it already exists
			if(System.IO.File.Exists(strFileSpec) == true)
			{
				try { System.IO.File.Delete(strFileSpec); }
				catch { return false; }
			}
			
			//	Save the current file specification if we intend to restore
			if(bSetFileSpec == false)
				strCurrentFileSpec = m_strFileSpec;
				
			//	Close the existing file
			Close();
			
			//	Change the filename and save the data
			m_strFileSpec = strFileSpec;
			//Added condition to cator .txt format as an input.
            String ext = Path.GetExtension(strFileSpec);
            if (ext == ".txt")
            {
                bSuccessful = SaveToTextFile(bCollections); 
            }
            else
            {
                bSuccessful = Save(bCollections);    
            }
		    
			
			//	Close the file and restore the filename
			if(bSetFileSpec == false)
			{
				Close();
				m_strFileSpec = strCurrentFileSpec;
			}
				
			return bSuccessful;
			
		}// public bool SaveAs(string strFileSpec, bool bCollections, bool bSetFileSpec)
		
		/// <summary>This method is called to lock the file for editing</summary>
		/// <param name="bRefresh">true to refresh the collections from file</param>
		/// <returns>True if locked</returns>
		public bool Lock(bool bRefresh)
		{
			string	strMsg = "";
			bool	bLockedOut = false;
			
			//	Make sure we have the collection
			Debug.Assert(m_tmaxCaseCodes != null);
			if(m_tmaxCaseCodes == null) return false;
			
			//	Must have a valid file specification
			if(m_strFileSpec.Length == 0) return false;
			
			//	Shouldn't have to do this but just in case...
			m_bLocked = false;
			
			//	Open the file and refresh if requested
			Open(bRefresh);

			//	Read the current lock
			m_xmlIni.SetSection(XMLINI_SECTION_NAME);
			m_strLockHolder = m_xmlIni.Read(XMLINI_LOCK_HOLDER, "");

			//	Is it locked by somebody else?
			if((m_strLockHolder.Length > 0) && (String.Compare(m_strLockHolder, GetLockId(), true) != 0))
			{
				//	Prompt the user
				strMsg = String.Format("{0} has locked the fielded data codes & pick lists for editing. If you want to override the lock it's possible that one of you may loose your changes. Do you want to override?", m_strLockHolder);
				if(MessageBox.Show(strMsg, "Locked", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					bLockedOut = true;
				}
							
			}
						
			//	Is the file available for locking?
			if(bLockedOut == false)
			{
				//	Set the lock
				m_strLockHolder = GetLockId();
				m_xmlIni.SetSection(XMLINI_SECTION_NAME);
				m_xmlIni.Write(XMLINI_LOCK_HOLDER, m_strLockHolder);
				
				Save();
				
				m_bLocked = true;
			
			}// if(bLockedOut == false)
					
			//	Make sure the file is closed
			Close();
			
			return m_bLocked;
		
		}// public CTmaxCaseCodes Lock(bool bRefresh)
		
		/// <summary>This method is called to unlock the file</summary>
		/// <param name="bSave">true to save the current contents</param>
		/// <returns>true if successful</returns>
		public bool Unlock(bool bSave)
		{
			//	Open the XML configuration file
			if(Open(false) == false) return false;
			
			//	Read the current lock
			//
			//	NOTE:	We do not assume our m_bLocked state is still valid. Another
			//			user may have come in and stepped on us
			m_xmlIni.SetSection(XMLINI_SECTION_NAME);
			m_strLockHolder = m_xmlIni.Read(XMLINI_LOCK_HOLDER, "");

			//	Should we save the file?
			if(bSave == true)
			{
				m_tmaxCaseCodes.Save(m_xmlIni);
			}
				
			//	Is the file locked?
			if(m_strLockHolder.Length > 0)
			{
				//	Is it OK for us to clear the lock?
				if(String.Compare(GetLockId(), m_strLockHolder, true) == 0)
				{
					m_xmlIni.SetSection(XMLINI_SECTION_NAME);
					m_xmlIni.Write(XMLINI_LOCK_HOLDER, "");
					
					m_strLockHolder = "";
					m_bLocked = false;
					
					Save();
					
					//	No need to save any more
					bSave = false;
				}
				
			}// if(m_strLockHolder.Length > 0)
			
			//	Do we still need to save the file
			if(bSave)
				Save();
				
			//	Make sure the file is closed
			Close();
				
			return true;
			
		}// public bool Unlock(bool bSave)
		
		/// <summary>This method is called to refresh the class members from file</summary>
		/// <returns>True if successful</returns>
		public bool Refresh()
		{
			bool bSuccessful = false;
			
			//	Make sure we have the collection
			Debug.Assert(m_tmaxCaseCodes != null);
			if(m_tmaxCaseCodes == null) return false;
			
			//	Must have a valid file specification
			if(m_strFileSpec.Length == 0) return false;
			
			//	Refresh the contents
			bSuccessful = Open(true);
			
			//	Make sure the file is closed
			Close();
			
			return bSuccessful;
		
		}// public bool Refresh()
		
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
					//	Update the Last Modified values
					if(m_strUserName.Length > 0)
						m_strModifiedBy = m_strUserName;
					m_strModifiedOn = System.DateTime.Now.ToString();
					
					//	Always update the static information
					if(m_xmlIni.SetSection(XMLINI_SECTION_NAME) == false) 
						break;
			
					m_xmlIni.Write(XMLINI_CASE_ID, m_strCaseId);
					m_xmlIni.Write(XMLINI_CASE_VERSION, m_strCaseVersion);
					m_xmlIni.Write(XMLINI_CREATED_ON, m_strCreatedOn);
					m_xmlIni.Write(XMLINI_CREATED_BY, m_strCreatedBy);
					m_xmlIni.Write(XMLINI_MODIFIED_ON, m_strModifiedOn);
					m_xmlIni.Write(XMLINI_MODIFIED_BY, m_strModifiedBy);

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
		/// <param name="bCollections">True to populate the child collections</param>
		/// <returns>true if successful</returns>
		private bool Open(bool bCollections)
		{
			bool			bSuccessful = false;
			CTmaxPickItem	tmaxPickItem = null;
			
			try
			{
				//	Make sure the existing file is closed
				Close();
			
				Debug.Assert(m_strFileSpec.Length > 0);

				//	Allocate and initialize a new XML file object
				m_xmlIni = new CXmlIni();
				m_xmlIni.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
				m_xmlIni.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
				
				m_xmlIni.XMLComments.Add("TrialMax .NET XML Case Codes Manager");
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
			
					//	Fill the collections if requested
					if((bCollections == true) && (m_tmaxCaseCodes != null))
					{
						//	Load the pick lists first
						if((m_tmaxPickLists != null) && (m_tmaxPickLists.Children != null))
							m_tmaxPickLists.Children.Load(m_xmlIni);
							
						if(m_tmaxCaseCodes != null)
						{
							m_tmaxCaseCodes.Load(m_xmlIni);
							
							//	Connect the pick lists to the codes
							if(m_tmaxPickLists != null)
							{
								foreach(CTmaxCaseCode O in m_tmaxCaseCodes)
								{
									if((O.Type == TmaxCodeTypes.PickList) && (O.PickListId > 0))
									{
										if((tmaxPickItem = m_tmaxPickLists.FindList(O.PickListId)) != null)
											O.PickList = tmaxPickItem;
									}
								
								}// foreach(CTmaxCaseCode O in m_tmaxCaseCodes)
							
							}// if(m_tmaxPickLists != null)
							
						}// if(m_tmaxCaseCodes != null)	
						
					}
						
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
		
		/// <summary>This method is called to get the id used to lock the file for editing</summary>
		/// <returns>The lock identifier</returns>
		private string GetLockId()
		{
			//	Do we have a valid user name?
			if(m_strUserName.Length > 0)
			{
				//	Do we have a valid machine name
				if(MachineName.Length > 0)
					return (m_strUserName + " ON " + m_strMachineName);
				else
					return (m_strUserName + " ON Unknown Machine");
			}
			else
			{
				//	Do we have a valid machine name
				if(MachineName.Length > 0)
					return ("Unknown User ON " + m_strMachineName);
				else
					return "Unknown User ON Unknown Machine";
			
			}// if(m_strUserName.Length > 0)
		
		}// private string GetLockId()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource;	}
		}
		
		/// <summary>Collection of case codes associated with the active case</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes; }
		}

		/// <summary>This is the application's root pick list item. The Children collection contains the top-level lists</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists; }
		}
		
		/// <summary>Record set collection used by the database to manage the case codes</summary>
		public object DxCaseCodes
		{
			get { return m_dxCaseCodes; }
			set { m_dxCaseCodes = value; }
		}

		/// <summary>Record set collection used by the database to manage the pick lists</summary>
		public object DxPickLists
		{
			get { return m_tmaxPickLists.DxRecord; }
			set { m_tmaxPickLists.DxRecord = value; }
		}
		
		/// <summary>Name of the active user</summary>
		public string UserName
		{
			get { return m_strUserName; }
			set { m_strUserName = value; }
		}

		/// <summary>Name of the active machine</summary>
		public string MachineName
		{
			get { return m_strMachineName; }
			set { m_strMachineName = value; }
		}

		/// <summary>Unique ID assigned to the active case</summary>
		public string CaseId
		{
			get { return m_strCaseId; }
			set { m_strCaseId = value; }
		}

		/// <summary>Version identifier assigned to the active case</summary>
		public string CaseVersion
		{
			get { return m_strCaseVersion; }
			set { m_strCaseVersion = value; }
		}

		/// <summary>Time of day when the file was created</summary>
		public string CreatedOn
		{
			get { return m_strCreatedOn; }
			set { m_strCreatedOn = value; }
		}
		
		/// <summary>Name of user that created the file</summary>
		public string CreatedBy
		{
			get { return m_strCreatedBy; }
			set { m_strCreatedBy = value; }
		}
		
		/// <summary>Flag to indicate that one or more of the case codes and/or pick lists has been modified</summary>
		public bool Modified
		{
			get { return m_bModified; }
			set { m_bModified = value; }
		}
		
		/// <summary>Time of day when the file was last modified</summary>
		public string ModifiedOn
		{
			get { return m_strModifiedOn; }
			set { m_strModifiedOn = value; }
		}
		
		/// <summary>Name of user that last modified the file</summary>
		public string ModifiedBy
		{
			get { return m_strModifiedBy; }
			set { m_strModifiedBy = value; }
		}
		
		/// <summary>Path to the configuration file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; } 
		}
		
		/// <summary>Name of user who has the CaseCodes collection locked</summary>
		public string LockHolder
		{
			get { return m_strLockHolder; }
			set { m_strLockHolder = value; }
		}

		/// <summary>Flag to indicate that use of pick lists is enabled / disabled</summary>
		public bool PickListsEnabled
		{
			get { return m_bPickListsEnabled; }
			set { m_bPickListsEnabled = value; }
		}
		
		#endregion Properties
	
	}// public class CTmaxCodesManager

}// namespace FTI.Shared.Trialmax
