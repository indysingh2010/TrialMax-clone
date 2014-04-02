/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcNitf.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_NITF_H_
#define  _LEAD_NITF_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LNITFFile                                                       |
| Desc      :                                                                 |
| Return    :                                                                 | 
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : July 2006                                                       |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LNITFFile : public LBase
{
   LEAD_DECLAREOBJECT(LNITFFile);
protected:
   HNITF m_hNITF;

public:
   LNITFFile();
   virtual ~LNITFFile();
   L_INT Create(L_TCHAR * pszFileName);
   L_INT Create();
   L_INT Destroy();
   L_INT GetStatus();
   L_INT SaveFile(L_TCHAR * pszFileName);
   L_INT AppendImageSegment(LBitmapBase * pBitmap, L_INT nFormat, L_INT nBpp, L_INT nQFactor);
   L_INT AppendGraphicSegment(LVectorBase * pVector, LPRECT prcVecBounds);
   L_INT AppendTextSegment(L_TCHAR* pszFileName);
   L_INT SetVector(L_UINT32 uIndex, LVectorBase * pVector);
   LVectorBase * GetVector(L_UINT32 uIndex);
   L_INT GetNITFHeader (pNITFHEADER pNITFHeader);
   L_INT SetNITFHeader (pNITFHEADER pNITFHeader);
   L_INT GetGraphicHeaderCount();
   L_INT GetGraphicHeader (L_UINT uIndex, pGRAPHICHEADER pGraphicHeader);
   L_INT SetGraphicHeader (L_UINT uIndex, pGRAPHICHEADER pGraphicsHeader);
   L_INT GetImageHeaderCount();
   L_INT GetImageHeader (L_UINT uIndex, pIMAGEHEADER pImageHeader);
   L_INT SetImageHeader (L_UINT uIndex, pIMAGEHEADER  pImageHeader);
   L_INT GetTextHeaderCount();
   L_INT GetTextHeader (L_UINT uIndex, pTXTHEADER pTxtHeader);
   L_INT SetTextHeader (L_UINT uIndex, pTXTHEADER pTxtHeader);
   L_INT GetTextSegment (L_UINT uIndex, LBuffer * pLBuffer);
   static L_INT FreeNITFHeader (pNITFHEADER pNITFHeader);
   static L_INT FreeGraphicHeader (pGRAPHICHEADER pGraphicsHeader);
   static L_INT FreeImageHeader(pIMAGEHEADER pImageHeader);
   static L_INT FreeTextHeader(pTXTHEADER pTxtHeader);

   pHNITF GetHandle();
};

#endif // _LEAD_NITF_H_
