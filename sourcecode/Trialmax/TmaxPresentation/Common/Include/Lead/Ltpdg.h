//==============================================================================
//
//  LTPDG : Header file.
//
//  Copyright (C) 1990, 1999 LEAD Technologies, Inc.
//  All rights reserved.
//
//==============================================================================
#if !defined LTPNTDLG_H_INCLUDED
#define LTPNTDLG_H_INCLUDED

#if !defined(L_LTPDG_API)
      #define L_LTPDG_API
#endif // #if !defined(L_LTPDG_API)

//============= INCLUDES =======================================================
#include "ltpnt.h"

#define L_HEADER_ENTRY 
#include "ltpck.h"

//============= CONSTANTS ======================================================
// Brush Flags
 
#define PAINT_DLG_BRUSH_SHOWALL                 0x00000001
#define PAINT_DLG_BRUSH_SHOWTOUCHCONTENT        0x00000002
#define PAINT_DLG_BRUSH_SHOWTOUCHCOLOR          0x00000004
#define PAINT_DLG_BRUSH_SHOWTOUCHIMAGE          0x00000008
#define PAINT_DLG_BRUSH_SHOWDIAMETER            0x00000010
#define PAINT_DLG_BRUSH_SHOWHARDNESS            0x00000020
#define PAINT_DLG_BRUSH_SHOWSPACING             0x00000040
#define PAINT_DLG_BRUSH_SHOWOPACITY             0x00000080
#define PAINT_DLG_BRUSH_SHOWDENSITY             0x00000100
#define PAINT_DLG_BRUSH_SHOWFADEOUTRATE         0x00000200
#define PAINT_DLG_BRUSH_SHOWTEXTURE             0x00000400
#define PAINT_DLG_BRUSH_SHOWDEFAULT             0x00000800
#define PAINT_DLG_BRUSH_INITUSEDEFAULT          0x00001000 
#define PAINT_DLG_BRUSH_SHOWTRANSPARENTCOLOR    0x00002000

// Shape Flags
#define PAINT_DLG_SHAPE_SHOWALL                 0x00000001
#define PAINT_DLG_SHAPE_SHOWBKGRNDSTYLE         0x00000002
#define PAINT_DLG_SHAPE_SHOWBKOPAQUECOLOR       0x00000004
#define PAINT_DLG_SHAPE_SHOWGRADIENTSTYLE       0x00000008
#define PAINT_DLG_SHAPE_SHOWGRADIENTDIRECTION   0x00000010
#define PAINT_DLG_SHAPE_SHOWGRADIENTSTARTCOLOR  0x00000020
#define PAINT_DLG_SHAPE_SHOWGRADIENTENDCOLOR    0x00000040
#define PAINT_DLG_SHAPE_SHOWGRADIENTPREVIEW     0x00000080
#define PAINT_DLG_SHAPE_SHOWGRADIENTSTEPS       0x00000100
#define PAINT_DLG_SHAPE_SHOWBKGRNDTILE          0x00000200
#define PAINT_DLG_SHAPE_SHOWOPACITY             0x00000400
#define PAINT_DLG_SHAPE_SHOWBORDERSTYLE         0x00000800
#define PAINT_DLG_SHAPE_SHOWBORDERWIDTH         0x00001000
#define PAINT_DLG_SHAPE_SHOWBORDERBRUSHSTYLE    0x00002000
#define PAINT_DLG_SHAPE_SHOWBORDERCOLOR         0x00004000
#define PAINT_DLG_SHAPE_SHOWBORDERPATTERN       0x00008000
#define PAINT_DLG_SHAPE_SHOWENDCAP              0x00010000
#define PAINT_DLG_SHAPE_SHOWELLIPSEWIDTH        0x00020000
#define PAINT_DLG_SHAPE_SHOWELLIPSEHEIGHT       0x00040000
#define PAINT_DLG_SHAPE_SHOWTEXTURE             0x00080000
#define PAINT_DLG_SHAPE_SHOWDEFAULT             0x00100000
#define PAINT_DLG_SHAPE_INITUSEDEFAULT          0x00200000

// Region Flags
#define PAINT_DLG_REGION_SHOWALL                0x00000001
#define PAINT_DLG_REGION_SHOWLOWERTOLERANCE     0x00000002
#define PAINT_DLG_REGION_SHOWUPPERTOLERANCE     0x00000004
#define PAINT_DLG_REGION_SHOWELLIPSEWIDTH       0x00000008
#define PAINT_DLG_REGION_SHOWELLIPSEHEIGHT      0x00000010
#define PAINT_DLG_REGION_SHOWDEFAULT            0x00000020
#define PAINT_DLG_REGION_INITUSEDEFAULT         0x00000040

// Fill Flags 
#define PAINT_DLG_FILL_SHOWALL                  0x00000001
#define PAINT_DLG_FILL_SHOWSTYLE                0x00000002
#define PAINT_DLG_FILL_SHOWSOLIDFILLCOLOR       0x00000004
#define PAINT_DLG_FILL_SHOWBKGRNDTILE           0x00000008
#define PAINT_DLG_FILL_SHOWGRADIENTSTYLE        0x00000010
#define PAINT_DLG_FILL_SHOWGRADIENTDIRECTION    0x00000020
#define PAINT_DLG_FILL_SHOWGRADIENTPREVIEW      0x00000040
#define PAINT_DLG_FILL_SHOWGRADIENTSTARTCOLOR   0x00000080
#define PAINT_DLG_FILL_SHOWGRADIENTENDCOLOR     0x00000100
#define PAINT_DLG_FILL_SHOWGRADIENTSTEPS        0x00000200
#define PAINT_DLG_FILL_SHOWLOWERTOLERANCE       0x00000400
#define PAINT_DLG_FILL_SHOWUPPERTOLERANCE       0x00000800
#define PAINT_DLG_FILL_SHOWOPACITY              0x00001000
#define PAINT_DLG_FILL_SHOWTEXTURE              0x00002000
#define PAINT_DLG_FILL_SHOWDEFAULT              0x00004000
#define PAINT_DLG_FILL_INITUSEDEFAULT           0x00008000

// Text Flags 

#define PAINT_DLG_TEXT_SHOWALL                  0x00000001
#define PAINT_DLG_TEXT_SHOWTEXT                 0x00000002
#define PAINT_DLG_TEXT_SHOWBKGRNDSTYLE          0x00000004
#define PAINT_DLG_TEXT_SHOWBKGRNDOPAQUECOLOR    0x00000008
#define PAINT_DLG_TEXT_SHOWBKGRNDTILE           0x00000010
#define PAINT_DLG_TEXT_SHOWBORDERSTYLE          0x00000020
#define PAINT_DLG_TEXT_SHOWBORDERWIDTH          0x00000040
#define PAINT_DLG_TEXT_SHOWBORDERCOLOR          0x00000080
#define PAINT_DLG_TEXT_SHOWHORZALIGNMENT        0x00000100
#define PAINT_DLG_TEXT_SHOWVERTALIGNMENT        0x00000200
#define PAINT_DLG_TEXT_SHOWROTATE               0x00000400
#define PAINT_DLG_TEXT_SHOWSCALE                0x00000800
#define PAINT_DLG_TEXT_SHOWTRUETYPEFONT         0x00001000
#define PAINT_DLG_TEXT_SHOWOPACITY              0x00002000
#define PAINT_DLG_TEXT_SHOWTEXTURE              0x00004000
#define PAINT_DLG_TEXT_SHOWDEFAULT              0x00008000
#define PAINT_DLG_TEXT_INITUSEDEFAULT           0x00010000

typedef struct _PAINTDLGBRUSHINFOA
{
   L_INT              nSize ;
   L_INT32            dwFlags ;
   L_CHAR *           pszTitle ;
   PAINTTOUCHCONTENTS nContentsType ; 
   COLORREF           crColor ; 
   L_CHAR **          ppszTouchImage ;
   L_UINT             uTouchImageCount ;
   L_INT              nActiveTouchImageItem ;
   COLORREF           crTransparentColor;
   L_INT              nDiameter;    
   L_INT              nHardnessValue ;
   L_INT              nSpacing;
   L_INT              nDensity;
   L_INT              nOpacity; 
   L_INT              nFadeOutRate ;
   L_CHAR **          ppszPaperTexture ;
   L_UINT             uPaperTextureCount ;
   L_INT              nActivePaperTextureItem ;
   
} PAINTDLGBRUSHINFOA, *pPAINTDLGBRUSHINFOA ;


typedef struct _PAINTDLGSHAPEINFOA
{
   L_INT                      nSize ;
   L_INT32                    dwFlags ;
   L_CHAR *                   pszTitle ;
   PAINTSHAPEBACKSTYLE        nBackgroundStyle ;   
   COLORREF                   crBackgroundColor ;     
   L_CHAR **                  ppszBackgroundTileBitmap ;
   L_UINT                     uBackgroundTileBitmapCount ;
   L_INT                      nActiveBackgroundTileBitmapItem ;
   PAINTSHAPEBORDERSTYLE      nBorderStyle ;
   L_INT                      nBorderWidth ;
   PAINTSHAPEBORDERBRUSHSTYLE nBorderBrushStyle ; 
   COLORREF                   crBorderColor ;   
   L_CHAR **                  ppszBorderTileBitmap ;
   L_UINT                     uBorderTileBitmapCount ;
   L_INT                      nActiveBorderTileBitmapItem ;
   PAINTSHAPEGRADIENTSTYLE    nGradientStyle ;
   COLORREF                   crGradientStartColor ;        
   COLORREF                   crGradientEndColor ;       
   L_UINT                     uGradientSteps ;  
   PAINTSHAPEBORDERENDCAP     nBorderEndCap ;
   L_INT                      nRoundRectEllipseWidth ; 
   L_INT                      nRoundRectEllipseHeight ; 
   L_INT                      nOpacity ;
   L_CHAR **                  ppszPaperTexture ;
   L_UINT                     uPaperTextureCount ;
   L_INT                      nActivePaperTextureItem ;
   
} PAINTDLGSHAPEINFOA, *pPAINTDLGSHAPEINFOA ;


typedef struct _PAINTDLGREGIONINFOA
{
   L_INT    nSize ;
   L_INT32  dwFlags ;
   L_CHAR  *pszTitle ;
   COLORREF crUpperTolerance ;
   COLORREF crLowerTolerance ;
   L_INT    nRoundRectEllipseWidth  ; 
   L_INT    nRoundRectEllipseHeight ; 
   
} PAINTDLGREGIONINFOA, *pPAINTDLGREGIONINFOA ;

typedef struct _PAINTDLGFILLINFOA
{
   L_INT                  nSize ;
   L_INT32                dwFlags ;
   L_CHAR *               pszTitle ;
   PAINTFILLSTYLE         nStyle ; 
   COLORREF               crSolidFillColor ;               
   L_CHAR **              ppszBackgroundTileBitmap ;
   L_UINT                 uBackgroundTileBitmapCount ;
   L_INT                  nActiveBackgroundTileBitmapItem ;
   PAINTFILLGRADIENTSTYLE nGradientStyle ;              
   COLORREF               crGradientStartColor ;        
   COLORREF               crGradientEndColor ;       
   L_UINT                 uGradientSteps ;  
   COLORREF               crUpperTolerance ;
   COLORREF               crLowerTolerance ; 
   L_INT                  nOpacity ; 
   L_CHAR **              ppszPaperTexture ;
   L_UINT                 uPaperTextureCount ;
   L_INT                  nActivePaperTextureItem ;
   
} PAINTDLGFILLINFOA, *pPAINTDLGFILLINFOA ;

typedef struct _PAINTDLGTEXTINFOA
{
   L_INT                     nSize ;
   L_INT32                   dwFlags ;
   L_CHAR *                  pszTitle ;
   L_CHAR *                  pszText ;
   LOGFONTA                  logFont ; 
   PAINTTEXTBORDERBRUSHSTYLE nBorderBrushStyle ; 
   L_INT                     nBorderWidth ; 
   COLORREF                  crBorderColor ; 
   PAINTTEXTBACKSTYLE        nBackgroundStyle ;
   COLORREF                  crBackgroundColor ; 
   L_CHAR **                 ppszBackgroundTileBitmap ;
   L_UINT                    uBackgroundTileBitmapCount ;
   L_INT                     nActiveBackgroundTileBitmapItem ;
   PAINTALIGNMENT            nAlignment ; 
   L_INT                     nOpacity ;   
   L_CHAR **                 ppszPaperTexture ;
   L_UINT                    uPaperTextureCount ;
   L_INT                     nActivePaperTextureItem ;
   PAINTTRANSFORM            TransformInfo ;
   
} PAINTDLGTEXTINFOA, *pPAINTDLGTEXTINFOA ;

   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgBrushA ( HWND hWnd, pPAINTDLGBRUSHINFOA  pBrushDlgInfo ) ;
   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgShapeA ( HWND hWnd, pPAINTDLGSHAPEINFOA  pShapeDlgInfo ) ;
   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgRegionA( HWND hWnd, pPAINTDLGREGIONINFOA pRegionDlgInfo) ;
   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgFillA  ( HWND hWnd, pPAINTDLGFILLINFOA   pFillDlgInfo  ) ;
   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgTextA  ( HWND hWnd, pPAINTDLGTEXTINFOA   pTextDlgInfo  ) ;

#if defined(FOR_UNICODE)
typedef struct _PAINTDLGBRUSHINFO
{
   L_INT              nSize ;
   L_INT32            dwFlags ;
   L_TCHAR *          pszTitle ;
   PAINTTOUCHCONTENTS nContentsType ; 
   COLORREF           crColor ; 
   L_TCHAR **         ppszTouchImage ;
   L_UINT             uTouchImageCount ;
   L_INT              nActiveTouchImageItem ;
   COLORREF           crTransparentColor;
   L_INT              nDiameter;    
   L_INT              nHardnessValue ;
   L_INT              nSpacing;
   L_INT              nDensity;
   L_INT              nOpacity; 
   L_INT              nFadeOutRate ;
   L_TCHAR **         ppszPaperTexture ;
   L_UINT             uPaperTextureCount ;
   L_INT              nActivePaperTextureItem ;
   
} PAINTDLGBRUSHINFO, *pPAINTDLGBRUSHINFO ;


typedef struct _PAINTDLGSHAPEINFO
{
   L_INT                      nSize ;
   L_INT32                    dwFlags ;
   L_TCHAR *                  pszTitle ;
   PAINTSHAPEBACKSTYLE        nBackgroundStyle ;   
   COLORREF                   crBackgroundColor ;     
   L_TCHAR **                 ppszBackgroundTileBitmap ;
   L_UINT                     uBackgroundTileBitmapCount ;
   L_INT                      nActiveBackgroundTileBitmapItem ;
   PAINTSHAPEBORDERSTYLE      nBorderStyle ;
   L_INT                      nBorderWidth ;
   PAINTSHAPEBORDERBRUSHSTYLE nBorderBrushStyle ; 
   COLORREF                   crBorderColor ;   
   L_TCHAR **                 ppszBorderTileBitmap ;
   L_UINT                     uBorderTileBitmapCount ;
   L_INT                      nActiveBorderTileBitmapItem ;
   PAINTSHAPEGRADIENTSTYLE    nGradientStyle ;
   COLORREF                   crGradientStartColor ;        
   COLORREF                   crGradientEndColor ;       
   L_UINT                     uGradientSteps ;  
   PAINTSHAPEBORDERENDCAP     nBorderEndCap ;
   L_INT                      nRoundRectEllipseWidth ; 
   L_INT                      nRoundRectEllipseHeight ; 
   L_INT                      nOpacity ;
   L_TCHAR **                 ppszPaperTexture ;
   L_UINT                     uPaperTextureCount ;
   L_INT                      nActivePaperTextureItem ;
   
} PAINTDLGSHAPEINFO, *pPAINTDLGSHAPEINFO ;


typedef struct _PAINTDLGREGIONINFO
{
   L_INT    nSize ;
   L_INT32  dwFlags ;
   L_TCHAR *pszTitle ;
   COLORREF crUpperTolerance ;
   COLORREF crLowerTolerance ;
   L_INT    nRoundRectEllipseWidth  ; 
   L_INT    nRoundRectEllipseHeight ; 
   
} PAINTDLGREGIONINFO, *pPAINTDLGREGIONINFO ;

typedef struct _PAINTDLGFILLINFO
{
   L_INT                  nSize ;
   L_INT32                dwFlags ;
   L_TCHAR *              pszTitle ;
   PAINTFILLSTYLE         nStyle ; 
   COLORREF               crSolidFillColor ;               
   L_TCHAR **             ppszBackgroundTileBitmap ;
   L_UINT                 uBackgroundTileBitmapCount ;
   L_INT                  nActiveBackgroundTileBitmapItem ;
   PAINTFILLGRADIENTSTYLE nGradientStyle ;              
   COLORREF               crGradientStartColor ;        
   COLORREF               crGradientEndColor ;       
   L_UINT                 uGradientSteps ;  
   COLORREF               crUpperTolerance ;
   COLORREF               crLowerTolerance ; 
   L_INT                  nOpacity ; 
   L_TCHAR **             ppszPaperTexture ;
   L_UINT                 uPaperTextureCount ;
   L_INT                  nActivePaperTextureItem ;
   
} PAINTDLGFILLINFO, *pPAINTDLGFILLINFO ;

typedef struct _PAINTDLGTEXTINFO
{
   L_INT                     nSize ;
   L_INT32                   dwFlags ;
   L_TCHAR *                 pszTitle ;
   L_TCHAR *                 pszText ;
   LOGFONTW                  logFont ; 
   PAINTTEXTBORDERBRUSHSTYLE nBorderBrushStyle ; 
   L_INT                     nBorderWidth ; 
   COLORREF                  crBorderColor ; 
   PAINTTEXTBACKSTYLE        nBackgroundStyle ;
   COLORREF                  crBackgroundColor ; 
   L_TCHAR **                ppszBackgroundTileBitmap ;
   L_UINT                    uBackgroundTileBitmapCount ;
   L_INT                     nActiveBackgroundTileBitmapItem ;
   PAINTALIGNMENT            nAlignment ; 
   L_INT                     nOpacity ;   
   L_TCHAR **                ppszPaperTexture ;
   L_UINT                    uPaperTextureCount ;
   L_INT                     nActivePaperTextureItem ;
   PAINTTRANSFORM            TransformInfo ;
   
} PAINTDLGTEXTINFO, *pPAINTDLGTEXTINFO ;

   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgBrush ( HWND hWnd, pPAINTDLGBRUSHINFO  pBrushDlgInfo ) ;
   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgShape ( HWND hWnd, pPAINTDLGSHAPEINFO  pShapeDlgInfo ) ;
   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgRegion( HWND hWnd, pPAINTDLGREGIONINFO pRegionDlgInfo) ;
   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgFill  ( HWND hWnd, pPAINTDLGFILLINFO   pFillDlgInfo  ) ;
   L_LTPDG_API L_INT EXT_FUNCTION L_PntDlgText  ( HWND hWnd, pPAINTDLGTEXTINFO   pTextDlgInfo  ) ;
#else
typedef PAINTDLGBRUSHINFOA PAINTDLGBRUSHINFO;
typedef pPAINTDLGBRUSHINFOA pPAINTDLGBRUSHINFO;
typedef PAINTDLGSHAPEINFOA PAINTDLGSHAPEINFO;
typedef pPAINTDLGSHAPEINFOA pPAINTDLGSHAPEINFO;
typedef PAINTDLGREGIONINFOA PAINTDLGREGIONINFO;
typedef pPAINTDLGREGIONINFOA pPAINTDLGREGIONINFO;
typedef PAINTDLGFILLINFOA PAINTDLGFILLINFO;
typedef pPAINTDLGFILLINFOA pPAINTDLGFILLINFO;
typedef PAINTDLGTEXTINFOA PAINTDLGTEXTINFO;
typedef pPAINTDLGTEXTINFOA pPAINTDLGTEXTINFO;

#define L_PntDlgBrush L_PntDlgBrushA
#define L_PntDlgShape L_PntDlgShapeA
#define L_PntDlgRegion L_PntDlgRegionA
#define L_PntDlgFill L_PntDlgFillA
#define L_PntDlgText L_PntDlgTextA

#endif // #if defined(FOR_UNICODE)


#undef L_HEADER_ENTRY
#include "ltpck.h"

#endif  // LTPNTDLG_H_INCLUDED
