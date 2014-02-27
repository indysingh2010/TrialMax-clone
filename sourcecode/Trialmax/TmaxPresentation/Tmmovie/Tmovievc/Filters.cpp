// Filters.cpp : implementation file
//

#include "stdafx.h"
#include "Tmovievc.h"
#include "Filters.h"
#include <tmmovie.h>
#include <tmmvdefs.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CFilters dialog


CFilters::CFilters(CWnd* pParent /*=NULL*/)
	: CDialog(CFilters::IDD, pParent)
{
	//{{AFX_DATA_INIT(CFilters)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	m_pTMMovie = 0;
}


void CFilters::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CFilters)
	DDX_Control(pDX, IDC_USER, m_ctrlUser);
	DDX_Control(pDX, IDC_REMOVE, m_ctrlRemove);
	DDX_Control(pDX, IDC_REGISTERED, m_ctrlRegistered);
	DDX_Control(pDX, IDC_ADD, m_ctrlAdd);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CFilters, CDialog)
	//{{AFX_MSG_MAP(CFilters)
	ON_LBN_SELCHANGE(IDC_REGISTERED, OnRegisteredChanged)
	ON_LBN_SELCHANGE(IDC_USER, OnUserChanged)
	ON_BN_CLICKED(IDC_ADD, OnAdd)
	ON_BN_CLICKED(IDC_REMOVE, OnRemove)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


BOOL CFilters::OnInitDialog() 
{
	CString strFilters;

	CDialog::OnInitDialog();
	
	if(m_pTMMovie)
	{
		//	Fill the list of registered filters
		strFilters = m_pTMMovie->GetRegFilters(0);
		FillListBox(strFilters, m_ctrlRegistered);

		//	Fill the list of user filters
		strFilters = m_pTMMovie->GetUserFilters(0);
		FillListBox(strFilters, m_ctrlUser);

		m_ctrlRegistered.SetCurSel(0);
		m_ctrlUser.SetCurSel(0);

		OnRegisteredChanged();
		OnUserChanged();

	}
	
	return TRUE;  
}

void CFilters::FillListBox(CString& rFilters, CListBox& rListBox)
{
	char szBuffer[2048];
	char* pToken;
	char* pNext;

	rListBox.ResetContent();

	lstrcpyn(szBuffer, rFilters, sizeof(szBuffer));
	pToken = strtok_s(szBuffer, "\r", &pNext);
	while(pToken != NULL)
	{
		rListBox.AddString(pToken);
		pToken = strtok_s(NULL, "\r", &pNext);
	}

}

void CFilters::OnRegisteredChanged() 
{
	CString Selection;
	int iIndex;

	if((iIndex = m_ctrlRegistered.GetCurSel()) != LB_ERR)
	{
		m_ctrlRegistered.GetText(iIndex, Selection);
		m_ctrlAdd.EnableWindow(m_ctrlUser.FindStringExact(-1, Selection) == LB_ERR);
	}
	else
	{
		m_ctrlAdd.EnableWindow(FALSE);
	}
	
}

void CFilters::OnUserChanged() 
{
	int iIndex;

	if((iIndex = m_ctrlUser.GetCurSel()) != LB_ERR)
	{
		m_ctrlRemove.EnableWindow(TRUE);
	}
	else
	{
		m_ctrlRemove.EnableWindow(FALSE);
	}
	
}

void CFilters::OnAdd() 
{
	int iIndex = m_ctrlRegistered.GetCurSel();
	CString strName;

	if(iIndex != LB_ERR)
	{
		m_ctrlRegistered.GetText(iIndex, strName);
		
		if((strName.IsEmpty() == FALSE) && (m_ctrlUser.FindStringExact(-1, strName) == LB_ERR))
		{
			if(m_pTMMovie->AddFilter(strName) == TMMOVIE_NOERROR)
				m_ctrlUser.AddString(strName);
		}
	
	}	
}

void CFilters::OnRemove() 
{
	int iIndex = m_ctrlUser.GetCurSel();
	CString strName;

	if(iIndex != LB_ERR)
	{
		m_ctrlUser.GetText(iIndex, strName);
		if(m_pTMMovie->RemoveFilter(strName) == TMMOVIE_NOERROR)
		{
			m_ctrlUser.DeleteString(iIndex);
			m_ctrlUser.SetCurSel(0);
			OnUserChanged();
		}	
	}	
}
