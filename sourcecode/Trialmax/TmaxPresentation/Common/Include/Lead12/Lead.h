#if !defined(AFX_LEAD_H__31A112CF_5F5B_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_LEAD_H__31A112CF_5F5B_11D5_8F0A_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


// Dispatch interfaces referenced by this interface
class COleFont;
class CPicture;
class CAnnToolbar;
class CAnnMenu;

/////////////////////////////////////////////////////////////////////////////
// CLead wrapper class

class CLead : public CWnd
{
protected:
	DECLARE_DYNCREATE(CLead)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0x120000, 0xb1ba, 0x11ce, { 0xab, 0xc6, 0xf5, 0xb2, 0xe7, 0x9d, 0x9e, 0x3f } };
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
	BOOL GetAutoSize();
	void SetAutoSize(BOOL);
	long GetReadyState();
	void SetReadyState(long);
	short GetAppearance();
	void SetAppearance(short);
	OLE_HANDLE GetHWnd();
	void SetHWnd(OLE_HANDLE);
	COleFont GetFont();
	void SetFont(LPDISPATCH);
	BOOL GetEnabled();
	void SetEnabled(BOOL);
	short GetBorderStyle();
	void SetBorderStyle(short);
	OLE_COLOR GetBackColor();
	void SetBackColor(OLE_COLOR);
	long GetBitmap();
	void SetBitmap(long);
	BOOL GetAutoRepaint();
	void SetAutoRepaint(BOOL);
	short GetPaintEffect();
	void SetPaintEffect(short);
	BOOL GetBackErase();
	void SetBackErase(BOOL);
	BOOL GetAutoScroll();
	void SetAutoScroll(BOOL);
	short GetPaintDither();
	void SetPaintDither(short);
	BOOL GetEnableProgressEvent();
	void SetEnableProgressEvent(BOOL);
	short GetBitonalScaling();
	void SetBitonalScaling(short);
	BOOL GetPaintWhileLoad();
	void SetPaintWhileLoad(BOOL);
	float GetBitmapWidth();
	void SetBitmapWidth(float);
	float GetBitmapHeight();
	void SetBitmapHeight(float);
	short GetBitmapBits();
	void SetBitmapBits(short);
	short GetVersionLevel();
	void SetVersionLevel(short);
	short GetVersionMajor();
	void SetVersionMajor(short);
	short GetVersionMinor();
	void SetVersionMinor(short);
	CString GetVersionProduct();
	void SetVersionProduct(LPCTSTR);
	CString GetVersionDate();
	void SetVersionDate(LPCTSTR);
	CString GetVersionTime();
	void SetVersionTime(LPCTSTR);
	short GetPaintPalette();
	void SetPaintPalette(short);
	BOOL GetEnableMethodErrors();
	void SetEnableMethodErrors(BOOL);
	float GetDstLeft();
	void SetDstLeft(float);
	float GetDstTop();
	void SetDstTop(float);
	float GetDstWidth();
	void SetDstWidth(float);
	float GetDstHeight();
	void SetDstHeight(float);
	float GetDstClipLeft();
	void SetDstClipLeft(float);
	float GetDstClipTop();
	void SetDstClipTop(float);
	float GetDstClipWidth();
	void SetDstClipWidth(float);
	float GetDstClipHeight();
	void SetDstClipHeight(float);
	float GetSrcLeft();
	void SetSrcLeft(float);
	float GetSrcTop();
	void SetSrcTop(float);
	float GetSrcWidth();
	void SetSrcWidth(float);
	float GetSrcHeight();
	void SetSrcHeight(float);
	float GetSrcClipLeft();
	void SetSrcClipLeft(float);
	float GetSrcClipTop();
	void SetSrcClipTop(float);
	float GetSrcClipWidth();
	void SetSrcClipWidth(float);
	float GetSrcClipHeight();
	void SetSrcClipHeight(float);
	short GetBitmapXRes();
	void SetBitmapXRes(short);
	short GetBitmapYRes();
	void SetBitmapYRes(short);
	long GetInfoPage();
	void SetInfoPage(long);
	float GetInfoWidth();
	void SetInfoWidth(float);
	float GetInfoHeight();
	void SetInfoHeight(float);
	short GetInfoBits();
	void SetInfoBits(short);
	short GetInfoFormat();
	void SetInfoFormat(short);
	long GetInfoSizeDisk();
	void SetInfoSizeDisk(long);
	long GetInfoSizeMem();
	void SetInfoSizeMem(long);
	CString GetInfoCompress();
	void SetInfoCompress(LPCTSTR);
	BOOL GetEnableTwainEvent();
	void SetEnableTwainEvent(BOOL);
	BOOL GetRubberBandVisible();
	void SetRubberBandVisible(BOOL);
	float GetRubberBandLeft();
	void SetRubberBandLeft(float);
	float GetRubberBandTop();
	void SetRubberBandTop(float);
	float GetRubberBandWidth();
	void SetRubberBandWidth(float);
	float GetRubberBandHeight();
	void SetRubberBandHeight(float);
	short GetPCDRes();
	void SetPCDRes(short);
	short GetMousePointer();
	void SetMousePointer(short);
	CPicture GetMouseIcon();
	void SetMouseIcon(LPDISPATCH);
	short GetScaleMode();
	void SetScaleMode(short);
	BOOL GetAutoSetRects();
	void SetAutoSetRects(BOOL);
	float GetScaleLeft();
	void SetScaleLeft(float);
	float GetScaleTop();
	void SetScaleTop(float);
	float GetScaleWidth();
	void SetScaleWidth(float);
	float GetScaleHeight();
	void SetScaleHeight(float);
	BOOL GetDbCanAppend();
	void SetDbCanAppend(BOOL);
	BOOL GetDbCanRestart();
	void SetDbCanRestart(BOOL);
	BOOL GetDbCanScroll();
	void SetDbCanScroll(BOOL);
	BOOL GetDbCanUpdate();
	void SetDbCanUpdate(BOOL);
	long GetDbRecordCount();
	void SetDbRecordCount(long);
	BOOL GetDbIsOpen();
	void SetDbIsOpen(BOOL);
	BOOL GetDbIsBOF();
	void SetDbIsBOF(BOOL);
	BOOL GetDbIsEOF();
	void SetDbIsEOF(BOOL);
	BOOL GetDbIsDeleted();
	void SetDbIsDeleted(BOOL);
	long GetDbCurrentRecord();
	void SetDbCurrentRecord(long);
	short GetDbEditMode();
	void SetDbEditMode(short);
	short GetDbLockingMode();
	void SetDbLockingMode(short);
	short GetDbLoadBits();
	void SetDbLoadBits(short);
	BOOL GetForePalette();
	void SetForePalette(BOOL);
	CString GetTwainAppAuthor();
	void SetTwainAppAuthor(LPCTSTR);
	CString GetTwainAppFamily();
	void SetTwainAppFamily(LPCTSTR);
	CString GetTwainAppName();
	void SetTwainAppName(LPCTSTR);
	CString GetTwainSourceName();
	void SetTwainSourceName(LPCTSTR);
	long GetTwainMaxPages();
	void SetTwainMaxPages(long);
	short GetTwainPixelType();
	void SetTwainPixelType(short);
	short GetTwainRes();
	void SetTwainRes(short);
	long GetTwainFrameLeft();
	void SetTwainFrameLeft(long);
	long GetTwainFrameTop();
	void SetTwainFrameTop(long);
	long GetTwainFrameWidth();
	void SetTwainFrameWidth(long);
	long GetTwainFrameHeight();
	void SetTwainFrameHeight(long);
	short GetTwainBits();
	void SetTwainBits(short);
	short GetTwainSourceCount();
	void SetTwainSourceCount(short);
	short GetTwainFlags();
	void SetTwainFlags(short);
	LPUNKNOWN GetDataSource();
	void SetDataSource(LPUNKNOWN);
	CString GetDataField();
	void SetDataField(LPCTSTR);
	short GetDataSaveBits();
	void SetDataSaveBits(short);
	short GetDataSaveFormat();
	void SetDataSaveFormat(short);
	short GetDataSaveQuality();
	void SetDataSaveQuality(short);
	short GetDataLoadBits();
	void SetDataLoadBits(short);
	BOOL GetDataChanged();
	void SetDataChanged(BOOL);
	long GetPaintROP3();
	void SetPaintROP3(long);
	short GetIsGrayscale();
	void SetIsGrayscale(short);
	BOOL GetHasRgn();
	void SetHasRgn(BOOL);
	float GetRgnLeft();
	void SetRgnLeft(float);
	float GetRgnTop();
	void SetRgnTop(float);
	float GetRgnWidth();
	void SetRgnWidth(float);
	float GetRgnHeight();
	void SetRgnHeight(float);
	short GetRgnMarkingMode();
	void SetRgnMarkingMode(short);
	short GetRgnFrameType();
	void SetRgnFrameType(short);
	long GetFloater();
	void SetFloater(long);
	BOOL GetFloaterVisible();
	void SetFloaterVisible(BOOL);
	float GetFloaterDstLeft();
	void SetFloaterDstLeft(float);
	float GetFloaterDstTop();
	void SetFloaterDstTop(float);
	float GetFloaterDstWidth();
	void SetFloaterDstWidth(float);
	float GetFloaterDstHeight();
	void SetFloaterDstHeight(float);
	float GetFloaterDstClipLeft();
	void SetFloaterDstClipLeft(float);
	float GetFloaterDstClipTop();
	void SetFloaterDstClipTop(float);
	float GetFloaterDstClipWidth();
	void SetFloaterDstClipWidth(float);
	float GetFloaterDstClipHeight();
	void SetFloaterDstClipHeight(float);
	float GetFloaterWidth();
	void SetFloaterWidth(float);
	float GetFloaterHeight();
	void SetFloaterHeight(float);
	short GetPolygonSize();
	void SetPolygonSize(short);
	short GetAnnUserMode();
	void SetAnnUserMode(short);
	short GetAnnTool();
	void SetAnnTool(short);
	BOOL GetAnnPasteReady();
	void SetAnnPasteReady(BOOL);
	long GetAnnContainer();
	void SetAnnContainer(long);
	short GetLoadInfoFormat();
	void SetLoadInfoFormat(short);
	float GetLoadInfoWidth();
	void SetLoadInfoWidth(float);
	float GetLoadInfoHeight();
	void SetLoadInfoHeight(float);
	short GetLoadInfoBits();
	void SetLoadInfoBits(short);
	short GetLoadInfoXRes();
	void SetLoadInfoXRes(short);
	short GetLoadInfoYRes();
	void SetLoadInfoYRes(short);
	long GetLoadInfoOffset();
	void SetLoadInfoOffset(long);
	long GetLoadInfoFlags();
	void SetLoadInfoFlags(long);
	long GetAnnAutomation();
	void SetAnnAutomation(long);
	short GetProgressivePasses();
	void SetProgressivePasses(short);
	short GetBitmapListIndex();
	void SetBitmapListIndex(short);
	short GetBitmapListCount();
	void SetBitmapListCount(short);
	BOOL GetAnimationEnable();
	void SetAnimationEnable(BOOL);
	float GetBitmapLeft();
	void SetBitmapLeft(float);
	float GetBitmapTop();
	void SetBitmapTop(float);
	long GetBitmapDelay();
	void SetBitmapDelay(long);
	float GetAnimationWidth();
	void SetAnimationWidth(float);
	float GetAnimationHeight();
	void SetAnimationHeight(float);
	unsigned long GetAnimationBackColor();
	void SetAnimationBackColor(unsigned long);
	short GetAnimationBitsPerPixel();
	void SetAnimationBitsPerPixel(short);
	BOOL GetBitmapEnableTransparency();
	void SetBitmapEnableTransparency(BOOL);
	unsigned long GetBitmapTransparentColor();
	void SetBitmapTransparentColor(unsigned long);
	short GetBitmapDisposalMethod();
	void SetBitmapDisposalMethod(short);
	long GetBitmapList();
	void SetBitmapList(long);
	BOOL GetAnnAutoMenuEnable();
	void SetAnnAutoMenuEnable(BOOL);
	BOOL GetAutoAnimate();
	void SetAutoAnimate(BOOL);
	BOOL GetAnimationLoop();
	void SetAnimationLoop(BOOL);
	float GetInfoAnimationWidth();
	void SetInfoAnimationWidth(float);
	float GetInfoAnimationHeight();
	void SetInfoAnimationHeight(float);
	BOOL GetInfoAnimation();
	void SetInfoAnimation(BOOL);
	CString GetBitmapDataPath();
	void SetBitmapDataPath(LPCTSTR);
	short GetPaintSizeMode();
	void SetPaintSizeMode(short);
	float GetPaintZoomFactor();
	void SetPaintZoomFactor(float);
	short GetAutoAnimationLoop();
	void SetAutoAnimationLoop(short);
	BOOL GetAnnAutoDrawEnable();
	void SetAnnAutoDrawEnable(BOOL);
	short GetLoadResolutionCount();
	void SetLoadResolutionCount(short);
	BOOL GetOLEDropAllowed();
	void SetOLEDropAllowed(BOOL);
	BOOL GetAutoRubberBand();
	void SetAutoRubberBand(BOOL);
	BOOL GetSaveInterlaced();
	void SetSaveInterlaced(BOOL);
	BOOL GetPaintSizeUseDPI();
	void SetPaintSizeUseDPI(BOOL);
	CString GetAnnDataPath();
	void SetAnnDataPath(LPCTSTR);
	BOOL GetDrawPersistence();
	void SetDrawPersistence(BOOL);
	short GetDrawMode();
	void SetDrawMode(short);
	unsigned long GetDrawPenColor();
	void SetDrawPenColor(unsigned long);
	short GetDrawPenWidth();
	void SetDrawPenWidth(short);
	short GetDrawPenStyle();
	void SetDrawPenStyle(short);
	unsigned long GetDrawFillColor();
	void SetDrawFillColor(unsigned long);
	short GetDrawFillStyle();
	void SetDrawFillStyle(short);
	unsigned long GetDrawFontColor();
	void SetDrawFontColor(unsigned long);
	short GetPaintContrast();
	void SetPaintContrast(short);
	short GetPaintGamma();
	void SetPaintGamma(short);
	short GetPaintIntensity();
	void SetPaintIntensity(short);
	BOOL GetLoadUseViewPerspective();
	void SetLoadUseViewPerspective(BOOL);
	BOOL GetAnnAutoRotate();
	void SetAnnAutoRotate(BOOL);
	short GetTransitionStyle();
	void SetTransitionStyle(short);
	short GetTransitionEffect();
	void SetTransitionEffect(short);
	short GetGradientStyle();
	void SetGradientStyle(short);
	unsigned long GetGradientStartColor();
	void SetGradientStartColor(unsigned long);
	unsigned long GetGradientEndColor();
	void SetGradientEndColor(unsigned long);
	short GetGradientSteps();
	void SetGradientSteps(short);
	short GetPatternStyle();
	void SetPatternStyle(short);
	unsigned long GetPatternForeColor();
	void SetPatternForeColor(unsigned long);
	unsigned long GetPatternBackColor();
	void SetPatternBackColor(unsigned long);
	short GetPaintPass();
	void SetPaintPass(short);
	short GetPaintMaxPasses();
	void SetPaintMaxPasses(short);
	short GetEffectGrain();
	void SetEffectGrain(short);
	short GetEffectDelay();
	void SetEffectDelay(short);
	short GetWandSize();
	void SetWandSize(short);
	unsigned long GetWandColor();
	void SetWandColor(unsigned long);
	BOOL GetPaintRgnOnly();
	void SetPaintRgnOnly(BOOL);
	float GetShapeLeft();
	void SetShapeLeft(float);
	float GetShapeTop();
	void SetShapeTop(float);
	float GetShapeWidth();
	void SetShapeWidth(float);
	float GetShapeHeight();
	void SetShapeHeight(float);
	float GetShapeBorderThickness();
	void SetShapeBorderThickness(float);
	unsigned long GetShapeBorderColor();
	void SetShapeBorderColor(unsigned long);
	short GetShapeInnerBandStyle();
	void SetShapeInnerBandStyle(short);
	float GetShapeInnerBandThickness();
	void SetShapeInnerBandThickness(float);
	unsigned long GetShapeInnerBandHiliteColor();
	void SetShapeInnerBandHiliteColor(unsigned long);
	unsigned long GetShapeInnerBandShadowColor();
	void SetShapeInnerBandShadowColor(unsigned long);
	short GetShapeOuterBandStyle();
	void SetShapeOuterBandStyle(short);
	float GetShapeOuterBandThickness();
	void SetShapeOuterBandThickness(float);
	unsigned long GetShapeOuterBandHiliteColor();
	void SetShapeOuterBandHiliteColor(unsigned long);
	unsigned long GetShapeOuterBandShadowColor();
	void SetShapeOuterBandShadowColor(unsigned long);
	short GetShapeBackgroundStyle();
	void SetShapeBackgroundStyle(short);
	float GetBackgroundImageLeft();
	void SetBackgroundImageLeft(float);
	float GetBackgroundImageTop();
	void SetBackgroundImageTop(float);
	float GetBackgroundImageWidth();
	void SetBackgroundImageWidth(float);
	float GetBackgroundImageHeight();
	void SetBackgroundImageHeight(float);
	float GetShadowXDepth();
	void SetShadowXDepth(float);
	float GetShadowYDepth();
	void SetShadowYDepth(float);
	unsigned long GetShadowColor();
	void SetShadowColor(unsigned long);
	unsigned long GetTextHiliteColor();
	void SetTextHiliteColor(unsigned long);
	float GetTextLeft();
	void SetTextLeft(float);
	float GetTextTop();
	void SetTextTop(float);
	float GetTextWidth();
	void SetTextWidth(float);
	float GetTextHeight();
	void SetTextHeight(float);
	short GetTextAngle();
	void SetTextAngle(short);
	short GetTextStyle();
	void SetTextStyle(short);
	short GetShapeBorderStyle();
	void SetShapeBorderStyle(short);
	float GetFrameThickness();
	void SetFrameThickness(float);
	unsigned long GetFrameColor();
	void SetFrameColor(unsigned long);
	short GetFrameInnerStyle();
	void SetFrameInnerStyle(short);
	float GetFrameInnerThickness();
	void SetFrameInnerThickness(float);
	unsigned long GetFrameInnerHiliteColor();
	void SetFrameInnerHiliteColor(unsigned long);
	unsigned long GetFrameInnerShadowColor();
	void SetFrameInnerShadowColor(unsigned long);
	short GetFrameOuterStyle();
	void SetFrameOuterStyle(short);
	float GetFrameOuterThickness();
	void SetFrameOuterThickness(float);
	unsigned long GetFrameOuterHiliteColor();
	void SetFrameOuterHiliteColor(unsigned long);
	unsigned long GetFrameOuterShadowColor();
	void SetFrameOuterShadowColor(unsigned long);
	short GetTextAlign();
	void SetTextAlign(short);
	BOOL GetTextWordWrap();
	void SetTextWordWrap(BOOL);
	long GetBitmapAlpha();
	void SetBitmapAlpha(long);
	short GetPaintNotificationDelay();
	void SetPaintNotificationDelay(short);
	BOOL GetTransparent();
	void SetTransparent(BOOL);
	short GetLevelLowBit();
	void SetLevelLowBit(short);
	short GetLevelHighBit();
	void SetLevelHighBit(short);
	long GetLevelLUTLength();
	void SetLevelLUTLength(long);
	long GetMinVal();
	void SetMinVal(long);
	long GetMaxVal();
	void SetMaxVal(long);
	short GetMinBit();
	void SetMinBit(short);
	short GetMaxBit();
	void SetMaxBit(short);
	BOOL GetAnnShowLockedIcon();
	void SetAnnShowLockedIcon(BOOL);
	BOOL GetAnnHyperlinkMenuEnable();
	void SetAnnHyperlinkMenuEnable(BOOL);
	BOOL GetEnableZoomInRect();
	void SetEnableZoomInRect(BOOL);
	short GetTwainIntensity();
	void SetTwainIntensity(short);
	short GetTwainContrast();
	void SetTwainContrast(short);
	short GetEnableTwainFeeder();
	void SetEnableTwainFeeder(short);
	short GetEnableTwainAutoFeed();
	void SetEnableTwainAutoFeed(short);
	float GetPanWinX();
	void SetPanWinX(float);
	float GetPanWinY();
	void SetPanWinY(float);
	float GetPanWinWidth();
	void SetPanWinWidth(float);
	float GetPanWinHeight();
	void SetPanWinHeight(float);
	unsigned long GetPanWinRectColor();
	void SetPanWinRectColor(unsigned long);
	CString GetPanWinTitle();
	void SetPanWinTitle(LPCTSTR);
	BOOL GetPanWinSysMenu();
	void SetPanWinSysMenu(BOOL);
	short GetPanWinPaintPalette();
	void SetPanWinPaintPalette(short);
	short GetPanWinBitonalScaling();
	void SetPanWinBitonalScaling(short);
	short GetPanWinPaintDither();
	void SetPanWinPaintDither(short);
	CPicture GetPanWinIcon();
	void SetPanWinIcon(LPDISPATCH);
	short GetPanWinPointer();
	void SetPanWinPointer(short);
	CPicture GetPanWinCursor();
	void SetPanWinCursor(LPDISPATCH);
	long GetInfoTotalPages();
	void SetInfoTotalPages(long);
	long GetSavePage();
	void SetSavePage(long);
	long GetAnimationBitmap();
	void SetAnimationBitmap(long);
	BOOL GetOLEDropAutoLoad();
	void SetOLEDropAutoLoad(BOOL);
	long GetWMFResolution();
	void SetWMFResolution(long);
	long GetInfoXRes();
	void SetInfoXRes(long);
	long GetInfoYRes();
	void SetInfoYRes(long);
	BOOL GetRefBitmap();
	void SetRefBitmap(BOOL);
	BOOL GetWindowLevelBitmap();
	void SetWindowLevelBitmap(BOOL);
	short GetSaveResolutionCount();
	void SetSaveResolutionCount(short);
	BOOL GetAutoPan();
	void SetAutoPan(BOOL);
	float GetPrinterScaleWidth();
	void SetPrinterScaleWidth(float);
	float GetPrinterScaleHeight();
	void SetPrinterScaleHeight(float);
	CAnnToolbar GetAnnToolbar();
	void SetAnnToolbar(LPDISPATCH);
	BOOL GetSaveLSB();
	void SetSaveLSB(BOOL);
	BOOL GetSavePageNumberTag();
	void SetSavePageNumberTag(BOOL);
	short GetTwainDuplex();
	void SetTwainDuplex(short);
	short GetEnableTwainDuplex();
	void SetEnableTwainDuplex(short);
	BOOL GetLoadCompressed();
	void SetLoadCompressed(BOOL);
	short GetPaintScaling();
	void SetPaintScaling(short);
	short GetPanWinPaintScaling();
	void SetPanWinPaintScaling(short);
	BOOL GetEnablePaint();
	void SetEnablePaint(BOOL);
	BOOL GetRefBitmapList();
	void SetRefBitmapList(BOOL);
	short GetMagGlassMaskCount();
	void SetMagGlassMaskCount(short);
	short GetPreferredLoadFormat();
	void SetPreferredLoadFormat(short);
	long GetDocCleanSuccess();
	void SetDocCleanSuccess(long);
	long GetHDocCleanRgn();
	void SetHDocCleanRgn(long);
	long GetDocCleanBitmap();
	void SetDocCleanBitmap(long);
	unsigned long GetRgnFrameColor();
	void SetRgnFrameColor(unsigned long);
	long GetInfoIFD();
	void SetInfoIFD(long);
	long GetLoadIFD();
	void SetLoadIFD(long);
	long GetSaveIFD();
	void SetSaveIFD(long);
	BOOL GetAnnUndoEnable();
	void SetAnnUndoEnable(BOOL);
	short GetAnnUndoDepth();
	void SetAnnUndoDepth(short);
	short GetAnnAutoDialogFontSize();
	void SetAnnAutoDialogFontSize(short);
	short GetMagGlassFlags();
	void SetMagGlassFlags(short);
	BOOL GetEnableMagGlassEvent();
	void SetEnableMagGlassEvent(BOOL);
	short GetInfoViewPerspective();
	void SetInfoViewPerspective(short);
	BOOL GetEnableTwainFileEvent();
	void SetEnableTwainFileEvent(BOOL);
	BOOL GetEnableTwainFileFormatEvent();
	void SetEnableTwainFileFormatEvent(BOOL);
	long GetTwainFileFormatStatus();
	void SetTwainFileFormatStatus(long);
	BOOL GetAnnAutoResize();
	void SetAnnAutoResize(BOOL);
	float GetConvertX();
	void SetConvertX(float);
	float GetConvertY();
	void SetConvertY(float);
	long GetPDFXResolution();
	void SetPDFXResolution(long);
	long GetPDFYResolution();
	void SetPDFYResolution(long);
	long GetPDFDisplayDepth();
	void SetPDFDisplayDepth(long);
	long GetPDFTextAlpha();
	void SetPDFTextAlpha(long);
	long GetPDFGraphicsAlpha();
	void SetPDFGraphicsAlpha(long);
	BOOL GetPDFUseLibFonts();
	void SetPDFUseLibFonts(BOOL);
	short GetRasterizeViewportWidth();
	void SetRasterizeViewportWidth(short);
	short GetRasterizeViewportHeight();
	void SetRasterizeViewportHeight(short);
	short GetRasterizeViewMode();
	void SetRasterizeViewMode(short);
	long GetHistogramGrayTableSize();
	void SetHistogramGrayTableSize(long);
	BOOL GetSavePad4();
	void SetSavePad4(BOOL);
	long GetFileSizeWritten();
	void SetFileSizeWritten(long);
	BOOL GetInfoHasAlpha();
	void SetInfoHasAlpha(BOOL);
	CString GetPDFPassword();
	void SetPDFPassword(LPCTSTR);
	BOOL GetEnableKeyboard();
	void SetEnableKeyboard(BOOL);
	BOOL GetEnableScroll();
	void SetEnableScroll(BOOL);

// Operations
public:
	void AboutBox();
	short Average(short iDim);
	short Median(short iDim);
	short Contrast(short iChange);
	short Despeckle();
	short Flip();
	short Grayscale(short iBitsPerPixel);
	short HistoEqualize();
	short Hue(short iAngle);
	short Intensity(short iChange);
	short Invert();
	short TwainAcquire(long hWnd);
	short TwainSelect(long hWnd);
	short Sharpen(short iChange);
	short Saturation(short iChange);
	short Reverse();
	short ForceRepaint();
	short Mosaic(short iDim);
	short SpatialFilter(short iType);
	short BinaryFilter(short iType);
	short Save(LPCTSTR pszName, short iFormat, short iBitsPerPixel, short iQuality, short iModify);
	short Posterize(short iLevels);
	CPicture GetPicture();
	short SetPicture(LPDISPATCH pPicture);
	short Rotate(long iAngle, short iFlags, unsigned long crFill);
	short Emboss(short iDirection, short iDepth);
	short AddNoise(short iRange, short iChannel);
	short IntensityDetect(short iLow, short iHigh);
	short Fill(unsigned long crFill);
	short StretchIntensity();
	short Deskew();
	short GammaCorrect(short iGamma);
	short Shear(short iAngle, BOOL bHorizontal, unsigned long crFill);
	short CreateBitmap(float fWidth, float fHeight, short iBitsPerPixel);
	short Render(long hDC, float fLeft, float fTop, float fWidth, float fHeight);
	long GetBitmapDC();
	short ReleaseBitmapDC();
	short Halftone(short iType, long iAngle);
	short Size(float fWidth, float fHeight, short iFlags);
	short Copy(short iFlags);
	short Paste(short iFlags);
	short Load(LPCTSTR pszName, short iBitsPerPixel, long lPage, long lPages);
	short SetDIB(long hDIB);
	long GetDIB();
	short SetDDB(long hDC, long hBitmap, long hPalette);
	long GetDDB(long hDC);
	short RemapIntensity(short iChannel);
	short GetHistogram(short iChannel);
	short ColorRes(short iBitsPerPixel, short iPalette, short iDither, short iColors);
	short Capture(long hDC, float fLeft, float fTop, float fWidth, float fHeight);
	short ColorSeparate(long iFlags);
	short ColorMerge(long iFlags);
	short Underlay(long hBitmap, short iFlags);
	short Combine(float fLeft, float fTop, float fWidth, float fHeight, long hBitmap, float fSrcLeft, float fSrcTop, long iFlags);
	short SetDstRect(float fLeft, float fTop, float fWidth, float fHeight);
	short SetDstClipRect(float fLeft, float fTop, float fWidth, float fHeight);
	short SetSrcRect(float fLeft, float fTop, float fWidth, float fHeight);
	short SetSrcClipRect(float fLeft, float fTop, float fWidth, float fHeight);
	short Trim(float fLeft, float fTop, float fWidth, float fHeight);
	short HistoContrast(short iChange);
	short GetFileInfo(LPCTSTR pszName, long lPage, long uFlags);
	short SaveWithStamp(LPCTSTR pszName, short iFormat, short iBitsPerPixel, short iQuality, float fStampWidth, float fStampHeight, short iStampBits);
	short LoadStamp(LPCTSTR pszName);
	void SetRubberBandRect(float fLeft, float fTop, float fWidth, float fHeight);
	long GetClientDC();
	short ReleaseClientDC();
	void dbAddNew();
	void dbDelete();
	void dbEdit();
	void dbMove(long lRows);
	void dbMoveFirst();
	void dbMoveLast();
	void dbMoveNext();
	void dbMovePrev();
	void dbClose();
	long GetPalette(long hDC);
	BOOL dbRequery();
	BOOL dbOpen(LPCTSTR pszConnect, LPCTSTR pszSQL, LPCTSTR pszField, long iOptions);
	short dbUpdate(short iFormat, short iBitsPerPixel, short iQuality);
	long PrintStart();
	void PrintEnd(long hDC);
	short TwainRealize(long hWnd);
	short TwainEnumSources(long hWnd);
	void UnlockSupport(short iType, LPCTSTR pszKey);
	BOOL IsSupportLocked(short iType);
	short StartFeedLoad(short iBitsPerPixel, long lPage, long lPages);
	short StopFeedLoad();
	short SaveMemory(long* phMem, short iFormat, short iBitsPerPixel, short iQuality, long* piSize);
	short LoadMemory(long hMem, short iBitsPerPixel, long lPage, long lPages, long iSize);
	short GetMemoryInfo(long hMem, long lPage, long iSize);
	short FeedLoad(const VARIANT& vBuffer, long iSize);
	BOOL DeleteRgnHandle(long hRgn);
	short FreeRgn();
	long GetRgnArea();
	long GetRgnHandle();
	BOOL IsPtInRgn(float fx, float fy);
	short OffsetRgn(float fDx, float fDy);
	short SetRgnColor(unsigned long crMatch, short iCombineMode);
	short SetRgnHandle(long hRgn, float fDx, float fDy, short iCombineMode);
	short SetRgnEllipse(float fLeft, float fTop, float fWidth, float fHeight, short iCombineMode);
	short SetRgnRect(float fLeft, float fTop, float fWidth, float fHeight, short iCombineMode);
	short SetRgnRoundRect(float fLeft, float fTop, float fWidth, float fHeight, float fWidthEllipse, float fHeightEllipse, short iCombineMode);
	BOOL IsPtInFloater(float fx, float fy);
	long GetFloaterHandle();
	short SetFloaterDstRect(float fLeft, float fTop, float fWidth, float fHeight);
	short SetFloaterDstClipRect(float fLeft, float fTop, float fWidth, float fHeight);
	short SetRgnPolygon(short iFillMode, short iCombineMode);
	short RepaintRect(float fLeft, float fTop, float fWidth, float fHeight, BOOL bErase);
	short AnnSetTag(long hObject, long iTag);
	long AnnGetTag(long hObject);
	short AnnGetType(long hObject);
	short AnnSave(LPCTSTR pszFile, short iFormat, BOOL fSelected);
	short AnnLoad(LPCTSTR pszFile, long lPage);
	short AnnCopy(short iFormat, BOOL fSelected, BOOL fEmpty);
	short AnnPaste();
	short AnnRealize(BOOL fRedactOnly);
	short AnnFlip(BOOL bUseHorz, float fHorz, BOOL bSelected);
	short AnnReverse(BOOL bUseVert, float fVert, BOOL bSelected);
	short AnnRotate(BOOL bUseCenter, float fCenterX, float fCenterY, float fAngle, BOOL bSelected);
	short AnnSetAutoText(short iItem, LPCTSTR pszText);
	CString AnnGetAutoText(short iItem);
	short ColorResList(short iBitsPerPixel, short iPalette, short iDither, short iColors);
	long GetColorCount();
	short InsertBitmapListItem(short iIndex, long hBitmap);
	short ReadLoadResolutions(LPCTSTR pszFile);
	short SetLoadResolution(short iFormat, float fWidth, float fHeight);
	short GetLoadResolution(short iFormat);
	short GetServerBitmap(long pServer);
	short SetServerBitmap(long pServer);
	short DeleteBitmapListItems(short iIndex, short iCount);
	short AnnLoadMemory(long hMem, long iSize);
	short AnnSaveMemory(long* phMem, short iFormat, BOOL bSelected, long* piSize);
	short DrawEllipse(float fLeft, float fTop, float fWidth, float fHeight);
	short DrawRectangle(float fLeft, float fTop, float fWidth, float fHeight);
	short DrawLine(float fX1, float fY1, float fX2, float fY2);
	short DrawTextOut(float fx, float fy, LPCTSTR pszText);
	float DrawTextWidth(LPCTSTR pszText);
	float DrawTextHeight(LPCTSTR pszText);
	short ZoomToRect(float fLeft, float fTop, float fWidth, float fHeight);
	short Solarize(short iThreshold);
	short AutoTrim(short iThreshold);
	short MinFilter(short iDim);
	short MaxFilter(short iDim);
	short Oilify(short iDim);
	VARIANT ReadComment(LPCTSTR pszFile, long lPage, short iIndex);
	long AnnGetPointCount(long hObject);
	VARIANT AnnGetPointX(long hObject);
	VARIANT AnnGetPointY(long hObject);
	short AnnGetSelectCount();
	VARIANT AnnGetSelectList();
	short FastRotate(short iAngle);
	short AnnMove(float dxOffset, float dyOffset, BOOL bSelected);
	short DrawShape(short uShape, long BackgroundImage);
	short DrawText(LPCTSTR szText, long ForegroundImage);
	short WindowLevel();
	short GetMinMaxVal();
	short GetMinMaxBits();
	short AnnLock(long hObject, LPCTSTR pszKey, BOOL fSelected);
	short AnnUnlock(long hObject, LPCTSTR pszKey);
	BOOL AnnGetLocked(long hObject);
	short AnnUnrealize(long hObject, BOOL fSelected);
	short AnnSaveTag(BOOL fWangMode, BOOL fSelected);
	short AnnSetHyperlinkString(long hObject, short uType, LPCTSTR pszHyperlink);
	short AnnSetHyperlinkMsg(long hObject, short uType, long lParam1, long lParam2, long lParam3, long lParam4, long lParam5);
	short AnnGetHyperlinkType(long hObject);
	CString AnnGetHyperlinkString(long hObject);
	long AnnGetHyperlinkParam(long hObject, short iParamIndex);
	short PreLoadFilters(short iFixedFilters, short iCachedFilters, LPCTSTR pszFilters);
	short IgnoreFilters(LPCTSTR pszFilters);
	short ZoomIn();
	short TwainCloseSession(long hWnd);
	short ShowPanWin(BOOL bShow);
	short DeletePage(LPCTSTR pszName, long lPage);
	short Picturize(LPCTSTR pszDirectory, short iFlags, float fCellWidth, float fCellHeight);
	short AnnEnumerate(long hObject, short iFlags, LPCTSTR pszUserList);
	short ContourFilter(short nThreshold, short nDeltaDirection, short nMaximumError, short nOption);
	short WindowLevelFillLUT(unsigned long crStart, unsigned long crEnd, long nLow, long nHigh, short nLowBit, short nHighBit, long nMinValue, long nMaxValue, BOOL bInside);
	short AnnDestroy(long hObject, short iFlags);
	long AnnCreate(short iType, BOOL bAutoInsert, BOOL bVisible);
	short AnnInsert(long hContainer, long hObject, BOOL bStripContainer);
	short AnnDefine(long hObject, float x, float y, short iState);
	short AnnSetAutoDefaults(long hObject, short iFlags);
	short SetMousePos(float x, float y, BOOL bUpdatePos);
	short AnnSetUser(long hObject, LPCTSTR pOldUser, LPCTSTR pNewUser, BOOL bSelected);
	short AnnSetVisible(long hObject, BOOL bVisible, short iFlags, LPCTSTR pszUserList);
	BOOL AnnGetVisible(long hObject);
	short AnnSetSelected(long hObject, BOOL bValue, BOOL bSelected);
	short AnnGroup(long hObject, short iFlags, LPCTSTR pszUserList);
	short AnnUngroup(long hObject, short iFlags, LPCTSTR pszUserList);
	long AnnGetObjectFromTag(long hObject, long lTag, BOOL bSelected);
	short AnnSetAutoMenuItemEnable(long hObject, short iType, short iItem, short iEnable, short iFlags, LPCTSTR pszUserList);
	short AnnGetAutoMenuItemEnable(long hObject, short iType, short iItem);
	short LoadArray(const VARIANT& vMem, short iBitsPerPixel, long lPage, long lPages, long lSize);
	VARIANT SaveArray(short iFormat, short iBitsPerPixel, short iQuality);
	short AnnLoadArray(const VARIANT& vMem, long lSize);
	VARIANT AnnSaveArray(short iFormat, BOOL bSelected);
	long ArraySize(const VARIANT& vMem);
	short WriteComment(LPCTSTR pszFile, long lPage);
	short SetTag(long lTag, short iType, long lCount, const VARIANT& vData);
	VARIANT GetTagData(long lTag);
	long GetTagCount(long lTag);
	short GetTagType(long lTag);
	short WriteTag(LPCTSTR pszFile, long lPage);
	short ReadTag(LPCTSTR pszFile, long lPage, long lTag);
	short StartMagGlass(float fWidth, float fHeight, short nZoom, unsigned long clrPen, unsigned long clrBack, BOOL bEllipse, float fBorderSize, BOOL b3D, short nCrosshair, BOOL bIgnoreRgn, BOOL bCenter);
	short StopMagGlass();
	short SetRgnMagicWand(float x, float y, short iTolerance, short iCombineMode);
	short Smooth(long iLength, long uFlags);
	short LineRemove(long uFlags, long iMinLineLength, long iMaxLineWidth, long iWall, long iMaxWallPercent, long iGapLength, long iVariance, long uRemoveFlags);
	short BorderRemove(long uFlags, long uBorderToRemove, long iBorderPercent, long iWhiteNoiseLength, long iVariance);
	short InvertedText(long uFlags, long iMinInvertWidth, long iMinInvertHeight, long iMinBlackPercent, long iMaxBlackPercent);
	short DotRemove(long uFlags, long iMinDotWidth, long iMinDotHeight, long iMaxDotWidth, long iMaxDotHeight);
	short HolePunchRemove(long uFlags, long iMinHoleCount, long iMaxHoleCount, long iMinHoleWidth, long iMinHoleHeight, long iMaxHoleWidth, long iMaxHoleHeight, long iLocation);
	unsigned long GetBitmapPalette(short iIndex);
	void SetBitmapPalette(short iIndex, unsigned long newValue);
	short GetRemapTable(short iIndex);
	void SetRemapTable(short iIndex, short nNewValue);
	long GetHistogramTable(short iIndex);
	void SetHistogramTable(short iIndex, long nNewValue);
	unsigned long GetUserPalette(short iIndex);
	void SetUserPalette(short iIndex, unsigned long newValue);
	long GetColorPlanes(short iIndex);
	void SetColorPlanes(short iIndex, long nNewValue);
	BOOL GetInfoPCDRes(short iIndex);
	CString GetTwainSourceList(short iIndex);
	float GetPolygonX(short iIndex);
	void SetPolygonX(short iIndex, float newValue);
	float GetPolygonY(short iIndex);
	void SetPolygonY(short iIndex, float newValue);
	unsigned long GetAnimationPalette(short iIndex);
	void SetAnimationPalette(short iIndex, unsigned long newValue);
	float GetLoadResolutionWidth(short iIndex);
	float GetLoadResolutionHeight(short iIndex);
	VARIANT GetComment(short iIndex);
	void SetComment(short iIndex, const VARIANT& newValue);
	unsigned long GetPixel(float x, float y);
	void SetPixel(float x, float y, unsigned long newValue);
	unsigned long GetLevelLUT(long iIndex);
	void SetLevelLUT(long iIndex, unsigned long newValue);
	float GetSaveResolutionWidth(short iIndex);
	void SetSaveResolutionWidth(short iIndex, float newValue);
	float GetSaveResolutionHeight(short iIndex);
	void SetSaveResolutionHeight(short iIndex, float newValue);
	float GetAnnRectLeft(long hObject);
	void SetAnnRectLeft(long hObject, float newValue);
	float GetAnnRectTop(long hObject);
	void SetAnnRectTop(long hObject, float newValue);
	float GetAnnRectWidth(long hObject);
	void SetAnnRectWidth(long hObject, float newValue);
	float GetAnnRectHeight(long hObject);
	void SetAnnRectHeight(long hObject, float newValue);
	unsigned long GetMagGlassMask(short iIndex);
	void SetMagGlassMask(short iIndex, unsigned long newValue);
	short AnnSetGrouping(long hObject, BOOL bAutoGroup, short iFlags);
	BOOL AnnGetGrouping(long hObject);
	short AnnAddUndoNode();
	short AnnUndo();
	short UpdateMagGlass(const VARIANT& vColorBuffer, long lColorSize, long hMaskPlane, short iMaskPlaneStart, short iMaskPlaneEnd, BOOL bUpdateBitmap);
	void PrintNewPage(long hDC);
	BOOL IsTwainAvailable(long hWnd);
	short TwainAcquireFile(long hWnd, LPCTSTR pszName, long lFormat);
	short ClientToBitmap(float x, float y);
	short BitmapToClient(float x, float y);
	short Deskew2(unsigned long crBack);
	short RemapHue(BOOL bUseMaskTable, BOOL bUseHTable, BOOL bUseSTable, BOOL bUseVTable);
	short GetMaskTable(short iIndex);
	void SetMaskTable(short iIndex, short nNewValue);
	short GetHTable(short iIndex);
	void SetHTable(short iIndex, short nNewValue);
	short GetSTable(short iIndex);
	void SetSTable(short iIndex, short nNewValue);
	short GetVTable(short iIndex);
	void SetVTable(short iIndex, short nNewValue);
	short HSV_HfromRGB(unsigned long cr);
	short HSV_SfromRGB(unsigned long cr);
	short HSV_VfromRGB(unsigned long cr);
	unsigned long RGBfromHSV(short nH, short nS, short nV);
	short SetRgnColorRGBRange(unsigned long crLower, unsigned long crUpper, short iCombineMode);
	short SetRgnColorHSVRange(short nH0, short nS0, short nV0, short nH1, short nS1, short nV1, short iCombineMode);
	short GetHistogramGray();
	long GetHistogramGrayTable(long nIndex);
	void SetHistogramGrayTable(long nIndex, long nNewValue);
	long GetDIB2(long nType);
	short UpdateMagGlassShape(short nMagGlassShape, long hMagGlassRgn);
	short SaveOffset(LPCTSTR pszName, long iOffsetBegin, short iFormat, short iBitsPerPixel, short iQuality, short iModify);
	short UpdateMagGlassExt(const VARIANT& vColorBuffer, const VARIANT& vMaskPlane, long nMaskPlaneStart, long nMaskPlaneEnd, BOOL bUpdateBitmap);
	long AnnAccessObject(const VARIANT& aObjects, short iIndex);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_LEAD_H__31A112CF_5F5B_11D5_8F0A_00802966F8C1__INCLUDED_)
