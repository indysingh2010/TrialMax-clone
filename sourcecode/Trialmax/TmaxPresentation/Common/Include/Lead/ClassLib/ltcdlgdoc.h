/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdlgImg.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DIALOGDOCUMENT_H_
#define  _LEAD_DIALOGDOCUMENT_H_


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LDialogDocument                                                         |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2003                                                    |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LDialogDocument: public LDialogBase
{
   LEAD_DECLAREOBJECT(LDialogDocument);

   protected:  
      SMOOTHDLGPARAMS                  m_SmoothDlgParam;
      LINEREMOVEDLGPARAMS              m_LineRemoveDlgParam;
      BORDERREMOVEDLGPARAMS            m_BorderRemoveDlgParam;
      INVERTEDTEXTDLGPARAMS            m_InvertedTextDlgParam;
      DOTREMOVEDLGPARAMS               m_DotRemoveDlgParam;      
      HOLEPUNCHREMOVEDLGPARAMS         m_HolePunchRemoveDlgParam;
   private:        
      L_VOID InitializeClass();
   
   public : 
      
      LDialogDocument();
      LDialogDocument(LBitmapBase * pLBitmap);
      virtual ~LDialogDocument();

      L_INT   GetSmoothParams             (LPSMOOTHDLGPARAMS                  pSmoothDlgParam, L_UINT uStructSize) const;
      L_INT   GetLineRemoveParams         (LPLINEREMOVEDLGPARAMS              pLineRemoveDlgParam, L_UINT uStructSize) const;
      L_INT   GetBorderRemoveParams       (LPBORDERREMOVEDLGPARAMS            pBorderRemoveDlgParam, L_UINT uStructSize) const;
      L_INT   GetInvertedTextParams       (LPINVERTEDTEXTDLGPARAMS            pInvertedTextDlgParam, L_UINT uStructSize) const;
      L_INT   GetDotRemoveParams          (LPDOTREMOVEDLGPARAMS               pDotRemoveDlgParam, L_UINT uStructSize) const;
      L_INT   GetHolePunchRemoveParams    (LPHOLEPUNCHREMOVEDLGPARAMS         pHolePunchRemoveDlgParam, L_UINT uStructSize) const;

      L_INT    SetSmoothParams             (LPSMOOTHDLGPARAMS                  pSmoothDlgParam);      
      L_INT    SetLineRemoveParams         (LPLINEREMOVEDLGPARAMS              pLineRemoveDlgParam);      
      L_INT    SetBorderRemoveParams       (LPBORDERREMOVEDLGPARAMS            pBorderRemoveDlgParam);      
      L_INT    SetInvertedTextParams       (LPINVERTEDTEXTDLGPARAMS            pInvertedTextDlgParam);      
      L_INT    SetDotRemoveParams          (LPDOTREMOVEDLGPARAMS               pDotRemoveDlgParam);            
      L_INT    SetHolePunchRemoveParams    (LPHOLEPUNCHREMOVEDLGPARAMS         pHolePunchRemoveDlgParam);      

      //Accessors:
      virtual L_INT  DoModalSmooth				   (HWND hWndOwner);
      virtual L_INT  DoModalLineRemove			   (HWND hWndOwner);
      virtual L_INT  DoModalBorderRemove		   (HWND hWndOwner);
      virtual L_INT  DoModalInvertedText		   (HWND hWndOwner);
      virtual L_INT  DoModalDotRemove			   (HWND hWndOwner);
      virtual L_INT  DoModalHolePunchRemove	   (HWND hWndOwner);
};

#endif //_LEAD_DIALOGDOCUMENT_H_
/*================================================================= EOF =====*/
