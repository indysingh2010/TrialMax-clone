//==============================================================================
//
// File Name:	handler.cpp
//
// Description:	This file contains member functions of the CErrorHandler class.
//
// See Also:	tmdbase.h
//
//==============================================================================
//	Date		Revision    Description
//	08-09-97	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <handler.h>

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

//==============================================================================
//
// 	Function Name:	CErrorHandler::CErrorHandler()
//
// 	Description:	This is the constructor for CErrorHandler objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CErrorHandler::CErrorHandler()
{
	m_hParent  = 0;
	m_bEnabled = TRUE;
	m_strTitle = "Error";
	m_uMessageId = 0;
}

//==============================================================================
//
// 	Function Name:	CErrorHandler::~CErrorHandler()
//
// 	Description:	This is the destructor for CErrorHandler objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CErrorHandler::~CErrorHandler()
{

}

//==============================================================================
//
// 	Function Name:	CErrorHandler::Enable()
//
// 	Description:	This function enables and disables runtime error messages.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CErrorHandler::Enable(BOOL bEnable)
{
	m_bEnabled = bEnable;
}

//==============================================================================
//
// 	Function Name:	CErrorHandler::GetParent()
//
// 	Description:	This function is called to get the parent window.
//
// 	Returns:		A handle to the parent window.
//
//	Notes:			None
//
//==============================================================================
HWND CErrorHandler::GetParent()
{
	return m_hParent;
}

//==============================================================================
//
// 	Function Name:	CErrorHandler::Handle()
//
// 	Description:	This function will display an error message associated with
//					the error level provided by the caller. The message string
//					is loaded from the application's string resources.
//
// 	Returns:		None
//
//	Notes:			The title is optional. If NULL, the string stored in the
//					call to SetTitle() is used instead.
//
//					The message string does not have to but can contain up to
//					two arguements. The string should be formatted in 
//					accordance with the call to AfxFormatString1() and
//					AfxFormatString2().
//
//==============================================================================
void CErrorHandler::Handle(LPCSTR lpTitle, UINT uStringId, 
						   LPCSTR lpArg1, LPCSTR lpArg2)
{
	CString strTitle;

	//	Do we need to use the default title?
	if(lpTitle)
		strTitle = lpTitle;
	else
		strTitle = m_strTitle;

	//	Format the error message
	if(lpArg2 != 0)
		AfxFormatString2(m_strMessage, uStringId, lpArg1, lpArg2);
	else if(lpArg1 != 0)
		AfxFormatString1(m_strMessage, uStringId, lpArg1);
	else
		m_strMessage.LoadString(uStringId);

	if(m_bEnabled)
	{
		if((m_uMessageId > 0) && IsWindow(m_hParent))
		{
			PostMessage(m_hParent, m_uMessageId, 0, (LPARAM)this);
		}
		else
		{
			MessageBeep(0xFFFFFFFF);
			MessageBox(m_hParent, m_strMessage, strTitle, MB_TOPMOST | MB_ICONEXCLAMATION | MB_OK);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CErrorHandler::Handle()
//
// 	Description:	This function will display the error message provided by
//					the caller.
//
// 	Returns:		None
//
//	Notes:			The title is optional. If NULL, the string stored in the
//					call to SetTitle() is used instead.
//
//==============================================================================
void CErrorHandler::Handle(LPCSTR lpTitle, LPCSTR lpMessage)
{
	CString strTitle;

	//	Do we need to use the default title?
	if(lpTitle)
		strTitle = lpTitle;
	else
		strTitle = m_strTitle;

	//	Is the message valid?
	if(lpMessage)
		m_strMessage = lpMessage;
	else
		m_strMessage = "Invalid error message string provided!";

	if(m_bEnabled)
	{
		if((m_uMessageId > 0) && IsWindow(m_hParent))
		{
			PostMessage(m_hParent, m_uMessageId, 0, (LPARAM)this);
		}
		else
		{
			MessageBeep(0xFFFFFFFF);
			MessageBox(m_hParent, m_strMessage, strTitle, MB_TOPMOST | MB_ICONEXCLAMATION | MB_OK);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CErrorHandler::IsEnabled()
//
// 	Description:	This function is called to determine if the error handler
//					is enabled.
//
// 	Returns:		TRUE if enabled.
//
//	Notes:			None
//
//==============================================================================
BOOL CErrorHandler::IsEnabled()
{
	return m_bEnabled;
}

//==============================================================================
//
// 	Function Name:	CErrorHandler::SetMessageId()
//
// 	Description:	This function sets the id of the message to be sent to
//					the parent in the event of an error.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CErrorHandler::SetMessageId(UINT uId)
{
	m_uMessageId = uId;
}

//==============================================================================
//
// 	Function Name:	CErrorHandler::SetParent()
//
// 	Description:	This function sets the parent window for the handler.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CErrorHandler::SetParent(HWND hParent)
{
	m_hParent = hParent;
}

//==============================================================================
//
// 	Function Name:	CErrorHandler::SetTitle()
//
// 	Description:	This function sets the default title for error messages.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CErrorHandler::SetTitle(LPCSTR lpTitle)
{
	m_strTitle = lpTitle;
}

