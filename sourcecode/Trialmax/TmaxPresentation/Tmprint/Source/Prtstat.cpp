//==============================================================================
//
// File Name:	prtstat.cpp
//
// Description:	This file contains member functions of the CPrintStatus class.
//
// See Also:	prtstat.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-15-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmprntap.h>
#include <prtstat.h>
#include <job.h>
#include <tmvdefs.h>
#include <tmppdefs.h>
#include <tmprdefs.h>

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
BEGIN_MESSAGE_MAP(CPrintStatus, CDialog)
	//{{AFX_MSG_MAP(CPrintStatus)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPrintStatus::CPrintStatus()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPrintStatus::CPrintStatus(CJob* pJob, int iPages, long lImages,
						   BOOL bPowerPoint, CWnd* pParent)
	         :CDialog(CPrintStatus::IDD, pParent)
{
	//{{AFX_DATA_INIT(CPrintStatus)
	m_strPages = _T("");
	m_strImages = _T("");
	//}}AFX_DATA_INIT

	m_pJob = pJob;
	m_iPages = iPages;
	m_lImages = lImages;
	m_iPage = 0;
	m_lImage = 0;
	m_bAbortJob = FALSE;
	m_bPowerPoint = bPowerPoint;

	//	Create the window now
	Create(CPrintStatus::IDD, pParent);
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::~CPrintStatus()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPrintStatus::~CPrintStatus()
{
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and the associated class members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintStatus::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPrintStatus)
	DDX_Control(pDX, IDC_IMAGES, m_ctrlImages);
	DDX_Control(pDX, IDC_PAGES, m_ctrlPages);
	DDX_Text(pDX, IDC_PAGES, m_strPages);
	DDX_Control(pDX, IDC_TMPOWER, m_TMPower);
	DDX_Control(pDX, IDC_TMVIEW, m_TMView);
	DDX_Text(pDX, IDC_IMAGES, m_strImages);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::Terminate()
//
// 	Description:	This function is called to terminate the printing and 
//					unload the viewers
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintStatus::Terminate()
{
	if(IsWindow(m_TMView.m_hWnd))
		m_TMView.LoadFile("", -1);
	if(IsWindow(m_TMPower.m_hWnd))
		m_TMPower.Close();
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::GetAbortJob()
//
// 	Description:	This function is called to check if the job should be
//					aborted
//
// 	Returns:		TRUE if job should be aborted
//
//	Notes:			None
//
//==============================================================================
BOOL CPrintStatus::GetAbortJob()
{
	return m_bAbortJob;
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::GetAspectRatio()
//
// 	Description:	This function is called to get the aspect ratio of the 
//					image currently loaded in the TMView control
//
// 	Returns:		The current aspect ratio
//
//	Notes:			None
//
//==============================================================================
float CPrintStatus::GetAspectRatio()
{
	//	Has the job been aborted?
	if(m_bAbortJob)
		return 0.0f;

	return m_TMView.GetSrcRatio(TMV_ACTIVEPANE);
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::LoadImage()
//
// 	Description:	This function is called to load the TMView control with the
//					requested image.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPrintStatus::LoadImage(LPCSTR lpFilename)
{
	//	Has the job been aborted?
	if(m_bAbortJob)
		return FALSE;

	//	Make sure the viewer is in single-pane mode
	if(m_TMView.GetSplitScreen() == TRUE)
		m_TMView.SetSplitScreen(FALSE);

	return (m_TMView.LoadFile(lpFilename, -1) == TMV_NOERROR);
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::LoadSlide()
//
// 	Description:	This function is called to load the TMPower control with the
//					requested presentation slide.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPrintStatus::LoadSlide(LPCSTR lpFilename, long lSlide, BOOL bUseId)
{
	//	Has the job been aborted?
	if(m_bAbortJob)
		return FALSE;

	//	Do we need to initialize the TMPower control?
	if((m_bPowerPoint == TRUE) && (m_TMPower.IsInitialized() == FALSE))
	{
		if(m_TMPower.Initialize() != 0)
			m_bPowerPoint = FALSE;
	}

	//	Can we access PowerPoint?
	if(m_bPowerPoint == FALSE)
		return FALSE;

	return (m_TMPower.LoadFile(lpFilename, lSlide, bUseId, -1) == TMPOWER_NOERROR);
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::LoadZap()
//
// 	Description:	This function is called to load the TMView control with the
//					requested zap file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPrintStatus::LoadZap(LPCSTR lpFilename, LPCSTR lpSourceImage,
						   LPCSTR lpSibling,  LPCSTR lpSiblingImage, long lFlags)
{
	BOOL	bSuccessful = FALSE;
	LPCSTR	lpTLFilename = NULL;
	LPCSTR	lpTLImage = NULL;
	LPCSTR	lpBRFilename = NULL;
	LPCSTR	lpBRImage = NULL;

	//	Has the job been aborted?
	if(m_bAbortJob)
		return FALSE;

	//	Are we loading a split-screen treatment?
	if((lpSibling != NULL) && (lstrlen(lpSibling) > 0))
	{
		//	Set the appropriate orientation
		if((lFlags & TMPRINT_SPLITZAP_HORIZONTAL) != 0)
		{
			if(m_TMView.GetSplitHorizontal() != TRUE)
				m_TMView.SetSplitHorizontal(TRUE); // Set to horizontal
		}
		else
		{
			if(m_TMView.GetSplitHorizontal() == TRUE)
				m_TMView.SetSplitHorizontal(FALSE); // Set to vertical
		}

		//	Make sure the viewer is in split screen mode
		if(m_TMView.GetSplitScreen() == FALSE)
			m_TMView.SetSplitScreen(TRUE);

		//	Should the primary treatment be loaded in the right pane?
		if((lFlags & TMPRINT_SPLITZAP_RIGHT) != 0)
		{
			lpTLFilename = lpSibling;
			lpTLImage = lpSiblingImage;
			lpBRFilename = lpFilename;
			lpBRImage = lpSourceImage;
		}
		else
		{
			lpTLFilename = lpFilename;
			lpTLImage = lpSourceImage;
			lpBRFilename = lpSibling;
			lpBRImage = lpSiblingImage;
		}

		//	Load the left pane then the right pane
		if(m_TMView.LoadZap(lpTLFilename, TRUE, TRUE, FALSE, TMV_LEFTPANE, lpTLImage) == TMV_NOERROR)
		{
			bSuccessful = (m_TMView.LoadZap(lpBRFilename, TRUE, TRUE, FALSE, TMV_RIGHTPANE, lpBRImage) == TMV_NOERROR);
		}
	}
	else
	{
		//	Make sure the viewer is in single-pane mode
		if(m_TMView.GetSplitScreen() == TRUE)
			m_TMView.SetSplitScreen(FALSE);

		//	Load the specified treatment in the active pane
		bSuccessful = (m_TMView.LoadZap(lpFilename, TRUE, TRUE, FALSE, -1, lpSourceImage) == TMV_NOERROR);
	}

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::OnCancel()
//
// 	Description:	This function is called when the user closes the dialog box
//					by clicking on Cancel or pressing escape
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintStatus::OnCancel()
{
	//	Set the abort flag
	m_bAbortJob = TRUE;

	//	Call the base class handler
	CDialog::OnCancel();
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::OnInitDialog()
//
// 	Description:	This function is called by the framework to initialize the
//					dialog box.
//
// 	Returns:		TRUE for default focus
//
//	Notes:			None
//
//==============================================================================
BOOL CPrintStatus::OnInitDialog() 
{
	//	Do the base class initialization
	CDialog::OnInitDialog();
	
	//	Initialize the page count
	m_strPages.Format("Page %d of %d", m_iPage, m_iPages);
	m_ctrlPages.SetWindowText(m_strPages);
	
	//	Initialize the image count
	m_strImages.Format("Image %ld of %ld", m_lImage, m_lImages);
	m_ctrlPages.SetWindowText(m_strImages);
	
	//	Set the TMView properties
	ASSERT(m_pJob);
	if(m_pJob)
	{
		m_TMView.SetPrintCallouts(m_pJob->GetPrintCallouts());
		m_TMView.SetPrintCalloutBorders(m_pJob->GetPrintCalloutBorders());
		m_TMView.SetPrintBorderColor((OLE_COLOR)m_pJob->GetPrintBorderColor());
		m_TMView.SetPrintBorderThickness(m_pJob->GetPrintBorderThickness());
		m_TMView.EnableDIBPrinting(m_pJob->GetEnableDIBPrinting() ? 1 : 0);
		m_TMView.SetCalloutFrameColor(m_pJob->GetCalloutFrameColor());
	}

	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::OnLoadViewer()
//
// 	Description:	This function is called by the printer when it loads and
//					activates one of the viewers.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintStatus::OnLoadViewer(BOOL bIsPowerPoint) 
{
	//	Has the operation been canceled?
	if(m_bAbortJob)
		return;

	//	Which viewer is being loaded?
	if(bIsPowerPoint)
	{
		//	Is the TMPower control visible?
		if(!m_TMPower.IsWindowVisible() && m_bPowerPoint)
		{
			//	Toggle the viewers
			m_TMPower.ShowWindow(SW_SHOW);
			m_TMPower.BringWindowToTop();
			m_TMView.ShowWindow(SW_HIDE);
		}
	}
	else
	{
		//	Is the TMView control visible?
		if(!m_TMView.IsWindowVisible())
		{
			//	Toggle the viewers
			m_TMView.ShowWindow(SW_SHOW);
			m_TMView.BringWindowToTop();
			m_TMPower.ShowWindow(SW_HIDE);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::PowerPointInitialized()
//
// 	Description:	This function is called to determine if the PowerPoint
//					viewer has been initialized
//
// 	Returns:		TRUE if initialized
//
//	Notes:			None
//
//==============================================================================
BOOL CPrintStatus::PowerPointInitialized()
{
	//	NOTE:	We use the local flag instead of the TMPower control because
	//			we don't bother initializing PowerPoint until we actually need
	//			if for the print job
	return m_bPowerPoint;
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::PrintEx()
//
// 	Description:	This function is called to print the current TMView image
//					into the dc provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintStatus::PrintEx(BOOL bFullImage, CDC* pdc, int iLeft, int iTop, 
						   int iWidth, int iHeight, BOOL bAutoRotate)
{
	HDC hdc;

	ASSERT(pdc);

	//	Has the operation been canceled?
	if(m_bAbortJob)
		return;

	if((hdc = pdc->GetSafeHdc()) != NULL)
		m_TMView.PrintEx((OLE_HANDLE)hdc, bFullImage, bAutoRotate,
						  iLeft, iTop, iWidth, iHeight, TMV_ACTIVEPANE);
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::SaveSlide()
//
// 	Description:	This function is called to save the current PowerPoint
//					slide to the specified file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPrintStatus::SaveSlide(LPCSTR lpFilename, int iFormat)
{
	//	Has the operation been canceled?
	if(m_bAbortJob)
		return FALSE;

	//	Do we need to initialize the TMPower control?
	if((m_bPowerPoint == TRUE) && (m_TMPower.IsInitialized() == FALSE))
	{
		if(m_TMPower.Initialize() != 0)
			m_bPowerPoint = FALSE;
	}

	//	Can we access PowerPoint?
	if(m_bPowerPoint == FALSE)
		return FALSE;

	//	Now save the current slide
	m_TMPower.SetSaveFormat(iFormat);

	return (m_TMPower.SaveSlide(lpFilename, -1) == TMPOWER_NOERROR);
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::SetAbortJob()
//
// 	Description:	This function is called to abort the job
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintStatus::SetAbortJob()
{
	m_bAbortJob = TRUE;
	
	CDialog::OnCancel();
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::SetImage()
//
// 	Description:	This function is called to set the current image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintStatus::SetImage(long lImage)
{
	//	Update the page indicator
	m_lImage = lImage;
	m_strImages.Format("Image %ld of %ld", m_lImage, m_lImages);
	m_ctrlImages.SetWindowText(m_strImages);
}

//==============================================================================
//
// 	Function Name:	CPrintStatus::SetPage()
//
// 	Description:	This function is called to set the current page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPrintStatus::SetPage(int iPage)
{
	//	Update the page indicator
	m_iPage = iPage;
	m_strPages.Format("Page %d of %d", m_iPage, m_iPages);
	m_ctrlPages.SetWindowText(m_strPages);
}

