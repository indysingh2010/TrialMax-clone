//==============================================================================
//
// File Name:	diagpg.cpp
//
// Description:	This file contains member functions of the CDiagnosticPage class.
//
// See Also:	graphpg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <diagpg.h>
#include <tmsetup.h>
#include <tmprint.h>
#include <tmview.h>
#include <tmstat.h>
#include <tmtext.h>
#include <tmtool.h>
#include <tmbars.h>
#include <tmlpen.h>
#include <tmmovie.h>
#include <tmpower.h>

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
extern CTMSetupCtrl*	theControl;
extern CString			theClsId;
extern CString			thePath;

extern const char* _AxClsIds[TMAX_AXCTRL_MAX];
extern const char* _AxNames[TMAX_AXCTRL_MAX];

//	Diagnostic descriptions
const char* _AxDiagnostic[TMAX_AXCTRL_MAX] = {	"Status Bar",
												"Scrolling Text",
												"Image Viewer",
												"Light Pen",
												"Toolbar",
												"Toolbar Setup",
												"Video Playback",
												"PowerPoint Viewer",
												"Formatted Printing",
												"Shared Memory",
												"Screen Capture",
												"Presentation Setup",
											   };

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CDiagnosticPage, CSetupPage)
	//{{AFX_MSG_MAP(CDiagnosticPage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CDiagnosticPage::Add()
//
// 	Description:	This function is called to add a row to the list box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDiagnosticPage::Add(CTMVersion& rVersion)
{
	CString strLocation;

	int	iIndex = m_ctrlList.GetItemCount();

	//	Insert the new row
	iIndex = m_ctrlList.InsertItem(iIndex, rVersion.GetName());

	//	Set the text in the remaining columns
	m_ctrlList.SetItemText(iIndex, DP_VERSION_COLUMN, rVersion.GetShortTextVer());
	m_ctrlList.SetItemText(iIndex, DP_BUILD_COLUMN, rVersion.GetBuildDate());
	m_ctrlList.SetItemText(iIndex, DP_DESCRIPTION_COLUMN, rVersion.GetDescription());
	m_ctrlList.SetItemText(iIndex, DP_PATH_COLUMN, rVersion.GetLocation(strLocation));
	m_ctrlList.SetItemText(iIndex, DP_CLSID_COLUMN, rVersion.GetClsId());
}

//==============================================================================
//
// 	Function Name:	CDiagnosticPage::CDiagnosticPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDiagnosticPage::CDiagnosticPage(CWnd* pParent) : CSetupPage(CDiagnosticPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDiagnosticPage)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CDiagnosticPage::~CDiagnosticPage()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDiagnosticPage::~CDiagnosticPage()
{
}

//==============================================================================
//
// 	Function Name:	CDiagnosticPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDiagnosticPage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDiagnosticPage)
	DDX_Control(pDX, IDC_LIST, m_ctrlList);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CDiagnosticPage::OnInitDialog()
//
// 	Description:	This function is called by the framework to initialize the
//					dialog box
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CDiagnosticPage::OnInitDialog() 
{
	CTMVersion tmVersion;

	//	Do the base class initialization
	CSetupPage::OnInitDialog();
	
	m_ctrlList.SetExtendedStyle(m_ctrlList.GetExtendedStyle() | LVS_EX_FULLROWSELECT);
	
	//	Insert the columns
	m_ctrlList.InsertColumn(DP_NAME_COLUMN, "Name", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("TmaxPresentation ") * 1.3f));

	m_ctrlList.InsertColumn(DP_VERSION_COLUMN, "Version", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("Version ") * 1.3f));
	
	m_ctrlList.InsertColumn(DP_BUILD_COLUMN, "Build", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("12-12-2004 ") * 1.3f));
	
	m_ctrlList.InsertColumn(DP_DESCRIPTION_COLUMN, "Description", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("Presentation Application ") * 1.3f));
	
	m_ctrlList.InsertColumn(DP_PATH_COLUMN, "Registered Path", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("c:\\program files\\fti\\trialmax 7\\tmaxPresentation.exe ") * 1.3f));

	m_ctrlList.InsertColumn(DP_CLSID_COLUMN, "CLSID", LVCFMT_LEFT, 
							(int)(m_ctrlList.GetStringWidth("{B581682E-5CC0-4E50-BBBC-582D78677E5A} ") * 1.3f));
	

	//	Add the TmaxPresentation application
	if(m_pControl != 0)
	{
		if(m_pControl->m_strPresentationFileSpec.GetLength() > 0)
		{
			tmVersion.InitFromFile("TmaxPresentation", "Presentation Application", m_pControl->m_strPresentationFileSpec);
			Add(tmVersion);
		}

	}

	//	Add each of the ActiveX controls
	for(int i = 0; i < TMAX_AXCTRL_MAX; i++)
	{
		tmVersion.InitFromClsId(_AxNames[i], _AxDiagnostic[i], _AxClsIds[i]);
		Add(tmVersion);
	}
	
	return TRUE;  
}	


