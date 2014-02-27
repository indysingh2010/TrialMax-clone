/*************************************************************
   Ltdlgefx.h - Common Dialogs module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDLG_EFX_H)
#define LTDLG_EFX_H

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
// flags for L_DlgGetShape() 
#define DLG_SHAPE_SHOW_CONTEXTHELP     0x00000001   
#define DLG_SHAPE_AUTOPREVIEW          0x00000002   
#define DLG_SHAPE_SHOW_PREVIEW         0x00000004   
#define DLG_SHAPE_BACKSTYLE            0x00000008   
#define DLG_SHAPE_FILLSTYLE            0x00000010   
#define DLG_SHAPE_FORE_BACK_COLOR      0x00000020   
#define DLG_SHAPE_BORDERSTYLE          0x00000040   
#define DLG_SHAPE_BORDERWIDTH          0x00000080   
#define DLG_SHAPE_BORDERCOLOR          0x00000100   
#define DLG_SHAPE_INNERSTYLE           0x00000200   
#define DLG_SHAPE_INNERWIDTH           0x00000400   
#define DLG_SHAPE_INNER_HILITE_SHADOW  0x00000800   
#define DLG_SHAPE_OUTERSTYLE           0x00001000   
#define DLG_SHAPE_OUTERWIDTH           0x00002000   
#define DLG_SHAPE_OUTER_HILITE_SHADOW  0x00004000   
#define DLG_SHAPE_SHADOWCOLOR          0x00008000   
#define DLG_SHAPE_SHADOW_X_Y           0x00010000   
#define DLG_SHAPE_BROWSEIMAGE          0x00020000   
#define DLG_SHAPE_NO_TREEVIEW          0x00040000   
#define DLG_SHAPE_CLASS_SQUARE         0x00080000   
#define DLG_SHAPE_CLASS_RECTANGLE      0x00100000   
#define DLG_SHAPE_CLASS_PARALLELOGRAM  0x00200000   
#define DLG_SHAPE_CLASS_TRAPEZOID      0x00400000   
#define DLG_SHAPE_CLASS_TRIANGLE       0x00800000   
#define DLG_SHAPE_CLASS_OTHER          0x01000000   
#define DLG_SHAPE_CLASS_CIRCLE         0x02000000   
#define DLG_SHAPE_CLASS_ELLIPSE        0x04000000   
#define DLG_SHAPE_CLASS_STAR           0x08000000   
#define DLG_SHAPE_CLASS_CROSS          0x10000000   
#define DLG_SHAPE_CLASS_ARROW          0x20000000   

// flags for L_DlgGetEffect() 
#define DLG_EFFECT_SHOW_CONTEXTHELP   0x00000001    
#define DLG_EFFECT_SHOW_PREVIEW       0x00000002    
#define DLG_EFFECT_AUTOPREVIEW        0x00000004    
#define DLG_EFFECT_DELAY              0x00000008    
#define DLG_EFFECT_GRAIN              0x00000010    
#define DLG_EFFECT_PASSES             0x00000020    
#define DLG_EFFECT_TRANSPARENT        0x00000040    
#define DLG_EFFECT_WAND               0x00000080    
#define DLG_EFFECT_NO_TREEVIEW        0x00000100    
#define DLG_EFFECT_CLASS_NONE         0x00000200    
#define DLG_EFFECT_CLASS_WIPE         0x00000400    
#define DLG_EFFECT_CLASS_WIPERECT     0x00000800    
#define DLG_EFFECT_CLASS_WIPECIRCLE   0x00001000    
#define DLG_EFFECT_CLASS_PUSH         0x00002000    
#define DLG_EFFECT_CLASS_SLIDE        0x00004000    
#define DLG_EFFECT_CLASS_ROLL         0x00008000    
#define DLG_EFFECT_CLASS_ROTATE       0x00010000    
#define DLG_EFFECT_CLASS_ZOOM         0x00020000    
#define DLG_EFFECT_CLASS_DRIP         0x00040000    
#define DLG_EFFECT_CLASS_BLIND        0x00080000    
#define DLG_EFFECT_CLASS_RANDOM       0x00100000    
#define DLG_EFFECT_CLASS_CHECK        0x00200000    
#define DLG_EFFECT_CLASS_BLOCKS       0x00400000    
#define DLG_EFFECT_CLASS_CIRCLE       0x00800000    
#define DLG_EFFECT_CLASS_ELLIPSE      0x01000000    

// flags for L_DlgGetTransition() 
#define DLG_TRANSITION_SHOW_CONTEXTHELP   0x00000001
#define DLG_TRANSITION_SHOW_PREVIEW       0x00000002
#define DLG_TRANSITION_AUTOPREVIEW        0x00000004
#define DLG_TRANSITION_FORECOLOR          0x00000008
#define DLG_TRANSITION_BACKCOLOR          0x00000010
#define DLG_TRANSITION_DELAY              0x00000020
#define DLG_TRANSITION_GRAIN              0x00000040
#define DLG_TRANSITION_EFFECT             0x00000080
#define DLG_TRANSITION_PASSES             0x00000100
#define DLG_TRANSITION_WAND               0x00000200
#define DLG_TRANSITION_TRANSPARENT        0x00000400
#define DLG_TRANSITION_GRADIENT           0x00000800

// Flags for L_DlgGetGradient (...)
#define DLG_GRADIENT_SHOW_CONTEXTHELP      0x00000001   
#define DLG_GRADIENT_SHOW_PREVIEW          0x00000002   
#define DLG_GRADIENT_AUTOPREVIEW           0x00000004   
#define DLG_GRADIENT_STARTCOLOR            0x00000008   
#define DLG_GRADIENT_ENDCOLOR              0x00000010   
#define DLG_GRADIENT_STEPS                 0x00000020   
#define DLG_GRADIENT_NO_TREEVIEW           0x00000040   
#define DLG_GRADIENT_CLASS_LINEAR          0x00000080   
#define DLG_GRADIENT_CLASS_ANGULAR         0x00000100   
#define DLG_GRADIENT_CLASS_RECTANGULAR     0x00000200   
#define DLG_GRADIENT_CLASS_ELLIPTICAL      0x00000400   
#define DLG_GRADIENT_CLASS_CONICAL         0x00000800   

// flags for L_DlgGetText (...) 
#define DLG_TEXT_SHOW_CONTEXTHELP      0x00000001   
#define DLG_TEXT_SHOW_PREVIEW          0x00000002   
#define DLG_TEXT_AUTOPREVIEW           0x00000004   
#define DLG_TEXT_SAMPLETEXT            0x00000008   
#define DLG_TEXT_STYLE                 0x00000010   
#define DLG_TEXT_COLOR                 0x00000020   
#define DLG_TEXT_BORDERCOLOR           0x00000040   
#define DLG_TEXT_ALIGN                 0x00000080   
#define DLG_TEXT_ANGLE                 0x00000100   
#define DLG_TEXT_WORDWRAP              0x00000200   
#define DLG_TEXT_FONT                  0x00000400   
#define DLG_TEXT_FOREIMAGE             0x00000800   
#define DLG_TEXT_BROWSEIMAGE           0x00001000   
#define DLG_TEXT_SHADOWCOLOR           0x00002000   
#define DLG_TEXT_SHADOW_X_Y            0x00004000   

typedef struct _SHAPEDLGPARAMS
{
   L_UINT           uStructSize  ;        
   pBITMAPHANDLE    pBitmap ;
   pBITMAPHANDLE    pBackgroundBitmap ; 
   L_UINT           uShape ;        
   COLORREF         crBack ;        
   L_UINT           uBackStyle ;    
   COLORREF         crFill ;        
   L_UINT           uFillStyle ;    
   COLORREF         crBorder ;      
   L_UINT           uBorderStyle ;  
   L_UINT           uBorderWidth ;  
   COLORREF         crInnerHilite ; 
   COLORREF         crInnerShadow ; 
   L_UINT           uInnerStyle ;   
   L_UINT           uInnerWidth ;   
   COLORREF         crOuterHilite ; 
   COLORREF         crOuterShadow ; 
   L_UINT           uOuterStyle ;   
   L_UINT           uOuterWidth ;   
   L_INT            nShadowX ;      
   L_INT            nShadowY ;      
   COLORREF         crShadow ;      
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} SHAPEDLGPARAMS, * LPSHAPEDLGPARAMS ;

typedef struct _EFFECTDLGPARAMS
{
   L_UINT           uStructSize  ;          
   pBITMAPHANDLE    pBitmap ;
   L_UINT           uEffect ;         
   L_UINT           uGrain ;          
   L_UINT           uDelay ;          
   L_UINT           uMaxPass ;        
   L_BOOL           bTransparent ;    
   COLORREF         crTransparent ;   
   L_UINT           uWandWidth ;      
   COLORREF         crWand ;          
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} EFFECTDLGPARAMS, * LPEFFECTDLGPARAMS ;


typedef struct _TRANSITIONDLGPARAMS
{
   L_UINT           uStructSize  ;        
   pBITMAPHANDLE    pBitmap ;
   L_UINT           uTransition ;   
   COLORREF         crBack ;        
   COLORREF         crFore ;        
   L_UINT           uSteps ;        
   L_UINT           uEffect ;       
   L_UINT           uGrain ;        
   L_UINT           uDelay ;        
   L_UINT           uMaxPass ;      
   L_BOOL           bTransparent ;  
   COLORREF         crTransparent ; 
   L_UINT           uWandWidth ;    
   COLORREF         crWand ;        
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} TRANSITIONDLGPARAMS, * LPTRANSITIONDLGPARAMS ;

typedef struct _GRADIENTDLGPARAMS
{
   L_UINT           uStructSize  ;  
   pBITMAPHANDLE    pBitmap ;   
   L_UINT           uStyle ;  
   COLORREF         crStart ; 
   COLORREF         crEnd ;   
   L_UINT           uSteps ;  
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} GRADIENTDLGPARAMS, * LPGRADIENTDLGPARAMS ;

typedef struct _TEXTDLGPARAMSA
{
   L_UINT           uStructSize  ;        
   pBITMAPHANDLE    pBitmap ;
   pBITMAPHANDLE    pForegroundBitmap ;
   LPSTR            pszSampleText ; 
   L_INT            nMaxCount ;     
   L_INT            nAngle ;        
   L_UINT           uStyle ;        
   L_UINT           uAlign ;        
   L_BOOL           bWordWrap ;     
   HFONT            hFont ;         
   COLORREF         crText ;        
   COLORREF         crHilite ;      
   COLORREF         crShadow ;      
   L_INT            nXDepth ;       
   L_INT            nYDepth ;       
   L_BOOL           bUseForeImage ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} TEXTDLGPARAMSA, * LPTEXTDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _TEXTDLGPARAMS
{
   L_UINT           uStructSize  ;        
   pBITMAPHANDLE    pBitmap ;
   pBITMAPHANDLE    pForegroundBitmap ;
   LPTSTR           pszSampleText ; 
   L_INT            nMaxCount ;     
   L_INT            nAngle ;        
   L_UINT           uStyle ;        
   L_UINT           uAlign ;        
   L_BOOL           bWordWrap ;     
   HFONT            hFont ;         
   COLORREF         crText ;        
   COLORREF         crHilite ;      
   COLORREF         crShadow ;      
   L_INT            nXDepth ;       
   L_INT            nYDepth ;       
   L_BOOL           bUseForeImage ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} TEXTDLGPARAMS, * LPTEXTDLGPARAMS ;
#else
typedef TEXTDLGPARAMSA TEXTDLGPARAMS;
typedef LPTEXTDLGPARAMSA LPTEXTDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

//.............................................................................
// enums, defines and structures
//.............................................................................


//.............................................................................
// Functions
//.............................................................................

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetShape ( HWND hWndOwner,
                                   LPSHAPEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetEffect ( HWND hWndOwner,
                                    LPEFFECTDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetTransition ( HWND hWndOwner,
                                        LPTRANSITIONDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetGradient ( HWND hWndOwner,
                                      LPGRADIENTDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetTextA ( HWND hWndOwner, 
                                  LPTEXTDLGPARAMSA pDlgParams ) ;
#if defined(FOR_UNICODE)
L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetText ( HWND hWndOwner, 
                                  LPTEXTDLGPARAMS pDlgParams ) ;
#else
#define L_DlgGetText L_DlgGetTextA
#endif // #if defined(FOR_UNICODE)

//.............................................................................
// Functions
//.............................................................................

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDLG_EFX_H)
