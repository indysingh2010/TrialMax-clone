using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Database
{
	/// <summary>This class serves as the base class for all TrialMax database classes</summary>
	public class CTmaxDatabaseCompactor
	{
		#region Constants

		private const string TMAX_COMPACTOR_FOLDER_NAME = "_tmax_compacted_";
		private const string TMAX_COMPACTED_FILENAME	= "_tmax_compacted.mdb";
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX					= 0;
		private const int ERROR_EXECUTE_EX						= 1;
		private const int ERROR_SET_COMPACTOR_FOLDER_EX			= 2;
		private const int ERROR_CREATE_COMPACTOR_FOLDER_FAILED	= 3;
		private const int ERROR_SET_SOURCE_FILESPEC_EX			= 4;
		private const int ERROR_SOURCE_NOT_FOUND				= 5;
		private const int ERROR_SET_COMPACTED_FILESPEC_EX		= 6;
		private const int ERROR_DELETE_COMPACTED_FAILED			= 7;
		private const int ERROR_SET_BACKUP_FILESPEC_EX			= 8;
		private const int ERROR_SWAP_FILES_EX					= 9;
		private const int ERROR_CREATE_BACKUP_FAILED			= 10;
		private const int ERROR_RENAME_COMPACTED_FAILED			= 11;
		private const int ERROR_CHECK_SOURCE_LOCKED_EX			= 12;
		private const int ERROR_SOURCE_IS_LOCKED				= 13;
		
		#endregion Constants

		#region Protected Members

		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Class member bound to SourceFileSpec property</summary>
		protected string m_strSourceFileSpec = "";

		/// <summary>Class member to store the path to the target file</summary>
		protected string m_strCompactedFileSpec = "";

		/// <summary>Class member to store the path to the backup file</summary>
		protected string m_strBackupFileSpec = "";

		/// <summary>Fully qualified path to the compactor's working folder</summary>
		protected string m_strCompactorFolder = "";

		/// <summary>Class member bound to Successful property</summary>
		protected bool m_bSuccessful = false;

		#endregion Private Members

		#region Public Methods

		public event System.EventHandler Finished;
		
		/// <summary>Constructor</summary>
		public CTmaxDatabaseCompactor()
		{
			m_tmaxEventSource.Name = "TmaxDatabaseCompactor";

			//	Populate the error builder collection
			SetErrorStrings();

		}// public CTmaxDatabaseCompactor()

		/// <summary>This method is called to execute the compact operation</summary>
		public void Execute()
		{
			JRO.JetEngineClass	jroEngine = new JRO.JetEngineClass();
			string				strSourceConnection = "";
			string				strTargetConnection = "";

			try
			{
				//	Make sure the source file is not locked by another user
				if(CheckSourceLocked(5000) == false)
				{
					//	Format the ADO connection strings
					strSourceConnection = "Provider=Microsoft.Jet.OLEDB.4.0;";
					strSourceConnection += ("Data Source=" + m_strSourceFileSpec);

					strTargetConnection = "Provider=Microsoft.Jet.OLEDB.4.0;";
					strTargetConnection += ("Data Source=" + m_strCompactedFileSpec);

					jroEngine.CompactDatabase(strSourceConnection, strTargetConnection);

					//	Swap the compacted and original files
					m_bSuccessful = SwapFiles();
				
				}
				else
				{
					m_bSuccessful = false; // Source is locked
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Execute", m_tmaxErrorBuilder.Message(ERROR_EXECUTE_EX, this.SourceFileSpec), Ex);
			}
			finally
			{
				if(Finished != null)
					Finished(this, System.EventArgs.Empty);
			}

		}// public void Execute()

		/// <summary>This method is called to initialize the operation</summary>
		/// <param name="strSourceFileSpec">The fully qualified path to the source database</param>
		/// <returns>true if successful</returns>
		public bool Initialize(string strSourceFileSpec)
		{
			bool bSuccessful = false;

			try
			{
				while(bSuccessful == false)
				{
					//	Set the path to the source database
					if(SetSourceFileSpec(strSourceFileSpec) == false)
						break;
						
					//	Set the working folder
					if(SetCompactorFolder() == false)
						break;

					//	Set the path to the compacted file
					if(SetCompactedFileSpec() == false)
						break;

					//	Set the path to the backup file
					if(SetBackupFileSpec() == false)
						break;

					//	All done
					bSuccessful = true;

				}// while(bSuccessful == false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX, strSourceFileSpec), Ex);
			}

			return bSuccessful;

		}// public bool Initialize(string strSourceFileSpec)

		#endregion Public Methods

		#region Private Methods

		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <returns>false</returns>
		private bool Warn(string strMsg)
		{
			MessageBox.Show(strMsg, "Compactor Error", MessageBoxButtons.OK,
						    MessageBoxIcon.Exclamation);

			return false; // allows for cleaner code						

		}// private bool Warn(string strMsg)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the compact operation:\n\n%1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to compact the database: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the working folder for the compact operation:\n\n Source Database = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the working folder for the compact operation:\n\n path = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the path to the source database:\n\n path = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the specified source database:\n\npath = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the path to the compactor's target file");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to delete a previous instance of the compactor's working file: \n\n %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the path to the backup file.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to swap the original and compacted database files.");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to complete the operation. The attempt to create the backup failed:\n\n %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to complete the operation. The attempt to rename the compacted file to the original failed:\n\ncompacted = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to check the lock state of the specified database:\n\n%1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to perform the operation. The database file is being used:\n\n %1");

		}// private void SetErrorStrings()

		/// <summary>Called to set the path to the source database</summary>
		/// <param name="strFileSpec">The path to the source file</param>
		/// <returns>true if successful</returns>
		private bool SetSourceFileSpec(string strFileSpec)
		{
			bool bSuccessful = false;
			
			try
			{
				m_strSourceFileSpec = strFileSpec;
				
				//	Make sure the file exists
				if((bSuccessful = System.IO.File.Exists(m_strSourceFileSpec)) == false)
					Warn(m_tmaxErrorBuilder.Message(ERROR_SOURCE_NOT_FOUND, m_strSourceFileSpec));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSourceFileSpec", m_tmaxErrorBuilder.Message(ERROR_SET_SOURCE_FILESPEC_EX, strFileSpec), Ex);
			}
			
			return bSuccessful;

		}// private bool SetSourceFileSpec(string strFileSpec)
		
		/// <summary>Called to get the path to the compactor's working folder</summary>
		/// <returns>true if successful</returns>
		private bool SetCompactorFolder()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Get the path to the folder containing the source database
				m_strCompactorFolder = System.IO.Path.GetDirectoryName(m_strSourceFileSpec);

				//	Append the default name used for the working folder
				if((m_strCompactorFolder.Length > 0) && (m_strCompactorFolder.EndsWith("\\") == false))
					m_strCompactorFolder += "\\";
				m_strCompactorFolder += TMAX_COMPACTOR_FOLDER_NAME;
				
				//	Make sure the folder exists
				if((bSuccessful = System.IO.Directory.Exists(m_strCompactorFolder)) == false)
				{			
					try
					{
						System.IO.Directory.CreateDirectory(m_strCompactorFolder);
						bSuccessful = true;
					}
					catch
					{
						Warn(m_tmaxErrorBuilder.Message(ERROR_CREATE_COMPACTOR_FOLDER_FAILED, m_strCompactorFolder));
					}

				}// if((bSuccessful = System.IO.Directory.Exists(m_strCompactorFolder)) == false)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetCompactorFolder", m_tmaxErrorBuilder.Message(ERROR_SET_COMPACTOR_FOLDER_EX, this.SourceFileSpec), Ex);
			}
			
			return bSuccessful;

		}// private bool SetCompactorFolder()

		/// <summary>Called to set the path to the compacted database</summary>
		/// <returns>true if successful</returns>
		private bool SetCompactedFileSpec()
		{
			bool bSuccessful = true;

			try
			{
				m_strCompactedFileSpec = m_strCompactorFolder;

				if((m_strCompactedFileSpec.Length > 0) && (m_strCompactedFileSpec.EndsWith("\\") == false))
					m_strCompactedFileSpec += "\\";
				m_strCompactedFileSpec += TMAX_COMPACTED_FILENAME;

				//	Does this file already exist?
				if(System.IO.File.Exists(m_strCompactedFileSpec) == true)
				{
					//	Try to delete the file
					try
					{
						System.IO.File.Delete(m_strCompactedFileSpec);
					}
					catch
					{
						Warn(m_tmaxErrorBuilder.Message(ERROR_DELETE_COMPACTED_FAILED, m_strCompactedFileSpec));
						bSuccessful = false;
					}

				}// if(System.IO.File.Exists(m_strCompactedFileSpec) == true)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetCompactedFileSpec", m_tmaxErrorBuilder.Message(ERROR_SET_COMPACTED_FILESPEC_EX), Ex);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool SetCompactedFileSpec()

		/// <summary>Called to set the path to the backup database</summary>
		/// <returns>true if successful</returns>
		private bool SetBackupFileSpec()
		{
			bool			bSuccessful = false;
			string			strSourceFilename = "";
			string			strSourceExtension = "";
			string			strDate = "";
			System.DateTime	dtNow = System.DateTime.Now;
			try
			{
				//	Get the name of the source file
				strSourceFilename = System.IO.Path.GetFileNameWithoutExtension(m_strSourceFileSpec);
				strSourceExtension = System.IO.Path.GetExtension(m_strSourceFileSpec);

				strDate = String.Format("{0:0000}{1:00}{2:00}", dtNow.Year, dtNow.Month, dtNow.Day);
				
				for(int i = 1; i <= 10000; i++)
				{
					//	Construct the filename
					m_strBackupFileSpec = String.Format("{0}\\{1}_{2}_{3}{4}", m_strCompactorFolder, strSourceFilename, strDate, i, strSourceExtension);

					//	Make sure the file does not already exist
					if(System.IO.File.Exists(m_strBackupFileSpec) == false)
						break;
						
				}// for(int i = 1; i <= 10000; i++)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetBackupFileSpec", m_tmaxErrorBuilder.Message(ERROR_SET_BACKUP_FILESPEC_EX), Ex);
			}

			return bSuccessful;

		}// private bool SetBackupFileSpec()

		/// <summary>Called to swap the original and compacted database files</summary>
		/// <returns>true if successful</returns>
		private bool SwapFiles()
		{
			bool bSuccessful = false;

			try
			{
				//	Move the original to it's backup location
				try
				{
					System.IO.File.Move(m_strSourceFileSpec, m_strBackupFileSpec);
				}
				catch
				{
					return Warn(m_tmaxErrorBuilder.Message(ERROR_CREATE_BACKUP_FAILED, m_strBackupFileSpec));
				}

				//	Move the compacted to the original location
				try
				{
					System.IO.File.Move(m_strCompactedFileSpec, m_strSourceFileSpec);
				}
				catch
				{
					try { System.IO.File.Copy(m_strBackupFileSpec, m_strSourceFileSpec, true); }
					catch {}
					
					return Warn(m_tmaxErrorBuilder.Message(ERROR_RENAME_COMPACTED_FAILED, m_strCompactedFileSpec));
				}
	
				//	If we made it this far everything must have worked
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SwapFiles", m_tmaxErrorBuilder.Message(ERROR_SWAP_FILES_EX), Ex);
			}

			return bSuccessful;

		}// private bool SwapFiles()

		/// <summary>Called to determine if the source file is locked by another user</summary>
		/// <param name="iMilliseconds">The maximum number of ms to wait for the lock to release</param>
		/// <returns>true if the source is locked</returns>
		private bool CheckSourceLocked(int iMilliseconds)
		{
			bool	bLocked = false;
			string	strLockFileSpec = "";

			try
			{
				//	Construct the path to the lock file
				strLockFileSpec = System.IO.Path.ChangeExtension(this.SourceFileSpec, "ldb");

				while(System.IO.File.Exists(strLockFileSpec) == true)
				{
					Thread.Sleep(250);
					
					if(iMilliseconds <= 0)
					{
						bLocked = true;
						Warn(m_tmaxErrorBuilder.Message(ERROR_SOURCE_IS_LOCKED, this.SourceFileSpec));
						break;
					}
					else
					{
						iMilliseconds -= 250;
					}

				}// while(System.IO.File.Exists(strLockFileSpec) == true)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckSourceLocked", m_tmaxErrorBuilder.Message(ERROR_CHECK_SOURCE_LOCKED_EX, this.SourceFileSpec), Ex);
			}

			return bLocked;

		}// private bool CheckSourceLocked()

		#endregion Private Methods

		#region Properties

		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>Full path specification for the source database file</summary>
		public string SourceFileSpec
		{
			get { return m_strSourceFileSpec; }
		}

		/// <summary>True if the operation was successful</summary>
		public bool Successful
		{
			get { return m_bSuccessful; }
		}

		#endregion Properties

	}// public class CTmaxDatabaseCompactor

}// namespace FTI.Trialmax.Database

