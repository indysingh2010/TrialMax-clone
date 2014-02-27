/*************************************************************
   Ltimg.h - LEADTOOLS image processing library
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTIMGCOR_H)
#define LTIMGCOR_H

#if !defined(L_LTIMGCOR_API)
   #define L_LTIMGCOR_API
#endif // #if !defined(L_LTIMGCOR_API)

#include "Ltkrn.h"
#include "Lttyp.h"
#include "Ltdis.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

/****************************************************************
   Enums/defines/macros/structures
****************************************************************/
//----------------L_GetHistogram, L_RemapBitmapIntensity, and L_AddNoiseBitmap Flags-----------------------
#define CHANNEL_MASTER  0x0000   // All channels
#define CHANNEL_RED     0x0001   // Red channel only
#define CHANNEL_GREEN   0x0002   // Green channel only
#define CHANNEL_BLUE    0x0003   // Blue channel only

//----------------L_HalfToneBitmap Flags-----------------------
#define HT_PRINT   0x0000
#define HT_VIEW    0x0001
#define HT_RECT    0x0002
#define HT_CIRC    0x0003
#define HT_ELLIPS  0x0004
#define HT_RAND    0x0005
#define HT_LINEAR  0x0006
#define HT_USERDEF 0x0007

//---------------- Fourier Transform Flags-----------------------
#define FFT_FFT         0x00000001
#define FFT_IFFT        0x00000002
#define FFT_BLUE        0x00000010
#define FFT_GREEN       0x00000020
#define FFT_RED         0x00000030
#define FFT_GRAY        0x00000040
#define FFT_IFFT_MAG    0x00000100
#define FFT_IFFT_PHS    0x00000200
#define FFT_IFFT_BOTH   0x00000300
#define FFT_IFFT_CLIP   0x00001000
#define FFT_IFFT_SCL    0x00002000

#define DFT_DFT         0x00000001
#define DFT_IDFT        0x00000002
#define DFT_BLUE        0x00000010
#define DFT_GREEN       0x00000020
#define DFT_RED         0x00000030
#define DFT_GRAY        0x00000040
#define DFT_IDFT_MAG    0x00000100
#define DFT_IDFT_PHS    0x00000200
#define DFT_IDFT_BOTH   0x00000300
#define DFT_IDFT_CLIP   0x00001000
#define DFT_IDFT_SCL    0x00002000
#define DFT_ALL         0x00010000
#define DFT_RANGE       0x00020000
#define DFT_INSIDE_X    0x00100000
#define DFT_OUTSIDE_X   0x00200000
#define DFT_INSIDE_Y    0x01000000
#define DFT_OUTSIDE_Y   0x02000000

#define FRQ_INSIDE_X    0x00000001
#define FRQ_OUTSIDE_X   0x00000002
#define FRQ_INSIDE_Y    0x00000010
#define FRQ_OUTSIDE_Y   0x00000020

#define DSP_FT_MAG      0x00000001
#define DSP_FT_PHS      0x00000002
#define DSP_FT_NORM     0x00000010
#define DSP_FT_LOG      0x00000020

//--------------L_AutoColorLevelBitmap Flags---------------------
#define AUTO_LEVEL      0x00000001
#define AUTO_CONTRAST   0x00000002
#define AUTO_INTENSITY  0x00000003
#define AUTO_NOPROCESS  0x00000004

//-------------- Constants and Flags for Doc Imaging functions ---------------------
#define SMOOTH_BUMP  0x0000
#define SMOOTH_NICK  0x0001
#define SMOOTH_NONE  0x0002

#define SMOOTH_HORIZONTAL_ELEMENT   0x0000
#define SMOOTH_VERTICAL_ELEMENT     0x0001

#define SUCCESS_REMOVE              0x0001
#define SUCCESS_NOREMOVE            0x0002
#define LINEREMOVE_HORIZONTAL       0x0001
#define LINEREMOVE_VERTICAL         0x0002
#define SUCCESS_INVERT              0x0001
#define SUCCESS_NOINVERT            0x0002

#define BORDER_LEFT        0x0001
#define BORDER_RIGHT       0x0002
#define BORDER_TOP         0x0004
#define BORDER_BOTTOM      0x0008
#define BORDER_ALL         (BORDER_LEFT | BORDER_RIGHT | BORDER_TOP | BORDER_BOTTOM)
#define BORDER_DELTA_MAX   (0xffffffff)

//-------------- flags for iLocation ---------------------
#define HOLEPUNCH_LEFT     0x0001
#define HOLEPUNCH_RIGHT    0x0002
#define HOLEPUNCH_TOP      0x0003
#define HOLEPUNCH_BOTTOM   0x0004

#define STAPLE_TOPLEFT     0x0001
#define STAPLE_TOPRIGHT    0x0002
#define STAPLE_BOTTOMLEFT  0x0003
#define STAPLE_BOTTOMRIGHT 0x0004

#define FLAG_USE_DPI          0x00000001
#define FLAG_SINGLE_REGION    0x00000002
#define FLAG_LEAD_REGION      0x00000004
#define FLAG_CALLBACK_REGION  0x00000008
#define FLAG_IMAGE_UNCHANGED  0x00000010
#define FLAG_USE_SIZE         0x00000020
#define FLAG_USE_COUNT        0x00000040
#define FLAG_USE_LOCATION     0x00000080
#define FLAG_FAVOR_LONG       0x00000100
#define FLAG_REMOVE_ENTIRE    0x00000200
#define FLAG_USE_GAP          0x00000400
#define FLAG_USE_VARIANCE     0x00000800
#define FLAG_USE_DIAGONALS    0x00001000

//--------------L_DeskewBitmap Flags---------------------
#define DSKW_PROCESS    0x00000000
#define DSKW_NOPROCESS  0x00000001

#define DSKW_FILL    0x00000000
#define DSKW_NOFILL  0x00000010

#define DSKW_NOTHRESHOLD   0x00000000
#define DSKW_THRESHOLD     0x00000100

#define DSKW_LINEAR     0x00000000
#define DSKW_RESAMPLE   0x00001000
#define DSKW_BICUBIC    0x00002000

#define DSKW_DOCUMENTIMAGE       0x00000000
#define DSKW_DOCUMENTANDPICTURE  0x00010000

#define DSKW_NORMALSPEEDROTATE  0x00000000
#define DSKW_HIGHSPEEDROTATE    0x00100000

#define DSKW_DONT_USE_CHECK_DESKEW           0x00000000
#define DSKW_USE_CHECK_DESKEW                0x01000000
#define DSKW_USE_CHECK_DESKEW_DETECT_LINES   0x02000000


#define CHECK_DESKEW_NOT_USED       0x00000000
#define CHECK_DESKEW_NORMAL         0x01000000
#define CHECK_DESKEW_DETECT_LINES   0x02000000

#if defined (LEADTOOLS_V16_OR_LATER)
#define DSKW_PERFORM_PREPROCESSING         0x00000000
#define DSKW_DONT_PERFORM_PREPROCESSING    0x10000000

#define DSKW_SELECTIVE_DETECTION     0x00000000
#define DSKW_NORMAL_DETECTION      0x20000000
#endif //LEADTOOLS_V16_OR_LATER

//--------------L_SmoothBitmap Flags---------------------
#define SMOOTH_SINGLE_REGION     FLAG_SINGLE_REGION
#define SMOOTH_LEAD_REGION       FLAG_LEAD_REGION
#define SMOOTH_IMAGE_UNCHANGED   FLAG_IMAGE_UNCHANGED
#define SMOOTH_FAVOR_LONG        FLAG_FAVOR_LONG
#define SMOOTH_ALLFLAGS          (                             \
                                    SMOOTH_SINGLE_REGION    |  \
                                    SMOOTH_LEAD_REGION      |  \
                                    SMOOTH_IMAGE_UNCHANGED  |  \
                                    SMOOTH_FAVOR_LONG          \
                                 )

//--------------L_LineRemoveBitmap Flags---------------------
#define LINE_USE_DPI          FLAG_USE_DPI
#define LINE_SINGLE_REGION    FLAG_SINGLE_REGION
#define LINE_LEAD_REGION      FLAG_LEAD_REGION
#define LINE_CALLBACK_REGION  FLAG_CALLBACK_REGION
#define LINE_IMAGE_UNCHANGED  FLAG_IMAGE_UNCHANGED
#define LINE_REMOVE_ENTIRE    FLAG_REMOVE_ENTIRE
#define LINE_USE_GAP          FLAG_USE_GAP
#define LINE_USE_VARIANCE     FLAG_USE_VARIANCE
#define LINE_ALLFLAGS         (                          \
                                 LINE_USE_DPI         |  \
                                 LINE_SINGLE_REGION   |  \
                                 LINE_LEAD_REGION     |  \
                                 LINE_CALLBACK_REGION |  \
                                 LINE_IMAGE_UNCHANGED |  \
                                 LINE_REMOVE_ENTIRE   |  \
                                 LINE_USE_GAP         |  \
                                 LINE_USE_VARIANCE       \
                              )

//--------------L_BorderRemoveBitmap Flags---------------------
#define BORDER_LEAD_REGION       FLAG_LEAD_REGION
#define BORDER_CALLBACK_REGION   FLAG_CALLBACK_REGION
#define BORDER_SINGLE_REGION     FLAG_SINGLE_REGION
#define BORDER_IMAGE_UNCHANGED   FLAG_IMAGE_UNCHANGED
#define BORDER_USE_VARIANCE      FLAG_USE_VARIANCE
#define BORDER_ALLFLAGS          (                             \
                                    BORDER_LEAD_REGION      |  \
                                    BORDER_CALLBACK_REGION  |  \
                                    BORDER_SINGLE_REGION    |  \
                                    BORDER_IMAGE_UNCHANGED  |  \
                                    BORDER_USE_VARIANCE        \
                                 )

//--------------L_InvertedTextBitmap Flags---------------------
#define INVERTEDTEXT_USE_DPI           FLAG_USE_DPI
#define INVERTEDTEXT_SINGLE_REGION     FLAG_SINGLE_REGION
#define INVERTEDTEXT_LEAD_REGION       FLAG_LEAD_REGION
#define INVERTEDTEXT_CALLBACK_REGION   FLAG_CALLBACK_REGION
#define INVERTEDTEXT_IMAGE_UNCHANGED   FLAG_IMAGE_UNCHANGED
#define INVERTEDTEXT_USE_DIAGONALS     FLAG_USE_DIAGONALS
#define INVERTEDTEXT_ALLFLAGS          (                                   \
                                          INVERTEDTEXT_USE_DPI          |  \
                                          INVERTEDTEXT_SINGLE_REGION    |  \
                                          INVERTEDTEXT_LEAD_REGION      |  \
                                          INVERTEDTEXT_CALLBACK_REGION  |  \
                                          INVERTEDTEXT_IMAGE_UNCHANGED  |  \
                                          INVERTEDTEXT_USE_DIAGONALS       \
                                       )

//--------------L_DotRemoveBitmap Flags---------------------
#define DOT_USE_DPI           FLAG_USE_DPI
#define DOT_SINGLE_REGION     FLAG_SINGLE_REGION
#define DOT_LEAD_REGION       FLAG_LEAD_REGION
#define DOT_CALLBACK_REGION   FLAG_CALLBACK_REGION
#define DOT_IMAGE_UNCHANGED   FLAG_IMAGE_UNCHANGED
#define DOT_USE_SIZE          FLAG_USE_SIZE
#define DOT_USE_DIAGONALS     FLAG_USE_DIAGONALS
#define DOT_ALLFLAGS          (                          \
                                 DOT_USE_DPI          |  \
                                 DOT_SINGLE_REGION    |  \
                                 DOT_LEAD_REGION      |  \
                                 DOT_CALLBACK_REGION  |  \
                                 DOT_IMAGE_UNCHANGED  |  \
                                 DOT_USE_SIZE         |  \
                                 DOT_USE_DIAGONALS       \
                              )

//--------------L_HolePunchRemoveBitmap Flags---------------------
#define HOLEPUNCH_USE_DPI           FLAG_USE_DPI
#define HOLEPUNCH_SINGLE_REGION     FLAG_SINGLE_REGION
#define HOLEPUNCH_LEAD_REGION       FLAG_LEAD_REGION
#define HOLEPUNCH_CALLBACK_REGION   FLAG_CALLBACK_REGION
#define HOLEPUNCH_IMAGE_UNCHANGED   FLAG_IMAGE_UNCHANGED
#define HOLEPUNCH_USE_SIZE          FLAG_USE_SIZE
#define HOLEPUNCH_USE_COUNT         FLAG_USE_COUNT
#define HOLEPUNCH_USE_LOCATION      FLAG_USE_LOCATION
#define HOLEPUNCH_ALLFLAGS          (                                \
                                       HOLEPUNCH_USE_DPI          |  \
                                       HOLEPUNCH_SINGLE_REGION    |  \
                                       HOLEPUNCH_LEAD_REGION      |  \
                                       HOLEPUNCH_CALLBACK_REGION  |  \
                                       HOLEPUNCH_IMAGE_UNCHANGED  |  \
                                       HOLEPUNCH_USE_SIZE         |  \
                                       HOLEPUNCH_USE_COUNT        |  \
                                       HOLEPUNCH_USE_LOCATION        \
                                    )

//--------------L_StapleRemoveBitmap Flags---------------------
#define STAPLE_USE_DPI           FLAG_USE_DPI
#define STAPLE_SINGLE_REGION     FLAG_SINGLE_REGION
#define STAPLE_LEAD_REGION       FLAG_LEAD_REGION
#define STAPLE_CALLBACK_REGION   FLAG_CALLBACK_REGION
#define STAPLE_IMAGE_UNCHANGED   FLAG_IMAGE_UNCHANGED
#define STAPLE_USE_SIZE          FLAG_USE_SIZE
#define STAPLE_USE_LOCATION      FLAG_USE_LOCATION
#define STAPLE_ALLFLAGS          (                             \
                                    STAPLE_USE_DPI          |  \
                                    STAPLE_SINGLE_REGION    |  \
                                    STAPLE_LEAD_REGION      |  \
                                    STAPLE_CALLBACK_REGION  |  \
                                    STAPLE_IMAGE_UNCHANGED  |  \
                                    STAPLE_USE_SIZE         |  \
                                    STAPLE_USE_LOCATION        \
                                 )

//--------------L_ConvertBitmapSignedToUnsigned Flags---------------------
#define SHIFT_ZERO_TO_CENTER        0x0000
#define SHIFT_MIN_TO_ZERO           0x0001
#define SHIFT_NEG_TO_ZERO           0x0002
#define SHIFT_RANGE_ONLY            0x0003
#define SHIFT_RANGE_PROCESS_OUTSIDE 0x0004

//--------------L_ApplyLUTBitmap Flags---------------------
#define LUT_SIGNED   0x0001   // The LUT entries are signed

//--------------L_SubtractBackgroundBitmap Flags---------------------
#define SBK_DEPEND      0
#define SBK_1_1         1
#define SBK_1_2         2
#define SBK_1_4         3
#define SBK_1_8         4

#define SBK_BG_DARK     0x00000000
#define SBK_BG_BRIGHT   0x00000001

#define SBK_RES_SHOW    0x00000000
#define SBK_BG_SHOW     0x00000010

//-------------- L_ApplyModalityLUT, L_ApplyLinearModalityLUT flags-----------------------------
#define M_LUT_SIGNED                0x0001   // The LUT entries are signed
#define M_LUT_UPDATE_MIN_MAX        0x0002   // Update MinVal,MaxVal inside the bitmap handle
#define M_LUT_USE_FULL_RANGE        0x0004   // Do not mask the values in the LUT 
#define M_LUT_ALLOW_RANGE_EXPANSION 0x0008   // Allow the function to increase pBitmap->HighBit
                                             // (if needed) to be able to hold the data range after
                                             // applying modality LUT

//-------------- L_ApplyVOILUT,L_ApplyLinearVOILUT flags-----------------------------
#define VOI_LUT_UPDATE_MIN_MAX   0x0001   // Update MinVal,MaxVal inside the bitmap handle
#define VOI_LUT_REVERSE_ORDER    0x0002   // Reverse ordered grayscale (light to dark)

//-------------- L_CountLUTColors flags-----------------------------
#define COUNT_LUT_UNSIGNED 0x0001
#define COUNT_LUT_SIGNED   0x0002

//---------------- L_IntensityDetectBitmap Flags-----------------------
#define IDB_CHANNEL_MASTER 0x0000
#define IDB_CHANNEL_RED    0x0001
#define IDB_CHANNEL_GREEN  0x0010
#define IDB_CHANNEL_BLUE   0x0100

//---------------- L_MultiScaleEnhancementBitmap Flags-----------------------
#define MSE_GAUSSIAN 0x0000
#define MSE_RESAMPLE 0x0001
#define MSE_BICUBIC  0x0002
#define MSE_NORMAL   0x0003

#define MSE_EDGEENH  0x0010
#define MSE_LATRED   0x0020

#define MSE_DEFAULT  (L_UINT)-1

//---------------- L_DigitalSubtractBitmap Flags-----------------------
#define DS_CONTRASTENH     0x0001
#define DS_OPTIMIZERANGE   0x0002

//---------------- L_SearchRegMarksBitmap & L_IsRegMarkBitmap Flags-----------------------
#define RGS_T  0x0000

//---------------- L_ApplyTransformationParameters Flags-----------------------
#define RGS_SIZE_NORMAL       0x0001
#define RGS_SIZE_RESAMPLE     0x0002
#define RGS_SIZE_BICUBIC      0x0003
#define RGS_SIZE_FAVORBLACK   0x0010
#define RGS_SIZE_FAVORWHITE   0x0020

//---------------- L_ConvertBitmapUnsignedToSigned Flags-----------------------
#define PROCESS_RANGE_ONLY    0x0001
#define PROCESS_OUTSIDE_RANGE 0x0002

//---------------- L_HalftonePattern   -----------------------
#define HTPATTERN_DOT      0x0001
#define HTPATTERN_LINE     0x0002
#define HTPATTERN_CIRCLE   0x0003
#define HTPATTERN_ELLIPSE  0x0004

//----------------- L_SliceBitmap Structure -----------------
#define SLC_DESKEW         0x0000
#define SLC_WITHOUTDESKEW  0x0001

#define SLC_DSKW_LINEAR    0x00000000
#define SLC_DSKW_RESAMPLE  0x00000010 
#define SLC_DSKW_BICUBIC   0x00000020

#define SLC_WITHOUTCUT     0x0000
#define SLC_CUTSLICES      0x0100

//----------------L_SizeBitmapInterpolate Flags-----------------------

#define SIZE_TRIANGLE                 0x0005
#define SIZE_HERMITE                  0x0006
#define SIZE_BELL                     0x0007
#define SIZE_QUADRATIC_B_SPLINE       0x0008
#define SIZE_CUBIC_B_SPLINE           0x0009
#define SIZE_BOXFILTER                0x000A
#define SIZE_LANCZOS                  0x000B
#define SIZE_MICHELL                  0x000C
#define SIZE_COSINE                   0x000D
#define SIZE_CATROM                   0x000E
#define SIZE_QUADRATIC                0x000F
#define SIZE_CUBIC_CONVOLUTION        0x0010
#define SIZE_BILINEAR                 0x0011
#define SIZE_BRESENHAM                0x0012

//--------------L_BlankPageDetectorBitmap---------------------

#define BLANK_DETECT_EMPTY               0x00000000
#define BLANK_DETECT_NOISY               0x00000001

#define BLANK_NOT_BLEED_THROUGH          0x00000000
#define BLANK_BLEED_THROUGH              0x00000010

#define BLANK_DONT_DETECT_LINES          0x00000000
#define BLANK_DETECT_LINES               0x00000100

#define BLANK_DONT_USE_ACTIVE_AREA       0x00000000
#define BLANK_USE_ACTIVE_AREA            0x00001000

#define BLANK_DEFAULT_MARGIN             0x00000000
#define BLANK_USER_MARGIN                0x00010000

//--------------L_AutoBinarizeBitmap---------------------

#define AUTO_BINARIZE_PRE_AUTO                0x00000000
#define AUTO_BINARIZE_NO_PRE                  0x00000001
#define AUTO_BINARIZE_PRE_BG_ELIMINATION      0x00000002
#define AUTO_BINARIZE_PRE_LEVELING            0x00000004

#define AUTO_BINARIZE_THRESHOLD_AUTO          0x00000000
#define AUTO_BINARIZE_THRESHOLD_USER          0x00000010
#define AUTO_BINARIZE_THRESHOLD_PERCENTILE    0x00000020
#define AUTO_BINARIZE_THRESHOLD_MEDIAN        0x00000040

//--------------L_AutoZoneBitmap---------------------

#define AUTOZONE_DETECT_TEXT                        0x0001
#define AUTOZONE_DETECT_GRAPHIC                     0x0002
#define AUTOZONE_DETECT_TABLE                       0x0004
#define AUTOZONE_DETECT_ALL                         0x0007

#define AUTOZONE_DONT_ALLOW_OVERLAP                 0x0000
#define AUTOZONE_ALLOW_OVERLAP                      0x0010
#define AUTOZONE_ACCURATE_ZONES                     0x0000
#define AUTOZONE_GENERAL_ZONES                      0x0100

#define AUTOZONE_DONT_RECOGNIZE_ONE_CELL_TABLE      0x0000
#define AUTOZONE_RECOGNIZE_ONE_CELL_TABLE           0x1000

#define AUTOZONE_USE_MULTITHREADING                 0x00000000
#define AUTOZONE_DONTUSE_MULTITHREADING             0x80000000

#define LEAD_ZONE_TYPE_TEXT      0
#define LEAD_ZONE_TYPE_GRAPHIC   1
#define LEAD_ZONE_TYPE_TABLE     2

//--------------L_InvertedPageBitmap-------------------
#define INVERTEDPAGE_PROCESS    0x00000000
#define INVERTEDPAGE_NOPROCESS  0x00000001

//--------------L_HighQualityRotateBitmap--------------
#define HIGHQUALITYROTATE_CROP        0x0000
#define HIGHQUALITYROTATE_RESIZE      0x0001

#define HIGHQUALITYROTATE_HIGH        0x0000
#define HIGHQUALITYROTATE_BEST        0x0010

//--------------TISSUE EQUALIZE FLAGS--------------
#define TISSUEEQUALIZE_SIMPLIFY        0x00000001
#define TISSUEEQUALIZE_INTENSIFY       0x00000002


///****************************************************************
//   Classes/structures
//****************************************************************/

//****************** L_AutoZoneBitmap Structure*******************



typedef struct _RAKEREMOVE{
   L_INT nMinLength;
   L_INT nMaxWidth;
   L_INT nMinWallHeight;
   L_INT nMaxWallPercent;
   L_INT nMaxSideteethLength;
   L_INT nMaxMidteethLength;
   L_INT nTeethSpacing;
   L_INT nGaps;
   L_INT nVariance;
}RAKEREMOVE, * pRAKEREMOVE;

typedef L_INT (EXT_CALLBACK RAKEREMOVECALLBACK)(
   L_HRGN      hRgn,
   L_INT       nLength,
   L_VOID      *pUserData);

typedef L_INT (EXT_CALLBACK OBJECTCOUNTERCALLBACK)(
   L_RECT      rcRect,
   L_INT      **Object,
   L_VOID      *pUserData);

typedef struct _OBJECTINFO
{
   L_RECT     rcRect;
   L_INT      **pObject;

}OBJECTINFO, *pOBJECTINFO;

typedef HANDLE MAGICWANDHANDLE;
#define pMAGICWANDHANDLE MAGICWANDHANDLE*;

typedef L_INT (pEXT_CALLBACK AUTOZONECALLBACK)(
   PRECT    pRect,
   L_UINT   uType,
   L_VOID   *pUserData);

typedef struct _LEADZONE
{
   L_UINT uStructSize;
   L_UINT uIndex;
   L_UINT uZoneType;
   RECT   rcLocation;
   L_VOID* pZoneData;
} LEADZONE, *pLEADZONE;

typedef struct _TABLEZONE
{
   PRECT       pCells;
   L_UINT       Rows;
   L_UINT       Columns;
}TABLEZONE, *pTABLEZONE;

typedef struct _TEXTZONE
{
   PRECT       pTextLines;
   L_UINT      uTextLinesCount;
}TEXTZONE, *pTEXTZONE;


//********************** FT functions Structure**********************

typedef struct _COMPLEX
{
   L_DOUBLE r;
   L_DOUBLE i;
}
L_COMPLEX, *pCOMPLEX;

typedef struct _FTARRAY
{
   L_UINT uStructSize;
   L_UINT uWidth;
   L_UINT uHeight;
   L_COMPLEX acxData[1];
}
FTARRAY, *pFTARRAY;

//********************** L_SmoothBitmap Structure**********************
typedef struct _SMOOTH
{
   L_UINT         uStructSize;
   L_UINT         uFlags;
   L_INT          iLength;
   pBITMAPHANDLE  pBitmapRegion;
   L_UINT         uBitmapStructSize;
   HRGN           hRgn;
}
SMOOTH, *pSMOOTH;

//********************** L_HolePunchRemoveBitmap Structure**********************
typedef struct _HOLEPUNCH
{
   L_UINT         uStructSize;
   L_UINT         uFlags;
   L_INT          iMinHoleCount;
   L_INT          iMaxHoleCount;
   L_INT          iMinHoleWidth;
   L_INT          iMinHoleHeight;
   L_INT          iMaxHoleWidth;
   L_INT          iMaxHoleHeight;
   L_INT          iLocation;
   pBITMAPHANDLE  pBitmapRegion;
   L_UINT         uBitmapStructSize;
   HRGN           hRgn;
}
HOLEPUNCH, *pHOLEPUNCH;

//********************** L_StapleRemoveBitmap Structure**********************
typedef struct _STAPLE
{
   L_UINT         uStructSize;
   L_UINT         uFlags;
   L_INT          iMinLength;
   L_INT          iMaxLength;
   L_INT          iLocation;
   pBITMAPHANDLE  pBitmapRegion;
   L_UINT         uBitmapStructSize;
   HRGN           hRgn;
}
STAPLE, *pSTAPLE;

//********************** L_DotRemoveBitmap Structure**********************
typedef struct _DOTREMOVE
{
   L_UINT         uStructSize;
   L_UINT         uFlags;
   L_INT          iMinDotWidth;
   L_INT          iMinDotHeight;
   L_INT          iMaxDotWidth;
   L_INT          iMaxDotHeight;
   pBITMAPHANDLE  pBitmapRegion;
   L_UINT         uBitmapStructSize;
   HRGN           hRgn;
}
DOTREMOVE, *pDOTREMOVE;

//********************** L_InvertedTextBitmap Structure**********************
typedef struct _INVERTEDTEXT
{
   L_UINT         uStructSize;
   L_UINT         uFlags;
   L_INT          iMinInvertWidth;
   L_INT          iMinInvertHeight;
   L_INT          iMinBlackPercent;
   L_INT          iMaxBlackPercent;
   pBITMAPHANDLE  pBitmapRegion;
   L_UINT         uBitmapStructSize;
   HRGN           hRgn;
}
INVERTEDTEXT, *pINVERTEDTEXT;

//********************** L_BorderRemoveBitmap Structure**********************
typedef struct _BORDERREMOVE
{
   L_UINT         uStructSize;
   L_UINT         uFlags;
   L_UINT         uBorderToRemove;
   L_INT          iBorderPercent;
   L_INT          iWhiteNoiseLength;
   L_INT          iVariance;
   HRGN           hRgn;
   pBITMAPHANDLE  pBitmapRegion;
   L_UINT         uBitmapStructSize;
}
BORDERREMOVE, *pBORDERREMOVE;

//********************** L_LineRemoveBitmap Structure**********************
typedef struct _LINEREMOVE
{
   L_UINT         uStructSize;
   L_UINT         uFlags;
   L_INT          iMinLineLength;
   L_INT          iMaxLineWidth;
   L_INT          iWall;
   L_INT          iMaxWallPercent;
   L_INT          iGapLength;
   L_INT          iVariance;
   L_UINT         uRemoveFlags;
   HRGN           hRgn;
   pBITMAPHANDLE  pBitmapRegion;
   L_UINT         uBitmapStructSize;
}
LINEREMOVE, *pLINEREMOVE;

//********************** L_ColorizeGrayBitmap Structure**********************
typedef struct _LTGRAYCOLOR
{
   L_UINT   uStructSize;
   RGBQUAD  crColor;
   L_UINT   uThreshold;
}
LTGRAYCOLOR, *pLTGRAYCOLOR;

//********************** L_SearchRegMarksBitmap Structure**********************
typedef struct _SEARCHMARKS
{
   L_UINT   uStructSize;
   L_UINT   uType;
   L_UINT   uWidth;
   L_UINT   uHeight;
   L_UINT   uMinScale;
   L_UINT   uMaxScale;
   RECT     rcRect;
   L_UINT   uSearchMarkCount;
   POINT    *pMarkDetectedPoints;
   L_UINT   uMarkDetectedCount;
}
SEARCHMARKS, *pSEARCHMARKS;

//********************** L_SliceBitmap Structure**********************
typedef struct _SLICEBITMAPOPTIONS
{
   L_UINT   uStructSize;
   L_UINT   uMaxDeskewAngle;
   L_INT    crFill;
   L_INT    uFlags;
}
SLICEBITMAPOPTIONS, *pSLICEBITMAPOPTIONS;

//**************************L_BlankPageDetectorBitmap***********************
typedef struct _PAGEMARGINS
{
   L_UINT   uTopMargin;
   L_UINT   uBottomMargin;
   L_UINT   uLeftMargin;
   L_UINT   uRightMargin;
}
PAGEMARGINS, *pPAGEMARGINS;

/****************************************************************
   Callback typedefs
****************************************************************/
typedef L_INT (pEXT_CALLBACK BITMAPSLICECALLBACK)(
   pBITMAPHANDLE  pBitmap,
   LPRECT         lpSliceRect,
   L_INT          nAngle,
   L_VOID         *pUserData);

////----------------------------------- callback typedefs --------------------------------------

typedef L_INT (pEXT_CALLBACK SMOOTHCALLBACK)(
   L_UINT   uBumpOrNick,
   L_INT    iStartRow,
   L_INT    iStartCol,
   L_INT    iLength,
   L_UINT   uHorV,
   L_VOID   *pUserData);

typedef L_INT (pEXT_CALLBACK LINEREMOVECALLBACK)(
   HRGN     hRgn,
   L_INT    iStartRow,
   L_INT    iStartCol,
   L_INT    iLength,
   L_VOID   *pUserData);

typedef L_INT (pEXT_CALLBACK BORDERREMOVECALLBACK)(
   HRGN     hRgn,
   L_UINT   uBorderToRemove,
   PRECT    pBoundingRect,
   L_VOID   *pUserData);

typedef L_INT (pEXT_CALLBACK INVERTEDTEXTCALLBACK)(
   HRGN     hRgn,
   PRECT    pBoundingRect,
   L_INT    iWhiteCount,
   L_INT    iBlackCount,
   L_VOID   *pUserData);

typedef L_INT (pEXT_CALLBACK DOTREMOVECALLBACK)(
   HRGN     hRgn,
   PRECT    pBoundingRect,
   L_INT    iWhiteCount,
   L_INT    iBlackCount,
   L_VOID   *pUserData);

typedef L_INT (pEXT_CALLBACK HOLEPUNCHREMOVECALLBACK)(
   HRGN     hRgn,
   PRECT    pBoundingRect,
   L_INT    iHoleIndex,
   L_INT    iHoleTotalCount,
   L_INT    iWhiteCount,
   L_INT    iBlackCount,
   L_VOID   *pUserData);

typedef L_INT (pEXT_CALLBACK STAPLEREMOVECALLBACK)(
   HRGN     hRgn,
   PRECT    pBoundingRect,
   L_UINT   iWhiteCount,
   L_UINT   iBlackCount,
   L_VOID   *pUserData);


/****************************************************************
   Function prototypes
****************************************************************/
#if !defined(FOR_MANAGED) || defined(FOR_MANAGED_IMGCOR)

#if defined (LEADTOOLS_V16_OR_LATER)
L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetAutoTrimRect(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uThreshold,
   RECT           *pRect, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_AutoTrimBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uThreshold, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DeskewBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pnAngle,
   COLORREF       crBack,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DeskewBitmapExt(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pnAngle,
   L_UINT         uAngleRange,
   L_UINT         uAngleResolution,
   COLORREF       crBack,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DeskewCheckBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pnAngle,
   COLORREF       crBack,
   L_UINT uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DespeckleBitmap(
   pBITMAPHANDLE pBitmap, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_MinFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_MaxFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ConvertBitmapSignedToUnsigned(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uShift, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ConvertBitmapUnsignedToSigned(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_HalfToneBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uType,
   L_INT32        nAngle,
   L_UINT         uDim,
   HBITMAPLIST    hList, 
   L_UINT32       uFlags);

// These functions not ported to Windows CE
#if !defined(FOR_WINCE)
L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetMinMaxBits(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pLowBit,
   L_INT          *pHighBit, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetMinMaxVal(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pMinVal,
   L_INT          *pMaxVal, 
   L_UINT32       uFlags);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_MedianFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim, 
   L_UINT32       uFlags);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_WindowLevelBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nLowBit,
   L_INT          nHighBit,
   RGBQUAD        *pLUT,
   L_UINT         uLUTLength,
   L_INT          nOrderDst, 
   L_UINT32       uFlags);

#if defined(LEADTOOLS_V16_OR_LATER) && !defined(FOR_WINCE)
L_LTIMGCOR_API L_INT EXT_FUNCTION L_WindowLevelBitmapExt
(
   pBITMAPHANDLE pBitmap,
   L_INT nLowBit, 
   L_INT nHighBit,
   L_RGBQUAD16 *pLUT,
   L_UINT uLUTLength,
   L_INT nOrderDst, 
   L_UINT32 uFlags
);
#endif // #if defined(LEADTOOLS_V16_OR_LATER) && !defined(FOR_WINCE)

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SmoothBitmap(
   pBITMAPHANDLE  pBitmap,
   pSMOOTH        pSmooth,
   SMOOTHCALLBACK pfnCallback,
   L_VOID         *pUserData, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_LineRemoveBitmap(
   pBITMAPHANDLE        pBitmap,
   pLINEREMOVE          pLineRemove,
   LINEREMOVECALLBACK   pfnCallback,
   L_VOID               *pUserData, 
   L_UINT32             uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_BorderRemoveBitmap(
   pBITMAPHANDLE        pBitmap,
   pBORDERREMOVE        pBorderRemove,
   BORDERREMOVECALLBACK pfnCallback,
   L_VOID               *pUserData, 
   L_UINT32             uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_InvertedTextBitmap(
   pBITMAPHANDLE        pBitmap,
   pINVERTEDTEXT        pInvertedText,
   INVERTEDTEXTCALLBACK pfnCallback,
   L_VOID               *pUserData, 
   L_UINT32             uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DotRemoveBitmap(
   pBITMAPHANDLE     pBitmap,
   pDOTREMOVE        pDotRemove,
   DOTREMOVECALLBACK pfnCallback,
   L_VOID            *pUserData, 
   L_UINT32           uFlags);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_HolePunchRemoveBitmap(
   pBITMAPHANDLE           pBitmap,
   pHOLEPUNCH              pHolePunch,
   HOLEPUNCHREMOVECALLBACK pfnCallback,
   L_VOID                  *pUserData, 
   L_UINT32                uFlags);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_StapleRemoveBitmap(
   pBITMAPHANDLE        pBitmap,
   pSTAPLE              pStaplePunch,
   STAPLEREMOVECALLBACK pfnCallback,
   L_VOID               *pUserData, 
   L_UINT32             uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FFTBitmap(
   pBITMAPHANDLE  pBitmap,
   pFTARRAY       pFTArray,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FTDisplayBitmap(
   pBITMAPHANDLE  pBitmap,
   pFTARRAY       pFTArray,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DFTBitmap(
   pBITMAPHANDLE  pBitmap,
   pFTARRAY       pFTArray,
   RECT           *prcRange,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FrqFilterBitmap(
   pFTARRAY pFTArray,
   LPRECT   prcRange,
   L_UINT   uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FrqFilterMaskBitmap(
   pBITMAPHANDLE  pMaskBitmap,
   pFTARRAY       pFTArray,
   L_BOOL         bOnOff, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_AllocFTArray(
   pBITMAPHANDLE  pBitmap,
   pFTARRAY       *ppFTArray,
   L_UINT         uStructSize, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FreeFTArray(
   pFTARRAY pFTArray, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_CorrelationBitmap(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pCorBitmap,
   POINT          *pPoints,
   L_UINT         uMaxPoints,
   L_UINT         *puNumOfPoints,
   L_UINT         uXStep,
   L_UINT         uYStep,
   L_UINT         uThreshold, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SubtractBackgroundBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uRollingBall,
   L_UINT         uShrinkSize,
   L_UINT         uBrightnessFactor,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyModalityLUT(
   pBITMAPHANDLE        pBitmap,
   L_UINT16             *pLUT,
   pDICOMLUTDESCRIPTOR  pLUTDescriptor,
   L_UINT               uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyLinearModalityLUT(
   pBITMAPHANDLE  pBitmap,
   L_DOUBLE       fIntercept,
   L_DOUBLE       fSlope,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyVOILUT(
   pBITMAPHANDLE        pBitmap,
   L_UINT16             *pLUT,
   pDICOMLUTDESCRIPTOR  pLUTDescriptor,
   L_UINT               uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyLinearVOILUT(
   pBITMAPHANDLE  pBitmap,
   L_DOUBLE       fCenter,
   L_DOUBLE       fWidth,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetLinearVOILUT(
   pBITMAPHANDLE  pBitmap,
   L_DOUBLE       *pCenter,
   L_DOUBLE       *pWidth,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_CountLUTColors(
   RGBQUAD  *pLUT,
   L_UINT   ulLLUTLen,
   L_UINT   *pNumberOfEntries,
   L_INT    *pFirstIndex,
   L_UINT   uFlags);

#ifdef CAN_HAVE_LUT16
L_LTIMGCOR_API L_INT EXT_FUNCTION L_CountLUTColorsExt(
   L_RGBQUAD16 *pLUT,
   L_UINT   ulLLUTLen,
   L_UINT   *pNumberOfEntries,
   L_INT    *pFirstIndex,
   L_UINT   uFlags);
#endif // #ifdef CAN_HAVE_LUT16

L_LTIMGCOR_API L_INT EXT_FUNCTION L_MultiScaleEnhancementBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uContrast,
   L_UINT         uEdgeLevels,
   L_UINT         uEdgeCoeff,
   L_UINT         uLatitudeLevels,
   L_UINT         uLatitudeCoeff,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ShiftBitmapData(
   pBITMAPHANDLE  pDstBitmap,
   pBITMAPHANDLE  pSrcBitmap,
   L_UINT         uSrcLowBit,
   L_UINT         uSrcHighBit,
   L_UINT         uDstLowBit,
   L_UINT         uDstBitsPerPixel, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SelectBitmapData(
   pBITMAPHANDLE  pDstBitmap,
   pBITMAPHANDLE  pSrcBitmap,
   COLORREF       crColor,
   L_UINT         uSrcLowBit,
   L_UINT         uSrcHighBit,
   L_UINT         uThreshold,
   L_BOOL         bCombine, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ColorizeGrayBitmap(
   pBITMAPHANDLE  pDstBitmap,
   pBITMAPHANDLE  pSrcBitmap,
   pLTGRAYCOLOR   pGrayColors,
   L_UINT         uCount, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DigitalSubtractBitmap(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pMaskBitmap,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_IsRegMarkBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uType,
   L_UINT         uMinScale,
   L_UINT         uMaxScale,
   L_UINT         uWidth,
   L_UINT         uHeight, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetMarksCenterMassBitmap(
   pBITMAPHANDLE  pBitmap,
   POINT          *pMarkPoints,
   POINT          *pMarkCMPoints,
   L_UINT         uMarksCount, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SearchRegMarksBitmap(
   pBITMAPHANDLE  pBitmap,
   pSEARCHMARKS   pSearchMarks,
   L_UINT         uMarkCount, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetTransformationParameters(
   pBITMAPHANDLE  pBitmap,
   POINT          *pRefPoints,
   POINT          *pTrnsPoints,
   L_INT          *pnXTranslation,
   L_INT          *pnYTranslation,
   L_INT          *pnAngle,
   L_UINT         *puXScale,
   L_UINT         *puYScale, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyTransformationParameters(
   pBITMAPHANDLE  pBitmap,
   L_INT          nXTranslation,
   L_INT          nYTranslation,
   L_INT          nAngle,
   L_UINT         uXScale,
   L_UINT         uYScale,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_HalfTonePatternBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uContrast,
   L_UINT         uRipple,
   L_UINT         uAngleContrast,
   L_UINT         uAngleRipple,
   L_INT          nAngleOffset,
   COLORREF       crForGround,
   COLORREF       crBackGround,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_CorrelationListBitmap(
   pBITMAPHANDLE  pBitmap,
   HBITMAPLIST    hCorList,
   POINT          *pPoints,
   L_UINT         *puListIndex,
   L_UINT         uMaxPoints,
   L_UINT         *puNumOfPoints,
   L_UINT         uXStep,
   L_UINT         uYStep,
   L_UINT         uThreshold, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SetKaufmannRgnBitmap(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pKaufmannProcBitmap,
   L_INT          nRadius,
   L_INT          nMinInput,
   L_INT          nMaxInput,
   L_INT          nRgnThreshold,
   POINT          ptRgnStart,
   L_BOOL         bRemoveHoles,
   L_SIZE_T      *puPixelsCount,
   L_UINT         uCombineMode, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SliceBitmap(
   pBITMAPHANDLE        pBitmap,
   pSLICEBITMAPOPTIONS  pOptions,
   L_INT                *pnDeskewAngle,
   BITMAPSLICECALLBACK  pfnCallback,
   L_VOID               *pUserData, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ShiftMinimumToZero(
   pBITMAPHANDLE  pBitmap,
   L_UINT         *puShiftAmount, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ShiftZeroToNegative(
   pBITMAPHANDLE  pBitmap,
   L_INT          nShiftAmount,
   L_INT          nMinInput,
   L_INT          nMaxInput,
   L_INT          nMinOutput,
   L_INT          nMaxOutput, 
   L_UINT32       uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SizeBitmapInterpolate(pBITMAPHANDLE pBitmap,
                                                         L_INT         nWidth,
                                                         L_INT         nHeight,
                                                         L_UINT        uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_BlankPageDetectorBitmap (pBITMAPHANDLE   pBitmap,
                                                             L_BOOL          *bIsBlank,
                                                             L_UINT          *pAccuracy,
                                                             pPAGEMARGINS    pMargins,
                                                             L_UINT          uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_AutoBinarizeBitmap (pBITMAPHANDLE   pBitmap,
                                                        L_UINT          uFactor,
                                                        L_UINT          uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_AutoZoneBitmap(pBITMAPHANDLE       pBitmap,
                                                   HGLOBAL *           phZones,
                                                   L_UINT32 *          puCount,
                                                   L_UINT32            uFlags,
                                                   AUTOZONECALLBACK    pCallback,
                                                   L_VOID *            pUserData);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FreeZoneData (HGLOBAL hZones, L_INT nCount);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_InvertedPageBitmap (pBITMAPHANDLE   pBitmap,
                                                        L_BOOL *        bIsInverted,
                                                        L_UINT          uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_HighQualityRotateBitmap (pBITMAPHANDLE   pBitmap,
                                                             L_INT           nAngle,
                                                             L_UINT          uFlags,
                                                             L_COLORREF      crFill);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_TissueEqualizeBitmap(pBITMAPHANDLE pBitmap,
                                                         L_UINT uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_RakeRemoveBitmap(pBITMAPHANDLE pBitmap,
                                                     L_BOOL bAuto,
                                                     pRAKEREMOVE pComb,
                                                     RECT* pDstRect,
                                                     L_INT nRectCount,
                                                     RAKEREMOVECALLBACK pCallback,
                                                     L_VOID *pUserData,
                                                     L_UINT32 uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_StartFastMagicWandEngine(MAGICWANDHANDLE* pMagicWnd,
                                                             pBITMAPHANDLE LeadBitmap,
                                                             L_UINT32 uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_EndFastMagicWandEngine(MAGICWANDHANDLE MagicWnd,
                                                           L_UINT32 uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FastMagicWand(MAGICWANDHANDLE MagicWnd,
                                                  L_INT nTolerance,
                                                  L_INT nXposition,
                                                  L_INT nYposition,
                                                  pOBJECTINFO pObjectInfo,
                                                  L_UINT32 uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DeleteObjectInfo(pOBJECTINFO pObjectInfo,
                                                     L_UINT32 uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ObjectCounter(pBITMAPHANDLE pBitmap,
                                                  L_UINT *uCount,
                                                  OBJECTCOUNTERCALLBACK pCallback,
                                                  L_VOID *pUserData,
                                                  L_UINT32 uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_AutoSegmentBitmap(pBITMAPHANDLE pBitmap,
                                                      L_RECT * pRect,
                                                      L_UINT32 uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SigmaFilterBitmap(pBITMAPHANDLE pBitmap,
                                                      L_UINT nSize,
                                                      L_UINT nSigma,
                                                      L_FLOAT fThreshhold,
                                                      L_BOOL bOutline,
                                                      L_UINT32 uFlags);


#endif // #if !defined(FOR_WINCE)



#else

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetAutoTrimRect(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uThreshold,
   RECT           *pRect);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_AutoTrimBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uThreshold);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DeskewBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pnAngle,
   COLORREF       crBack,
   L_UINT uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DeskewBitmapExt(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pnAngle,
   L_UINT         uAngleRange,
   L_UINT         uAngleResolution,
   COLORREF       crBack,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DeskewCheckBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pnAngle,
   COLORREF       crBack,
   L_UINT uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DespeckleBitmap(
   pBITMAPHANDLE pBitmap);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_MinFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_MaxFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim);

// These functions not ported to Windows CE
#if !defined(FOR_WINCE)
L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetMinMaxBits(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pLowBit,
   L_INT          *pHighBit);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetMinMaxVal(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pMinVal,
   L_INT          *pMaxVal);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_MedianFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_ConvertBitmapSignedToUnsigned(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uShift);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_WindowLevelBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nLowBit,
   L_INT          nHighBit,
   RGBQUAD        *pLUT,
   L_UINT         uLUTLength,
   L_INT          nOrderDst);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_SmoothBitmap(
   pBITMAPHANDLE  pBitmap,
   pSMOOTH        pSmooth,
   SMOOTHCALLBACK pfnCallback,
   L_VOID         *pUserData);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_LineRemoveBitmap(
   pBITMAPHANDLE        pBitmap,
   pLINEREMOVE          pLineRemove,
   LINEREMOVECALLBACK   pfnCallback,
   L_VOID               *pUserData);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_BorderRemoveBitmap(
   pBITMAPHANDLE        pBitmap,
   pBORDERREMOVE        pBorderRemove,
   BORDERREMOVECALLBACK pfnCallback,
   L_VOID               *pUserData);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_InvertedTextBitmap(
   pBITMAPHANDLE        pBitmap,
   pINVERTEDTEXT        pInvertedText,
   INVERTEDTEXTCALLBACK pfnCallback,
   L_VOID               *pUserData);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DotRemoveBitmap(
   pBITMAPHANDLE     pBitmap,
   pDOTREMOVE        pDotRemove,
   DOTREMOVECALLBACK pfnCallback,
   L_VOID            *pUserData);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_HolePunchRemoveBitmap(
   pBITMAPHANDLE           pBitmap,
   pHOLEPUNCH              pHolePunch,
   HOLEPUNCHREMOVECALLBACK pfnCallback,
   L_VOID                  *pUserData);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_StapleRemoveBitmap(
   pBITMAPHANDLE        pBitmap,
   pSTAPLE              pStaplePunch,
   STAPLEREMOVECALLBACK pfnCallback,
   L_VOID               *pUserData);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_HalfToneBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uType,
   L_INT32        nAngle,
   L_UINT         uDim,
   HBITMAPLIST    hList);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FFTBitmap(
   pBITMAPHANDLE  pBitmap,
   pFTARRAY       pFTArray,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FTDisplayBitmap(
   pBITMAPHANDLE  pBitmap,
   pFTARRAY       pFTArray,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DFTBitmap(
   pBITMAPHANDLE  pBitmap,
   pFTARRAY       pFTArray,
   RECT           *prcRange,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FrqFilterBitmap(
   pFTARRAY pFTArray,
   LPRECT   prcRange,
   L_UINT   uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FrqFilterMaskBitmap(
   pBITMAPHANDLE  pMaskBitmap,
   pFTARRAY       pFTArray,
   L_BOOL         bOnOff);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_AllocFTArray(
   pBITMAPHANDLE  pBitmap,
   pFTARRAY       *ppFTArray,
   L_UINT         uStructSize);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_FreeFTArray(
   pFTARRAY pFTArray);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_CorrelationBitmap(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pCorBitmap,
   POINT          *pPoints,
   L_UINT         uMaxPoints,
   L_UINT         *puNumOfPoints,
   L_UINT         uXStep,
   L_UINT         uYStep,
   L_UINT         uThreshold);


L_LTIMGCOR_API L_INT EXT_FUNCTION L_SubtractBackgroundBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uRollingBall,
   L_UINT         uShrinkSize,
   L_UINT         uBrightnessFactor,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyModalityLUT(
   pBITMAPHANDLE        pBitmap,
   L_UINT16             *pLUT,
   pDICOMLUTDESCRIPTOR  pLUTDescriptor,
   L_UINT               uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyLinearModalityLUT(
   pBITMAPHANDLE  pBitmap,
   L_DOUBLE       fIntercept,
   L_DOUBLE       fSlope,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyVOILUT(
   pBITMAPHANDLE        pBitmap,
   L_UINT16             *pLUT,
   pDICOMLUTDESCRIPTOR  pLUTDescriptor,
   L_UINT               uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyLinearVOILUT(
   pBITMAPHANDLE  pBitmap,
   L_DOUBLE       fCenter,
   L_DOUBLE       fWidth,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetLinearVOILUT(
   pBITMAPHANDLE  pBitmap,
   L_DOUBLE       *pCenter,
   L_DOUBLE       *pWidth,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_CountLUTColors(
   RGBQUAD  *pLUT,
   L_UINT   ulLLUTLen,
   L_UINT   *pNumberOfEntries,
   L_INT    *pFirstIndex,
   L_UINT   uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_MultiScaleEnhancementBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uContrast,
   L_UINT         uEdgeLevels,
   L_UINT         uEdgeCoeff,
   L_UINT         uLatitudeLevels,
   L_UINT         uLatitudeCoeff,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ShiftBitmapData(
   pBITMAPHANDLE  pDstBitmap,
   pBITMAPHANDLE  pSrcBitmap,
   L_UINT         uSrcLowBit,
   L_UINT         uSrcHighBit,
   L_UINT         uDstLowBit,
   L_UINT         uDstBitsPerPixel);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SelectBitmapData(
   pBITMAPHANDLE  pDstBitmap,
   pBITMAPHANDLE  pSrcBitmap,
   COLORREF       crColor,
   L_UINT         uSrcLowBit,
   L_UINT         uSrcHighBit,
   L_UINT         uThreshold,
   L_BOOL         bCombine);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ColorizeGrayBitmap(
   pBITMAPHANDLE  pDstBitmap,
   pBITMAPHANDLE  pSrcBitmap,
   pLTGRAYCOLOR   pGrayColors,
   L_UINT         uCount);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_DigitalSubtractBitmap(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pMaskBitmap,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_IsRegMarkBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uType,
   L_UINT         uMinScale,
   L_UINT         uMaxScale,
   L_UINT         uWidth,
   L_UINT         uHeight);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetMarksCenterMassBitmap(
   pBITMAPHANDLE  pBitmap,
   POINT          *pMarkPoints,
   POINT          *pMarkCMPoints,
   L_UINT         uMarksCount);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SearchRegMarksBitmap(
   pBITMAPHANDLE  pBitmap,
   pSEARCHMARKS   pSearchMarks,
   L_UINT         uMarkCount);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_GetTransformationParameters(
   pBITMAPHANDLE  pBitmap,
   POINT          *pRefPoints,
   POINT          *pTrnsPoints,
   L_INT          *pnXTranslation,
   L_INT          *pnYTranslation,
   L_INT          *pnAngle,
   L_UINT         *puXScale,
   L_UINT         *puYScale);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ApplyTransformationParameters(
   pBITMAPHANDLE  pBitmap,
   L_INT          nXTranslation,
   L_INT          nYTranslation,
   L_INT          nAngle,
   L_UINT         uXScale,
   L_UINT         uYScale,
   L_UINT         uFlags);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ConvertBitmapUnsignedToSigned(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uFlag);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_HalfTonePatternBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uContrast,
   L_UINT         uRipple,
   L_UINT         uAngleContrast,
   L_UINT         uAngleRipple,
   L_INT          nAngleOffset,
   COLORREF       crForGround,
   COLORREF       crBackGround,
   L_UINT         uFlag);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_CorrelationListBitmap(
   pBITMAPHANDLE  pBitmap,
   HBITMAPLIST    hCorList,
   POINT          *pPoints,
   L_UINT         *puListIndex,
   L_UINT         uMaxPoints,
   L_UINT         *puNumOfPoints,
   L_UINT         uXStep,
   L_UINT         uYStep,
   L_UINT         uThreshold);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SetKaufmannRgnBitmap(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pKaufmannProcBitmap,
   L_INT          nRadius,
   L_INT          nMinInput,
   L_INT          nMaxInput,
   L_INT          nRgnThreshold,
   POINT          ptRgnStart,
   L_BOOL         bRemoveHoles,
   L_SIZE_T      *puPixelsCount,
   L_UINT         uCombineMode);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SliceBitmap(
   pBITMAPHANDLE        pBitmap,
   pSLICEBITMAPOPTIONS  pOptions,
   L_INT                *pnDeskewAngle,
   BITMAPSLICECALLBACK  pfnCallback,
   L_VOID               *pUserData);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ShiftMinimumToZero(
   pBITMAPHANDLE  pBitmap,
   L_UINT         *puShiftAmount);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_ShiftZeroToNegative(
   pBITMAPHANDLE  pBitmap,
   L_INT          nShiftAmount,
   L_INT          nMinInput,
   L_INT          nMaxInput,
   L_INT          nMinOutput,
   L_INT          nMaxOutput);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_SizeBitmapInterpolate(pBITMAPHANDLE pBitmap,
                                                         L_INT         nWidth,
                                                         L_INT         nHeight,
                                                         L_UINT        uFlag);

L_LTIMGCOR_API L_INT EXT_FUNCTION L_BlankPageDetectorBitmap (pBITMAPHANDLE   pBitmap,
                                                             L_BOOL          *bIsBlank,
                                                             L_UINT          *pAccuracy,
                                                             pPAGEMARGINS    pMargins,
                                                             L_UINT          uFlags);

#endif // #if !defined(FOR_WINCE)

#endif //LEADTOOLS_V16_OR_LATER


#endif // #if !defined(FOR_MANAGED) || defined(FOR_MANAGED_IMGCOR)

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTIMGCOR_H)
