//==============================================================================
//
// File Name:	frame.cpp
//
// Description:	This file contains member functions of the CXmlFrame class
//
// See Also:	frame.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-03-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <xmlframe.h>
#include <tmxml.h>
#include <tmxmdefs.h>
#include <tmtbdefs.h>
#include <tmvdefs.h>
#include <tmprdefs.h>
#include <afxinet.h>
#include <icall.h>
#include <propsht.h>
#include <download.h>
#include <wraphtml.h>
#include <diagnose.h>
#include <xmlinet.h>
#include <selprint.h>
#include <tmbrowsedef.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern CTMXmlApp NEAR	theApp;
SDownload				theDownload;

const char _szButtonMask[TMTB_MAXBUTTONS + 1] = 
{
'0',	// TMTB_CONFIG              
'0',	// TMTB_UNUSED1
'0',	// TMTB_CONFIGTOOLBARS      
'0',	// TMTB_CLEAR               
'1',	// TMTB_ROTATECW            
'1',	// TMTB_ROTATECCW           
'1',	// TMTB_NORMAL              
'1',	// TMTB_ZOOM                
'1',	// TMTB_ZOOMWIDTH           
'1',	// TMTB_PAN             
'1',	// TMTB_CALLOUT         
'1',	// TMTB_DRAWTOOL            
'1',	// TMTB_HIGHLIGHT           
'1',	// TMTB_REDACT              
'1',	// TMTB_ERASE               
'1',	// TMTB_FIRSTPAGE           
'1',	// TMTB_PREVPAGE            
'1',	// TMTB_NEXTPAGE            
'1',	// TMTB_LASTPAGE            
'1',	// TMTB_SAVEZAP         
'1',	// TMTB_FIRSTZAP            
'1',	// TMTB_PREVZAP         
'1',	// TMTB_NEXTZAP         
'1',	// TMTB_LASTZAP         
'0',	// TMTB_STARTMOVIE          
'0',	// TMTB_BACKMOVIE           
'0',	// TMTB_PAUSEMOVIE          
'0',	// TMTB_PLAYMOVIE           
'0',	// TMTB_FWDMOVIE            
'0',	// TMTB_ENDMOVIE            
'0',	// TMTB_FIRSTDESIGNATION    
'0',	// TMTB_BACKDESIGNATION 
'0',	// TMTB_PREVDESIGNATION 
'0',	// TMTB_STARTDESIGNATION    
'0',	// TMTB_PAUSEDESIGNATION    
'0',	// TMTB_PLAYDESIGNATION 
'0',	// TMTB_NEXTDESIGNATION 
'0',	// TMTB_FWDDESIGNATION      
'0',	// TMTB_LASTDESIGNATION 
'1',	// TMTB_PRINT               
'0',	// TMTB_SPLITPANE           
'0',	// TMTB_SINGLEPANE          
'0',	// TMTB_DISABLELINKS        
'0',	// TMTB_ENABLELINKS     
'0',	// TMTB_EXIT               
'1',	// TMTB_RED             
'1',	// TMTB_GREEN               
'1',	// TMTB_BLUE                
'1',	// TMTB_YELLOW              
'1',	// TMTB_BLACK               
'1',	// TMTB_WHITE               
'0',	// TMTB_PLAYTHROUGH               
'0',	// TMTB_CUEPGLNCURRENT               
'0',	// TMTB_CUEPGLNNEXT               
'1',	// TMTB_DELETEANN               
'1',	// TMTB_SELECT   
'0',	// TMTB_TEXT              
'0',	// TMTB_SELECTTOOL             
'1',	// TMTB_FREEHAND               
'1',	// TMTB_LINE              
'1',	// TMTB_ARROW               
'1',	// TMTB_ELLIPSE               
'1',	// TMTB_RECTANGLE               
'1',	// TMTB_FILLEDELLIPSE               
'1',	// TMTB_FILLEDRECTANGLE 
'0',	// TMTB_FULLSCREEN
'0',	// TMTB_STATUSBAR
'0',	// TMTB_SHADEDCALLOUTS
'1',	// TMTB_DARKRED             
'1',	// TMTB_DARKGREEN               
'1',	// TMTB_DARKBLUE                
'1',	// TMTB_LIGHTRED              
'1',	// TMTB_LIGHTGREEN              
'1',	// TMTB_LIGHTBLUE               
'1',	// TMTB_POLYLINE               
'1',	// TMTB_POLYGON 
'1',	// TMTB_ANNTEXT 
'1',	// TMTB_UPDATEZAP 
'1',	// TMTB_DELETEZAP
'1',	// TMTB_ZOOMRESTRICTED
0,		// NULL TERMINATION
};

short _aButtonMap[TMTB_MAXBUTTONS] = 
{
TMTB_NORMAL, 
TMTB_ZOOM,
TMTB_ZOOMRESTRICTED,
TMTB_ZOOMWIDTH, 
TMTB_ROTATECW, 
TMTB_ROTATECCW, 
TMTB_CALLOUT, 
TMTB_SELECT, 
TMTB_HIGHLIGHT, 
TMTB_DRAWTOOL, 
TMTB_FREEHAND,
TMTB_LINE,
TMTB_ARROW, 
TMTB_ELLIPSE, 
TMTB_RECTANGLE, 
TMTB_FILLEDELLIPSE, 
TMTB_FILLEDRECTANGLE, 
TMTB_PREVZAP, 
TMTB_NEXTZAP,  
TMTB_PREVPAGE,  
TMTB_NEXTPAGE,
-1, 
-1,
-1, 
-1,
-1, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1,
-1,
-1, 
-1, 
-1,
-1,
-1,  
-1,
-1,  
-1, 
-1, 
-1, 
-1,
-1,
-1, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1,       
-1,          
-1,       
-1,     
-1,
-1,
-1,    
-1,    
-1,
-1, 
-1,   
-1,     
-1,         
-1,
-1,
-1,
-1,
-1,  
-1,     
-1,     
-1,   
-1,     
-1,     
-1,    
-1,
-1,
};

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CXmlFrame, CDialog)
	//{{AFX_MSG_MAP(CXmlFrame)
	ON_WM_CTLCOLOR()
	ON_WM_SIZE()
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
	ON_MESSAGE(WM_DOWNLOAD, OnWMDownload)
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CXmlFrame, CDialog)
    //{{AFX_EVENTSINK_MAP(CXmlFrame)
	ON_EVENT(CXmlFrame, IDC_TMTOOL, 1 /* ButtonClick */, OnEvButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CXmlFrame, IDC_TMTOOL, 2 /* Reconfigure */, OnEvReconfigure, VTS_NONE)
	ON_EVENT(CXmlFrame, IDC_TMVIEW, 7 /* CloseTextBox */, OnEvCloseTextBox, VTS_I2)
	ON_EVENT(CXmlFrame, IDC_TMVIEW, 1 /* MouseClick */, OnEvViewClick, VTS_I2 VTS_I2)
	ON_EVENT(CXmlFrame, IDC_TMBROWSE, 1 /* LoadComplete */, OnEvBrowseComplete, VTS_BSTR)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

//==============================================================================
//
// 	Function Name:	DownloadThreadProc()
//
// 	Description:	This is the thread service procedure used to download
//					files from a remote server
//
// 	Returns:		Zero
//
//	Notes:			None
//
//==============================================================================
UINT DownloadThreadProc(void* pFrame)
{
	char		szCache[1024];
	CICallback	ICallback;
	HRESULT		hResult;
	LPTSTR		lpszErrorMsg;

	//	Download the file from the remote server
	hResult = URLDownloadToCacheFile(theDownload.lpUnknown,
									 theDownload.strSource, 
									 szCache, sizeof(szCache),
									 0, &ICallback);
	
	//	Was the operation successful?
	if(SUCCEEDED(hResult))
	{
		theDownload.strCached = szCache;
		theDownload.strErrorMsg.Empty();
		theDownload.bError = FALSE;
		theDownload.bComplete = TRUE;
	}
	else
	{
		theDownload.strCached.Empty();
		theDownload.bError = TRUE;
		theDownload.bComplete = TRUE;

		//	Construct the error message
		theDownload.strErrorMsg.Format("Unable to download %s: Error code: 0x%08lX",
									   theDownload.strSource, (DWORD)hResult);
		if(FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | 
						 FORMAT_MESSAGE_FROM_SYSTEM | 
                         FORMAT_MESSAGE_IGNORE_INSERTS, NULL, hResult,
						 MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
						 (LPTSTR)&lpszErrorMsg, 0, NULL))
		{
			theDownload.strErrorMsg += "\r\n\r\n";

			if(lstrlen(lpszErrorMsg) > 0)
			{
				theDownload.strErrorMsg += "< ";
				theDownload.strErrorMsg += lpszErrorMsg;
				theDownload.strErrorMsg += " >";
			}
			else
			{
				theDownload.strErrorMsg += "No message text available";
			}

			LocalFree(lpszErrorMsg);
		}
		else
		{
			theDownload.strErrorMsg += "\r\n\r\nNo message text available";
		}

	}

	//	Do we need to send a message to the frame window?
	if((theDownload.hWnd != 0) && IsWindow(theDownload.hWnd))
	{
		PostMessage(theDownload.hWnd, WM_DOWNLOAD,  
					TMXML_DOWNLOAD_COMPLETE, theDownload.lParam);
	}

    return 0;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::SetXmlPage()
//
// 	Description:	This function is called to set the viewer to the specified
//					Xml page
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::SetXmlPage(CXmlPage* pXmlPage) 
{
	//	Make sure the specified page is in the current media object's
	//	list of pages
	if((m_pXmlMedia == 0) || (m_pXmlMedia->m_Pages.Find(pXmlPage) == NULL))
		return FALSE;

	//	Load the page into the viewer
	if(ViewPage(pXmlPage))
	{
		//	Set the page iterator to the correct position
		m_pXmlMedia->m_Pages.SetPosition(pXmlPage);
		
		//	Update the menu commands
		EnableMenuCommands();
	
		return TRUE;
	}
	else
	{
		//	Make sure everything is still in sync
		OnViewError();

		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::AddPrintRequest()
//
// 	Description:	This function will add a request for the specified page
//					and/or treatment to the TMPrint control's queue.
//
// 	Returns:		The number of requests currently in the queue
//
//	Notes:			None
//
//==============================================================================
int CXmlFrame::AddPrintRequest(CXmlPage* pPage, LPCSTR lpPageFile, 
							   CXmlTreatment* pTreatment, LPCSTR lpTreatmentFile)
{
	CString	strParam;
	CString	strCell;
	CString	strPath;
	CString	strFilename;
	BOOL	bTreatment;
	CCell*	pCell;

	ASSERT(pPage);
	ASSERT(lpPageFile);
	ASSERT(pPage->m_pXmlMedia);

	//	Is this a treatment?
	if((pTreatment != 0) && (lpTreatmentFile != 0) && (lstrlen(lpTreatmentFile) > 0))
		bTreatment = TRUE;
	else
		bTreatment = FALSE;

	//	Format the identifiers
	//
	//	NOTE:	We need a non-zero tertiary id to indicate that we want to 
	//			print a treatment
	strCell.Format("%s=%s|%s=%s|%s=%s",
				   PRIMARY_LABEL, pPage->m_pXmlMedia->m_strId,
				   SECONDARY_LABEL, pPage->m_strId,
				   TERTIARY_LABEL, bTreatment ? pTreatment->m_strId : "0");

	//	Add the type identifier
	strParam.Format("|%s=%c", TYPE_LABEL, CELL_TYPECHAR_DOCUMENT);
	strCell += strParam;

	//	We have to break the file specification into path - filename
	if(bTreatment)
	{
		strPath = lpTreatmentFile;
	}
	else
	{
		strPath = lpPageFile;
	}
	ExtractFilename(strPath, strFilename);
	StripFilename(strPath);

	//	Add the page filename and path parameters
	strParam.Format("|%s=%s|%s=%s", 
					PATH_LABEL, strPath,
					FILENAME_LABEL, strFilename);
	strCell += strParam;

	//	Do we need to add a treatment source image path?
	if(bTreatment)
	{
		strParam.Format("|%s=%s", TREATMENTIMAGE_LABEL, lpPageFile);
		strCell += strParam;
	}

	//	Allocate a new cell and add it to the batch job
	pCell = new CCell();
	pCell->m_strString = strCell;
	m_BatchJob.Add(pCell);

	//	Display the file if the viewer is not in use
	if(m_pXmlPage == 0)
	{
		if(bTreatment)
			m_TMView.LoadZap(lpTreatmentFile, TRUE, TRUE, FALSE, -1, lpPageFile);
		else
			m_TMView.LoadFile(lpPageFile, -1);
	}

	//	Return the number of requests in the batch job
	return m_BatchJob.GetCount();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ApplyOptions()
//
// 	Description:	This function resets the viewer using the currently defined
//					options.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::ApplyOptions()
{
	CTemplate* pTemplate;

	//	Set the error handlers
	if(m_pErrors)
		m_pErrors->Enable(m_ViewerOptions.bEnableErrors);
	m_TMTool.SetEnableErrors(m_ViewerOptions.bEnableErrors);
	m_TMView.SetEnableErrors(m_ViewerOptions.bEnableErrors);
	m_TMPrint.SetEnableErrors(m_ViewerOptions.bEnableErrors);

	//	Set the color depth used for realizing annotations when
	//	the source image is black/white
	if(m_ViewerOptions.bMinimizeColorDepth)
		m_TMView.SetAnnColorDepth(8);
	else
		m_TMView.SetAnnColorDepth(16);

	m_TMView.SetResizeCallouts(m_ToolOptions.bResizableCallouts);
	m_TMView.SetAnnColor(m_ToolOptions.sAnnColor);
	m_TMView.SetHighlightColor(m_ToolOptions.sHighlightColor);
	m_TMView.SetRedactColor(m_ToolOptions.sRedactColor);
	m_TMView.SetCalloutColor(m_ToolOptions.sCalloutColor);
	m_TMView.SetCalloutFrameColor(m_ToolOptions.sCalloutFrameColor);
	m_TMView.SetCalloutHandleColor(m_ToolOptions.sCalloutHandleColor);
	m_TMView.SetAnnThickness(m_ToolOptions.sAnnThickness);
	m_TMView.SetCalloutFrameThickness(m_ToolOptions.sCalloutFrameThickness);
	m_TMView.SetMaxZoom(m_ToolOptions.sMaxZoom);
	m_TMView.SetBitonalScaling(m_ToolOptions.sBitonalScaling);
	m_TMView.SetAnnTool(m_ToolOptions.sAnnTool);
	m_TMView.SetAnnFontSize(m_ToolOptions.sAnnFontSize);
	m_TMView.SetAnnFontBold(m_ToolOptions.bAnnFontBold);
	m_TMView.SetAnnFontStrikeThrough(m_ToolOptions.bAnnFontStrikeThrough);
	m_TMView.SetAnnFontUnderline(m_ToolOptions.bAnnFontUnderline);
	m_TMView.SetAnnFontName(m_ToolOptions.strAnnFontName);

	//	Set the printer selection
	m_TMView.SetPrinterByName(m_ViewerOptions.szPrinter);

	//	Reset the session information and the TMPrint control
	//
	//	NOTE:	If the TMView is unable to use the specified printer it
	//			will retain the name of the last used printer
	theApp.m_strPrinter = m_TMView.GetCurrentPrinter();
	m_TMPrint.SetPrinter(theApp.m_strPrinter);
	lstrcpyn(m_ViewerOptions.szPrinter, theApp.m_strPrinter, sizeof(m_ViewerOptions.szPrinter));

	//	Set the active batch printing template
	if(lstrlen(m_ViewerOptions.szTemplate) > 0)
	{
		//	Locate the template specified by the user
		pTemplate = m_Templates.GetFirstTemplate();
		while(pTemplate != 0)
		{
			if(lstrcmpi(m_ViewerOptions.szTemplate, pTemplate->m_strDescription) == 0)
				break;
			else
				pTemplate = m_Templates.GetNextTemplate();
		}
	}
	else
	{
		pTemplate = 0;
	}

	//	Get the first in the list if none have been defined
	if(pTemplate == 0)
		pTemplate = m_Templates.GetFirstTemplate();

	//	Set the active template for batch printing
	m_TMPrint.SetPrintTemplate((long)pTemplate);
	if(pTemplate)
	{
		//	Override the TMPrint property values with the template options
		m_TMPrint.SetPrintImage(pTemplate->m_bPrintImage);
		m_TMPrint.SetPrintBarcodeText(pTemplate->GetPrintEnabled(TEMPLATE_BARCODE));
		m_TMPrint.SetPrintBarcodeGraphic(pTemplate->GetPrintEnabled(TEMPLATE_GRAPHIC));
		m_TMPrint.SetPrintFileName(pTemplate->GetPrintEnabled(TEMPLATE_FILENAME));
		m_TMPrint.SetPrintName(pTemplate->GetPrintEnabled(TEMPLATE_NAME));
		m_TMPrint.SetPrintPageNumber(pTemplate->GetPrintEnabled(TEMPLATE_PAGENUM));
		m_TMPrint.SetPrintCellBorder(pTemplate->m_bPrintBorder);
		m_TMPrint.SetIncludePathInFileName(pTemplate->m_bPrintFullPath);
		m_TMPrint.SetIncludePageTotal(pTemplate->m_bPageAsSeries);
		m_TMPrint.SetPrintDeponent(pTemplate->GetPrintEnabled(TEMPLATE_DEPONENT));

		lstrcpyn(m_ViewerOptions.szTemplate, pTemplate->m_strDescription,
				 sizeof(m_ViewerOptions.szTemplate));
	}
	else
	{
		memset(m_ViewerOptions.szTemplate, 0, sizeof(m_ViewerOptions.szTemplate));
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::BuildPopup()
//
// 	Description:	This function constructs the popup menu
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::BuildPopup() 
{
	//	Create the media submenu
	m_MediaTreeMenu.CreatePopupMenu();
	m_MediaTreeMenu.AppendMenu(MF_STRING, TMXML_MEDIATREES_FIRST, "&First");
	m_MediaTreeMenu.AppendMenu(MF_STRING, TMXML_MEDIATREES_PREVIOUS, "&Previous");
	m_MediaTreeMenu.AppendMenu(MF_STRING, TMXML_MEDIATREES_NEXT, "&Next");
	m_MediaTreeMenu.AppendMenu(MF_STRING, TMXML_MEDIATREES_LAST, "&Last");

	//	Create the media submenu
	m_MediaMenu.CreatePopupMenu();
	m_MediaMenu.AppendMenu(MF_STRING, TMXML_MEDIA_FIRST, "&First");
	m_MediaMenu.AppendMenu(MF_STRING, TMXML_MEDIA_PREVIOUS, "&Previous");
	m_MediaMenu.AppendMenu(MF_STRING, TMXML_MEDIA_NEXT, "&Next");
	m_MediaMenu.AppendMenu(MF_STRING, TMXML_MEDIA_LAST, "&Last");

	//	Create the page submenu
	m_PageMenu.CreatePopupMenu();
	m_PageMenu.AppendMenu(MF_STRING, TMXML_PAGES_FIRST, "&First");
	m_PageMenu.AppendMenu(MF_STRING, TMXML_PAGES_PREVIOUS, "&Previous");
	m_PageMenu.AppendMenu(MF_STRING, TMXML_PAGES_NEXT, "&Next");
	m_PageMenu.AppendMenu(MF_STRING, TMXML_PAGES_LAST, "&Last");

	//	Create the treatment submenu
	m_TreatmentMenu.CreatePopupMenu();
	m_TreatmentMenu.AppendMenu(MF_STRING, TMXML_TREATMENTS_FIRST, "&First");
	m_TreatmentMenu.AppendMenu(MF_STRING, TMXML_TREATMENTS_PREVIOUS, "&Previous");
	m_TreatmentMenu.AppendMenu(MF_STRING, TMXML_TREATMENTS_NEXT, "&Next");
	m_TreatmentMenu.AppendMenu(MF_STRING, TMXML_TREATMENTS_LAST, "&Last");
	m_TreatmentMenu.AppendMenu(MF_SEPARATOR, 0);
	m_TreatmentMenu.AppendMenu(MF_STRING, TMXML_TREATMENTS_SAVE, "&Add");
	m_TreatmentMenu.AppendMenu(MF_STRING, TMXML_TREATMENTS_UPDATE, "&Update");
	m_TreatmentMenu.AppendMenu(MF_STRING, TMXML_TREATMENTS_DELETE, "&Delete");

	//	Create the view submenu
	m_ViewMenu.CreatePopupMenu();
	m_ViewMenu.AppendMenu(MF_STRING, TMXML_VIEW_NORMAL, "&Normal");
	m_ViewMenu.AppendMenu(MF_STRING, TMXML_VIEW_FULLWIDTH, "&Full Width");

	//	Create the rotate submenu
	m_RotateMenu.CreatePopupMenu();
	m_RotateMenu.AppendMenu(MF_STRING, TMXML_ROTATE_CW, "&Clockwise");
	m_RotateMenu.AppendMenu(MF_STRING, TMXML_ROTATE_CCW, "C&ounter Clockwise");

	//	Create the erase submenu
	m_EraseMenu.CreatePopupMenu();
	m_EraseMenu.AppendMenu(MF_STRING, TMXML_ERASE_ALL, "&All");
	m_EraseMenu.AppendMenu(MF_STRING, TMXML_ERASE_SELECTIONS, "&Selections");

	//	Create the print submenu
	m_PrintMenu.CreatePopupMenu();
	m_PrintMenu.AppendMenu(MF_STRING, TMXML_PRINT_CURRENT, "&Current");
	m_PrintMenu.AppendMenu(MF_SEPARATOR, 0);
	m_PrintMenu.AppendMenu(MF_STRING, TMXML_PRINT_DOCUMENT, "&Document");
	m_PrintMenu.AppendMenu(MF_STRING, TMXML_PRINT_SELECTIONS, "&Selections ...");
	m_PrintMenu.AppendMenu(MF_SEPARATOR, 0);
	m_PrintMenu.AppendMenu(MF_STRING, TMXML_PRINT_SETUP, "Se&tup ...");

	//	Create the draw menu
	m_DrawMenu.CreatePopupMenu();
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_FREEHAND, "&Freehand");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_LINE, "&Line");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_ARROW, "&Arrow");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_ELLIPSE, "&Ellipse");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_RECTANGLE, "&Rectangle");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_FILLEDELLIPSE, "F&illed Ellipse");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_FILLEDRECTANGLE, "Fi&lled Rectangle");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_POLYLINE, "&Polyline");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_POLYGON, "P&olygon");
	m_DrawMenu.AppendMenu(MF_STRING, TMXML_DRAW_TEXT, "&Text");

	//	Create the tool submenu
	//
	//	NOTE:	This menu can not be created until the Draw submenu is created
	m_ToolMenu.CreatePopupMenu();
	m_ToolMenu.AppendMenu(MF_STRING, TMXML_TOOL_ZOOM, "&Zoom");
	m_ToolMenu.AppendMenu(MF_STRING, TMXML_TOOL_ZOOM_RESTRICTED, "Z&oom Restricted");
	m_ToolMenu.AppendMenu(MF_STRING, TMXML_TOOL_PAN, "&Pan");
	m_ToolMenu.AppendMenu(MF_STRING | MF_POPUP, (UINT)m_DrawMenu.GetSafeHmenu(), "&Draw");
	m_ToolMenu.AppendMenu(MF_STRING, TMXML_TOOL_HIGHLIGHT, "&Highlight");
	m_ToolMenu.AppendMenu(MF_STRING, TMXML_TOOL_REDACT, "&Redact");
	m_ToolMenu.AppendMenu(MF_STRING, TMXML_TOOL_CALLOUT, "&Callout");
	m_ToolMenu.AppendMenu(MF_STRING, TMXML_TOOL_SELECT, "&Select");
	
	//	Create the color submenu
	m_ColorMenu.CreatePopupMenu();
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_BLACK, "&Black");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_RED, "&Red");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_GREEN, "&Green");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_BLUE, "Bl&ue");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_YELLOW, "&Yellow");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_MAGENTA, "&Magenta");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_CYAN, "&Cyan");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_GREY, "Gre&y");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_WHITE, "&White");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_DARKRED, "&Dark Red");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_DARKGREEN, "D&ark Green");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_DARKBLUE, "Dar&k Blue");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_LIGHTRED, "&Light Red");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_LIGHTGREEN, "Lig&ht Green");
	m_ColorMenu.AppendMenu(MF_STRING, TMXML_COLOR_LIGHTBLUE, "Ligh&t Blue");
	
	//	Create the popup menu
	m_PopupMenu.CreatePopupMenu();

	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_MediaTreeMenu.GetSafeHmenu(), "&Media Trees");
	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_MediaMenu.GetSafeHmenu(), "M&edia");
	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_PageMenu.GetSafeHmenu(), "&Pages");
	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_TreatmentMenu.GetSafeHmenu(), "&Treatments");
	m_PopupMenu.AppendMenu(MF_SEPARATOR, 0);

	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_ViewMenu.GetSafeHmenu(), "&View");
	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_RotateMenu.GetSafeHmenu(), "&Rotate");
	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_EraseMenu.GetSafeHmenu(), "&Erase");
	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_PrintMenu.GetSafeHmenu(), "&Print");

	m_PopupMenu.AppendMenu(MF_SEPARATOR, 0);
	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_ToolMenu.GetSafeHmenu(), "T&ools");
	m_PopupMenu.AppendMenu(MF_POPUP | MF_STRING, (UINT)m_ColorMenu.GetSafeHmenu(), "&Colors");

	m_PopupMenu.AppendMenu(MF_SEPARATOR, 0);
	m_PopupMenu.AppendMenu(MF_STRING, TMXML_POPUP_PREFERENCES, "Pre&ferences ...");
	m_PopupMenu.AppendMenu(MF_STRING, TMXML_POPUP_PROPERTIES, "Propert&ies ...");
	
	//	Should we add a diagnostics option?
	if(m_ViewerOptions.bDiagnostics)
	{
		m_PopupMenu.AppendMenu(MF_SEPARATOR, 0);
		m_PopupMenu.AppendMenu(MF_STRING, TMXML_POPUP_DIAGNOSTICS, "&Diagnostics ...");
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::CheckMenuColor()
//
// 	Description:	This function is called to set the menu check state of the
//					color associated with the current annotation tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::CheckMenuColor(BOOL bClearAll) 
{
	//	Clear the current selection
	for(int i = TMXML_COLOR_BLACK; i <= TMXML_COLOR_LIGHTBLUE; i++)
		m_ColorMenu.CheckMenuItem(i, MF_UNCHECKED | MF_BYCOMMAND);

	if(IsWindow(m_TMView.m_hWnd) && !bClearAll)
	{
		//	What is the TMView color associated with the current tool?
		switch(m_TMView.GetColor())
		{
			case TMV_BLACK:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_BLACK, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_BLACK);
				break;

			case TMV_RED:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_RED, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_RED);
				break;

			case TMV_GREEN:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_GREEN, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_GREEN);
				break;

			case TMV_BLUE:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_BLUE, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_BLUE);
				break;

			case TMV_YELLOW:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_YELLOW, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_YELLOW);
				break;

			case TMV_MAGENTA:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_MAGENTA, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(-1);
				break;

			case TMV_CYAN:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_CYAN, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(-1);
				break;

			case TMV_GREY:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_GREY, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(-1);
				break;

			case TMV_WHITE:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_WHITE, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_WHITE);
				break;

			case TMV_LIGHTRED:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_LIGHTRED, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_LIGHTRED);
				break;

			case TMV_LIGHTGREEN:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_LIGHTGREEN, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_LIGHTGREEN);
				break;

			case TMV_LIGHTBLUE:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_LIGHTBLUE, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_LIGHTBLUE);
				break;

			case TMV_DARKRED:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_DARKRED, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_DARKRED);
				break;

			case TMV_DARKGREEN:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_DARKGREEN, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_DARKGREEN);
				break;

			case TMV_DARKBLUE:

				m_ColorMenu.CheckMenuItem(TMXML_COLOR_DARKBLUE, MF_CHECKED | MF_BYCOMMAND);
				m_TMTool.SetColorButton(TMTB_DARKBLUE);
				break;

		}

	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::CheckMenuTool()
//
// 	Description:	This function is called to set the menu check state of the
//					specified annotation/drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::CheckMenuTool(int iCmd, BOOL bClearAll) 
{
	//	Clear the current selection
	m_ToolMenu.CheckMenuItem(TMXML_TOOL_ZOOM, MF_UNCHECKED | MF_BYCOMMAND);
	m_ToolMenu.CheckMenuItem(TMXML_TOOL_ZOOM_RESTRICTED, MF_UNCHECKED | MF_BYCOMMAND);
	m_ToolMenu.CheckMenuItem(TMXML_TOOL_PAN, MF_UNCHECKED | MF_BYCOMMAND);
	m_ToolMenu.CheckMenuItem(2, MF_UNCHECKED | MF_BYPOSITION); // Draw submenu
	m_ToolMenu.CheckMenuItem(TMXML_TOOL_HIGHLIGHT, MF_UNCHECKED | MF_BYCOMMAND);
	m_ToolMenu.CheckMenuItem(TMXML_TOOL_REDACT, MF_UNCHECKED | MF_BYCOMMAND);
	m_ToolMenu.CheckMenuItem(TMXML_TOOL_CALLOUT, MF_UNCHECKED | MF_BYCOMMAND);
	m_ToolMenu.CheckMenuItem(TMXML_TOOL_SELECT, MF_UNCHECKED | MF_BYCOMMAND);

	m_DrawMenu.CheckMenuItem(TMXML_DRAW_FREEHAND, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_LINE, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_ARROW, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_ELLIPSE, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_RECTANGLE, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_FILLEDELLIPSE, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_FILLEDRECTANGLE, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_POLYLINE, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_POLYGON, MF_UNCHECKED | MF_BYCOMMAND);
	m_DrawMenu.CheckMenuItem(TMXML_DRAW_TEXT, MF_UNCHECKED | MF_BYCOMMAND);

	//	Check the requested selection
	if(!bClearAll)
	{
		if(iCmd < TMXML_DRAW_FREEHAND)
		{
			m_ToolMenu.CheckMenuItem(iCmd, MF_CHECKED | MF_BYCOMMAND);
		}
		else
		{
			m_ToolMenu.CheckMenuItem(2, MF_CHECKED | MF_BYPOSITION);
			m_DrawMenu.CheckMenuItem(iCmd, MF_CHECKED | MF_BYCOMMAND);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::Create()
//
// 	Description:	This is an overloaded version of the base class member. It
//					will create the frame window.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::Create() 
{
	ASSERT(m_pControl);
	if(!m_pControl) return FALSE;

	//	Create the dialog box
	return CDialog::Create(CXmlFrame::IDD, m_pControl);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::CXmlFrame()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlFrame::CXmlFrame(CTMXmlCtrl* pControl, CErrorHandler* pErrors)
		  :CDialog(CXmlFrame::IDD, pControl)
{
	//{{AFX_DATA_INIT(CXmlFrame)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	m_pControl = pControl;
	m_pErrors  = pErrors;
	m_pIBrowser = 0;
	m_pPrintProgress = 0;
	m_pXmlSettings = 0;
	m_pXmlDocument = 0;
	m_pXmlTreatment = 0;
	m_pXmlPage = 0;
	m_pXmlMedia = 0;
	m_pXmlMediaTree = 0;
	m_pXmlPrintAction = 0;
	m_pXmlPrintTree = 0;
	m_pXmlPrintTreatment = 0;
	m_pXmlPrintPage = 0;
	m_pXmlPrintMedia = 0;
	m_iFilesPerPage = 0;
	m_iViewer = TMXML_VIEWER_UNKNOWN;
	m_lPrintPages = 0;
	m_lPrintPage = 0;
	m_bIsRemote = FALSE;
	m_bShowPrintProgress = FALSE;
	m_bEmbedded = FALSE;
	m_bIsSecure = FALSE;
	m_hWaitCursor = 0;
	m_hStandardCursor = 0;
	m_strSource.Empty();
	m_strRelative.Empty();
	m_strAbsolute.Empty();
	m_strGetXmlScript.Empty();
	m_strPutTreatmentScript.Empty();
	m_strDeleteTreatmentScript.Empty();
	m_strXmlFilename.Empty();
	m_strPrintPage.Empty();
	m_strLoading.Empty();
	ZeroMemory(&m_rcTMTool, sizeof(m_rcTMTool));
	ZeroMemory(&m_rcViewer, sizeof(m_rcViewer));
	ZeroMemory(&m_rcClient, sizeof(m_rcClient));
	ZeroMemory(&m_rcProgressBar, sizeof(m_rcProgressBar));
	ZeroMemory(&m_rcPageBar, sizeof(m_rcPageBar));
	ZeroMemory(&m_rcPrintProgress, sizeof(m_rcPrintProgress));
	ZeroMemory(&m_ViewerOptions, sizeof(m_ViewerOptions));
	
	m_PutTreatment.lpRequest = 0;
	m_PutTreatment.dwLength  = 0;
	m_PutTreatment.dwStatusCode = 0;
	m_PutTreatment.iAction = 0;
	m_PutTreatment.strRequestHeader.Empty();
	m_PutTreatment.strResponse.Empty();
	m_PutTreatment.strResponseHeader.Empty();
	m_PutTreatment.strUrl.Empty();
	m_PutTreatment.strPageId.Empty();
	m_PutTreatment.strTreatmentId.Empty();

	m_DeleteTreatment.dwStatusCode = 0;
	m_DeleteTreatment.strUrl.Empty();
	m_DeleteTreatment.strRequest.Empty();
	m_DeleteTreatment.strResponseHeader.Empty();
	m_DeleteTreatment.strResponse.Empty();
	m_DeleteTreatment.strTreatmentId.Empty();

	//	Initialize the background brush
	m_crBackground = RGB(0,0,0);
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(m_crBackground);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::~CXmlFrame()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlFrame::~CXmlFrame()
{
	//	Free all memory
	ReleaseAll();

	//	Deallocate the print progress window
	if(m_pPrintProgress)
		delete m_pPrintProgress;

	//	Flush the list of print templates
	m_Templates.Flush(TRUE);
	m_BatchJob.Flush(TRUE);

	//	Flush the list of file extensions
	m_Extensions.Flush(TRUE);

	//	Release the Internet Explorer interfaces
	DELETE_INTERFACE(m_pIBrowser);

	FreePutTreatment();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and the associated class members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CXmlFrame)
	DDX_Control(pDX, IDC_TMTOOL, m_TMTool);
	DDX_Control(pDX, IDC_TMVIEW, m_TMView);
	DDX_Control(pDX, IDC_TMPRINT, m_TMPrint);
	DDX_Control(pDX, IDC_TMBROWSE, m_TMBrowse);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::Download()
//
// 	Description:	This function is called to download the specified file to
//					the local cache.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::Download(LPCSTR lpFilename, CString& rCached, 
						 BOOL bShowErrors, BOOL bShowProgress, LPARAM lParam)
{
	CWinThread* pThread;
	CDownload*	pProgress = 0;
	DWORD		dwStartTime;
	DWORD		dwDelay = (DWORD)(m_ViewerOptions.fProgressDelay * 1000.0);
	DWORD		dwElapsed;
	MSG			Msg;

    //	Create the worker thread. Do actually start the thread until we 
	//	initialize all the download parameters
    pThread = AfxBeginThread(DownloadThreadProc, this,
                             THREAD_PRIORITY_NORMAL, 0, CREATE_SUSPENDED);
	if(pThread == NULL)
	{
		return FALSE;
	}

	//	Set up the parameters for the download operation
	theDownload.lpUnknown = m_pControl->GetControllingUnknown();
	theDownload.strSource = lpFilename;
	theDownload.strCached.Empty();
	theDownload.strErrorMsg.Empty();
	theDownload.bError = FALSE;
	theDownload.bComplete = FALSE;
	theDownload.bAbort = FALSE;
	theDownload.bRemote = TRUE;
	theDownload.ulProgress = 0;
	theDownload.ulProgressMax = 0;

	//	Initialize the status message
	theDownload.strStatus.Format("Waiting For: %s", lpFilename);

	//	Does the caller want to send a message on completion instead of waiting?
	if(lParam > 0)
	{
		theDownload.hWnd   = m_hWnd;
		theDownload.lParam = lParam;
	}
	else
	{
		theDownload.hWnd   = 0;
		theDownload.lParam = 0;
	}

	//	Start the download
	pThread->ResumeThread();
	dwStartTime = GetTickCount();

	//	Stop here if the caller wants a message sent on completion
	if(theDownload.hWnd != 0)
		return TRUE;
	
	if(bShowProgress)
		SetWaitCursor(TRUE);

	//	Wait for the download to complete
	while(1)
	{
		//	Is the download complete?
		if(theDownload.bComplete)
			break;
		
		//	How much time has elapsed?
		dwElapsed = GetTickCount() - dwStartTime;

		//	Is it time to create the progress dialog
		if(bShowProgress && (dwElapsed >= dwDelay) && (pProgress == 0))
		{
			pProgress = new CDownload(this);
		}

		//	Process all pending messages for this thread
		while(::PeekMessage(&Msg, NULL, 0, 0, PM_NOREMOVE))
		{
			AfxGetThread()->PumpMessage();
		}
	
		//	Update the progress
		if(pProgress)
		{
			pProgress->Update(theDownload.ulProgress, 
							  theDownload.ulProgressMax, 
							  theDownload.strStatus);

			//	Does the user want to abort?
			theDownload.bAbort = pProgress->GetAborted();
		}
		
		//	Introduce some delay
		Sleep(200);

	}

	//	Set the cached filename
	rCached = theDownload.strCached;

	//	Delete the progress dialog
	if(pProgress)
		delete pProgress;

	//	Do we need to display an error message?
	if(!theDownload.bAbort && theDownload.bError && m_pErrors && bShowErrors)
		m_pErrors->Handle(0, theDownload.strErrorMsg);

	if(bShowProgress)
		SetWaitCursor(FALSE);

	return (theDownload.bError == FALSE);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::EnableMediaCommands()
//
// 	Description:	This function is called to enable/disable the commands 
//					used to iterate the media objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::EnableMediaCommands() 
{
	//	Do we have a valid list of media objects?
	if((m_pXmlMediaTree != 0) && (m_pXmlMediaTree->GetCount() > 1))
	{
		m_PopupMenu.EnableMenuItem(1, MF_ENABLED | MF_BYPOSITION);

		//	Are we on the first media object?
		if((m_pXmlMedia != 0) && (m_pXmlMediaTree->IsFirst(m_pXmlMedia) == TRUE))
		{
			m_MediaMenu.EnableMenuItem(TMXML_MEDIA_FIRST, MENU_DISABLED);
			m_MediaMenu.EnableMenuItem(TMXML_MEDIA_PREVIOUS, MENU_DISABLED);
		}
		else
		{
			m_MediaMenu.EnableMenuItem(TMXML_MEDIA_FIRST, MENU_ENABLED);
			m_MediaMenu.EnableMenuItem(TMXML_MEDIA_PREVIOUS, MENU_ENABLED);
		}

		//	Are we on the last media object?
		if((m_pXmlMedia != 0) && (m_pXmlMediaTree->IsLast(m_pXmlMedia) == TRUE))
		{
			m_MediaMenu.EnableMenuItem(TMXML_MEDIA_LAST, MENU_DISABLED);
			m_MediaMenu.EnableMenuItem(TMXML_MEDIA_NEXT, MENU_DISABLED);
		}
		else
		{
			m_MediaMenu.EnableMenuItem(TMXML_MEDIA_LAST, MENU_ENABLED);
			m_MediaMenu.EnableMenuItem(TMXML_MEDIA_NEXT, MENU_ENABLED);
		}
	}
	else
	{
		m_PopupMenu.EnableMenuItem(1, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);

		m_MediaMenu.EnableMenuItem(TMXML_MEDIA_NEXT, MENU_DISABLED);
		m_MediaMenu.EnableMenuItem(TMXML_MEDIA_PREVIOUS, MENU_DISABLED);
		m_MediaMenu.EnableMenuItem(TMXML_MEDIA_FIRST, MENU_DISABLED);
		m_MediaMenu.EnableMenuItem(TMXML_MEDIA_LAST, MENU_DISABLED);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::EnableMediaTreeCommands()
//
// 	Description:	This function is called to enable/disable the commands 
//					used to iterate the media trees
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::EnableMediaTreeCommands() 
{
	//	Do we have a valid list of media trees?
	if(m_MediaTrees.GetCount() > 1)
	{
		m_PopupMenu.EnableMenuItem(0, MF_ENABLED | MF_BYPOSITION);

		//	Are we on the first media tree?
		if((m_pXmlMediaTree != 0) && (m_MediaTrees.IsFirst(m_pXmlMediaTree) == TRUE))
		{
			m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_FIRST, MENU_DISABLED);
			m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_PREVIOUS, MENU_DISABLED);
		}
		else
		{
			m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_FIRST, MENU_ENABLED);
			m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_PREVIOUS, MENU_ENABLED);
		}

		//	Are we on the last media object?
		if((m_pXmlMediaTree != 0) && (m_MediaTrees.IsLast(m_pXmlMediaTree) == TRUE))
		{
			m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_LAST, MENU_DISABLED);
			m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_NEXT, MENU_DISABLED);
		}
		else
		{
			m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_LAST, MENU_ENABLED);
			m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_NEXT, MENU_ENABLED);
		}
	}
	else
	{
		m_PopupMenu.EnableMenuItem(0, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);
		m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_NEXT, MENU_DISABLED);
		m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_PREVIOUS, MENU_DISABLED);
		m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_FIRST, MENU_DISABLED);
		m_MediaTreeMenu.EnableMenuItem(TMXML_MEDIATREES_LAST, MENU_DISABLED);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::EnableMenuCommands()
//
// 	Description:	This function is called to enable/disable the menu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::EnableMenuCommands() 
{
	EnableMediaTreeCommands();
	EnableMediaCommands();
	EnablePageCommands();
	EnableTreatmentCommands();
	EnablePrintCommands();
	EnableViewerCommands();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::EnablePageCommands()
//
// 	Description:	This function is called to enable/disable the commands 
//					used to iterate the pages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::EnablePageCommands() 
{
	//	Do we have a valid list of pages?
	if((m_pXmlMedia != 0) && (m_pXmlMedia->m_Pages.GetCount() > 1))
	{
		m_PopupMenu.EnableMenuItem(2, MF_ENABLED | MF_BYPOSITION);

		//	Are we on the first page?
		if((m_pXmlPage != 0) && (m_pXmlMedia->m_Pages.IsFirst(m_pXmlPage) == TRUE))
		{
			m_PageMenu.EnableMenuItem(TMXML_PAGES_FIRST, MENU_DISABLED);
			m_PageMenu.EnableMenuItem(TMXML_PAGES_PREVIOUS, MENU_DISABLED);
			ENABLE_BUTTON(TMTB_PREVPAGE, FALSE);
			ENABLE_BUTTON(TMTB_FIRSTPAGE, FALSE);
		}
		else
		{
			m_PageMenu.EnableMenuItem(TMXML_PAGES_FIRST, MENU_ENABLED);
			m_PageMenu.EnableMenuItem(TMXML_PAGES_PREVIOUS, MENU_ENABLED);
			ENABLE_BUTTON(TMTB_PREVPAGE, TRUE);
			ENABLE_BUTTON(TMTB_FIRSTPAGE, TRUE);
		}

		//	Are we on the last page?
		if((m_pXmlPage != 0) && (m_pXmlMedia->m_Pages.IsLast(m_pXmlPage) == TRUE))
		{
			m_PageMenu.EnableMenuItem(TMXML_PAGES_LAST, MENU_DISABLED);
			m_PageMenu.EnableMenuItem(TMXML_PAGES_NEXT, MENU_DISABLED);
			ENABLE_BUTTON(TMTB_NEXTPAGE, FALSE);
			ENABLE_BUTTON(TMTB_LASTPAGE, FALSE);
		}
		else
		{
			m_PageMenu.EnableMenuItem(TMXML_PAGES_LAST, MENU_ENABLED);
			m_PageMenu.EnableMenuItem(TMXML_PAGES_NEXT, MENU_ENABLED);
			ENABLE_BUTTON(TMTB_NEXTPAGE, TRUE);
			ENABLE_BUTTON(TMTB_LASTPAGE, TRUE);
		}
	}
	else
	{
		m_PopupMenu.EnableMenuItem(2, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);

		m_PageMenu.EnableMenuItem(TMXML_PAGES_NEXT, MENU_DISABLED);
		m_PageMenu.EnableMenuItem(TMXML_PAGES_PREVIOUS, MENU_DISABLED);
		m_PageMenu.EnableMenuItem(TMXML_PAGES_FIRST, MENU_DISABLED);
		m_PageMenu.EnableMenuItem(TMXML_PAGES_LAST, MENU_DISABLED);

		ENABLE_BUTTON(TMTB_NEXTPAGE, FALSE);
		ENABLE_BUTTON(TMTB_PREVPAGE, FALSE);
		ENABLE_BUTTON(TMTB_FIRSTPAGE, FALSE);
		ENABLE_BUTTON(TMTB_LASTPAGE, FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::EnablePrintCommands()
//
// 	Description:	This function is called to enable/disable the commands 
//					used to print images
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::EnablePrintCommands() 
{
	//	Is the control ready?
	if(m_TMView.CanPrint())
	{
		m_PrintMenu.EnableMenuItem(TMXML_PRINT_CURRENT, (m_iViewer == TMXML_VIEWER_TMVIEW));
		m_PrintMenu.EnableMenuItem(TMXML_PRINT_SETUP, MENU_ENABLED);
		ENABLE_BUTTON(TMTB_PRINT, (m_iViewer == TMXML_VIEWER_TMVIEW));
	}
	else
	{
		m_PrintMenu.EnableMenuItem(TMXML_PRINT_CURRENT, MENU_DISABLED);
		m_PrintMenu.EnableMenuItem(TMXML_PRINT_SETUP, MENU_DISABLED);
		ENABLE_BUTTON(TMTB_PRINT, FALSE);
	}

	//	Is batch printing available?
	if(m_TMPrint.IsReady() && m_MediaTrees.GetCount() > 0)
	{
		m_PrintMenu.EnableMenuItem(TMXML_PRINT_SELECTIONS, MENU_ENABLED);
		
		if(m_pXmlMedia != 0)
			m_PrintMenu.EnableMenuItem(TMXML_PRINT_DOCUMENT, MENU_ENABLED);
		else
			m_PrintMenu.EnableMenuItem(TMXML_PRINT_DOCUMENT, MENU_DISABLED);
	}
	else
	{
		m_PrintMenu.EnableMenuItem(TMXML_PRINT_SELECTIONS, MENU_DISABLED);
		m_PrintMenu.EnableMenuItem(TMXML_PRINT_DOCUMENT, MENU_DISABLED);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::EnableViewerCommands()
//
// 	Description:	This function is called to enable/disable the viewer 
//					specific commands. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::EnableViewerCommands() 
{
	BOOL bEnable = (m_iViewer == TMXML_VIEWER_TMVIEW);

	if(bEnable)
	{
		m_PopupMenu.EnableMenuItem(4, MF_ENABLED | MF_BYPOSITION);
		m_PopupMenu.EnableMenuItem(5, MF_ENABLED | MF_BYPOSITION);
		m_PopupMenu.EnableMenuItem(6, MF_ENABLED | MF_BYPOSITION);
		m_PopupMenu.EnableMenuItem(8, MF_ENABLED | MF_BYPOSITION);
		m_PopupMenu.EnableMenuItem(9, MF_ENABLED | MF_BYPOSITION);
	}
	else
	{
		m_PopupMenu.EnableMenuItem(4, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);
		m_PopupMenu.EnableMenuItem(5, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);
		m_PopupMenu.EnableMenuItem(6, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);
		m_PopupMenu.EnableMenuItem(8, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);
		m_PopupMenu.EnableMenuItem(9, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);
	}

	ENABLE_BUTTON(TMTB_NORMAL, bEnable);
	ENABLE_BUTTON(TMTB_ZOOMWIDTH, bEnable);
	
	ENABLE_BUTTON(TMTB_ROTATECW, bEnable);
	ENABLE_BUTTON(TMTB_ROTATECCW, bEnable);
	
	ENABLE_BUTTON(TMTB_ERASE, bEnable);
	ENABLE_BUTTON(TMTB_DELETEANN, bEnable);
	
	ENABLE_BUTTON(TMTB_FREEHAND, bEnable);
	ENABLE_BUTTON(TMTB_LINE, bEnable);
	ENABLE_BUTTON(TMTB_ARROW, bEnable);
	ENABLE_BUTTON(TMTB_ELLIPSE, bEnable);
	ENABLE_BUTTON(TMTB_RECTANGLE, bEnable);
	ENABLE_BUTTON(TMTB_FILLEDELLIPSE, bEnable);
	ENABLE_BUTTON(TMTB_FILLEDRECTANGLE, bEnable);
	ENABLE_BUTTON(TMTB_POLYLINE, bEnable);
	ENABLE_BUTTON(TMTB_POLYGON, bEnable);
	ENABLE_BUTTON(TMTB_ANNTEXT, bEnable);
	ENABLE_BUTTON(TMTB_DRAWTOOL, bEnable);

	ENABLE_BUTTON(TMTB_ZOOM, bEnable);
	ENABLE_BUTTON(TMTB_ZOOMRESTRICTED, bEnable);
	ENABLE_BUTTON(TMTB_PAN, bEnable);
	ENABLE_BUTTON(TMTB_CALLOUT, bEnable);
	ENABLE_BUTTON(TMTB_HIGHLIGHT, bEnable);
	ENABLE_BUTTON(TMTB_REDACT, bEnable);
	ENABLE_BUTTON(TMTB_SELECT, bEnable);

	ENABLE_BUTTON(TMTB_RED, bEnable);
	ENABLE_BUTTON(TMTB_GREEN, bEnable);
	ENABLE_BUTTON(TMTB_BLUE, bEnable);
	ENABLE_BUTTON(TMTB_YELLOW, bEnable);
	ENABLE_BUTTON(TMTB_BLACK, bEnable);
	ENABLE_BUTTON(TMTB_WHITE, bEnable);
	ENABLE_BUTTON(TMTB_DARKRED, bEnable);
	ENABLE_BUTTON(TMTB_DARKGREEN, bEnable);
	ENABLE_BUTTON(TMTB_DARKBLUE, bEnable);
	ENABLE_BUTTON(TMTB_LIGHTRED, bEnable);
	ENABLE_BUTTON(TMTB_LIGHTGREEN, bEnable);
	ENABLE_BUTTON(TMTB_LIGHTBLUE, bEnable);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::EnableTreatmentCommands()
//
// 	Description:	This function is called to enable/disable the commands 
//					used to iterate the treatments
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::EnableTreatmentCommands() 
{
	//	Enable/disable the treatment popup menu
	if(m_pXmlPage != 0)
	{
		m_PopupMenu.EnableMenuItem(3, MF_ENABLED | MF_BYPOSITION);
	}
	else
	{
		m_PopupMenu.EnableMenuItem(3, MF_DISABLED | MF_BYPOSITION | MF_GRAYED);
	}

	//	Enable/disable the save treatment command
	if((m_pXmlPage != 0) && (m_iViewer == TMXML_VIEWER_TMVIEW) &&
	   (m_strPutTreatmentScript.GetLength() > 0))
	{
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_SAVE, MENU_ENABLED);
		ENABLE_BUTTON(TMTB_SAVEZAP, TRUE);
	}
	else
	{
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_SAVE, MENU_DISABLED);
		ENABLE_BUTTON(TMTB_SAVEZAP, FALSE);
	}

	//	Enable/disable the delete/update treatment commands
	if((m_pXmlTreatment != 0) && (m_strPutTreatmentScript.GetLength() > 0))
	{
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_UPDATE, MENU_ENABLED);
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_DELETE, MENU_ENABLED);
		ENABLE_BUTTON(TMTB_UPDATEZAP, TRUE);
		ENABLE_BUTTON(TMTB_DELETEZAP, TRUE);
	}
	else
	{
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_UPDATE, MENU_DISABLED);
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_DELETE, MENU_DISABLED);
		ENABLE_BUTTON(TMTB_UPDATEZAP, FALSE);
		ENABLE_BUTTON(TMTB_DELETEZAP, FALSE);
	}

	//	Do we have a valid list of treatments?
	if((m_pXmlPage != 0) && (m_pXmlPage->m_Treatments.GetCount() > 0))
	{
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_NEXT, MENU_ENABLED);
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_PREVIOUS, MENU_ENABLED);

		ENABLE_BUTTON(TMTB_NEXTZAP, TRUE);
		ENABLE_BUTTON(TMTB_PREVZAP, TRUE);		 
		
		if((m_pXmlTreatment != 0) && (m_pXmlPage->m_Treatments.IsFirst(m_pXmlTreatment) == TRUE))
		{
			m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_FIRST, MENU_DISABLED);
			ENABLE_BUTTON(TMTB_FIRSTZAP, FALSE);
		}
		else
		{
			m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_FIRST, MENU_ENABLED);
			ENABLE_BUTTON(TMTB_FIRSTZAP, TRUE);
		}

		if((m_pXmlTreatment != 0) && (m_pXmlPage->m_Treatments.IsLast(m_pXmlTreatment) == TRUE))
		{
			m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_LAST, MENU_DISABLED);
			ENABLE_BUTTON(TMTB_LASTZAP, FALSE);
		}
		else
		{
			m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_LAST, MENU_ENABLED);
			ENABLE_BUTTON(TMTB_LASTZAP, TRUE);
		}
	}
	else
	{
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_NEXT, MENU_DISABLED);
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_PREVIOUS, MENU_DISABLED);
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_FIRST, MENU_DISABLED);
		m_TreatmentMenu.EnableMenuItem(TMXML_TREATMENTS_LAST, MENU_DISABLED);
		 
		ENABLE_BUTTON(TMTB_NEXTZAP, FALSE);
		ENABLE_BUTTON(TMTB_PREVZAP, FALSE);		 
		ENABLE_BUTTON(TMTB_FIRSTZAP, FALSE);
		ENABLE_BUTTON(TMTB_LASTZAP, FALSE);		 
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::Encode()
//
// 	Description:	This function will encode the string passed by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::Encode(CString& rUrl) 
{
	rUrl.Replace(" ", "%20");
	rUrl.Replace("?", "%3F");
	rUrl.Replace("=", "%3D");
	rUrl.Replace("/", "%2F");
	rUrl.Replace("&", "%26");
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ExecuteJavascript()
//
// 	Description:	This function will cause Internet Explorer to execute the
//					specified Java Script.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::ExecuteJavascript(LPCSTR lpScript) 
{
	char	szCached[512];
	CString	strScript;

	//	Do we need to insert the Javascript director?
	if(_strnicmp("javascript", lpScript, 10) != 0)
		strScript.Format("javascript:%s", lpScript);
	else
		strScript = lpScript;

	//	Run the script by downloading it to cache
	URLDownloadToCacheFile(m_pControl->GetControllingUnknown(),
						   strScript, szCached, sizeof(szCached), 0, 0);

	//	NOTE:	We don't check for any error because we don't actually expect
	//			to be able to cache a file

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ExtractCasebook()
//
// 	Description:	This function is called to extract the Ringtail case book
//					folder from the path provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::ExtractCasebook(LPCSTR lpPath, CString& rCasebook)
{
	char	szCasebook[_MAX_PATH];
	char*	pToken;

	ASSERT(lpPath != NULL);
	
	//	Move past the leading slash if it exists
	if(IsDirSeparator(lpPath[0]))
		lpPath++;

	//	Make a working copy of the path
	lstrcpyn(szCasebook, lpPath, sizeof(szCasebook));

	//	Now look for the next directory separator
	if((pToken = strchr(szCasebook, '/')) == 0)
	{
		pToken = strchr(szCasebook, '\\');
	}

	//	Did we find a directory separator?
	if(pToken != 0)
	{
		*pToken = '\0';

		rCasebook = szCasebook;
	}
	else
	{
		//	No casebook folder found
		rCasebook.Empty();
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ExtractDrive()
//
// 	Description:	This function is called to extract the root portion of the
//					specified path.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::ExtractDrive(LPCSTR lpPath, CString& rDrive)
{
	char	szBuffer[_MAX_PATH];
	char*	pToken;

	ASSERT(lpPath != NULL);
	
	rDrive.Empty();

	//	Copy the path to a working buffer
	lstrcpyn(szBuffer, lpPath, sizeof(szBuffer));

	//	Is this a UNC (ie. \\server\share\) path ?
	if(IsDirSeparator(szBuffer[0]) && IsDirSeparator(szBuffer[1]))
	{
		//	Separate the server name from the rest of the path
		if((pToken = strchr(&(szBuffer[2]), '\\')) == 0)
			pToken = strchr(&(szBuffer[2]), '/');
		if(pToken != 0)
			*pToken = '\0';

		rDrive = &(szBuffer[2]);
	}
	else
	{
		//	Not a UNC so just look for the : character
		if((pToken = strchr(szBuffer, ':')) != 0)
		{
			*(pToken + 1) = '\0';
			rDrive = szBuffer;
		}
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ExtractFilename()
//
// 	Description:	This function is called to extract the filename portion of 
//					the specified path.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::ExtractFilename(LPCSTR lpPath, CString& rFilename)
{
	int i = 0;
	
	ASSERT(lpPath != NULL);

	//	Find the last occurrance of a directory separator
	for(i = lstrlen(lpPath) - 1; i >= 0; i--)
	{
		if(IsDirSeparator(lpPath[i]))
		{
			break;
		}
	}

	if(i >= 0)
		rFilename = (lpPath + i + 1);
	else
		rFilename.Empty();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ExtractTreatmentId()
//
// 	Description:	This function is called to extract the treatment id using
//					the src attribute assigned to the treatment object.
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function is provided to maintain backward compatability
//					with early Ringtail scripts that did not furnish the id as
//					part of the XML specification.
//
//==============================================================================
BOOL CXmlFrame::ExtractTreatmentId(CXmlTreatment* pTreatment)
{
	char	szBuffer[512];
	char*	pToken;

	ASSERT(pTreatment != NULL);

	//	Transfer the source attribute to our working buffer
	lstrcpyn(szBuffer, pTreatment->m_strSource, sizeof(szBuffer));

	//	Search for the treatment_id parameter
	if((pToken = strrchr(szBuffer, '?')) != NULL)
	{
		//	Extract the id
		if((pToken = strchr(pToken, '=')) != NULL)
		{
			pTreatment->m_strId = (pToken + 1);
			return TRUE;
		}
	}

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::FillExtensions()
//
// 	Description:	This function will fill the list of file extensions to be
//					associated with the TMView control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::FillExtensions()
{
	CTMIni		Ini;
	CExtension*	pExtension;
	char		szLineId[32];
	char		szIniStr[32];
	int			i;

	//	Build the default file extension list
	m_Extensions.Add("png");
	m_Extensions.Add("tiff");
	m_Extensions.Add("tif");
	m_Extensions.Add("bmp");
	m_Extensions.Add("pcx");
	m_Extensions.Add("jpeg");
	m_Extensions.Add("jpg");
	m_Extensions.Add("zap");

	//	Now read the configurable extensions from the ini file
	if(Ini.Open(m_strIniFilename, TMXML_INI_EXTENSIONS_SECTION))
	{
		//	Read the list of extensions to be added to the list
		for(i = 1; ; i++)
		{
			sprintf_s(szLineId, sizeof(szLineId), "A%d", i);
			Ini.ReadString(szLineId, szIniStr, sizeof(szIniStr));

			if(lstrlen(szIniStr) > 0)
			{
				//	Make sure this extension is not already in the list
				if(m_Extensions.Find(szIniStr) == 0)
					m_Extensions.Add(szIniStr);
			}
			else
			{
				break;
			}
		}

		//	Read the list of extensions to be removed from the list
		for(i = 1; ; i++)
		{
			sprintf_s(szLineId, sizeof(szLineId), "R%d", i);
			Ini.ReadString(szLineId, szIniStr, sizeof(szIniStr));

			if(lstrlen(szIniStr) > 0)
			{
				//	Is this extension in the list?
				if((pExtension = m_Extensions.Find(szIniStr)) != 0)
					m_Extensions.Remove(pExtension, TRUE);
			}
			else
			{
				break;
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::FindFile()
//
// 	Description:	This function is called to determine if the specified file
//					exists
//
// 	Returns:		TRUE if the file is found
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::FindFile(LPCSTR lpFilespec) 
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;

	if((hFind = FindFirstFile(lpFilespec, &FindData)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		FindClose(hFind);
		return TRUE;
	}	
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::FreePutTreatment()
//
// 	Description:	This function is called to free the memory and resources
//					associated with the put treatment transfer buffer.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::FreePutTreatment() 
{
	//	Deallocate the existing request
	if(m_PutTreatment.lpRequest != 0)
		HeapFree(GetProcessHeap(), 0, m_PutTreatment.lpRequest);

	//	Reset the members
	m_PutTreatment.lpRequest = 0;
	m_PutTreatment.dwLength  = 0;
	m_PutTreatment.dwStatusCode = 0;
	m_PutTreatment.iAction = 0;
	m_PutTreatment.strRequestHeader.Empty();
	m_PutTreatment.strResponse.Empty();
	m_PutTreatment.strResponseHeader.Empty();
	m_PutTreatment.strUrl.Empty();
	m_PutTreatment.strPageId.Empty();
	m_PutTreatment.strTreatmentId.Empty();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetConnection()
//
// 	Description:	This function is called to create a connection to the
//					remote server using the session provided by the caller.
//
// 	Returns:		A pointer to the HTTP connection object
//
//	Notes:			None
//
//==============================================================================
CHttpConnection* CXmlFrame::GetConnection(CXmlSession* pSession, LPCSTR lpServer) 
{
	CHttpConnection* pConnection = 0;

	ASSERT(pSession);

	//	Now try to establish the FTP connection
	try
	{
		//	Are we using a proxy server?
		if((m_ViewerOptions.iConnection == TMXML_CONNECTION_PROXY) && 
		   (m_ViewerOptions.uProxyPort > 0))
		{
			pConnection = pSession->GetHttpConnection(lpServer, 
													 (INTERNET_PORT)m_ViewerOptions.uProxyPort);
		}

		//	Are we using a user defined HTTP port?
		else if((m_ViewerOptions.iConnection == TMXML_CONNECTION_ASSIGNED) && 
		        (m_ViewerOptions.uInternetPort > 0))
		{
			pConnection = pSession->GetHttpConnection(lpServer, 
													 (INTERNET_PORT)m_ViewerOptions.uInternetPort);
		}
		else
		{
			if(m_bIsSecure)
			{
				pConnection = pSession->GetHttpConnection(lpServer, (INTERNET_PORT)INTERNET_DEFAULT_HTTPS_PORT);
			}
			else
			{
				pConnection = pSession->GetHttpConnection(lpServer);
			}
		}

		ASSERT(pConnection);
		return pConnection;
	}
	catch (CInternetException* pEx)
	{
		//	Should we display an error message?
		if(m_pErrors)
		{
			char szError[256];

			if(pEx->GetErrorMessage(szError, sizeof(szError)))
				m_pErrors->Handle(0, IDS_TMXML_CONNECTION_FAILED, szError);
			else
				m_pErrors->Handle(0, IDS_TMXML_CONNECTION_FAILED, "A system exception occurred");
		}
		
		pEx->Delete();

		if(pConnection)
			delete pConnection;

		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetDefaultTemplate()
//
// 	Description:	This function is called to get a default template to be
//					used for printing batch jobs.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTemplate* CXmlFrame::GetDefaultTemplate() 
{
	CTemplate*	pTemplate;

	//	Allocate a new template
	pTemplate = new CTemplate();
	ASSERT(pTemplate);

	//	Set default values for the template so that the images get printed
	//	one per page
	pTemplate->m_fTopMargin			=  0.00f;
	pTemplate->m_fLeftMargin		=  0.00f;
	pTemplate->m_fCellHeight		= 10.50f;
	pTemplate->m_fCellWidth			=  8.00f;
	pTemplate->m_fCellSpaceWidth	=  0.00f;
	pTemplate->m_fCellSpaceHeight	=  0.00f;
	pTemplate->m_fCellPadWidth		=  0.05f;
	pTemplate->m_fCellPadHeight		=  0.05f;
	pTemplate->m_fImagePadWidth		=  0.05f;
	pTemplate->m_fImagePadHeight	=  0.05f;
	pTemplate->m_sRows				=  1;
	pTemplate->m_sColumns			=  1;
	pTemplate->m_bImageEnable		=  TRUE;
	pTemplate->m_bPrintImage		=  TRUE;
	pTemplate->m_bPageAsSeries		=  FALSE;
	pTemplate->m_bPrintBorder		=  FALSE;
	pTemplate->m_bPrintFullPath	    =  FALSE;
	pTemplate->m_strDescription     = "Tmxml: 1 per page";

	pTemplate->SetPrintEnabled(TEMPLATE_BARCODE, FALSE);
	pTemplate->SetPrintEnabled(TEMPLATE_GRAPHIC, FALSE);
	pTemplate->SetPrintEnabled(TEMPLATE_FILENAME, FALSE);
	pTemplate->SetPrintEnabled(TEMPLATE_PAGENUM, FALSE);
	pTemplate->SetPrintEnabled(TEMPLATE_NAME, FALSE);
	pTemplate->SetPrintEnabled(TEMPLATE_DEPONENT, FALSE);

	return pTemplate;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetExplorer()
//
// 	Description:	This function is called to attach to the running instance
//					of Internet Explorer.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::GetExplorer()
{
	IServiceProvider*	pService   = NULL;
	IServiceProvider*	pService2  = NULL;
	IWebBrowser2*		pBrowser   = NULL;
	IOleContainer*		pContainer = NULL;
	LPDISPATCH			lpDispatch = NULL;

	ASSERT(m_pControl);

	//	Have we already attached?
	if(m_pIBrowser != 0)
		return TRUE;

	//	Get the dispatch interface for the container's service provider
	m_pControl->GetClientSite()->QueryInterface(IID_IServiceProvider,
											   (void **)&pService);
	if(pService != NULL)
	{
		//	Get the top level service provider
		pService->QueryService(SID_STopLevelBrowser, IID_IServiceProvider,
							  (void **)&pService2);
		if(pService2 != NULL)
		{
			//	Get the web browser interface application 
			pService2->QueryService(SID_SWebBrowserApp, IID_IWebBrowser2,
								   (void **)&pBrowser);
			if(pBrowser != NULL)
			{		
				//	Store the browser interface
				m_pIBrowser = new CIWebBrowser2((LPDISPATCH)pBrowser);
			}
		}
	}

	//	Were we unable to attach to the browser interface
	if((m_pIBrowser == NULL) || (m_pIBrowser->m_lpDispatch == NULL))
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_EXPLORER_FAILED);

		RELEASE_INTERFACE(pService);
		RELEASE_INTERFACE(pService2);
		RELEASE_INTERFACE(pBrowser);
		return FALSE;
	}

	//MessageBox(m_pIBrowser->GetFullName());
	
	//	Release the interfaces that we no longer need
	RELEASE_INTERFACE(pService);
	RELEASE_INTERFACE(pService2);
	
	return TRUE;
/*

	CIHTMLDocument2* pHTML = new CIHTMLDocument2(m_pIBrowser->GetDocument());
	if(pHTML->m_lpDispatch != NULL)
	{
		CIHTMLFramesCollection2* pFrames = new CIHTMLFramesCollection2(pHTML->GetFrames());
		if(pFrames->m_lpDispatch != 0)
		{
			CString M;
			M.Format("%ld", pFrames->GetLength());
			MessageBox("Cool", pHTML->GetTitle());
		}

		if(pFrames)
			delete pFrames;
	}

	if(pHTML)
		delete pHTML;

	char		szCache[1024];

	//	Download the file from the remote server
	URLDownloadToCacheFile(m_pControl->GetControllingUnknown(),
						  "javascript:window.alert(\"Hi Mr. Phil\")", 
						   szCache, sizeof(szCache), 0, 0);
MessageBox("Stop");
	
	m_pControl->GetClientSite()->GetContainer(&pContainer);
	if(pContainer != NULL)
	{
		pContainer->QueryInterface(IID_IHTMLDocument2, (void**)&lpDispatch); 

		if(lpDispatch != 0)
		{
			CIHTMLDocument2* pHTML = new CIHTMLDocument2(lpDispatch);
			if(pHTML->m_lpDispatch != NULL)
			{
				MessageBox("Cool");
			}
		}
	}
*/
}
 
//==============================================================================
//
// 	Function Name:	CXmlFrame::GetFile()
//
// 	Description:	This function is called to retrieve a file based on the
//					source specification.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::GetFile(LPCSTR lpSource, CString& rFilename, BOOL bShowProgress,
						LPARAM lParam)
{
	CString strCached;
	CString	strDrive;

	ASSERT(lpSource);
	ASSERT(lstrlen(lpSource) > 0);

	//	Get the drive specification
	ExtractDrive(lpSource, strDrive);

	//	Do we need to add the path?
	if(strDrive.GetLength() == 0)
	{
		//	Is this an absolute path specification?
		if(IsDirSeparator(lpSource[0]))
		{
			rFilename = m_strAbsolute;
		}
		else
		{
			rFilename = m_strRelative;
		}
		rFilename += lpSource;
	}
	else
	{
		rFilename = lpSource;
	}

	//	Do we need to download the file?
	if(m_bIsRemote)
	{
		if(Download(rFilename, strCached, TRUE, bShowProgress, lParam))
		{
			//	Return the cached filename
			rFilename = strCached;
			return TRUE;
		}
		else
		{
			return FALSE;
		}
	}	
	else
	{
		//	Is the caller expecting a message to be sent when the file is
		//	available?
		if(lParam > 0)
		{
			//	Make it look like the file was downloaded
			theDownload.hWnd   = m_hWnd;
			theDownload.lParam = lParam;
			theDownload.strSource = lpSource;
			theDownload.bComplete = TRUE;
			theDownload.bAbort = FALSE;
			theDownload.bRemote = FALSE;

			//	Does the file exist?
			if(FindFile(rFilename))
			{
				theDownload.strCached = rFilename;
				theDownload.strErrorMsg.Empty();
				theDownload.bError = FALSE;
			}
			else
			{
				theDownload.strCached.Empty();
				theDownload.strErrorMsg.Format("Unable to locate %s", rFilename);
				theDownload.bError = TRUE;
			}

			//	Post the message so that we can return before it's processed
			PostMessage(WM_DOWNLOAD, TMXML_DOWNLOAD_COMPLETE, lParam);

			//	Always return TRUE for asynchronous. The caller will process any
			//	errors on the download notification
			return TRUE;
		}
		else
		{
			return FindFile(rFilename);
		}
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetMenuColorCmd()
//
// 	Description:	This function is called to get the menu command associated
//					with the specified toolbar color button.
//
// 	Returns:		The associated command identifier
//
//	Notes:			None
//
//==============================================================================
int CXmlFrame::GetMenuColorCmd(int iColorButton) 
{
	switch(iColorButton)
	{
		case TMTB_RED:			return TMXML_COLOR_RED;
		case TMTB_GREEN:		return TMXML_COLOR_GREEN;
		case TMTB_BLUE:			return TMXML_COLOR_BLUE;
		case TMTB_YELLOW:		return TMXML_COLOR_YELLOW;
		case TMTB_BLACK:		return TMXML_COLOR_BLACK;
		case TMTB_WHITE:		return TMXML_COLOR_WHITE;
		case TMTB_DARKRED:		return TMXML_COLOR_DARKRED;
		case TMTB_DARKGREEN:	return TMXML_COLOR_DARKGREEN;
		case TMTB_DARKBLUE:		return TMXML_COLOR_DARKBLUE;
		case TMTB_LIGHTRED:		return TMXML_COLOR_LIGHTRED;
		case TMTB_LIGHTGREEN:	return TMXML_COLOR_LIGHTGREEN;
		case TMTB_LIGHTBLUE:	return TMXML_COLOR_LIGHTBLUE;
		default:				ASSERT(0);
								return TMXML_COLOR_RED;
	}
}
 
//==============================================================================
//
// 	Function Name:	CXmlFrame::GetMenuToolCmd()
//
// 	Description:	This function is called to get the menu command associated
//					with the specified TMView drawing tool.
//
// 	Returns:		The associated command identifier
//
//	Notes:			None
//
//==============================================================================
int CXmlFrame::GetMenuToolCmd(int iDrawTool) 
{
	switch(iDrawTool)
	{
		case LINE:				return TMXML_DRAW_LINE;	
		case ARROW:				return TMXML_DRAW_ARROW;	
		case ELLIPSE:			return TMXML_DRAW_ELLIPSE;	
		case RECTANGLE:			return TMXML_DRAW_RECTANGLE;	
		case FILLED_ELLIPSE:	return TMXML_DRAW_FILLEDELLIPSE;	
		case FILLED_RECTANGLE:	return TMXML_DRAW_FILLEDRECTANGLE;	
		case POLYLINE:			return TMXML_DRAW_POLYLINE;	
		case POLYGON:			return TMXML_DRAW_POLYGON;	
		case ANNTEXT:			return TMXML_DRAW_TEXT;	
		case FREEHAND:
		default:				return TMXML_DRAW_FREEHAND;
	}
}
 
//==============================================================================
//
// 	Function Name:	CXmlFrame::GetNextPrintFile()
//
// 	Description:	This function is called to get the next file required for
//					the batch print job.
//
// 	Returns:		TRUE if there is another file to retrieve
//
//	Notes:			The only time this message should be received is for
//					retrieving files for printing.
//
//==============================================================================
BOOL CXmlFrame::GetNextPrintFile(void* lpPrevious)
{
	CString strSource;
	CString strCached;
	LPARAM	lParam;

	//	Is this the page file we just downloaded?
	if((CXmlPage*)lpPrevious == m_pXmlPrintPage)
	{
		//	Is there a treatment pending?
		if(m_pXmlPrintTreatment)
		{
			//	Download the treatment
			strSource = m_pXmlPrintTreatment->m_strSource;
			lParam    = (LPARAM)m_pXmlPrintTreatment;
		}
		else
		{
			//	Get the next page
			if(!GetNextPrintPage())
				return FALSE;
			ASSERT(m_pXmlPrintPage != 0);

			//	Always start with the page
			strSource = m_pXmlPrintPage->m_strSource;
			lParam    = (LPARAM)m_pXmlPrintPage;
		
		}//	if(m_pXmlPrintTreatment)

	}
	
	//	Is it the treatment file that was just downloaded?
	else if((CXmlTreatment*)lpPrevious == m_pXmlPrintTreatment)
	{
		//	Get the next treatment
		if((m_pXmlPrintTreatment = m_pXmlPrintPage->m_Treatments.Next()) != 0)
		{
			//	Download the treatment
			strSource = m_pXmlPrintTreatment->m_strSource;
			lParam    = (LPARAM)m_pXmlPrintTreatment;
		}
		else
		{
			//	Get the next page
			if(!GetNextPrintPage())
				return FALSE;
			ASSERT(m_pXmlPrintPage != 0);

			//	Always start with the page
			strSource = m_pXmlPrintPage->m_strSource;
			lParam    = (LPARAM)m_pXmlPrintPage;
		}
	}
	else
	{
		ASSERT(0);
		return FALSE;
	}

	//	Get the next file
	if(GetFile(strSource, strCached, FALSE, lParam))
	{
		//	Notify the progress dialog if this is a local file
		if((m_pPrintProgress != 0) && (theDownload.bRemote == FALSE))
			m_pPrintProgress->SetFilename(theDownload.strCached);

		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetNextPrintPage()
//
// 	Description:	This function is called to get the next page in the 
//					current batch printing job.
//
// 	Returns:		TRUE if there is another page
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::GetNextPrintPage()
{
	CString strFilename;

	ASSERT(m_pXmlPrintTree != 0);
	ASSERT(m_pXmlPrintMedia != 0);
	
	if((m_pXmlPrintTree == 0) || (m_pXmlPrintMedia == 0))
		return FALSE;

	//	Get the next page in the current media object
	if((m_pXmlPrintPage = m_pXmlPrintMedia->m_Pages.Next()) == 0)
	{
		//	Get the next media object in the tree
		while((m_pXmlPrintMedia = m_pXmlPrintTree->Next()) != 0)
		{
			//	Get the first page of the new media object
			if((m_pXmlPrintPage = m_pXmlPrintMedia->m_Pages.First()) != 0)
			{
				//	Get the first treatment in this page
				m_pXmlPrintTreatment = m_pXmlPrintPage->m_Treatments.First();
				break;
			}
		}
	}
	else
	{
		m_pXmlPrintTreatment = m_pXmlPrintPage->m_Treatments.First();
	}

	return (m_pXmlPrintPage != 0);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetPutTreatment()
//
// 	Description:	This function is called to construct the request for 
//					posting a new treatment to the remote server.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::GetPutTreatment(LPCSTR lpFilename, LPCSTR lpPageId,
							    int iAction, LPCSTR lpTreatmentId) 
{
	CString	strBefore;
	CString strAfter;
	CString	strTemp;
	CString	strHeader;
	CString	strAction;
	CString	strTreatmentId;
	LPBYTE	lpRequest = 0;
	DWORD	dwBytes;
	DWORD	dwAfter;
	DWORD	dwBefore;
	DWORD	dwFile;
	char	szErrorMsg[512];

	ASSERT(lpFilename);
	ASSERT(lstrlen(lpFilename) > 0);
	ASSERT(lpPageId);
	ASSERT(lstrlen(lpPageId) > 0);

	//	What action is being used to submit the request?
	switch(iAction)
	{
		case TMXML_PUT_TREATMENT_SAVE:

			strAction = "ADD";
			strTreatmentId = "0";
			break;

		case TMXML_PUT_TREATMENT_UPDATE:

			strAction = "UPDATE";

			ASSERT(lpTreatmentId);
			ASSERT(lstrlen(lpTreatmentId) > 0);

			if(lpTreatmentId)
				strTreatmentId = lpTreatmentId;
			break;

		default:

			ASSERT(0);
			return FALSE;
	}

	//	Store the action identifier
	m_PutTreatment.iAction = iAction;

	//	Open the file
	try
	{
		//	Format the header required for the request
		strHeader.Format("Content-Type: multipart/form-data; boundary=%s\r\n",
						 TMXML_POST_DELIMITER);

		//	Open the zap file
		CFile Treatment(lpFilename, CFile::modeRead);
	
		//	Construct the block that goes before the file data
		strBefore  = TMXML_POST_DELIMITER;
		strBefore += "\r\n";		

		strBefore += "Content-Disposition: form-data; name=\"pageid\"\r\n\r\n";

		strBefore += lpPageId;
		strBefore += "\r\n";

		strBefore += TMXML_POST_DELIMITER;
		strBefore += "\r\n";		

		strBefore += "Content-Disposition: form-data; name=\"action\"\r\n\r\n";

		strBefore += strAction;
		strBefore += "\r\n";

		strBefore += TMXML_POST_DELIMITER;
		strBefore += "\r\n";		

		strBefore += "Content-Disposition: form-data; name=\"treatment_id\"\r\n\r\n";

		strBefore += strTreatmentId;
		strBefore += "\r\n";

		strBefore += TMXML_POST_DELIMITER;
		strBefore += "\r\n";		

		strBefore += "Content-Disposition: form-data; name=\"theFile\"; ";

		strTemp.Format("filename=\"%s\"\r\n", lpFilename);
		strBefore += strTemp;

		strBefore += "Content-Type: application/ftic-zap4\r\n\r\n";

		//	Construct the block that goes after the file data
		strAfter.Format("\r\n%s--\r\n", TMXML_POST_DELIMITER);

		//	Allocate the memory to store the request
		dwBefore = (DWORD)strBefore.GetLength();
		dwAfter  = (DWORD)strAfter.GetLength();
		dwFile   = (DWORD)Treatment.GetLength();
		dwBytes  = dwBefore + dwFile + dwAfter;
		if(dwBytes > 0)
			lpRequest = (LPBYTE)HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, dwBytes);

		if(lpRequest)
		{
			//	Transfer data to the buffer
			memcpy(lpRequest, (LPVOID)((LPCSTR)strBefore), dwBefore);

			//	Add the file data to the request
			Treatment.Read((lpRequest + dwBefore), dwFile);

			//	Transfer the rest of the request
			memcpy((lpRequest + dwBefore + dwFile),(LPVOID)((LPCSTR)strAfter), 
				    dwAfter);

			//	Deallocate the existing request
			FreePutTreatment();

			//	Create the new request
			m_PutTreatment.iAction = iAction;
			m_PutTreatment.lpRequest = lpRequest;
			m_PutTreatment.dwLength  = dwBytes;
			m_PutTreatment.strRequestHeader = strHeader;
			m_PutTreatment.strPageId = lpPageId;
			m_PutTreatment.strTreatmentId = strTreatmentId;

			return TRUE;
		}
		else
		{
			return FALSE;
		}

	}
	//	Catch all file exceptions
	catch(CFileException* pFileException)
	{
		//	Get the error message and pass it on to the handler
		if(m_pErrors)
		{
			pFileException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
			m_pErrors->Handle(0, szErrorMsg);
		}
		pFileException->Delete();
		return FALSE;
	}
	//	Catch all other exceptions
	catch(CException* pException)
	{
		//	Get the error message and pass it on to the handler
		if(m_pErrors)
		{
			pException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
			m_pErrors->Handle(0, szErrorMsg);
		}
		pException->Delete();
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetResponseHeaders()
//
// 	Description:	This function is provided as a debugging tool. It is called
//					to view the response headers returned by the server for
//					a specific request.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::GetResponseHeaders(LPCSTR lpRequest, CString& rResponse) 
{
	CInternetSession	Session;
	CHttpFile*			pFile;

	if((pFile = (CHttpFile*)Session.OpenURL(lpRequest)) != 0)
	{
		pFile->QueryInfo(HTTP_QUERY_RAW_HEADERS_CRLF, rResponse);
	}
	else
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_RESPONSE_HEADERS, lpRequest);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetSession()
//
// 	Description:	This function is called to create an internet session to
//					be used for submitting remote requests.
//
// 	Returns:		A pointer to the new internet session object
//
//	Notes:			The caller is responsible for deallocation of the session
//
//==============================================================================
CXmlSession* CXmlFrame::GetSession() 
{
	CXmlSession* pSession = 0;

	//	Now try to establish the session
	try
	{
		//	Create the new session
		if((m_ViewerOptions.iConnection == TMXML_CONNECTION_PROXY) &&
		   (lstrlen(m_ViewerOptions.szProxyServer) > 0))
		{
			pSession = new CXmlSession(this, 1, INTERNET_OPEN_TYPE_PROXY, 
									   m_ViewerOptions.szProxyServer);
		}
		else
		{
			pSession = new CXmlSession(this, 1, INTERNET_OPEN_TYPE_PRECONFIG, 
									   NULL);
		}
		ASSERT(pSession);

		return pSession;

	}
	catch (CInternetException* pEx)
	{
		//	Should we display an error message?
		if(m_pErrors)
		{
			char szError[256];

			if(pEx->GetErrorMessage(szError, sizeof(szError)))
				m_pErrors->Handle(0, IDS_TMXML_SESSION_FAILED, szError);
			else
				m_pErrors->Handle(0, IDS_TMXML_SESSION_FAILED, "A system exception occurred");
		}
		
		pEx->Delete();

		if(pSession)
			delete pSession;

		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::GetViewerFromFile()
//
// 	Description:	This function is called to get the viewer that should be
//					used to display the specified file.
//
// 	Returns:		The appropriate viewer identifier
//
//	Notes:			None
//
//==============================================================================
int CXmlFrame::GetViewerFromFile(LPCSTR lpszFilename)
{
	int		iViewer = TMXML_VIEWER_TMBROWSE;
	char*	pExtension;

	ASSERT(lpszFilename);

	//	Get the file extension
	if((pExtension = (char*)strrchr(lpszFilename, '.')) != 0)
	{
		//	Is this extension in the list?
		if(m_Extensions.Find(pExtension + 1) != 0)
			iViewer = TMXML_VIEWER_TMVIEW;
	}
	return iViewer;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::InitPageBar()
//
// 	Description:	This function is called to initialize the page navigation
//					window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::InitPageBar() 
{
	//	Create the page navigation window
	if(!m_PageBar.Create(this)) return;
	
	//	Initialize with the control's ini file
	m_PageBar.Initialize(m_strIniFilename, TMXML_INI_PAGEBAR_SECTION);
	
	//	Set the navigation toolbar properties to match the primary toolbar
	m_PageBar.SetToolbarProps(m_TMTool);

	//	Build the page navigation bar
	m_PageBar.Build();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::InitToolbar()
//
// 	Description:	This function is called to initialize the viewer's toolbar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::InitToolbar() 
{
	//	Set the ini filename and section
	m_TMTool.SetIniFile(m_strIniFilename);
	m_TMTool.SetIniSection(TMXML_INI_TOOLBAR_SECTION);

	//	Set the button mask for the toolbar
	m_TMTool.SetButtonMask(_szButtonMask);

	//	Set the default button map for the toolbar
	m_TMTool.SetButtonMap(_aButtonMap);

	//	Initialize the control
	m_TMTool.Initialize();

	//	Set the check state of the zoom buttons
	m_TMTool.SetZoomButton(m_TMView.GetAction() == ZOOM, m_TMView.GetZoomToRect());
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::IsDirSeparator()
//
// 	Description:	This function is called to determine if the specified 
//					character is used to separate directories in a path
//
// 	Returns:		TRUE if the character is a separator
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::IsDirSeparator(char cChar)
{
	return (cChar == '\\' || cChar == '/');
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::IsXMLFile()
//
// 	Description:	This function is called determine if the specified filename
//					identifies a custom XML file
//
// 	Returns:		TRUE if the file is an XML formatted file
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::IsXMLFile(LPCSTR lpFilename) 
{
	char	szFilename[_MAX_PATH];
	char*	pExtension;

	lstrcpyn(szFilename, lpFilename, sizeof(szFilename));

	//	Locate the file extension
	if((pExtension = strrchr(szFilename, '.')) != 0)
	{
		return (lstrcmpi(pExtension + 1, TMXML_FILE_EXTENSION) == 0);
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::jumpToPage()
//
// 	Description:	This method is called to load the specified page in the
//					active media tree. It is provided to support the Ringtail
//					software developers.
//
// 	Returns:		TMXML_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CXmlFrame::jumpToPage(LPCTSTR lpszPageId) 
{
	CXmlPage* pXmlPage = 0;

	//	Get the specified page
	if(m_pXmlMedia != 0)
		pXmlPage = m_pXmlMedia->m_Pages.Find(lpszPageId);

	//	Were we able to locate the specified page?
	if(pXmlPage != 0)
	{
		if(SetXmlPage(pXmlPage))
		{
			return TMXML_NOERROR;
		}
		else
		{
			return TMXML_JUMPPAGEFAILED;
		}
	}
	else
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_FIND_PAGE_FAILED, lpszPageId);
							  
		return TMXML_FINDPAGEFAILED;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::loadDocument()
//
// 	Description:	This method is called to load a new document. It is 
//					provided specifically at the request of the Ringtail
//					software developers.
//
// 	Returns:		TMXML_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CXmlFrame::loadDocument(LPCTSTR lpszUrl) 
{
	return LoadFile(lpszUrl);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::LoadFile()
//
// 	Description:	This function is called to load a new file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::LoadFile(LPCSTR lpSource) 
{
	CString strSource;
	CString	strFilename;
	CString	strRequest;

	//	Reset the page navigation window
	m_PageBar.SetXmlMedia(0);

	//	Reset the local interfaces and lists
	ReleaseAll();

	//	Did the caller provide an empty filename?
	if((lpSource == 0) || (lstrlen(lpSource) == 0))
	{
		strSource.Empty();
		SetPaths(strSource);
		return TRUE;
	}
	
	//	Determine the root folder and whether or not this is a remote file
	strSource = lpSource;
	SetPaths(strSource);

	//	Get the file from the remote server if necessary
	if(!GetFile(strSource, strFilename, TRUE, 0))
		return FALSE;

	//	Is this an XML formatted file
	if(IsXMLFile(strFilename))
	{
		//	Load the file into the parser
		if(XMLLoadFile(strFilename))
		{
			//	Should we display the page navigation window?
			if(m_ViewerOptions.bShowPageNavigation)
			{
				if(IsWindow(m_PageBar.m_hWnd) && !m_PageBar.IsWindowVisible())
				{
					RecalcLayout();
				}
			}

			//	Process all actions contained in the file
			return XMLProcessActions();
		}
		else
		{
			return FALSE;
		}
	}
	else
	{
		//	Load the file directly into the viewer
		if(m_TMView.LoadFile(strFilename, -1) == TMV_NOERROR)
		{
			//	Do we need to run a script to get the XML file?
			if(m_strGetXmlScript.GetLength() > 0)
			{
				XMLRunScript(m_strGetXmlScript);
			}
			else
			{
				//	Should we turn off the page navigation bar?
				if(IsWindow(m_PageBar.m_hWnd) && m_PageBar.IsWindowVisible())
					RecalcLayout();
			}

			return TRUE;
		}
		else
		{
			return FALSE;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnColorCommand()
//
// 	Description:	This function processes color submenu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnColorCommand(int iCmd) 
{
	switch(iCmd)
	{
		case TMXML_COLOR_BLACK:

			m_TMView.SetColor(TMV_BLACK);
			break;

		case TMXML_COLOR_RED:

			m_TMView.SetColor(TMV_RED);
			break;

		case TMXML_COLOR_GREEN:

			m_TMView.SetColor(TMV_GREEN);
			break;

		case TMXML_COLOR_BLUE:

			m_TMView.SetColor(TMV_BLUE);
			break;

		case TMXML_COLOR_YELLOW:

			m_TMView.SetColor(TMV_YELLOW);
			break;

		case TMXML_COLOR_MAGENTA:

			m_TMView.SetColor(TMV_MAGENTA);
			break;

		case TMXML_COLOR_CYAN:

			m_TMView.SetColor(TMV_CYAN);
			break;

		case TMXML_COLOR_GREY:

			m_TMView.SetColor(TMV_GREY);
			break;

		case TMXML_COLOR_WHITE:

			m_TMView.SetColor(TMV_WHITE);
			break;

		case TMXML_COLOR_DARKRED:

			m_TMView.SetColor(TMV_DARKRED);
			break;

		case TMXML_COLOR_DARKGREEN:

			m_TMView.SetColor(TMV_DARKGREEN);
			break;

		case TMXML_COLOR_DARKBLUE:

			m_TMView.SetColor(TMV_DARKBLUE);
			break;

		case TMXML_COLOR_LIGHTRED:

			m_TMView.SetColor(TMV_LIGHTRED);
			break;

		case TMXML_COLOR_LIGHTGREEN:

			m_TMView.SetColor(TMV_LIGHTGREEN);
			break;

		case TMXML_COLOR_LIGHTBLUE:

			m_TMView.SetColor(TMV_LIGHTBLUE);
			break;
	}

	//	Make sure the correct color button is checked
	CheckMenuColor(FALSE);

	//	Make sure the local options are up to date
	UpdateToolOptions();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnCtlColor()
//
// 	Description:	This function traps all WM_CTLCOLOR messages
//
// 	Returns:		Handle to the brush used to paint the background
//
//	Notes:			None
//
//==============================================================================
HBRUSH CXmlFrame::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG && m_pBackground)
		return (HBRUSH)(*m_pBackground);
	else
		return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnDeleteTreatment()
//
// 	Description:	This function is called to delete the current treatment
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnDeleteTreatment() 
{
	CXmlSession*	pSession = 0;
	CHttpFile*		pFile = 0;
	CString			strResponse;

	ASSERT(m_strDeleteTreatmentScript.GetLength() > 0);
	if(m_strDeleteTreatmentScript.GetLength() == 0)
		return;

	//	We have to have a valid treatment selection
	ASSERT(m_pXmlPage != 0);
	ASSERT(m_pXmlTreatment != 0);
	if((m_pXmlPage == 0) || (m_pXmlTreatment == 0))
		return;

	//	Now try to establish the HTTP connection
	try
	{
		//	Create the new session
		if((pSession = GetSession()) == 0)
			return;

		m_DeleteTreatment.strUrl = m_strDeleteTreatmentScript;
		m_DeleteTreatment.strTreatmentId = m_pXmlTreatment->m_strId;

		//	Format the request string
		m_DeleteTreatment.strRequest.Format("%s?treatment_id=%s", 
											m_strDeleteTreatmentScript,
											m_pXmlTreatment->m_strId);
		//	Open the HTTP connection
		pFile = (CHttpFile*)pSession->OpenURL(m_DeleteTreatment.strRequest);
		ASSERT(pFile);

		//	Store the status code
		pFile->QueryInfoStatusCode(m_DeleteTreatment.dwStatusCode);

		//	Save the response header
		pFile->QueryInfo(HTTP_QUERY_RAW_HEADERS_CRLF, 
						 m_DeleteTreatment.strResponseHeader);

		//	Save the reponse body
		m_DeleteTreatment.strResponse.Empty();
		while(pFile->ReadString(strResponse))
			m_DeleteTreatment.strResponse += strResponse;

		//	Check the status code from the server
		switch(m_DeleteTreatment.dwStatusCode)
		{
			case TMXML_DELETE_TREATMENT_SUCCESS:

				//	Remove the treatment from the list
				m_pXmlPage->m_Treatments.Remove(m_pXmlTreatment, TRUE);
				m_pXmlTreatment = 0;

				//	Switch to the base image
				ViewPage(m_pXmlPage);
				break;

			case TMXML_DELETE_TREATMENT_ASP_NOT_FOUND:

				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_DELETE_TREATMENT_ASP_NOT_FOUND,
									  m_DeleteTreatment.strUrl);
				break;

			case TMXML_DELETE_TREATMENT_BAD_REQUEST:

				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_DELETE_TREATMENT_BAD_REQUEST,
									  m_DeleteTreatment.strTreatmentId);
				break;

			case TMXML_DELETE_TREATMENT_OBJECT_NOT_FOUND:

				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_DELETE_TREATMENT_OBJECT_NOT_FOUND,
									  m_DeleteTreatment.strTreatmentId);
				break;

			case TMXML_DELETE_TREATMENT_NO_DATABASE:

				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_DELETE_TREATMENT_NO_DATABASE,
									  m_DeleteTreatment.strUrl);
				break;
		}

	}
	catch (CInternetException* pEx)
	{
		//	Should we display an error message?
		if(m_pErrors)
		{
			char szError[256];

			if(pEx->GetErrorMessage(szError, sizeof(szError)))
				m_pErrors->Handle(0, IDS_TMXML_SESSION_FAILED, szError);
			else
				m_pErrors->Handle(0, IDS_TMXML_SESSION_FAILED, "An unknown error occurred");
		}
		pEx->Delete();
	}

	if(pSession)
		delete pSession;
	if(pFile)
		delete pFile;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnDestroy()
//
// 	Description:	This function traps all WM_DESTROY messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnDestroy() 
{
	//	Update the ini file
	WriteIniFile();

	//	Save the page navigation bar configuration if it's visible when this
	//	object gets destroyed
	if(m_PageBar.GetVisible())
		m_PageBar.Save(m_strIniFilename, TMXML_INI_PAGEBAR_SECTION);

	CDialog::OnDestroy();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnDiagnostics()
//
// 	Description:	This function is called when the user clicks on Diagnostics
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnDiagnostics() 
{
	CDiagnostics Diagnostics;

	//	Initialize the tmx page
	Diagnostics.m_Tmx.m_strFilename  = m_strXmlFilename;
	Diagnostics.m_Tmx.m_strSource    = m_strSource;
	Diagnostics.m_Tmx.m_pXmlDocument = m_pXmlDocument;

	//	Initialize the treatment page
	Diagnostics.m_Treatment.m_pPutTreatment = &m_PutTreatment;
	Diagnostics.m_Treatment.m_strTarget = m_strPutTreatmentScript;
	Diagnostics.m_Treatment.m_pXmlFrame = this;

	//	Initialize the revisions page
	Diagnostics.m_Revisions.m_pTMXmlCtrl = this->m_pControl;

	//	Show the property sheet
	m_pControl->PreModalDialog();
	Diagnostics.DoModal();
	m_pControl->PostModalDialog();

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnDownloadComplete()
//
// 	Description:	This function is called when the window receives 
//					notification that a download operation has completed.
//
// 	Returns:		None
//
//	Notes:			The only time this message should be received is for
//					retrieving files for printing.
//
//==============================================================================
void CXmlFrame::OnDownloadComplete(LPARAM lParam)
{
	//	Was the operation aborted?
	if(theDownload.bAbort)
	{
		//	Stop retrieving files and make sure the print progress dialog is
		//	no longer visible
		ShowPrintProgress(FALSE);
		return;
	}

	//	Did an error occur?
	if(theDownload.bError)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, theDownload.strErrorMsg);

		//	Make sure there's no attempt to retrieve treatments if the error
		//	occurred on the base image
		if((CXmlPage*)lParam == m_pXmlPrintPage)
			m_pXmlPrintTreatment = 0;
	}
	else
	{
		//	Print the downloaded file
		if((CXmlPage*)lParam == m_pXmlPrintPage)
		{
			//	Save the cached filename for printing treatments
			m_strPrintPage = theDownload.strCached;

			AddPrintRequest(m_pXmlPrintPage, m_strPrintPage, 0, 0);
		}
		else if((CXmlTreatment*)lParam == m_pXmlPrintTreatment)
		{
			AddPrintRequest(m_pXmlPrintTreatment->m_pXmlPage, 
							m_strPrintPage, m_pXmlPrintTreatment, 
							theDownload.strCached);
		}
		else
		{
			ASSERT(0);
		}

		//	New page?
		if(m_BatchJob.GetCount() >= m_iFilesPerPage)
		{
			//	Update the progress dialog
			m_lPrintPage++;
			if(m_pPrintProgress)
				m_pPrintProgress->SetPage(m_lPrintPage);
			
			//	Should we go ahead and print now?
			if(m_ViewerOptions.bCombinePrintPages == FALSE)
				PrintBatch();
		}
	}

	//	Get the next file to be printed
	if(!GetNextPrintFile((void*)lParam))
	{
		//	Are there any requests still pending
		if(m_BatchJob.GetCount() > 0)
		{
			PrintBatch();
		}

		//	Update the progress dialog
		if(m_pPrintProgress)
			m_pPrintProgress->Finish();

		//	Clear the viewer if we're using it to display the printed images
		if(m_pXmlPage == 0)
			m_TMView.LoadFile("", -1);

		//	Remove the current tree from the queue
		if(m_pXmlPrintTree)
			m_PrintTrees.Remove(m_pXmlPrintTree, TRUE);

		//	Reset the pointers
		m_pXmlPrintTree = 0;
		m_pXmlPrintMedia = 0;
		m_pXmlPrintPage = 0;
		m_pXmlPrintTreatment = 0;

		//	Print the next tree in the queue
		if((m_pXmlPrintTree = m_PrintTrees.First()) != 0)
			PrintMediaTree(m_pXmlPrintTree);
		else
			ShowPrintProgress(FALSE);
	}
	else
	{
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnDownloadProgress()
//
// 	Description:	This function is called when the window receives 
//					notification that the download progress has been updated.
//
// 	Returns:		None
//
//	Notes:			Only batch printing downloads are performed asynchronously
//
//==============================================================================
void CXmlFrame::OnDownloadProgress(LPARAM lParam)
{
	//	Update the progress dialog
	if(m_pPrintProgress != 0)
		m_pPrintProgress->SetProgress(theDownload.ulProgress,
									  theDownload.ulProgressMax,
									  theDownload.strStatus);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnDraw()
//
// 	Description:	This function is called by the parent control when the 
//					frame needs to be redrawn.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnDraw() 
{
	//if(IsWindow(m_TMView.m_hWnd))
		//m_TMView.Redraw();
	//if(IsWindow(m_TMBrowse.m_hWnd))
		//m_TMBrowse.RedrawWindow();
	if(IsWindow(m_TMTool.m_hWnd))
		m_TMTool.RedrawWindow();
}
		
//==============================================================================
//
// 	Function Name:	CXmlFrame::OnDrawCommand()
//
// 	Description:	This function processes draw submenu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnDrawCommand(int iCmd) 
{
	switch(iCmd)
	{
		case TMXML_DRAW_FREEHAND:

			m_TMView.SetAnnTool(FREEHAND);
			m_TMTool.SetShapeButton(TMTB_FREEHAND);
			break;

		case TMXML_DRAW_LINE:

			m_TMView.SetAnnTool(LINE);
			m_TMTool.SetShapeButton(TMTB_LINE);
			break;

		case TMXML_DRAW_ARROW:

			m_TMView.SetAnnTool(ARROW);
			m_TMTool.SetShapeButton(TMTB_ARROW);
			break;

		case TMXML_DRAW_ELLIPSE:

			m_TMView.SetAnnTool(ELLIPSE);
			m_TMTool.SetShapeButton(TMTB_ELLIPSE);
			break;

		case TMXML_DRAW_RECTANGLE:

			m_TMView.SetAnnTool(RECTANGLE);
			m_TMTool.SetShapeButton(TMTB_RECTANGLE);
			break;

		case TMXML_DRAW_FILLEDELLIPSE:

			m_TMView.SetAnnTool(FILLED_ELLIPSE);
			m_TMTool.SetShapeButton(TMTB_FILLEDELLIPSE);
			break;

		case TMXML_DRAW_FILLEDRECTANGLE:

			m_TMView.SetAnnTool(FILLED_RECTANGLE);
			m_TMTool.SetShapeButton(TMTB_FILLEDRECTANGLE);
			break;

		case TMXML_DRAW_POLYLINE:

			m_TMView.SetAnnTool(POLYLINE);
			m_TMTool.SetShapeButton(TMTB_POLYLINE);
			break;

		case TMXML_DRAW_POLYGON:

			m_TMView.SetAnnTool(POLYGON);
			m_TMTool.SetShapeButton(TMTB_POLYGON);
			break;

		case TMXML_DRAW_TEXT:

			m_TMView.SetAnnTool(ANNTEXT);
			m_TMTool.SetShapeButton(TMTB_ANNTEXT);
			break;

		default:

			ASSERT(0);
			return;

	}

	m_TMTool.SetToolButton(TMTB_DRAWTOOL);
	m_TMView.SetAction(DRAW);
	CheckMenuTool(iCmd, FALSE);
	CheckMenuColor(FALSE);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnEraseCommand()
//
// 	Description:	This function processes erase submenu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnEraseCommand(int iCmd) 
{
	switch(iCmd)
	{
		case TMXML_ERASE_ALL:

			m_TMView.Erase(TMV_ACTIVEPANE);
			break;

		case TMXML_ERASE_SELECTIONS:

			m_TMView.DeleteSelections(TMV_ACTIVEPANE);
			break;
	
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnEvBrowseComplete()
//
// 	Description:	This function handles notifications fired when the TMBrowse
//					control finishes loading a file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnEvBrowseComplete(LPCTSTR lpszFilename) 
{
	//	Is this the file we are in the process of loading?
	if(m_strLoading.CompareNoCase(lpszFilename) == 0)
	{	
		//	Make the TMBrowse control the active viewer
		SetViewer(TMXML_VIEWER_TMBROWSE);
		
		//	Reset the menu command
		EnableMenuCommands();
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnEvButtonClick()
//
// 	Description:	This function traps all notifications fired when the user
//					clicks on one of the toolbar buttons
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnEvButtonClick(short sId, BOOL bChecked) 
{
	switch(sId)
	{
		case TMTB_NORMAL:

			OnMenuCommand(TMXML_VIEW_NORMAL);
			break;

		case TMTB_ZOOMWIDTH:

			OnMenuCommand(TMXML_VIEW_FULLWIDTH);
			break;

		case TMTB_ROTATECW:

			OnMenuCommand(TMXML_ROTATE_CW);
			break;

		case TMTB_ROTATECCW:

			OnMenuCommand(TMXML_ROTATE_CCW);
			break;

		case TMTB_ERASE:

			OnMenuCommand(TMXML_ERASE_ALL);
			break;

		case TMTB_NEXTZAP:

			OnMenuCommand(TMXML_TREATMENTS_NEXT);
			break;

		case TMTB_PREVZAP:

			OnMenuCommand(TMXML_TREATMENTS_PREVIOUS);
			break;

		case TMTB_FIRSTZAP:

			OnMenuCommand(TMXML_TREATMENTS_FIRST);
			break;

		case TMTB_LASTZAP:

			OnMenuCommand(TMXML_TREATMENTS_LAST);
			break;

		case TMTB_SAVEZAP:

			OnMenuCommand(TMXML_TREATMENTS_SAVE);
			break;

		case TMTB_UPDATEZAP:

			OnMenuCommand(TMXML_TREATMENTS_UPDATE);
			break;

		case TMTB_DELETEZAP:

			OnMenuCommand(TMXML_TREATMENTS_DELETE);
			break;

		case TMTB_NEXTPAGE:

			OnMenuCommand(TMXML_PAGES_NEXT);
			break;

		case TMTB_PREVPAGE:

			OnMenuCommand(TMXML_PAGES_PREVIOUS);
			break;

		case TMTB_FIRSTPAGE:

			OnMenuCommand(TMXML_PAGES_FIRST);
			break;

		case TMTB_LASTPAGE:

			OnMenuCommand(TMXML_PAGES_LAST);
			break;

		case TMTB_DELETEANN:

			OnMenuCommand(TMXML_ERASE_SELECTIONS);
			break;

		case TMTB_ZOOM:

			OnMenuCommand(TMXML_TOOL_ZOOM);
			break;

		case TMTB_ZOOMRESTRICTED:

			OnMenuCommand(TMXML_TOOL_ZOOM_RESTRICTED);
			break;

		case TMTB_PAN:

			OnMenuCommand(TMXML_TOOL_PAN);
			break;

		case TMTB_DRAWTOOL:

			//	Is the user toggling the drawing tool?
			if(m_TMView.GetAction() == DRAW)
			{
				//	Turn off annotations
				m_TMView.SetAction(NONE);
				m_TMTool.SetToolButton(-1);
				m_TMTool.SetColorButton(-1);
				CheckMenuTool(0, TRUE);
				CheckMenuColor(TRUE);
			}
			else
			{
				//	Select the appropriate tool
				OnMenuCommand(GetMenuToolCmd(m_TMView.GetAnnTool()));
			}
			break;

		case TMTB_HIGHLIGHT:

			OnMenuCommand(TMXML_TOOL_HIGHLIGHT);
			break;

		case TMTB_REDACT:

			OnMenuCommand(TMXML_TOOL_REDACT);
			break;

		case TMTB_SELECT:

			OnMenuCommand(TMXML_TOOL_SELECT);
			break;

		case TMTB_CALLOUT:

			OnMenuCommand(TMXML_TOOL_CALLOUT);
			break;

		case TMTB_FREEHAND:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_FREEHAND);
			break;

		case TMTB_LINE:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_LINE);
			break;

		case TMTB_ARROW:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_ARROW);
			break;

		case TMTB_ELLIPSE:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_ELLIPSE);
			break;

		case TMTB_RECTANGLE:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_RECTANGLE);
			break;

		case TMTB_FILLEDELLIPSE:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_FILLEDELLIPSE);
			break;

		case TMTB_FILLEDRECTANGLE:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_FILLEDRECTANGLE);
			break;

		case TMTB_POLYLINE:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_POLYLINE);
			break;

		case TMTB_POLYGON:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_POLYGON);
			break;

		case TMTB_ANNTEXT:

			if(bChecked)
				OnMenuCommand(TMXML_DRAW_TEXT);
			break;

		case TMTB_PRINT:

			OnMenuCommand(TMXML_PRINT_CURRENT);
			break;

		case TMTB_RED:
		case TMTB_GREEN:
		case TMTB_BLUE:
		case TMTB_YELLOW:
		case TMTB_BLACK:
		case TMTB_WHITE:
		case TMTB_DARKRED:
		case TMTB_DARKGREEN:
		case TMTB_DARKBLUE:
		case TMTB_LIGHTRED:
		case TMTB_LIGHTGREEN:
		case TMTB_LIGHTBLUE:

			if(bChecked)
				OnMenuCommand(GetMenuColorCmd(sId));
			break;

	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnEvCloseTextBox()
//
// 	Description:	This function handles notifications from the TMView control
//					when it closes the text annotation dialog
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnEvCloseTextBox(short sPane) 
{
	//	Force a redrawing of the TMView control
	m_TMView.Redraw();	
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnEvReconfigure()
//
// 	Description:	This function traps all notifications fired by the TMTool
//					control when the user changes its configuration.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnEvReconfigure() 
{
	//	Reposition the control windows
	RecalcLayout();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnEvViewClick()
//
// 	Description:	This function handles notifications fired by the TMView
//					control when the user clicks in its window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnEvViewClick(short Button, short Key) 
{
	POINT	Pos;
	int		iCmd;

	if((Button == RIGHT_MOUSEBUTTON) && (m_PopupMenu.GetSafeHmenu() != 0))
	{
		//	Get the current mouse position
		GetCursorPos(&Pos);

		//	Make sure the color selection is up to date
		CheckMenuColor(FALSE);

		//	Display the popup menu
		iCmd = TrackPopupMenuEx(m_PopupMenu.GetSafeHmenu(), 
						        TPM_RETURNCMD, Pos.x, Pos.y, m_hWnd, NULL);
								
		if(iCmd > 0)
			OnMenuCommand(iCmd);	
	}
	
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnInitDialog()
//
// 	Description:	This function traps all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================

BOOL CXmlFrame::OnInitDialog() 
{
	//	Do the base class processing first
	CDialog::OnInitDialog();
	
	//	Attach to Internet Explorer application if running as embedded object
	if(m_bEmbedded)
		GetExplorer();

	//	Create the window used to display batch printing progress
	m_pPrintProgress = new CPrintProgress(this);

	//	Build the specification for the ini file containing the configuration
	//	information
	m_strIniFilename.Format("%s%s", m_pControl->GetFolder(), TMXML_INI_FILENAME);

	//	Build the file extension list
	FillExtensions();

	//	Read the options from the ini file
	ReadIniFile();

	//	Should we override the printer stored in the ini file with the
	//	printer currently defined for the session?
	if(theApp.m_strPrinter.GetLength() > 0)
		lstrcpyn(m_ViewerOptions.szPrinter, theApp.m_strPrinter,
				 sizeof(m_ViewerOptions.szPrinter));

	//	Set the list of templates used for batch printing
	m_TMPrint.SetPrintTemplates((long)&m_Templates);

	//	Initialize the toolbar
	InitToolbar();

	//	Initialize the page navigation window. 
	//
	//	NOTE:	This has to be done AFTER initializing the toolbar so that it
	//			can use some of the toolbar properties
	InitPageBar();

	//	Initialize the controls
	ApplyOptions();

	//	Load the custom wait cursor
	m_hWaitCursor = theApp.LoadCursor(IDC_WAIT_ARROW);
	m_hStandardCursor = theApp.LoadStandardCursor(IDC_ARROW);

	//	Build the popup menu
	BuildPopup();

	//	Make sure everything is positioned properly
	RecalcLayout();

	//	Set the initial viewer
	SetViewer(TMXML_VIEWER_TMVIEW);

	//	Set initial action to Zoom
	OnMenuCommand(TMXML_TOOL_ZOOM);

	//	Enable/disable the menu commands and toolbar buttons
	EnableMenuCommands();

	//	Make sure the correct color button is checked
	CheckMenuColor(FALSE);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnMediaCommand()
//
// 	Description:	This function processes media menu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnMediaCommand(int iCmd) 
{
	CXmlMedia* pMedia = 0;

	//	We have to have a valid media tree object
	if(m_pXmlMediaTree == 0)
		return;

	switch(iCmd)
	{
		case TMXML_MEDIA_FIRST:

			pMedia = m_pXmlMediaTree->First();
			break;

		case TMXML_MEDIA_NEXT:

			pMedia = m_pXmlMediaTree->Next();
			break;

		case TMXML_MEDIA_PREVIOUS:

			pMedia = m_pXmlMediaTree->Prev();
			break;

		case TMXML_MEDIA_LAST:

			pMedia = m_pXmlMediaTree->Last();
			break;
	
	}//	switch(iCmd)

	if(pMedia)
		ViewMedia(pMedia);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnMediaTreeCommand()
//
// 	Description:	This function processes media tree menu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnMediaTreeCommand(int iCmd) 
{
	CXmlMediaTree* pTree = 0;

	switch(iCmd)
	{
		case TMXML_MEDIATREES_FIRST:

			pTree = m_MediaTrees.First();
			break;

		case TMXML_MEDIATREES_NEXT:

			pTree = m_MediaTrees.Next();
			break;

		case TMXML_MEDIATREES_PREVIOUS:

			pTree = m_MediaTrees.Prev();
			break;

		case TMXML_MEDIATREES_LAST:

			pTree = m_MediaTrees.Last();
			break;
	
	}//	switch(iCmd)

	if(pTree)
		ViewMediaTree(pTree);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnMenuCommand()
//
// 	Description:	This function processes menu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnMenuCommand(int iCmd) 
{
	switch(iCmd)
	{
		case TMXML_MEDIATREES_FIRST:
		case TMXML_MEDIATREES_NEXT:
		case TMXML_MEDIATREES_PREVIOUS:
		case TMXML_MEDIATREES_LAST:

			OnMediaTreeCommand(iCmd);
			break;

		case TMXML_MEDIA_FIRST:
		case TMXML_MEDIA_NEXT:
		case TMXML_MEDIA_PREVIOUS:
		case TMXML_MEDIA_LAST:

			OnMediaCommand(iCmd);
			break;

		case TMXML_PAGES_FIRST:
		case TMXML_PAGES_NEXT:
		case TMXML_PAGES_PREVIOUS:
		case TMXML_PAGES_LAST:

			OnPageCommand(iCmd);
			break;

		case TMXML_TREATMENTS_FIRST:
		case TMXML_TREATMENTS_LAST:
		case TMXML_TREATMENTS_PREVIOUS:
		case TMXML_TREATMENTS_NEXT:

			OnTreatmentCommand(iCmd);
			break;

		case TMXML_TREATMENTS_SAVE:

			OnSaveTreatment();
			break;

		case TMXML_TREATMENTS_UPDATE:

			OnUpdateTreatment();
			break;

		case TMXML_TREATMENTS_DELETE:

			OnDeleteTreatment();
			break;

		case TMXML_VIEW_NORMAL:
		case TMXML_VIEW_FULLWIDTH:

			OnViewCommand(iCmd);
			break;

		case TMXML_ROTATE_CW:
		case TMXML_ROTATE_CCW:

			OnRotateCommand(iCmd);
			break;

		case TMXML_ERASE_ALL:
		case TMXML_ERASE_SELECTIONS:

			OnEraseCommand(iCmd);
			break;

		case TMXML_PRINT_CURRENT:

			if(m_iViewer == TMXML_VIEWER_TMVIEW)
				m_TMView.Print(TRUE, TMV_ACTIVEPANE);
			break;

		case TMXML_PRINT_SELECTIONS:

			OnPrintSelections();
			break;

		case TMXML_PRINT_DOCUMENT:

			OnPrintDocument();
			break;

		case TMXML_PRINT_SETUP:

			//	Open the preferences page to allow the user to set the printer
			OnPreferences(2);			
			break;

		case TMXML_TOOL_ZOOM:
		case TMXML_TOOL_ZOOM_RESTRICTED:
		case TMXML_TOOL_PAN:
		case TMXML_TOOL_HIGHLIGHT:
		case TMXML_TOOL_REDACT:
		case TMXML_TOOL_CALLOUT:
		case TMXML_TOOL_SELECT:
		
			OnToolCommand(iCmd);
			break;
		
		case TMXML_DRAW_FREEHAND:
		case TMXML_DRAW_LINE:
		case TMXML_DRAW_ARROW:
		case TMXML_DRAW_ELLIPSE:
		case TMXML_DRAW_RECTANGLE:
		case TMXML_DRAW_FILLEDELLIPSE:
		case TMXML_DRAW_FILLEDRECTANGLE:
		case TMXML_DRAW_POLYLINE:
		case TMXML_DRAW_POLYGON:
		case TMXML_DRAW_TEXT:

			OnDrawCommand(iCmd);
			break;

		case TMXML_COLOR_BLACK:
		case TMXML_COLOR_RED:
		case TMXML_COLOR_GREEN:
		case TMXML_COLOR_BLUE:
		case TMXML_COLOR_YELLOW:
		case TMXML_COLOR_MAGENTA:
		case TMXML_COLOR_CYAN:
		case TMXML_COLOR_GREY:
		case TMXML_COLOR_WHITE:
		case TMXML_COLOR_DARKRED:
		case TMXML_COLOR_DARKGREEN:
		case TMXML_COLOR_DARKBLUE:
		case TMXML_COLOR_LIGHTRED:
		case TMXML_COLOR_LIGHTGREEN:
		case TMXML_COLOR_LIGHTBLUE:

			OnColorCommand(iCmd);
			break;

		case TMXML_POPUP_PREFERENCES:

			OnPreferences();
			break;

		case TMXML_POPUP_PROPERTIES:

			OnProperties();
			break;

		case TMXML_POPUP_DIAGNOSTICS:

			OnDiagnostics();
			break;
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnPageBarClick()
//
// 	Description:	This function is called by the page navigation window when
//					the user clicks on one of the toolbar buttons
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnPageBarClick(short sId, BOOL bChecked) 
{
	//	Make it appear as though the user clicked on the frame's toolbar
	OnEvButtonClick(sId, bChecked);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnPageBarChange()
//
// 	Description:	This function is called by the page navigation window when
//					the user changes the selection in the drop down list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnPageBarChange(CXmlPage* pXmlPage) 
{
	//	Make sure the specified page is in the list associated with the
	//	current media object
	if((m_pXmlMedia != 0) && (m_pXmlMedia->m_Pages.Find(pXmlPage) != NULL))
	{
		SetXmlPage(pXmlPage);
	}
	else
	{
		ASSERT(0);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnPageCommand()
//
// 	Description:	This function processes page menu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnPageCommand(int iCmd) 
{
	CXmlPage* pPage = 0;

	//	We have to have a valid media object
	if(m_pXmlMedia == 0)
		return;

	switch(iCmd)
	{
		case TMXML_PAGES_FIRST:

			pPage = m_pXmlMedia->m_Pages.First();
			break;

		case TMXML_PAGES_NEXT:

			pPage = m_pXmlMedia->m_Pages.Next();
			break;

		case TMXML_PAGES_PREVIOUS:

			pPage = m_pXmlMedia->m_Pages.Prev();
			break;

		case TMXML_PAGES_LAST:

			pPage = m_pXmlMedia->m_Pages.Last();
			break;
	
	}//	switch(iCmd)

	if(pPage)
	{
		if(ViewPage(pPage) == FALSE)
			OnViewError();
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnPostRequest()
//
// 	Description:	This function is called by the treatment diagnostics page
//					to post the existing put treatment request to the specified
//					server.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::OnPostRequest(LPCSTR lpUrl)
{
	ASSERT(lpUrl);
	ASSERT(lstrlen(lpUrl) > 0);
	if((lpUrl == 0) || (lstrlen(lpUrl) == 0))
		return FALSE;
		
	//	Do we have an existing request?
	if(m_PutTreatment.lpRequest != 0)
	{
		m_PutTreatment.strUrl = lpUrl;
		return PutTreatment();
	}
	else
	{
		return FALSE;
	}
}
 
//==============================================================================
//
// 	Function Name:	CXmlFrame::OnPreferences()
//
// 	Description:	This function is called when the user clicks on Preferences
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnPreferences(int iPage) 
{
	CPreferences Preferences(this, iPage);

	m_pControl->PreModalDialog();
	
	//	Initialize the viewer page
	Preferences.m_Viewer.m_bEnableErrors = m_ViewerOptions.bEnableErrors;
	Preferences.m_Viewer.m_bFloatProgress = m_ViewerOptions.bFloatPrintProgress;
	Preferences.m_Viewer.m_bConfirmBatch = m_ViewerOptions.bConfirmBatch;
	Preferences.m_Viewer.m_bShowPageNavigation = m_ViewerOptions.bShowPageNavigation;
	Preferences.m_Viewer.m_fProgressDelay = m_ViewerOptions.fProgressDelay;
	Preferences.m_Viewer.m_iConnection = m_ViewerOptions.iConnection;
	Preferences.m_Viewer.m_uInternetPort = m_ViewerOptions.uInternetPort;
	Preferences.m_Viewer.m_uProxyPort = m_ViewerOptions.uProxyPort;
	Preferences.m_Viewer.m_strProxyAddress = m_ViewerOptions.szProxyServer;

	//	Initialize the tools page
	Preferences.m_Tools.SetOptions(&m_ToolOptions);

	//	Initialize the printer page
	Preferences.m_Printer.m_strPrinter = m_ViewerOptions.szPrinter;
	Preferences.m_Printer.m_bCurrentSession = TRUE;	//	Force user to select FALSE
	Preferences.m_Printer.m_pTemplates  = &m_Templates;
	Preferences.m_Printer.m_strTemplate = m_ViewerOptions.szTemplate;
	Preferences.m_Printer.m_iCopies = m_ViewerOptions.iCopies;
	Preferences.m_Printer.m_bCollate = m_ViewerOptions.bCollate;
	Preferences.m_Printer.m_bMinimizeColorDepth = m_ViewerOptions.bMinimizeColorDepth;
	Preferences.m_Printer.m_bCombinePrintPages = m_ViewerOptions.bCombinePrintPages;

	//	Initialize the toolbar page
	Preferences.m_Toolbar.SetToolbar(&m_TMTool);

	//	Open the property sheet
	if(Preferences.DoModal() == IDOK)
	{
		//	Reset the viewer options
		m_ViewerOptions.fProgressDelay = Preferences.m_Viewer.m_fProgressDelay;
		m_ViewerOptions.bFloatPrintProgress = Preferences.m_Viewer.m_bFloatProgress;
		m_ViewerOptions.bConfirmBatch = Preferences.m_Viewer.m_bConfirmBatch;
		m_ViewerOptions.bEnableErrors = Preferences.m_Viewer.m_bEnableErrors;
		m_ViewerOptions.bShowPageNavigation = Preferences.m_Viewer.m_bShowPageNavigation;
		m_ViewerOptions.bCurrentSession = Preferences.m_Printer.m_bCurrentSession;
		m_ViewerOptions.bCollate = Preferences.m_Printer.m_bCollate;
		m_ViewerOptions.bMinimizeColorDepth = Preferences.m_Printer.m_bMinimizeColorDepth;
		m_ViewerOptions.bCombinePrintPages = Preferences.m_Printer.m_bCombinePrintPages;
		m_ViewerOptions.iCopies = Preferences.m_Printer.m_iCopies;
		m_ViewerOptions.iConnection = Preferences.m_Viewer.m_iConnection;
		m_ViewerOptions.uInternetPort = Preferences.m_Viewer.m_uInternetPort;
		m_ViewerOptions.uProxyPort = Preferences.m_Viewer.m_uProxyPort;
		lstrcpyn(m_ViewerOptions.szTemplate, Preferences.m_Printer.m_strTemplate,
				 sizeof(m_ViewerOptions.szTemplate));
		lstrcpyn(m_ViewerOptions.szPrinter, Preferences.m_Printer.m_strPrinter,
				 sizeof(m_ViewerOptions.szPrinter));
		lstrcpyn(m_ViewerOptions.szProxyServer, Preferences.m_Viewer.m_strProxyAddress,
				 sizeof(m_ViewerOptions.szProxyServer));

		//	Reset the local tool options
		Preferences.m_Tools.GetOptions(&m_ToolOptions);

		//	Reset the controls
		ApplyOptions();

		//	Save the viewer options
		WriteIniFile();

		//	Did the user go to the toolbar page?
		if(Preferences.m_Toolbar.GetInitialized())
		{
			//	Disable automatic reset until we reset all the properties
			m_TMTool.SetAutoReset(FALSE);

			//	Reset the toolbar properties
			m_TMTool.SetButtonSize(Preferences.m_Toolbar.m_iSize);
			m_TMTool.SetStyle(Preferences.m_Toolbar.m_bFlat ? TMTB_FLAT : TMTB_RAISED);
			m_TMTool.SetButtonRows(Preferences.m_Toolbar.m_sRows);
			m_TMTool.SetButtonMap(Preferences.m_Toolbar.GetButtonMap());
			m_TMTool.SetOrientation(Preferences.m_Toolbar.m_iOrientation);

			//	Reset the toolbar
			m_TMTool.Reset();
			m_TMTool.SetAutoReset(TRUE);

			//	Reset the page navigation toolbar
			if(IsWindow(m_PageBar.m_hWnd))
				m_PageBar.SetToolbarProps(m_TMTool);

			//	Save the toolbar and viewer settings
			m_TMTool.Save();
		}
		
		//	Adjust the viewer layout
		RecalcLayout();

		EnableMenuCommands();
	}
	
	m_pControl->PostModalDialog();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnPrintAbort()
//
// 	Description:	This function is called by the print progress dialog to
//					abort the current batch printing job
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnPrintAbort() 
{
	//	Set the flag to stop downloads
	theDownload.bAbort = TRUE;
	
	//	Abort the active TMPrint job and flush the queue
	m_TMPrint.Abort();
	m_TMPrint.Clear();

	//	Flush the batch job
	m_BatchJob.Flush(TRUE);

	//	Flush the local print queue
	m_pXmlPrintTree		 = 0;
	m_pXmlPrintMedia	 = 0;
	m_pXmlPrintPage		 = 0;
	m_pXmlPrintTreatment = 0;
	m_PrintTrees.Flush(TRUE);
	
	//	Get rid of the print progress dialog
	ShowPrintProgress(FALSE);		
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnPrintDocument()
//
// 	Description:	This function will print all pages in the current document
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnPrintDocument() 
{
	CXmlMediaTree*	pPrintTree;
	CXmlPage*		pPage;
	CXmlPage*		pPrintPage;
	CXmlTreatment*	pTreatment;
	CXmlTreatment*	pPrintTreatment;
	CXmlMedia*		pPrintMedia;
	CString			strPrompt;
	BOOL			bTreatments = FALSE;
	POSITION		Pages;
	POSITION		Treatments;

	//	We have to have a media object selected
	if(m_pXmlMedia == 0)
		return;

	//	Prompt the user to determine if we should include the treatments
	strPrompt.LoadString(IDS_TMXML_PRINT_TREATMENTS);
	if(MessageBox(strPrompt, "", MB_YESNO | MB_ICONQUESTION) == IDYES)
		bTreatments = TRUE;

	//	Allocate a media tree to store the document
	pPrintTree = new CXmlMediaTree();
	ASSERT(pPrintTree);
	pPrintTree->m_strName.Format("Print Document %s", m_pXmlMedia->m_strId);

	//	Allocate a media object to store the pages
	pPrintMedia = new CXmlMedia(pPrintTree);
	ASSERT(pPrintMedia);
	pPrintMedia->SetAttributes(*m_pXmlMedia);

	//	Add the media object to the tree
	pPrintTree->Add(pPrintMedia);

	//	Iterate the list of pages in the current media object
	Pages = m_pXmlMedia->m_Pages.GetHeadPosition();
	while(Pages != NULL)
	{
		if((pPage = (CXmlPage*)m_pXmlMedia->m_Pages.GetNext(Pages)) != 0)
		{
			//	Make a copy of this page and add it to the print media object
			pPrintPage = new CXmlPage(pPrintMedia);
			ASSERT(pPrintPage);
			pPrintPage->SetAttributes(*pPage);

			pPrintMedia->m_Pages.Add(pPrintPage);

			//	Should we add the treatments for each page?
			if(bTreatments)
			{
				//	Iterate the list of treatments for this page
				Treatments = pPage->m_Treatments.GetHeadPosition();
				while(Treatments != NULL)
				{
					if((pTreatment = (CXmlTreatment*)pPage->m_Treatments.GetNext(Treatments)) != 0)
					{
						pPrintTreatment = new CXmlTreatment(pPrintPage);
						ASSERT(pPrintTreatment);
						pPrintTreatment->SetAttributes(*pTreatment);
						pPrintPage->m_Treatments.Add(pPrintTreatment);
					}
				}
			}
		}
	}

	//	Add the tree containing the document to the print queue
	m_PrintTrees.Add(pPrintTree);

	//	Is the print engine available?
	if(m_pXmlPrintTree == 0)
	{
		//	Start printing the first tree in the queue
		if((pPrintTree = m_PrintTrees.First()) != 0)
			PrintMediaTree(pPrintTree);
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnPrintSelections()
//
// 	Description:	This function invokes a property sheet that allows the user
//					to select and print any object(s) in the media tree list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnPrintSelections() 
{
	CSelectPrint	Select(this, m_pErrors);
	CXmlMediaTree*	pTree;

	//	Initialize the dialog box
	Select.m_pXmlMediaTrees = &m_MediaTrees;
	Select.m_pXmlMediaTree = m_pXmlMediaTree;
	Select.m_pXmlMedia = m_pXmlMedia;
	Select.m_pXmlPage = m_pXmlPage;
	Select.m_pXmlTreatment = m_pXmlTreatment;

	//	Notify the control that we are about to invoke a modal dialog
	m_pControl->PreModalDialog();

	//	Allow the user to select the pages to print
	if(Select.DoModal() == IDOK)
	{
		//	Queue all the print jobs
		pTree = Select.m_Queue.First();
		while(pTree != 0)
		{
			m_PrintTrees.Add(pTree);
			pTree = Select.m_Queue.Next();
		}

		//	Flush the selection queue without destroying the objects
		Select.m_Queue.Flush(FALSE);

		//	Is the print engine available?
		if(m_pXmlPrintTree == 0)
		{
			//	Start printing the first tree in the queue
			if((pTree = m_PrintTrees.First()) != 0)
				PrintMediaTree(pTree);
		}

	}
	else
	{
		//	Deallocate all the selections
		Select.m_Queue.Flush(TRUE);
	}

	//	Notify the control that we are done
	m_pControl->PostModalDialog();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnProperties()
//
// 	Description:	This function is called when the user clicks on Properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnProperties() 
{
	STMVImageProperties Properties;

	//	Get the image properties from the TMView control
	if(m_TMView.GetImageProperties((long)&Properties, TMV_ACTIVEPANE) != TMV_NOERROR)
		return;

	//	Initialize the image page
	m_Properties.m_Image.SetImageProperties(&Properties);

	//	Initialize the media page
	m_Properties.m_Media.m_pXmlTrees = &m_MediaTrees;

	//	Show the property sheet
	m_pControl->PreModalDialog();
	m_Properties.DoModal();
	m_pControl->PostModalDialog();

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnRotateCommand()
//
// 	Description:	This function processes rotate submenu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnRotateCommand(int iCmd) 
{
	switch(iCmd)
	{
		case TMXML_ROTATE_CW:

			m_TMView.RotateCw(TRUE, TMV_ACTIVEPANE);
			break;

		case TMXML_ROTATE_CCW:

			m_TMView.RotateCcw(TRUE, TMV_ACTIVEPANE);
			break;

	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnSaveTreatment()
//
// 	Description:	This function is called to save the current image and 
//					annotations as a new treatment.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnSaveTreatment() 
{
	CString strFilename;

	//	We have to have an active page
	ASSERT(m_pXmlPage != 0);
	if(m_pXmlPage == 0)
		return;

	theApp.DoWaitCursor(1);

	//	Assemble a name to use to store a temporary treatment file
	strFilename.Format("%s%s", m_pControl->GetFolder(), TMXML_TREATMENT_FILENAME);
	_unlink(strFilename);

	//	Save the current image/annotations to a temporary treatment file
	if(m_TMView.SaveZap(strFilename, -1) != TMV_NOERROR)
	{
		theApp.DoWaitCursor(-1);
		return;
	}

	//	Get the formatted request
	if(GetPutTreatment(strFilename, m_pXmlPage->m_strId, TMXML_PUT_TREATMENT_SAVE, 0))
	{
		ASSERT(m_PutTreatment.lpRequest);
		ASSERT(m_PutTreatment.dwLength > 0);
		
		//	Post the request to the remote server
		m_PutTreatment.strUrl = m_strPutTreatmentScript;
		if(PutTreatment())
		{
			//	Process the response
			ProcessPutTreatment();
		}
	}

	theApp.DoWaitCursor(-1);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnSize()
//
// 	Description:	This function handles all WM_SIZE messages sent to the 
//					window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnSize(UINT nType, int cx, int cy) 
{
	//	Perform the base class processing
	CDialog::OnSize(nType, cx, cy);
	
	//	Make sure everything is positioned properly
	RecalcLayout();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnToolCommand()
//
// 	Description:	This function processes tool submenu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnToolCommand(int iCmd) 
{
	short sAction = NONE;
	short sButton = -1;

	switch(iCmd)
	{
		case TMXML_TOOL_ZOOM:

			if(m_TMView.GetAction() != ZOOM || m_TMView.GetZoomToRect())
			{
				sAction = ZOOM;
				sButton = TMTB_ZOOM; 
				m_TMView.SetZoomToRect(FALSE);
			}
			break;

		case TMXML_TOOL_ZOOM_RESTRICTED:

			if(m_TMView.GetAction() != ZOOM || !m_TMView.GetZoomToRect())
			{
				sAction = ZOOM; 
				sButton = TMTB_ZOOMRESTRICTED;
				m_TMView.SetZoomToRect(TRUE);
			}
			break;

		case TMXML_TOOL_PAN:

			if(m_TMView.GetAction() != PAN)
			{
				sAction = PAN; 
				sButton = TMTB_PAN;
			}
			break;

		case TMXML_TOOL_HIGHLIGHT:

			if(m_TMView.GetAction() != HIGHLIGHT)
			{
				sAction = HIGHLIGHT; 
				sButton = TMTB_HIGHLIGHT;
			}
			break;

		case TMXML_TOOL_REDACT:

			if(m_TMView.GetAction() != REDACT)
			{
				sAction = REDACT; 
				sButton = TMTB_REDACT;
			}
			break;

		case TMXML_TOOL_CALLOUT:

			if(m_TMView.GetAction() != CALLOUT)
			{
				sAction = CALLOUT; 
				sButton = TMTB_CALLOUT;
			}
			break;

		case TMXML_TOOL_SELECT:

			if(m_TMView.GetAction() != SELECT)
			{
				sAction = SELECT; 
				sButton = TMTB_SELECT;
			}
			break;

		default:

			ASSERT(0);
			return;

	}

	//	Select the requested annotation tool
	m_TMView.SetAction(sAction);

	//	Set the toolbar button
	m_TMTool.SetToolButton(sButton);
	m_TMTool.SetZoomButton(m_TMView.GetAction() == ZOOM,
						   m_TMView.GetZoomToRect());

	//	Synchronize the menu
	CheckMenuTool(iCmd, m_TMView.GetAction() == NONE);

	//	Make sure the correct color button is checked
	//
	//	NOTE:	The color changes based on the current annotation tool
	CheckMenuColor(FALSE);

	//	Make sure the local options are up to date
	UpdateToolOptions();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnTreatmentCommand()
//
// 	Description:	This function processes treatment iteration menu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnTreatmentCommand(int iCmd) 
{
	CXmlTreatment* pTreatment = 0;

	//	We must have an active page
	if(m_pXmlPage == 0)
		return;

	switch(iCmd)
	{
		case TMXML_TREATMENTS_FIRST:

			pTreatment = m_pXmlPage->m_Treatments.First();
			break;

		case TMXML_TREATMENTS_LAST:

			pTreatment = m_pXmlPage->m_Treatments.Last();
			break;

		case TMXML_TREATMENTS_PREVIOUS:

			//	Do we have an active treatment?
			if(m_pXmlTreatment != 0)
			{
				pTreatment = m_pXmlPage->m_Treatments.Prev();
			}
			else
			{
				pTreatment = m_pXmlPage->m_Treatments.Last();
			}
			break;
			
		case TMXML_TREATMENTS_NEXT:

			//	Do we have an active treatment?
			if(m_pXmlTreatment != 0)
			{
				pTreatment = m_pXmlPage->m_Treatments.Next();
			}
			else
			{
				pTreatment = m_pXmlPage->m_Treatments.First();
			}
			break;

	}//switch(iCmd)

	//	Load the requested treatment
	if(pTreatment != 0)
	{
		if(!ViewTreatment(pTreatment))
			OnViewError();
	}
	else
	{
		//	Load the base image
		if(!ViewPage(m_pXmlPage))
			OnViewError();
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnUpdateTreatment()
//
// 	Description:	This function is called to update the current treatment
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnUpdateTreatment() 
{
	CString strFilename;

	//	We have to have an active page and treatment
	ASSERT(m_pXmlPage != 0);
	ASSERT(m_pXmlTreatment != 0);
	if((m_pXmlPage == 0) && (m_pXmlTreatment != 0))
		return;

	theApp.DoWaitCursor(1);

	//	Assemble a name to use to store a temporary treatment file
	strFilename.Format("%s%s", m_pControl->GetFolder(), TMXML_TREATMENT_FILENAME);
	_unlink(strFilename);

	//	Save the current image/annotations to a temporary treatment file
	if(m_TMView.SaveZap(strFilename, -1) != TMV_NOERROR)
	{
		theApp.DoWaitCursor(-1);
		return;
	}

	//	Get the formatted request
	if(GetPutTreatment(strFilename, m_pXmlPage->m_strId, TMXML_PUT_TREATMENT_UPDATE,
					   m_pXmlTreatment->m_strId))
	{
		ASSERT(m_PutTreatment.lpRequest);
		ASSERT(m_PutTreatment.dwLength > 0);
		
		//	Post the request to the remote server
		m_PutTreatment.strUrl = m_strPutTreatmentScript;
		if(PutTreatment())
		{
			//	Process the response
			ProcessPutTreatment();
		}
	}

	theApp.DoWaitCursor(-1);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnViewCommand()
//
// 	Description:	This function processes view submenu commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnViewCommand(int iCmd) 
{
	switch(iCmd)
	{
		case TMXML_VIEW_NORMAL:

			m_TMView.ResetZoom(TMV_ACTIVEPANE);
			break;

		case TMXML_VIEW_FULLWIDTH:

			m_TMView.ZoomFullWidth(TMV_ACTIVEPANE);
			break;
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnViewError()
//
// 	Description:	This function is called when an error occurs while viewing
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::OnViewError() 
{
	//	Make sure the media tree list is in the correct position
	if(m_pXmlMediaTree != 0)
		m_pXmlMediaTree = m_MediaTrees.SetPosition(m_pXmlMediaTree);

	//	Make sure the media list is in the correct position
	if((m_pXmlMediaTree != 0) && (m_pXmlMedia != 0))
	{
		//	Make sure the media list is in the correct position
		m_pXmlMedia = m_pXmlMediaTree->SetPosition(m_pXmlMedia);

		if((m_pXmlMedia != 0) && (m_pXmlPage != 0))
		{
			//	Make sure the page list is in the correct position
			m_pXmlPage = m_pXmlMedia->m_Pages.SetPosition(m_pXmlPage);

			if((m_pXmlPage != 0) && (m_pXmlTreatment != 0))
			{
				//	Make sure the treatment list is in the correct position
				m_pXmlTreatment = m_pXmlPage->m_Treatments.SetPosition(m_pXmlTreatment);
			}
			else
			{
				m_pXmlTreatment = 0;
			}
		}
		else
		{
			m_pXmlPage = 0;
			m_pXmlTreatment = 0;
		}
	}
	else
	{
		m_pXmlMedia = 0;
		m_pXmlPage = 0;
		m_pXmlTreatment = 0;
	}

	//	Notify the navigation window
	m_PageBar.SetXmlMedia(m_pXmlMedia);
	m_PageBar.SetXmlPage(m_pXmlPage);
	
	//	Reset the menu and toolbar states
	EnableMenuCommands();	
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::OnWMDownload()
//
// 	Description:	This function handles all WM_DOWNLOADCOMPLETE messages
//
// 	Returns:		Zero
//
//	Notes:			None
//
//==============================================================================
LONG CXmlFrame::OnWMDownload(WPARAM wParam, LPARAM lParam)
{
	CString strFilename;

	switch(wParam)
	{
		//	Is the download complete?
		case TMXML_DOWNLOAD_COMPLETE:

			OnDownloadComplete(lParam);
			break;

		//	Is this a progress update?
		case TMXML_DOWNLOAD_PROGRESS:

			OnDownloadProgress(lParam);
			break;

		default:

			ASSERT(0);
			break;
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::PrintBatch()
//
// 	Description:	This function is called to do the batch printing.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::PrintBatch()
{
	CCell* pCell;

	//	Flush the TMPrint queue
	m_TMPrint.Clear();

	//	Don't bother if nothing is in the batch job
	if(m_BatchJob.GetCount() == 0)
		return TRUE;

	//	Set the job properties
	m_TMPrint.SetCopies(m_ViewerOptions.iCopies);
	m_TMPrint.SetCollate(m_ViewerOptions.bCollate);

	//	Add each cell in the batch job to the TMPrint queue
	pCell = m_BatchJob.First();
	while(pCell)
	{
		if(m_TMPrint.Add(pCell->m_strString) != TMPRINT_NOERROR)
		{
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMXML_INVALID_CELL, pCell->m_strString);
		}

		pCell = m_BatchJob.Next();
	}

	//	Flush the batch job list
	m_BatchJob.Flush(TRUE);

	//	Print the job
	m_TMPrint.Print();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::PrintMediaTree()
//
// 	Description:	This function will print all media objects in the specified
//					tree
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::PrintMediaTree(CXmlMediaTree* pTree)
{
	CString strFilename;
	float	fPages;

	ASSERT(pTree);
	
	//	Reset the current values
	m_pXmlPrintTree		 = pTree;
	m_pXmlPrintMedia	 = 0;
	m_pXmlPrintPage		 = 0;
	m_pXmlPrintTreatment = 0;
	m_iFilesPerPage		 = 0;
	m_lPrintPages		 = 0;
	m_lPrintPage		 = 0;
	m_strPrintPage.Empty();

	//	Line up on the first available page/treatment in the tree
	m_pXmlPrintMedia = m_pXmlPrintTree->First();
	while(m_pXmlPrintMedia != 0)
	{
		//	Get the first page in this media object
		if((m_pXmlPrintPage = m_pXmlPrintMedia->m_Pages.First()) != 0)
		{
			m_pXmlPrintTreatment = m_pXmlPrintPage->m_Treatments.First();
			break;
		}
		else
		{
			//	Try the next media object
			m_pXmlPrintMedia = m_pXmlPrintTree->Next();
		}
	}

	//	We must at least have a page to print
	if(m_pXmlPrintPage == 0)
	{
		m_pXmlPrintTree = 0;
		m_pXmlPrintMedia = 0;
		m_pXmlPrintTreatment = 0;
		return FALSE;
	}

	//	How many files can we fit on a single page?
	m_iFilesPerPage = m_TMPrint.GetRowsPerPage() * m_TMPrint.GetColumnsPerPage();
	ASSERT(m_iFilesPerPage != 0);
	if(m_iFilesPerPage == 0)
		m_iFilesPerPage = 1;	//	Prevents divide by zero

	//	How many pages are going to be required for the job
	fPages = ((float)m_pXmlPrintTree->GetFileCount() / (float)m_iFilesPerPage);
	m_lPrintPages = (long)(fPages + 0.5); // Round up

	//	Set the job information in the print progress dialog
	if(m_pPrintProgress)
	{
		m_pPrintProgress->Start(m_pXmlPrintTree->m_strName, m_lPrintPages);

		//	Make sure the dialog box is visible
		ShowPrintProgress(TRUE);
	}

	//	Start by downloading the page image
	if(GetFile(m_pXmlPrintPage->m_strSource, strFilename, 
			   FALSE, (LPARAM)m_pXmlPrintPage))
	{
		//	Notify the progress dialog if this is a local file
		if((m_pPrintProgress != 0) && (theDownload.bRemote == FALSE))
			m_pPrintProgress->SetFilename(theDownload.strCached);

		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ProcessPutTreatment()
//
// 	Description:	This function is called to process the response from server
//					when a request to save a new treatment is submitted.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::ProcessPutTreatment() 
{
	CIXMLDOMDocument*	pXml;
	CIXMLDOMNodeList*	pNodes = 0;
	CIXMLDOMNode*		pNode = 0;
	long				lNodes;
	BOOL				bSuccess = FALSE;

	//	We have to have a valid page for all types of actions
	ASSERT(m_pXmlPage);
	if(m_pXmlPage == 0)
		return FALSE;

	//	We have to have a valid treatment if we are updating
	if(m_PutTreatment.iAction == TMXML_PUT_TREATMENT_UPDATE)
	{
		ASSERT(m_pXmlTreatment);
		if(m_pXmlTreatment == 0)
			return FALSE;
	}

	//	Check for an error before processing the response
	switch(m_PutTreatment.dwStatusCode)
	{
		case TMXML_PUT_TREATMENT_SUCCESS:

			break;

		case TMXML_PUT_TREATMENT_ASP_NOT_FOUND:

			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMXML_PUT_TREATMENT_ASP_NOT_FOUND,
								  m_PutTreatment.strUrl);
			return FALSE;

		case TMXML_PUT_TREATMENT_BAD_REQUEST:

			if(m_pErrors)
			{
				if(m_PutTreatment.iAction == TMXML_PUT_TREATMENT_SAVE)
				{
					m_pErrors->Handle(0, IDS_TMXML_PUT_TREATMENT_INVALID_PAGEID,
									  m_PutTreatment.strPageId);
				}
				else
				{
					m_pErrors->Handle(0, IDS_TMXML_PUT_TREATMENT_INVALID_TREATMENTID,
									  m_PutTreatment.strTreatmentId);
				}
			}
			return FALSE;

		case TMXML_PUT_TREATMENT_OBJECT_NOT_FOUND:

			if(m_pErrors)
			{
				if(m_PutTreatment.iAction == TMXML_PUT_TREATMENT_SAVE)
				{
					m_pErrors->Handle(0, IDS_TMXML_PUT_TREATMENT_PAGE_NOT_FOUND,
									  m_PutTreatment.strPageId);
				}
				else
				{
					m_pErrors->Handle(0, IDS_TMXML_PUT_TREATMENT_TREATMENT_NOT_FOUND,
									  m_PutTreatment.strTreatmentId);
				}
			}
			return FALSE;

		case TMXML_PUT_TREATMENT_NO_DATABASE:

			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMXML_PUT_TREATMENT_NO_DATABASE,
								  m_PutTreatment.strUrl);
			return FALSE;

		default:

			ASSERT(0);
			break;
	}

	//	Attach to the XML parsing engine
	if((pXml = XMLAttach()) == NULL)
		return FALSE;

	//	Parse the XML returned by the request
	pXml->loadXML(m_PutTreatment.strResponse);

	//	Make sure a parsing error did not occur
	if(XMLError(pXml) == FALSE)
	{
		//	Get the list of top level child nodes
		pNodes = new CIXMLDOMNodeList(pXml->GetChildNodes());
		if(pNodes->m_lpDispatch == 0)
		{
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMXML_DOCUMENT_CHILDREN);
			
			DELETE_INTERFACE(pXml);
			
			return FALSE;
		}

		//	Get the total number of nodes in the list
		lNodes = pNodes->GetLength();

		//	Make sure we are lined up on the first node
		pNodes->reset();

		//	Iterate the list of nodes until we find the root node
		for(long i = 0; ((i < lNodes) && (pNode == 0)); i++)
		{
			pNode = new CIXMLDOMNode(pNodes->nextNode());
			ASSERT(pNode);
			ASSERT(pNode->m_lpDispatch);

			//	Is this an element?
			if(pNode->GetNodeType() == NODE_ELEMENT)
			{
				//	Is this the treatment specification?
				if(lstrcmpi(pNode->GetNodeName(), TMXML_ELEMENT_TREATMENT) != 0)
				{
					//	Delete the interface so that we keep going through
					//	the loop
					DELETE_INTERFACE(pNode);
				}

			}//if(pNode->GetNodeType() == NODE_ELEMENT)
			else
			{	
				//	Release this interface
				DELETE_INTERFACE(pNode);
			}
		}

		//	Did we find the treatment node?
		if(pNode != 0)
		{
			//	Make sure we have a valid page
			ASSERT(m_pXmlPage != 0);
			if(m_pXmlPage != 0)
			{
				//	Are we updating an existing treatment?
				if(m_PutTreatment.iAction == TMXML_PUT_TREATMENT_UPDATE)
				{
					bSuccess = XMLReadTreatment(m_pXmlPage, pNode, m_pXmlTreatment);
				}
				else
				{
					bSuccess = XMLReadTreatment(m_pXmlPage, pNode);

					//	Make sure we are positioned on the new treatment
					//
					//	NOTE:	The new treatment should be the last one in the
					//			list of treatments for the current page
					if(bSuccess)
					{
						m_pXmlTreatment = m_pXmlPage->m_Treatments.Last();
						EnableTreatmentCommands();
					}
				}
			}
			else
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_PUT_TREATMENT_NO_PAGE);
			}
		}
		else
		{
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMXML_PUT_TREATMENT_NO_ELEMENT,
								  pXml->GetXml());
		}
		
	}

	//	Clean up
	DELETE_INTERFACE(pNode);
	DELETE_INTERFACE(pNodes);
	DELETE_INTERFACE(pXml);

	return bSuccess;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::PutTreatment()
//
// 	Description:	This function is called to send a request to the specified
//					url to save the current image and annotations as a new 
//					treatment.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::PutTreatment() 
{
	CXmlSession*		pSession = 0;
	CHttpConnection*	pConnection = 0;
	CHttpFile*			pFile = 0;
	CString				strFilename;
	CString				strServer;
	CString				strObject;
	CString				strUserName;
	CString				strPassword;
	CString				strResponse;
	DWORD				dwService;
	INTERNET_PORT		iPort;
	DWORD				dwFlags;
	UINT				uErrorMsg = IDS_TMXML_OPEN_REQUEST_FAILED;
	
	ASSERT(m_PutTreatment.strUrl.GetLength() > 0);
	if(m_PutTreatment.strUrl.GetLength() == 0)
		return FALSE;

	//	Parse the url into it's component parts
	if(!AfxParseURLEx(m_PutTreatment.strUrl, dwService, strServer, strObject, 
					  iPort, strUserName, strPassword, ICU_NO_ENCODE | ICU_NO_META))
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PARSEURL_FAILED, 
							  m_PutTreatment.strUrl);
		return FALSE;
	}

	//	Create the new session
	if((pSession = GetSession()) == 0)
		return FALSE;

	//	Create the connection
	if((pConnection = GetConnection(pSession, strServer)) == 0)
	{
		delete pSession;
		return FALSE;
	}

	//	Now try to establish the FTP connection
	try
	{
		//	Are we connected to a secure server?
		if(m_bIsSecure)
		{
			dwFlags = (INTERNET_FLAG_EXISTING_CONNECT | INTERNET_FLAG_SECURE);
		}
		else
		{
			dwFlags = INTERNET_FLAG_EXISTING_CONNECT;
		}

		//	Create the request used to transfer the file
		pFile = pConnection->OpenRequest(_T("POST"), strObject, NULL, 1,
										 NULL, NULL, dwFlags);
		ASSERT(pFile);

		//	Change the error message
		uErrorMsg = IDS_TMXML_SEND_REQUEST_FAILED;
		
		//	Send the request to the server
		if(pFile->SendRequest(m_PutTreatment.strRequestHeader, 
							  m_PutTreatment.lpRequest, 
							  m_PutTreatment.dwLength))
		{
			//	Store the status code
			pFile->QueryInfoStatusCode(m_PutTreatment.dwStatusCode);

			//	Save the response header
			pFile->QueryInfo(HTTP_QUERY_RAW_HEADERS_CRLF, 
							 m_PutTreatment.strResponseHeader);

			//	Save the reponse body
			m_PutTreatment.strResponse.Empty();
			while(pFile->ReadString(strResponse))
				m_PutTreatment.strResponse += strResponse;

			//	Deallocate the Internet objects
			if(pSession)
				delete pSession;
			if(pConnection)
				delete pConnection;
			if(pFile)
				delete pFile;

			return TRUE;
		}
	}
	catch (CInternetException* pEx)
	{
		//	Should we display an error message?
		if(m_pErrors)
		{
			char szError[256];

			if(pEx->GetErrorMessage(szError, sizeof(szError)))
				m_pErrors->Handle(0, uErrorMsg, szError);
			else
				m_pErrors->Handle(0, uErrorMsg, "A system exception has occurred");
		}
		
		pEx->Delete();

		if(pSession)
			delete pSession;
		if(pConnection)
			delete pConnection;
		if(pFile)
			delete pFile;

		return FALSE;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ReadIniFile()
//
// 	Description:	This function reads the ini file and uses the values to 
//					set the viewer options.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::ReadIniFile()
{
	CTMIni Ini;

	//	Read the viewer options	
	if(Ini.Open(m_strIniFilename, TMXML_INI_VIEWER_SECTION))
	{
		m_ViewerOptions.bEnableErrors = Ini.ReadBool(TMXML_ENABLE_ERRORS_LINE, TMXML_ENABLEERRORS);
		m_ViewerOptions.bDiagnostics = Ini.ReadBool(TMXML_DIAGNOSTICS_LINE, TMXML_DIAGNOSTICS);
		m_ViewerOptions.fProgressDelay = (float)Ini.ReadDouble(TMXML_PROGRESS_DELAY_LINE, TMXML_PROGRESS_DELAY);
		m_ViewerOptions.bConfirmBatch = Ini.ReadBool(TMXML_CONFIRM_BATCH_LINE, TMXML_CONFIRM_BATCH);
		m_ViewerOptions.bFloatPrintProgress = Ini.ReadBool(TMXML_FLOAT_PRINT_PROGRESS_LINE, TMXML_FLOAT_PRINT_PROGRESS);
		m_ViewerOptions.bCollate = Ini.ReadBool(TMXML_PRINT_COLLATE_LINE, TMXML_PRINT_COLLATE);
		m_ViewerOptions.bMinimizeColorDepth = Ini.ReadBool(TMXML_MINIMIZE_COLOR_DEPTH_LINE, TMXML_MINIMIZE_COLOR_DEPTH);
		m_ViewerOptions.bCombinePrintPages = Ini.ReadBool(TMXML_COMBINE_PRINT_PAGES_LINE, TMXML_COMBINE_PRINT_PAGES);
		m_ViewerOptions.bShowPageNavigation = Ini.ReadBool(TMXML_SHOW_PAGE_NAVIGATION_LINE, TMXML_SHOW_PAGE_NAVIGATION);
		m_ViewerOptions.iCopies = Ini.ReadLong(TMXML_PRINT_COPIES_LINE, TMXML_PRINT_COPIES);
		m_ViewerOptions.iConnection = (int)Ini.ReadLong(TMXML_CONNECTION_LINE, TMXML_CONNECTION);
		m_ViewerOptions.uInternetPort = (UINT)Ini.ReadLong(TMXML_INTERNET_PORT_LINE, TMXML_INTERNET_PORT);
		m_ViewerOptions.uProxyPort = (UINT)Ini.ReadLong(TMXML_PROXY_PORT_LINE, TMXML_PROXY_PORT);
		Ini.ReadString(TMXML_PRINTER_LINE, m_ViewerOptions.szPrinter, 
					   sizeof(m_ViewerOptions.szPrinter), TMXML_PRINTER);
		Ini.ReadString(TMXML_PRINT_TEMPLATE_LINE, m_ViewerOptions.szTemplate, 
					   sizeof(m_ViewerOptions.szTemplate), TMXML_PRINT_TEMPLATE);
		Ini.ReadString(TMXML_PROXY_SERVER_LINE, m_ViewerOptions.szProxyServer, 
					   sizeof(m_ViewerOptions.szProxyServer), TMXML_PROXY_SERVER);

		//	Read the list of print templates from the file
		m_Templates.ReadFile(m_strIniFilename, TMXML_INI_TEMPLATES_SECTION, FALSE);
	}
	else
	{
		m_ViewerOptions.bEnableErrors = TMXML_ENABLEERRORS;
		m_ViewerOptions.fProgressDelay = TMXML_PROGRESS_DELAY;
		m_ViewerOptions.bConfirmBatch = TMXML_CONFIRM_BATCH;
		m_ViewerOptions.bFloatPrintProgress = TMXML_FLOAT_PRINT_PROGRESS;
		m_ViewerOptions.bCollate = TMXML_PRINT_COLLATE;
		m_ViewerOptions.bShowPageNavigation = TMXML_SHOW_PAGE_NAVIGATION;
		m_ViewerOptions.iCopies = TMXML_PRINT_COPIES;
		m_ViewerOptions.iConnection = TMXML_CONNECTION;
		m_ViewerOptions.uInternetPort = TMXML_INTERNET_PORT;
		m_ViewerOptions.uProxyPort = TMXML_PROXY_PORT;
		lstrcpy(m_ViewerOptions.szPrinter, TMXML_PRINTER);
		lstrcpy(m_ViewerOptions.szTemplate, TMXML_PRINT_TEMPLATE);
		lstrcpy(m_ViewerOptions.szProxyServer, TMXML_PROXY_SERVER);
	}

	//	Read the tool options from the ini file
	//
	//	NOTE:	This call will automatically assign the default values if
	//			the ini file could not be opened
	Ini.ReadGraphicsOptions(&m_ToolOptions);

	//	Get the default printer if none defined in the ini file
	if(lstrlen(m_ViewerOptions.szPrinter) == 0)
		lstrcpyn(m_ViewerOptions.szPrinter, m_TMView.GetDefaultPrinter(), 
				 sizeof(m_ViewerOptions.szPrinter));

	//	Add a default template if none were found in the file
	if(m_Templates.GetCount() == 0)
		m_Templates.Add(GetDefaultTemplate());
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::RecalcLayout()
//
// 	Description:	This function manages the placement of the controls on the
//					dialog box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::RecalcLayout()
{
	int		iPPHeight;
	BOOL	bPageBar = FALSE;
	RECT	rcAdjusted;

	//	Get the full client area
	GetClientRect(&m_rcClient);

	//	Make sure the controls have been created
	if(!IsWindow(m_TMView.m_hWnd) || !IsWindow(m_TMTool.m_hWnd) ||
	   !IsWindow(m_TMBrowse.m_hWnd))
		return;

	//	Set the rectangle used for the page navigation window
	if((m_pXmlDocument != 0) && m_ViewerOptions.bShowPageNavigation)
	{
		//	We are going to display the page bar
		bPageBar = TRUE;

		//	Keep the page navigation window pinned to the top
		m_rcPageBar.top    = 0;
		m_rcPageBar.left   = 0;
		m_rcPageBar.right  = m_rcClient.right;
		m_rcPageBar.bottom = m_PageBar.GetMinHeight();

		//	Adjust the client area to allow for the page navigation
		rcAdjusted.left = 0;
		rcAdjusted.right = m_rcClient.right;
		rcAdjusted.top = m_rcPageBar.bottom;
		rcAdjusted.bottom = m_rcClient.bottom;
	}
	else
	{
		//	Minimize the page navigation bar
		m_rcPageBar.top    = 0;
		m_rcPageBar.left   = 0;
		m_rcPageBar.right  = 1;
		m_rcPageBar.bottom = 1;

		//	Use the full client area for the other windows
		memcpy(&rcAdjusted, &m_rcClient, sizeof(RECT));
	}

	//	Get the height of the print progress dialog box
	if((m_pPrintProgress != 0) && IsWindow(m_pPrintProgress->m_hWnd))
		iPPHeight = m_pPrintProgress->GetHeight();
	else
		iPPHeight = 0;

	//	Make sure the toolbar is in position
	m_TMTool.ResetFrame();

	//	Make sure the print control uses the full client area
	//
	//	NOTE:	We do this so that the print status dialog will appear centered
	//			within the client area of this control
	m_TMPrint.MoveWindow(&m_rcClient);

	//	How is the toolbar oriented?
	switch(m_TMTool.GetOrientation())
	{
		case TMTB_BOTTOM:

			m_rcTMTool.left   = rcAdjusted.left;
			m_rcTMTool.right  = rcAdjusted.right;
			m_rcTMTool.bottom = rcAdjusted.bottom;
			m_rcTMTool.top	  = rcAdjusted.bottom - m_TMTool.GetBarHeight();

			m_rcPrintProgress.left   = rcAdjusted.left;
			m_rcPrintProgress.right  = rcAdjusted.right;
			m_rcPrintProgress.top    = rcAdjusted.top;
			m_rcPrintProgress.bottom = rcAdjusted.top + iPPHeight;

			m_rcViewer.left   = rcAdjusted.left;
			m_rcViewer.right  = rcAdjusted.right;
			m_rcViewer.bottom = m_rcTMTool.top - 1;

			if(m_bShowPrintProgress && !m_ViewerOptions.bFloatPrintProgress)
				m_rcViewer.top    = m_rcPrintProgress.bottom;
			else
				m_rcViewer.top    = rcAdjusted.top;

			break;

		case TMTB_LEFT:

			m_rcTMTool.top    = rcAdjusted.top;
			m_rcTMTool.bottom = rcAdjusted.bottom;
			m_rcTMTool.left   = rcAdjusted.left;
			m_rcTMTool.right  = rcAdjusted.left + m_TMTool.GetBarWidth();

			m_rcPrintProgress.left   = m_rcTMTool.right;
			m_rcPrintProgress.right  = rcAdjusted.right;
			m_rcPrintProgress.bottom = rcAdjusted.bottom;
			m_rcPrintProgress.top    = rcAdjusted.bottom - iPPHeight;

			m_rcViewer.left   = m_rcTMTool.right;
			m_rcViewer.right  = rcAdjusted.right;
			m_rcViewer.top    = rcAdjusted.top;
			
			if(m_bShowPrintProgress && !m_ViewerOptions.bFloatPrintProgress)
				m_rcViewer.bottom = m_rcPrintProgress.top;
			else
				m_rcViewer.bottom = rcAdjusted.bottom;

			break;

		case TMTB_RIGHT:

			m_rcTMTool.top    = rcAdjusted.top;
			m_rcTMTool.bottom = rcAdjusted.bottom;
			m_rcTMTool.right  = rcAdjusted.right;
			m_rcTMTool.left   = rcAdjusted.right - m_TMTool.GetBarWidth();

			m_rcPrintProgress.left   = rcAdjusted.left;
			m_rcPrintProgress.right  = m_rcTMTool.left;
			m_rcPrintProgress.bottom = rcAdjusted.bottom;
			m_rcPrintProgress.top    = rcAdjusted.bottom - iPPHeight;

			m_rcViewer.left   = rcAdjusted.left;
			m_rcViewer.right  = m_rcTMTool.left;
			m_rcViewer.top    = rcAdjusted.top;
			
			if(m_bShowPrintProgress && !m_ViewerOptions.bFloatPrintProgress)
				m_rcViewer.bottom = m_rcPrintProgress.top;
			else
				m_rcViewer.bottom = rcAdjusted.bottom;

			break;

		case TMTB_TOP:
		default:

			m_rcTMTool.left   = rcAdjusted.left;
			m_rcTMTool.right  = rcAdjusted.right;
			m_rcTMTool.top	  = rcAdjusted.top;
			m_rcTMTool.bottom = m_rcTMTool.top + m_TMTool.GetBarHeight();

			m_rcPrintProgress.left   = rcAdjusted.left;
			m_rcPrintProgress.right  = rcAdjusted.right;
			m_rcPrintProgress.bottom = rcAdjusted.bottom;
			m_rcPrintProgress.top    = rcAdjusted.bottom - iPPHeight;

			m_rcViewer.left   = rcAdjusted.left;
			m_rcViewer.right  = rcAdjusted.right;
			m_rcViewer.top    = m_rcTMTool.bottom + 1;

			if(m_bShowPrintProgress && !m_ViewerOptions.bFloatPrintProgress)
				m_rcViewer.bottom = m_rcPrintProgress.top;
			else
				m_rcViewer.bottom = rcAdjusted.bottom;

			//	This should never happen
			if(m_TMTool.GetOrientation() != TMTB_TOP)
			{
				ASSERT(1);
				m_TMTool.MoveWindow(&m_rcTMTool);
			}
			break;
	}

	//	Move the controls into position
	m_TMTool.MoveWindow(&m_rcTMTool);
	m_TMView.MoveWindow(&m_rcViewer);

	if(bPageBar)
	{
		//	Make sure it's in the correct position
		m_PageBar.MoveWindow(&m_rcPageBar);

		//	Make sure it's visible
		m_PageBar.ShowWindow(SW_SHOW);
		m_PageBar.BringWindowToTop();

	}
	else
	{
		//	Is the page bar visible?
		if(m_PageBar.IsWindowVisible())
		{
			//	Save the configuration
			m_PageBar.Save(m_strIniFilename, TMXML_INI_PAGEBAR_SECTION);

			//	Hide the window
			m_PageBar.ShowWindow(SW_HIDE);
			m_PageBar.MoveWindow(&m_rcPageBar);
		}
	}

	//	Set the flag used by the frame when it gets destroyed
	m_PageBar.SetVisible(bPageBar);

	//	Only resize the browser control if it is currently the active viewer
	if(m_iViewer == TMXML_VIEWER_TMBROWSE)
		m_TMBrowse.MoveWindow(&m_rcViewer);

	if((m_pPrintProgress != 0) && IsWindow(m_pPrintProgress->m_hWnd))
		m_pPrintProgress->MoveWindow(&m_rcPrintProgress);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ReleaseAll()
//
// 	Description:	This function is called to release all dispatch interfaces
//					and flush all local lists.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::ReleaseAll() 
{
	//	Reset the list pointers	
	//
	//	NOTE:	Do this first to prevent any iteration before deallocating the
	//			local objects
	m_pXmlMediaTree = 0;
	m_pXmlMedia = 0;
	m_pXmlPage = 0;
	m_pXmlTreatment = 0;
	m_pXmlPrintAction = 0;
	m_pXmlPrintTree = 0;
	m_pXmlPrintTreatment = 0;
	m_pXmlPrintPage = 0;
	m_pXmlPrintMedia = 0;
	m_strLoading.Empty();

	//	Is the window still valid?
	if(IsWindow(m_hWnd))
		EnableMenuCommands();

	//	Deallocate all dynamic objects
	m_PrintTrees.Flush(TRUE);
	m_ViewActions.Flush(FALSE);
	m_PrintActions.Flush(FALSE);
	m_MediaTrees.Flush(TRUE);
	m_XmlActions.Flush(TRUE);
	if(m_pXmlSettings)
	{
		delete m_pXmlSettings;
		m_pXmlSettings = 0;
	}

	//	Release the XML interface
	if(m_pXmlDocument)
	{
		DELETE_INTERFACE(m_pXmlDocument);
		m_pXmlDocument = 0;
	}
	
	//	Reset the put treatment transfer buffer
	FreePutTreatment();
}	

//==============================================================================
//
// 	Function Name:	CXmlFrame::ServiceToString()
//
// 	Description:	This function will convert the internet service identifier
//					to the appropriate string value.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::ServiceToString(DWORD dwService, CString& rService)
{
	switch(dwService)
	{
		case AFX_INET_SERVICE_HTTP:		rService = "http://";
										break;
		case AFX_INET_SERVICE_HTTPS:	rService = "https://";
										break;
		case AFX_INET_SERVICE_FTP:		rService = "ftp://";
										break;
		case AFX_INET_SERVICE_GOPHER:	rService = "gopher://";
										break;
		case AFX_INET_SERVICE_FILE:		rService = "file://";
										break;
		default:						rService.Empty();
										break;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::SetBackColor()
//
// 	Description:	This function is called to set the background color of the
//					dialog box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::SetBackColor(COLORREF crBackground) 
{
	//	Save the new color
	m_crBackground = crBackground;

	//	Create a new brush
	if(m_pBackground) delete m_pBackground;
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(crBackground);

	//	Set the background color
	m_TMView.SetBackColor((OLE_COLOR)m_crBackground);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::SetFloatPrintProgress()
//
// 	Description:	This function is called to set the flag that controls 
//					whether the print progress dialog is docked or floating
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::SetFloatPrintProgress(BOOL bFloat) 
{
	//	Has the value changed?
	if(m_ViewerOptions.bFloatPrintProgress != bFloat)
	{
		m_ViewerOptions.bFloatPrintProgress = bFloat;

		//	Reposition the windows
		RecalcLayout();
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::SetPaths()
//
// 	Description:	This function is called to set the root folder path using
//					the filename specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::SetPaths(CString& rSource) 
{
	CString			strServer;
	CString			strObject;
	CString			strFilename;
	CString			strCasebook;
	DWORD			dwService;
	CString			strUserName;
	CString			strPassword;
	INTERNET_PORT	iPort;

	//	Reset the existing paths
	m_strSource = rSource;
	m_strRelative.Empty();
	m_strAbsolute.Empty();
	m_strGetXmlScript.Empty();
	m_strPutTreatmentScript.Empty();
	m_strDeleteTreatmentScript.Empty();
	m_strXmlFilename.Empty();
	m_bIsRemote = FALSE;
	m_bIsSecure = FALSE;

	//	Stop here if no file provided
	if(rSource.GetLength() == 0)
		return;

	//	Parse the filename/url into it's component parts
	AfxParseURLEx(rSource, dwService, strServer, strObject, iPort,
				  strUserName, strPassword, ICU_NO_ENCODE | ICU_NO_META);

	//	Is this a local file specification?
	if(dwService == AFX_INET_SERVICE_FILE)
	{
		//	The absolute path is the drive root
		ExtractDrive(strObject, m_strAbsolute);

		//	Strip the filename to get the relative path
		m_strRelative = strObject;
		StripFilename(m_strRelative);

		//	Reset the filename in case we have removed a file:// prefix
		rSource = strObject;
	}
	else
	{
		//	Construct the absolute path
		ServiceToString(dwService, m_strAbsolute);
		m_strAbsolute += strServer;
		
		//	Build the relative path
		m_strRelative = rSource;
		StripFilename(m_strRelative);

		//	Files are stored on a remote server
		m_bIsRemote = TRUE;

		//	Are we connecting to a secure server?
		if(dwService == AFX_INET_SERVICE_HTTPS)
			m_bIsSecure = TRUE;
			
		//	Assemble the URL for the RingTail scripts
		ExtractCasebook(strObject, strCasebook);
		if(strCasebook.GetLength() > 0)
		{
			//	Assemble the paths to the Ringtail scripts
			m_strGetXmlScript.Format("%s/%s/%s", m_strAbsolute, strCasebook,
								     TMXML_RINGTAIL_GET_XML);
			m_strPutTreatmentScript.Format("%s/%s/%s", m_strAbsolute, strCasebook,
								           TMXML_RINGTAIL_PUT_TREATMENT);
			m_strDeleteTreatmentScript.Format("%s/%s/%s", m_strAbsolute, strCasebook,
								              TMXML_RINGTAIL_DELETE_TREATMENT);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::SetViewer()
//
// 	Description:	This function is called to set the current viewer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::SetViewer(int iViewer) 
{
	//	Has the viewer changed?
	if(m_iViewer == iViewer) return;

	//	Which viewer are we activating?
	switch(iViewer)
	{
		case TMXML_VIEWER_TMVIEW:

			//	Activate the TMView control
			if(IsWindow(m_TMView.m_hWnd))
			{
				m_TMView.ShowWindow(SW_SHOW);
				m_TMView.BringWindowToTop();

				//	Move the browser outside the client area
				//
				//	NOTE:	We do this because of a bug in the Microsoft web
				//			browsing control. If the control is not visible, 
				//			it does not fire certain events
				if(IsWindow(m_TMBrowse.m_hWnd))
				{
					m_TMBrowse.Load("");
					m_TMBrowse.MoveWindow(m_rcClient.right + 1,
										  m_rcViewer.top,
										  m_rcViewer.right - m_rcViewer.left,
										  m_rcViewer.bottom - m_rcViewer.top);
				}
				m_iViewer = TMXML_VIEWER_TMVIEW;
			}
			break;

		case TMXML_VIEWER_TMBROWSE:

			//	Move the browser into position
			if(IsWindow(m_TMBrowse.m_hWnd))
			{
				m_TMBrowse.MoveWindow(&m_rcViewer);
				m_TMBrowse.BringWindowToTop();

				if(IsWindow(m_TMView.m_hWnd))
				{
					m_TMView.LoadFile("", -1);
					m_TMView.ShowWindow(SW_HIDE);
				}
				m_iViewer = TMXML_VIEWER_TMBROWSE;
			}
			break;

		case TMXML_VIEWER_UNKNOWN:
		default:

			//	Hide both viewers
			if(IsWindow(m_TMView.m_hWnd))
			{
				m_TMView.ShowWindow(SW_HIDE);
			}
			if(IsWindow(m_TMBrowse.m_hWnd))
			{
				m_TMBrowse.MoveWindow(m_rcClient.right + 1,
									  m_rcViewer.top,
									  m_rcViewer.right - m_rcViewer.left,
									  m_rcViewer.bottom - m_rcViewer.top);
			}
			m_iViewer = TMXML_VIEWER_UNKNOWN;

			break;
	}

}

//==============================================================================
//
// 	Function Name:	CXmlFrame::SetWaitCursor()
//
// 	Description:	This function is called to show/hide the custom wait cursor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::SetWaitCursor(BOOL bWait) 
{
	//	Do we have the custom cursor?
	if((m_hWaitCursor != 0) && (m_hStandardCursor != 0))
	{
		SetCursor(bWait ? m_hWaitCursor : m_hStandardCursor);
	}
	else
	{
		theApp.DoWaitCursor(bWait ? 1 : -1);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ShowPrintProgress()
//
// 	Description:	This function is called to show/hide the progress dialog
//					for batch printing
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::ShowPrintProgress(BOOL bShow) 
{
	//	Don't bother if the window hasn't been created
	if((m_pPrintProgress == 0) || !IsWindow(m_pPrintProgress->m_hWnd))
		return;

	//	Are we supposed to show the window?
	if(bShow && (m_pPrintProgress->IsWindowVisible() == FALSE))
	{
		m_bShowPrintProgress = TRUE;

		//	Do we need to reposition the viewer?
		if(!m_ViewerOptions.bFloatPrintProgress)
			RecalcLayout();

		//	Show the progress dialog
		m_pPrintProgress->ShowWindow(SW_SHOW);
		m_pPrintProgress->RedrawWindow();
		m_pPrintProgress->BringWindowToTop();

	}
	else if(!bShow && (m_pPrintProgress->IsWindowVisible() == TRUE))
	{
		m_bShowPrintProgress = FALSE;

		//	Do we need to reposition the viewer?
		if(!m_ViewerOptions.bFloatPrintProgress)
			RecalcLayout();

		//	Hide the progress dialog
		m_TMView.BringWindowToTop();
		m_pPrintProgress->ShowWindow(SW_HIDE);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::StripFilename()
//
// 	Description:	This function is called to remove the filename from the
//					specified path.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::StripFilename(CString& rPath)
{
	LPSTR	lpszPath;
	int		i = 0;

	lpszPath = rPath.GetBuffer(rPath.GetLength() + 1);
	
	for(i = lstrlen(lpszPath) - 1; i >= 0; i--)
	{
		if(IsDirSeparator(lpszPath[i]))
			break;
	}

	if(i >= 0)
		lpszPath[i + 1] = '\0';

	rPath.ReleaseBuffer();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::UpdateServerPage()
//
// 	Description:	This function is called to notify the server that a new
//					page in the current media object has been loaded.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::UpdateServerPage(CXmlPage* pPage)
{
	CString	strCommand;

	ASSERT(pPage);

	//	If this page is located on a remote server send the command
	//	to make sure the server knows what page we are on
	if((m_bIsRemote == TRUE) && (m_pXmlPage->m_lPosition > 0))
	{
		strCommand.Format("if(parent && parent.pageHeader) { parent.pageHeader.selectPage(%s); }", m_pXmlPage->m_strNumber);
		ExecuteJavascript(strCommand);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::UpdateToolOptions()
//
// 	Description:	This function is called to update the tool options using
//					the current TMView settings.
//
// 	Returns:		None
//
//	Notes:			This function is necessary because some options can be 
//					changed via the preferences page and the popup menu.
//
//==============================================================================
void CXmlFrame::UpdateToolOptions()
{
	m_ToolOptions.sAnnColor = m_TMView.GetAnnColor();
	m_ToolOptions.sHighlightColor = m_TMView.GetHighlightColor();
	m_ToolOptions.sRedactColor = m_TMView.GetRedactColor();
	m_ToolOptions.sCalloutColor = m_TMView.GetCalloutColor();
	m_ToolOptions.sCalloutFrameColor = m_TMView.GetCalloutFrameColor();
	m_ToolOptions.sAnnThickness = m_TMView.GetAnnThickness();
	m_ToolOptions.sMaxZoom = m_TMView.GetMaxZoom();
	m_ToolOptions.sCalloutFrameThickness = m_TMView.GetCalloutFrameThickness();
	m_ToolOptions.sBitonalScaling = m_TMView.GetBitonalScaling();
	m_ToolOptions.sAnnTool = m_TMView.GetAnnTool();
	m_ToolOptions.sAnnFontSize = m_TMView.GetAnnFontSize();
	m_ToolOptions.bAnnFontBold = m_TMView.GetAnnFontBold();
	m_ToolOptions.bAnnFontStrikeThrough = m_TMView.GetAnnFontStrikeThrough();
	m_ToolOptions.bAnnFontUnderline = m_TMView.GetAnnFontUnderline();
	m_ToolOptions.strAnnFontName = m_TMView.GetAnnFontName();
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ViewMedia()
//
// 	Description:	This function is called to view the media object specified
//					by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::ViewMedia(CXmlMedia* pMedia) 
{
	CXmlPage* pPage = 0;

	ASSERT(pMedia);
	if(pMedia == 0)
		return FALSE;

	//	Notify the navigation window that we have a new document
	//
	//	NOTE:	We do this before loading the page so that the navigation
	//			window can populate its page list
	m_PageBar.SetXmlMedia(pMedia);

	//	Get the first page in the media object
	pPage = pMedia->m_Pages.First();

	if(pPage != 0)
	{
		//	Load the page into the viewer
		if(ViewPage(pPage))
		{
			//	Save the active media object
			m_pXmlMedia = pMedia;

			//	Update the menu
			EnableMenuCommands();
			return TRUE;
		}
		else
		{
			OnViewError();
			return FALSE;
		}
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ViewMediaTree()
//
// 	Description:	This function is called to view the media tree specified
//					by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::ViewMediaTree(CXmlMediaTree* pTree) 
{
	CXmlMedia* pMedia;

	//	Use the first media tree if not specified by the caller
	if(pTree == 0)
		m_pXmlMediaTree = m_MediaTrees.First();
	else
		m_pXmlMediaTree = pTree;

	if(m_pXmlMediaTree != 0)
	{
		//	Get the first media object in the tree	
		if((pMedia = m_pXmlMediaTree->First()) != 0)
		{
			return ViewMedia(pMedia);
		}
	}

	//	Update the menu
	EnableMenuCommands();

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ViewPage()
//
// 	Description:	This function is called to load the specified page
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::ViewPage(CXmlPage* pPage)
{
	CString strFilename;
	CString	strCommand;

	ASSERT(pPage);

	//	Get the file we should load in the viewer
	if(GetFile(pPage->m_strSource, strFilename, TRUE, 0))
	{
		//	Keep track of the file we are loading
		m_strLoading = strFilename;

		//	Get the appropriate viewer to use for this file
		switch(GetViewerFromFile(strFilename))
		{
			case TMXML_VIEWER_TMVIEW:

				//	Load the file into the viewer
				if(m_TMView.LoadFile(strFilename, -1) == TMV_NOERROR)
				{
					m_pXmlPage      = pPage;
					m_pXmlTreatment = 0;
				}
				SetViewer(TMXML_VIEWER_TMVIEW);
				break;

			case TMXML_VIEWER_UNKNOWN:

				ASSERT(0);
				//	Drop through ........

			case TMXML_VIEWER_TMBROWSE:
			default:

				if(m_TMBrowse.Load(strFilename) == TMBROWSE_NOERROR)
				{
					m_pXmlPage      = pPage;
					m_pXmlTreatment = 0;
				}

				//	Delay setting the viewer until the load operation completes

				break;
		}

		//	Enable/disable the menu commands
		EnableMenuCommands();

		//	Update the page navigation window
		m_PageBar.SetXmlPage(m_pXmlPage);

		//	Make sure the server page number is in sync
		if(m_pXmlPage != 0)
			UpdateServerPage(m_pXmlPage);
		
		return TRUE;
	}

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::ViewTreatment()
//
// 	Description:	This function is called to load the specified treatment
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::ViewTreatment(CXmlTreatment* pTreatment) 
{
	CString strPage;
	CString strTreatment;

	ASSERT(m_pXmlPage);
	ASSERT(pTreatment);

	if((m_pXmlPage == 0) || (pTreatment == 0))
		return FALSE;

	//	Get the source image for the page
	if(!GetFile(m_pXmlPage->m_strSource, strPage, TRUE, 0))
		return FALSE;

	//	Get the source image for the treatment
	if(!GetFile(pTreatment->m_strSource, strTreatment, TRUE, 0))
		return FALSE;

	//	Load the treatment into the viewer
	m_TMView.LoadZap(strTreatment, TRUE, TRUE, TRUE, TMV_ACTIVEPANE, strPage);

	//	Save the pointer to the active treatment
	m_pXmlTreatment = pTreatment;

	//	Enable/disable the menu commands
	EnableTreatmentCommands();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::WriteIniFile()
//
// 	Description:	This function stores the viewer options in the ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlFrame::WriteIniFile()
{
	CTMIni Ini;

	//	Save the viewer options	
	Ini.Open(m_strIniFilename, TMXML_INI_VIEWER_SECTION);

	Ini.WriteBool(TMXML_ENABLE_ERRORS_LINE, m_ViewerOptions.bEnableErrors);
	Ini.WriteBool(TMXML_CONFIRM_BATCH_LINE, m_ViewerOptions.bConfirmBatch);
	Ini.WriteBool(TMXML_FLOAT_PRINT_PROGRESS_LINE, m_ViewerOptions.bFloatPrintProgress);
	Ini.WriteBool(TMXML_PRINT_COLLATE_LINE, m_ViewerOptions.bCollate);
	Ini.WriteBool(TMXML_MINIMIZE_COLOR_DEPTH_LINE, m_ViewerOptions.bMinimizeColorDepth);
	Ini.WriteBool(TMXML_COMBINE_PRINT_PAGES_LINE, m_ViewerOptions.bCombinePrintPages);
	Ini.WriteBool(TMXML_DIAGNOSTICS_LINE, m_ViewerOptions.bDiagnostics);
	Ini.WriteBool(TMXML_SHOW_PAGE_NAVIGATION_LINE, m_ViewerOptions.bShowPageNavigation);
	Ini.WriteLong(TMXML_PRINT_COPIES_LINE, m_ViewerOptions.iCopies);
	Ini.WriteLong(TMXML_CONNECTION_LINE, m_ViewerOptions.iConnection);
	Ini.WriteLong(TMXML_INTERNET_PORT_LINE, m_ViewerOptions.uInternetPort);
	Ini.WriteLong(TMXML_PROXY_PORT_LINE, m_ViewerOptions.uProxyPort);
	Ini.WriteDouble(TMXML_PROGRESS_DELAY_LINE, m_ViewerOptions.fProgressDelay);
	Ini.WriteString(TMXML_PRINT_TEMPLATE_LINE, m_ViewerOptions.szTemplate);
	Ini.WriteString(TMXML_PROXY_SERVER_LINE, m_ViewerOptions.szProxyServer);

	//	Save the name of the current printer if it's not in use just for the
	//	current session
	if(!m_ViewerOptions.bCurrentSession)
		Ini.WriteString(TMXML_PRINTER_LINE, m_ViewerOptions.szPrinter);

	//	Write the drawing tool options to the file
	//
	//	NOTE:	This call will change the INI section
	Ini.WriteGraphicsOptions(&m_ToolOptions);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLAttach()
//
// 	Description:	This function is called to attach to the XML engine
//
// 	Returns:		A pointer to the new XML document interface
//
//	Notes:			The caller is responsible for deallocation of the interface
//
//==============================================================================
CIXMLDOMDocument* CXmlFrame::XMLAttach() 
{
	CIXMLDOMDocument*	pDocument;
	COleException		OE;
	CLSID				ClassId;
	char				szError[256];

	//	Get the Class ID for the XML parser
	if(CLSIDFromProgID(L"Microsoft.XMLDOM", &ClassId) != S_OK)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_CLSIDFAILED, "Microsoft.XMLDOM");
		return NULL;
	}

	//	Allocate a new XML document interface
	pDocument = new CIXMLDOMDocument();
	ASSERT(pDocument);

	//	Open the interface to the XML Parser
	if(!pDocument->CreateDispatch(ClassId, &OE))
	{
		OE.GetErrorMessage(szError, sizeof(szError));
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_ATTACHFAILED, szError);
		return NULL;
	}
	else
	{ 
		//	Force synchronous loading of the file because the assumption is
		//	that it's already been downloaded to the local machine
		pDocument->SetAsync(FALSE);

		return pDocument;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLError()
//
// 	Description:	This function is called to run the RingTail script and
//					generate an XML tree.
//
// 	Returns:		TRUE if an error occurred
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLError(CIXMLDOMDocument* pDocument) 
{
	CIXMLDOMParseError*	pIError;
	BOOL				bError = FALSE;
	CString				strMsg;

	ASSERT(pDocument);

	pIError = new CIXMLDOMParseError(pDocument->GetParseError());
	ASSERT(pIError);
	ASSERT(pIError->m_lpDispatch);

	if(pIError->m_lpDispatch)
	{
		//	Did an error occur?
		if(pIError->GetErrorCode() != 0)
		{
			//	Display the error message
			if(m_pErrors)
			{
				strMsg.Format("XML Parse Error: %s - "
							  "Line: %ld Column %ld\n\nSource: %s",
						      pIError->GetReason(),
						      pIError->GetLine(),
							  pIError->GetLinepos(),
							  pIError->GetSrcText());
				m_pErrors->Handle(0, strMsg);
			}
			bError = TRUE;
		}
	}

	DELETE_INTERFACE(pIError);

	return bError;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLLoadFile()
//
// 	Description:	This function is called to load the requested file into the
//					XML parser.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLLoadFile(LPCSTR lpFilename) 
{
	VARIANT		vFilename;
	CString		strFilename;

	ASSERT(lpFilename);
	ASSERT(lstrlen(lpFilename) > 0);

	ASSERT(m_pXmlDocument == 0);
	if(m_pXmlDocument)
	{
		DELETE_INTERFACE(m_pXmlDocument);
		m_pXmlDocument = 0;
	}

	//	Attach to the XML parsing engine
	if((m_pXmlDocument = XMLAttach()) == NULL)
		return FALSE;

	//	Set up the variant used to load the parser
	strFilename = "file://";
	strFilename += lpFilename;

	VariantInit(&vFilename);
	V_VT(&vFilename) = VT_BSTR;
	V_BSTR(&vFilename) = strFilename.AllocSysString();

	//	Parse the XML file
	if(m_pXmlDocument->load(vFilename) == TRUE)
	{
		//	Release the memory used to load the XML engine
		VariantClear(&vFilename);
	
		//	Save the path to the XML file
		m_strXmlFilename = lpFilename;

		//	Process the entire document
		return XMLReadDocument(m_pXmlDocument);
	}
	else
	{
		//	Release the memory used to load the XML engine
		VariantClear(&vFilename);
	
		//	Get the error message
		XMLError(m_pXmlDocument);

		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLProcessActions()
//
// 	Description:	This function is called to process the lists of actions
//					read in from the XML file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLProcessActions() 
{
	CXmlMediaTree*	pTree;
	CXmlAction*		pAction;

	//	The file MUST contain at least one media tree
	if(m_MediaTrees.GetCount() == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PROCESS_NO_TREES); 
		return FALSE;
	}

	//	Default to viewing the first media tree if no actions were defined in
	//	the XML file
	if(m_XmlActions.GetCount() == 0)
	{
		pTree = m_MediaTrees.First();
		ASSERT(pTree);

		//	Create a default View action
		pAction = new CXmlAction();
		ASSERT(pAction);

		pAction->m_strCommand = TMXML_ACTION_COMMAND_VIEW;
		pAction->m_strTree = pTree->m_strName;
		pAction->m_strPageId.Empty();
		pAction->m_strOnComplete.Empty();

		m_XmlActions.Add(pAction);
		m_ViewActions.Add(pAction);
	}

	//	Process the View actions
	pAction = m_ViewActions.First();
	while(pAction != 0)
	{
		//	Process this View action
		//
		//	NOTE:	At this time we assume only one View action so we break out
		//			as soon as we are successful processing one of them
		if(XMLProcessView(pAction))
			break;
		else
			pAction = m_ViewActions.Next();
	}

	//	Stop here if there are no print actions
	if(m_PrintActions.GetCount() == 0)
		return TRUE;

	//	We MUST be attached to a printer
	if(m_TMPrint.IsReady() == FALSE)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_BATCH_NOT_READY); 
		return FALSE;
	}

	//	Queue all the print jobs
	pAction = m_PrintActions.First();
	while(pAction != 0)
	{
		XMLProcessPrint(pAction);
		pAction = m_PrintActions.Next();
	}

	//	Is the print engine available?
	if(m_pXmlPrintTree == 0)
	{
		//	Start printing the first tree in the queue
		if((pTree = m_PrintTrees.First()) != 0)
			PrintMediaTree(pTree);
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLProcessPrint()
//
// 	Description:	This function is called to process the Print action
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLProcessPrint(CXmlAction* pPrint) 
{
	CXmlMediaTree*	pTree = 0;
	POSITION		Pos;
	CString			strPrompt;

	ASSERT(pPrint != 0);
	ASSERT(lstrcmpi(pPrint->m_strCommand, TMXML_ACTION_COMMAND_PRINT) == 0);
	ASSERT(lstrlen(pPrint->m_strTree) > 0);

	//	Prompt the user for confirmation before printing
	if(m_ViewerOptions.bConfirmBatch)
	{
		AfxFormatString1(strPrompt, IDS_PROMPT_PRINT_TREE, pPrint->m_strTree);
		if(MessageBox(strPrompt, "Confirm", MB_ICONQUESTION | MB_YESNO) == IDNO)
			return FALSE;
	}

	//	Locate the specified media tree
	//
	//	NOTE:	We do not use the built in iterator for media trees because the
	//			user may also be viewing the trees at the same time
	Pos = m_MediaTrees.GetHeadPosition();
	while(Pos != NULL)
	{
		if((pTree = (CXmlMediaTree*)m_MediaTrees.GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pTree->m_strName, pPrint->m_strTree) == 0)
				break;
		}
		else
		{
			pTree = 0;
		}
	}
	
	//	Were we unable to locate the media tree?
	if(pTree == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PROCESS_PRINT_TREE, pPrint->m_strTree);
		return FALSE;
	}
	else
	{
		//	Add a duplicate tree to the queue
		m_PrintTrees.Add(pTree->GetDuplicate());
		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLProcessSettings()
//
// 	Description:	This function is called to process the list of settings
//					extracted from the XML file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLProcessSettings() 
{
	BOOL	bVersionError = FALSE;
	CString	strTmxVersion;
	CString	strXmlVersion;

	ASSERT(m_pXmlSettings);

	//	Should we override the absolute and relative paths?
	if(m_bIsRemote && m_pXmlSettings->m_strBaseURL.GetLength() > 0)
	{
		m_strAbsolute = m_pXmlSettings->m_strBaseURL;
		m_strRelative = m_pXmlSettings->m_strBaseURL;
	}
		
	//	Should we check the version information?
	if(m_pXmlSettings->m_iVerMajor > 0)
	{
		//	Are the major versions equal?
		if(m_pXmlSettings->m_iVerMajor == _iTmxVerMajor)
		{
			//	Can we deal with this minor version?
			if(m_pXmlSettings->m_iVerMinor <= _iTmxVerMinor)
			{
				bVersionError = FALSE;
			}
			else
			{	
				//	XML file requires higher minor version
				bVersionError = TRUE;
			}
		}

		//	Does the XML file require a higher major version?
		else if(m_pXmlSettings->m_iVerMajor > _iTmxVerMajor)
		{
			bVersionError = TRUE;
		}
		
		//	The control has a higher major version
		else
		{
			bVersionError = FALSE;
		}		
	}
	//	Don't bother checking version information
	else
	{
		bVersionError = FALSE;
	}

	//	Do we need to display an error message?
	if((bVersionError == TRUE) && (m_pErrors != 0))
	{
		strXmlVersion.Format("%d.%d", m_pXmlSettings->m_iVerMajor,
									  m_pXmlSettings->m_iVerMinor);
		strTmxVersion.Format("%d.%d", _iTmxVerMajor, _iTmxVerMinor);

		m_pErrors->Handle(0, IDS_TMXML_SETTINGS_VERSION, strXmlVersion, 
														 strTmxVersion);
	}
	 
	return (bVersionError != TRUE);
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLProcessView()
//
// 	Description:	This function is called to process the View action
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLProcessView(CXmlAction* pView) 
{
	CXmlMediaTree*	pTree = 0;
	CXmlMedia*		pMedia = 0;
	CXmlPage*		pPage = 0;

	ASSERT(pView != 0);
	ASSERT(lstrcmpi(pView->m_strCommand, TMXML_ACTION_COMMAND_VIEW) == 0);
	ASSERT(lstrlen(pView->m_strTree) > 0);

	//	NOTE:	We are going to use built in iterators in this function so that
	//			the lists are left in the correct position when we start viewing

	//	Locate the specified media tree
	pTree = m_MediaTrees.First();
	while(pTree != 0)
	{
		if(lstrcmpi(pTree->m_strName, pView->m_strTree) == 0)
			break;
		else
			pTree = m_MediaTrees.Next();
	}
	
	//	Were we unable to locate the media tree?
	if(pTree == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PROCESS_VIEW_TREE, pView->m_strTree);
		return ViewMediaTree(0);
	}

	//	Do we need to search for a specific page?
	if(pView->m_strPageId.GetLength() > 0)
	{
		//	Search each media object in the tree for the one that contains the
		//	specified page
		pMedia = pTree->First();
		while((pMedia != 0) && (pPage == 0))
		{
			//	Check each page in this media object
			pPage = pMedia->m_Pages.First();
			while(pPage != 0)
			{
				//	Is this the specified page?
				if(lstrcmpi(pPage->m_strId, pView->m_strPageId) == 0)
					break;
				else
					pPage = pMedia->m_Pages.Next();
			}

			//	Get the next media object if we haven't found the page yet
			if(pPage == 0)
				pMedia = pTree->Next();
		}
	
	}//	if(pView->m_strPageId.GetLength() > 0)

	//	Were we unable to find the specified page ?
	if((pPage == 0) && (pView->m_strPageId.GetLength() > 0))
	{
		//	Must not have been able to find the specified page
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PROCESS_VIEW_PAGE,
							  pView->m_strTree, pView->m_strPageId);
	}

	//	Load the first page if unable to find the requested page or if no page
	//	was specified as part of the action
	if(pPage == 0)
	{
		return ViewMediaTree(pTree);
	}
	else
	{
		ASSERT(pMedia != 0);

		//	Update the class references
		m_pXmlMediaTree = pTree;
		m_pXmlMedia = pMedia;

		//	Update the page navigation bar
		m_PageBar.SetXmlMedia(m_pXmlMedia);

		//	View the specified page
		return ViewPage(pPage);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadAction()
//
// 	Description:	This function is called to read the attributes for an 
//					individual action and add it to the appropriate list.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadAction(CIXMLDOMNode* pNode) 
{
	CXmlAction*				pAction;
	CIXMLDOMNamedNodeMap*	pAttributes;
	CIXMLDOMNode*			pAttribute;
	long					lAttributes;
	long					i;

	ASSERT(pNode);

	//	Allocate a new action object
	pAction = new CXmlAction();
	ASSERT(pAction);

	//	How many attributes does this element have?
	pAttributes = new CIXMLDOMNamedNodeMap(pNode->GetAttributes());
	ASSERT(pAttributes);
	lAttributes = pAttributes->GetLength();

	//	Iterate the list of attributes
	for(i = 0; i < lAttributes; i++)
	{
		pAttribute = new CIXMLDOMNode(pAttributes->GetItem(i));
		ASSERT(pAttribute);
		ASSERT(pAttribute->m_lpDispatch);

		if(pAttribute->m_lpDispatch)
		{
			if(pAction->SetAttribute(pAttribute->GetNodeName(), 
								     pAttribute->GetText()) == FALSE)
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_ACTION_ATTRIBUTE, 
									  pAttribute->GetNodeName(),
									  pAttribute->GetXml());
			}
		}

		DELETE_INTERFACE(pAttribute);
	}

	DELETE_INTERFACE(pAttributes);

	//	Are we missing any required attributes?
	if(pAction->m_strCommand.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_ACTION_NO_COMMAND, pNode->GetXml());
		delete pAction;
		return FALSE;
	}
	if(pAction->m_strTree.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_ACTION_NO_TREE, pNode->GetXml());
		delete pAction;
		return FALSE;
	}

	//	Add the action to the appropriate list
	if(lstrcmpi(pAction->m_strCommand, TMXML_ACTION_COMMAND_VIEW) == 0)
	{
		m_XmlActions.Add(pAction);
		m_ViewActions.Add(pAction);
	}
	else if(lstrcmpi(pAction->m_strCommand, TMXML_ACTION_COMMAND_PRINT) == 0)
	{
		m_XmlActions.Add(pAction);
		m_PrintActions.Add(pAction);

		//	Warn the user if a page id was specified
		if(pAction->m_strPageId.GetLength() > 0)
		{
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMXML_ACTION_PRINT_PAGE, 
								  pNode->GetXml());
		}
	}
	else
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_ACTION_BAD_COMMAND, 
							  pAction->m_strCommand, pNode->GetXml());
		delete pAction;
		return FALSE;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadActions()
//
// 	Description:	This function is called to build the list of actions using
//					the specified XML child node.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadActions(CIXMLDOMNode* pNode) 
{
	CIXMLDOMNodeList*		pChildren;
	CIXMLDOMNode*			pChild;
	long					lChildren;
	long					i;

	ASSERT(pNode);

	//	Get the list of child nodes
	pChildren = new CIXMLDOMNodeList(pNode->GetChildNodes());
	if(pChildren->m_lpDispatch != 0)
	{
		lChildren = pChildren->GetLength();

		for(i = 0; i < lChildren; i++)
		{
			pChild = new CIXMLDOMNode(pChildren->GetItem(i));
			ASSERT(pChild);
			ASSERT(pChild->m_lpDispatch);

			//	Is this an element?
			if((pChild->m_lpDispatch != 0) && (pChild->GetNodeType() == NODE_ELEMENT))
			{
				//	Is this an action object?
				if(lstrcmpi(pChild->GetNodeName(), TMXML_ELEMENT_ACTION) == 0)
				{
					XMLReadAction(pChild);
				}
				else
				{
					if(m_pErrors)
						m_pErrors->Handle(0, IDS_TMXML_ACTIONS_NODE, 
										  pChild->GetNodeName());
				}
			}
			
			DELETE_INTERFACE(pChild);
		}
	}
	DELETE_INTERFACE(pChildren);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadDocument()
//
// 	Description:	This function is called to read the XML document and use it
//					to initialize the local lists
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadDocument(CIXMLDOMDocument* pDocument) 
{
	CIXMLDOMNodeList*	pNodes;
	CIXMLDOMNode*		pNode;
	long				lNodes;
	BOOL				bSuccess = FALSE;
	BOOL				bTmxFound = FALSE;

	ASSERT(pDocument);
		
	//	Get the list of top level child nodes
	pNodes = new CIXMLDOMNodeList(pDocument->GetChildNodes());
	if(pNodes->m_lpDispatch == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_DOCUMENT_CHILDREN);
		return FALSE;
	}
	else
	{
		//	Get the total number of nodes in the list
		lNodes = pNodes->GetLength();

		//	Make sure we are lined up on the first node
		pNodes->reset();

		//	Iterate the list of nodes until we find the root node
		for(long i = 0; ((i < lNodes) && (bTmxFound == FALSE)); i++)
		{
			pNode = new CIXMLDOMNode(pNodes->nextNode());
			ASSERT(pNode);
			ASSERT(pNode->m_lpDispatch);

			//	Is this an element?
			if(pNode->GetNodeType() == NODE_ELEMENT)
			{
				//	Is this the document settings?
				if(lstrcmpi(pNode->GetNodeName(), TMXML_ELEMENT_TMXROOT) == 0)
				{
					//	We located the TMX root node
					bTmxFound = TRUE;

					//	Load the root node
					bSuccess = XMLReadTmx(pNode);
				}

			}//if(pNode->GetNodeType() == NODE_ELEMENT)
				
			//	Release this interface
			DELETE_INTERFACE(pNode);
		}

		DELETE_INTERFACE(pNodes);

		//	Were we unable to find the TMX root?
		if(bTmxFound == FALSE)
		{
			if(m_pErrors)
				m_pErrors->Handle(0, IDS_TMXML_NO_ROOT); 
		}

		return bSuccess;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadMedia()
//
// 	Description:	This function is called to create and intialize a new media
//					object using the node provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadMedia(CXmlMediaTree* pTree, CIXMLDOMNode* pNode) 
{
	CXmlMedia*				pMedia;
	CXmlPage*				pPage;
	CIXMLDOMNamedNodeMap*	pAttributes;
	CIXMLDOMNodeList*		pChildren;
	CIXMLDOMNode*			pAttribute;
	CIXMLDOMNode*			pChild;
	long					lAttributes;
	long					lChildren;
	long					i;
	long					lPosition = 1;

	ASSERT(pNode);
	ASSERT(pTree);

	//	Allocate a new media object
	pMedia = new CXmlMedia(pTree);
	ASSERT(pMedia);

	//	How many attributes does this element have?
	pAttributes = new CIXMLDOMNamedNodeMap(pNode->GetAttributes());
	ASSERT(pAttributes);
	lAttributes = pAttributes->GetLength();

	//	Iterate the list of attributes
	for(i = 0; i < lAttributes; i++)
	{
		pAttribute = new CIXMLDOMNode(pAttributes->GetItem(i));
		ASSERT(pAttribute);
		ASSERT(pAttribute->m_lpDispatch);

		if(pAttribute->m_lpDispatch)
		{
			if(pMedia->SetAttribute(pAttribute->GetNodeName(), 
								    pAttribute->GetText()) == FALSE)
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_MEDIA_ATTRIBUTE, 
									  pAttribute->GetNodeName(),
									  pAttribute->GetXml());
			}
		}

		DELETE_INTERFACE(pAttribute);
	}
	DELETE_INTERFACE(pAttributes);

	//	Are we missing any required attributes?
	if(pMedia->m_strId.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_MEDIA_ID, pNode->GetXml());
		delete pMedia;
		return FALSE;
	}

	//	Do we already have a media object with this id?
	if(pTree->Find(pMedia->m_strId) != 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_MEDIA_DUPLICATE, 
							  pMedia->m_strId, pTree->m_strName);
		delete pMedia;
		return FALSE;
	}

	//	Add the media object to the specified tree
	pTree->Add(pMedia);

	//	Get the list of child nodes
	pChildren = new CIXMLDOMNodeList(pNode->GetChildNodes());
	if(pChildren->m_lpDispatch != 0)
	{
		lChildren = pChildren->GetLength();

		for(i = 0; i < lChildren; i++)
		{
			pChild = new CIXMLDOMNode(pChildren->GetItem(i));
			ASSERT(pChild);
			ASSERT(pChild->m_lpDispatch);

			//	Is this an element?
			if((pChild->m_lpDispatch != 0) && (pChild->GetNodeType() == NODE_ELEMENT))
			{
				//	Is this a media object?
				if(lstrcmpi(pChild->GetNodeName(), TMXML_ELEMENT_PAGE) == 0)
					XMLReadPage(pMedia, pChild);
			}
			
			DELETE_INTERFACE(pChild);
		}
	}
	DELETE_INTERFACE(pChildren);

	//	Set the page position identifiers
	pPage = pMedia->m_Pages.First();
	while(pPage)
	{
		pPage->m_lPosition = lPosition;
		lPosition++;
		pPage = pMedia->m_Pages.Next();
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadMediaTree()
//
// 	Description:	This function is called to create and intialize a new media
//					tree object using the node provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadMediaTree(CIXMLDOMNode* pNode) 
{
	CXmlMediaTree*			pTree;
	CIXMLDOMNamedNodeMap*	pAttributes;
	CIXMLDOMNodeList*		pChildren;
	CIXMLDOMNode*			pAttribute;
	CIXMLDOMNode*			pChild;
	long					lAttributes;
	long					lChildren;
	long					i;

	ASSERT(pNode);

	//	Allocate a new media tree object
	pTree = new CXmlMediaTree();
	ASSERT(pTree);

	//	How many attributes does this element have?
	pAttributes = new CIXMLDOMNamedNodeMap(pNode->GetAttributes());
	ASSERT(pAttributes);
	lAttributes = pAttributes->GetLength();

	//	Iterate the list of attributes
	for(i = 0; i < lAttributes; i++)
	{
		pAttribute = new CIXMLDOMNode(pAttributes->GetItem(i));
		ASSERT(pAttribute);
		ASSERT(pAttribute->m_lpDispatch);

		if(pAttribute->m_lpDispatch)
		{
			if(pTree->SetAttribute(pAttribute->GetNodeName(), 
								   pAttribute->GetText()) == FALSE)
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_TREE_ATTRIBUTE, 
									  pAttribute->GetNodeName(),
									  pAttribute->GetXml());
			}
		}

		DELETE_INTERFACE(pAttribute);
	}

	DELETE_INTERFACE(pAttributes);

	//	Are we missing any required attributes?
	if(pTree->m_strName.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_TREE_NAME, pNode->GetXml());
		delete pTree;
		return FALSE;
	}

	//	Do we already have a media tree with this name?
	if(m_MediaTrees.Find(pTree->m_strName) != 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_TREE_DUPLICATE, pTree->m_strName);
		delete pTree;
		return FALSE;
	}

	//	Add the tree to the list
	m_MediaTrees.Add(pTree);

	//	Get the list of child nodes
	pChildren = new CIXMLDOMNodeList(pNode->GetChildNodes());
	if(pChildren->m_lpDispatch != 0)
	{
		lChildren = pChildren->GetLength();

		for(i = 0; i < lChildren; i++)
		{
			pChild = new CIXMLDOMNode(pChildren->GetItem(i));
			ASSERT(pChild);
			ASSERT(pChild->m_lpDispatch);

			//	Is this an element?
			if((pChild->m_lpDispatch != 0) && (pChild->GetNodeType() == NODE_ELEMENT))
			{
				//	Is this a media object?
				if(lstrcmpi(pChild->GetNodeName(), TMXML_ELEMENT_MEDIA) == 0)
					XMLReadMedia(pTree, pChild);
			}
			
			DELETE_INTERFACE(pChild);
		}
	}
	DELETE_INTERFACE(pChildren);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadPage()
//
// 	Description:	This function is called to create and intialize a new page
//					object using the node provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadPage(CXmlMedia* pMedia, CIXMLDOMNode* pNode) 
{
	CXmlPage*				pPage;
	CIXMLDOMNamedNodeMap*	pAttributes;
	CIXMLDOMNodeList*		pChildren;
	CIXMLDOMNode*			pAttribute;
	CIXMLDOMNode*			pChild;
	long					lAttributes;
	long					lChildren;
	long					i;

	ASSERT(pNode);
	ASSERT(pMedia);

	//	Allocate a new page object
	pPage = new CXmlPage(pMedia);
	ASSERT(pPage);

	//	How many attributes does this element have?
	pAttributes = new CIXMLDOMNamedNodeMap(pNode->GetAttributes());
	ASSERT(pAttributes);
	lAttributes = pAttributes->GetLength();

	//	Iterate the list of attributes
	for(i = 0; i < lAttributes; i++)
	{
		pAttribute = new CIXMLDOMNode(pAttributes->GetItem(i));
		ASSERT(pAttribute);
		ASSERT(pAttribute->m_lpDispatch);

		if(pAttribute->m_lpDispatch)
		{
			if(pPage->SetAttribute(pAttribute->GetNodeName(), 
								   pAttribute->GetText()) == FALSE)
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_PAGE_ATTRIBUTE, 
									  pAttribute->GetNodeName(),
									  pAttribute->GetXml());
			}
		}

		DELETE_INTERFACE(pAttribute);
	}
	DELETE_INTERFACE(pAttributes);

	//	Are we missing any required attributes?
	if(pPage->m_strId.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PAGE_ID, pNode->GetXml());
		delete pPage;
		return FALSE;
	}

	//	Do we already have a page object with this id?
	if(pMedia->m_Pages.Find(pPage->m_strId) != 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PAGE_DUPLICATE, 
							  pNode->GetXml());
		delete pPage;
		return FALSE;
	}

	if(pPage->m_strSource.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PAGE_SOURCE, pNode->GetXml());
		delete pPage;
		return FALSE;
	}

	if(pPage->m_strType.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_PAGE_TYPE, pNode->GetXml());
		delete pPage;
		return FALSE;
	}

	//	Add the page
	pMedia->m_Pages.Add(pPage);

	//	Get the list of child nodes
	pChildren = new CIXMLDOMNodeList(pNode->GetChildNodes());
	if(pChildren->m_lpDispatch != 0)
	{
		lChildren = pChildren->GetLength();

		for(i = 0; i < lChildren; i++)
		{
			pChild = new CIXMLDOMNode(pChildren->GetItem(i));
			ASSERT(pChild);
			ASSERT(pChild->m_lpDispatch);

			//	Is this an element?
			if((pChild->m_lpDispatch != 0) && (pChild->GetNodeType() == NODE_ELEMENT))
			{
				//	Is this a media object?
				if(lstrcmpi(pChild->GetNodeName(), TMXML_ELEMENT_TREATMENT) == 0)
					XMLReadTreatment(pPage, pChild);
			}
			
			DELETE_INTERFACE(pChild);
		}
	}
	DELETE_INTERFACE(pChildren);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadSetting()
//
// 	Description:	This function is called to read the attributes for an 
//					individual setting.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadSetting(CIXMLDOMNode* pNode) 
{
	CIXMLDOMNamedNodeMap*	pAttributes;
	CIXMLDOMNode*			pAttribute;
	long					lAttributes;
	long					i;

	ASSERT(pNode);

	//	The settings list should have already been allocated
	ASSERT(m_pXmlSettings);
	if(m_pXmlSettings == 0)
		return FALSE;

	//	How many attributes does this element have?
	pAttributes = new CIXMLDOMNamedNodeMap(pNode->GetAttributes());
	ASSERT(pAttributes);
	lAttributes = pAttributes->GetLength();

	//	Iterate the list of attributes
	for(i = 0; i < lAttributes; i++)
	{
		pAttribute = new CIXMLDOMNode(pAttributes->GetItem(i));
		ASSERT(pAttribute);
		ASSERT(pAttribute->m_lpDispatch);

		if(pAttribute->m_lpDispatch)
		{
			if(m_pXmlSettings->Add(pAttribute->GetNodeName(), 
								   pAttribute->GetText()) == FALSE)
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_SETTING_ATTRIBUTE, 
									  pAttribute->GetNodeName(),
									  pAttribute->GetXml());
			}
		}

		DELETE_INTERFACE(pAttribute);
	}

	DELETE_INTERFACE(pAttributes);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadSettings()
//
// 	Description:	This function is called to build the list of settings using
//					the specified XML child node.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadSettings(CIXMLDOMNode* pNode) 
{
	CIXMLDOMNodeList*	pChildren;
	CIXMLDOMNode*		pChild;
	long				lChildren;
	long				i;

	ASSERT(pNode);

	//	There should only be one list of settings per XML file
	if(m_pXmlSettings != 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_SETTINGS_MULTIPLE);
		return FALSE;
	}
	else
	{
		//	Allocate a new list of settings
		m_pXmlSettings = new CXmlSettings;
	}

	//	Get the list of child nodes
	pChildren = new CIXMLDOMNodeList(pNode->GetChildNodes());
	if(pChildren->m_lpDispatch != 0)
	{
		lChildren = pChildren->GetLength();

		for(i = 0; i < lChildren; i++)
		{
			pChild = new CIXMLDOMNode(pChildren->GetItem(i));
			ASSERT(pChild);
			ASSERT(pChild->m_lpDispatch);

			//	Is this an element?
			if((pChild->m_lpDispatch != 0) && (pChild->GetNodeType() == NODE_ELEMENT))
			{
				//	Is this a setting object?
				if(lstrcmpi(pChild->GetNodeName(), TMXML_ELEMENT_SETTING) == 0)
				{
					//	Read the setting
					XMLReadSetting(pChild);
				}
				else
				{
					if(m_pErrors)
						m_pErrors->Handle(0, IDS_TMXML_SETTINGS_NODE, 
										  pChild->GetNodeName());
				}
			}
			
			DELETE_INTERFACE(pChild);
		}
	}
	DELETE_INTERFACE(pChildren);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadTmx()
//
// 	Description:	This function is called to read the TMX root node of an
//					XML formatted document
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadTmx(CIXMLDOMNode* pNode) 
{
	CIXMLDOMNodeList*	pChildren;
	CIXMLDOMNode*		pChild;
	long				lChildren;

	ASSERT(pNode);
		
	//	Get the list of child nodes
	pChildren = new CIXMLDOMNodeList(pNode->GetChildNodes());
	if(pChildren->m_lpDispatch == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_DOCUMENT_CHILDREN);
		return FALSE;
	}
	else
	{
		//	Get the total number of nodes in the list
		lChildren = pChildren->GetLength();

		//	Make sure we are lined up on the first node
		pChildren->reset();

		//	Iterate the list of nodes
		for(long i = 0; i < lChildren; i++)
		{
			pChild = new CIXMLDOMNode(pChildren->nextNode());
			ASSERT(pChild);
			ASSERT(pChild->m_lpDispatch);

			//	Is this an element?
			if(pChild->GetNodeType() == NODE_ELEMENT)
			{
				//	Is this the document settings?
				if(lstrcmpi(pChild->GetNodeName(), TMXML_ELEMENT_SETTINGS) == 0)
				{
					XMLReadSettings(pChild);

					//	Process the settings right away in case the versions
					//	do not match
					if(!XMLProcessSettings())
					{
						DELETE_INTERFACE(pChild);
						DELETE_INTERFACE(pChildren);
						return FALSE;
					}
				}

				//	Is this the action list?
				else if(lstrcmpi(pChild->GetNodeName(), TMXML_ELEMENT_ACTIONS) == 0)
				{
					XMLReadActions(pChild);
				}

				//	Is this a media tree
				else if(lstrcmpi(pChild->GetNodeName(), TMXML_ELEMENT_MEDIA_TREE) == 0)
				{
					XMLReadMediaTree(pChild);
				}

				//	This is an invalid root node
				else
				{
					if(m_pErrors)
						m_pErrors->Handle(0, IDS_TMXML_ROOT_NODE, 
										  pChild->GetNodeName());
				}

			}//if(pChild->GetNodeType() == NODE_ELEMENT)
				
			//	Release this interface
			DELETE_INTERFACE(pChild);
		}

		DELETE_INTERFACE(pChildren);

		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLReadTreatment()
//
// 	Description:	This function is called to create and intialize a new 
//					treatment object using the node provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLReadTreatment(CXmlPage* pPage, CIXMLDOMNode* pNode,
								 CXmlTreatment* pUpdate) 
{
	CXmlTreatment*			pTreatment;
	CIXMLDOMNamedNodeMap*	pAttributes;
	CIXMLDOMNode*			pAttribute;
	long					lAttributes;
	long					i;

	ASSERT(pNode);
	ASSERT(pPage);

	//	Allocate a new treatment object
	pTreatment = new CXmlTreatment(pPage);
	ASSERT(pTreatment);

	//	How many attributes does this element have?
	pAttributes = new CIXMLDOMNamedNodeMap(pNode->GetAttributes());
	ASSERT(pAttributes);
	lAttributes = pAttributes->GetLength();

	//	Iterate the list of attributes
	for(i = 0; i < lAttributes; i++)
	{
		pAttribute = new CIXMLDOMNode(pAttributes->GetItem(i));
		ASSERT(pAttribute);
		ASSERT(pAttribute->m_lpDispatch);

		if(pAttribute->m_lpDispatch)
		{
			if(pTreatment->SetAttribute(pAttribute->GetNodeName(), 
								        pAttribute->GetText()) == FALSE)
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_TREATMENT_ATTRIBUTE, 
									  pAttribute->GetNodeName(),
									  pAttribute->GetXml());
			}
		}

		DELETE_INTERFACE(pAttribute);
	}
	DELETE_INTERFACE(pAttributes);

	//	Try to extract the treatment id from the source attribute if an id
	//	was not provided as part of the XML specification
	if(pTreatment->m_strId.IsEmpty())
		ExtractTreatmentId(pTreatment);

	//	Are we missing any required attributes?
	if(pTreatment->m_strId.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_TREATMENT_ID, pNode->GetXml());
		delete pTreatment;
		return FALSE;
	}

	if(pTreatment->m_strSource.IsEmpty())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_TREATMENT_SOURCE, pNode->GetXml());
		delete pTreatment;
		return FALSE;
	}

	//	Are we updating an existing treatment?
	if(pUpdate != 0)
	{
		//	Copy the new attributes
		pUpdate->SetAttributes(*pTreatment);
		delete pTreatment;
	}
	else
	{
		//	Add the treatment object to the specified page
		pPage->m_Treatments.Add(pTreatment);
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLRequest()
//
// 	Description:	This function is called to run the RingTail script and
//					generate an XML tree.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLRequest(LPCSTR lpTarget) 
{
	CIXMLHttpRequest*	pIRequest;
	VARIANT				vAsync;
	VARIANT				vNull;
	VARIANT				vDOM;
	COleException		OE;
	CLSID				ClassId;
	char				szError[256];
	CString				strNull;
	BOOL				bSuccess = FALSE;

	ASSERT(lpTarget);

	//	Get the Class ID for the XML parser
	if(CLSIDFromProgID(L"Microsoft.XMLHTTP", &ClassId) != S_OK)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_CLSIDFAILED, "Microsoft.XMLHTTP");
		return FALSE;
	}

	//	Allocate a new XML HTTP request interface
	pIRequest = new CIXMLHttpRequest();
	ASSERT(pIRequest);

	//	Open the interface to the XML engine
	if(!pIRequest->CreateDispatch(ClassId, &OE))
	{
		OE.GetErrorMessage(szError, sizeof(szError));
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_ATTACH_REQUEST, szError);
		
		DELETE_INTERFACE(pIRequest);
		return FALSE;
	}

	//	Release the existing XML document
	if(m_pXmlDocument)
	{
		DELETE_INTERFACE(m_pXmlDocument);
		m_pXmlDocument = 0;
	}

	//	Attach to a new XML document
	if((m_pXmlDocument = XMLAttach()) == NULL)
	{
		DELETE_INTERFACE(pIRequest);
		return FALSE;
	}

	try
	{
		//	Initialize the parameters
		strNull.Empty();

		VariantInit(&vAsync);
		V_VT(&vAsync) = VT_BOOL;
		V_BOOL(&vAsync) = FALSE;

		VariantInit(&vNull);
		V_VT(&vNull) = VT_BSTR;
		V_BSTR(&vNull) = strNull.AllocSysString();

		VariantInit(&vDOM);
		V_VT(&vDOM) = VT_DISPATCH;
		V_DISPATCH(&vDOM) = m_pXmlDocument->m_lpDispatch;

		//	Open the connection and send the request
		pIRequest->open(_T("GET"), lpTarget, vAsync, vNull, vNull);
		pIRequest->send(vDOM);

		//	Did the request get processed OK?
		if(pIRequest->GetStatus() == TMXML_REQUEST_SUCCESS)
		{
			//CString M;
			//M = pIRequest->getAllResponseHeaders();
			//MessageBox(M);
			bSuccess = TRUE;

			m_pXmlDocument->loadXML(pIRequest->GetResponseText());

			//	Make sure a parsing error did not occur
			if(XMLError(m_pXmlDocument) == FALSE)
			{
				bSuccess = XMLReadDocument(m_pXmlDocument);
			}

		}
		else
		{
			//	Were we unable to find the file?
			if(pIRequest->GetStatus() == TMXML_REQUEST_NOTFOUND)
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_REQUEST_NOTFOUND, lpTarget);
			}
			else
			{
				if(m_pErrors)
					m_pErrors->Handle(0, IDS_TMXML_REQUEST_ERROR);
			}
		}

	}
	catch(...)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMXML_REQUEST_ERROR);
	}

	//	Did an error occur?
	if(bSuccess == FALSE)
	{
		//	Release the XML interface
		if(m_pXmlDocument)
		{
			DELETE_INTERFACE(m_pXmlDocument);
			m_pXmlDocument = 0;
		}
	}	

	//	Clean up
	VariantClear(&vAsync);
	VariantClear(&vNull);
	VariantClear(&vDOM);
	DELETE_INTERFACE(pIRequest);

	return bSuccess;
}

//==============================================================================
//
// 	Function Name:	CXmlFrame::XMLRunScript()
//
// 	Description:	This function is called to run the specified script and
//					load the resultant XML file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlFrame::XMLRunScript(LPCSTR lpUrl)
{
	CString strRequest;
	CString strCached;
	CString	strParameter;

	ASSERT(lpUrl);
	ASSERT(lstrlen(lpUrl) > 0);

	//	Encode the parameter string
	strParameter = m_strSource;
	Encode(strParameter);

	//	Build the request string
	strRequest.Format("%s?src=%s", lpUrl, strParameter);

	//	Now download the XML formatted file 
	if(Download(strRequest, strCached, FALSE, TRUE, 0))
	{
		//	Load the file into the parser
		if(XMLLoadFile(strCached))
		{
			//	Should we display the page navigation window?
			if(m_ViewerOptions.bShowPageNavigation)
			{
				if(IsWindow(m_PageBar.m_hWnd) && !m_PageBar.IsWindowVisible())
					RecalcLayout();
			}

			//	Process the actions contained in the file
			return XMLProcessActions();
		}
		else
		{
			return FALSE;
		}

	}
	else
	{
		return FALSE;
	}
}


