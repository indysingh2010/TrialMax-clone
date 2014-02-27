using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Win32;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	public class CObjectionsDatabase : CBaseDatabase
	{
		#region Constants

		public const string OBJECTIONS_DATABASE_TEMPLATE_FILENAME = "_tmax_blank_objections.mdb";
		public const string OBJECTIONS_DATABASE_CASE_FILENAME = "_tmax_objections.mdb";

		#endregion Constants

		#region Error Identifiers

		/// <summary>Error message identifiers</summary>
		public const int ERROR_OBJECTIONS_DATABASE_CREATE_EX				= ERROR_BASE_DATABASE_LAST + 1;
		public const int ERROR_OBJECTIONS_DATABASE_CREATE_NO_TEMPLATE		= ERROR_BASE_DATABASE_LAST + 2;
		public const int ERROR_OBJECTIONS_DATABASE_GET_TEMPLATE_FILESPEC_EX	= ERROR_BASE_DATABASE_LAST + 3;
		public const int ERROR_OBJECTIONS_DATABASE_RECORDS_EXCHANGE			= ERROR_BASE_DATABASE_LAST + 4;
		public const int ERROR_OBJECTIONS_DATABASE_OPEN_COLLECTIONS_EX		= ERROR_BASE_DATABASE_LAST + 5;
		public const int ERROR_OBJECTIONS_DATABASE_ADD_CASE_EX				= ERROR_BASE_DATABASE_LAST + 6;
		public const int ERROR_OBJECTIONS_DATABASE_ADD_DEPOSITION_EX		= ERROR_BASE_DATABASE_LAST + 7;
		public const int ERROR_OBJECTIONS_DATABASE_ADD_STATE_EX				= ERROR_BASE_DATABASE_LAST + 8;
		public const int ERROR_OBJECTIONS_DATABASE_ADD_OBJECTION_EX			= ERROR_BASE_DATABASE_LAST + 9;
		public const int ERROR_OBJECTIONS_DATABASE_FILL_EX					= ERROR_BASE_DATABASE_LAST + 10;
		public const int ERROR_OBJECTIONS_DATABASE_ADD_RULING_EX			= ERROR_BASE_DATABASE_LAST + 11;
		public const int ERROR_OBJECTIONS_DATABASE_SET_FILTER_EX			= ERROR_BASE_DATABASE_LAST + 12;
		public const int ERROR_OBJECTIONS_DATABASE_GET_OX_CASE_EX			= ERROR_BASE_DATABASE_LAST + 13;
		public const int ERROR_OBJECTIONS_DATABASE_GET_OX_DEPOSITION_EX		= ERROR_BASE_DATABASE_LAST + 14;
		public const int ERROR_OBJECTIONS_DATABASE_SET_OX_RULING_EX			= ERROR_BASE_DATABASE_LAST + 15;
		public const int ERROR_OBJECTIONS_DATABASE_SET_OX_STATE_EX			= ERROR_BASE_DATABASE_LAST + 16;
		public const int ERROR_OBJECTIONS_DATABASE_DELETE_OBJECTION_EX		= ERROR_BASE_DATABASE_LAST + 17;
		public const int ERROR_OBJECTIONS_DATABASE_UPDATE_OBJECTION_EX		= ERROR_BASE_DATABASE_LAST + 18;

		#endregion Error Identifiers

		#region Private Members

		/// <summary>Local member bounded to Folder property</summary>
		private string m_strFolder = System.Environment.CurrentDirectory;

		/// <summary>Local member bounded to Filename property</summary>
		private string m_strFilename = OBJECTIONS_DATABASE_CASE_FILENAME;

		/// <summary>Local member to access the OxVersions table</summary>
		private COxVersions m_oxVersions = null;

		/// <summary>Local member to bound to OxVersion property</summary>
		private COxVersion m_oxVersion = null;

		/// <summary>Local member bounded to OxUsers property</summary>
		private COxUsers m_oxUsers = null;

		/// <summary>Local member bounded to OxUser property</summary>
		private COxUser m_oxUser = null;

		/// <summary>Local member bounded to OxCases property</summary>
		private COxCases m_oxCases = null;

		/// <summary>Local member bounded to OxCase property</summary>
		private COxCase m_oxCase = null;

		/// <summary>Local member bounded to OxDepositions property</summary>
		private COxDepositions m_oxDepositions = null;

		/// <summary>Local member bounded to OxStates property</summary>
		private COxStates m_oxStates = null;

		/// <summary>Local member bounded to OxRulings property</summary>
		private COxRulings m_oxRulings = null;

		/// <summary>Local member bounded to OxObjections property</summary>
		private COxObjections m_oxObjections = null;

		/// <summary>Local member bounded to CaseDatabase property</summary>
		private CTmaxCaseDatabase m_tmaxCaseDatabase = null;

		/// <summary>Local member bounded to Objections property</summary>
		private CTmaxObjections m_tmaxObjections = new CTmaxObjections();

		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CObjectionsDatabase() : base()
		{
			m_tmaxEventSource.Name = "Objections Database";
			
		}// public CObjectionsDatabase() : base()

		/// <summary>Call to close the database</summary>
		public override void Close()
		{
			//	Close the provider connection
			base.Close();

			m_oxVersion = null;
			m_oxUser = null;
			m_oxCase = null;
			
			//	Close the record sets
			if(m_oxVersions != null)
				m_oxVersions = m_oxVersions.Dispose();
			if(m_oxUsers != null)
				m_oxUsers = m_oxUsers.Dispose();
			if(m_oxCases != null)
				m_oxCases = m_oxCases.Dispose();
			if(m_oxDepositions != null)
				m_oxDepositions = m_oxDepositions.Dispose();
			if(m_oxObjections != null)
				m_oxObjections = m_oxObjections.Dispose();
			if(m_oxStates != null)
				m_oxStates = m_oxStates.Dispose();
			if(m_oxRulings != null)
				m_oxRulings = m_oxRulings.Dispose();
				
			//	Clear the application collections
			m_tmaxObjections.Clear();
			
			//	Reset the local members
			//
			//	NOTE:	Don't reset the Filename value because
			//			we need it to construct the path
			m_strFolder = "";

		}// public void Close()

		/// <summary>Call to open the database</summary>
		/// <param name="strFolder">Folder containing the objections database</param>
		/// <param name="strUser">Name of the user opening the database</param>
		/// <returns>true if successful</returns>
		public bool Open(string strFolder, string strUser)
		{
			//	Open the database and confirm the version information
			if(Open(strFolder, true) == false) 
				return false;

			Debug.Assert(m_oxVersions != null);
			Debug.Assert(m_oxVersion != null);

			//	Fill the local record collections
			//
			//	NOTE:	The Versions collection gets filled in response
			//			to the call to CheckDatabaseVersion()
			m_oxUsers.Fill();
			m_oxCases.Fill();
			m_oxDepositions.Fill();
			m_oxStates.Fill();
			m_oxRulings.Fill();

			//	We don't fill the Objections until the owner sets the filter
			//m_oxObjections.Fill();
				
			//	Assign the active user
			SetUser(strUser);

			return true;

		}// public bool Open(string strFolder, string strUser)

		/// <summary>Call to create the database</summary>
		/// <param name="strFolder">Folder containing the objections database</param>
		/// <param name="strUser">Name of the user creating the database</param>
		/// <returns>true if successful</returns>
		public bool Create(string strFolder,string strUser)
		{
			string	strTemplate = "";
			string	strTarget = "";
			bool	bSuccessful = false;

			try
			{
				while(bSuccessful == false)
				{
					//	Build the file specification for the template database
					strTemplate = GetTemplateFileSpec();
					if((strTemplate == null) || (strTemplate.Length == 0))
						break;

					//	Make sure the template file exists
					if(FindFile(strTemplate) == false)
					{
						FireError(this,"Create",this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_CREATE_NO_TEMPLATE,strTemplate));
						break;
					}

					//	Get the full path specification for the target file
					strTarget = GetFileSpec(strFolder);
					if((strTarget == null) || (strTarget.Length == 0))
						break;

					//	Make sure the folder exists
					if(System.IO.Directory.Exists(strFolder) == false)
						System.IO.Directory.CreateDirectory(strFolder);

					//	Assume the caller has already checked for overwrite
					//	permission
					System.IO.File.Copy(strTemplate,strTarget,true);

					//	Make sure it is not read-only
					System.IO.File.SetAttributes(strTarget,System.IO.FileAttributes.Normal);

					//	Now open the datebase for operation
					if(Open(strFolder,false) == false)
						break;

					//	Add the version information
					if(SetVersions() == false)
						break;

					//	Add the user information
					if(SetUser(strUser) == false)
						break;
						
					//	The template is pre-loaded with default types
					m_oxStates.Fill();
					m_oxRulings.Fill();
					
					//	We're done
					bSuccessful = true;

				}// while(bSuccessful == false)
				
			}
			catch(System.Exception Ex)
			{
				FireError(this,"Create",this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_CREATE_EX,strTemplate,strTarget),Ex);
			}

			//	Clean up if an error occurred
			if(bSuccessful == false)
				Close();

			return bSuccessful;

		}// public bool Create(string strFolder,string strUser)

		/// <summary>This method will build the fully qualified path specification for the database file using the specified folder</summary>
		/// <param name="strFolder">Folder containing the objections database</param>
		/// <returns>The fully qualified path to the database file</returns>
		public string GetFileSpec(string strFolder)
		{
			string strFileSpec = "";

			if((strFolder != null) && (strFolder.Length > 0))
			{
				strFileSpec = strFolder;
			}
			else
			{
				strFileSpec = System.Environment.CurrentDirectory;
			}

			if(strFileSpec.EndsWith("\\") == false)
				strFileSpec += "\\";

			//	Assign default filename if not set by the owner object
			if(m_strFilename.Length == 0)
				m_strFilename = OBJECTIONS_DATABASE_CASE_FILENAME;

			//	Add the filename
			strFileSpec += m_strFilename;

			return strFileSpec;

		}//	public string GetFileSpec(string strFolder)

		/// <summary>This method is called to get the id of the current user</summary>
		/// <returns>The Id of the current user if available</returns>
		public System.Guid GetUserId()
		{
			if(m_oxUser != null)
				return m_oxUser.UniqueId;
			else
				return System.Guid.Empty;

		}// public long GetUserId()

		/// <summary>This method is called to get the name of the current user</summary>
		/// <returns>The name of the current user if available</returns>
		public string GetUserName()
		{
			if(m_oxUser != null)
				return m_oxUser.Name;
			else
				return "";

		}// public string GetUserName()

		/// <summary>This method is called to get the name of the user with the specified id</summary>
		/// <returns>The name of the specified user</returns>
		public string GetUserName(System.Guid uniqueId)
		{
			if(m_oxUsers != null)
			{
				foreach(COxUser O in m_oxUsers)
				{
					if(O.UniqueId == System.Guid.Empty)
						return O.Name;
				}
			}

			return "";

		}// public string GetUserName(long lId)

		/// <summary>This method is called to set the active case</summary>
		/// <param name="tmaxCaseDatabase">The TrialMax case database</param>
		/// <returns>true if successful</returns>
		public bool SetCase(CTmaxCaseDatabase tmaxCaseDatabase)
		{
			bool bSuccessful = true;
			
			m_oxCase = null;

			if(tmaxCaseDatabase != null)
			{
				//	Search for the database in our existing collection
				if((m_oxCase = m_oxCases.Find(tmaxCaseDatabase.CaseName, true)) == null)
				{
					//	Add this case to the database
					m_oxCase = AddCase(tmaxCaseDatabase);
				}
				
				bSuccessful = (m_oxCase != null);

			}// if(m_tmaxCaseDatabase != null)
			
			return bSuccessful;

		}// public bool SetCase(CTmaxCaseDatabase tmaxCaseDatabase)

		/// <summary>Called to fill the objections collection</summary>
		/// <returns>true if successful</returns>
		public bool Fill()
		{
			bool bSuccessful = false;
			
			try
			{
				Debug.Assert(m_oxObjections != null);
				if(m_oxObjections == null) return false;
				Debug.Assert(m_tmaxObjections != null);
				if(m_tmaxObjections == null) return false;

				m_tmaxObjections.Clear();
				m_oxObjections.Clear();
				
				m_oxObjections.Fill();

				foreach(COxObjection O in m_oxObjections)
				{
					O.TmaxObjection.IOxObjection = O;
					m_tmaxObjections.Add(O.TmaxObjection);
				}

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				FireError(this, "Fill", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_FILL_EX), Ex);
			}

			return bSuccessful;

		}// public bool Fill()
		
		/// <summary>This method is called to add a deposition to the database</summary>
		/// <param name="dxDeposition">The exchange interface for the deposition record in the case database</param>
		/// <returns>The exchange interface for the new deposition if successful</returns>
		public COxDeposition AddDeposition(CDxPrimary dxDeposition)
		{
			COxDeposition oxDeposition = null;

			try
			{
				Debug.Assert(m_oxDepositions != null);
				if(m_oxDepositions == null) return null;
				
				Debug.Assert(dxDeposition != null);
				if(dxDeposition == null) return null;

				//	We need the transcript record
				if(dxDeposition.Transcript == null)
					return null;
					
				oxDeposition = new COxDeposition();

				oxDeposition.MediaId   = dxDeposition.MediaId;
				oxDeposition.Deponent  = dxDeposition.Transcript.Deponent;
				oxDeposition.Filename  = dxDeposition.Transcript.Filename;
				oxDeposition.FirstPL   = dxDeposition.Transcript.FirstPL;
				oxDeposition.LastPL    = dxDeposition.Transcript.LastPL;
				oxDeposition.CreatedOn = System.DateTime.Now;

				try { oxDeposition.DeposedOn = System.DateTime.Parse(dxDeposition.Transcript.DeposedOn); }
				catch { }

				oxDeposition = m_oxDepositions.Add(oxDeposition);

			}
			catch(System.Exception Ex)
			{
				FireError(this, "AddDeposition", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_ADD_DEPOSITION_EX), Ex);
				oxDeposition = null;
			}

			return oxDeposition;

		}// public COxDeposition AddDeposition(CDxPrimary dxDeposition)

		/// <summary>This method is called to add an objection type to the database</summary>
		/// <param name="strLabel">The type label</param>
		/// <returns>The exchange interface for the new type if successful</returns>
		public COxState AddState(string strLabel)
		{
			COxState oxState = null;

			try
			{
				Debug.Assert(m_oxStates != null);
				if(m_oxStates == null) return null;

				oxState = new COxState();

				oxState.Label = strLabel;
				oxState.CreatedOn = System.DateTime.Now;

				oxState = m_oxStates.Add(oxState);

			}
			catch(System.Exception Ex)
			{
				FireError(this, "AddState", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_ADD_STATE_EX), Ex);
				oxState = null;
			}

			return oxState;

		}// public COxState AddState(CDxPrimary dxState)

		/// <summary>This method is called to add an objection type to the database</summary>
		/// <param name="strLabel">The type label</param>
		/// <returns>The exchange interface for the new type if successful</returns>
		public COxRuling AddRuling(string strLabel)
		{
			COxRuling oxRuling = null;

			try
			{
				Debug.Assert(m_oxRulings != null);
				if(m_oxRulings == null) return null;

				oxRuling = new COxRuling();

				oxRuling.Label = strLabel;
				oxRuling.CreatedOn = System.DateTime.Now;

				oxRuling = m_oxRulings.Add(oxRuling);

			}
			catch(System.Exception Ex)
			{
				FireError(this, "AddRuling", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_ADD_RULING_EX), Ex);
				oxRuling = null;
			}

			return oxRuling;

		}// public COxRuling AddRuling(string strLabel)

		/// <summary>This method is called to add an objection to the database</summary>
		/// <param name="tmaxObjection">The application objection object</param>
		/// <returns>The exchange interface for the new record if successful</returns>
		public COxObjection AddObjection(CTmaxObjection tmaxObjection)
		{
			COxObjection oxObjection = null;

			try
			{
				Debug.Assert(m_oxObjections != null);
				if(m_oxObjections == null) return null;
				Debug.Assert(m_tmaxObjections != null);
				if(m_tmaxObjections == null) return null;

				//	Do we need to set the deposition?
				if(tmaxObjection.IOxDeposition == null)
				{
					//	We must have a case deposition
					if(tmaxObjection.ICaseDeposition != null)
					{
						if((tmaxObjection.IOxDeposition = m_oxDepositions.Find(tmaxObjection.Deposition, true)) == null)
						{
							tmaxObjection.IOxDeposition = AddDeposition(tmaxObjection.ICaseDeposition as CDxPrimary);
						}

					}// if(tmaxObjection.ITmaxDeposition != null)
					
				}
				if(tmaxObjection.IOxDeposition == null) return null; // Can't add
				
				oxObjection = new COxObjection(tmaxObjection);

				//	Set the user and time stamp
				oxObjection.ModifiedOn = System.DateTime.Now;
				if(this.OxUser != null)
					oxObjection.OxModifiedBy = this.OxUser;

				//	Set the case id if necessary
				if((tmaxObjection.CaseId.Length == 0) && (this.OxCase != null))
					tmaxObjection.Case = this.OxCase.TmaxCase;

				if((oxObjection = m_oxObjections.Add(oxObjection)) != null)
				{
					//	Cross link the application object and the exchange interface
					tmaxObjection.IOxObjection = oxObjection;
					
					//	Add to the application collection
					m_tmaxObjections.Add(tmaxObjection);

				}// if((oxObjection = m_oxObjections.Add(oxObjection)) != null)

			}
			catch(System.Exception Ex)
			{
				FireError(this, "AddObjection", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_ADD_OBJECTION_EX), Ex);
				oxObjection = null;
			}

			return oxObjection;

		}// public COxObjection AddObjection(CTmaxObjection tmaxObjection)

		/// <summary>This method will set the filter used to populate the Filtered record collection</summary>
		/// <param name="tmaxItems">Event items used to identify case codes to be used</param>
		/// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
		/// <returns>true if successful</returns>
		public bool SetFilter(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			bool			bSuccessful = false;
			
			try
			{
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				FireError(this, "SetFilter", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_SET_FILTER_EX), Ex);
			}
			
			return bSuccessful;

		}// public bool SetFilter(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

		/// <summary>Called to set the active objection ruling</summary>
		/// <param name="tmaxObjection">The objection to be set</param>
		/// <param name="bAdd"</param>True if OK to add a new ruling
		/// <returns>true if successful</returns>
		public bool SetOxRuling(CTmaxObjection tmaxObjection, bool bAdd)
		{
			COxRuling oxRuling = null;
			
			try
			{
				//	Do we have a valid label?
				if(tmaxObjection.Ruling.Length > 0)
				{
					//	Does it exist in the objections database?
					if((oxRuling = m_oxRulings.Find(tmaxObjection.Ruling, true)) == null)
					{
						//	We have to add a ruling to the database
						if(bAdd == true)
						{
							oxRuling = AddRuling(tmaxObjection.Ruling);
						}

					}// if((m_oxRuling = m_tmaxObjectionsDatabase.Rulings.Find(strLabel)) == null)

				}// if(strLabel.Length > 0)
				
				//	Assign the default ruling if not found
				if((oxRuling == null) && (m_oxRulings.Count > 0))
					oxRuling = m_oxRulings[0];
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetOxRuling", m_tmaxErrorBuilder.Message(ERROR_OBJECTIONS_DATABASE_SET_OX_RULING_EX), Ex);
			}

			//	Make sure the objection has the correct record
			if(oxRuling != null)
				tmaxObjection.IOxRuling = oxRuling;
				
			return (oxRuling != null);

		}// public bool SetRuling(CTmaxObjection tmaxObjection)

		/// <summary>Called to set the active objection state</summary>
		/// <param name="tmaxObjection">The objection to be set</param>
		/// <param name="bAdd"</param>True if OK to add a new state
		/// <returns>true if successful</returns>
		public bool SetOxState(CTmaxObjection tmaxObjection, bool bAdd)
		{
			COxState oxState = null;

			try
			{
				//	Do we have a valid label?
				if(tmaxObjection.State.Length > 0)
				{
					//	Does it exist in the objections database?
					if((oxState = m_oxStates.Find(tmaxObjection.State, true)) == null)
					{
						//	We have to add a state to the database
						if(bAdd == true)
						{
							oxState = AddState(tmaxObjection.State);
						}

					}// if((m_oxState = m_tmaxObjectionsDatabase.States.Find(strLabel)) == null)

				}// if(strLabel.Length > 0)

				//	Assign the default state if not found
				if((oxState == null) && (m_oxStates.Count > 0))
					oxState = m_oxStates[0];
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetOxState", m_tmaxErrorBuilder.Message(ERROR_OBJECTIONS_DATABASE_SET_OX_STATE_EX), Ex);
			}

			//	Make sure the objection has the correct record
			if(oxState != null)
				tmaxObjection.IOxState = oxState;

			return (oxState != null);

		}// public bool SetState(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to delete an objection in the database</summary>
		/// <param name="oxObjection">The exchange interface to the desired record</param>
		/// <returns>true if successful</returns>
		public bool DeleteObjection(COxObjection oxObjection)
		{
			bool bSuccessful = false;

			try
			{
				Debug.Assert(m_oxObjections != null);
				if(m_oxObjections == null) return false;
				Debug.Assert(m_tmaxObjections != null);
				if(m_tmaxObjections == null) return false;

				//	Remove from the database
				m_oxObjections.Delete(oxObjection);
				
				//	Remove from the master collection
				if(oxObjection.TmaxObjection != null)
					m_tmaxObjections.Remove(oxObjection.TmaxObjection);
					
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				FireError(this, "DeleteObjection", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_DELETE_OBJECTION_EX), Ex);
			}

			return bSuccessful;

		}// public bool DeleteObjection(COxObjection oxObjection)

		/// <summary>This method is called to update an objection in the database</summary>
		/// <param name="oxObjection">The exchange interface to the desired record</param>
		/// <returns>true if successful</returns>
		public bool UpdateObjection(COxObjection oxObjection)
		{
			bool bSuccessful = false;

			try
			{
				Debug.Assert(m_oxObjections != null);
				if(m_oxObjections == null) return false;
				Debug.Assert(m_tmaxObjections != null);
				if(m_tmaxObjections == null) return false;

				//	Remove from the database
				m_oxObjections.Update(oxObjection);

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				FireError(this, "UpdateObjection", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_UPDATE_OBJECTION_EX), Ex);
			}

			return bSuccessful;

		}// public bool UpdateObjection(COxObjection oxObjection)

		#endregion Public Methods

		#region Protected Methods

		/// <summary>This method is called to populate the error builder's format string collection</summary>
        protected override void SetErrorStrings()
        {
            if(m_tmaxErrorBuilder == null) return;
            if(m_tmaxErrorBuilder.FormatStrings == null) return;

            //	Let the base class add its strings first
            base.SetErrorStrings();

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the new database: template = %1  target = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the blank template for the objections database: path = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to construct the path to the objections database template.");
			m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while exchanging fields in the %1 table. bSetFields = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the record collections.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a case to the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a deposition to the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a state identifier to the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add an objection to the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the objections collection.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a ruling to the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the record filter.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to locate the specified case: Id=%1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to locate the deposition: MediaId=%1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the ruling identifier for the objection");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the status identifier for the objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete the specified objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the specified objection");

		}// protected override void SetErrorStrings()
		
		#endregion Protected Methods
		
		#region Private Methods

		/// <summary>Call to open the database</summary>
		/// <param name="strFolder">Folder containing the objections database</param>
		/// <param name="bCheckVersion">True to verify the version information stored in the database</param>
		/// <returns>true if successful</returns>
		private bool Open(string strFolder, bool bCheckVersion)
		{
			string	strFileSpec = "";
			bool	bSuccessful = false;

			//	Get the fully qualified path for the database file
			strFileSpec = GetFileSpec(strFolder);

			//	Close the existing database connection
			Close();

			//	Connect to the data provider
			if(Open(strFileSpec) == true) 
			{
				//	Save the new folder location
				m_strFolder = strFolder.ToLower();

				//	Open the record collections
				bSuccessful = OpenCollections(bCheckVersion);
			
			}
			
			return bSuccessful;

		}// private bool Open(string strFolder, bool bCheckVersion)

		/// <summary>Called locally to open the record collections</summary>
		/// <param name="bCheckVersion">True to check the database version information</param>
		/// <returns>true if successful</returns>
		private bool OpenCollections(bool bCheckVersion)
		{
			bool bSuccessful = false;

			try
			{
				//	Allocate the record collections
				m_oxVersions = new COxVersions(this);
				m_oxUsers = new COxUsers(this);
				m_oxCases = new COxCases(this);
				m_oxDepositions = new COxDepositions(this);
				m_oxStates = new COxStates(this);
				m_oxRulings = new COxRulings(this);
				m_oxObjections = new COxObjections(this);

				//	Open the record collections
				while(bSuccessful == false)
				{
					if(m_oxVersions.Open() == false)
						break;

					if(bCheckVersion == true)
					{
						if(CheckVersions() == false)
							break;
					}

					if(m_oxUsers.Open() == false)
						break;

					if(m_oxCases.Open() == false)
						break;

					if(m_oxDepositions.Open() == false)
						break;

					if(m_oxStates.Open() == false)
						break;

					if(m_oxRulings.Open() == false)
						break;

					if(m_oxObjections.Open() == false)
						break;

					//	We're finished
					bSuccessful = true;

				}// while(bSuccessful == false)
			}
			catch(OleDbException oleEx)
			{
				FireError(this,"OpenCollections",this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_OPEN_COLLECTIONS_EX),oleEx);
			}
			catch(System.Exception Ex)
			{
				FireError(this,"OpenCollections",this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_OPEN_COLLECTIONS_EX),Ex);
			}

			//	Clean up if not successful
			if(bSuccessful == false)
			{
				Close();
			}

			return bSuccessful;

		}// private bool OpenCollections(bool bCheckVersion)

		/// <summary>Call to get the fully qualified path to the template used to create a new database</summary>
		/// <param name="strFolder">Folder containing the objections database</param>
		/// <param name="strUser">Name of the user creating the database</param>
		/// <returns>true if successful</returns>
		private string GetTemplateFileSpec()
		{
			string strTemplate = "";

			try
			{
				if((Application.ExecutablePath != null) && (Application.ExecutablePath.Length > 0))
					strTemplate = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
					
				if(strTemplate.Length == 0)
					strTemplate = System.Environment.CurrentDirectory;
				
				if(strTemplate.EndsWith("\\") == false)
					strTemplate += "\\";
				
				strTemplate += OBJECTIONS_DATABASE_TEMPLATE_FILENAME;
				
			}
			catch(System.Exception Ex)
			{
				FireError(this,"GetTemplateFileSpec",this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_GET_TEMPLATE_FILESPEC_EX),Ex);
				strTemplate = "";
			}
			

			return strTemplate;

		}// private string GetTemplateFileSpec()

		/// <summary>Called to check the current version to see if any upgrades are required</summary>
		/// <returns>true if successful</returns>
		private bool CheckVersions()
		{
			bool bSuccessful = false;

			Debug.Assert(m_oxVersions != null);
			if(m_oxVersions == null) return false;

			//	Populate the versions collection
			m_oxVersions.Fill();
			if(m_oxVersions.Count > 0)
			{
				m_oxVersion = m_oxVersions[0];
				bSuccessful = true;
			}
			else
			{
				//	We should never reach this point but just in case
				//	we do we are going to add the version information and just
				//	cross our fingers that everything is OK
				bSuccessful = SetVersions();
			}

			return bSuccessful;

		}// private bool CheckVersions()

		/// <summary>Called to populate the Version information for the database</summary>
		/// <returns>true if successful</returns>
		private bool SetVersions()
		{
			CTmdataVersion dbVer = new CTmdataVersion();

			Debug.Assert(m_oxVersion == null);
			Debug.Assert(m_oxVersions != null);

			if(m_oxVersions != null)
			{
				//	Allocate a new transfer object
				if((m_oxVersion = new COxVersion()) != null)
				{
					m_oxVersion.UniqueId = System.Guid.NewGuid().ToString();
					m_oxVersion.VerMajor = dbVer.Major;
					m_oxVersion.VerMinor = dbVer.Minor;
					m_oxVersion.VerQEF   = dbVer.QEF;
					m_oxVersion.VerBuild = dbVer.Build;

					return (m_oxVersions.Add(m_oxVersion) != null);

				}// if((m_oxVersion = new COxVersion()) != null)

			}// if(m_oxVersions != null)

			return false;

		}// private bool SetVersions()

		/// <summary>This method is called to set the active user</summary>
		/// <param name="strUser">Name of the current user</param>
		/// <returns>true if successful</returns>
		private bool SetUser(string strUser)
		{
			Debug.Assert(m_oxUsers != null);
			Debug.Assert(m_oxUser == null);

			//	Search for the specified user
			if(m_oxUsers.Count > 0)
				m_oxUser = m_oxUsers.Find(strUser, true);
				
			//	Has this user already been added to the database?
			if(m_oxUser != null)
			{
				//	Update the user time stamp
				m_oxUser.LastTime = System.DateTime.Now;
				m_oxUsers.Update(m_oxUser);
			}
			else
			{
				//	Add this user to the database
				m_oxUser = new COxUser();

				m_oxUser.Name = strUser;
				m_oxUser.LastTime = System.DateTime.Now;

				m_oxUser = m_oxUsers.Add(m_oxUser);

			}// if((m_oxUser = m_oxUsers.Find(strUser)) != null)

			return (m_oxUser != null);

		}// private bool SetUser(string strUser)

		/// <summary>This method is called to add a case descriptor to the database</summary>
		/// <param name="strCaseId">The unique TrialMax identifier assigned to the case</param>
		/// <param name="strCaseName">The name assigned to the case</param>
		/// <param name="strCaseVersion">The case's version descriptor</param>
		/// <returns>The exchange interface for the new case if successful</returns>
		public COxCase AddCase(string strCaseId, string strCaseName, string strCaseVersion)
		{
			COxCase oxCase = null;

			try
			{
				Debug.Assert(m_oxCases != null);

				if(m_oxCases != null)
				{
					oxCase = new COxCase();

					oxCase.UniqueId = strCaseId;
					oxCase.Name = strCaseName;
					oxCase.Version = strCaseVersion;
					oxCase.CreatedOn = System.DateTime.Now;

					oxCase = m_oxCases.Add(oxCase);

				}// if(m_oxCases != null)

			}
			catch(System.Exception Ex)
			{
				FireError(this, "AddCase", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_ADD_CASE_EX), Ex);
				oxCase = null;
			}

			return oxCase;

		}// public COxCase AddCase(string strCaseId, string strCaseName, string strCaseVersion)

		/// <summary>This method is called to add a case descriptor to the database</summary>
		/// <param name="tmaxCaseDatabase">The TrialMax case to be added</param>
		/// <returns>The exchange interface for the new case if successful</returns>
		public COxCase AddCase(CTmaxCaseDatabase tmaxCaseDatabase)
		{
			return AddCase(tmaxCaseDatabase.GetCaseId(), tmaxCaseDatabase.GetCaseName(), tmaxCaseDatabase.GetVersionString(false));
		}

		/// <summary>This method is called to add a case descriptor to the database</summary>
		/// <param name="xmlObjections">The objections owned by the case</param>
		/// <returns>The exchange interface for the new case if successful</returns>
		public COxCase AddCase(CXmlObjections xmlObjections)
		{
			return AddCase(xmlObjections.CaseId, xmlObjections.CaseName, xmlObjections.CaseVersion);
		}

		/// <summary>This method is called to get the case associated with the specified collection</summary>
		/// <param name="xmlObjections">The collection of objections associated with the desired case</param>
		/// <returns>The exchange interface for the associated case record</returns>
		public COxCase GetOxCase(CXmlObjections xmlObjections, bool bAdd)
		{
			COxCase oxCase = null;
			
			try
			{
				//	Has a case been assigned to the collection?
				if((xmlObjections.CaseName.Length > 0) || (xmlObjections.CaseId.Length > 0))
				{
					//	First search for the case by name
					if(xmlObjections.CaseName.Length > 0)
						oxCase = m_oxCases.Find(xmlObjections.CaseName, true);

					//	Should we search by CaseId?
					if((oxCase == null) && (xmlObjections.CaseId.Length > 0))
						oxCase = m_oxCases.Find(xmlObjections.CaseId, false);

					//	Should we add the case?
					if((oxCase == null) && (bAdd == true))
						oxCase = AddCase(xmlObjections);
				}
				else
				{
					if((oxCase = this.OxCase) != null)
					{
						xmlObjections.CaseId = oxCase.UniqueId;
						xmlObjections.CaseName = oxCase.Name;
						xmlObjections.CaseVersion = oxCase.Version;
					}

				}// if(xmlObjections.CaseId.Length > 0)
					
			}
			catch(System.Exception Ex)
			{
				FireError(this, "GetOxCase", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_GET_OX_CASE_EX, xmlObjections.CaseId), Ex);
				oxCase = null;
			}

			return oxCase;

		}// public COxCase GetOxCase(CXmlObjections xmlObjections)

		/// <summary>This method is called to get the matching deposition record from the Depositions table</summary>
		/// <param name="dxDeposition">The deposition record stored in the case database</param>
		/// <param name="bAdd">True to add if not found</param>
		/// <returns>The exchange interface for the associated deposition record</returns>
		public COxDeposition GetOxDeposition(CDxPrimary dxDeposition, bool bAdd)
		{
			COxDeposition oxDeposition = null;

			Debug.Assert(dxDeposition != null);
			Debug.Assert(m_oxDepositions != null);
			
			try
			{
				if(m_oxDepositions != null)
				{
					if((oxDeposition = m_oxDepositions.Find(dxDeposition.MediaId, true)) == null)
					{
						//	Add the deposition to the database
						oxDeposition = AddDeposition(dxDeposition);
					}

				}// if(m_oxDepositions != null)
		
			}
			catch(System.Exception Ex)
			{
				FireError(this, "GetOxDeposition", this.ExBuilder.Message(ERROR_OBJECTIONS_DATABASE_GET_OX_DEPOSITION_EX, dxDeposition.MediaId), Ex);
			}

			return oxDeposition;

		}// public COxDeposition GetOxDeposition(CDxPrimary dxDeposition, bool bAdd)

		#endregion Private Methods

		#region Properties 

		/// <summary>The active case database</summary>
		public CTmaxCaseDatabase CaseDatabase
		{
			get { return m_tmaxCaseDatabase; }
			set { m_tmaxCaseDatabase = value; }
		}

		/// <summary>Name of the database file</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value; }
		}

		/// <summary>Folder where the database file is stored</summary>
		/// <remarks>This property is read only. It will be set in the call to Open()</remarks>
		public string Folder
		{
			get { return m_strFolder; }
		}

		/// <summary>This property exposes the information stored in the Versions table</summary>
		public COxVersion OxVersion
		{
			get { return m_oxVersion; }
		}

		/// <summary>This property exposes the current user</summary>
		public COxUser OxUser
		{
			get { return m_oxUser; }
		}

		/// <summary>This property exposes the users collection</summary>
		public COxUsers OxUsers
		{
			get { return m_oxUsers; }
		}

		/// <summary>This property exposes the current case</summary>
		public COxCase OxCase
		{
			get { return m_oxCase; }
		}

		/// <summary>This property exposes the users record set collection</summary>
		public COxCases OxCases
		{
			get { return m_oxCases; }
		}

		/// <summary>This property exposes the Depositions record set collection</summary>
		public COxDepositions OxDepositions
		{
			get { return m_oxDepositions; }
		}

		/// <summary>This property exposes the States record set collection</summary>
		public COxStates OxStates
		{
			get { return m_oxStates; }
		}

		/// <summary>This property exposes the Rulings record set collection</summary>
		public COxRulings OxRulings
		{
			get { return m_oxRulings; }
		}

		/// <summary>This property exposes the Objections record set collection</summary>
		public COxObjections OxObjections
		{
			get { return m_oxObjections; }
		}

		/// <summary>This property exposes the application's Objections collection</summary>
		public CTmaxObjections Objections
		{
			get { return m_tmaxObjections; }
		}

		#endregion Properties


	}// class CObjectionsDatabase : CBaseDatabase

}// namespace FTI.Trialmax.Database
