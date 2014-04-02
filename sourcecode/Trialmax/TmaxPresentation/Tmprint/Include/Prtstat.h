//==============================================================================
//
// File Name:	prtstat.h
//
// Description:	This file contains the declaration of the CPrintStatus class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-15-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_PRTSTAT_H__A4EC5A46_CB33_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_PRTSTAT_H__A4EC5A46_CB33_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmpower.h>
#include <tmview.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CJob;

class CPrintStatus : public CDialog
{
	private:

		CJob*				m_pJob;
		int					m_iPages;
		int					m_iPage;
		long				m_lImages;
		long				m_lImage;
		BOOL				m_bPowerPoint;
		BOOL				m_bAbortJob;

	public:
	
	
	
							CPrintStatus(CJob* pJob = 0,
										 int iPages = 0,
										 long lImages = 0, 
										 BOOL bPowerPoint = FALSE,
										 CWnd* pParent = 0);
						   ~CPrintStatus(); 

		void				SetAbortJob();
		void				Terminate();
		void				SetPage(int iPage);
		void				SetImage(long lImage);
		void				OnLoadViewer(BOOL bIsPowerPoint);


		void				PrintEx(BOOL bFullImage, CDC* pdc, int iLeft, 
								    int iTop, int iWidth, int iHeight, BOOL bAutoRotate);

		BOOL				GetAbortJob();
		BOOL				PowerPointInitialized();
		BOOL				LoadZap(LPCSTR lpFilename, LPCSTR lpSourceImage,
									LPCSTR lpSibling, LPCSTR lpSiblingImage,
									long lFlags);
		BOOL				LoadImage(LPCSTR lpFilename);
		BOOL				LoadSlide(LPCSTR lpFilename, long lSlide, BOOL bUseId);
		BOOL				SaveSlide(LPCSTR lpFilename, int iFormat);

		float				GetAspectRatio();

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CPrintStatus)
	enum { IDD = IDD_PRINTER_STATUS };
	CStatic	m_ctrlImages;
	CStatic	m_ctrlPages;
	CString	m_strPages;
	CTMPower	m_TMPower;
	CTm_view	m_TMView;
	CString	m_strImages;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPrintStatus)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CPrintStatus)
	virtual void OnCancel();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PRTSTAT_H__A4EC5A46_CB33_11D3_8177_00802966F8C1__INCLUDED_)
