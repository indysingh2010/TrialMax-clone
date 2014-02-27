/*************************************************************
   Ltdlgimgefx.h - Common Dialogs module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDLG_IMGEFX_H)
#define LTDLG_IMGEFX_H

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

// Flags For L_DlgMotionBlur ( ... )
#define DLG_MOTIONBLUR_AUTOPROCESS          0x00000001
#define DLG_MOTIONBLUR_SHOW_CONTEXTHELP     0x00000002
#define DLG_MOTIONBLUR_SHOW_PREVIEW         0x00000004
#define DLG_MOTIONBLUR_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_MOTIONBLUR_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_MOTIONBLUR_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_MOTIONBLUR_SHOW_TOOL_RESET      0x00000040
#define DLG_MOTIONBLUR_SHOW_APPLY           0x00000080

// Flags For L_DlgRadialBlur ( ... ) 
#define DLG_RADIALBLUR_AUTOPROCESS          0x00000001
#define DLG_RADIALBLUR_SHOW_CONTEXTHELP     0x00000002
#define DLG_RADIALBLUR_SHOW_PREVIEW         0x00000004
#define DLG_RADIALBLUR_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_RADIALBLUR_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_RADIALBLUR_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_RADIALBLUR_SHOW_TOOL_RESET      0x00000040
#define DLG_RADIALBLUR_SHOW_APPLY           0x00000080

// Flags For L_DlgZoomBlur ( ... ) 
#define DLG_ZOOMBLUR_AUTOPROCESS          0x00000001
#define DLG_ZOOMBLUR_SHOW_CONTEXTHELP     0x00000002
#define DLG_ZOOMBLUR_SHOW_PREVIEW         0x00000004
#define DLG_ZOOMBLUR_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_ZOOMBLUR_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_ZOOMBLUR_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_ZOOMBLUR_SHOW_TOOL_RESET      0x00000040
#define DLG_ZOOMBLUR_SHOW_APPLY           0x00000080

// Flags For L_DlgGaussianBlur ( ... ) 
#define DLG_GAUSSIANBLUR_AUTOPROCESS          0x00000001
#define DLG_GAUSSIANBLUR_SHOW_CONTEXTHELP     0x00000002
#define DLG_GAUSSIANBLUR_SHOW_PREVIEW         0x00000004
#define DLG_GAUSSIANBLUR_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_GAUSSIANBLUR_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_GAUSSIANBLUR_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_GAUSSIANBLUR_SHOW_TOOL_RESET      0x00000040
#define DLG_GAUSSIANBLUR_SHOW_APPLY           0x00000080

// Flags For L_DlgAntiAlias ( ... )
#define DLG_ANTIALIAS_AUTOPROCESS          0x00000001
#define DLG_ANTIALIAS_SHOW_CONTEXTHELP     0x00000002
#define DLG_ANTIALIAS_SHOW_PREVIEW         0x00000004
#define DLG_ANTIALIAS_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_ANTIALIAS_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_ANTIALIAS_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_ANTIALIAS_SHOW_TOOL_RESET      0x00000040
#define DLG_ANTIALIAS_SHOW_APPLY           0x00000080

// Flags For L_DlgAverage ( ... )
#define DLG_AVERAGE_AUTOPROCESS          0x00000001
#define DLG_AVERAGE_SHOW_CONTEXTHELP     0x00000002
#define DLG_AVERAGE_SHOW_PREVIEW         0x00000004
#define DLG_AVERAGE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_AVERAGE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_AVERAGE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_AVERAGE_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgMedian ( ... )
#define DLG_MEDIAN_AUTOPROCESS          0x00000001
#define DLG_MEDIAN_SHOW_CONTEXTHELP     0x00000002
#define DLG_MEDIAN_SHOW_PREVIEW         0x00000004
#define DLG_MEDIAN_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_MEDIAN_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_MEDIAN_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_MEDIAN_SHOW_TOOL_RESET      0x00000040
#define DLG_MEDIAN_SHOW_APPLY           0x00000080

// Flags For L_DlgAddNoise ( ... )
#define DLG_ADDNOISE_AUTOPROCESS          0x00000001
#define DLG_ADDNOISE_SHOW_CONTEXTHELP     0x00000002
#define DLG_ADDNOISE_SHOW_PREVIEW         0x00000004
#define DLG_ADDNOISE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_ADDNOISE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_ADDNOISE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_ADDNOISE_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgMaxFilter ( ... )
#define DLG_MAXFILTER_AUTOPROCESS          0x00000001
#define DLG_MAXFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_MAXFILTER_SHOW_PREVIEW         0x00000004
#define DLG_MAXFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_MAXFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_MAXFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_MAXFILTER_SHOW_TOOL_RESET      0x00000040
#define DLG_MAXFILTER_SHOW_APPLY           0x00000080

// Flags For L_DlgMinFilter ( ... )
#define DLG_MINFILTER_AUTOPROCESS          0x00000001
#define DLG_MINFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_MINFILTER_SHOW_PREVIEW         0x00000004
#define DLG_MINFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_MINFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_MINFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_MINFILTER_SHOW_TOOL_RESET      0x00000040
#define DLG_MINFILTER_SHOW_APPLY           0x00000080

// Flags For L_DlgSharpen ( ... )
#define DLG_SHARPEN_AUTOPROCESS          0x00000001
#define DLG_SHARPEN_SHOW_CONTEXTHELP     0x00000002
#define DLG_SHARPEN_SHOW_PREVIEW         0x00000004
#define DLG_SHARPEN_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_SHARPEN_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_SHARPEN_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_SHARPEN_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgShiftDifferenceFilter ( ... )
#define DLG_SHIFTDIFFERENCEFILTER_AUTOPROCESS          0x00000001
#define DLG_SHIFTDIFFERENCEFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_SHIFTDIFFERENCEFILTER_SHOW_PREVIEW         0x00000004
#define DLG_SHIFTDIFFERENCEFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_SHIFTDIFFERENCEFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_SHIFTDIFFERENCEFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_SHIFTDIFFERENCEFILTER_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgEmboss ( ... )
#define DLG_EMBOSS_AUTOPROCESS          0x00000001
#define DLG_EMBOSS_SHOW_CONTEXTHELP     0x00000002
#define DLG_EMBOSS_SHOW_PREVIEW         0x00000004
#define DLG_EMBOSS_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_EMBOSS_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_EMBOSS_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_EMBOSS_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgOilify ( ... )
#define DLG_OILIFY_AUTOPROCESS          0x00000001
#define DLG_OILIFY_SHOW_CONTEXTHELP     0x00000002
#define DLG_OILIFY_SHOW_PREVIEW         0x00000004
#define DLG_OILIFY_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_OILIFY_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_OILIFY_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_OILIFY_SHOW_TOOL_RESET      0x00000040
#define DLG_OILIFY_SHOW_APPLY           0x00000080

// Flags For L_DlgMosaic ( ... )
#define DLG_MOSAIC_AUTOPROCESS          0x00000001
#define DLG_MOSAIC_SHOW_CONTEXTHELP     0x00000002
#define DLG_MOSAIC_SHOW_PREVIEW         0x00000004
#define DLG_MOSAIC_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_MOSAIC_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_MOSAIC_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_MOSAIC_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgErosionFilter ( ... )
#define DLG_EROSIONFILTER_AUTOPROCESS          0x00000001
#define DLG_EROSIONFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_EROSIONFILTER_SHOW_PREVIEW         0x00000004
#define DLG_EROSIONFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_EROSIONFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_EROSIONFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_EROSIONFILTER_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgDilationFilter ( ... )
#define DLG_DILATIONFILTER_AUTOPROCESS          0x00000001
#define DLG_DILATIONFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_DILATIONFILTER_SHOW_PREVIEW         0x00000004
#define DLG_DILATIONFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_DILATIONFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_DILATIONFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_DILATIONFILTER_SHOW_TOOL_RESET      0x00000040

// flags for L_DlgContourFilter() 
#define DLG_CONTOURFILTER_AUTOPROCESS                0x00000001  
#define DLG_CONTOURFILTER_SHOW_CONTEXTHELP           0x00000002  
#define DLG_CONTOURFILTER_SHOW_PREVIEW               0x00000004  
#define DLG_CONTOURFILTER_SHOW_TOOL_ZOOMLEVEL        0x00000008  
#define DLG_CONTOURFILTER_SHOW_OPTION                0x00000010  
#define DLG_CONTOURFILTER_SHOW_THRESHOLD             0x00000020  
#define DLG_CONTOURFILTER_SHOW_DELTADIRECTION        0x00000040  
#define DLG_CONTOURFILTER_SHOW_MAXIMUMERROR          0x00000080  
#define DLG_CONTOURFILTER_SHOW_TOOL_ONSCREEN         0x00000100
#define DLG_CONTOURFILTER_SHOW_TOOL_SHOWEFFECT       0x00000200
#define DLG_CONTOURFILTER_SHOW_TOOL_RESET            0x00000400
#define DLG_CONTOURFILTER_SHOW_APPLY                 0x00000800

// flags for uOptionFlags in LPCONTOURFILTERDLGPARAMS
#define DLG_CONTOURFILTER_SHOW_OPTION_THIN           0x00000001
#define DLG_CONTOURFILTER_SHOW_OPTION_LINK_BW        0x00000002
#define DLG_CONTOURFILTER_SHOW_OPTION_LINK_GRAY      0x00000004
#define DLG_CONTOURFILTER_SHOW_OPTION_LINK_COLOR     0x00000008
#define DLG_CONTOURFILTER_SHOW_OPTION_APPROX_COLOR   0x00000010
#define DLG_CONTOURFILTER_SHOW_OPTION_ALL            0x0000001F

// Flags For L_DlgGradientFilter ( ... )
#define DLG_GRADIENTFILTER_AUTOPROCESS          0x00000001
#define DLG_GRADIENTFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_GRADIENTFILTER_SHOW_PREVIEW         0x00000004
#define DLG_GRADIENTFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_GRADIENTFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_GRADIENTFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_GRADIENTFILTER_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgLaplacianFilter ( ... )
#define DLG_LAPLACIANFILTER_AUTOPROCESS          0x00000001
#define DLG_LAPLACIANFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_LAPLACIANFILTER_SHOW_PREVIEW         0x00000004
#define DLG_LAPLACIANFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_LAPLACIANFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_LAPLACIANFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_LAPLACIANFILTER_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgSobelFilter ( ... )
#define DLG_SOBELFILTER_AUTOPROCESS          0x00000001
#define DLG_SOBELFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_SOBELFILTER_SHOW_PREVIEW         0x00000004
#define DLG_SOBELFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_SOBELFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_SOBELFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_SOBELFILTER_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgPrewittFilter ( ... )
#define DLG_PREWITTFILTER_AUTOPROCESS          0x00000001
#define DLG_PREWITTFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_PREWITTFILTER_SHOW_PREVIEW         0x00000004
#define DLG_PREWITTFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_PREWITTFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_PREWITTFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_PREWITTFILTER_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgLineSegmentFilter ( ... )
#define DLG_LINESEGMENTFILTER_AUTOPROCESS          0x00000001
#define DLG_LINESEGMENTFILTER_SHOW_CONTEXTHELP     0x00000002
#define DLG_LINESEGMENTFILTER_SHOW_PREVIEW         0x00000004
#define DLG_LINESEGMENTFILTER_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_LINESEGMENTFILTER_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_LINESEGMENTFILTER_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_LINESEGMENTFILTER_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgUnsharpMask ( ... )
#define DLG_UNSHARPMASK_AUTOPROCESS          0x00000001
#define DLG_UNSHARPMASK_SHOW_CONTEXTHELP     0x00000002
#define DLG_UNSHARPMASK_SHOW_PREVIEW         0x00000004
#define DLG_UNSHARPMASK_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_UNSHARPMASK_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_UNSHARPMASK_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_UNSHARPMASK_SHOW_TOOL_RESET      0x00000040
#define DLG_UNSHARPMASK_SHOW_APPLY           0x00000080

// Flags For L_DlgMultiply ( ... ) 
#define DLG_MULTIPLY_AUTOPROCESS          0x00000001
#define DLG_MULTIPLY_SHOW_CONTEXTHELP     0x00000002
#define DLG_MULTIPLY_SHOW_PREVIEW         0x00000004
#define DLG_MULTIPLY_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_MULTIPLY_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_MULTIPLY_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_MULTIPLY_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgAddBitmaps ( ... ) 
#define DLG_ADDBITMAPS_AUTOPROCESS          0x00000001
#define DLG_ADDBITMAPS_SHOW_CONTEXTHELP     0x00000002
#define DLG_ADDBITMAPS_SHOW_PREVIEW         0x00000004
#define DLG_ADDBITMAPS_SHOW_TOOL_ZOOMLEVEL  0x00000008

// Flags for L_DlgStitch ( ... )
#define DLG_STITCH_NOPAGESETUPONSTART 0x00000001  

// Flags For L_DlgFreeHandWave ( ... ) 
#define DLG_FREEHANDWAVE_AUTOPROCESS          0x00000001
#define DLG_FREEHANDWAVE_SHOW_CONTEXTHELP     0x00000002
#define DLG_FREEHANDWAVE_SHOW_PREVIEW         0x00000004
#define DLG_FREEHANDWAVE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_FREEHANDWAVE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_FREEHANDWAVE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_FREEHANDWAVE_SHOW_TOOL_RESET      0x00000040
#define DLG_FREEHANDWAVE_SHOW_APPLY           0x00000080

// Flags For L_DlgWind ( ... ) 
#define DLG_WIND_AUTOPROCESS          0x00000001
#define DLG_WIND_SHOW_CONTEXTHELP     0x00000002
#define DLG_WIND_SHOW_PREVIEW         0x00000004
#define DLG_WIND_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_WIND_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_WIND_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_WIND_SHOW_TOOL_RESET      0x00000040
#define DLG_WIND_SHOW_APPLY           0x00000080

// Flags For L_DlgPolar ( ... ) 
#define DLG_POLAR_AUTOPROCESS          0x00000001
#define DLG_POLAR_SHOW_CONTEXTHELP     0x00000002
#define DLG_POLAR_SHOW_PREVIEW         0x00000004
#define DLG_POLAR_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_POLAR_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_POLAR_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_POLAR_SHOW_TOOL_RESET      0x00000040
#define DLG_POLAR_SHOW_APPLY           0x00000080

// Flags For L_DlgZoomWave ( ... ) 
#define DLG_ZOOMWAVE_AUTOPROCESS          0x00000001
#define DLG_ZOOMWAVE_SHOW_CONTEXTHELP     0x00000002
#define DLG_ZOOMWAVE_SHOW_PREVIEW         0x00000004
#define DLG_ZOOMWAVE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_ZOOMWAVE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_ZOOMWAVE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_ZOOMWAVE_SHOW_TOOL_RESET      0x00000040
#define DLG_ZOOMWAVE_SHOW_APPLY           0x00000080

// Flags For L_DlgRadialWave ( ... ) 
#define DLG_RADIALWAVE_AUTOPROCESS          0x00000001
#define DLG_RADIALWAVE_SHOW_CONTEXTHELP     0x00000002
#define DLG_RADIALWAVE_SHOW_PREVIEW         0x00000004
#define DLG_RADIALWAVE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_RADIALWAVE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_RADIALWAVE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_RADIALWAVE_SHOW_TOOL_RESET      0x00000040
#define DLG_RADIALWAVE_SHOW_APPLY           0x00000080

// Flags For L_DlgSwirl ( ... ) 
#define DLG_SWIRL_AUTOPROCESS          0x00000001
#define DLG_SWIRL_SHOW_CONTEXTHELP     0x00000002
#define DLG_SWIRL_SHOW_PREVIEW         0x00000004
#define DLG_SWIRL_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_SWIRL_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_SWIRL_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_SWIRL_SHOW_TOOL_RESET      0x00000040
#define DLG_SWIRL_SHOW_APPLY           0x00000080

// Flags For L_DlgWave ( ... ) 
#define DLG_WAVE_AUTOPROCESS          0x00000001
#define DLG_WAVE_SHOW_CONTEXTHELP     0x00000002
#define DLG_WAVE_SHOW_PREVIEW         0x00000004
#define DLG_WAVE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_WAVE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_WAVE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_WAVE_SHOW_TOOL_RESET      0x00000040
#define DLG_WAVE_SHOW_APPLY           0x00000080

// Flags For L_DlgWaveShear ( ... ) 
#define DLG_WAVESHEAR_AUTOPROCESS          0x00000001
#define DLG_WAVESHEAR_SHOW_CONTEXTHELP     0x00000002
#define DLG_WAVESHEAR_SHOW_PREVIEW         0x00000004
#define DLG_WAVESHEAR_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_WAVESHEAR_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_WAVESHEAR_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_WAVESHEAR_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgPunch ( ... ) 
#define DLG_PUNCH_AUTOPROCESS          0x00000001
#define DLG_PUNCH_SHOW_CONTEXTHELP     0x00000002
#define DLG_PUNCH_SHOW_PREVIEW         0x00000004
#define DLG_PUNCH_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_PUNCH_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_PUNCH_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_PUNCH_SHOW_TOOL_RESET      0x00000040
#define DLG_PUNCH_SHOW_APPLY           0x00000080

// Flags For L_DlgRipple ( ... ) 
#define DLG_RIPPLE_AUTOPROCESS          0x00000001
#define DLG_RIPPLE_SHOW_CONTEXTHELP     0x00000002
#define DLG_RIPPLE_SHOW_PREVIEW         0x00000004
#define DLG_RIPPLE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_RIPPLE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_RIPPLE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_RIPPLE_SHOW_TOOL_RESET      0x00000040
#define DLG_RIPPLE_SHOW_APPLY           0x00000080

// Flags For L_DlgBending ( ... ) 
#define DLG_BENDING_AUTOPROCESS          0x00000001
#define DLG_BENDING_SHOW_CONTEXTHELP     0x00000002
#define DLG_BENDING_SHOW_PREVIEW         0x00000004
#define DLG_BENDING_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_BENDING_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_BENDING_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_BENDING_SHOW_TOOL_RESET      0x00000040
#define DLG_BENDING_SHOW_APPLY           0x00000080

// Flags For L_DlgCylindrical ( ... ) 
#define DLG_CYLINDRICAL_AUTOPROCESS          0x00000001
#define DLG_CYLINDRICAL_SHOW_CONTEXTHELP     0x00000002
#define DLG_CYLINDRICAL_SHOW_PREVIEW         0x00000004
#define DLG_CYLINDRICAL_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_CYLINDRICAL_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_CYLINDRICAL_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_CYLINDRICAL_SHOW_TOOL_RESET      0x00000040
#define DLG_CYLINDRICAL_SHOW_APPLY           0x00000080

// Flags For L_DlgSpherize ( ... ) 
#define DLG_SPHERIZE_AUTOPROCESS          0x00000001
#define DLG_SPHERIZE_SHOW_CONTEXTHELP     0x00000002
#define DLG_SPHERIZE_SHOW_PREVIEW         0x00000004
#define DLG_SPHERIZE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_SPHERIZE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_SPHERIZE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_SPHERIZE_SHOW_TOOL_RESET      0x00000040
#define DLG_SPHERIZE_SHOW_APPLY           0x00000080

// Flags For L_DlgImpressionist ( ... ) 
#define DLG_IMPRESSIONIST_AUTOPROCESS          0x00000001
#define DLG_IMPRESSIONIST_SHOW_CONTEXTHELP     0x00000002
#define DLG_IMPRESSIONIST_SHOW_PREVIEW         0x00000004
#define DLG_IMPRESSIONIST_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_IMPRESSIONIST_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_IMPRESSIONIST_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_IMPRESSIONIST_SHOW_TOOL_RESET      0x00000040

// Flags For L_DlgPixelate ( ... ) 
#define DLG_PIXELATE_AUTOPROCESS          0x00000001
#define DLG_PIXELATE_SHOW_CONTEXTHELP     0x00000002
#define DLG_PIXELATE_SHOW_PREVIEW         0x00000004
#define DLG_PIXELATE_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_PIXELATE_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_PIXELATE_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_PIXELATE_SHOW_TOOL_RESET      0x00000040
#define DLG_PIXELATE_SHOW_APPLY           0x00000080

// Flags For L_DlgEdgeDetector ( ... )
#define DLG_EDGEDETECTOR_AUTOPROCESS          0x00000001
#define DLG_EDGEDETECTOR_SHOW_CONTEXTHELP     0x00000002
#define DLG_EDGEDETECTOR_SHOW_PREVIEW         0x00000004
#define DLG_EDGEDETECTOR_SHOW_TOOL_ZOOMLEVEL  0x00000008
#define DLG_EDGEDETECTOR_SHOW_TOOL_ONSCREEN   0x00000010
#define DLG_EDGEDETECTOR_SHOW_TOOL_SHOWEFFECT 0x00000020
#define DLG_EDGEDETECTOR_SHOW_TOOL_RESET      0x00000040

// Flags for L_DlgUnderlay ( ... )
#define DLG_UNDERLAY_AUTOPROCESS           0X0001
#define DLG_UNDERLAY_SHOW_CONTEXTHELP      0X0002
#define DLG_UNDERLAY_SHOW_PREVIEW          0X0004
#define DLG_UNDERLAY_SHOW_TOOL_ZOOMLEVEL   0X0008
#define DLG_UNDERLAY_SHOW_TOOL_SHOWEFFECT  0X0010
#define DLG_UNDERLAY_SHOW_TOOL_ONSCREEN    0X0020
#define DLG_UNDERLAY_SHOW_TOOL_RESET       0X0040

// flags for L_DlgPicturize (...) 
#define DLG_PICTURIZE_AUTOPROCESS      0x00000001 
#define DLG_PICTURIZE_SHOW_CONTEXTHELP 0x00000002 

typedef struct _MOTIONBLURDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;
   L_UINT           uDim ;
   L_INT            nAngle ;
   L_BOOL           bUnidirectional ;
   L_UINT32         uDlgFlags ;
   LPPOINT          pptPosition ;
   LTCOMMDLGHELPCB  pfnHelpCallback ;
   L_VOID *         pHelpCallBackUserData ;

} MOTIONBLURDLGPARAMS, * LPMOTIONBLURDLGPARAMS ;

typedef struct _RADIALBLURDLGPARAMS
{
   L_UINT           uStructSize ;    
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uDim ;
   L_UINT           uStress ;
   POINT            ptCenter ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} RADIALBLURDLGPARAMS, * LPRADIALBLURDLGPARAMS ;

typedef struct _ZOOMBLURDLGPARAMS
{
   L_UINT           uStructSize ;    
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uDim ;
   L_UINT           uStress ;
   POINT            ptCenter ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ; 

} ZOOMBLURDLGPARAMS, * LPZOOMBLURDLGPARAMS ;


typedef struct _GAUSSIANBLURDLGPARAMS
{
   L_UINT           uStructSize ;     
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nRadius ;   
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} GAUSSIANBLURDLGPARAMS, * LPGAUSSIANBLURDLGPARAMS ;

typedef struct _ANTIALIASDLGPARAMS
{
   L_UINT           uStructSize ;      
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nThreshold ; 
   L_UINT           uDim ;       
   L_UINT           uFilter ;    
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} ANTIALIASDLGPARAMS, * LPANTIALIASDLGPARAMS ;

typedef struct _AVERAGEDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uDim ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} AVERAGEDLGPARAMS, * LPAVERAGEDLGPARAMS ;

typedef struct _MEDIANDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uDim ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} MEDIANDLGPARAMS, * LPMEDIANDLGPARAMS ;

typedef struct _ADDNOISEDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uRange ;
   L_UINT           uChannel ;   
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} ADDNOISEDLGPARAMS, * LPADDNOISEDLGPARAMS ;

typedef struct _MAXFILTERDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uDim ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} MAXFILTERDLGPARAMS, * LPMAXFILTERDLGPARAMS ;

typedef struct _MINFILTERDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uDim ;
   L_UINT32         uDlgFlags ;
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ;
   L_VOID *         pHelpCallBackUserData ;
   
} MINFILTERDLGPARAMS, * LPMINFILTERDLGPARAMS ;

typedef struct _SHARPENDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT32          nSharpness ;
   L_UINT32         uDlgFlags ;
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ;
   L_VOID *         pHelpCallBackUserData ;
   
} SHARPENDLGPARAMS, * LPSHARPENDLGPARAMS ;

typedef struct _SHIFTDIFFERENCEFILTERDLGPARAMS
{
   L_UINT           uStructSize ;
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   pSPATIALFLT     pFilter ;
   L_UINT32         uDlgFlags ;
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ;
   L_VOID *         pHelpCallBackUserData ;
   
} SHIFTDIFFERENCEFILTERDLGPARAMS, * LPSHIFTDIFFERENCEFILTERDLGPARAMS ;

typedef struct _EMBOSSDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT          uDepth ;
   L_UINT          uDirection ;   
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} EMBOSSDLGPARAMS, * LPEMBOSSDLGPARAMS ;

typedef struct _OILIFYDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT          uDim ;   
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} OILIFYDLGPARAMS, * LPOILIFYDLGPARAMS ;

typedef struct _MOSAICDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT          uDim ;   
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} MOSAICDLGPARAMS, * LPMOSAICDLGPARAMS ;

typedef struct _EROSIONFILTERDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   pBINARYFLT      pFilter ;
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} EROSIONFILTERDLGPARAMS, * LPEROSIONFILTERDLGPARAMS ;

typedef struct _DILATIONFILTERDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   pBINARYFLT      pFilter ;
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} DILATIONFILTERDLGPARAMS, * LPDILATIONFILTERDLGPARAMS ;

typedef struct _CONTOURFILTERDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT16         nThreshold ;
   L_INT16         nDeltaDirection ;
   L_INT16         nMaxError ;
   L_INT           nOption ;
   L_UINT32        uOptionFlags ;
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} CONTOURFILTERDLGPARAMS, * LPCONTOURFILTERDLGPARAMS ;

typedef struct _GRADIENTFILTERDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   pSPATIALFLT     pFilter ;
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} GRADIENTFILTERDLGPARAMS, * LPGRADIENTFILTERDLGPARAMS ;

typedef struct _LAPLACIANFILTERDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   pSPATIALFLT     pFilter ;
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} LAPLACIANFILTERDLGPARAMS, * LPLAPLACIANFILTERDLGPARAMS ;

typedef struct _SOBELFILTERDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   pSPATIALFLT    pFilter ;
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} SOBELFILTERDLGPARAMS, * LPSOBELFILTERDLGPARAMS ;

typedef struct _PREWITTFILTERDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   pSPATIALFLT    pFilter ;
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *          pHelpCallBackUserData ;
   
} PREWITTFILTERDLGPARAMS, * LPPREWITTFILTERDLGPARAMS ;

typedef struct _LINESEGMENTFILTERDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   pSPATIALFLT    pFilter ;
   L_UINT32        uDlgFlags ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ;
   L_VOID *        pHelpCallBackUserData ;
   
} LINESEGMENTFILTERDLGPARAMS, * LPLINESEGMENTFILTERDLGPARAMS ;

typedef struct _UNSHARPMASKDLGPARAMS
{
   L_UINT           uStructSize ;      
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nAmount ;    
   L_INT            nRadius ;    
   L_INT            nThreshold ; 
   L_UINT           uUnshrpMaskFlags ;     
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} UNSHARPMASKDLGPARAMS, * LPUNSHARPMASKDLGPARAMS ;

typedef struct _MULTIPLYDLGPARAMS
{
   L_UINT           uStructSize ;   
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uFactor ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
 
} MULTIPLYDLGPARAMS, * LPMULTIPLYDLGPARAMS ;

typedef struct _ADDBITMAPSDLGPARAMSA
{
   L_UINT             uStructSize ;                
   pBITMAPHANDLE      pBitmap ;
   L_BOOL             bZoomToFit ;  
   LPDLGBITMAPLISTA   pBitmapList ;
   L_UINT             uAddBitmapsFlags ;
   L_UINT *           puInListIndexes ;
   L_INT32            nInCount ; 
   HBITMAPLIST        hList ;
   L_UINT32           uDlgFlags ; 
   LPPOINT            pptPosition ;   
   LTCOMMDLGHELPCB    pfnHelpCallback ; 
   L_VOID   *         pHelpCallBackUserData ;

} ADDBITMAPSDLGPARAMSA, * LPADDBITMAPSDLGPARAMSA;

typedef struct _STITCHDLGPARAMSA
{
   L_UINT          uStructSize ;       
   pBITMAPHANDLE   pResultingBitmap ;
   L_UINT          uResultingBitmapStructSize ;
   L_INT           nResultingBitmapWidth ;
   L_INT           nResultingBitmapHeight ;
   L_INT           nResultingBitmapBitsPerPixel ;
   L_INT           nRes ;
   COLORREF        crBackGround ;
   LPDLGBITMAPLISTA pBitmapList ;

   // user interface customization
   HICON hWindowIcon ;
   L_INT nCmdShow ;

   // html map creator customization
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} STITCHDLGPARAMSA, * LPSTITCHDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _ADDBITMAPSDLGPARAMS
{
   L_UINT             uStructSize ;                
   pBITMAPHANDLE      pBitmap ;
   L_BOOL             bZoomToFit ;  
   LPDLGBITMAPLIST    pBitmapList ;
   L_UINT             uAddBitmapsFlags ;
   L_UINT *           puInListIndexes ;
   L_INT32            nInCount ; 
   HBITMAPLIST        hList ;
   L_UINT32           uDlgFlags ; 
   LPPOINT            pptPosition ;   
   LTCOMMDLGHELPCB    pfnHelpCallback ; 
   L_VOID   *         pHelpCallBackUserData ;

} ADDBITMAPSDLGPARAMS, * LPADDBITMAPSDLGPARAMS ;

typedef struct _STITCHDLGPARAMS
{
   L_UINT          uStructSize ;       
   pBITMAPHANDLE   pResultingBitmap ;
   L_UINT          uResultingBitmapStructSize ;
   L_INT           nResultingBitmapWidth ;
   L_INT           nResultingBitmapHeight ;
   L_INT           nResultingBitmapBitsPerPixel ;
   L_INT           nRes ;
   COLORREF        crBackGround ;
   LPDLGBITMAPLIST pBitmapList ;

   // user interface customization
   HICON hWindowIcon ;
   L_INT nCmdShow ;

   // html map creator customization
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} STITCHDLGPARAMS, * LPSTITCHDLGPARAMS ;
#else
typedef ADDBITMAPSDLGPARAMSA ADDBITMAPSDLGPARAMS;
typedef LPADDBITMAPSDLGPARAMSA LPADDBITMAPSDLGPARAMS;
typedef STITCHDLGPARAMSA STITCHDLGPARAMS;
typedef LPSTITCHDLGPARAMSA LPSTITCHDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

typedef struct _FREEHANDWAVEDLGPARAMS
{
   L_UINT          uStructSize ;     
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT *         pAmplitudes ;  
   L_UINT          uAmplitudesCount ;  
   L_UINT          uScale ; 
   L_UINT          uWaveLen ;
   L_INT           nAngle ;
   COLORREF        crFill ;
   L_UINT          uFreeHandWaveFlags ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} FREEHANDWAVEDLGPARAMS, * LPFREEHANDWAVEDLGPARAMS ;

typedef struct _WINDDLGPARAMS
{
   L_UINT           uStructSize ;    
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uDim ;     
   L_INT            nAngle ;   
   L_UINT           uOpacity ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} WINDDLGPARAMS, * LPWINDDLGPARAMS ;

typedef struct _POLARDLGPARAMS
{
   L_UINT           uStructSize ;  
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   COLORREF         crFill ; 
   L_UINT           uPolarFlags ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *          pHelpCallBackUserData ;

} POLARDLGPARAMS, * LPPOLARDLGPARAMS ;

typedef struct _ZOOMWAVEDLGPARAMS
{
   L_UINT           uStructSize ;       
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uAmplitude ;
   L_UINT           uFrequency ;
   L_INT            nPhase ;
   L_UINT           uZoomFactor ;
   POINT            ptCenter ;
   COLORREF         crFill ;
   L_UINT           uZoomWaveFlags ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} ZOOMWAVEDLGPARAMS, * LPZOOMWAVEDLGPARAMS ;

typedef struct _RADIALWAVEDLGPARAMS
{
   L_UINT           uStructSize ;       
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uAmplitude ;
   L_UINT           uWaveLen ;
   L_INT            nPhase ;
   POINT            ptCenter ;
   COLORREF         crFill ;
   L_UINT           uRadialWaveFlags ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} RADIALWAVEDLGPARAMS, * LPRADIALWAVEDLGPARAMS ;

typedef struct _SWIRLDLGPARAMS
{
   L_UINT           uStructSize ;    
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nAngle ;   
   POINT            ptCenter ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} SWIRLDLGPARAMS, * LPSWIRLDLGPARAMS ;

typedef struct _WAVEDLGPARAMS
{
   L_UINT          uStructSize ;       
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT          uAmplitude ;
   L_UINT          uWaveLen ;
   L_INT           nAngle ;
   L_UINT          uHorzScale ;
   L_UINT          uVertScale ;
   COLORREF        crFill ;
   L_UINT          uWaveFlags ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} WAVEDLGPARAMS, * LPWAVEDLGPARAMS ;

typedef struct _WAVESHEARDLGPARAMS
{
   L_UINT           uStructSize ;     
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT *          pAmplitudes ;  
   L_UINT           uAmplitudesCount ;  
   L_UINT           uScale ; 
   COLORREF         crFill ;
   L_UINT           uWaveShearFlags ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} WAVESHEARDLGPARAMS, * LPWAVESHEARDLGPARAMS ;

typedef struct _PUNCHDLGPARAMS
{
   L_UINT           uStructSize ;       
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uStress ;
   L_INT            nValue ;
   POINT            ptCenter ;
   COLORREF         crFill ;
   L_UINT           uPunchFlags ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} PUNCHDLGPARAMS, * LPPUNCHDLGPARAMS ;

typedef struct _RIPPLEDLGPARAMS
{
   L_UINT           uStructSize ;       
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uAmplitude ;
   L_UINT           uFrequency ;
   L_INT            nPhase ;
   L_UINT           uAttenuation ;
   POINT            ptCenter ;
   COLORREF         crFill ;
   L_UINT           uRippleFlag ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} RIPPLEDLGPARAMS, * LPRIPPLEDLGPARAMS ;

typedef struct _BENDINGDLGPARAMS
{
   L_UINT           uStructSize ;       
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nValue ;
   POINT            ptCenter ;
   COLORREF         crFill ;
   L_UINT           uBendingFlags ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} BENDINGDLGPARAMS, * LPBENDINGDLGPARAMS ;

typedef struct _CYLINDRICALDLGPARAMS
{
   L_UINT          uStructSize ;  
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_INT           nValue ; 
   L_UINT          uType ;  
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} CYLINDRICALDLGPARAMS, * LPCYLINDRICALDLGPARAMS ;

typedef struct _SPHERIZEDLGPARAMS
{
   L_UINT           uStructSize ;       
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nValue ;
   POINT            ptCenter ;
   COLORREF         crFill ;
   L_UINT           uSpherizeFlags ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} SPHERIZEDLGPARAMS, * LPSPHERIZEDLGPARAMS ;

typedef struct _IMPRESSIONISTDLGPARAMS
{
   L_UINT           uStructSize ;    
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uHorzDim ; 
   L_UINT           uVertDim ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
                    
} IMPRESSIONISTDLGPARAMS, * LPIMPRESSIONISTDLGPARAMS ;

typedef struct _PIXELATEDLGPARAMS
{
   L_UINT           uStructSize ;       
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_UINT           uCellWidth ;   
   L_UINT           uCellHeight ; 
   L_UINT           uOpacity ; 
   POINT            ptCenter ;
   L_UINT           uPixelateFlags ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} PIXELATEDLGPARAMS, * LPPIXELATEDLGPARAMS ;

typedef struct _EDGEDETECTORDLGPARAMS
{
   L_UINT           uStructSize ;      
   pBITMAPHANDLE    pBitmap ;
   L_BOOL           bZoomToFit ;  
   L_INT            nThreshold ; 
   L_UINT           uFilter ;    
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;
   
} EDGEDETECTORDLGPARAMS, * LPEDGEDETECTORDLGPARAMS ;

typedef struct _UNDERLAYDLGPARAMSA
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT32        uUnderlayFlags ;
   L_INT           nUnderlayBitmapIndex ;
   LPDLGBITMAPLISTA pBitmapList ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} UNDERLAYDLGPARAMSA, * LPUNDERLAYDLGPARAMSA ;

typedef struct _PICTURIZEDLGPARAMSA
{
   L_UINT           uStructSize ;        
   pBITMAPHANDLE    pBitmap ;
   L_CHAR *         pszPath ;   
   L_INT            nCellWidth ;
   L_INT            nCellHeight ;
   L_UINT           uResize ;     
   L_INT            nBitmapWidth ;
   L_INT            nBitmapHeight ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} PICTURIZEDLGPARAMSA, * LPPICTURIZEDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _UNDERLAYDLGPARAMS
{
   L_UINT          uStructSize ;
   pBITMAPHANDLE   pBitmap ;
   L_BOOL          bZoomToFit ;  
   L_UINT32        uUnderlayFlags ;
   L_INT           nUnderlayBitmapIndex ;
   LPDLGBITMAPLIST pBitmapList ;
   L_UINT32        uDlgFlags ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} UNDERLAYDLGPARAMS, * LPUNDERLAYDLGPARAMS ;

typedef struct _PICTURIZEDLGPARAMS
{
   L_UINT           uStructSize ;        
   pBITMAPHANDLE    pBitmap ;
   L_TCHAR *        pszPath ;   
   L_INT            nCellWidth ;
   L_INT            nCellHeight ;
   L_UINT           uResize ;     
   L_INT            nBitmapWidth ;
   L_INT            nBitmapHeight ;
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} PICTURIZEDLGPARAMS, * LPPICTURIZEDLGPARAMS ;
#else
typedef PICTURIZEDLGPARAMSA PICTURIZEDLGPARAMS;
typedef LPPICTURIZEDLGPARAMSA LPPICTURIZEDLGPARAMS;
typedef UNDERLAYDLGPARAMSA UNDERLAYDLGPARAMS;
typedef LPUNDERLAYDLGPARAMSA LPUNDERLAYDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

//.............................................................................
// enums, defines and structures
//.............................................................................


//.............................................................................
// Functions
//.............................................................................

L_LTDLG_API L_INT EXT_FUNCTION L_DlgMotionBlur ( HWND hWndOwner,          
                                     LPMOTIONBLURDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgRadialBlur ( HWND hWndOwner,
                                     LPRADIALBLURDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgZoomBlur ( HWND hWndOwner,
                                   LPZOOMBLURDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGaussianBlur ( HWND hWndOwner,
                                       LPGAUSSIANBLURDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgAntiAlias ( HWND hWndOwner,          
                                    LPANTIALIASDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgAverage ( HWND hWndOwner,          
                                  LPAVERAGEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgMedian ( HWND hWndOwner,          
                                 LPMEDIANDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgAddNoise ( HWND hWndOwner,          
                                   LPADDNOISEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgMaxFilter ( HWND hWndOwner,          
                                    LPMAXFILTERDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgMinFilter ( HWND hWndOwner,          
                                    LPMINFILTERDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgSharpen ( HWND hWndOwner,          
                                  LPSHARPENDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgShiftDifferenceFilter ( HWND hWndOwner,
                                                LPSHIFTDIFFERENCEFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgEmboss ( HWND hWndOwner,
                                 LPEMBOSSDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgOilify ( HWND hWndOwner,
                                 LPOILIFYDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgMosaic ( HWND hWndOwner,
                                 LPMOSAICDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgErosionFilter ( HWND hWndOwner,
                                        LPEROSIONFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgDilationFilter ( HWND hWndOwner,
                                         LPDILATIONFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgContourFilter ( HWND hWndOwner,
                                        LPCONTOURFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGradientFilter ( HWND hWndOwner,
                                         LPGRADIENTFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgLaplacianFilter ( HWND hWndOwner,
                                          LPLAPLACIANFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgSobelFilter ( HWND hWndOwner,
                                      LPSOBELFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPrewittFilter ( HWND hWndOwner,
                                        LPPREWITTFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgLineSegmentFilter ( HWND hWndOwner,
                                            LPLINESEGMENTFILTERDLGPARAMS  pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgUnsharpMask ( HWND hWndOwner,          
                                      LPUNSHARPMASKDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgMultiply ( HWND hWndOwner,
                                   LPMULTIPLYDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgAddBitmapsA ( HWND hWndOwner,
                                     LPADDBITMAPSDLGPARAMSA pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgStitchA ( HWND hWndOwner,
                                 LPSTITCHDLGPARAMSA pDlgParams ) ;

#if defined(FOR_UNICODE)
L_LTDLG_API L_INT EXT_FUNCTION L_DlgAddBitmaps ( HWND hWndOwner,
                                     LPADDBITMAPSDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgStitch ( HWND hWndOwner,
                                 LPSTITCHDLGPARAMS pDlgParams ) ;
#else
#define L_DlgAddBitmaps L_DlgAddBitmapsA
#define L_DlgStitch L_DlgStitchA
#endif // #if defined(FOR_UNICODE)

L_LTDLG_API L_INT EXT_FUNCTION L_DlgFreeHandWave ( HWND hWndOwner,
                                       LPFREEHANDWAVEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgWind ( HWND hWndOwner,          
                               LPWINDDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPolar ( HWND hWndOwner,          
                                LPPOLARDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgZoomWave ( HWND hWndOwner,          
                                   LPZOOMWAVEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgRadialWave ( HWND hWndOwner,
                                     LPRADIALWAVEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgSwirl ( HWND hWndOwner,
                                LPSWIRLDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgWave ( HWND hWndOwner,
                               LPWAVEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgWaveShear ( HWND hWndOwner,
                                    LPWAVESHEARDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPunch ( HWND hWndOwner,
                                LPPUNCHDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgRipple ( HWND hWndOwner,
                                 LPRIPPLEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgBending ( HWND hWndOwner,
                                  LPBENDINGDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgCylindrical ( HWND hWndOwner,
                                      LPCYLINDRICALDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgSpherize ( HWND hWndOwner,
                                   LPSPHERIZEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgImpressionist ( HWND hWndOwner,
                                        LPIMPRESSIONISTDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPixelate ( HWND hWndOwner,
                                   LPPIXELATEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgEdgeDetector ( HWND hWndOwner,          
                                       LPEDGEDETECTORDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgUnderlayA ( HWND hWndOwner, 
                                   LPUNDERLAYDLGPARAMSA pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPicturizeA ( HWND hWndOwner, 
                                   LPPICTURIZEDLGPARAMSA pDlgParams ) ;
#if defined(FOR_UNICODE)
L_LTDLG_API L_INT EXT_FUNCTION L_DlgUnderlay ( HWND hWndOwner, 
                                   LPUNDERLAYDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPicturize ( HWND hWndOwner, 
                                   LPPICTURIZEDLGPARAMS pDlgParams ) ;
#else
#define L_DlgPicturize L_DlgPicturizeA
#define L_DlgUnderlay  L_DlgUnderlayA
#endif // #if defined(FOR_UNICODE)

//.............................................................................
// Functions
//.............................................................................

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDLG_IMGEFX_H)
