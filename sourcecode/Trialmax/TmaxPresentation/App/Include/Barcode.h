//==============================================================================
//
// File Name:	barcode.h
//
// Description:	This file contains the declarations of all classes used by
//				TrialMax to manage barcode operations.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-25-98	1.00		Original Release
//==============================================================================
#if !defined(__BARCODE_H__)
#define __BARCODE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMAX_BARCODE_DELIMITER	'.'

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class is used to manage a TrialMax barcode
class CBarcode : public CObject
{
	private:

	public:

		CString			m_strMediaId;
		long			m_lSecondaryId;
		long			m_lTertiaryId;

						CBarcode(LPCSTR lpszBarcode = NULL);
						CBarcode(const CBarcode& rBarcode);
					   ~CBarcode();

		BOOL			SetBarcode(LPCSTR lpszBarcode);
		CString			GetBarcode();

		UINT			MsgBox(HWND hWnd);
		void			Reset();
		void			operator = (const CBarcode& rBarcode);

	protected:
};

//	This class manages a sorted list of CBarcode objects. 
class CBarcodes : public CObList
{
	private:

		POSITION		m_NextPos;
		POSITION		m_PrevPos;

	public:

						CBarcodes();
		virtual		   ~CBarcodes();

		void			Flush(BOOL bDeleteAll);
		void			Add(CBarcode* pBarcode);
		void			Remove(CBarcode* pBarcode, BOOL bDelete);
		POSITION		Find(CBarcode* pBarcode);
		CBarcode*		Find(LPCSTR lpszBarcode);

		//	List iteration members
		CBarcode*		First();
		CBarcode*		Last();
		CBarcode*		Next();
		CBarcode*		Prev();
		CBarcode*		SetPos(CBarcode* pBarcode);

	protected:

};

//	This class manages a buffer of barcodes navigated by the application 
class CBarcodeBuffer
{
	private:

		CBarcode**		m_paBarcodes;
		int				m_iMaxBarcodes;
		int				m_iInBuffer;
		int				m_iActive;

	public:

						CBarcodeBuffer();
		virtual		   ~CBarcodeBuffer();

		int				GetMaxBarcodes(){ return m_iMaxBarcodes; }
		int				Add(LPCSTR lpszBarcode);
		int				Add(CBarcode& rBarcode);
		BOOL			SetMaxBarcodes(int iMaxBarcodes);

		void			Clear();

		CBarcode*		GetActive();
		CBarcode*		GetNext();
		CBarcode*		GetPrevious();

		BOOL			OnFirst();
		BOOL			OnLast();
		BOOL			IsEmpty(){ return (m_iInBuffer <= 0); }

		UINT			MsgBox(HWND hWnd);

	protected:

		void			Free();

};

#endif // !defined(__BARCODE_H__)
