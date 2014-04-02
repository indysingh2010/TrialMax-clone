/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright(c) 1991-2007 LEAD Technologies, Inc.                              |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : tcFncID.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_FUNCTIONS_ID_H_
#define  _LEAD_FUNCTIONS_ID_H_

/*----------------------------------------------------------------------------+
| MACROS                                                                      |
+----------------------------------------------------------------------------*/
#ifdef USE_POINTERS_TO_LEAD_FUNCTIONS

//-----------------------------------------------------------------------------
//--LTKRN.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define IDF_L_AccessBitmap                              "L_AccessBitmap"
#define IDF_L_AllocateBitmap                            "L_AllocateBitmap"
#define IDF_L_ChangeBitmapHeight                        "L_ChangeBitmapHeight"
#define IDF_L_ChangeBitmapViewPerspective               "L_ChangeBitmapViewPerspective"
#define IDF_L_ChangeFromDIB                             "L_ChangeFromDIB"
#define IDF_L_ChangeToDIB                               "L_ChangeToDIB"
#define IDF_L_ClearBitmap                               "L_ClearBitmap"
#define IDF_L_ClearNegativePixels                       "L_ClearNegativePixels"
#define IDF_L_ColorResBitmap                            "L_ColorResBitmap"
#define IDF_L_ColorResBitmapList                        "L_ColorResBitmapList"
#define IDF_L_ColorResBitmapListExt                     "L_ColorResBitmapListExt"
#define IDF_L_CompressRow                               "L_CompressRow"
#define IDF_L_CompressRows                              "L_CompressRows"
#define IDF_L_ConvertBuffer                             "L_ConvertBuffer"
#define IDF_L_ConvertBufferExt                          "L_ConvertBufferExt"
#define IDF_L_ConvertFromDIB                            "L_ConvertFromDIB"
#define IDF_L_ConvertToDIB                              "L_ConvertToDIB"
#define IDF_L_CopyBitmap                                "L_CopyBitmap"
#define IDF_L_CopyBitmap2                               "L_CopyBitmap2"
#define IDF_L_CopyBitmapData                            "L_CopyBitmapData"
#define IDF_L_CopyBitmapHandle                          "L_CopyBitmapHandle"
#define IDF_L_CopyBitmapListItems                       "L_CopyBitmapListItems"
#define IDF_L_CopyBitmapRect                            "L_CopyBitmapRect"
#define IDF_L_CopyBitmapRect2                           "L_CopyBitmapRect2"
#define IDF_L_CreateBitmap                              "L_CreateBitmap"
#define IDF_L_CreateBitmapList                          "L_CreateBitmapList"
#define IDF_L_CreateUserMatchTable                      "L_CreateUserMatchTable"
#define IDF_L_DefaultDithering                          "L_DefaultDithering"
#define IDF_L_DeleteBitmapListItems                     "L_DeleteBitmapListItems"
#define IDF_L_DestroyBitmapList                         "L_DestroyBitmapList"
#define IDF_L_DitherLine                                "L_DitherLine"
#define IDF_L_CopyBitmapPalette                         "L_CopyBitmapPalette"
#define IDF_L_DupBitmapPalette                          "L_DupBitmapPalette"
#define IDF_L_DupPalette                                "L_DupPalette"
#define IDF_L_ExpandRow                                 "L_ExpandRow"
#define IDF_L_ExpandRows                                "L_ExpandRows"
#define IDF_L_FillBitmap                                "L_FillBitmap"
#define IDF_L_FlipBitmap                                "L_FlipBitmap"
#define IDF_L_FreeBitmap                                "L_FreeBitmap"
#define IDF_L_FreeUserMatchTable                        "L_FreeUserMatchTable"
#define IDF_L_GetBitmapColors                           "L_GetBitmapColors"
#define IDF_L_GetBitmapListCount                        "L_GetBitmapListCount"
#define IDF_L_GetBitmapListItem                         "L_GetBitmapListItem"
#define IDF_L_GetBitmapRow                              "L_GetBitmapRow"
#define IDF_L_GetBitmapRowCol                           "L_GetBitmapRowCol"
#define IDF_L_GetBitmapRowColCompressed                 "L_GetBitmapRowColCompressed"
#define IDF_L_GetBitmapRowCompressed                    "L_GetBitmapRowCompressed"
#define IDF_L_GetFixedPalette                           "L_GetFixedPalette"
#define IDF_L_GetPixelColor                             "L_GetPixelColor"
#define IDF_L_GetStatusCallBack                         "L_GetStatusCallBack"
#define IDF_L_SetStatusCallBack                         "L_SetStatusCallBack"
#define IDF_L_GetCopyStatusCallBack                     "L_GetCopyStatusCallBack"
#define IDF_L_SetCopyStatusCallBack                     "L_SetCopyStatusCallBack"
#define IDF_L_GrayScaleBitmap                           "L_GrayScaleBitmap"
#define IDF_L_InitBitmap                                "L_InitBitmap"
#define IDF_L_InsertBitmapListItem                      "L_InsertBitmapListItem"
#define IDF_L_IsGrayScaleBitmap                         "L_IsGrayScaleBitmap"
#define IDF_L_IsSupportLocked                           "L_IsSupportLocked"
#define IDF_L_PointFromBitmap                           "L_PointFromBitmap"
#define IDF_L_PointToBitmap                             "L_PointToBitmap"
#define IDF_L_PutBitmapColors                           "L_PutBitmapColors"
#define IDF_L_PutBitmapRow                              "L_PutBitmapRow"
#define IDF_L_PutBitmapRowCol                           "L_PutBitmapRowCol"
#define IDF_L_PutBitmapRowColCompressed                 "L_PutBitmapRowColCompressed"
#define IDF_L_PutBitmapRowCompressed                    "L_PutBitmapRowCompressed"
#define IDF_L_PutPixelColor                             "L_PutPixelColor"
#define IDF_L_RectFromBitmap                            "L_RectFromBitmap"
#define IDF_L_RectToBitmap                              "L_RectToBitmap"
#if defined(FOR_UNICODE)
   #define IDF_L_RedirectIO                                "L_RedirectIO"
#else
   #define IDF_L_RedirectIO                                "L_RedirectIOA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_ReleaseBitmap                             "L_ReleaseBitmap"
#define IDF_L_RemoveBitmapListItem                      "L_RemoveBitmapListItem"
#define IDF_L_Resize                                    "L_Resize"
#define IDF_L_ResizeBitmap                              "L_ResizeBitmap"
#define IDF_L_ReverseBitmap                             "L_ReverseBitmap"
#define IDF_L_RotateBitmap                              "L_RotateBitmap"
#define IDF_L_RotateBitmapViewPerspective               "L_RotateBitmapViewPerspective"
#define IDF_L_SetBitmapDataPointer                      "L_SetBitmapDataPointer"
#define IDF_L_SetBitmapListItem                         "L_SetBitmapListItem"
#define IDF_L_SetUserMatchTable                         "L_SetUserMatchTable"
#define IDF_L_SizeBitmap                                "L_SizeBitmap"
#define IDF_L_StartDithering                            "L_StartDithering"
#define IDF_L_StartResize                               "L_StartResize"
#define IDF_L_StopDithering                             "L_StopDithering"
#define IDF_L_StopResize                                "L_StopResize"
#define IDF_L_TranslateBitmapColor                      "L_TranslateBitmapColor"
#define IDF_L_ToggleBitmapCompression                   "L_ToggleBitmapCompression"
#define IDF_L_TrimBitmap                                "L_TrimBitmap"
#if defined(FOR_UNICODE)
   #define IDF_L_UnlockSupport                             "L_UnlockSupport"
#else
   #define IDF_L_UnlockSupport                             "L_UnlockSupportA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_KernelHasExpired                          "L_KernelHasExpired"
#define IDF_L_FlipBitmapViewPerspective                 "L_FlipBitmapViewPerspective"
#define IDF_L_ReverseBitmapViewPerspective              "L_ReverseBitmapViewPerspective"
#define IDF_L_StartResizeBitmap                         "L_StartResizeBitmap"
#define IDF_L_GetResizedRowCol                          "L_GetResizedRowCol"
#define IDF_L_StopResizeBitmap                          "L_StopResizeBitmap"
#define IDF_L_MoveBitmapListItems                       "L_MoveBitmapListItems"
#define IDF_L_ChangeBitmapCompression                   "L_ChangeBitmapCompression"
#define IDF_L_GetPixelData                              "L_GetPixelData"
#define IDF_L_PutPixelData                              "L_PutPixelData"
#define IDF_L_SetDefaultMemoryType                      "L_SetDefaultMemoryType"
#define IDF_L_GetDefaultMemoryType                      "L_GetDefaultMemoryType"
#define IDF_L_SetMemoryThresholds                       "L_SetMemoryThresholds"
#define IDF_L_GetMemoryThresholds                       "L_GetMemoryThresholds"
#define IDF_L_SetBitmapMemoryInfo                       "L_SetBitmapMemoryInfo"
#define IDF_L_GetBitmapMemoryInfo                       "L_GetBitmapMemoryInfo"
#if defined(FOR_UNICODE)
   #define IDF_L_SetTempDirectory                          "L_SetTempDirectory"
#else
   #define IDF_L_SetTempDirectory                          "L_SetTempDirectoryA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_GetTempDirectory                          "L_GetTempDirectory"
#else
   #define IDF_L_GetTempDirectory                          "L_GetTempDirectoryA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_SetBitmapPalette                          "L_SetBitmapPalette"
#define IDF_L_ScrambleBitmap                            "L_ScrambleBitmap"
#define IDF_L_CombineBitmapWarp                         "L_CombineBitmapWarp"
#define IDF_L_SetOverlayBitmap                          "L_SetOverlayBitmap"
#define IDF_L_GetOverlayBitmap                          "L_GetOverlayBitmap"
#if defined(FOR_UNICODE)
   #define IDF_L_SetOverlayAttributes                      "L_SetOverlayAttributes"
#else
   #define IDF_L_SetOverlayAttributes                      "L_SetOverlayAttributesA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_GetOverlayAttributes                      "L_GetOverlayAttributes"
#else
   #define IDF_L_GetOverlayAttributes                      "L_GetOverlayAttributesA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_UpdateBitmapOverlayBits                   "L_UpdateBitmapOverlayBits"
#define IDF_L_GetOverlayCount                           "L_GetOverlayCount"
#define IDF_L_CreateLeadDC                              "L_CreateLeadDC"
#define IDF_L_DeleteLeadDC                              "L_DeleteLeadDC"
#define IDF_L_GetBitmapAlpha                            "L_GetBitmapAlpha"
#define IDF_L_SetBitmapAlpha                            "L_SetBitmapAlpha"
#define IDF_L_SetBitmapAlphaValues                      "L_SetBitmapAlphaValues"
#define IDF_L_ShearBitmap                               "L_ShearBitmap"
#define IDF_L_VersionInfo                               "L_VersionInfo"
#define IDF_L_HasExpired                                "L_HasExpired"
#define IDF_L_CreateBitmapListOptPal                    "L_CreateBitmapListOptPal"
#define IDF_L_CreateGrayScaleBitmap                     "L_CreateGrayScaleBitmap"
//-----------------------------------------------------------------------------
//--LTIMG.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_ChangeBitmapContrast                      "L_ChangeBitmapContrast"
#define IDF_L_ChangeBitmapHue                           "L_ChangeBitmapHue"
#define IDF_L_ChangeBitmapIntensity                     "L_ChangeBitmapIntensity"
#define IDF_L_ChangeBitmapSaturation                    "L_ChangeBitmapSaturation"
#define IDF_L_InvertBitmap                              "L_InvertBitmap"
#define IDF_L_RemapBitmapIntensity                      "L_RemapBitmapIntensity"
#define IDF_L_SharpenBitmap                             "L_SharpenBitmap"
#define IDF_L_GetBitmapColorCount                       "L_GetBitmapColorCount"
#define IDF_L_GetAutoTrimRect                           "L_GetAutoTrimRect"
#define IDF_L_AutoTrimBitmap                            "L_AutoTrimBitmap"
#define IDF_L_UnsharpMaskBitmap                         "L_UnsharpMaskBitmap"
#define IDF_L_DeskewBitmap                              "L_DeskewBitmap"
#define IDF_L_DeskewCheckBitmap                         "L_DeskewCheckBitmap"
#define IDF_L_DeskewBitmapExt                           "L_DeskewBitmapExt"
#define IDF_L_BlankPageDetectorBitmap                   "L_BlankPageDetectorBitmap"
#define IDF_L_AutoBinarizeBitmap                        "L_AutoBinarizeBitmap"
#define IDF_L_StartFastMagicWandEngine                  "L_StartFastMagicWandEngine"
#define IDF_L_EndFastMagicWandEngine                    "L_EndFastMagicWandEngine"
#define IDF_L_FastMagicWand                             "L_FastMagicWand"
#define IDF_L_DeleteObjectInfo                          "L_DeleteObjectIfno"
#define IDF_L_TissueEqualizeBitmap                      "L_TissueEqualizeBitmap"
#define IDF_L_SigmaFilterBitmap                         "L_SigmaFilterBitmap"
#define IDF_L_AutoSegmentBitmap                         "L_AutoSegmentBitmap"
#define IDF_L_SigmaFilterBitmap                         "L_SigmaFilterBitmap"
#define IDF_L_DespeckleBitmap                           "L_DespeckleBitmap"
#define IDF_L_EdgeDetectorBitmap                        "L_EdgeDetectorBitmap"
#define IDF_L_IntensityDetectBitmap                     "L_IntensityDetectBitmap"
#define IDF_L_AverageFilterBitmap                       "L_AverageFilterBitmap"
#define IDF_L_BinaryFilterBitmap                        "L_BinaryFilterBitmap"
#define IDF_L_CombineBitmap                             "L_CombineBitmap"
#define IDF_L_MinFilterBitmap                           "L_MinFilterBitmap"
#define IDF_L_MaxFilterBitmap                           "L_MaxFilterBitmap"
#define IDF_L_AddBitmapNoise                            "L_AddBitmapNoise"
#define IDF_L_ColorMergeBitmap                          "L_ColorMergeBitmap"
#define IDF_L_ColorSeparateBitmap                       "L_ColorSeparateBitmap"
#define IDF_L_EmbossBitmap                              "L_EmbossBitmap"
#define IDF_L_GammaCorrectBitmap                        "L_GammaCorrectBitmap"
#define IDF_L_GetMinMaxBits                             "L_GetMinMaxBits"
#define IDF_L_GetMinMaxVal                              "L_GetMinMaxVal"
#define IDF_L_HistoContrastBitmap                       "L_HistoContrastBitmap"
#define IDF_L_MedianFilterBitmap                        "L_MedianFilterBitmap"
#define IDF_L_MosaicBitmap                              "L_MosaicBitmap"
#define IDF_L_PosterizeBitmap                           "L_PosterizeBitmap"
#define IDF_L_LineProfile                               "L_LineProfile"
#define IDF_L_GrayScaleBitmapExt                        "L_GrayScaleBitmapExt"
#define IDF_L_SwapColors                                "L_SwapColors"
#define IDF_L_BalanceColors                             "L_BalanceColors"
#define IDF_L_ConvertToColoredGray                      "L_ConvertToColoredGray"
#define IDF_L_HistoEqualizeBitmap                       "L_HistoEqualizeBitmap"
#define IDF_L_AlphaBlendBitmap                          "L_AlphaBlendBitmap"
#define IDF_L_AntiAliasBitmap                           "L_AntiAliasBitmap"
#define IDF_L_GetBitmapHistogram                        "L_GetBitmapHistogram"
#define IDF_L_GetUserLookUpTable                        "L_GetUserLookUpTable"
#define IDF_L_GetFunctionalLookupTable                  "L_GetFunctionalLookupTable"
#define IDF_L_ConvertBitmapSignedToUnsigned             "L_ConvertBitmapSignedToUnsigned"
#define IDF_L_TextureAlphaBlendBitmap                   "L_TextureAlphaBlendBitmap"
#define IDF_L_RemapBitmapHue                            "L_RemapBitmapHue"
#define IDF_L_MultiplyBitmap                            "L_MultiplyBitmap"
#define IDF_L_LocalHistoEqualizeBitmap                  "L_LocalHistoEqualizeBitmap"
#define IDF_L_GetCurvePoints                            "L_GetCurvePoints"
#define IDF_L_SolarizeBitmap                            "L_SolarizeBitmap"
#define IDF_L_SpatialFilterBitmap                       "L_SpatialFilterBitmap"
#define IDF_L_StretchBitmapIntensity                    "L_StretchBitmapIntensity"
#define IDF_L_WindowLevelBitmap                         "L_WindowLevelBitmap"
#define IDF_L_WindowLevelBitmapExt                      "L_WindowLevelBitmapExt"
#define IDF_L_GaussianFilterBitmap                      "L_GaussianFilterBitmap"
#define IDF_L_SmoothBitmap                              "L_SmoothBitmap"
#define IDF_L_LineRemoveBitmap                          "L_LineRemoveBitmap"
#define IDF_L_RakeRemoveBitmap                          "L_RakeRemoveBitmap"
#define IDF_L_ObjectCounterBitmap                          "L_ObjectCounter"
#define IDF_L_BorderRemoveBitmap                        "L_BorderRemoveBitmap"
#define IDF_L_InvertedTextBitmap                        "L_InvertedTextBitmap"
#define IDF_L_DotRemoveBitmap                           "L_DotRemoveBitmap"
#define IDF_L_HolePunchRemoveBitmap                     "L_HolePunchRemoveBitmap"
#define IDF_L_StapleRemoveBitmap                        "L_StapleRemoveBitmap"
#define IDF_L_OilifyBitmap                              "L_OilifyBitmap"
#if defined(FOR_UNICODE)
   #define IDF_L_PicturizeBitmap                           "L_PicturizeBitmap"
#else
   #define IDF_L_PicturizeBitmap                           "L_PicturizeBitmapA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_ContourFilterBitmap                       "L_ContourFilterBitmap"
#define IDF_L_ResizeBitmapRgn                           "L_ResizeBitmapRgn"
#define IDF_L_CreateFadedMask                           "L_CreateFadedMask"
#define IDF_L_AddBitmaps                                "L_AddBitmaps"
#define IDF_L_MotionBlurBitmap                          "L_MotionBlurBitmap"
#define IDF_L_PicturizeBitmapList                       "L_PicturizeBitmapList"
#define IDF_L_PicturizeBitmapSingle                     "L_PicturizeBitmapSingle"
#define IDF_L_AddBorder                                 "L_AddBorder"
#define IDF_L_AddFrame                                  "L_AddFrame"
#define IDF_L_AddWeightedBitmaps                        "L_AddWeightedBitmaps"
#if defined(FOR_UNICODE)
   #define IDF_L_AddMessageToBitmap                        "L_AddMessageToBitmap"
#else
   #define IDF_L_AddMessageToBitmap                        "L_AddMessageToBitmapA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_ExtractMessageFromBitmap                  "L_ExtractMessageFromBitmap"
#else
   #define IDF_L_ExtractMessageFromBitmap                  "L_ExtractMessageFromBitmapA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_PixelateBitmap                            "L_PixelateBitmap"
#define IDF_L_SpherizeBitmap                            "L_SpherizeBitmap"
#define IDF_L_CylindricalBitmap                         "L_CylindricalBitmap"
#define IDF_L_BendingBitmap                             "L_BendingBitmap"
#define IDF_L_PunchBitmap                               "L_PunchBitmap"
#define IDF_L_PolarBitmap                               "L_PolarBitmap"
#define IDF_L_RadialBlurBitmap                          "L_RadialBlurBitmap"
#define IDF_L_RippleBitmap                              "L_RippleBitmap"
#define IDF_L_SwirlBitmap                               "L_SwirlBitmap"
#define IDF_L_ZoomBlurBitmap                            "L_ZoomBlurBitmap"
#define IDF_L_FreeHandWaveBitmap                        "L_FreeHandWaveBitmap"
#define IDF_L_ImpressionistBitmap                       "L_ImpressionistBitmap"
#define IDF_L_RadWaveBitmap                             "L_RadWaveBitmap"
#define IDF_L_FreeHandShearBitmap                       "L_FreeHandShearBitmap"
#define IDF_L_WaveBitmap                                "L_WaveBitmap"
#define IDF_L_WindBitmap                                "L_WindBitmap"
#define IDF_L_ZoomWaveBitmap                            "L_ZoomWaveBitmap"
#define IDF_L_RemoveRedeyeBitmap                        "L_RemoveRedeyeBitmap"
#define IDF_L_DeinterlaceBitmap                         "L_DeinterlaceBitmap"
#define IDF_L_SampleTargetBitmap                        "L_SampleTargetBitmap"
#define IDF_L_HalfToneBitmap                            "L_HalfToneBitmap"
#define IDF_L_HolesRemovalBitmapRgn                     "L_HolesRemovalBitmapRgn"
#define IDF_L_CubismBitmap                              "L_CubismBitmap"
#define IDF_L_LightControlBitmap                        "L_LightControlBitmap"
#define IDF_L_GlassEffectBitmap                         "L_GlassEffectBitmap"
#define IDF_L_LensFlareBitmap                           "L_LensFlareBitmap"
#define IDF_L_BumpMapBitmap                             "L_BumpMapBitmap"
#define IDF_L_GlowFilterBitmap                          "L_GlowFilterBitmap"
#define IDF_L_EdgeDetectStatisticalBitmap               "L_EdgeDetectStatisticalBitmap"
#define IDF_L_DesaturateBitmap                          "L_DesaturateBitmap"
#define IDF_L_SmoothEdgesBitmap                         "L_SmoothEdgesBitmap"
#define IDF_L_AutoBinaryBitmap                          "L_AutoBinaryBitmap"
#define IDF_L_ChannelMix                                "L_ChannelMix"
#define IDF_L_PlaneBitmap                               "L_PlaneBitmap"
#define IDF_L_PlaneBendBitmap                           "L_PlaneBendBitmap"
#define IDF_L_TunnelBitmap                              "L_TunnelBitmap"
#define IDF_L_FreeRadBendBitmap                         "L_FreeRadBendBitmap"
#define IDF_L_FreePlaneBendBitmap                       "L_FreePlaneBendBitmap"
#define IDF_L_OceanBitmap                               "L_OceanBitmap"
#define IDF_L_LightBitmap                               "L_LightBitmap"
#define IDF_L_DryBitmap                                 "L_DryBitmap"
#define IDF_L_DrawStarBitmap                            "L_DrawStarBitmap"
#define IDF_L_FFTBitmap                                 "L_FFTBitmap"
#define IDF_L_FTDisplayBitmap                           "L_FTDisplayBitmap"
#define IDF_L_DFTBitmap                                 "L_DFTBitmap"
#define IDF_L_FrqFilterBitmap                           "L_FrqFilterBitmap"
#define IDF_L_FrqFilterMaskBitmap                       "L_FrqFilterMaskBitmap"
#define IDF_L_AllocFTArray                              "L_AllocFTArray"
#define IDF_L_FreeFTArray                               "L_FreeFTArray"
#define IDF_L_GrayScaleToDuotone                        "L_GrayScaleToDuotone"
#define IDF_L_GrayScaleToMultitone                      "L_GrayScaleToMultitone"
#define IDF_L_SkeletonBitmap                            "L_SkeletonBitmap"
#define IDF_L_ColorLevelBitmap                          "L_ColorLevelBitmap"
#define IDF_L_AutoColorLevelBitmap                      "L_AutoColorLevelBitmap"
#define IDF_L_SelectiveColorBitmap                      "L_SelectiveColorBitmap"
#define IDF_L_CorrelationBitmap                         "L_CorrelationBitmap"
#define IDF_L_GetObjectInfo                             "L_GetObjectInfo"
#define IDF_L_GetRgnContourPoints                       "L_GetRgnContourPoints"
#define IDF_L_SegmentBitmap                             "L_SegmentBitmap"
#define IDF_L_GetRgnPerimeterLength                     "L_GetRgnPerimeterLength"
#define IDF_L_GetFeretsDiameter                         "L_GetFeretsDiameter"
#define IDF_L_ColorReplaceBitmap                        "L_ColorReplaceBitmap"
#define IDF_L_ChangeHueSatIntBitmap                     "L_ChangeHueSatIntBitmap"
#define IDF_L_GetBitmapStatisticsInfo                   "L_GetBitmapStatisticsInfo"
#define IDF_L_UserFilterBitmap                          "L_UserFilterBitmap"
#define IDF_L_ColorReplaceWeightsBitmap                 "L_ColorReplaceWeightsBitmap"
#define IDF_L_DirectionEdgeStatisticalBitmap            "L_DirectionEdgeStatisticalBitmap"
#define IDF_L_MathFunctionBitmap                        "L_MathFunctionBitmap"
#define IDF_L_ColorThresholdBitmap                      "L_ColorThresholdBitmap"
#define IDF_L_RevEffectBitmap                           "L_RevEffectBitmap"
#define IDF_L_AddShadowBitmap                           "L_AddShadowBitmap"
#define IDF_L_SubtractBackgroundBitmap                  "L_SubtractBackgroundBitmap"
#define IDF_L_ApplyModalityLUT                          "L_ApplyModalityLUT"
#define IDF_L_ApplyLinearModalityLUT                    "L_ApplyLinearModalityLUT"
#define IDF_L_ApplyVOILUT                               "L_ApplyVOILUT"
#define IDF_L_ApplyLinearVOILUT                         "L_ApplyLinearVOILUT"
#define IDF_L_GetLinearVOILUT                           "L_GetLinearVOILUT"
#define IDF_L_CountLUTColors                            "L_CountLUTColors"
#define IDF_L_CountLUTColorsExt                         "L_CountLUTColorsExt"
#define IDF_L_AdaptiveContrastBitmap                    "L_AdaptiveContrastBitmap"
#define IDF_L_AgingBitmap                               "L_AgingBitmap"
#define IDF_L_DynamicBinaryBitmap                       "L_DynamicBinaryBitmap"
#define IDF_L_ColorIntensityBalance                     "L_ColorIntensityBalance"
#define IDF_L_ApplyMathLogicBitmap                      "L_ApplyMathLogicBitmap"
#define IDF_L_EdgeDetectEffectBitmap                    "L_EdgeDetectEffectBitmap"
#define IDF_L_FunctionalLightBitmap                     "L_FunctionalLightBitmap"
#define IDF_L_DiceEffectBitmap                          "L_DiceEffectBitmap"
#define IDF_L_PuzzleEffectBitmap                        "L_PuzzleEffectBitmap"
#define IDF_L_RingEffectBitmap                          "L_RingEffectBitmap"
#define IDF_L_MultiScaleEnhancementBitmap               "L_MultiScaleEnhancementBitmap"
#define IDF_L_ShiftBitmapData                           "L_ShiftBitmapData"
#define IDF_L_SelectBitmapData                          "L_SelectBitmapData"
#define IDF_L_ColorizeGrayBitmap                        "L_ColorizeGrayBitmap"
#define IDF_L_DigitalSubtractBitmap                     "L_DigitalSubtractBitmap"
#define IDF_L_ContBrightIntBitmap                       "L_ContBrightIntBitmap"
#define IDF_L_IsRegMarkBitmap                           "L_IsRegMarkBitmap"
#define IDF_L_GetMarksCenterMassBitmap                  "L_GetMarksCenterMassBitmap"
#define IDF_L_SearchRegMarksBitmap                      "L_SearchRegMarksBitmap"
#define IDF_L_GetTransformationParameters               "L_GetTransformationParameters"
#define IDF_L_ApplyTransformationParameters             "L_ApplyTransformationParameters"
#define IDF_L_ConvertBitmapUnsignedToSigned             "L_ConvertBitmapUnsignedToSigned"
#define IDF_L_OffsetBitmap                              "L_OffsetBitmap"
#define IDF_L_BricksTextureBitmap                       "L_BricksTextureBitmap"
#define IDF_L_CanvasBitmap                              "L_CanvasBitmap"
#define IDF_L_FragmentBitmap                            "L_FragmentBitmap"
#define IDF_L_CloudsBitmap                              "L_CloudsBitmap"
#define IDF_L_VignetteBitmap                            "L_VignetteBitmap"
#define IDF_L_MosaicTilesBitmap                         "L_MosaicTilesBitmap"
#define IDF_L_RomanMosaicBitmap                         "L_RomanMosaicBitmap"
#define IDF_L_GammaCorrectBitmapExt                     "L_GammaCorrectBitmapExt"
#define IDF_L_MaskConvolutionBitmap                     "L_MaskConvolutionBitmap"
#define IDF_L_PlasmaFilterBitmap                        "L_PlasmaFilterBitmap"
#define IDF_L_AdjustBitmapTint                          "L_AdjustBitmapTint"
#define IDF_L_ColoredPencilBitmap                       "L_ColoredPencilBitmap"
#define IDF_L_PerlinBitmap                              "L_PerlinBitmap"
#define IDF_L_DiffuseGlowBitmap                         "L_DiffuseGlowBitmap"
#define IDF_L_DisplaceMapBitmap                         "L_DisplaceMapBitmap"
#define IDF_L_HighPassFilterBitmap                      "L_HighPassFilterBitmap"
#define IDF_L_ZigZagBitmap                              "L_ZigZagBitmap"
#define IDF_L_ColorHalfToneBitmap                       "L_ColorHalfToneBitmap"
#define IDF_L_ColoredBallsBitmap                        "L_ColoredBallsBitmap"
#define IDF_L_PerspectiveBitmap                         "L_PerspectiveBitmap"
#define IDF_L_PointillistBitmap                         "L_PointillistBitmap"
#define IDF_L_HalfTonePatternBitmap                     "L_HalfTonePatternBitmap"
#define IDF_L_CorrelationListBitmap                     "L_CorrelationListBitmap"
#define IDF_L_SetKaufmannRgnBitmap                      "L_SetKaufmannRgnBitmap"
#define IDF_L_SliceBitmap                               "L_SliceBitmap"
#define IDF_L_ShiftMinimumToZero                        "L_ShiftMinimumToZero"
#define IDF_L_ShiftZeroToNegative                       "L_ShiftZeroToNegative"
#define IDF_L_FeatherAlphaBlendBitmap                   "L_FeatherAlphaBlendBitmap"
#define IDF_L_ColoredPencilBitmapExt                    "L_ColoredPencilBitmapExt"
#define IDF_L_IntelligentUpScaleBitmap                  "L_IntelligentUpScaleBitmap"
#define IDF_L_IntelligentDownScaleBitmap                "L_IntelligentDownScaleBitmap"

//-----------------------------------------------------------------------------
//--LTDIS.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_AppendPlayback                            "L_AppendPlayback"
#define IDF_L_CancelPlaybackWait                        "L_CancelPlaybackWait"
#define IDF_L_ChangeFromDDB                             "L_ChangeFromDDB"
#define IDF_L_ChangeToDDB                               "L_ChangeToDDB"
#define IDF_L_ClearPlaybackUpdateRect                   "L_ClearPlaybackUpdateRect"
#define IDF_L_ClipboardReady                            "L_ClipboardReady"
#define IDF_L_ConvertColorSpace                         "L_ConvertColorSpace"
#define IDF_L_ConvertFromDDB                            "L_ConvertFromDDB"
#define IDF_L_ConvertToDDB                              "L_ConvertToDDB"
#define IDF_L_CopyFromClipboard                         "L_CopyFromClipboard"
#define IDF_L_CopyToClipboard                           "L_CopyToClipboard"
#define IDF_L_CreatePaintPalette                        "L_CreatePaintPalette"
#define IDF_L_CreatePlayback                            "L_CreatePlayback"
#define IDF_L_DestroyPlayback                           "L_DestroyPlayback"
#define IDF_L_GetDisplayMode                            "L_GetDisplayMode"
#define IDF_L_GetPaintContrast                          "L_GetPaintContrast"
#define IDF_L_GetPaintGamma                             "L_GetPaintGamma"
#define IDF_L_GetPaintIntensity                         "L_GetPaintIntensity"
#define IDF_L_GetPlaybackDelay                          "L_GetPlaybackDelay"
#define IDF_L_GetPlaybackIndex                          "L_GetPlaybackIndex"
#define IDF_L_GetPlaybackState                          "L_GetPlaybackState"
#define IDF_L_GetPlaybackUpdateRect                     "L_GetPlaybackUpdateRect"
#define IDF_L_PaintDC                                   "L_PaintDC"
#define IDF_L_PaintDCBuffer                             "L_PaintDCBuffer"
#define IDF_L_ProcessPlayback                           "L_ProcessPlayback"
#define IDF_L_RGBtoHSV                                  "L_RGBtoHSV"
#define IDF_L_HSVtoRGB                                  "L_HSVtoRGB"
#define IDF_L_SetDisplayMode                            "L_SetDisplayMode"
#define IDF_L_SetPaintContrast                          "L_SetPaintContrast"
#define IDF_L_SetPaintGamma                             "L_SetPaintGamma"
#define IDF_L_SetPaintIntensity                         "L_SetPaintIntensity"
#define IDF_L_SetPlaybackIndex                          "L_SetPlaybackIndex"
#define IDF_L_UnderlayBitmap                            "L_UnderlayBitmap"
#define IDF_L_ValidatePlaybackLines                     "L_ValidatePlaybackLines"
#define IDF_L_WindowLevel                               "L_WindowLevel"
#define IDF_L_WindowLevelFillLUT                        "L_WindowLevelFillLUT"
#define IDF_L_WindowLevelFillLUT2                       "L_WindowLevelFillLUT2"
#define IDF_L_GetBitmapClipSegments                     "L_GetBitmapClipSegments"
#define IDF_L_GetBitmapClipSegmentsMax                  "L_GetBitmapClipSegmentsMax"
#define IDF_L_StartMagGlass                             "L_StartMagGlass"
#define IDF_L_StopMagGlass                              "L_StopMagGlass"
#define IDF_L_UpdateMagGlassRect                        "L_UpdateMagGlassRect"
#define IDF_L_UpdateMagGlassPaintFlags                  "L_UpdateMagGlassPaintFlags"
#define IDF_L_UpdateMagGlass                            "L_UpdateMagGlass"
#define IDF_L_UpdateMagGlassBitmap                      "L_UpdateMagGlassBitmap"
#define IDF_L_WindowHasMagGlass                         "L_WindowHasMagGlass"
#define IDF_L_SetMagGlassOwnerDrawCallback              "L_SetMagGlassOwnerDrawCallbac"
#define IDF_L_SetMagGlassPaintOptions                   "L_SetMagGlassPaintOptions"
#define IDF_L_ShowMagGlass                              "L_ShowMagGlass"
#define IDF_L_SetMagGlassPos                            "L_SetMagGlassPos"
#define IDF_L_UpdateMagGlassShape                       "L_UpdateMagGlassShape"
#define IDF_L_PrintBitmap                               "L_PrintBitmap"
#define IDF_L_PrintBitmapFast                           "L_PrintBitmapFast"
#define IDF_L_ScreenCaptureBitmap                       "L_ScreenCaptureBitmap"
#define IDF_L_BitmapHasRgn                              "L_BitmapHasRgn"
#define IDF_L_CreateMaskFromBitmapRgn                   "L_CreateMaskFromBitmapRgn"
#define IDF_L_CurveToBezier                             "L_CurveToBezier"
#define IDF_L_FrameBitmapRgn                            "L_FrameBitmapRgn"
#define IDF_L_ColorBitmapRgn                            "L_ColorBitmapRgn"
#define IDF_L_FreeBitmapRgn                             "L_FreeBitmapRgn"
#define IDF_L_GetBitmapRgnArea                          "L_GetBitmapRgnArea"
#define IDF_L_GetBitmapRgnBounds                        "L_GetBitmapRgnBounds"
#define IDF_L_GetBitmapRgnHandle                        "L_GetBitmapRgnHandle"
#define IDF_L_IsPtInBitmapRgn                           "L_IsPtInBitmapRgn"
#define IDF_L_OffsetBitmapRgn                           "L_OffsetBitmapRgn"
#define IDF_L_PaintRgnDC                                "L_PaintRgnDC"
#define IDF_L_PaintRgnDCBuffer                          "L_PaintRgnDCBuffer"
#define IDF_L_SetBitmapRgnColor                         "L_SetBitmapRgnColor"
#define IDF_L_SetBitmapRgnColorHSVRange                 "L_SetBitmapRgnColorHSVRange"
#define IDF_L_SetBitmapRgnColorRGBRange                 "L_SetBitmapRgnColorRGBRange"
#define IDF_L_SetBitmapRgnMagicWand                     "L_SetBitmapRgnMagicWand"
#define IDF_L_SetBitmapRgnEllipse                       "L_SetBitmapRgnEllipse"
#define IDF_L_SetBitmapRgnFromMask                      "L_SetBitmapRgnFromMask"
#define IDF_L_SetBitmapRgnHandle                        "L_SetBitmapRgnHandle"
#define IDF_L_SetBitmapRgnPolygon                       "L_SetBitmapRgnPolygon"
#define IDF_L_SetBitmapRgnRect                          "L_SetBitmapRgnRect"
#define IDF_L_SetBitmapRgnRoundRect                     "L_SetBitmapRgnRoundRect"
#define IDF_L_SetBitmapRgnCurve                         "L_SetBitmapRgnCurve"
#if defined(FOR_UNICODE)
   #define IDF_L_CreatePanWindow                           "L_CreatePanWindow"
#else
   #define IDF_L_CreatePanWindow                           "L_CreatePanWindowA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_UpdatePanWindow                           "L_UpdatePanWindow"
#else
   #define IDF_L_UpdatePanWindow                           "L_UpdatePanWindowA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_DestroyPanWindow                          "L_DestroyPanWindow"
#define IDF_L_GetBitmapRgnData                          "L_GetBitmapRgnData"
#define IDF_L_SetBitmapRgnData                          "L_SetBitmapRgnData"
#define IDF_L_SetBitmapRgnClip                          "L_SetBitmapRgnClip"
#define IDF_L_GetBitmapRgnClip                          "L_GetBitmapRgnClip"
#define IDF_L_GetBitmapRgnBoundsClip                    "L_GetBitmapRgnBoundsClip"
#define IDF_L_ConvertFromWMF                            "L_ConvertFromWMF"
#define IDF_L_ChangeFromWMF                             "L_ChangeFromWMF"
#define IDF_L_ConvertToWMF                              "L_ConvertToWMF"
#define IDF_L_ChangeToWMF                               "L_ChangeToWMF"
#define IDF_L_ConvertFromEMF                            "L_ConvertFromEMF"
#define IDF_L_ChangeFromEMF                             "L_ChangeFromEMF"
#define IDF_L_ConvertToEMF                              "L_ConvertToEMF"
#define IDF_L_ChangeToEMF                               "L_ChangeToEMF"
#define IDF_L_PaintDCOverlay                            "L_PaintDCOverlay"
#define IDF_L_DoubleBufferEnable                        "L_DoubleBufferEnable"
#define IDF_L_DoubleBufferCreateHandle                  "L_DoubleBufferCreateHandle"
#define IDF_L_DoubleBufferDestroyHandle                 "L_DoubleBufferDestroyHandle"
#define IDF_L_DoubleBufferBegin                         "L_DoubleBufferBegin"
#define IDF_L_DoubleBufferEnd                           "L_DoubleBufferEnd"
#define IDF_L_SetBitmapRgnBorder                        "L_SetBitmapRgnBorder"
#define IDF_L_PaintDCCMYKArray                          "L_PaintDCCMYKArray"
#define IDF_L_SizeBitmapInterpolate                     "L_SizeBitmapInterpolate"
#define IDF_L_PrintBitmapGDIPlus                        "L_PrintBitmapGDIPlus"
//-----------------------------------------------------------------------------
//--LTFIL.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define IDF_L_CompressBuffer                            "L_CompressBuffer"
#if defined(FOR_UNICODE)
   #define IDF_L_DeletePage                                "L_DeletePage"
#else
   #define IDF_L_DeletePage                                "L_DeletePageA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_EndCompressBuffer                         "L_EndCompressBuffer"
#if defined(FOR_UNICODE)
   #define IDF_L_ReadLoadResolutions                       "L_ReadLoadResolutions"
#else
   #define IDF_L_ReadLoadResolutions                       "L_ReadLoadResolutionsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_FeedLoad                                  "L_FeedLoad"
#else
   #define IDF_L_FeedLoad                                  "L_FeedLoadA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_FileConvert                               "L_FileConvert"
#else
   #define IDF_L_FileConvert                               "L_FileConvertA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_FileInfo                                  "L_FileInfo"
#else
   #define IDF_L_FileInfo                                  "L_FileInfoA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_FileInfoMemory                            "L_FileInfoMemory"
#else
   #define IDF_L_FileInfoMemory                            "L_FileInfoMemoryA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetComment                                "L_GetComment"
#define IDF_L_GetLoadResolution                         "L_GetLoadResolution"
#if defined(FOR_UNICODE)
   #define IDF_L_GetFileCommentSize                        "L_GetFileCommentSize"
#else
   #define IDF_L_GetFileCommentSize                        "L_GetFileCommentSizeA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetTag                                    "L_GetTag"
#if defined(FOR_UNICODE)
   #define IDF_L_LoadBitmap                                "L_LoadBitmap"
#else
   #define IDF_L_LoadBitmap                                "L_LoadBitmapA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadBitmapList                            "L_LoadBitmapList"
#else
   #define IDF_L_LoadBitmapList                            "L_LoadBitmapListA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadBitmapMemory                          "L_LoadBitmapMemory"
#else
   #define IDF_L_LoadBitmapMemory                          "L_LoadBitmapMemoryA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadFile                                  "L_LoadFile"
#else
   #define IDF_L_LoadFile                                  "L_LoadFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadFileTile                              "L_LoadFileTile"
#else
   #define IDF_L_LoadFileTile                              "L_LoadFileTileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadMemoryTile                            "L_LoadMemoryTile"
#else
   #define IDF_L_LoadMemoryTile                            "L_LoadMemoryTileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadFileOffset                            "L_LoadFileOffset"
#else
   #define IDF_L_LoadFileOffset                            "L_LoadFileOffsetA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadMemory                                "L_LoadMemory"
#else
   #define IDF_L_LoadMemory                                "L_LoadMemoryA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_ReadFileComment                           "L_ReadFileComment"
#else
   #define IDF_L_ReadFileComment                           "L_ReadFileCommentA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_ReadFileCommentExt                        "L_ReadFileCommentExt"
#else
   #define IDF_L_ReadFileCommentExt                        "L_ReadFileCommentExtA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_ReadFileCommentMemory                     "L_ReadFileCommentMemory"
#if defined(FOR_UNICODE)
   #define IDF_L_ReadFileTag                               "L_ReadFileTag"
#else
   #define IDF_L_ReadFileTag                               "L_ReadFileTagA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_ReadFileTagMemory                         "L_ReadFileTagMemory"
#if defined(FOR_UNICODE)
   #define IDF_L_ReadFileStamp                             "L_ReadFileStamp"
#else
   #define IDF_L_ReadFileStamp                             "L_ReadFileStampA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveBitmap                                "L_SaveBitmap"
#else
   #define IDF_L_SaveBitmap                                "L_SaveBitmapA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveBitmapBuffer                          "L_SaveBitmapBuffer"
#else
   #define IDF_L_SaveBitmapBuffer                          "L_SaveBitmapBufferA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveBitmapList                            "L_SaveBitmapList"
#else
   #define IDF_L_SaveBitmapList                            "L_SaveBitmapListA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveBitmapMemory                          "L_SaveBitmapMemory"
#else
   #define IDF_L_SaveBitmapMemory                          "L_SaveBitmapMemoryA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveFile                                  "L_SaveFile"
#else
   #define IDF_L_SaveFile                                  "L_SaveFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveFileBuffer                            "L_SaveFileBuffer"
#else
   #define IDF_L_SaveFileBuffer                            "L_SaveFileBufferA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveFileMemory                            "L_SaveFileMemory"
#else
   #define IDF_L_SaveFileMemory                            "L_SaveFileMemoryA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveFileTile                              "L_SaveFileTile"
#else
   #define IDF_L_SaveFileTile                              "L_SaveFileTileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveFileOffset                            "L_SaveFileOffset"
#else
   #define IDF_L_SaveFileOffset                            "L_SaveFileOffsetA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_SetComment                                "L_SetComment"
#define IDF_L_SetLoadInfoCallback                       "L_SetLoadInfoCallback"
#define IDF_L_GetLoadInfoCallbackData                   "L_GetLoadInfoCallbackData"
#define IDF_L_SetLoadResolution                         "L_SetLoadResolution"
#define IDF_L_SetTag                                    "L_SetTag"
#if defined(FOR_UNICODE)
   #define IDF_L_StartCompressBuffer                       "L_StartCompressBuffer"
#else
   #define IDF_L_StartCompressBuffer                       "L_StartCompressBufferA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_StartFeedLoad                             "L_StartFeedLoad"
#else
   #define IDF_L_StartFeedLoad                             "L_StartFeedLoadA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_StopFeedLoad                              "L_StopFeedLoad"
#else
   #define IDF_L_StopFeedLoad                              "L_StopFeedLoadA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WriteFileCommentExt                       "L_WriteFileCommentExt"
#else
   #define IDF_L_WriteFileCommentExt                       "L_WriteFileCommentExtA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WriteFileStamp                            "L_WriteFileStamp"
#else
   #define IDF_L_WriteFileStamp                            "L_WriteFileStampA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_SetSaveResolution                         "L_SetSaveResolution"
#define IDF_L_GetSaveResolution                         "L_GetSaveResolution"
#define IDF_L_GetDefaultLoadFileOption                  "L_GetDefaultLoadFileOption"
#if defined(FOR_UNICODE)
   #define IDF_L_GetDefaultSaveFileOption                  "L_GetDefaultSaveFileOption"
#else
   #define IDF_L_GetDefaultSaveFileOption                  "L_GetDefaultSaveFileOptionA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WriteFileTag                              "L_WriteFileTag"
#else
   #define IDF_L_WriteFileTag                              "L_WriteFileTagA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WriteFileComment                          "L_WriteFileComment"
#else
   #define IDF_L_WriteFileComment                          "L_WriteFileCommentA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CreateThumbnailFromFile                   "L_CreateThumbnailFromFile"
#else
   #define IDF_L_CreateThumbnailFromFile                   "L_CreateThumbnailFromFileA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetJ2KOptions                             "L_GetJ2KOptions"
#define IDF_L_GetDefaultJ2KOptions                      "L_GetDefaultJ2KOptions"
#define IDF_L_SetJ2KOptions                             "L_SetJ2KOptions"
#define IDF_L_MarkerCallbackProxy                       "L_MarkerCallbackProxy"
#if defined(FOR_UNICODE)
   #define IDF_L_TransformFile                             "L_TransformFile"
#else
   #define IDF_L_TransformFile                             "L_TransformFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_ReadFileExtensions                        "L_ReadFileExtensions"
#else
   #define IDF_L_ReadFileExtensions                        "L_ReadFileExtensionsA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_FreeExtensions                            "L_FreeExtensions"
#define IDF_L_LoadExtensionStamp                        "L_LoadExtensionStamp"
#define IDF_L_GetExtensionAudio                         "L_GetExtensionAudio"
#if defined(FOR_UNICODE)
   #define IDF_L_StartDecompressBuffer                     "L_StartDecompressBuffer"
#else
   #define IDF_L_StartDecompressBuffer                     "L_StartDecompressBufferA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_StopDecompressBuffer                      "L_StopDecompressBuffer"
#define IDF_L_DecompressBuffer                          "L_DecompressBuffer"
#if defined(FOR_UNICODE)
   #define IDF_L_IgnoreFilters                             "L_IgnoreFilters"
#else
   #define IDF_L_IgnoreFilters                             "L_IgnoreFiltersA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_PreLoadFilters                            "L_PreLoadFilters"
#else
   #define IDF_L_PreLoadFilters                            "L_PreLoadFiltersA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_GetIgnoreFilters                          "L_GetIgnoreFilters"
#else
   #define IDF_L_GetIgnoreFilters                          "L_GetIgnoreFiltersA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_GetPreLoadFilters                         "L_GetPreLoadFilters"
#else
   #define IDF_L_GetPreLoadFilters                         "L_GetPreLoadFiltersA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetJBIG2Options                           "L_GetJBIG2Options"
#define IDF_L_SetJBIG2Options                           "L_SetJBIG2Options"
#define IDF_L_CreateMarkers                             "L_CreateMarkers"
#if defined(FOR_UNICODE)
   #define IDF_L_LoadMarkers                               "L_LoadMarkers"
#else
   #define IDF_L_LoadMarkers                               "L_LoadMarkersA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_FreeMarkers                               "L_FreeMarkers"
#define IDF_L_SetMarkers                                "L_SetMarkers"
#define IDF_L_GetMarkers                                "L_GetMarkers"
#define IDF_L_EnumMarkers                               "L_EnumMarkers"
#define IDF_L_DeleteMarker                              "L_DeleteMarker"
#define IDF_L_InsertMarker                              "L_InsertMarker"
#define IDF_L_CopyMarkers                               "L_CopyMarkers"
#define IDF_L_GetMarkerCount                            "L_GetMarkerCount"
#define IDF_L_GetMarker                                 "L_GetMarker"
#define IDF_L_DeleteMarkerIndex                         "L_DeleteMarkerIndex"
#if defined(FOR_UNICODE)
   #define IDF_L_WriteFileMetaData                         "L_WriteFileMetaData"
#else
   #define IDF_L_WriteFileMetaData                         "L_WriteFileMetaDataA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_StartSaveData                             "L_StartSaveData"
#else
   #define IDF_L_StartSaveData                             "L_StartSaveDataA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_SaveData                                  "L_SaveData"
#define IDF_L_StopSaveData                              "L_StopSaveData"
#if defined(FOR_UNICODE)
   #define IDF_L_SetOverlayCallback                        "L_SetOverlayCallback"
#else
   #define IDF_L_SetOverlayCallback                        "L_SetOverlayCallbackA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_GetOverlayCallback                        "L_GetOverlayCallback"
#else
   #define IDF_L_GetOverlayCallback                        "L_GetOverlayCallbackA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetFilterListInfo                         "L_GetFilterListInfo"
#define IDF_L_GetFilterInfo                             "L_GetFilterInfo"
#define IDF_L_FreeFilterInfo                            "L_FreeFilterInfo"
#define IDF_L_SetFilterInfo                             "L_SetFilterInfo"
#if defined(FOR_UNICODE)
   #define IDF_L_GetTXTOptions                             "L_GetTXTOptions"
#else
   #define IDF_L_GetTXTOptions                             "L_GetTXTOptionsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SetTXTOptions                             "L_SetTXTOptions"
#else
   #define IDF_L_SetTXTOptions                             "L_SetTXTOptionsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CompactFile                               "L_CompactFile"
#else
   #define IDF_L_CompactFile                               "L_CompactFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadFileCMYKArray                         "L_LoadFileCMYKArray"
#else
   #define IDF_L_LoadFileCMYKArray                         "L_LoadFileCMYKArrayA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveFileCMYKArray                         "L_SaveFileCMYKArray"
#else
   #define IDF_L_SaveFileCMYKArray                         "L_SaveFileCMYKArrayA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_EnumFileTags                              "L_EnumFileTags"
#else
   #define IDF_L_EnumFileTags                              "L_EnumFileTagsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_DeleteTag                                 "L_DeleteTag"
#else
   #define IDF_L_DeleteTag                                 "L_DeleteTagA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_SetGeoKey                                 "L_SetGeoKey"
#define IDF_L_GetGeoKey                                 "L_GetGeoKey"
#if defined(FOR_UNICODE)
   #define IDF_L_WriteFileGeoKey                           "L_WriteFileGeoKey"
#else
   #define IDF_L_WriteFileGeoKey                           "L_WriteFileGeoKeyA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_ReadFileGeoKey                            "L_ReadFileGeoKey"
#else
   #define IDF_L_ReadFileGeoKey                            "L_ReadFileGeoKeyA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_EnumFileGeoKeys                           "L_EnumFileGeoKeys"
#else
   #define IDF_L_EnumFileGeoKeys                           "L_EnumFileGeoKeysA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_ReadFileCommentOffset                     "L_ReadFileCommentOffset"
#define IDF_L_GetLoadStatus                             "L_GetLoadStatus"
#define IDF_L_DecodeABIC                                "L_DecodeABIC"
#define IDF_L_EncodeABIC                                "L_EncodeABIC"
#define IDF_L_SetPNGTRNS                                "L_SetPNGTRNS"
#define IDF_L_GetPNGTRNS                                "L_GetPNGTRNS"
#if defined(FOR_UNICODE)
   #define IDF_L_LoadBitmapResize                          "L_LoadBitmapResize"
#else
   #define IDF_L_LoadBitmapResize                          "L_LoadBitmapResizeA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_ReadFileTransforms                        "L_ReadFileTransforms"
#else
   #define IDF_L_ReadFileTransforms                        "L_ReadFileTransformsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WriteFileTransforms                       "L_WriteFileTransforms"
#else
   #define IDF_L_WriteFileTransforms                       "L_WriteFileTransformsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_GetPCDResolution                          "L_GetPCDResolution"
#else
   #define IDF_L_GetPCDResolution                          "L_GetPCDResolutionA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetWMFResolution                          "L_GetWMFResolution"
#define IDF_L_SetPCDResolution                          "L_SetPCDResolution"
#define IDF_L_SetWMFResolution                          "L_SetWMFResolution"
#if defined(FOR_UNICODE)
   #define IDF_L_SaveCustomFile                            "L_SaveCustomFile"
#else
   #define IDF_L_SaveCustomFile                            "L_SaveCustomFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_LoadCustomFile                            "L_LoadCustomFile"
#else
   #define IDF_L_LoadCustomFile                            "L_LoadCustomFileA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_2DSetViewport                             "L_2DSetViewport"
#define IDF_L_2DGetViewport                             "L_2DGetViewport"
#define IDF_L_2DSetViewMode                             "L_2DSetViewMode"
#define IDF_L_2DGetViewMode                             "L_2DGetViewMode"
#if defined(FOR_UNICODE)
   #define IDF_L_VecLoadFile                               "L_VecLoadFile"
#else
   #define IDF_L_VecLoadFile                               "L_VecLoadFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecLoadMemory                             "L_VecLoadMemory"
#else
   #define IDF_L_VecLoadMemory                             "L_VecLoadMemoryA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecStartFeedLoad                          "L_VecStartFeedLoad"
#else
   #define IDF_L_VecStartFeedLoad                          "L_VecStartFeedLoadA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecFeedLoad                               "L_VecFeedLoad"
#define IDF_L_VecStopFeedLoad                           "L_VecStopFeedLoad"
#if defined(FOR_UNICODE)
   #define IDF_L_VecSaveFile                               "L_VecSaveFile"
#else
   #define IDF_L_VecSaveFile                               "L_VecSaveFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecSaveMemory                             "L_VecSaveMemory"
#else
   #define IDF_L_VecSaveMemory                             "L_VecSaveMemoryA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetPLTOptions                             "L_GetPLTOptions"
#define IDF_L_SetPLTOptions                             "L_SetPLTOptions"
#if defined(FOR_UNICODE)
   #define IDF_L_GetPDFInitDir                             "L_GetPDFInitDir"
#else
   #define IDF_L_GetPDFInitDir                             "L_GetPDFInitDirA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SetPDFInitDir                             "L_SetPDFInitDir"
#else
   #define IDF_L_SetPDFInitDir                             "L_SetPDFInitDirA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetRTFOptions                             "L_GetRTFOptions"
#define IDF_L_SetRTFOptions                             "L_SetRTFOptions"
#define IDF_L_GetPTKOptions                             "L_GetPTKOptions"
#define IDF_L_SetPTKOptions                             "L_SetPTKOptions"
#if defined(FOR_UNICODE)
   #define IDF_L_GetPDFOptions                             "L_GetPDFOptions"
#else
   #define IDF_L_GetPDFOptions                             "L_GetPDFOptionsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SetPDFOptions                             "L_SetPDFOptions"
#else
   #define IDF_L_SetPDFOptions                             "L_SetPDFOptionsA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetPDFSaveOptions                         "L_GetPDFSaveOptions"
#define IDF_L_SetPDFSaveOptions                         "L_SetPDFSaveOptions"
#define IDF_L_GetDJVOptions                             "L_GetDJVOptions"
#define IDF_L_SetDJVOptions                             "L_SetDJVOptions"
#if defined(FOR_UNICODE)
   #define IDF_L_LoadLayer                                 "L_LoadLayer"
#else
   #define IDF_L_LoadLayer                                 "L_LoadLayerA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveBitmapWithLayers                      "L_SaveBitmapWithLayers"
#else
   #define IDF_L_SaveBitmapWithLayers                      "L_SaveBitmapWithLayersA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_GetAutoCADFilesColorScheme                "L_GetAutoCADFilesColorScheme"
#define IDF_L_SetAutoCADFilesColorScheme                "L_SetAutoCADFilesColorScheme"
#define IDF_L_VecAddFontMapper                          "L_VecAddFontMapper"
#define IDF_L_VecRemoveFontMapper                       "L_VecRemoveFontMapper"
#define IDF_L_GetXPSOptions                             "L_GetXPSOptions"
#define IDF_L_SetXPSOptions                             "L_SetXPSOptions"
//-----------------------------------------------------------------------------
//--LTEFX.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_EfxPaintTransition                        "L_EfxPaintTransition"
#define IDF_L_EfxPaintBitmap                            "L_EfxPaintBitmap"
#define IDF_L_EfxDrawFrame                              "L_EfxDrawFrame"
#define IDF_L_EfxTileRect                               "L_EfxTileRect"
#define IDF_L_EfxGradientFillRect                       "L_EfxGradientFillRect"
#define IDF_L_EfxPatternFillRect                        "L_EfxPatternFillRect"
#if defined(FOR_UNICODE)
   #define IDF_L_EfxDraw3dText                             "L_EfxDraw3dText"
#else
   #define IDF_L_EfxDraw3dText                             "L_EfxDraw3dTextA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_EfxEffectBlt                              "L_EfxEffectBlt"
#define IDF_L_PaintDCEffect                             "L_PaintDCEffect"
#define IDF_L_PaintRgnDCEffect                          "L_PaintRgnDCEffect"
#if defined(FOR_UNICODE)
   #define IDF_L_EfxDrawRotated3dText                      "L_EfxDrawRotated3dText"
#else
   #define IDF_L_EfxDrawRotated3dText                      "L_EfxDrawRotated3dTextA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_EfxDraw3dShape                            "L_EfxDraw3dShape"

//-----------------------------------------------------------------------------
//--LTTW2.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#if defined(FOR_UNICODE)
   #define IDF_L_TwainInitSession                          "L_TwainInitSession"
#else
   #define IDF_L_TwainInitSession                          "L_TwainInitSessionA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TwainInitSession2                         "L_TwainInitSession2"
#else
   #define IDF_L_TwainInitSession2                         "L_TwainInitSession2A"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TwainEndSession                           "L_TwainEndSession"
#if defined(FOR_UNICODE)
   #define IDF_L_TwainSetProperties                        "L_TwainSetProperties"
#else
   #define IDF_L_TwainSetProperties                        "L_TwainSetPropertiesA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TwainGetProperties                        "L_TwainGetProperties"
#else
   #define IDF_L_TwainGetProperties                        "L_TwainGetPropertiesA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TwainAcquireList                          "L_TwainAcquireList"
#else
   #define IDF_L_TwainAcquireList                          "L_TwainAcquireListA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TwainAcquire                              "L_TwainAcquire"
#else
   #define IDF_L_TwainAcquire                              "L_TwainAcquireA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TwainSelectSource                         "L_TwainSelectSource"
#else
   #define IDF_L_TwainSelectSource                         "L_TwainSelectSourceA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TwainQueryProperty                        "L_TwainQueryProperty"
#define IDF_L_TwainStartCapsNeg                         "L_TwainStartCapsNeg"
#define IDF_L_TwainEndCapsNeg                           "L_TwainEndCapsNeg"
#define IDF_L_TwainSetCapability                        "L_TwainSetCapability"
#define IDF_L_TwainGetCapability                        "L_TwainGetCapability"
#define IDF_L_TwainEnumCapabilities                     "L_TwainEnumCapabilities"
#define IDF_L_TwainCreateNumericContainerOneValue       "L_TwainCreateNumericContainerOneValue"
#define IDF_L_TwainCreateNumericContainerRange          "L_TwainCreateNumericContainerRange"
#define IDF_L_TwainCreateNumericContainerArray          "L_TwainCreateNumericContainerArray"
#define IDF_L_TwainCreateNumericContainerEnum           "L_TwainCreateNumericContainerEnum"
#define IDF_L_TwainGetNumericContainerValue             "L_TwainGetNumericContainerValue"
#define IDF_L_TwainFreeContainer                        "L_TwainFreeContainer"
#define IDF_L_TwainFreePropQueryStructure               "L_TwainFreePropQueryStructure"
#if defined(FOR_UNICODE)
   #define IDF_L_TwainTemplateDlg                          "L_TwainTemplateDlg"
#else
   #define IDF_L_TwainTemplateDlg                          "L_TwainTemplateDlgA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TwainOpenTemplateFile                     "L_TwainOpenTemplateFile"
#else
   #define IDF_L_TwainOpenTemplateFile                     "L_TwainOpenTemplateFileA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TwainAddCapabilityToFile                  "L_TwainAddCapabilityToFile"
#define IDF_L_TwainGetCapabilityFromFile                "L_TwainGetCapabilityFromFile"
#define IDF_L_TwainGetNumOfCapsInFile                   "L_TwainGetNumOfCapsInFile"
#define IDF_L_TwainCloseTemplateFile                    "L_TwainCloseTemplateFile"
#define IDF_L_TwainGetExtendedImageInfo                 "L_TwainGetExtendedImageInfo"
#define IDF_L_TwainFreeExtendedImageInfoStructure       "L_TwainFreeExtendedImageInfoStructure"
#define IDF_L_TwainLockContainer                        "L_TwainLockContainer"
#define IDF_L_TwainUnlockContainer                      "L_TwainUnlockContainer"
#define IDF_L_TwainGetNumericContainerItemType          "L_TwainGetNumericContainerItemType"
#define IDF_L_TwainGetNumericContainerINTValue          "L_TwainGetNumericContainerINTValue"
#define IDF_L_TwainGetNumericContainerUINTValue         "L_TwainGetNumericContainerUINTValue"
#define IDF_L_TwainGetNumericContainerBOOLValue         "L_TwainGetNumericContainerBOOLValue"
#define IDF_L_TwainGetNumericContainerFIX32Value        "L_TwainGetNumericContainerFIX32Value"
#define IDF_L_TwainGetNumericContainerFRAMEValue        "L_TwainGetNumericContainerFRAMEValue"
#define IDF_L_TwainGetNumericContainerSTRINGValue       "L_TwainGetNumericContainerSTRINGValue"
#define IDF_L_TwainGetNumericContainerUNICODEValue      "L_TwainGetNumericContainerUNICODEValue"
#if defined(FOR_UNICODE)
   #define IDF_L_TwainAcquireMulti                         "L_TwainAcquireMulti"
#else
   #define IDF_L_TwainAcquireMulti                         "L_TwainAcquireMultiA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_IsTwainAvailable                          "L_IsTwainAvailable"
#if defined(FOR_UNICODE)
   #define IDF_L_TwainFindFastConfig                       "L_TwainFindFastConfig"
#else
   #define IDF_L_TwainFindFastConfig                       "L_TwainFindFastConfigA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TwainGetScanConfigs                       "L_TwainGetScanConfigs"
#define IDF_L_TwainFreeScanConfig                       "L_TwainFreeScanConfig"
#if defined(FOR_UNICODE)
   #define IDF_L_TwainGetSources                           "L_TwainGetSources"
#else
   #define IDF_L_TwainGetSources                           "L_TwainGetSourcesA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TwainEnableShowUserInterfaceOnly          "L_TwainEnableShowUserInterfaceOnly"
#define IDF_L_TwainCancelAcquire                        "L_TwainCancelAcquire"
#define IDF_L_TwainQueryFileSystem                      "L_TwainQueryFileSystem"
#define IDF_L_TwainGetJPEGCompression                   "L_TwainGetJPEGCompression"
#define IDF_L_TwainSetJPEGCompression                   "L_TwainSetJPEGCompression"
#if defined(FOR_UNICODE)
   #define IDF_L_TwainSetTransferOptions                   "L_TwainSetTransferOptions"
#else
   #define IDF_L_TwainSetTransferOptions                   "L_TwainSetTransferOptionsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TwainGetTransferOptions                   "L_TwainGetTransferOptions"
#else
   #define IDF_L_TwainGetTransferOptions                   "L_TwainGetTransferOptionsA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TwainGetSupportedTransferMode             "L_TwainGetSupportedTransferMode"
#define IDF_L_TwainSetResolution                        "L_TwainSetResolution"
#define IDF_L_TwainGetResolution                        "L_TwainGetResolution"
#define IDF_L_TwainSetImageFrame                        "L_TwainSetImageFrame"
#define IDF_L_TwainGetImageFrame                        "L_TwainGetImageFrame"
#define IDF_L_TwainSetImageUnit                         "L_TwainSetImageUnit"
#define IDF_L_TwainGetImageUnit                         "L_TwainGetImageUnit"
#define IDF_L_TwainSetImageBitsPerPixel                 "L_TwainSetImageBitsPerPixel"
#define IDF_L_TwainGetImageBitsPerPixel                 "L_TwainGetImageBitsPerPixel"
#define IDF_L_TwainSetImageEffects                      "L_TwainSetImageEffects"
#define IDF_L_TwainGetImageEffects                      "L_TwainGetImageEffects"
#define IDF_L_TwainSetAcquirePageOptions                "L_TwainSetAcquirePageOptions"
#define IDF_L_TwainGetAcquirePageOptions                "L_TwainGetAcquirePageOptions"
#define IDF_L_TwainSetRGBResponse                       "L_TwainSetRGBResponse"
#define IDF_L_TwainShowProgress                         "L_TwainShowProgress"
#define IDF_L_TwainEnableDuplex                         "L_TwainEnableDuplex"
#define IDF_L_TwainGetDuplexOptions                     "L_TwainGetDuplexOptions"
#define IDF_L_TwainSetMaxXferCount                      "L_TwainSetMaxXferCount"
#define IDF_L_TwainGetMaxXferCount                      "L_TwainGetMaxXferCount"
#define IDF_L_TwainStopFeeder                           "L_TwainStopFeeder"
#define IDF_L_TwainSetDeviceEventCallback               "L_TwainSetDeviceEventCallback"
#define IDF_L_TwainGetDeviceEventData                   "L_TwainGetDeviceEventData"
#define IDF_L_TwainSetDeviceEventCapability             "L_TwainSetDeviceEventCapability"
#define IDF_L_TwainGetDeviceEventCapability             "L_TwainGetDeviceEventCapability"
#define IDF_L_TwainResetDeviceEventCapability           "L_TwainResetDeviceEventCapability"
#if defined(FOR_UNICODE)
   #define IDF_L_TwainFastAcquire                          "L_TwainFastAcquire"
#else
   #define IDF_L_TwainFastAcquire                          "L_TwainFastAcquireA"
#endif //#if defined(FOR_UNICODE)

#if defined(LEADTOOLS_V16_OR_LATER)

#define IDF_L_TwainGetCustomDSData                      "L_TwainGetCustomDSData"
#define IDF_L_TwainSetCustomDSData                      "L_TwainSetCustomDSData"

#endif // #if defined(LEADTOOLS_V16_OR_LATER)

//-----------------------------------------------------------------------------
//--LTANN.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_AnnBringToFront                           "L_AnnBringToFront"
#define IDF_L_AnnClipboardReady                         "L_AnnClipboardReady"
#define IDF_L_AnnCopy                                   "L_AnnCopy"
#define IDF_L_AnnCopyFromClipboard                      "L_AnnCopyFromClipboard"
#define IDF_L_AnnCopyToClipboard                        "L_AnnCopyToClipboard"
#define IDF_L_AnnCutToClipboard                         "L_AnnCutToClipboard"
#define IDF_L_AnnCreate                                 "L_AnnCreate"
#define IDF_L_AnnCreateContainer                        "L_AnnCreateContainer"
#define IDF_L_AnnCreateItem                             "L_AnnCreateItem"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnCreateToolBar                          "L_AnnCreateToolBar"
#else
   #define IDF_L_AnnCreateToolBar                          "L_AnnCreateToolBarA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnDefine                                 "L_AnnDefine"
#define IDF_L_AnnDeletePageOffset                       "L_AnnDeletePageOffset"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnDeletePage                             "L_AnnDeletePage"
#else
   #define IDF_L_AnnDeletePage                             "L_AnnDeletePageA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnDeletePageMemory                       "L_AnnDeletePageMemory"
#define IDF_L_AnnDestroy                                "L_AnnDestroy"
#define IDF_L_AnnDraw                                   "L_AnnDraw"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnEnumerate                              "L_AnnEnumerate"
#else
   #define IDF_L_AnnEnumerate                              "L_AnnEnumerateA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnFileInfo                               "L_AnnFileInfo"
#else
   #define IDF_L_AnnFileInfo                               "L_AnnFileInfoA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnFileInfoOffset                         "L_AnnFileInfoOffset"
#define IDF_L_AnnFileInfoMemory                         "L_AnnFileInfoMemory"
#define IDF_L_AnnFlip                                   "L_AnnFlip"
#define IDF_L_AnnGetActiveState                         "L_AnnGetActiveState"
#define IDF_L_AnnGetAutoContainer                       "L_AnnGetAutoContainer"
#define IDF_L_AnnGetAutoDrawEnable                      "L_AnnGetAutoDrawEnable"
#define IDF_L_AnnGetAutoMenuEnable                      "L_AnnGetAutoMenuEnable"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetAutoText                            "L_AnnGetAutoText"
#else
   #define IDF_L_AnnGetAutoText                            "L_AnnGetAutoTextA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetAutoTextLen                         "L_AnnGetAutoTextLen"
#define IDF_L_AnnGetBackColor                           "L_AnnGetBackColor"
#define IDF_L_AnnGetBitmap                              "L_AnnGetBitmap"
#define IDF_L_AnnGetBitmapDpiX                          "L_AnnGetBitmapDpiX"
#define IDF_L_AnnGetBitmapDpiY                          "L_AnnGetBitmapDpiY"
#define IDF_L_AnnGetBoundingRect                        "L_AnnGetBoundingRect"
#define IDF_L_AnnGetContainer                           "L_AnnGetContainer"
#define IDF_L_AnnGetDistance                            "L_AnnGetDistance"
#define IDF_L_AnnGetDpiX                                "L_AnnGetDpiX"
#define IDF_L_AnnGetDpiY                                "L_AnnGetDpiY"
#define IDF_L_AnnGetFillMode                            "L_AnnGetFillMode"
#define IDF_L_AnnGetFillPattern                         "L_AnnGetFillPattern"
#define IDF_L_AnnGetFontBold                            "L_AnnGetFontBold"
#define IDF_L_AnnGetFontItalic                          "L_AnnGetFontItalic"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetFontName                            "L_AnnGetFontName"
#else
   #define IDF_L_AnnGetFontName                            "L_AnnGetFontNameA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetFontNameLen                         "L_AnnGetFontNameLen"
#define IDF_L_AnnGetFontSize                            "L_AnnGetFontSize"
#define IDF_L_AnnGetFontStrikeThrough                   "L_AnnGetFontStrikeThrough"
#define IDF_L_AnnGetFontUnderline                       "L_AnnGetFontUnderline"
#define IDF_L_AnnGetForeColor                           "L_AnnGetForeColor"
#define IDF_L_AnnGetGaugeLength                         "L_AnnGetGaugeLength"
#define IDF_L_AnnGetTicMarkLength                       "L_AnnGetTicMarkLength"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetHyperlink                           "L_AnnGetHyperlink"
#else
   #define IDF_L_AnnGetHyperlink                           "L_AnnGetHyperlinkA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetHyperlinkLen                        "L_AnnGetHyperlinkLen"
#define IDF_L_AnnGetHyperlinkMenuEnable                 "L_AnnGetHyperlinkMenuEnable"
#define IDF_L_AnnGetLineStyle                           "L_AnnGetLineStyle"
#define IDF_L_AnnGetLineWidth                           "L_AnnGetLineWidth"
#define IDF_L_AnnGetLocked                              "L_AnnGetLocked"
#define IDF_L_AnnGetOffsetX                             "L_AnnGetOffsetX"
#define IDF_L_AnnGetOffsetY                             "L_AnnGetOffsetY"
#define IDF_L_AnnGetPointCount                          "L_AnnGetPointCount"
#define IDF_L_AnnGetPoints                              "L_AnnGetPoints"
#define IDF_L_AnnGetPolyFillMode                        "L_AnnGetPolyFillMode"
#define IDF_L_AnnGetRect                                "L_AnnGetRect"
#define IDF_L_AnnGetROP2                                "L_AnnGetROP2"
#define IDF_L_AnnGetScalarX                             "L_AnnGetScalarX"
#define IDF_L_AnnGetScalarY                             "L_AnnGetScalarY"
#define IDF_L_AnnGetSelectCount                         "L_AnnGetSelectCount"
#define IDF_L_AnnGetSelected                            "L_AnnGetSelected"
#define IDF_L_AnnGetSelectItems                         "L_AnnGetSelectItems"
#define IDF_L_AnnGetSelectRect                          "L_AnnGetSelectRect"
#define IDF_L_AnnGetTag                                 "L_AnnGetTag"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetText                                "L_AnnGetText"
#else
   #define IDF_L_AnnGetText                                "L_AnnGetTextA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetTextLen                             "L_AnnGetTextLen"
#define IDF_L_AnnGetTextAlign                           "L_AnnGetTextAlign"
#define IDF_L_AnnGetTextRotate                          "L_AnnGetTextRotate"
#define IDF_L_AnnGetTextPointerFixed                    "L_AnnGetTextPointerFixed"
#define IDF_L_AnnSetTextExpandTokens                    "L_AnnSetTextExpandTokens"
#define IDF_L_AnnGetTextExpandTokens                    "L_AnnGetTextExpandTokens"
#define IDF_L_AnnGetTool                                "L_AnnGetTool"
#define IDF_L_AnnGetToolBarButtonVisible                "L_AnnGetToolBarButtonVisible"
#define IDF_L_AnnGetToolBarChecked                      "L_AnnGetToolBarChecked"
#define IDF_L_AnnGetTransparent                         "L_AnnGetTransparent"
#define IDF_L_AnnGetType                                "L_AnnGetType"
#define IDF_L_AnnGetTopContainer                        "L_AnnGetTopContainer"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetUnit                                "L_AnnGetUnit"
#else
   #define IDF_L_AnnGetUnit                                "L_AnnGetUnitA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetUnitLen                             "L_AnnGetUnitLen"
#define IDF_L_AnnGetUserMode                            "L_AnnGetUserMode"
#define IDF_L_AnnGetVisible                             "L_AnnGetVisible"
#define IDF_L_AnnGetWnd                                 "L_AnnGetWnd"
#define IDF_L_AnnInsert                                 "L_AnnInsert"
#define IDF_L_AnnGetItem                                "L_AnnGetItem"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnLoad                                   "L_AnnLoad"
#else
   #define IDF_L_AnnLoad                                   "L_AnnLoadA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnLoadOffset                             "L_AnnLoadOffset"
#define IDF_L_AnnLoadMemory                             "L_AnnLoadMemory"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnLock                                   "L_AnnLock"
#else
   #define IDF_L_AnnLock                                   "L_AnnLockA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnMove                                   "L_AnnMove"
#define IDF_L_AnnPrint                                  "L_AnnPrint"
#define IDF_L_AnnRealize                                "L_AnnRealize"
#define IDF_L_AnnResize                                 "L_AnnResize"
#define IDF_L_AnnReverse                                "L_AnnReverse"
#define IDF_L_AnnRemove                                 "L_AnnRemove"
#define IDF_L_AnnRotate                                 "L_AnnRotate"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSave                                   "L_AnnSave"
#else
   #define IDF_L_AnnSave                                   "L_AnnSaveA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSaveOffset                             "L_AnnSaveOffset"
#else
   #define IDF_L_AnnSaveOffset                             "L_AnnSaveOffsetA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSaveMemory                             "L_AnnSaveMemory"
#else
   #define IDF_L_AnnSaveMemory                             "L_AnnSaveMemoryA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSaveTag                                "L_AnnSaveTag"
#define IDF_L_AnnSelectPoint                            "L_AnnSelectPoint"
#define IDF_L_AnnSelectRect                             "L_AnnSelectRect"
#define IDF_L_AnnSendToBack                             "L_AnnSendToBack"
#define IDF_L_AnnSetActiveState                         "L_AnnSetActiveState"
#define IDF_L_AnnSetAutoContainer                       "L_AnnSetAutoContainer"
#define IDF_L_AnnSetAutoDrawEnable                      "L_AnnSetAutoDrawEnable"
#define IDF_L_AnnSetAutoMenuEnable                      "L_AnnSetAutoMenuEnable"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetAutoText                            "L_AnnSetAutoText"
#else
   #define IDF_L_AnnSetAutoText                            "L_AnnSetAutoTextA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetBackColor                           "L_AnnSetBackColor"
#define IDF_L_AnnSetBitmap                              "L_AnnSetBitmap"
#define IDF_L_AnnSetBitmapDpiX                          "L_AnnSetBitmapDpiX"
#define IDF_L_AnnSetBitmapDpiY                          "L_AnnSetBitmapDpiY"
#define IDF_L_AnnSetDpiX                                "L_AnnSetDpiX"
#define IDF_L_AnnSetDpiY                                "L_AnnSetDpiY"
#define IDF_L_AnnSetFillMode                            "L_AnnSetFillMode"
#define IDF_L_AnnSetFillPattern                         "L_AnnSetFillPattern"
#define IDF_L_AnnSetFontBold                            "L_AnnSetFontBold"
#define IDF_L_AnnSetFontItalic                          "L_AnnSetFontItalic"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetFontName                            "L_AnnSetFontName"
#else
   #define IDF_L_AnnSetFontName                            "L_AnnSetFontNameA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetFontSize                            "L_AnnSetFontSize"
#define IDF_L_AnnSetFontStrikeThrough                   "L_AnnSetFontStrikeThrough"
#define IDF_L_AnnSetFontUnderline                       "L_AnnSetFontUnderline"
#define IDF_L_AnnSetForeColor                           "L_AnnSetForeColor"
#define IDF_L_AnnSetGaugeLength                         "L_AnnSetGaugeLength"
#define IDF_L_AnnSetTicMarkLength                       "L_AnnSetTicMarkLength"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetHyperlink                           "L_AnnSetHyperlink"
#else
   #define IDF_L_AnnSetHyperlink                           "L_AnnSetHyperlinkA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetHyperlinkMenuEnable                 "L_AnnSetHyperlinkMenuEnable"
#define IDF_L_AnnSetLineStyle                           "L_AnnSetLineStyle"
#define IDF_L_AnnSetLineWidth                           "L_AnnSetLineWidth"
#define IDF_L_AnnSetOffsetX                             "L_AnnSetOffsetX"
#define IDF_L_AnnSetOffsetY                             "L_AnnSetOffsetY"
#define IDF_L_AnnSetPoints                              "L_AnnSetPoints"
#define IDF_L_AnnSetPolyFillMode                        "L_AnnSetPolyFillMode"
#define IDF_L_AnnSetROP2                                "L_AnnSetROP2"
#define IDF_L_AnnSetRect                                "L_AnnSetRect"
#define IDF_L_AnnSetSelected                            "L_AnnSetSelected"
#define IDF_L_AnnSetScalarX                             "L_AnnSetScalarX"
#define IDF_L_AnnSetScalarY                             "L_AnnSetScalarY"
#define IDF_L_AnnSetTag                                 "L_AnnSetTag"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetText                                "L_AnnSetText"
#else
   #define IDF_L_AnnSetText                                "L_AnnSetTextA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetTextAlign                           "L_AnnSetTextAlign"
#define IDF_L_AnnSetTextRotate                          "L_AnnSetTextRotate"
#define IDF_L_AnnSetTextPointerFixed                    "L_AnnSetTextPointerFixed"
#define IDF_L_AnnSetTool                                "L_AnnSetTool"
#define IDF_L_AnnSetToolBarButtonVisible                "L_AnnSetToolBarButtonVisible"
#define IDF_L_AnnSetToolBarChecked                      "L_AnnSetToolBarChecked"
#define IDF_L_AnnSetTransparent                         "L_AnnSetTransparent"
#define IDF_L_AnnSetUndoDepth                           "L_AnnSetUndoDepth"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetUnit                                "L_AnnSetUnit"
#else
   #define IDF_L_AnnSetUnit                                "L_AnnSetUnitA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetUserMode                            "L_AnnSetUserMode"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetVisible                             "L_AnnSetVisible"
#else
   #define IDF_L_AnnSetVisible                             "L_AnnSetVisibleA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetWnd                                 "L_AnnSetWnd"
#define IDF_L_AnnShowLockedIcon                         "L_AnnShowLockedIcon"
#define IDF_L_AnnUndo                                   "L_AnnUndo"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnUnlock                                 "L_AnnUnlock"
#else
   #define IDF_L_AnnUnlock                                 "L_AnnUnlockA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnUnrealize                              "L_AnnUnrealize"
#define IDF_L_AnnSetNodes                               "L_AnnSetNodes"
#define IDF_L_AnnGetNodes                               "L_AnnGetNodes"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetProtractorOptions                   "L_AnnSetProtractorOptions"
#else
   #define IDF_L_AnnSetProtractorOptions                   "L_AnnSetProtractorOptionsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetProtractorOptions                   "L_AnnGetProtractorOptions"
#else
   #define IDF_L_AnnGetProtractorOptions                   "L_AnnGetProtractorOptionsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetNameOptions                         "L_AnnGetNameOptions"
#else
   #define IDF_L_AnnGetNameOptions                         "L_AnnGetNameOptionsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetNameOptions                         "L_AnnSetNameOptions"
#else
   #define IDF_L_AnnSetNameOptions                         "L_AnnSetNameOptionsA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetShowFlags                           "L_AnnSetShowFlags"
#define IDF_L_AnnGetShowFlags                           "L_AnnGetShowFlags"
#define IDF_L_AnnGetAngle                               "L_AnnGetAngle"
#define IDF_L_AnnSetMetafile                            "L_AnnSetMetafile"
#define IDF_L_AnnGetMetafile                            "L_AnnGetMetafile"
#define IDF_L_AnnGetSecondaryMetafile                   "L_AnnGetSecondaryMetafile"
#define IDF_L_AnnSetPredefinedMetafile                  "L_AnnSetPredefinedMetafile"
#define IDF_L_AnnGetPredefinedMetafile                  "L_AnnGetPredefinedMetafile"
#define IDF_L_AnnSetSecondaryBitmap                     "L_AnnSetSecondaryBitmap"
#define IDF_L_AnnGetSecondaryBitmap                     "L_AnnGetSecondaryBitmap"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetAutoMenuItemEnable                  "L_AnnSetAutoMenuItemEnable"
#else
   #define IDF_L_AnnSetAutoMenuItemEnable                  "L_AnnSetAutoMenuItemEnableA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetAutoMenuItemEnable                  "L_AnnGetAutoMenuItemEnable"
#define IDF_L_AnnSetAutoMenuState                       "L_AnnSetAutoMenuState"
#define IDF_L_AnnGetAutoMenuState                       "L_AnnGetAutoMenuState"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetUser                                "L_AnnSetUser"
#else
   #define IDF_L_AnnSetUser                                "L_AnnSetUserA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetToolBarButtons                      "L_AnnSetToolBarButtons"
#else
   #define IDF_L_AnnSetToolBarButtons                      "L_AnnSetToolBarButtonsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetToolBarButtons                      "L_AnnGetToolBarButtons"
#else
   #define IDF_L_AnnGetToolBarButtons                      "L_AnnGetToolBarButtonsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnFreeToolBarButtons                     "L_AnnFreeToolBarButtons"
#else
   #define IDF_L_AnnFreeToolBarButtons                     "L_AnnFreeToolBarButtonsA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetToolBarInfo                         "L_AnnGetToolBarInfo"
#define IDF_L_AnnSetToolBarColumns                      "L_AnnSetToolBarColumns"
#define IDF_L_AnnSetToolBarRows                         "L_AnnSetToolBarRows"
#define IDF_L_AnnSetAutoDefaults                        "L_AnnSetAutoDefaults"
#define IDF_L_AnnSetTransparentColor                    "L_AnnSetTransparentColor"
#define IDF_L_AnnGetTransparentColor                    "L_AnnGetTransparentColor"
#define IDF_L_AnnGetUndoDepth                           "L_AnnGetUndoDepth"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGroup                                  "L_AnnGroup"
#else
   #define IDF_L_AnnGroup                                  "L_AnnGroupA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnUngroup                                "L_AnnUngroup"
#else
   #define IDF_L_AnnUngroup                                "L_AnnUngroupA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetAutoOptions                         "L_AnnSetAutoOptions"
#define IDF_L_AnnGetAutoOptions                         "L_AnnGetAutoOptions"
#define IDF_L_AnnGetObjectFromTag                       "L_AnnGetObjectFromTag"
#define IDF_L_AnnGetRgnHandle                           "L_AnnGetRgnHandle"
#define IDF_L_AnnGetArea                                "L_AnnGetArea"
#define IDF_L_AnnSetAutoDialogFontSize                  "L_AnnSetAutoDialogFontSize"
#define IDF_L_AnnGetAutoDialogFontSize                  "L_AnnGetAutoDialogFontSize"
#define IDF_L_AnnSetGrouping                            "L_AnnSetGrouping"
#define IDF_L_AnnGetGrouping                            "L_AnnGetGrouping"
#define IDF_L_AnnSetAutoBackColor                       "L_AnnSetAutoBackColor"
#define IDF_L_AnnGetAutoBackColor                       "L_AnnGetAutoBackColor"
#define IDF_L_AnnAddUndoNode                            "L_AnnAddUndoNode"
#define IDF_L_AnnSetAutoUndoEnable                      "L_AnnSetAutoUndoEnable"
#define IDF_L_AnnGetAutoUndoEnable                      "L_AnnGetAutoUndoEnable"
#define IDF_L_AnnSetToolBarParent                       "L_AnnSetToolBarParent"
#define IDF_L_AnnSetEncryptOptions                      "L_AnnSetEncryptOptions"
#define IDF_L_AnnGetEncryptOptions                      "L_AnnGetEncryptOptions"
#define IDF_L_AnnEncryptApply                           "L_AnnEncryptApply"
#define IDF_L_AnnSetPredefinedBitmap                    "L_AnnSetPredefinedBitmap"
#define IDF_L_AnnGetPredefinedBitmap                    "L_AnnGetPredefinedBitmap"
#define IDF_L_AnnGetPointOptions                        "L_AnnGetPointOptions"
#define IDF_L_AnnSetPointOptions                        "L_AnnSetPointOptions"
#define IDF_L_AnnAddUserHandle                          "L_AnnAddUserHandle"
#define IDF_L_AnnGetUserHandle                          "L_AnnGetUserHandle"
#define IDF_L_AnnGetUserHandles                         "L_AnnGetUserHandles"
#define IDF_L_AnnChangeUserHandle                       "L_AnnChangeUserHandle"
#define IDF_L_AnnDeleteUserHandle                       "L_AnnDeleteUserHandle"
#define IDF_L_AnnEnumerateHandles                       "L_AnnEnumerateHandles"
#define IDF_L_AnnDebug                                  "L_AnnDebug"
#define IDF_L_AnnDumpObject                             "L_AnnDumpObject"
#define IDF_L_AnnHitTest                                "L_AnnHitTest"
#define IDF_L_AnnGetRotateAngle                         "L_AnnGetRotateAngle"
#define IDF_L_AnnAdjustPoint                            "L_AnnAdjustPoint"
#define IDF_L_AnnConvert                                "L_AnnConvert"
#define IDF_L_AnnRestrictCursor                         "L_AnnRestrictCursor"
#define IDF_L_AnnSetRestrictToContainer                 "L_AnnSetRestrictToContainer"
#define IDF_L_AnnGetRestrictToContainer                 "L_AnnGetRestrictToContainer"
#define IDF_L_AnnDefine2                                "L_AnnDefine2"
// Text Token Table Functions
#if defined(FOR_UNICODE)
   #define IDF_L_AnnInsertTextTokenTable                   "L_AnnInsertTextTokenTable"
#else
   #define IDF_L_AnnInsertTextTokenTable                   "L_AnnInsertTextTokenTableA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnEnumerateTextTokenTable                "L_AnnEnumerateTextTokenTable"
#else
   #define IDF_L_AnnEnumerateTextTokenTable                "L_AnnEnumerateTextTokenTableA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnDeleteTextTokenTable                   "L_AnnDeleteTextTokenTable"
#else
   #define IDF_L_AnnDeleteTextTokenTable                   "L_AnnDeleteTextTokenTableA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnClearTextTokenTable                    "L_AnnClearTextTokenTable"
// Fixed Annotation Functions
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetNoScroll                            "L_AnnSetNoScroll"
#else
   #define IDF_L_AnnSetNoScroll                            "L_AnnSetNoScrollA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetNoScroll                            "L_AnnGetNoScroll"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetNoZoom                              "L_AnnSetNoZoom"
#else
   #define IDF_L_AnnSetNoZoom                              "L_AnnSetNoZoomA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnGetNoZoom                              "L_AnnGetNoZoom"
#define IDF_L_AnnEnableFixed                            "L_AnnEnableFixed"
#define IDF_L_AnnGetFixed                               "L_AnnGetFixed"
#define IDF_L_AnnSetFixed                               "L_AnnSetFixed"
#define IDF_L_AnnPushFixedState                         "L_AnnPushFixedState"
#define IDF_L_AnnPopFixedState                          "L_AnnPopFixedState"
#define IDF_L_AnnIsFixedInRect                          "L_AnnIsFixedInRect"
#define IDF_L_AnnGetDistance2                           "L_AnnGetDistance2"
#define IDF_L_AnnDumpSmartDistance                      "L_AnnDumpSmartDistance"
#define IDF_L_AnnSetAutoCursor                          "L_AnnSetAutoCursor"
#define IDF_L_AnnGetAutoCursor                          "L_AnnGetAutoCursor"
#define IDF_L_AnnSetUserData                            "L_AnnSetUserData"
#define IDF_L_AnnGetUserData                            "L_AnnGetUserData"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetTextRTF                             "L_AnnSetTextRTF"
#else
   #define IDF_L_AnnSetTextRTF                             "L_AnnSetTextRTFA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AnnGetTextRTF                             "L_AnnGetTextRTF"
#else
   #define IDF_L_AnnGetTextRTF                             "L_AnnGetTextRTFA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_DebugEnableWorldTransform                 "L_DebugEnableWorldTransform"
#define IDF_L_DebugSetGlobal                            "L_DebugSetGlobal"
#define IDF_L_DebugGetGlobal                            "L_DebugGetGlobal"
#if defined(FOR_UNICODE)
   #define IDF_L_AnnSetlocale                              "L_AnnSetlocale"
#else
   #define IDF_L_AnnSetlocale                              "L_AnnSetlocaleA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AnnSetAutoHilightPen                      "L_AnnSetAutoHilightPen"
#define IDF_L_AnnSetOptions                             "L_AnnSetOptions"
#define IDF_L_AnnGetOptions                             "L_AnnGetOptions"
#define IDF_L_AnnGetRotateOptions                       "L_AnnGetRotateOptions"
#define IDF_L_AnnSetRotateOptions                       "L_AnnSetRotateOptions"
#define IDF_L_AnnCalibrateRuler                         "L_AnnCalibrateRuler"
#define IDF_L_AnnTextEdit                               "L_AnnTextEdit"
#define IDF_L_AnnSetTextOptions                         "L_AnnSetTextOptions"
#define IDF_L_AnnGetTextOptions                         "L_AnnGetTextOptions"
#define IDF_L_AnnGetAutoSnapCursor                      "L_AnnGetAutoSnapCursor"
#define IDF_L_AnnSetAutoSnapCursor                      "L_AnnSetAutoSnapCursor"
#define IDF_L_AnnSetTextFixedSize                       "L_AnnSetTextFixedSize"
#define IDF_L_AnnGetTextFixedSize                       "L_AnnGetTextFixedSize"
#define IDF_L_AnnGetLineFixedWidth                      "L_AnnGetLineFixedWidth"
#define IDF_L_AnnSetLineFixedWidth                      "L_AnnSetLineFixedWidth"
#define IDF_L_AnnGetPointerOptions                      "L_AnnGetPointerOptions"
#define IDF_L_AnnSetPointerOptions                      "L_AnnSetPointerOptions"
#define IDF_L_AnnSetRenderMode                          "L_AnnSetRenderMode"
#define IDF_L_AnnGetShowStampBorder                     "L_AnnGetShowStampBorder"
#define IDF_L_AnnSetShowStampBorder                     "L_AnnSetShowStampBorder"

//-----------------------------------------------------------------------------
//--LTSCR.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_SetCaptureOption                          "L_SetCaptureOption"
#define IDF_L_GetCaptureOption                          "L_GetCaptureOption"
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureWindow                             "L_CaptureWindow"
#else
   #define IDF_L_CaptureWindow                             "L_CaptureWindowA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureActiveWindow                       "L_CaptureActiveWindow"
#else
   #define IDF_L_CaptureActiveWindow                       "L_CaptureActiveWindowA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureActiveClient                       "L_CaptureActiveClient"
#else
   #define IDF_L_CaptureActiveClient                       "L_CaptureActiveClientA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureWallPaper                          "L_CaptureWallPaper"
#else
   #define IDF_L_CaptureWallPaper                          "L_CaptureWallPaperA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureFullScreen                         "L_CaptureFullScreen"
#else
   #define IDF_L_CaptureFullScreen                         "L_CaptureFullScreenA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureMenuUnderCursor                    "L_CaptureMenuUnderCursor"
#else
   #define IDF_L_CaptureMenuUnderCursor                    "L_CaptureMenuUnderCursorA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureWindowUnderCursor                  "L_CaptureWindowUnderCursor"
#else
   #define IDF_L_CaptureWindowUnderCursor                  "L_CaptureWindowUnderCursorA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureSelectedObject                     "L_CaptureSelectedObject"
#else
   #define IDF_L_CaptureSelectedObject                     "L_CaptureSelectedObjectA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureArea                               "L_CaptureArea"
#else
   #define IDF_L_CaptureArea                               "L_CaptureAreaA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureMouseCursor                        "L_CaptureMouseCursor"
#else
   #define IDF_L_CaptureMouseCursor                        "L_CaptureMouseCursorA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_CaptureSetHotKeyCallback                  "L_CaptureSetHotKeyCallback"
#define IDF_L_GetDefaultAreaOption                      "L_GetDefaultAreaOption"
#define IDF_L_GetDefaultObjectOption                    "L_GetDefaultObjectOption"
#define IDF_L_StopCapture                               "L_StopCapture"
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureFromExe                            "L_CaptureFromExe"
#else
   #define IDF_L_CaptureFromExe                            "L_CaptureFromExeA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureGetResCount                        "L_CaptureGetResCount"
#else
   #define IDF_L_CaptureGetResCount                        "L_CaptureGetResCountA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_IsCaptureActive                           "L_IsCaptureActive"
#define IDF_L_SetCaptureOptionDlg                       "L_SetCaptureOptionDlg"
#define IDF_L_CaptureAreaOptionDlg                      "L_CaptureAreaOptionDlg"
#define IDF_L_CaptureObjectOptionDlg                    "L_CaptureObjectOptionDlg"
#if defined(FOR_UNICODE)
   #define IDF_L_CaptureFromExeDlg                         "L_CaptureFromExeDlg"
#else
   #define IDF_L_CaptureFromExeDlg                         "L_CaptureFromExeDlgA"
#endif //#if defined(FOR_UNICODE)
//-----------------------------------------------------------------------------
//--LTNET.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_InetConnect                               "L_InetConnect"
#define IDF_L_InetServerInit                            "L_InetServerInit"
#define IDF_L_InetClose                                 "L_InetClose"
#define IDF_L_InetSendData                              "L_InetSendData"
#define IDF_L_InetSendMMData                            "L_InetSendMMData"
#define IDF_L_InetReadData                              "L_InetReadData"
#define IDF_L_InetGetHostName                           "L_InetGetHostName"
#define IDF_L_InetAcceptConnect                         "L_InetAcceptConnect"
#define IDF_L_InetSendBitmap                            "L_InetSendBitmap"
#define IDF_L_InetAutoProcess                           "L_InetAutoProcess"
#define IDF_L_InetSendRawData                           "L_InetSendRawData"
#define IDF_L_InetGetQueueSize                          "L_InetGetQueueSize"
#define IDF_L_InetClearQueue                            "L_InetClearQueue"
#define IDF_L_InetStartUp                               "L_InetStartUp"
#define IDF_L_InetShutDown                              "L_InetShutDown"
#define IDF_L_InetSendSound                             "L_InetSendSound"
#define IDF_L_InetAttachToSocket                        "L_InetAttachToSocket"
#define IDF_L_InetDetachFromSocket                      "L_InetDetachFromSocket"
#define IDF_L_InetSetCallback                           "L_InetSetCallback"
#define IDF_L_InetGetCallback                           "L_InetGetCallback"
#define IDF_L_InetCreatePacket                          "L_InetCreatePacket"
#define IDF_L_InetCreatePacketFromParams                "L_InetCreatePacketFromParams"
#define IDF_L_InetFreePacket                            "L_InetFreePacket"
#define IDF_L_InetSendCmd                               "L_InetSendCmd"
#define IDF_L_InetSendRsp                               "L_InetSendRsp"
#if defined(FOR_UNICODE)
   #define IDF_L_InetSendLoadCmd                           "L_InetSendLoadCmd"
#else
   #define IDF_L_InetSendLoadCmd                           "L_InetSendLoadCmdA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_InetSendLoadRsp                           "L_InetSendLoadRsp"
#if defined(FOR_UNICODE)
   #define IDF_L_InetSendSaveCmd                           "L_InetSendSaveCmd"
#else
   #define IDF_L_InetSendSaveCmd                           "L_InetSendSaveCmdA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_InetSendSaveRsp                           "L_InetSendSaveRsp"
#if defined(FOR_UNICODE)
   #define IDF_L_InetSendCreateWinCmd                      "L_InetSendCreateWinCmd"
#else
   #define IDF_L_InetSendCreateWinCmd                      "L_InetSendCreateWinCmdA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_InetSendCreateWinRsp                      "L_InetSendCreateWinRsp"
#define IDF_L_InetSendSizeWinCmd                        "L_InetSendSizeWinCmd"
#define IDF_L_InetSendSizeWinRsp                        "L_InetSendSizeWinRsp"
#define IDF_L_InetSendShowWinCmd                        "L_InetSendShowWinCmd"
#define IDF_L_InetSendShowWinRsp                        "L_InetSendShowWinRsp"
#define IDF_L_InetSendCloseWinCmd                       "L_InetSendCloseWinCmd"
#define IDF_L_InetSendCloseWinRsp                       "L_InetSendCloseWinRsp"
#define IDF_L_InetSendFreeBitmapCmd                     "L_InetSendFreeBitmapCmd"
#define IDF_L_InetSendFreeBitmapRsp                     "L_InetSendFreeBitmapRsp"
#define IDF_L_InetSendSetRectCmd                        "L_InetSendSetRectCmd"
#define IDF_L_InetSendSetRectRsp                        "L_InetSendSetRectRsp"
#define IDF_L_InetSetCommandCallback                    "L_InetSetCommandCallback"
#define IDF_L_InetSetResponseCallback                   "L_InetSetResponseCallback"
#define IDF_L_InetSendAttachBitmapCmd                   "L_InetSendAttachBitmapCmd"
#define IDF_L_InetSendAttachBitmapRsp                   "L_InetSendAttachBitmapRsp"
#define IDF_L_InetSendGetMagGlassDataCmd                "L_InetSendGetMagGlassDataCmd"
#define IDF_L_InetSendGetMagGlassDataRsp                "L_InetSendGetMagGlassDataRsp"
#define IDF_L_InetGetMagGlassData                       "L_InetGetMagGlassData"
#define IDF_L_InetGetParameters                         "L_InetGetParameters"
#define IDF_L_InetCopyParameters                        "L_InetCopyParameters"
#define IDF_L_InetFreeParameters                        "L_InetFreeParameters"

//-----------------------------------------------------------------------------
//--LTWEB.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpConnect                            "L_InetFtpConnect"
#else
   #define IDF_L_InetFtpConnect                            "L_InetFtpConnectA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_InetFtpDisConnect                         "L_InetFtpDisConnect"
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpSendFile                           "L_InetFtpSendFile"
#else
   #define IDF_L_InetFtpSendFile                           "L_InetFtpSendFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpChangeDir                          "L_InetFtpChangeDir"
#else
   #define IDF_L_InetFtpChangeDir                          "L_InetFtpChangeDirA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpGetFile                            "L_InetFtpGetFile"
#else
   #define IDF_L_InetFtpGetFile                            "L_InetFtpGetFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpRenameFile                         "L_InetFtpRenameFile"
#else
   #define IDF_L_InetFtpRenameFile                         "L_InetFtpRenameFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpDeleteFile                         "L_InetFtpDeleteFile"
#else
   #define IDF_L_InetFtpDeleteFile                         "L_InetFtpDeleteFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpCreateDir                          "L_InetFtpCreateDir"
#else
   #define IDF_L_InetFtpCreateDir                          "L_InetFtpCreateDirA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpDeleteDir                          "L_InetFtpDeleteDir"
#else
   #define IDF_L_InetFtpDeleteDir                          "L_InetFtpDeleteDirA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpGetCurrentDir                      "L_InetFtpGetCurrentDir"
#else
   #define IDF_L_InetFtpGetCurrentDir                      "L_InetFtpGetCurrentDirA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpSendBitmap                         "L_InetFtpSendBitmap"
#else
   #define IDF_L_InetFtpSendBitmap                         "L_InetFtpSendBitmapA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetFtpBrowseDir                          "L_InetFtpBrowseDir"
#else
   #define IDF_L_InetFtpBrowseDir                          "L_InetFtpBrowseDirA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetHttpConnect                           "L_InetHttpConnect"
#else
   #define IDF_L_InetHttpConnect                           "L_InetHttpConnectA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_InetHttpDisconnect                        "L_InetHttpDisconnect"
#if defined(FOR_UNICODE)
   #define IDF_L_InetHttpOpenRequest                       "L_InetHttpOpenRequest"
#else
   #define IDF_L_InetHttpOpenRequest                       "L_InetHttpOpenRequestA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetHttpOpenRequestEx                     "L_InetHttpOpenRequestEx"
#else
   #define IDF_L_InetHttpOpenRequestEx                     "L_InetHttpOpenRequestExA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_InetHttpCloseRequest                      "L_InetHttpCloseRequest"
#if defined(FOR_UNICODE)
   #define IDF_L_InetHttpSendRequest                       "L_InetHttpSendRequest"
#else
   #define IDF_L_InetHttpSendRequest                       "L_InetHttpSendRequestA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetHttpSendBitmap                        "L_InetHttpSendBitmap"
#else
   #define IDF_L_InetHttpSendBitmap                        "L_InetHttpSendBitmapA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetHttpSendData                          "L_InetHttpSendData"
#else
   #define IDF_L_InetHttpSendData                          "L_InetHttpSendDataA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_InetHttpSendForm                          "L_InetHttpSendForm"
#else
   #define IDF_L_InetHttpSendForm                          "L_InetHttpSendFormA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_InetHttpGetResponse                       "L_InetHttpGetResponse"
#define IDF_L_InetHttpGetServerStatus                   "L_InetHttpGetServerStatus"

//-----------------------------------------------------------------------------
//--LTDLG.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_DlgInit                                   "L_DlgInit"
#define IDF_L_DlgFree                                   "L_DlgFree"

#define IDF_L_DlgSetFont                                "L_DlgSetFont"
#define IDF_L_DlgGetStringLen                           "L_DlgGetStringLen"
#if defined(FOR_UNICODE)
   #define IDF_L_DlgSetString                              "L_DlgSetString"
#else
   #define IDF_L_DlgSetString                              "L_DlgSetStringA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_DlgGetString                              "L_DlgGetString"
#else
   #define IDF_L_DlgGetString                              "L_DlgGetStringA"
#endif //#if defined(FOR_UNICODE)
//{{ Color dialogs C DLL's group - LTDlgClr14?.dll

#define IDF_L_DlgBalanceColors                          "L_DlgBalanceColors"
#define IDF_L_DlgColoredGray                            "L_DlgColoredGray"
#define IDF_L_DlgGrayScale                              "L_DlgGrayScale"
#define IDF_L_DlgRemapIntensity                         "L_DlgRemapIntensity"
#define IDF_L_DlgRemapHue                               "L_DlgRemapHue"
#define IDF_L_DlgCustomizePalette                       "L_DlgCustomizePalette"
#define IDF_L_DlgLocalHistoEqualize                     "L_DlgLocalHistoEqualize"
#define IDF_L_DlgIntensityDetect                        "L_DlgIntensityDetect"
#define IDF_L_DlgSolarize                               "L_DlgSolarize"
#define IDF_L_DlgPosterize                              "L_DlgPosterize"
#define IDF_L_DlgBrightness                             "L_DlgBrightness"
#define IDF_L_DlgContrast                               "L_DlgContrast"
#define IDF_L_DlgHue                                    "L_DlgHue"
#define IDF_L_DlgSaturation                             "L_DlgSaturation"
#define IDF_L_DlgGammaAdjustment                        "L_DlgGammaAdjustment"
#if defined(FOR_UNICODE)
   #define IDF_L_DlgHalftone                               "L_DlgHalftone"
#else
   #define IDF_L_DlgHalftone                               "L_DlgHalftoneA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_DlgColorRes                               "L_DlgColorRes"
#define IDF_L_DlgHistoContrast                          "L_DlgHistoContrast"
#define IDF_L_DlgWindowLevel                            "L_DlgWindowLevel"
#define IDF_L_DlgColor                                  "L_DlgColor"
//}} Color dialogs C DLL's group - LTDlgClr14?.dll

//{{ Image Effects dialogs C DLL's group - LTDlgImgEfx14?.dll

#define IDF_L_DlgMotionBlur                             "L_DlgMotionBlur"
#define IDF_L_DlgRadialBlur                             "L_DlgRadialBlur"
#define IDF_L_DlgZoomBlur                               "L_DlgZoomBlur"
#define IDF_L_DlgGaussianBlur                           "L_DlgGaussianBlur"
#define IDF_L_DlgAntiAlias                              "L_DlgAntiAlias"
#define IDF_L_DlgAverage                                "L_DlgAverage"
#define IDF_L_DlgMedian                                 "L_DlgMedian"
#define IDF_L_DlgAddNoise                               "L_DlgAddNoise"
#define IDF_L_DlgMaxFilter                              "L_DlgMaxFilter"
#define IDF_L_DlgMinFilter                              "L_DlgMinFilter"
#define IDF_L_DlgSharpen                                "L_DlgSharpen"
#define IDF_L_DlgShiftDifferenceFilter                  "L_DlgShiftDifferenceFilter"
#define IDF_L_DlgEmboss                                 "L_DlgEmboss"
#define IDF_L_DlgOilify                                 "L_DlgOilify"
#define IDF_L_DlgMosaic                                 "L_DlgMosaic"
#define IDF_L_DlgErosionFilter                          "L_DlgErosionFilter"
#define IDF_L_DlgDilationFilter                         "L_DlgDilationFilter"
#define IDF_L_DlgContourFilter                          "L_DlgContourFilter"
#define IDF_L_DlgGradientFilter                         "L_DlgGradientFilter"
#define IDF_L_DlgLaplacianFilter                        "L_DlgLaplacianFilter"
#define IDF_L_DlgSobelFilter                            "L_DlgSobelFilter"
#define IDF_L_DlgPrewittFilter                          "L_DlgPrewittFilter"
#define IDF_L_DlgLineSegmentFilter                      "L_DlgLineSegmentFilter"
#define IDF_L_DlgUnsharpMask                            "L_DlgUnsharpMask"
#define IDF_L_DlgMultiply                               "L_DlgMultiply"
#if defined(FOR_UNICODE)
   #define IDF_L_DlgAddBitmaps                             "L_DlgAddBitmaps"
#else
   #define IDF_L_DlgAddBitmaps                             "L_DlgAddBitmapsA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_DlgStitch                                 "L_DlgStitch"
#else
   #define IDF_L_DlgStitch                                 "L_DlgStitchA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_DlgFreeHandWave                           "L_DlgFreeHandWave"
#define IDF_L_DlgWind                                   "L_DlgWind"

#define IDF_L_DlgPolar                                  "L_DlgPolar"
#define IDF_L_DlgZoomWave                               "L_DlgZoomWave"
#define IDF_L_DlgRadialWave                             "L_DlgRadialWave"
#define IDF_L_DlgSwirl                                  "L_DlgSwirl"
#define IDF_L_DlgWave                                   "L_DlgWave"

#define IDF_L_DlgWaveShear                              "L_DlgWaveShear"
#define IDF_L_DlgPunch                                  "L_DlgPunch"
#define IDF_L_DlgRipple                                 "L_DlgRipple"
#define IDF_L_DlgBending                                "L_DlgBending"
#define IDF_L_DlgCylindrical                            "L_DlgCylindrical"
#define IDF_L_DlgSpherize                               "L_DlgSpherize"
#define IDF_L_DlgImpressionist                          "L_DlgImpressionist"
#define IDF_L_DlgPixelate                               "L_DlgPixelate"
#define IDF_L_DlgEdgeDetector                           "L_DlgEdgeDetector"
#if defined(FOR_UNICODE)
   #define IDF_L_DlgUnderlay                               "L_DlgUnderlay"
#else
   #define IDF_L_DlgUnderlay                               "L_DlgUnderlayA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_DlgPicturize                              "L_DlgPicturize"
#else
   #define IDF_L_DlgPicturize                              "L_DlgPicturizeA"
#endif //#if defined(FOR_UNICODE)
//}} Image Effects dialogs C DLL's group - LTDlgImgEfx14?.dll

//{{ Image dialogs  C DLL's group - LTDlgImg14?.dll

#define IDF_L_DlgRotate                                 "L_DlgRotate"
#define IDF_L_DlgShear                                  "L_DlgShear"
#define IDF_L_DlgResize                                 "L_DlgResize"
#if defined(FOR_UNICODE)
   #define IDF_L_DlgAddBorder                              "L_DlgAddBorder"
#else
   #define IDF_L_DlgAddBorder                              "L_DlgAddBorderA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_DlgAddFrame                               "L_DlgAddFrame"
#else
   #define IDF_L_DlgAddFrame                               "L_DlgAddFrameA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_DlgAutoTrim                               "L_DlgAutoTrim"
#define IDF_L_DlgCanvasResize                           "L_DlgCanvasResize"
#define IDF_L_DlgHistogram                              "L_DlgHistogram"

//}} Image dialogs C DLL's group - LTDlgImg14?.dll

//{{ Web dialogs C DLL's group - LTDlgWeb14?.dll

#define IDF_L_DlgPNGWebTuner                            "L_DlgPNGWebTuner"
#define IDF_L_DlgGIFWebTuner                            "L_DlgGIFWebTuner"
#define IDF_L_DlgJPEGWebTuner                           "L_DlgJPEGWebTuner"
#define IDF_L_DlgHTMLMapper                             "L_DlgHTMLMapper"
//}} Web dialogs C DLL's group - LTDlgWeb14?.dll


//{{ File dialogs C DLL's group - LTDlgFile14?.dll

#if defined(FOR_UNICODE)
   #define IDF_L_DlgGetDirectory                           "L_DlgGetDirectory"
#else
   #define IDF_L_DlgGetDirectory                           "L_DlgGetDirectoryA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_DlgFileConversion                         "L_DlgFileConversion"
#else
   #define IDF_L_DlgFileConversion                         "L_DlgFileConversionA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_DlgFilesAssociation                       "L_DlgFilesAssociation"
#else
   #define IDF_L_DlgFilesAssociation                       "L_DlgFilesAssociationA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_DlgPrintStitchedImages                    "L_DlgPrintStitchedImages"
#else
   #define IDF_L_DlgPrintStitchedImages                    "L_DlgPrintStitchedImagesA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_DlgPrintPreview                           "L_DlgPrintPreview"
#if defined(FOR_UNICODE)
   #define IDF_L_DlgSave                                   "L_DlgSave"
#else
   #define IDF_L_DlgSave                                   "L_DlgSaveA"
#endif //#if defined(FOR_UNICODE)


#if defined(FOR_UNICODE)
   #define IDF_L_DlgOpen                                   "L_DlgOpen"
#else
   #define IDF_L_DlgOpen                                   "L_DlgOpenA"
#endif //#if defined(FOR_UNICODE)

#if defined(FOR_UNICODE)
   #define IDF_L_DlgICCProfile                             "L_DlgICCProfile"
#else
   #define IDF_L_DlgICCProfile                             "L_DlgICCProfileA"
#endif //#if defined(FOR_UNICODE)
//}} File dialogs C DLL's group - LTDlgFile14?.dll

//{{ Effects dialogs C DLL's group - LTDlgEfx14?.dll

#define IDF_L_DlgGetShape                               "L_DlgGetShape"
#define IDF_L_DlgGetEffect                              "L_DlgGetEffect"
#define IDF_L_DlgGetTransition                          "L_DlgGetTransition"
#define IDF_L_DlgGetGradient                            "L_DlgGetGradient"
#if defined(FOR_UNICODE)
   #define IDF_L_DlgGetText                                "L_DlgGetText"
#else
   #define IDF_L_DlgGetText                                "L_DlgGetTextA"
#endif //#if defined(FOR_UNICODE)
//}} Effects dialogs C DLL's group - LTDlgEfx14?.dll

//{{ Document Image dialogs C DLL's group- LTDlgImgDoc14?.dll

#define IDF_L_DlgSmooth                                 "L_DlgSmooth"
#define IDF_L_DlgLineRemove                             "L_DlgLineRemove"
#define IDF_L_DlgBorderRemove                           "L_DlgBorderRemove"
#define IDF_L_DlgInvertedText                           "L_DlgInvertedText"
#define IDF_L_DlgDotRemove                              "L_DlgDotRemove"
#define IDF_L_DlgHolePunchRemove                        "L_DlgHolePunchRemove"

//-----------------------------------------------------------------------------
//--LTTMB.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#if defined(FOR_UNICODE)
   #define IDF_L_BrowseDir                                 "L_BrowseDir"
#else
   #define IDF_L_BrowseDir                                 "L_BrowseDirA"
#endif //#if defined(FOR_UNICODE)

//-----------------------------------------------------------------------------
//--LTLST.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_CreateImageListControl                    "L_CreateImageListControl"
#define IDF_L_UseImageListControl                       "L_UseImageListControl"

//-----------------------------------------------------------------------------
//--LVKRN.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_VecDuplicateObjectDescriptor              "L_VecDuplicateObjectDescriptor" 
//Do not remove above

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  General functions.                                                   []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define IDF_L_VecInit                                   "L_VecInit"
#define IDF_L_VecFree                                   "L_VecFree"
#define IDF_L_VecEmpty                                  "L_VecEmpty"
#define IDF_L_VecIsEmpty                                "L_VecIsEmpty"
#define IDF_L_VecCopy                                   "L_VecCopy"
#define IDF_L_VecSetDisplayOptions                      "L_VecSetDisplayOptions"
#define IDF_L_VecGetDisplayOptions                      "L_VecGetDisplayOptions"
#define IDF_L_VecInvertColors                           "L_VecInvertColors"
#define IDF_L_VecSetViewport                            "L_VecSetViewport"
#define IDF_L_VecGetViewport                            "L_VecGetViewport"
#define IDF_L_VecSetPan                                 "L_VecSetPan"
#define IDF_L_VecGetPan                                 "L_VecGetPan"
#define IDF_L_VecPaint                                  "L_VecPaint"
#define IDF_L_VecRealize                                "L_VecRealize"
#define IDF_L_VecPaintDC                                "L_VecPaintDC"
#define IDF_L_VecIs3D                                   "L_VecIs3D"
#define IDF_L_VecIsLocked                               "L_VecIsLocked"
#define IDF_L_VecSetLocked                              "L_VecSetLocked"
#define IDF_L_VecSetBackgroundColor                     "L_VecSetBackgroundColor"
#define IDF_L_VecGetBackgroundColor                     "L_VecGetBackgroundColor"
#define IDF_L_VecLogicalToPhysical                      "L_VecLogicalToPhysical"
#define IDF_L_VecPhysicalToLogical                      "L_VecPhysicalToLogical"
#define IDF_L_VecSetPalette                             "L_VecSetPalette"
#define IDF_L_VecGetPalette                             "L_VecGetPalette"
#define IDF_L_VecSetViewMode                            "L_VecSetViewMode"
#define IDF_L_VecGetViewMode                            "L_VecGetViewMode"
/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/    
/*[]  Transformation function.                                             []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecSetTranslation                         "L_VecSetTranslation"
#define IDF_L_VecGetTranslation                         "L_VecGetTranslation"
#define IDF_L_VecSetRotation                            "L_VecSetRotation"
#define IDF_L_VecGetRotation                            "L_VecGetRotation"
#define IDF_L_VecSetScale                               "L_VecSetScale"
#define IDF_L_VecGetScale                               "L_VecGetScale"
#define IDF_L_VecSetOrigin                              "L_VecSetOrigin"
#define IDF_L_VecGetOrigin                              "L_VecGetOrigin"
#define IDF_L_VecApplyTransformation                    "L_VecApplyTransformation"
#define IDF_L_VecZoomRect                               "L_VecZoomRect"
/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Attributes functions.                                                []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecSetBindVerticesMode                    "L_VecSetBindVerticesMode"
#define IDF_L_VecGetBindVerticesMode                    "L_VecGetBindVerticesMode"
#define IDF_L_VecSetParallelogram                       "L_VecSetParallelogram"
#define IDF_L_VecGetParallelogram                       "L_VecGetParallelogram"
#define IDF_L_VecEnumVertices                           "L_VecEnumVertices"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Camera functions.                                                    []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecSetCamera                              "L_VecSetCamera"
#define IDF_L_VecGetCamera                              "L_VecGetCamera"
/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Metafile functions.                                                  []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecConvertToWMF                           "L_VecConvertToWMF"
#define IDF_L_VecConvertFromWMF                         "L_VecConvertFromWMF"
#define IDF_L_VecConvertToEMF                           "L_VecConvertToEMF"
#define IDF_L_VecConvertFromEMF                         "L_VecConvertFromEMF"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Engine functions.                                                    []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecAttachToWindow                         "L_VecAttachToWindow"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Marker functions.                                                    []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecSetMarker                              "L_VecSetMarker"
#define IDF_L_VecGetMarker                              "L_VecGetMarker"
/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Unit functions.                                                      []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
/* Reserved for internal use */
#define IDF_L_VecSetUnit                                "L_VecSetUnit"
#define IDF_L_VecGetUnit                                "L_VecGetUnit"
#define IDF_L_VecConvertPointToUnit                     "L_VecConvertPointToUnit"
#define IDF_L_VecConvertPointFromUnit                   "L_VecConvertPointFromUnit"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Hit test functions.                                                  []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecSetHitTest                             "L_VecSetHitTest"
#define IDF_L_VecGetHitTest                             "L_VecGetHitTest"
#define IDF_L_VecHitTest                                "L_VecHitTest"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Polygon functions.                                                   []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecSetPolygonMode                         "L_VecSetPolygonMode"
#define IDF_L_VecGetPolygonMode                         "L_VecGetPolygonMode"
#define IDF_L_VecGetUseLights                           "L_VecGetUseLights"
#define IDF_L_VecSetAmbientColor                        "L_VecSetAmbientColor"
#define IDF_L_VecGetAmbientColor                        "L_VecGetAmbientColor"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Clipboard functions.                                                 []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecClipboardReady                         "L_VecClipboardReady"
#define IDF_L_VecCopyToClipboard                        "L_VecCopyToClipboard"
#define IDF_L_VecCopyFromClipboard                      "L_VecCopyFromClipboard"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Layer functions.                                                     []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#if defined(FOR_UNICODE)
   #define IDF_L_VecAddLayer                               "L_VecAddLayer"
#else
   #define IDF_L_VecAddLayer                               "L_VecAddLayerA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecDeleteLayer                            "L_VecDeleteLayer"
#define IDF_L_VecEmptyLayer                             "L_VecEmptyLayer"
#define IDF_L_VecCopyLayer                              "L_VecCopyLayer"
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetLayerByName                         "L_VecGetLayerByName"
#else
   #define IDF_L_VecGetLayerByName                         "L_VecGetLayerByNameA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecGetLayerCount                          "L_VecGetLayerCount"
#define IDF_L_VecGetLayerByIndex                        "L_VecGetLayerByIndex"
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetLayer                               "L_VecGetLayer"
#else
   #define IDF_L_VecGetLayer                               "L_VecGetLayerA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecFreeLayer                              "L_VecFreeLayer"
#else
   #define IDF_L_VecFreeLayer                              "L_VecFreeLayerA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecSetLayer                               "L_VecSetLayer"
#else
   #define IDF_L_VecSetLayer                               "L_VecSetLayerA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecSetActiveLayer                         "L_VecSetActiveLayer"
#define IDF_L_VecGetActiveLayer                         "L_VecGetActiveLayer"
#define IDF_L_VecEnumLayers                             "L_VecEnumLayers"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Group functions.                                                     []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#if defined(FOR_UNICODE)
   #define IDF_L_VecAddGroup                               "L_VecAddGroup"
#else
   #define IDF_L_VecAddGroup                               "L_VecAddGroupA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecDeleteGroup                            "L_VecDeleteGroup"
#define IDF_L_VecDeleteGroupClones                      "L_VecDeleteGroupClones"
#define IDF_L_VecEmptyGroup                             "L_VecEmptyGroup"
#define IDF_L_VecCopyGroup                              "L_VecCopyGroup"
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetGroupByName                         "L_VecGetGroupByName"
#else
   #define IDF_L_VecGetGroupByName                         "L_VecGetGroupByNameA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecGetGroupCount                          "L_VecGetGroupCount"
#define IDF_L_VecGetGroupByIndex                        "L_VecGetGroupByIndex"
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetGroup                               "L_VecGetGroup"
#else
   #define IDF_L_VecGetGroup                               "L_VecGetGroupA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecFreeGroup                              "L_VecFreeGroup"
#else
   #define IDF_L_VecFreeGroup                              "L_VecFreeGroupA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecSetGroup                               "L_VecSetGroup"
#else
   #define IDF_L_VecSetGroup                               "L_VecSetGroupA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecEnumGroups                             "L_VecEnumGroups"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Object functions.                                                    []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#define IDF_L_VecInitObject                             "L_VecInitObject"
#if defined(FOR_UNICODE)
   #define IDF_L_VecAddObject                              "L_VecAddObject"
#else
   #define IDF_L_VecAddObject                              "L_VecAddObjectA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecDeleteObject                           "L_VecDeleteObject"
#define IDF_L_VecCopyObject                             "L_VecCopyObject"
#define IDF_L_VecGetObject                              "L_VecGetObject"
#define IDF_L_VecFreeObject                             "L_VecFreeObject"
#define IDF_L_VecSetObject                              "L_VecSetObject"
#define IDF_L_VecExplodeObject                          "L_VecExplodeObject"
#define IDF_L_VecGetObjectParallelogram                 "L_VecGetObjectParallelogram"
#define IDF_L_VecGetObjectRect                          "L_VecGetObjectRect"
#define IDF_L_VecIsObjectInsideParallelogram            "L_VecIsObjectInsideParallelogram"
#define IDF_L_VecIsObjectInsideRect                     "L_VecIsObjectInsideRect"
#define IDF_L_VecSelectObject                           "L_VecSelectObject"
#define IDF_L_VecIsObjectSelected                       "L_VecIsObjectSelected"
#define IDF_L_VecHideObject                             "L_VecHideObject"
#define IDF_L_VecIsObjectHidden                         "L_VecIsObjectHidden"
#define IDF_L_VecEnumObjects                            "L_VecEnumObjects"
#define IDF_L_VecEnumObjectsInLayer                     "L_VecEnumObjectsInLayer"
#if defined(FOR_UNICODE)
   #define IDF_L_VecSetObjectAttributes                    "L_VecSetObjectAttributes"
#else
   #define IDF_L_VecSetObjectAttributes                    "L_VecSetObjectAttributesA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetObjectAttributes                    "L_VecGetObjectAttributes"
#else
   #define IDF_L_VecGetObjectAttributes                    "L_VecGetObjectAttributesA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecAddObjectToGroup                       "L_VecAddObjectToGroup"
#else
   #define IDF_L_VecAddObjectToGroup                       "L_VecAddObjectToGroupA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecEnumObjectsInGroup                     "L_VecEnumObjectsInGroup"
#if defined(FOR_UNICODE)
   #define IDF_L_VecSetObjectTooltip                       "L_VecSetObjectTooltip"
#else
   #define IDF_L_VecSetObjectTooltip                       "L_VecSetObjectTooltipA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetObjectTooltip                       "L_VecGetObjectTooltip"
#else
   #define IDF_L_VecGetObjectTooltip                       "L_VecGetObjectTooltipA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecShowObjectTooltip                      "L_VecShowObjectTooltip"
#else
   #define IDF_L_VecShowObjectTooltip                      "L_VecShowObjectTooltipA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecHideObjectTooltip                      "L_VecHideObjectTooltip"
#define IDF_L_VecSetObjectViewContext                   "L_VecSetObjectViewContext"
#define IDF_L_VecGetObjectViewContext                   "L_VecGetObjectViewContext"
#define IDF_L_VecRemoveObjectViewContext                "L_VecRemoveObjectViewContext"
#if defined(FOR_UNICODE)
   #define IDF_L_VecAddHyperlink                           "L_VecAddHyperlink"
#else
   #define IDF_L_VecAddHyperlink                           "L_VecAddHyperlinkA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecSetHyperlink                           "L_VecSetHyperlink"
#else
   #define IDF_L_VecSetHyperlink                           "L_VecSetHyperlinkA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetHyperlink                           "L_VecGetHyperlink"
#else
   #define IDF_L_VecGetHyperlink                           "L_VecGetHyperlinkA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecGetHyperlinkCount                      "L_VecGetHyperlinkCount"
#define IDF_L_VecGotoHyperlink                          "L_VecGotoHyperlink"
#if defined(FOR_UNICODE)
   #define IDF_L_VecSetObjectDescription                   "L_VecSetObjectDescription"
#else
   #define IDF_L_VecSetObjectDescription                   "L_VecSetObjectDescriptionA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetObjectDescription                   "L_VecGetObjectDescription"
#else
   #define IDF_L_VecGetObjectDescription                   "L_VecGetObjectDescriptionA"
#endif //#if defined(FOR_UNICODE)

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Event functions.                                                     []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#if defined(FOR_UNICODE)
   #define IDF_L_VecSetEventCallback                       "L_VecSetEventCallback"
#else
   #define IDF_L_VecSetEventCallback                       "L_VecSetEventCallbackA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecEvent                                  "L_VecEvent"

/*[]-----------------------------------------------------------------------[]*/  
/*[]                                                                       []*/  
/*[]  Font Substitution functions.                                         []*/  
/*[]                                                                       []*/  
/*[]-----------------------------------------------------------------------[]*/  
#if defined(FOR_UNICODE)
   #define IDF_L_VecSetFontMapper                          "L_VecSetFontMapper"
#else
   #define IDF_L_VecSetFontMapper                          "L_VecSetFontMapperA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecGetFontMapper                          "L_VecGetFontMapper"
#else
   #define IDF_L_VecGetFontMapper                          "L_VecGetFontMapperA"
#endif //#if defined(FOR_UNICODE)

//-----------------------------------------------------------------------------
//--LVDLG.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_VecDlgRotate                              "L_VecDlgRotate"
#define IDF_L_VecDlgScale                               "L_VecDlgScale"
#define IDF_L_VecDlgTranslate                           "L_VecDlgTranslate"
#define IDF_L_VecDlgCamera                              "L_VecDlgCamera"
#define IDF_L_VecDlgRender                              "L_VecDlgRender"
#define IDF_L_VecDlgViewMode                            "L_VecDlgViewMode"
#define IDF_L_VecDlgHitTest                             "L_VecDlgHitTest"
#define IDF_L_VecDlgEditAllLayers                       "L_VecDlgEditAllLayers"
#define IDF_L_VecDlgNewLayer                            "L_VecDlgNewLayer"
#define IDF_L_VecDlgEditLayer                           "L_VecDlgEditLayer"
#define IDF_L_VecDlgEditAllGroups                       "L_VecDlgEditAllGroups"
#define IDF_L_VecDlgNewGroup                            "L_VecDlgNewGroup"
#define IDF_L_VecDlgEditGroup                           "L_VecDlgEditGroup"
#define IDF_L_VecDlgNewObject                           "L_VecDlgNewObject"
#define IDF_L_VecDlgEditObject                          "L_VecDlgEditObject"
#define IDF_L_VecDlgObjectAttributes                    "L_VecDlgObjectAttributes"
#define IDF_L_VecDlgGetStringLen                        "L_VecDlgGetStringLen"
#if defined(FOR_UNICODE)
   #define IDF_L_VecDlgGetString                           "L_VecDlgGetString"
#else
   #define IDF_L_VecDlgGetString                           "L_VecDlgGetStringA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_VecDlgSetString                           "L_VecDlgSetString"
#else
   #define IDF_L_VecDlgSetString                           "L_VecDlgSetStringA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_VecDlgSetFont                             "L_VecDlgSetFont"

//-----------------------------------------------------------------------------
//--LTBAR.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_BarCodeRead                               "L_BarCodeRead"
#define IDF_L_BarCodeWrite                              "L_BarCodeWrite"
#define IDF_L_BarCodeFree                               "L_BarCodeFree"
#define IDF_L_BarCodeIsDuplicated                       "L_BarCodeIsDuplicated"
#define IDF_L_BarCodeGetDuplicated                      "L_BarCodeGetDuplicated"
#define IDF_L_BarCodeGetFirstDuplicated                 "L_BarCodeGetFirstDuplicated"
#define IDF_L_BarCodeGetNextDuplicated                  "L_BarCodeGetNextDuplicated"
#define IDF_L_BarCodeInit                               "L_BarCodeInit"
#define IDF_L_BarCodeExit                               "L_BarCodeExit"
#define IDF_L_BarCodeVersionInfo                        "L_BarCodeVersionInfo"
//-----------------------------------------------------------------------------
//--LTAUT.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_AutIsValid                                "L_AutIsValid"
#define IDF_L_AutInit                                   "L_AutInit"
#define IDF_L_AutCreate                                 "L_AutCreate"
#define IDF_L_AutFree                                   "L_AutFree"
#define IDF_L_AutSetUndoLevel                           "L_AutSetUndoLevel"
#define IDF_L_AutGetUndoLevel                           "L_AutGetUndoLevel"
#define IDF_L_AutCanUndo                                "L_AutCanUndo"
#define IDF_L_AutCanRedo                                "L_AutCanRedo"
#define IDF_L_AutUndo                                   "L_AutUndo"
#define IDF_L_AutRedo                                   "L_AutRedo"
#define IDF_L_AutSetUndoEnabled                         "L_AutSetUndoEnabled"
#define IDF_L_AutAddUndoNode                            "L_AutAddUndoNode"
#define IDF_L_AutSelect                                 "L_AutSelect"
#define IDF_L_AutClipboardDataReady                     "L_AutClipboardDataReady"
#define IDF_L_AutCut                                    "L_AutCut"
#define IDF_L_AutCopy                                   "L_AutCopy"
#define IDF_L_AutPaste                                  "L_AutPaste"
#define IDF_L_AutDelete                                 "L_AutDelete"
#define IDF_L_AutPrint                                  "L_AutPrint"
// Container Functions.
#define IDF_L_AutAddContainer                           "L_AutAddContainer"
#define IDF_L_AutRemoveContainer                        "L_AutRemoveContainer"
#define IDF_L_AutSetActiveContainer                     "L_AutSetActiveContainer"
#define IDF_L_AutGetActiveContainer                     "L_AutGetActiveContainer"
#define IDF_L_AutEnumContainers                         "L_AutEnumContainers"
// Painting Functionts.
#if defined(FOR_UNICODE)
   #define IDF_L_AutSetPaintProperty                       "L_AutSetPaintProperty"
#else
   #define IDF_L_AutSetPaintProperty                       "L_AutSetPaintPropertyA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_AutGetPaintProperty                       "L_AutGetPaintProperty"
#else
   #define IDF_L_AutGetPaintProperty                       "L_AutGetPaintPropertyA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_AutSetPaintBkColor                        "L_AutSetPaintBkColor"
#define IDF_L_AutGetPaintBkColor                        "L_AutGetPaintBkColor"
// Vector Functions.
#define IDF_L_AutSetVectorProperty                      "L_AutSetVectorProperty"
#define IDF_L_AutGetVectorProperty                      "L_AutGetVectorProperty"
#define IDF_L_AutEditVectorObject                       "L_AutEditVectorObject"
//Toolbar Functions.
#define IDF_L_AutSetToolbar                             "L_AutSetToolbar"
#define IDF_L_AutGetToolbar                             "L_AutGetToolbar"
#define IDF_L_AutSetCurrentTool                         "L_AutSetCurrentTool"
#define IDF_L_AutGetCurrentTool                         "L_AutGetCurrentTool"

//-----------------------------------------------------------------------------
//--LTCON.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
 // general container operations funtions.
 #define IDF_L_ContainerIsValid                         "L_ContainerIsValid"
 #define IDF_L_ContainerInit                            "L_ContainerInit" 
 #define IDF_L_ContainerCreate                          "L_ContainerCreate" 
 #define IDF_L_ContainerFree                            "L_ContainerFree" 
 #define IDF_L_ContainerUpdate                          "L_ContainerUpdate" 
 #define IDF_L_ContainerReset                           "L_ContainerReset" 
 #define IDF_L_ContainerEditObject                      "L_ContainerEditObject" 
 // setting functions.
 #define IDF_L_ContainerSetOwner                        "L_ContainerSetOwner" 
 #define IDF_L_ContainerSetMetrics                      "L_ContainerSetMetrics" 
 #define IDF_L_ContainerSetOffset                       "L_ContainerSetOffset" 
 #define IDF_L_ContainerSetScalar                       "L_ContainerSetScalar" 
 #define IDF_L_ContainerSetObjectType                   "L_ContainerSetObjectType" 
 #define IDF_L_ContainerSetObjectCursor                 "L_ContainerSetObjectCursor" 
 #define IDF_L_ContainerSetEnabled                      "L_ContainerSetEnabled" 
 #define IDF_L_ContainerSetCallback                     "L_ContainerSetCallback" 
 #define IDF_L_ContainerSetOwnerDraw                    "L_ContainerSetOwnerDraw" 
 // getting functions.
 #define IDF_L_ContainerGetOwner                        "L_ContainerGetOwner" 
 #define IDF_L_ContainerGetMetrics                      "L_ContainerGetMetrics" 
 #define IDF_L_ContainerGetOffset                       "L_ContainerGetOffset" 
 #define IDF_L_ContainerGetScalar                       "L_ContainerGetScalar" 
 #define IDF_L_ContainerGetObjectType                   "L_ContainerGetObjectType" 
 #define IDF_L_ContainerGetObjectCursor                 "L_ContainerGetObjectCursor" 
 #define IDF_L_ContainerGetCallback                     "L_ContainerGetCallback" 
 // status query functions.
 #define IDF_L_ContainerIsEnabled                       "L_ContainerIsEnabled" 
 #define IDF_L_ContainerIsOwnerDraw                     "L_ContainerIsOwnerDraw" 
 #define IDF_L_ContainerSetAutomationCallback           "L_ContainerSetAutomationCallback"
 #define IDF_L_ScreenToContainer                        "L_ScreenToContainer" 
 #define IDF_L_ContainerToScreen                        "L_ContainerToScreen" 
 #define IDF_L_ContainerEnableUpdate                    "L_ContainerEnableUpdate" 

//-----------------------------------------------------------------------------
//--LTPNT.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
   #define IDF_L_PntIsValid                             "L_PntIsValid"
   #define IDF_L_PntInit                                "L_PntInit"
   #define IDF_L_PntFree                                "L_PntFree"
   #if defined(FOR_UNICODE)
   #define IDF_L_PntSetProperty                         "L_PntSetProperty"
   #else
      #define IDF_L_PntSetProperty                         "L_PntSetPropertyA"
   #endif //#if defined(FOR_UNICODE)
   #if defined(FOR_UNICODE)
   #define IDF_L_PntGetProperty                         "L_PntGetProperty"
   #else
      #define IDF_L_PntGetProperty                         "L_PntGetPropertyA"
   #endif //#if defined(FOR_UNICODE)
   #define IDF_L_PntSetMetrics                          "L_PntSetMetrics"
   #define IDF_L_PntSetTransformation                   "L_PntSetTransformation"
   #define IDF_L_PntGetTransformation                   "L_PntGetTransformation"
   #define IDF_L_PntSetDCExtents                        "L_PntSetDCExtents"
   #define IDF_L_PntGetDCExtents                        "L_PntGetDCExtents"
   #define IDF_L_PntSetClipRgn                          "L_PntSetClipRgn"
   #define IDF_L_PntGetClipRgn                          "L_PntGetClipRgn"
   #define IDF_L_PntOffsetClipRgn                       "L_PntOffsetClipRgn"
   #define IDF_L_PntBrushMoveTo                         "L_PntBrushMoveTo"
   #define IDF_L_PntBrushLineTo                         "L_PntBrushLineTo"
   #define IDF_L_PntDrawShapeLine                       "L_PntDrawShapeLine"
   #define IDF_L_PntDrawShapeRectangle                  "L_PntDrawShapeRectangle"
   #define IDF_L_PntDrawShapeRoundRect                  "L_PntDrawShapeRoundRect"
   #define IDF_L_PntDrawShapeEllipse                    "L_PntDrawShapeEllipse"
   #define IDF_L_PntDrawShapePolygon                    "L_PntDrawShapePolygon"
   #define IDF_L_PntDrawShapePolyBezier                 "L_PntDrawShapePolyBezier"
   #define IDF_L_PntRegionRect                          "L_PntRegionRect"
   #define IDF_L_PntRegionRoundRect                     "L_PntRegionRoundRect"
   #define IDF_L_PntRegionEllipse                       "L_PntRegionEllipse"
   #define IDF_L_PntRegionPolygon                       "L_PntRegionPolygon"
   #define IDF_L_PntRegionSurface                       "L_PntRegionSurface"
   #define IDF_L_PntRegionBorder                        "L_PntRegionBorder"
   #define IDF_L_PntRegionColor                         "L_PntRegionColor"
   #define IDF_L_PntRegionTranslate                     "L_PntRegionTranslate"
   #define IDF_L_PntRegionScale                         "L_PntRegionScale"
   #define IDF_L_PntFillSurface                         "L_PntFillSurface"
   #define IDF_L_PntFillBorder                          "L_PntFillBorder"
   #define IDF_L_PntFillColorReplace                    "L_PntFillColorReplace"
   #define IDF_L_PntApplyText                           "L_PntApplyText"
   #define IDF_L_PntPickColor                           "L_PntPickColor"
   #define IDF_L_PntSetCallback                         "L_PntSetCallback"
   #define IDF_L_PntUpdateLeadDC                        "L_PntUpdateLeadDC"

//-----------------------------------------------------------------------------
//--LTTLB.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
// general toolbar operations funtions.
#define IDF_L_TBIsValid                                 "L_TBIsValid"
#define IDF_L_TBInit                                    "L_TBInit"
#define IDF_L_TBFree                                    "L_TBFree"
#if defined(FOR_UNICODE)
   #define IDF_L_TBCreate                                  "L_TBCreate"
#else
   #define IDF_L_TBCreate                                  "L_TBCreateA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TBFreeToolbarInfo                         "L_TBFreeToolbarInfo"
#else
   #define IDF_L_TBFreeToolbarInfo                         "L_TBFreeToolbarInfoA"
#endif //#if defined(FOR_UNICODE)
// status query functions.
#define IDF_L_TBIsVisible                               "L_TBIsVisible"
#define IDF_L_TBIsButtonEnabled                         "L_TBIsButtonEnabled"
#define IDF_L_TBIsButtonVisible                         "L_TBIsButtonVisible"
// setting functions.
#define IDF_L_TBSetVisible                              "L_TBSetVisible"
#define IDF_L_TBSetPosition                             "L_TBSetPosition"
#define IDF_L_TBSetRows                                 "L_TBSetRows"
#define IDF_L_TBSetButtonChecked                        "L_TBSetButtonChecked"
#define IDF_L_TBSetButtonEnabled                        "L_TBSetButtonEnabled"
#define IDF_L_TBSetButtonVisible                        "L_TBSetButtonVisible"
#if defined(FOR_UNICODE)
   #define IDF_L_TBSetToolbarInfo                          "L_TBSetToolbarInfo"
#else
   #define IDF_L_TBSetToolbarInfo                          "L_TBSetToolbarInfoA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TBSetCallback                             "L_TBSetCallback"
// getting functions.
#define IDF_L_TBGetPosition                             "L_TBGetPosition"
#define IDF_L_TBGetRows                                 "L_TBGetRows"
#define IDF_L_TBGetButtonChecked                        "L_TBGetButtonChecked"
#if defined(FOR_UNICODE)
   #define IDF_L_TBGetToolbarInfo                          "L_TBGetToolbarInfo"
#else
   #define IDF_L_TBGetToolbarInfo                          "L_TBGetToolbarInfoA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TBGetCallback                             "L_TBGetCallback"
// new functions
#if defined(FOR_UNICODE)
   #define IDF_L_TBAddButton                               "L_TBAddButton"
#else
   #define IDF_L_TBAddButton                               "L_TBAddButtonA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TBRemoveButton                            "L_TBRemoveButton"
#if defined(FOR_UNICODE)
   #define IDF_L_TBGetButtonInfo                           "L_TBGetButtonInfo"
#else
   #define IDF_L_TBGetButtonInfo                           "L_TBGetButtonInfoA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_TBSetButtonInfo                           "L_TBSetButtonInfo"
#else
   #define IDF_L_TBSetButtonInfo                           "L_TBSetButtonInfoA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_TBSetAutomationCallback                   "L_TBSetAutomationCallback"
//-----------------------------------------------------------------------------
//--LTPDG.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#if defined(FOR_UNICODE)
   #define IDF_L_PntDlgBrush                               "L_PntDlgBrush"
#else
   #define IDF_L_PntDlgBrush                               "L_PntDlgBrushA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_PntDlgShape                               "L_PntDlgShape"
#else
   #define IDF_L_PntDlgShape                               "L_PntDlgShapeA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_PntDlgRegion                              "L_PntDlgRegion"
#else
   #define IDF_L_PntDlgRegion                              "L_PntDlgRegionA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_PntDlgFill                                "L_PntDlgFill"
#else
   #define IDF_L_PntDlgFill                                "L_PntDlgFillA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_PntDlgText                                "L_PntDlgText"
#else
   #define IDF_L_PntDlgText                                "L_PntDlgTextA"
#endif //#if defined(FOR_UNICODE)
//-----------------------------------------------------------------------------
//--LTSGM.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_MrcStartBitmapSegmentation                "L_MrcStartBitmapSegmentation"
#define IDF_L_MrcStopBitmapSegmentation                 "L_MrcStopBitmapSegmentation"
#define IDF_L_MrcSegmentBitmap                          "L_MrcSegmentBitmap"
#define IDF_L_MrcCreateNewSegment                       "L_MrcCreateNewSegment"
#define IDF_L_MrcEnumSegments                           "L_MrcEnumSegments"
#define IDF_L_MrcSetSegmentData                         "L_MrcSetSegmentData"
#define IDF_L_MrcDeleteSegment                          "L_MrcDeleteSegment"
#define IDF_L_MrcCombineSegments                        "L_MrcCombineSegments"
#define IDF_L_MrcCopySegmentationHandle                 "L_MrcCopySegmentationHandle"
#if defined(FOR_UNICODE)
   #define IDF_L_MrcSaveBitmap                             "L_MrcSaveBitmap"
#else
   #define IDF_L_MrcSaveBitmap                             "L_MrcSaveBitmapA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_MrcSaveBitmapT44                          "L_MrcSaveBitmapT44"
#else
   #define IDF_L_MrcSaveBitmapT44                          "L_MrcSaveBitmapT44A"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_MrcLoadBitmap                             "L_MrcLoadBitmap"
#else
   #define IDF_L_MrcLoadBitmap                             "L_MrcLoadBitmapA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_MrcGetPagesCount                          "L_MrcGetPagesCount"
#else
   #define IDF_L_MrcGetPagesCount                          "L_MrcGetPagesCountA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_MrcSaveSegmentation                       "L_MrcSaveSegmentation"
#else
   #define IDF_L_MrcSaveSegmentation                       "L_MrcSaveSegmentationA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_MrcLoadSegmentation                       "L_MrcLoadSegmentation"
#else
   #define IDF_L_MrcLoadSegmentation                       "L_MrcLoadSegmentationA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_MrcSaveBitmapList                         "L_MrcSaveBitmapList"
#else
   #define IDF_L_MrcSaveBitmapList                         "L_MrcSaveBitmapListA"
#endif //#if defined(FOR_UNICODE)
//-----------------------------------------------------------------------------
//--LTZMV.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_WindowHasZoomView                         "L_WindowHasZoomView"
#define IDF_L_CreateZoomView                            "L_CreateZoomView"
#define IDF_L_GetZoomViewProps                          "L_GetZoomViewProps"
#define IDF_L_UpdateZoomView                            "L_UpdateZoomView"
#define IDF_L_DestroyZoomView                           "L_DestroyZoomView"
#define IDF_L_GetZoomViewsCount                         "L_GetZoomViewsCount"
#define IDF_L_RenderZoomView                            "L_RenderZoomView"
#define IDF_L_StartZoomViewAnnEdit                      "L_StartZoomViewAnnEdit"
#define IDF_L_StopZoomViewAnnEdit                       "L_StopZoomViewAnnEdit"

//-----------------------------------------------------------------------------
//--LTIMGOPT.H FUNCTIONS MACROS-------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_OptGetDefaultOptions                      "L_OptGetDefaultOptions"
#define IDF_L_OptOptimizeBuffer                         "L_OptOptimizeBuffer"
#if defined(FOR_UNICODE)
   #define IDF_L_OptOptimizeDir                            "L_OptOptimizeDir"
#else
   #define IDF_L_OptOptimizeDir                            "L_OptOptimizeDirA"
#endif //#if defined(FOR_UNICODE)

//-----------------------------------------------------------------------------
//--LPDFComp.H FUNCTIONS MACROS----------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_PdfCompInit                               "L_PdfCompInit"
#define IDF_L_PdfCompFree                               "L_PdfCompFree"
#if defined(FOR_UNICODE)
   #define IDF_L_PdfCompWrite                              "L_PdfCompWrite"
#else
   #define IDF_L_PdfCompWrite                              "L_PdfCompWriteA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_PdfCompSetCompression                     "L_PdfCompSetCompression"
#define IDF_L_PdfCompInsertMRC                          "L_PdfCompInsertMRC"
#define IDF_L_PdfCompInsertNormal                       "L_PdfCompInsertNormal"
#define IDF_L_PdfCompInsertSegments                     "L_PdfCompInsertSegments"

//-----------------------------------------------------------------------------
//--LTIVW.H FUNCTIONS MACROS--------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_DispContainerCreate                       "L_DispContainerCreate"
#define IDF_L_DispContainerGetHandle                    "L_DispContainerGetHandle"
#define IDF_L_DispContainerGetWindowHandle              "L_DispContainerGetWindowHandle"
#define IDF_L_DispContainerDestroy                      "L_DispContainerDestroy"
#define IDF_L_DispContainerSetProperties                "L_DispContainerSetProperties"
#define IDF_L_DispContainerGetProperties                "L_DispContainerGetProperties"
#define IDF_L_DispContainerInsertCell                   "L_DispContainerInsertCell"
#define IDF_L_DispContainerRemoveCell                   "L_DispContainerRemoveCell"
#define IDF_L_DispContainerGetCellCount                 "L_DispContainerGetCellCount"
#define IDF_L_DispContainerGetCellWindowHandle          "L_DispContainerGetCellWindowHandle"
#define IDF_L_DispContainerSetCellBitmapList            "L_DispContainerSetCellBitmapList"
#define IDF_L_DispContainerAddAction                    "L_DispContainerAddAction"
#define IDF_L_DispContainerSetAction                    "L_DispContainerSetAction"
#if defined(FOR_UNICODE)
   #define IDF_L_DispContainerSetCellTag                   "L_DispContainerSetCellTag"
   #define IDF_L_DispContainerSaveAnnotation               "L_DispContainerSaveAnnotation"
   #define IDF_L_DispContainerLoadAnnotation               "L_DispContainerLoadAnnotation"
   #define IDF_L_DispContainerLoadRegion                   "L_DispContainerLoadRegion"
   #define IDF_L_DispContainerSaveRegion                   "L_DispContainerSaveRegion"
   #define IDF_L_DispContainerSetSubCellTag                "L_DispContainerSetSubCellTag"
#else
   #define IDF_L_DispContainerSetCellTag                   "L_DispContainerSetCellTagA"
   #define IDF_L_DispContainerSaveAnnotation               "L_DispContainerSaveAnnotationA"
   #define IDF_L_DispContainerLoadAnnotation               "L_DispContainerLoadAnnotationA"
   #define IDF_L_DispContainerLoadRegion                   "L_DispContainerLoadRegionA"
   #define IDF_L_DispContainerSaveRegion                   "L_DispContainerSaveRegionA"
   #define IDF_L_DispContainerSetSubCellTag                "L_DispContainerSetSubCellTagA"
#endif //#if defined(FOR_UNICODE)


#define IDF_L_DispContainerSetCellProperties            "L_DispContainerSetCellProperties"
#define IDF_L_DispContainerGetCellProperties            "L_DispContainerGetCellProperties"
#define IDF_L_DispContainerGetCellPosition              "L_DispContainerGetCellPosition"
#define IDF_L_DispContainerRepositionCell               "L_DispContainerRepositionCell"
#define IDF_L_DispContainerGetCellBitmapList            "L_DispContainerGetCellBitmapList"
#define IDF_L_DispContainerGetCellBounds                "L_DispContainerGetCellBounds"
#define IDF_L_DispContainerFreezeCell                   "L_DispContainerFreezeCell"
#define IDF_L_DispContainerSetFirstVisibleRow           "L_DispContainerSetFirstVisibleRow"
#define IDF_L_DispContainerGetFirstVisibleRow           "L_DispContainerGetFirstVisibleRow"
#define IDF_L_DispContainerSetActionProperties          "L_DispContainerSetActionProperties"
#define IDF_L_DispContainerGetActionProperties          "L_DispContainerGetActionProperties"
#define IDF_L_DispContainerRemoveAction                 "L_DispContainerRemoveAction"
#define IDF_L_DispContainerGetActionCount               "L_DispContainerGetActionCount"
#define IDF_L_DispContainerSetKeyboardAction            "L_DispContainerSetKeyboardAction"
#define IDF_L_DispContainerSetBounds                    "L_DispContainerSetBounds"
#define IDF_L_DispContainerGetBounds                    "L_DispContainerGetBounds"
#define IDF_L_DispContainerSelectCell                   "L_DispContainerSelectCell"
#define IDF_L_DispContainerIsCellSelected               "L_DispContainerIsCellSelected"
#define IDF_L_DispContainerSetTagCallBack               "L_DispContainerSetTagCallBack"
#define IDF_L_DispContainerSetActionCallBack            "L_DispContainerSetActionCallBack"
#define IDF_L_DispContainerGetTagCallBack               "L_DispContainerGetTagCallBack"
#define IDF_L_DispContainerGetActionCallBack            "L_DispContainerGetActionCallBack"
#define IDF_L_DispContainerIsActionActive               "L_DispContainerIsActionActive"
#define IDF_L_DispContainerGetKeyboardAction            "L_DispContainerGetKeyboardAction"
#define IDF_L_DispContainerIsCellFrozen                 "L_DispContainerIsCellFrozen"
#define IDF_L_DispContainerSetPaintCallBack             "L_DispContainerSetPaintCallBack"
#define IDF_L_DispContainerGetPaintCallBack             "L_DispContainerGetPaintCallBack"
#define IDF_L_DispContainerRepaintCell                  "L_DispContainerRepaintCell"
#define IDF_L_DispContainerGetBitmapHandle              "L_DispContainerGetBitmapHandle"
#define IDF_L_DispContainerSetBitmapHandle              "L_DispContainerSetBitmapHandle"
#define IDF_L_DispContainerSetRulerUnit                 "L_DispContainerSetRulerUnit"
#define IDF_L_DispContainerCalibrateRuler               "L_DispContainerCalibrateRuler"
#define IDF_L_DispContainerGetActionButton              "L_DispContainerGetActionButton"
#define IDF_L_DispContainerAnnToRgn                     "L_DispContainerAnnToRgn"
#define IDF_L_DispContainerIsButtonValid                "L_DispContainerIsButtonValid"
#define IDF_L_DispContainerStartAnimation               "L_DispContainerStartAnimation"
#define IDF_L_DispContainerSetAnimationProperties       "L_DispContainerSetAnimationProperties"
#define IDF_L_DispContainerGetAnimationProperties       "L_DispContainerGetAnimationProperties"
#define IDF_L_DispContainerStopAnimation                "L_DispContainerStopAnimation"
#define IDF_L_DispContainerShowTitlebar                 "L_DispContainerShowTitlebar"
#define IDF_L_DispContainerSetTitlebarProperties        "L_DispContainerSetTitlebarProperties"
#define IDF_L_DispContainerGetTitlebarProperties        "L_DispContainerGetTitlebarProperties"
#define IDF_L_DispContainerSetIconProperties            "L_DispContainerSetIconProperties"
#define IDF_L_DispContainerGetIconProperties            "L_DispContainerGetIconProperties"
#define IDF_L_DispContainerCheckTitlebarIcon            "L_DispContainerCheckTitlebarIcon"
#define IDF_L_DispContainerIsTitlebarIconChecked        "L_DispContainerIsTitlebarIconChecked"
#define IDF_L_DispContainerIsCellAnimated               "L_DispContainerIsCellAnimated"
#define IDF_L_DispContainerGetRulerUnit                 "L_DispContainerGetRulerUnit"
#define IDF_L_DispContainerIsTitlebarEnabled            "L_DispContainerIsTitlebarEnabled"
#define IDF_L_DispContainerGetSelectedAnnotationAttributes "L_DispContainerGetSelectedAnnotationAttributes"
#define IDF_L_DispContainerHandlePalette                "L_DispContainerHandlePalette"
#define IDF_L_DispContainerUpdateCellView               "L_DispContainerUpdateCellView"
//New Medical Viewer Functionalities
#define IDF_L_DispContainerRotateAnnotationContainer          "L_DispContainerRotateAnnotationContainer"
#define IDF_L_DispContainerSetAnnotationCallBack              "L_DispContainerSetAnnotationCallBack"
#define IDF_L_DispContainerGetAnnotationCallBack              "L_DispContainerGetAnnotationCallBack"
#define IDF_L_DispContainerSetRegionCallBack                  "L_DispContainerSetRegionCallBack"
#define IDF_L_DispContainerGetRegionCallBack                  "L_DispContainerGetRegionCallBack"
#define IDF_L_DispContainerSetAnnotationCreatedCallBack       "L_DispContainerSetAnnotationCreatedCallBack"
#define IDF_L_DispContainerGetAnnotationCreatedCallBack       "L_DispContainerGetAnnotationCreatedCallBack"
#define IDF_L_DispContainerSetActiveSubCellChangedCallBack    "L_DispContainerSetActiveSubCellChangedCallBack"
#define IDF_L_DispContainerGetActiveSubCellChangedCallBack    "L_DispContainerGetActiveSubCellChangedCallBack"
#define IDF_L_DispContainerGetBitmapPixel                     "L_DispContainerGetBitmapPixel"
//More Medical Viewer Functionalities
#define IDF_L_DispContainerGetCellTag                      "L_DispContainerGetCellTag"
#define IDF_L_DispContainerDeleteCellTag                   "L_DispContainerDeleteCellTag"
#define IDF_L_DispContainerEditCellTag                     "L_DispContainerEditCellTag"
#define IDF_L_DispContainerGetAnnotationContainer          "L_DispContainerGetAnnotationContainer"
#define IDF_L_DispContainerSetAnnotationContainer          "L_DispContainerSetAnnotationContainer"
#define IDF_L_DispContainerSetCellRegionHandle             "L_DispContainerSetCellRegionHandle"
#define IDF_L_DispContainerGetCellRegionHandle             "L_DispContainerGetCellRegionHandle"
#define IDF_L_DispContainerSetMouseCallBack                "L_DispContainerSetMouseCallBack"
#define IDF_L_DispContainerGetMouseCallBack                "L_DispContainerGetMouseCallBack"
#define IDF_L_DispContainerGetSubCellTag                   "L_DispContainerGetSubCellTag"
#define IDF_L_DispContainerEditSubCellTag                  "L_DispContainerEditSubCellTag"
#define IDF_L_DispContainerDeleteSubCellTag                "L_DispContainerDeleteSubCellTag"
#define IDF_L_DispContainerSetPrePaintCallBack             "L_DispContainerSetPrePaintCallBack"
#define IDF_L_DispContainerGetPrePaintCallBack             "L_DispContainerGetPrePaintCallBack"
#define IDF_L_DispContainerSetPostPaintCallBack            "L_DispContainerSetPostPaintCallBack"
#define IDF_L_DispContainerGetPostPaintCallBack            "L_DispContainerGetPostPaintCallBack"
/*#define IDF_L_DispContainerSaveAnnotationA                 "L_DispContainerSaveAnnotationA"
#define IDF_L_DispContainerLoadAnnotationA                 "L_DispContainerLoadAnnotationA"
#define IDF_L_DispContainerLoadRegionA                     "L_DispContainerLoadRegionA"
#define IDF_L_DispContainerSaveRegionA                     "L_DispContainerSaveRegionA"*/
#define IDF_L_DispContainerReverseAnnotationContainer      "L_DispContainerReverseAnnotationContainer"
#define IDF_L_DispContainerFlipAnnotationContainer         "L_DispContainerFlipAnnotationContainer"
#define IDF_L_DispContainerEnableCellLowMemoryUsage        "L_DispContainerEnableCellLowMemoryUsage"
#define IDF_L_DispContainerGetLowMemoryUsageCallBack       "L_DispContainerGetLowMemoryUsageCallBack"
#define IDF_L_DispContainerSetLowMemoryUsageCallBack       "L_DispContainerSetLowMemoryUsageCallBack"
#define IDF_L_DispContainerSetRequestedImage               "L_DispContainerSetRequestedImage"
#define IDF_L_DispContainerFreezeSubCell                   "L_DispContainerFreezeSubCell"
#define IDF_L_DispContainerIsSubCellFrozen                 "L_DispContainerIsSubCellFrozen"
#define IDF_L_DispContainerGetCellScaleMode                "L_DispContainerGetCellScaleMode"
#define IDF_L_DispContainerSetCellScale                    "L_DispContainerSetCellScale"
#define IDF_L_DispContainerSetCellScaleMode                "L_DispContainerSetCellScaleMode"
#define IDF_L_DispContainerGetCellScale                    "L_DispContainerGetCellScale"
#define IDF_L_DispContainerPrintSubCell                    "L_DispContainerPrintSubCell"
#define IDF_L_DispContainerHandlePalette                   "L_DispContainerHandlePalette"
#define IDF_L_DispContainerCalibrateCell                   "L_DispContainerCalibrateCell"
#define IDF_L_DispContainerSetBitmapListInfo               "L_DispContainerSetBitmapListInfo"
#define IDF_L_DispContainerSetAnimationStartedCallBack     "L_DispContainerSetAnimationStartedCallBack"
#define IDF_L_DispContainerSetAnimationStoppedCallBack     "L_DispContainerSetAnimationStoppedCallBack"
#define IDF_L_DispContainerGetAnimationStartedCallBack     "L_DispContainerGetAnimationStartedCallBack"
#define IDF_L_DispContainerGetAnimationStoppedCallBack     "L_DispContainerGetAnimationStoppedCallBack"
#define IDF_L_DispContainerSetDefaultWindowLevelValues     "L_DispContainerSetDefaultWindowLevelValues"
#define IDF_L_DispContainerGetDefaultWindowLevelValues     "L_DispContainerGetDefaultWindowLevelValues"
#define IDF_L_DispContainerResetWindowLevelValues          "L_DispContainerResetWindowLevelValues"
#define IDF_L_DispContainerInvertBitmap                    "L_DispContainerInvertBitmap"
#define IDF_L_DispContainerReverseBitmap                   "L_DispContainerReverseBitmap"
#define IDF_L_DispContainerFlipBitmap                      "L_DispContainerFlipBitmap"
#define IDF_L_DispContainerIsBitmapReversed                "L_DispContainerIsBitmapReversed"
#define IDF_L_DispContainerIsBitmapFlipped                 "L_DispContainerIsBitmapFlipped"
#define IDF_L_DispContainerIsBitmapInverted                "L_DispContainerIsBitmapInverted"
#define IDF_L_DispContainerRotateBitmapPerspective         "L_DispContainerRotateBitmapPerspective"
#define IDF_L_DispContainerGetRotateBitmapPerspectiveAngle "L_DispContainerGetRotateBitmapPerspectiveAngle"
#define IDF_L_DispContainerPrintCell                       "L_DispContainerPrintCell"
#define IDF_L_DispContainerRemoveBitmapRegion              "L_DispContainerRemoveBitmapRegion"
#define IDF_L_DispContainerBeginUpdate                     "L_DispContainerBeginUpdate"
#define IDF_L_DispContainerEndUpdate                       "L_DispContainerEndUpdate"

//-----------------------------------------------------------------------------
//--LTCLR.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_GetParametricCurveNumberOfParameters      "L_GetParametricCurveNumberOfParameters"
#if defined(FOR_UNICODE)
   #define IDF_L_ClrInit                                   "L_ClrInit"
#else
   #define IDF_L_ClrInit                                   "L_ClrInitA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_ClrSetConversionParams                    "L_ClrSetConversionParams"
#else
   #define IDF_L_ClrSetConversionParams                    "L_ClrSetConversionParamsA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_ClrConvertDirect                          "L_ClrConvertDirect"
#define IDF_L_ClrConvertDirectToBitmap                  "L_ClrConvertDirectToBitmap"
#define IDF_L_ClrConvert                                "L_ClrConvert"
#define IDF_L_ClrConvertToBitmap                        "L_ClrConvertToBitmap"
#define IDF_L_ClrFree                                   "L_ClrFree"
#define IDF_L_ClrIsValid                                "L_ClrIsValid"
#if defined(FOR_UNICODE)
   #define IDF_L_ClrDlg                                    "L_ClrDlg"
#else
   #define IDF_L_ClrDlg                                    "L_ClrDlgA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_FillICCProfileStructure                   "L_FillICCProfileStructure"
#if defined(FOR_UNICODE)
   #define IDF_L_FillICCProfileFromICCFile                 "L_FillICCProfileFromICCFile"
#else
   #define IDF_L_FillICCProfileFromICCFile                 "L_FillICCProfileFromICCFileA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_InitICCProfile                            "L_InitICCProfile"
#define IDF_L_FreeICCProfile                            "L_FreeICCProfile"
#define IDF_L_InitICCHeader                             "L_InitICCHeader"
#define IDF_L_SetICCCMMType                             "L_SetICCCMMType"
#define IDF_L_SetICCDeviceClass                         "L_SetICCDeviceClass"
#define IDF_L_SetICCColorSpace                          "L_SetICCColorSpace"
#define IDF_L_SetICCConnectionSpace                     "L_SetICCConnectionSpace"
#define IDF_L_SetICCPrimaryPlatform                     "L_SetICCPrimaryPlatform"
#define IDF_L_SetICCFlags                               "L_SetICCFlags"
#define IDF_L_SetICCDevManufacturer                     "L_SetICCDevManufacturer"
#define IDF_L_SetICCDevModel                            "L_SetICCDevModel"
#define IDF_L_SetICCDeviceAttributes                    "L_SetICCDeviceAttributes"
#define IDF_L_SetICCRenderingIntent                     "L_SetICCRenderingIntent"
#define IDF_L_SetICCCreator                             "L_SetICCCreator"
#define IDF_L_SetICCDateTime                            "L_SetICCDateTime"
#define IDF_L_SetICCTagData                             "L_SetICCTagData"
#define IDF_L_GetICCTagData                             "L_GetICCTagData"
#define IDF_L_CreateICCTagData                          "L_CreateICCTagData"
#define IDF_L_DeleteICCTag                              "L_DeleteICCTag"
#if defined(FOR_UNICODE)
   #define IDF_L_GenerateICCFile                           "L_GenerateICCFile"
#else
   #define IDF_L_GenerateICCFile                           "L_GenerateICCFileA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_DoubleTo2bFixed2bNumber                   "L_DoubleTo2bFixed2bNumber"
#define IDF_L_2bFixed2bNumberToDouble                   "L_2bFixed2bNumberToDouble"
#define IDF_L_DoubleToU8Fixed8Number                    "L_DoubleToU8Fixed8Number"
#define IDF_L_U8Fixed8NumberToDouble                    "L_U8Fixed8NumberToDouble"
#define IDF_L_GenerateICCPointer                        "L_GenerateICCPointer"
#define IDF_L_GetICCTagTypeSig                          "L_GetICCTagTypeSig"
#define IDF_L_FreeICCTagType                            "L_FreeICCTagType"
#define IDF_L_ConvertCLUTToBuffer                       "L_ConvertCLUTToBuffer"
#define IDF_L_ConvertCurveTypeToBuffer                  "L_ConvertCurveTypeToBuffer"
#define IDF_L_ConvertParametricCurveTypeToBuffer        "L_ConvertParametricCurveTypeToBuffer"
#define IDF_L_SetICCProfileId                           "L_SetICCProfileId"
#if defined(FOR_UNICODE)
   #define IDF_L_LoadICCProfile                            "L_LoadICCProfile"
#else
   #define IDF_L_LoadICCProfile                            "L_LoadICCProfileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_SaveICCProfile                            "L_SaveICCProfile"
#else
   #define IDF_L_SaveICCProfile                            "L_SaveICCProfileA"
#endif //#if defined(FOR_UNICODE)

//-----------------------------------------------------------------------------
//--LTNTF.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#if defined(FOR_UNICODE)
   #define IDF_L_NITFCreate                                "L_NITFCreate"
#else
   #define IDF_L_NITFCreate                                "L_NITFCreateA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_NITFDestroy                               "L_NITFDestroy"
#define IDF_L_NITFGetStatus                             "L_NITFGetStatus"
#if defined(FOR_UNICODE)
   #define IDF_L_NITFSaveFile                              "L_NITFSaveFile"
#else
   #define IDF_L_NITFSaveFile                              "L_NITFSaveFileA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_NITFAppendImageSegment                    "L_NITFAppendImageSegment"
#define IDF_L_NITFAppendGraphicSegment                  "L_NITFAppendGraphicSegment"
#if defined(FOR_UNICODE)
   #define IDF_L_NITFAppendTextSegment                     "L_NITFAppendTextSegment"
#else
   #define IDF_L_NITFAppendTextSegment                     "L_NITFAppendTextSegmentA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_NITFSetVectorHandle                       "L_NITFSetVectorHandle"
#define IDF_L_NITFGetVectorHandle                       "L_NITFGetVectorHandle"
#define IDF_L_NITFGetNITFHeader                         "L_NITFGetNITFHeader"
#define IDF_L_NITFSetNITFHeader                         "L_NITFSetNITFHeader"
#define IDF_L_NITFGetGraphicHeaderCount                 "L_NITFGetGraphicHeaderCount"
#define IDF_L_NITFGetGraphicHeader                      "L_NITFGetGraphicHeader"
#define IDF_L_NITFSetGraphicHeader                      "L_NITFSetGraphicHeader"
#define IDF_L_NITFGetImageHeaderCount                   "L_NITFGetImageHeaderCount"
#define IDF_L_NITFGetImageHeader                        "L_NITFGetImageHeader"
#define IDF_L_NITFSetImageHeader                        "L_NITFSetImageHeader"
#define IDF_L_NITFGetTextHeaderCount                    "L_NITFGetTextHeaderCount"
#define IDF_L_NITFGetTextHeader                         "L_NITFGetTextHeader"
#define IDF_L_NITFSetTextHeader                         "L_NITFSetTextHeader"
#define IDF_L_NITFGetTextSegment                        "L_NITFGetTextSegment"
#define IDF_L_NITFFreeNITFHeader                        "L_NITFFreeNITFHeader"
#define IDF_L_NITFFreeGraphicHeader                     "L_NITFFreeGraphicHeader"
#define IDF_L_NITFFreeImageHeader                       "L_NITFFreeImageHeader"
#define IDF_L_NITFFreeTextHeader                        "L_NITFFreeTextHeader"

//-----------------------------------------------------------------------------
//--LTWIA.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define IDF_L_WiaIsAvailable                          "L_WiaIsAvailable"
#define IDF_L_WiaInitSession                          "L_WiaInitSession"
#define IDF_L_WiaEndSession                           "L_WiaEndSession"
#if defined(FOR_UNICODE)
   #define IDF_L_WiaEnumDevices                          "L_WiaEnumDevices"
#else
   #define IDF_L_WiaEnumDevices                          "L_WiaEnumDevicesA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_WiaSelectDeviceDlg                      "L_WiaSelectDeviceDlg"
#define IDF_L_WiaSelectDevice                         "L_WiaSelectDevice"
#define IDF_L_WiaGetSelectedDeviceType                "L_WiaGetSelectedDeviceType"
#if defined(FOR_UNICODE)
   #define IDF_L_WiaGetSelectedDevice                    "L_WiaGetSelectedDevice"
#else
   #define IDF_L_WiaGetSelectedDevice                    "L_WiaGetSelectedDeviceA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_WiaGetProperties                        "L_WiaGetProperties"
#define IDF_L_WiaSetProperties                        "L_WiaSetProperties"
#if defined(FOR_UNICODE)
   #define IDF_L_WiaAcquire                              "L_WiaAcquire"
#else
   #define IDF_L_WiaAcquire                              "L_WiaAcquireA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaAcquireToFile                        "L_WiaAcquireToFile"
#else
   #define IDF_L_WiaAcquireToFile                        "L_WiaAcquireToFileA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaAcquireSimple                        "L_WiaAcquireSimple"
#else
   #define IDF_L_WiaAcquireSimple                        "L_WiaAcquireSimpleA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_WiaStartVideoPreview                    "L_WiaStartVideoPreview"
#define IDF_L_WiaResizeVideoPreview                   "L_WiaResizeVideoPreview"
#define IDF_L_WiaEndVideoPreview                      "L_WiaEndVideoPreview"
#if defined(FOR_UNICODE)
   #define IDF_L_WiaAcquireImageFromVideo                "L_WiaAcquireImageFromVideo"
#else
   #define IDF_L_WiaAcquireImageFromVideo                "L_WiaAcquireImageFromVideoA"
#endif //#if defined(FOR_UNICODE)
#define IDF_L_WiaIsVideoPreviewAvailable              "L_WiaIsVideoPreviewAvailable"
#define IDF_L_WiaGetRootItem                          "L_WiaGetRootItem"
#define IDF_L_WiaEnumChildItems                       "L_WiaEnumChildItems"
#define IDF_L_WiaFreeItem                             "L_WiaFreeItem"
#if defined(FOR_UNICODE)
   #define IDF_L_WiaGetPropertyLong                      "L_WiaGetPropertyLong"
#else
   #define IDF_L_WiaGetPropertyLong                      "L_WiaGetPropertyLongA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaSetPropertyLong                      "L_WiaSetPropertyLong"
#else
   #define IDF_L_WiaSetPropertyLong                      "L_WiaSetPropertyLongA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaGetPropertyGUID                      "L_WiaGetPropertyGUID"
#else
   #define IDF_L_WiaGetPropertyGUID                      "L_WiaGetPropertyGUIDA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaSetPropertyGUID                      "L_WiaSetPropertyGUID"
#else
   #define IDF_L_WiaSetPropertyGUID                      "L_WiaSetPropertyGUIDA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaGetPropertyString                    "L_WiaGetPropertyString"
#else
   #define IDF_L_WiaGetPropertyString                    "L_WiaGetPropertyStringA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaSetPropertyString                    "L_WiaSetPropertyString"
#else
   #define IDF_L_WiaSetPropertyString                    "L_WiaSetPropertyStringA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaGetPropertySystemTime                "L_WiaGetPropertySystemTime"
#else
   #define IDF_L_WiaGetPropertySystemTime                "L_WiaGetPropertySystemTimeA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaSetPropertySystemTime                "L_WiaSetPropertySystemTime"
#else
   #define IDF_L_WiaSetPropertySystemTime                "L_WiaSetPropertySystemTimeA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaGetPropertyBuffer                    "L_WiaGetPropertyBuffer"
#else
   #define IDF_L_WiaGetPropertyBuffer                    "L_WiaGetPropertyBufferA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaSetPropertyBuffer                    "L_WiaSetPropertyBuffer"
#else
   #define IDF_L_WiaSetPropertyBuffer                    "L_WiaSetPropertyBufferA"
#endif //#if defined(FOR_UNICODE)
#if defined(FOR_UNICODE)
   #define IDF_L_WiaEnumCapabilities                     "L_WiaEnumCapabilities"
#else
   #define IDF_L_WiaEnumCapabilities                     "L_WiaEnumCapabilitiesA"
#endif //#if defined(FOR_UNICODE)

#define IDF_L_WiaEnumFormats                          "L_WiaEnumFormats"

#endif //USE_POINTERS_TO_LEAD_FUNCTIONS
#endif //_LEAD_FUNCTIONS_ID_H_
/*================================================================= EOF =====*/
