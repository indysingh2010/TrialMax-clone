//==============================================================================
//
// File Name:	powerpt.cpp
//
// Description:	This file contains member functions of the CPowerPoint class.
//
// See Also:	powerpt.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-26-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <powerpt.h>
#include <tmpower.h>
#include <tmppdefs.h>
#include <resource.h>
#include <snapshot.h>
#include <toolbox.h>

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
BEGIN_MESSAGE_MAP(CPowerPoint, CDialog)
	//{{AFX_MSG_MAP(CPowerPoint)
	ON_WM_CTLCOLOR()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	EnumViewWindows()
//
// 	Description:	This callback is used to enumerate the view windows
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CALLBACK EnumViewWindows(HWND hWnd, LPARAM lpParam)
{
	CPowerPoint* pPP = (CPowerPoint*)lpParam;

	if(pPP)
		if(pPP->m_fPPVersion >= PpVersion::ppVersion2010)
		return pPP->MyEnumDesktopWindows(hWnd);
		else
		return pPP->OnEnumWindow(hWnd);
	else
		return FALSE;
};

//==============================================================================
//
// 	Function Name:	CPowerPoint::AddPresentation()
//
// 	Description:	This function is called to add a presentation to the active
//					list of presentations
//
// 	Returns:		A pointer to the dispatch interface for the new presentation
//
//	Notes:			None
//
//==============================================================================
_Presentation* CPowerPoint::AddPresentation() 
{
	LPDISPATCH		lpDispatch;
	_Presentation*	pIPresentation;

	//	Do we have the presentation list?
	if(m_pIPresentations == 0)
		return 0;
	
	//	Add a presentation
	if((lpDispatch = m_pIPresentations->Add(0)) != 0)
	{
		pIPresentation = new _Presentation(lpDispatch);
		ASSERT(pIPresentation->m_lpDispatch);

		return pIPresentation;
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::AssignNew()
//
// 	Description:	This function will assign all the new dispatch interfaces
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::AssignNew(long lStart) 
{
	//	View may have gotten destroyed between timer events
	if(m_pNIView == 0) return;

	//	Prevent access while we make the assignment
	Lock();

	//	Set the show to the correct slide while it's still off screen
	if(lStart > 1)
		SetInitialSlide(lStart);
			
	//	Reassign the parent for the new slide show window and size it properly
	AttachWnd(m_hNSSWnd);

	//	Close all active dispatch interfaces
	ReleaseActive();
	
	//	Update the window handles
	m_hSSWnd = m_hNSSWnd;
	m_hSSParent = m_hNSSParent;
	m_hNSSWnd = 0;
	m_hNSSParent = 0;

	m_pIView = m_pNIView;
	m_pIWindow = m_pNIWindow;
	m_pISettings = m_pNISettings;
	m_pISlides = m_pNISlides;
	m_pIPresentation = m_pNIPresentation;
	m_pIPresentations = m_pNIPresentations;

	m_pNIView = 0;
	m_pNIWindow = 0;
	m_pNISettings = 0;
	m_pNISlides = 0;
	m_pNIPresentation = 0;
	m_pNIPresentations = 0;

	if(m_fPPVersion >= PpVersion::ppVersion2010)
	{
		m_pIView->GotoSlide(lStart, PP_TRUE);
	}
	//	Make sure the view is visible
	::ShowWindow(m_hSSWnd, SW_SHOW);
	::BringWindowToTop(m_hSSWnd);
	
	//	Delete the snapshot
	if(m_pSnapshot)
	{
		m_pSnapshot->ShowWindow(SW_HIDE);
		delete m_pSnapshot;
		m_pSnapshot = 0;
	}

	//	Make sure the task bar gets restored
	//ShowTaskBar();

	//	Notify the control that we have a new presentation
	m_strFilename = m_strNIFilename;
	if(m_pControl) m_pControl->OnPPFileChange(m_lId, m_strFilename);

	//	Allow access
	//
	//	NOTE:	We have to do this here because SetCurrent() locks access
	Unlock();

	//	Make sure we have the correct slide information
	SetCurrent();

	//	Start the focus timer
	StartTimers();

}

//==============================================================================
//
// 	Function Name:	CPowerPoint::AttachWnd()
//
// 	Description:	This function will make the specified window a child of
//					this pane.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::AttachWnd(HWND hWnd) 
{
	CWnd* pWnd;

	//	Reassign the parent and size the window
	if((pWnd = CWnd::FromHandle(hWnd)) != 0)
	{
		pWnd->ShowWindow(SW_HIDE);
		pWnd->MoveWindow(&m_rcMax,FALSE);
		pWnd->SetParent(this);
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::CheckWnd()
//
// 	Description:	This function is called to verify that the slide show
//					window is properly positioned
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::CheckWnd() 
{
	RECT	rcWnd;
	BOOL	bResize = FALSE;
	BOOL	bNotify = FALSE;

	//	We only have to check for this condition if using PowerPoint 2K
	if((m_fPPVersion < 9.0) || (m_fPPVersion >= 10.0)) return;

	//	Do we have a slide show view ?
	if((m_pIView == 0) || !IsWindow(m_hSSWnd)) return;

	//	Do we have to check for the error?
	if(!m_bCheckWndError)
	{
		//	Get the current size and position
		::GetWindowRect(m_hSSWnd, &rcWnd);
		ScreenToClient(&rcWnd);

		//	Allow a small tolerance
		if(abs(rcWnd.left) > 1)
			m_bCheckWndError = TRUE;
		else if(abs(rcWnd.top) > 1)
			m_bCheckWndError = TRUE;
		else if(abs(rcWnd.right - m_rcMax.right) > 1)
			m_bCheckWndError = TRUE;
		else if(abs(rcWnd.bottom - m_rcMax.bottom) > 1)
			m_bCheckWndError = TRUE;

		//	Notify the user
		if(m_bCheckWndError)
		{
			//	Postpone notification until we resize the window
			bNotify = TRUE;
		}
		else
		{
			//	Don't bother going any further if no error encountered
			return;
		}
	}

	//	Resize the window to use the full client area
	GetClientRect(&rcWnd);
	::MoveWindow(m_hSSWnd, 0, 0, rcWnd.right, rcWnd.bottom, TRUE);

	//	Should we notify the user?
	if(bNotify)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_SHOWSCROLLBAR, m_strFilename);
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::CopySlide()
//
// 	Description:	This function is called to copy the current slide to the
//					clipboard
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::CopySlide() 
{
	LPDISPATCH	lpDispatch;
	_Slide*		pISlide;

	//	Do we have a slide show view?
	if(m_pIView == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}

	//	Get the current slide
	if((lpDispatch = m_pIView->GetSlide()) == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOSLIDEFOUND);
		return TMPOWER_NOSLIDEFOUND;
	}
	else
	{
		pISlide = new _Slide(lpDispatch);
		ASSERT(pISlide->m_lpDispatch);
	}

	//	Copy the slide
	pISlide->Copy();
	delete pISlide;

	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::CPowerPoint()
//
// 	Description:	This is the class constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPowerPoint::CPowerPoint() : CDialog(CPowerPoint::IDD, 0)
{
	//{{AFX_DATA_INIT(CPowerPoint)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	m_pControl = 0;
	m_pIApp = 0;
	m_pSnapshot = 0;
	m_hPPWnd = 0;
	m_hSSWnd = 0;
	m_hSSParent = 0;
	m_hNSSWnd = 0;
	m_hNSSParent = 0;
	m_pErrors = 0;
	m_lSlides = 0;
	m_lUserData = 0;
	m_lAnimations = 0;
	m_lAnimation = 0;
	m_bEnableAccelerators = FALSE;
	m_bCheckWndError = FALSE;
	m_bHideTaskBar = TRUE;
	m_uFocusTimer = 0;
	m_fPPVersion = 0;
	m_sPPState = TMPOWER_DONE;
	
	m_pIPresentations = 0;
	m_pIPresentation = 0;
	m_pISlides = 0;
	m_pISettings = 0;
	m_pIWindow = 0;
	m_pIView = 0;
	m_pISlide = 0;

	m_pNIPresentations = 0;
	m_pNIPresentation = 0;
	m_pNISlides = 0;
	m_pNISettings = 0;
	m_pNIWindow = 0;
	m_pNIView = 0;
	
	m_lId = 0;
	m_strEnum.Empty();
	m_strFilename.Empty();
	m_strNIFilename.Empty();
	m_crBackground = RGB(0,0,0);
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(m_crBackground);
	ZeroMemory(&m_rcMax, sizeof(m_rcMax));

	InitializeCriticalSection(&m_Lock);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::~CPowerPoint()
//
// 	Description:	This is the class destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPowerPoint::~CPowerPoint()
{
	//	Make sure all the dispatch interfaces have been released
	ReleaseNew();
	ReleaseActive();

	//	Delete the snapshot
	if(m_pSnapshot) delete m_pSnapshot;

	//	Delete the background brush
	if(m_pBackground) delete m_pBackground;
		
	DeleteCriticalSection(&m_Lock);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Create()
//
// 	Description:	This is called to create the powerpoint instance. 
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CPowerPoint::Create(CTMPowerCtrl* pControl, long lId)
{
	ASSERT(pControl);
	ASSERT(m_pIApp);
	ASSERT(IsWindow(pControl->m_hWnd));

	//	Set the members provided by the control
	m_lId = lId;
	m_pControl = pControl;

	//	Create the dialog box
	if(!CDialog::Create(CPowerPoint::IDD, m_pControl))
		return FALSE;

	//	Set the initial size
	Resize();

	return  TRUE;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box and class members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPowerPoint)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::FindFile()
//
// 	Description:	This function is called to determine if the specified file
//					exists
//
// 	Returns:		TRUE if the file is found
//
//	Notes:			None
//
//==============================================================================
BOOL CPowerPoint::FindFile(LPCSTR lpFilespec) 
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;

	if((hFind = FindFirstFile(lpFilespec, &FindData)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		FindClose(hFind);
		return TRUE;
	}	
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::First()
//
// 	Description:	This function is called to advance to the first slide in the
//					show
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::First() 
{
	//	Do we have a slide show view?
	if(m_pIView == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}
	else
	{
		try
		{
			m_pIView->First();

			//	Update the slide information
			OnChangeSlide();

			//	This call leaves the slide show on the last animation of the
			//	first slide
			m_lAnimation = m_lAnimations;

			return TMPOWER_NOERROR;
		}
		catch(COleException *e)
		{
			HandleException(e);
			return TMPOWER_OLEEXCEPTION;
		}
		catch(COleDispatchException *e)
		{
			HandleException(e);
			return TMPOWER_OLEEXCEPTION;
		}

	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetAnimated()
//
// 	Description:	This function is called to determine if the specified
//					shape is animated.
//
// 	Returns:		TRUE if animated
//
//	Notes:			None
//
//==============================================================================
BOOL CPowerPoint::GetAnimated(Shape* pIShape) 
{
	AnimationSettings*	pISettings;
	BOOL				bAnimated = FALSE;

	ASSERT(pIShape != 0);
	ASSERT(pIShape->m_lpDispatch != 0);

	pISettings = new AnimationSettings(pIShape->GetAnimationSettings());
	ASSERT(pISettings);
	ASSERT(pISettings->m_lpDispatch);

	if((pISettings != 0) && (pISettings->m_lpDispatch != 0))
		bAnimated = pISettings->GetAnimate();

	RELEASE_INTERFACE(pISettings);

	return bAnimated;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetAnimations()
//
// 	Description:	This function is called to determine how many animations
//					are present in the current slide.
//
// 	Returns:		The number of animated shapes in the current slide
//
//	Notes:			None
//
//==============================================================================
long CPowerPoint::GetAnimations() 
{
	//	Make sure we have a valid slide
	ASSERT(m_pISlide != 0);
	ASSERT(m_pISlide->m_lpDispatch != 0);
	if((m_pISlide == 0) || (m_pISlide->m_lpDispatch == 0)) return 0;
	
	//	Is the presentation have animations enabled?
	if((m_pISettings != 0) && (m_pISettings->GetShowWithAnimation() == 0))
		return 0;

	//	This is a shortcut we take for determining the number of animations
	//
	//	If this proves to be unreliable for some animation types, there is a
	//	more accurate implementation commented out at the end of this file
	return (m_pISlide->GetPrintSteps() - 1);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetDDBitmap()
//
// 	Description:	This function is called to get a DDB equivalent of the 
//					slide show window contents
//
// 	Returns:		A handle to the bitmap if successful 
//
//	Notes:			None
//
//==============================================================================
HBITMAP CPowerPoint::GetDDBitmap(int* pWidth, int* pHeight)
{
	HDC		dcSSWnd;
	HDC		dcMem;
	HBITMAP	hBitmap;
	HBITMAP	hOldBitmap;
	RECT	rcSSWnd;
	int		iWidth;
	int		iHeight;

	//	Is the slide show window available and visible?
	if(m_hSSWnd == 0 || !IsWindow(m_hSSWnd))
		return NULL;	

	//	Get the client area of the rendering window
	GetClientRect(&rcSSWnd);

	//	Make sure we're not dealing with an empty rectangle
	if(IsRectEmpty(&rcSSWnd))
		return NULL;

	//	Create the device contexts we need to create the bitmap
	if((dcSSWnd = ::GetDC(m_hWnd)) == NULL)
		return NULL;
	if((dcMem = ::CreateCompatibleDC(dcSSWnd)) == NULL)
	{
		DeleteDC(dcSSWnd);
		return NULL;
	}

	//	Calculate the width and height of the capture area
	iWidth  = rcSSWnd.right - rcSSWnd.left;
	iHeight = rcSSWnd.bottom - rcSSWnd.top;

	//	Create a bitmap compatible with the screen dc
	if((hBitmap = ::CreateCompatibleBitmap(dcSSWnd, iWidth, iHeight)) == NULL)
	{
		DeleteDC(dcSSWnd);
		DeleteDC(dcMem);
		return NULL;
	}

	//	Select the new bitmap into the memory dc
	hOldBitmap = (HBITMAP)::SelectObject(dcMem, hBitmap);

	//	BitBlt the slide show area to the memory dc
	BitBlt(dcMem, 0, 0, iWidth, iHeight, dcSSWnd, 0,
		   0, SRCCOPY);

	//	Select the old bitmap back into the memory dc and retain the new bitmap
	hBitmap = (HBITMAP)SelectObject(dcMem, hOldBitmap);
	if(pWidth)	*pWidth  = iWidth;
	if(pHeight) *pHeight = iHeight;

	//	Clean up
	DeleteDC(dcSSWnd);
	DeleteDC(dcMem);

	return hBitmap;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetISlide()
//
// 	Description:	This function is called to get the dispatch interface for
//					the current slide
//
// 	Returns:		A pointer to the interface if successful
//
//	Notes:			The caller is responsible for deallocation of the 
//					interface
//
//==============================================================================
_Slide* CPowerPoint::GetISlide() 
{
	LPDISPATCH	lpDispatch;
	_Slide*		pISlide;

	//	Do we have a slide show view?
	if(m_pIView == 0)
		return 0;

	//	Get the current slide
	if((lpDispatch = m_pIView->GetSlide()) != 0)
	{
		pISlide = new _Slide(lpDispatch);
		ASSERT(pISlide->m_lpDispatch);
		return pISlide;
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetSlideNumber()
//
// 	Description:	This function is called to get the index of the slide with
//					the specified id.
//
// 	Returns:		The associated slide index if found. 0 otherwise.
//
//	Notes:			None
//
//==============================================================================
long CPowerPoint::GetSlideNumber(long lSlideId, Slides* pISlides)
{
	_Slide*	pISlide = 0;
	long	lNumber = 0;
	CString	strError;

	//	Should we use the current slide container
	if(pISlides == 0)
		pISlides = m_pISlides;

	//	Do we have a list of slides
	if(pISlides == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}
	else
	{
		//	Get the interface to the requested slide
		pISlide = new _Slide(pISlides->FindBySlideID(lSlideId));
		if(pISlide->m_lpDispatch != 0)
		{
			//	Get the slide number and release the interface
			lNumber = pISlide->GetSlideIndex();
			RELEASE_INTERFACE(pISlide);
		}
		else
		{
			if(m_pErrors)
			{
				strError.Format("%ld", lSlideId);
				m_pErrors->Handle(0, IDS_TMPOWER_INVALIDSLIDEID, strError);
			}
			return TMPOWER_INVALIDSLIDEID;
		}
		
		return lNumber;
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetSlideRange()
//
// 	Description:	This function is called to get a range of slides from the
//					active presentation
//
// 	Returns:		A pointer to dispatch interface for the new range
//
//	Notes:			None
//
//==============================================================================
SlideRange* CPowerPoint::GetSlideRange(long lFirst, long lLast)
{
	LPDISPATCH		lpDispatch;
	SlideRange*		pIRange;
	COleSafeArray	SafeArray;
	SAFEARRAYBOUND	Bounds;
	long*			pSlides;

	//	Do we have the slide list?
	if(m_pISlides == 0)
		return 0;

	//	Define the array limits
	Bounds.lLbound   = 0;
	Bounds.cElements = lLast - lFirst + 1;

	//	Create a byte array of the proper size
	SafeArray.Create(VT_I4, 1, &Bounds);

	//	Get a pointer to the array data
	SafeArray.AccessData((void**)&pSlides);
	ASSERT(pSlides != 0);

	for(long i = lFirst; i <= lLast; i++)
		pSlides[i - lFirst] = i;

	//	Unlock the array buffer
	SafeArray.UnaccessData();

	//	Get the requested range
	if((lpDispatch = m_pISlides->Range(SafeArray)) != 0)
	{
		pIRange = new SlideRange(lpDispatch);
		ASSERT(pIRange->m_lpDispatch);

		return pIRange;
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetSnapshot()
//
// 	Description:	This function is called to get a snapshot of the current
//					slide show window
//
// 	Returns:		A pointer to the snapshot window if successful
//
//	Notes:			None
//
//==============================================================================
CSnapshot* CPowerPoint::GetSnapshot(BOOL bPopup) 
{
	CSnapshot*			pSnapshot = 0;
	int					iWidth;
	int					iHeight;
	HBITMAP				hDDB;

	//	Allocate a new snapshot
	pSnapshot = new CSnapshot();
	ASSERT(pSnapshot);

	//	Create the snapshot window
	if(!pSnapshot->Create(this, bPopup))
	{
		delete pSnapshot;
		return 0;
	}

	ASSERT(IsWindow(pSnapshot->m_hWnd));
	if(!IsWindow(pSnapshot->m_hWnd))
	{
		delete pSnapshot;
		return 0;
	}

	//	Do we have the slide show window?
	if(m_hSSWnd && IsWindow(m_hSSWnd) && ::IsWindowVisible(m_hSSWnd))
	{
		//	Get a device dependent bitmap
		if((hDDB = GetDDBitmap(&iWidth, &iHeight)) == 0)
		{
			pSnapshot->SetDDBitmap(0, 0, 0);
		}
		else
		{
			//	Set the snapshot image
			pSnapshot->SetDDBitmap(hDDB, iWidth, iHeight);
		}

	}

	//	Resize the snapshot to fit over this window
	pSnapshot->MoveWindow(&m_rcMax);

	return pSnapshot;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::MyEnumDesktopWindows()
//
// 	Description:	This function is called to handle the Powerpoint.exe windows.
//
// 	Returns:		Returns BOOL for succesful launch of Powerpoint window.
//
//	Notes:			None
//
//==============================================================================
HWND pppWnd=0;
DWORD oppThread=0;
BOOL CPowerPoint::MyEnumDesktopWindows(HWND hWnd)
{

	CWnd*	pChild = CWnd::FromHandle(hWnd);
	char	szTitle[512];
	HWND	hwndMDIClient;
	HWND    hwndmdiClass;

	//	Get the title of this window
	if(pChild == 0)
		return TRUE;
	else
		pChild->GetWindowText(szTitle, sizeof(szTitle));


	#define pwrpt "PowerPoint Slide Show"
	if(strncmp(pwrpt, szTitle, lstrlen(pwrpt)) == 0)
	{
		//	Get the child by using the class name of the slide show window
		if((hwndMDIClient = ::FindWindowEx(hWnd, NULL, "MDIClient", NULL)) == NULL) {

			return TRUE;
		}

		if((hwndmdiClass = ::FindWindowEx(hwndMDIClient, NULL, "mdiClass", NULL)) == NULL) {

			return TRUE;
		}
		
		// Powerpoint 2013/2016 fix. The paneClassDC does not exist in this scenario.
		if(m_fPPVersion == PpVersion::ppVersion2013){
			
			m_hNSSWnd = hwndmdiClass;
		}
		else if((m_hNSSWnd = ::FindWindowEx(hwndmdiClass, NULL, "paneClassDC", NULL)) == NULL) {

			return TRUE;
		}

		char pText[255];
		::GetWindowTextA(m_hNSSWnd, pText, 255);

		if(strcmp(pText, "Slide Show") != 0 && strcmp(pText, "") != 0 ) return TRUE;


		m_hNSSParent = hWnd;

			if(oppThread == 0) 
			oppThread = GetWindowThreadProcessId(m_hPPWnd, 0);

		m_hPPWnd = hWnd;
		
		DWORD _dwPPThread = GetWindowThreadProcessId(hWnd, 0);
		AttachThreadInput(oppThread, m_pControl->m_dwFocusThread, FALSE);
		AttachThreadInput(_dwPPThread, m_pControl->m_dwFocusThread, TRUE);
		SetFocus();
		oppThread = _dwPPThread;
		

		return (m_hNSSWnd == 0);
	}
	else
	{
		//	Keep enumerating - window not found
		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetSSHandle()
//
// 	Description:	This function is called to retrieve the handle of the
//					slide show window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPowerPoint::GetSSHandle(LPCTSTR lpName) 
{
	//	Reset the existing handles
	m_hNSSParent = 0;
	m_hNSSWnd = 0;

	//	Build the title of the document window that contains the slide show
	//	view we are looking for
	//m_strEnum.Format("PowerPoint Slide Show - [%s [Read-Only]]", lpName);

	char	szName[512];
	char*	pExtension;

	lstrcpyn(szName, lpName, sizeof(szName));
	if((pExtension = strrchr(szName, '.')) != 0)
		*pExtension = '\0';
	m_strEnum = szName;

	
	if(m_fPPVersion >= PpVersion::ppVersion2010)
	{
		//For powerpoint 2010 we need to loop on all the windows to find the
		//slideshow window previously it was not required as the the slide
		//show uses the same application window
		EnumChildWindows(GetDesktopWindow()->m_hWnd, EnumViewWindows, 
					(LPARAM)this);
	}
	else
	{
	//	Now enumerate all children of the PowerPoint main window
		::EnumChildWindows(m_hPPWnd, EnumViewWindows, (LPARAM)this);
	}
	return (m_hNSSWnd != 0);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::GetState()
//
// 	Description:	This function is called to retrieve the current state of the
//					slide show viewer
//
// 	Returns:		The current view state
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::GetState() 
{
	short sState = TMPOWER_DONE;

	//	Do we have an active viewer
	try
	{
		if(m_pIView)
		{
			//	Get the current state of the viewer and translate to TMPower
			//	state identifiers
			switch(m_pIView->GetState())
			{
				case ppSlideShowRunning:		sState = TMPOWER_RUNNING;
												break;
				case ppSlideShowPaused:			sState = TMPOWER_PAUSED;
												break;
				case ppSlideShowWhiteScreen:	sState = TMPOWER_WHITESCREEN;
												break;
				case ppSlideShowBlackScreen:	sState = TMPOWER_BLACKSCREEN;
												break;
				case ppSlideShowDone:	
				default:						sState = TMPOWER_DONE;
												break;
			}
		}
	}
	catch(...)
	{
	}
	
	return sState;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::HandleException()
//
// 	Description:	This function is called to handle the specified exception
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::HandleException(COleDispatchException* pException) 
{
	char	szMsg[1024];
	CString	strMsg;

	ASSERT(pException);

	if(m_pErrors)
	{
		if(pException->m_strDescription.GetLength() > 0)
		{
			lstrcpyn(szMsg, pException->m_strDescription, sizeof(szMsg));
		}
		else
		{
			if(!pException->GetErrorMessage(szMsg, sizeof(szMsg)))
				sprintf_s(szMsg, sizeof(szMsg), "No description available");
		}
		strMsg.Format("PowerPoint Exception SCODE: %08lx \n\n%s", szMsg);
		m_pErrors->Handle(0, strMsg);
	}	
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::HandleException()
//
// 	Description:	This function is called to handle the specified exception
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::HandleException(COleException* pException) 
{
	char	szMsg[1024];
	CString	strMsg;

	ASSERT(pException);

	if(m_pErrors)
	{
		if(!pException->GetErrorMessage(szMsg, sizeof(szMsg)))
			sprintf_s(szMsg, sizeof(szMsg), "No description available");
		strMsg.Format("PowerPoint Exception SCODE: %08lx \n\n%s", szMsg);
		m_pErrors->Handle(0, strMsg);
	}	
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Last()
//
// 	Description:	This function is called to advance to the last slide in the
//					show
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::Last() 
{
	//	Do we have a slide show view?
	if(m_pIView == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}
	else
	{
		try
		{
			m_pIView->Last();

			//	Update the slide information
			OnChangeSlide();

			return TMPOWER_NOERROR;
		}
		catch(COleException *e)
		{
			HandleException(e);
			return TMPOWER_OLEEXCEPTION;
		}
		catch(COleDispatchException *e)
		{
			HandleException(e);
			return TMPOWER_OLEEXCEPTION;
		}
	
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Lock()
//
// 	Description:	This function is called to set the lock
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::Lock()	
{ 
#if defined _USE_LOCK
	EnterCriticalSection(&m_Lock);
#endif 
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Next()
//
// 	Description:	This function is called to advance to the next slide in the
//					show
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::Next() 
{
	//	Do we have a slide show view?
	if(m_pIView == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}
	else
	{
		//	Are we already at the end of the show?
		if(UpdateState() == TMPOWER_DONE)
		{
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMPOWER_OUTOFRANGE);
			return TMPOWER_OUTOFRANGE;
		}
		else
		{
			try
			{
				m_pIView->Next();

				//	Do we need to adjust the animation index?
				if(m_lAnimations > 0)
				{
					if(m_lAnimation < m_lAnimations)
						m_lAnimation++;
				}

				//	Update the slide information
				OnChangeSlide();

				return TMPOWER_NOERROR;
			}
			catch(COleException *e)
			{
				HandleException(e);
				return TMPOWER_OLEEXCEPTION;
			}
			catch(COleDispatchException *e)
			{
				HandleException(e);
				return TMPOWER_OLEEXCEPTION;
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::OnChangeSlide()
//
// 	Description:	This function is called when the user changes the slide
//					in the current presentation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::OnChangeSlide() 
{
	//	Are we connected to PowerPoint 2K
	if((m_fPPVersion >= 9.0) && (m_fPPVersion < 10.0))
	{
		//	Have we encountered the CheckWnd error?
		if(m_bCheckWndError)
		{
			//	Make sure the window is properly sized
			CheckWnd();
		}
		else
		{
			//	Set the CheckWnd timer
			//
			//	NOTE:	We have to use a timer to break the call chain because
			//			the actual resizing of the window performed by PowerPoint
			//			will not take place until the calling method returns. Therefore
			//			our test for the CheckWnd error will always pass even though
			//			the window actually gets resized by PowerPoint
			SetTimer(CHECKWND_TIMER, 1, NULL);
		}
	}

	//	Make sure we update the slide information
	SetCurrent();	
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::OnCtlColor()
//
// 	Description:	This function is overloaded to draw the view in the
//					appropriate color
//
// 	Returns:		The handle of the brush used to paint the background
//
//	Notes:			None
//
//==============================================================================
HBRUSH CPowerPoint::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG && m_pBackground)
		return (HBRUSH)(*m_pBackground);
	else
		return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::OnEnumWindow()
//
// 	Description:	This function is called by the window enumeration callback
//					when we are looking for the PowerPoint view window
//
// 	Returns:		TRUE if enumeration should continue
//
//	Notes:			None
//
//==============================================================================
BOOL CPowerPoint::OnEnumWindow(HWND hWnd) 
{
	CWnd*	pChild = CWnd::FromHandle(hWnd);
	CString strTitle;
//////////////////////////// Code change for Powerpoint 2010 issue
	
	//	Get the title of this window
	if(pChild == 0)
		return TRUE;
	else {
		pChild->GetWindowText(strTitle);
	}

	//	Get the child by using the class name of the slide show window
	if((m_hNSSWnd = ::FindWindowEx(hWnd, NULL, "paneClassDC", NULL)) == NULL) {

		return TRUE;
	}

	char pText[255];
	::GetWindowTextA(m_hNSSWnd, pText, 255);

	if(strcmp(pText, "Slide Show") != 0 && strcmp(pText, "") != 0 ) return TRUE;

	//	This is actually the parent of the slide show window
	m_hNSSParent = hWnd;

//////////////////////////// Code change for Powerpoint 2010 issue


	//	No need to keep enumerating
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::OnTimer()
//
// 	Description:	This function handles all WM_TIMER messages
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::OnTimer(UINT nIDEvent) 
{
	long lState = ppSlideShowDone;
	SlideShowWindow *sswnd=NULL;
	SlideShowView *svw=NULL;

	//	Which timer event is this?
	switch(nIDEvent)
	{
		case ASSIGN_NEW_TIMER:

			KillTimer(ASSIGN_NEW_TIMER);

			sswnd = new SlideShowWindow(m_pNIPresentation->GetSlideShowWindow());
			svw = new SlideShowView( sswnd->GetView() );
			svw->SetPointerType(1);

			//	Do we still have a valid view?
			if(m_pNIView == 0) return;

			//	Get the state of the new view
			try
			{
				lState = m_pNIView->GetState();
			}
			catch(...)
			{
				return;
			}

			//	Is the view ready
			if(lState != ppSlideShowDone)
			{
				AssignNew(m_lSetSlide);
			}
			else
			{
				//	Have we timed out?
				if((GetTickCount() - m_dwAssignNew) > ASSIGN_NEW_TIMEOUT)
				{
					//	Go ahead and try anyway
					AssignNew(m_lSetSlide);
				}
				else
				{
					//	Restart the timer
					SetTimer(ASSIGN_NEW_TIMER, 100, NULL);
				}
				
			}
			break;

		case CHECKWND_TIMER:

			//	Kill the timer until and check the current window position
			KillTimer(CHECKWND_TIMER);
			CheckWnd();
			break;

		case FOCUS_TIMER:

			//	NOTE:	DO NOT kill the timer. This ensures that PowerPoint never
			//			gains the keyboard focus
			if(IsWindow(m_hSSWnd) && (::GetFocus() == m_hSSWnd))
			{
				if(m_pControl)
				{
					m_pControl->OnPPFocus(m_lId);
				}
			}
			break;

		case UPDATE_TIMER:

			KillTimer(UPDATE_TIMER);
			UpdateState();
			SetCurrent();
			break;
	}

	CDialog::OnTimer(nIDEvent);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Previous()
//
// 	Description:	This function is called to advance to go back to the 
//					previous slide
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::Previous() 
{
	long lCurrent;

	//	Do we have a slide show view?
	if(m_pIView == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}
	else
	{
		try
		{
			m_pIView->Previous();

			//	Do we need to adjust the animation index?
			if(m_lAnimations > 0)
			{
				if(m_lAnimation > 0)
					m_lAnimation--;
			}

			lCurrent = m_lCurrent;

			//	Update the slide information
			OnChangeSlide();

			//	Did the slide change?
			if(lCurrent != m_lCurrent)
			{
				//	The user might have backed into an animation
				m_lAnimation = m_lAnimations;
			}

			return TMPOWER_NOERROR;
		}
		catch(COleException *e)
		{
			HandleException(e);
			return TMPOWER_OLEEXCEPTION;
		}
		catch(COleDispatchException *e)
		{
			HandleException(e);
			return TMPOWER_OLEEXCEPTION;
		}
	}

}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Redraw()
//
// 	Description:	This function is called to redraw the window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::Redraw() 
{
	if(IsWindow(m_hSSWnd))
	{
		CWnd::FromHandle(m_hSSWnd)->RedrawWindow();
	}
	else if(IsWindow(m_hWnd))
	{
		RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::ReleaseActive()
//
// 	Description:	This function is called to release all active dispatch
//					interfaces
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::ReleaseActive() 
{
	CWnd* pOld;

	//	Give the active slide show window back to PowerPoint
	if(m_hSSWnd && m_hSSParent && IsWindow(m_hSSWnd) && IsWindow(m_hSSParent))
	{
		::ShowWindow(m_hSSWnd, SW_HIDE);

		//	We need to do this in PowerPoint 97 because the window will flash when we
		//	reassign the parent but in PowerPoint 2002 (XP) this causes huge delays if
		//	attempting to rescale a slide that has a photograph in it
		if(m_fPPVersion < 10.0)
		{
			::MoveWindow(m_hSSWnd, 0, 0, 1, 1, FALSE);
		}

		if((pOld = CWnd::FromHandle(m_hSSWnd)) != 0)
			pOld->SetParent(CWnd::FromHandle(m_hSSParent));
	}

	//	Close the current presentation
	if(m_pIPresentation)
	{
		try
		{
			m_pIPresentation->SetSaved(PP_TRUE);
			m_pIPresentation->Close();
		}
		catch(...)
		{
		}
	}

	//	Close all active dispatch interfaces
	RELEASE_INTERFACE(m_pISlide);
	RELEASE_INTERFACE(m_pIView);
	RELEASE_INTERFACE(m_pIWindow);
	RELEASE_INTERFACE(m_pISettings);
	RELEASE_INTERFACE(m_pISlides);
	RELEASE_INTERFACE(m_pIPresentation);
	RELEASE_INTERFACE(m_pIPresentations);

	m_lCurrent = 0;
	m_lAnimations = 0;
	m_lAnimation = 0;
	m_bCheckWndError = FALSE;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::ReleaseNew()
//
// 	Description:	This function is called to release all new dispatch
//					interfaces in the event of a failure while trying to load
//					a new presentation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::ReleaseNew() 
{
	CWnd* pOld;

	//	Give the active slide show window back to PowerPoint
	if(m_hNSSWnd && m_hNSSParent && IsWindow(m_hNSSWnd) && IsWindow(m_hNSSParent))
	{
		::ShowWindow(m_hNSSWnd, SW_HIDE);

		//	We need to do this in PowerPoint 97 because the window will flash when we
		//	reassign the parent but in PowerPoint 2002 (XP) this causes huge delays if
		//	attempting to rescale a slide that has a photograph in it
		if(m_fPPVersion < 10.0)
		{
			::MoveWindow(m_hNSSWnd, 0, 0, 1, 1, FALSE);
		}

		if((pOld = CWnd::FromHandle(m_hNSSWnd)) != 0)
			pOld->SetParent(CWnd::FromHandle(m_hNSSParent));
	}

	//	Close the presentation
	if(m_pNIPresentation)
	{
		try
		{
			m_pNIPresentation->SetSaved(PP_TRUE);
			m_pNIPresentation->Close();
		}
		catch(...)
		{
		}
	}

	//	Close all active dispatch interfaces
	RELEASE_INTERFACE(m_pNIView);
	RELEASE_INTERFACE(m_pNIWindow);
	RELEASE_INTERFACE(m_pNISettings);
	RELEASE_INTERFACE(m_pNISlides);
	RELEASE_INTERFACE(m_pNIPresentation);
	RELEASE_INTERFACE(m_pNIPresentations);
	m_strNIFilename.Empty();
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Resize()
//
// 	Description:	This function is called resize and position the window using
//					the maximum available rectangle
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::Resize() 
{
	//	Has the window been created yet?
	if(!IsWindow(m_hWnd))
		return;

	//	Move the windows
	MoveWindow(m_rcMax.left, m_rcMax.top, (m_rcMax.left + m_rcMax.right), 
										  (m_rcMax.top + m_rcMax.bottom));
	//	Adjust the slide show window
	if(IsWindow(m_hSSWnd))
		::MoveWindow(m_hSSWnd, 0, 0, m_rcMax.right, m_rcMax.bottom, TRUE);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::SaveSlide()
//
// 	Description:	This function is called to save the current slide to 
//					the specified file
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::SaveSlide(LPCTSTR lpFilename, int iFormat) 
{
	_Slide*		pISlide;
	CString		strFilter;
	DWORD		dwStart = GetTickCount();
	
	//	Are we waiting to assign the interfaces for a new file?
	while(m_pNIView != 0)
	{
		//	Give PowerPoint some time to load the file
		Sleep(500);

		//	Are we ready to assign the new file?
		OnTimer(ASSIGN_NEW_TIMER);

		//	Have we timed out?
		if((GetTickCount() - dwStart) > 60000)
		{
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMPOWER_LOAD_TIMED_OUT);
			return TMPOWER_LOAD_TIMED_OUT;
		}
	}

	//	Do we have a slide show view?
	if(m_pIView == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}

	//	Get the current slide
	if((pISlide = GetISlide()) == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOSLIDEFOUND);
		return TMPOWER_NOSLIDEFOUND;
	}

	//	Delete the existing file
	if(FindFile(lpFilename))
		_unlink(lpFilename);

	//	Get the appropriate filter
	switch(iFormat)
	{
		case TMPOWER_TIF:	strFilter = "TIF";
							break;
		case TMPOWER_BMP:	strFilter = "BMP";
							break;
		case TMPOWER_PNG:	strFilter = "PNG";
							break;
		case TMPOWER_WMF:	strFilter = "WMF";
							break;
		case TMPOWER_GIF:	strFilter = "GIF";
							break;
		case TMPOWER_JPG:
		default:			strFilter = "JPG";
							break;
	}

	//	Create the requested file
	pISlide->Export(lpFilename, strFilter, 0, 0);
	delete pISlide;

	if(FindFile(lpFilename))
		return TMPOWER_NOERROR;
	else
		return TMPOWER_SAVESLIDEFAILED;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::SetBackColor()
//
// 	Description:	This function is called to set the background color of the
//					view window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::SetBackColor(COLORREF crBackground) 
{
	//	Save the new color
	m_crBackground = crBackground;

	//	Create a new brush
	if(m_pBackground) delete m_pBackground;
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(crBackground);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::SetCurrent()
//
// 	Description:	This function is called to set the current slide index
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::SetCurrent() 
{
	long lCurrent;

	//	Prevent access while we update
	Lock();

	//	Update the current state
	UpdateState();

	//	Do we have an active view?
	if(m_pIView == 0)
	{
		m_lCurrent = 0;
		m_lAnimations = 0;
		m_lAnimation = 0;
	}
	else if(m_sPPState == TMPOWER_DONE)
	{
		m_lCurrent = m_lSlides + 1;
		m_lAnimations = 0;
		m_lAnimation = 0;
	}
	else
	{
		//	Release the current dispatch interface
		RELEASE_INTERFACE(m_pISlide);

		//	Get the interface to the current slide
		m_pISlide = new _Slide(m_pIView->GetSlide());
		if(m_pISlide->m_lpDispatch != 0)
		{
			lCurrent = m_pISlide->GetSlideNumber();

			//	Has the slide changed?
			if(lCurrent != m_lCurrent)
			{
				m_lCurrent = lCurrent;
				m_lAnimations = GetAnimations();
				m_lAnimation = 0;
			}
		}
		else
		{
			m_lCurrent = 0;
			m_lAnimations = 0;
			m_lAnimation = 0;
		}

	}

	Unlock();

	//	Notify the control
	if(m_pControl)
		m_pControl->OnPPSlideChange(m_lId, m_lCurrent);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::SetFilename()
//
// 	Description:	This function is called to set the name of the slide show
//					to be viewed in this window
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::SetFilename(LPCSTR lpFilename, long lStart, BOOL bSlideId) 
{
	LPDISPATCH	lpDispatch;
	static BYTE byParams[] = VTS_I4;
	CWnd*		pWnd;

	//	Are we unloading the current presentation?
	if(lpFilename == 0 || lstrlen(lpFilename) == 0)
		return Unload();

	//	Are we in the process of loading a file?
	if((m_pNIPresentation != 0) && (m_pNIView != 0))
	{
		//	Is this the same file
		if(lstrcmpi(lpFilename, m_strNIFilename) == 0)
		{
			//	Do we have to translate the slide ?
			if((lStart > 0) && bSlideId)
			{
				if((lStart = GetSlideNumber(lStart, m_pNISlides)) == 0)
					lStart = 1;
			}

			//	Is the slide out of range?
			if((lStart <= 0) || (lStart > m_lSlides))
				m_lSetSlide = 1;
			else
				m_lSetSlide = lStart;

			return TMPOWER_NOERROR;
		}
		else
		{
			//	Stop loading this file
			ReleaseNew();

			return SetFilename(lpFilename, lStart, bSlideId);
		}

	}

	//	Does this file exist?
	if(!FindFile(lpFilename))
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_FILENOTFOUND, lpFilename);
		return TMPOWER_FILENOTFOUND;
	}

	ASSERT(m_pIApp);
	if(m_pIApp == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}
	try
	{
		m_pIApp->SetVisible(TRUE);
		m_pIApp->SetWindowState(ppWindowNormal);
		
	}
	catch(...)
	{

	}

	//	Lock access to prevent the app from closing down while we establish the
	//	interfaces
	Lock();

	m_strNIFilename = lpFilename;

	//	Get the presentation container
	m_pNIPresentations = new Presentations(m_pIApp->GetPresentations());
	if(m_pNIPresentations->m_lpDispatch == 0)
	{
		Unlock();
		ReleaseNew();
		
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOPRESENTATIONS);
		
		return TMPOWER_NOPRESENTATIONS;
	}

	//	Load the file
	long bWithWindow;
	if(m_fPPVersion >= PpVersion::ppVersion2010)
	{
		bWithWindow = FALSE;
	}
	else
	{
		bWithWindow = FALSE;
	}
	if((lpDispatch = m_pNIPresentations->Open(lpFilename, TRUE, FALSE, bWithWindow)) == 0)
	{
		Unlock();
		ReleaseNew();
		
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOPRESENTATION, lpFilename);

		return TMPOWER_NOPRESENTATION;
	}
	else
	{
		m_pNIPresentation = new _Presentation(lpDispatch);
		ASSERT(m_pNIPresentation->m_lpDispatch);

		//	Notify the control
		if(m_pControl)
			m_pControl->OnPPLoad(m_lId);
	}

	//	Get the slides for this presentation
	m_pNISlides = new Slides(m_pNIPresentation->GetSlides());
	if(m_pNISlides->m_lpDispatch == 0)
	{
		Unlock();
		ReleaseNew();
		
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOSLIDES);

		return TMPOWER_NOSLIDES;
	}

	// Powerpoint 2010 has by default transition enabled that we need to disable it other wise getting error

	long savEntryEffect=0;
	long tmpStartSlide = 1;
	if(m_fPPVersion >= PpVersion::ppVersion2010)
	{

		if((lStart > 0) && bSlideId)
		{
			if((tmpStartSlide = GetSlideNumber(lStart, m_pNISlides)) == 0)
				tmpStartSlide = 1;
		}
		long i = 1;	
		try {
			VARIANT v;
			v.lVal = i;
			v.vt = VT_I4;
			_Slide slide(m_pNISlides->Item(v));
			SlideShowTransition transition(slide.GetSlideShowTransition());
			savEntryEffect=transition.GetEntryEffect();
			transition.SetEntryEffect(ppAnimateLevelNone);
		}
		catch(CException* pException) {

			char errMsg[255];
			pException->GetErrorMessage(errMsg, 255);
			AfxMessageBox(errMsg);
		}
	}
	
	//	Get the slide show settings for this presentation
	m_pNISettings = new SlideShowSettings(m_pNIPresentation->GetSlideShowSettings());
	if(m_pNISettings->m_lpDispatch == 0)
	{
		Unlock();
		ReleaseNew();
		
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOSETTINGS);

		return TMPOWER_NOSETTINGS;
	}
	else
	{
		try
		{
			//	Set the slide show properties
			m_pNISettings->SetShowType(ppShowTypeWindow);
			m_pNISettings->SetAdvanceMode(ppSlideShowManualAdvance);

		}
		catch(COleException *e)
		{
			Unlock();
			ReleaseNew();
		
			HandleException(e);

			return TMPOWER_OLEEXCEPTION;
		}
		catch(COleDispatchException *e)
		{
			Unlock();
			ReleaseNew();
		
			HandleException(e);

			return TMPOWER_OLEEXCEPTION;
		}

		//	We discovered a problem with PowerPoint 2K/XP where PowerPoint resizes the
		//	slide show window whenever you change the slide. This only occurs if the
		//	presentation was created with the "Show scrollbar" option turned ON (default)
		//	for the	slide show. 
		//
		//	This code will turn that property off. Unfortunately the ShowScrollbar property
		//	is only exposed in the PowerPoint 10 interface. Rather than loose support for
		//	Office 97/2K I have chosen to set the property using the low-level interface
		//	rather than updating the class wrapper.
		if(m_fPPVersion >= 10.0)
		{
			try
			{
				m_pNISettings->InvokeHelper(0x7df, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, 
											byParams, PP_FALSE);
			}
			catch(...)
			{
			}
		}

		//--------------------------------------------------------------------
		//	THIS WILL PRESET THE RANGE BUT THEN WE CAN ONLY INTERATE WITHIN
		//	THIS PREDEFINED RANGE
		//--------------------------------------------------------------------
		/*
		try
		{
			m_pNISettings->SetStartingSlide((long)4);
			m_pNISettings->SetEndingSlide(6);
			m_pNISettings->SetRangeType(ppShowSlideRange);
		}
		catch(COleException *e)
		{
			HandleException(e);
			return TMPOWER_OLEEXCEPTION;
		}
		catch(COleDispatchException *e)
		{
			HandleException(e);
			return TMPOWER_OLEEXCEPTION;
		}
		*/

		try
		{
			m_pNIPresentation->SetSaved(PP_TRUE);	
		}
		catch(...)
		{
			Unlock();
			ReleaseNew();

			//	Don't bother with an error message because the app may be closing

			return TMPOWER_OLEEXCEPTION;
		}

	}
	
	//	Snapshot the current slide
	SetSnapshot();

	//	Hide the task bar before running the show
	//
	//	NOTE:	We do this because PowerPoint activates the task bar when
	//			we start a new show
	if(m_bHideTaskBar)
		CTMToolbox::SetTaskBarVisible(FALSE);

	//	Run the slide show
	if((lpDispatch = m_pNISettings->Run()) == 0)
	{
		Unlock();
		ReleaseNew();

		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOSLIDEWINDOW);

		return TMPOWER_NOSLIDEWINDOW;
	}
	else
	{
		m_pNIWindow = new SlideShowWindow(lpDispatch);
		ASSERT(m_pNIWindow->m_lpDispatch);
	}

	if(m_fPPVersion >= PpVersion::ppVersion2010)
	{		

		long i = 1;	
		try {
			VARIANT v;
			v.lVal = i;
			v.vt = VT_I4;
			_Slide slide(m_pNISlides->Item(v));
			SlideShowTransition transition(slide.GetSlideShowTransition());					
			savEntryEffect=0;
			transition.SetEntryEffect(savEntryEffect);
		}
		catch(CException* pException) {

			char errMsg[255];
			pException->GetErrorMessage(errMsg, 255);
			AfxMessageBox(errMsg);
		}		
	}

	//	Get the slide show view interface
	m_pNIView = new SlideShowView(m_pNIWindow->GetView());
	if(m_pNIView->m_lpDispatch == 0)
	{
		Unlock();
		ReleaseNew();

		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOSLIDEVIEW);

		return TMPOWER_NOSLIDEVIEW;
	}
	else
	{
		//	Should we disable the PowerPoint accelerators?
		if(!m_bEnableAccelerators)
		{
			try
			{
				m_pNIView->SetAcceleratorsEnabled(PP_FALSE);
			}
			catch(...)
			{
				//	Ignore
			}
		}
	}
	
	//	Get the slide show window handle
	if(!GetSSHandle(m_pNIPresentation->GetName()))
	{
		Unlock();
		ReleaseNew();

		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOSLIDEHANDLE);

		return TMPOWER_NOSLIDEHANDLE;
	}

	//	Get the number of slides in the new presentation
	if((m_lSlides = m_pNISlides->GetCount()) <= 0)
	{
		Unlock();
		ReleaseNew();

		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOSLIDES);

		return TMPOWER_NOSLIDES;
	}

	//	Do we have to translate the slide ?
	if((lStart > 0) && bSlideId)
	{
		if((lStart = GetSlideNumber(lStart, m_pNISlides)) == 0)
			lStart = 1;
	}

	//	Is the slide out of range?
	if((lStart <= 0) || (lStart > m_lSlides))
		lStart = 1;

	//	PowerPoint 97 will not allow us to advance the slide unless
	//	we first assign the parent and make the slide show visible
	if((lStart > 1) && (m_fPPVersion <= 8.0))
	{
		if((pWnd = CWnd::FromHandle(m_hNSSWnd)) != 0)
		{
			pWnd->MoveWindow(0,0,1,1,FALSE);
			pWnd->SetParent(this);
			pWnd->ShowWindow(SW_SHOW);
		}
	}


	//	Release the lock
	Unlock();

	//	Use a timer to break the call chain. This gives the slide show
	//	the ability to initialize before we attempt to change the slide
	m_lSetSlide   = lStart;
	m_dwAssignNew = GetTickCount();
	SetTimer(ASSIGN_NEW_TIMER, 1, NULL);

	//	Get the list of presentations from the application
	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::SetInitialSlide()
//
// 	Description:	This function is called to advance the specified slide show
//					view to the requested slide
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPowerPoint::SetInitialSlide(long lSlide) 
{
	ASSERT(m_pNIView);
	ASSERT(m_pNISlides);
	if((m_pNIView == 0) || (m_pNISlides == 0) || (lSlide <= 0) || (lSlide > m_lSlides))
		return FALSE;

	try
	{
		//	Go to the caller specified slide
		m_pNIView->GotoSlide(lSlide, PP_TRUE);
		return TRUE;
	}
	catch(COleException *e)
	{
		HandleException(e);
		return FALSE;
	}
	catch(COleDispatchException *e)
	{
		HandleException(e);
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::SetMaxRect()
//
// 	Description:	This function is called by the control object when its size
//					changes. It sets the rectangle that defines the position and
//					size of the rectangle this object can use.
//
// 	Returns:		None
//
//	Notes:			The control window sets the extents so that they always 
//					reflect the maximum available size. 
//
//					pRect->right  = Width (not right hand coordinate)
//					pRect->bottom = Height (not bottom coordinate)
//
//==============================================================================
void CPowerPoint::SetMaxRect(RECT& rRect) 
{
	//	Save the new rectangle
	m_rcMax.left   = rRect.left;
	m_rcMax.top    = rRect.top;
	m_rcMax.right  = rRect.right;
	m_rcMax.bottom = rRect.bottom;

	//	This prevents the possibility of divide by zero errors
	if(m_rcMax.right == 0)
		m_rcMax.right = 1;
	if(m_rcMax.bottom == 0)
		m_rcMax.bottom = 1;

	//	Resize the window
	Resize();
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::SetSlide()
//
// 	Description:	This function is called to advance to the slide show to the
//					specified slide
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::SetSlide(long lSlide, BOOL bSlideId) 
{
	//	Are we in the process of loading a file?
	if(m_pNIView != 0)
	{
		return SetFilename(m_strFilename, lSlide, bSlideId);
	}

	//	Do we have to translate the slide id?
	if(bSlideId)
	{
		if((lSlide = GetSlideNumber(lSlide, 0)) == 0)
			return TMPOWER_INVALIDSLIDEID;
	}

	//	Do we have a slide show view?
	if(m_pIView == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPOWER_NOTREADY);
		return TMPOWER_NOTREADY;
	}
	else
	{
		//	Is the slide within range?
		if(lSlide < 1 || lSlide > m_lSlides)
		{ 
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMPOWER_OUTOFRANGE);
			return TMPOWER_OUTOFRANGE;
		}
		else
		{
			//	Go to the caller specified slide
			m_pIView->GotoSlide(lSlide, PP_TRUE);

			//	No longer positioned on an animation
			m_lAnimation = 0;

			//	Update the slide information
			OnChangeSlide();

			return TMPOWER_NOERROR;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::SetSnapshot()
//
// 	Description:	This function is called to create a position a snapshot of
//					the current slide show window.
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::SetSnapshot() 
{
	CSnapshot* pSnapshot;

	//	Get a snapshot of the current slide
	if((pSnapshot = GetSnapshot(FALSE)) != 0)
	{
		//	Show the new snapshot
		pSnapshot->ShowWindow(SW_SHOW);
		pSnapshot->BringWindowToTop();

		//	Delete the existing snapshot
		if(m_pSnapshot)
			delete m_pSnapshot;

		//	Save the pointer to the new snapshot
		m_pSnapshot = pSnapshot;
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::StartTimers()
//
// 	Description:	This function is called to start the timers we need when the
//					viewer is activated.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::StartTimers() 
{
	//	Start the timer we use to check for slide show focus
	if(m_uFocusTimer == 0)
		m_uFocusTimer = SetTimer(FOCUS_TIMER, 150, NULL);
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::StopTimers()
//
// 	Description:	This function is called to stop the timers when the viewer
//					is deactivated.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::StopTimers() 
{
	//	Start the timer we use to check for slide show focus
	if(m_uFocusTimer != 0)
	{
		KillTimer(m_uFocusTimer);
		m_uFocusTimer = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Unload()
//
// 	Description:	This function is called to unload the current presentation
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::Unload() 
{
	Lock();

	if(m_bHideTaskBar)
		CTMToolbox::SetTaskBarVisible(FALSE);

	//	Stop the timers
	StopTimers();

	//	Close all active dispatch interfaces
	ReleaseNew();
	ReleaseActive();

	m_strFilename.Empty();

	//	Notify the control
	if(m_pControl)
		m_pControl->OnPPFileChange(m_lId, m_strFilename);

	//	Allow access
	//
	//	NOTE:	We have to do this here because SetCurrent() locks access
	Unlock();

	SetCurrent();

	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::Unlock()
//
// 	Description:	This function is called to release the lock
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPowerPoint::Unlock()	
{ 
#if defined _USE_LOCK
	LeaveCriticalSection(&m_Lock);
#endif 
}

//==============================================================================
//
// 	Function Name:	CPowerPoint::UpdateState()
//
// 	Description:	This function is called to retrieve the current state of the
//					slide show viewer
//
// 	Returns:		The current view state
//
//	Notes:			None
//
//==============================================================================
short CPowerPoint::UpdateState() 
{
	short sState = GetState();

	//	Has the state changed?
	if(sState != m_sPPState)
	{
		//	Update the local member
		m_sPPState = sState;

		//	Notify the control
		if(m_pControl)
			m_pControl->OnPPStateChange(m_lId, m_sPPState);
	}
	
	return m_sPPState;	
}

/*----------------------------------------------------------------------------------------
	THIS MAY BE A MORE ACCURATE MEANS OF DETERMINING THE ANIMATION COUNT FOR A SLIDE
	BUT IT ADDS A BIG TIME HIT WHEN LOADING A NEW SLIDE THAT HAS A LOT OF SHAPES
------------------------------------------------------------------------------------------
long CPowerPoint::GetAnimations() 
{
	Shapes*		pIShapes = 0;
	Shape*		pIShape = 0;
	long		lShapes = 0;
	long		lAnimations = 0;
	VARIANT		vIndex;
	LPDISPATCH	lpDispatch;

CString M;

	//	Make sure we have a valid slide
	ASSERT(m_pISlide != 0);
	ASSERT(m_pISlide->m_lpDispatch != 0);
	if((m_pISlide == 0) || (m_pISlide->m_lpDispatch == 0)) return 0;
	
	//	Is the presentation have animations enabled?
	if((m_pISettings != 0) && (m_pISettings->GetShowWithAnimation() == 0))
		return 0;

	//	Set up the parameter variant
	VariantInit(&vIndex);
	vIndex.vt = VT_I4;

	while(1)
	{
		//	Get the shapes container
		pIShapes = new Shapes(m_pISlide->GetShapes());
		if(pIShapes->m_lpDispatch == 0) break;
			
		//	How many shapes are in this slide?
		lShapes = pIShapes->GetCount();

		//	Check each shape in the slide
		for(long i = 1; i <= lShapes; i++)
		{
			vIndex.lVal = i;

			//	Get the interface for the next shape
			if((lpDispatch = pIShapes->Item(vIndex)) != 0)
			{
				pIShape = new Shape(lpDispatch);
				ASSERT(pIShape);
				ASSERT(pIShape->m_lpDispatch != 0);

				if((pIShape->m_lpDispatch != 0) && GetAnimated(pIShape))
					lAnimations ++;

				RELEASE_INTERFACE(pIShape);
			}

		}


		//	We're done
		break;
	}

	RELEASE_INTERFACE(pIShapes);

M.Format("Shapes: %ld\nAnimations: %ld", lShapes, lAnimations);
MessageBox(M);

	return lAnimations;
}

*/


