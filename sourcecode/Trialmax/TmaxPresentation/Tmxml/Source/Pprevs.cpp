//==============================================================================
//
// File Name:	pprevs.cpp
//
// Description:	This file contains member functions of the CPPRevisions class
//
// See Also:	pprevs.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	06-25-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <pprevs.h>
#include <tmxml.h>

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
extern CString theClsId;
extern CString thePath;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
IMPLEMENT_DYNCREATE(CPPRevisions, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPRevisions, CPropertyPage)
	//{{AFX_MSG_MAP(CPPRevisions)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPRevisions::Add()
//
// 	Description:	This function is called to add a row to the list box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPRevisions::Add(LPCSTR lpName, LPCSTR lpDescription, int iMajor,
					   int iMinor, long lBuild, LPCSTR lpClsid,LPCSTR lpPath)
{
	int		iIndex = m_ctrlList.GetItemCount();
	CString	strVersion;
	CString	strBuild;

	//	Format the version information
	strVersion.Format("%d.%d", iMajor, iMinor);
	strBuild.Format("%ld", lBuild);

	//	Insert the new row
	iIndex = m_ctrlList.InsertItem(iIndex, lpName);

	//	Set the text in the remaining columns
	m_ctrlList.SetItemText(iIndex, DIAGNOSTIC_DESCRIPTION_COLUMN, lpDescription);
	m_ctrlList.SetItemText(iIndex, DIAGNOSTIC_VERSION_COLUMN, strVersion);
	m_ctrlList.SetItemText(iIndex, DIAGNOSTIC_BUILD_COLUMN, strBuild);
	m_ctrlList.SetItemText(iIndex, DIAGNOSTIC_CLSID_COLUMN, lpClsid);
	m_ctrlList.SetItemText(iIndex, DIAGNOSTIC_PATH_COLUMN, lpPath);
}

//==============================================================================
//
// 	Function Name:	CPPRevisions::CPPRevisions()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPRevisions::CPPRevisions() : CPropertyPage(CPPRevisions::IDD)
{
	//{{AFX_DATA_INIT(CPPRevisions)
	//}}AFX_DATA_INIT

	m_pTMXmlCtrl = 0;
}

//==============================================================================
//
// 	Function Name:	CPPRevisions::~CPPRevisions()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPRevisions::~CPPRevisions()
{
}

//==============================================================================
//
// 	Function Name:	CPPRevisions::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPRevisions::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPRevisions)
	DDX_Control(pDX, IDC_LIST, m_ctrlList);
	DDX_Control(pDX, IDC_TMPOWER, m_TMPower);
	DDX_Control(pDX, IDC_TMPRINT, m_TMPrint);
	DDX_Control(pDX, IDC_TMTOOL, m_TMTool);
	DDX_Control(pDX, IDC_TMVIEW, m_TMView);
	DDX_Control(pDX, IDC_TMBROWSE, m_TMBrowse);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPPRevisions::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPRevisions::OnInitDialog() 
{
	CString	strClsId;
	CString	strPath;
	
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	m_ctrlList.SetExtendedStyle(m_ctrlList.GetExtendedStyle() | LVS_EX_FULLROWSELECT);
	
	//	Insert the columns
	m_ctrlList.InsertColumn(DIAGNOSTIC_NAME_COLUMN, "Name", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("TMMovie ") * 1.3f));

	m_ctrlList.InsertColumn(DIAGNOSTIC_DESCRIPTION_COLUMN, "Description", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("Application Setup ") * 1.3f));
	
	m_ctrlList.InsertColumn(DIAGNOSTIC_VERSION_COLUMN, "Version", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("Version ") * 1.3f));
	
	m_ctrlList.InsertColumn(DIAGNOSTIC_BUILD_COLUMN, "Build", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("88888 ") * 1.3f));
	
	m_ctrlList.InsertColumn(DIAGNOSTIC_CLSID_COLUMN, "CLSID", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("{B581682E-5CC0-4E50-BBBC-582D78677E5A} ") * 1.3f));
	
	m_ctrlList.InsertColumn(DIAGNOSTIC_PATH_COLUMN, "Registered Path", LVCFMT_LEFT, 1000);

	//	Add the TMXml (this) control
	if(m_pTMXmlCtrl != 0)
	{
		Add("TMXml", "TrialMax XML", 
			m_pTMXmlCtrl->GetVerMajor(), m_pTMXmlCtrl->GetVerMinor(), m_pTMXmlCtrl->GetVerBuild(), theClsId, thePath);
	}

	//	Add the TMView control
	strClsId = m_TMView.GetClassIdString();
	strPath  = m_TMView.GetRegisteredPath();
	Add("TMView", "Document Viewer", m_TMView.GetVerMajor(),
		m_TMView.GetVerMinor(), m_TMView.GetVerBuild(), strClsId, strPath);

	//	Add the TMTool control
	strClsId = m_TMTool.GetClassIdString();
	strPath  = m_TMTool.GetRegisteredPath();
	Add("TMTool", "TrialMax Toolbar", m_TMTool.GetVerMajor(),
		m_TMTool.GetVerMinor(),	m_TMTool.GetVerBuild(), strClsId, strPath);

	//	Add the TMPower control
	strClsId = m_TMPower.GetClassIdString();
	strPath  = m_TMPower.GetRegisteredPath();
	Add("TMPower", "PowerPoint Viewer", m_TMPower.GetVerMajor(),
		m_TMPower.GetVerMinor(), m_TMPower.GetVerBuild(), strClsId, strPath);

	//	Add the TMPrint control
	strClsId = m_TMPrint.GetClassIdString();
	strPath  = m_TMPrint.GetRegisteredPath();
	Add("TMPrint", "Formatted Printing", m_TMPrint.GetVerMajor(),
		m_TMPrint.GetVerMinor(), m_TMPrint.GetVerBuild(), strClsId, strPath);

	//	Add the TMBrowse control
	strClsId = m_TMBrowse.GetClassIdString();
	strPath  = m_TMBrowse.GetRegisteredPath();
	Add("TMBrowse", "Web Browser", m_TMBrowse.GetVerMajor(),
		m_TMBrowse.GetVerMinor(), m_TMBrowse.GetVerBuild(), strClsId, strPath);

	return TRUE;
}


