//==============================================================================
//
// File Name:	barcodes.cpp
//
// Description:	This file contains member functions of the CCell and
//				CCells classes.
//
// See Also:	barcodes.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-25-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <cell.h>

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
// 	Function Name:	CCellField::CCellField()
//
// 	Description:	This is the constructor for CCellField objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCellField::CCellField(int iId, LPCSTR lpszText)
{
	m_iId = iId;
	m_strText = lpszText != NULL ? lpszText : "";
}

//==============================================================================
//
// 	Function Name:	CCellField::CCellField()
//
// 	Description:	This is the destructor for CCellField objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCellField::~CCellField()
{

}

//==============================================================================
//
// 	Function Name:	CCellFields::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CCellFields::Add(CCellField* pField)
{
	ASSERT(pField);
	if(!pField)
		return FALSE;

	try
	{
		//	Add at the head of the list if the list is empty
		if(IsEmpty())
			AddHead(pField);
		else
			AddTail(pField);

		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CCellField::CCellFields()
//
// 	Description:	This is the constructor for CCellFields objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCellFields::CCellFields() : CObList()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;

	InitializeCriticalSection(&m_Lock);
}

//==============================================================================
//
// 	Function Name:	CCellField::~CCellFields()
//
// 	Description:	This is the destructor for CCellFields objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCellFields::~CCellFields()
{
	//	Flush the list and destroy its objects
	Flush(TRUE);
		
	DeleteCriticalSection(&m_Lock);
}

//==============================================================================
//
// 	Function Name:	CCellFields::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CCellFields::Find(CCellField* pField)
{
	return (CObList::Find(pField));
}

//==============================================================================
//
// 	Function Name:	CCellFields::Find()
//
// 	Description:	Called to find the object with the specified Id
//
// 	Returns:		The object is found
//
//	Notes:			None
//
//==============================================================================
CCellField* CCellFields::Find(int iId)
{
	CCellField* pField = NULL;
	POSITION	Pos = NULL;

	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pField = (CCellField*)GetNext(Pos)) != NULL)
		{
			if(pField->m_iId == iId)
				return pField;		
		}

	}// while(Pos != NULL)

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CCellFields::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CCellField* CCellFields::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CCellField*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CCellFields::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CCellFields::Flush(BOOL bDelete)
{
	CCellField* pField;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pField = (CCellField*)GetNext(m_NextPos)) != 0)
				delete pField;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CCellFields::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CCellField* CCellFields::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CCellField*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CCellFields::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CCellField* CCellFields::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CCellField*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CCellFields::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CCellField* CCellFields::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CCellField*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CCellFields::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCellFields::Remove(CCellField* pField, BOOL bDelete)
{
	POSITION Pos = Find(pField);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pField;
	}
}

//==============================================================================
//
// 	Function Name:	CCell::CCell()
//
// 	Description:	This is the constructor for CCell objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCell::CCell(LPSTR lpString)
{
	m_lTertiaryId			=  0;
	m_lPage					= -1;
	m_lPages				=  0;
	m_lFlags				=  0;
	m_sType					=  CELL_DOCUMENT;
	m_strString				= "";	
	m_strMasterId			= "";
	m_strSecondId			= "";
	m_strPath				= "";
	m_strTreatmentImage		= "";
	m_strSiblingFileSpec	= "";
	m_strSiblingImage		= "";

	//	Set the members if a string was provided
	if(lpString)
		SetFromString(lpString);
}

//==============================================================================
//
// 	Function Name:	CCell::CCell()
//
// 	Description:	This is the destructor for CCell objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCell::~CCell()
{
	m_aTextFields.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CCell::GetText()
//
// 	Description:	This function is called to get the text for the specified
//					fixed field.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
CString CCell::GetText(int iFixedField, CTemplate* pTemplate)
{
	CString strText = "";

	ASSERT(iFixedField >= 0);
	ASSERT(iFixedField < TEMPLATE_MAX_FIXED_FIELDS);
	
	if((iFixedField >= 0) && (iFixedField < TEMPLATE_MAX_FIXED_FIELDS))
	{
		//	If this is the page number field we need to format it according
		//	to the template options
		if(iFixedField == TEMPLATE_PAGENUM)
		{
			//	Format the page number text
			if((pTemplate != NULL) && (pTemplate->m_bPageAsSeries))
				m_aFixedFields[TEMPLATE_PAGENUM].Format("Page %ld of %ld", m_lPage, m_lPages);
			else
				m_aFixedFields[TEMPLATE_PAGENUM].Format("Page %ld", m_lPage);
		}

		strText = m_aFixedFields[iFixedField];
	}
	
	return strText;
}

//==============================================================================
//
// 	Function Name:	CCell::SetFromString()
//
// 	Description:	This function will set the class members using the string
//					provided by the caller
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CCell::SetFromString(LPSTR lpString)
{
	char*	pParam;
	char*	pDelimiter;
	
	ASSERT(lpString);
	if(lpString == 0)
		return FALSE;

	//	Clear the text fields collection
	m_aTextFields.Flush(TRUE);

	//	Save the string specification
	m_strString = lpString;

	//	Initialize the pointers
	pParam = lpString;
	if((pDelimiter = strchr(lpString, '|')) == 0)
		pDelimiter = strchr(lpString, '~');	//	Done for backward compatability
	if(pDelimiter != 0)
		*pDelimiter = 0;

	while(pParam != 0)
	{
		//	Set the member associated with this parameter
		if(!SetMember(pParam))
			return FALSE;

		//	Line up on the next parameter
		if(pDelimiter != 0)
		{
			pParam = pDelimiter + sizeof(TCHAR);
			if((pParam != 0) && (*pParam != '\0'))
			{
				if((pDelimiter = strchr(pParam, '|')) == 0)
					pDelimiter = strchr(pParam, '~');
				if(pDelimiter != 0)
					*pDelimiter = 0;//	NULL terminate
			}
			else
			{
				pDelimiter = 0;
			}
		}
		else
		{
			pParam = 0;
			pDelimiter = 0;
		}

	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CCell::SetMember()
//
// 	Description:	This function will set the appropriate class member using
//					the string parameter provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CCell::SetMember(LPCSTR lpParam)
{
	char		szLabel[256];
	char		szValue[256];
	char*		pToken;
	CCellField*	pField = NULL;

	//	Parse the parameter into its label and value
	ASSERT(lpParam);
	lstrcpyn(szLabel, lpParam, sizeof(szLabel));
	if((pToken = strchr(szLabel, '=')) == 0)
		return FALSE;

	*pToken = 0;
	pToken++;
	lstrcpyn(szValue, pToken, sizeof(szValue));

	//	Is this the primary id?
	if(lstrcmpi(szLabel, PRIMARY_LABEL) == 0)
	{
		m_strMasterId = szValue;
	}

	//	Is this the secondary id?
	else if(lstrcmpi(szLabel, SECONDARY_LABEL) == 0)
	{
		m_strSecondId = szValue;
	}

	//	Is this the tertiary identifier?
	else if(lstrcmpi(szLabel, TERTIARY_LABEL) == 0)
	{
		m_lTertiaryId = atol(szValue);
	}

	//	Is this the page number?
	else if(lstrcmpi(szLabel, PAGE_LABEL) == 0)
	{
		m_lPage = atol(szValue);
		m_aFixedFields[TEMPLATE_PAGENUM].Format("%ld", m_lPage);
	}

	//	Is this the page count?
	else if(lstrcmpi(szLabel, PAGES_LABEL) == 0)
	{
		m_lPages = atol(szValue);
	}

	//	Is this the barcode?
	else if(lstrcmpi(szLabel, BARCODE_LABEL) == 0)
	{
		m_aFixedFields[TEMPLATE_BARCODE] = szValue;
	}

	//	Is this the foreign barcode?
	else if(lstrcmpi(szLabel, FOREIGN_BARCODE_LABEL) == 0)
	{
		m_aFixedFields[TEMPLATE_FOREIGN_BARCODE] = szValue;
	}

	//	Is this the source barcode?
	else if(lstrcmpi(szLabel, SOURCE_BARCODE_LABEL) == 0)
	{
		m_aFixedFields[TEMPLATE_SOURCE_BARCODE] = szValue;
	}

	//	Is this the graphic barcode?
	else if(lstrcmpi(szLabel, GRAPHIC_BARCODE_LABEL) == 0)
	{
		m_aFixedFields[TEMPLATE_GRAPHIC] = szValue;
	}

	//	Is this the deponent name?
	else if(lstrcmpi(szLabel, DEPONENT_LABEL) == 0)
	{
		m_aFixedFields[TEMPLATE_DEPONENT] = szValue;
	}

	//	Is this the filename?
	else if(lstrcmpi(szLabel, FILENAME_LABEL) == 0)
	{
		m_aFixedFields[TEMPLATE_FILENAME] = szValue;
	}

	//	Is this the media name?
	else if(lstrcmpi(szLabel, NAME_LABEL) == 0)
	{
		m_aFixedFields[TEMPLATE_NAME] = szValue;
	}

	//	Is this the file path?
	else if(lstrcmpi(szLabel, PATH_LABEL) == 0)
	{
		m_strPath = szValue;
	}

	//	Is this the treatment image file?
	else if(lstrcmpi(szLabel, TREATMENTIMAGE_LABEL) == 0)
	{
		m_strTreatmentImage = szValue;
	}

	//	Is this the sibling treatment path?
	else if(lstrcmpi(szLabel, SIBLINGFILESPEC_LABEL) == 0)
	{
		m_strSiblingFileSpec = szValue;
	}

	//	Is this the sibling treatment image file?
	else if(lstrcmpi(szLabel, SIBLINGIMAGE_LABEL) == 0)
	{
		m_strSiblingImage = szValue;
	}

	//	Is this the bit-packed cell flags?
	else if(lstrcmpi(szLabel, FLAGS_LABEL) == 0)
	{
		m_lFlags = atol(szValue);
	}

	//	Is this the type identifier?
	else if(lstrcmpi(szLabel, TYPE_LABEL) == 0)
	{
		switch(toupper(szValue[0]))
		{
			case CELL_TYPECHAR_GRAPHIC:		m_sType = CELL_GRAPHIC;
											break;

			case CELL_TYPECHAR_MOVIE:		m_sType = CELL_MOVIE;
											break;

			case CELL_TYPECHAR_PLAYLIST:	m_sType = CELL_PLAYLIST;
											break;

			case CELL_TYPECHAR_POWERPOINT:	m_sType = CELL_POWERPOINT;
											break;

			case CELL_TYPECHAR_DESIGNATION:	m_sType = CELL_DESIGNATION;
											break;
			case CELL_TYPECHAR_DOCUMENT:
			default:						m_sType = CELL_DOCUMENT;
											break;
		}
	}

	//	Is this one of the user defined text fields?
	else if(_strnicmp(szLabel, TEXTFIELD_LABEL, lstrlen(TEXTFIELD_LABEL)) == 0)
	{
		pField = new CCellField();
		pField->m_iId = atoi(&(szLabel[lstrlen(TEXTFIELD_LABEL)]));
		pField->m_strText = szValue;
		m_aTextFields.Add(pField);
	}

	else
	{
		return FALSE;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CCell::MsgBox()
//
// 	Description:	This function is provided as a debugging aide. It will 
//					display the current values of the class members
//
// 	Returns:		IDOK / IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CCell::MsgBox(HWND hWnd)
{
	CString strMember;
	CString	strMsg;

	strMember.Format("Master = %s\n", m_strMasterId);
	strMsg += strMember;

	strMember.Format("Second = %s\n", m_strSecondId);
	strMsg += strMember;

	strMember.Format("Tertiary = %ld\n", m_lTertiaryId);
	strMsg += strMember;

	strMember.Format("Type = %d\n", m_sType);
	strMsg += strMember;

	strMember.Format("Barcode = %s\n", m_aFixedFields[TEMPLATE_BARCODE]);
	strMsg += strMember;

	strMember.Format("Foreign Barcode = %s\n", m_aFixedFields[TEMPLATE_FOREIGN_BARCODE]);
	strMsg += strMember;

	strMember.Format("Source Barcode = %s\n", m_aFixedFields[TEMPLATE_SOURCE_BARCODE]);
	strMsg += strMember;

	strMember.Format("Graphic Barcode = %s\n", m_aFixedFields[TEMPLATE_GRAPHIC]);
	strMsg += strMember;

	strMember.Format("Name = %s\n", m_aFixedFields[TEMPLATE_NAME]);
	strMsg += strMember;

	strMember.Format("Path = %s\n", m_strPath);
	strMsg += strMember;

	strMember.Format("Filename = %s\n", m_aFixedFields[TEMPLATE_FILENAME]);
	strMsg += strMember;

	strMember.Format("Page = %ld\n", m_lPage);
	strMsg += strMember;

	strMember.Format("Pages = %ld\n", m_lPages);
	strMsg += strMember;

	strMember.Format("Flags = 0x%04x\n", (int)m_lFlags);
	strMsg += strMember;

	strMember.Format("Deponent = %s\n", m_aFixedFields[TEMPLATE_DEPONENT]);
	strMsg += strMember;

	strMember.Format("TreatmentImage = %s\n", m_strTreatmentImage);
	strMsg += strMember;

	strMember.Format("SiblingFileSpec = %s\n", m_strSiblingFileSpec);
	strMsg += strMember;

	strMember.Format("SiblingImage = %s\n", m_strSiblingImage);
	strMsg += strMember;

	return MessageBox(hWnd, strMsg, "Cell", MB_ICONINFORMATION | MB_OKCANCEL);
}

//==============================================================================
//
// 	Function Name:	CCells::Add()
//
// 	Description:	This function will add a link object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CCells::Add(CCell* pCell)
{
	ASSERT(pCell);
	if(!pCell)
		return FALSE;

	try
	{
		//	Add at the head of the list if the list is empty
		if(IsEmpty())
			AddHead(pCell);
		else
			AddTail(pCell);

		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CCell::CCells()
//
// 	Description:	This is the constructor for CCells objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCells::CCells() : CObList()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;

	InitializeCriticalSection(&m_Lock);
}

//==============================================================================
//
// 	Function Name:	CCell::~CCells()
//
// 	Description:	This is the destructor for CCells objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCells::~CCells()
{
	//	Flush the list and destroy its objects
	Flush(TRUE);
		
	DeleteCriticalSection(&m_Lock);
}

//==============================================================================
//
// 	Function Name:	CCells::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CCells::Find(CCell* pCell)
{
	return (CObList::Find(pCell));
}

//==============================================================================
//
// 	Function Name:	CCells::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CCell* CCells::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CCell*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CCells::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CCells::Flush(BOOL bDelete)
{
	CCell* pCell;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pCell = (CCell*)GetNext(m_NextPos)) != 0)
				delete pCell;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CCells::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CCell* CCells::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CCell*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CCell::MsgBox()
//
// 	Description:	This function is provided as a debugging aide. It will 
//					display the current values of all cells in the container
//
// 	Returns:		IDOK / IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CCells::MsgBox(HWND hWnd)
{
	CCell*		pCell = NULL;
	POSITION	Pos = NULL;

	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pCell = (CCell*)GetNext(Pos)) != NULL)
		{
			if(pCell->MsgBox(hWnd) == IDCANCEL)
				return IDCANCEL;
		}
	}

	return IDOK;
}

//==============================================================================
//
// 	Function Name:	CCells::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CCell* CCells::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CCell*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CCells::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CCell* CCells::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CCell*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CCells::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCells::Remove(CCell* pCell, BOOL bDelete)
{
	POSITION Pos = Find(pCell);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pCell;
	}
}

//==============================================================================
//
// 	Function Name:	CCells::UsesPowerPoint()
//
// 	Description:	This function is called to determine if one or more of the
//					cells in the list uses PowerPoint
//
// 	Returns:		TRUE if PowerPoint is required
//
//	Notes:			None
//
//==============================================================================
BOOL CCells::UsesPowerPoint()
{
	CCell*		pCell;
	POSITION	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pCell = (CCell*)GetNext(Pos)) != 0)
		{
			if(pCell->m_sType == CELL_POWERPOINT)
				return TRUE;
		}
	}
	return FALSE;
}

