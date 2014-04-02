//==============================================================================
//
// File Name:	printpro.cpp
//
// Description:	This file contains member functions of the CPrintProgress class.
//
// See Also:	printpro.h
//
//==============================================================================
//	Date		Revision    Description
//	06-30-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <printpro.h>
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
BEGIN_MESSAGE_MAP(CPrintProgress, CDialog)
	//{{AFX_MSG_MAP(CPrintProgress)
	ON_WM_SIZE()
	ON_BN_CLICKED(ID_ABORT, OnAbort)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPrintProgress::CPrintProgress()
//
// 	Description:	This is the constructor for CPrintProgress objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPrintProgress::CPrintProgress(CXmlFrame* pXmlFrame)
	           :CDialog(CPrintProgress::IDD, (CWnd*)pXmlFrame)
{
	//{{AFX_DATA_INIT(CPrintProgress)
	m_strBytes = _T("");
	m_strJob = _T("");
	m_strStatus = _T("");
	//}}AFX_DATA_INIT

	m_pXmlFrame = pXmlFrame;
	m_lPages = 0;
	m_strName.Empty();

	//	Create the window
	Create(CPrintProgress::IDD, m_pXmlFrame);
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintProgress::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPrintProgress)
	DDX_Control(pDX, IDC_DOWNLOAD_STATUS, m_ctrlStatus);
	DDX_Control(pDX, IDC_DOWNLOAD_JOB, m_ctrlJob);
	DDX_Control(pDX, IDC_PROGRESS_BAR, m_ctrlBar);
	DDX_Control(pDX, IDC_DOWNLOAD_BYTES, m_ctrlBytes);
	DDX_Control(pDX, ID_ABORT, m_ctrlAbort);
	DDX_Text(pDX, IDC_DOWNLOAD_BYTES, m_strBytes);
	DDX_Text(pDX, IDC_DOWNLOAD_JOB, m_strJob);
	DDX_Text(pDX, IDC_DOWNLOAD_STATUS, m_strStatus);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::Finish()
//
// 	Description:	This function is called to when the job is complete
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintProgress::Finish() 
{
	//	Update the job information
	m_strJob.Format("%s - finished", m_strName);
	
	//	Clear the status information
	m_strStatus.Empty();
	m_strBytes.Empty();

	UpdateData(FALSE);

	//	Update the progress bar
	m_ctrlBar.SetPos(m_lPages);
	
	//	Create a slight delay so the user can see the change
	Sleep(1000);
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::GetHeight()
//
// 	Description:	This function is called to retrieve the height of the dialog
//
// 	Returns:		The height in pixels
//
//	Notes:			None
//
//==============================================================================
int CPrintProgress::GetHeight()
{
	RECT rcWnd;

	//	Get the client rectangle
	GetClientRect(&rcWnd);

	return (rcWnd.bottom - rcWnd.top);
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::OnAbort()
//
// 	Description:	This function is called when the user clicks on Abort
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintProgress::OnAbort() 
{
	if(m_pXmlFrame)
		m_pXmlFrame->OnPrintAbort();	
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::OnInitDialog()
//
// 	Description:	This function traps the WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPrintProgress::OnInitDialog() 
{
	//	Do the base class initialization first
	CDialog::OnInitDialog();
	
	//	Set the initial format of the progress bar text
	m_ctrlBar.SetTextFormat("%d%%", PBS_SHOW_PERCENT);
	m_ctrlBar.SetBkColor(::GetSysColor(COLOR_WINDOW));
	m_ctrlBar.SetGradientColorsEx(3, ::GetSysColor(COLOR_WINDOW), 
									 ::GetSysColor(COLOR_ACTIVECAPTION),
									 ::GetSysColor(COLOR_WINDOW));
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::OnSize()
//
// 	Description:	This function traps all WM_SIZE messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintProgress::OnSize(UINT nType, int cx, int cy) 
{
	RECT rcJob;
	RECT rcBar;
	RECT rcBytes;
	RECT rcStatus;
	RECT rcAbort;
	int  iWidth;

	//	Do the base class processing
	CDialog::OnSize(nType, cx, cy);

	//	Don't bother if the controls haven't been created yet
	if(IsWindow(m_ctrlAbort.m_hWnd) == FALSE)
		return;

	//	Set the position of the Abort button
	m_ctrlAbort.GetWindowRect(&rcAbort);
	ScreenToClient(&rcAbort);
	iWidth = rcAbort.right - rcAbort.left;
	rcAbort.right = cx - 4;
	rcAbort.left = rcAbort.right - iWidth;
	m_ctrlAbort.MoveWindow(&rcAbort);

	//	Set the position of the Job control
	m_ctrlJob.GetWindowRect(&rcJob);
	ScreenToClient(&rcJob);
	rcJob.right = rcAbort.right;
	m_ctrlJob.MoveWindow(&rcJob);

	//	Set the position of the download status control
	m_ctrlStatus.GetWindowRect(&rcStatus);
	ScreenToClient(&rcStatus);
	rcStatus.right = rcAbort.right;
	m_ctrlStatus.MoveWindow(&rcStatus);

	//	Set the position of the download bytes control
	m_ctrlBytes.GetWindowRect(&rcBytes);
	ScreenToClient(&rcBytes);
	rcBytes.right = rcAbort.left - 4;
	m_ctrlBytes.MoveWindow(&rcBytes);

	//	Set the position of the progress bar
	m_ctrlBar.GetWindowRect(&rcBar);
	ScreenToClient(&rcBar);
	rcBar.right = rcAbort.left - 4;
	m_ctrlBar.MoveWindow(&rcBar);

}

//==============================================================================
//
// 	Function Name:	CPrintProgress::SetFilename()
//
// 	Description:	This function is called to set the filename information when
//					the file resides on a local drive instead of a remote drive.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintProgress::SetFilename(LPCSTR lpFilename) 
{
	if((lpFilename != 0) && (lstrlen(lpFilename) > 0))
	{
		m_strStatus.Format("Printing %s", lpFilename);
		m_strBytes.Empty();
	}
	else
	{
		m_strStatus.Empty();
		m_strBytes.Empty();
	}

	UpdateData(FALSE);
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::SetPage()
//
// 	Description:	This function is called to set the page information
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintProgress::SetPage(long lPage) 
{
	//	Set the range of the progress bar
	if(IsWindow(m_ctrlBar.m_hWnd))
		m_ctrlBar.SetPos(lPage);
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::Start()
//
// 	Description:	This function is called to initialize a new job
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintProgress::Start(LPCSTR lpName, long lPages) 
{
	m_lPages  = lPages;
	m_strName = lpName;

	//	Update the job information
	if(lpName)
		m_strJob.Format("%s - %ld pages", lpName, lPages);
	else
		m_strJob.Empty();

	//	Clear the status information
	m_strStatus.Empty();
	m_strBytes.Empty();

	//	Update the controls
	UpdateData(FALSE);

	//	Set the range of the progress bar
	if(IsWindow(m_ctrlBar.m_hWnd))
	{
		m_ctrlBar.SetRange(0, (int)lPages);
		m_ctrlBar.SetPos(0);
	}
}

//==============================================================================
//
// 	Function Name:	CPrintProgress::SetProgress()
//
// 	Description:	This function will update the download progress controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintProgress::SetProgress(ULONG ulProgress, ULONG ulMaximum, LPCSTR lpszStatus)
{	
	char	szProgress[256];
	char	szMaximum[256];

	//	Don't bother if the window is not valid
	if(!IsWindow(m_hWnd) || !IsWindowVisible())
		return;

	if(lpszStatus != 0)
	{
		m_strStatus = lpszStatus;
    }
    else
    {
        m_strStatus.Empty();
    }

    StrFormatByteSize(ulProgress, szProgress, sizeof(szProgress));
    if(ulMaximum > 0)
    {
		StrFormatByteSize(ulMaximum, szMaximum, sizeof(szMaximum));
        m_strBytes.Format("Downloaded %s of %s", szProgress, szMaximum);
    }
    else
    {
        m_strBytes.Format("Downloaded %s (total size unknown)", szProgress);
    }
	
	UpdateData(FALSE);
}

