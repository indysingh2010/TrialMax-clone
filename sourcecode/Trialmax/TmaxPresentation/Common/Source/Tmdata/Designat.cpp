//==============================================================================
//
// File Name:	designat.cpp
//
// Description:	This file contains member functions of the CDesignation and
//				CDesignations classes.
//
// See Also:	designat.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-11-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <designat.h>
#include <dbdefs.h>
#include <msxml3.h>

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
// 	Function Name:	CDesignation::CDesignation()
//
// 	Description:	This is the constructor for CDesignation objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDesignation::CDesignation() : CTertiary()
{
	//	Initialize the class members
	m_lTranscriptId		= 0;
	m_lVideoId			= 0;
	m_bScrollText		= TRUE;
	m_strOverlay.Empty();
	m_strOverlayRelativePath.Empty();
	m_strOverlayFilename.Empty();
}

//==============================================================================
//
// 	Function Name:	CDesignation::~CDesignation()
//
// 	Description:	This is the destructor for CDesignation objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDesignation::~CDesignation()
{
	m_Pages.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CDesignation::GetElapsedTime()
//
// 	Description:	This function will compute the time that has elapsed between
//					the start of this designation and the specified position
//
// 	Returns:		The elasped time in seconds
//
//	Notes:			None
//
//==============================================================================
double CDesignation::GetElapsedTime(double dPosition)
{
	if(dPosition <= m_dStartTime)
		return 0;
	else if(dPosition >= m_dStopTime)
		return GetTotalTime();
	else
		return (dPosition - m_dStartTime);
}

//==============================================================================
//
// 	Function Name:	CDesignation::FirstPage()
//
// 	Description:	This function will retrieve the first page object in the
//					list.
//
// 	Returns:		A pointer to the first object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CTextPage* CDesignation::FirstPage()
{
	return m_Pages.First();
}

//==============================================================================
//
// 	Function Name:	CDesignation::GetTime()
//
// 	Description:	This function will compute the time required to play this
//					designation between the specified positions
//
// 	Returns:		The time required in seconds
//
//	Notes:			None
//
//==============================================================================
double CDesignation::GetTime(double dStart, double dStop)
{
	//	Do we need to adjust the positions
	if(dStart < m_dStartTime)
		dStart = m_dStartTime;
	if(dStop > m_dStopTime || dStop < m_dStartTime)
		dStop = m_dStopTime;

	return (dStop - dStart);
}

//==============================================================================
//
// 	Function Name:	CDesignation::CDesignation()
//
// 	Description:	Called to determine if the text associated with this record
//					should be scrolled when played
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
BOOL CDesignation::GetScrollTextEnabled()
{
	//	Can't scroll if no text is available
	if(HasText() == FALSE)
		return FALSE;
	else
		return ((m_lAttributes & NET_TERTIARY_SCROLL_TEXT) != 0);
}

//==============================================================================
//
// 	Function Name:	CDesignation::GetText()
//
// 	Description:	This function is called to set the designation text using
//					the XML transcripts collection specified by the caller
//
// 	Returns:		TRUE if text was found in the database
//
//	Notes:			None
//
//==============================================================================
BOOL CDesignation::GetText(CIXMLDOMNodeList* pXmlTranscripts)
{
	long			lNodes = 0;
	CIXMLDOMNode*	pXmlNode = 0;
	CTextLine*		pLine = 0;
	CTextPage*		pPage = 0;

	ASSERT(pXmlTranscripts != NULL);

	//	How many lines of text are in the list?
	if((lNodes = pXmlTranscripts->GetLength()) > 0)
	{
		for(int i = 0; i < lNodes; i++)
		{
			//	Get the next transcript line node
			pXmlNode = new CIXMLDOMNode(pXmlTranscripts->nextNode());
			ASSERT(pXmlNode);
			ASSERT(pXmlNode->m_lpDispatch);

			if((pLine = GetLine(pXmlNode)) != NULL)
			{
				//	Does this page already exist?
				if((pPage = m_Pages.Find(pLine->m_lPageNum)) != 0)
				{
					//	Add this line
					pPage->m_Lines.Add(pLine);
				}
				else
				{
					//	Allocate and initialize the new page
					pPage = new CTextPage();
					pPage->m_lDesignationId = pLine->m_lDesignationId;
					pPage->m_lPageNum = pLine->m_lPageNum;
					pPage->m_lDesignationOrder = m_lPlaybackOrder;

					//	Add the page
					m_Pages.Add(pPage);

					//	Add the line
					pPage->m_Lines.Add(pLine);
				}

			}

			DELETE_INTERFACE(pXmlNode);

		}// for(int i = 0; i < lNodes; i++)

	}// if((lNodes = pXmlNodes->GetLength()) > 0)

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDesignation::GetTime()
//
// 	Description:	This function will get the position that corresponds to
//					the specified line in this page.
//
// 	Returns:		The requested frame if found. -1 otherwise
//
//	Notes:			None
//
//==============================================================================
double CDesignation::GetTime(long lPage, long lLine, BOOL bStartLine)
{
	return m_Pages.GetTime(lPage, lLine, bStartLine);
}

//==============================================================================
//
// 	Function Name:	CDesignation::GetLine()
//
// 	Description:	This function is called to create a new text line from the
//					specified XML node.
//
// 	Returns:		A pointer to the new line object
//
//	Notes:			None
//
//==============================================================================
CTextLine* CDesignation::GetLine(CIXMLDOMNode* pXmlTranscript)
{
	CIXMLDOMNamedNodeMap*	pAttributes = NULL;
	CIXMLDOMNode*			pXmlText = NULL;
	CIXMLDOMNode*			pXmlPage = NULL;
	CIXMLDOMNode*			pXmlLine = NULL;
	CIXMLDOMNode*			pXmlStart = NULL;
	CIXMLDOMNode*			pXmlStop = NULL;
	CIXMLDOMNode*			pXmlQA = NULL;
	CTextLine*				pLine = NULL;
	BOOL					bSuccessful = FALSE;
	CString					strQA = "";

	ASSERT(pXmlTranscript != NULL);

	//	Get the node's attributes
	pAttributes = new CIXMLDOMNamedNodeMap(pXmlTranscript->GetAttributes());
	if((pAttributes == 0) || (pAttributes->m_lpDispatch == 0))
		return 0;

	//	Retrieve each of the required attributes
	while(bSuccessful == FALSE)
	{
		pXmlPage = new CIXMLDOMNode(pAttributes->getNamedItem("page"));
		if((pXmlPage == 0) || (pXmlPage->m_lpDispatch == 0))
			break;

		pXmlLine = new CIXMLDOMNode(pAttributes->getNamedItem("line"));
		if((pXmlLine == 0) || (pXmlLine->m_lpDispatch == 0))
			break;

		pXmlText = new CIXMLDOMNode(pAttributes->getNamedItem("text"));
		if((pXmlText == 0) || (pXmlText->m_lpDispatch == 0))
			break;

		pXmlStart = new CIXMLDOMNode(pAttributes->getNamedItem("start"));
		if((pXmlStart == 0) || (pXmlStart->m_lpDispatch == 0))
			break;

		pXmlStop = new CIXMLDOMNode(pAttributes->getNamedItem("stop"));
		if((pXmlStop == 0) || (pXmlStop->m_lpDispatch == 0))
			break;

		pXmlQA = new CIXMLDOMNode(pAttributes->getNamedItem("qa"));
		if((pXmlStop == 0) || (pXmlStop->m_lpDispatch == 0))
			break;

		//	We're done
		bSuccessful = TRUE;

	}

	//	We're we able to get all the attributes?
	if(bSuccessful == TRUE)
	{
		pLine = new CTextLine();
		ASSERT(pLine != 0);

		pLine->m_strMediaId = m_strMediaId;
		pLine->m_lDesignationId = m_lTertiaryId;
		pLine->m_lDesignationOrder = m_lPlaybackOrder;
		if(m_lHighlighterId > 0)
			pLine->m_crColor = m_crHighlighter;

		pLine->m_lPageNum		= atol(pXmlPage->GetText());
		pLine->m_lLineNum		= atol(pXmlLine->GetText());
		pLine->m_dStartTime		= atof(pXmlStart->GetText());
		pLine->m_dStopTime		= atof(pXmlStop->GetText());
		pLine->m_bEnableScroll	= m_bScrollText;

		if((pXmlQA != 0) && (lstrlen(pXmlQA->GetText()) > 0))
		{
			//	Is it just the Q/A character?
			if(lstrlen(pXmlQA->GetText()) == 1)
				strQA = (pXmlQA->GetText() + ".  ");// Insert a period (.)
			else
				strQA = (pXmlQA->GetText() + "  ");
		}

		pLine->m_strText = (strQA + pXmlText->GetText());
	}

	//	Clean up
	DELETE_INTERFACE(pXmlText);
	DELETE_INTERFACE(pXmlPage);
	DELETE_INTERFACE(pXmlLine);
	DELETE_INTERFACE(pXmlStart);
	DELETE_INTERFACE(pXmlStop);
	DELETE_INTERFACE(pAttributes);

	return pLine;
}

//==============================================================================
//
// 	Function Name:	CDesignation::GetRemainingTime()
//
// 	Description:	This function will compute the time required to play this
//					designation from the specified position to the end
//
// 	Returns:		The remaining time in seconds
//
//	Notes:			None
//
//==============================================================================
double CDesignation::GetRemainingTime(double dPosition)
{
	if(m_dStopTime > dPosition)
		return (m_dStopTime - dPosition);
	else
		return (double)0;
}

//==============================================================================
//
// 	Function Name:	CDesignation::GetTotalTime()
//
// 	Description:	This function will compute the total time required to play
//					this designation
//
// 	Returns:		The time required in seconds
//
//	Notes:			None
//
//==============================================================================
double CDesignation::GetTotalTime()
{
	return (m_dStopTime - m_dStartTime);
}

//==============================================================================
//
// 	Function Name:	CDesignation::IsInRange()
//
// 	Description:	This function is called to determine if the page and line
//					provided by the caller is within the range defined by 
//					the designation
//
// 	Returns:		TRUE if it is within range
//
//	Notes:			None
//
//==============================================================================
BOOL CDesignation::IsInRange(long lPage, long lLine)
{
	if((lPage < m_lStartPage) || 
	  ((lPage == m_lStartPage) && (lLine < m_lStartLine)))
		return FALSE;

	if((lPage > m_lStopPage) || 
	  ((lPage == m_lStopPage) && (lLine > m_lStopLine)))
		return FALSE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDesignation::LastPage()
//
// 	Description:	This function will retrieve the last page object in the
//					list.
//
// 	Returns:		A pointer to the last object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CTextPage* CDesignation::LastPage()
{
	return m_Pages.Last();
}

//==============================================================================
//
// 	Function Name:	CDesignation::NextPage()
//
// 	Description:	This function will retrieve the next page object in the
//					list.
//
// 	Returns:		A pointer to the next object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CTextPage* CDesignation::NextPage()
{
	return m_Pages.Next();
}

//==============================================================================
//
// 	Function Name:	CDesignation::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			Designation objects use the playback order for comparison.
//
//==============================================================================
BOOL CDesignation::operator < (const CDesignation& Compare)
{
	return m_lPlaybackOrder < Compare.m_lPlaybackOrder;
}

//==============================================================================
//
// 	Function Name:	CDesignation::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CDesignation::operator == (const CDesignation& Compare)
{
	return m_lPlaybackOrder == Compare.m_lPlaybackOrder;
}

//==============================================================================
//
// 	Function Name:	CDesignation::PrevPage()
//
// 	Description:	This function will retrieve the previous page object in the
//					list.
//
// 	Returns:		A pointer to the previous object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CTextPage* CDesignation::PrevPage()
{
	return m_Pages.Prev();
}

//==============================================================================
//
// 	Function Name:	CDesignations::Add()
//
// 	Description:	This function will add the object pointer to the list in the
//					correct position based on the current sort order. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDesignations::Add(CDesignation* pDesignation, BOOL bSorted)
{
	POSITION		Pos;
	POSITION		Prev;
	CDesignation*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pDesignation);
		return;
	}

	//	Look for the correct position if sorting on entry
	if(bSorted)
	{
		Pos = GetHeadPosition();
		Prev = Pos;
		while(Pos != NULL)
		{
			if((pCurrent = (CDesignation*)GetNext(Pos)) == NULL)
				continue;

			//	Are we sorting in ascending order?
			if(m_bAscending)
			{
				if(*pDesignation < *pCurrent)
				{
					InsertBefore(Prev, pDesignation);
					return;
				}
			}
			else
			{
				if(*pCurrent < *pDesignation)
				{
					InsertBefore(Prev, pDesignation);
					return;
				}
			}
			
			Prev = Pos;	
		}
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pDesignation);
}

//==============================================================================
//
// 	Function Name:	CDesignations::CDesignations()
//
// 	Description:	This is the constructor for CDesignations objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDesignations::CDesignations(BOOL bAscending, int iAllocSize)
              :CObList(iAllocSize)
{
	//	Initialize the local members
	m_bAscending = bAscending;
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CDesignations::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CDesignations::Find(CDesignation* pDesignation)
{
	return (CObList::Find(pDesignation));
}

//==============================================================================
//
// 	Function Name:	CDesignations::FindFromBarcode()
//
// 	Description:	This function will retrieve the designation requested by
//					the caller using the specified barcode id
//
// 	Returns:		0 if not found
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDesignations::FindFromBarcode(long lBarcode)
{
	POSITION		Pos;
	CDesignation*	pDesignation;

	//	Get the first position
	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pDesignation = (CDesignation*)GetNext(Pos)) != 0)
		{
			if(pDesignation->m_lBarcodeId == lBarcode)
				return pDesignation;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CDesignations::FindFromId()
//
// 	Description:	This function will retrieve the designation requested by
//					the caller using the specified tertiary id
//
// 	Returns:		0 if not found
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDesignations::FindFromId(long lId)
{
	POSITION		Pos;
	CDesignation*	pDesignation;

	//	Get the first position
	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pDesignation = (CDesignation*)GetNext(Pos)) != 0)
		{
			if(pDesignation->m_lTertiaryId == lId)
				return pDesignation;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CDesignations::FindFromOrder()
//
// 	Description:	This function will retrieve the designation requested by
//					the caller using the specified playback order value
//
// 	Returns:		0 if not found
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDesignations::FindFromOrder(long lOrder)
{
	POSITION		Pos;
	CDesignation*	pDesignation;

	//	Get the first position
	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pDesignation = (CDesignation*)GetNext(Pos)) != 0)
		{
			if(pDesignation->m_lPlaybackOrder == lOrder)
				return pDesignation;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CDesignations::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CDesignation* CDesignations::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CDesignation*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CDesignations::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CDesignations::Flush(BOOL bDelete)
{
	CDesignation* pDesignation;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pDesignation = (CDesignation*)GetNext(m_NextPos)) != 0)
				delete pDesignation;
		}
	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CDesignations::GetFirstInRange()
//
// 	Description:	This function is called to get the first designation in 
//					where the page-line provided by the caller is within the
//					range defined by the designation
//
// 	Returns:		A pointer to the first designation 
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDesignations::GetFirstInRange(int iPage, int iLine, long lTranscript)
{
	POSITION		Pos;
	CDesignation*	pDesignation;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pDesignation = (CDesignation*)CObList::GetNext(Pos)) != NULL)
		{
			//	Did the caller request a specific transcript?
			if((lTranscript <= 0) || (pDesignation->m_lTranscriptId == lTranscript))
			{
				if(pDesignation->IsInRange(iPage, iLine))
					return pDesignation;
			}
		}
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CDesignations::GetFirstOrder()
//
// 	Description:	This function will retrieve the playback order of the first 
//					object in the list.
//
// 	Returns:		The playback order if found. -1 otherwise.
//
//	Notes:			None
//
//==============================================================================
long CDesignations::GetFirstOrder()
{
	POSITION Pos = GetHeadPosition();
	CDesignation* pFirst;

	//	Retrieve the last designation
	if(Pos != NULL)
		if((pFirst = (CDesignation*)GetHead()) != NULL)
			return pFirst->m_lPlaybackOrder;

	return -1L;
}

//==============================================================================
//
// 	Function Name:	CDesignations::GetLastOrder()
//
// 	Description:	This function will retrieve the playback order of the last 
//					object in the list.
//
// 	Returns:		The playback order if found. -1 otherwise.
//
//	Notes:			None
//
//==============================================================================
long CDesignations::GetLastOrder()
{
	POSITION Pos = GetTailPosition();
	CDesignation* pLast;

	//	Retrieve the last designation
	if(Pos != NULL)
		if((pLast = (CDesignation*)GetTail()) != NULL)
			return pLast->m_lPlaybackOrder;

	return -1L;
}

//==============================================================================
//
// 	Function Name:	CDesignations::GetNextOrder()
//
// 	Description:	This function will retrieve the playback order of the  
//					designation following the one provided by the caller
//
// 	Returns:		The playback order if found. -1 otherwise.
//
//	Notes:			None
//
//==============================================================================
long CDesignations::GetNextOrder(long lOrder)
{
	POSITION		Pos = GetHeadPosition();
	CDesignation*	pDesignation;

	while(Pos != NULL)
	{
		//	Get the current designation and advance the position
		if((pDesignation = (CDesignation*)GetNext(Pos)) == NULL)
			continue;

		//	Is this the designation specified by the caller?
		if(pDesignation->m_lPlaybackOrder == lOrder)
		{
			//	Is the next position valid?
			if(Pos == NULL)
				return -1;

			//	Get the next designation
			pDesignation = (CDesignation*)GetAt(Pos);
			if(pDesignation != 0)
				return pDesignation->m_lPlaybackOrder;
			else
				return -1L;
		}

	}

	return -1L;
}

//==============================================================================
//
// 	Function Name:	CDesignations::GetPrevOrder()
//
// 	Description:	This function will retrieve the playback order of the  
//					designation preceding the one provided by the caller
//
// 	Returns:		The playback order if found. -1 otherwise.
//
//	Notes:			None
//
//==============================================================================
long CDesignations::GetPrevOrder(long lOrder)
{
	POSITION		Pos = GetTailPosition();
	CDesignation*	pDesignation;

	while(Pos != NULL)
	{
		//	Get the current designation and position of the previous
		if((pDesignation = (CDesignation*)GetPrev(Pos)) == NULL)
			continue;

		//	Is this the designation specified by the caller?
		if(pDesignation->m_lPlaybackOrder == lOrder)
		{
			//	Is the previous position valid?
			if(Pos == NULL)
				return -1;

			//	Get the previous designation
			pDesignation = (CDesignation*)GetAt(Pos);
			if(pDesignation != 0)
				return pDesignation->m_lPlaybackOrder;
			else
				return -1L;
		}

	}

	return -1L;
}

//==============================================================================
//
// 	Function Name:	CDesignations::GetTime()
//
// 	Description:	This function will compute the time required to play the
//					designations from the specifed start position to the 
//					specified stop position
//
// 	Returns:		The playback time in seconds
//
//	Notes:			None
//
//==============================================================================
double CDesignations::GetTime(long lStartOrder, double dStartPosition,
							  long lStopOrder, double dStopPosition)
{
	POSITION		Pos;
	CDesignation*	pDesignation;
	double			dTime = 0.0;

	//	Are we starting at the beginning?
	if(lStartOrder <= 0)
	{
		//	Are we looking for the total time?
		if(lStopOrder <= 0)
		{
			return GetTotalTime();
		}
		else
		{
			Pos = GetHeadPosition();
			pDesignation = (CDesignation*)GetNext(Pos);
		}
	}
	else
	{
		//	Locate the start designation
		Pos = GetHeadPosition();
		while(Pos != NULL)
		{
			if((pDesignation = (CDesignation*)GetNext(Pos)) != 0)
			{
				//	Is this the start designation?
				if(lStartOrder == pDesignation->m_lPlaybackOrder)
					break;
			}
		}
	}

	//	Did we find the first designation?
	if(pDesignation == 0)
		return 0.0;

	//	Are we starting from the first frame?
	if(dStartPosition <= 0)
		dStartPosition = pDesignation->m_dStartTime;

	//	Are we only looking at one designation?
	if(pDesignation->m_lPlaybackOrder == lStopOrder)
	{
		//	Are we looking for the last frame?
		if(dStopPosition <= 0)
			dStopPosition = pDesignation->m_dStopTime;

		if(dStopPosition > dStartPosition)
			return (dStopPosition - dStartPosition);
		else
			return 0.0;
	}
	else
	{
		//	Initialize the time using the first designation
		dTime = pDesignation->GetRemainingTime(dStartPosition);
	}
	
	//	Now iterate through the rest of the designations
	while(Pos != NULL)
	{
		if((pDesignation = (CDesignation*)GetNext(Pos)) != 0)
		{
			//	Is this the stop designation?
			if(lStopOrder == pDesignation->m_lPlaybackOrder)
			{
				if(dStopPosition > 0)
					dTime += pDesignation->GetElapsedTime(dStopPosition);
				else
					dTime += pDesignation->GetTotalTime();
				break;
			}
			else
			{
				dTime += pDesignation->GetTotalTime();
			}
		}
	}
		
	return dTime;
}

//==============================================================================
//
// 	Function Name:	CDesignations::GetTotalTime()
//
// 	Description:	This function will compute the time required to play all
//					the designations in the list.
//
// 	Returns:		The total time in seconds
//
//	Notes:			None
//
//==============================================================================
double CDesignations::GetTotalTime()
{
	POSITION Pos;
	double   dTime = 0.0;

	//	Add the time for each designation
	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		CDesignation* pDesignation = (CDesignation*)GetNext(Pos);
		if(pDesignation != NULL)
			dTime += pDesignation->GetTotalTime();
	}
	return dTime;
}

//==============================================================================
//
// 	Function Name:	CDesignations::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CDesignation* CDesignations::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CDesignation*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CDesignations::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDesignations::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CDesignation*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CDesignations::Next()
//
// 	Description:	This is an alternate form of the function that will get the
//					next designation in the list without disrupting the local
//					iterators
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDesignations::Next(CDesignation* pDesignation)
{
	POSITION Pos = Find(pDesignation);

	if(!Pos)
		return 0;
	else
		GetNext(Pos);

	if(Pos)
		return (CDesignation*)GetNext(Pos);
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CDesignations::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDesignations::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CDesignation*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CDesignations::Prev()
//
// 	Description:	This is an alternate form of the function that will get the
//					previous designation in the list without disrupting the 
//					local iterators
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDesignations::Prev(CDesignation* pDesignation)
{
	POSITION Pos = Find(pDesignation);

	if(!Pos)
		return 0;
	else
		GetPrev(Pos);

	if(Pos)
		return (CDesignation*)GetPrev(Pos);
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CDesignations::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDesignations::Remove(CDesignation* pDesignation, BOOL bDelete)
{
	POSITION Pos = Find(pDesignation);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pDesignation;
	}
}




















