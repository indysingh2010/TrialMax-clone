/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdlgImg.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DIALOGWEB_H_
#define  _LEAD_DIALOGWEB_H_


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LDialogWeb                                                         |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2003                                                    |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LDialogWeb: public LDialogBase
{
   LEAD_DECLAREOBJECT(LDialogWeb);

   protected:  
      PNGWEBTUNERDLGPARAMS             m_PNGWebTunerDlgParam;
      GIFWEBTUNERDLGPARAMS             m_GIFWebTunerDlgParam;
      JPEGWEBTUNERDLGPARAMS            m_JPEGWebTunerDlgParam;
      HTMLMAPPERDLGPARAMS              m_HTMLMapperDlgParam;

   private:        
      L_VOID InitializeClass();
   
   public : 
      
      LDialogWeb();
      LDialogWeb(LBitmapBase * pLBitmap);
      virtual ~LDialogWeb();

      L_INT    GetPNGWebTunerParams        (LPPNGWEBTUNERDLGPARAMS             pPNGWebTunerDlgParam, L_UINT uStructSize) const;
      L_INT    GetGIFWebTunerParams        (LPGIFWEBTUNERDLGPARAMS             pGIFWebTunerDlgParam, L_UINT uStructSize) const;
      L_INT    GetJPEGWebTunerParams       (LPJPEGWEBTUNERDLGPARAMS            pJPEGWebTunerDlgParam, L_UINT uStructSize) const;
      L_INT    GetHTMLMapperParams         (LPHTMLMAPPERDLGPARAMS              pHTMLMapperDlgParam, L_UINT uStructSize) const;
      L_INT    SetPNGWebTunerParams        (LPPNGWEBTUNERDLGPARAMS             pPNGWebTunerDlgParam);
      L_INT    SetGIFWebTunerParams        (LPGIFWEBTUNERDLGPARAMS             pGIFWebTunerDlgParam);
      L_INT    SetJPEGWebTunerParams       (LPJPEGWEBTUNERDLGPARAMS            pJPEGWebTunerDlgParam);
      L_INT    SetHTMLMapperParams         (LPHTMLMAPPERDLGPARAMS              pHTMLMapperDlgParam);
      
      //Accessors:
      virtual L_INT  DoModalPNGWebTuner			(HWND hWndOwner);
      virtual L_INT  DoModalGIFWebTuner			(HWND hWndOwner);
      virtual L_INT  DoModalJPEGWebTuner		   (HWND hWndOwner);
      virtual L_INT  DoModalHTMLMapper			   (HWND hWndOwner);
};

#endif //_LEAD_DIALOGWEB_H_
/*================================================================= EOF =====*/
