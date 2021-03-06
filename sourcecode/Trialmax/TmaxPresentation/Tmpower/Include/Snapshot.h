//==============================================================================
//
// File Name:	snapshot.h
//
// Description:	This file contains the declaration of the CSnapshot class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-04-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_SNAPSHOT_H__BD9C3501_4B0F_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_SNAPSHOT_H__BD9C3501_4B0F_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define SNAPSHOT_ID		100

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CPowerPoint;

class CSnapshot : public CWnd
{
	private:

		CPowerPoint*			m_pParent;
		CDC*					m_pScratch;
		BITMAPINFOHEADER*		m_pDIBHeader;
		LPBYTE					m_pDIBBytes;
		HBITMAP					m_hDDBitmap;
		int						m_iDDBWidth;
		int						m_iDDBHeight;
		int						m_iDIBWidth;
		int						m_iDIBHeight;

	public:
	
								CSnapshot();
							   ~CSnapshot();

		BOOL					Create(CPowerPoint* pParent, BOOL bPopup = FALSE);
		void					SetDDBitmap(HBITMAP hDDB, int Width, int Height);
		void					SetDIBitmap(BITMAPINFOHEADER* pHeader);

	protected:

		void					FreeDDBitmap();
		void					FreeDIBitmap();
		void					DrawDDB(CPaintDC* pdc, RECT* pRect);
		void					DrawDIB(CPaintDC* pdc, RECT* pRect);
		BOOL					CreatePopup();

	//	The remainder of this declaration is maintained by ClassWizard
	public:
	
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSnapshot)
	//}}AFX_VIRTUAL

	protected:
	//{{AFX_MSG(CSnapshot)
	afx_msg void OnPaint();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SNAPSHOT_H__BD9C3501_4B0F_11D3_8177_00802966F8C1__INCLUDED_)
