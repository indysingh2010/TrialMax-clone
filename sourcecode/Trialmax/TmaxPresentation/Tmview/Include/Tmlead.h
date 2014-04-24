//==============================================================================
//
// File Name:	tmlead.h
//
// Description:	This file contains the declaration of the CTMLead class.
//				This class is derived for the CLead class and adds methods
//				specifically for using the LeadTools control within the
//				TMView ocx.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-11-97	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMLEAD_H__808F18DA_1C7D_11D1_B033_008029EFD140__INCLUDED_)
#define AFX_TMLEAD_H__808F18DA_1C7D_11D1_B033_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmvdefs.h>
#include <lead.h>
#include <handler.h>
#include <tmini.h>
#include <ltann.h>
#include <annotate.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMVERRORS_TITLE			"TMView Error"
#define IDC_TMLEAD				200
#define PAN_BORDER				10
#define MINIMUM_CALLOUTRECT		10
#define MINIMUM_ZOOMRECT		10
#define CALLOUT_MARGIN			5	//	Percentage of available window
#define CALLOUT_MINGRABBORDER	10	
#define TMLEAD_LOCK_KEY			"key"

//	Identifiers for appending information to zap annotation files
#define TMLEAD_SECTION			"TMLEAD"
#define LEFTPANE_SECTION		"LEFT PANE"
#define RIGHTPANE_SECTION		"RIGHT PANE"

#define KEYCHECK_LINE			"KeyCheck"
#define DSTTOP_LINE				"DstTop"
#define DSTLEFT_LINE			"DstLeft"
#define DSTHEIGHT_LINE			"DstHeight"
#define DSTWIDTH_LINE			"DstWidth"
#define FILENAME_LINE			"Filename"
#define VIEWWIDTH_LINE			"ViewWidth"
#define VIEWHEIGHT_LINE			"ViewHeight"
#define ROTATION_LINE			"Rotate"
#define CALLOUT_LINE			"Callout"

//	Indices into cursor array
#define PAN_CURSOR				0
#define CALLOUT_CURSOR			1
#define ZOOM_CURSOR				2
#define MAX_CURSORS				3

//	This key is used to validate the information appended to the original
//	format of the zap file
#define ZAP_ORIGINAL_VALIDATE_KEY		41298L

//	This version identifier is used to determine if the zap file is not of
//	the original format.
//
//	NOTE:	The original version did not allow us to test for a specific 
//			version because no identifier was included in the file. We use
//			this number to distinquish between "Old" (original) formatted
//			zap files and those of later versions
#define ZAP_NEW_VERSION			103098L

//	Packed flags stored in zap headers
#define ZAP_HF_WND_COORDINATES	0x0001	//	Window coordinates are valid

//	Maximum length to string buffers in zap file structures
#define ZAP_MAXLEN_SPLIT_SCREEN_ID	64
#define ZAP_MAXLEN_UNUSED_STRING	512

#define LSTRCPYN(a,b)			lstrcpyn(a, b, sizeof(a))

//	These definitions were extracted from LeadTools files
#define SUCCESS_ABORT				2   /** Function successful. You can quit now. **/
#define SUCCESS						1   /** Function successful        **/
#define FAILURE						0   /** Function not successful    **/

#define COPY_EMPTY					0x0001
#define COPY_DIB					0x0002
#define COPY_DDB					0x0004
#define COPY_PALETTE				0x0008
#define COPY_RGN					0x0010      

#define PASTE_ISREADY				0x4000

#define CRP_FIXEDPALETTE			0x0001
#define CRP_OPTIMIZEDPALETTE		0x0002
#define CRP_BYTEORDERBGR			0x0004
#define CRP_BYTEORDERRGB			0x0000
#define CRP_IDENTITYPALETTE			0x0008
#define CRP_USERPALETTE				0x0010
#define CRP_NETSCAPEPALETTE			0x0040
#define CRP_BYTEORDERGRAY			0x0080

#define CRD_NODITHERING				0x0000
#define CRD_FLOYDSTEINDITHERING		0x0001
#define CRD_STUCKIDITHERING			0x0002
#define CRD_BURKESDITHERING			0x0003
#define CRD_SIERRADITHERING			0x0004
#define CRD_STEVENSONARCEDITHERING	0x0005
#define CRD_JARVISDITHERING			0x0006
#define CRD_ORDEREDDITHERING		0x0007
#define CRD_CLUSTEREDDITHERING		0x0008

#ifndef SAVE_OVERWRITE
	#define SAVE_OVERWRITE			0
	#define SAVE_APPEND				1
	#define SAVE_REPLACE			2
	#define SAVE_INSERT				3
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CTMViewCtrl;
class CCallout;
class CCallouts;

//	This structure is used to maintain annotations for each page 
//	stored in memory
typedef struct
{
	HGLOBAL	hMemory;
	ULONG	ulSize;
}SAnnMemory;

//	This structure is used to pass parameters to the callback that's invoked
//	to retrieve the handle for an annotation
typedef struct
{
	DWORD		dwTag;
	HANNOBJECT	hAnn;
}SGetAnnHandle;

//	These structure is the header for all zap (treatment) files
typedef struct
{
	long	lVersion;		//	Zap version identifier
	int		sScreenWidth;	//	Width of the screen
	int		sScreenHeight;	//	Height of the screen
	RECT	rcWindow;		//	Screen position of TMView control
	BOOL	bSplitScreen;	//	Flag to indicate if split-screen zap
	DWORD	dwCtrlVersion;	//				.
	DWORD	dwUnused1;		//	Not currently in use
	WORD	wFooterSize;	//	Size in bytes of the footer structure
	WORD	wFlags;			//	Packed bit flags
}SZapHeader;

//	These structure is the footer for all zap (treatment) files
typedef struct
{
	DWORD	dwUnused1;		//				.
	DWORD	dwUnused2;		//				.
	WORD	wUnused1;		//				.
	WORD	wUnused2;		//				.
}SZapFooter;

//	This structure stores the information for an individual pane in the
//	zap file. A pane is a single instance of a LeadTools control. TMView
//	uses two panes but at this time split screen zaps are not supported.
typedef struct
{
	float	fDstTop;				//	Top coordinate of visible part of image
	float	fDstLeft;				//	Left coordinate of visible part of image
	float	fDstHeight;				//	Height of thevisible part of image
	float	fDstWidth;				//	Width of the visible part of the image
	int		iViewHeight;			//	Height of the LeadTool control
	int		iViewWidth;				//	Width of the LeadTool control
	short	sAngle;					//	Rotation angle of the image
	short	sCallouts;				//	Number of callouts associated with the pane
	long	lAnnBytes;				//	Number of bytes used to store annotations
	char	szFilespec[_MAX_PATH];	//	Full path of image being displayed
	DWORD	dwTLMax;				//	Packed coordinates for top/left of max rectangle
	DWORD	dwBRMax;				//	Packed coordinates for bottom/right of max rectangle
	WORD	wPaneLeft;				//	Left coordinate for pane window
	WORD	wPaneTop;				//	Top coordinate for pane window
}SZapPane;
	
//	This structure is used to store the information for recreating a callout in
//	the zap file.
typedef struct
{
	RECT	rcRubberBand;			//	Coordinates of original rubber band rectangle
	RECT	rcDstRect;				//	Coordinates of visible portion of the image
	RECT	rcMax;					//	Screen coordinates of bounding rectangle
	RECT	rcPosition;
	short	sAngle;					//	Rotation angle of the image
	WORD	wAnnId;					//	Id of annotation associated with the callout
	long	lAnnBytes;				//	Number of bytes used to store annotations
	DWORD	dwUnused1;				//	Not used in this version
	DWORD	dwUnused2;				//				.
	WORD	wUnused1;				//				.
	WORD	wFlags;					//	Packed flags
}SZapCallout;

typedef struct 
{
	int		iTimerId;
	UINT	uTimer;
	BOOL	bUseView;
	BOOL	bScaleView;
	BOOL	bCallouts;
	BOOL	bZap;
	CString	strFilename;
	CString	strSourceFilename;
}SAsyncParams;

class CTMLead : public CLead
{
	private:

		CTMViewCtrl*	m_pControl;
		CAnnotations	m_Annotations;
		CCallouts*		m_pCallouts;
		CCallout*		m_pCallout;	
		CTMIni			m_Ini;
		CErrorHandler*	m_pErrors;
		SAnnMemory*		m_pAnnMemory;
		HANNOBJECT		m_hEditTextAnn;
		HANNOBJECT		m_hCalloutShade;
		RECT			m_rcMax;
		RECT			m_rcRubberBand;
		RECT			m_rcPrintCallout;
		RECT			m_rcZapControl;
		HCURSOR			m_aCursors[MAX_CURSORS];
		COLORREF		m_crDraw;
		COLORREF		m_crRedact;
		COLORREF		m_crHighlight;
		COLORREF		m_crBackground;
		COLORREF		m_crCallout;
		COLORREF		m_crCallFrame;
		COLORREF		m_crCallHandle;
		COLORREF		m_crDeskew;
		COLORREF		m_crPrintBorder;
		COLORREF		m_crCalloutShadeForeground;

		short			m_sLeadError;
		short			m_sPage;
		short			m_sPages;
		short			m_sZoomState;
		short			m_sAngle;
		short			m_sLastAction;
		int				m_iWidth;
		int				m_iHeight;
		int				m_iTop;
		int				m_iLeft;
		int				m_iFillMode;
		float			m_fImageHeight;
		float			m_fImageWidth;
		float			m_fZoomFactor;
		float			m_fAspectRatio;
		float			m_fPrintBorderThickness;
		float			m_fZapFactor;
		float			m_fZapRatio;
		float			m_fZapWidth;
		float			m_fZapHeight;
		float			m_fPrintFactor;
		float			m_fPrintCx;
		float			m_fPrintCy;
		long			m_lPanX;
		long			m_lPanY;
		long			m_lMouseX;
		long			m_lMouseY;
		long			m_lAnnX;
		long			m_lAnnY;
		BOOL			m_bLoaded;
		BOOL			m_bAnimation;
		BOOL			m_bPlayingAnimation;
		BOOL			m_bSetAnnProps;
		BOOL			m_bPrintBorder;
		BOOL			m_bPrintCalloutBorders;
		BOOL			m_bResizeCallouts;
		BOOL			m_bPanCallouts;
		BOOL			m_bZoomCallouts;
		BOOL			m_bDIBPrintingEnabled;
		BOOL			m_bSplitScreen;
	
	public:

		//	This member is used by the control to associate a caller
		//	specified value with the window used to create a pane
		long			m_lUserData;

		//	This member is used to indicate whether or not the object is
		//	owned by a callout
		CCallout*		m_pOwner;
		RECT			m_rcZapMax;

		SAsyncParams	m_AsyncParams;

		//	Control properties
		CString			m_strFilename;
		CString			m_strAnnFontName;
		short			m_sAnnFontSize;
		short			m_sAction;
		short			m_sAnnTool;
		short			m_sAnnThickness;
		short			m_sAnnColor;
		short			m_sRedactColor;
		short			m_sHighlightColor;
		short			m_sCalloutColor;
		short			m_sRotation;
		short			m_sQFactor;
		short			m_sZoomOnLoad;
		short			m_sCallFrameThick;
		short			m_sCallFrameColor;
		short			m_sCallHandleColor;
		short			m_sBitonal;
		short			m_sAnnColorDepth;
		COLORREF		m_crCalloutShadeBackground;
		BOOL			m_bScaleImage;
		BOOL			m_bFitToImage;
		BOOL			m_bHideScrollBars;
		BOOL			m_bRightClickPan;
		BOOL			m_bSyncCalloutAnn;
		BOOL			m_bKeepAspect;
		BOOL			m_bZoomToRect;
		BOOL			m_bAnnFontBold;
		BOOL			m_bAnnFontUnderline;
		BOOL			m_bAnnFontStrikeThrough;
		BOOL			m_bAnnotateCallouts;
		BOOL			m_bShadeOnCallout;
		float			m_fMaxZoom;
		float			m_fPanPercent;
		BOOL            m_bZoomedSwipe;
		long            m_lTop;
		long            m_lBottom;

						CTMLead();
					   ~CTMLead();

		//	Public access members
		void			SetMaxRect(RECT* pRect, BOOL bSplitScreen, BOOL bRedraw);
		void			SetToScreenRatio(RECT* pRect);
		void			SetToRatio(RECT* pRect, double dRatio);
		void			GetSrcRects(RECT* pSrc, RECT* pClip);
		void			GetDstRects(RECT* pDst, RECT* pClip); 
		void			SetSrcRects(RECT* pSrc, RECT* pClip);
		void			SetDstRects(RECT* pDst, RECT* pClip); 
		void			SetDstRects(ANNRECT* pDst, ANNRECT* pClip); 
		void			SetAnnotations(HGLOBAL hAnnMem, long lAnnBytes);
		void			ShowRectangle(RECT* pRect, LPSTR lpTitle = "");
		void			ShowRectangle(ANNRECT* pAnnRect, LPSTR lpTitle = "");
		void			FireRectangleDiagnostic(RECT* pRect, LPSTR lpTitle = "");
		void			ShowRectangles(LPSTR lpTitle);
		void			SetControl(CTMViewCtrl* pControl, CErrorHandler* pErrors);
		void			SetColor(short sColor);
		void			UnloadImage();
		void			Copy(CTMLead* pLead);
		void			CopyAnn(CCallout* pCallout, HANNOBJECT hAnn);
		void			ChangeAnn(CCallout* pCallout, HANNOBJECT hAnn);
		void			DeleteAnn(CCallout* pCallout, HANNOBJECT hAnn);
		void			SourceToClient(RECT* pRect);
		void			ClientToSource(RECT* pRect);
		void			ClientToSource(ANNRECT* pRect);
		void			RotateRect(RECT* pRect, BOOL bClockwise);
		void			Rotate(short sAngle);
		void			OnEndEditTextAnn(BOOL bCancelled);
		short			DeleteAnn(HANNOBJECT hAnn, BOOL bIgnoreLock);
		void			DeleteAnn(DWORD dwTag);
		void			SelectAnn(DWORD dwTag);
		void			GetFormatText(int iFormat, CString &rFormat);
		short			GetLeadError();
		short			ClearSelections();
		short			DeleteSelections();
		CTMViewCtrl*	GetControl();
		CCallouts*		GetCallouts();
		long			GetCalloutCount();
		long			CopyAnnContainer();
		long			GetAnnCount(){ return m_Annotations.GetCount(); }
		HANNOBJECT		GetEditTextAnn(){ return m_hEditTextAnn; }
		HANNOBJECT		CopyAnn(CTMLead* pSource, HANNOBJECT hAnn);
		HANNOBJECT		GetHandleFromTag(DWORD dwTag);
		HANNOBJECT		GetAnnFromPt(POINT* pPoint);
		HANNOBJECT		DrawSourceRectangle(RECT rcBounds, COLORREF crColor, short sTransparency);
		HANNOBJECT		DrawRectangle(RECT rcBounds, COLORREF crColor, short sTransparency);
		HANNOBJECT		DrawSourceText(LPCSTR lpszText, RECT rcBounds, COLORREF crColor, LPCSTR lpszFont, short sSize);
		HANNOBJECT		DrawText(LPCSTR lpszText, RECT rcBounds, COLORREF crColor, LPCSTR lpszFont, short sSize);
		short			LockAnn(HANNOBJECT hAnn);
		short			UnlockAnn(HANNOBJECT hAnn);
		DWORD			GetTagFromHandle(HANNOBJECT hAnn);
		CAnnotations&	GetAnnotations(){ return m_Annotations; }
		CAnnotation*	GetAnnFromTag(DWORD dwTag);
		CAnnotation*	GetAnnFromHandle(HANNOBJECT hAnn);
		UINT			GetSelections(HANNOBJECT** pSelections);
		BOOL			Create(CWnd* pParent, UINT uId = IDC_TMLEAD);
		COLORREF		GetColorRef(int iColor);
		COLORREF		GetDeskewBackColor();
		COLORREF		GetPrintBorderColor();
		COLORREF		GetCallFrameColor();
		COLORREF		GetCallHandleColor();
		int				GetColorId(COLORREF crColor);
		int				GetColor();
		int				GetScreenWidth();
		int				GetScreenHeight();
		short			GetAction();
		short			GetAngle();
		LPCTSTR			GetFilename();
		float			GetSrcAspect();
		float			GetDstAspect();
		float			GetWndAspect();
		float			GetPrintBorderThickness();
		double			GetScreenRatio();
		BOOL			GetPaneScreenRect(RECT* prcRect, BOOL bMax);
		BOOL			GetPrintBorder();
		BOOL			GetPrintCalloutBorders();
		BOOL			GetResizeCallouts();
		BOOL			GetPanCallouts();
		BOOL			GetZoomCallouts();
		BOOL			SetZapParameters(RECT* pZapControl);
		BOOL			ResizeToRatio(RECT* pRect, float fRatio);
		BOOL			AnnGetText(HANNOBJECT hAnn, CString& rText);
		BOOL			GetSrcVisible(RECT* pSrc);
		BOOL			GetSrcVisible(ANNRECT* pAnnSrc);
		BOOL			GetDIBPrintingEnabled(){ return m_bDIBPrintingEnabled; }
		void			ResizeViewToSrc(ANNRECT* prcSrc);
		void			RescaleZapCallouts();
		void			ResetZapParameters();
		void			ResizeSourceToImage();
		void			ResizeSourceToView();
		void			RotateSrcRect(ANNRECT* prcSrc, BOOL bClockwise);
		CTMLead*		Rasterize();
		CTMLead*		GetScratchCopy();
	
		//	Property change handlers
		void			SetAction(short sAction);
		void			SetBackColor(OLE_COLOR ocColor, COLORREF crColor); 
		void			SetRedactColor(short sRedactColor);
		void			SetHighlightColor(short sHighlightColor);
		void			SetCalloutColor(short sCalloutColor);
		void			SetCalloutShadeColor(COLORREF crColor);
		void			SetShadeOnCallout(BOOL bShadeOnCallout);
		void			SetDeskewBackColor(COLORREF crColor);
		void			SetPrintBorderColor(COLORREF crColor);
		void			SetPrintBorder(BOOL bPrint);
		void			SetPrintCalloutBorders(BOOL bPrint);
		void			SetAnnColor(short sAnnColor);
		void			SetAnnColorDepth(short sAnnColorDepth);
		void			SetAnnTool(short sAnnTool);
		void			SetAnnThickness(short sAnnThickness);
		void			SetAnnFontName(LPCTSTR lpName);
		void			SetAnnFontSize(short sSize);
		void			SetAnnFontBold(BOOL bBold);
		void			SetAnnFontUnderline(BOOL bUnderline);
		void			SetAnnFontStrikeThrough(BOOL bStrikeThrough);
		void			SetBitonal(short sBitonal);
		void			SetMaxZoom(short sMaxZoom);
		void			SetRotation(short sRotation);
		void			SetPanPercent(short sPanPercent);
		void			SetZoomOnLoad(short sZoomOnLoad);
		void			SetScaleImage(BOOL bScaleImage);
		void			SetFitToImage(BOOL bFitToImage);
		void			SetHideScrollBars(BOOL bHideScrollBars);
		void			SetKeepAspect(BOOL bKeepAspect);
		void			SetZoomToRect(BOOL bZoomToRect);
		void			SetRightClickPan(BOOL bRightClickPan);
		void			SetLoopAnimate(BOOL bLoop);
		void			SetResizeCallouts(BOOL bResize);
		void			SetPanCallouts(BOOL bPan);
		void			SetZoomCallouts(BOOL bZoom);
		void			SetCallFrameThickness(short sThickness);
		void			SetCallFrameColor(short sColor);
		void			SetCallFrameColor(COLORREF crColor);
		void			SetCallHandleColor(short sColor);
		void			SetCallHandleColor(COLORREF crColor);
		void			SetSyncCalloutAnn(BOOL bSync);
		void			SetAnnotateCallouts(BOOL bAnnotateCallouts);
		void			SetPrintBorderThickness(float fThickness);
		void			SetQFactor(short sQFactor);
		short			SetFilename(LPCTSTR lpszNewValue, BOOL bDraw = TRUE);

		//	Control Methods
		void			Redraw();
		void			Rotate(BOOL bRedraw);
		void			RotateCw(BOOL bRedraw);
		void			RotateCcw(BOOL bRedraw);
		void			Erase(BOOL bIgnoreLocked);
		void			ResetZoom();
		void			ZoomFullWidth();
		void			ZoomFullHeight();
		void			EnableDIBPrinting(BOOL bEnable);
		float			GetImageHeight();
		float			GetImageWidth();
		short			GetCurrentPage();
		float			GetAspectRatio();
		short			GetPageCount();
		short			GetPanStates();
		short			GetZoomState();
		float			GetZoomFactor();
		short			NextPage();
		short			PrevPage();
		short			LastPage();
		short			FirstPage();
		short			PlayAnimation(BOOL bPlay, BOOL bContinuous);
		short			Render(OLE_HANDLE hDc, float fLeft, float fTop,
							   float fWidth, float fHeight);
		short			Copy();
		short			Paste();
		short			Realize(BOOL bRemove);
		short			Deskew();
		short			Despeckle();
		short			Smooth(long lLength, int iFavorLong);
		short			BorderRemove(long lBorderPercent, long lWhiteNoise,
									 long lVariance, long lLocation);
		short			DotRemove(long lMinWidth, long lMinHeight, 
								  long lMaxWidth, long lMaxHeight);
		short			HolePunchRemove(long lMinWidth, long lMinHeight, 
								        long lMaxWidth, long lMaxHeight,
										long lLocation);
		short			HolePunchRemove2(long lMinHoles, long lMaxHoles, 
										long lLocation);
		short			Cleanup(LPCTSTR lpszSaveAs);
		BOOL			RenderDIB(HDC hdc, int iLeft, int iTop,
								  int iWidth, int iHeight);
		short			ViewImageProperties();
		short			GetImageProperties(STMVImageProperties* pProperties);
		short			ShowCallouts(BOOL bShow);
		short			DestroyCallouts();
		short			Save(LPCTSTR lpszFilename);
		short			SavePages(LPCTSTR lpszFilename, LPCSTR lpszFolder, LPCSTR lpszPrefix);
		short			DeleteLastAnn();
		BOOL			Pan(short sDirection);
		BOOL			IsAnimation();
		BOOL			IsPlaying();
		BOOL			IsLoaded();
		BOOL			FindFile(LPCSTR lpFilespec);
		long			CreateAnnContainer();
		CErrorHandler*	GetHandler();

		//	Event handlers
		void			OnAnnDrawn(long hObject);
		void			OnAnnCreate(long hObject);
		void			OnAnnChange(long hObject, long uType);
		void			OnAnnDestroy(long hObject);
		void			OnAnnSelect();
		void			OnMouseDown(short Button, short Shift, long X, long Y);
		void			OnRubberBand();
		void			OnAnimate(BOOL bEnable);
		void			OnMouseMove(short Button, short Shift, long x, long y);
		void			OnMouseUp(short Button, short Shift, long x, long y);
		void			OnMouseDblClick();
		void			OnAnnMouseDown(short Button, short Shift, long x, long y);
		void			OnAnnMouseUp(short Button, short Shift, long x, long y);
		void			OnKeyUp(short* KeyCode, short Shift); 

		//	Callout notification handlers
		void			OnCloseCallout(CCallout* pCallout);
		void			OnCalloutSelection(CCallout* pSelector);
		void			OnClickCallout(CCallout* pCallout, 
									   short sButton, short sKey);
		void			OnActivateCallout(CCallout* pCallout, BOOL bNotify);
		void			OnCalloutResized(CCallout* pCallout); 
		void			OnCalloutMoved(CCallout* pCallout); 
		void			OnCalloutModified(CCallout* pCallout, RECT* pSource);
		void			RedrawZoomed();

		//	Treatments
		void			LoadCallout(SZapHeader* pHeader, SZapCallout* pZap, HGLOBAL hAnnMem);
		BOOL			ReadZapCallout(CFile* pFile, SZapCallout* pHeader,
									   HGLOBAL* phAnnMem);
		BOOL			LoadZap(CFile* pFile, SZapHeader* pHeader,
								BOOL bUseView, BOOL bScaleView, 
								BOOL bCallouts, LPCSTR lpszSourceFile);
		BOOL			LoadCallouts(CFile* pFile, SZapHeader* pZap,
								     SZapPane* pPane, BOOL bVisible);
		BOOL			SaveZap(CFile* pFile);
		UINT			MsgBox(SZapHeader* pHeader, LPCSTR lpszTitle = NULL);
		UINT			MsgBox(SZapPane* pPane, LPCSTR lpszTitle = NULL);
		UINT			MsgBox(SZapCallout* pCallout, LPCSTR lpszTitle = NULL);

		//	These functions maintain backward compatability with zap files
		//	created prior to revision 2.1
		void			LoadOldCallouts(CTMIni* pIni);
		short			LoadOldZap(LPCTSTR lpszFilename, LPCTSTR lpszSection,
								   BOOL bUseView, BOOL bScaleView, 
								   BOOL bCallouts);

		//	Printing operations
		BOOL			IsPostScript(HDC hdc);
		BOOL			GetRenderAsDIB(HDC hdc, RECT* prcPrint);
		BOOL			GetRenderAsDIB(HDC hdc, int iPrintWidth, int iPrintHeight);
		BOOL			Print(CDC* pdc, RECT* prcImage, RECT* prcCallouts, BOOL bFullImage = TRUE, short sRotation = 0);
		void			PrintCallouts(CDC* pdc, RECT* pRect, short sRotation = 0);
		void			PrintCallout(CCallout* pCallout, CDC* pdc, RECT* pRect, short sRotation = 0);
		void			PrintBorder(CDC* pdc, COLORREF crColor, float fThickness, 
									RECT* pRect); 

	protected:	

		void			Draw();
		void			Erase(CDC* pdc, RECT* pRect);
		void			RedrawFullWidth();
		void			RedrawFullHeight();
		void			RedrawZap();
		void			RedrawNormal();
		void			ResizeWndToRatio(float fHeight, float fWidth);
		void			ResizeView();
		void			ResizeWndToDst();
		void			ResizeWndToSrc();
		void			ResizeWndToMax();
		void			SetSrcVisible(ANNRECT* prcSrc, BOOL bFullSource);
		void			SetDraw();
		void			SetRedact();
		void			SetHighlight();
		void			SetZoom();
		void			SetCallout();
		void			SetPan();
		void			SetNone();
		void			SetSelect();
		void			SetAnnUserModeEx(short sMode);
		void			Pan(long lX, long lY);
		void			ZoomUnrestricted(RECT* pRect);
		void			ZoomRestricted(RECT* pRect);
		void			CreateUserCallout(RECT* prcUser);
		void			RotateAnnCopy(CTMLead* pSource, HANNOBJECT hObject);
		void			SyncAnnotation(CAnnotation* pAnn, CCallout* pExclude = 0);
		void			SyncSelection(CAnnotation* pAnn, CCallout* pExclude = 0);
		void			DeleteCallout(CCallout* pCallout);
		void			ResyncAnnotations();
		void			RotateCallouts(BOOL bClockwise, BOOL bRedraw);
		short			HandleFileError(LPCTSTR lpszFile, short sLeadError);
		short			LoadImage(LPCTSTR lpszFile, short sPage);
		short			HandleLeadError(short sError);
		short			GetSaveQFactor();
		HANNOBJECT		DrawCalloutAnn(RECT* pRect, BOOL bBackground);
		

	//	This portion of the file is maintained by Class Wizard
	public:

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTMLead)
	//}}AFX_VIRTUAL

	protected:
	//{{AFX_MSG(CTMLead)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:
	void GesturePan(long lX, long lY);
	void GestureZoom(float zoomFactor);
	void ZoomToFactor();
};


//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMLEAD_H__808F18DA_1C7D_11D1_B033_008029EFD140__INCLUDED_)
