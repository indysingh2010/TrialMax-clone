//==============================================================================
//
// File Name:	toolbars.cpp
//
// Description:	This file contains member functions of the CToolbars class.
//
// See Also:	toolbars.h
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
#include <toolbars.h>

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
BEGIN_MESSAGE_MAP(CToolbars, CDialog)
	//{{AFX_MSG_MAP(CToolbars)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CToolbars::CToolbars()
//
// 	Description:	This is the constructor for CToolbars objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CToolbars::CToolbars(CWnd* pParent)
		  :CDialog(CToolbars::IDD, pParent)
{
	//{{AFX_DATA_INIT(CToolbars)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	m_strIniFile.Empty();
}

//==============================================================================
//
// 	Function Name:	CToolbars::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and dialog box controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CToolbars::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CToolbars)
	DDX_Control(pDX, IDC_TMBARS, m_TMBars);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CToolbars::GetIniFile()
//
// 	Description:	This function is called to get the ini file to be used for
//					configuration.
//
// 	Returns:		A pointer to the ini filename
//
//	Notes:			None
//
//==============================================================================
LPCSTR CToolbars::GetIniFile()
{
	return m_strIniFile;
}

//==============================================================================
//
// 	Function Name:	CToolbars::OnInitDialog()
//
// 	Description:	This function is called to initialize the dialog box
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CToolbars::OnInitDialog() 
{
	//	Do the base class processing first
	CDialog::OnInitDialog();
	
	//	Initialize the TMBars control
	m_TMBars.SetIniFile(m_strIniFile);
	m_TMBars.Initialize();

	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CToolbars::OnOK()
//
// 	Description:	This function is called when the user clicks on OK
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CToolbars::OnOK() 
{
	//	Save the new configurations
	m_TMBars.Save();

	//	Close the dialog
	CDialog::OnOK();
}

//==============================================================================
//
// 	Function Name:	CToolbars::SetIniFile()
//
// 	Description:	This function is called to set the ini file to be used for
//					configuration.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CToolbars::SetIniFile(LPCSTR lpFilename)
{
	m_strIniFile = (lpFilename != 0) ? lpFilename : "";
}



