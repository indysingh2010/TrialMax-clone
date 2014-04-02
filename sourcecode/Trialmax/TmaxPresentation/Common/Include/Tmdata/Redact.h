//==============================================================================
//
// File Name:	redact.h
//
// Description:	This file contains the declaration of the CRedaction and 
//				CRedactions	classes.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	06-04-03	1.00		Original Release
//==============================================================================
#if !defined(__REDACT_H__)
#define __REDACT_H__

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

//	A redaction is a rectangular annotation that gets applied to a page when it
//	gets loaded.
class CRedaction : public CObject
{
	private:

	public:

		long			m_lId;
		long			m_lPage;
		long			m_lAnnRedaction;
		long			m_lAnnLabel;
		short			m_sFontSize;
		RECT			m_rcBounds;
		CString			m_strLabel;
		CString			m_strFontName;
		COLORREF		m_crRedaction;
		COLORREF		m_crLabel;
		BOOL			m_bOpaque;

						CRedaction();
		virtual		   ~CRedaction();

		BOOL			operator < (const CRedaction& Compare);
		BOOL			operator == (const CRedaction& Compare);
	
	protected:

};

//	This class manages a sorted list of CRedaction objects. 
class CRedactions : public CObList
{
	private:

		POSITION		m_NextPos;
		POSITION		m_PrevPos;

	public:

		BOOL			m_bAscending;

						CRedactions(BOOL bAscending = TRUE, int iAllocSize = 1);
		virtual		   ~CRedactions();

		void			Flush(BOOL bDeleteAll);
		void			Add(CRedaction* pRedaction);
		void			Remove(CRedaction* pRedaction, BOOL bDelete);
		POSITION		Find(CRedaction* pRedaction);
		CRedaction*		Find(long lId);

		//	List iteration members
		CRedaction*		First();
		CRedaction*		Last();
		CRedaction*		Next();
		CRedaction*		Prev();

	protected:

};

#endif // !defined(__REDACT_H__)
