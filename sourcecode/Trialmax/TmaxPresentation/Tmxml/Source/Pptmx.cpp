//==============================================================================
//
// File Name:	pptmx.cpp
//
// Description:	This file contains member functions of the CPPTmx class
//
// See Also:	pptmx.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-20-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <pptmx.h>
#include <wrapxml.h>

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
IMPLEMENT_DYNCREATE(CPPTmx, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPTmx, CPropertyPage)
	//{{AFX_MSG_MAP(CPPTmx)
	ON_BN_CLICKED(IDC_SAVE_AS, OnSaveAs)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPTmx::CPPTmx()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPTmx::CPPTmx() : CPropertyPage(CPPTmx::IDD)
{
	//{{AFX_DATA_INIT(CPPTmx)
	m_strFilename = _T("");
	m_strSource = _T("");
	//}}AFX_DATA_INIT

	m_pXmlDocument = 0;
}

//==============================================================================
//
// 	Function Name:	CPPTmx::~CPPTmx()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPTmx::~CPPTmx()
{
}

//==============================================================================
//
// 	Function Name:	CPPTmx::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTmx::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPTmx)
	DDX_Control(pDX, IDC_SAVE_AS, m_ctrlSaveAs);
	DDX_Control(pDX, IDC_TMX_XML, m_ctrlXml);
	DDX_Text(pDX, IDC_TMX_FILENAME, m_strFilename);
	DDX_Text(pDX, IDC_TMX_SOURCE, m_strSource);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPPTmx::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPTmx::OnInitDialog() 
{
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	//	Do we have an XML document?
	if(m_pXmlDocument)
	{
		m_ctrlXml.SetWindowText(m_pXmlDocument->GetXml());
		m_ctrlSaveAs.EnableWindow(TRUE);
	}
	else
	{
		m_ctrlSaveAs.EnableWindow(FALSE);
	}
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPPTmx::OnSaveAs()
//
// 	Description:	This function is called when the user clicks on Save As
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTmx::OnSaveAs() 
{
	CFileDialog	FileDlg(FALSE);
	char		szFilter[] = "Tmx Files (*.tmx)\0*.tmx\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	ASSERT(m_pXmlDocument);
	if(m_pXmlDocument == 0)
		return;

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_LONGNAMES | OFN_NONETWORKBUTTON;
	if(FileDlg.DoModal() == IDOK)
	{
		FILE* fptr = NULL;
		fopen_s(&fptr, szFilename, "wt");

		if(fptr != NULL)
		{
			fprintf(fptr, "%s", m_pXmlDocument->GetXml());
			fclose(fptr);
		}
		else
		{
			CString strError;
			strError.Format("Unable to open %s", szFilename);
			MessageBox(strError, "Error", MB_ICONEXCLAMATION | MB_OK);
		}
	}
}
