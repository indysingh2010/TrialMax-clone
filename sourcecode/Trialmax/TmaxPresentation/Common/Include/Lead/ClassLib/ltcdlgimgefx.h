/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdlgImgEfx.h                                                  |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DIALOGIMGEFX_H_
#define  _LEAD_DIALOGIMGEFX_H_


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LDialogImageEffect                                              |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2003                                                    |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LDialogImageEffect: public LDialogBase
{
   LEAD_DECLAREOBJECT(LDialogImageEffect);

   protected:  
      MOTIONBLURDLGPARAMS              m_MotionBlurDlgParam;
      RADIALBLURDLGPARAMS              m_RadialBlurDlgParam;
      ZOOMBLURDLGPARAMS                m_ZoomBlurDlgParam;
      GAUSSIANBLURDLGPARAMS            m_GaussianBlurDlgParam;
      ANTIALIASDLGPARAMS               m_AntiAliasDlgParam;
      AVERAGEDLGPARAMS                 m_AverageDlgParam;
      MEDIANDLGPARAMS                  m_MedianDlgParam;
      ADDNOISEDLGPARAMS                m_AddNoiseDlgParam;
      MAXFILTERDLGPARAMS               m_MaxFilterDlgParam;
      MINFILTERDLGPARAMS               m_MinFilterDlgParam;
      SHARPENDLGPARAMS                 m_SharpenDlgParam;
      SHIFTDIFFERENCEFILTERDLGPARAMS   m_ShiftdifferenceFilterDlgParam;
      EMBOSSDLGPARAMS                  m_EmbossDlgParam;
      OILIFYDLGPARAMS                  m_OilifyDlgParam;
      MOSAICDLGPARAMS                  m_MosaicDlgParam;
      EROSIONFILTERDLGPARAMS           m_ErosionFilterDlgParam;
      DILATIONFILTERDLGPARAMS          m_DilationFilterDlgParam;
      CONTOURFILTERDLGPARAMS           m_ContourFilterDlgParam;
      GRADIENTFILTERDLGPARAMS          m_GradientFilterDlgParam;
      LAPLACIANFILTERDLGPARAMS         m_LaplacianFilterDlgParam;
      SOBELFILTERDLGPARAMS             m_SobelFilterDlgParam;
      PREWITTFILTERDLGPARAMS           m_PrewittFilterDlgParam;
      LINESEGMENTFILTERDLGPARAMS       m_LineSegmentFilterDlgParam;
      UNSHARPMASKDLGPARAMS             m_UnsharpMaskDlgParam;
      MULTIPLYDLGPARAMS                m_MultiplyDlgParam;
      ADDBITMAPSDLGPARAMS              m_AddBitmapsDlgParam;
      STITCHDLGPARAMS                  m_StitchDlgParam;
      FREEHANDWAVEDLGPARAMS            m_FreeHandWaveDlgParam;
      WINDDLGPARAMS                    m_WindDlgParam;
      POLARDLGPARAMS                   m_PolarDlgParam;
      ZOOMWAVEDLGPARAMS                m_ZoomWaveDlgParam;
      RADIALWAVEDLGPARAMS              m_RadialWaveDlgParam;
      SWIRLDLGPARAMS                   m_SwirlDlgParam;
      WAVEDLGPARAMS                    m_WaveDlgParam;
      WAVESHEARDLGPARAMS               m_WaveShearDlgParam;
      PUNCHDLGPARAMS                   m_PunchDlgParam;
      RIPPLEDLGPARAMS                  m_RippleDlgParam;
      BENDINGDLGPARAMS                 m_BendingDlgParam;
      CYLINDRICALDLGPARAMS             m_CylindricalDlgParam;
      SPHERIZEDLGPARAMS                m_SpherizeDlgParam;
      IMPRESSIONISTDLGPARAMS           m_ImpressionistDlgParam;
      PIXELATEDLGPARAMS                m_PixelateDlgParam;
      EDGEDETECTORDLGPARAMS            m_EdgeDetectorDlgParam;
      UNDERLAYDLGPARAMS                m_UnderlayDlgParam;
      PICTURIZEDLGPARAMS               m_PicturizeDlgParam;

   private:        
      L_VOID InitializeClass();
   
   public : 
      LDialogImageEffect();
      LDialogImageEffect(LBitmapBase * pLBitmap);
      virtual ~LDialogImageEffect();

      L_INT    GetMotionBlurParams         (LPMOTIONBLURDLGPARAMS              pMotionBlurDlgParam, L_UINT uStructSize) const;
      L_INT    GetRadialBlurParams         (LPRADIALBLURDLGPARAMS              pRadialBlurDlgParam, L_UINT uStructSize) const;
      L_INT    GetZoomBlurParams           (LPZOOMBLURDLGPARAMS                pZoomBlurDlgParam, L_UINT uStructSize) const;
      L_INT    GetGaussianBlurParams       (LPGAUSSIANBLURDLGPARAMS            pGaussianBlurDlgParam, L_UINT uStructSize) const;
      L_INT    GetAntiAliasParams          (LPANTIALIASDLGPARAMS               pAntiAliasDlgParam, L_UINT uStructSize) const;
      L_INT    GetAverageParams            (LPAVERAGEDLGPARAMS                 pAverageDlgParam, L_UINT uStructSize) const;
      L_INT    GetMedianParams             (LPMEDIANDLGPARAMS                  pMedianDlgParam, L_UINT uStructSize) const;
      L_INT    GetAddNoiseParams           (LPADDNOISEDLGPARAMS                pAddNoiseDlgParam, L_UINT uStructSize) const;
      L_INT    GetMaxFilterParams          (LPMAXFILTERDLGPARAMS               pMaxFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetMinFilterParams          (LPMINFILTERDLGPARAMS               pMinFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetSharpenParams            (LPSHARPENDLGPARAMS                 pSharpenDlgParam, L_UINT uStructSize) const;
      L_INT    GetShiftDifferenceFilterParams(LPSHIFTDIFFERENCEFILTERDLGPARAMS   pShiftdifferenceFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetEmbossParams             (LPEMBOSSDLGPARAMS                  pEmbossDlgParam, L_UINT uStructSize) const;
      L_INT    GetOilifyParams             (LPOILIFYDLGPARAMS                  pOilifyDlgParam, L_UINT uStructSize) const;
      L_INT    GetMosaicParams             (LPMOSAICDLGPARAMS                  pMosaicDlgParam, L_UINT uStructSize) const;
      L_INT    GetErosionFilterParams      (LPEROSIONFILTERDLGPARAMS           pErosionFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetDilationFilterParams     (LPDILATIONFILTERDLGPARAMS          pDilationFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetContourFilterParams      (LPCONTOURFILTERDLGPARAMS           pContourFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetGradientFilterParams     (LPGRADIENTFILTERDLGPARAMS          pGradientFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetLaplacianFilterParams    (LPLAPLACIANFILTERDLGPARAMS         pLaplacianFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetSobelFilterParams        (LPSOBELFILTERDLGPARAMS             pSobelFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetPrewittFilterParams      (LPPREWITTFILTERDLGPARAMS           pPrewittFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetLineSegmentFilterParams  (LPLINESEGMENTFILTERDLGPARAMS       pLineSegmentFilterDlgParam, L_UINT uStructSize) const;
      L_INT    GetUnsharpMaskParams        (LPUNSHARPMASKDLGPARAMS             pUnsharpMaskDlgParam, L_UINT uStructSize) const;
      L_INT    GetMultiplyParams           (LPMULTIPLYDLGPARAMS                pMultiplyDlgParam, L_UINT uStructSize) const;
      L_INT    GetAddBitmapsParams         (LPADDBITMAPSDLGPARAMS              pAddBitmapsDlgParam, L_UINT uStructSize) const;
      L_INT    GetStitchParams             (LPSTITCHDLGPARAMS                  pStitchDlgParam, L_UINT uStructSize) const;
      L_INT    GetFreeHandWaveParams       (LPFREEHANDWAVEDLGPARAMS            pFreeHandWaveDlgParam, L_UINT uStructSize) const;
      L_INT    GetWindParams               (LPWINDDLGPARAMS                    pWindDlgParam, L_UINT uStructSize) const;
      L_INT    GetPolarParams              (LPPOLARDLGPARAMS                   pPolarDlgParam, L_UINT uStructSize) const;
      L_INT    GetZoomWaveParams           (LPZOOMWAVEDLGPARAMS                pZoomWaveDlgParam, L_UINT uStructSize) const;
      L_INT    GetRadialWaveParams         (LPRADIALWAVEDLGPARAMS              pRadialWaveDlgParam, L_UINT uStructSize) const;
      L_INT    GetSwirlParams              (LPSWIRLDLGPARAMS                   pSwirlDlgParam, L_UINT uStructSize) const;
      L_INT    GetWaveParams               (LPWAVEDLGPARAMS                    pWaveDlgParam, L_UINT uStructSize) const;
      L_INT    GetWaveShearParams          (LPWAVESHEARDLGPARAMS               pWaveShearDlgParam, L_UINT uStructSize) const;
      L_INT    GetPunchParams              (LPPUNCHDLGPARAMS                   pPunchDlgParam, L_UINT uStructSize) const;
      L_INT    GetRippleParams             (LPRIPPLEDLGPARAMS                  pRippleDlgParam, L_UINT uStructSize) const;
      L_INT    GetBendingParams            (LPBENDINGDLGPARAMS                 pBendingDlgParam, L_UINT uStructSize) const;
      L_INT    GetCylindricalParams        (LPCYLINDRICALDLGPARAMS             pCylindricalDlgParam, L_UINT uStructSize) const;
      L_INT    GetSpherizeParams           (LPSPHERIZEDLGPARAMS                pSpherizeDlgParam, L_UINT uStructSize) const;
      L_INT    GetImpressionistParams      (LPIMPRESSIONISTDLGPARAMS           pImpressionistDlgParam, L_UINT uStructSize) const;
      L_INT    GetPixelateParams           (LPPIXELATEDLGPARAMS                pPixelateDlgParam, L_UINT uStructSize) const;
      L_INT    GetEdgeDetectorParams       (LPEDGEDETECTORDLGPARAMS            pEdgeDetectorDlgParam, L_UINT uStructSize) const;
      L_INT    GetUnderlayParams           (LPUNDERLAYDLGPARAMS                pUnderlayDlgParam, L_UINT uStructSize) const;
      L_INT    GetPicturizeParams          (LPPICTURIZEDLGPARAMS               pPicturizeDlgParam, L_UINT uStructSize) const;

      L_INT    SetMotionBlurParams         (LPMOTIONBLURDLGPARAMS              pMotionBlurDlgParam);
      L_INT    SetRadialBlurParams         (LPRADIALBLURDLGPARAMS              pRadialBlurDlgParam);
      L_INT    SetZoomBlurParams           (LPZOOMBLURDLGPARAMS                pZoomBlurDlgParam);
      L_INT    SetGaussianBlurParams       (LPGAUSSIANBLURDLGPARAMS            pGaussianBlurDlgParam);
      L_INT    SetAntiAliasParams          (LPANTIALIASDLGPARAMS               pAntiAliasDlgParam);
      L_INT    SetAverageParams            (LPAVERAGEDLGPARAMS                 pAverageDlgParam);
      L_INT    SetMedianParams             (LPMEDIANDLGPARAMS                  pMedianDlgParam);
      L_INT    SetAddNoiseParams           (LPADDNOISEDLGPARAMS                pAddNoiseDlgParam);
      L_INT    SetMaxFilterParams          (LPMAXFILTERDLGPARAMS               pMaxFilterDlgParam);
      L_INT    SetMinFilterParams          (LPMINFILTERDLGPARAMS               pMinFilterDlgParam);
      L_INT    SetSharpenParams            (LPSHARPENDLGPARAMS                 pSharpenDlgParam);
      L_INT    SetShiftDifferenceFilterParams(LPSHIFTDIFFERENCEFILTERDLGPARAMS   pShiftdifferenceFilterDlgParam);
      L_INT    SetEmbossParams             (LPEMBOSSDLGPARAMS                  pEmbossDlgParam);
      L_INT    SetOilifyParams             (LPOILIFYDLGPARAMS                  pOilifyDlgParam);
      L_INT    SetMosaicParams             (LPMOSAICDLGPARAMS                  pMosaicDlgParam);
      L_INT    SetErosionFilterParams      (LPEROSIONFILTERDLGPARAMS           pErosionFilterDlgParam);
      L_INT    SetDilationFilterParams     (LPDILATIONFILTERDLGPARAMS          pDilationFilterDlgParam);
      L_INT    SetContourFilterParams      (LPCONTOURFILTERDLGPARAMS           pContourFilterDlgParam);
      L_INT    SetGradientFilterParams     (LPGRADIENTFILTERDLGPARAMS          pGradientFilterDlgParam);
      L_INT    SetLaplacianFilterParams    (LPLAPLACIANFILTERDLGPARAMS         pLaplacianFilterDlgParam);
      L_INT    SetSobelFilterParams        (LPSOBELFILTERDLGPARAMS             pSobelFilterDlgParam);
      L_INT    SetPrewittFilterParams      (LPPREWITTFILTERDLGPARAMS           pPrewittFilterDlgParam);
      L_INT    SetLineSegmentFilterParams  (LPLINESEGMENTFILTERDLGPARAMS       pLineSegmentFilterDlgParam);
      L_INT    SetUnsharpMaskParams        (LPUNSHARPMASKDLGPARAMS             pUnsharpMaskDlgParam);
      L_INT    SetMultiplyParams           (LPMULTIPLYDLGPARAMS                pMultiplyDlgParam);
      L_INT    SetAddBitmapsParams         (LPADDBITMAPSDLGPARAMS              pAddBitmapsDlgParam);
      L_INT    SetStitchParams             (LPSTITCHDLGPARAMS                  pStitchDlgParam);
      L_INT    SetFreeHandWaveParams       (LPFREEHANDWAVEDLGPARAMS            pFreeHandWaveDlgParam);
      L_INT    SetWindParams               (LPWINDDLGPARAMS                    pWindDlgParam);
      L_INT    SetPolarParams              (LPPOLARDLGPARAMS                   pPolarDlgParam);
      L_INT    SetZoomWaveParams           (LPZOOMWAVEDLGPARAMS                pZoomWaveDlgParam);
      L_INT    SetRadialWaveParams         (LPRADIALWAVEDLGPARAMS              pRadialWaveDlgParam);
      L_INT    SetSwirlParams              (LPSWIRLDLGPARAMS                   pSwirlDlgParam);
      L_INT    SetWaveParams               (LPWAVEDLGPARAMS                    pWaveDlgParam);
      L_INT    SetWaveShearParams          (LPWAVESHEARDLGPARAMS               pWaveShearDlgParam);
      L_INT    SetPunchParams              (LPPUNCHDLGPARAMS                   pPunchDlgParam);
      L_INT    SetRippleParams             (LPRIPPLEDLGPARAMS                  pRippleDlgParam);
      L_INT    SetBendingParams            (LPBENDINGDLGPARAMS                 pBendingDlgParam);
      L_INT    SetCylindricalParams        (LPCYLINDRICALDLGPARAMS             pCylindricalDlgParam);
      L_INT    SetSpherizeParams           (LPSPHERIZEDLGPARAMS                pSpherizeDlgParam);
      L_INT    SetImpressionistParams      (LPIMPRESSIONISTDLGPARAMS           pImpressionistDlgParam);
      L_INT    SetPixelateParams           (LPPIXELATEDLGPARAMS                pPixelateDlgParam);
      L_INT    SetEdgeDetectorParams       (LPEDGEDETECTORDLGPARAMS            pEdgeDetectorDlgParam);
      L_INT    SetUnderlayParams           (LPUNDERLAYDLGPARAMS                pUnderlayDlgParam);
      L_INT    SetPicturizeParams          (LPPICTURIZEDLGPARAMS               pPicturizeDlgParam);

      
      //Accessors:
      virtual L_INT  DoModalMotionBlur			   (HWND hWndOwner);
      virtual L_INT  DoModalRadialBlur			   (HWND hWndOwner);
      virtual L_INT  DoModalZoomBlur			   (HWND hWndOwner);
      virtual L_INT  DoModalGaussianBlur		   (HWND hWndOwner);
      virtual L_INT  DoModalAntiAlias			   (HWND hWndOwner);
      virtual L_INT  DoModalAverage				   (HWND hWndOwner);
      virtual L_INT  DoModalMedian				   (HWND hWndOwner);
      virtual L_INT  DoModalAddNoise			   (HWND hWndOwner);
      virtual L_INT  DoModalMaxFilter			   (HWND hWndOwner);
      virtual L_INT  DoModalMinFilter			   (HWND hWndOwner);
      virtual L_INT  DoModalSharpen				   (HWND hWndOwner);
      virtual L_INT  DoModalShiftDifferenceFilter(HWND hWndOwner);
      virtual L_INT  DoModalEmboss				   (HWND hWndOwner);
      virtual L_INT  DoModalOilify				   (HWND hWndOwner);
      virtual L_INT  DoModalMosaic				   (HWND hWndOwner);
      virtual L_INT  DoModalErosionFilter		   (HWND hWndOwner);
      virtual L_INT  DoModalDilationFilter		(HWND hWndOwner);
      virtual L_INT  DoModalContourFilter		   (HWND hWndOwner);
      virtual L_INT  DoModalGradientFilter		(HWND hWndOwner);
      virtual L_INT  DoModalLaplacianFilter		(HWND hWndOwner);
      virtual L_INT  DoModalSobelFilter			(HWND hWndOwner);
      virtual L_INT  DoModalPrewittFilter		   (HWND hWndOwner);
      virtual L_INT  DoModalLineSegmentFilter	(HWND hWndOwner);
      virtual L_INT  DoModalUnsharpMask			(HWND hWndOwner);
      virtual L_INT  DoModalMultiply			   (HWND hWndOwner);
      virtual L_INT  DoModalAddBitmaps			   (HWND hWndOwner);
      virtual L_INT  DoModalStitch				   (HWND hWndOwner);
      virtual L_INT  DoModalFreeHandWave		   (HWND hWndOwner);
      virtual L_INT  DoModalWind				      (HWND hWndOwner);
      virtual L_INT  DoModalPolar				   (HWND hWndOwner);
      virtual L_INT  DoModalZoomWave			   (HWND hWndOwner);
      virtual L_INT  DoModalRadialWave			   (HWND hWndOwner);
      virtual L_INT  DoModalSwirl				   (HWND hWndOwner);
      virtual L_INT  DoModalWave				      (HWND hWndOwner);
      virtual L_INT  DoModalWaveShear			   (HWND hWndOwner);
      virtual L_INT  DoModalPunch				   (HWND hWndOwner);
      virtual L_INT  DoModalRipple				   (HWND hWndOwner);
      virtual L_INT  DoModalBending				   (HWND hWndOwner);
      virtual L_INT  DoModalCylindrical			(HWND hWndOwner);
      virtual L_INT  DoModalSpherize			   (HWND hWndOwner);
      virtual L_INT  DoModalImpressionist		   (HWND hWndOwner);
      virtual L_INT  DoModalPixelate			   (HWND hWndOwner);
      virtual L_INT  DoModalEdgeDetector		   (HWND hWndOwner);
      virtual L_INT  DoModalUnderlay			   (HWND hWndOwner);
      virtual L_INT  DoModalPicturize			   (HWND hWndOwner);
};

#endif //_LEAD_DIALOGIMGEFX_H_
/*================================================================= EOF =====*/
