//==============================================================================
//
// File Name:	ppprint.cpp
//
// Description:	This file contains member functions of the CPPPrint class
//
// See Also:	ppprint.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	07-03-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <ppprint.h>

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
IMPLEMENT_DYNCREATE(CPPPrint, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPPrint, CPropertyPage)
	//{{AFX_MSG_MAP(CPPPrint)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CPPPrint, CPropertyPage)
    //{{AFX_EVENTSINK_MAP(CPPPrint)
	ON_EVENT(CPPPrint, IDC_TMPRINT, 3 /* FirstPrinter */, OnFirstPrinter, VTS_BSTR)
	ON_EVENT(CPPPrint, IDC_TMPRINT, 4 /* NextPrinter */, OnNextPrinter, VTS_BSTR)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

//==============================================================================
//
// 	Function Name:	CPPPrint::CPPPrint()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPPrint::CPPPrint() : CPropertyPage(CPPPrint::IDD)
{
	//{{AFX_DATA_INIT(CPPPrint)
	m_strPrinter = _T("");
	m_bCurrentSession = FALSE;
	m_strTemplate = _T("");
	m_bCollate = FALSE;
	m_iCopies = 0;
	m_bCombinePrintPages = FALSE;
	m_bMinimizeColorDepth = FALSE;
	//}}AFX_DATA_INIT

	m_pTemplates = 0;
}

//==============================================================================
//
// 	Function Name:	CPPPrint::~CPPPrint()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPPrint::~CPPPrint()
{
}

//==============================================================================
//
// 	Function Name:	CPPPrint::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPPrint::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPPrint)
	DDX_Control(pDX, IDC_TEMPLATES, m_ctrlTemplates);
	DDX_Control(pDX, IDC_PRINTERS, m_ctrlPrinters);
	DDX_Control(pDX, IDC_TMPRINT, m_TMPrint);
	DDX_LBString(pDX, IDC_PRINTERS, m_strPrinter);
	DDX_Check(pDX, IDC_CURRENT_SESSION, m_bCurrentSession);
	DDX_LBString(pDX, IDC_TEMPLATES, m_strTemplate);
	DDX_Check(pDX, IDC_COLLATE, m_bCollate);
	DDX_Text(pDX, IDC_COPIES, m_iCopies);
	DDV_MinMaxInt(pDX, m_iCopies, 1, 100);
	DDX_Check(pDX, IDC_COMBINE_PRINT_PAGES, m_bCombinePrintPages);
	DDX_Check(pDX, IDC_MINIMIZE_COLOR_DEPTH, m_bMinimizeColorDepth);
	//}}AFX_DATA_MAP

}

//==============================================================================
//
// 	Function Name:	CPPPrint::FillTemplates()
//
// 	Description:	This function will fill the list of batch printing templates
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPPrint::FillTemplates()
{
	CTemplate*	pTemplate;
	int			iIndex;

	//	Flush the existing list
	m_ctrlTemplates.ResetContent();

	//	Add the description of each template
	if(m_pTemplates)
	{
		pTemplate = m_pTemplates->GetFirstTemplate();
		while(pTemplate)
		{
			if((iIndex = m_ctrlTemplates.AddString(pTemplate->m_strDescription)) != LB_ERR)
				m_ctrlTemplates.SetItemData(iIndex, (DWORD)pTemplate);
			pTemplate = m_pTemplates->GetNextTemplate();
		}
	}

	//	Set the current selection
	iIndex = LB_ERR;
	if(m_strTemplate.GetLength() > 0)
		iIndex = m_ctrlTemplates.FindStringExact(-1, m_strTemplate);
	if(iIndex >= 0)
		m_ctrlTemplates.SetCurSel(iIndex);
	else
		m_ctrlTemplates.SetCurSel(0);
}

//==============================================================================
//
// 	Function Name:	CPPPrint::OnFirstPrinter()
//
// 	Description:	This function handles events fired by the TMPrint control
//					when it starts enumerating the system printers.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPPrint::OnFirstPrinter(LPCTSTR lpszPrinter) 
{
	//	Add this string to the list box
	m_ctrlPrinters.AddString(lpszPrinter);
}

//==============================================================================
//
// 	Function Name:	CPPPrint::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPPrint::OnInitDialog() 
{
	int iIndex = LB_ERR;

	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	//	Ask the TMPrint control to enumerate the system printers
	m_TMPrint.EnumeratePrinters();

	//	Do we have any registered printers?
	if(m_ctrlPrinters.GetCount() > 0)
	{
		//	Did the caller provide a printer name?
		if(m_strPrinter.GetLength() > 0)
			iIndex = m_ctrlPrinters.FindStringExact(-1, m_strPrinter);

		//	Set the initial selection
		if(iIndex != LB_ERR)
			m_ctrlPrinters.SetCurSel(iIndex);
		else
			m_ctrlPrinters.SetCurSel(0);
	}

	//	Fill the templates list
	FillTemplates();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPPPrint::OnNextPrinter()
//
// 	Description:	This function handles events fired by the TMPrint control
//					as it enumerates the system printers.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPPrint::OnNextPrinter(LPCTSTR lpszPrinter) 
{
	//	Add this string to the list box
	m_ctrlPrinters.AddString(lpszPrinter);
}



