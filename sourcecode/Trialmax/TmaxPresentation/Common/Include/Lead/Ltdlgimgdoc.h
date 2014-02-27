/*************************************************************
   Ltdlgimgdoc.h - Common Dialogs module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDLG_IMGDOC_H)
#define LTDLG_IMGDOC_H

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
// Flags for L_DlgSmooth ( ... )
#define DLG_SMOOTH_AUTOPROCESS          0x00000001
#define DLG_SMOOTH_SHOW_CONTEXTHELP     0x00000002
#define DLG_SMOOTH_SHOW_PREVIEW         0x00000004
#define DLG_SMOOTH_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_SMOOTH_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_SMOOTH_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_SMOOTH_SHOW_TOOL_RESET      0x00000040

// Flags for L_DlgLineRemove ( ... )
#define DLG_LINEREMOVE_AUTOPROCESS          0x00000001
#define DLG_LINEREMOVE_SHOW_CONTEXTHELP     0x00000002
#define DLG_LINEREMOVE_SHOW_PREVIEW         0x00000004
#define DLG_LINEREMOVE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_LINEREMOVE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_LINEREMOVE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_LINEREMOVE_SHOW_TOOL_RESET      0x00000040

// Flags for L_DlgBorderRemove ( ... )
#define DLG_BORDERREMOVE_AUTOPROCESS          0x00000001
#define DLG_BORDERREMOVE_SHOW_CONTEXTHELP     0x00000002
#define DLG_BORDERREMOVE_SHOW_PREVIEW         0x00000004
#define DLG_BORDERREMOVE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_BORDERREMOVE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_BORDERREMOVE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_BORDERREMOVE_SHOW_TOOL_RESET      0x00000040

// Flags for L_DlgInvertedText ( ... )
#define DLG_INVERTEDTEXT_AUTOPROCESS          0x00000001
#define DLG_INVERTEDTEXT_SHOW_CONTEXTHELP     0x00000002
#define DLG_INVERTEDTEXT_SHOW_PREVIEW         0x00000004
#define DLG_INVERTEDTEXT_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_INVERTEDTEXT_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_INVERTEDTEXT_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_INVERTEDTEXT_SHOW_TOOL_RESET      0x00000040

// Flags for L_DlgDotRemove ( ... )
#define DLG_DOTREMOVE_AUTOPROCESS          0x00000001
#define DLG_DOTREMOVE_SHOW_CONTEXTHELP     0x00000002
#define DLG_DOTREMOVE_SHOW_PREVIEW         0x00000004
#define DLG_DOTREMOVE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_DOTREMOVE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_DOTREMOVE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_DOTREMOVE_SHOW_TOOL_RESET      0x00000040

// Flags for L_DlgHolePunchRemove ( ... )
#define DLG_HOLEPUNCHREMOVE_AUTOPROCESS          0x00000001
#define DLG_HOLEPUNCHREMOVE_SHOW_CONTEXTHELP     0x00000002
#define DLG_HOLEPUNCHREMOVE_SHOW_PREVIEW         0x00000004
#define DLG_HOLEPUNCHREMOVE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_HOLEPUNCHREMOVE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_HOLEPUNCHREMOVE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_HOLEPUNCHREMOVE_SHOW_TOOL_RESET      0x00000040

typedef struct _SMOOTHDLGPARAMS
{
   L_UINT           uStructSize ;      
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   SMOOTH           Smooth ;
   COLORREF         crWhiteArea ;
   COLORREF         crBlackArea ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} SMOOTHDLGPARAMS, * LPSMOOTHDLGPARAMS ;

typedef struct _LINEREMOVEDLGPARAMS
{
   L_UINT           uStructSize ;      
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   LINEREMOVE       LineRemove ;
   COLORREF         crModification ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} LINEREMOVEDLGPARAMS, * LPLINEREMOVEDLGPARAMS ;

typedef struct _BORDERREMOVEDLGPARAMS
{
   L_UINT           uStructSize ;      
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   BORDERREMOVE     BorderRemove ;
   COLORREF         crModification ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} BORDERREMOVEDLGPARAMS, * LPBORDERREMOVEDLGPARAMS ;

typedef struct _INVERTEDTEXTDLGPARAMS
{
   L_UINT           uStructSize ;      
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   INVERTEDTEXT     InvertedText ;
   COLORREF         crWhiteArea ;
   COLORREF         crBlackArea ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} INVERTEDTEXTDLGPARAMS, * LPINVERTEDTEXTDLGPARAMS ;

typedef struct _DOTREMOVEDLGPARAMS
{
   L_UINT          uStructSize ;      
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   DOTREMOVE       DotRemove ;
   COLORREF        crWhiteArea ;
   COLORREF        crBlackArea ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;
   
} DOTREMOVEDLGPARAMS, * LPDOTREMOVEDLGPARAMS ;

typedef struct _HOLEPUNCHREMOVEDLGPARAMS
{
   L_UINT          uStructSize ;      
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ; 
   HOLEPUNCH       HolePunchRemove ;
   COLORREF        crWhiteArea ;
   COLORREF        crBlackArea ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;
   
} HOLEPUNCHREMOVEDLGPARAMS, * LPHOLEPUNCHREMOVEDLGPARAMS ;

//.............................................................................
// enums, defines and structures
//.............................................................................


//.............................................................................
// Functions
//.............................................................................

L_LTDLG_API L_INT EXT_FUNCTION L_DlgSmooth ( HWND hWndOwner,
                                LPSMOOTHDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgLineRemove ( HWND hWndOwner,
                                    LPLINEREMOVEDLGPARAMS pDlgParams ) ;
                                       
L_LTDLG_API L_INT EXT_FUNCTION L_DlgBorderRemove ( HWND hWndOwner,
                                      LPBORDERREMOVEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgInvertedText ( HWND hWndOwner,
                                      LPINVERTEDTEXTDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgDotRemove ( HWND hWndOwner,
                                   LPDOTREMOVEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgHolePunchRemove ( HWND hWndOwner,
                                         LPHOLEPUNCHREMOVEDLGPARAMS pDlgParams ) ;

//.............................................................................
// Functions
//.............................................................................

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDLG_IMGDOC_H)
