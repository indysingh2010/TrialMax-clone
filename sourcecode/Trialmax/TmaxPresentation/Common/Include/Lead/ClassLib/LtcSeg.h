/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2001 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcSeg.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_SEGMENT_H_
#define  _LEAD_SEGMENT_H_

//#ifdef MRC_IS_RELEASED

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LSegment                                                        |
| Desc      :                                                                 |
| Return    :                                                                 | 
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 2001                                                  |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LSegment : public LBase
{
	LEAD_DECLAREOBJECT(LSegment);
protected:
   HSEGMENTATION m_hSeg;

private:
   static L_INT EXT_CALLBACK  MrcEnumSegmentsCS(HSEGMENTATION hSegment, const pSEGMENTDATA pSegment, L_INT nSegId, L_VOID * pUserData);

protected:
   virtual L_INT MrcEnumSegmentsCallBack(const pSEGMENTDATA pSegment, L_INT nSegId);

public:
   LSegment();
   virtual ~LSegment();

   pHSEGMENTATION MrcGetHandle();
   L_INT MrcStartBitmapSegmentation(LBitmapBase * pBitmap, COLORREF clrBackground, COLORREF clrForeground);
   L_INT MrcStopBitmapSegmentation(L_VOID);
   L_INT MrcCreateNewSegment(LBitmapBase * pBitmap, pSEGMENTDATA pSegment);
   L_INT MrcEnumSegments(L_UINT32 dwFlags);
   L_INT MrcSetSegmentData(LBitmapBase * pBitmap, L_INT nSegId, pSEGMENTDATA pSegmentData);
   L_INT MrcDeleteSegment(L_INT nSegId);
   L_INT MrcCombineSegments(L_INT nSegId1, L_INT nSegId2, L_UINT16 uCombineFlags, L_UINT16 uCombineFactor);
   L_INT MrcSaveBitmap(LBitmapBase * pBitmap, pCOMPRESSIONOPTIONS pCmpOption, L_TCHAR * pszFileName, L_INT nFormat, pSAVEFILEOPTION pSaveOptions);
   L_INT MrcSaveBitmapT44(LBitmapBase * pBitmap, pCOMPRESSIONOPTIONS pCmpOption, L_TCHAR * pszFileName, L_INT nFormat, pSAVEFILEOPTION pSaveOptions);
   L_INT MrcLoadBitmap(L_TCHAR * pszFileName, LBitmapBase * pBitmap, L_UINT uStructSize, L_INT nPageNo);
   L_INT MrcGetPagesCount(L_TCHAR * pszFileName);
   L_INT MrcSaveSegmentation(L_TCHAR * pszFileName);
   L_INT MrcLoadSegmentation(LBitmapBase * pBitmap, L_TCHAR * pszFileName);
   L_INT MrcCopySegmentationHandle(LSegment& SegSrc);
   L_INT MrcSegmentBitmap (LBitmapBase* pBitmap, pSEGMENTEXTOPTIONS pSegOption);
   L_INT MrcSaveBitmapList(HSEGMENTATION * hSegmentList, L_UINT uhSegmentCount, HBITMAPLIST hList, 
                           pCOMPRESSIONOPTIONS pCmpOption, L_TCHAR * pszFileName,
                           L_INT nFormat);
};

//#endif //MRC_IS_RELEASED

#endif // _LEAD_SEGMENT_H_
