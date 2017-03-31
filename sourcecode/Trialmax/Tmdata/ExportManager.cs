using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.IO;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Encode;

namespace FTI.Trialmax.Database
{
    /// <summary>This class manages the export operations for the database</summary>
    public class CTmaxExportManager
    {
        #region Constants

        /// <summary>Error message identifiers</summary>
        private const int ERROR_INITIALIZE_EX = 0;
        private const int ERROR_GET_EXPORT_STREAM_EX = 1;
        private const int ERROR_EXPORT_EX = 2;
        private const int ERROR_CREATE_STATUS_FORM_EX = 3;
        private const int ERROR_FILE_OPEN_FAILED = 4;
        private const int ERROR_NO_BARCODE_MAP_ENTRIES = 5;
        private const int ERROR_EXPORT_BARCODE_MAP_EX = 6;
        private const int ERROR_EXPORT_CODES_EX = 7;
        private const int ERROR_GET_CODES_OPTIONS_EX = 8;
        private const int ERROR_GET_VIDEO_OPTIONS_EX = 9;
        private const int ERROR_NO_VIDEO_IN_SCRIPT = 10;
        private const int ERROR_GET_VIDEO_SCENES_EX = 11;
        private const int ERROR_GET_EXPORT_FILESPEC_EX = 12;
        private const int ERROR_EXPORT_AS_WMV_EX = 13;
        private const int ERROR_INITIALIZE_WMV_ENCODER = 14;
        private const int ERROR_SET_WMV_PROFILE = 15;
        private const int ERROR_ADD_WMV_SOURCE = 16;
        private const int ERROR_EXECUTE_WMV_ENCODER = 17;
        private const int ERROR_EXPORT_AS_SAMI_EX = 18;
        private const int ERROR_EXPORT_AS_SAMI_FAILED = 19;
        private const int ERROR_VIDEO_FILE_NOT_FOUND = 20;
        private const int ERROR_EXPORT_THREAD_PROC_EX = 21;
        private const int ERROR_INVALID_EXPORT_FILESPEC = 22;
        private const int ERROR_GET_TEXT_OPTIONS_EX = 23;
        private const int ERROR_GET_XML_SOURCE_EX = 24;
        private const int ERROR_GET_XML_OPTIONS_EX = 25;
        private const int ERROR_XML_EMPTY_SCRIPT_EX = 26;
        private const int ERROR_XML_DESIGNATIONS_ONLY = 27;
        private const int ERROR_XML_NO_SOURCE_DESIGNATION = 28;
        private const int ERROR_XML_EXPORT_SCRIPT_EX = 29;
        private const int ERROR_XML_SAVE_FAILED = 30;
        private const int ERROR_XML_ONE_DEPOSITION_ONLY = 31;
        private const int ERROR_XML_ADD_DEPOSITION_EX = 32;
        private const int ERROR_XML_NO_DEPOSITION = 33;
        private const int ERROR_WMV_INCOMPLETE = 34;
        private const int ERROR_GET_XML_CASE_CODES_OPTIONS_EX = 35;
        private const int ERROR_NO_CASE_CODES = 36;
        private const int ERROR_EXPORT_XML_CASE_CODES_EX = 37;
        private const int ERROR_EXPORT_XML_CASE_CODES_SAVE = 38;
        private const int ERROR_OPEN_CODES_DATABASE_EX = 39;
        private const int ERROR_LOCATE_CODES_DATABASE_FAILED = 40;
        private const int ERROR_COPY_CODES_DATABASE_FAILED = 41;
        private const int ERROR_OPEN_CODES_DATABASE_FAILED = 42;
        private const int ERROR_CREATE_CODES_TABLE_FAILED = 43;
        private const int ERROR_NO_SOURCE_XML_BINDERS = 44;
        private const int ERROR_EXPORT_XML_BINDERS_EX = 45;

        #endregion Constants

        #region Private Members

        /// <summary>Local member bounded to EventSource property</summary>
        private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

        /// <summary>Local member used to construct error messages</summary>
        private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

        /// <summary>Local member bound to ExportOptions property</summary>
        private CTmaxExportOptions m_tmaxExportOptions = null;

        /// <summary>Local member bound to Database property</summary>
        private CTmaxCaseDatabase m_tmaxDatabase = null;

        /// <summary>Local member bound to Source property</summary>
        private CTmaxItems m_tmaxSource = null;

        /// <summary>Local member to keep track of the initial record</summary>
        private CBaseRecord m_dxInitial = null;

        /// <summary>Local member bound to Format property</summary>
        private TmaxExportFormats m_eFormat = TmaxExportFormats.Unknown;

        /// <summary>Local member bound to StatusForm property</summary>
        private CFExportStatus m_wndStatus = null;

        /// <summary>Local member bound to Stream property</summary>
        private System.IO.StreamWriter m_streamWriter = null;

        /// <summary>Local member bound to FileSpec property</summary>
        private string m_strFileSpec = "";

        /// <summary>Delimiter(s) used to parse lines in the export file</summary>
        private string m_strDelimiters = "\t";

        /// <summary>Filter string used in file selection dialog</summary>
        private string m_strFileFilter = "";

        /// <summary>Default file extension in file selection dialog</summary>
        private string m_strExtension = "";

        /// <summary>Total number of records that have been exported</summary>
        private long m_lExported = 0;

        /// <summary>Total number of files that have been created</summary>
        private long m_lFiles = 0;

        /// <summary>Flag to indicate if the operation should be terminated</summary>
        private bool m_bTerminate = false;

        /// <summary>Flag to indicate if the operation was aborted by the user</summary>
        private bool m_bAborted = false;

        /// <summary>Flag to indicate if the user should be prompted before deleting output file</summary>
        private bool m_bConfirmOverwrite = true;

        /// <summary>Flag to indicate if the filename for multiple outputs should be automatically assigned</summary>
        private bool m_bAutoFilenames = true;

        /// <summary>Fully qualified path to the ouput folder</summary>
        private string m_strExportFolder = "";

        /// <summary>Private member bound to WMEncoder property</summary>
        private FTI.Trialmax.Encode.CWMEncoder m_wmEncoder = null;

        /// <summary>Private member for exporting codes (fielded data) to database</summary>
        private CTmaxCodesDatabase m_tmaxCodesDatabase = null;

        private bool m_bIsMpeg2Selected = false;

        #endregion Private Members

        #region Public Methods

        /// <summary>Constructor</summary>
        public CTmaxExportManager()
        {
            //	Initialize the event source and error builder
            SetErrorStrings();
            m_tmaxEventSource.Name = "Export Manager";

        }// public CTmaxExportManager()

        /// <summary>This method uses the specified parameters to set the associated properties</summary>
        /// <param name="tmaxParameters">The collection of parameters</param>
        /// <param name="tmaxSource">Collection of event items that identify the source records</param>
        /// <returns>true if successful</returns>
        public bool Initialize(CTmaxParameters tmaxParameters, CTmaxItems tmaxSource)
        {
            CTmaxParameter tmaxParameter = null;

            //	Reset the current values to their defaults
            Reset();
            m_bAborted = false;

            try
            {
                if (tmaxParameters != null)
                {
                    //	Get the export format
                    if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.ExportFormat)) != null)
                    {
                        try { this.Format = (TmaxExportFormats)(tmaxParameter.AsInteger()); }
                        catch { }
                    }

                }
                else
                {
                    this.Format = TmaxExportFormats.Unknown;
                    return false;	//	Cancel the operation
                }

                //	Set the format specific defaults
                switch (this.Format)
                {
                    case TmaxExportFormats.Video:

                        //						m_strExtension = "edl";
                        //						m_strFileFilter = "EDL Files (*.edl)|*.edl|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        m_strExtension = "";
                        m_strFileFilter = "";
                        break;

                    case TmaxExportFormats.LoadFile:

                        m_strExtension = "lfp";
                        m_strFileFilter = "Load Files (*.lfp)|*.lfp|All Files (*.*)|*.*";
                        break;


                    case TmaxExportFormats.XmlCaseCodes:

                        m_strExtension = "xml";
                        //Added .txt format in the option on 28June2012
                        m_strFileFilter = "XML Files (*.xml)|*.xml|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        break;

                    case TmaxExportFormats.CodesDatabase:

                        m_strExtension = "mdb";
                        m_strFileFilter = "Database Files (*.mdb)|*.mdb|All Files (*.*)|*.*";
                        break;

                    case TmaxExportFormats.XmlBinder:

                        m_strExtension = CXmlBinder.GetExtension();
                        m_strFileFilter = CXmlBinder.GetFilter(true);
                        break;

                    case TmaxExportFormats.BarcodeMap:
                    case TmaxExportFormats.AsciiMedia:
                    case TmaxExportFormats.Codes:
                    case TmaxExportFormats.XmlScript:
                    case TmaxExportFormats.AsciiPickList:
                    default:

                        m_strExtension = "txt";
                        m_strFileFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        break;

                }// switch(this.Format)

                //	Set the source records
                return SetSource(tmaxSource);
            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX));
                return false;
            }

        }// public bool Initialize(CTmaxParameters tmaxParameters)

        /// <summary>This method is called to execute the export operation</summary>
        /// <returns>true if successful</returns>
        public bool Export()
        {
            //	Get the user defined options
            switch (this.Format)
            {
                case TmaxExportFormats.Codes:
                case TmaxExportFormats.CodesDatabase:

                    if (GetCodesOptions() == false)
                        return false;
                    break;

                case TmaxExportFormats.XmlCaseCodes:

                    if (GetXmlCaseCodesOptions() == false)
                        return false;
                    break;

                case TmaxExportFormats.Video:

                    if (GetVideoOptions() == false)
                        return false;
                    break;

                case TmaxExportFormats.AsciiMedia:

                    if (GetAsciiOptions() == false)
                        return false;
                    break;

                case TmaxExportFormats.XmlScript:

                    if (GetXmlOptions() == false)
                        return false;
                    break;

            }// switch(this.Format)

            //	Create the status form for the operation
            if (this.CreateStatusForm() == false) return false;

            try
            {
                ExportThreadProc();
                /*
                //	Start the operation
                exportThread = new Thread(new ThreadStart(this.ExportThreadProc));
                exportThread.Start();
					
                //	Block the caller until operation is complete or the user cancels
                StatusForm.ShowDialog();
			
                //	Wait for thread to be terminated
                while(exportThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    Thread.Sleep(500);
					
                    //	Crude test for timeout
                    if(iAttempts < 20)
                        iAttempts++;
                    else
                        break;
                }
*/
            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "Export", m_tmaxErrorBuilder.Message(ERROR_EXPORT_EX), Ex);
                AddMessage(m_tmaxErrorBuilder.Message(ERROR_EXPORT_THREAD_PROC_EX), TmaxMessageLevels.FatalError);
            }

            return (m_lExported > 0);

        }// public bool Export()

        /// <summary>This method is called to add a message to the status form list</summary>
        /// <param name="strMessage">The message to be added</param>
        /// <param name="eType">Enumerated message type to define error level</param>
        public void AddMessage(string strMessage, TmaxMessageLevels eLevel)
        {
            try
            {
                if ((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
                {
                    m_wndStatus.AddMessage(strMessage, m_wndStatus.Filename, eLevel);
                }

            }
            catch
            {
            }

        }// public void AddMessage(string strMessage, TmaxMessageLevels eLevel)

        #endregion Public Methods

        #region Private Methods

        /// <summary>This method is called to execute the export thread</summary>
        public void ExportThreadProc()
        {
            try
            {
                //	What are we exporting?
                switch (m_eFormat)
                {
                    case TmaxExportFormats.BarcodeMap:

                        ExportBarcodeMap();
                        break;

                    case TmaxExportFormats.XmlCaseCodes:

                        ExportXmlCaseCodes();
                        break;

                    case TmaxExportFormats.AsciiMedia:

                        ExportAscii();
                        break;

                    case TmaxExportFormats.LoadFile:

                        ExportLoadFile();
                        break;

                    case TmaxExportFormats.Video:

                        ExportVideo();
                        break;

                    case TmaxExportFormats.Codes:
                    case TmaxExportFormats.CodesDatabase:

                        ExportCodes();
                        break;

                    case TmaxExportFormats.XmlScript:

                        ExportXmlScripts();
                        break;

                    case TmaxExportFormats.XmlBinder:

                        ExportXmlBinders();
                        break;

                    case TmaxExportFormats.AsciiPickList:

                        ExportPickLists();
                        break;

                    default:

                        Debug.Assert(false, m_eFormat.ToString() + " is not a valid format for the export operation");
                        break;

                }// switch(m_eFormat)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "ExportThreadProc", m_tmaxErrorBuilder.Message(ERROR_EXPORT_THREAD_PROC_EX), Ex);
                AddMessage(m_tmaxErrorBuilder.Message(ERROR_EXPORT_THREAD_PROC_EX), TmaxMessageLevels.FatalError);
            }

            //	Notify the status form
            if (this.StatusForm != null)
            {
                FTI.Shared.Win32.User.MessageBeep(0);

                if (this.StatusForm.Aborted == false)
                {
                    this.StatusForm.Finished = true;
                    SetStatus("Export operation complete");
                }

            }// if(this.StatusForm != null)

        }// public void Export()

        /// <summary>This method is called to export the requested codes (fielded data)</summary>
        /// <returns>True if successful</returns>
        private bool ExportCodes()
        {
            bool bSuccessful = false;
            CDxPrimaries dxPrimaries = null;

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(m_tmaxExportOptions != null);

            //	Make sure we have valid export columns
            if ((m_tmaxExportOptions.Columns == null) || (m_tmaxExportOptions.Columns.Count == 0))
            {
                MessageBox.Show("No columns have been selected to perform the export operation", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            try
            {
                //	Make the status form visible
                SetStatusVisible(true, "Retrieving primary records ...");
                Cursor.Current = Cursors.WaitCursor;

                //	Create a temporary collection to hold the source records
                dxPrimaries = new CDxPrimaries();
                dxPrimaries.Comparer = new CTmaxRecordSorter();
                ((CTmaxRecordSorter)(dxPrimaries.Comparer)).PrimarySortOrder = CTmaxRecordSorter.PRIMARY_SORT_ORDER_MEDIA_ID;
                dxPrimaries.KeepSorted = false;

                //	If no source provided we are supposed to export all primaries
                if ((m_tmaxSource == null) || (m_tmaxSource.Count == 0))
                {
                    //	Add all primary records
                    foreach (CDxPrimary O in m_tmaxDatabase.Primaries)
                        dxPrimaries.AddList(O);
                }
                else
                {
                    foreach (CTmaxItem O in m_tmaxSource)
                    {
                        //	Is this a media record?
                        if (O.IPrimary != null)
                        {
                            if (dxPrimaries.Contains((CDxPrimary)(O.IPrimary)) == false)
                                dxPrimaries.AddList((CDxPrimary)(O.IPrimary));
                        }

                        //	Is this a binder
                        else if (O.IBinderEntry != null)
                        {
                            GetPrimaries((CDxBinderEntry)(O.IBinderEntry), dxPrimaries, m_tmaxExportOptions.SubBinders);
                        }

                        else if (O.MediaType != TmaxMediaTypes.Unknown)
                        {
                            GetPrimaries(O.MediaType, dxPrimaries);
                        }

                    }// foreach(CTmaxItem O in m_tmaxSource)

                }// if((m_tmaxSource == null) || (m_tmaxSource.Count == 0))

                //	Make sure the user hasn't aborted
                if (CheckAborted() == true)
                    return false;

                //	Do we have any primary records?
                if ((dxPrimaries == null) || (dxPrimaries.Count == 0))
                {
                    MessageBox.Show("No primary records are available to perform the export operation", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                //	Make sure the source records are properly sorted
                dxPrimaries.Sort(true);

                //	Open the file stream
                if (GetExportStream(null) == true)
                {
                    //	Make the status form visible
                    SetStatus("Exporting primary records ...");

                    //	Include the sort order in the output file
                    m_tmaxExportOptions.Columns.SortOrder = 1;
                    m_tmaxExportOptions.Columns.UseSortOrder = true;

                    //	Make sure the columns have valid case code references
                    m_tmaxExportOptions.Columns.SetCaseCodes(m_tmaxDatabase.CaseCodes);

                    //	Should we write the column headers?
                    if ((m_eFormat == TmaxExportFormats.Codes) && (m_tmaxExportOptions.ColumnHeaders == true))
                        ExportHeaders(m_tmaxExportOptions.Columns);

                    //	Export the codes for each record
                    foreach (CDxPrimary O in dxPrimaries)
                    {
                        ExportCodes(O);

                        if (CheckAborted() == true)
                            break;
                    }

                    if (CheckAborted() == false)
                        bSuccessful = true;
                }

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_CODES_EX, m_strFileSpec));
            }
            finally
            {
                CloseStream();

                Cursor.Current = Cursors.Default;

                if (dxPrimaries != null)
                {
                    dxPrimaries.Clear();
                    dxPrimaries = null;
                }

                if (m_lExported > 0)
                {
                    SetSummary(m_lExported.ToString() + " records exported to " + m_strFileSpec);
                }

                //	Reset the sort order options
                m_tmaxExportOptions.Columns.SortOrder = 0;
                m_tmaxExportOptions.Columns.UseSortOrder = false;

            }

            return bSuccessful;

        }// private bool ExportCodes()

        /// <summary>This method is called to export the codes associated with the specified record</summary>
        /// <param name="dxPrimary">The primary record to be exported</param>
        /// <returns>True if successful</returns>
        private bool ExportCodes(CDxPrimary dxPrimary)
        {
            bool bSuccessful = true;
            ArrayList aLines = null;

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(m_tmaxExportOptions != null);
            Debug.Assert(dxPrimary != null);

            //	Update the status form
            SetStatus("Exporting " + dxPrimary.MediaId);

            try
            {
                if (m_eFormat == TmaxExportFormats.CodesDatabase)
                {
                    Debug.Assert(m_tmaxCodesDatabase != null);

                    bSuccessful = m_tmaxCodesDatabase.Add(dxPrimary, m_tmaxExportOptions.Columns);
                }
                else
                {
                    Debug.Assert(m_streamWriter != null);

                    //	Get the collection of lines to be exported
                    if ((aLines = m_tmaxExportOptions.Columns.GetLines(dxPrimary, m_tmaxExportOptions)) != null)
                    {
                        foreach (string O in aLines)
                        {
                            m_streamWriter.WriteLine(O);
                        }

                        aLines.Clear();
                        aLines = null;

                    }// if((aLines = m_tmaxExportOptions.Columns.GetLines(dxPrimary, m_tmaxExportOptions)) != null)

                    bSuccessful = true;

                }// if(m_eFormat == TmaxExportFormats.CodesDatabase)

            }
            catch
            {
            }
            finally
            {
            }

            if (bSuccessful == true)
                m_lExported++;

            return bSuccessful;

        }// private bool ExportCodes(CDxPrimary dxPrimary)

        /// <summary>This method is called to write the column headers to the export file</summary>
        /// <param name="tmaxColumns">The collection of columns being exported</param>
        /// <returns>True if successful</returns>
        private bool ExportHeaders(CTmaxExportColumns tmaxColumns)
        {
            bool bSuccessful = true;
            string strLine = "";

            Debug.Assert(m_streamWriter != null);
            Debug.Assert(m_tmaxExportOptions != null);
            Debug.Assert(tmaxColumns != null);
            Debug.Assert(tmaxColumns.Count > 0);

            //	Update the status form
            SetStatus("Exporting column headers ...");

            try
            {
                //	Get the header line
                strLine = tmaxColumns.GetHeader(m_tmaxExportOptions);

                if (strLine.Length > 0)
                {
                    m_streamWriter.WriteLine(strLine);

                    bSuccessful = true;

                }

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireDiagnostic(this, "ExportHeaders", Ex);
            }
            finally
            {
            }

            return bSuccessful;

        }// private bool ExportHeaders(CTmaxExportColumns tmaxColumns)

        /// <summary>This method is called to export the barcode map table</summary>
        /// <returns>True if successful</returns>
        private bool ExportBarcodeMap()
        {
            bool bSuccessful = false;
            string strLine = "";

            Debug.Assert(m_tmaxDatabase != null);

            //	NOTE:	The initialization should have taken care of this
            if ((m_tmaxDatabase.BarcodeMap == null) || (m_tmaxDatabase.BarcodeMap.Count == 0))
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_NO_BARCODE_MAP_ENTRIES));
                return false;
            }

            //	Open the file stream
            if (GetExportStream(null) == false) return false;

            //	Make the status form visible
            SetStatusVisible(true, "Exporting foreign barcodes");
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //	Export each of the entries in the map
                foreach (CDxBarcode O in m_tmaxDatabase.BarcodeMap)
                {
                    if ((O.Source != null) && (O.ForeignBarcode.Length > 0))
                    {
                        strLine = String.Format("{0}{1}{2}", O.Source.GetBarcode(true), m_strDelimiters, O.ForeignBarcode);
                        m_streamWriter.WriteLine(strLine);
                        m_lExported++;

                        //	Make sure the user has not aborted the operation
                        if (CheckAborted() == true) break;

                    }

                }// foreach(CDxBarcode O in m_tmaxDatabase.BarcodeMap)

                if (CheckAborted() == false)
                    bSuccessful = true;

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_BARCODE_MAP_EX, m_strFileSpec));
            }
            finally
            {
                CloseStream();

                Cursor.Current = Cursors.Default;

                if (bSuccessful == true)
                {
                    SetSummary(m_lExported.ToString() + " records exported to " + m_strFileSpec);
                }

            }

            return bSuccessful;

        }// private bool ExportBarcodeMap(string strFileSpec)

        /// <summary>This method is called to export the case codes and pick lists</summary>
        /// <returns>True if successful</returns>
        private bool ExportXmlCaseCodes()
        {
            bool bSuccessful = false;
            bool bContinue = false;
            string strFileSpec = "";

            Debug.Assert(m_tmaxDatabase != null);

            //	NOTE:	The initialization should have taken care of this
            if ((m_tmaxDatabase.CaseCodes != null) && (m_tmaxDatabase.CodesManager != null))
            {
                if (m_tmaxDatabase.CaseCodes.Count > 0)
                {
                    bContinue = true;
                }
                else if (m_tmaxDatabase.PickLists != null)
                {
                    if ((m_tmaxDatabase.PickLists.Children != null) && (m_tmaxDatabase.PickLists.Children.Count > 0))
                        bContinue = true;
                }

            }
            if (bContinue == false)
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_NO_BARCODE_MAP_ENTRIES));
                return false;
            }

            //	Get the path to the file
            strFileSpec = GetExportFileSpec(null);
            if ((strFileSpec == null) || (strFileSpec.Length == 0))
                return false;
            else
                this.FileSpec = strFileSpec; // This is now the active file

            //	Make the status form visible
            SetStatusVisible(true, "Exporting data fields");
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //	Are we supposed to refresh the codes first?
                if (m_tmaxExportOptions.RefreshSource == true)
                {
                    m_tmaxDatabase.FireCommand(TmaxCommands.RefreshCodes);
                }

                //	Now save the codes using the file specified by the caller
                if (m_tmaxDatabase.CodesManager.SaveAs(this.FileSpec, true, false) == true)
                {
                    if (CheckAborted() == true)
                        bSuccessful = false;
                }
                else
                {
                    OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_XML_CASE_CODES_SAVE, this.FileSpec));
                }

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_XML_CASE_CODES_EX, this.FileSpec));
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                if (bSuccessful == true)
                {
                    SetSummary("Data fields exported to " + this.FileSpec);
                }

            }

            return bSuccessful;

        }// private bool ExportXmlCaseCodes()

        /// <summary>This method is called to export the source records as text</summary>
        /// <returns>True if successful</returns>
        private bool ExportAscii()
        {
            bool bSuccessful = true;
            CDxMediaRecord dxSource = null;

            Debug.Assert(m_tmaxDatabase != null);

            if (m_tmaxSource == null) return false;
            if (m_tmaxSource.Count == 0) return false;

            //	Make the status form visible
            SetStatusVisible(true, "Exporting records ...");
            Cursor.Current = Cursors.WaitCursor;

            //	Export each of the records specified by the caller
            foreach (CTmaxItem O in m_tmaxSource)
            {
                //	Get the source record to be exported
                if ((dxSource = GetRecord(O)) == null) continue;

                //	The source must be a binder or a script
                if (dxSource.GetDataType() == TmaxDataTypes.Media)
                {
                    if (dxSource.MediaType != TmaxMediaTypes.Script) continue;
                }
                else if (dxSource.GetDataType() == TmaxDataTypes.Binder)
                {
                    if (m_eFormat != TmaxExportFormats.AsciiMedia) continue;
                }
                else
                {
                    continue; // Only scripts and binders
                }

                //	Open the file stream for this record
                if ((CheckAborted() == true) || (GetExportStream(dxSource) == false))
                {
                    bSuccessful = false;
                    break;
                }
                else
                {
                    //	Increment the file counter
                    m_lFiles++;
                }

                //	Export this record
                if (dxSource.GetDataType() == TmaxDataTypes.Binder)
                {
                    if (ExportAscii((CDxBinderEntry)dxSource) == false)
                        bSuccessful = false;
                }
                else
                {
                    if (ExportAscii(dxSource) == false)
                    {
                        bSuccessful = false;
                    }

                }// if(dxSource.GetDataType() == TmaxDataTypes.Binder)

                //	Close the file stream
                CloseStream();

                //	Break out on error
                if (bSuccessful == false)
                    break;

            }// foreach(CTmaxItem O in m_tmaxSource)

            Cursor.Current = Cursors.Default;

            if (m_lFiles > 0)
            {
                SetSummary(String.Format("{0} records exported to {1} file(s)", m_lExported, m_lFiles));
            }

            return bSuccessful;

        }// private bool ExportAscii()

        /// <summary>This method is called to export the source pick lists to text</summary>
        /// <returns>True if successful</returns>
        private bool ExportPickLists()
        {
            bool bSuccessful = true;
            CDxPickItem dxSource = null;

            Debug.Assert(m_tmaxDatabase != null);

            if (m_tmaxSource == null) return false;
            if (m_tmaxSource.Count == 0) return false;

            //	Make the status form visible
            SetStatusVisible(true, "Exporting records ...");
            Cursor.Current = Cursors.WaitCursor;

            //	Export each of the records specified by the caller
            foreach (CTmaxItem O in m_tmaxSource)
            {
                //	We must have the list
                if (O.PickItem == null) continue;
                if ((dxSource = (CDxPickItem)(O.PickItem.DxRecord)) == null) continue;

                //	Open the file stream for this record
                if ((CheckAborted() == true) || (GetExportStream(dxSource) == false))
                {
                    bSuccessful = false;
                    break;
                }
                else
                {
                    //	Increment the file counter
                    m_lFiles++;
                }

                //	Export this list
                if (ExportPickList(O.PickItem, dxSource) == false)
                    bSuccessful = false;

                //	Close the file stream
                CloseStream();

                //	Break out on error
                if (bSuccessful == false)
                    break;

            }// foreach(CTmaxItem O in m_tmaxSource)

            Cursor.Current = Cursors.Default;

            if (m_lFiles > 0)
            {
                SetSummary(String.Format("{0} lists exported to {1} file(s)", m_lExported, m_lFiles));
            }

            return bSuccessful;

        }// private bool ExportPickLists()

        /// <summary>This method is called to export the specified pick list record</summary>
        /// <param name="dxBinder">The application pick list item being exported</param>
        /// <param name="dxPickItem">The data exchange interface for the export record</param>
        /// <returns>True if successful</returns>
        private bool ExportPickList(CTmaxPickItem tmaxPickItem, CDxPickItem dxPickItem)
        {
            bool bSuccessful = true;

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(m_streamWriter != null);
            Debug.Assert(tmaxPickItem != null);

            //	Make the status form visible
            SetStatus("Exporting " + tmaxPickItem.Name);

            try
            {
                //	Do we have any children
                if ((tmaxPickItem.Children != null) || (tmaxPickItem.Children.Count > 0))
                {
                    foreach (CTmaxPickItem O in tmaxPickItem.Children)
                    {
                        m_streamWriter.WriteLine(O.Name);
                        m_lExported++;
                    }

                }

            }
            catch
            {
            }
            finally
            {
            }

            return bSuccessful;

        }// private bool ExportPickList(CTmaxPickItem tmaxPickItem, CDxPickItem dxPickItem)

        /// <summary>This method is called to export the source records to XML</summary>
        /// <returns>True if successful</returns>
        private bool ExportXmlScripts()
        {
            bool bSuccessful = true;
            CDxPrimaries dxScripts = new CDxPrimaries();

            Debug.Assert(m_tmaxDatabase != null);

            if (m_tmaxSource == null) return false;
            if (m_tmaxSource.Count == 0) return false;

            //	Make the status form visible
            SetStatusVisible(true, "Exporting records to XML ...");
            Cursor.Current = Cursors.WaitCursor;

            //	Get the source records for the operation
            if (GetSourceXmlScripts(dxScripts) == true)
            {
                //	Export each script
                foreach (CDxPrimary O in dxScripts)
                {
                    if (CheckAborted() == true)
                        break;

                    if (ExportXmlScript(O) == false)
                        break;

                }// foreach(CDxPrimary O in dxScripts)

            }// if(GetXmlSource(dxScripts) == true)

            Cursor.Current = Cursors.Default;

            if (m_lFiles > 0)
            {
                SetSummary(String.Format("{0} records exported to {1} file(s)", m_lExported, m_lFiles));
                bSuccessful = true; // At least one file exported
            }

            return bSuccessful;

        }// private bool ExportXmlScripts()

        /// <summary>This method is called to export export the specified script to XML</summary>
        /// <param name="dxScript">The script to be exported</param>
        /// <returns>True if the operation should continue</returns>
        private bool ExportXmlScript(CDxPrimary dxScript)
        {
            string strFileSpec = "";
            bool bFilled = false;
            bool bContinue = true;
            CXmlScript xmlScript = null;
            CXmlScene xmlScene = null;
            CXmlDesignation xmlDesignation = null;
            CDxMediaRecord dxSource = null;
            CDxTertiary dxTertiary = null;

            Debug.Assert(dxScript != null);

            //	Fill the child collection if necessary
            if ((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
            {
                dxScript.Fill();
                bFilled = true;
            }

            //	Are there any records to be exported?
            if (dxScript.Secondaries.Count == 0)
            {
                AddMessage(m_tmaxErrorBuilder.Message(ERROR_XML_EMPTY_SCRIPT_EX, dxScript.GetBarcode(false)), TmaxMessageLevels.Warning);
                return true; // Allow to continue to the next script
            }

            //	Update the status form
            SetStatus("Exporting " + dxScript.GetBarcode(false) + " to XML ...");

            //	Get the path to the output file
            strFileSpec = GetExportFileSpec(dxScript);
            if ((strFileSpec == null) || (strFileSpec.Length == 0))
                return true; // Allow to continue to the next script

            try
            {
                //	Create and initialize the XML script
                xmlScript = new CXmlScript();
                xmlScript.XmlScriptFormat = m_tmaxExportOptions.XmlScriptFormat;
                xmlScript.MediaId = dxScript.GetBarcode(true);
                xmlScript.Name = dxScript.Name.Length > 0 ? dxScript.Name : dxScript.GetBarcode(true);

                //	Add the scenes to the XML script
                foreach (CDxSecondary O in dxScript.Secondaries)
                {
                    //	Reset
                    xmlScene = null;
                    xmlDesignation = null;

                    //	Must have a valid source record
                    if ((dxSource = O.GetSource()) == null) continue;

                    //	Video Viewer only supports designations
                    if (m_tmaxExportOptions.XmlScriptFormat == TmaxXmlScriptFormats.VideoViewer)
                    {
                        if (dxSource.MediaType != TmaxMediaTypes.Designation)
                        {
                            AddMessage(m_tmaxErrorBuilder.Message(ERROR_XML_DESIGNATIONS_ONLY, dxScript.GetBarcode(false)), TmaxMessageLevels.Warning);
                            xmlScript = null;
                            break;
                        }

                    }

                    //	Create and initialize a new scene object
                    xmlScene = new CXmlScene();
                    xmlScene.SourceId = dxSource.GetBarcode(true);
                    xmlScene.SourceType = dxSource.MediaType;
                    xmlScene.Hidden = O.Hidden;
                    xmlScene.AutoTransition = O.AutoTransition;
                    xmlScene.TransitionTime = O.TransitionTime;
                    xmlScene.BarcodeId = O.BarcodeId;

                    //	Do we need to create an associated designation?
                    if ((dxSource.MediaType == TmaxMediaTypes.Designation) ||
                       (dxSource.MediaType == TmaxMediaTypes.Clip))
                    {
                        dxTertiary = (CDxTertiary)dxSource;

                        if ((xmlDesignation = m_tmaxDatabase.GetXmlDesignation(dxTertiary, true, false, false)) == null)
                        {
                            xmlScript = null;
                            AddMessage(m_tmaxErrorBuilder.Message(ERROR_XML_NO_SOURCE_DESIGNATION, dxScript.GetBarcode(false), dxSource.GetBarcode(true)), TmaxMessageLevels.Warning);
                            break;
                        }
                        else
                        {
                            //	Make sure we can link back to the primary source
                            if ((dxTertiary.Secondary != null) && (dxTertiary.Secondary.Primary != null))
                                xmlDesignation.PrimaryId = dxTertiary.Secondary.Primary.MediaId;

                            //	Make sure we can link back to the secondary parent
                            if (dxTertiary.MediaType == TmaxMediaTypes.Designation)
                                xmlDesignation.Segment = dxTertiary.GetExtent().XmlSegmentId.ToString();
                            else
                                xmlDesignation.Segment = dxTertiary.Secondary.BarcodeId.ToString();

                            //	Should we add the source deposition?
                            if (AddXmlDeposition(xmlScript, (CDxTertiary)dxSource) == false)
                            {
                                xmlScript = null;
                                break;
                            }

                        }

                    }// if(dxSource.MediaType == TmaxMediaTypes.Designation)

                    //	Add this scene to the script
                    if (xmlScript != null)
                        xmlScript.Add(xmlScene, xmlDesignation);

                }// foreach(CDxSecondary O in dxScript.Secondaries)

                if (CheckAborted() == false)
                {
                    //	Save the script
                    if (xmlScript != null)
                    {
                        if (xmlScript.Save(strFileSpec) == true)
                        {
                            m_lFiles++;
                            m_lExported += xmlScript.XmlScenes.Count;
                        }
                        else
                        {
                            AddMessage(m_tmaxErrorBuilder.Message(ERROR_XML_SAVE_FAILED, dxScript.GetBarcode(false), dxScript.GetBarcode(true), strFileSpec), TmaxMessageLevels.Warning);
                        }

                    }// if(xmlScript != null)

                }// if(CheckAborted() == false)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireDiagnostic(this, "ExportXml", Ex);
                OnError(m_tmaxErrorBuilder.Message(ERROR_XML_EXPORT_SCRIPT_EX, dxScript.MediaId), false);
            }

            //	Clean up
            if (xmlScript != null)
            {
                xmlScript.Clear();
                xmlScript = null;
            }

            //	Reset the secondaries if necessary
            if (bFilled == true)
                dxScript.Secondaries.Clear();

            return bContinue;

        }// private bool ExportXmlScript(CDxPrimary dxScript)

        /// <summary>This method is called to add the source XML deposition to the XML script if required</summary>
        /// <param name="xmlScript">The exported XML script</param>
        /// <param name="dxTertiary">The tertiary record created from the deposition</param>
        /// <returns>True if successful</returns>
        private bool AddXmlDeposition(CXmlScript xmlScript, CDxTertiary dxTertiary)
        {
            bool bSuccessful = true;
            CXmlDeposition xmlDeposition = null;

            try
            {
                while (true)
                {
                    //	Only required for designations
                    if (dxTertiary.MediaType != TmaxMediaTypes.Designation)
                        break;

                    //	Is this really necessary?
                    //
                    //	NOTE: Video viewer always requires the source deposition
                    if ((m_tmaxExportOptions.IncludeDepositions == false) &&
                       (m_tmaxExportOptions.IncludeObjections == false) &&
                       (xmlScript.XmlScriptFormat != TmaxXmlScriptFormats.VideoViewer))
                        break;

                    //	Is the deposition already in the script?
                    if ((xmlDeposition = xmlScript.XmlDepositions.Find(dxTertiary.Secondary.Primary.MediaId)) != null)
                        break;

                    //	Can only have one deposition in Video Viewer scripts
                    if ((xmlScript.XmlScriptFormat == TmaxXmlScriptFormats.VideoViewer) && (xmlScript.XmlDepositions.Count > 0))
                    {
                        OnError(m_tmaxErrorBuilder.Message(ERROR_XML_ONE_DEPOSITION_ONLY, xmlScript.MediaId), false);
                        bSuccessful = false;
                        break;
                    }

                    //	Get the XML deposition
                    if ((m_tmaxExportOptions.IncludeDepositions == true) || (xmlScript.XmlScriptFormat == TmaxXmlScriptFormats.VideoViewer))
                        xmlDeposition = m_tmaxDatabase.GetXmlDeposition(dxTertiary, true, true, false);
                    else
                        xmlDeposition = m_tmaxDatabase.GetXmlDeposition(dxTertiary, false, false, false);

                    if (xmlDeposition == null)
                    {
                        OnError(m_tmaxErrorBuilder.Message(ERROR_XML_NO_DEPOSITION, xmlScript.MediaId, dxTertiary.GetBarcode(true)), false);
                        bSuccessful = false;
                        break;
                    }

                    xmlDeposition.MediaId = dxTertiary.Secondary.Primary.MediaId;

                    if ((m_tmaxExportOptions.IncludeObjections == true) && (m_tmaxDatabase.Objections != null))
                        xmlDeposition.AddObjections(m_tmaxDatabase.Detail.TmaxCase, m_tmaxDatabase.Objections);

                    //	Add to the script's collection
                    xmlScript.XmlDepositions.Add(xmlDeposition);

                    //	We're done
                    break;

                }// while(true)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireDiagnostic(this, "ExportXml", Ex);
                OnError(m_tmaxErrorBuilder.Message(ERROR_XML_ADD_DEPOSITION_EX, xmlScript.MediaId), false);
                bSuccessful = false;
            }

            return bSuccessful;

        }// private bool AddXmlDeposition(CXmlScript xmlScript, CDxTertiary dxTertiary)

        /// <summary>This method is called to export the source binders to XML</summary>
        /// <returns>True if the operation is successful</returns>
        private bool ExportXmlBinders()
        {
            CTmaxBinderItems tmaxBinders = null;
            string strFileSpec = "";
            bool bSuccessful = false;
            CXmlBinder xmlBinder = null;

            //	Update the status form
            SetStatusVisible(true, "Exporting binders to XML ...");
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //	Get the binders to be exported
                if ((tmaxBinders = GetSourceXmlBinders()) == null)
                {
                    AddMessage(m_tmaxErrorBuilder.Message(ERROR_NO_SOURCE_XML_BINDERS), TmaxMessageLevels.Warning);
                    return false;
                }

                Debug.Assert((tmaxBinders.Count > 0), "Empty binders collection");

                //	Get the path to the output file
                if (tmaxBinders.Count == 1)
                    strFileSpec = GetExportFileSpec((CBaseRecord)(tmaxBinders[0].IBinder));
                else
                    strFileSpec = GetExportFileSpec(null);
                if ((strFileSpec == null) || (strFileSpec.Length == 0))
                    return false;

                this.StatusForm.Filename = System.IO.Path.GetFileName(strFileSpec);
                SetStatus("Creating " + strFileSpec.ToLower() + " ...");

                //	Create and initialize the XML binder
                xmlBinder = new CXmlBinder();
                m_tmaxEventSource.Attach(xmlBinder.EventSource);
                xmlBinder.Binders = tmaxBinders;

                SetStatus("Saving " + strFileSpec.ToLower() + " ...");
                bSuccessful = xmlBinder.Save(strFileSpec);

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireDiagnostic(this, "ExportXmlBinders", Ex);
                OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_XML_BINDERS_EX, strFileSpec), false);
            }
            finally
            {
                //	Clean up
                if (xmlBinder != null)
                    xmlBinder.Binders = null;
                if (tmaxBinders != null)
                {
                    m_lExported = tmaxBinders.Count;
                    tmaxBinders.Clear(true);
                    tmaxBinders = null;
                }

                Cursor.Current = Cursors.Default;
            }

            if (bSuccessful == true)
            {
                SetSummary(m_lExported.ToString() + " binders exported to " + strFileSpec);
            }

            return bSuccessful;

        }// private bool ExportXmlBinders()

        /// <summary>This method is called to export the source records to a load file</summary>
        /// <returns>True if successful</returns>
        private bool ExportLoadFile()
        {
            bool bSuccessful = true;
            CDxMediaRecord dxSource = null;
            CDxMediaRecords dxDocuments = null;
            string strMsg = "";

            Debug.Assert(m_tmaxDatabase != null);

            //	Must have some source records to export
            if (m_tmaxSource == null) return false;
            if (m_tmaxSource.Count == 0) return false;

            //	Open the file stream
            if (GetExportStream(null) == false) return false;

            //	Make the status form visible
            SetStatusVisible(true, "Exporting records ...");
            Cursor.Current = Cursors.WaitCursor;

            //	Allocate the temporary collection used to track document count
            dxDocuments = new CDxMediaRecords();
            dxDocuments.KeepSorted = false;

            //	Export each of the records specified by the caller
            foreach (CTmaxItem O in m_tmaxSource)
            {
                //	Has the user aborted?
                if (CheckAborted() == true)
                {
                    bSuccessful = false; // Break out
                }
                else
                {
                    //	Get the source record to be exported
                    if ((dxSource = GetRecord(O)) != null)
                    {
                        if (ExportLoadFile(dxSource, dxDocuments) == false)
                            bSuccessful = false;
                    }

                }

                //	Break out on error
                if (bSuccessful == false)
                    break;

                if (m_bTerminate == true)
                    break;

            }// foreach(CTmaxItem O in m_tmaxSource)

            //	Close the file stream
            CloseStream();

            Cursor.Current = Cursors.Default;

            //	Did we export any pages?
            if (m_lExported > 0)
            {
                //	Summarize the operation
                strMsg = String.Format("{0} pages in {1} documents exported to {2}", m_lExported, dxDocuments.Count, this.FileSpec);
                SetSummary(strMsg);
            }
            else
            {
                //	If no errors there must not have been any source pages
                if (bSuccessful == true)
                {
                    strMsg = "Unable to locate any pages to export to the load file.";
                    SetSummary(strMsg);
                }

                try { System.IO.File.Delete(this.FileSpec); }
                catch { };
            }

            //	Clean up
            if (dxDocuments != null)
            {
                dxDocuments.Clear();
                dxDocuments = null;
            }

            return bSuccessful;

        }// private bool ExportLoadFile()

        /// <summary>This method is called to export the specified record to a load file</summary>
        /// <param name="dxRecord">The source record to be exported to the load file</param>
        /// <param name="dxDocuments">Collection of records that represent exported documents</param>
        /// <returns>True if successful</returns>
        private bool ExportLoadFile(CDxMediaRecord dxRecord, CDxMediaRecords dxDocuments)
        {
            bool bSuccessful = true;
            bool bFilled = false;
            string strLine = "";
            string strFileSpec = "";

            Debug.Assert(m_tmaxDatabase != null);

            //	Is this a binder we are exporting?
            if (dxRecord.GetDataType() == TmaxDataTypes.Binder)
            {
                //	Make sure the child collection has been populated
                if (((CDxBinderEntry)dxRecord).Contents.Count == 0)
                {
                    ((CDxBinderEntry)dxRecord).Fill();
                    bFilled = true;
                }

                //	Update the status form
                SetStatus("Exporting " + ((CDxBinderEntry)dxRecord).Name + " ...");

                //	Iterate the contents and write each child to the load file
                foreach (CDxBinderEntry O in ((CDxBinderEntry)dxRecord).Contents)
                {
                    if (m_bTerminate == true) break;

                    //	Media reference?
                    if (O.GetSource(true) != null)
                        ExportLoadFile(O.GetSource(true), dxDocuments);
                    else
                        ExportLoadFile(O, dxDocuments);
                }

                //	Do we need to clear the contents collection?
                if (bFilled == true)
                    ((CDxBinderEntry)dxRecord).Contents.Clear();
            }
            else
            {
                //	What type of media?
                switch (dxRecord.MediaType)
                {
                    case TmaxMediaTypes.Document:
                    case TmaxMediaTypes.Script:

                        //	Update the status form
                        SetStatus("Exporting " + dxRecord.GetBarcode(false) + " ...");

                        //	If this is a document make sure the folder does not
                        //	contain a comma
                        if (dxRecord.MediaType == TmaxMediaTypes.Document)
                        {
                            strFileSpec = m_tmaxDatabase.GetFolderSpec((CDxPrimary)dxRecord, false);
                            if (strFileSpec.IndexOf(',') >= 0)
                            {
                                OnError(m_tmaxErrorBuilder.Message(ERROR_INVALID_EXPORT_FILESPEC, dxRecord.GetBarcode(false), strFileSpec), false);
                                return true;
                            }

                        }

                        //	Make sure the child collection has been populated
                        if (((CDxPrimary)dxRecord).Secondaries.Count == 0)
                        {
                            ((CDxPrimary)dxRecord).Fill();
                            bFilled = true;
                        }

                        //	Drill into secondaries collection
                        foreach (CDxSecondary O in ((CDxPrimary)dxRecord).Secondaries)
                        {
                            if (O.MediaType == TmaxMediaTypes.Page)
                            {
                                ExportLoadFile(O, dxDocuments);
                            }
                            else
                            {
                                //	Must be dealing with a scene
                                if (O.GetSource() != null)
                                    ExportLoadFile(O.GetSource(), dxDocuments);
                            }

                            if (m_bTerminate == true) break;

                        }// foreach(CDxSecondary O in ((CDxPrimary)dxRecord).Secondaries)

                        //	Do we need to clear the child collection?
                        if (bFilled == true)
                            ((CDxPrimary)dxRecord).Secondaries.Clear();

                        break;

                    case TmaxMediaTypes.Page:

                        //	Make sure no comma appears in the file path
                        strFileSpec = m_tmaxDatabase.GetFileSpec(dxRecord);
                        if (strFileSpec.IndexOf(',') >= 0)
                        {
                            OnError(m_tmaxErrorBuilder.Message(ERROR_INVALID_EXPORT_FILESPEC, dxRecord.GetBarcode(false), strFileSpec), false);
                            return true;
                        }

                        //	Get the line to be written to the file
                        strLine = GetLoadFileLine((CDxSecondary)dxRecord);
                        if (strLine.Length > 0)
                        {
                            m_streamWriter.WriteLine(strLine);
                            m_lExported++;

                            //	Make sure the parent document is in the caller's collection
                            if ((dxDocuments != null) && (dxDocuments.Contains(dxRecord.GetParent()) == false))
                            {
                                dxDocuments.AddList(dxRecord.GetParent());
                            }

                        }
                        break;

                }// switch(dxRecord.MediaType)

            }// if(dxRecord.GetDataType() == TmaxDataTypes.Binder)

            return bSuccessful;

        }// private bool ExportLoadFile()

        /// <summary>Called to get the line to be written to the load file</summary>
        /// <param name="dxPage">The page being written to the load file</param>
        /// <returns>The text to be written to the load file</returns>
        private string GetLoadFileLine(CDxSecondary dxPage)
        {
            string strKey = "";
            string strBoundryFlag = "";
            string strRelativePath = "";
            string strFilename = "";
            string strType = "";
            string strLine = "";

            try
            {
                //	Is this the first page of the document?
                if (dxPage.DisplayOrder == 1)
                {
                    strKey = dxPage.GetParent().GetBarcode(false);
                    strBoundryFlag = "D";
                }
                else
                {
                    strKey = dxPage.GetBarcode(false);
                    strBoundryFlag = " ";
                }

                //	Pages are stored in the parent document folder
                strRelativePath = m_tmaxDatabase.GetRelativePath(dxPage);
                if (strRelativePath.StartsWith("\\") == true)
                    strRelativePath = strRelativePath.Substring(1, strRelativePath.Length - 1);
                if (strRelativePath.EndsWith("\\") == true)
                    strRelativePath = strRelativePath.Substring(0, strRelativePath.Length - 1);

                strFilename = dxPage.Filename;

                if (dxPage.HighResolution == true)
                    strType = "2";
                else
                    strType = "4";

                strLine = String.Format("IM,{0},{1},0,@VOL;{2};{3};{4}",
                                        strKey, strBoundryFlag,
                                        strRelativePath, strFilename, strType);
            }
            catch
            {
            }

            return strLine;

        }// private string GetLoadFileLine(CDxSecondary dxPage)

        /// <summary>This method is called to export the source records as video</summary>
        /// <returns>True if successful</returns>
        private bool ExportVideo()
        {
            bool bSuccessful = false;
            CDxMediaRecord dxSource = null;

            Debug.Assert(m_tmaxDatabase != null);

            if (m_tmaxSource == null) return false;
            if (m_tmaxSource.Count == 0) return false;

            //	Make the status form is visible
            SetStatusVisible(true, "Exporting video ...");
            Cursor.Current = Cursors.WaitCursor;

            //	Export each of the records specified by the caller
            foreach (CTmaxItem O in m_tmaxSource)
            {
                //	Must be a script for exporting as video
                if ((dxSource = GetRecord(O)) == null) continue;
                if (dxSource.MediaType != TmaxMediaTypes.Script) continue;

                //	Did the user abort?
                if (CheckAborted() == true) break;

                //	Export this script
                ExportVideo((CDxPrimary)dxSource);

                //	Did a fatal error occur?
                if (m_bTerminate == true) break;

            }// foreach(CTmaxItem O in m_tmaxSource)

            //	Turn off the wait cursor
            Cursor.Current = Cursors.Default;

            if (m_lFiles > 0)
            {
                SetSummary(String.Format("{0} records exported to {1} file(s)", m_lExported, m_lFiles));
                bSuccessful = true;
            }

            return bSuccessful;

        }// private bool ExportVideo()

        /// <summary>This method is called to export the specified script to video</summary>
        /// <param name="dxScript">The script to be exported</param>
        /// <returns>True if operation should continue</returns>
        private bool ExportVideo(CDxPrimary dxScript)
        {
            bool bSuccessful = false;
            bool bFilled = false;
            CDxSecondaries dxScenes = null;
            string strFileSpec = "";

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(dxScript != null);

            //	Make the status form visible
            SetStatus("Exporting " + dxScript.GetBarcode(false) + " to video ...");

            try
            {
                //	Fill the child collection if necessary
                if ((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
                {
                    dxScript.Fill();
                    bFilled = true;
                }

                while (true)
                {
                    //	Get the scenes that are bound to video
                    if ((dxScenes = GetVideoScenes(dxScript)) == null)
                    {
                        OnError(m_tmaxErrorBuilder.Message(ERROR_NO_VIDEO_IN_SCRIPT, dxScript.MediaId), false);
                        break;
                    }

                    //	Get the path to the export file(s)
                    strFileSpec = GetExportFileSpec(dxScript);
                    if ((strFileSpec == null) || (strFileSpec.Length == 0))
                        break;

                    m_strExtension = System.IO.Path.GetExtension(strFileSpec);
                    m_strExtension = m_strExtension.Replace(".", "");
                    //	Are we exporting to WMV?
                    if ((CheckAborted() == false) && (m_tmaxExportOptions.VideoWMV == true))
                    {
                        if (ExportAsWMV(dxScript, dxScenes, strFileSpec) == true)
                        {
                            m_lFiles++;
                            bSuccessful = true;
                        }

                    }

                    //	Are we exporting to EDL?
                    if ((CheckAborted() == false) && (m_bTerminate == false) && (m_tmaxExportOptions.VideoEDL == true))
                    {
                        if (ExportAsEDL(dxScript, dxScenes, strFileSpec) == true)
                        {
                            m_lFiles++;
                            bSuccessful = true;
                        }

                    }

                    //	Are we exporting to SAMI?
                    if ((CheckAborted() == false) && (m_bTerminate == false) && (m_tmaxExportOptions.VideoSAMI == true))
                    {
                        if (ExportAsSAMI(dxScript, dxScenes, strFileSpec) == true)
                        {
                            m_lFiles++;
                            bSuccessful = true;
                        }

                    }

                    //	Did we write the scenes to at least one file?
                    if (bSuccessful == true)
                        m_lExported += dxScenes.Count;

                    //	We're done
                    break;
                }

            }
            catch
            {
            }
            finally
            {
                //	Clean up
                if (bFilled == true)
                    dxScript.Secondaries.Clear();

                if (dxScenes != null)
                {
                    dxScenes.Clear();
                    dxScenes = null;
                }

            }

            return bSuccessful;

        }// private bool ExportVideo(CDxPrimary dxScript)

        /// <summary>This method is called to export the specified script to a WMV file</summary>
        /// <param name="dxScript">The script to be exported</param>
        /// <param name="dxScenes">The collection of scenes to be exported</param>
        /// <param name="strFileSpec">The path to the export file</param>
        /// <returns>True if successful</returns>
        private bool ExportAsWMV(CDxPrimary dxScript, CDxSecondaries dxScenes, string strFileSpec)
        {
            CWMEncoder wmEncoder = null;
            CFFMpegEncoder ffmpegEncoder = null;
            bool bSuccessful = false;
            bool bEncode = true;
            CDxTertiary dxTertiary = null;
            string strVideo = "";

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(dxScript != null);
            Debug.Assert(dxScenes != null);
            Debug.Assert(dxScenes.Count > 0);
            Debug.Assert(strFileSpec != null);
            Debug.Assert(strFileSpec.Length > 0);

            // get extension of the output file
            string ext = System.IO.Path.GetExtension(strFileSpec);
            ext = ext.Replace(".", "");

            if (ext.ToUpper() == "WMV" || ext.ToUpper() == "AVI" || ext.ToUpper() == "MPEG" || ext.ToUpper() == "MPG" || ext.ToUpper() == "MP4" || ext.ToUpper() == "MOV")
            {
                // do not change the extention
            }
            else
            {
                ext = "wmv";
                //	Substitute the appropriate extension for the WMV file
                strFileSpec = System.IO.Path.ChangeExtension(strFileSpec, ".wmv");
            }

            //	Make the status form visible
            SetStatus("Exporting " + dxScript.GetBarcode(false) + " to " + ext.ToUpper() + "...");

            try
            {
                //	Delete the existing file
                if (System.IO.File.Exists(strFileSpec) == true)
                {
                    try { System.IO.File.Delete(strFileSpec); }
                    catch { }
                }

                SetStatusFilename(System.IO.Path.GetFileName(strFileSpec));

                //	Use our Windows Media Encoder wrapper to perform the operation
                //wmEncoder = new CWMEncoder();
                //m_tmaxEventSource.Attach(wmEncoder.EventSource);
                //wmEncoder.EncoderStatusUpdate += new FTI.Trialmax.Encode.CWMEncoder.EncoderStatusHandler(this.OnEncoderStatus);
                //wmEncoder.ShowCancel = false;

                ffmpegEncoder = new CFFMpegEncoder();
                ffmpegEncoder.EncoderStatusUpdate += new CFFMpegEncoder.EncoderStatusHandler(this.OnEncoderStatus);
                ffmpegEncoder.ShowCancel = false;
                ffmpegEncoder.m_bIsMpeg2Selected = m_bIsMpeg2Selected;
                while (bSuccessful == false)
                {
                    //	Initialize for this operation
                    if (ffmpegEncoder.Initialize(strFileSpec) == false)
                    {
                        OnError(m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_WMV_ENCODER, strFileSpec), false);
                        break;
                    }

                    //	Set the encoder profile
                    //if(wmEncoder.SetProfile(m_wmEncoder.LastProfile) == false)
                    //{
                    //    OnError(m_tmaxErrorBuilder.Message(ERROR_SET_WMV_PROFILE, m_wmEncoder.LastProfile), false);
                    //    break;
                    //}

                    //	Add a source descriptor for each scene
                    foreach (CDxSecondary O in dxScenes)
                    {
                        //	This collection should only contain valid designations and clips
                        Debug.Assert(O.GetSource() != null, "NULL SOURCE");
                        if (O.GetSource() == null) continue;
                        Debug.Assert((O.GetSource().MediaType == TmaxMediaTypes.Designation) || (O.GetSource().MediaType == TmaxMediaTypes.Clip));
                        if ((O.GetSource().MediaType != TmaxMediaTypes.Designation) &&
                            (O.GetSource().MediaType != TmaxMediaTypes.Clip)) continue;

                        dxTertiary = (CDxTertiary)(O.GetSource());

                        //	Get the path to the video source
                        strVideo = m_tmaxDatabase.GetFileSpec(dxTertiary.Secondary);

                        //	Add a source
                        if ((strVideo.Length > 0) && (dxTertiary.GetExtent() != null))
                        {
                            //	Make sure the video exists
                            if (System.IO.File.Exists(strVideo) == true)
                            {
                                if (ffmpegEncoder.AddSource(O.GetBarcode(false), strVideo, dxTertiary.GetExtent().Start, dxTertiary.GetExtent().Stop) == false)
                                {
                                    OnError(m_tmaxErrorBuilder.Message(ERROR_ADD_WMV_SOURCE, O.GetBarcode(false)), false);
                                    bEncode = false;
                                    break;
                                }

                            }
                            else
                            {
                                OnError(m_tmaxErrorBuilder.Message(ERROR_VIDEO_FILE_NOT_FOUND, O.GetBarcode(false), strVideo), false);
                                bEncode = false;
                                break;

                            }// if(System.IO.File.Exists(strVideo) == true)

                        }// if((strVideo.Length > 0) && (dxTertiary.GetExtent() != null))

                    }// foreach(CDxSecondary O in dxScenes)

                    //	Should we skip encoding
                    if (bEncode == false) break;

                    if (ffmpegEncoder.Encode() == false)
                    {
                        /*
						if(wmEncoder.Cancelled == false)
						{
							//	Did the encoder fail to process all source groups?
							if(wmEncoder.Completed < wmEncoder.SourceGroups.Count)
								OnError(m_tmaxErrorBuilder.Message(ERROR_WMV_INCOMPLETE, dxScript.GetBarcode(false), wmEncoder.Completed, wmEncoder.SourceGroups.Count), false);
							else
								OnError(m_tmaxErrorBuilder.Message(ERROR_EXECUTE_WMV_ENCODER, dxScript.GetBarcode(false)), false);
						}
                         */
                        break;
                    }

                    //	We're done
                    bSuccessful = true;

                    // reset the property
                    m_bIsMpeg2Selected = false;

                }//	while(bSuccessful = false)					

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_AS_WMV_EX, dxScript.GetBarcode(false)), false);
            }
            finally
            {
                //	Clean up
                if (ffmpegEncoder != null)
                {
                    ffmpegEncoder.Clear();
                    ffmpegEncoder = null;
                }

            }

            return bSuccessful;

        }

        // private bool ExportAsWMV(CDxPrimary dxScript, CDxSecondaries dxScenes, string strFileSpec)

        /// <summary>This method is called to export the specified script to an EDL clip descriptor file</summary>
        /// <param name="dxScript">The script to be exported</param>
        /// <param name="dxScenes">The collection of scenes to be exported</param>
        /// <param name="strFileSpec">The path to the export file</param>
        /// <returns>True if successful</returns>
        private bool ExportAsEDL(CDxPrimary dxScript, CDxSecondaries dxScenes, string strFileSpec)
        {
            bool bSuccessful = false;
            CDxSecondaries dxSegments = null;
            CDxTertiary dxTertiary = null;
            string strLine = "";
            string strFileTag = "";

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(dxScript != null);
            Debug.Assert(dxScenes != null);
            Debug.Assert(dxScenes.Count > 0);
            Debug.Assert(strFileSpec != null);
            Debug.Assert(strFileSpec.Length > 0);

            //	Make the status form visible
            SetStatus("Exporting " + dxScript.GetBarcode(false) + " to EDL ...");

            try
            {
                //	Substitute the appropriate extension for the WMV file
                strFileSpec = System.IO.Path.ChangeExtension(strFileSpec, ".edl");

                //	Delete the existing file
                if (System.IO.File.Exists(strFileSpec) == true)
                {
                    try { System.IO.File.Delete(strFileSpec); }
                    catch { }
                }

                SetStatusFilename(System.IO.Path.GetFileName(strFileSpec));

                //	Allocate the working collections
                dxSegments = new CDxSecondaries();

                //	Locate all clips and segments
                foreach (CDxSecondary O in dxScenes)
                {
                    if ((dxTertiary = ((CDxTertiary)(O.GetSource()))) != null)
                    {
                        //	Do we have the information we need?
                        if ((dxTertiary.Secondary != null) && (dxTertiary.GetExtent() != null))
                        {
                            if (dxSegments.Contains(dxTertiary.Secondary) == false)
                                dxSegments.AddList(dxTertiary.Secondary);
                        }

                    }

                }// foreach(CDxSecondary O in dxScenes)

                //	Make sure the user has not aborted the operation
                if (CheckAborted() == true) return false;

                //	Open the new stream
                if (OpenStream(strFileSpec) == false)
                    return false;

                //	Write the EDL header
                m_streamWriter.WriteLine("# TYPE=EDL");
                m_streamWriter.WriteLine("# VERSION=3.0.0");
                m_streamWriter.WriteLine("");

                //	Do we have any scenes to write to the file?
                if (dxScenes.Count == 0)
                {
                    m_streamWriter.WriteLine("# NO CLIPS FOUND");
                }
                else
                {
                    //	Write the video file segments
                    m_streamWriter.WriteLine("# VIDEO FILE TAGS");
                    m_streamWriter.WriteLine("");
                    foreach (CDxSecondary O in dxSegments)
                    {
                        strFileTag = O.GetBarcode(false).ToUpper();
                        strFileTag = strFileTag.Replace(' ', '_');

                        m_streamWriter.WriteLine("# " + strFileTag + " = " + m_tmaxDatabase.GetFileSpec(O));
                    }
                    m_streamWriter.WriteLine("");
                    m_streamWriter.WriteLine("");

                    //	Write the clip information
                    foreach (CDxSecondary O in dxScenes)
                    {
                        dxTertiary = (CDxTertiary)(O.GetSource());

                        strFileTag = dxTertiary.Secondary.GetBarcode(false).ToUpper();
                        strFileTag = strFileTag.Replace(' ', '_');

                        strLine = String.Format("{0} VA C {1:.#}s {2:.#}s",
                            strFileTag,
                            dxTertiary.GetExtent().Start,
                            dxTertiary.GetExtent().Stop);

                        m_streamWriter.WriteLine("# " + O.GetBarcode(false) + "  (" + dxTertiary.Name + ")");
                        m_streamWriter.WriteLine(strLine);
                        m_streamWriter.WriteLine("");

                    }// foreach(CDxSecondary O in dxScenes)

                }// if(dxScenes.Count == 0)

                bSuccessful = true;

            }
            catch
            {
            }
            finally
            {
                CloseStream();
            }

            return bSuccessful;

        }// private bool ExportEDL(CDxPrimary dxScript, CDxSecondaries dxScenes, string strFileSpec)

        /// <summary>This method is called to export the specified script to an SAMI clip descriptor file</summary>
        /// <param name="dxScript">The script to be exported</param>
        /// <param name="dxScenes">The collection of scenes to be exported</param>
        /// <param name="strFileSpec">The path to the export file</param>
        /// <returns>True if successful</returns>
        private bool ExportAsSAMI(CDxPrimary dxScript, CDxSecondaries dxScenes, string strFileSpec)
        {
            CWMSAMI wmSAMI = null;
            bool bSuccessful = false;
            bool bSceneError = false;
            CDxTertiary dxTertiary = null;
            string strDesignation = "";
            CDxHighlighter dxHighlighter = null;
            System.Drawing.Color highlighterColor = System.Drawing.Color.Yellow;
            string strMessage = "";
            string strName = "";
            double dStart = 0;
            double dStop = 0;

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(dxScript != null);
            Debug.Assert(dxScenes != null);
            Debug.Assert(dxScenes.Count > 0);
            Debug.Assert(strFileSpec != null);
            Debug.Assert(strFileSpec.Length > 0);

            //	Make the status form visible
            SetStatus("Exporting " + dxScript.GetBarcode(false) + " to SAMI ...");

            try
            {
                //	What name do we want to use for the conversion?
                if (dxScript.Name.Length > 0)
                    strName = dxScript.Name;
                else
                    strName = dxScript.MediaId;

                //	Substitute the appropriate extension for the SAMI file
                strFileSpec = System.IO.Path.ChangeExtension(strFileSpec, CWMSAMI.SAMI_DEFAULT_EXTENSION);

                //	Delete the existing file
                if (System.IO.File.Exists(strFileSpec) == true)
                {
                    try { System.IO.File.Delete(strFileSpec); }
                    catch { }
                }

                SetStatusFilename(System.IO.Path.GetFileName(strFileSpec));

                //	Use our custom encoder wrapper to perform the operation
                wmSAMI = new CWMSAMI();
                m_tmaxEventSource.Attach(wmSAMI.EventSource);

                //	Set the user defined options
                wmSAMI.FontFamily = m_tmaxExportOptions.SAMIFontFamily;
                wmSAMI.FontColor = m_tmaxExportOptions.SAMIFontColor;
                wmSAMI.FontSize = m_tmaxExportOptions.SAMIFontSize;
                wmSAMI.FontHighlighter = m_tmaxExportOptions.SAMIFontHighlighter;
                wmSAMI.VisibleLines = m_tmaxExportOptions.SAMILines;
                wmSAMI.PageNumbers = m_tmaxExportOptions.SAMIPageNumbers;

                while (bSuccessful == false)
                {
                    //	Make sure the user has not aborted the operation
                    if (CheckAborted() == true) return false;

                    //	Initialize for this operation
                    if (wmSAMI.Initialize(strName, strFileSpec) == false)
                    {
                        strMessage = wmSAMI.GetLastErrorMessage(false);
                        if (strMessage.Length == 0)
                            strMessage = "The attempt to initialize the SAMI converter object failed";
                        break;
                    }

                    //	Add a source descriptor for each scene
                    foreach (CDxSecondary O in dxScenes)
                    {
                        //	This collection should only contain valid designations and clips
                        Debug.Assert(O.GetSource() != null, "NULL SOURCE");
                        if (O.GetSource() == null) continue;
                        Debug.Assert((O.GetSource().MediaType == TmaxMediaTypes.Designation) || (O.GetSource().MediaType == TmaxMediaTypes.Clip));
                        if ((O.GetSource().MediaType != TmaxMediaTypes.Designation) &&
                            (O.GetSource().MediaType != TmaxMediaTypes.Clip)) continue;

                        dxTertiary = (CDxTertiary)(O.GetSource());

                        //	Get the playback extents
                        dStart = dxTertiary.GetExtent().Start;
                        dStop = dxTertiary.GetExtent().Stop;

                        //	Get the path to the XML designation
                        strDesignation = m_tmaxDatabase.GetFileSpec(dxTertiary);
                        if ((strDesignation == null) || (strDesignation.Length == 0)) continue;

                        //	Does the file exist?
                        if (System.IO.File.Exists(strDesignation) == false)
                        {
                            if (m_tmaxDatabase.CreateXmlDesignation(dxTertiary, strDesignation) == false)
                            {
                                bSceneError = true;
                                strMessage = String.Format("Unable to create XML designation for {0}: filename = {1}", dxTertiary.GetBarcode(false), strDesignation);
                                break;
                            }

                        }// if(System.IO.File.Exists(strDesignation) == false)

                        //	Get the highlighter color
                        try
                        {
                            if ((dxHighlighter = m_tmaxDatabase.Highlighters.Find(dxTertiary.GetExtent().HighlighterId)) == null)
                                dxHighlighter = m_tmaxDatabase.Highlighters[0];
                            if (dxHighlighter != null)
                                highlighterColor = dxHighlighter.GetSysColor();
                        }
                        catch
                        {
                        }

                        //	Add a source
                        if (wmSAMI.AddSource(strDesignation, dStart, dStop, highlighterColor) == null)
                        {
                            bSceneError = true;
                            strMessage = wmSAMI.GetLastErrorMessage(false);
                            break;
                        }

                    }// foreach(CDxSecondary O in dxScenes)

                    //	Should we skip encoding
                    if (bSceneError == true) break;

                    if (wmSAMI.Save() == false)
                    {
                        strMessage = wmSAMI.GetLastErrorMessage(false);
                        break;
                    }

                    //	We're done
                    bSuccessful = true;

                }//	while(bSuccessful = false)		

                if (bSuccessful == false)
                {
                    OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_AS_SAMI_FAILED, dxScript.GetBarcode(false), strMessage), false);
                }

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_AS_SAMI_EX, dxScript.GetBarcode(false)), false);
            }
            finally
            {
                //	Clean up
                if (wmSAMI != null)
                {
                    wmSAMI.Clear();
                    wmSAMI = null;
                }

            }

            return bSuccessful;

        }// private bool ExportSAMI(CDxPrimary dxScript, CDxSecondaries dxScenes, string strFileSpec)

        /// <summary>This method is called to export the specified media record</summary>
        /// <param name="dxMedia">The media record to be exported</param>
        /// <returns>True if successful</returns>
        private bool ExportAscii(CDxMediaRecord dxMedia)
        {
            bool bSuccessful = false;
            bool bFilled = false;
            string strLine = "";
            CDxPrimary dxScript = null;
            CDxTertiary dxTertiary = null;

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(m_streamWriter != null);
            Debug.Assert(dxMedia != null);

            //	Only know how to export scripts right now
            if (dxMedia.MediaType != TmaxMediaTypes.Script) return false;
            dxScript = (CDxPrimary)dxMedia;

            //	Make the status form visible
            SetStatus("Exporting " + dxMedia.GetBarcode(false));

            try
            {
                //	Fill the child collection if necessary
                if ((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
                {
                    dxScript.Fill();
                    bFilled = true;
                }

                //	Write all scenes to the file
                foreach (CDxSecondary O in dxScript.Secondaries)
                {
                    //	Must have a valid source record
                    if (O.GetSource() == null) continue;

                    if (O.GetSource().MediaType == TmaxMediaTypes.Designation)
                    {
                        dxTertiary = (CDxTertiary)O.GetSource();

                        //	Need to be able to get to the parent deposition
                        Debug.Assert(dxTertiary.Secondary != null);
                        if (dxTertiary.Secondary == null) continue;
                        Debug.Assert(dxTertiary.Secondary.Primary != null);
                        if (dxTertiary.Secondary.Primary == null) continue;

                        //	Must have the extents
                        Debug.Assert(dxTertiary.GetExtent() != null);
                        if (dxTertiary.GetExtent() == null) continue;

                        //	Format the line
                        strLine = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                            CTmaxToolbox.PLToPage(dxTertiary.StartPL),
                            CTmaxToolbox.PLToLine(dxTertiary.StartPL),
                            CTmaxToolbox.PLToPage(dxTertiary.StopPL),
                            CTmaxToolbox.PLToLine(dxTertiary.StopPL),
                            dxTertiary.Secondary.Primary.MediaId,
                            dxTertiary.GetExtent().HighlighterId);

                        //	Add the tuning information
                        if (dxTertiary.StartTuned == true)
                            strLine += ("\t" + dxTertiary.Start.ToString());
                        else
                            strLine += ("\t-1.0");
                        if (dxTertiary.StopTuned == true)
                            strLine += ("\t" + dxTertiary.Stop.ToString());
                        else
                            strLine += ("\t-1.0");

                        m_streamWriter.WriteLine(strLine);
                        m_lExported++;
                    }
                    else if (O.GetSource().MediaType == TmaxMediaTypes.Clip)
                    {
                        dxTertiary = (CDxTertiary)O.GetSource();

                        //	Need to be able to get to the parent segment
                        Debug.Assert(dxTertiary.Secondary != null);
                        if (dxTertiary.Secondary == null) continue;

                        //	Must have the extents
                        Debug.Assert(dxTertiary.GetExtent() != null);
                        if (dxTertiary.GetExtent() == null) continue;

                        //	Format the line
                        strLine = String.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                            dxTertiary.Start.ToString(),
                            dxTertiary.Stop.ToString(),
                            dxTertiary.StartTuned == true ? "1.0" : "-1.0",
                            dxTertiary.StopTuned == true ? "1.0" : "-1.0",
                            dxTertiary.Secondary.GetBarcode(true));

                        m_streamWriter.WriteLine(strLine);
                        m_lExported++;
                    }
                    else
                    {
                        //	Just write the source barcode
                        m_streamWriter.WriteLine(O.GetSource().GetBarcode(true));
                        m_lExported++;
                    }

                }// foreach(CDxSecondary O in dxScript.Secondaries)

                bSuccessful = true;
            }
            catch
            {
            }
            finally
            {
                if (bFilled == true)
                    dxScript.Secondaries.Clear();
            }

            return bSuccessful;

        }// private bool ExportAscii(CDxMediaRecord dxMedia)

        /// <summary>This method is called to export the specified binder record</summary>
        /// <param name="dxBinder">The binder record to be exported</param>
        /// <returns>True if successful</returns>
        private bool ExportAscii(CDxBinderEntry dxBinder)
        {
            bool bSuccessful = true;
            bool bFilled = false;

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(m_streamWriter != null);
            Debug.Assert(dxBinder != null);

            //	Only format supported for binders is Text
            Debug.Assert(m_eFormat == TmaxExportFormats.AsciiMedia);
            if (m_eFormat != TmaxExportFormats.AsciiMedia) return false;

            //	Make the status form visible
            SetStatus("Exporting " + dxBinder.Name);

            try
            {
                //	Fill the binder if necessary
                if ((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
                {
                    dxBinder.Fill();
                    bFilled = true;
                }

                //	Export all contents to the file
                foreach (CDxBinderEntry O in dxBinder.Contents)
                {
                    if (O.IsMedia() == true)
                    {
                        if (O.GetSource(true) != null)
                        {
                            m_streamWriter.WriteLine(O.GetSource(true).GetBarcode(true));
                            m_lExported++;
                        }

                    }
                    else
                    {
                        //	Drill the subbinders
                        if ((bSuccessful = ExportAscii(O)) == false)
                            break;
                    }

                }// foreach(CDxBinderEntry O in dxBinder)

            }
            catch
            {
            }
            finally
            {
                if (bFilled == true)
                    dxBinder.Contents.Clear();
            }

            return bSuccessful;

        }// private bool ExportAscii(CDxBinderEntry dxBinder)

        /// <summary>This method sets the source collection for the operation</summary>
        /// <param name="tmaxSource">Collection of event items that identify the source records</param>
        /// <returns>true if successful</returns>
        private bool SetSource(CTmaxItems tmaxSource)
        {
            m_tmaxSource = tmaxSource;
            m_dxInitial = null;

            //	Did the caller provide a valid source collection?
            if ((tmaxSource != null) && (tmaxSource.Count > 0))
            {
                //	Are we exporting pick lists?
                if (tmaxSource[0].DataType == TmaxDataTypes.PickItem)
                {
                    if (tmaxSource[0].PickItem != null)
                        m_dxInitial = ((CBaseRecord)(tmaxSource[0].PickItem.DxRecord));
                }
                if (tmaxSource[0].DataType == TmaxDataTypes.CaseCode)
                {
                    //	Nothing to do for case codes
                }
                else
                {
                    //	Is the first record a media record?
                    if ((m_dxInitial = (CBaseRecord)(tmaxSource[0].GetMediaRecord())) == null)
                    {
                        //	Must be a binder
                        m_dxInitial = (CBaseRecord)(tmaxSource[0].IBinderEntry);
                    }

                }// if(tmaxSource[0].DataType == TmaxDataTypes.PickItem)

            }// if((tmaxSource != null) && (tmaxSource.Count > 0))

            //	What is the export format?
            switch (m_eFormat)
            {
                case TmaxExportFormats.BarcodeMap:

                    //	Make sure there are records to be exported
                    if (m_tmaxDatabase.BarcodeMap != null)
                    {
                        //	Do we need to try filling the collection?
                        if (m_tmaxDatabase.BarcodeMap.Count == 0)
                            m_tmaxDatabase.BarcodeMap.Fill();

                        //	Do we have any records to export?
                        if (m_tmaxDatabase.BarcodeMap.Count > 0)
                            return true;
                    }

                    //	Must not be anything to export
                    MessageBox.Show("No entries in the barcode map for the export operation", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;

                case TmaxExportFormats.Codes:
                case TmaxExportFormats.CodesDatabase:

                    //	As long as we have primaries we have something to export
                    if (m_tmaxDatabase.Primaries != null)
                    {
                        //	Do we have any records to export?
                        if (m_tmaxDatabase.Primaries.Count > 0)
                            return true;
                    }

                    //	Must not be anything to export
                    MessageBox.Show("No primary records to be exported", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;

                case TmaxExportFormats.XmlCaseCodes:

                    //	Do we have any case codes?
                    if (m_tmaxDatabase.CodesEnabled == true)
                    {
                        if (m_tmaxDatabase.CaseCodes != null)
                            if (m_tmaxDatabase.CaseCodes.Count > 0)
                                return true;

                        if (m_tmaxDatabase.PickLists != null)
                            if (m_tmaxDatabase.PickLists.Children != null)
                                if (m_tmaxDatabase.PickLists.Children.Count > 0)
                                    return true;
                    }

                    //	Must not be anything to export
                    MessageBox.Show("No case codes or pick lists to be exported", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;

                case TmaxExportFormats.AsciiPickList:
                    if (tmaxSource != null && tmaxSource[0].DataType == TmaxDataTypes.PickItem)
                    {
                        if (m_tmaxDatabase.PickLists != null)
                            if (m_tmaxDatabase.PickLists.Children != null && tmaxSource[0].PickItem != null)
                                if (m_tmaxDatabase.PickLists.Children.Find(tmaxSource[0].PickItem.Name) != null)
                                    if (m_tmaxDatabase.PickLists.Children.Find(tmaxSource[0].PickItem.Name).Children.Count > 0)
                                        return true;
                    }

                    return false;
                case TmaxExportFormats.AsciiMedia:
                case TmaxExportFormats.Video:
                case TmaxExportFormats.LoadFile:
                case TmaxExportFormats.XmlBinder:
                case TmaxExportFormats.XmlScript:
                default:

                    return (m_dxInitial != null);

            }// switch(m_eFormat)

        }// private bool SetSource(CTmaxItems tmaxSource)

        /// <summary>This method is called to create the status form for the operation</summary>
        /// <returns>true if successful</returns>
        private bool CreateStatusForm()
        {
            try
            {
                //	Clear the Aborted flag
                m_bAborted = false;

                //	Make sure the previous instance is disposed
                if (m_wndStatus != null)
                {
                    if (m_wndStatus.IsDisposed == false)
                        m_wndStatus.Dispose();
                    m_wndStatus = null;
                }

                //	Create a new instance
                m_wndStatus = new FTI.Trialmax.Forms.CFExportStatus();
                m_wndStatus.Title = "Export Status";

                //	Set the initial status message
                SetStatus("Initializing export operation ...");
            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_CREATE_STATUS_FORM_EX));
                m_wndStatus = null;
            }

            return (m_wndStatus != null);

        }// private bool CreateStatusForm()

        /// <summary>This method is called to update the status text on the status form</summary>
        /// <param name="strStatus">The new status message</param>
        private void SetStatus(string strStatus)
        {
            try
            {
                if ((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
                {
                    m_wndStatus.Status = strStatus;
                    m_wndStatus.Refresh();
                }

            }
            catch
            {
            }

        }// private void SetStatus(string strStatus)

        /// <summary>This method is called to show/hide the status form</summary>
        /// <param name="bVisible">true if visible</param>
        /// <param name="strStatus">optional status message </param>
        private void SetStatusVisible(bool bVisible, string strStatus)
        {
            try
            {
                if ((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
                {
                    if (strStatus != null)
                        m_wndStatus.Status = strStatus;

                    if (bVisible == true)
                    {
                        m_wndStatus.Show();
                        m_wndStatus.Refresh();
                    }
                    else
                    {
                        m_wndStatus.Hide();
                    }

                }

            }
            catch
            {
            }

        }// private void SetStatusVisible(bool bVisible, string strStatus)

        /// <summary>This method is called to show/hide the status form</summary>
        /// <param name="bVisible">true if visible</param>
        private void SetStatusVisible(bool bVisible)
        {
            SetStatusVisible(bVisible, null);
        }

        /// <summary>This method is called to set the operation summary message</summary>
        /// <param name="strSummary">The new summary message</param>
        private void SetSummary(string strSummary)
        {
            try
            {
                if ((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
                {
                    m_wndStatus.Summary = strSummary;
                }

            }
            catch
            {
            }

        }// private void SetSummary(string strSummary)

        /// <summary>This method is called to set the filename displayed in the status form</summary>
        /// <param name="strFilename">The new filename</param>
        private void SetStatusFilename(string strFilename)
        {
            try
            {
                if ((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
                {
                    m_wndStatus.Filename = strFilename;
                }

            }
            catch
            {
            }

        }// private void SetStatusFilename(string strFilename)

        /// <summary>This method is called to determine if the user has aborted the operation</summary>
        /// <return>True if aborted</return>
        private bool CheckAborted()
        {
            try
            {
                if (m_bAborted == false)
                {
                    if (m_wndStatus != null)
                    {
                        Application.DoEvents();
                        m_bAborted = m_wndStatus.Aborted;
                    }

                }
            }
            catch
            {
            }

            return m_bAborted;

        }// private bool CheckAborted()

        /// <summary>This method will close the active file stream</summary>
        private void CloseStream()
        {
            if (m_streamWriter != null)
            {
                try { m_streamWriter.Close(); }
                catch { }

                m_streamWriter = null;
            }

            if (m_tmaxCodesDatabase != null)
            {
                m_tmaxCodesDatabase.Close();
                m_tmaxCodesDatabase = null;
            }

        }// private void CloseStream()

        /// <summary>This method will open the file stream for the specified file</summary>
        /// <param name="strFileSpec">Fully qualified path to the desired file</param>
        /// <returns>true if successful</returns>
        private bool OpenStream(string strFileSpec)
        {
            System.IO.FileStream fs = null;

            //	Make sure the active stream is closed
            CloseStream();

            try
            {
                fs = new FileStream(strFileSpec, FileMode.Create);

                if (fs != null)
                {
                    //	Open the file stream				
                    m_streamWriter = new StreamWriter(fs, System.Text.Encoding.Default);
                    return true;
                }

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_FILE_OPEN_FAILED, strFileSpec));
            }

            return false;

        }// private bool OpenStream(string strFileSpec)

        /// <summary>This method will open the database for exporting fielded data (codes)</summary>
        /// <param name="strFileSpec">Fully qualified path to the desired file</param>
        /// <returns>true if successful</returns>
        private bool OpenCodesDatabase(string strFileSpec)
        {
            string strTemplate = "";
            bool bSuccessful = false;

            //	Make sure the active database is closed
            CloseStream();

            try
            {
                while (bSuccessful == false)
                {
                    //	Get the path to the template
                    strTemplate = m_tmaxDatabase.GetCodesDatabaseFileSpec();
                    if (System.IO.File.Exists(strTemplate) == false)
                    {
                        OnError(m_tmaxErrorBuilder.Message(ERROR_LOCATE_CODES_DATABASE_FAILED, strTemplate), true);
                        break;
                    }

                    //	Copy the template to the requested location
                    try
                    {
                        System.IO.File.Copy(strTemplate, strFileSpec, true);
                    }
                    catch
                    {
                        OnError(m_tmaxErrorBuilder.Message(ERROR_COPY_CODES_DATABASE_FAILED, strTemplate, strFileSpec), true);
                        break;
                    }

                    //	Create the database
                    m_tmaxCodesDatabase = new CTmaxCodesDatabase();
                    m_tmaxEventSource.Attach(m_tmaxCodesDatabase.EventSource);
                    m_tmaxCodesDatabase.ExportOptions = m_tmaxExportOptions;
                    m_tmaxCodesDatabase.CaseDatabase = m_tmaxDatabase;

                    //	Open the database
                    if (m_tmaxCodesDatabase.Open(strFileSpec) == false)
                    {
                        OnError(m_tmaxErrorBuilder.Message(ERROR_OPEN_CODES_DATABASE_FAILED, strFileSpec), true);
                        break;
                    }

                    //	Create the table to store the codes
                    if (m_tmaxCodesDatabase.CreateTable(m_tmaxExportOptions.Columns) == false)
                    {
                        OnError(m_tmaxErrorBuilder.Message(ERROR_CREATE_CODES_TABLE_FAILED, strFileSpec), true);
                        break;
                    }

                    //	We're done
                    bSuccessful = true;

                }// while(bSuccessful == false)

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_OPEN_CODES_DATABASE_EX, strFileSpec));
            }

            return bSuccessful;

        }// private bool OpenCodesDatabase(string strFileSpec)

        /// <summary>This method retrieves the record to be exported from the event item</summary>
        /// <param name="tmaxItem">The event item that represent the record</param>
        /// <returns>The record if found</returns>
        private CDxMediaRecord GetRecord(CTmaxItem tmaxItem)
        {
            CDxMediaRecord dxRecord = null;

            //	Is this a media record?
            if ((dxRecord = (CDxMediaRecord)(tmaxItem.GetMediaRecord())) == null)
            {
                dxRecord = (CDxMediaRecord)(tmaxItem.IBinderEntry);
            }

            return dxRecord;

        }// private CDxMediaRecord GetRecord(CTmaxItem tmaxItem)

        /// <summary>This method is called to get all primary records of the specified media type</summary>
        /// <param name="eType">The desired media type</param>
        /// <param name="dxPrimaries">The collection in which to store the requested records</param>
        /// <returns>True if successful</returns>
        private bool GetPrimaries(TmaxMediaTypes eType, CDxPrimaries dxPrimaries)
        {
            Debug.Assert(m_tmaxDatabase != null);

            if (m_tmaxDatabase == null) return false;
            if (m_tmaxDatabase.Primaries == null) return false;

            try
            {
                //	Check each primary in the active database
                foreach (CDxPrimary O in m_tmaxDatabase.Primaries)
                {
                    if (O.MediaType == eType)
                    {
                        if (dxPrimaries.Contains(O) == false)
                            dxPrimaries.AddList(O);
                    }

                }// foreach(CDxPrimary O in m_tmaxDatabase.Primaries)

                return true;

            }
            catch
            {
                return false;
            }

        }// private bool GetPrimaries(TmaxMediaTypes eType, CDxPrimaries dxPrimaries)

        /// <summary>This method is called to get all primary records in the specified binder</summary>
        /// <param name="dxBinder">The binder to be checked</param>
        /// <param name="dxPrimaries">The collection in which to store the requested records</param>
        /// <param name="bSubBinders">true to include subbinders</param>
        /// <returns>True if successful</returns>
        private bool GetPrimaries(CDxBinderEntry dxBinder, CDxPrimaries dxPrimaries, bool bSubBinders)
        {
            bool bSuccessful = false;
            bool bFilled = false;
            CTmaxItem tmaxItem = null;

            Debug.Assert(m_tmaxDatabase != null);

            if (m_tmaxDatabase == null) return false;
            if (m_tmaxDatabase.Primaries == null) return false;

            try
            {
                tmaxItem = new CTmaxItem();

                //	Make sure the binder contents have been expanded
                if (dxBinder.Contents.Count == 0)
                {
                    dxBinder.Fill();
                    bFilled = true;
                }

                //	Check each child in the binder
                foreach (CDxBinderEntry O in dxBinder.Contents)
                {
                    if (O.IsMedia() == true)
                    {
                        if (O.GetSource(true) != null)
                        {
                            //	This is an easy way to bust out the primary interface
                            tmaxItem.SetRecord(O.GetSource(true));

                            if (tmaxItem.IPrimary != null)
                            {
                                if (dxPrimaries.Contains((CDxPrimary)(tmaxItem.IPrimary)) == false)
                                {
                                    dxPrimaries.AddList((CDxPrimary)(tmaxItem.IPrimary));
                                }

                            }// if(tmaxItem.IPrimary != null)

                        }// if(O.GetSource() != null)

                    }
                    else
                    {
                        //	Are we including subbinders?
                        if (bSubBinders == true)
                            GetPrimaries(O, dxPrimaries, bSubBinders);

                    }// if(O.IsMedia() == true)

                }// foreach(CDxBinderEntry O in dxBinder.Contents)

                bSuccessful = true;

            }
            catch
            {
            }

            //	Did we fill the binder contents?
            if (bFilled == true)
                dxBinder.Contents.Clear();

            return bSuccessful;

        }// private bool GetPrimaries(CDxBinderEntry dxBinder, CDxPrimaries dxPrimaries, bool bSubBinders)

        /// <summary>This method will reset the local members to their default values</summary>
        private void Reset()
        {
            //	Close the file
            CloseStream();

            m_tmaxSource = null;
            m_dxInitial = null;
            m_lExported = 0;
            m_lFiles = 0;
            m_eFormat = TmaxExportFormats.Unknown;
            m_bTerminate = false;
            m_bConfirmOverwrite = true;
            m_bAutoFilenames = false;
            m_strExportFolder = "";

            //	Destroy the status form
            if (m_wndStatus != null)
            {
                if (m_wndStatus.IsDisposed == false)
                    m_wndStatus.Dispose();
                m_wndStatus = null;
            }

            //	Never reset the file specification because we use it to 
            //	initialize the file selection dialog

        }// private void Reset()

        /// <summary>This method will get the path to the export file</summary>
        ///	<param name="dxExport">Exchange interface to the record used to create the file</param>
        /// <returns>The fully qualified path to the export file</returns>
        private string GetExportFileSpec(CBaseRecord dxExport)
        {
            SaveFileDialog saveFile = null;
            string strFilename = "";

            try
            {
                //	Get the defaults for initializing the dialog box
                strFilename = GetDefaultFilename(dxExport);

                //	Make sure no illegal characters are in the filename
                strFilename = CTmaxToolbox.CleanFilename(strFilename, false);

                //	Should we try to auto-assign the filename?
                //
                //	NOTE:	We use the ExportFolder member to be sure that the
                //			user gets prompted at least once so that we can
                //			determine the output folder
                if ((m_strExportFolder.Length > 0) && (m_bAutoFilenames == true))
                {
                    //	Add the extension
                    //
                    //	NOTE:	This would normally be done by the file open dialog
                    if (m_strExtension.Length > 0)
                        strFilename += ("." + m_strExtension);

                    //	Build the fully qualified path
                    if (m_strExportFolder.EndsWith("\\") == true)
                        strFilename = (m_strExportFolder + strFilename);
                    else
                        strFilename = String.Format("{0}\\{1}", m_strExportFolder, strFilename);

                    //	Make sure we can overwrite
                    if (ConfirmOverwrite(strFilename) == true)
                        return strFilename;
                }
                else
                {
                    //	Use the last output file to initialize the output folder
                    if ((m_strExportFolder.Length == 0) && (m_strFileSpec.Length > 0) && (System.IO.Path.IsPathRooted(m_strFileSpec) == true))
                        m_strExportFolder = System.IO.Path.GetDirectoryName(m_strFileSpec);
                }

                while (true)
                {
                    //	Initialize the file selection dialog
                    saveFile = new SaveFileDialog();
                    saveFile.AddExtension = true;
                    saveFile.CheckPathExists = true;
                    saveFile.OverwritePrompt = false;
                    saveFile.FileName = System.IO.Path.GetFileName(strFilename);
                    saveFile.DefaultExt = m_strExtension;
                    saveFile.Filter = m_strFileFilter;
                    saveFile.InitialDirectory = m_strExportFolder;

                    //	Open the dialog box
                    if (saveFile.ShowDialog() == DialogResult.Cancel)
                    {
                        strFilename = "";
                    }
                    else
                    {
                        strFilename = saveFile.FileName;
                        m_strExportFolder = System.IO.Path.GetDirectoryName(strFilename);
                        if (saveFile.FilterIndex == 4)
                            m_bIsMpeg2Selected = true;
                    }

                    //	Clean up
                    try { saveFile.Dispose(); }
                    catch { }
                    saveFile = null;
                    Application.DoEvents();

                    //	Stop here if cancelled by the user
                    if (strFilename.Length == 0) return "";

                    //	Confirm overwrite if necessary
                    if (ConfirmOverwrite(strFilename) == false)
                        continue;

                    break;

                }// while(true)

                return strFilename;

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_GET_EXPORT_FILESPEC_EX));
                return "";
            }

        }// private bool GetExportFileSpec(CBaseRecord dxExport)

        /// <summary>This method will get the path to the export file</summary>
        ///	<param name="dxExport">Exchange interface to the record used to create the file</param>
        /// <returns>true if the user selects a file for output</returns>
        private bool GetExportStream(CBaseRecord dxExport)
        {
            string strFileSpec = "";
            bool bSuccessful = false;

            try
            {
                while (bSuccessful == false)
                {
                    //	Get the path to the file
                    strFileSpec = GetExportFileSpec(dxExport);
                    if ((strFileSpec == null) || (strFileSpec.Length == 0))
                        break;

                    if (m_eFormat == TmaxExportFormats.CodesDatabase)
                    {
                        if (OpenCodesDatabase(strFileSpec) == false)
                            break;
                    }
                    else
                    {
                        if (OpenStream(strFileSpec) == false)
                            break;
                    }

                    //	Save the path to the file selected by the user
                    this.FileSpec = strFileSpec;

                    bSuccessful = true;

                }// while(bSuccessful == false)

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_GET_EXPORT_STREAM_EX));
            }

            return bSuccessful;

        }// private bool GetExportStream(CBaseRecord dxExport)

        /// <summary>Called to confirm if OK to overwrite existing file</summary>
        /// <param name="strFilespec">The fully qualified path to the export file</param>
        /// <returns>true if successful</returns>
        private bool ConfirmOverwrite(string strFilespec)
        {
            bool bSuccessful = false;
            string strMsg = "";

            try
            {
                //	Does this file already exist?
                if (System.IO.File.Exists(strFilespec) == false)
                    return true; // Nothing to overwrite

                //	Build the prompt for the user
                if (m_bConfirmOverwrite == true)
                    strMsg = String.Format("{0} already exists. Do you want to overwrite this file?", strFilespec);

                //	Do we need to prompt?
                if (strMsg.Length > 0)
                {
                    //	Prompt for confirmation
                    if (MessageBox.Show(strMsg, "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false; // User does not want to overwrite
                }

                //	Attempt to delete the file
                try
                {
                    System.IO.File.Delete(strFilespec);
                }
                catch
                {
                    strMsg = String.Format("Unable to delete {0} for overwrite", strFilespec);
                    MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false; // Pick a new filename
                }

                //	We're done
                bSuccessful = true;

            }
            catch
            {
            }

            return bSuccessful;

        }// private bool ConfirmOverwrite(string strFilespec)

        /// <summary>This method will get the default filename to use for the specified record</summary>
        ///	<param name="dxRecord">the record used to create the filename</param>
        /// <returns>the default filename</returns>
        private string GetDefaultFilename(CBaseRecord dxRecord)
        {
            string strFilename = "";

            //	Get the filename to use for initializing the dialog box
            try
            {
                if (dxRecord != null)
                {
                    //	Is this a pick list?
                    if (dxRecord.GetDataType() == TmaxDataTypes.PickItem)
                    {
                        //	Use the name as the default filename
                        strFilename = ((CDxPickItem)(dxRecord)).Name;
                    }
                    //	Is this a media record?
                    else if (dxRecord.GetDataType() == TmaxDataTypes.Media)
                    {
                        //	Build the default filename
                        strFilename = ((CDxMediaRecord)dxRecord).GetBarcode(false);
                        if (((CDxMediaRecord)dxRecord).GetName().Length > 0)
                            strFilename += (" " + ((CDxMediaRecord)dxRecord).GetName());
                    }
                    else if (dxRecord.GetDataType() == TmaxDataTypes.Binder)
                    {
                        strFilename = ((CDxMediaRecord)dxRecord).GetName();
                    }

                }
                else
                {
                    if (m_eFormat == TmaxExportFormats.BarcodeMap)
                        strFilename = "barcode_map";
                    else if (m_eFormat == TmaxExportFormats.XmlCaseCodes)
                        strFilename = "data_fields";
                    else if (m_eFormat == TmaxExportFormats.XmlBinder)
                        strFilename = "trialmax_binders";
                }

            }
            catch
            {
            }

            return strFilename;

        }// private string GetDefaultFilename(CBaseRecord dxRecord)

        /// <summary>This method is called to prompt the user for the options used to export metadata</summary>
        /// <returns>true to continue, false to cancel</returns>
        private bool GetCodesOptions()
        {
            FTI.Trialmax.Forms.CFExportCodes exportOptions = null;
            bool bContinue = false;

            try
            {
                exportOptions = new CFExportCodes();

                m_tmaxEventSource.Attach(exportOptions.EventSource);
                exportOptions.CaseCodes = this.Database.CaseCodes;
                exportOptions.ExportOptions = m_tmaxExportOptions;
                exportOptions.ExportAsDatabase = (m_eFormat == TmaxExportFormats.CodesDatabase);

                if (exportOptions.ShowDialog() == DialogResult.OK)
                    bContinue = true;
            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_GET_CODES_OPTIONS_EX));
                return false;
            }

            return bContinue;

        }// private bool GetCodesOptions()

        /// <summary>This method is called to prompt the user for the options used to export metadata</summary>
        /// <returns>true to continue, false to cancel</returns>
        private bool GetXmlCaseCodesOptions()
        {
            FTI.Trialmax.Forms.CFExportXmlCaseCodes exportOptions = null;
            bool bContinue = false;

            try
            {
                exportOptions = new CFExportXmlCaseCodes();

                m_tmaxEventSource.Attach(exportOptions.EventSource);
                exportOptions.ExportOptions = m_tmaxExportOptions;

                if (exportOptions.ShowDialog() == DialogResult.OK)
                    bContinue = true;

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_GET_XML_CASE_CODES_OPTIONS_EX));
                return false;
            }

            return bContinue;

        }// private bool GetXmlCaseCodesOptions()

        /// <summary>This method is called to prompt the user for the options used to export metadata</summary>
        /// <returns>true to continue, false to cancel</returns>
        private bool GetVideoOptions()
        {
            FTI.Trialmax.Forms.CFExportVideo exportOptions = null;
            bool bContinue = false;

            try
            {
                exportOptions = new CFExportVideo();

                m_tmaxEventSource.Attach(exportOptions.EventSource);
                exportOptions.ExportOptions = m_tmaxExportOptions;
                exportOptions.WMEncoder = this.WMEncoder;

                if (exportOptions.ShowDialog() == DialogResult.OK)
                {
                    //	Set up default file filters and extension for prompting the user
                    if (m_tmaxExportOptions.VideoWMV == true)
                    {
                        m_strExtension = "wmv";
                        m_strFileFilter = "WMV Files (*.wmv)|*.wmv|MP4 Files (*.mp4)|*.mp4|MPEG-1 Files (*.mpg)|*.mpg|MPEG-2 Files (*.mpg)|*.mpg|AVI Files (*.avi)|*.avi|MOV Files (*.mov)|*.mov|";
                    }
                    if (m_tmaxExportOptions.VideoEDL == true)
                    {
                        if (m_strExtension.Length == 0) m_strExtension = "edl";
                        m_strFileFilter += "EDL Files (*.edl)|*.edl|";
                    }
                    if (m_tmaxExportOptions.VideoSAMI == true)
                    {
                        if (m_strExtension.Length == 0) m_strExtension = "smi";
                        m_strFileFilter += "SMI Files (*.smi)|*.smi|";
                    }

                    m_strFileFilter += "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                    m_bConfirmOverwrite = m_tmaxExportOptions.ConfirmOverwrite;
                    m_bAutoFilenames = m_tmaxExportOptions.AutoFilenames;

                    bContinue = true;

                }// if(exportOptions.ShowDialog() == DialogResult.OK)

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_GET_VIDEO_OPTIONS_EX));
                return false;
            }

            return bContinue;

        }// private bool GetVideoOptions()

        /// <summary>This method is called to prompt the user for the options used to export to text</summary>
        /// <returns>true to continue, false to cancel</returns>
        private bool GetAsciiOptions()
        {
            FTI.Trialmax.Forms.CFExportText exportOptions = null;
            bool bContinue = false;

            try
            {
                exportOptions = new CFExportText();

                m_tmaxEventSource.Attach(exportOptions.EventSource);
                exportOptions.ExportOptions = m_tmaxExportOptions;

                if (exportOptions.ShowDialog() == DialogResult.OK)
                {
                    m_bConfirmOverwrite = m_tmaxExportOptions.ConfirmOverwrite;
                    m_bAutoFilenames = m_tmaxExportOptions.AutoFilenames;

                    bContinue = true;

                }// if(exportOptions.ShowDialog() == DialogResult.OK)

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_GET_TEXT_OPTIONS_EX));
                return false;
            }

            return bContinue;

        }// private bool GetAsciiOptions()

        /// <summary>This method is called to prompt the user for the options used to export to XML</summary>
        /// <returns>true to continue, false to cancel</returns>
        private bool GetXmlOptions()
        {
            FTI.Trialmax.Forms.CFExportXml exportOptions = null;
            bool bContinue = false;

            try
            {
                exportOptions = new CFExportXml();

                m_tmaxEventSource.Attach(exportOptions.EventSource);
                exportOptions.ExportOptions = m_tmaxExportOptions;

                if (exportOptions.ShowDialog() == DialogResult.OK)
                {
                    m_bConfirmOverwrite = m_tmaxExportOptions.ConfirmOverwrite;
                    m_bAutoFilenames = m_tmaxExportOptions.AutoFilenames;
                    m_strExtension = CXmlScript.GetExtension(m_tmaxExportOptions.XmlScriptFormat);
                    m_strFileFilter = CXmlScript.GetFilter(m_tmaxExportOptions.XmlScriptFormat, true);

                    bContinue = true;

                }// if(exportOptions.ShowDialog() == DialogResult.OK)

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_GET_XML_OPTIONS_EX));
                return false;
            }

            return bContinue;

        }// private bool GetXmlOptions()

        /// <summary>This method is called to get a collection of scenes that are bound to video designations or clips</summary>
        /// <param name="dxScript">The script that owns the available scenes</param>
        /// <returns>The collection of scenes that are bound to designations or clips</returns>
        private CDxSecondaries GetVideoScenes(CDxPrimary dxScript)
        {
            CDxSecondaries dxScenes = null;

            Debug.Assert(m_tmaxDatabase != null);
            Debug.Assert(dxScript != null);

            try
            {
                //	NOTE:	This child collection should be filled before
                //			calling this method!

                //	Create the collection in which to store the scenes
                dxScenes = new CDxSecondaries();

                //	Locate all scenes that are bound to designations or clips
                foreach (CDxSecondary O in dxScript.Secondaries)
                {
                    if ((O.GetSource() != null) &&
                        ((O.GetSource().MediaType == TmaxMediaTypes.Clip) ||
                        (O.GetSource().MediaType == TmaxMediaTypes.Designation)))
                    {
                        dxScenes.AddList(O);
                    }

                }// foreach(CDxSecondary O in dxScript.Secondaries)

            }
            catch
            {
                OnError(m_tmaxErrorBuilder.Message(ERROR_GET_VIDEO_SCENES_EX, dxScript.MediaId));
                return null;
            }

            if ((dxScenes != null) && (dxScenes.Count > 0))
                return dxScenes;
            else
                return null;

        }// private CDxSecondaries GetVideoScenes(CDxPrimary dxScript)

        /// <summary>This method is called to populate the error builder's format string collection</summary>
        private void SetErrorStrings()
        {
            if (m_tmaxErrorBuilder == null) return;
            if (m_tmaxErrorBuilder.FormatStrings == null) return;

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the export operation");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the export file");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to launch the export operation.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the status form for the export operation");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to open %1 to perform the export operation");

            m_tmaxErrorBuilder.FormatStrings.Add("There are no barcode map entries to be exported");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while exporting the barcode map to %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while exporting fielded data to %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the options for the export codes operation");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the options for the export video operation");

            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export to video. The script does not contain any designations or clips: MediaID = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while extracting the scenes from the active script: MediaID = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the path to the export file.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the script as a WMV. MediaID = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export to WMV. The encoder could not be initialized. File = %1");

            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export to WMV. The encoder profile could not be set: Profile = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("The attempt to add a WMV encoder source for the scene failed: Scene = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("The attempt to execute the WMV encoder failed: MediaID = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the script to SAMI. MediaID = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("The attempt to export the script to SAMI failed. MediaID = %1\n\n%2");

            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the video file: Media ID: %1  Path: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while performing the export operation.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export %1 to load file. Commas not permitted in file path: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prompt for the export to text options");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export to XML");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prompt for the export to XML options");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export %1. The script does not contain any scenes.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export %1. Video Viewer only supports designations.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export %1. No source designation available: source barcode = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export %1 to XML");

            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export %1. Unable to save %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export %1. Video Viewer only supports one source deposition.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a source deposition for %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export %1. Unable to add the source deposition for %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Error exporting %1 to WMV. Only %2 of %3 designations were encoded.");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prompt for the export data fields to XML options");
            m_tmaxErrorBuilder.FormatStrings.Add("No data fields or pick lists are available for the export operation.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export data fields to %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to save data fields to %1. Internal field manager error.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the database for the operation: filename = %1");

            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the database template for fielded data: filename = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to copy the database template for fielded data: template = %1 target = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to open the database for fielded data: filename = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the database table for fielded data: filename = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("No binders available to export to XML.");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export binders to XML: filename = %1");

        }// private void SetErrorStrings()

        /// <summary>This method is called to handle an error</summary>
        /// <param name="strMessage">The error message to be displayed</param>
        /// <param name="bFatal">True if the error is fatal</param>
        /// <returns>true to terminate the operation</returns>
        private bool OnError(string strMessage, bool bFatal)
        {
            //	Add this message to the status form
            AddMessage(strMessage, bFatal ? TmaxMessageLevels.FatalError : TmaxMessageLevels.Warning);

            //	Is this a fatal error?
            if (bFatal == true)
            {
                strMessage += "\n\nThe operation will be cancelled";

                MessageBox.Show(m_wndStatus, strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                m_bTerminate = true;
            }
            else
            {
                strMessage += "\n\nDo you want to continue?";

                if (MessageBox.Show(m_wndStatus, strMessage, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    m_bTerminate = true;
            }

            return m_bTerminate;

        }// private bool OnError(string strMessage, bool bFatal)

        /// <summary>This method is called to handle a fatal error</summary>
        /// <param name="strMessage">The error message to be displayed</param>
        /// <returns>true to continue the operation</returns>
        private bool OnError(string strMessage)
        {
            return OnError(strMessage, true);
        }

        /// <summary>This method is called to handle a fatal error</summary>
        /// <param name="strMessage">The error message to be displayed</param>
        /// <returns>true to continue the operation</returns>
        private bool OnEncoderStatus(object sender, string strMessage)
        {
            if ((strMessage != null) && (strMessage.Length > 0))
                SetStatus(strMessage);

            //	Should we continue?
            return (CheckAborted() == false);

        }// private bool OnEncoderStatus(object sender, string strMessage)

        /// <summary>This method is called to get the collection of scripts to be exported to XML</summary>
        /// <param name="dxScripts">The collection in which to store the scripts to be exported</param>
        /// <returns>True if successful</returns>
        private bool GetSourceXmlScripts(CDxPrimaries dxScripts)
        {
            CDxMediaRecord dxSource = null;

            Debug.Assert(m_tmaxDatabase != null);

            if (m_tmaxSource == null) return false;
            if (m_tmaxSource.Count == 0) return false;

            //	Make the status form visible
            SetStatus("Loading scripts ...");

            try
            {
                //	Clear the caller's collection
                dxScripts.Clear();

                //	Export each of the records specified by the caller
                foreach (CTmaxItem O in m_tmaxSource)
                {
                    //	Get the source record to be exported
                    if ((dxSource = GetRecord(O)) == null) continue;

                    //	The source must be a binder or a script
                    if (dxSource.GetDataType() == TmaxDataTypes.Media)
                    {
                        if (dxSource.MediaType == TmaxMediaTypes.Script)
                        {

                            dxScripts.AddList((CDxPrimary)dxSource);
                        }

                    }
                    else if (dxSource.GetDataType() == TmaxDataTypes.Binder)
                    {
                        GetSourceXmlScripts(dxScripts, ((CDxBinderEntry)dxSource));
                    }

                }// foreach(CTmaxItem O in m_tmaxSource)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "GetSourceXmlScripts", m_tmaxErrorBuilder.Message(ERROR_GET_XML_SOURCE_EX), Ex);
                AddMessage(m_tmaxErrorBuilder.Message(ERROR_GET_XML_SOURCE_EX), TmaxMessageLevels.FatalError);
            }

            return (dxScripts.Count > 0);

        }// private bool GetSourceXmlScripts(CDxPrimaries dxScripts)

        /// <summary>This method is called to search the specified binder for scripts to be exported to XML</summary>
        /// <param name="dxScripts">The collection in which to store the scripts to be exported</param>
        /// <param name="dxBinder">The binder to be searched for valid scripts</param>
        /// <returns>True if successful</returns>
        private bool GetSourceXmlScripts(CDxPrimaries dxScripts, CDxBinderEntry dxBinder)
        {
            CDxMediaRecord dxSource = null;
            bool bFilled = false;

            //	Do we need to fill the binder contents?
            if ((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
            {
                dxBinder.Fill();
                bFilled = true;
            }
            if ((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
                return false;

            //	Search for all scripts contained in the binder
            foreach (CDxBinderEntry O in dxBinder.Contents)
            {
                if (O.IsMedia() == true)
                {
                    if ((dxSource = O.GetSource(true)) != null)
                    {
                        if (dxSource.MediaType == TmaxMediaTypes.Script)
                            dxScripts.AddList(dxSource);
                    }

                }
                else
                {
                    GetSourceXmlScripts(dxScripts, O);
                }

            }// foreach(CDxBinderEntry O in dxBinder.Contents)

            if (bFilled == true)
                dxBinder.Contents.Clear();

            return true;

        }// private bool GetSourceXmlScripts(CDxPrimaries dxScripts, CDxBinderEntry dxBinder)

        /// <summary>This method is called to build the collection of binders to be exported to XML</summary>
        /// <returns>The collection of binders to be exported</returns>
        private CTmaxBinderItems GetSourceXmlBinders()
        {
            CTmaxBinderItems tmaxBinders = null;
            CTmaxBinderItem tmaxBinder = null;

            try
            {
                tmaxBinders = new CTmaxBinderItems();

                //	Search the source collection for binders to be exported
                foreach (CTmaxItem O in m_tmaxSource)
                {
                    //	Can't be a media reference
                    if ((O.IPrimary == null) && (O.IBinderEntry != null))
                    {
                        if ((tmaxBinder = GetSourceXmlBinder((CDxBinderEntry)(O.IBinderEntry))) != null)
                            tmaxBinders.Add(tmaxBinder);
                    }


                }// foreach(CTmaxItem O in m_tmaxSource)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireDiagnostic(this, "GetSourceXmlBinders", Ex);
            }

            if ((tmaxBinders != null) && (tmaxBinders.Count > 0))
                return tmaxBinders;
            else
                return null;

        }// private CTmaxBinderItems GetSourceXmlBinders()

        /// <summary>This method is called to get the binder object to represent the binder to be exported</summary>
        /// <param name="dxBinder">The binder being exported to XML</param>
        /// <returns>The binder item used for exporting to XML</returns>
        private CTmaxBinderItem GetSourceXmlBinder(CDxBinderEntry dxBinder)
        {
            CTmaxBinderItem tmaxBinder = null;
            CTmaxBinderItem tmaxChild = null;
            bool bFilled = false;

            try
            {
                //	Allocate an item for this binder
                tmaxBinder = new CTmaxBinderItem();
                tmaxBinder.SetProperties(dxBinder);

                //	Override the default description because we only want user assigned
                //	values to be written to the XML file
                tmaxBinder.Description = dxBinder.GetDescription(true);

                //	Add the children if this is not a media reference
                if ((dxBinder.IsMedia() == false) && (tmaxBinder.Children != null))
                {
                    //	Do we need to populate the child collection
                    if ((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
                    {
                        dxBinder.Fill();
                        bFilled = true;
                    }

                    //	Add each of the children
                    foreach (CDxBinderEntry O in dxBinder.Contents)
                    {
                        if ((tmaxChild = GetSourceXmlBinder(O)) != null)
                            tmaxBinder.Children.Add(tmaxChild);
                    }

                    if (bFilled == true)
                    {
                        dxBinder.Contents.Clear();
                    }

                }// if(dxBinder.IsMedia() == false)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireDiagnostic(this, "GetSourceXmlBinder", Ex);
                tmaxBinder = null;
            }

            return tmaxBinder;

        }// private CTmaxBinderItem GetSourceXmlBinder(CDxBinderEntry dxBinder)

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

        /// <summary>The user defined set of export options</summary>
        public CTmaxExportOptions ExportOptions
        {
            get { return m_tmaxExportOptions; }
            set { m_tmaxExportOptions = value; }
        }

        /// <summary>TrialMax application Windows Media Encoder wrapper</summary>
        public FTI.Trialmax.Encode.CWMEncoder WMEncoder
        {
            get { return m_wmEncoder; }
            set { m_wmEncoder = value; }
        }

        /// <summary>Enumerated export format identifier</summary>
        public FTI.Shared.Trialmax.TmaxExportFormats Format
        {
            get { return m_eFormat; }
            set { m_eFormat = value; }
        }

        /// <summary>Form displayed during the export operation</summary>
        public FTI.Trialmax.Forms.CFExportStatus StatusForm
        {
            get { return m_wndStatus; }
        }

        /// <summary>Fully qualified path to the active file stream</summary>
        public string FileSpec
        {
            get { return m_strFileSpec; }
            set
            {
                m_strFileSpec = value;
                SetStatusFilename(System.IO.Path.GetFileName(m_strFileSpec));
            }

        }

        /// <summary>The collection of event items that identify the source records</summary>
        public CTmaxItems Source
        {
            get { return m_tmaxSource; }
            set { m_tmaxSource = value; }
        }

        /// <summary>True if aborted by the user</summary>
        public bool Aborted
        {
            get { return m_bAborted; }
        }

        #endregion Properties

    }// class CTmaxExportManager

}// namespace FTI.Trialmax.Database

