/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdlgImg.h                                                     |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DIALOGFILE_H_
#define  _LEAD_DIALOGFILE_H_


/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LDialogFile                                                         |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2003                                                    |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LDialogFile: public LDialogBase
{
   LEAD_DECLAREOBJECT(LDialogFile);

   protected:  
      GETDIRECTORYDLGPARAMS            m_GetDirectoryDlgParam;
      FILECONVERSIONDLGPARAMS          m_FileConversionDlgParam;
      FILESASSOCIATIONDLGPARAMS        m_FilesAssociationDlgParam;
      PRINTSTITCHEDIMAGESDLGPARAMS     m_PrintStitchedImagesDlgParam;
      PRINTPREVIEWDLGPARAMS            m_PrintPreviewDlgParam;
      OPENFILENAME                     m_OpenFileName; 
      SAVEDLGPARAMS                    m_SaveDlgParam;
      OPENDLGPARAMS                    m_OpenDlgParam;
      ICCPROFILEDLGPARAMS              m_ICCProfileDlgParam ;

   private:
      L_VOID FreeOpenDlgParam();
      L_VOID InitializeClass();
      L_TCHAR                           m_szFilename[L_MAXPATH];
      static L_INT    EXT_CALLBACK DialogOpenCS(LPOPENDLGFILEDATA lpFileData, L_INT nTotalPercent, L_INT nFilePercent,L_VOID * lpUserData );
   
   protected:
      virtual L_INT DialogOpenCallBack(LPOPENDLGFILEDATA lpFileData, L_INT nTotalPercent, L_INT nFilePercent);


   public : 
      
      LDialogFile();
      LDialogFile(LBitmapBase * pLBitmap);
      virtual ~LDialogFile();
      L_VOID           GetFileName(L_TCHAR * pszBuff, L_INT cbSize) const;
      L_INT            SetFileName(L_TCHAR * pszFile);

      L_INT   GetDirectoryParams          (LPGETDIRECTORYDLGPARAMS            pGetDirectoryDlgParam, L_UINT uStructSize )const;
      L_INT   GetFileConversionParams     (LPFILECONVERSIONDLGPARAMS          pFileConversionDlgParam, L_UINT uStructSize)const;
      L_INT   GetFilesAssociationParams   (LPFILESASSOCIATIONDLGPARAMS        pFilesAssociationDlgParam, L_UINT uStructSize)const;
      L_INT   GetPrintStitchedImagesParams(LPPRINTSTITCHEDIMAGESDLGPARAMS     pPrintStitchedImagesDlgParam, L_UINT uStructSize)const;
#if defined(FOR_UNICODE)
      L_VOID   GetOpenFile                (LPOPENFILENAMEW                    pOpenFileName)const; 
#else
      L_VOID   GetOpenFile                (LPOPENFILENAMEA                    pOpenFileName)const; 
#endif // #if defined(FOR_UNICODE)
      L_INT   GetPrintPreviewParams       (LPPRINTPREVIEWDLGPARAMS            pPrintPreviewDlgParam, L_UINT uStructSize)const;
      L_INT   GetSaveParams               (LPSAVEDLGPARAMS                    pSaveDlgParam, L_UINT uStructSize)const;
      L_INT   GetOpenParams               (LPOPENDLGPARAMS                    pOpenDlgParam, L_UINT uStructSize)const;

      L_INT    SetDirectoryParams          (LPGETDIRECTORYDLGPARAMS            pGetDirectoryDlgParam);
      L_INT    SetFileConversionParams     (LPFILECONVERSIONDLGPARAMS          pFileConversionDlgParam);
      L_INT    SetFilesAssociationParams   (LPFILESASSOCIATIONDLGPARAMS        pFilesAssociationDlgParam);
      L_INT    SetPrintStitchedImagesParams(LPPRINTSTITCHEDIMAGESDLGPARAMS     pPrintStitchedImagesDlgParam);
      L_INT    SetPrintPreviewParams       (LPPRINTPREVIEWDLGPARAMS            pPrintPreviewDlgParam);
#if defined(FOR_UNICODE)
      L_INT    SetOpenFile                 (LPOPENFILENAMEW                    pOpenFileName); 
#else
      L_INT    SetOpenFile                 (LPOPENFILENAMEA                    pOpenFileName); 
#endif // #if defined(FOR_UNICODE)
      L_INT    SetSaveParams               (LPSAVEDLGPARAMS                    pSaveDlgParam);
      L_INT    SetOpenParams               (LPOPENDLGPARAMS                    pOpenDlgParam);

      /*L_INT LWRP_EXPORT L_DlgOpen ( HWND hWndOwner,
                               LPOPENFILENAMEW pOpenFileName,
                               LPOPENDLGPARAMS pDlgParams ) ;*/
      virtual L_INT  DoModalGetDirectory		      (HWND hWndOwner);
      virtual L_INT  DoModalFileConversion		   (HWND hWndOwner);
      virtual L_INT  DoModalFilesAssociation	      (HWND hWndOwner);
      virtual L_INT  DoModalPrintStitchedImages    (HWND hWndOwner);
      virtual L_INT  DoModalPrintPreview	         (HWND hWndOwner);
      virtual L_INT  DoModalSave				         (HWND hWndOwner);
      virtual L_INT  DoModalOpen				         (HWND hWndOwner);
      
      L_INT   GetICCProfileParams                  (LPICCPROFILEDLGPARAMS  pICCProfileDlgParam, L_UINT uStructSize)const;
      L_INT   SetICCProfileParams                  (LPICCPROFILEDLGPARAMS  pICCProfileDlgParam);
      virtual L_INT  DoModalICCProfile             (HWND hWndOwner);
};

#endif //_LEAD_DIALOGFILE_H_
/*================================================================= EOF =====*/
