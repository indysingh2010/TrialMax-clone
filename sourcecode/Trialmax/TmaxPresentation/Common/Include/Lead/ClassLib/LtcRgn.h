/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcrgn.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_BITMAPRGN_H_
#define  _LEAD_BITMAPRGN_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LBitmapRgn                                                      |
| Desc      :                                                                 |
| Return    :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 1998                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LBitmapRgn :public LBase 
{
   LEAD_DECLAREOBJECT(LBitmapRgn);

   public:
      L_VOID *m_extLBitmapRgn;
      

   protected:  
      LBitmapBase       *  m_pLBitmap;
      L_UINT               m_uCombineMode;
      RGNXFORM             m_RgnXform;
      L_BOOL               m_bRgnXform;
   public : 
      LBitmapRgn();
      LBitmapRgn(LBitmapBase * pLBitmap, L_UINT uCombineMode = L_RGN_SET);
      virtual ~LBitmapRgn();
      //Accessors:
      L_UINT         GetRgnCombineMode();
      L_UINT         SetRgnCombineMode(L_UINT uCombineMode);
      L_INT          SetRgnXForm(pRGNXFORM pXform);
      L_BOOL         GetRgnXForm(pRGNXFORM pXform);
      //Operations:
      virtual L_BOOL IsValid();
      virtual L_VOID SetBitmap(LBitmapBase * pLBitmap);

      virtual L_INT  Free();   
      virtual L_INT  CreateMaskFromBitmapRgn(LBitmapBase *  pLBitmapMask, L_UINT uStructSize);
      virtual L_BOOL IsPtInRgn(L_INT nRow,L_INT  nCol); 
      virtual L_BOOL IsPtInRgn(LPPOINT pPoint); 
      virtual L_INT  Offset(L_INT nRowOffset,L_INT nColOffset);    
      virtual L_INT  SetRgnColor(COLORREF crColor);
      virtual L_INT  SetRgnColorRGBRange(COLORREF crLower, 
                                         COLORREF crUpper);
      virtual L_INT  SetRgnMagicWand(L_INT x, L_INT y, COLORREF crLowerTolerance, COLORREF crUpperTolerance);
      virtual L_INT  SetRgnEllipse(LPRECT pRect);               
      virtual L_INT  SetRgnFromMask(LBitmapBase& LBitmapMask); 
      virtual L_INT  SetRgnPolygon(POINT * pPoints,L_UINT uPoints,L_UINT uFillMode =L_POLY_WINDING);
      virtual L_INT  SetRgnRect(LPRECT pRect); 
      virtual L_INT  SetRgnRoundRect(LPRECT pRect,L_INT nWidthEllipse, L_INT nHeightEllipse); 
      virtual L_INT  GetRgnBounds(LPRECT pRect);
      virtual L_INT  GetRgnArea(L_SIZE_T  * puArea);
      virtual L_INT  SetRgnHandle(HRGN hRgn);
      virtual HRGN   GetRgnHandle(); 
      virtual L_BOOL BitmapHasRgn();
      virtual L_INT  SetRgnCurve(pCURVE pCurve);
      virtual L_INT  CurveToBezier(pCURVE pCurve, L_INT * pOutPointCount, POINT * pOutPoint);
      virtual L_INT  ResizeRgn(L_UINT uInflate, L_UINT uFlag, L_BOOL bAsFrame);
#if defined (LEADTOOLS_V16_OR_LATER)
      virtual L_INT  HolesRemovalRgn(L_UINT32 uFlags = 0);
#else
      virtual L_INT  HolesRemovalRgn();
#endif //LEADTOOLS_V16_OR_LATER
      virtual L_INT  GetClipSegments(L_INT nRow, L_UINT * pSegmentBuffer, L_UINT * puSegmentCount);

      virtual L_INT  GetClipSegmentsMax(L_UINT * puMaxSegments);

      virtual L_INT  SetRgnBorder(L_INT         x, 
                                     L_INT         y, 
                                     COLORREF      crBorderColor,
                                     COLORREF      crLowerTolerance, 
                                     COLORREF      crUpperTolerance);

      L_INT  SetRgnColorHSVRange ( HSVREF hsvLower, HSVREF hsvUpper);
};

#endif //_LEAD_BITMAPRGN_H_
/*================================================================= EOF =====*/
