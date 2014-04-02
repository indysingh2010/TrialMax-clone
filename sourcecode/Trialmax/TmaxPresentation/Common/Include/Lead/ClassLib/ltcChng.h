/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcCnvrt.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_CHANGE_H_
#define  _LEAD_CHANGE_H_

/*----------------------------------------------------------------------------+
| DEFINES                                                                     |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LChange                                                         |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : september 1998                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LChange : public LBase
{
   LEAD_DECLAREOBJECT(LChange);

   public:
      L_VOID *m_extLChange;

   public : 
      LChange();
      virtual ~LChange();

      static HGLOBAL ChangeToDIB(LBitmapBase * pLBitmap, L_UINT uType = DIB_BITMAPV5HEADER);
      static L_INT   ChangeFromDIB(LBitmapBase * pLBitmap,L_UINT uStructSize, HGLOBAL hDIB);
      static L_INT   ChangeFromDDB(HDC hDC,
                                   LBitmapBase * pLBitmap,
                                   L_UINT uStructSize,
                                   HBITMAP hBitmap,
                                   HPALETTE hPalette
                                  );
      static HBITMAP ChangeToDDB(HDC hDC,LBitmapBase * pLBitmap);

      
      static L_INT   ChangeFromEMF(
                                   LBitmapBase * pLBitmap,
                                   L_UINT uStructSize,
                                   HENHMETAFILE hEmf, 
                                   L_UINT uWidth, 
                                   L_UINT uHeight);

      static L_INT   ChangeFromWMF(
                                         LBitmapBase * pLBitmap,
                                         L_UINT uStructSize,
                                         HMETAFILE hWmf, 
                                         L_UINT uWidth, 
                                         L_UINT uHeight);
      static HENHMETAFILE ChangeToEMF(LBitmapBase * pLBitmap);
      static HMETAFILE ChangeToWMF(LBitmapBase * pLBitmap);
      
};

#endif //_LEAD_CHANGE_H_
/*================================================================= EOF =====*/
