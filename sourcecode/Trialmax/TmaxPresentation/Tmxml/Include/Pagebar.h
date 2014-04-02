//==============================================================================
//
// File Name:	pagebar.h
//
// Description:	This file contains the declarations of the CPageBar class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	06-19-02	1.00		Original Release
//==============================================================================
#if !defined(__PAGEBAR_H__)
#define __PAGEBAR_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmtool.h>
#include <pbtool.h>
#include <pbtext.h>
#include <pblist.h>
#include <tmini.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define PAGEBAR_LEFT_BAND			0
#define PAGEBAR_TOOLBAR_BAND		1
#define PAGEBAR_TEXT_BAND			2
#define PAGEBAR_LIST_BAND			3
#define PAGEBAR_RIGHT_BAND			4
#define PAGEBAR_MAX_BANDS			5

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CXmlFrame;
class CXmlMedia;
class CXmlPage;

class CPageBar : public CReBar
{
	private:

		CPBBand*			m_aBands[PAGEBAR_MAX_BANDS];
		CPBToolbar			m_pbToolbar;
		CPBList				m_pbList;
		CPBText				m_pbText;
		CPBBand				m_pbLeft;
		CPBBand				m_pbRight;
		CXmlFrame*			m_pXmlFrame;
		CFont				m_SmallFont;
		CFont				m_MediumFont;
		CFont				m_LargeFont;
		COLORREF			m_crBackground;
		COLORREF			m_crForeground;
		BOOL				m_bVisible;

	public:
	
							CPageBar();
						   ~CPageBar();

		void				SetXmlMedia(CXmlMedia* pXmlMedia);
		void				SetXmlPage(CXmlPage* pXmlPage);
		void				OnBandSize(int iId);
		void				OnButtonClick(short sId, BOOL bChecked);
		void				OnSelChanged(CXmlPage* pXmlPage);
		void				SetVisible(BOOL bVisible){ m_bVisible = bVisible; }
		BOOL				GetVisible(){ return m_bVisible; }
		BOOL				Initialize(LPCSTR lpszFilename, LPCSTR lpszSection);
		BOOL				Save(LPCSTR lpszFilename, LPCSTR lpszSection);
		BOOL				Create(CXmlFrame* pFrame);
		BOOL				SetToolbarProps(CTMTool& rSource);
		BOOL				EnableButton(short sId, BOOL bEnabled);
		BOOL				Build();
		int					GetMinHeight();

	protected:

		void				RecalcLayout();
		BOOL				Add(CPBBand* pBand);

		afx_msg void		OnSize(UINT nType, int cx, int cy);

		DECLARE_MESSAGE_MAP()
};

#endif // !defined(__PAGEBAR_H__)
