//==============================================================================
//
// File Name:	callout.h
//
// Description:	This file contains the declarations of the CCallout and 
//				CCallouts classes.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	03-22-98	1.00		Original Release
//==============================================================================
#if !defined(AFX_CALLOUT_H__606BC621_C025_11D1_B14D_008029EFD140__INCLUDED_)
#define AFX_CALLOUT_H__606BC621_C025_11D1_B14D_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmlead.h>
#include <ltann.h>
#include <resource.h>
#include <tmtrack.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define CALLOUT_ZAP_SHADED		0x0001

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CTMViewCtrl;
class CAnnotation;

class CCallout : public CDialog
{
	private:

		CTMTracker		m_Tracker;
		CTMViewCtrl*	m_pControl;
		CTMLead			m_TMLead;
		CTMLead*		m_pSource;
		CBrush*			m_pBackground;
		RECT			m_rcDrag;
		RECT			m_rcOriginalContainer;
		RECT			m_rcOriginalPosition;
		BOOL			m_bDrag;
		BOOL			m_bParentNotify;
		BOOL			m_bEventNotify;
		BOOL			m_bSetCursor;
		BOOL			m_bAnnotateCallouts;
		BOOL			m_bResizeable;
		BOOL			m_bPanCallouts;
		BOOL			m_bZoomCallouts;
		BOOL			m_bShaded;
		long			m_lDragX;
		long			m_lDragY;
		WORD			m_wAnnId;
		short			m_sAction;
		float			m_fScaleFactor;
		int				m_iMouseButton;

	public:
	
		//	Public members used by CTMLead to recreate and print a callout
		RECT			m_rcMax;
		RECT			m_rcDst;
		RECT			m_rcRubberBand;
		RECT			m_rcContainer;
		int				m_iWidth;
		int				m_iHeight;

						CCallout(CTMViewCtrl* pControl, CTMLead* pSource);
					   ~CCallout();
						   
		void			SetOriginalContainer(RECT* pContainer);
		void			SetOriginalPosition(RECT* pPosition);
		void			SetContainer(RECT* pContainer);
		void			Rescale();
		void			SetRects(RECT* pMax, RECT* pDst, RECT* pRubberBand, int iFrame, BOOL isResize = FALSE);
		void			SetAnnotations(HGLOBAL hAnnMem, long lAnnBytes);
		void			Rotate(BOOL bClockwise, BOOL bRedraw);
		void			SetAnnId(WORD wId);
		void			SetAnnotateCallouts(BOOL bAnnotateCallouts);
		void			DeleteAnn(HANNOBJECT hAnn);
		void			DeleteAnn(DWORD dwTag);
		void			SelectAnn(DWORD dwTag);
		void			Print(CDC* pdc, RECT* pRect, short sRotation);
		void			OnPanComplete();
		void			OnZoomComplete();
		void			OnModified();
		short			ClearSelections();
		short			DeleteSelections();
		HANNOBJECT		CopyAnn(CTMLead* pSource, HANNOBJECT hAnn);
		HANNOBJECT		GetHandleFromTag(DWORD dwTag);
		CAnnotation*	GetAnnFromTag(DWORD dwTag);
		DWORD			GetTagFromHandle(HANNOBJECT hAnn); 
		BOOL			SaveZap(CFile* pFile);
		BOOL			GetShaded();
		BOOL			ScalePrintRect(RECT* prcPrint, double* pdWidth, double* pdHeight);
		CTMLead*		GetTMLead();
		WORD			GetAnnId();
		
		//	Property change handlers
		void			SetAction(short sAction);
		void			SetRedactColor(short sRedactColor);
		void			SetHighlightColor(short sHighlightColor);
		void			SetAnnColor(short sAnnColor);
		void			SetAnnColorDepth(short sAnnColorDepth);
		void			SetAnnTool(short sAnnTool);
		void			SetAnnThickness(short sAnnThickness);
		void			SetAnnFontName(LPCTSTR lpName);
		void			SetAnnFontSize(short sSize);
		void			SetAnnFontBold(BOOL bBold);
		void			SetAnnFontUnderline(BOOL bUnderline);
		void			SetAnnFontStrikeThrough(BOOL bStrikeThrough);
		void			SetHideScrollBars(BOOL bHideScrollBars);
		void			SetRightClickPan(BOOL bRightClickPan);
		void			SetPanCallouts(BOOL bPan);
		void			SetZoomCallouts(BOOL bZoom);
		void			SetFrameThickness(short sThickness);
		void			SetFrameColor(COLORREF crColor);
		void			SetHandleColor(COLORREF crColor);
		void			SetResizeable(BOOL bResizeable);
		void			SetShaded(BOOL bShaded);

	protected:

		void			ShowRectangles(LPCSTR lpATitle);
		void			ShowRectangle(RECT* pRect, LPCSTR lpTitle); 
		void			DrawDragRect();
		void			StartDrag(long lX, long lY);
		void			EndDrag();
		void			ResizeSourceToView();
		void			ResizeSourceToImage();
		short			GetFrameThickness();
		CTMLead*		GetScratchCopy();
		BOOL			Resize();

	//	The remainder of this declaration is maintained by Class Wizard

	public:
	//{{AFX_DATA(CCallout)
	enum { IDD = IDD_CALLOUT };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA


	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CCallout)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	afx_msg void OnEvAnnDrawn(long hObject);
	afx_msg void OnEvAnnCreate(long hObject);
	afx_msg	void OnEvAnnChange(long hObject, long uType);
	afx_msg void OnEvAnnDestroy(long hObject);
	afx_msg void OnEvAnnSelect(const VARIANT FAR& aObjects, short uCount);
	afx_msg void OnEvMouseDown(short Button, short Shift, long X, long Y);
	afx_msg void OnEvRubberBand();
	afx_msg void OnEvMouseMove(short Button, short Shift, long x, long y);
	afx_msg void OnEvMouseUp(short Button, short Shift, long x, long y);
	afx_msg void OnEvMouseClick();
	afx_msg void OnEvMouseDblClick();
	afx_msg void OnEvAnnMouseDown(short Button, short Shift, long x, long y);

	// Generated message map functions
	//{{AFX_MSG(CCallout)
	afx_msg void OnPaletteChanged(CWnd* pFocusWnd);
	afx_msg BOOL OnQueryNewPalette();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnPaint();
	afx_msg void OnParentNotify(UINT message, LPARAM lParam);
	afx_msg BOOL OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//	Objects of this class are used to manage a list of callouts.
class CCallouts : public CObList
{
	private:

	public:

							CCallouts();
		virtual			   ~CCallouts();

		virtual BOOL		Add(CCallout* pCallout);
		virtual BOOL		CheckShaded();
		virtual void		Flush(BOOL bDelete);
		virtual void		Remove(CCallout* pCallout, BOOL bDelete);
		virtual POSITION	Find(CCallout* pCallout);
		virtual CCallout*	Find(WORD wAnnId);

		//	List iteration members
		virtual CCallout*	First();
		virtual CCallout*	Last();
		virtual CCallout*	Next();
		virtual CCallout*	Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CALLOUT_H__606BC621_C025_11D1_B14D_008029EFD140__INCLUDED_)
