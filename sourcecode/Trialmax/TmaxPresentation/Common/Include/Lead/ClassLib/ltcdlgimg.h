/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdlgImg.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DIALOGIMAGE_H_
#define  _LEAD_DIALOGIMAGE_H_


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LDialogImage                                                         |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2003                                                    |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LDialogImage: public LDialogBase
{
   LEAD_DECLAREOBJECT(LDialogImage);

   protected:  
      ROTATEDLGPARAMS                  m_RotateDlgParam;
      SHEARDLGPARAMS                   m_ShearDlgParam;
      RESIZEDLGPARAMS                  m_ResizeDlgParam;
      ADDBORDERDLGPARAMS               m_AddBorderDlgParam;
      ADDFRAMEDLGPARAMS                m_AddFrameDlgParam;
      AUTOTRIMDLGPARAMS                m_AutotrimDlgParam;
      CANVASRESIZEDLGPARAMS            m_CanvasResizeDlgParam;
      HISTOGRAMDLGPARAMS               m_HistogramDlgParam;

   private:        
      L_VOID InitializeClass();

   public : 
      
      LDialogImage();
      LDialogImage(LBitmapBase * pLBitmap);
      virtual ~LDialogImage();
      
      L_INT    GetRotateParams             (LPROTATEDLGPARAMS                  pRotateDlgParam, L_UINT uStructSize) const;
      L_INT    SetRotateParams             (LPROTATEDLGPARAMS                  pRotateDlgParam);
      L_INT    GetShearParams              (LPSHEARDLGPARAMS                   pShearDlgParam, L_UINT uStructSize) const;
      L_INT    SetShearParams              (LPSHEARDLGPARAMS                   pShearDlgParam);
      L_INT    GetResizeParams             (LPRESIZEDLGPARAMS                  pResizeDlgParam, L_UINT uStructSize) const;
      L_INT    SetResizeParams             (LPRESIZEDLGPARAMS                  pResizeDlgParam);
      L_INT    GetAddBorderParams          (LPADDBORDERDLGPARAMS               pAddBorderDlgParam, L_UINT uStructSize) const;
      L_INT    GetAddFrameParams           (LPADDFRAMEDLGPARAMS                pAddFrameDlgParam, L_UINT uStructSize) const;
      L_INT    SetAddBorderParams          (LPADDBORDERDLGPARAMS               pAddBorderDlgParam);
      L_INT    SetAddFrameParams           (LPADDFRAMEDLGPARAMS                pAddFrameDlgParam);
      L_INT    GetAutoTrimParams           (LPAUTOTRIMDLGPARAMS                pAutotrimDlgParam, L_UINT uStructSize) const;
      L_INT    GetCanvasResizeParams       (LPCANVASRESIZEDLGPARAMS            pCanvasResizeDlgParam, L_UINT uStructSize) const;
      L_INT    GetHistogramParams          (LPHISTOGRAMDLGPARAMS               pHistogramDlgParam, L_UINT uStructSize) const;
      L_INT    SetAutoTrimParams           (LPAUTOTRIMDLGPARAMS                pAutotrimDlgParam);
      L_INT    SetCanvasResizeParams       (LPCANVASRESIZEDLGPARAMS            pCanvasResizeDlgParam);
      L_INT    SetHistogramParams          (LPHISTOGRAMDLGPARAMS               pHistogramDlgParam);

      //Accessors:
      virtual L_INT  DoModalRotate				   (HWND hWndOwner);
      virtual L_INT  DoModalShear				   (HWND hWndOwner);
      virtual L_INT  DoModalResize				   (HWND hWndOwner);
      virtual L_INT  DoModalAddBorder			   (HWND hWndOwner);
      virtual L_INT  DoModalAddFrame			   (HWND hWndOwner);
      virtual L_INT  DoModalAutoTrim			   (HWND hWndOwner);
      virtual L_INT  DoModalCanvasResize		   (HWND hWndOwner);
      virtual L_INT  DoModalHistogram			   (HWND hWndOwner);
};

#endif //_LEAD_DIALOGIMAGE_H_
/*================================================================= EOF =====*/
