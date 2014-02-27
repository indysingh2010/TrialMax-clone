/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdlgImg.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DIALOGCOLOR_H_
#define  _LEAD_DIALOGCOLOR_H_


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LDialogColor                                                         |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2003                                                    |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LDialogColor: public LDialogBase
{
   LEAD_DECLAREOBJECT(LDialogColor);

   protected:  
      BALANCECOLORSDLGPARAMS           m_BalanceColorsDlgParam;
      COLOREDGRAYDLGPARAMS             m_ColoredGrayDlgParam;
      GRAYSCALEDLGPARAMS               m_GrayscaleDlgParam;
      REMAPINTENSITYDLGPARAMS          m_RemapintInsityDlgParam;
      REMAPHUEDLGPARAMS                m_RemaphueDlgParam;
      CUSTOMIZEPALETTEDLGPARAMS        m_CustomizePaletteDlgParam;
      LOCALHISTOEQUALIZEDLGPARAMS      m_LocalHistoEqualizeDlgParam;
      INTENSITYDETECTDLGPARAMS         m_IntensityDetectDlgParam;
      SOLARIZEDLGPARAMS                m_SolarizeDlgParam;
      POSTERIZEDLGPARAMS               m_PosterizeDlgParam;
      BRIGHTNESSDLGPARAMS              m_BrightnessDlgParam;
      CONTRASTDLGPARAMS                m_ContrastDlgParam;
      HUEDLGPARAMS                     m_HueDlgParam;
      SATURATIONDLGPARAMS              m_SaturationDlgParam;
      GAMMAADJUSTMENTDLGPARAMS         m_GammaAdjustmentDlgParam;
      HALFTONEDLGPARAMS                m_HalftoneDlgParam;
      COLORRESDLGPARAMS                m_ColorResDlgParam;
      HISTOCONTRASTDLGPARAMS           m_HistoContrastDlgParam;
      WINDOWLEVELDLGPARAMS             m_WindowLevelDlgParam;
      COLORDLGPARAMS                   m_ColorDlgParam;

   private:        
      L_VOID InitializeClass();

   public : 
      
      LDialogColor();
      LDialogColor(LBitmapBase * pLBitmap);
      virtual ~LDialogColor();

      L_INT   GetBalanceColorsParams      (LPBALANCECOLORSDLGPARAMS           pBalanceColorsDlgParam, L_UINT uStructSize) const;
      L_INT   GetColoredGrayParams        (LPCOLOREDGRAYDLGPARAMS             pColoredGrayDlgParam, L_UINT uStructSize) const;
      L_INT   GetGrayScaleParams          (LPGRAYSCALEDLGPARAMS               pGrayscaleDlgParam, L_UINT uStructSize) const;
      L_INT   GetRemapIntensityParams     (LPREMAPINTENSITYDLGPARAMS          pRemapintEnsityDlgParam, L_UINT uStructSize) const;
      L_INT   GetRemapHueParams           (LPREMAPHUEDLGPARAMS                pRemaphueDlgParam, L_UINT uStructSize) const;
      L_INT   GetCustomizePaletteParams   (LPCUSTOMIZEPALETTEDLGPARAMS        pCustomizePaletteDlgParam, L_UINT uStructSize) const;
      L_INT   GetLocalHistoEqualizeParams (LPLOCALHISTOEQUALIZEDLGPARAMS      pLocalHistoEqualizeDlgParam, L_UINT uStructSize) const;
      L_INT   GetIntensityDetectParams    (LPINTENSITYDETECTDLGPARAMS         pIntensityDetectDlgParam, L_UINT uStructSize) const;
      L_INT   GetSolarizeParams           (LPSOLARIZEDLGPARAMS                pSolarizeDlgParam, L_UINT uStructSize) const;
      L_INT   GetPosterizeParams          (LPPOSTERIZEDLGPARAMS               pPosterizeDlgParam, L_UINT uStructSize) const;
      L_INT   GetBrightnessParams         (LPBRIGHTNESSDLGPARAMS              pBrightnessDlgParam, L_UINT uStructSize) const;
      L_INT   GetContrastParams           (LPCONTRASTDLGPARAMS                pContrastDlgParam, L_UINT uStructSize) const;
      L_INT   GetHueParams                (LPHUEDLGPARAMS                     pHueDlgParam, L_UINT uStructSize) const;
      L_INT   GetSaturationParams         (LPSATURATIONDLGPARAMS              pSaturationDlgParam, L_UINT uStructSize) const;
      L_INT   GetGammaAdjustmentParams    (LPGAMMAADJUSTMENTDLGPARAMS         pGammaAdjustmentDlgParam, L_UINT uStructSize) const;
      L_INT   GetHalfToneParams           (LPHALFTONEDLGPARAMS                pHalftoneDlgParam, L_UINT uStructSize) const;
      L_INT   GetColorResParams           (LPCOLORRESDLGPARAMS                pColorResDlgParam, L_UINT uStructSize) const;
      L_INT   GetHistoContrastParams      (LPHISTOCONTRASTDLGPARAMS           pHistoContrastDlgParam, L_UINT uStructSize) const;
      L_INT   GetWindowLevelParams        (LPWINDOWLEVELDLGPARAMS             pWindowLevelDlgParam, L_UINT uStructSize) const;
      L_INT   GetColorParams              (LPCOLORDLGPARAMS                   pColorDlgParam, L_UINT uStructSize) const;
      
      L_INT    SetBalanceColorsParams      (LPBALANCECOLORSDLGPARAMS           pBalanceColorsDlgParam);
      L_INT    SetColoredGrayParams        (LPCOLOREDGRAYDLGPARAMS             pColoredGrayDlgParam);
      L_INT    SetGrayScaleParams          (LPGRAYSCALEDLGPARAMS               pGrayscaleDlgParam);
      L_INT    SetRemapIntensityParams     (LPREMAPINTENSITYDLGPARAMS          pRemapintEnsityDlgParam);
      L_INT    SetRemapHueParams           (LPREMAPHUEDLGPARAMS                pRemaphueDlgParam);
      L_INT    SetCustomizePaletteParams   (LPCUSTOMIZEPALETTEDLGPARAMS        pCustomizePaletteDlgParam);
      L_INT    SetLocalHistoEqualizeParams (LPLOCALHISTOEQUALIZEDLGPARAMS      pLocalHistoEqualizeDlgParam);
      L_INT    SetIntensityDetectParams    (LPINTENSITYDETECTDLGPARAMS         pIntensityDetectDlgParam);
      L_INT    SetSolarizeParams           (LPSOLARIZEDLGPARAMS                pSolarizeDlgParam);
      L_INT    SetPosterizeParams          (LPPOSTERIZEDLGPARAMS               pPosterizeDlgParam);
      L_INT    SetBrightnessParams         (LPBRIGHTNESSDLGPARAMS              pBrightnessDlgParam);
      L_INT    SetContrastParams           (LPCONTRASTDLGPARAMS                pContrastDlgParam);
      L_INT    SetHueParams                (LPHUEDLGPARAMS                     pHueDlgParam);
      L_INT    SetSaturationParams         (LPSATURATIONDLGPARAMS              pSaturationDlgParam);
      L_INT    SetGammaAdjustmentParams    (LPGAMMAADJUSTMENTDLGPARAMS         pGammaAdjustmentDlgParam);
      L_INT    SetHalfToneParams           (LPHALFTONEDLGPARAMS                pHalftoneDlgParam);
      L_INT    SetColorResParams           (LPCOLORRESDLGPARAMS                pColorResDlgParam);
      L_INT    SetHistoContrastParams      (LPHISTOCONTRASTDLGPARAMS           pHistoContrastDlgParam);
      L_INT    SetWindowLevelParams        (LPWINDOWLEVELDLGPARAMS             pWindowLevelDlgParam);
      L_INT    SetColorParams              (LPCOLORDLGPARAMS                   pColorDlgParam);
      
      //Accessors:
      virtual L_INT  DoModalBalanceColors	      (HWND hWndOwner);
      virtual L_INT  DoModalColoredGray		   (HWND hWndOwner);
      virtual L_INT  DoModalGrayScale		      (HWND hWndOwner);
      virtual L_INT  DoModalRemapIntensity	   (HWND hWndOwner);
      virtual L_INT  DoModalRemapHue			   (HWND hWndOwner);
      virtual L_INT  DoModalCustomizePalette	   (HWND hWndOwner);
      virtual L_INT  DoModalLocalHistoEqualize  (HWND hWndOwner);
      virtual L_INT  DoModalIntensityDetect	   (HWND hWndOwner);
      virtual L_INT  DoModalSolarize			   (HWND hWndOwner);
      virtual L_INT  DoModalPosterize			   (HWND hWndOwner);
      virtual L_INT  DoModalBrightness			   (HWND hWndOwner);
      virtual L_INT  DoModalContrast			   (HWND hWndOwner);
      virtual L_INT  DoModalHue					   (HWND hWndOwner);
      virtual L_INT  DoModalSaturation			   (HWND hWndOwner);
      virtual L_INT  DoModalGammaAdjustment	   (HWND hWndOwner);
      virtual L_INT  DoModalHalfTone			   (HWND hWndOwner);
      virtual L_INT  DoModalColorRes			   (HWND hWndOwner);
      virtual L_INT  DoModalHistoContrast		   (HWND hWndOwner);
      virtual L_INT  DoModalWindowLevel			(HWND hWndOwner);
      virtual L_INT  DoModalColor				   (HWND hWndOwner);
      
};

#endif //_LEAD_DIALOGCOLOR_H_
/*================================================================= EOF =====*/
