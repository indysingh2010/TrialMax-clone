/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2005 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcimgopt.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_OPTIMIZE_H_
#define  _LEAD_OPTIMIZE_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LOptimize                                                       |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 03 August 2005                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LOptimize:public LBase
{

private:
   static L_INT EXT_CALLBACK OptimizeDirCS(pOPTIMIZEIMAGEDIRINFO  pOptImgDirCBInfo,
                                          L_VOID *     pUserData);

   static L_INT EXT_CALLBACK OptimizeBufferCS(L_INT nPercent, L_VOID * pUserData);

protected:
   virtual L_INT OptimizeDirCallBack(pOPTIMIZEIMAGEDIRINFO  pOptImgDirCBInfo);
   virtual L_INT OptimizeBufferCallBack(L_INT nPercent);

public:
   LOptimize();

   ~LOptimize();

   L_INT OptimizeBuffer(L_UCHAR       *         pOrgImgBuffer,
                        L_SIZE_T                uOrgImgBufferSize,
                        HGLOBAL       *         phOptImgBuffer,
                        L_SIZE_T      *         puOptImgBufferSize,
                        pOPTIMIZEIMAGEOPTIONS   pOptImgOptions);

   L_INT OptimizeDir(L_TCHAR       *         pszOrgDirPath,
                     L_TCHAR       *         pszOptDirPath,
                     pOPTIMIZEIMAGEOPTIONS   pOptImgOptions,
                     L_TCHAR       *         pszFilesExt,
                     L_BOOL                  bIncludeSubDirs);
   
   L_INT GetDefaultOptions(pOPTIMIZEIMAGEOPTIONS   pOptImgOptions,
                           L_UINT32                uStructSize);
};
#endif //_LEAD_OPTIMIZE_H_
/*================================================================= EOF =====*/
