//==============================================================================
//
// File Name:	app.cpp
//
// Description:	This file contains member functions of the CApp class.
//
// See Also:	app.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-17-97	1.00		Original Release
//	12-07-97	1.10		Modified keyboard hook to support playlist cueing
//	02-08-98	1.10		Added ResetHook()
//	01-30-01	4.10		Fixed Ctrl-End crash
//	01-29-2014	7.27		Added function and configration for gestures
//	02-21-2014	7.29		Toolbar will be displayed if there is no bar code
//							i.e. blank presentation.
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <app.h>
#include <document.h>
#include <frame.h>
#include <view.h>
#include <tmcmdlin.h>
#include <dbdefs.h>
#include <splash.h>
#include <afxwin.h>
#include <enumdisplays.h>
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
//	REVISION INFORMATION
//------------------------------------------------------------------------------
//
//	_wVerMajor:	Major version identifer is changed when significant changes
//				have been made in the entire suite of controls and applications
//				All controls and applications should ALWAYS have the same
//				major version identifier
//	_wVerMinor:	Minor version identifier is changed when changes have been made
//				to a control and/or application that would render it unusable
//				with the existing release. All controls and applications will
//				have the same minor revision identifier when bundled as a new
//				release but individual controls and/or applications may be
//				upgraded between releases.
//	_wVerBuild:	Build version identifier is used to track controls and 
//				applications during development. This identifier is updated
//				on a weekly basis.
//
//	NOTE:		For ActiveX controls, the associated object definition library
//				(.odl) file MUST be updated when the major or minor version
//				identifiers are changed.				
//
//	REV 5.0:	Updated application to use Trialmax 5.0 controls
//				
//	REV 6.1.0:	Moved all version identifiers to Version resource
//				
//------------------------------------------------------------------------------
const WORD	_wVerMajor = 7;	//	Used for database version checking
const WORD	_wVerMinor = 0;	//	Used for database version checking

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
CApp theApp;

// This identifier was generated to be statistically unique for your app.
// You may change it if you prefer to choose a specific identifier.

// {AA005143-16FD-11D1-B02E-008029EFD140}
static const CLSID clsid =
{ 0xaa005143, 0x16fd, 0x11d1, { 0xb0, 0x2e, 0x0, 0x80, 0x29, 0xef, 0xd1, 0x40 } };

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CApp, CWinApp)
	//{{AFX_MSG_MAP(CApp)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

#if defined _USE_DAO36_
STDAPI Access2KStringAllocCallback(DWORD dwLen, DWORD pData, void** ppv)
{
	LPTSTR lpsz;
	CString* pstr = (CString*)pData;

	dwLen++;

	TRY
	{
		//Allocate twice the space needed so that DAO does not overwrite the buffer
		lpsz = pstr->GetBufferSetLength(2*dwLen/sizeof(TCHAR));
		*ppv = (void*)(dwLen > 0 ? lpsz : NULL);
	}
	CATCH_ALL(e)
	{
		e->Delete();
		return E_OUTOFMEMORY;
	}
	END_CATCH_ALL

	return S_OK;
}  
#endif

//==============================================================================
//
// 	Function Name:	CApp::ActivateInstance()
//
// 	Description:	This function is called to activate the specified instance
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::ActivateInstance(HWND hMainWnd) 
{
	CWnd* pMainWnd = NULL;
	
	if((pMainWnd = CWnd::FromHandle(hMainWnd)) != NULL)
	{
		//	Restore the window if it's minimized
		if(pMainWnd->IsIconic())
			pMainWnd->ShowWindow(SW_RESTORE);

		//	Bring the previous instance to the foreground
		pMainWnd->SetForegroundWindow();
	}

}

//==============================================================================
//
// 	Function Name:	CApp::CMainView()
//
// 	Description:	This is the constructor for CMainView objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CApp::CApp()
{
	m_pFrame = 0;
	m_strKBBuffer.Empty();
	m_bHook = FALSE;
	m_bSilent = FALSE;
	m_bEscapeHook = FALSE;
	m_bMouseHook = FALSE;
	m_bAlternate = FALSE;
	m_bDualMonitors = FALSE;
	m_iPrimaryWidth = 0;
	m_iPrimaryHeight = 0;
	m_iSecondaryWidth = 0;
	m_iSecondaryHeight = 0;
	m_sHookState = WAITING_START;
	m_pSplashBox = 0;
	m_cPrimary = 0;
	m_cAlternate = 0;
	m_cVK = 0;
	bSetDisplay = FALSE;
	memset(m_szKey, 0, sizeof(m_szKey));
}

//==============================================================================
//
// 	Function Name:	CApp::DoSplashBox()
//
// 	Description:	This function is called to control the visibility of the
//					application's splash box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::DoSplashBox(BOOL bShow)
{
	//	Are we supposed to be showing the box?
	if(bShow)
	{
		if(m_pSplashBox == 0)
			m_pSplashBox = new CSplashBox();
		m_pSplashBox->RedrawWindow();
	}
	else
	{
		if(m_pSplashBox)
		{
			delete m_pSplashBox;
			m_pSplashBox = 0;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CApp::EnableEscapeHook()
//
// 	Description:	This function is called to enable/disable the escape key
//					hook.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::EnableEscapeHook(BOOL bEnable)
{
	m_bEscapeHook = bEnable;
}

//==============================================================================
//
// 	Function Name:	CApp::EnableHook()
//
// 	Description:	This function is called to enable/disable the keyboard hook.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::EnableHook(BOOL bEnable)
{
	m_bHook      = bEnable;
	m_bAlternate = FALSE;
	m_sHookState = WAITING_START;
	m_strKBBuffer.Empty();
}

//==============================================================================
//
// 	Function Name:	CApp::EnableMouseHook()
//
// 	Description:	This function is called to enable/disable the mouse hook.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::EnableMouseHook(BOOL bEnable)
{
	m_bMouseHook = bEnable;
}

//==============================================================================
//
// 	Function Name:	CApp::ExitInstance()
//
// 	Description:	Called when the application instance is being terminated
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CApp::ExitInstance() 
{
	//	Make sure the task bar is visible. 
	CTMToolbox::SetTaskBarVisible(TRUE);
	
	return CWinApp::ExitInstance();
}

//==============================================================================
//
// 	Function Name:	CApp::GetMonitorInfo()
//
// 	Description:	This function is called to iterate the system's display
//					devices to determine if a secondary monitor is available.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CApp::GetMonitorInfo()
{
	DISPLAY_DEVICE			 ddEnum;
	DEVMODE					 devMode;

	//	Get the screen extents for the primary display
	m_iPrimaryWidth  = GetSystemMetrics(SM_CXSCREEN);
	m_iPrimaryHeight = GetSystemMetrics(SM_CYSCREEN);

	ZeroMemory(&ddEnum, sizeof(ddEnum));
    ddEnum.cb = sizeof(ddEnum);

	for(int i = 0; EnumDisplayDevices(NULL, i, &ddEnum, 0); i++)
	{
		//	Is this device attached to the desktop?
		if((ddEnum.StateFlags & DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) != 0)
		{
			//	Ignore the primary monitor
			if((ddEnum.StateFlags & DISPLAY_DEVICE_PRIMARY_DEVICE) == 0)
			{
				// get information about the display's position and the current display mode
				ZeroMemory(&devMode, sizeof(devMode));
				devMode.dmSize = sizeof(devMode);

				if (EnumDisplaySettings(ddEnum.DeviceName, ENUM_CURRENT_SETTINGS, &devMode) == FALSE)
					EnumDisplaySettings(ddEnum.DeviceName, ENUM_REGISTRY_SETTINGS, &devMode);
			
				if((devMode.dmPelsWidth > 0) && (devMode.dmPelsHeight > 0))
				{
					m_bDualMonitors = TRUE;
					m_iSecondaryWidth  = devMode.dmPelsWidth;
					m_iSecondaryHeight = devMode.dmPelsHeight;
				}

			}
		
		}

	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CApp::GetPrevInstance()
//
// 	Description:	This function is called to determine if this is the first
//					instance of the application
//
// 	Returns:		Pointer to main frame of active instance
//
//	Notes:			None
//
//==============================================================================
HWND CApp::GetPrevInstance() 
{
	HWND	hMainWnd = NULL;

	EnumWindows(OnEnumWindow, (LPARAM)(&hMainWnd));

	return hMainWnd;
}

//==============================================================================
//
// 	Function Name:	CApp::InitInstance()
//
// 	Description:	This function performs the application's initialization.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CApp::InitInstance()
{
	HWND	hwndPrevious = NULL;
	char	szFolder[512];
	char*	pToken;

	//unlink("TrialMaxDebugLog.txt");

	//	Only allow one instance of the application
	if((hwndPrevious = GetPrevInstance()) != NULL)
	{
		ParseCommandLine(m_TMCmdLineInfo);
		StoreCommandLine(m_TMCmdLineInfo);
		
		//	Bring the previous instance to the front
		ActivateInstance(hwndPrevious);

		//	Send a message to the previous instance
		PostMessage(hwndPrevious, WM_NEWINSTANCE, 0, 0);

		return FALSE;
	
	}// if((hwndPrevious = GetPrevInstance()) != NULL)

	//	Get the application folder
	lstrcpyn(szFolder, m_pszHelpFilePath, sizeof(szFolder));
	if((pToken = strrchr(szFolder, '\\')) != 0)
		*(pToken + 1) = 0;
	m_strAppFolder = szFolder;
	m_strAppFolder.MakeLower();

	//	Check for availability of a secondary monitor
	GetMonitorInfo();

	// Initialize OLE libraries
	if(!AfxOleInit())
	{
		AfxMessageBox(IDP_OLE_INIT_FAILED);
		return FALSE;
	}

	//	Turn off the server busy dialogs
	AfxOleGetMessageFilter()->EnableNotRespondingDialog(FALSE);
	AfxOleGetMessageFilter()->EnableBusyDialog(FALSE);

	//	Enable this application as an ActiveX container
	AfxEnableControlContainer();

	//	Enable use of 3d controls
	//Enable3dControls();			

	//	Parse the command line options
	ParseCommandLine(m_TMCmdLineInfo);
	m_bSilent = m_TMCmdLineInfo.m_bSilent;

//#if defined _USE_DAO36_
//	AfxGetModuleState()->m_dwVersion = 0x0601; 
//#else
//	AfxMessageBox("NO ACCESS 2000 SUPPORT");
//#endif

	//	Show the splash box
	if(!m_bSilent)
		DoSplashBox(TRUE);

	//	Register the application's document templates.  
	CSingleDocTemplate* pDocTemplate;
	pDocTemplate = new CSingleDocTemplate(
		IDR_MAINFRAME,
		RUNTIME_CLASS(CTMDocument),
		RUNTIME_CLASS(CMainFrame),       
		RUNTIME_CLASS(CMainView));
	AddDocTemplate(pDocTemplate);

	//	Connect the COleTemplateServer to the document template. The
	//  COleTemplateServer creates new documents on behalf of requesting OLE
	//  containers by using information specified in the document template.
	m_Server.ConnectTemplate(clsid, pDocTemplate, TRUE);

	// Check to see if launched as OLE server
	if(m_TMCmdLineInfo.m_bRunEmbedded || m_TMCmdLineInfo.m_bRunAutomated)
	{
		// Register all OLE server (factories) as running.  This enables the
		//  OLE libraries to create objects from other applications.
		COleTemplateServer::RegisterAll();

		//	Application was run with /Embedding or /Automation.  Don't show the
		//  main window in this case.
		return TRUE;
	}

	//	When a server application is launched stand-alone, it is a good idea
	//  to update the system registry in case it has been damaged.
	m_Server.UpdateRegistry(OAT_DISPATCH_OBJECT);
	COleObjectFactory::UpdateRegistryAll();

	//	Dispatch commands specified on the command line. This call will 
	//	create the frame and view windows
	if(!ProcessShellCommand(m_TMCmdLineInfo))
		return FALSE;

	//	Save a casted pointer to the main frame
	m_pFrame = (CMainFrame*)m_pMainWnd;

	//	Set the window title so that we can check to see if this is the
	//	first instance
	m_pFrame->SetWindowText(TRIALMAX_WINDOW_TITLE);
	
	// Now maximize the window
	if((m_pFrame->GetUseSecondaryMonitor() == TRUE) && (m_bDualMonitors == TRUE))
	{
		m_pFrame->SetWindowPos(&CWnd::wndTopMost, m_iPrimaryWidth, 0, 
							   m_iSecondaryWidth, m_iSecondaryHeight,
							   SWP_SHOWWINDOW | SWP_FRAMECHANGED);
	}
	else
	{
		m_pFrame->SetWindowPos(&CWnd::wndTopMost, 0, 0, m_iPrimaryWidth, m_iPrimaryHeight,
								SWP_SHOWWINDOW | SWP_FRAMECHANGED);
	}
	SetForegroundWindow(m_pFrame->m_hWnd);
	SetActiveWindow(m_pFrame->m_hWnd);
	SetFocus(m_pFrame->m_hWnd);
	m_pFrame->SetWindowPos(&CWnd::wndNoTopMost, 0, 0, 0, 0,
				           SWP_NOREDRAW | SWP_NOSIZE | SWP_NOMOVE);

	//	Get the keyboard hook characters from the main view
	m_cVK = m_pFrame->GetVKChar();
	m_cPrimary = m_pFrame->GetPrimaryBarcodeChar();
	m_cAlternate = m_pFrame->GetAlternateBarcodeChar();

	//	Enable the keyboard hook
	EnableHook(TRUE);
	//AllocConsole();

	//	Get rid of the splash box
	if(!m_bSilent)
		DoSplashBox(FALSE);

	if(!m_TMCmdLineInfo.m_strBarcode.IsEmpty())
	{
		m_pFrame->SetLoadPage((int)(m_TMCmdLineInfo.m_lPageNumber));
		m_pFrame->SetLoadLine(m_TMCmdLineInfo.m_iLineNumber);
		m_pFrame->LoadFromBarcode(m_TMCmdLineInfo.m_strBarcode, TRUE, FALSE);
	} else {
		// show toolbar on blank presentation (Ctrl+F5)
		CMainView* pView = (CMainView*)m_pFrame->GetActiveView();
		pView->BlankPresentationToolbar();
	}

	//FreeConsole();
	// WM_GESTURE configuration
	// uncomment below line to endable 2 finger gertures on whole presentaion
	//InitWmGesture(m_pFrame->m_hWnd);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CApp::LockInstance()
//
// 	Description:	This function is called to make this the active instance
//					of the application.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::LockInstance(HWND hMainWnd)
{
	if(IsWindow(hMainWnd))
		SetWindowLong(hMainWnd, GWL_USERDATA, GetInstanceKey());
}

//==============================================================================
//
// 	Function Name:	CApp::OnEnumWindow()
//
// 	Description:	This function hook the keyboard and assemble barcodes.
//
// 	Returns:		TRUE if handled.
//
//	Notes:			None
//
//==============================================================================
BOOL CALLBACK CApp::OnEnumWindow(HWND hWnd, LPARAM lpParam)
{
	//	Is this the TrialMax main window ?
	if(GetWindowLong(hWnd, GWL_USERDATA) == GetInstanceKey())
	{
		//	Return this handle to the caller
		*((HWND*)lpParam) = hWnd;
		
		//	Stop here
		return FALSE;
	}
	else
	{
		//	Keep going
		return TRUE;
	} 

};

//==============================================================================
//
// 	Function Name:	CApp::PreTranslateMessage()
//
// 	Description:	This function hook the keyboard and assemble barcodes.
//
// 	Returns:		TRUE if handled.
//
//	Notes:			None
//
//==============================================================================
BOOL CApp::PreTranslateMessage(MSG* pMsg) 
{
	char cKey;

	//	Are we hooking the keyboard?
	if(!m_bHook)
	{
		//	Are we hooking the escape key?
		if(m_bEscapeHook)
		{
			switch(pMsg->message)
			{
				case WM_KEYDOWN:
				case WM_KEYUP:
				case WM_CHAR:

					if(pMsg->wParam == VK_ESCAPE)
						return TRUE;
					else
						break;

				default:
					break;
			}
		}
		return CWinApp::PreTranslateMessage(pMsg);
	}

	switch(pMsg->message)
	{
		//	Is this a mouse message?
		case WM_LBUTTONDOWN:
		case WM_LBUTTONUP:
		case WM_RBUTTONDOWN:
		case WM_RBUTTONUP:

			//	Is the mouse hook enabled?
			if(m_bMouseHook)
			{
				//	Let the view process this message first
				if(m_pFrame->ProcessMouseMessage(pMsg))
					return TRUE;
				else
					return CWinApp::PreTranslateMessage(pMsg);
			}
			else
			{
				return CWinApp::PreTranslateMessage(pMsg);
			}

		case WM_CHAR:
		
			cKey = (TCHAR)(pMsg->wParam);

			// check if specail characters, not supported, entered.
			// for windows8 onscreen keyboard
			if (cKey < 0 )
			{
				return false;
			}

			//	Is the buffer full?
			if(m_strKBBuffer.GetLength() >= MAXLEN_KBBUFFER)
			{
				m_sHookState = WAITING_START;
				m_strKBBuffer.Empty();
			}

			//	Are we waiting for a start character?
			else if(m_sHookState == WAITING_START)
			{
				//	Is this the leading character used to indicate virtual keys?
				if(cKey == m_cVK)
				{
					m_strKBBuffer.Empty();
					m_sHookState = WAITING_VIRTUAL_CODE;
					return TRUE;
				}

				//	Is this the leading character used to indicate a barcode?
				else if(toupper(cKey) == m_cPrimary)
				{
					m_strKBBuffer.Empty();
					m_sHookState = WAITING_MEDIA_DELIMITER;
					m_bAlternate = FALSE;
					return TRUE;
				}

				//	Is this the leading character used to indicate an
				//	alternate barcode?
				else if(toupper(cKey) == m_cAlternate)
				{
					m_strKBBuffer.Empty();
					m_sHookState = WAITING_MEDIA_DELIMITER;
					m_bAlternate = TRUE;
					return TRUE;
				}

				//	If this is a digit we can assume the user is entering a page
				//	number to advance to
				else if(isdigit(cKey))
				{
					m_szKey[0] = cKey;
					m_strKBBuffer += m_szKey;
					m_sHookState = WAITING_PAGE_NUMBER;
					return TRUE;
				}
			
			}

			//	Are we waiting on the rest of the media id?
			else if(m_sHookState == WAITING_MEDIA_DELIMITER)
			{
				//	Add the character to the buffer
				m_szKey[0] = cKey;
				m_strKBBuffer += m_szKey;

				//	Is this the delimiter?
				if(cKey == TMAX_BARCODE_DELIMITER)
					m_sHookState = WAITING_SECONDARY_DELIMITER;

				return TRUE;
			}

			//	Are we waiting for the rest of the secondary identifier?
			else if(m_sHookState == WAITING_SECONDARY_DELIMITER)
			{
				//	Add the character to the buffer
				m_szKey[0] = cKey;
				m_strKBBuffer += m_szKey;

				//	Is this the delimiter?
				if(cKey == TMAX_BARCODE_DELIMITER)
					m_sHookState = WAITING_RETURN;

				return TRUE;
			}

			//	Are we waiting on the rest of the tertiary id?
			else if(m_sHookState == WAITING_RETURN)
			{
				m_szKey[0] = cKey;
				m_strKBBuffer += m_szKey;
				return TRUE;
			}

			//	Are we waiting on the virtual key code?
			else if(m_sHookState == WAITING_VIRTUAL_CODE)
			{
				if(isdigit(cKey))
				{ 
					m_szKey[0] = cKey;
					m_strKBBuffer += m_szKey;
					return TRUE;
				}
			}

			//	Are we waiting on the page number?
			else if(m_sHookState == WAITING_PAGE_NUMBER)
			{
				if(isdigit(cKey))
				{ 
					m_szKey[0] = cKey;
					m_strKBBuffer += m_szKey;
					return TRUE;
				}
			}

			//	Is this one of the special command keystrokes?
			if(m_pFrame->ProcessCommandKey(cKey))
			{
				m_strKBBuffer.Empty();
				m_sHookState = WAITING_START;
				return TRUE;
			}
			else
			{
				//	If we get this far the barcode must be invalid
				m_strKBBuffer.Empty();
				m_sHookState = WAITING_START;
			}

			return TRUE;

		//	We have to watch for the return key here because WM_CHAR is not
		//	sent for the return key
		case WM_KEYDOWN:

			//	What key has been pressed?
			switch(pMsg->wParam)
			{
				case VK_RETURN:
				
					//	Are we processing a barcode?
					if(m_sHookState == WAITING_MEDIA_DELIMITER ||
					   m_sHookState == WAITING_SECONDARY_DELIMITER ||
					   m_sHookState == WAITING_RETURN)
					{
						m_pFrame->LoadFromBarcode(m_strKBBuffer, TRUE, m_bAlternate);
					}
					//	Load a new page if this is a page number
					else if(m_sHookState == WAITING_PAGE_NUMBER)
					{
						long lPage = atol(m_strKBBuffer);
						m_pFrame->LoadPageFromKeyboard(lPage);
					}
					
					//	Process the virtual key if that's what we're waiting for
					else if(m_sHookState == WAITING_VIRTUAL_CODE)
					{
						m_pFrame->ProcessVirtualKey(atoi(m_strKBBuffer));
					}
					
					//	Clear the buffer
					m_strKBBuffer.Empty();
					m_sHookState = WAITING_START;

					return TRUE;

				default:

					//	Check to see if this is a hotkey
					if(m_pFrame->ProcessVirtualKey(pMsg->wParam))
						return TRUE;
					else
						return CWinApp::PreTranslateMessage(pMsg);

			}

		default:

			return CWinApp::PreTranslateMessage(pMsg);

	} // switch(pMsg->message)

}

//==============================================================================
//
// 	Function Name:	CApp::ResetHook()
//
// 	Description:	This function is called to reset the keyboard hook.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::ResetHook()
{
	m_sHookState = WAITING_START;
	m_bAlternate = FALSE;
	m_strKBBuffer.Empty();
}

//==============================================================================
//
// 	Function Name:	CApp::StoreCommandLine()
//
// 	Description:	This function is called to store the current command line
//					parameters in an ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::StoreCommandLine(CTMCommandLineInfo& rCmdLine)
{
	SCommandLine	CmdLine;
	CTMIni			Ini;
	char			szIniFile[256];
	char*			pToken;

	//	Create a command line file in the application directory
	lstrcpyn(szIniFile, m_pszHelpFilePath, sizeof(szIniFile));
	if((pToken = strrchr(szIniFile, '\\')) != 0)
		*(pToken + 1) = 0;
	lstrcat(szIniFile, DEFAULT_COMMANDLINE_FILE);

	//	Open the ini file
	Ini.Open(szIniFile);

	//	Set the command line information
	ZeroMemory(&CmdLine, sizeof(CmdLine));
	CmdLine.lPageNumber = rCmdLine.m_lPageNumber;
	CmdLine.iLineNumber = rCmdLine.m_iLineNumber;
	lstrcpyn(CmdLine.szBarcode, rCmdLine.m_strBarcode, sizeof(CmdLine.szBarcode));
	lstrcpyn(CmdLine.szCaseFolder, rCmdLine.m_strCaseFolder, sizeof(CmdLine.szCaseFolder));

	//	Write the options to the ini file
	Ini.WriteCommandLine(&CmdLine);
}

//==============================================================================
//
// 	Function Name:	CApp::UnlockInstance()
//
// 	Description:	This function is called to release the instance lock when
//					this application is terminating
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::UnlockInstance()
{
	if((m_pMainWnd != NULL) && (IsWindow(m_pMainWnd->m_hWnd) != 0))
		SetWindowLong(m_pMainWnd->m_hWnd, GWL_USERDATA, 0);
}



//==============================================================================
//
// 	Function Name:	CApp::InitWmGesture()
//
// 	Description:	Set configration for WM_GESTURE
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CApp::InitWmGesture(HWND hWnd)
{
	DWORD panWant = GC_PAN
                  | GC_PAN_WITH_SINGLE_FINGER_VERTICALLY
                  | GC_PAN_WITH_SINGLE_FINGER_HORIZONTALLY
                  | GC_PAN_WITH_INERTIA;
    GESTURECONFIG gestureConfig[] =
    {
        { GID_PAN, panWant, GC_PAN_WITH_GUTTER },
        { GID_ZOOM, GC_ZOOM, 0 }
        //{ GID_ROTATE, GC_ROTATE, 0},
        //{ GID_TWOFINGERTAP, GC_TWOFINGERTAP, 0 },
        //{ GID_PRESSANDTAP, GC_PRESSANDTAP, 0 }
    };
    SetGestureConfig(hWnd, 0, 2, gestureConfig, sizeof(GESTURECONFIG));
}
