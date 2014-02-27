//==============================================================================
//
// File Name:	tmtrack.h
//
// Description:	This file contains the declaration of the CTMTracker class.
//
// Author:		Kenneth Moore
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	06-01-02	1.00		Original Release
//==============================================================================
#if !defined(__TMTRACK_H__)
#define __TMTRACK_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Grab handle identifiers
#define TMTRACKER_GH_NONE			0
#define TMTRACKER_GH_LEFT			1
#define TMTRACKER_GH_RIGHT			2
#define TMTRACKER_GH_TOP			3
#define TMTRACKER_GH_BOTTOM			4
#define TMTRACKER_GH_TOPLEFT		5
#define TMTRACKER_GH_BOTTOMLEFT		6
#define TMTRACKER_GH_BOTTOMRIGHT	7
#define TMTRACKER_GH_TOPRIGHT		8

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CTMTracker : public CRectTracker
{
	private:

		HWND					m_hAttachment;
		float					m_fAspectRatio;
		int						m_iFrameThickness;
		COLORREF				m_crHandle;

	public:

								CTMTracker();
		virtual				   ~CTMTracker();

		void					Attach(HWND hWnd);
		void					Move();
		void					Draw();
		void					SetHandleColor(COLORREF crColor){ m_crHandle = crColor; }
		void					SetAspectRatio(float fRatio){ m_fAspectRatio = fRatio; }
		void					SetFrameThickness(int iThickness){ m_iFrameThickness = iThickness;
																   m_nHandleSize = iThickness; }
		int						GetFrameThickness(){ return m_iFrameThickness; }
		BOOL					Track(LPRECT lpRect);
		BOOL					SetCursor(CWnd* pWnd, UINT uHitTest);
		int						GetHandle();
		UINT					GetHandleMask() const;

	protected:

		void					AdjustRect(int iHandle, LPRECT lpRect);
		BOOL					GetPosition(LPPOINT lpPoint);
		int						Translate(int iHandle);

};

#endif // !defined(__TMTRACK_H__)
