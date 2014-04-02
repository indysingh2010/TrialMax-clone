//==============================================================================
//
// File Name:	tmprint.h
//
// Description:	This file contains the declaration of the CTMPrintCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-13-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMPRINT_H__CCAA2374_B13D_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMPRINT_H__CCAA2374_B13D_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <options.h>
#include <objsafe.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

#define WM_ERROR_EVENT		(WM_USER + 1)

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTemplate;

class CTMPrintCtrl : public COleControl
{
	private:

						DECLARE_DYNCREATE(CTMPrintCtrl)

		COptions*		m_pOptions;
		CErrorHandler	m_Errors;
		CTMVersion		m_tmVersion;
		int				m_iHeight;
		int				m_iWidth;
		BOOL			m_bDIBPrintingEnabled;

	public:
	
						CTMPrintCtrl();
					   ~CTMPrintCtrl();

		void			OnStartJob(LPCSTR lpPrinter, long lPages, long lImages,
								   CTemplate* pTemplate);
		void			OnEndJob(BOOL bAborted);
		void			OnPrintPage(long lPage);
		void			OnPrintImage(long lImage, LPCSTR lpFilename);
									 
		LONG			OnWMErrorEvent(WPARAM wParam, LPARAM lParam);

	protected:

		BOOL			CheckVersion(DWORD dwVersion);
		void			CalculateSize();
		void			GetRegistration();
		void			SetProperties(CTemplate* pTemplate);

	//--------------------------------------------------------------------
	//	Dispatch interface for implementing object safety
	//
	//	This is required to eliminate warnings when the control is invoked
	//	directly from a browser
	DECLARE_INTERFACE_MAP()

	BEGIN_INTERFACE_PART(ObjSafety, IObjectSafety)
		STDMETHOD_(HRESULT, GetInterfaceSafetyOptions) ( 
            /* [in] */ REFIID riid,
            /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
            /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions
		);
        
        STDMETHOD_(HRESULT, SetInterfaceSafetyOptions) ( 
            /* [in] */ REFIID riid,
            /* [in] */ DWORD dwOptionSetMask,
            /* [in] */ DWORD dwEnabledOptions
		);
	END_INTERFACE_PART(ObjSafety);
	//--------------------------------------------------------------------

	//	The remainder of this declaration is maintained by Class Wizard

	public:
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTMPrintCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual BOOL OnSetExtent(LPSIZEL lpSizeL);
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMPrintCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMPrintCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMPrintCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMPrintCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMPrintCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSetFocus(CWnd* pOldWnd);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMPrintCtrl)
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	CString m_strIniFile;
	afx_msg void OnIniFileChanged();
	CString m_strIniSection;
	afx_msg void OnIniSectionChanged();
	BOOL m_bEnablePowerPoint;
	afx_msg void OnEnablePowerPointChanged();
	BOOL m_bCollate;
	afx_msg void OnCollateChanged();
	short m_sCopies;
	afx_msg void OnCopiesChanged();
	BOOL m_bIncludePathInFileName;
	afx_msg void OnIncludePathInFileNameChanged();
	BOOL m_bIncludePageTotal;
	afx_msg void OnIncludePageTotalChanged();
	BOOL m_bPrintImage;
	afx_msg void OnPrintImageChanged();
	BOOL m_bPrintBarcodeGraphic;
	afx_msg void OnPrintBarcodeGraphicChanged();
	BOOL m_bPrintBarcodeText;
	afx_msg void OnPrintBarcodeTextChanged();
	BOOL m_bPrintName;
	afx_msg void OnPrintNameChanged();
	BOOL m_bPrintFileName;
	afx_msg void OnPrintFileNameChanged();
	BOOL m_bPrintDeponent;
	afx_msg void OnPrintDeponentChanged();
	BOOL m_bPrintPageNumber;
	afx_msg void OnPrintPageNumberChanged();
	BOOL m_bPrintCellBorder;
	afx_msg void OnPrintCellBorderChanged();
	CString m_strPrinter;
	afx_msg void OnPrinterChanged();
	CString m_strTemplateName;
	afx_msg void OnTemplateNameChanged();
	BOOL m_bForceNewPage;
	afx_msg void OnForceNewPageChanged();
	BOOL m_bUseSlideIDs;
	afx_msg void OnUseSlideIDsChanged();
	CString m_strBarcodeCharacter;
	afx_msg void OnBarcodeCharacterChanged();
	BOOL m_bShowOptions;
	afx_msg void OnShowOptionsChanged();
	CString m_strBarcodeFont;
	afx_msg void OnBarcodeFontChanged();
	BOOL m_bShowStatus;
	afx_msg void OnShowStatusChanged();
	float m_fLeftMargin;
	afx_msg void OnLeftMarginChanged();
	float m_fTopMargin;
	afx_msg void OnTopMarginChanged();
	BOOL m_bPrintCalloutBorders;
	afx_msg void OnPrintCalloutBordersChanged();
	OLE_COLOR m_lPrintBorderColor;
	afx_msg void OnPrintBorderColorChanged();
	float m_fPrintBorderThickness;
	afx_msg void OnPrintBorderThicknessChanged();
	BOOL m_bPrintCallouts;
	afx_msg void OnPrintCalloutsChanged();
	BOOL m_bAutoRotate;
	afx_msg void OnAutoRotateChanged();
	BOOL m_bPrintForeignBarcode;
	afx_msg void OnPrintForeignBarcodeChanged();
	afx_msg short GetVerBuild();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg BSTR GetVerTextLong();
	afx_msg short Initialize();
	afx_msg short Add(LPCTSTR lpszString);
	afx_msg long GetQueueCount();
	afx_msg short Clear();
	afx_msg short RefreshTemplates();
	afx_msg short Print();
	afx_msg short EnumerateTemplates();
	afx_msg short EnumeratePrinters();
	afx_msg BSTR GetDefaultPrinter();
	afx_msg short SelectPrinter();
	afx_msg short SetPrintTemplates(long lTemplates);
	afx_msg short SetPrintTemplate(long lTemplate);
	afx_msg BOOL IsReady();
	afx_msg BSTR GetRegisteredPath();
	afx_msg BSTR GetClassIdString();
	afx_msg long GetPrintTemplates();
	afx_msg long GetPrintTemplate();
	afx_msg short GetRowsPerPage();
	afx_msg short GetColumnsPerPage();
	afx_msg short Abort();
	afx_msg long GetFieldEnabledMask(LPCTSTR lpszTemplate);
	afx_msg long GetFieldDefaultMask(LPCTSTR lpszTemplate);
	afx_msg BOOL SetPrinterProperties(OLE_HANDLE hWnd);
	afx_msg short ShowPrinterCaps();
	afx_msg void EnableDIBPrinting(short bEnable);
	afx_msg short EnumerateTextFields(LPCTSTR lpszTemplate);
	afx_msg short SetTextFieldEnabled(long lId, LPCTSTR lpszName, short bEnabled);
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.0
	BOOL m_bEnableAxErrors;
	afx_msg void OnEnableAxErrorsChanged();

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	//	Added in rev 6.1.6
	CString m_strJobName;
	afx_msg void OnJobNameChanged();

	//	Added in rev 6.2.3
	BOOL m_bPrintSourceBarcode;
	afx_msg void OnPrintSourceBarcodeChanged();

	//	Added in rev 6.3.3
	BOOL m_bInsertSlipSheet;
	afx_msg void OnInsertSlipSheetChanged();
	short m_sCalloutFrameColor;
	afx_msg void OnCalloutFrameColorChanged();

	//{{AFX_EVENT(CTMPrintCtrl)
	void FireFirstTemplate(LPCTSTR lpszTemplate)
		{FireEvent(eventidFirstTemplate,EVENT_PARAM(VTS_BSTR), lpszTemplate);}
	void FireNextTemplate(LPCTSTR lpszTemplate)
		{FireEvent(eventidNextTemplate,EVENT_PARAM(VTS_BSTR), lpszTemplate);}
	void FireFirstPrinter(LPCTSTR lpszPrinter)
		{FireEvent(eventidFirstPrinter,EVENT_PARAM(VTS_BSTR), lpszPrinter);}
	void FireNextPrinter(LPCTSTR lpszPrinter)
		{FireEvent(eventidNextPrinter,EVENT_PARAM(VTS_BSTR), lpszPrinter);}
	void FireEndJob(short bAborted)
		{FireEvent(eventidEndJob,EVENT_PARAM(VTS_I2), bAborted);}
	void FirePrintPage(long lPage)
		{FireEvent(eventidPrintPage,EVENT_PARAM(VTS_I4), lPage);}
	void FireStartJob(LPCTSTR lpszPrinter, long lPages, long lImages, long lpTemplate)
		{FireEvent(eventidStartJob,EVENT_PARAM(VTS_BSTR  VTS_I4  VTS_I4  VTS_I4), lpszPrinter, lPages, lImages, lpTemplate);}
	void FirePrintImage(long lImage, LPCTSTR lpszFilename)
		{FireEvent(eventidPrintImage,EVENT_PARAM(VTS_I4  VTS_BSTR), lImage, lpszFilename);}
	void FireAxError(LPCTSTR lpszMessage)
		{FireEvent(eventidAxError,EVENT_PARAM(VTS_BSTR), lpszMessage);}
	void FireAxDiagnostic(LPCTSTR lpszMethod, LPCTSTR lpszMessage)
		{FireEvent(eventidAxDiagnostic,EVENT_PARAM(VTS_BSTR  VTS_BSTR), lpszMethod, lpszMessage);}
	void FireFirstTextField(long lId, LPCTSTR lpszName, LPCTSTR lpszText, short bPrint, short bDefault)
		{FireEvent(eventidFirstTextField,EVENT_PARAM(VTS_I4  VTS_BSTR  VTS_BSTR  VTS_I2  VTS_I2), lId, lpszName, lpszText, bPrint, bDefault);}
	void FireNextTextField(long lId, LPCTSTR lpszName, LPCTSTR lpszText, short bPrint, short bDefault)
		{FireEvent(eventidNextTextField,EVENT_PARAM(VTS_I4  VTS_BSTR  VTS_BSTR  VTS_I2  VTS_I2), lId, lpszName, lpszText, bPrint, bDefault);}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMPrintCtrl)
	dispidAutoInit = 1L,
	dispidVerBuild = 34L,
	dispidEnableErrors = 2L,
	dispidVerMajor = 35L,
	dispidVerMinor = 36L,
	dispidVerTextLong = 37L,
	dispidIniFile = 3L,
	dispidIniSection = 4L,
	dispidEnablePowerPoint = 5L,
	dispidCollate = 6L,
	dispidCopies = 7L,
	dispidIncludePathInFileName = 8L,
	dispidIncludePageTotal = 9L,
	dispidPrintImage = 10L,
	dispidPrintBarcodeGraphic = 11L,
	dispidPrintBarcodeText = 12L,
	dispidPrintName = 13L,
	dispidPrintFileName = 14L,
	dispidPrintDeponent = 15L,
	dispidPrintPageNumber = 16L,
	dispidPrintCellBorder = 17L,
	dispidPrinter = 18L,
	dispidTemplateName = 19L,
	dispidForceNewPage = 20L,
	dispidUseSlideIDs = 21L,
	dispidBarcodeCharacter = 22L,
	dispidShowOptions = 23L,
	dispidBarcodeFont = 24L,
	dispidShowStatus = 25L,
	dispidLeftMargin = 26L,
	dispidTopMargin = 27L,
	dispidPrintCalloutBorders = 28L,
	dispidPrintBorderColor = 29L,
	dispidPrintBorderThickness = 30L,
	dispidPrintCallouts = 31L,
	dispidAutoRotate = 32L,
	dispidPrintForeignBarcode = 33L,
	dispidInitialize = 38L,
	dispidAdd = 39L,
	dispidGetQueueCount = 40L,
	dispidClear = 41L,
	dispidRefreshTemplates = 42L,
	dispidPrint = 43L,
	dispidEnumerateTemplates = 44L,
	dispidEnumeratePrinters = 45L,
	dispidGetDefaultPrinter = 46L,
	dispidSelectPrinter = 47L,
	dispidSetPrintTemplates = 48L,
	dispidSetPrintTemplate = 49L,
	dispidIsReady = 50L,
	dispidGetRegisteredPath = 51L,
	dispidGetClassIdString = 52L,
	dispidGetPrintTemplates = 53L,
	dispidGetPrintTemplate = 54L,
	dispidGetRowsPerPage = 55L,
	dispidGetColumnsPerPage = 56L,
	dispidAbort = 57L,
	dispidGetFieldEnabledMask = 58L,
	dispidGetFieldDefaultMask = 59L,
	dispidSetPrinterProperties = 60L,
	dispidShowPrinterCaps = 61L,
	dispidEnableDIBPrinting = 62L,
	dispidEnumerateTextFields = 63L,
	dispidSetTextFieldEnabled = 64L,
	eventidFirstTemplate = 1L,
	eventidNextTemplate = 2L,
	eventidFirstPrinter = 3L,
	eventidNextPrinter = 4L,
	eventidEndJob = 5L,
	eventidPrintPage = 6L,
	eventidStartJob = 7L,
	eventidPrintImage = 8L,
	eventidAxError = 9L,
	eventidAxDiagnostic = 10L,
	eventidFirstTextField = 11L,
	eventidNextTextField = 12L,
	//}}AFX_DISP_ID
	};

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMPRINT_H__CCAA2374_B13D_11D3_8177_00802966F8C1__INCLUDED)
