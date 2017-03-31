using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Win32;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;
using FTI.Trialmax.ActiveX;
using FTI.Trialmax.MSOffice.MSPowerPoint;
using FTI.Trialmax.Encode;

using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;

using log4net;

using iTextSharp.text.pdf;

using Interop.TiffPageSplitDLL;
using System.Collections.Generic;
using FTI.Shared.Database;
namespace FTI.Trialmax.Database
{
    /// <summary>This class serves as the outer wrapper for all Trialmax database services</summary>
    public class CTmaxCaseDatabase : CBaseDatabase
    {
        #region Constants

        private const string CASE_DATABASE_TEMPLATE_FILENAME = "_tmax_blank.mdb";
        private const string CASE_DATABASE_CASE_FILENAME = "_tmax_case.mdb";
        private const string CASE_DATABASE_CASE_CODES_FILENAME = "_tmax_case_codes.xml";
        private const string CASE_DATABASE_POWERPOINT_EXPORT_EXTENSION = ".png";
        private const string CASE_DATABASE_INVALID_MEDIA_ID_CHARS = "`~@!#^&()+=\\|[]{}\"':;?><,\t ";
        private const string CASE_DATABASE_STATION_OPTIONS_FILENAME = "_tmax_station.xml";
        private const string CASE_DATABASE_CODES_DATABASE_FILENAME = "_tmax_fielded_data.mdb";

        private const string CASE_DATABASE_CLIPS_FOLDER_600 = "_tmax_designations";
        private const string CASE_DATABASE_CLIPS_FOLDER_601 = "_tmax_clips";

        private const string CASE_DATABASE_PICKLISTS_VERSION = "6.2.1";
        private const string CASE_DATABASE_PICKLISTS_WARNING = "This database was created with pick lists for fielded data assignments. They are not supported by your version of TrialMax Manager.";

        #endregion Constants

        #region Error Identifiers



        /// <summary>Error message identifiers</summary>
        public const int ERROR_CASE_DATABASE_ADD_DESIGNATION_EX = ERROR_BASE_DATABASE_LAST + 1;
        public const int ERROR_CASE_DATABASE_OPEN_COLLECTIONS_EX = ERROR_BASE_DATABASE_LAST + 2;
        public const int ERROR_CASE_DATABASE_LINK_SOURCE_NOT_FOUND = ERROR_BASE_DATABASE_LAST + 3;
        public const int ERROR_CASE_DATABASE_ADD_LINK_EX = ERROR_BASE_DATABASE_LAST + 4;
        public const int ERROR_CASE_DATABASE_ADD_ZAP_EX = ERROR_BASE_DATABASE_LAST + 5;

        public const int ERROR_CASE_DATABASE_REFRESH_CASE_CODES_EX = ERROR_BASE_DATABASE_LAST + 6;
        public const int ERROR_CASE_DATABASE_FLUSH_CODES_EX = ERROR_BASE_DATABASE_LAST + 7;
        public const int ERROR_CASE_DATABASE_ADD_PRIMARY_INVALID_TYPE = ERROR_BASE_DATABASE_LAST + 8;
        public const int ERROR_CASE_DATABASE_ADD_SOURCE_EX = ERROR_BASE_DATABASE_LAST + 9;
        public const int ERROR_CASE_DATABASE_ADD_CASE_CODES_EX = ERROR_BASE_DATABASE_LAST + 10;

        public const int ERROR_CASE_DATABASE_SET_CASE_CODES_EX = ERROR_BASE_DATABASE_LAST + 11;
        public const int ERROR_CASE_DATABASE_CREATE_NO_TEMPLATE = ERROR_BASE_DATABASE_LAST + 12;
        public const int ERROR_CASE_DATABASE_CREATE_COPY_FAILED = ERROR_BASE_DATABASE_LAST + 13;
        public const int ERROR_CASE_DATABASE_DUPLICATE_EX = ERROR_BASE_DATABASE_LAST + 14;
        public const int ERROR_CASE_DATABASE_EDIT_DESIGNATIONS_EX = ERROR_BASE_DATABASE_LAST + 15;

        public const int ERROR_CASE_DATABASE_EXPORT_EX = ERROR_BASE_DATABASE_LAST + 16;
        public const int ERROR_CASE_DATABASE_EXPORT_SLIDE_FILE_NOT_FOUND = ERROR_BASE_DATABASE_LAST + 17;
        public const int ERROR_CASE_DATABASE_EXPORT_SLIDE_EX = ERROR_BASE_DATABASE_LAST + 18;
        public const int ERROR_CASE_DATABASE_EXPORT_SLIDES_EX = ERROR_BASE_DATABASE_LAST + 19;
        public const int ERROR_CASE_DATABASE_FIRE_INTERNAL_UPDATE_EX = ERROR_BASE_DATABASE_LAST + 20;

        public const int ERROR_CASE_DATABASE_FIRE_COMMAND_EX = ERROR_BASE_DATABASE_LAST + 21;
        public const int ERROR_CASE_DATABASE_GET_ALIASED_PATH_EX = ERROR_BASE_DATABASE_LAST + 22;
        public const int ERROR_CASE_DATABASE_GET_ALIASED_PATH_FAILED = ERROR_BASE_DATABASE_LAST + 23;
        public const int ERROR_CASE_DATABASE_GET_BINDER_ENTRIES_EX = ERROR_BASE_DATABASE_LAST + 24;
        public const int ERROR_CASE_DATABASE_GET_ZAP_FILES_EX = ERROR_BASE_DATABASE_LAST + 25;

        public const int ERROR_CASE_DATABASE_GET_RECORD_FROM_BARCODE_EX = ERROR_BASE_DATABASE_LAST + 26;
        public const int ERROR_CASE_DATABASE_GET_RECORD_FROM_BARCODE_FAILED = ERROR_BASE_DATABASE_LAST + 27;
        public const int ERROR_CASE_DATABASE_GET_RECORD_FROM_ID_EX = ERROR_BASE_DATABASE_LAST + 28;
        public const int ERROR_CASE_DATABASE_GET_RECORD_FROM_ID_FAILED = ERROR_BASE_DATABASE_LAST + 29;
        public const int ERROR_CASE_DATABASE_GET_RECORDS_FROM_BARCODES_EX = ERROR_BASE_DATABASE_LAST + 30;

        public const int ERROR_CASE_DATABASE_GET_XML_CASE_EX = ERROR_BASE_DATABASE_LAST + 31;
        public const int ERROR_CASE_DATABASE_GET_XML_DEPOSITION_NO_RECORD = ERROR_BASE_DATABASE_LAST + 32;
        public const int ERROR_CASE_DATABASE_GET_XML_DEPOSITION_NO_TRANSCRIPT = ERROR_BASE_DATABASE_LAST + 33;
        public const int ERROR_CASE_DATABASE_GET_XML_DEPOSITION_NO_FILE = ERROR_BASE_DATABASE_LAST + 34;
        public const int ERROR_CASE_DATABASE_GET_XML_DEPOSITION_EX = ERROR_BASE_DATABASE_LAST + 35;

        public const int ERROR_CASE_DATABASE_IMPORT_EX = ERROR_BASE_DATABASE_LAST + 36;
        public const int ERROR_CASE_DATABASE_MOVE_EX = ERROR_BASE_DATABASE_LAST + 37;
        public const int ERROR_CASE_DATABASE_REGISTER_CLOSED = ERROR_BASE_DATABASE_LAST + 38;
        public const int ERROR_CASE_DATABASE_REGISTER_EMPTY = ERROR_BASE_DATABASE_LAST + 39;
        public const int ERROR_CASE_DATABASE_REGISTER_THREAD_EX = ERROR_BASE_DATABASE_LAST + 40;

        public const int ERROR_CASE_DATABASE_CREATE_XML_DESIGNATION_EX = ERROR_BASE_DATABASE_LAST + 41;
        public const int ERROR_CASE_DATABASE_SET_CASE_OPTIONS_EX = ERROR_BASE_DATABASE_LAST + 42;
        public const int ERROR_CASE_DATABASE_BULK_UPDATE_EX = ERROR_BASE_DATABASE_LAST + 43;
        public const int ERROR_CASE_DATABASE_HIDE_CASE_CODES_EX = ERROR_BASE_DATABASE_LAST + 44;
        public const int ERROR_CASE_DATABASE_SET_FILTER_EX = ERROR_BASE_DATABASE_LAST + 45;

        public const int ERROR_CASE_DATABASE_SET_USER_PATH_EX = ERROR_BASE_DATABASE_LAST + 46;
        public const int ERROR_CASE_DATABASE_TRIM_EX = ERROR_BASE_DATABASE_LAST + 47;
        public const int ERROR_CASE_DATABASE_VALIDATE_THREAD_EX = ERROR_BASE_DATABASE_LAST + 48;
        public const int ERROR_CASE_DATABASE_MERGE_SOURCE_EX = ERROR_BASE_DATABASE_LAST + 49;
        public const int ERROR_CASE_DATABASE_MORPH_FOLDER_EX = ERROR_BASE_DATABASE_LAST + 50;

        public const int ERROR_CASE_DATABASE_MORPH_OFFSET_EX = ERROR_BASE_DATABASE_LAST + 51;
        public const int ERROR_CASE_DATABASE_SET_ONE_PER_FILE_EX = ERROR_BASE_DATABASE_LAST + 52;
        public const int ERROR_CASE_DATABASE_SEPARATE_SOURCE_EX = ERROR_BASE_DATABASE_LAST + 53;
        public const int ERROR_CASE_DATABASE_FILENAME_AS_BARCODE_EX = ERROR_BASE_DATABASE_LAST + 54;
        public const int ERROR_CASE_DATABASE_ADD_TRANSCRIPT_EX = ERROR_BASE_DATABASE_LAST + 55;

        public const int ERROR_CASE_DATABASE_ADD_EXTENT_EX = ERROR_BASE_DATABASE_LAST + 56;
        public const int ERROR_CASE_DATABASE_PDF_CREATE_TARGET_FAILED = ERROR_BASE_DATABASE_LAST + 57;
        public const int ERROR_CASE_DATABASE_PDF_START_CONVERT_FAILED = ERROR_BASE_DATABASE_LAST + 58;
        public const int ERROR_CASE_DATABASE_PDF_CONVERSION_FAILED = ERROR_BASE_DATABASE_LAST + 59;
        public const int ERROR_CASE_DATABASE_EXPORT_ADOBE_EX = ERROR_BASE_DATABASE_LAST + 60;

        public const int ERROR_CASE_DATABASE_PDF_PAGE_COUNTS = ERROR_BASE_DATABASE_LAST + 61;
        public const int ERROR_CASE_DATABASE_DELETE_PICK_ITEMS_EX = ERROR_BASE_DATABASE_LAST + 62;
        public const int ERROR_CASE_DATABASE_DELETE_CASE_CODES_EX = ERROR_BASE_DATABASE_LAST + 63;
        public const int ERROR_CASE_DATABASE_MOVE_BINDER_ENTRIES_EX = ERROR_BASE_DATABASE_LAST + 64;
        public const int ERROR_CASE_DATABASE_ADD_TREATMENT_EX = ERROR_BASE_DATABASE_LAST + 65;

        public const int ERROR_CASE_DATABASE_SET_SOURCE_TYPES_EX = ERROR_BASE_DATABASE_LAST + 66;
        public const int ERROR_CASE_DATABASE_GROUP_SOURCE_EX = ERROR_BASE_DATABASE_LAST + 67;
        public const int ERROR_CASE_DATABASE_CREATE_PRIMARY_FOLDER_EX = ERROR_BASE_DATABASE_LAST + 68;
        public const int ERROR_CASE_DATABASE_CREATE_PRIMARY_TYPE_EX = ERROR_BASE_DATABASE_LAST + 69;
        public const int ERROR_CASE_DATABASE_CREATE_SECONDARY_EX = ERROR_BASE_DATABASE_LAST + 70;

        public const int ERROR_CASE_DATABASE_DELETE_SOURCE_EX = ERROR_BASE_DATABASE_LAST + 71;
        public const int ERROR_CASE_DATABASE_TRANSFER_SOURCE_NOT_FOUND = ERROR_BASE_DATABASE_LAST + 72;
        public const int ERROR_CASE_DATABASE_TRANSFER_SOURCE_EX = ERROR_BASE_DATABASE_LAST + 73;
        public const int ERROR_CASE_DATABASE_GET_SOURCE_FOLDER_NOT_FOUND = ERROR_BASE_DATABASE_LAST + 74;
        public const int ERROR_CASE_DATABASE_GET_SOURCE_LINK_FOLDER = ERROR_BASE_DATABASE_LAST + 75;

        public const int ERROR_CASE_DATABASE_GET_SCENES_EX = ERROR_BASE_DATABASE_LAST + 76;
        public const int ERROR_CASE_DATABASE_GET_LINKS_EX = ERROR_BASE_DATABASE_LAST + 77;
        public const int ERROR_CASE_DATABASE_GET_DURATION_EX = ERROR_BASE_DATABASE_LAST + 78;
        public const int ERROR_CASE_DATABASE_SHELL_TRANSFER_EX = ERROR_BASE_DATABASE_LAST + 79;
        public const int ERROR_CASE_DATABASE_SET_RELATIVE_PATH_EX = ERROR_BASE_DATABASE_LAST + 80;

        public const int ERROR_CASE_DATABASE_GET_CASE_FOLDER_EX = ERROR_BASE_DATABASE_LAST + 81;
        public const int ERROR_CASE_DATABASE_CREATE_FOLDER_FAILED = ERROR_BASE_DATABASE_LAST + 82;
        public const int ERROR_CASE_DATABASE_PDF_CONVERTER_NOT_INSTALLED = ERROR_BASE_DATABASE_LAST + 83;
        public const int ERROR_CASE_DATABASE_PDF_CONVERTER_NOT_FOUND = ERROR_BASE_DATABASE_LAST + 84;
        public const int ERROR_CASE_DATABASE_ADD_DUPLICATE_SOURCE_NOT_FOUND = ERROR_BASE_DATABASE_LAST + 85;

        public const int ERROR_CASE_DATABASE_ADD_DUPLICATE_OPEN_FAILED = ERROR_BASE_DATABASE_LAST + 86;
        public const int ERROR_CASE_DATABASE_ADD_DUPLICATE_LINKS_EX = ERROR_BASE_DATABASE_LAST + 87;
        public const int ERROR_CASE_DATABASE_INITIALIZE_CASE_CODES_EX = ERROR_BASE_DATABASE_LAST + 88;
        public const int ERROR_CASE_DATABASE_GET_CODES_FROM_FILE_EX = ERROR_BASE_DATABASE_LAST + 89;
        public const int ERROR_CASE_DATABASE_FILL_CASE_CODES_EX = ERROR_BASE_DATABASE_LAST + 90;

        public const int ERROR_CASE_DATABASE_ENABLE_CASE_CODES_EX = ERROR_BASE_DATABASE_LAST + 91;
        public const int ERROR_CASE_DATABASE_CREATE_TABLE_EX = ERROR_BASE_DATABASE_LAST + 92;
        public const int ERROR_CASE_DATABASE_REORDER_NO_CHILDREN = ERROR_BASE_DATABASE_LAST + 93;
        public const int ERROR_CASE_DATABASE_REORDER_COUNTS = ERROR_BASE_DATABASE_LAST + 94;
        public const int ERROR_MEDIA_RECORD_GET_CODED_PROP_VALUE_EX = ERROR_BASE_DATABASE_LAST + 95;

        public const int ERROR_MEDIA_RECORD_SET_CODED_PROP_VALUE_EX = ERROR_BASE_DATABASE_LAST + 96;
        public const int ERROR_MEDIA_RECORD_EXCHANGE_CODED_PROPS_EX = ERROR_BASE_DATABASE_LAST + 97;
        public const int ERROR_MEDIA_RECORD_ADD_EX = ERROR_BASE_DATABASE_LAST + 98;
        public const int ERROR_MEDIA_RECORD_INVALID_AUTO_ID = ERROR_BASE_DATABASE_LAST + 99;
        public const int ERROR_MEDIA_RECORD_ADD_SQL_EX = ERROR_BASE_DATABASE_LAST + 100;

        public const int ERROR_MEDIA_RECORD_GET_FOREIGN_BARCODE_EX = ERROR_BASE_DATABASE_LAST + 101;
        public const int ERROR_MEDIA_RECORD_INHERITED_FOREIGN_BARCODE = ERROR_BASE_DATABASE_LAST + 102;
        public const int ERROR_MEDIA_RECORD_SET_FOREIGN_BARCODE_EX = ERROR_BASE_DATABASE_LAST + 103;
        public const int ERROR_COMMON_EXCHANGE_FIELDS_EX = ERROR_BASE_DATABASE_LAST + 104;
        public const int ERROR_HIGHLIGHTER_ADD_EX = ERROR_BASE_DATABASE_LAST + 105;

        public const int ERROR_COMMON_DELETE_SQL_EX = ERROR_BASE_DATABASE_LAST + 106;
        public const int ERROR_PRIMARIES_UPDATE_MEDIA_ID = ERROR_BASE_DATABASE_LAST + 107;
        public const int ERROR_PRIMARIES_FILL_FILTERED_EX = ERROR_BASE_DATABASE_LAST + 108;
        public const int ERROR_CASE_DATABASE_IMPORT_OBJECTIONS_EX = ERROR_BASE_DATABASE_LAST + 109;
        public const int ERROR_CASE_DATABASE_UPDATE_OBJECTIONS_EX = ERROR_BASE_DATABASE_LAST + 110;

        public const int ERROR_CASE_DATABASE_DELETE_OBJECTIONS_EX = ERROR_BASE_DATABASE_LAST + 111;
        public const int ERROR_CASE_DATABASE_ADD_OBJECTIONS_EX = ERROR_BASE_DATABASE_LAST + 112;
        public const int ERROR_CASE_DATABASE_EXPORT_OBJECTIONS_EX = ERROR_BASE_DATABASE_LAST + 113;
        public const int ERROR_CASE_DATABASE_FILTER_OBJECTIONS_EX = ERROR_BASE_DATABASE_LAST + 114;
        public const int ERROR_CASE_DATABASE_SET_SHORT_CASE_NAME_EX = ERROR_BASE_DATABASE_LAST + 115;

        public const int ERROR_CASE_DATABASE_SET_CASE_NAME_EX = ERROR_BASE_DATABASE_LAST + 116;
        public const int ERROR_CASE_DATABASE_COMPACT_EX = ERROR_BASE_DATABASE_LAST + 117;
        public const int ERROR_CASE_DATABASE_ON_COMPACTOR_FINISHED_EX = ERROR_BASE_DATABASE_LAST + 118;
        public const int ERROR_CASE_DATABASE_ADD_ZAP_TREATMENT_EX = ERROR_BASE_DATABASE_LAST + 119;

        #endregion Error Identifiers

        #region Private Members

        /// <summary>Local member to manage access to the separate objections database</summary>
        private CObjectionsDatabase m_tmaxObjectionsDatabase = new CObjectionsDatabase();

        /// <summary>Local member associated with the MediaTypes property</summary>
        private FTI.Shared.Trialmax.CTmaxMediaTypes m_tmaxMediaTypes = null;

        /// <summary>Local member associated with the SourceTypes property</summary>
        private FTI.Shared.Trialmax.CTmaxSourceTypes m_tmaxSourceTypes = null;

        /// <summary>Local member associated with the RegistrationOptions property</summary>
        private CTmaxRegOptions m_tmaxRegisterOptions = null;

        /// <summary>Local member to keep track of validation options</summary>
        private CTmaxValidateOptions m_tmaxValidateOptions = new CTmaxValidateOptions();

        /// <summary>Local member associated with the ApplicationOptions property</summary>
        private CTmaxManagerOptions m_tmaxAppOptions = null;

        /// <summary>Local member associated with the CaseOptions property</summary>
        private CTmaxCaseOptions m_tmaxCaseOptions = new CTmaxCaseOptions();

        /// <summary>Local member associated with the CodesManager property</summary>
        private CTmaxCodesManager m_tmaxCodesManager = null;

        /// <summary>Local member to perform compact operations</summary>
        private CTmaxDatabaseCompactor m_tmaxCompactor = null;

        /// <summary>Status form displayed during a compact operation</summary>
        private FTI.Trialmax.Forms.CFCompactorStatus m_wndCompactorStatus = null;

        /// <summary>Local member associated with the StationOptions property</summary>
        private CTmaxStationOptions m_tmaxStationOptions = null;

        /// <summary>Local member to store a reference to the last filter</summary>
        private CTmaxFilter m_tmaxLastFilter = null;

        /// <summary>Array of root media folder specifications</summary>
        private CTmaxSourceFolders m_aCaseFolders = new CTmaxSourceFolders();

        /// <summary>Local member bound to TmxMovie property</summary>
        private FTI.Trialmax.ActiveX.CTmxMovie m_tmxMovie = null;

        /// <summary>Local member bound to TmxView property</summary>
        private FTI.Trialmax.ActiveX.CTmxView m_tmxView = null;

        /// <summary>Source folder containing files to be registered</summary>
        private CTmaxSourceFolder m_RegSourceFolder = null;

        /// <summary>Parent folder of registration source files and folders</summary>
        private CTmaxSourceFolder m_RegSourceParent = null;

        /// <summary>Registration source type associated with the source folder</summary>
        private FTI.Shared.Trialmax.RegSourceTypes m_eRegSourceType = RegSourceTypes.NoSource;

        /// <summary>Flag to indicate registration cancelled by the user</summary>
        private bool m_bRegisterCancelled = false;

        /// <summary>Flag to indicate warning should be displayed if Adobe converter is not found</summary>
        private bool m_bRegisterWarnAdobe = false;

        /// <summary>Flag to indicate validation cancelled by the user</summary>
        private bool m_bValidateCancelled = false;

        /// <summary>Flag to indicate if files should be moved to new (ver 6.0.1) location when validating the database</summary>
        private bool m_bRequires601Update = false;

        /// <summary>Flag to indicate that the user has given permission to perform the move during the validation</summary>
        private bool m_bValidateMoveConfirmed = false;

        /// <summary>Flag to enable/disable loading of media when database gets opened</summary>
        private bool m_bFillOnOpen = true;

        /// <summary>Local member used display validation progress form</summary>
        private CFValidateProgress m_cfValidateProgress = null;

        /// <summary>Packed version of assembly when Codes (fielded data) first appeared</summary>
        private long m_lFirstCodesVersion = 0;

        /// <summary>Flag to indicate the number of errors that occurred when moving files during validation</summary>
        private long m_lMoveOnValidateErrors = 0;

        /// <summary>Flag to indicate all records in current operation should be deleted</summary>
        private bool m_bDeleteAll = false;

        /// <summary>Flag to indicate that references are being deleted</summary>
        private bool m_bDeletingReferences = false;

        /// <summary>Flag to request automatic conflict resolution for single registration session</summary>
        private bool m_bAutoResolve = false;

        /// <summary>Local flag to indicate if Codes (fielded data) support should be added on validate)</summary>
        private bool m_bCodesConversionRequired = false;

        /// <summary>Local flag to indicate that the database should be converted to codes on validation</summary>
        private bool m_bConvertToCodes = false;

        /// <summary>Local flag to indicate that the pick lists table should be added to the database</summary>
        private bool m_bCreatePickLists = false;

        /// <summary>Local flag to indicate that the user is setting the database filter</summary>
        private bool m_bSettingFilter = false;

        /// <summary>Local member bound to the MaxLineTimeIndex property</summary>
        private int m_iMaxLineTimeIndex = -1;

        /// <summary>Local member bound to the SiblingIdIndex property</summary>
        private int m_iSiblingIdIndex = -1;

        /// <summary>Local member bound to the CodesIndexValuePickList property</summary>
        private int m_iCodesIndexValuePickList = -1;

        /// <summary>Local member bound to the CodesIndexModifiedBy property</summary>
        private int m_iCodesIndexModifiedBy = -1;

        /// <summary>Local member bound to the CodesIndexModifiedOn property</summary>
        private int m_iCodesIndexModifiedOn = -1;

        /// <summary>Local member used display registration progress form</summary>
        private CFRegProgress m_cfRegisterProgress = null;

        /// <summary>Local member used resolve registration conflicts</summary>
        private string m_strResolution = "";

        /// <summary>Default name assigned to designations case folder</summary>
        private string m_strDefaultClipsFolder = CASE_DATABASE_CLIPS_FOLDER_601;

        /// <summary>Local member used to manage an import operation</summary>
        private CTmaxImportManager m_tmaxImportManager = new CTmaxImportManager();

        /// <summary>Local member used to manage an import operation</summary>
        private COImportManager m_tmaxImportObjectionsManager = new COImportManager();

        /// <summary>Local member used to manage an export operation</summary>
        private CTmaxExportManager m_tmaxExportManager = new CTmaxExportManager();

        /// <summary>Local member used to manage an export operation</summary>
        private COExportManager m_tmaxExportObjectionsManager = new COExportManager();

        /// <summary>Local member used to manage a trim operation</summary>
        private CTmaxTrimManager m_tmaxTrimManager = new CTmaxTrimManager();

        /// <summary>Private member bound to WMEncoder property</summary>
        private FTI.Trialmax.Encode.CWMEncoder m_wmEncoder = null;

        /// <summary>Local member used to edit designations</summary>
        private CTmaxDesignationEditor m_tmaxDesignationEditor = new CTmaxDesignationEditor();

        /// <summary>Local member bounded to Detail property</summary>
        private CDxDetail m_dxDetail = null;

        /// <summary>Local member bounded to User property</summary>
        private CDxUser m_dxUser = null;

        /// <summary>Local member bounded to AppFolder property</summary>
        private string m_strAppFolder = System.Environment.CurrentDirectory;

        /// <summary>Local member bounded to Folder property</summary>
        private string m_strFolder = System.Environment.CurrentDirectory;

        /// <summary>Local member bounded to Filename property</summary>
        private string m_strFilename = CASE_DATABASE_CASE_FILENAME;

        /// <summary>Local member bounded to Detail property</summary>
        private CDxDetails m_dxDetails = null;

        /// <summary>Local member bounded to Users property</summary>
        private CDxUsers m_dxUsers = null;

        /// <summary>Local member bounded to Highlighters property</summary>
        private CDxHighlighters m_dxHighlighters = null;

        /// <summary>Local member bounded to BarcodeMap property</summary>
        private CDxBarcodes m_dxBarcodeMap = null;

        /// <summary>Local member bounded to Primaries property</summary>
        private CDxPrimaries m_dxPrimaries = null;

        /// <summary>Local member bounded to Filtered property</summary>
        private CDxPrimaries m_dxFiltered = null;

        /// <summary>Local member bounded to Transcripts property</summary>
        private CDxTranscripts m_dxTranscripts = null;

        /// <summary>Local member bounded to Binders property</summary>
        private CDxBinderEntry m_dxRootBinder = null;

        /// <summary>Local member bounded to TargetBinder property</summary>
        private CDxBinderEntry m_dxTargetBinder = null;

        /// <summary>Temporary collection used for records being deleted</summary>
        private CTmaxItems m_tmaxTrashCan = new CTmaxItems();

        /// <summary>Local member bound to TmaxRegistry property</summary>
        private FTI.Shared.Trialmax.CTmaxRegistry m_tmaxRegistry = null;

        /// <summary>Local member bound to TmaxProduct property</summary>
        private FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = null;

        /// <summary>Local member bound to PaneId property</summary>
        private int m_iPaneId = 0;

        /// <summary>Total number of pages that will be converted</summary>
        private long m_totalPages = 0;

        /// <summary>Leadtools codec to count total number of pages</summary>
        private GhostscriptVersionInfo m_gvi = null;

        /// <summary>Leadtools CodecsImageInfo to get PDF info</summary>
        private GhostscriptRasterizer m_rasterizer = null;

        /// <summary>Thread that does the entire Registration for PDF Imports</summary>
        private static Thread RegThread = null;

        /// <summary>List that will contain un-completed tasks</summary>
        private List<CTmaxPDFManager> ConversionTasksArray = null;

        /// <summary>Lock for accessing ConverstionTasksArray above</summary>
        //private static object lockConversionTasksArray = true;

        /// <summary>Lock for Conflict Resolution form because only 1 form should appear at any given instance</summary>
        private static object lockForConflictForm = true;

        /// <summary>Local variable to log detail errors with stacktrace</summary>
        private static readonly log4net.ILog logDetailed = log4net.LogManager.GetLogger("DetailedLog");

        /// <summary>Local variable to log user level details</summary>
        private static readonly log4net.ILog logUser = log4net.LogManager.GetLogger("UserLog");

        #endregion Private Members

        #region Public Methods

        /// <summary>This is the delegate used to handle all database events</summary>
        /// <param name="objSender">Object firing the event</param>
        /// <param name="Args">Object containing the command event arguments</param>
        public delegate void DatabaseEventHandler(object objSender, CTmaxItems tmaxItems);

        /// <summary>This event is fired by the database when it performs an internal update of a record</summary>
        /// <remarks>An internal update is one in which the database modifies an existing record without being told to by the application</remarks>
        public event DatabaseEventHandler InternalUpdateEvent;

        /// <summary>This event is fired to propagate command events back to the application</summary>
        public event FTI.Shared.Trialmax.TmaxCommandHandler TmaxCommandEvent;

        /// <summary>Constructor</summary>
        public CTmaxCaseDatabase()
        {
            m_lFirstCodesVersion = GetPackedVer(6, 1, 6);
            log4net.Config.XmlConfigurator.Configure();
            m_tmaxImportManager.Database = this;
            m_tmaxExportManager.Database = this;
            m_tmaxDesignationEditor.Database = this;
            m_tmaxObjectionsDatabase.CaseDatabase = this;
            m_tmaxImportObjectionsManager.CaseDatabase = this;
            m_tmaxExportObjectionsManager.CaseDatabase = this;

            SetHandlers(m_tmaxCaseOptions.EventSource);
            SetHandlers(m_tmaxImportManager.EventSource);
            SetHandlers(m_tmaxExportManager.EventSource);
            SetHandlers(m_tmaxDesignationEditor.EventSource);
            SetHandlers(m_tmaxObjectionsDatabase.EventSource);
            SetHandlers(m_tmaxImportObjectionsManager.EventSource);
            SetHandlers(m_tmaxExportObjectionsManager.EventSource);

        }// public CTmaxCaseDatabase()

        /// <summary>This method is called to populate the database with the specified number of primary records</summary>
        /// <param name="iPrimaries">The number of primaries to be added</param>
        ///	<remarks>This method is provided as a tool for development and testing</remarks>
        public void Add(int iPrimaries, TmaxMediaTypes eType)
        {
            CDxPrimary dxPrimary = null;

            for (int i = 0; i < iPrimaries; i++)
            {
                //	Create a new primary exchange object of the specified type
                if ((dxPrimary = CreatePrimary(eType)) == null) break;

                //	Add it to the database
                if (dxPrimary != null)
                    if (m_dxPrimaries.Add(dxPrimary) == null) break;
            }

        }// public void Add(int iPrimaries, TmaxMediaTypes eType)

        /// <summary>This method will add or insert a record using the specified event item</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxResults">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        public bool Add(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {
            CTmaxItems tmaxAdded = null;
            CTmaxItem tmaxParent = null;
            bool bSuccessful = false;

            //	First check to see if the caller is adding objections
            if (IsObjectionsCommand(tmaxParameters) == true)
                return AddObjections(tmaxItem, tmaxParameters, tmaxResults);

            Debug.Assert(tmaxItem != null);

            //	Create a collection to hold the results
            tmaxAdded = new CTmaxItems();

            //	Are we adding items to a binder?
            if (tmaxItem.DataType == TmaxDataTypes.Binder)
            {
                AddBinderEntries(tmaxItem, tmaxAdded, tmaxParameters);
            }

            //	Are we adding new media records
            else if (tmaxItem.DataType == TmaxDataTypes.Media)
            {
                AddMedia(tmaxItem, tmaxAdded, tmaxParameters);
            }

            //	Are we adding pick list items?
            else if (tmaxItem.DataType == TmaxDataTypes.PickItem)
            {
                AddPickItems(tmaxItem, tmaxAdded, tmaxParameters);
            }

            //	Are we adding case codes?
            else if (tmaxItem.DataType == TmaxDataTypes.CaseCode)
            {
                AddCaseCodes(tmaxItem, tmaxAdded, tmaxParameters);
            }

            else
            {
                // OOPS
                Debug.Assert(false, "Unknown data type for Add: Type = " + tmaxItem.DataType.ToString());
            }

            //	Give the object that initiated the request access to the
            //	return results
            if ((tmaxAdded.Count > 0) && (tmaxItem.ReturnItem == null))
            {
                tmaxItem.ReturnItem = new CTmaxItem();
                tmaxItem.ReturnItem.SourceItems = tmaxAdded;
            }

            bSuccessful = (tmaxAdded.Count > 0);

            //	Does the user want the results?
            if ((tmaxResults != null) && (bSuccessful == true))
            {
                tmaxParent = new CTmaxItem();
                tmaxParent.Copy(tmaxItem, false, false);

                foreach (CTmaxItem O in tmaxAdded)
                {
                    //	Make sure the parent item has been set
                    if (O.ParentItem == null)
                        O.ParentItem = tmaxParent;

                    tmaxParent.SubItems.Add(O);
                }

                tmaxResults.Added.Add(tmaxParent);
            }

            // NOTE: Do not clear the added collection. It is returned as
            //		 a result
            //tmaxAdded.Clear();
            tmaxAdded = null;

            return bSuccessful;

        }// public bool Add(CTmaxItem tmaxItem, CTmaxItems tmaxAdded, CTmaxParameters tmaxParameters)

        /// <summary>This method will add an entry to the binders table using the specified event item</summary>
        /// <param name="dxParent">The parent binder</param>
        /// <param name="tmaxItem">Event item to identify the new entry</param>
        /// <param name="dxInsertAt">Insertion point for the new entry</param>
        /// <param name="bBefore">true to insert before the specified insertion point</param>
        /// <returns>true if successful</returns>
        public CDxBinderEntry AddBinderEntry(CDxBinderEntry dxParent, CTmaxItem tmaxItem, CDxBinderEntry dxInsertAt, bool bBefore)
        {
            CDxBinderEntry dxEntry = null;
            CTmaxItems tmaxAdded = null;

            Debug.Assert(tmaxItem != null);

            //	Allocate a temporary collection for the results
            tmaxAdded = new CTmaxItems();

            dxEntry = AddBinderEntry(dxParent, tmaxItem, dxInsertAt, bBefore, tmaxAdded);

            //	Destroy the temporary collection
            tmaxAdded.Clear();
            tmaxAdded = null;

            return dxEntry;

        }// public CDxBinderEntry AddBinderEntry(CDxBinderEntry dxParent, CTmaxItem tmaxItem, CDxBinderEntry dxInsertAt, bool bBefore)

        /// <summary>This method will add a designation to the database</summary>
        /// <param name="dxSecondary">The secondary media object that owns the new designation</param>
        /// <param name="xmlDesignation">The XML designation descriptor</param>
        /// <returns>The record exchange object for the new designation</returns>
        public CDxTertiary AddDesignation(CDxSecondary dxSecondary, CXmlDesignation xmlDesignation)
        {
            CDxTertiary dxTertiary = null;
            string strFileSpec = "";

            Debug.Assert(dxSecondary != null);

            //	Make sure the secondary child collection is filled
            if ((dxSecondary.ChildCount > 0) && (dxSecondary.Tertiaries.Count == 0))
            {
                if (dxSecondary.Fill() == false)
                    return null;
            }

            //	Initialize a new tertiary exchange object
            dxTertiary = new CDxTertiary(dxSecondary);
            dxTertiary.HasShortcuts = false;
            if (dxSecondary.Primary.MediaType == TmaxMediaTypes.Recording)
            {
                xmlDesignation.HasText = false;
                xmlDesignation.ScrollText = false;
                dxTertiary.MediaType = TmaxMediaTypes.Clip;

                //	Make sure the root folder for clips exists
                if (CreateCaseFolder(TmaxMediaTypes.Clip, true) == false)
                    return null;

            }
            else if (dxSecondary.Primary.MediaType == TmaxMediaTypes.Deposition)
            {
                xmlDesignation.HasText = true;

                //	caller may have set the scroll text option

                dxTertiary.MediaType = TmaxMediaTypes.Designation;

                //	Make sure the root folder for designations exists
                if (CreateCaseFolder(TmaxMediaTypes.Designation, true) == false)
                    return null;

            }
            else
            {
                Debug.Assert(false);
                return null;
            }

            dxTertiary.Filename = ""; // Don't know until after we add
            if (dxSecondary.Tertiaries.Count > 0)
                dxTertiary.DisplayOrder = dxSecondary.Tertiaries[dxSecondary.Tertiaries.Count - 1].DisplayOrder + 1;
            else
                dxTertiary.DisplayOrder = dxSecondary.ChildCount + 1; // Just in case

            dxTertiary.BarcodeId = dxSecondary.Tertiaries.GetNextBarcodeId();
            dxTertiary.Name = xmlDesignation.Name;

            //	Add the new record to the secondary collection
            if (dxSecondary.Add(dxTertiary) == false) return null;

            try
            {
                //	Get the filename we want to use for this designation
                strFileSpec = GetFileSpec(dxTertiary);

                //	Make sure the parent folder exists
                if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(strFileSpec)) == false)
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(strFileSpec));

                //	Update the child counter on the secondary record
                dxSecondary.ChildCount = dxSecondary.Tertiaries.Count;
                dxSecondary.Collection.Update(dxSecondary);

                //	Finish initializing the xml designation
                xmlDesignation.CreatedBy = GetUserName(dxTertiary.CreatedBy);
                xmlDesignation.CreatedOn = dxTertiary.CreatedOn.ToString();
                xmlDesignation.ModifiedBy = xmlDesignation.CreatedBy;
                xmlDesignation.ModifiedOn = xmlDesignation.CreatedOn;
                xmlDesignation.PrimaryId = dxSecondary.Primary.MediaId;

                if (dxTertiary.MediaType == TmaxMediaTypes.Designation)
                    xmlDesignation.Segment = dxSecondary.GetExtent().XmlSegmentId.ToString();
                else
                    xmlDesignation.Segment = dxSecondary.BarcodeId.ToString();

                //	Do we need to create a case node?
                if (xmlDesignation.Case == null)
                    xmlDesignation.Case = GetXmlCase();

                //	NOTE:	The designation events should be hooked by the creator

                //	Save the designation
                if (xmlDesignation.Save(strFileSpec) == false)
                {
                    dxSecondary.Delete(dxTertiary);
                    return null;
                }

                //	Add the extents for this new designation
                AddExtent(dxTertiary, xmlDesignation);

            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddDesignation", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_DESIGNATION_EX), Ex);
                dxSecondary.Delete(dxTertiary);
                return null;
            }

            //	Now update the FileName field in the tertiary record
            dxTertiary.Filename = System.IO.Path.GetFileName(strFileSpec);
            dxSecondary.Tertiaries.Update(dxTertiary);

            return dxTertiary;

        }// public CDxTertiary AddDesignation(CDxSecondary dxSecondary, CXmlDesignation xmlDesignation)

        /// <summary>This method will add a link to the database</summary>
        /// <param name="dxParent">The tertiary parent that owns the new link</param>
        /// <param name="xmlDesignation">The XML parent designation descriptor</param>
        /// <param name="xmlLink">The XML link descriptor</param>
        /// <returns>The record exchange object for the new link</returns>
        public CDxQuaternary AddLink(CDxTertiary dxParent, CXmlDesignation xmlDesignation, CXmlLink xmlLink)
        {
            CDxQuaternary dxQuaternary = null;
            CDxMediaRecord dxSource = null;
            int iIndex = -1;

            Debug.Assert(dxParent != null);
            Debug.Assert(xmlLink != null);

            //	Make sure we can get the source record if this is not
            //	a hidden link
            if (xmlLink.Hide == false)
            {
                //	First try the PSTQ if specified
                if (xmlLink.SourceDbId.Length > 0)
                    dxSource = GetRecordFromId(xmlLink.SourceDbId, false);

                //	Now try the media id if the PSTQ didn't work
                if ((dxSource == null) && (xmlLink.SourceMediaId.Length > 0))
                {
                    dxSource = GetRecordFromBarcode(xmlLink.SourceMediaId, false, false);
                    xmlLink.SourceDbId = dxSource.GetUniqueId();
                }

                if (dxSource == null)
                {
                    FireError(this, "AddLink", this.ExBuilder.Message(ERROR_CASE_DATABASE_LINK_SOURCE_NOT_FOUND, xmlLink.SourceDbId, xmlLink.SourceMediaId));
                    return null;
                }

            }// if(xmlLink.Hide == false)

            //	Make sure the parent's child collection is filled
            //
            //	NOTE:	We don't test for ChildCount > 0 because we want to
            //			make sure a record sorter object gets attached
            if (dxParent.Quaternaries.Count == 0)
            {
                if (dxParent.Fill() == false)
                    return null;
            }

            //	Initialize a new tertiary exchange object
            dxQuaternary = new CDxQuaternary(dxParent);
            dxQuaternary.MediaType = TmaxMediaTypes.Link;
            dxQuaternary.Tertiary = dxParent;
            dxQuaternary.TertiaryMediaId = dxParent.AutoId;
            dxQuaternary.Source = dxSource;
            dxQuaternary.SourceType = (dxSource != null) ? dxSource.MediaType : TmaxMediaTypes.Unknown;

            //	Set the XML related properties
            dxQuaternary.SetProperties(xmlLink);

            if (dxParent.Quaternaries.Count > 0)
                dxQuaternary.DisplayOrder = dxParent.Quaternaries[dxParent.Quaternaries.Count - 1].DisplayOrder + 1;
            else
                dxQuaternary.DisplayOrder = dxParent.ChildCount + 1; // Just in case

            dxQuaternary.BarcodeId = dxParent.Quaternaries.GetNextBarcodeId();

            //	Add the new record to the tertiary collection
            if (dxParent.Add(dxQuaternary) == false) return null;

            try
            {
                //	We need to sort these records based on start position
                if (dxParent.Quaternaries.Comparer != null)
                {
                    dxParent.Quaternaries.Sort();

                    if ((iIndex = dxParent.Quaternaries.IndexOf(dxQuaternary)) >= 0)
                    {
                        for (int i = iIndex; i < dxParent.Quaternaries.Count; i++)
                        {
                            dxParent.Quaternaries[i].DisplayOrder = i + 1;
                            dxParent.Update(dxParent.Quaternaries[i]);
                        }
                    }

                }

                //	Update the child counter on the tertiary parent record
                dxParent.ChildCount = dxParent.Quaternaries.Count;
                dxParent.Collection.Update(dxParent);

                //	Finish initializing the xml link
                xmlLink.DatabaseId = GetUniqueId(dxQuaternary);

                //	Do we need to create the XML designation?
                if (xmlDesignation == null)
                    xmlDesignation = GetXmlDesignation(dxParent, true, true, true);

                if (xmlDesignation != null)
                {
                    //	Add it to the xml designation list if not already added by the caller
                    if (xmlDesignation.Links.Contains(xmlLink) == false)
                    {
                        xmlDesignation.Links.Add(xmlLink);
                        xmlDesignation.Links.Sort();
                    }

                    //	Update the xml designation
                    xmlDesignation.ModifiedBy = GetUserName(dxParent.ModifiedBy);
                    xmlDesignation.ModifiedOn = dxParent.ModifiedOn.ToString();

                    //	Save the designation
                    xmlDesignation.Save();

                }// if(xmlDesignation != null)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddLink", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_LINK_EX), Ex);
                dxParent.Delete(dxQuaternary);
                xmlDesignation.Links.Remove(xmlLink);
                return null;
            }

            return dxQuaternary;

        }// public CDxQuaternary AddLink(CDxTertiary dxParent, CXmlDesignation xmlDesignation, CXmlLink xmlLink)

        /// <summary>Called to add a treatment using a zap file created in TmaxPresentation</summary>
        /// <param name="strSource">The fully qualified path to the zap file created by TmaxPresentation</param>
        ///	<param name="tmaxResults">Collection in which to place items representing new records</param>
        /// <returns>the exchange interface for the new treatment record</returns>
        public CDxTertiary AddZap(string strSource, CTmaxDatabaseResults tmaxResults)
        {
            CTmaxZapFile tmaxZap = null;
            CDxTertiary dxTertiary = null;
            CDxTertiary dxSibling = null;
            string strMsg = "";

            try
            {
                tmaxZap = new CTmaxZapFile();

                if (tmaxZap.Initialize(strSource) == true)
                {
                    dxTertiary = AddTreatment(tmaxZap.SourcePageId, tmaxZap.SourceFileSpec, tmaxResults);

                    //	Is this a split screen treatment?
                    if ((dxTertiary != null) && (tmaxZap.SplitScreen == true))
                    {
                        dxSibling = AddTreatment(tmaxZap.SiblingPageId, tmaxZap.SiblingFileSpec, tmaxResults);

                        //	Update the split screen attributes
                        if (dxSibling != null)
                        {
                            dxTertiary.SiblingId = GetUniqueId(dxSibling);
                            dxTertiary.SplitScreen = true;
                            dxTertiary.SplitRight = false;
                            dxTertiary.SplitHorizontal = tmaxZap.Horizontal;
                            dxTertiary.Secondary.Update(dxTertiary);

                            dxSibling.SiblingId = GetUniqueId(dxTertiary);
                            dxSibling.SplitScreen = true;
                            dxSibling.SplitRight = true;
                            dxSibling.SplitHorizontal = tmaxZap.Horizontal;
                            dxSibling.Secondary.Update(dxSibling);
                        }

                    }// if((dxTertiary != null) && (tmaxZap.SplitScreen == true))

                }
                else
                {
                    strMsg = String.Format("The attempt to initialize the treatment failed: {0}\n",
                                            System.IO.Path.GetFileName(strSource));

                    if (tmaxZap.EventSource.LastErrorArgs != null)
                    {
                        strMsg += "\n";
                        strMsg += tmaxZap.EventSource.LastErrorArgs.Message;
                        strMsg += "\n";
                    }

                    OnAddPresentationError(strMsg, strSource);
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddZap", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_ZAP_EX, strSource), Ex);
            }

            return dxTertiary;

        }// public CDxTertiary AddZap(string strSource, CTmaxDatabaseResults tmaxResults)

        /// <summary>This method is called to add all treatments created and saved in Presentation</summary>
        /// <param name="tmaxResults">Collection in which to store items representing new treatments</param>
        public void AddZaps(CTmaxDatabaseResults tmaxResults)
        {
            ArrayList aZapFiles = GetZapFiles();

            if ((aZapFiles != null) && (aZapFiles.Count > 0))
            {
                foreach (string O in aZapFiles)
                {
                    AddZap(O, tmaxResults);
                }

                aZapFiles.Clear();
            }

        }// public void AddZaps(CTmaxDatabaseResults tmaxResults)

        /// <summary>Called to refresh the application's collection of case codes</summary>
        /// <returns>true if successful</returns>
        public bool RefreshCaseCodes()
        {
            bool bSuccessful = true;

            //	Don't bother if we don't have a case codes collection
            if (this.CaseCodes == null) return false;

            try
            {
                //	Make sure we have the latest Hidden attributes
                if (m_tmaxStationOptions != null)
                    m_tmaxStationOptions.SaveCaseCodes(this.CaseCodes);

                //	Is the information stored in the database?
                if (this.DxCaseCodes != null)
                {
                    //	Clear the collections
                    this.CaseCodes.Clear();
                    this.DxCaseCodes.Clear();

                    //	Refill
                    this.DxCaseCodes.Fill();
                    foreach (CDxCaseCode O in this.DxCaseCodes)
                    {
                        O.TmaxCaseCode.DxRecord = O;
                        this.CaseCodes.Add(O.TmaxCaseCode);
                    }

                    //	Are pick lists stored in the database?
                    if (this.DxPickLists != null)
                    {
                        this.DxPickLists.Fill(true);
                    }

                }
                else
                {
                    m_tmaxCodesManager.Refresh();
                }

                //	Make sure the codes are sorted
                if (this.CaseCodes != null)
                    this.CaseCodes.Sort(true);

                //	Update the Hidden property of each code
                if ((this.CaseCodes != null) && (m_tmaxStationOptions != null))
                    m_tmaxStationOptions.LoadCaseCodes(this.CaseCodes);

                //	Force the primary records to refresh the fielded data
                this.Primaries.ResetCodes();

            }
            catch (System.Exception Ex)
            {
                FireError(this, "RefreshCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_REFRESH_CASE_CODES_EX), Ex);
                bSuccessful = false;
            }

            return bSuccessful;

        }// public bool RefreshCaseCodes()

        /// <summary>Called to clear the contents of the Codes table</summary>
        /// <returns>true if successful</returns>
        public bool FlushCodes()
        {
            bool bSuccessful = true;
            CDxCodes dxCodes = null;

            //	Don't bother if we don't support codes (fielded data)
            if (this.CodesEnabled == false) return false;

            try
            {
                //	Allocate a collection to do the work
                dxCodes = new CDxCodes(this);
                dxCodes.Flush();

                //	Force the primary records to refresh the fielded data
                this.Primaries.ResetCodes();

                bSuccessful = true;

            }
            catch (System.Exception Ex)
            {
                FireError(this, "FlushCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_FLUSH_CODES_EX), Ex);
            }

            return bSuccessful;

        }// public bool FlushCodes()

        /// <summary>Called to get the color used to highlight objections owned by the specified party</summary>
        /// <param name="eParty">The enumerated party identifier</param>
        /// <returns>The color used to highlight the objections</returns>
        public System.Drawing.Color GetObjectionsColor(TmaxHighlighterGroups eParty)
        {
            CDxHighlighter dxHighlighter = null;
            System.Drawing.Color objColor = System.Drawing.Color.Red;

            //	Get the first highlighter associated with the specified party
            if ((m_dxHighlighters != null) && (m_dxHighlighters.Count > 0))
            {
                foreach (CDxHighlighter O in m_dxHighlighters)
                {
                    if (O.Group == eParty)
                    {
                        dxHighlighter = O;
                        objColor = O.GetSysColor();
                        break;
                    }

                }// foreach(CDxHighlighter O in m_dxHighlighters)

            }// if((m_dxHighlighters != null) && (m_dxHighlighters.Count > 0))

            //	Do we need to assign default color?
            if (dxHighlighter == null)
            {
                switch (eParty)
                {
                    case TmaxHighlighterGroups.Defendant:
                        objColor = System.Drawing.Color.Blue;
                        break;

                    case TmaxHighlighterGroups.Plaintiff:
                        objColor = System.Drawing.Color.Red;
                        break;

                    case TmaxHighlighterGroups.Other:
                    default:
                        objColor = System.Drawing.Color.Magenta;
                        break;

                }// switch(eParty)

            }// if(dxHighlighter == null)

            return objColor;

        }// public System.Drawing.Color GetObjectionsColor(TmaxHighlighterGroups eParty)

        /// <summary>Called to get the pick list assigned to the specified case code object</summary>
        /// <param name="dxCaseCode">The exchange interface for the case code that references the desired pick list value</param>
        /// <returns>true if successful</returns>
        public bool GetPickList(CDxCaseCode dxCaseCode)
        {
            //	Use the application object bound to this record
            if (dxCaseCode.TmaxCaseCode != null)
                return GetPickList(dxCaseCode.TmaxCaseCode);
            else
                return false;

        }// public bool GetPickList(CDxCaseCode dxCaseCode)

        /// <summary>Called to get the pick list assigned to the specified case code object</summary>
        /// <param name="tmaxCaseCode">The application case code that references the desired pick list value</param>
        /// <returns>true if successful</returns>
        public bool GetPickList(CTmaxCaseCode tmaxCaseCode)
        {
            //	Don't bother if we aren't using pick lists
            if (this.PickListsEnabled == false) return false;
            if (this.PickLists == null) return false;

            //	Don't bother if this is not a pick list type code
            if (tmaxCaseCode.Type != TmaxCodeTypes.PickList) return false;
            if (tmaxCaseCode.PickListId <= 0) return false;

            //	Search the root for the specified pick list
            tmaxCaseCode.PickList = this.PickLists.FindList(tmaxCaseCode.PickListId);

            return (tmaxCaseCode.PickList != null);

        }// public bool GetPickList(CTmaxCaseCode tmaxCaseCode)

        /// <summary>This method will add a new primary record of the specified type to the database</summary>
        /// <param name="tmaxType">The primary media type</param>
        /// <param name="strMediaId">The default media id for the new record</param>
        /// <param name="strName">The default name for the new record</param>
        /// <param name="strDescription">The default description for the new record</param>
        /// <returns>The record exchange object for the new primary record</returns>
        public CDxPrimary AddPrimary(TmaxMediaTypes tmaxType, string strMediaId, string strName, string strDescription)
        {
            CDxPrimary dxPrimary = null;
            FTI.Trialmax.Forms.CFAddPrimary cfAddPrimary = null;
            string strMsg = "";
            bool bSetMediaId = false;

            Debug.Assert(tmaxType != TmaxMediaTypes.Unknown);
            if (tmaxType == TmaxMediaTypes.Unknown)
            {
                FireError(this, "AddPrimary", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_PRIMARY_INVALID_TYPE));
                return null;
            }

            //	Create a new primary exchange object of the specified type
            if ((dxPrimary = CreatePrimary(tmaxType)) == null) return null;

            //	Add it to the database
            if (m_dxPrimaries.Add(dxPrimary) == null) return null;

            //	Set the default record values supplied by the caller
            if ((strName != null) && (strName.Length > 0))
                dxPrimary.Name = strName;
            if ((strDescription != null) && (strDescription.Length > 0))
                dxPrimary.Description = strDescription;

            if ((strMediaId != null) && (strMediaId.Length > 0))
            {
                dxPrimary.MediaId = strMediaId;

                //	Attempt to set the media id without prompting
                bSetMediaId = true;
            }
            else
            {
                dxPrimary.MediaId = dxPrimary.AutoId.ToString();
            }

            //	Prompt the user for the values
            while (dxPrimary != null)
            {
                //	Should we try to set the media id?
                if (bSetMediaId == true)
                {
                    if (m_dxPrimaries.UpdateMediaId(dxPrimary) == 0)
                    {
                        //	Update the non-unique values
                        m_dxPrimaries.Update(dxPrimary);
                        break;
                    }
                    else
                    {
                        //	Try again
                        strMsg = String.Format("{0} is already assigned. You must supply a unique Media Id", dxPrimary.MediaId);
                        MessageBox.Show(strMsg, "Duplicate Id", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        if (cfAddPrimary != null)
                        {
                            try
                            {
                                cfAddPrimary.Dispose();
                                cfAddPrimary = null;
                            }
                            catch
                            {
                            }

                        }// if(cfAddPrimary != null)

                    }// if(m_dxPrimaries.UpdateMediaId(dxPrimary) == 0)

                }// if(bSetMediaId == true)

                //	Create the form that allows the user to specify the primary record values
                cfAddPrimary = new FTI.Trialmax.Forms.CFAddPrimary();
                cfAddPrimary.MediaType = dxPrimary.MediaType;
                cfAddPrimary.AutoId = dxPrimary.AutoId;
                cfAddPrimary.MediaId = dxPrimary.MediaId;
                cfAddPrimary.Exhibit = dxPrimary.Exhibit;
                cfAddPrimary.MediaName = dxPrimary.Name;
                cfAddPrimary.Description = dxPrimary.Description;

                //	Prompt the user for the values
                DisableTmaxKeyboard(true);
                if (cfAddPrimary.ShowDialog() == DialogResult.OK)
                {
                    DisableTmaxKeyboard(false);

                    //	Get the user supplied values
                    dxPrimary.MediaId = cfAddPrimary.MediaId;
                    dxPrimary.Name = cfAddPrimary.MediaName;

                    //	Update the coded properties
                    if (dxPrimary.Exhibit != cfAddPrimary.Exhibit)
                        dxPrimary.Exhibit = cfAddPrimary.Exhibit;
                    if (dxPrimary.Description != cfAddPrimary.Description)
                        dxPrimary.Description = cfAddPrimary.Description;

                    //	Use these values to set the media id
                    bSetMediaId = true;
                }
                else
                {
                    DisableTmaxKeyboard(false);

                    //	User canceled so remove the record
                    dxPrimary.DeleteCodes();
                    m_dxPrimaries.Delete(dxPrimary);
                    dxPrimary = null;	//	This will break out of the loop
                }

            }// while(dxPrimary != null)

            if (cfAddPrimary != null)
                cfAddPrimary.Dispose();

            return dxPrimary;

        }// public CDxPrimary AddPrimary(TmaxMediaTypes tmaxType, string strMediaId, string strName, string strDescription)

        /// <summary>This method will drill down the specified source folder and add all files to the database</summary>
        /// <param name="tmaxSource">The folder containing all source files to be merged</param>
        /// <returns>true if successful</returns>
        public bool AddSource(CTmaxSourceFolder tmaxSource)
        {
            CDxPrimary dxPrimary = null;

            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.SubFolders != null);

            //	Make sure we can import new primary records
            if (CheckMaxPrimaries(false) == true)
                return false;

            //	Are there any files in the source folder?
            if (tmaxSource.Files.Count > 0)
            {
                if (m_bRegisterCancelled) return true;

                //	Get the primary exchange object for this folder
                if ((dxPrimary = CreatePrimary(tmaxSource)) != null)
                {
                    //	Set the primary interface
                    tmaxSource.IPrimary = dxPrimary;

                    //	Set the flag to indicate that this folder has been registered
                    tmaxSource.Registered = true;

                    //	Is this primary being created from an XML node?
                    if (tmaxSource.XmlPrimary != null)
                    {
                        //	Should we add a foreign barcode?
                        //
                        //	NOTE:	We do this BEFORE adding the child secondaries so that
                        //			if one of the children is assigned the same FBC it will
                        //			overwrite the primary FBC
                        if (tmaxSource.XmlPrimary.ForeignBarcode.Length > 0)
                            dxPrimary.SetForeignBarcode(tmaxSource.XmlPrimary.ForeignBarcode, true, false);
                    }

                    //	Do we need to add transcript information?
                    if (dxPrimary.MediaType == TmaxMediaTypes.Deposition)
                    {
                        Debug.Assert(tmaxSource.UserData != null);
                        if (tmaxSource.UserData != null)
                            AddTranscript(tmaxSource, dxPrimary);
                    }

                    //	Is this a PowerPoint presentation?
                    if (tmaxSource.SourceType == RegSourceTypes.Powerpoint)
                    {
                        if (ExportSlides(dxPrimary, tmaxSource) == false || m_bRegisterCancelled == true)
                        {
                            if ((dxPrimary != null))
                            {
                                try
                                {
                                    File.Delete(GetFileSpec(dxPrimary));
                                    DeleteDirectory(GetSlidesFolderSpec(dxPrimary));
                                    m_dxPrimaries.Delete(dxPrimary);
                                    FireCommand(TmaxCommands.RefreshCodes);
                                }
                                catch { m_dxPrimaries.Delete(dxPrimary); FireCommand(TmaxCommands.RefreshCodes); };
                                dxPrimary = null;
                            }
                        }
                    }
                    else
                    {
                        //	Add each of the secondary records
                        AddSource(tmaxSource, dxPrimary, 1);
                    }

                }// if((dxPrimary = CreatePrimary(tmaxSource)) != null)

            }// if(tmaxSource.Files.Count > 0)

            //	Add each subfolder
            foreach (CTmaxSourceFolder tmaxSubFolder in tmaxSource.SubFolders)
            {
                try
                {
                    if (m_bRegisterCancelled == false)
                    {
                        AddSourceAgs variables = new AddSourceAgs();
                        variables.tmaxSource = tmaxSource;
                        variables.tmaxSubFolder = tmaxSubFolder;
                        AddSourceProcess(variables);
                    }
                }
                catch (System.Exception Ex)
                {
                    FireError(this, "AddSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_SOURCE_EX), Ex);
                }

            }//foreach(CTmaxSourceFolder tmaxSubFolder in tmaxSource.SubFolders)


            //Task.WaitAll(PdfTasks.ToArray());

            return m_bRegisterCancelled;

        }// public bool AddSource(CTmaxSourceFolder tmaxSource)

        class AddSourceAgs
        {
            public CTmaxSourceFolder tmaxSource { get; set; }
            public CTmaxSourceFolder tmaxSubFolder { get; set; }
        }

        public void AddSourceProcess(Object arguments)
        {
            try
            {
                AddSourceAgs args = (AddSourceAgs)arguments;
                if (m_bRegisterCancelled == false)
                {
                    AddSource(args.tmaxSubFolder);

                    //	Mark this parent as being registered if its subfolder got registered
                    //
                    //	NOTE:	This allows us to maintain the registration chain when a folder
                    //			has nothing but subfolders
                    if (args.tmaxSubFolder.Registered == true)
                        args.tmaxSource.Registered = true;
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_SOURCE_EX), Ex);
            }
        }
        /// <summary>This method will add the specified case codes to the database</summary>
        /// <param name="tmaxCaseCodes">The collection of codes to be added</param>
        /// <param name="tmaxInsertAt">The object that defines where the new objects are to be inserted</param>
        /// <param name="bInsertBefore">true to insert before the specified insertion point</param>
        /// <param name="bAutoId">True if the database should be allowed to assign the UniqueId of each code</param>
        /// <param name="tmaxAdded">Collection to populate with event items to represent the new records</param>
        /// <returns>true if successful</returns>
        public bool AddCaseCodes(CTmaxCaseCodes tmaxCaseCodes, CTmaxCaseCode tmaxInsertAt, bool bInsertBefore, bool bAutoId, CTmaxItems tmaxAdded)
        {
            CDxCaseCode dxCaseCode = null;
            CTmaxCaseCodes tmaxReorder = null;
            bool bOldAutoId = false;
            int iIndex = -1;
            long lSortOrder = 0;

            Debug.Assert(tmaxCaseCodes != null);
            Debug.Assert(tmaxCaseCodes.Count != 0);

            //	Make sure we have case codes available for this database
            if (this.CaseCodes == null) return false;

            try
            {
                //	Get the value to use for the next sort identifier
                lSortOrder = this.CaseCodes.GetNextSortOrder();

                //	Get the index of the insertion point
                if (tmaxInsertAt != null)
                    iIndex = this.CaseCodes.IndexOf(tmaxInsertAt);
                if (iIndex >= 0)
                {
                    //	Move down one if inserting after the specified object
                    if (bInsertBefore == false)
                    {
                        if ((iIndex = iIndex + 1) >= this.CaseCodes.Count)
                            iIndex = -1; //	Same as adding to the end of collection
                    }

                }// if(iIndex >= 0)

                //	Remove those that follow the insertion point so that we can add
                //	the new to the end of the collection
                if (iIndex >= 0)
                {
                    tmaxReorder = new CTmaxCaseCodes();

                    while (iIndex < this.CaseCodes.Count)
                    {
                        tmaxReorder.Add(this.CaseCodes[iIndex]);
                        this.CaseCodes.RemoveAt(iIndex);
                    }

                }// if(iIndex >= 0)

                //	Are we adding to the database?
                if (this.DxCaseCodes != null)
                {
                    //	Set up the AutoId mode as requested by the caller
                    bOldAutoId = this.DxCaseCodes.AutoIdEnabled;
                    this.DxCaseCodes.AutoIdEnabled = bAutoId;
                }

                //	Add records for each new code
                foreach (CTmaxCaseCode O in tmaxCaseCodes)
                {
                    //	Set the sort order for this new code
                    O.SortOrder = lSortOrder++;

                    //	Add a record if using the database
                    if (this.DxCaseCodes != null)
                    {
                        dxCaseCode = new CDxCaseCode(O);

                        if ((dxCaseCode = this.DxCaseCodes.Add(dxCaseCode)) != null)
                        {
                            O.DxRecord = dxCaseCode;
                        }

                    }// if(this.DxCaseCodes != null)

                    //	Add to the application collection
                    this.CaseCodes.Add(O);

                    if (tmaxAdded != null)
                        tmaxAdded.Add(new CTmaxItem(O));

                }// foreach(CTmaxCaseCode O in tmaxCaseCodes)

                //	Restore any that were removed from the collection
                if ((tmaxReorder != null) && (tmaxReorder.Count > 0))
                {
                    //	Add each back to the application collection
                    foreach (CTmaxCaseCode O in tmaxReorder)
                        this.CaseCodes.Add(O);
                    tmaxReorder.Clear();

                    //	Make sure all sort order values are updated
                    for (int i = 0; i < this.CaseCodes.Count; i++)
                    {
                        if (this.CaseCodes[i].SortOrder != (i + 1))
                        {
                            this.CaseCodes[i].SortOrder = (i + 1);

                            if (this.CaseCodes[i].DxRecord != null)
                                this.DxCaseCodes.Update((CDxCaseCode)(this.CaseCodes[i].DxRecord));
                        }

                    }// for(int i = 0; i < this.CaseCodes.Count; i++)

                }// if((tmaxReorder != null) && (tmaxReorder.Count > 0))

            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_CASE_CODES_EX), Ex);
            }
            finally
            {
                if (this.DxCaseCodes != null)
                    this.DxCaseCodes.AutoIdEnabled = bOldAutoId;
                else
                    this.CodesManager.Modified = true; // Force a Save operation

                //	Make sure the collection is sorted
                this.CaseCodes.Sort(true);
            }

            return true;

        }// public bool AddCaseCodes(CTmaxCaseCodes tmaxCaseCodes, bool bAutoId)

        /// <summary>This method will load the case codes and pick lists defined in the specified case codes manager</summary>
        /// <param name="tmaxCodesManager">The manager that contains the new codes and pick lists</param>
        /// <returns>true if successful</returns>
        public bool SetCaseCodes(CTmaxCodesManager tmaxCodesManager)
        {
            bool bSuccessful = false;
            string strFileSpec = "";

            Debug.Assert(tmaxCodesManager != null);

            //	Make sure we have case codes available for this database
            if (this.CodesManager == null) return false;
            if (this.CaseCodes == null) return false;

            try
            {
                //	Get the path to the default XML file for this case
                strFileSpec = GetCaseCodesFileSpec(false);

                //	Force a refresh of the database information
                tmaxCodesManager.CaseId = "";

                //	Save this file to the default location
                if (tmaxCodesManager.SaveAs(strFileSpec, true, false) == false)
                    return false;

                //	Clear the pick lists table if using the database
                if (this.DxPickLists != null)
                    this.DxPickLists.Children.Flush();

                //	Clear the case codes table if using the database
                if (this.DxCaseCodes != null)
                    this.DxCaseCodes.Flush();

                //	Populate the active codes manager using the new file
                if (GetCodesFromFile() == true)
                {
                    //	Now fill the database tables
                    FillCaseCodes();

                    bSuccessful = true;

                }// if(GetCodesFromFile() == true)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_CASE_CODES_EX), Ex);
            }
            finally
            {
                //	Make sure the collection is sorted
                if (this.CaseCodes != null)
                    this.CaseCodes.Sort(true);
            }

            return bSuccessful;

        }// public bool SetCaseCodes(CTmaxCodesManager tmaxCodesManager)


        /// <summary>This method will flush out the case codes from the data base.</summary>
        /// <returns>true if successful</returns>
        public bool SetCaseCodesForTextFile()
        {
            bool bSuccessful = true;

            try
            {
                //	Clear the case codes table if using the database
                if (this.DxCaseCodes != null)
                    this.DxCaseCodes.Flush();

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_CASE_CODES_EX), Ex);
            }
            finally
            {
                //	Make sure the collection is sorted
                if (this.CaseCodes != null)
                    this.CaseCodes.Sort(true);
            }

            return bSuccessful;

        }// public bool SetCaseCodesForTextFile(CTmaxCodesManager tmaxCodesManager)

        /// <summary>This method will use the current registration options to adjust the caller's string to be used as a MediaID</summary>
        ///	<param name="strString">The source string</param>
        /// <param name="eMediaType">The type of media being assigned</param>
        ///	<returns>The adjusted MediaId value</returns>
        public string AdjustForMediaId(string strString, TmaxMediaTypes eMediaType)
        {
            string strAdjusted = strString;
            int iIndex = 0;

            Debug.Assert(m_tmaxRegisterOptions != null);
            if (m_tmaxRegisterOptions == null) return strString;

            //	Don't worry about depositions because they are never
            //	brought up with a barcode
            if (eMediaType == TmaxMediaTypes.Deposition) return strString;

            //	Should we truncate at a space?
            if (m_tmaxRegisterOptions.MediaIdAdjustments.GetSelected(RegMediaIdAdjustments.TruncateOnSpace) == true)
            {
                if ((iIndex = strAdjusted.IndexOf(' ')) > 0)
                {
                    strAdjusted = strAdjusted.Substring(0, iIndex);
                }

            }

            //	Should we truncate at a hyphen?
            if (m_tmaxRegisterOptions.MediaIdAdjustments.GetSelected(RegMediaIdAdjustments.TruncateOnHyphen) == true)
            {
                if ((iIndex = strAdjusted.IndexOf('-')) > 0)
                {
                    strAdjusted = strAdjusted.Substring(0, iIndex);
                }

            }

            //	Are we stripping all instances of zero padding?
            if (m_tmaxRegisterOptions.MediaIdAdjustments.GetSelected(RegMediaIdAdjustments.StripZerosAll) == true)
            {
                strAdjusted = CTmaxToolbox.StripZeroPadding(strAdjusted, true);
            }

                //	Are we stripping the first numeric block?
            else if (m_tmaxRegisterOptions.MediaIdAdjustments.GetSelected(RegMediaIdAdjustments.StripZerosFirst) == true)
            {
                strAdjusted = CTmaxToolbox.StripZeroPadding(strAdjusted, false);
            }

            //	Make sure the string is properly formatted	
            return FormatMediaId(strAdjusted, eMediaType);

        }// public string AdjustForMediaId(string strString, TmaxMediaTypes eMediaType)

        /// <summary>This method will use the current registration options to adjust the caller's string to be assigned to the Name field</summary>
        ///	<param name="strString">The source string</param>
        /// <param name="eMediaType">The type of media being assigned</param>
        ///	<returns>The adjusted Name value</returns>
        public string AdjustForName(string strString, TmaxMediaTypes eMediaType)
        {
            string strAdjusted = strString;
            int iIndex = 0;
            //bool	bAdjusted = false;

            Debug.Assert(m_tmaxRegisterOptions != null);
            if (m_tmaxRegisterOptions == null) return strString;

            //	We don't modify the name for depositions
            if (eMediaType == TmaxMediaTypes.Deposition) return strString;

            //	Use the remainder if the string if truncating at a space
            if (m_tmaxRegisterOptions.MediaIdAdjustments.GetSelected(RegMediaIdAdjustments.TruncateOnSpace) == true)
            {
                iIndex = strAdjusted.IndexOf(' ');

                if ((iIndex >= 0) && (iIndex < strAdjusted.Length - 1))
                {
                    strAdjusted = strAdjusted.Substring(iIndex + 1);
                    strAdjusted = strAdjusted.Trim();
                    //bAdjusted = true;
                }

            }

            //	REMOVED THIS TEST 06-28-2006 in response to complaint from Eric
            //	
            //	Don't remember why we put it in but I'm sure it's going to
            //	come back to haunt us

            //	Don't bother if we've already adjusted
            //			if(bAdjusted == false)
            //			{
            //	Should we truncate at a hyphen?
            if (m_tmaxRegisterOptions.MediaIdAdjustments.GetSelected(RegMediaIdAdjustments.TruncateOnHyphen) == true)
            {
                iIndex = strAdjusted.IndexOf('-');

                if ((iIndex >= 0) && (iIndex < strAdjusted.Length - 1))
                {
                    strAdjusted = strAdjusted.Substring(iIndex + 1);
                    strAdjusted = strAdjusted.Trim();
                }

            }

            //			}// if(strAdjusted.Length == 0)

            //	Put back the caller's string if we stripped everything
            if (strAdjusted.Length == 0)
                strAdjusted = strString;

            return strAdjusted;

        }// public string AdjustForName(string strString, TmaxMediaTypes eMediaType)

        /// <summary>This method is called to determine if the exported image for the specified slide is valid</summary>
        /// <param name="dxSlide">Secondary exchange object that represents the slide</param>
        /// <param name="bUpdate">True to update the exported image if required</param>
        ///	<param name="bSilent">True to suppress status form during export operation</param>
        /// <returns>true if the exported image is valid</returns>
        public bool CheckSlide(CDxSecondary dxSlide, bool bUpdate, bool bSilent)
        {
            string strPresentation = "";
            string strSlide = "";
            System.DateTime dtPresentation;
            System.DateTime dtSlide;
            bool bRequiresUpdate = false;

            Debug.Assert(dxSlide != null);
            Debug.Assert(dxSlide.MediaType == TmaxMediaTypes.Slide);
            Debug.Assert(dxSlide.Primary != null);
            if ((dxSlide == null) ||
                (dxSlide.Primary == null) ||
                (dxSlide.MediaType != TmaxMediaTypes.Slide)) return false;

            //	Get the specification for the presentation file
            strPresentation = GetFileSpec(dxSlide.Primary);
            if ((strPresentation == null) || (strPresentation.Length == 0))
                return false;

            //	Get the specification for the exported slide
            strSlide = GetFileSpec(dxSlide);
            if ((strSlide == null) || (strSlide.Length == 0))
                return false;

            //	Make sure the presentation file exists
            if (System.IO.File.Exists(strPresentation) == false)
            {
                return false;
            }

            //	Does the exported image exist?
            if (System.IO.File.Exists(strSlide) == true)
            {
                //	Get the last modified times for each file
                dtPresentation = System.IO.File.GetLastWriteTime(strPresentation);
                dtSlide = System.IO.File.GetLastWriteTime(strSlide);

                //	Does this slide need to be updated?
                if (dtPresentation > dtSlide)
                    bRequiresUpdate = true;
            }
            else
            {
                //	Need to export the image
                bRequiresUpdate = true;
            }

            //	Does the slide need to be updated?
            if (bRequiresUpdate == true)
            {
                //	Should we update the exported image now?
                if (bUpdate == true)
                    return ExportSlide(dxSlide, bSilent);
                else
                    return false; // Image is out of date
            }
            else
            {
                return true;	//	OK to use existing export
            }

        }// public bool CheckSlide(CDxSecondary dxSlide)

        /// <summary>This method is called to determine if the exported image for the specified slide is valid</summary>
        /// <param name="dxSlide">Secondary exchange object that represents the slide</param>
        /// <param name="bUpdate">True to update the exported image if required</param>
        /// <returns>true if the exported image is valid</returns>
        public bool CheckSlide(CDxSecondary dxSlide, bool bUpdate)
        {
            return CheckSlide(dxSlide, bUpdate, false);
        }

        /// <summary>This method is called to determine if the exported image for the specified slide is valid</summary>
        /// <param name="dxSlide">Secondary exchange object that represents the slide</param>
        /// <returns>true if the exported image is valid</returns>
        public bool CheckSlide(CDxSecondary dxSlide)
        {
            return CheckSlide(dxSlide, true, false);
        }

        /// <summary>This method is called to disable/enable the application's keyboard hook</summary>
        /// <param name="bDisable">True to disable the hook</param>
        public void DisableTmaxKeyboard(bool bDisable)
        {
            if (m_tmaxAppOptions != null)
                m_tmaxAppOptions.DisableTmaxKeyboard = bDisable;
        }

        /// <summary>Called to compact the specified database</summary>
        /// <param name="strFileSpec">The fully qualified path to the database</param>
        public bool Compact(string strFileSpec)
        {
            bool bSuccessful = false;

            try
            {
                //	Allocate the database compactor
                m_tmaxCompactor = new CTmaxDatabaseCompactor();
                SetHandlers(m_tmaxCompactor.EventSource);
                m_tmaxCompactor.Finished += new System.EventHandler(this.OnCompactorFinished);

                //	Initialize the operation
                if (m_tmaxCompactor.Initialize(strFileSpec) == true)
                {
                    m_wndCompactorStatus = new CFCompactorStatus();

                    if ((m_tmaxAppOptions != null) && (m_tmaxAppOptions.AppMainForm != null))
                        m_wndCompactorStatus.Owner = m_tmaxAppOptions.AppMainForm;

                    m_wndCompactorStatus.FileSpec = m_tmaxCompactor.SourceFileSpec;

                    //	Run the registration in it's own thread
                    Thread thread = new Thread(new ThreadStart(m_tmaxCompactor.Execute));
                    thread.Start();

                    m_wndCompactorStatus.ShowDialog();

                    bSuccessful = m_tmaxCompactor.Successful;

                }// if(m_tmaxCompactor.Initialize(strFileSpec) == true)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "Compact", this.ExBuilder.Message(ERROR_CASE_DATABASE_COMPACT_EX, strFileSpec), Ex);
            }
            finally
            {
                //	Make sure the status form has been shut down
                if (m_wndCompactorStatus != null)
                {
                    if (m_wndCompactorStatus.Visible == true)
                        m_wndCompactorStatus.Close();
                    m_wndCompactorStatus = null;
                }

                m_tmaxCompactor = null;

            }

            return bSuccessful;

        }// public bool Compact(string strFileSpec)

        /// <summary>Call to close the database</summary>
        public override void Close()
        {
            //	Close the objections database
            m_tmaxObjectionsDatabase.Close();

            //	Release the local data exchange objects
            m_dxUser = null;
            m_dxDetail = null;
            m_dxTargetBinder = null;

            //	Release the record collections
            if (m_dxDetails != null)
                m_dxDetails = m_dxDetails.Dispose();
            if (m_dxUsers != null)
                m_dxUsers = m_dxUsers.Dispose();
            if (m_dxPrimaries != null)
                m_dxPrimaries = m_dxPrimaries.Dispose();
            if (m_dxTranscripts != null)
                m_dxTranscripts = m_dxTranscripts.Dispose();
            if (m_dxHighlighters != null)
                m_dxHighlighters = m_dxHighlighters.Dispose();
            if (m_dxBarcodeMap != null)
                m_dxBarcodeMap = m_dxBarcodeMap.Dispose();
            if (m_dxRootBinder != null)
            {
                if (m_dxRootBinder.Contents != null)
                    m_dxRootBinder.Contents.Dispose();

                m_dxRootBinder = null;
            }

            if (m_dxFiltered != null)
            {
                m_dxFiltered.Clear();
                m_dxFiltered = null;
            }
            m_tmaxLastFilter = null;

            //	Save and close the case options
            m_tmaxCaseOptions.Terminate();

            //	Reset the station options
            if (m_tmaxStationOptions != null)
            {
                //	Make sure we have the latest Hidden attributes
                if (this.CaseCodes != null)
                    m_tmaxStationOptions.SaveCaseCodes(this.CaseCodes);

                m_tmaxStationOptions.Closed = System.DateTime.Now.ToString();
                m_tmaxStationOptions.Save();
                m_tmaxStationOptions.Clear();
                m_tmaxStationOptions = null;
            }

            //	Reset the case codes manager
            if (m_tmaxCodesManager != null)
            {
                m_tmaxCodesManager.Terminate();
                m_tmaxCodesManager = null;
            }
            m_bCodesConversionRequired = false;

            //	Close the provider connection
            base.Close();

            //	Reset the local members
            //
            //	NOTE:	Don't reset the Filename value because
            //			we need it to construct the path
            m_strFolder = "";
            m_aCaseFolders.Clear();

        }// public void Close()

        /// <summary>Call to create a new database</summary>
        /// <param name="strFolder">Folder containing the case database</param>
        /// <param name="strUser">Name of user opening the database</param>
        /// <returns>true if successful</returns>
        public bool Create(string strFolder, string strUser)
        {
            string strTemplate = "";
            string strTarget = "";
            bool bSuccessful = false;

            //	Build the file specification for the template database
            if ((m_strAppFolder == null) || (m_strAppFolder.Length == 0))
            {
                strTemplate = System.Environment.CurrentDirectory;
            }
            else
            {
                strTemplate = m_strAppFolder;
            }
            if (strTemplate.EndsWith("\\") == false)
                strTemplate += "\\";
            strTemplate += CASE_DATABASE_TEMPLATE_FILENAME;

            //	Get the full path specification for the target file
            strTarget = GetFileSpec(strFolder);

            //	Make sure the template file exists
            if (FindFile(strTemplate) == false)
            {
                FireError(this, "Create", this.ExBuilder.Message(ERROR_CASE_DATABASE_CREATE_NO_TEMPLATE, strTemplate));
                return false;
            }

            try
            {
                //	Make sure the folder exists
                if (System.IO.Directory.Exists(strFolder) == false)
                    System.IO.Directory.CreateDirectory(strFolder);

                //	Assume the caller has already checked for overwrite
                //	permission
                System.IO.File.Copy(strTemplate, strTarget, true);

                //	Make sure it is not read-only
                System.IO.File.SetAttributes(strTarget, System.IO.FileAttributes.Normal);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "Create", this.ExBuilder.Message(ERROR_CASE_DATABASE_CREATE_COPY_FAILED, strTemplate, strTarget), Ex);
                return false;
            }

            //	Open the database
            if (Open(strFolder, false) == true)
            {
                //	Set the current user
                if (SetUser(strUser) == true)
                {
                    SetHighlighters();

                    //	Set the version information
                    bSuccessful = SetDetails();

                    //	Initialize the case codes for fielded data
                    //
                    //	NOTE:	This must be done AFTER setting the details
                    InitializeCaseCodes(true);

                    //	Initialize the station specific options
                    SetStationOptions();

                    //	Initialize the objections database
                    InitializeObjections();
                }

            }// if(Open(strFolder, false) == true)

            //	Clean up if an error occurred
            if (bSuccessful == false)
                Close();

            return bSuccessful;

        }// public bool Create(string strFolder, string strUser)

        /// <summary>This method will delete each of the records associated with the specified items</summary>
        /// <param name="tmaxItem">The event item that represents the parent of those records being deleted</param>
        /// <param name="tmaxResults">Collection to populate with items that represent records that have been deleted</param>
        /// <returns>true if records have been deleted</returns>
        public bool Delete(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {
            long lDeleted = 0;

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.SubItems != null);

            //	Is there anything to delete?
            if (tmaxItem.SubItems.Count == 0) return false;

            //	Initialize the opearation
            m_tmaxTrashCan.Clear();
            m_bDeleteAll = false;
            m_bDeletingReferences = false;

            //	Is the caller deleting objections?
            if (IsObjectionsCommand(tmaxParameters) == true)
            {
                return DeleteObjections(tmaxItem, tmaxParameters, tmaxResults);
            }
            else if (tmaxItem.DataType == TmaxDataTypes.Binder)
            {
                DeleteBinders(tmaxItem);
            }
            else if (tmaxItem.DataType == TmaxDataTypes.PickItem)
            {
                DeletePickItems(tmaxItem);
            }
            else if (tmaxItem.DataType == TmaxDataTypes.CaseCode)
            {
                DeleteCaseCodes(tmaxItem, true, true);
            }
            else
            {
                //	Delete the requested media records
                DeleteMedia(tmaxItem);
            }

            //	How many records were deleted?
            lDeleted = m_tmaxTrashCan.Count;

            //	Transfer the items from the trash can to the caller's collection
            if ((tmaxResults != null) && (tmaxResults.Deleted != null))
            {
                foreach (CTmaxItem tmaxTrash in m_tmaxTrashCan)
                    tmaxResults.Deleted.Add(tmaxTrash);
            }

            m_tmaxTrashCan.Clear();
            m_bDeleteAll = false;
            m_bDeletingReferences = false;

            return (lDeleted > 0);

        }// public bool Delete(CTmaxItem tmaxItem, CTmaxItems tmaxDeleted)

        /// <summary>This method make a duplicate of the specified source record</summary>
        /// <param name="tmaxSource">The TrialMax event item that represents the source record</param>
        /// <param name="tmaxAdded">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        public bool Duplicate(CTmaxItem tmaxSource, CTmaxItems tmaxAdded)
        {
            CTmaxItem tmaxScript = null;
            CTmaxItem tmaxParent = null;
            CDxPrimary dxScript = null;
            CDxPrimary dxSource = null;
            CDxSecondary dxScene = null;

            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.GetMediaRecord() != null);
            Debug.Assert(tmaxAdded != null);
            Debug.Assert(tmaxAdded.Count == 0);

            //	The collection should be empty
            tmaxAdded.Clear();

            //	The source should be a script
            if (tmaxSource == null) return false;
            if (tmaxSource.GetMediaRecord() == null) return false;
            if (tmaxSource.MediaType != TmaxMediaTypes.Script) return false;

            try
            {
                dxSource = (CDxPrimary)tmaxSource.IPrimary;

                //	Make sure we have all the scenes loaded
                if ((dxSource.Secondaries == null) || (dxSource.Secondaries.Count == 0))
                {
                    dxSource.Fill();
                }

                //	Start by adding a new primary record for the new
                //	script
                if ((dxScript = AddPrimary(TmaxMediaTypes.Script)) != null)
                {
                    //	Copy the source attributes
                    dxScript.Attributes = dxSource.Attributes;

                    //	Create a new event item to represent the new script
                    tmaxScript = new CTmaxItem(dxScript);

                    //	Now duplicate each of the scenes in the source collection
                    foreach (CDxSecondary O in dxSource.Secondaries)
                    {
                        if (O.GetSource() != null)
                        {
                            if ((dxScene = AddScene(dxScript, O.GetSource(), false, null)) != null)
                                tmaxScript.SubItems.Add(new CTmaxItem(dxScene));
                        }
                    }

                    //	Just playing it safe ...
                    dxScript.ChildCount = dxScript.Secondaries.Count;
                    dxScript.SetPlaylistFromChildren();
                    m_dxPrimaries.Update(dxScript);

                    //	This allows the event trigger to grab the interface to the new script
                    if (tmaxSource.ReturnItem == null)
                        tmaxSource.ReturnItem = new CTmaxItem(dxScript);

                    //	NOTE:	To make this appear like any other Add operation the
                    //			main application wants us to return the Parent of the 
                    //			new script instead of the script itself
                    tmaxParent = new CTmaxItem();
                    tmaxParent.MediaType = TmaxMediaTypes.Script;
                    tmaxParent.SubItems.Add(tmaxScript);

                    //	Add the item to the caller's collection
                    tmaxAdded.Add(tmaxParent);
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "Duplicate", this.ExBuilder.Message(ERROR_CASE_DATABASE_DUPLICATE_EX, GetBarcode(dxScript, false)), Ex);
            }

            return (tmaxAdded.Count > 0);

        }// public bool Duplicate(CTmaxItem tmaxSource, CTmaxItems tmaxAdded)

        /// <summary>This method will edit the specified designation</summary>
        /// <param name="tmaxItems">Collection containing the designations to be edited</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        public bool EditDesignations(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {
            bool bSuccessful = false;

            Debug.Assert(m_tmaxDesignationEditor != null, "NULL Designation Editor");
            if (m_tmaxDesignationEditor == null) return false;

            try
            {
                //	Prepare the editor for a new operation
                if (m_tmaxDesignationEditor.Initialize(tmaxItems, tmaxParameters, tmaxResults) == true)
                {
                    //	Execute the operation
                    bSuccessful = m_tmaxDesignationEditor.Edit();
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "EditDesignations", this.ExBuilder.Message(ERROR_CASE_DATABASE_EDIT_DESIGNATIONS_EX), Ex);
                bSuccessful = false;
            }
            finally
            {
            }

            return bSuccessful;

        }// public bool EditDesignations(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

        /// <summary>Call to determine if the case file exists in the specified folder</summary>
        /// <param name="strFolder">Folder containing the case database</param>
        /// <returns>true if the database exists</returns>
        public bool Exists(string strFolder)
        {
            string strFileSpec = "";

            //	Get the fully qualified path for the database file
            strFileSpec = GetFileSpec(strFolder);

            return FindFile(strFileSpec);

        }// Exists(string strFolder)

        /// <summary>This method will expand the Id.Id.Id ... binder path specification to its string equivalent</summary>
        /// <param name="strPath">The formatted binder path descriptor</param>
        /// <returns>The expanded path</returns>
        public string ExpandBinderPath(string strPath)
        {
            CDxBinderEntry dxBinder = null;
            string strExpanded = "";

            //	Get the binder associated with this path
            if ((dxBinder = GetBinderFromPath(strPath)) != null)
            {
                //	Build the expanded path
                if (dxBinder.AutoId > 0)
                    strExpanded = dxBinder.Name;

                while ((dxBinder = dxBinder.Parent) != null)
                {
                    if (dxBinder.AutoId > 0)
                        strExpanded = dxBinder.Name + "\\" + strExpanded;
                }

            }

            return strExpanded;

        }//	public string ExpandBinderPath(string strPath)

        /// <summary>This method will export the requested records</summary>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxSource">Collection to source records to be exported</param>
        /// <returns>true if successful</returns>
        public bool Export(CTmaxParameters tmaxParameters, CTmaxItems tmaxSource)
        {
            bool bSuccessful = false;

            try
            {
                //	First check to see if the caller is importing objections
                if (IsObjectionsCommand(tmaxParameters) == true)
                {
                    bSuccessful = ExportObjections(tmaxParameters, tmaxSource);
                }
                else
                {
                    //	Prepare the manager for a new operation
                    if (m_tmaxStationOptions != null)
                        m_tmaxExportManager.ExportOptions = m_tmaxStationOptions.ExportOptions;
                    m_tmaxExportManager.WMEncoder = this.WMEncoder;

                    //	Initialize the operation
                    if (m_tmaxExportManager.Initialize(tmaxParameters, tmaxSource) == false)
                        return false;

                    //	We MUST have a valid export format
                    if (m_tmaxExportManager.Format == TmaxExportFormats.Unknown)
                    {
                        Debug.Assert(m_tmaxExportManager.Format != TmaxExportFormats.Unknown, "Unknown export format specification");
                        return false;
                    }

                    Cursor.Current = Cursors.WaitCursor;

                    //	Execute the operation
                    bSuccessful = m_tmaxExportManager.Export();

                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "Export", this.ExBuilder.Message(ERROR_CASE_DATABASE_EXPORT_EX), Ex);
                bSuccessful = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            return bSuccessful;

        }// public bool Export(CTmaxParameters tmaxParameters, CTmaxItems tmaxSource)

        /// <summary>This method will export the requested records</summary>
        /// <param name="eFormat">The enumerated export format</param>
        /// <param name="tmaxSource">Collection of source records to be exported</param>
        /// <returns>true if successful</returns>
        public bool Export(TmaxExportFormats eFormat, CTmaxItems tmaxSource)
        {
            CTmaxParameters tmaxParameters = new CTmaxParameters();
            tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ExportFormat, (int)eFormat));
            return Export(tmaxParameters, tmaxSource);
        }

        /// <summary>This method will export the requested records</summary>
        /// <param name="eFormat">The enumerated export format</param>
        /// <returns>true if successful</returns>
        public bool Export(TmaxExportFormats eFormat)
        {
            return Export(eFormat, null);
        }

        /// <summary>This method is called to export the specified slide</summary>
        /// <param name="dxSlide">Secondary exchange object that represents the slide</param>
        /// <param name="bSilent">true to suppress the export status form</param>
        /// <returns>true if successful</returns>
        public bool ExportSlide(CDxSecondary dxSlide, bool bSilent)
        {
            CMSPowerPoint Presentation = null;
            bool bSuccessful = false;
            string strPresentation = "";
            string strSlide = "";
            CFExportSlide cfExport = null;

            Debug.Assert(dxSlide.MediaType == TmaxMediaTypes.Slide);
            Debug.Assert(dxSlide.Primary != null);
            if ((dxSlide == null) ||
                (dxSlide.Primary == null) ||
                (dxSlide.MediaType != TmaxMediaTypes.Slide)) return false;

            //	Get the specification for the presentation file
            strPresentation = GetFileSpec(dxSlide.Primary);
            if ((strPresentation == null) || (strPresentation.Length == 0))
                return false;

            //	Make sure the presentation exists
            if (System.IO.File.Exists(strPresentation) == false)
            {
                FireError(this, "ExportSlide", this.ExBuilder.Message(ERROR_CASE_DATABASE_EXPORT_SLIDE_FILE_NOT_FOUND, strPresentation));
                return false;
            }

            //	Get the specification for the exported slide
            strSlide = GetFileSpec(dxSlide);
            if ((strSlide == null) || (strSlide.Length == 0))
                return false;

            //	Delete the existing exported slide
            if (System.IO.File.Exists(strSlide) == true)
            {
                try { System.IO.File.Delete(strSlide); }
                catch { }
            }

            try
            {
                //	Display the status form unless supressed by the caller
                if (bSilent == false)
                {
                    cfExport = new CFExportSlide();
                    cfExport.Presentation = strPresentation.ToLower();
                    cfExport.Export = strSlide.ToLower();
                    cfExport.Show();
                    cfExport.Refresh();
                }

                Presentation = new FTI.Trialmax.MSOffice.MSPowerPoint.CMSPowerPoint();
                SetHandlers(Presentation.EventSource);

                if (Presentation.Initialize() == true)
                {
                    //	Open the presentation
                    if (Presentation.Open(strPresentation) == true)
                    {
                        //	Make sure the target folder exists
                        if (CreateFolder(dxSlide, true, !bSilent) == true)
                        {
                            //	Export the slide
                            bSuccessful = Presentation.Export((int)dxSlide.MultipageId, strSlide);
                        }

                    }// if(Presentation.Open(strSource) == true)

                }// if(Presentation.Initialize() == true)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "ExportSlide", this.ExBuilder.Message(ERROR_CASE_DATABASE_EXPORT_SLIDE_EX, dxSlide.MultipageId, strSlide), Ex);
            }
            finally
            {
                //	Make sure the presentation is closed
                if (Presentation != null)
                {
                    Presentation.Close();
                    Presentation.Terminate();
                }

                //	Make sure the status form is closed
                if (cfExport != null)
                {
                    cfExport.Hide();
                    cfExport.Dispose();
                }

            }

            return bSuccessful;

        }// private bool ExportSlide(CDxSecondary dxSlide)

        /// <summary>This method is called to export the specified slide</summary>
        /// <param name="dxSlide">Secondary exchange object that represents the slide</param>
        /// <returns>true if successful</returns>
        public bool ExportSlide(CDxSecondary dxSlide)
        {
            return ExportSlide(dxSlide, false);
        }

        /// <summary>This method will add a secondary media object for each slide in the primary PowerPoint presentation</summary>
        /// <param name="dxPrimary">The primary media object associated with the PowerPoint presentation</param>
        /// <returns>true if successful</returns>
        public bool ExportSlides(CDxPrimary dxPrimary)
        {
            FTI.Trialmax.MSOffice.MSPowerPoint.CMSPowerPoint Presentation = null;
            CDxSecondary dxSecondary = null;
            CTmaxSourceFile tmaxFile = null;
            string strPresentation = "";
            string strFolder = "";
            string strSlide = "";
            bool bSuccessful = false;
            long lSlides = 0;
            bool bExport = true;

            Debug.Assert(dxPrimary != null);
            Debug.Assert(dxPrimary.Secondaries != null);
            Debug.Assert(dxPrimary.Secondaries.Count == 0);

            try
            {
                //	Get the path to the presentation file
                //
                //	NOTE:	Do this first so that it can be provided with error messages
                strPresentation = GetFileSpec(dxPrimary);

                Presentation = new FTI.Trialmax.MSOffice.MSPowerPoint.CMSPowerPoint();
                SetHandlers(Presentation.EventSource);

                //	Make sure the target folder for exported images exists
                strFolder = GetSlidesFolderSpec(dxPrimary);
                bExport = CreateFolder(TmaxMediaTypes.Slide, strFolder, true);

                //	Open the presentation
                if (Presentation.Initialize() == false) return false;
                if (Presentation.Open(strPresentation) == false) return false;

                //	Get the number of slides in this presentation
                if ((lSlides = Presentation.GetSlideCount()) > 0)
                {
                    //	Set up a TrialMax source file to simulate registration
                    tmaxFile = new CTmaxSourceFile();
                    tmaxFile.Path = strPresentation;

                    //	Add one secondary for each slide
                    for (int i = 1; i <= lSlides; i++)
                    {
                        //	Has the user canceled?
                        if (m_bRegisterCancelled) return true;

                        //	Create a new secondary
                        if ((dxSecondary = CreateSecondary(dxPrimary, tmaxFile)) != null)
                        {
                            //	Get the unique id assigned by PowerPoint
                            dxSecondary.MultipageId = Presentation.GetSlideId(i);

                            //dxSecondary.Name = ("Slide:" + dxSecondary.MultipageId.ToString());

                            // This causes PowerPoint server timeouts on Office 2K
                            //dxSecondary.Description = Presentation.GetSlideTitle(i);

                            //	Construct the name of the exported image
                            dxSecondary.Filename = (dxSecondary.MultipageId.ToString() + CASE_DATABASE_POWERPOINT_EXPORT_EXTENSION);

                            if (bExport == true)
                            {
                                //	Get the full path to the exported image
                                strSlide = GetFileSpec(dxSecondary);

                                //	Update the progress form
                                // SetRegisterProgress("Exporting " + strSlide);
                                if (m_bRegisterCancelled) return true;

                                //	Export the image
                                Presentation.Export((int)dxSecondary.MultipageId, strSlide);
                            }

                            //	Add this to the secondaries collection
                            dxPrimary.Add(dxSecondary);
                        }

                    }// for(int i = 1; i <= lSlides; i++)

                }// if((lSlides = Presentation.GetSlideCount()) > 0)

                //	Update the number of children in the primary object
                if (dxPrimary.Secondaries.Count > 0)
                {
                    dxPrimary.ChildCount = dxPrimary.Secondaries.Count;
                    m_dxPrimaries.Update(dxPrimary);
                }

                //	Cool !
                bSuccessful = true;

            }
            catch (System.Exception Ex)
            {
                FireError(this, "ExportSlides", this.ExBuilder.Message(ERROR_CASE_DATABASE_EXPORT_SLIDES_EX, strPresentation), Ex);
                logDetailed.Error(Ex.ToString());
            }
            finally
            {
                //	Make sure the presentation is closed
                if (Presentation != null)
                {
                    Presentation.Close();
                    Presentation.Terminate();
                }
            }

            return bSuccessful;

        }//	public bool ExportSlides(CDxPrimary dxPrimary)

        /// <summary>Called by the application when it traps a TmxView event</summary>
        /// <param name="Args">The event arguments</param>
        /// <returns>true if the file exists</returns>
        public void OnTmxViewEvent(CTmxEventArgs Args)
        {
            try
            {
                //	Is this a SavedPage event?
                if (Args.Event == TmxEvents.SavedPage)
                {
                    //	Is a registration in progress?
                    if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                    {
                        string strMsg = String.Format("Splitting {0}: Pg {1} of {2} to {3}",
                                                    System.IO.Path.GetFileName(Args.Filename),
                                                    Args.PageNumber,
                                                    Args.TotalPages,
                                                    System.IO.Path.GetFileName(Args.PageFilename));
                        SetRegisterProgress(strMsg);
                    }

                }// if(Args.Event == TmxEvents.SavedPage)

            }
            catch
            {
            }

        }// public void OnTmxViewEvent(CTmxEventArgs Args)

        /// <summary>This method is called to fire an error event</summary>
        /// <param name="Args">Argument object to be passed in the event</param>
        public override void FireError(CTmaxErrorArgs Args)
        {
            if (Args != null)
            {
                //	Notify the registration status form if it exists and the operation has not been cancelled
                if ((m_cfRegisterProgress != null) && (m_bRegisterCancelled == false))
                {
                    Args.Show = false; // Don't use the application popup
                    m_cfRegisterProgress.OnError(this, Args);
                }

                //	Notify the import status form if it exists and the import operation has not been cancelled
                else if ((m_tmaxImportManager.StatusForm != null) && (m_tmaxImportManager.StatusForm.IsDisposed == false) &&
                        (m_tmaxImportManager.Cancelled == false))
                {
                    Args.Show = false; // Don't use the application popup

                    //	Are we importing objections?
                    if ((m_tmaxImportObjectionsManager != null) && (m_tmaxImportObjectionsManager.ObjectionsDatabase != null))
                        m_tmaxImportObjectionsManager.AddMessage(Args.Message, TmaxMessageLevels.CriticalError);
                    else
                        m_tmaxImportManager.AddMessage(Args.Message, TmaxMessageLevels.CriticalError);
                }

                //	Let the base class do it's processing
                base.FireError(Args);

            }// if(Args != null)

        }// public override void FireError(CTmaxErrorArgs Args)

        /// <summary>This method fire the InternalUpdate event for the specified record</summary>
        /// <param name="dxRecord">The record that has been updated</param>
        public void FireInternalUpdate(CDxMediaRecord dxRecord)
        {
            CTmaxItems tmaxItems = null;

            Debug.Assert(dxRecord != null);

            if ((dxRecord != null) && (InternalUpdateEvent != null))
            {
                try
                {
                    //	Allocate the event item collection
                    tmaxItems = new CTmaxItems();

                    //	Add an item to represent the specified record
                    tmaxItems.Add(new CTmaxItem(dxRecord));

                    //	Fire the event
                    InternalUpdateEvent(this, tmaxItems);

                }
                catch (System.Exception Ex)
                {
                    if (dxRecord.Collection != null)
                        FireError(this, "FireInternalUpdate", this.ExBuilder.Message(ERROR_CASE_DATABASE_FIRE_INTERNAL_UPDATE_EX, dxRecord.GetBarcode(false)), Ex, dxRecord.Collection.GetErrorItems(dxRecord));
                    else
                        FireError(this, "FireInternalUpdate", this.ExBuilder.Message(ERROR_CASE_DATABASE_FIRE_INTERNAL_UPDATE_EX, dxRecord.GetBarcode(false)), Ex);
                }

            }// if((dxRecord != null) && (InternalUpdate != null))

        }// public void FireInternalUpdate(CDxMediaRecord dxRecord)

        /// <summary>This method is called to fire a command event using the specified values</summary>
        /// <param name="Args">Command argument object</param>
        /// <returns>The argument object used to fire the command event</returns>
        public CTmaxCommandArgs FireCommand(CTmaxCommandArgs Args)
        {
            try
            {
                //	Is anybody registered?
                if (TmaxCommandEvent != null)
                {
                    TmaxCommandEvent(this, Args);
                }
                else
                {
                    Args.Successful = false;
                }
            }
            catch (System.Exception Ex)
            {
                FireError(this, "FireCommand", this.ExBuilder.Message(ERROR_CASE_DATABASE_FIRE_COMMAND_EX, Args.Command), Ex);
                Args.Successful = false;
            }

            return Args;

        }// public CTmaxCommandArgs FireCommand(CTmaxCommandArgs Args)

        /// <summary>This method is called to fire a command event using the specified values</summary>
        /// <param name="eCommand">The TrialMax command identifier</param>
        /// <param name="tmaxItems">The collection of items associated with the command event</param>
        /// <param name="tmaxParameters">The collection of parameters associated with the event</param>
        /// <returns>The argument object used to fire the command event</returns>
        public CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
        {
            CTmaxCommandArgs Args = null;

            try
            {
                // Get the command arguments
                if ((Args = new CTmaxCommandArgs(eCommand, m_iPaneId, tmaxItems, tmaxParameters)) != null)
                {
                    Args.Successful = false;

                    //	Fire the command event
                    return FireCommand(Args);
                }

            }
            catch
            {
            }

            return Args;

        }//	public CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

        /// <summary>This method is called to fire a command event using the specified values</summary>
        /// <param name="eCommand">The TrialMax command identifier</param>
        /// <param name="tmaxItems">The collection of items associated with the command event</param>
        /// <returns>The argument object used to fire the command event</returns>
        public CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)
        {
            return FireCommand(eCommand, tmaxItems, null);

        }//	public CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)

        /// <summary>This method is called to fire a command event using the specified values</summary>
        /// <param name="eCommand">The TrialMax command identifier</param>
        /// <param name="tmaxItem">The item to be passed with the event</param>
        /// <param name="tmaxParameters">The collection of parameters associated with the event</param>
        /// <returns>The argument object used to fire the command event</returns>
        public CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)
        {
            CTmaxItems tmaxItems = new CTmaxItems();

            tmaxItems.Add(tmaxItem);

            return FireCommand(eCommand, tmaxItems, tmaxParameters);

        }//	public CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)

        /// <summary>This method is called to fire a command event using the specified values</summary>
        /// <param name="eCommand">The TrialMax command identifier</param>
        /// <param name="tmaxItem">The item to be passed with the event</param>
        /// <returns>The argument object used to fire the command event</returns>
        public CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)
        {
            return FireCommand(eCommand, tmaxItem, null);

        }//	public CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)

        /// <summary>This method is called to fire a command event using the specified values</summary>
        /// <param name="eCommand">The TrialMax command identifier</param>
        /// <returns>The argument object used to fire the command event</returns>
        public CTmaxCommandArgs FireCommand(TmaxCommands eCommand)
        {
            return FireCommand(eCommand, (CTmaxItems)null, (CTmaxParameters)null);

        }//	public CTmaxCommandArgs FireCommand(TmaxCommands eCommand)

        /// <summary>This method is called to remove/replace invalid characters in the MediaId string</summary>
        ///	<param name="strString">The source string</param>
        /// <param name="eMediaType">The type of media being assigned</param>
        ///	<returns>The formatted MediaId value</returns>
        public string FormatMediaId(string strString, TmaxMediaTypes eMediaType)
        {
            string strFormatted = "";

            //	Does this string contain any invalid characters?
            if (strString.IndexOfAny(CASE_DATABASE_INVALID_MEDIA_ID_CHARS.ToCharArray()) >= 0)
            {
                foreach (char c in strString)
                {
                    //	Is this a valid character?
                    if (CASE_DATABASE_INVALID_MEDIA_ID_CHARS.IndexOf(c) < 0)
                        strFormatted += c;
                }

            }
            else
            {
                strFormatted = strString;
            }

            //	MediaId can't have any periods because it messes up the barcode
            strFormatted = strFormatted.Replace(".", "_");

            //	Convert to upper case
            return strFormatted.ToUpper();

        }// public string FormatMediaId(string strString)

        /// <summary>Call to retrieve the linked path for the specified record</summary>
        /// <param name="dxRecord">The primary/secondary record that has been linked</param>
        /// <returns>The linked path if successful</returns>
        public string GetAliasedPath(CDxMediaRecord dxRecord)
        {
            string strPath = "";
            string strLinked = "";
            long lAlias = 0;

            Debug.Assert(dxRecord != null);
            Debug.Assert(m_tmaxCaseOptions != null);
            Debug.Assert(m_tmaxCaseOptions.Aliases != null);

            if (m_tmaxCaseOptions == null) return "";
            if (m_tmaxCaseOptions.Aliases == null) return "";

            try
            {
                //	Get the aliasing information for this record
                if (dxRecord.GetMediaLevel() == TmaxMediaLevels.Primary)
                {
                    lAlias = ((CDxPrimary)dxRecord).AliasId;
                    strLinked = ((CDxPrimary)dxRecord).RelativePath;
                }
                else if (dxRecord.GetMediaLevel() == TmaxMediaLevels.Secondary)
                {
                    lAlias = ((CDxSecondary)dxRecord).AliasId;
                    strLinked = ((CDxSecondary)dxRecord).RelativePath;
                }
                else
                {
                    return "";
                }

                //	Get the aliased path
                if (lAlias > 0)
                    strPath = m_tmaxCaseOptions.Aliases.GetAliasedPath(lAlias, strLinked);

                if (strPath.Length == 0)
                {
                    FireError(this, "GetAliasedPath", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_ALIASED_PATH_FAILED, GetBarcode(dxRecord, false)));
                }

                return strPath;

            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetAliasedPath", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_ALIASED_PATH_EX, GetBarcode(dxRecord, false)), Ex);
                return "";
            }

        }// public string GetAliasedPath(CDxMediaRecord dxRecord)

        /// <summary>Called to retrieve the barcode for the specified record</summary>
        /// <param name="tmaxRecord">The record interface object to construct the barcode for</param>
        /// <param name="bIgnoreMapped">The true to ignore the record's foreign barcode if mapped</param>
        /// <returns>The appropriate barcode</returns>
        public string GetBarcode(ITmaxMediaRecord tmaxRecord, bool bIgnoreMapped)
        {
            string strBarcode = "0.0.0";
            CDxPrimary dxPrimary = null;
            CDxSecondary dxSecondary = null;
            CDxTertiary dxTertiary = null;
            CDxQuaternary dxQuaternary = null;

            //	Should we look for a foreign barcode first?
            if ((bIgnoreMapped == false) && (m_tmaxAppOptions.ShowForeignBarcodes == true))
            {
                if ((((CDxMediaRecord)tmaxRecord).ForeignBarcode != null) &&
                    (((CDxMediaRecord)tmaxRecord).ForeignBarcode.Length > 0))
                {
                    return ((CDxMediaRecord)tmaxRecord).ForeignBarcode;
                }

            }

            switch (tmaxRecord.GetMediaLevel())
            {
                case TmaxMediaLevels.Primary:

                    dxPrimary = (CDxPrimary)tmaxRecord;
                    break;

                case TmaxMediaLevels.Secondary:

                    dxSecondary = (CDxSecondary)tmaxRecord;
                    dxPrimary = dxSecondary.Primary;
                    Debug.Assert(dxPrimary != null);
                    break;

                case TmaxMediaLevels.Tertiary:

                    dxTertiary = (CDxTertiary)tmaxRecord;
                    dxSecondary = dxTertiary.Secondary;
                    Debug.Assert(dxSecondary != null);
                    dxPrimary = dxSecondary.Primary;
                    Debug.Assert(dxPrimary != null);
                    break;

                case TmaxMediaLevels.Quaternary:

                    dxQuaternary = (CDxQuaternary)tmaxRecord;
                    dxTertiary = dxQuaternary.Tertiary;
                    Debug.Assert(dxTertiary != null);
                    dxSecondary = dxTertiary.Secondary;
                    Debug.Assert(dxSecondary != null);
                    dxPrimary = dxSecondary.Primary;
                    Debug.Assert(dxPrimary != null);
                    break;

            }// switch(tmaxRecord.GetMediaLevel())

            if (dxPrimary != null)
            {
                strBarcode = dxPrimary.MediaId;

                if (dxSecondary != null)
                {
                    strBarcode += ("." + dxSecondary.BarcodeId.ToString());

                    if (dxTertiary != null)
                    {
                        strBarcode += ("." + dxTertiary.BarcodeId.ToString());

                        if (dxQuaternary != null)
                        {
                            strBarcode += ("." + dxQuaternary.BarcodeId.ToString());
                        }

                    }

                }

            }

            return strBarcode;

        }// public string GetBarcode(ITmaxMediaRecord tmaxRecord, bool bIgnoreMapped)

        /// <summary>This method will get the collection of binder entries that reference the specified source record</summary> 
        /// <param name="dxRecord">The source record</param>
        /// <returns>The collection of binders sourced by the specified record</returns>
        public CDxBinderEntries GetBinderEntries(CDxMediaRecord dxSource)
        {
            CDxBinderEntries dxEntries = null;

            Debug.Assert(dxSource != null);

            try
            {
                //	Allocate the new collection
                dxEntries = new CDxBinderEntries(this);

                //	Set the source record
                dxEntries.FillFromSource = true;
                dxEntries.SourceId = GetUniqueId(dxSource);

                //	Get the entries sourced by this record and/or its children
                dxEntries.Fill();
            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetBinderEntries", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_BINDER_ENTRIES_EX, GetBarcode(dxSource, false)), Ex);
            }

            if ((dxEntries != null) && (dxEntries.Count > 0))
            {
                //	Connect the parent chains for each entry so that we can display the path
                foreach (CDxBinderEntry O in dxEntries)
                {
                    try
                    {
                        O.QueryForParents();
                    }
                    catch
                    {
                    }

                }

                return dxEntries;
            }
            else
            {
                return null;
            }

        }//	public CDxBinderEntries GetBinderEntries(CDxMediaRecord dxSource)

        /// <summary>This method will retrieve the specified binder entry record</summary>
        /// <param name="strPath">The formatted binder path descriptor</param>
        /// <returns>The associated binder record if found</returns>
        public CDxBinderEntry GetBinderFromPath(string strPath)
        {
            char[] acDelimiter = { '.' };
            CDxBinderEntry dxBinder = null;
            long lAutoId = 0;

            //	Do we have the root binders?
            if ((m_dxRootBinder == null) || (m_dxRootBinder.Contents == null) ||
                (m_dxRootBinder.Contents.Count == 0))
                return null;

            //	Is this the root binder
            if ((strPath == null) || (strPath.Length == 0) || (strPath == "0") || (strPath == "0."))
                return m_dxRootBinder;

            //	Parse the string id into individual record identifiers
            string[] aIds = strPath.Split(acDelimiter);
            if ((aIds == null) || (aIds.Length == 0)) return null;

            try
            {
                //	Start with the root binder
                dxBinder = m_dxRootBinder;

                //	Iterate the collection of record identifiers
                foreach (string strAutoId in aIds)
                {
                    lAutoId = System.Convert.ToInt64(strAutoId);

                    //	Do we need to fill the child collection?
                    if (dxBinder.Contents.Count == 0)
                    {
                        dxBinder.Fill();
                    }

                    if ((dxBinder = dxBinder.Contents.Find(lAutoId)) == null)
                    {
                        break;
                    }

                }// foreach(string strAutoId in aIds)

            }
            catch
            {
                dxBinder = null;
            }

            return dxBinder;

        }//	public CDxBinderEntry GetBinderFromPath(string strPath)

        /// <summary>This method will retrieve the specified binder entry record</summary>
        /// <param name="lId">The binder's identifier</param>
        /// <returns>The associated binder record if found</returns>
        public CDxBinderEntry GetBinderFromId(long lId)
        {
            CDxBinderEntries dxBinders = null;
            CDxBinderEntry dxBinder = null;
            string strPath = "";

            try
            {
                dxBinders = new CDxBinderEntries(this);

                //	Get the requested binder record
                dxBinders.Fill(lId);

                if (dxBinders.Count > 0)
                {
                    //	Build the parent chain
                    if (dxBinders[0].QueryForParents() == true)
                    {
                        //	Get the path to the parent
                        strPath = dxBinders[0].GetParentPathId();

                        //	Append the id for this record
                        if (strPath.Length > 0)
                            strPath += ".";
                        strPath += dxBinders[0].AutoId;

                        dxBinder = GetBinderFromPath(strPath);

                    }// if(dxBinders[0].QueryForParents() == true)

                }// if(dxBinders.Count > 0)

            }
            catch
            {
            }

            return dxBinder;

        }//	public CDxBinderEntry GetBinderFromId(long lId)

        /// <summary>This method will retrieve the active target binder</summary>
        /// <returns>The active target binder record if found</returns>
        public CDxBinderEntry GetTargetBinder()
        {
            //	Do we need to look up the target binder?
            if (m_dxTargetBinder == null)
            {
                if ((m_tmaxStationOptions != null) && (m_tmaxStationOptions.TargetBinderId > 0))
                    m_dxTargetBinder = GetBinderFromId(m_tmaxStationOptions.TargetBinderId);

                //	Should we assign the first root level binder?
                if ((m_dxTargetBinder == null) && (this.Binders != null) && (this.Binders.Count > 0))
                    SetTargetBinder(this.Binders[0]);
            }

            return m_dxTargetBinder;

        }//	public CDxBinderEntry GetTargetBinder()

        /// <summary>This method will set the active target binder</summary>
        /// <param name="lId">The id of the binder to be activated</param>
        /// <returns>The new target binder</returns>
        public CDxBinderEntry SetTargetBinder(long lId)
        {
            if (lId > 0)
                SetTargetBinder(GetBinderFromId(lId));
            else
                SetTargetBinder(null);

            return m_dxTargetBinder;

        }//	public CDxBinderEntry SetTargetBinder(long lId)

        /// <summary>This method will set the active target binder</summary>
        /// <param name="dxBinder">The binder to be activated</param>
        public void SetTargetBinder(CDxBinderEntry dxBinder)
        {
            m_dxTargetBinder = dxBinder;

            //	Update the station options
            if (m_tmaxStationOptions != null)
            {
                if (m_dxTargetBinder != null)
                    m_tmaxStationOptions.TargetBinderId = m_dxTargetBinder.AutoId;
                else
                    m_tmaxStationOptions.TargetBinderId = 0;
            }

        }//	public void SetTargetBinder(CDxBinderEntry dxBinder)

        /// <summary>This method is called to get the unique id assigned to the active case</summary>
        /// <returns>The name of the active case</returns>
        public string GetCaseId()
        {
            string strId = "";

            //	Do we have an active case?
            if (m_dxDetail != null)
                strId = m_dxDetail.UniqueId;

            return strId;

        }// public string GetCaseId()

        /// <summary>This method is called to get the name assigned to the active case</summary>
        /// <returns>The name of the active case</returns>
        public string GetCaseName()
        {
            string strName = "";

            //	Do we have an active case?
            if (m_dxDetail != null)
            {
                strName = m_dxDetail.Name;

                if ((strName.Length == 0) && (this.Folder.Length > 0))
                    strName = System.IO.Path.GetFileName(this.Folder);
            }

            return strName;

        }// public string GetCaseName()

        /// <summary>Called to get the short case name for the active database</summary>
        /// <returns>The short name assigned to the database</returns>
        public string GetShortCaseName()
        {
            string strName = "";

            try
            {
                if (m_dxDetail != null)
                    strName = m_dxDetail.ShortName;
            }
            catch
            {
            }

            return strName;

        }// public string GetShortCaseName()

        /// <summary>This methods gets the path to the case folder for the specified media type</summary>
        /// <param name="eMediaType">The TrialMax media type identifier</param>
        /// <returns>The path to the case folder</returns>
        public string GetCasePath(TmaxMediaTypes eMediaType)
        {
            CTmaxSourceFolder tmaxFolder = null;
            string strPath = "";

            //	Get the folder where this case puts files of the specified media type
            if ((tmaxFolder = GetCaseFolder(eMediaType)) != null)
            {
                strPath = tmaxFolder.Path;

                if ((strPath.Length > 0) && (strPath.EndsWith("\\") == false))
                    strPath += "\\";
            }

            return strPath.ToLower();

        }// public string GetCasePath(TmaxMediaTypes eMediaType)

        /// <summary>This methods gets the path to the case folder for the specified media record</summary>
        /// <param name="dxRecord">The desired record</param>
        /// <returns>The case path for the specified record</returns>
        public string GetCasePath(CDxMediaRecord dxRecord)
        {
            switch (dxRecord.MediaType)
            {
                case TmaxMediaTypes.Segment:

                    //	We have to use the primary type because segments
                    //	can belong to depositions or recordings
                    return GetCasePath(((CDxSecondary)dxRecord).Primary.MediaType);

                default:

                    return GetCasePath(dxRecord.MediaType);

            }// switch(dxRecord.MediaType)

        }// public string GetCasePath(CDxMediaRecord dxRecord)

        /// <summary>This method is called to get the path to the template used to export fielded data to a flat database</summary>
        /// <returns>The fully qualified path to the database template</returns>
        public string GetCodesDatabaseFileSpec()
        {
            string strFileSpec = "";

            //	The template should be stored in the application folder
            strFileSpec = m_tmaxAppOptions.ApplicationFolder;

            if ((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
                strFileSpec += "\\";
            strFileSpec += CASE_DATABASE_CODES_DATABASE_FILENAME;

            return strFileSpec;

        }// public string GetCodesDatabaseFileSpec()

        /// <summary>This method will build the fully qualified path specification for the database file using the specified folder</summary>
        /// <param name="strFolder">Folder containing the case database</param>
        /// <returns>The fully qualified path to the database file</returns>
        public string GetFileSpec(string strFolder)
        {
            string strFileSpec = "";

            if ((strFolder != null) && (strFolder.Length > 0))
            {
                strFileSpec = strFolder;
            }
            else
            {
                strFileSpec = System.Environment.CurrentDirectory;
            }

            if (strFileSpec.EndsWith("\\") == false)
                strFileSpec += "\\";

            //	Add the filename
            strFileSpec += m_strFilename;

            return strFileSpec;

        }//	public string GetFileSpec(string strFolder)

        /// <summary>This method will build the fully qualified path specification for the secondary file specified by the caller</summary>
        /// <param name="dxPrimary">The primary exchange object whos file we want</param>
        /// <param name="strFilename">The name of the secondary file</param>
        /// <returns>The fully qualified path to the secondary file</returns>
        public string GetFileSpec(CDxPrimary dxPrimary, string strFilename)
        {
            string strFileSpec = "";

            Debug.Assert(dxPrimary != null);
            Debug.Assert(strFilename != null);
            Debug.Assert(strFilename.Length > 0);

            //	Get the folder to the secondary files
            strFileSpec = GetFolderSpec(dxPrimary, false);

            //	Now add the filename
            strFileSpec += strFilename;

            return strFileSpec;

        }//	GetFileSpec()

        /// <summary>This method will build the fully qualified path specification for the primary media object specified by the caller</summary>
        /// <param name="dxPrimary">The primary exchange object whos file we want</param>
        /// <returns>The fully qualified path to the primary file</returns>
        public string GetFileSpec(CDxPrimary dxPrimary)
        {
            //	Is this a deposition?
            if (dxPrimary.MediaType == TmaxMediaTypes.Deposition)
            {
                //	Deposition segments can be linked separately from 
                //	their parent so we have to actually retrieve the 
                //	path for the first file
                if ((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
                    dxPrimary.Fill();

                //	Now use the actual information for the first segment
                if (dxPrimary.Secondaries.Count > 0)
                    return GetFileSpec(dxPrimary.Secondaries[0]);
                else
                    return "";
            }
            else
            {
                if ((dxPrimary.Filename != null) && (dxPrimary.Filename.Length > 0))
                    return GetFileSpec(dxPrimary, dxPrimary.Filename);
                else
                    return "";
            }

        }//	public string GetFileSpec(CDxPrimary dxPrimary)

        /// <summary>This method will build the fully qualified path specification for the secondary media object specified by the caller</summary>
        /// <param name="dxSecondary">The secondary exchange object whos file we want</param>
        /// <returns>The fully qualified path to the secondary file</returns>
        public string GetFileSpec(CDxSecondary dxSecondary)
        {
            string strFileSpec = "";

            Debug.Assert(dxSecondary != null);
            Debug.Assert(dxSecondary.Primary != null);
            if (dxSecondary.Primary == null) return "";

            //	What type of secondary media type are we dealing with?
            switch (dxSecondary.MediaType)
            {
                case TmaxMediaTypes.Page:

                    //	Combine the primary folder and secondary filename
                    strFileSpec = GetFolderSpec(dxSecondary.Primary, false);
                    strFileSpec += dxSecondary.Filename;
                    break;

                case TmaxMediaTypes.Slide:

                    //	Should we get the exported slide image?
                    if (dxSecondary.Filename.Length > 0)
                    {
                        strFileSpec = GetSlidesFolderSpec(dxSecondary.Primary);
                        strFileSpec += dxSecondary.Filename;
                    }
                    else
                    {
                        //	Get the spec for the actual PowerPoint presentation
                        strFileSpec = GetFolderSpec(dxSecondary.Primary, false);
                        strFileSpec += dxSecondary.Primary.Filename;
                    }
                    break;

                case TmaxMediaTypes.Segment:

                    //	Is this a recording segment?
                    if (dxSecondary.Primary.MediaType == TmaxMediaTypes.Recording)
                    {
                        //	Combine the primary folder and secondary filename
                        strFileSpec = GetFolderSpec(dxSecondary.Primary, false);
                        strFileSpec += dxSecondary.Filename;
                    }
                    else
                    {
                        //	Has the user specified a path to this video?
                        if (dxSecondary.AliasId > 0)
                        {
                            //	Combine the aliased folder and secondary filename
                            strFileSpec = GetAliasedPath(dxSecondary);
                            strFileSpec += dxSecondary.Filename;
                        }
                        else
                        {
                            //	Does this segment have it's own relative path specification?
                            if (dxSecondary.RelativePath.Length > 0)
                            {
                                strFileSpec = GetCasePath(TmaxMediaTypes.Deposition);
                                strFileSpec += dxSecondary.RelativePath;
                                if (strFileSpec.EndsWith("\\") == false)
                                    strFileSpec += "\\";
                                strFileSpec += dxSecondary.Filename;
                            }
                            else
                            {
                                //	Combine the primary folder and secondary filename
                                strFileSpec = GetFolderSpec(dxSecondary.Primary, false);
                                strFileSpec += dxSecondary.Filename;
                            }

                        }

                    }
                    break;

                case TmaxMediaTypes.Scene:

                    //	Get the file specification for the source record
                    if (dxSecondary.GetSource() != null)
                        strFileSpec = GetFileSpec(dxSecondary.GetSource());

                    break;
            }

            return strFileSpec;

        }//	public string GetFileSpec(CDxSecondary dxSecondary)

        /// <summary>This method will build the fully qualified path specification for the tertiary media object specified by the caller</summary>
        /// <param name="dxTertiary">The tertiary exchange object whos file we want</param>
        /// <returns>The fully qualified path to the tertiary file</returns>
        public string GetFileSpec(CDxTertiary dxTertiary)
        {
            //	Are we using the paths prior to version 6.0.1?
            if (m_bRequires601Update == true)
                return GetFileSpec600(dxTertiary);
            else
                return GetFileSpec601(dxTertiary);

        }//	public string GetFileSpec(CDxTertiary dxTertiary)

        /// <summary>This method will build the fully qualified path specification for the quaternary media object specified by the caller</summary>
        /// <param name="dxQuaternary">The quaternary exchange object whos file we want</param>
        /// <returns>The fully qualified path to the quaternary file</returns>
        public string GetFileSpec(CDxQuaternary dxQuaternary)
        {
            string strFileSpec = "";

            Debug.Assert(dxQuaternary != null);
            if (dxQuaternary == null) return "";

            //	What type of secondary media type are we dealing with?
            switch (dxQuaternary.MediaType)
            {
                case TmaxMediaTypes.Link:

                    //	Get the file specification for the source record
                    if ((dxQuaternary.HideLink == false) && (dxQuaternary.GetSource() != null))
                        strFileSpec = GetFileSpec(dxQuaternary.GetSource());
                    break;

            }

            return strFileSpec;

        }//	public string GetFileSpec(CDxQuaternary dxQuaternary)

        /// <summary>This method will build the fully qualified path specification for the TrialMax event item specified by the caller</summary>
        /// <param name="tmaxItem">The desired TrialMax event item</param>
        /// <returns>The fully qualified path to the item's media file</returns>
        public string GetFileSpec(CTmaxItem tmaxItem)
        {
            Debug.Assert(tmaxItem != null);

            if (tmaxItem.GetMediaRecord() != null)
                return GetFileSpec((CDxMediaRecord)tmaxItem.GetMediaRecord());
            else
                return "";

        }//	public string GetFileSpec(CTmaxItem tmaxItem)

        /// <summary>This method will build the fully qualified path specification for the transcript object specified by the caller</summary>
        /// <param name="dxTranscript">The transcript exchange object whos file we want</param>
        /// <returns>The fully qualified path to the transcript file</returns>
        public string GetFileSpec(CDxTranscript dxTranscript)
        {
            string strFileSpec = "";

            Debug.Assert(dxTranscript != null);
            Debug.Assert(dxTranscript.Filename != null);
            Debug.Assert(dxTranscript.Filename.Length > 0);
            Debug.Assert(dxTranscript.Primary != null);

            if (dxTranscript.Filename == null) return "";
            if (dxTranscript.Filename.Length == 0) return "";
            if (dxTranscript.Primary == null) return "";

            //	Get the folder associated with the owner primary record
            strFileSpec = GetTranscriptFolder(dxTranscript);

            //	Now add the filename
            strFileSpec += dxTranscript.Filename;

            return strFileSpec;

        }//	public string GetFileSpec(CDxTranscript dxTranscript)

        /// <summary>This method will build the fully qualified path specification for the TrialMax record exchange object</summary>
        /// <param name="dxRecord">The record exchange objct</param>
        /// <returns>The fully qualified path to the record's media file</returns>
        public string GetFileSpec(CDxMediaRecord dxRecord)
        {
            string strFileSpec = "";

            Debug.Assert(dxRecord != null);

            //	What type of record is this?
            switch (dxRecord.GetMediaLevel())
            {
                case TmaxMediaLevels.Primary:

                    strFileSpec = GetFileSpec((CDxPrimary)dxRecord);
                    break;

                case TmaxMediaLevels.Secondary:

                    strFileSpec = GetFileSpec((CDxSecondary)dxRecord);
                    break;

                case TmaxMediaLevels.Tertiary:

                    strFileSpec = GetFileSpec((CDxTertiary)dxRecord);
                    break;

                case TmaxMediaLevels.Quaternary:

                    strFileSpec = GetFileSpec((CDxQuaternary)dxRecord);
                    break;

            }

            return strFileSpec;

        }//	public string GetFileSpec(CDxMediaRecord dxRecord)

        /// <summary>This method will build the fully qualified path for specified tertiary record using the file structure up to ver 6.0.0</summary>
        /// <param name="dxTertiary">The desired tertiary record</param>
        /// <returns>The fully qualified path to the tertiary file</returns>
        public string GetFileSpec600(CDxTertiary dxTertiary)
        {
            string strFileSpec = "";

            Debug.Assert(dxTertiary != null);
            if (dxTertiary == null) return "";
            Debug.Assert(dxTertiary.Secondary != null);
            if (dxTertiary.Secondary == null) return "";

            //	Get the folder where the file should be stored
            strFileSpec = GetFolderSpec600(dxTertiary);

            //	Has the filename already been stored?
            if (dxTertiary.Filename != null && (dxTertiary.Filename.Length > 0))
            {
                strFileSpec += dxTertiary.Filename;

                //	Since the Filename field has been set, the file should exist
                if (System.IO.File.Exists(strFileSpec) == false)
                {
                    //	Check to see if it exists in its new location. It's possible
                    //	the validation process was interrupted or an error occurred
                    string strNewLocation = GetFileSpec601(dxTertiary);
                    if ((strNewLocation.Length > 0) && (System.IO.File.Exists(strNewLocation) == true))
                        strFileSpec = strNewLocation;
                }

            }
            else
            {
                if (dxTertiary.MediaType == TmaxMediaTypes.Treatment)
                    strFileSpec += ("Z_" + dxTertiary.Secondary.AutoId.ToString() + "_" + dxTertiary.AutoId.ToString() + ".zap");
                else
                    strFileSpec += ("D_" + dxTertiary.Secondary.AutoId.ToString() + "_" + dxTertiary.AutoId.ToString() + "." + CXmlDesignation.GetExtension());

            }// if(dxTertiary.Filename != null && (dxTertiary.Filename.Length > 0))

            return strFileSpec;

        }//	public string GetFileSpec600(CDxTertiary dxTertiary)

        /// <summary>This method will build the fully qualified path for specified tertiary record using the file structure starting with ver 6.0.0</summary>
        /// <param name="dxTertiary">The desired tertiary record</param>
        /// <returns>The fully qualified path to the tertiary file</returns>
        public string GetFileSpec601(CDxTertiary dxTertiary)
        {
            string strFileSpec = "";

            Debug.Assert(dxTertiary != null);
            Debug.Assert(dxTertiary.Secondary != null);
            Debug.Assert(dxTertiary.Secondary.Primary != null);

            if (dxTertiary.Secondary == null) return "";
            if (dxTertiary.Secondary.Primary == null) return "";

            //	Get the folder where the file should be stored
            strFileSpec = GetFolderSpec601(dxTertiary);

            //	Has the filename already been stored?
            if (dxTertiary.Filename != null && (dxTertiary.Filename.Length > 0))
            {
                strFileSpec += dxTertiary.Filename;
            }
            else
            {
                //	What type of media is this
                switch (dxTertiary.MediaType)
                {
                    case TmaxMediaTypes.Treatment:

                        //	Use the PST values to construct a default filename
                        strFileSpec += ("Z_" + dxTertiary.Secondary.Primary.AutoId.ToString() + "_");
                        strFileSpec += (dxTertiary.Secondary.AutoId.ToString() + "_");
                        strFileSpec += (dxTertiary.AutoId.ToString() + ".zap");
                        break;

                    case TmaxMediaTypes.Designation:

                        strFileSpec += ("D_" + dxTertiary.Secondary.Primary.AutoId.ToString() + "_");
                        strFileSpec += (dxTertiary.Secondary.AutoId.ToString() + "_");
                        strFileSpec += (dxTertiary.AutoId.ToString() + "." + CXmlDesignation.GetExtension());
                        break;

                    case TmaxMediaTypes.Clip:

                        strFileSpec += ("C_" + dxTertiary.Secondary.Primary.AutoId.ToString() + "_");
                        strFileSpec += (dxTertiary.Secondary.AutoId.ToString() + "_");
                        strFileSpec += (dxTertiary.AutoId.ToString() + "." + CXmlDesignation.GetExtension());
                        break;

                    default:

                        return "";

                }// switch(dxTertiary.MediaType)

            }// if(dxTertiary.Filename != null && (dxTertiary.Filename.Length > 0))

            return strFileSpec;

        }//	public string GetFileSpec601(CDxTertiary dxTertiary)

        /// <summary>This method is called to get the current and default paths for the folder containing the files associated with the specified record</summary>
        /// <param name="dxRecord">The primary/secondary record that defines the media group</param>
        /// <param name="rCurrent">Reference to location to store current path</param>
        /// <param name="rDefault">Reference to location to store default path</param>
        /// <returns>true if successful</returns>
        public bool GetFolderPaths(CDxMediaRecord dxRecord, ref string rCurrent, ref string rDefault)
        {
            string strCurrent = "";
            string strDefault = "";
            CDxSecondary dxSecondary = null;

            //	Is this a primary media record?
            if (dxRecord.GetMediaLevel() == TmaxMediaLevels.Primary)
            {
                strCurrent = GetFolderSpec((CDxPrimary)dxRecord, false);
                strDefault = GetCasePath(dxRecord.MediaType);
            }
            else if (dxRecord.GetMediaLevel() == TmaxMediaLevels.Secondary)
            {
                dxSecondary = (CDxSecondary)dxRecord;

                //	Can only link deposition segments
                if (dxSecondary.Primary.MediaType != TmaxMediaTypes.Deposition)
                    return false;

                //	The default for a deposition segment is to use the parent deposition' folder
                strDefault = GetCasePath(TmaxMediaTypes.Deposition);

                //	Does the segment specify it's own link?
                if (dxSecondary.AliasId > 0)
                {
                    strCurrent = GetAliasedPath(dxSecondary);
                }
                else
                {
                    //	Does this segment have it's own relative path
                    if (dxSecondary.RelativePath.Length > 0)
                    {
                        strCurrent = GetCasePath(TmaxMediaTypes.Deposition);
                        strCurrent += dxSecondary.RelativePath;
                    }
                    else
                    {
                        //	Use the current primary path
                        strCurrent = GetFolderSpec(dxSecondary.Primary, false);
                    }

                }

            }
            else
            {
                //	Must be primary or secondary
                return false;
            }

            if (strDefault.Length > 0)
            {
                if (strDefault.Length == 1)
                    strDefault += ":\\";
                else if (strDefault.EndsWith("\\") == false)
                    strDefault += "\\";
            }

            if (strCurrent.Length > 0)
            {
                if (strCurrent.Length == 1)
                    strCurrent += ":\\";
                else if (strCurrent.EndsWith("\\") == false)
                    strCurrent += "\\";
            }

            //	Set the caller's values
            if (rCurrent != null)
                rCurrent = strCurrent;
            if (rDefault != null)
                rDefault = strDefault;

            return true;

        }// public bool GetFolderPaths(CDxMediaRecord dxRecord, ref string rCurrent, ref string rDefault)

        /// <summary>This method will build the fully qualified path specification for the primary folder containing the secondary files</summary>
        /// <param name="dxPrimary">The desired primary exchange object</param>
        /// <param name="bIgnoreLinked">True to ignore the record's linked attribute</param>
        /// <returns>The fully qualified path to the primary folder</returns>
        public string GetFolderSpec(CDxPrimary dxPrimary, bool bIgnoreLinked)
        {
            string strFolder = "";

            Debug.Assert(dxPrimary != null);

            //	Is this media object linked?
            if ((dxPrimary.Linked == true) && (bIgnoreLinked == false))
            {
                Debug.Assert(m_tmaxCaseOptions != null);
                Debug.Assert(m_tmaxCaseOptions.Aliases != null);

                strFolder = GetAliasedPath(dxPrimary);
            }
            else
            {
                strFolder = GetCasePath(dxPrimary.MediaType);

                if (dxPrimary.RelativePath.Length > 0)
                    strFolder += dxPrimary.RelativePath;
            }

            //	Make sure we have the trailing backslash
            if (strFolder.Length > 0)
            {
                if (strFolder.EndsWith("\\") == false)
                    strFolder += "\\";
            }

            return strFolder;

        }//	public string GetFolderSpec(CDxPrimary dxPrimary, bool bIgnoreLinked)

        /// <summary>This method will build the fully qualified path specification to the folder that contains the secondary file</summary>
        /// <param name="dxSecondary">The desired secondary exchange object</param>
        /// <param name="bIgnoreLinked">True to ignore the record's linked attribute</param>
        /// <returns>The fully qualified path to the secondary folder</returns>
        public string GetFolderSpec(CDxSecondary dxSecondary, bool bIgnoreLinked)
        {
            string strFolder = "";

            Debug.Assert(dxSecondary != null);
            Debug.Assert(dxSecondary.Primary != null);
            if ((dxSecondary == null) || (dxSecondary.Primary == null)) return "";

            //	Is this a slide?
            if (dxSecondary.MediaType == TmaxMediaTypes.Slide)
            {
                strFolder = GetCasePath(TmaxMediaTypes.Slide);
                if (strFolder.EndsWith("\\") == false)
                    strFolder += "\\";

                //	Add the primary id
                strFolder += (dxSecondary.Primary.AutoId.ToString() + "\\");

                return strFolder;
            }
            else
            {
                //	The other secondary types store their files in the primary folder
                return GetFolderSpec(dxSecondary.Primary, bIgnoreLinked);
            }

        }//	public string GetFolderSpec(CDxSecondary dxSecondary, bool bIgnoreLinked)

        /// <summary>This method will build the fully qualified path specification to the folder that contains the tertiary file</summary>
        /// <param name="dxSecondary">The desired tertiary exchange object</param>
        /// <returns>The fully qualified path to the tertiary folder</returns>
        public string GetFolderSpec(CDxTertiary dxTertiary)
        {
            if (m_bRequires601Update == true)
                return GetFolderSpec600(dxTertiary);
            else
                return GetFolderSpec601(dxTertiary);

        }//	public string GetFolderSpec(CDxTertiary dxTertiary)

        /// <summary>This method will build the fully qualified path for folder containing the file for this tertiary record</summary>
        /// <param name="dxTertiary">The desired tertiary record</param>
        /// <returns>The fully qualified path to the parent folder</returns>
        public string GetFolderSpec600(CDxTertiary dxTertiary)
        {
            string strFolderSpec = "";

            Debug.Assert(dxTertiary != null);
            if (dxTertiary == null) return "";
            Debug.Assert(dxTertiary.Secondary != null);
            if (dxTertiary.Secondary == null) return "";

            //	Get the root folder for this media type
            if (dxTertiary.MediaType == TmaxMediaTypes.Designation)
            {
                //	We have to ask for clips instead of designations because
                //	in ver 6.0.1 the GetMediaPath() method has been modified to
                //	put designations under their parent deposition (transcript)
                strFolderSpec = GetCasePath(TmaxMediaTypes.Clip);
            }
            else
            {
                //	Ver 6.0.1 still uses the same root for all other media types
                strFolderSpec = GetCasePath(dxTertiary.MediaType);
            }

            if ((strFolderSpec.Length > 0) && (strFolderSpec.EndsWith("\\") == false))
                strFolderSpec += "\\";

            return strFolderSpec;

        }//	public string GetFolderSpec600(CDxTertiary dxTertiary)

        /// <summary>This method will build the fully qualified path of the folder for the specified tertiary record using the file structure starting with ver 6.0.0</summary>
        /// <param name="dxTertiary">The desired tertiary record</param>
        /// <returns>The fully qualified path to the tertiary folder</returns>
        public string GetFolderSpec601(CDxTertiary dxTertiary)
        {
            string strFolderSpec = "";

            Debug.Assert(dxTertiary != null);
            Debug.Assert(dxTertiary.Secondary != null);
            Debug.Assert(dxTertiary.Secondary.Primary != null);

            if (dxTertiary.Secondary == null) return "";
            if (dxTertiary.Secondary.Primary == null) return "";

            //	Get the case folder for this type of media
            strFolderSpec = GetCasePath(dxTertiary.MediaType);
            if ((strFolderSpec.Length > 0) && (strFolderSpec.EndsWith("\\") == false))
                strFolderSpec += "\\";

            //	What type of media is this
            switch (dxTertiary.MediaType)
            {
                case TmaxMediaTypes.Treatment:

                    //	Add a two-level subfolder \PrimaryId\SourceFilename
                    strFolderSpec += (dxTertiary.Secondary.Primary.AutoId.ToString() + "\\");
                    strFolderSpec += (System.IO.Path.GetFileNameWithoutExtension(dxTertiary.Secondary.Filename) + "\\");
                    break;

                case TmaxMediaTypes.Designation:

                    //	Add a subfolder using the primary id of the parent deposition
                    //
                    //	NOTE:	We don't bother with a second-level subfolder because segments are
                    //			really just artifical boundries in a transcript and the liklihood
                    //			of deleting an individual segment is pretty slim
                    strFolderSpec += (dxTertiary.Secondary.Primary.AutoId.ToString() + "\\");
                    break;

                case TmaxMediaTypes.Clip:

                    //	Add a two-level subfolder \PrimaryId\SourceFilename
                    strFolderSpec += (dxTertiary.Secondary.Primary.AutoId.ToString() + "\\");
                    strFolderSpec += (System.IO.Path.GetFileNameWithoutExtension(dxTertiary.Secondary.Filename) + "\\");
                    break;

                default:

                    return "";

            }// switch(dxTertiary.MediaType)

            return strFolderSpec;

        }//	public string GetFolderSpec601(CDxTertiary dxTertiary)

        /// <summary>This method is called to convert the individual version identifiers into a single value appropriate for comparison</summary>
        /// <param name="iMajor">The major version identifier</param>
        /// <param name="iMinor">The minor version identifier</param>
        /// <param name="iQEF">The QEF version identifier</param>
        /// <returns>The packed version identifier</returns>
        public long GetPackedVer(int iMajor, int iMinor, int iQEF)
        {
            return CBaseVersion.GetPackedVersion(iMajor, iMinor, iQEF);
        }

        /// <summary>This method is called to get a packed version identifier for the assembly or active database</summary>
        /// <param name="bAssembly">True to get the assembly version</param>
        /// <returns>The packed version identifier</returns>
        public long GetPackedVer(bool bAssembly)
        {
            int iMajor = 0;
            int iMinor = 0;
            int iQEF = 0;

            if (bAssembly == true)
            {
                CTmdataVersion tmdataVer = new CTmdataVersion();

                iMajor = tmdataVer.Major;
                iMinor = tmdataVer.Minor;
                iQEF = tmdataVer.QEF;
            }
            else
            {
                //	Use the version information stored in the database
                if (this.Detail != null)
                {
                    iMajor = (int)(this.Detail.TmaxCase.VerMajor);
                    iMinor = (int)(this.Detail.TmaxCase.VerMinor);
                    iQEF = (int)(this.Detail.TmaxCase.VerQEF);
                }

            }// if(bAssembly == true)

            return CBaseVersion.GetPackedVersion(iMajor, iMinor, iQEF);

        }// public long GetPackedVer(bool bAssembly)

        /// <summary>Called to get the collection of zap files (treatments) created in TmaxPresentation</summary>
        /// <returns>An array of pending treatments</returns>
        public ArrayList GetZapFiles()
        {
            string strSearchPath = "";
            ArrayList aFileSpecs = new ArrayList();

            //	Construct the search path
            strSearchPath = m_strFolder;
            if (strSearchPath.EndsWith("\\") == false)
                strSearchPath += "\\";
            strSearchPath += "_tmax_presentation\\treatments\\";

            try
            {
                //	Make sure the folder exists
                if (System.IO.Directory.Exists(strSearchPath) == true)
                {
                    //	Get the collection of files
                    string[] aFiles = System.IO.Directory.GetFiles(strSearchPath, "*.zap_");
                    foreach (string O in aFiles)
                    {
                        aFileSpecs.Add(O);
                    }
                }
            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetZapFiles", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_ZAP_FILES_EX, strSearchPath), Ex);
            }

            return aFileSpecs;

        }// public ArrayList GetZapFiles()

        /// <summary>This method is called to get the id of the record property with the specified name</summary>
        /// <param name="strName">The property name</param>
        /// <returns>The associated identifier</returns>
        /// <remarks>Property identifiers are unique across media record classes</remarks>
        public int GetPropertyId(string strName)
        {
            CDxPrimary dxPrimary = new CDxPrimary();
            CDxSecondary dxSecondary = null;
            CDxTertiary dxTertiary = null;
            CDxQuaternary dxQuaternary = null;
            int iId = -1;

            //	Is this a primary or base class identifier?
            if ((iId = dxPrimary.GetPropertyId(strName)) <= 0)
            {
                //	Check to see if it's a secondary property
                dxSecondary = new CDxSecondary();
                if ((iId = dxSecondary.GetPropertyId(strName)) <= 0)
                {
                    //	Check to see if it's tertiary
                    dxTertiary = new CDxTertiary();
                    if ((iId = dxTertiary.GetPropertyId(strName)) <= 0)
                    {
                        //	Must be quaternary
                        dxQuaternary = new CDxQuaternary();
                        iId = dxQuaternary.GetPropertyId(strName);
                    }

                }

            }

            return iId;

        }// public int GetPropertyId(string strName)

        /// <summary>This method will retrieve the record exchange object associated with the specified barcode</summary>
        /// <param name="strBarcode">The unique barcode MediaId.S.T.Q</param>
        /// <param name="bSilent">true to inhibit error events</param>
        /// <param name="bIgnoreMapped">The true to ignore the record's foreign barcode if mapped</param>
        /// <returns>The associated record exchange object</returns>
        public CDxMediaRecord GetRecordFromBarcode(string strBarcode, bool bSilent, bool bIgnoreMapped)
        {
            char[] acDelimiter = { '.' };
            CDxPrimary dxPrimary = null;
            CDxSecondary dxSecondary = null;
            CDxTertiary dxTertiary = null;
            CDxQuaternary dxQuaternary = null;
            CDxBarcode dxBarcode = null;
            long lBarcodeId = 0;
            bool bInvalid = false;

            Debug.Assert(strBarcode != null);
            Debug.Assert(strBarcode.Length != 0);
            if ((strBarcode == null) || (strBarcode.Length == 0)) return null;

            //	Should we look for a foreign barcode first?
            if ((bIgnoreMapped == false) && (m_tmaxAppOptions.ShowForeignBarcodes == true))
            {
                if ((dxBarcode = m_dxBarcodeMap.FindForeign(strBarcode)) != null)
                {
                    if (dxBarcode.Source != null)
                        return dxBarcode.Source;
                }

            }// if((bIgnoreMapped == false) && (m_tmaxAppOptions.ShowForeignBarcodes == true))

            //	Parse the string id into individual record identifiers
            string[] aIds = strBarcode.Split(acDelimiter);
            if ((aIds == null) || (aIds.Length == 0)) return null;

            try
            {
                foreach (string strBarcodeId in aIds)
                {
                    //	Are we looking for the primary record?
                    if (dxPrimary == null)
                    {
                        //	Locate by media id
                        if ((dxPrimary = m_dxPrimaries.Find(strBarcodeId)) == null)
                        {
                            bInvalid = true;
                            break;
                        }
                    }

                        //	Secondary record?
                    else if (dxSecondary == null)
                    {
                        lBarcodeId = System.Convert.ToInt64(strBarcodeId);

                        //	Do we need to fill the primary collection
                        if (dxPrimary.Secondaries.Count == 0)
                            dxPrimary.Fill();

                        if ((dxSecondary = dxPrimary.Secondaries.Find(lBarcodeId, true)) == null)
                        {
                            bInvalid = true;
                            break;
                        }

                    }

                        //	Tertiary record?
                    else if (dxTertiary == null)
                    {
                        lBarcodeId = System.Convert.ToInt64(strBarcodeId);

                        //	Do we need to fill the secondary collection
                        if (dxSecondary.Tertiaries.Count == 0)
                            dxSecondary.Fill();

                        if ((dxTertiary = dxSecondary.Tertiaries.Find(lBarcodeId, true)) == null)
                        {
                            bInvalid = true;
                            break;
                        }

                    }

                        //	Must be looking for Quaternary
                    else
                    {
                        lBarcodeId = System.Convert.ToInt64(strBarcodeId);

                        //	Do we need to fill the tertiary collection
                        if (dxTertiary.Quaternaries.Count == 0)
                            dxTertiary.Fill();

                        if ((dxQuaternary = dxTertiary.Quaternaries.Find(lBarcodeId, true)) == null)
                        {
                            bInvalid = true;
                        }

                        //	Can't drill down any further
                        break;
                    }


                }// foreach(string strAutoId in aIds)

            }
            catch (System.Exception Ex)
            {
                if (bSilent == false)
                    FireError(this, "GetRecordFromBarcode", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_RECORD_FROM_BARCODE_EX, strBarcode), Ex);
                return null;
            }

            //	Did the caller provide an invalid id?
            if (bInvalid == true)
            {
                if (bSilent == false)
                    FireError(this, "GetRecordFromBarcode", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_RECORD_FROM_BARCODE_FAILED, strBarcode));
                return null;
            }
            else
            {
                //	Return the lowest level record
                if (dxQuaternary != null)
                    return dxQuaternary;
                else if (dxTertiary != null)
                    return dxTertiary;
                else if (dxSecondary != null)
                    return dxSecondary;
                else
                    return dxPrimary;

            }// if(bInvalid == true)

        }//	public CDxMediaRecord GetRecordFromBarcode(string strBarcode)

        /// <summary>This method will retrieve the record exchange object associated with the specified P.S.T.Q id string</summary>
        /// <param name="strId">The unique record identifier with P.S.T.Q format</param>
        /// <param name="bSilent">true to inhibit error events</param>
        /// <returns>The associated record exchange object</returns>
        public CDxMediaRecord GetRecordFromId(string strId, bool bSilent)
        {
            char[] acDelimiter = { '.' };
            CDxPrimary dxPrimary = null;
            CDxSecondary dxSecondary = null;
            CDxTertiary dxTertiary = null;
            CDxQuaternary dxQuaternary = null;
            long lAutoId = 0;
            bool bInvalid = false;

            Debug.Assert(strId != null);
            Debug.Assert(strId.Length != 0);
            if ((strId == null) || (strId.Length == 0)) return null;

            //	Parse the string id into individual record identifiers
            string[] aIds = strId.Split(acDelimiter);
            if ((aIds == null) || (aIds.Length == 0)) return null;

            try
            {
                foreach (string strAutoId in aIds)
                {
                    lAutoId = System.Convert.ToInt64(strAutoId);

                    //	Are we looking for the primary record?
                    if (dxPrimary == null)
                    {
                        if ((dxPrimary = m_dxPrimaries.Find(lAutoId, false)) == null)
                        {
                            bInvalid = true;
                            break;
                        }
                    }

                        //	Secondary record?
                    else if (dxSecondary == null)
                    {
                        //	Do we need to fill the primary collection
                        if (dxPrimary.Secondaries.Count == 0)
                            dxPrimary.Fill();

                        if ((dxSecondary = dxPrimary.Secondaries.Find(lAutoId, false)) == null)
                        {
                            bInvalid = true;
                            break;
                        }

                    }

                        //	Tertiary record?
                    else if (dxTertiary == null)
                    {
                        //	Do we need to fill the secondary collection
                        if (dxSecondary.Tertiaries.Count == 0)
                            dxSecondary.Fill();

                        if ((dxTertiary = dxSecondary.Tertiaries.Find(lAutoId, false)) == null)
                        {
                            bInvalid = true;
                            break;
                        }

                    }

                        //	Must be looking for Quaternary
                    else
                    {
                        //	Do we need to fill the tertiary collection
                        if (dxTertiary.Quaternaries.Count == 0)
                            dxTertiary.Fill();

                        if ((dxQuaternary = dxTertiary.Quaternaries.Find(lAutoId, false)) == null)
                        {
                            bInvalid = true;
                        }

                        //	Can't drill down any further
                        break;
                    }


                }// foreach(string strAutoId in aIds)

            }
            catch (System.Exception Ex)
            {
                if (bSilent == false)
                    FireError(this, "GetRecordFromId", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_RECORD_FROM_ID_EX, strId), Ex);
                return null;
            }

            //	Did the caller provide an invalid id?
            if (bInvalid == true)
            {
                if (bSilent == false)
                    FireError(this, "GetRecordFromId", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_RECORD_FROM_ID_FAILED, strId));
                return null;
            }
            else
            {
                //	Return the lowest level record
                if (dxQuaternary != null)
                    return dxQuaternary;
                else if (dxTertiary != null)
                    return dxTertiary;
                else if (dxSecondary != null)
                    return dxSecondary;
                else
                    return dxPrimary;

            }// if(bInvalid == true)

        }//	public CDxMediaRecord GetRecordFromId(string strId)

        /// <summary>This method will retrieve the record exchange object associated with the specified event item</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <returns>The associated record exchange object</returns>
        public CDxMediaRecord GetRecordFromItem(CTmaxItem tmaxItem)
        {
            CDxMediaRecord dxRecord = null;

            Debug.Assert(tmaxItem != null);

            if (tmaxItem != null)
                dxRecord = (CDxMediaRecord)tmaxItem.GetRecord();

            return dxRecord;

        }//	public CDxMediaRecord GetRecordFromItem(CTmaxItem tmaxItem)

        /// <summary>This method will retrieve the record exchange object associated with the specified barcode</summary>
        /// <param name="strBarcodes">The delimited string of barcodes</param>
        /// <param name="dxRecords">The collection in which to store the associated records</param>
        ///	<param name="aInvalid">The collection in which to store barcodes that were found to be invalid</param>
        /// <param name="strDelimiters">The delimiters used to parse the barcode strings</param>
        /// <param name="bSilent">true to inhibit error events when invalid barcodes are encountered</param>
        /// <param name="bIgnoreMapped">true to ignore entries in the foreign barcode map</param>
        /// <returns>The number of records added to the collection</returns>
        public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid, string strDelimiters, bool bSilent, bool bIgnoreMapped)
        {
            int iStartCount = 0;
            CDxMediaRecord dxRecord = null;

            Debug.Assert(dxRecords != null);
            if (dxRecords == null) return 0;

            Debug.Assert(strBarcodes != null);
            Debug.Assert(strBarcodes.Length != 0);
            if ((strBarcodes == null) || (strBarcodes.Length == 0)) return 0;

            Debug.Assert(strDelimiters != null);
            Debug.Assert(strDelimiters.Length != 0);
            if ((strDelimiters == null) || (strDelimiters.Length == 0)) return 0;

            //	How many records are in the collection right now?
            iStartCount = dxRecords.Count;

            try
            {
                //	Parse the string id into individual record identifiers
                string[] aBarcodes = strBarcodes.Split(strDelimiters.ToCharArray());
                if ((aBarcodes == null) || (aBarcodes.Length == 0)) return 0;

                foreach (string O in aBarcodes)
                {
                    //	Get the record for this barcode
                    if ((dxRecord = GetRecordFromBarcode(O, bSilent, bIgnoreMapped)) != null)
                    {
                        //	Add to the caller's collection
                        dxRecords.AddList(dxRecord);
                    }
                    else
                    {
                        //	Add this barcode to the collection of invalid barcodes 
                        //	provided by the caller
                        if (aInvalid != null)
                            aInvalid.Add(O);
                    }

                }// foreach(string strAutoId in aIds)

            }
            catch (System.Exception Ex)
            {
                if (bSilent == false)
                    FireError(this, "GetRecordsFromBarcodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_RECORDS_FROM_BARCODES_EX, strBarcodes), Ex);
            }

            //	Return the number of records added to the collection
            return (dxRecords.Count - iStartCount);

        }//	public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid, string strDelimiters, bool bSilent)

        /// <summary>This method will retrieve the record exchange object associated with the specified barcode</summary>
        /// <param name="strBarcodes">The delimited string of barcodes</param>
        /// <param name="dxRecords">The collection in which to store the associated records</param>
        /// <param name="strDelimiters">The delimiters used to parse the barcode strings</param>
        /// <param name="bSilent">true to inhibit error events when invalid barcodes are encountered</param>
        /// <param name="bIgnoreMapped">true to ignore entries in the foreign barcode map</param>
        /// <returns>The number of records added to the collection</returns>
        public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, string strDelimiters, bool bSilent, bool bIgnoreMapped)
        {
            return GetRecordsFromBarcodes(strBarcodes, dxRecords, null, strDelimiters, bSilent, bIgnoreMapped);

        }//	public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, string strDelimiters, bool bSilent)

        /// <summary>This method will retrieve the record exchange object associated with the specified barcode</summary>
        /// <param name="strBarcodes">The delimited string of barcodes</param>
        /// <param name="dxRecords">The collection in which to store the associated records</param>
        ///	<param name="aInvalid">The collection in which to store barcodes that were found to be invalid</param>
        /// <param name="bSilent">true to inhibit error events when invalid barcodes are encountered</param>
        /// <param name="bIgnoreMapped">true to ignore entries in the foreign barcode map</param>
        /// <returns>The number of records added to the collection</returns>
        public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid, bool bSilent, bool bIgnoreMapped)
        {
            return GetRecordsFromBarcodes(strBarcodes, dxRecords, aInvalid, ",", bSilent, bIgnoreMapped);

        }//	public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid, string strDelimiters, bool bSilent)

        /// <summary>This method will retrieve the record exchange object associated with the specified barcode</summary>
        /// <param name="strBarcodes">The delimited string of barcodes</param>
        /// <param name="dxRecords">The collection in which to store the associated records</param>
        /// <param name="bIgnoreMapped">True to ignore entries in the barcode map</param>
        /// <param name="bSilent">true to inhibit error events when invalid barcodes are encountered</param>
        /// <param name="bIgnoreMapped">true to ignore entries in the foreign barcode map</param>
        /// <returns>The number of records added to the collection</returns>
        public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, bool bSilent, bool bIgnoreMapped)
        {
            return GetRecordsFromBarcodes(strBarcodes, dxRecords, null, ",", bSilent, bIgnoreMapped);

        }//	public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, bool bSilent)

        /// <summary>This method will retrieve the record exchange object associated with the specified barcode</summary>
        /// <param name="strBarcodes">The delimited string of barcodes</param>
        /// <param name="dxRecords">The collection in which to store the associated records</param>
        ///	<param name="aInvalid">The collection in which to store barcodes that were found to be invalid</param>
        /// <param name="strDelimiters">The delimiters used to parse the barcode strings</param>
        /// <param name="bIgnoreMapped">true to ignore entries in the foreign barcode map</param>
        /// <returns>The number of records added to the collection</returns>
        public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid, string strDelimiters, bool bIgnoreMapped)
        {
            return GetRecordsFromBarcodes(strBarcodes, dxRecords, aInvalid, strDelimiters, false, bIgnoreMapped);

        }//	public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid, string strDelimiters)

        /// <summary>This method will retrieve the record exchange object associated with the specified barcode</summary>
        /// <param name="strBarcodes">The delimited string of barcodes</param>
        /// <param name="dxRecords">The collection in which to store the associated records</param>
        ///	<param name="aInvalid">The collection in which to store barcodes that were found to be invalid</param>
        /// <param name="bIgnoreMapped">true to ignore entries in the foreign barcode map</param>
        /// <returns>The number of records added to the collection</returns>
        public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid, bool bIgnoreMapped)
        {
            return GetRecordsFromBarcodes(strBarcodes, dxRecords, aInvalid, ",", false, bIgnoreMapped);

        }//	public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid)

        /// <summary>This method will retrieve the record exchange object associated with the specified barcode</summary>
        /// <param name="strBarcodes">The delimited string of barcodes</param>
        /// <param name="dxRecords">The collection in which to store the associated records</param>
        /// <param name="bIgnoreMapped">true to ignore entries in the foreign barcode map</param>
        /// <returns>The number of records added to the collection</returns>
        public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, bool bIgnoreMapped)
        {
            return GetRecordsFromBarcodes(strBarcodes, dxRecords, null, ",", false, bIgnoreMapped);

        }//	public int GetRecordsFromBarcodes(string strBarcodes, CDxMediaRecords dxRecords, ArrayList aInvalid)

        /// <summary>This methods gets the path relative to the case root for the specified record</summary>
        /// <param name="dxRecord">The desired record</param>
        /// <returns>The relative path for the specified record</returns>
        public string GetRelativePath(CDxMediaRecord dxRecord)
        {
            string strFileSpec = "";
            string strCasePath = "";
            string strRelative = "";

            //	Get the full file specification
            strFileSpec = GetFileSpec(dxRecord);

            if ((strFileSpec != null) && (strFileSpec.Length > 0))
            {
                strCasePath = GetCasePath(dxRecord);

                strRelative = System.IO.Path.GetDirectoryName(strFileSpec);

                if (strRelative.StartsWith(strCasePath) == true)
                    strRelative = strRelative.Substring(strCasePath.Length);
            }

            return strRelative;

        }// public string GetRelativePath(CDxMediaRecord dxRecord)

        /// <summary>This method is called to get the folder where exported slides for the specified presentation should be stored</summary>
        /// <param name="dxPrimary">The primary exchange record for the presentation</param>
        /// <returns>The folder where the slides should be stored</returns>
        public string GetSlidesFolderSpec(CDxPrimary dxPrimary)
        {
            string strFolder = "";

            //	Start with the case folder
            strFolder = m_strFolder;
            if (strFolder.EndsWith("\\") == false)
                strFolder += "\\";

            //	Now add the exported slides subfolder
            strFolder += "_tmax_slides\\";

            //	Now add the primary id
            strFolder += (dxPrimary.AutoId.ToString() + "\\");

            return strFolder.ToLower();

        }// public string GetSlidesFolderSpec(CDxPrimary dxPrimary)

        /// <summary>This method is called to get the folder where the specified transcript should be stored</summary>
        /// <param name="dxTranscript">The exchange interface for the desired transcript</param>
        /// <returns>The root folder for deposition videos</returns>
        public string GetTranscriptFolder(CDxTranscript dxTranscript)
        {
            string strFolder = "";

            Debug.Assert(m_aCaseFolders != null);
            Debug.Assert((int)TmaxCaseFolders.Transcripts < m_aCaseFolders.Count);
            if (m_aCaseFolders == null) return "";
            if ((int)TmaxCaseFolders.Transcripts >= m_aCaseFolders.Count) return "";

            strFolder = m_aCaseFolders[(int)TmaxCaseFolders.Transcripts].Path;

            if (strFolder.EndsWith("\\") == false)
                strFolder += "\\";

            //	Now add the id of the primary owner
            strFolder += (dxTranscript.PrimaryId.ToString() + "\\");
            return strFolder.ToLower();

        }// public string GetTranscriptFolder(CDxTranscript dxTranscript)

        /// <summary>Called to retrieve the unique id for the specified record</summary>
        /// <param name="tmaxRecord">The record interface object to construct the id</param>
        /// <returns>The unique id</returns>
        public string GetUniqueId(ITmaxMediaRecord tmaxRecord)
        {
            string strId = "";
            CDxPrimary dxPrimary = null;
            CDxSecondary dxSecondary = null;
            CDxTertiary dxTertiary = null;
            CDxQuaternary dxQuaternary = null;

            switch (tmaxRecord.GetMediaLevel())
            {
                case TmaxMediaLevels.Primary:

                    dxPrimary = (CDxPrimary)tmaxRecord;
                    break;

                case TmaxMediaLevels.Secondary:

                    dxSecondary = (CDxSecondary)tmaxRecord;
                    dxPrimary = dxSecondary.Primary;
                    Debug.Assert(dxPrimary != null);
                    break;

                case TmaxMediaLevels.Tertiary:

                    dxTertiary = (CDxTertiary)tmaxRecord;
                    dxSecondary = dxTertiary.Secondary;
                    Debug.Assert(dxSecondary != null);
                    dxPrimary = dxSecondary.Primary;
                    Debug.Assert(dxPrimary != null);
                    break;

                case TmaxMediaLevels.Quaternary:

                    dxQuaternary = (CDxQuaternary)tmaxRecord;
                    dxTertiary = dxQuaternary.Tertiary;
                    Debug.Assert(dxTertiary != null);
                    dxSecondary = dxTertiary.Secondary;
                    Debug.Assert(dxSecondary != null);
                    dxPrimary = dxSecondary.Primary;
                    Debug.Assert(dxPrimary != null);
                    break;

            }// switch(tmaxRecord.GetMediaLevel())

            if (dxPrimary != null)
            {
                strId = dxPrimary.AutoId.ToString();

                if (dxSecondary != null)
                {
                    strId += ("." + dxSecondary.AutoId.ToString());

                    if (dxTertiary != null)
                    {
                        strId += ("." + dxTertiary.AutoId.ToString());

                        if (dxQuaternary != null)
                        {
                            strId += ("." + dxQuaternary.AutoId.ToString());
                        }

                    }

                }

            }

            return strId;

        }// public string GetUniqueId(ITmaxMediaRecord tmaxRecord)

        /// <summary>This method is called to get the id of the current user</summary>
        /// <returns>The Id of the current user if available</returns>
        public long GetUserId()
        {
            if (m_dxUser != null)
                return m_dxUser.AutoId;
            else
                return -1;

        }// public long GetUserId()

        /// <summary>This method is called to get the name of the current user</summary>
        /// <returns>The name of the current user if available</returns>
        public string GetUserName()
        {
            if (m_dxUser != null)
                return m_dxUser.Name;
            else
                return "";
        }

        /// <summary>This method is called to get the name of the user with the specified id</summary>
        /// <returns>The name of the specified user</returns>
        public string GetUserName(long lId)
        {
            if (m_dxUsers != null)
            {
                foreach (CDxUser dxUser in m_dxUsers)
                {
                    if (dxUser.AutoId == lId)
                        return dxUser.Name;
                }
            }

            return "";

        }// public string GetUserName(long lId)

        /// <summary>This method is called to get a string representation of the database/assembly version</summary>
        /// <param name="bAssembly">True to get the assembly version</param>
        /// <returns>The version string</returns>
        public string GetVersionString(bool bAssembly)
        {
            string strVersion = "";

            if (bAssembly == true)
            {
                CTmdataVersion tmdataVer = new CTmdataVersion();
                strVersion = tmdataVer.ShortVersion;
            }
            else
            {
                //	Use the version information stored in the database
                strVersion = this.Detail.TmaxCase.Version;

            }// if(bAssembly == true)

            return strVersion;

        }// public string GetVersionString(bool bAssembly)

        /// <summary>This method is used to create an XML case object from the database</summary>
        /// <returns>The initialized CXmlCase object</returns>
        public CXmlCase GetXmlCase()
        {
            CXmlCase xmlCase = null;

            try
            {
                xmlCase = new CXmlCase(this.Detail.TmaxCase);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetXmlCase", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_XML_CASE_EX), Ex);
                xmlCase = null;
            }

            return xmlCase;

        }// public CXmlCase GetXmlCase()

        /// <summary>This method is used to retrieve the XML file associated with the specified record</summary>
        /// <param name="dxRecord">the media record associated with the deposition</param>
        /// <param name="bSegments">true to fill the segments collection</param>
        /// <param name="bTranscripts">true to fill the transcripts collection</param>
        /// <param name="bObjections">true to fill the objections collection</param>
        /// <returns>The associated XML deposition if it exists</returns>
        public CXmlDeposition GetXmlDeposition(CDxMediaRecord dxRecord, bool bSegments, bool bTranscript, bool bObjections)
        {
            CXmlDeposition xmlDeposition = null;
            CDxPrimary dxDeposition = null;
            CDxTranscript dxTranscript = null;
            string strFileSpec = "";

            try
            {
                //	What type of record are we dealing with?
                switch (dxRecord.MediaType)
                {
                    case TmaxMediaTypes.Deposition:

                        dxDeposition = (CDxPrimary)dxRecord;
                        break;

                    case TmaxMediaTypes.Segment:

                        if ((dxRecord.GetParent() != null) && (dxRecord.GetParent().MediaType == TmaxMediaTypes.Deposition))
                            dxDeposition = (CDxPrimary)dxRecord;
                        break;

                    case TmaxMediaTypes.Designation:

                        if ((dxRecord.GetParent() != null) && (dxRecord.GetParent().GetParent() != null))
                            dxDeposition = (CDxPrimary)(dxRecord.GetParent().GetParent());
                        break;

                }

                //	Can't do anything without the deposition record
                if (dxDeposition == null)
                {
                    FireError(this, "GetXmlDeposition", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_XML_DEPOSITION_NO_RECORD, dxRecord.GetBarcode(false)));
                    return null;
                }

                //	Get the transcript record for this deposition
                if ((dxTranscript = dxDeposition.GetTranscript()) == null)
                {
                    FireError(this, "GetXmlDeposition", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_XML_DEPOSITION_NO_TRANSCRIPT, dxRecord.GetBarcode(false)));
                    return null;
                }

                //	Make sure the XML file exists
                strFileSpec = this.GetFileSpec(dxTranscript);
                if (System.IO.File.Exists(strFileSpec) == false)
                {
                    FireError(this, "GetXmlDeposition", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_XML_DEPOSITION_NO_FILE, strFileSpec));
                    return null;
                }

                //	Allocate a new XML deposition
                xmlDeposition = new CXmlDeposition();
                SetHandlers(xmlDeposition.EventSource);

                //	Open the deposition
                xmlDeposition.FastFill(strFileSpec, bSegments, bTranscript, bObjections);

            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetXmlDeposition", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_XML_DEPOSITION_EX, dxRecord.GetBarcode(false)), Ex);

                if (xmlDeposition != null)
                {
                    xmlDeposition.Clear();
                    xmlDeposition = null;
                }

            }

            return xmlDeposition;

        }// public CXmlDeposition GetXmlDeposition(CDxMediaRecord dxRecord, bool bSegments, bool bTranscript)

        /// <summary>This method is called to get the XML designation associated with the specified tertiary record</summary>
        /// <param name="dxTertiary">The tertiary record that owns the XML designation</param>
        /// <param name="bCreate">True to create the file if it does not exist</param>
        /// <param name="bWarn">True to warn the user if there is a problem</param>
        /// <param name="bSynchronize">True to make sure the XML contents are in sync with the database contents</param>
        ///	<returns>The associated XML designationi</returns>
        public CXmlDesignation GetXmlDesignation(CDxTertiary dxTertiary, bool bCreate, bool bWarn, bool bSynchronize)
        {
            CXmlDesignation xmlDesignation = null;
            string strFileSpec = "";
            CXmlLink xmlLink = null;

            //	Make sure the tertiary record is of the appropriate type
            if ((dxTertiary.MediaType != TmaxMediaTypes.Designation) &&
                (dxTertiary.MediaType != TmaxMediaTypes.Clip))
                return null;

            //	Get the path to the XML file
            strFileSpec = GetFileSpec(dxTertiary);
            if ((strFileSpec == null) || (strFileSpec.Length == 0))
                return null;

            //	Does the file exist?
            if (System.IO.File.Exists(strFileSpec) == false)
            {
                //	Stop here if not allowed to create the file
                if (bCreate == false) return null;

                //	Warn the user if requested
                if (bWarn == true)
                {
                    string strMsg = String.Format("Unable to locate {0}. The XML file will be recreated but edited text will be restored to the original", strFileSpec);
                    MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //	Create the XML file
                if (CreateXmlDesignation(dxTertiary, strFileSpec) == false)
                    return null;

            }// if(System.IO.File.Exists(strFileSpec) == false)

            xmlDesignation = new CXmlDesignation();

            //	Read the XML descriptor
            //
            //	NOTE:	We don't bother getting the link information because we're
            //			going to use the database records to build the collection.
            //			This ensures everything stays in sync
            if (xmlDesignation.FastFill(strFileSpec, !bSynchronize, true) == true)
            {
                //	Are we supposed to synchronize the XML contents?
                if (bSynchronize == true)
                {
                    if (dxTertiary.GetExtent() != null)
                        dxTertiary.SetAttributes(xmlDesignation);

                    //	Make sure we've loaded the links for this designation/clip
                    if ((dxTertiary.Quaternaries == null) || (dxTertiary.Quaternaries.Count == 0))
                    {
                        if (dxTertiary.ChildCount > 0)
                            dxTertiary.Fill();
                    }

                    //	Make sure the links are up to date
                    foreach (CDxQuaternary O in dxTertiary.Quaternaries)
                    {
                        xmlLink = new CXmlLink();
                        O.SetAttributes(xmlLink);
                        xmlDesignation.Links.Add(xmlLink);
                    }

                }// if(bSynchronize == true)

            }
            else
            {
                xmlDesignation = null;

            }// if(xmlDesignation.FastFill(strFileSpec, !bSynchronize, true) == true)

            return xmlDesignation;

        }// public CXmlDesignations GetXmlDesignation(CDxTertiary dxTertiary)

        /// <summary>This method will add or insert records using information stored in files selected by the user</summary>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxTarget">The TrialMax event item that identifies the target parent record and optional insertion point</param>
        /// <param name="tmaxResults">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        public bool Import(CTmaxParameters tmaxParameters, CTmaxItem tmaxTarget, CTmaxDatabaseResults tmaxResults)
        {
            bool bSuccessful = false;

            try
            {
                //	First check to see if the caller is importing objections
                if (IsObjectionsCommand(tmaxParameters) == true)
                    return ImportObjections(tmaxTarget, tmaxParameters, tmaxResults);

                //	Prepare the manager for a new operation
                m_tmaxImportManager.ImportOptions = m_tmaxStationOptions.ImportOptions;
                m_tmaxImportManager.Initialize(tmaxParameters, tmaxResults);
                m_tmaxImportManager.SetTarget(tmaxTarget);

                //	We MUST have a valid import format
                if (m_tmaxImportManager.Format == TmaxImportFormats.Unknown)
                {
                    Debug.Assert(m_tmaxImportManager.Format != TmaxImportFormats.Unknown, "Unknown import format specification");
                    return false;
                }

                //	Prompt the user for the list of files to be imported
                if (m_tmaxImportManager.GetSourceFiles() == false) return false;

                //	Execute the import operation
                if (m_tmaxImportManager.Import() == true)
                {
                    //	Should we store the results in the target collection
                    if ((tmaxTarget != null) && (tmaxResults != null) && (tmaxResults.Added != null))
                    {
                        tmaxTarget.ReturnItem = new CTmaxItem();
                        tmaxTarget.ReturnItem.DataType = TmaxDataTypes.Media;
                        tmaxTarget.ReturnItem.MediaType = TmaxMediaTypes.Script;

                        foreach (CTmaxItem O in tmaxResults.Added)
                        {
                            //	Only include in the return results if this was
                            //	NOT an update of an original record
                            if (O.UserData1 == null)// NO original source record
                            {
                                try
                                {
                                    tmaxTarget.ReturnItem.SubItems.Add(O);
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                //	Add to source items collection to allow the caller
                                //	to isolate those that were updated as opposed to added
                                if (tmaxTarget.ReturnItem.SourceItems == null)
                                    tmaxTarget.ReturnItem.SourceItems = new CTmaxItems();

                                tmaxTarget.ReturnItem.SourceItems.Add(O);
                            }

                        }

                    }

                    bSuccessful = true;

                }// if(m_tmaxImportManager.Import() == true)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "Import", this.ExBuilder.Message(ERROR_CASE_DATABASE_IMPORT_EX), Ex);
            }
            finally
            {
            }

            return bSuccessful;

        }// public bool Import(CTmaxParameters tmaxParameters, CTmaxItems tmaxImported, CTmaxItem tmaxTarget)

        /// <summary>This method will add or insert records using information stored in files selected by the user</summary>
        /// <param name="eFormat">The enumerated format type identifier</param>
        /// <param name="tmaxTarget">The TrialMax event item that identifies the target parent record and optional insertion point</param>
        /// <param name="tmaxResults">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        public bool Import(TmaxImportFormats eFormat, CTmaxItem tmaxTarget, CTmaxDatabaseResults tmaxResults)
        {
            CTmaxParameters tmaxParameters = null;

            //	Create a parameter collection to define the import format
            tmaxParameters = new CTmaxParameters();
            tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)eFormat));

            //	Perform the operation
            return Import(tmaxParameters, tmaxTarget, tmaxResults);
        }

        /// <summary>This method will add or insert records using information stored in files selected by the user</summary>
        /// <param name="eFormat">The enumerated format type identifier</param>
        /// <param name="tmaxResults">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        public bool Import(TmaxImportFormats eFormat, CTmaxDatabaseResults tmaxResults)
        {
            return Import(eFormat, null, tmaxResults);
        }

        /// <summary>This method will add or insert records using information stored in files selected by the user</summary>
        /// <param name="eFormat">The enumerated format type identifier</param>
        /// <returns>true if successful</returns>
        public bool Import(TmaxImportFormats eFormat)
        {
            return Import(eFormat, null, null);
        }

        /// <summary>This method will insert scenes into the specified script using the collection of source items provided by the caller</summary>
        /// <param name="dxScript">The primary exchange object that represents the parent script</param>
        ///	<parma name="tmaxSource">The collection of media records to be added as scenes</parma>
        ///	<parma name="tmaxScenes">The collection in which to store new scene objects</parma>
        /// <param name="dxInsertion">The insertion point into the secondary collection</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        public bool InsertScenes(CDxPrimary dxScript, CTmaxItems tmaxSource, CTmaxItems tmaxScenes, CDxSecondary dxInsertion, CTmaxParameters tmaxParameters)
        {
            CTmaxParameter tmaxParameter = null;
            long lOriginal = dxScript.ChildCount;
            long lAdded = 0;
            int iInsertIndex = -1;
            int i;
            CDxMediaRecord dxRecord;
            CDxPrimary dxPrimary;
            CDxSecondary dxScene;
            CTmaxItems tmaxExpanded = new CTmaxItems();
            CDxSecondaries dxHolding = null;
            bool bReorder = true;
            bool bBefore = false;
            bool bShortcut = false;

            if (tmaxParameters != null)
            {
                //	Get the parameters passed by the caller
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Before)) != null)
                    bBefore = tmaxParameter.AsBoolean();
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Shortcut)) != null)
                    bShortcut = tmaxParameter.AsBoolean();
            }

            //	Add each of the scenes to the end of the script
            foreach (CTmaxItem tmaxItem in tmaxSource)
            {
                if ((dxRecord = (CDxMediaRecord)tmaxItem.GetMediaRecord()) != null)
                {
                    if (dxRecord.GetMediaLevel() == TmaxMediaLevels.Primary)
                    {
                        dxPrimary = (CDxPrimary)dxRecord;

                        //	Do we need to fill the primary record?
                        if (dxPrimary.Secondaries.Count == 0)
                            dxPrimary.Fill();

                        foreach (CDxSecondary dxSecondary in dxPrimary.Secondaries)
                        {
                            if ((dxScene = AddScene(dxScript, dxSecondary, bShortcut, null)) != null)
                            {
                                //	Give the caller access to the new record
                                tmaxItem.ReturnItem = new CTmaxItem(dxScene);

                                //	Put it in the return collection
                                if (tmaxScenes != null)
                                    tmaxScenes.Add(tmaxItem.ReturnItem);
                            }
                        }
                    }
                    else
                    {
                        if ((dxScene = AddScene(dxScript, dxRecord, bShortcut, tmaxItem.XmlScene)) != null)
                        {
                            //	Give the caller access to the new record
                            tmaxItem.ReturnItem = new CTmaxItem(dxScene);

                            //	Put it in the return collection
                            tmaxScenes.Add(tmaxItem.ReturnItem);
                        }
                    }

                }// if((dxRecord = (CDxMediaRecord)tmaxItem.GetMediaRecord()) != null)

            }// foreach(CTmaxItem tmaxItem in tmaxSource)

            //	How many children were actually added to the collection
            lAdded = (dxScript.Secondaries.Count - lOriginal);
            if (lAdded <= 0)
            {
                //	Shouldn't be negative
                Debug.Assert(lAdded == 0);
                return false;
            }

            //	Update the number of children
            dxScript.ChildCount = dxScript.Secondaries.Count;
            dxScript.SetPlaylistFromChildren();
            m_dxPrimaries.Update(dxScript);

            //	Get the index of the insertion point
            if (dxInsertion != null)
                iInsertIndex = dxScript.Secondaries.IndexOf(dxInsertion);

            //	Do we need to reorder the collection?
            if (iInsertIndex < 0)
            {
                bReorder = false;
            }
            else if ((bBefore == false) && (ReferenceEquals(dxInsertion, dxScript.Secondaries[(int)lOriginal - 1]) == true))
            {
                bReorder = false;
            }

            if (bReorder == true)
            {
                //	Remove the new records and put them in the holding collection
                dxHolding = new CDxSecondaries();
                for (i = 0; i < lAdded; i++)
                {
                    dxHolding.AddList(dxScript.Secondaries[(int)lOriginal]);
                    dxScript.Secondaries.RemoveList(dxScript.Secondaries[(int)lOriginal]);
                }

                //	Adjust the index if we are inserting after
                if (bBefore == false)
                    iInsertIndex++;

                //	Now put the new records in the correct location
                for (i = 0; i < dxHolding.Count; i++)
                {
                    dxScript.Secondaries.InsertList(iInsertIndex, dxHolding[i]);
                    iInsertIndex++;
                }
                dxHolding.Clear();

                //	Now renumber the secondaries
                for (i = 0; i < dxScript.Secondaries.Count; i++)
                {
                    if (dxScript.Secondaries[i].DisplayOrder != (i + 1))
                    {
                        dxScript.Secondaries[i].DisplayOrder = (i + 1);
                        dxScript.Update(dxScript.Secondaries[i]);
                    }
                }

            }// if(bReorder == true)

            return true;

        }// public bool InsertScenes(CDxPrimary dxScript, CTmaxItems tmaxSource, CTmaxItems tmaxScenes, CDxSecondary dxInsertion, CTmaxParameters tmaxParameters)

        /// <summary>This method will insert the source files provided by the caller</summary>
        /// <param name="dxPrimary">The primary exchange object that owns the secondary collection</param>
        ///	<parma name="tmaxSource">The folder containing the files to be added</parma>
        /// <param name="dxInsertion">The insertion point into the secondary collection</param>
        /// <param name="bBefore">true to insert before, false to insert after</param>
        /// <returns>true if successful</returns>
        public bool InsertSource(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource, CDxSecondary dxSecondary, bool bBefore)
        {
            long lOriginal = dxPrimary.ChildCount;
            long lAdded = 0;
            int iInsertIndex = -1;
            int i;
            CDxSecondaries dxHolding = null;
            bool bReorder = true;
            long lBarcodeId = 0;

            //	Get the next barcode identifier
            if ((dxPrimary.ChildCount > 0) && (dxPrimary.Secondaries.Count == 0))
                dxPrimary.Fill();
            lBarcodeId = dxPrimary.Secondaries.GetNextBarcodeId();

            //	Start by adding the new source
            if (AddSource(tmaxSource, dxPrimary, lBarcodeId) == false)
                return false;

            //	How many children were actually added to the collection
            lAdded = (dxPrimary.Secondaries.Count - lOriginal);
            if (lAdded <= 0)
            {
                //	Shouldn't be negative
                Debug.Assert(lAdded == 0);
                return false;
            }

            //	Update the number of children
            dxPrimary.ChildCount = dxPrimary.Secondaries.Count;
            m_dxPrimaries.Update(dxPrimary);

            //	Get the index of the insertion point
            if (dxSecondary != null)
                iInsertIndex = dxPrimary.Secondaries.IndexOf(dxSecondary);

            //	Do we need to reorder the collection?
            if (iInsertIndex < 0)
            {
                bReorder = false;
            }
            else if ((bBefore == false) && (ReferenceEquals(dxSecondary, dxPrimary.Secondaries[(int)lOriginal - 1]) == true))
            {
                bReorder = false;
            }

            if (bReorder == true)
            {
                //	Remove the new records and put them in the holding collection
                dxHolding = new CDxSecondaries();
                for (i = 0; i < lAdded; i++)
                {
                    dxHolding.AddList(dxPrimary.Secondaries[(int)lOriginal]);
                    dxPrimary.Secondaries.RemoveList(dxPrimary.Secondaries[(int)lOriginal]);
                }

                //	Adjust the index if we are inserting after
                if (bBefore == false)
                    iInsertIndex++;

                //	Now put the new records in the correct location
                for (i = 0; i < dxHolding.Count; i++)
                {
                    dxPrimary.Secondaries.InsertList(iInsertIndex, dxHolding[i]);
                    iInsertIndex++;
                }
                dxHolding.Clear();

                //	Now renumber the secondaries
                for (i = 0; i < dxPrimary.Secondaries.Count; i++)
                {
                    if (dxPrimary.Secondaries[i].DisplayOrder != (i + 1))
                    {
                        dxPrimary.Secondaries[i].DisplayOrder = (i + 1);
                        dxPrimary.Update(dxPrimary.Secondaries[i]);
                    }
                }

            }// if(bReorder == true)

            //	Has the first file changed?
            if (dxPrimary.Secondaries[0].Filename != dxPrimary.Filename)
            {
                dxPrimary.Filename = dxPrimary.Secondaries[0].Filename;
                m_dxPrimaries.Update(dxPrimary);
            }

            return true;

        }// public bool InsertSource(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource, CDxSecondary dxSecondary, bool bBefore)

        /// <summary>This method is called to determine if the specified record exchange object is still valid</summary>
        /// <param name="dxRecord">The record exchange object being validated</param>
        /// <returns>true if valid</returns>
        public bool IsValidRecord(CDxMediaRecord dxRecord)
        {
            CDxMediaRecord dxParent;

            if ((dxRecord == null) ||
                (m_dxPrimaries == null) ||
                (m_dxPrimaries.Count == 0))
            {
                return false;
            }

            //	Is this a media record?
            if (dxRecord.GetDataType() == TmaxDataTypes.Media)
            {
                //	Get the parent record
                if ((dxParent = dxRecord.GetParent()) != null)
                {
                    //	Make sure the parent is still valid
                    if (IsValidRecord(dxParent) == true)
                    {
                        if (dxParent.GetChildCollection() != null)
                            return dxParent.GetChildCollection().Contains(dxRecord);
                    }

                    return false;

                }
                else
                {
                    //	Only primary records have no parent
                    return m_dxPrimaries.Contains(dxRecord);
                }

            }

                //	Is it a binder?
            else if (dxRecord.GetDataType() == TmaxDataTypes.Binder)
            {
                //	Get the parent record
                if ((dxParent = dxRecord.GetParent()) != null)
                {
                    //	Make sure the parent is still valid
                    if (IsValidRecord(dxParent) == true)
                    {
                        if (dxParent.GetChildCollection() != null)
                            return dxParent.GetChildCollection().Contains(dxRecord);
                    }

                    return false;

                }
                else
                {
                    //	Only root binders have no parent
                    return ReferenceEquals(dxRecord, m_dxRootBinder);
                }

            }// else if(dxRecord.GetDataType() == TmaxDataTypes.Binder)

            else
            {
                //	Assume it's valid since we can't test
                return true;
            }

        }// public bool IsValidRecord(CDxMediaRecord dxRecord)

        /// <summary>This method is called to determine if the record associated with the specified item is valid</summary>
        /// <param name="tmaxItem">The event item that references the record being validated</param>
        /// <returns>true if valid</returns>
        public bool IsValidRecord(CTmaxItem tmaxItem)
        {
            if (tmaxItem.GetMediaRecord() != null)
                return IsValidRecord((CDxMediaRecord)(tmaxItem.GetMediaRecord()));
            else if (tmaxItem.IBinderEntry != null)
                return IsValidRecord((CDxMediaRecord)(tmaxItem.IBinderEntry));
            else
                return false;

        }// public bool IsValidRecord(CTmaxItem tmaxItem)

        /// <summary>This method will merge two or more media objects</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxResults">Collection to populate with item that represent the new record</param>
        /// <returns>true if successful</returns>
        public bool Merge(CTmaxItem tmaxItem, CTmaxDatabaseResults tmaxResults)
        {
            CDxPrimary dxMerged = null;
            CDxPrimary dxScript = null;
            CTmaxItems tmaxSource = null;
            CTmaxItems tmaxScenes = null;
            CTmaxItem tmaxAdded = null;

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.SubItems != null);
            Debug.Assert(tmaxItem.SubItems.Count > 1);

            if (tmaxItem == null) return false;
            if (tmaxItem.SubItems == null) return false;
            if (tmaxItem.SubItems.Count < 2) return false;

            //	The return collection should be empty
            if ((tmaxResults != null) && (tmaxResults.Added != null))
                tmaxResults.Added.Clear();

            //	Are we merging scripts?
            if (tmaxItem.MediaType == TmaxMediaTypes.Script)
            {
                //	Start by adding a new script
                if ((dxMerged = AddPrimary(TmaxMediaTypes.Script)) != null)
                {
                    //	Allocate the working collections
                    tmaxSource = new CTmaxItems();
                    tmaxScenes = new CTmaxItems();

                    //	Build the collection of source records
                    foreach (CTmaxItem O in tmaxItem.SubItems)
                    {
                        Debug.Assert(O.GetMediaRecord() != null);
                        Debug.Assert(O.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script);

                        if ((O.GetMediaRecord() != null) && (O.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script))
                        {
                            dxScript = (CDxPrimary)O.GetMediaRecord();

                            //	Do we need to fill this script?
                            if ((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
                                dxScript.Fill();

                            //	Add an item for each scene
                            foreach (CDxSecondary dxScene in dxScript.Secondaries)
                            {
                                if (dxScene.GetSource() != null)
                                    tmaxSource.Add(new CTmaxItem(dxScene.GetSource()));
                            }

                        }

                    }// foreach(CTmaxItem O in tmaxItem.SubItems)

                    //	Add the scenes
                    if (tmaxSource.Count > 0)
                        InsertScenes(dxMerged, tmaxSource, tmaxScenes, null, null);

                    //	Clear the working collections
                    tmaxScenes.Clear();
                    tmaxSource.Clear();

                    //	Only return the primary record
                    tmaxItem.ReturnItem = new CTmaxItem(dxMerged);
                    if ((tmaxResults != null) && (tmaxResults.Added != null))
                    {
                        tmaxAdded = new CTmaxItem();
                        tmaxAdded.DataType = TmaxDataTypes.Media;
                        tmaxAdded.MediaType = TmaxMediaTypes.Script;
                        tmaxAdded.SubItems.Add(new CTmaxItem(dxMerged));

                        tmaxResults.Added.Add(tmaxAdded);
                    }

                }// if((dxMerged = AddPrimary(TmaxMediaTypes.Script)) != null)

            }
            else
            {
                //	Don't know how to merge anything else now
                Debug.Assert(tmaxItem.MediaType == TmaxMediaTypes.Script);
            }

            return (tmaxItem.ReturnItem != null);

        }// public bool Merge(CTmaxItem tmaxItem, CTmaxDatabaseResults tmaxResults)

        /// <summary>This method will move the records in the SourceItems collection of the event item</summary>
        /// <param name="tmaxMove">The event item that identifies the new parent, the new insertion point, and the records to be moved</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        public bool Move(CTmaxItem tmaxMove, CTmaxParameters tmaxParameters)
        {
            CTmaxParameter tmaxParameter = null;
            bool bBefore = false;
            bool bSuccessful = false;

            Debug.Assert(tmaxMove != null, "CTmaxCaseDatabase::Move() -> Invalid parameter tmaxMove");
            Debug.Assert(tmaxMove.SourceItems != null, "CTmaxCaseDatabase::Move() -> no source items collection");
            Debug.Assert(tmaxMove.SourceItems.Count > 0, "CTmaxCaseDatabase::Move() -> no source items to be moved");

            try
            {
                //	Did the caller provide a parameters collection?
                if (tmaxParameters != null)
                {
                    //	Get the Before parameter if it exists
                    if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Before)) != null)
                        bBefore = tmaxParameter.AsBoolean();
                }

                //	Are we moving binder entries?
                if (tmaxMove.DataType == TmaxDataTypes.Binder)
                {
                    bSuccessful = MoveBinderEntries(tmaxMove, bBefore);
                }
                else
                {
                    // Can't move anything but binders
                    Debug.Assert(false, "Move only valid for binder entries");
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "Move", this.ExBuilder.Message(ERROR_CASE_DATABASE_MOVE_EX), Ex);
            }

            return bSuccessful;

        }// public bool Move(CTmaxItem tmaxMove, CTmaxParameters tmaxParameters, CTmaxItems tmaxMoved)

        public void ResetDisplayOrder(CDxBinderEntry dxEntry)
        {
            //if(tmaxMove.DataType == TmaxDataTypes.Binder)
            Debug.Assert(dxEntry != null);
            dxEntry.ResetDisplayOrder();
        }

        public void ResetSortProperty(CDxBinderEntry dxEntry)
        {
            //if(tmaxMove.DataType == TmaxDataTypes.Binder)
            Debug.Assert(dxEntry != null);
            dxEntry.ResetSortProperty();
        }
        /// <summary>This method is called to warn the user when an error occurs while adding a Presentation treatment</summary>
        /// <param name="strMsg">The message to be displayed</param>
        /// <param name="strFileSpec">The path to the file being imported</param>
        public void OnAddPresentationError(string strMsg, string strFileSpec)
        {
            strMsg += ("\n\nDo you want to delete the source file: " + strFileSpec);

            if (MessageBox.Show(strMsg, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                try
                {
                    System.IO.File.Delete(strFileSpec);
                }
                catch
                {
                }

            }

        }// public void OnAddPresentationError(string strMsg, string strFileSpec)

        /// <summary>This method handles all Progress events fired by a depolog during its conversion</summary>
        /// <param name="objSender">The object sending the event</param>
        /// <param name="strProgress">The progress text</param>
        public void OnDepoLogProgress(object objSender, string strProgress)
        {
            SetRegisterProgress(strProgress);
        }

        /// <summary>This method handles all Diagnostic events received by the database</summary>
        /// <param name="objSender">The object sending the event</param>
        /// <param name="Args">Diagnostic event arguments</param>
        public void OnDiagnostic(object objSender, CTmaxDiagnosticArgs Args)
        {
            //	Propagate the error
            FireDiagnostic(Args);
        }

        /// <summary>This method handles all Error events received by the database</summary>
        /// <param name="objSender">The object sending the event</param>
        /// <param name="Args">Error event arguments</param>
        public void OnError(object objSender, CTmaxErrorArgs Args)
        {
            //	Propagate the error
            FireError(Args);
        }

        /// <summary>Call to open the database</summary>
        /// <param name="strFolder">Folder containing the case database</param>
        /// <param name="strUser">Name of user opening the database</param>
        /// <returns>true if successful</returns>
        public bool Open(string strFolder, string strUser)
        {
            //	Open the database and confirm the version information
            if (Open(strFolder, true) == false) return false;

            Debug.Assert(m_dxDetails != null);
            Debug.Assert(m_dxDetail != null);

            //	Fill the local record collections
            //
            //	NOTE:	The details collection gets filled in response
            //			to the call to CheckDatabaseVersion()
            m_dxUsers.Fill();
            m_dxHighlighters.Fill();

            if (m_bFillOnOpen == true)
            {
                m_dxPrimaries.Fill();
                m_dxRootBinder.Fill();
                m_dxBarcodeMap.Fill();
            }

            //	Locate the specified user
            if ((m_dxUser = m_dxUsers.Find(strUser)) != null)
            {
                //	Update the user time stamp
                m_dxUser.LastTime = System.DateTime.Now;
                m_dxUsers.Update(m_dxUser);

                //	Set the user name in the case options
                if ((m_tmaxCaseOptions != null) && (m_tmaxCaseOptions.Machine != null))
                {
                    m_tmaxCaseOptions.Machine.User = m_dxUser.Name;
                }

            }
            else
            {
                //	Add this user to the database
                if (!SetUser(strUser))
                {
                    Close();
                    return false;
                }

            }

            //	Create the default highlighters if this is the first time
            if (m_dxHighlighters.Count == 0)
            {
                SetHighlighters();
            }

            //	Get the codes to be used for this case
            InitializeCaseCodes(false);

            //	Initialize the station specific options
            SetStationOptions();

            //	Assign the codes to the filter
            if (m_tmaxStationOptions.AdvancedFilter != null)
            {
                m_tmaxStationOptions.AdvancedFilter.CaseCodes = this.CaseCodes;
                m_tmaxStationOptions.AdvancedFilter.PickLists = this.PickLists;
                m_tmaxLastFilter = m_tmaxStationOptions.AdvancedFilter;
            }

            //	Populate the filtered collection if requested
            if ((m_tmaxAppOptions != null) && (m_tmaxAppOptions.FilterOnOpen == true))
                SetFiltered(null);

            //	Initialize the objections database
            InitializeObjections();

            return true;

        }// Open(string strFolder, string strUser)

        /// <summary>This method will register the source files provided by the caller</summary>
        /// <param name="tmaxSource">The parent source folder containing the files to be registered</param>
        /// <param name="tmaxParameters">The parameters passed with the event that triggered the registration</param>
        /// <returns>true if successful</returns>
        /// <remarks>This methods executes a thread to run the registration</remarks>
        public bool Register(CTmaxSourceFolder tmaxSourceFolder, CTmaxParameters tmaxParameters)
        {
            long lFiles;
            CTmaxParameter paramSourceType = null;

            //	Make sure the database is open
            if (m_dxPrimaries == null)
            {
                FireError(this, "Register", this.ExBuilder.Message(ERROR_CASE_DATABASE_REGISTER_CLOSED));
                return false;
            }

            //	How many files are we attempting to register?
            if ((lFiles = tmaxSourceFolder.GetFileCount(true)) == 0)
            {
                FireError(this, "Register", this.ExBuilder.Message(ERROR_CASE_DATABASE_REGISTER_EMPTY, tmaxSourceFolder.Path));
                return false;
            }

            //	Get the optional event parameters
            if (tmaxParameters != null)
            {
                paramSourceType = tmaxParameters.Find(TmaxCommandParameters.RegSourceType);
            }

            //	Store the registration folders
            m_RegSourceFolder = tmaxSourceFolder;
            m_RegSourceParent = tmaxSourceFolder;

            //	Clear the local register operation flags
            m_bRegisterWarnAdobe = true;

            //	Get the registration source type
            if (paramSourceType != null)
                m_eRegSourceType = (RegSourceTypes)(paramSourceType.iValue);
            else
                m_eRegSourceType = RegSourceTypes.AllFiles;

            if (m_tmaxRegisterOptions != null)
            {
                //	Are we supposed to automatically resolve registration conflicts
                m_bAutoResolve = (m_tmaxRegisterOptions.ConflictResolution == RegConflictResolutions.Automatic);

                //	Are we registering Multi-page TIFF files?
                m_tmaxSourceTypes.UseMultiPageTIFF = m_tmaxRegisterOptions.GetFlag(RegFlags.MultiPageTiff);
            }

            //	Adjust the registration type if using MultiPage TIFF combined with document
            if ((m_tmaxSourceTypes.UseMultiPageTIFF == true) && (m_eRegSourceType == RegSourceTypes.Document))
                m_eRegSourceType = RegSourceTypes.MultiPageTIFF;

            //	Create the progress form before launching the registration thread
            CreateRegisterProgress("Registration Progress", ("Registering " + lFiles.ToString() + " source files"), lFiles);

            try
            {
                //	Run the registration in it's own thread
                if (RegThread != null)
                {
                    RegThread.Abort();
                    RegThread = null;
                }
                RegThread = new Thread(new ThreadStart(this.RegisterThreadProc));
                RegThread.Start();
            }
            catch (System.Exception Ex)
            {
                FireError(this, "Register", this.ExBuilder.Message(ERROR_CASE_DATABASE_REGISTER_THREAD_EX), Ex);
                return false;
            }

            //	Open the progress form
            if (m_cfRegisterProgress != null)
            {
                //	Show modal to prevent returning until finished or canceled
                if (m_cfRegisterProgress.ShowDialog() == DialogResult.Cancel)
                {
                    m_bRegisterCancelled = true;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        //  Now we need to check if any tasks are pending. If there are then we need to signal each task to stop processing and perform cleanp.
                        if (RegThread != null)
                        {
                            //lock (lockConversionTasksArray)
                            //{
                            if (ConversionTasksArray != null)
                            {
                                for (int i = 0; i < ConversionTasksArray.Count; i++)
                                {
                                    ConversionTasksArray[i].StopConversionProcess();
                                }
                                ConversionTasksArray = null;
                            }
                            //}
                            RegThread.Join();   // We need to wait for all tasks to be stopped (if any) and then proceed so proper cleanup is done.
                            RegThread = null;
                        }
                    }
                    catch (Exception Ex)
                    {
                        logDetailed.Error(Ex.ToString());
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    if (RegThread != null)
                    {
                        RegThread.Join();   // We need to wait for all tasks to be stopped (if any) and then proceed so proper cleanup is done.
                        RegThread = null;
                    }
                }

                //	Clean up
                m_cfRegisterProgress.Dispose();
                m_cfRegisterProgress = null;

                if (m_bRegisterCancelled)
                {
                    FireCommand(TmaxCommands.RefreshCodes);
                }
            }// if(m_cfRegisterProgress != null)

            //	Clear this flag before returning
            m_tmaxSourceTypes.UseMultiPageTIFF = false;

            return !m_bRegisterCancelled;
        }

        /// <summary>This method will reorder the child records of the specified parent</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <returns>true if successful</returns>
        public bool Reorder(CTmaxItem tmaxItem)
        {
            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.SubItems != null);
            Debug.Assert(tmaxItem.SubItems.Count > 0);
            if (tmaxItem == null) return false;
            if (tmaxItem.SubItems == null) return false;
            if (tmaxItem.SubItems.Count <= 0) return false;

            //	Is the user attempting to reorder the case codes?
            if (tmaxItem.DataType == TmaxDataTypes.CaseCode)
                return ReorderCaseCodes(tmaxItem);
            else
                return ReorderMedia(tmaxItem);

        }//	public bool Reorder(CTmaxItem tmaxItem)

        /// <summary>This method will save the designation/clip as an XML file</summary>
        /// <param name="dxScene">The script scene record that owns the designation</param>
        /// <param name="dxTertiary">The source tertiary record</param>
        /// <param name="strFileSpec">The path to the desired XML file</param>
        /// <returns>true if successful</returns>
        public bool CreateXmlDesignation(CDxTertiary dxTertiary, string strFileSpec)
        {
            CDxTranscript dxTranscript = null;
            CXmlDesignation xmlDesignation = null;
            CXmlLink xmlLink = null;
            CXmlDeposition xmlDeposition = null;
            bool bSuccessful = false;
            string strDepositionFile = "";

            try
            {
                //	Create a new XML designation object
                xmlDesignation = new CXmlDesignation();

                //	Make sure the XML is consistent with the database
                dxTertiary.SetAttributes(xmlDesignation);

                //	Make sure we've retrieved the links
                if (dxTertiary.Quaternaries.Count == 0)
                    dxTertiary.Fill();

                //	Add all available links
                foreach (CDxQuaternary O in dxTertiary.Quaternaries)
                {
                    xmlLink = new CXmlLink();
                    O.SetAttributes(xmlLink);
                    xmlDesignation.Links.Add(xmlLink);
                }

                //	Retrieve the text if this is a designation
                if (dxTertiary.MediaType == TmaxMediaTypes.Designation)
                {
                    Debug.Assert(dxTertiary.Secondary != null);
                    Debug.Assert(dxTertiary.Secondary.Primary != null);

                    if ((dxTertiary.Secondary != null) && (dxTertiary.Secondary.Primary != null))
                        dxTranscript = dxTertiary.Secondary.Primary.GetTranscript();

                    //	Do we have the transcript object?
                    if (dxTranscript != null)
                    {
                        xmlDeposition = new CXmlDeposition();
                        strDepositionFile = GetFileSpec(dxTranscript);

                        //	Open the master deposition XML file
                        if (xmlDeposition.FastFill(strDepositionFile, true, true, false) == true)
                        {
                            xmlDeposition.GetTranscripts(xmlDesignation);
                        }
                        else
                        {
                            MessageBox.Show("Unable to retrieve the transcript text for " + dxTertiary.GetBarcode(false) + " from " + strDepositionFile,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        //	Flush and close the XML deposition
                        xmlDeposition.Close(true);
                        xmlDeposition = null;

                    }
                    else
                    {
                        MessageBox.Show("Unable to retrieve the transcript information for " + dxTertiary.GetBarcode(false),
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }// if(dxTertiary.MediaType == TmaxMediaTypes.Designation)

                //	Do we have any text associated with this designation?
                if ((xmlDesignation.Transcripts != null) &&
                    (xmlDesignation.Transcripts.Count > 0))
                {
                    xmlDesignation.HasText = true;
                    xmlDesignation.ScrollText = true;
                }
                else
                {
                    xmlDesignation.HasText = false;
                    xmlDesignation.ScrollText = false;
                }

                //	Save the designation
                if (CreateFolder(dxTertiary, true, true) == true)
                {
                    bSuccessful = xmlDesignation.Save(strFileSpec);
                }

                return bSuccessful;
            }
            catch (System.Exception Ex)
            {
                FireError(this, "CreateXmlDesignation", this.ExBuilder.Message(ERROR_CASE_DATABASE_CREATE_XML_DESIGNATION_EX, dxTertiary.GetBarcode(true), strFileSpec), Ex);
                return false;
            }

        }// public bool CreateXmlDesignation(CDxTertiary dxTertiary, string strFileSpec)

        /// <summary>This method will set the source record for the binder entry specified by the caller</summary>
        /// <param name="dxEntry">The entry that identifies the desired binder record</param>
        /// <param name="dxSource">The new source record</param>
        /// <returns>The application's binder record</returns>
        /// <remarks>The GetBinderEntries() method identifies binders but does not return the actual records. This method will retrieve the actual application record</remarks>
        public CDxBinderEntry SetBinderSource(CDxBinderEntry dxEntry, CDxMediaRecord dxSource)
        {
            CDxBinderEntry dxBinder = null;
            CDxBinderEntry dxParent = null;

            try
            {
                //	Get the actual application record interface to the parent binder
                if ((dxParent = GetBinderFromPath(dxEntry.ParentPathId)) != null)
                {
                    //	Now locate this binder in the child collection
                    if (dxParent.Contents.Count == 0)
                        dxParent.Fill();

                    if ((dxBinder = dxParent.Contents.Find(dxEntry.AutoId)) != null)
                    {
                        dxBinder.Source = dxSource;
                        dxBinder.Name = dxSource.GetUniqueId();

                        Update(new CTmaxItem(dxBinder), null, null);
                    }

                }

            }
            catch
            {
            }

            return dxBinder;

        }// public CDxBinderEntry SetBinderSource(CDxBinderEntry dxEntry, CDxMediaRecord dxSource)

        /// <summary>This method is called to let the user set the case options</summary>
        /// <returns>True if the user accepts the changes</returns>
        public bool SetCaseOptions()
        {
            CFCaseOptions caseOptions = null;
            bool bApplied = false;

            Debug.Assert(m_tmaxCaseOptions != null);
            if (m_tmaxCaseOptions == null) return false;

            try
            {
                //	Refresh the case codes
                //
                //	NOTE:	We do this via the application so that the panes using the codes get notified
                //			This MUST be done PRIOR to assigning the collection to the editor
                FireCommand(TmaxCommands.RefreshCodes);

                //	Create the options form
                caseOptions = new FTI.Trialmax.Forms.CFCaseOptions();
                SetHandlers(caseOptions.EventSource);
                caseOptions.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);

                //	Set the property values
                caseOptions.CaseOptions = m_tmaxCaseOptions;
                caseOptions.CodesManager = m_tmaxCodesManager;
                caseOptions.StationOptions = m_tmaxStationOptions;
                caseOptions.CaseName = this.CaseName;
                caseOptions.ShortCaseName = this.ShortCaseName;
                caseOptions.CaseFolder = this.Folder;

                //	Are we using file based case codes?
                if (this.DxCaseCodes == null)
                {
                    //	Lock the file for editing
                    if (m_tmaxCodesManager != null)
                        caseOptions.EnableCodeEditors = m_tmaxCodesManager.Lock(false);
                    else
                        caseOptions.EnableCodeEditors = false;
                }
                else
                {
                    //	We let the database handle collisions
                    caseOptions.EnableCodeEditors = true;
                }

                //	Disable the keyboard hook
                DisableTmaxKeyboard(true);

                //	Let the user set the options
                if ((bApplied = caseOptions.ShowDialog() == DialogResult.OK) == true)
                {
                    //	Update the case paths if necessary
                    if (caseOptions.PathsModified == true)
                        SetCaseFolders();

                    //	Make sure we have the correct case names
                    SetCaseName(caseOptions.CaseName);
                    SetShortCaseName(caseOptions.ShortCaseName);

                    //	Make sure the objections database has the correct case information
                    if (m_tmaxObjectionsDatabase != null)
                        m_tmaxObjectionsDatabase.SetCase(this);

                }// if(caseOptions.ShowDialog() == DialogResult.OK)

                //	Unlock the file if not using the database
                if (m_tmaxCodesManager != null)
                {
                    if (this.DxCaseCodes == null)
                        m_tmaxCodesManager.Unlock(m_tmaxCodesManager.Modified);
                    else
                        m_tmaxCodesManager.Modified = false; // Clear the flag if using the database
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetCaseOptions", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_CASE_OPTIONS_EX), Ex);
            }

            if (caseOptions != null)
            {
                try { caseOptions.Dispose(); }
                catch { };
            }

            //	Enable the keyboard hook
            DisableTmaxKeyboard(false);

            return bApplied;

        }// public bool SetCaseOptions()

        /// <summary>This method will perform the requested codes operation</summary>
        /// <param name="tmaxItems">The collection of items representing the owner records</param>
        /// <param name="tmaxModified">Collection to populate with items that represent the modified records</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        public bool SetCodes(CTmaxItems tmaxItems, CTmaxItems tmaxModified, CTmaxParameters tmaxParameters)
        {
            CTmaxParameter tmaxParameter = null;
            TmaxCodeActions eAction = TmaxCodeActions.Unknown;
            CDxMediaRecord dxOwner = null;
            CTmaxItems tmaxCodes = null;

            Debug.Assert(tmaxItems != null);
            Debug.Assert(tmaxParameters != null);

            //	What action are we supposed to take?
            if (tmaxParameters != null)
            {
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.CodesAction)) != null)
                {
                    try { eAction = (TmaxCodeActions)(tmaxParameter.AsInteger()); }
                    catch { }
                }

            }
            if (eAction == TmaxCodeActions.Unknown)
            {
                Debug.Assert(eAction != TmaxCodeActions.Unknown, "Invalid SetCodes action");
                return false;
            }

            //	Iterate the collection of event items to get the owner records
            foreach (CTmaxItem O in tmaxItems)
            {
                if ((dxOwner = (CDxMediaRecord)(O.GetMediaRecord())) == null) continue;

                //	Does this owner used meta tags?
                if (dxOwner.GetCodes(false) == null) continue;

                //	Get the collection containing the codes
                if (eAction == TmaxCodeActions.Add)
                    tmaxCodes = O.SourceItems;
                else
                    tmaxCodes = O.SubItems;

                //	Do we have anything to act on?
                if ((tmaxCodes != null) || (tmaxCodes.Count > 0))
                {
                    //	Perform the action
                    SetCodes(dxOwner, tmaxCodes, eAction, tmaxModified);
                }

            }// foreach(CTmaxItem O in tmaxItems)

            if (tmaxModified != null)
                return (tmaxModified.Count > 0);
            else
                return true;

        }// public bool SetCodes(CTmaxItem tmaxItem, CTmaxItems tmaxModified, CTmaxParameters tmaxParameters)

        /// <summary>This method will perform the requested codes operation</summary>
        /// <param name="tmaxItems">The item that identifies the target and the source to be added</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxResults">Collection to store the results of the operation</param>
        /// <returns>true if successful</returns>
        public bool SendToBinder(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.SourceItems != null);
            Debug.Assert(tmaxItem.SourceItems.Count > 0);

            return true;

        }// public bool SendToBinder(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)

        /// <summary>This method will perform a bulk update of fielded data codes specified by the user</summary>
        /// <param name="tmaxItems">The collection of items representing the source primary records selected by the user</param>
        /// <param name="tmaxModified">Collection to populate with items that represent the modified records</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        public bool BulkUpdate(CTmaxItems tmaxSelected, CTmaxItems tmaxModified, CTmaxParameters tmaxParameters)
        {
            CFUpdateCodes updateCodes = null;
            CDxPrimaries dxPrimaries = null;
            CDxPrimaries dxModified = null;
            bool bSuccessful = false;

            try
            {
                //	Don't bother if no primary records
                if ((m_dxPrimaries == null) || (m_dxPrimaries.Count == 0))
                    return false;

                //	Allocate the form to allow the user to set up the operation
                updateCodes = new CFUpdateCodes();
                SetHandlers(updateCodes.EventSource);
                updateCodes.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);

                //	Initialize the form
                updateCodes.CaseCodes = this.CaseCodes;
                updateCodes.PickLists = this.PickLists;

                //	Set the source collections
                updateCodes.Primaries = new CTmaxItems();
                foreach (CDxPrimary O in m_dxPrimaries)
                    updateCodes.Primaries.Add(new CTmaxItem(O));

                if ((m_dxFiltered != null) && (m_dxFiltered.Count > 0))
                {
                    updateCodes.Filtered = new CTmaxItems();
                    foreach (CDxPrimary O in m_dxFiltered)
                        updateCodes.Filtered.Add(new CTmaxItem(O));
                }

                if ((tmaxSelected != null) && (tmaxSelected.Count > 0))
                    updateCodes.Selected = tmaxSelected;

                //	Disable the keyboard hook
                DisableTmaxKeyboard(true);

                //	Let the user set the options
                if (updateCodes.ShowDialog() == DialogResult.OK)
                {
                    //	Initialize a collection for the primary source records
                    if (updateCodes.AllPrimaries == false)
                    {
                        if (updateCodes.Source != null)
                        {
                            dxPrimaries = new CDxPrimaries(this);
                            foreach (CTmaxItem O in updateCodes.Source)
                            {
                                if (O.IPrimary != null)
                                    dxPrimaries.AddList((CDxPrimary)(O.IPrimary));
                            }

                        }// if(updateCodes.Source != null)

                    }// if(updateCodes.AllPrimaries == true)

                    bSuccessful = BulkUpdate(updateCodes.Actions, dxPrimaries);

                    //	Does the user want to know which records were modified?
                    if ((tmaxModified != null) && (bSuccessful == true))
                    {
                        //	Did we use the master collection of primaries?
                        if (dxPrimaries == null)
                            dxModified = m_dxPrimaries;
                        else
                            dxModified = dxPrimaries;

                        foreach (CDxPrimary O in dxModified)
                            tmaxModified.Add(new CTmaxItem(O));

                    }// if((tmaxModified != null) && (bSuccessful == true))

                }// if(updateCodes.ShowDialog() == DialogResult.OK)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "BulkUpdate", this.ExBuilder.Message(ERROR_CASE_DATABASE_BULK_UPDATE_EX), Ex);
            }

            //	Clean up
            if (dxPrimaries != null)
                dxPrimaries.Clear();
            if (updateCodes.Actions != null)
                updateCodes.Actions.Clear();
            if (updateCodes.Primaries != null)
                updateCodes.Primaries.Clear();
            if (updateCodes.Filtered != null)
                updateCodes.Filtered.Clear();

            DisableTmaxKeyboard(false);
            return bSuccessful;

        }// public bool BulkUpdate(CTmaxItems tmaxSource, CTmaxItems tmaxModified, CTmaxParameters tmaxParameters)

        /// <summary>This method will set the Hidden property of the specified case codes</summary>
        /// <param name="tmaxItems">Event items used to identify case codes to be used</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        public bool HideCaseCodes(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
        {
            bool bSuccessful = false;

            //	Do we have case codes
            if (this.CaseCodes == null) return false;

            try
            {
                //	Did the caller specify the codes to act on?
                if ((tmaxItems != null) && (tmaxItems.Count > 0))
                {

                }
                else
                {
                    //	Allocate and initialize the form to allow the user to build the filter
                    CFHideCaseCodes hideCodes = new CFHideCaseCodes();
                    SetHandlers(hideCodes.EventSource);
                    hideCodes.CaseCodes = this.CaseCodes;

                    DisableTmaxKeyboard(true);

                    if (hideCodes.ShowDialog() == DialogResult.OK)
                    {
                        bSuccessful = true;
                    }

                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "HideCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_HIDE_CASE_CODES_EX), Ex);
            }

            DisableTmaxKeyboard(false);

            return bSuccessful;

        }// public bool HideCaseCodes(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

        /// <summary>This method will set the filter used to populate the Filtered record collection</summary>
        /// <param name="tmaxItems">Event items used to identify case codes to be used</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        public bool SetFilter(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
        {
            CTmaxParameter tmaxParameter = null;
            CTmaxFilter tmaxFilter = null;
            CTmaxCaseCode tmaxCaseCode = null;
            CTmaxPickItem tmaxPickList = null;
            long lFlags = (long)(TmaxSetFilterFlags.Advanced | TmaxSetFilterFlags.PromptUser);
            string strText = "";
            bool bSuccessful = true;
            bool bAdvanced = false;

            //	Do we have anything to filter?
            if (m_dxPrimaries == null) return false;
            if (m_dxPrimaries.Count == 0) return false;

            //	First check to see if the caller is filtering objections
            if (IsObjectionsCommand(tmaxParameters) == true)
            {
                return FilterObjections(tmaxItems, tmaxParameters);
            }
            else
            {
                //	Check the reentrant flag
                if (m_bSettingFilter == true) return false;
                else m_bSettingFilter = true;
            }

            //	Search the event items for a case code to use for the operation
            if ((tmaxItems != null) && (tmaxItems.Count > 0))
            {
                foreach (CTmaxItem O in tmaxItems)
                {
                    //	Is this a case code?
                    if (O.CaseCode != null)
                    {
                        tmaxCaseCode = O.CaseCode;

                        //	Did the caller provide a pick list?
                        if ((O.SubItems != null) && (O.SubItems.Count > 0))
                        {
                            foreach (CTmaxItem SubItem in O.SubItems)
                            {
                                if (SubItem.PickItem != null)
                                {
                                    tmaxPickList = SubItem.PickItem;
                                    break;
                                }

                            }// foreach(CTmaxItem SubItem in O.SubItems)

                        }// if((O.SubItems != null) && (O.SubItems.Count > 0))

                        break;

                    }// if(O.CaseCode != null)

                }// foreach(CTmaxItem O in tmaxItems)

            }// if((tmaxItems != null) && (tmaxItems.Count > 0))

            //	Did the caller provide a parameters collection?
            if (tmaxParameters != null)
            {
                //	Get the command flags if provided
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.SetFilterFlags)) != null)
                    lFlags = tmaxParameter.AsLong();

                //	Get the filter text if provided
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.SetFilterText)) != null)
                    strText = tmaxParameter.AsString();

            }// if(tmaxParameters != null)

            try
            {
                //	Are we supposed to prompt the user for the new parameters?
                if ((lFlags & (long)(TmaxSetFilterFlags.PromptUser)) != 0)
                {
                    //	Are we supposed to open the simple form?
                    if ((lFlags & (long)(TmaxSetFilterFlags.Advanced)) == 0)
                    {
                        //	Allocate and initialize the form to allow the user to build the filter
                        CFFastFilter fastFilter = new CFFastFilter();
                        SetHandlers(fastFilter.EventSource);
                        fastFilter.CaseCode = tmaxCaseCode;
                        fastFilter.PickList = tmaxPickList;
                        fastFilter.SearchText = strText;

                        if ((m_tmaxLastFilter != null) && (tmaxCaseCode == null))
                        {
                            fastFilter.SearchFields = m_tmaxLastFilter.FastFields;
                            if (fastFilter.SearchText.Length == 0)
                                fastFilter.SearchText = m_tmaxLastFilter.FastText;
                        }

                        DisableTmaxKeyboard(true);

                        if (fastFilter.ShowDialog() == DialogResult.Cancel)
                        {
                            bSuccessful = false;
                        }
                        else
                        {
                            //	Are we supposed to switch to the advanced form?
                            if (fastFilter.GoAdvanced == true)
                            {
                                bAdvanced = true;
                            }
                            else
                            {
                                //	Create a text filter
                                tmaxFilter = new CTmaxFilter();
                                tmaxFilter.SetFastFilter(fastFilter.CaseCode, fastFilter.PickList, fastFilter.SearchText, lFlags);
                                tmaxFilter.FastFields = fastFilter.SearchFields;
                                SetHandlers(tmaxFilter.EventSource);

                            }// if(fastFilter.GoAdvanced == true)

                        }// if(fastFilter.ShowDialog() == DialogResult.Cancel)

                    }
                    else
                    {
                        bAdvanced = true;	//	Open the advanced form

                    }// if((lFlags & (long)(TmaxSetFilterFlags.Advanced)) == 0)

                    //	Are we supposed to open the advanced form?
                    if (bAdvanced == true)
                    {
                        //	Allocate and initialize the form to allow the user to build the filter
                        CFFilterBuilder builder = new CFFilterBuilder();
                        SetHandlers(builder.EventSource);

                        builder.CaseCodes = this.CaseCodes;
                        builder.PickLists = this.PickLists;
                        builder.Filter = this.StationOptions.AdvancedFilter;

                        DisableTmaxKeyboard(true);

                        if (builder.ShowDialog() == DialogResult.Cancel)
                        {
                            bSuccessful = false;
                        }
                        else
                        {
                            //	Use the filter returned by the form
                            this.StationOptions.AdvancedFilter.Copy(builder.Filter);
                            tmaxFilter = this.StationOptions.AdvancedFilter;
                        }

                    }// if(bAdvanced == true)

                }
                else
                {
                    //	Create a new text filter if specified
                    if (strText.Length > 0)
                    {
                        tmaxFilter = new CTmaxFilter();
                        tmaxFilter.SetFastFilter(tmaxCaseCode, tmaxPickList, strText, lFlags);
                        SetHandlers(tmaxFilter.EventSource);
                    }
                    else
                    {
                        //	Do nothing ... This is a refresh
                    }

                }// if((lFlags & (long)(TmaxSetFilterFlags.PromptUser)) != 0)

                //	Rebuild the filtered collection
                if (bSuccessful == true)
                    bSuccessful = SetFiltered(tmaxFilter);

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetFilter", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_FILTER_EX), Ex);
            }
            finally
            {
                m_bSettingFilter = false;
                DisableTmaxKeyboard(false);
            }

            return bSuccessful;

        }// public bool SetFilter(CTmaxParameters tmaxParameters)

        /// <summary>This method will apply the existing media filter</summary>
        /// <param name="tmaxFilter">The filter to be applied to the primary record collection</param>
        /// <returns>true if successful</returns>
        public bool SetFiltered(CTmaxFilter tmaxFilter)
        {
            CDxPrimaries dxFiltered = null;

            //	Do we have anything to filter?
            if (m_dxPrimaries == null) return false;
            if (m_dxPrimaries.Count == 0) return false;

            try
            {
                //	Should we use the last filter?
                if (tmaxFilter == null)
                {
                    tmaxFilter = m_tmaxLastFilter;
                }
                else
                {
                    //	Make sure using the active collections
                    SetHandlers(tmaxFilter.EventSource);
                    tmaxFilter.CaseCodes = this.CaseCodes;
                    tmaxFilter.PickLists = this.PickLists;
                }

                //	Allocate a new collection for filtered records
                dxFiltered = new CDxPrimaries(this);

                if (dxFiltered.Fill(tmaxFilter) == true)
                {
                    //	Switch the collections
                    m_dxFiltered = dxFiltered;

                    //	Store this filter for refresh
                    m_tmaxLastFilter = tmaxFilter;

                    return true;
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetFiltered", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_FILTER_EX), Ex);
            }

            return false;

        }// public bool SetFiltered()

        /// <summary>This method attaches the local handlers to the specified event source</summary>
        /// <param name="tmaxEventSource">The source of the events to be handled</param>
        public void SetHandlers(CTmaxEventSource tmaxEventSource)
        {
            tmaxEventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
            tmaxEventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);

        }// public void SetHandlers(CTmaxEventSource tmaxEventSource)

        /// <summary>Call to assign the user defined path to the specified record's folder</summary>
        /// <param name="dxRecord">The primary/secondary record to be updated</param>
        /// <param name="strUserPath">The path to be assigned to the record</param>
        /// <returns>true if successful</returns>
        public bool SetUserPath(CDxMediaRecord dxRecord, string strUserPath)
        {
            string strRoot = "";
            string strRelative = "";
            string strDefault = "";
            string strCurrent = "";
            CTmaxAlias tmaxAlias = null;
            long lAliasId = 0;

            //	Must be a primary or secondary record
            if ((dxRecord.GetMediaLevel() != TmaxMediaLevels.Primary) &&
                (dxRecord.GetMediaLevel() != TmaxMediaLevels.Secondary))
                return false;

            //	Secondary records must be deposition segments
            if ((dxRecord.GetMediaLevel() == TmaxMediaLevels.Secondary) &&
                (dxRecord.GetParent().MediaType != TmaxMediaTypes.Deposition))
                return false;

            try
            {
                Debug.Assert(m_tmaxCaseOptions != null);
                Debug.Assert(m_tmaxCaseOptions.Aliases != null);
                if (m_tmaxCaseOptions == null) return false;
                if (m_tmaxCaseOptions.Aliases == null) return false;

                //	Get the current and default paths to the media folder
                GetFolderPaths(dxRecord, ref strCurrent, ref strDefault);

                //	Make adjustments to the user's path if necessary
                if ((strUserPath != null) && (strUserPath.Length > 0))
                {
                    //	If the user only provides a single letter assume they are
                    //	attempting to specify a drive
                    if (strUserPath.Length == 1)
                        strUserPath += ":\\";
                    else if (strUserPath.EndsWith("\\") == false)
                        strUserPath += "\\";

                    //	Has the caller actually specified the default folder?
                    if (String.Compare(strDefault, strUserPath, true) == 0)
                    {
                        //	This forces the switch to the default path
                        strUserPath = "";
                    }

                }// if((strUserPath != null) && (strUserPath.Length > 0))

                //	We have to make this check again because the path
                //	specified by the caller may have been cleared if it
                //	turned out to be the same as the default path
                if ((strUserPath != null) && (strUserPath.Length > 0))
                {
                    //	Has the user specified an absolute path?
                    if (System.IO.Path.IsPathRooted(strUserPath) == true)
                    {
                        //	Get the root drive/server for this path
                        strRoot = System.IO.Directory.GetDirectoryRoot(strUserPath);
                        if (strRoot.Length > 0)
                        {
                            strRelative = strUserPath.Remove(0, strRoot.Length);

                            if (strRoot.EndsWith("\\") == false)
                                strRoot += "\\";
                            if (strRelative.StartsWith("\\") == true)
                                strRelative = strRelative.Remove(0, 1);

                            strRoot = strRoot.ToLower();
                            strRelative = strRelative.ToLower();

                            if ((tmaxAlias = m_tmaxCaseOptions.Aliases.Find(strRoot)) == null)
                            {
                                tmaxAlias = m_tmaxCaseOptions.AddAlias(strRoot, true);
                            }

                            Debug.Assert(tmaxAlias != null);
                            lAliasId = tmaxAlias.Id;

                        }
                        else
                        {
                            //	This shouldn't happen but just in case ...
                            strRelative = strUserPath;

                        }// if(strRoot.Length > 0)

                    }
                    else
                    {
                        //	Assign relative path
                        strRelative = strUserPath;

                    }// if(System.IO.Path.IsPathRooted(strNewPath) == true)

                }// if((strUserPath != null) && (strUserPath.Length > 0))

                //	Set the record properties
                if (dxRecord.GetMediaLevel() == TmaxMediaLevels.Primary)
                {
                    ((CDxPrimary)dxRecord).AliasId = lAliasId;
                    ((CDxPrimary)dxRecord).RelativePath = strRelative;
                }
                else if (dxRecord.GetMediaLevel() == TmaxMediaLevels.Secondary)
                {
                    ((CDxSecondary)dxRecord).AliasId = lAliasId;
                    ((CDxSecondary)dxRecord).RelativePath = strRelative;
                }
                else
                {
                    return false;
                }

                //	It's all good...
                return true;

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetUserPath", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_USER_PATH_EX, strUserPath), Ex);
                return false;
            }

        }// public bool SetUserPath(CDxMediaRecord dxRecord, string strUserPath)

        /// <summary>This method will synchronize the barcode identifiers and display order numbers</summary>
        /// <param name="tmaxItems">A collection of items to be synchronized</param>
        /// <param name="tmaxModified">Collection in which to place items that were modified</param>
        /// <returns>true if one or more records were modified</returns>
        public bool Synchronize(CTmaxItems tmaxItems, CTmaxItems tmaxModified)
        {
            bool bModified = false;

            foreach (CTmaxItem O in tmaxItems)
            {
                if (Synchronize(O, tmaxModified) == true)
                    bModified = true;
            }

            return bModified;

        }//	public bool Synchronize(CTmaxItems tmaxItems, CTmaxItems tmaxModified)

        /// <summary>This method will perform an update of the database record associated with the specified item</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxResults">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        public bool Update(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {
            CTmaxParameter tmaxParameter = null;
            bool bFromFile = false;
            CDxMediaRecord dxMedia = null;
            bool bSuccessful = false;
            bool bSyncXml = false;

            Debug.Assert(tmaxItem != null);

            //	Are these objection records?
            if (IsObjectionsCommand(tmaxParameters) == true)
            {
                return UpdateObjections(tmaxItem, tmaxParameters, tmaxResults);
            }
            //	Is this a pick list item?
            else if (tmaxItem.DataType == TmaxDataTypes.PickItem)
            {
                CDxPickItem dxPickItem = null;

                //	Get the record being updated
                if (tmaxItem.PickItem != null)
                    dxPickItem = (CDxPickItem)(tmaxItem.PickItem.DxRecord);

                //	Use the owner collection to update the object
                if (dxPickItem.Collection != null)
                    bSuccessful = dxPickItem.Collection.Update(dxPickItem);
            }
            //	Is this a case code ?
            else if (tmaxItem.DataType == TmaxDataTypes.CaseCode)
            {
                if (tmaxItem.CaseCode != null)
                {
                    //	Are we using the database?
                    if (this.DxCaseCodes != null)
                    {
                        if (tmaxItem.CaseCode.DxRecord != null)
                            bSuccessful = this.DxCaseCodes.Update((CDxCaseCode)(tmaxItem.CaseCode.DxRecord));
                    }
                    else
                    {
                        //	Mark it as modified to save the file
                        m_tmaxCodesManager.Modified = true;
                    }

                }// if(tmaxItem.CaseCode != null)

            }
            else
            {
                if ((dxMedia = (CDxMediaRecord)GetRecordFromItem(tmaxItem)) == null) return false;
                if (dxMedia.Collection == null) return false;

                //	Did the caller provide a parameters collection?
                if (tmaxParameters != null)
                {
                    //	Get the parameters passed by the caller
                    if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.UpdateFromFile)) != null)
                        bFromFile = tmaxParameter.AsBoolean();
                    if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.SyncXml)) != null)
                        bSyncXml = tmaxParameter.AsBoolean();
                }

                //	Are we supposed to update from an external file?
                if (bFromFile == true)
                {
                    //	What type of record are we dealing with?
                    switch (dxMedia.MediaType)
                    {
                        case TmaxMediaTypes.Deposition:

                            if (UpdateDeposition((CDxPrimary)dxMedia) == false)
                            {
                                return false;
                            }
                            else
                            {
                                //	This lets the pane firing the event know that the
                                //	operation was successful
                                tmaxItem.ReturnItem = new CTmaxItem(dxMedia);
                            }
                            break;

                        default:

                            Debug.Assert(false, "Can only update depositions from file");
                            return false;
                    }

                }

                //	Update the record using its owner collection
                if ((bSuccessful = dxMedia.Collection.Update(dxMedia)) == true)
                {
                    //	Should we synchronize the associated XML designation?
                    if ((bSyncXml == true) && (dxMedia.MediaType == TmaxMediaTypes.Designation))
                        SyncXmlDesignation((CDxTertiary)dxMedia);

                }// if((bSuccessful = dxMedia.Collection.Update(dxMedia)) == true)

            }// if(tmaxItem.DataType == TmaxDataTypes.PickItem)

            if ((bSuccessful == true) && (tmaxResults != null))
                tmaxResults.Updated.Add(tmaxItem);

            return bSuccessful;

        }//	public bool Update(CTmaxItem tmaxItem)

        /// <summary>This method will update the XML designation to match the specified record</summary>
        /// <param name="dxDesignation">The designation record</param>
        /// <returns>true if successful</returns>
        public bool SyncXmlDesignation(CDxTertiary dxDesignation)
        {
            CXmlDesignation xmlDesignation = null;
            bool bSuccessful = false;

            try
            {
                //	Get the associated XML designation (create if necessary)
                if ((xmlDesignation = dxDesignation.GetXmlDesignation()) != null)
                {
                    dxDesignation.SetAttributes(xmlDesignation);
                    bSuccessful = xmlDesignation.Save();
                }

            }
            catch
            {
                bSuccessful = false;
            }

            return bSuccessful;

        }// public bool SyncXmlDesignation(CDxTertiary dxDesignation)

        /// <summary>This method is called to trim the database contents</summary>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if no successful</returns>
        public bool Trim(CTmaxParameters tmaxParameters)
        {
            bool bSuccessful = false;

            Debug.Assert(m_tmaxTrimManager != null);
            if (m_tmaxTrimManager == null) return false;

            try
            {
                if ((m_dxPrimaries != null) && (m_dxPrimaries.Count > 0) && (m_tmaxStationOptions != null))
                {
                    //	Prepare the manager for a new operation
                    m_tmaxTrimManager.Database = this;
                    m_tmaxTrimManager.Options = m_tmaxStationOptions.TrimOptions;

                    //	Execute the operation
                    if (m_tmaxTrimManager.Initialize(tmaxParameters) == true)
                    {
                        bSuccessful = m_tmaxTrimManager.Trim();

                        //	Clean up
                        m_tmaxTrimManager.Clear();
                    }

                }// if((m_dxPrimaries != null) && (m_dxPrimaries.Count > 0) && (m_tmaxStationOptions != null))

            }
            catch (System.Exception Ex)
            {
                FireError(this, "Trim", this.ExBuilder.Message(ERROR_CASE_DATABASE_TRIM_EX, m_tmaxStationOptions.TrimOptions.CaseFolder), Ex);
            }
            finally
            {
            }

            return bSuccessful;

        }// public bool Trim()

        /// <summary>This method is called to validate the entire database contents</summary>
        /// <returns>true if no successful</returns>
        public bool Validate()
        {
            CFValidateOptions validateOptions = null;
            string strMsg = "";
            bool bSuccessful = false;

            Debug.Assert(m_tmaxValidateOptions != null);
            Debug.Assert(m_dxPrimaries != null);
            if (m_tmaxValidateOptions == null) return false;
            if (m_dxPrimaries == null) return false;

            //	Clear the flags
            m_bConvertToCodes = false;
            m_bCreatePickLists = false;
            m_bValidateMoveConfirmed = false;

            try
            {
                validateOptions = new CFValidateOptions();

                validateOptions.ValidateOptions = m_tmaxValidateOptions;
                validateOptions.ShowTransferCodes = (m_bCodesConversionRequired == false);

                //	Let the user set the options
                if (validateOptions.ShowDialog() == DialogResult.Cancel)
                    return false; // No validation performed

                //	Prompt the user for confirmation if we are supposed to move the files
                if (m_bRequires601Update)
                {
                    strMsg = "During validation, files for treatments, designations, and clips will be moved ";
                    strMsg += "to match the improved storage structure defined in version 6.0.1. There should not ";
                    strMsg += "be any other users connected to this case while the files are being moved.\n\n";

                    strMsg += "After the files are moved, ALL users MUST have the latest TrialMax update installed. ";
                    strMsg += "Otherwise they will not be able to access files that have been moved. Assuming no errors, this update";
                    strMsg += " only has to be performed once for each case created prior to version 6.0.1\n\n";

                    strMsg += "Performing this upgrade is not required. TrialMax will function normally without moving ";
                    strMsg += "the files. However, if you're certain that all users are running the latest update, ";
                    strMsg += "it's recommended that you allow the files to be moved.\n\n";

                    strMsg += "Is it OK to perform this upgrade?";

                    m_bValidateMoveConfirmed = (MessageBox.Show(strMsg, "Move Files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);

                }// if(m_bRequires601Update)

                // Enable codes only if 601 update is not required
                if (m_bCodesConversionRequired == true)
                {
                    if (m_bRequires601Update == true)
                    {
                        strMsg = "This database should be upgraded to use fielded data but the file relocations should be performed first";
                        strMsg += "Run Validate again after performing the file transfer.";
                        MessageBox.Show(strMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                    else
                    {
                        strMsg = "This case does not currently support the use of fielded data. ";
                        strMsg += "It can be upgraded to support fielded data as part of this validation.\n\n";

                        strMsg += "Beware, once upgraded, ALL users MUST have the latest TrialMax update installed. ";
                        strMsg += "The primary media properties of Description, Exhibit, and Admitted will be converted ";
                        strMsg += "to data fields. Users of older TrialMax versions will no longer be able to access these properties\n\n";

                        strMsg += "Performing this upgrade is not required. TrialMax will function normally without the ";
                        strMsg += "upgrade. Operations related to fielded data will remain disabled. \n\n";

                        strMsg += "Is it OK to perform this upgrade?";
                        if (MessageBox.Show(strMsg, "Enable Fielded Data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            m_bConvertToCodes = true;
                    }

                }// if(m_bCodesConversionRequired == true)

                //	Do we need to add the pick list support?
                else if ((this.CaseCodes != null) && (this.DxPickLists == null))
                {
                    strMsg = "This case does not currently support the use of pick lists for fielded data. ";
                    strMsg += "It can be upgraded to support pick lists as part of this validation.\n\n";

                    strMsg += "Beware, once upgraded, ALL users MUST have the latest TrialMax update installed. ";
                    strMsg += "Users of older TrialMax versions will no longer be able to manage fielded data\n\n";

                    strMsg += "Performing this upgrade is not required. TrialMax will function normally without the ";
                    strMsg += "upgrade but pick lists for fielded data will not be available. \n\n";

                    strMsg += "Is it OK to perform this upgrade?";
                    if (MessageBox.Show(strMsg, "Enable Pick Lists?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        m_bCreatePickLists = true;
                }

                //	Create the progress form before launching the thread
                CreateValidateProgress();

                try
                {
                    //	Run the registration in it's own thread
                    Thread validateThread = new Thread(new ThreadStart(this.ValidateThreadProc));
                    validateThread.Start();
                }
                catch (System.Exception Ex)
                {
                    FireError(this, "Validate", this.ExBuilder.Message(ERROR_CASE_DATABASE_VALIDATE_THREAD_EX), Ex);
                    return false;
                }

                //	Open the progress form
                if (m_cfValidateProgress != null)
                {
                    //	Show modal to prevent returning until finished or canceled
                    if (m_cfValidateProgress.ShowDialog() == DialogResult.Cancel)
                    {
                        m_bValidateCancelled = true;
                    }

                    //	Clean up
                    m_cfValidateProgress.Dispose();
                    m_cfValidateProgress = null;

                }// if(m_cfValidateProgress != null)

                bSuccessful = true;
            }
            catch
            {
            }

            //	Be sure to clear this flag just in case the conversion failed
            m_bConvertToCodes = false;
            m_bCreatePickLists = false;

            return bSuccessful;

        }// public bool Validate()

        /// <summary>This method is called to validate the specified binder</summary>
        private void Validate(CDxBinderEntry dxBinder)
        {
            bool bFilled = false;

            Debug.Assert(dxBinder != null);

            //	Update the status text
            SetValidateStatus("Validating binder: " + dxBinder.Name + " ......");

            //	Make sure the child collection is full
            if ((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
            {
                dxBinder.Fill();
                bFilled = true;
            }

            //	Check the contents to make sure all records are valid
            foreach (CDxBinderEntry O in dxBinder.Contents)
            {
                //	Has the user cancelled the operation
                if (GetValidateCancelled() == true)
                    break;

                //	Is this a media record?
                if (O.IsMedia() == true)
                {
                    //	Is the source record valid
                    if ((O.GetSource(true) == null) || (IsValidRecord(O.GetSource(true)) == false))
                    {
                        AddValidationError(dxBinder.Name, "Invalid binder reference: " + O.Name, true);
                    }

                }
                else
                {
                    Validate(O);
                }

            }

            //	Clear the contents if we filled them
            if (bFilled == true)
                dxBinder.Contents.Clear();

        }// private void Validate(CDxPrimary dxPrimary)

        /// <summary>Call to verify a path the user is attempting to assign to the relative path field for the specified record</summary>
        /// <param name="dxRecord">The primary/secondary record being modified</param>
        /// <param name="strUserPath">The path the user wants to put in the relative path field</param>
        /// <param name="bPrompt">true to prompt the user for confirmation</param>
        /// <returns>true if successful</returns>
        public bool VerifyUserPath(CDxMediaRecord dxRecord, string strUserPath, bool bPrompt)
        {
            string strCurrent = "";
            string strTarget = "";
            string strDefault = "";
            string strMsg = "";
            bool bExists = false;

            Debug.Assert(m_tmaxCaseOptions != null);
            Debug.Assert(m_tmaxCaseOptions.Aliases != null);
            Debug.Assert(dxRecord != null);
            if (m_tmaxCaseOptions == null) return false;
            if (m_tmaxCaseOptions.Aliases == null) return false;
            if (dxRecord == null) return false;

            //	Get the current and default paths to the folder
            if (GetFolderPaths(dxRecord, ref strCurrent, ref strDefault) == false)
                return false;

            //	Is the user setting a path?
            if (strUserPath.Length > 0)
            {
                //	Is the user specifying an absolute path?
                if (System.IO.Path.IsPathRooted(strUserPath) == true)
                {
                    //	If the user is attempting to link the path must contain a 
                    //	valid drive / server specification
                    if (CTmaxAlias.PathToAlias(strUserPath, ref strTarget) == false)
                    {
                        strMsg = "Unable to set linked path. The specified path does not contain a valid drive or UNC server:\n\n";
                        strMsg += strUserPath;

                        MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                }
                else
                {
                    strTarget = strDefault;
                    if ((strTarget.Length > 0) && (strTarget.EndsWith("\\") == false))
                        strTarget += "\\";
                    strTarget += strUserPath;
                }

            }
            else
            {
                //	If the user is clearing the relative path the target becomes the default
                strTarget = strDefault;
            }

            //	Is the path actually changing
            if (String.Compare(strCurrent, strTarget, true) == 0)
                return true; // Nothing to change

            //	Does the target folder exist?
            if (strTarget.Length > 0)
                bExists = System.IO.Directory.Exists(strTarget);

            //	Do we need to prompt the user for confirmation first?
            if ((bPrompt == true) || (bExists == false))
            {
                strMsg = "You are attempting to change the folder for this record's media files !\n\n";
                strMsg += ("FROM: " + strCurrent + "\n\n");

                if (strTarget.Length > 0)
                    strMsg += ("TO:    " + strTarget + "\n\n");
                else
                    strMsg += ("TO:    " + strDefault + "\n\n");

                if (bExists == false)
                    strMsg += "WARNING: The specified folder does not exist\n\n";

                strMsg += "Are you sure you want to continue?\n";

                if (MessageBox.Show(strMsg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return false;
            }

            //	It's all good...
            return true;

        }// public bool VerifyUserPath(CDxMediaRecord dxRecord, string strUserPath, bool bPrompt)

        #endregion Public Methods

        #region Protected Methods

        #endregion Protected Methods

        #region Private Methods

        /// <summary>This method will add an entry to the binders table using the specified event item</summary>
        /// <param name="dxParent">The parent binder</param>
        /// <param name="tmaxItem">Event item to identify the new entry</param>
        /// <param name="dxInsertAt">Insertion point for the new entry</param>
        /// <param name="bBefore">true to insert before the specified insertion point</param>
        /// <param name="tmaItems">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        private CDxBinderEntry AddBinderEntry(CDxBinderEntry dxParent, CTmaxItem tmaxItem, CDxBinderEntry dxInsertAt, bool bBefore, CTmaxItems tmaxAdded)
        {
            CDxMediaRecord dxRecord = null;
            CDxBinderEntry dxEntry = null;
            CFAddBinder cfBinder = null;
            CTmaxItem tmaxEntry = null;

            Debug.Assert(tmaxItem != null);

            //	Are we supposed to add a new media entry?
            if (tmaxItem.MediaType != TmaxMediaTypes.Unknown)
            {
                //	Do we need to add a new primary record?
                if ((dxRecord = (CDxMediaRecord)tmaxItem.GetMediaRecord()) == null)
                {
                    if ((dxRecord = AddPrimary(tmaxItem.MediaType)) != null)
                    {
                        //	Add to the caller's collection
                        if (tmaxAdded != null)
                            tmaxAdded.Add(new CTmaxItem(dxRecord));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    //	Use the source record if this is a link
                    if (dxRecord.MediaType == TmaxMediaTypes.Link)
                    {
                        dxRecord = ((CDxQuaternary)dxRecord).GetSource();
                    }

                    if (dxRecord == null)
                        return null;
                }

                //	Create the new binder entry
                dxEntry = new CDxBinderEntry();
                dxEntry.Source = dxRecord;
                dxEntry.Name = this.GetUniqueId(dxRecord);
                dxEntry.Attributes |= (long)TmaxBinderAttributes.IsMedia;

            }
            else // if(tmaxItem.MediaType != TmaxMediaTypes.Unknown)
            {
                //	Are we copying an existing binder?
                if (tmaxItem.IBinderEntry != null)
                {
                    //	Create a new binder
                    dxEntry = new CDxBinderEntry();
                    dxEntry.Name = ((CDxBinderEntry)(tmaxItem.IBinderEntry)).Name;
                    dxEntry.Description = ((CDxBinderEntry)(tmaxItem.IBinderEntry)).Description;
                }
                //	Are we using a source folder to create the binder?
                else if (tmaxItem.SourceFolder != null)
                {
                    //	Create a new binder using the folder name
                    dxEntry = new CDxBinderEntry();
                    if (tmaxItem.SourceFolder.Name.Length > 0)
                        dxEntry.Name = tmaxItem.SourceFolder.Name;
                    else
                        dxEntry.Name = tmaxItem.SourceFolder.Path;

                    dxEntry.Description = tmaxItem.SourceFolder.Path;
                }
                //	Are we using a source file to create the binder?
                else if (tmaxItem.SourceFile != null)
                {
                    //	Create a new binder using the filename
                    dxEntry = new CDxBinderEntry();
                    if (tmaxItem.SourceFile.Name.Length > 0)
                        dxEntry.Name = System.IO.Path.GetFileNameWithoutExtension(tmaxItem.SourceFile.Name);
                    else
                        dxEntry.Name = tmaxItem.SourceFile.Path;

                    long parentId = 0;
                    if (dxParent != null) // If parent is null, then the new binder added is not a child of anyone
                    {
                        parentId = dxParent.AutoId; // If parent is not null, then the new binder is a child of an already existing binder
                    }
                    // Search in database if there already exists a binder with the same name and same parent
                    GetDataReader("SELECT * FROM BinderEntries WHERE ParentId = " + parentId + " AND Name = \"" + System.IO.Path.GetFileNameWithoutExtension(dxEntry.Name) + "\"");

                    if (m_oleDbReader.HasRows) // A binder already exists if this condition is true
                    {
                        short appendedNumber = 1;
                        while (true) // Append integer to the binder name and check if it exists already. Keep trying until we succeed
                        {
                            GetDataReader("SELECT * FROM BinderEntries WHERE ParentId = " + parentId + " AND Name = \"" + dxEntry.Name + "-" + appendedNumber.ToString("00") + "\"");
                            if (!m_oleDbReader.HasRows)
                            {
                                dxEntry.Name += "-" + appendedNumber.ToString("00");
                                break;
                            }
                            appendedNumber++;
                        }
                    }
                    dxEntry.Description = ("Imported from " + tmaxItem.SourceFile.Path);
                }
                else
                {
                    //	Prompt the user for the binder information
                    cfBinder = new CFAddBinder();

                    DisableTmaxKeyboard(true);

                    if (cfBinder.ShowDialog() == DialogResult.OK)
                    {
                        //	Create a new binder
                        dxEntry = new CDxBinderEntry();
                        dxEntry.Name = cfBinder.BinderName;
                    }
                    else
                    {
                        DisableTmaxKeyboard(false);
                        return null;
                    }

                }

            } // if(tmaxItem.MediaType != TmaxMediaTypes.Unknown)

            //	Set the parent
            dxEntry.SetParent(dxParent);

            //	Add to the binder heirarchy in the appropriate position
            if (InsertBinder(dxEntry, dxInsertAt, bBefore) == false)
            {
                DisableTmaxKeyboard(false);
                return null;
            }

            //	Add an item to the caller's collection
            tmaxEntry = new CTmaxItem();
            tmaxEntry.IBinderEntry = (ITmaxMediaRecord)dxEntry;
            tmaxEntry.ParentItem = new CTmaxItem(dxParent);

            if (tmaxAdded != null)
                tmaxAdded.Add(tmaxEntry);

            //	Did we just add a binder?
            if (dxEntry.IsMedia() == false)
            {
                //	Should we add children to the new binder?
                if ((tmaxItem.SourceItems != null) && (tmaxItem.SourceItems.Count > 0))
                {
                    foreach (CTmaxItem O in tmaxItem.SourceItems)
                    {
                        //	Add a new entry for this source item
                        //
                        //	NOTE:	We switch the collection in which to store items
                        //			for each new record so that the heirarchial information
                        //			is maintained
                        AddBinderEntry(dxEntry, O, null, false, tmaxEntry.SubItems);
                    }
                }

            }

            DisableTmaxKeyboard(false);

            return dxEntry;

        }// private CDxBinderEntry AddBinderEntry(CDxBinderEntry dxParent, CTmaxItem tmaxItem, CDxBinderEntry dxInsertAt, bool bBefore, CTmaxItems tmaxAdded)

        /// <summary>This method will perform the requested code operation</summary>
        /// <param name="dxOwner">The record that owns the codes</param>
        /// <param name="tmaxCodes">Collection of event items that identify the code records</param>
        /// <param name="eAction">The action to be performed</param>
        /// <param name="tmaxModified">Collection to populate with items that represent the modified records</param>
        /// <returns>true if the owner is modified</returns>
        private bool SetCodes(CDxMediaRecord dxOwner, CTmaxItems tmaxCodes, TmaxCodeActions eAction, CTmaxItems tmaxModified)
        {
            CDxCode dxCode = null;
            CTmaxItem tmaxOwner = null;

            Debug.Assert(dxOwner != null);
            if (dxOwner == null) return false;
            Debug.Assert(dxOwner.GetCodes(false) != null);
            if (dxOwner.GetCodes(false) == null) return false;

            //	Create an event item to represent the owner
            tmaxOwner = new CTmaxItem(dxOwner);

            //	Retrieve each of the codes
            foreach (CTmaxItem O in tmaxCodes)
            {
                if ((dxCode = (CDxCode)(O.ICode)) == null) continue;

                //	Make sure the owner has been assigned to the code
                dxCode.SetOwner(dxOwner);

                //	What are we supposed to do?
                switch (eAction)
                {
                    case TmaxCodeActions.Add:

                        if (dxOwner.Add(dxCode) != null)
                            tmaxOwner.SubItems.Add(new CTmaxItem(dxCode));
                        break;

                    case TmaxCodeActions.Update:

                        if (dxOwner.Update(dxCode) == true)
                            tmaxOwner.SubItems.Add(new CTmaxItem(dxCode));
                        break;

                    case TmaxCodeActions.Delete:

                        if (dxOwner.Delete(dxCode) == true)
                            tmaxOwner.SubItems.Add(new CTmaxItem(dxCode));
                        break;

                }// switch(eAction)

            }// foreach(CTmaxItem O in tmaxCodes)

            //	Did we modify any codes?
            if (tmaxOwner.SubItems.Count > 0)
            {
                if (tmaxModified != null)
                    tmaxModified.Add(tmaxOwner);
                else
                    tmaxOwner.SubItems.Clear();

                //	Update the time stamps on the owner record
                dxOwner.Collection.Update(dxOwner);

                //	Notify the system that the owner has been modified
                FireInternalUpdate(dxOwner);

                return true;

            }
            else
            {
                return false;
            }

        }// private bool SetCodes(CDxMediaRecord dxOwner, CTmaxItems tmaxCodes, TmaxCodeActions eAction)

        /// <summary>Call to open the database</summary>
        /// <param name="strFolder">Folder containing the case database</param>
        /// <param name="bCheckVersion">True to verify the version information stored in the database</param>
        /// <returns>true if successful</returns>
        private bool Open(string strFolder, bool bCheckVersion)
        {
            string strFileSpec = "";

            //	Clear the version specific flags
            //
            //	NOTE:	We do this here because the method gets called by
            //			both Open() and Create()
            m_iMaxLineTimeIndex = -1;
            m_iSiblingIdIndex = -1;
            m_iCodesIndexValuePickList = -1;
            m_iCodesIndexModifiedBy = -1;
            m_iCodesIndexModifiedOn = -1;
            m_bRequires601Update = false;
            m_strDefaultClipsFolder = CASE_DATABASE_CLIPS_FOLDER_601;

            //	Get the fully qualified path for the database file
            strFileSpec = GetFileSpec(strFolder);

            //	Close the existing database connection
            Close();

            //	Connect to the data provider
            if (Open(strFileSpec) == false) return false;

            //	Save the new folder location
            m_strFolder = strFolder.ToLower();

            // To check either SpareText & SpareNumber columns exist
            int spareNumberIndex, spareTextIndex;
            spareTextIndex = GetColumnIndex(CDxBinderEntries.TABLE_NAME, CDxBinderEntries.eFields.SpareText.ToString());
            if (spareTextIndex < 0)
            {
                if (AddColumn(CDxBinderEntries.TABLE_NAME, CDxBinderEntries.eFields.SpareText.ToString(), "TEXT(255)", null) == true)
                    spareTextIndex = GetColumnIndex(CDxBinderEntries.TABLE_NAME, CDxBinderEntries.eFields.SpareText.ToString());
            }

            spareNumberIndex = GetColumnIndex(CDxBinderEntries.TABLE_NAME, CDxBinderEntries.eFields.SpareNumber.ToString());
            if (spareNumberIndex < 0)
            {
                if (AddColumn(CDxBinderEntries.TABLE_NAME, CDxBinderEntries.eFields.SpareNumber.ToString(), "Integer", null) == true)
                    spareNumberIndex = GetColumnIndex(CDxBinderEntries.TABLE_NAME, CDxBinderEntries.eFields.SpareNumber.ToString());
            }

            //	Open the case options configuration file
            strFileSpec = System.IO.Path.ChangeExtension(strFileSpec, "xml");
            m_tmaxCaseOptions.CaseFolder = m_strFolder;
            m_tmaxCaseOptions.Initialize(strFileSpec, null);

            //	Make sure we have the default subfolders
            SetCaseFolders();

            //	Open the record collections
            return OpenCollections(bCheckVersion);

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
                m_dxDetails = new CDxDetails(this);
                m_dxUsers = new CDxUsers(this);
                m_dxPrimaries = new CDxPrimaries(this);
                m_dxTranscripts = new CDxTranscripts(this);
                m_dxRootBinder = new CDxBinderEntry();
                m_dxRootBinder.Contents.Database = this;
                m_dxHighlighters = new CDxHighlighters(this);
                m_dxBarcodeMap = new CDxBarcodes(this);

                //	Open the record collections
                while (bSuccessful == false)
                {
                    if (m_dxDetails.Open() == false)
                        break;

                    if (bCheckVersion == true)
                    {
                        if (CheckVersions() == false)
                            break;
                    }

                    if (m_dxUsers.Open() == false)
                        break;

                    if (m_dxPrimaries.Open() == false)
                        break;

                    if (m_dxTranscripts.Open() == false)
                        break;

                    if (m_dxRootBinder.Contents.Open() == false)
                        break;

                    if (m_dxHighlighters.Open() == false)
                        break;

                    if (m_dxBarcodeMap.Open() == false)
                        break;

                    //	Get the index of the MaxLineTime column if it exists
                    m_iMaxLineTimeIndex = GetColumnIndex(CDxExtents.TABLE_NAME, CDxExtents.eFields.MaxLineTime.ToString());

                    //	Get the index of the SiblingID column if it exists
                    m_iSiblingIdIndex = GetColumnIndex(CDxTertiaries.TABLE_NAME, CDxTertiaries.eFields.SiblingId.ToString());

                    //	We're finished
                    bSuccessful = true;

                }// while(bSuccessful == false)

            }
            catch (OleDbException oleEx)
            {
                FireError(this, "OpenCollections", this.ExBuilder.Message(ERROR_CASE_DATABASE_OPEN_COLLECTIONS_EX), oleEx);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "OpenCollections", this.ExBuilder.Message(ERROR_CASE_DATABASE_OPEN_COLLECTIONS_EX), Ex);
            }

            //	Clean up if not successful
            if (bSuccessful == false)
            {
                Close();
            }

            return bSuccessful;

        }// private bool OpenCollections(bool bCheckVersion)

        /// <summary>This method is called to set and store the database details</summary>
        /// <returns>True if successful</returns>
        private bool SetDetails()
        {
            CTmdataVersion dbVer = new CTmdataVersion();

            Debug.Assert(m_dxDetail == null);
            Debug.Assert(m_dxDetails != null);

            if (m_dxDetails != null)
            {
                //	Allocate a new transfer object
                if ((m_dxDetail = new CDxDetail()) != null)
                {
                    m_dxDetail.UniqueId = System.Guid.NewGuid().ToString();
                    m_dxDetail.Name = System.IO.Path.GetFileName(this.Folder);
                    m_dxDetail.Version = dbVer.Version;
                    try
                    {
                        m_dxDetail.Version = string.IsNullOrEmpty(m_tmaxProductManager.TmaxManagerVersion) ? dbVer.Version : m_tmaxProductManager.TmaxManagerVersion;
                    }
                    catch { }

                    m_dxDetail.CreatedOn = System.DateTime.Now;

                    return (m_dxDetails.Add(m_dxDetail) != null);
                }

            }

            return false;

        }// private bool SetDetails()

        /// <summary>This method is called to get the TmaxManager Version</summary>
        /// <returns>TmaxManager Version xyz format</returns>
        private long GetTmaxManagerVersion()
        {
            string[] TmManagerVer = m_tmaxProductManager.TmaxManagerVersion.Split('.');
            try
            {
                return GetPackedVer(Convert.ToInt32(TmManagerVer.GetValue(0)), Convert.ToInt32(TmManagerVer.GetValue(1)), Convert.ToInt32(TmManagerVer.GetValue(2)));
            }
            catch { }
            return 0;

        }// private long GetTmaxManagerVersion()

        /// <summary>This method is called to get the TmaxManager Version</summary>
        /// <returns>TmaxManager Version x.y.z format</returns>
        private string GetTmaxManagerVersionString()
        {
            try
            {
                return m_tmaxProductManager.TmaxManagerVersion.Substring(0, m_tmaxProductManager.TmaxManagerVersion.LastIndexOf('.'));
            }
            catch { }
            return "0";

        }// private long GetTmaxManagerVersionString()

        /// <summary>This method is called to check the version information stored in the database</summary>
        /// <returns>True if OK to continue</returns>
        private bool CheckVersions()
        {
            long lDatabaseVer = 0;
            long lTmdataVer = 0;
            long lTmManagerVer = 0;
            bool bSyncVersion = false;

            Debug.Assert(m_dxDetails != null);
            if (m_dxDetails == null) return false;

            //	Populate the details collection
            m_dxDetails.Fill();
            if (m_dxDetails.Count > 0)
            {
                m_dxDetail = m_dxDetails[0];
            }
            else
            {
                //	We should never reach this point but just in case
                //	we do we are going to add the details and just
                //	cross our fingers that everything is OK
                return SetDetails();
            }

            //	Get the packed version identifiers
            lTmdataVer = GetPackedVer(true);
            lDatabaseVer = GetPackedVer(false);
            lTmManagerVer = GetTmaxManagerVersion();

            //	Was the database created with a newer version of the application?
            if (lDatabaseVer > lTmManagerVer)
            {
                //	Show the warning messages and let the user decide if we should continue
                if (ShowVersionWarnings() == false)
                    return false;
            }

            //	Was the database created with an older version of the application
            else if (lTmManagerVer > lDatabaseVer)
            {
                //	Was this database created with a version earlier than 6.0.1?
                if (lDatabaseVer < GetPackedVer(6, 0, 1))
                {
                    m_bRequires601Update = true;

                    //	We changed the name of the clips folder in 6.0.1
                    //	because now all it contains is clips
                    m_strDefaultClipsFolder = CASE_DATABASE_CLIPS_FOLDER_600;
                    SetCasePath(TmaxCaseFolders.Clips, null, false);
                }

                //	Did we add the short case name column?
                if ((m_dxDetails.AddedShortName == true) && (lDatabaseVer <= 6300))
                {
                    bSyncVersion = true;
                    AddWarning("6.3.1", "Modified to use case identifiers when importing and exporting objections");
                }

                //	Do we need to add the SiblingId column?
                m_iSiblingIdIndex = GetColumnIndex(CDxTertiaries.TABLE_NAME, CDxTertiaries.eFields.SiblingId.ToString());
                if (m_iSiblingIdIndex < 0)
                {
                    if (AddColumn(CDxTertiaries.TABLE_NAME, CDxTertiaries.eFields.SiblingId.ToString(), "TEXT(255)", "") == true)
                        m_iSiblingIdIndex = GetColumnIndex(CDxTertiaries.TABLE_NAME, CDxTertiaries.eFields.SiblingId.ToString());

                    if (m_iSiblingIdIndex >= 0)
                    {
                        bSyncVersion = true;
                        AddWarning("6.3.4", "Added support for split screen treatments");
                    }

                }// if(m_iSiblingIdIndex < 0)

                //	Do we need to sync the database version?
                if ((bSyncVersion == true) && (m_bRequires601Update == false))
                {
                    SyncCaseVersion();
                }

            }// if(lTmdataVer < lDatabaseVer)

            return true;

        }// private bool CheckVersions()

        /// <summary>This method is called to get the list of warnings stored in the database</summary>
        /// <returns>The list of warnings</returns>
        private ArrayList GetWarnings()
        {
            CDxWarnings dxWarnings = null;
            ArrayList aWarnings = null;
            long lTmdataVer = 0;

            try
            {
                if (this.IsConnected == false) return null;

                //	Make sure we have a warnings table
                if (this.HasTable(CDxWarnings.TABLE_NAME) == false) return null;

                //	Open the warnings collection
                dxWarnings = new CDxWarnings(this);
                if (dxWarnings.Open() == false) return null;

                //	Fill the collection
                dxWarnings.Fill();

                if (dxWarnings.Count > 0)
                {
                    //	Create a collection to hold the warning messages
                    aWarnings = new ArrayList();

                    //	Get the version of this assembly
                    lTmdataVer = GetPackedVer(true);

                    foreach (CDxWarning O in dxWarnings)
                    {
                        if (O.GetPackedVersion() > lTmdataVer)
                            aWarnings.Add(O);
                    }

                }// if(dxWarnings.Count > 0)

            }
            catch
            {

            }
            finally
            {
                if (dxWarnings != null)
                {
                    dxWarnings.Close();
                    dxWarnings = null;
                }

            }

            if ((aWarnings != null) && (aWarnings.Count > 0))
                return aWarnings;
            else
                return null;

        }// private ArrayList GetWarnings()

        /// <summary>This method is called to add a warning to the database</summary>
        /// <returns>True if successful</returns>
        private bool AddWarning(string strVersion, string strMessage)
        {
            CDxWarnings dxWarnings = null;
            bool bSuccessful = false;

            try
            {
                if (this.IsConnected == false) return false;

                //	Make sure we have a warnings table
                if (this.HasTable(CDxWarnings.TABLE_NAME) == false)
                    return false;

                //	Open the warnings collection
                dxWarnings = new CDxWarnings(this);
                if (dxWarnings.Open() == false) return false;

                //	Add the warning
                bSuccessful = (dxWarnings.Add(strVersion, strMessage) != null);

            }
            catch
            {
            }

            return bSuccessful;

        }// private bool AddWarning(string strVersion, string strMessage)

        /// <summary>This method is called to add a warning for the current version to the database</summary>
        /// <param name="strMessage">The warning message to be added</param>
        /// <returns>True if successful</returns>
        private bool AddWarning(string strMessage)
        {
            bool bSuccessful = false;

            try
            {
                CTmdataVersion tmdataVer = new CTmdataVersion();
                bSuccessful = AddWarning(tmdataVer.ShortVersion, strMessage);
            }
            catch
            {
            }

            return bSuccessful;

        }// private bool AddWarning(string strMessage)

        /// <summary>This method is called to display warnings when it's found that the database was created with a newer version of the assembly</summary>
        /// <returns>True if OK to continue</returns>
        private bool ShowVersionWarnings()
        {
            CFVersionWarnings verWarnings = null;
            ArrayList aWarnings = null;
            string TmaxManagerVersion = GetTmaxManagerVersionString();
            try
            {
                //	Get any warning messages stored in the database
                try { aWarnings = GetWarnings(); }
                catch { };

                verWarnings = new CFVersionWarnings();
                verWarnings.DatabaseVersion = GetVersionString(false);
                verWarnings.AssemblyVersion = TmaxManagerVersion == "0" ? GetVersionString(true) : TmaxManagerVersion;
                verWarnings.Warnings = aWarnings;

                verWarnings.ShowDialog();
            }
            catch
            {
            }

            return true;

        }// private bool ShowVersionWarnings()

        /// <summary>This method is called to set the set the primary media properties using the specified folder</summary>
        /// <param name="tmaxSource">The source folder used to register the object</param>
        /// <param name="dxPrimary">The primary media object being registered</param>
        /// <returns>true if successful</returns>
        private bool SetProperties(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)
        {
            string strAssign = "";

            //	Get the text used to populate the primary fields
            strAssign = GetAssignmentText(tmaxSource, false);

            //	Set the folder assignment options
            if (m_tmaxRegisterOptions.GetFolderAssignment(RegFolderAssignments.MediaName) == true)
                dxPrimary.Name = AdjustForName(strAssign, dxPrimary.MediaType);

            if (m_tmaxRegisterOptions.GetFolderAssignment(RegFolderAssignments.Description) == true)
                dxPrimary.Description = strAssign;

            //	Override the default values if this primary is being created from an XML node
            if (tmaxSource.XmlPrimary != null)
            {
                if (tmaxSource.XmlPrimary.Description.Length > 0)
                    dxPrimary.Description = tmaxSource.XmlPrimary.Description;
                if (tmaxSource.XmlPrimary.Name.Length > 0)
                    dxPrimary.Name = tmaxSource.XmlPrimary.Name;
            }

            //	By default assign the name stored in the XML if this is a deposition
            if ((dxPrimary.MediaType == TmaxMediaTypes.Deposition) &&
                (tmaxSource.UserData != null))
            {
                try
                {
                    CXmlDeposition xmld = (CXmlDeposition)tmaxSource.UserData;

                    if ((xmld.Name != null) && (xmld.Name.Length > 0))
                    {
                        dxPrimary.Name = xmld.Name;
                    }

                }
                catch
                {
                }

            }

            return true;

        }// private bool SetProperties(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)

        /// <summary>This method is called to set the path to the folder where the secondary files should be stored</summary>
        /// <param name="tmaxSource">The source folder used to register the object</param>
        /// <param name="dxPrimary">The primary media object being registered</param>
        /// <returns>true if successful</returns>
        private bool SetTargetPath(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)
        {
            if (m_tmaxRegisterOptions.FileTransfer == RegFileTransfers.Link)
            {
                //	Set the aliasing information for this record
                return SetUserPath(dxPrimary, tmaxSource.Path);
            }
            else
            {
                //	Use the source folder location to set the relative path
                //
                //	NOTE:	Depositions don't use relative path information at registration
                //			because we don't make any attempt to copy the videos. It's up to
                //			the user to make sure the videos are in the right location and the
                //			relative path is set to the right value
                if (dxPrimary.MediaType != TmaxMediaTypes.Deposition)
                {
                    if (SetRelativePath(tmaxSource, dxPrimary) == false)
                        return false;

                    //	Make sure this folder has not already been registered by the user
                    if (CheckTargetPath(tmaxSource, dxPrimary) == false)
                        return false;

                }// if(dxPrimary.MediaType != TmaxMediaTypes.Deposition)

                //	Everything must be OK
                return true;

            }// if(m_tmaxRegisterOptions.FileTransfer == RegFileTransfers.Link)

        }// private bool SetTargetPath(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)

        /// <summary>This method is called to check the target path to see if something is already registered there</summary>
        /// <param name="tmaxSource">The source folder used to register the object</param>
        /// <param name="dxPrimary">The primary media object being initialized</param>
        /// <returns>True if OK to continue</returns>
        private bool CheckTargetPath(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)
        {
            CTmaxSourceFolder tmaxTarget = new CTmaxSourceFolder();
            string strCasePath = "";
            string strTargetPath = "";
            string strRelativePath = "";
            bool bUpdate = false;
            bool bSuccessful = true;

            //	Clear this value in case we have to resolve a conflict
            m_strResolution = "";

            //	Check the source type to see if this test is necessary
            switch (tmaxSource.SourceType)
            {
                case RegSourceTypes.Document:
                case RegSourceTypes.Recording:
                    //	Get the default case path for this media type
                    strCasePath = GetCasePath(dxPrimary.MediaType);
                    if (strCasePath.EndsWith("\\") == true)
                        strCasePath = strCasePath.Substring(0, strCasePath.Length - 1);

                    //	Is the user doing an in-place registration?
                    if (tmaxSource.Path.ToLower().StartsWith(strCasePath) == true)
                        return true;

                    //	Check for the folder
                    break;
                case RegSourceTypes.Adobe:
                case RegSourceTypes.MultiPageTIFF:
                case RegSourceTypes.Powerpoint:	//	Only one folder for PowerPoints
                case RegSourceTypes.Deposition: //	Folder is quarenteed to be unique
                default:
                    return true;

            }// switch(tmaxSource.SourceType)

            //	Get the target folder
            strTargetPath = GetFolderSpec(dxPrimary, true);

            while (bSuccessful == true)
            {
                tmaxTarget.Initialize(strTargetPath);

                //	Does this folder exist?
                if (System.IO.Directory.Exists(tmaxTarget.Path) == false)
                    break;

                //	Do any files exist in the folder?
                if (tmaxTarget.GetFiles(true) == 0)
                    break;

                //	Attempt to resolve the conflict
                if (Resolve(tmaxSource, dxPrimary, ref strTargetPath, false) == false)
                {
                    bSuccessful = false; // Unable to register this record
                }
                else
                {
                    //	Path must be updated when we confirm this folder
                    bUpdate = true;

                }// if(Resolve(tmaxSource, ref strTargetPath, false) == false)

            }// while(bSuccessful == true)

            //	Do we need to update the relative path?
            if (bSuccessful == true)
            {
                //	Do we need to update the relative path?
                if (bUpdate == true)
                {
                    //	Get the new relative path
                    strCasePath = GetCasePath(dxPrimary.MediaType);
                    tmaxTarget.Initialize(strTargetPath);
                    tmaxTarget.GetRelativePath(strCasePath, ref strRelativePath);

                    //	Update the record's relative path
                    dxPrimary.RelativePath = strRelativePath;
                    if (dxPrimary.RelativePath.Length > 0)
                    {
                        if (dxPrimary.RelativePath.EndsWith("\\") == false)
                            dxPrimary.RelativePath += "\\";
                    }

                }// if(bUpdate == true)

                //	Set the target name so that it can be used to assign the MediaId for this
                //	record if the user has requested that option
                tmaxSource.TargetName = tmaxTarget.Name;

            }// if(bSuccessful == true)

            return bSuccessful;

        }// private bool CheckTargetPath(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)

        /// <summary>Called to resolve the specified registration conflict</summary>
        /// <param name="tmaxSource">The source folder being registered</param>
        /// <param name="dxPrimary">The primary record being registered</param>
        /// <param name="strConflict">The conflicting value</param>
        /// <param name="bMediaId">true if MediaId conflict, false if folder conflict</param>
        /// <returns>true if successful</returns>
        private bool Resolve(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary, ref string strConflict, bool bMediaId)
        {
            CFResolveConflict wndResolve = null;
            string strResolve = "";
            string strModified = "";
            string strParent = "";
            bool bSuccessful = true;
            int iIndex = 0;

            Debug.Assert(strConflict.Length > 0);
            if (strConflict.Length == 0) return false;

            //	Are we resolving a MediaId conflict?
            if (bMediaId == true)
            {
                strResolve = strConflict;
            }
            else
            {
                strParent = strConflict;

                //	Strip the trailing backslash
                while (strParent.EndsWith("\\") == true)
                    strParent = strParent.Substring(0, strParent.Length - 1);

                //	Separate the folder name from its parent folder
                if ((iIndex = strParent.LastIndexOf("\\")) >= 0)
                {
                    strResolve = strParent.Substring(iIndex + 1);
                    strParent = strParent.Substring(0, iIndex + 1);
                }

            }// if(bMediaId == true)

            //	Has automatic resolution be requested for this session?
            lock (lockForConflictForm)
                if (m_bAutoResolve == true)
                {
                    strModified = m_tmaxRegisterOptions.Resolve(dxPrimary, strResolve, RegConflictResolutions.Automatic);
                }
                else if (m_tmaxRegisterOptions.ConflictResolution == RegConflictResolutions.Prompt)
                {
                    //	Get the default automatic resolution
                    strModified = m_tmaxRegisterOptions.Resolve(dxPrimary, strResolve, RegConflictResolutions.Automatic);
                    if (m_strResolution.Length == 0)
                        m_strResolution = strModified;

                    //	Create the form to prompt the user for the resolution
                    wndResolve = new CFResolveConflict();
                    SetHandlers(wndResolve.EventSource);

                    //	Initialize the form
                    wndResolve.IsMediaId = bMediaId;
                    wndResolve.Conflict = strResolve;// the locally adjusted conflict 
                    wndResolve.Resolution = m_strResolution; // Initialize with last value

                    //	Is this actually a file-based registration?
                    switch (tmaxSource.SourceType)
                    {
                        case RegSourceTypes.Adobe:
                        case RegSourceTypes.MultiPageTIFF:
                        case RegSourceTypes.Powerpoint:
                        case RegSourceTypes.Deposition:

                            //	Use the filename as the source identifier
                            if ((tmaxSource.Files != null) && (tmaxSource.Files.Count == 1))
                                wndResolve.Source = tmaxSource.Files[0].Path;
                            break;

                    }// switch(tmaxSource.SourceType)

                    //	Use the folder path if not assigned a source value
                    if (wndResolve.Source.Length == 0)
                        wndResolve.Source = tmaxSource.Path;

                    //	Open the form
                    DisableTmaxKeyboard(true);
                    FTI.Shared.Win32.User.MessageBeep(0);
                    bSuccessful = (wndResolve.ShowDialog() == DialogResult.OK);
                    DisableTmaxKeyboard(false);

                    //	Did the user specify a value?
                    if (bSuccessful == true)
                    {
                        //	Has the user requested automatic resolution?
                        if ((wndResolve.AutoResolve == true) || (wndResolve.AutoResolveAll == true))
                        {
                            //	Nothing to do, strModified already set to auto value

                            //	Automatically resolve all subsequent conflicts?
                            if (wndResolve.AutoResolveAll == true)
                                m_bAutoResolve = true;

                        }
                        else
                        {
                            //	Use the value specified by the user
                            strModified = wndResolve.Resolution.Trim();
                        }

                        //	Save the last value
                        m_strResolution = strModified;

                    }// if(bSuccessful == true)


                }// else if(m_tmaxRegisterOptions.ConflictResolution == RegConflictResolutions.Prompt)
                else
                {
                    //	Apply the user defined options
                    strModified = m_tmaxRegisterOptions.Resolve(dxPrimary, strResolve);
                }

            if (bSuccessful == true)
            {
                if (strParent.Length > 0)
                {
                    strConflict = strParent;
                    if (strConflict.EndsWith("\\") == false)
                        strConflict += "\\";
                    strConflict += strModified;
                }
                else
                {
                    strConflict = strModified;
                }

            }// if(bSuccessful == true)

            return bSuccessful;

        }// private bool Resolve(CTmaxSourceFolder tmaxSource, ref string strConflict, bool bMediaId)

        /// <summary>This method is called to get the primary record that owns the files at the specified path</summary>
        /// <param name="strPath">The path to be checked</param>
        /// <param name="bIsFolder">True if the path specifies a folder, otherwise it's a filename</param>
        /// <returns>The exchange interface to the primary record that owns the files</returns>
        private CDxPrimary GetPrimaryOwner(string strPath, bool bIsFolder)
        {
            string strPrimary = "";

            //	Is the path a folder?
            if (bIsFolder == true)
            {
                //	First check to see if the folder exists
                if (System.IO.Directory.Exists(strPath) == false)
                    return null;

                //	Ignore the folder if it doesn't contain any files
                string[] aFiles = System.IO.Directory.GetFiles(strPath);
                if ((aFiles == null) || (aFiles.GetUpperBound(0) < 0))
                {
                    return null;
                }
                else
                {
                    aFiles = null;
                }

                //	Make sure the path is properly formatted to peform the comparison
                if (strPath.EndsWith("\\") == false)
                    strPath += "\\";

            }
            else
            {
                //	Does this file exist?
                if (System.IO.File.Exists(strPath) == false)
                    return null;

            }

            //	Search for the primary record that owns this file/folder
            foreach (CDxPrimary O in m_dxPrimaries)
            {
                //	Are we searching for primary media that map to individual folders?
                if (bIsFolder == true)
                {
                    if ((O.MediaType == TmaxMediaTypes.Document) ||
                       (O.MediaType == TmaxMediaTypes.Recording))
                    {
                        //	Get the path to this record's source folder
                        strPrimary = GetFolderSpec(O, true);

                        //	Do they match
                        if (String.Compare(strPrimary, strPath, true) == 0)
                            return O;
                    }

                }
                else
                {
                    //	The only target paths we check at registration that use files
                    //	instead of folders is PowerPoint presentations
                    if (O.MediaType == TmaxMediaTypes.Powerpoint)
                    {
                        //	Get the path to this record's filename
                        strPrimary = GetFileSpec(O);

                        //	Do they match
                        if (String.Compare(strPrimary, strPath, true) == 0)
                            return O;
                    }

                }// if(bIsFolder == true)

            }// foreach(CDxPrimary O in m_dxPrimaries)

            return null;

        }// private CDxPrimary GetPrimaryOwner(string strPath, bool bIsFolder)

        /// <summary>This method will move all tertiary grandchildren to the new location defined in ver 6.0.1</summary>
        /// <param name="dxPrimary">The parent primary record</param>
        private void MoveFiles600(CDxPrimary dxPrimary)
        {
            bool bClearSecondaries = false;
            bool bClearTertiaries = false;

            Debug.Assert(dxPrimary != null);
            if (dxPrimary == null) return;

            //	Do we need to fill the secondaries collection?
            if ((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
            {
                dxPrimary.Fill();
                if (dxPrimary.Secondaries.Count == 0) return;
                bClearSecondaries = true;
            }

            //	Locate all the designations and clips in the script
            foreach (CDxSecondary O in dxPrimary.Secondaries)
            {
                //	Is this a scene in a script?
                if (O.MediaType == TmaxMediaTypes.Scene)
                {
                    if (O.GetSource() != null)
                    {
                        switch (O.GetSource().MediaType)
                        {
                            case TmaxMediaTypes.Designation:
                            case TmaxMediaTypes.Clip:

                                //	Move the XML file associated with this record
                                if (MoveFile600((CDxTertiary)(O.GetSource())) == false)
                                    m_lMoveOnValidateErrors++;

                                break;

                        }

                    }// if(O.GetSource() != null)

                }
                else if (O.MediaType == TmaxMediaTypes.Page)
                {
                    //	Do we need to fill the tertiaries collection?
                    if ((O.Tertiaries == null) || (O.Tertiaries.Count == 0))
                    {
                        O.Fill();
                        if (O.Tertiaries.Count > 0)
                            bClearTertiaries = true;
                    }

                    //	Move all of the treatments
                    foreach (CDxTertiary T in O.Tertiaries)
                    {
                        if (MoveFile600(T) == false)
                            m_lMoveOnValidateErrors++;
                    }

                    if (bClearTertiaries == true) O.Tertiaries.Clear();

                }// if(O.MediaType == TmaxMediaTypes.Scene)

            }// foreach(CDxSecondary O in dxScript.Secondaries)

            //	Should we clear the collection?
            if (bClearSecondaries == true)
                dxPrimary.Secondaries.Clear();

        }// private void MoveFiles600(CDxPrimary dxPrimary)

        /// <summary>This method will move the specified designation to the position used starting with version 6.0.1</summary>
        /// <param name="dxTertiary">The designation being moved</param>
        private bool MoveFile600(CDxTertiary dxTertiary)
        {
            string strNewFolder = "";
            string strSource = "";
            string strTarget = "";

            Debug.Assert(dxTertiary != null);
            if (dxTertiary == null) return false;

            //	First check to see if the file already exists using the new path
            //
            //	NOTE:	Assuming it hasn't been overridden, the system is currently
            //			using the default 600 folder (_tmax_designations) for clips
            if (dxTertiary.MediaType == TmaxMediaTypes.Clip)
                SetCasePath(TmaxCaseFolders.Clips, CASE_DATABASE_CLIPS_FOLDER_601, false);

            strTarget = GetFileSpec601(dxTertiary);

            if (dxTertiary.MediaType == TmaxMediaTypes.Clip)
                SetCasePath(TmaxCaseFolders.Clips, m_strDefaultClipsFolder, false);

            if (strTarget.Length == 0) return false;
            if (System.IO.File.Exists(strTarget) == true) return true;

            //	Get the path to the file being moved and make sure it exists
            strSource = GetFileSpec600(dxTertiary);
            if (strSource.Length == 0) return false;
            if (System.IO.File.Exists(strSource) == false) return false;

            //	Set the status message in the progress form
            SetValidateStatus("Moving " + strSource);

            //	Make sure the target folder exists
            strNewFolder = System.IO.Path.GetDirectoryName(strTarget);
            try
            {
                if (System.IO.Directory.Exists(strNewFolder) == false)
                    System.IO.Directory.CreateDirectory(strNewFolder);
            }
            catch
            {
                AddValidationError(dxTertiary.GetBarcode(true), "Unable to create target folder to move the designation: " + strNewFolder, true);
                return false;
            }

            //	Move the file
            try
            {
                System.IO.File.Move(strSource, strTarget);
                return true;
            }
            catch
            {
                AddValidationError(dxTertiary.GetBarcode(true), "Unable to move designation from " + strSource + " to " + strTarget, true);
                return false;
            }

        }// private bool MoveFile600(CDxTertiary dxTertiary)

        /// <summary>This method is called to synchronize the version information in the database with the assembly version information</summary>
        private bool SyncCaseVersion()
        {
            CTmdataVersion dbVer = new CTmdataVersion();

            return SetCaseVersion(dbVer.Major, dbVer.Minor, dbVer.QEF, dbVer.Build);

        }// private bool SyncCaseVersion()

        /// <summary>This method is called to synchronize the version information in the database with the assembly version information</summary>
        /// <param name="iMajor">The major version identifier</param>
        /// <param name="iMinor">The minor version identifier</param>
        /// <param name="iQEF">The QEF version identifier</param>
        /// <param name="iBuild">The build version identifier</param>
        /// <returns>True if successful</returns>
        private bool SetCaseVersion(int iMajor, int iMinor, int iQEF, int iBuild)
        {
            Debug.Assert(m_dxDetail != null);
            Debug.Assert(m_dxDetails != null);

            if ((m_dxDetails != null) && (m_dxDetail != null))
            {
                m_dxDetail.TmaxCase.VerMajor = iMajor;
                m_dxDetail.TmaxCase.VerMinor = iMinor;
                m_dxDetail.TmaxCase.VerQEF = iQEF;
                m_dxDetail.TmaxCase.VerBuild = iBuild;
                m_dxDetail.Version = ""; // Force recreation of the text descriptor

                return m_dxDetails.Update(m_dxDetail);
            }

            return false;

        }// private bool SetCaseVersion(int iMajor, int iMinor, int iQEF, int iBuild)

        /// <summary>Called to set the name assigned to the active case</summary>
        /// <param name="strName">The name to be assigned</param>
        /// <returns>true if successful</returns>
        public bool SetCaseName(string strName)
        {
            bool bSuccessful = true;

            //	Make sure we have an active database
            if (m_dxDetails == null) return false;
            if (m_dxDetail == null) return false;

            try
            {
                //	Should we assign the default name?
                if ((strName.Length == 0) && (this.Folder.Length > 0))
                    strName = System.IO.Path.GetFileName(this.Folder);

                //	Has the value changed?
                if (m_dxDetail.Name != strName)
                {
                    m_dxDetail.Name = strName;
                    bSuccessful = m_dxDetails.Update(m_dxDetail);
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetCaseName", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_CASE_NAME_EX, strName), Ex);
                bSuccessful = false;
            }

            return bSuccessful;

        }// public bool SetCaseName(string strName)

        /// <summary>Called to set the short case name for the active database</summary>
        /// <param name="strName">The short name to be assigned</param>
        /// <returns>true if successful</returns>
        public bool SetShortCaseName(string strName)
        {
            bool bSuccessful = true;

            //	Make sure we have an active database
            if (m_dxDetails == null) return false;
            if (m_dxDetail == null) return false;

            try
            {
                //	Has the value changed?
                if (m_dxDetail.ShortName != strName)
                {
                    m_dxDetail.ShortName = strName;
                    bSuccessful = m_dxDetails.Update(m_dxDetail);
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetShortCaseName", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_SHORT_CASE_NAME_EX, strName), Ex);
                bSuccessful = false;
            }

            return bSuccessful;

        }// public bool SetShortCaseName(string strName)

        /// <summary>This method is called to populate the highlighters collection</summary>
        private bool SetHighlighters()
        {
            //	Add the default highlighters
            if (m_dxHighlighters != null)
            {
                m_dxHighlighters.Add(System.Drawing.Color.Blue, TmaxHighlighterGroups.Defendant, "Our Designations");
                m_dxHighlighters.Add(System.Drawing.Color.Red, TmaxHighlighterGroups.Plaintiff, "Their Designations");
                m_dxHighlighters.Add(System.Drawing.Color.DarkGreen, TmaxHighlighterGroups.Defendant, "Our Counters");
                m_dxHighlighters.Add(System.Drawing.Color.DarkMagenta, TmaxHighlighterGroups.Plaintiff, "Their Counters");
                m_dxHighlighters.Add(System.Drawing.Color.LightGreen, TmaxHighlighterGroups.Defendant, "Our Counter Counters");
                m_dxHighlighters.Add(System.Drawing.Color.Violet, TmaxHighlighterGroups.Plaintiff, "Their Counter Counters");
                m_dxHighlighters.Add(System.Drawing.Color.Orange, TmaxHighlighterGroups.Defendant, "");

                return true;
            }
            else
            {
                return false;
            }

        }// private bool SetHighlighters()

        /// <summary>This method is called to set and store the current user information</summary>
        /// <param name="strUser">Name of the current user</param>
        /// <returns>true if successful</returns>
        private bool SetUser(string strUser)
        {
            Debug.Assert(m_dxUser == null);

            //	Allocate a new transfer object
            if ((m_dxUser = new CDxUser()) != null)
            {
                m_dxUser.Name = strUser;
                m_dxUser.LastTime = System.DateTime.Now;

                //	Set the user name in the case options
                if ((m_tmaxCaseOptions != null) && (m_tmaxCaseOptions.Machine != null))
                {
                    m_tmaxCaseOptions.Machine.User = m_dxUser.Name;
                }

                return (m_dxUsers.Add(m_dxUser) != null);
            }
            else
            {
                return false;
            }

        }// private bool SetUser(string strUser)

        /// <summary>This method sets the station specific options for this case</summary>
        /// <returns>True if successful</returns>
        private bool SetStationOptions()
        {
            string strFileSpec = "";
            string strCaseId = "";
            string strUser = "";
            bool bSuccessful = false;

            //	Allocate the object if necessary
            if (m_tmaxStationOptions == null)
                m_tmaxStationOptions = new CTmaxStationOptions();

            //	Open the station options configuration file
            //
            //	NOTE:	The station options are stored in the application folder
            //			because they are station specific
            strFileSpec = AppFolder;
            if (strFileSpec.EndsWith("\\") == false)
                strFileSpec += "\\";
            strFileSpec += CASE_DATABASE_STATION_OPTIONS_FILENAME;

            if (this.Detail != null)
                strCaseId = this.Detail.UniqueId;
            if (this.User != null)
                strUser = this.User.Name;

            if ((bSuccessful = m_tmaxStationOptions.Initialize(strFileSpec, strCaseId, strUser)) == true)
            {
                if (this.CaseCodes != null)
                    m_tmaxStationOptions.LoadCaseCodes(this.CaseCodes);
            }

            return bSuccessful;

        }// private bool SetStationOptions()

        /// <summary>This method will drill down the specified source folder and move all child files into the parent folder</summary>
        /// <param name="tmaxSource">The folder containing all source files to be merged</param>
        /// <returns>true if successful</returns>
        private bool MergeSource(CTmaxSourceFolder tmaxSource)
        {
            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.SubFolders != null);

            SetRegisterProgress("Merging source files ...");

            //	Drill down into the subfolders to pull up all their files
            foreach (CTmaxSourceFolder tmaxFolder in tmaxSource.SubFolders)
            {
                try
                {
                    //	Did the user cancel?
                    if (m_bRegisterCancelled == true) break;

                    MergeSource(tmaxFolder, tmaxSource);
                }
                catch (System.Exception Ex)
                {
                    FireError(this, "MergeSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_MERGE_SOURCE_EX), Ex);
                }

            }

            //	Mark the primary folder as being merged
            tmaxSource.SetPrimaryAttribute(TmaxPrimaryAttributes.Merged, true);

            return true;

        }// private bool MergeSource(CTmaxSourceFolder tmaxSource)

        /// <summary>This method will reorder the child records of the specified parent</summary>
        /// <param name="tmaxItem">The TrialMax event item whose children are to be synchronized</param>
        /// <param name="tmaxModified">Collection in which to place items that were modified</param>
        /// <returns>true if one or more records were modified</returns>
        private bool Synchronize(CTmaxItem tmaxItem, CTmaxItems tmaxModified)
        {
            CDxMediaRecord dxParent = null;
            CDxMediaRecords dxChildren = null;
            bool bModified = false;
            long lDisplayOrder = 1;

            Debug.Assert(tmaxItem != null);
            if (tmaxItem == null) return false;
            if (tmaxItem.IPrimary == null) return false;

            //	Get the parent's exchange object
            if ((dxParent = GetRecordFromItem(tmaxItem)) != null)
            {
                //	Get the child collection
                if ((dxChildren = dxParent.GetChildCollection()) != null)
                {
                    //	Do we need to populate the child collection?
                    if (dxChildren.Count == 0)
                    {
                        dxParent.Fill();
                    }

                    //	Now synchronize the records
                    foreach (CDxMediaRecord O in dxChildren)
                    {
                        //	Is this barcode or display order wrong?
                        if ((O.DisplayOrder != lDisplayOrder) || (O.BarcodeId != lDisplayOrder))
                        {
                            //	Update the identifiers
                            O.DisplayOrder = lDisplayOrder;
                            O.BarcodeId = lDisplayOrder;
                            O.Collection.Update(O);

                            //	Add an item to the caller's collection
                            tmaxModified.Add(new CTmaxItem(O));

                            bModified = true;
                        }

                        lDisplayOrder++;

                    }// foreach(CDxMediaRecord O in dxChildren)

                    //	Was one of the children modified?
                    if (bModified)
                    {
                        //	Update the parent record
                        dxParent.Collection.Update(dxParent);
                        tmaxModified.Add(new CTmaxItem(dxParent));
                    }

                }// if((dxChildren = dxParent.GetChildCollection()) != null)

            }// if((dxParent = GetRecordFromItem(tmaxItem)) != null) 

            return bModified;

        }//	private bool Synchronize(CTmaxItem tmaxItem, CTmaxItems tmaxModified)

        /// <summary>This method will modify the specified folder name by applying the morphing methods set in the registration options</summary>
        ///	<param name="strFolder">The source folder</param>
        ///	<returns>the morphed values</returns>
        private string Morph(string strFolder)
        {
            string strMorphed = strFolder;
            long lOriginal;
            long lOffset;

            if (m_tmaxRegisterOptions != null)
            {
                //	What morphing method is being used?
                switch (m_tmaxRegisterOptions.MorphMethod)
                {
                    case RegMorphMethods.Offset:

                        try
                        {
                            lOriginal = System.Convert.ToInt64(strFolder);
                        }
                        catch (System.Exception Ex)
                        {
                            FireError(this, "Morph", this.ExBuilder.Message(ERROR_CASE_DATABASE_MORPH_FOLDER_EX, strFolder), Ex);
                            break;
                        }


                        try
                        {
                            lOffset = System.Convert.ToInt64(m_tmaxRegisterOptions.MorphMethodText);
                        }
                        catch (System.Exception Ex)
                        {
                            FireError(this, "Morph", this.ExBuilder.Message(ERROR_CASE_DATABASE_MORPH_OFFSET_EX, m_tmaxRegisterOptions.MorphMethodText), Ex);
                            break;
                        }

                        lOriginal += lOffset;
                        strMorphed = lOriginal.ToString();
                        break;

                    case RegMorphMethods.Prefix:

                        strMorphed = m_tmaxRegisterOptions.MorphMethodText + strFolder;
                        break;

                    case RegMorphMethods.Suffix:

                        strMorphed = strFolder + m_tmaxRegisterOptions.MorphMethodText;
                        break;

                    case RegMorphMethods.None:
                    default:

                        break;
                }

            }// private string Morph(string strFolder)

            return strMorphed;

        }// private string Morph(string strFolder)

        /// <summary>This method will take transfer all files from the source folder to the target folder</summary>
        /// <param name="tmaxSource">The folder containing the files to be merged</param>
        /// <param name="tmaxTarget">The folder in which to store the merged files</param>
        /// <returns>true if successful</returns>
        private bool MergeSource(CTmaxSourceFolder tmaxSource, CTmaxSourceFolder tmaxTarget)
        {
            string strTarget = "";

            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.SubFolders != null);
            Debug.Assert(tmaxTarget != null);
            Debug.Assert(tmaxTarget.Files != null);
            Debug.Assert(tmaxTarget.SubFolders != null);

            //	We are going to strip off the target folder path
            strTarget = tmaxTarget.Path.ToLower();
            if (strTarget.EndsWith("\\") == false)
                strTarget += "\\";

            //	Transfer the files to the target folder
            foreach (CTmaxSourceFile O in tmaxSource.Files)
            {
                //	Modify the name of the file to include that portion of
                //	the path that is being lost due to the transfer
                //
                //	NOTE:	We always want to be able to get the fully qualified
                //			path by adding the folder path and the file name
                if (O.Path.ToLower().StartsWith(strTarget) == true)
                    O.Name = O.Path.Substring(strTarget.Length);

                //	Add to the target's file collection
                tmaxTarget.Files.Add(O);
            }

            //	Clear the source collection
            tmaxSource.Files.Clear();

            //	Now drill down the subfolders
            foreach (CTmaxSourceFolder tmaxFolder in tmaxSource.SubFolders)
            {
                //	Did the user cancel?
                if (m_bRegisterCancelled == true) break;

                MergeSource(tmaxFolder, tmaxTarget);
            }

            return true;

        }// private bool MergeSource(CTmaxSourceFolder tmaxSource, CTmaxSourceFolder tmaxTarget)

        /// <summary>This method will transfer each file in the source folder into it's own subfolder</summary>
        /// <param name="tmaxSource">The folder containing all source files to be separated</param>
        /// <param name="eSourceType">The registration type to be assigned to the new subfolders</param>
        /// <returns>true if successful</returns>
        private bool SetOnePerFile(CTmaxSourceFolder tmaxSource, RegSourceTypes eSourceType)
        {
            CTmaxSourceType tmaxSourceType = null;
            CTmaxSourceType tmaxRequested = null;

            Debug.Assert(m_tmaxSourceTypes != null);
            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.SubFolders != null);

            SetRegisterProgress("Reorganizing source files ...");

            //	Split all the subfolders first
            //
            //	NOTE:	We have to do this first because each file in the folder's
            //			collection is going to be converted to a subfolder
            foreach (CTmaxSourceFolder O in tmaxSource.SubFolders)
            {
                //	Did the user cancel
                if (m_bRegisterCancelled == true) break;

                SetOnePerFile(O, eSourceType);

            }// foreach(CTmaxSourceFolder O in tmaxSource.SubFolders)

            try
            {
                //	Stop here if the user cancelled or if there are no files
                if ((tmaxSource.Files.Count == 0) || (m_bRegisterCancelled == true))
                    return true;

                //	Get the descriptor for the requested source type
                if ((eSourceType != RegSourceTypes.AllFiles) && (eSourceType != RegSourceTypes.NoSource))
                    tmaxRequested = m_tmaxSourceTypes.Find(eSourceType);

                //	Check for the simple case where there is only one file in the folder
                if (tmaxSource.Files.Count == 1)
                {
                    //	Initialize the folder type information
                    tmaxSource.SourceType = RegSourceTypes.NoSource;
                    tmaxSource.MediaType = TmaxMediaTypes.Unknown;

                    //	Get the source information for this file
                    if ((tmaxSourceType = m_tmaxSourceTypes.FindFromFilename(tmaxSource.Files[0].Name)) != null)
                    {
                        //	Does this match the requested type?
                        if ((tmaxRequested == null) || (tmaxSourceType.RegSourceType == tmaxRequested.RegSourceType))
                        {
                            tmaxSource.MediaType = tmaxSourceType.MediaType;
                            tmaxSource.SourceType = tmaxSourceType.RegSourceType;
                            tmaxSource.SplitOnRegistration = true;
                        }

                    }// if((tmaxSourceType = m_tmaxSourceTypes.FindFromFilename(tmaxSource.Files[0].Name)) != null)

                    //	Do we have a valid source type?
                    if (tmaxSource.SourceType == RegSourceTypes.NoSource)
                    {
                        tmaxSource.Files.Clear();
                    }

                }
                else
                {
                    //	Did the caller specify a source type?
                    if (tmaxRequested != null)
                    {
                        //	Extract all source of the specified type
                        SeparateSource(tmaxSource, null, tmaxRequested.RegSourceType, true);
                    }
                    else
                    {
                        //	Separate out the depositions first
                        //
                        //	NOTE:	We have to do this first because we don't want to split
                        //			any videos referenced by a deposition into their own folder
                        SeparateSource(tmaxSource, null, RegSourceTypes.Deposition, false);

                        //	Separate all the remaining types
                        Array aTypes = Enum.GetValues(typeof(RegSourceTypes));
                        foreach (RegSourceTypes eType in aTypes)
                        {
                            switch (eType)
                            {
                                case RegSourceTypes.Deposition:

                                    //	Already processed
                                    break;

                                case RegSourceTypes.AllFiles:
                                case RegSourceTypes.NoSource:

                                    //	Not valid file types
                                    break;

                                default:

                                    //	Separate files of this type
                                    SeparateSource(tmaxSource, null, eType, false);
                                    break;

                            }

                            //	Have we processed all the files?
                            if (tmaxSource.Files.Count == 0)
                                break;

                        }// foreach(RegSourceTypes eType in aTypes)

                    }// if(tmaxSourceType != null)		

                }// if(tmaxSource.Files.Count == 1)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetOnePerFile", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_ONE_PER_FILE_EX), Ex);
                return false;
            }

            return true;

        }// private bool SetOnePerFile(CTmaxSourceFolder tmaxSource, RegSourceTypes eSourceType)

        /// <summary>This method will separate files of the specified type in the source collection into individual folders</summary>
        /// <param name="tmaxSource">The folder containing the files to be split</param>
        /// <param name="tmaxFolders">The collection in which new folders are to be placed</param>
        /// <param name="eSourceType">The registration source type of files to be split</param>
        /// <param name="bClear">true to clear the source collection after splitting the requested files</param>
        /// <returns>true if successful</returns>
        /// <remarks>This method does not drill into the subfolders</remarks>
        private bool SeparateSource(CTmaxSourceFolder tmaxSource, CTmaxSourceFolders tmaxFolders, RegSourceTypes eSourceType, bool bClear)
        {
            CTmaxSourceFolder tmaxFolder = null;
            CTmaxSourceFolders tmaxDepositions = null;
            CTmaxSourceType tmaxSourceType = null;
            CTmaxSourceType tmaxRequested = null;
            CTmaxSourceFiles tmaxInvalid = null;
            CTmaxSourceFiles tmaxSeparated = null;
            int iSeparated = 0;
            bool bInsert = false;

            //	Sanity checks ...
            Debug.Assert(m_tmaxSourceTypes != null);
            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.SubFolders != null);
            if (m_tmaxSourceTypes == null) return false;
            if (tmaxSource == null) return false;
            if (tmaxSource.Files == null) return false;
            if (tmaxSource.SubFolders == null) return false;

            //	Get the descriptor for the source type requested by the caller
            if ((tmaxRequested = m_tmaxSourceTypes.Find(eSourceType)) == null)
            {
                Debug.Assert(false, "invalid source type");
                return false;
            }

            //	Don't bother if there are no files to be separated
            if (tmaxSource.Files.Count == 0) return true;

            try
            {
                //	Is there only one file in this folder?
                if (tmaxSource.Files.Count == 1)
                {
                    //	Does this file match the requested type?
                    if (m_tmaxSourceTypes.CheckFile(tmaxSource.Files[0].Name, tmaxRequested.RegSourceType) != null)
                    {
                        //	Mark the folder
                        tmaxSource.SourceType = tmaxRequested.RegSourceType;
                        tmaxSource.MediaType = tmaxRequested.MediaType;
                    }
                    else
                    {
                        //	No valid source files in this folder
                        tmaxSource.Files.Clear();
                        tmaxSource.SourceType = RegSourceTypes.NoSource;
                        tmaxSource.MediaType = TmaxMediaTypes.Unknown;
                    }

                    //	Nothing more to do
                    return true;

                }// if(tmaxSource.Files.Count == 1)

                //	Use the source's subfolder collection if no target specified
                if (tmaxFolders == null)
                    tmaxFolders = tmaxSource.SubFolders;

                //	Insert new folders at top if target is already populated
                bInsert = (tmaxFolders.Count > 0);

                //	Allocate temporary collections used to perform separation
                tmaxDepositions = new CTmaxSourceFolders();
                tmaxInvalid = new CTmaxSourceFiles();
                tmaxSeparated = new CTmaxSourceFiles();

                //	Locate all files of the requested type
                foreach (CTmaxSourceFile O in tmaxSource.Files)
                {
                    //	Get the source type descriptor for this file
                    if ((tmaxSourceType = m_tmaxSourceTypes.FindFromFilename(O.Name)) == null)
                    {
                        //	This file is not valid source media
                        tmaxInvalid.Add(O);
                        continue;
                    }

                    //	Ignore this file if it is not of the requested type
                    if (tmaxSourceType.RegSourceType != tmaxRequested.RegSourceType)
                        continue;

                    //	Create a folder that uses the same path as the source
                    tmaxFolder = new CTmaxSourceFolder(tmaxSource.Path);

                    //	Put this file in the new folder
                    tmaxFolder.Files.Add(O);
                    tmaxSeparated.Add(O);

                    //	Assign the type information to the new folder
                    tmaxFolder.SourceType = tmaxRequested.RegSourceType;
                    tmaxFolder.MediaType = tmaxRequested.MediaType;

                    //	What type of source is this?
                    switch (tmaxFolder.SourceType)
                    {
                        case RegSourceTypes.Deposition:

                            //	Get the segment information from the transcript file
                            if (GetDepoSegments(tmaxFolder) == false)
                            {
                                //	Don't add this new folder to the target collection
                                continue;
                            }
                            else
                            {
                                //	Store in the temporary collection
                                tmaxDepositions.Add(tmaxFolder);
                            }
                            break;

                        case RegSourceTypes.Document:
                        case RegSourceTypes.Recording:

                            //	Set the flag to indicate that this is a split folder
                            tmaxFolder.SplitOnRegistration = true;
                            break;

                    }

                    //	Put the new folder in the target collection
                    if (bInsert == true)
                        tmaxFolders.Insert(tmaxFolder, iSeparated);
                    else
                        tmaxFolders.Add(tmaxFolder);

                    iSeparated++;

                    //	Add a suffix just in case the folder name is being
                    //	assigned to other primary media fields like MediaId
                    tmaxFolder.Suffix = ("_" + tmaxSourceType.RegSourceType.ToString() + "_" + iSeparated.ToString());

                }// foreach(CTmaxSourceFile O in tmaxSource.Files)

                //	Are we supposed to clear the unused files from the collection?
                if (bClear == true)
                {
                    tmaxSource.Files.Clear();
                }
                else
                {
                    //	Remove the invalid files
                    if (tmaxInvalid.Count > 0)
                    {
                        foreach (CTmaxSourceFile O in tmaxInvalid)
                            tmaxSource.Files.Remove(O);
                        tmaxInvalid.Clear();
                    }

                    //	Remove the files that were put in their own folders
                    if (tmaxSeparated.Count > 0)
                    {
                        foreach (CTmaxSourceFile O in tmaxSeparated)
                            tmaxSource.Files.Remove(O);
                        tmaxSeparated.Clear();
                    }

                    //	Remove all video segments associated with depositions
                    if (tmaxDepositions.Count > 0)
                    {
                        foreach (CTmaxSourceFolder O in tmaxDepositions)
                            RemoveSegments(O, tmaxSource);
                        tmaxDepositions.Clear();
                    }

                }// if(bClear == true)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "SeparateSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_SEPARATE_SOURCE_EX, eSourceType, tmaxSource.Path), Ex);
            }

            //	Clean up
            if (tmaxInvalid != null)
            {
                tmaxInvalid.Clear();
                tmaxInvalid = null;
            }
            if (tmaxSeparated != null)
            {
                tmaxSeparated.Clear();
                tmaxSeparated = null;
            }
            if (tmaxDepositions != null)
            {
                tmaxDepositions.Clear();
                tmaxDepositions = null;
            }

            return true;

        }// private bool SeparateSource(CTmaxSourceFolder tmaxSource, CTmaxSourceFolders tmaxSeparated, RegSourceTypes eSourceType, bool bClear)

        /// <summary>This method runs in its own thread to register source files</summary>
        /// <remarks>This method runs in a local worker thread executed by the Register() method</remarks>
        private void RegisterThreadProc()
        {
            try
            {
                Debug.Assert(m_RegSourceFolder != null);
                if (ConversionTasksArray == null)
                {
                    //lock (lockConversionTasksArray)
                    //{
                    if (ConversionTasksArray == null)
                    {
                        ConversionTasksArray = new List<CTmaxPDFManager>();
                    }
                    //}
                }
                //	Are we merging all source files into a single primary media object?
                if ((m_tmaxRegisterOptions != null) && (m_tmaxRegisterOptions.MediaCreation == RegMediaCreations.Merge))
                {
                    //	Search for the first folder that contains at least 1 file
                    //	or more than one subfolder
                    while ((m_RegSourceFolder.Files.Count == 0) && (m_RegSourceFolder.SubFolders.Count == 1))
                        m_RegSourceFolder = m_RegSourceFolder.SubFolders[0];

                    //	Merge the files
                    MergeSource(m_RegSourceFolder);

                    //	Set the folder type information
                    SetSourceTypes(m_RegSourceFolder, m_eRegSourceType);
                }
                //	Are we splitting all the files into individual media objects?
                else if ((m_tmaxRegisterOptions != null) && (m_tmaxRegisterOptions.MediaCreation == RegMediaCreations.Split))
                {
                    //	Create one folder for each file
                    SetOnePerFile(m_RegSourceFolder, m_eRegSourceType);
                }
                else
                {
                    //	Set the media types
                    SetSourceTypes(m_RegSourceFolder, m_eRegSourceType);
                }

                // Count total number of pages in all the files that needs to be converted
                m_totalPages = 0;
                m_cfRegisterProgress.CompletedPages = 0;
                logDetailed.Info("********************************* STARTING REGISTRATION PROCESS *********************************");
                logUser.Info("********************************* STARTING REGISTRATION PROCESS *********************************");
                CalculateTotalPages(m_RegSourceFolder);
                m_cfRegisterProgress.TotalPages = m_totalPages * 2;//33% Detection 33% extraction and 33% conversion;
                SetRegisterProgress("Registration in progress ...");
                //	Add the new records to the database
                if ((m_RegSourceFolder != null) && (m_bRegisterCancelled == false))
                    AddSource(m_RegSourceFolder);

                //	Notify the progress form
                if ((m_cfRegisterProgress != null) && (m_bRegisterCancelled == false))
                {
                    m_cfRegisterProgress.CompletedPages = m_totalPages * 2; //Same as above;
                    m_cfRegisterProgress.Finished = true;
                    SetRegisterProgress("Registration complete");
                }
            }
            catch (Exception Ex)
            {
                logDetailed.Error(Ex.ToString());
            }

        }// private void RegisterThreadProc()

        private void CalculateTotalPages(CTmaxSourceFolder tmaxSource)
        {

            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.SubFolders != null);

            //	Are there any files in the source folder?
            if (tmaxSource.Files.Count > 0)
            {
                for (int i = 0; i < tmaxSource.Files.Count; i++)
                {
                    CTmaxSourceFile tmaxImportFile = (CTmaxSourceFile)tmaxSource.Files.GetAt(0);
                    switch (tmaxSource.SourceType)
                    {
                        case RegSourceTypes.Adobe:
                            {
                                try
                                {
                                    PdfReader pdfFile = new PdfReader(tmaxImportFile.Path);
                                    m_totalPages += pdfFile.NumberOfPages;
                                    pdfFile.Close();
                                }
                                catch (Exception Ex)
                                {
                                    m_totalPages += 1;
                                    logDetailed.Error(Ex.ToString()); // Exception thrown when counting number of pages. May be because of corrupt PDF
                                }
                            }
                            break;
                        default:
                            m_totalPages += 1;
                            break;
                    }
                }

            }// if(tmaxSource.Files.Count > 0)

            //	Add each subfolder
            foreach (CTmaxSourceFolder tmaxSubFolder in tmaxSource.SubFolders)
            {
                if (m_bRegisterCancelled == false)
                {
                    CalculateTotalPages(tmaxSubFolder);

                    //	Mark this parent as being registered if its subfolder got registered
                    //
                    //	NOTE:	This allows us to maintain the registration chain when a folder
                    //			has nothing but subfolders
                    if (tmaxSubFolder.Registered == true)
                        tmaxSource.Registered = true;
                }

            }//foreach(CTmaxSourceFolder tmaxSubFolder in tmaxSource.SubFolders)
        }

        /// <summary>This method runs in its own thread to validate the database contents</summary>
        /// <remarks>This method runs in a local worker thread executed by the Validate() method</remarks>
        private void ValidateThreadProc()
        {
            Debug.Assert(m_dxPrimaries != null);
            Debug.Assert(m_tmaxValidateOptions != null);

            //	Reset the error counter in case we have to do a conversion
            m_lMoveOnValidateErrors = 0;

            //	Add the Codes table if we are performing a codes conversion
            if ((m_bConvertToCodes == true) || (m_bCreatePickLists == true))
            {
                if (EnableCaseCodes() == false)
                {
                    m_bConvertToCodes = false;
                    m_bCreatePickLists = false;
                }

            }// if((m_bConvertToCodes == true) || (m_bCreatePickLists == true))

            //	Iterate all the primary records
            foreach (CDxPrimary O in m_dxPrimaries)
            {
                //	Has the user cancelled the operation
                if (GetValidateCancelled() == true)
                    break;

                //	Turn the properties into codes
                if ((m_bConvertToCodes == true) || (m_tmaxValidateOptions.TransferCodes == true))
                {
                    SetValidateStatus("Converting Fielded Data " + O.GetBarcode(false) + " ......");
                    O.ExchangeCodedProperties(true, false, true, false);
                }

                //	What type of primary record are we dealing with?
                switch (O.MediaType)
                {
                    case TmaxMediaTypes.Document:

                        if (m_tmaxValidateOptions.Documents == true)
                            Validate(O);

                        //	Should we move all treatments to their new location?
                        if ((m_bRequires601Update == true) && (m_bValidateMoveConfirmed == true))
                            MoveFiles600(O);

                        break;

                    case TmaxMediaTypes.Powerpoint:

                        if (m_tmaxValidateOptions.PowerPoints == true)
                            Validate(O);
                        break;

                    case TmaxMediaTypes.Recording:

                        if (m_tmaxValidateOptions.Recordings == true)
                            Validate(O);
                        break;

                    case TmaxMediaTypes.Deposition:

                        if ((m_tmaxValidateOptions.Transcripts == true) ||
                            (m_tmaxValidateOptions.VideoFiles == true))
                            Validate(O);
                        break;

                    case TmaxMediaTypes.Script:

                        if (m_tmaxValidateOptions.Scripts == true)
                            Validate(O);

                        //	Should we move all designations to their new location?
                        if ((m_bRequires601Update == true) && (m_bValidateMoveConfirmed == true))
                            MoveFiles600(O);

                        break;

                }// switch(O.MediaType)

            }// foreach(CDxPrimary O in m_dxPrimaries)

            //	Did we move the files during this operation?
            if ((m_bRequires601Update == true) && (m_bValidateMoveConfirmed == true))
            {
                //	Did all files get moved OK
                if ((m_lMoveOnValidateErrors == 0) && (GetValidateCancelled() == false))
                {
                    //	Update the version information in the database
                    if (SyncCaseVersion() == true)
                    {
                        //	Clear the flag
                        m_bRequires601Update = false;

                        //	Switch the default designations folder
                        m_strDefaultClipsFolder = CASE_DATABASE_CLIPS_FOLDER_601;
                        SetCasePath(TmaxCaseFolders.Clips, null, false);

                    }// if(SyncDatabaseVersion() == true)

                }// ((m_lMoveOnValidateErrors == 0) && (GetValidateCancelled() == false))

            }// if((m_bRequires601Update == true) && (m_bValidateMoveConfirmed == true))

            //	Did we convert to codes during this operation?
            if ((m_bConvertToCodes == true) || (m_bCreatePickLists == true))
            {
                if (GetValidateCancelled() == false)
                {
                    //	Update the version information in the database
                    if (SyncCaseVersion() == true)
                    {
                        //	Initialize the new case codes
                        InitializeCaseCodes(true);

                    }// if(SyncDatabaseVersion() == true)

                }// if(GetValidateCancelled() == false)

            }// if((m_bConvertToCodes == true) && (GetValidateCancelled() == false))

            //	Are we supposed to validate the binders?
            if ((m_dxRootBinder != null) && (m_tmaxValidateOptions.Binders == true))
            {
                Validate(m_dxRootBinder);
            }

            //	Are we supposed to validate the barcode map?
            if ((m_dxBarcodeMap != null) && (m_tmaxValidateOptions.BarcodeMap == true))
            {
                //	Check the contents to make sure all records are valid
                foreach (CDxBarcode O in m_dxBarcodeMap)
                {
                    //	Has the user cancelled the operation
                    if (GetValidateCancelled() == true)
                        break;

                    //	Update the status text
                    SetValidateStatus("Validating foreign barcode: " + O.ForeignBarcode + " ......");

                    //	Is the source record valid
                    if ((O.GetSource() == null) || (IsValidRecord(O.GetSource()) == false))
                    {
                        AddValidationError(O.ForeignBarcode, "Invalid mapped barcode: PSTQ = " + O.PSTQ, true);
                    }

                }

            }// if((m_dxBarcodeMap != null) && (m_tmaxValidateOptions.BarcodeMap == true))

            //	Notify the progress form
            if ((m_cfValidateProgress != null) && (GetValidateCancelled() == false))
            {
                m_cfValidateProgress.Finished = true;
                SetValidateStatus("Validation complete !");
            }

        }// ValidateThreadProc()

        /// <summary>Handles events fired by the database compactor when the operation is finished</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnCompactorFinished(object sender, System.EventArgs e)
        {
            try
            {
                if ((m_wndCompactorStatus != null) && (m_wndCompactorStatus.Visible == true))
                {
                    m_wndCompactorStatus.Close();
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "OnCompactorFinished", this.ExBuilder.Message(ERROR_CASE_DATABASE_ON_COMPACTOR_FINISHED_EX), Ex);
            }

        }// private void OnCompactorFinished(object sender, System.EventArgs e)

        private void MoveDesignations(CDxPrimary dxDeposition)
        {
            string strOldFolder = "";
            string strNewFolder = "";
            string strSource = "";
            string strTarget = "";
            bool bFillTertiary = true;
            bool bFillSecondary = false;

            Debug.Assert(dxDeposition != null);
            Debug.Assert(dxDeposition.MediaType == TmaxMediaTypes.Deposition);
            Debug.Assert(dxDeposition.GetTranscript() != null);
            if (dxDeposition == null) return;
            if (dxDeposition.MediaType != TmaxMediaTypes.Deposition) return;
            if (dxDeposition.GetTranscript() == null) return;

            //	Move all designations to the transcript folder
            strNewFolder = GetTranscriptFolder(dxDeposition.GetTranscript());
            try
            {
                if (System.IO.Directory.Exists(strNewFolder) == false)
                    System.IO.Directory.CreateDirectory(strNewFolder);
            }
            catch
            {
                AddValidationError(dxDeposition.GetBarcode(true), "Unable to create folder for designations: " + strNewFolder, true);
                return;
            }

            //	Get the old folder
            strOldFolder = GetCasePath(TmaxMediaTypes.Designation);

            //	Get the list of secondary segments
            if (dxDeposition.Secondaries.Count == 0)
            {
                dxDeposition.Fill();
                bFillSecondary = true;
            }

            foreach (CDxSecondary S in dxDeposition.Secondaries)
            {
                //	Get the designations
                if (S.Tertiaries.Count == 0)
                {
                    S.Fill();
                    if (S.Tertiaries.Count == 0) continue;
                    else bFillTertiary = true;
                }

                foreach (CDxTertiary T in S.Tertiaries)
                {
                    strSource = strOldFolder;
                    strTarget = strNewFolder;

                    //	Build the paths to the files
                    if (T.Filename != null && (T.Filename.Length > 0))
                    {
                        strSource += T.Filename;
                        strTarget += T.Filename;
                    }
                    else
                    {
                        strSource += ("D_" + T.Secondary.AutoId.ToString() + "_" + T.AutoId.ToString() + "." + CXmlDesignation.GetExtension());
                        strTarget += ("D_" + T.Secondary.AutoId.ToString() + "_" + T.AutoId.ToString() + "." + CXmlDesignation.GetExtension());
                    }

                    //	Does the target already exist?
                    if (System.IO.File.Exists(strTarget) == true) continue;

                    SetValidateStatus("Moving " + strSource);

                    //	Move the file
                    if (System.IO.File.Exists(strSource) == true)
                    {
                        try
                        {
                            System.IO.File.Move(strSource, strTarget);
                        }
                        catch
                        {
                            AddValidationError(T.GetBarcode(true), "Unable to move designation from " + strSource + " to " + strTarget, true);
                        }

                    }

                }// foreach(CDxTertiary T in S.Tertiaries)

                if (bFillTertiary == true)
                    S.Tertiaries.Clear();

            }// foreach(CDxSecondary S in dxDeposition.Secondaries)

            if (bFillSecondary == true)
                dxDeposition.Secondaries.Clear();

        }

        /// <summary>This method is called to validate the specified primary record</summary>
        private void Validate(CDxPrimary dxPrimary)
        {
            string strBarcode = "";
            string strPath = "";
            bool bFilled = false;

            Debug.Assert(dxPrimary != null);

            //	Update the status text
            strBarcode = GetBarcode(dxPrimary, false);
            SetValidateStatus("Validating " + strBarcode + " ......");

            //	What type of primary record are we dealing with?
            switch (dxPrimary.MediaType)
            {
                case TmaxMediaTypes.Document:
                case TmaxMediaTypes.Recording:

                    if (dxPrimary.MediaType == TmaxMediaTypes.Document)
                        m_cfValidateProgress.Documents = m_cfValidateProgress.Documents + 1;
                    else
                        m_cfValidateProgress.Recordings = m_cfValidateProgress.Recordings + 1;

                    //	Make sure the folder exists
                    strPath = GetFolderSpec(dxPrimary, false);

                    //	Does the folder exist?
                    if (Validate(strPath, false) == false)
                    {
                        AddValidationError(strBarcode, "Folder not found: " + strPath, true);
                    }
                    else
                    {
                        //	Do we need to fill the secondaries collection
                        if ((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
                        {
                            dxPrimary.Fill();
                            bFilled = true;
                        }

                        //	Validate each of the secondaries
                        foreach (CDxSecondary O in dxPrimary.Secondaries)
                        {
                            if (GetValidateCancelled() == true)
                                break;

                            if (dxPrimary.MediaType == TmaxMediaTypes.Document)
                                m_cfValidateProgress.Pages = m_cfValidateProgress.Pages + 1;

                            Validate(O);
                        }
                    }
                    break;

                case TmaxMediaTypes.Powerpoint:

                    //	Make sure the presentation file exists
                    strPath = GetFileSpec(dxPrimary);

                    m_cfValidateProgress.PowerPoints = m_cfValidateProgress.PowerPoints + 1;

                    //	Does the file exist?
                    if (Validate(strPath, true) == false)
                    {
                        AddValidationError(strBarcode, "File not found: " + strPath, true);
                    }
                    break;

                case TmaxMediaTypes.Deposition:

                    m_cfValidateProgress.Depositions = m_cfValidateProgress.Depositions + 1;

                    //	Are we validating the associated transcript?
                    if (m_tmaxValidateOptions.Transcripts == true)
                    {
                        //	Make sure the transcript file exists
                        if (dxPrimary.GetTranscript() != null)
                        {
                            strPath = GetFileSpec(dxPrimary.GetTranscript());

                            if (Validate(strPath, true) == false)
                            {
                                AddValidationError(strBarcode, "Transcript not found: " + strPath, true);
                            }

                            //	Make sure all the designations are in the correct folder
                            MoveDesignations(dxPrimary);
                        }

                    }

                    //	Are we validating the associated segments?
                    if (m_tmaxValidateOptions.VideoFiles == true)
                    {
                        //	Do we need to fill the secondaries collection
                        if ((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
                        {
                            dxPrimary.Fill();
                            bFilled = true;
                        }

                        //	Validate each of the secondaries
                        foreach (CDxSecondary O in dxPrimary.Secondaries)
                        {
                            if (GetValidateCancelled() == true)
                                break;

                            m_cfValidateProgress.Videos = m_cfValidateProgress.Videos + 1;

                            Validate(O);
                        }

                    }
                    break;

                case TmaxMediaTypes.Script:

                    // NO FOLDER TO CHECK FOR

                    //	Do we need to fill the secondaries collection
                    if ((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
                    {
                        dxPrimary.Fill();
                        bFilled = true;
                    }

                    //	Validate each of the secondaries
                    foreach (CDxSecondary O in dxPrimary.Secondaries)
                    {
                        if (GetValidateCancelled() == true)
                            break;

                        Validate(O);
                    }
                    break;
            }

            //	Clear the secondaries if we filled them
            if (bFilled == true)
                dxPrimary.Secondaries.Clear();

        }// private void Validate(CDxPrimary dxPrimary)

        /// <summary>This method is called to validate the specified secondary record</summary>
        ///	<param name="dxSecondary">The secondary record to be validated</param>
        private void Validate(CDxSecondary dxSecondary)
        {
            CDxTertiary dxTertiary = null;
            string strBarcode = "";
            string strPath = "";
            bool bFilled = false;
            bool bTertiaryFilled = false;
            bool bFileNotFound = false;
            bool bVideo = false;

            Debug.Assert(dxSecondary != null);

            //	Update the status text
            strBarcode = GetBarcode(dxSecondary, false);
            SetValidateStatus("Validating " + strBarcode + " ......");

            //	What type of primary record are we dealing with?
            switch (dxSecondary.MediaType)
            {
                case TmaxMediaTypes.Page:

                    //	Make sure the file exists
                    strPath = GetFileSpec(dxSecondary);

                    //	Does the file exist?
                    if (Validate(strPath, true) == false)
                    {
                        AddValidationError(strBarcode, "File not found: " + strPath, true);
                    }
                    else
                    {
                        //	Do we need to fill the tertiaries collection
                        if ((dxSecondary.Tertiaries == null) || (dxSecondary.Tertiaries.Count == 0))
                        {
                            dxSecondary.Fill();
                            bFilled = true;
                        }

                        //	Validate each of the tertiaries
                        foreach (CDxTertiary O in dxSecondary.Tertiaries)
                        {
                            if (GetValidateCancelled() == true)
                                break;

                            //	Validate this record
                            Validate(O);
                        }

                    }
                    break;

                case TmaxMediaTypes.Slide:

                    //	The primary validation already tested for the
                    //	existance of the presentation file
                    break;

                case TmaxMediaTypes.Segment:

                    //	Make sure the file exists
                    strPath = GetFileSpec(dxSecondary);

                    //	Does the file exist?
                    if (Validate(strPath, true) == false)
                    {
                        AddValidationError(strBarcode, "File not found: " + strPath, true);
                    }

                    //	NOTE:	Don't verify child clips or designation here. They 
                    //			get validated by the scene that refrences them
                    break;

                case TmaxMediaTypes.Scene:

                    //	Get the source record
                    if (dxSecondary.GetSource() == null)
                    {
                        AddValidationError(strBarcode, "Source record not found (PSTQ): " + dxSecondary.SourceId, true);
                    }
                    else
                    {
                        //	Make sure the file associated with the source record exists
                        strPath = GetFileSpec(dxSecondary.GetSource());

                        if (GetValidateCancelled() == true)
                            break;

                        //	Make sure the source file exists
                        if (Validate(strPath, true) == false)
                        {
                            bFileNotFound = true;

                            //	Is this scene a designation or clip?
                            if ((dxSecondary.GetSource().MediaType == TmaxMediaTypes.Designation) ||
                               (dxSecondary.GetSource().MediaType == TmaxMediaTypes.Clip))
                            {
                                //	Should we attempt to create the XML file?
                                if (m_tmaxValidateOptions.CreateDesignations == true)
                                {
                                    dxTertiary = ((CDxTertiary)(dxSecondary.GetSource()));
                                    if (CreateXmlDesignation(dxTertiary, strPath) == true)
                                        bFileNotFound = false;
                                }

                            }

                        }// if(Validate(strPath, true) == false)

                        //	Were we unable to find the source file?
                        if (bFileNotFound == true)
                        {
                            AddValidationError(strBarcode, "Scene source file not found: " + strPath, true);
                        }
                        else
                        {
                            //	Is this scene a designation or clip?
                            if ((dxSecondary.GetSource().MediaType == TmaxMediaTypes.Designation) ||
                               (dxSecondary.GetSource().MediaType == TmaxMediaTypes.Clip))
                            {
                                dxTertiary = ((CDxTertiary)(dxSecondary.GetSource()));

                                //	Should we search for the segment?
                                if (dxTertiary.MediaType == TmaxMediaTypes.Designation)
                                    bVideo = m_tmaxValidateOptions.VideoFiles;
                                else
                                    bVideo = m_tmaxValidateOptions.Recordings;

                                //	Make sure the associated video segment exists
                                strPath = GetFileSpec(dxTertiary.Secondary);
                                if ((bVideo == true) && (Validate(strPath, true) == false))
                                {
                                    AddValidationError(strBarcode, "Scene source file not found: " + strPath, true);
                                }
                                else
                                {
                                    //	Does this record have any children
                                    if (dxTertiary.ChildCount > 0)
                                    {
                                        //	Do we need to fill the child collection?
                                        if ((dxTertiary.Quaternaries == null) ||
                                           (dxTertiary.Quaternaries.Count == 0))
                                        {
                                            dxTertiary.Fill();
                                            bTertiaryFilled = true;
                                        }

                                        //	Validate each link
                                        foreach (CDxQuaternary Q in dxTertiary.Quaternaries)
                                        {
                                            if (GetValidateCancelled() == true)
                                                break;

                                            Validate(Q, strBarcode);
                                        }

                                        if (bTertiaryFilled == true)
                                            dxTertiary.Quaternaries.Clear();

                                    }// if(dxTertiary.ChildCount > 0)

                                }

                            }

                        }// if(Validate(strPath, true) == false)

                    }
                    break;

            }

            //	Clear the secondaries if we filled them
            if (bFilled == true)
                dxSecondary.Tertiaries.Clear();

        }// private void Validate(CDxSecondary dxSecondary)

        /// <summary>This method is called to validate the specified tertiary record</summary>
        ///	<param name="dxSecondary">The tertiary record to be validated</param>
        private void Validate(CDxTertiary dxTertiary)
        {
            string strBarcode = "";
            string strPath = "";

            Debug.Assert(dxTertiary != null);

            //	What type of tertiary record are we dealing with?
            switch (dxTertiary.MediaType)
            {
                case TmaxMediaTypes.Treatment:

                    //	Update the status text
                    strBarcode = GetBarcode(dxTertiary, false);
                    SetValidateStatus("Validating " + strBarcode + " ......");

                    //	Make sure the treatment file exists
                    strPath = GetFileSpec(dxTertiary);
                    if (Validate(strPath, true) == false)
                    {
                        AddValidationError(strBarcode, "Treatment file not found: " + strPath, true);
                    }

                    //	Is this a split screen treatment?
                    if ((dxTertiary.SplitScreen == true) && (dxTertiary.SiblingId.Length > 0))
                    {
                        //	Make sure we can retrieve the sibling record
                        //
                        //	NOTE:	We don't have to validate the zap file because it will
                        //			be validated when the sibling record gets processed
                        if (dxTertiary.GetSibling(true) == null)
                        {
                            AddValidationError(strBarcode, "Split screen sibling record not found: " + dxTertiary.SiblingId, true);
                        }
                    }
                    break;

                //	These types are validated as part of the script scene
                //	to which they belong
                case TmaxMediaTypes.Designation:
                case TmaxMediaTypes.Clip:

                    break;

            }// switch(dxTertiary.MediaType)

        }// private void Validate(CDxTertiary dxTertiary)

        /// <summary>This method is called to validate the specified quaternary record</summary>
        ///	<param name="dxQuaternary">The quaternary record to be validated</param>
        ///	<param name="strSceneBarcode">The barcode of the scene that owns the link</param>
        private void Validate(CDxQuaternary dxQuaternary, string strSceneBarcode)
        {
            string strPath = "";

            Debug.Assert(dxQuaternary != null);

            //	What type of quaternary record are we dealing with?
            switch (dxQuaternary.MediaType)
            {
                case TmaxMediaTypes.Link:

                    //	No validation required if this is a hide link
                    if ((dxQuaternary.HideLink == false) && (dxQuaternary.SourceId.Length > 0))
                    {
                        //	Get the source record
                        if (dxQuaternary.GetSource() == null)
                        {
                            AddValidationError(strSceneBarcode, "Link source record not found (PSTQ): " + dxQuaternary.SourceId, true);
                        }
                        else
                        {
                            //	Make sure the file associated with the source record exists
                            strPath = GetFileSpec(dxQuaternary.GetSource());

                            if (Validate(strPath, true) == false)
                            {
                                AddValidationError(strSceneBarcode, "Link source file not found: " + strPath, true);
                            }

                        }

                    }
                    break;

                default:
                    break;

            }// switch(dxQuaternary.MediaType)

        }// private void Validate(CDxQuaternary dxQuaternary)

        /// <summary>This method is called to determine if the specified folder or file exists</summary>
        /// <param name="strPath">The path to the folder or file</param>
        /// <param name="bFile">True if strPath refers to a file</param>
        /// <returns>True if it exists</returns>
        private bool Validate(string strPath, bool bFile)
        {
            bool bExists = false;

            try
            {
                if (bFile == true)
                    bExists = System.IO.File.Exists(strPath);
                else
                    bExists = System.IO.Directory.Exists(strPath);
            }
            catch
            {
            }

            return bExists;

        }// private bool Validate(string strPath, bool bFile)

        /// <summary>This method will add all files in the source folder as secondary media objects</summary>
        /// <param name="tmaxSource">The folder containing the source files collection</param>
        /// <param name="dxPrimary">The primary record's data exchange object</param>
        /// <param name="lBarcodeId">The starting barcode identifier for new secondaries added to the database</param>
        /// <returns>true if successful</returns>
        private bool AddSource(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary, long lBarcodeId)
        {
            CDxSecondary dxSecondary;
            string strForeignBarcode = "";
            string strFilename = "";

            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.SubFolders != null);
            Debug.Assert(tmaxSource.Files.Count > 0);

            //	Make sure the folder where the media files get stored exists
            if (CreateFolder(dxPrimary, false, true) == false)
            {
                //	Ignore this condition if this is a deposition. We don't
                //	require them to have the video files when they register the deposition
                if (dxPrimary.MediaType != TmaxMediaTypes.Deposition)
                    return false;

            }// if(CreateFolder(dxPrimary, false, true) == false)

            //	Make sure we have a valid barcode id?
            if (lBarcodeId <= 0) lBarcodeId = 1;

            //	Add each file
            foreach (CTmaxSourceFile tmaxFile in tmaxSource.Files)
            {
                //	Check to see if the user cancelled the operation before adding the next file
                if (m_bRegisterCancelled) return true;

                //	Get a secondary exchange object for this file
                if ((dxSecondary = CreateSecondary(dxPrimary, tmaxFile)) != null)
                {
                    //	Transfer the file
                    if (TransferSource(dxPrimary, tmaxFile, true) == true)
                    {
                        //	Set the barcode identifier
                        if ((dxPrimary.MediaType == TmaxMediaTypes.Document) &&
                            (m_tmaxRegisterOptions.GetFlag(RegFlags.AssignFilenameToBarcode) == true))
                        {
                            try
                            {
                                strFilename = System.IO.Path.GetFileNameWithoutExtension(tmaxFile.Path);
                                dxSecondary.BarcodeId = System.Convert.ToInt64(strFilename);
                            }
                            catch
                            {
                                FireError(this, "AddSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_FILENAME_AS_BARCODE_EX, strFilename));
                                dxSecondary.BarcodeId = lBarcodeId;
                            }
                        }
                        else
                        {
                            dxSecondary.BarcodeId = lBarcodeId;
                        }

                        //	Add to the database
                        if (dxPrimary.Add(dxSecondary) == true)
                        {
                            //	Adjust the barcode id for the next record
                            lBarcodeId++;

                            //	Set the interfaces to the record exchange object
                            tmaxFile.IPrimary = dxPrimary;
                            tmaxFile.ISecondary = dxSecondary;

                            //	Set the flags to indicate that this file and it's parent folder have been registered
                            tmaxFile.Registered = true;

                            //	Do we need to add playback extents?
                            if (dxSecondary.MediaType == TmaxMediaTypes.Segment)
                            {
                                AddExtent(tmaxFile, dxSecondary);
                            }

                            //	Is this secondary created from an XML node?
                            if (tmaxFile.XmlSecondary != null)
                            {
                                //	Is a foreign barcode defined in the XML node
                                if (tmaxFile.XmlSecondary.ForeignBarcode.Length > 0)
                                    strForeignBarcode = tmaxFile.XmlSecondary.ForeignBarcode;
                            }

                            //	Should we used the filename as the foreign barcode?
                            if ((strForeignBarcode.Length == 0) && (dxSecondary.MediaType == TmaxMediaTypes.Page))
                            {
                                if ((m_tmaxRegisterOptions.ForeignBarcodeAdjustments.GetSelected(RegForeignBarcodeAdjustments.AssignFromFilename) == true) &&
                                   (dxSecondary.Filename.Length > 0))
                                {
                                    strForeignBarcode = System.IO.Path.GetFileNameWithoutExtension(dxSecondary.Filename);
                                }

                            }

                            //	Should we assign a foreign barcode?
                            if (strForeignBarcode.Length > 0)
                            {
                                dxSecondary.SetForeignBarcode(GetAdjustedForeignBarcode(strForeignBarcode), true, false);
                                strForeignBarcode = "";
                            }

                            //	Update the progress form
                            if ((tmaxSource.SourceType != RegSourceTypes.Adobe) &&
                               (tmaxSource.SourceType != RegSourceTypes.MultiPageTIFF))
                                UpdateProgressBar(null, null); // StepProgressCompleted();
                            // SetRegisterProgress("Added: " + tmaxFile.Path);

                        }

                    }// if(TransferSource(dxPrimary, tmaxFile) == true)

                }// if((dxSecondary = CreateSecondary(dxPrimary, tmaxSource, tmaxFile)) != null)

            }// foreach(CTmaxSourceFile tmaxFile in tmaxSource.Files)

            if (tmaxSource.SourceType == RegSourceTypes.MultiPageTIFF) // Adobe conversion class (TmaxPdfManager) updates progress bar by itself 
                UpdateProgressBar(null, null);

            return true;

        }// AddSource(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)

        /// <summary>
        /// This method will add the transcript information associated with the primary deposition
        /// </summary>
        /// <param name="tmaxSource">The folder containing the deposition descriptor</param>
        /// <param name="dxPrimary">The primary record's data exchange object</param>
        /// <returns>true if successful</returns>
        private bool AddTranscript(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)
        {
            CDxTranscript dxTranscript = null;
            CXmlDeposition xmlDeposition = null;
            string strFileSpec = "";
            bool bSuccessful = false;

            Debug.Assert(tmaxSource.UserData != null);
            if (tmaxSource.UserData == null) return false;

            try
            {
                xmlDeposition = (CXmlDeposition)(tmaxSource.UserData);

                //	Initialize a new transcript exchange object
                dxTranscript = new CDxTranscript(dxPrimary);
                dxTranscript.PrimaryId = dxPrimary.AutoId;
                dxTranscript.Deponent = xmlDeposition.Deponent;
                dxTranscript.DeposedOn = xmlDeposition.Date;
                dxTranscript.Filename = System.IO.Path.GetFileName(xmlDeposition.FileSpec);
                dxTranscript.LinesPerPage = xmlDeposition.LinesPerPage;
                dxTranscript.FirstPL = xmlDeposition.GetFirstPL();
                dxTranscript.LastPL = xmlDeposition.GetLastPL();

                //	Copy the transcript to its appropriate location
                strFileSpec = GetFileSpec(dxTranscript);

                if (System.IO.File.Exists(strFileSpec) == false)
                {
                    //	Always copy the XML file so that we don't disturb the user's source files
                    bSuccessful = ShellTransfer(xmlDeposition.FileSpec, strFileSpec, true);

                    //	Make sure it is not read-only
                    if (bSuccessful == true)
                    {
                        System.IO.File.SetAttributes(strFileSpec, System.IO.FileAttributes.Normal);
                    }
                    else
                    {
                        return false;
                    }

                }// if(System.IO.File.Exists(strFileSpec) == false)

                //	Make sure the temporary file no longer exists if we
                //	converted from a log file to an XML transcript
                if (xmlDeposition.Converted == true)
                {
                    if (System.IO.File.Exists(xmlDeposition.FileSpec) == true)
                    {
                        try { System.IO.File.Delete(xmlDeposition.FileSpec); }
                        catch { }
                    }

                }

                //	The user may have cancelled the file transfer
                if (m_bRegisterCancelled == false)
                    return dxPrimary.Add(dxTranscript);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddTranscript", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_TRANSCRIPT_EX, dxPrimary.MediaId), Ex);
            }

            return false;

        }// private bool AddTranscript(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)

        /// <summary>
        /// This method will add the extent information associated with the secondary segment
        /// </summary>
        /// <param name="tmaxSource">The file containing the extent information</param>
        /// <param name="dxPrimary">The secondary record's data exchange object</param>
        /// <returns>true if successful</returns>
        private bool AddExtent(CTmaxSourceFile tmaxSource, CDxSecondary dxSecondary)
        {
            CDxExtent dxExtent = null;
            CXmlSegment xmlSegment = null;

            Debug.Assert(dxSecondary != null);
            Debug.Assert(dxSecondary.Primary != null);

            try
            {
                //	Is this a recording segment?
                if (dxSecondary.Primary.MediaType == TmaxMediaTypes.Recording)
                {
                    dxExtent = new CDxExtent(dxSecondary);
                    dxExtent.SecondaryId = dxSecondary.AutoId;
                    dxExtent.Start = 0;
                    dxExtent.Stop = GetDuration(tmaxSource.Path);

                    //	Did the attempt to get the duration fail?
                    if (dxExtent.Stop < 0)
                        dxExtent = null;	//	prevent adding to the database
                }

                //	A deposition?
                else if (dxSecondary.Primary.MediaType == TmaxMediaTypes.Deposition)
                {
                    Debug.Assert(tmaxSource.UserData != null);
                    if (tmaxSource.UserData == null) return false;

                    xmlSegment = (CXmlSegment)(tmaxSource.UserData);

                    dxExtent = new CDxExtent(dxSecondary);
                    dxExtent.SecondaryId = dxSecondary.AutoId;
                    dxExtent.Start = xmlSegment.Start;
                    dxExtent.Stop = xmlSegment.Stop;
                    dxExtent.StartPL = xmlSegment.FirstPL;
                    dxExtent.StopPL = xmlSegment.LastPL;
                    dxExtent.StartTuned = xmlSegment.StartTuned;
                    dxExtent.StopTuned = xmlSegment.StopTuned;
                    dxExtent.XmlSegmentId = Convert.ToInt32(xmlSegment.Key);
                }

                //	Add to the database if we have a valid exchange object
                if (dxExtent != null)
                    return dxSecondary.Add(dxExtent);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddExtent", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_EXTENT_EX, GetBarcode(dxSecondary, false)), Ex);
                return false;
            }

            //	No extent added
            return false;

        }

        /// <summary>This method will add the extent information associated with the tertiary segment</summary>
        /// <param name="dxTertiary">The tertiary record that owns the extents</param>
        /// <param name="xmlDesignation">The xml designation descriptor</param>
        /// <returns>true if successful</returns>
        private bool AddExtent(CDxTertiary dxTertiary, CXmlDesignation xmlDesignation)
        {
            CDxExtent dxExtent = null;

            Debug.Assert(dxTertiary != null);
            Debug.Assert(dxTertiary.Secondary != null);

            try
            {
                dxExtent = new CDxExtent(dxTertiary);

                dxExtent.Start = xmlDesignation.Start;
                dxExtent.Stop = xmlDesignation.Stop;
                dxExtent.StartPL = xmlDesignation.FirstPL;
                dxExtent.StopPL = xmlDesignation.LastPL;
                dxExtent.StartTuned = xmlDesignation.StartTuned;
                dxExtent.StopTuned = xmlDesignation.StopTuned;
                dxExtent.HighlighterId = xmlDesignation.Highlighter;

                //	Should we add the time to play the last line?
                if (this.MaxLineTimeIndex >= 0)
                    dxExtent.MaxLineTime = xmlDesignation.GetMaxLineTime();
                else
                    dxExtent.MaxLineTime = -1.0;

                if ((dxTertiary.Secondary != null) && (dxTertiary.Secondary.GetExtent() != null))
                {
                    dxExtent.XmlSegmentId = dxTertiary.Secondary.GetExtent().XmlSegmentId;
                }

                //	Add to the database if we have a valid exchange object
                if (dxExtent != null)
                    return dxTertiary.Add(dxExtent);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddExtent", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_EXTENT_EX, GetBarcode(dxTertiary, false)), Ex);
                return false;
            }

            //	No extent added
            return false;

        }// private bool AddExtent(CDxTertiary dxTertiary, CXmlDesignation xmlDesignation)

        /// <summary>This method will add the extent information associated with the tertiary segment</summary>
        /// <param name="dxTertiary">The tertiary record that owns the extents</param>
        /// <param name="xmlLink">The xml link descriptor</param>
        /// <returns>true if successful</returns>
        private bool AddExtent(CDxTertiary dxTertiary, CXmlLink xmlLink)
        {
            CDxExtent dxExtent = null;

            Debug.Assert(dxTertiary != null);
            Debug.Assert(dxTertiary.Secondary != null);

            try
            {
                dxExtent = new CDxExtent(dxTertiary);

                dxExtent.Start = xmlLink.Start;
                dxExtent.StartPL = xmlLink.PL;
                dxExtent.StartTuned = xmlLink.StartTuned;
                dxExtent.StopPL = -1;
                dxExtent.Stop = -1;
                dxExtent.StopTuned = false;

                if ((dxTertiary.Secondary != null) && (dxTertiary.Secondary.GetExtent() != null))
                    dxExtent.XmlSegmentId = dxTertiary.Secondary.GetExtent().XmlSegmentId;

                //	Add to the database if we have a valid exchange object
                if (dxExtent != null)
                    return dxTertiary.Add(dxExtent);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddExtent", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_EXTENT_EX, GetBarcode(dxTertiary, false)), Ex);
                return false;
            }

            //	No extent added
            return false;

        }// private bool AddExtent(CDxTertiary dxTertiary, CXmlLink xmlLink)

        /// <summary>This method will add a new scene to the script using the specified record</summary>
        /// <param name="dxScript">The script to be added to</param>
        /// <param name="dxSource">The record to use as the source for the scene</param>
        /// <param name="bShortcut">True to treat new designations and clips as shortcuts</param>
        /// <param name="xmlScene">An optional XML scene used to initialize the record properties</param>
        /// <returns>The new scene exchange object</returns>
        private CDxSecondary AddScene(CDxPrimary dxScript, CDxMediaRecord dxSource, bool bShortcut, CXmlScene xmlScene)
        {
            CDxSecondary dxSecondary = null;
            string strId = "";
            CDxTertiary dxDuplicate = null;
            CDxScenes dxScenes = null;

            Debug.Assert(dxScript != null);
            Debug.Assert(dxSource != null);
            Debug.Assert(dxSource.GetMediaLevel() != TmaxMediaLevels.None);
            Debug.Assert(dxSource.GetMediaLevel() != TmaxMediaLevels.Primary);

            //	If the source record is itself a scene we want to add using it's source
            if (dxSource.MediaType == TmaxMediaTypes.Scene)
            {
                if (((CDxSecondary)dxSource).Source == null)
                    ((CDxSecondary)dxSource).Source = GetRecordFromId(((CDxSecondary)dxSource).SourceId, false);

                if (((CDxSecondary)dxSource).Source == null)
                    return null;
                else
                    return AddScene(dxScript, ((CDxSecondary)dxSource).Source, bShortcut, xmlScene);
            }

            //	What type of source media are we dealing with?
            switch (dxSource.MediaType)
            {
                case TmaxMediaTypes.Designation:
                case TmaxMediaTypes.Clip:

                    //	Should we add an additional reference to this record?
                    if (bShortcut == true)
                    {
                        //	We need to indicate that this record now has multiple references
                        if (((CDxTertiary)dxSource).HasShortcuts == false)
                        {
                            ((CDxTertiary)dxSource).HasShortcuts = true;
                            ((CDxTertiary)dxSource).Secondary.Tertiaries.Update((CDxTertiary)dxSource);
                        }

                    }
                    else
                    {
                        //	Create a duplicate only if there is another scene that references this record
                        //
                        //	NOTE:	We put this check in because this could be getting called as
                        //			a result of the user copying a designation from one script to another
                        //			or creating a brand new scene manually. Without this check, manual
                        //			additions result in an extra designation/clip that never gets referenced
                        if ((dxScenes = GetScenes(dxSource, false)) != null)
                        {
                            if (dxScenes.Count > 0)
                            {
                                //	Add a duplicate of the source record
                                if ((dxDuplicate = AddDuplicate((CDxTertiary)dxSource)) == null)
                                    return null;

                                //	The duplicate is now the source record
                                dxSource = dxDuplicate;

                                dxScenes.Clear();

                            }

                            dxScenes = null;

                        }// if((dxScenes = GetScenes(dxSource, false)) != null)

                    }
                    break;

            }// switch(dxSource.MediaType)

            //	Get the unique id for this source
            strId = GetUniqueId(dxSource);
            if ((strId == null) || (strId.Length == 0))
                return null;

            dxSecondary = new CDxSecondary(dxScript);
            dxSecondary.PrimaryMediaId = dxScript.AutoId;
            dxSecondary.DisplayOrder = dxScript.Secondaries.GetNextDisplayOrder();
            dxSecondary.BarcodeId = dxScript.Secondaries.GetNextBarcodeId();
            dxSecondary.MediaType = TmaxMediaTypes.Scene;
            dxSecondary.SourceType = dxSource.MediaType;
            dxSecondary.SourceId = strId;
            dxSecondary.Source = dxSource;

            //	Override with an XML scene?
            if (xmlScene != null)
                dxSecondary.SetProperties(xmlScene);

            if (dxScript.Add(dxSecondary) == true)
                return dxSecondary;
            else
                return null;

        }// AddScene(CDxPrimary dxScript, CDxMediaRecord dxSource)

        /// <summary>Called to extract the secondary source files for the specified record</summary>
        /// <param name="tmaxSource">The source folder that contains the files being registered</param>
        /// <param name="dxPrimary">The primary record that owns the source files</param>
        /// <returns>true if successful</returns>
        private bool ExtractSourceFiles(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)
        {
            //	If this is an Adobe PDF we need to export the file so that we can
            //	register the pages.
            //
            //	NOTE:	We assume the relative path has already been set so we know where
            //			to put the extracted pages
            if (tmaxSource.SourceType == RegSourceTypes.Adobe)
            {
                if (GetAdobeSource(dxPrimary, tmaxSource) == false)
                    return false;
            }
            else if (tmaxSource.SourceType == RegSourceTypes.MultiPageTIFF)
            {
                if (GetMultiPageTIFFSource(dxPrimary, tmaxSource) == false)
                    return false;
            }

            //	Set the number of children depending on the source type
            switch (tmaxSource.SourceType)
            {
                case RegSourceTypes.Powerpoint:

                    dxPrimary.ChildCount = 0;
                    break;

                default:

                    dxPrimary.ChildCount = tmaxSource.Files.Count;
                    break;

            }// switch(tmaxSource.SourceType)

            //	Set the name of the first secondary file
            if ((tmaxSource.Files != null) && (tmaxSource.Files.Count > 0))
                dxPrimary.Filename = tmaxSource.Files[0].Name;

            return true;

        }// private bool ExtractSourceFiles(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)

        /// <summary>This method will add a secondary media object for each slide in the primary PowerPoint presentation</summary>
        /// <param name="dxPrimary">The primary media object associated with the PowerPoint presentation</param>
        /// <param name="tmaxSource">The source folder associated with the presentation</param>
        /// <returns>true if successful</returns>
        private bool ExportSlides(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource)
        {
            long lSlides = 0;
            CTmaxSourceFile tmaxFile = null;

            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.Files.Count == 1);

            if (tmaxSource.Files == null) return false;
            if (tmaxSource.Files.Count == 0) return false; // Just playing it safe

            //	Transfer the presentation file
            if (TransferSource(dxPrimary, tmaxSource.Files[0], true) == false) return false;

            //	The user may have cancelled during the file transfer
            if (m_bRegisterCancelled == true) return false;

            //	Add secondary objects for each slide to the database
            if (ExportSlides(dxPrimary) == false) return false;

            //	Were we able to add any slides?
            if ((lSlides = dxPrimary.Secondaries.Count) > 0)
            {
                //	Prevent sorting based on filename because we want the
                //	objects in the files collection to represent the correct
                //	registration order
                tmaxSource.Files.KeepSorted = false;

                //	Set the interfaces on the caller's source file
                tmaxSource.Files[0].IPrimary = dxPrimary;
                tmaxSource.Files[0].ISecondary = dxPrimary.Secondaries[0];
                tmaxSource.Files[0].Registered = true;
                tmaxSource.Files[0].Initialize(GetFileSpec(dxPrimary.Secondaries[0]));

                //	Now we add new source file objects to make it look like there is one file for each slide
                for (int i = 1; i < lSlides; i++)
                {
                    if (m_bRegisterCancelled == true) return false;
                    tmaxFile = new CTmaxSourceFile(GetFileSpec(dxPrimary.Secondaries[i]));
                    tmaxFile.IPrimary = dxPrimary;
                    tmaxFile.ISecondary = dxPrimary.Secondaries[i];
                    tmaxFile.Registered = true;

                    tmaxSource.Files.Add(tmaxFile);

                }// for(int i = 1; i < lSlides; i++)

            }// if((lSlides = dxPrimary.Secondaries.Count) > 0)

            //	Update the progress form
            UpdateProgressBar(null, null);
            //SetRegisterProgress("Added: " + tmaxSource.Files[0].Path);

            return true;
        }

        /// <summary>This method will export the pages in the specified Adobe PDF document</summary>
        /// <param name="strAdobeFileSpec">The fully qualified path to the Adobe PDF file</param>
        /// <param name="strTarget">The target folder where the files should be stored</param>
        /// <returns>the number of pages exported by the conversion utility</returns>
        private int ExportAdobe(string strAdobeFileSpec, string strTarget)
        {
            Cursor oldCursor = Cursor.Current;
            int iPages = 0;
            int iFile = 1;
            string strFilename = "";

            //	Make sure the caller specified a target
            Debug.Assert(strTarget != null);
            if (strTarget == null) return 0;

            //	The converter program can not deal with a trailing backslash
            if (strTarget.EndsWith("\\") == true)
                strTarget = strTarget.Substring(0, strTarget.Length - 1);
            Debug.Assert(strTarget.Length > 0);
            if (strTarget.Length == 0) return 0;

            //	Verify that the source file exists
            Debug.Assert(System.IO.File.Exists(strAdobeFileSpec) == true);
            if (System.IO.File.Exists(strAdobeFileSpec) == false) return 0;

            //	Make sure the target folder exists
            if (CreateFolder(TmaxMediaTypes.Page, strTarget, true) == false)
            {
                if (!m_bRegisterCancelled)
                    FireError(this, "ExportAdobe", this.ExBuilder.Message(ERROR_CASE_DATABASE_PDF_CREATE_TARGET_FAILED, strTarget));
                return 0;
            }

            try
            {
                // Start the PDF Manager and provide the data needed for conversion
                CTmaxPDFManager PDFManager = new CTmaxPDFManager(strAdobeFileSpec.ToLower(), strTarget.ToLower(), m_tmaxRegisterOptions.OutputType, m_tmaxRegisterOptions.UseCustomDPI ? m_tmaxRegisterOptions.CustomDPI : (short)0, m_tmaxRegisterOptions.DisableCustomDither);
                PDFManager.notifyRegOptionsForm += new EventHandler(UpdateProgressBar);
                if (m_bRegisterCancelled == true)
                    return iPages;
                //lock (lockConversionTasksArray)
                //{
                if (ConversionTasksArray == null)
                    return iPages;
                ConversionTasksArray.Add(PDFManager);
                //}
                Console.WriteLine("Starting converting file = " + strAdobeFileSpec.ToLower());
                bool status = PDFManager.StartConversion();

                // If the new PDF Manager fails to convert, add exception to the Log file so the user can track why the conversion failed
                if (!status)
                {
                    Console.WriteLine("File completed un - successfully" + strAdobeFileSpec.ToLower());
                    logUser.Error(Path.GetFileName(strAdobeFileSpec) + "                Status: UnSuccessful");
                    if (!m_bRegisterCancelled)
                        FireError(this, "ExportAdobe", this.ExBuilder.Message(ERROR_CASE_DATABASE_EXPORT_ADOBE_EX, strAdobeFileSpec));
                    return iPages;
                }
                else // File was converted successfully
                {
                    //lock (lockConversionTasksArray)
                    //{
                    if (ConversionTasksArray == null)
                    {
                        return iPages;
                    }
                    ConversionTasksArray.Remove(PDFManager);
                    //}
                    if (strTarget.EndsWith("\\") == false)
                        strTarget += "\\";
                    while (true)
                    {
                        strFilename = String.Format("{0}{1:0000}.png", strTarget, iFile);
                        if (System.IO.File.Exists(strFilename) == true)
                        {
                            iFile++;
                        }
                        else
                        {
                            strFilename = String.Format("{0}{1:0000}.tif", strTarget, iFile);
                            if (System.IO.File.Exists(strFilename) == true)
                            {
                                iFile++;
                            }
                            else
                            {
                                //	Update the progress form
                                if (iFile > 1)
                                {
                                    // Previously the Progress status was updated as follows. But in the new implementation, since multiple files could complete
                                    // at once, we have removed the functionality of showing the Progress status and now only Progress bar is maintained
                                    // SetRegisterProgress("Exported " + System.IO.Path.GetFileName(strAdobeFileSpec) + " page " + (iFile - 1).ToString());
                                }
                                iPages = iFile - 1;
                                break;
                            }

                        }
                    }
                }
                PDFManager.Dispose();
                PDFManager = null;
                if (iPages > 0)
                    logUser.Info(Path.GetFileName(strAdobeFileSpec) + "             Status: Successful");
                else
                {
                    logUser.Error(Path.GetFileName(strAdobeFileSpec) + "                Status: UnSuccessful");
                    FireError(this, "ExportAdobe", this.ExBuilder.Message(ERROR_CASE_DATABASE_EXPORT_ADOBE_EX, strAdobeFileSpec));
                    // Console.WriteLine("File completed successfully" + strAdobeFileSpec.ToLower());
                }
            }
            catch (ThreadAbortException Ex)
            {
                logUser.Error(Path.GetFileName(strAdobeFileSpec) + "                Status: UnSuccessful");
                logDetailed.Error(Ex.ToString());
                Console.WriteLine("Exception was thrown here. Caught you." + Ex.ToString());
            }
            catch (System.Exception Ex)
            {
                logUser.Error(Path.GetFileName(strAdobeFileSpec) + "                Status: UnSuccessful");
                logDetailed.Error(Ex.ToString());
                FireError(this, "ExportAdobe", this.ExBuilder.Message(ERROR_CASE_DATABASE_EXPORT_ADOBE_EX, strAdobeFileSpec), Ex);
            }

            //	Restore the cursor
            Cursor.Current = oldCursor != null ? oldCursor : Cursors.Default;

            return iPages;

        }// private bool ExportAdobe(string strAdobeFileSpec, CTmaxSourceFolder tmaxTarget)

        /// <summary>This method will get the source files for the specified Adobe PDF document</summary>
        /// <param name="dxPrimary">The primary media object associated with the Adobe PDF</param>
        /// <param name="tmaxSource">The source folder associated with the Adobe PDF</param>
        /// <returns>true if successful</returns>
        private bool GetAdobeSource(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource)
        {
            string strFileSpec = "";
            string strTarget = "";
            int iPages = 0;
            bool bSuccessful = false;

            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.Files.Count == 1);

            if (tmaxSource.Files == null) return false;
            if (tmaxSource.Files.Count != 1) return false; // Just playing it safe

            //	Get the folder where the pages will be stored
            strTarget = GetFolderSpec(dxPrimary, true);
            if (strTarget.EndsWith("\\") == false)
                strTarget += "\\";

            //	Get the path to the PDF file
            strFileSpec = tmaxSource.Files[0].Path;
            if (strFileSpec.Length == 0) return false;

            //	Update the progress form
            // SetRegisterProgress("Exporting " + System.IO.Path.GetFileName(strFileSpec) + " pages ...");

            //------------------------------------------------------------------------------//

            bool isDuplicate = Directory.Exists(strTarget.ToLower());

            string strNewName = string.Empty;
            lock (lockForConflictForm)
            {
                while (isDuplicate)
                {
                    CFResolveConflict wndResolve = new CFResolveConflict();
                    SetHandlers(wndResolve.EventSource);
                    string tempName = string.Empty;
                    string path = GetCasePath(TmaxMediaTypes.Document);
                    strNewName = m_tmaxRegisterOptions.Resolve(dxPrimary, dxPrimary.RelativePath, RegConflictResolutions.Automatic);
                    if (m_bAutoResolve == true)
                    {
                        path += strNewName;
                    }
                    else if (m_tmaxRegisterOptions.ConflictResolution == RegConflictResolutions.Prompt)
                    {
                        //	Initialize the form
                        wndResolve.IsMediaId = true;
                        wndResolve.Conflict = dxPrimary.RelativePath;// the locally adjusted conflict 
                        wndResolve.Resolution = strNewName; // Initialize with last value
                        if (wndResolve.Source.Length == 0)
                            wndResolve.Source = tmaxSource.Path + "\\" + dxPrimary.RelativePath + (dxPrimary.MediaType == TmaxMediaTypes.Document ? ".pdf" : "");
                        //	Open the form
                        DisableTmaxKeyboard(true);
                        wndResolve.TopMost = true;
                        FTI.Shared.Win32.User.MessageBeep(0);
                        m_cfRegisterProgress.DisableForm(); // Disable the Progress Form when autoresolve screen appears
                        DialogResult conflictResolveResult = wndResolve.ShowDialog();
                        if (conflictResolveResult == DialogResult.OK)
                        {
                            DisableTmaxKeyboard(false);
                        }
                        else if (conflictResolveResult == DialogResult.Cancel)
                        {
                            dxPrimary.RelativePath = @wndResolve.Resolution;
                            bSuccessful = false;
                            m_cfRegisterProgress.EnableForm(); // Enable the Progress Form when autoresolve screen is closed
                            return bSuccessful;
                        }
                        else
                        {
                            try
                            {
                                if (m_gvi == null)
                                    m_gvi = new GhostscriptVersionInfo(@"PDFManager\gsdll32.dll"); ;
                                if (m_rasterizer != null)
                                {
                                    m_rasterizer.Dispose();
                                    m_rasterizer = null;
                                }
                                m_rasterizer = new GhostscriptRasterizer();
                                m_rasterizer.Open(strFileSpec, m_gvi, false);
                                m_totalPages += m_rasterizer.PageCount;

                                if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                                    m_cfRegisterProgress.CompletedPages = m_cfRegisterProgress.CompletedPages + m_rasterizer.PageCount;

                                m_rasterizer.Dispose();
                                m_rasterizer = null;
                            }
                            catch (Exception Ex)
                            {
                                logDetailed.Error(Ex.ToString());
                            }
                            bSuccessful = false;
                            m_cfRegisterProgress.EnableForm(); // Enable the Progress Form when autoresolve screen is closed
                            return bSuccessful;
                        }
                        m_cfRegisterProgress.EnableForm(); // Enable the Progress Form when autoresolve screen is closed
                        m_cfRegisterProgress.Activate();
                        if (wndResolve.AutoResolveAll == true)
                            m_bAutoResolve = true;
                        path += @wndResolve.Resolution.Trim();
                        strNewName = wndResolve.Resolution.Trim();
                    }
                    else
                    {
                        strNewName = m_tmaxRegisterOptions.Resolve(dxPrimary, dxPrimary.RelativePath, RegConflictResolutions.Automatic);
                        path += strNewName;
                    }
                    isDuplicate = Directory.Exists(path.ToLower());
                }
            }
            if (!string.IsNullOrEmpty(strNewName))
            {
                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(System.IO.Path.GetTempPath() + strNewName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(System.IO.Path.GetTempPath() + strNewName));
                    }
                    File.Copy(strFileSpec, System.IO.Path.GetTempPath() + strNewName + ".pdf");
                }
                catch (IOException Ex)
                {
                    File.Delete(System.IO.Path.GetTempPath() + strNewName + ".pdf");
                    File.Copy(strFileSpec, System.IO.Path.GetTempPath() + strNewName + ".pdf");
                }
                strFileSpec = System.IO.Path.GetTempPath() + strNewName + ".pdf";
                dxPrimary.RelativePath = strNewName;
                strTarget = GetFolderSpec(dxPrimary, true);
                if (strTarget.EndsWith("\\") == false)
                    strTarget += "\\";
            }
            //------------------------------------------------------------------------------//

            //	Export the PDF pages to the target folder
            if ((iPages = ExportAdobe(strFileSpec, strTarget)) != 0)
            {
                //  ------------------Delete PDF file that was copied to temporary folder---------------------------
                if (!string.IsNullOrEmpty(strNewName))
                {
                    try
                    {
                        string temporaryDirectory = @System.IO.Path.GetTempPath();
                        string parentPath = strNewName;   // This will store the parent folder name in which the file was store if a folder was selected while importing
                        while (true)
                        {
                            string temp = Path.GetDirectoryName(parentPath);
                            if (String.IsNullOrEmpty(temp))
                                break;
                            parentPath = temp;
                        }
                        if (!string.IsNullOrEmpty(parentPath) && Directory.Exists(temporaryDirectory + parentPath))
                        {
                            Console.WriteLine("Deleting source folder = " + temporaryDirectory + parentPath);
                            Directory.Delete(temporaryDirectory + parentPath, true);
                        }
                        else
                        {
                            File.Delete(strFileSpec);
                        }
                    }
                    catch (IOException Ex)
                    {
                        Console.WriteLine("Exception while deleting source folder = " + Ex.ToString());
                        //  Do Nothing
                    }
                }
                //  --------------------------------------------------------------------------------------------------
                //	Change the source path to match the target
                tmaxSource.Initialize(strTarget);

                //	Get the exported files
                tmaxSource.GetFiles("*.tif,*.tiff,*.png", true);

                //	Do the page count's match?
                if ((iPages > 0) && (iPages != tmaxSource.GetFileCount(false)))
                {
                    //	Warn the user that the page counts do not match
                    FireError(this, "GetAdobeSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_PDF_PAGE_COUNTS, iPages.ToString(), tmaxSource.GetFileCount(false).ToString()));
                }

                //	Make sure the files in the collection are in sorted order
                tmaxSource.Files.Sort(true);

                bSuccessful = true;

            }
            else
            {
                //  -----------Delete exported files since there was an error or user cancelled the opertation-------
                string caseDirectory = GetCasePath(TmaxMediaTypes.Document); ;
                if (strTarget.Contains(caseDirectory.ToLower()))
                {
                    int index = strTarget.IndexOf(caseDirectory.ToLower());
                    string cleanPath = (index < 0)
                        ? strTarget
                        : strTarget.Remove(index, caseDirectory.Length);

                    try
                    {
                        string parentPath = cleanPath;   // This will store the parent folder name in which the file was store if a folder was selected while importing
                        parentPath = Path.GetDirectoryName(parentPath);
                        if (!string.IsNullOrEmpty(parentPath) && Directory.Exists(caseDirectory + parentPath))
                        {
                            Console.WriteLine("Deleting directory= " + caseDirectory + parentPath);

                            DeleteDirectory(caseDirectory + parentPath);
                        }
                        else
                        {
                            File.Delete(strTarget);
                        }
                    }
                    catch (IOException Ex)
                    {
                        //  Do Nothing
                    }
                }
                //  ------------------Delete PDF file that was copied to temporary folder---------------------------
                if (!string.IsNullOrEmpty(strNewName))
                {
                    try
                    {
                        string temporaryDirectory = @System.IO.Path.GetTempPath();
                        string parentPath = strNewName;   // This will store the parent folder name in which the file was store if a folder was selected while importing
                        while (true)
                        {
                            string temp = Path.GetDirectoryName(parentPath);
                            if (String.IsNullOrEmpty(temp))
                                break;
                            parentPath = temp;
                        }
                        if (!string.IsNullOrEmpty(parentPath) && Directory.Exists(temporaryDirectory + parentPath))
                        {
                            Console.WriteLine("Deleting source folder = " + temporaryDirectory + parentPath);
                            Directory.Delete(temporaryDirectory + parentPath, true);
                        }
                        else
                        {
                            File.Delete(strFileSpec);
                        }
                    }
                    catch (IOException Ex)
                    {
                        Console.WriteLine("Exception while deleting source folder = " + Ex.ToString());
                        //  Do Nothing
                    }
                }
                //  -------------------------------------------------------------------------------------------------

                //	Delete the target since no pages were exported
                try { System.IO.Directory.Delete(strTarget, true); }
                catch { }
                return false;
            }

            return bSuccessful;

        }// private bool GetAdobeSource(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource)

        /// <summary>This method will delete the directory and any files that exists in that directory.</summary> 
        /// <summary>Used for deleting Temporary files and un-complete exported PDF's.</summary>
        /// <param name="directoryPath">The directory path that will be deleted</param>
        /// <returns>void</returns>
        private void DeleteDirectory(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath);
            string[] dirs = Directory.GetDirectories(directoryPath);

            try
            {
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            catch { }

            try
            {
                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }
            }
            catch { }

            try
            {
                Directory.Delete(directoryPath, false);
            }
            catch { }
        }
        /// <summary>This method will delete the specified media record</summary>
        /// <param name="dxRecord">The record to be deleted</param>
        /// <param name="tmaxDeleted">Collection of items representing deleted records</param>
        /// <param name="rbCancel">Reference to cancellation variable</param>
        /// <returns>true if ok to continue deleting records</returns>
        private bool DeleteMedia(CDxMediaRecord dxRecord, CTmaxItems tmaxDeleted, ref bool rbCancel)
        {
            CFConfirmMediaDelete wndConfirmation = null;
            CDxScenes dxScenes = null;
            CDxScenes dxSiblingScenes = null;
            CDxLinks dxLinks = null;
            CDxBinderEntries dxBinderEntries = null;
            CDxBinderEntries dxSiblingBinderEntries = null;
            CDxMediaRecords dxSource = null;
            CDxTertiary dxSibling = null;
            bool bConfirmDelete = true;
            int iReferences = 0;
            CTmaxSourceFolder tmaxScratched = new CTmaxSourceFolder();

            Debug.Assert(dxRecord != null);

            //	Initialize the cancellation variable
            rbCancel = false;

            //	Search for any scenes, links, or binders that might be referencing this record
            if ((dxScenes = GetScenes(dxRecord, true)) != null)
                iReferences += dxScenes.Count;
            if ((dxLinks = GetLinks(dxRecord)) != null)
                iReferences += dxLinks.Count;
            if ((dxBinderEntries = GetBinderEntries(dxRecord)) != null)
                iReferences += dxBinderEntries.Count;

            try
            {
                if (dxRecord.MediaType == TmaxMediaTypes.Treatment)
                {
                    if (((CDxTertiary)dxRecord).SplitScreen == true)
                    {
                        dxSibling = ((CDxTertiary)dxRecord).Sibling;
                        if (dxSibling != null)
                        {
                            iReferences += 1;

                            //	Now check for binder entries or scenes that may reference
                            //	the sibling
                            if ((dxSiblingScenes = GetScenes(dxSibling, true)) != null)
                                iReferences += dxSiblingScenes.Count;
                            if ((dxSiblingBinderEntries = GetBinderEntries(dxSibling)) != null)
                                iReferences += dxSiblingBinderEntries.Count;

                        }// if(dxSibling != null)

                    }// if(((CDxTertiary)dxRecord).SplitScreen == true)

                }// if(dxRecord.MediaType == TmaxMediaTypes.Treatment)
            }
            catch (System.Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
                return false;
            }

            //	Can we bypass the user prompt?
            if (m_bDeletingReferences == true)
            {
                bConfirmDelete = false;
            }
            else if (m_bDeleteAll == true)
            {
                //	Do we need to override the Delete All request because of existing references?
                if ((iReferences == 0) || (m_tmaxAppOptions.ConfirmDeleteReferences == false))
                    bConfirmDelete = false;
            }
            else
            {
                //	What type of record is being deleted?
                switch (dxRecord.MediaType)
                {

                    //	Don't need confirmation for these types
                    case TmaxMediaTypes.Script:
                    case TmaxMediaTypes.Scene:
                    case TmaxMediaTypes.Link:
                    case TmaxMediaTypes.Designation:
                    case TmaxMediaTypes.Clip:

                        bConfirmDelete = false;
                        break;
                }

            }

            //	Should we display the confirmation form?
            if (bConfirmDelete == true)
            {
                wndConfirmation = new CFConfirmMediaDelete();

                //	Set the form properties
                wndConfirmation.ManagerOptions = m_tmaxAppOptions;
                wndConfirmation.Record = dxRecord;
                if (dxScenes != null)
                {
                    foreach (CDxMediaRecord O in dxScenes)
                        wndConfirmation.References.Add(O);
                }
                if (dxLinks != null)
                {
                    foreach (CDxMediaRecord O in dxLinks)
                        wndConfirmation.References.Add(O);
                }
                if (dxBinderEntries != null)
                {
                    foreach (CDxMediaRecord O in dxBinderEntries)
                        wndConfirmation.References.Add(O);
                }
                if (dxSibling != null)
                {
                    wndConfirmation.References.Add(dxSibling);
                }
                if (dxSiblingScenes != null)
                {
                    foreach (CDxMediaRecord O in dxSiblingScenes)
                        wndConfirmation.References.Add(O);
                }
                if (dxSiblingBinderEntries != null)
                {
                    foreach (CDxMediaRecord O in dxSiblingBinderEntries)
                        wndConfirmation.References.Add(O);
                }

                switch (wndConfirmation.ShowDialog())
                {
                    case DialogResult.No:

                        //	Don't delete but OK to continue
                        return false;

                    case DialogResult.Cancel:

                        // Don't delete and don't process any more requests to delete
                        rbCancel = true;
                        return false;

                    case DialogResult.Yes:

                        m_bDeleteAll = wndConfirmation.YesToAll;
                        break;
                }

            }// if(bConfirmDelete == true)

            //	Get the files that should be deleted after we delete the record
            GetScratchFiles(dxRecord, tmaxScratched);

            //	What level of record are we deleting?
            switch (dxRecord.GetMediaLevel())
            {
                case TmaxMediaLevels.Primary:

                    //	Drill down and flush all children
                    if (((CDxPrimary)dxRecord).Secondaries.Count == 0)
                        ((CDxPrimary)dxRecord).Secondaries.Fill();

                    foreach (CDxSecondary dxSecondary in ((CDxPrimary)dxRecord).Secondaries)
                    {
                        if (dxSecondary.MediaType == TmaxMediaTypes.Scene)
                        {
                            //	Queue the source record to see if it should be deleted
                            try
                            {
                                if (dxSource == null) dxSource = new CDxMediaRecords();
                                if (dxSecondary.GetSource() != null)
                                    dxSource.AddList(dxSecondary.GetSource());
                            }
                            catch
                            {
                            }

                        }
                        else
                        {
                            if (dxSecondary.Tertiaries.Count == 0)
                                dxSecondary.Tertiaries.Fill();

                            //	Delete all quaternary children
                            foreach (CDxTertiary dxTertiary in dxSecondary.Tertiaries)
                                dxTertiary.Quaternaries.Flush();

                            //	Delete all tertiary children
                            dxSecondary.Tertiaries.Flush();
                        }

                    }// foreach(CDxSecondary dxSecondary in ((CDxPrimary)dxRecord).Secondaries)

                    //	Delete all secondary children
                    ((CDxPrimary)dxRecord).Secondaries.Flush();

                    //	Delete the primary record
                    if ((m_dxPrimaries.Delete(dxRecord)) == true)
                    {
                        //	Clean up the barcode map
                        if (m_dxBarcodeMap != null)
                            m_dxBarcodeMap.OnSourceDeleted(dxRecord);

                        //	Remove all the codes
                        dxRecord.DeleteCodes();

                        //	Add an item to the caller's collection
                        tmaxDeleted.Add(new CTmaxItem(dxRecord));
                    }
                    else
                    {
                        return false;
                    }
                    break;

                case TmaxMediaLevels.Secondary:

                    if (dxRecord.MediaType == TmaxMediaTypes.Scene)
                    {
                        //	Queue the source record to see if it should be deleted
                        try
                        {
                            if (dxSource == null) dxSource = new CDxMediaRecords();
                            if (((CDxSecondary)dxRecord).GetSource() != null)
                                dxSource.AddList(((CDxSecondary)dxRecord).GetSource());
                        }
                        catch
                        {
                        }

                    }
                    else
                    {
                        //	Drill down and flush all children
                        if (((CDxSecondary)dxRecord).Tertiaries.Count == 0)
                            ((CDxSecondary)dxRecord).Tertiaries.Fill();

                        //	Delete all quaternary children
                        foreach (CDxTertiary dxTertiary in ((CDxSecondary)dxRecord).Tertiaries)
                            dxTertiary.Quaternaries.Flush();

                        //	Delete all tertiary children
                        ((CDxSecondary)dxRecord).Tertiaries.Flush();

                    }// if(dxRecord.MediaType == TmaxMediaTypes.Scene)						

                    //	Delete the secondary record
                    if ((((CDxSecondary)dxRecord).Collection.Delete(dxRecord)) == true)
                    {
                        //	Clean up the barcode map
                        if (m_dxBarcodeMap != null)
                            m_dxBarcodeMap.OnSourceDeleted(dxRecord);

                        //	Remove all the codes
                        dxRecord.DeleteCodes();

                        //	Add an item to the caller's collection
                        tmaxDeleted.Add(new CTmaxItem(dxRecord));
                    }
                    else
                    {
                        return false;
                    }
                    break;

                case TmaxMediaLevels.Tertiary:

                    //	Do we also need to delete a sibling treatment?
                    if (((CDxTertiary)dxRecord).SplitScreen == true)
                        dxSibling = ((CDxTertiary)dxRecord).Sibling;

                    //	Flush the quaternary children
                    ((CDxTertiary)dxRecord).Quaternaries.Flush();
                    if (dxSibling != null)
                        dxSibling.Quaternaries.Flush();

                    //	Delete the tertiary record and it's sibling record
                    if (dxSibling != null)
                    {
                        if (dxSibling.Collection.Delete(dxSibling) == false)
                            return false;

                        //	Make sure the zap file gets deleted
                        GetScratchFiles(dxSibling, tmaxScratched);
                    }
                    if ((((CDxTertiary)dxRecord).Collection.Delete(dxRecord)) == false)
                    {
                        return false;
                    }

                    //	Clean up the barcode map
                    if (m_dxBarcodeMap != null)
                    {
                        m_dxBarcodeMap.OnSourceDeleted(dxRecord);
                        if (dxSibling != null)
                            m_dxBarcodeMap.OnSourceDeleted(dxSibling);
                    }

                    //	Remove all the codes
                    dxRecord.DeleteCodes();
                    if (dxSibling != null)
                        dxSibling.DeleteCodes();

                    //	Add an item to the caller's collection
                    tmaxDeleted.Add(new CTmaxItem(dxRecord));
                    if (dxSibling != null)
                        tmaxDeleted.Add(new CTmaxItem(dxSibling));

                    break;

                case TmaxMediaLevels.Quaternary:

                    //	Delete the specified record
                    if ((((CDxQuaternary)dxRecord).Collection.Delete(dxRecord)) == true)
                    {
                        tmaxDeleted.Add(new CTmaxItem(dxRecord));
                    }
                    break;

                default:

                    Debug.Assert(false);
                    return false;
            }

            try
            {
                //	Prevent the prompt from displaying while we delete the references
                m_bDeletingReferences = true;

                //	Do we have any script scenes that need to be deleted?
                if ((dxScenes != null) && (dxScenes.Count > 0))
                {
                    Delete(dxScenes);
                }

                //	Do we have any links that need to be deleted?
                if ((dxLinks != null) && (dxLinks.Count > 0))
                {
                    Delete(dxLinks);
                }

                //	Do we have any binders that need to be deleted?
                if ((dxBinderEntries != null) && (dxBinderEntries.Count > 0))
                {
                    Delete(dxBinderEntries);
                }

                //	Do we have any sibling treatment binder entries that need to be deleted?
                if ((dxSiblingBinderEntries != null) && (dxSiblingBinderEntries.Count > 0))
                {
                    Delete(dxSiblingBinderEntries);
                }

                //	Do we have any sibling script scenes that need to be deleted?
                if ((dxSiblingScenes != null) && (dxSiblingScenes.Count > 0))
                {
                    Delete(dxSiblingScenes);
                }

                //	Do we have any source records for scenes?
                if ((dxSource != null) && (dxSource.Count > 0))
                {
                    DeleteSource(dxSource);
                }

                m_bDeletingReferences = false;

                //	Delete the scratched files
                if (tmaxScratched != null)
                {
                    DeleteScratchedFiles(tmaxScratched);
                    tmaxScratched = null;
                }

            }
            catch (System.Exception Ex)
            {
                m_bDeletingReferences = false;
                FireDiagnostic(this, "DeleteMedia", Ex.ToString());
            }

            return true; // Keep going

        }//	private bool DeleteMedia(CDxMediaRecord dxRecord, CTmaxItems tmaxDeleted, ref bool rbCancel)

        /// <summary>This method will delete the scenes contained in the collection</summary>
        /// <param name="dxScenes">The collection of scenes to be deleted</param>
        /// <returns>True if successful</returns>
        private bool Delete(CDxScenes dxScenes)
        {
            CTmaxItems tmaxScripts = null;
            CTmaxItem tmaxScript = null;
            CDxPrimary dxScript = null;

            //	Make sure there is something to delete
            if ((dxScenes == null) || (dxScenes.Count == 0)) return true;

            //	We're going to trick the system into thinking the application
            //	is requesting removal of these scenes
            tmaxScripts = new CTmaxItems();

            foreach (CDxSecondary dxScene in dxScenes)
            {
                //	This should be a valid application record
                Debug.Assert(dxScene.Primary != null);
                Debug.Assert(dxScene.Primary.Secondaries.Count != 0);
                if (dxScene.Primary == null) continue;

                dxScript = dxScene.Primary;
                if (dxScript.Secondaries.Count == 0)
                    dxScript.Secondaries.Fill();

                //	Do we need to locate the item used to identify the script?
                if ((tmaxScript == null) || (ReferenceEquals(tmaxScript.GetMediaRecord(), dxScript) == false))
                {
                    //	Is it already in the collection
                    if ((tmaxScript = tmaxScripts.Find(dxScript)) == null)
                    {
                        //	Add a new script item
                        tmaxScript = new CTmaxItem(dxScript);
                        tmaxScripts.Add(tmaxScript);
                    }

                }

                //	Add to the parent's subitem collection
                if (tmaxScript != null)
                    tmaxScript.SubItems.Add(new CTmaxItem(dxScene));

            }// foreach(CDxSecondary dxScene in dxScenes)

            //	Now that we have the scenes grouped by script we can
            //	ask the system to delete them
            foreach (CTmaxItem O in tmaxScripts)
            {
                DeleteMedia(O);
            }

            //	Clear the collection
            dxScenes.Clear();

            return true;

        }// private bool Delete(CDxScenes dxScenes)

        /// <summary>This method will delete the links contained in the collection</summary>
        /// <param name="dxScenes">The collection of links to be deleted</param>
        /// <returns>True if successful</returns>
        private bool Delete(CDxLinks dxLinks)
        {
            CTmaxItems tmaxParents = null;
            CTmaxItem tmaxParent = null;
            CDxTertiary dxTertiary = null;

            //	Make sure there is something to delete
            if ((dxLinks == null) || (dxLinks.Count == 0)) return true;

            //	We're going to trick the system into thinking the application
            //	is requesting removal of these scenes
            tmaxParents = new CTmaxItems();

            foreach (CDxQuaternary dxLink in dxLinks)
            {
                //	This should be a valid application record
                Debug.Assert(dxLink.Tertiary != null);
                Debug.Assert(dxLink.Tertiary.Quaternaries.Count != 0);
                if (dxLink.Tertiary == null) continue;

                dxTertiary = dxLink.Tertiary;
                if (dxTertiary.Quaternaries.Count == 0)
                    dxTertiary.Quaternaries.Fill();

                //	Do we need to locate the item used to identify the parent?
                if ((tmaxParent == null) || (ReferenceEquals(tmaxParent.GetMediaRecord(), dxTertiary) == false))
                {
                    //	Is it already in the collection
                    if ((tmaxParent = tmaxParents.Find(dxTertiary)) == null)
                    {
                        //	Add a new script item
                        tmaxParent = new CTmaxItem(dxTertiary);
                        tmaxParents.Add(tmaxParent);
                    }

                }

                //	Add to the parent's subitem collection
                if (tmaxParent != null)
                    tmaxParent.SubItems.Add(new CTmaxItem(dxLink));

            }// foreach(CDxQuaternary dxLink in dxLinks)

            //	Now ask the system to delete the links
            foreach (CTmaxItem O in tmaxParents)
            {
                DeleteMedia(O);
            }

            //	Clear the collection
            dxLinks.Clear();

            return true;

        }// private bool Delete(CDxLinks dxLinks)

        /// <summary>This method will delete the binders contained in the collection</summary>
        /// <param name="dxBinders">The collection of binders to be deleted</param>
        /// <returns>True if successful</returns>
        private bool Delete(CDxBinderEntries dxBinders)
        {
            CTmaxItems tmaxParents = null;
            CTmaxItem tmaxParent = null;
            CTmaxItem tmaxBinder = null;
            CDxBinderEntry dxBinder = null;
            CDxBinderEntry dxParent = null;

            //	Make sure there is something to delete
            if ((dxBinders == null) || (dxBinders.Count == 0)) return true;

            //	We're going to trick the system into thinking the application
            //	is requesting removal of these scenes
            tmaxParents = new CTmaxItems();

            foreach (CDxBinderEntry O in dxBinders)
            {
                //	Get the actual application record interface to the parent binder
                if ((dxParent = GetBinderFromPath(O.ParentPathId)) == null) continue;

                //	Now locate this binder in the child collection
                if (dxParent.Contents.Count == 0)
                    dxParent.Fill();

                if ((dxBinder = dxParent.Contents.Find(O.AutoId)) == null) continue;

                //	Do we need to locate the item used to identify the parent?
                if ((tmaxParent == null) || (ReferenceEquals(tmaxParent.IBinderEntry, dxParent) == false))
                {
                    //	Is it already in the collection
                    if ((tmaxParent = tmaxParents.FindBinderEntry(dxParent)) == null)
                    {
                        //	Add a new parent item
                        tmaxParent = new CTmaxItem();
                        tmaxParent.DataType = TmaxDataTypes.Binder;
                        tmaxParent.IBinderEntry = dxParent;
                        tmaxParents.Add(tmaxParent);
                    }

                }

                //	Add to the parent's subitem collection
                if (tmaxParent != null)
                {
                    tmaxBinder = new CTmaxItem();
                    tmaxBinder.DataType = TmaxDataTypes.Binder;
                    tmaxBinder.IBinderEntry = dxBinder;
                    tmaxParent.SubItems.Add(tmaxBinder);
                }

            }// foreach(CDxBinderEntry O in dxBinders)

            //	Now preform the actual removal
            foreach (CTmaxItem O in tmaxParents)
            {
                DeleteBinders(O);
            }

            //	Clear the collection
            dxBinders.Clear();

            return true;

        }// private bool Delete(CDxBinderEntries dxBinders)

        /// <summary>This method is called to delete the binder entries identified by the items in tmaxChildren</summary>
        /// <param name="tmaxBinder">The event item that represents the parent binder</param>
        /// <returns>true if items have been deleted</returns>
        private bool DeleteBinders(CTmaxItem tmaxBinder)
        {
            CDxBinderEntry dxChild = null;
            CDxBinderEntry dxParent = null;
            CTmaxItem tmaxParent = null;
            int i = 0;

            Debug.Assert(tmaxBinder != null);
            Debug.Assert(tmaxBinder.DataType == TmaxDataTypes.Binder);
            Debug.Assert(tmaxBinder.SubItems != null);

            //	Is there anything to delete?
            if (tmaxBinder.SubItems.Count == 0) return false;

            try
            {
                //	Make a copy of the parent so that we can maintain
                //	the heirarchy in the trash can
                tmaxParent = new CTmaxItem();
                tmaxParent.IBinderEntry = tmaxBinder.IBinderEntry;
                tmaxParent.DataType = TmaxDataTypes.Binder;

                //	Delete each of the requested children
                foreach (CTmaxItem O in tmaxBinder.SubItems)
                {
                    if ((dxChild = (CDxBinderEntry)(O.IBinderEntry)) != null)
                    {
                        DeleteBinder(dxChild, tmaxParent.SubItems);
                    }
                }


                //	Update the parent
                if ((dxParent = (CDxBinderEntry)tmaxBinder.IBinderEntry) != null)
                {

                    foreach (CDxBinderEntry x in dxParent.Contents)
                    {
                        long it = x.DisplayOrder;
                    }
                    dxParent.Contents.Comparer = new DisplayOrderComparer();
                    dxParent.Contents.Sort();

                    for (i = 0; i < dxParent.Contents.Count; i++)
                    {
                        CDxBinderEntry binder = dxParent.Contents[i];
                        dxParent.Contents[i].DisplayOrder = i + 1;
                        dxParent.Contents.Update(dxParent.Contents[i]);
                    }

                    //	Update the parent record
                    dxParent.ChildCount = dxParent.Contents.Count;
                    if (dxParent.Collection != null)
                    {
                        dxParent.Collection.Update(dxParent);
                    }
                }
                else
                {
                    //	Update the display order for the root binders
                    for (i = 0; i < m_dxRootBinder.Contents.Count; i++)
                    {
                        m_dxRootBinder.Contents[i].DisplayOrder = i + 1;
                        m_dxRootBinder.Contents.Update(m_dxRootBinder.Contents[i]);
                    }

                    m_dxRootBinder.ChildCount = m_dxRootBinder.Contents.Count;
                }


            }
            catch
            {
            }

            //	Did we delete anything?
            if (tmaxParent.SubItems.Count > 0)
            {
                m_tmaxTrashCan.Add(tmaxParent);
                return true;
            }
            else
            {
                return false;
            }

        }// private bool DeleteBinders(CTmaxItem tmaxBinder)

        /// <summary>This method will delete the specified binder entry record</summary>
        /// <param name="dxBinder">The binder record to be deleted</param>
        /// <param name="tmaxDeleted">Collection to populate with items that represent records that have been deleted</param>
        /// <returns>true if records have been deleted</returns>
        private bool DeleteBinder(CDxBinderEntry dxBinder, CTmaxItems tmaxDeleted)
        {
            CTmaxItem tmaxBinder = null;

            Debug.Assert(dxBinder != null);
            Debug.Assert(tmaxDeleted != null);

            //	Create an item to represent this binder
            tmaxBinder = new CTmaxItem();
            tmaxBinder.IBinderEntry = dxBinder;
            tmaxBinder.DataType = TmaxDataTypes.Binder;

            //	Get all the child records
            if ((dxBinder.IsMedia() == false) && (dxBinder.Contents.Count == 0))
            {
                dxBinder.Fill();
            }

            //	Delete each child
            //
            //	NOTE:	We can't use a standard foreach() loop here because the call
            //			do delete binder is going to remove the record object from the
            //			list. That will raise an exception when done from within an iterator	
            while (dxBinder.Contents.Count > 0)
            {
                //	Perform a recursive delete so that we drill down and remove all children
                if (DeleteBinder(dxBinder.Contents[0], tmaxBinder.SubItems) == false)
                {
                    //	Make sure the record gets removed from the collection 
                    //	otherwise we get caught in a loop
                    dxBinder.Contents.RemoveList(dxBinder.Contents[0]);
                }

            }// while(dxBinder.Contents.Count > 0)

            //	Now remove this record
            dxBinder.Collection.Delete(dxBinder);

            //	Add an item to the caller's collection
            tmaxDeleted.Add(tmaxBinder);

            //	Force a refresh of the target binder
            m_dxTargetBinder = null;

            return true;

        }// private bool DeleteBinder(CDxBinderEntry dxBinder, CTmaxItems tmaxDeleted)

        /// <summary>This method is called to delete the binder entries identified by the items in tmaxChildren</summary>
        /// <param name="tmaxItem">The event item that represents the parent pick item</param>
        /// <returns>true if items have been deleted</returns>
        private bool DeletePickItems(CTmaxItem tmaxItem)
        {
            CDxPickItem dxChild = null;
            CDxPickItem dxParent = null;
            CTmaxItem tmaxParent = null;

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.DataType == TmaxDataTypes.PickItem);
            Debug.Assert(tmaxItem.SubItems != null);

            //	Is there anything to delete?
            if (tmaxItem.SubItems.Count == 0) return false;

            try
            {
                //	Make a copy of the parent so that we can maintain
                //	the heirarchy in the trash can
                tmaxParent = new CTmaxItem();
                tmaxParent.PickItem = tmaxItem.PickItem;
                tmaxParent.DataType = TmaxDataTypes.PickItem;

                //	Delete each of the requested children
                foreach (CTmaxItem O in tmaxItem.SubItems)
                {
                    if (O.PickItem != null)
                    {
                        if ((dxChild = ((CDxPickItem)(O.PickItem.DxRecord))) != null)
                        {
                            DeletePickItem(dxChild, tmaxParent.SubItems, true);
                        }
                    }

                }// foreach(CTmaxItem O in tmaxItem.SubItems)

                //	Get the exchange interface for the parent record
                if (tmaxItem.PickItem != null)
                    dxParent = (CDxPickItem)(tmaxItem.PickItem.DxRecord);

                //	Update the parent if all its children have been deleted
                if ((dxParent != null) && (dxParent.Children != null))
                {
                    if ((dxParent.Children.Count == 0) && (dxParent.UniqueId > 0))
                    {
                        //	Change the type of pick item
                        if (dxParent.Collection != null)
                        {
                            dxParent.Type = TmaxPickItemTypes.Unknown;
                            dxParent.Collection.Update(dxParent);
                        }

                    }// if((dxParent.Children.Count == 0) && (dxParent.UniqueId > 0))

                }// if((dxParent != null) && (dxParent.Children != null))

            }
            catch (System.Exception Ex)
            {
                FireError(this, "DeletePickItems", this.ExBuilder.Message(ERROR_CASE_DATABASE_DELETE_PICK_ITEMS_EX), Ex);
            }

            //	Did we delete anything?
            if (tmaxParent.SubItems.Count > 0)
            {
                m_tmaxTrashCan.Add(tmaxParent);
                return true;
            }
            else
            {
                return false;
            }

        }// private bool DeletePickItems(CTmaxItem tmaxItem)

        /// <summary>This method is called to delete the binder entries identified by the items in tmaxChildren</summary>
        /// <param name="tmaxItem">The event item that represents the parent pick item</param>
        /// <param name="bCheckCodes">True to check for data codes (fielded data) that reference the case codes being deleted</param>
        /// <param name="bConfirm">True to prompt for confirmation</param>
        /// <returns>true if items have been deleted</returns>
        private bool DeleteCaseCodes(CTmaxItem tmaxItem, bool bReferences, bool bConfirm)
        {
            CTmaxItem tmaxParent = null;

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.DataType == TmaxDataTypes.CaseCode);
            Debug.Assert(tmaxItem.SubItems != null);

            //	Is there anything to delete?
            if (tmaxItem.SubItems.Count == 0) return false;

            try
            {
                //	Make a copy of the parent so that we can maintain
                //	the heirarchy in the trash can
                tmaxParent = new CTmaxItem();
                tmaxParent.CaseCode = tmaxItem.CaseCode;
                tmaxParent.DataType = TmaxDataTypes.CaseCode;

                //	Delete each of the requested children
                foreach (CTmaxItem O in tmaxItem.SubItems)
                {
                    if (O.CaseCode != null)
                    {
                        DeleteCaseCode(O.CaseCode, tmaxParent.SubItems, bReferences, bConfirm);
                    }

                }// foreach(CTmaxItem O in tmaxItem.SubItems)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "DeleteCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_DELETE_CASE_CODES_EX), Ex);
            }

            //	Did we delete anything?
            if (tmaxParent.SubItems.Count > 0)
            {
                m_tmaxTrashCan.Add(tmaxParent);
                return true;
            }
            else
            {
                return false;
            }

        }// private bool DeleteCaseCodes(CTmaxItem tmaxItem)

        /// <summary>This method will delete the specified binder entry record</summary>
        /// <param name="dxPickItem">The pick item record to be deleted</param>
        /// <param name="tmaxDeleted">Collection to populate with items that represent records that have been deleted</param>
        /// <param name="bConfirm">True to confirm before removal if references are found</param>
        /// <returns>true if records have been deleted</returns>
        private bool DeletePickItem(CDxPickItem dxPickItem, CTmaxItems tmaxDeleted, bool bConfirm)
        {
            CTmaxItem tmaxPickItem = null;
            CTmaxItem tmaxParent = null;
            CDxCodes dxCodes = new CDxCodes(this);
            CTmaxCaseCodes tmaxCaseCodes = null;
            CFConfirmCodesDelete confirmDelete = null;

            Debug.Assert(dxPickItem != null);
            Debug.Assert(dxPickItem.TmaxPickItem != null);
            Debug.Assert(dxPickItem.Parent != null);
            Debug.Assert(tmaxDeleted != null);

            //	Get all case codes that reference this item
            if (this.CaseCodes != null)
                tmaxCaseCodes = this.CaseCodes.GetReferences(dxPickItem.TmaxPickItem);
            if ((tmaxCaseCodes != null) && (tmaxCaseCodes.Count == 0))
                tmaxCaseCodes = null;

            //	Get all the codes that reference this pick list item and prompt the user for confirmation
            dxCodes.Fill(dxPickItem.TmaxPickItem);
            if (dxCodes.Count == 0) dxCodes = null;

            if (bConfirm == true)
            {
                if ((dxCodes != null) || (tmaxCaseCodes != null))
                {
                    confirmDelete = new CFConfirmCodesDelete();
                    confirmDelete.DeleteName = dxPickItem.Name;
                    confirmDelete.CodesDisplayMode = CDxCode.TMAX_LISTVIEW_DISPLAY_MODE_CONFIRM;
                    confirmDelete.CaseCodesDisplayMode = 0;
                    confirmDelete.ICodes = dxCodes;
                    confirmDelete.ICaseCodes = tmaxCaseCodes;

                    switch (confirmDelete.ShowDialog())
                    {
                        case DialogResult.Cancel:
                        case DialogResult.No:

                            return false;

                        case DialogResult.Yes:
                        default:

                            break;

                    }// switch(confirmDelete.ShowDialog())

                }// if((dxCodes != null) || (tmaxCaseCodes != null))

            }// if(bConfirm == true)

            //	Create an item to represent this pick list item
            tmaxPickItem = new CTmaxItem(dxPickItem.TmaxPickItem);

            //	Does this item have children?
            if (dxPickItem.Children != null)
            {
                //	Do we need to fill the collection
                if (dxPickItem.Children.Count == 0)
                    dxPickItem.Fill(false);

                //	Delete each child
                //
                //	NOTE:	We can't use a standard foreach() loop here because the call
                //			do delete the item is going to remove the record object from the
                //			list. That will raise an exception when done from within an iterator	
                while (dxPickItem.Children.Count > 0)
                {
                    //	Perform a recursive delete so that we drill down and remove all children
                    if (DeletePickItem(dxPickItem.Children[0], tmaxPickItem.SubItems, false) == false)
                    {
                        //						//	Make sure the record gets removed from the collection 
                        //						//	otherwise we get caught in a loop
                        //						dxPickItem.TmaxPickItem.Children.Remove(dxPickItem.Children[0].TmaxPickItem);
                        //						dxPickItem.Children.RemoveList(dxPickItem.Children[0]);
                    }

                }// while(dxPickItem.Children.Count > 0)

            }// if(dxPickItem.Children != null)

            //	Now remove this record
            //
            //	NOTE:	We have to use the parent to delete the record to keep the collections in sync
            dxPickItem.Parent.Delete(dxPickItem);

            //	Add an item to the caller's collection
            tmaxDeleted.Add(tmaxPickItem);

            //	Do we have any case codes to delete?
            if ((tmaxCaseCodes != null) && (tmaxCaseCodes.Count > 0))
            {
                tmaxParent = new CTmaxItem();
                tmaxParent.DataType = TmaxDataTypes.CaseCode;
                foreach (CTmaxCaseCode O in tmaxCaseCodes)
                    tmaxParent.SubItems.Add(new CTmaxItem(O));

                this.DeleteCaseCodes(tmaxParent, false, false);
            }

            //	Do we have any fielded data to delete?
            if ((dxCodes != null) && (dxCodes.Count == 0))
            {
                try { dxCodes.Delete(dxCodes); }
                catch { }
            }

            return true;

        }// private bool DeletePickItem(CDxPickItem dxPickItem, CTmaxItems tmaxDeleted)

        /// <summary>This method will delete the specified case code</summary>
        /// <param name="tmaxCaseCode">The application case code to be deleted</param>
        /// <param name="tmaxDeleted">Collection to populate with items that represent records that have been deleted</param>
        /// <param name="bCheckCodes">True to check for data codes (fielded data) that reference the case codes being deleted</param>
        /// <param name="bConfirm">True to prompt for confirmation</param>
        /// <returns>true if records have been deleted</returns>
        private bool DeleteCaseCode(CTmaxCaseCode tmaxDelete, CTmaxItems tmaxDeleted, bool bReferences, bool bConfirm)
        {
            CTmaxItem tmaxCaseCode = null;
            CDxCodes dxCodes = null;
            CFConfirmCodesDelete confirmDelete = null;

            Debug.Assert(tmaxDelete != null);
            Debug.Assert(tmaxDeleted != null);

            //	Should we check for references?
            if (bReferences == true)
            {
                dxCodes = new CDxCodes(this);

                //	Get all the codes that reference this pick list item and prompt the user for confirmation
                dxCodes.Fill(tmaxDelete);

                if ((dxCodes.Count > 0) && (bConfirm == true))
                {
                    confirmDelete = new CFConfirmCodesDelete();
                    confirmDelete.DeleteName = tmaxDelete.Name;
                    confirmDelete.CodesDisplayMode = CDxCode.TMAX_LISTVIEW_DISPLAY_MODE_CONFIRM;
                    confirmDelete.ICodes = dxCodes;

                    switch (confirmDelete.ShowDialog())
                    {
                        case DialogResult.Cancel:
                        case DialogResult.No:

                            return false;

                        case DialogResult.Yes:
                        default:

                            break;

                    }// switch(confirmDelete.ShowDialog())

                }// if((dxCodes.Count > 0) && (bConfirm == true))

            }// if(bReferences == true)

            //	Create an item to represent this case code
            tmaxCaseCode = new CTmaxItem(tmaxDelete);

            //	Are we using the database for case codes?
            if (this.DxCaseCodes != null)
            {
                if (tmaxDelete.DxRecord != null)
                    this.DxCaseCodes.Delete((CDxCaseCode)(tmaxDelete.DxRecord));
            }

            //	Remove from the application collection
            this.CaseCodes.Remove(tmaxDelete);

            //	Mark the collection if not using the database
            if (this.DxCaseCodes == null)
                m_tmaxCodesManager.Modified = true;

            //	Add an item to the caller's collection
            tmaxDeleted.Add(tmaxCaseCode);

            //	Do we have any fielded data to delete?
            if ((dxCodes != null) && (dxCodes.Count > 0))
            {
                try { dxCodes.Delete(dxCodes); }
                catch { }
            }

            return true;

        }// private bool DeleteCaseCode(CDxCaseCode dxCaseCode, CTmaxItems tmaxDeleted)

        /// <summary>This method will delete the file associated with the specified record</summary>
        /// <param name="dxRecord">The record who's file is to be deleted</param>
        /// <returns>true if successful</returns>
        private bool DeleteFile(CDxMediaRecord dxRecord)
        {
            CDxPrimary dxPrimary = null;
            string strFileSpec = "";

            //	Make sure this record type has an associated file
            switch (dxRecord.MediaType)
            {
                case TmaxMediaTypes.Page:
                case TmaxMediaTypes.Segment:

                    //	Check to see if this is a linked file
                    if ((dxPrimary = ((CDxSecondary)dxRecord).Primary) != null)
                    {
                        if (dxPrimary.Linked == false)
                        {
                            strFileSpec = GetFileSpec(dxRecord);
                        }
                    }
                    break;

                case TmaxMediaTypes.Treatment:
                case TmaxMediaTypes.Clip:
                case TmaxMediaTypes.Designation:

                    strFileSpec = GetFileSpec(dxRecord);
                    break;

                case TmaxMediaTypes.Document:
                case TmaxMediaTypes.Powerpoint:
                case TmaxMediaTypes.Slide:
                case TmaxMediaTypes.Script:
                case TmaxMediaTypes.Scene:
                case TmaxMediaTypes.Link:
                case TmaxMediaTypes.Unknown:

                    //	No file for these types
                    return true;

                default:

                    Debug.Assert(false);
                    return false;

            }

            if (strFileSpec.Length > 0)
            {
                try
                {
                    System.IO.File.Delete(strFileSpec);
                }
                catch
                {
                    return false;
                }
            }

            return true;

        }// private bool DeleteFile(CDxMediaRecord dxRecord)

        /// <summary>This method will get the files and/or folders that should be deleted when the specified record gets deleted</summary>
        /// <param name="dxRecord">The record that is being deleted</param>
        /// <param name="tmaxScratched">The folder in which to store the files/folders to be deleted</param>
        /// <returns>True if any files/folders were marked for removal</returns>
        private bool GetScratchFiles(CDxMediaRecord dxRecord, CTmaxSourceFolder tmaxScratched)
        {
            string strPath = "";

            Debug.Assert(dxRecord != null);
            Debug.Assert(tmaxScratched != null);

            //	What type of media are we dealing with?
            switch (dxRecord.MediaType)
            {
                case TmaxMediaTypes.Document:
                case TmaxMediaTypes.Deposition:
                case TmaxMediaTypes.Recording:

                    //	Are we using the updated file structure for treatments?
                    if (m_bRequires601Update == false)
                    {
                        if (dxRecord.MediaType == TmaxMediaTypes.Document)
                        {
                            //	Delete the folder containing the treatments
                            strPath = GetCasePath(TmaxMediaTypes.Treatment);
                            if (strPath.Length > 0)
                                strPath += dxRecord.AutoId.ToString();
                        }
                        else if (dxRecord.MediaType == TmaxMediaTypes.Deposition)
                        {
                            //	Delete the folder containing the transcript and designations
                            if ((((CDxPrimary)dxRecord).GetTranscript()) != null)
                                strPath = GetTranscriptFolder(((CDxPrimary)dxRecord).GetTranscript());
                        }
                        else
                        {
                            //	Delete the folder containing the clips
                            strPath = GetCasePath(TmaxMediaTypes.Clip);
                            if (strPath.Length > 0)
                                strPath += dxRecord.AutoId.ToString();
                        }

                        //	Scratch the folder
                        if (strPath.Length > 0)
                            tmaxScratched.SubFolders.Add(new CTmaxSourceFolder(strPath));
                    }
                    else
                    {
                        //	Since the old file system did not organize treatments,
                        //	designations, and clips under their primary owner, the
                        //	only way we have to scratch the files is to drill the
                        //	tertiary collections
                        //
                        //	NOTE:	Performance-wise this isn't too bad a hit because
                        //			the code to delete the record has to populate the
                        //			secondary and tertiary collections anyway
                        if (((CDxPrimary)dxRecord).Secondaries.Count == 0)
                            ((CDxPrimary)dxRecord).Secondaries.Fill();

                        foreach (CDxSecondary S in ((CDxPrimary)dxRecord).Secondaries)
                        {
                            if (S.Tertiaries.Count == 0)
                                S.Tertiaries.Fill();

                            foreach (CDxTertiary T in S.Tertiaries)
                            {
                                strPath = GetFileSpec(T);
                                if (strPath.Length > 0)
                                    tmaxScratched.Files.Add(new CTmaxSourceFile(strPath));

                            }// foreach(CDxTertiary T in S.Tertiaries)					

                        }// foreach(CDxSecondary S in ((CDxPrimary)dxRecord).Secondaries)

                    }// if(m_bRequires601Update == false)

                    break;

                case TmaxMediaTypes.Powerpoint:

                    //	Get the path to the folder for the exported slides
                    strPath = GetSlidesFolderSpec((CDxPrimary)dxRecord);
                    if (strPath.Length > 0)
                        tmaxScratched.SubFolders.Add(new CTmaxSourceFolder(strPath));
                    break;

                case TmaxMediaTypes.Page:

                    //	Do we have any treatments for this page?
                    if (((CDxSecondary)dxRecord).Tertiaries.Count == 0)
                        ((CDxSecondary)dxRecord).Tertiaries.Fill();
                    if (((CDxSecondary)dxRecord).Tertiaries.Count == 0)
                        break;

                    //	Are we using the updated file structure for treatments?
                    if (m_bRequires601Update == false)
                    {
                        //	Delete the folder containing the treatments
                        strPath = GetFolderSpec(((CDxSecondary)dxRecord).Tertiaries[0]);
                        if (strPath.Length > 0)
                            tmaxScratched.SubFolders.Add(new CTmaxSourceFolder(strPath));
                    }
                    else
                    {
                        //	Since the old file system put all treatments in one folder
                        //	we have to add the file for each treatment
                        foreach (CDxTertiary T in ((CDxSecondary)dxRecord).Tertiaries)
                        {
                            strPath = GetFileSpec(T);
                            if (strPath.Length > 0)
                                tmaxScratched.Files.Add(new CTmaxSourceFile(strPath));

                        }// foreach(CDxTertiary T in S.Tertiaries)					

                    }// if(m_bRequires601Update == false)

                    break;

                case TmaxMediaTypes.Segment:

                    //	Do we have any designations/clips for this segment?
                    if (((CDxSecondary)dxRecord).Tertiaries.Count == 0)
                        ((CDxSecondary)dxRecord).Tertiaries.Fill();
                    if (((CDxSecondary)dxRecord).Tertiaries.Count == 0)
                        break;

                    //	Are we using the updated file structure for recording segments?
                    if ((m_bRequires601Update == false) && (((CDxSecondary)dxRecord).Primary.MediaType == TmaxMediaTypes.Recording))
                    {
                        //	Delete the folder containing the clips
                        strPath = GetFolderSpec(((CDxSecondary)dxRecord).Tertiaries[0]);
                        if (strPath.Length > 0)
                            tmaxScratched.SubFolders.Add(new CTmaxSourceFolder(strPath));
                    }
                    else
                    {
                        //	Add the files for the tertiary records
                        foreach (CDxTertiary T in ((CDxSecondary)dxRecord).Tertiaries)
                        {
                            strPath = GetFileSpec(T);
                            if (strPath.Length > 0)
                                tmaxScratched.Files.Add(new CTmaxSourceFile(strPath));

                        }// foreach(CDxTertiary T in S.Tertiaries)					

                    }// if(m_bRequires601Update == false)

                    break;

                case TmaxMediaTypes.Treatment:
                case TmaxMediaTypes.Designation:
                case TmaxMediaTypes.Clip:

                    strPath = GetFileSpec((CDxTertiary)dxRecord);
                    if (strPath.Length > 0)
                        tmaxScratched.Files.Add(new CTmaxSourceFile(strPath));
                    break;

                case TmaxMediaTypes.Slide:
                case TmaxMediaTypes.Script:
                case TmaxMediaTypes.Scene:
                case TmaxMediaTypes.Link:
                default:

                    break;

            }// switch(dxRecord.GetMediaLevel())

            return ((tmaxScratched.SubFolders.Count > 0) || (tmaxScratched.Files.Count > 0));

        }// private bool GetScratchFiles(CDxMediaRecord dxRecord, CTmaxSourceFolder tmaxScratched)

        /// <summary>This method will delete the file/folders identified in the scratched folder</summary>
        /// <param name="tmaxScratched">The folder that identifies the files/folders to be deleted</param>
        /// <returns>True if no errors occur</returns>
        private bool DeleteScratchedFiles(CTmaxSourceFolder tmaxScratched)
        {
            bool bNoErrors = true;

            if (tmaxScratched == null) return true;

            //	Delete the folders first
            foreach (CTmaxSourceFolder O in tmaxScratched.SubFolders)
            {
                if (System.IO.Directory.Exists(O.Path) == true)
                {
                    try { System.IO.Directory.Delete(O.Path, true); }
                    catch { bNoErrors = false; }
                }

            }

            //	Now delete the files
            foreach (CTmaxSourceFile O in tmaxScratched.Files)
            {
                if (System.IO.File.Exists(O.Path) == true)
                {
                    try { System.IO.File.Delete(O.Path); }
                    catch { bNoErrors = false; }
                }

            }

            return bNoErrors;

        }// private void DeleteScratchedFiles(CTmaxSourceFolder tmaxScratched)

        /// <summary>This method will add or insert new media records using the specified event item</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxAdded">Collection to populate with items that represent the new records</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        private bool AddMedia(CTmaxItem tmaxItem, CTmaxItems tmaxAdded, CTmaxParameters tmaxParameters)
        {
            CTmaxParameter tmaxParameter = null;
            ITmaxMediaRecord IRecord = null;
            CTmaxItem tmaxReturn = null;
            CTmaxItem tmaxRecord = null;
            bool bBefore = false;

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.DataType == TmaxDataTypes.Media);
            Debug.Assert(tmaxItem.MediaType != TmaxMediaTypes.Unknown);
            Debug.Assert(tmaxAdded != null);

            if (tmaxParameters != null)
            {
                //	Get the parameters passed by the caller
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Before)) != null)
                    bBefore = tmaxParameter.AsBoolean();
            }

            //	Are we meant to be adding a new primary record?
            if (tmaxItem.IPrimary == null)
            {
                //	Do we have room for a new primary record?
                if (CheckMaxPrimaries(false) == true)
                    return false;

                if ((IRecord = AddPrimary(tmaxItem.MediaType)) != null)
                {
                    //	Add an item to identify the new primary
                    tmaxAdded.Add(new CTmaxItem(IRecord));
                }

            }

            //	Are we adding new secondary records?
            else if (tmaxItem.ISecondary == null)
            {
                //	Do we need to get the secondary records?
                //
                //	NOTE:	We need the secondaries so that the new records
                //			get assigned the correct display order value
                if (((CDxPrimary)tmaxItem.IPrimary).Secondaries.Count == 0)
                {
                    ((CDxPrimary)tmaxItem.IPrimary).Fill();
                }

                switch (tmaxItem.IPrimary.GetMediaType())
                {
                    case TmaxMediaTypes.Document:
                    case TmaxMediaTypes.Recording:

                        //	Prompt the user for the files to be added
                        if ((tmaxItem.SourceFolder = GetSource((CDxPrimary)tmaxItem.IPrimary)) != null)
                        {
                            //	Add to the primary record
                            if (tmaxItem.SubItems.Count > 0 && (tmaxItem.SubItems[0].ISecondary != null))
                            {
                                InsertSource((CDxPrimary)tmaxItem.IPrimary, tmaxItem.SourceFolder, (CDxSecondary)(tmaxItem.SubItems[0].ISecondary), bBefore);
                            }
                            else
                            {
                                InsertSource((CDxPrimary)tmaxItem.IPrimary, tmaxItem.SourceFolder, null, false);
                            }

                            //	Add an item for each new file to the caller's collection
                            foreach (CTmaxSourceFile tmaxFile in tmaxItem.SourceFolder.Files)
                            {
                                if (tmaxFile.ISecondary != null)
                                {
                                    tmaxRecord = new CTmaxItem(tmaxFile.ISecondary);
                                    tmaxRecord.ParentItem = new CTmaxItem(((CDxSecondary)(tmaxFile.ISecondary)).Primary);

                                    tmaxAdded.Add(new CTmaxItem(tmaxRecord));
                                }

                            }// foreach(CTmaxSourceFile tmaxFile in tmaxItem.SourceFolder.Files)

                        }

                        break;

                    case TmaxMediaTypes.Powerpoint:

                        Debug.Assert(false);
                        break;

                    case TmaxMediaTypes.Script:

                        //	The caller should populate the source items collection
                        if ((tmaxItem.SourceItems != null) && (tmaxItem.SourceItems.Count > 0))
                        {
                            //	Add to the script
                            if (tmaxItem.SubItems.Count > 0 && (tmaxItem.SubItems[0].ISecondary != null))
                            {
                                InsertScenes((CDxPrimary)tmaxItem.IPrimary, tmaxItem.SourceItems, tmaxAdded, (CDxSecondary)(tmaxItem.SubItems[0].ISecondary), tmaxParameters);
                            }
                            else
                            {
                                InsertScenes((CDxPrimary)tmaxItem.IPrimary, tmaxItem.SourceItems, tmaxAdded, null, null);
                            }

                        }

                        break;

                    default:

                        break;
                }

            }// if(tmaxItem.ISecondary == null)

            //	Are we adding a new tertiary?
            else if (tmaxItem.ITertiary == null)
            {
                //	What type of tertiary object are we adding
                switch (tmaxItem.ISecondary.GetMediaType())
                {
                    case TmaxMediaTypes.Page:

                        Debug.Assert(tmaxItem.SourceFolder != null);
                        Debug.Assert(tmaxItem.SourceFolder.Files != null);
                        Debug.Assert(tmaxItem.SourceFolder.Files.Count == 1);

                        if ((IRecord = AddTreatment((CDxSecondary)tmaxItem.ISecondary, tmaxItem.SourceFolder.Files[0].Path)) != null)
                        {
                            tmaxRecord = new CTmaxItem(IRecord);
                            tmaxRecord.ParentItem = new CTmaxItem(tmaxItem.ISecondary);

                            tmaxAdded.Add(tmaxRecord);
                        }
                        break;

                    case TmaxMediaTypes.Segment:

                        Debug.Assert(tmaxItem.SourceItems != null);
                        Debug.Assert(tmaxItem.SourceItems.Count > 0);
                        Debug.Assert(tmaxItem.SourceItems[0].XmlDesignation != null);

                        foreach (CTmaxItem O in tmaxItem.SourceItems)
                        {
                            if (O.XmlDesignation != null)
                            {
                                IRecord = AddDesignation((CDxSecondary)tmaxItem.GetMediaRecord(),
                                                         O.XmlDesignation);
                                if (IRecord != null)
                                {
                                    O.SetRecord(IRecord);

                                    tmaxRecord = new CTmaxItem(IRecord);
                                    tmaxRecord.ParentItem = new CTmaxItem(IRecord.GetParent());

                                    tmaxAdded.Add(new CTmaxItem(tmaxRecord));
                                }

                            }

                        }
                        break;

                    case TmaxMediaTypes.Scene:

                        Debug.Assert(false);
                        break;

                    default:

                        //	Don't know how to do any other tertiary yet
                        break;

                }

            }// if(tmaxItem.ITertiary == null)

            //	Must be new quaternary
            else if (tmaxItem.IQuaternary == null)
            {
                Debug.Assert(tmaxItem.SourceItems != null);
                Debug.Assert(tmaxItem.SourceItems.Count > 0);
                Debug.Assert(tmaxItem.SourceItems[0].XmlLink != null);

                //	Add new links
                foreach (CTmaxItem O in tmaxItem.SourceItems)
                {
                    if (O.XmlLink != null)
                    {
                        IRecord = AddLink((CDxTertiary)tmaxItem.GetMediaRecord(),
                                           tmaxItem.XmlDesignation, O.XmlLink);

                        if (IRecord != null)
                        {
                            O.SetRecord(IRecord);

                            tmaxReturn = new CTmaxItem(IRecord);
                            tmaxReturn.XmlLink = O.XmlLink; // Return the XML
                            tmaxReturn.UserData1 = O.XmlLink;
                            tmaxReturn.ParentItem = new CTmaxItem(IRecord.GetParent());

                            tmaxAdded.Add(tmaxReturn);
                        }

                    }

                }
            }

            return (tmaxAdded.Count > 0);

        }// public bool AddMedia(CTmaxItem tmaxItem, CTmaxItems tmaxAdded, bool bBefore)

        /// <summary>This method will add a new primary record of the specified type to the database</summary>
        /// <param name="tmaxType">The primary media type</param>
        /// <returns>The record exchange object for the new primary record</returns>
        private CDxPrimary AddPrimary(TmaxMediaTypes tmaxType)
        {
            return AddPrimary(tmaxType, null, null, null);
        }

        /// <summary>This method will add binder entries using the specified items</summary>
        /// <param name="tmaxParent">The TrialMax event item</param>
        /// <param name="tmaxAdded">Collection to populate with items that represent the new records</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        private bool AddBinderEntries(CTmaxItem tmaxParent, CTmaxItems tmaxAdded, CTmaxParameters tmaxParameters)
        {

            CTmaxParameter tmaxParameter = null;
            CDxBinderEntry dxEntry = null;
            CDxBinderEntry dxParent = null;
            CDxBinderEntry dxInsert = null;
            bool bBefore = false;
            bool bNoDuplicates = false;

            Debug.Assert(tmaxParent != null);
            Debug.Assert(tmaxAdded != null);

            //	The parent MUST not be a media object
            Debug.Assert(tmaxParent.MediaType == TmaxMediaTypes.Unknown);
            if (tmaxParent.MediaType != TmaxMediaTypes.Unknown) return false;

            //	Make sure we have stuff to add
            Debug.Assert(tmaxParent.SourceItems != null);
            Debug.Assert(tmaxParent.SourceItems.Count > 0);
            if ((tmaxParent.SourceItems == null) || (tmaxParent.SourceItems.Count == 0)) return false;

            //	Did the caller provide a parameters collection?
            if (tmaxParameters != null)
            {
                //	Get the parameters passed by the caller
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Before)) != null)
                    bBefore = tmaxParameter.AsBoolean();
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.NoDuplicates)) != null)
                    bNoDuplicates = tmaxParameter.AsBoolean();
            }

            //	Get the parent binder
            if (tmaxParent.IBinderEntry != null)
                dxParent = (CDxBinderEntry)(tmaxParent.IBinderEntry);

            if (tmaxParent != null && tmaxParent.SourceItems[0].SourceFolder != null)
            {
                long parentId = 0;
                if (dxParent != null) // If parent is null, then the new binder added is not a child of anyone
                {
                    parentId = dxParent.AutoId; // If parent is not null, then the new binder is a child of an already existing binder
                }
                // Search in database if there already exists a binder with the same name and same parent
                GetDataReader("SELECT * FROM BinderEntries WHERE ParentId = " + parentId + " AND Name = \"" + System.IO.Path.GetFileNameWithoutExtension(tmaxParent.SourceItems[0].SourceFolder.Name) + "\"");

                if (m_oleDbReader.HasRows) // A binder already exists if this condition is true
                {
                    short appendedNumber = 1;
                    while (true) // Append integer to the binder name and check if it exists already. Keep trying until we succeed
                    {
                        GetDataReader("SELECT * FROM BinderEntries WHERE ParentId = " + parentId + " AND Name = \"" + tmaxParent.SourceItems[0].SourceFolder.Name + "-" + appendedNumber.ToString("00") + "\"");
                        if (!m_oleDbReader.HasRows)
                        {
                            tmaxParent.SourceItems[0].SourceFolder.Name += "-" + appendedNumber.ToString("00");
                            break;
                        }
                        appendedNumber++;
                    }
                }
            }

            //	Check for duplicate entries
            if (ConfirmBinderDuplicates(dxParent, tmaxParent.SourceItems, bNoDuplicates) == false)
                return false;

            //	Get the insertion point if defined
            if ((tmaxParent.SubItems != null) && (tmaxParent.SubItems.Count > 0))
            {
                if (tmaxParent.SubItems[0].IBinderEntry != null)
                    dxInsert = (CDxBinderEntry)(tmaxParent.SubItems[0].IBinderEntry);
            }

            //	Add the first entry
            if ((dxEntry = AddBinderEntry(dxParent, tmaxParent.SourceItems[0], dxInsert, bBefore, tmaxAdded)) == null)
                return false;

            //	Make the new entry the insertion point for the next addition
            dxInsert = dxEntry;

            //	Now add all subsequent entries
            for (int i = 1; i < tmaxParent.SourceItems.Count; i++)
            {
                //	Add new entry for each item and insert after previous
                if ((dxEntry = AddBinderEntry(dxParent, tmaxParent.SourceItems[i], dxInsert, false, tmaxAdded)) != null)
                {
                    dxInsert = dxEntry;
                }
            }

            return true;

        }// private bool AddBinderEntries(CTmaxItem tmaxParent, CTmaxItems tmaxAdded, bool bBefore)

        /// <summary>This method will insert a new binder entry</summary>
        /// <param name="dxEntry">The binder entry to be added to the database</param>
        /// <param name="dxInsertAt">The binder entry where the new entry is to be inserted</param>
        ///	<param name="bBefore">true if to be inserted before the InsertAt entry</param>
        /// <returns>true if successful</returns>
        private bool InsertBinder(CDxBinderEntry dxEntry, CDxBinderEntry dxInsertAt, bool bBefore)
        {
            CDxBinderEntries dxEntries = null;
            CDxBinderEntries dxHolding = null;
            CDxBinderEntry dxHold = null;
            int iInsertAt = -1;

            Debug.Assert(dxEntry != null);

            //	Get the parent collection
            if (dxEntry.Parent != null)
            {
                if (dxEntry.Parent.Contents.Count == 0)
                    dxEntry.Parent.Fill();

                dxEntries = dxEntry.Parent.Contents;
                dxEntries.Comparer = new DisplayOrderComparer();
                dxEntries.Sort();

            }
            else
            {
                dxEntries = m_dxRootBinder.Contents;
            }

            Debug.Assert(dxEntries != null);
            if (dxEntries == null) return false;

            //	Did the user provide an insertion point?
            if (dxInsertAt != null)
            {
                iInsertAt = dxEntries.IndexOf(dxInsertAt);
                Debug.Assert(iInsertAt >= 0);

                //	Adjust the index if we are inserting after
                if (bBefore == false)
                {
                    iInsertAt++;

                    //	Should we just add to the end?
                    if (iInsertAt >= dxEntries.Count)
                        iInsertAt = -1; //	Add to end
                }

            }// if(dxInsertAt != null)

            //	Add to the end of the collection
            dxEntry.DisplayOrder = dxEntries.GetNextDisplayOrder();


            if (dxEntries.Add(dxEntry) == null)
            {
                return false;
            }

            //	Update the parent's child count
            if ((dxEntry.Parent != null) && (dxEntry.Parent.Collection != null))
            {
                dxEntry.Parent.ChildCount = dxEntries.Count;
                dxEntry.Parent.Collection.Update(dxEntry.Parent);
            }

            //	Do we need to reorder?
            if (iInsertAt >= 0)
            {
                //	Remove the new entry from the end of the list
                dxEntries.RemoveList(dxEntry);

                //	Now store all records beyond the insertion point in
                //	a temporary holding collection
                dxHolding = new CDxBinderEntries();
                while (iInsertAt < dxEntries.Count)
                {
                    dxHold = dxEntries[iInsertAt];
                    dxHolding.AddList(dxHold);
                    dxEntries.RemoveList(dxHold);
                }

                //	Put the new entry back in
                dxEntry.DisplayOrder = dxEntries.GetNextDisplayOrder();
                dxEntries.AddList(dxEntry);
                dxEntries.Update(dxEntry);

                //	Now put all the holding records back in
                foreach (CDxBinderEntry O in dxHolding)
                {
                    O.DisplayOrder = dxEntries.GetNextDisplayOrder();
                    dxEntries.AddList(O);
                    dxEntries.Update(O);
                }

                dxHolding.Clear();
            }

            return true;

        }// private bool InsertBinder(CDxBinderEntry dxEntry, CDxBinder dxInsertAt, bool bBefore)

        /// <summary>This method will move the specified binder entries to a new parent</summary>
        /// <param name="tmaxMove">The event item that defines the new parent and the entries to be moved</param>
        /// <param name="bBefore">true to insert before the first item in the tmaxMove SubItem collection</param>
        /// <returns>true if successful</returns>
        private bool MoveBinderEntries(CTmaxItem tmaxMove, bool bBefore)
        {
            CDxBinderEntry dxParent = null;
            CDxBinderEntry dxEntry = null;
            CDxBinderEntry dxInsert = null;
            CDxBinderEntries dxEntries = null;
            CDxBinderEntries dxHolding = null;
            int iIndex = 0;
            bool bSuccessful = false;
            bool bKeepSorted = false;

            Debug.Assert(tmaxMove != null, "CTmaxCaseDatabase::MoveBinderEntries - invalid parameter tmaxMove");
            if (tmaxMove == null) return false;

            //	Do we have anything to be moved?
            if (tmaxMove.SourceItems == null) return false;
            if (tmaxMove.SourceItems.Count == 0) return false;

            try
            {
                //	Get the parent to which these entries are being moved
                if ((dxParent = (CDxBinderEntry)(tmaxMove.IBinderEntry)) != null)
                {
                    //	Make sure the child collection has been filled
                    if ((dxParent.Contents == null) || (dxParent.Contents.Count == 0))
                        if (dxParent.Fill() == false) return false;

                    //	We will use this collection for the new location
                    dxEntries = dxParent.Contents;
                    dxEntries.Comparer = new DisplayOrderComparer();
                    dxEntries.Sort();
                    //	Confirm duplicates before making the move
                    if (ConfirmBinderDuplicates(dxParent, tmaxMove.SourceItems, false) == false)
                        return false;
                }
                else
                {
                    //	Use the root binder collection
                    Debug.Assert(m_dxRootBinder != null, "MoveBinderEntries: no root binder available");
                    Debug.Assert(m_dxRootBinder.Contents != null, "MoveBinderEntries: no root binder contents available");
                    if ((m_dxRootBinder != null) && (m_dxRootBinder.Contents != null))
                        dxEntries = m_dxRootBinder.Contents;
                    else
                        return false;
                }

                //	Make sure the records are sorted and then turn off
                //	sorting while we move things around
                dxEntries.Sort(true);
                bKeepSorted = dxEntries.KeepSorted;
                dxEntries.KeepSorted = false;

                Debug.Assert(dxEntries != null, "MoveBinderEntries: no target collection available");
                if (dxEntries == null) return false;

                //	Did the caller specify an insertion point?
                if (tmaxMove.SubItems.Count > 0)
                {
                    if ((dxInsert = ((CDxBinderEntry)(tmaxMove.SubItems[0].IBinderEntry))) != null)
                    {
                        //	Get the index of the insertion point
                        if ((iIndex = dxEntries.IndexOf(dxInsert)) >= 0)
                        {
                            //	Create a temporary collection to hold the entries that follow the insertion point
                            dxHolding = new CDxBinderEntries();

                            //	Adjust the index if inserting after
                            if (bBefore == false) iIndex++;

                            //	Transfer all entries starting at the insertion point
                            while (iIndex < dxEntries.Count)
                            {
                                dxHolding.AddList(dxEntries[iIndex]);
                                dxEntries.Remove(dxEntries[iIndex]);
                            }

                        }// if((iIndex = dxEntries.IndexOf(dxInsert)) >= 0)	

                    }// if((dxInsert = ((CDxBinderEntry)(tmaxMove.SubItems[0].IBinderEntry))) != null)

                    //	Clear the subitems collection to make room for the moved entries
                    tmaxMove.SubItems.Clear();

                }// if(tmaxMove.SubItems.Count > 0)

                //	SourceItems contains the binders to be moved
                foreach (CTmaxItem O in tmaxMove.SourceItems)
                {
                    //	Should only be binder entries in this collection
                    if ((dxEntry = (CDxBinderEntry)(O.IBinderEntry)) == null)
                        continue;

                    //	Store the reference to the current parent so we can use it later
                    O.UserData1 = (dxEntry.Parent != null) ? dxEntry.Parent : m_dxRootBinder;

                    //	Remove this entry from it's existing parent
                    ((CDxBinderEntry)(O.UserData1)).Contents.RemoveList(dxEntry);

                    //	Add to the new parent's collection and update the Parent information
                    dxEntries.AddList(dxEntry);
                    dxEntry.Collection = dxEntries;
                    dxEntry.SetParent(dxEntries.Parent);
                    dxEntry.DisplayOrder = -1; // This forces an update of the record

                    //	Transfer this item to the SubItems collection after its moved
                    tmaxMove.SubItems.Add(O);

                }// foreach(CTmaxItem O in tmaxMove.SourceItems)

                //	Replace any records that were removed when inserting
                if ((dxHolding != null) && (dxHolding.Count > 0))
                {
                    foreach (CDxBinderEntry O in dxHolding)
                        dxEntries.AddList(O);

                    dxHolding.Clear();
                    dxHolding = null;

                }// if((dxHolding != null) && (dxHolding.Count > 0))

                //	Make sure that all the display order values are set
                for (int i = 0; i < dxEntries.Count; i++)
                {
                    if (dxEntries[i].DisplayOrder != (i + 1))
                    {
                        dxEntries[i].DisplayOrder = (i + 1);
                        dxEntries.Update(dxEntries[i]);
                    }

                }// for(int i = 0; i < dxEntries.Count; i++)

                //	Restore the KeepSorted property
                dxEntries.KeepSorted = bKeepSorted;

                //	Update the parent's child count
                if ((dxParent != null) && (dxParent.Collection != null))
                {
                    dxParent.ChildCount = dxEntries.Count;
                    dxParent.Collection.Update(dxParent);
                }

                //	Update each of the old parent binders
                foreach (CTmaxItem O in tmaxMove.SubItems)
                {
                    //	Remove from the caller's collection
                    tmaxMove.SourceItems.Remove(O);

                    if ((dxParent = ((CDxBinderEntry)(O.UserData1))) == null)
                        continue;

                    //	The child count should have gone down
                    if (dxParent.ChildCount != dxParent.Contents.Count)
                    {
                        dxParent.ChildCount = dxParent.Contents.Count;
                        if ((dxParent.AutoId > 0) && (dxParent.Collection != null))
                            dxParent.Collection.Update(dxParent);
                    }

                    //	Make sure the DisplayOrder value for all old siblings is correct
                    for (int i = 0; i < dxParent.Contents.Count; i++)
                    {
                        if (dxParent.Contents[i].DisplayOrder != (i + 1))
                        {
                            dxParent.Contents[i].DisplayOrder = (i + 1);
                            dxParent.Update(dxParent.Contents[i]);
                        }

                    }// for(int i = 0; i < dxEntries.Count; i++)

                }// foreach(CTmaxItem O in tmaxMove.SubItems)

                bSuccessful = true;

            }
            catch (System.Exception Ex)
            {
                FireError(this, "MoveBinderEntries", this.ExBuilder.Message(ERROR_CASE_DATABASE_MOVE_BINDER_ENTRIES_EX), Ex);
            }

            return bSuccessful;

        }// private bool MoveBinderEntries(CTmaxItem tmaxMove, bool bBefore)

        /// <summary>This method will add new pick items using the specified event items</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxAdded">Collection to populate with items that represent the new records</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        private bool AddPickItems(CTmaxItem tmaxItem, CTmaxItems tmaxAdded, CTmaxParameters tmaxParameters)
        {
            //	The event item should represent the parent pick item
            Debug.Assert(tmaxItem.DataType == TmaxDataTypes.PickItem);
            if (tmaxItem.DataType != TmaxDataTypes.PickItem) return false;

            //	Make sure we have stuff to add
            Debug.Assert(tmaxItem.SourceItems != null);
            Debug.Assert(tmaxItem.SourceItems.Count > 0);
            if ((tmaxItem.SourceItems == null) || (tmaxItem.SourceItems.Count == 0)) return false;

            //	Make sure pick lists are enabled
            if (this.PickListsEnabled == false) return false;

            //	The source items should contain the items to be added
            foreach (CTmaxItem O in tmaxItem.SourceItems)
            {
                //	Should be a pick item bound to this event item
                if (O.PickItem == null) continue;

                //	Make sure the correct parent has been assigned
                if (tmaxItem.PickItem == null)
                    O.PickItem.Parent = this.PickLists;
                else
                    O.PickItem.Parent = tmaxItem.PickItem;

                //	Add this item
                AddPickItem(O.PickItem, false, true, tmaxAdded);

            }// foreach(CTmaxItem O in tmaxItem.SourceItems)

            return true;

        }// private bool AddPickItems(CTmaxItem tmaxItem, CTmaxItems tmaxAdded, CTmaxParameters tmaxParameters)

        /// <summary>This method will add a new pick item using the specified objects</summary>
        /// <param name="tmaxParent">The parent pick list item</param>
        /// <param name="tmaxChildren">The collection of child items to be added</param>
        /// <param name="bAutoId">True if the database should be allowed to assign the UniqueId of each code</param>
        /// <param name="tmaxAdded">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        private bool AddPickItem(CTmaxPickItem tmaxPickItem, bool bChildren, bool bAutoId, CTmaxItems tmaxAdded)
        {
            ArrayList aChildren = null;
            CDxPickItem dxParent = null;
            CDxPickItem dxChild = null;
            CTmaxItem tmaxChild = null;

            //	Make sure pick lists are enabled
            if (this.PickListsEnabled == false) return false;

            //	Exchange interface for the parent
            if ((tmaxPickItem.Parent != null) && (tmaxPickItem.Parent.DxRecord != null))
                dxParent = (CDxPickItem)(tmaxPickItem.Parent.DxRecord);

            //	Add to the parent's child collection
            if (dxParent != null)
            {
                //	Allocate and initialize a new child object
                dxChild = new CDxPickItem(dxParent);
                dxChild.TmaxPickItem = tmaxPickItem;

                //	Add to the database
                if (dxParent.Add(dxChild, bAutoId) == true)
                {
                    //	Cross link the two objects
                    dxChild.TmaxPickItem.DxRecord = dxChild;

                    //	Add an item to the result collection
                    if (tmaxAdded != null)
                    {
                        tmaxChild = new CTmaxItem(dxChild.TmaxPickItem);
                        tmaxAdded.Add(tmaxChild);
                    }

                }// if(dxParent.Children.Add(dxChild) != null)

            }// if(dxParent != null)

            //	Are we supposed to add the children?
            if ((bChildren == true) && (tmaxPickItem.Children != null) && (tmaxPickItem.Children.Count > 0))
            {
                //	Copy the children to a temporary collection
                //
                //	NOTE:	We do this so that the child doesn't get added to the list
                //			again when it gets added to the database
                aChildren = new ArrayList();
                foreach (CTmaxPickItem O in tmaxPickItem.Children)
                    aChildren.Add(O);
                tmaxPickItem.Children.Clear(false);

                //	Add each of the children to the database
                foreach (CTmaxPickItem O in aChildren)
                    AddPickItem(O, true, bAutoId, tmaxChild != null ? tmaxChild.SubItems : null);

                aChildren.Clear();
            }

            return true;

        }// private bool AddPickItem(CTmaxPickItem tmaxPickItem, bool bChildren, bool bAutoId, CTmaxItems tmaxAdded)

        /// <summary>This method will add new case codes using the specified event items</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxAdded">Collection to populate with items that represent the new records</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        private bool AddCaseCodes(CTmaxItem tmaxItem, CTmaxItems tmaxAdded, CTmaxParameters tmaxParameters)
        {
            CTmaxCaseCodes tmaxCodes = null;
            CTmaxCaseCode tmaxInsertAt = null;
            CTmaxParameter tmaxParameter = null;
            bool bSuccessful = false;
            bool bBefore = true;

            //	The event item should be of type CaseCode
            Debug.Assert(tmaxItem.DataType == TmaxDataTypes.CaseCode);

            //	Make sure we have stuff to add
            Debug.Assert(tmaxItem.SourceItems != null);
            Debug.Assert(tmaxItem.SourceItems.Count > 0);
            if ((tmaxItem.SourceItems == null) || (tmaxItem.SourceItems.Count == 0)) return false;

            //	Create a temporary collection to store the codes
            tmaxCodes = new CTmaxCaseCodes();

            //	The source items should contain the items to be added
            foreach (CTmaxItem O in tmaxItem.SourceItems)
            {
                //	Should be a case code bound to this event item
                if (O.CaseCode != null)
                {
                    tmaxCodes.Add(O.CaseCode);
                }

            }// foreach(CTmaxItem O in tmaxItem.SourceItems)

            //	Add the codes to the database
            if (tmaxCodes.Count > 0)
            {
                //	Did the caller specify an insertion point?
                if ((tmaxItem.SubItems != null) && (tmaxItem.SubItems.Count > 0))
                    tmaxInsertAt = tmaxItem.SubItems[0].CaseCode;

                if (tmaxParameters != null)
                {
                    if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Before)) != null)
                        bBefore = tmaxParameter.AsBoolean();
                }

                bSuccessful = AddCaseCodes(tmaxCodes, tmaxInsertAt, bBefore, true, tmaxAdded);

                tmaxCodes.Clear();

            }

            return bSuccessful;

        }// private bool AddCaseCodes(CTmaxItem tmaxItem, CTmaxItems tmaxAdded, CTmaxParameters tmaxParameters)

        /// <summary>This method will add a treatment using the specified TmaxPresentation treatment descriptor</summary>
        /// <param name="strPageId">The unique id of the parent page</param>
        /// <param name="strFileSpec">The fully qualified path to the treatment file</param>
        ///	<param name="tmaxResults">Collection in which to place items representing new records</param>
        /// <returns>the exchange interface for the new treatment</returns>
        /// <remarks>The file should be formatted according to: uniqueid_##.zap_</remarks>
        public CDxTertiary AddTreatment(string strPageId, string strFileSpec, CTmaxDatabaseResults tmaxResults)
        {
            CDxTertiary dxTertiary = null;
            CDxMediaRecord dxPage = null;
            string strMsg = "";
            CTmaxItem tmaxParent = null;

            try
            {
                //	Get the secondary record
                if ((dxPage = GetRecordFromId(strPageId, true)) != null)
                {
                    Debug.Assert(dxPage.GetMediaLevel() == TmaxMediaLevels.Secondary);
                    if (dxPage.GetMediaLevel() != TmaxMediaLevels.Secondary)
                    {
                        dxPage = null;
                    }
                }

                //	Did we have a problem with locating the parent record?
                if (dxPage == null)
                {
                    strMsg = String.Format("{0} is not a valid database identifier for a document page.",
                                            strPageId);
                    OnAddPresentationError(strMsg, strFileSpec);
                    return null;
                }

                //	Add the treatment to the database
                if ((dxTertiary = AddTreatment((CDxSecondary)dxPage, strFileSpec)) != null)
                {
                    //	Delete the source file
                    try { System.IO.File.Delete(strFileSpec); }
                    catch { }

                    //	Add an item to the caller's collection
                    if ((tmaxResults != null) && (tmaxResults.Added != null))
                    {
                        //	Get the parent node
                        if ((tmaxParent = tmaxResults.Added.Find(dxTertiary.Secondary)) == null)
                        {
                            tmaxParent = new CTmaxItem(dxTertiary.Secondary);
                            tmaxResults.Added.Add(tmaxParent);
                        }

                        tmaxParent.SubItems.Add(new CTmaxItem(dxTertiary));

                    }

                }// if((dxTertiary = AddTreatment((CDxSecondary)dxPage, tmaxZap.SourceFileSpec)) != null)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddTreatment", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_ZAP_TREATMENT_EX, strFileSpec), Ex);
            }

            return dxTertiary;

        }// public CDxTertiary AddTreatment(CTmaxZapFile tmaxZap, CTmaxDatabaseResults tmaxResults)

        /// <summary>This method will add a treatment to the secondary page specified by the caller</summary>
        /// <param name="dxSecondary">The secondary media object that owns the new treatment</param>
        /// <param name="strSource">The source file for the new treatment</param>
        /// <returns>The record exchange object for the new treatment</returns>
        private CDxTertiary AddTreatment(CDxSecondary dxSecondary, string strSource)
        {
            CDxTertiary dxTertiary = null;
            string strFileSpec = "";

            if ((dxSecondary == null) || (strSource.Length == 0)) return null;

            //	Make sure the secondary child collection is filled
            if ((dxSecondary.ChildCount > 0) && (dxSecondary.Tertiaries.Count == 0))
            {
                if (dxSecondary.Fill() == false)
                    return null;
            }

            //	Make sure the root folder for treatments exists before
            //	attempting to add a new one
            if (CreateCaseFolder(TmaxMediaTypes.Treatment, true) == false)
                return null;

            //	Initialize a new tertiary exchange object
            dxTertiary = new CDxTertiary(dxSecondary);
            dxTertiary.MediaType = TmaxMediaTypes.Treatment;
            dxTertiary.Filename = ""; // Don't know until after we add
            dxTertiary.HasShortcuts = false;
            if (dxSecondary.Tertiaries.Count > 0)
                dxTertiary.DisplayOrder = dxSecondary.Tertiaries[dxSecondary.Tertiaries.Count - 1].DisplayOrder + 1;
            else
                dxTertiary.DisplayOrder = dxSecondary.ChildCount + 1; // Just in case

            dxTertiary.BarcodeId = dxSecondary.Tertiaries.GetNextBarcodeId();

            //	Add the new record to the secondary collection
            if (dxSecondary.Add(dxTertiary) == true)
            {
                //	Get the filename we want to use for this treatment
                strFileSpec = GetFileSpec(dxTertiary);

                //	Update the child counter on the secondary record
                dxSecondary.ChildCount = dxSecondary.Tertiaries.Count;
                dxSecondary.Collection.Update(dxSecondary);

                //	Copy the source file
                try
                {
                    //	First make sure the parent folder exists
                    if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(strFileSpec)) == false)
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(strFileSpec));

                    System.IO.File.Copy(strSource, strFileSpec, true);
                    System.IO.File.SetAttributes(strFileSpec, System.IO.FileAttributes.Normal);
                }
                catch (System.Exception Ex)
                {
                    FireError(this, "AddTreatment", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_TREATMENT_EX), Ex);
                    dxSecondary.Delete(dxTertiary);
                    return null;
                }

                //	Now update the FileName field in the tertiary record
                dxTertiary.Filename = System.IO.Path.GetFileName(strFileSpec);
                dxSecondary.Tertiaries.Update(dxTertiary);

                return dxTertiary;
            }

            return null;
        }

        /// <summary>This method will set the media and source type identifiers for the specified folder and it's subfolders</summary>
        /// <param name="tmaxSource">The folder containing all subfolders to be typed</param>
        /// <param name="eSourceType">The registration source type to be associated with each folder</param>
        /// <returns>true if successful</returns>
        private bool SetSourceTypes(CTmaxSourceFolder tmaxSource, RegSourceTypes eSourceType)
        {
            bool bCheckFile = false;
            CTmaxSourceType tmaxSourceType = null;

            Debug.Assert(m_tmaxSourceTypes != null);
            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.SubFolders != null);

            SetRegisterProgress("Setting source media types ...");

            //	Do we need to check the files to determine the types?
            if ((eSourceType == RegSourceTypes.AllFiles) || (eSourceType == RegSourceTypes.NoSource))
                bCheckFile = true;

            //	Drill down into the subfolders
            //
            //	NOTE:	We do this first because we might be adding folders
            //			to the collection if this folder contains mixed types
            foreach (CTmaxSourceFolder tmaxSubFolder in tmaxSource.SubFolders)
            {
                try
                {
                    if (m_bRegisterCancelled == true) return true;

                    SetSourceTypes(tmaxSubFolder, eSourceType);
                }
                catch (System.Exception Ex)
                {
                    FireError(this, "SetSourceTypes", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_SOURCE_TYPES_EX), Ex);
                }

            }// foreach(CTmaxSourceFolder tmaxSubFolder in tmaxSource.SubFolders)

            //	Has the caller specified the type to be assigned?
            if (bCheckFile == false)
            {
                //	What type of source are we dealing with?
                switch (eSourceType)
                {
                    case RegSourceTypes.Powerpoint:
                    case RegSourceTypes.Deposition:
                    case RegSourceTypes.Adobe:
                    case RegSourceTypes.MultiPageTIFF:

                        //	Put each file in its own folder and get rid of files
                        //	that are not of the specified type
                        SeparateSource(tmaxSource, null, eSourceType, true);
                        break;

                    default:

                        //	Set the registration type
                        tmaxSource.SourceType = eSourceType;

                        //	Retrieve the source type descriptor to set the appropriate
                        //	media type identifier
                        if ((tmaxSourceType = m_tmaxSourceTypes.Find(eSourceType)) != null)
                        {
                            tmaxSource.MediaType = tmaxSourceType.MediaType;
                        }
                        else
                        {
                            Debug.Assert(false, "media type object not found");
                            tmaxSource.MediaType = TmaxMediaTypes.Unknown;
                        }
                        break;

                }// switch(eSourceType)

            }
            else if (tmaxSource.Files.Count > 0)
            {
                //	This is easy if there is only one file
                if (tmaxSource.Files.Count == 1)
                {
                    if ((tmaxSourceType = m_tmaxSourceTypes.FindFromFilename(tmaxSource.Files[0].Name)) != null)
                    {
                        tmaxSource.SourceType = tmaxSourceType.RegSourceType;
                        tmaxSource.MediaType = tmaxSourceType.MediaType;
                    }
                    else
                    {
                        tmaxSource.SourceType = RegSourceTypes.NoSource;
                        tmaxSource.MediaType = TmaxMediaTypes.Unknown;
                        tmaxSource.Files.Clear(); // not valid media
                    }
                }
                else
                {
                    GroupSource(tmaxSource);
                }

            }// else if(tmaxSource.Files.Count > 0)

            return true;

        }// private bool SetSourceTypes(CTmaxSourceFolder tmaxSource, RegSourceTypes eSourceType)

        /// <summary>This method will set the media id of a primary record that's just been added to the database</summary>
        /// <param name="dxPrimary">The primary record's data exchange object</param>
        /// <param name="tmaxSource">Source folder used to create the record</param>
        /// <returns>true if successful</returns>
        private bool SetMediaId(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource)
        {
            int iUpdate;
            string strOriginal = dxPrimary.MediaId;
            string strDuplicate = "";
            string strDesired = "";
            bool bSuccessful = false;
            bool bResolved = false;
            bool bAssignFolder = false;

            //	Is this primary being created from an XML primary node?
            if ((tmaxSource.XmlPrimary != null) && (tmaxSource.XmlPrimary.MediaId.Length > 0))
            {
                //	Use the MediaId assigned to the node
                dxPrimary.MediaId = AdjustForMediaId(tmaxSource.XmlPrimary.MediaId, tmaxSource.MediaType);
            }
            else
            {
                //	Should we assign the folder name to the media id?
                Debug.Assert(m_tmaxRegisterOptions != null);
                if (m_tmaxRegisterOptions != null)
                    bAssignFolder = m_tmaxRegisterOptions.GetFolderAssignment(RegFolderAssignments.MediaId);

                //	Construct the Media Id
                if (bAssignFolder == true)
                {
                    dxPrimary.MediaId = GetAssignmentText(tmaxSource, true);
                }
                else
                {
                    dxPrimary.MediaId = m_tmaxRegisterOptions.Resolve(dxPrimary, "", RegConflictResolutions.Automatic);
                    dxPrimary.MediaId = FormatMediaId(dxPrimary.MediaId, dxPrimary.MediaType);
                }

            }// if((tmaxSource.XmlPrimary != null) && (tmaxSource.XmlPrimary.MediaId.Length > 0))

            //	Apply the user defined morphing options
            dxPrimary.MediaId = Morph(dxPrimary.MediaId);

            //	Save the first attempt in case we have to report a conflict
            strDesired = dxPrimary.MediaId;

            while (bSuccessful == false)
            {
                //	Don't bother if the operation has been cancelled
                if (m_bRegisterCancelled == true)
                    break;

                //	Attempt to update the media id
                if ((iUpdate = m_dxPrimaries.UpdateMediaId(dxPrimary)) == 0)
                {
                    bSuccessful = true;// we're done
                }
                else if (iUpdate < 0)//	Did a system error occur?
                {
                    break;// break on error
                }
                else
                {
                    //	Must be a duplicate, we have to resolve
                    strDuplicate = dxPrimary.MediaId;
                    if (Resolve(tmaxSource, dxPrimary, ref strDuplicate, true) == false)
                    {
                        break;// Unable to resolve
                    }
                    else
                    {
                        //	Try again
                        dxPrimary.MediaId = FormatMediaId(strDuplicate, dxPrimary.MediaType);
                        bResolved = true;

                    }// if(Resolve(tmaxSource, ref strTargetPath, false) == false)

                }// if(iUpdate < 0)

            }// while(bSuccessful == false)

            if (bSuccessful == true)
            {
                //	Report the conflict if a resolution was required
                if (bResolved == true)
                {
                    if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                    {
                        m_cfRegisterProgress.OnConflict(dxPrimary.AutoId, strDesired, dxPrimary.MediaId);
                    }

                }// if(bResolved == true)

            }
            else
            {
                dxPrimary.MediaId = strOriginal;// restore the original
            }

            return bSuccessful;

        }// private bool SetMediaId(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource)

        /// <summary>This method will group the source files in the specified folder by primary media type</summary>
        /// <param name="tmaxSource">The folder containing the source files</param>
        /// <returns>true if successful</returns>
        ///	<remarks>This method does not drill down into subfolders</remarks>
        private bool GroupSource(CTmaxSourceFolder tmaxSource)
        {
            CTmaxSourceType tmaxSourceType = null;
            CTmaxSourceFolders tmaxFolders = null;
            CTmaxSourceFolder tmaxFolder = null;
            bool bSuccessful = false;

            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.Files.Count > 1);
            Debug.Assert(m_tmaxSourceTypes != null);

            try
            {
                if (m_bRegisterCancelled == true) return true;

                SetRegisterProgress("Grouping source files ...");

                //	Create the temporary collection to hold the new folders
                tmaxFolders = new CTmaxSourceFolders();

                //	Start by extracting depositions. We do this first to make things
                //	easier. Separating the depositions will also remove any segments
                //	owned by the deposition. That keeps us from registering the
                //	segments as part of a recording
                SeparateSource(tmaxSource, tmaxFolders, RegSourceTypes.Deposition, false);

                //	Check the each of the remaining files
                foreach (CTmaxSourceFile tmaxFile in tmaxSource.Files)
                {
                    //	Get the source type for this file
                    tmaxSourceType = m_tmaxSourceTypes.FindFromFilename(tmaxFile.Name);
                    if (tmaxSourceType == null) continue;

                    //	What type of source are we dealing with?
                    switch (tmaxSourceType.RegSourceType)
                    {
                        case RegSourceTypes.Deposition:

                            //	These should have already been processed
                            Debug.Assert(false, "Tmdata::GroupSource() - depositions already processed");
                            continue;

                        case RegSourceTypes.Powerpoint:
                        case RegSourceTypes.Adobe:
                        case RegSourceTypes.MultiPageTIFF:

                            //	Force creation of a new folder for this type of file
                            //
                            //	NOTE:	We could preprocess this type like we did
                            //			depositions but that would slow things down
                            tmaxFolder = null;
                            break;

                        default:

                            //	Get the correct folder for this type
                            if ((tmaxFolder == null) || (tmaxFolder.SourceType != tmaxSourceType.RegSourceType))
                            {
                                tmaxFolder = tmaxFolders.Find(tmaxSourceType.RegSourceType);
                            }
                            break;

                    }// switch(tmaxSourceType.RegSourceType)

                    //	Do we need to create a new folder?
                    if (tmaxFolder == null)
                    {
                        tmaxFolder = new CTmaxSourceFolder(tmaxSource.Path);
                        tmaxFolder.SourceType = tmaxSourceType.RegSourceType;
                        tmaxFolder.MediaType = tmaxSourceType.MediaType;
                        tmaxFolder.PrimaryAttributes = tmaxSource.PrimaryAttributes;

                        //	Add to our temporary collection
                        tmaxFolders.Add(tmaxFolder);
                    }

                    //	Add this file to the temporary folder
                    tmaxFolder.Files.Add(tmaxFile);

                }// foreach(CTmaxSourceFile tmaxFile in tmaxSource.Files)

                //	All valid source files are now in temporary folders
                tmaxSource.Files.Clear();

                //	Do we have more than one temporary folder?
                if (tmaxFolders.Count > 1)
                {
                    //	Add each of the temporary folders to the source's subfolder collection
                    for (int i = 0; i < tmaxFolders.Count; i++)
                    {
                        tmaxSource.SubFolders.Insert(tmaxFolders[i], i);

                        //	Set the suffix in case the folder name is being used for MediaId
                        if (tmaxFolders[i].Suffix.Length == 0)
                            tmaxFolders[i].Suffix = ("_" + tmaxFolders[i].MediaType.ToString());

                    }//for(int i = 0; i < iFolders; i++) 

                    //	Source folder no longer contains files
                    tmaxSource.SourceType = RegSourceTypes.NoSource;
                    tmaxSource.MediaType = TmaxMediaTypes.Unknown;
                    tmaxSource.PrimaryAttributes = 0;
                }
                else if (tmaxFolders.Count == 1)
                {
                    //	Copy the relavent properties to the original source folder
                    tmaxSource.SourceType = tmaxFolders[0].SourceType;
                    tmaxSource.MediaType = tmaxFolders[0].MediaType;
                    tmaxSource.UserData = tmaxFolders[0].UserData;

                    //	Put the files back in the source folder
                    foreach (CTmaxSourceFile O in tmaxFolders[0].Files)
                        tmaxSource.Files.Add(O);

                    tmaxFolders[0].Files.Clear();
                }

                bSuccessful = true;

            }
            catch (System.Exception Ex)
            {
                FireError(this, "GroupSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_GROUP_SOURCE_EX), Ex);
            }

            if (tmaxFolders != null)
            {
                tmaxFolders.Clear();
                tmaxFolders = null;
            }

            return bSuccessful;

        }// private bool GroupSource(CTmaxSourceFolder tmaxSource)

        /// <summary>This method will retrieve the deposition properties and segment collection from the XML file</summary>
        /// <param name="tmaxSource">The folder containing the deposition source</param>
        /// <returns>true if successful</returns>
        private bool GetDepoSegments(CTmaxSourceFolder tmaxDeposition)
        {
            string strExtension = "";
            string strFolder = "";
            bool bConverted = false;
            CTmaxSourceFile tmaxFile = null;
            CXmlDeposition xmlDeposition = null;

            Debug.Assert(tmaxDeposition != null);
            Debug.Assert(tmaxDeposition.MediaType == TmaxMediaTypes.Deposition);
            Debug.Assert(tmaxDeposition.Files != null);
            Debug.Assert(tmaxDeposition.Files.Count == 1);

            //	Have we already retrieved the segments for this deposition
            if (tmaxDeposition.UserData != null) return true;

            //	Should have the transcript file in the folder
            if ((tmaxDeposition.Files == null) || (tmaxDeposition.Files.Count == 0))
                return false;

            //	Do we have to convert the file to an XML deposition?
            strExtension = System.IO.Path.GetExtension(tmaxDeposition.Files[0].Name);
            if ((strExtension != null) && (String.Compare(strExtension, ".log", true) == 0))
            {
                if (ConvertSourceDeposition(tmaxDeposition, null) == false)
                    return false;
                else
                    bConverted = true;
            }

            //	Load the deposition properties and segments
            xmlDeposition = new CXmlDeposition();
            SetHandlers(xmlDeposition.EventSource);
            xmlDeposition.Converted = bConverted;

            if (xmlDeposition.FastFill(tmaxDeposition.Files[0].Path, true, false, false) == true)
            {
                //	Store the XML deposition for use during registration
                tmaxDeposition.UserData = xmlDeposition;

                //	Get the folder path
                strFolder = tmaxDeposition.Path;
                if (strFolder.EndsWith("\\") == false)
                    strFolder += "\\";

                //	Clear the files collection so that we can transfer all 
                //	segments
                tmaxDeposition.Files.Clear();
                tmaxDeposition.Files.KeepSorted = false; //	Maintain segments order

                //	Now add a file for each segment
                if (xmlDeposition.Segments != null)
                {
                    foreach (CXmlSegment xmls in xmlDeposition.Segments)
                    {
                        tmaxFile = new CTmaxSourceFile(strFolder + xmls.Filename);
                        tmaxFile.UserData = xmls;
                        tmaxDeposition.Files.Add(tmaxFile);
                    }

                }

            }
            else
            {
                FireError(this, "GetDepoSegments", "Unable to read source XML: filename = " + tmaxDeposition.Files[0].Path);
                xmlDeposition.Clear();
            }

            return (tmaxDeposition.UserData != null);

        }// private bool GetDepoSegments(CTmaxSourceFolder tmaxDeposition)

        /// <summary>This method will convert the deposition's legacy log file to a properly formatted XML deposition</summary>
        /// <param name="tmaxDeposition">The folder containing the source deposition information</param>
        ///	<param name="dxTranscript">Optional transcript record associated with this deposition</param>
        /// <returns>true if successful</returns>
        private bool ConvertSourceDeposition(CTmaxSourceFolder tmaxDeposition, CDxTranscript dxTranscript)
        {
            bool bSuccessful = false;
            string strFilename = "";

            FTI.Trialmax.Database.Legacy.CTmaxDepoLog depoLog = new FTI.Trialmax.Database.Legacy.CTmaxDepoLog();

            //	Connect the event sources
            depoLog.DepoLogProgressEvent += new FTI.Trialmax.Database.Legacy.CTmaxDepoLog.DepoLogProgressHandler(this.OnDepoLogProgress);
            SetHandlers(depoLog.EventSource);
            SetHandlers(depoLog.XmlDeposition.EventSource);

            Cursor.Current = Cursors.WaitCursor;

            //	Perform the conversion
            SetRegisterProgress("Converting " + tmaxDeposition.Files[0].Path);
            if (depoLog.Convert(tmaxDeposition.Files[0].Path) == false)
            {
                Cursor.Current = Cursors.Default;
                return false;
            }

            //	Get a temporary path where we can store the new xml
            strFilename = System.IO.Path.GetTempPath();
            if (strFilename.EndsWith("\\") == false)
                strFilename += "\\";
            strFilename += System.IO.Path.GetFileNameWithoutExtension(tmaxDeposition.Files[0].Path);
            if (strFilename.EndsWith(".") == false)
                strFilename += ".";
            strFilename += CXmlDeposition.GetExtension();

            SetRegisterProgress("Saving converted log to XML");
            if (depoLog.XmlDeposition.Save(strFilename) == true)
            {
                bSuccessful = true;

                //	Replace tmaxDepositons.Files[0] with info for new file after converting
                tmaxDeposition.Files[0].Initialize(strFilename);
                tmaxDeposition.Files[0].Temporary = true;
            }

            //	Update the transcript extents
            if (dxTranscript != null)
            {
                try
                {
                    dxTranscript.FirstPL = depoLog.XmlDeposition.GetFirstPL();
                    dxTranscript.LastPL = depoLog.XmlDeposition.GetLastPL();
                }
                catch
                {
                }
            }

            //	Close down the file
            depoLog.XmlDeposition.Clear();
            depoLog.XmlDeposition.Close();

            Cursor.Current = Cursors.Default;

            return bSuccessful;

        }// private bool ConvertSourceDeposition(CTmaxSourceFolder tmaxDeposition)

        /// <summary>This method will update the deposition transcript using the log file specified by the caller</summary>
        /// <param name="dxDeposition">The deposition record being updated</param>
        /// <returns>true if successful</returns>
        private bool UpdateDeposition(CDxPrimary dxDeposition)
        {
            OpenFileDialog openFile = null;
            CDxTranscript dxTranscript = null;
            CTmaxSourceFolder tmaxSource = null;
            string strFileSpec = "";
            string strBackup = "";

            Debug.Assert(dxDeposition != null);
            if (dxDeposition == null) return false;

            if ((dxTranscript = dxDeposition.GetTranscript()) == null)
                return false;

            //	Get the file to the existing XML transcript
            strFileSpec = GetFileSpec(dxTranscript);

            //	Initialize the file selection dialog
            openFile = new System.Windows.Forms.OpenFileDialog(); ;
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.Multiselect = false;
            openFile.Title = "Select Log File";
            openFile.Filter = "Log files (*.log)|*.log|All Files (*.*)|*.*";

            openFile.FileName = System.IO.Path.GetFileNameWithoutExtension(strFileSpec);
            openFile.FileName += ".log";

            if (dxDeposition.RegisterPath != null && (dxDeposition.RegisterPath.Length > 0))
            {
                if (System.IO.Directory.Exists(dxDeposition.RegisterPath) == true)
                    openFile.InitialDirectory = dxDeposition.RegisterPath;
            }

            //	Open the dialog box
            if (openFile.ShowDialog() == DialogResult.Cancel) return false;

            //	Allocate a folder to pass to the conversion routine
            tmaxSource = new CTmaxSourceFolder(System.IO.Path.GetDirectoryName(openFile.FileName));
            tmaxSource.Files.Add(new CTmaxSourceFile(openFile.FileName));

            if (ConvertSourceDeposition(tmaxSource, dxTranscript) == false)
                return false;

            strBackup = System.IO.Path.ChangeExtension(strFileSpec, "bak");

            //	Back up the existing file
            if (System.IO.File.Exists(strFileSpec) == true)
            {
                if (System.IO.File.Exists(strBackup) == true)
                {
                    try { System.IO.File.Delete(strBackup); }
                    catch { }
                }
                try { System.IO.File.Move(strFileSpec, strBackup); }
                catch { }

            }

            //	Now copy the new transcript into place
            try
            {
                System.IO.File.Move(tmaxSource.Files[0].Path, strFileSpec);

                //	Update the transcript extents
                if (dxTranscript.Collection != null)
                    dxTranscript.Collection.Update(dxTranscript);

                return true;
            }
            catch
            {
                //	Restore the backup
                if (System.IO.File.Exists(strBackup) == true)
                {
                    try { System.IO.File.Copy(strBackup, strFileSpec, true); }
                    catch { }
                }

            }

            //	Must have been some kind of error
            return false;

        }// private bool UpdateDeposition(CDxPrimary dxDeposition)

        /// <summary>This method is called to remove any segment associated with the deposition from the specified recording files</summary>
        /// <param name="tmaxDeposition">The deposition that owns the segments being removed</param>
        /// <param name="tmaxRecording">The recording that may or may not contain the video segments</param>
        private void RemoveSegments(CTmaxSourceFolder tmaxDeposition, CTmaxSourceFolder tmaxRecording)
        {
            CXmlDeposition xmlDeposition = null;
            int iIndex = 0;

            Debug.Assert(tmaxDeposition != null);
            Debug.Assert(tmaxRecording != null);

            //	Do we need to get the segment information?
            if (tmaxDeposition.UserData == null)
            {
                if (GetDepoSegments(tmaxDeposition) == false)
                    return;
            }

            //	We have to have the segments
            if (tmaxDeposition.UserData == null) return;
            if (tmaxRecording.Files == null) return;
            if (tmaxRecording.Files.Count == 0) return;

            xmlDeposition = (CXmlDeposition)tmaxDeposition.UserData;

            if ((xmlDeposition.Segments != null) && (xmlDeposition.Segments.Count > 0))
            {
                foreach (CXmlSegment xmls in xmlDeposition.Segments)
                {
                    if ((iIndex = tmaxRecording.Files.GetIndexFromName(xmls.Filename)) >= 0)
                    {
                        tmaxRecording.Files.RemoveAt(iIndex);
                    }
                }

            }

        }// private void RemoveSegments(CTmaxSourceFolder tmaxDeposition, CTmaxSourceFolder tmaxRecording)

        /// <summary>Called to create a new primary record and add it to the database</summary>
        /// <param name="tmaxSource">The source folder used to initialize the object</param>
        /// <returns>An new primary exchange object if successful</returns>
        private CDxPrimary CreatePrimary(CTmaxSourceFolder tmaxSource)
        {
            CDxPrimary dxPrimary = null;
            bool bSuccessful = false;

            try
            {
                //	Do some preprocessing if required
                switch (tmaxSource.SourceType)
                {
                    case RegSourceTypes.Deposition:

                        //	Make sure we have the transcript and segments
                        //	before attempting to add a deposition
                        if (tmaxSource.UserData == null)
                        {
                            if (GetDepoSegments(tmaxSource) == false)
                            {
                                return null;
                            }
                        }
                        break;

                    case RegSourceTypes.Adobe:

                        //	Make sure the PDF converter is ready
                        if (PrepAdobeConverter(tmaxSource) == false)
                            return null;

                        break;

                }// switch(tmaxSource.SourceType)

                //	Create a default object
                if ((dxPrimary = CreatePrimary(tmaxSource.MediaType)) == null)
                    return null;

                //	Make sure the media id field is unique when we initially add the record
                dxPrimary.MediaId = System.Guid.NewGuid().ToString();
                dxPrimary.RegisterPath = tmaxSource.Path;
                dxPrimary.Attributes = tmaxSource.PrimaryAttributes;

                //	Add the record to the database
                //
                //	NOTE:	We have to do this now to get a valid AutoId value for the record
                if (m_dxPrimaries.Add(dxPrimary) == null)
                    return null;

                //	Finish initializing this record
                while (bSuccessful == false)
                {
                    //	Set the path to the secondary files
                    if (SetTargetPath(tmaxSource, dxPrimary) == false)
                        break;

                    //	Make sure we have the secondary source files
                    if (ExtractSourceFiles(tmaxSource, dxPrimary) == false)
                        break;

                    //	Use the source folder to set the requested primary properties
                    if (SetProperties(tmaxSource, dxPrimary) == false)
                        break;

                    //	Perform an update now that we've set the properties
                    //
                    //	NOTE:	We used to wait to add the record until all the properties were
                    //			set but we can't do that any more because we want to be able to
                    //			use the AutoId value to resolve the target path if necessary
                    m_dxPrimaries.Update(dxPrimary);

                    //	Set the MediaId
                    //
                    //	NOTE:	We perform this as a separate operation because we need to
                    //			assume that an exception is because of duplicate values
                    if (SetMediaId(dxPrimary, tmaxSource) == false)
                        break;

                    //	All done...
                    bSuccessful = true;

                }// while(bSuccessful == false)

            }
            catch (ThreadAbortException Ex)
            {
                Console.WriteLine(Ex.ToString());
                //	Should we delete the record?
                if ((dxPrimary != null) && (bSuccessful == false))
                {
                    try { m_dxPrimaries.Delete(dxPrimary); }
                    catch { };
                    dxPrimary = null;
                }
            }
            catch (System.Exception Ex)
            {
                if (!m_bRegisterCancelled)
                    FireError(this, "CreatePrimary", this.ExBuilder.Message(ERROR_CASE_DATABASE_CREATE_PRIMARY_FOLDER_EX, tmaxSource.Path), Ex);
            }

            //	Should we delete the record?
            if ((dxPrimary != null) && (bSuccessful == false))
            {
                //  Make sure the output directory is deleted since those files wont be registered in the database
                string caseDirectory = GetCasePath(TmaxMediaTypes.Document); ;
                try
                {
                    if (!string.IsNullOrEmpty(dxPrimary.RelativePath) && Directory.Exists(caseDirectory + dxPrimary.RelativePath))
                    {
                        Console.WriteLine("Deleting directory= " + caseDirectory + dxPrimary.RelativePath);

                        DeleteDirectory(caseDirectory + dxPrimary.RelativePath);
                    }
                    else
                    {
                    }
                }
                catch (IOException Ex)
                {
                    //  Do Nothing
                }
                //  ---
                try { m_dxPrimaries.Delete(dxPrimary); }
                catch { };
                dxPrimary = null;
            }

            return dxPrimary;

        }// private CDxPrimary CreatePrimary(CTmaxSourceFolder tmaxSource)

        /// <summary>This method allocate and initalize a default primary exchange object</summary>
        /// <param name="tmaxType">The media type associated with the new record</param>
        /// <returns>An new primary exchange object if successful</returns>
        private CDxPrimary CreatePrimary(TmaxMediaTypes tmaxType)
        {
            CDxPrimary dxPrimary = null;

            try
            {
                dxPrimary = new CDxPrimary();

                dxPrimary.RelativePath = "";
                dxPrimary.MediaType = tmaxType;
                dxPrimary.Attributes = 0;
                dxPrimary.ChildCount = 0;
                dxPrimary.Filename = "";
                dxPrimary.AliasId = 0;
                dxPrimary.MediaId = System.Guid.NewGuid().ToString();
                dxPrimary.Name = "";
                dxPrimary.Exhibit = "";
                dxPrimary.Description = "";

                return dxPrimary;
            }
            catch (System.Exception Ex)
            {
                FireError(this, "CreatePrimary", this.ExBuilder.Message(ERROR_CASE_DATABASE_CREATE_PRIMARY_TYPE_EX, tmaxType.ToString() + " default"), Ex);
                return null;
            }

        }// CreatePrimary(TmaxMediaTypes tmaxType)

        /// <summary>
        /// This method will allocate and initalize a new secondary exchange object for the source file
        /// </summary>
        /// <param name="dxPrimary">The primary media object that owns the file</param>
        /// <param name="tmaxFile">The TrialMax source file descriptor</param>
        /// <returns>An new secondary exchange object if successful</returns>
        private CDxSecondary CreateSecondary(CDxPrimary dxPrimary, CTmaxSourceFile tmaxFile)
        {
            CDxSecondary dxSecondary = null;
            string strExtension = null;
            try
            {
                dxSecondary = new CDxSecondary(dxPrimary);

                dxSecondary.PrimaryMediaId = dxPrimary.AutoId;
                dxSecondary.Filename = tmaxFile.Name;
                dxSecondary.DisplayOrder = dxPrimary.Secondaries.GetNextDisplayOrder();
                dxSecondary.BarcodeId = dxSecondary.DisplayOrder;
                dxSecondary.MediaType = GetMediaType(dxPrimary.MediaType).SecondaryType;

                //	Is this secondary being created from an XML node?
                if (tmaxFile.XmlSecondary != null)
                {
                    //	Assign the values contained in the node
                    if (tmaxFile.XmlSecondary.Name.Length > 0)
                        dxSecondary.Name = tmaxFile.XmlSecondary.Name;
                    if (tmaxFile.XmlSecondary.Description.Length > 0)
                        dxSecondary.Name = tmaxFile.XmlSecondary.Description;
                }

                //	Is this a document?
                if (dxPrimary.MediaType == TmaxMediaTypes.Document)
                {
                    //	Is this a high resolution page?
                    strExtension = System.IO.Path.GetExtension(dxSecondary.Filename);
                    strExtension = strExtension.ToLower();

                    if ((strExtension.EndsWith("tif") == true) || (strExtension.EndsWith("tiff") == true))
                        dxSecondary.Attributes |= (uint)TmaxSecondaryAttributes.HighResPage;
                }

                return dxSecondary;
            }
            catch (System.Exception Ex)
            {
                FireError(this, "CreateSecondary", this.ExBuilder.Message(ERROR_CASE_DATABASE_CREATE_SECONDARY_EX, tmaxFile.Path), Ex);
                return null;
            }

        }// CreateSecondary(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxFolder, CTmaxSourceFile tmaxFile)

        /// <summary>This method will delete each of the records associated with the specified items</summary>
        /// <param name="tmaxItem">The event item that represents the parent of those records being deleted</param>
        /// <returns>true if records have been deleted</returns>
        private bool DeleteMedia(CTmaxItem tmaxItem)
        {
            CDxMediaRecord dxRecord = null;
            CDxPrimary dxPrimary = null;
            CDxSecondary dxSecondary = null;
            CDxTertiary dxTertiary = null;
            CTmaxItem tmaxParent = null;
            bool bCancel = false;

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.SubItems != null);

            //	Is there anything to delete?
            if (tmaxItem.SubItems.Count == 0) return false;

            //	Make a copy of the parent so that we can maintain
            //	the heirarchy in the trash can
            tmaxParent = new CTmaxItem();
            tmaxParent.IPrimary = tmaxItem.IPrimary;
            tmaxParent.ISecondary = tmaxItem.ISecondary;
            tmaxParent.ITertiary = tmaxItem.ITertiary;
            tmaxParent.IQuaternary = tmaxItem.IQuaternary;
            tmaxParent.MediaType = tmaxItem.MediaType;

            //	Delete each of the requested subitems
            foreach (CTmaxItem O in tmaxItem.SubItems)
            {
                if ((dxRecord = (CDxMediaRecord)(O.GetMediaRecord())) != null)
                {
                    DeleteMedia(dxRecord, tmaxParent.SubItems, ref bCancel);
                }

                //	Did the user cancel?
                if (bCancel == true)
                    break;
            }

            //	Do we need to update the parent?
            switch (tmaxItem.MediaLevel)
            {
                case TmaxMediaLevels.Primary:

                    if ((dxPrimary = (CDxPrimary)tmaxItem.GetMediaRecord()) != null)
                    {
                        if (dxPrimary.Secondaries.Count == 0)
                            dxPrimary.Secondaries.Fill();

                        //	Postpone this until user synchronizes	
                        //						for(i = 0; i < dxPrimary.Secondaries.Count; i++)
                        //						{
                        //							dxPrimary.Secondaries[i].DisplayOrder = i + 1;
                        //							dxPrimary.Secondaries.Update(dxPrimary.Secondaries[i]);
                        //						}

                        //	Update the primary record
                        dxPrimary.ChildCount = dxPrimary.Secondaries.Count;
                        if (dxPrimary.ChildCount > 0)
                            dxPrimary.Filename = dxPrimary.Secondaries[0].Filename;
                        else
                            dxPrimary.Filename = "";

                        if (dxPrimary.MediaType == TmaxMediaTypes.Script)
                            dxPrimary.SetPlaylistFromChildren();

                        m_dxPrimaries.Update(dxPrimary);

                    }
                    break;

                case TmaxMediaLevels.Secondary:

                    if ((dxSecondary = (CDxSecondary)tmaxItem.GetMediaRecord()) != null)
                    {
                        if (dxSecondary.Tertiaries.Count == 0)
                            dxSecondary.Tertiaries.Fill();

                        //	Postpone until user synchronizes
                        //						for(i = 0; i < dxSecondary.Tertiaries.Count; i++)
                        //						{
                        //							dxSecondary.Tertiaries[i].DisplayOrder = i + 1;
                        //							dxSecondary.Tertiaries.Update(dxSecondary.Tertiaries[i]);
                        //						}

                        //	Update the secondary record
                        dxSecondary.ChildCount = dxSecondary.Tertiaries.Count;
                        if (dxSecondary.Collection != null)
                            dxSecondary.Collection.Update(dxSecondary);
                    }
                    break;

                case TmaxMediaLevels.Tertiary:

                    if ((dxTertiary = (CDxTertiary)tmaxItem.GetMediaRecord()) != null)
                    {
                        if (dxTertiary.Quaternaries.Count == 0)
                            dxTertiary.Quaternaries.Fill();

                        //	Postpone until the user synchronizes
                        //						for(i = 0; i < dxTertiary.Quaternaries.Count; i++)
                        //						{
                        //							dxTertiary.Quaternaries[i].DisplayOrder = i + 1;
                        //							dxTertiary.Quaternaries.Update(dxTertiary.Quaternaries[i]);
                        //						}

                        //	Update the secondary record
                        dxTertiary.ChildCount = dxTertiary.Quaternaries.Count;
                        if (dxTertiary.Collection != null)
                            dxTertiary.Collection.Update(dxTertiary);
                    }
                    break;

                case TmaxMediaLevels.Quaternary:
                case TmaxMediaLevels.None:

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            //	Did we delete anything?
            if (tmaxParent.SubItems.Count > 0)
            {
                m_tmaxTrashCan.Add(tmaxParent);
                return true;
            }
            else
            {
                return false;
            }

        }// private bool DeleteMedia(CTmaxItem tmaxItem)

        /// <summary>Deletes the source records contained in the specified collection</summary>
        /// <param name="dxSources">The collection of source records to be deleted if possible</param>
        /// <returns>True if successful</returns>
        private bool DeleteSource(CDxMediaRecords dxSources)
        {
            CDxScenes dxScenes = null;
            CTmaxItem tmaxItem = null;

            Debug.Assert(dxSources != null);

            try
            {
                //	Allocate a collection to check for scenes that still reference
                //	the source record
                dxScenes = new CDxScenes(this);

                //	Check each of the source records to see if it should be deleted
                foreach (CDxMediaRecord O in dxSources)
                {
                    //	The source record must be a designation or clip
                    if ((O.MediaType != TmaxMediaTypes.Designation) &&
                        (O.MediaType != TmaxMediaTypes.Clip)) continue;

                    //	Get the scenes that still reference this source
                    if (((CDxTertiary)O).HasShortcuts == true)
                    {
                        dxScenes.Source = O;
                        dxScenes.Fill();
                    }

                    //	Are there no remaining references?
                    if ((((CDxTertiary)O).HasShortcuts == false) || (dxScenes.Count == 0))
                    {
                        //	It's safe to delete this designation
                        tmaxItem = new CTmaxItem(O.GetParent());
                        tmaxItem.SubItems.Add(new CTmaxItem(O));

                        DeleteMedia(tmaxItem);

                        tmaxItem.SubItems.Clear();
                        tmaxItem = null;

                    }
                    else
                    {
                        dxScenes.Clear();
                    }

                }// foreach(CDxMediaRecord O in dxSources)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "DeleteSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_DELETE_SOURCE_EX), Ex);
            }

            if (dxScenes != null)
            {
                dxScenes.Clear();
                dxScenes = null;
            }

            return true;

        }//	private bool DeleteSource(CDxSecondary dxScene)

        /// <summary>This method will transfer the source file to its appropriate location within the database hiearchy</summary>
        /// <param name="dxPrimary">The primary media object that owns the file</param>
        /// <param name="tmaxFile">The registration source file descriptor</param>
        /// <param name="bOverwrite">true to allow overwrite without error</param>
        /// <returns>true if successful</returns>
        private bool TransferSource(CDxPrimary dxPrimary, CTmaxSourceFile tmaxFile, bool bOverwrite)
        {
            string strTarget = "";
            string strFolder = "";
            bool bSuccessful = false;

            try
            {
                //	Don't bother if linked
                if (dxPrimary.Linked == true) return true;

                //	Don't bother if this is deposition video
                if (dxPrimary.MediaType == TmaxMediaTypes.Deposition) return true;

                //	Get the target path
                strTarget = GetFileSpec(dxPrimary, tmaxFile.Name);

                //	Make sure the target folder exists
                strFolder = System.IO.Path.GetDirectoryName(strTarget);
                if ((strFolder != null) && (strFolder.Length > 0))
                {
                    if (System.IO.Directory.Exists(strFolder) == false)
                        System.IO.Directory.CreateDirectory(strFolder);
                }

                //	Does the target file already exist?
                if (System.IO.File.Exists(strTarget) == true)
                {
                    //	Are the source and target the same?
                    if (String.Compare(strTarget, tmaxFile.Path, true) == 0)
                        return true;

                    //	Is overwrite permitted?
                    if (bOverwrite == true)
                    {
                        if (dxPrimary.MediaType != TmaxMediaTypes.Powerpoint) // In case ppt is imported, we will no delete the old file if it exists
                        {
                            try { System.IO.File.Delete(strTarget); }
                            catch { };
                        }
                    }
                    else
                    {
                        //	Assume this is the file they want
                        return true;
                    }

                }

                // This means that PPT is being imported and there already exists a file with the same name
                if (dxPrimary.MediaType == TmaxMediaTypes.Powerpoint && System.IO.File.Exists(strTarget))
                {
                    // Identifying if we have resolved media id conflict because then media id will be different from the file name
                    if (dxPrimary.MediaId != Path.GetFileNameWithoutExtension(dxPrimary.Filename))
                    {
                        dxPrimary.Filename = dxPrimary.MediaId + Path.GetExtension(strTarget); // Updating the file name.
                        strTarget = GetFileSpec(dxPrimary, dxPrimary.Filename); // Updating the location were the ppt will be copied
                    }
                }

                //	Make sure the source file exists
                if (System.IO.File.Exists(tmaxFile.Path) == false)
                {
                    FireError(this, "TransferSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_TRANSFER_SOURCE_NOT_FOUND, tmaxFile.Path));

                    //	If this is a deposition let them copy it later
                    if (dxPrimary.MediaType == TmaxMediaTypes.Deposition)
                        return true;
                    else
                        return false;
                }

                //	Are we moving the files
                if ((m_tmaxRegisterOptions != null) && (m_tmaxRegisterOptions.FileTransfer == RegFileTransfers.Move))
                {
                    bSuccessful = ShellTransfer(tmaxFile.Path, strTarget, false);
                }
                else
                {
                    bSuccessful = ShellTransfer(tmaxFile.Path, strTarget, true);
                }

                //	Make sure it is not read-only
                if (bSuccessful == true)
                    System.IO.File.SetAttributes(strTarget, System.IO.FileAttributes.Normal);

                return bSuccessful;
            }
            catch (System.Exception Ex)
            {
                FireError(this, "TransferSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_TRANSFER_SOURCE_EX, tmaxFile.Path, strTarget), Ex);
                return false;
            }

        }// private bool TransferSource(CDxPrimary dxPrimary, CTmaxSourceFile tmaxFile, bool bOverwrite)

        /// <summary>This method will get the source files for the specified MultiPage TIFF document</summary>
        /// <param name="dxPrimary">The primary media object associated with the Adobe PDF</param>
        /// <param name="tmaxSource">The source folder associated with the file</param>
        /// <returns>true if successful</returns>
        private bool GetMultiPageTIFFSource(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource)
        {
            CTmaxTiffSplitter tmaxSplitter = null;
            string strFileSpec = "";
            string strTarget = "";
            bool bSuccessful = false;

            Debug.Assert(tmaxSource != null);
            Debug.Assert(tmaxSource.Files != null);
            Debug.Assert(tmaxSource.Files.Count == 1);

            if (tmaxSource.Files == null) return false;
            if (tmaxSource.Files.Count != 1) return false; // Just playing it safe

            //	Get the folder where the pages will be stored
            strTarget = GetFolderSpec(dxPrimary, true);
            if (strTarget.EndsWith("\\") == false)
                strTarget += "\\";

            //	Get the path to the TIFF file
            strFileSpec = tmaxSource.Files[0].Path;
            if (strFileSpec.Length == 0) return false;

            //	Is the splitter available?
            tmaxSplitter = new CTmaxTiffSplitter();
            SetHandlers(tmaxSplitter.EventSource);
            tmaxSplitter.SavedPage += new CTmaxTiffSplitter.SavedPageHandler(this.OnSplitterSavePage);
            tmaxSplitter.TmxView = this.TmxView;

            if (tmaxSplitter.Initialize(tmaxSource.Files[0].Path, strTarget, !m_tmaxRegisterOptions.UseSherrodSplitter) == false)
            {
                tmaxSplitter = null;
                return false;
            }

            //	Update the progress form
            SetRegisterProgress("Splitting " + System.IO.Path.GetFileName(strFileSpec) + " pages ...");

            try
            {
                //	Save the pages to the target folder
                if (tmaxSplitter.Split() == true)
                {
                    //	Reset the source folder
                    tmaxSource.Files.Clear();
                    tmaxSource.Initialize(strTarget);
                    foreach (CTmaxSourceFile O in tmaxSplitter.PageFiles)
                        tmaxSource.Files.Add(O);

                    bSuccessful = true;
                }
                else
                {
                    //	Delete the target since no pages were exported
                    try { System.IO.Directory.Delete(strTarget, true); }
                    catch { }

                    return false;
                }

            }
            catch
            {
            }

            return bSuccessful;

        }// private bool GetMultiPageTIFFSource(CDxPrimary dxPrimary, CTmaxSourceFolder tmaxSource)

        /// <summary>Handles events fired by TmaxTiffSplitter when it saves a page</summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="strSourceFileSpec">The path to the source file</param>
        /// <param name="strPageFileSpec">The path to the page file</param>
        /// <param name="iPageNumber">The number of the saved page</param>
        public void OnSplitterSavePage(object sender, string strSourceFileSpec, string strPageFileSpec, int iPageNumber)
        {
            try
            {
                //	Is a registration in progress?
                if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                {
                    string strMsg = String.Format("Saved : Pg {0} of {1} to {2}",
                        iPageNumber,
                        System.IO.Path.GetFileName(strSourceFileSpec),
                        System.IO.Path.GetFileName(strPageFileSpec));
                    SetRegisterProgress(strMsg);
                }
            }
            catch
            {
            }

        }// public void OnSplitterSavePage(object sender, int iPageNumber, string strFileSpec)

        /// <summary>This method locate the TrialMax media type object associated with the specified registration type</summary>
        /// <param name="eRegSourceType">The enumerated registration type</param>
        /// <returns>The associated media type object</returns>
        private CTmaxMediaType GetMediaType(FTI.Shared.Trialmax.RegSourceTypes eRegSourceType)
        {
            CTmaxMediaType tmaxMediaType = null;
            CTmaxSourceType tmaxSourceType = null;

            Debug.Assert(m_tmaxMediaTypes != null);
            Debug.Assert(m_tmaxSourceTypes != null);

            if ((m_tmaxSourceTypes != null) && (m_tmaxMediaTypes != null))
            {
                if ((tmaxSourceType = m_tmaxSourceTypes.Find(eRegSourceType)) != null)
                {
                    if ((tmaxMediaType = m_tmaxMediaTypes.Find(tmaxSourceType.MediaType)) == null)
                    {
                        tmaxMediaType = m_tmaxMediaTypes.Find(TmaxMediaTypes.Document);
                    }
                }

            }// if((m_tmaxSourceTypes != null) && (m_tmaxMediaTypes != null))

            return tmaxMediaType;

        }// private CTmaxMediaType GetMediaType(FTI.Shared.Trialmax.RegSourceTypes eRegSourceType)

        /// <summary>This method locate the TrialMax media type object associated with the specified primary media type identifier</summary>
        /// <param name="eRegSourceType">The enumerated media type</param>
        /// <returns>The associated media type object</returns>
        private CTmaxMediaType GetMediaType(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)
        {
            CTmaxMediaType tmaxMediaType = null;

            Debug.Assert(m_tmaxMediaTypes != null);

            if (m_tmaxMediaTypes != null)
            {
                if ((tmaxMediaType = m_tmaxMediaTypes.Find(eMediaType)) == null)
                {
                    tmaxMediaType = m_tmaxMediaTypes.Find(TmaxMediaTypes.Document);
                }
            }
            return tmaxMediaType;

        }// private CTmaxMediaType GetMediaType(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)

        /// <summary>This method locate the TrialMax media type object associated with the specified filename</summary>
        /// <param name="strFilename">The name of the file to check for</param>
        /// <returns>The associated media type object</returns>
        private CTmaxMediaType GetMediaType(string strFilename)
        {
            CTmaxMediaType tmaxMediaType = null;
            CTmaxSourceType tmaxSourceType = null;

            Debug.Assert(m_tmaxMediaTypes != null);
            Debug.Assert(m_tmaxSourceTypes != null);

            if (m_tmaxSourceTypes != null)
                tmaxSourceType = m_tmaxSourceTypes.FindFromFilename(strFilename);

            if (tmaxSourceType != null)
                tmaxMediaType = GetMediaType(tmaxSourceType.RegSourceType);

            return tmaxMediaType;

        }// private CTmaxMediaType GetMediaType(string strFilename)

        /// <summary>This method will prompt the user for source files to be added to the primary record</summary>
        /// <param name="dxPrimary">The primary record being added to</param>
        /// <returns>The populated source folder</returns>
        private CTmaxSourceFolder GetSource(CDxPrimary dxPrimary)
        {
            OpenFileDialog dlgFiles = new System.Windows.Forms.OpenFileDialog();
            CTmaxSourceType tmaxSourceType = null;
            CTmaxSourceFolder tmaxSource = null;
            string strFolder = "";

            //	Verify the source folder still exists if this is a linked primary
            if (dxPrimary.Linked == true)
            {
                // Get the folder where the source media is stored
                strFolder = GetAliasedPath(dxPrimary);
                if (strFolder.Length == 0) return null;

                if (System.IO.Directory.Exists(strFolder) == false)
                {
                    FireError(this, "GetSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_SOURCE_FOLDER_NOT_FOUND, strFolder));
                    return null;
                }

            }
            else
            {
                strFolder = dxPrimary.RegisterPath;
            }

            //	Get the source type associated with this primary record
            switch (dxPrimary.MediaType)
            {
                case TmaxMediaTypes.Document:

                    tmaxSourceType = m_tmaxSourceTypes.Find(RegSourceTypes.Document);
                    break;

                case TmaxMediaTypes.Recording:

                    tmaxSourceType = m_tmaxSourceTypes.Find(RegSourceTypes.Recording);
                    break;

                case TmaxMediaTypes.Deposition:

                    tmaxSourceType = m_tmaxSourceTypes.Find(RegSourceTypes.Deposition);
                    break;

                case TmaxMediaTypes.Powerpoint:

                    tmaxSourceType = m_tmaxSourceTypes.Find(RegSourceTypes.Powerpoint);
                    break;

            }// switch(dxPrimary.MediaType)

            //	Initialize the file selection dialog
            dlgFiles.CheckFileExists = true;
            dlgFiles.CheckPathExists = true;
            dlgFiles.Multiselect = true;
            dlgFiles.InitialDirectory = strFolder;
            dlgFiles.Title = "Select Files";

            if (tmaxSourceType != null)
            {
                dlgFiles.Filter = (tmaxSourceType.GetFileSelectionString() + "|" + tmaxSourceType.GetFileFilterString() + "|All Files (*.*)|*.*");
            }
            else
            {
                dlgFiles.Filter = "All Files (*.*)|*.*";
            }

            while (true)
            {
                //	Open the dialog box
                if (dlgFiles.ShowDialog() != DialogResult.OK)
                    return null;

                //	Allocate and initialize the source folder specification
                tmaxSource = new CTmaxSourceFolder(dlgFiles.FileNames);
                if ((tmaxSource.Files == null) || (tmaxSource.Files.Count == 0))
                    return null;

                //	Is this a linked primary
                if (dxPrimary.Linked == true)
                {
                    //	Did the user confine their selections to the source folder
                    if (String.Compare(tmaxSource.Path, strFolder) != 0)
                    {
                        FireError(this, "GetSource", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_SOURCE_LINK_FOLDER, dxPrimary.MediaId, strFolder));

                        //	Reset the dialog
                        dlgFiles.Reset();
                        dlgFiles.InitialDirectory = strFolder;

                        //	Try again
                        continue;
                    }

                }

                break;
            }

            return tmaxSource;

        }// private CTmaxSourceFolder GetSource(CDxPrimary dxPrimary)

        /// <summary>This method is called to create and initialize a new instance of the progress form</summary>
        /// <param name="strTitle">The title to be displayed in the form</param>
        /// <param name="strDescription">The description of the operation displayed in the form</param>
        /// <param name="lMaximum">Maximum value for the operation</param>
        private void CreateRegisterProgress(string strTitle, string strDescription, long lMaximum)
        {
            //	Clear the cancellation flag
            m_bRegisterCancelled = false;

            try
            {
                //	Make sure the previous instance is disposed
                if (m_cfRegisterProgress != null)
                {
                    m_cfRegisterProgress.Dispose();
                    m_cfRegisterProgress = null;
                }

                //	Create a new instance
                m_cfRegisterProgress = new FTI.Trialmax.Forms.CFRegProgress(strTitle);

                //	Set the properties
                m_cfRegisterProgress.Description = strDescription;
                m_cfRegisterProgress.Maximum = lMaximum;

                if ((m_tmaxRegisterOptions != null) && (m_tmaxRegisterOptions.GetFlag(RegFlags.PauseOnError) == true))
                {
                    m_cfRegisterProgress.PauseOnError = true;
                }
                else
                {
                    m_cfRegisterProgress.PauseOnError = false;
                }
            }
            catch
            {
                m_cfRegisterProgress = null;
            }

        }//	private void CreateRegisterProgress(string strTitle, string strDescription, long lMaximum)

        /// <summary>This method is called to create and initialize a new instance of the progress form</summary>
        /// <param name="strTitle">The title to be displayed in the form</param>
        /// <param name="strDescription">The description of the operation displayed in the form</param>
        /// <param name="lMaximum">Maximum value for the operation</param>
        private void CreateValidateProgress()
        {
            //	Clear the cancellation flag
            m_bValidateCancelled = false;

            try
            {
                //	Make sure the previous instance is disposed
                if (m_cfValidateProgress != null)
                {
                    m_cfValidateProgress.Dispose();
                    m_cfValidateProgress = null;
                }

                //	Create a new instance
                m_cfValidateProgress = new FTI.Trialmax.Forms.CFValidateProgress();

                m_cfValidateProgress.CaseFolder = Folder;
            }
            catch
            {
                m_cfValidateProgress = null;
            }

        }//	private void CreateValidateProgress()

        /// <summary>This method will get the text when the folder is used to initialize fields on registration</summary>
        /// <param name="tmaxSource">The source folder used to initialize the object</param>
        /// <returns>The source folder text</returns>
        private string GetAssignmentText(CTmaxSourceFolder tmaxSource, bool bMediaId)
        {
            string strText = "";

            Debug.Assert(tmaxSource != null);

            //	What type of media is being registered?
            switch (tmaxSource.SourceType)
            {
                case RegSourceTypes.Deposition:

                    //	Use the name of the XML deposition instead of the folder name
                    if (tmaxSource.UserData != null)
                        strText = System.IO.Path.GetFileNameWithoutExtension(((CXmlDeposition)tmaxSource.UserData).FileSpec);

                    break;

                case RegSourceTypes.Powerpoint:

                    //	Use the name of the PowerPoint file instead of the folder name
                    if (tmaxSource.TargetName.Length > 0)
                        strText = tmaxSource.TargetName;
                    else if ((tmaxSource.Files != null) && (tmaxSource.Files.Count == 1))
                        strText = System.IO.Path.GetFileNameWithoutExtension(tmaxSource.Files[0].Path);

                    break;

                case RegSourceTypes.Adobe:
                case RegSourceTypes.MultiPageTIFF:

                    //	By the time this gets called we have already exported the PDF so
                    //	we use the folder name instead of the filename
                    strText = tmaxSource.Name;

                    break;

            }// switch(tmaxSource.SourceType)

            //	Assign the default value if not already assigned a value
            if (strText.Length == 0)
            {
                if (tmaxSource.TargetName.Length > 0)
                    strText = tmaxSource.TargetName;
                else
                    strText = (tmaxSource.Name + tmaxSource.Suffix);
            }

            //	Is this to be used for a MediaId?
            if ((bMediaId == true) && (strText.Length > 0))
                return AdjustForMediaId(strText, tmaxSource.MediaType);
            else
                return strText;

        }// private string GetAssignmentText(CTmaxSourceFolder tmaxSource, bool bMediaId)

        /// <summary>This method will adjust the specified text using the Foreign barcode registration options</summary>
        /// <param name="strText">The text to be adjusted</param>
        /// <returns>The text to be assigned as a foreign barcode</returns>
        private string GetAdjustedForeignBarcode(string strText)
        {
            string strAdjusted = strText;
            int iIndex = -1;

            //	Should we truncate at a space?
            if (m_tmaxRegisterOptions.ForeignBarcodeAdjustments.GetSelected(RegForeignBarcodeAdjustments.TruncateOnSpace) == true)
            {
                if ((iIndex = strAdjusted.IndexOf(' ')) > 0)
                {
                    strAdjusted = strAdjusted.Substring(0, iIndex);
                }

            }

            //	Should we truncate at a hyphen?
            if (m_tmaxRegisterOptions.ForeignBarcodeAdjustments.GetSelected(RegForeignBarcodeAdjustments.TruncateOnHyphen) == true)
            {
                if ((iIndex = strAdjusted.IndexOf('-')) > 0)
                {
                    strAdjusted = strAdjusted.Substring(0, iIndex);
                }

            }

            //	Are we stripping all instances of zero padding?
            if (m_tmaxRegisterOptions.ForeignBarcodeAdjustments.GetSelected(RegForeignBarcodeAdjustments.StripZerosAll) == true)
            {
                strAdjusted = CTmaxToolbox.StripZeroPadding(strAdjusted, true);
            }

            //	Are we stripping the first numeric block?
            else if (m_tmaxRegisterOptions.ForeignBarcodeAdjustments.GetSelected(RegForeignBarcodeAdjustments.StripZerosFirst) == true)
            {
                strAdjusted = CTmaxToolbox.StripZeroPadding(strAdjusted, false);
            }

            return strAdjusted;

        }// private string GetAdjustedForeignBarcode(string strText)

        /// <summary>This method is called to update the progress form's status message</summary>
        /// <param name="strStatus">The new status message</param>
        private void SetRegisterProgress(string strStatus)
        {
            try
            {
                if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                {
                    m_cfRegisterProgress.Status = strStatus;
                }
            }
            catch
            {
            }

        }// private void SetRegisterProgress(string strStatus)

        /// <summary>This method is called to update the validation progress form</summary>
        /// <param name="strStatus">The new status message</param>
        private void SetValidateStatus(string strStatus)
        {
            try
            {
                if ((m_cfValidateProgress != null) && (m_cfValidateProgress.IsDisposed == false))
                    m_cfValidateProgress.Status = strStatus;
            }
            catch
            {
            }

        }// private void SetValidateStatus(string strStatus)

        /// <summary>This method is called to add a validation error to the progress form</summary>
        /// <param name="strBarcode">The barcode of the associated record</param>
        /// <param name="strMessage">The error message to be added</param>
        /// <param name="bCritical">True if the error is critical</param>
        private void AddValidationError(string strBarcode, string strMessage, bool bCritical)
        {
            try
            {
                if ((m_cfValidateProgress != null) && (m_cfValidateProgress.IsDisposed == false))
                {
                    m_cfValidateProgress.AddError(strBarcode, strMessage, bCritical);
                }
            }
            catch
            {
            }

        }// private void AddValidationError(string strBarcode, string strMessage, bool bCritical)

        /// <summary>This method is called to determine if the validate operation has been cancelled</summary>
        /// <returns>True if cancelled</returns>
        private bool GetValidateCancelled()
        {
            try
            {
                if ((m_cfValidateProgress != null) && (m_cfValidateProgress.IsDisposed == false))
                    return m_cfValidateProgress.Cancelled;
                else
                    return m_bValidateCancelled;
            }
            catch
            {
                return false;
            }

        }// private bool GetValidateCancelled()

        /// <summary>This method is called to increment the progress form's completed value</summary>
        private void StepProgressCompleted()
        {
            try
            {
                if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                    m_cfRegisterProgress.Completed = m_cfRegisterProgress.Completed + 1;
            }
            catch
            {
            }

        }// private void StepProgressCompleted()

        /// <summary>This method is fired when TmaxPDFManager to update the progress form's completed pages value</summary>
        private void UpdateProgressBar(object sender, EventArgs e)
        {
            try
            {
                if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                    m_cfRegisterProgress.CompletedPages = m_cfRegisterProgress.CompletedPages + 1;
            }
            catch
            {
            }

        }

        /// <summary>This method will get the collection of scenes referenced by the specified record</summary> 
        /// <param name="dxRecord">The source record</param>
        /// <param name="bIgnoreDesignations">True to ignore designations and clips</param>
        /// <returns>The collection of scenes sourced by the specified record</returns>
        private CDxScenes GetScenes(CDxMediaRecord dxSource, bool bIgnoreDesignations)
        {
            CDxScenes dxScenes = null;

            Debug.Assert(dxSource != null);

            switch (dxSource.MediaType)
            {
                case TmaxMediaTypes.Link:
                case TmaxMediaTypes.Scene:
                case TmaxMediaTypes.Script:

                    return null;

                case TmaxMediaTypes.Segment:

                    //	Deposition segments cant be the source for a scene
                    if (((CDxSecondary)dxSource).Primary.MediaType == TmaxMediaTypes.Deposition)
                        return null;
                    break;

                case TmaxMediaTypes.Designation:
                case TmaxMediaTypes.Clip:

                    //	Are we ignoring designations and clips
                    if (bIgnoreDesignations == true)
                        return null;

                    //	OK to check for designations and clips
                    break;

                //	All other media can be the source for a scene
                case TmaxMediaTypes.Document:
                case TmaxMediaTypes.Page:
                case TmaxMediaTypes.Treatment:
                case TmaxMediaTypes.Powerpoint:
                case TmaxMediaTypes.Slide:
                case TmaxMediaTypes.Recording:
                case TmaxMediaTypes.Deposition:
                default:
                    break;
            }

            try
            {
                //	Allocate the new collection
                dxScenes = new CDxScenes(this);

                //	Set the source record
                dxScenes.Source = dxSource;

                //	Get the scenes sourced by this record and/or its children
                dxScenes.Fill();
            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetScenes", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_SCENES_EX, dxSource.MediaType, GetUniqueId(dxSource)), Ex);
            }

            if ((dxScenes != null) && (dxScenes.Count > 0))
                return dxScenes;
            else
                return null;

        }//	private CDxScenes GetScenes(CDxMediaRecord dxSource, bool bIgnoreDesignations)

        /// <summary>This method will get the collection of scenes referenced by the specified record</summary> 
        /// <param name="dxRecord">The source record</param>
        /// <returns>The collection of scenes sourced by the specified record</returns>
        private CDxLinks GetLinks(CDxMediaRecord dxSource)
        {
            CDxLinks dxLinks = null;

            Debug.Assert(dxSource != null);

            switch (dxSource.MediaType)
            {
                //	Only these types can be the source for a link
                case TmaxMediaTypes.Document:
                case TmaxMediaTypes.Page:
                case TmaxMediaTypes.Treatment:
                case TmaxMediaTypes.Powerpoint:
                case TmaxMediaTypes.Slide:

                    break;

                case TmaxMediaTypes.Link:
                case TmaxMediaTypes.Scene:
                case TmaxMediaTypes.Script:
                case TmaxMediaTypes.Segment:
                case TmaxMediaTypes.Recording:
                case TmaxMediaTypes.Clip:
                case TmaxMediaTypes.Deposition:
                case TmaxMediaTypes.Designation:
                default:

                    return null;
            }

            try
            {
                //	Allocate the new collection
                dxLinks = new CDxLinks(this);

                //	Set the source record
                dxLinks.Source = dxSource;

                //	Get the scenes sourced by this record and/or its children
                dxLinks.Fill();
            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetLinks", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_LINKS_EX, dxSource.MediaType, GetUniqueId(dxSource)), Ex);
            }

            if ((dxLinks != null) && (dxLinks.Count > 0))
                return dxLinks;
            else
                return null;

        }//	private CDxLinks GetLinks(CDxMediaRecord dxSource)

        /// <summary>This method is called to get the duration of the specified recording</summary>
        /// <param name="strFilename">The recording file</param>
        /// <returns>The duration in seconds</returns>
        private double GetDuration(string strFilename)
        {
            double dDuration = -1.0;

            try
            {
                if (m_tmxMovie != null)
                    dDuration = m_tmxMovie.GetDuration(strFilename);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetDuration", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_DURATION_EX, strFilename), Ex);
            }

            if (dDuration > 0)
                return dDuration;
            else
                return 0.0;	//	This allows normal playback

        }// private double GetDuration(string strFilename)

        /// <summary>This method uses the shell to transfer or move a file</summary>
        /// <param name="strSource">Fully qualified path to the source file</param>
        /// <param name="strTarget">Fully qualified path to the target file</param>
        /// <param name="bCopy">True to copy, false to move</param>
        /// <returns>true if successful</returns>
        private bool ShellTransfer(string strSource, string strTarget, bool bCopy)
        {
            FTI.Shared.Win32.SHFILEOPSTRUCT opStruct = new SHFILEOPSTRUCT();
            int iError = 0;
            bool bSuccessful = false;
            string strFolder = "";

            opStruct.wFunc = bCopy == true ? Shell.FO_COPY : Shell.FO_MOVE;
            opStruct.pFrom = strSource + "\0";
            opStruct.pTo = strTarget + "\0";
            opStruct.fFlags = (Shell.FOF_SILENT | Shell.FOF_NOCONFIRMATION | Shell.FOF_NOCONFIRMMKDIR);

            try
            {
                //	Make sure the target folder exists
                //
                //	NOTE:	If we don't do this, and we use the FOF_SILENT flag, the call
                //			to SHFileOperation() returns 1223 (user cancelled) - go figure....
                strFolder = System.IO.Path.GetDirectoryName(strTarget);
                if ((strFolder.Length > 0) && (System.IO.Directory.Exists(strFolder) == false))
                {
                    try { System.IO.Directory.CreateDirectory(strFolder); }
                    catch { }
                }

                //	Perform the operation
                if ((iError = Shell.SHFileOperation(ref opStruct)) != 0)
                {
                    //	Did the user abort?
                    //
                    //	NOTE:	ERROR_CANCELED is defined in Winerror.h as 1223, fAnyOperationsAborted not always non-zero on cancel
                    if ((iError == 1223) || (opStruct.fAnyOperationsAborted != 0))
                    {
                        m_bRegisterCancelled = true;
                        if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                            m_cfRegisterProgress.Cancel();
                    }

                }
                else
                {
                    bSuccessful = true;
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "ShellTransfer", this.ExBuilder.Message(ERROR_CASE_DATABASE_SHELL_TRANSFER_EX, strSource, strTarget), Ex);
            }

            return bSuccessful;

        }// private bool ShellTransfer(string strSource, string strTarget, bool bCopy)

        /// <summary>This method will initialize the array of case root folders</summary>
        private void SetCaseFolders()
        {
            CTmaxSourceFolder tmaxFolder = null;
            bool bPrompt = true;

            //	Get the array of enumerated folders
            Array aValues = Enum.GetValues(typeof(TmaxCaseFolders));

            //	Clear the existing folders
            m_aCaseFolders.Clear();

            //	Construct the path to each folder
            foreach (TmaxCaseFolders e in aValues)
            {
                //	Allocate a new folder and add it to the list
                tmaxFolder = new CTmaxSourceFolder();
                tmaxFolder.UserData = e;

                m_aCaseFolders.Add(tmaxFolder);

                //	Set the path to the folder
                if (SetCasePath(e, tmaxFolder, null, bPrompt) == false)
                    bPrompt = false;

            }// foreach(CTmaxCaseFolder e in aValues)

        }// private void SetCaseFolders()

        /// <summary>This method will set the path for the specified folder</summary>
        /// <param name="eFolder">The enumerated media folder type</param>
        /// <param name="tmaxFolder">The variable in which to store the path</param>
        ///	<param name="strDefault">The default value to use for the folder if not overridden by user</param>
        /// <param name="bPrompt">True to prompt the user on an error</param>
        /// <returns>true if successful</returns>
        private bool SetCasePath(TmaxCaseFolders eFolder, CTmaxSourceFolder tmaxFolder, string strDefault, bool bPrompt)
        {
            string strPath = "";
            bool bOverride = true;

            //	Get the user defined override if it exists
            if (m_tmaxCaseOptions != null)
            {
                strPath = m_tmaxCaseOptions.GetCasePath(eFolder);
            }

            //	Use the default if no override specified by the user
            if (strPath.Length == 0)
            {
                strPath = m_strFolder;
                if (strPath.EndsWith("\\") == false)
                    strPath += "\\";

                //	Did the caller provide a default?
                if ((strDefault != null) && (strDefault.Length > 0))
                {
                    strPath += strDefault;
                    if (strPath.EndsWith("\\") == false)
                        strPath += "\\";
                }
                else
                {
                    if (eFolder == TmaxCaseFolders.Clips)
                    {
                        if (m_strDefaultClipsFolder.Length > 0)
                            strPath += (m_strDefaultClipsFolder + "\\");
                        else
                            strPath += ("_tmax_" + eFolder.ToString() + "\\");
                    }
                    else
                    {
                        strPath += ("_tmax_" + eFolder.ToString() + "\\");
                    }

                }
                bOverride = false; // Using default

            }
            else
            {
                if (strPath.EndsWith("\\") == false)
                    strPath += "\\";
            }

            //	Update the caller's folder 
            tmaxFolder.Initialize(strPath.ToLower());

            //	Use the Anchor flag to keep track of whether this has
            //	been overridden by the user
            tmaxFolder.Anchor = bOverride;

            //	Make sure the folder exists
            return CreateCaseFolder(tmaxFolder, bPrompt);

        }// private bool SetCasePath(TmaxCaseFolders eFolder, CTmaxSourceFolder tmaxFolder, string strDefault, bool bPrompt)

        /// <summary>This method will set the path for the specified folder using the requested default</summary>
        /// <param name="eFolder">The enumerated media folder type</param>
        /// <param name="strDefault">The default value if not overridden by the user</param>
        /// <param name="bPrompt">True to prompt the user on an error</param>
        /// <returns>true if successful</returns>
        private bool SetCasePath(TmaxCaseFolders eFolder, string strDefault, bool bPrompt)
        {
            CTmaxSourceFolder tmaxFolder = null;
            bool bSuccessful = false;

            //	Get the requested folder
            if ((tmaxFolder = m_aCaseFolders[(int)eFolder]) != null)
            {
                bSuccessful = SetCasePath(eFolder, tmaxFolder, strDefault, bPrompt);
            }

            return bSuccessful;

        }// private bool SetCasePath(TmaxCaseFolders eFolder, string strDefault, bool bPrompt)

        /// <summary>This method is called to set the relative path to the source files for the primary record</summary>
        /// <param name="tmaxSource">The source folder used to register the object</param>
        /// <param name="dxPrimary">The primary media object being initialized</param>
        /// <returns>True if successful</returns>
        private bool SetRelativePath(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)
        {
            CTmaxSourceFolder tmaxCaseFolder = null;
            string strRelative = "";
            string strCasePath = "";
            string strSourcePath = "";
            string strFilename = "";
            int iIndex = 0;

            Debug.Assert(tmaxSource != null);
            if (tmaxSource == null) return false;

            try
            {
                //	Get the default case path (convert to lower case and strip the drive)
                if ((tmaxCaseFolder = GetCaseFolder(dxPrimary.MediaType)) == null)
                    return false;
                strCasePath = tmaxCaseFolder.GetRelativePath().ToLower();
                if (strCasePath.EndsWith("\\") == false)
                    strCasePath += "\\";

                //	Strip the drive from the source path and convert to lower
                strSourcePath = tmaxSource.GetRelativePath().ToLower();
                if (strSourcePath.EndsWith("\\") == false)
                    strSourcePath += "\\";

                //	Does the source path start with the media path?
                if ((iIndex = strSourcePath.IndexOf(strCasePath)) == 0)
                {
                    strRelative = strSourcePath.Substring(strCasePath.Length);
                }
                else
                {
                    //	Use the path relative to the top level registration folder
                    tmaxSource.GetRelativePath(m_RegSourceParent.Path, ref strRelative);

                    //	Should the parent folder be included in the relative path?
                    if (m_RegSourceParent.Anchor == true)
                    {
                        //	Back up to include the name of the parent folder if it's not a drive
                        if (m_RegSourceFolder.GetRelativePath().Length > 0)
                            strRelative = (m_RegSourceParent.Name + "\\" + strRelative);
                    }

                }

                dxPrimary.RelativePath = strRelative;

                //	Should we create a subfolder based on the filename?
                if ((tmaxSource.SplitOnRegistration == true) ||
                   (tmaxSource.SourceType == RegSourceTypes.Adobe) ||
                   (tmaxSource.SourceType == RegSourceTypes.Recording) ||
                   (tmaxSource.SourceType == RegSourceTypes.MultiPageTIFF))
                {
                    //	There should be one file in this folder
                    if (tmaxSource.Files.Count == 1)
                    {
                        //	Append the filename to create a unique subfolder
                        strFilename = System.IO.Path.GetFileNameWithoutExtension(tmaxSource.Files[0].Path);
                        strFilename = strFilename.Trim();

                        if (strFilename.Length > 0)
                        {
                            if (dxPrimary.RelativePath.Length > 0)
                            {
                                if (dxPrimary.RelativePath.EndsWith("\\") == false)
                                    dxPrimary.RelativePath += "\\";
                            }

                            dxPrimary.RelativePath += strFilename;

                        }// if(strFilename.Length > 0)

                    }// if(tmaxSource.Files.Count == 1)

                }// if(tmaxSource.SplitOnRegistration == true)

                return true;
            }
            catch (System.Exception Ex)
            {
                FireError(this, "SetRelativePath", this.ExBuilder.Message(ERROR_CASE_DATABASE_SET_RELATIVE_PATH_EX, tmaxSource.Path), Ex);
                return false;
            }

        }// private bool SetRelativePath(CTmaxSourceFolder tmaxSource, CDxPrimary dxPrimary)

        /// <summary>This methods gets the folder where files of the specified media type are stored</summary>
        /// <param name="eMediaType">The TrialMax media type identifier</param>
        /// <returns>The case folder for the specified media type</returns>
        private CTmaxSourceFolder GetCaseFolder(TmaxMediaTypes eMediaType)
        {
            Debug.Assert(m_aCaseFolders != null);
            Debug.Assert(m_aCaseFolders.Count > 0);
            if (m_aCaseFolders == null) return null;
            if (m_aCaseFolders.Count == 0) return null;

            try
            {
                //	What type of media?
                switch (eMediaType)
                {
                    case TmaxMediaTypes.Document:
                    case TmaxMediaTypes.Page:

                        return m_aCaseFolders[(int)TmaxCaseFolders.Documents];

                    case TmaxMediaTypes.Treatment:

                        return m_aCaseFolders[(int)TmaxCaseFolders.Treatments];

                    case TmaxMediaTypes.Powerpoint:

                        return m_aCaseFolders[(int)TmaxCaseFolders.PowerPoints];

                    case TmaxMediaTypes.Recording:

                        return m_aCaseFolders[(int)TmaxCaseFolders.Recordings];

                    case TmaxMediaTypes.Deposition:

                        return m_aCaseFolders[(int)TmaxCaseFolders.Videos];

                    case TmaxMediaTypes.Designation:

                        //	This change was made in version 6.0.1
                        //
                        //	Designations and clips used to all be stored in one
                        //	folder. Designations have been moved to reside with 
                        //	their parent transcript
                        return m_aCaseFolders[(int)TmaxCaseFolders.Transcripts];

                    case TmaxMediaTypes.Clip:

                        return m_aCaseFolders[(int)TmaxCaseFolders.Clips];

                    case TmaxMediaTypes.Slide:

                        return m_aCaseFolders[(int)TmaxCaseFolders.Slides];

                    default:

                        Debug.Assert(false);
                        return null;

                }// switch(eMediaType)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetCaseFolder", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_CASE_FOLDER_EX, eMediaType.ToString()), Ex);
                return null;
            }

        }// private CTmaxSourceFolder GetCaseFolder(TmaxMediaTypes eMediaType)

        /// <summary>This methods gets the root case folder for the specified record</summary>
        /// <param name="dxRecord">The desired record</param>
        /// <returns>The case folder for the specified record</returns>
        private CTmaxSourceFolder GetCaseFolder(CDxMediaRecord dxRecord)
        {
            switch (dxRecord.MediaType)
            {
                case TmaxMediaTypes.Segment:

                    //	We have to use the primary type because segments
                    //	can belong to depositions or recordings
                    return GetCaseFolder(((CDxSecondary)dxRecord).Primary);

                default:

                    return GetCaseFolder(dxRecord.MediaType);

            }// switch(dxRecord.MediaType)

        }// private CTmaxSourceFolder GetCaseFolder(CDxMediaRecord dxRecord)

        /// <summary>This methods will create the folder for the specified record</summary>
        /// <param name="dxRecord">The desired record</param>
        /// <param name="bIgnoreLinked">True to ignore the record's linked state</param>
        /// <param name="bPrompt">True to prompt the user if the case root does not exist</param>
        /// <returns>true if successful</returns>
        private bool CreateFolder(CDxMediaRecord dxRecord, bool bIgnoreLinked, bool bPrompt)
        {
            CTmaxSourceFolder tmaxCaseFolder = null;
            string strFolderSpec = "";
            bool bLinked = false;

            //	What type of record is this?
            switch (dxRecord.GetMediaLevel())
            {
                case TmaxMediaLevels.Primary:

                    strFolderSpec = GetFolderSpec(((CDxPrimary)dxRecord), bIgnoreLinked);

                    if (((CDxPrimary)dxRecord).Linked == true)
                        bLinked = true;
                    break;

                case TmaxMediaLevels.Secondary:

                    strFolderSpec = GetFolderSpec(((CDxSecondary)dxRecord), bIgnoreLinked);

                    if (((CDxSecondary)dxRecord).Linked == true)
                        bLinked = true;
                    break;

                case TmaxMediaLevels.Tertiary:

                    strFolderSpec = GetFolderSpec(((CDxTertiary)dxRecord));
                    break;

                default:

                    break;

            }// switch(dxRecord.GetMediaLevel())

            if (strFolderSpec.Length == 0) return false;

            //	Nothing to do if the record is linked
            if ((bIgnoreLinked == false) && (bLinked == true))
                return true;

            //	Make sure the case root folder for this record exists
            if ((tmaxCaseFolder = GetCaseFolder(dxRecord)) == null)
                return false;

            //	Make sure the case folder exists
            if (CreateCaseFolder(tmaxCaseFolder, bPrompt) == false)
                return false;

            //	Create the folder for this record
            if (System.IO.Directory.Exists(strFolderSpec) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(strFolderSpec);
                }
                catch
                {
                    FireError(this, "CreateFolder", this.ExBuilder.Message(ERROR_CASE_DATABASE_CREATE_FOLDER_FAILED, dxRecord.GetBarcode(false), strFolderSpec));
                    return false;
                }

            }// if(System.IO.Directory.Exists(strFolderSpec) == false)

            return true;

        }// private bool CreateFolder(CDxMediaRecord dxRecord, bool bIgnoreLinked, bool bPrompt)

        /// <summary>This methods will create the specified folder for media storage</summary>
        /// <param name="eMediaType">The type of media being stored in the folder</param>
        /// <param name="strFolderSpec">The fully qualified path for the folder</param>
        /// <param name="bPrompt">True to prompt the user if the case root does not exist</param>
        /// <returns>true if successful</returns>
        private bool CreateFolder(TmaxMediaTypes eMediaType, string strFolderSpec, bool bPrompt)
        {
            CTmaxSourceFolder tmaxCaseFolder = null;

            //	Get the case folder for this media type
            if ((tmaxCaseFolder = GetCaseFolder(eMediaType)) == null)
                return false;

            //	Make sure the case folder exists
            if (CreateCaseFolder(tmaxCaseFolder, bPrompt) == false)
                return false;

            //	Create the requested folder
            if (System.IO.Directory.Exists(strFolderSpec) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(strFolderSpec);
                }
                catch
                {
                    return false;
                }

            }// if(System.IO.Directory.Exists(strFolderSpec) == false)

            return true;

        }// private bool CreateFolder(TmaxMediaTypes eMediaType, string strFolderSpec, bool bPrompt)

        /// <summary>This methods will create the specified root case folder</summary>
        /// <param name="eMediaType">The media type associated with the desired case folder</param>
        /// <param name="bPrompt">True to prompt the user if not successful</param>
        /// <returns>true if successful</returns>
        private bool CreateCaseFolder(TmaxMediaTypes eMediaType, bool bPrompt)
        {
            CTmaxSourceFolder tmaxCaseFolder = GetCaseFolder(eMediaType);

            if (tmaxCaseFolder != null)
                return CreateCaseFolder(tmaxCaseFolder, bPrompt);
            else
                return false;

        }// private bool CreateCaseFolder(TmaxMediaTypes eMediaType, bool bPrompt)

        /// <summary>This methods will create the specified root case folder</summary>
        /// <param name="tmaxCaseFolder">The case folder to be created</param>
        /// <param name="bPrompt">True to prompt the user if not successful</param>
        /// <returns>true if successful</returns>
        private bool CreateCaseFolder(CTmaxSourceFolder tmaxCaseFolder, bool bPrompt)
        {
            Debug.Assert(tmaxCaseFolder != null, "CreateCaseFolder: NULL tmaxCaseFolder");
            if (tmaxCaseFolder == null) return false;

            //	Do we have to create the folder?
            if (System.IO.Directory.Exists(tmaxCaseFolder.Path) == false)
            {
                //	Can only create it if it's not an overridden folder
                //
                //	NOTE:	We borrowed the Anchor flag in the call to SetCaseFolder()
                if (tmaxCaseFolder.Anchor == false)
                {
                    try { System.IO.Directory.CreateDirectory(tmaxCaseFolder.Path); }
                    catch
                    {
                        if (bPrompt == true)
                            ShowCasePathWarning(((TmaxCaseFolders)tmaxCaseFolder.UserData), tmaxCaseFolder.Path, true);
                        return false;
                    }
                }
                else
                {
                    if (bPrompt == true)
                        ShowCasePathWarning(((TmaxCaseFolders)tmaxCaseFolder.UserData), tmaxCaseFolder.Path, false);
                    return false;
                }

            }// if(System.IO.Directory.Exists(tmaxCaseFolder.Path) == false)

            return true;

        }// private bool CreateCaseFolder(CTmaxSourceFolder tmaxCaseFolder, bool bPrompt)

        /// <summary>Displays a warning when unable to locate or create one of the case media paths</summary>
        /// <param name="eFolder">The enumerated media folder type</param>
        /// <param name="strPath">The path that has triggered the warning</param>
        ///	<param name="bCreate">True indicates that this is an attempt to create the folder</param>
        private void ShowCasePathWarning(TmaxCaseFolders eFolder, string strPath, bool bCreate)
        {
            string strMsg = "";

            //	Is this an attempt to create the folder?
            if (bCreate == true)
            {
                strMsg = "Unable to create the case folder for ";
                strMsg += (eFolder.ToString() + ":\n\n");
                strMsg += (strPath + "\n\n");

                if (m_tmaxCaseOptions.PathMap != null)
                    strMsg += ("You are currently connected to the path map named " + m_tmaxCaseOptions.PathMap.Name + "\n\n");

                strMsg += "Use the case options [F2] to select or edit a path map";
            }
            else
            {
                strMsg = "Mapped case folder for ";
                strMsg += (eFolder.ToString() + " does not exist!\n\n");
                strMsg += (strPath + "\n\n");

                if (m_tmaxCaseOptions.PathMap != null)
                    strMsg += ("You are currently connected to the path map named " + m_tmaxCaseOptions.PathMap.Name + "\n\n");

                strMsg += "Create the folder or use case options [F2] to select or edit a path map";
            }

            MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }// private void ShowCasePathWarning(TmaxCaseFolders eFolder, string strPath, bool bCreate)

        /// <summary>This method is called to prepare the Adobe converter to perform an operation</summary>
        /// <param name="tmaxSource">The folder containing all source files to be merged</param>
        /// <returns>true if successful</returns>
        private bool PrepAdobeConverter(CTmaxSourceFolder tmaxSource)
        {
            string strWarning = "";

            if (System.IO.File.Exists("PDFManager\\gsdll32.dll"))
            {
                if (System.IO.File.Exists("PDFManager\\gsdll32.lib"))
                {
                    if (System.IO.File.Exists("PDFManager\\mudraw.exe"))
                    {
                        // All files required for PDF conversion exists and we can proceed.
                    }
                    else
                    {
                        strWarning = @"PDFManager\mudraw.exe";
                    }
                }
                else
                {
                    strWarning = @"PDFManager\gsdll32.lib";
                }
            }
            else
            {
                strWarning = @"PDFManager\gsdll32.dll";
            }

            //	Were we unable to get the converter?
            if (strWarning.Length > 0)
            {
                //	Should we display the warning message?
                if (m_bRegisterWarnAdobe == true)
                {
                    MessageBox.Show(strWarning + " is missing. Please reinstall Trialmax.", "PDF Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    m_bRegisterWarnAdobe = false;
                }
                return false;
            }
            else
            {
                return true; // Ready to convert
            }

        }// private bool PrepAdobeConverter()

        /// <summary>This method will add a duplicate of the tertiary record specified by the caller</summary>
        /// <param name="dxSource">The source record to be duplicated</param>
        /// <returns>true if successful</returns>
        private CDxTertiary AddDuplicate(CDxTertiary dxSource)
        {
            string strSourceFileSpec = "";
            CXmlDesignation xmlSource = null;
            CXmlLink xmlLink = null;
            CDxTertiary dxDuplicate = null;

            Debug.Assert(dxSource != null);
            Debug.Assert((dxSource.MediaType == TmaxMediaTypes.Designation) || (dxSource.MediaType == TmaxMediaTypes.Clip));

            //	We only duplicate designations and clips
            if ((dxSource.MediaType != TmaxMediaTypes.Designation) && (dxSource.MediaType != TmaxMediaTypes.Clip))
                return null;

            //	Verify that the source file exists
            strSourceFileSpec = GetFileSpec(dxSource);
            if (System.IO.File.Exists(strSourceFileSpec) == false)
            {
                FireError(this, "AddDuplicate", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_DUPLICATE_SOURCE_NOT_FOUND, dxSource.MediaType, GetBarcode(dxSource, false), strSourceFileSpec));
                return null;
            }

            //	Attempt to open the source file
            try
            {
                xmlSource = new CXmlDesignation();
                SetHandlers(xmlSource.EventSource);

                //	Open the source file and fill the child collections
                //
                //	NOTE:	Don't read in the links. We'll get those from
                //			the source record
                if (xmlSource.FastFill(strSourceFileSpec, false, true) == false)
                {
                    xmlSource.Close(true);
                    xmlSource = null;
                }

            }
            catch
            {
            }

            //	Were we unable to open the source file?
            if (xmlSource == null)
            {
                FireError(this, "AddDuplicate", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_DUPLICATE_OPEN_FAILED, dxSource.MediaType, GetBarcode(dxSource, false), strSourceFileSpec));
                return null;
            }

            //	Create the duplicate record using the XML designation
            if ((dxDuplicate = AddDesignation(dxSource.Secondary, xmlSource)) == null)
                return null;

            //	Does this record have quaternary children?
            if ((dxSource.ChildCount > 0) && (dxSource.Quaternaries != null))
            {
                try
                {
                    //	Fill the quaternary collection if not already filled
                    if (dxSource.Quaternaries.Count == 0)
                        dxSource.Fill();

                    //	Add copies of each link to the duplicate
                    foreach (CDxQuaternary O in dxSource.Quaternaries)
                    {
                        //	Create an XML link from the source record
                        xmlLink = new CXmlLink();
                        O.SetAttributes(xmlLink);

                        //	Add to the duplicate record
                        AddLink(dxDuplicate, xmlSource, xmlLink);

                    }// foreach(CDxQuaternary O in dxSource.Quaternaries)

                }
                catch (System.Exception Ex)
                {
                    FireError(this, "AddDuplicate", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_DUPLICATE_LINKS_EX, dxSource.MediaType, GetBarcode(dxSource, false)), Ex);
                }

            }// if((dxSource.ChildCount > 0) && (dxSource.Quaternaries != null))

            return dxDuplicate;

        }// private CDxTertiary AddDuplicate(CDxTertiary dxSource)

        /// <summary>This method is called to determine if the maximum number of primaries has been reached</summary>
        /// <param name="bSilent">True to inhibit the popup message if limit is reached</param>
        /// <returns>True if the limit has been reached</returns>
        private bool CheckMaxPrimaries(bool bSilent)
        {
            string strMsg = "";

            //	No limit if the application has been activated
            if (this.TmaxProductManager == null) return false;
            if (this.TmaxProductManager.Activated == true) return false;

            //	Has a valid limit been specified?
            if (m_tmaxAppOptions == null) return false;
            if (m_tmaxAppOptions.MaxPrimaries <= 0) return false;

            //	Have we reached the limit?
            if (m_dxPrimaries == null) return false;
            if (m_dxPrimaries.Count < m_tmaxAppOptions.MaxPrimaries) return false;

            //	Should we warn the user?
            if (bSilent == false)
            {
                strMsg = "TrialMax has not been activated. ";
                strMsg += "The database is limited to ";
                strMsg += m_tmaxAppOptions.MaxPrimaries.ToString();
                strMsg += " primary media records.\n\n";
                strMsg += "Select Activate TrialMax from the Help menu to get instructions for activating the application.";

                MessageBox.Show(strMsg, "Activation Required", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //	Cancel the registration if active
            m_bRegisterCancelled = true;
            if ((m_cfRegisterProgress != null) && (m_cfRegisterProgress.IsDisposed == false))
                m_cfRegisterProgress.Cancel();

            return true;

        }// private bool CheckMaxPrimaries(bool bSilent)

        /// <summary>This method initializes the objections database</summary>
        /// <returns>True if successful</returns>
        private bool InitializeObjections()
        {
            CTmaxSourceFolder tmaxFolder = null;
            string strFolder = "";
            string strFileSpec = "";
            bool bSuccessful = false;

            //	Get the objections folder defined by the active path map
            if ((tmaxFolder = m_aCaseFolders[(int)(TmaxCaseFolders.Objections)]) != null)
                strFolder = tmaxFolder.Path;

            //	Assign the default folder if necessary
            if (strFolder.Length == 0)
                strFolder = this.Folder;

            //	Append the trailing backslash
            if ((strFolder.Length > 0) && (strFolder.EndsWith("\\") == false))
                strFolder += "\\";

            //	Make sure the default filename has been assigned
            m_tmaxObjectionsDatabase.Filename = CObjectionsDatabase.OBJECTIONS_DATABASE_CASE_FILENAME;

            //	Get the fully qualified path
            strFileSpec = m_tmaxObjectionsDatabase.GetFileSpec(strFolder);

            //	Does the file already exist?
            if (System.IO.File.Exists(strFileSpec) == true)
            {
                //	Open the existing database
                bSuccessful = m_tmaxObjectionsDatabase.Open(strFolder, this.GetUserName());

                //	Load the existing objections
                if (bSuccessful == true)
                    m_tmaxObjectionsDatabase.Fill();
            }
            else
            {
                //	Create a new objections database
                bSuccessful = m_tmaxObjectionsDatabase.Create(strFolder, this.GetUserName());
            }

            //	Set the active case
            if (bSuccessful == true)
                m_tmaxObjectionsDatabase.SetCase(this);

            return bSuccessful;

        }// private bool InitializeObjections()

        /// <summary>This method initializes the case codes used for fielded data</summary>
        /// <param name="bPopulateTables">True to populate the tables after they've been opened</param>
        /// <returns>True if successful</returns>
        private bool InitializeCaseCodes(bool bPopulateTables)
        {
            bool bSuccessful = false;
            CDxCodes dxCodes = null;
            long lDatabaseVer = 0;

            try
            {
                //	Initialize the class members to assume that the required
                //	tables are not in the database
                m_bCodesConversionRequired = true;
                m_iCodesIndexValuePickList = -1;
                m_iCodesIndexModifiedBy = -1;
                m_iCodesIndexModifiedOn = -1;

                //	Do we have the codes (fielded data) table?
                //
                //	NOTE: Codes were added in version 6.1.6
                m_bCodesConversionRequired = (this.HasTable(CDxCodes.TABLE_NAME) == false);

                //	Stop here if we can't support fielded data
                if (m_bCodesConversionRequired == true) return false;

                //	Create the codes manager
                m_tmaxCodesManager = new FTI.Shared.Trialmax.CTmaxCodesManager();
                SetHandlers(m_tmaxCodesManager.EventSource);

                //	Do we have the CaseCodes table? (added in version 6.2.1)
                if (this.HasTable(CDxCaseCodes.TABLE_NAME) == true)
                {
                    //	Initialize the case codes interface
                    this.DxCaseCodes = new CDxCaseCodes(this);
                    this.DxCaseCodes.Open();

                    //	Do we have the PickLists table? (added in version 6.2.1)
                    if (this.HasTable(CDxPickItems.TABLE_NAME) == true)
                    {
                        //	Initialize the exchange interface for the root pick list item
                        this.DxPickLists = new CDxPickItem();
                        this.DxPickLists.Children.Database = this;
                        this.DxPickLists.TmaxPickItem = this.PickLists;
                        this.DxPickLists.CaseSensitive = true;

                        if (this.DxPickLists.Children.Open() == true)
                        {
                            //	Make sure the valuePickList column has been added to the Codes table
                            dxCodes = new CDxCodes(this);
                            dxCodes.CreateColumn((int)(CDxCodes.eFields.valuePickList), false);
                            m_iCodesIndexValuePickList = this.GetColumnIndex(CDxCodes.TABLE_NAME, CDxCodes.eFields.valuePickList.ToString());

                            //	What version is this database?
                            lDatabaseVer = GetPackedVer(false);

                            //	Was this database created with a version earlier than 6.2.3?
                            if (lDatabaseVer < GetPackedVer(6, 2, 3))
                            {
                                //	Make sure the ModifiedBy/On columns have been added to the Codes table (ver 6.2.3)
                                dxCodes.CreateColumn((int)(CDxCodes.eFields.ModifiedBy), true);
                                m_iCodesIndexModifiedBy = this.GetColumnIndex(CDxCodes.TABLE_NAME, CDxCodes.eFields.ModifiedBy.ToString());
                                dxCodes.CreateColumn((int)(CDxCodes.eFields.ModifiedOn), true);
                                m_iCodesIndexModifiedOn = this.GetColumnIndex(CDxCodes.TABLE_NAME, CDxCodes.eFields.ModifiedOn.ToString());

                                //	Update the version identifier if successful
                                if ((m_iCodesIndexModifiedBy > 0) && (m_iCodesIndexModifiedOn > 0))
                                    SetCaseVersion(6, 2, 3, 0);

                            }// if(lDatabaseVer < GetPackedVer(6,2,3))
                            else
                            {
                                m_iCodesIndexModifiedBy = this.GetColumnIndex(CDxCodes.TABLE_NAME, CDxCodes.eFields.ModifiedBy.ToString());
                                m_iCodesIndexModifiedOn = this.GetColumnIndex(CDxCodes.TABLE_NAME, CDxCodes.eFields.ModifiedOn.ToString());
                                Debug.Assert(m_iCodesIndexModifiedBy > 0);
                                Debug.Assert(m_iCodesIndexModifiedOn > 0);
                            }

                        }
                        else
                        {
                            //	Can't support pick lists
                            this.DxPickLists = null;
                        }

                    }// if(this.Connection.HasTable(CDxCaseCodes.TABLE_NAME) == true)

                }// if(this.Connection.HasTable(CDxPickItems.TABLE_NAME) == true)

                //	Should we allow use of pick lists?
                m_tmaxCodesManager.PickListsEnabled = this.PickListsEnabled;

                //	Should we read the XML file to populate the manager's collections?
                if ((bPopulateTables == true) || (this.PickListsEnabled == false))
                {
                    //	Load the case codes from file
                    if (GetCodesFromFile() == true)
                    {
                        FillCaseCodes();

                        bSuccessful = true;

                    }// if(GetCodesFromFile() == true)

                }
                else
                {
                    //	Populate the collections with the database tables
                    RefreshCaseCodes();
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "InitializeCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_INITIALIZE_CASE_CODES_EX), Ex);
            }

            return bSuccessful;

        }// private bool InitializeCaseCodes(bool bPopulateTables)

        /// <summary>This method initializes the case codes from an external file</summary>
        /// <returns>True if successful</returns>
        /// <remarks>CaseCodes were moved to the database in version 6.2.1</remarks>
        private bool GetCodesFromFile()
        {
            bool bSuccessful = false;
            string strFileSpec = "";
            string strAppTypes = "";

            try
            {
                Debug.Assert(m_tmaxCodesManager != null);

                //	Build the path to the XML file containing the type descriptors
                strFileSpec = GetCaseCodesFileSpec(false);

                //	Check to see if the file exists
                if (System.IO.File.Exists(strFileSpec) == false)
                {
                    //	Build the path to the master file
                    strAppTypes = GetCaseCodesFileSpec(true);

                    //	Does the application master file exist?
                    if (System.IO.File.Exists(strAppTypes) == true)
                    {
                        //	Copy the master
                        try { System.IO.File.Copy(strAppTypes, strFileSpec, true); }
                        catch { }
                    }

                }// if(System.IO.File.Exists(strFileSpec) == false)

                //	Load the information stored in the file
                if (m_tmaxCodesManager.Load(strFileSpec) == true)
                {
                    if (this.User != null)
                        m_tmaxCodesManager.UserName = this.User.Name;
                    if ((m_tmaxCaseOptions != null) && (m_tmaxCaseOptions.Machine != null))
                        m_tmaxCodesManager.MachineName = m_tmaxCaseOptions.Machine.Name;

                    //	Is this a new file?
                    if (m_tmaxCodesManager.CaseId != this.Detail.UniqueId)
                    {
                        if (this.Detail != null)
                        {
                            //	Set the database id and version
                            m_tmaxCodesManager.CaseId = this.Detail.UniqueId;
                            CTmdataVersion ver = new CTmdataVersion();
                            m_tmaxCodesManager.CaseVersion = ver.Version;

                        }// if(this.Detail != null)

                        m_tmaxCodesManager.CreatedBy = m_tmaxCodesManager.UserName;
                        m_tmaxCodesManager.ModifiedBy = m_tmaxCodesManager.UserName;
                        m_tmaxCodesManager.CreatedOn = System.DateTime.Now.ToString();
                        m_tmaxCodesManager.ModifiedOn = m_tmaxCodesManager.CreatedOn;

                        m_tmaxCodesManager.Save(true);

                    }// if(m_tmaxCodesManager.CaseId != this.Detail.MasterId)

                    bSuccessful = true;

                }// if(m_tmaxCodesManager.Initialize(strFileSpec) == true)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "GetCodesFromFile", this.ExBuilder.Message(ERROR_CASE_DATABASE_GET_CODES_FROM_FILE_EX), Ex);
            }

            return bSuccessful;

        }// private bool GetCodesFromFile()

        /// <summary>This method is called to get the path to the file containing the case code descriptors</summary>
        /// <param name="bMaster">True to get the path to the application's master file</param>
        /// <returns>The fully qualified path</returns>
        private string GetCaseCodesFileSpec(bool bMaster)
        {
            string strFileSpec = "";

            //	Are we looking for the master?
            if (bMaster == true)
            {
                strFileSpec = m_tmaxAppOptions.ApplicationFolder;
            }
            else
            {
                //	Build the path to the XML file containing the type descriptors
                strFileSpec = this.Folder;
            }

            if ((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
                strFileSpec += "\\";
            strFileSpec += CASE_DATABASE_CASE_CODES_FILENAME;

            return strFileSpec;

        }// private string GetCaseCodesFileSpec(bool bMaster)

        /// <summary>This method uses the codes manager to fill the CaseCodes and PickItems tables</summary>
        /// <returns>True if successful</returns>
        private bool FillCaseCodes()
        {
            bool bSuccessful = false;
            CTmaxCaseCodes tmaxAddCodes = null;
            CTmaxItem tmaxAddPickItems = null;

            try
            {
                //	Codes must be enabled
                if (this.CodesEnabled == false) return false;
                if (this.CodesManager == null) return false;

                //	Should we fill the pick items table?
                if ((this.DxPickLists != null) && (this.PickLists != null))
                {
                    if ((this.PickLists.Children != null) && (this.PickLists.Children.Count > 0))
                    {
                        //	Create an event item for the operation
                        tmaxAddPickItems = new CTmaxItem();
                        tmaxAddPickItems.DataType = TmaxDataTypes.PickItem;

                        //	Add each of the children to the source collection
                        foreach (CTmaxPickItem O in this.PickLists.Children)
                            tmaxAddPickItems.SourceItems.Add(new CTmaxItem(O));

                        //	Add to the database
                        AddPickItem(this.PickLists, true, false, null);

                    }// if((this.PickLists.Children != null) && (this.PickLists.Children.Count > 0))

                }// if((this.DxPickLists != null) && (this.PickLists != null))

                //	Should we fill the case codes table?
                if ((this.DxCaseCodes != null) && (this.CaseCodes != null))
                {
                    if (this.CaseCodes.Count > 0)
                    {
                        //	Transfer the codes to a temporary collection
                        tmaxAddCodes = new CTmaxCaseCodes();
                        foreach (CTmaxCaseCode O in this.CaseCodes)
                            tmaxAddCodes.Add(O);
                        this.CaseCodes.Clear();

                        AddCaseCodes(tmaxAddCodes, null, false, false, null);

                        tmaxAddCodes.Clear();

                    }// if((this.CaseCodes != null) && (this.CaseCodes.Count > 0))

                }// if(this.DxPickLists != null)

                bSuccessful = true;

            }
            catch (System.Exception Ex)
            {
                FireError(this, "FillCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_FILL_CASE_CODES_EX), Ex);
            }

            return bSuccessful;

        }// private bool FillCaseCodes()

        /// <summary>This method is called to upgrade the database to supported codes (fielded data) and pick lists</summary>
        /// <returns>true if enabled</returns>
        private bool EnableCaseCodes()
        {
            bool bSuccessful = false;

            //	We have to have a valid connection
            if (this.IsConnected == false) return false;

            try
            {
                while (bSuccessful == false)
                {
                    //	Make sure we have the Codes (fielded data) table
                    if (CreateTable(CDxCodes.TABLE_NAME) == false)
                        break;

                    //	Make sure we have the pick list column in the Codes table
                    //
                    //	NOTE:	The previous call to CreateTable() adds this column if the table
                    //			already exists in the database
                    m_iCodesIndexValuePickList = this.GetColumnIndex(CDxCodes.TABLE_NAME, CDxCodes.eFields.valuePickList.ToString());
                    Debug.Assert(m_iCodesIndexValuePickList >= 0);

                    //	Make sure we have the time stamp columns in the Codes table
                    //
                    //	NOTE:	The previous call to CreateTable() adds these columns if the table
                    //			already exists in the database
                    m_iCodesIndexModifiedBy = this.GetColumnIndex(CDxCodes.TABLE_NAME, CDxCodes.eFields.ModifiedBy.ToString());
                    Debug.Assert(m_iCodesIndexModifiedBy >= 0);
                    m_iCodesIndexModifiedOn = this.GetColumnIndex(CDxCodes.TABLE_NAME, CDxCodes.eFields.ModifiedOn.ToString());
                    Debug.Assert(m_iCodesIndexModifiedOn >= 0);

                    //	Do we need to create the codes manager?
                    if (m_tmaxCodesManager == null)
                    {
                        //	Create the codes manager
                        m_tmaxCodesManager = new FTI.Shared.Trialmax.CTmaxCodesManager();
                        SetHandlers(m_tmaxCodesManager.EventSource);
                    }

                    //	Make sure we have the CaseCodes table
                    if (CreateTable(CDxCaseCodes.TABLE_NAME) == false)
                        break;

                    //	Make sure we have the PickLists table
                    if (CreateTable(CDxPickItems.TABLE_NAME) == false)
                        break;

                    //	Add the warning message to the database
                    AddWarning(CASE_DATABASE_PICKLISTS_VERSION, CASE_DATABASE_PICKLISTS_WARNING);

                    //	All done
                    bSuccessful = true;

                }// while(bSuccessful == false)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "EnableCaseCodes", this.ExBuilder.Message(ERROR_CASE_DATABASE_ENABLE_CASE_CODES_EX), Ex);
                bSuccessful = false;
            }

            return bSuccessful;

        }// private bool EnableCaseCodes()

        /// <summary>This method is called to add a table to the database</summary>
        /// <param name="dxTable">The record collection object for the specified table</param>
        /// <returns>true if successful</returns>
        private bool CreateTable(CBaseRecords dxTable)
        {
            bool bSuccessful = true;

            //	We have to have a valid connection
            if (this.IsConnected == false) return false;
            Debug.Assert(dxTable != null);
            if (dxTable == null) return false;

            try
            {
                //	Make sure the collection has been assigned the database
                if (dxTable.Database == null)
                    dxTable.Database = this;

                //	Do we need to add the requested table?
                if (this.HasTable(dxTable.TableName) == false)
                {
                    //	Create the table
                    if (dxTable.Create() == false)
                        bSuccessful = false;

                }
                else
                {
                    //	Is this the Codes table?
                    if (String.Compare(dxTable.TableName, CDxCodes.TABLE_NAME, true) == 0)
                    {
                        //	Make sure we have the column to store the pick list values
                        dxTable.CreateColumn((int)(CDxCodes.eFields.valuePickList), false);

                        //	Make sure we have the column to store the ModifiedBy value
                        dxTable.CreateColumn((int)(CDxCodes.eFields.ModifiedBy), false);

                        //	Make sure we have the column to store the ModifiedOn value
                        dxTable.CreateColumn((int)(CDxCodes.eFields.ModifiedOn), false);

                    }

                }// if(this.Connection.HasTable(dxTable.TableName) == false)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "CreateTable", this.ExBuilder.Message(ERROR_CASE_DATABASE_CREATE_TABLE_EX, dxTable.TableName), Ex);
                bSuccessful = false;
            }

            return bSuccessful;

        }// private bool CreateTable(CBaseRecords dxTable)

        /// <summary>This method is called to create the specified table</summary>
        ///	<param name="strName">The name of the table to create</param>
        /// <returns>true if successful</returns>
        private bool CreateTable(string strName)
        {
            switch (strName)
            {
                case CDxCodes.TABLE_NAME:

                    return CreateTable(new CDxCodes());

                case CDxCaseCodes.TABLE_NAME:

                    return CreateTable(new CDxCaseCodes());

                case CDxPickItems.TABLE_NAME:

                    return CreateTable(new CDxPickItems());

                default:

                    Debug.Assert(false, "Unable to create table: name = " + strName);
                    return false;

            }// switch(strName)

        }// private bool CreateTable(string strName)

        /// <summary>This method is called to check for duplicates before adding new binder entries</summary>
        /// <param name="dxParent">The binder to which entries are being added</param>
        /// <param name="tmaxSource">The collection of items that identify the records to be added</param>
        /// <param name="bNoDuplicates">true to prevent duplicates without prompting for confirmation</param>
        /// <returns>True if OK to continue with the operation</returns>
        private bool ConfirmBinderDuplicates(CDxBinderEntry dxParent, CTmaxItems tmaxSource, bool bNoDuplicates)
        {
            CDxMediaRecords dxContents = null;
            CDxMediaRecords dxDuplicates = null;
            CDxMediaRecord dxMedia = null;
            bool bContinue = true;
            string strMsg = "";

            //	Don't bother if no source
            if ((tmaxSource == null) || (tmaxSource.Count == 0)) return true;

            //	Don't bother checking if adding to the root
            if (dxParent == null) return true;

            //	Don't bother checking if the user has turned off the warning
            if ((m_tmaxAppOptions != null) && (m_tmaxAppOptions.WarnBinderDuplicates == false)) return true;

            //	Make sure the parent collection has been populated
            if ((dxParent.Contents == null) || (dxParent.Contents.Count == 0))
                dxParent.Fill();

            try
            {
                //	Allocate a temporary collection to use for checking
                dxContents = new CDxMediaRecords();
                dxDuplicates = new CDxMediaRecords();

                //	Copy the existing media entries to the temporary collection
                foreach (CDxBinderEntry O in dxParent.Contents)
                {
                    if ((O.IsMedia() == true) && (O.GetSource(true) != null))
                        dxContents.AddList(O.GetSource(true));
                }

                //	Now iterate the source items and check for duplicates
                foreach (CTmaxItem O in tmaxSource)
                {
                    if ((dxMedia = (CDxMediaRecord)(O.GetMediaRecord())) != null)
                    {
                        //	Does the record already exist in the contents?
                        if (dxContents.Contains(dxMedia) == true)
                        {
                            //	This is a duplicate
                            dxDuplicates.AddList(dxMedia);
                        }
                        else
                        {
                            //	Make it part of the contents
                            dxContents.AddList(dxMedia);
                        }

                    }// if((dxMedia = O.GetMediaRecord()) != null)

                }// foreach(CTmaxItem O in tmaxSource)

                //	Do we have any duplicates?
                if (dxDuplicates.Count > 0)
                {
                    //	Have duplicates been prohibited by the caller
                    if (bNoDuplicates == true)
                    {
                        bContinue = false;
                    }
                    else
                    {
                        //	Construct a message to warn the user
                        strMsg = "If you continue with this operation ";
                        strMsg += dxParent.Name;
                        strMsg += " will have duplicate entries for:\n\n";

                        foreach (CDxMediaRecord O in dxDuplicates)
                            strMsg += (O.GetBarcode(false) + "\n");

                        strMsg += "\nDo you want to continue?";


                    }// if(bNoDuplicates == true)

                }// if(dxDuplicates.Count > 0)

            }
            catch
            {
            }
            finally
            {
                //	Flush the temporary collections
                if (dxContents != null)
                {
                    dxContents.Clear();
                    dxContents = null;
                }
                if (dxDuplicates != null)
                {
                    dxDuplicates.Clear();
                    dxDuplicates = null;
                }

            }

            return bContinue;

        }// private bool ConfirmBinderDuplicates(CDxBinderEntry dxParent, CTmaxItems tmaxSource)

        /// <summary>This method will reorder the child records of the specified media parent</summary>
        /// <param name="tmaxItem">The TrialMax event item that identifies the parent</param>
        /// <returns>true if successful</returns>
        private bool ReorderMedia(CTmaxItem tmaxItem)
        {
            CDxMediaRecord dxParent = null;
            CDxMediaRecord dxChild = null;
            CDxMediaRecords dxChildren = null;
            CDxMediaRecords dxRecords = new CDxMediaRecords();

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.SubItems != null);
            Debug.Assert(tmaxItem.SubItems.Count > 0);
            if (tmaxItem == null) return false;
            if (tmaxItem.SubItems == null) return false;
            if (tmaxItem.SubItems.Count <= 0) return false;

            //	Get the parent's exchange object
            if ((dxParent = GetRecordFromItem(tmaxItem)) == null)
            {
                //	Are we reordering root-level binders?
                if (tmaxItem.SubItems[0].DataType == TmaxDataTypes.Binder)
                    dxParent = m_dxRootBinder;
                else
                    return false;
            }

            //	Get the parent's child collection
            if ((dxChildren = dxParent.GetChildCollection()) == null)
            {
                FireError(this, "Reorder", this.ExBuilder.Message(ERROR_CASE_DATABASE_REORDER_NO_CHILDREN, dxParent.MediaType, dxParent.AutoId));
                return false;
            }

            //	Build the new ordered collection
            foreach (CTmaxItem tmaxSubItem in tmaxItem.SubItems)
            {
                if (dxParent.GetDataType() == TmaxDataTypes.Media)
                    dxChild = (CDxMediaRecord)tmaxSubItem.GetMediaRecord();
                else
                    dxChild = (CDxMediaRecord)tmaxSubItem.IBinderEntry;

                if (dxChild != null)
                    dxRecords.AddList(dxChild);
            }

            //	The collection sizes should match
            if (dxRecords.Count != dxChildren.Count)
            {
                FireError(this, "Reorder", this.ExBuilder.Message(ERROR_CASE_DATABASE_REORDER_COUNTS, dxParent.MediaType, dxParent.AutoId));
                return false;
            }

            if (dxParent.Reorder(dxRecords) == true)
            {
                tmaxItem.State = TmaxItemStates.Processed;
                return true;
            }
            else
            {
                return false;
            }

        }//	private bool ReorderMedia(CTmaxItem tmaxItem)

        /// <summary>This method will reorder the application's collection of CaseCodes</summary>
        /// <param name="tmaxItem">The TrialMax event item that defines the new order</param>
        /// <returns>true if successful</returns>
        private bool ReorderCaseCodes(CTmaxItem tmaxItem)
        {
            CTmaxCaseCode tmaxCaseCode = null;

            Debug.Assert(tmaxItem != null);
            Debug.Assert(tmaxItem.DataType == TmaxDataTypes.CaseCode);
            Debug.Assert(tmaxItem.SubItems != null);
            Debug.Assert(tmaxItem.SubItems.Count > 0);
            if (tmaxItem == null) return false;
            if (tmaxItem.DataType != TmaxDataTypes.CaseCode) return false;
            if (tmaxItem.SubItems == null) return false;
            if (tmaxItem.SubItems.Count <= 0) return false;

            //	Case codes must be enabled
            if (this.CaseCodes == null) return false;

            //	Should be as many case codes as subitems
            if (this.CaseCodes.Count != tmaxItem.SubItems.Count) return false;

            //	All we have to do is update the SortOrder identifier of each object
            for (int i = 0; i < tmaxItem.SubItems.Count; i++)
            {
                if ((tmaxCaseCode = tmaxItem.SubItems[i].CaseCode) != null)
                {
                    //	Do we need to change the sort order?
                    if (tmaxCaseCode.SortOrder != (i + 1))
                    {
                        tmaxCaseCode.SortOrder = (i + 1);

                        if ((this.DxCaseCodes != null) && (tmaxCaseCode.DxRecord != null))
                            this.DxCaseCodes.Update((CDxCaseCode)(tmaxCaseCode.DxRecord));

                    }// if(tmaxCaseCode.SortOrder != (i + 1))

                }// if((tmaxCaseCode = tmaxItem.SubItems[i].CaseCode) != null)

            }// for(int i = 0; i < tmaxItem.SubItems.Count; i++)

            //	Make sure the collection is sorted
            this.CaseCodes.Sort(true);

            //	If not using the database mark the manager as modified
            if (this.DxCaseCodes == null)
                this.CodesManager.Modified = true;

            tmaxItem.State = TmaxItemStates.Processed;
            return true;

        }//	private bool ReorderCaseCodes(CTmaxItem tmaxItem)

        /// <summary>This method handles command events fired by one of the child controls</summary>
        /// <param name="objSender">The object firing the event</param>
        /// <param name="Args">The event arguments</param>
        private void OnTmaxCommand(object objSender, FTI.Shared.Trialmax.CTmaxCommandArgs Args)
        {
            //	Propagate the event
            if (TmaxCommandEvent != null)
                TmaxCommandEvent(objSender, Args);
        }

        /// <summary>This method will perform a bulk update of fielded data codes</summary>
        /// <param name="tmaxUpdates">The updates that define the bulk operation</param>
        /// <param name="dxPrimaries">The primary records being updated</param>
        /// <returns>true if successful</returns>
        private bool BulkUpdate(CTmaxCodeUpdates tmaxUpdates, CDxPrimaries dxPrimaries)
        {
            CDxCodes dxCodes = null;
            bool bSuccessful = false;

            //	This shouldn't happen but just in case...
            //
            //	NOTE:	A null primaries collection means all primaries get updated
            if (tmaxUpdates == null) return false;
            if (tmaxUpdates.Count == 0) return false;

            try
            {
                //	Allocate a Codes collection to perform the operation
                dxCodes = new CDxCodes(this);

                //	Start by deleting the codes being updated
                if (dxCodes.Delete(tmaxUpdates, dxPrimaries) == true)
                {
                    //	Did the caller provide specific primary records?
                    //
                    //	NOTE:	We don't use the master collection for Delete because
                    //			it's optimized for All Primaries
                    if (dxPrimaries == null)
                        dxPrimaries = m_dxPrimaries; // Use the master collection

                    //	Now add the requested codes
                    bSuccessful = dxCodes.Add(tmaxUpdates, dxPrimaries);

                }// if(dxCodes.Delete(tmaxUpdates, dxPrimaries) == true)

                //	Force a refresh of the codes bound to each primary record
                if (dxPrimaries != null)
                    dxPrimaries.ResetCodes();
                else
                    m_dxPrimaries.ResetCodes();
            }
            catch (System.Exception Ex)
            {
                FireError(this, "BulkUpdate", this.ExBuilder.Message(ERROR_CASE_DATABASE_BULK_UPDATE_EX), Ex);
            }

            return bSuccessful;

        }// private bool BulkUpdate(CTmaxCodeUpdates tmaxUpdates, CDxPrimaries dxPrimaries)

        /// <summary>This method is called to populate the error builder's format string collection</summary>
        protected override void SetErrorStrings()
        {
            if (m_tmaxErrorBuilder == null) return;
            if (m_tmaxErrorBuilder.FormatStrings == null) return;

            //	Let the base class add its strings first
            base.SetErrorStrings();

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a designation to the database.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the database record collections.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the source media for a link: PSTQ: %1  MediaID: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a link to the database.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a treatment created with TmaxPresentation: Source = %1");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to refresh the case codes.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to flush the codes table.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to add the primary record. Invalid media type specified.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to register the source folder.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add case codes to the database.");

            //	11
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the case codes collection.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the template to create the new database: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to copy the database template from %1 to %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to duplicate the requested record: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the requested designations.");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the export operation.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to export the slide. Presentation file not found: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while exporting slide #%1 to %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the slides for the PowerPoint presentation: filename = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the Internal Update event: record = %1");

            //	21
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the application command event: command = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to resolve the aliased path: record = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("The attempt to resolve the aliased path for the record failed: record = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to search for binder entries: record = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while searching for treatments created by TmaxPresentation: path = %1");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to search for the record with the specified barcode: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the record with the specified barcode: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to search for the record with the specified id: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the record with the specified id: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while searching for records with the specified barcodes: %1");

            //	31
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the XML case descriptor.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the record for the specified deposition: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the transcript record for the specified deposition: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the XML transcript file for the deposition: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the XML transcript for the deposition: %1");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the import operation.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the move operation.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to register the source media. The database is closed.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to perform the registration. The source folder is empty: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to start the registration thread.");

            //	41
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the XML designation for %1: path = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the case options.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the bulk update.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to hide the case codes.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the filter.");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to assign the user supplied relative path: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to trim the database. folder = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to execute the validation thread.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to merge the source media.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to morph the source folder. folder = %1");

            //	51
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to offset the source folder: offset = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to assign one record per file.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to separate the source media: type = %1 folder = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to use the filename as the page number. %1 is not a valid number");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the transcript record for %1");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the extents record for %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the target folder to export the PDF document: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to start the PDF conversion using %1");
            m_tmaxErrorBuilder.FormatStrings.Add("The attempt to convert %1 returned an error code: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to convert %1");

            //	61
            m_tmaxErrorBuilder.FormatStrings.Add("Page count returned by PDF converter does not match file count: pages = %1  files = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete the pick list records.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete the case code records.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to move the binder entries.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a new treatment.");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the source media types.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to group the source media.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create a primary record for the source folder: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create a primary record for the specified media type: type = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create a secondary record for the file: path = %1");

            //	71
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete the source records.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to copy source file. %1 could not be found.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to copy %1 to %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the folder where the source media is stored: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("%1 was registered as linked media. All secondary files must be stored in: %2");

            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while locating the scenes sourced by %1 record: DbId = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while locating the links sourced by %1 record: DbId = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the duration of the specified recording: path = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to use the shell to transfer %1 to %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the relative path for the source media: folder = %1");

            //	81
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while retrieving the case folder for %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the target folder for %1: path = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to register %1\n\nThe PDF converter plug-in has not been installed. Select \"Check For Updates ...\" from the application's Help menu to download and install the TrialMax PDF converter. ");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to register %1\n\nThe PDF converter plug-in could not be found in it's registered location %2\n\nSelect \"Check For Updates ...\" from the application's Help menu to download and update the TrialMax PDF converter. ");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to duplicate specified %1 (barcode = %2). The XML source file could not be found (filename = %3)");

            m_tmaxErrorBuilder.FormatStrings.Add("Unable to duplicate specified %1 (barcode = %2). The attempt to open the XML source file failed (filename = %3)");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to duplicate the links for a %1 (barcode = %2).");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the case codes.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to load the case codes from file.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the case codes table.");

            //	91
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to enable case codes");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the table named %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to reorder the records. Unable to retrieve child collection for: Type = %1 Id = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to reorder the records. The child collection sizes do not match: Type = %1 Id = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get a coded property value: barcode = %1 code = %2");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set a coded property value: barcode = %1 code = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the coded properties: barcode = %1  SetCodes = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a record to the %1 table.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve valid auto-identifier after adding a record to the %1 table using SQL statement: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a record to the %1 table using SQL statement: %2");

            //	101
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the foreign barcode.");
            m_tmaxErrorBuilder.FormatStrings.Add("Unable to set the foreign barcode for %1 to %2 - the FBC is inherited by %3");
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while setting the foreign barcode for %1 to %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while exchanging fields in the %1 table. bSetFields = %2");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a highlighter record");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete a record in the %1 table using SQL statement: %2");
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while updating the Media Id for primary record: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while attempting to fill the filtered collection using %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import objections from file.");
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while attempting to update objections");

            // 111
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while attempting to delete objections");
            m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while attempting to add objections");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export objections to file.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to filter the objection records.");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the short case name: Name = %1");

            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the case name: Name = %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to compact the database: %1");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling the compactor finished event");
            m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a treatment using this zap file: %1");

        }// protected override void SetErrorStrings()

        /// <summary>Called to determine if the parameter for declaring objections has been set</summary>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if the Objections parameter has been set</returns>
        public bool IsObjectionsCommand(CTmaxParameters tmaxParameters)
        {
            CTmaxParameter tmaxParameter = null;
            bool bObjections = false;

            if (tmaxParameters != null)
            {
                if ((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Objections)) != null)
                    bObjections = tmaxParameter.AsBoolean();
            }

            return bObjections;

        }// public bool IsObjectionsCommand(CTmaxParameters tmaxParameters)

        /// <summary>This method will add objetions to the database</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxResults">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        public bool AddObjections(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {
            bool bSuccessful = false;
            COxObjection oxObjection = null;

            Debug.Assert(tmaxItem != null);

            if (m_tmaxObjectionsDatabase == null) return false;
            if (m_tmaxObjectionsDatabase.IsConnected == false) return false;
            if (m_tmaxObjectionsDatabase.OxObjections == null) return false;

            try
            {
                //	The application objects should be in the source collection
                if (tmaxItem.SourceItems != null)
                {
                    foreach (CTmaxItem O in tmaxItem.SourceItems)
                    {
                        if (O.Objection != null)
                        {
                            if ((oxObjection = m_tmaxObjectionsDatabase.AddObjection(O.Objection)) != null)
                            {
                                if (tmaxResults != null)
                                    tmaxResults.OnAdded(oxObjection);

                                tmaxItem.SubItems.Add(new CTmaxItem(oxObjection));

                                bSuccessful = true;
                            }

                        }// if(O.Objection != null)

                    }// foreach(CTmaxItem O in tmaxItem.SourceItems)

                }// if(tmaxItem.SourceItems != null)				

            }
            catch (System.Exception Ex)
            {
                FireError(this, "AddObjections", this.ExBuilder.Message(ERROR_CASE_DATABASE_ADD_OBJECTIONS_EX), Ex);
            }

            return bSuccessful;

        }// public bool AddObjections(CTmaxItem tmaxItem, CTmaxItems tmaxAdded, CTmaxParameters tmaxParameters)

        /// <summary>This method will perform an update of the database record associated with the specified item</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxResults">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        public bool UpdateObjections(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {
            bool bSuccessful = false;

            Debug.Assert(tmaxItem != null);

            if (m_tmaxObjectionsDatabase == null) return false;
            if (m_tmaxObjectionsDatabase.IsConnected == false) return false;
            if (m_tmaxObjectionsDatabase.OxObjections == null) return false;

            try
            {
                //	Do we have a valid objection record?
                if (tmaxItem.IObjection != null)
                    bSuccessful = m_tmaxObjectionsDatabase.OxObjections.Update((CBaseRecord)(tmaxItem.IObjection));

                if ((bSuccessful == true) && (tmaxResults != null))
                    tmaxResults.OnUpdated((COxObjection)(tmaxItem.IObjection));
            }
            catch (System.Exception Ex)
            {
                FireError(this, "UpdateObjections", this.ExBuilder.Message(ERROR_CASE_DATABASE_UPDATE_OBJECTIONS_EX), Ex);
            }

            return bSuccessful;

        }//	public bool UpdateObjections(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)

        /// <summary>This method will delete records stored in the objections database</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        ///	<param name="tmaxResults">Collection in which to return the results</param>
        /// <returns>true if successful</returns>
        public bool DeleteObjections(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {
            COxObjection oxObjection = null;
            bool bSuccessful = false;


            Debug.Assert(tmaxItem != null);

            if (m_tmaxObjectionsDatabase == null) return false;
            if (m_tmaxObjectionsDatabase.IsConnected == false) return false;
            if (m_tmaxObjectionsDatabase.OxObjections == null) return false;

            try
            {
                //	Delete each of the specified objections
                foreach (CTmaxItem O in tmaxItem.SubItems)
                {
                    //	Get the record exchange interface for this item
                    if (O.IObjection != null)
                        oxObjection = (COxObjection)(O.IObjection);
                    else if (O.Objection != null)
                        oxObjection = (COxObjection)(O.Objection.IOxObjection);
                    else
                        oxObjection = null;

                    //	Delete the specified record
                    if (oxObjection != null)
                    {
                        m_tmaxObjectionsDatabase.DeleteObjection(oxObjection);

                        //	Add to the results
                        if (tmaxResults != null)
                            tmaxResults.OnDeleted(oxObjection);

                    }// if(oxObjection != null)

                }// foreach(CTmaxItem O in tmaxItem.SubItems)

                bSuccessful = true;

            }
            catch (System.Exception Ex)
            {
                FireError(this, "UpdateObjections", this.ExBuilder.Message(ERROR_CASE_DATABASE_DELETE_OBJECTIONS_EX), Ex);
            }

            return bSuccessful;

        }//	public bool DeleteObjections(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)

        /// <summary>This method will add or insert records using information stored in files selected by the user</summary>
        /// <param name="tmaxItem">The TrialMax event item used to perform the operation</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxResults">Collection to populate with items that represent the new records</param>
        /// <returns>true if successful</returns>
        private bool ImportObjections(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
        {
            bool bSuccessful = false;

            try
            {
                if (m_tmaxObjectionsDatabase == null) return false;

                //	Prepare the manager for a new operation
                m_tmaxImportObjectionsManager.ObjectionsDatabase = m_tmaxObjectionsDatabase;
                m_tmaxImportObjectionsManager.Options = m_tmaxStationOptions.ImportOptions;
                m_tmaxImportObjectionsManager.Initialize(tmaxParameters, tmaxResults);

                //	Prompt the user for the list of files to be imported
                if (m_tmaxImportObjectionsManager.GetSourceFiles() == false) return false;

                //	Execute the import operation
                if (m_tmaxImportObjectionsManager.Import() == true)
                {
                    bSuccessful = true;

                }// if(m_tmaxImportObjectionsManager.Import() == true)

            }
            catch (System.Exception Ex)
            {
                FireError(this, "ImportObjections", this.ExBuilder.Message(ERROR_CASE_DATABASE_IMPORT_OBJECTIONS_EX), Ex);
            }
            finally
            {
                //	This prevents confusion with the standard import manager
                m_tmaxImportObjectionsManager.ObjectionsDatabase = null;
            }

            return bSuccessful;

        }// private bool ImportObjections(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)

        /// <summary>This method will export the requested objections to file</summary>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <param name="tmaxSource">Collection to source records to be exported</param>
        /// <returns>true if successful</returns>
        private bool ExportObjections(CTmaxParameters tmaxParameters, CTmaxItems tmaxSource)
        {
            bool bSuccessful = false;

            try
            {
                if (m_tmaxObjectionsDatabase == null) return false;

                //	Prepare the manager for a new operation
                m_tmaxExportManager.Database = this;
                m_tmaxExportObjectionsManager.ObjectionsDatabase = m_tmaxObjectionsDatabase;
                m_tmaxExportObjectionsManager.ExportOptions = m_tmaxStationOptions.ExportObjectionOptions;

                if (m_tmaxExportObjectionsManager.Initialize(tmaxParameters, tmaxSource) == true)
                {
                    bSuccessful = m_tmaxExportObjectionsManager.Export();
                }

            }
            catch (System.Exception Ex)
            {
                FireError(this, "ExportObjections", this.ExBuilder.Message(ERROR_CASE_DATABASE_EXPORT_OBJECTIONS_EX), Ex);
            }
            finally
            {
                //	This prevents confusion with the standard import manager
                //m_tmaxExportObjectionsManager.ObjectionsDatabase = null;
            }

            return bSuccessful;

        }// private bool ExportObjections(CTmaxItem tmaxItem, CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)

        /// <summary>This method will set the filter used to populate the Filtered Objections record collection</summary>
        /// <param name="tmaxItems">Event items used to identify the filter criteria</param>
        /// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
        /// <returns>true if successful</returns>
        public bool FilterObjections(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
        {
            bool bSuccessful = false;

            try
            {
                if (m_tmaxObjectionsDatabase != null)
                    bSuccessful = m_tmaxObjectionsDatabase.SetFilter(tmaxItems, tmaxParameters);
            }
            catch (System.Exception Ex)
            {
                FireError(this, "FilterObjections", this.ExBuilder.Message(ERROR_CASE_DATABASE_FILTER_OBJECTIONS_EX), Ex);
            }

            return bSuccessful;

        }// public bool FilterObjections(CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

        #endregion Private Methods

        #region Properties

        /// <summary>This is the collection of media types supported by the database</summary>
        public FTI.Shared.Trialmax.CTmaxMediaTypes MediaTypes
        {
            get { return m_tmaxMediaTypes; }
            set { m_tmaxMediaTypes = value; }
        }

        /// <summary>This is the collection of source types supported by the database</summary>
        public FTI.Shared.Trialmax.CTmaxSourceTypes SourceTypes
        {
            get { return m_tmaxSourceTypes; }
            set { m_tmaxSourceTypes = value; }
        }

        /// <summary>This property exposes the current user</summary>
        public CDxUser User
        {
            get { return m_dxUser; }
        }

        /// <summary>This property exposes the users collection</summary>
        public CDxUsers Users
        {
            get { return m_dxUsers; }
        }

        /// <summary>This property exposes the highlighters collection</summary>
        public CDxHighlighters Highlighters
        {
            get { return m_dxHighlighters; }
        }

        /// <summary>True if objection database operations are enabled</summary>
        public bool ObjectionsEnabled
        {
            get
            {
                if (m_tmaxObjectionsDatabase == null)
                    return false;
                if (m_tmaxObjectionsDatabase.IsConnected == false)
                    return false;
                else
                    return true;
            }

        }

        /// <summary>This property exposes the Objections database</summary>
        public CObjectionsDatabase ObjectionsDatabase
        {
            get { return m_tmaxObjectionsDatabase; }
        }

        /// <summary>This property exposes the Objections collection</summary>
        public CTmaxObjections Objections
        {
            get { return ((m_tmaxObjectionsDatabase != null) ? m_tmaxObjectionsDatabase.Objections : null); }
        }

        /// <summary>This property exposes the BarcodeMap collection</summary>
        public CDxBarcodes BarcodeMap
        {
            get { return m_dxBarcodeMap; }
        }

        /// <summary>This property exposes the Primary Media collection</summary>
        public CDxPrimaries Primaries
        {
            get { return m_dxPrimaries; }
        }

        /// <summary>This property exposes the collection of filtered media records</summary>
        public CDxPrimaries Filtered
        {
            get { return m_dxFiltered; }
        }

        /// <summary>This property exposes the active filter object</summary>
        public CTmaxFilter Filter
        {
            get { return m_tmaxLastFilter; }
        }

        /// <summary>This is the application's registry interface</summary>
        public FTI.Shared.Trialmax.CTmaxRegistry TmaxRegistry
        {
            get { return m_tmaxRegistry; }
            set { m_tmaxRegistry = value; }
        }

        /// <summary>This is the application's product descriptor</summary>
        public FTI.Shared.Trialmax.CTmaxProductManager TmaxProductManager
        {
            get { return m_tmaxProductManager; }
            set { m_tmaxProductManager = value; }
        }

        /// <summary>This property exposes the Transcripts collection</summary>
        public CDxTranscripts Transcripts
        {
            get { return m_dxTranscripts; }
        }

        /// <summary>This property exposes the Binders collection</summary>
        public CDxBinderEntries Binders
        {
            get
            {
                if (m_dxRootBinder != null)
                    return m_dxRootBinder.Contents;
                else
                    return null;
            }

        }

        /// <summary>This is the default target for SendToBinder commands</summary>
        public CDxBinderEntry TargetBinder
        {
            get { return GetTargetBinder(); }
            set { SetTargetBinder(value); }
        }

        /// <summary>This property exposes the information stored in the Details table</summary>
        public CDxDetail Detail
        {
            get { return m_dxDetail; }
        }

        /// <summary>Short case name assigned to the database</summary>
        public string ShortCaseName
        {
            get { return GetShortCaseName(); }
            set { SetShortCaseName(value); }
        }

        /// <summary>Case name assigned to the database</summary>
        public string CaseName
        {
            get { return GetCaseName(); }
            set { SetCaseName(value); }
        }

        /// <summary>Name of the database file</summary>
        public string Filename
        {
            get { return m_strFilename; }
            set { m_strFilename = value; }
        }

        /// <summary>Identifier to be used when firing application command events</summary>
        public int PaneId
        {
            get { return m_iPaneId; }
            set { m_iPaneId = value; }
        }

        /// <summary>Folder where the database file is stored</summary>
        /// <remarks>This property is read only. It will be set in the call to Open()</remarks>
        public string Folder
        {
            get { return m_strFolder; }
        }

        /// <summary>Folder where the application that owns this database is stored</summary>
        /// <remarks>This property is used to locate the template required to create a new database</remarks>
        public string AppFolder
        {
            get { return m_strAppFolder; }
            set { m_strAppFolder = value; }
        }

        /// <summary>TrialMax application source registration options</summary>
        public CTmaxRegOptions RegistrationOptions
        {
            get { return m_tmaxRegisterOptions; }
            set { m_tmaxRegisterOptions = value; }
        }

        /// <summary>TrialMax application options</summary>
        public CTmaxManagerOptions AppOptions
        {
            get { return m_tmaxAppOptions; }
            set { m_tmaxAppOptions = value; }
        }

        /// <summary>TrialMax application Windows Media Encoder wrapper</summary>
        public FTI.Trialmax.Encode.CWMEncoder WMEncoder
        {
            get { return m_wmEncoder; }
            set { m_wmEncoder = value; }
        }

        /// <summary>TrialMax case options</summary>
        public CTmaxCaseOptions CaseOptions
        {
            get { return m_tmaxCaseOptions; }
        }

        /// <summary>TrialMax case-specific station options</summary>
        public CTmaxStationOptions StationOptions
        {
            get { return m_tmaxStationOptions; }
        }

        /// <summary>The case codes manager for the active case</summary>
        public FTI.Shared.Trialmax.CTmaxCodesManager CodesManager
        {
            get { return m_tmaxCodesManager; }
        }

        /// <summary>True if code operations are enabled</summary>
        public bool CodesEnabled
        {
            get { return ((m_bCodesConversionRequired == false) || (m_bConvertToCodes == true)); }
        }

        /// <summary>The active collection of case code descriptors</summary>
        public FTI.Shared.Trialmax.CTmaxCaseCodes CaseCodes
        {
            get { return (m_tmaxCodesManager != null) ? m_tmaxCodesManager.CaseCodes : null; }
        }

        /// <summary>The active collection of case code pick lists</summary>
        public FTI.Shared.Trialmax.CTmaxPickItem PickLists
        {
            get { return (m_tmaxCodesManager != null) ? m_tmaxCodesManager.PickLists : null; }
        }

        /// <summary>The data exchange interface to the case codes collection</summary>
        private CDxCaseCodes DxCaseCodes
        {
            get { return (m_tmaxCodesManager != null) ? ((CDxCaseCodes)(m_tmaxCodesManager.DxCaseCodes)) : null; }
            set { if (m_tmaxCodesManager != null) m_tmaxCodesManager.DxCaseCodes = value; }
        }

        /// <summary>The data exchange interface to the pick lists collection</summary>
        private CDxPickItem DxPickLists
        {
            get { return (this.PickLists != null) ? ((CDxPickItem)(this.PickLists.DxRecord)) : null; }
            set { if (this.PickLists != null) this.PickLists.DxRecord = value; }
        }

        /// <summary>True if pick lists are supported by the database</summary>
        public bool PickListsEnabled
        {
            get { return (this.DxPickLists != null); }
        }

        /// <summary>Reference to application instance of TmxMovie control</summary>
        public FTI.Trialmax.ActiveX.CTmxMovie TmxMovie
        {
            get { return m_tmxMovie; }
            set { m_tmxMovie = value; }
        }

        /// <summary>Reference to application instance of TmxView control</summary>
        public FTI.Trialmax.ActiveX.CTmxView TmxView
        {
            get { return m_tmxView; }
            set { m_tmxView = value; }
        }

        /// <summary>Invalid MediaID characters</summary>
        public string InvalidMediaIdChars
        {
            get { return CASE_DATABASE_INVALID_MEDIA_ID_CHARS; }
        }

        /// <summary>Index of the MaxLineTime column in the Extents table</summary>
        public int MaxLineTimeIndex
        {
            get { return m_iMaxLineTimeIndex; }
        }

        /// <summary>Index of the SiblingId column in the TertiaryMedia table</summary>
        public int SiblingIdIndex
        {
            get { return m_iSiblingIdIndex; }
        }

        /// <summary>Index of the valuePickList column in the Codes table</summary>
        public int CodesIndexValuePickList
        {
            get { return m_iCodesIndexValuePickList; }
        }

        /// <summary>Index of the ModifiedBy column in the Codes table</summary>
        public int CodesIndexModifiedBy
        {
            get { return m_iCodesIndexModifiedBy; }
        }

        /// <summary>Index of the ModifiedOn column in the Codes table</summary>
        public int CodesIndexModifiedOn
        {
            get { return m_iCodesIndexModifiedOn; }
        }

        /// <summary>True to load media when database gets opened</summary>
        public bool FillOnOpen
        {
            get { return m_bFillOnOpen; }
            set { m_bFillOnOpen = value; }
        }

        #endregion Properties

    }// public class CTmaxCaseDatabase

}// namespace FTI.Trialmax.Database

