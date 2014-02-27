//==============================================================================
//
// File Name:	tmview.h
//
// Description:	This file contains the declaration of the CTMViewCtrl class.
//				This is the OCX control object for tm_view6.ocx.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-11-97	1.00		Original Release
//	01-09-98	1.10		Added methods for retrieving image width, height
//							and aspect ratio
//	01-09-98	1.10		Added Render() method
//	06-14-01	5.00		Upgraded to support LeadTools 12.0
//	02-14-2009	6.3.4		Added SaveSplitZap method
//==============================================================================
#if !defined(AFX_TMVIEW_H__FEB40E04_FA01_11D0_B002_008029EFD140__INCLUDED_)
#define AFX_TMVIEW_H__FEB40E04_FA01_11D0_B002_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmlead.h>
#include <callout.h>
#include <handler.h>
#include <tmini.h>
#include <printer.h>
#include <objsafe.h>
#include <diagnose.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Control identifiers for left and right panes
#define IDC_PANEA			200
#define IDC_PANEB			300
#define IDC_BACKUP			400

//	Timer identifiers used for asynchronous file loading
#define ASYNC_TIMER_PANEA	1
#define ASYNC_TIMER_PANEB	2

//	Event identifiers used in call to FireEvent()
#define EV_MOUSECLICK		1
#define EV_MOUSEDBLCLICK	2

#define _EVENT_DIAGNOSTICS	1

#define WM_ERROR_EVENT		(WM_USER + 1)

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMViewCtrl : public COleControl
{
	private:

		CTMPrinter			m_Printer;
		CTMVersion			m_tmVersion;
		CErrorHandler		m_Errors;
		CCallouts			m_Callouts;
		CCallout*			m_pCallout;
		CTMLead				m_PaneA;
		CTMLead				m_PaneB;
		CTMLead				m_Scratch;
		CTMLead*			m_pActive;
		CTMLead*			m_pLeft;
		CTMLead*			m_pRight;
		CTMIni				m_Zap;
		short				m_sButton;
		short				m_sKey;
		int					m_iWidth;
		int					m_iHeight;
		int					m_iTextOpen;
		RECT				m_rcLPane;
		RECT				m_rcRPane;
		RECT				m_rcRFrame;
		RECT				m_rcLFrame;
		RECT				m_rcMax;
		BOOL				m_bRedraw;
		BOOL				m_bParentNotify;
		BOOL				m_bSetCursor;
		BOOL				m_bPenTop;
		BOOL				m_bEditTextAnn;
		BOOL				m_bLoadingZap;
		
		DECLARE_DYNCREATE(CTMViewCtrl)

	public:
	
		CDiagnostics		m_Diagnostics;

							CTMViewCtrl();
						   ~CTMViewCtrl();

		void				OnAnnotationDrawn(CTMLead* pPane, long lAnnotation); 
		void				OnAnnotationDeleted(CTMLead* pPane, long lAnnotation); 
		void				OnAnnotationModified(CTMLead* pPane, long lAnnotation); 
		void				OnCalloutResized(CTMLead* pPane, CCallout* pCallout); 
		void				OnCalloutMoved(CTMLead* pPane, CCallout* pCallout); 
		void				OnCalloutCreated(CTMLead* pSource, CCallout* pCallout);
		void				OnCalloutDestroyed(CTMLead* pSource, CCallout* pCallout);
		void				OnCalloutActivated(CTMLead* pSource, CCallout* pCallout);

		void				OnOpenTextBox(CTMLead* pTMLead);
		void				OnCloseTextBox(CTMLead* pTMLead);
		void				OnStartEditTextAnn(CTMLead* pTMLead);
		void				OnEndEditTextAnn(CTMLead* pTMLead);
		void				OnAnnFontChanged(CTMLead* pTMLead);
		void				OnClickCallout(CTMLead* pTMLead, CCallout* pCallout, 
										   short sButton, short sKey);
		void				OnSavedPage(LPCSTR lpszSourceFile, LPCSTR lpszPageFile,
										short sPage, short sTotal);

		CTMLead*			GetScratchPane();
		CDiagnostics&		GetDiagnostics(){ return m_Diagnostics; }

		LONG				OnWMErrorEvent(WPARAM wParam, LPARAM lParam);

	protected:

		void				GetRegistration();
		void				RecalcLayout();
		void				ShowRectangles(LPCSTR lpTitle);
		void				DrawSplitFrame(RECT* pRect);
		void				DrawPenSelector();
		void				EraseSplitFrame(RECT* pRect);
		void				SetPaneProps(CTMLead* pPane);
		void				SetPane(CTMLead* pPane);
		void				FillZapHeader(SZapHeader* pZapHeader);
		void				SetPanePrintRect(RECT* prcBounds, RECT* prcPane, 
											 CTMLead& rwndPane, short sRotation);
		BOOL				CheckVersion(DWORD dwVersion);
		BOOL				IsOldZap(LPCTSTR lpszFilename);
		BOOL				GetOrientation(BOOL bLeft, BOOL bRight);
		CTMLead*			GetPane(short sPane = -1);
		short				GetPaneId(CTMLead* pPane);
		short				LoadOldZap(LPCTSTR lpszFilename, BOOL bUseView, 
									   BOOL bScaleView, BOOL bCallouts, 
									   short sPane);
		short				GetControlKeys();
		short				SaveZapEx(LPCTSTR lpszFilename, SZapHeader* pZapHeader, 
									  SZapFooter* pZapFooter, short sPane);

	
	//--------------------------------------------------------------------
	//	Dispatch interface for implementing object safety
	//
	//	This is required to eliminate warnings when the control is invoked
	//	directly from a browser
	DECLARE_INTERFACE_MAP()

	BEGIN_INTERFACE_PART(ObjSafety, IObjectSafety)
		STDMETHOD_(HRESULT, GetInterfaceSafetyOptions) ( 
            /* [in] */ REFIID riid,
            /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
            /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions
		);
        
        STDMETHOD_(HRESULT, SetInterfaceSafetyOptions) ( 
            /* [in] */ REFIID riid,
            /* [in] */ DWORD dwOptionSetMask,
            /* [in] */ DWORD dwEnabledOptions
		);
	END_INTERFACE_PART(ObjSafety);
	//--------------------------------------------------------------------

	public:
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTMViewCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();
	virtual DWORD GetControlFlags();
	virtual void OnBackColorChanged();
	virtual void OnEnabledChanged();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	//}}AFX_VIRTUAL

	protected:
	DECLARE_OLECREATE_EX(CTMViewCtrl)	// Class factory and guid
	DECLARE_OLETYPELIB(CTMViewCtrl)		// GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMViewCtrl)    // Property page IDs
	DECLARE_OLECTLTYPE(CTMViewCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMViewCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnPaletteChanged(CWnd* pFocusWnd);
	afx_msg void OnParentNotify(UINT message, LPARAM lParam);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnQueryNewPalette();
	afx_msg BOOL OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message);
	
	afx_msg void OnAAnimate(BOOL bEnable);
	afx_msg void OnAAnnChange(long hObject, long uType);
	afx_msg void OnAAnnClicked(long hObject);
	afx_msg void OnAAnnCreate(long hObject);
	afx_msg void OnAAnnDestroy(long hObject);
	afx_msg void OnAAnnDrawn(long hObject);
	afx_msg void OnAAnnMenu(LPDISPATCH AnnMenu);
	afx_msg void OnAAnnMouseDown(short Button, short Shift, long x, long y);
	afx_msg void OnAAnnMouseMove(short Button, short Shift, long x, long y);
	afx_msg void OnAAnnMouseUp(short Button, short Shift, long x, long y);
	afx_msg void OnAAnnSelect(const VARIANT& aObjects, short uCount);
	afx_msg void OnAAnnUserMenu(long nID);
	afx_msg void OnAKeyDown(short FAR* KeyCode, short Shift);
	afx_msg void OnAKeyPress(short FAR* KeyAscii);
	afx_msg void OnAKeyUp(short FAR* KeyCode, short Shift);
	afx_msg void OnAMouseClick();
	afx_msg void OnAMouseDblClick();
	afx_msg void OnAMouseDown(short Button, short Shift, long X, long Y);
	afx_msg void OnAMouseMove(short Button, short Shift, long x, long y);
	afx_msg void OnAMouseUp(short Button, short Shift, long x, long y);
	afx_msg void OnARubberBand();
	
	afx_msg void OnBAnimate(BOOL bEnable);
	afx_msg void OnBAnnChange(long hObject, long uType);
	afx_msg void OnBAnnClicked(long hObject);
	afx_msg void OnBAnnCreate(long hObject);
	afx_msg void OnBAnnDestroy(long hObject);
	afx_msg void OnBAnnDrawn(long hObject);
	afx_msg void OnBAnnMenu(LPDISPATCH AnnMenu);
	afx_msg void OnBAnnMouseDown(short Button, short Shift, long x, long y);
	afx_msg void OnBAnnMouseMove(short Button, short Shift, long x, long y);
	afx_msg void OnBAnnMouseUp(short Button, short Shift, long x, long y);
	afx_msg void OnBAnnSelect(const VARIANT& aObjects, short uCount);
	afx_msg void OnBAnnUserMenu(long nID);
	afx_msg void OnBKeyDown(short FAR* KeyCode, short Shift);
	afx_msg void OnBKeyPress(short FAR* KeyAscii);
	afx_msg void OnBKeyUp(short FAR* KeyCode, short Shift);
	afx_msg void OnBMouseClick();
	afx_msg void OnBMouseDblClick();
	afx_msg void OnBMouseDown(short Button, short Shift, long X, long Y);
	afx_msg void OnBMouseMove(short Button, short Shift, long x, long y);
	afx_msg void OnBMouseUp(short Button, short Shift, long x, long y);
	afx_msg void OnBRubberBand();
	
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	//===============================================================================================================================
	// UNUSED LEAD TOOLS v16.5 EVENT HANDLERS
	//===============================================================================================================================
	//afx_msg void OnAAnimationFrame(short iFrame);
	//afx_msg void OnAAnnEnumerate(long hObject);
	//afx_msg void OnAAnnHyperlink(long hObject, short iParamCount, long lParam1, long lParam2, long lParam3, long lParam4, long lParam5);
	//afx_msg void OnAAnnHyperlinkMenu(const VARIANT& aObjects, short uCount);
	//afx_msg void OnAAnnLocked(long hObject);
	//afx_msg void OnAAnnToolChecked(short iTool);
	//afx_msg void OnAAnnToolDestroy();
	//afx_msg void OnAAnnUnlocked(long hObject);
	//afx_msg void OnABitmapDataPathClosed();
	//afx_msg void OnABorderRemoveEvent(long hRgn, long uBorderToRemove, float fBoundingRectLeft, float fBoundingRectTop, float fBoundingRectWidth, float fBoundingRectHeight);
	//afx_msg void OnAChange();
	//afx_msg void OnACustomCompressedData(long hTileDIB);
	//afx_msg void OnACustomUnCompressedData(const VARIANT& vCompressedData, long lCompressedDataSize, float fCurTileWidth, float fCurTileHeight, short iBitsPerPixel);
	//afx_msg void OnADotRemoveEvent(long hRgn, float fBoundingRectLeft, float fBoundingRectTop, float fBoundingRectWidth, float fBoundingRectHeight, long iWhiteCount, long iBlackCount);
	//afx_msg void OnAFilePage();
	//afx_msg void OnAFilePageLoaded();
	//afx_msg void OnAHolePunchRemoveEvent(long hRgn, float fBoundingRectLeft, float fBoundingRectTop, float fBoundingRectWidth, float fBoundingRectHeight, long iHoleIndex, long iHoleTotalCount, long iWhiteCount, long iBlackCount);
	//afx_msg void OnAInvertedTextEvent(long hRgn, float fBoundingRectLeft, float fBoundingRectTop, float fBoundingRectWidth, float fBoundingRectHeight, long iWhiteCount, long iBlackCount);
	//afx_msg void OnALineRemoveEvent(long nStartRow, long nStartCol, long nLength, long hRgnLine);
	//afx_msg void OnALoadInfo();
	//afx_msg void OnAMagGlass(long nMaskPlaneStart, long nMaskPlaneEnd, const VARIANT& vMaskPlane);
	//afx_msg void OnAMagGlassCursor();
	//afx_msg void OnAMouseWheel(short nDelta, short Shift, long x, long y);
	//afx_msg void OnAOLECompleteDrag(short Button, short Shift, long x, long y);
	//afx_msg void OnAOLEDragOver(short Button, short Shift, long x, long y);
	//afx_msg void OnAOLEDropFile(LPCTSTR pszName);
	//afx_msg void OnAOLEGiveFeedback(BOOL* pbUseDefaultCursor);
	//afx_msg void OnAOLEStartDrag(short Button, short Shift, long x, long y);
	//afx_msg void OnAPaint();
	//afx_msg void OnAPaintNotification(short uPass, short uType);
	//afx_msg void OnAPanWin(long hPanWin, short iFlag);
	//afx_msg void OnAProgressStatus(short iPercent);
	//afx_msg void OnAReadyStateChange(long ReadyState);
	//afx_msg void OnAResize();
	//afx_msg void OnARgnChange();
	//afx_msg void OnASaveBuffer(long uRequiredSize);
	//afx_msg void OnAScroll();
	//afx_msg void OnASmoothEvent(long nBumpOrNick, long nStartRow, long nStartCol, long nLength, long uHorV);
	//afx_msg void OnATransformMarker(short iMarker, long lSize, const VARIANT& vData, short iTransform);
	//afx_msg void OnAZoomInDone(long x, long y);
	//===============================================================================================================================

	public:
	//{{AFX_DISPATCH(CTMViewCtrl)
	short m_sRedactColor;
	afx_msg void OnRedactColorChanged();
	short m_sHighlightColor;
	afx_msg void OnHighlightColorChanged();
	short m_sAnnColor;
	afx_msg void OnAnnColorChanged();
	short m_sAnnTool;
	afx_msg void OnAnnToolChanged();
	short m_sAnnThickness;
	afx_msg void OnAnnThicknessChanged();
	BOOL m_bScaleImage;
	afx_msg void OnScaleImageChanged();
	short m_sRotation;
	afx_msg void OnRotationChanged();
	short m_sMaxZoom;
	afx_msg void OnMaxZoomChanged();
	BOOL m_bFitToImage;
	afx_msg void OnFitToImageChanged();
	short m_sAction;
	afx_msg void OnActionChanged();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	CString m_strBackgroundFile;
	afx_msg void OnBackgroundFileChanged();
	BOOL m_bAutoAnimate;
	afx_msg void OnAutoAnimateChanged();
	BOOL m_bLoopAnimation;
	afx_msg void OnLoopAnimationChanged();
	BOOL m_bHideScrollBars;
	afx_msg void OnHideScrollBarsChanged();
	short m_sPanPercent;
	afx_msg void OnPanPercentChanged();
	short m_sZoomOnLoad;
	afx_msg void OnZoomOnLoadChanged();
	short m_sCalloutColor;
	afx_msg void OnCalloutColorChanged();
	short m_sCalloutFrameThickness;
	afx_msg void OnCalloutFrameThicknessChanged();
	short m_sCalloutFrameColor;
	afx_msg void OnCalloutFrameColorChanged();
	BOOL m_bRightClickPan;
	afx_msg void OnRightClickPanChanged();
	short m_sSplitFrameThickness;
	afx_msg void OnSplitFrameThicknessChanged();
	short m_sSplitFrameColor;
	afx_msg void OnSplitFrameColorChanged();
	BOOL m_bSplitScreen;
	afx_msg void OnSplitScreenChanged();
	short m_sActivePane;
	afx_msg void OnActivePaneChanged();
	BOOL m_bSyncPanes;
	afx_msg void OnSyncPanesChanged();
	CString m_strLeftFile;
	afx_msg void OnLeftFileChanged();
	CString m_strRightFile;
	afx_msg void OnRightFileChanged();
	BOOL m_bSyncCalloutAnn;
	afx_msg void OnSyncCalloutAnnChanged();
	BOOL m_bPenSelectorVisible;
	afx_msg void OnPenSelectorVisibleChanged();
	short m_sPenSelectorColor;
	afx_msg void OnPenSelectorColorChanged();
	short m_sPenSelectorSize;
	afx_msg void OnPenSelectorSizeChanged();
	BOOL m_bKeepAspect;
	afx_msg void OnKeepAspectChanged();
	short m_sBitonal;
	afx_msg void OnBitonalScalingChanged();
	BOOL m_bZoomToRect;
	afx_msg void OnZoomToRectChanged();
	CString m_strAnnFontName;
	afx_msg void OnAnnFontNameChanged();
	short m_sAnnFontSize;
	afx_msg void OnAnnFontSizeChanged();
	BOOL m_bAnnFontBold;
	afx_msg void OnAnnFontBoldChanged();
	BOOL m_bAnnFontStrikeThrough;
	afx_msg void OnAnnFontStrikeThroughChanged();
	BOOL m_bAnnFontUnderline;
	afx_msg void OnAnnFontUnderlineChanged();
	OLE_COLOR m_lDeskewBackColor;
	afx_msg void OnDeskewBackColorChanged();
	BOOL m_bAnnotateCallouts;
	afx_msg void OnAnnotateCalloutsChanged();
	short m_sPrintOrientation;
	afx_msg void OnPrintOrientationChanged();
	OLE_COLOR m_lPrintBorderColor;
	afx_msg void OnPrintBorderColorChanged();
	float m_fPrintLeftMargin;
	afx_msg void OnPrintLeftMarginChanged();
	float m_fPrintRightMargin;
	afx_msg void OnPrintRightMarginChanged();
	float m_fPrintTopMargin;
	afx_msg void OnPrintTopMarginChanged();
	float m_fPrintBottomMargin;
	afx_msg void OnPrintBottomMarginChanged();
	float m_fPrintBorderThickness;
	afx_msg void OnPrintBorderThicknessChanged();
	BOOL m_bPrintCallouts;
	afx_msg void OnPrintCalloutsChanged();
	BOOL m_bPrintBorder;
	afx_msg void OnPrintBorderChanged();
	BOOL m_bPrintCalloutBorders;
	afx_msg void OnPrintCalloutBordersChanged();
	afx_msg short GetVerBuild();
	afx_msg BSTR GetVerTextLong();
	afx_msg void Redraw();
	afx_msg short SetAnnotationProperties(long lFlags);
	afx_msg void SetPrinter();
	afx_msg short LoadFile(LPCTSTR lpszFilename, short sPane);
	afx_msg short Print(BOOL bFullImage, short sPane);
	afx_msg short DestroyCallouts(short sPane);
	afx_msg short Erase(short sPane);
	afx_msg float GetAspectRatio(short sPane);
	afx_msg short GetCurrentPage(short sPane);
	afx_msg float GetImageHeight(short sPane);
	afx_msg float GetImageWidth(short sPane);
	afx_msg short GetPageCount(short sPane);
	afx_msg short GetPanStates(short sPane);
	afx_msg float GetSrcRatio(short sPane);
	afx_msg float GetZoomFactor(short sPane);
	afx_msg short GetZoomState(short sPane);
	afx_msg BOOL IsAnimation(short sPane);
	afx_msg BOOL IsLoaded(short sPane);
	afx_msg BOOL IsPlaying(short sPane);
	afx_msg short NextPage(short sPane);
	afx_msg short PlayAnimation(BOOL bPlay, BOOL bContinuous, short sPane);
	afx_msg short PrevPage(short sPane);
	afx_msg short Realize(short sPane);
	afx_msg BOOL Pan(short sDirection, short sPane);
	afx_msg short Render(OLE_HANDLE hDc, float fLeft, float fTop, float fWidth, float fHeight, short sPane);
	afx_msg void ResetZoom(short sPane);
	afx_msg void ResizeSourceToImage(short sPane);
	afx_msg void ResizeSourceToView(short sPane);
	afx_msg void Rotate(BOOL bRedraw, short sPane);
	afx_msg void RotateCcw(BOOL bRedraw, short sPane);
	afx_msg void RotateCw(BOOL bRedraw, short sPane);
	afx_msg short Save(LPCTSTR lpszFilename, short sPane);
	afx_msg short SaveZap(LPCTSTR lpszFilename, short sPane);
	afx_msg short ShowCallouts(BOOL bShow, short sPane);
	afx_msg void ZoomFullHeight(short sPane);
	afx_msg void ZoomFullWidth(short sPane);
	afx_msg short FirstPage(short sPane);
	afx_msg short LastPage(short sPane);
	afx_msg short GetColor();
	afx_msg void SetColor(short sColor);
	afx_msg void SetData(short sPane, long lData);
	afx_msg long GetData(short sPane);
	afx_msg short GetSelectCount(short sPane);
	afx_msg short DeleteSelections(short sPane);
	afx_msg short DeleteLastAnn(short sPane);
	afx_msg short LoadZap(LPCTSTR lpszFilename, BOOL bUseView, BOOL bScaleView, BOOL bCallouts, short sPane, LPCTSTR lpszSourceFile);
	afx_msg short Copy(short sPane);
	afx_msg short Paste(short sPane);
	afx_msg long GetRGBColor(short sColor);
	afx_msg short ViewImageProperties(short sPane);
	afx_msg BOOL SetPrinterByName(LPCTSTR lpName);
	afx_msg BSTR GetDefaultPrinter();
	afx_msg BOOL CanPrint();
	afx_msg BSTR GetCurrentPrinter();
	afx_msg short GetImageProperties(long lpProperties, short sPane);
	afx_msg short Deskew(short sPane);
	afx_msg BSTR GetRegisteredPath();
	afx_msg BSTR GetClassIdString();
	afx_msg short Despeckle(short sPane);
	afx_msg short GetLeadToolError(short sPane);
	afx_msg short DotRemove(short sPane, long lMinWidth, long lMinHeight, long lMaxWidth, long lMaxHeight);
	afx_msg short HolePunchRemove(short sPane, long lMinWidth, long lMinHeight, long lMaxWidth, long lMaxHeight, long lLocation);
	afx_msg short Smooth(short sPane, long lLength, short sFavorLong);
	afx_msg short BorderRemove(short sPane, long lBorderPercent, long lWhiteNoise, long lVariance, long lLocation);
	afx_msg short Cleanup(short sPane, LPCTSTR lpszSaveAs);
	afx_msg short SetupPrintPage();
	afx_msg short PrintEx(OLE_HANDLE hDC, short bFullImage, short bRotate, short sLeft, short sTop, short sWidth, short sHeight, short sPane);
	afx_msg short RescaleZapCallouts();
	afx_msg short ShowDiagnostics(BOOL bShow);
	afx_msg long DrawSourceRectangle(short sLeft, short sTop, short sRight, short sBottom, OLE_COLOR lColor, short sTransparency, BOOL bLocked, short sPane);
	afx_msg short DeleteAnn(long lAnnotation, short sPane);
	afx_msg long DrawText(LPCTSTR lpszText, short sLeft, short sTop, short sRight, short sBottom, OLE_COLOR lColor, LPCTSTR lpszFont, short sSize, BOOL bLocked, short sPane);
	afx_msg long DrawSourceText(LPCTSTR lpszText, short sLeft, short sTop, short sRight, short sBottom, OLE_COLOR lColor, LPCTSTR lpszFont, short sSize, BOOL bLocked, short sPane);
	afx_msg short DrawTmaxRedaction(long pRedaction, short sPane);
	afx_msg short DrawTmaxRedactions(long paRedactions, short sPane);
	afx_msg short DeleteTmaxRedaction(long pRedaction, short sPane);
	afx_msg short DeleteTmaxRedactions(long paRedactions, short sPane);
	afx_msg long DrawRectangle(short sLeft, short sTop, short sRight, short sBottom, OLE_COLOR lColor, short sTransparency, BOOL bLocked, short sPane);
	afx_msg short LockAnn(long lAnnotation, short sPane);
	afx_msg short UnlockAnn(long lAnnotation, short sPane);
	afx_msg OLE_COLOR GetOleColor(short sTmviewColor);
	afx_msg short GetCalloutCount(short sPane);
	afx_msg BOOL SetPrinterProperties(OLE_HANDLE hWnd);
	afx_msg short SetProperties(LPCTSTR lpszFilename, LPCTSTR lpszSection);
	afx_msg short SaveProperties(LPCTSTR lpszFilename, LPCTSTR lpszSection);
	afx_msg short HolePunchRemove2(short sPane, long lMinHolePunches, long lMaxHolePunches, long lLocation);
	afx_msg short SavePages(LPCTSTR lpszFilename, LPCTSTR lpszFolder, LPCTSTR lpszPrefix);
	afx_msg short ShowPrinterCaps();
	afx_msg void EnableDIBPrinting(short bEnable);
	afx_msg short SwapPanes();
	afx_msg short SaveSplitZap(LPCTSTR lpszTLFilename, LPCTSTR lpszBRFilename);
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 5.1
	short m_sAnnColorDepth;
	afx_msg void OnAnnColorDepthChanged();

	//	Added in rev 5.2
	BOOL m_bSplitHorizontal;
	afx_msg void OnSplitHorizontalChanged();

	//	Added in rev 5.3
	short m_sQFactor;
	afx_msg void OnQFactorChanged();

	//	Added in rev 5.4
	BOOL m_bResizeCallouts;
	afx_msg void OnResizeCalloutsChanged();
	short m_sCalloutHandleColor;
	afx_msg void OnCalloutHandleColorChanged();

	//	Added in rev 5.7
	BOOL m_bShadeOnCallout;
	afx_msg void OnShadeOnCalloutChanged();
	short m_sCalloutShadeGrayscale;
	afx_msg void OnCalloutShadeGrayscaleChanged();

	//	Added in rev 5.8
	BOOL m_bPanCallouts;
	afx_msg void OnPanCalloutsChanged();
	BOOL m_bZoomCallouts;
	afx_msg void OnZoomCalloutsChanged();

	//	Added in rev 6.0
	BOOL m_bLoadAsync;
	afx_msg void OnLoadAsyncChanged();
	BOOL m_bEnableAxErrors;
	afx_msg void OnEnableAxErrorsChanged();

	//	Added in rev 6.1.0
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	afx_msg void AboutBox();

	//{{AFX_EVENT(CTMViewCtrl)
	void FireMouseClick(short Button, short Key)
		{FireEvent(eventidMouseClick,EVENT_PARAM(VTS_I2  VTS_I2), Button, Key);}
	void FireMouseDblClick(short Button, short Key)
		{FireEvent(eventidMouseDblClick,EVENT_PARAM(VTS_I2  VTS_I2), Button, Key);}
	void FireCreateCallout(OLE_HANDLE hCallout)
		{FireEvent(eventidCreateCallout,EVENT_PARAM(VTS_HANDLE), hCallout);}
	void FireDestroyCallout(OLE_HANDLE hCallout)
		{FireEvent(eventidDestroyCallout,EVENT_PARAM(VTS_HANDLE), hCallout);}
	void FireSelectPane(short sPane)
		{FireEvent(eventidSelectPane,EVENT_PARAM(VTS_I2), sPane);}
	void FireOpenTextBox(short sPane)
		{FireEvent(eventidOpenTextBox,EVENT_PARAM(VTS_I2), sPane);}
	void FireCloseTextBox(short sPane)
		{FireEvent(eventidCloseTextBox,EVENT_PARAM(VTS_I2), sPane);}
	void FireSelectCallout(OLE_HANDLE hCallout, short sPane)
		{FireEvent(eventidSelectCallout,EVENT_PARAM(VTS_HANDLE  VTS_I2), hCallout, sPane);}
	void FireStartTextEdit(short sPane)
		{FireEvent(eventidStartTextEdit,EVENT_PARAM(VTS_I2), sPane);}
	void FireStopTextEdit(short sPane)
		{FireEvent(eventidStopTextEdit,EVENT_PARAM(VTS_I2), sPane);}
	void FireAnnotationDeleted(long lAnnotation, short sPane)
		{FireEvent(eventidAnnotationDeleted,EVENT_PARAM(VTS_I4  VTS_I2), lAnnotation, sPane);}
	void FireAnnotationModified(long lAnnotation, short sPane)
		{FireEvent(eventidAnnotationModified,EVENT_PARAM(VTS_I4  VTS_I2), lAnnotation, sPane);}
	void FireAnnotationDrawn(long lAnnotation, short sPane)
		{FireEvent(eventidAnnotationDrawn,EVENT_PARAM(VTS_I4  VTS_I2), lAnnotation, sPane);}
	void FireCalloutResized(OLE_HANDLE hCallout, short sPane)
		{FireEvent(eventidCalloutResized,EVENT_PARAM(VTS_HANDLE  VTS_I2), hCallout, sPane);}
	void FireCalloutMoved(OLE_HANDLE hCallout, short sPane)
		{FireEvent(eventidCalloutMoved,EVENT_PARAM(VTS_HANDLE  VTS_I2), hCallout, sPane);}
	void FireAxError(LPCTSTR lpszMessage)
		{FireEvent(eventidAxError,EVENT_PARAM(VTS_BSTR), lpszMessage);}
	void FireAxDiagnostic(LPCTSTR lpszMethod, LPCTSTR lpszMessage)
		{FireEvent(eventidAxDiagnostic,EVENT_PARAM(VTS_BSTR  VTS_BSTR), lpszMethod, lpszMessage);}
	void FireSavedPage(LPCTSTR lpszSourceFile, LPCTSTR lpszPageFile, short sPage, short sTotal)
		{FireEvent(eventidSavedPage,EVENT_PARAM(VTS_BSTR  VTS_BSTR  VTS_I2  VTS_I2), lpszSourceFile, lpszPageFile, sPage, sTotal);}
	void FireMouseMove(short Button, short Shift, OLE_XPOS_PIXELS x, OLE_YPOS_PIXELS y)
		{FireEvent(DISPID_MOUSEMOVE,EVENT_PARAM(VTS_I2  VTS_I2  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS), Button, Shift, x, y);}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	// Dispatch and event IDs
	public:
	enum {
		dispidDoGestureZoom = 147L,
		dispidDoGesturePan = 146L,
		//{{AFX_DISP_ID(CTMViewCtrl)
	dispidRedactColor = 1L,
	dispidHighlightColor = 2L,
	dispidAnnColor = 3L,
	dispidAnnTool = 4L,
	dispidAnnThickness = 5L,
	dispidScaleImage = 6L,
	dispidRotation = 7L,
	dispidMaxZoom = 8L,
	dispidFitToImage = 9L,
	dispidAction = 10L,
	dispidEnableErrors = 11L,
	dispidBackgroundFile = 12L,
	dispidAutoAnimate = 13L,
	dispidLoopAnimation = 14L,
	dispidVerBuild = 53L,
	dispidHideScrollBars = 15L,
	dispidPanPercent = 16L,
	dispidZoomOnLoad = 17L,
	dispidCalloutColor = 18L,
	dispidCalloutFrameThickness = 19L,
	dispidCalloutFrameColor = 20L,
	dispidRightClickPan = 21L,
	dispidSplitFrameThickness = 22L,
	dispidSplitFrameColor = 23L,
	dispidSplitScreen = 24L,
	dispidActivePane = 25L,
	dispidSyncPanes = 26L,
	dispidLeftFile = 27L,
	dispidRightFile = 28L,
	dispidSyncCalloutAnn = 29L,
	dispidPenSelectorVisible = 30L,
	dispidPenSelectorColor = 31L,
	dispidPenSelectorSize = 32L,
	dispidKeepAspect = 33L,
	dispidBitonalScaling = 34L,
	dispidZoomToRect = 35L,
	dispidVerTextLong = 54L,
	dispidAnnFontName = 36L,
	dispidAnnFontSize = 37L,
	dispidAnnFontBold = 38L,
	dispidAnnFontStrikeThrough = 39L,
	dispidAnnFontUnderline = 40L,
	dispidDeskewBackColor = 41L,
	dispidAnnotateCallouts = 42L,
	dispidPrintOrientation = 43L,
	dispidPrintBorderColor = 44L,
	dispidPrintLeftMargin = 45L,
	dispidPrintRightMargin = 46L,
	dispidPrintTopMargin = 47L,
	dispidPrintBottomMargin = 48L,
	dispidPrintBorderThickness = 49L,
	dispidPrintCallouts = 50L,
	dispidPrintBorder = 51L,
	dispidPrintCalloutBorders = 52L,
	dispidRedraw = 55L,
	dispidSetAnnotationProperties = 56L,
	dispidSetPrinter = 57L,
	dispidLoadFile = 58L,
	dispidPrint = 59L,
	dispidDestroyCallouts = 60L,
	dispidErase = 61L,
	dispidGetAspectRatio = 62L,
	dispidGetCurrentPage = 63L,
	dispidGetImageHeight = 64L,
	dispidGetImageWidth = 65L,
	dispidGetPageCount = 66L,
	dispidGetPanStates = 67L,
	dispidGetSrcRatio = 68L,
	dispidGetZoomFactor = 69L,
	dispidGetZoomState = 70L,
	dispidIsAnimation = 71L,
	dispidIsLoaded = 72L,
	dispidIsPlaying = 73L,
	dispidNextPage = 74L,
	dispidPlayAnimation = 75L,
	dispidPrevPage = 76L,
	dispidRealize = 77L,
	dispidPan = 78L,
	dispidRender = 79L,
	dispidResetZoom = 80L,
	dispidResizeSourceToImage = 81L,
	dispidResizeSourceToView = 82L,
	dispidRotate = 83L,
	dispidRotateCcw = 84L,
	dispidRotateCw = 85L,
	dispidSave = 86L,
	dispidSaveZap = 87L,
	dispidShowCallouts = 88L,
	dispidZoomFullHeight = 89L,
	dispidZoomFullWidth = 90L,
	dispidFirstPage = 91L,
	dispidLastPage = 92L,
	dispidGetColor = 93L,
	dispidSetColor = 94L,
	dispidSetData = 95L,
	dispidGetData = 96L,
	dispidGetSelectCount = 97L,
	dispidDeleteSelections = 98L,
	dispidDeleteLastAnn = 99L,
	dispidLoadZap = 100L,
	dispidCopy = 101L,
	dispidPaste = 102L,
	dispidGetRGBColor = 103L,
	dispidViewImageProperties = 104L,
	dispidSetPrinterByName = 105L,
	dispidGetDefaultPrinter = 106L,
	dispidCanPrint = 107L,
	dispidGetCurrentPrinter = 108L,
	dispidGetImageProperties = 109L,
	dispidDeskew = 110L,
	dispidGetRegisteredPath = 111L,
	dispidGetClassIdString = 112L,
	dispidDespeckle = 113L,
	dispidGetLeadToolError = 114L,
	dispidDotRemove = 115L,
	dispidHolePunchRemove = 116L,
	dispidSmooth = 117L,
	dispidBorderRemove = 118L,
	dispidCleanup = 119L,
	dispidSetupPrintPage = 120L,
	dispidPrintEx = 121L,
	dispidRescaleZapCallouts = 122L,
	dispidShowDiagnostics = 123L,
	dispidDrawSourceRectangle = 124L,
	dispidDeleteAnn = 125L,
	dispidDrawText = 126L,
	dispidDrawSourceText = 127L,
	dispidDrawTmaxRedaction = 128L,
	dispidDrawTmaxRedactions = 129L,
	dispidDeleteTmaxRedaction = 130L,
	dispidDeleteTmaxRedactions = 131L,
	dispidDrawRectangle = 132L,
	dispidLockAnn = 133L,
	dispidUnlockAnn = 134L,
	dispidGetOleColor = 135L,
	dispidGetCalloutCount = 136L,
	dispidSetPrinterProperties = 137L,
	dispidSetProperties = 138L,
	dispidSaveProperties = 139L,
	dispidHolePunchRemove2 = 140L,
	dispidSavePages = 141L,
	dispidShowPrinterCaps = 142L,
	dispidEnableDIBPrinting = 143L,
	dispidSwapPanes = 144L,
	dispidSaveSplitZap = 145L,
	eventidMouseClick = 1L,
	eventidMouseDblClick = 2L,
	eventidCreateCallout = 3L,
	eventidDestroyCallout = 4L,
	eventidSelectPane = 5L,
	eventidOpenTextBox = 6L,
	eventidCloseTextBox = 7L,
	eventidSelectCallout = 8L,
	eventidStartTextEdit = 9L,
	eventidStopTextEdit = 10L,
	eventidAnnotationDeleted = 11L,
	eventidAnnotationModified = 12L,
	eventidAnnotationDrawn = 13L,
	eventidCalloutResized = 14L,
	eventidCalloutMoved = 15L,
	eventidAxError = 16L,
	eventidAxDiagnostic = 17L,
	eventidSavedPage = 18L,
	//}}AFX_DISP_ID
	};
public:
	void DoGesturePan(LONG lX, LONG lY);
protected:
	void DoGestureZoom(FLOAT zoomFactor, LONG lX1, LONG lY1, LONG lX2, LONG lY2);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMVIEW_H__FEB40E04_FA01_11D0_B002_008029EFD140__INCLUDED)
