/*************************************************************
   Ltdlgclr.h - Common Dialogs module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDLG_CLR_H)
#define LTDLG_CLR_H

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

// Flags For L_DlgBalanceColors ( ... ) 
#define DLG_BALANCECOLORS_AUTOPROCESS          0x00000001
#define DLG_BALANCECOLORS_SHOW_CONTEXTHELP     0x00000002
#define DLG_BALANCECOLORS_SHOW_PREVIEW         0x00000004
#define DLG_BALANCECOLORS_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_BALANCECOLORS_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_BALANCECOLORS_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_BALANCECOLORS_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgColoredGray (...) 
#define DLG_COLOREDGRAY_AUTOPROCESS          0x00000001
#define DLG_COLOREDGRAY_SHOW_CONTEXTHELP     0x00000002
#define DLG_COLOREDGRAY_SHOW_PREVIEW         0x00000004
#define DLG_COLOREDGRAY_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_COLOREDGRAY_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_COLOREDGRAY_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_COLOREDGRAY_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgGrayScale ( ... ) 
#define DLG_GRAYSCALE_AUTOPROCESS          0x00000001
#define DLG_GRAYSCALE_SHOW_CONTEXTHELP     0x00000002
#define DLG_GRAYSCALE_SHOW_PREVIEW         0x00000004
#define DLG_GRAYSCALE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_GRAYSCALE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_GRAYSCALE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_GRAYSCALE_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgRemapIntensity ( ... ) 
#define DLG_REMAPINTENSITY_AUTOPROCESS          0x00000001
#define DLG_REMAPINTENSITY_SHOW_CONTEXTHELP     0x00000002
#define DLG_REMAPINTENSITY_SHOW_PREVIEW         0x00000004
#define DLG_REMAPINTENSITY_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_REMAPINTENSITY_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_REMAPINTENSITY_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_REMAPINTENSITY_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgRemapHue ( ... ) 
#define DLG_REMAPHUE_AUTOPROCESS          0x00000001
#define DLG_REMAPHUE_SHOW_CONTEXTHELP     0x00000002
#define DLG_REMAPHUE_SHOW_PREVIEW         0x00000004
#define DLG_REMAPHUE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_REMAPHUE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_REMAPHUE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_REMAPHUE_SHOW_TOOL_RESET      0x00000040

// Flags for L_DlgCustomizePalette ( ... )
#define DLG_CUSTOMIZEPALETTE_SHOW_CONTEXTHELP          0x00000001  
#define DLG_CUSTOMIZEPALETTE_SHOW_SORT                 0x00000002  
#define DLG_CUSTOMIZEPALETTE_SHOW_FINDCLOSEST          0x00000004  
#define DLG_CUSTOMIZEPALETTE_SHOW_ADDOPTION            0x00000008  
#define DLG_CUSTOMIZEPALETTE_SHOW_REMOVEOPTION         0x00000010  
#define DLG_CUSTOMIZEPALETTE_SHOW_COLORMODEL           0x00000020  
#define DLG_CUSTOMIZEPALETTE_SHOW_RGBLEFTPAN           0x00000040  
#define DLG_CUSTOMIZEPALETTE_SHOW_HTMLLEFTPAN          0x00000080  
#define DLG_CUSTOMIZEPALETTE_SHOW_INDEXLEFTPAN         0x00000100  
#define DLG_CUSTOMIZEPALETTE_SHOW_RGBRIGHTPAN          0x00000200  
#define DLG_CUSTOMIZEPALETTE_SHOW_HTMLRIGHTPAN         0x00000400  
#define DLG_CUSTOMIZEPALETTE_SHOW_INDEXRIGHTPAN        0x00000800  
#define DLG_CUSTOMIZEPALETTE_SHOW_NEW                  0x00001000  
#define DLG_CUSTOMIZEPALETTE_SHOW_OPEN                 0x00002000  
#define DLG_CUSTOMIZEPALETTE_SHOW_SAVE                 0x00004000  
#define DLG_CUSTOMIZEPALETTE_SHOW_SAVEAS               0x00008000  
#define DLG_CUSTOMIZEPALETTE_GENERATE_PALETTE          0x00010000  
#define DLG_CUSTOMIZEPALETTE_SHOW_APPLYPALETTEWHENEXIT 0x00020000  

// Flags For L_DlgLocalHistoEqualize ( ... ) 
#define DLG_LOCALHISTOEQUALIZE_AUTOPROCESS          0x00000001
#define DLG_LOCALHISTOEQUALIZE_SHOW_CONTEXTHELP     0x00000002
#define DLG_LOCALHISTOEQUALIZE_SHOW_PREVIEW         0x00000004
#define DLG_LOCALHISTOEQUALIZE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_LOCALHISTOEQUALIZE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_LOCALHISTOEQUALIZE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_LOCALHISTOEQUALIZE_SHOW_TOOL_RESET      0x00000040
#define DLG_LOCALHISTOEQUALIZE_SHOW_APPLY           0x00000080

// Flags For L_DlgIntensityDetect ( ... ) 
#define DLG_INTENSITYDETECT_AUTOPROCESS          0x00000001
#define DLG_INTENSITYDETECT_SHOW_CONTEXTHELP     0x00000002
#define DLG_INTENSITYDETECT_SHOW_PREVIEW         0x00000004
#define DLG_INTENSITYDETECT_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_INTENSITYDETECT_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_INTENSITYDETECT_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_INTENSITYDETECT_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgSolarize ( ... ) 
#define DLG_SOLARIZE_AUTOPROCESS          0x00000001
#define DLG_SOLARIZE_SHOW_CONTEXTHELP     0x00000002
#define DLG_SOLARIZE_SHOW_PREVIEW         0x00000004
#define DLG_SOLARIZE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_SOLARIZE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_SOLARIZE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_SOLARIZE_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgPosterize ( ... ) 
#define DLG_POSTERIZE_AUTOPROCESS          0x00000001
#define DLG_POSTERIZE_SHOW_CONTEXTHELP     0x00000002
#define DLG_POSTERIZE_SHOW_PREVIEW         0x00000004
#define DLG_POSTERIZE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_POSTERIZE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_POSTERIZE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_POSTERIZE_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgBrightness ( ... ) 
#define DLG_BRIGHTNESS_AUTOPROCESS          0x00000001
#define DLG_BRIGHTNESS_SHOW_CONTEXTHELP     0x00000002
#define DLG_BRIGHTNESS_SHOW_PREVIEW         0x00000004
#define DLG_BRIGHTNESS_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_BRIGHTNESS_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_BRIGHTNESS_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_BRIGHTNESS_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgContrast ( ... ) 
#define DLG_CONTRAST_AUTOPROCESS          0x00000001
#define DLG_CONTRAST_SHOW_CONTEXTHELP     0x00000002
#define DLG_CONTRAST_SHOW_PREVIEW         0x00000004
#define DLG_CONTRAST_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_CONTRAST_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_CONTRAST_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_CONTRAST_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgHue ( ... ) 
#define DLG_HUE_AUTOPROCESS          0x00000001
#define DLG_HUE_SHOW_CONTEXTHELP     0x00000002
#define DLG_HUE_SHOW_PREVIEW         0x00000004
#define DLG_HUE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_HUE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_HUE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_HUE_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgSaturation ( ... ) 
#define DLG_SATURATION_AUTOPROCESS          0x00000001
#define DLG_SATURATION_SHOW_CONTEXTHELP     0x00000002
#define DLG_SATURATION_SHOW_PREVIEW         0x00000004
#define DLG_SATURATION_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_SATURATION_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_SATURATION_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_SATURATION_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgGammaAdjustment ( ... ) 
#define DLG_GAMMAADJUSTMENT_AUTOPROCESS          0x00000001
#define DLG_GAMMAADJUSTMENT_SHOW_CONTEXTHELP     0x00000002
#define DLG_GAMMAADJUSTMENT_SHOW_PREVIEW         0x00000004
#define DLG_GAMMAADJUSTMENT_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_GAMMAADJUSTMENT_FORCELINKCHANNELS    0x00000010
#define DLG_GAMMAADJUSTMENT_SHOW_TOOL_ONSCREEN   0x00000020
#define DLG_GAMMAADJUSTMENT_SHOW_TOOL_SHOWEFFECT 0x00000040
#define DLG_GAMMAADJUSTMENT_SHOW_TOOL_RESET      0x00000080

// Flags For L_DlgHalftone ( ... ) 
#define DLG_HALFTONE_AUTOPROCESS          0x00000001
#define DLG_HALFTONE_SHOW_CONTEXTHELP     0x00000002
#define DLG_HALFTONE_SHOW_PREVIEW         0x00000004
#define DLG_HALFTONE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_HALFTONE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_HALFTONE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_HALFTONE_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgColorRes ( ... ) 
#define DLG_COLORRES_AUTOPROCESS          0x00000001
#define DLG_COLORRES_SHOW_CONTEXTHELP     0x00000002
#define DLG_COLORRES_SHOW_PREVIEW         0x00000004
#define DLG_COLORRES_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_COLORRES_SHOW_ORDER           0x00000010
#define DLG_COLORRES_SHOW_OPENPALFILE     0x00000020
#define DLG_COLORRES_SHOW_TOOL_ONSCREEN   0x00000040
#define DLG_COLORRES_SHOW_TOOL_SHOWEFFECT 0x00000080
#define DLG_COLORRES_SHOW_TOOL_RESET      0x00000100

// Extra Flags For L_DlgColorRes ( ... ) 
#define DLG_COLORRES_SHOW_1BIT             0x00000001 
#define DLG_COLORRES_SHOW_2BIT             0x00000002 
#define DLG_COLORRES_SHOW_3BIT             0x00000004 
#define DLG_COLORRES_SHOW_4BIT             0x00000008 
#define DLG_COLORRES_SHOW_5BIT             0x00000010 
#define DLG_COLORRES_SHOW_6BIT             0x00000020 
#define DLG_COLORRES_SHOW_7BIT             0x00000040 
#define DLG_COLORRES_SHOW_8BIT             0x00000080 
#define DLG_COLORRES_SHOW_12BIT            0x00000100 
#define DLG_COLORRES_SHOW_16BIT            0x00000200 
#define DLG_COLORRES_SHOW_24BIT            0x00000400 
#define DLG_COLORRES_SHOW_32BIT            0x00000800 
#define DLG_COLORRES_SHOW_48BIT            0x00001000 
#define DLG_COLORRES_SHOW_64BIT            0x00002000 
#define DLG_COLORRES_SHOW_BITALL           0x00003FFF 

#define DLG_COLORRES_SHOW_DITHER_NONE      0x00004000 
#define DLG_COLORRES_SHOW_DITHER_FLOYD     0x00008000 
#define DLG_COLORRES_SHOW_DITHER_STUCKI    0x00010000 
#define DLG_COLORRES_SHOW_DITHER_BURKES    0x00020000 
#define DLG_COLORRES_SHOW_DITHER_SIERRA    0x00040000 
#define DLG_COLORRES_SHOW_DITHER_STEVENSON 0x00080000 
#define DLG_COLORRES_SHOW_DITHER_JARVIS    0x00100000 
#define DLG_COLORRES_SHOW_DITHER_CLUSTER   0x00200000 
#define DLG_COLORRES_SHOW_DITHER_ORDERED   0x00400000 
#define DLG_COLORRES_SHOW_DITHER_ALL       0x007FC000 

#define DLG_COLORRES_SHOW_PAL_FIXED        0x00800000 
#define DLG_COLORRES_SHOW_PAL_OPTIMIZED    0x01000000 
#define DLG_COLORRES_SHOW_PAL_IDENTITY     0x02000000 
#define DLG_COLORRES_SHOW_PAL_NETSCAPE     0x04000000 
#define DLG_COLORRES_SHOW_PAL_SVGA         0x08000000 
#define DLG_COLORRES_SHOW_PAL_UNIFORM      0x10000000 
#define DLG_COLORRES_SHOW_PAL_MSIE         0x20000000 
#define DLG_COLORRES_SHOW_PAL_ALL          0x3F800000 

#define DLG_COLORRES_ORDER_HIDEGRAYSCALE   0x40000000 

// Flags For L_DlgHistoContrast ( ... ) 
#define DLG_HISTOCONTRAST_AUTOPROCESS          0x00000001
#define DLG_HISTOCONTRAST_SHOW_CONTEXTHELP     0x00000002
#define DLG_HISTOCONTRAST_SHOW_PREVIEW         0x00000004
#define DLG_HISTOCONTRAST_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_HISTOCONTRAST_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_HISTOCONTRAST_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_HISTOCONTRAST_SHOW_TOOL_RESET      0x00000040

// Flags for L_DlgWindowLevel ( ... )
#define DLG_WINDOWLEVEL_AUTOPROCESS          0x00000001  
#define DLG_WINDOWLEVEL_SHOW_CONTEXTHELP     0x00000002  
#define DLG_WINDOWLEVEL_SHOW_PREVIEW         0x00000004
#define DLG_WINDOWLEVEL_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_WINDOWLEVEL_SHOW_RANGE           0x00000010

// Flags for L_DlgColor (...)
#define DLG_COLOR_SHOW_CONTEXTHELP           0x00000001
#define DLG_COLOR_SHOW_OLDCOLOR              0x00000002 
#define DLG_COLOR_SHOW_NEWCOLOR              0x00000004 
#define DLG_COLOR_SHOW_NAME                  0x00000008
#define DLG_COLOR_COLORSPACE_SHOW_HUE        0x00000010 
#define DLG_COLOR_COLORSPACE_SHOW_BRIGHTNESS 0x00000020 
#define DLG_COLOR_COLORSPACE_SHOW_WHEEL      0x00000040 
#define DLG_COLOR_COLORSPACE_SHOW_RGB        0x00000080 
#define DLG_COLOR_COLORSPACE_SHOW_CMY        0x00000100 
#define DLG_COLOR_COLORSPACE_SHOW_CMYK       0x00000200 
#define DLG_COLOR_COLORSPACE_SHOW_LAB        0x00000400 
#define DLG_COLOR_COLORMODEL_SHOW_RGB        0x00000800
#define DLG_COLOR_COLORMODEL_SHOW_HSB        0x00001000
#define DLG_COLOR_COLORMODEL_SHOW_HLS        0x00002000
#define DLG_COLOR_COLORMODEL_SHOW_CMY        0x00004000
#define DLG_COLOR_COLORMODEL_SHOW_CMYK       0x00008000
#define DLG_COLOR_COLORMODEL_SHOW_LAB        0x00010000


typedef struct _BALANCECOLORSDLGPARAMS
{
   L_UINT          uStructSize ; 
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   BALANCING       RedFactor ;   
   BALANCING       GreenFactor ; 
   BALANCING       BlueFactor ;  
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} BALANCECOLORSDLGPARAMS, * LPBALANCECOLORSDLGPARAMS ;

typedef struct _COLOREDGRAYDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;       
   L_INT            nRedFactor ;       
   L_INT            nGreenFactor ;     
   L_INT            nBlueFactor ;      
   L_INT            nRedGrayFactor ;   
   L_INT            nGreenGrayFactor ; 
   L_INT            nBlueGrayFactor ;  
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} COLOREDGRAYDLGPARAMS, * LPCOLOREDGRAYDLGPARAMS ;

typedef struct _GRAYSCALEDLGPARAMS
{
   L_UINT           uStructSize ;      
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nRedFactor ;   
   L_INT            nGreenFactor ; 
   L_INT            nBlueFactor ;  
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} GRAYSCALEDLGPARAMS, * LPGRAYSCALEDLGPARAMS ;

typedef struct _REMAPINTENSITYDLGPARAMS
{
   L_UINT           uStructSize ;         
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;     
   L_UINT           uChannel ;   
   L_UINT *         puRGBLookup ;   
   L_UINT *         puRedLookup ;   
   L_UINT *         puGreenLookup ; 
   L_UINT *         puBlueLookup ;  
   L_UINT           uLookupLen ;     
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} REMAPINTENSITYDLGPARAMS, * LPREMAPINTENSITYDLGPARAMS ;

typedef struct _REMAPHUEDLGPARAMS
{
   L_UINT          uStructSize ;              
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;          
   L_UINT *        puMaskLookup ;       
   L_UINT *        puHueLookup ;        
   L_UINT *        puSaturationLookup ; 
   L_UINT *        puValueLookup ;      
   L_UINT          uLookupLen ;          
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} REMAPHUEDLGPARAMS, * LPREMAPHUEDLGPARAMS ;

typedef struct _CUSTOMIZEPALETTEDLGPARAMS
{
   L_UINT           uStructSize ;
   HPALETTE         hpalUser ;
   HPALETTE         hpalGenerated ;
   L_BOOL           bApplyPaletteWhenExit ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} CUSTOMIZEPALETTEDLGPARAMS, * LPCUSTOMIZEPALETTEDLGPARAMS ;

typedef struct _LOCALHISTOEQUALIZEDLGPARAMS
{
   L_UINT           uStructSize ;   
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nWidth ; 
   L_INT            nHeight ;
   L_INT            nXExtention ;
   L_INT            nYExtention ;
   L_UINT           uType ;
   L_UINT           uSmooth ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} LOCALHISTOEQUALIZEDLGPARAMS, * LPLOCALHISTOEQUALIZEDLGPARAMS ;

typedef struct _INTENSITYDETECTDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT          uLow ;
   L_UINT          uHigh ;
   L_UINT          uChannel ;
   COLORREF        crInColor ;
   COLORREF        crOutColor ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} INTENSITYDETECTDLGPARAMS, * LPINTENSITYDETECTDLGPARAMS ;

typedef struct _SOLARIZEDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nThreshold ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} SOLARIZEDLGPARAMS, * LPSOLARIZEDLGPARAMS ;

typedef struct _POSTERIZEDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nLevels ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *          pHelpCallBackUserData ;

} POSTERIZEDLGPARAMS, * LPPOSTERIZEDLGPARAMS ;

typedef struct _BRIGHTNESSDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nChange ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} BRIGHTNESSDLGPARAMS, * LPBRIGHTNESSDLGPARAMS ;

typedef struct _CONTRASTDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nChange ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} CONTRASTDLGPARAMS, * LPCONTRASTDLGPARAMS ;

typedef struct _HUEDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nAngle ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} HUEDLGPARAMS, * LPHUEDLGPARAMS ;

typedef struct _SATURATIONDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nChange ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} SATURATIONDLGPARAMS, * LPSATURATIONDLGPARAMS ;

typedef struct _GAMMAADJUSTMENTDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nRedValue ;
   L_INT           nGreenValue ;
   L_INT           nBlueValue ;
   L_BOOL          bAllChannels ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} GAMMAADJUSTMENTDLGPARAMS, * LPGAMMAADJUSTMENTDLGPARAMS ;

typedef struct _HALFTONEDLGPARAMSA
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT          uDim ;  
   L_INT           nAngle ; 
   L_UINT32        uType ; 
   LPDLGBITMAPLISTA pBitmapList ;
   L_UINT *        puInListIndexes ;
   L_INT32         nInCount ; 
   HBITMAPLIST     hList ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} HALFTONEDLGPARAMSA, * LPHALFTONEDLGPARAMSA ;
#if defined(FOR_UNICODE)
typedef struct _HALFTONEDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT          uDim ;  
   L_INT           nAngle ; 
   L_UINT32        uType ; 
   LPDLGBITMAPLIST pBitmapList ;
   L_UINT *        puInListIndexes ;
   L_INT32         nInCount ; 
   HBITMAPLIST     hList ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} HALFTONEDLGPARAMS, * LPHALFTONEDLGPARAMS ;
#else
typedef HALFTONEDLGPARAMSA HALFTONEDLGPARAMS;
typedef LPHALFTONEDLGPARAMSA LPHALFTONEDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

typedef struct _COLORRESDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nBitsPerPixel ;   
   L_UINT32        uColorResFlags ;  
   HPALETTE        hpalCustom ;      
   L_UINT32        uDlgFlags ; 
   L_UINT32        uDlgFlagsEx ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} COLORRESDLGPARAMS, * LPCOLORRESDLGPARAMS ;

typedef struct _HISTOCONTRASTDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nChange ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} HISTOCONTRASTDLGPARAMS, * LPHISTOCONTRASTDLGPARAMS ;

typedef struct _WINDOWLEVELDLGPARAMS
{
   L_UINT            uStructSize ;          
   pBITMAPHANDLE     pBitmap ;
   L_BOOL            bZoomToFit ;  
   L_RGBQUAD         *pLUT ;
   L_UINT32          uLUTLength ; 
   L_UINT            uLowBit ; 
   L_UINT            uHighBit ; 
   L_INT             nLow ;
   L_INT             nHigh ; 
   COLORREF          crStart ; 
   COLORREF          crEnd ;
   L_INT             nFactor ; 
   L_UINT32          uWindowLevelFlags;
   L_UINT32          uDlgFlags ; 
   LPPOINT           pptPosition ;   
   LTCOMMDLGHELPCB   pfnHelpCallback ; 
   L_VOID *          pHelpCallBackUserData ;

} WINDOWLEVELDLGPARAMS, * LPWINDOWLEVELDLGPARAMS ;

typedef struct _COLORDLGPARAMS
{
   L_UINT          uStructSize ;       
   COLORREF        crColor ;     
   L_UINT32        uColorSpace ; 
   L_UINT32        uColorModel ; 
   HPALETTE        hpalCustom ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} COLORDLGPARAMS, * LPCOLORDLGPARAMS ;
//.............................................................................
// enums, defines and structures
//.............................................................................


//.............................................................................
// Functions
//.............................................................................
L_LTDLG_API L_INT EXT_FUNCTION L_DlgBalanceColors ( HWND hWndOwner,
                                       LPBALANCECOLORSDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgColoredGray ( HWND hWndOwner,
                                     LPCOLOREDGRAYDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGrayScale ( HWND hWndOwner,          
                                   LPGRAYSCALEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgRemapIntensity ( HWND hWndOwner,
                                        LPREMAPINTENSITYDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgRemapHue ( HWND hWndOwner,
                                  LPREMAPHUEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgCustomizePalette ( HWND hWndOwner,
                                          LPCUSTOMIZEPALETTEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgLocalHistoEqualize ( HWND hWndOwner,
                                             LPLOCALHISTOEQUALIZEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgIntensityDetect ( HWND hWndOwner,
                                          LPINTENSITYDETECTDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgSolarize ( HWND hWndOwner,
                                   LPSOLARIZEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPosterize ( HWND hWndOwner,
                                    LPPOSTERIZEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgBrightness ( HWND hWndOwner,
                                     LPBRIGHTNESSDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgContrast ( HWND hWndOwner,
                                   LPCONTRASTDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgHue ( HWND hWndOwner,
                              LPHUEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgSaturation ( HWND hWndOwner,
                                     LPSATURATIONDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGammaAdjustment ( HWND hWndOwner,
                                          LPGAMMAADJUSTMENTDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgHalftoneA ( HWND hWndOwner,
                                   LPHALFTONEDLGPARAMSA pDlgParams ) ;
#if defined(FOR_UNICODE)
L_LTDLG_API L_INT EXT_FUNCTION L_DlgHalftone ( HWND hWndOwner,
                                   LPHALFTONEDLGPARAMS pDlgParams ) ;
#else
#define L_DlgHalftone L_DlgHalftoneA
#endif // #if defined(FOR_UNICODE)

L_LTDLG_API L_INT EXT_FUNCTION L_DlgColorRes ( HWND hWndOwner,
                                   LPCOLORRESDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgHistoContrast ( HWND hWndOwner,
                                        LPHISTOCONTRASTDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgWindowLevel ( HWND hWndOwner,
                                      LPWINDOWLEVELDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgColor ( HWND hWndOwner,
                                LPCOLORDLGPARAMS pDlgParams ) ;

//.............................................................................
// Functions
//.............................................................................

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDLG_CLR_H)
