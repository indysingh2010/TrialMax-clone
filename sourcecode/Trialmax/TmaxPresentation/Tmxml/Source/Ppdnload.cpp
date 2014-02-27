//==============================================================================
//
// File Name:	ppdnload.cpp
//
// Description:	This file contains member functions of the CPPDownload class
//
// See Also:	pptmx.h
//
// Copyright	FTI Consulting 2002
//
//==============================================================================
//	Date		Revision    Description
//	09-25-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <ppdnload.h>
#include <xmlframe.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern SDownload theDownload;


//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
IMPLEMENT_DYNCREATE(CPPDownload, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPDownload, CPropertyPage)
	//{{AFX_MSG_MAP(CPPDownload)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPDownload::CPPDownload()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPDownload::CPPDownload() : CPropertyPage(CPPDownload::IDD)
{
	//{{AFX_DATA_INIT(CPPDownload)
	m_strAborted = _T("");
	m_strCached = _T("");
	m_strComplete = _T("");
	m_strError = _T("");
	m_strErrorMsg = _T("");
	m_strLParam = _T("");
	m_strMaxProgress = _T("");
	m_strProgress = _T("");
	m_strRemote = _T("");
	m_strSource = _T("");
	m_strStatus = _T("");
	//}}AFX_DATA_INIT

}

//==============================================================================
//
// 	Function Name:	CPPDownload::~CPPDownload()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPDownload::~CPPDownload()
{
}

//==============================================================================
//
// 	Function Name:	CPPDownload::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPDownload::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPDownload)
	DDX_Text(pDX, IDC_ABORTED, m_strAborted);
	DDX_Text(pDX, IDC_CACHED, m_strCached);
	DDX_Text(pDX, IDC_COMPLETE, m_strComplete);
	DDX_Text(pDX, IDC_ERROR, m_strError);
	DDX_Text(pDX, IDC_ERROR_MESSAGE, m_strErrorMsg);
	DDX_Text(pDX, IDC_LPARAM, m_strLParam);
	DDX_Text(pDX, IDC_MAX_PROGRESS, m_strMaxProgress);
	DDX_Text(pDX, IDC_PROGRESS, m_strProgress);
	DDX_Text(pDX, IDC_REMOTE, m_strRemote);
	DDX_Text(pDX, IDC_SOURCE, m_strSource);
	DDX_Text(pDX, IDC_STATUS, m_strStatus);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPPDownload::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPDownload::OnInitDialog() 
{
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	//	Initialize the controls
	m_strComplete = theDownload.bComplete ? "TRUE" : "FALSE";
	m_strAborted  = theDownload.bAbort ? "TRUE" : "FALSE";
	m_strRemote   = theDownload.bRemote ? "TRUE" : "FALSE";
	m_strError    = theDownload.bError ? "TRUE" : "FALSE";
	m_strSource   = theDownload.strSource;
	m_strCached   = theDownload.strCached;
	m_strStatus   = theDownload.strStatus;
	m_strErrorMsg = theDownload.strErrorMsg;
	m_strProgress.Format("%lu", theDownload.ulProgress);
	m_strMaxProgress.Format("%lu", theDownload.ulProgressMax);
	m_strLParam.Format("%lx", theDownload.lParam);

	UpdateData(FALSE);

	return TRUE;
}

