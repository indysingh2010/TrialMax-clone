//==============================================================================
//
// File Name:	printer.h
//
// Description:	This file contains the declaration of the CTMPrinter class.
//
// Author:		Kenneth Moore
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	04-29-01	1.00		Original Release
//==============================================================================
#if !defined(__PRINTER_H__)
#define __PRINTER_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <winspool.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

#define TMPRINTER_ENUM_FLAGS (PRINTER_ENUM_LOCAL | PRINTER_ENUM_CONNECTIONS)

//	Orientation identifiers
#define TMPRINTER_ORIENTATION_DEVICE		0
#define TMPRINTER_ORIENTATION_PORTRAIT		1
#define TMPRINTER_ORIENTATION_LANDSCAPE		2

//	TMPrinter Error Identifiers
#define TMPRINTER_ERROR_NONE					 0
#define TMPRINTER_ERROR_NOT_ATTACHED			-1
#define TMPRINTER_ERROR_CREATE_DC_FAILED		-2
#define TMPRINTER_ERROR_DRIVER_NOT_FOUND		-3
#define TMPRINTER_ERROR_NO_DEFAULT_PRINTER		-4
#define TMPRINTER_ERROR_START_JOB_FAILED		-5
#define TMPRINTER_ERROR_GET_DEVICE_MODE_FAILED	-6
#define TMPRINTER_ERROR_SET_MODE_PROPS_FAILED	-7
#define TMPRINTER_ERROR_OPEN_PRINTER_FAILED		-8

#define FREE_HANDLE(x) { if(x) GlobalFree(x); x = NULL; }

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CTMPrinter
{
	private:

	public:

								CTMPrinter();
		virtual				   ~CTMPrinter();

		//	Properties
		virtual CDC*			GetDC(){ return m_pDC; }
		virtual LPCSTR			GetName(){ return m_strPrinter; }
		virtual BOOL			GetDefault(CString& rName);
		virtual BOOL			GetDefault(LPSTR lpName, int iNameLength);
		virtual BOOL			GetCollate(){ return m_bCollate; }
		virtual RECT*			GetMaxRect(){ return &m_rcMax; }
		virtual int				GetOrientation(){ return m_iOrientation; }
		virtual int				GetXDpi(){ return m_iXDpi; }
		virtual int				GetYDpi(){ return m_iYDpi; }
		virtual int				GetError(){ return m_iError; }
		virtual int				GetCopies(){ return m_iCopies; }
		virtual LPCSTR			GetErrorMsg(){ return m_strErrorMsg; }
		virtual float			GetLeftMargin(){ return m_fLeftMargin; }
		virtual float			GetRightMargin(){ return m_fRightMargin; }
		virtual float			GetTopMargin(){ return m_fTopMargin; }
		virtual float			GetBottomMargin(){ return m_fBottomMargin; }

		virtual void			SetOrientation(int iOrientation){ m_iOrientation = iOrientation; }
		virtual void			SetLeftMargin(float fInches){ m_fLeftMargin = (fInches > 0) ? fInches : 0.0f; }
		virtual void			SetRightMargin(float fInches){ m_fRightMargin = (fInches > 0) ? fInches : 0.0f; }
		virtual void			SetTopMargin(float fInches){ m_fTopMargin = (fInches > 0) ? fInches : 0.0f; }
		virtual void			SetBottomMargin(float fInches){ m_fBottomMargin = (fInches > 0) ? fInches : 0.0f; }
		virtual void			SetName(LPCSTR lpszName);
		virtual void			SetCopies(int iCopies){ m_iCopies = (iCopies > 0) ? iCopies : 1; }
		virtual void			SetCollate(BOOL bCollate){ m_bCollate = bCollate; }

		//	Operations
		virtual void			Select(CWnd* pParent);
		virtual void			Abort();
		virtual void			End();
		virtual CDC*			Start(LPCSTR lpTitle = NULL);
		virtual CDC*			GetNamedDC(LPCSTR lpszName);
		virtual HGLOBAL			GetNamedDevMode(LPCSTR lpszName);
		virtual BOOL			EndPage();
		virtual BOOL			StartPage();
		virtual BOOL			Attach();
		virtual int				SetProperties(HWND hWnd);
		virtual CObList*		EnumPrinters(DWORD dwFlags = TMPRINTER_ENUM_FLAGS);

	protected:

		HGLOBAL					m_hDevMode;
		HGLOBAL					m_hDevNames;
		CDC*					m_pDC;
		CString					m_strAttached;
		CString					m_strPrinter;
		CString					m_strDriver;
		CString					m_strDevice;
		CString					m_strPort;
		CString					m_strErrorMsg;
		RECT					m_rcMax;
		int						m_iXDpi;
		int						m_iYDpi;
		int						m_iOrientation;
		int						m_iCopies;
		int						m_iError;
		float					m_fLeftMargin;
		float					m_fRightMargin;
		float					m_fTopMargin;
		float					m_fBottomMargin;
		BOOL					m_bCollate;

		virtual void			Free();
		virtual void			FreeDC();
		virtual void			CrackDevNames();
		virtual void			GetDevModeProperties();
		virtual BOOL			GetDevMode();
		virtual BOOL			SetDevModeProperties(BOOL bPrinting);
		virtual BOOL			SetDevModeOrientation();
		virtual BOOL			SetDevModeScale();
		virtual BOOL			CreateDC();
		virtual BOOL			GetDriver(LPCSTR lpName);
		virtual void			MessageBox(LPCSTR lpTitle, LPCSTR lpFormat, ...);
		virtual void			HandleError(int iError, long lParam1 = 0, long lParam2 = 0);
		virtual void			ShowDeviceInfo();
};

#endif // !defined(__PRINTER_H__)
