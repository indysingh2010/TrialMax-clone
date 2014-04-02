/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2005 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : LTCColor.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_COLOR_H_
#define  _LEAD_COLOR_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LColor                                                          |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 14 November 2005                                                |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LColor : public LBase
{

private:
   HANDLE m_ColorHandle;

protected:

public:
   LColor();
   ~LColor();

   L_INT Initialize(L_INT nSrcFormat, L_INT nDstFormat, LPCONVERSION_PARAMS pParams);
   L_INT SetConversionParams(LPCONVERSION_PARAMS pParams);
   static L_INT ConvertDirect(L_INT nSrcFormat, L_INT nDstFormat, L_UCHAR *pSrcBuf, L_UCHAR *pDstBuf,
                              L_INT nWidth, L_INT nHeight, L_INT nInAlign, L_INT nOutAlign);
   static L_INT ConvertDirectToBitmap(L_INT nSrcFormat, L_INT nDstFormat, L_UCHAR *pSrcBuf,
                                      LBitmapBase  *pBitmap, L_UINT uStructSize, L_INT nWidth, L_INT nHeight,
                                      L_INT nInAlign, L_INT nOutAlign);
   L_INT Convert(L_UCHAR *pSrc, L_UCHAR *pDst, L_INT nWidth, L_INT nHeight, L_INT nInAlign, L_INT nOutAlign);
   L_INT ConvertToBitmap(L_UCHAR *pSrcBuf, LBitmapBase *pBitmap, L_UINT uStructSize, L_INT nWidth,
                         L_INT nHeight, L_INT nInAlign, L_INT nOutAlign);
   L_INT Free();
   L_INT IsValid();
   L_INT ClrDlg(L_INT nDlg, HANDLE hWnd, LPCONVERSION_PARAMS pParams);
};
#endif //_LEAD_COLOR_H_
/*================================================================= EOF =====*/
