/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2005 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcImgViewer.h                                                  |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_CONTAINERVIEWER_H_
#define  _LEAD_CONTAINERVIEWER_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/
 
/*----------------------------------------------------------------------------+
| Class     : LImageViewer                                                    |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 12 August 2005                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LImageViewer:public LBase
{
private:
   HDISPCONTAINER m_hDispContainer;
   L_BOOL         m_bEnableTagCallBack;
   L_BOOL         m_bEnableActionCallBack;
   L_BOOL         m_bEnablePaintCallBack;
#if defined (LEADTOOLS_V16_OR_LATER)
   L_BOOL         m_bEnableRegionCallBack;
   L_BOOL         m_bEnableAnnotationCallBack;
   L_BOOL         m_bEnableAnnotationCreatedCallBack;
   L_BOOL         m_bEnableActiveSubCellChangedCallBack;
   L_BOOL         m_bEnableAnimationStartedCallBack;
   L_BOOL         m_bEnableAnimationStoppedCallBack;
   L_BOOL         m_bEnableLowMemoryUsageCallBack;
   L_BOOL         m_bEnableMouseCallBack;
   L_BOOL         m_bEnablePrePaintCallBack;
   L_BOOL         m_bEnablePostPaintCallBack;
#endif // LEADTOOLS_V16_OR_LATER

   static  L_INT EXT_CALLBACK PaintCS(HDC    hMemDC,
                                      LPRECT lpRect,
                                      L_INT  nCellIndex,
                                      L_INT  nSubCellIndex,
                                      L_VOID *  pUserData);

   static L_INT EXT_CALLBACK TagCS(L_INT nCellIndex, 
                                  HDC hDC, 
                                  RECT * lpRect, 
                                  L_VOID *pUserData);

   static L_INT EXT_CALLBACK ActionCS(L_INT nCellIndex,
                                       HBITMAPLIST * phBitmapList,
                                       L_UINT uCount,
                                       L_INT nAction,
                                       L_UINT uMessage,
                                       L_UINT wParam,
                                       POINT * ptMousePos, 
                                       L_VOID *pUserData);

#if defined (LEADTOOLS_V16_OR_LATER)

   static L_INT EXT_CALLBACK AnimationStartedCS (L_INT    nCellIndex,
                                                 L_VOID * pAnimationStartedUserData);

   static L_INT EXT_CALLBACK AnimationStoppedCS (L_INT    nCellIndex,
                                                 L_VOID * pAnimationStoppedUserData);

   static L_INT EXT_CALLBACK LowMemoryUsageCS (L_INT    nCellIndex,
                                               L_UINT * puFramesRequested,
                                               L_UINT   uLength,
                                               L_VOID * pUserData);

   static L_INT EXT_CALLBACK PrePaintCS       (pDISPCONTAINERCELLINFO pCellInfo,
                                               L_VOID *               pUserData);

   static L_INT EXT_CALLBACK PostPaintCS      (pDISPCONTAINERCELLINFO pCellInfo,
                                               L_VOID *               pUserData);

   static L_INT EXT_CALLBACK MouseCS           (L_UINT                 uMessage,
                                                pDISPCONTAINERCELLINFO pCellInfo,
                                                L_VOID *               pUserData);

   static L_INT EXT_CALLBACK ActiveSubCellChangedCS (L_INT    nCellIndex,
                                                     L_INT    nSubCellIndex,
                                                     L_INT    nPreviousSubCellIndex,
                                                     L_VOID * pUserData);

   static L_INT EXT_CALLBACK AnnotationCreatedCS (L_INT    nCellIndex,
                                                  L_INT    nSubCellIndex,
                                                  L_UINT   uAnnotationType,
                                                  L_VOID * pUserData);

   static L_INT EXT_CALLBACK RegionCS (HRGN     hRgn,
                                       L_INT    nCellIndex,
                                       L_INT    nSubCellIndex,
                                       L_UINT   uOperation,
                                       L_VOID * pUserData);

   static L_INT EXT_CALLBACK AnnotationCS (L_UINT   uMessage,
                                           L_INT    nX,
                                           L_INT    nY,
                                           L_INT    nCellIndex,
                                           L_INT    nSubCellIndex,
                                           L_VOID * pUserData);
#endif // LEADTOOLS_V16_OR_LATER

protected:
   virtual L_INT PaintCallBack(HDC    hMemDC,
                               LPRECT lpRect,
                               L_INT  nCellIndex,
                               L_INT  nSubCellIndex);

   virtual L_INT ActionCallBack(L_INT nCellIndex,
                                 HBITMAPLIST * phBitmapList,
                                 L_UINT uCount,
                                 L_INT nAction,
                                 L_UINT uMessage,
                                 L_UINT wParam,
                                 POINT * ptMousePos);

   virtual L_INT TagCallBack(L_INT nCellIndex, 
                              HDC hDC, 
                              RECT * lpRect);

#if defined (LEADTOOLS_V16_OR_LATER)

   virtual L_INT AnimationStartedCallBack (L_INT nCellIndex);

   virtual L_INT AnimationStoppedCallBack (L_INT nCellIndex);

   virtual L_INT LowMemoryUsageCallBack (L_INT    nCellIndex,
                                         L_UINT * puFramesRequested,
                                         L_UINT   uLength);

   virtual L_INT PrePaintCallBack       (pDISPCONTAINERCELLINFO pCellInfo);

   virtual L_INT PostPaintCallBack      (pDISPCONTAINERCELLINFO pCellInfo);

   virtual L_INT MouseCallBack           (L_UINT                 uMessage,
                                          pDISPCONTAINERCELLINFO pCellInfo);

   virtual L_INT ActiveSubCellChangedCallBack (L_INT    nCellIndex,
                                               L_INT    nSubCellIndex,
                                               L_INT    nPreviousSubCellIndex);

   virtual L_INT AnnotationCreatedCallBack (L_INT    nCellIndex,
                                            L_INT    nSubCellIndex,
                                            L_UINT   uAnnotationType);

   virtual L_INT RegionCallBack (HRGN     hRgn,
                                 L_INT    nCellIndex,
                                 L_INT    nSubCellIndex,
                                 L_UINT   uOperation);

   virtual L_INT AnnotationCallBack (L_UINT   uMessage,
                                     L_INT    nX,
                                     L_INT    nY,
                                     L_INT    nCellIndex,
                                     L_INT    nSubCellIndex);
#endif // LEADTOOLS_V16_OR_LATER


public:
   LImageViewer();
   ~LImageViewer();

   L_INT Create(HWND hWndParent, RECT *lpRect, L_UINT uFlags);
   HWND  GetWindowHandle(L_UINT uFlags);
   L_INT Destroy(L_BOOL bCleanImages, L_UINT uFlags);
   L_INT SetProperties (pDISPCONTAINERPROPERTIES pDispContainerProp, L_UINT uFlags);
   L_INT GetProperties(pDISPCONTAINERPROPERTIES pDispContainerProp, L_UINT uFlags);
   L_INT InsertCell(L_INT nCellIndex, L_UINT uFlags);
   L_INT RemoveCell(L_INT nCellIndex, L_BOOL bCleanImages, L_UINT uFlags);
   L_INT GetCellCount(L_UINT uFlags);
   HWND  GetCellWindowHandle(L_INT nCellIndex, L_UINT uFlags);
   L_INT SetCellBitmapList(L_INT nCellIndex, HBITMAPLIST hBitmapList, L_BOOL bCleanImages, L_UINT uFlags);
   L_INT AddAction(L_INT nAction, L_UINT uFlags);
   L_INT SetAction(L_INT nAction, L_INT nMouseButton, L_UINT uFlags);
   L_INT SetCellTag(L_INT nCellIndex, L_UINT uRow, L_UINT uAlign, L_UINT uType, LPTSTR pString, L_UINT uFlags);
   L_INT SetCellProperties(L_INT nCellIndex, pDISPCELLPROPERTIES pCellProperties, L_UINT uFlags);
   L_INT GetCellProperties(L_INT nCellIndex, pDISPCELLPROPERTIES pCellProperties, L_UINT uFlags);
   L_INT GetCellPosition(L_INT nCellIndex, L_UINT * puRow, L_UINT * puCol, L_UINT uFlags);
   L_INT RepositionCell (L_INT nCellIndex, L_INT nTargetIndex, L_BOOL bSwap, L_UINT uFlags);
   L_INT GetCellBitmapList(L_INT nCellIndex, pHBITMAPLIST phBitmapList, L_UINT uFlags);
   L_INT GetCellBounds(L_INT nCellIndex, LPRECT lpRect, L_UINT uFlags);
   L_INT FreezeCell(L_INT nCellIndex, L_BOOL bFreeze, L_UINT uFlags);
   L_INT SetFirstVisibleRow(L_UINT uRow, L_UINT uFlags);
   L_INT GetFirstVisibleRow(L_UINT * uRow, L_UINT uFlags);
   L_INT SetActionProperties(L_INT nAction, L_INT nCellIndex, L_INT nSubCellIndex, L_VOID * pActionProperties, L_UINT uFlags);
   L_INT GetActionProperties(L_INT nAction, L_INT nCellIndex, L_INT nSubCellIndex, L_VOID * pActionProperties, L_UINT uFlags);
   L_INT RemoveAction(L_INT nAction, L_UINT uFlags);
   L_INT GetActionCount(L_INT *  pnCount, L_UINT uFlags);
   L_INT SetKeyboardAction(L_INT nAction, L_INT nButton, L_UINT uKey, L_UINT uFlags);
   L_INT SetBounds(LPRECT lpRect, L_UINT uFlags);
   L_INT GetBounds(LPRECT lpRect, L_UINT uFlags);
   L_INT SelectCell(L_INT nCellIndex, L_BOOL bSelect, L_UINT uFlags);
   L_BOOL IsCellSelected(L_INT nCellIndex, L_UINT uFlags);
   L_INT GetKeyboardAction(L_INT nAction, L_INT nMouseDirection, L_UINT *puVk, L_UINT * puModifiers, L_UINT uFlags);
   L_BOOL IsCellFrozen(L_INT nCellIndex, L_UINT uFlags);
   L_BOOL IsActionActive(L_INT nAction, L_UINT uFlags);

   // Enable those callback.
   L_BOOL EnableTagCallBack(L_BOOL bEnable);
   L_BOOL EnableActionCallBack(L_BOOL bEnable);
   L_BOOL EnablePaintCallBack(L_BOOL bEnable);

#if defined (LEADTOOLS_V16_OR_LATER)
   L_BOOL EnableAnnotationCallBack (L_BOOL bEnable);
   L_BOOL EnableRegionCallBack (L_BOOL bEnable);
   L_BOOL EnableAnnotationCreatedCallBack (L_BOOL bEnable);
   L_BOOL EnableActiveSubCellChangedCallBack (L_BOOL bEnable);
   L_BOOL EnableMouseCallBack (L_BOOL bEnable);
   L_BOOL EnablePrePaintCallBack (L_BOOL bEnable);
   L_BOOL EnablePostPaintCallBack (L_BOOL bEnable);
   L_BOOL EnableLowMemoryUsageCallBack (L_BOOL bEnable);
   L_BOOL EnableAnimationStartedCallBack (L_BOOL bEnable);
   L_BOOL EnableAnimationStoppedCallBack (L_BOOL bEnable);
#endif //LEADTOOLS_V16_OR_LATER

   L_INT RepaintCell(HWND hWnd, L_INT nCellIndex, L_UINT uFlags);
   L_INT GetBitmapHandle(L_INT nCellIndex, L_INT nSubCellIndex, pBITMAPHANDLE pBitmap, L_UINT uFlags);
   L_INT SetBitmapHandle(L_INT nCellIndex, L_INT nSubCellIndex, pBITMAPHANDLE pBitmap, L_BOOL bRepaint, L_UINT uFlags);
   L_INT SetRulerUnit(L_UINT uUnit, L_UINT uFlags);
   L_INT CalibrateRuler(L_INT nCellIndex, L_INT nSubCellIndex, L_DOUBLE dLength, L_UINT uUnit, L_UINT uFlags);
   L_INT GetActionButton(L_INT nAction, L_INT * pnMouseButton, L_UINT * puFlags);
   L_INT AnnToRgn(L_INT nCellIndex, L_INT nSubCellIndex, L_UINT uCombineMode, L_BOOL bDeleteAnn, L_UINT uFlags);
   L_INT IsButtonValid(L_INT nAction, L_INT nMouseButton, L_UINT uFlags);
   L_INT StartAnimation(L_INT nCellIndex, L_INT nStartFrame, L_INT nFrameCount, L_BOOL bAnimateAllSubCells, L_UINT uFlags);
   L_INT SetAnimationProperties(L_INT nCellIndex, pDISPANIMATIONPROPS pDisAnimationProps, L_UINT uFlags);
   L_INT GetAnimationProperties(L_INT nCellIndex, pDISPANIMATIONPROPS pDisAnimationProps, L_UINT uFlags);
   L_INT StopAnimation(L_INT nCellIndex, L_UINT uFlags);
   L_INT ShowTitlebar(L_UINT uShow, L_UINT uFlags);
   L_INT SetTitlebarProperties(pDISPCONTAINERTITLEBARPROPS pDispContainerTitlebarProps, L_UINT uFlags);
   L_INT GetTitlebarProperties(pDISPCONTAINERTITLEBARPROPS pDispContainerTitlebarProps, L_UINT uFlags);
   L_INT SetIconProperties(L_INT nIconIndex, pDISPCONTAINERTITLEBARICONPROPS pDispContainerIconProps, L_UINT uFlags);
   L_INT GetIconProperties(L_INT nIconIndex, pDISPCONTAINERTITLEBARICONPROPS pDispContainerIconProps, L_UINT uFlags);
   L_INT CheckTitlebarIcon(L_INT nCellIndex, L_INT nSubCellIndex,L_INT nIconIndex, L_BOOL bCheck, L_UINT uFlags);
   L_BOOL IsTitlebarIconChecked(L_INT nCellIndex, L_INT nSubCellIndex, L_INT nIconIndex, L_UINT uFlags);
   L_BOOL IsCellAnimated(L_INT nCellIndex, L_UINT uFlags);
   L_INT GetRulerUnit(L_UINT * puUnit, L_UINT uFlags);
   L_BOOL IsTitlebarEnabled(L_UINT uFlags);
   L_INT GetSelectedAnnotationAttributes (L_INT nCellIndex, L_INT nSubCellIndex, pDISPCONTAINERANNATTRIBS pAnnAttributes, L_UINT uFlags);
   L_BOOL HandlePalette(L_UINT uMessage, WPARAM wParam, L_UINT uFlags);
   L_INT RotateAnnotationContainer(L_INT nCellIndex,L_INT nSubCellIndex,L_DOUBLE dAngle,L_UINT uFlags);
   L_INT FlipAnnotationContainer(L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_INT ReverseAnnotationContainer(L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);

   L_INT SaveAnnotation (LPTSTR pFileName, L_INT nCellIndex, L_INT nSubCellIndex, L_INT nStartPage, L_UINT uFlags);
   L_INT LoadAnnotation (LPTSTR pFileName, L_INT nCellIndex, L_INT nSubCellIndex, L_INT nStartPage, L_UINT uFlags);
   L_INT GetBitmapPixel (L_INT nCellIndex, L_INT nSubCellIndex, LPPOINT pSrcPoint, LPPOINT pBitmapPoint, L_UINT uFlags);
   L_INT LoadRegion (LPTSTR pFileName, L_INT nCellIndex, L_INT nSubCellIndex, L_INT nStartPage, L_UINT uFlags);
   L_INT SaveRegion (LPTSTR pFileName, L_INT nCellIndex, L_INT nSubCellIndex, L_INT nStartPage, L_UINT uFlags);
   L_INT SetCellRegionHandle(L_INT nCellIndex,L_INT nSubCellIndex,L_HRGN hRgn,L_UINT uCombineMode,L_UINT uFlags);
   L_INT GetCellRegionHandle(L_INT nCellIndex,L_INT nSubCellIndex,L_HRGN * phRgn,L_UINT uFlags);
   L_INT GetAnnotationContainer(L_INT nCellIndex,L_INT nSubCellIndex,HANNOBJECT * PhAnnContainer,L_UINT uFlags);
   L_INT SetAnnotationContainer(L_INT nCellIndex,L_INT nSubCellIndex,HANNOBJECT hAnnContainer,L_UINT uFlags);
   L_INT SetSubCellTag(L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uRow,L_UINT uAlign,L_UINT uType,LPTSTR pString,L_UINT uFlags);
   L_INT GetSubCellTag(L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uRow,L_UINT uAlign,DISPCELLTAGINFO * pTagInfo,L_UINT uFlags);
   L_INT EditSubCellTag(L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uRow,L_UINT uAlign,DISPCELLTAGINFO * pTagInfo,L_UINT uFlags);
   L_INT DeleteSubCellTag(L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uRow,L_UINT uAlign,L_UINT uFlags);
   L_INT GetCellTag (L_INT nCellIndex,L_UINT uRow,L_UINT uAlign,DISPCELLTAGINFO * pTagInfo,L_UINT uFlags);
   L_INT DeleteCellTag (L_INT nCellIndex,L_UINT uRow,L_UINT uAlign,L_UINT uFlags);
   L_INT EditCellTag (L_INT nCellIndex,L_UINT uRow,L_UINT uAlign,DISPCELLTAGINFO * pTagInfo,L_UINT uFlags);
   HBITMAP PrintCell (L_INT nCellIndex,L_UINT uFlags);
   HDISPCONTAINER GetHandle();

#if defined (LEADTOOLS_V16_OR_LATER)
   L_INT EnableCellLowMemoryUsage(L_INT nCellIndex,L_INT nHiddenCount,L_INT nFrameCount,DISPCONTAINERBITMAPINFO * pBitmapInfo,L_UINT uFlags);
   L_INT SetRequestedImage (L_INT nCellIndex,pBITMAPHANDLE pBitmaps,L_INT * pBitmapIndexes,L_INT nLength,L_UINT uFlags);
   L_INT FreezeSubCell (L_INT nCellIndex,L_INT nSubCellIndex,L_BOOL bFreeze,L_UINT uFlags);
   L_BOOL IsSubCellFrozen (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_INT GetCellScaleMode (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT * puScaleMode,L_UINT uFlags);
   L_INT SetCellScale (L_INT nCellIndex,L_INT nSubCellIndex,L_DOUBLE dScale,L_UINT uFlags);
   L_INT SetCellScaleMode (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uScaleMode,L_UINT uFlags);
   L_INT GetCellScale (L_INT nCellIndex,L_INT nSubCellIndex,L_DOUBLE * pdScale,L_UINT uFlags);
   L_INT UpdateCellView (HWND hWnd, L_INT nCellIndex, L_UINT uFlags);
   HBITMAP PrintSubCell (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_INT CalibrateCell (L_INT nCellIndex,L_INT nSubCellIndex,L_DOUBLE dLength,L_UINT uUnit,L_DOUBLE dTargetLength,L_UINT uTargetUnit,L_UINT uFlags);
   L_INT SetBitmapListInfo (L_INT nCellIndex,L_INT nSubCellIndex,DISPCONTAINERBITMAPINFO * pBitmapInfo,L_UINT uFlags);
   L_INT SetDefaultWindowLevelValues (L_INT nCellIndex,L_INT nSubCellIndex,L_INT nWidth,L_INT nCenter,L_UINT uFlags);
   L_INT GetDefaultWindowLevelValues (L_INT nCellIndex,L_INT nSubCellIndex,L_INT * pnWidth,L_INT * pnCenter,L_UINT uFlags);
   L_INT ResetWindowLevelValues (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_INT InvertBitmap (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_BOOL IsBitmapInverted (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_INT RotateBitmapPerspective (L_INT nCellIndex,L_INT nSubCellIndex,L_INT nAngle,L_UINT uFlags);
   L_INT GetRotateBitmapPerspectiveAngle(L_INT nCellIndex,L_INT nSubCellIndex,L_INT * pnAngle,L_UINT uFlags);
   L_INT RemoveBitmapRegion (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_INT BeginUpdate (L_UINT uFlags);
   L_INT EndUpdate (L_UINT uFlags);

   L_INT ReverseBitmap (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_BOOL IsBitmapReversed (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_INT FlipBitmap (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);
   L_BOOL IsBitmapFlipped (L_INT nCellIndex,L_INT nSubCellIndex,L_UINT uFlags);

#endif //LEADTOOLS_V16_OR_LATER


//#if defined(FOR_UNICODE)
//   L_INT SetCellTag         ( 
//      L_INT          nCellIndex,
//      L_UINT         uRow,
//      L_UINT         uAlign,
//      L_UINT         uType,
//      LPTSTR         pString,
//      L_UINT         uFlags);
//
//   L_INT SaveAnnotation( 
//      LPTSTR         pFileName,
//      L_INT          nCellIndex,
//      L_INT          nSubCellIndex,
//      L_INT          nStartPage,
//      L_UINT         uFlags);
//
//   L_INT LoadAnnotation( 
//      LPTSTR         pFileName,
//      L_INT          nCellIndex,
//      L_INT          nSubCellIndex,
//      L_INT          nStartPage,
//      L_UINT         uFlags);
//
//   L_INT LoadRegion    ( 
//      LPTSTR         pFileName,
//      L_INT          nCellIndex,
//      L_INT          nSubCellIndex,
//      L_INT          nStartPage,
//      L_UINT         uFlags);
//
//   L_INT SaveRegion    ( 
//      LPTSTR         pFileName,
//      L_INT          nCellIndex,
//      L_INT          nSubCellIndex,
//      L_INT          nStartPage,
//      L_UINT         uFlags);
//
//   L_INT SetSubCellTag( 
//      L_INT          nCellIndex,
//      L_INT          nSubCellIndex,
//      L_UINT         uRow,
//      L_UINT         uAlign,
//      L_UINT         uType,
//      LPTSTR         pString,
//      L_UINT         uFlags);

};
#endif //_LEAD_OPTIMIZE_H_
/*================================================================= EOF =====*/
