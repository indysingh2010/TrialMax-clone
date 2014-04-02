/*************************************************************
   Ltdlgimg.h - Common Dialogs module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDLG_IMG_H)
#define LTDLG_IMG_H

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
// Flags For L_DlgRotate ( ... ) 
#define DLG_ROTATE_AUTOPROCESS          0x00000001
#define DLG_ROTATE_SHOW_CONTEXTHELP     0x00000002
#define DLG_ROTATE_SHOW_PREVIEW         0x00000004
#define DLG_ROTATE_SHOW_BACKCOLOR       0x00000008
#define DLG_ROTATE_SHOW_RESIZE          0x00000010
#define DLG_ROTATE_SHOW_TOOL_ONSCREEN   0x00000020
#define DLG_ROTATE_SHOW_TOOL_SHOWEFFECT 0x00000040
#define DLG_ROTATE_SHOW_TOOL_RESET      0x00000080

// Flags For L_DlgShear ( ... ) 
#define DLG_SHEAR_AUTOPROCESS          0x00000001
#define DLG_SHEAR_SHOW_CONTEXTHELP     0x00000002
#define DLG_SHEAR_SHOW_PREVIEW         0x00000004
#define DLG_SHEAR_SHOW_BACKCOLOR       0x00000008
#define DLG_SHEAR_SHOW_HORIZONTAL      0x00000010
#define DLG_SHEAR_SHOW_TOOL_ONSCREEN   0x00000020
#define DLG_SHEAR_SHOW_TOOL_SHOWEFFECT 0x00000040
#define DLG_SHEAR_SHOW_TOOL_RESET      0x00000080

// Flags For L_DlgResize ( ... ) 
#define DLG_RESIZE_AUTOPROCESS            0x00000001
#define DLG_RESIZE_SHOW_CONTEXTHELP       0x00000002
#define DLG_RESIZE_SHOW_PERCENTAGE        0x00000004
#define DLG_RESIZE_SHOW_MAINTAINASPECT    0x00000008
#define DLG_RESIZE_SHOW_RESOLUTIONGRP     0x00000010
#define DLG_RESIZE_SHOW_IDENTICALVALUE    0x00000020
#define DLG_RESIZE_MAINTAINASPECT         0x00000040

// Flags For L_DlgAddBorder ( ... ) 
#define DLG_ADDBORDER_AUTOPROCESS          0x00000001
#define DLG_ADDBORDER_SHOW_CONTEXTHELP     0x00000002
#define DLG_ADDBORDER_SHOW_PREVIEW         0x00000004
#define DLG_ADDBORDER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_ADDBORDER_SHOW_TOOL_SHOWEFFECT 0x00000010
#define DLG_ADDBORDER_SHOW_TOOL_ONSCREEN   0x00000020
#define DLG_ADDBORDER_SHOW_TOOL_RESET      0x00000040 
#define DLG_ADDBORDER_SHOW_APPLY           0x00000080 

// Flags For L_DlgAddFrame ( ... ) 
#define DLG_ADDFRAME_AUTOPROCESS           0x00000001
#define DLG_ADDFRAME_SHOW_CONTEXTHELP      0x00000002
#define DLG_ADDFRAME_SHOW_PREVIEW          0x00000004
#define DLG_ADDFRAME_SHOW_TOOL_ZOOMLEVEL   0x00000008
#define DLG_ADDFRAME_SHOW_TOOL_COLORPICKER 0x00000010
#define DLG_ADDFRAME_SHOW_TOOL_SHOWEFFECT  0x00000020
#define DLG_ADDFRAME_SHOW_TOOL_ONSCREEN    0x00000040 
#define DLG_ADDFRAME_SHOW_TOOL_RESET       0x00000080
#define DLG_ADDFRAME_SHOW_APPLY            0x00000100 

// Flags For L_DlgAutoTrim ( ... ) 
#define DLG_AUTOTRIM_AUTOPROCESS            0x00000001  
#define DLG_AUTOTRIM_SHOW_CONTEXTHELP       0x00000002
#define DLG_AUTOTRIM_SHOW_PREVIEW           0x00000004  
#define DLG_AUTOTRIM_SHOW_TOOL_SHOWEFFECT   0x00000010 
#define DLG_AUTOTRIM_SHOW_TOOL_ONSCREEN     0x00000020
#define DLG_AUTOTRIM_SHOW_TOOL_RESET        0x00000040
#define DLG_AUTOTRIM_SHOW_APPLY             0x00000080

// flags for L_DlgCanvasResize (...) 
#define DLG_CANVASRESIZE_AUTOPROCESS         0x00000001  
#define DLG_CANVASRESIZE_SHOW_CONTEXTHELP    0x00000002  
#define DLG_CANVASRESIZE_SHOW_CURRENT_HEIGHT 0x00000004  
#define DLG_CANVASRESIZE_SHOW_CURRENT_WIDTH  0x00000008  
#define DLG_CANVASRESIZE_SHOW_HORIZPOS       0x00000010  
#define DLG_CANVASRESIZE_SHOW_VERTZPOS       0x00000020  
#define DLG_CANVASRESIZE_SHOW_BACKCOLOR      0x00000040  
#define DLG_CANVASRESIZE_SHOW_KEEPASPECT     0x00000080  

// Flags For L_DlgHistogram ( ... ) 
#define DLG_HISTOGRAM_SHOW_CONTEXTHELP 0x00000001
#define DLG_HISTOGRAM_SHOW_VIEWSTYLE   0x00000002
#define DLG_HISTOGRAM_USERPENCOLORS    0x00000004

typedef struct _ROTATEDLGPARAMS
{
   L_UINT           uStructSize ;         
   pBITMAPHANDLE    pBitmap ;       
   L_INT            nAngle ;        
   L_BOOL           bResize ;       
   L_UINT           uRotateFlags ;  
   COLORREF         crBack ;        
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} ROTATEDLGPARAMS, * LPROTATEDLGPARAMS ;

typedef struct _SHEARDLGPARAMS
{
   L_UINT           uStructSize ;         
   pBITMAPHANDLE    pBitmap ;       
   L_INT            nAngle ;        
   L_BOOL           bHorizontal ;   
   COLORREF         crBack ;        
   L_UINT32         uDlgFlags ;     
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} SHEARDLGPARAMS, * LPSHEARDLGPARAMS ;

typedef struct _RESIZEDLGPARAMS
{
   L_UINT          uStructSize ;     
   pBITMAPHANDLE   pBitmap ;         
   L_UINT          uMaxNewWidth ;    
   L_UINT          uMaxNewHeight ;   
   L_UINT          uMaxNewResolutionX ; 
   L_UINT          uMaxNewResolutionY ; 
   L_UINT          uNewWidth ;       
   L_UINT          uNewHeight ;      
   L_UINT          uNewResolutionX ; 
   L_UINT          uNewResolutionY ; 
   L_UINT          uOriginalWidth ;        
   L_UINT          uOriginalHeight ;       
   L_UINT          uOriginalResolutionX ;  
   L_UINT          uOriginalResolutionY ;  
   L_UINT          uOriginalBitsPerPixel ; 
   L_UINT          uResize ;         
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} RESIZEDLGPARAMS, * LPRESIZEDLGPARAMS ;

typedef struct _ADDBORDERDLGPARAMSA 
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ; 
   L_INT           nTileBitmapIndex ;
   LPDLGBITMAPLISTA pBitmapList ;
   L_INT           nLeftThickness;
   L_INT           nTopThickness;
   L_INT           nRightThickness;
   L_INT           nBottomThickness;
   L_INT           nLocation;
   L_INT           nStyle;
   L_INT           nEffectStyle;
   L_INT           nGradientStyle;
   COLORREF        crOpaque;
   COLORREF        crGradientStart;
   COLORREF        crGradientEnd;
   L_INT           nCurveIntensity;
   L_BOOL          bSoftCurve;
   L_BOOL          bShadow;
   L_INT           nShadowSize;
   L_INT           nShadowDirection;
   L_BOOL          bBumpyShadow;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} ADDBORDERDLGPARAMSA, * LPADDBORDERDLGPARAMSA ;

typedef struct _ADDFRAMEDLGPARAMSA 
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ; 
   L_BOOL          bUseMask ; 
   L_BOOL          bKeepFrameState ;
   COLORREF        crMask ;
   BYTE            SmoothEdge ;
   L_INT           nLocation ;
   L_INT           nQuality ;
   LPDLGBITMAPLISTA pBitmapList ;
   L_INT           nFrameBitmapIndex ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} ADDFRAMEDLGPARAMSA, * LPADDFRAMEDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _ADDBORDERDLGPARAMS 
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ; 
   L_INT           nTileBitmapIndex ;
   LPDLGBITMAPLIST pBitmapList ;
   L_INT           nLeftThickness;
   L_INT           nTopThickness;
   L_INT           nRightThickness;
   L_INT           nBottomThickness;
   L_INT           nLocation;
   L_INT           nStyle;
   L_INT           nEffectStyle;
   L_INT           nGradientStyle;
   COLORREF        crOpaque;
   COLORREF        crGradientStart;
   COLORREF        crGradientEnd;
   L_INT           nCurveIntensity;
   L_BOOL          bSoftCurve;
   L_BOOL          bShadow;
   L_INT           nShadowSize;
   L_INT           nShadowDirection;
   L_BOOL          bBumpyShadow;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} ADDBORDERDLGPARAMS, * LPADDBORDERDLGPARAMS ;

typedef struct _ADDFRAMEDLGPARAMS 
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ; 
   L_BOOL          bUseMask ; 
   L_BOOL          bKeepFrameState ;
   COLORREF        crMask ;
   BYTE            SmoothEdge ;
   L_INT           nLocation ;
   L_INT           nQuality ;
   LPDLGBITMAPLIST pBitmapList ;
   L_INT           nFrameBitmapIndex ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} ADDFRAMEDLGPARAMS, * LPADDFRAMEDLGPARAMS ;
#else
typedef ADDFRAMEDLGPARAMSA ADDFRAMEDLGPARAMS;
typedef LPADDFRAMEDLGPARAMSA LPADDFRAMEDLGPARAMS;
typedef ADDBORDERDLGPARAMSA ADDBORDERDLGPARAMS;
typedef LPADDBORDERDLGPARAMSA LPADDBORDERDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

typedef struct _AUTOTRIMDLGPARAMS 
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_INT           nThreshold ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *          pHelpCallBackUserData ;

} AUTOTRIMDLGPARAMS, * LPAUTOTRIMDLGPARAMS ; 

typedef struct _CANVASRESIZEDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   
   L_INT            nCurrentWidth ;
   L_INT            nCurrentHeight ;   
   
   POINT            ptTopLeft ;
   L_INT            nNewWidth ;
   L_INT            nNewHeight ;
   COLORREF         crBkgnd ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *          pHelpCallBackUserData ;

} CANVASRESIZEDLGPARAMS, * LPCANVASRESIZEDLGPARAMS ;

typedef struct _HISTOGRAMDLGPARAMS
{
   L_UINT           uStructSize ;              
   pBITMAPHANDLE    pBitmap ;
   L_UINT32 *       puMasterHistogram;      
   L_UINT           uMasterHistogramLen;  
   L_UINT32 *       puRedHistogram;         
   L_UINT           uRedHistogramLen;     
   L_UINT32 *       puGreenHistogram;    
   L_UINT           uGreenHistogramLen;   
   L_UINT32 *       puBlueHistogram;     
   L_UINT           uBlueHistogramLen;    
   COLORREF         crMasterPen ;
   COLORREF         crRedChannelPen ;
   COLORREF         crGreenChannelPen ;
   COLORREF         crBlueChannelPen ;
   L_UINT           uHistogramFlags ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} HISTOGRAMDLGPARAMS, * LPHISTOGRAMDLGPARAMS ;

//.............................................................................
// enums, defines and structures
//.............................................................................


//.............................................................................
// Functions
//.............................................................................

L_LTDLG_API L_INT EXT_FUNCTION L_DlgRotate ( HWND hWndOwner, 
                                 LPROTATEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgShear ( HWND hWndOwner, 
                                LPSHEARDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgResize ( HWND hWndOwner, 
                                 LPRESIZEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgAddBorderA ( HWND hWndOwner, 
                                    LPADDBORDERDLGPARAMSA pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgAddFrameA ( HWND hWndOwner, 
                                   LPADDFRAMEDLGPARAMSA pDlgParams ) ;

#if defined(FOR_UNICODE)
L_LTDLG_API L_INT EXT_FUNCTION L_DlgAddBorder ( HWND hWndOwner, 
                                    LPADDBORDERDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgAddFrame ( HWND hWndOwner, 
                                   LPADDFRAMEDLGPARAMS pDlgParams ) ;
#else
#define L_DlgAddFrame L_DlgAddFrameA
#define L_DlgAddBorder L_DlgAddBorderA
#endif // #if defined(FOR_UNICODE)

L_LTDLG_API L_INT EXT_FUNCTION L_DlgAutoTrim ( HWND hWndOwner, 
                                   LPAUTOTRIMDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgCanvasResize ( HWND hWndOwner,
                                       LPCANVASRESIZEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgHistogram ( HWND hWndOwner,
                                    LPHISTOGRAMDLGPARAMS pDlgParams ) ;

//.............................................................................
// Functions
//.............................................................................

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDLG_IMG_H)
