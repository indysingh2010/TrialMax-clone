/*************************************************************
   Lttwn.h - twain module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTTWN_H)
#define LTTWN_H

#if !defined(L_LTTWN_API)
   #define L_LTTWN_API
#endif // #if !defined(L_LTTWN_API)

#include "Lttyp.h"
#include "Lterr.h"
#include "twain.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

/****************************************************************
   Enums/defines/macros
****************************************************************/

// Error Codes
#define TWAIN_SUCCESS                  SUCCESS
#define TWAIN_SUCCESS_RETRY            TWAIN_SUCCESS + 1
#define TWAIN_SUCCESS_ABORT            TWAIN_SUCCESS + 2

// Default TWFF
#define TWFF_DEFAULT                   99

// Function flags
// L_TwainSetProperties flags
#define LTWAIN_PROPERTIES_SET          1   // Set values as the current values
#define LTWAIN_PROPERTIES_RESET        2   // Reset values to their default values

// L_TwainGetProperties flags
#define LTWAIN_PROPERTIES_GETCURRENT   3   // Get the currently set values
#define LTWAIN_PROPERTIES_GETDEFAULT   4   // Get the default values

// L_TwainSetCapability flags
#define LTWAIN_CAPABILITY_SET          1   // Set values as the current values
#define LTWAIN_CAPABILITY_RESET        2   // Reset values to their default values

// L_TwainGetCapability flags
#define LTWAIN_CAPABILITY_GETCURRENT   3   // Get the currently set values
#define LTWAIN_CAPABILITY_GETDEFAULT   4   // Get the default values
#define LTWAIN_CAPABILITY_GETVALUES    5   // Enum available values

// L_TwainEnumCapabilities flags
#define LTWAIN_CAPABILITY_DONTGET      0   // Do not get the capability values

// L_TwainGetSources functions flags
#define LTWAIN_SOURCE_ENUMERATE_ALL       0x0000
#define LTWAIN_SOURCE_ENUMERATE_DEFAULT   0x0001

// L_TwainGetNumericContainerValue index values
#define LTWAIN_VALUE_COUNT                -1
#define LTWAIN_VALUE_CURRENT              -2
#define LTWAIN_VALUE_DEFAULT              -3
#define LTWAIN_VALUE_MINIMUM              -4
#define LTWAIN_VALUE_MAXIMUM              -5
#define LTWAIN_VALUE_STEPSIZE             -6

// L_TwainAcquire flags
#define LTWAIN_SHOW_USER_INTERFACE        0x0001
#define LTWAIN_MODAL_USER_INTERFACE       0x0002
#define LTWAIN_BITMAP_TYPE_DISK           0x0004
#define LTWAIN_USE_THREAD_MODE            0x0008
#define LTWAIN_CHECK_ALL_DEFAULT_BPP      0x0010
#define LTWAIN_KEEPOPEN                   0x0020
#define LTWAIN_MEMORY_CHECK_IMAGEINFO     0x0040
#define LTWAIN_IMAGESIZE_UNDEFINED        0x0080

// L_TwainOpenTemplateFile 
#define LTWAIN_TEMPLATE_OPEN_READ         0
#define LTWAIN_TEMPLATE_OPEN_WRITE        1

// Flags for uTransferMode parameter of L_TwainAcquireMulti ...
#define LTWAIN_FILE_MODE                  0x001
#define LTWAIN_BUFFER_MODE                0x002
#define LTWAIN_NATIVE_MODE                0x004

// L_TwainInitSession2 flags
#define LTWAIN_INIT_MULTI_THREADED        0x0001

// L_TwainGetJPEGCompression flags
#define LTWAIN_GET_JPEG_COMPRESSION          0x0001
#define LTWAIN_GET_DEFAULT_JPEG_COMPRESSION  0x0002

// L_TwainSetJPEGCompression flags
#define LTWAIN_SET_JPEG_COMPRESSION          0x0001
#define LTWAIN_RESET_JPEG_COMPRESSION        0x0002

typedef enum
{
   FILESYSTEMMSG_CHANGEDIRECTORY          = 0x0801,
   FILESYSTEMMSG_CREATEDIRECTORY          = 0x0802,
   FILESYSTEMMSG_DELETE                   = 0x0803,
   FILESYSTEMMSG_FORMATMEDIA              = 0x0804,
   FILESYSTEMMSG_GETCLOSE                 = 0x0805,
   FILESYSTEMMSG_GETFIRSTFILE             = 0x0806,
   FILESYSTEMMSG_GETINFO                  = 0x0807,
   FILESYSTEMMSG_GETNEXTFILE              = 0x0808,
   FILESYSTEMMSG_RENAME                   = 0x0809,
   FILESYSTEMMSG_COPY                     = 0x080A,
   FILESYSTEMMSG_AUTOMATICCAPTUREDIRECTORY= 0x080B,
} FILESYSTEMMSG;

typedef enum _LTWAINNUMERICTYPE
{
   TWAINNUMERICTYPE_TW_INT8      = TWTY_INT8,
   TWAINNUMERICTYPE_TW_INT16     = TWTY_INT16,
   TWAINNUMERICTYPE_TW_INT32     = TWTY_INT32,
   TWAINNUMERICTYPE_TW_UINT8     = TWTY_UINT8,
   TWAINNUMERICTYPE_TW_UINT16    = TWTY_UINT16,
   TWAINNUMERICTYPE_TW_UINT32    = TWTY_UINT32,
   TWAINNUMERICTYPE_TW_BOOL      = TWTY_BOOL,
   TWAINNUMERICTYPE_TW_FIX32     = TWTY_FIX32,
   TWAINNUMERICTYPE_TW_FRAME     = TWTY_FRAME,
   TWAINNUMERICTYPE_TW_STR32     = TWTY_STR32,
   TWAINNUMERICTYPE_TW_STR64     = TWTY_STR64,
   TWAINNUMERICTYPE_TW_STR128    = TWTY_STR128,
   TWAINNUMERICTYPE_TW_STR255    = TWTY_STR255,
   TWAINNUMERICTYPE_TW_STR1024   = TWTY_STR1024,
   TWAINNUMERICTYPE_TW_UNI512    = TWTY_UNI512
} LTWAINNUMERICTYPE;

typedef enum
{
   TWAIN_TRANSFER_FILE     = 0x001,
   TWAIN_TRANSFER_MEMORY   = 0x002,
   TWAIN_TRANSFER_NATIVE   = 0x004,
} TWAIN_TRANSFER_MODES;

#define TWAIN_NEGOTIATE_BRIGHTNESS  0x0010
#define TWAIN_NEGOTIATE_CONTRAST    0x0020
#define TWAIN_NEGOTIATE_HIGHLIGHT   0x0040

#define TWAIN_RGB_RESPONSE_SET   0x0001
#define TWAIN_RGB_RESPONSE_RESET 0x0002

// TWAINSESSION Handle
typedef HANDLE                HTWAINSESSION;
typedef HTWAINSESSION *       pHTWAINSESSION;

// TWAINTEMPLATEFILE Handle
typedef HANDLE                HTWAINTEMPLATEFILE;
typedef HTWAINTEMPLATEFILE *  pHTWAINTEMPLATEFILE;

/****************************************************************
   Classes/structures
****************************************************************/

// TWAIN structure LTWAINSOURCE
typedef struct _LTWAINSOURCEA
{
   L_UINT      uStructSize;
   L_CHAR *    pszTwainSourceName;
} LTWAINSOURCEA, * pLTWAINSOURCEA;
#if defined(FOR_UNICODE)
typedef struct _LTWAINSOURCE
{
   L_UINT      uStructSize;
   L_TCHAR *   pszTwainSourceName;
} LTWAINSOURCE, * pLTWAINSOURCE;
#else
typedef LTWAINSOURCEA LTWAINSOURCE;
typedef pLTWAINSOURCEA pLTWAINSOURCE;
#endif // #if defined(FOR_UNICODE)

// TWAIN structure LTWAINPROPERTYQUERY
typedef struct _LTWAINPROPERTYQUERY
{
   L_UINT   uStructSize;
   L_UINT   uType;
   union
   {
      pTW_ONEVALUE     pltwOneValue;
      pTW_RANGE        pltwRange;
      pTW_ENUMERATION  pltwEnumeration;
      pTW_ARRAY        pltwArray;
   };
} LTWAINPROPERTYQUERY, * pLTWAINPROPERTYQUERY;

#define LTWAINPROPERTYQUERYSIZE sizeof (LTWAINPROPERTYQUERY)

// Properties structures
typedef struct _IMAGERESOLUTION
{
   L_UINT   uStructSize;
   L_INT    nUnitOfResolution;   // Unit of resolution in general (Meters, Inches, ...)
   L_INT    nBitsPerPixel;       // Bits per pixel of image
   L_FLOAT  fHorzResolution;     // Horizontal resolution of image
   L_FLOAT  fVertResolution;     // Vertical resolution of image
   L_FLOAT  fXScaling;           // Unit of resolution in the X direction
   L_FLOAT  fYScaling;           // Unit of resolution in the Y direction
   L_FLOAT  fRotationAngle;      // Rotation angle of scanned image
   L_FLOAT  fLeftMargin;         // Left margin of scanned image
   L_FLOAT  fRightMargin;        // Right margin of scanned image
   L_FLOAT  fTopMargin;          // Top margin of scanned image
   L_FLOAT  fBottomMargin;       // Bottom margin of scanned image
} IMAGERESOLUTION, * pIMAGERESOLUTION;

#define IMAGERESOLUTIONSIZE sizeof (IMAGERESOLUTION)

typedef struct _DATATRANSFERA
{
   L_UINT   uStructSize;
   L_INT    nFillOrder;          // CCITT decoding variable, can be MSB or LSB
   L_INT    nBufMemCompression;  // Compression used when using beffered memory transfer
   L_INT    nTransferMode;       // Data transfer mode, can be Native, Buffered memory, Disk File
   L_INT    nScanFileFormat;     // File format used when scanning, can be (BMP, JPEG, PICT, TIFF, XBM)
   L_INT    nMemBufSize;         // Maximum size to be used in the Buffered memory transfer mode
   L_BOOL   bSaveToOneFile;      // Used to save all scanned images to one MULTI PAGE file
   L_BOOL   bAppendToFile;       // Used to append images to an existing MULTI PAGE file
   L_BOOL   bDumpMemBufsToFile;  // Used to save the memory buffers in case memory buffered transfer is chosen to the file name below
   L_CHAR   szFileName [250];    // The file used to save scanned images.
} DATATRANSFERA, * pDATATRANSFERA;

#if defined(FOR_UNICODE)
typedef struct _DATATRANSFER
{
   L_UINT   uStructSize;
   L_INT    nFillOrder;          // CCITT decoding variable, can be MSB or LSB
   L_INT    nBufMemCompression;  // Compression used when using beffered memory transfer
   L_INT    nTransferMode;       // Data transfer mode, can be Native, Buffered memory, Disk File
   L_INT    nScanFileFormat;     // File format used when scanning, can be (BMP, JPEG, PICT, TIFF, XBM)
   L_INT    nMemBufSize;         // Maximum size to be used in the Buffered memory transfer mode
   L_BOOL   bSaveToOneFile;      // Used to save all scanned images to one MULTI PAGE file
   L_BOOL   bAppendToFile;       // Used to append images to an existing MULTI PAGE file
   L_BOOL   bDumpMemBufsToFile;  // Used to save the memory buffers in case memory buffered transfer is chosen to the file name below
   L_TCHAR  szFileName [250];    // The file used to save scanned images.
} DATATRANSFER, * pDATATRANSFER;
#define DATATRANSFERSIZE sizeof (DATATRANSFER)
#else
typedef DATATRANSFERA DATATRANSFER;
typedef pDATATRANSFERA pDATATRANSFER;
#define DATATRANSFERSIZE sizeof (DATATRANSFERA)
#endif // #if defined(FOR_UNICODE)


typedef struct _APPLICATIONDATAA
{
   L_UINT   uStructSize;
   HWND     hWnd;                         // Window handle of an application, may not be NULL
   L_CHAR   szManufacturerName [256];     // Application manufacturer name
   L_CHAR   szAppProductFamily [256];     // Application product family
   L_CHAR   szVersionInfo [32];           // Application version info
   L_CHAR   szAppName [256];              // Application Name
} APPLICATIONDATAA, * pAPPLICATIONDATAA;

typedef struct _IMAGEEFFECTSA
{
   L_UINT   uStructSize;
   L_INT    nPixFlavor;       // Pixel flavor, 0 for Chocolate, 1 for Vanilla
   L_INT    nColorScheme;     // Color scheme used, B/W, Gray256, RGB
   L_FLOAT  fHighLight;       // Highlight value
   L_FLOAT  fShadow;          // Shadow value
   L_FLOAT  fBrightness;      // Brightness value
   L_FLOAT  fContrast;        // Contrast value
   L_CHAR   szHalfTone[32];   // Half tone pattern string
} IMAGEEFFECTSA, * pIMAGEEFFECTSA;

#if defined(FOR_UNICODE)
typedef struct _APPLICATIONDATA
{
   L_UINT   uStructSize;
   HWND     hWnd;                         // Window handle of an application, may not be NULL
   L_TCHAR  szManufacturerName [256];     // Application manufacturer name
   L_TCHAR  szAppProductFamily [256];     // Application product family
   L_TCHAR  szVersionInfo [32];           // Application version info
   L_TCHAR  szAppName [256];              // Application Name
   L_UINT16 uLanguage;
   L_UINT16 uCountry;
} APPLICATIONDATA, * pAPPLICATIONDATA;

typedef struct _IMAGEEFFECTS
{
   L_UINT   uStructSize;
   L_INT    nPixFlavor;       // Pixel flavor, 0 for Chocolate, 1 for Vanilla
   L_INT    nColorScheme;     // Color scheme used, B/W, Gray256, RGB
   L_FLOAT  fHighLight;       // Highlight value
   L_FLOAT  fShadow;          // Shadow value
   L_FLOAT  fBrightness;      // Brightness value
   L_FLOAT  fContrast;        // Contrast value
   L_TCHAR  szHalfTone[32];   // Half tone pattern string
} IMAGEEFFECTS, * pIMAGEEFFECTS;

#define APPLICATIONDATASIZE sizeof (APPLICATIONDATA)
#define IMAGEEFFECTSSIZE sizeof (IMAGEEFFECTS)
#else
typedef APPLICATIONDATAA APPLICATIONDATA;
typedef pAPPLICATIONDATAA pAPPLICATIONDATA;
typedef IMAGEEFFECTSA IMAGEEFFECTS;
typedef pIMAGEEFFECTSA pIMAGEEFFECTS;
#define APPLICATIONDATASIZE sizeof (APPLICATIONDATAA)
#endif 

typedef struct _LTWAINPROPERTIESA
{
   L_UINT            uStructSize;
   L_BOOL            bPaperSource;     // Paper Source used when scanning (auto feed)
   L_INT             nMaxNumOfPages;   // The maximum number of pages to scan
   L_INT             nDuplexScanning;  // Duplex scanning enable
   IMAGERESOLUTION   ImageRes;         // Image resolution structure
   DATATRANSFERA     DataTransfer;     // Data transfer structure
   IMAGEEFFECTSA     ImageEff;         // Image effects used
} LTWAINPROPERTIESA, * pLTWAINPROPERTIESA;

#if defined(FOR_UNICODE)
typedef struct _LTWAINPROPERTIES
{
   L_UINT            uStructSize;
   L_BOOL            bPaperSource;     // Paper Source used when scanning (auto feed)
   L_INT             nMaxNumOfPages;   // The maximum number of pages to scan
   L_INT             nDuplexScanning;  // Duplex scanning enable
   IMAGERESOLUTION   ImageRes;         // Image resolution structure
   DATATRANSFER      DataTransfer;     // Data transfer structure
   IMAGEEFFECTS      ImageEff;         // Image effects used
} LTWAINPROPERTIES, * pLTWAINPROPERTIES;

#define LTWAINPROPERTIESSIZE sizeof (LTWAINPROPERTIES)
#else
typedef LTWAINPROPERTIESA LTWAINPROPERTIES;
typedef pLTWAINPROPERTIESA pLTWAINPROPERTIES;
#define LTWAINPROPERTIESSIZE sizeof (LTWAINPROPERTIESA)
#endif // 

// Acquire list call back structure (user data)
typedef struct _ACQUIRELIST
{
   L_UINT      uStructSize;
   HBITMAPLIST hBitmap;
} ACQUIRELIST, *pACQUIRELIST;

#define ACQUIRELISTSIZE sizeof (ACQUIRELIST)

typedef struct _FASTCONFIG
{
   L_UINT   uStructSize;
   L_UINT   uTransferMode;
   L_INT    nFileFormat;
   L_UINT32 ulBufferSize; // -1 : means not available, or not tested yet.
   L_UINT   uTime;       // -1 : means not available, or not tested yet.
   L_INT    nBitsPerPixel;
   L_BOOL   bSuccess;
} FASTCONFIG, * pFASTCONFIG;

// TWAIN structure LTWAINSOURCEINFO
typedef struct _LTWAINSOURCEINFOA
{
   L_UINT      uStructSize;
   L_CHAR *    pszTwnSourceName;
   L_CHAR *    pszTwnProductFamily;
   L_CHAR *    pszTwnManufacturer;
} LTWAINSOURCEINFOA, * pLTWAINSOURCEINFOA;

typedef struct _tagTransferOptionsA
{
   L_UINT               uStructSize;
   TWAIN_TRANSFER_MODES TransferMode;
   L_CHAR               szFileName[MAX_PATH];
   L_UINT               uFileFormat;
   L_UINT               uCompType;
} TRANSFEROPTIONSA, * pTRANSFEROPTIONSA;

#if defined(FOR_UNICODE)
typedef struct _LTWAINSOURCEINFO
{
   L_UINT      uStructSize;
   L_TCHAR *   pszTwnSourceName;
   L_TCHAR *   pszTwnProductFamily;
   L_TCHAR *   pszTwnManufacturer;
} LTWAINSOURCEINFO, * pLTWAINSOURCEINFO;

typedef struct _tagTransferOptions
{
   L_UINT               uStructSize;
   TWAIN_TRANSFER_MODES TransferMode;
   L_TCHAR              szFileName[MAX_PATH];
   L_UINT               uFileFormat;
   L_UINT               uCompType;
} TRANSFEROPTIONS, * pTRANSFEROPTIONS;
#else
typedef TRANSFEROPTIONSA TRANSFEROPTIONS;
typedef pTRANSFEROPTIONSA pTRANSFEROPTIONS;
typedef LTWAINSOURCEINFOA LTWAINSOURCEINFO;
typedef pLTWAINSOURCEINFOA pLTWAINSOURCEINFO;
#endif // #if defined(FOR_UNICODE)

/****************************************************************
   Callback typedefs
****************************************************************/

// Callback function
typedef L_INT (pEXT_CALLBACK LTWAINBITMAPCALLBACK)(
                           HTWAINSESSION hSession,
                           pBITMAPHANDLE pBitmap,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LTWAINCAPABILITYCALLBACK)(
                           HTWAINSESSION hSession,
                           L_UINT uCap,
                           pTW_CAPABILITY pCapability,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LTWAINSETPROPERTYCALLBACK)(
                           HTWAINSESSION hSession,
                           L_UINT uCap,
                           L_INT nStatus,
                           L_VOID * pValue,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LTWAINSAVECAPCALLBACK)(
                           HTWAINSESSION hSession,
                           pTW_CAPABILITY pCapability,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LTWAINSAVEERRORCALLBACK)(
                           HTWAINSESSION hSession,
                           pTW_CAPABILITY pCapability,
                           L_UINT uError,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LTWAINFINDFASTCONFIG)(
                           HTWAINSESSION hSession,
                           pFASTCONFIG pResConfig,
                           L_VOID *pUserData);


typedef L_INT (pEXT_CALLBACK LTWAINSOURCEINFOCALLBACKA)(
                           HTWAINSESSION hSession,
                           pLTWAINSOURCEINFOA pSourceInfo,
                           L_VOID * pUserData);

typedef L_VOID (pEXT_CALLBACK LTWAINACQUIRECALLBACKA)(
                           HTWAINSESSION hSession,
                           L_INT nPage,
                           L_CHAR * pszFileName,
                           L_BOOL bFinishScan,
                           L_VOID *pUserData);

#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK LTWAINSOURCEINFOCALLBACK)(
                           HTWAINSESSION hSession,
                           pLTWAINSOURCEINFO pSourceInfo,
                           L_VOID * pUserData);

typedef L_VOID (pEXT_CALLBACK LTWAINACQUIRECALLBACK)(
                           HTWAINSESSION hSession,
                           L_INT nPage,
                           L_TCHAR * pszFileName,
                           L_BOOL bFinishScan,
                           L_VOID *pUserData);
#else
typedef LTWAINACQUIRECALLBACKA LTWAINACQUIRECALLBACK;
typedef LTWAINSOURCEINFOCALLBACKA LTWAINSOURCEINFOCALLBACK;
#endif // #if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK LTWAINDEVICEEVENTCALLBACK)(
                           HTWAINSESSION hSession,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LTWAINTEMPLATECALLBACK)(
                           HTWAINSESSION hSession,
                           pTW_CAPABILITY pCapability,
									L_INT nStatus,
                           L_VOID * pUserData);

/****************************************************************
   Function prototypes
****************************************************************/
L_LTTWN_API L_INT EXT_FUNCTION L_TwainInitSessionA (pHTWAINSESSION phSession, pAPPLICATIONDATAA pAppData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainInitSession2A(pHTWAINSESSION phSession, pAPPLICATIONDATAA pAppData, L_UINT uFlags);

#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainInitSession (pHTWAINSESSION phSession, pAPPLICATIONDATA pAppData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainInitSession2(pHTWAINSESSION phSession, pAPPLICATIONDATA pAppData, L_UINT uFlags);
#else
#define L_TwainInitSession L_TwainInitSessionA
#define L_TwainInitSession2 L_TwainInitSession2A
#endif // #if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainEndSession (pHTWAINSESSION phSession);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetPropertiesA (HTWAINSESSION hSession, pLTWAINPROPERTIESA pltProperties, L_UINT uFlags, LTWAINSETPROPERTYCALLBACK pfnCallBack, L_VOID * pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetPropertiesA (HTWAINSESSION hSession, pLTWAINPROPERTIESA pltProperties, L_UINT uStructSize, L_UINT uFlags);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainAcquireListA (HTWAINSESSION hSession, HBITMAPLIST hBitmap, L_CHAR * lpszTemplateFile, L_UINT uFlags);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainAcquireA (HTWAINSESSION hSession, pBITMAPHANDLE pBitmap, L_UINT uStructSize, LTWAINBITMAPCALLBACK pfnCallBack, L_UINT uFlags, L_CHAR * lpszTemplateFile, L_VOID * pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSelectSourceA (HTWAINSESSION hSession, pLTWAINSOURCEA pltSource);
#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetProperties (HTWAINSESSION hSession, pLTWAINPROPERTIES pltProperties, L_UINT uFlags, LTWAINSETPROPERTYCALLBACK pfnCallBack, L_VOID * pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetProperties (HTWAINSESSION hSession, pLTWAINPROPERTIES pltProperties, L_UINT uStructSize, L_UINT uFlags);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainAcquireList (HTWAINSESSION hSession, HBITMAPLIST hBitmap, L_TCHAR * lpszTemplateFile, L_UINT uFlags);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainAcquire (HTWAINSESSION hSession, pBITMAPHANDLE pBitmap, L_UINT uStructSize, LTWAINBITMAPCALLBACK pfnCallBack, L_UINT uFlags, L_TCHAR * lpszTemplateFile, L_VOID * pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSelectSource (HTWAINSESSION hSession, pLTWAINSOURCE pltSource);
#else
#define L_TwainSetProperties L_TwainSetPropertiesA
#define L_TwainGetProperties L_TwainGetPropertiesA
#define L_TwainAcquireList L_TwainAcquireListA
#define L_TwainAcquire L_TwainAcquireA
#define L_TwainSelectSource L_TwainSelectSourceA
#endif // #if defined(FOR_UNICODE)

L_LTTWN_API L_INT EXT_FUNCTION L_TwainQueryProperty (HTWAINSESSION hSession, L_UINT uCapability, pLTWAINPROPERTYQUERY* ppltProperty, L_UINT uStructSize);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainStartCapsNeg (HTWAINSESSION hSession);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainEndCapsNeg (HTWAINSESSION hSession);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetCapability (HTWAINSESSION hSession, pTW_CAPABILITY pCapability, L_UINT uFlags);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetCapability (HTWAINSESSION hSession, pTW_CAPABILITY pCapability, L_UINT uFlags);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainEnumCapabilities (HTWAINSESSION hSession, LTWAINCAPABILITYCALLBACK pfnCallBack, L_UINT uFlags, L_VOID * pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainCreateNumericContainerOneValue (TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT_PTR uValue);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainCreateNumericContainerRange (TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uMinValue, L_UINT32 uMaxValue, L_UINT32 uStepSize, L_UINT32 uDefaultValue, L_UINT32 uCurrentValue);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainCreateNumericContainerArray (TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uNumOfItems, L_VOID * pData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainCreateNumericContainerEnum (TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uNumOfItems, L_UINT32 uCurrentIndex, L_UINT32 uDefaultIndex, L_VOID * pData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerValue (TW_CAPABILITY * pCapability, L_INT nIndex, L_VOID ** ppValue);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainFreeContainer (TW_CAPABILITY * pCapability);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainFreePropQueryStructure (pLTWAINPROPERTYQUERY * ppltProperty);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainTemplateDlgA (HTWAINSESSION hSession, L_CHAR * lpszTemplateFile, LTWAINSAVECAPCALLBACK pfnCallBack, LTWAINSAVEERRORCALLBACK pfnErCallBack, L_VOID * pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainOpenTemplateFileA (HTWAINSESSION hSession, HTWAINTEMPLATEFILE * phFile, L_CHAR * lpszTemplateFile, L_UINT uAccess);
#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainTemplateDlg (HTWAINSESSION hSession, L_TCHAR * lpszTemplateFile, LTWAINSAVECAPCALLBACK pfnCallBack, LTWAINSAVEERRORCALLBACK pfnErCallBack, L_VOID * pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainOpenTemplateFile (HTWAINSESSION hSession, HTWAINTEMPLATEFILE * phFile, L_TCHAR * lpszTemplateFile, L_UINT uAccess);
#else
#define L_TwainTemplateDlg L_TwainTemplateDlgA
#define L_TwainOpenTemplateFile L_TwainOpenTemplateFileA
#endif // #if defined(FOR_UNICODE)

L_LTTWN_API L_INT EXT_FUNCTION L_TwainAddCapabilityToFile (HTWAINSESSION hSession, HTWAINTEMPLATEFILE hFile, pTW_CAPABILITY pCapability);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetCapabilityFromFile (HTWAINSESSION hSession, HTWAINTEMPLATEFILE hFile, pTW_CAPABILITY * ppCapability, L_UINT uIndex);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumOfCapsInFile (HTWAINSESSION hSession, HTWAINTEMPLATEFILE hFile, L_UINT * puCapCount);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainCloseTemplateFile (HTWAINSESSION hSession, HTWAINTEMPLATEFILE hFile);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetExtendedImageInfo (HTWAINSESSION hSession, TW_EXTIMAGEINFO * ptwExtImgInfo);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainFreeExtendedImageInfoStructure (TW_EXTIMAGEINFO ** pptwExtImgInfo);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainLockContainer (TW_CAPABILITY * pCapability, void ** ppContainer);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainUnlockContainer (TW_CAPABILITY * pCapability);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerItemType (TW_CAPABILITY * pCapability, L_INT * pnItemType);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerINTValue (TW_CAPABILITY * pCapability, L_INT nIndex, L_INT * pnValue);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerUINTValue (TW_CAPABILITY * pCapability, L_INT nIndex, L_UINT * puValue);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerBOOLValue (TW_CAPABILITY * pCapability, L_INT nIndex, L_BOOL * pbValue);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerFIX32Value (TW_CAPABILITY * pCapability, L_INT nIndex, TW_FIX32 * ptwFix);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerFRAMEValue (TW_CAPABILITY * pCapability, L_INT nIndex, TW_FRAME * ptwFrame);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerSTRINGValue (TW_CAPABILITY * pCapability, L_INT nIndex, TW_STR1024 twString);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetNumericContainerUNICODEValue (TW_CAPABILITY * pCapability, L_INT nIndex, TW_UNI512 twUniCode);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainAcquireMultiA(HTWAINSESSION         hSession,
                                      L_CHAR *              pszBaseFileName,
                                      L_UINT                uFlags,
                                      L_UINT                uTransferMode,
                                      L_INT                 nFormat,
                                      L_INT                 nBitsPerPixel,
                                      L_BOOL                bMultiPageFile,
                                      L_UINT32              uUserBufSize,
                                      L_BOOL                bUsePrefferedBuffer,
                                      LTWAINACQUIRECALLBACKA pfnCallBack,
                                      L_VOID              * pUserData);

#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainAcquireMulti(HTWAINSESSION         hSession,
                                      L_TCHAR *             pszBaseFileName,
                                      L_UINT                uFlags,
                                      L_UINT                uTransferMode,
                                      L_INT                 nFormat,
                                      L_INT                 nBitsPerPixel,
                                      L_BOOL                bMultiPageFile,
                                      L_UINT32              uUserBufSize,
                                      L_BOOL                bUsePrefferedBuffer,
                                      LTWAINACQUIRECALLBACK pfnCallBack,
                                      L_VOID              * pUserData);
#else
#define L_TwainAcquireMulti L_TwainAcquireMultiA
#endif // #if defined(FOR_UNICODE)
L_LTTWN_API L_BOOL EXT_FUNCTION L_IsTwainAvailable(HWND hWnd);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainFindFastConfigA(HTWAINSESSION        hSession,
                                        L_CHAR *             pszWorkingFolder,
                                        L_UINT               uFlags,
                                        L_INT                nBitsPerPixel,
                                        L_INT                nBufferIteration,
                                        pFASTCONFIG          pInFastConfigs,
                                        L_INT                nInFastConfigsCount,
                                        pFASTCONFIG        * ppTestConfigs,
                                        L_INT              * pnTestConfigsCount,
                                        pFASTCONFIG          pOutBestConfig,
                                        L_UINT               uStructSize,
                                        LTWAINFINDFASTCONFIG pfnCallBack,
                                        L_VOID             * pUserData);

#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainFindFastConfig(HTWAINSESSION        hSession,
                                        L_TCHAR *            pszWorkingFolder,
                                        L_UINT               uFlags,
                                        L_INT                nBitsPerPixel,
                                        L_INT                nBufferIteration,
                                        pFASTCONFIG          pInFastConfigs,
                                        L_INT                nInFastConfigsCount,
                                        pFASTCONFIG        * ppTestConfigs,
                                        L_INT              * pnTestConfigsCount,
                                        pFASTCONFIG          pOutBestConfig,
                                        L_UINT               uStructSize,
                                        LTWAINFINDFASTCONFIG pfnCallBack,
                                        L_VOID             * pUserData);
#else
#define L_TwainFindFastConfig L_TwainFindFastConfigA
#endif // #if defined(FOR_UNICODE)

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetScanConfigs(HTWAINSESSION   hSession,
                                        L_INT           nBitsPerPixel,
                                        L_UINT          uTransferMode,
                                        L_INT           nBufferIteration,
                                        pFASTCONFIG    *ppFastConfig,
                                        L_UINT          uStructSize,
                                        L_INT          *pnFastConfigCount);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainFreeScanConfig(HTWAINSESSION   hSession,
                                        pFASTCONFIG    *ppFastConfig,
                                        L_INT           nFastConfigCount);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetSourcesA (HTWAINSESSION            hSession,
                                     LTWAINSOURCEINFOCALLBACKA pfnCallBack,
                                     L_UINT                   uStructSize,
                                     L_UINT                   uFlags,
                                     L_VOID                 * pUserData);
#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetSources (HTWAINSESSION            hSession,
                                     LTWAINSOURCEINFOCALLBACK pfnCallBack,
                                     L_UINT                   uStructSize,
                                     L_UINT                   uFlags,
                                     L_VOID                 * pUserData);
#else
#define L_TwainGetSources L_TwainGetSourcesA
#endif // #if defined(FOR_UNICODE)

L_LTTWN_API L_INT EXT_FUNCTION L_TwainEnableShowUserInterfaceOnly(HTWAINSESSION  hSession,
                                                     L_BOOL         bEnable);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainCancelAcquire(HTWAINSESSION hSession);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainQueryFileSystem(HTWAINSESSION hSession, FILESYSTEMMSG FileMsg, pTW_FILESYSTEM pTwFile);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetJPEGCompression(HTWAINSESSION hSession, pTW_JPEGCOMPRESSION pTwJpegComp, L_UINT uFlag);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetJPEGCompression(HTWAINSESSION hSession, pTW_JPEGCOMPRESSION pTwJpegComp, L_UINT uFlag);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetTransferOptionsA(HTWAINSESSION hSession, pTRANSFEROPTIONSA pTransferOpts);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetTransferOptionsA(HTWAINSESSION hSession, pTRANSFEROPTIONSA pTransferOpts, L_UINT uStructSize);

#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetTransferOptions(HTWAINSESSION hSession, pTRANSFEROPTIONS pTransferOpts);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetTransferOptions(HTWAINSESSION hSession, pTRANSFEROPTIONS pTransferOpts, L_UINT uStructSize);
#else
#define L_TwainSetTransferOptions L_TwainSetTransferOptionsA
#define L_TwainGetTransferOptions L_TwainGetTransferOptionsA
#endif // #if defined(FOR_UNICODE)

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetSupportedTransferMode(HTWAINSESSION hSession, L_UINT * pTransferModes);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetResolution(HTWAINSESSION hSession, pTW_FIX32 pXRes, pTW_FIX32 pYRes);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetResolution(HTWAINSESSION hSession, pTW_FIX32 pXRes, pTW_FIX32 pYRes);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetImageFrame(HTWAINSESSION hSession, pTW_FRAME pFrame);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetImageFrame(HTWAINSESSION hSession, pTW_FRAME pFrame);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetImageUnit(HTWAINSESSION hSession, L_INT nUnit);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetImageUnit(HTWAINSESSION hSession, L_INT * pnUnit);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetImageBitsPerPixel(HTWAINSESSION hSession, L_INT nBitsPerPixel);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetImageBitsPerPixel(HTWAINSESSION hSession, L_INT * pnBitsPerPixel);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetImageEffects(HTWAINSESSION hSession, L_UINT32 ulFlags, pTW_FIX32 pBrightness, pTW_FIX32 pContrast, pTW_FIX32 pHighlight);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetImageEffects(HTWAINSESSION hSession, L_UINT32 ulFlags, pTW_FIX32 pBrightness, pTW_FIX32 pContrast, pTW_FIX32 pHighlight);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetAcquirePageOptions(HTWAINSESSION hSession, L_INT nPaperSize, L_INT nPaperDirection);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetAcquirePageOptions(HTWAINSESSION hSession, L_INT * pnPaperSize, L_INT * pnPaperDirection);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetRGBResponse(HTWAINSESSION hSession, pTW_RGBRESPONSE pRgbResponse, L_INT nBitsPerPixel, L_UINT uFlag);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainShowProgress(HTWAINSESSION hSession, L_BOOL bShow);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainEnableDuplex(HTWAINSESSION hSession, L_BOOL bEnableDuplex);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetDuplexOptions(HTWAINSESSION hSession, L_BOOL * pbEnableDuplex, L_INT * pnDuplexMode);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetMaxXferCount(HTWAINSESSION hSession, L_INT nMaxXferCount);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetMaxXferCount(HTWAINSESSION hSession, L_INT * pnMaxXferCount);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainStopFeeder(HTWAINSESSION hSession);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetDeviceEventCallback(HTWAINSESSION hSession, LTWAINDEVICEEVENTCALLBACK pfnCallBack, L_VOID * pUserData);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetDeviceEventData(HTWAINSESSION hSession, pTW_DEVICEEVENT pDeviceEvent);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetDeviceEventCapability(HTWAINSESSION hSession, pTW_CAPABILITY pDeviceCap);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetDeviceEventCapability(HTWAINSESSION hSession, pTW_CAPABILITY pDeviceCap);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainResetDeviceEventCapability(HTWAINSESSION hSession, pTW_CAPABILITY pDeviceCap);

L_LTTWN_API L_INT EXT_FUNCTION L_TwainFastAcquireA(HTWAINSESSION         hSession,
                                                   L_CHAR *              pszBaseFileName,
                                                   L_UINT                uFlags,
                                                   L_UINT                uTransferMode,
                                                   L_INT                 nFormat,
                                                   L_INT                 nBitsPerPixel,
                                                   L_BOOL                bMultiPageFile,
                                                   L_UINT32              uUserBufSize,
                                                   L_BOOL                bUsePrefferedBuffer,
                                                   LTWAINACQUIRECALLBACKA pfnCallBack,
                                                   L_VOID              * pUserData);

#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainFastAcquire(HTWAINSESSION         hSession,
                                                  L_TCHAR *             pszBaseFileName,
                                                  L_UINT                uFlags,
                                                  L_UINT                uTransferMode,
                                                  L_INT                 nFormat,
                                                  L_INT                 nBitsPerPixel,
                                                  L_BOOL                bMultiPageFile,
                                                  L_UINT32              uUserBufSize,
                                                  L_BOOL                bUsePrefferedBuffer,
                                                  LTWAINACQUIRECALLBACK pfnCallBack,
                                                  L_VOID              * pUserData);
#else
#define L_TwainFastAcquire L_TwainFastAcquireA
#endif // #if defined(FOR_UNICODE)

L_LTTWN_API TW_FIX32 EXT_FUNCTION L_TwainFloatToFix32(L_FLOAT fFloater);
L_LTTWN_API L_FLOAT EXT_FUNCTION L_TwainFix32ToFloat(pTW_FIX32 pFix32);

#if defined(LEADTOOLS_V16_OR_LATER)

L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetCustomDSDataA(HTWAINSESSION hSession, pTW_CUSTOMDSDATA pCustomData, L_CHAR * pszFileName);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetCustomDSDataA(HTWAINSESSION hSession, pTW_CUSTOMDSDATA pCustomData, L_CHAR * pszFileName);

#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainGetCustomDSData(HTWAINSESSION hSession, pTW_CUSTOMDSDATA pCustomData, L_TCHAR * pszFileName);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSetCustomDSData(HTWAINSESSION hSession, pTW_CUSTOMDSDATA pCustomData, L_TCHAR * pszFileName);
#else
#define L_TwainGetCustomDSData   L_TwainGetCustomDSDataA
#define L_TwainSetCustomDSData   L_TwainSetCustomDSDataA
#endif // #if defined(FOR_UNICODE)

L_LTTWN_API L_INT EXT_FUNCTION L_TwainSaveTemplateFileA(HTWAINSESSION hSession, L_CHAR * lpszTemplateFile, L_UINT uFlags, L_UINT* puCapabilities, L_UINT uCount, LTWAINTEMPLATECALLBACK pfnCallBack, L_VOID* pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainLoadTemplateFileA(HTWAINSESSION hSession, L_CHAR * lpszTemplateFile, LTWAINTEMPLATECALLBACK pfnCallBack, L_VOID* pUserData);

#if defined(FOR_UNICODE)
L_LTTWN_API L_INT EXT_FUNCTION L_TwainSaveTemplateFile(HTWAINSESSION hSession, L_TCHAR * lpszTemplateFile, L_UINT uFlags, L_UINT* puCapabilities, L_UINT uCount, LTWAINTEMPLATECALLBACK pfnCallBack, L_VOID* pUserData);
L_LTTWN_API L_INT EXT_FUNCTION L_TwainLoadTemplateFile(HTWAINSESSION hSession, L_TCHAR * lpszTemplateFile, LTWAINTEMPLATECALLBACK pfnCallBack, L_VOID* pUserData);
#else
#define L_TwainSaveTemplateFile L_TwainSaveTemplateFileA
#define L_TwainLoadTemplateFile L_TwainLoadTemplateFileA
#endif // #if defined(FOR_UNICODE)

#endif // #if defined(LEADTOOLS_V16_OR_LATER)

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTTWN_H)
