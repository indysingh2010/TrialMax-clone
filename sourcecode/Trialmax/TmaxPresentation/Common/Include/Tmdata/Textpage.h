//==============================================================================
//
// File Name:	textpage.h
//
// Description:	This file contains the declarations of the CTextPage and 
//				CTextPages classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-10-00	1.00		Original Release
//==============================================================================
#if !defined(__TEXTPAGE_H__)
#define __TEXTPAGE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <textline.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CTextPage : public CObject
{
	private:

	public:

		CTextLines			m_Lines;
		long				m_lPageNum;
		long				m_lDesignationId;
		long				m_lDesignationOrder;

							CTextPage();
		virtual			   ~CTextPage();

		BOOL				operator < (const CTextPage& Compare);
		BOOL				operator > (const CTextPage& Compare);
		BOOL				operator == (const CTextPage& Compare);

		double				GetTime(long lLine, BOOL bStartLine = TRUE);

		CTextLine*			FirstLine();
		CTextLine*			LastLine();
		CTextLine*			NextLine();
		CTextLine*			PrevLine();

	protected:

};

//	Objects of this class are used to manage a list of CTextPage objects
class CTextPages : public CObList
{
	private:

	public:

							CTextPages();
		virtual			   ~CTextPages();

		BOOL				Add(CTextPage* pPage);
		void				Flush(BOOL bDelete);
		void				Remove(CTextPage* pPage, BOOL bDelete);
		POSITION			Find(CTextPage* pPage);
		CTextPage*			Find(long lPageNum);
		double				GetTime(long lPage, long lLine, 
									BOOL bStartLine = TRUE);

		//	List iteration members
		CTextPage*			First();
		CTextPage*			Last();
		CTextPage*			Next();
		CTextPage*			Prev();
		CTextPage*			SetPos(long lPageNum);

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__TEXTPAGE_H__)
