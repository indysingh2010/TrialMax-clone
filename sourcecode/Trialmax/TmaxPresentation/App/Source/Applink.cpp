//==============================================================================
//
// File Name:	applink.cpp
//
// Description:	This file contains member functions of the CAppLink class.
//
// See Also:	applink.h
//
// Copyright	FTI Consulting 1997-2008
//
//==============================================================================
//	Date		Revision    Description
//	02-10-2008	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <applink.h>

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
// 	Function Name:	CAppLink::CAppLink()
//
// 	Description:	This is the constructor for CAppLink objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAppLink::CAppLink()
{
	//	Assign default values
	Clear();

}

//==============================================================================
//
// 	Function Name:	CAppLink::~CAppLink()
//
// 	Description:	This is the destructor for CAppLink objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAppLink::~CAppLink()
{

}

//==============================================================================
//
// 	Function Name:	CAppLink::Clear()
//
// 	Description:	Called to reset the class members to their default values
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAppLink::Clear()
{
	m_bIsPending = FALSE;
	m_bIsEvent = FALSE;
	m_lAttributes = 0;
	m_strBarcode = "";
	m_strDatabaseId = "";
}

//==============================================================================
//
// 	Function Name:	CAppLink::GetHideLink()
//
// 	Description:	Called to determine if the linked media should be hidden
//
// 	Returns:		TRUE if media should be hidden
//
//	Notes:			None
//
//==============================================================================
BOOL CAppLink::GetHideLink()
{
	if(GetFlag(TMFLAG_LINK_HIDE) == TRUE)
		return TRUE;
	else if(m_strBarcode.GetLength() == 0)
		return TRUE; // Assume hidden if no barcode has been assigned
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CAppLink::MsgBox()
//
// 	Description:	Called as a debugging aide to display the values in a 
//					standard Windows message box
//
// 	Returns:		IDOK/IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CAppLink::MsgBox(HWND hWnd)
{
	CString	strMsg = "";
	CString	strTemp = "";

	strTemp.Format("Barcode: %s\n", m_strBarcode);
	strMsg += strTemp;

	strTemp.Format("DatabaseId: %s\n", m_strDatabaseId);
	strMsg += strTemp;

	strTemp.Format("IsPending: %s\n", m_bIsPending ? "TRUE" : "FALSE");
	strMsg += strTemp;

	strTemp.Format("IsEvent: %s\n", m_bIsEvent ? "TRUE" : "FALSE");
	strMsg += strTemp;

	return MessageBox(hWnd, strMsg, "Barcode", MB_OKCANCEL | MB_ICONINFORMATION);
}

//==============================================================================
//
// 	Function Name:	CAppLink::SetFlag()
//
// 	Description:	Called to set the specified attributes flag
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAppLink::SetFlag(long lMask, BOOL bState)
{
	//AfxMessageBox("CAppLink::SetFlag");
	if(bState == TRUE)
		m_lAttributes |= lMask;  // Set the associated bit
	else
		m_lAttributes &= ~lMask; //	Clear the associated bit

}

