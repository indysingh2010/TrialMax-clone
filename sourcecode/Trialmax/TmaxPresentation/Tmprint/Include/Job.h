//==============================================================================
//
// File Name:	job.h
//
// Description:	This file contains the declarations of the CJob class. This 
//				class encapsulates the data and operations required to implement
//				an individual print job.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-19-99	1.00		Original Release
//==============================================================================
#if !defined(__JOB_H__)
#define __JOB_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <cell.h>
#include <template.h>
#include <resource.h>
#include <winspool.h>
#include <printer.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Printer error levels	
#define TMPRINTER_NOERROR			0
#define TMPRINTER_NOTEMPLATE		1
#define TMPRINTER_NOTOPEN			2

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations of classes contained in this file
class CJob;
class CPrintStatus;

//	Forward declarations of classes declared outside this file
class CTm_view;
class CTMPower;
class COptions;

class CJob : public CTMPrinter
{
	private:

		COptions*		m_pOptions;
		CCells			m_Page;
		CCells*			m_pQueue;
		CTemplate*		m_pTemplate;
		CString			m_strBarcodeFont;

		int				m_iPgWidth;
		int				m_iPgHeight;
		int				m_iRows;
		int				m_iCols;
		int				m_iCellWidth;
		int				m_iCellHeight;
		int				m_iCellSpaceX;
		int				m_iCellSpaceY;
		int				m_iCellPadX;
		int				m_iCellPadY;
		int				m_iImagePadX;
		int				m_iImagePadY;
		int				m_iCellsPerPage;
		int				m_iCells;
		int				m_iPages;
		int				m_iSaveFormat;
		long			m_lImages;
		long			m_lImage;
		short			m_sCalloutFrameColor;
		float			m_fPrintBorderThickness;
		COLORREF		m_crPrintBorderColor;
		BOOL			m_bEnablePowerPoint;
		BOOL			m_bEnableDIBPrinting;
		BOOL			m_bBreakOnDocument;
		BOOL			m_bInsertSlipSheet;
		BOOL			m_bAutoRotateImage;
		BOOL			m_bUseSlideId;
		BOOL			m_bShowStatus;
		BOOL			m_bPrintCallouts;
		BOOL			m_bPrintCalloutBorders;
		char			m_cBarcode;

	public:

						CJob();
					   ~CJob();

		void			SetBarcodeCharacter(char cBarcode);
		void			SetEnablePowerPoint(BOOL bEnable);
		void			SetEnableDIBPrinting(BOOL bEnable);
		void			SetBreakOnDocument(BOOL bBreak);
		void			SetInsertSlipSheet(BOOL bInsert);
		void			SetUseSlideId(BOOL bUseSlideId);
		void			SetShowStatus(BOOL bShowStatus);
		void			SetPrintCallouts(BOOL bPrint);
		void			SetPrintCalloutBorders(BOOL bPrint);
		void			SetPrintBorderThickness(float fThickness);
		void			SetPrintBorderColor(COLORREF crColor);
		void			SetCalloutFrameColor(short sColor);
		void			SetOptions(COptions* pOptions);
		void			SetBarcodeFont(LPCSTR lpFontName);
		void			SetSaveFormat(int iFormat);
		void			SetTemplate(CTemplate* pTemplate);
		void			ShowExtents(HWND hWnd);

		BOOL			GetEnableDIBPrinting(){ return m_bEnableDIBPrinting; }
		BOOL			GetPrintCallouts(){ return m_bPrintCallouts; }
		BOOL			GetPrintCalloutBorders(){ return m_bPrintCalloutBorders; }
		float			GetPrintBorderThickness(){ return m_fPrintBorderThickness; }
		COLORREF		GetPrintBorderColor(){ return m_crPrintBorderColor; }
		short			GetCalloutFrameColor(){ return m_sCalloutFrameColor; }
		
		//	Printing operations
		int				Print(CCells* pCells, CWnd* pParent, 
							  short sCopies, BOOL bCollate);		

		static BOOL CALLBACK AbortTMPrint(HDC hDc, int iCode);

	protected:

		void			CloseStatus();
		void			GetPageExtents();
		void			SubtractRect(RECT* pRect1, RECT* pRect2);
		void			PrintCell(int iLeft, int iTop, CCell* pCell);
		void			PrintText(LPCSTR lpText, CTemplateField* pField,
								  RECT* pCellRect, RECT* pRect);
		void			PrintGraphic(CCell* pCell, CTemplateField* pField,
									 RECT* pCellRect, RECT* pRect);
		void			PrintTMView(RECT* pImage, LPCSTR lpFilename,
								    long lTertiary, BOOL bNotify,
									LPCSTR lpTreatmentSource, LPCSTR lpSibling,
									LPCSTR lpSiblingImage, long lFlags);
		void			PrintTMPower(RECT* pImage, LPCSTR lpFilename, long lSlide);
		void			DrawText(LPCSTR lpText, CTemplateField* pField,
								 RECT* pCellRect, RECT* pRect);
		BOOL			PrintPage();
		BOOL			PrintSlipSheet();
		BOOL			FindFile(LPCSTR lpszFilename);

		//	Overridden base class members
		BOOL			SetDevModeProperties(BOOL bPrinting);
};

#endif // !defined(__JOB_H__)
