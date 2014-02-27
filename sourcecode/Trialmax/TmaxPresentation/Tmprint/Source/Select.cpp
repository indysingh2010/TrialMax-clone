//==============================================================================
//
// File Name:	select.cpp
//
// Description:	This file contains member functions of the CSelect class.
//
// See Also:	select.h
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	04-27-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <select.h>
#include <options.h>

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
BEGIN_MESSAGE_MAP(CSelect, CDialog)
	//{{AFX_MSG_MAP(CSelect)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CSelect::CSelect()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSelect::CSelect(COptions* pOptions) : CDialog(CSelect::IDD, pOptions)
{
	//{{AFX_DATA_INIT(CSelect)
	m_bCollate = FALSE;
	m_iCopies = 0;
	m_strPrinter = _T("");
	//}}AFX_DATA_INIT

	ASSERT(pOptions);

	m_pOptions = pOptions;
}

//==============================================================================
//
// 	Function Name:	CSelect::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelect::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSelect)
	DDX_Control(pDX, IDC_PRINTERS, m_ctrlPrinters);
	DDX_Check(pDX, IDC_COLLATE, m_bCollate);
	DDX_Text(pDX, IDC_COPIES, m_iCopies);
	DDX_LBString(pDX, IDC_PRINTERS, m_strPrinter);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CSelect::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CSelect::OnInitDialog() 
{
	CString strPrinter;

	//	Do the base class initialization first
	CDialog::OnInitDialog();
	
	//	Fill the printer list box
	for(int i = 0; i < m_pOptions->m_ctrlPrinters.GetCount(); i++)
	{
		m_pOptions->m_ctrlPrinters.GetLBText(i, strPrinter);
		m_ctrlPrinters.AddString(strPrinter);
	}
	
	//	Update the controls
	UpdateData(FALSE);
		
	return TRUE; 
}
