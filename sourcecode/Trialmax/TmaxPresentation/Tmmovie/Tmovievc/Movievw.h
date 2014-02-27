// Movievw.h : interface of the CMovieView class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_MOVIEVW_H__10AFCB1F_02BA_11D2_B1BF_008029EFD140__INCLUDED_)
#define AFX_MOVIEVW_H__10AFCB1F_02BA_11D2_B1BF_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#include <tmmovie.h>

class CMovieView : public CFormView
{
protected: // create from serialization only
	CMovieView();
	DECLARE_DYNCREATE(CMovieView)

public:
	//{{AFX_DATA(CMovieView)
	enum { IDD = IDD_TMOVIEVC_FORM };
	CTMMovie	m_Movie;
	//}}AFX_DATA

// Attributes
public:
	CMovieDoc* GetDocument();
	CStatusBar*  m_pStatus;
	BOOL		 m_bResumeCue;

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMovieView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMovieView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CMovieView)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnSetfilename();
	afx_msg void OnTmPause();
	afx_msg void OnUpdateTmPause(CCmdUI* pCmdUI);
	afx_msg void OnTmPlay();
	afx_msg void OnUpdateTmPlay(CCmdUI* pCmdUI);
	afx_msg void OnTmResume();
	afx_msg void OnUpdateTmResume(CCmdUI* pCmdUI);
	afx_msg void OnTmShow();
	afx_msg void OnTmStop();
	afx_msg void OnUpdateTmStop(CCmdUI* pCmdUI);
	afx_msg void OnTmAutoplay();
	afx_msg void OnUpdateTmAutoplay(CCmdUI* pCmdUI);
	afx_msg void OnTmUnload();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnTmFwd1();
	afx_msg void OnTmFwd150();
	afx_msg void OnTmFwd30();
	afx_msg void OnTmRev1();
	afx_msg void OnTmRev150();
	afx_msg void OnTmRev30();
	afx_msg void OnTmCuefirst();
	afx_msg void OnTmCuelast();
	afx_msg void OnTmResumecue();
	afx_msg void OnUpdateTmResumecue(CCmdUI* pCmdUI);
	afx_msg void OnTmAutoshow();
	afx_msg void OnUpdateTmAutoshow(CCmdUI* pCmdUI);
	afx_msg void OnTmCueabsolute();
	afx_msg void OnTmVideoprops();
	afx_msg void OnTmKeepaspect();
	afx_msg void OnUpdateTmKeepaspect(CCmdUI* pCmdUI);
	afx_msg void OnTmScale();
	afx_msg void OnUpdateTmScale(CCmdUI* pCmdUI);
	afx_msg void OnTmSofter();
	afx_msg void OnUpdateTmSofter(CCmdUI* pCmdUI);
	afx_msg void OnTmSlower();
	afx_msg void OnUpdateTmSlower(CCmdUI* pCmdUI);
	afx_msg void OnTmRight();
	afx_msg void OnUpdateTmRight(CCmdUI* pCmdUI);
	afx_msg void OnTmLouder();
	afx_msg void OnUpdateTmLouder(CCmdUI* pCmdUI);
	afx_msg void OnTmLeft();
	afx_msg void OnUpdateTmLeft(CCmdUI* pCmdUI);
	afx_msg void OnTmFaster();
	afx_msg void OnUpdateTmFaster(CCmdUI* pCmdUI);
	afx_msg void OnTmGetresolution();
	afx_msg void OnTmRange();
	afx_msg void OnTmStep();
	afx_msg void OnTmUpdatevideo();
	afx_msg void OnTmSnapshot();
	afx_msg void OnTmCapture();
	afx_msg void OnTmDibsnaps();
	afx_msg void OnUpdateTmDibsnaps(CCmdUI* pCmdUI);
	afx_msg void OnTmFilterprops();
	afx_msg void OnTmOverlayvisible();
	afx_msg void OnUpdateTmOverlayvisible(CCmdUI* pCmdUI);
	afx_msg void OnTmSetoverlay();
	afx_msg void OnTmClearoverlay();
	afx_msg void OnFileChangeTmmoviectrl1(LPCTSTR lpFilename);
	afx_msg void OnStateChangeTmmoviectrl1(short sState);
	afx_msg void OnPlaybackErrorTmmoviectrl1(long lError, BOOL bStopped);
	afx_msg void OnPlaybackCompleteTmmoviectrl1();
	afx_msg void OnClassId();
	afx_msg void OnRegisteredPath();
	afx_msg void OnTmDetachbeforeload();
	afx_msg void OnUpdateTmDetachbeforeload(CCmdUI* pCmdUI);
	afx_msg void OnTmSetUserFilters();
	afx_msg void OnPositionChangeTmmoviectrl1(double dPosition);
	afx_msg void OnTmShowaudioimage();
	afx_msg void OnUpdateTmShowaudioimage(CCmdUI* pCmdUI);
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in Movievw.cpp
inline CMovieDoc* CMovieView::GetDocument()
   { return (CMovieDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MOVIEVW_H__10AFCB1F_02BA_11D2_B1BF_008029EFD140__INCLUDED_)
