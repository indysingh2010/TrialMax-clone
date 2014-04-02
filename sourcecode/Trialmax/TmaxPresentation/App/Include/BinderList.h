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
#include <list>
using namespace std;

class CMainView;

class CBinderList : public CDialog
{
	DECLARE_DYNAMIC(CBinderList)

public:
	CBinderList(CMainView* parentWindow, CWnd* pParent = NULL);   // standard constructor
	virtual ~CBinderList();	// Destructor
	
	typedef void (CBinderList::*fn)(int iButtonId);
	typedef std::map< UINT, fn > EventMessageMap;
	
	int					m_xPosition;
	int					m_yPosition;	
	BOOL				m_isShowBackButton;
	list<CBinderEntry>  m_binderEntryList;	
	CBinderEntry		m_parentBinder;	
	
// Dialog Data
	enum { IDD = IDD_BINDER_LIST };

public:
	virtual BOOL OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo);	
	void OnCancel();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
			BOOL CBinderList::OnInitDialog();			
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);	

			void CBinderList::Recieved(int iButtonId);		
			void CBinderList::PostNcDestroy();

	DECLARE_MESSAGE_MAP()
	

private:
	long			m_lCurrentParentId;
	CMainView*		m_parentWindow;
	CBrush			m_brush;
	EventMessageMap m_msgMap;	

private:
	BOOL AddButtons();
	void CreateButton(CString sButtonName, DWORD pButtonStyle, int iButtonId, int iButtonWidth, int iButtonHeight, 
				int iXPosition, int iYPosition,int iButtonNumber, int iMediaType);
	void CBinderList::CleanButtons();
	void LogMe(LPCTSTR msg);
};
