//==============================================================================
//
// File Name:	multipg.h
//
// Description:	This file contains the declarations of the CMultipage and
//				CMultipages classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-03-00	1.00		Original Release
//==============================================================================
#if !defined(__MULTIPG_H__)
#define __MULTIPG_H__

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

//	This class manages information associated with image media records
class CMultipage : public CMedia
{
	private:

	public:
		
		CSecondaries		m_Pages;

							CMultipage(CMedia* pMedia = 0);
		virtual			   ~CMultipage();

	protected:

};

//	Objects of this class are used to manage a list of CMultipage objects
class CMultipages : public CObList
{
	private:

	public:

							CMultipages();
		virtual			   ~CMultipages();

		BOOL				Add(CMultipage* pMultipage);
		void				Flush(BOOL bDelete);
		void				Remove(CMultipage* pMultipage, BOOL bDelete);
		POSITION			Find(CMultipage* pMultipage);
		CMultipage*			Find(LPCSTR lpId);
		CMultipage*			Find(long lId);

		//	List iteration members
		CMultipage*			First();
		CMultipage*			Last();
		CMultipage*			Next();
		CMultipage*			Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__MULTIPG_H__)
