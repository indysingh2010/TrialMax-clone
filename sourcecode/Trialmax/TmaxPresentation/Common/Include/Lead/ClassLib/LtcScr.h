/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcbase.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_SCREEN_CAPTURE_H_
#define  _LEAD_SCREEN_CAPTURE_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LScreenCapture                                                  |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 1998                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LScreenCapture : public LBase
{
   LEAD_DECLAREOBJECT(LScreenCapture);

   public:
      L_VOID *m_extLScreenCapture;
      
   protected:  
      LEADCAPTUREINFO         m_CaptureInfo; 
      LEADCAPTUREOPTION       m_CaptureOptions;
      LEADCAPTUREAREAOPTION   m_CaptureAreaOption; 
      LEADCAPTUREOBJECTOPTION m_CaptureObjectOptions;

      LBitmapBase *      m_pBitmap;

   private:  
      L_VOID InitializeClass();
      static L_INT EXT_CALLBACK ScreenCaptureCS(pBITMAPHANDLE pBitmap,
                                    pLEADCAPTUREINFO pCaptureInfo,
                                    L_VOID  * pUserData);

      static L_VOID EXT_CALLBACK ScreenCaptureHelpCS(
                                                L_UINT32 uFlag,
                                                L_INT nCtlID, 
                                                L_VOID * pUserData
                                                );
      
      static L_INT EXT_CALLBACK CaptureHotKeyCS(L_INT nHotKey,L_UINT uHotKeyModifiers,
                                    L_VOID * pUserData);

   protected:
      virtual L_INT  ScreenCaptureCallBack();
      virtual L_VOID ScreenCaptureHelpCallBack(L_UINT32 uFlag, L_INT nCtlID);
      virtual L_INT  CaptureHotKeyCallBack(L_INT nHotKey,L_UINT uHotKeyModifiers);

   public:
      LScreenCapture();
      LScreenCapture(LBitmapBase * pLBitmap);
      LScreenCapture(LScreenCapture * pScreenCapture);
      virtual ~LScreenCapture();

      virtual L_VOID    SetBitmap(LBitmapBase * pLBitmap);
      virtual L_BOOL    IsValid();

      virtual L_INT     SetCaptureOptions(pLEADCAPTUREOPTION pCaptureOptions);
      virtual L_INT     GetCaptureOptions(pLEADCAPTUREOPTION pCaptureOptions);

      virtual L_INT     SetCaptureAreaOptions(pLEADCAPTUREAREAOPTION pCaptureAreaOption);
      virtual L_INT     GetCaptureAreaOptions(pLEADCAPTUREAREAOPTION pCaptureAreaOption);

      virtual L_INT     SetCaptureObjectOptions(pLEADCAPTUREOBJECTOPTION   pCaptureObjectOptions);
      virtual L_INT     GetCaptureObjectOptions(pLEADCAPTUREOBJECTOPTION   pCaptureObjectOptions);

      virtual L_INT     SetDefaultCaptureOptions(L_VOID);
      virtual L_INT     SetDefaultAreaOptions(L_VOID);
      virtual L_INT     SetDefaultObjectOptions(L_VOID);

      virtual L_INT     CaptureWallpaper(L_VOID);
      virtual L_INT     CaptureFullScreen(L_VOID);
      virtual L_INT     CaptureMenuUnderCursor(L_VOID);
      virtual L_INT     CaptureWindowUnderCursor(L_VOID);
      virtual L_INT     CaptureMouseCursor(COLORREF crFill=RGB(255,255,255));

      virtual L_INT     CaptureWindow(HWND hWnd,WINDOWCAPTURETYPE wctCaptureType);
      virtual L_INT     CaptureActiveClient(L_VOID);
      virtual L_INT     CaptureActiveWindow(L_VOID);

      virtual L_INT     CaptureArea(L_VOID); 
      virtual L_INT     CaptureAreaOptionDlg(HWND hParentWnd, L_UINT uFlags);

      virtual L_INT     CaptureSelectedObject(L_VOID);
      virtual L_INT     CaptureObjectOptionDlg(HWND hParentWnd, L_UINT uFlags);

      virtual L_INT     SetCaptureOptionDlg(HWND hParentWnd, L_UINT uFlags);

      virtual L_INT     CaptureFromEXE(
                                       L_TCHAR * pszFileName,
                                       L_INT nResType,
                                       L_TCHAR * pResID,
                                       L_BOOL bCaptureByIndex,
                                       COLORREF clrBackGnd=RGB(0,0,0)
                                      );

      virtual L_INT     CaptureFromEXEDlg(
                                          L_TCHAR * pszFileName,
                                          COLORREF * pTransparentColor,
                                          L_INT nResType, 
                                          L_UINT uFlags,
                                          L_INT nDialogType=LTCAPDLG_TREEVIEW
                                         );

      virtual L_INT     CaptureGetResCount(
                                             L_TCHAR * pszFileName, 
                                             L_INT nResType, 
                                             L_INT32 * pnCount
                                          );

      static  L_BOOL    IsCaptureActive();
      static  L_INT     StopCapture(L_VOID);
      
      static  L_INT     SetCaptureOption(pLEADCAPTUREOPTION pOptions);
      static  L_INT     GetCaptureOption(pLEADCAPTUREOPTION pOptions, L_UINT uStructSize);

};


#endif //_LEAD_SCREEN_CAPTURE_H_
/*================================================================= EOF =====*/
