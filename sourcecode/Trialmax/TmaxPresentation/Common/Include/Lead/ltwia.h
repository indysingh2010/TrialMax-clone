/*************************************************************
   Ltwia.h - WIA module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTWIA_H)
#define LTWIA_H

#if !defined(L_LTWIA_API)
   #define L_LTWIA_API
#endif // #if !defined(L_LTWIA_API)

#include "initguid.h"
#include <wia.h>

#define L_HEADER_ENTRY
#include "Ltpck.h"

#include "Lttyp.h"
#include "Lterr.h"

/****************************************************************
   Enums/defines/macros
****************************************************************/
#define WIA_SUCCESS  SUCCESS
#define WIA_RETRY    WIA_SUCCESS + 1
#define WIA_ABORT    WIA_SUCCESS + 2

#define L_WIA_SELECT_DEVICE_NODEFAULT        0x00000001  // Force the L_WiaSelectDeviceDlg method to display 
                                                         // the Select Device dialog box even when there is 
                                                         // only one device detected.

#define L_WIA_DEVICE_DIALOG_SINGLE_IMAGE     0x00000002  // Only allow one image to be selected
#define L_WIA_SHOW_USER_INTERFACE            0x00000003  // Show the manufacturer's image acquisition user interface dialog.
#define L_WIA_DEVICE_DIALOG_USE_COMMON_UI    0x00000004  // Give preference to the system-provided UI, if available

#define L_WIA_ACQUIRE_START_OF_PAGE          0x00000001  // Incdicates the start of the page currently acquiring.
#define L_WIA_ACQUIRE_END_OF_PAGE            0x00000002  // Incdicates the end of the page currently acquiring.

typedef enum _L_WIADEVICETYPE
{
    WiaDeviceTypeDefault          = 0,
    WiaDeviceTypeScanner          = 1,
    WiaDeviceTypeDigitalCamera    = 2,
    WiaDeviceTypeStreamingVideo   = 3
} L_WIADEVICETYPE;

typedef enum _L_WIAVERSION
{
   WiaVersion1 = 1,
   WiaVersion2 = 2
} L_WIAVERSION;

/****************************************************************
   Classes/structures
****************************************************************/

// Device ID structure LWIADEVICEID
typedef struct _LWIADEVICEIDA
{
   L_UINT      uStructSize;
   L_CHAR*     pszDeviceId;      // Selected device ID.
   L_CHAR*     pszDeviceName;    // Selected device name.
   L_CHAR*     pszDeviceDesc;    // Selected device description.
} LWIADEVICEIDA, * pLWIADEVICEIDA;

#if defined(FOR_UNICODE)
typedef struct _LWIADEVICEID
{
   L_UINT      uStructSize;
   L_TCHAR*    pszDeviceId;      // Selected device ID.
   L_TCHAR*    pszDeviceName;    // Selected device name.
   L_TCHAR*    pszDeviceDesc;    // Selected device description.
} LWIADEVICEID, * pLWIADEVICEID;
#else
typedef LWIADEVICEIDA LWIADEVICEID;
typedef pLWIADEVICEIDA pLWIADEVICEID;
#endif // #if defined(FOR_UNICODE)

typedef struct _LWIAACQUIREOPTIONSA
{
   L_UINT      uStructSize;
   L_UINT      uMemBufSize;            // Device's memory transfer buffer size.
   L_BOOL      bDoubleBuffer;          // Enable/Disable double buffer.
   L_BOOL      bOverwriteExisting;     // This member determines whether to overwrite any existing file on disk or not.
   L_BOOL      bSaveToOneFile;         // Flag to determine whether to save all acquired pages to one multi-page file,
                                       // (if the selected format doesn't supports multi-page then one file will be
                                       // created and it will contain the last acquired page).
   L_BOOL      bAppend;                // Append the acquired files to the existing multi-page support file.
   L_CHAR      szFileName[MAX_PATH];   // The file used to save scanned images.
} LWIAACQUIREOPTIONSA, * pLWIAACQUIREOPTIONSA;

#if defined(FOR_UNICODE)
typedef struct _LWIAACQUIREOPTIONS
{
   L_UINT      uStructSize;
   L_UINT      uMemBufSize;            // Device's memory transfer buffer size.
   L_BOOL      bDoubleBuffer;          // Enable/Disable double buffer.
   L_BOOL      bOverwriteExisting;     // This member determines whether to overwrite any existing file on disk or not.
   L_BOOL      bSaveToOneFile;         // Flag to determine whether to save all acquired pages to one multi-page file,
                                       // (if the selected format doesn't supports multi-page then one file will be
                                       // created and it will contain the last acquired page).
   L_BOOL      bAppend;                // Append the acquired files to the existing multi-page support file.
   L_TCHAR     szFileName[MAX_PATH];   // The file used to save scanned images.
} LWIAACQUIREOPTIONS, * pLWIAACQUIREOPTIONS;
#else
typedef LWIAACQUIREOPTIONSA LWIAACQUIREOPTIONS;
typedef pLWIAACQUIREOPTIONSA pLWIAACQUIREOPTIONS;
#endif // #if defined(FOR_UNICODE)

typedef struct _LWIADATATRANSFER
{
   L_UINT   uStructSize;
   GUID *   pguidFormat;            // File format used when scanning, can be (WiaImgFmt_BMP, WiaImgFmt_JPEG, WiaImgFmt_TIFF, WiaImgFmt_GIF, WiaImgFmt_PNG, ...)
   L_INT    nCompression;           // Compression used when using buffered memory transfer, can be WIA_COMPRESSION_BI_RLE4, WIA_COMPRESSION_G3, ...
   L_INT    nTransferMode;          // Data transfer mode, can be TYMED_CALLBACK, TYMED_FILE.
   L_INT    nImageDataType;         // Set the current data type of the image about to be transferred, valid values:
                                    // (WIA_DATA_COLOR, WIA_DATA_GRAYSCALE, WIA_DATA_DITHER, ...) - when format is NOT Raw
                                    // (WIA_DATA_GRAYSCALE, WIA_DATA_RAW_BGR, WIA_DATA_RAW_CMY, ...) - when format is Raw
} LWIADATATRANSFER, * pLWIADATATRANSFER;

typedef struct _LWIAIMAGERESOLUTION
{
   L_UINT   uStructSize;
   L_INT    nBitsPerPixel;       // Bits per pixel of image
   L_INT    nHorzResolution;     // Horizontal resolution of image
   L_INT    nVertResolution;     // Vertical resolution of image
   L_INT    nXScaling;           // Unit of resolution in the X direction
   L_INT    nYScaling;           // Unit of resolution in the Y direction
   L_INT    nRotationAngle;      // Rotation angle of the scanned image, can be (PORTRAIT, LANDSCAPE, ROT180 or ROT270).
   L_INT    nXPos;               // X coordinate of the upper-left corner of the selection area.
   L_INT    nYPos;               // Y coordinate of the upper-left corner of the selection area.
   L_INT    nWidth;              // Width of the selection area to acquire.
   L_INT    nHeight;             // Height of the selection area to acquire.
} LWIAIMAGERESOLUTION, * pLWIAIMAGERESOLUTION;

typedef struct _LWIAIMAGEEFFECTS
{
   L_UINT   uStructSize;
   L_INT    nBrightness;   // Brightness value (From -1000 To 1000).
   L_INT    nContrast;     // Contrast value (From -1000 To 1000).
} LWIAIMAGEEFFECTS, * pLWIAIMAGEEFFECTS;

typedef struct _LWIAPROPERTIES
{
   L_UINT               uStructSize;
   L_INT                nScanningMode;    // Specifies scanner paper source and duplex mode (FEEDER, FLATBED, DUPLEX, FRONT_ONLY, ...)
   L_INT                nImageType;       // Type of picture you want to scan (WIA_INTENT_IMAGE_TYPE_COLOR, WIA_INTENT_IMAGE_TYPE_GRAYSCALE, WIA_INTENT_IMAGE_TYPE_TEXT, ...)
   L_INT                nOrientation;     // Orientation value relative to PORTRAIT, can be any of the following (PORTRAIT, LANDSCAPE, ROT180 or ROT270).
   L_INT                nMaxNumOfPages;   // The maximum number of pages to scan
   pLWIAIMAGERESOLUTION pImageResolution; // Image resolution structure
   pLWIADATATRANSFER    pDataTransfer;    // Data transfer structure
   pLWIAIMAGEEFFECTS    pImageEffects;    // Image effects used
} LWIAPROPERTIES, * pLWIAPROPERTIES;

typedef struct _LWIACAPLISTVALUES
{
   L_UINT      uStructSize;
   L_UINT      uCapListValuesCount;
   L_VOID **   ppCapListValues;
} LWIACAPLISTVALUES, * pLWIACAPLISTVALUES;

typedef struct _LWIACAPFLAGVALUES
{
   L_UINT   uStructSize;
   L_UINT   uCapFlagValues;
   L_UINT   uCapFlagNominalValue;
} LWIACAPFLAGVALUES, * pLWIACAPFLAGVALUES;

typedef struct _LWIACAPRANGEVALUES
{
   L_UINT   uStructSize;
   L_INT    nCapRangeMinValue;
   L_INT    nCapRangeMaxValue;
   L_INT    nCapRangeNominalValue;
   L_INT    nCapRangeStep;
} LWIACAPRANGEVALUES, * pLWIACAPRANGEVALUES;

typedef struct _LWIACAPABILITYVALUES
{
   L_UINT               uStructSize;
   pLWIACAPFLAGVALUES   pCapFlagsValues;
   pLWIACAPLISTVALUES   pCapListValues;
   pLWIACAPRANGEVALUES  pCapRangeValues;
} LWIACAPABILITYVALUES, * pLWIACAPABILITYVALUES;

typedef struct _LWIACAPABILITYA
{
   L_UINT                  uStructSize;
   L_UINT                  uPropertyID;
   L_CHAR                  szPropertyName[MAX_PATH];
   L_UINT                  uVariableType;
   L_UINT                  uPropertyAttributes;
   pLWIACAPABILITYVALUES   pCapabilityValues;
} LWIACAPABILITYA, * pLWIACAPABILITYA;

#if defined(FOR_UNICODE)
typedef struct _LWIACAPABILITY
{
   L_UINT                  uStructSize;
   L_UINT                  uPropertyID;
   L_TCHAR                 szPropertyName[MAX_PATH];
   L_UINT                  uVariableType;
   L_UINT                  uPropertyAttributes;
   pLWIACAPABILITYVALUES   pCapabilityValues;
} LWIACAPABILITY, * pLWIACAPABILITY;
#else
typedef LWIACAPABILITYA LWIACAPABILITY;
typedef pLWIACAPABILITYA pLWIACAPABILITY;
#endif // #if defined(FOR_UNICODE)

// WIASESSION Handle
typedef L_VOID *        HWIASESSION;
typedef HWIASESSION *   pHWIASESSION;

/****************************************************************
   CallBack typedefs
****************************************************************/

typedef L_INT (pEXT_CALLBACK LWIAENUMDEVICESCALLBACKA)(
                           HWIASESSION hSession,
                           pLWIADEVICEIDA pDeviceID,
                           L_VOID * pUserData);
#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK LWIAENUMDEVICESCALLBACK)(
                           HWIASESSION hSession,
                           pLWIADEVICEID pDeviceID,
                           L_VOID * pUserData);
#else
typedef LWIAENUMDEVICESCALLBACKA LWIAENUMDEVICESCALLBACK;
#endif // #if defined(FOR_UNICODE)

typedef L_INT (pEXT_CALLBACK LWIAACQUIRECALLBACKA)(
                           HWIASESSION hSession,
                           pBITMAPHANDLE pBitmap,
                           L_CHAR * pszFilename,
                           L_UINT32 uPercent,
                           L_UINT32 uFlags,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LWIAACQUIREFILECALLBACKA)(
                           HWIASESSION hSession,
                           L_CHAR * pszFilename,
                           L_UINT32 uPercent,
                           L_UINT32 uFlags,
                           L_VOID * pUserData);
#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK LWIAACQUIRECALLBACK)(
                           HWIASESSION hSession,
                           pBITMAPHANDLE pBitmap,
                           L_TCHAR * pszFilename,
                           L_UINT32 uPercent,
                           L_UINT32 uFlags,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LWIAACQUIREFILECALLBACK)(
                           HWIASESSION hSession,
                           L_TCHAR * pszFilename,
                           L_UINT32 uPercent,
                           L_UINT32 uFlags,
                           L_VOID * pUserData);
#else
typedef LWIAACQUIRECALLBACKA LWIAACQUIRECALLBACK;
typedef LWIAACQUIREFILECALLBACKA LWIAACQUIREFILECALLBACK;
#endif // #if defined(FOR_UNICODE)

typedef L_INT (pEXT_CALLBACK LWIAENUMITEMSCALLBACK)(
                           HWIASESSION hSession,
                           L_INT nItemsCount,
                           L_VOID * pItem,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LWIASETPROPERTIESCALLBACK)(
                           HWIASESSION hSession,
                           L_INT PropertyID,
                           L_INT nError,
                           L_UINT uValueType,
                           L_VOID * pValue,
                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK LWIAENUMCAPABILITIESCALLBACKA)(
                           HWIASESSION hSession,
                           L_INT nCapsCount,
                           pLWIACAPABILITYA pCapability,
                           L_VOID * pUserData);
#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK LWIAENUMCAPABILITIESCALLBACK)(
                           HWIASESSION hSession,
                           L_INT nCapsCount,
                           pLWIACAPABILITY pCapability,
                           L_VOID * pUserData);
#else
typedef  LWIAENUMCAPABILITIESCALLBACKA LWIAENUMCAPABILITIESCALLBACK;
#endif // #if defined(FOR_UNICODE)

typedef L_INT (pEXT_CALLBACK LWIAENUMFORMATSCALLBACK)(
                           HWIASESSION hSession,
                           L_INT nFormatsCount,
                           L_INT nTransferMode,
                           GUID * pFormat,
                           L_VOID * pUserData);


/****************************************************************
   Function prototypes
****************************************************************/

L_LTWIA_API L_BOOL EXT_FUNCTION L_WiaIsAvailable (L_UINT uWiaVersion);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaInitSession (L_UINT uWiaVersion, pHWIASESSION phSession);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaEndSession (HWIASESSION hSession);


L_LTWIA_API L_INT EXT_FUNCTION L_WiaEnumDevicesA(HWIASESSION hSession, 
                                                LWIAENUMDEVICESCALLBACKA pfnCallBack, 
                                                L_VOID * pUserData);
#if defined(FOR_UNICODE)
L_LTWIA_API L_INT EXT_FUNCTION L_WiaEnumDevices(HWIASESSION hSession, 
                                                LWIAENUMDEVICESCALLBACK pfnCallBack, 
                                                L_VOID * pUserData);
#else
#define L_WiaEnumDevices L_WiaEnumDevicesA
#endif //#if defined(FOR_UNICODE)

L_LTWIA_API L_INT EXT_FUNCTION L_WiaSelectDeviceDlg(HWIASESSION hSession, 
                                                    HWND hWndParent, 
                                                    L_UINT32 uDeviceType, 
                                                    L_UINT32 uFlags);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaSelectDeviceA(HWIASESSION hSession, L_CHAR* pszDeviceId);
L_LTWIA_API L_CHAR* EXT_FUNCTION L_WiaGetSelectedDeviceA(HWIASESSION hSession);

#if defined(FOR_UNICODE)
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSelectDevice(HWIASESSION hSession, L_TCHAR* pszDeviceId);
L_LTWIA_API L_TCHAR* EXT_FUNCTION L_WiaGetSelectedDevice(HWIASESSION hSession);
#else
#define L_WiaSelectDevice L_WiaSelectDeviceA
#define L_WiaGetSelectedDevice L_WiaGetSelectedDeviceA
#endif // #if defined(FOR_UNICODE)

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetSelectedDeviceType(HWIASESSION hSession, 
                                                          L_UINT32 * puDeviceType);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetProperties(HWIASESSION hSession, 
                                                  L_VOID * pItem, 
                                                  pLWIAPROPERTIES pProperties);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetProperties(HWIASESSION hSession, 
                                                  L_VOID * pItem, 
                                                  pLWIAPROPERTIES pProperties, 
                                                  LWIASETPROPERTIESCALLBACK pfnCallBack, 
                                                  L_VOID * pUserData);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaAcquireA(HWIASESSION hSession, 
                                            HWND hWndParent, 
                                            L_UINT32 uFlags, 
                                            L_VOID * pSourceItem,
                                            pLWIAACQUIREOPTIONSA pAcquireOptions, 
                                            L_INT * pnFilesCount,
                                            L_CHAR *** pppszFilePaths,
                                            LWIAACQUIRECALLBACKA pfnCallBack, 
                                            L_VOID * pUserData);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaAcquireToFileA(HWIASESSION hSession, 
                                                  HWND hWndParent, 
                                                  L_UINT32 uFlags, 
                                                  L_VOID * pSourceItem,
                                                  pLWIAACQUIREOPTIONSA pAcquireOptions, 
                                                  L_INT * pnFilesCount,
                                                  L_CHAR *** pppszFilePaths,
                                                  LWIAACQUIREFILECALLBACKA pfnCallBack, 
                                                  L_VOID * pUserData);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaAcquireSimpleA(L_UINT uWiaVersion, 
                                                  HWND hWndParent, 
                                                  L_UINT32 uDeviceType, 
                                                  L_UINT32 uFlags, 
                                                  L_VOID * pSourceItem,
                                                  pLWIAACQUIREOPTIONSA pAcquireOptions, 
                                                  L_INT * pnFilesCount,
                                                  L_CHAR *** pppszFilePaths,
                                                  LWIAACQUIRECALLBACKA pfnCallBack, 
                                                  L_VOID * pUserData);
#if defined(FOR_UNICODE)
L_LTWIA_API L_INT EXT_FUNCTION L_WiaAcquire(HWIASESSION hSession, 
                                            HWND hWndParent, 
                                            L_UINT32 uFlags, 
                                            L_VOID * pSourceItem,
                                            pLWIAACQUIREOPTIONS pAcquireOptions, 
                                            L_INT * pnFilesCount,
                                            L_TCHAR *** pppszFilePaths,
                                            LWIAACQUIRECALLBACK pfnCallBack, 
                                            L_VOID * pUserData);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaAcquireToFile(HWIASESSION hSession, 
                                                  HWND hWndParent, 
                                                  L_UINT32 uFlags, 
                                                  L_VOID * pSourceItem,
                                                  pLWIAACQUIREOPTIONS pAcquireOptions, 
                                                  L_INT * pnFilesCount,
                                                  L_TCHAR *** pppszFilePaths,
                                                  LWIAACQUIREFILECALLBACK pfnCallBack, 
                                                  L_VOID * pUserData);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaAcquireSimple(L_UINT uWiaVersion, 
                                                  HWND hWndParent, 
                                                  L_UINT32 uDeviceType, 
                                                  L_UINT32 uFlags, 
                                                  L_VOID * pSourceItem,
                                                  pLWIAACQUIREOPTIONS pAcquireOptions, 
                                                  L_INT * pnFilesCount,
                                                  L_TCHAR *** pppszFilePaths,
                                                  LWIAACQUIRECALLBACK pfnCallBack, 
                                                  L_VOID * pUserData);
#else
#define L_WiaAcquire L_WiaAcquireA
#define L_WiaAcquireToFile L_WiaAcquireToFileA
#define L_WiaAcquireSimple L_WiaAcquireSimpleA
#endif //#if defined(FOR_UNICODE)

L_LTWIA_API L_INT EXT_FUNCTION L_WiaStartVideoPreview(HWIASESSION hSession, 
                                                      HWND hWndParent, 
                                                      L_BOOL bStretchToFitParent);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaResizeVideoPreview(HWIASESSION hSession, 
                                                       L_BOOL bStretchToFitParent);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaEndVideoPreview(HWIASESSION hSession);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaAcquireImageFromVideoA(HWIASESSION hSession, 
                                                          L_CHAR* pszFileName, 
                                                          L_SIZE_T* puLength);
#if defined(FOR_UNICODE)
L_LTWIA_API L_INT EXT_FUNCTION L_WiaAcquireImageFromVideo(HWIASESSION hSession, 
                                                          L_TCHAR* pszFileName, 
                                                          L_SIZE_T* puLength);
#else
#define L_WiaAcquireImageFromVideo L_WiaAcquireImageFromVideoA
#endif  // #if defined(FOR_UNICODE)

L_LTWIA_API L_BOOL EXT_FUNCTION L_WiaIsVideoPreviewAvailable(HWIASESSION hSession);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetRootItem(HWIASESSION hSession, 
                                                L_VOID * pItem, 
                                                L_VOID ** ppWiaRootItem);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaEnumChildItems(HWIASESSION hSession, 
                                                   L_VOID * pWiaRootItem,
                                                   LWIAENUMITEMSCALLBACK pfnCallBack, 
                                                   L_VOID * pUserData);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaFreeItem(HWIASESSION hSession, L_VOID * pItem);


L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertyLongA(HWIASESSION hSession, 
                                                    L_VOID * pItem, 
                                                    L_CHAR* pszID, 
                                                    L_UINT32 uID, 
                                                    L_INT32* plValue);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertyLongA(HWIASESSION hSession, 
                                                    L_VOID * pItem, 
                                                    L_CHAR* pszID, 
                                                    L_UINT32 uID, 
                                                    L_INT32 lValue);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertyGUIDA(HWIASESSION hSession, 
                                                    L_VOID * pItem, 
                                                    L_CHAR* pszID, 
                                                    L_UINT32 uID, 
                                                    GUID* pGuidValue);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertyGUIDA(HWIASESSION hSession, 
                                                    L_VOID * pItem, 
                                                    L_CHAR* pszID, 
                                                    L_UINT32 uID, 
                                                    GUID* pGuidValue);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertyStringA(HWIASESSION hSession, 
                                                      L_VOID * pItem, 
                                                      L_CHAR* pszID, 
                                                      L_UINT32 uID, 
                                                      L_CHAR* pszValue, 
                                                      L_SIZE_T* puLength);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertyStringA(HWIASESSION hSession, 
                                                      L_VOID * pItem, 
                                                      L_CHAR* pszID, 
                                                      L_UINT32 uID, 
                                                      L_CHAR* pszValue);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertySystemTimeA(HWIASESSION hSession, 
                                                          L_VOID * pItem, 
                                                          L_CHAR* pszID, 
                                                          L_UINT32 uID, 
                                                          LPSYSTEMTIME pValue);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertySystemTimeA(HWIASESSION hSession, 
                                                          L_VOID * pItem, 
                                                          L_CHAR* pszID, 
                                                          L_UINT32 uID, 
                                                          LPSYSTEMTIME pValue);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertyBufferA(HWIASESSION hSession, 
                                                      L_VOID * pItem, 
                                                      L_CHAR* pszID, 
                                                      L_UINT32 uID, 
                                                      L_UCHAR* pValue, 
                                                      L_SIZE_T* puSize);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertyBufferA(HWIASESSION hSession, 
                                                      L_VOID * pItem, 
                                                      L_CHAR* pszID, 
                                                      L_UINT32 uID, 
                                                      L_UCHAR* pValue, 
                                                      L_SIZE_T uSize);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaEnumCapabilitiesA(HWIASESSION hSession, 
                                                     L_VOID * pItem,
                                                     L_UINT uFlags, // For future use.
                                                     LWIAENUMCAPABILITIESCALLBACKA pfnCallBack, 
                                                     L_VOID * pUserData);

#if defined(FOR_UNICODE)
L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertyLong(HWIASESSION hSession, 
                                                    L_VOID * pItem, 
                                                    L_TCHAR* pszID, 
                                                    L_UINT32 uID, 
                                                    L_INT32* plValue);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertyLong(HWIASESSION hSession, 
                                                    L_VOID * pItem, 
                                                    L_TCHAR* pszID, 
                                                    L_UINT32 uID, 
                                                    L_INT32 lValue);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertyGUID(HWIASESSION hSession, 
                                                    L_VOID * pItem, 
                                                    L_TCHAR* pszID, 
                                                    L_UINT32 uID, 
                                                    GUID* pGuidValue);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertyGUID(HWIASESSION hSession, 
                                                    L_VOID * pItem, 
                                                    L_TCHAR* pszID, 
                                                    L_UINT32 uID, 
                                                    GUID* pGuidValue);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertyString(HWIASESSION hSession, 
                                                      L_VOID * pItem, 
                                                      L_TCHAR* pszID, 
                                                      L_UINT32 uID, 
                                                      L_TCHAR* pszValue, 
                                                      L_SIZE_T* puLength);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertyString(HWIASESSION hSession, 
                                                      L_VOID * pItem, 
                                                      L_TCHAR* pszID, 
                                                      L_UINT32 uID, 
                                                      L_TCHAR* pszValue);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertySystemTime(HWIASESSION hSession, 
                                                          L_VOID * pItem, 
                                                          L_TCHAR* pszID, 
                                                          L_UINT32 uID, 
                                                          LPSYSTEMTIME pValue);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertySystemTime(HWIASESSION hSession, 
                                                          L_VOID * pItem, 
                                                          L_TCHAR* pszID, 
                                                          L_UINT32 uID, 
                                                          LPSYSTEMTIME pValue);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaGetPropertyBuffer(HWIASESSION hSession, 
                                                      L_VOID * pItem, 
                                                      L_TCHAR* pszID, 
                                                      L_UINT32 uID, 
                                                      L_UCHAR* pValue, 
                                                      L_SIZE_T* puSize);
L_LTWIA_API L_INT EXT_FUNCTION L_WiaSetPropertyBuffer(HWIASESSION hSession, 
                                                      L_VOID * pItem, 
                                                      L_TCHAR* pszID, 
                                                      L_UINT32 uID, 
                                                      L_UCHAR* pValue, 
                                                      L_SIZE_T uSize);

L_LTWIA_API L_INT EXT_FUNCTION L_WiaEnumCapabilities(HWIASESSION hSession, 
                                                     L_VOID * pItem,
                                                     L_UINT uFlags, // For future use.
                                                     LWIAENUMCAPABILITIESCALLBACK pfnCallBack, 
                                                     L_VOID * pUserData);

#else
#define L_WiaGetPropertyLong L_WiaGetPropertyLongA
#define L_WiaSetPropertyLong L_WiaSetPropertyLongA
#define L_WiaGetPropertyGUID L_WiaGetPropertyGUIDA
#define L_WiaSetPropertyGUID L_WiaSetPropertyGUIDA
#define L_WiaGetPropertyString L_WiaGetPropertyStringA
#define L_WiaSetPropertyString L_WiaSetPropertyStringA
#define L_WiaGetPropertySystemTime L_WiaGetPropertySystemTimeA
#define L_WiaSetPropertySystemTime L_WiaSetPropertySystemTimeA
#define L_WiaGetPropertyBuffer L_WiaGetPropertyBufferA
#define L_WiaSetPropertyBuffer L_WiaSetPropertyBufferA
#define L_WiaEnumCapabilities L_WiaEnumCapabilitiesA
#endif // #if defined(FOR_UNICODE)


L_LTWIA_API L_INT EXT_FUNCTION L_WiaEnumFormats(HWIASESSION hSession, 
                                                L_VOID * pItem,
                                                L_UINT uFlags, // For future use.
                                                LWIAENUMFORMATSCALLBACK pfnCallBack, 
                                                L_VOID * pUserData);

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTWIA_H)
