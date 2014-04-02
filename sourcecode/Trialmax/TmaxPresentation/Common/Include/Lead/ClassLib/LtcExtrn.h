/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcExtrn.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_EXTERN_H_
#define  _LEAD_EXTERN_H_

/*----------------------------------------------------------------------------+
| extern LWRP_EXPORT LWRP_EXPORTAL VARIABLES                                                          |   
+----------------------------------------------------------------------------*/
extern HINSTANCE     hWrpDLLInst;
extern LWRP_EXPORT   LBitmapDictionary    BitmapDictionary;
extern               LDictionary          LStatusDictionary;

//--LEAD FUNCTIONS POINTERS----------------------------------------------------
#ifdef USE_POINTERS_TO_LEAD_FUNCTIONS

//-----------------------------------------------------------------------------
//--LTKRN.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_ACCESSBITMAP  pL_AccessBitmap;
extern LWRP_EXPORT pL_ALLOCATEBITMAP   pL_AllocateBitmap;
extern LWRP_EXPORT pL_CHANGEBITMAPHEIGHT  pL_ChangeBitmapHeight;
extern LWRP_EXPORT pL_CHANGEBITMAPVIEWPERSPECTIVE  pL_ChangeBitmapViewPerspective;
extern LWRP_EXPORT pL_CHANGEFROMDIB pL_ChangeFromDIB;
extern LWRP_EXPORT pL_CHANGETODIB   pL_ChangeToDIB;
extern LWRP_EXPORT pL_CLEARBITMAP   pL_ClearBitmap;
extern LWRP_EXPORT pL_CLEARNEGATIVEPIXELS pL_ClearNegativePixels;
extern LWRP_EXPORT pL_COLORRESBITMAP   pL_ColorResBitmap;
extern LWRP_EXPORT pL_COLORRESBITMAPLIST  pL_ColorResBitmapList;
extern LWRP_EXPORT pL_COLORRESBITMAPLISTEXT  pL_ColorResBitmapListExt;
extern LWRP_EXPORT pL_COMPRESSROW   pL_CompressRow;
extern LWRP_EXPORT pL_COMPRESSROWS  pL_CompressRows;
extern LWRP_EXPORT pL_CONVERTBUFFER pL_ConvertBuffer;
extern LWRP_EXPORT pL_CONVERTBUFFEREXT pL_ConvertBufferExt;
extern LWRP_EXPORT pL_CONVERTFROMDIB   pL_ConvertFromDIB;
extern LWRP_EXPORT pL_CONVERTTODIB  pL_ConvertToDIB;
extern LWRP_EXPORT pL_COPYBITMAP pL_CopyBitmap;
extern LWRP_EXPORT pL_COPYBITMAP2   pL_CopyBitmap2;
extern LWRP_EXPORT pL_COPYBITMAPDATA   pL_CopyBitmapData;
extern LWRP_EXPORT pL_COPYBITMAPHANDLE pL_CopyBitmapHandle;
extern LWRP_EXPORT pL_COPYBITMAPLISTITEMS pL_CopyBitmapListItems;
extern LWRP_EXPORT pL_COPYBITMAPRECT   pL_CopyBitmapRect;
extern LWRP_EXPORT pL_COPYBITMAPRECT2  pL_CopyBitmapRect2;
extern LWRP_EXPORT pL_CREATEBITMAP  pL_CreateBitmap;
extern LWRP_EXPORT pL_CREATEBITMAPLIST pL_CreateBitmapList;
extern LWRP_EXPORT pL_CREATEUSERMATCHTABLE   pL_CreateUserMatchTable;
extern LWRP_EXPORT pL_DEFAULTDITHERING pL_DefaultDithering;
extern LWRP_EXPORT pL_DELETEBITMAPLISTITEMS  pL_DeleteBitmapListItems;
extern LWRP_EXPORT pL_DESTROYBITMAPLIST   pL_DestroyBitmapList;
extern LWRP_EXPORT pL_DITHERLINE pL_DitherLine;
extern LWRP_EXPORT pL_COPYBITMAPPALETTE   pL_CopyBitmapPalette;
extern LWRP_EXPORT pL_DUPBITMAPPALETTE pL_DupBitmapPalette;
extern LWRP_EXPORT pL_DUPPALETTE pL_DupPalette;
extern LWRP_EXPORT pL_EXPANDROW  pL_ExpandRow;
extern LWRP_EXPORT pL_EXPANDROWS pL_ExpandRows;
extern LWRP_EXPORT pL_FILLBITMAP pL_FillBitmap;
extern LWRP_EXPORT pL_FLIPBITMAP pL_FlipBitmap;
extern LWRP_EXPORT pL_FREEBITMAP pL_FreeBitmap;
extern LWRP_EXPORT pL_FREEUSERMATCHTABLE  pL_FreeUserMatchTable;
extern LWRP_EXPORT pL_GETBITMAPCOLORS  pL_GetBitmapColors;
extern LWRP_EXPORT pL_GETBITMAPLISTCOUNT  pL_GetBitmapListCount;
extern LWRP_EXPORT pL_GETBITMAPLISTITEM   pL_GetBitmapListItem;
extern LWRP_EXPORT pL_GETBITMAPROW  pL_GetBitmapRow;
extern LWRP_EXPORT pL_GETBITMAPROWCOL  pL_GetBitmapRowCol;
extern LWRP_EXPORT pL_GETBITMAPROWCOLCOMPRESSED pL_GetBitmapRowColCompressed;
extern LWRP_EXPORT pL_GETBITMAPROWCOMPRESSED pL_GetBitmapRowCompressed;
extern LWRP_EXPORT pL_GETFIXEDPALETTE  pL_GetFixedPalette;
extern LWRP_EXPORT pL_GETPIXELCOLOR pL_GetPixelColor;
extern LWRP_EXPORT pL_GETSTATUSCALLBACK   pL_GetStatusCallBack;
extern LWRP_EXPORT pL_SETSTATUSCALLBACK   pL_SetStatusCallBack;
extern LWRP_EXPORT pL_GETCOPYSTATUSCALLBACK  pL_GetCopyStatusCallBack;
extern LWRP_EXPORT pL_SETCOPYSTATUSCALLBACK  pL_SetCopyStatusCallBack;
extern LWRP_EXPORT pL_GRAYSCALEBITMAP  pL_GrayScaleBitmap;
extern LWRP_EXPORT pL_INITBITMAP pL_InitBitmap;
extern LWRP_EXPORT pL_INSERTBITMAPLISTITEM   pL_InsertBitmapListItem;
extern LWRP_EXPORT pL_ISGRAYSCALEBITMAP   pL_IsGrayScaleBitmap;
extern LWRP_EXPORT pL_ISSUPPORTLOCKED  pL_IsSupportLocked;
extern LWRP_EXPORT pL_POINTFROMBITMAP  pL_PointFromBitmap;
extern LWRP_EXPORT pL_POINTTOBITMAP pL_PointToBitmap;
extern LWRP_EXPORT pL_PUTBITMAPCOLORS  pL_PutBitmapColors;
extern LWRP_EXPORT pL_PUTBITMAPROW  pL_PutBitmapRow;
extern LWRP_EXPORT pL_PUTBITMAPROWCOL  pL_PutBitmapRowCol;
extern LWRP_EXPORT pL_PUTBITMAPROWCOLCOMPRESSED pL_PutBitmapRowColCompressed;
extern LWRP_EXPORT pL_PUTBITMAPROWCOMPRESSED pL_PutBitmapRowCompressed;
extern LWRP_EXPORT pL_PUTPIXELCOLOR pL_PutPixelColor;
extern LWRP_EXPORT pL_RECTFROMBITMAP   pL_RectFromBitmap;
extern LWRP_EXPORT pL_RECTTOBITMAP  pL_RectToBitmap;
extern LWRP_EXPORT pL_REDIRECTIO pL_RedirectIO;
extern LWRP_EXPORT pL_RELEASEBITMAP pL_ReleaseBitmap;
extern LWRP_EXPORT pL_REMOVEBITMAPLISTITEM   pL_RemoveBitmapListItem;
extern LWRP_EXPORT pL_RESIZE  pL_Resize;
extern LWRP_EXPORT pL_RESIZEBITMAP  pL_ResizeBitmap;
extern LWRP_EXPORT pL_REVERSEBITMAP pL_ReverseBitmap;
extern LWRP_EXPORT pL_ROTATEBITMAP  pL_RotateBitmap;
extern LWRP_EXPORT pL_ROTATEBITMAPVIEWPERSPECTIVE  pL_RotateBitmapViewPerspective;
extern LWRP_EXPORT pL_SETBITMAPDATAPOINTER   pL_SetBitmapDataPointer;
extern LWRP_EXPORT pL_SETBITMAPLISTITEM   pL_SetBitmapListItem;
extern LWRP_EXPORT pL_SETUSERMATCHTABLE   pL_SetUserMatchTable;
extern LWRP_EXPORT pL_SIZEBITMAP pL_SizeBitmap;
extern LWRP_EXPORT pL_STARTDITHERING   pL_StartDithering;
extern LWRP_EXPORT pL_STARTRESIZE   pL_StartResize;
extern LWRP_EXPORT pL_STOPDITHERING pL_StopDithering;
extern LWRP_EXPORT pL_STOPRESIZE pL_StopResize;
extern LWRP_EXPORT pL_TRANSLATEBITMAPCOLOR   pL_TranslateBitmapColor;
extern LWRP_EXPORT pL_TOGGLEBITMAPCOMPRESSION   pL_ToggleBitmapCompression;
extern LWRP_EXPORT pL_TRIMBITMAP pL_TrimBitmap;
extern LWRP_EXPORT pL_UNLOCKSUPPORT pL_UnlockSupport;
extern LWRP_EXPORT pL_KERNELHASEXPIRED pL_KernelHasExpired;
extern LWRP_EXPORT pL_FLIPBITMAPVIEWPERSPECTIVE pL_FlipBitmapViewPerspective;
extern LWRP_EXPORT pL_REVERSEBITMAPVIEWPERSPECTIVE pL_ReverseBitmapViewPerspective;
extern LWRP_EXPORT pL_STARTRESIZEBITMAP   pL_StartResizeBitmap;
extern LWRP_EXPORT pL_GETRESIZEDROWCOL pL_GetResizedRowCol;
extern LWRP_EXPORT pL_STOPRESIZEBITMAP pL_StopResizeBitmap;
extern LWRP_EXPORT pL_MOVEBITMAPLISTITEMS pL_MoveBitmapListItems;
extern LWRP_EXPORT pL_CHANGEBITMAPCOMPRESSION   pL_ChangeBitmapCompression;
extern LWRP_EXPORT pL_GETPIXELDATA  pL_GetPixelData;
extern LWRP_EXPORT pL_PUTPIXELDATA  pL_PutPixelData;
extern LWRP_EXPORT pL_SETDEFAULTMEMORYTYPE   pL_SetDefaultMemoryType;
extern LWRP_EXPORT pL_GETDEFAULTMEMORYTYPE   pL_GetDefaultMemoryType;
extern LWRP_EXPORT pL_SETMEMORYTHRESHOLDS pL_SetMemoryThresholds;
extern LWRP_EXPORT pL_GETMEMORYTHRESHOLDS pL_GetMemoryThresholds;
extern LWRP_EXPORT pL_SETBITMAPMEMORYINFO pL_SetBitmapMemoryInfo;
extern LWRP_EXPORT pL_GETBITMAPMEMORYINFO pL_GetBitmapMemoryInfo;
extern LWRP_EXPORT pL_SETTEMPDIRECTORY pL_SetTempDirectory;
extern LWRP_EXPORT pL_GETTEMPDIRECTORY pL_GetTempDirectory;
extern LWRP_EXPORT pL_SETBITMAPPALETTE pL_SetBitmapPalette;
extern LWRP_EXPORT pL_SCRAMBLEBITMAP   pL_ScrambleBitmap;
extern LWRP_EXPORT pL_COMBINEBITMAPWARP   pL_CombineBitmapWarp;
extern LWRP_EXPORT pL_SETOVERLAYBITMAP pL_SetOverlayBitmap;
extern LWRP_EXPORT pL_GETOVERLAYBITMAP pL_GetOverlayBitmap;
extern LWRP_EXPORT pL_SETOVERLAYATTRIBUTES   pL_SetOverlayAttributes;
extern LWRP_EXPORT pL_GETOVERLAYATTRIBUTES   pL_GetOverlayAttributes;
extern LWRP_EXPORT pL_UPDATEBITMAPOVERLAYBITS   pL_UpdateBitmapOverlayBits;
extern LWRP_EXPORT pL_GETOVERLAYCOUNT  pL_GetOverlayCount;
extern LWRP_EXPORT pL_CREATELEADDC  pL_CreateLeadDC;
extern LWRP_EXPORT pL_DELETELEADDC  pL_DeleteLeadDC;
extern LWRP_EXPORT pL_GETBITMAPALPHA   pL_GetBitmapAlpha;
extern LWRP_EXPORT pL_SETBITMAPALPHA   pL_SetBitmapAlpha;
extern LWRP_EXPORT pL_SETBITMAPALPHAVALUES   pL_SetBitmapAlphaValues;
extern LWRP_EXPORT pL_SHEARBITMAP   pL_ShearBitmap;
extern LWRP_EXPORT pL_VERSIONINFO   pL_VersionInfo;
extern LWRP_EXPORT pL_HASEXPIRED pL_HasExpired;
extern LWRP_EXPORT pL_CREATEBITMAPLISTOPTPAL  pL_CreateBitmapListOptPal ;
extern LWRP_EXPORT pL_CREATEGRAYSCALEBITMAP  pL_CreateGrayScaleBitmap;

//-----------------------------------------------------------------------------
//--LTIMG.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_CHANGEBITMAPCONTRAST   pL_ChangeBitmapContrast;
extern LWRP_EXPORT pL_CHANGEBITMAPHUE  pL_ChangeBitmapHue;
extern LWRP_EXPORT pL_CHANGEBITMAPINTENSITY  pL_ChangeBitmapIntensity;
extern LWRP_EXPORT pL_CHANGEBITMAPSATURATION pL_ChangeBitmapSaturation;
extern LWRP_EXPORT pL_INVERTBITMAP  pL_InvertBitmap;
extern LWRP_EXPORT pL_REMAPBITMAPINTENSITY   pL_RemapBitmapIntensity;
extern LWRP_EXPORT pL_SHARPENBITMAP pL_SharpenBitmap;
extern LWRP_EXPORT pL_GETBITMAPCOLORCOUNT pL_GetBitmapColorCount;
extern LWRP_EXPORT pL_GETAUTOTRIMRECT  pL_GetAutoTrimRect;
extern LWRP_EXPORT pL_AUTOTRIMBITMAP   pL_AutoTrimBitmap;
extern LWRP_EXPORT pL_UNSHARPMASKBITMAP   pL_UnsharpMaskBitmap;
extern LWRP_EXPORT pL_DESKEWBITMAP  pL_DeskewBitmap;
extern LWRP_EXPORT pL_DESKEWBITMAP  pL_DeskewCheckBitmap;
extern LWRP_EXPORT pL_DESKEWBITMAPEXT  pL_DeskewBitmapExt;
extern LWRP_EXPORT pL_BLANKPAGEDETECTORBITMAP  pL_BlankPageDetectorBitmap;
extern LWRP_EXPORT pL_AUTOBINARIZEBITMAP  pL_AutoBinarizeBitmap;
extern LWRP_EXPORT pL_STARTFASTMAGICWANDENGINE  pL_StartFastMagicWandEngine;
extern LWRP_EXPORT pL_ENDFASTMAGICWANDENGINE  pL_EndFastMagicWandEngine;
extern LWRP_EXPORT pL_FASTMAGICWAND  pL_FastMagicWand;
extern LWRP_EXPORT pL_DELETEOBJECTINFO  pL_DeleteObjectInfo;
extern LWRP_EXPORT pL_TISSUEEQUALIZEBITMAP  pL_TissueEqualizeBitmap;
extern LWRP_EXPORT pL_SIGMAFILTERBITMAP  pL_SigmaFilterBitmap;
extern LWRP_EXPORT pL_AUTOSEGMENTBITMAP  pL_AutoSegmentBitmap;
extern LWRP_EXPORT pL_DESPECKLEBITMAP  pL_DespeckleBitmap;
extern LWRP_EXPORT pL_EDGEDETECTORBITMAP  pL_EdgeDetectorBitmap;
extern LWRP_EXPORT pL_INTENSITYDETECTBITMAP  pL_IntensityDetectBitmap;
extern LWRP_EXPORT pL_AVERAGEFILTERBITMAP pL_AverageFilterBitmap;
extern LWRP_EXPORT pL_BINARYFILTERBITMAP  pL_BinaryFilterBitmap;
extern LWRP_EXPORT pL_COMBINEBITMAP pL_CombineBitmap;
extern LWRP_EXPORT pL_MINFILTERBITMAP  pL_MinFilterBitmap;
extern LWRP_EXPORT pL_MAXFILTERBITMAP  pL_MaxFilterBitmap;
extern LWRP_EXPORT pL_ADDBITMAPNOISE   pL_AddBitmapNoise;
extern LWRP_EXPORT pL_COLORMERGEBITMAP pL_ColorMergeBitmap;
extern LWRP_EXPORT pL_COLORSEPARATEBITMAP pL_ColorSeparateBitmap;
extern LWRP_EXPORT pL_EMBOSSBITMAP  pL_EmbossBitmap;
extern LWRP_EXPORT pL_GAMMACORRECTBITMAP  pL_GammaCorrectBitmap;
extern LWRP_EXPORT pL_GETMINMAXBITS pL_GetMinMaxBits;
extern LWRP_EXPORT pL_GETMINMAXVAL  pL_GetMinMaxVal;
extern LWRP_EXPORT pL_HISTOCONTRASTBITMAP pL_HistoContrastBitmap;
extern LWRP_EXPORT pL_MEDIANFILTERBITMAP  pL_MedianFilterBitmap;
extern LWRP_EXPORT pL_MOSAICBITMAP  pL_MosaicBitmap;
extern LWRP_EXPORT pL_POSTERIZEBITMAP  pL_PosterizeBitmap;
extern LWRP_EXPORT pL_LINEPROFILE   pL_LineProfile;
extern LWRP_EXPORT pL_GRAYSCALEBITMAPEXT  pL_GrayScaleBitmapExt;
extern LWRP_EXPORT pL_SWAPCOLORS pL_SwapColors;
extern LWRP_EXPORT pL_BALANCECOLORS pL_BalanceColors;
extern LWRP_EXPORT pL_CONVERTTOCOLOREDGRAY   pL_ConvertToColoredGray;
extern LWRP_EXPORT pL_HISTOEQUALIZEBITMAP pL_HistoEqualizeBitmap;
extern LWRP_EXPORT pL_ALPHABLENDBITMAP pL_AlphaBlendBitmap;
extern LWRP_EXPORT pL_ANTIALIASBITMAP  pL_AntiAliasBitmap;
extern LWRP_EXPORT pL_GETBITMAPHISTOGRAM  pL_GetBitmapHistogram;
extern LWRP_EXPORT pL_GETUSERLOOKUPTABLE  pL_GetUserLookUpTable;
extern LWRP_EXPORT pL_GETFUNCTIONALLOOKUPTABLE  pL_GetFunctionalLookupTable;
extern LWRP_EXPORT pL_CONVERTBITMAPSIGNEDTOUNSIGNED   pL_ConvertBitmapSignedToUnsigned;
extern LWRP_EXPORT pL_TEXTUREALPHABLENDBITMAP   pL_TextureAlphaBlendBitmap;
extern LWRP_EXPORT pL_REMAPBITMAPHUE   pL_RemapBitmapHue;
extern LWRP_EXPORT pL_MULTIPLYBITMAP   pL_MultiplyBitmap;
extern LWRP_EXPORT pL_LOCALHISTOEQUALIZEBITMAP  pL_LocalHistoEqualizeBitmap;
extern LWRP_EXPORT pL_GETCURVEPOINTS   pL_GetCurvePoints;
extern LWRP_EXPORT pL_SOLARIZEBITMAP   pL_SolarizeBitmap;
extern LWRP_EXPORT pL_SPATIALFILTERBITMAP pL_SpatialFilterBitmap;
extern LWRP_EXPORT pL_STRETCHBITMAPINTENSITY pL_StretchBitmapIntensity;
extern LWRP_EXPORT pL_WINDOWLEVELBITMAP   pL_WindowLevelBitmap;
#ifdef LEADTOOLS_V16_OR_LATER
extern LWRP_EXPORT pL_WINDOWLEVELBITMAPEXT   pL_WindowLevelBitmapExt;
extern LWRP_EXPORT pL_RAKEREMOVEBITMAP pL_RakeRemoveBitmap;
extern LWRP_EXPORT pL_OBJECTCOUNTER pL_ObjectCounter;
#endif // #ifdef LEADTOOLS_V16_OR_LATER
extern LWRP_EXPORT pL_GAUSSIANFILTERBITMAP   pL_GaussianFilterBitmap;
extern LWRP_EXPORT pL_SMOOTHBITMAP  pL_SmoothBitmap;
extern LWRP_EXPORT pL_LINEREMOVEBITMAP pL_LineRemoveBitmap;

extern LWRP_EXPORT pL_BORDERREMOVEBITMAP  pL_BorderRemoveBitmap;
extern LWRP_EXPORT pL_INVERTEDTEXTBITMAP  pL_InvertedTextBitmap;
extern LWRP_EXPORT pL_DOTREMOVEBITMAP  pL_DotRemoveBitmap;
extern LWRP_EXPORT pL_HOLEPUNCHREMOVEBITMAP  pL_HolePunchRemoveBitmap;

// L_StapleRemoveBitmap reserved for future use
extern LWRP_EXPORT pL_STAPLEREMOVEBITMAP  pL_StapleRemoveBitmap;
extern LWRP_EXPORT pL_OILIFYBITMAP  pL_OilifyBitmap;
extern LWRP_EXPORT pL_PICTURIZEBITMAP  pL_PicturizeBitmap;
extern LWRP_EXPORT pL_CONTOURFILTERBITMAP pL_ContourFilterBitmap;
extern LWRP_EXPORT pL_RESIZEBITMAPRGN  pL_ResizeBitmapRgn;
extern LWRP_EXPORT pL_CREATEFADEDMASK  pL_CreateFadedMask;
extern LWRP_EXPORT pL_ADDBITMAPS pL_AddBitmaps;
extern LWRP_EXPORT pL_MOTIONBLURBITMAP pL_MotionBlurBitmap;
extern LWRP_EXPORT pL_PICTURIZEBITMAPLIST pL_PicturizeBitmapList;
extern LWRP_EXPORT pL_PICTURIZEBITMAPSINGLE  pL_PicturizeBitmapSingle;
extern LWRP_EXPORT pL_ADDBORDER  pL_AddBorder;
extern LWRP_EXPORT pL_ADDFRAME   pL_AddFrame;
extern LWRP_EXPORT pL_ADDWEIGHTEDBITMAPS  pL_AddWeightedBitmaps;
extern LWRP_EXPORT pL_ADDMESSAGETOBITMAP  pL_AddMessageToBitmap;
extern LWRP_EXPORT pL_EXTRACTMESSAGEFROMBITMAP  pL_ExtractMessageFromBitmap;
extern LWRP_EXPORT pL_PIXELATEBITMAP   pL_PixelateBitmap;
extern LWRP_EXPORT pL_SPHERIZEBITMAP   pL_SpherizeBitmap;
extern LWRP_EXPORT pL_CYLINDRICALBITMAP   pL_CylindricalBitmap;
extern LWRP_EXPORT pL_BENDINGBITMAP pL_BendingBitmap;
extern LWRP_EXPORT pL_PUNCHBITMAP   pL_PunchBitmap;
extern LWRP_EXPORT pL_POLARBITMAP   pL_PolarBitmap;
extern LWRP_EXPORT pL_RADIALBLURBITMAP pL_RadialBlurBitmap;
extern LWRP_EXPORT pL_RIPPLEBITMAP  pL_RippleBitmap;
extern LWRP_EXPORT pL_SWIRLBITMAP   pL_SwirlBitmap;
extern LWRP_EXPORT pL_ZOOMBLURBITMAP   pL_ZoomBlurBitmap;
extern LWRP_EXPORT pL_FREEHANDWAVEBITMAP  pL_FreeHandWaveBitmap;
extern LWRP_EXPORT pL_IMPRESSIONISTBITMAP pL_ImpressionistBitmap;
extern LWRP_EXPORT pL_RADWAVEBITMAP pL_RadWaveBitmap;
extern LWRP_EXPORT pL_FREEHANDSHEARBITMAP pL_FreeHandShearBitmap;
extern LWRP_EXPORT pL_WAVEBITMAP pL_WaveBitmap;
extern LWRP_EXPORT pL_WINDBITMAP pL_WindBitmap;
extern LWRP_EXPORT pL_ZOOMWAVEBITMAP   pL_ZoomWaveBitmap;
extern LWRP_EXPORT pL_REMOVEREDEYEBITMAP  pL_RemoveRedeyeBitmap;
//*** v14 functions ***
extern LWRP_EXPORT pL_DEINTERLACEBITMAP   pL_DeinterlaceBitmap;
extern LWRP_EXPORT pL_SAMPLETARGETBITMAP  pL_SampleTargetBitmap;
extern LWRP_EXPORT pL_HALFTONEBITMAP   pL_HalfToneBitmap;
extern LWRP_EXPORT pL_HOLESREMOVALBITMAPRGN  pL_HolesRemovalBitmapRgn;
extern LWRP_EXPORT pL_CUBISMBITMAP  pL_CubismBitmap;
extern LWRP_EXPORT pL_LIGHTCONTROLBITMAP  pL_LightControlBitmap;
extern LWRP_EXPORT pL_GLASSEFFECTBITMAP   pL_GlassEffectBitmap;
extern LWRP_EXPORT pL_LENSFLAREBITMAP  pL_LensFlareBitmap;
extern LWRP_EXPORT pL_BUMPMAPBITMAP pL_BumpMapBitmap;
extern LWRP_EXPORT pL_GLOWFILTERBITMAP pL_GlowFilterBitmap;
extern LWRP_EXPORT pL_EDGEDETECTSTATISTICALBITMAP  pL_EdgeDetectStatisticalBitmap;
extern LWRP_EXPORT pL_DESATURATEBITMAP pL_DesaturateBitmap;
extern LWRP_EXPORT pL_SMOOTHEDGESBITMAP   pL_SmoothEdgesBitmap;
extern LWRP_EXPORT pL_AUTOBINARYBITMAP pL_AutoBinaryBitmap;
extern LWRP_EXPORT pL_CHANNELMIX pL_ChannelMix;
extern LWRP_EXPORT pL_PLANEBITMAP   pL_PlaneBitmap;
extern LWRP_EXPORT pL_PLANEBENDBITMAP  pL_PlaneBendBitmap;
extern LWRP_EXPORT pL_TUNNELBITMAP  pL_TunnelBitmap;
extern LWRP_EXPORT pL_FREERADBENDBITMAP   pL_FreeRadBendBitmap;
extern LWRP_EXPORT pL_FREEPLANEBENDBITMAP pL_FreePlaneBendBitmap;
extern LWRP_EXPORT pL_OCEANBITMAP   pL_OceanBitmap;
extern LWRP_EXPORT pL_LIGHTBITMAP   pL_LightBitmap;
extern LWRP_EXPORT pL_DRYBITMAP  pL_DryBitmap;
extern LWRP_EXPORT pL_DRAWSTARBITMAP   pL_DrawStarBitmap;
extern LWRP_EXPORT pL_FFTBITMAP  pL_FFTBitmap;
extern LWRP_EXPORT pL_FTDISPLAYBITMAP  pL_FTDisplayBitmap;
extern LWRP_EXPORT pL_DFTBITMAP  pL_DFTBitmap;
extern LWRP_EXPORT pL_FRQFILTERBITMAP  pL_FrqFilterBitmap;
extern LWRP_EXPORT pL_FRQFILTERMASKBITMAP pL_FrqFilterMaskBitmap;
extern LWRP_EXPORT pL_ALLOCFTARRAY  pL_AllocFTArray;
extern LWRP_EXPORT pL_FREEFTARRAY   pL_FreeFTArray;
extern LWRP_EXPORT pL_GRAYSCALETODUOTONE  pL_GrayScaleToDuotone;
extern LWRP_EXPORT pL_GRAYSCALETOMULTITONE   pL_GrayScaleToMultitone;
extern LWRP_EXPORT pL_SKELETONBITMAP   pL_SkeletonBitmap;
extern LWRP_EXPORT pL_COLORLEVELBITMAP pL_ColorLevelBitmap;
extern LWRP_EXPORT pL_AUTOCOLORLEVELBITMAP   pL_AutoColorLevelBitmap;
extern LWRP_EXPORT pL_SELECTIVECOLORBITMAP   pL_SelectiveColorBitmap;
extern LWRP_EXPORT pL_CORRELATIONBITMAP   pL_CorrelationBitmap;
extern LWRP_EXPORT pL_GETOBJECTINFO pL_GetObjectInfo;
extern LWRP_EXPORT pL_GETRGNCONTOURPOINTS pL_GetRgnContourPoints;
extern LWRP_EXPORT pL_SEGMENTBITMAP pL_SegmentBitmap;
extern LWRP_EXPORT pL_GETRGNPERIMETERLENGTH  pL_GetRgnPerimeterLength;
extern LWRP_EXPORT pL_GETFERETSDIAMETER   pL_GetFeretsDiameter;
extern LWRP_EXPORT pL_COLORREPLACEBITMAP  pL_ColorReplaceBitmap;
extern LWRP_EXPORT pL_CHANGEHUESATINTBITMAP  pL_ChangeHueSatIntBitmap;
extern LWRP_EXPORT pL_GETBITMAPSTATISTICSINFO   pL_GetBitmapStatisticsInfo;
extern LWRP_EXPORT pL_USERFILTERBITMAP pL_UserFilterBitmap;
extern LWRP_EXPORT pL_COLORREPLACEWEIGHTSBITMAP pL_ColorReplaceWeightsBitmap;
extern LWRP_EXPORT pL_DIRECTIONEDGESTATISTICALBITMAP  pL_DirectionEdgeStatisticalBitmap;
extern LWRP_EXPORT pL_MATHFUNCTIONBITMAP  pL_MathFunctionBitmap;
extern LWRP_EXPORT pL_COLORTHRESHOLDBITMAP   pL_ColorThresholdBitmap;
extern LWRP_EXPORT pL_REVEFFECTBITMAP  pL_RevEffectBitmap;
extern LWRP_EXPORT pL_ADDSHADOWBITMAP  pL_AddShadowBitmap;
extern LWRP_EXPORT pL_SUBTRACTBACKGROUNDBITMAP  pL_SubtractBackgroundBitmap;
extern LWRP_EXPORT pL_APPLYMODALITYLUT pL_ApplyModalityLUT;
extern LWRP_EXPORT pL_APPLYLINEARMODALITYLUT pL_ApplyLinearModalityLUT;
extern LWRP_EXPORT pL_APPLYVOILUT   pL_ApplyVOILUT;
extern LWRP_EXPORT pL_APPLYLINEARVOILUT   pL_ApplyLinearVOILUT;
extern LWRP_EXPORT pL_GETLINEARVOILUT  pL_GetLinearVOILUT;
extern LWRP_EXPORT pL_COUNTLUTCOLORS   pL_CountLUTColors;
#ifdef LEADTOOLS_V16_OR_LATER
extern LWRP_EXPORT pL_COUNTLUTCOLORSEXT   pL_CountLUTColorsExt;
#endif // #ifdef LEADTOOLS_V16_OR_LATER
extern LWRP_EXPORT pL_ADAPTIVECONTRASTBITMAP pL_AdaptiveContrastBitmap;
extern LWRP_EXPORT pL_AGINGBITMAP   pL_AgingBitmap;
extern LWRP_EXPORT pL_DYNAMICBINARYBITMAP pL_DynamicBinaryBitmap;
extern LWRP_EXPORT pL_COLORINTENSITYBALANCE  pL_ColorIntensityBalance;
extern LWRP_EXPORT pL_APPLYMATHLOGICBITMAP   pL_ApplyMathLogicBitmap;
extern LWRP_EXPORT pL_EDGEDETECTEFFECTBITMAP pL_EdgeDetectEffectBitmap;
extern LWRP_EXPORT pL_FUNCTIONALLIGHTBITMAP  pL_FunctionalLightBitmap;
extern LWRP_EXPORT pL_DICEEFFECTBITMAP pL_DiceEffectBitmap;
extern LWRP_EXPORT pL_PUZZLEEFFECTBITMAP  pL_PuzzleEffectBitmap;
extern LWRP_EXPORT pL_RINGEFFECTBITMAP pL_RingEffectBitmap;
extern LWRP_EXPORT pL_MULTISCALEENHANCEMENTBITMAP  pL_MultiScaleEnhancementBitmap;
extern LWRP_EXPORT pL_SHIFTBITMAPDATA  pL_ShiftBitmapData;
extern LWRP_EXPORT pL_SELECTBITMAPDATA pL_SelectBitmapData;
extern LWRP_EXPORT pL_COLORIZEGRAYBITMAP  pL_ColorizeGrayBitmap;
extern LWRP_EXPORT pL_DIGITALSUBTRACTBITMAP  pL_DigitalSubtractBitmap;
extern LWRP_EXPORT pL_CONTBRIGHTINTBITMAP pL_ContBrightIntBitmap;
extern LWRP_EXPORT pL_ISREGMARKBITMAP  pL_IsRegMarkBitmap;
extern LWRP_EXPORT pL_GETMARKSCENTERMASSBITMAP  pL_GetMarksCenterMassBitmap;
extern LWRP_EXPORT pL_SEARCHREGMARKSBITMAP   pL_SearchRegMarksBitmap;
extern LWRP_EXPORT pL_GETTRANSFORMATIONPARAMETERS  pL_GetTransformationParameters;
extern LWRP_EXPORT pL_APPLYTRANSFORMATIONPARAMETERS   pL_ApplyTransformationParameters;
extern LWRP_EXPORT pL_CONVERTBITMAPUNSIGNEDTOSIGNED   pL_ConvertBitmapUnsignedToSigned;
extern LWRP_EXPORT pL_OFFSETBITMAP  pL_OffsetBitmap;
extern LWRP_EXPORT pL_BRICKSTEXTUREBITMAP pL_BricksTextureBitmap;
extern LWRP_EXPORT pL_CANVASBITMAP  pL_CanvasBitmap;
extern LWRP_EXPORT pL_FRAGMENTBITMAP   pL_FragmentBitmap;
extern LWRP_EXPORT pL_CLOUDSBITMAP  pL_CloudsBitmap;
extern LWRP_EXPORT pL_VIGNETTEBITMAP   pL_VignetteBitmap;
extern LWRP_EXPORT pL_MOSAICTILESBITMAP   pL_MosaicTilesBitmap;
extern LWRP_EXPORT pL_ROMANMOSAICBITMAP   pL_RomanMosaicBitmap;
extern LWRP_EXPORT pL_GAMMACORRECTBITMAPEXT  pL_GammaCorrectBitmapExt;
extern LWRP_EXPORT pL_MASKCONVOLUTIONBITMAP  pL_MaskConvolutionBitmap;
extern LWRP_EXPORT pL_PLASMAFILTERBITMAP  pL_PlasmaFilterBitmap;
extern LWRP_EXPORT pL_ADJUSTBITMAPTINT pL_AdjustBitmapTint;
extern LWRP_EXPORT pL_COLOREDPENCILBITMAP pL_ColoredPencilBitmap;
extern LWRP_EXPORT pL_PERLINBITMAP  pL_PerlinBitmap;
extern LWRP_EXPORT pL_DIFFUSEGLOWBITMAP   pL_DiffuseGlowBitmap;
extern LWRP_EXPORT pL_DISPLACEMAPBITMAP   pL_DisplaceMapBitmap;
extern LWRP_EXPORT pL_HIGHPASSFILTERBITMAP   pL_HighPassFilterBitmap;
extern LWRP_EXPORT pL_ZIGZAGBITMAP  pL_ZigZagBitmap;
extern LWRP_EXPORT pL_COLORHALFTONEBITMAP pL_ColorHalfToneBitmap;
extern LWRP_EXPORT pL_COLOREDBALLSBITMAP  pL_ColoredBallsBitmap;
extern LWRP_EXPORT pL_PERSPECTIVEBITMAP   pL_PerspectiveBitmap;
extern LWRP_EXPORT pL_POINTILLISTBITMAP   pL_PointillistBitmap;
extern LWRP_EXPORT pL_HALFTONEPATTERNBITMAP  pL_HalfTonePatternBitmap;
extern LWRP_EXPORT pL_CORRELATIONLISTBITMAP  pL_CorrelationListBitmap;
extern LWRP_EXPORT pL_SETKAUFMANNRGNBITMAP   pL_SetKaufmannRgnBitmap;
extern LWRP_EXPORT pL_SLICEBITMAP   pL_SliceBitmap;
extern LWRP_EXPORT pL_SHIFTMINIMUMTOZERO  pL_ShiftMinimumToZero;
extern LWRP_EXPORT pL_SHIFTZEROTONEGATIVE pL_ShiftZeroToNegative;
extern LWRP_EXPORT pL_FEATHERALPHABLENDBITMAP   pL_FeatherAlphaBlendBitmap;
extern LWRP_EXPORT pL_SIZEBITMAPINTERPOLATE  pL_SizeBitmapInterpolate;
extern LWRP_EXPORT pL_COLOREDPENCILBITMAPEXT pL_ColoredPencilBitmapExt;
extern LWRP_EXPORT pL_INTELLIGENTUPSCALEBITMAP pL_IntelligentUpScaleBitmap;
extern LWRP_EXPORT pL_INTELLIGENTDOWNSCALEBITMAP pL_IntelligentDownScaleBitmap;
//-----------------------------------------------------------------------------
//--LTDIS.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------

extern LWRP_EXPORT pL_APPENDPLAYBACK pL_AppendPlayback;
extern LWRP_EXPORT pL_CANCELPLAYBACKWAIT  pL_CancelPlaybackWait;
extern LWRP_EXPORT pL_CHANGEFROMDDB pL_ChangeFromDDB;
extern LWRP_EXPORT pL_CHANGETODDB   pL_ChangeToDDB;
extern LWRP_EXPORT pL_CLEARPLAYBACKUPDATERECT   pL_ClearPlaybackUpdateRect;
extern LWRP_EXPORT pL_CLIPBOARDREADY   pL_ClipboardReady;
extern LWRP_EXPORT pL_CONVERTCOLORSPACE   pL_ConvertColorSpace;
extern LWRP_EXPORT pL_CONVERTFROMDDB   pL_ConvertFromDDB;
extern LWRP_EXPORT pL_CONVERTTODDB  pL_ConvertToDDB;
extern LWRP_EXPORT pL_COPYFROMCLIPBOARD   pL_CopyFromClipboard;
extern LWRP_EXPORT pL_COPYTOCLIPBOARD  pL_CopyToClipboard;
extern LWRP_EXPORT pL_CREATEPAINTPALETTE  pL_CreatePaintPalette;
extern LWRP_EXPORT pL_CREATEPLAYBACK   pL_CreatePlayback;
extern LWRP_EXPORT pL_DESTROYPLAYBACK  pL_DestroyPlayback;
extern LWRP_EXPORT pL_GETDISPLAYMODE   pL_GetDisplayMode;
extern LWRP_EXPORT pL_GETPAINTCONTRAST pL_GetPaintContrast;
extern LWRP_EXPORT pL_GETPAINTGAMMA pL_GetPaintGamma;
extern LWRP_EXPORT pL_GETPAINTINTENSITY   pL_GetPaintIntensity;
extern LWRP_EXPORT pL_GETPLAYBACKDELAY pL_GetPlaybackDelay;
extern LWRP_EXPORT pL_GETPLAYBACKINDEX pL_GetPlaybackIndex;
extern LWRP_EXPORT pL_GETPLAYBACKSTATE pL_GetPlaybackState;
extern LWRP_EXPORT pL_GETPLAYBACKUPDATERECT  pL_GetPlaybackUpdateRect;
extern LWRP_EXPORT pL_PAINTDC pL_PaintDC;
extern LWRP_EXPORT pL_PAINTDCBUFFER pL_PaintDCBuffer;
extern LWRP_EXPORT pL_PROCESSPLAYBACK  pL_ProcessPlayback;
extern LWRP_EXPORT pL_RGBTOHSV   pL_RGBtoHSV;
extern LWRP_EXPORT pL_HSVTORGB   pL_HSVtoRGB;
extern LWRP_EXPORT pL_SETDISPLAYMODE   pL_SetDisplayMode;
extern LWRP_EXPORT pL_SETPAINTCONTRAST pL_SetPaintContrast;
extern LWRP_EXPORT pL_SETPAINTGAMMA pL_SetPaintGamma;
extern LWRP_EXPORT pL_SETPAINTINTENSITY   pL_SetPaintIntensity;
extern LWRP_EXPORT pL_SETPLAYBACKINDEX pL_SetPlaybackIndex;
extern LWRP_EXPORT pL_UNDERLAYBITMAP   pL_UnderlayBitmap;
extern LWRP_EXPORT pL_VALIDATEPLAYBACKLINES  pL_ValidatePlaybackLines;
extern LWRP_EXPORT pL_WINDOWLEVEL   pL_WindowLevel;
extern LWRP_EXPORT pL_WINDOWLEVELFILLLUT  pL_WindowLevelFillLUT;
extern LWRP_EXPORT pL_WINDOWLEVELFILLLUT2 pL_WindowLevelFillLUT2;
extern LWRP_EXPORT pL_GETBITMAPCLIPSEGMENTS  pL_GetBitmapClipSegments;
extern LWRP_EXPORT pL_GETBITMAPCLIPSEGMENTSMAX  pL_GetBitmapClipSegmentsMax;
extern LWRP_EXPORT pL_STARTMAGGLASS pL_StartMagGlass;
extern LWRP_EXPORT pL_STOPMAGGLASS  pL_StopMagGlass;
extern LWRP_EXPORT pL_UPDATEMAGGLASSRECT  pL_UpdateMagGlassRect;
extern LWRP_EXPORT pL_UPDATEMAGGLASSPAINTFLAGS  pL_UpdateMagGlassPaintFlags;
extern LWRP_EXPORT pL_UPDATEMAGGLASS   pL_UpdateMagGlass;
extern LWRP_EXPORT pL_UPDATEMAGGLASSBITMAP   pL_UpdateMagGlassBitmap;
extern LWRP_EXPORT pL_WINDOWHASMAGGLASS   pL_WindowHasMagGlass;
extern LWRP_EXPORT pL_SETMAGGLASSOWNERDRAWCALLBACK pL_SetMagGlassOwnerDrawCallback;
extern LWRP_EXPORT pL_SETMAGGLASSPAINTOPTIONS   pL_SetMagGlassPaintOptions;
extern LWRP_EXPORT pL_SHOWMAGGLASS  pL_ShowMagGlass;
extern LWRP_EXPORT pL_SETMAGGLASSPOS   pL_SetMagGlassPos;
extern LWRP_EXPORT pL_UPDATEMAGGLASSSHAPE pL_UpdateMagGlassShape;
extern LWRP_EXPORT pL_PRINTBITMAP   pL_PrintBitmap;
extern LWRP_EXPORT pL_PRINTBITMAPFAST  pL_PrintBitmapFast;
extern LWRP_EXPORT pL_SCREENCAPTUREBITMAP pL_ScreenCaptureBitmap;
extern LWRP_EXPORT pL_BITMAPHASRGN  pL_BitmapHasRgn;
extern LWRP_EXPORT pL_CREATEMASKFROMBITMAPRGN   pL_CreateMaskFromBitmapRgn;
extern LWRP_EXPORT pL_CURVETOBEZIER pL_CurveToBezier;
extern LWRP_EXPORT pL_FRAMEBITMAPRGN   pL_FrameBitmapRgn;
extern LWRP_EXPORT pL_COLORBITMAPRGN   pL_ColorBitmapRgn;
extern LWRP_EXPORT pL_FREEBITMAPRGN pL_FreeBitmapRgn;
extern LWRP_EXPORT pL_GETBITMAPRGNAREA pL_GetBitmapRgnArea;
extern LWRP_EXPORT pL_GETBITMAPRGNBOUNDS  pL_GetBitmapRgnBounds;
extern LWRP_EXPORT pL_GETBITMAPRGNHANDLE  pL_GetBitmapRgnHandle;
extern LWRP_EXPORT pL_ISPTINBITMAPRGN  pL_IsPtInBitmapRgn;
extern LWRP_EXPORT pL_OFFSETBITMAPRGN  pL_OffsetBitmapRgn;
extern LWRP_EXPORT pL_PAINTRGNDC pL_PaintRgnDC;
extern LWRP_EXPORT pL_PAINTRGNDCBUFFER pL_PaintRgnDCBuffer;
extern LWRP_EXPORT pL_SETBITMAPRGNCOLOR   pL_SetBitmapRgnColor;
extern LWRP_EXPORT pL_SETBITMAPRGNCOLORHSVRANGE pL_SetBitmapRgnColorHSVRange;
extern LWRP_EXPORT pL_SETBITMAPRGNCOLORRGBRANGE pL_SetBitmapRgnColorRGBRange;
extern LWRP_EXPORT pL_SETBITMAPRGNMAGICWAND  pL_SetBitmapRgnMagicWand;
extern LWRP_EXPORT pL_SETBITMAPRGNELLIPSE pL_SetBitmapRgnEllipse;
extern LWRP_EXPORT pL_SETBITMAPRGNFROMMASK   pL_SetBitmapRgnFromMask;
extern LWRP_EXPORT pL_SETBITMAPRGNHANDLE  pL_SetBitmapRgnHandle;
extern LWRP_EXPORT pL_SETBITMAPRGNPOLYGON pL_SetBitmapRgnPolygon;
extern LWRP_EXPORT pL_SETBITMAPRGNRECT pL_SetBitmapRgnRect;
extern LWRP_EXPORT pL_SETBITMAPRGNROUNDRECT  pL_SetBitmapRgnRoundRect;
extern LWRP_EXPORT pL_SETBITMAPRGNCURVE   pL_SetBitmapRgnCurve;
extern LWRP_EXPORT pL_CREATEPANWINDOW  pL_CreatePanWindow;
extern LWRP_EXPORT pL_UPDATEPANWINDOW  pL_UpdatePanWindow;
extern LWRP_EXPORT pL_DESTROYPANWINDOW pL_DestroyPanWindow;
extern LWRP_EXPORT pL_GETBITMAPRGNDATA pL_GetBitmapRgnData;
extern LWRP_EXPORT pL_SETBITMAPRGNDATA pL_SetBitmapRgnData;
extern LWRP_EXPORT pL_SETBITMAPRGNCLIP pL_SetBitmapRgnClip;
extern LWRP_EXPORT pL_GETBITMAPRGNCLIP pL_GetBitmapRgnClip;
extern LWRP_EXPORT pL_GETBITMAPRGNBOUNDSCLIP pL_GetBitmapRgnBoundsClip;
extern LWRP_EXPORT pL_CONVERTFROMWMF   pL_ConvertFromWMF;
extern LWRP_EXPORT pL_CHANGEFROMWMF pL_ChangeFromWMF;
extern LWRP_EXPORT pL_CONVERTTOWMF  pL_ConvertToWMF;
extern LWRP_EXPORT pL_CHANGETOWMF   pL_ChangeToWMF;
extern LWRP_EXPORT pL_CONVERTFROMEMF   pL_ConvertFromEMF;
extern LWRP_EXPORT pL_CHANGEFROMEMF pL_ChangeFromEMF;
extern LWRP_EXPORT pL_CONVERTTOEMF  pL_ConvertToEMF;
extern LWRP_EXPORT pL_CHANGETOEMF   pL_ChangeToEMF;
extern LWRP_EXPORT pL_PAINTDCOVERLAY   pL_PaintDCOverlay;
extern LWRP_EXPORT pL_DOUBLEBUFFERENABLE  pL_DoubleBufferEnable;
extern LWRP_EXPORT pL_DOUBLEBUFFERCREATEHANDLE  pL_DoubleBufferCreateHandle;
extern LWRP_EXPORT pL_DOUBLEBUFFERDESTROYHANDLE pL_DoubleBufferDestroyHandle;
extern LWRP_EXPORT pL_DOUBLEBUFFERBEGIN   pL_DoubleBufferBegin;
extern LWRP_EXPORT pL_DOUBLEBUFFEREND  pL_DoubleBufferEnd;
extern LWRP_EXPORT pL_SETBITMAPRGNBORDER  pL_SetBitmapRgnBorder;
extern LWRP_EXPORT pL_PAINTDCCMYKARRAY pL_PaintDCCMYKArray;
extern LWRP_EXPORT pL_PRINTBITMAPGDIPLUS  pL_PrintBitmapGDIPlus;
//-----------------------------------------------------------------------------
//--LTFIL.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_COMPRESSBUFFER   pL_CompressBuffer;
extern LWRP_EXPORT pL_DELETEPAGE pL_DeletePage;
extern LWRP_EXPORT pL_ENDCOMPRESSBUFFER   pL_EndCompressBuffer;
extern LWRP_EXPORT pL_READLOADRESOLUTIONS pL_ReadLoadResolutions;
extern LWRP_EXPORT pL_FEEDLOAD   pL_FeedLoad;
extern LWRP_EXPORT pL_FILECONVERT   pL_FileConvert;
extern LWRP_EXPORT pL_FILEINFO   pL_FileInfo;
extern LWRP_EXPORT pL_FILEINFOMEMORY   pL_FileInfoMemory;
extern LWRP_EXPORT pL_GETCOMMENT pL_GetComment;
extern LWRP_EXPORT pL_GETLOADRESOLUTION   pL_GetLoadResolution;
extern LWRP_EXPORT pL_GETFILECOMMENTSIZE  pL_GetFileCommentSize;
extern LWRP_EXPORT pL_GETTAG  pL_GetTag;
extern LWRP_EXPORT pL_LOADBITMAP pL_LoadBitmap;
extern LWRP_EXPORT pL_LOADBITMAPLIST   pL_LoadBitmapList;
extern LWRP_EXPORT pL_LOADBITMAPMEMORY pL_LoadBitmapMemory;
extern LWRP_EXPORT pL_LOADFILE   pL_LoadFile;
extern LWRP_EXPORT pL_LOADFILETILE  pL_LoadFileTile;
extern LWRP_EXPORT pL_LOADMEMORYTILE   pL_LoadMemoryTile;
extern LWRP_EXPORT pL_LOADFILEOFFSET   pL_LoadFileOffset;
extern LWRP_EXPORT pL_LOADMEMORY pL_LoadMemory;
extern LWRP_EXPORT pL_READFILECOMMENT  pL_ReadFileComment;
extern LWRP_EXPORT pL_READFILECOMMENTEXT  pL_ReadFileCommentExt;
extern LWRP_EXPORT pL_READFILECOMMENTMEMORY  pL_ReadFileCommentMemory;
extern LWRP_EXPORT pL_READFILETAG   pL_ReadFileTag;
extern LWRP_EXPORT pL_READFILETAGMEMORY   pL_ReadFileTagMemory;
extern LWRP_EXPORT pL_READFILESTAMP pL_ReadFileStamp;
extern LWRP_EXPORT pL_SAVEBITMAP pL_SaveBitmap;
extern LWRP_EXPORT pL_SAVEBITMAPBUFFER pL_SaveBitmapBuffer;
extern LWRP_EXPORT pL_SAVEBITMAPLIST   pL_SaveBitmapList;
extern LWRP_EXPORT pL_SAVEBITMAPMEMORY pL_SaveBitmapMemory;
extern LWRP_EXPORT pL_SAVEFILE   pL_SaveFile;
extern LWRP_EXPORT pL_SAVEFILEBUFFER   pL_SaveFileBuffer;
extern LWRP_EXPORT pL_SAVEFILEMEMORY   pL_SaveFileMemory;
extern LWRP_EXPORT pL_SAVEFILETILE  pL_SaveFileTile;
extern LWRP_EXPORT pL_SAVEFILEOFFSET   pL_SaveFileOffset;
extern LWRP_EXPORT pL_SETCOMMENT pL_SetComment;
extern LWRP_EXPORT pL_SETLOADINFOCALLBACK pL_SetLoadInfoCallback;
extern LWRP_EXPORT pL_GETLOADINFOCALLBACKDATA   pL_GetLoadInfoCallbackData;
extern LWRP_EXPORT pL_SETLOADRESOLUTION   pL_SetLoadResolution;
extern LWRP_EXPORT pL_SETTAG  pL_SetTag;
extern LWRP_EXPORT pL_STARTCOMPRESSBUFFER pL_StartCompressBuffer;
extern LWRP_EXPORT pL_STARTFEEDLOAD pL_StartFeedLoad;
extern LWRP_EXPORT pL_STOPFEEDLOAD  pL_StopFeedLoad;
extern LWRP_EXPORT pL_WRITEFILECOMMENTEXT pL_WriteFileCommentExt;
extern LWRP_EXPORT pL_WRITEFILESTAMP   pL_WriteFileStamp;
extern LWRP_EXPORT pL_SETSAVERESOLUTION   pL_SetSaveResolution;
extern LWRP_EXPORT pL_GETSAVERESOLUTION   pL_GetSaveResolution;
extern LWRP_EXPORT pL_GETDEFAULTLOADFILEOPTION  pL_GetDefaultLoadFileOption;
extern LWRP_EXPORT pL_GETDEFAULTSAVEFILEOPTION  pL_GetDefaultSaveFileOption;
extern LWRP_EXPORT pL_WRITEFILETAG  pL_WriteFileTag;
extern LWRP_EXPORT pL_WRITEFILECOMMENT pL_WriteFileComment;
extern LWRP_EXPORT pL_CREATETHUMBNAILFROMFILE   pL_CreateThumbnailFromFile;
extern LWRP_EXPORT pL_GETJ2KOPTIONS pL_GetJ2KOptions;
extern LWRP_EXPORT pL_GETDEFAULTJ2KOPTIONS   pL_GetDefaultJ2KOptions;
extern LWRP_EXPORT pL_SETJ2KOPTIONS pL_SetJ2KOptions;
extern LWRP_EXPORT pL_MARKERCALLBACKPROXY pL_MarkerCallbackProxy;
extern LWRP_EXPORT pL_TRANSFORMFILE pL_TransformFile;
extern LWRP_EXPORT pL_READFILEEXTENSIONS  pL_ReadFileExtensions;
extern LWRP_EXPORT pL_FREEEXTENSIONS   pL_FreeExtensions;
extern LWRP_EXPORT pL_LOADEXTENSIONSTAMP  pL_LoadExtensionStamp;
extern LWRP_EXPORT pL_GETEXTENSIONAUDIO   pL_GetExtensionAudio;
extern LWRP_EXPORT pL_STARTDECOMPRESSBUFFER  pL_StartDecompressBuffer;
extern LWRP_EXPORT pL_STOPDECOMPRESSBUFFER   pL_StopDecompressBuffer;
extern LWRP_EXPORT pL_DECOMPRESSBUFFER pL_DecompressBuffer;
extern LWRP_EXPORT pL_IGNOREFILTERS pL_IgnoreFilters;
extern LWRP_EXPORT pL_PRELOADFILTERS   pL_PreLoadFilters;
extern LWRP_EXPORT pL_GETIGNOREFILTERS pL_GetIgnoreFilters;
extern LWRP_EXPORT pL_GETPRELOADFILTERS   pL_GetPreLoadFilters;
extern LWRP_EXPORT pL_GETJBIG2OPTIONS  pL_GetJBIG2Options;
extern LWRP_EXPORT pL_SETJBIG2OPTIONS  pL_SetJBIG2Options;
extern LWRP_EXPORT pL_CREATEMARKERS pL_CreateMarkers;
extern LWRP_EXPORT pL_LOADMARKERS   pL_LoadMarkers;
extern LWRP_EXPORT pL_FREEMARKERS   pL_FreeMarkers;
extern LWRP_EXPORT pL_SETMARKERS pL_SetMarkers;
extern LWRP_EXPORT pL_GETMARKERS pL_GetMarkers;
extern LWRP_EXPORT pL_ENUMMARKERS   pL_EnumMarkers;
extern LWRP_EXPORT pL_DELETEMARKER  pL_DeleteMarker;
extern LWRP_EXPORT pL_INSERTMARKER  pL_InsertMarker;
extern LWRP_EXPORT pL_COPYMARKERS   pL_CopyMarkers;
extern LWRP_EXPORT pL_GETMARKERCOUNT   pL_GetMarkerCount;
extern LWRP_EXPORT pL_GETMARKER  pL_GetMarker;
extern LWRP_EXPORT pL_DELETEMARKERINDEX   pL_DeleteMarkerIndex;
extern LWRP_EXPORT pL_WRITEFILEMETADATA   pL_WriteFileMetaData;
extern LWRP_EXPORT pL_STARTSAVEDATA pL_StartSaveData;
extern LWRP_EXPORT pL_SAVEDATA   pL_SaveData;
extern LWRP_EXPORT pL_STOPSAVEDATA  pL_StopSaveData;
extern LWRP_EXPORT pL_SETOVERLAYCALLBACK  pL_SetOverlayCallback;
extern LWRP_EXPORT pL_GETOVERLAYCALLBACK  pL_GetOverlayCallback;
extern LWRP_EXPORT pL_GETFILTERLISTINFO   pL_GetFilterListInfo;
extern LWRP_EXPORT pL_GETFILTERINFO pL_GetFilterInfo;
extern LWRP_EXPORT pL_FREEFILTERINFO   pL_FreeFilterInfo;
extern LWRP_EXPORT pL_SETFILTERINFO pL_SetFilterInfo;
extern LWRP_EXPORT pL_GETTXTOPTIONS pL_GetTXTOptions;
extern LWRP_EXPORT pL_SETTXTOPTIONS pL_SetTXTOptions;
extern LWRP_EXPORT pL_COMPACTFILE   pL_CompactFile;
extern LWRP_EXPORT pL_LOADFILECMYKARRAY   pL_LoadFileCMYKArray;
extern LWRP_EXPORT pL_SAVEFILECMYKARRAY   pL_SaveFileCMYKArray;
extern LWRP_EXPORT pL_ENUMFILETAGS  pL_EnumFileTags;
extern LWRP_EXPORT pL_DELETETAG  pL_DeleteTag;
extern LWRP_EXPORT pL_SETGEOKEY  pL_SetGeoKey;
extern LWRP_EXPORT pL_GETGEOKEY  pL_GetGeoKey;
extern LWRP_EXPORT pL_WRITEFILEGEOKEY  pL_WriteFileGeoKey;
extern LWRP_EXPORT pL_READFILEGEOKEY   pL_ReadFileGeoKey;
extern LWRP_EXPORT pL_ENUMFILEGEOKEYS  pL_EnumFileGeoKeys;
extern LWRP_EXPORT pL_READFILECOMMENTOFFSET  pL_ReadFileCommentOffset;
extern LWRP_EXPORT pL_GETLOADSTATUS pL_GetLoadStatus;
extern LWRP_EXPORT pL_DECODEABIC pL_DecodeABIC;
extern LWRP_EXPORT pL_ENCODEABIC pL_EncodeABIC;
extern LWRP_EXPORT pL_GETPNGTRNS pL_GetPNGTRNS;
// These functions not ported to Windows CE
extern LWRP_EXPORT pL_LOADBITMAPRESIZE pL_LoadBitmapResize;
extern LWRP_EXPORT pL_READFILETRANSFORMS  pL_ReadFileTransforms;
extern LWRP_EXPORT pL_WRITEFILETRANSFORMS pL_WriteFileTransforms;
extern LWRP_EXPORT pL_GETPCDRESOLUTION pL_GetPCDResolution;
extern LWRP_EXPORT pL_GETWMFRESOLUTION pL_GetWMFResolution;
extern LWRP_EXPORT pL_SETPCDRESOLUTION pL_SetPCDResolution;
extern LWRP_EXPORT pL_SETWMFRESOLUTION pL_SetWMFResolution;
extern LWRP_EXPORT pL_SAVECUSTOMFILE   pL_SaveCustomFile;
extern LWRP_EXPORT pL_LOADCUSTOMFILE   pL_LoadCustomFile;
extern LWRP_EXPORT pL_2DSETVIEWPORT pL_2DSetViewport;
extern LWRP_EXPORT pL_2DGETVIEWPORT pL_2DGetViewport;
extern LWRP_EXPORT pL_2DSETVIEWMODE pL_2DSetViewMode;
extern LWRP_EXPORT pL_2DGETVIEWMODE pL_2DGetViewMode;
extern LWRP_EXPORT pL_VECLOADFILE   pL_VecLoadFile;
extern LWRP_EXPORT pL_VECLOADMEMORY pL_VecLoadMemory;
extern LWRP_EXPORT pL_VECSTARTFEEDLOAD pL_VecStartFeedLoad;
extern LWRP_EXPORT pL_VECFEEDLOAD   pL_VecFeedLoad;
extern LWRP_EXPORT pL_VECSTOPFEEDLOAD  pL_VecStopFeedLoad;
extern LWRP_EXPORT pL_VECSAVEFILE   pL_VecSaveFile;
extern LWRP_EXPORT pL_VECSAVEMEMORY pL_VecSaveMemory;
extern LWRP_EXPORT pL_GETPLTOPTIONS pL_GetPLTOptions;
extern LWRP_EXPORT pL_SETPLTOPTIONS pL_SetPLTOptions;
extern LWRP_EXPORT pL_GETPDFINITDIR pL_GetPDFInitDir;
extern LWRP_EXPORT pL_SETPDFINITDIR pL_SetPDFInitDir;
extern LWRP_EXPORT pL_GETRTFOPTIONS pL_GetRTFOptions;
extern LWRP_EXPORT pL_SETRTFOPTIONS pL_SetRTFOptions;
extern LWRP_EXPORT pL_GETPTKOPTIONS pL_GetPTKOptions;
extern LWRP_EXPORT pL_SETPTKOPTIONS pL_SetPTKOptions;
extern LWRP_EXPORT pL_GETPDFOPTIONS pL_GetPDFOptions;
extern LWRP_EXPORT pL_SETPDFOPTIONS pL_SetPDFOptions;
extern LWRP_EXPORT pL_GETPDFSAVEOPTIONS   pL_GetPDFSaveOptions;
extern LWRP_EXPORT pL_SETPDFSAVEOPTIONS   pL_SetPDFSaveOptions;
extern LWRP_EXPORT pL_GETDJVOPTIONS pL_GetDJVOptions;
extern LWRP_EXPORT pL_SETDJVOPTIONS pL_SetDJVOptions;
extern LWRP_EXPORT pL_LOADLAYER  pL_LoadLayer;
extern LWRP_EXPORT pL_SAVEBITMAPWITHLAYERS   pL_SaveBitmapWithLayers;
extern LWRP_EXPORT pL_GETAUTOCADFILESCOLORSCHEME   pL_GetAutoCADFilesColorScheme;
extern LWRP_EXPORT pL_SETAUTOCADFILESCOLORSCHEME   pL_SetAutoCADFilesColorScheme;
extern LWRP_EXPORT pL_VECADDFONTMAPPER pL_VecAddFontMapper;
extern LWRP_EXPORT pL_VECREMOVEFONTMAPPER pL_VecRemoveFontMapper;
extern LWRP_EXPORT pL_GETXPSOPTIONS pL_GetXPSOptions;
extern LWRP_EXPORT pL_SETXPSOPTIONS pL_SetXPSOptions;

//-----------------------------------------------------------------------------
//--LTEFX.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_EFXPAINTTRANSITION  pL_EfxPaintTransition;
extern LWRP_EXPORT pL_EFXPAINTBITMAP   pL_EfxPaintBitmap;
extern LWRP_EXPORT pL_EFXDRAWFRAME  pL_EfxDrawFrame;
extern LWRP_EXPORT pL_EFXTILERECT   pL_EfxTileRect;
extern LWRP_EXPORT pL_EFXGRADIENTFILLRECT pL_EfxGradientFillRect;
extern LWRP_EXPORT pL_EFXPATTERNFILLRECT  pL_EfxPatternFillRect;
extern LWRP_EXPORT pL_EFXDRAW3DTEXT pL_EfxDraw3dText;
extern LWRP_EXPORT pL_EFXEFFECTBLT  pL_EfxEffectBlt;
extern LWRP_EXPORT pL_PAINTDCEFFECT pL_PaintDCEffect;
extern LWRP_EXPORT pL_PAINTRGNDCEFFECT pL_PaintRgnDCEffect;
extern LWRP_EXPORT pL_EFXDRAWROTATED3DTEXT   pL_EfxDrawRotated3dText;
extern LWRP_EXPORT pL_EFXDRAW3DSHAPE   pL_EfxDraw3dShape;

//-----------------------------------------------------------------------------
//--LTDLG.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------

extern LWRP_EXPORT pL_DLGINIT                   pL_DlgInit;
extern LWRP_EXPORT pL_DLGFREE                   pL_DlgFree;
extern LWRP_EXPORT pL_DLGSETFONT                pL_DlgSetFont;
extern LWRP_EXPORT pL_DLGGETSTRINGLEN           pL_DlgGetStringLen;
extern LWRP_EXPORT pL_DLGSETSTRING              pL_DlgSetString;
extern LWRP_EXPORT pL_DLGGETSTRING              pL_DlgGetString;

//{{ Color dialogs C DLL's group - LTDlgClr14?.dll
extern LWRP_EXPORT pL_DLGBALANCECOLORS          pL_DlgBalanceColors;
extern LWRP_EXPORT pL_DLGCOLOREDGRAY            pL_DlgColoredGray;
extern LWRP_EXPORT pL_DLGGRAYSCALE              pL_DlgGrayScale;
extern LWRP_EXPORT pL_DLGREMAPINTENSITY         pL_DlgRemapIntensity;
extern LWRP_EXPORT pL_DLGREMAPHUE               pL_DlgRemapHue;
extern LWRP_EXPORT pL_DLGCUSTOMIZEPALETTE       pL_DlgCustomizePalette;
extern LWRP_EXPORT pL_DLGLOCALHISTOEQUALIZE     pL_DlgLocalHistoEqualize;
extern LWRP_EXPORT pL_DLGINTENSITYDETECT        pL_DlgIntensityDetect;
extern LWRP_EXPORT pL_DLGSOLARIZE               pL_DlgSolarize;
extern LWRP_EXPORT pL_DLGPOSTERIZE              pL_DlgPosterize;
extern LWRP_EXPORT pL_DLGBRIGHTNESS             pL_DlgBrightness;
extern LWRP_EXPORT pL_DLGCONTRAST               pL_DlgContrast;
extern LWRP_EXPORT pL_DLGHUE                    pL_DlgHue;
extern LWRP_EXPORT pL_DLGSATURATION             pL_DlgSaturation;
extern LWRP_EXPORT pL_DLGGAMMAADJUSTMENT        pL_DlgGammaAdjustment;
extern LWRP_EXPORT pL_DLGHALFTONE               pL_DlgHalftone;
extern LWRP_EXPORT pL_DLGCOLORRES               pL_DlgColorRes;
extern LWRP_EXPORT pL_DLGHISTOCONTRAST          pL_DlgHistoContrast;
extern LWRP_EXPORT pL_DLGWINDOWLEVEL            pL_DlgWindowLevel;
extern LWRP_EXPORT pL_DLGCOLOR                  pL_DlgColor;

//{{ Image Effects dialogs C DLL's group - LTDlgImgEfx14?.dll
extern LWRP_EXPORT pL_DLGMOTIONBLUR             pL_DlgMotionBlur;
extern LWRP_EXPORT pL_DLGRADIALBLUR             pL_DlgRadialBlur;
extern LWRP_EXPORT pL_DLGZOOMBLUR               pL_DlgZoomBlur;
extern LWRP_EXPORT pL_DLGGAUSSIANBLUR           pL_DlgGaussianBlur;
extern LWRP_EXPORT pL_DLGANTIALIAS              pL_DlgAntiAlias;
extern LWRP_EXPORT pL_DLGAVERAGE                pL_DlgAverage;
extern LWRP_EXPORT pL_DLGMEDIAN                 pL_DlgMedian;
extern LWRP_EXPORT pL_DLGADDNOISE               pL_DlgAddNoise;
extern LWRP_EXPORT pL_DLGMAXFILTER              pL_DlgMaxFilter;
extern LWRP_EXPORT pL_DLGMINFILTER              pL_DlgMinFilter;
extern LWRP_EXPORT pL_DLGSHARPEN                pL_DlgSharpen;
extern LWRP_EXPORT pL_DLGSHIFTDIFFERENCEFILTER  pL_DlgShiftDifferenceFilter;
extern LWRP_EXPORT pL_DLGEMBOSS                 pL_DlgEmboss;
extern LWRP_EXPORT pL_DLGOILIFY                 pL_DlgOilify;
extern LWRP_EXPORT pL_DLGMOSAIC                 pL_DlgMosaic;
extern LWRP_EXPORT pL_DLGEROSIONFILTER          pL_DlgErosionFilter;
extern LWRP_EXPORT pL_DLGDILATIONFILTER         pL_DlgDilationFilter;
extern LWRP_EXPORT pL_DLGCONTOURFILTER          pL_DlgContourFilter;
extern LWRP_EXPORT pL_DLGGRADIENTFILTER         pL_DlgGradientFilter;
extern LWRP_EXPORT pL_DLGLAPLACIANFILTER        pL_DlgLaplacianFilter;
extern LWRP_EXPORT pL_DLGSOBELFILTER            pL_DlgSobelFilter;
extern LWRP_EXPORT pL_DLGPREWITTFILTER          pL_DlgPrewittFilter;
extern LWRP_EXPORT pL_DLGLINESEGMENTFILTER      pL_DlgLineSegmentFilter;
extern LWRP_EXPORT pL_DLGUNSHARPMASK            pL_DlgUnsharpMask;
extern LWRP_EXPORT pL_DLGMULTIPLY               pL_DlgMultiply;
extern LWRP_EXPORT pL_DLGADDBITMAPS             pL_DlgAddBitmaps;
extern LWRP_EXPORT pL_DLGSTITCH                 pL_DlgStitch;
extern LWRP_EXPORT pL_DLGFREEHANDWAVE           pL_DlgFreeHandWave;
extern LWRP_EXPORT pL_DLGWIND                   pL_DlgWind;
extern LWRP_EXPORT pL_DLGPOLAR                  pL_DlgPolar;
extern LWRP_EXPORT pL_DLGZOOMWAVE               pL_DlgZoomWave;
extern LWRP_EXPORT pL_DLGRADIALWAVE             pL_DlgRadialWave;
extern LWRP_EXPORT pL_DLGSWIRL                  pL_DlgSwirl;
extern LWRP_EXPORT pL_DLGWAVE                   pL_DlgWave;
extern LWRP_EXPORT pL_DLGWAVESHEAR              pL_DlgWaveShear;
extern LWRP_EXPORT pL_DLGPUNCH                  pL_DlgPunch;
extern LWRP_EXPORT pL_DLGRIPPLE                 pL_DlgRipple;
extern LWRP_EXPORT pL_DLGBENDING                pL_DlgBending;
extern LWRP_EXPORT pL_DLGCYLINDRICAL            pL_DlgCylindrical;
extern LWRP_EXPORT pL_DLGSPHERIZE               pL_DlgSpherize;
extern LWRP_EXPORT pL_DLGIMPRESSIONIST          pL_DlgImpressionist;
extern LWRP_EXPORT pL_DLGPIXELATE               pL_DlgPixelate;
extern LWRP_EXPORT pL_DLGEDGEDETECTOR           pL_DlgEdgeDetector;
extern LWRP_EXPORT pL_DLGUNDERLAY               pL_DlgUnderlay;
extern LWRP_EXPORT pL_DLGPICTURIZE              pL_DlgPicturize;

//{{ Image dialogs  C DLL's group - LTDlgImg14?.dll
extern LWRP_EXPORT pL_DLGROTATE                 pL_DlgRotate;
extern LWRP_EXPORT pL_DLGSHEAR                  pL_DlgShear;
extern LWRP_EXPORT pL_DLGRESIZE                 pL_DlgResize;
extern LWRP_EXPORT pL_DLGADDBORDER              pL_DlgAddBorder;
extern LWRP_EXPORT pL_DLGADDFRAME               pL_DlgAddFrame;
extern LWRP_EXPORT pL_DLGAUTOTRIM               pL_DlgAutoTrim;
extern LWRP_EXPORT pL_DLGCANVASRESIZE           pL_DlgCanvasResize;
extern LWRP_EXPORT pL_DLGHISTOGRAM              pL_DlgHistogram;

//{{ Web dialogs C DLL's group - LTDlgWeb14?.dll
extern LWRP_EXPORT pL_DLGPNGWEBTUNER            pL_DlgPNGWebTuner;
extern LWRP_EXPORT pL_DLGGIFWEBTUNER            pL_DlgGIFWebTuner;
extern LWRP_EXPORT pL_DLGJPEGWEBTUNER           pL_DlgJPEGWebTuner;
extern LWRP_EXPORT pL_DLGHTMLMAPPER             pL_DlgHTMLMapper;

//{{ File dialogs C DLL's group - LTDlgFile14?.dll
extern LWRP_EXPORT pL_DLGGETDIRECTORY           pL_DlgGetDirectory;
extern LWRP_EXPORT pL_DLGFILECONVERSION         pL_DlgFileConversion;
extern LWRP_EXPORT pL_DLGFILESASSOCIATION       pL_DlgFilesAssociation;
extern LWRP_EXPORT pL_DLGPRINTSTITCHEDIMAGES    pL_DlgPrintStitchedImages;
extern LWRP_EXPORT pL_DLGSAVE                   pL_DlgSave;
extern LWRP_EXPORT pL_DLGOPEN                   pL_DlgOpen;
extern LWRP_EXPORT pL_DLGPRINTPREVIEW           pL_DlgPrintPreview;
extern LWRP_EXPORT pL_DLGICCPROFILE             pL_DlgICCProfile;

//{{ Effects dialogs C DLL's group - LTDlgEfx14?.dll
extern LWRP_EXPORT pL_DLGGETSHAPE               pL_DlgGetShape;
extern LWRP_EXPORT pL_DLGGETEFFECT              pL_DlgGetEffect;
extern LWRP_EXPORT pL_DLGGETTRANSITION          pL_DlgGetTransition;
extern LWRP_EXPORT pL_DLGGETGRADIENT            pL_DlgGetGradient;
extern LWRP_EXPORT pL_DLGGETTEXT                pL_DlgGetText;

//{{ Document Image dialogs C DLL's group - LTDlgImgDoc14?.dll
extern LWRP_EXPORT pL_DLGSMOOTH                 pL_DlgSmooth;
extern LWRP_EXPORT pL_DLGLINEREMOVE             pL_DlgLineRemove;
extern LWRP_EXPORT pL_DLGBORDERREMOVE           pL_DlgBorderRemove;
extern LWRP_EXPORT pL_DLGINVERTEDTEXT           pL_DlgInvertedText;
extern LWRP_EXPORT pL_DLGDOTREMOVE              pL_DlgDotRemove;
extern LWRP_EXPORT pL_DLGHOLEPUNCHREMOVE        pL_DlgHolePunchRemove;

//-----------------------------------------------------------------------------
//--LTTWN.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_TWAININITSESSION pL_TwainInitSession;
extern LWRP_EXPORT pL_TWAININITSESSION2   pL_TwainInitSession2;
extern LWRP_EXPORT pL_TWAINENDSESSION  pL_TwainEndSession;
extern LWRP_EXPORT pL_TWAINSETPROPERTIES  pL_TwainSetProperties;
extern LWRP_EXPORT pL_TWAINGETPROPERTIES  pL_TwainGetProperties;
extern LWRP_EXPORT pL_TWAINACQUIRELIST pL_TwainAcquireList;
extern LWRP_EXPORT pL_TWAINACQUIRE  pL_TwainAcquire;
extern LWRP_EXPORT pL_TWAINSELECTSOURCE   pL_TwainSelectSource;
extern LWRP_EXPORT pL_TWAINQUERYPROPERTY  pL_TwainQueryProperty;
extern LWRP_EXPORT pL_TWAINSTARTCAPSNEG   pL_TwainStartCapsNeg;
extern LWRP_EXPORT pL_TWAINENDCAPSNEG  pL_TwainEndCapsNeg;
extern LWRP_EXPORT pL_TWAINSETCAPABILITY  pL_TwainSetCapability;
extern LWRP_EXPORT pL_TWAINGETCAPABILITY  pL_TwainGetCapability;
extern LWRP_EXPORT pL_TWAINENUMCAPABILITIES  pL_TwainEnumCapabilities;
extern LWRP_EXPORT pL_TWAINCREATENUMERICCONTAINERONEVALUE   pL_TwainCreateNumericContainerOneValue;
extern LWRP_EXPORT pL_TWAINCREATENUMERICCONTAINERRANGE   pL_TwainCreateNumericContainerRange;
extern LWRP_EXPORT pL_TWAINCREATENUMERICCONTAINERARRAY   pL_TwainCreateNumericContainerArray;
extern LWRP_EXPORT pL_TWAINCREATENUMERICCONTAINERENUM pL_TwainCreateNumericContainerEnum;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERVALUE   pL_TwainGetNumericContainerValue;
extern LWRP_EXPORT pL_TWAINFREECONTAINER  pL_TwainFreeContainer;
extern LWRP_EXPORT pL_TWAINFREEPROPQUERYSTRUCTURE  pL_TwainFreePropQueryStructure;
extern LWRP_EXPORT pL_TWAINTEMPLATEDLG pL_TwainTemplateDlg;
extern LWRP_EXPORT pL_TWAINOPENTEMPLATEFILE  pL_TwainOpenTemplateFile;
extern LWRP_EXPORT pL_TWAINADDCAPABILITYTOFILE  pL_TwainAddCapabilityToFile;
extern LWRP_EXPORT pL_TWAINGETCAPABILITYFROMFILE   pL_TwainGetCapabilityFromFile;
extern LWRP_EXPORT pL_TWAINGETNUMOFCAPSINFILE   pL_TwainGetNumOfCapsInFile;
extern LWRP_EXPORT pL_TWAINCLOSETEMPLATEFILE pL_TwainCloseTemplateFile;
extern LWRP_EXPORT pL_TWAINGETEXTENDEDIMAGEINFO pL_TwainGetExtendedImageInfo;
extern LWRP_EXPORT pL_TWAINFREEEXTENDEDIMAGEINFOSTRUCTURE   pL_TwainFreeExtendedImageInfoStructure;
extern LWRP_EXPORT pL_TWAINLOCKCONTAINER  pL_TwainLockContainer;
extern LWRP_EXPORT pL_TWAINUNLOCKCONTAINER   pL_TwainUnlockContainer;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERITEMTYPE   pL_TwainGetNumericContainerItemType;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERINTVALUE   pL_TwainGetNumericContainerINTValue;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERUINTVALUE  pL_TwainGetNumericContainerUINTValue;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERBOOLVALUE  pL_TwainGetNumericContainerBOOLValue;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERFIX32VALUE pL_TwainGetNumericContainerFIX32Value;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERFRAMEVALUE pL_TwainGetNumericContainerFRAMEValue;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERSTRINGVALUE   pL_TwainGetNumericContainerSTRINGValue;
extern LWRP_EXPORT pL_TWAINGETNUMERICCONTAINERUNICODEVALUE  pL_TwainGetNumericContainerUNICODEValue;
extern LWRP_EXPORT pL_TWAINACQUIREMULTI   pL_TwainAcquireMulti;
extern LWRP_EXPORT pL_ISTWAINAVAILABLE pL_IsTwainAvailable;
extern LWRP_EXPORT pL_TWAINFINDFASTCONFIG pL_TwainFindFastConfig;
extern LWRP_EXPORT pL_TWAINGETSCANCONFIGS pL_TwainGetScanConfigs;
extern LWRP_EXPORT pL_TWAINFREESCANCONFIG pL_TwainFreeScanConfig;
extern LWRP_EXPORT pL_TWAINGETSOURCES  pL_TwainGetSources;
extern LWRP_EXPORT pL_TWAINENABLESHOWUSERINTERFACEONLY   pL_TwainEnableShowUserInterfaceOnly;
extern LWRP_EXPORT pL_TWAINCANCELACQUIRE  pL_TwainCancelAcquire;
extern LWRP_EXPORT pL_TWAINQUERYFILESYSTEM   pL_TwainQueryFileSystem;
extern LWRP_EXPORT pL_TWAINGETJPEGCOMPRESSION   pL_TwainGetJPEGCompression;
extern LWRP_EXPORT pL_TWAINSETJPEGCOMPRESSION   pL_TwainSetJPEGCompression;
extern LWRP_EXPORT pL_TWAINSETTRANSFEROPTIONS   pL_TwainSetTransferOptions;
extern LWRP_EXPORT pL_TWAINGETTRANSFEROPTIONS   pL_TwainGetTransferOptions;
extern LWRP_EXPORT pL_TWAINGETSUPPORTEDTRANSFERMODE   pL_TwainGetSupportedTransferMode;
extern LWRP_EXPORT pL_TWAINSETRESOLUTION  pL_TwainSetResolution;
extern LWRP_EXPORT pL_TWAINGETRESOLUTION  pL_TwainGetResolution;
extern LWRP_EXPORT pL_TWAINSETIMAGEFRAME  pL_TwainSetImageFrame;
extern LWRP_EXPORT pL_TWAINGETIMAGEFRAME  pL_TwainGetImageFrame;
extern LWRP_EXPORT pL_TWAINSETIMAGEUNIT   pL_TwainSetImageUnit;
extern LWRP_EXPORT pL_TWAINGETIMAGEUNIT   pL_TwainGetImageUnit;
extern LWRP_EXPORT pL_TWAINSETIMAGEBITSPERPIXEL pL_TwainSetImageBitsPerPixel;
extern LWRP_EXPORT pL_TWAINGETIMAGEBITSPERPIXEL pL_TwainGetImageBitsPerPixel;
extern LWRP_EXPORT pL_TWAINSETIMAGEEFFECTS   pL_TwainSetImageEffects;
extern LWRP_EXPORT pL_TWAINGETIMAGEEFFECTS   pL_TwainGetImageEffects;
extern LWRP_EXPORT pL_TWAINSETACQUIREPAGEOPTIONS   pL_TwainSetAcquirePageOptions;
extern LWRP_EXPORT pL_TWAINGETACQUIREPAGEOPTIONS   pL_TwainGetAcquirePageOptions;
extern LWRP_EXPORT pL_TWAINSETRGBRESPONSE pL_TwainSetRGBResponse;
extern LWRP_EXPORT pL_TWAINSHOWPROGRESS   pL_TwainShowProgress;
extern LWRP_EXPORT pL_TWAINENABLEDUPLEX   pL_TwainEnableDuplex;
extern LWRP_EXPORT pL_TWAINGETDUPLEXOPTIONS  pL_TwainGetDuplexOptions;
extern LWRP_EXPORT pL_TWAINSETMAXXFERCOUNT   pL_TwainSetMaxXferCount;
extern LWRP_EXPORT pL_TWAINGETMAXXFERCOUNT   pL_TwainGetMaxXferCount;
extern LWRP_EXPORT pL_TWAINSTOPFEEDER  pL_TwainStopFeeder;
extern LWRP_EXPORT pL_TWAINSETDEVICEEVENTCALLBACK pL_TwainSetDeviceEventCallback;
extern LWRP_EXPORT pL_TWAINGETDEVICEEVENTDATA pL_TwainGetDeviceEventData;
extern LWRP_EXPORT pL_TWAINSETDEVICEEVENTCAPABILITY pL_TwainSetDeviceEventCapability;
extern LWRP_EXPORT pL_TWAINGETDEVICEEVENTCAPABILITY pL_TwainGetDeviceEventCapability;
extern LWRP_EXPORT pL_TWAINRESETDEVICEEVENTCAPABILITY pL_TwainResetDeviceEventCapability;
extern LWRP_EXPORT pL_TWAINFASTACQUIRE   pL_TwainFastAcquire;

#if defined(LEADTOOLS_V16_OR_LATER)
extern LWRP_EXPORT pL_TWAINGETCUSTOMDSDATA      pL_TwainGetCustomDSData;
extern LWRP_EXPORT pL_TWAINSETCUSTOMDSDATA      pL_TwainSetCustomDSData;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

//-----------------------------------------------------------------------------
//--LTANN.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------

extern LWRP_EXPORT pL_ANNBRINGTOFRONT  pL_AnnBringToFront;
extern LWRP_EXPORT pL_ANNCLIPBOARDREADY   pL_AnnClipboardReady;
extern LWRP_EXPORT pL_ANNCOPY pL_AnnCopy;
extern LWRP_EXPORT pL_ANNCOPYFROMCLIPBOARD   pL_AnnCopyFromClipboard;
extern LWRP_EXPORT pL_ANNCOPYTOCLIPBOARD  pL_AnnCopyToClipboard;
extern LWRP_EXPORT pL_ANNCUTTOCLIPBOARD   pL_AnnCutToClipboard;
extern LWRP_EXPORT pL_ANNCREATE  pL_AnnCreate;
extern LWRP_EXPORT pL_ANNCREATECONTAINER  pL_AnnCreateContainer;
extern LWRP_EXPORT pL_ANNCREATEITEM pL_AnnCreateItem;
extern LWRP_EXPORT pL_ANNCREATETOOLBAR pL_AnnCreateToolBar;
extern LWRP_EXPORT pL_ANNDEFINE  pL_AnnDefine;
extern LWRP_EXPORT pL_ANNDELETEPAGEOFFSET pL_AnnDeletePageOffset;
extern LWRP_EXPORT pL_ANNDELETEPAGE pL_AnnDeletePage;
extern LWRP_EXPORT pL_ANNDELETEPAGEMEMORY pL_AnnDeletePageMemory;
extern LWRP_EXPORT pL_ANNDESTROY pL_AnnDestroy;
extern LWRP_EXPORT pL_ANNDRAW pL_AnnDraw;
extern LWRP_EXPORT pL_ANNENUMERATE  pL_AnnEnumerate;
extern LWRP_EXPORT pL_ANNFILEINFO   pL_AnnFileInfo;
extern LWRP_EXPORT pL_ANNFILEINFOOFFSET   pL_AnnFileInfoOffset;
extern LWRP_EXPORT pL_ANNFILEINFOMEMORY   pL_AnnFileInfoMemory;
extern LWRP_EXPORT pL_ANNFLIP pL_AnnFlip;
extern LWRP_EXPORT pL_ANNGETACTIVESTATE   pL_AnnGetActiveState;
extern LWRP_EXPORT pL_ANNGETAUTOCONTAINER pL_AnnGetAutoContainer;
extern LWRP_EXPORT pL_ANNGETAUTODRAWENABLE   pL_AnnGetAutoDrawEnable;
extern LWRP_EXPORT pL_ANNGETAUTOMENUENABLE   pL_AnnGetAutoMenuEnable;
extern LWRP_EXPORT pL_ANNGETAUTOTEXT   pL_AnnGetAutoText;
extern LWRP_EXPORT pL_ANNGETAUTOTEXTLEN   pL_AnnGetAutoTextLen;
extern LWRP_EXPORT pL_ANNGETBACKCOLOR  pL_AnnGetBackColor;
extern LWRP_EXPORT pL_ANNGETBITMAP  pL_AnnGetBitmap;
extern LWRP_EXPORT pL_ANNGETBITMAPDPIX pL_AnnGetBitmapDpiX;
extern LWRP_EXPORT pL_ANNGETBITMAPDPIY pL_AnnGetBitmapDpiY;
extern LWRP_EXPORT pL_ANNGETBOUNDINGRECT  pL_AnnGetBoundingRect;
extern LWRP_EXPORT pL_ANNGETCONTAINER  pL_AnnGetContainer;
extern LWRP_EXPORT pL_ANNGETDISTANCE   pL_AnnGetDistance;
extern LWRP_EXPORT pL_ANNGETDPIX pL_AnnGetDpiX;
extern LWRP_EXPORT pL_ANNGETDPIY pL_AnnGetDpiY;
extern LWRP_EXPORT pL_ANNGETFILLMODE   pL_AnnGetFillMode;
extern LWRP_EXPORT pL_ANNGETFILLPATTERN   pL_AnnGetFillPattern;
extern LWRP_EXPORT pL_ANNGETFONTBOLD   pL_AnnGetFontBold;
extern LWRP_EXPORT pL_ANNGETFONTITALIC pL_AnnGetFontItalic;
extern LWRP_EXPORT pL_ANNGETFONTNAME   pL_AnnGetFontName;
extern LWRP_EXPORT pL_ANNGETFONTNAMELEN   pL_AnnGetFontNameLen;
extern LWRP_EXPORT pL_ANNGETFONTSIZE   pL_AnnGetFontSize;
extern LWRP_EXPORT pL_ANNGETFONTSTRIKETHROUGH   pL_AnnGetFontStrikeThrough;
extern LWRP_EXPORT pL_ANNGETFONTUNDERLINE pL_AnnGetFontUnderline;
extern LWRP_EXPORT pL_ANNGETFORECOLOR  pL_AnnGetForeColor;
extern LWRP_EXPORT pL_ANNGETGAUGELENGTH   pL_AnnGetGaugeLength;
extern LWRP_EXPORT pL_ANNGETTICMARKLENGTH pL_AnnGetTicMarkLength;
extern LWRP_EXPORT pL_ANNGETHYPERLINK  pL_AnnGetHyperlink;
extern LWRP_EXPORT pL_ANNGETHYPERLINKLEN  pL_AnnGetHyperlinkLen;
extern LWRP_EXPORT pL_ANNGETHYPERLINKMENUENABLE pL_AnnGetHyperlinkMenuEnable;
extern LWRP_EXPORT pL_ANNGETLINESTYLE  pL_AnnGetLineStyle;
extern LWRP_EXPORT pL_ANNGETLINEWIDTH  pL_AnnGetLineWidth;
extern LWRP_EXPORT pL_ANNGETLOCKED  pL_AnnGetLocked;
extern LWRP_EXPORT pL_ANNGETOFFSETX pL_AnnGetOffsetX;
extern LWRP_EXPORT pL_ANNGETOFFSETY pL_AnnGetOffsetY;
extern LWRP_EXPORT pL_ANNGETPOINTCOUNT pL_AnnGetPointCount;
extern LWRP_EXPORT pL_ANNGETPOINTS  pL_AnnGetPoints;
extern LWRP_EXPORT pL_ANNGETPOLYFILLMODE  pL_AnnGetPolyFillMode;
extern LWRP_EXPORT pL_ANNGETRECT pL_AnnGetRect;
extern LWRP_EXPORT pL_ANNGETROP2 pL_AnnGetROP2;
extern LWRP_EXPORT pL_ANNGETSCALARX pL_AnnGetScalarX;
extern LWRP_EXPORT pL_ANNGETSCALARY pL_AnnGetScalarY;
extern LWRP_EXPORT pL_ANNGETSELECTCOUNT   pL_AnnGetSelectCount;
extern LWRP_EXPORT pL_ANNGETSELECTED   pL_AnnGetSelected;
extern LWRP_EXPORT pL_ANNGETSELECTITEMS   pL_AnnGetSelectItems;
extern LWRP_EXPORT pL_ANNGETSELECTRECT pL_AnnGetSelectRect;
extern LWRP_EXPORT pL_ANNGETTAG  pL_AnnGetTag;
extern LWRP_EXPORT pL_ANNGETTEXT pL_AnnGetText;
extern LWRP_EXPORT pL_ANNGETTEXTLEN pL_AnnGetTextLen;
extern LWRP_EXPORT pL_ANNGETTEXTALIGN  pL_AnnGetTextAlign;
extern LWRP_EXPORT pL_ANNGETTEXTROTATE pL_AnnGetTextRotate;
extern LWRP_EXPORT pL_ANNGETTEXTPOINTERFIXED pL_AnnGetTextPointerFixed;
extern LWRP_EXPORT pL_ANNSETTEXTEXPANDTOKENS pL_AnnSetTextExpandTokens;
extern LWRP_EXPORT pL_ANNGETTEXTEXPANDTOKENS pL_AnnGetTextExpandTokens;
extern LWRP_EXPORT pL_ANNGETTOOL pL_AnnGetTool;
extern LWRP_EXPORT pL_ANNGETTOOLBARBUTTONVISIBLE   pL_AnnGetToolBarButtonVisible;
extern LWRP_EXPORT pL_ANNGETTOOLBARCHECKED   pL_AnnGetToolBarChecked;
extern LWRP_EXPORT pL_ANNGETTRANSPARENT   pL_AnnGetTransparent;
extern LWRP_EXPORT pL_ANNGETTYPE pL_AnnGetType;
extern LWRP_EXPORT pL_ANNGETTOPCONTAINER  pL_AnnGetTopContainer;
extern LWRP_EXPORT pL_ANNGETUNIT pL_AnnGetUnit;
extern LWRP_EXPORT pL_ANNGETUNITLEN pL_AnnGetUnitLen;
extern LWRP_EXPORT pL_ANNGETUSERMODE   pL_AnnGetUserMode;
extern LWRP_EXPORT pL_ANNGETVISIBLE pL_AnnGetVisible;
extern LWRP_EXPORT pL_ANNGETWND  pL_AnnGetWnd;
extern LWRP_EXPORT pL_ANNINSERT  pL_AnnInsert;
extern LWRP_EXPORT pL_ANNGETITEM pL_AnnGetItem;
extern LWRP_EXPORT pL_ANNLOAD pL_AnnLoad;
extern LWRP_EXPORT pL_ANNLOADOFFSET pL_AnnLoadOffset;
extern LWRP_EXPORT pL_ANNLOADMEMORY pL_AnnLoadMemory;
extern LWRP_EXPORT pL_ANNLOCK pL_AnnLock;
extern LWRP_EXPORT pL_ANNMOVE pL_AnnMove;
extern LWRP_EXPORT pL_ANNPRINT   pL_AnnPrint;
extern LWRP_EXPORT pL_ANNREALIZE pL_AnnRealize;
extern LWRP_EXPORT pL_ANNRESIZE  pL_AnnResize;
extern LWRP_EXPORT pL_ANNREVERSE pL_AnnReverse;
extern LWRP_EXPORT pL_ANNREMOVE  pL_AnnRemove;
extern LWRP_EXPORT pL_ANNROTATE  pL_AnnRotate;
extern LWRP_EXPORT pL_ANNSAVE pL_AnnSave;
extern LWRP_EXPORT pL_ANNSAVEOFFSET pL_AnnSaveOffset;
extern LWRP_EXPORT pL_ANNSAVEMEMORY pL_AnnSaveMemory;
extern LWRP_EXPORT pL_ANNSAVETAG pL_AnnSaveTag;
extern LWRP_EXPORT pL_ANNSELECTPOINT   pL_AnnSelectPoint;
extern LWRP_EXPORT pL_ANNSELECTRECT pL_AnnSelectRect;
extern LWRP_EXPORT pL_ANNSENDTOBACK pL_AnnSendToBack;
extern LWRP_EXPORT pL_ANNSETACTIVESTATE   pL_AnnSetActiveState;
extern LWRP_EXPORT pL_ANNSETAUTOCONTAINER pL_AnnSetAutoContainer;
extern LWRP_EXPORT pL_ANNSETAUTODRAWENABLE   pL_AnnSetAutoDrawEnable;
extern LWRP_EXPORT pL_ANNSETAUTOMENUENABLE   pL_AnnSetAutoMenuEnable;
extern LWRP_EXPORT pL_ANNSETAUTOTEXT   pL_AnnSetAutoText;
extern LWRP_EXPORT pL_ANNSETBACKCOLOR  pL_AnnSetBackColor;
extern LWRP_EXPORT pL_ANNSETBITMAP  pL_AnnSetBitmap;
extern LWRP_EXPORT pL_ANNSETBITMAPDPIX pL_AnnSetBitmapDpiX;
extern LWRP_EXPORT pL_ANNSETBITMAPDPIY pL_AnnSetBitmapDpiY;
extern LWRP_EXPORT pL_ANNSETDPIX pL_AnnSetDpiX;
extern LWRP_EXPORT pL_ANNSETDPIY pL_AnnSetDpiY;
extern LWRP_EXPORT pL_ANNSETFILLMODE   pL_AnnSetFillMode;
extern LWRP_EXPORT pL_ANNSETFILLPATTERN   pL_AnnSetFillPattern;
extern LWRP_EXPORT pL_ANNSETFONTBOLD   pL_AnnSetFontBold;
extern LWRP_EXPORT pL_ANNSETFONTITALIC pL_AnnSetFontItalic;
extern LWRP_EXPORT pL_ANNSETFONTNAME   pL_AnnSetFontName;
extern LWRP_EXPORT pL_ANNSETFONTSIZE   pL_AnnSetFontSize;
extern LWRP_EXPORT pL_ANNSETFONTSTRIKETHROUGH   pL_AnnSetFontStrikeThrough;
extern LWRP_EXPORT pL_ANNSETFONTUNDERLINE pL_AnnSetFontUnderline;
extern LWRP_EXPORT pL_ANNSETFORECOLOR  pL_AnnSetForeColor;
extern LWRP_EXPORT pL_ANNSETGAUGELENGTH   pL_AnnSetGaugeLength;
extern LWRP_EXPORT pL_ANNSETTICMARKLENGTH pL_AnnSetTicMarkLength;
extern LWRP_EXPORT pL_ANNSETHYPERLINK  pL_AnnSetHyperlink;
extern LWRP_EXPORT pL_ANNSETHYPERLINKMENUENABLE pL_AnnSetHyperlinkMenuEnable;
extern LWRP_EXPORT pL_ANNSETLINESTYLE  pL_AnnSetLineStyle;
extern LWRP_EXPORT pL_ANNSETLINEWIDTH  pL_AnnSetLineWidth;
extern LWRP_EXPORT pL_ANNSETOFFSETX pL_AnnSetOffsetX;
extern LWRP_EXPORT pL_ANNSETOFFSETY pL_AnnSetOffsetY;
extern LWRP_EXPORT pL_ANNSETPOINTS  pL_AnnSetPoints;
extern LWRP_EXPORT pL_ANNSETPOLYFILLMODE  pL_AnnSetPolyFillMode;
extern LWRP_EXPORT pL_ANNSETROP2 pL_AnnSetROP2;
extern LWRP_EXPORT pL_ANNSETRECT pL_AnnSetRect;
extern LWRP_EXPORT pL_ANNSETSELECTED   pL_AnnSetSelected;
extern LWRP_EXPORT pL_ANNSETSCALARX pL_AnnSetScalarX;
extern LWRP_EXPORT pL_ANNSETSCALARY pL_AnnSetScalarY;
extern LWRP_EXPORT pL_ANNSETTAG  pL_AnnSetTag;
extern LWRP_EXPORT pL_ANNSETTEXT pL_AnnSetText;
extern LWRP_EXPORT pL_ANNSETTEXTALIGN  pL_AnnSetTextAlign;
extern LWRP_EXPORT pL_ANNSETTEXTROTATE pL_AnnSetTextRotate;
extern LWRP_EXPORT pL_ANNSETTEXTPOINTERFIXED pL_AnnSetTextPointerFixed;
extern LWRP_EXPORT pL_ANNSETTOOL pL_AnnSetTool;
extern LWRP_EXPORT pL_ANNSETTOOLBARBUTTONVISIBLE   pL_AnnSetToolBarButtonVisible;
extern LWRP_EXPORT pL_ANNSETTOOLBARCHECKED   pL_AnnSetToolBarChecked;
extern LWRP_EXPORT pL_ANNSETTRANSPARENT   pL_AnnSetTransparent;
extern LWRP_EXPORT pL_ANNSETUNDODEPTH  pL_AnnSetUndoDepth;
extern LWRP_EXPORT pL_ANNSETUNIT pL_AnnSetUnit;
extern LWRP_EXPORT pL_ANNSETUSERMODE   pL_AnnSetUserMode;
extern LWRP_EXPORT pL_ANNSETVISIBLE pL_AnnSetVisible;
extern LWRP_EXPORT pL_ANNSETWND  pL_AnnSetWnd;
extern LWRP_EXPORT pL_ANNSHOWLOCKEDICON   pL_AnnShowLockedIcon;
extern LWRP_EXPORT pL_ANNUNDO pL_AnnUndo;
extern LWRP_EXPORT pL_ANNUNLOCK  pL_AnnUnlock;
extern LWRP_EXPORT pL_ANNUNREALIZE  pL_AnnUnrealize;
extern LWRP_EXPORT pL_ANNSETNODES   pL_AnnSetNodes;
extern LWRP_EXPORT pL_ANNGETNODES   pL_AnnGetNodes;
extern LWRP_EXPORT pL_ANNSETPROTRACTOROPTIONS   pL_AnnSetProtractorOptions;
extern LWRP_EXPORT pL_ANNGETPROTRACTOROPTIONS   pL_AnnGetProtractorOptions;
extern LWRP_EXPORT pL_ANNGETNAMEOPTIONS   pL_AnnGetNameOptions;
extern LWRP_EXPORT pL_ANNSETNAMEOPTIONS   pL_AnnSetNameOptions;
extern LWRP_EXPORT pL_ANNSETSHOWFLAGS  pL_AnnSetShowFlags;
extern LWRP_EXPORT pL_ANNGETSHOWFLAGS  pL_AnnGetShowFlags;
extern LWRP_EXPORT pL_ANNGETANGLE   pL_AnnGetAngle;
extern LWRP_EXPORT pL_ANNSETMETAFILE   pL_AnnSetMetafile;
extern LWRP_EXPORT pL_ANNGETMETAFILE   pL_AnnGetMetafile;
extern LWRP_EXPORT pL_ANNGETSECONDARYMETAFILE   pL_AnnGetSecondaryMetafile;
extern LWRP_EXPORT pL_ANNSETPREDEFINEDMETAFILE  pL_AnnSetPredefinedMetafile;
extern LWRP_EXPORT pL_ANNGETPREDEFINEDMETAFILE  pL_AnnGetPredefinedMetafile;
extern LWRP_EXPORT pL_ANNSETSECONDARYBITMAP  pL_AnnSetSecondaryBitmap;
extern LWRP_EXPORT pL_ANNGETSECONDARYBITMAP  pL_AnnGetSecondaryBitmap;
extern LWRP_EXPORT pL_ANNSETAUTOMENUITEMENABLE  pL_AnnSetAutoMenuItemEnable;
extern LWRP_EXPORT pL_ANNGETAUTOMENUITEMENABLE  pL_AnnGetAutoMenuItemEnable;
extern LWRP_EXPORT pL_ANNSETAUTOMENUSTATE pL_AnnSetAutoMenuState;
extern LWRP_EXPORT pL_ANNGETAUTOMENUSTATE pL_AnnGetAutoMenuState;
extern LWRP_EXPORT pL_ANNSETUSER pL_AnnSetUser;
extern LWRP_EXPORT pL_ANNSETTOOLBARBUTTONS   pL_AnnSetToolBarButtons;
extern LWRP_EXPORT pL_ANNGETTOOLBARBUTTONS   pL_AnnGetToolBarButtons;
extern LWRP_EXPORT pL_ANNFREETOOLBARBUTTONS  pL_AnnFreeToolBarButtons;
extern LWRP_EXPORT pL_ANNGETTOOLBARINFO   pL_AnnGetToolBarInfo;
extern LWRP_EXPORT pL_ANNSETTOOLBARCOLUMNS   pL_AnnSetToolBarColumns;
extern LWRP_EXPORT pL_ANNSETTOOLBARROWS   pL_AnnSetToolBarRows;
extern LWRP_EXPORT pL_ANNSETAUTODEFAULTS  pL_AnnSetAutoDefaults;
extern LWRP_EXPORT pL_ANNSETTRANSPARENTCOLOR pL_AnnSetTransparentColor;
extern LWRP_EXPORT pL_ANNGETTRANSPARENTCOLOR pL_AnnGetTransparentColor;
extern LWRP_EXPORT pL_ANNGETUNDODEPTH  pL_AnnGetUndoDepth;
extern LWRP_EXPORT pL_ANNGROUP   pL_AnnGroup;
extern LWRP_EXPORT pL_ANNUNGROUP pL_AnnUngroup;
extern LWRP_EXPORT pL_ANNSETAUTOOPTIONS   pL_AnnSetAutoOptions;
extern LWRP_EXPORT pL_ANNGETAUTOOPTIONS   pL_AnnGetAutoOptions;
extern LWRP_EXPORT pL_ANNGETOBJECTFROMTAG pL_AnnGetObjectFromTag;
extern LWRP_EXPORT pL_ANNGETRGNHANDLE  pL_AnnGetRgnHandle;
extern LWRP_EXPORT pL_ANNGETAREA pL_AnnGetArea;
extern LWRP_EXPORT pL_ANNSETAUTODIALOGFONTSIZE  pL_AnnSetAutoDialogFontSize;
extern LWRP_EXPORT pL_ANNGETAUTODIALOGFONTSIZE  pL_AnnGetAutoDialogFontSize;
extern LWRP_EXPORT pL_ANNSETGROUPING   pL_AnnSetGrouping;
extern LWRP_EXPORT pL_ANNGETGROUPING   pL_AnnGetGrouping;
extern LWRP_EXPORT pL_ANNSETAUTOBACKCOLOR pL_AnnSetAutoBackColor;
extern LWRP_EXPORT pL_ANNGETAUTOBACKCOLOR pL_AnnGetAutoBackColor;
extern LWRP_EXPORT pL_ANNADDUNDONODE   pL_AnnAddUndoNode;
extern LWRP_EXPORT pL_ANNSETAUTOUNDOENABLE   pL_AnnSetAutoUndoEnable;
extern LWRP_EXPORT pL_ANNGETAUTOUNDOENABLE   pL_AnnGetAutoUndoEnable;
extern LWRP_EXPORT pL_ANNSETTOOLBARPARENT pL_AnnSetToolBarParent;
extern LWRP_EXPORT pL_ANNSETENCRYPTOPTIONS   pL_AnnSetEncryptOptions;
extern LWRP_EXPORT pL_ANNGETENCRYPTOPTIONS   pL_AnnGetEncryptOptions;
extern LWRP_EXPORT pL_ANNENCRYPTAPPLY  pL_AnnEncryptApply;
extern LWRP_EXPORT pL_ANNSETPREDEFINEDBITMAP pL_AnnSetPredefinedBitmap;
extern LWRP_EXPORT pL_ANNGETPREDEFINEDBITMAP pL_AnnGetPredefinedBitmap;
extern LWRP_EXPORT pL_ANNGETPOINTOPTIONS  pL_AnnGetPointOptions;
extern LWRP_EXPORT pL_ANNSETPOINTOPTIONS  pL_AnnSetPointOptions;
extern LWRP_EXPORT pL_ANNADDUSERHANDLE pL_AnnAddUserHandle;
extern LWRP_EXPORT pL_ANNGETUSERHANDLE pL_AnnGetUserHandle;
extern LWRP_EXPORT pL_ANNGETUSERHANDLES   pL_AnnGetUserHandles;
extern LWRP_EXPORT pL_ANNCHANGEUSERHANDLE pL_AnnChangeUserHandle;
extern LWRP_EXPORT pL_ANNDELETEUSERHANDLE pL_AnnDeleteUserHandle;
extern LWRP_EXPORT pL_ANNENUMERATEHANDLES pL_AnnEnumerateHandles;
// Misc functions
extern LWRP_EXPORT pL_ANNDEBUG   pL_AnnDebug;
extern LWRP_EXPORT pL_ANNDUMPOBJECT pL_AnnDumpObject;
extern LWRP_EXPORT pL_ANNHITTEST pL_AnnHitTest;
extern LWRP_EXPORT pL_ANNGETROTATEANGLE   pL_AnnGetRotateAngle;
extern LWRP_EXPORT pL_ANNADJUSTPOINT   pL_AnnAdjustPoint;
extern LWRP_EXPORT pL_ANNCONVERT pL_AnnConvert;
extern LWRP_EXPORT pL_ANNRESTRICTCURSOR   pL_AnnRestrictCursor;
extern LWRP_EXPORT pL_ANNSETRESTRICTTOCONTAINER pL_AnnSetRestrictToContainer;
extern LWRP_EXPORT pL_ANNGETRESTRICTTOCONTAINER pL_AnnGetRestrictToContainer;
extern LWRP_EXPORT pL_ANNDEFINE2 pL_AnnDefine2;
// Text Token Table Functions
extern LWRP_EXPORT pL_ANNINSERTTEXTTOKENTABLE   pL_AnnInsertTextTokenTable;
extern LWRP_EXPORT pL_ANNENUMERATETEXTTOKENTABLE   pL_AnnEnumerateTextTokenTable;
extern LWRP_EXPORT pL_ANNDELETETEXTTOKENTABLE   pL_AnnDeleteTextTokenTable;
extern LWRP_EXPORT pL_ANNCLEARTEXTTOKENTABLE pL_AnnClearTextTokenTable;
// Fixed Annotation Functions
extern LWRP_EXPORT pL_ANNSETNOSCROLL   pL_AnnSetNoScroll;
extern LWRP_EXPORT pL_ANNGETNOSCROLL   pL_AnnGetNoScroll;
extern LWRP_EXPORT pL_ANNSETNOZOOM  pL_AnnSetNoZoom;
extern LWRP_EXPORT pL_ANNGETNOZOOM  pL_AnnGetNoZoom;
extern LWRP_EXPORT pL_ANNENABLEFIXED   pL_AnnEnableFixed;
extern LWRP_EXPORT pL_ANNGETFIXED   pL_AnnGetFixed;
extern LWRP_EXPORT pL_ANNSETFIXED   pL_AnnSetFixed;
extern LWRP_EXPORT pL_ANNPUSHFIXEDSTATE   pL_AnnPushFixedState;
extern LWRP_EXPORT pL_ANNPOPFIXEDSTATE pL_AnnPopFixedState;
extern LWRP_EXPORT pL_ANNISFIXEDINRECT pL_AnnIsFixedInRect;
extern LWRP_EXPORT pL_ANNGETDISTANCE2  pL_AnnGetDistance2;
extern LWRP_EXPORT pL_ANNDUMPSMARTDISTANCE   pL_AnnDumpSmartDistance;
extern LWRP_EXPORT pL_ANNSETAUTOCURSOR pL_AnnSetAutoCursor;
extern LWRP_EXPORT pL_ANNGETAUTOCURSOR pL_AnnGetAutoCursor;
extern LWRP_EXPORT pL_ANNSETUSERDATA   pL_AnnSetUserData;
extern LWRP_EXPORT pL_ANNGETUSERDATA   pL_AnnGetUserData;
extern LWRP_EXPORT pL_ANNSETTEXTRTF pL_AnnSetTextRTF;
extern LWRP_EXPORT pL_ANNGETTEXTRTF pL_AnnGetTextRTF;
extern LWRP_EXPORT pL_DEBUGENABLEWORLDTRANSFORM pL_DebugEnableWorldTransform;
extern LWRP_EXPORT pL_DEBUGSETGLOBAL   pL_DebugSetGlobal;
extern LWRP_EXPORT pL_DEBUGGETGLOBAL   pL_DebugGetGlobal;
extern LWRP_EXPORT pL_ANNSETLOCALE  pL_AnnSetlocale;
extern LWRP_EXPORT pL_ANNSETAUTOHILIGHTPEN   pL_AnnSetAutoHilightPen;
extern LWRP_EXPORT pL_ANNSETOPTIONS pL_AnnSetOptions;
extern LWRP_EXPORT pL_ANNGETOPTIONS pL_AnnGetOptions;
extern LWRP_EXPORT pL_ANNGETROTATEOPTIONS pL_AnnGetRotateOptions;
extern LWRP_EXPORT pL_ANNSETROTATEOPTIONS pL_AnnSetRotateOptions;
extern LWRP_EXPORT pL_ANNCALIBRATERULER   pL_AnnCalibrateRuler;
extern LWRP_EXPORT pL_ANNTEXTEDIT   pL_AnnTextEdit;
extern LWRP_EXPORT pL_ANNSETTEXTOPTIONS   pL_AnnSetTextOptions;
extern LWRP_EXPORT pL_ANNGETTEXTOPTIONS   pL_AnnGetTextOptions;
extern LWRP_EXPORT pL_ANNGETAUTOSNAPCURSOR   pL_AnnGetAutoSnapCursor;
extern LWRP_EXPORT pL_ANNSETAUTOSNAPCURSOR   pL_AnnSetAutoSnapCursor;
extern LWRP_EXPORT pL_ANNSETTEXTFIXEDSIZE pL_AnnSetTextFixedSize;
extern LWRP_EXPORT pL_ANNGETTEXTFIXEDSIZE pL_AnnGetTextFixedSize;
extern LWRP_EXPORT pL_ANNGETLINEFIXEDWIDTH   pL_AnnGetLineFixedWidth;
extern LWRP_EXPORT pL_ANNSETLINEFIXEDWIDTH   pL_AnnSetLineFixedWidth;
extern LWRP_EXPORT pL_ANNGETPOINTEROPTIONS   pL_AnnGetPointerOptions;
extern LWRP_EXPORT pL_ANNSETPOINTEROPTIONS   pL_AnnSetPointerOptions;
extern LWRP_EXPORT pL_ANNSETRENDERMODE pL_AnnSetRenderMode;
extern LWRP_EXPORT pL_ANNGETSHOWSTAMPBORDER pL_AnnGetShowStampBorder;
extern LWRP_EXPORT pL_ANNSETSHOWSTAMPBORDER pL_AnnSetShowStampBorder;

//-----------------------------------------------------------------------------
//--LTSCR.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_SETCAPTUREOPTION                 pL_SetCaptureOption;
extern LWRP_EXPORT pL_GETCAPTUREOPTION                 pL_GetCaptureOption;
extern LWRP_EXPORT pL_CAPTUREWINDOW                    pL_CaptureWindow;
extern LWRP_EXPORT pL_CAPTUREACTIVEWINDOW              pL_CaptureActiveWindow;
extern LWRP_EXPORT pL_CAPTUREACTIVECLIENT              pL_CaptureActiveClient;
extern LWRP_EXPORT pL_CAPTUREWALLPAPER                 pL_CaptureWallPaper;
extern LWRP_EXPORT pL_CAPTUREFULLSCREEN                pL_CaptureFullScreen;
extern LWRP_EXPORT pL_CAPTUREMENUUNDERCURSOR           pL_CaptureMenuUnderCursor;
extern LWRP_EXPORT pL_CAPTUREWINDOWUNDERCURSOR         pL_CaptureWindowUnderCursor;
extern LWRP_EXPORT pL_CAPTURESELECTEDOBJECT            pL_CaptureSelectedObject;
extern LWRP_EXPORT pL_CAPTUREAREA                      pL_CaptureArea;
extern LWRP_EXPORT pL_CAPTUREMOUSECURSOR               pL_CaptureMouseCursor;
extern LWRP_EXPORT pL_CAPTURESETHOTKEYCALLBACK         pL_CaptureSetHotKeyCallback;
extern LWRP_EXPORT pL_SETCAPTUREOPTIONDLG              pL_SetCaptureOptionDlg;
extern LWRP_EXPORT pL_CAPTUREAREAOPTIONDLG             pL_CaptureAreaOptionDlg;
extern LWRP_EXPORT pL_CAPTUREOBJECTOPTIONDLG           pL_CaptureObjectOptionDlg;
extern LWRP_EXPORT pL_GETDEFAULTAREAOPTION             pL_GetDefaultAreaOption;
extern LWRP_EXPORT pL_GETDEFAULTOBJECTOPTION           pL_GetDefaultObjectOption;
extern LWRP_EXPORT pL_STOPCAPTURE                      pL_StopCapture;
extern LWRP_EXPORT pL_CAPTUREFROMEXEDLG                pL_CaptureFromExeDlg;
extern LWRP_EXPORT pL_CAPTUREFROMEXE                   pL_CaptureFromExe;
extern LWRP_EXPORT pL_CAPTUREGETRESCOUNT               pL_CaptureGetResCount;
extern LWRP_EXPORT pL_ISCAPTUREACTIVE                  pL_IsCaptureActive;

//-----------------------------------------------------------------------------
//--LTNET.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_INETCONNECT   pL_InetConnect;
extern LWRP_EXPORT pL_INETSERVERINIT   pL_InetServerInit;
extern LWRP_EXPORT pL_INETCLOSE  pL_InetClose;
extern LWRP_EXPORT pL_INETSENDDATA  pL_InetSendData;
extern LWRP_EXPORT pL_INETSENDMMDATA   pL_InetSendMMData;
extern LWRP_EXPORT pL_INETREADDATA  pL_InetReadData;
extern LWRP_EXPORT pL_INETGETHOSTNAME  pL_InetGetHostName;
extern LWRP_EXPORT pL_INETACCEPTCONNECT   pL_InetAcceptConnect;
extern LWRP_EXPORT pL_INETSENDBITMAP   pL_InetSendBitmap;
extern LWRP_EXPORT pL_INETAUTOPROCESS  pL_InetAutoProcess;
extern LWRP_EXPORT pL_INETSENDRAWDATA  pL_InetSendRawData;
extern LWRP_EXPORT pL_INETGETQUEUESIZE pL_InetGetQueueSize;
extern LWRP_EXPORT pL_INETCLEARQUEUE   pL_InetClearQueue;
extern LWRP_EXPORT pL_INETSTARTUP   pL_InetStartUp;
extern LWRP_EXPORT pL_INETSHUTDOWN  pL_InetShutDown;
extern LWRP_EXPORT pL_INETSENDSOUND pL_InetSendSound;
extern LWRP_EXPORT pL_INETATTACHTOSOCKET  pL_InetAttachToSocket;
extern LWRP_EXPORT pL_INETDETACHFROMSOCKET   pL_InetDetachFromSocket;
extern LWRP_EXPORT pL_INETSETCALLBACK  pL_InetSetCallback;
extern LWRP_EXPORT pL_INETGETCALLBACK  pL_InetGetCallback;
extern LWRP_EXPORT pL_INETCREATEPACKET pL_InetCreatePacket;
extern LWRP_EXPORT pL_INETCREATEPACKETFROMPARAMS   pL_InetCreatePacketFromParams;
extern LWRP_EXPORT pL_INETFREEPACKET   pL_InetFreePacket;
extern LWRP_EXPORT pL_INETSENDCMD   pL_InetSendCmd;
extern LWRP_EXPORT pL_INETSENDRSP   pL_InetSendRsp;
extern LWRP_EXPORT pL_INETSENDLOADCMD  pL_InetSendLoadCmd;
extern LWRP_EXPORT pL_INETSENDLOADRSP  pL_InetSendLoadRsp;
extern LWRP_EXPORT pL_INETSENDSAVECMD  pL_InetSendSaveCmd;
extern LWRP_EXPORT pL_INETSENDSAVERSP  pL_InetSendSaveRsp;
extern LWRP_EXPORT pL_INETSENDCREATEWINCMD   pL_InetSendCreateWinCmd;
extern LWRP_EXPORT pL_INETSENDCREATEWINRSP   pL_InetSendCreateWinRsp;
extern LWRP_EXPORT pL_INETSENDSIZEWINCMD  pL_InetSendSizeWinCmd;
extern LWRP_EXPORT pL_INETSENDSIZEWINRSP  pL_InetSendSizeWinRsp;
extern LWRP_EXPORT pL_INETSENDSHOWWINCMD  pL_InetSendShowWinCmd;
extern LWRP_EXPORT pL_INETSENDSHOWWINRSP  pL_InetSendShowWinRsp;
extern LWRP_EXPORT pL_INETSENDCLOSEWINCMD pL_InetSendCloseWinCmd;
extern LWRP_EXPORT pL_INETSENDCLOSEWINRSP pL_InetSendCloseWinRsp;
extern LWRP_EXPORT pL_INETSENDFREEBITMAPCMD  pL_InetSendFreeBitmapCmd;
extern LWRP_EXPORT pL_INETSENDFREEBITMAPRSP  pL_InetSendFreeBitmapRsp;
extern LWRP_EXPORT pL_INETSENDSETRECTCMD  pL_InetSendSetRectCmd;
extern LWRP_EXPORT pL_INETSENDSETRECTRSP  pL_InetSendSetRectRsp;
extern LWRP_EXPORT pL_INETSETCOMMANDCALLBACK pL_InetSetCommandCallback;
extern LWRP_EXPORT pL_INETSETRESPONSECALLBACK   pL_InetSetResponseCallback;
extern LWRP_EXPORT pL_INETSENDATTACHBITMAPCMD   pL_InetSendAttachBitmapCmd;
extern LWRP_EXPORT pL_INETSENDATTACHBITMAPRSP   pL_InetSendAttachBitmapRsp;
extern LWRP_EXPORT pL_INETSENDGETMAGGLASSDATACMD   pL_InetSendGetMagGlassDataCmd;
extern LWRP_EXPORT pL_INETSENDGETMAGGLASSDATARSP   pL_InetSendGetMagGlassDataRsp;
extern LWRP_EXPORT pL_INETGETMAGGLASSDATA pL_InetGetMagGlassData;
extern LWRP_EXPORT pL_INETGETPARAMETERS pL_InetGetParameters;
extern LWRP_EXPORT pL_INETCOPYPARAMETERS pL_InetCopyParameters;
extern LWRP_EXPORT pL_INETFREEPARAMETERS pL_InetFreeParameters;

//-----------------------------------------------------------------------------
//--LTWEB.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_INETFTPCONNECT   pL_InetFtpConnect;
extern LWRP_EXPORT pL_INETFTPDISCONNECT   pL_InetFtpDisConnect;
extern LWRP_EXPORT pL_INETFTPSENDFILE  pL_InetFtpSendFile;
extern LWRP_EXPORT pL_INETFTPCHANGEDIR pL_InetFtpChangeDir;
extern LWRP_EXPORT pL_INETFTPGETFILE   pL_InetFtpGetFile;
extern LWRP_EXPORT pL_INETFTPRENAMEFILE   pL_InetFtpRenameFile;
extern LWRP_EXPORT pL_INETFTPDELETEFILE   pL_InetFtpDeleteFile;
extern LWRP_EXPORT pL_INETFTPCREATEDIR pL_InetFtpCreateDir;
extern LWRP_EXPORT pL_INETFTPDELETEDIR pL_InetFtpDeleteDir;
extern LWRP_EXPORT pL_INETFTPGETCURRENTDIR   pL_InetFtpGetCurrentDir;
extern LWRP_EXPORT pL_INETFTPSENDBITMAP   pL_InetFtpSendBitmap;
extern LWRP_EXPORT pL_INETFTPBROWSEDIR pL_InetFtpBrowseDir;
extern LWRP_EXPORT pL_INETHTTPCONNECT  pL_InetHttpConnect;
extern LWRP_EXPORT pL_INETHTTPDISCONNECT  pL_InetHttpDisconnect;
extern LWRP_EXPORT pL_INETHTTPOPENREQUEST pL_InetHttpOpenRequest;
extern LWRP_EXPORT pL_INETHTTPOPENREQUESTEX  pL_InetHttpOpenRequestEx;
extern LWRP_EXPORT pL_INETHTTPCLOSEREQUEST   pL_InetHttpCloseRequest;
extern LWRP_EXPORT pL_INETHTTPSENDREQUEST pL_InetHttpSendRequest;
extern LWRP_EXPORT pL_INETHTTPSENDBITMAP  pL_InetHttpSendBitmap;
extern LWRP_EXPORT pL_INETHTTPSENDDATA pL_InetHttpSendData;
extern LWRP_EXPORT pL_INETHTTPSENDFORM pL_InetHttpSendForm;
extern LWRP_EXPORT pL_INETHTTPGETRESPONSE pL_InetHttpGetResponse;
extern LWRP_EXPORT pL_INETHTTPGETSERVERSTATUS   pL_InetHttpGetServerStatus;

//-----------------------------------------------------------------------------
//--LTTMB.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_BROWSEDIR  pL_BrowseDir;

//-----------------------------------------------------------------------------
//--LTLST.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_CREATEIMAGELISTCONTROL pL_CreateImageListControl;
extern LWRP_EXPORT pL_USEIMAGELISTCONTROL pL_UseImageListControl;


//-----------------------------------------------------------------------------
//--LVKRN.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_VECDUPLICATEOBJECTDESCRIPTOR pL_VecDuplicateObjectDescriptor;
//Do not remove the one above

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  General functions.                                                   []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  General functions.                                                   []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECINIT pL_VecInit;
extern LWRP_EXPORT pL_VECFREE pL_VecFree;
extern LWRP_EXPORT pL_VECEMPTY   pL_VecEmpty;
extern LWRP_EXPORT pL_VECISEMPTY pL_VecIsEmpty;
extern LWRP_EXPORT pL_VECCOPY pL_VecCopy;
extern LWRP_EXPORT pL_VECSETDISPLAYOPTIONS   pL_VecSetDisplayOptions;
extern LWRP_EXPORT pL_VECGETDISPLAYOPTIONS   pL_VecGetDisplayOptions;
extern LWRP_EXPORT pL_VECINVERTCOLORS  pL_VecInvertColors;
extern LWRP_EXPORT pL_VECSETVIEWPORT   pL_VecSetViewport;
extern LWRP_EXPORT pL_VECGETVIEWPORT   pL_VecGetViewport;
extern LWRP_EXPORT pL_VECSETPAN  pL_VecSetPan;
extern LWRP_EXPORT pL_VECGETPAN  pL_VecGetPan;
extern LWRP_EXPORT pL_VECPAINT   pL_VecPaint;
extern LWRP_EXPORT pL_VECREALIZE pL_VecRealize;
extern LWRP_EXPORT pL_VECPAINTDC pL_VecPaintDC;
extern LWRP_EXPORT pL_VECIS3D pL_VecIs3D;
extern LWRP_EXPORT pL_VECISLOCKED   pL_VecIsLocked;
extern LWRP_EXPORT pL_VECSETLOCKED  pL_VecSetLocked;
extern LWRP_EXPORT pL_VECSETBACKGROUNDCOLOR  pL_VecSetBackgroundColor;
extern LWRP_EXPORT pL_VECGETBACKGROUNDCOLOR  pL_VecGetBackgroundColor;
extern LWRP_EXPORT pL_VECLOGICALTOPHYSICAL   pL_VecLogicalToPhysical;
extern LWRP_EXPORT pL_VECPHYSICALTOLOGICAL   pL_VecPhysicalToLogical;
extern LWRP_EXPORT pL_VECSETPALETTE pL_VecSetPalette;
extern LWRP_EXPORT pL_VECGETPALETTE pL_VecGetPalette;
extern LWRP_EXPORT pL_VECSETVIEWMODE   pL_VecSetViewMode;
extern LWRP_EXPORT pL_VECGETVIEWMODE   pL_VecGetViewMode;
/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Transformation function.                                             []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECSETTRANSLATION   pL_VecSetTranslation;
extern LWRP_EXPORT pL_VECGETTRANSLATION   pL_VecGetTranslation;
extern LWRP_EXPORT pL_VECSETROTATION   pL_VecSetRotation;
extern LWRP_EXPORT pL_VECGETROTATION   pL_VecGetRotation;
extern LWRP_EXPORT pL_VECSETSCALE   pL_VecSetScale;
extern LWRP_EXPORT pL_VECGETSCALE   pL_VecGetScale;
extern LWRP_EXPORT pL_VECSETORIGIN  pL_VecSetOrigin;
extern LWRP_EXPORT pL_VECGETORIGIN  pL_VecGetOrigin;
extern LWRP_EXPORT pL_VECAPPLYTRANSFORMATION pL_VecApplyTransformation;
extern LWRP_EXPORT pL_VECZOOMRECT   pL_VecZoomRect;
/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Attributes functions.                                                []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECSETBINDVERTICESMODE pL_VecSetBindVerticesMode;
extern LWRP_EXPORT pL_VECGETBINDVERTICESMODE pL_VecGetBindVerticesMode;
extern LWRP_EXPORT pL_VECSETPARALLELOGRAM pL_VecSetParallelogram;
extern LWRP_EXPORT pL_VECGETPARALLELOGRAM pL_VecGetParallelogram;
extern LWRP_EXPORT pL_VECENUMVERTICES  pL_VecEnumVertices;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Camera functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECSETCAMERA  pL_VecSetCamera;
extern LWRP_EXPORT pL_VECGETCAMERA  pL_VecGetCamera;
/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Metafile functions.                                                  []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECCONVERTTOWMF  pL_VecConvertToWMF;
extern LWRP_EXPORT pL_VECCONVERTFROMWMF   pL_VecConvertFromWMF;
extern LWRP_EXPORT pL_VECCONVERTTOEMF  pL_VecConvertToEMF;
extern LWRP_EXPORT pL_VECCONVERTFROMEMF   pL_VecConvertFromEMF;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Engine functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECATTACHTOWINDOW   pL_VecAttachToWindow;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Marker functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECSETMARKER  pL_VecSetMarker;
extern LWRP_EXPORT pL_VECGETMARKER  pL_VecGetMarker;
/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Unit functions.                                                      []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
/* Reserved for internal use */
extern LWRP_EXPORT pL_VECSETUNIT pL_VecSetUnit;
extern LWRP_EXPORT pL_VECGETUNIT pL_VecGetUnit;
extern LWRP_EXPORT pL_VECCONVERTPOINTTOUNIT pL_VecConvertPointToUnit;
extern LWRP_EXPORT pL_VECCONVERTPOINTFROMUNIT  pL_VecConvertPointFromUnit;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Hit test functions.                                                  []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECSETHITTEST pL_VecSetHitTest;
extern LWRP_EXPORT pL_VECGETHITTEST pL_VecGetHitTest;
extern LWRP_EXPORT pL_VECHITTEST pL_VecHitTest;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Polygon functions.                                                   []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECSETPOLYGONMODE   pL_VecSetPolygonMode;
extern LWRP_EXPORT pL_VECGETPOLYGONMODE   pL_VecGetPolygonMode;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Clipboard functions.                                                 []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECCLIPBOARDREADY   pL_VecClipboardReady;
extern LWRP_EXPORT pL_VECCOPYTOCLIPBOARD  pL_VecCopyToClipboard;
extern LWRP_EXPORT pL_VECCOPYFROMCLIPBOARD   pL_VecCopyFromClipboard;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Layer functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECADDLAYER   pL_VecAddLayer;
extern LWRP_EXPORT pL_VECDELETELAYER   pL_VecDeleteLayer;
extern LWRP_EXPORT pL_VECEMPTYLAYER pL_VecEmptyLayer;
extern LWRP_EXPORT pL_VECCOPYLAYER  pL_VecCopyLayer;
extern LWRP_EXPORT pL_VECGETLAYERBYNAME   pL_VecGetLayerByName;
extern LWRP_EXPORT pL_VECGETLAYERCOUNT pL_VecGetLayerCount;
extern LWRP_EXPORT pL_VECGETLAYERBYINDEX  pL_VecGetLayerByIndex;
extern LWRP_EXPORT pL_VECGETLAYER   pL_VecGetLayer;
extern LWRP_EXPORT pL_VECFREELAYER  pL_VecFreeLayer;
extern LWRP_EXPORT pL_VECSETLAYER   pL_VecSetLayer;
extern LWRP_EXPORT pL_VECSETACTIVELAYER   pL_VecSetActiveLayer;
extern LWRP_EXPORT pL_VECGETACTIVELAYER   pL_VecGetActiveLayer;
extern LWRP_EXPORT pL_VECENUMLAYERS pL_VecEnumLayers;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Group functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECADDGROUP   pL_VecAddGroup;
extern LWRP_EXPORT pL_VECDELETEGROUP   pL_VecDeleteGroup;
extern LWRP_EXPORT pL_VECDELETEGROUPCLONES   pL_VecDeleteGroupClones;
extern LWRP_EXPORT pL_VECEMPTYGROUP pL_VecEmptyGroup;
extern LWRP_EXPORT pL_VECCOPYGROUP  pL_VecCopyGroup;
extern LWRP_EXPORT pL_VECGETGROUPBYNAME   pL_VecGetGroupByName;
extern LWRP_EXPORT pL_VECGETGROUPCOUNT pL_VecGetGroupCount;
extern LWRP_EXPORT pL_VECGETGROUPBYINDEX  pL_VecGetGroupByIndex;
extern LWRP_EXPORT pL_VECGETGROUP   pL_VecGetGroup;
extern LWRP_EXPORT pL_VECFREEGROUP  pL_VecFreeGroup;
extern LWRP_EXPORT pL_VECSETGROUP   pL_VecSetGroup;
extern LWRP_EXPORT pL_VECENUMGROUPS pL_VecEnumGroups;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Object functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECINITOBJECT pL_VecInitObject;
extern LWRP_EXPORT pL_VECADDOBJECT  pL_VecAddObject;
extern LWRP_EXPORT pL_VECDELETEOBJECT  pL_VecDeleteObject;
extern LWRP_EXPORT pL_VECCOPYOBJECT pL_VecCopyObject;
extern LWRP_EXPORT pL_VECGETOBJECT  pL_VecGetObject;
extern LWRP_EXPORT pL_VECFREEOBJECT pL_VecFreeObject;
extern LWRP_EXPORT pL_VECSETOBJECT  pL_VecSetObject;
extern LWRP_EXPORT pL_VECEXPLODEOBJECT pL_VecExplodeObject;
extern LWRP_EXPORT pL_VECGETOBJECTPARALLELOGRAM pL_VecGetObjectParallelogram;
extern LWRP_EXPORT pL_VECGETOBJECTRECT pL_VecGetObjectRect;
extern LWRP_EXPORT pL_VECISOBJECTINSIDEPARALLELOGRAM  pL_VecIsObjectInsideParallelogram;
extern LWRP_EXPORT pL_VECISOBJECTINSIDERECT  pL_VecIsObjectInsideRect;
extern LWRP_EXPORT pL_VECSELECTOBJECT  pL_VecSelectObject;
extern LWRP_EXPORT pL_VECISOBJECTSELECTED pL_VecIsObjectSelected;
extern LWRP_EXPORT pL_VECHIDEOBJECT pL_VecHideObject;
extern LWRP_EXPORT pL_VECISOBJECTHIDDEN   pL_VecIsObjectHidden;
extern LWRP_EXPORT pL_VECENUMOBJECTS   pL_VecEnumObjects;
extern LWRP_EXPORT pL_VECENUMOBJECTSINLAYER  pL_VecEnumObjectsInLayer;
extern LWRP_EXPORT pL_VECSETOBJECTATTRIBUTES pL_VecSetObjectAttributes;
extern LWRP_EXPORT pL_VECGETOBJECTATTRIBUTES pL_VecGetObjectAttributes;
extern LWRP_EXPORT pL_VECADDOBJECTTOGROUP pL_VecAddObjectToGroup;
extern LWRP_EXPORT pL_VECENUMOBJECTSINGROUP  pL_VecEnumObjectsInGroup;
extern LWRP_EXPORT pL_VECSETOBJECTTOOLTIP pL_VecSetObjectTooltip;
extern LWRP_EXPORT pL_VECGETOBJECTTOOLTIP pL_VecGetObjectTooltip;
extern LWRP_EXPORT pL_VECSHOWOBJECTTOOLTIP   pL_VecShowObjectTooltip;
extern LWRP_EXPORT pL_VECHIDEOBJECTTOOLTIP   pL_VecHideObjectTooltip;
extern LWRP_EXPORT pL_VECSETOBJECTVIEWCONTEXT   pL_VecSetObjectViewContext;
extern LWRP_EXPORT pL_VECGETOBJECTVIEWCONTEXT   pL_VecGetObjectViewContext;
extern LWRP_EXPORT pL_VECREMOVEOBJECTVIEWCONTEXT   pL_VecRemoveObjectViewContext;
extern LWRP_EXPORT pL_VECADDHYPERLINK  pL_VecAddHyperlink;
extern LWRP_EXPORT pL_VECSETHYPERLINK  pL_VecSetHyperlink;
extern LWRP_EXPORT pL_VECGETHYPERLINK  pL_VecGetHyperlink;
extern LWRP_EXPORT pL_VECGETHYPERLINKCOUNT   pL_VecGetHyperlinkCount;
extern LWRP_EXPORT pL_VECGOTOHYPERLINK pL_VecGotoHyperlink;
extern LWRP_EXPORT pL_VECSETOBJECTDESCRIPTION   pL_VecSetObjectDescription;
extern LWRP_EXPORT pL_VECGETOBJECTDESCRIPTION   pL_VecGetObjectDescription;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Event functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECSETEVENTCALLBACK pL_VecSetEventCallback;
extern LWRP_EXPORT pL_VECEVENT   pL_VecEvent;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Font Substitution functions.                                         []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
extern LWRP_EXPORT pL_VECSETFONTMAPPER pL_VecSetFontMapper;
extern LWRP_EXPORT pL_VECGETFONTMAPPER pL_VecGetFontMapper;

//-----------------------------------------------------------------------------
//--LVDLG.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_VECDLGROTATE  pL_VecDlgRotate;
extern LWRP_EXPORT pL_VECDLGSCALE   pL_VecDlgScale;
extern LWRP_EXPORT pL_VECDLGTRANSLATE  pL_VecDlgTranslate;
extern LWRP_EXPORT pL_VECDLGCAMERA  pL_VecDlgCamera;
extern LWRP_EXPORT pL_VECDLGRENDER  pL_VecDlgRender;
extern LWRP_EXPORT pL_VECDLGVIEWMODE   pL_VecDlgViewMode;
extern LWRP_EXPORT pL_VECDLGHITTEST pL_VecDlgHitTest;
extern LWRP_EXPORT pL_VECDLGEDITALLLAYERS pL_VecDlgEditAllLayers;
extern LWRP_EXPORT pL_VECDLGNEWLAYER   pL_VecDlgNewLayer;
extern LWRP_EXPORT pL_VECDLGEDITLAYER  pL_VecDlgEditLayer;
extern LWRP_EXPORT pL_VECDLGEDITALLGROUPS pL_VecDlgEditAllGroups;
extern LWRP_EXPORT pL_VECDLGNEWGROUP   pL_VecDlgNewGroup;
extern LWRP_EXPORT pL_VECDLGEDITGROUP  pL_VecDlgEditGroup;
extern LWRP_EXPORT pL_VECDLGNEWOBJECT  pL_VecDlgNewObject;
extern LWRP_EXPORT pL_VECDLGEDITOBJECT pL_VecDlgEditObject;
extern LWRP_EXPORT pL_VECDLGOBJECTATTRIBUTES pL_VecDlgObjectAttributes;
extern LWRP_EXPORT pL_VECDLGGETSTRINGLEN  pL_VecDlgGetStringLen;
extern LWRP_EXPORT pL_VECDLGGETSTRING  pL_VecDlgGetString;
extern LWRP_EXPORT pL_VECDLGSETSTRING  pL_VecDlgSetString;
extern LWRP_EXPORT pL_VECDLGSETFONT pL_VecDlgSetFont;

//-----------------------------------------------------------------------------
//--LTBAR.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_BARCODEREAD   pL_BarCodeRead;
extern LWRP_EXPORT pL_BARCODEWRITE  pL_BarCodeWrite;
extern LWRP_EXPORT pL_BARCODEFREE   pL_BarCodeFree;
extern LWRP_EXPORT pL_BARCODEISDUPLICATED pL_BarCodeIsDuplicated;
extern LWRP_EXPORT pL_BARCODEGETDUPLICATED   pL_BarCodeGetDuplicated;
extern LWRP_EXPORT pL_BARCODEGETFIRSTDUPLICATED pL_BarCodeGetFirstDuplicated;
extern LWRP_EXPORT pL_BARCODEGETNEXTDUPLICATED  pL_BarCodeGetNextDuplicated;
extern LWRP_EXPORT pL_BARCODEINIT   pL_BarCodeInit ;
extern LWRP_EXPORT pL_BARCODEEXIT   pL_BarCodeExit ;
extern LWRP_EXPORT pL_BARCODEVERSIONINFO  pL_BarCodeVersionInfo;

//-----------------------------------------------------------------------------
//--LTAUT.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_AUTISVALID pL_AutIsValid;
extern LWRP_EXPORT pL_AUTINIT pL_AutInit;
extern LWRP_EXPORT pL_AUTCREATE  pL_AutCreate;
extern LWRP_EXPORT pL_AUTFREE pL_AutFree;
extern LWRP_EXPORT pL_AUTSETUNDOLEVEL  pL_AutSetUndoLevel;
extern LWRP_EXPORT pL_AUTGETUNDOLEVEL  pL_AutGetUndoLevel;
extern LWRP_EXPORT pL_AUTCANUNDO pL_AutCanUndo;
extern LWRP_EXPORT pL_AUTCANREDO pL_AutCanRedo;
extern LWRP_EXPORT pL_AUTUNDO pL_AutUndo;
extern LWRP_EXPORT pL_AUTREDO pL_AutRedo;
extern LWRP_EXPORT pL_AUTSETUNDOENABLED   pL_AutSetUndoEnabled;
extern LWRP_EXPORT pL_AUTADDUNDONODE   pL_AutAddUndoNode;
extern LWRP_EXPORT pL_AUTSELECT  pL_AutSelect;
extern LWRP_EXPORT pL_AUTCLIPBOARDDATAREADY  pL_AutClipboardDataReady;
extern LWRP_EXPORT pL_AUTCUT  pL_AutCut;
extern LWRP_EXPORT pL_AUTCOPY pL_AutCopy;
extern LWRP_EXPORT pL_AUTPASTE   pL_AutPaste;
extern LWRP_EXPORT pL_AUTDELETE  pL_AutDelete;
extern LWRP_EXPORT pL_AUTPRINT   pL_AutPrint;
// Container Functions.
extern LWRP_EXPORT pL_AUTADDCONTAINER  pL_AutAddContainer;
extern LWRP_EXPORT pL_AUTREMOVECONTAINER  pL_AutRemoveContainer;
extern LWRP_EXPORT pL_AUTSETACTIVECONTAINER  pL_AutSetActiveContainer;
extern LWRP_EXPORT pL_AUTGETACTIVECONTAINER  pL_AutGetActiveContainer;
extern LWRP_EXPORT pL_AUTENUMCONTAINERS   pL_AutEnumContainers;
// Painting Functionts.
extern LWRP_EXPORT pL_AUTSETPAINTPROPERTY pL_AutSetPaintProperty;
extern LWRP_EXPORT pL_AUTGETPAINTPROPERTY pL_AutGetPaintProperty;
extern LWRP_EXPORT pL_AUTSETPAINTBKCOLOR  pL_AutSetPaintBkColor;
extern LWRP_EXPORT pL_AUTGETPAINTBKCOLOR  pL_AutGetPaintBkColor;
// Vector Functions.
extern LWRP_EXPORT pL_AUTSETVECTORPROPERTY   pL_AutSetVectorProperty;
extern LWRP_EXPORT pL_AUTGETVECTORPROPERTY   pL_AutGetVectorProperty;
extern LWRP_EXPORT pL_AUTEDITVECTOROBJECT pL_AutEditVectorObject;
//Toolbar Functions.
extern LWRP_EXPORT pL_AUTSETTOOLBAR pL_AutSetToolbar;
extern LWRP_EXPORT pL_AUTGETTOOLBAR pL_AutGetToolbar;
extern LWRP_EXPORT pL_AUTSETCURRENTTOOL   pL_AutSetCurrentTool;
extern LWRP_EXPORT pL_AUTGETCURRENTTOOL   pL_AutGetCurrentTool;

//-----------------------------------------------------------------------------
//--LTCON.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
 // general container operations funtions.
 extern LWRP_EXPORT pL_CONTAINERISVALID   pL_ContainerIsValid;
 extern LWRP_EXPORT pL_CONTAINERINIT   pL_ContainerInit; 
 extern LWRP_EXPORT pL_CONTAINERCREATE pL_ContainerCreate; 
 extern LWRP_EXPORT pL_CONTAINERFREE   pL_ContainerFree; 
 extern LWRP_EXPORT pL_CONTAINERUPDATE pL_ContainerUpdate; 
 extern LWRP_EXPORT pL_CONTAINERRESET  pL_ContainerReset; 
 extern LWRP_EXPORT pL_CONTAINEREDITOBJECT   pL_ContainerEditObject; 
 // setting functions.
 extern LWRP_EXPORT pL_CONTAINERSETOWNER  pL_ContainerSetOwner; 
 extern LWRP_EXPORT pL_CONTAINERSETMETRICS   pL_ContainerSetMetrics; 
 extern LWRP_EXPORT pL_CONTAINERSETOFFSET pL_ContainerSetOffset; 
 extern LWRP_EXPORT pL_CONTAINERSETSCALAR pL_ContainerSetScalar; 
 extern LWRP_EXPORT pL_CONTAINERSETOBJECTTYPE   pL_ContainerSetObjectType; 
 extern LWRP_EXPORT pL_CONTAINERSETOBJECTCURSOR pL_ContainerSetObjectCursor; 
 extern LWRP_EXPORT pL_CONTAINERSETENABLED   pL_ContainerSetEnabled; 
 extern LWRP_EXPORT pL_CONTAINERSETCALLBACK  pL_ContainerSetCallback; 
 extern LWRP_EXPORT pL_CONTAINERSETOWNERDRAW pL_ContainerSetOwnerDraw; 
 // getting functions.
 extern LWRP_EXPORT pL_CONTAINERGETOWNER  pL_ContainerGetOwner; 
 extern LWRP_EXPORT pL_CONTAINERGETMETRICS   pL_ContainerGetMetrics; 
 extern LWRP_EXPORT pL_CONTAINERGETOFFSET pL_ContainerGetOffset; 
 extern LWRP_EXPORT pL_CONTAINERGETSCALAR pL_ContainerGetScalar; 
 extern LWRP_EXPORT pL_CONTAINERGETOBJECTTYPE   pL_ContainerGetObjectType; 
 extern LWRP_EXPORT pL_CONTAINERGETOBJECTCURSOR pL_ContainerGetObjectCursor; 
 extern LWRP_EXPORT pL_CONTAINERGETCALLBACK  pL_ContainerGetCallback; 
 // status query functions.
 extern LWRP_EXPORT pL_CONTAINERISENABLED pL_ContainerIsEnabled; 
 extern LWRP_EXPORT pL_CONTAINERISOWNERDRAW  pL_ContainerIsOwnerDraw; 
 extern LWRP_EXPORT pL_CONTAINERSETAUTOMATIONCALLBACK pL_ContainerSetAutomationCallback; 
 extern LWRP_EXPORT pL_SCREENTOCONTAINER  pL_ScreenToContainer; 
 extern LWRP_EXPORT pL_CONTAINERTOSCREEN  pL_ContainerToScreen; 
 extern LWRP_EXPORT pL_CONTAINERENABLEUPDATE pL_ContainerEnableUpdate; 

//-----------------------------------------------------------------------------
//--LTTLB.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
// general toolbar operations funtions.
extern LWRP_EXPORT pL_TBISVALID  pL_TBIsValid;
extern LWRP_EXPORT pL_TBINIT  pL_TBInit;
extern LWRP_EXPORT pL_TBFREE  pL_TBFree;
extern LWRP_EXPORT pL_TBCREATE   pL_TBCreate;
extern LWRP_EXPORT pL_TBFREETOOLBARINFO   pL_TBFreeToolbarInfo;
// status query functions.
extern LWRP_EXPORT pL_TBISVISIBLE   pL_TBIsVisible;
extern LWRP_EXPORT pL_TBISBUTTONENABLED   pL_TBIsButtonEnabled;
extern LWRP_EXPORT pL_TBISBUTTONVISIBLE   pL_TBIsButtonVisible;
// setting functions.
extern LWRP_EXPORT pL_TBSETVISIBLE  pL_TBSetVisible;
extern LWRP_EXPORT pL_TBSETPOSITION pL_TBSetPosition;
extern LWRP_EXPORT pL_TBSETROWS  pL_TBSetRows;
extern LWRP_EXPORT pL_TBSETBUTTONCHECKED  pL_TBSetButtonChecked;
extern LWRP_EXPORT pL_TBSETBUTTONENABLED  pL_TBSetButtonEnabled;
extern LWRP_EXPORT pL_TBSETBUTTONVISIBLE  pL_TBSetButtonVisible;
extern LWRP_EXPORT pL_TBSETTOOLBARINFO pL_TBSetToolbarInfo;
extern LWRP_EXPORT pL_TBSETCALLBACK pL_TBSetCallback;
// getting functions.
extern LWRP_EXPORT pL_TBGETPOSITION pL_TBGetPosition;
extern LWRP_EXPORT pL_TBGETROWS  pL_TBGetRows;
extern LWRP_EXPORT pL_TBGETBUTTONCHECKED  pL_TBGetButtonChecked;
extern LWRP_EXPORT pL_TBGETTOOLBARINFO pL_TBGetToolbarInfo;
extern LWRP_EXPORT pL_TBGETCALLBACK pL_TBGetCallback;
// new functions
extern LWRP_EXPORT pL_TBADDBUTTON   pL_TBAddButton;
extern LWRP_EXPORT pL_TBREMOVEBUTTON   pL_TBRemoveButton;
extern LWRP_EXPORT pL_TBGETBUTTONINFO  pL_TBGetButtonInfo;
extern LWRP_EXPORT pL_TBSETBUTTONINFO  pL_TBSetButtonInfo;
extern LWRP_EXPORT pL_TBSETAUTOMATIONCALLBACK   pL_TBSetAutomationCallback;//-----------------------------------------------------------------------------
//--LTPNT.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_PNTISVALID pL_PntIsValid;   
extern LWRP_EXPORT pL_PNTINIT pL_PntInit;   
extern LWRP_EXPORT pL_PNTFREE pL_PntFree;   
extern LWRP_EXPORT pL_PNTSETPROPERTY   pL_PntSetProperty;   
extern LWRP_EXPORT pL_PNTGETPROPERTY   pL_PntGetProperty;   
extern LWRP_EXPORT pL_PNTSETMETRICS pL_PntSetMetrics;   
extern LWRP_EXPORT pL_PNTSETTRANSFORMATION   pL_PntSetTransformation;   
extern LWRP_EXPORT pL_PNTGETTRANSFORMATION   pL_PntGetTransformation;   
extern LWRP_EXPORT pL_PNTSETDCEXTENTS  pL_PntSetDCExtents;   
extern LWRP_EXPORT pL_PNTGETDCEXTENTS  pL_PntGetDCExtents;   
extern LWRP_EXPORT pL_PNTSETCLIPRGN pL_PntSetClipRgn;   
extern LWRP_EXPORT pL_PNTGETCLIPRGN pL_PntGetClipRgn;   
extern LWRP_EXPORT pL_PNTOFFSETCLIPRGN pL_PntOffsetClipRgn;   
// brush fucntions.
extern LWRP_EXPORT pL_PNTBRUSHMOVETO   pL_PntBrushMoveTo;   
extern LWRP_EXPORT pL_PNTBRUSHLINETO   pL_PntBrushLineTo;   
// shape functions.
extern LWRP_EXPORT pL_PNTDRAWSHAPELINE pL_PntDrawShapeLine;
extern LWRP_EXPORT pL_PNTDRAWSHAPERECTANGLE  pL_PntDrawShapeRectangle;   
extern LWRP_EXPORT pL_PNTDRAWSHAPEROUNDRECT  pL_PntDrawShapeRoundRect;   
extern LWRP_EXPORT pL_PNTDRAWSHAPEELLIPSE pL_PntDrawShapeEllipse;   
extern LWRP_EXPORT pL_PNTDRAWSHAPEPOLYGON pL_PntDrawShapePolygon;   
extern LWRP_EXPORT pL_PNTDRAWSHAPEPOLYBEZIER pL_PntDrawShapePolyBezier;   
// region functions.
extern LWRP_EXPORT pL_PNTREGIONRECT pL_PntRegionRect;   
extern LWRP_EXPORT pL_PNTREGIONROUNDRECT  pL_PntRegionRoundRect;   
extern LWRP_EXPORT pL_PNTREGIONELLIPSE pL_PntRegionEllipse;   
extern LWRP_EXPORT pL_PNTREGIONPOLYGON pL_PntRegionPolygon;   
extern LWRP_EXPORT pL_PNTREGIONSURFACE pL_PntRegionSurface;   
extern LWRP_EXPORT pL_PNTREGIONBORDER  pL_PntRegionBorder;   
extern LWRP_EXPORT pL_PNTREGIONCOLOR   pL_PntRegionColor;   
extern LWRP_EXPORT pL_PNTREGIONTRANSLATE  pL_PntRegionTranslate;   
extern LWRP_EXPORT pL_PNTREGIONSCALE   pL_PntRegionScale;   
// fill functions.
extern LWRP_EXPORT pL_PNTFILLSURFACE   pL_PntFillSurface;
extern LWRP_EXPORT pL_PNTFILLBORDER pL_PntFillBorder;
extern LWRP_EXPORT pL_PNTFILLCOLORREPLACE pL_PntFillColorReplace;   
// text functions.
extern LWRP_EXPORT pL_PNTAPPLYTEXT  pL_PntApplyText;   
// paint helping functions.
extern LWRP_EXPORT pL_PNTPICKCOLOR  pL_PntPickColor;   
extern LWRP_EXPORT pL_PNTSETCALLBACK   pL_PntSetCallback;
extern LWRP_EXPORT pL_PNTUPDATELEADDC  pL_PntUpdateLeadDC;

//-----------------------------------------------------------------------------
//--LTPDG.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_PNTDLGBRUSH  pL_PntDlgBrush;
extern LWRP_EXPORT pL_PNTDLGSHAPE  pL_PntDlgShape;
extern LWRP_EXPORT pL_PNTDLGREGION pL_PntDlgRegion;
extern LWRP_EXPORT pL_PNTDLGFILL   pL_PntDlgFill;
extern LWRP_EXPORT pL_PNTDLGTEXT   pL_PntDlgText;

//-----------------------------------------------------------------------------
//--LTSGM.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
// Initialize segmentation handle
extern LWRP_EXPORT pL_MRCSTARTBITMAPSEGMENTATION   pL_MrcStartBitmapSegmentation;
// Free segmentation handle
extern LWRP_EXPORT pL_MRCSTOPBITMAPSEGMENTATION pL_MrcStopBitmapSegmentation;
// Break a bitmap into segments in other way
extern LWRP_EXPORT pL_MRCSEGMENTBITMAP pL_MrcSegmentBitmap;
// Set a new segment in the segmentation handle 
extern LWRP_EXPORT pL_MRCCREATENEWSEGMENT pL_MrcCreateNewSegment;
// Get all segments stored inside a segmentation handle
extern LWRP_EXPORT pL_MRCENUMSEGMENTS  pL_MrcEnumSegments;
// Update a certain segment in the segmentation handle 
extern LWRP_EXPORT pL_MRCSETSEGMENTDATA   pL_MrcSetSegmentData;
extern LWRP_EXPORT pL_MRCDELETESEGMENT pL_MrcDeleteSegment;
extern LWRP_EXPORT pL_MRCCOMBINESEGMENTS  pL_MrcCombineSegments;
//------------Copy segmentation handle ---------------------------------------------//
extern LWRP_EXPORT pL_MRCCOPYSEGMENTATIONHANDLE pL_MrcCopySegmentationHandle;
//------------Save a file as MRC ---------------------------------------------------//
extern LWRP_EXPORT pL_MRCSAVEBITMAP pL_MrcSaveBitmap;
extern LWRP_EXPORT pL_MRCSAVEBITMAPT44 pL_MrcSaveBitmapT44;
//------------Load an MRC file -----------------------------------------------------//
extern LWRP_EXPORT pL_MRCLOADBITMAP pL_MrcLoadBitmap;
extern LWRP_EXPORT pL_MRCGETPAGESCOUNT pL_MrcGetPagesCount;
//------------Load/Save segments to/from files -------------------------------------//
extern LWRP_EXPORT pL_MRCSAVESEGMENTATION pL_MrcSaveSegmentation;
extern LWRP_EXPORT pL_MRCLOADSEGMENTATION pL_MrcLoadSegmentation;
extern LWRP_EXPORT pL_MRCSAVEBITMAPLIST   pL_MrcSaveBitmapList;

//-----------------------------------------------------------------------------
//--LTZMV.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_WINDOWHASZOOMVIEW   pL_WindowHasZoomView;
extern LWRP_EXPORT pL_CREATEZOOMVIEW   pL_CreateZoomView;
extern LWRP_EXPORT pL_GETZOOMVIEWPROPS pL_GetZoomViewProps;
extern LWRP_EXPORT pL_UPDATEZOOMVIEW   pL_UpdateZoomView;
extern LWRP_EXPORT pL_DESTROYZOOMVIEW  pL_DestroyZoomView;
extern LWRP_EXPORT pL_GETZOOMVIEWSCOUNT   pL_GetZoomViewsCount;
extern LWRP_EXPORT pL_RENDERZOOMVIEW   pL_RenderZoomView;
extern LWRP_EXPORT pL_STARTZOOMVIEWANNEDIT   pL_StartZoomViewAnnEdit;
extern LWRP_EXPORT pL_STOPZOOMVIEWANNEDIT pL_StopZoomViewAnnEdit;

//-----------------------------------------------------------------------------
//--LTIMGOPT.H FUNCTIONS POINTERS---------------------------------------------
//-----------------------------------------------------------------------------

extern LWRP_EXPORT pL_OPTGETDEFAULTOPTIONS   pL_OptGetDefaultOptions;
extern LWRP_EXPORT pL_OPTOPTIMIZEBUFFER   pL_OptOptimizeBuffer;
extern LWRP_EXPORT pL_OPTOPTIMIZEDIR   pL_OptOptimizeDir;

//-----------------------------------------------------------------------------
//--LPDFComp.H FUNCTIONS POINTERS---------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_PDFCOMPINIT   pL_PdfCompInit;
extern LWRP_EXPORT pL_PDFCOMPFREE   pL_PdfCompFree;
extern LWRP_EXPORT pL_PDFCOMPWRITE  pL_PdfCompWrite;
extern LWRP_EXPORT pL_PDFCOMPSETCOMPRESSION  pL_PdfCompSetCompression;
extern LWRP_EXPORT pL_PDFCOMPINSERTMRC pL_PdfCompInsertMRC;
extern LWRP_EXPORT pL_PDFCOMPINSERTNORMAL pL_PdfCompInsertNormal;
extern LWRP_EXPORT pL_PDFCOMPINSERTSEGMENTS  pL_PdfCompInsertSegments;

//--LTIVW.H FUNCTIONS POINTERS   ---------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_DISPCONTAINERCREATE  pL_DispContainerCreate;
extern LWRP_EXPORT pL_DISPCONTAINERGETWINDOWHANDLE pL_DispContainerGetWindowHandle;
extern LWRP_EXPORT pL_DISPCONTAINERDESTROY pL_DispContainerDestroy;
extern LWRP_EXPORT pL_DISPCONTAINERSETPROPERTIES  pL_DispContainerSetProperties ;
extern LWRP_EXPORT pL_DISPCONTAINERGETPROPERTIES pL_DispContainerGetProperties;
extern LWRP_EXPORT pL_DISPCONTAINERINSERTCELL pL_DispContainerInsertCell;
extern LWRP_EXPORT pL_DISPCONTAINERREMOVECELL pL_DispContainerRemoveCell;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLCOUNT pL_DispContainerGetCellCount;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLWINDOWHANDLE pL_DispContainerGetCellWindowHandle;
extern LWRP_EXPORT pL_DISPCONTAINERSETCELLBITMAPLIST pL_DispContainerSetCellBitmapList;
extern LWRP_EXPORT pL_DISPCONTAINERADDACTION pL_DispContainerAddAction;
extern LWRP_EXPORT pL_DISPCONTAINERSETACTION pL_DispContainerSetAction;
extern LWRP_EXPORT pL_DISPCONTAINERSETCELLTAG pL_DispContainerSetCellTag;
extern LWRP_EXPORT pL_DISPCONTAINERSETCELLPROPERTIES pL_DispContainerSetCellProperties;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLPROPERTIES pL_DispContainerGetCellProperties;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLPOSITION pL_DispContainerGetCellPosition;
extern LWRP_EXPORT pL_DISPCONTAINERREPOSITIONCELL pL_DispContainerRepositionCell;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLBITMAPLIST pL_DispContainerGetCellBitmapList;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLBOUNDS pL_DispContainerGetCellBounds;
extern LWRP_EXPORT pL_DISPCONTAINERFREEZECELL pL_DispContainerFreezeCell;
extern LWRP_EXPORT pL_DISPCONTAINERSETFIRSTVISIBLEROW pL_DispContainerSetFirstVisibleRow;
extern LWRP_EXPORT pL_DISPCONTAINERGETFIRSTVISIBLEROW pL_DispContainerGetFirstVisibleRow;
extern LWRP_EXPORT pL_DISPCONTAINERSETACTIONPROPERTIES pL_DispContainerSetActionProperties;
extern LWRP_EXPORT pL_DISPCONTAINERGETACTIONPROPERTIES pL_DispContainerGetActionProperties;
extern LWRP_EXPORT pL_DISPCONTAINERREMOVEACTION pL_DispContainerRemoveAction;
extern LWRP_EXPORT pL_DISPCONTAINERGETACTIONCOUNT pL_DispContainerGetActionCount;
extern LWRP_EXPORT pL_DISPCONTAINERSETKEYBOARDACTION pL_DispContainerSetKeyboardAction;
extern LWRP_EXPORT pL_DISPCONTAINERSETBOUNDS pL_DispContainerSetBounds;
extern LWRP_EXPORT pL_DISPCONTAINERGETBOUNDS pL_DispContainerGetBounds;
extern LWRP_EXPORT pL_DISPCONTAINERSELECTCELL pL_DispContainerSelectCell;
extern LWRP_EXPORT pL_DISPCONTAINERISCELLSELECTED pL_DispContainerIsCellSelected;
extern LWRP_EXPORT pL_DISPCONTAINERGETHANDLE pL_DispContainerGetHandle;
extern LWRP_EXPORT pL_DISPCONTAINERGETKEYBOARDACTION pL_DispContainerGetKeyboardAction;
extern LWRP_EXPORT pL_DISPCONTAINERISCELLFROZEN pL_DispContainerIsCellFrozen;
extern LWRP_EXPORT pL_DISPCONTAINERISACTIONACTIVE pL_DispContainerIsActionActive;
extern LWRP_EXPORT pL_DISPCONTAINERSETTAGCALLBACK pL_DispContainerSetTagCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETACTIONCALLBACK pL_DispContainerSetActionCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETTAGCALLBACK pL_DispContainerGetTagCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETACTIONCALLBACK pL_DispContainerGetActionCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETPAINTCALLBACK   pL_DispContainerSetPaintCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETPAINTCALLBACK   pL_DispContainerGetPaintCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERREPAINTCELL  pL_DispContainerRepaintCell;
extern LWRP_EXPORT pL_DISPCONTAINERGETBITMAPHANDLE pL_DispContainerGetBitmapHandle;
extern LWRP_EXPORT pL_DISPCONTAINERSETBITMAPHANDLE pL_DispContainerSetBitmapHandle;
extern LWRP_EXPORT pL_DISPCONTAINERSETRULERUNIT pL_DispContainerSetRulerUnit;
extern LWRP_EXPORT pL_DISPCONTAINERCALIBRATERULER  pL_DispContainerCalibrateRuler;
extern LWRP_EXPORT pL_DISPCONTAINERGETACTIONBUTTON pL_DispContainerGetActionButton;
extern LWRP_EXPORT pL_DISPCONTAINERANNTORGN  pL_DispContainerAnnToRgn;
extern LWRP_EXPORT pL_DISPCONTAINERISBUTTONVALID   pL_DispContainerIsButtonValid;
extern LWRP_EXPORT pL_DISPCONTAINERSTARTANIMATION  pL_DispContainerStartAnimation;
extern LWRP_EXPORT pL_DISPCONTAINERSETANIMATIONPROPERTIES   pL_DispContainerSetAnimationProperties;
extern LWRP_EXPORT pL_DISPCONTAINERGETANIMATIONPROPERTIES   pL_DispContainerGetAnimationProperties;
extern LWRP_EXPORT pL_DISPCONTAINERSTOPANIMATION   pL_DispContainerStopAnimation;
extern LWRP_EXPORT pL_DISPCONTAINERSHOWTITLEBAR pL_DispContainerShowTitlebar;
extern LWRP_EXPORT pL_DISPCONTAINERSETTITLEBARPROPERTIES pL_DispContainerSetTitlebarProperties;
extern LWRP_EXPORT pL_DISPCONTAINERGETTITLEBARPROPERTIES pL_DispContainerGetTitlebarProperties;
extern LWRP_EXPORT pL_DISPCONTAINERSETICONPROPERTIES  pL_DispContainerSetIconProperties;
extern LWRP_EXPORT pL_DISPCONTAINERGETICONPROPERTIES  pL_DispContainerGetIconProperties;
extern LWRP_EXPORT pL_DISPCONTAINERCHECKTITLEBARICON  pL_DispContainerCheckTitlebarIcon;
extern LWRP_EXPORT pL_DISPCONTAINERISTITLEBARICONCHECKED  pL_DispContainerIsTitlebarIconChecked;
extern LWRP_EXPORT pL_DISPCONTAINERISCELLANIMATED  pL_DispContainerIsCellAnimated;
extern LWRP_EXPORT pL_DISPCONTAINERGETRULERUNIT pL_DispContainerGetRulerUnit;
extern LWRP_EXPORT pL_DISPCONTAINERISTITLEBARENABLED  pL_DispContainerIsTitlebarEnabled;
extern LWRP_EXPORT pL_DISPCONTAINERGETSELECTEDANNOTATIONATTRIBUTES  pL_DispContainerGetSelectedAnnotationAttributes;
//New Medical Viewer Functionalities
extern LWRP_EXPORT pL_DISPCONTAINERROTATEANNOTATIONCONTAINER pL_DispContainerRotateAnnotationContainer;
extern LWRP_EXPORT pL_DISPCONTAINERREVERSEANNOTATIONCONTAINER pL_DispContainerReverseAnnotationContainer;
extern LWRP_EXPORT pL_DISPCONTAINERFLIPANNOTATIONCONTAINER pL_DispContainerFlipAnnotationContainer;
extern LWRP_EXPORT pL_DISPCONTAINERSETANNOTATIONCALLBACK pL_DispContainerSetAnnotationCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETANNOTATIONCALLBACK pL_DispContainerGetAnnotationCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETREGIONCALLBACK pL_DispContainerSetRegionCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETREGIONCALLBACK pL_DispContainerGetRegionCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETANNOTATIONCREATEDCALLBACK pL_DispContainerSetAnnotationCreatedCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETANNOTATIONCREATEDCALLBACK pL_DispContainerGetAnnotationCreatedCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETACTIVESUBCELLCHANGEDCALLBACK pL_DispContainerSetActiveSubCellChangedCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETACTIVESUBCELLCHANGEDCALLBACK pL_DispContainerGetActiveSubCellChangedCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSAVEANNOTATION pL_DispContainerSaveAnnotation;
extern LWRP_EXPORT pL_DISPCONTAINERLOADANNOTATION pL_DispContainerLoadAnnotation;
extern LWRP_EXPORT pL_DISPCONTAINERGETBITMAPPIXEL pL_DispContainerGetBitmapPixel;
extern LWRP_EXPORT pL_DISPCONTAINERLOADREGION pL_DispContainerLoadRegion;
extern LWRP_EXPORT pL_DISPCONTAINERSAVEREGION pL_DispContainerSaveRegion;
extern LWRP_EXPORT pL_DISPCONTAINERSETCELLREGIONHANDLE pL_DispContainerSetCellRegionHandle;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLREGIONHANDLE pL_DispContainerGetCellRegionHandle;
extern LWRP_EXPORT pL_DISPCONTAINERGETANNOTATIONCONTAINER pL_DispContainerGetAnnotationContainer;
extern LWRP_EXPORT pL_DISPCONTAINERSETANNOTATIONCONTAINER pL_DispContainerSetAnnotationContainer;
extern LWRP_EXPORT pL_DISPCONTAINERSETMOUSECALLBACK pL_DispContainerSetMouseCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETMOUSECALLBACK pL_DispContainerGetMouseCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETSUBCELLTAG pL_DispContainerSetSubCellTag;
extern LWRP_EXPORT pL_DISPCONTAINERGETSUBCELLTAG pL_DispContainerGetSubCellTag;
extern LWRP_EXPORT pL_DISPCONTAINEREDITSUBCELLTAG pL_DispContainerEditSubCellTag;
extern LWRP_EXPORT pL_DISPCONTAINERDELETESUBCELLTAG pL_DispContainerDeleteSubCellTag;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLTAG pL_DispContainerGetCellTag;
extern LWRP_EXPORT pL_DISPCONTAINERDELETECELLTAG pL_DispContainerDeleteCellTag;
extern LWRP_EXPORT pL_DISPCONTAINEREDITCELLTAG pL_DispContainerEditCellTag;
extern LWRP_EXPORT pL_DISPCONTAINERPRINTCELL pL_DispContainerPrintCell;
extern LWRP_EXPORT pL_DISPCONTAINERSETPREPAINTCALLBACK pL_DispContainerSetPrePaintCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETPREPAINTCALLBACK pL_DispContainerGetPrePaintCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETPOSTPAINTCALLBACK pL_DispContainerSetPostPaintCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETPOSTPAINTCALLBACK pL_DispContainerGetPostPaintCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERHANDLEPALETTE pL_DispContainerHandlePalette;

//More Medical Viewer Functionalities
#if defined(LEADTOOLS_V16_OR_LATER)
extern LWRP_EXPORT pL_DISPCONTAINERUPDATECELLVIEW pL_DispContainerUpdateCellView;
extern LWRP_EXPORT pL_DISPCONTAINERENABLECELLLOWMEMORYUSAGE pL_DispContainerEnableCellLowMemoryUsage;
extern LWRP_EXPORT pL_DISPCONTAINERGETLOWMEMORYUSAGECALLBACK pL_DispContainerGetLowMemoryUsageCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETLOWMEMORYUSAGECALLBACK pL_DispContainerSetLowMemoryUsageCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETREQUESTEDIMAGE pL_DispContainerSetRequestedImage;
extern LWRP_EXPORT pL_DISPCONTAINERFREEZESUBCELL pL_DispContainerFreezeSubCell;
extern LWRP_EXPORT pL_DISPCONTAINERISSUBCELLFROZEN pL_DispContainerIsSubCellFrozen;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLSCALEMODE pL_DispContainerGetCellScaleMode;
extern LWRP_EXPORT pL_DISPCONTAINERSETCELLSCALE pL_DispContainerSetCellScale;
extern LWRP_EXPORT pL_DISPCONTAINERSETCELLSCALEMODE pL_DispContainerSetCellScaleMode;
extern LWRP_EXPORT pL_DISPCONTAINERGETCELLSCALE pL_DispContainerGetCellScale;
extern LWRP_EXPORT pL_DISPCONTAINERPRINTSUBCELL pL_DispContainerPrintSubCell;
extern LWRP_EXPORT pL_DISPCONTAINERCALIBRATECELL pL_DispContainerCalibrateCell;
extern LWRP_EXPORT pL_DISPCONTAINERSETBITMAPLISTINFO pL_DispContainerSetBitmapListInfo;
extern LWRP_EXPORT pL_DISPCONTAINERSETANIMATIONSTARTEDCALLBACK pL_DispContainerSetAnimationStartedCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETANIMATIONSTOPPEDCALLBACK pL_DispContainerSetAnimationStoppedCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETANIMATIONSTARTEDCALLBACK pL_DispContainerGetAnimationStartedCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERGETANIMATIONSTOPPEDCALLBACK pL_DispContainerGetAnimationStoppedCallBack;
extern LWRP_EXPORT pL_DISPCONTAINERSETDEFAULTWINDOWLEVELVALUES pL_DispContainerSetDefaultWindowLevelValues;
extern LWRP_EXPORT pL_DISPCONTAINERGETDEFAULTWINDOWLEVELVALUES pL_DispContainerGetDefaultWindowLevelValues;
extern LWRP_EXPORT pL_DISPCONTAINERRESETWINDOWLEVELVALUES pL_DispContainerResetWindowLevelValues;
extern LWRP_EXPORT pL_DISPCONTAINERINVERTBITMAP pL_DispContainerInvertBitmap;
extern LWRP_EXPORT pL_DISPCONTAINERREVERSEBITMAP pL_DispContainerReverseBitmap;
extern LWRP_EXPORT pL_DISPCONTAINERFLIPBITMAP pL_DispContainerFlipBitmap;
extern LWRP_EXPORT pL_DISPCONTAINERISBITMAPREVERSED pL_DispContainerIsBitmapReversed;
extern LWRP_EXPORT pL_DISPCONTAINERISBITMAPFLIPPED pL_DispContainerIsBitmapFlipped;
extern LWRP_EXPORT pL_DISPCONTAINERISBITMAPINVERTED pL_DispContainerIsBitmapInverted;
extern LWRP_EXPORT pL_DISPCONTAINERROTATEBITMAPPERSPECTIVE pL_DispContainerRotateBitmapPerspective;
extern LWRP_EXPORT pL_DISPCONTAINERGETROTATEBITMAPPERSPECTIVEANGLE pL_DispContainerGetRotateBitmapPerspectiveAngle;
extern LWRP_EXPORT pL_DISPCONTAINERREMOVEBITMAPREGION pL_DispContainerRemoveBitmapRegion;
extern LWRP_EXPORT pL_DISPCONTAINERBEGINUPDATE pL_DispContainerBeginUpdate;
extern LWRP_EXPORT pL_DISPCONTAINERENDUPDATE pL_DispContainerEndUpdate;
#endif //(LEADTOOLS_V16_OR_LATER)
//-----------------------------------------------------------------------------
//--LTCLR.H FUNCTIONS POINTERS-----------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_GETPARAMETRICCURVENUMBEROFPARAMETERS  pL_GetParametricCurveNumberOfParameters;
extern LWRP_EXPORT pL_CLRINIT pL_ClrInit;
extern LWRP_EXPORT pL_CLRSETCONVERSIONPARAMS pL_ClrSetConversionParams;
extern LWRP_EXPORT pL_CLRCONVERTDIRECT pL_ClrConvertDirect;
extern LWRP_EXPORT pL_CLRCONVERTDIRECTTOBITMAP  pL_ClrConvertDirectToBitmap;
extern LWRP_EXPORT pL_CLRCONVERT pL_ClrConvert;
extern LWRP_EXPORT pL_CLRCONVERTTOBITMAP  pL_ClrConvertToBitmap;
extern LWRP_EXPORT pL_CLRFREE pL_ClrFree;
extern LWRP_EXPORT pL_CLRISVALID pL_ClrIsValid;
extern LWRP_EXPORT pL_CLRDLG  pL_ClrDlg;
extern LWRP_EXPORT pL_FILLICCPROFILESTRUCTURE   pL_FillICCProfileStructure;
extern LWRP_EXPORT pL_FILLICCPROFILEFROMICCFILE pL_FillICCProfileFromICCFile;
extern LWRP_EXPORT pL_INITICCPROFILE pL_InitICCProfile;
extern LWRP_EXPORT pL_FREEICCPROFILE pL_FreeICCProfile;
extern LWRP_EXPORT pL_INITICCHEADER pL_InitICCHeader;
extern LWRP_EXPORT pL_SETICCCMMTYPE pL_SetICCCMMType;
extern LWRP_EXPORT pL_SETICCDEVICECLASS   pL_SetICCDeviceClass;
extern LWRP_EXPORT pL_SETICCCOLORSPACE pL_SetICCColorSpace;
extern LWRP_EXPORT pL_SETICCCONNECTIONSPACE  pL_SetICCConnectionSpace;
extern LWRP_EXPORT pL_SETICCPRIMARYPLATFORM  pL_SetICCPrimaryPlatform;
extern LWRP_EXPORT pL_SETICCFLAGS   pL_SetICCFlags;
extern LWRP_EXPORT pL_SETICCDEVMANUFACTURER  pL_SetICCDevManufacturer;
extern LWRP_EXPORT pL_SETICCDEVMODEL   pL_SetICCDevModel;
extern LWRP_EXPORT pL_SETICCDEVICEATTRIBUTES pL_SetICCDeviceAttributes;
extern LWRP_EXPORT pL_SETICCRENDERINGINTENT  pL_SetICCRenderingIntent;
extern LWRP_EXPORT pL_SETICCCREATOR pL_SetICCCreator;
extern LWRP_EXPORT pL_SETICCDATETIME   pL_SetICCDateTime;
extern LWRP_EXPORT pL_SETICCTAGDATA pL_SetICCTagData;
extern LWRP_EXPORT pL_GETICCTAGDATA pL_GetICCTagData;
extern LWRP_EXPORT pL_CREATEICCTAGDATA pL_CreateICCTagData;
extern LWRP_EXPORT pL_DELETEICCTAG  pL_DeleteICCTag;
extern LWRP_EXPORT pL_GENERATEICCFILE  pL_GenerateICCFile;
extern LWRP_EXPORT pL_DOUBLETO2BFIXED2BNUMBER   pL_DoubleTo2bFixed2bNumber;
extern LWRP_EXPORT pL_2BFIXED2BNUMBERTODOUBLE   pL_2bFixed2bNumberToDouble;
extern LWRP_EXPORT pL_DOUBLETOU8FIXED8NUMBER pL_DoubleToU8Fixed8Number;
extern LWRP_EXPORT pL_U8FIXED8NUMBERTODOUBLE pL_U8Fixed8NumberToDouble;
extern LWRP_EXPORT pL_GENERATEICCPOINTER  pL_GenerateICCPointer;
extern LWRP_EXPORT pL_GETICCTAGTYPESIG pL_GetICCTagTypeSig;
extern LWRP_EXPORT pL_FREEICCTAGTYPE   pL_FreeICCTagType;
extern LWRP_EXPORT pL_CONVERTCLUTTOBUFFER pL_ConvertCLUTToBuffer;
extern LWRP_EXPORT pL_CONVERTCURVETYPETOBUFFER  pL_ConvertCurveTypeToBuffer;
extern LWRP_EXPORT pL_CONVERTPARAMETRICCURVETYPETOBUFFER pL_ConvertParametricCurveTypeToBuffer;
extern LWRP_EXPORT pL_SETICCPROFILEID  pL_SetICCProfileId;
extern LWRP_EXPORT pL_LOADICCPROFILE   pL_LoadICCProfile;
extern LWRP_EXPORT pL_SAVEICCPROFILE   pL_SaveICCProfile;

//-----------------------------------------------------------------------------
//--LTNTF.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
extern LWRP_EXPORT pL_NITFCREATE pL_NITFCreate;
extern LWRP_EXPORT pL_NITFDESTROY   pL_NITFDestroy;
extern LWRP_EXPORT pL_NITFGETSTATUS    pL_NITFGetStatus ;
extern LWRP_EXPORT pL_NITFSAVEFILE  pL_NITFSaveFile;
extern LWRP_EXPORT pL_NITFAPPENDIMAGESEGMENT pL_NITFAppendImageSegment;
extern LWRP_EXPORT pL_NITFAPPENDGRAPHICSEGMENT  pL_NITFAppendGraphicSegment;
extern LWRP_EXPORT pL_NITFAPPENDTEXTSEGMENT  pL_NITFAppendTextSegment;
extern LWRP_EXPORT pL_NITFSETVECTORHANDLE pL_NITFSetVectorHandle;
extern LWRP_EXPORT pL_NITFGETVECTORHANDLE pL_NITFGetVectorHandle;
extern LWRP_EXPORT pL_NITFGETNITFHEADER   pL_NITFGetNITFHeader ;
extern LWRP_EXPORT pL_NITFSETNITFHEADER   pL_NITFSetNITFHeader ;
extern LWRP_EXPORT pL_NITFGETGRAPHICHEADERCOUNT pL_NITFGetGraphicHeaderCount;
extern LWRP_EXPORT pL_NITFGETGRAPHICHEADER   pL_NITFGetGraphicHeader ;
extern LWRP_EXPORT pL_NITFSETGRAPHICHEADER   pL_NITFSetGraphicHeader ;
extern LWRP_EXPORT pL_NITFGETIMAGEHEADERCOUNT   pL_NITFGetImageHeaderCount;
extern LWRP_EXPORT pL_NITFGETIMAGEHEADER  pL_NITFGetImageHeader ;
extern LWRP_EXPORT pL_NITFSETIMAGEHEADER  pL_NITFSetImageHeader ;
extern LWRP_EXPORT pL_NITFGETTEXTHEADERCOUNT pL_NITFGetTextHeaderCount;
extern LWRP_EXPORT pL_NITFGETTEXTHEADER   pL_NITFGetTextHeader ;
extern LWRP_EXPORT pL_NITFSETTEXTHEADER   pL_NITFSetTextHeader ;
extern LWRP_EXPORT pL_NITFGETTEXTSEGMENT  pL_NITFGetTextSegment ;
extern LWRP_EXPORT pL_NITFFREENITFHEADER  pL_NITFFreeNITFHeader ;
extern LWRP_EXPORT pL_NITFFREEGRAPHICHEADER    pL_NITFFreeGraphicHeader ;
extern LWRP_EXPORT pL_NITFFREEIMAGEHEADER   pL_NITFFreeImageHeader ;
extern LWRP_EXPORT pL_NITFFREETEXTHEADER    pL_NITFFreeTextHeader ;

//-----------------------------------------------------------------------------
//--LTWIA.H FUNCTIONS POINTERS-------------------------------------------------
//-----------------------------------------------------------------------------
#if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)

extern LWRP_EXPORT pL_WIAISAVAILABLE   pL_WiaIsAvailable ;
extern LWRP_EXPORT pL_WIAINITSESSION   pL_WiaInitSession ;
extern LWRP_EXPORT pL_WIAENDSESSION    pL_WiaEndSession ;
extern LWRP_EXPORT pL_WIAENUMDEVICES   pL_WiaEnumDevices;
extern LWRP_EXPORT pL_WIASELECTDEVICEDLG  pL_WiaSelectDeviceDlg;
extern LWRP_EXPORT pL_WIASELECTDEVICE  pL_WiaSelectDevice;
extern LWRP_EXPORT pL_WIAGETSELECTEDDEVICE   pL_WiaGetSelectedDevice;
extern LWRP_EXPORT pL_WIAGETSELECTEDDEVICETYPE   pL_WiaGetSelectedDeviceType;
extern LWRP_EXPORT pL_WIAGETPROPERTIES pL_WiaGetProperties;
extern LWRP_EXPORT pL_WIASETPROPERTIES pL_WiaSetProperties;
extern LWRP_EXPORT pL_WIAACQUIRE pL_WiaAcquire;
extern LWRP_EXPORT pL_WIAACQUIRETOFILE pL_WiaAcquireToFile;
extern LWRP_EXPORT pL_WIAACQUIRESIMPLE pL_WiaAcquireSimple;
extern LWRP_EXPORT pL_WIASTARTVIDEOPREVIEW   pL_WiaStartVideoPreview;
extern LWRP_EXPORT pL_WIARESIZEVIDEOPREVIEW  pL_WiaResizeVideoPreview;
extern LWRP_EXPORT pL_WIAENDVIDEOPREVIEW  pL_WiaEndVideoPreview;
extern LWRP_EXPORT pL_WIAACQUIREIMAGEFROMVIDEO  pL_WiaAcquireImageFromVideo;
extern LWRP_EXPORT pL_WIAISVIDEOPREVIEWAVAILABLE   pL_WiaIsVideoPreviewAvailable;
extern LWRP_EXPORT pL_WIAGETROOTITEM   pL_WiaGetRootItem;
extern LWRP_EXPORT pL_WIAENUMCHILDITEMS   pL_WiaEnumChildItems;
extern LWRP_EXPORT pL_WIAFREEITEM   pL_WiaFreeItem;
extern LWRP_EXPORT pL_WIAGETPROPERTYLONG  pL_WiaGetPropertyLong;
extern LWRP_EXPORT pL_WIASETPROPERTYLONG  pL_WiaSetPropertyLong;
extern LWRP_EXPORT pL_WIAGETPROPERTYGUID  pL_WiaGetPropertyGUID;
extern LWRP_EXPORT pL_WIASETPROPERTYGUID  pL_WiaSetPropertyGUID;
extern LWRP_EXPORT pL_WIAGETPROPERTYSTRING   pL_WiaGetPropertyString;
extern LWRP_EXPORT pL_WIASETPROPERTYSTRING   pL_WiaSetPropertyString;
extern LWRP_EXPORT pL_WIAGETPROPERTYSYSTEMTIME  pL_WiaGetPropertySystemTime;
extern LWRP_EXPORT pL_WIASETPROPERTYSYSTEMTIME  pL_WiaSetPropertySystemTime;
extern LWRP_EXPORT pL_WIAGETPROPERTYBUFFER   pL_WiaGetPropertyBuffer;
extern LWRP_EXPORT pL_WIASETPROPERTYBUFFER   pL_WiaSetPropertyBuffer;
extern LWRP_EXPORT pL_WIAENUMCAPABILITIES pL_WiaEnumCapabilities;
extern LWRP_EXPORT pL_WIAENUMFORMATS   pL_WiaEnumFormats;

#endif // #if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)
#endif //USE_POINTERS_TO_LEAD_FUNCTIONS
#endif //_LEAD_EXTERN_H_
/*================================================================= EOF =====*/




