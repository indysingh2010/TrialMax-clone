/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdlg.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DIALOG_H_
#define  _LEAD_DIALOG_H_

/*----------------------------------------------------------------------------+
| DEFINES                                                                     |
+----------------------------------------------------------------------------*/
//#define DLG_FS_ALL            0xFFFFFFFF  /* all file formats */

#define IDCB_OPENFILE        0   
#define IDCB_GETANGLE        1
#define IDCB_GETSIZE         2
#define IDCB_GETFILTER       3
#define IDCB_GETGHANGE       4  
#define IDCB_GETGAMMA        5
#define IDCB_GETNOISE        6 
#define IDCB_GETEMBOSS       7             
#define IDCB_GETRANG         8           
#define IDCB_COLORRES        9
#define IDCB_FILESAVE        10 
#define IDCB_WINDOWLEVEL     11
#define IDCB_COUNT           IDCB_WINDOWLEVEL+1

#define     START_SHOW_DIALOG(ncdlg)                                 \
LBitmapBase * pPrevBitmap = m_pLBitmap;                              \
  PrepareUIFlags(ncdlg, &uTempFlags);                                  \
START_BITMAP_CHANGING(m_pLBitmap,ncdlg,NCAT_DIALOG);                 \
/*LockHelpCallback();   */                                               \
  

#define     END_SHOW_DIALOG(ncdlg)                                   \
/*UnlockHelpCallback(); */                                               \
m_pLBitmap = pPrevBitmap;                                            \
END_BITMAP_CHANGING(m_pLBitmap,ncdlg,NCAT_DIALOG,nRetCode);          \
HandleAutoProcess(ncdlg, nRetCode, uTempFlags);                      




/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LDialogBase                                                     |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2003                                                    |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LDialogBase : public LBase
{
   LEAD_DECLAREOBJECT(LDialogBase);

   private:  
      static LDialogBase * pActiveDlg;

   protected:
      L_BOOL              m_bShowPreview;
      L_BOOL              m_bAutoProcess;
      L_BOOL              m_bToolbar;

   protected:  
      LBitmapBase *    m_pLBitmap;
      L_TCHAR                           m_szFilename[L_MAXPATH];
      static  L_VOID    EXT_CALLBACK DialogHelpCS(L_UINT32 uFlag, L_INT iControlID, L_VOID * pUserData);

   private:        
      L_VOID            InitializeClass();
   protected:
      virtual L_VOID    DialogHelpCallBack(L_UINT32 uFlag, L_INT iControlID);
      L_VOID            HandleAutoProcess(L_UINT DlgType, L_INT nRetCode, L_UINT32 uUIFlags);
      L_VOID            PrepareUIFlags(L_UINT DlgType, L_UINT32  * puUIFlags);
   
   public : 
      
      LDialogBase();
      LDialogBase(LBitmapBase * pLBitmap);
      virtual ~LDialogBase();

      L_VOID           SetBitmap(LBitmapBase * pBitmap);
      LBitmapBase   *  GetBitmap();
      //L_BOOL           IsValid();
      L_BOOL           EnablePreview(L_BOOL bEnablePreview =TRUE);
      L_BOOL           IsPreviewEnabled() const;
      L_BOOL           EnableAutoProcess(L_BOOL bAuto = TRUE);
      L_BOOL           IsAutoProcessEnabled() const;
      L_BOOL           EnableToolbar(L_BOOL bEnableToolbar = TRUE);
      L_BOOL           IsToolbarEnabled() const;
     
      //Accessors:
      static L_INT  Initialize                    (L_UINT32 uFlags);
      static L_INT  Free                           ();
      static HFONT  SetFont				            (HFONT hFont);
      static L_INT  GetStringLen	                  (L_UINT32 uString, L_UINT  * puLen);
      static L_INT  SetString		                  (L_UINT32 uString, L_TCHAR * szString);
      static L_INT  GetString		                  (L_UINT32 uString, L_TCHAR * szString, L_SIZE_T sizeInWords);
};

#endif //_LEAD_DIALOG_H_
/*================================================================= EOF =====*/
