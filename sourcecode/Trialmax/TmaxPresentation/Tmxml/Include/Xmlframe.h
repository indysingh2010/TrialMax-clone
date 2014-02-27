//==============================================================================
//
// File Name:	xmlframe.h
//
// Description:	This file contains the declaration of the CXmlFrame class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-03-01	1.00		Original Release
//==============================================================================
#if !defined(__XMLFRAME_H__)
#define __XMLFRAME_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>
#include <handler.h>
#include <tmtool.h>
#include <tmview.h>
#include <tmbrowse.h>
#include <tmprint.h>
#include <prefer.h>
#include <wrapxml.h>
#include <xmlmedia.h>
#include <xmlact.h>
#include <xmlset.h>
#include <template.h>
#include <propsht.h>
#include <tmini.h>
#include <printpro.h>
#include <shdocvw.h>
#include <cell.h>
#include <extension.h>
#include <pagebar.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

#define TMXML_FILE_EXTENSION					"tmx"
#define TMXML_INI_FILENAME						"Tmxml.ini"
#define TMXML_TREATMENT_FILENAME				"Tmxml.zap"
#define TMXML_INI_TOOLBAR_SECTION				"TOOLBAR"
#define TMXML_INI_PAGEBAR_SECTION				"PAGE NAVIGATION"
#define TMXML_INI_VIEWER_SECTION				"VIEWER"
#define TMXML_INI_TEMPLATES_SECTION				"TEMPLATES"
#define TMXML_INI_EXTENSIONS_SECTION			"EXTENSIONS"

//	Viewer identifiers
#define TMXML_VIEWER_UNKNOWN					0
#define TMXML_VIEWER_TMVIEW						1
#define TMXML_VIEWER_TMBROWSE					2

//	RingTail scripts
#define TMXML_RINGTAIL_GET_XML					"fti/trialmax.asp"
#define TMXML_RINGTAIL_PUT_TREATMENT			"fti/put_treatment.asp"
#define TMXML_RINGTAIL_DELETE_TREATMENT			"fti/delete_treatment.asp"

#define TMXML_PROGRESS_DELAY_LINE				"ProgressDelay"
#define TMXML_ENABLE_ERRORS_LINE				"EnableErrors"
#define TMXML_FLOAT_PRINT_PROGRESS_LINE			"FloatPrintProgress"
#define TMXML_SHOW_PAGE_NAVIGATION_LINE			"ShowPageNavigation"
#define TMXML_PRINTER_LINE						"Printer"
#define TMXML_CONFIRM_BATCH_LINE				"ConfirmBatch"
#define TMXML_PRINT_TEMPLATE_LINE				"PrintTemplate"
#define TMXML_PRINT_COPIES_LINE					"Copies"
#define TMXML_PRINT_COLLATE_LINE				"Collate"
#define TMXML_DIAGNOSTICS_LINE					"Diagnostics"
#define TMXML_CONNECTION_LINE					"Connection"
#define TMXML_INTERNET_PORT_LINE				"InternetPort"
#define TMXML_PROXY_SERVER_LINE					"ProxyServer"
#define TMXML_PROXY_PORT_LINE					"ProxyPort"
#define TMXML_MINIMIZE_COLOR_DEPTH_LINE			"MinimizeColorDepth"
#define TMXML_COMBINE_PRINT_PAGES_LINE			"CombinePrintPages"

#define TMXML_MEDIATREES_FIRST					11000
#define TMXML_MEDIATREES_PREVIOUS				11001
#define TMXML_MEDIATREES_NEXT					11002
#define TMXML_MEDIATREES_LAST					11003

#define TMXML_MEDIA_FIRST						12000
#define TMXML_MEDIA_PREVIOUS					12001
#define TMXML_MEDIA_NEXT						12002
#define TMXML_MEDIA_LAST						12003

#define TMXML_PAGES_FIRST						13000
#define TMXML_PAGES_PREVIOUS					13001
#define TMXML_PAGES_NEXT						13002
#define TMXML_PAGES_LAST						13003

#define TMXML_TREATMENTS_FIRST					14000
#define TMXML_TREATMENTS_PREVIOUS				14001
#define TMXML_TREATMENTS_NEXT					14002
#define TMXML_TREATMENTS_LAST					14003
#define TMXML_TREATMENTS_SAVE					14004
#define TMXML_TREATMENTS_UPDATE					14005
#define TMXML_TREATMENTS_DELETE					14006

#define TMXML_VIEW_NORMAL						15000
#define TMXML_VIEW_FULLWIDTH					15001

#define TMXML_ROTATE_CW							16000
#define TMXML_ROTATE_CCW						16001

#define TMXML_ERASE_ALL							17000
#define TMXML_ERASE_SELECTIONS					17001

#define TMXML_PRINT_CURRENT						18000
#define TMXML_PRINT_SELECTIONS					18001
#define TMXML_PRINT_SETUP						18002
#define TMXML_PRINT_DOCUMENT					18003

#define TMXML_TOOL_ZOOM							19000
#define TMXML_TOOL_ZOOM_RESTRICTED				19001
#define TMXML_TOOL_PAN							19002
#define TMXML_TOOL_HIGHLIGHT					19003
#define TMXML_TOOL_REDACT						19004
#define TMXML_TOOL_CALLOUT						19005
#define TMXML_TOOL_SELECT						19006

#define TMXML_DRAW_FREEHAND						19100
#define TMXML_DRAW_LINE							19101
#define TMXML_DRAW_ARROW						19102
#define TMXML_DRAW_ELLIPSE						19103
#define TMXML_DRAW_RECTANGLE					19104
#define TMXML_DRAW_FILLEDELLIPSE				19105
#define TMXML_DRAW_FILLEDRECTANGLE				19106
#define TMXML_DRAW_POLYLINE						19107
#define TMXML_DRAW_POLYGON						19108
#define TMXML_DRAW_TEXT							19109

#define TMXML_COLOR_BLACK						20000
#define TMXML_COLOR_RED							20001
#define TMXML_COLOR_GREEN						20002
#define TMXML_COLOR_BLUE						20003
#define TMXML_COLOR_YELLOW						20004
#define TMXML_COLOR_MAGENTA						20005
#define TMXML_COLOR_CYAN						20006
#define TMXML_COLOR_GREY						20007
#define TMXML_COLOR_WHITE						20008
#define TMXML_COLOR_DARKRED						20009
#define TMXML_COLOR_DARKGREEN					20010
#define TMXML_COLOR_DARKBLUE					20011
#define TMXML_COLOR_LIGHTRED					20012
#define TMXML_COLOR_LIGHTGREEN					20013
#define TMXML_COLOR_LIGHTBLUE					20014

#define TMXML_POPUP_PREFERENCES					10000
#define TMXML_POPUP_PROPERTIES					10001
#define TMXML_POPUP_DIAGNOSTICS					10002

#define TMXML_REQUEST_SUCCESS					200
#define TMXML_REQUEST_NOTFOUND					404

//	Put Treatment request errors
#define TMXML_PUT_TREATMENT_SUCCESS				200
#define TMXML_PUT_TREATMENT_ASP_NOT_FOUND		404
#define TMXML_PUT_TREATMENT_BAD_REQUEST			400
#define TMXML_PUT_TREATMENT_OBJECT_NOT_FOUND	406
#define TMXML_PUT_TREATMENT_NO_DATABASE			409

//	Put Treatment action identifiers
#define TMXML_PUT_TREATMENT_SAVE				1
#define TMXML_PUT_TREATMENT_UPDATE				2

//	Put Treatment request errors
#define TMXML_DELETE_TREATMENT_SUCCESS			200
#define TMXML_DELETE_TREATMENT_ASP_NOT_FOUND	404
#define TMXML_DELETE_TREATMENT_BAD_REQUEST		400
#define TMXML_DELETE_TREATMENT_OBJECT_NOT_FOUND	406
#define TMXML_DELETE_TREATMENT_NO_DATABASE		409

//	XML element tags
#define TMXML_ELEMENT_TMXROOT					"tmx"
#define TMXML_ELEMENT_SETTINGS					"tmxSettings"
#define TMXML_ELEMENT_SETTING					"tmxSetting"
#define TMXML_ELEMENT_ACTIONS					"tmxActions"
#define TMXML_ELEMENT_ACTION					"tmxAction"
#define TMXML_ELEMENT_MEDIA_TREE				"tmxMediaTree"
#define TMXML_ELEMENT_MEDIA						"tmxMedia"
#define TMXML_ELEMENT_PAGE						"tmxPage"
#define TMXML_ELEMENT_TREATMENT					"tmxTreatment"

#define WM_DOWNLOAD								(WM_USER + 1)
#define TMXML_DOWNLOAD_COMPLETE					1
#define TMXML_DOWNLOAD_PROGRESS					2

#define TMXML_POST_DELIMITER					"---------------------------7d11411a0"

#define DELETE_INTERFACE(x) { if (x) delete x; x = 0; }
#define RELEASE_INTERFACE(x) { if (x) x->Release(); x = 0; }
#define ENABLE_BUTTON(x,y){ m_TMTool.EnableButton(x,y); m_PageBar.EnableButton(x,y); }

#define MENU_ENABLED	(MF_BYCOMMAND | MF_ENABLED)
#define MENU_DISABLED	(MF_BYCOMMAND | MF_DISABLED | MF_GRAYED)


//	Extracted from MSXML type library
//typedef enum 
//{
//	NODE_INVALID = 0,
//    NODE_ELEMENT = 1,
//    NODE_ATTRIBUTE = 2,
//    NODE_TEXT = 3,
//    NODE_CDATA_SECTION = 4,
//    NODE_ENTITY_REFERENCE = 5,
//    NODE_ENTITY = 6,
//    NODE_PROCESSING_INSTRUCTION = 7,
//    NODE_COMMENT = 8,
//    NODE_DOCUMENT = 9,
//    NODE_DOCUMENT_TYPE = 10,
//    NODE_DOCUMENT_FRAGMENT = 11,
//    NODE_NOTATION = 12
//}XMLNodeTypes;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This structure is used to group the user defined viewer options
typedef struct
{
	float	fProgressDelay;
	BOOL	bEnableErrors;
	BOOL	bFloatPrintProgress;
	BOOL	bCombinePrintPages;
	BOOL	bMinimizeColorDepth;
	BOOL	bShowPageNavigation;
	int		iCopies;
	int		iConnection;
	UINT	uInternetPort;
	UINT	uProxyPort;
	BOOL	bCollate;
	BOOL	bCurrentSession;
	BOOL	bConfirmBatch;
	BOOL	bDiagnostics;
	char	szPrinter[256];
	char	szTemplate[256];
	char	szProxyServer[256];
}SViewerOptions;

//	This structure is used to group the parameters required for a download
//	operation
typedef struct
{
	LPUNKNOWN	lpUnknown;
	CString		strSource;
	CString		strCached;
	CString		strErrorMsg;
	CString		strStatus;
	BOOL		bError;
	BOOL		bComplete;
	BOOL		bAbort;
	BOOL		bRemote;
	ULONG		ulProgress;
	ULONG		ulProgressMax;
	HWND		hWnd;
	LPARAM		lParam;
}SDownload;

typedef struct
{
	LPBYTE		lpRequest;
	DWORD		dwLength;
	CString		strRequestHeader;
	CString		strResponseHeader;
	CString		strResponse;
	CString		strUrl;
	CString		strPageId;
	CString		strTreatmentId;
	DWORD		dwStatusCode;
	int			iAction;
}SPutTreatment;

typedef struct
{
	CString		strUrl;
	CString		strRequest;
	CString		strResponse;
	CString		strResponseHeader;
	CString		strTreatmentId;
	DWORD		dwStatusCode;
}SDeleteTreatment;

//	Forward declarations
class CTMXmlCtrl;
class CXmlSession;
class CHttpConnection;

class CXmlFrame : public CDialog
{
	private:

		SViewerOptions			m_ViewerOptions;
		SGraphicsOptions		m_ToolOptions;
		SPutTreatment			m_PutTreatment;
		SDeleteTreatment		m_DeleteTreatment;
		CIWebBrowser2*			m_pIBrowser;
		CPageBar				m_PageBar;
		CCells					m_BatchJob;
		CExtensions				m_Extensions;
		CXmlSettings*			m_pXmlSettings;
		CXmlActions				m_XmlActions;
		CXmlActions				m_ViewActions;
		CXmlActions				m_PrintActions;
		CXmlMediaTrees			m_MediaTrees;
		CXmlMediaTrees			m_PrintTrees;
		CProperties				m_Properties;
		CTemplates				m_Templates;
		CTMXmlCtrl*				m_pControl;
		CErrorHandler*			m_pErrors;
		CPrintProgress*			m_pPrintProgress;
		CIXMLDOMDocument*		m_pXmlDocument;
		CXmlMediaTree*			m_pXmlMediaTree;
		CXmlMedia*				m_pXmlMedia;
		CXmlPage*				m_pXmlPage;
		CXmlTreatment*			m_pXmlTreatment;
		CXmlAction*				m_pXmlPrintAction;
		CXmlMediaTree*			m_pXmlPrintTree;
		CXmlMedia*				m_pXmlPrintMedia;
		CXmlPage*				m_pXmlPrintPage;
		CXmlTreatment*			m_pXmlPrintTreatment;
		COLORREF				m_crBackground;
		CBrush*					m_pBackground;
		HCURSOR					m_hWaitCursor;
		HCURSOR					m_hStandardCursor;
		RECT					m_rcClient;
		RECT					m_rcViewer;
		RECT					m_rcTMTool;
		RECT					m_rcPrintProgress;
		RECT					m_rcPageBar;
		RECT					m_rcProgressBar;

		CMenu					m_PopupMenu;
		CMenu					m_ViewMenu;
		CMenu					m_RotateMenu;
		CMenu					m_EraseMenu;
		CMenu					m_TreatmentMenu;
		CMenu					m_PageMenu;
		CMenu					m_MediaMenu;
		CMenu					m_MediaTreeMenu;
		CMenu					m_PrintMenu;

		CMenu					m_ToolMenu;
		CMenu					m_ColorMenu;
		CMenu					m_DrawMenu;

		BOOL					m_bIsRemote;
		BOOL					m_bEmbedded;
		BOOL					m_bShowPrintProgress;
		BOOL					m_bIsSecure;
		int						m_iFilesPerPage;
		int						m_iViewer;
		long					m_lPrintPages;
		long					m_lPrintPage;
		CString					m_strAbsolute;
		CString					m_strRelative;
		CString					m_strGetXmlScript;
		CString					m_strPutTreatmentScript;
		CString					m_strDeleteTreatmentScript;
		CString					m_strSource;
		CString					m_strXmlFilename;
		CString					m_strIniFilename;
		CString					m_strPrintPage;
		CString					m_strLoading;

	public:
		
								CXmlFrame(CTMXmlCtrl* pControl, CErrorHandler* pErrors);
							   ~CXmlFrame();

		//	Public properties
		void					SetEmbedded(BOOL bEmbedded){ m_bEmbedded = bEmbedded; }
		void					SetFloatPrintProgress(BOOL bFloat);
		void					SetBackColor(COLORREF crColor);

		BOOL					GetEmbedded(){ return m_bEmbedded; }
		BOOL					GetFloatPrintProgress(){ return m_ViewerOptions.bFloatPrintProgress; }
		COLORREF				GetBackColor(){ return m_crBackground; }

		//	Public operations
		BOOL					Create();
		BOOL					LoadFile(LPCSTR lpSource);
		BOOL					OnPostRequest(LPCSTR lpUrl);
		void					OnDraw();

		//	Callback notification handlers
		void					OnPrintAbort();
		LONG					OnWMDownload(WPARAM wParam,LPARAM lParam);

		//	Page navigation window handlers
		void					OnPageBarClick(short sId, BOOL bChecked);
		void					OnPageBarChange(CXmlPage* pXmlPage);

		//	Ringtail custom methods
		short					jumpToPage(LPCTSTR lpszPageId); 
		short					loadDocument(LPCTSTR lpszUrl); 

	protected:

		void					RecalcLayout();
		void					Encode(CString& rUrl);
		void					BuildPopup();
		void					InitToolbar();
		void					InitPageBar();
		void					OnMenuCommand(int iCmd);
		void					OnTreatmentCommand(int iCmd);
		void					OnPageCommand(int iCmd);
		void					OnMediaCommand(int iCmd);
		void					OnMediaTreeCommand(int iCmd);
		void					OnViewCommand(int iCmd);
		void					OnViewError();
		void					OnDownloadComplete(LPARAM lParam);
		void					OnDownloadProgress(LPARAM lParam);
		void					OnRotateCommand(int iCmd);
		void					OnEraseCommand(int iCmd);
		void					OnPrintDocument();
		void					OnPrintSelections();
		void					OnToolCommand(int iCmd);
		void					OnDrawCommand(int iCmd);
		void					OnColorCommand(int iCmd);
		void					OnProperties();
		void					OnDiagnostics();
		void					OnPreferences(int iPage = 0);
		void					OnSaveTreatment();
		void					OnUpdateTreatment();
		void					OnDeleteTreatment();
		void					FreePutTreatment();
		void					SetWaitCursor(BOOL bWait);
		void					CheckMenuTool(int iCmd, BOOL bClearAll);
		void					CheckMenuColor(BOOL bClearAll);
		void					EnableMenuCommands();
		void					EnableMediaCommands();
		void					EnableMediaTreeCommands();
		void					EnablePageCommands();
		void					EnableTreatmentCommands();
		void					EnablePrintCommands();
		void					EnableViewerCommands();
		void					ExtractDrive(LPCSTR lpPath, CString& rDrive);
		void					ExtractFilename(LPCSTR lpPath, CString& rFilename);
		void					ExtractCasebook(LPCSTR lpPath, CString& rCasebook);
		void					StripFilename(CString& rPath);
		void					SetPaths(CString& rSource);
		void					ServiceToString(DWORD dwService, CString& rService);
		void					ReleaseAll();
		void					ReadIniFile();
		void					FillExtensions();
		void					WriteIniFile();
		void					ApplyOptions();
		void					ShowPrintProgress(BOOL bShow);
		void					GetResponseHeaders(LPCSTR lpRequest, CString& rResponse); 
		void					UpdateToolOptions();
		void					UpdateServerPage(CXmlPage* pPage);
		void					SetViewer(int iViewer);
		BOOL					SetXmlPage(CXmlPage* pXmlPage);
		CIXMLDOMDocument*		XMLAttach();
		BOOL					XMLLoadFile(LPCSTR lpFilename);
		BOOL					XMLError(CIXMLDOMDocument* pDocument);
		BOOL					XMLReadDocument(CIXMLDOMDocument* pDocument);
		BOOL					XMLReadTmx(CIXMLDOMNode* pNode);
		BOOL					XMLReadMediaTree(CIXMLDOMNode* pNode);
		BOOL					XMLReadMedia(CXmlMediaTree* pTree, CIXMLDOMNode* pNode);
		BOOL					XMLReadPage(CXmlMedia* pMedia, CIXMLDOMNode* pNode);
		BOOL					XMLReadTreatment(CXmlPage* pPage, CIXMLDOMNode* pNode,
												 CXmlTreatment* pUpdate = 0);
		BOOL					XMLRequest(LPCSTR lpTarget);
		BOOL					XMLReadActions(CIXMLDOMNode* pNode);
		BOOL					XMLReadAction(CIXMLDOMNode* pNode);
		BOOL					XMLReadSettings(CIXMLDOMNode* pNode);
		BOOL					XMLReadSetting(CIXMLDOMNode* pNode);
		BOOL					XMLProcessActions();
		BOOL					XMLProcessSettings();
		BOOL					XMLProcessView(CXmlAction* pAction);
		BOOL					XMLProcessPrint(CXmlAction* pAction);
		BOOL					XMLRunScript(LPCSTR lpUrl);
		BOOL					IsXMLFile(LPCSTR lpFilespec);
		BOOL					IsDirSeparator(char cChar);
		BOOL					FindFile(LPCSTR lpFilespec);
		BOOL					PrintMediaTree(CXmlMediaTree* pTree);
		BOOL					PrintBatch();
		BOOL					GetExplorer();
		BOOL					ExecuteJavascript(LPCSTR lpScript);
		BOOL					ViewPage(CXmlPage* pPage);
		BOOL					ViewMedia(CXmlMedia* pMedia);
		BOOL					ViewMediaTree(CXmlMediaTree* pTree);
		BOOL					ViewTreatment(CXmlTreatment* pTreatment);
		BOOL					GetNextPrintFile(void* lpPrevious);
		BOOL					GetNextPrintPage();
		BOOL					GetPutTreatment(LPCSTR lpFilename, LPCSTR lpPageId,
												int iAction, LPCSTR lpTreatmentId);
		BOOL					ExtractTreatmentId(CXmlTreatment* pTreatment);
		BOOL					ProcessPutTreatment();
		BOOL					PutTreatment();
		BOOL					GetFile(LPCSTR lpSource, CString& rFilename, 
										BOOL bShowProgress, LPARAM lParam);
		BOOL					Download(LPCSTR lpFilename, CString& rCached,
										 BOOL bShowErrors = TRUE, 
										 BOOL bShowProgress = TRUE,
										 LPARAM lParam = 0);
		int						AddPrintRequest(CXmlPage* pPage, LPCSTR lpPageFile, 
												CXmlTreatment* pTreatment, LPCSTR lpTreatmentFile);
		int						GetMenuToolCmd(int iDrawTool);
		int						GetMenuColorCmd(int iColorButton);
		int						GetViewerFromFile(LPCSTR lpszFilename);
		CTemplate*				GetDefaultTemplate();
		CXmlSession*			GetSession(); 
		CHttpConnection*		GetConnection(CXmlSession* pSession, LPCSTR lpServer);

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CXmlFrame)
	enum { IDD = IDD_FRAME };
	CTMTool	m_TMTool;
	CTm_view	m_TMView;
	CTMPrint	m_TMPrint;
	CTMBrowse	m_TMBrowse;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CXmlFrame)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CXmlFrame)
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	virtual BOOL OnInitDialog();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnEvButtonClick(short sId, BOOL bChecked);
	afx_msg void OnEvReconfigure();
	afx_msg void OnEvCloseTextBox(short sPane);
	afx_msg void OnEvViewClick(short Button, short Key);
	afx_msg void OnDestroy();
	afx_msg void OnEvBrowseComplete(LPCTSTR lpszFilename);
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__XMLFRAME_H__)
