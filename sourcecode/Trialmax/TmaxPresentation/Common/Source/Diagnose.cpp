//==============================================================================
//
// File Name:	diagnose.cpp
//
// Description:	This file contains member functions of the CDiagnostics class
//
// See Also:	diagnose.h
//
// Copyright FTI Consulting - All Rights Reserved
//
//==============================================================================
//	Date		Revision    Description
//	07-15-2002	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <diagnose.h>

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
BEGIN_MESSAGE_MAP(CDiagnostics, CDialog)
	//{{AFX_MSG_MAP(CDiagnostics)
	ON_WM_SIZE()
	ON_LBN_DBLCLK(IDC_LIST, OnDoubleClick)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessage::CDiagMessage()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CDiagMessage::CDiagMessage()
{
	m_strTime.Empty();
	m_strDate.Empty();
	m_strText.Empty();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessage::~CDiagMessage()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CDiagMessage::~CDiagMessage()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::Add()
//
//	Parameters:		pError - A pointer to the object being added
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to add an object to the list
//
//------------------------------------------------------------------------------
BOOL CDiagMessages::Add(CDiagMessage* pError)
{
	ASSERT(pError);
	if(!pError)
		return FALSE;

	try
	{
		//	Add at the head of the list
		AddHead(pError);
		return TRUE;

	}
	catch(...)
	{
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::CDiagMessages()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CDiagMessages::CDiagMessages()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::~CDiagMessages()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor 
//
//------------------------------------------------------------------------------
CDiagMessages::~CDiagMessages()
{
	//	Flush the list and destroy the objects
	Flush(TRUE);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::Find()
//
//	Parameters:		pError - pointer to object being located
//
// 	Return Value:	The position of the object in the list
//
// 	Description:	This function is called to determine the position of the
//					specified object in the list
//
//------------------------------------------------------------------------------
POSITION CDiagMessages::Find(CDiagMessage* pError)
{
	return (CObList::Find(pError));
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::First()
//
//	Parameters:		None
//
// 	Return Value:	The first object pointer in the list if any
//
// 	Description:	This function is called to get the first object pointer
//					in the list
//
//------------------------------------------------------------------------------
CDiagMessage* CDiagMessages::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CDiagMessage*)GetNext(m_NextPos);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::Flush()
//
//	Parameters:		bDelete - TRUE to force deallocation of all objects
//
// 	Return Value:	None
//
// 	Description:	This function is called to remove all objects from the
//					list
//
//------------------------------------------------------------------------------
void CDiagMessages::Flush(BOOL bDelete)
{
	CDiagMessage* pError;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pError = (CDiagMessage*)GetNext(m_NextPos)) != 0)
				delete pError;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::Last()
//
//	Parameters:		None
//
// 	Return Value:	The last object pointer in the list if any
//
// 	Description:	This function is called to get the last object pointer
//					in the list
//
//------------------------------------------------------------------------------
CDiagMessage* CDiagMessages::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CDiagMessage*)GetPrev(m_PrevPos);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::Next()
//
//	Parameters:		None
//
// 	Return Value:	The next object pointer in the list if any
//
// 	Description:	This function is called to get the next object pointer
//					in the list
//
//------------------------------------------------------------------------------
CDiagMessage* CDiagMessages::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CDiagMessage*)GetNext(m_NextPos);
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::Prev()
//
//	Parameters:		None
//
// 	Return Value:	The previous object pointer in the list if any
//
// 	Description:	This function is called to get the previous object pointer
//					in the list
//
//------------------------------------------------------------------------------
CDiagMessage* CDiagMessages::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CDiagMessage*)GetPrev(m_PrevPos);
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::Remove()
//
//	Parameters:		pError - pointer to the object being removed
//					bDelete - TRUE to deallocate the object after removal
//
// 	Return Value:	None
//
// 	Description:	This function is called to remove an object from the list
//
//------------------------------------------------------------------------------
void CDiagMessages::Remove(CDiagMessage* pError, BOOL bDelete)
{
	POSITION Pos = Find(pError);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pError;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagMessages::RemoveTail()
//
//	Parameters:		bDelete - TRUE to deallocate the object after removal
//
// 	Return Value:	None
//
// 	Description:	This function is called to remove the object at the end
//					of the list.
//
//------------------------------------------------------------------------------
void CDiagMessages::RemoveTail(BOOL bDelete)
{
	POSITION		Pos = GetTailPosition();
	CDiagMessage*	pError;

	if(Pos != NULL)
	{
		pError = (CDiagMessage*)GetAt(Pos);
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete && pError)
			delete pError;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::AddMessage()
//
//	Parameters:		pMessage - pointer to error being added to the list
//
// 	Return Value:	None
//
// 	Description:	This function is called to add an error to the list box
//
//------------------------------------------------------------------------------
void CDiagnostics::AddMessage(CDiagMessage* pMessage)
{
	CString	strMsg;

	ASSERT(pMessage);

	if(IsWindow(m_ctrlMessages.m_hWnd))
	{
		//	Format the error message
		strMsg.Format("%s  %s", pMessage->m_strTime, pMessage->m_strText);

		//	Add the error to the bottom of the list
		m_ctrlMessages.AddString(strMsg);

		//	Have we reached the limit?
		while(IsWindow(m_ctrlMessages.m_hWnd) && 
			m_ctrlMessages.GetCount() > DIAGNOSTICS_MAX_MESSAGES)
			m_ctrlMessages.DeleteString(0);
		
		m_ctrlMessages.SetCurSel(m_ctrlMessages.GetCount() - 1);
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::CDiagnostics()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CDiagnostics::CDiagnostics(CWnd* pParent) : CDialog(CDiagnostics::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDiagnostics)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	m_bLogEnabled = FALSE;
	m_strLogFilename.Empty();
	m_strLogFolder.Empty();
	m_hNotify = 0;
	m_uNotify = 0;
	ZeroMemory(&m_Time, sizeof(m_Time));
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::~CDiagnostics()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CDiagnostics::~CDiagnostics()
{
	m_Messages.Flush(TRUE);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::Create()
//
//	Parameters:		None
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the diagnostics window
//
//------------------------------------------------------------------------------
BOOL CDiagnostics::Create(CWnd* pParent)
{
	return CDialog::Create(CDiagnostics::IDD, pParent);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::DoDataExchange()
//
//	Parameters:		pDX - pointer to data exchange object
//
// 	Return Value:	None
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and the associated class members
//
//------------------------------------------------------------------------------
void CDiagnostics::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDiagnostics)
	DDX_Control(pDX, IDC_LIST, m_ctrlMessages);
	//}}AFX_DATA_MAP
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::LogMessage()
//
//	Parameters:		pMessage - pointer to error to be logged
//
// 	Return Value:	None
//
// 	Description:	This function will add an entry to the log file
//
//------------------------------------------------------------------------------
void CDiagnostics::LogMessage(CDiagMessage* pMessage)
{
	CString strFilespec;
	FILE*	pFile = NULL;

	ASSERT(pMessage);

	//	If no filename is specified construct one from the current time
	if(m_strLogFilename.IsEmpty())
		strFilespec.Format("%sEL%02d%02d%04d.txt", m_strLogFolder, m_Time.wMonth, m_Time.wDay, m_Time.wYear);
	else
		strFilespec.Format("%s%s", m_strLogFolder, m_strLogFilename);

	if(fopen_s(&pFile, strFilespec, "at") == 0)
	{
		ASSERT(pFile != NULL);
		
		fprintf(pFile, "%s %s %s\n", pMessage->m_strDate, pMessage->m_strTime, pMessage->m_strText);
		fclose(pFile);
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::OnCancel()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is called when the user closes the dialog
//					box using the system menu in the title bar.
//
//------------------------------------------------------------------------------
void CDiagnostics::OnCancel()
{
	//	Hide the window without actually destroying it
	ShowWindow(SW_HIDE);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::OnDoubleClick()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is handles double click events fired by the
//					list box.
//
//------------------------------------------------------------------------------
void CDiagnostics::OnDoubleClick() 
{
	m_Messages.Flush(TRUE);
	m_ctrlMessages.ResetContent();	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::OnInitDialog()
//
//	Parameters:		None
//
// 	Return Value:	TRUE for default focus assignment
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
//------------------------------------------------------------------------------
BOOL CDiagnostics::OnInitDialog() 
{
	//	Do the base class processing first
	CDialog::OnInitDialog();
		
	m_ctrlMessages.SetHorizontalExtent(1024);

	//	Set the list box
	SetListPosition();

	return TRUE;  
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::OnSize()
//
//	Parameters:		nType - Windows size type identifier
//					cx	  - new window width
//					cy    - new window height
//
// 	Return Value:	None
//
// 	Description:	This function handles all WM_SIZE messages
//
//------------------------------------------------------------------------------
void CDiagnostics::OnSize(UINT nType, int cx, int cy) 
{
	//	Do the base class processing
	CDialog::OnSize(nType, cx, cy);
	
	//	Reset the list box
	SetListPosition();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::Report()
//
//	Parameters:		lpUser    - name used to report errors
//					lpFormat  - error message printf style format specification
//					...		  - variable length parameter string
//
// 	Return Value:	None
//
// 	Description:	This function handle the error reported by the user
//
//------------------------------------------------------------------------------
void CDiagnostics::Report(LPCSTR lpUser, LPCSTR lpFormat, ...)
{
	CDiagMessage*	pMessage;
	char			szBuffer[1024];

	//	Get the current system time
	GetLocalTime(&m_Time);

	//	Declare the variable list of arguements            
	va_list	Arguements;

	//	Insert the first variable arguement into the arguement list
	va_start(Arguements, lpFormat);

	//	Format the message
	vsprintf_s(szBuffer, sizeof(szBuffer), lpFormat, Arguements);

	//	Clean up the arguement list
	va_end(Arguements);

	//	Allocate a new error
	pMessage = new CDiagMessage();
	ASSERT(pMessage);

	//	Initialize the error
	pMessage->m_strDate.Format("%02d-%02d-%04d", m_Time.wMonth, m_Time.wDay, m_Time.wYear);
	pMessage->m_strTime.Format("%02d:%02d:%02d", m_Time.wHour, m_Time.wMinute, m_Time.wSecond);
	
	if((lpUser != 0) && (lstrlen(lpUser) > 0))
		pMessage->m_strText.Format("%s - %s", lpUser, szBuffer);
	else
		pMessage->m_strText = szBuffer;

	//	Add to the list
	m_Messages.Add(pMessage);

	//	Make sure we haven't exceeded the maximum
	while(m_Messages.GetCount() > DIAGNOSTICS_MAX_MESSAGES)
		m_Messages.RemoveTail(TRUE);

	//	Log this error
	if(m_bLogEnabled)
		LogMessage(pMessage);

	//	Add to the list box
	AddMessage(pMessage);

	//	Notify the attached window
	if((m_hNotify != 0) && IsWindow(m_hNotify))
		::PostMessage(m_hNotify, m_uNotify, 0, (LONG)pMessage);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::Report()
//
//	Parameters:		lpUser  - name used to report errors
//					uId     - string resource identifier
//					lpArg1  - optional character string arguement
//					lpArg2  - optional character string arguement
//
// 	Return Value:	None
//
// 	Description:	This function is called to report an error using a string
//					resource.
//
//------------------------------------------------------------------------------
void CDiagnostics::Report(LPCSTR lpUser, UINT uId, LPCSTR lpArg1, LPCSTR lpArg2)
{
	CString strMessage;

	//	Format the error message
	if(lpArg2 != 0)
		AfxFormatString2(strMessage, uId, lpArg1, lpArg2);
	else if(lpArg1 != 0)
		AfxFormatString1(strMessage, uId, lpArg1);
	else
		strMessage.LoadString(uId);

	Report(lpUser, strMessage);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::SetListPosition()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is called to set the size and position of the
//					list box within the client area of the dialog box.
//
//------------------------------------------------------------------------------
void CDiagnostics::SetListPosition() 
{
	//	Has the list box been created yet?
	if(IsWindow(m_ctrlMessages.m_hWnd))
	{
		//	Get the client rectangle
		RECT rcClient;
		GetClientRect(&rcClient);

		//	Adjust to allow for the border
		rcClient.left   += DIAGNOSTICS_LIST_BORDER;
		rcClient.right  -= DIAGNOSTICS_LIST_BORDER;
		rcClient.top    += DIAGNOSTICS_LIST_BORDER;
		rcClient.bottom -= DIAGNOSTICS_LIST_BORDER;

		//	Move the list box into position
		m_ctrlMessages.MoveWindow(&rcClient);
	}	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::SetLogEnabled()
//
//	Parameters:		bEnabled - flag to enable/disable logging
//
// 	Return Value:	None
//
// 	Description:	This function is called to enable or disable error logging
//
//------------------------------------------------------------------------------
void CDiagnostics::SetLogEnabled(BOOL bEnabled)
{
	m_bLogEnabled = bEnabled;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::SetLogFilename()
//
//	Parameters:		lpFilename - pointer to buffer containing the filename
//
// 	Return Value:	None
//
// 	Description:	This function is called to set the name of the log file
//
//------------------------------------------------------------------------------
void CDiagnostics::SetLogFilename(LPCSTR lpFilename)
{
	if((lpFilename != 0) && (lstrlen(lpFilename) > 0))
		m_strLogFilename = lpFilename;
	else
		m_strLogFilename.Empty();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::SetLogFolder()
//
//	Parameters:		lpFolder - pointer to buffer containing the folder path
//
// 	Return Value:	None
//
// 	Description:	This function is called to set the folder where the log 
//					file is stored.
//
//------------------------------------------------------------------------------
void CDiagnostics::SetLogFolder(LPCSTR lpFolder)
{
	if((lpFolder != 0) && (lstrlen(lpFolder) > 0))
	{
		m_strLogFolder = lpFolder;
		if(m_strLogFolder.Right(1) != "\\")
			m_strLogFolder += "\\";
	}
	else
	{
		m_strLogFolder.Empty();
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::SetNotificationHandle()
//
//	Parameters:		hWnd - window to send notification messages to
//
// 	Return Value:	None
//
// 	Description:	This function is called to specify the window to which 
//					notification messages should be sent when the error list
//					is modified.
//
//------------------------------------------------------------------------------
void CDiagnostics::SetNotificationHandle(HWND hWnd)
{
	m_hNotify = hWnd;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDiagnostics::SetNotificationMessage()
//
//	Parameters:		uMsg - id of notification message to be sent
//
// 	Return Value:	None
//
// 	Description:	This function is called to set the id of the message to be
//					sent when the error list is modified.
//
//------------------------------------------------------------------------------
void CDiagnostics::SetNotificationMessage(UINT uMsg)
{
	m_uNotify = uMsg;
}

