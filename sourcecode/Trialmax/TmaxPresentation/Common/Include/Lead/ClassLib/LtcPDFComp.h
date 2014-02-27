/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2005 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcpdfcomp.h                                                    |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_PDFCOMPRESSOR_H_
#define  _LEAD_PDFCOMPRESSOR_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LPDFCompressor                                                  |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 10 August 2005                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LPDFCompressor:public LBase
{

private:
   LCPDF_HANDLE   m_hDocument;
   static L_INT   EXT_CALLBACK   ImageCS(LCPDF_HANDLE hDocument,
                                          L_INT nPage, LPSEGMENTINFO pSegment,
                                          HANDLE pUserData);

protected:
   virtual L_INT 	ImageCallBack(L_INT nPage, LPSEGMENTINFO pSegment);

public:
   LPDFCompressor();

   ~LPDFCompressor();

   L_INT    Init(L_VOID);
   L_VOID   Free(L_VOID);

   L_INT    InsertMRC(LBitmapBase* pBitmap, LPPDFCOMPOPTIONS pPDFOptions);
   L_INT    InsertNormal(LBitmapBase* pBitmap);

   L_INT    Write(L_TCHAR* pwszOutFile);
   
   L_INT SetCompression(LPPDFCOMPRESSION pCompression);

   L_INT InsertSegments(LBitmapBase* pBitmap, 
                        L_UINT uSegmentCnt, LPSEGMENTINFO pSegmentInfo, 
                        L_BOOL bIsThereBackGround, COLORREF rgbBackGroundColor);
};
#endif //_LEAD_PDFCOMPRESSOR_H_
/*================================================================= EOF =====*/
