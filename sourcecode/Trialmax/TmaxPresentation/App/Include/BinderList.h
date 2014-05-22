//==============================================================================
//
// File Name:	BinderList.h
//
// Description:	This file contains dialog of binder list
//				
//
// See Also:	BinderList.cpp
//
// Author:		Muhammad Hussain
//
// Copyright	Tenpearls LLC 2014
//
//==============================================================================
//	Date		Revision    Description
//	24-03-14	1.00		Original Release
//==============================================================================

#if !defined(__BINDERLIST_H__)
#define __BINDERLIST_H__
#endif


#if _MSC_VER > 1000
#pragma once
#endif

// CBinderList dialog
#include <resource.h>
#include < map >
#include <vector>
#include <afxbutton.h>
#include <BinderEntry.h>
#include <afxwin.h>
#include <list>
using namespace std;

class CMainView;

class CBinderList : public CDialog
{
	DECLARE_DYNAMIC(CBinderList)

public:
	CBinderList(CMainView* parentWindow, int nTotalRecords,CWnd* pParent = NULL);   // standard constructor
	virtual ~CBinderList();	// Destructor
	
	typedef void (CBinderList::*fn)(int iButtonId);
	typedef std::map< UINT, fn > EventMessageMap;
	
	int					m_nXPosition;
	int					m_nYPosition;	
	BOOL				m_bIsShowBackButton;
	list<CBinderEntry>  m_binderEntryList;	
	CBinderEntry		m_parentBinder;	
	
// Dialog Data
	enum { IDD = IDD_BINDER_LIST };

public:
	virtual BOOL OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo);	
	void OnCancel();
	void HandleMouseClick();	
	void PostNcDestroy();
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
			BOOL CBinderList::OnInitDialog();			
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);		
			void CBinderList::Recieved(int iButtonId);			
	DECLARE_MESSAGE_MAP()

private:
	long			m_lCurrentParentId;
	CMainView*		m_parentWindow;
	CBrush			m_brush;
	EventMessageMap m_msgMap;	
	int				m_nCurHeight;
	int				m_nScrollPos;	
	int				m_nListHeight;
	int				m_nTotalRecords;	

private:
	BOOL		AddButtons();
	CMFCButton*  CreateButton(CString sButtonName, DWORD pButtonStyle, int nButtonId, int nButtonWidth, int nButtonHeight, 
	int			nXPosition, int nYPosition,int nButtonNumber, int nMediaType);
	void		CleanButtons();
	void		LogMe(LPCTSTR msg);
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);			
	afx_msg BOOL OnMouseWheel(UINT nFlags, short zDelta, CPoint pt);

public:
	void HandlePan(int diff);
};
