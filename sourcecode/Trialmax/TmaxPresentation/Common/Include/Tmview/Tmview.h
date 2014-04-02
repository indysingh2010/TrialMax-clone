#if !defined(AFX_TMVIEW_H__A5385279_9610_4B4D_8BB3_03897ACF682E__INCLUDED_)
#define AFX_TMVIEW_H__A5385279_9610_4B4D_8BB3_03897ACF682E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.

/////////////////////////////////////////////////////////////////////////////
// CTm_view wrapper class

class CTm_view : public CWnd
{
protected:
	DECLARE_DYNCREATE(CTm_view)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0x5a3a9fc9, 0xd747, 0x4b92, { 0x91, 0x6, 0xa3, 0x2c, 0x7e, 0x6e, 0x84, 0xa3 } };
		return clsid;
	}
	virtual BOOL Create(LPCTSTR lpszClassName,
		LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect,
		CWnd* pParentWnd, UINT nID,
		CCreateContext* pContext = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID); }

    BOOL Create(LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect, CWnd* pParentWnd, UINT nID,
		CFile* pPersist = NULL, BOOL bStorage = FALSE,
		BSTR bstrLicKey = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID,
		pPersist, bStorage, bstrLicKey); }

// Attributes
public:
	OLE_COLOR GetBackColor();
	void SetBackColor(OLE_COLOR);
	short GetBorderStyle();
	void SetBorderStyle(short);
	BOOL GetEnabled();
	void SetEnabled(BOOL);
	short GetRedactColor();
	void SetRedactColor(short);
	short GetHighlightColor();
	void SetHighlightColor(short);
	short GetAnnColor();
	void SetAnnColor(short);
	short GetAnnTool();
	void SetAnnTool(short);
	short GetAnnThickness();
	void SetAnnThickness(short);
	BOOL GetScaleImage();
	void SetScaleImage(BOOL);
	short GetRotation();
	void SetRotation(short);
	short GetMaxZoom();
	void SetMaxZoom(short);
	BOOL GetFitToImage();
	void SetFitToImage(BOOL);
	short GetAction();
	void SetAction(short);
	BOOL GetEnableErrors();
	void SetEnableErrors(BOOL);
	CString GetBackgroundFile();
	void SetBackgroundFile(LPCTSTR);
	BOOL GetAutoAnimate();
	void SetAutoAnimate(BOOL);
	BOOL GetLoopAnimation();
	void SetLoopAnimation(BOOL);
	short GetVerBuild();
	void SetVerBuild(short);
	BOOL GetHideScrollBars();
	void SetHideScrollBars(BOOL);
	short GetPanPercent();
	void SetPanPercent(short);
	short GetZoomOnLoad();
	void SetZoomOnLoad(short);
	short GetCalloutColor();
	void SetCalloutColor(short);
	short GetCalloutFrameThickness();
	void SetCalloutFrameThickness(short);
	short GetCalloutFrameColor();
	void SetCalloutFrameColor(short);
	BOOL GetRightClickPan();
	void SetRightClickPan(BOOL);
	short GetSplitFrameThickness();
	void SetSplitFrameThickness(short);
	short GetSplitFrameColor();
	void SetSplitFrameColor(short);
	BOOL GetSplitScreen();
	void SetSplitScreen(BOOL);
	short GetActivePane();
	void SetActivePane(short);
	BOOL GetSyncPanes();
	void SetSyncPanes(BOOL);
	CString GetLeftFile();
	void SetLeftFile(LPCTSTR);
	CString GetRightFile();
	void SetRightFile(LPCTSTR);
	BOOL GetSyncCalloutAnn();
	void SetSyncCalloutAnn(BOOL);
	BOOL GetPenSelectorVisible();
	void SetPenSelectorVisible(BOOL);
	short GetPenSelectorColor();
	void SetPenSelectorColor(short);
	short GetPenSelectorSize();
	void SetPenSelectorSize(short);
	BOOL GetKeepAspect();
	void SetKeepAspect(BOOL);
	short GetBitonalScaling();
	void SetBitonalScaling(short);
	BOOL GetZoomToRect();
	void SetZoomToRect(BOOL);
	CString GetVerTextLong();
	void SetVerTextLong(LPCTSTR);
	CString GetAnnFontName();
	void SetAnnFontName(LPCTSTR);
	short GetAnnFontSize();
	void SetAnnFontSize(short);
	BOOL GetAnnFontBold();
	void SetAnnFontBold(BOOL);
	BOOL GetAnnFontStrikeThrough();
	void SetAnnFontStrikeThrough(BOOL);
	BOOL GetAnnFontUnderline();
	void SetAnnFontUnderline(BOOL);
	unsigned long GetDeskewBackColor();
	void SetDeskewBackColor(unsigned long);
	BOOL GetAnnotateCallouts();
	void SetAnnotateCallouts(BOOL);
	short GetPrintOrientation();
	void SetPrintOrientation(short);
	unsigned long GetPrintBorderColor();
	void SetPrintBorderColor(unsigned long);
	float GetPrintLeftMargin();
	void SetPrintLeftMargin(float);
	float GetPrintRightMargin();
	void SetPrintRightMargin(float);
	float GetPrintTopMargin();
	void SetPrintTopMargin(float);
	float GetPrintBottomMargin();
	void SetPrintBottomMargin(float);
	float GetPrintBorderThickness();
	void SetPrintBorderThickness(float);
	BOOL GetPrintCallouts();
	void SetPrintCallouts(BOOL);
	BOOL GetPrintBorder();
	void SetPrintBorder(BOOL);
	BOOL GetPrintCalloutBorders();
	void SetPrintCalloutBorders(BOOL);
	short GetAnnColorDepth();
	void SetAnnColorDepth(short);
	BOOL GetSplitHorizontal();
	void SetSplitHorizontal(BOOL);
	short GetQFactor();
	void SetQFactor(short);
	BOOL GetResizeCallouts();
	void SetResizeCallouts(BOOL);
	unsigned long GetCalloutHandleColor();
	void SetCalloutHandleColor(unsigned long);
	BOOL GetShadeOnCallout();
	void SetShadeOnCallout(BOOL);
	short GetCalloutShadeGrayscale();
	void SetCalloutShadeGrayscale(short);
	BOOL GetPanCallouts();
	void SetPanCallouts(BOOL);
	BOOL GetZoomCallouts();
	void SetZoomCallouts(BOOL);
	BOOL GetLoadAsynch();
	void SetLoadAsynch(BOOL);
	BOOL GetEnableAxErrors();
	void SetEnableAxErrors(BOOL);
	short GetVerMajor();
	void SetVerMajor(short);
	short GetVerMinor();
	void SetVerMinor(short);
	short GetVerQEF();
	void SetVerQEF(short);
	CString GetVerTextShort();
	void SetVerTextShort(LPCTSTR);
	CString GetVerBuildDate();
	void SetVerBuildDate(LPCTSTR);

// Operations
public:
	void Redraw();
	short SetAnnotationProperties(long lFlags);
	void SetPrinter();
	short LoadFile(LPCTSTR lpszFilename, short sPane);
	short Print(BOOL bFullImage, short sPane);
	short DestroyCallouts(short sPane);
	short Erase(short sPane);
	float GetAspectRatio(short sPane);
	short GetCurrentPage(short sPane);
	float GetImageHeight(short sPane);
	float GetImageWidth(short sPane);
	short GetPageCount(short sPane);
	short GetPanStates(short sPane);
	float GetSrcRatio(short sPane);
	float GetZoomFactor(short sPane);
	short GetZoomState(short sPane);
	BOOL IsAnimation(short sPane);
	BOOL IsLoaded(short sPane);
	BOOL IsPlaying(short sPane);
	short NextPage(short sPane);
	short PlayAnimation(BOOL bPlay, BOOL bContinuous, short sPane);
	short PrevPage(short sPane);
	short Realize(short sPane);
	BOOL Pan(short sDirection, short sPane);
	short Render(long hDc, float fLeft, float fTop, float fWidth, float fHeight, short sPane);
	void ResetZoom(short sPane);
	void ResizeSourceToImage(short sPane);
	void ResizeSourceToView(short sPane);
	void Rotate(BOOL bRedraw, short sPane);
	void RotateCcw(BOOL bRedraw, short sPane);
	void RotateCw(BOOL bRedraw, short sPane);
	short Save(LPCTSTR lpszFilename, short sPane);
	short SaveZap(LPCTSTR lpszFilename, short sPane);
	short ShowCallouts(BOOL bShow, short sPane);
	void ZoomFullHeight(short sPane);
	void ZoomFullWidth(short sPane);
	short FirstPage(short sPane);
	short LastPage(short sPane);
	short GetColor();
	void SetColor(short sColor);
	void SetData(short sPane, long lData);
	long GetData(short sPane);
	short GetSelectCount(short sPane);
	short DeleteSelections(short sPane);
	short DeleteLastAnn(short sPane);
	short LoadZap(LPCTSTR lpszFilename, BOOL bUseView, BOOL bScaleView, BOOL bCallouts, short sPane, LPCTSTR lpszSourceFile);
	short Copy(short sPane);
	short Paste(short sPane);
	long GetRGBColor(short sColor);
	short ViewImageProperties(short sPane);
	BOOL SetPrinterByName(LPCTSTR lpName);
	CString GetDefaultPrinter();
	BOOL CanPrint();
	CString GetCurrentPrinter();
	short GetImageProperties(long lpProperties, short sPane);
	short Deskew(short sPane);
	CString GetRegisteredPath();
	CString GetClassIdString();
	short Despeckle(short sPane);
	short GetLeadToolError(short sPane);
	short DotRemove(short sPane, long lMinWidth, long lMinHeight, long lMaxWidth, long lMaxHeight);
	short HolePunchRemove(short sPane, long lMinWidth, long lMinHeight, long lMaxWidth, long lMaxHeight, long lLocation);
	short Smooth(short sPane, long lLength, short sFavorLong);
	short BorderRemove(short sPane, long lBorderPercent, long lWhiteNoise, long lVariance, long lLocation);
	short Cleanup(short sPane, LPCTSTR lpszSaveAs);
	short SetupPrintPage();
	short PrintEx(long hDc, short bFullImage, short bAutoRotate, short sLeft, short sTop, short sWidth, short sHeight, short sPane);
	short RescaleZapCallouts();
	short ShowDiagnostics(BOOL bShow);
	long DrawSourceRectangle(short sLeft, short sTop, short sRight, short sBottom, unsigned long lColor, short sTransparency, BOOL bLocked, short sPane);
	short DeleteAnn(long lAnnotation, short sPane);
	long DrawText(LPCTSTR lpszText, short sLeft, short sTop, short sRight, short sBottom, unsigned long lColor, LPCTSTR lpszFont, short sSize, BOOL bLocked, short sPane);
	long DrawSourceText(LPCTSTR lpszText, short sLeft, short sTop, short sRight, short sBottom, unsigned long lColor, LPCTSTR lpszFont, short sSize, BOOL bLocked, short sPane);
	short DrawTmaxRedaction(long pRedaction, short sPane);
	short DrawTmaxRedactions(long paRedactions, short sPane);
	short DeleteTmaxRedaction(long pRedaction, short sPane);
	short DeleteTmaxRedactions(long paRedactions, short sPane);
	long DrawRectangle(short sLeft, short sTop, short sRight, short sBottom, unsigned long lColor, short sTransparency, BOOL bLocked, short sPane);
	short LockAnn(long lAnnotation, short sPane);
	short UnlockAnn(long lAnnotation, short sPane);
	unsigned long GetOleColor(short sTmviewColor);
	short GetCalloutCount(short sPane);
	BOOL SetPrinterProperties(long hWnd);
	short SetProperties(LPCTSTR lpszFilename, LPCTSTR lpszSection);
	short SaveProperties(LPCTSTR lpszFilename, LPCTSTR lpszSection);
	short HolePunchRemove2(short sPane, long lMinHolePunches, long lMaxHolePunches, long lLocation);
	short SavePages(LPCTSTR lpszFilename, LPCTSTR lpszFolder, LPCTSTR lpszPrefix);
	short ShowPrinterCaps();
	void EnableDIBPrinting(short bEnable);
	short SwapPanes();
	short SaveSplitZap(LPCTSTR lpszTLFilename, LPCTSTR lpszBRFilename);
	void AboutBox();
	void DoGesturePan(long lX, long lY);
	void DoGestureZoom(float zoomFactor);
	void SetZoomedNextPage(bool bZoomed);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMVIEW_H__A5385279_9610_4B4D_8BB3_03897ACF682E__INCLUDED_)
