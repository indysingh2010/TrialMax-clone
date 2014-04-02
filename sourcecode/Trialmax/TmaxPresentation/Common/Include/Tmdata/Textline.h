//==============================================================================
//
// File Name:	textline.h
//
// Description:	This file contains the declarations of the CTextLine and 
//				CTextLines classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-10-00	1.00		Original Release
//==============================================================================
#if !defined(__TEXTLINE_H__)
#define __TEXTLINE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTextPage;
class CTextPages;

class CTextLine : public CObject
{
	private:


	public:

		long				m_lDesignationId;
		long				m_lDesignationOrder;
		long				m_lLineNum;
		long				m_lPageNum;
		double				m_dStartTime;
		double				m_dStopTime;
		BOOL				m_bEnableScroll;
		COLORREF			m_crColor;
		CString				m_strMediaId;
		CString				m_strText;

							CTextLine();
		virtual			   ~CTextLine();

		BOOL				operator < (const CTextLine& Compare);
		BOOL				operator > (const CTextLine& Compare);
		BOOL				operator == (const CTextLine& Compare);

	protected:

};

//	Objects of this class are used to manage a list of CTextLine objects
class CTextLines : public CObList
{
	private:

	public:

							CTextLines();
		virtual			   ~CTextLines();

		BOOL				Add(CTextLine* pLine);
		BOOL				Add(CTextPage* pPage);
		BOOL				Add(CTextPages* pPages);
		void				Flush(BOOL bDelete, long lDelete = 0);
		void				Remove(CTextLine* pLine, BOOL bDelete);
		POSITION			Find(CTextLine* pLine);
		CTextLine*			Find(long lLineNum);
		double				GetTime(long lLine, BOOL bStartLine = TRUE);

		//	List iteration members
		CTextLine*			First();
		CTextLine*			Last();
		CTextLine*			Next();
		CTextLine*			Prev();
		CTextLine*			SetPos(CTextLine* pLine);

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__TEXTLINE_H__)
