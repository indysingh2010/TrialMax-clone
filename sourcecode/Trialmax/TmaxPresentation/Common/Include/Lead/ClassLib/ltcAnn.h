/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcAnn.h                                                        |
| DESC      : Annotation classes                                              |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_ANN_H_
#define  _LEAD_ANN_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LAnnotation                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnotation: public LBase
{
   LEAD_DECLAREOBJECT(LAnnotation);

   friend class LAnnotationWindow;

   public:
      L_VOID *m_extLAnnotation;
      
   protected :
      HANNOBJECT      m_hAnnObject; 
      L_UINT          m_uClassType; 

   protected : 
      L_INT    Create(L_UINT uObjectType);    
      L_UINT   GetFillPattern();
      L_INT    SetFillPattern(L_UINT uFillPattern=ANNPATTERN_SOLID,L_UINT uFlags=0);
      L_UINT   GetLineStyle();
      L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID,L_UINT uFlags=0);
      L_DOUBLE GetLineWidth();
      L_INT    SetLineWidth(L_DOUBLE dLineWidth,L_UINT uFlags=0);
      L_INT    GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      L_SIZE_T GetTextLen();
      L_INT    SetText(L_TCHAR * pText, L_UINT uFlags=0);
      L_UINT   GetPolyFillMode();
      L_INT    SetPolyFillMode(L_UINT uPolyFillMode=ANNPOLYFILL_WINDING,
                               L_UINT uFlags=0);
      L_DOUBLE GetGaugeLength();
      L_INT    SetGaugeLength(L_DOUBLE dLength, L_UINT uFlags=0);
      L_INT    GetDistance(L_DOUBLE *pdDistance,
                           L_DOUBLE *pdDistance2);      
      L_UINT   GetUnit(L_TCHAR *pUnitAbbrev, L_SIZE_T  *puUnivAbbrevLen, L_UINT * puPrecision);
      L_SIZE_T GetUnitLen();
      L_INT    SetUnit(L_UINT uUnit,L_TCHAR * pUnitAbbrev,
                       L_UINT uPrecision,L_UINT uFlags);
      L_UINT   GetPointCount();
      L_INT    GetPoints(pANNPOINT pPoints);
      L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount);
      L_INT    SetTransparent(L_BOOL  bTransparent,L_UINT uFlags=0);
      L_BOOL   GetTransparent();
      L_INT    SetDpiX(L_DOUBLE dDpiX, L_UINT uFlags=0);
      L_INT    SetDpiY(L_DOUBLE dDpiY, L_UINT uFlags=0);
      L_INT    GetBitmap(pBITMAPHANDLE pBitmap, L_UINT uStructSize);
      L_INT    GetBitmap(LBitmapBase * pLBitmap, L_UINT uStructSize);
      L_INT    SetBitmap(LBitmapBase * pBitmap, L_UINT uFlags=0);
      L_INT    SetBitmap(pBITMAPHANDLE pBitmap,L_UINT uFlags=0);
      L_INT    SetSecondaryBitmap(pBITMAPHANDLE pBitmap,L_UINT uFlags=0);
      L_INT    SetSecondaryBitmap(LBitmapBase * pBitmap, L_UINT uFlags=0);
      L_INT    SetMetafile (HMETAFILE hMetafile, 
                                     L_UINT uType, 
                                     L_UINT uFlags=0);

      L_INT    GetMetafile (HMETAFILE *phMetafile);

      L_INT    SetProtractorOptions(   L_BOOL   bAcute = TRUE,
                                       L_UINT   uUnit = ANNANGLE_DEGREES,
                                       L_TCHAR   *pszAbbrev = NULL,
                                       L_UINT   uPrecision = 2,
                                       L_DOUBLE dArcRadius = 30,
                                       L_UINT   uFlags = 0);

      L_INT    GetProtractorOptions(L_BOOL  *pbAcute = NULL,
                                    L_UINT  *puUnit = NULL,
                                    L_SIZE_T *puAbbrevLen = NULL,
                                    L_TCHAR *pszAbbrev = NULL,
                                    L_UINT  *puPrecision = NULL,
                                    L_DOUBLE *pdArcRadius = NULL);

      L_INT    GetAngle(  L_DOUBLE *pdAngle);


      L_INT    SetShowFlags (   L_UINT uShowFlags = 0, 
                                L_UINT uFlags     = 0 );

      L_INT    GetShowFlags (   L_UINT *puShowFlags);

      L_INT    SetNodes(   L_BOOL bShowNodes = TRUE, 
                           L_UINT uGapNodes = 0, 
                           L_UINT uFlags = 0);

      L_INT    GetNodes( L_BOOL *pbShowNodes, 
                         L_UINT *puGapNodes);

      L_INT    SetAutoOptions ( L_UINT uFlags);
      L_INT    GetAutoOptions ( L_UINT *puFlags);

      L_INT    GetUndoDepth ( L_UINT *puUsedLevels,
                              L_UINT *puMaxLevels);


      L_INT    SetTransparentColor ( COLORREF crTransparent, 
                                             L_UINT uFlags=0);

      L_INT    GetTransparentColor ( COLORREF * pcrTransparent);

      L_INT    GetObjectFromTag ( L_UINT32 uTag,
                                  pHANNOBJECT phTagObject,
                                  L_UINT uFlags=ANNFLAG_RECURSE | ANNFLAG_NOTCONTAINER);     

      L_INT     GetTextAlign(L_UINT *puTextAlign);
      L_INT     SetTextAlign(L_UINT uTextAlign, L_UINT uFlags);
      
      L_INT SetTextRotate(L_UINT uTextRotate, L_UINT uFlags);
      L_INT GetTextRotate(L_UINT *puTextRotate);
      
      L_INT SetTextPointerFixed(L_BOOL bPointerFixed, L_UINT uFlags);
      L_INT GetTextPointerFixed(L_BOOL *pbPointerFixed);

      L_INT SetEncryptOptions (pANNENCRYPTOPTIONS pEncryptOptions,L_UINT uFlags);
      L_INT GetEncryptOptions(pANNENCRYPTOPTIONS pEncryptOptions);
      L_INT GetSecondaryMetafile (HMETAFILE *phMetafile);
      L_INT SetPointOptions(pANNPOINTOPTIONS pPointOptions,L_UINT uFlags);
      L_INT GetPointOptions(pANNPOINTOPTIONS pPointOptions,L_UINT uStructSize);

      L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT *pCount);
      L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      L_INT DeleteUserHandle(L_INT32 nIndex);
      L_INT GetRotateAngle(L_DOUBLE *pdAngle);

      L_INT GetFixed(L_BOOL *pbFixed);
      L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      L_INT PushFixedState(L_UINT uFlags);
      L_INT PopFixedState(L_UINT uFlags);
      L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);

      L_INT SetTextRTF(L_UINT uFormat, L_TCHAR *pText, L_UINT uFlags);
      L_INT GetTextRTF(L_UINT uFormat, L_TCHAR *pText, L_SIZE_T  *puLen);
      L_INT SetTicMarkLength(L_DOUBLE dLength, L_UINT uFlags);
      L_INT GetTicMarkLength(L_DOUBLE *pdLength);
      L_INT GetDistance2(L_UINT             *puCount,
                         pANNSMARTDISTANCE   pDistance,
                         pANNSMARTDISTANCE   pTotalDistance,
                         L_UINT              uStructSize);

public:
      LAnnotation();
      virtual ~LAnnotation();

      L_BOOL  IsCreated();
      L_BOOL  IsValid();
      HANNOBJECT GetHandle();
      L_INT              SetHandle(HANNOBJECT  hAnnObject);
      virtual L_BOOL     IsClipboardReady();
      virtual L_INT      Copy(HANNOBJECT hAnnSource);
      virtual L_INT      Copy(LAnnotation&  LAnnSource);
      LAnnotation&       operator=( LAnnotation& LAnnSource);
      virtual L_INT      CopyToClipboard(L_UINT uFormat=ANNFMT_XML, 
                                         L_BOOL bSelected=FALSE,
                                         L_BOOL bEmpty=FALSE,
                                         L_BOOL fCheckMenu=FALSE);
      static  LAnnotation * CreateAnnObject(HANNOBJECT hAnnObject);
      virtual L_INT      CutToClipboard ( L_UINT uFormat=ANNFMT_XML, 
                                          L_BOOL bSelected=FALSE, 
                                          L_BOOL bEmpty=FALSE,
                                          L_BOOL fCheckMenu=FALSE);
      virtual L_INT      Define(LPPOINT pPoint, L_UINT uState);
      virtual L_INT      Define2(pANNPOINT apt, L_UINT uState);
      virtual L_INT      Destroy(L_UINT uFlags=0);
      virtual L_INT      Draw(HDC hDC,LPRECT  prcInvalid);
      virtual L_INT      Flip(pANNPOINT pCenter, L_UINT uFlags=0);
      virtual L_UINT     GetActiveState();
      virtual L_INT      GetBoundingRect(LPRECT pRect, LPRECT pRectName = NULL);
      virtual L_INT      GetContainer(pHANNOBJECT phContainer);
      virtual L_INT      GetContainer(LAnnContainer * pLContainer);

      virtual L_INT      GetHyperlink(L_UINT * puType,L_UINT * puMsg,
                                      WPARAM * pwParam, L_TCHAR * pLink, L_SIZE_T *puLen);
      virtual L_SIZE_T   GetHyperlinkLen();
      virtual L_BOOL     IsLocked();
      virtual L_DOUBLE   GetOffsetX();
      virtual L_DOUBLE   GetOffsetY();
      virtual L_INT      GetRect(pANNRECT pRect, pANNRECT pRectName = NULL);
      virtual L_UINT     GetROP2();
      virtual L_DOUBLE   GetScalarX();
      virtual L_DOUBLE   GetScalarY();
      virtual L_BOOL     IsSelected();
      virtual L_UINT32   GetTag();
      virtual L_UINT     GetType();
      virtual L_INT      GetTopContainer(pHANNOBJECT phContainer);
      virtual L_INT      GetTopContainer(LAnnContainer * pLContainer);
      virtual L_BOOL     IsVisible();
      virtual HWND       GetWnd();
      virtual L_INT      Lock(L_TCHAR * pLockKey, L_UINT uFlags=0);
      virtual L_INT      Move(L_DOUBLE dDx, L_DOUBLE dDy, L_UINT uFlags=0);
      virtual L_INT      Print(HDC hDC, LPRECT prcBounds);
      virtual L_INT      Realize(pBITMAPHANDLE pBitmap, LPRECT prcBounds);
      virtual L_INT      Realize(LBitmapBase * pLBitmap, LPRECT prcBounds);
      virtual L_INT      Resize(L_DOUBLE dFactorX, L_DOUBLE dFactorY, 
                                pANNPOINT pCenter, L_UINT uFlags=0);
      virtual L_INT      Reverse(pANNPOINT pCenter, L_UINT uFlags=0);
      virtual L_INT      Remove();
      virtual L_INT      Rotate(L_DOUBLE dAngle, pANNPOINT pCenter, L_UINT uFlags=0);
      virtual L_INT      SendToBack();
      virtual L_INT      BringToFront();
      virtual L_INT      SetActiveState(L_UINT uState=ANNACTIVE_ENABLED);
      virtual L_INT      SetHyperlink(L_UINT uType, L_UINT uMsg, WPARAM wParam, 
                                      L_TCHAR * pLink, L_UINT uFlags=0);
      virtual L_INT      SetOffsetX(L_DOUBLE dOffsetX, L_UINT uFlags=0);
      virtual L_INT      SetOffsetY(L_DOUBLE dOffsetY, L_UINT uFlags=0);
      virtual L_INT      SetROP2(L_UINT uROP2=ANNROP2_COPY, L_UINT uFlags=0);
      virtual L_INT      SetRect(pANNRECT pRect);
      virtual L_INT      SetSelected(L_BOOL bSelected=TRUE, L_UINT uFlags=0);
      virtual L_INT      SetScalarX(L_DOUBLE dScalarX, L_UINT uFlags=0);
      virtual L_INT      SetScalarY(L_DOUBLE dScalarY, L_UINT uFlags=0);
      virtual L_INT      SetTag(L_UINT32 uTag, L_UINT uFlags=0);
      virtual L_UINT     GetUserMode();
      virtual L_INT      SetVisible(L_BOOL bVisible=TRUE, L_UINT uFlags=0, L_TCHAR *pUserList = NULL);
      
      virtual L_INT      GetNameOptions(pANNNAMEOPTIONS pNameOptions, 
                                       L_UINT uStructSize);

      virtual L_INT      SetNameOptions(pANNNAMEOPTIONS pNameOptions, 
                                       L_UINT        uFlags);
      virtual L_INT      ShowLockedIcon (L_BOOL bShow=TRUE, L_UINT uFlags=0);
      virtual L_INT      Unlock(L_TCHAR * pUnlockKey, L_UINT uFlags=0);
      virtual L_BOOL   IsFontBold();
      virtual L_BOOL   IsFontItalic();
      virtual L_INT    GetFontName(L_TCHAR * pFontName, L_UINT *puLen);
      virtual L_UINT   GetFontNameLen();
      virtual L_DOUBLE GetFontSize();
      virtual L_BOOL   IsFontStrikeThrough();
      virtual L_BOOL   IsFontUnderline();
      virtual L_INT    SetFontBold(L_BOOL bFontBold=TRUE, L_UINT uFlags=0);
      virtual L_INT    SetFontItalic(L_BOOL bFontItalic=TRUE, L_UINT uFlags=0);
      virtual L_INT    SetFontName(L_TCHAR * pFontName, L_UINT uFlags=0);
      virtual L_INT    SetFontSize(L_DOUBLE dFontSize, L_UINT uFlags=0);
      virtual L_INT    SetFontStrikeThrough(L_BOOL bFontStrikeThrough=TRUE, 
                                    L_UINT uFlags=0);
      virtual L_INT    SetFontUnderline(L_BOOL bFontUnderline=TRUE,L_UINT uFlags=0);
      virtual COLORREF GetForeColor();
      virtual L_INT    SetForeColor(COLORREF crFore=RGB(255,0,0),L_UINT uFlags=0);
      virtual COLORREF GetBackColor();
      virtual L_INT    SetBackColor(COLORREF crBack=RGB(0,0,0),L_UINT uFlags=0);

      virtual L_INT    SetAutoMenuItemEnable( L_INT nObjectType, 
                                              L_UINT uItem, 
                                              L_UINT uEnable, 
                                              L_UINT uFlags,
                                              L_TCHAR *pUserList);

      virtual L_INT    GetAutoMenuItemEnable( L_INT nObjectType, 
                                              L_UINT uItem, 
                                              L_UINT *puEnable);

      virtual L_INT    SetAutoMenuState( L_INT nObjectType, 
                                         L_UCHAR *pEnable, 
                                         L_UCHAR *pEnableFlags, 
                                         L_UINT uBits, 
                                         L_UINT uFlags);

      virtual L_INT    GetAutoMenuState( L_INT nObjectType, 
                                         L_UCHAR *pEnable, 
                                         L_UCHAR *pEnableFlags, 
                                         L_UINT uBits); 

      virtual L_INT    SetUser(  L_TCHAR *pOldUser, 
                                 L_TCHAR *pNewUser, 
                                 L_UINT uFlags=0);
      
      virtual L_INT    Group ( L_UINT uFlags, 
                               L_TCHAR *pUserList=NULL);

      virtual L_INT    Ungroup ( L_UINT uFlags, 
                               L_TCHAR *pUserList=NULL);

      virtual L_INT    GetRgnHandle(HRGN *phRgn,pRGNXFORM pXForm=NULL);
      virtual L_INT    GetArea( L_SIZE_T *puCount);
      virtual L_INT    SetAutoDefaults( HANNOBJECT hAutoObject, L_UINT uFlags=0);
      virtual L_INT    SetAutoDefaults( LAnnAutomation &AnnAuto, L_UINT uFlags=0);

      static  L_INT    SetPredefinedMetafile(L_UINT uType, HMETAFILE hMetafile);
      static  L_INT    GetPredefinedMetafile(L_UINT uType,
                                             HMETAFILE * phMetafile,
                                             L_BOOL * pbEnhanced);
      
      static  L_INT    AdjustPoint(pANNPOINT pptAnchor, pANNPOINT pptMove, L_DOUBLE dAngle, L_INT nType);
      virtual L_INT SetUserData(L_UCHAR *pUserData, L_UINT uUserDataSize, L_UINT uFlags);
      virtual L_INT GetUserData(L_UCHAR *pUserData, L_UINT *puUserDataSize);
      virtual L_INT SetRestrictToContainer(L_BOOL bRestrict, L_UINT uFlags);
      virtual L_INT GetRestrictToContainer(L_BOOL * pbRestrict);
      virtual L_INT SetAutoBackColor(L_UINT uObjectType,COLORREF crBack);
      virtual L_INT GetAutoBackColor(L_UINT uObjectType,COLORREF *pcrBack);

      L_INT SetOptions (L_UINT uOptions);
      L_INT GetOptions (L_UINT *puOptions);
      L_INT GetFillMode(L_UINT * puFillMode, L_INT * pnAlpha);
      L_INT SetFillMode (L_UINT uFillMode, L_INT  nAlpha, L_UINT uFlags);
      L_INT GetRotateOptions (pANNROTATEOPTIONS pRotateOptions, L_UINT uStructSize);
      L_INT SetRotateOptions (pANNROTATEOPTIONS pRotateOptions, L_UINT uFlags);
      L_INT CalibrateRuler (L_DOUBLE dCalibrateLength, L_UINT uCalibrateUnit, L_DOUBLE dDpiRatioXtoY);
      L_INT TextEdit ();
      L_INT GetTextOptions (pANNTEXTOPTIONS pTextOptions, L_UINT uStructSize);
      L_INT SetTextOptions (pANNTEXTOPTIONS pTextOptions, L_UINT uFlags);
      L_INT GetAutoSnapCursor (L_BOOL *pbSnap);
      L_INT SetAutoSnapCursor (L_BOOL bSnap);

      L_INT GetShowStampBorder(L_BOOL *pbShowStampBorder);
      L_INT SetShowStampBorder(L_BOOL bShowStampBorder, L_UINT uFlags);
};

/*----------------------------------------------------------------------------+
| Class     : LAnnAutomation                                                  |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 27 may 1998                                                     |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnAutomation: public LAnnotation
{
   LEAD_DECLAREOBJECT(LAnnAutomation);
private:
      COLORREF  m_crHilightPen;
private :
      static   L_INT  EXT_CALLBACK EnumTextTokenTableCS (L_INT nTextTokenCount, L_INT nIndex, pANNTEXTTOKEN pTextToken, L_VOID * pUserData);
   protected :
      virtual  L_INT    EnumTextTokenTableCallBack(L_INT nTextTokenCount, L_INT nIndex, pANNTEXTTOKEN pTextToken);

   public:
      L_VOID *m_extLAnnAutomation;
      
   public : 
      LAnnAutomation(L_BOOL bCreate=TRUE);
      LAnnAutomation(HANNOBJECT& hAutomation);
      virtual ~LAnnAutomation();

      virtual  L_INT    Create();
      virtual  L_INT    GetAutoContainer(pHANNOBJECT phContainer);
      virtual  L_INT    GetAutoContainer(LAnnContainer * pLContainer);
      virtual  L_BOOL   IsAutoDrawEnabled();
      virtual  L_BOOL   IsAutoMenuEnabled();
      virtual  L_INT    GetAutoText(L_UINT uItem, L_TCHAR *pText, L_SIZE_T *puLen);
      virtual  L_SIZE_T GetAutoTextLen (L_UINT uItem);
      virtual  L_DOUBLE GetDpiX();
      virtual  L_DOUBLE GetDpiY();
      virtual  L_BOOL   IsHyperlinkMenuEnabled();
      virtual  L_UINT   GetTool();
      virtual  L_INT    SetTransparent(L_BOOL  bTransparent,L_UINT uFlags=0);
      virtual  L_INT    SetAutoContainer(HANNOBJECT hContainer);
      virtual  L_INT    SetAutoContainer(LAnnContainer * pLContainer);
      virtual  L_INT    SetAutoDrawEnabled(L_BOOL bEnable=TRUE);
      virtual  L_INT    SetAutoMenuEnabled(L_BOOL bEnable=TRUE);
      virtual  L_INT    SetAutoText(L_UINT uItem, L_TCHAR *pText);
      virtual  L_INT    SetDpiX(L_DOUBLE dDpiX, L_UINT uFlags=0);
      virtual  L_INT    SetDpiY(L_DOUBLE dDpiY, L_UINT uFlags=0);
      virtual  L_INT    SetHyperlinkMenuEnabled(L_BOOL bEnable=TRUE, L_UINT uFlags=0);
      virtual  L_INT    SetTool(L_UINT uTool=ANNTOOL_SELECT);
      virtual  L_INT    SetUndoDepth(L_UINT uLevels=5);
      virtual  L_INT    Undo(L_UINT uLevels=1);
      virtual  L_INT    SetBitmapDpiX(L_DOUBLE dDpiX, L_UINT uFlags=0);
      virtual  L_INT    SetBitmapDpiY(L_DOUBLE dDpiY, L_UINT uFlags=0);
      virtual  L_DOUBLE GetBitmapDpiX();
      virtual  L_DOUBLE GetBitmapDpiY();
      virtual  L_UINT   GetFillPattern();
      virtual  L_INT    SetFillPattern(L_UINT uFillPattern=ANNPATTERN_SOLID, 
                                       L_UINT uFlags=0);
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, 
                                     L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth,L_UINT uFlags=0);
      virtual  L_INT    GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual  L_SIZE_T GetTextLen();
      virtual  L_INT    SetText(L_TCHAR * pText,L_UINT uFlags=0);
      virtual  L_UINT   GetPolyFillMode();
      virtual  L_INT    SetPolyFillMode(L_UINT uPolyFillMode=ANNPOLYFILL_WINDING, 
                                        L_UINT uFlags=0);
      virtual  L_DOUBLE GetGaugeLength();
      virtual  L_INT    SetGaugeLength(L_DOUBLE dLength, L_UINT uFlags=0);
      virtual  L_INT    GetDistance(L_DOUBLE *pdDistance,
                                    L_DOUBLE *pdDistance2=0);      
      virtual  L_UINT   GetUnit(L_TCHAR * pUnitAbbrev, L_SIZE_T  *puUnivAbbrevLen, L_UINT * puPrecision);
      virtual  L_SIZE_T GetUnitLen();
      virtual  L_INT    SetUnit(L_UINT uUnit,
                                L_TCHAR * pUnitAbbrev,
                                L_UINT uPrecision,L_UINT uFlags=0);

      virtual  L_INT    SetAutoOptions ( L_UINT uFlags);
      virtual  L_INT    GetAutoOptions ( L_UINT *puFlags);

      virtual L_INT    GetUndoDepth ( L_UINT *puUsedLevels,
                                      L_UINT *puMaxLevels);

      virtual L_INT    SetTransparentColor ( COLORREF crTransparent, 
                                             L_UINT uFlags=0);

      virtual L_INT    GetTransparentColor ( COLORREF * pcrTransparent);

      virtual L_INT    SetNodes( L_BOOL bShowNodes = TRUE, 
                                 L_UINT uGapNodes = 0, 
                                 L_UINT uFlags = 0);

      virtual L_INT    GetNodes( L_BOOL *pbShowNodes, 
                                 L_UINT *puGapNodes);

      virtual  L_INT    SetShowFlags ( L_UINT uShowFlags = 0, 
                                       L_UINT uFlags     = 0 );

      virtual  L_INT    GetShowFlags ( L_UINT *puShowFlags);
      
      virtual  L_INT  GetTextAlign(L_UINT *puTextAlign);
      virtual  L_INT  SetTextAlign(L_UINT uTextAlign, L_UINT uFlags);
      virtual  L_INT  SetEncryptOptions(pANNENCRYPTOPTIONS pEncryptOptions,L_UINT uFlags);
      virtual  L_INT  GetEncryptOptions(pANNENCRYPTOPTIONS pEncryptOptions);
      virtual  L_INT  GetSecondaryMetafile(HMETAFILE *phMetafile);
      virtual  L_INT  SetPointOptions(pANNPOINTOPTIONS pPointOptions,L_UINT uFlags);
      virtual  L_INT  GetPointOptions(pANNPOINTOPTIONS pPointOptions,L_UINT uStructSize);
      virtual  L_INT  SetTextRotate(L_UINT uTextRotate, L_UINT uFlags);
      virtual  L_INT  GetTextRotate(L_UINT *puTextRotate);
      virtual  L_INT  SetTextPointerFixed(L_BOOL bPointerFixed, L_UINT uFlags);
      virtual  L_INT  GetTextPointerFixed(L_BOOL *pbPointerFixed);
      virtual  L_INT  InsertTextTokenTable(pANNTEXTTOKEN pTextToken);
      virtual  L_INT  EnumerateTextTokenTable();
      virtual  L_INT  DeleteTextTokenTable(L_TCHAR cToken);
      virtual  L_INT  ClearTextTokenTable();
      virtual  L_INT  GetTextExpandTokens(L_BOOL *pbTextExpandTokens);
      virtual  L_INT  SetTextExpandTokens(L_BOOL bTextExpandTokens, L_UINT uFlags);
      virtual  L_INT  GetFixed(L_BOOL *pbFixed);
      virtual  L_INT  SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT  PushFixedState(L_UINT uFlags);
      virtual  L_INT  PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
      
      virtual  L_INT  SetAutoCursor(L_UINT uItem, HCURSOR hCursor);
      virtual  L_INT  GetAutoCursor(L_UINT uItem, HCURSOR *phCursor);

      virtual  L_INT SetTextRTF(L_UINT uFormat, L_TCHAR *pText, L_UINT uFlags);
      virtual  L_INT GetTextRTF(L_UINT uFormat, L_TCHAR *pText, L_SIZE_T  *puLen);
      
      virtual  L_INT    SetTicMarkLength(L_DOUBLE dLength,
                                         L_UINT uFlags);
      virtual  L_INT    GetTicMarkLength(L_DOUBLE *pdLength);
      virtual  L_INT    SetAutoDialogFontSize(L_INT   nFontSize);
      virtual  L_INT    GetAutoDialogFontSize(L_INT * pnFontSize);
      
      virtual  L_INT    SetAutoUndoEnable(L_BOOL      bEnable);
      virtual  L_INT    GetAutoUndoEnable(L_BOOL *pbEnable);
      virtual  L_INT    AddUndoNode();
      virtual  L_INT    SetAutoHilightPen(COLORREF crHilight);
      virtual  COLORREF GetAutoHilightPen();
};
/*----------------------------------------------------------------------------+
| Class     : LAnnContainer                                                   |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnContainer: public LAnnotation      
{
   LEAD_DECLAREOBJECT(LAnnContainer);

   public:
      L_VOID *m_extLAnnContainer;
      
   private :
      static   L_INT  EXT_CALLBACK EnumerateCS (HANNOBJECT hObject,L_VOID * pUserData);
   protected :
      virtual  L_INT    EnumerateCallBack(HANNOBJECT hObject);
   public : 
      LAnnContainer();
      LAnnContainer(HWND hWnd,pANNRECT pRect,L_BOOL bVisible);
      LAnnContainer(HANNOBJECT& hContainer);
      virtual ~LAnnContainer();
      virtual L_INT     Create(HWND hWnd, pANNRECT pRect, L_BOOL bVisible=FALSE);
      virtual L_INT     Insert (LAnnotation& LAnnObject,
                                L_BOOL bStripContainer=FALSE);
      virtual L_INT     SetWnd(HWND hWnd);
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, 
                                     L_UINT uFlags=0);
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth,L_UINT uFlags=0);
      virtual  L_INT    SetPolyFillMode(L_UINT uPolyFillMode=ANNPOLYFILL_WINDING,
                                        L_UINT uFlags=0);
      virtual  L_INT    SetFillPattern(L_UINT uFillPattern=ANNPATTERN_SOLID, 
                                       L_UINT uFlags=0);
      virtual  L_INT    SetText(L_TCHAR * pText,L_UINT uFlags=0);
      virtual  L_INT    SetBitmap(pBITMAPHANDLE pBitmap, L_UINT uFlags=0);
      virtual  L_INT    SetBitmap(LBitmapBase * pBitmap, L_UINT uFlags=0);
      virtual  L_INT    SetSecondaryBitmap(pBITMAPHANDLE pBitmap,L_UINT uFlags);
      virtual  L_INT    SetSecondaryBitmap(LBitmapBase * pBitmap, L_UINT uFlags);


      virtual  L_INT    SetDpiX(L_DOUBLE dDpiX, L_UINT uFlags=0);
      virtual  L_INT    SetDpiY(L_DOUBLE dDpiY, L_UINT uFlags=0);
      virtual  L_INT    SetTransparent(L_BOOL  bTransparent,L_UINT uFlags=0);
      virtual L_INT     CopyFromClipboard (HWND hWnd);
      virtual L_INT     Enumerate(L_UINT uFlags=ANNFLAG_SELECTED, L_TCHAR *pUserList=NULL);
      virtual L_UINT    GetSelectCount();
      virtual L_INT     GetSelectItems(pHANNOBJECT pItems);
      virtual L_INT     GetSelectRect(LPRECT pRect);
      virtual L_INT     SelectRect(LPRECT pRect);
      virtual L_INT     SelectPoint(LPPOINT pPoint);
      virtual L_INT     GetItem(pHANNOBJECT phItem);
      virtual LAnnotation *  GetItem();
      virtual L_INT     Save(L_TCHAR * pFile, L_UINT uFormat = ANNFMT_XML, 
                             L_BOOL bSelected = TRUE, pSAVEFILEOPTION pSaveOptions = NULL);
      virtual L_SIZE_T  SaveOffset(L_HFILE fd, L_SSIZE_T nOffset,
                                   L_UINT uFormat = ANNFMT_XML, 
                                   L_BOOL bSelected = TRUE,
                                   pSAVEFILEOPTION pSaveOptions = NULL);
      
      virtual L_INT     SaveTag(L_UINT uFormat = ANNFMT_TIFFTAG,L_BOOL bSelected = FALSE);
      
      virtual L_INT     SaveMemory(L_UINT uFormat, L_BOOL bSelected, 
                                   HGLOBAL * phMem,
                                   L_SIZE_T * puMemSize,
                                   pSAVEFILEOPTION pSaveOptions = NULL);
      virtual L_INT     Load(L_TCHAR * pFile, pLOADFILEOPTION pLoadFileOption=NULL);
      virtual L_INT     LoadOffset(L_HFILE fd, L_SSIZE_T nOffset, L_SIZE_T nLength,pLOADFILEOPTION pLoadFileOption=NULL);
      virtual L_INT     LoadMemory(L_UCHAR * pMem, L_UINT32 uMemSize);
      virtual L_INT     Realize(pBITMAPHANDLE pBitmap, 
                                LPRECT prcBounds,L_BOOL bRedactOnly=TRUE);
      virtual L_INT     Realize(LBitmapBase * pBitmap, LPRECT prcBounds,
                                L_BOOL bRedactOnly=TRUE);
      virtual L_INT     Unrealize(pBITMAPHANDLE pBitmap, LPRECT prcBounds, 
                                  L_BOOL bSelected=TRUE);
      virtual L_INT     Unrealize(LBitmapBase * pBitmap, LPRECT prcBounds, 
                                  L_BOOL bSelected=TRUE);

      virtual L_INT     SetUserMode(L_UINT uMode=ANNUSER_DESIGN);
      virtual L_DOUBLE GetLineWidth();
      virtual L_UINT   GetLineStyle();
      virtual L_UINT   GetFillPattern();

      virtual L_INT    SetTransparentColor ( COLORREF crTransparent, 
                                             L_UINT uFlags=ANNFLAG_RECURSE | ANNFLAG_NOTCONTAINER);

      virtual L_INT    GetObjectFromTag( L_UINT32 uTag,
                                         pHANNOBJECT phTagObject,
                                         L_UINT uFlags=ANNFLAG_RECURSE | ANNFLAG_NOTCONTAINER);

      virtual L_INT    SetNodes( L_BOOL bShowNodes = TRUE, 
                                 L_UINT uGapNodes = 0, 
                                 L_UINT uFlags = ANNFLAG_RECURSE | ANNFLAG_NOTCONTAINER);

      virtual L_INT    FileInfo(L_TCHAR * pszFile, pANNFILEINFO pAnnFileInfo, L_UINT uStructSize);

      virtual L_INT    FileInfoMemory(L_UCHAR * pMem, L_UINT32 uMemSize, pANNFILEINFO pAnnFileInfo, L_UINT uStructSize);

      virtual L_INT    FileInfoOffset(L_HFILE fd, pANNFILEINFO pAnnFileInfo, L_UINT uStructSize);

      virtual L_INT    DeletePage(L_TCHAR * pszFile, L_INT32 nPage);

      virtual L_INT    DeletePageMemory(HGLOBAL hMem, L_SIZE_T * puMemSize, L_INT32 nPage);

      virtual L_INT    DeletePageOffset(L_HFILE fd, L_SSIZE_T nOffset, L_INT32 nPage);
      
      
      virtual L_INT     EncryptApply(L_UINT uEncryptFlags, 
                                     L_UINT        uFlags);
      virtual L_INT    RestrictCursor(LPRECT lpRect, LPPOINT pPoint, LPRECT prcOldClip, L_BOOL bRestrictClient);
      virtual L_INT    HitTest(LPPOINT pPoint,L_UINT * puResult,pHANNOBJECT phObjectHit,pANNHITTESTINFO pHitTestInfo,L_UINT uStructSize);
      virtual LAnnotation  *  HitTest(LPPOINT pPoint,L_UINT * puResult, pANNHITTESTINFO pHitTestInfo,L_UINT uStructSize);
      virtual L_INT Convert(LPPOINT pPoints, pANNPOINT pAnnPoints, L_INT nCount, L_INT nConvert);
      virtual L_INT SetGrouping(L_BOOL bAutoGroup,L_UINT uFlags);
      virtual L_INT GetGrouping(L_BOOL * pbAutoGroup);

};

/*----------------------------------------------------------------------------+
| Class     : LAnnText                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnText: public LAnnotation     
{
   LEAD_DECLAREOBJECT(LAnnText);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnText;
      
   public : 
      LAnnText(L_BOOL bCreate=TRUE);
      LAnnText (HANNOBJECT& hText);
      virtual ~LAnnText();

      virtual L_INT     Create();
      virtual L_INT     GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual L_SIZE_T  GetTextLen();
      virtual L_INT     SetText(L_TCHAR * pText,L_UINT uFlags=0);
      virtual  L_INT GetTextAlign(L_UINT *puTextAlign);
      virtual  L_INT SetTextAlign(L_UINT uTextAlign, L_UINT uFlags);
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnStamp                                                       |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnStamp: public LAnnotation    
{
   LEAD_DECLAREOBJECT(LAnnStamp);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnStamp;
      
   public : 
      LAnnStamp(L_BOOL bCreate=TRUE);
      LAnnStamp(HANNOBJECT& hStamp);
      virtual ~LAnnStamp();

      virtual L_INT     Create();
      virtual L_INT     GetBitmap(pBITMAPHANDLE pBitmap, L_UINT uStructSize);
      virtual L_INT     GetBitmap(LBitmapBase * pBitmap, L_UINT uStructSize);
      virtual L_INT     SetBitmap(pBITMAPHANDLE pBitmap, L_UINT uFlags=0);
      virtual L_INT     SetBitmap(LBitmapBase * pBitmap, L_UINT uFlags=0);
      virtual L_INT     SetTransparent(L_BOOL  bTransparent,L_UINT uFlags=0);
      virtual L_BOOL    GetTransparent();
      virtual L_UINT    GetFillPattern();
      virtual L_INT     SetFillPattern(L_UINT uFillPattern=ANNPATTERN_SOLID, 
                                       L_UINT uFlags=0);
      virtual L_UINT    GetLineStyle();
      virtual L_INT     SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, 
                                     L_UINT uFlags=0);
      virtual L_DOUBLE  GetLineWidth();
      virtual L_INT     SetLineWidth(L_DOUBLE dLineWidth, L_UINT uFlags=0);
      virtual L_INT     GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual L_SIZE_T  GetTextLen();
      virtual L_INT     SetText(L_TCHAR * pText, L_UINT uFlags=0);

      virtual L_INT     SetMetafile (HMETAFILE hMetafile, 
                                     L_UINT uType, 
                                     L_UINT uFlags=0);

      virtual L_INT     GetMetafile (HMETAFILE *phMetafile);
          
      virtual L_INT    SetTransparentColor ( COLORREF crTransparent, 
                                             L_UINT uFlags=0);

      virtual L_INT    GetTransparentColor ( COLORREF * pcrTransparent);
      virtual L_INT GetSecondaryMetafile(HMETAFILE *phMetafile);
      
      
      virtual L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual L_INT EnumerateHandles();
      virtual L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual L_INT GetFixed(L_BOOL *pbFixed);
      virtual L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual L_INT PushFixedState(L_UINT uFlags);
      virtual L_INT PopFixedState(L_UINT uFlags);
      virtual L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};

/*----------------------------------------------------------------------------+
| Class     : LAnnLine                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnLine: public LAnnotation     
{
   LEAD_DECLAREOBJECT(LAnnLine);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnLine;

   public : 
      LAnnLine (L_BOOL bCreate=TRUE);
      LAnnLine (HANNOBJECT& hLine);
      virtual ~LAnnLine();

      virtual  L_INT    Create();
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth, L_UINT uFlags=0);
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnPolyline                                                    |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnPolyline: public LAnnotation    
{
   LEAD_DECLAREOBJECT(LAnnPolyline);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnPolyline;
      
   public : 
      LAnnPolyline(L_BOOL bCreate=TRUE);
      LAnnPolyline(HANNOBJECT& hPolyline);
      virtual  ~LAnnPolyline();

      virtual  L_INT    Create();
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, 
                                     L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth, L_UINT uFlags=0);
      virtual  L_UINT   GetPointCount();
      virtual  L_INT    GetPoints(pANNPOINT pPoints);
      virtual  L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount);
      virtual  L_INT    SetNodes(   L_BOOL bShowNodes = TRUE, 
                                    L_UINT uGapNodes = 0, 
                                    L_UINT uFlags = 0);

      virtual  L_INT    GetNodes(L_BOOL *pbShowNodes, 
                                 L_UINT *puGapNodes);
      
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnPolygon                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnPolygon: public LAnnotation     
{
   LEAD_DECLAREOBJECT(LAnnPolygon);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnPolygon;
      
   public : 
      LAnnPolygon(L_BOOL bCreate=TRUE);
      LAnnPolygon(HANNOBJECT& hPolygon);
      virtual ~LAnnPolygon();

      virtual  L_INT    Create();
      virtual  L_UINT   GetPolyFillMode();
      virtual  L_INT    SetPolyFillMode(L_UINT uPolyFillMode=ANNPOLYFILL_WINDING, 
                                        L_UINT uFlags=0);
      virtual  L_UINT   GetFillPattern();
      virtual  L_INT    SetFillPattern(L_UINT uFillPattern=ANNPATTERN_SOLID, 
                                       L_UINT uFlags=0);
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, 
                                     L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth,L_UINT uFlags=0);

      virtual  L_UINT   GetPointCount();
      virtual  L_INT    GetPoints(pANNPOINT pPoints);
      virtual  L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount);
      virtual  L_INT    SetNodes(   L_BOOL bShowNodes = TRUE, 
                                    L_UINT uGapNodes = 0, 
                                    L_UINT uFlags = 0);

      virtual  L_INT    GetNodes(L_BOOL *pbShowNodes, 
                                 L_UINT *puGapNodes);
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnFreehand                                                    |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnFreehand: public LAnnotation    
{
   LEAD_DECLAREOBJECT(LAnnFreehand);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnFreehand;
      
   public : 
      LAnnFreehand(L_BOOL bCreate=TRUE);
      LAnnFreehand(HANNOBJECT& hFreehand);
      virtual ~LAnnFreehand();

      virtual  L_INT    Create();
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID,
                                     L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth, L_UINT uFlags=0);
      virtual  L_UINT   GetPointCount();
      virtual  L_INT    GetPoints(pANNPOINT pPoints);
      virtual  L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount);
      virtual  L_INT    SetNodes(   L_BOOL bShowNodes = TRUE, 
                                    L_UINT uGapNodes = 0, 
                                    L_UINT uFlags = 0);

      virtual  L_INT    GetNodes(L_BOOL *pbShowNodes, 
                                 L_UINT *puGapNodes);
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnRectangle                                                   |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnRectangle: public LAnnotation      
{
   LEAD_DECLAREOBJECT(LAnnRectangle);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnRectangle;
      
   public : 
      LAnnRectangle(L_BOOL bCreate=TRUE);
      LAnnRectangle(HANNOBJECT& hRectangle);
      virtual ~LAnnRectangle();

      virtual  L_INT    Create();
      virtual  L_UINT   GetFillPattern();
      virtual  L_INT    SetFillPattern(L_UINT uFillPattern=ANNPATTERN_SOLID, 
                                       L_UINT uFlags=0);
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth, L_UINT uFlags=0);
      
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnEllipse                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnEllipse: public LAnnotation     
{
   LEAD_DECLAREOBJECT(LAnnEllipse);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnEllipse;
      
   public : 
      LAnnEllipse(L_BOOL bCreate=TRUE);
      LAnnEllipse (HANNOBJECT& hEllipse);
      virtual ~LAnnEllipse();

      virtual  L_INT    Create();
      virtual  L_UINT   GetFillPattern();
      virtual  L_INT    SetFillPattern(L_UINT uFillPattern=ANNPATTERN_SOLID, 
                                       L_UINT uFlags=0);
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, 
                                     L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth, L_UINT uFlags=0);
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnPointer                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnPointer: public LAnnotation     
{
   LEAD_DECLAREOBJECT(LAnnPointer);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnPointer;
      
   public : 
      LAnnPointer(L_BOOL bCreate=TRUE);
      LAnnPointer (HANNOBJECT& hPointer);
      virtual  ~LAnnPointer();

      virtual  L_INT    Create();
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, 
                                     L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth, L_UINT uFlags=0);
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnButton                                                      |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnButton: public LAnnotation      
{
   LEAD_DECLAREOBJECT(LAnnButton);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnButton;

   public : 
      LAnnButton(L_BOOL bCreate=TRUE);
      LAnnButton (HANNOBJECT& hButton);
      virtual  ~LAnnButton();

      virtual  L_INT    Create();
      virtual  L_INT    GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual  L_SIZE_T GetTextLen();
      virtual  L_INT    SetText(L_TCHAR * pText, L_UINT uFlags=0);
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnHilite                                                      |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnHilite: public LAnnotation      
{
   LEAD_DECLAREOBJECT(LAnnHilite);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnHilite;
      
   public : 
      LAnnHilite(L_BOOL bCreate=TRUE);
      LAnnHilite (HANNOBJECT& hHilite);
      virtual  ~LAnnHilite();

      virtual  L_INT    Create();
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnRedact                                                      |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnRedact: public LAnnotation      
{
   LEAD_DECLAREOBJECT(LAnnRedact);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnRedact;
      
   public : 
      LAnnRedact(L_BOOL bCreate=TRUE);
      LAnnRedact (HANNOBJECT& hRedact);
      virtual  ~LAnnRedact();

      virtual  L_INT    Create();
      virtual  L_INT    Unrealize(pBITMAPHANDLE pBitmap, 
                                  LPRECT prcBounds);
      virtual  L_INT    Unrealize(LBitmapBase * pBitmap, 
                                  LPRECT prcBounds);
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnAudioClip                                                   |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnAudioClip: public LAnnotation      
{
   LEAD_DECLAREOBJECT(LAnnAudioClip);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 
   
   public:
      L_VOID *m_extLAnnAudioClip;

   public : 
      LAnnAudioClip(L_BOOL bCreate=TRUE);
      LAnnAudioClip (HANNOBJECT& hAudioClip);
      virtual  ~LAnnAudioClip();

      virtual  L_INT    Create();
      virtual  L_INT    GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual  L_SIZE_T GetTextLen();
      virtual  L_INT    SetText(L_TCHAR * pText, L_UINT uFlags=0);
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnHotSpot                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnHotSpot: public LAnnotation     
{
   LEAD_DECLAREOBJECT(LAnnHotSpot);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnHotSpot;
      
   public : 
      LAnnHotSpot(L_BOOL bCreate=TRUE);
      LAnnHotSpot (HANNOBJECT& hHotSpot);
      virtual  ~LAnnHotSpot();

      virtual  L_INT Create();
      virtual  L_INT GetMetafile(HMETAFILE * phMetafile);
      virtual  L_INT SetMetafile(HMETAFILE hMetafile,
                                 L_UINT uType,
                                 L_UINT uFlags = 0);
      
      virtual  L_INT GetSecondaryMetafile(HMETAFILE *phMetafile);
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnRuler                                                       |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnRuler: public LAnnotation    
{
   LEAD_DECLAREOBJECT(LAnnRuler);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnRuler;
      
   public : 
      LAnnRuler(L_BOOL bCreate=TRUE);
      LAnnRuler (HANNOBJECT& hRuler);
      virtual  ~LAnnRuler();

      virtual  L_INT    Create();
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID, 
                                     L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth,L_UINT uFlags=0);
      virtual  L_DOUBLE GetGaugeLength();
      virtual  L_INT    SetGaugeLength(L_DOUBLE dLength,L_UINT uFlags=0);
      virtual  L_INT    GetDistance(L_DOUBLE *pdDistance);      
      virtual  L_UINT   GetUnit(L_TCHAR * pUnitAbbrev, 
                                L_SIZE_T  *puUnivAbbrevLen,
                                L_UINT * puPrecision);
      virtual  L_SIZE_T GetUnitLen();
      virtual  L_INT    SetUnit(L_UINT uUnit,
                                L_TCHAR * pUnitAbbrev,
                                L_UINT uPrecision,L_UINT uFlags=0);

      virtual  L_INT    GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual  L_SIZE_T GetTextLen();
      virtual  L_INT    SetShowFlags ( L_UINT uShowFlags = 0, 
                                       L_UINT uFlags     = 0 );

      virtual  L_INT    GetShowFlags ( L_UINT *puShowFlags);
      virtual  L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount);
      virtual  L_INT    GetPoints(pANNPOINT pPoints);
      
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
      virtual  L_INT    SetTicMarkLength(L_DOUBLE dLength,
                                         L_UINT uFlags);
      virtual  L_INT    GetTicMarkLength(L_DOUBLE *pdLength);
      virtual  L_INT    GetDistance2(L_UINT             *puCount,
                         pANNSMARTDISTANCE   pDistance,
                         pANNSMARTDISTANCE   pTotalDistance,
                         L_UINT              uStructSize);
};
/*----------------------------------------------------------------------------+
| Class     : LAnnNote                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnNote: public LAnnotation     
{
   LEAD_DECLAREOBJECT(LAnnNote);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnNote;
      
   public : 
      LAnnNote(L_BOOL bCreate=TRUE);
      LAnnNote (HANNOBJECT& hNote);
      virtual  ~LAnnNote();

      virtual  L_INT    Create();
      virtual  L_INT    GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual  L_SIZE_T GetTextLen();
      virtual  L_INT    SetText(L_TCHAR * pText, L_UINT uFlags=0);
      
      virtual  L_INT     GetTextAlign(L_UINT *puTextAlign);
      virtual  L_INT     SetTextAlign(L_UINT uTextAlign, L_UINT uFlags);
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT SetTextRotate(L_UINT uTextRotate, L_UINT uFlags);
      virtual  L_INT GetTextRotate(L_UINT *puTextRotate);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};

/*----------------------------------------------------------------------------+
| Class     : LAnnToolBar                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnToolBar : public LBase
{
   LEAD_DECLAREOBJECT(LAnnToolBar);

   public:
      L_VOID *m_extLAnnToolBar;
      
   protected:  
      HWND m_hWndToolBar;

   public : 
      LAnnToolBar();
      virtual ~LAnnToolBar();

      virtual L_BOOL IsCreated();
      virtual HWND   GetWndHandle();
      virtual HWND   Create(  HWND hWndParent,
                              LPPOINT pPoint,
                              L_UINT uAlign,
                              L_BOOL bVisible,
                              L_UINT uButtons=NULL,
                              pANNBUTTON pButtons=NULL);

      virtual L_INT  SetButtonVisible(L_UINT uButton,L_BOOL bVisible);
      virtual L_BOOL IsButtonVisible(L_UINT uButton);
      virtual L_UINT GetToolChecked();
      virtual L_INT  SetToolChecked(L_UINT uChecked);
         
      virtual L_INT  SetToolBarButtons(pANNBUTTON pButtons,L_UINT uButtons);
      virtual L_INT  GetToolBarButtons(pANNBUTTON pButtons, L_UINT uStructSize,L_UINT *puButtons);
      virtual L_INT  FreeToolBarButtons(pANNBUTTON pButtons,L_UINT uButtons);
      virtual L_INT  GetToolBarInfo(pANNTOOLBARINFO pInfo, L_UINT uStructSize);
      virtual L_INT  SetToolBarColumns(L_UINT uColumns);
      virtual L_INT  SetToolBarRows(L_UINT uRows);

      virtual L_INT  Destroy();
};

/*----------------------------------------------------------------------------+
| Class     : LAnnPushPin                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnPushPin: public LAnnNote     
{
   LEAD_DECLAREOBJECT(LAnnPushPin);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnPushPin;
      
   public : 
      LAnnPushPin (L_BOOL bCreate=TRUE);
      LAnnPushPin (HANNOBJECT& hPushPin);
      virtual ~LAnnPushPin();

      virtual  L_INT    Create();
      virtual  L_INT    SetBitmap(pBITMAPHANDLE pBitmap,L_UINT uFlags);
      virtual  L_INT    SetBitmap(LBitmapBase * pBitmap, L_UINT uFlags);
      virtual  L_INT    GetBitmap(pBITMAPHANDLE pBitmap, L_UINT uStructSize);
      virtual  L_INT    GetBitmap(LBitmapBase * pLBitmap, L_UINT uStructSize);
      virtual  L_INT    SetSecondaryBitmap(pBITMAPHANDLE pBitmap,L_UINT uFlags);
      virtual  L_INT    SetSecondaryBitmap(LBitmapBase * pBitmap, L_UINT uFlags);
      virtual  L_INT    GetSecondaryBitmap(pBITMAPHANDLE pBitmap, L_UINT uStructSize);
      virtual  L_INT    GetSecondaryBitmap(LBitmapBase * pLBitmap, L_UINT uStructSize);

      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};


/*----------------------------------------------------------------------------+
| Class     : LAnnVideo                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnVideo: public LAnnotation     
{
   LEAD_DECLAREOBJECT(LAnnVideo);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnVideo;
      
   public : 
      LAnnVideo (L_BOOL bCreate=TRUE);
      LAnnVideo (HANNOBJECT& hVideo);
      virtual ~LAnnVideo();

      virtual  L_INT    Create();
      virtual  L_INT    GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual  L_SIZE_T GetTextLen();
      virtual  L_INT    SetText(L_TCHAR * pText, L_UINT uFlags=0);
      
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};

/*----------------------------------------------------------------------------+
| Class     : LAnnProtractor                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnProtractor: public LAnnRuler     
{
   LEAD_DECLAREOBJECT(LAnnProtractor);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnProtractor;
      
   public : 
      LAnnProtractor (L_BOOL bCreate=TRUE);
      LAnnProtractor (HANNOBJECT& hVideo);
      virtual ~LAnnProtractor();

      virtual  L_INT    Create();
      virtual  L_UINT   GetPointCount();
      virtual  L_INT    GetPoints(pANNPOINT pPoints);
      virtual  L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount=3);
      virtual  L_INT    SetProtractorOptions(   L_BOOL   bAcute = TRUE,
                                                L_UINT   uUnit = ANNANGLE_DEGREES,
                                                L_TCHAR *pszAbbrev = NULL,
                                                L_UINT   uPrecision = 2,
                                                L_DOUBLE dArcRadius = 30,
                                                L_UINT   uFlags = 0);

      virtual L_INT     GetProtractorOptions(L_BOOL  *pbAcute = NULL,
                                             L_UINT  *puUnit = NULL,
                                             L_SIZE_T*puAbbrevLen = NULL,
                                             L_TCHAR *pszAbbrev = NULL,
                                             L_UINT  *puPrecision = NULL,
                                             L_DOUBLE *pdArcRadius = NULL);

      virtual L_INT    GetAngle(  L_DOUBLE *pdAngle);
      virtual  L_INT    GetDistance(L_DOUBLE *pdDistance,
                                    L_DOUBLE *pdDistance2);      
      
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
      virtual  L_INT    GetDistance2(L_UINT             *puCount,
                         pANNSMARTDISTANCE   pDistance,
                         pANNSMARTDISTANCE   pTotalDistance,
                         L_UINT              uStructSize);

};

/*----------------------------------------------------------------------------+
| Class     : LAnnCrossProduct                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnCrossProduct: public LAnnRuler     
{
   LEAD_DECLAREOBJECT(LAnnCrossProduct);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnCrossProduct;
      
   public : 
      LAnnCrossProduct (L_BOOL bCreate=TRUE);
      LAnnCrossProduct (HANNOBJECT& hVideo);
      virtual ~LAnnCrossProduct();

      virtual  L_INT    Create();
      virtual  L_UINT   GetPointCount();
      virtual  L_INT    GetPoints(pANNPOINT pPoints);
      virtual  L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount=5);
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID,L_UINT uFlags=0);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth,L_UINT uFlags=0);
      virtual  L_INT    GetDistance(L_DOUBLE *pdDistance,
                                    L_DOUBLE *pdDistance2);      
      
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
      virtual  L_INT    GetDistance2(L_UINT             *puCount,
                         pANNSMARTDISTANCE   pDistance,
                         pANNSMARTDISTANCE   pTotalDistance,
                         L_UINT              uStructSize);

};

/*----------------------------------------------------------------------------+
| Class     : LAnnFreehandHotSpot                                             |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnFreehandHotSpot: public LAnnFreehand
{
   LEAD_DECLAREOBJECT(LAnnFreehandHotSpot);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnFreehandHotSpot;
      
   public : 
      LAnnFreehandHotSpot (L_BOOL bCreate=TRUE);
      LAnnFreehandHotSpot (HANNOBJECT& hFreehandHotSpot);
      virtual ~LAnnFreehandHotSpot();

      virtual  L_INT    Create();     
      virtual  L_INT    GetMetafile(HMETAFILE * phMetafile);
      virtual  L_INT    SetMetafile(HMETAFILE hMetafile,
                                    L_UINT uType,
                                    L_UINT uFlags = 0);
      virtual  L_INT    GetSecondaryMetafile(HMETAFILE *phMetafile);
      
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};

/*----------------------------------------------------------------------------+
| Class     : LAnnPoint                                                       |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnPoint:public LAnnotation
{
   LEAD_DECLAREOBJECT(LAnnPoint);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnPoint;
      
   public : 
      LAnnPoint (L_BOOL bCreate=TRUE);
      LAnnPoint (HANNOBJECT& hPoint);      
      virtual ~LAnnPoint();
      virtual  L_INT    Create();

      virtual L_INT     GetBitmap(pBITMAPHANDLE pBitmap, L_UINT uStructSize);
      virtual L_INT     GetBitmap(LBitmapBase * pBitmap, L_UINT uStructSize);
      virtual L_INT     SetBitmap(pBITMAPHANDLE pBitmap, L_UINT uFlags=0);
      virtual L_INT     SetBitmap(LBitmapBase * pBitmap, L_UINT uFlags=0);
      virtual L_INT     SetTransparent(L_BOOL  bTransparent,L_UINT uFlags=0);
      virtual L_BOOL    GetTransparent();
      virtual L_INT     SetTransparentColor ( COLORREF crTransparent,L_UINT uFlags=0);
      virtual L_INT     GetTransparentColor ( COLORREF * pcrTransparent);
      virtual L_INT     GetPoints(pANNPOINT pPoint);
      virtual L_INT     SetPoints(pANNPOINT pPoint);
      virtual L_INT     SetPointOptions (pANNPOINTOPTIONS pPointOptions, L_UINT uFlags);
      virtual L_INT     GetPointOptions(pANNPOINTOPTIONS pPointOptions,L_UINT uStructSize);
      
      
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
      
      static  L_INT SetPredefinedBitmap(L_UINT uType, 
                                            pBITMAPHANDLE pBitmap);

      static  L_INT SetPredefinedBitmap(L_UINT uType, 
                                            LBitmapBase * pBitmap);
      
      static  L_INT GetPredefinedBitmap(L_UINT uType, 
                                            pBITMAPHANDLE pBitmap,
                                            L_UINT uStructSize);
      static  L_INT GetPredefinedBitmap(L_UINT uType, 
                                            LBitmapBase * pBitmap,
                                            L_UINT uStructSize);
};

/*----------------------------------------------------------------------------+
| Class     : LAnnTextPointer                                                 |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : October 2003                                                    |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnTextPointer:public LAnnotation
{
   LEAD_DECLAREOBJECT(LAnnTextPointer);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnTextPointer;
      
   public : 
      LAnnTextPointer (L_BOOL bCreate=TRUE);
      LAnnTextPointer (HANNOBJECT& hPoint);      
      virtual ~LAnnTextPointer();
      virtual  L_INT    Create();

      
         virtual  L_INT GetTextAlign(L_UINT *puTextAlign);
         virtual  L_INT SetTextAlign(L_UINT uTextAlign, L_UINT uFlags);
         virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
         virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
         virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
         virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
         virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
         virtual  L_INT EnumerateHandles();
         virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
         virtual  L_INT SetTextRotate(L_UINT uTextRotate, L_UINT uFlags);
         virtual  L_INT GetTextRotate(L_UINT *puTextRotate);
      virtual  L_INT SetTextPointerFixed(L_BOOL bPointerFixed, L_UINT uFlags);
      virtual  L_INT GetTextPointerFixed(L_BOOL *pbPointerFixed);
         virtual  L_INT GetFixed(L_BOOL *pbFixed);
         virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
         virtual  L_INT PushFixedState(L_UINT uFlags);
         virtual  L_INT PopFixedState(L_UINT uFlags);
         virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
};

/*----------------------------------------------------------------------------+
| Class     : LAnnEncrypt                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : October 2003                                                    |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnEncrypt:public LAnnotation
{
   LEAD_DECLAREOBJECT(LAnnEncrypt);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnEncrypt;
      
   public : 
      LAnnEncrypt (L_BOOL bCreate=TRUE);
      LAnnEncrypt (HANNOBJECT& hEncrypt);
      virtual ~LAnnEncrypt();
      virtual  L_INT    Create();
      virtual  L_INT SetEncryptOptions (pANNENCRYPTOPTIONS pEncryptOptions,L_UINT uFlags);
      virtual  L_INT GetEncryptOptions(pANNENCRYPTOPTIONS pEncryptOptions);
      virtual  L_INT GetMetafile(HMETAFILE * phMetafile);
      virtual  L_INT SetMetafile(HMETAFILE hMetafile,
                                 L_UINT uType,
                                 L_UINT uFlags = 0);
      virtual  L_INT GetSecondaryMetafile(HMETAFILE *phMetafile);
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetRotateAngle(L_DOUBLE *pdAngle);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth,L_UINT uFlags=0);
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID,
                                     L_UINT uFlags=0);

};

/*----------------------------------------------------------------------------+
| Class     : LAnnRTF                                                         |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : October 2003                                                    |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnRTF:public LAnnotation
{
   LEAD_DECLAREOBJECT(LAnnRTF);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnRTF;
      
   public : 
      LAnnRTF (L_BOOL bCreate=TRUE);
      LAnnRTF (HANNOBJECT& hRTF);      
      virtual ~LAnnRTF();
      virtual  L_INT    Create();
      virtual  L_INT    GetText(L_TCHAR * pText, L_SIZE_T *puLen);
      virtual  L_SIZE_T GetTextLen();
      virtual  L_INT    SetText(L_TCHAR * pText, L_UINT uFlags=0);
      virtual  L_INT    AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT    GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT    GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT    ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT    DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT    EnumerateHandles();
      virtual  L_INT    GetFixed(L_BOOL *pbFixed);
      virtual  L_INT    SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT    PushFixedState(L_UINT uFlags);
      virtual  L_INT    PopFixedState(L_UINT uFlags);
      virtual  L_BOOL   IsFixedInRect(LPRECT prc, L_UINT uFlags);
      virtual  L_INT    SetTextRTF(L_UINT uFormat, L_TCHAR *pText, L_UINT uFlags);
      virtual  L_INT    GetTextRTF(L_UINT uFormat, L_TCHAR *pText, L_SIZE_T *puLen);
};

/*----------------------------------------------------------------------------+
| Class     : LAnnCurve                                                       |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : October 2003                                                    |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnCurve:public LAnnotation
{
   LEAD_DECLAREOBJECT(LAnnCurve);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnCurve;
      
   public : 
      LAnnCurve (L_BOOL bCreate=TRUE);
      LAnnCurve (HANNOBJECT& hCurve);      
      virtual ~LAnnCurve();
      virtual  L_INT Create();
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
      virtual  L_UINT   GetPointCount();
      virtual  L_INT    GetPoints(pANNPOINT pPoints);
      virtual  L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount);

};

/*----------------------------------------------------------------------------+
| Class     : LAnnCurveClosed                                                 |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : October 2003                                                    |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnCurveClosed:public LAnnotation
{
   LEAD_DECLAREOBJECT(LAnnCurveClosed);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnCurveClosed;
      
   public : 
      LAnnCurveClosed (L_BOOL bCreate=TRUE);
      LAnnCurveClosed (HANNOBJECT& hCurveClosed);
      virtual ~LAnnCurveClosed();
      virtual  L_INT Create();
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
      virtual  L_UINT   GetPointCount();
      virtual  L_INT    GetPoints(pANNPOINT pPoints);
      virtual  L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount);

};

/*----------------------------------------------------------------------------+
| Class     : LAnnPolyRuler                                                   |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : October 2003                                                    |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LAnnPolyRuler:public LAnnotation
{
   LEAD_DECLAREOBJECT(LAnnPolyRuler);
   protected :
      virtual  L_INT    EnumHandleCallBack(HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo);

   private :
      static   L_INT  EXT_CALLBACK EnumHandleCS (HANNOBJECT hObject, pANNHANDLEINFO pHandleInfo, L_VOID * pUserData); 

   public:
      L_VOID *m_extLAnnPolyRuler;
      
   public : 

      LAnnPolyRuler (L_BOOL bCreate=TRUE);
      LAnnPolyRuler (HANNOBJECT& hPolyRuler);      
      virtual ~LAnnPolyRuler();
      virtual  L_INT Create();
      virtual  L_INT AddUserHandle(pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandle(L_INT32 uIndex, pANNHANDLE pAnnHandle);
      virtual  L_INT GetUserHandles(pANNHANDLE pAnnHandle,L_UINT    *pCount);
      virtual  L_INT ChangeUserHandle(L_INT32 nIndex,pANNHANDLE pAnnHandle);
      virtual  L_INT DeleteUserHandle(L_INT32 nIndex);
      virtual  L_INT EnumerateHandles();
      virtual  L_INT GetFixed(L_BOOL *pbFixed);
      virtual  L_INT SetFixed(L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags); 
      virtual  L_INT PushFixedState(L_UINT uFlags);
      virtual  L_INT PopFixedState(L_UINT uFlags);
      virtual  L_BOOL IsFixedInRect(LPRECT prc, L_UINT uFlags);
      virtual L_UINT   GetPointCount();
      virtual L_INT    GetPoints(pANNPOINT pPoints);
      virtual L_INT    SetPoints(pANNPOINT pPoints, L_UINT uCount);
      virtual  L_DOUBLE GetLineWidth();
      virtual  L_INT    SetLineWidth(L_DOUBLE dLineWidth,L_UINT uFlags=0);
      virtual  L_UINT   GetLineStyle();
      virtual  L_INT    SetLineStyle(L_UINT uLineStyle=ANNLINE_SOLID,
                                     L_UINT uFlags=0);
      virtual  L_INT    SetNodes(L_BOOL bShowNodes = TRUE, 
                                    L_UINT uGapNodes = 0, 
                                    L_UINT uFlags = 0);

      virtual  L_INT    GetNodes(L_BOOL *pbShowNodes, 
                                 L_UINT *puGapNodes);

      virtual  L_DOUBLE GetGaugeLength();
      virtual  L_INT    SetGaugeLength(L_DOUBLE dLength,L_UINT uFlags=0);
      virtual  L_UINT   GetUnit(L_TCHAR * pUnitAbbrev, 
                                L_SIZE_T *puUnivAbbrevLen,
                                L_UINT * puPrecision);
      virtual  L_INT    SetUnit(L_UINT uUnit,
                                L_TCHAR * pUnitAbbrev,
                                L_UINT uPrecision,L_UINT uFlags=0);
      virtual  L_INT    SetTicMarkLength(L_DOUBLE dLength,
                                         L_UINT uFlags);
      virtual  L_INT    GetTicMarkLength(L_DOUBLE *pdLength);
      virtual  L_INT    GetDistance(L_DOUBLE *pdDistance,
                                    L_DOUBLE *pdDistance2=0);      
      virtual  L_INT    GetDistance2(L_UINT             *puCount,
                         pANNSMARTDISTANCE   pDistance,
                         pANNSMARTDISTANCE   pTotalDistance,
                         L_UINT              uStructSize);
      virtual  L_SIZE_T GetUnitLen();
};
#endif //_LEAD_ANN_H_
/*================================================================= EOF =====*/