//==============================================================================
//
// File Name:	link.h
//
// Description:	This file contains the declaration of the CLink and CLinks
//				classes.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-11-00	1.00		Original Release
//==============================================================================
#if !defined(__LINK_H__)
#define __LINK_H__

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

//	A link is an image or document that is to be displayed during some specific
//	interval associated with the playback of a designation. CLink objects 
//	encapsulate the information required to manage a linked image.
class CLink : public CObject
{
	private:

	public:

		long			m_lId;
		long			m_lOwnerId;
		long			m_lPage;
		long			m_lLine;
		long			m_lDisplayType;
		long			m_lFlags;
		BOOL			m_bSplitScreen;
		BOOL			m_bHide;	
		double			m_dTrigger;
		CString			m_strMediaId;
		CString			m_strItemBarcode;
		CString			m_strPST;

						CLink();
						CLink(const CLink& rLink);
		virtual		   ~CLink();

		long			GetFlags();

		BOOL			operator < (const CLink& Compare);
		BOOL			operator == (const CLink& Compare);
	
	protected:

};

//	This class manages a sorted list of CLink objects. 
class CLinks : public CObList
{
	private:

		POSITION		m_NextPos;
		POSITION		m_PrevPos;

	public:

		BOOL			m_bAscending;

						CLinks(BOOL bAscending = TRUE, int iAllocSize = 1);
		virtual		   ~CLinks();

		void			Flush(BOOL bDeleteAll);
		void			Add(CLink* pLink);
		void			Remove(CLink* pLink, BOOL bDelete);
		POSITION		Find(CLink* pLink);
		CLink*			Find(long lId);
		CLink*			FindAtPosition(double dPosition);

		//	List iteration members
		CLink*			First();
		CLink*			Last();
		CLink*			Next();
		CLink*			Prev();
		CLink*			SetPos(CLink* pLink);

	protected:

};

#endif // !defined(__LINK_H__)
