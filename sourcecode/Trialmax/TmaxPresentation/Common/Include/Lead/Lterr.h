/*************************************************************
   Lterr.h - error codes
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTERR_H)
#define LTERR_H

/*
   Next available error code: -1498
   Use this value for the next error code.
   Change this value everytime you create a new error code!
*/

//=================================================================
// Ltkrn
//=================================================================

/*	TRIALMAX MODIFICATION */
#if defined NOT_TRIALMAX


// General errors (logical)
typedef enum
{
   SUCCESS                          = 1,     // Function successful
   FAILURE                          = 0,     // Function not successful
   SUCCESS_DELETE                   = 3,     // Function successful. Delete file before quiting
   SUCCESS_RETRY                    = 4,     // Function successful. Retry doing the failed op
   SUCCESS_PARTIAL_FAILURE          = 8,     // Function called recursively. A least one call was successful, but some failed

   ERROR_INV_PARAMETER              = -13,   // Invalid parameter passed
   ERROR_INVALID_STRUCT_SIZE        = -789,  // The structure size is invalid
   ERROR_BITPERPIXEL                = -27,   // Invalid bits/pixel
   ERROR_INV_RANGE                  = -11,   // Invalid width/height
   ERROR_NO_BITMAP                  = -2,    // Invalid bitmap handle
   ERROR_NULL_PTR                   = -814,  // A NULL pointer was passed
   ERROR_NOT_INITIALIZED            = -30,   // Internal error - call LEAD
   ERROR_SIGNED_DATA_NOT_SUPPORTED  = -1360, // Signed image data not supported
   ERROR_BUFFER_TOO_SMALL           = -290,  // The buffer size is too small
   ERROR_EXCEPTION                  = -149,  // An unspecified exception occured - could be memory access violations
   ERROR_NOTHING_TO_DO              = -351,  // There is nothing to do.  No items was found
   ERROR_IMAGE_EMPTY                = -144,  // Image is empty
   ERROR_INTERNAL                   = -74,   // Internal error
   ERROR_QFACTOR                    = -21,   // Invalid QFactor specified
   ERROR_INV_HANDLE                 = -261,  // Invalid handle
   ERROR_BAD_POINTER                = -60,   // Internal error - call LEAD
   ERROR_GRAY32_UNSUPPORTED         = -1364, // 32-bit Grayscale bitmaps not supported
   ERROR_INVALID_LUT_LENGTH         = -1486, // The LUT length is invalid. LUTLength should be 1 << (HighBit - LowBit + 1)
   ERROR_INVALID_DIB                = -1491, // The DIB is invalid
} L_ERROR_GENERAL;

// Operation abortion errors (runtime)
typedef enum
{
   SUCCESS_ABORT        = 2,     // Function successful. You can quit now
   ERROR_USER_ABORT     = -100,  // User has aborted operation
   ERROR_NORMAL_ABORT   = -29,   // Escape key pressed - or user aborted operation
} L_ERROR_ABORTION;

// Memory errors (runtime)
typedef enum
{
   ERROR_NO_MEMORY      = -1, // Not enough memory available
   ERROR_MEMORY_TOO_LOW = -3, // Not enough memory available
} L_ERROR_MEMORY;

// Support locked errors (logical)
typedef enum
{
   ERROR_FEATURE_NOT_SUPPORTED   = -16,   // Feature not supported
   ERROR_DOCUMENT_NOT_ENABLED    = -143,  // Document capability is required to use this function
   ERROR_MEDICAL_NOT_ENABLED     = -314,  // Medical Express capability is required to use this function
   ERROR_EXTGRAY_NOT_ENABLED     = -804,  // Extended grayscale support is required to use this function
   ERROR_LTPRO_NOT_ENABLED       = -930,  // LEADTOOLS Pro Features are not enabled
   ERROR_LZW_LOCKED              = -146,  // The LZW capabilities are locked
   ERROR_JBIG_NOT_ENABLED        = -315,  // JBIG capability is required to use this function
   ERROR_JBIG2_LOCKED            = -1362, // JBIG2 file format is locked
   ERROR_J2K_LOCKED              = -542,  // J2K support is locked
   ERROR_PDF_NOT_ENABLED         = -408,  // PDF support is locked
   ERROR_CMW_LOCKED              = -354,  // The CMW support is locked
   ERROR_ABC_LOCKED              = -1361, // ABC file format is locked
   ERROR_MEDICAL_NET_NOT_ENABLED = -318,  // Medical Network Express capability is required to use this function
   ERROR_NITF_LOCKED             = -1363, // NITF support is locked
   ERROR_JPIP_LOCKED             = -1477, // JPIP support is locked
   ERROR_FORMS_LOCKED            = -1478, // FORMS support is locked
   ERROR_DOCUMENT_WRITERS_NOT_ENABLED        = -1493, // Document Writers capability is required to use this function
   ERROR_MEDIA_WRITER_NOT_ENABLED            = -1494, // Media writer capability is required to use this function
   ERROR_LEADPRINTER_NOT_ENABLED             = -1494, // LEADTOOLS Printer.Driver is required to use this function
   ERROR_DOCUMENT_WRITERS_PDF_NOT_ENABLED    = -1495, // Document Writer PDF support is locked
} L_ERROR_SUPPORT_GENERAL;

// File I/O errors (runtime)
typedef enum
{
   ERROR_FILENOTFOUND   = -10,   // File not found
   ERROR_FILE_OPEN      = -14,   // Not able to open file
   ERROR_FILE_WRITE     = -5,    // Error writing file
   ERROR_FILE_GONE      = -6,    // File not present - abort
   ERROR_FILE_READ      = -7,    // Error reading file
   ERROR_FILE_LSEEK     = -4,    // Error seeking to position
   ERROR_DISK_ISFULL    = -295,  // not enough disk space
   ERROR_FILE_READONLY  = -805,  // File is read-only. Cannot open file with write access
   ERROR_FILE_FORMAT    = -9,    // Invalid file format
   ERROR_INV_FILENAME   = -8,    // Invalid filename specified
   ERROR_FILE_CREATE    = -1490, // Error creating file
} L_ERROR_FILE_IO;

// DLL missing errors (runtime)
typedef enum
{
   ERROR_DXF_FILTER_MISSING         = -309,  // The DXF filter is needed to use this function
   ERROR_JBIG_FILTER_MISSING        = -319,  // The JBIG filter is needed to use this function
   ERROR_WMF_FILTER_MISSING         = -349,  // WMF filter is needed to use this function
   ERROR_CMW_FILTER_MISSING         = -355,  // CMW filter is needed to use this function
   ERROR_CMP_FILTER_MISSING         = -356,  // CMP filter is needed to use this function
   ERROR_FAX_FILTER_MISSING         = -357,  // FAX filter is needed to use this function
   ERROR_PDF_FILTER_MISSING         = -358,  // PDF filter is needed to use this function
   ERROR_TIF_FILTER_MISSING         = -359,  // TIF filter is needed to use this function
   ERROR_DCR_FILTER_MISSING         = -819,  // LFDCR filter is needed to use this function
   ERROR_KDC_FILTER_MISSING         = -820,  // LFKDC filter is needed to use this function
   ERROR_DCS_FILTER_MISSING         = -821,  // LFDCS filter is needed to use this function
   ERROR_ABC_FILTER_MISSING         = -822,  // LFABC filter is needed to use this function
   ERROR_ABI_FILTER_MISSING         = -823,  // LFABI (ABIC) filter is needed to use this function
   ERROR_JB2_FILTER_MISSING         = -824,  // LFJB2 (JBIG2) filter is needed to use this function
   ERROR_J2K_FILTER_MISSING         = -541,  // J2K filter is needed to use this function
   ERROR_PNG_FILTER_MISSING         = -825,  // LFPNG filter is needed to use this function
   ERROR_LTCLR_MISSING              = -807,  // LTCLR DLL is missing
   ERROR_LVKRN_MISSING              = -353,  // The LVKRN DLL is needed to use this function
   ERROR_LTSGM_MISSING              = -818,  // LTSGM DLL is missing
   ERROR_LTANN_MISSING              = -1479, // LTANN DLL is missing
   ERROR_LTIMG_MISSING              = -1480, // LTIMG DLL is missing
   ERROR_LTKRN_MISSING              = -1482, // LTKRN DLL is missing
   ERROR_LTFIL_MISSING              = -1483, // LTFIL DLL is missing
   ERROR_LTDIS_MISSING              = -1484, // LTDIS DLL is missing
   ERROR_LTDIC_MISSING              = -1485, // LTDIC DLL is missing
   ERROR_BMP_FILTER_MISSING         = -1496, // LFBMP DLL is missing
   ERROR_RAW_FILTER_MISSING         = -1497, // LFRAW DLL is missing

} L_ERROR_MISSING;

// Overlay errors (logical)
typedef enum
{
   ERROR_NO_OVERLAY     = -815,  // The overlay does not exist
   ERROR_OVERLAY_INDEX  = -816,  // Something is wrong with the overlay index
} L_ERROR_OVERLAY;

//=================================================================
// Ltfil
//=================================================================

// Ltfil general errors (logical)
typedef enum
{
   ERROR_WINDOWSIZE        = -28,   // Invalid window size
   ERROR_INV_FILTERNAME    = -1353, // Invalid filter name
   ERROR_PAGE_NOT_FOUND    = -310,  // Page not found
   ERROR_DELETE_LAST_PAGE  = -311,  // You cannot delete a page from a file containing only one page
   ERROR_IMAGE_TYPE        = -12,   // Image format recognized, but sub-type not supported
   ERROR_BAD_DECODE_STATE  = -77,   // Premature end-of-line code
   ERROR_LAYER_MISSING     = -800,  // The PSD Layer is missing
   ERROR_OUTPUTTYPE        = -23,   // Invalid compression format
   ERROR_VERSION_NUMBER    = -78,   // Bad version number
   ERROR_NO_THUMBNAIL      = -520,  // Thumbnail not found
   ERROR_CHANNEL_MISSING   = -1492  // The PSD Channel is missing
} L_ERROR_LTFIL;

// J2K errors (logical)
typedef enum
{
   ERROR_JP2_FAILURE             = -530,  // Error in JP2 Box values
   ERROR_JP2_SIGNATURE           = -531,  // The header does not match the JP2 signature - not a JP2 file
   ERROR_JP2_UNSUPPORTED         = -532,  // JP2 file has a feature that is usupported
   ERROR_J2K_FAILURE             = -533,  // Invalid save options were specified to the encoder
   ERROR_J2K_NO_SOC              = -534,  // File header does not contain SOC marker
   ERROR_J2K_NO_SOT              = -535,  // File contains complete header but no compressed image data
   ERROR_J2K_INFORMATION_SET     = -536,  // Invalid save options were specified or file includes invalid encoded values
   ERROR_J2K_LOW_TARGET_SIZE     = -537,  // Compression ratio, target file size, or tile size was too small for encoder
   ERROR_J2K_DECOMPOSITION_LEVEL = -538,  // Specified Wavelet decomposition level was too high
   ERROR_J2K_MARKER_VALUE        = -539,  // Decoder could not translate J2K marker - file is corrupt or invalid
   ERROR_J2K_UNSUPPORTED         = -540,  // J2K file has image with more than 30 bits per component
} L_ERROR_J2K;

// Stamp errors (logical)
typedef enum
{
   ERROR_NO_STAMP             = -65,   // stamp not found
   ERROR_INVALID_STAMP_SIZE   = -141,  // Stamp size is too big or invalid bits/pixel, etc
   ERROR_BAD_STAMP            = -142,  // Stamp is present but data is corrupted
} L_ERROR_STAMP;

// Marker callback status (runtime)
typedef enum
{
   SUCCESS_IGNORETHISCOPYALL  = 7,  // Function successful. Ignore this marker but copy all remaining markers
   SUCCESS_IGNOREALLMARKERS   = 6,  // Function successful. Ignore this marker and all remaining markers
   SUCCESS_IGNOREMARKER       = 5,  // Function successful. Ignore this marker
} L_ERROR_MARKER_STATUS;

// Marker errors (logical)
typedef enum
{
   ERROR_BAD_MARKER           = -801,  // Bad JPEG marker
   ERROR_BAD_RESYNC_MARKER    = -806,  // Bad JPEG Resync marker
   ERROR_MARKER_SIZE_TOO_BIG  = -808,  // The size of the JPEG marker cannot exceed 64K
   ERROR_MARKER_MISSING       = -809,  // The required JPEG marker is missing
   ERROR_MARKER_INDEX         = -811,  // The marker index is invalid (too big)
} L_ERROR_MARKER;

// Extension errors (logical)
typedef enum
{
   ERROR_AUDIO_MISSING           = -802,  // The audio data is not present
   ERROR_EXTENSIONS_MISSING      = -810,  // This file does not contain Exif extensions
} L_ERROR_EXTENSION;

//=================================================================
// Ltclr
//=================================================================

// Ltclr general errors (logical)
typedef enum
{
   ERROR_INV_COLORSPACE                               = -918,  // Invalid colorspace
   ERROR_INVALID_FORMAT                               = -785,  // Invalid format
   ERROR_UNSUPPORTED_METHOD                           = -786,  // Method not supported
   ERROR_OPENING_PROFILE                              = -787,  // Error opening profile
   ERROR_INVALID_COLOR_PROFILE                        = -788,  // Invalid color profile
   ERROR_U_V_NOT_MULTIPLES                            = -791,  // U and V not multiples
   ERROR_NO_NONPLANAR_VERTICAL_SUBSAMPLING_SUPPORTED  = -792,  // No non planar vertical sub sampling supported
   ERROR_PLANAR_ALIGNMENT_NOT_SUPPORTED               = -793,  // Planar alignment not supported
   ERROR_UNSUPPORTED_CONVERSION                       = -794,  // Unsupported conversion
   ERROR_TRUNCATE_HEIGHT                              = -795,  // Truncate height
   ERROR_TRUNCATE_WIDTH                               = -796,  // Truncate width
   ERROR_TRUNCATE_WIDTH_AND_HEIGHT                    = -797,  // Truncate width and height
} L_ERROR_LTCLR;

// Ltclr ICC errors (logical)
typedef enum
{
   ERROR_ICC_UNKNOWN_TAG            = -1150, // Unknown ICC profile Tag
   ERROR_ICC_UNKNOWN_TYPE           = -1151, // Unknown ICC profile Type
   ERROR_ICC_UNKNOWN_TAG_AND_TYPE   = -1152, // Unknown ICC profile Tag and Type
   ERROR_INVALID_ICC_PROFILE        = -1153, // Invalid ICC profile
   ERROR_ICC_IMAGE_NOT_SUPPORTED    = -1154, // Invalid ICC profile
   ERROR_NO_PROFILE                 = -812,  // The ICC profile was not found
   ERROR_CORRUPT_PROFILE            = -817,  // The ICC profile was invalid
   ERROR_DECODING_PROFILE           = -813,  // An error has occured while decoding the profile
} L_ERROR_ICC;

//=================================================================
// Ltdis
//=================================================================

// Pan window errors (logical)
typedef enum
{
   ERROR_PANWINDOW_NOT_CREATED   = -91,   // PanWindow creation failed
} L_ERROR_PANWIN;

//=================================================================
// Lttmb
//=================================================================

// Browser errors
typedef enum
{
   ERROR_BROWSE_FAILED  = -350,  // PanWindow creation failed
} L_ERRORL_LTTMB;

//=================================================================
// Ltdic
//=================================================================
typedef enum
{
   ERROR_DICOM_NOT_ENABLED = -803,  // DICOM support is required to use this function
} L_ERROR_SUPPORT_DICOM;

//=================================================================
// Pdf
//=================================================================
typedef enum
{
   ERROR_PDF_FILE_ENCRYPTED               = -721,  // The Pdf file is encrypted and the szPassword member of the FILEPDFOPTIONS is not set  (no password).
   ERROR_PDF_INVALID_PASSWORD             = -722,  // Invalid password specified
   ERROR_PDF_FAX_NOT_ENABLED              = -723,  // FAX is required for this function
   ERROR_PDF_JPEG_NOT_ENABLED             = -724,  // CMP is required for this function
   ERROR_PDF_INV_DOC_STRUCTURING_COMMENTS = -725,  // Invalid Document Structuring comments (Ps and EPS)
   ERROR_PDF_FONTS_DIRECTORY_NOT_FOUND    = -726,  // Could not find Fonts directory
   ERROR_PDF_CANNOT_EDIT_FILE             = -727,  // Cannot insert, delete, append or replace pages.
   ERROR_PDF_BAD_CONTENT                  = -728,  // File is corrupted
   ERROR_PDF_BAD_INITIALIZATION_FILES     = -729,  // Either the files required for initializing the PDF engine were not not found or they were found but they are incorrect
} L_ERROR_RASTERPDF;

//=================================================================
// Ltimg
//=================================================================
typedef enum
{
   ERROR_NO_CHANGE          = -145,  // The image hasn't changed
   ERROR_IMAGE_SIZE         = -910,  // Image size not sufficient
   ERROR_NO_MESSAGE         = -911,  // There is no message
   ERROR_INV_PASSWORD       = -912,  // Invalid password
   ERROR_NOT_ENOUGH_IMAGES  = -92,   // There are not enough images to complete the task
} L_ERROR_IMG;

//=================================================================
// Ltimgopt
//=================================================================
typedef enum
{
   ERROR_OPT_ABORT                     = -1100, // Abort the optimization operation
   ERROR_OPT_SKIPIMAGE                 = -1101, // Skip optimizing current image
   ERROR_OPT_INVALID_INPUT_PATH        = -1102, // Invalid input path
   ERROR_OPT_INVALID_OUTPUT_PATH       = -1103, // Invalid output path
   ERROR_OPT_CANNOT_CREATE_OUTPUTPATH  = -1104, // Cannot Create Output Directory
} L_ERROR_IMGOPT;

//=================================================================
// Ltnet
//=================================================================
typedef enum
{
   ERROR_HOST_RESOLVE      = -270,  // Can't resolve hostname
   ERROR_CANT_INITIALIZE   = -271,  // Can't initialize network
   ERROR_NO_CONNECTION     = -272,  // No open network connections
   ERROR_HOST_NOT_FOUND    = -273,  // Hostname not found
   ERROR_NOT_SERVER        = -274,  // Computer is not a server
   ERROR_NO_CONNECTIONS    = -275,  // No connection requests waiting
   ERROR_CONNECT_REFUSED   = -276,  // Connection was refused
   ERROR_IS_CONNECTED      = -277,  // computer is already connected
   ERROR_NET_UNREACH       = -278,  // network is unreachable from host
   ERROR_TIME_OUT          = -279,  // connect attempt time out without connecting
   ERROR_NET_DOWN          = -280,  // network subsystem is down
   ERROR_NO_BUFFERS        = -281,  // no buffer space is available, too many connections
   ERROR_NO_FILE_DESCR     = -282,  // no more file handle available
   ERROR_DATA_QUEUED       = -283,  // data has been queued to send later
   ERROR_UNKNOWN           = -284,  // unkown network error has occured
   ERROR_CONNECT_RESET     = -285,  // connection was reset - socket might be unusable
   ERROR_TRANSFER_ABORTED  = -286,  // transfer was aborted
   ERROR_DSHOW_FAILURE     = -287,  // DirectShow isn't installed correctly
   ERROR_REGISTRY_READ     = -288,  // Error reading from the registry
} L_ERROR_NET;

//=================================================================
// Ltdlg
//=================================================================
typedef enum
{
   SUCCESS_DLG_OK             = 100,   // The "OK" button was pressed, and the dialog exited successfully
   SUCCESS_DLG_CANCEL         = 101,   // The "Cancel" button was pressed, and the dialog exited successfully.
   SUCCESS_DLG_CLOSE          = 102,   // The "Close" button was pressed, and the dialog exited successfully.
   SUCCESS_DLG_EXIT           = 103,   // The dialog exits successfully after selecting exit option from menu or by closing the window.
   SUCCESS_DLG_EXPORTANDEXIT  = 104,   // The dialog exits successfully after selecting exit and export option, dialog parameters will have the resulting bitmap allocated.
} L_ERROR_DLG_SUCCESS;

typedef enum
{
   ERROR_DLG_FAILED                 = -150,  // An error occured during creation of the common dialog
   ERROR_DLG_ALREADYINITIATED       = -151,  // You have already initialized the dialogs
   ERROR_DLG_NOTINITIATED           = -152,  // You did not initialize the dialogs
   ERROR_DLG_COLORNOTINITIALIZED    = -153,  // You did not initialize the dialogs with DLG_INIT_COLOR
   ERROR_DLG_COLORDLL_UNABLETOLOAD  = -154,  // The dialogs could not load the LTCLR dll
   ERROR_DLG_RESDLL_UNABLETOLOAD    = -155,  // The dialogs could not load the LTDLGRES dll
   ERROR_LTCLR_DLL_NOTLOADED        = -900,  // The LTCLR DLL is not loaded
   ERROR_LTDLGRES_DLL_NOTLOADED     = -901,  // The LDLGRES DLL is not loaded
   ERROR_LTDLG_COLOR_NOTINITIALIZE  = -902,  // You must initialize LTDLG using L_DlgInit
} L_ERROR_DLG;

//=================================================================
// Ltann
//=================================================================
typedef enum
{
   ERROR_LTANN_NOT_LOADED        = -202,  // Annotation DLL not loaded
   ERROR_LTDIS_NOT_LOADED        = -200,  // LTDIS DLL not loaded
   ERROR_LTFIL_NOT_LOADED        = -201,  // LTFIL DLL not loaded
   ERROR_NO_VIDEO_MODULE         = -258,  // No video module present on machine
   ERROR_UNDO_STACK_EMPTY        = -316,  // Undo stack is empty - too many undos
   ERROR_NO_UNDO_STACK           = -406,  // There is no undo stack
   ERROR_UNDO_DISABLED           = -407,  // The undo is disabled
   ERROR_ANN_LOCKED              = -148,  // One or more annotation objects are still locked
} L_ERROR_ANN;

//=================================================================
// Lftif
//=================================================================
typedef enum
{
   ERROR_UNKNOWN_COMP      = -15,   // Unknown compression format
   ERROR_COMPACT_ABORTED   = -917,  // The compact function was finished without copying all the pages
   ERROR_BAD_TAG           = -140,  // Bad TIFF tag
   ERROR_TAG_MISSING       = -404,  // Tag not found
   ERROR_CANNOT_INCREASE_IFD_SIZE = -1365,   // Cannot add tags to a IFD selected using SAVEFILEOPTION.IFD
} L_ERROR_TIF;

//=================================================================
// Lfcmp
//=================================================================
typedef enum
{
   ERROR_CU_BUSY                 = -31,   // Internal error - call LEAD
   ERROR_INVALID_TABLE_TYPE      = -32,   // Internal error - call LEAD
   ERROR_INVALID_BUFFER          = -34,   // Internal error - call LEAD
   ERROR_MISSING_TILE_DATA       = -35,   // Internal error - call LEAD
   ERROR_INVALID_QVALUE          = -36,   // Internal error - call LEAD
   ERROR_INVALIDDATA             = -37,   // Internal error - call LEAD
   ERROR_INVALID_COMPRESSED_TYPE = -38,   // Internal error - call LEAD
   ERROR_INVALID_COMPONENT_NUM   = -39,   // Internal error - call LEAD
   ERROR_INVALID_PIXEL_TYPE      = -40,   // Internal error - call LEAD
   ERROR_INVALID_PIXEL_SAMPLING  = -41,   // Internal error - call LEAD
   ERROR_INVALID_IMAGE_DIMS      = -44,   // Internal error - call LEAD
   ERROR_INVALID_TILE_DIMS       = -45,   // Internal error - call LEAD
   ERROR_INVALID_PIX_BUFF_DIMS   = -46,   // Internal error - call LEAD
   ERROR_SEGMENT_OVERFLOW        = -47,   // Internal error - call LEAD
   ERROR_INVALID_SUBSAMPLING     = -48,   // Internal error - call LEAD
   ERROR_INVALID_Q_VIS_TABLE     = -49,   // Internal error - call LEAD
   ERROR_INVALID_DC_CODE_TABLE   = -50,   // Internal error - call LEAD
   ERROR_INVALID_AC_CODE_TABLE   = -51,   // Internal error - call LEAD
   ERROR_INSUFFICIENT_DATA       = -52,   // Internal error - call LEAD
   ERROR_MISSING_FUNC_POINTER    = -53,   // Internal error - call LEAD
   ERROR_TOO_MANY_DC_CODE_TABLES = -54,   // Internal error - call LEAD
   ERROR_TOO_MANY_AC_CODE_TABLES = -55,   // Internal error - call LEAD
   ERROR_INVALID_SUBIMAGE        = -56,   // Internal error - call LEAD
   ERROR_INVALID_ABORTION        = -57,   // Internal error - call LEAD
   ERROR_CU_NO_SUPPORT           = -58,   // Internal error - call LEAD
   ERROR_CU_FAILURE              = -59,   // Internal error - call LEAD
   ERROR_COMPRESSED_DATA_FAILURE = -62,   // Internal error - call LEAD
} L_ERROR_CMP;

//=================================================================
// Ltdoc
//=================================================================
typedef enum
{
   ERROR_DOC_DLL_NOT_LOADED                  = -1200, // ScanSoft DLLs are not loaded
   ERROR_DOC_INV_PAGE_INDEX                  = -1201, // Specified index is not corrected
   ERROR_DOC_INV_FILLMETHOD                  = -1202, // invalid fill method
   ERROR_DOC_INV_RECOG_MODULE                = -1203, // Invalid recognition module
   ERROR_DOC_INV_CHAR_FILTER                 = -1204, // invalid character filter
   ERROR_DOC_INV_ZONE_TYPE                   = -1205, // invalid zone type
   ERROR_DOC_INV_LANGID                      = -1206, // invalid language Id
   ERROR_DOC_INV_SPELL_LANGUAGE              = -1207, // invalid spell language
   ERROR_DOC_INV_SECTION_ENUMERATION         = -1208, // invalid enumeration section
   ERROR_DOC_INV_PAGE_COUNT                  = -1209, // invalid page count
   ERROR_DOC_ILLEGAL_CODE                    = -1210, // Illegal internal code as a parameter
   ERROR_DOC_INV_PARAMETER                   = -1211, // Set parameter is not acceptable
   ERROR_DOC_END_LIST_CODE_PAGES             = -1212, // End of list of the available Code Pages
   ERROR_DOC_LOAD_CODE_PAGE_FILE             = -1213, // Error while loading the Code Page Definition file
   ERROR_DOC_INV_BUFFER_SIZE                 = -1214, // The length of the exported code exceeds the buffer size
   ERROR_DOC_INV_CHARACTER_CONVERSION        = -1215, // Character conversion is not available for the given character
   ERROR_DOC_INV_CHARACTER_LANGUAGE          = -1216, // Conflict: The selected Code Page does not support some characters in the selected languages. There is no exact code in the Code Page for them
   ERROR_DOC_INITIALIZE_CODE_PAGE            = -1217, // Character Set and Code Pages module initialization error
   ERROR_DOC_INITIALIZE_ENGINE               = -1218, // OCR engine initialization error, or there is no appropriate license
   ERROR_DOC_TERMINATE_ENGINE                = -1219, // OCR engine termination error
   ERROR_DOC_INITIALIZE_WARNING              = -1220, // Module initialization warning
   ERROR_DOC_PROCESS_USER_ABORT              = -1221, // Application has aborted the current process
   ERROR_DOC_FINISH_PROCESS                  = -1222, // Application has terminated the current recognition process without losing the recognized text
   ERROR_DOC_MODULE_MISSING                  = -1223, // Module is not present
   ERROR_DOC_MODULE_LOAD                     = -1224, // OS could not load a module
   ERROR_DOC_MODULE_MISSING_ENTRY            = -1225, // Missing entry in a module
   ERROR_DOC_MODULE_INVALID                  = -1226, // Invalid module
   ERROR_DOC_MODULE_INITIALIZE               = -1227, // Module initialization error
   ERROR_DOC_FEATURE_NOT_SUPPORTED           = -1228, // The requested function is not available, or there is no appropriate license
   ERROR_DOC_GENERAL                         = -1229, // General error in the engine
   ERROR_DOC_GPF                             = -1230, // General Protection Fault in the engine
   ERROR_DOC_NOT_SUPPORT_OS                  = -1231, // Not supported operational system
   ERROR_DOC_INV_SETTING_FILE                = -1232, // Syntax error in the specified engine Settings file
   ERROR_DOC_INV_SETTING                     = -1233, // Invalid setting
   ERROR_DOC_ENGINE_BUSY                     = -1234, // The engine is busy
   ERROR_DOC_RECOGNITION_TIME_OUT            = -1235, // Recognition process Time Out
   ERROR_DOC_INV_IMAGE_MODULE_SETTING        = -1236, // Internal error in image module
   ERROR_DOC_NO_MEMORY                       = -1237, // Not enough memory during image processing
   ERROR_DOC_INV_IMAGE_DIMENSIONS            = -1238, // Invalid rectangle dimensions
   ERROR_DOC_INV_IMAGE_RESOLUTION            = -1239, // Non-supported resolution
   ERROR_DOC_CANNOT_COMPRESS_IMAGE           = -1240, // Cannot process compressed image
   ERROR_DOC_BAD_BITMAP                      = -1241, // Invalid bitmap address
   ERROR_DOC_NOT_SUPPORT_BPP                 = -1242, // Unsupported BitsPerPixel value
   ERROR_DOC_INV_IMAGE_MODULE                = -1243, // Internal error in image module
   ERROR_DOC_INV_IMAGE_HANDLE                = -1244, // Invalid image handle
   ERROR_DOC_BUFFER_OVERFLOW                 = -1245, // Buffer overflow during processing the image
   ERROR_DOC_ACCESS_DENIED                   = -1246, // Image operation denied
   ERROR_DOC_NO_MORE_LINES                   = -1247, // No more lines in defined image area
   ERROR_DOC_BAD_IMAGE_SIZE                  = -1248, // Non-supported image size
   ERROR_DOC_BAD_ENGINE_MANAGER_MODULE       = -1249, // Engine manager module error
   ERROR_DOC_NO_RECOGNIZED_TEXT_AVAILABLE    = -1250, // No recognized text available, either because the zone is empty or the required recognition module has not been initialized properly
   ERROR_DOC_NO_SELECTED_RECOGNITION_MODULE  = -1251, // There is no selected recognition module
   ERROR_DOC_NO_ZONE                         = -1252, // There is no zone in the zone list
   ERROR_DOC_INV_ZONE_INDEX                  = -1253, // Invalid zone index
   ERROR_DOC_INV_ZONE_COORDINATES            = -1254, // Invalid zone coordinates
   ERROR_DOC_MOR_INITIALIZE_MODULE           = -1255, // MOR recognition module initialization error
   ERROR_DOC_MOR_BASE_FILE_NOT_FOUND         = -1256, // MOR recognition module's knowledge base file not found
   ERROR_DOC_MOR_BASE_FILE_CORRUPTED         = -1257, // MOR recognition module's knowledge base file corrupted
   ERROR_DOC_MOR_BASE_FILE_VERSION           = -1258, // Incorrect knowledge base file version
   ERROR_DOC_MOR_IMAGE_SIZE                  = -1259, // Size of image is too large
   ERROR_DOC_MOR_FILE_CORRUPTED              = -1260, // File is corrupted
   ERROR_DOC_MOR_BAD_RECOG_MODULE            = -1261, // Internal error in the MOR recognition module
   ERROR_DOC_MOR_GPF_MODULE                  = -1262, // General Protection Fault in the MOR recognition module
   ERROR_DOC_DOT_RECOGNITION                 = -1263, // Dot-matrix recognition error
   ERROR_DOC_OMR_CHECKMARK_RECOGNITION       = -1264, // Checkmark recognition error
   ERROR_DOC_HNR_BASE_FILE__CORRUPTED        = -1265, // HNR module's knowledge base file is corrupted
   ERROR_DOC_HNR_PARAM_OUT_RANGE             = -1266, // Parameter is out of valid range
   ERROR_DOC_RER_MODULE                      = -1267, // Internal error in the RER module
   ERROR_DOC_RER_BASE_FILE_NOT_FOUND         = -1268, // RER module's knowledge base file not found
   ERROR_DOC_RER_CHAR_SET_EMPTY              = -1269, // Character Set is empty for the RER module
   ERROR_DOC_RER_CHAR_SET_NOT_SUPPORTED      = -1270, // The specified Character Set not fully supported by the RER recognition module
   ERROR_DOC_RER_MODULE_NOT_FOUND            = -1271, // RER specific file not found
   ERROR_DOC_SPELL_NO_MEMORY                 = -1272, // Not enough memory for the SPL module
   ERROR_DOC_SPELL_UNINITIALIZED             = -1273, // Uninitialized spell object
   ERROR_DOC_SPELL_FILE_OPEN                 = -1274, // Spell file open error
   ERROR_DOC_SPELL_FILE_READ                 = -1275, // Spell file read error
   ERROR_DOC_SPELL_USER_DICTIONARY_WRITE     = -1276, // User dictionary write error
   ERROR_DOC_SPELL_INV_FILE_FORMAT           = -1277, // Invalid file format
   ERROR_DOC_SPELL_INITIALIZE_MODULE         = -1278, // Module initialization error
   ERROR_DOC_SPELL_USER_DICTIONARY_CLOSE     = -1279, // User dictionary close error
   ERROR_DOC_SPELL_ILLEGAL_LANGUAGE_SETTING  = -1280, // Illegal language setting
   ERROR_DOC_SPELL_NO_MORE_ITEMS             = -1281, // No more suggestions/items available
   ERROR_DOC_SPELL_CHECK_ERROR               = -1282, // Internal checking error
   ERROR_DOC_SPELL_ITEM_ALREADY_EXIST        = -1283, // Item already exists in the User dictionary
   ERROR_DOC_SPELL_ITEM_NOT_EXIST            = -1284, // Item doesn't exist in the User dictionary
   ERROR_DOC_SPELL_ITEM_NOT_INSERTED         = -1285, // Item wasn't inserted into the User dictionary
   ERROR_DOC_SPELL_SECTION_NOT_EXIST         = -1286, // Section does not exist in the User dictionary
   ERROR_DOC_SPELL_BAD_SYNTEX                = -1287, // Regular expression syntax error
   ERROR_DOC_OUTPUT_NOT_INITIALIZED          = -1288, // Output format conversion subsystem was not initialized
   ERROR_DOC_OUTPUT_CONVERT_NOT_EXIST        = -1289, // No more converters available
   ERROR_DOC_OUTPUT_CONVERT_DLL_NOT_EXIST    = -1290, // There is no converter DLL file
   ERROR_DOC_OUTPUT_BAD_PARAMETER            = -1291, // Wrong parameter or parameter not found
   ERROR_DOC_OUTPUT_FILE_CORRUPTED           = -1292, // TMP file is corrupted
   ERROR_DOC_OUTPUT_FILE_NOT_FOUND           = -1293, // TMP file not found
   ERROR_DOC_OUTPUT_CREATE_FILE              = -1294, // Cannot create TMP file
   ERROR_DOC_OUTPUT_SEEK_FILE                = -1295, // Cannot seek in TMP file
   ERROR_DOC_OUTPUT_READ_FILE                = -1296, // TMP file read error
   ERROR_DOC_OUTPUT_WRITE_FILE               = -1297, // TMP file write error
   ERROR_DOC_OUTPUT_CLOSE_FILE               = -1298, // Cannot close TMP file
   ERROR_DOC_OUTPUT_LOAD_DLL                 = -1299, // Cannot load the DLL file
   ERROR_DOC_OUTPUT_END_OF_FILE              = -1300, // Internal error in ATMTXT module. (End-Of-File detected)
   ERROR_DOC_OUTPUT_NO_MEMORY                = -1301, // Not enough memory for ATMTXT/TMP module
   ERROR_DOC_OUTPUT_IMAGE_NOT_AVAILABLE      = -1302, // Image is not available for inserting into the output
   ERROR_DOC_OUTPUT_NOT_REGISTER_INFO_ITEM   = -1303, // Not enough memory to register an info item
   ERROR_DOC_TXT_FILE_OPEN                   = -1304, // Output file open error
   ERROR_DOC_TXT_FILE_READ                   = -1305, // Output file read error
   ERROR_DOC_TXT_FILE_NOT_FOUND              = -1306, // Output file not found
   ERROR_DOC_TXT_FILE_WRITE                  = -1307, // Output file write error
   ERROR_DOC_TXT_INV_COMMAND_SEQUENCE        = -1308, // Invalid command sequence in the ICF file
   ERROR_DOC_TXT_CREATE_FILE                 = -1309, // Cannot create the output file
   ERROR_DOC_TXT_NO_MEMORY                   = -1310, // Not enough memory for ATMTXT/TXT module
   ERROR_DOC_TXT_INV_DOCUMENT                = -1311, // Invalid document type
   ERROR_DOC_TXT_FILE_TOO_LARGE              = -1312, // File too large
   ERROR_DOC_TXT_WARNING_OUTPUT_FILE         = -1313, // There was a WARNING during the output file conversion
   ERROR_DOC_TXT_OPERATION_CANCELLED         = -1314, // Operation cancelled
   ERROR_DOC_TXT_ILLEGAL_OPTION              = -1315, // Illegal option specified
   ERROR_DOC_TXT_ENCRYPTED_SOURCE            = -1316, // Encrypted source
   ERROR_DOC_TXT_SEEK_ERROR                  = -1317, // Output file seek error
   ERROR_DOC_BRAILLE_BAD_INITIALIZATION      = -1318, // Braille recognition module initialization error
   ERROR_DOC_MATRIX_BAD_INITIALIZATION       = -1319, // Matrix matching recognition module initialization error
   ERROR_DOC_MTX_BAD_INITIALIZATION          = -1320, // M/TEXT recognition module initialization error
   ERROR_DOC_NON_IMPLEMENTED_FEATURE         = -1321, // Non-implemented feature
   ERROR_DOC_NOT_FOUND_ZONE                  = -1322, // couldn't find specific zone
   ERROR_DOC_NO_SELECTED_ZONE                = -1323, // No available selected zone
   ERROR_DOC_EXPORT_PAGE                     = -1324, // can't export the specified page
   ERROR_DOC_LOCK_PAGE                       = -1325, // can't lock the specified page
   ERROR_DOC_ACTIVE_PAGE                     = -1326, // can't set the specified page to be activated.
   ERROR_DOC_LOCKED                          = -1327, // The OCR document engine is locked.
   ERROR_DOC_OUTPUT_PDF_LOCKED               = -1328, // Output PDF engine is locked.
   ERROR_DOC_OMR_LOCKED                      = -1329, // OMR Options is locked
   ERROR_DOC_ICR_LOCKED                      = -1330, // ICR Options is locked

   // Error codes added for Arabic OCR Engine...
   ERROR_DOC_ARABIC_LOCKED                   = -1331, // The Arabic OCR document engine is locked.
   ERROR_DOC_TOO_MANY_REGION                 = -1332, // Too many regions result when layout analysis
   ERROR_DOC_NO_HORIZONTAL_BOUNDARY          = -1333, // Top or bottom horizontal line not found
   ERROR_DOC_SLOPE_LINE_TOO_LARGE            = -1334, // The slope of line too large
   ERROR_DOC_FRAME_NOT_DETECTED              = -1335, // Frame has not been detected, so can not add virtualline
   ERROR_DOC_TOO_MANY_LINES                  = -1336, // Too many lines
   ERROR_DOC_TOO_MANY_PAGES                  = -1337, // Too many pages in batch processing list
   ERROR_DOC_NO_TABLE                        = -1338, // This region is not a table region
   ERROR_DOC_INVALID_TABLE_DATA              = -1339, // This region is a table region but contains no data
   ERROR_DOC_INVALID_FRAME_LINE              = -1340, // This line is not valid  (neither horizontal nor vertical or type is not match)
   ERROR_DOC_TOO_LARGE_ANGLE                 = -1341, // Angle is too large, can not rotate image
   ERROR_DOC_ORIENT                          = -1342, // Unable to determine orientation

   ERROR_DOC_INV_RDF_FILENAME                = -1343, // Invalid RDF file name
   ERROR_DOC_LTPDF_DLL_NOT_LOADED            = -1344, // LEAD PDF DLLs are not loaded
   ERROR_DOC_OCR_PDF_LEAD_OUTPUT_LOCKED      = -1345, // OCR PDF LEAD Output is locked.

   ERROR_DOC_ICR_MISSING                     = -1346, // ICR Module is missing
   ERROR_DOC_OMR_MISSING                     = -1347, // OMR Module is missing
   ERROR_DOC_LANGUAGE_MISSING                = -1348, // Languages files are missing

   ERROR_DOC2_INITIALIZE_ENGINE  = -1349, // OCR Professional engine initialization error, or there is no appropriate license
   ERROR_DOC2_ICR_MISSING        = -1350, // ICR Module is missing for Professional OCR Engine
   ERROR_DOC2_OMR_MISSING        = -1351, // OMR Module is missing for Professional OCR Engine
   ERROR_DOC2_LANGUAGE_MISSING   = -1352, // Languages files are missing for Professional OCR Engine
   ERROR_DOC2_ASIAN_LOCKED       = -1353, // Asian module is locked

} L_ERROR_SUPPORT_DOC;

//=================================================================
// Lttwn
//=================================================================
typedef enum
{
   ERROR_TWAIN_BUMMER                     = -80,  // TWAIN Failure due to unknown causes
   ERROR_TWAIN_LOWMEMORY                  = -81,  // TWAIN Not enough memory to perform operation
   ERROR_TWAIN_NODS                       = -82,  // TWAIN No Data Source
   ERROR_TWAIN_MAXCONNECTIONS             = -83,  // TWAIN DS is connected to max possible apps
   ERROR_TWAIN_OPERATIONERROR             = -84,  // TWAIN DS or DSM reported error, app shouldn't (no need for your app to report the error)
   ERROR_TWAIN_BADPROTOCOL                = -86,  // TWAIN Unrecognized MSG DG DAT combination
   ERROR_TWAIN_CANCEL                     = -90,  // TWAIN Operation was cancelled

   ERROR_TWAIN_NO_LIBRARY                  = -560,  // No TWAIN Library
   ERROR_TWAIN_INVALID_DLL                 = -561,  // Invalid DLL
   ERROR_TWAIN_NOT_INITIALIZED             = -562,  // TWAIN is not initialized

   ERROR_TWAIN_CHECK_STATUS                = -564,  // Check status
   ERROR_TWAIN_END_OF_LIST                 = -565,  // End of list
   ERROR_TWAIN_CAP_NOT_SUPPORTED           = -566,  // Cap is not supported
   ERROR_TWAIN_SOURCE_NOT_OPEN             = -567,  // Source is not open
   ERROR_TWAIN_BAD_VALUE                   = -568,  // Bad value
   ERROR_TWAIN_INVALID_STATE               = -569,  // Invalid state
   ERROR_TWAIN_CAPS_NEG_NOT_ENDED          = -570,  // Caps neg not ended
   ERROR_TWAIN_OPEN_FILE                   = -571,  // Open file
   ERROR_TWAIN_INV_HANDLE                  = -572,  // Invalid handle
   ERROR_TWAIN_WRITE_TO_FILE               = -573,  // Write to file
   ERROR_TWAIN_INV_VERSION_NUM             = -574,  // Invalid version number
   ERROR_TWAIN_READ_FROM_FILE              = -575,  // Read from file
   ERROR_TWAIN_NOT_VALID_FILE              = -576,  // File is not valid
   ERROR_TWAIN_INV_ACCESS_RIGHT            = -577,  // Invalid access right
   ERROR_TWAIN_CUSTOMBASE                  = -578,  // Custom base
   ERROR_TWAIN_DENIED                      = -579,  // Denied
   ERROR_TWAIN_FILEEXISTS                  = -580,  // File exists
   ERROR_TWAIN_FILENOTFOUND                = -581,  // File not found
   ERROR_TWAIN_NOTEMPTY                    = -582,  // Not empty
   ERROR_TWAIN_PAPERJAM                    = -583,  // Paper jam
   ERROR_TWAIN_PAPERDOUBLEFEED             = -584,  // Paper double feed
   ERROR_TWAIN_FILEWRITEERROR              = -585,  // File write error
   ERROR_TWAIN_CHECKDEVICEONLINE           = -586,  // Check device on line

   ERROR_TWAIN_STOP_ENUMERATION            = SUCCESS_ABORT,  // Stop enumeration
   ERROR_TWAIN_STOP_SCAN                   = -587,  // Stop scan
} L_ERROR_TWAIN;

//=================================================================
// Lttwn
//=================================================================
typedef enum
{
   ERROR_BARCODE_DIGIT_CHECK              = -410,  // Invalid error check digit
   ERROR_BARCODE_INVALID_TYPE             = -411,  // Invalid bar code type
   ERROR_BARCODE_TEXTOUT                  = -412,  // Invalid bar code text out option
   ERROR_BARCODE_WIDTH                    = -413,  // Invalid bar code width
   ERROR_BARCODE_HEIGHT                   = -414,  // Invalid bar code height
   ERROR_BARCODE_TOSMALL                  = -415,  // Bar code string is too small
   ERROR_BARCODE_STRING                   = -416,  // Invalid bar code string for a specified bar code type
   ERROR_BARCODE_NOTFOUND                 = -417,  // No bar code recognition
   ERROR_BARCODE_UNITS                    = -418,  // Invalid bar code measurement unit
   ERROR_BARCODE_MULTIPLEMAXCOUNT         = -419,  // Invalid multiple max count
   ERROR_BARCODE_GROUP                    = -420,  // Invalid bar code group
   ERROR_BARCODE_NO_DATA                  = -421,  // Invalid BARCODEDATA structure
   ERROR_BARCODE_NOTFOUND_DUPLICATED      = -422,  // No duplicated bar code
   ERROR_BARCODE_LAST_DUPLICATED          = -423,  // Reached the last duplicated bar code
   ERROR_BARCODE_STRING_LENGTH            = -424,  // Invalid bar code data string length
   ERROR_BARCODE_LOCATION                 = -425,  // Invalid bar code area location
   ERROR_BARCODE_1D_LOCKED                = -426,  // Bar Code 1D is locked
   ERROR_BARCODE_2D_READ_LOCKED           = -427,  // Bar Code Read 2D (CodeOne) is locked
   ERROR_BARCODE_2D_WRITE_LOCKED          = -428,  // Bar Code Write 2D (CodeOne) is locked
   ERROR_BARCODE_PDF_READ_LOCKED          = -429,  // Bar Code Read PDF is locked
   ERROR_BARCODE_PDF_WRITE_LOCKED         = -430,  // Bar Code Write PDF is locked
   ERROR_BARCODE_FOUNDCORRUPT             = -431,  // Barcode PDF417 symbol is found but cannot read successfully
   ERROR_BARCODE_DATAMATRIX_READ_LOCKED   = -432,  // Bar Code Read DataMatrix is locked
   ERROR_BARCODE_DATAMATRIX_WRITE_LOCKED  = -433,  // Bar Code Write DataMatrix is locked
   ERROR_BARCODE_QR_READ_LOCKED           = -1380, // Bar Code Read QR is locked
   ERROR_BARCODE_QR_WRITE_LOCKED          = -1381, // Bar Code Write QR is locked
   ERROR_BARCODE_DLL_NOT_FOUND            = -1382, // Bar Code Engine DLL not found
   ERROR_BARCODE_XMODULE                  = -1481, // Incorrect module value it should be >= 0
} L_ERROR_BARCODE;

//=================================================================
// Lvkrn
//=================================================================
typedef enum
{
   ERROR_VECTOR_NOT_ENABLED            = -400,  // Vector capability is required to use this function
   ERROR_VECTOR_DXF_NOT_ENABLED        = -401,  // Vector DXF capability is required to use this function
   ERROR_VECTOR_DWG_NOT_ENABLED        = -402,  // Vector DWG capability is required to use this function
   ERROR_VECTOR_MISC_NOT_ENABLED       = -403,  // Vector Misc capability is required to use this function
   ERROR_VECTOR_DWF_NOT_ENABLED        = -405,  // Vector DWF capability is required to use this function
   ERROR_VECTOR_IS_LOCKED              = -500,  // Vector handle is locked
   ERROR_VECTOR_IS_EMPTY               = -501,  // Vector is empty
   ERROR_VECTOR_LAYER_NOT_FOUND        = -502,  // Layer is not found
   ERROR_VECTOR_LAYER_IS_LOCKED        = -503,  // Layer is locked
   ERROR_VECTOR_LAYER_ALREADY_EXISTS   = -504,  // Layer already exists
   ERROR_VECTOR_OBJECT_NOT_FOUND       = -505,  // Object is not found
   ERROR_VECTOR_INVALID_OBJECT_TYPE    = -506,  // Invalid object type
   ERROR_VECTOR_PEN_NOT_FOUND          = -507,  // Pen is not found
   ERROR_VECTOR_BRUSH_NOT_FOUND        = -508,  // Brush is not found
   ERROR_VECTOR_FONT_NOT_FOUND         = -509,  // Font is not found
   ERROR_VECTOR_BITMAP_NOT_FOUND       = -510,  // Bitmap is not found
   ERROR_VECTOR_POINT_NOT_FOUND        = -511,  // Point is not found
   ERROR_VECTOR_ENGINE_NOT_FOUND       = -512,  // Vector engine not found
   ERROR_VECTOR_INVALID_ENGINE         = -513,  // Invalid vector engine
   ERROR_VECTOR_CLIPBOARD              = -514,  // Clipboard error
   ERROR_VECTOR_CLIPBOARD_IS_EMPTY     = -515,  // Clipboard is empty
   ERROR_VECTOR_CANT_ADD_TEXT          = -516,  // Cannot add text
   ERROR_VECTOR_CANT_READ_WMF          = -517,  // Cannot read WMF
   ERROR_VECTOR_GROUP_NOT_FOUND        = -518,  // Group is not found
   ERROR_VECTOR_GROUP_ALREADY_EXISTS   = -519,  // Group already exists
} L_ERROR_VECTOR;

//=================================================================
// Lfmpg
//=================================================================
typedef enum
{
   ERROR_INV_HORZSIZE         = -360,  // The horizontal size is outside the specification
   ERROR_INV_VERTSIZE         = -361,  // The veritical size is outside the specification
   ERROR_ASPECTRATIO          = -362,  // Invalid pel aspect ratio
   ERROR_BITRATE              = -363,  // Invalid bitrate
   ERROR_VBV_BUFFER           = -364,  // VBV Buffer Size is isn't valid
   ERROR_CHROMA_FORMAT        = -365,  // Invalid chroma format
   ERROR_VIDEO_FORMAT         = -366,  // Invalid video format
   ERROR_COLOR_PRIMARY        = -367,  // Invalid number of color primaries
   ERROR_TRANSFER_CHAR        = -368,  // Invalid number of transfer characteristics
   ERROR_MATRIX_COEFF         = -369,  // Invalid number of matrix coeeficients
   ERROR_DISPLAY_HORZSIZE     = -370,  // Invalid horizontal display size
   ERROR_DISPLAY_VERTSIZE     = -371,  // Invalid vertical display size
   ERROR_INTRA_DCPREC         = -372,  // Invalid Intra DC Precision
   ERROR_FCODE                = -373,  // Invalid f code
   ERROR_SRCH_WINDOW          = -374,  // Invalid motion data search window
   ERROR_PROFILEID            = -375,  // Invalid profile id
   ERROR_LEVELID              = -376,  // Invalid level id
   ERROR_PROFILEID_NS         = -377,  // Encoder doesn't support scalabale bitstreams
   ERROR_INV_COMBINATION      = -378,  // Undefined Profile & Level id combination
   ERROR_BPIC_NOTALLOWED      = -379,  // B Pictures not allowed
   ERROR_RPTFIRST_MUSTBEZERO  = -380,  // Repeat first must be zero
   ERROR_FRAME_RATE           = -381,  // Invalid frame rate
} L_ERROR_MPG;

//=================================================================
// Ltweb
//=================================================================
typedef enum
{
   ERROR_NET_FIRST                              = -435,  // First net error code
   ERROR_NET_OUT_OF_HANDLES                     = -435,  // Out of handles
   ERROR_NET_TIMEOUT                            = -436,  // Time out
   ERROR_NET_EXTENDED_ERROR                     = -437,  // Extended error
   ERROR_NET_INTERNAL_ERROR                     = -438,  // Internal error
   ERROR_NET_INVALID_URL                        = -439,  // Invalid URL
   ERROR_NET_UNRECOGNIZED_SCHEME                = -440,  // Unrecognized schema
   ERROR_NET_NAME_NOT_RESOLVED                  = -441,  // Name not resolved
   ERROR_NET_PROTOCOL_NOT_FOUND                 = -442,  // Protocol not found
   ERROR_NET_INVALID_OPTION                     = -443,  // Invalid option
   ERROR_NET_BAD_OPTION_LENGTH                  = -444,  // Bad option length
   ERROR_NET_OPTION_NOT_SETTABLE                = -445,  // Option not settable
   ERROR_NET_SHUTDOWN                           = -446,  // Shutdown
   ERROR_NET_INCORRECT_USER_NAME                = -447,  // Incorrect username
   ERROR_NET_INCORRECT_PASSWORD                 = -448,  // Incorrect password
   ERROR_NET_LOGIN_FAILURE                      = -449,  // Log-in failure
   ERROR_NET_INVALID_OPERATION                  = -450,  // Invalid operation
   ERROR_NET_OPERATION_CANCELLED                = -451,  // Operation cancelled
   ERROR_NET_INCORRECT_HANDLE_TYPE              = -452,  // Incorrect handle type
   ERROR_NET_INCORRECT_HANDLE_STATE             = -453,  // Incorrect handle state
   ERROR_NET_NOT_PROXY_REQUEST                  = -454,  // Not proxy request
   ERROR_NET_REGISTRY_VALUE_NOT_FOUND           = -455,  // Registry value not found
   ERROR_NET_BAD_REGISTRY_PARAMETER             = -456,  // Bad registry parameter
   ERROR_NET_NO_DIRECT_ACCESS                   = -457,  // No direct access
   ERROR_NET_NO_CONTEXT                         = -458,  // No context
   ERROR_NET_NO_CALLBACK                        = -459,  // No callback
   ERROR_NET_REQUEST_PENDING                    = -460,  // Request pending
   ERROR_NET_INCORRECT_FORMAT                   = -461,  // Incorrect format
   ERROR_NET_ITEM_NOT_FOUND                     = -462,  // Item not found
   ERROR_NET_CANNOT_CONNECT                     = -463,  // Can't connect
   ERROR_NET_CONNECTION_ABORTED                 = -464,  // Connection aborted
   ERROR_NET_CONNECTION_RESET                   = -465,  // Connection reset
   ERROR_NET_FORCE_RETRY                        = -466,  // Force retry
   ERROR_NET_INVALID_PROXY_REQUEST              = -467,  // Invalid proxy request
   ERROR_NET_NEED_UI                            = -468,  // Need UI
   ERROR_NET_HANDLE_EXISTS                      = -469,  // Handle exists
   ERROR_NET_SEC_CERT_DATE_INVALID              = -470,  // Sec cert dat invalid
   ERROR_NET_SEC_CERT_CN_INVALID                = -471,  // Sec cert cn invalid
   ERROR_NET_HTTP_TO_HTTPS_ON_REDIR             = -472,  // HTTP to HTTPS on redir
   ERROR_NET_HTTPS_TO_HTTP_ON_REDIR             = -473,  // HTTPs to HTTP on redir
   ERROR_NET_MIXED_SECURITY                     = -474,  // Mixed security
   ERROR_NET_CHG_POST_IS_NON_SECURE             = -475,  // CHG post is non secure
   ERROR_NET_POST_IS_NON_SECURE                 = -476,  // Post is non secure
   ERROR_NET_CLIENT_AUTH_CERT_NEEDED            = -477,  // Client auth cert needed
   ERROR_NET_INVALID_CA                         = -478,  // Invalid CA
   ERROR_NET_CLIENT_AUTH_NOT_SETUP              = -479,  // Client auth not setup
   ERROR_NET_ASYNC_THREAD_FAILED                = -480,  // Thread asynchronous
   ERROR_NET_REDIRECT_SCHEME_CHANGE             = -481,  // Redirect schema change
   ERROR_NET_DIALOG_PENDING                     = -482,  // Dialog pending
   ERROR_NET_RETRY_DIALOG                       = -483,  // Retry dialog
   ERROR_NET_HTTPS_HTTP_SUBMIT_REDIR            = -484,  // HTTPS HTTP submit reader
   ERROR_NET_INSERT_CDROM                       = -485,  // Insert CD-ROM
   ERROR_NET_HTTP_HEADER_NOT_FOUND              = -486,  // HTTP header not found
   ERROR_NET_HTTP_DOWNLEVEL_SERVER              = -487,  // HTTP down level server
   ERROR_NET_HTTP_INVALID_SERVER_RESPONSE       = -488,  // HTTP invalid server response
   ERROR_NET_HTTP_INVALID_HEADER                = -489,  // HTTP invalid header
   ERROR_NET_HTTP_INVALID_QUERY_REQUEST         = -490,  // HTTP header query request
   ERROR_NET_HTTP_HEADER_ALREADY_EXISTS         = -491,  // HTTP header already exists
   ERROR_NET_HTTP_REDIRECT_FAILED               = -492,  // HTTP redirect faild
   ERROR_NET_HTTP_NOT_REDIRECTED                = -493,  // HTTP not redirected
   ERROR_NET_HTTP_COOKIE_NEEDS_CONFIRMATION     = -494,  // HTTP needs confirmation
   ERROR_NET_HTTP_COOKIE_DECLINED               = -495,  // HTTP cookie declined
   ERROR_NET_HTTP_REDIRECT_NEEDS_CONFIRMATION   = -496,  // HTTP redirect needs confirmation
   ERROR_NET_NO_OPEN_REQUEST                    = -497,  // No open request
} L_ERROR_WEB;

//=================================================================
// Ltaut
//=================================================================
typedef enum
{
   ERROR_AUTOMATION_INV_HANDLE   = -690,  // Invalid handle
   ERROR_AUTOMATION_INV_STATE    = -691,  // Invalid state
} L_ERROR_AUTOMATION;

//=================================================================
// Lttlb
//=================================================================
typedef enum
{
   ERROR_TOOLBAR_NO_RESOURCES = -660,  // No resources available
   ERROR_TOOLBAR_INV_STATE    = -661,  // Invalid state
   ERROR_TOOLBAR_INV_HANDLE   = -662,  // Invalid handle
} L_ERROR_TOOLBAR;

//=================================================================
// Ltpnt
//=================================================================
typedef enum
{
   ERROR_PAINT_INTERNAL       = -600,  // Internal error (Call LEAD)
   ERROR_PAINT_INV_DATA       = -601,  // Internal error (Call LEAD)
   ERROR_PAINT_NO_RESOURCES   = -602,  // Internal error (Call LEAD)
   ERROR_PAINT_NOT_ENABLED    = -603,  // DigitalPaint capability is required to use this function
} L_ERROR_PAINT;

//=================================================================
// Ltcon
//=================================================================
typedef enum
{
   ERROR_CONTAINER_INV_HANDLE    = -630,  // Invalid state
   ERROR_CONTAINER_INV_OPERATION = -631,  // Invalid operation
   ERROR_CONTAINER_NO_RESOURCES  = -632,  // No resources available
} L_ERROR_CONTAINER;

//=================================================================
// Ltscr
//=================================================================
typedef enum
{
   ERROR_CAPTURE_CANNOT_CREATE_HOTKEY_WINDOW       = -313,  // Error creating the hotkey window
   ERROR_CAPTURE_STILL_IN_PROCESS                  = -320,  // Capture is still in progress
   ERROR_CAPTURE_INVALID_DELAY                     = -321,  // Invalid delay specified
   ERROR_CAPTURE_INVALID_COUNT                     = -322,  // Invalid capture count specified
   ERROR_CAPTURE_INVALID_INTERVAL                  = -323,  // Invalid interval specified
   ERROR_CAPTURE_HOTKEY_CONFLICTS_WITH_CANCELKEY   = -324,  // Hotkey Conflicts With Cancelkey
   ERROR_CAPTURE_INVALID_AREA_TYPE                 = -325,  // Invalid Area Type specified
   ERROR_CAPTURE_NO_OPTION_STRUCTURE               = -326,  // Invalid or missing options strucuture
   ERROR_CAPTURE_INVALID_FILL_PATTERN              = -327,  // Invalid fill pattern
   ERROR_CAPTURE_INVALID_LINE_STYLE                = -328,  // Invalid line style
   ERROR_CAPTURE_INVALID_INFOWND_POS               = -329,  // Invalid info window position
   ERROR_CAPTURE_INVALID_INFOWND_SIZE              = -330,  // Invalid info window size
   ERROR_CAPTURE_ZERO_AREA_SIZE                    = -331,  // Zero Area Size for capture
   ERROR_CAPTURE_FILE_ACCESS_FAILED                = -332,  // The specified filename could not be read
   ERROR_CAPTURE_INVALID_32BIT_EXE_OR_DLL          = -333,  // The specified filename is not a valid 32bit EXE/DLL
   ERROR_CAPTURE_INVALID_RESOURCE_INDEX            = -335,  // The index resource is out of range
   ERROR_CAPTURE_NO_ACTIVE_WINDOW                  = -336,  // No active window for capture
   ERROR_CAPTURE_CANNOT_CAPTURE_WINDOW             = -337,  // Could not capture the window
   ERROR_CAPTURE_STRING_ID_NOT_DEFINED             = -338,  // String ID Not Defined.
   ERROR_CAPTURE_DELAY_LESS_THAN_ZERO              = -339,  // Delay value cannot be less than zero
   ERROR_CAPTURE_NO_MENU                           = -340,  // No menu to capture
} L_ERROR_SCR;

//=================================================================
// Lfsvg
//=================================================================
typedef enum
{
   ERROR_SVG_FILE_SIZE_READ                = -770,  // File size read
   ERROR_SVG_ROOT_NOT_SVG                  = -771,  // Root not SVG
   ERROR_SVG_NOFOUND_ROOT_ELEMENT          = -772,  // No root element found
   ERROR_SVG_INV_ELEMENT                   = -773,  // Invalid element
   ERROR_SVG_DUPLICATED_ATTRIBUTE          = -774,  // Duplicated attribute
   ERROR_SVG_INV_ATTRIBUTE                 = -775,  // Invalid attribute
   ERROR_SVG_INV_ATTRIBUTE_VALUE           = -776,  // Invalid attrubute value
   ERROR_SVG_BAD_CSS_PROPERTY              = -777,  // Bad CSS property
   ERROR_SVG_MISSING_REQUIRED_ATTRIBUTE    = -778,  // Missing required attribute
   ERROR_SVG_DUPLICATED_ID                 = -779,  // Duplicated ID
   ERROR_SVG_INV_COORDINATES_NUMBER        = -780,  // Invalid coordinates number
   ERROR_SVG_INV_STRING_COMMA              = -781,  // Invalid string comma
} L_ERROR_SVG;

//=================================================================
// Lffpx
//=================================================================
typedef enum
{
   ERROR_FPX_INVALID_FORMAT_ERROR       = -101,  // Invalid FlashPix format
   ERROR_FPX_FILE_WRITE_ERROR           = -102,  // Error writing file
   ERROR_FPX_FILE_READ_ERROR            = -103,  // Error reading file
   ERROR_FPX_FILE_NOT_FOUND             = -104,  // File not found
   ERROR_FPX_COLOR_CONVERSION_ERROR     = -105,  // Color conversion error
   ERROR_FPX_SEVER_INIT_ERROR           = -106,  // Server initialize error
   ERROR_FPX_LOW_MEMORY_ERROR           = -107,  // Not enough memory available
   ERROR_FPX_IMAGE_TOO_BIG_ERROR        = -108,  // Image too big
   ERROR_FPX_INVALID_COMPRESSION_ERROR  = -109,  // Invalid compression
   ERROR_FPX_INVALID_RESOLUTION         = -110,  // Invalid resolution
   ERROR_FPX_INVALID_FPX_HANDLE         = -111,  // Invalid handle
   ERROR_FPX_TOO_MANY_LINES             = -112,  // Too many lines
   ERROR_FPX_BAD_COORDINATES            = -113,  // Invalid coordinates
   ERROR_FPX_FILE_SYSTEM_FULL           = -114,  // File system full
   ERROR_FPX_MISSING_TABLE              = -115,  // The table is missing
   ERROR_FPX_RETURN_PARAMETER_TOO_LARGE = -116,  // Parameter too large
   ERROR_FPX_NOT_A_VIEW                 = -117,  // Error view
   ERROR_FPX_VIEW_IS_TRANFORMLESS       = -118,  // The view is transformless
   ERROR_FPX_ERROR                      = -119,  // General error
   ERROR_FPX_UNIMPLEMENTED_FUNCTION     = -120,  // Unimplemented function
   ERROR_FPX_INVALID_IMAGE_DESC         = -121,  // Invalid image description
   ERROR_FPX_INVALID_JPEG_TABLE         = -122,  // Invalid jpeg table
   ERROR_FPX_ILLEGAL_JPEG_ID            = -123,  // Illegal jpeg identifier
   ERROR_FPX_MEMORY_ALLOCATION_FAILED   = -124,  // Memory allocation failed
   ERROR_FPX_NO_MEMORY_MANAGEMENT       = -125,  // No memory management
   ERROR_FPX_OBJECT_CREATION_FAILED     = -126,  // Object creation failed
   ERROR_FPX_EXTENSION_FAILED           = -127,  // Extension_failed
   ERROR_FPX_FREE_NULL_PTR              = -128,  // Free null pointer
   ERROR_FPX_INVALID_TILE               = -129,  // Invalid tile
   ERROR_FPX_FILE_IN_USE                = -130,  // File in use
   ERROR_FPX_FILE_CREATE_ERROR          = -131,  // Error creating file
   ERROR_FPX_FILE_NOT_OPEN_ERROR        = -132,  // File not open
   ERROR_FPX_USER_ABORT                 = -133,  // User abort
   ERROR_FPX_OLE_FILE_ERROR             = -134,  // OLE File I/O error
   ERROR_FPXEXTENSIONS_LOCKED           = -147   // The FlashPix extension capabilities are locked
} L_ERROR_FPX;

//=================================================================
// Ltwia
//=================================================================
typedef enum
{
   ERROR_WIA_GENERAL_ERROR                   = -1450, // An unknown error has occurred with the Microsoft Windows Image Acquisition (WIA) device.
   ERROR_WIA_PAPER_JAM                       = -1451, // Paper is jammed in the scanner's document feeder.
   ERROR_WIA_PAPER_EMPTY                     = -1452, // The user requested a scan and there are no documents left in the document feeder.
   ERROR_WIA_PAPER_PROBLEM                   = -1453, // An unspecified problem occurred with the scanner's document feeder.
   ERROR_WIA_OFFLINE                         = -1454, // The WIA device is not online.
   ERROR_WIA_BUSY                            = -1455, // The WIA device is busy.
   ERROR_WIA_WARMING_UP                      = -1456, // The WIA device is warming up.
   ERROR_WIA_USER_INTERVENTION               = -1457, // An unspecified error has occurred with the WIA device that requires user intervention. The user should ensure that the device is turned on, online, and any cables are properly connected.
   ERROR_WIA_ITEM_DELETED                    = -1458, // The WIA device was deleted. It can no longer be accessed.
   ERROR_WIA_DEVICE_COMMUNICATION            = -1459, // An unspecified error occurred during an attempted communication with the WIA device.
   ERROR_WIA_INVALID_COMMAND                 = -1460, // The device does not support this command.
   ERROR_WIA_INCORRECT_HARDWARE_SETTING      = -1461, // There is an incorrect setting on the WIA device.
   ERROR_WIA_DEVICE_LOCKED                   = -1462, // The scanner head is locked.
   ERROR_WIA_EXCEPTION_IN_DRIVER             = -1463, // The device driver threw an exception.
   ERROR_WIA_INVALID_DRIVER_RESPONSE         = -1464, // The response from the driver is invalid.
   ERROR_WIA_COVER_OPEN                      = -1465, // No Description available.
   ERROR_WIA_LAMP_OFF                        = -1466, // No Description available.
   ERROR_WIA_DESTINATION                     = -1467, // No Description available.
   ERROR_WIA_NETWORK_RESERVATION_FAILED      = -1468, // No Description available.
   ERROR_WIA_NO_DEVICE_AVAILABLE             = -1469, // There are no WIA hardware devices that match the given specifications.
   ERROR_WIA_NO_DEVICE_SELECTED              = -1470, // There is no device currently selected.
   ERROR_WIA_INVALID_STREAMING_VIDEO_DEVICE  = -1471, // Selected device does not support streaming video.
   ERROR_WIA_VIDEO_STREAM_NOT_INITIALIZED    = -1472, // WIA video stream No initialized.
   ERROR_WIA_ACCESS_DENIED                   = -1473, // Access denied.
   ERROR_WIA_NO_ITEMS_AVAILABLE              = -1474, // No child items available.
   ERROR_WIA_UNKNOWN                         = -1475, // Unknown WIA error occurred.
   ERROR_WIA_INVALID_VERSION                 = -1476, // Invalid WIA version.
} L_ERROR_WIA;

//=================================================================
// ActiveX
//=================================================================
typedef enum
{
   ERROR_NO_TOOLBAR                          = -317,  // The toolbar window was not created.
   ERROR_INV_VARIANT_TYPE                    = -1487, // Invalid Variant type.
   ERROR_INV_STRING                          = -1488, // Invalid string value.
   ERROR_INV_DISPATCH                        = -1489, // Invalid dispatch.

} L_ERROR_ACTIVEX;

//=================================================================
// LEADTOOLS Printer Driver
//=================================================================
typedef enum
{
   ERROR_PRNDRV_LOCKED                    = -1600, // LEADTOOLS Printer is locked.
   ERROR_PRNDRV_MORE_DATA                 = -1601, // Allocated memory are not enough to retrieve the required data
   ERROR_PRNDRV_NO_OPTIONS                = -1602, // No previously options set
   ERROR_PRNDRV_NOT_LEADTOOLS_PRINTER     = -1603, // Not a LEADTOOLS Printer
   ERROR_PRNDRV_PRINTER_DATA              = -1604, // Invalid printer data
   ERROR_PRNDRV_SET_PRINTER_DATA          = -1605, // Can not set printer data
   ERROR_PRNDRV_CREATE_REG_KEY            = -1606, // Can not create registry key
   ERROR_PRNDRV_SET_REG_VALUE             = -1607, // Can not set registry key value
   ERROR_PRNDRV_ADD_PRINTER               = -1608, // Can not add printer
   ERROR_PRNDRV_PRINTER_NOTFOUND          = -1609, // Printer not found
   ERROR_PRNDRV_ROOTDIR_NOTFOUND          = -1610, // Root directory not found
   ERROR_PRNDRV_CONNECTION_EXE_NOTFOUND   = -1611, // Connection file not found
   ERROR_PRNDRV_SPOOL_FOLDER_NOTFOUND     = -1612, // Spool folder not found
   ERROR_PRNDRV_INVALID_PRINTER_TYPE      = -1613, // Invalid printer type
   ERROR_PRNDRV_INVALID_PRINTER           = -1614, // Invalid printer
   ERROR_PRNDRV_DOC_ALREADY_STARTED       = -1615, // Document already started
   ERROR_PRNDRV_CREATE_TEMP_FILE          = -1616, // Can not create temporary file
   ERROR_PRNDRV_LOAD_DLL                  = -1617, // Error loading DLL
   ERROR_PRNDRV_LOADFONT_DATA             = -1618, // fixed palette data not found
   ERROR_PRNDRV_CANNOT_LOCK               = -1619, // Printer Created Without Password
} L_ERROR_PRINTERDRIVER;


#endif // #if defined NOT_TRIALMAX

#endif // #if !defined(LTERR_H)
