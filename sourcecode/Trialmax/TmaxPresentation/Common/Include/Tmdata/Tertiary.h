//==============================================================================
//
// File Name:	tertiary.h
//
// Description:	This file contains the declarations of the CTertiary and 
//				CTertiarys classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-18-00	1.00		Original Release
//==============================================================================
#if !defined(__TERTIARY_H__)
#define __TERTIARY_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <link.h>
#include <dbdefs.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CSecondary; 

//	This class manages information associated with image media records
class CTertiary	 : public CObject
{
	private:

	public:

		CLinks			m_Links;
		CLink*			m_pActiveLink;	//	Used by presentation during clip playback
		long			m_lTertiaryId;
		long			m_lSecondaryId;
		long			m_lPlaybackOrder;
		CString			m_strMediaId;
		CString			m_strFilename;
		CString			m_strDescription;
		CString			m_strRelativePath;

		//	These members added for .NET support
		CSecondary*		m_pSecondary;
		long			m_lBarcodeId;
		long			m_lAttributes;
		long			m_lPrimaryId;
		long			m_lStartPage;
		long			m_lStopPage;
		long			m_lStartLine;
		long			m_lStopLine;
		long			m_lHighlighterId;
		COLORREF		m_crHighlighter;
		double			m_dStartTime;
		double			m_dStopTime;
		short			m_sMediaType;
		CString			m_strName;
		CString			m_strSiblingId;

						CTertiary(CSecondary* pSecondary = 0);
		virtual		   ~CTertiary();

		BOOL			GetSplitScreen(){ return (GetFlag(NET_TERTIARY_SPLIT_SCREEN) && (m_strSiblingId.GetLength() > 0)); }
		BOOL			GetSplitHorizontal(){ return GetFlag(NET_TERTIARY_SPLIT_HORIZONTAL); }
		BOOL			GetSplitRight(){ return GetFlag(NET_TERTIARY_SPLIT_RIGHT); }

		void			SetSplitScreen(BOOL bSplitScreen){ SetFlag(NET_TERTIARY_SPLIT_SCREEN, bSplitScreen); }
		void			SetSplitHorizontal(BOOL bSplitHorizontal){ SetFlag(NET_TERTIARY_SPLIT_HORIZONTAL, bSplitHorizontal); }
		void			SetSplitRight(BOOL bSplitRight){ SetFlag(NET_TERTIARY_SPLIT_RIGHT, bSplitRight); }

		CLink*			FirstLink();
		CLink*			LastLink();
		CLink*			NextLink();
		CLink*			PrevLink();

		BOOL			operator < (const CTertiary& Compare);
		BOOL			operator > (const CTertiary& Compare);
		BOOL			operator == (const CTertiary& Compare);
	
	protected:

		BOOL			GetFlag(long lMask){ return ((m_lAttributes & lMask) != 0); }
		void			SetFlag(long lMask, BOOL bState);

};

//	Objects of this class are used to manage a list of CTertiary objects
class CTertiaries : public CObList
{
	private:

	public:

						CTertiaries();
		virtual		   ~CTertiaries();

		void			Add(CTertiary* pTertiary, BOOL bSorted);
		void			Flush(BOOL bDelete);
		void			Remove(CTertiary* pTertiary, BOOL bDelete);
		POSITION		Find(CTertiary* pTertiary);
		CTertiary*		FindByBarcodeId(long lId);
		CTertiary*		FindByDatabaseId(long lId);
		CTertiary*		FindByOrder(long lOrder);
		BOOL			IsFirst(CTertiary* pTertiary);
		BOOL			IsLast(CTertiary* pTertiary);

		//	List iteration members
		CTertiary*		First();
		CTertiary*		Last();
		CTertiary*		Next();
		CTertiary*		Prev();

	protected:

		POSITION		m_NextPos;
		POSITION		m_PrevPos;

};

#endif // !defined(__TERTIARY_H__)
