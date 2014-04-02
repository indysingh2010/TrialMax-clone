//==============================================================================
//
// File Name:	deposit.h
//
// Description:	This file contains the declarations of the CDeposition and
//				CDepositions classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2008
//
//==============================================================================
//	Date		Revision    Description
//	07-10-08	1.00		Original Release
//==============================================================================
#if !defined(__DEPOSIT_H__)
#define __DEPOSIT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <media.h>
#include <secondary.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CTranscript;

//	This class manages information associated with a video deposition
class CDeposition : public CMedia
{
	private:

	public:
		
		CTranscript*		m_pTranscript;
		CSecondaries		m_Segments;

							CDeposition(CMedia* pMedia = NULL);
		virtual			   ~CDeposition();

	protected:

};

//	Objects of this class are used to manage a list of CDeposition objects
class CDepositions : public CObList
{
	private:

	public:

							CDepositions();
		virtual			   ~CDepositions();

		BOOL				Add(CDeposition* pDeposition);
		void				Flush(BOOL bDelete);
		void				Remove(CDeposition* pDeposition, BOOL bDelete);
		POSITION			Find(CDeposition* pDeposition);
		CDeposition*		Find(LPCSTR lpszMediaId);
		CDeposition*		Find(long lDatabaseId);

		//	List iteration members
		CDeposition*		First();
		CDeposition*		Last();
		CDeposition*		Next();
		CDeposition*		Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__DEPOSIT_H__)
