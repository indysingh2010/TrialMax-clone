//==============================================================================
//
// File Name:	setup.cpp
//
// Description:	This file contains member functions of the CSetup class.
//
// See Also:	setup.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	02-27-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <app.h>
#include <setup.h>

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
extern CApp theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CSetup, CDialog)
	//{{AFX_MSG_MAP(CSetup)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CSetup::CSetup()
//
// 	Description:	This is the constructor for CSetup objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSetup::CSetup(CWnd* pParent) : CDialog(CSetup::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSetup)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	m_strIniFile.Empty();
	m_strPresentationFileSpec.Empty();
	m_strFilters.Empty();
	m_lpMediaControl = 0;
}

//==============================================================================
//
// 	Function Name:	CSetup::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and dialog box controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetup::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSetup)
	DDX_Control(pDX, IDC_TMSETUP, m_TMSetup);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CSetup::OnInitDialog()
//
// 	Description:	This function is called to initialize the dialog box
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CSetup::OnInitDialog() 
{
	//	Do the base class processing first
	CDialog::OnInitDialog();
	
	//	Initialize the TMBars control
	m_TMSetup.SetIniFile(m_strIniFile);
	m_TMSetup.SetActiveFilters(m_strFilters, m_lpMediaControl);
	m_TMSetup.SetPresentationFileSpec(m_strPresentationFileSpec);

	m_TMSetup.Initialize();

	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CSetup::OnOK()
//
// 	Description:	This function is called when the user clicks on OK
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetup::OnOK() 
{
	//	Save the new configurations
	m_TMSetup.Save();

	//	Close the dialog
	CDialog::OnOK();
}

//==============================================================================
//
// 	Function Name:	CSetup::SetActiveFilters()
//
// 	Description:	This function is called to set the active filter information
//					displayed on the DirectX page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetup::SetActiveFilters(LPCSTR lpFilters, LPUNKNOWN lpMediaControl)
{
	m_strFilters = (lpFilters != 0) ? lpFilters : "";
	m_lpMediaControl = lpMediaControl;
}

//==============================================================================
//
// 	Function Name:	CSetup::SetIniFile()
//
// 	Description:	This function is called to set the ini file to be used for
//					configuration.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetup::SetIniFile(LPCSTR lpFilename)
{
	m_strIniFile = (lpFilename != 0) ? lpFilename : "";
}

//==============================================================================
//
// 	Function Name:	CSetup::SetPresentationFileSpec()
//
// 	Description:	This function is called to set the path to the 
//					TmaxPresentation executable.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetup::SetPresentationFileSpec(LPCSTR lpszFileSpec)
{
	m_strPresentationFileSpec = (lpszFileSpec != 0) ? lpszFileSpec : "";
}


