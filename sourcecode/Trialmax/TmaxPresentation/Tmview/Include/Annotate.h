//==============================================================================
//
// File Name:	annotate.h
//
// Description:	This file contains the declarations of the CCalloutAnn,
//				CCalloutAnns, CAnnotation and CAnnotations classes.
//
// Author:		Kenneth Moore
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	09-09-98	1.00		Original Release
//==============================================================================
#if !defined(__ANNOTATE_H__)
#define __ANNOTATE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <ltann.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Constants used to assemble the packed tag that gets attached to annotations
//	
//	The low word is used to store the annotation id
//	The high word stores the flags
//
#define TMANN_CALLOUT			0x00010000
#define TMANN_CALLOUT_SHADE		0x00020000
#define TMANN_LOCKED			0x00040000

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CCallout;
class CCallouts;
class CAnnotation;
class CAnnotations;

//	Objects of this class are used to maintain information about annotations
//	in the source image container
class CAnnotation : public CObject
{
	private:

		CCallouts*				m_pCallouts;

	public:

		WORD					m_wId;
		BOOL					m_bIsCallout;
		BOOL					m_bIsCalloutShade;
		BOOL					m_bIsLocked;
		ANNRECT					m_rcAnn;
		CString					m_strText;

								CAnnotation(WORD wId = 0);
								CAnnotation(DWORD dwTag);
		virtual				   ~CAnnotation();
		virtual void			Add(CCallout* pCallout);
		virtual void			Remove(CCallout* pCallout);
		DWORD					GetAnnTag();

		CCallout*				First();
		CCallout*				Next();

	protected:

};

//	Objects of this class are used to manage a list of annotations
class CAnnotations : public CObList
{
	private:

		WORD					m_wAnnId;

	public:

								CAnnotations();
		virtual				   ~CAnnotations();

		virtual BOOL			Add(CAnnotation* pAnn);
		virtual CAnnotation*	Add();
		virtual void			Flush(BOOL bDelete);
		virtual void			Remove(CAnnotation* pAnn, BOOL bDelete);
		virtual void			Remove(CCallout* pCallout);
		virtual POSITION		Find(CAnnotation* pAnn);
		virtual CAnnotation*	Find(DWORD dwTag);
		virtual CAnnotation*	Find(WORD wId);
		virtual CAnnotation*	FindCalloutShade();
		virtual BOOL			HasLocked();

		//	List iteration members
		virtual CAnnotation*	First();
		virtual CAnnotation*	Last();
		virtual CAnnotation*	Next();
		virtual CAnnotation*	Prev();

	protected:

		POSITION				m_NextPos;
		POSITION				m_PrevPos;

};

#endif // !defined(__ANNOTATE_H__)
