/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdlgImg.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DIALOGEFFECT_H_
#define  _LEAD_DIALOGEFFECT_H_


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LDialogEffect                                                         |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2003                                                    |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LDialogEffect: public LDialogBase
{
   LEAD_DECLAREOBJECT(LDialogEffect);

   protected:  
      SHAPEDLGPARAMS                   m_ShapeDlgParam;
      EFFECTDLGPARAMS                  m_EffectDlgParam;
      TRANSITIONDLGPARAMS              m_TransitionDlgParam;
      GRADIENTDLGPARAMS                m_GradientDlgParam;
      TEXTDLGPARAMS                    m_TextDlgParam;

   private:        
      L_VOID InitializeClass();
   
   public : 
      
      LDialogEffect();
      LDialogEffect(LBitmapBase * pLBitmap);
      virtual ~LDialogEffect();

      L_INT    GetShapeParams              (LPSHAPEDLGPARAMS                   pShapeDlgParam, L_UINT uStructSize) const;
      L_INT    GetEffectParams             (LPEFFECTDLGPARAMS                  pEffectDlgParam, L_UINT uStructSize) const;
      L_INT    GetTransitionParams         (LPTRANSITIONDLGPARAMS              pTransitionDlgParam, L_UINT uStructSize) const;
      L_INT    GetGradientParams           (LPGRADIENTDLGPARAMS                pGradientDlgParam, L_UINT uStructSize) const;
      L_INT    GetTextParams               (LPTEXTDLGPARAMS                    pTextDlgParam, L_UINT uStructSize) const;
      L_INT    SetShapeParams              (LPSHAPEDLGPARAMS                   pShapeDlgParam);
      L_INT    SetEffectParams             (LPEFFECTDLGPARAMS                  pEffectDlgParam);
      L_INT    SetTransitionParams         (LPTRANSITIONDLGPARAMS              pTransitionDlgParam);
      L_INT    SetGradientParams           (LPGRADIENTDLGPARAMS                pGradientDlgParam);
      L_INT    SetTextParams               (LPTEXTDLGPARAMS                    pTextDlgParam);
      
      //Accessors:
      virtual L_INT  DoModalGetShape			      (HWND hWndOwner);
      virtual L_INT  DoModalGetEffect			      (HWND hWndOwner);
      virtual L_INT  DoModalGetTransition		      (HWND hWndOwner);
      virtual L_INT  DoModalGetGradient			   (HWND hWndOwner);
      virtual L_INT  DoModalGetText				      (HWND hWndOwner);
};

#endif //_LEAD_DIALOGEFFECT_H_
/*================================================================= EOF =====*/
