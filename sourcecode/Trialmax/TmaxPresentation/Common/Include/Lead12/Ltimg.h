/*
   LTIMG.H - image module header file
   Copyright (c) 1991-2000 LEAD Technologies, Inc.
   All Rights Reserved.
*/

#ifndef _LTIMG_H_
#define _LTIMG_H_

#include "ltkrn.h"

#define _HEADER_ENTRY_
#include "ltpck.h"

/*
   flags for L_EmbossBitmap
*/
#define EMBOSS_N    0
#define EMBOSS_NE   1
#define EMBOSS_E    2
#define EMBOSS_SE   3
#define EMBOSS_S    4
#define EMBOSS_SW   5
#define EMBOSS_W    6
#define EMBOSS_NW   7

/*
   flags for L_GetHistogram, L_RemapBitmapIntensity, and L_AddNoiseBitmap
*/
#define CHANNEL_MASTER 0        /* All channels */
#define CHANNEL_RED    1        /* Red channel only */
#define CHANNEL_GREEN  2        /* Green channel only */
#define CHANNEL_BLUE   3        /* Blue channel only */

/*
   flags for L_ColorSeparateBitmap/L_ColorMergeBitmap
*/
#define COLORSEP_RGB                 0x00 /* Use 3 RGB color planes. */
#define COLORSEP_CMYK                0x01 /* Use 4 CMYK color planes. */
#define COLORSEP_HSV                 0x02 /* Use 3 HSV color planes. */
#define COLORSEP_HLS                 0x03 /* Use 3 HLS color planes. */
#define COLORSEP_CMY                 0x04 /* Use 3 CMY color planes. */

/*
   Binary filter constants
*/
#define BFLT_DONTCARE   256

typedef struct tagBINARYFLT
{
   L_BOOL bfltMax;              /* select maximimum value */
   L_UINT bfltDim;              /* filter dimensions */
   L_INT bfltMatrix[1];         /* array of filter coefficients */
} BINARYFLT, L_FAR * LPBINARYFLT;

#define BINARYFLTSIZE(dim) (sizeof(BINARYFLT) + (sizeof(L_INT) * (((L_INT) (dim) * (L_INT) (dim)) - 1)))
#define MAKEINTBFLT(i) ((LPBINARYFLT)((L_UINT32)((L_UINT16) (i))))

#define BFLT_EROSION_OMNI    MAKEINTBFLT(0)
#define BFLT_EROSION_HORZ    MAKEINTBFLT(1)
#define BFLT_EROSION_VERT    MAKEINTBFLT(2)
#define BFLT_EROSION_DIAG    MAKEINTBFLT(3)
#define BFLT_DILATION_OMNI    MAKEINTBFLT(4)
#define BFLT_DILATION_HORZ    MAKEINTBFLT(5)
#define BFLT_DILATION_VERT    MAKEINTBFLT(6)
#define BFLT_DILATION_DIAG    MAKEINTBFLT(7)

/*
   spatial filter constants
*/
typedef struct tagSPATIALFLT
{
   L_INT fltDivisor;            /* sum divisor */
   L_INT fltBias;               /* sum bias */
   L_UINT fltDim;               /* filter dimensions */
   L_INT fltMatrix[1];          /* array of filter coefficients */
} SPATIALFLT, L_FAR * LPSPATIALFLT;

#define SPATIALFLTSIZE(dim) (sizeof(SPATIALFLT) + (sizeof(L_INT) * (((L_INT) (dim) * (L_INT) (dim)) - 1)))
#define MAKEINTFLT(i) ((LPSPATIALFLT)((L_UINT32)((L_UINT16) (i))))

#define FLT_EMBOSS_N    MAKEINTFLT(0)
#define FLT_EMBOSS_NE   MAKEINTFLT(1)
#define FLT_EMBOSS_E    MAKEINTFLT(2)
#define FLT_EMBOSS_SE   MAKEINTFLT(3)
#define FLT_EMBOSS_S    MAKEINTFLT(4)
#define FLT_EMBOSS_SW   MAKEINTFLT(5)
#define FLT_EMBOSS_W    MAKEINTFLT(6)
#define FLT_EMBOSS_NW   MAKEINTFLT(7)

#define FLT_GRADIENT_N    MAKEINTFLT(8)
#define FLT_GRADIENT_NE   MAKEINTFLT(9)
#define FLT_GRADIENT_E    MAKEINTFLT(10)
#define FLT_GRADIENT_SE   MAKEINTFLT(11)
#define FLT_GRADIENT_S    MAKEINTFLT(12)
#define FLT_GRADIENT_SW   MAKEINTFLT(13)
#define FLT_GRADIENT_W    MAKEINTFLT(14)
#define FLT_GRADIENT_NW   MAKEINTFLT(15)

#define FLT_LAPLACE_1     MAKEINTFLT(16)
#define FLT_LAPLACE_2     MAKEINTFLT(17)
#define FLT_LAPLACE_3     MAKEINTFLT(18)
#define FLT_LAPLACE_DIAG  MAKEINTFLT(19)
#define FLT_LAPLACE_HORZ  MAKEINTFLT(20)
#define FLT_LAPLACE_VERT  MAKEINTFLT(21)

#define FLT_SOBEL_HORZ    MAKEINTFLT(22)
#define FLT_SOBEL_VERT    MAKEINTFLT(23)

#define FLT_PREWITT_HORZ  MAKEINTFLT(24)
#define FLT_PREWITT_VERT  MAKEINTFLT(25)

#define FLT_SHIFTDIFF_DIAG  MAKEINTFLT(26)
#define FLT_SHIFTDIFF_HORZ  MAKEINTFLT(27)
#define FLT_SHIFTDIFF_VERT  MAKEINTFLT(28)

#define FLT_LINESEG_HORZ  MAKEINTFLT(29)
#define FLT_LINESEG_VERT  MAKEINTFLT(30)
#define FLT_LINESEG_LTOR  MAKEINTFLT(31)
#define FLT_LINESEG_RTOL  MAKEINTFLT(32)

/*
   Flags for L_PicturizeBitmap
*/
#define PICTURIZE_LOADALL     0x0001   /* Load all files in memory */
#define PICTURIZE_RESIZE      0x0002   /* Resize all images that are not the required size */
#define PICTURIZE_RESAMPLE    0x0004   /* Resample all images that are not the required size */
#define PICTURIZE_ONCE        0x0010   /* Use the image only once thoughout the process */
#define PICTURIZE_EQUALUSAGE  0x0020   /* Use all images equally, i.e. before an image will be
                                          used twice all other images must be used at least once */
#define PICTURIZE_BESTFIT     0x0040   /* Images will be resized to fit the image if the bitmap
                                          width or height is not a multiple of nCellWidth/nCellHeight */

#define CONTOUR_THIN              0
#define CONTOUR_LINK_BLACKWHITE   1
#define CONTOUR_LINK_GRAY         2
#define CONTOUR_LINK_COLOR        3
#define CONTOUR_APPROX_COLOR      4

#define DEFAULT_THRESHOLD       15 /* default for background threshold value */
#define DEFAULT_DELTADIRECTION  35 /* default for maximum angle for the thinning algorithm */
#define DEFAULT_MAXIMUMERROR     5 /* default for maximum approximation error */

/*
   Constants and Flags for Doc Imaging functions

*/
/* Defined so that !(SMOOTH_BUMP) == (SMOOTH_NICK) */
#define SMOOTH_BUMP 0
#define SMOOTH_NICK 1
#define SMOOTH_NONE 2

#define SMOOTH_HORIZONTAL_ELEMENT   0
#define SMOOTH_VERTICAL_ELEMENT     1

#define SUCCESS_REMOVE              1  
#define SUCCESS_NOREMOVE            2  
#define LINEREMOVE_HORIZONTAL       1
#define LINEREMOVE_VERTICAL         2
#define SUCCESS_INVERT              1
#define SUCCESS_NOINVERT            2

#define BORDER_LEFT     1
#define BORDER_RIGHT    2
#define BORDER_TOP      4
#define BORDER_BOTTOM   8
#define BORDER_ALL      (BORDER_LEFT | BORDER_RIGHT | BORDER_TOP | BORDER_BOTTOM)
#define BORDER_DELTA_MAX (0xffffffff)

//flags for iLocation
#define HOLEPUNCH_LEFT     (1)
#define HOLEPUNCH_RIGHT    (2)
#define HOLEPUNCH_TOP      (3)
#define HOLEPUNCH_BOTTOM   (4)

#define STAPLE_TOPLEFT     (1)
#define STAPLE_TOPRIGHT    (2)
#define STAPLE_BOTTOMLEFT  (3)
#define STAPLE_BOTTOMRIGHT (4)

#define FLAG_USE_DPI                 0x00000001
#define FLAG_SINGLE_REGION           0x00000002
#define FLAG_LEAD_REGION             0x00000004
#define FLAG_CALLBACK_REGION         0x00000008
#define FLAG_IMAGE_UNCHANGED         0x00000010
#define FLAG_USE_SIZE                0x00000020
#define FLAG_USE_COUNT               0x00000040
#define FLAG_USE_LOCATION            0x00000080
#define FLAG_FAVOR_LONG              0x00000100
#define FLAG_REMOVE_ENTIRE           0x00000200
#define FLAG_USE_GAP                 0x00000400
#define FLAG_USE_VARIANCE            0x00000800
#define FLAG_USE_DIAGONALS           0x00001000


#define SMOOTH_SINGLE_REGION         (FLAG_SINGLE_REGION)
#define SMOOTH_LEAD_REGION           (FLAG_LEAD_REGION)
#define SMOOTH_IMAGE_UNCHANGED       (FLAG_IMAGE_UNCHANGED)
#define SMOOTH_FAVOR_LONG            (FLAG_FAVOR_LONG)
#define SMOOTH_ALLFLAGS              ( \
                                       SMOOTH_SINGLE_REGION       | \
                                       SMOOTH_LEAD_REGION         | \
                                       SMOOTH_IMAGE_UNCHANGED     | \
                                       SMOOTH_FAVOR_LONG \
                                      )

#define LINE_USE_DPI                 (FLAG_USE_DPI)            
#define LINE_SINGLE_REGION           (FLAG_SINGLE_REGION)     
#define LINE_LEAD_REGION             (FLAG_LEAD_REGION)  
#define LINE_CALLBACK_REGION         (FLAG_CALLBACK_REGION) 
#define LINE_IMAGE_UNCHANGED         (FLAG_IMAGE_UNCHANGED)
#define LINE_REMOVE_ENTIRE           (FLAG_REMOVE_ENTIRE)
#define LINE_USE_GAP                 (FLAG_USE_GAP)
#define LINE_USE_VARIANCE            (FLAG_USE_VARIANCE)
#define LINE_ALLFLAGS                ( \
                                       LINE_USE_DPI                  | \
                                       LINE_SINGLE_REGION            | \
                                       LINE_LEAD_REGION              | \
                                       LINE_CALLBACK_REGION          | \
                                       LINE_REMOVE_ENTIRE            | \
                                       LINE_USE_GAP                  | \
                                       LINE_USE_VARIANCE               \
                                     )

#define BORDER_LEAD_REGION           (FLAG_LEAD_REGION) 
#define BORDER_CALLBACK_REGION       (FLAG_CALLBACK_REGION)
#define BORDER_SINGLE_REGION         (FLAG_SINGLE_REGION)
#define BORDER_IMAGE_UNCHANGED       (FLAG_IMAGE_UNCHANGED)
#define BORDER_USE_VARIANCE          (FLAG_USE_VARIANCE)
#define BORDER_ALLFLAGS              ( \
                                       BORDER_LEAD_REGION      | \
                                       BORDER_CALLBACK_REGION  | \
                                       BORDER_SINGLE_REGION    | \
                                       BORDER_IMAGE_UNCHANGED  | \
                                       BORDER_USE_VARIANCE       \
                                     )

#define INVERTEDTEXT_USE_DPI         (FLAG_USE_DPI) 
#define INVERTEDTEXT_SINGLE_REGION   (FLAG_SINGLE_REGION)  
#define INVERTEDTEXT_LEAD_REGION     (FLAG_LEAD_REGION)
#define INVERTEDTEXT_CALLBACK_REGION (FLAG_CALLBACK_REGION)
#define INVERTEDTEXT_IMAGE_UNCHANGED (FLAG_IMAGE_UNCHANGED)
#define INVERTEDTEXT_USE_DIAGONALS   (FLAG_USE_DIAGONALS)

#define INVERTEDTEXT_ALLFLAGS         ( \
                                       INVERTEDTEXT_USE_DPI             | \
                                       INVERTEDTEXT_SINGLE_REGION       | \
                                       INVERTEDTEXT_LEAD_REGION         | \
                                       INVERTEDTEXT_CALLBACK_REGION     | \
                                       INVERTEDTEXT_IMAGE_UNCHANGED     | \
                                       INVERTEDTEXT_USE_DIAGONALS         \
                                      )


#define DOT_USE_DPI                  (FLAG_USE_DPI)
#define DOT_SINGLE_REGION            (FLAG_SINGLE_REGION)
#define DOT_LEAD_REGION              (FLAG_LEAD_REGION)
#define DOT_CALLBACK_REGION          (FLAG_CALLBACK_REGION)
#define DOT_IMAGE_UNCHANGED          (FLAG_IMAGE_UNCHANGED)
#define DOT_USE_SIZE                 (FLAG_USE_SIZE)
#define DOT_USE_DIAGONALS            (FLAG_USE_DIAGONALS)

#define DOT_ALLFLAGS                 ( \
                                       DOT_USE_DPI             | \
                                       DOT_SINGLE_REGION       | \
                                       DOT_LEAD_REGION         | \
                                       DOT_CALLBACK_REGION     | \
                                       DOT_IMAGE_UNCHANGED     | \
                                       DOT_USE_SIZE            | \
                                       DOT_USE_DIAGONALS         \
                                     )


#define HOLEPUNCH_USE_DPI            (FLAG_USE_DPI)       
#define HOLEPUNCH_SINGLE_REGION      (FLAG_SINGLE_REGION)
#define HOLEPUNCH_LEAD_REGION        (FLAG_LEAD_REGION)
#define HOLEPUNCH_CALLBACK_REGION    (FLAG_CALLBACK_REGION)
#define HOLEPUNCH_IMAGE_UNCHANGED    (FLAG_IMAGE_UNCHANGED)
#define HOLEPUNCH_USE_SIZE           (FLAG_USE_SIZE)      
#define HOLEPUNCH_USE_COUNT          (FLAG_USE_COUNT)     
#define HOLEPUNCH_USE_LOCATION       (FLAG_USE_LOCATION)  

#define HOLEPUNCH_ALLFLAGS           ( \
                                       HOLEPUNCH_USE_DPI          | \
                                       HOLEPUNCH_SINGLE_REGION    | \
                                       HOLEPUNCH_LEAD_REGION      | \
                                       HOLEPUNCH_CALLBACK_REGION  | \
                                       HOLEPUNCH_IMAGE_UNCHANGED  | \
                                       HOLEPUNCH_USE_SIZE         | \
                                       HOLEPUNCH_USE_COUNT        | \
                                       HOLEPUNCH_USE_LOCATION \
                                       )


#define STAPLE_USE_DPI            (FLAG_USE_DPI)       
#define STAPLE_SINGLE_REGION      (FLAG_SINGLE_REGION)
#define STAPLE_LEAD_REGION        (FLAG_LEAD_REGION)
#define STAPLE_CALLBACK_REGION    (FLAG_CALLBACK_REGION)
#define STAPLE_IMAGE_UNCHANGED    (FLAG_IMAGE_UNCHANGED)
#define STAPLE_USE_SIZE           (FLAG_USE_SIZE)      
#define STAPLE_USE_LOCATION       (FLAG_USE_LOCATION)  

#define STAPLE_ALLFLAGS           ( \
                                       STAPLE_USE_DPI          | \
                                       STAPLE_SINGLE_REGION    | \
                                       STAPLE_LEAD_REGION      | \
                                       STAPLE_CALLBACK_REGION  | \
                                       STAPLE_IMAGE_UNCHANGED  | \
                                       STAPLE_USE_SIZE         | \
                                       STAPLE_USE_LOCATION \
                                       )


/*
structures for Doc Imaging functions
*/

typedef struct tagSMOOTH
{
   L_INT32        iSize;              
   L_UINT32       uFlags;
   L_INT32        iLength;  
   pBITMAPHANDLE  pBitmapRegion;
   HRGN           hRgn;
} SMOOTH, L_FAR *LPSMOOTH;

typedef struct tagHOLEPUNCH
{
   L_INT32        iSize;              
   L_UINT32       uFlags;
   L_INT32        iMinHoleCount;
   L_INT32        iMaxHoleCount;
   L_INT32        iMinHoleWidth;
   L_INT32        iMinHoleHeight;
   L_INT32        iMaxHoleWidth;
   L_INT32        iMaxHoleHeight;
   L_INT32        iLocation;
   pBITMAPHANDLE  pBitmapRegion;
   HRGN           hRgn;

} HOLEPUNCH, L_FAR * LPHOLEPUNCH;

typedef struct tagSTAPLE
{
   L_INT32        iSize;              
   L_UINT32       uFlags;
   L_INT32        iMinLength;
   L_INT32        iMaxLength;
   L_INT32        iLocation;
   pBITMAPHANDLE  pBitmapRegion;
   HRGN           hRgn;

} STAPLE, L_FAR * LPSTAPLE;

typedef struct tagDOTREMOVE
{
   L_INT32        iSize;
   L_UINT32       uFlags;
   L_INT32        iMinDotWidth;
   L_INT32        iMinDotHeight;
   L_INT32        iMaxDotWidth;
   L_INT32        iMaxDotHeight;
   pBITMAPHANDLE  pBitmapRegion;
   HRGN           hRgn;
} DOTREMOVE, L_FAR *LPDOTREMOVE;

typedef struct tagINVERTEDTEXT
{
   L_INT32        iSize;
   L_UINT32       uFlags;
   L_INT32        iMinInvertWidth;
   L_INT32        iMinInvertHeight;
   L_INT32        iMinBlackPercent;
   L_INT32        iMaxBlackPercent;
   pBITMAPHANDLE  pBitmapRegion;
   HRGN           hRgn;
} INVERTEDTEXT, L_FAR *LPINVERTEDTEXT;

typedef struct tagBORDERREMOVE
{
   L_INT32       iSize;
   L_UINT32      uFlags;
   L_UINT32      uBorderToRemove;
   L_INT32       iBorderPercent;
   L_INT32       iWhiteNoiseLength;
   L_INT32       iVariance;
   HRGN          hRgn;
   pBITMAPHANDLE pBitmapRegion;
} BORDERREMOVE, L_FAR *LPBORDERREMOVE;

typedef struct tagLINEREMOVE
{
   L_INT32       iSize;
   L_UINT32      uFlags;
   L_INT32       iMinLineLength;
   L_INT32       iMaxLineWidth;
   L_INT32       iWall;
   L_INT32       iMaxWallPercent;
   L_INT32       iGapLength;
   L_INT32       iVariance;
   L_UINT32      uRemoveFlags;
   HRGN          hRgn;
   pBITMAPHANDLE pBitmapRegion;
} LINEREMOVE, L_FAR *LPLINEREMOVE;


/*
   callback typedefs
*/
typedef L_INT (pEXT_CALLBACK PICTURIZECALLBACK)(pBITMAPHANDLE, L_INT, L_INT, L_VOID L_FAR *);

typedef L_INT (pEXT_CALLBACK SMOOTHCALLBACK)
   (  
    L_UINT32       uBumpOrNick,
    L_INT32        iStartRow, 
    L_INT32        iStartCol, 
    L_INT32        iLength,
    L_UINT32       uHorV,
    L_VOID L_FAR  *pUserData
    );

typedef L_INT (pEXT_CALLBACK LINEREMOVECALLBACK)
   (
    HRGN           hRgn, 
    L_INT32        iStartRow, 
    L_INT32        iStartCol, 
    L_INT32        iLength, 
    L_VOID L_FAR  *pUserData
    );

typedef L_INT (pEXT_CALLBACK BORDERREMOVECALLBACK)
   (
    HRGN          hRgn, 
    L_UINT32      uBorderToRemove, 
    PRECT         pBoundingRect, 
    L_VOID L_FAR *pUserData
    );

typedef L_INT (pEXT_CALLBACK INVERTEDTEXTCALLBACK)
   (
    HRGN          hRgn, 
    PRECT         pBoundingRect, 
    L_INT32       iWhiteCount, 
    L_INT32       iBlackCount, 
    L_VOID L_FAR *pUserData
    );

typedef L_INT (pEXT_CALLBACK DOTREMOVECALLBACK)
   (
    HRGN          hRgn, 
    PRECT         pBoundingRect, 
    L_INT32       iWhiteCount, 
    L_INT32       iBlackCount, 
    L_VOID L_FAR *pUserData
    );

typedef L_INT (pEXT_CALLBACK HOLEPUNCHREMOVECALLBACK)
   (
    HRGN          hRgn, 
    PRECT         pBoundingRect, 
    L_INT32       iHoleIndex,  
    L_INT32       iHoleTotalCount, 
    L_INT32       iWhiteCount, 
    L_INT32       iBlackCount, 
    L_VOID L_FAR *pUserData
    );

typedef L_INT (pEXT_CALLBACK STAPLEREMOVECALLBACK)
   (
    HRGN          hRgn, 
    PRECT         pBoundingRect, 
    L_UINT32      iWhiteCount, 
    L_UINT32      iBlackCount, 
    L_VOID L_FAR *pUserData
    );


/*
   callback typedefs
*/
typedef L_INT (pEXT_CALLBACK PICTURIZECALLBACK) (pBITMAPHANDLE, L_INT, L_INT, L_VOID L_FAR *);

/*
   function prototypes
*/
L_INT EXT_FUNCTION L_AddBitmapNoise (pBITMAPHANDLE pBitmap,
                                      L_UINT uRange,
                                      L_UINT uChannel);

L_INT EXT_FUNCTION L_AutoTrimBitmap(pBITMAPHANDLE pBitmap,
                                     L_UINT uThreshold);

L_INT EXT_FUNCTION L_AverageFilterBitmap (pBITMAPHANDLE pBitmap,
                                           L_UINT uDim);

L_INT EXT_FUNCTION L_BinaryFilterBitmap (pBITMAPHANDLE pBitmap,
                                          LPBINARYFLT pFilter);

L_INT EXT_FUNCTION L_ChangeBitmapContrast (pBITMAPHANDLE pBitmap,
                                            L_INT nChange);

L_INT EXT_FUNCTION L_ChangeBitmapHue (pBITMAPHANDLE pBitmap,
                                       L_INT nAngle);

L_INT EXT_FUNCTION L_ChangeBitmapIntensity (pBITMAPHANDLE pBitmap,
                                             L_INT nChange);

L_INT EXT_FUNCTION L_ChangeBitmapSaturation (pBITMAPHANDLE pBitmap,
                                              L_INT nChange);

L_INT EXT_FUNCTION L_ColorMergeBitmap (pBITMAPHANDLE pBitmap,
                                        pBITMAPHANDLE L_FAR * ppBitmap,
                                        L_UINT32 uFlags);

L_INT EXT_FUNCTION L_ColorSeparateBitmap (pBITMAPHANDLE pBitmap,
                                           pBITMAPHANDLE L_FAR * ppBitmap,
                                           L_UINT32 uFlags);

L_INT EXT_FUNCTION L_DeskewBitmap (pBITMAPHANDLE pBitmap,
                                   L_INT32 L_FAR *pnAngle);

L_INT EXT_FUNCTION L_DeskewBitmap2 (pBITMAPHANDLE pBitmap,
                                    L_INT32 L_FAR *pnAngle,
									COLORREF crBack);

L_INT EXT_FUNCTION L_DespeckleBitmap (pBITMAPHANDLE pBitmap);

L_INT EXT_FUNCTION L_SmoothBitmap ( pBITMAPHANDLE pBitmap, 
                                    LPSMOOTH pSmooth,
                                    SMOOTHCALLBACK pfnCallback,
                                    L_VOID L_FAR * pUserData);


L_INT EXT_FUNCTION L_LineRemoveBitmap(   pBITMAPHANDLE pBitmap,
                                         LPLINEREMOVE pLineRemove,
                                         LINEREMOVECALLBACK pfnCallback,
                                         L_VOID L_FAR * pUserData);

L_INT EXT_FUNCTION L_BorderRemoveBitmap(   pBITMAPHANDLE pBitmap, 
                                           LPBORDERREMOVE pBorderRemove,
                                           BORDERREMOVECALLBACK pfnCallback,
                                           L_VOID L_FAR *pUserData);

L_INT EXT_FUNCTION L_InvertedTextBitmap(pBITMAPHANDLE pBitmap,
                                        LPINVERTEDTEXT pInvertedText,
                                        INVERTEDTEXTCALLBACK pfnCallback,
                                        L_VOID L_FAR *pUserData);

L_INT EXT_FUNCTION L_DotRemoveBitmap(
                                     pBITMAPHANDLE pBitmap,
                                     LPDOTREMOVE pDotRemove,
                                     DOTREMOVECALLBACK pfnCallback,
                                     L_VOID L_FAR *pUserData);


L_INT EXT_FUNCTION L_HolePunchRemoveBitmap(
                                           pBITMAPHANDLE pBitmap,
                                           LPHOLEPUNCH pHolePunch,
                                           HOLEPUNCHREMOVECALLBACK pfnCallback,
                                           L_VOID L_FAR *pUserData);


//L_StapleRemoveBitmap() reserved for future use
L_INT EXT_FUNCTION L_StapleRemoveBitmap(
                                           pBITMAPHANDLE pBitmap,
                                           LPSTAPLE pStaplePunch,
                                           STAPLEREMOVECALLBACK pfnCallback,
                                           L_VOID L_FAR *pUserData);

L_INT EXT_FUNCTION L_EmbossBitmap (pBITMAPHANDLE pBitmap,
                                    L_UINT uDirection,
                                    L_UINT uDepth);

L_INT EXT_FUNCTION L_GammaCorrectBitmap (pBITMAPHANDLE pBitmap,
                                          L_UINT uGamma);

L_INT EXT_FUNCTION L_GetAutoTrimRect(pBITMAPHANDLE pBitmap,
                                     L_UINT uThreshold,
                                     RECT L_FAR *pRect);

L_INT EXT_FUNCTION L_GetBitmapColorCount(pBITMAPHANDLE pBitmap,
                                         L_UINT32 L_FAR *puCount);

L_INT EXT_FUNCTION L_GetBitmapHistogram (pBITMAPHANDLE pBitmap,
                                          L_UINT32 L_FAR * pHistogram,
                                          L_UINT uChannel);

L_INT EXT_FUNCTION L_GetBitmapHistogramGray(pBITMAPHANDLE pBitmap, 
                                            L_UINT32 L_FAR * pHisto,  
                                            L_UINT32 uHistoLen);

L_INT EXT_FUNCTION L_GetMinMaxBits(pBITMAPHANDLE pBitmap,
                                  L_INT L_FAR*pLowBit,
                                  L_INT L_FAR*pHighBit);

L_INT EXT_FUNCTION L_GetMinMaxVal(pBITMAPHANDLE pBitmap,
                                  L_UINT L_FAR*pMinVal,
                                  L_UINT L_FAR*pMaxVal);

L_INT EXT_FUNCTION L_HistoContrastBitmap (pBITMAPHANDLE pBitmap,
                                           L_INT nChange);

L_INT EXT_FUNCTION L_HistoEqualizeBitmap (pBITMAPHANDLE pBitmap);

L_INT EXT_FUNCTION L_IntensityDetectBitmap (pBITMAPHANDLE pBitmap,
                                             L_UINT uLow,
                                             L_UINT uHigh);

L_INT EXT_FUNCTION L_InvertBitmap (pBITMAPHANDLE pBitmap);


L_INT EXT_FUNCTION L_MaxFilterBitmap(pBITMAPHANDLE pBitmap,
                                     L_UINT uDim);

L_INT EXT_FUNCTION L_MedianFilterBitmap (pBITMAPHANDLE pBitmap,
                                          L_UINT uDim);

L_INT EXT_FUNCTION L_MinFilterBitmap(pBITMAPHANDLE pBitmap,
                                     L_UINT uDim);

L_INT EXT_FUNCTION L_MosaicBitmap (pBITMAPHANDLE pBitmap,
                                    L_UINT uDim);

L_INT EXT_FUNCTION L_OilifyBitmap (pBITMAPHANDLE pBitmap,
                                     L_UINT uDim);

L_INT EXT_FUNCTION L_PosterizeBitmap (pBITMAPHANDLE pBitmap,
                                       L_UINT uLevels);

L_INT EXT_FUNCTION L_PicturizeBitmap(pBITMAPHANDLE pBitmap, 
                                    L_CHAR L_FAR*pszDirName, 
                                    L_UINT uFlags,
                                    L_INT nCellWidth,
                                    L_INT nCellHeight,
                                    PICTURIZECALLBACK pfnCallback,
                                    L_VOID L_FAR*pUserData);

L_INT EXT_FUNCTION L_RemapBitmapHue(
                                    pBITMAPHANDLE pBitmap,
                                    L_UCHAR L_FAR * pMask,
                                    L_UCHAR L_FAR * pHTable,
                                    L_UCHAR L_FAR * pSTable,
                                    L_UCHAR L_FAR * pVTable);


L_INT EXT_FUNCTION L_RemapBitmapIntensity (pBITMAPHANDLE pBitmap,
                                            L_UCHAR L_FAR * pLUT,
                                            L_UINT uChannel);

L_INT EXT_FUNCTION L_SharpenBitmap (pBITMAPHANDLE pBitmap,
                                     L_INT nSharpness);

L_INT EXT_FUNCTION L_SolarizeBitmap (pBITMAPHANDLE pBitmap,
                                     L_UINT uThreshold);

L_INT EXT_FUNCTION L_SpatialFilterBitmap (pBITMAPHANDLE pBitmap,
                                           LPSPATIALFLT pFilter);

L_INT EXT_FUNCTION L_StretchBitmapIntensity (pBITMAPHANDLE pBitmap);


L_INT EXT_FUNCTION L_WindowLevelBitmap( pBITMAPHANDLE pBitmap,
                                 L_INT nLowBit,
                                 L_INT nHighBit,
                                 RGBQUAD L_HUGE*pLUT,
                                 L_UINT32 uLUTLength,
                                 L_INT nOrderDst);

L_INT EXT_FUNCTION L_ContourFilterBitmap( pBITMAPHANDLE pBitmap,
                                          L_INT16 nThreshold,
                                          L_INT16 nDeltaDirection,
                                          L_INT16 nMaximumError,
                                          L_INT nOption);

#undef _HEADER_ENTRY_
#include "ltpck.h"

#endif                          /* _LTIMG_H_ */
