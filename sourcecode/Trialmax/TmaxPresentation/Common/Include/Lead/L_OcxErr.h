/*
   L_OCXERR.H - error codes used by the ActiveX
   Copyright (c) 1991-2000 LEAD Technologies, Inc.
   All Rights Reserved.
*/
#ifndef _L_OCXERR_H_
#define _L_OCXERR_H_

#define ERROR_FAILURE                    20000  /** Function not successful **/

#define ERROR_NO_MEMORY                  20001  /** Not enough memory available**/
#define ERROR_NO_BITMAP                  20002  /** Invalid bitmap handle      **/
#define ERROR_MEMORY_TOO_LOW             20003  /** Not enough memory available**/
#define ERROR_FILE_LSEEK                 20004  /** Error seeking to position  **/
#define ERROR_FILE_WRITE                 20005  /** Error writing file         **/
#define ERROR_FILE_GONE                  20006  /** File not present - abort   **/
#define ERROR_FILE_READ                  20007  /** Error reading file         **/
#define ERROR_INV_FILENAME               20008  /** Invalid filename specified **/
#define ERROR_FILE_FORMAT                20009  /** Invalid file format        **/
#define ERROR_FILENOTFOUND               20010  /** File not found             **/
#define ERROR_INV_RANGE                  20011  /** Invalid width/height       **/
#define ERROR_IMAGE_TYPE                 20012  /** Image format recognized, but sub-type not supported **/
#define ERROR_INV_PARAMETER              20013  /** Invalid parameter passed   **/
#define ERROR_FILE_OPEN                  20014  /** Not able to open file      **/
#define ERROR_UNKNOWN_COMP               20015  /** Unknown compression format **/
#define ERROR_FEATURE_NOT_SUPPORTED      20016  /** Feature not supported      **/
#define ERROR_NOT_256_COLOR              20017  /** VGA card only supports 256 colors (8 bit)           **/
#define ERROR_PRINTER                    20018  /** Printer error              **/
#define ERROR_CRC_CHECK                  20019  /** Data CRC check error       **/
#define ERROR_QFACTOR                    20021  /** Invalid QFactor specified  **/
#define ERROR_TARGAINSTALL               20022  /** TARGA not installed        **/
#define ERROR_OUTPUTTYPE                 20023  /** Invalid compression format **/
#define ERROR_XORIGIN                    20024  /** X origin specified invalid **/
#define ERROR_YORIGIN                    20025  /** Y origin specified invalid **/
#define ERROR_VIDEOTYPE                  20026  /** Invalid video mode         **/
#define ERROR_BITPERPIXEL                20027  /** Invalid bits/pixel         **/
#define ERROR_WINDOWSIZE                 20028  /** Invalid window size        **/
#define ERROR_NORMAL_ABORT               20029  /** Escape key pressed - or user aborted operation      **/
#define ERROR_NOT_INITIALIZED            20030  /** Internal error - call LEAD **/
#define ERROR_CU_BUSY                    20031  /** Internal error - call LEAD **/
#define ERROR_INVALID_TABLE_TYPE         20032  /** Internal error - call LEAD **/
#define ERROR_UNEQUAL_TABLES             20033  /** Internal error - call LEAD **/
#define ERROR_INVALID_BUFFER             20034  /** Internal error - call LEAD **/
#define ERROR_MISSING_TILE_DATA          20035  /** Internal error - call LEAD **/
#define ERROR_INVALID_QVALUE             20036  /** Internal error - call LEAD **/
#define ERROR_INVALIDDATA                20037  /** Internal error - call LEAD **/
#define ERROR_INVALID_COMPRESSED_TYPE    20038  /** Internal error - call LEAD **/
#define ERROR_INVALID_COMPONENT_NUM      20039  /** Internal error - call LEAD **/
#define ERROR_INVALID_PIXEL_TYPE         20040  /** Internal error - call LEAD **/
#define ERROR_INVALID_PIXEL_SAMPLING     20041  /** Internal error - call LEAD **/
#define ERROR_INVALID_SOURCE_FILE        20042  /** Internal error - call LEAD **/
#define ERROR_INVALID_TARGET_FILE        20043  /** Internal error - call LEAD **/
#define ERROR_INVALID_IMAGE_DIMS         20044  /** Internal error - call LEAD **/
#define ERROR_INVALID_TILE_DIMS          20045  /** Internal error - call LEAD **/
#define ERROR_INVALID_PIX_BUFF_DIMS      20046  /** Internal error - call LEAD **/
#define ERROR_SEGMENT_OVERFLOW           20047  /** Internal error - call LEAD **/
#define ERROR_INVALID_SUBSAMPLING        20048  /** Internal error - call LEAD **/
#define ERROR_INVALID_Q_VIS_TABLE        20049  /** Internal error - call LEAD **/
#define ERROR_INVALID_DC_CODE_TABLE      20050  /** Internal error - call LEAD **/
#define ERROR_INVALID_AC_CODE_TABLE      20051  /** Internal error - call LEAD **/
#define ERROR_INSUFFICIENT_DATA          20052  /** Internal error - call LEAD **/
#define ERROR_MISSING_FUNC_POINTER       20053  /** Internal error - call LEAD **/
#define ERROR_TOO_MANY_DC_CODE_TABLES    20054  /** Internal error - call LEAD **/
#define ERROR_TOO_MANY_AC_CODE_TABLES    20055  /** Internal error - call LEAD **/
#define ERROR_INVALID_SUBIMAGE           20056  /** Internal error - call LEAD **/
#define ERROR_INVALID_ABORTION           20057  /** Internal error - call LEAD **/
#define ERROR_CU_NO_SUPPORT              20058  /** Internal error - call LEAD **/
#define ERROR_CU_FAILURE                 20059  /** Internal error - call LEAD **/
#define ERROR_BAD_POINTER                20060  /** Internal error - call LEAD **/
#define ERROR_HEADER_DATA_FAILURE        20061  /** Internal error - call LEAD **/
#define ERROR_COMPRESSED_DATA_FAILURE    20062  /** Internal error - call LEAD **/

#define ERROR_FIXEDPAL_DATA              20063  /** fixed palette data not found **/
#define ERROR_LOADFONT_DATA              20064  /** fixed palette data not found **/
#define ERROR_NO_STAMP                   20065  /** stamp not found **/
#define ERROR_G3CODE_INVALID             20070  /** invalid group 3 dat **/
#define ERROR_G3CODE_EOF                 20071  /** group3 code end of file **/
#define ERROR_G3CODE_EOL                 20072  /** group3 code end of code **/
#define ERROR_PREMATURE_EOF              20073  /** premature EOF at scanline **/
#define ERROR_PREMATURE_EOL              20074  /** premature end-of-line code **/
#define ERROR_UNCOMP_EOF                 20075  /** premature end-of-line code **/
#define ERROR_ACT_INCOMP                 20076  /** premature end-of-line code **/
#define ERROR_BAD_DECODE_STATE           20077  /** premature end-of-line code **/
#define ERROR_VERSION_NUMBER             20078  /** bad version number **/
#define ERROR_TWAIN_NODSM                20079  /** TWAIN source manager not found **/
#define ERROR_TWAIN_BUMMER               20080  /** TWAIN Failure due to unknown causes **/
#define ERROR_TWAIN_LOWMEMORY            20081  /** TWAIN Not enough memory to perform operation **/
#define ERROR_TWAIN_NODS                 20082  /** TWAIN No Data Source **/
#define ERROR_TWAIN_MAXCONNECTIONS       20083  /** TWAIN DS is connected to max possible apps **/
#define ERROR_TWAIN_OPERATIONERROR       20084  /** TWAIN DS or DSM reported error, app shouldn't (no need for your app to report the error) **/
#define ERROR_TWAIN_BADCAP               20085  /** TWAIN Unknown capability **/
#define ERROR_TWAIN_BADPROTOCOL          20086  /** TWAIN Unrecognized MSG DG DAT combination **/
#define ERROR_TWAIN_BADVALUE             20087  /** TWAIN Data parameter out of range **/
#define ERROR_TWAIN_SEQERROR             20088  /** TWAIN DG DAT MSG out of expected sequence **/
#define ERROR_TWAIN_BADDEST              20089  /** TWAIN Unknown destination App/Src in DSM_Entry **/
#define ERROR_TWAIN_CANCEL               20090  /** TWAIN Operation was cancelled **/
#define ERROR_PANWINDOW_NOT_CREATED      20091  /** PanWindow creation failed **/
#define ERROR_NOT_ENOUGH_IMAGES          20092  /** Not enough images - more input images are required **/
#define ERROR_TWAIN_CAPUNSUPPORTED       20093  /** TWAIN capability unsupported **/
#define ERROR_TWAIN_FILEXFER_UNSUPPORTED 20094 /** TWAIN File Transfer unsupported **/

#define ERROR_USER_ABORT                 20100  /** User has aborted operation **/

#define ERROR_FPX_INVALID_FORMAT_ERROR       20101  /* Invalid FlashPix format */
#define ERROR_FPX_FILE_WRITE_ERROR           20102  /* Error writing file */
#define ERROR_FPX_FILE_READ_ERROR            20103  /* Error reading file */
#define ERROR_FPX_FILE_NOT_FOUND             20104  /* File not found */
#define ERROR_FPX_COLOR_CONVERSION_ERROR     20105  /* Color conversion error */
#define ERROR_FPX_SEVER_INIT_ERROR           20106  /* Server initialize error */
#define ERROR_FPX_LOW_MEMORY_ERROR           20107  /* Not enough memory available */
#define ERROR_FPX_IMAGE_TOO_BIG_ERROR        20108  /* Image too big */
#define ERROR_FPX_INVALID_COMPRESSION_ERROR  20109  /* Invalid compression */
#define ERROR_FPX_INVALID_RESOLUTION         20110  /* Invalid resolution */
#define ERROR_FPX_INVALID_FPX_HANDLE         20111  /* Invalid handle */
#define ERROR_FPX_TOO_MANY_LINES             20112  /* Too many lines */
#define ERROR_FPX_BAD_COORDINATES            20113  /* Invalid coordinates */
#define ERROR_FPX_FILE_SYSTEM_FULL           20114  /* File system full */
#define ERROR_FPX_MISSING_TABLE              20115  /* The table is missing */
#define ERROR_FPX_RETURN_PARAMETER_TOO_LARGE 20116  /* Parameter too large */
#define ERROR_FPX_NOT_A_VIEW                 20117  /* Error view */
#define ERROR_FPX_VIEW_IS_TRANFORMLESS       20118  /* The view is transformless */
#define ERROR_FPX_ERROR                      20119  /* General error */
#define ERROR_FPX_UNIMPLEMENTED_FUNCTION     20120  /* Unimplemented function */
#define ERROR_FPX_INVALID_IMAGE_DESC         20121  /* Invalid image description */
#define ERROR_FPX_INVALID_JPEG_TABLE         20122  /* Invalid jpeg table */
#define ERROR_FPX_ILLEGAL_JPEG_ID            20123  /* Illegal jpeg identifier */
#define ERROR_FPX_MEMORY_ALLOCATION_FAILED   20124  /* Memory allocation failed */
#define ERROR_FPX_NO_MEMORY_MANAGEMENT       20125  /* No memory management */
#define ERROR_FPX_OBJECT_CREATION_FAILED     20126  /* Object creation failed */
#define ERROR_FPX_EXTENSION_FAILED           20127  /* Extension_failed */
#define ERROR_FPX_FREE_NULL_PTR              20128  /* Free null pointer */
#define ERROR_FPX_INVALID_TILE               20129  /* Invalid tile */
#define ERROR_FPX_FILE_IN_USE                20130  /* File in use */
#define ERROR_FPX_FILE_CREATE_ERROR          20131  /* Error creating file */
#define ERROR_FPX_FILE_NOT_OPEN_ERROR        20132  /* File not open */
#define ERROR_FPX_USER_ABORT                 20133  /* User abort */
#define ERROR_FPX_OLE_FILE_ERROR             20134  /* OLE File I/O error */

#define ERROR_BAD_TAG                        20140  /* Bad TIFF tag */
#define ERROR_INVALID_STAMP_SIZE             20141  /* Stamp size is too big or invalid bits/pixel, etc */
#define ERROR_BAD_STAMP                      20142  /* Stamp is present but data is corrupted */
#define ERROR_DOCUMENT_NOT_ENABLED           20143  /* Document capability is required to use this function */
#define ERROR_IMAGE_EMPTY                    20144  /* Image is empty */
#define ERROR_NO_CHANGE                      20145  /* The image hasn't changed */
#define ERROR_LZW_LOCKED                     20146  /* The LZW capabilities are locked */
#define ERROR_FPXEXTENSIONS_LOCKED           20147  /* The FlashPix extension capabilities are locked */
#define ERROR_ANN_LOCKED                     20148  /* One or more annotation objects are still locked */
#define ERROR_EXCEPTION                      20149  /* An unspecified exception occured - could be memory access violations */

#define ERROR_DLG_FAILED                     20150  /* An error occured during creation of the common dialog */
#define ERROR_DLG_ALREADYINITIATED           20151  /* You have already initialized the dialogs */
#define ERROR_DLG_NOTINITIATED               20152  /* You did not initialize the dialogs */
#define ERROR_DLG_COLORNOTINITIALIZED        20153  /* You did not initialize the dialogs with DLG_INIT_COLOR */
#define ERROR_DLG_COLORDLL_UNABLETOLOAD      20154  /* The dialogs could not load the LTCLR dll */
#define ERROR_DLG_RESDLL_UNABLETOLOAD        20155  /* The dialogs could not load the LTDLGRES dll */

#define ERROR_ISIS_NOCURSCANNER              20160  /* ISIS No Scanner selected */
#define ERROR_ISIS_SCANDRIVER_NOT_LOADED     20161  /* ISIS Scanner driver failed to load */
#define ERROR_ISIS_CANCEL                    20162  /* ISIS Operation canceled */
#define ERROR_ISIS_BAD_TAG_OR_VALUE          20163  /* ISIS Bad Tag or Bad Tag Value */
#define ERROR_ISIS_NOT_READY                 20164  /* ISIS Scanner not ready */
#define ERROR_ISIS_NO_PAGE                   20165  /* ISIS No page in the Scanner */
#define ERROR_ISIS_JAM                       20166  /* ISIS Scanner Jam error */
#define ERROR_ISIS_SCANNER_ERROR             20167  /* ISIS Scanner driver rror */
#define ERROR_ISIS_BUSY                      20168  /* ISIS Scanner is busy */
#define ERROR_ISIS_FILE_ERROR                20169  /* ISIS File driver error */
#define ERROR_ISIS_NETWORK_ERROR             20170  /* ISIS Network error */
#define ERROR_ISIS_NOT_INSTALLED             20171  /* ISIS Is not installed - you need to have ISIS installed to use this method/property */
#define ERROR_ISIS_NO_PIXDFLT                20172  /* ISIS Is not installed */
#define ERROR_ISIS_PIXVERSION                20173  /* ISIS Version is incorrect */
#define ERROR_ISIS_PERM_NOACCESS             20174  /* ISIS No permission/access */

#define ERROR_RECORDING                      20250  /* Wave device is currently recording */
#define ERROR_COMPRESSOR                     20251  /* Invalid compressor */
#define ERROR_SOUND_DEVICE                   20252  /* Problem with sound card */
#define ERROR_DEVICE_INUSE                   20253  /* The sound card is currently in use */
#define ERROR_INV_TRACKTYPE                  20254  /* Invalid track for operation */
#define ERROR_NO_SOUNDCARD                   20255  /* No sound card is presend */
#define ERROR_NOT_RECORDING                  20256  /* Not currently recording */
#define ERROR_INV_MODE                       20257  /* Invalid open mode for operation */
#define ERROR_NO_VIDEO_MODULE                20258  /* No video module present on machine */
#define ERROR_QUEUE_FULL                     20259  /* The buffer queue is full */
#define ERROR_CONFIGURE_RESTART              20260  /* Configuration was successful, but you need to restart Windows */
#define ERROR_INV_HANDLE                     20261  /* Invalid handle */

#define ERROR_HOST_RESOLVE                   20270  /* Can't resolve hostname */
#define ERROR_CANT_INITIALIZE                20271  /* Can't initialize network */
#define ERROR_NO_CONNECTION                  20272  /* No open network connections */
#define ERROR_HOST_NOT_FOUND                 20273  /* Hostname not found */
#define ERROR_NOT_SERVER                     20274  /* Computer is not a server */
#define ERROR_NO_CONNECTIONS                 20275  /* No connection requests waiting */
#define ERROR_CONNECT_REFUSED                20276  /* Connection was refused */
#define ERROR_IS_CONNECTED                   20277  /* computer is already connected */
#define ERROR_NET_UNREACH                    20278  /* network is unreachable from host */
#define ERROR_TIME_OUT                       20279  /* connect attempt time out without connecting */
#define ERROR_NET_DOWN                       20280  /* network subsystem is down */
#define ERROR_NO_BUFFERS                     20281  /* no buffer space is available, too many connections*/
#define ERROR_NO_FILE_DESCR                  20282  /* no more file handle available */
#define ERROR_DATA_QUEUED                    20283  /* data has been queued to send later */
#define ERROR_UNKNOWN                        20284  /* unkown network error has occured */
#define ERROR_CONNECT_RESET                  20285  /* connection was reset - socket might be unusable */
#define ERROR_TRANSFER_ABORTED               20286  /* transfer was aborted */
#define ERROR_DSHOW_FAILURE                  20287  /* DirectShow isn't installed correctly */
#define ERROR_REGISTRY_READ                  20288  /* Error reading from the registry */
#define ERROR_WAVE_FORMAT                    20289  /* The wave format is not supported */
#define ERROR_BUFFER_TOO_SMALL               20290  /* The buffer size is too small */
#define ERROR_WAVE_CONVERT                   20291  /* Error converting the wave format */
#define ERROR_MULTIMEDIA_NOT_ENABLED         20292  /* Video capability is required to use this function */

#define ERROR_CAP_CONNECT                    20293  /* error connecting to capture device */
#define ERROR_CAP_DISCONNECT                 20294  /* error disconnecting capture device */
#define ERROR_DISK_ISFULL                    20295  /* not enough disk space */
#define ERROR_CAP_OVERLAY                    20296  /* error setting capture overlay */
#define ERROR_CAP_PREVIEW                    20297  /* error setting capture preview */
#define ERROR_CAP_COPY                       20298  /* error copying to clipboard */
#define ERROR_CAP_WINDOW                     20299  /* error with capture window */
#define ERROR_CAP_ISCAPTURING                20300  /* operation invalid during capture */
#define ERROR_NO_STREAMS                     20301  /* no streams to play */
#define ERROR_CREATE_STREAM                  20302  /* error creating streams */
#define ERROR_FRAME_DELETE                   20303  /* error deleting frames */

#define ERROR_DXF_FILTER_MISSING             20309  /* The DXF filter is needed to use this function */
#define ERROR_PAGE_NOT_FOUND                 20310  /* Page not found */
#define ERROR_DELETE_LAST_PAGE               20311  /* You cannot delete a page from a file containing only one page */
#define ERROR_NO_HOTKEY                      20312  /* There is no hotkey */
#define ERROR_CANNOT_CREATE_HOTKEY_WINDOW    20313  /* Error creating the hotkey window */
#define ERROR_MEDICAL_NOT_ENABLED            20314  /* Medical Express capability is required to use this function */
#define ERROR_JBIG_NOT_ENABLED               20315  /* JBIG capability is required to use this function */
#define ERROR_UNDO_STACK_EMPTY               20316  /* Undo stack is empty - too many undos */
#define ERROR_NO_TOOLBAR                     20317  /* ActiveX error - the toolbar window was not created */
#define ERROR_MEDICAL_NET_NOT_ENABLED        20318  /* Medical Network Express capability is required to use this function */
#define ERROR_JBIG_FILTER_MISSING            20319  /* The JBIG filter is needed to use this function */

#define ERROR_CAPTURE_STILL_IN_PROCESS          20320  /* Capture is still in progress */
#define ERROR_INVALID_DELAY                     20321  /* Invalid delay specified */
#define ERROR_INVALID_COUNT                     20322  /* Invalid capture count specified */
#define ERROR_INVALID_INTERVAL                  20323  /* Invalid interval specified */
#define ERROR_HOTKEY_CONFLICTS_WITH_CANCELKEY   20324  /* Hotkey Conflicts With Cancelkey */
#define ERROR_CAPTURE_INVALID_AREA_TYPE         20325  /* Invalid Area Type specified */
#define ERROR_CAPTURE_NO_OPTION_STRUCTURE       20326  /* Invalid or missing options strucuture */
#define ERROR_CAPTURE_INVALID_FILL_PATTERN      20327  /* Invalid fill pattern */
#define ERROR_CAPTURE_INVALID_LINE_STYLE        20328  /* Invalid line style */
#define ERROR_CAPTURE_INVALID_INFOWND_POS       20329  /* Invalid info window position */
#define ERROR_CAPTURE_INVALID_INFOWND_SIZE      20330  /* Invalid info window size */
#define ERROR_CAPTURE_ZERO_AREA_SIZE            20331  /* Zero Area Size for capture */
#define ERROR_CAPTURE_FILE_ACCESS_FAILED        20332  /* The specified filename could not be read */
#define ERROR_CAPTURE_INVALID_32BIT_EXE_OR_DLL  20333  /* The specified filename is not a valid 32bit EXE/DLL */
#define ERROR_CAPTURE_INVALID_RESOURCE_TYPE     20334  /* The specified resource type is not supported */
#define ERROR_CAPTURE_INVALID_RESOURCE_INDEX    20335  /* The index resource is out of range */
#define ERROR_CAPTURE_NO_ACTIVE_WINDOW          20336  /* No active window for capture */
#define ERROR_CAPTURE_CANNOT_CAPTURE_WINDOW     20337  /* Could not capture the window */
#define ERROR_CAPTURE_STRING_ID_NOT_DEFINED     20338  /* String ID Not Defined. */
#define ERROR_CAPTURE_DELAY_LESS_THAN_ZERO      20339  /* Delay value cannot be less than zero */
#define ERROR_CAPTURE_NO_MENU                   20340  /* No menu to capture */

#define ERROR_BROWSE_FAILED                     20350  /* directory browsing failed */
#define ERROR_NOTHING_TO_DO                     20351  /* There is nothing to do! - i.e. no files were found */
#define ERROR_INTERNET_NOT_ENABLED              20352  /* Internet capability is required to use this function */
#define ERROR_LVKRN_MISSING                     20353  /* The LVKRN DLL is needed to use this function */
#define ERROR_CMW_LOCKED                        20354  /* The CMW support is locked */
#define ERROR_CMW_FILTER_MISSING                20355  /* CMW filter is needed to use this function */
#define ERROR_CMP_FILTER_MISSING                20356  /* CMP filter is needed to use this function */
#define ERROR_FAX_FILTER_MISSING                20357  /* FAX filter is needed to use this function */

// start of MPEG specific error codes

#define ERROR_INV_HORZSIZE                      20360   /* The horizontal size is outside the specification */
#define ERROR_INV_VERTSIZE                      20361   /* The veritical size is outside the specification */   
#define ERROR_ASPECTRATIO                       20362   /* Invalid pel aspect ratio */
#define ERROR_BITRATE                           20363   /* Invalid bitrate */
#define ERROR_VBV_BUFFER                        20364   /* VBV Buffer Size is isn't valid */
#define ERROR_CHROMA_FORMAT                     20365   /* Invalid chroma format */
#define ERROR_VIDEO_FORMAT                      20366   /* Invalid video format */
#define ERROR_COLOR_PRIMARY                     20367   /* Invalid number of color primaries */
#define ERROR_TRANSFER_CHAR                     20368   /* Invalid number of transfer characteristics */
#define ERROR_MATRIX_COEFF                      20369   /* Invalid number of matrix coeeficients */
#define ERROR_DISPLAY_HORZSIZE                  20370   /* Invalid horizontal display size */
#define ERROR_DISPLAY_VERTSIZE                  20371   /* Invalid vertical display size */
#define ERROR_INTRA_DCPREC                      20372   /* Invalid Intra DC Precision */
#define ERROR_FCODE                             20373   /* Invalid f code */
#define ERROR_SRCH_WINDOW                       20374   /* Invalid motion data search window */
#define ERROR_PROFILEID                         20375   /* Invalid profile id */
#define ERROR_LEVELID                           20376   /* Invalid level id */
#define ERROR_PROFILEID_NS                      20377   /* Encoder doesn't support scalabale bitstreams */
#define ERROR_INV_COMBINATION                   20378   /* Undefined Profile & Level id combination */
#define ERROR_BPIC_NOTALLOWED                   20379   /* B Pictures not allowed */
#define ERROR_RPTFIRST_MUSTBEZERO               20380   /* Repeat first must be zero */
#define ERROR_FRAME_RATE                        20381   /* Invalid frame rate */

// end MPEG specific error codes

#define ERROR_VECTOR_NOT_ENABLED                20400  /* Vector capability is required to use this function */
#define ERROR_VECTOR_DXF_NOT_ENABLED            20401  /* Vector DXF capability is required to use this function */
#define ERROR_VECTOR_DWG_NOT_ENABLED            20402  /* Vector DWG capability is required to use this function */
#define ERROR_VECTOR_MISC_NOT_ENABLED           20403  /* Vector Misc capability is required to use this function */
#define ERROR_TAG_MISSING                       20404  /* Tag not found */
#define ERROR_VECTOR_DWF_NOT_ENABLED            20405  /* Vector DWF capability is required to use this function */
#define ERROR_NO_UNDO_STACK                     20406  /* There is no undo stack */
#define ERROR_UNDO_DISABLED                     20407  /* The undo is disabled */
#define ERROR_PDF_NOT_ENABLED                   20408  /* PDF capability is required to use this function */
#define ERROR_ENTENSIONS_MISSING                20409  /* EXIF extensions are missing */
#define ERROR_BARCODE_DIGIT_CHECK               20410  /* Invalid error check digit */
#define ERROR_BARCODE_INVALID_TYPE              20411  /* Invalid bar code type */
#define ERROR_BARCODE_TEXTOUT                   20412  /* Invalid bar code text out option */
#define ERROR_BARCODE_WIDTH                     20413  /* Invalid bar code width */
#define ERROR_BARCODE_HEIGHT                    20414  /* Invalid bar code height */
#define ERROR_BARCODE_TOSMALL                   20415  /* Bar code string is too small */
#define ERROR_BARCODE_STRING                    20416  /* Invalid bar code string for a specified bar code type */
#define ERROR_BARCODE_NOTFOUND                  20417  /* No bar code recognition */
#define ERROR_BARCODE_UNITS                     20418  /* Invalid bar code measurement unit */
#define ERROR_BARCODE_MULTIPLEMAXCOUNT          20419  /* Invalid multiple max count */
#define ERROR_BARCODE_GROUP                     20420  /* Invalid bar code group */
#define ERROR_BARCODE_NO_DATA                   20421  /* Invalid BARCODEDATA structure */
#define ERROR_BARCODE_NOTFOUND_DUPLICATED       20422  /* No duplicated bar code */
#define ERROR_BARCODE_LAST_DUPLICATED           20423  /* Reached the last duplicated bar code */
#define ERROR_BARCODE_STRING_LENGTH             20424  /* Invalid bar code data string length */
#define ERROR_BARCODE_LOCATION                  20425  /* Invalid bar code area location */
#define ERROR_BARCODE_1D_LOCKED                 20426  /* Bar Code 1D is locked */
#define ERROR_BARCODE_2D_READ_LOCKED            20427  /* Bar Code Read 2D (CodeOne) is locked */
#define ERROR_BARCODE_2D_WRITE_LOCKED           20428  /* Bar Code Write 2D (CodeOne) is locked */
#define ERROR_BARCODE_PDF_READ_LOCKED           20429  /* Bar Code Read PDF is locked */
#define ERROR_BARCODE_PDF_WRITE_LOCKED          20430  /* Bar Code Write PDF is locked */
#define ERROR_BARCODE_FOUNDCORRUPT              20431  /* Barcode PDF417 symbol is found but cannot read successfully */
#define ERROR_BARCODE_DATAMATRIX_READ_LOCKED    20432  /* Bar Code Read DataMatrix is locked */
#define ERROR_BARCODE_DATAMATRIX_WRITE_LOCKED   20433  /* Bar Code Write DataMatrix is locked */

#define ERROR_NET_FIRST                            20435
#define ERROR_NET_OUT_OF_HANDLES                   20435
#define ERROR_NET_TIMEOUT                          20436
#define ERROR_NET_EXTENDED_ERROR                   20437
#define ERROR_NET_INTERNAL_ERROR                   20438
#define ERROR_NET_INVALID_URL                      20439
#define ERROR_NET_UNRECOGNIZED_SCHEME              20440
#define ERROR_NET_NAME_NOT_RESOLVED                20441
#define ERROR_NET_PROTOCOL_NOT_FOUND               20442
#define ERROR_NET_INVALID_OPTION                   20443
#define ERROR_NET_BAD_OPTION_LENGTH                20444
#define ERROR_NET_OPTION_NOT_SETTABLE              20445
#define ERROR_NET_SHUTDOWN                         20446
#define ERROR_NET_INCORRECT_USER_NAME              20447
#define ERROR_NET_INCORRECT_PASSWORD               20448
#define ERROR_NET_LOGIN_FAILURE                    20449
#define ERROR_NET_INVALID_OPERATION                20450
#define ERROR_NET_OPERATION_CANCELLED              20451
#define ERROR_NET_INCORRECT_HANDLE_TYPE            20452
#define ERROR_NET_INCORRECT_HANDLE_STATE           20453
#define ERROR_NET_NOT_PROXY_REQUEST                20454
#define ERROR_NET_REGISTRY_VALUE_NOT_FOUND         20455
#define ERROR_NET_BAD_REGISTRY_PARAMETER           20456
#define ERROR_NET_NO_DIRECT_ACCESS                 20457
#define ERROR_NET_NO_CONTEXT                       20458
#define ERROR_NET_NO_CALLBACK                      20459
#define ERROR_NET_REQUEST_PENDING                  20460
#define ERROR_NET_INCORRECT_FORMAT                 20461
#define ERROR_NET_ITEM_NOT_FOUND                   20462
#define ERROR_NET_CANNOT_CONNECT                   20463
#define ERROR_NET_CONNECTION_ABORTED               20464
#define ERROR_NET_CONNECTION_RESET                 20465
#define ERROR_NET_FORCE_RETRY                      20466
#define ERROR_NET_INVALID_PROXY_REQUEST            20467
#define ERROR_NET_NEED_UI                          20468
#define ERROR_NET_HANDLE_EXISTS                    20469
#define ERROR_NET_SEC_CERT_DATE_INVALID            20470
#define ERROR_NET_SEC_CERT_CN_INVALID              20471
#define ERROR_NET_HTTP_TO_HTTPS_ON_REDIR           20472
#define ERROR_NET_HTTPS_TO_HTTP_ON_REDIR           20473
#define ERROR_NET_MIXED_SECURITY                   20474
#define ERROR_NET_CHG_POST_IS_NON_SECURE           20475
#define ERROR_NET_POST_IS_NON_SECURE               20476
#define ERROR_NET_CLIENT_AUTH_CERT_NEEDED          20477
#define ERROR_NET_INVALID_CA                       20478
#define ERROR_NET_CLIENT_AUTH_NOT_SETUP            20479
#define ERROR_NET_ASYNC_THREAD_FAILED              20480
#define ERROR_NET_REDIRECT_SCHEME_CHANGE           20481
#define ERROR_NET_DIALOG_PENDING                   20482
#define ERROR_NET_RETRY_DIALOG                     20483
#define ERROR_NET_HTTPS_HTTP_SUBMIT_REDIR          20484
#define ERROR_NET_INSERT_CDROM                     20485
#define ERROR_NET_HTTP_HEADER_NOT_FOUND            20486
#define ERROR_NET_HTTP_DOWNLEVEL_SERVER            20487
#define ERROR_NET_HTTP_INVALID_SERVER_RESPONSE     20488
#define ERROR_NET_HTTP_INVALID_HEADER              20489
#define ERROR_NET_HTTP_INVALID_QUERY_REQUEST       20490
#define ERROR_NET_HTTP_HEADER_ALREADY_EXISTS       20491
#define ERROR_NET_HTTP_REDIRECT_FAILED             20492
#define ERROR_NET_HTTP_NOT_REDIRECTED              20493
#define ERROR_NET_HTTP_COOKIE_NEEDS_CONFIRMATION   20494
#define ERROR_NET_HTTP_COOKIE_DECLINED             20495
#define ERROR_NET_HTTP_REDIRECT_NEEDS_CONFIRMATION 20496
#define ERROR_NET_NO_OPEN_REQUEST                  20497

// start VECTOR error codes

#define ERROR_VECTOR_IS_LOCKED                  20500     /* Vector handle is locked */
#define ERROR_VECTOR_IS_EMPTY                   20501     /* Vector is empty */
#define ERROR_VECTOR_LAYER_NOT_FOUND            20502     /* Layer is not found */
#define ERROR_VECTOR_LAYER_IS_LOCKED            20503     /* Layer is locked */
#define ERROR_VECTOR_LAYER_ALREADY_EXISTS       20504     /* Layer already exists */
#define ERROR_VECTOR_OBJECT_NOT_FOUND           20505     /* Object is not found */
#define ERROR_VECTOR_INVALID_OBJECT_TYPE        20506     /* Invalid object type */
#define ERROR_VECTOR_PEN_NOT_FOUND              20507     /* Pen is not found */
#define ERROR_VECTOR_BRUSH_NOT_FOUND            20508     /* Brush is not found */
#define ERROR_VECTOR_FONT_NOT_FOUND             20509     /* Font is not found */
#define ERROR_VECTOR_BITMAP_NOT_FOUND           20510     /* Bitmap is not found */
#define ERROR_VECTOR_POINT_NOT_FOUND            20511     /* Point is not found */
#define ERROR_VECTOR_ENGINE_NOT_FOUND           20512     /* Vector engine not found */
#define ERROR_VECTOR_INVALID_ENGINE             20513     /* Invalid vector engine */
#define ERROR_VECTOR_CLIPBOARD                  20514     /* Clipboard error */
#define ERROR_VECTOR_CLIPBOARD_IS_EMPTY         20515     /* Clipboard is empty */
#define ERROR_VECTOR_CANT_ADD_TEXT              20516     /* Cannot add text */
#define ERROR_VECTOR_CANT_READ_WMF              20517     /* Cannot read WMF */
#define ERROR_VECTOR_GROUP_NOT_FOUND            20518     /* Group is not found */
#define ERROR_VECTOR_GROUP_ALREADY_EXISTS       20519     /* Group already exists */

// end VECTOR error codes


// start JPEG2000 error codes 

#define ERROR_JP2_FAILURE                       20530  /* Error in JP2 Box values */
#define ERROR_JP2_SIGNATURE                     20531  /* The header does not match the JP2 signature - not a JP2 file */
#define ERROR_JP2_UNSUPPORTED                   20532  /* JP2 file has a feature that is usupported */
#define ERROR_J2K_FAILURE                       20533  /* Invalid save options were specified to the encoder */
#define ERROR_J2K_NO_SOC                        20534  /* File header does not contain SOC marker */
#define ERROR_J2K_NO_SOT                        20535  /* File contains complete header but no compressed image data */
#define ERROR_J2K_INFORMATION_SET               20536  /* Invalid save options were specified or file includes invalid encoded values */
#define ERROR_J2K_LOW_TARGET_SIZE               20537  /* Compression ratio, target file size, or tile size was too small for encoder */
#define ERROR_J2K_DECOMPOSITION_LEVEL           20538  /* Specified Wavelet decomposition level was too high */
#define ERROR_J2K_MARKER_VALUE                  20539  /* Decoder could not translate J2K marker - file is corrupt or invalid */
#define ERROR_J2K_UNSUPPORTED                   20540  /* J2K file has image with more than 30 bits per component */
#define ERROR_J2K_FILTER_MISSING                20541  /* J2K filter is needed to use this function */
#define ERROR_J2K_LOCKED                        20542  /* J2K support is locked */

// end JPEG2000 error codes

// start of twain toolkit error codes

#define ERROR_TWAIN_NO_LIBRARY                  20560
#define ERROR_TWAIN_NOT_AVAILABLE               ERROR_TWAIN_NO_LIBRARY
#define ERROR_TWAIN_INVALID_DLL                 20561
#define ERROR_TWAIN_NOT_INITIALIZED             20562
#define ERROR_TWAIN_CANCELED                    20563
#define ERROR_TWAIN_CHECK_STATUS                20564
#define ERROR_TWAIN_END_OF_LIST                 20565
#define ERROR_TWAIN_CAP_NOT_SUPPORTED           20566
#define ERROR_TWAIN_SOURCE_NOT_OPEN             20567
#define ERROR_TWAIN_BAD_VALUE                   20568
#define ERROR_TWAIN_INVALID_STATE               20569
#define ERROR_TWAIN_CAPS_NEG_NOT_ENDED          20570
#define ERROR_TWAIN_OPEN_FILE                   20571
#define ERROR_TWAIN_INV_HANDLE                  20572
#define ERROR_TWAIN_WRITE_TO_FILE               20573
#define ERROR_TWAIN_INV_VERSION_NUM             20574
#define ERROR_TWAIN_READ_FROM_FILE              20575
#define ERROR_TWAIN_NOT_VALID_FILE              20576
#define ERROR_TWAIN_INV_ACCESS_RIGHT            20577
#define ERROR_TWAIN_CUSTOMBASE                  20578
#define ERROR_TWAIN_DENIED                      20579
#define ERROR_TWAIN_FILEEXISTS                  20580
#define ERROR_TWAIN_FILENOTFOUND                20581
#define ERROR_TWAIN_NOTEMPTY                    20582
#define ERROR_TWAIN_PAPERJAM                    20583
#define ERROR_TWAIN_PAPERDOUBLEFEED             20584
#define ERROR_TWAIN_FILEWRITEERROR              20585
#define ERROR_TWAIN_CHECKDEVICEONLINE           20586

#define ERROR_TWAIN_STOP_ENUMERATION            SUCCESS_ABORT
#define ERROR_TWAIN_STOP_SCAN                   20587

// end of twain toolkit error codes 

// start PaintTools error codes

#define ERROR_PAINT_INTERNAL                    20600   /* Internal error (Call LEAD) */
#define ERROR_PAINT_INV_DATA                    20601   /* Internal error (Call LEAD) */
#define ERROR_PAINT_NO_RESOURCES                20602   /* Internal error (Call LEAD) */
#define ERROR_PAINT_NOT_ENABLED                 20603   /* DigitalPaint capability is required to use this function */

// end PaintTools error codes


// start Container error codes

#define ERROR_CONTAINER_INV_HANDLE              20630
#define ERROR_CONTAINER_INV_OPERATION           20631
#define ERROR_CONTAINER_NO_RESOURCES            20632

// end Container error codes


// start Toolbar error codes

#define ERROR_TOOLBAR_NO_RESOURCES              20660
#define ERROR_TOOLBAR_INV_STATE                 20661
#define ERROR_TOOLBAR_INV_HANDLE                20662

// end Toolbar error codes


// start Automation error codes

#define ERROR_AUTOMATION_INV_HANDLE             20690
#define ERROR_AUTOMATION_INV_STATE              20691

// end Automation error codes

// start Segmentation errors

#define ERROR_INV_SEG_HANDLE                    20700
#define ERROR_INV_SEG_DATA                      20701
#define ERROR_INV_LAYER_DATA                    20702
#define ERROR_MRC_NOT_ALLOCATED                 20703
#define ERROR_INV_MIN_SEGMENT                   20704
#define ERROR_SEGMENTATION_FAILURE              20705
#define ERROR_MRC_INV_PAGE                      20706

// end Segmentation errors

// Start Pdf Errors 

//#define ERROR_PDF_NOT_ENABLED                 20408  
#define ERROR_PDF_FILE_ENCRYPTED                20721  /* The Pdf file is encrypted and the szPassword 
                                                      member of the FILEPDFOPTIONS is not set 
                                                      (no password). */
#define ERROR_PDF_INVALID_PASSWORD              20722  /* Invalid password specified */
#define ERROR_PDF_FAX_NOT_ENABLED               20723  /* LFFAX is required for this function */
#define ERROR_PDF_JPEG_NOT_ENABLED              20724  /* LFCMP is required for this function */
#define ERROR_PDF_INV_DOC_STRUCTURING_COMMENTS  20725  /* Invalid Document Structuring comments (Ps and EPS)*/
#define ERROR_PDF_FONTS_DIRECTORY_NOT_FOUND     20726  /* Could not find Fonts directory */
#define ERROR_PDF_CANNOT_EDIT_FILE              20727  /* Cannot insert, delete, append or replace pages. */
#define ERROR_PDF_BAD_CONTENT                   20728  /* File is corrupted */
#define ERROR_PDF_BAD_INITIALIZATION_FILES      20729  /* Either the files required for initializing the PDF engine were 
                                                      not found or they were found but they are incorrect. */
// Start ISAPI Errors
#define ERROR_ISAPI_CONFIG_NOT_FOUND            20740  /* Config Not Found In Registry*/
#define ERROR_ISAPI_DSN_NOT_FOUND               20741  /* DSN Not Found In ODBC*/
#define ERROR_ISAPI_SQL_EXEC                    20742  /* Table Or Fields Not Found In Database*/
#define ERROR_ISAPI_RECORD_NOT_FOUND            20743  /* Record Not Found In Database*/
#define ERROR_ISAPI_DATABASE_SETTING            20744  /* Error On Database Settings in Registry*/
#define ERROR_ISAPI_DATABASE_SAVE_BITMAP        20745  /* Can't Save Bitmap in Database */
#define ERROR_ISAPI_DATABASE_RECORD_DUPLICATE   20746  /* Record is Alredy Exist in Database , 
                                                      Doublecat In Primary Key*/
#define ERROR_ISAPI_SESSION_FULL                20747  /* Can't Add User To ISAPI SESSION*/
#define ERROR_ISAPI_CACHE_SYSTEM_NOT_ENABLED    20748  /* The caching system not enabled in 
                                                      Configuration Settings*/
#define ERROR_ISAPI_CACHE_MAX_SIZE              20749  /* The maximum size of cache limit*/
#define ERROR_ISAPI_CACHE_SETTING               20750  /* Error On Cache System Settings in Registry */
// end ISAPI errors

// Start SVG errors
#define ERROR_SVG_FILE_SIZE_READ                20770
#define ERROR_SVG_ROOT_NOT_SVG                  20771
#define ERROR_SVG_NOFOUND_ROOT_ELEMENT          20772
#define ERROR_SVG_INV_ELEMENT                   20773
#define ERROR_SVG_DUPLICATED_ATTRIBUTE          20774
#define ERROR_SVG_INV_ATTRIBUTE                 20775
#define ERROR_SVG_INV_ATTRIBUTE_VALUE           20776
#define ERROR_SVG_BAD_CSS_PROPERTY              20777
#define ERROR_SVG_MISSING_REQUIRED_ATTRIBUTE    20778
#define ERROR_SVG_DUPLICATED_ID                 20779
#define ERROR_SVG_INV_COORDINATES_NUMBER        20780
#define ERROR_SVG_INV_STRING_COMMA              20781
// End SVG errors

// start ColorSpace SDK errors
#define ERROR_INVALID_FORMAT                               20785
#define ERROR_UNSUPPORTED_METHOD                           20786
#define ERROR_OPENING_PROFILE                              20787
#define ERROR_INVALID_COLOR_PROFILE                        20788
#define ERROR_INVALID_STRUCT_SIZE                          20789
/*#define ERROR_INVALID_PRAMETER_VALUE                     20790 -- duplicate, use ERROR_INV_PARAMETER instead*/
#define ERROR_U_V_NOT_MULTIPLES                            20791
#define ERROR_NO_NONPLANAR_VERTICAL_SUBSAMPLING_SUPPORTED  20792
#define ERROR_PLANAR_ALIGNMENT_NOT_SUPPORTED               20793
#define ERROR_UNSUPPORTED_CONVERSION                       20794
#define ERROR_TRUNCATE_HEIGHT                              20795
#define ERROR_TRUNCATE_WIDTH                               20796
#define ERROR_TRUNCATE_WIDTH_AND_HEIGHT                    20797
// end ColorSpace SDK errors

#define ERROR_LAYER_MISSING                     20800  /* The PSD Layer is missing */
#define ERROR_BAD_MARKER                        20801  /* Bad JPEG marker */
#define ERROR_AUDIO_MISSING                     20802  /* The audio data is not present */
#define ERROR_DICOM_NOT_ENABLED                 20803  /* DICOM support is required to use this function */
#define ERROR_EXTGRAY_NOT_ENABLED               20804  /* Extended grayscale support is required to use this function */
#define ERROR_FILE_READONLY                     20805  /* File is read-only. Cannot open file with write access */
#define ERROR_BAD_RESYNC_MARKER                 20806  /* Bad JPEG Resync marker */
#define ERROR_LTCLR_MISSING                     20807  /* LTCLR DLL is missing */
#define ERROR_MARKER_SIZE_TOO_BIG               20808  /* The size of the JPEG marker cannot exceed 64K */
#define ERROR_MARKER_MISSING                    20809  /* The required JPEG marker is missing */
#define ERROR_EXTENSIONS_MISSING                20810  /* This file does not contain Exif extensions */
#define ERROR_MARKER_INDEX                      20811  /* The marker index is invalid (too big) */
#define ERROR_NO_PROFILE                        20812  /* The ICC profile was not found */
#define ERROR_DECODING_PROFILE                  20813  /* An error has occured while decoding the profile */
#define ERROR_NULL_PTR                          20814  /* A NULL pointer was passed */
#define ERROR_NO_OVERLAY                        20815  /* The overlay does not exist */
#define ERROR_OVERLAY_INDEX                     20816  /* Something is wrong with the overlay index */

#define ERROR_LTCLR_DLL_NOTLOADED       20900  /* The LTCLR DLL is not loaded */
#define ERROR_LTDLGRES_DLL_NOTLOADED    20901  /* The LDLGRES DLL is not loaded */
#define ERROR_LTDLG_COLOR_NOTINITIALIZE 20902  /* You must initialize LTDLG using L_DlgInit */ 

#define ERROR_IMAGE_SIZE   20910  /* Image size not sufficient */
#define ERROR_NO_MESSAGE   20911  /* There is no message.*/
#define ERROR_INV_PASSWORD 20912  /* Invalid password.*/

#define ERROR_TIFF_COMMAND_NOT_ALLOWED          20913  /* The specified TIFF command is not allowed */
#define ERROR_BAD_TIFF_TAG_VALUE                20914  /* Bad TIFF Tag value. */
#define ERROR_NOT_MULTIPAGE_TIFF_FILE           20915  /* Not Multi-Page TIFF file. */
#define ERROR_DELETE_TIFF_FILE_NOT_ALLOWED      20916  /* Not allowed to delete the TIFF file. */
#define ERROR_COMPACT_ABORTED                   20917  /* The compact function was finished without copying all the pages */
#define ERROR_INV_COLORSPACE                    20918  /* Invalid colorspace */

#define ERROR_LTPRO_NOT_ENABLED                 20930  /* LEADTOOLS Pro Features are not enabled */

#define ERROR_EPRINT_LOCKED                     21000 /* ePrint is locked, Please execute a License Agreement.*/

// HTML Errors
// Internal errors 

#define ERROR_OPT_INVALID_DOC2_PTR          21050 /* Internal error - call LEAD */
#define ERROR_OPT_INVALID_ELEMENTS_PTR      21051 /* Internal error - call LEAD */
#define ERROR_OPT_INVALID_ELEMENT_PTR       21052 /* Internal error - call LEAD */
#define ERROR_OPT_INVALID_ELEMENT_DISP      21053 /* Internal error - call LEAD */
#define ERROR_OPT_INVALID_INPUT_PTR         21054 /* Internal error - call LEAD */
#define ERROR_OPT_INVALID_IMG_PTR           21055 /* Internal error - call LEAD */
#define ERROR_OPT_INVALID_LINK_PTR          21056 /* Internal error - call LEAD */
#define ERROR_OPT_INVALID_SCRIPT_PTR        21057 /* Internal error - call LEAD */
#define ERROR_OPT_INVALID_TABLE_PTR         21058 /* Internal error - call LEAD */

#define ERROR_OPT_CANCELED                  21100 /* HTML operation was canceled */
#define ERROR_OPT_SKIPEVENT                 21101 /* Event skipped */
#define ERROR_OPT_SKIPIMAGE                 21102 /* Image processing skipped */

#define ERROR_OPT_PATH_MISSING_DRIVE        21103 /* Input path misses drive name */
#define ERROR_OPT_INVALID_OUTPUT_PATH       21104 /* Invalid output file path */
#define ERROR_OPT_FILE_CANNOT_FIND          21105 /* Cannot find file */
#define ERROR_OPT_INVALID_INPUT_DIR_NAME    21106 /* Invalid input directory name */
#define ERROR_OPT_INVALID_OUTPUT_DIR_NAME   21107 /* Invalid output directory name */

// start document OCR errors
#define ERROR_DOC_DLL_NOT_LOADED                         21200 /* ScanSoft DLLs are not loaded */
#define ERROR_DOC_INV_PAGE_INDEX                         21201 /* Specified index is not corrected */
#define ERROR_DOC_INV_FILLMETHOD                         21202 /* invalid fill method  */
#define ERROR_DOC_INV_RECOG_MODULE                       21203 /* Invalid recognition module */
#define ERROR_DOC_INV_CHAR_FILTER                        21204 /* invalid character filter */
#define ERROR_DOC_INV_ZONE_TYPE                          21205 /* invalid zone type */
#define ERROR_DOC_INV_LANGID                             21206 /* invalid language Id */
#define ERROR_DOC_INV_SPELL_LANGUAGE                     21207 /* invalid spell language */
#define ERROR_DOC_INV_SECTION_ENUMERATION                21208 /* invalid enumeration section */
#define ERROR_DOC_INV_PAGE_COUNT                         21209 /* invalid page count */

#define ERROR_DOC_ILLEGAL_CODE                           21210 /* Illegal internal code as a parameter */
#define ERROR_DOC_INV_PARAMETER                          21211 /* Set parameter is not acceptable */
#define ERROR_DOC_END_LIST_CODE_PAGES                    21212 /* End of list of the available Code Pages */
#define ERROR_DOC_LOAD_CODE_PAGE_FILE                    21213 /* Error while loading the Code Page Definition file */
#define ERROR_DOC_INV_BUFFER_SIZE                        21214 /* The length of the exported code exceeds the buffer size */
#define ERROR_DOC_INV_CHARACTER_CONVERSION               21215 /* Character conversion is not available for the given character */
#define ERROR_DOC_INV_CHARACTER_LANGUAGE                 21216 /* Conflict: The selected Code Page does not support some characters in the selected languages. There is no exact code in the Code Page for them */
#define ERROR_DOC_INITIALIZE_CODE_PAGE                   21217 /* Character Set and Code Pages module initialization error */
#define ERROR_DOC_INITIALIZE_ENGINE                      21218 /* OCR engine initialization error, or there is no appropriate license */
#define ERROR_DOC_TERMINATE_ENGINE                       21219 /* OCR engine termination error */
#define ERROR_DOC_INITIALIZE_WARNING                     21220 /* Module initialization warning */
#define ERROR_DOC_PROCESS_USER_ABORT                     21221 /* Application has aborted the current process */
#define ERROR_DOC_FINISH_PROCESS                         21222 /* Application has terminated the current recognition process without losing the recognized text */
#define ERROR_DOC_MODULE_MISSING                         21223 /* Module is not present */
#define ERROR_DOC_MODULE_LOAD                            21224 /* OS could not load a module */
#define ERROR_DOC_MODULE_MISSING_ENTRY                   21225 /* Missing entry in a module */
#define ERROR_DOC_MODULE_INVALID                         21226 /* Invalid module */
#define ERROR_DOC_MODULE_INITIALIZE                      21227 /* Module initialization error */
#define ERROR_DOC_FEATURE_NOT_SUPPORTED                  21228 /* The requested function is not available, or there is no appropriate license */
#define ERROR_DOC_GENERAL                                21229 /* General error in the engine */
#define ERROR_DOC_GPF                                    21230 /* General Protection Fault in the engine */
#define ERROR_DOC_NOT_SUPPORT_OS                         21231 /* Not supported operational system */
#define ERROR_DOC_INV_SETTING_FILE                       21232 /* Syntax error in the specified engine Settings file */
#define ERROR_DOC_INV_SETTING                            21233 /* Invalid setting */
#define ERROR_DOC_ENGINE_BUSY                            21234 /* The engine is busy */
#define ERROR_DOC_RECOGNITION_TIME_OUT                   21235 /* Recognition process Time Out */
#define ERROR_DOC_INV_IMAGE_MODULE_SETTING               21236 /* Internal error in image module */
#define ERROR_DOC_NO_MEMORY                              21237 /* Not enough memory during image processing */
#define ERROR_DOC_INV_IMAGE_DIMENSIONS                   21238 /* Invalid rectangle dimensions */
#define ERROR_DOC_INV_IMAGE_RESOLUTION                   21239 /* Non-supported resolution */
#define ERROR_DOC_CANNOT_COMPRESS_IMAGE                  21240 /* Cannot process compressed image */
#define ERROR_DOC_BAD_BITMAP                             21241 /* Invalid bitmap address */
#define ERROR_DOC_NOT_SUPPORT_BPP                        21242 /* Unsupported BitsPerPixel value */
#define ERROR_DOC_INV_IMAGE_MODULE                       21243 /* Internal error in image module */
#define ERROR_DOC_INV_IMAGE_HANDLE                       21244 /* Invalid image handle */
#define ERROR_DOC_BUFFER_OVERFLOW                        21245 /* Buffer overflow during processing the image */
#define ERROR_DOC_ACCESS_DENIED                          21246 /* Image operation denied */
#define ERROR_DOC_NO_MORE_LINES                          21247 /* No more lines in defined image area */
#define ERROR_DOC_BAD_IMAGE_SIZE                         21248 /* Non-supported image size */
#define ERROR_DOC_BAD_ENGINE_MANAGER_MODULE              21249 /* Engine manager module error */
#define ERROR_DOC_NO_RECOGNIZED_TEXT_AVAILABLE           21250 /* No recognized text available, either because the zone is empty or the required recognition module has not been initialized properly */
#define ERROR_DOC_NO_SELECTED_RECOGNITION_MODULE         21251 /* There is no selected recognition module */
#define ERROR_DOC_NO_ZONE                                21252 /* There is no zone in the zone list */
#define ERROR_DOC_INV_ZONE_INDEX                         21253 /* Invalid zone index */
#define ERROR_DOC_INV_ZONE_COORDINATES                   21254 /* Invalid zone coordinates */
#define ERROR_DOC_MOR_INITIALIZE_MODULE                  21255 /* MOR recognition module initialization error */
#define ERROR_DOC_MOR_BASE_FILE_NOT_FOUND                21256 /* MOR recognition module's knowledge base file not found */
#define ERROR_DOC_MOR_BASE_FILE_CORRUPTED                21257 /* MOR recognition module's knowledge base file corrupted */
#define ERROR_DOC_MOR_BASE_FILE_VERSION                  21258 /* Incorrect knowledge base file version */
#define ERROR_DOC_MOR_IMAGE_SIZE                         21259 /* Size of image is too large */
#define ERROR_DOC_MOR_FILE_CORRUPTED                     21260 /* File is corrupted */
#define ERROR_DOC_MOR_BAD_RECOG_MODULE                   21261 /* Internal error in the MOR recognition module */
#define ERROR_DOC_MOR_GPF_MODULE                         21262 /* General Protection Fault in the MOR recognition module */
#define ERROR_DOC_DOT_RECOGNITION                        21263 /* Dot-matrix recognition error */
#define ERROR_DOC_OMR_CHECKMARK_RECOGNITION              21264 /* Checkmark recognition error */
#define ERROR_DOC_HNR_BASE_FILE__CORRUPTED               21265 /* HNR module's knowledge base file is corrupted */
#define ERROR_DOC_HNR_PARAM_OUT_RANGE                    21266 /* Parameter is out of valid range */
#define ERROR_DOC_PER_MODULE                             21267 /* Internal error in the RER module */
#define ERROR_DOC_PER_BASE_FILE_NOT_FOUND                21268 /* RER module's knowledge base file not found */
#define ERROR_DOC_PER_CHAR_SET_EMPTY                     21269 /* Character Set is empty for the RER module */
#define ERROR_DOC_PER_CHAR_SET_NOT_SUPPORTED             21270 /* The specified Character Set not fully supported by the RER recognition module */
#define ERROR_DOC_PER_MODULE_NOT_FOUND                   21271 /* RER specific file not found */
#define ERROR_DOC_SPELL_NO_MEMORY                        21272 /* Not enough memory for the SPL module */
#define ERROR_DOC_SPELL_UNINITIALIZED                    21273 /* Uninitialized spell object */
#define ERROR_DOC_SPELL_FILE_OPEN                        21274 /* Spell file open error */
#define ERROR_DOC_SPELL_FILE_READ                        21275 /* Spell file read error */
#define ERROR_DOC_SPELL_USER_DICTIONARY_WRITE            21276 /* User dictionary write error */
#define ERROR_DOC_SPELL_INV_FILE_FORMAT                  21277 /* Invalid file format */
#define ERROR_DOC_SPELL_INITIALIZE_MODULE                21278 /* Module initialization error */
#define ERROR_DOC_SPELL_USER_DICTIONARY_CLOSE            21279 /* User dictionary close error */
#define ERROR_DOC_SPELL_ILLEGAL_LANGUAGE_SETTING         21280 /* Illegal language setting */
#define ERROR_DOC_SPELL_NO_MORE_ITEMS                    21281 /* No more suggestions/items available */
#define ERROR_DOC_SPELL_CHECK_ERROR                      21282 /* Internal checking error */
#define ERROR_DOC_SPELL_ITEM_ALREADY_EXIST               21283 /* Item already exists in the User dictionary */
#define ERROR_DOC_SPELL_ITEM_NOT_EXIST                   21284 /* Item doesn't exist in the User dictionary */
#define ERROR_DOC_SPELL_ITEM_NOT_INSERTED                21285 /* Item wasn't inserted into the User dictionary */
#define ERROR_DOC_SPELL_SECTION_NOT_EXIST                21286 /* Section does not exist in the User dictionary */
#define ERROR_DOC_SPELL_BAD_SYNTEX                       21287 /* Regular expression syntax error */
#define ERROR_DOC_OUTPUT_NOT_INITIALIZED                 21288 /* Output format conversion subsystem was not initialized */
#define ERROR_DOC_OUTPUT_CONVERT_NOT_EXIST               21289 /* No more converters available */
#define ERROR_DOC_OUTPUT_CONVERT_DLL_NOT_EXIST           21290 /* There is no converter DLL file */
#define ERROR_DOC_OUTPUT_BAD_PARAMETER                   21291 /* Wrong parameter or parameter not found */
#define ERROR_DOC_OUTPUT_FILE_CORRUPTED                  21292 /* TMP file is corrupted */
#define ERROR_DOC_OUTPUT_FILE_NOT_FOUND                  21293 /* TMP file not found */
#define ERROR_DOC_OUTPUT_CREATE_FILE                     21294 /* Cannot create TMP file */
#define ERROR_DOC_OUTPUT_SEEK_FILE                       21295 /* Cannot seek in TMP file */
#define ERROR_DOC_OUTPUT_READ_FILE                       21296 /* TMP file read error */
#define ERROR_DOC_OUTPUT_WRITE_FILE                      21297 /* TMP file write error */
#define ERROR_DOC_OUTPUT_CLOSE_FILE                      21298 /* Cannot close TMP file */
#define ERROR_DOC_OUTPUT_LOAD_DLL                        21299 /* Cannot load the DLL file */
#define ERROR_DOC_OUTPUT_END_OF_FILE                     21300 /* Internal error in ATMTXT module. (End-Of-File detected) */
#define ERROR_DOC_OUTPUT_NO_MEMORY                       21301 /* Not enough memory for ATMTXT/TMP module */
#define ERROR_DOC_OUTPUT_IMAGE_NOT_AVAILABLE             21302 /* Image is not available for inserting into the output */
#define ERROR_DOC_OUTPUT_NOT_REGISTER_INFO_ITEM          21303 /* Not enough memory to register an info item */
#define ERROR_DOC_TXT_FILE_OPEN                          21304 /* Output file open error */
#define ERROR_DOC_TXT_FILE_READ                          21305 /* Output file read error */
#define ERROR_DOC_TXT_FILE_NOT_FOUND                     21306 /* Output file not found */
#define ERROR_DOC_TXT_FILE_WRITE                         21307 /* Output file write error */
#define ERROR_DOC_TXT_INV_COMMAND_SEQUENCE               21308 /* Invalid command sequence in the ICF file */
#define ERROR_DOC_TXT_CREATE_FILE                        21309 /* Cannot create the output file */
#define ERROR_DOC_TXT_NO_MEMORY                          21310 /* Not enough memory for ATMTXT/TXT module */
#define ERROR_DOC_TXT_INV_DOCUMENT                       21311 /* Invalid document type */
#define ERROR_DOC_TXT_FILE_TOO_LARGE                     21312 /* File too large */
#define ERROR_DOC_TXT_WARNING_OUTPUT_FILE                21313 /* There was a WARNING during the output file conversion  */
#define ERROR_DOC_TXT_OPERATION_CANCELLED                21314 /* Operation cancelled  */
#define ERROR_DOC_TXT_ILLEGAL_OPTION                     21315 /* Illegal option specified */
#define ERROR_DOC_TXT_ENCRYPTED_SOURCE                   21316 /* Encrypted source */
#define ERROR_DOC_TXT_SEEK_ERROR                         21317 /* Output file seek error */
#define ERROR_DOC_BRAILLE_BAD_INITIALIZATION             21318 /* Braille recognition module initialization error */
#define ERROR_DOC_MATRIX_BAD_INITIALIZATION              21319 /* Matrix matching recognition module initialization error */
#define ERROR_DOC_MTX_BAD_INITIALIZATION                 21320 /* M/TEXT recognition module initialization error */
#define ERROR_DOC_NON_IMPLEMENTED_FEATURE                21321 /* Non-implemented feature */
#define ERROR_DOC_NOT_FOUND_ZONE                         21322 /* couldn't find specific zone */
#define ERROR_DOC_NO_SELECTED_ZONE                       21323 /* No available selected zone */
#define ERROR_DOC_EXPORT_PAGE                            21324 /* can't export the specified page */
#define ERROR_DOC_LOCK_PAGE                              21325 /* can't lock the specified page */
#define ERROR_DOC_ACTIVE_PAGE                            21326 /* can't set the specified page to be activated. */
#define ERROR_DOC_LOCKED                                 21327 /* The OCR document engine is locked. */
#define ERROR_DOC_OUTPUT_PDF_LOCKED                      21328 /* Output PDF engine is locked. */

#define ERROR_ABC_LOCKED                                 21361 /* ABC file format is locked*/


// end document OCR errors

#endif  /* _L_OCXERR_H_ */
