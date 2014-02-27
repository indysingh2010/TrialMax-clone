//==============================================================================
//
// File Name:	options.h
//
// Description:	This file contains the declaration of the COptions class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-15-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_OPTIONS_H__67E2EBB6_B2F6_11D3_BF86_0080296301C0__INCLUDED_)
#define AFX_OPTIONS_H__67E2EBB6_B2F6_11D3_BF86_0080296301C0__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>
#include <tmini.h>
#include <handler.h>
#include <tmview.h>
#include <tmpower.h>
#include <template.h>
#include <job.h>
#include <tmhelp.h>
#include <cell.h>
#include <tmprdefs.h>
#include <tmppdefs.h>
#include <colorctl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Ini line specifications
#define	INILINE_PRINTIMAGE				"PrintImage"
#define INILINE_PRINTBARCODE			"PrintBarcodeText"
#define INILINE_PRINTGRAPHIC			"PrintBarcodeGraphic"
#define INILINE_PRINTFOREIGNBARCODE		"PrintForeignBarcode"
#define INILINE_PRINTSOURCEBARCODE		"PrintSourceBarcode"
#define INILINE_PRINTFILENAME			"PrintFilename"
#define INILINE_PRINTNAME				"PrintName"
#define INILINE_PRINTDEPONENT			"PrintDeponent"
#define INILINE_PRINTPAGENUM			"PrintPageNum"
#define INILINE_PRINTBORDER				"PrintBorder"
#define INILINE_PRINTFULLPATH			"PrintFullPath"
#define INILINE_PRINTPAGETOTAL			"PrintPageTotal"
#define INILINE_COLLATE					"Collate"
#define INILINE_COPIES					"Copies"
#define INILINE_FORCENEWPAGE			"ForceNewPage"
#define INILINE_INSERTSLIPSHEET			"InsertSlipSheet"
#define INILINE_USESLIDEIDS				"UseSlideIDs"
#define INILINE_BARCODECHARACTER		"BarcodeCharacter"
#define INILINE_TEMPLATESECTION			"Templates"
#define INILINE_BARCODEFONT				"BarcodeFont"
#define INILINE_SAVEFORMAT				"SaveFormat"
#define INILINE_LEFTMARGIN				"LeftMargin"
#define INILINE_TOPMARGIN				"TopMargin"
#define INILINE_PRINTCALLOUTS			"PrintCallouts"
#define INILINE_PRINTCALLOUTBORDERS		"PrintCalloutBorders"
#define INILINE_PRINTBORDERTHICKNESS	"PrintBorderThickness"
#define INILINE_PRINTBORDERCOLOR		"PrintBorderColor"
#define INILINE_SHOWSTATUS				"ShowStatus"
#define INILINE_AUTOROTATE				"AutoRotate"
#define INILINE_JOBNAME					"JobName"

//	Default ini values
#define	DEFAULT_PRINTIMAGE				TRUE
#define DEFAULT_PRINTBARCODE			TRUE
#define DEFAULT_PRINTFOREIGNBARCODE		TRUE
#define DEFAULT_PRINTSOURCEBARCODE		TRUE
#define DEFAULT_PRINTGRAPHIC			TRUE
#define DEFAULT_PRINTFILENAME			TRUE
#define DEFAULT_PRINTPAGENUM			TRUE
#define DEFAULT_PRINTNAME				TRUE
#define DEFAULT_PRINTDEPONENT			TRUE
#define DEFAULT_PRINTCELLBORDER			TRUE
#define DEFAULT_PRINTBORDERCOLOR		((OLE_COLOR)RGB(0,0,0))
#define DEFAULT_PRINTBORDERTHICKNESS	0.025f
#define DEFAULT_PRINTFULLPATH			FALSE
#define DEFAULT_PRINTPAGETOTAL			FALSE
#define DEFAULT_COLLATE					TRUE
#define DEFAULT_COPIES					1
#define DEFAULT_FORCENEWPAGE			FALSE
#define DEFAULT_INSERTSLIPSHEET			FALSE
#define DEFAULT_USESLIDEIDS				FALSE
#define DEFAULT_BARCODECHARACTER		"x"
#define DEFAULT_TEMPLATESECTION			"PAGE TEMPLATES"
#define DEFAULT_BARCODEFONT				TMPRINT_BARCODEFONT
#define DEFAULT_SAVEFORMAT				TMPOWER_JPG
#define DEFAULT_CALLOUTS				TRUE
#define DEFAULT_CALLOUTBORDERS			TRUE
#define DEFAULT_LEFTMARGIN				-1.00f
#define DEFAULT_TOPMARGIN				-1.00f
#define DEFAULT_SHOWSTATUS				TRUE
#define DEFAULT_AUTOROTATE				FALSE
#define DEFAULT_JOBNAME					""

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMPrintCtrl;

class COptions : public CDialog
{
	private:
		
		CTMPrintCtrl*		m_pTMPrint;
		CTMHelp				m_Help;
		CErrorHandler*		m_pErrors;
		CTemplates			m_Templates;
		CTemplate*			m_pTemplate;
		CBrush				m_brRed;
		CTMIni				m_Ini;
		CCells				m_Queue;
		CJob				m_Printer;
		COLORREF			m_crPrintBorderColor;
		short				m_sCalloutFrameColor;
		int					m_iSaveFormat;

		CString				m_strIniFilespec;
		CString				m_strIniSection;
		CString				m_strTemplateSection;

		BOOL				m_bEnablePowerPoint;
		BOOL				m_bDIBPrintingEnabled;
		BOOL				m_bAborted;

	public:
	
							COptions(CTMPrintCtrl* pControl, 
									 CErrorHandler* pErrors);
						   ~COptions();
					   			
		BOOL				Create(); 
		BOOL				SetFromIni(LPCSTR lpFilespec, LPCSTR lpSection);
		BOOL				SetPrinter(LPCSTR lpPrinter);
		BOOL				SetPrinterProperties(HWND hWnd);
		BOOL				SelectPrinter();
		BOOL				SetTemplate(CTemplate* pTemplate);
		BOOL				GetAborted(){ return m_bAborted; }
		void				Abort();
		void				EnablePowerPoint(BOOL bEnable);
		void				EnableDIBPrinting(BOOL bEnable);
		void				SetForceNewPage(BOOL bForce);
		void				SetInsertSlipSheet(BOOL bForce);
		void				SetIncludePath(BOOL bInclude);
		void				SetIncludeTotal(BOOL bInclude);
		void				SetCollate(BOOL bCollate);
		void				SetCopies(int iCopies);
		void				SetShowStatus(BOOL bShowStatus);
		void				SetUseSlideId(BOOL bUseSlideId);
		void				SetBarcodeCharacter(char cBarcode);
		void				SetBarcodeFont(LPCSTR lpFontName);
		void				SetJobName(LPCSTR lpJobName);
		void				SetLeftMargin(float fMargin);
		void				SetRightMargin(float fMargin);
		void				SetTopMargin(float fMargin);
		void				SetBottomMargin(float fMargin);
		void				SetPrintCallouts(BOOL bPrint);
		void				SetPrintCalloutBorders(BOOL bPrint);
		void				SetPrintBorderThickness(float fThickness);
		void				SetPrintBorderColor(COLORREF crColor);
		void				SetCalloutFrameColor(short sColor);
		void				Flush();
		short				Add(LPCSTR lpString);
		short				Print();
		short				RefreshTemplates();
		short				SetTemplates(CTemplates* pTemplates);
		short				GetDefaultPrinter(CString& rPrinter);
		short				GetRowsPerPage();
		short				GetColumnsPerPage();
		long				GetQueueCount();
		long				GetEnabledMask(LPCSTR lpTemplate);
		long				GetDefaultMask(LPCSTR lpTemplate);
		CTemplate*			SetTemplate(LPCSTR lpTemplate);
		CTemplate*			GetTemplate(LPCSTR lpTemplate);
		CTemplate*			GetTemplate();
		CTemplates*			GetTemplates();

		void				OnStartJob(LPCSTR lpPrinter, long lPages, 
									   long lImages, CTemplate* pTemplate);
		void				OnEndJob(BOOL bAborted);
		void				OnPrintPage(long lPage);
		void				OnPrintImage(long lImage, LPCSTR lpFilename);

	protected:

		BOOL				SaveToIni();
		BOOL				ReadTemplates(BOOL bFlush);
		void				FillTemplates();
		void				SetControlStates();
		void				SetStatusText();
		void				FillPrinters();
	
	//	The remainder of this declaration is maintained with ClassWizard
	public:

	//{{AFX_DATA(COptions)
	enum { IDD = IDD_OPTIONS };
	CButton	m_ctrlInsertSlipSheet;
	CButton	m_ctrlSourceBarcode;
	CButton	m_ctrlForeignBarcode;
	CButton	m_ctrlAutoRotate;
	CButton	m_ctrlPageSeries;
	CButton	m_ctrlIncludePath;
	CButton	m_ctrlPrintBorder;
	CComboBox	m_ctrlPrinters;
	CButton	m_ctrlPrintCallouts;
	CButton	m_ctrlPrintCalloutBorders;
	CEdit	m_ctrlTopMargin;
	CEdit	m_ctrlLeftMargin;
	CColorPushbutton	m_ctrlBorderColor;
	CEdit	m_ctrlPrintBorderThickness;
	CButton	m_ctrlShowStatus;
	CButton	m_ctrlCollate;
	CEdit	m_ctrlCopies;
	CEdit	m_ctrlBarcodeFont;
	CEdit	m_ctrlBarcodeCharacter;
	CEdit	m_ctrlJobName;
	CButton	m_ctrlUseSlideId;
	CButton	m_ctrlForceNewPage;
	CButton	m_ctrlDeponent;
	CButton	m_ctrlName;
	CButton	m_ctrlReload;
	CButton	m_ctrlPageNum;
	CButton	m_ctrlImage;
	CButton	m_ctrlGraphic;
	CButton	m_ctrlFilename;
	CButton	m_ctrlBarcode;
	CButton	m_ctrlPrint;
	CListBox	m_ctrlTemplates;
	CStatic	m_ctrlLStatus;
	BOOL	m_bBarcode;
	BOOL	m_bFilename;
	BOOL	m_bGraphic;
	BOOL	m_bImage;
	BOOL	m_bIncludePath;
	BOOL	m_bPrintBorder;
	BOOL	m_bIncludeTotal;
	BOOL	m_bPageNum;
	BOOL	m_bName;
	BOOL	m_bDeponent;
	BOOL	m_bCollate;
	short	m_sCopies;
	BOOL	m_bForceNewPage;
	BOOL	m_bUseSlideId;
	CString	m_strBarcodeCharacter;
	CString	m_strBarcodeFont;
	CString	m_strJobName;
	BOOL	m_bShowStatus;
	float	m_fPrintBorderThickness;
	float	m_fLeftMargin;
	BOOL	m_bPrintCalloutBorders;
	BOOL	m_bPrintCallouts;
	float	m_fTopMargin;
	CString	m_strPrinter;
	BOOL	m_bAutoRotate;
	BOOL	m_bForeignBarcode;
	BOOL	m_bSourceBarcode;
	BOOL	m_bInsertSlipSheet;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(COptions)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(COptions)
	virtual BOOL OnInitDialog();
	afx_msg void OnTemplateChange();
	afx_msg void OnHelp();
	afx_msg void OnPrint();
	afx_msg void OnReload();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnFonts();
	afx_msg void OnBorderColor();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_OPTIONS_H__67E2EBB6_B2F6_11D3_BF86_0080296301C0__INCLUDED_)
