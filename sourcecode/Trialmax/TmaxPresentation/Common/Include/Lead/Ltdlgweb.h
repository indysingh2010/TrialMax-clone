/*************************************************************
   Ltdlgweb.h - Common Dialogs module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDLG_WEB_H)
#define LTDLG_WEB_H

#if !defined(L_LTDLG_API)
   #define L_LTDLG_API
#endif // #if !defined(L_LTDLG_API)

#include "Ltkrn.h"
#include "Ltimg.h"
#include "Ltfil.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

//.............................................................................
// enums, defines and structures
//.............................................................................
// Flags for L_DlgPNGWebTuner ( ... )
#define DLG_PNGWEBTUNER_SHOW_CONTEXTHELP        0x00000001  
#define DLG_PNGWEBTUNER_SHOW_PREVIEW            0x00000002  
#define DLG_PNGWEBTUNER_SHOW_TOOL_ZOOMLEVEL     0x00000004  
#define DLG_PNGWEBTUNER_SHOW_TOOL_COLORPICKER   0x00000008  
#define DLG_PNGWEBTUNER_SHOW_INFORMATION        0x00000010  
#define DLG_PNGWEBTUNER_SHOW_ADDWINDOWCOLOR     0x00000020  
#define DLG_PNGWEBTUNER_SHOW_TRANSPARENCY       0x00000040  
#define DLG_PNGWEBTUNER_SHOW_EXPORT             0x00000080  
#define DLG_PNGWEBTUNER_SHOW_APPLY              0x00000100  

// Flags for L_DlgGIFWebTuner ( ... )
#define DLG_GIFWEBTUNER_SHOW_CONTEXTHELP        0x00000001  
#define DLG_GIFWEBTUNER_SHOW_PREVIEW            0x00000002  
#define DLG_GIFWEBTUNER_SHOW_TOOL_ZOOMLEVEL     0x00000004  
#define DLG_GIFWEBTUNER_SHOW_TOOL_COLORPICKER   0x00000008  
#define DLG_GIFWEBTUNER_SHOW_ADDWINDOWCOLOR     0x00000020  
#define DLG_GIFWEBTUNER_SHOW_TRANSPARENCY       0x00000040  
#define DLG_GIFWEBTUNER_SHOW_INFORMATION        0x00000080  
#define DLG_GIFWEBTUNER_SHOW_OPTIONS            0x00000100  
#define DLG_GIFWEBTUNER_SHOW_EXPORT             0x00000200  

// Flags for L_DlgJPEGWebTuner ( ... )
#define DLG_JPEGWEBTUNER_SHOW_CONTEXTHELP        0x00000001  
#define DLG_JPEGWEBTUNER_SHOW_PREVIEW            0x00000002  
#define DLG_JPEGWEBTUNER_SHOW_TOOL_ZOOMLEVEL     0x00000004  
#define DLG_JPEGWEBTUNER_SHOW_INFORMATION        0x00000008  
#define DLG_JPEGWEBTUNER_SHOW_SAVETHUMBNAIL      0x00000010  
#define DLG_JPEGWEBTUNER_SHOW_OPTIONS            0x00000100  
#define DLG_JPEGWEBTUNER_SHOW_EXPORT             0x00000200  

// Flags for L_DlgHTMLMapper ( ... )
#define DLG_HTMLMAPPER_SHOW_CONTEXTHELP        0x00000001  


typedef struct _PNGWEBTUNERDLGPARAMS
{
   L_UINT           uStructSize ; 
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   COLORREF         crTransparent ;
   L_INT            nPalType ;
   L_BOOL           bAddWindowsColors ;
   L_INT            nDitherType ;
   L_INT            nBitsPerPixel ;
   L_INT            nNumOfColors ;
   L_INT            nTransparencyTolerance ;
   L_BOOL           bTransparent ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;     

} PNGWEBTUNERDLGPARAMS, * LPPNGWEBTUNERDLGPARAMS ;

typedef struct _GIFWEBTUNERDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   COLORREF         crTransparent ;
   L_INT            nPalType ;
   L_BOOL           bAddWindowsColors ;
   L_INT            nDitherType ;
   L_INT            nBitsPerPixel ;
   L_INT            nNumOfColors ;
   L_INT            nTransparencyTolerance ;
   L_BOOL           bTransparent ;
   L_BOOL           bInterlaced ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} GIFWEBTUNERDLGPARAMS, * LPGIFWEBTUNERDLGPARAMS ;

typedef struct _JPEGWEBTUNERDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nQuality ;
   L_INT            nFormat ;
   L_BOOL           bProgressive ;
   L_BOOL           bWithStamp ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} JPEGWEBTUNERDLGPARAMS, * LPJPEGWEBTUNERDLGPARAMS ;

typedef struct _HTMLMAPPERDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;  
   L_BOOL           bZoomToFit ;  
   LPDLGHISTORYLIST pURLEntries ;
   LPDLGHISTORYLIST pALTEntries ;
   LPDLGHISTORYLIST pTargetEntries  ;
   LPDLGHISTORYLIST pRolloverEntries  ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} HTMLMAPPERDLGPARAMS, * LPHTMLMAPPERDLGPARAMS ;

//.............................................................................
// enums, defines and structures
//.............................................................................


//.............................................................................
// Functions
//.............................................................................

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPNGWebTuner ( HWND hWndOwner, 
                                      LPPNGWEBTUNERDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGIFWebTuner ( HWND hWndOwner, 
                                      LPGIFWEBTUNERDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgJPEGWebTuner ( HWND hWndOwner, 
                                       LPJPEGWEBTUNERDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgHTMLMapper ( HWND hWndOwner, 
                                     LPHTMLMAPPERDLGPARAMS pDlgParams ) ;

//.............................................................................
// Functions
//.............................................................................

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDLG_WEB_H)
