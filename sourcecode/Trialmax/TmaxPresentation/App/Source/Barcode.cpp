//==============================================================================
//
// File Name:	barcode.cpp
//
// Description:	This file contains member functions of the CBarcode class.
//
// See Also:	barcode.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-12-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <barcode.h>

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
// 	Function Name:	CBarcode::CBarcode()
//
// 	Description:	This is the constructor for CBarcode objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBarcode::CBarcode(LPCSTR lpszBarcode)
{
	SetBarcode(lpszBarcode);
}

//==============================================================================
//
// 	Function Name:	CBarcode::CBarcode()
//
// 	Description:	This is the copy constructor for CBarcode objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBarcode::CBarcode(const CBarcode& rBarcode)
{
	m_lSecondaryId = rBarcode.m_lSecondaryId;
	m_lTertiaryId  = rBarcode.m_lTertiaryId;
	m_strMediaId   = rBarcode.m_strMediaId;
}

//==============================================================================
//
// 	Function Name:	CBarcode::~CBarcode()
//
// 	Description:	This is the destructor for CBarcode objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBarcode::~CBarcode()
{

}

//==============================================================================
//
// 	Function Name:	CBarcode::GetBarcode()
//
// 	Description:	Called to get the properly formatted barcode
//
// 	Returns:		The barcode equivalent
//
//	Notes:			None
//
//==============================================================================
CString CBarcode::GetBarcode()
{
	CString	strBarcode = "";

	if(m_strMediaId.GetLength() > 0)
	{
		if(m_lSecondaryId > 0)
		{
			if(m_lTertiaryId > 0)
				strBarcode.Format("%s.%ld.%ld", m_strMediaId, m_lSecondaryId, m_lTertiaryId);
			else
				strBarcode.Format("%s.%ld", m_strMediaId, m_lSecondaryId);
		}
		else
		{
			strBarcode = m_strMediaId;
		}

	}// if(m_strMediaId.GetLength() > 0)

	return strBarcode;
}

//==============================================================================
//
// 	Function Name:	CBarcode::MsgBox()
//
// 	Description:	Called as a debugging aide to display the values in a 
//					standard Windows message box
//
// 	Returns:		IDOK/IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CBarcode::MsgBox(HWND hWnd)
{
	CString	strMsg = "";
	CString	strTemp = "";

	strTemp.Format("Barcode: %s\n", GetBarcode());
	strMsg += strTemp;

	strTemp.Format("MediaId: %s\n", m_strMediaId);
	strMsg += strTemp;

	strTemp.Format("SecondaryId: %ld\n", m_lSecondaryId);
	strMsg += strTemp;

	strTemp.Format("TertiaryId: %ld\n", m_lTertiaryId);
	strMsg += strTemp;

	return MessageBox(hWnd, strMsg, "Barcode", MB_OKCANCEL | MB_ICONINFORMATION);
}

//==============================================================================
//
// 	Function Name:	CLine::operator = ()
//
// 	Description:	This is an overloaded version of the assignment operator.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarcode::operator = (const CBarcode& rBarcode)
{
	m_lSecondaryId = rBarcode.m_lSecondaryId;
	m_lTertiaryId  = rBarcode.m_lTertiaryId;
	m_strMediaId   = rBarcode.m_strMediaId;
}

//==============================================================================
//
// 	Function Name:	CBarcode::Reset()
//
// 	Description:	This function is called to reset the members to their 
//					initial state.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarcode::Reset()
{
	m_lSecondaryId = -1;
	m_lTertiaryId  = -1;
	m_strMediaId   = "";
}

//==============================================================================
//
// 	Function Name:	CBarcode::SetBarcode()
//
// 	Description:	This member is called to initialize the object using the
//					string provided by the caller.
//
// 	Returns:		TRUE if valid
//
//	Notes:			None
//
//==============================================================================
BOOL CBarcode::SetBarcode(LPCSTR lpszBarcode)
{
	char	szBuffer[256];
	char*	pToken;

	//	Clear the existing information
	Reset();

	//	Now set the new values
	if((lpszBarcode != NULL) && (lstrlen(lpszBarcode) > 0))
	{
		//	Copy the barcode to our working buffer so we can parse it into it's 
		//	component parts
		lstrcpyn(szBuffer, lpszBarcode, sizeof(szBuffer));

		//	Look for the first delimiter so we can get the media id
		if((pToken = strchr(szBuffer, TMAX_BARCODE_DELIMITER)) == 0)
		{
			//	No secondary or tertiary id has been supplied
			m_strMediaId = szBuffer;
		}
		else
		{
			//	Strip the media id
			*pToken = 0;
			m_strMediaId = szBuffer;
			lstrcpy(szBuffer, (pToken + 1));

			//	Now look for the secondary id
			if((pToken = strchr(szBuffer, TMAX_BARCODE_DELIMITER)) == 0)
			{
				m_lSecondaryId = atol(szBuffer);
			}
			else
			{
				//	Convert the secondary to a number
				*pToken = 0;
				m_lSecondaryId = atol(szBuffer);
				lstrcpy(szBuffer, (pToken + 1));

				//	Do we have a tertiary identifier?
				if(lstrlen(szBuffer) > 0)
					m_lTertiaryId = atol(szBuffer);
			}

		}

	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Add()
//
// 	Description:	This function will add the object pointer to the list in the
//					correct position based on the current sort order. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarcodes::Add(CBarcode* pBarcode)
{
	AddTail(pBarcode);
}

//==============================================================================
//
// 	Function Name:	CBarcodes::CBarcodes()
//
// 	Description:	This is the constructor for CBarcodes objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBarcodes::CBarcodes() : CObList(1)
{
	//	Initialize the local members
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

///==============================================================================
//
// 	Function Name:	CBarcodes::~CBarcodes()
//
// 	Description:	This is the destructor for CBarcodes objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBarcodes::~CBarcodes()
{
	//	Flush the list
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CBarcodes::Find(CBarcode* pBarcode)
{
	return (CObList::Find(pBarcode));
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Find()
//
// 	Description:	This function will retrieve the object with the specified
//					barcode
//
// 	Returns:		NULL if not found
//
//	Notes:			None
//
//==============================================================================
CBarcode* CBarcodes::Find(LPCSTR lpszBarcode)
{
	POSITION	Pos = NULL;
	CBarcode*	pBarcode = NULL;

	//	Get the first position
	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		pBarcode = (CBarcode*)GetNext(Pos);

		if(pBarcode && (pBarcode->GetBarcode().CompareNoCase(lpszBarcode) == 0))
			return pBarcode;
	}
	return NULL;
}

//==============================================================================
//
// 	Function Name:	CBarcodes::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CBarcode* CBarcodes::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CBarcode*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Flush()
//
// 	Description:	This function will flush all CBarcode objects from the 
//					list. If bDeleteAll is TRUE, the objects are deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarcodes::Flush(BOOL bDeleteAll)
{
	//	Do we want to delete the objects?
	if(bDeleteAll)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			CBarcode* pBarcode = (CBarcode*)CObList::GetNext(m_NextPos);
			if(pBarcode != NULL)
				delete pBarcode;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CBarcode* CBarcodes::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CBarcode*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CBarcode* CBarcodes::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CBarcode*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CBarcode* CBarcodes::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CBarcode*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarcodes::Remove(CBarcode* pBarcode, BOOL bDelete)
{
	POSITION Pos = Find(pBarcode);

	//	Is this object in the list
	if(Pos != NULL)
		RemoveAt(Pos);

	//	Do we need to delete the object?
	if(bDelete)
		delete pBarcode;
}

//==============================================================================
//
// 	Function Name:	CBarcodes::SetPos()
//
// 	Description:	This function will set the position of the local iterator
//					on the line specified by the caller
//
// 	Returns:		A pointer to object if found
//
//	Notes:			None
//
//==============================================================================
CBarcode* CBarcodes::SetPos(CBarcode* pBarcode)
{
	CBarcode* pCurrent;

	pCurrent = First();
	while(pCurrent)
	{
		if(pCurrent == pBarcode)
			return pBarcode;
		else
			pCurrent = Next();
	}

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Add()
//
// 	Description:	Called to add a barcode to the collection
//
// 	Returns:		The index at which the barcode was added
//
//	Notes:			None
//
//==============================================================================
int CBarcodeBuffer::Add(LPCSTR lpszBarcode)
{
	CBarcode barcode;

	ASSERT(lpszBarcode != NULL);
	ASSERT(lstrlen(lpszBarcode) > 0);
	if((lpszBarcode == NULL) || (lstrlen(lpszBarcode) == 0))
		return -1;

	//	Initialize the barcode
	if(barcode.SetBarcode(lpszBarcode) == TRUE)
		return Add(barcode);
	else
		return -1;
}

//==============================================================================
//
// 	Function Name:	CBarcodes::Add()
//
// 	Description:	Called to add a barcode to the collection
//
// 	Returns:		The index at which the barcode was added
//
//	Notes:			None
//
//==============================================================================
int CBarcodeBuffer::Add(CBarcode& rBarcode)
{
	ASSERT(m_paBarcodes != NULL);
	ASSERT(m_iMaxBarcodes > 0);
	if((m_paBarcodes == NULL) || (m_iMaxBarcodes <= 0))
		return -1;

	//	Is the buffer full?
	if(m_iInBuffer >= m_iMaxBarcodes)
	{
		//	Make room for the new barcode
		if(m_paBarcodes[0] != NULL)
		{
			delete m_paBarcodes[0];
			m_paBarcodes[0] = NULL;
		}

		for(int i = 1; i < m_iMaxBarcodes; i++)
			m_paBarcodes[i - 1] = m_paBarcodes[i];

		m_paBarcodes[m_iMaxBarcodes - 1] = NULL;
		m_iInBuffer = m_iMaxBarcodes - 1;

	}// if(m_iInBuffer >= m_iMaxBarcodes)

	//	Add a copy to the collection
	m_paBarcodes[m_iInBuffer] = new CBarcode(rBarcode);
	m_iInBuffer += 1;

	//	Assume the new barcode is active
	m_iActive = m_iInBuffer - 1;

	return m_iActive;
}

//==============================================================================
//
// 	Function Name:	CBarcodes::CBarcodeBuffer()
//
// 	Description:	This is the constructor for CBarcodeBuffer objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBarcodeBuffer::CBarcodeBuffer()
{
	m_paBarcodes = NULL;
	m_iMaxBarcodes = 0;
	m_iInBuffer = 0;
	m_iActive = -1;
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::~CBarcodeBuffer()
//
// 	Description:	This is the destructor for CBarcodeBuffer objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBarcodeBuffer::~CBarcodeBuffer()
{
	Free();
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::Clear()
//
// 	Description:	Called to clear all barcodes in the collection
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarcodeBuffer::Clear()
{
	if((m_paBarcodes != NULL) && (m_iMaxBarcodes > 0))
	{
		for(int i = 0; i < m_iMaxBarcodes; i++)
		{
			if(m_paBarcodes[i] != NULL)
				m_paBarcodes[i]->Reset();
		}
	}

	m_iInBuffer = 0;
	m_iActive = -1;
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::Free()
//
// 	Description:	Called to deallocate memory used by the buffer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarcodeBuffer::Free()
{
	if(m_paBarcodes != NULL)
	{
		for(int i = 0; i < m_iMaxBarcodes; i++)
		{
			if(m_paBarcodes[i] != NULL)
				delete m_paBarcodes[i];
		}

		delete [] m_paBarcodes;
		m_paBarcodes = NULL;
	}

	m_iMaxBarcodes = 0;
	m_iInBuffer = 0;
	m_iActive = -1;
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::GetActive()
//
// 	Description:	Called to get the active barcode
//
// 	Returns:		A pointer to the active barcode
//
//	Notes:			None
//
//==============================================================================
CBarcode* CBarcodeBuffer::GetActive()
{
	if((m_paBarcodes != NULL) && (m_iInBuffer > 0))
	{
		if((m_iActive >= 0) && (m_iActive < m_iInBuffer))
			return m_paBarcodes[m_iActive];

	}// if((m_paBarcodes != NULL) && (m_iInBuffer > 0))
	
	return NULL;
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::GetNext()
//
// 	Description:	Called to get the next barcode in the collection
//
// 	Returns:		A pointer to the next barcode
//
//	Notes:			None
//
//==============================================================================
CBarcode* CBarcodeBuffer::GetNext()
{
	if((m_paBarcodes != NULL) && (m_iInBuffer > 0))
	{
		if(m_iActive + 1 < m_iInBuffer)
		{
			m_iActive++;
			return GetActive(); // Does full range checking
		}

	}// if((m_paBarcodes != NULL) && (m_iInBuffer > 0))
	
	return NULL;
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::GetPrevious()
//
// 	Description:	Called to get the previous barcode in the collection
//
// 	Returns:		A pointer to the previous barcode
//
//	Notes:			None
//
//==============================================================================
CBarcode* CBarcodeBuffer::GetPrevious()
{
	if((m_paBarcodes != NULL) && (m_iInBuffer > 0))
	{
		if(m_iActive > 0)
		{
			m_iActive--;
			return GetActive(); // Does full range checking
		}

	}// if((m_paBarcodes != NULL) && (m_iInBuffer > 0))
	
	return NULL;
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::MsgBox()
//
// 	Description:	Called as a debugging aide to display the values in a 
//					standard Windows message box
//
// 	Returns:		IDOK/IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CBarcodeBuffer::MsgBox(HWND hWnd)
{
	CString	strMsg = "";
	CString	strTemp = "";

	strTemp.Format("MaxBarcodes: %d\n", m_iMaxBarcodes);
	strMsg += strTemp;

	strTemp.Format("InBuffer: %d\n", m_iInBuffer);
	strMsg += strTemp;

	strTemp.Format("Active: %d\n", m_iActive);
	strMsg += strTemp;

	if(MessageBox(hWnd, strMsg, "Barcode Buffer", MB_OKCANCEL | MB_ICONINFORMATION) == IDCANCEL)
		return IDCANCEL;

	if((m_paBarcodes != NULL) && (m_iMaxBarcodes > 0))
	{
		for(int i = 0; i < m_iMaxBarcodes; i++)
		{
			if(m_paBarcodes[i] != NULL)
			{
				if(m_paBarcodes[i]->MsgBox(hWnd) == IDCANCEL)
					return IDCANCEL;
			}

		}// for(int i = 0; i < m_iMaxBarcodes; i++)

	}// if((m_paBarcodes != NULL) && (m_iMaxBarcodes > 0))
	
	return IDOK;	
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::OnFirst()
//
// 	Description:	Called to determine if positioned on the first barcode
//
// 	Returns:		TRUE if the first barcode is the active barcode
//
//	Notes:			None
//
//==============================================================================
BOOL CBarcodeBuffer::OnFirst()
{
	if((m_paBarcodes != NULL) && (m_iInBuffer > 0))
		return (m_iActive == 0);
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::OnLast()
//
// 	Description:	Called to determine if positioned on the last barcode
//
// 	Returns:		TRUE if the last barcode is the active barcode
//
//	Notes:			None
//
//==============================================================================
BOOL CBarcodeBuffer::OnLast()
{
	if((m_paBarcodes != NULL) && (m_iInBuffer > 0))
		return (m_iActive == (m_iInBuffer - 1));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CBarcodeBuffer::SetMaxBarcodes()
//
// 	Description:	Called to set the maximum number of barcodes in the buffer
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CBarcodeBuffer::SetMaxBarcodes(int iMaxBarcodes)
{
	//	Free the existing buffer
	Free();

	//	Make sure we have a valid value
	if((m_iMaxBarcodes = iMaxBarcodes) < 2)
		m_iMaxBarcodes = 2;

	//	Allocate the new barcodes
	m_paBarcodes = new CBarcode*[m_iMaxBarcodes];
	memset(m_paBarcodes, 0, sizeof(CBarcode*));

	//	Allocate the barcode objects
	for(int i = 0; i < m_iMaxBarcodes; i++)
		m_paBarcodes[i] = new CBarcode();

	return (m_paBarcodes != NULL);
}



