//==============================================================================
//
// File Name:	download.cpp
//
// Description:	This file contains member functions of the CDownload class.
//
// See Also:	download.h
//
//==============================================================================
//	Date		Revision    Description
//	06-05-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <download.h>
#include <shlwapi.h>
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

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CDownload, CDialog)
	//{{AFX_MSG_MAP(CDownload)
	ON_BN_CLICKED(ID_ABORT, OnAbort)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CDownload::CDownload()
//
// 	Description:	This is the constructor for CDownload objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDownload::CDownload(CXmlFrame* pFrame) : CDialog(CDownload::IDD, pFrame)
{
	//{{AFX_DATA_INIT(CDownload)
	m_strProgress = _T("");
	m_strMessage = _T("");
	//}}AFX_DATA_INIT

	m_pXmlFrame = pFrame;
	m_bAborted  = FALSE;

	//	Create the window
	Create(CDownload::IDD, m_pXmlFrame);
}

//==============================================================================
//
// 	Function Name:	CDownload::~CDownload()
//
// 	Description:	This is the destructor for CDownload objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDownload::~CDownload()
{
}

//==============================================================================
//
// 	Function Name:	CDownload::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDownload::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDownload)
	DDX_Control(pDX, IDC_PROGRESS_BAR, m_ctrlProgressBar);
	DDX_Text(pDX, IDC_PROGRESS, m_strProgress);
	DDX_Text(pDX, IDC_EDIT, m_strMessage);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CDownload::OnAbort()
//
// 	Description:	This function is called when the user clicks on abort
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDownload::OnAbort() 
{
	//	Set the abort flag
	m_bAborted = TRUE;

	//	Close the dialog
	CDialog::OnOK();	
}

//==============================================================================
//
// 	Function Name:	CDownload::OnInitDialog()
//
// 	Description:	This function traps all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CDownload::OnInitDialog() 
{
	//	Do the base class processing
	CDialog::OnInitDialog();
	
	//	Initialize the progress bar
	m_ctrlProgressBar.SetTextFormat("%d%%", PBS_SHOW_PERCENT);
	m_ctrlProgressBar.SetBkColor(::GetSysColor(COLOR_MENU));
	m_ctrlProgressBar.SetGradientColorsEx(3, ::GetSysColor(COLOR_WINDOW), 
											 ::GetSysColor(COLOR_ACTIVECAPTION),
											 ::GetSysColor(COLOR_WINDOW));
	m_ctrlProgressBar.SetRange(0, 100);
	m_ctrlProgressBar.SetPos(0);

	//	Center the dialog box within the frame window
	CenterWindow(m_pXmlFrame);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDownload::Update()
//
// 	Description:	This function will update the dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDownload::Update(ULONG ulProgress, ULONG ulMaximum, LPCSTR lpszStatus)
{	
	char	szProgress[256];
	char	szMaximum[256];

	//	Don't bother if the window is not valid
	if(!IsWindow(m_hWnd) || !IsWindowVisible())
		return;

	if(lpszStatus != 0)
	{
		m_strMessage = lpszStatus;
    }
    else
    {
        m_strMessage.Empty();
    }

    StrFormatByteSize(ulProgress, szProgress, sizeof(szProgress));
    if(ulMaximum > 0)
    {
		StrFormatByteSize(ulMaximum, szMaximum, sizeof(szMaximum));
        m_strProgress.Format("Downloaded %s of %s", szProgress, szMaximum);

		//	Update the progress bar
		m_ctrlProgressBar.SetPos((int)(100.0 * ulProgress / ulMaximum));
    }
    else
    {
        m_strProgress.Format("Downloaded %s (total size unknown)", szProgress);
    }

	UpdateData(FALSE);
}


