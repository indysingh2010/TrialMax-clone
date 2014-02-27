//==============================================================================
//
// File Name:	frame.cpp
//
// Description:	This file contains member functions of the CFrame class.
//
// See Also:	frame.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	05-24-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <grabapp.h>
#include <frame.h>
#include <handler.h>
#include <grabctl.h>
#include <grabdefs.h>

#include <l_ocxerr.h>

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
extern CTMGrabApp NEAR	theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CFrame, CDialog)
	//{{AFX_MSG_MAP(CFrame)
	ON_WM_DESTROY()
	ON_BN_CLICKED(ID_CLIPBOARD, OnClipboard)
	ON_BN_CLICKED(ID_SAVE_AS, OnSaveAs)
	ON_BN_CLICKED(ID_STOP, OnStop)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CFrame::Capture()
//
// 	Description:	This function is called to start a capture session or 
//					perform a single shot capture
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CFrame::Capture()
{
	BOOL			bSuccessful = FALSE;
	LCaptureCtrl*	pCaptureCtrl = NULL;
	
	//	Get the bitmap's associated capture control
	if((pCaptureCtrl = (LCaptureCtrl*)m_bmpCapture.ScreenCapture()) != NULL)
	{
		//	Initialize the control and set the options
		if(Initialize(pCaptureCtrl) == TRUE)
		{
			//	What area are we supposed to capture?
			switch(m_sArea)
			{
				case TMGRAB_AREA_ACTIVE_WINDOW:		
					bSuccessful = CaptureActive();
					break;
													
				case TMGRAB_AREA_SELECTION:
					bSuccessful = CaptureSelection();
					break;
													
				case TMGRAB_AREA_FULL_SCREEN:
				default:							
					bSuccessful = CaptureScreen();
					break;
			
			}// switch(m_sArea)
		
		}// if(Initialize(pCaptureCtrl) == TRUE)

	}// if((pCaptureCtrl = (LCaptureCtrl*)m_bmpCapture.ScreenCapture()) != NULL)
	
	return bSuccessful;

}

//==============================================================================
//
// 	Function Name:	CFrame::CaptureActive()
//
// 	Description:	This function is called to capture the active window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CFrame::CaptureActive()
{
	int iResult;

	//	Is there a hotkey defined?
	if(m_sHotkey > 0)
	{
		while(1)
		{
			iResult = m_bmpCapture.ScreenCapture()->CaptureActiveWindow();

			switch(iResult)
			{
				case SUCCESS:		
				
					if(m_bOneShot == TRUE)
					{
						m_bmpCapture.ScreenCapture()->StopCapture();
						return TRUE;
					}
					else
					{
						break;	//	Successful - keep going ...
					}

				case ERROR_USER_ABORT:
				case SUCCESS_ABORT:

					((LCaptureCtrl*)(m_bmpCapture.ScreenCapture()))->SetHotKey(0);
					return TRUE;	//	User canceled - no error

				default:

					((LCaptureCtrl*)(m_bmpCapture.ScreenCapture()))->SetHotKey(0);
					return FALSE;	//	Error occured
			}

		}
	}
	else
	{
		return (m_bmpCapture.ScreenCapture()->CaptureActiveWindow() == SUCCESS);
	}
}

//==============================================================================
//
// 	Function Name:	CFrame::CaptureScreen()
//
// 	Description:	This function is called to capture the full screen
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CFrame::CaptureScreen()
{
	int iResult;

	//	Is there a hotkey defined?
	if(m_sHotkey > 0)
	{
		while(1)
		{
			iResult = m_bmpCapture.ScreenCapture()->CaptureFullScreen();

			switch(iResult)
			{
				case SUCCESS:		
				
					if(m_bOneShot == TRUE)
					{
						m_bmpCapture.ScreenCapture()->StopCapture();
						return TRUE;
					}
					else
					{
						break;	//	Successful - keep going ...
					}

				case ERROR_USER_ABORT:
				case SUCCESS_ABORT:

					((LCaptureCtrl*)(m_bmpCapture.ScreenCapture()))->SetHotKey(0);
					return TRUE;	//	User canceled - no error

				default:

					((LCaptureCtrl*)(m_bmpCapture.ScreenCapture()))->SetHotKey(0);
					return FALSE;	//	Error occured
			}

		}
	}
	else
	{
		return (m_bmpCapture.ScreenCapture()->CaptureFullScreen() == SUCCESS);
	}
}

//==============================================================================
//
// 	Function Name:	CFrame::CaptureSelection()
//
// 	Description:	This function is called to capture the user defined portion
//					of the screen
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CFrame::CaptureSelection()
{
	int iResult;

	//	Is there a hotkey defined?
	if(m_sHotkey > 0)
	{
		while(1)
		{
			iResult = m_bmpCapture.ScreenCapture()->CaptureArea();

			switch(iResult)
			{
				case SUCCESS:		
				
					if(m_bOneShot == TRUE)
					{
						return TRUE;
					}
					else
					{
						break;	//	Successful - keep going ...
					}

				case ERROR_USER_ABORT:
				case SUCCESS_ABORT:

					((LCaptureCtrl*)(m_bmpCapture.ScreenCapture()))->SetHotKey(0);
					return TRUE;	//	User canceled - no error

				default:

					((LCaptureCtrl*)(m_bmpCapture.ScreenCapture()))->SetHotKey(0);
					return FALSE;	//	Error occured
			}

		}
	}
	else
	{
		return (m_bmpCapture.ScreenCapture()->CaptureArea() == SUCCESS);
	}
}

//==============================================================================
//
// 	Function Name:	CFrame::CFrame()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFrame::CFrame(CWnd* pParent) : CDialog(CFrame::IDD, pParent)
{
	//{{AFX_DATA_INIT(CFrame)
	//}}AFX_DATA_INIT

	m_pErrors = 0;
	m_pControl = 0;
	m_hwndBitmap = NULL;
	m_bSilent = TMGRAB_DEFAULT_SILENT;
	m_bOneShot = TMGRAB_DEFAULT_ONESHOT;
	m_sArea = TMGRAB_DEFAULT_AREA;
	m_sHotkey = TMGRAB_DEFAULT_HOTKEY;
	m_sCancelKey = TMGRAB_DEFAULT_CANCELKEY;
	m_uLTLibsLoaded = 0;
}

//==============================================================================
//
// 	Function Name:	CFrame::~CFrame()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFrame::~CFrame()
{
}

//==============================================================================
//
// 	Function Name:	CFrame::Create()
//
// 	Description:	This function is called to create the frame window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CFrame::Create(CTMGrabCtrl* pControl, CErrorHandler* pErrors)
{
	BOOL bSuccessful = FALSE;

	m_pControl = pControl;
	m_pErrors  = pErrors;

	//	Create the window
	if(!CDialog::Create(CFrame::IDD))
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_CREATE_FRAME_FAILED);
		return FALSE;
	}

	//	Make the frame window the parent of the error handler
	ASSERT(pErrors);
	if(m_pErrors != NULL)
		m_pErrors->SetParent(m_hWnd);

	// Load the LeadTools libraries
	m_uLTLibsLoaded = LBase::LoadLibraries(LT_SCR | LT_KRN | LT_DIS | LT_DLG | LT_FIL);

	// Check if libraries successfull loaded.
	if(m_uLTLibsLoaded == (LT_SCR + LT_KRN + LT_DIS + LT_DLG + LT_FIL))
	{
		bSuccessful = TRUE;
		
		// Support GIF, JBIG, & TIF images formats
		WRPUNLOCKSUPPORT();
		LDialogBase::Initialize(DLG_INIT_COLOR);
	}
	else
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_LOAD_LEADLIBS_FAILED);
	}

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CFrame::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and their associated class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFrame::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CFrame)
	DDX_Control(pDX, ID_STOP, m_ctrlStop);
	DDX_Control(pDX, IDCANCEL, m_ctrlCancel);
	//}}AFX_DATA_MAP
	DDX_Control(pDX, IDC_BITMAP_PANEL, m_wndBitmapPanel);
}

//==============================================================================
//
// 	Function Name:	CFrame::Initialize()
//
// 	Description:	This function is called to initialize a capture session
//
// 	Returns:		TRUE if successful
//
//==============================================================================
BOOL CFrame::Initialize(LCaptureCtrl* pCaptureCtrl)
{
	BOOL bSuccessful = FALSE;
	
	ASSERT(pCaptureCtrl != NULL);
	
	//	Initialize the control and set the options
	if(pCaptureCtrl->Initialize(this) == TRUE)
	{
		//	Set the keystrokes
		if(m_sHotkey > 0)
		{
			if(pCaptureCtrl->SetHotKey(m_sHotkey) == TRUE)
			{
				if(m_sCancelKey > 0)
					bSuccessful = pCaptureCtrl->SetCancelKey(m_sCancelKey);
				else
					bSuccessful = pCaptureCtrl->SetCancelKey(VK_ESCAPE);				
			}
			
			//	Make the Stop button available
			if(IsWindow(m_ctrlStop.m_hWnd) && bSuccessful)
				m_ctrlStop.ShowWindow(SW_SHOW);
		}
		else
		{
			if(pCaptureCtrl->SetHotKey(0) == TRUE)
				bSuccessful = pCaptureCtrl->SetCancelKey(VK_ESCAPE);

			//	Hide the Stop button 
			if(IsWindow(m_ctrlStop.m_hWnd) && bSuccessful)
				m_ctrlStop.ShowWindow(SW_HIDE);
		}
		
		if(bSuccessful == FALSE)
		{
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_SET_OPTIONS_FAILED);
		}
	
	}
	else
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_INIT_SESSION_FAILED);
	}
	
	return bSuccessful;

}

//==============================================================================
//
// 	Function Name:	CFrame::OnCancel()
//
// 	Description:	This function is called when the user cancels the operation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFrame::OnCancel() 
{
	//	Hide the window without destroying the dialog
	ShowWindow(SW_HIDE);
}

//==============================================================================
//
// 	Function Name:	CFrame::OnCaptureImage()
//
// 	Description:	This function traps events fired by the LeadTools control
//					when it captures a new image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFrame::OnCaptureImage(LBitmapBase* pLBitmap, long CaptureNumber) 
{
	//	Transfer the captured image to the main control
	m_bmpCapture.SetHandle(pLBitmap->GetHandle());

	if(m_bmpCapture.IsAllocated())
		m_wndBitmap.SetHandle(m_bmpCapture.GetHandle());

	//	Are we in silent mode?
	if(m_bSilent)
	{
		//	Copy the image to the clipboard
		OnClipboard();
	}
	else
	{
		//	Make sure the window is visible
		ShowWindow(SW_SHOW);
	}

	//	Is this one shot?
	if(m_bOneShot == TRUE)
		m_bmpCapture.ScreenCapture()->StopCapture();

	//	Notify the container
	if(m_pControl)
		m_pControl->OnCaptureImage();
}

//==============================================================================
//
// 	Function Name:	CFrame::OnClipboard()
//
// 	Description:	This function is called to copy the current image to the
//					clipboard
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFrame::OnClipboard() 
{
    if(m_wndBitmap.CopyToClipboard(m_wndBitmap.GetBitmapWnd(), COPY2CB_EMPTY | COPY2CB_DIB | COPY2CB_DDB) == SUCCESS)
	{
		//	Hide the window if it's visible
		if(IsWindow(m_hWnd) && IsWindowVisible())
			ShowWindow(SW_HIDE);
	}
	else
	{
		//	Display an error message
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_CLIPBOARD_FAILED);
	}
}

//==============================================================================
//
// 	Function Name:	CFrame::OnDestroy()
//
// 	Description:	This function traps the WM_DESTROY message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFrame::OnDestroy() 
{
	//	Make sure the capture session is stopped
	Stop();

	//	Do the base class cleanup
	CDialog::OnDestroy();
}

//==============================================================================
//
// 	Function Name:	CFrame::OnInitDialog()
//
// 	Description:	This function traps the WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CFrame::OnInitDialog() 
{
	CRect rcBitmap;
	
	//	Do the base class initialization
	CDialog::OnInitDialog();
	
	m_wndBitmapPanel.GetWindowRect(&rcBitmap);
	ScreenToClient(&rcBitmap);
	
	m_hwndBitmap = m_wndBitmap.CreateWnd(GetSafeHwnd(), 0,
										 WS_VISIBLE | L_BS_CENTER | L_BS_PROCESSKEYBOARD, 
										 rcBitmap.left, rcBitmap.top, rcBitmap.Width(), rcBitmap.Height());	

	m_wndBitmap.EnableAutoErrorDisplay(FALSE);
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CFrame::OnSaveAs()
//
// 	Description:	This function is called when the user clicks on the Save As
//					button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFrame::OnSaveAs() 
{
	static OPENFILENAME	OpenFileName;
	L_CHAR				szFileName[_MAX_PATH] = "";
	char				szDir[_MAX_PATH];
	SAVEDLGPARAMS		FSParams;
	CString				strMsg;

	//	Get the current working directory
	GetCurrentDirectory(sizeof(szDir),szDir);

	OpenFileName.lStructSize       = sizeof(OPENFILENAME);
	OpenFileName.lpstrInitialDir   = szDir;
	OpenFileName.lpstrTitle        = "Save As";
	OpenFileName.lpstrFile         = szFileName;
	OpenFileName.nMaxFile          = sizeof(szFileName);
	OpenFileName.nFileOffset       = 0;
	OpenFileName.Flags             = 0;

	memset(&FSParams, 0, sizeof(FSParams));
	FSParams.pBitmap = NULL;
	FSParams.nBitsPerPixel = 24;
	FSParams.nFormat = FILE_BMP;
	FSParams.nQFactor = 2;
	FSParams.uSaveMulti = MULTIPAGE_OPERATION_OVERWRITE;
	FSParams.nPasses = 0;
	FSParams.bSaveWithStamp = FALSE;
	FSParams.uDlgFlags =	DLG_SAVE_SHOW_FILEOPTIONS_PROGRESSIVE     |
							DLG_SAVE_SHOW_FILEOPTIONS_MULTIPAGE       |
							DLG_SAVE_SHOW_FILEOPTIONS_STAMP           |
							DLG_SAVE_SHOW_FILEOPTIONS_QFACTOR         |
							DLG_SAVE_SHOW_FILEOPTIONS_J2KOPTIONS      |
							DLG_SAVE_SHOW_FILEOPTIONS_BASICJ2KOPTIONS;
							
   m_wndBitmap.DialogFile()->SetSaveParams(&FSParams);
   m_wndBitmap.DialogFile()->SetOpenFile(&OpenFileName);

   // Save the bitmap automatically from save dlg.
   if(!m_wndBitmap.DialogFile()->IsAutoProcessEnabled())
      m_wndBitmap.DialogFile()->EnableAutoProcess();
   
	if(m_wndBitmap.DialogFile()->DoModalSave(m_hWnd) != SUCCESS_DLG_OK)
	{
		strMsg.Format("Unable to create the LeadTools File Save dialog. Try using the clipboard instead");
		MessageBox(strMsg, "Error", MB_OK | MB_ICONEXCLAMATION);
	}

}

//==============================================================================
//
// 	Function Name:	CFrame::OnStop()
//
// 	Description:	This function is called when the user clicks on the Stop
//					button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFrame::OnStop() 
{
	//	Stop the operation
	Stop();
}

//==============================================================================
//
// 	Function Name:	CFrame::Save()
//
// 	Description:	This method is called to save the current capture to file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CFrame::Save(LPCTSTR pszName, short iFormat, short iBitsPerPixel, 
				  short iQuality, short iModify) 
{
	BOOL bSuccessful = FALSE;

	if(m_wndBitmap.GetHandle() != NULL)
	{
		//	Enable error messages
		if((m_pErrors != 0) && m_pErrors->IsEnabled())
			m_wndBitmap.EnableAutoErrorDisplay(TRUE);

		//	Save the image
		if(m_wndBitmap.Save((L_TCHAR*)pszName, iFormat, iBitsPerPixel, iQuality, iModify) == SUCCESS)
			bSuccessful = TRUE;

		//	Turn LeadTools messages back off
		m_wndBitmap.EnableAutoErrorDisplay(FALSE);

	
	}
	
	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CFrame::Stop()
//
// 	Description:	This function is called to stop a multiple capture session
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CFrame::Stop()
{
	//	Do we have an active capture?
	if(m_bmpCapture.ScreenCapture()->IsCaptureActive())
	{
		m_bmpCapture.ScreenCapture()->StopCapture();
	}

	//	Hide the window if it's visible
	if(IsWindow(m_hWnd) && IsWindowVisible())
	{
		ShowWindow(SW_HIDE);
	}

	return TRUE;
}
