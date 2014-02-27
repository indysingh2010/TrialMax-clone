//==============================================================================
//
// File Name:	page.h
//
// Description:	This file contains the declarations of the CSecondary and 
//				CSecondaries classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-18-00	1.00		Original Release
//==============================================================================
#if !defined(__SECONDARY_H__)
#define __SECONDARY_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tertiary.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class manages information associated with image media records
class CSecondary : public CObject
{
	private:

	public:

		CTertiaries			m_Children;
		long				m_lSecondaryId;
		long				m_lPlaybackOrder;
		long				m_lSlideId;
		long				m_lDisplayType;
		CString				m_strMediaId;
		CString				m_strFilename;
	
		//	These members were added for .NET
		short				m_sMediaType;
		long				m_lPrimaryId;
		long				m_lBarcodeId;
		long				m_lChildren;
		long				m_lAttributes;
		long				m_lStartPage;
		long				m_lStopPage;
		long				m_lStartLine;
		long				m_lStopLine;
		long				m_lAliasId;
		long				m_lXmlSegmentId;
		double				m_dStartTime;
		double				m_dStopTime;
		CString				m_strDescription;
		CString				m_strName;
		CString				m_strRelativePath;
		BOOL				m_bLinked;

							CSecondary();
		virtual			   ~CSecondary();

		BOOL				operator < (const CSecondary& Compare);
		BOOL				operator > (const CSecondary& Compare);
		BOOL				operator == (const CSecondary& Compare);

		UINT				MsgBox(HWND hWnd, LPCSTR lpszTitle = "");
	
	protected:

};

//	Objects of this class are used to manage a list of CSecondary objects
class CSecondaries : public CObList
{
	private:

	public:

							CSecondaries();
		virtual			   ~CSecondaries();

		void				Add(CSecondary* pSecondary, BOOL bSorted = TRUE);
		void				Flush(BOOL bDelete);
		void				Remove(CSecondary* pSecondary, BOOL bDelete);
		POSITION			Find(CSecondary* pSecondary);
		CSecondary*			FindByBarcodeId(long lId);
		CSecondary*			FindByDatabaseId(long lId);
		CSecondary*			FindByOrder(long lOrder);
		CSecondary*			FindBySlide(long lSlide);
		CSecondary*			FindNext(CSecondary* pSecondary);
		CSecondary*			FindPrev(CSecondary* pSecondary);

		BOOL				IsLast(CSecondary* pSecondary);
		BOOL				IsFirst(CSecondary* pSecondary);

		//	List iteration members
		CSecondary*			First();
		CSecondary*			Last();
		CSecondary*			Next();
		CSecondary*			Prev();

		UINT				MsgBox(HWND hWnd, LPCSTR lpszTitle = "");

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__SECONDARY_H__)
