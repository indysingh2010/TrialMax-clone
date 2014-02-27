using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Database
{
    /// <summary>This class manages a trim database operation</summary>
    public class CTmaxTrimManager
    {
        #region Constants

		private const string TMAX_DATABASE_FILENAME = "_tmax_case.mdb";
		private const string TMAX_CONFIGURATION_FILENAME = "_tmax_case.xml";
		
		/// <summary>Error message identifiers</summary>
        private const int ERROR_INITIALIZE_EX			= 0;
        private const int ERROR_TRIM_EX					= 1;
		private const int ERROR_CREATE_PROGRESS_FORM_EX = 2;
		private const int ERROR_COPY_SOURCE_EX			= 3;
		private const int ERROR_OPEN_TRIMMED_EX			= 4;
		private const int ERROR_ADD_SELECTIONS_EX		= 5;
		private const int ERROR_CHECK_RECORD_EX			= 6;
		private const int ERROR_DELETE_RECORDS_EX		= 7;
		private const int ERROR_TRANSFER_FILES_EX		= 8;
		private const int ERROR_TRANSFER_PRIMARY_EX		= 9;
		private const int ERROR_CREATE_FOLDER_FAILED	= 10;
		private const int ERROR_NO_TRANSCRIPT			= 11;
		private const int ERROR_COPY_FAILED				= 12;
		private const int ERROR_CHECK_BINDER_EX			= 13;
		private const int ERROR_ADD_SCRIPTS_EX			= 14;
		private const int ERROR_ADD_TREATMENTS_EX		= 15;
		private const int ERROR_ADD_BINDERS_EX			= 16;

        #endregion Constants

        #region Private Members

        /// <summary>Local member bounded to EventSource property</summary>
        protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

        /// <summary>Local member used to construct error messages</summary>
        protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

        /// <summary>Local member bound to Database property</summary>
        private CTmaxCaseDatabase m_tmaxDatabase = null;

		/// <summary>Local member bound to Trimmed property</summary>
		private CTmaxCaseDatabase m_tmaxTrimmed = null;

		/// <summary>Local member bound to Options property</summary>
        protected CTmaxTrimOptions m_tmaxTrimOptions = null;

        /// <summary>Local member bound to StatusForm property</summary>
        private FTI.Trialmax.Forms.CFTrimProgress m_wndProgress = null;

        /// <summary>Local class member bound to Cancelled property</summary>
        private bool m_bCancelled = false;

		/// <summary>Local class member to store the path to the fully qualified source database</summary>
		private CTmaxSourceFolder m_tmaxSourceFolder = new CTmaxSourceFolder();

		/// <summary>Local class member to store the path to the fully qualified trimmed database</summary>
		private CTmaxSourceFolder m_tmaxTrimmedFolder = new CTmaxSourceFolder();

		/// <summary>Local class member to store the path to the fully qualified trimmed database file</summary>
		private string m_strTrimmedFileSpec = "";

		/// <summary>Local class member to store the barcode of the active record</summary>
		private string m_strBarcode = "";

		/// <summary>Local class member to store all primary script records</summary>
		private CDxPrimaries m_dxScripts = null;

		/// <summary>Local class member to store all primary document records</summary>
		private CDxPrimaries m_dxDocuments = null;

		/// <summary>Local class member to store all primaries that are not scripts</summary>
		private CDxPrimaries m_dxOthers = null;

		/// <summary>Local class member to store all primary records that are references and should be retained</summary>
		private CDxPrimaries m_dxAllUsed = null;

		/// <summary>Local class member to store all tertiary records that we want to keep</summary>
		private CDxTertiaries m_dxDesignations = null;

		/// <summary>Local class member to store all page records</summary>
		private CDxSecondaries m_dxPages = null;

		/// <summary>Local class member to store all binder records</summary>
		private CDxBinderEntries m_dxBinders = null;

		#endregion Private Members

        #region Public Methods

        /// <summary>Constructor</summary>
        public CTmaxTrimManager()
        {
            //	Initialize the event source and error builder
            SetErrorStrings();
            m_tmaxEventSource.Name = "Trim Manager";

        }// public CTmaxTrimManager()

        /// <summary>This method uses the specified parameters to set the associated properties</summary>
        /// <param name="tmaxParameters">The collection of parameters</param>
		/// <returns>True if successful</returns>
        public bool Initialize(CTmaxParameters tmaxParameters)
        {
			bool bSuccessful = false;
			
            try
            {
				//	Reset the current values to their defaults
				Clear();

				//	Do we have any source records to be retained?
				if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null) && (m_tmaxDatabase.Primaries.Count > 0))
				{
					//	Allocate the collections needed for the operation
					m_dxAllUsed = new CDxPrimaries();
					m_dxScripts = new CDxPrimaries();
					m_dxDocuments = new CDxPrimaries();
					m_dxOthers = new CDxPrimaries();
					m_dxDesignations = new CDxTertiaries();
					m_dxPages = new CDxSecondaries();
					m_dxBinders = new CDxBinderEntries();
					
					//	Separate the source records to speed up processing
					foreach(CDxPrimary O in m_tmaxDatabase.Primaries)
					{
						if(O.MediaType == TmaxMediaTypes.Script)
							m_dxScripts.AddList(O);
						else if(O.MediaType == TmaxMediaTypes.Document)
							m_dxDocuments.AddList(O);
						else
							m_dxOthers.AddList(O);

					}// foreach(CDxPrimary O in m_tmaxDatabase.Primaries)

					bSuccessful = true;
				}
				else
				{
					MessageBox.Show("No primary records available in the source database", "Trim", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				
            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
            }
            
            return bSuccessful;

        }// public void Initialize(CTmaxParameters tmaxParameters)

        /// <summary>This method is called to execute the operation</summary>
        /// <returns>true if successful</returns>
        public bool Trim()
        {
            System.Threading.Thread trimThread = null;
            int iAttempts = 0;
            bool bSuccessful = false;

            //	Create the status form for the operation
            if (this.CreateProgressForm() == false) return false;

            try
            {
                //	Start the operation
                trimThread = new Thread(new ThreadStart(this.TrimThreadProc));
                trimThread.Start();

                //	Block the caller until operation is complete or the user cancels
                if(m_wndProgress.ShowDialog() == DialogResult.Cancel)
                {
					m_bCancelled = m_wndProgress.Cancelled;

                }// if(StatusForm.ShowDialog() == DialogResult.Cancel)
                else
                {
                    bSuccessful = true;
                }

                //	Wait for thread to be terminated
                while(trimThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    Thread.Sleep(500);

                    //	Crude test for timeout
                    if(iAttempts < 25)
                        iAttempts++;
                    else
                        break;
                }

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "Trim", m_tmaxErrorBuilder.Message(ERROR_TRIM_EX), Ex);
            }

            return bSuccessful;

        }// public bool Trim()

		/// <summary>This method will reset the local members to their default values</summary>
		/// <param name="bTotal">True to do a reset of all members, False for new source file</param>
		public void Clear()
		{
			try
			{
				if(m_dxAllUsed != null)
				{
					m_dxAllUsed.Clear();
					m_dxAllUsed = null;
				}

				if(m_dxScripts != null)
				{
					m_dxScripts.Clear();
					m_dxScripts = null;
				}

				if(m_dxDocuments != null)
				{
					m_dxDocuments.Clear();
					m_dxDocuments = null;
				}

				if(m_dxOthers != null)
				{
					m_dxOthers.Clear();
					m_dxOthers = null;
				}

				if(m_dxDesignations != null)
				{
					m_dxDesignations.Clear();
					m_dxDesignations = null;
				}

				if(m_dxPages != null)
				{
					m_dxPages.Clear();
					m_dxPages = null;
				}

				if(m_dxBinders != null)
				{
					m_dxBinders.Clear();
					m_dxBinders = null;
				}

				if(m_tmaxTrimmed != null)
				{
					m_tmaxTrimmed.Close();
					m_tmaxTrimmed = null;
				}

				//	Destroy the status form
				if(m_wndProgress != null)
				{
					if(m_wndProgress.IsDisposed == false)
						m_wndProgress.Dispose();
					m_wndProgress = null;
				}

				m_tmaxSourceFolder.Reset();
				m_tmaxTrimmedFolder.Reset();
				m_strTrimmedFileSpec = "";
			
			}
			catch
			{
			}

		}// public void Clear()

		/// <summary>This method is called to add a message to the status form list</summary>
        /// <param name="strMessage">The message to be added</param>
        /// <param name="eType">Enumerated error level</param>
		public void AddMessage(string strMessage, TmaxMessageLevels eLevel)
        {
           try
            {
				if((m_wndProgress != null) && (m_wndProgress.IsDisposed == false))
				{
					if(eLevel == TmaxMessageLevels.Warning)
						m_wndProgress.AddWarning(m_strBarcode, strMessage);
					else
						m_wndProgress.AddError(m_strBarcode, strMessage);
				}

            }
            catch
            {
            }

		}// public void AddMessage(string strMessage, TmaxMessageLevels eLevel)

        #endregion Public Methods

        #region Private Methods

        /// <summary>Called to copy the source database to the target folder</summary>
        /// <returns>true if successful</returns>
        private bool CopySource()
        {
			bool	bSuccessful = false;
			string	strSourceFileSpec = "";
			string	strTrimmedFileSpec = "";
			
			try
			{
				//	Set the source and target folders
				m_tmaxSourceFolder.Initialize(m_tmaxDatabase.Folder);
				m_tmaxTrimmedFolder.Initialize(m_tmaxTrimOptions.CaseFolder);
			
				SetStatus("Copying " + m_tmaxSourceFolder.Name);
				
				//	Build the fully qualified paths to the source and target database files
				strSourceFileSpec = m_tmaxSourceFolder.Path;
				if(strSourceFileSpec.EndsWith("\\") == false)
					strSourceFileSpec += "\\";
				strSourceFileSpec += TMAX_DATABASE_FILENAME;

				strTrimmedFileSpec = m_tmaxTrimmedFolder.Path;
				if (strTrimmedFileSpec.EndsWith("\\") == false)
					strTrimmedFileSpec += "\\";
				strTrimmedFileSpec += TMAX_DATABASE_FILENAME;
				
				//	Copy the source database
				//
				//	NOTE:	The GetOptions() form verifies that the trimmed folder
				//			exists and is different than that of the source folder
				System.IO.File.Copy(strSourceFileSpec, strTrimmedFileSpec);
				
				//	Save the path to the trimmed file for future use
				m_strTrimmedFileSpec = strTrimmedFileSpec;
				
				//	Copy the source configuration file
				/*
				 * DON'T COPY CASE.XML - THIS FORCES USE OF DEFAULT PATHS
				 * 
				strSourceFileSpec = m_tmaxSourceFolder.Path;
				if (strSourceFileSpec.EndsWith("\\") == false)
					strSourceFileSpec += "\\";
				strSourceFileSpec += TMAX_CONFIGURATION_FILENAME;

				strTrimmedFileSpec = m_tmaxTrimmedFolder.Path;
				if (strTrimmedFileSpec.EndsWith("\\") == false)
					strTrimmedFileSpec += "\\";
				strTrimmedFileSpec += TMAX_CONFIGURATION_FILENAME;

				try
				{
					System.IO.File.Copy(strSourceFileSpec, strTrimmedFileSpec);
				}
				catch
				{
					//	We can live without the configuration file if we have to
				}
				*/
				
				bSuccessful = true;
			
			}
			catch (System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CopySource", m_tmaxErrorBuilder.Message(ERROR_COPY_SOURCE_EX, strSourceFileSpec, strTrimmedFileSpec), Ex);
			}
			
			return bSuccessful;

		}// private bool CopySource()

		/// <summary>Called to open the trimmed database</summary>
		/// <returns>true if successful</returns>
		private bool OpenTrimmed()
        {
			bool bSuccessful = false;
			
			try
			{
				SetStatus("Opening " + m_tmaxTrimmedFolder.Name);

				m_tmaxTrimmed = new CTmaxCaseDatabase();
				this.EventSource.Attach(m_tmaxTrimmed.EventSource);
				m_tmaxTrimmed.FillOnOpen = false; // don't need the trimmed record set
				
				if(m_tmaxTrimmed.Open(m_tmaxTrimmedFolder.Path, m_tmaxDatabase.GetUserName()) == true)
				{
					bSuccessful = true;
				}
				
			}
			catch (System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenTrimmed", m_tmaxErrorBuilder.Message(ERROR_OPEN_TRIMMED_EX, m_strTrimmedFileSpec), Ex);
			}

			return bSuccessful;

		}// private bool OpenTrimmed()
		
        /// <summary>This method is called to create the status form for the operation</summary>
        /// <returns>true if successful</returns>
        private bool CreateProgressForm()
        {
            //	Clear the cancellation flag
            m_bCancelled = false;

            try
            {
                //	Make sure the previous instance is disposed
                if (m_wndProgress != null)
                {
                    if (m_wndProgress.IsDisposed == false)
                        m_wndProgress.Dispose();
                    m_wndProgress = null;
                }

                //	Create a new instance
                m_wndProgress = new FTI.Trialmax.Forms.CFTrimProgress();
                m_wndProgress.CaseFolder = m_tmaxDatabase.Folder;
                if(m_wndProgress.CaseFolder.EndsWith("\\") == true)
					m_wndProgress.CaseFolder = m_wndProgress.CaseFolder.Substring(0, m_wndProgress.CaseFolder.Length - 1);

                //	Set the initial status message
                SetStatus("Initializing trim operation ...");

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "CreateProgressForm", m_tmaxErrorBuilder.Message(ERROR_CREATE_PROGRESS_FORM_EX), Ex);
                m_wndProgress = null;
            }

            return (m_wndProgress != null);

        }// private bool CreateProgressForm()

        /// <summary>This method is called to update the status text on the status form</summary>
        /// <param name="strStatus">The new status message</param>
        private void SetStatus(string strStatus)
        {
            try
            {
                if ((m_wndProgress != null) && (m_wndProgress.IsDisposed == false))
                {
                    m_wndProgress.Status = strStatus;
                    m_wndProgress.Refresh();
                }

            }
            catch
            {
            }

        }// private void SetStatus(string strStatus)

        /// <summary>This method runs in its own thread to perform the import operation</summary>
        private void TrimThreadProc()
        {
			bool bSuccessful = false;
			
            Debug.Assert(m_tmaxDatabase != null);

			while(bSuccessful == false)
			{
				//	Copy the source database file
				if(CopySource() == false)
					break;
				if(m_bCancelled == true)
					break;
					
				//	Open the database to be trimmed
				if(OpenTrimmed() == false)
					break;
				if(m_bCancelled == true)
					break;

				//	Get the list of primaries to be retained
				if(AddSelections() == false)
					break;
				if(m_bCancelled == true)
					break;

				//	Delete the unused records
				if(DeleteRecords() == false)
					break;
				if(m_bCancelled == true)
					break;

				//	Transfer the source files
				if(TransferFiles() == false)
					break;
				if(m_bCancelled == true)
					break;

				bSuccessful = true;

			}// while(bSuccessful == false)

//string strMsg = String.Format("All: {0}\nPages: {1}\nDesignations: {2}", m_dxAllUsed.Count, m_dxPages.Count, m_dxDesignations.Count);
//MessageBox.Show(strMsg);
			
            //	Notify the status form
            if(m_wndProgress != null)
            {
                FTI.Shared.Win32.User.MessageBeep(0);

				if(m_bCancelled == false)
                {
					m_wndProgress.Finished = true;
                    SetStatus("Trim operation complete");
                }
                else
                {
					SetStatus("Trim operation cancelled");
                }

			}// if (m_wndProgress != null)

			//	Clean up
			if(m_tmaxTrimmed != null)
			{
				m_tmaxTrimmed.Close();
				m_tmaxTrimmed = null;
			}

		}// private void TrimThreadProc()

		/// <summary>Called to check the primary record to determine if it should be retained</summary>
		/// <returns>true if successful</returns>
		private bool CheckRecord(CDxMediaRecord dxRecord)
		{
			bool	bSuccessful = true;
			string	strBarcode = "";

			try
			{
				strBarcode = dxRecord.GetBarcode(true);
				SetStatus("Checking " + strBarcode);
	
				switch(dxRecord.MediaType)
				{
					case TmaxMediaTypes.Script:
					
						//	Has this script already been added?
						if(m_dxAllUsed.Contains(dxRecord) == false)
						{
							//	Add this script
							m_dxAllUsed.AddList(dxRecord);
							
							//	Make sure the scenes collection has been populated
							if(((CDxPrimary)dxRecord).Secondaries.Count == 0)
								((CDxPrimary)dxRecord).Fill();
								
							//	Process each of the scenes
							foreach(CDxSecondary O in ((CDxPrimary)dxRecord).Secondaries)
							{
								if(O.GetSource() != null)
									CheckRecord(O.GetSource());
							}

						}// if(m_dxAllUsed.Contains(dxRecord) == false)
						break;

					case TmaxMediaTypes.Document:

						if(m_dxAllUsed.Contains(dxRecord) == false)
							m_dxAllUsed.AddList(dxRecord);
						
						//	Make sure all the pages are in the Pages collection
						if(((CDxPrimary)dxRecord).Secondaries.Count == 0)
							((CDxPrimary)dxRecord).Fill();

						foreach(CDxSecondary O in ((CDxPrimary)dxRecord).Secondaries)
						{
							if(m_dxPages.Contains(O) == false)
								m_dxPages.AddList(O);
						}	
						break;

					case TmaxMediaTypes.Powerpoint:
					case TmaxMediaTypes.Deposition:
					case TmaxMediaTypes.Recording:

						if(m_dxAllUsed.Contains(dxRecord) == false)
						{
							//	Add this script
							m_dxAllUsed.AddList(dxRecord);
						}
						break;

					case TmaxMediaTypes.Segment:
					case TmaxMediaTypes.Slide:

						if(m_dxAllUsed.Contains(dxRecord.GetParent()) == false)
						{
							m_dxAllUsed.AddList(dxRecord.GetParent());
						}
						break;

					case TmaxMediaTypes.Page:

						if(m_dxAllUsed.Contains(dxRecord.GetParent()) == false)
						{
							m_dxAllUsed.AddList(dxRecord.GetParent());
						}
						
						//	Add to the pages collection
						if(m_dxPages.Contains(dxRecord) == false)
							m_dxPages.AddList(dxRecord);
						
						break;

					case TmaxMediaTypes.Treatment:
					
						//	Add the parent document to the collection
						if(dxRecord.GetParent() != null)
						{
							//	Add the parent to the pages collection
							if(m_dxPages.Contains(dxRecord.GetParent()) == false)
								m_dxPages.AddList(dxRecord.GetParent());
								
							if(m_dxAllUsed.Contains(dxRecord.GetParent().GetParent()) == false)
							{
								m_dxAllUsed.AddList(dxRecord.GetParent().GetParent());

							}// if(m_dxAllUsed.Contains(dxRecord.GetParent().GetParent()) == false)
						
						}// if(dxRecord.GetParent() != null)
						break;

					case TmaxMediaTypes.Designation:
					case TmaxMediaTypes.Clip:
					
						if(dxRecord.GetParent() != null)
						{
							if(m_dxAllUsed.Contains(dxRecord.GetParent().GetParent()) == false)
							{
								m_dxAllUsed.AddList(dxRecord.GetParent().GetParent());
								
							}// if(m_dxAllUsed.Contains(dxRecord.GetParent().GetParent()) == false)

							if(m_dxDesignations.Contains(dxRecord) == false)
							{
								m_dxDesignations.AddList(dxRecord);

								//	Does this have any child links?
								if(((CDxTertiary)dxRecord).ChildCount > 0)
								{
									if(((CDxTertiary)dxRecord).Quaternaries.Count == 0)
										((CDxTertiary)dxRecord).Fill();

									foreach(CDxQuaternary O in ((CDxTertiary)dxRecord).Quaternaries)
										CheckRecord(O);

								}// if(((CDxTertiary)dxRecord).ChildCount > 0)

							}// if(m_dxTertiaries.Contains(dxRecord) == false)
								
						}// if(dxRecord.GetParent() != null)
						break;

					case TmaxMediaTypes.Scene:

						//	We have to process the entire script
						CheckRecord(dxRecord.GetParent());
						break;

					case TmaxMediaTypes.Link:
					
						if(((CDxQuaternary)dxRecord).GetSource() != null)
						{
							CheckRecord(((CDxQuaternary)dxRecord).GetSource());
						}
						break;
						
				}// switch(dxRecord.MediaType)	
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckRecord", m_tmaxErrorBuilder.Message(ERROR_CHECK_RECORD_EX, strBarcode), Ex);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool CheckPrimary(CDxPrimary dxPrimary)

		/// <summary>Called to check the specified event item to process the binder it is bound to</summary>
		/// <param name="tmaxBinder">The event item that identifies the binder</param>
		/// <returns>true if successful</returns>
		private bool CheckBinder(CTmaxItem tmaxBinder)
		{
			bool bSuccessful = true;

			try
			{
				//	This item must be bound to a binder
				if(tmaxBinder.IBinderEntry == null) return false;

				//	Are the desired child binder entries specified?
				if((tmaxBinder.SubItems != null) && (tmaxBinder.SubItems.Count > 0))
				{
					if(m_dxBinders.Contains((CDxBinderEntry)(tmaxBinder.IBinderEntry)) == false)
						m_dxBinders.AddList((CDxBinderEntry)(tmaxBinder.IBinderEntry));
						
					foreach(CTmaxItem O in tmaxBinder.SubItems)
						CheckBinder(O);
				}
				else
				{
					if(((CDxBinderEntry)(tmaxBinder.IBinderEntry)).IsMedia() == true)
					{
						if(((CDxBinderEntry)(tmaxBinder.IBinderEntry)).GetSource(true) != null)
						{
							CheckRecord(((CDxBinderEntry)(tmaxBinder.IBinderEntry)).GetSource(true));
						}
						
					}
					else
					{
						//	Process the full contents of this binder
						CheckBinder((CDxBinderEntry)(tmaxBinder.IBinderEntry));
					}

				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckBinder", m_tmaxErrorBuilder.Message(ERROR_CHECK_BINDER_EX, tmaxBinder.IBinderEntry.GetName()), Ex);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool CheckBinder(CTmaxItem tmaxBinder)

		/// <summary>Called to check the specified binder to see if it has media that should be processed</summary>
		/// <param name="dxBinder">The binder to be checked</param>
		/// <returns>true if successful</returns>
		private bool CheckBinder(CDxBinderEntry dxBinder)
		{
			bool bSuccessful = true;

			try
			{
				SetStatus("Checking binder -> " + dxBinder.Name);
				
				if(dxBinder.Contents.Count == 0)
					dxBinder.Fill();
					
				if(m_dxBinders.Contains(dxBinder) == false)
					m_dxBinders.AddList(dxBinder);
					
				foreach(CDxBinderEntry O in dxBinder.Contents)
				{
					//	Is this a media reference?
					if(O.IsMedia() == true)
					{
						if(O.GetSource(true) != null)
							CheckRecord(O.GetSource(true));
					}
					else
					{
						CheckBinder(O);
					}

				}// foreach(CDxBinderEntry O in dxBinder.Contents)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckBinder", m_tmaxErrorBuilder.Message(ERROR_CHECK_BINDER_EX, dxBinder.Name), Ex);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool CheckBinder(CDxBinderEntry dxBinder)

		/// <summary>Called to add the requested media to the trimmed database</summary>
		/// <returns>true if successful</returns>
		private bool AddSelections()
		{
			bool bSuccessful = true;

			try
			{
				SetStatus("Adding selected media");

				//	Are we keeping the scripts?
				if((m_tmaxTrimOptions.KeepScripts == true) && (m_dxScripts != null))
				{
					AddScripts();
				}

				//	Are we keeping the treatments?
				if((m_tmaxTrimOptions.KeepTreatments == true) && (m_dxDocuments != null))
				{
					AddTreatments();
				}
				
				//	Are we keeping the binder contents?
				if((m_tmaxTrimOptions.BinderSelections != null) && (m_tmaxTrimOptions.BinderSelections.Count > 0))
				{
					AddBinders();
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSelections", m_tmaxErrorBuilder.Message(ERROR_ADD_SELECTIONS_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;

		}// private bool AddSelections()

		/// <summary>Called to add the scripts to the trimmed database</summary>
		/// <returns>true if successful</returns>
		private bool AddScripts()
		{
			bool bSuccessful = true;

			try
			{
				if((m_dxScripts != null) && (m_dxScripts.Count > 0))
				{
					SetStatus("Adding " + m_dxScripts.Count.ToString() + " scripts");

					foreach(CDxPrimary O in m_dxScripts)
					{
						m_strBarcode = O.MediaId;
						SetStatus("Adding " + m_strBarcode);
						
						CheckRecord(O);
						
						if(m_bCancelled == true)
							break;
							
					}// foreach(CDxPrimary O in m_dxScripts)
				
				}// if((m_dxScripts != null) && (m_dxScripts.Count > 0))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddScripts", m_tmaxErrorBuilder.Message(ERROR_ADD_SCRIPTS_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;

		}// private bool AddScripts()

		/// <summary>Called to add the treatments to the trimmed database</summary>
		/// <returns>true if successful</returns>
		private bool AddTreatments()
		{
			bool	bSuccessful = true;
			bool	bTreated = false;
			bool	bFilled = false;

			try
			{
				if((m_dxDocuments != null) && (m_dxDocuments.Count > 0))
				{
					SetStatus("Adding treatments for " + m_dxDocuments.Count.ToString() + " documents");
					
					//	Check all documents
					foreach(CDxPrimary O in m_dxDocuments)
					{
						m_strBarcode = O.MediaId;
						SetStatus("Checking " + m_strBarcode + " for treatments");
						
						//	Reset the flags for this document
						bTreated = false;
						bFilled = false;

						//	Do we need to fill the secondaries collection?
						if(O.Secondaries.Count == 0)
						{
							O.Fill();
							bFilled = true;
						}
						
						//	Check each of the pages
						foreach(CDxSecondary dxPage in O.Secondaries)
						{
							//	Has this page been treated?
							if(dxPage.ChildCount > 0)
							{
								if(m_dxPages.Contains(dxPage) == false)
									m_dxPages.AddList(dxPage);
							
								bTreated = true;

							}// if(dxPage.ChildCount > 0)
								
						}// foreach(CDxSecondary dxPage in O.Secondaries)
						
						//	Has this document been treated?
						if(bTreated == true)
						{
							//	Add to the collection if not already added by a script
							if(m_dxAllUsed.Contains(O) == false)
								m_dxAllUsed.AddList(O);
						}
						else
						{
							//	Should we clear the collection?
							if(bFilled == true)
								O.Secondaries.Clear();

						}// if(bTreated == true)
						
						//	Has the user cancelled?
						if(m_bCancelled == true)
							break;

					}// foreach(CDxPrimary O in m_dxDocuments)

				}// if((m_dxDocuments != null) && (m_dxDocuments.Count > 0))
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddTreatments", m_tmaxErrorBuilder.Message(ERROR_ADD_TREATMENTS_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;

		}// private bool AddTreatments()

		/// <summary>Called to add the treatments to the trimmed database</summary>
		/// <returns>true if successful</returns>
		private bool AddBinders()
		{
			bool bSuccessful = true;

			try
			{
				m_strBarcode = "";
				
				if((m_tmaxTrimOptions.BinderSelections != null) && (m_tmaxTrimOptions.BinderSelections.Count > 0))
				{
					SetStatus("Adding binder contents");

					foreach(CTmaxItem O in m_tmaxTrimOptions.BinderSelections)
					{
						CheckBinder(O);
						
						if(m_bCancelled == true)
							break;
							
					}// foreach(CTmaxItem O in m_tmaxTrimOptions.BinderSelections)
				
				}// if((m_tmaxTrimOptions.BinderSelections != null) && (m_tmaxTrimOptions.BinderSelections.Count > 0))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddBinders", m_tmaxErrorBuilder.Message(ERROR_ADD_BINDERS_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;

		}// private bool AddBinders()

		/// <summary>This method is called to delete the unused primary and secondary records</summary>
		///	<returns>true if successful</returns>
		private bool DeleteRecords()
		{
			bool	bSuccessful = true;
			string	strRecords = "";
			string	strSQL = "";

			try
			{
				if(m_tmaxTrimmed == null) return false;
				if(m_tmaxTrimmed.IsConnected == false) return false;
				
				if(m_dxAllUsed.Count == 0) 
				{
					MessageBox.Show("No primary records to be retained. The operation will be cancelled", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}

				//	Should we delete the unused media records?
				if(m_tmaxTrimOptions.UnusedMediaRecords == false)
				{
					SetStatus("Deleting unused media records");

					//	Build the partial statement to identify the primary records
					foreach(CDxPrimary O in m_dxAllUsed)
					{
						if(strRecords.Length > 0)
							strRecords += ",";
						else
							strRecords = "(";

						strRecords += O.AutoId.ToString();
					}
					strRecords += ")";
					
					//	Build the SQL statement to delete the primary records
					strSQL = String.Format("DELETE FROM PrimaryMedia WHERE AutoId NOT IN {0};", strRecords);

					//	Execute the statement
					if(m_tmaxTrimmed.Execute(strSQL) == false)
					{
						//	NOTE:	The database template defines a relationship between the primary
						//			and secondary tables so there's no need to delete the secondary records
						bSuccessful = false;

					}// if(m_tmaxTrimmed.Connection.Execute(strSQL) == true)

				}// if(m_tmaxTrimOptions.UnusedMediaRecords == false)
				
				if((m_dxBinders != null) && (m_dxBinders.Count > 0))
				{
					strRecords = "";
					
					//	Build the partial statement to identify the records
					foreach(CDxBinderEntry O in m_dxBinders)
					{
						if(strRecords.Length > 0)
							strRecords += ",";
						else
							strRecords = "(";

						strRecords += O.AutoId.ToString();
					}
					strRecords += ")";
				
					//	Build the SQL statement to get rid of any unused binders
					strSQL = String.Format("DELETE FROM BinderEntries WHERE ((AutoId NOT IN {0}) AND (Attributes = 0));", strRecords);
					m_tmaxTrimmed.Execute(strSQL);
					
					//	Build the SQL statement to get rid of any unused media
					strSQL = String.Format("DELETE FROM BinderEntries WHERE ((AutoId NOT IN {0}) AND (ParentId NOT IN {0}));", strRecords);
					m_tmaxTrimmed.Execute(strSQL);
				}
				else
				{
					strSQL = "DELETE * FROM BinderEntries;";
					m_tmaxTrimmed.Execute(strSQL);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "DeleteRecords", m_tmaxErrorBuilder.Message(ERROR_DELETE_RECORDS_EX), Ex);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool DeleteRecords()

		/// <summary>This method is called to copy all files from the source database to the trimmed database</summary>
		///	<returns>true if successful</returns>
		private bool TransferFiles()
		{
			bool bSuccessful = true;

			try
			{
				SetStatus("Transferring Files");

				//	Transfer the files associated with each primary record
				foreach(CDxPrimary O in m_dxAllUsed)
				{
					if(TransferFiles(O) == false)
						bSuccessful = false;
						
					//	Has the user cancelled?
					if(m_bCancelled == true)
						break;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "TransferFiles", m_tmaxErrorBuilder.Message(ERROR_TRANSFER_FILES_EX), Ex);
			}

			return bSuccessful;

		}// private bool TransferFiles()

		/// <summary>This method is called to copy all files for the specified primary record</summary>
		/// <param name="dxPrimary">The primary record that owns the files</param>
		///	<returns>true if successful</returns>
		private bool TransferFiles(CDxPrimary dxPrimary)
		{
			bool			bSuccessful = true;
			CDxTertiaries	dxTertiaries = null;
			CDxSecondaries	dxPages = null;
			
			try
			{
				//	Nothing to do for scripts
				if(dxPrimary.MediaType == TmaxMediaTypes.Script)
					return true;

				m_strBarcode = dxPrimary.MediaId;// just in case an error occurs
				SetStatus("Transferring files for " + m_strBarcode);

				//	What type of primary media are we dealing with?
				switch(dxPrimary.MediaType)
				{
					case TmaxMediaTypes.Document:

						//	Get the pages that should be transferred for this document
						if((dxPages = GetTransferPages(dxPrimary)) != null)
						{
							//	Transfer each page
							foreach(CDxSecondary O in dxPages)
							{
								//	Copy the page file
								Copy(O);

								//	Always transfer all treatments since this page
								//	will appear in the tree
								if(O.ChildCount > 0)
								{
									if(O.Tertiaries.Count == 0)
										O.Fill();

									foreach(CDxTertiary T in O.Tertiaries)
										Copy(T);
								}

							}// foreach(CDxSecondary O in dxPages)

						}// if((dxPages = GetTransferPages(dxPrimary)) != null)
						
						break;

					case TmaxMediaTypes.Recording:

						//	Transfer each of the secondary segments
						if(dxPrimary.Secondaries.Count == 0)
							dxPrimary.Fill();
							
						foreach(CDxSecondary O in dxPrimary.Secondaries)
							Copy(O);
							
						//	Transfer all the tertiaries that reference this primary
						if((dxTertiaries = GetTertiaries(dxPrimary)) != null)
						{
							foreach(CDxTertiary O in dxTertiaries)
								Copy(O);

							FreeTertiaries(dxTertiaries);
						}
						break;

					case TmaxMediaTypes.Deposition:
					
						//	Copy the transcript for this record
						if(dxPrimary.GetTranscript() != null)
							Copy(dxPrimary.GetTranscript());
						else
							AddMessage(ERROR_NO_TRANSCRIPT, dxPrimary.MediaId);

						//	Transfer all the designations that reference this deposition
						//
						//	NOTE:	We don't bother transferring videos
						if((dxTertiaries = GetTertiaries(dxPrimary)) != null)
						{
							foreach(CDxTertiary O in dxTertiaries)
								Copy(O);
								
							FreeTertiaries(dxTertiaries);
						}
						break;

					case TmaxMediaTypes.Powerpoint:
					
						//	Copy the presentation for this record
						Copy(dxPrimary);
						break;

					case TmaxMediaTypes.Script:
						break; // no files associated with a script
					
					default:
					
						Debug.Assert(false, "Unhandled primary media type -> " + dxPrimary.MediaType.ToString());
						break;

				}// switch(dxPrimary.MediaType)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "TransferFiles", m_tmaxErrorBuilder.Message(ERROR_TRANSFER_PRIMARY_EX, dxPrimary.MediaId), Ex);
			}

			return bSuccessful;

		}// private bool TransferFiles(CDxPrimary dxPrimary)

		/// <summary>This method is called to copy the source file for the specified record to the trimmed database</summary>
		/// <param name="dxRecord">The owner record</param>
		///	<returns>true if successful</returns>
		private bool Copy(CDxMediaRecord dxRecord)
		{
			string	strSourceFileSpec = "";
			string	strTargetFileSpec = "";

			//	Get the paths
			strSourceFileSpec = m_tmaxDatabase.GetFileSpec(dxRecord);
			strTargetFileSpec = m_tmaxTrimmed.GetFileSpec(dxRecord);

			if((strSourceFileSpec.Length > 0) && (strTargetFileSpec.Length > 0))
			{
				return Copy(strSourceFileSpec, strTargetFileSpec);
			}
			else
			{
				return false;
			}

		}// private bool Copy(CDxMediaRecord dxRecord)

		/// <summary>This method is called to copy the source file for the specified transcript</summary>
		/// <param name="dxTranscript">The owner record</param>
		///	<returns>true if successful</returns>
		private bool Copy(CDxTranscript dxTranscript)
		{
			string strSourceFileSpec = "";
			string strTargetFileSpec = "";

			//	Get the paths
			strSourceFileSpec = m_tmaxDatabase.GetFileSpec(dxTranscript);
			strTargetFileSpec = m_tmaxTrimmed.GetFileSpec(dxTranscript);

			if((strSourceFileSpec.Length > 0) && (strTargetFileSpec.Length > 0))
			{
				return Copy(strSourceFileSpec, strTargetFileSpec);
			}
			else
			{
				return false;
			}

		}// private bool Copy(CDxTranscript dxTranscript)

		/// <summary>This method is called to copy the specified source file to the target location</summary>
		/// <param name="strSourceFileSpec">Fully qualified path to the source file</param>
		/// <param name="strTargetFileSpec">Fully qualified path to the target file</param>
		///	<returns>true if successful</returns>
		private bool Copy(string strSourceFileSpec, string strTargetFileSpec)
		{
			bool	bSuccessful = true;
			string	strTargetFolder = "";

			SetStatus("Copying " + strSourceFileSpec);

			//	Make sure the target folder exists
			strTargetFolder = System.IO.Path.GetDirectoryName(strTargetFileSpec);
			if((strTargetFolder.Length > 0) && (System.IO.Directory.Exists(strTargetFolder) == false))
			{
				try
				{
					System.IO.Directory.CreateDirectory(strTargetFolder);
				}
				catch
				{
					AddMessage(ERROR_CREATE_FOLDER_FAILED, strTargetFolder);
					return false;
				}

			}// if((strTargetFolder.Length > 0) && (System.IO.Directory.Exists(strTargetFolder) == false))

			//	Copy the source file to the requested location
			try
			{
				System.IO.File.Copy(strSourceFileSpec, strTargetFileSpec, true);
			}
			catch
			{
				AddMessage(ERROR_COPY_FAILED, strSourceFileSpec, strTargetFileSpec);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool Copy(string strSourceFileSpec, string strTargetFileSpec)

		/// <summary>This method is called to get all tertiaries that were created from the specified primary record</summary>
		/// <param name="dxPrimary">The primary record that owns the tertiaries</param>
		///	<returns>true if successful</returns>
		private CDxTertiaries GetTertiaries(CDxPrimary dxPrimary)
		{
			CDxTertiaries dxTertiaries = null;

			try
			{
				if((m_dxDesignations != null) && (m_dxDesignations.Count > 0))
				{
					dxTertiaries = new CDxTertiaries();

					foreach(CDxTertiary O in m_dxDesignations)
					{
						if((O.Secondary != null) && (ReferenceEquals(O.Secondary.Primary, dxPrimary) == true))
							dxTertiaries.AddList(O);					
					}

				}// if((m_dxTertiaries != null) && (m_dxTertiaries.Count > 0))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetTertiaries", Ex);
			}

			if((dxTertiaries != null) && (dxTertiaries.Count == 0))
				dxTertiaries = null;

			return dxTertiaries;

		}// private CDxTertiaries GetTertiaries(CDxPrimary dxPrimary)

		/// <summary>This method is called to remove the specified tertiaries from the local collection</summary>
		/// <param name="dxTertiaries">The tertiaries that have been transferred</param>
		private void FreeTertiaries(CDxTertiaries dxTertiaries)
		{
			try
			{
				if((dxTertiaries != null) && (m_dxDesignations != null) && (m_dxDesignations.Count > 0))
				{
					foreach(CDxTertiary O in dxTertiaries)
					{
						m_dxDesignations.RemoveList(O);
					}

				}// if((dxTertiaries != null) && (m_dxTertiaries != null) && (m_dxTertiaries.Count > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FreeTertiaries", Ex);
			}

		}// private void FreeTertiaries(CDxTertiaries dxTertiaries)

		/// <summary>This method is called to get the collection of pages that should be transferred for the document</summary>
		/// <param name="dxPrimary">The document being transferred</param>
		///	<returns>the collection of pages that should be transferred</returns>
		private CDxSecondaries GetTransferPages(CDxPrimary dxPrimary)
		{
			CDxSecondaries	dxPages = null;
			long			lId = 0;
			
			//	Are we applying the master document range?
			if(m_tmaxTrimOptions.UseMasterRange == true)
			{
				//	Convert this document's media id to a number
				try { lId = System.Convert.ToInt64(dxPrimary.MediaId); }
				catch { lId = 0; }
			
				//	Did we get a valid numeric value?
				if(lId > 0)
				{
					//	Is this document in the master range?
					if((lId >= m_tmaxTrimOptions.FirstDocument) && (lId <= m_tmaxTrimOptions.LastDocument))
					{
						//	We only want to transfer pages that are actually in use
						dxPages = new CDxSecondaries();
						foreach(CDxSecondary O in m_dxPages)
						{
							if(ReferenceEquals(O.Primary, dxPrimary) == true)
								dxPages.AddList(O);
						}

						//	Did we find any pages?
						if(dxPages.Count > 0)
						{
							//	Remove from the master list to speed up future processing
							foreach(CDxSecondary O in dxPages)
								m_dxPages.Remove(O);
								
							//	Are we supposed to make sure we have the first page?
							if(m_tmaxTrimOptions.KeepFirstPage == true)
							{
								//	Make sure the collection has been populated
								if(dxPrimary.Secondaries.Count == 0)
									dxPrimary.Fill();
									
								if(dxPrimary.Secondaries.Count > 0)
								{
									if(dxPages.Contains(dxPrimary.Secondaries[0]) == false)
										dxPages.AddList(dxPrimary.Secondaries[0]);
								}
					
							}// if(m_tmaxTrimOptions.KeepFirstPage == true)
							
						}
						else
						{
							//	This shouldn't happen otherwise the document wouldn't be in the list
							dxPages = null; // Force the default collection
						}
						
					}// if((lId >= m_tmaxTrimOptions.FirstDocument) && (lId <= m_tmaxTrimOptions.LastDocument))

				}// if(lId > 0)

			}// if(m_tmaxTrimOptions.UseMasterRange == true)
			
			//	Should we just use the default collection?
			if(dxPages == null)
			{
				//	Make sure the collection has been populated
				if(dxPrimary.Secondaries.Count == 0)
					dxPrimary.Fill();
					
				dxPages = dxPrimary.Secondaries;

			}// if(dxPages == null)

			return dxPages;

		}// private CDxSecondaries GetTransferPages(CDxPrimary dxPrimary)
		
        /// <summary>This method is called to populate the error builder's format string collection</summary>
        private void SetErrorStrings()
        {
            if (m_tmaxErrorBuilder == null) return;
            if (m_tmaxErrorBuilder.FormatStrings == null) return;

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the trim operation");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to execute the trim operation");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the progress form for the trim operation");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to copy the source database for the trim operation: source = %1  trimmed = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the trimmed database: filename = %1");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the selected media");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to check the record: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete the unused records");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to transfer files from the source database");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to transfer the files for %1");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the target folder: path = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve the transcript record for %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to copy %1 to %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while checking the contents of the binder named %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding scripts to the trimmed database");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding treatments to the trimmed database");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding binders to the trimmed database");

		}// private void SetErrorStrings()

        /// <summary>This method is called to add a warning message to the status form list</summary>
        /// <param name="strMessage">The message to be added</param>
        private void AddMessage(string strMessage)
        {
            AddMessage(strMessage, TmaxMessageLevels.Warning);
        }

        /// <summary>This method is called to add a message to the status form list</summary>
        /// <param name="iErrorId">Error identifier</param>
        /// <param name="param1">Parameter 1 to construct the message</param>
        /// <param name="param2">Parameter 2 to construct the message</param>
        /// <param name="param3">Parameter 3 to construct the message</param>
		/// <param name="eLevel">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, object param1, object param2, object param3, TmaxMessageLevels eLevel)
        {
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1, param2, param3), eLevel);
        }

        /// <summary>This method is called to add a message to the status form list</summary>
        /// <param name="iErrorId">Error identifier</param>
        /// <param name="param1">Parameter 1 to construct the message</param>
        /// <param name="param2">Parameter 2 to construct the message</param>
        /// <param name="eType">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, object param1, object param2, TmaxMessageLevels eLevel)
        {
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1, param2), eLevel);
        }

        /// <summary>This method is called to add a message to the status form list</summary>
        /// <param name="iErrorId">Error identifier</param>
        /// <param name="param1">Parameter 1 to construct the message</param>
        /// <param name="eType">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, object param1, TmaxMessageLevels eLevel)
        {
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1), eLevel);
        }

        /// <summary>This method is called to add a message to the status form list</summary>
        /// <param name="iErrorId">Error identifier</param>
        /// <param name="eType">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, TmaxMessageLevels eLevel)
        {
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId), eLevel);
        }

        /// <summary>This method is called to add a warning message to the status form list</summary>
        /// <param name="iErrorId">Error identifier</param>
        /// <param name="param1">Parameter 1 to construct the message</param>
        /// <param name="param2">Parameter 2 to construct the message</param>
        /// <param name="param3">Parameter 3 to construct the message</param>
        private void AddMessage(int iErrorId, object param1, object param2, object param3)
        {
            AddMessage(iErrorId, param1, param2, param3, TmaxMessageLevels.Warning);
        }

        /// <summary>This method is called to add a warning message to the status form list</summary>
        /// <param name="iErrorId">Error identifier</param>
        /// <param name="param1">Parameter 1 to construct the message</param>
        /// <param name="param2">Parameter 2 to construct the message</param>
        private void AddMessage(int iErrorId, object param1, object param2)
        {
            AddMessage(iErrorId, param1, param2, TmaxMessageLevels.Warning);
        }

        /// <summary>This method is called to add a warning message to the status form list</summary>
        /// <param name="iErrorId">Error identifier</param>
        /// <param name="param1">Parameter 1 to construct the message</param>
        private void AddMessage(int iErrorId, object param1)
        {
            AddMessage(iErrorId, param1, TmaxMessageLevels.Warning);
        }

        /// <summary>This method is called to add a warning message to the status form list</summary>
        /// <param name="iErrorId">Error identifier</param>
        private void AddMessage(int iErrorId)
        {
            AddMessage(iErrorId, TmaxMessageLevels.Warning);
        }

        #endregion Private Methods

        #region Properties

        /// <summary>Event source interface for error and diagnostic events</summary>
        public FTI.Shared.Trialmax.CTmaxEventSource EventSource
        {
            get { return m_tmaxEventSource; }
        }

        /// <summary>The active database</summary>
        public CTmaxCaseDatabase Database
        {
            get { return m_tmaxDatabase; }
            set { m_tmaxDatabase = value; }
        }

        /// <summary>The active set of trim options</summary>
        public CTmaxTrimOptions Options
        {
            get { return m_tmaxTrimOptions; }
            set { m_tmaxTrimOptions = value; }
        }

        #endregion Properties

    }// class CTmaxTrimManager

}// namespace FTI.Trialmax.Database

