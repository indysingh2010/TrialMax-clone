//==============================================================================
//
// File Name:	ppviewer.cpp
//
// Description:	This file contains member functions of the CPPViewer class
//
// See Also:	ppviewer.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-10-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <ppviewer.h>
#include <tmxmdefs.h>

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


//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
IMPLEMENT_DYNCREATE(CPPViewer, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPViewer, CPropertyPage)
	//{{AFX_MSG_MAP(CPPViewer)
	ON_BN_CLICKED(IDC_ASSIGN_PORT, OnAssignPort)
	ON_BN_CLICKED(IDC_PROXY_SERVER, OnProxyServer)
	ON_BN_CLICKED(IDC_DEFAULT_PORT, OnDefaultPort)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPViewer::CPPViewer()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPViewer::CPPViewer() : CPropertyPage(CPPViewer::IDD)
{
	//{{AFX_DATA_INIT(CPPViewer)
	m_fProgressDelay = 0.0f;
	m_bEnableErrors = FALSE;
	m_bFloatProgress = FALSE;
	m_bConfirmBatch = FALSE;
	m_iConnection = 0;
	m_uInternetPort = 0;
	m_strProxyAddress = _T("");
	m_uProxyPort = 0;
	m_bShowPageNavigation = FALSE;
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CPPViewer::~CPPViewer()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPViewer::~CPPViewer()
{
}

//==============================================================================
//
// 	Function Name:	CPPViewer::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPViewer::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPViewer)
	DDX_Control(pDX, IDC_PROXY_PORT_LABEL, m_ctrlProxyPortLabel);
	DDX_Control(pDX, IDC_PROXY_PORT, m_ctrlProxyPort);
	DDX_Control(pDX, IDC_PROXY_ADDRESS_LABEL, m_ctrlProxyAddressLabel);
	DDX_Control(pDX, IDC_INTERNET_PORT, m_ctrlInternetPort);
	DDX_Control(pDX, IDC_PROXY_ADDRESS, m_ctrlProxyAddress);
	DDX_Control(pDX, IDC_SPINDELAY, m_ctrlSpinDelay);
	DDX_Text(pDX, IDC_PROGRESS_DELAY, m_fProgressDelay);
	DDX_Check(pDX, IDC_ENABLEERRORS, m_bEnableErrors);
	DDX_Check(pDX, IDC_FLOAT_PROGRESS, m_bFloatProgress);
	DDX_Check(pDX, IDC_CONFIRM_BATCH, m_bConfirmBatch);
	DDX_Radio(pDX, IDC_DEFAULT_PORT, m_iConnection);
	DDX_Text(pDX, IDC_INTERNET_PORT, m_uInternetPort);
	DDX_Text(pDX, IDC_PROXY_ADDRESS, m_strProxyAddress);
	DDX_Text(pDX, IDC_PROXY_PORT, m_uProxyPort);
	DDX_Check(pDX, IDC_SHOW_PAGE_NAVIGATION, m_bShowPageNavigation);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPPViewer::EnablePortControls()
//
// 	Description:	This function is called to enable/disable the appropriate
//					port assignment controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPViewer::EnablePortControls()
{
	UpdateData(TRUE);

	//	Which connection type is being used?
	switch(m_iConnection)
	{
		case TMXML_CONNECTION_ASSIGNED:

			m_ctrlInternetPort.EnableWindow(TRUE);
			m_ctrlProxyAddress.EnableWindow(FALSE);
			m_ctrlProxyAddressLabel.EnableWindow(FALSE);
			m_ctrlProxyPort.EnableWindow(FALSE);
			m_ctrlProxyPortLabel.EnableWindow(FALSE);
			break;

		case TMXML_CONNECTION_PROXY:

			m_ctrlInternetPort.EnableWindow(FALSE);
			m_ctrlProxyAddress.EnableWindow(TRUE);
			m_ctrlProxyAddressLabel.EnableWindow(TRUE);
			m_ctrlProxyPort.EnableWindow(TRUE);
			m_ctrlProxyPortLabel.EnableWindow(TRUE);
			break;

		case TMXML_CONNECTION_DEFAULT:
		default:

			m_ctrlInternetPort.EnableWindow(FALSE);
			m_ctrlProxyAddress.EnableWindow(FALSE);
			m_ctrlProxyAddressLabel.EnableWindow(FALSE);
			m_ctrlProxyPort.EnableWindow(FALSE);
			m_ctrlProxyPortLabel.EnableWindow(FALSE);
			break;
	}

}

//==============================================================================
//
// 	Function Name:	CPPViewer::OnAssignPort()
//
// 	Description:	This function is called when the user clicks on the Assign
//					Port radio button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPViewer::OnAssignPort() 
{
	EnablePortControls();
}

//==============================================================================
//
// 	Function Name:	CPPViewer::OnDefaultPort()
//
// 	Description:	This function is called when the user clicks on the Use
//					Default Port radio button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPViewer::OnDefaultPort() 
{
	EnablePortControls();
}

//==============================================================================
//
// 	Function Name:	CPPViewer::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPViewer::OnInitDialog() 
{
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	m_ctrlSpinDelay.SetRange(1, 60);

	EnablePortControls();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPPViewer::OnProxyServer()
//
// 	Description:	This function is called when the user clicks on the Use
//					Proxy Server radio button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPViewer::OnProxyServer() 
{
	EnablePortControls();
}


