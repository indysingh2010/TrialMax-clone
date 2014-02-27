//==============================================================================
//
// File Name:	machine.h
//
// Description:	This file contains the declaration of the CMachine class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2004
//
//==============================================================================
//	Date		Revision    Description
//	01-31-04	1.00		Original Release
//==============================================================================
#if !defined(__MACHINE_H__)
#define __MACHINE_H__

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

//	This class manages information associated with drive / server alias
class CMachine : public CObject
{
	private:

	public:

		int					m_iPathMap;
		CString				m_strName;


							CMachine();
		virtual			   ~CMachine();

	protected:

};

#endif // !defined(__MACHINE_H__)
