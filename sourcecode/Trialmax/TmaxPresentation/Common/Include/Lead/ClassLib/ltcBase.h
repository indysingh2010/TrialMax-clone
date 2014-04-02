/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcbase.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_BASE_H_
#define  _LEAD_BASE_H_
/*----------------------------------------------------------------------------+
| INCLUDES                                                                    |
+----------------------------------------------------------------------------*/
//#include "ltwrappr.h"

#define  ERRORS_COUNT                  993 //find out after link 
#define  MAX_ERR_SIZE                  256*sizeof(L_TCHAR)
#define  LEAD_INITALIZE_ERROR_DEPTH     1


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LBase                                                    |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LBase
{
   LEAD_DECLAREOBJECT(LBase);

   public:
      L_VOID *m_extLBase;

   private:
      static L_TCHAR          m_ErrorStr[ERRORS_COUNT][MAX_ERR_SIZE];
      static HGLOBAL          m_hErrorList;
      static L_UINT           m_uErrorsCount;
      static L_UINT           m_uListDepth;
      static L_INT*           m_pnErrorList;
      static L_UINT32         m_uErrorFlags;
      static L_UINT           m_uLastErrorIndex;
      static L_INT            m_nInstanceCounter; 

      static HINSTANCE        m_hLTDisDll;
      static HINSTANCE        m_hLTAnnDll;
      static HINSTANCE        m_hLTDlgImgEfxDll;
      static HINSTANCE        m_hLTDlgEfxDll;
      static HINSTANCE        m_hLTDlgFileDll;
      static HINSTANCE        m_hLTDlgImgDll;
      static HINSTANCE        m_hLTDlgImgDocDll;
      static HINSTANCE        m_hLTDlgClrDll;
      static HINSTANCE        m_hLTDlgKrnDll;
      static HINSTANCE        m_hLTDlgWebDll;
      static HINSTANCE        m_hLTEfxDll;
      static HINSTANCE        m_hLTFilDll;
      static HINSTANCE        m_hLTKrnDll;
      static HINSTANCE        m_hLTNetDll;
      static HINSTANCE        m_hLTScrDll;
      static HINSTANCE        m_hLTTw2Dll;
      static HINSTANCE        m_hLTWebDll;
      static HINSTANCE        m_hLTTmbDll;
      static HINSTANCE        m_hLTLstDll;
      static HINSTANCE        m_hLVKrnDll;
      static HINSTANCE        m_hLVDlgDll;
      static HINSTANCE        m_hLTBarDll;
      static HINSTANCE        m_hLDKrnDll;
      static HINSTANCE        m_hLTAutDll;
      static HINSTANCE        m_hLTConDll;
      static HINSTANCE        m_hLTPntDll;
      static HINSTANCE        m_hLTTlbDll;
      static HINSTANCE        m_hLTPdgDll;
      static HINSTANCE        m_hLTSgmDll;
      static HINSTANCE        m_hLTZmvDll;
      static HINSTANCE        m_hLTImgOptDll;
      static HINSTANCE        m_hLCMrcDll;
      static HINSTANCE        m_hLTIvwDll;
      static HINSTANCE        m_hLTClrDll;
      static HINSTANCE        m_hLTNitfDll;

      static HINSTANCE        m_hLTImgCorDll;
      static HINSTANCE        m_hLTImgClrDll;
      static HINSTANCE        m_hLTImgSfxDll;
      static HINSTANCE        m_hLTImgEfxDll;

      static HINSTANCE        m_hLTWiaDll;

      static L_BOOL           m_bAutoErrorDisplay;

      L_BOOL                  m_bEnableCallBack;
      L_BOOL                  m_bEnableStatusCallBack;
      L_BOOL                  m_bEnableOverlayCallBack;
      L_BOOL                  m_bEnableLoadInfoCB;

   protected: 

   private:
      static  L_BOOL CALLBACK DisplayAllErrorDlgProc(HWND hDlg,UINT uMsg, WPARAM wParam, LPARAM lParam);

   protected: 
      static  L_INT   EXT_CALLBACK StatusCS(L_INT nPercentComplete, L_VOID *pUserData);
      virtual L_INT   StatusCallBack(L_INT nPercentComplete);

      static  L_INT   EXT_CALLBACK OverlayCS(pFILEOVERLAYCALLBACKDATA pOverlayCallbackData, L_VOID *pUserData);
      virtual L_INT   OverlayCallBack(pFILEOVERLAYCALLBACKDATA pOverlayCallbackData);

      static   L_INT    EXT_CALLBACK LoadInfoCS(L_HFILE fd,pLOADINFO pInfo, L_VOID * pBaseFile);
      virtual  L_INT    LoadInfoCallBack(L_HFILE fd,pLOADINFO pInfo);

   public:
      LBase();
      ~LBase();
      
      static L_INT            VersionInfo(pVERSIONINFO pVersionInfo, L_UINT uStructSize);
      L_BOOL                  IsCallBackEnabled();
      L_BOOL                  EnableCallBack(L_BOOL bEnable);

      L_BOOL                  IsOverlayCallBackEnabled();
      L_BOOL                  EnableOverlayCallBack(L_BOOL bEnable);

      L_BOOL                  IsStatusCallBackEnabled();
      L_BOOL                  EnableStatusCallBack(L_BOOL bEnable);

      L_BOOL                  IsLoadInfoCallBackEnabled();
      L_BOOL                  EnableLoadInfoCallBack(L_BOOL bEnable);

      static  L_BOOL          IsAutoErrorDisplayEnabled();
      static  L_BOOL          EnableAutoErrorDisplay(L_BOOL bEnable);

      static  L_INT           GetErrorFromList(L_UINT uIndex=LEAD_LAST_ERROR); 
      static  L_UINT          SetErrorListDepth(L_UINT uListDepth);
      static  L_UINT          GetErrorListDepth(L_VOID);
      static  L_VOID          ClearErrorList(L_VOID);
      static  L_UINT          GetErrorsNumber();
      static  L_VOID          DisplayErrorList(HWND hWndParent,L_BOOL bShowLastErrorFirst=TRUE);
      static  L_VOID          DisplayErrorFromList(HWND  hWndParent=NULL, L_UINT uIndex=LEAD_LAST_ERROR);

      static  L_VOID          DisplayError(HWND hWndParent, L_INT nErrorCode);
      static  L_VOID          DisplayError(HWND hWndParent,L_TCHAR * lpszStr);
      static  L_TCHAR *   GetErrorString(L_INT nErrorCode);
      static  L_VOID          SetErrorString(L_INT nErrorCode, L_TCHAR * pszNewErrString, L_TCHAR * pszOldErrString=NULL,L_UINT uSizeOldErrStrBuff=0);
      static  L_VOID          RecordError(L_INT nErrorCode);
      static  LBase*          LEAD_ObjectManager(LWRAPPEROBJECTTYPE nType,LBase* This,LPLEADCREATEOBJECT* pCreateObj);

      static  L_UINT32        LoadLibraries(L_UINT32 uLibraries, L_UINT32 uSubLibraries = LT_ALL_DLG);
      static  L_VOID          UnloadLibraries(L_UINT32 uLibraries, L_UINT32 uSubLibraries = LT_ALL_DLG);
      static  L_UINT32        GetLoadedLibraries();

      static  L_INT           DecodeABIC ( L_UCHAR * pInputData, 
                                           L_INT nLength, 
                                           L_UCHAR ** ppOutputData, 
                                           L_INT nAlign, 
                                           L_INT nWidth,
                                           L_INT nHeight, 
                                           L_BOOL bBiLevel );

      static  L_INT           EncodeABIC ( L_UCHAR *pInputData,
                                           L_INT nAlign,
                                           L_INT nWidth,
                                           L_INT nHeight,
                                           L_UCHAR **ppOutputData,
                                           L_SSIZE_T *pnLength,
                                           L_BOOL bBiLevel );
};

#endif //_LEAD_BASE_H_

/*================================================================= EOF =====*/
