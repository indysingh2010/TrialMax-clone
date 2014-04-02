//==============================================================================
//
// File Name:	fprintercaps.cpp
//
// Description:	This file contains the implementation of the CFPrinterCaps class
//
// See Also:	fprintercaps.h
//
// Copyright FTI Consulting - All Rights Reserved
//
//==============================================================================
//	Date		Revision    Description
//	06-15-2006	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <fprintercaps.h>

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
BEGIN_MESSAGE_MAP(CFPrinterCaps, CDialog)
	//{{AFX_MSG_MAP(CFPrinterCaps)
	ON_LBN_SELCHANGE(IDC_PRINTERS, OnPrinterChanged)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//------------------------------------------------------------------------------
//
// 	Function Name:	CFPrinterCaps::CFPrinterCaps()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CFPrinterCaps::CFPrinterCaps(CWnd* pParent) : CDialog(CFPrinterCaps::IDD, pParent)
{
	//{{AFX_DATA_INIT(CFPrinterCaps)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CFPrinterCaps::DoDataExchange()
//
//	Parameters:		pDX - MFC data exchange interface
//
// 	Return Value:	None
//
// 	Description:	Manages the exchange between child controls and their 
//					associated class members
//
//------------------------------------------------------------------------------
void CFPrinterCaps::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CFPrinterCaps)
	DDX_Control(pDX, IDC_DEVMODE, m_ctrlDevMode);
	DDX_Control(pDX, IDC_DEVICE_CAPS, m_ctrlDeviceCaps);
	DDX_Control(pDX, IDC_PRINTERS, m_ctrlPrinters);
	//}}AFX_DATA_MAP
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CFPrinterCaps::FillPrinters()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Called to populate the list box of printers
//
//------------------------------------------------------------------------------
void CFPrinterCaps::FillPrinters()
{
	CObList*	paPrinters = NULL;
	POSITION	Pos = NULL;
	CString*	pPrinter = NULL;

	m_ctrlPrinters.ResetContent();

	if((paPrinters = m_tmPrinter.EnumPrinters()) != NULL)
	{
		Pos = paPrinters->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pPrinter = (CString*)(paPrinters->GetNext(Pos))) != NULL)
			{
				m_ctrlPrinters.AddString(*pPrinter);
				delete pPrinter;
			}

		}

		paPrinters->RemoveAll();
		delete paPrinters;

	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CFPrinterCaps::OnInitDialog()
//
//	Parameters:		None
//
// 	Return Value:	TRUE for default focus assignment
//
// 	Description:	Called to handle the WM_INITDIALOG message
//
//------------------------------------------------------------------------------
BOOL CFPrinterCaps::OnInitDialog() 
{
	//	Do the base class initialization
	CDialog::OnInitDialog();
	
	//	Populate the printers list
	FillPrinters();

	//	Initialize the device caps control
	m_ctrlDeviceCaps.Initialize();
	m_ctrlDevMode.Initialize();

	//	Set the initial selection
	if(m_ctrlPrinters.GetCount() > 0)
		m_ctrlPrinters.SetCurSel(0);
	OnPrinterChanged();
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CFPrinterCaps::OnPrinterChanged()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Called when the user selects a new printer in the list
//
//------------------------------------------------------------------------------
void CFPrinterCaps::OnPrinterChanged() 
{
	int		iIndex = -1;
	CDC*	pdc = NULL;
	HGLOBAL	hDevMode = NULL;
	CString	strPrinter = "";

	if((iIndex = m_ctrlPrinters.GetCurSel()) >= 0)
		m_ctrlPrinters.GetText(iIndex, strPrinter);
	if(strPrinter.GetLength() > 0)
	{
		pdc = m_tmPrinter.GetNamedDC(strPrinter);
		hDevMode = m_tmPrinter.GetNamedDevMode(strPrinter);
	}

	m_ctrlDeviceCaps.Fill(pdc);	
	m_ctrlDevMode.Fill(hDevMode);
}
