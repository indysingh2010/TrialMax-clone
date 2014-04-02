//==============================================================================
//
// File Name:	transcpt.cpp
//
// Description:	This file contains member functions of the CTranscript and
//				CTranscripts classes
//
// See Also:	transcpt.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-20-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <transcpt.h>
#include <dbdefs.h> // DELETE_INTERFACE  RELEASE_INTERFACE
#include <msxml3.h>
#include <designat.h>

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

//==============================================================================
//
// 	Function Name:	CTranscript::Close()
//
// 	Description:	Called to close the associated XML file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTranscript::Close()
{
	if(m_pXmlDocument != NULL)
	{
		DELETE_INTERFACE(m_pXmlDocument);		
	}
	m_strXmlFileSpec = "";
}

//==============================================================================
//
// 	Function Name:	CTranscript::CTranscript()
//
// 	Description:	This is the constructor for CTranscript objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTranscript::CTranscript()
{
	m_pXmlDocument = NULL;
	m_lTranscriptId = 0;
	m_lAttributes = 0;
	m_lPrimaryMediaId = 0;
	m_lFirstPL = 0;
	m_lLastPL = 0;
	m_lAliasId = 0;
	m_sLinesPerPage = 0;
	m_bLinked = FALSE;
	m_strAltBarcode = "";
	m_strFilename = "";
	m_strBaseFilename = "";
	m_strCtxExtension = "";
	m_strDbExtension = "";
	m_strTranscriptName = "";
	m_strRelativePath = "";
	m_strDate = "";
	m_strXmlFileSpec = "";
}

//==============================================================================
//
// 	Function Name:	CTranscript::~CTranscript()
//
// 	Description:	This is the destructor for CTranscript objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTranscript::~CTranscript()
{
	//	Make sure the XML interface is closed
	Close();
}

//==============================================================================
//
// 	Function Name:	CTranscript::CreateXmlDocument()
//
// 	Description:	This function is called to create an XML document object
//
// 	Returns:		A pointer to the new XML document interface
//
//	Notes:			The caller is responsible for deallocation of the interface
//
//==============================================================================
CIXMLDOMDocument* CTranscript::CreateXmlDocument()
{
	CIXMLDOMDocument*	pXmlDocument = NULL;
	COleException		OE;
	CLSID				ClassId;

	//	Get the Class ID for the XML parser
	if(CLSIDFromProgID(L"Microsoft.XMLDOM", &ClassId) != S_OK)
	{
		return NULL;
	}

	//	Allocate a new XML document interface
	pXmlDocument = new CIXMLDOMDocument();
	ASSERT(pXmlDocument);

	//	Open the interface to the XML Parser
	if(!pXmlDocument->CreateDispatch(ClassId, &OE))
	{
		return NULL;
	}
	else
	{ 
		//	Force synchronous loading of the file 
		pXmlDocument->SetAsync(FALSE);

		return pXmlDocument;
	}
}

//==============================================================================
//
// 	Function Name:	CTranscript::GetText()
//
// 	Description:	This function is called to get the text for the specified
//					segment and store it in the designation
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTranscript::GetText(long lSegmentId, CDesignation* pDesignation)
{
	CIXMLDOMNodeList*	pXmlTranscripts = NULL;
	CString				strXPath = "";
	BOOL				bSuccessful = FALSE;

	//	The document should already be open
	ASSERT(m_pXmlDocument != NULL);
	if(m_pXmlDocument == NULL) return FALSE;

	//	Format the XPath query
	strXPath.Format("trialmax/deposition/segment/transcript[@segment=\"%ld\"]", lSegmentId);

	while(bSuccessful == FALSE)
	{
		//	Get the collection of transcript lines
		pXmlTranscripts = new CIXMLDOMNodeList(m_pXmlDocument->selectNodes(strXPath));
		if(pXmlTranscripts->m_lpDispatch == NULL)
			break;

		//	Let the designation process the transcripts nodes
		bSuccessful = pDesignation->GetText(pXmlTranscripts);
	
	}// while(bSuccessful == FALSE)

	//	Clean up
	DELETE_INTERFACE(pXmlTranscripts);		

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CTranscript::MsgBox()
//
// 	Description:	This is for debugging purposes to view the values assigned
//					to the class members
//
// 	Returns:		MB_OK or MB_CANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CTranscript::MsgBox(HWND hWnd, LPCSTR lpszTitle)
{
	CString	strTemp = "";
	CString	strTitle = "";
	CString strMsg = "";

	strTemp.Format("PrimaryMediaId: %ld\n", m_lPrimaryMediaId);
	strMsg += strTemp;

	strTemp.Format("TranscriptId: %ld\n", m_lTranscriptId);
	strMsg += strTemp;

	strTemp.Format("Attributes: %ld\n", m_lAttributes);
	strMsg += strTemp;

	strMsg += "\n";

	strTemp.Format("FirstPL: %ld\n", m_lFirstPL);
	strMsg += strTemp;

	strTemp.Format("LastPL: %ld\n", m_lLastPL);
	strMsg += strTemp;

	strTemp.Format("LinesPerPage: %d\n", m_sLinesPerPage);
	strMsg += strTemp;

	strMsg += "\n";

	strTemp.Format("TranscriptName: %s\n", m_strTranscriptName);
	strMsg += strTemp;

	strTemp.Format("Date: %s\n", m_strDate);
	strMsg += strTemp;

	strTemp.Format("AltBarcode: %s\n", m_strAltBarcode);
	strMsg += strTemp;

	strTemp.Format("AliasId: %ld\n", m_lAliasId);
	strMsg += strTemp;

	strTemp.Format("Linked: %d\n", m_bLinked);
	strMsg += strTemp;

	strMsg += "\n";

	strTemp.Format("RelativePath: %s\n", m_strRelativePath);
	strMsg += strTemp;

	strTemp.Format("Filename: %s\n", m_strFilename);
	strMsg += strTemp;

	strTemp.Format("BaseFilename: %s\n", m_strBaseFilename);
	strMsg += strTemp;

	strTemp.Format("CtxExtension: %s\n", m_strCtxExtension);
	strMsg += strTemp;

	strTemp.Format("DbExtension: %s\n", m_strDbExtension);
	strMsg += strTemp;

	strTemp.Format("XmlFileSpec: %s\n", m_strXmlFileSpec);
	strMsg += strTemp;

	if(lpszTitle != NULL)
		strTitle = lpszTitle;

	return MessageBox(hWnd, strMsg, strTitle, MB_ICONINFORMATION | MB_OKCANCEL);
}

//==============================================================================
//
// 	Function Name:	CTranscript::Open()
//
// 	Description:	Called to open the specified XML file that contains the
//					transcript text
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTranscript::Open(LPCSTR lpszXmlFileSpec)
{
	CIXMLDOMDocument*	pXmlDocument = NULL;
	BOOL				bSuccessful = FALSE;
	VARIANT				vFilename;

	//	Close the existing document
	Close();

	m_strXmlFileSpec = lpszXmlFileSpec;

	while(bSuccessful == FALSE)
	{
		//	Attach to the XML engine
		if((pXmlDocument = CreateXmlDocument()) == NULL)
			break;

		VariantInit(&vFilename);
		V_VT(&vFilename) = VT_BSTR;
		V_BSTR(&vFilename) = m_strXmlFileSpec.AllocSysString();

		//	Parse the XML file
		if(pXmlDocument->load(vFilename) == FALSE)
			break;

		//	We're done
		bSuccessful = TRUE;
	
	}// while(bSuccessful == FALSE)

	if(pXmlDocument != NULL)
	{
		VariantClear(&vFilename);

		if(bSuccessful == TRUE)
			m_pXmlDocument = pXmlDocument;
		else
			Close();
	}

	return bSuccessful;
	
}

//==============================================================================
//
// 	Function Name:	CTranscripts::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTranscripts::Add(CTranscript* pTranscript)
{
	ASSERT(pTranscript);
	if(!pTranscript)
		return FALSE;

	//	MFC will throw a memory exception if it can't add the object to the list
	try
	{
		//	Add the link to the end of the list
		AddTail(pTranscript);
		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTranscript::CTranscripts()
//
// 	Description:	This is the constructor for CTranscripts objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTranscripts::CTranscripts()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTranscript::~CTranscripts()
//
// 	Description:	This is the destructor for CTranscripts objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTranscripts::~CTranscripts()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTranscripts::Find()
//
// 	Description:	This function will search the list for the item with the
//					numeric identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CTranscript* CTranscripts::Find(long lId)
{
	CTranscript* pTranscript;
	POSITION	 Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pTranscript = (CTranscript*)GetNext(Pos)) != NULL)
		{
			if(pTranscript->m_lTranscriptId == lId)
				return pTranscript;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTranscripts::Find()
//
// 	Description:	This function will search the list for the item with the
//					primary identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CTranscript* CTranscripts::FindByPrimary(long lPrimary)
{
	CTranscript* pTranscript;
	POSITION	 Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pTranscript = (CTranscript*)GetNext(Pos)) != NULL)
		{
			if(pTranscript->m_lPrimaryMediaId == lPrimary)
				return pTranscript;
		}
	}

	return 0;
}
//==============================================================================
//
// 	Function Name:	CTranscripts::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CTranscripts::Find(CTranscript* pTranscript)
{
	return (CObList::Find(pTranscript));
}

//==============================================================================
//
// 	Function Name:	CTranscripts::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CTranscript* CTranscripts::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CTranscript*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CTranscripts::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CTranscripts::Flush(BOOL bDelete)
{
	CTranscript* pTranscript;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pTranscript = (CTranscript*)GetNext(m_NextPos)) != 0)
				delete pTranscript;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTranscripts::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CTranscript* CTranscripts::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CTranscript*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CTranscripts::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CTranscript* CTranscripts::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CTranscript*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTranscripts::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CTranscript* CTranscripts::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CTranscript*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTranscripts::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTranscripts::Remove(CTranscript* pTranscript, BOOL bDelete)
{
	POSITION Pos = Find(pTranscript);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pTranscript;
	}
}

