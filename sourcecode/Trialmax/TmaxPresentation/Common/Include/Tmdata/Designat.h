//==============================================================================
//
// File Name:	designat.h
//
// Description:	This file contains the declaration of CDesignation and 
//				CDesignations classes.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-11-00	1.00		Original Release
//==============================================================================
#if !defined(__DESIGNAT_H__)
#define __DESIGNAT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tertiary.h>
#include <textpage.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CIXMLDOMNodeList;
class CIXMLDOMNode;

//	A designation defines a portion of a video file to be played. CDesignation
//	objects encapsulate the information and functions required to manage a
//	designation. A designation contains a list of link images that should be
//	displayed at various times during play back of the designation.
class CDesignation : public CTertiary
{
	private:

	public:

		CTextPages		m_Pages;
		long			m_lTranscriptId;
		long			m_lVideoId;
		BOOL			m_bScrollText;
		CString			m_strOverlayRelativePath;
		CString			m_strOverlayFilename;
		CString			m_strOverlay;

						CDesignation();
		virtual		   ~CDesignation();

		CTextPage*		FirstPage();
		CTextPage*		LastPage();
		CTextPage*		NextPage();
		CTextPage*		PrevPage();

		double			GetTotalTime();
		double			GetElapsedTime(double dPosition);
		double			GetRemainingTime(double dPosition);
		BOOL			IsInRange(long lPage, long lLine);
		BOOL			HasText(){ return (m_Pages.GetCount() > 0); }
		BOOL			GetScrollTextEnabled();
		BOOL			GetText(CIXMLDOMNodeList* pXmlTranscripts);
		double			GetTime(long lPage, long lLine, BOOL bStartLine =  TRUE);
		double			GetTime(double dStart, double dStop);
		BOOL			operator < (const CDesignation& Compare);
		BOOL			operator == (const CDesignation& Compare);
	
	protected:

		CTextLine*		GetLine(CIXMLDOMNode* pXmlTranscript);
};

//	This class manages a list of CDesignation objects. The objects are sorted
//	based on the overloaded < and == operators of CDesignation.
class CDesignations : public CObList
{
	private:

		POSITION		m_NextPos;
		POSITION		m_PrevPos;

	public:

		BOOL			m_bAscending;

						CDesignations(BOOL bAscending = TRUE,
										 int iAllocSize = 1);

		void			Flush(BOOL bDeleteAll);
		void			Add(CDesignation* pDesignation, BOOL bSorted);
		void			Remove(CDesignation* pDesignation, BOOL bDelete);
		POSITION		Find(CDesignation* pDesignation);
		CDesignation*	FindFromId(long lId);
		CDesignation*	FindFromBarcode(long lBarcode);
		CDesignation*	FindFromOrder(long lOrder);

		long			GetLastOrder();
		long			GetFirstOrder();
		long			GetNextOrder(long lOrder);
		long			GetPrevOrder(long lOrder);
		CDesignation*	GetFirstInRange(int iPage, int iLine, long lTranscript);
		double			GetTotalTime();
		double			GetTime(long lStartOrder, double dStartPosition,
								long lStopOrder, double dStopPosition);

		//	List iteration routines
		CDesignation*	First();
		CDesignation*	Last();
		CDesignation*	Next();
		CDesignation*	Prev();
		CDesignation*	Next(CDesignation* pDesignation);
		CDesignation*	Prev(CDesignation* pDesignation);

	protected:

};

#endif // !defined(__DESIGNAT_H__)
