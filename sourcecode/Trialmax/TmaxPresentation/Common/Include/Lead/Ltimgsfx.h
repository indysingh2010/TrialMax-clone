/*************************************************************
   Ltimg.h - LEADTOOLS image processing library
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTIMGSFX_H)
#define LTIMGSFX_H

#if !defined(L_LTIMGSFX_API)
   #define L_LTIMGSFX_API
#endif // #if !defined(L_LTIMGSFX_API)

#include "Ltkrn.h"
#include "Lttyp.h"
#include "Ltdis.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

///****************************************************************
//   Enums/defines/macros/structures
//****************************************************************/

#if !defined(FOR_PRE_16_5)
//----------------L_IntelligentUpScaleBitmap/L_IntelligentDownScaleBitmap-----------------------
#define INTELLIGENTRESCALE_VERTHORZ               0
#define INTELLIGENTRESCALE_HORZVERT               1
#define INTELLIGENTRESCALE_NOPRESERVECOLOR       -1
#define INTELLIGENTRESCALE_NOREMOVECOLOR         -1
#define INTELLIGENTRESCALE_DEFAULTUPSCALINGFACTOR 4
#define INTELLIGENTRESCALE_NOUPSCALINGFACTOR      1
#endif //# !defined(FOR_PRE_16_5)

//----------------L_CylendricalBitmap Flags-----------------------
#define CYL_HORZ  0x0000
#define CYL_VERT  0x0001

//----------------L_BendingBitmap Flags-----------------------
#define BND_NORM  0x0000
#define BND_HORZ  0x0100
#define BND_VERT  0x0200

//----------------L_PolarBitmap Flags-----------------------
#define CART_TO_POL  0x0000
#define POL_TO_CART  0x0010

//----------------L_PixelateBitmap Flags-----------------------
#define PIX_MAX   0x0000
#define PIX_MIN   0x0001
#define PIX_AVR   0x0002
#define PIX_RECT  0x0010
#define PIX_RAD   0x0020
#define PIX_WFRQ  0x0100
#define PIX_WPER  0x0200
#define PIX_HFRQ  0x0400
#define PIX_HPER  0x0800

//----------------L_FreeHandWaveBitmap, L_RadWaveBitmap, and L_WaveBitmap Flags-----------------------
#define DIS_PER   0x0000
#define DIS_FRQ   0x0010

//----------------L_FreeHandShearBitmap Flags-----------------------
#define SHR_HORZ  0x0000
#define SHR_VERT  0x0010

//----------------L_WaveBitmap Flags-----------------------
#define WV_SIN    0x0000
#define WV_COS    0x0100
#define WV_SQUARE 0x0200
#define WV_TRIANG 0x0300

//----------------L_PlaneBendBitmap, L_PlaneBitmap Flags-----------------------
#define PLANE_FILL_CLR  0x0001
#define PLANE_NO_CHG    0x0002
#define PLANE_LEFT      0x0010
#define PLANE_RIGHT     0x0020
#define PLANE_UP        0x0040
#define PLANE_DOWN      0x0080

//----------------L_TunnelBitmap Flags-----------------------
#define TUN_FILL_CLR    0x0001
#define TUN_NO_CHG      0x0002
#define TUN_AXIS_WIDTH  0x0010
#define TUN_AXIS_HEIGHT 0x0020

//----------------L_FreePlaneBendBitmap Flags-----------------------
#define FPB_HORZ  0x0010
#define FPB_VERT  0x0020
#define FPB_VRHZ  0x0030

//----------------L_GlassEffectBitmap Flags-----------------------
#define GLASS_WFRQ   0x0001
#define GLASS_WPER   0x0002
#define GLASS_HFRQ   0x0010
#define GLASS_HPER   0x0020

//----------------L_LensFlareBitmap Flags-----------------------
#define LNS_TYPE_1   0x0000
#define LNS_TYPE_2   0x0001
#define LNS_TYPE_3   0x0002

//----------------L_LightBitmap Flags-----------------------
#define LGT_SPOTLIGHT   0x0000
#define LGT_DIRELIGHT   0x0001

//----------------L_DrawStarBitmap Flags-----------------------
#define STR_INSIDE   0x0000
#define STR_OUTSIDE  0x0001
#define STR_INNER    0x0010

//--------------L_AddShadowBitmap Flags---------------------
#define SHADOW_E        0
#define SHADOW_NE       1
#define SHADOW_N        2
#define SHADOW_NW       3
#define SHADOW_W        4
#define SHADOW_SW       5
#define SHADOW_S        6
#define SHADOW_SE       7

#define SHADOW_CLR_RGB  0
#define SHADOW_CLR_GRAY 1

//-------------- L_AgingBitmap Flags---------------------
#define AGING_ADD_NOTHING  0x0000
#define AGING_ADD_VSCRATCH 0x0001
#define AGING_ADD_HSCRATCH 0x0002
#define AGING_ADD_DUST     0x0004
#define AGING_ADD_PITS     0x0008
#define AGING_SCRATCH_INV  0x0000
#define AGING_SCRATCH_CLR  0x0010
#define AGING_DUST_INV     0x0000
#define AGING_DUST_CLR     0x0020
#define AGING_PITS_INV     0x0000
#define AGING_PITS_CLR     0x0040

//---------------- L_CanvasBitmap -------------------------------
#define CANVAS_FIT   0x0000
#define CANVAS_SHIFT 0x0010

//---------------- L_FunctionalLightBitmap Flags-----------------------
#define FL_LINEAR_QUADRATIC   0x1000
#define FL_TRIGONOMETRY       0x2000
#define FL_FREEHAND           0x4000

#define FL_LINEAR_INNER       0x0001
#define FL_LINEAR_OUTER       0x0002
#define FL_QUADRATIC_INNER    0x0004
#define FL_QUADRATIC_OUTER    0x0008

#define FL_UNIDIRECTION       0x0010
#define FL_CIRCLES            0x0020
#define FL_ADD                0x0040
#define FL_MUL                0x0080

//----------------L_PuzzleEffectBitmap Flags-----------------------
#define PUZZLE_BORDER   0x0001
#define PUZZLE_SHUFFLE  0x0002
#define PUZZLE_SIZE     0x0010
#define PUZZLE_COUNT    0x0020

//---------------- L_RingEffectBitmap Flags-----------------------
#define RING_COLOR      0x0000
#define RING_REPEAT     0x0001
#define RING_NOCHANGE   0x0002
#define RING_FIXEDANGLE 0x0010
#define RING_RADIUS     0x0100
#define RING_MAXRADIUS  0x0200

//----------------L_DiceEffectBitmap Flags-----------------------
#define DICE_SIZE       0x0010
#define DICE_COUNT      0x0020
#define DICE_BORDER     0x0001

#define BITMAP_RESIZE   0x0100   // Used for the above 3 functions.

//---------------- L_BricksTextureBitmap ------------------------
#define BRICKS_SOLID       0x0000
#define BRICKS_SMOOTHEDOUT 0x0001
#define BRICKS_SMOOTHEDIN  0x0002

#define BRICKS_TRANSPARENTMORTAR 0x0000
#define BRICKS_COLOREDMORTAR     0x0010

//---------------- L_CloudsEffect ID's -----------------------
#define CLD_PURE        0x0000
#define CLD_DIFFERENCE  0x0001
#define CLD_OPACITY     0x0002

//---------------- L_VignetteBitmap ID's -----------------------
#define VIG_SQUARE      0x0000
#define VIG_RECTANGLE   0x0001
#define VIG_CIRCLE      0x0002
#define VIG_ELLIPSE     0x0003

#define VIG_FILLIN      0x0000
#define VIG_FILLOUT     0x0010

//---------------- L_MosaicTiles ID's -----------------------
#define MSCT_CART       0x0000
#define MSCT_POLAR      0x0001
#define MSCT_FLAT       0x0000
#define MSCT_SHADOWRGB  0x0010
#define MSCT_SHADOWGRAY 0x0020

//---------------- L_RomanMosaic ID's -----------------------
#define RMN_RECT        0x0000
#define RMN_CIRC        0x0001
#define RMN_BOTH        0x0002
#define RMN_FLAT        0x0000
#define RMN_SHADOWRGB   0x0010 
#define RMN_SHADOWGRAY  0x0020

//---------------- Plasma Data  -----------------------
#define PLSCLR_HUE         0x0000
#define PLSCLR_RGB1        0x0010
#define PLSCLR_RGB2        0x0020
#define PLSCLR_CUST        0x0030

#define PLSTYP_VERTICAL    0x0000
#define PLSTYP_HORIZONTAL  0x0001
#define PLSTYP_DIAGONAL    0x0002
#define PLSTYP_CROSS       0x0003
#define PLSTYP_CIRCULAR    0x0004
#define PLSTYP_RANDOM1     0x0005
#define PLSTYP_RANDOM2     0x0006

#define PLSCLR_TEST        0x00F0
#define PLSTYP_TEST        0x000F
#define PERCISION          100

//----------------L_ZigZagBitmap Flags-----------------------
#define ZG_RAD    0x0000
#define ZG_POND   0x0010

//---------------- L_PerlinBitmap   -----------------------
#define PRL_PURE        0x00
#define PRL_COMBINE     0x01
#define PRL_DIFFERENCE  0x02

#define PRL_CIRCLE      0x00
#define PRL_LINE        0x10
#define PRL_RANDOM      0x20

//---------------- L_ColorBalls   -----------------------
#define CLRBALLS_SHADING_SINGLE     0x0001
#define CLRBALLS_SHADING_LEFTRIGHT  0x0002
#define CLRBALLS_SHADING_TOPBOTTOM  0x0003
#define CLRBALLS_SHADING_CIRCULAR   0x0004
#define CLRBALLS_SHADING_ELLIPTICAL 0x0005
#define CLRBALLS_STICKER            0x0010
#define CLRBALLS_BALL               0x0020
#define CLRBALLS_IMAGE              0x0100
#define CLRBALLS_COLOR              0x0200
#define CLRBALLS_BALLCLR_MASK       0x1000
#define CLRBALLS_BALLCLR_OPACITY    0x2000

//---------------- L_Perspective   -----------------------
#define PERSPECTIVE_IMAGE  0x0001
#define PERSPECTIVE_COLOR  0x0002

//---------------- L_Pointilist   -----------------------
#define POINTILLIST_IMAGE     0x0001
#define POINTILLIST_COLOR     0x0002
#define POINTILLIST_STICKER   0x0010
#define POINTILLIST_POINT     0x0020

//---------------- L_ColoredPencilBitmapExt -----------------------
#define ARTISTIC_COLOREDPENCIL    0x01
#define DIRECTIONAL_COLOREDPENCIL 0x02
#define COMBINE_ORIGINAL          0x10

//********************** L_BumpMapBitmap Structure**********************
typedef struct _BUMPDATA
{
   L_UINT   uStructSize;
   L_INT    nAzimuth;
   L_UINT   uElevation;
   L_UINT   uDepth;
   L_INT    nXOffset;
   L_INT    nYOffset;
   L_INT    nXDst;
   L_INT    nYDst;
   L_BOOL   bTile;
   L_INT    nBright;
   L_INT    nIntensity;
   L_UINT   *pLut;
}
BUMPDATA, *pBUMPDATA;

//********************** L_LightBitmap Structure**********************
typedef struct _LIGHTINFO
{
   L_UINT   uStructSize;
   POINT    ptCenter;
   L_UINT   uWidth;
   L_UINT   uHeight;
   L_INT    nAngle;
   L_UINT   uBright;
   L_UINT   uEdge; 
   COLORREF crFill;
   L_UINT   uOpacity;
   L_UINT   uFlag;
}
LIGHTINFO, *pLIGHTINFO;

//********************** L_DrawStarBitmap Structure**********************
typedef struct _STARINFO
{
   L_UINT   uStructSize;
   POINT    pCenter;
   L_UINT   uSpoke;
   L_UINT   uStarWidth;
   L_UINT   uStarHeight;
   L_UINT   uHoleSize;
   L_INT    nPhase;
   L_INT    nAngle;
   L_INT    nDistOpac;
   L_INT    nSpokeDiv;
   L_INT    nAngleOpac;
   L_INT    nBorderOpac;
   COLORREF crFillLower;
   COLORREF crFillUpper;
   L_UINT   uOpacity;
   L_UINT   uFlag;
}
STARINFO, *pSTARINFO;

//********************** L_FunctionalLightBitmap Structure**********************
typedef struct _LIGHTPARAMS
{ 
   L_UINT   uStructSize;
   L_UINT   uFreq;
   L_UINT   uRAmp;
   L_UINT   uGAmp;
   L_UINT   uBAmp;
   L_INT    nAngle;
   L_UINT   uXOrigin;
   L_UINT   uYOrigin;
   L_INT    nPhase;
   L_INT    *pBuff;
   L_UINT   uBuffCount;
   L_UINT   uFlags;
}
LIGHTPARAMS, *pLIGHTPARAMS;

//---------------- L_VignetteBitmap ID's -----------------------
typedef struct _tagVIGNETTEINFO
{
   L_UINT   uStructSize;   // The size of the structure
   POINT    ptCenter;      // Center for the Vignette object
   L_INT    nFading;       // Fading percentage (0 - 100)
   L_UINT   uFadingRate;   // power of fading
   L_UINT   uWidth;        // Width for Squares,Rectangles, and Ellipses
   L_UINT   uHeight;       // Height for Squares,Rectangles, and Ellipses
   COLORREF crVigColor;    // The color used
   L_UINT   uFlags;        // Vignette Types
}
VIGNETTEINFO,*pVIGNETTEINFO;

//---------------- L_MosaicTiles ID's -----------------------
typedef struct _tagMOSAICTILESINFO
{
   L_UINT   uStructSize;      // The size of the structure
   L_UINT   uOpacity;         // Opacity between the image and the enterd color
   L_UINT   uPenWidth;        // Pen width
   L_UINT   uTileWidth;       // Block Width in cartesian case
   L_UINT   uTileHeight;      // Block Height in cartesian case
   POINT    ptCenter;         // Center point for the polar coordinates system
   L_UINT   uFlags;           // Flags
   L_UINT   uShadowAngle;     // Shadow Angle
   L_UINT   uShadowThreshold; // Shadow Threshold
   COLORREF crBorderColor;    // Border color
   COLORREF crTilesColor;     // Tile color
}
MOSAICTILESINFO,*pMOSAICTILESINFO;

//---------------- Plasma Data  -----------------------
typedef struct _PLASMAINFO
{
   L_UINT   uStructSize;
   L_UINT   uRedFreq;
   L_UINT   uGreenFreq;
   L_UINT   uBlueFreq;
   L_UINT   uOpacity;
   L_UINT   uShift;
   L_UINT   uSize;
   L_UINT   uFlags;
}
PLASMAINFO, *pPLASMAINFO;

typedef struct _MHBRGB
{
   L_UINT   uRed;
   L_UINT   uGreen;
   L_UINT   uBlue;
}
MHBRGB, *pMHBRGB;

typedef struct _MHBHSV
{
   L_DOUBLE dHue;
   L_DOUBLE dSat;
   L_DOUBLE dVal;
}
MHBHSV, *pMHBHSV;

///****************************************************************
//   Callback typedefs
//****************************************************************/
#if !defined(FOR_MANAGED) || defined(FOR_MANAGED_IMGSFX)


#if !defined(FOR_PRE_16_5)
L_LTIMGSFX_API L_INT EXT_FUNCTION L_IntelligentUpScaleBitmap(pBITMAPHANDLE  pBitmap,
                                                             pBITMAPHANDLE  pMaskBitmap,
                                                             COLORREF       crRemoveObjectColor,
                                                             COLORREF       crPreserveObjectColor,
                                                             L_INT          nNewWidth,
                                                             L_INT          nWidthUpScalingFactor,
                                                             L_INT          nNewHeight,
                                                             L_INT          nHeightUpScalingFactor,
                                                             L_INT          nUpScalingOrder,
                                                             L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_IntelligentDownScaleBitmap(pBITMAPHANDLE  pBitmap,
                                                               pBITMAPHANDLE  pMaskBitmap,
                                                               COLORREF       crRemoveObjectColor,
                                                               COLORREF       crPreserveObjectColor,
                                                               L_INT          nNewWidth,
                                                               L_INT          nNewHeight,
                                                               L_INT          nDownScalingOrder,
                                                               L_UINT32       uFlags);
#endif // !defined(FOR_PRE_16_5)

#if !defined(FOR_WINCE)

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PixelateBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uCellWidth,
   L_UINT         uCellHeight,
   L_UINT         uOpacity,
   POINT          CenterPt,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_SpherizeBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nValue,
   POINT          CenterPt,
   COLORREF       crFill,
   L_UINT         uFlags);


L_LTIMGSFX_API L_INT EXT_FUNCTION L_BendingBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nValue,
   POINT          CenterPt,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PunchBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nValue,
   L_UINT         uStress,
   POINT          CenterPt,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PolarBitmap(
   pBITMAPHANDLE  pBitmap,
   COLORREF       crFill,
   L_UINT         uFlags);


L_LTIMGSFX_API L_INT EXT_FUNCTION L_RippleBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uAmplitude,
   L_UINT         uFrequency,
   L_INT          nPhase,
   L_UINT         uAttenuation,
   POINT          CenterPt,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_FreeHandWaveBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pAmplitudes,
   L_UINT         uAmplitudesCount,
   L_UINT         uScale,
   L_UINT         uWaveLen,
   L_INT          nAngle,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_RadWaveBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uAmplitude,
   L_UINT         uWaveLen,
   L_INT          nPhase,
   POINT          pCenter,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_FreeHandShearBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pAmplitudes,
   L_UINT         uAmplitudesCount,
   L_UINT         uScale,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_WaveBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uAmplitude,
   L_UINT         uWaveLen,
   L_INT          nAngle,
   L_UINT         uHorzFact,
   L_UINT         uVertFact,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ZoomWaveBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uAmplitude,
   L_UINT         uFrequency,
   L_INT          nPhase,
   L_UINT         uZomFact,
   POINT          CenterPt,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_GlassEffectBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uCellWidth,
   L_UINT         uCellHeight,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_LensFlareBitmap(
   pBITMAPHANDLE  pBitmap,
   POINT          ptCenter,
   L_UINT         uBright,
   L_UINT         uFlags,
   COLORREF       crColor);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PlaneBitmap(
   pBITMAPHANDLE  pBitmap,
   POINT          ptCenterPoint,
   L_UINT         uZValue,
   L_INT          nDistance,
   L_UINT         uPlaneOffset,
   L_INT          nRepeat,
   L_INT          nPydAngle,
   L_UINT         uStretch,
   L_UINT         uStartBright,
   L_UINT         uEndBright,
   L_UINT         uBrightLength,
   COLORREF       crBright,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PlaneBendBitmap(
   pBITMAPHANDLE  pBitmap,
   POINT          ptCenterPoint,
   L_UINT         uZValue,
   L_INT          nDistance,
   L_UINT         uPlaneOffset,
   L_INT          nRepeat,
   L_INT          nPydAngle,
   L_UINT         uStretch,
   L_UINT         uBendFactor,
   L_UINT         uStartBright,
   L_UINT         uEndBright,
   L_UINT         uBrightLength,
   COLORREF       crBright,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_TunnelBitmap(
   pBITMAPHANDLE  pBitmap,
   POINT          ptCenterPoint,
   L_UINT         uZValue,
   L_INT          nDistance,
   L_UINT         uRad,
   L_INT          nRepeat,
   L_INT          nRotationOffset,
   L_UINT         uStretch,
   L_UINT         uStartBright,
   L_UINT         uEndBright,
   L_UINT         uBrightLength,
   COLORREF       crBright,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_FreeRadBendBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pnCurve,
   L_UINT         uCurveSize,
   L_UINT         uScale,
   POINT          CenterPt,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_FreePlaneBendBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          *pnCurve,
   L_UINT         uCurveSize,
   L_UINT         uScale,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_AddShadowBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uAngle,
   L_UINT         uThreshold,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_AgingBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uHScratchCount,
   L_UINT         uVScratchCount,
   L_UINT         uMaxScratchLen,
   L_UINT         uDustDensity,
   L_UINT         uPitsDensity,
   L_UINT         uMaxPitSize,
   COLORREF       crScratch,
   COLORREF       crDust,
   COLORREF       crPits,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_DiceEffectBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uXBlock,
   L_UINT         uYBlock,
   L_UINT         uRandomize,
   L_UINT         uFlags,
   COLORREF       crColor);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PuzzleEffectBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uXBlock,
   L_UINT         uYBlock,
   L_UINT         uRandomize,
   L_UINT         uFlags,
   COLORREF       crColor);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_RingEffectBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nXOrigin,
   L_INT          nYOrigin,
   L_UINT         uRadius,
   L_UINT         uRingCount,
   L_UINT         uRandomize,
   COLORREF       crColor,
   L_INT          nAngle,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_BricksTextureBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uBricksWidth,
   L_UINT         uBricksHeight,
   L_UINT         uOffsetX,
   L_UINT         uOffsetY,
   L_UINT         uEdgeWidth,
   L_UINT         uMortarWidth,
   L_UINT         uShadeAngle,
   L_UINT         uRowDifference,
   L_UINT         uMortarRoughness,
   L_UINT         uMortarRoughnessEevenness,
   L_UINT         uBricksRoughness,
   L_UINT         uBricksRoughnessEevenness,
   COLORREF       crMortarColor,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_CanvasBitmap(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pThumbBitmap,
   L_UINT         uTransparency,
   L_UINT         uEmboss,
   L_INT          nXOffset,
   L_INT          nYOffset,
   L_UINT         uTilesOffset,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_CloudsBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uSeed,
   L_UINT         uFrequency,
   L_UINT         uDensity,
   L_UINT         uOpacity,
   COLORREF       cBackColor,
   COLORREF       crCloudsColor,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_RomanMosaicBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uTileWidth,
   L_UINT         uTileHeight,
   L_UINT         uBorder,
   L_UINT         uShadowAngle,
   L_UINT         uShadowThresh,
   COLORREF       crColor,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PerlinBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uSeed,
   L_UINT         uFrequency,
   L_UINT         uDensity,
   L_UINT         uOpacity,
   COLORREF       cBackColor,
   COLORREF       Perlin,
   L_INT          nxCircle,
   L_INT          nyCircle,
   L_INT          nFreqLayout,
   L_INT          nDenLayout,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ZigZagBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uAmplitude,
   L_UINT         uAttenuation,
   L_UINT         uFrequency,
   L_INT          nPhase,
   POINT          CenterPt,
   COLORREF       crFill,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ColoredBallsBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uNumBalls,
   L_UINT         uSize,
   L_UINT         uSizeVariation,
   L_INT          nHighLightAng,
   COLORREF       crHighLight,
   COLORREF       crBkgColor,
   COLORREF       crShadingColor,
   COLORREF       *pBallColors,
   L_UINT         uNumOfBallColors,
   L_UINT         uAvrBallClrOpacity,
   L_UINT         uBallClrOpacityVariation,
   L_UINT         uRipple,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PerspectiveBitmap(
   pBITMAPHANDLE  pBitmap,
   POINT          *pPoints,
   COLORREF       crBkgColor,
   L_UINT         uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PointillistBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uSize,
   COLORREF       crColor,
   L_UINT         uFlags);

#if defined (LEADTOOLS_V16_OR_LATER)

L_LTIMGSFX_API L_INT EXT_FUNCTION L_CylindricalBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nValue,
   L_UINT         uType,
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_RadialBlurBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim,
   L_UINT         uStress,
   POINT          CenterPt, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_SwirlBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nAngle,
   POINT          CenterPt, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ZoomBlurBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim,
   L_UINT         uStress,
   POINT          CenterPt, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ImpressionistBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uHorzDim,
   L_UINT         uVertDim, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_WindBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim,
   L_INT          nAngle,
   L_UINT         uOpacity, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_BumpMapBitmap(
   pBITMAPHANDLE  pBitmapDst,
   pBITMAPHANDLE  pBumpBitmap,
   pBUMPDATA      pBumpData, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_GlowFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim,
   L_UINT         uBright,
   L_UINT         uThreshold, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_OceanBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uAmplitude,
   L_UINT         uFrequency,
   L_BOOL         bLowerTrnsp, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_LightBitmap(
   pBITMAPHANDLE  pBitmap,
   pLIGHTINFO     pLightInfo,
   L_UINT         uLightNo,
   L_UINT         uBright,
   L_UINT         uAmbient,
   COLORREF       crAmbientClr, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_DryBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_DrawStarBitmap(
   pBITMAPHANDLE  pBitmap,
   pSTARINFO      pStarInfo, 
   L_UINT32 uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_RevEffectBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uLineSpace,
   L_UINT         uMaximumHeight, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_FunctionalLightBitmap(
   pBITMAPHANDLE  pBitmap,
   pLIGHTPARAMS   pLightParams, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_FragmentBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uOffset,
   L_UINT         uOpacity, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_VignetteBitmap(
   pBITMAPHANDLE  pBitmap,
   pVIGNETTEINFO  pVignetteInfo, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_MosaicTilesBitmap(
   pBITMAPHANDLE     pBitmap,
   pMOSAICTILESINFO  pMosaicTilesInfo, 
   L_UINT32          uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PlasmaFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   pPLASMAINFO    pPlasmaInfo, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ColoredPencilBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uRatio,
   L_UINT         uDim, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_DiffuseGlowBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nGlowAmount,
   L_UINT         uClearAmount,
   L_UINT         uSpreadAmount,
   L_UINT         uWhiteNoise,
   COLORREF       crGlowColor, 
   L_UINT32       uFlags);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ColoredPencilBitmapExt(pBITMAPHANDLE pBitmap,
                                                           L_UINT uSize,
                                                           L_UINT uStrength,
                                                           L_UINT uThreshold,
                                                           L_UINT uPencilRoughness,
                                                           L_UINT uStrokeLength,
                                                           L_UINT uPaperRoughness,
                                                           L_INT  nAngle,
                                                           L_UINT uFlags);

#else

L_LTIMGSFX_API L_INT EXT_FUNCTION L_CylindricalBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nValue,
   L_UINT         uType);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_RadialBlurBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim,
   L_UINT         uStress,
   POINT          CenterPt);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_SwirlBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nAngle,
   POINT          CenterPt);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ZoomBlurBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim,
   L_UINT         uStress,
   POINT          CenterPt);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ImpressionistBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uHorzDim,
   L_UINT         uVertDim);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_WindBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim,
   L_INT          nAngle,
   L_UINT         uOpacity);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_BumpMapBitmap(
   pBITMAPHANDLE  pBitmapDst,
   pBITMAPHANDLE  pBumpBitmap,
   pBUMPDATA      pBumpData);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_GlowFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim,
   L_UINT         uBright,
   L_UINT         uThreshold);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_OceanBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uAmplitude,
   L_UINT         uFrequency,
   L_BOOL         bLowerTrnsp);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_LightBitmap(
   pBITMAPHANDLE  pBitmap,
   pLIGHTINFO     pLightInfo,
   L_UINT         uLightNo,
   L_UINT         uBright,
   L_UINT         uAmbient,
   COLORREF       crAmbientClr);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_DryBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uDim);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_DrawStarBitmap(
   pBITMAPHANDLE  pBitmap,
   pSTARINFO      pStarInfo);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_RevEffectBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uLineSpace,
   L_UINT         uMaximumHeight);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_FunctionalLightBitmap(
   pBITMAPHANDLE  pBitmap,
   pLIGHTPARAMS   pLightParams);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_FragmentBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uOffset,
   L_UINT         uOpacity);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_VignetteBitmap(
   pBITMAPHANDLE  pBitmap,
   pVIGNETTEINFO  pVignetteInfo);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_MosaicTilesBitmap(
   pBITMAPHANDLE     pBitmap,
   pMOSAICTILESINFO  pMosaicTilesInfo);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_PlasmaFilterBitmap(
   pBITMAPHANDLE  pBitmap,
   pPLASMAINFO    pPlasmaInfo);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_ColoredPencilBitmap(
   pBITMAPHANDLE  pBitmap,
   L_UINT         uRatio,
   L_UINT         uDim);

L_LTIMGSFX_API L_INT EXT_FUNCTION L_DiffuseGlowBitmap(
   pBITMAPHANDLE  pBitmap,
   L_INT          nGlowAmount,
   L_UINT         uClearAmount,
   L_UINT         uSpreadAmount,
   L_UINT         uWhiteNoise,
   COLORREF       crGlowColor
);

#endif //LEADTOOLS_V16_OR_LATER

#endif // #if !defined(FOR_WINCE)

#endif // #if !defined(FOR_MANAGED) || defined(FOR_MANAGED_IMGSFX)

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTIMG_H)
