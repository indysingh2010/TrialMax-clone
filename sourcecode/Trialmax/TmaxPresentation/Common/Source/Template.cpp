//==============================================================================
//
// File Name:	template.cpp
//
// Description:	This file contains member functions of the CTemplate and
//				CTemplates classes.
//
// See Also:	template.h
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
#include <template.h>

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
// 	Function Name:	CTemplateField::Copy()
//
// 	Description:	This function will copy the members of the source frame.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplateField::Copy(CTemplateField& rSource)
{
	//	Initialize the class members
	m_iId = rSource.m_iId;
	m_sFont = rSource.m_sFont;
	m_sVAlign = rSource.m_sVAlign;
	m_sHAlign = rSource.m_sHAlign;
	m_fHPos = rSource.m_fHPos;
	m_fVPos = rSource.m_fVPos;
	m_bEnable = rSource.m_bEnable;
	m_bDefault = rSource.m_bDefault;
	m_bPrint = rSource.m_bPrint;
	m_strName = rSource.m_strName;
	m_strPrefix = rSource.m_strPrefix;
	m_strText = rSource.m_strText;
}

//==============================================================================
//
// 	Function Name:	CTemplateField::CTemplateField()
//
// 	Description:	This is the constructor for CTemplateField objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplateField::CTemplateField(int iId, LPCSTR lpszPrefix, LPCSTR lpszName)
{
	//	Set class members to their default values
	Reset();

	//	Now initialize using the caller's values
	Initialize(iId, lpszPrefix, lpszName);
}

//==============================================================================
//
// 	Function Name:	CTemplateField::CTemplateField()
//
// 	Description:	This is the copy constructor for CTemplateField objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplateField::CTemplateField(CTemplateField& rSource)
{
	//	Copy the source frame
	Copy(rSource);
}

//==============================================================================
//
// 	Function Name:	CTemplateField::CTemplateField()
//
// 	Description:	This is the destructor for CTemplateField objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplateField::~CTemplateField()
{

}

//==============================================================================
//
// 	Function Name:	CTemplateField::Initialize()
//
// 	Description:	Called to initialize the object
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplateField::Initialize(int iId, LPCSTR lpszPrefix, LPCSTR lpszName)
{
	m_iId = iId;

	m_strPrefix = lpszPrefix != NULL ? lpszPrefix : "";
	
	if((lpszName != NULL) && (lstrlen(lpszName) > 0))
		m_strName = lpszName;
	else
		m_strName = m_strPrefix; // By default the name and prefix are the same
}

//==============================================================================
//
// 	Function Name:	CTemplateField::Show()
//
// 	Description:	This function is provided as a debugging aide. It will 
//					display the current values of the class members
//
// 	Returns:		IDOK / IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CTemplateField::MsgBox(HWND hWnd)
{
	CString strMember;
	CString	strMsg;

	strMember.Format("Id = %d\n", m_iId);
	strMsg += strMember;

	strMember.Format("Prefix = %s\n", m_strPrefix);
	strMsg += strMember;

	strMember.Format("Name = %s\n", m_strName);
	strMsg += strMember;

	strMember.Format("Text = %s\n", m_strText);
	strMsg += strMember;

	strMember.Format("Enable = %s\n", m_bEnable ? "TRUE" : "FALSE");
	strMsg += strMember;

	strMember.Format("Default = %s\n", m_bDefault ? "TRUE" : "FALSE");
	strMsg += strMember;

	strMember.Format("Print = %s\n", m_bPrint ? "TRUE" : "FALSE");
	strMsg += strMember;

	strMember.Format("Horz Pos = %f\n", m_fHPos);
	strMsg += strMember;

	strMember.Format("Vert Pos = %f\n", m_fVPos);
	strMsg += strMember;

	switch(m_sVAlign)
	{
		case TEMPLATE_ALIGNTOP:		strMember.Format("VAlign = %s\n", "Top");
									break;
		case TEMPLATE_ALIGNBOTTOM:	strMember.Format("VAlign = %s\n", "Bottom");
									break;
		case TEMPLATE_ALIGNCENTER:	strMember.Format("VAlign = %s\n", "Center");
									break;
		default:					strMember.Format("VAlign = %s\n", "Invalid");
									break;
	}
	strMsg += strMember;

	switch(m_sHAlign)
	{
		case TEMPLATE_ALIGNLEFT:	strMember.Format("HAlign = %s\n", "Left");
									break;
		case TEMPLATE_ALIGNRIGHT:	strMember.Format("HAlign = %s\n", "Right");
									break;
		case TEMPLATE_ALIGNCENTER:	strMember.Format("HAlign = %s\n", "Center");
									break;
		default:					strMember.Format("HAlign = %s\n", "Invalid");
									break;
	}
	strMsg += strMember;

	strMember.Format("Font = %d\n", m_sFont);
	strMsg += strMember;

	return MessageBox(hWnd, strMsg, "TemplateField", MB_ICONINFORMATION | MB_OKCANCEL);
}

//==============================================================================
//
// 	Function Name:	CTemplateField::Read()
//
// 	Description:	This function will read the template field descriptor 
//					stored in the specified file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplateField::Read(CTMIni& rIni)
{
	char	szLine[64];
	char	szAlign[4];
	char	szIniStr[256];

	ASSERT(lstrlen(m_strPrefix) > 0);

	//	Get the name used to identify this field
	sprintf_s(szLine, sizeof(szLine), "%s%s", m_strPrefix, FIELDNAME_LINE);
	rIni.ReadString(szLine, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strName = szIniStr;

	//	Should we assign default name?
	if(lstrlen(m_strName) == 0)
		m_strName = m_strPrefix; // Assign prefix as default name

	//	Get the text to be written in this field
	sprintf_s(szLine, sizeof(szLine), "%s%s", m_strPrefix, FIELDTEXT_LINE);
	rIni.ReadString(szLine, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strText = szIniStr;

	//	Get the font specification
	sprintf_s(szLine, sizeof(szLine), "%s%s", m_strPrefix, FIELDFONT_LINE);
	m_sFont = (short)rIni.ReadLong(szLine, DEFAULT_TEMPLATE_FIELDFONT);

	//	Get the enabled specification
	//
	//	NOTE:	This was changed in version 5.0 of the TM_Print control. The
	//			original meaning of this flag was to enable/disable the
	//			availability of the field. The bDefault member was added
	//			and this flag now indicates whether or not the default is to
	//			print this field. The absence of this line now means that
	//			the field is not available for printing.
	sprintf_s(szLine, sizeof(szLine), "%s%s", m_strPrefix, FIELDENABLE_LINE);
	rIni.ReadString(szLine, szIniStr, sizeof(szIniStr));
	lstrcat(szLine, " - ");
	lstrcat(szLine, szIniStr);

	if(lstrlen(szIniStr) == 0)
	{
		m_bEnable = FALSE;
		m_bDefault = FALSE;
	}
	else
	{
		m_bEnable = TRUE;
		if(lstrcmpi(szIniStr, "FALSE") == 0)
			m_bDefault = FALSE;
		else
			m_bDefault = TRUE;
	}

	//	Get the verticle position specification
	sprintf_s(szLine, sizeof(szLine), "%s%s", m_strPrefix, FIELDVPOS_LINE);
	m_fVPos = (float)rIni.ReadDouble(szLine, DEFAULT_TEMPLATE_FIELDVPOS);

	//	Get the horizontal position specification
	sprintf_s(szLine, sizeof(szLine), "%s%s", m_strPrefix, FIELDHPOS_LINE);
	m_fHPos = (float)rIni.ReadDouble(szLine, DEFAULT_TEMPLATE_FIELDHPOS);

	//	Get the verticle alignment specification
	sprintf_s(szLine, sizeof(szLine), "%s%s", m_strPrefix, FIELDVALIGN_LINE);
	rIni.ReadString(szLine, szAlign, sizeof(szAlign));
	switch(szAlign[0])
	{
		case 'T':
		case 't':	m_sVAlign = TEMPLATE_ALIGNTOP;
					break;

		case 'B':
		case 'b':	m_sVAlign = TEMPLATE_ALIGNBOTTOM;
					break;

		default:	m_sVAlign = TEMPLATE_ALIGNCENTER;
					break;
	}

	//	Get the horizontal alignment specification
	sprintf_s(szLine, sizeof(szLine), "%s%s", m_strPrefix, FIELDHALIGN_LINE);
	rIni.ReadString(szLine, szAlign, sizeof(szAlign));
	switch(szAlign[0])
	{
		case 'L':
		case 'l':	m_sHAlign = TEMPLATE_ALIGNLEFT;
					break;

		case 'R':
		case 'r':	m_sHAlign = TEMPLATE_ALIGNRIGHT;
					break;

		default:	m_sHAlign = TEMPLATE_ALIGNCENTER;
					break;
	}

	//	Initialize to use the default value
	m_bPrint = m_bDefault;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTemplateField::Reset()
//
// 	Description:	Called to reset class members to their default values
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplateField::Reset()
{
	m_iId = 0;
	m_sFont = 0;
	m_sVAlign = TEMPLATE_ALIGNCENTER;
	m_sHAlign = TEMPLATE_ALIGNCENTER;
	m_fHPos = 0;
	m_fVPos = 0;
	m_bEnable = FALSE;
	m_bDefault = FALSE;
	m_strPrefix = "";
	m_strName = "";
	m_strText = "";
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::Add()
//
// 	Description:	This function will add a link object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplateFields::Add(CTemplateField* pField)
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
// 	Function Name:	CTemplateField::CTemplateFields()
//
// 	Description:	This is the constructor for CTemplateFields objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplateFields::CTemplateFields() : CObList()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;

	InitializeCriticalSection(&m_Lock);
}

//==============================================================================
//
// 	Function Name:	CTemplateField::~CTemplateFields()
//
// 	Description:	This is the destructor for CTemplateFields objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplateFields::~CTemplateFields()
{
	//	Flush the list and destroy its objects
	Flush(TRUE);
		
	DeleteCriticalSection(&m_Lock);
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CTemplateFields::Find(CTemplateField* pField)
{
	return (CObList::Find(pField));
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::Find()
//
// 	Description:	Called to find the object with the specified Id
//
// 	Returns:		The object is found
//
//	Notes:			None
//
//==============================================================================
CTemplateField* CTemplateFields::Find(int iId)
{
	CTemplateField* pField = NULL;
	POSITION		Pos = NULL;

	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pField = (CTemplateField*)GetNext(Pos)) != NULL)
		{
			if(pField->m_iId == iId)
				return pField;		
		}

	}// while(Pos != NULL)

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::Find()
//
// 	Description:	Called to find the object with the specified name
//
// 	Returns:		The object is found
//
//	Notes:			None
//
//==============================================================================
CTemplateField* CTemplateFields::Find(LPCSTR lpszName)
{
	CTemplateField* pField = NULL;
	POSITION		Pos = NULL;

	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pField = (CTemplateField*)GetNext(Pos)) != NULL)
		{
			if(pField->m_strName.CompareNoCase(lpszName) == 0)
				return pField;		
		}

	}// while(Pos != NULL)

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CTemplateField* CTemplateFields::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CTemplateField*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CTemplateFields::Flush(BOOL bDelete)
{
	CTemplateField* pField;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pField = (CTemplateField*)GetNext(m_NextPos)) != 0)
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
// 	Function Name:	CTemplateFields::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CTemplateField* CTemplateFields::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CTemplateField*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CTemplateField* CTemplateFields::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CTemplateField*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CTemplateField* CTemplateFields::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CTemplateField*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTemplateFields::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplateFields::Remove(CTemplateField* pField, BOOL bDelete)
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
// 	Function Name:	CTemplate::CTemplate()
//
// 	Description:	This is the constructor for CTemplate objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplate::CTemplate()
{
	//	Initialize the class members
	m_fTopMargin			= 0;
	m_fLeftMargin			= 0;
	m_fCellHeight			= 0;
	m_fCellWidth			= 0;
	m_fCellSpaceWidth		= 0;
	m_fCellSpaceHeight		= 0;
	m_fCellPadWidth			= 0;
	m_fCellPadHeight		= 0;
	m_fImagePadWidth		= 0;
	m_fImagePadHeight		= 0;
	m_sRows					= 0;
	m_sColumns				= 0;
	m_sOrientation			= 0;
	m_bImageEnable			= FALSE;
	m_bDefaultImage			= TRUE;
	m_bPrintImage			= TRUE;
	m_bPrintBorder			= TRUE;
	m_bDefaultBorder		= TRUE;
	m_bPrintFullPath		= TRUE;
	m_bPageAsSeries			= TRUE;
	m_bAutoRotateEnable		= FALSE;
	m_bDefaultAutoRotate	= FALSE;
	m_bAutoRotate			= FALSE;
	m_strDescription		= "";

	//	Initialize the fixed fields
	m_aFixedFields[TEMPLATE_NAME].Initialize(TEMPLATE_NAME, "Name");
	m_aFixedFields[TEMPLATE_BARCODE].Initialize(TEMPLATE_BARCODE, "Barcode");
	m_aFixedFields[TEMPLATE_GRAPHIC].Initialize(TEMPLATE_GRAPHIC, "BarcodeGraphic", "Barcode Graphic");
	m_aFixedFields[TEMPLATE_FILENAME].Initialize(TEMPLATE_FILENAME, "Filename");
	m_aFixedFields[TEMPLATE_PAGENUM].Initialize(TEMPLATE_PAGENUM, "PageNum", "Page Number");
	m_aFixedFields[TEMPLATE_DEPONENT].Initialize(TEMPLATE_DEPONENT, "Deponent");
	m_aFixedFields[TEMPLATE_FOREIGN_BARCODE].Initialize(TEMPLATE_FOREIGN_BARCODE, "ForeignBarcode", "Foreign Barcode");
	m_aFixedFields[TEMPLATE_SOURCE_BARCODE].Initialize(TEMPLATE_SOURCE_BARCODE, "SourceBarcode", "Source Barcode");
}

//==============================================================================
//
// 	Function Name:	CTemplate::CTemplate()
//
// 	Description:	This is the copy constructor for CTemplate objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplate::CTemplate(CTemplate& rSource)
{
	//	Copy the source template
	Copy(rSource);
}

//==============================================================================
//
// 	Function Name:	CTemplate::~CTemplate()
//
// 	Description:	This is the destructor for CTemplate objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplate::~CTemplate()
{
	m_aTextFields.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTemplate::GetDefaultMask()
//
// 	Description:	This function is called to get the mask that represents the
//					fields in this template that are turned on by default.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
long CTemplate::GetDefaultMask()
{
	long lMask = 0;

	if(m_bDefaultBorder == TRUE)
		lMask |= TEMPLATE_BITMASK_CELL_BORDER;

	if(m_bDefaultAutoRotate == TRUE)
		lMask |= TEMPLATE_BITMASK_AUTO_ROTATE;

	if(m_bDefaultImage == TRUE)
		lMask |= TEMPLATE_BITMASK_IMAGE;

	if(m_aFixedFields[TEMPLATE_GRAPHIC].m_bDefault == TRUE)
		lMask |= TEMPLATE_BITMASK_GRAPHIC;

	if(m_aFixedFields[TEMPLATE_BARCODE].m_bDefault == TRUE)
		lMask |= TEMPLATE_BITMASK_BARCODE;

	if(m_aFixedFields[TEMPLATE_NAME].m_bDefault == TRUE)
		lMask |= TEMPLATE_BITMASK_NAME;

	if(m_aFixedFields[TEMPLATE_FILENAME].m_bDefault == TRUE)
		lMask |= TEMPLATE_BITMASK_FILENAME;

	if(m_aFixedFields[TEMPLATE_PAGENUM].m_bDefault == TRUE)
		lMask |= TEMPLATE_BITMASK_PAGENUM;

	if(m_aFixedFields[TEMPLATE_DEPONENT].m_bDefault == TRUE)
		lMask |= TEMPLATE_BITMASK_DEPONENT;

	if(m_aFixedFields[TEMPLATE_FOREIGN_BARCODE].m_bDefault == TRUE)
		lMask |= TEMPLATE_BITMASK_FOREIGN_BARCODE;

	if(m_aFixedFields[TEMPLATE_SOURCE_BARCODE].m_bDefault == TRUE)
		lMask |= TEMPLATE_BITMASK_SOURCE_BARCODE;

	return lMask;
}

//==============================================================================
//
// 	Function Name:	CTemplate::GetDefault()
//
// 	Description:	Called to get the default enable/disable value
//
// 	Returns:		TRUE if enabled by default
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplate::GetDefault(int iFixedField)
{
	ASSERT(iFixedField >= 0);
	ASSERT(iFixedField < TEMPLATE_MAX_FIXED_FIELDS);
	
	if((iFixedField >= 0) && (iFixedField < TEMPLATE_MAX_FIXED_FIELDS))
		return (m_aFixedFields[iFixedField].m_bDefault);
	else
		return FALSE;	
}

//==============================================================================
//
// 	Function Name:	CTemplate::GetEnabledMask()
//
// 	Description:	This function is called to get the mask that represents the
//					fields in this template that are enabled.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
long CTemplate::GetEnabledMask()
{
	long lMask = TEMPLATE_BITMASK_CELL_BORDER;

	if(m_bImageEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_IMAGE;

	if(m_bAutoRotateEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_AUTO_ROTATE;

	if(m_aFixedFields[TEMPLATE_GRAPHIC].m_bEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_GRAPHIC;

	if(m_aFixedFields[TEMPLATE_BARCODE].m_bEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_BARCODE;

	if(m_aFixedFields[TEMPLATE_NAME].m_bEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_NAME;

	if(m_aFixedFields[TEMPLATE_FILENAME].m_bEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_FILENAME;

	if(m_aFixedFields[TEMPLATE_PAGENUM].m_bEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_PAGENUM;

	if(m_aFixedFields[TEMPLATE_DEPONENT].m_bEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_DEPONENT;

	if(m_aFixedFields[TEMPLATE_FOREIGN_BARCODE].m_bEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_FOREIGN_BARCODE;

	if(m_aFixedFields[TEMPLATE_SOURCE_BARCODE].m_bEnable == TRUE)
		lMask |= TEMPLATE_BITMASK_SOURCE_BARCODE;

	return lMask;
}

//==============================================================================
//
// 	Function Name:	CTemplate::Copy()
//
// 	Description:	This function will copy the members of the source template.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplate::Copy(CTemplate& rSource)
{
	CTemplateField* pField = NULL;

	//	Initialize the class members
	m_fTopMargin			= rSource.m_fTopMargin;
	m_fLeftMargin			= rSource.m_fLeftMargin;
	m_fCellHeight			= rSource.m_fCellHeight;
	m_fCellWidth			= rSource.m_fCellWidth;
	m_fCellSpaceWidth		= rSource.m_fCellSpaceWidth;
	m_fCellSpaceHeight		= rSource.m_fCellSpaceHeight;
	m_fCellPadWidth			= rSource.m_fCellPadWidth;
	m_fCellPadHeight		= rSource.m_fCellPadHeight;
	m_fImagePadWidth		= rSource.m_fImagePadWidth;
	m_fImagePadHeight		= rSource.m_fImagePadHeight;
	m_sRows					= rSource.m_sRows;
	m_sColumns				= rSource.m_sColumns;
	m_sOrientation			= rSource.m_sOrientation;
	m_bImageEnable			= rSource.m_bImageEnable;
	m_bDefaultImage			= rSource.m_bDefaultImage;
	m_bPrintImage			= rSource.m_bPrintImage;
	m_bPrintBorder			= rSource.m_bPrintBorder;
	m_bPrintFullPath		= rSource.m_bPrintFullPath;
	m_bPageAsSeries			= rSource.m_bPageAsSeries;
	m_bDefaultBorder		= rSource.m_bDefaultBorder;
	m_bAutoRotateEnable		= rSource.m_bAutoRotateEnable;
	m_bDefaultAutoRotate	= rSource.m_bDefaultAutoRotate;
	m_bAutoRotate			= rSource.m_bAutoRotate;
	m_strDescription		= rSource.m_strDescription;

	for(int i = 0; i < TEMPLATE_MAX_FIXED_FIELDS; i++)
		m_aFixedFields[i].Copy(rSource.m_aFixedFields[i]);

	m_aTextFields.Flush(TRUE);
	pField = rSource.m_aTextFields.First();
	while(pField != NULL)
	{
		m_aTextFields.Add(new CTemplateField(*pField));
		pField = rSource.m_aTextFields.Next();
	}

}

//==============================================================================
//
// 	Function Name:	CTemplate::GetFieldEnabled()
//
// 	Description:	Called to determine if the specified field should be
//					made available for the print job
//
// 	Returns:		TRUE if enabled
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplate::GetFieldEnabled(int iFixedField)
{
	ASSERT(iFixedField >= 0);
	ASSERT(iFixedField < TEMPLATE_MAX_FIXED_FIELDS);
	
	if((iFixedField >= 0) && (iFixedField < TEMPLATE_MAX_FIXED_FIELDS))
		return (m_aFixedFields[iFixedField].m_bEnable);
	else
		return FALSE;	
}

//==============================================================================
//
// 	Function Name:	CTemplate::GetPrintEnabled()
//
// 	Description:	Called to determine if the specified field should be
//					included in the print job
//
// 	Returns:		TRUE if enabled
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplate::GetPrintEnabled(int iFixedField)
{
	ASSERT(iFixedField >= 0);
	ASSERT(iFixedField < TEMPLATE_MAX_FIXED_FIELDS);
	
	if((iFixedField >= 0) && (iFixedField < TEMPLATE_MAX_FIXED_FIELDS))
		return (m_aFixedFields[iFixedField].m_bEnable && m_aFixedFields[iFixedField].m_bPrint);
	else
		return FALSE;	
}

//==============================================================================
//
// 	Function Name:	CTemplate::MsgBox()
//
// 	Description:	This member is provided as a debugging aid to display the
//					current template settings
//
// 	Returns:		IDOK or IDCANCEL
//
//	Notes:			None
//
//==============================================================================
int CTemplate::MsgBox(HWND hParent)
{
	CString			Msg = "";
	CString			Tmp = "";
	CTemplateField*	pField = NULL;

	Tmp.Format("Description: %s\n", m_strDescription); Msg += Tmp;
	Tmp.Format("Top Margin: %0.3f\n", m_fTopMargin); Msg += Tmp;
	Tmp.Format("Left Margin: %0.3f\n", m_fLeftMargin); Msg += Tmp;
	Tmp.Format("CellHeight: %0.3f\n", m_fCellHeight); Msg += Tmp;
	Tmp.Format("Cell Width: %0.3f\n", m_fCellWidth); Msg += Tmp;
	Tmp.Format("Cell Pad Width: %0.3f\n", m_fCellPadWidth); Msg += Tmp;
	Tmp.Format("Cell Pad Height: %0.3f\n", m_fCellPadHeight); Msg += Tmp;
	Tmp.Format("Cell Space Width: %0.3f\n", m_fCellSpaceWidth); Msg += Tmp;
	Tmp.Format("Cell Space Height: %0.3f\n", m_fCellSpaceHeight); Msg += Tmp;
	Tmp.Format("Image Pad Width: %0.3f\n", m_fImagePadWidth); Msg += Tmp;
	Tmp.Format("Image Pad Height: %0.3f\n", m_fImagePadHeight); Msg += Tmp;
	Tmp.Format("Rows per page: %d\n", m_sRows); Msg += Tmp;
	Tmp.Format("Columns per page: %d\n\n", m_sColumns); Msg += Tmp;
	Tmp.Format("Auto Rotate: %s\n", (m_bAutoRotate) ? "TRUE" : "FALSE"); Msg += Tmp;
	Tmp.Format("Print Image: %s\n", (m_bPrintImage) ? "TRUE" : "FALSE"); Msg += Tmp;
	
	if(MessageBox(hParent, Msg, "Page Template", MB_ICONINFORMATION | MB_OKCANCEL) == IDCANCEL)
		return IDCANCEL;

	for(int i = 0; i < TEMPLATE_MAX_FIXED_FIELDS; i++)
	{
		if(m_aFixedFields[i].MsgBox(hParent) == IDCANCEL)
			return IDCANCEL;
	}

	pField = m_aTextFields.First();
	while(pField != NULL)
	{
		if(pField->MsgBox(hParent) == IDCANCEL)
			return IDCANCEL;
		else
			pField = m_aTextFields.Next();
	}

	return IDOK;
}

//==============================================================================
//
// 	Function Name:	CTemplate::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplate::operator < (const CTemplate& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTemplate::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplate::operator == (const CTemplate& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTemplate::ReadFile()
//
// 	Description:	This function will read the template descriptor stored
//					in the specified file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplate::ReadFile(CTMIni& rIni, LPCSTR lpSection)
{
	CTemplateField*	pField = NULL;
	char			szIniStr[256];
	char			szPrefix[32];
	int				i = 0;

	ASSERT(lpSection);

	//	Line up on the section for this template
	rIni.SetSection(lpSection);

	//	Get the page specifications first
	m_fTopMargin = (float)rIni.ReadDouble(TOPMARGIN_LINE, DEFAULT_TEMPLATE_TOPMARGIN);
	m_fLeftMargin = (float)rIni.ReadDouble(LEFTMARGIN_LINE, DEFAULT_TEMPLATE_LEFTMARGIN);
	m_sRows = (int)rIni.ReadLong(ROWS_LINE, DEFAULT_TEMPLATE_ROWS);
	m_sColumns = (int)rIni.ReadLong(COLUMNS_LINE, DEFAULT_TEMPLATE_COLUMNS);
	m_fCellHeight = (float)rIni.ReadDouble(CELLHEIGHT_LINE, DEFAULT_TEMPLATE_CELLHEIGHT);
	m_fCellWidth = (float)rIni.ReadDouble(CELLWIDTH_LINE, DEFAULT_TEMPLATE_CELLWIDTH);
	m_fCellSpaceWidth = (float)rIni.ReadDouble(CELLSPACEWIDTH_LINE, DEFAULT_TEMPLATE_CELLSPACEWIDTH);
	m_fCellSpaceHeight = (float)rIni.ReadDouble(CELLSPACEHEIGHT_LINE, DEFAULT_TEMPLATE_CELLSPACEHEIGHT);
	m_fCellPadWidth = (float)rIni.ReadDouble(CELLPADWIDTH_LINE, DEFAULT_TEMPLATE_CELLPADWIDTH);
	m_fCellPadHeight = (float)rIni.ReadDouble(CELLPADHEIGHT_LINE, DEFAULT_TEMPLATE_CELLPADHEIGHT);
	m_fImagePadHeight = (float)rIni.ReadDouble(IMAGEPADHEIGHT_LINE, DEFAULT_TEMPLATE_IMAGEPADHEIGHT);
	m_fImagePadWidth = (float)rIni.ReadDouble(IMAGEPADWIDTH_LINE, DEFAULT_TEMPLATE_IMAGEPADWIDTH);
	
	m_bDefaultBorder = rIni.ReadBool(PRINTBORDER_LINE, DEFAULT_TEMPLATE_PRINTBORDER);

	//	These guys are too lazy to make sure the ImageEnable line is in the
	//	ini file so we always enable the image field and set the default
	//	print value to true if it does not exist in the file
	m_bImageEnable = TRUE;
	m_bDefaultImage = rIni.ReadBool(IMAGEENABLE_LINE, TRUE);

	//	Get the orientation
	//
	//	NOTE:	We read this in as a string instead of a boolean value to allow
	//			for backward compatability. Template descriptors prior to version
	//			5.0 did not allow for the Landscape option
	rIni.ReadString(LANDSCAPE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_TEMPLATE_LANDSCAPE);
	if(lstrlen(szIniStr) == 0)
	{
		m_sOrientation = TEMPLATE_ORIENTATION_UNKNOWN;
	}
	else if(lstrcmpi(szIniStr, "TRUE") == 0)
	{
		m_sOrientation = TEMPLATE_ORIENTATION_LANDSCAPE;
	}
	else
	{
		m_sOrientation = TEMPLATE_ORIENTATION_PORTRAIT;
	}

	//	Get the auto rotate option
	//
	//	NOTE:	We read this in as a string instead of a boolean value to allow
	//			for backward compatability. Template descriptors prior to version
	//			6.0 did not allow for the AutoRotate option
	rIni.ReadString(AUTOROTATE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_TEMPLATE_AUTOROTATE);
	if(lstrlen(szIniStr) == 0)
	{
		//	Disable the ability to auto rotate if this line is not in the INI file
		m_bAutoRotateEnable = FALSE;
		m_bDefaultAutoRotate = FALSE;
	}
	else
	{
		m_bAutoRotateEnable = TRUE;

		if(lstrcmpi(szIniStr, "TRUE") == 0)
			m_bDefaultAutoRotate = TRUE;
		else
			m_bDefaultAutoRotate = FALSE;
	}

	//	Now read in the specifications for each fixed field
	for(i = 0; i < TEMPLATE_MAX_FIXED_FIELDS; i++)
		m_aFixedFields[i].Read(rIni);

	//	Read in the optional text field descriptors
	m_aTextFields.Flush(TRUE);
	for(i = 1; ; i++)
	{
		//	Allocate and initialize a new field
		sprintf_s(szPrefix, "Text%d", i);
		pField = new CTemplateField(i, szPrefix);
		
		//	Read the descriptor stored in the ini file
		pField->Read(rIni);
		
		//	Have we run out of text fields?
		if(pField->m_bEnable == FALSE)
		{
			delete pField;
			break;
		}
		else
		{
			//	Add to the collection
			m_aTextFields.Add(pField);
		}
				
	}// for(int i = 1; ; i++)

	//	Initialize the runtime members to match the stored values
	//
	//	NOTE:	m_bPrintFullPath and m_bPageAsSeries are not retrieved as
	//			part of the template descriptor. They are set by the control
	//			before starting a print job
	m_bPrintImage			= m_bDefaultImage;
	m_bPrintBorder			= m_bDefaultBorder;
	m_bAutoRotate			= m_bDefaultAutoRotate;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTemplate::SetPrintEnabled()
//
// 	Description:	Called to enable/disable printing of the specified fixed 
//					field
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTemplate::SetPrintEnabled(int iFixedField, BOOL bEnabled)
{
	ASSERT(iFixedField >= 0);
	ASSERT(iFixedField < TEMPLATE_MAX_FIXED_FIELDS);
	
	if((iFixedField >= 0) && (iFixedField < TEMPLATE_MAX_FIXED_FIELDS))
	{
		m_aFixedFields[iFixedField].m_bPrint = bEnabled;
	}	
}

//==============================================================================
//
// 	Function Name:	CTemplates::Add()
//
// 	Description:	This function will add the object pointer to the list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplates::Add(CTemplate* pTemplate)
{
	//	Add at the head of the list if the list is empty
	if(IsEmpty())
		AddHead(pTemplate);
	else
		AddTail(pTemplate);
}

//==============================================================================
//
// 	Function Name:	CTemplates::CTemplates()
//
// 	Description:	This is the constructor for CTemplates objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplates::CTemplates() : CObList(1)
{
	m_Pos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTemplates::~CTemplates()
//
// 	Description:	This is the destructor for CTemplates objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplates::~CTemplates()
{
	//	Delete any objects currently in the list
	Flush(TRUE);

}

//==============================================================================
//
// 	Function Name:	CTemplates::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CTemplates::Find(CTemplate* pTemplate)
{
	return (CObList::Find(pTemplate));
}

//==============================================================================
//
// 	Function Name:	CTemplates::Find()
//
// 	Description:	This function will locate the template with the name 
//					specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplate* CTemplates::Find(LPCSTR lpszTemplate)
{
	CTemplate*	pTemplate = 0;
	POSITION	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		pTemplate = (CTemplate*)GetNext(Pos);
		if(pTemplate != 0)
		{
			if(lstrcmpi(pTemplate->m_strDescription, lpszTemplate) == 0)
				return pTemplate;
		}
	}

	return pTemplate;
}

//==============================================================================
//
// 	Function Name:	CTemplates::Flush()
//
// 	Description:	This function will flush all CTemplate objects from the 
//					list. If bDeleteAll is TRUE, the objects are deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplates::Flush(BOOL bDeleteAll)
{
	//	Do we want to delete the objects?
	if(bDeleteAll)
	{
		m_Pos = GetHeadPosition();

		while(m_Pos != NULL)
		{
			CTemplate* pTemplate = (CTemplate*)GetNext(m_Pos);
			if(pTemplate != NULL)
				delete pTemplate;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();

	//	The position is no longer valid
	m_Pos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTemplates::GetFirstTemplate()
//
// 	Description:	This function will retrieve the first template object in the
//					list.
//
// 	Returns:		A pointer to the first object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CTemplate* CTemplates::GetFirstTemplate()
{
	//	Get the first position
	m_Pos = GetHeadPosition();

	if(m_Pos == NULL)
		return NULL;
	else
		return (CTemplate*)GetNext(m_Pos);
}

//==============================================================================
//
// 	Function Name:	CTemplates::GetLastTemplate()
//
// 	Description:	This function will retrieve the last template object in the
//					list.
//
// 	Returns:		A pointer to the last object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CTemplate* CTemplates::GetLastTemplate()
{
	//	Get the last position
	m_Pos = GetTailPosition();

	if(m_Pos == NULL)
		return NULL;
	else
		return (CTemplate*)GetPrev(m_Pos);
}

//==============================================================================
//
// 	Function Name:	CTemplates::GetNextTemplate()
//
// 	Description:	This function will retrieve the next template object in the
//					list.
//
// 	Returns:		A pointer to the next object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CTemplate* CTemplates::GetNextTemplate()
{
	if(m_Pos == NULL)
		return NULL;
	else
		return (CTemplate*)GetNext(m_Pos);
}

//==============================================================================
//
// 	Function Name:	CTemplates::GetPrevTemplate()
//
// 	Description:	This function will retrieve the previous template object in the
//					list.
//
// 	Returns:		A pointer to the previous object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CTemplate* CTemplates::GetPrevTemplate()
{
	if(m_Pos == NULL)
		return NULL;
	else
		return (CTemplate*)GetPrev(m_Pos);
}

//==============================================================================
//
// 	Function Name:	CTemplates::MsgBox()
//
// 	Description:	This function is provided as a debugging aid. It will
//					iterate the list and call the Show() member of each 
//					object in the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplates::MsgBox(HWND hParent)
{
	POSITION Pos;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		CTemplate* pTemplate = (CTemplate*)GetNext(Pos);
		if(pTemplate != NULL)
			if(pTemplate->MsgBox(hParent) == IDCANCEL)
				break;
	}

}

//==============================================================================
//
// 	Function Name:	CTemplates::ReadFile()
//
// 	Description:	This function will read the template specifications stored
//					in the specified file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTemplates::ReadFile(LPCSTR lpFilename, LPCSTR lpSection, BOOL bFlush)
{
	CTMIni		Ini;
	CTemplate*	pTemplate = 0;
	int			i = 1;
	char		szIniStr[256];
	char		szSection[32];

	ASSERT(lpFilename);
	ASSERT(lpSection);

	//	Open the file
	if(Ini.Open(lpFilename, lpSection) == FALSE)
		return FALSE;

	//	Should we flush the list?
	if(bFlush)
		Flush(TRUE);

	//	Read each template defined in the file
	while(1)
	{
		//	Set the appropriate section. We have to put this call inside the
		//	loop because the call to CTemplate::ReadFile will change the section
		Ini.SetSection(lpSection);

		//	Read the next description
		Ini.ReadString(i, szIniStr, sizeof(szIniStr));

		//	Have we run out of templates?
		if(lstrlen(szIniStr) == 0)
			break;

		//	Allocate a new template object
		pTemplate = new CTemplate();
		ASSERT(pTemplate);
		pTemplate->m_strDescription = szIniStr;

		//	Read the information for the new template
		sprintf_s(szSection, "TEMPLATE%d", i);
		if(pTemplate->ReadFile(Ini, szSection))
			Add(pTemplate);
		else
			delete pTemplate;

		pTemplate = 0;

		//	Next Line
		i++;
	}
		 
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTemplates::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTemplates::Remove(CTemplate* pTemplate, BOOL bDelete)
{
	POSITION Pos = Find(pTemplate);

	//	Is this object in the list
	if(Pos != NULL)
		RemoveAt(Pos);

	//	Do we need to delete the object?
	if(bDelete)
		delete pTemplate;
}

