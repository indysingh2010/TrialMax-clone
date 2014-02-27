//==============================================================================
//
// File Name:	media.cpp
//
// Description:	This file contains member functions of the CMedia and
//				CMedias classes
//
// See Also:	media.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-03-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <media.h>

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
// 	Function Name:	CMedia::CMedia()
//
// 	Description:	This is the constructor for CMedia objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMedia::CMedia(CMedia* pMedia)
{
	if(pMedia)
	{
		m_lPrimaryId = pMedia->m_lPrimaryId;
		m_lPlayerType = pMedia->m_lPlayerType;
		m_lFlags = pMedia->m_lFlags;
		m_strFilename = pMedia->m_strFilename;
		m_strMediaId = pMedia->m_strMediaId;
		m_strName = pMedia->m_strName;
		m_strRelativePath = pMedia->m_strRelativePath;

		m_lChildren = pMedia->m_lChildren;
		m_lAttributes = pMedia->m_lAttributes;
		m_lAliasId = pMedia->m_lAliasId;
		m_sMediaType = pMedia->m_sMediaType;
		m_strAltBarcode = pMedia->m_strAltBarcode;
		m_strDescription = pMedia->m_strDescription;
		m_strExhibit = pMedia->m_strExhibit;
		
		m_bLinked = (m_lAliasId > 0);
	}
	else
	{
		m_lPrimaryId = 0;
		m_lPlayerType = 0;
		m_lFlags = 0;
		m_strFilename.Empty();
		m_strMediaId.Empty();
		m_strName.Empty();
		m_strRelativePath.Empty();

		m_lChildren = -1;
		m_lAttributes = 0;
		m_lAliasId = 0;
		m_sMediaType = -1;
		m_strAltBarcode.Empty();
		m_strDescription.Empty();
		m_strExhibit.Empty();
		m_bLinked = FALSE;
	}		
}

//==============================================================================
//
// 	Function Name:	CMedia::~CMedia()
//
// 	Description:	This is the destructor for CMedia objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMedia::~CMedia()
{

}

//==============================================================================
//
// 	Function Name:	CMedia::Compare()
//
// 	Description:	This is called to compare the two objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CMedia::Compare(CMedia* pCompare)
{
	return Compare(m_strMediaId, pCompare->m_strMediaId, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMedia::Extract()
//
// 	Description:	This function will extract the first pure block of numbers
//					or the first block of alpha characters from the specified 
//					string
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMedia::Extract(char** ppString, char* pExtracted, int iMaxExtracted)
{
	BOOL	bNumerical = FALSE;
	BOOL	bFirstTime = TRUE;
	char*	pString = *ppString;
	int		iChars = 0;

	ASSERT(pString != NULL);
	ASSERT(pExtracted != NULL);
	ASSERT(iMaxExtracted > 1);
	if(pString == NULL) return;

	//	Clear the caller's extraction buffer
	memset(pExtracted, 0, iMaxExtracted);

	//	Are we extracting a numerical value?
	bNumerical = (isdigit(*pString) != 0);

	//	Check each character until we run out of characters or room in the buffer
	while((*pString != '\0') && (iChars < iMaxExtracted))
	{
		//	Is this the first time through the loop?
		if(bFirstTime)
		{
			bFirstTime = FALSE;
		}
		else
		{
			//	Are we extracting a numerical value?
			if(bNumerical == TRUE)
			{
				//	Stop here if this character is Alpha
				if(isdigit(*pString) == 0)
					break;
			}
			else
			{
				//	Stop here if this character is numeric
				if(isdigit(*pString) != 0)
					break;
			}

		}
		
		//	Transfer this character to the extraction buffer
		pExtracted[iChars] = *pString;
		iChars++;
		
		//	Move to the next character
		pString++;
	}

	//	Reset the caller's pointer
	*ppString = pString;

}

//==============================================================================
//
// 	Function Name:	CMedia::Compare()
//
// 	Description:	This function will compare two strings
//
// 	Returns:		-1 if pObj1 < pObj2, 0 if equal, 1 if pObj1 > pObj2
//
//	Notes:			None
//
//==============================================================================
int CMedia::Compare(CString& rString1, CString& rString2, BOOL bIgnoreCase)
{
	int		iReturn = 0;
	char*	pStr1 = rString1.GetBuffer(0);
	char*	pStr2 = rString2.GetBuffer(0);
	char	szSubString1[1024];
	char	szSubString2[1024];
	long	lNumber1;
	long	lNumber2;

	//	Continue while strings evaluate as equal
	while(iReturn == 0)
	{
		//	Get the next pair of substrings
		Extract(&pStr1, szSubString1, sizeof(szSubString1));
		Extract(&pStr2, szSubString2, sizeof(szSubString2));

		//	Compare the substrings
		if(szSubString1[0] == '\0')
		{
			//	Are both substrings empty?
			if(szSubString2[0] == '\0')
			{
				return 0;	//	They must be equal
			}
			else
			{
				return -1;	//	String2 has more characters
			}
			
		}
		else
		{
			if(szSubString2[0] == '\0')
			{
				return 1;	//	String1 has more characters
			}
			else
			{
				//	Is substring1 numerical?
				if(isdigit(szSubString1[0]) != 0)
				{
					//	Is substring2 numerical?
					if(isdigit(szSubString2[0]) != 0)
					{
						//	Convert to numbers
						lNumber1 = atol(szSubString1);
						lNumber2 = atol(szSubString2);

						//	Are the substrings numerically equal?
						if(lNumber1 == lNumber2)
						{
							//	Use the string lengths if numerically equal
							if(lstrlen(szSubString1) < lstrlen(szSubString2))
								iReturn = 1;
							else if(lstrlen(szSubString1) > lstrlen(szSubString2))
								iReturn = -1;
						}
						else
						{
							if(lNumber1 < lNumber2)
								iReturn = -1;
							else if(lNumber1 > lNumber2)
								iReturn = 1;
						}
					
					}
					else
					{
						//	strSubString1 has numbers, strSubString2 has characters
						if(bIgnoreCase == TRUE)
							iReturn = (toupper(szSubString1[0]) < toupper(szSubString2[0])) ? -1 : 1;
						else
							iReturn = (szSubString1[0] < szSubString2[0]) ? -1 : 1;
					}						
				
				}
				else
				{
					//	Is substring2 numerical?
					if(isdigit(szSubString2[0]) != 0)
					{
						//	strSubString2 has numbers, strSubString1 has characters
						if(bIgnoreCase == TRUE)
							iReturn = (toupper(szSubString1[0]) < toupper(szSubString2[0])) ? -1 : 1;
						else
							iReturn = (szSubString1[0] < szSubString2[0]) ? -1 : 1;
					}
					else
					{
						//	Both substrings are non-numeric
						if(bIgnoreCase == TRUE)
							iReturn = lstrcmpi(szSubString1, szSubString2);
						else
							iReturn = lstrcmp(szSubString1, szSubString2);
					}						
				
				}// if(IsDigit(szSubString1[0])) 
			
			}// if(szSubString2[0] == '\0')
			
		}// if(szSubString1[0] == '\0')
		
	}// while(iReturn == 0) 

	rString1.ReleaseBuffer();
	rString2.ReleaseBuffer();

	return iReturn;
}
		
//==============================================================================
//
// 	Function Name:	CMedia::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CMedia::operator < (const CMedia& rCompare)
{
	return (Compare(m_strMediaId, (CString&)(rCompare.m_strMediaId), TRUE) < 0);
}

//==============================================================================
//
// 	Function Name:	CMedia::operator > ()
//
// 	Description:	This is an overloaded version of the > operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CMedia::operator > (const CMedia& rCompare)
{
	return (Compare(m_strMediaId, (CString&)(rCompare.m_strMediaId), TRUE) > 0);
}

//==============================================================================
//
// 	Function Name:	CMedia::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CMedia::operator == (const CMedia& rCompare)
{
	return (Compare(m_strMediaId, (CString&)(rCompare.m_strMediaId), TRUE) == 0);
}

//==============================================================================
//
// 	Function Name:	CMedias::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMedias::Add(CMedia* pMedia)
{
	POSITION	Pos;
	POSITION	Prev;
	CMedia*		pCurrent;

	ASSERT(pMedia);
	if(!pMedia)
		return FALSE;

	//	MFC will throw a memory exception if it can't add the object to the list
	try
	{
		if(m_bKeepSorted == FALSE)
		{
			//	Add the link to the end of the list
			AddTail(pMedia);
		}
		else
		{
			//	Add at the head of the list if the list is empty
			if(IsEmpty())
			{
				AddHead(pMedia);
			}
			else
			{
				//	Look for the correct position to insert the object
				if((*((CMedia*)GetTail()) > *pMedia))
				{
					Pos  = GetHeadPosition();
					Prev = Pos;
					while(Pos != NULL)
					{
						if((pCurrent = (CMedia*)GetNext(Pos)) == NULL)
							continue;

						if(*pMedia < *pCurrent)
						{
							InsertBefore(Prev, pMedia);
							return TRUE;
						}
						
						Prev = Pos;	
					}
				}

				//	If we made it this far we must have to add it to the end of the list
				AddTail(pMedia);
			}

		}

		return TRUE;

	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMedia::CMedias()
//
// 	Description:	This is the constructor for CMedias objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMedias::CMedias(BOOL bKeepSorted)
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
	m_bKeepSorted = bKeepSorted;
	m_pFirst = 0;
	m_pLast = 0;
}

//==============================================================================
//
// 	Function Name:	CMedia::~CMedias()
//
// 	Description:	This is the destructor for CMedias objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMedias::~CMedias()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CMedias::Find()
//
// 	Description:	This function will search the list for the item with the
//					text identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			This function will update the local iterators
//
//==============================================================================
CMedia* CMedias::Find(LPCSTR lpId)
{
	CMedia*	pMedia;

	//	Get the first object
	pMedia = First();
	while(pMedia)
	{
		if(lstrcmpi(pMedia->m_strMediaId, lpId) == 0)
			return pMedia;
		else
			pMedia = Next();
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CMedias::Compare()
//
// 	Description:	This function will compare two media objects
//
// 	Returns:		-1 if pObj1 < pObj2, 0 if equal, 1 if pObj1 > pObj2
//
//	Notes:			None
//
//==============================================================================
int CMedias::Compare(const void* pObj1, const void* pObj2)
{
	CMedia**	ppMedia1 = (CMedia**)pObj1;
	CMedia**	ppMedia2 = (CMedia**)pObj2;	

	if((ppMedia1 != 0) && (ppMedia2 != 0))
	{
		return (*ppMedia1)->Compare(*ppMedia2);
	}
	else
	{
		return -1;
	}

}

//==============================================================================
//
// 	Function Name:	CMedias::Find()
//
// 	Description:	This function will search the list for the item with the
//					numeric identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CMedia* CMedias::Find(long lId)
{
	CMedia*	pMedia;

	//	Get the first object
	pMedia = First();
	while(pMedia)
	{
		if(pMedia->m_lPrimaryId == lId)
			return pMedia;
		else
			pMedia = Next();
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CMedias::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CMedias::Find(CMedia* pMedia)
{
	return (CObList::Find(pMedia));
}

//==============================================================================
//
// 	Function Name:	CMedias::FindFirst()
//
// 	Description:	This function will locate the last object in the list as
//					if it were sorted
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMedia* CMedias::FindFirst()
{
	CMedia*	pMedia = 0;

	//	Have we already located the last object?
	if(m_pFirst != 0) return m_pFirst;

	//	Initialize the pointer
	m_pFirst = First();

	if(m_pFirst != 0)
	{
		while((pMedia = Next()) != 0)
		{
			if(m_pFirst->Compare(pMedia) > 0)
				m_pFirst = pMedia;
		}
	}
	
	return m_pFirst;
}

//==============================================================================
//
// 	Function Name:	CMedias::FindLast()
//
// 	Description:	This function will locate the last object in the list as
//					if it were sorted
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMedia* CMedias::FindLast()
{
	CMedia*	pMedia = 0;

	//	Have we already located the last object?
	if(m_pLast != 0) return m_pLast;

	//	Initialize the pointer
	m_pLast = First();

	if(m_pLast != 0)
	{
		while((pMedia = Next()) != 0)
		{
			if(m_pLast->Compare(pMedia) < 0)
				m_pLast = pMedia;
		}
	}
	
	return m_pLast;
}

//==============================================================================
//
// 	Function Name:	CMedias::FindNext()
//
// 	Description:	This function will retrieve the object that follows the
//					caller specified object.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CMedia* CMedias::FindNext(CMedia* pMedia)
{
	POSITION	Pos;
	CMedia*		pNext = 0;
	CMedia*		pCurrent = 0;

	//	Does the caller want the first media object?
	if(pMedia == 0)
	{
		return FindFirst();
	}
	else if(pMedia == m_pLast)
	{
		return 0;
	}
	else
	{
		Pos = GetHeadPosition();
		while(Pos != NULL)
		{
			if((pCurrent = (CMedia*)GetNext(Pos)) != 0)
			{
				//	Is this object greater than the caller's object
				if(pCurrent->Compare(pMedia) > 0)
				{
					//	Have we already found a greater object
					if(pNext != 0)
					{
						//	Is this less than the one we already have lined up?
						if(pCurrent->Compare(pNext) < 0)
							pNext = pCurrent;
					}
					else
					{
						pNext = pCurrent;
					}
				}
			
			}

		}

		//	This must be the last object if no next one could be found
		if((pNext == 0) && (m_pLast == 0))
			m_pLast = pMedia;

		return pNext;

	}
}

//==============================================================================
//
// 	Function Name:	CMedias::FindPrev()
//
// 	Description:	This function will retrieve the object that preceeds the
//					caller specified object.
//
// 	Returns:		A pointer to the previous object in the list
//
//	Notes:			None
//
//==============================================================================
CMedia* CMedias::FindPrev(CMedia* pMedia)
{
	POSITION	Pos;
	CMedia*		pCurrent = 0;
	CMedia*		pPrev = 0;

	//	Does the caller want the first media object?
	if(pMedia == 0)
	{
		return FindFirst();
	}
	else if(pMedia == m_pFirst)
	{
		return 0;
	}
	else
	{
		Pos = GetHeadPosition();
		while(Pos != NULL)
		{
			if((pCurrent = (CMedia*)GetNext(Pos)) != 0)
			{
				//	Is this object less than the caller's object
				if(pCurrent->Compare(pMedia) < 0)
				{
					//	Have we already found a lesser object
					if(pPrev != 0)
					{
						//	Is this greater than the one we already have lined up?
						if(pCurrent->Compare(pPrev) > 0)
							pPrev = pCurrent;
					}
					else
					{
						pPrev = pCurrent;
					}
				
				}// if(pCurrent->Compare(pMedia) < 0)
			
			}// if((pCurrent = (CMedia*)GetNext(Pos)) != 0)

		}// while(Pos != NULL)

		//	This must be the first object if no previous one could be found
		if((pPrev == 0) && (m_pFirst == 0))
			m_pFirst = pMedia;

		return pPrev;
	}
}

//==============================================================================
//
// 	Function Name:	CMedias::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CMedia* CMedias::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CMedia*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CMedias::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CMedias::Flush(BOOL bDelete)
{
	CMedia* pMedia;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pMedia = (CMedia*)GetNext(m_NextPos)) != 0)
				delete pMedia;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
	m_pFirst = 0;
	m_pLast = 0;
}

//==============================================================================
//
// 	Function Name:	CMedias::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CMedias::IsFirst(CMedia* pMedia)
{
	return (pMedia == m_pFirst);
}

//==============================================================================
//
// 	Function Name:	CMedias::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CMedias::IsLast(CMedia* pMedia)
{
	return (pMedia == m_pLast);
}

//==============================================================================
//
// 	Function Name:	CMedias::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CMedia* CMedias::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CMedia*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CMedias::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CMedia* CMedias::Next()
{
	if(m_NextPos == NULL)
	{
		return NULL;
	}
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CMedia*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CMedias::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CMedia* CMedias::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CMedia*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CMedias::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMedias::Remove(CMedia* pMedia, BOOL bDelete)
{
	POSITION Pos = Find(pMedia);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pMedia;

		//	Do we need to reset the extents?
		if((m_pFirst == pMedia) || (m_pLast == pMedia))
			SetExtents();
	}
}

//==============================================================================
//
// 	Function Name:	CMedias::SetExtents()
//
// 	Description:	This function will set the first and last objects in the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMedias::SetExtents()
{
	CMedia*	pMedia = 0;

	//	Initialize the pointers
	m_pFirst = First();
	m_pLast  = m_pFirst;

//double dStart = (double)GetTickCount();

	if(m_pFirst != 0)
	{
		while((pMedia = Next()) != 0)
		{
			if(m_pFirst->Compare(pMedia) > 0)
				m_pFirst = pMedia;
			if(m_pLast->Compare(pMedia) < 0)
				m_pLast = pMedia;
		}
	}

//double dEnd = (double)GetTickCount();
//CString M;
//M.Format("Time to set extents for %ld records: %.3f", GetCount(), ((dEnd - dStart) / 1000.0));
//MessageBox(0, M, "", MB_OK);
}

//==============================================================================
//
// 	Function Name:	CMedias::SetKeepSorted()
//
// 	Description:	This function is called to set the flag that instructs the
//					list to be kept in sorted order
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMedias::SetKeepSorted(BOOL bKeepSorted)
{
	if(m_bKeepSorted != bKeepSorted)
	{
		m_bKeepSorted = bKeepSorted;

		if(m_bKeepSorted && GetCount() > 1)
			Sort();
	}

}

//==============================================================================
//
// 	Function Name:	CMedias::Sort()
//
// 	Description:	This function will sort the objects in the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMedias::Sort()
{
	CMedia**	paMedia = 0;
	CMedia*		pMedia = 0;
	long		lIndex = 0;
	long		lTotal = 0;

	//	Must be something to sort
	if((lTotal = GetCount()) < 2) return;

	//	Allocate an array to store the pointers to the objects in the list
	paMedia = new CMedia*[lTotal];

	//	Initialize the array to represent the current order of objects in the list
	pMedia = First();
	while(pMedia != 0)
	{
		paMedia[lIndex] = pMedia;
		lIndex++;
		pMedia = Next();
	}

	//	Sort the objects
	qsort(paMedia, lTotal, sizeof(CMedia*), Compare);

//FILE* fptr = fopen("f:\\media_sort.txt", "wt");
	
	//	Reorder this list to match the sorted order
	Flush(FALSE);
	for(lIndex = 0; lIndex < lTotal; lIndex++)
	{
		Add(paMedia[lIndex]);
		//fprintf(fptr, "%s\n", paMedia[lIndex]->m_strMediaId);
	}
//fclose(fptr);
	
	delete [] paMedia;
}

/*
//==============================================================================
//
// 	Function Name:	CMedia::Extract()
//
// 	Description:	This function will extract the first pure block of numbers
//					or the first block of alpha characters from the specified 
//					string
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMedia::Extract(CString& rString, CString& rExtracted)
{
	char	szBuffer[2048];
	char*	pRemaining = szBuffer;
	BOOL	bNumerical = FALSE;
	BOOL	bFirstTime = TRUE;

	rExtracted.Empty();
	
	lstrcpyn(szBuffer, rString, sizeof(szBuffer));

	//	Are we doing a numerical value?
	bNumerical = IsDigit(szBuffer[0]);

	while(*pRemaining != '\0')
	{
		if(!bFirstTime)
		{
			//	Should we stop here?
			if(bNumerical == TRUE)
			{
				if(!IsDigit(*pRemaining))
					break;
			}
			else
			{
				if(IsDigit(*pRemaining))
					break;
			}

		}
		else
		{
			bFirstTime = FALSE;
		}
		
		//	Move to the next character in the buffer
		pRemaining++;
	}

	rString = pRemaining;
	*pRemaining = 0;
	rExtracted = szBuffer;
}

//==============================================================================
//
// 	Function Name:	CMedia::Compare()
//
// 	Description:	This function will compare two strings
//
// 	Returns:		-1 if pObj1 < pObj2, 0 if equal, 1 if pObj1 > pObj2
//
//	Notes:			None
//
//==============================================================================
int CMedia::Compare(CString& rString1, CString& rString2, BOOL bIgnoreCase)
{
	int		iReturn = 0;
	CString	str1;
	CString	str2;
	CString	strSubString1;
	CString	strSubString2;
	long	lNumber1;
	long	lNumber2;

	str1 = rString1;
	str2 = rString2;

	//	Continue while strings evaluate as equal
	while(iReturn == 0)
	{
		//	Get the next pair of substrings
		Extract(str1, strSubString1);
		Extract(str2, strSubString2);

		//	Compare the substrings
		if(strSubString1[0] == '\0')
		{
			//	Are both strings empty?
			if(strSubString2[0] == '\0')
			{
				return 0;	//	They must be equal
			}
			else
			{
				return -1;	//	String2 has more characters
			}
			
		}
		else
		{
			if(strSubString2[0] == '\0')
			{
				return 1;	//	String1 has more characters
			}
			else
			{

				//	Is substring1 numerical?
				if(IsDigit(strSubString1[0]))
				{
					//	Is substring2 numerical?
					if(IsDigit(strSubString2[0]))
					{
						//	Convert to numbers
						lNumber1 = atol(strSubString1);
						lNumber2 = atol(strSubString2);

						if(lNumber1 < lNumber2)
							iReturn = -1;
						else if(lNumber1 > lNumber2)
							iReturn = 1;
					}
					else
					{
						//	strSubString1 has numbers, strSubString2 has characters
						//iReturn = (toupper(strSubString1[0]) < toupper(strSubString2[0])) ? -1 : 1;

						if(bIgnoreCase == TRUE)
							iReturn = strSubString1.CompareNoCase(strSubString2);
						else
							iReturn = strSubString1.Compare(strSubString2);
					
						//	Must make one or the other so that we break out
						if(iReturn == 0)
							iReturn = 1;

					}						
				
				}
				else
				{
					//	Is substring2 numerical?
					if(IsDigit(strSubString2[0]))
					{
						//	strSubString2 has numbers, strSubString1 has characters
						//iReturn = (toupper(strSubString1[0]) < toupper(strSubString2[0])) ? -1 : 1;

						if(bIgnoreCase == TRUE)
							iReturn = strSubString1.CompareNoCase(strSubString2);
						else
							iReturn = strSubString1.Compare(strSubString2);
					
						//	Must make one or the other so that we break out
						if(iReturn == 0)
							iReturn = 1;
					}
					else
					{
						//	Both substrings are non-numeric
						if(bIgnoreCase == TRUE)
							iReturn = strSubString1.CompareNoCase(strSubString2);
						else
							iReturn = strSubString1.Compare(strSubString2);
					}						
				
				}// if(IsDigit(strSubString1[0]) == TRUE) 
			
			}// if(strSubString2.GetLength() == 0)
			
		}// if(strSubString1.GetLength() == 0)
		
	}// while(iReturn == 0) 

	return iReturn;
}
		
*/