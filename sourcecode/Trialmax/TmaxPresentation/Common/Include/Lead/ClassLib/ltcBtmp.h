/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcbtmap.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_BITMAP_H_
#define  _LEAD_BITMAP_H_  


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LBitmap                                                         |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 27 may 1998                                                     |
+----------------------------------------------------------------------------*/
#if !defined (LEADTOOLS_V16_OR_LATER)

class LWRP_EXPORT LBitmap:public LBitmapBase
{
   LEAD_DECLAREOBJECT(LBitmap);
   LEAD_DECLARE_CLASS_MAP();

   public:
      L_VOID *m_extLBitmap;
      

private:
   //BITMAPHANDLE BitmapRegion;       //Used for Document Imaging functions
   //LBitmapBase *pBitmapRgn;      //Used for Document Imaging functions


   static L_INT EXT_CALLBACK PicturizeCS(
      pBITMAPHANDLE pCellBitmap,
      L_INT x,L_INT y,
      L_VOID * pUserData
      );
   
   static L_INT EXT_CALLBACK SmoothCS(  
      L_UINT32       uBumpOrNick,
      L_INT32        iStartRow, 
      L_INT32        iStartCol, 
      L_INT32        iLength,
      L_UINT32       uHorV,
      L_VOID         *pUserData
      );
   
   static L_INT EXT_CALLBACK LineRemoveCS(
      HRGN           hRgn, 
      L_INT32        iStartRow, 
      L_INT32        iStartCol, 
      L_INT32        iLength, 
      L_VOID         *pUserData
      );
   
   static L_INT EXT_CALLBACK BorderRemoveCS(
      HRGN          hRgn, 
      L_UINT32      uBorderToRemove, 
      PRECT         pBoundingRect, 
      L_VOID        *pUserData
      );
   
   static L_INT EXT_CALLBACK InvertedTextCS(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount, 
      L_VOID        *pUserData
      );
   
   static L_INT EXT_CALLBACK DotRemoveCS(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount, 
      L_VOID        *pUserData
      );
   
   static L_INT EXT_CALLBACK HolePunchRemoveCS(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iHoleIndex,  
      L_INT32       iHoleTotalCount, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount, 
      L_VOID        *pUserData
      );

   static L_INT EXT_CALLBACK SliceCS(pBITMAPHANDLE  pBitmap, 
      LPRECT         lpSliceRect, 
      L_INT          nAngle, 
      L_VOID       * pUserData);

protected : 
   virtual L_INT PicturizeCallBack(
      pBITMAPHANDLE pCellBitmap,
      L_INT x,L_INT y
      );
   
   virtual L_INT SmoothCallBack(  
      L_UINT32       uBumpOrNick,
      L_INT32        iStartRow, 
      L_INT32        iStartCol, 
      L_INT32        iLength,
      L_UINT32       uHorV
      );
   
   virtual L_INT LineRemoveCallBack(
      HRGN           hRgn, 
      L_INT32        iStartRow, 
      L_INT32        iStartCol, 
      L_INT32        iLength
      );
   
   virtual L_INT BorderRemoveCallBack(
      HRGN          hRgn, 
      L_UINT32      uBorderToRemove, 
      PRECT         pBoundingRect
      );
   
   virtual L_INT InvertedTextCallBack(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount
      );
   
   virtual L_INT DotRemoveCallBack(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount
      );
   
   virtual L_INT HolePunchRemoveCallBack(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iHoleIndex,  
      L_INT32       iHoleTotalCount, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount
      );

   virtual L_INT SliceCallBack(LBitmapBase &Bitmap, 
      LPRECT         lpSliceRect, 
      L_INT          nAngle);

public : 
   LBitmap();
   LBitmap(
      L_UINT      uWidth,
      L_UINT      uHeight,
      L_UINT      uBitsPerPixel=24,
      L_UINT      uOrder=ORDER_BGR,
               LPRGBQUAD   pPalette=NULL,
               L_UINT      uViewPerspective=TOP_LEFT,
               COLORREF    crFill=0,
               L_UINT      uMemory=TYPE_CONV
             );
      LBitmap(
               BITMAPINFO *pInfo,
               L_UCHAR *  pBits
             );
      LBitmap(
               HDC hDC,
               HBITMAP hBitmap,
               HPALETTE hPalette
             );
      LBitmap(pBITMAPHANDLE pBitmapHandle);

      virtual ~LBitmap();
      LBitmap(LBitmap& LBitmapSrc);
      LBitmap&      operator =(LBitmap& LBitmapSrc); 

      virtual L_INT AddNoise(L_UINT uRange=500,L_UINT uChannel=CHANNEL_MASTER);
      virtual L_INT AutoTrim(L_INT uThreshold=0); 
      virtual L_INT AverageFilter(L_UINT uDim=3);
      virtual L_INT BinaryFilter(pBINARYFLT pFilter);
      virtual L_INT ChangeContrast(L_INT nChange);
      virtual L_INT ChangeHue(L_INT nAngle); 
      virtual L_INT ChangeIntensity(L_INT nChange);
      virtual L_INT ChangeSaturation(L_INT nChange);
      virtual L_INT ColorMerge(LBitmap * pLBitmaps, L_UINT uStructSize, L_UINT32  uFlags);
      virtual L_INT ColorSeparate(LBitmap * pLBitmaps, L_UINT uStructSize,L_UINT32 uFlags);
      virtual L_INT Deskew(L_INT32 * pnAngle, COLORREF crBack, L_UINT uFlags);
      L_INT DeskewCheck(L_INT32 * pnAngle, COLORREF crBack, L_UINT uFlags); 
      virtual L_INT Despeckle(); 
      
      virtual L_INT Smooth(pSMOOTH pSmooth);
      virtual L_INT LineRemove(pLINEREMOVE pLineRemove);
      virtual L_INT BorderRemove(pBORDERREMOVE pBorderRemove);
      virtual L_INT InvertedText(pINVERTEDTEXT pInvertedText);
      virtual L_INT DotRemove(pDOTREMOVE pDotRemove);
      virtual L_INT HolePunchRemove(pHOLEPUNCH pHolePunchRemove);
      
      virtual L_INT Emboss(L_UINT uDirection=EMBOSS_N,L_UINT uDepth=500);
      virtual L_INT GammaCorrect(L_UINT uGamma);
      virtual L_INT GetAutoTrimRect(L_UINT uThreshold,LPRECT pRect);
      virtual L_INT GetHistogram(L_UINT32 * pHisto, L_UINT uHistoLen, L_UINT uFlags);
      virtual L_INT Invert();    
      virtual L_INT HistoContrast(L_INT nChange);
      virtual L_INT IntensityDetect(L_UINT        uLow,L_UINT        uHigh,COLORREF      crInColor,COLORREF      crOutColor,L_UINT        uChannel);
      virtual L_INT MaxFilter(L_UINT uDim=3); 
      virtual L_INT MedianFilter(L_UINT uDim=3);
      virtual L_INT MinFilter(L_UINT uDim=3);
      virtual L_INT Oilify(L_UINT uDim=4);
      virtual L_INT Posterize(L_UINT uLevels=4);
      virtual L_INT Solarize(L_UINT uThreshold);
      virtual L_INT SpatialFilter(pSPATIALFLT pFilter);
      virtual L_INT StretchIntensity(); 
      virtual L_INT GetMinMaxBits(L_INT * pnLowBit,L_INT * pnHighBit);
      virtual L_INT GetMinMaxVal(L_INT * puMinVal,L_INT * puMaxVal);
      
      virtual L_INT RemapIntensity(L_INT * pLUT, L_UINT uLUTLen, L_UINT uFlags);
      
      virtual L_INT Mosaic(L_UINT uDim=3);
      virtual L_INT Sharpen(L_INT nSharpness);
      virtual L_INT Picturize(
                              L_TCHAR * pszDirName,
                              L_UINT uFlags,
                              L_INT nCellWidth,
                              L_INT nCellHeight
                             );
      virtual L_INT WindowLevel(
                                 L_INT nLowBit,
                                 L_INT nHighBit,
                                 RGBQUAD *pLUT,
                                 L_UINT uLUTLength,
                                 L_UINT uFlags
                               );
      virtual L_INT WindowLevelBitmap(
                                       L_INT nLowBit,
                                       L_INT nHighBit,
                                       RGBQUAD *pLUT,
                                       L_UINT uLUTLength,
                                       L_INT nOrderDst
                                     );

      static L_INT WindowLevelFillLUT(
                                    RGBQUAD * pLUT,
                                    L_UINT32 ulLUTLen,
                                    COLORREF crStart,
                                    COLORREF crEnd,
                                    L_INT nLow,
                                    L_INT nHigh,
                                    L_UINT uLowBit,
                                    L_UINT uHighBit,
                                    L_INT nMinValue,
                                    L_INT nMaxValue,
                                    L_INT  nFactor,
                                    L_UINT uFlags
                                    );

      virtual L_INT ContourFilter(
                                   L_INT16 nThreshold,
                                   L_INT16 nDeltaDirection,
                                   L_INT16 nMaximumError,
                                   L_INT nOption
                                   );

      virtual L_INT GaussianFilter(L_INT nRadius);
      
      virtual L_INT UnsharpMask(L_INT nAmount, L_INT nRadius, L_INT nThreshold, L_UINT uColorType);

      virtual L_INT GrayScaleExt(L_INT RedFact, L_INT GreenFact, L_INT BlueFact);

      virtual L_INT ConvertToColoredGray(L_INT RedFact, L_INT GreenFact, L_INT BlueFact, L_INT RedGrayFact, L_INT GreenGrayFact, L_INT BlueGrayFact);

      virtual L_INT BalanceColors(BALANCING * pRedFact, BALANCING * pGreenFact, BALANCING * pBlueFact);

      virtual L_INT SwapColors(L_INT nFlags);

      virtual L_INT LineProfile(POINT FirstPoint, POINT SecondPoint, L_INT ** ppRed, L_INT ** ppGreen, L_INT ** ppBlue);

      virtual L_INT HistoEqualize(L_INT nFlag);

      virtual L_INT AntiAlias(L_UINT uThreshold, L_UINT uDim, L_UINT uFilter);
      L_INT AntiAlias2(L_INT nThreshold, L_UINT uDim, L_UINT uFilter);

      virtual L_INT EdgeDetector(L_UINT uThreshold, L_UINT uFilter);
      L_INT EdgeDetector2(L_INT nThreshold, L_UINT uFilter);

      virtual L_INT RemoveRedEye(COLORREF rcNewColor, L_UINT uThreshold, L_INT nLightness);

      virtual L_INT MotionBlur(L_UINT uDim, L_INT nAngle, L_BOOL bUnidirectional);

      virtual L_INT PicturizeList(L_UINT uxDim, L_UINT uyDim, L_UINT uLightnessFact,
                                  LBitmapList * pBitmapList);

      virtual L_INT PicturizeSingle(LBitmapBase * pThumbBitmap,
                                    L_UINT uxDim, L_UINT uyDim,
                                    L_UINT uLightnessFact);

      virtual L_INT GetFunctionalLookupTable(L_INT * pLookupTable, L_UINT uLookupLen,
                                             L_INT nStart, L_INT nEnd,
                                             L_INT nFactor, L_UINT uFlags);

      virtual L_INT GetUserLookupTable(L_UINT * pLookupTable, L_UINT uLookupLen,
                                       POINT * apUserPoint, L_UINT uUserPointCount,
                                       L_UINT * puPointCount);

      virtual L_INT AddBorder(pADDBORDERINFO pAddBorderInfo);

      virtual L_INT AddFrame(pADDFRAMEINFO pAddFrameInfo);
      
      virtual L_INT Multiply(L_UINT uFactor);
      virtual L_INT RemapHue(L_UINT * pMask,L_UINT * pHTable,L_UINT * pSTable,L_UINT * pVTable,L_UINT uLUTLen);
      virtual L_INT AddWeighted(LBitmapList *pBitmapList,L_UINT * puFactor,L_UINT uFlags);
      virtual L_INT AddWeighted(HBITMAPLIST hBitmapList,L_UINT * puFactor,L_UINT uFlags);
      virtual L_INT LocalHistoEqualize(L_INT nWidthLen,L_INT nHeightLen,L_INT nxExt,L_INT nyExt,L_UINT uType,L_UINT uSmooth);
            
      virtual L_INT Pixelate(L_UINT uCellWidth,L_UINT uCellHeight,L_UINT uOpacity,POINT CenterPt, L_UINT uFlags);
      virtual L_INT Wind(L_UINT uDim,L_INT nAngle,L_UINT uOpacity);
      virtual L_INT Impressionist(L_UINT uHorzDim,L_UINT uVertDim);
      virtual L_INT Wave(L_UINT uAmplitude,L_UINT uWaveLen,L_INT nAngle,L_UINT uHorzFact,L_UINT uVertFact,COLORREF crFill,L_UINT uFlags);
      virtual L_INT ZoomWave(L_UINT uAmplitude,L_UINT uFrequency,L_INT nPhase,L_UINT uZomFact,POINT pCenter,COLORREF crFill,L_UINT uFlags);
      virtual L_INT RadWave(L_UINT uAmplitude,L_UINT   uWaveLen,L_INT    nPhase,POINT    pCenter,COLORREF crFill,L_UINT   uFlags);
      virtual L_INT FreeHandShear(L_INT *pAmplitudes,L_UINT uAmplitudesCount,L_UINT uScale,COLORREF crFill,L_UINT uFlags);
      virtual L_INT FreeHandWave(L_INT * pAmplitudes,L_UINT uAmplitudesCount,L_UINT uScale,L_UINT uWaveLen,L_INT nAngle,COLORREF crFill,L_UINT uFlags);
      virtual L_INT AddMessage(pADDMESGINFO pAddMesgInfo);
      virtual L_INT ExtractMessage(pADDMESGINFO pAddMesgInfo);
      virtual L_INT Spherize(L_INT nValue,POINT CenterPt,COLORREF crFill,L_UINT uFlags);
      virtual L_INT Cylindrical(L_INT nValue,L_UINT uType);
      virtual L_INT Bending(L_INT nValue,POINT CenterPt,COLORREF crFill,L_UINT uFlags);
      virtual L_INT Punch(L_INT nValue,L_UINT uStress,POINT CenterPt,COLORREF crFill,L_UINT uFlags);
      virtual L_INT Ripple(L_UINT uAmplitude,L_UINT uFrequency,L_INT nPhase,L_UINT uAttenuation,POINT CenterPt,COLORREF crFill,L_UINT uFlags);
      virtual L_INT Polar(COLORREF crFill,L_UINT uFlags);
      virtual L_INT RadialBlur(L_UINT uDim,L_UINT uStress,POINT CenterPt);
      static  L_INT GetCurvePoints(L_INT  * pCurve,POINT * apUserPoint,L_UINT uUserPointCount,L_UINT * puPointCount,L_UINT uFlags);
      virtual L_INT ZoomBlur(L_UINT uDim,L_UINT uStress,POINT CenterPt);
      virtual L_INT Swirl(L_INT nRotationAngle,POINT CenterPt);

      virtual L_INT Deinterlace(L_UINT uFlags);
      virtual L_INT SampleTarget(COLORREF crSample,COLORREF crTarget,L_UINT uFlags);
      virtual L_INT HalfTone(L_UINT uType,L_INT32 nAngle,L_UINT uDim,LBitmapList * pBitmapList);
      virtual L_INT HalfTone(L_UINT uType,L_INT32 nAngle,L_UINT uDim, HBITMAPLIST hBitmapList);
      virtual L_INT Cubism(L_UINT uSpace, L_UINT uLength, L_INT nBrightness, L_INT nAngle, COLORREF crColor, L_UINT uFlags);
      virtual L_INT LightControl(L_UINT * puLowerAvr, L_UINT * puAvrage, L_UINT * puUpperAvr, L_UINT uFlags);
      virtual L_INT GlassEffect(L_UINT uCellWidth, L_UINT uCellHeight, L_UINT uFlags);
      virtual L_INT LensFlare(POINT ptCenter, L_UINT uBright, L_UINT uFlags, COLORREF crColor);
      virtual L_INT BumpMap(LBitmap *pBumpBitmap, pBUMPDATA pBumpData);
      virtual L_INT BumpMap(pBITMAPHANDLE hBumpBitmap, pBUMPDATA pBumpData);
      virtual L_INT GlowFilter( L_UINT uDim, L_UINT uBright, L_UINT uThreshold);
      virtual L_INT EdgeDetectStatistical(L_UINT uDim, L_UINT uThreshold, COLORREF crEdgeColor, COLORREF crBkColor);
      L_INT EdgeDetectStatistical2(L_UINT uDim, L_INT nThreshold, COLORREF crEdgeColor, COLORREF crBkColor);
      virtual L_INT Desaturate();
      virtual L_INT SmoothEdges(L_UINT nAmount, L_UINT nThreshold);
      virtual L_INT AutoBinary();
      virtual L_INT ChannelMix (pCOLORDATA pRedFactor, pCOLORDATA pGreenFactor, pCOLORDATA pBlueFactor);
      virtual L_INT Plane(POINT ptCenterPoint, L_UINT uZValue, L_INT nDistance, L_UINT uPlaneOffset, L_INT nRepeat, L_INT nPydAngle, L_UINT uStretch, L_UINT uStartBright, L_UINT uEndBright, L_UINT uBrightLength, COLORREF crBright, COLORREF crFill, L_UINT uFlags);
      virtual L_INT PlaneBend(POINT ptCenterPoint, L_UINT uZValue, L_INT nDistance, L_UINT uPlaneOffset, L_INT nRepeat, L_INT nPydAngle, L_UINT uStretch, L_UINT uBendFactor, L_UINT uStartBright, L_UINT uEndBright, L_UINT uBrightLength, COLORREF crBright, COLORREF crFill, L_UINT uFlags);
      virtual L_INT Tunnel(POINT ptCenterPoint, L_UINT uZValue, L_INT nDistance, L_UINT nRad, L_INT nRepeat, L_INT nRotationOffset, L_UINT uStretch, L_UINT uStartBright, L_UINT uEndBright, L_UINT uBrightLength, COLORREF crBright, COLORREF crFill, L_UINT uFlags);
      virtual L_INT FreeRadBend(L_INT * pCurve, L_UINT uCurveSize, L_UINT uScale, POINT CenterPt, COLORREF crFill, L_UINT uFlags);
      virtual L_INT FreePlaneBend(L_INT * puCurve, L_UINT uCurveSize, L_UINT uScale, COLORREF crFill, L_UINT uFlags);
      virtual L_INT Ocean(L_UINT uAmplitude, L_UINT uFrequency, L_BOOL bLowerTrnsp);
      virtual L_INT Light(pLIGHTINFO pLightInfo, L_UINT uLightNo, L_UINT uBright, L_UINT uAmbient, COLORREF crAmbientClr);
      virtual L_INT Dry(L_UINT uDim);
      virtual L_INT DrawStar(pSTARINFO pStarInfo);

      virtual L_INT GrayScaleToDuotone(LPRGBQUAD pNewColor,COLORREF crColor,L_UINT uFlags);
      virtual L_INT GrayScaleToMultitone(L_UINT uToneType,L_UINT uDistType,LPCOLORREF pColor,LPRGBQUAD * pGradient,L_UINT uFlags);
      virtual L_INT Skeleton(L_UINT uThreshold);
      L_INT Skeleton2(L_INT nThreshold);
      virtual L_INT ColorLevel(pLVLCLR pLvlClr,L_UINT uFlags);
      virtual L_INT AutoColorLevel(pLVLCLR pLvlClr,L_UINT uBlackClip,L_UINT uWhiteClip,L_UINT uFlags);
      virtual L_INT SelectiveColor(pSELCLR pSelClr);
      virtual L_INT Correlation(pBITMAPHANDLE  pCorBitmap,
                           POINT          * pPoints,
                           L_UINT         uMaxPoints,
                           L_UINT         * puNumOfPoints,
                           L_UINT         uXStep,
                           L_UINT         uYStep,
                           L_UINT         uThreshold);

      virtual L_INT Correlation(LBitmapBase *plCorBitmap,
                           POINT        * pPoints,
                           L_UINT         uMaxPoints,
                           L_UINT       * puNumOfPoints,
                           L_UINT         uXStep,
                           L_UINT         uYStep,
                           L_UINT         uThreshold);
      
      virtual L_INT SetOverlay(L_INT nIndex,pBITMAPHANDLE pOverlayBitmap,  L_UINT uFlags);
      virtual L_INT SetOverlay(L_INT nIndex, LBitmapBase *  pOverlayBitmap,  L_UINT uFlags);
      
      virtual L_INT GetOverlay(L_INT nIndex,pBITMAPHANDLE pOverlayBitmap,L_UINT uStructSize,L_UINT uFlags);
      virtual L_INT GetOverlay(L_INT nIndex,LBitmapBase * pBitmap,L_UINT uStructSize,L_UINT uFlags);

      virtual L_INT SetOverlayAttributes(L_INT nIndex,
                                    pOVERLAYATTRIBUTES pOverlayAttributes,
                                    L_UINT uFlags);

      virtual L_INT GetOverlayAttributes(L_INT nIndex,
                                    pOVERLAYATTRIBUTES pOverlayAttributes,
                                    L_UINT uStructSize,
                                    L_UINT uFlags);

      virtual L_INT UpdateOverlayBits(L_INT nIndex,L_UINT uFlags);

      virtual L_UINT GetOverlayCount(L_UINT uFlags);
      

      virtual L_INT Scramble
                           (
                           L_INT32 nColStart,
                           L_INT32 nRowStart,
                           L_INT32 nWidth,
                           L_INT32 nHeight,
                           L_UINT32 uKey,
                           L_UINT uFlags
                           );

      virtual L_INT ApplyModalityLUT(
                                      L_UINT16              *pLUT,
                                      pDICOMLUTDESCRIPTOR   pLUTDescriptor,
                                      L_UINT                uFlags
                                     );

      virtual L_INT ApplyLinearModalityLUT( 
                                             L_DOUBLE       fIntercept, 
                                             L_DOUBLE       fSlope, 
                                             L_UINT         uFlags
                                          );

      virtual L_INT ApplyVOILUT(   
                                    L_UINT16      *      pLUT           ,
                                    pDICOMLUTDESCRIPTOR  pLUTDescriptor ,   
                                    L_UINT               uFlags
                               );

      virtual L_INT ApplyLinearVOILUT(   
                                          L_DOUBLE       fCenter     , 
                                          L_DOUBLE       fWidth      ,    
                                          L_UINT         uFlags
                                      );

      
      
      // Group 4
      virtual L_INT AddShadow(L_UINT uAngle,L_UINT uThreshold,L_UINT uFlags);
      virtual L_INT AllocFTArray(pFTARRAY * ppFTArray,L_UINT uStructSize);
      virtual L_INT ChangeHueSatInt(L_INT nHue,L_INT nSaturation,L_INT nIntensity,pHSIDATA pHsiData,L_UINT uHsiDataCount);
      virtual L_INT ColorReplace(pCOLORREPLACE pColorReplace,L_UINT uColorCount,L_INT nHue,L_INT nSaturation,L_INT nBrightness);
      virtual L_INT ColorThreshold(L_UINT uColorSpace,pCOMPDATA pCompData);
      virtual L_INT DFT(pFTARRAY pFTArray,RECT * prcRange,L_UINT uFlags);
      virtual L_INT DirectionEdgeStatistical(L_UINT uDim,L_UINT uThreshold,L_INT nAngle,COLORREF crEdgeColor,COLORREF crBkColor);      
      L_INT DirectionEdgeStatistical2(L_UINT uDim,L_INT nThreshold,L_INT nAngle,COLORREF crEdgeColor,COLORREF crBkColor);      
      virtual L_INT FFT(pFTARRAY pFTArray,L_UINT uFlags);
      static  L_INT FreeFTArray(pFTARRAY pFTArray);
      virtual L_INT FTDisplay(pFTARRAY pFTArray,L_UINT uFlags);
      static  L_INT FrqFilter(pFTARRAY pFTArray,LPRECT prcRange,L_UINT uFlags);
      virtual L_INT FrqFilterMask(pFTARRAY pFTArray,L_BOOL bOnOff);
      virtual L_INT GetStatisticsInfo(pSTATISTICSINFO pStatisticsInfo,L_UINT uChannel,L_UINT uStart,L_UINT uEnd);
      L_INT GetStatisticsInfo2(pSTATISTICSINFO pStatisticsInfo,L_UINT uChannel,L_INT nStart,L_INT nEnd);
      static  L_INT GetFeretsDiameter(POINT * pPoints,L_UINT uSize,L_UINT * puFeretsDiameter,L_UINT * puFirstIndex,L_UINT * puSecondIndex);
      virtual L_INT GetObjectInfo(L_UINT * puX,L_UINT * puY,L_INT * pnAngle,L_UINT * puRoundness,L_BOOL bWeighted);
      virtual L_INT GetRgnContourPoints(pRGNXFORM pXForm,POINT * * ppPoints,L_UINT * puSize, L_UINT uFlags);
      virtual L_INT GetRgnPerimeterLength(pRGNXFORM pXForm,L_SIZE_T* puLength);
      virtual L_INT MathFunction(L_UINT uMType,L_UINT uFactor);
      virtual L_INT RevEffect(L_UINT uLineSpace,L_UINT uMaximumHeight);
      virtual L_INT Segment(L_UINT uThreshold,L_UINT uFlags);
      virtual L_INT SubtractBackground(L_UINT uRollingBall,L_UINT uShrinkSize,L_UINT uBrightnessFactor,L_UINT uFlags);
      virtual L_INT UserFilter(pUSERFLT pFilter);
      // Group 5
      virtual L_INT AdaptiveContrast(L_UINT uDim,L_UINT uAmount,L_UINT uFlags);
      virtual L_INT Aging(L_UINT uHScratchCount,L_UINT uVScratchCount,L_UINT uMaxScratchLen,L_UINT uDustDensity,L_UINT uPitsDensity,L_UINT uMaxPitSize,COLORREF crScratch,COLORREF crDust,COLORREF crPits,L_UINT uFlags);
      virtual L_INT ApplyMathLogic(L_INT nFactor, L_UINT uFlags);
      virtual L_INT ColorIntensityBalance(pBALANCEDATA pShadows,pBALANCEDATA pMidTone,pBALANCEDATA pHighLight,L_BOOL bLuminance);
      virtual L_INT ColorizeGray(pBITMAPHANDLE pSrcBitmap,pLTGRAYCOLOR pGrayColors,L_UINT uCount);
      virtual L_INT ColorizeGray(LBitmapBase *plSrcBitmap,pLTGRAYCOLOR pGrayColors,L_UINT uCount);
      virtual L_INT ContBrightInt(L_INT nContrast,L_INT nBrightness, L_INT nIntensity);
      virtual L_INT DiceEffect(L_UINT uXBlock,L_UINT uYBlock,L_UINT uRandomize,L_UINT uFlags,COLORREF crColor);
      virtual L_INT DigitalSubtract(pBITMAPHANDLE pMaskBitmap,L_UINT uFlags);
      virtual L_INT DigitalSubtract(LBitmapBase * plMaskBitmap,L_UINT uFlags);
      virtual L_INT DynamicBinary(L_UINT uDim,L_UINT uLocalContrast);
      virtual L_INT EdgeDetectEffect(L_UINT uLevel,L_UINT uThreshold,L_UINT uFlags);
      virtual L_INT FunctionalLight(pLIGHTPARAMS pLightParams);
      virtual L_INT MultiScaleEnhancement(L_UINT uContrast,L_UINT uEdgeLevels,L_UINT uEdgeCoeff,L_UINT uLatitudeLevels,L_UINT uLatitudeCoeff,L_UINT uFlags);
      virtual L_INT PuzzleEffect(L_UINT uXBlock,L_UINT uYBlock,L_UINT uRandomize,L_UINT uFlags,COLORREF crColor);
      virtual L_INT RingEffect(L_INT nXOrigin,L_INT nYOrigin,L_UINT uRadius,L_UINT uRingCount,L_UINT uRandomize, COLORREF crColor,L_INT nAngle,L_UINT uFlags);
      virtual L_INT SelectData(pBITMAPHANDLE pDstBitmap,COLORREF crColor,L_UINT uSrcLowBit,L_UINT uSrcHighBit,L_UINT uThreshold,L_BOOL bCombine);
      virtual L_INT SelectData(LBitmapBase * plDstBitmap,COLORREF crColor,L_UINT uSrcLowBit,L_UINT uSrcHighBit,L_UINT uThreshold,L_BOOL bCombine);
      virtual L_INT ShiftData(pBITMAPHANDLE pSrcBitmap,L_UINT uSrcLowBit,L_UINT uSrcHighBit,L_UINT uDstLowBit,L_UINT uDstBitsPerPixel);
      virtual L_INT ShiftData(LBitmapBase * plSrcBitmap,L_UINT uSrcLowBit,L_UINT uSrcHighBit,L_UINT uDstLowBit,L_UINT uDstBitsPerPixel);
      virtual L_INT TextureAlphaBlend(L_INT nXDst,L_INT nYDst,L_INT nWidth,L_INT nHeight,pBITMAPHANDLE pBitmapSrc,L_INT nXSrc,L_INT nYSrc,pBITMAPHANDLE pBitmapMask,L_INT nOpacity,pBITMAPHANDLE pBitmapUnderlay,LPPOINT pOffset);
      virtual L_INT TextureAlphaBlend(L_INT nXDst,L_INT nYDst,L_INT nWidth,L_INT nHeight,LBitmapBase *plBitmapSrc,L_INT nXSrc,L_INT nYSrc,LBitmapBase *plBitmapMask,L_INT nOpacity,LBitmapBase *plBitmapUnderlay,LPPOINT pOffset);
      // Group 6
      virtual L_INT IsRegMark(L_UINT uType,L_UINT uMinScale,L_UINT uMaxScale,L_UINT uWidth,L_UINT uHeight);
      virtual L_INT GetMarksCenterMass(POINT * pMarkPoints,POINT * pMarkCMPoints,L_UINT uMarksCount);
      virtual L_INT SearchRegMarks(pSEARCHMARKS pSearchMarks,L_UINT uMarkCount);
      virtual L_INT GetTransformationParameters(POINT * pRefPoints,POINT * pTrnsPoints,L_INT * pnXTranslation,L_INT * pnYTranslation,L_INT * pnAngle,L_UINT * puXScale,L_UINT * puYScale);
      virtual L_INT ApplyTransformationParameters(L_INT nXTranslation,L_INT nYTranslation,L_INT nAngle,L_UINT uXScale,L_UINT uYScale,L_UINT uFlags);
      virtual L_INT        Add(LBitmapList * pBitmapList, L_UINT uFlags = BC_ADD);

      static  L_INT CountLUTColors(RGBQUAD * pLUT,L_UINT32 ulLLUTLen,L_UINT * pNumberOfEntries,L_INT  *  pFirstIndex,L_UINT   uFlags);
      virtual L_INT GetLinearVOILUT(L_DOUBLE *pCenter,L_DOUBLE *pWidth,L_UINT uFlags);
      virtual L_INT ConvertSignedToUnsigned(L_UINT uShift);
      virtual L_INT ConvertUnsignedToSigned(L_UINT uFlags);
      //Used for Document Imaging functions
      //virtual L_INT LBitmap::GetDocImageRgn(LBitmapBase *pDocImageRgn);

      L_INT Perlin (L_UINT32 uSeed, L_UINT uFrequency, L_UINT uIteration, L_UINT uOpacity,
         COLORREF crBClr, COLORREF crFClr, L_INT nxCircle, L_INT nyCircle, L_INT nFreqLayout,
         L_INT nDenLayout, L_UINT uFlags);

      L_INT ShiftMinimumToZero (L_UINT * puShiftAmount);

      L_INT ShiftZeroToNegative (L_INT nShiftAmount,L_INT nMinInput,L_INT nMaxInput,L_INT nMinOutput,L_INT nMaxOutput);

      L_INT ColoredPencil (L_UINT uRatio, L_UINT uDim);

      L_INT SetKaufmannRgn (pBITMAPHANDLE    pProcessedBitmap,
                                    L_INT            nRadius,
                                    L_INT            nMinInput, 
                                    L_INT            nMaxInput,
                                    L_INT            nRgnThreshold,
                                    POINT            ptRgnStart,
                                    L_BOOL           bRemoveHoles,
                                    L_SIZE_T       * puPixelsCount,
                                    L_UINT           uCombineMode);

      L_INT Canvas(LBitmapBase *plCanvasBitmap,
                     L_UINT uTransparency,
                     L_UINT uEmboss,
                     L_INT  nXOffset,
                     L_INT  nYOffset,
                     L_UINT uTilesOffset,
                     L_UINT uFlags);

      L_INT Canvas(pBITMAPHANDLE phCanvasBitmap,
                     L_UINT uTransparency,
                     L_UINT uEmboss,
                     L_INT  nXOffset,
                     L_INT nYOffset,
                     L_UINT uTilesOffset,
                     L_UINT uFlags);

      L_INT DisplaceMap(LBitmapBase *plDisplaceBitmap,
                           L_UINT uHorzFact,
                           L_UINT uVertFact,
                           COLORREF crFill,
                           L_UINT uFlags);

      L_INT DisplaceMap(pBITMAPHANDLE phDisplaceBitmap,
                          L_UINT uHorzFact,
                          L_UINT uVertFact,
                          COLORREF crFill,
                          L_UINT uFlags);

      L_INT CorrelationList(HBITMAPLIST    hCorList,
                              POINT  * pPoints,
                              L_UINT * puListIndex,
                              L_UINT uMaxPoints,
                              L_UINT * puNumOfPoints,
                              L_UINT uXStep,
                              L_UINT uYStep,
                              L_UINT uThreshold);

      L_INT Perspective (POINT * pPoints,COLORREF crBkgColor,L_UINT uFlags);

      L_INT ColoredBalls (L_UINT uNumBalls,
                           L_UINT uSize,
                           L_UINT  uSizeVariation,
                           L_INT nHighLightAng,
                           COLORREF crHighLight,
                           COLORREF crBkgColor,
                           COLORREF crShadingColor,
                           COLORREF * pBallColors,
                           L_UINT uNumOfBallColors,
                           L_UINT uAvrBallClrOpacity,
                           L_UINT uBallClrOpacityVariation,
                           L_UINT uRipple,
                           L_UINT uFlags);

      L_INT AdjustTint (L_INT nAngleA, L_INT nAngleB);

      L_INT HalfTonePattern (L_UINT uContrast,
                              L_UINT uRipple,
                              L_UINT uAngleContrast,
                              L_UINT uAngleRipple,
                              L_INT nAngleOffset,
                              COLORREF crForGround,
                              COLORREF crBackGround,
                              L_UINT uFlags);

      L_INT Pointillist (L_UINT uSize,
                           COLORREF crColor,
                           L_UINT uFlags);

      L_INT ColorHalfTone (L_UINT uMaxRad,
                              L_INT nCyanAngle,
                              L_INT nMagentaAngle,
                              L_INT nYellowAngle,
                              L_INT nBlackAngle);

      L_INT RomanMosaic (L_UINT uTileWidth,
                           L_UINT uTileHeight,
                           L_UINT uBorder,
                           L_UINT uShadowAngle,
                           L_UINT uShadowThresh,
                           COLORREF crColor,
                           L_UINT uFlags);

      L_INT Vignette (pVIGNETTEINFO pVignetteInfo);

      L_INT MosaicTiles (pMOSAICTILESINFO pMosaicTilesInfo);

      L_INT Fragment (L_UINT uOffset, L_UINT uOpacity);

      L_INT Clouds (L_UINT uSeed,
                     L_UINT uFrequency,
                     L_UINT uDensity,
                     L_UINT uOpacity,
                     COLORREF cBackColor,
                     COLORREF crCloudsColor,
                     L_UINT uFlags);

      L_INT Offset (L_INT nHorizontalShift,
                     L_INT nVerticalShift,
                     COLORREF crBackColor,
                     L_UINT uFlags);

      L_INT BricksTexture (L_UINT uBricksWidth,
                           L_UINT uBricksHeight,
                           L_UINT uOffsetX,
                           L_UINT uOffsetY,
                           L_UINT uEdgeWidth,
                           L_UINT uMortarWidth,
                           L_UINT uShadeAngle,
                           L_UINT uRowDifference,
                           L_UINT uMortarRoughness,
                           L_UINT uMortarRoughnessEevenness,
                           L_UINT uBricksRoughness,
                           L_UINT uBricksRoughnessEevenness,
                           COLORREF crMortarColor,
                           L_UINT uFlags);

      L_INT PlasmaFilter (pPLASMAINFO PlasmaInfo);

      L_INT MaskConvolution (L_INT nAngle,
                              L_UINT uDepth,
                              L_UINT uHeight,
                              L_UINT uFlags);

      L_INT GammaCorrectExt (L_UINT uGamma, L_UINT uFlags);

      L_INT HighPassFilter (L_UINT uRadius, L_UINT uOpacity);

      L_INT ZigZag (L_UINT uAmplitude,
                     L_UINT uAttenuation,
                     L_UINT uFrequency,
                     L_INT  nPhase,
                     POINT  CenterPt,
                     COLORREF crFill,
                     L_UINT uFlags);

      L_INT DeskewExt (L_INT32 *pnAngle,
                        L_UINT uAngleRange,
                        L_UINT uAngleResolution,
                        COLORREF crBack,
                        L_UINT uFlags);

      L_INT DiffuseGlow (L_INT nGlowAmount,
                           L_UINT uClearAmount,
                           L_UINT uSpreadAmount, 
                           L_UINT uWhiteNoise,
                           COLORREF crGlowColor);

      L_INT Slice(pSLICEBITMAPOPTIONS  pOptions,
                  L_INT32  * pnDeskewAngle);

      L_INT BlankPageDetector(L_BOOL * bIsBlank,
                              L_UINT * pAccuracy,
                              pPAGEMARGINS PMargins,
                              L_UINT uFlags);
};
#else
class LWRP_EXPORT LBitmap:public LBitmapBase
{
   LEAD_DECLAREOBJECT(LBitmap);
   LEAD_DECLARE_CLASS_MAP();

   public:
      L_VOID *m_extLBitmap;
      

private:
   //BITMAPHANDLE BitmapRegion;       //Used for Document Imaging functions
   //LBitmapBase *pBitmapRgn;      //Used for Document Imaging functions


   static L_INT EXT_CALLBACK PicturizeCS(
      pBITMAPHANDLE pCellBitmap,
      L_INT x,L_INT y,
      L_VOID * pUserData
      );
   
   static L_INT EXT_CALLBACK SmoothCS(  
      L_UINT32       uBumpOrNick,
      L_INT32        iStartRow, 
      L_INT32        iStartCol, 
      L_INT32        iLength,
      L_UINT32       uHorV,
      L_VOID         *pUserData
      );
   
   static L_INT EXT_CALLBACK LineRemoveCS(
      HRGN           hRgn, 
      L_INT32        iStartRow, 
      L_INT32        iStartCol, 
      L_INT32        iLength, 
      L_VOID         *pUserData
      );

   static L_INT EXT_CALLBACK RakeRemoveCS(
      L_HRGN      hRgn,
      L_INT       nLength,
      L_VOID      *pUserData
      );

   static L_INT EXT_CALLBACK ObjectCounterCS(
      L_RECT      rcRect,
      L_INT      **Object,
      L_VOID      *pUserData
      );


   static L_INT EXT_CALLBACK BorderRemoveCS(
      HRGN          hRgn, 
      L_UINT32      uBorderToRemove, 
      PRECT         pBoundingRect, 
      L_VOID        *pUserData
      );
   
   static L_INT EXT_CALLBACK InvertedTextCS(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount, 
      L_VOID        *pUserData
      );
   
   static L_INT EXT_CALLBACK DotRemoveCS(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount, 
      L_VOID        *pUserData
      );
   
   static L_INT EXT_CALLBACK HolePunchRemoveCS(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iHoleIndex,  
      L_INT32       iHoleTotalCount, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount, 
      L_VOID        *pUserData
      );

   static L_INT EXT_CALLBACK SliceCS(pBITMAPHANDLE  pBitmap, 
      LPRECT         lpSliceRect, 
      L_INT          nAngle, 
      L_VOID       * pUserData);

protected : 
   virtual L_INT PicturizeCallBack(
      pBITMAPHANDLE pCellBitmap,
      L_INT x,L_INT y
      );
   
   virtual L_INT SmoothCallBack(  
      L_UINT32       uBumpOrNick,
      L_INT32        iStartRow, 
      L_INT32        iStartCol, 
      L_INT32        iLength,
      L_UINT32       uHorV
      );
   
   virtual L_INT LineRemoveCallBack(
      HRGN           hRgn, 
      L_INT32        iStartRow, 
      L_INT32        iStartCol, 
      L_INT32        iLength
      );

   virtual L_INT RakeRemoveCallBack(
      HRGN           hRgn, 
      L_INT          nLength
      );

   virtual L_INT ObjectCounterCallBack(
      L_RECT      rcRect,
      L_INT      **Object
      );


   virtual L_INT BorderRemoveCallBack(
      HRGN          hRgn, 
      L_UINT32      uBorderToRemove, 
      PRECT         pBoundingRect
      );
   
   virtual L_INT InvertedTextCallBack(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount
      );
   
   virtual L_INT DotRemoveCallBack(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount
      );
   
   virtual L_INT HolePunchRemoveCallBack(
      HRGN          hRgn, 
      PRECT         pBoundingRect, 
      L_INT32       iHoleIndex,  
      L_INT32       iHoleTotalCount, 
      L_INT32       iWhiteCount, 
      L_INT32       iBlackCount
      );

   virtual L_INT SliceCallBack(LBitmapBase &Bitmap, 
      LPRECT         lpSliceRect, 
      L_INT          nAngle);

public : 
   LBitmap();
   LBitmap(
      L_UINT      uWidth,
      L_UINT      uHeight,
      L_UINT      uBitsPerPixel=24,
      L_UINT      uOrder=ORDER_BGR,
               LPRGBQUAD   pPalette=NULL,
               L_UINT      uViewPerspective=TOP_LEFT,
               COLORREF    crFill=0,
               L_UINT      uMemory=TYPE_CONV
             );
      LBitmap(
               BITMAPINFO *pInfo,
               L_UCHAR *  pBits
             );
      LBitmap(
               HDC hDC,
               HBITMAP hBitmap,
               HPALETTE hPalette
             );
      LBitmap(pBITMAPHANDLE pBitmapHandle);

      virtual ~LBitmap();
      LBitmap(LBitmap& LBitmapSrc);
      LBitmap&      operator =(LBitmap& LBitmapSrc); 

      virtual L_INT AddNoise(L_UINT uRange=500,L_UINT uChannel=CHANNEL_MASTER, L_UINT32 uFlags = 0);
      virtual L_INT AutoTrim(L_INT uThreshold=0, L_UINT32 uFlags = 0); 
      virtual L_INT AverageFilter(L_UINT uDim=3, L_UINT32 uFlags = 0);
      virtual L_INT BinaryFilter(pBINARYFLT pFilter, L_UINT32 uFlags = 0);
      virtual L_INT ChangeContrast(L_INT nChange, L_UINT32 uFlags = 0);
      virtual L_INT ChangeHue(L_INT nAngle, L_UINT32 uFlags = 0); 
      virtual L_INT ChangeIntensity(L_INT nChange, L_UINT32 uFlags = 0);
      virtual L_INT ChangeSaturation(L_INT nChange, L_UINT32 uFlags = 0);
      virtual L_INT ColorMerge(LBitmap * pLBitmaps, L_UINT uStructSize, L_UINT32  uFlags);
      virtual L_INT ColorSeparate(LBitmap * pLBitmaps, L_UINT uStructSize,L_UINT32 uFlags);
      virtual L_INT Deskew(L_INT32 * pnAngle, COLORREF crBack, L_UINT uFlags);
      L_INT DeskewCheck(L_INT32 * pnAngle, COLORREF crBack, L_UINT uFlags); 
      virtual L_INT Despeckle(L_UINT32 uFlags = 0); 
      
      virtual L_INT Smooth(pSMOOTH pSmooth, L_UINT32 uFlags = 0);
      virtual L_INT LineRemove(pLINEREMOVE pLineRemove, L_UINT32 uFlags = 0);
      virtual L_INT RakeRemove(L_BOOL bAuto, pRAKEREMOVE pRakeRemove,RECT* pDstRect, L_INT nRectCount, L_UINT32 uFlags = 0);
      virtual L_INT LBitmap::ObjectCounter(L_UINT *uCount, L_UINT32 uFlags);
      virtual L_INT BorderRemove(pBORDERREMOVE pBorderRemove, L_UINT32 uFlags = 0);
      virtual L_INT InvertedText(pINVERTEDTEXT pInvertedText, L_UINT32 uFlags = 0);
      virtual L_INT DotRemove(pDOTREMOVE pDotRemove, L_UINT32 uFlags = 0);
      virtual L_INT HolePunchRemove(pHOLEPUNCH pHolePunchRemove, L_UINT32 uFlags = 0);
      
      virtual L_INT Emboss(L_UINT uDirection=EMBOSS_N,L_UINT uDepth=500, L_UINT32 uFlags = 0);
      virtual L_INT GammaCorrect(L_UINT uGamma, L_UINT32 uFlags = 0);
      virtual L_INT GetAutoTrimRect(L_UINT uThreshold,LPRECT pRect, L_UINT32 uFlags = 0);
      virtual L_INT GetHistogram(L_UINT32 * pHisto, L_UINT uHistoLen, L_UINT uFlags);
      virtual L_INT Invert(L_UINT32 uFlags = 0);    
      virtual L_INT HistoContrast(L_INT nChange, L_UINT32 uFlags = 0);
      virtual L_INT IntensityDetect(L_UINT uLow,L_UINT uHigh,COLORREF crInColor,COLORREF crOutColor,L_UINT uChannel, L_UINT32 uFlags = 0);
      virtual L_INT MaxFilter(L_UINT uDim=3, L_UINT32 uFlags = 0); 
      virtual L_INT MedianFilter(L_UINT uDim=3, L_UINT32 uFlags = 0);
      virtual L_INT MinFilter(L_UINT uDim=3, L_UINT32 uFlags = 0);
      virtual L_INT Oilify(L_UINT uDim=4, L_UINT32 uFlags = 0);
      virtual L_INT Posterize(L_UINT uLevels=4, L_UINT32 uFlags = 0);
      virtual L_INT Solarize(L_UINT uThreshold, L_UINT32 uFlags = 0);
      virtual L_INT SpatialFilter(pSPATIALFLT pFilter, L_UINT32 uFlags = 0);
      virtual L_INT StretchIntensity(L_UINT32 uFlags = 0); 
      virtual L_INT GetMinMaxBits(L_INT * pnLowBit,L_INT * pnHighBit, L_UINT32 uFlags = 0);
      virtual L_INT GetMinMaxVal(L_INT * puMinVal,L_INT * puMaxVal, L_UINT32 uFlags = 0);
      
      virtual L_INT RemapIntensity(L_INT * pLUT, L_UINT uLUTLen, L_UINT uFlags);
      
      virtual L_INT Mosaic(L_UINT uDim=3, L_UINT32 uFlags = 0);
      virtual L_INT Sharpen(L_INT nSharpness, L_UINT32 uFlags = 0);
      virtual L_INT Picturize(
                              L_TCHAR * pszDirName,
                              L_UINT uFlags,
                              L_INT nCellWidth,
                              L_INT nCellHeight
                             );
      virtual L_INT WindowLevel(
                                 L_INT nLowBit,
                                 L_INT nHighBit,
                                 RGBQUAD *pLUT,
                                 L_UINT uLUTLength,
                                 L_UINT uFlags
                               );
      virtual L_INT WindowLevelBitmap(
                                       L_INT nLowBit,
                                       L_INT nHighBit,
                                       RGBQUAD *pLUT,
                                       L_UINT uLUTLength,
                                       L_INT nOrderDst, 
                                       L_UINT32 uFlags = 0
                                       );

      static L_INT WindowLevelFillLUT(
                                    RGBQUAD * pLUT,
                                    L_UINT32 ulLUTLen,
                                    COLORREF crStart,
                                    COLORREF crEnd,
                                    L_INT nLow,
                                    L_INT nHigh,
                                    L_UINT uLowBit,
                                    L_UINT uHighBit,
                                    L_INT nMinValue,
                                    L_INT nMaxValue,
                                    L_INT  nFactor,
                                    L_UINT uFlags
                                    );

      virtual L_INT ContourFilter(
                                   L_INT16 nThreshold,
                                   L_INT16 nDeltaDirection,
                                   L_INT16 nMaximumError,
                                   L_INT nOption,
                                   L_UINT32 uFlags = 0);

      virtual L_INT GaussianFilter(L_INT nRadius, L_UINT32 uFlags = 0);
      
      virtual L_INT UnsharpMask(L_INT nAmount, L_INT nRadius, L_INT nThreshold, L_UINT uFlags);

      virtual L_INT GrayScaleExt(L_INT RedFact, L_INT GreenFact, L_INT BlueFact, L_UINT32 uFlags = 0);

      virtual L_INT ConvertToColoredGray(L_INT RedFact, L_INT GreenFact, L_INT BlueFact, L_INT RedGrayFact, L_INT GreenGrayFact, L_INT BlueGrayFact, L_UINT32 uFlags = 0);

      virtual L_INT BalanceColors(BALANCING * pRedFact, BALANCING * pGreenFact, BALANCING * pBlueFact, L_UINT32 uFlags = 0);

      virtual L_INT SwapColors(L_UINT32 uFlags = 0);

      virtual L_INT LineProfile(POINT FirstPoint, POINT SecondPoint, L_INT ** ppRed, L_INT ** ppGreen, L_INT ** ppBlue, L_UINT32 uFlags = 0);

      virtual L_INT HistoEqualize(L_UINT32 uFlags = 0);

      virtual L_INT AntiAlias(L_UINT uThreshold, L_UINT uDim, L_UINT uFilter, L_UINT32 uFlags = 0);
      L_INT AntiAlias2(L_INT nThreshold, L_UINT uDim, L_UINT uFilter, L_UINT32 uFlags = 0);

      virtual L_INT EdgeDetector(L_UINT uThreshold, L_UINT uFilter, L_UINT32 uFlags = 0);
      L_INT EdgeDetector2(L_INT nThreshold, L_UINT uFilter, L_UINT32 uFlags = 0);

      virtual L_INT RemoveRedEye(COLORREF rcNewColor, L_UINT uThreshold, L_INT nLightness, L_UINT32 uFlags = 0);

      virtual L_INT MotionBlur(L_UINT uDim, L_INT nAngle, L_BOOL bUnidirectional, L_UINT32 uFlags = 0);

      virtual L_INT PicturizeList(L_UINT uxDim, L_UINT uyDim, L_UINT uLightnessFact, LBitmapList * pBitmapList, L_UINT32 uFlags = 0);

      virtual L_INT PicturizeSingle(LBitmapBase * pThumbBitmap,
                                    L_UINT uxDim, L_UINT uyDim,
                                    L_UINT uLightnessFact,
                                    L_UINT32 uFlags = 0);

      virtual L_INT GetFunctionalLookupTable(L_INT * pLookupTable, L_UINT uLookupLen,
                                             L_INT nStart, L_INT nEnd,
                                             L_INT nFactor, L_UINT uFlags);

      virtual L_INT GetUserLookupTable(L_UINT * pLookupTable, L_UINT uLookupLen,
                                       POINT * apUserPoint, L_UINT uUserPointCount,
                                       L_UINT * puPointCount,
                                       L_UINT32 uFlags = 0);

      virtual L_INT AddBorder(pADDBORDERINFO pAddBorderInfo, L_UINT32 uFlags = 0);

      virtual L_INT AddFrame(pADDFRAMEINFO pAddFrameInfo, L_UINT32 uFlags = 0);
      
      virtual L_INT Multiply(L_UINT uFactor, L_UINT32 uFlags = 0);
      virtual L_INT RemapHue(L_UINT * pMask,L_UINT * pHTable,L_UINT * pSTable,L_UINT * pVTable,L_UINT uLUTLen, L_UINT32 uFlags = 0);
      virtual L_INT AddWeighted(LBitmapList *pBitmapList,L_UINT * puFactor,L_UINT uFlags);
      virtual L_INT AddWeighted(HBITMAPLIST hBitmapList,L_UINT * puFactor,L_UINT uFlags);
      virtual L_INT LocalHistoEqualize(L_INT nWidthLen,L_INT nHeightLen,L_INT nxExt,L_INT nyExt,L_UINT uType,L_UINT uSmooth, L_UINT32 uFlags = 0);
            
      virtual L_INT Pixelate(L_UINT uCellWidth,L_UINT uCellHeight,L_UINT uOpacity,POINT CenterPt, L_UINT uFlags);
      virtual L_INT Wind(L_UINT uDim,L_INT nAngle,L_UINT uOpacity, L_UINT32 uFlags = 0);
      virtual L_INT Impressionist(L_UINT uHorzDim,L_UINT uVertDim, L_UINT32 uFlags = 0);
      virtual L_INT Wave(L_UINT uAmplitude,L_UINT uWaveLen,L_INT nAngle,L_UINT uHorzFact,L_UINT uVertFact,COLORREF crFill,L_UINT uFlags);
      virtual L_INT ZoomWave(L_UINT uAmplitude,L_UINT uFrequency,L_INT nPhase,L_UINT uZomFact,POINT pCenter,COLORREF crFill,L_UINT uFlags);
      virtual L_INT RadWave(L_UINT uAmplitude,L_UINT   uWaveLen,L_INT    nPhase,POINT    pCenter,COLORREF crFill,L_UINT   uFlags);
      virtual L_INT FreeHandShear(L_INT *pAmplitudes,L_UINT uAmplitudesCount,L_UINT uScale,COLORREF crFill,L_UINT uFlags);
      virtual L_INT FreeHandWave(L_INT * pAmplitudes,L_UINT uAmplitudesCount,L_UINT uScale,L_UINT uWaveLen,L_INT nAngle,COLORREF crFill,L_UINT uFlags);
      virtual L_INT AddMessage(pADDMESGINFO pAddMesgInfo, L_UINT32 uFlags = 0);
      virtual L_INT ExtractMessage(pADDMESGINFO pAddMesgInfo, L_UINT32 uFlags = 0);
      virtual L_INT Spherize(L_INT nValue,POINT CenterPt,COLORREF crFill,L_UINT uFlags);
      virtual L_INT Cylindrical(L_INT nValue,L_UINT uType, L_UINT32 uFlags = 0);
      virtual L_INT Bending(L_INT nValue,POINT CenterPt,COLORREF crFill,L_UINT uFlags);
      virtual L_INT Punch(L_INT nValue,L_UINT uStress,POINT CenterPt,COLORREF crFill,L_UINT uFlags);
      virtual L_INT Ripple(L_UINT uAmplitude,L_UINT uFrequency,L_INT nPhase,L_UINT uAttenuation,POINT CenterPt,COLORREF crFill,L_UINT uFlags);
      virtual L_INT Polar(COLORREF crFill,L_UINT uFlags);
      virtual L_INT RadialBlur(L_UINT uDim,L_UINT uStress,POINT CenterPt, L_UINT32 uFlags = 0);
      static  L_INT GetCurvePoints(L_INT  * pCurve,POINT * apUserPoint,L_UINT uUserPointCount,L_UINT * puPointCount,L_UINT uFlags);
      virtual L_INT ZoomBlur(L_UINT uDim,L_UINT uStress,POINT CenterPt, L_UINT32 uFlags = 0);
      virtual L_INT Swirl(L_INT nRotationAngle,POINT CenterPt, L_UINT32 uFlags = 0);

      virtual L_INT Deinterlace(L_UINT uFlags);
      virtual L_INT SampleTarget(COLORREF crSample,COLORREF crTarget,L_UINT uFlags);
      virtual L_INT HalfTone(L_UINT uType,L_INT32 nAngle,L_UINT uDim,LBitmapList * pBitmapList, L_UINT32 uFlags = 0);
      virtual L_INT HalfTone(L_UINT uType,L_INT32 nAngle,L_UINT uDim, HBITMAPLIST hBitmapList, L_UINT32 uFlags = 0);
      virtual L_INT Cubism(L_UINT uSpace, L_UINT uLength, L_INT nBrightness, L_INT nAngle, COLORREF crColor, L_UINT uFlags);
      virtual L_INT LightControl(L_UINT * puLowerAvr, L_UINT * puAvrage, L_UINT * puUpperAvr, L_UINT uFlags);
      virtual L_INT GlassEffect(L_UINT uCellWidth, L_UINT uCellHeight, L_UINT uFlags);
      virtual L_INT LensFlare(POINT ptCenter, L_UINT uBright, L_UINT uFlags, COLORREF crColor);
      virtual L_INT BumpMap(LBitmap *pBumpBitmap, pBUMPDATA pBumpData, L_UINT32 uFlags = 0);
      virtual L_INT BumpMap(pBITMAPHANDLE hBumpBitmap, pBUMPDATA pBumpData, L_UINT32 uFlags = 0);
      virtual L_INT GlowFilter( L_UINT uDim, L_UINT uBright, L_UINT uThreshold, L_UINT32 uFlags = 0);
      virtual L_INT EdgeDetectStatistical(L_UINT uDim, L_UINT uThreshold, COLORREF crEdgeColor, COLORREF crBkColor, L_UINT32 uFlags = 0);
      L_INT EdgeDetectStatistical2(L_UINT uDim, L_INT nThreshold, COLORREF crEdgeColor, COLORREF crBkColor, L_UINT32 uFlags = 0);
      virtual L_INT Desaturate(L_UINT32 uFlags = 0);
      virtual L_INT SmoothEdges(L_UINT nAmount, L_UINT nThreshold, L_UINT32 uFlags = 0);
      virtual L_INT AutoBinary(L_UINT32 uFlags = 0);
      virtual L_INT ChannelMix (pCOLORDATA pRedFactor, pCOLORDATA pGreenFactor, pCOLORDATA pBlueFactor, L_UINT32 uFlags = 0);
      virtual L_INT Plane(POINT ptCenterPoint, L_UINT uZValue, L_INT nDistance, L_UINT uPlaneOffset, L_INT nRepeat, L_INT nPydAngle, L_UINT uStretch, L_UINT uStartBright, L_UINT uEndBright, L_UINT uBrightLength, COLORREF crBright, COLORREF crFill, L_UINT uFlags);
      virtual L_INT PlaneBend(POINT ptCenterPoint, L_UINT uZValue, L_INT nDistance, L_UINT uPlaneOffset, L_INT nRepeat, L_INT nPydAngle, L_UINT uStretch, L_UINT uBendFactor, L_UINT uStartBright, L_UINT uEndBright, L_UINT uBrightLength, COLORREF crBright, COLORREF crFill, L_UINT uFlags);
      virtual L_INT Tunnel(POINT ptCenterPoint, L_UINT uZValue, L_INT nDistance, L_UINT nRad, L_INT nRepeat, L_INT nRotationOffset, L_UINT uStretch, L_UINT uStartBright, L_UINT uEndBright, L_UINT uBrightLength, COLORREF crBright, COLORREF crFill, L_UINT uFlags);
      virtual L_INT FreeRadBend(L_INT * pCurve, L_UINT uCurveSize, L_UINT uScale, POINT CenterPt, COLORREF crFill, L_UINT uFlags);
      virtual L_INT FreePlaneBend(L_INT * puCurve, L_UINT uCurveSize, L_UINT uScale, COLORREF crFill, L_UINT uFlags);
      virtual L_INT Ocean(L_UINT uAmplitude, L_UINT uFrequency, L_BOOL bLowerTrnsp, L_UINT32 uFlags = 0);
      virtual L_INT Light(pLIGHTINFO pLightInfo, L_UINT uLightNo, L_UINT uBright, L_UINT uAmbient, COLORREF crAmbientClr, L_UINT32 uFlags = 0);
      virtual L_INT Dry(L_UINT uDim, L_UINT32 uFlags = 0);
      virtual L_INT DrawStar(pSTARINFO pStarInfo, L_UINT32 uFlags = 0);

      virtual L_INT GrayScaleToDuotone(LPRGBQUAD pNewColor,COLORREF crColor,L_UINT uFlags);
      virtual L_INT GrayScaleToMultitone(L_UINT uToneType,L_UINT uDistType,LPCOLORREF pColor,LPRGBQUAD * pGradient,L_UINT uFlags);
      virtual L_INT Skeleton(L_UINT uThreshold, L_UINT32 uFlags = 0);
      L_INT Skeleton2(L_INT nThreshold, L_UINT32 uFlags = 0);
      virtual L_INT ColorLevel(pLVLCLR pLvlClr,L_UINT uFlags);
      virtual L_INT AutoColorLevel(pLVLCLR pLvlClr,L_UINT uBlackClip,L_UINT uWhiteClip,L_UINT uFlags);
      virtual L_INT SelectiveColor(pSELCLR pSelClr, L_UINT32 uFlags = 0);
      virtual L_INT Correlation(pBITMAPHANDLE  pCorBitmap,
                           POINT          * pPoints,
                           L_UINT         uMaxPoints,
                           L_UINT         * puNumOfPoints,
                           L_UINT         uXStep,
                           L_UINT         uYStep,
                           L_UINT         uThreshold, 
                           L_UINT32       uFlags = 0);

      virtual L_INT Correlation(LBitmapBase *plCorBitmap,
                           POINT        * pPoints,
                           L_UINT         uMaxPoints,
                           L_UINT       * puNumOfPoints,
                           L_UINT         uXStep,
                           L_UINT         uYStep,
                           L_UINT         uThreshold,
                           L_UINT32       uFlags = 0);
      
      virtual L_INT SetOverlay(L_INT nIndex,pBITMAPHANDLE pOverlayBitmap,  L_UINT uFlags);
      virtual L_INT SetOverlay(L_INT nIndex, LBitmapBase *  pOverlayBitmap,  L_UINT uFlags);
      
      virtual L_INT GetOverlay(L_INT nIndex,pBITMAPHANDLE pOverlayBitmap,L_UINT uStructSize,L_UINT uFlags);
      virtual L_INT GetOverlay(L_INT nIndex,LBitmapBase * pBitmap,L_UINT uStructSize,L_UINT uFlags);

      virtual L_INT SetOverlayAttributes(L_INT nIndex,
                                    pOVERLAYATTRIBUTES pOverlayAttributes,
                                    L_UINT uFlags);

      virtual L_INT GetOverlayAttributes(L_INT nIndex,
                                    pOVERLAYATTRIBUTES pOverlayAttributes,
                                    L_UINT uStructSize,
                                    L_UINT uFlags);

      virtual L_INT UpdateOverlayBits(L_INT nIndex,L_UINT uFlags);

      virtual L_UINT GetOverlayCount(L_UINT uFlags);
      

      virtual L_INT Scramble
                           (
                           L_INT32 nColStart,
                           L_INT32 nRowStart,
                           L_INT32 nWidth,
                           L_INT32 nHeight,
                           L_UINT32 uKey,
                           L_UINT uFlags
                           );

      virtual L_INT ApplyModalityLUT(
                                      L_UINT16              *pLUT,
                                      pDICOMLUTDESCRIPTOR   pLUTDescriptor,
                                      L_UINT                uFlags
                                     );

      virtual L_INT ApplyLinearModalityLUT( 
                                             L_DOUBLE       fIntercept, 
                                             L_DOUBLE       fSlope, 
                                             L_UINT         uFlags
                                          );

      virtual L_INT ApplyVOILUT(   
                                    L_UINT16      *      pLUT           ,
                                    pDICOMLUTDESCRIPTOR  pLUTDescriptor ,   
                                    L_UINT               uFlags
                               );

      virtual L_INT ApplyLinearVOILUT(   
                                          L_DOUBLE       fCenter     , 
                                          L_DOUBLE       fWidth      ,    
                                          L_UINT         uFlags
                                      );

      
      
      // Group 4
      virtual L_INT AddShadow(L_UINT uAngle,L_UINT uThreshold,L_UINT uFlags);
      virtual L_INT AllocFTArray(pFTARRAY * ppFTArray,L_UINT uStructSize, L_UINT32 uFlags = 0);
      virtual L_INT ChangeHueSatInt(L_INT nHue,L_INT nSaturation,L_INT nIntensity,pHSIDATA pHsiData,L_UINT uHsiDataCount, L_UINT32 uFlags = 0);
      virtual L_INT ColorReplace(pCOLORREPLACE pColorReplace,L_UINT uColorCount,L_INT nHue,L_INT nSaturation,L_INT nBrightness, L_UINT32 uFlags = 0);
      virtual L_INT ColorThreshold(L_UINT uColorSpace,pCOMPDATA pCompData, L_UINT32 uFlags = 0);
      virtual L_INT DFT(pFTARRAY pFTArray,RECT * prcRange,L_UINT uFlags);
      virtual L_INT DirectionEdgeStatistical(L_UINT uDim,L_UINT uThreshold,L_INT nAngle,COLORREF crEdgeColor,COLORREF crBkColor, L_UINT32 uFlags = 0);
      L_INT DirectionEdgeStatistical2(L_UINT uDim,L_INT nThreshold,L_INT nAngle,COLORREF crEdgeColor,COLORREF crBkColor, L_UINT32 uFlags = 0);
      virtual L_INT FFT(pFTARRAY pFTArray,L_UINT uFlags);
      static  L_INT FreeFTArray(pFTARRAY pFTArray, L_UINT32 uFlags = 0);
      virtual L_INT FTDisplay(pFTARRAY pFTArray,L_UINT uFlags);
      static  L_INT FrqFilter(pFTARRAY pFTArray,LPRECT prcRange,L_UINT uFlags);
      virtual L_INT FrqFilterMask(pFTARRAY pFTArray,L_BOOL bOnOff, L_UINT32 uFlags = 0);
      virtual L_INT GetStatisticsInfo(pSTATISTICSINFO pStatisticsInfo,L_UINT uChannel,L_UINT uStart,L_UINT uEnd, L_UINT32 uFlags = 0);
      L_INT GetStatisticsInfo2(pSTATISTICSINFO pStatisticsInfo,L_UINT uChannel,L_INT nStart,L_INT nEnd, L_UINT32 uFlags = 0);
      static  L_INT GetFeretsDiameter(POINT * pPoints,L_UINT uSize,L_UINT * puFeretsDiameter,L_UINT * puFirstIndex,L_UINT * puSecondIndex, L_UINT32 uFlags = 0);
      virtual L_INT GetObjectInfo(L_UINT * puX,L_UINT * puY,L_INT * pnAngle,L_UINT * puRoundness,L_BOOL bWeighted, L_UINT32 uFlags = 0);
      virtual L_INT GetRgnContourPoints(pRGNXFORM pXForm,POINT * * ppPoints,L_UINT * puSize, L_UINT uFlags);
      virtual L_INT GetRgnPerimeterLength(pRGNXFORM pXForm,L_SIZE_T* puLength, L_UINT32 uFlags = 0);
      virtual L_INT MathFunction(L_UINT uMType,L_UINT uFactor, L_UINT32 uFlags = 0);
      virtual L_INT RevEffect(L_UINT uLineSpace,L_UINT uMaximumHeight, L_UINT32 uFlags = 0);
      virtual L_INT Segment(L_UINT uThreshold,L_UINT uFlags);
      virtual L_INT SubtractBackground(L_UINT uRollingBall,L_UINT uShrinkSize,L_UINT uBrightnessFactor,L_UINT uFlags);
      virtual L_INT UserFilter(pUSERFLT pFilter, L_UINT32 uFlags = 0);
      // Group 5
      virtual L_INT AdaptiveContrast(L_UINT uDim,L_UINT uAmount,L_UINT uFlags);
      virtual L_INT Aging(L_UINT uHScratchCount,L_UINT uVScratchCount,L_UINT uMaxScratchLen,L_UINT uDustDensity,L_UINT uPitsDensity,L_UINT uMaxPitSize,COLORREF crScratch,COLORREF crDust,COLORREF crPits,L_UINT uFlags);
      virtual L_INT ApplyMathLogic(L_INT nFactor, L_UINT uFlags);
      virtual L_INT ColorIntensityBalance(pBALANCEDATA pShadows,pBALANCEDATA pMidTone,pBALANCEDATA pHighLight,L_BOOL bLuminance, L_UINT32 uFlags = 0);
      virtual L_INT ColorizeGray(pBITMAPHANDLE pSrcBitmap,pLTGRAYCOLOR pGrayColors,L_UINT uCount, L_UINT32 uFlags = 0);
      virtual L_INT ColorizeGray(LBitmapBase *plSrcBitmap,pLTGRAYCOLOR pGrayColors,L_UINT uCount, L_UINT32 uFlags = 0);
      virtual L_INT ContBrightInt(L_INT nContrast,L_INT nBrightness, L_INT nIntensity, L_UINT32 uFlags = 0);
      virtual L_INT DiceEffect(L_UINT uXBlock,L_UINT uYBlock,L_UINT uRandomize,L_UINT uFlags,COLORREF crColor);
      virtual L_INT DigitalSubtract(pBITMAPHANDLE pMaskBitmap,L_UINT uFlags);
      virtual L_INT DigitalSubtract(LBitmapBase * plMaskBitmap,L_UINT uFlags);
      virtual L_INT DynamicBinary(L_UINT uDim,L_UINT uLocalContrast, L_UINT32 uFlags = 0);
      virtual L_INT EdgeDetectEffect(L_UINT uLevel,L_UINT uThreshold,L_UINT uFlags);
      virtual L_INT FunctionalLight(pLIGHTPARAMS pLightParams, L_UINT32 uFlags = 0);
      virtual L_INT MultiScaleEnhancement(L_UINT uContrast,L_UINT uEdgeLevels,L_UINT uEdgeCoeff,L_UINT uLatitudeLevels,L_UINT uLatitudeCoeff,L_UINT uFlags);
      virtual L_INT PuzzleEffect(L_UINT uXBlock,L_UINT uYBlock,L_UINT uRandomize,L_UINT uFlags,COLORREF crColor);
      virtual L_INT RingEffect(L_INT nXOrigin,L_INT nYOrigin,L_UINT uRadius,L_UINT uRingCount,L_UINT uRandomize, COLORREF crColor,L_INT nAngle,L_UINT uFlags);
      virtual L_INT SelectData(pBITMAPHANDLE pDstBitmap,COLORREF crColor,L_UINT uSrcLowBit,L_UINT uSrcHighBit,L_UINT uThreshold,L_BOOL bCombine, L_UINT32 uFlags = 0);
      virtual L_INT SelectData(LBitmapBase * plDstBitmap,COLORREF crColor,L_UINT uSrcLowBit,L_UINT uSrcHighBit,L_UINT uThreshold,L_BOOL bCombine, L_UINT32 uFlags = 0);
      virtual L_INT ShiftData(pBITMAPHANDLE pSrcBitmap,L_UINT uSrcLowBit,L_UINT uSrcHighBit,L_UINT uDstLowBit,L_UINT uDstBitsPerPixel, L_UINT32 uFlags = 0);
      virtual L_INT ShiftData(LBitmapBase * plSrcBitmap,L_UINT uSrcLowBit,L_UINT uSrcHighBit,L_UINT uDstLowBit,L_UINT uDstBitsPerPixel, L_UINT32 uFlags = 0);
      virtual L_INT TextureAlphaBlend(L_INT nXDst,L_INT nYDst,L_INT nWidth,L_INT nHeight,pBITMAPHANDLE pBitmapSrc,L_INT nXSrc,L_INT nYSrc,pBITMAPHANDLE pBitmapMask,L_INT nOpacity,pBITMAPHANDLE pBitmapUnderlay,LPPOINT pOffset, L_UINT32 uFlags = 0);
      virtual L_INT TextureAlphaBlend(L_INT nXDst,L_INT nYDst,L_INT nWidth,L_INT nHeight,LBitmapBase *plBitmapSrc,L_INT nXSrc,L_INT nYSrc,LBitmapBase *plBitmapMask,L_INT nOpacity,LBitmapBase *plBitmapUnderlay,LPPOINT pOffset, L_UINT32 uFlags = 0);
      // Group 6
      virtual L_INT IsRegMark(L_UINT uType,L_UINT uMinScale,L_UINT uMaxScale,L_UINT uWidth,L_UINT uHeight, L_UINT32 uFlags = 0);
      virtual L_INT GetMarksCenterMass(POINT * pMarkPoints,POINT * pMarkCMPoints,L_UINT uMarksCount, L_UINT32 uFlags = 0);
      virtual L_INT SearchRegMarks(pSEARCHMARKS pSearchMarks,L_UINT uMarkCount, L_UINT32 uFlags = 0);
      virtual L_INT GetTransformationParameters(POINT * pRefPoints,POINT * pTrnsPoints,L_INT * pnXTranslation,L_INT * pnYTranslation,L_INT * pnAngle,L_UINT * puXScale,L_UINT * puYScale, L_UINT32 uFlags = 0);
      virtual L_INT ApplyTransformationParameters(L_INT nXTranslation,L_INT nYTranslation,L_INT nAngle,L_UINT uXScale,L_UINT uYScale,L_UINT uFlags);
      virtual L_INT        Add(LBitmapList * pBitmapList, L_UINT uFlags = BC_ADD);

      static  L_INT CountLUTColors(RGBQUAD * pLUT,L_UINT32 ulLLUTLen,L_UINT * pNumberOfEntries,L_INT  *  pFirstIndex,L_UINT   uFlags);
      virtual L_INT GetLinearVOILUT(L_DOUBLE *pCenter,L_DOUBLE *pWidth,L_UINT uFlags);
      virtual L_INT ConvertSignedToUnsigned(L_UINT uShift, L_UINT32 uFlags = 0);
      virtual L_INT ConvertUnsignedToSigned(L_UINT uFlags);
      //Used for Document Imaging functions
      //virtual L_INT LBitmap::GetDocImageRgn(LBitmapBase *pDocImageRgn);

      L_INT Perlin (L_UINT32 uSeed, L_UINT uFrequency, L_UINT uIteration, L_UINT uOpacity,
         COLORREF crBClr, COLORREF crFClr, L_INT nxCircle, L_INT nyCircle, L_INT nFreqLayout,
         L_INT nDenLayout, L_UINT uFlags);

      L_INT ShiftMinimumToZero (L_UINT * puShiftAmount, L_UINT32 uFlags = 0);

      L_INT ShiftZeroToNegative (L_INT nShiftAmount,L_INT nMinInput,L_INT nMaxInput,L_INT nMinOutput,L_INT nMaxOutput, L_UINT32 uFlags = 0);

      L_INT ColoredPencil (L_UINT uRatio, L_UINT uDim, L_UINT32 uFlags = 0);

      L_INT SetKaufmannRgn (pBITMAPHANDLE    pProcessedBitmap,
                                    L_INT            nRadius,
                                    L_INT            nMinInput, 
                                    L_INT            nMaxInput,
                                    L_INT            nRgnThreshold,
                                    POINT            ptRgnStart,
                                    L_BOOL           bRemoveHoles,
                                    L_SIZE_T       * puPixelsCount,
                                    L_UINT           uCombineMode,
                                    L_UINT32         uFlags = 0);

      L_INT Canvas(LBitmapBase *plCanvasBitmap,
                     L_UINT uTransparency,
                     L_UINT uEmboss,
                     L_INT  nXOffset,
                     L_INT  nYOffset,
                     L_UINT uTilesOffset,
                     L_UINT uFlags);

      L_INT Canvas(pBITMAPHANDLE phCanvasBitmap,
                     L_UINT uTransparency,
                     L_UINT uEmboss,
                     L_INT  nXOffset,
                     L_INT nYOffset,
                     L_UINT uTilesOffset,
                     L_UINT uFlags);

      L_INT DisplaceMap(LBitmapBase *plDisplaceBitmap,
                           L_UINT uHorzFact,
                           L_UINT uVertFact,
                           COLORREF crFill,
                           L_UINT uFlags);

      L_INT DisplaceMap(pBITMAPHANDLE phDisplaceBitmap,
                          L_UINT uHorzFact,
                          L_UINT uVertFact,
                          COLORREF crFill,
                          L_UINT uFlags);

      L_INT CorrelationList(HBITMAPLIST    hCorList,
                              POINT  * pPoints,
                              L_UINT * puListIndex,
                              L_UINT uMaxPoints,
                              L_UINT * puNumOfPoints,
                              L_UINT uXStep,
                              L_UINT uYStep,
                              L_UINT uThreshold,
                              L_UINT32 uFlags = 0);

      L_INT Perspective (POINT * pPoints,COLORREF crBkgColor,L_UINT uFlags);

      L_INT ColoredBalls (L_UINT uNumBalls,
                           L_UINT uSize,
                           L_UINT  uSizeVariation,
                           L_INT nHighLightAng,
                           COLORREF crHighLight,
                           COLORREF crBkgColor,
                           COLORREF crShadingColor,
                           COLORREF * pBallColors,
                           L_UINT uNumOfBallColors,
                           L_UINT uAvrBallClrOpacity,
                           L_UINT uBallClrOpacityVariation,
                           L_UINT uRipple,
                           L_UINT uFlags);

      L_INT AdjustTint (L_INT nAngleA, L_INT nAngleB, L_UINT32 uFlags = 0);

      L_INT HalfTonePattern (L_UINT uContrast,
                              L_UINT uRipple,
                              L_UINT uAngleContrast,
                              L_UINT uAngleRipple,
                              L_INT nAngleOffset,
                              COLORREF crForGround,
                              COLORREF crBackGround,
                              L_UINT uFlags);

      L_INT Pointillist (L_UINT uSize,
                           COLORREF crColor,
                           L_UINT uFlags);

      L_INT ColorHalfTone (L_UINT uMaxRad,
                              L_INT nCyanAngle,
                              L_INT nMagentaAngle,
                              L_INT nYellowAngle,
                              L_INT nBlackAngle,
                              L_UINT32 uFlags = 0);

      L_INT RomanMosaic (L_UINT uTileWidth,
                           L_UINT uTileHeight,
                           L_UINT uBorder,
                           L_UINT uShadowAngle,
                           L_UINT uShadowThresh,
                           COLORREF crColor,
                           L_UINT uFlags);

      L_INT Vignette (pVIGNETTEINFO pVignetteInfo, L_UINT32 uFlags = 0);

      L_INT MosaicTiles (pMOSAICTILESINFO pMosaicTilesInfo, L_UINT32 uFlags = 0);

      L_INT Fragment (L_UINT uOffset, L_UINT uOpacity, L_UINT32 uFlags = 0);

      L_INT Clouds (L_UINT uSeed,
                     L_UINT uFrequency,
                     L_UINT uDensity,
                     L_UINT uOpacity,
                     COLORREF cBackColor,
                     COLORREF crCloudsColor,
                     L_UINT uFlags);

      L_INT Offset (L_INT nHorizontalShift,
                     L_INT nVerticalShift,
                     COLORREF crBackColor,
                     L_UINT uFlags);

      L_INT BricksTexture (L_UINT uBricksWidth,
                           L_UINT uBricksHeight,
                           L_UINT uOffsetX,
                           L_UINT uOffsetY,
                           L_UINT uEdgeWidth,
                           L_UINT uMortarWidth,
                           L_UINT uShadeAngle,
                           L_UINT uRowDifference,
                           L_UINT uMortarRoughness,
                           L_UINT uMortarRoughnessEevenness,
                           L_UINT uBricksRoughness,
                           L_UINT uBricksRoughnessEevenness,
                           COLORREF crMortarColor,
                           L_UINT uFlags);

      L_INT PlasmaFilter (pPLASMAINFO PlasmaInfo, L_UINT32 uFlags = 0);

      L_INT MaskConvolution (L_INT nAngle,
                              L_UINT uDepth,
                              L_UINT uHeight,
                              L_UINT uFlags);

      L_INT GammaCorrectExt (L_UINT uGamma, L_UINT uFlags);

      L_INT HighPassFilter (L_UINT uRadius, L_UINT uOpacity, L_UINT32 uFlags = 0);

      L_INT ZigZag (L_UINT uAmplitude,
                     L_UINT uAttenuation,
                     L_UINT uFrequency,
                     L_INT  nPhase,
                     POINT  CenterPt,
                     COLORREF crFill,
                     L_UINT uFlags);

      L_INT DeskewExt (L_INT32 *pnAngle,
                        L_UINT uAngleRange,
                        L_UINT uAngleResolution,
                        COLORREF crBack,
                        L_UINT uFlags);

      L_INT DiffuseGlow (L_INT nGlowAmount,
                           L_UINT uClearAmount,
                           L_UINT uSpreadAmount, 
                           L_UINT uWhiteNoise,
                           COLORREF crGlowColor,
                           L_UINT32 uFlags = 0);

      L_INT Slice(pSLICEBITMAPOPTIONS  pOptions,
                  L_INT32  * pnDeskewAngle,
                  L_UINT32 uFlags = 0);

      L_INT BlankPageDetector(L_BOOL * bIsBlank,
                              L_UINT * pAccuracy,
                              pPAGEMARGINS PMargins,
                              L_UINT uFlags);

      // New functions only for version 16
      L_INT ColoredPencilExt(L_UINT uSize,
                             L_UINT uStrength,
                             L_UINT uThreshold,
                             L_UINT uPencilRoughness,
                             L_UINT uStrokeLength,
                             L_UINT uPaperRoughness,
                             L_INT  nAngle,
                             L_UINT uFlags);

      L_INT AutoBinarize(L_UINT uFactor,
                         L_UINT uFlags);

      L_INT StartFastMagicWandEngine(MAGICWANDHANDLE* pMagicWnd,
                                     L_UINT32 uFlags);

      L_INT EndFastMagicWandEngine(MAGICWANDHANDLE MagicWnd,
         L_UINT32 uFlags);


      L_INT FastMagicWand(MAGICWANDHANDLE MagicWnd,
         L_INT nTolerance,
         L_INT nXposition,
         L_INT nYposition,
         pOBJECTINFO pObjectInfo,
         L_UINT32 uFlags);

      L_INT DeleteObjectInfo(pOBJECTINFO pObjectInfo,
         L_UINT32 uFlags);

      L_INT TissueEqualize(L_UINT uFlags);

      L_INT SigmaFilter( L_UINT nSize,
                         L_UINT nSigma,
                         L_FLOAT nThreshhold, 
                         L_BOOL bOutline,
                         L_UINT32 uFlags);

      L_INT AutoSegment(L_RECT * prcRect, 
                        L_UINT32 uFlags);

      L_INT IntelligentDownScale(LBitmapBase * plMaskBitmap,
                                 COLORREF crRemoveObjectColor,
                                 COLORREF crPreserveObjectColor,
                                 L_INT nNewWidth,
                                 L_INT nNewHeight,
                                 L_INT nDownScalingOrder,
                                 L_UINT32 uFlags = 0);

      L_INT IntelligentDownScale(pBITMAPHANDLE phMaskBitmap,
                                 COLORREF crRemoveObjectColor,
                                 COLORREF crPreserveObjectColor,
                                 L_INT nNewWidth,
                                 L_INT nNewHeight,
                                 L_INT nDownScalingOrder,
                                 L_UINT32 uFlags = 0);

      L_INT IntelligentUpScale  (pBITMAPHANDLE phMaskBitmap,
                                 COLORREF crRemoveObjectColor,
                                 COLORREF crPreserveObjectColor,
                                 L_INT nNewWidth,
                                 L_INT nWidthUpScalingFactor,
                                 L_INT nNewHeight,
                                 L_INT nHeightUpScalingFactor,
                                 L_INT nUpScalingOrder,
                                 L_UINT32 uFlags = 0);

      L_INT IntelligentUpScale   (LBitmapBase * pMaskBitmap,
                                  COLORREF crRemoveObjectColor,
                                  COLORREF crPreserveObjectColor,
                                  L_INT nNewWidth,
                                  L_INT nWidthUpScalingFactor,
                                  L_INT nNewHeight,
                                  L_INT nHeightUpScalingFactor,
                                  L_INT nUpScalingOrder,
                                  L_UINT32 uFlags = 0);


};
#endif

#endif //_LEAD_BITMAP_H_
/*================================================================= EOF =====*/



