//==============================================================================
//
// File Name:	options.cpp
//
// Description:	This file contains member functions of the COptions class.
//
// See Also:	options.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-15-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <options.h>
#include <tmprint.h>
#include <tmprdefs.h>
#include <select.h>

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

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(COptions, CDialog)
	//{{AFX_MSG_MAP(COptions)
	ON_LBN_SELCHANGE(IDC_TEMPLATES, OnTemplateChange)
	ON_BN_CLICKED(IDC_PRINTHELP, OnHelp)
	ON_BN_CLICKED(IDC_PRINT, OnPrint)
	ON_BN_CLICKED(IDC_RELOAD, OnReload)
	ON_WM_CTLCOLOR()
	ON_BN_CLICKED(IDC_FONTS, OnFonts)
	ON_BN_CLICKED(IDC_BORDER_COLOR, OnBorderColor)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	COptions::Abort()
//
// 	Description:	This function is called to abort the current print job.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::Abort() 
{
	m_bAborted = TRUE;
}

//==============================================================================
//
// 	Function Name:	COptions::Add()
//
// 	Description:	This function will add a new cell to the queue based on the
//					string specification provided by the caller.
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short COptions::Add(LPCSTR lpString)
{
	char	szBuffer[2048];
	CCell*	pCell;

	ASSERT(lpString);

	//	Copy the caller's string to a working buffer
	lstrcpyn(szBuffer, lpString, sizeof(szBuffer));

	//	Allocate a new cell
	pCell = new CCell();

	//	Initialize the cell using the string
	if(!pCell->SetFromString(szBuffer))
	{
		delete pCell;
		return TMPRINT_INVALIDSTRING;
	}

	//	Add the cell to the queue
	m_Queue.Lock();
	m_Queue.Add(pCell);
	m_Queue.Unlock();

	//	Update the controls
	SetControlStates();

	return TMPRINT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	COptions::COptions()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
COptions::COptions(CTMPrintCtrl* pControl, CErrorHandler* pErrors) 
		 :CDialog(COptions::IDD, pControl)
{
	//{{AFX_DATA_INIT(COptions)
	m_bBarcode = FALSE;
	m_bFilename = FALSE;
	m_bGraphic = FALSE;
	m_bImage = FALSE;
	m_bIncludePath = FALSE;
	m_bPrintBorder = FALSE;
	m_bIncludeTotal = FALSE;
	m_bPageNum = FALSE;
	m_bName = FALSE;
	m_bDeponent = FALSE;
	m_bCollate = FALSE;
	m_sCopies = 0;
	m_bForceNewPage = FALSE;
	m_bUseSlideId = FALSE;
	m_strBarcodeCharacter = _T("");
	m_strBarcodeFont = _T("");
	m_strJobName = _T("");
	m_bShowStatus = FALSE;
	m_fPrintBorderThickness = 0.0f;
	m_fLeftMargin = 0.0f;
	m_bPrintCalloutBorders = FALSE;
	m_bPrintCallouts = FALSE;
	m_fTopMargin = 0.0f;
	m_strPrinter = _T("");
	m_bAutoRotate = FALSE;
	m_bForeignBarcode = FALSE;
	m_bSourceBarcode = FALSE;
	m_bInsertSlipSheet = FALSE;
	//}}AFX_DATA_INIT

	m_pTMPrint = pControl;
	m_pErrors = pErrors;
	m_pTemplate = 0;
	m_iSaveFormat = 0;
	m_strIniFilespec.Empty();
	m_strIniSection.Empty();
	m_strTemplateSection.Empty();
	m_bEnablePowerPoint = FALSE;
 	m_bDIBPrintingEnabled = TRUE;
	m_brRed.CreateSolidBrush(RGB(255,0,0));
	m_crPrintBorderColor = RGB(0,0,0);
	m_sCalloutFrameColor = 0;
}

//==============================================================================
//
// 	Function Name:	COptions::~COptions()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
COptions::~COptions()
{
	//	Flush the list of cells
	m_Queue.Flush(TRUE);

	//	Flush the list of templates
	m_Templates.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	COptions::Create()
//
// 	Description:	This is an overloaded version of the base class member. It
//					will create the options window.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL COptions::Create() 
{
	ASSERT(m_pTMPrint);
	if(!m_pTMPrint) return FALSE;

	//	Create the dialog box
	return CDialog::Create(COptions::IDD, m_pTMPrint);
}

//==============================================================================
//
// 	Function Name:	COptions::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					class members and dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(COptions)
	DDX_Control(pDX, IDC_INSERTSLIPSHEET, m_ctrlInsertSlipSheet);
	DDX_Control(pDX, IDC_SOURCEBARCODE, m_ctrlSourceBarcode);
	DDX_Control(pDX, IDC_FOREIGNBARCODE, m_ctrlForeignBarcode);
	DDX_Control(pDX, IDC_AUTOROTATE, m_ctrlAutoRotate);
	DDX_Control(pDX, IDC_PAGESERIES, m_ctrlPageSeries);
	DDX_Control(pDX, IDC_INCLUDEPATH, m_ctrlIncludePath);
	DDX_Control(pDX, IDC_PRINTBORDER, m_ctrlPrintBorder);
	DDX_Control(pDX, IDC_PRINTERS, m_ctrlPrinters);
	DDX_Control(pDX, IDC_PRINTCALLOUTS, m_ctrlPrintCallouts);
	DDX_Control(pDX, IDC_PRINTCALLOUTBORDERS, m_ctrlPrintCalloutBorders);
	DDX_Control(pDX, IDC_TOPMARGIN, m_ctrlTopMargin);
	DDX_Control(pDX, IDC_LEFTMARGIN, m_ctrlLeftMargin);
	DDX_Control(pDX, IDC_BORDER_COLOR, m_ctrlBorderColor);
	DDX_Control(pDX, IDC_CALLOUTBORDERTHICKNESS, m_ctrlPrintBorderThickness);
	DDX_Control(pDX, IDC_SHOWSTATUS, m_ctrlShowStatus);
	DDX_Control(pDX, IDC_COLLATE, m_ctrlCollate);
	DDX_Control(pDX, IDC_COPIES, m_ctrlCopies);
	DDX_Control(pDX, IDC_BARCODEFONT, m_ctrlBarcodeFont);
	DDX_Control(pDX, IDC_BARCODECHARACTER, m_ctrlBarcodeCharacter);
	DDX_Control(pDX, IDC_JOBNAME, m_ctrlJobName);
	DDX_Control(pDX, IDC_USESLIDEID, m_ctrlUseSlideId);
	DDX_Control(pDX, IDC_FORCENEWPAGE, m_ctrlForceNewPage);
	DDX_Control(pDX, IDC_HIDEDEPONENT, m_ctrlDeponent);
	DDX_Control(pDX, IDC_HIDENAME, m_ctrlName);
	DDX_Control(pDX, IDC_RELOAD, m_ctrlReload);
	DDX_Control(pDX, IDC_PAGENUM, m_ctrlPageNum);
	DDX_Control(pDX, IDC_HIDEIMAGE, m_ctrlImage);
	DDX_Control(pDX, IDC_HIDEGRAPHIC, m_ctrlGraphic);
	DDX_Control(pDX, IDC_HIDEFILENAME, m_ctrlFilename);
	DDX_Control(pDX, IDC_HIDEBARCODE, m_ctrlBarcode);
	DDX_Control(pDX, IDC_PRINT, m_ctrlPrint);
	DDX_Control(pDX, IDC_TEMPLATES, m_ctrlTemplates);
	DDX_Control(pDX, IDC_STATUSLEFT, m_ctrlLStatus);
	DDX_Check(pDX, IDC_HIDEBARCODE, m_bBarcode);
	DDX_Check(pDX, IDC_HIDEFILENAME, m_bFilename);
	DDX_Check(pDX, IDC_HIDEGRAPHIC, m_bGraphic);
	DDX_Check(pDX, IDC_HIDEIMAGE, m_bImage);
	DDX_Check(pDX, IDC_INCLUDEPATH, m_bIncludePath);
	DDX_Check(pDX, IDC_PRINTBORDER, m_bPrintBorder);
	DDX_Check(pDX, IDC_PAGESERIES, m_bIncludeTotal);
	DDX_Check(pDX, IDC_PAGENUM, m_bPageNum);
	DDX_Check(pDX, IDC_HIDENAME, m_bName);
	DDX_Check(pDX, IDC_HIDEDEPONENT, m_bDeponent);
	DDX_Check(pDX, IDC_COLLATE, m_bCollate);
	DDX_Text(pDX, IDC_COPIES, m_sCopies);
	DDX_Check(pDX, IDC_FORCENEWPAGE, m_bForceNewPage);
	DDX_Check(pDX, IDC_USESLIDEID, m_bUseSlideId);
	DDX_Text(pDX, IDC_BARCODECHARACTER, m_strBarcodeCharacter);
	DDX_Text(pDX, IDC_BARCODEFONT, m_strBarcodeFont);
	DDX_Text(pDX, IDC_JOBNAME, m_strJobName);
	DDX_Check(pDX, IDC_SHOWSTATUS, m_bShowStatus);
	DDX_Text(pDX, IDC_CALLOUTBORDERTHICKNESS, m_fPrintBorderThickness);
	DDX_Text(pDX, IDC_LEFTMARGIN, m_fLeftMargin);
	DDX_Check(pDX, IDC_PRINTCALLOUTBORDERS, m_bPrintCalloutBorders);
	DDX_Check(pDX, IDC_PRINTCALLOUTS, m_bPrintCallouts);
	DDX_Text(pDX, IDC_TOPMARGIN, m_fTopMargin);
	DDX_CBString(pDX, IDC_PRINTERS, m_strPrinter);
	DDX_Check(pDX, IDC_AUTOROTATE, m_bAutoRotate);
	DDX_Check(pDX, IDC_FOREIGNBARCODE, m_bForeignBarcode);
	DDX_Check(pDX, IDC_SOURCEBARCODE, m_bSourceBarcode);
	DDX_Check(pDX, IDC_INSERTSLIPSHEET, m_bInsertSlipSheet);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	COptions::EnableDIBPrinting()
//
// 	Description:	This function is called to set the flag that enables and
//					disables DIB printing
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::EnableDIBPrinting(BOOL bEnable) 
{
	m_bDIBPrintingEnabled = bEnable;
}

//==============================================================================
//
// 	Function Name:	COptions::EnablePowerPoint()
//
// 	Description:	This function is called to set the flag that enables and
//					disables PowerPoint support.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::EnablePowerPoint(BOOL bEnable) 
{
	m_bEnablePowerPoint = bEnable;
}

//==============================================================================
//
// 	Function Name:	COptions::FillPrinters()
//
// 	Description:	This method is called to fill the printers list box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::FillPrinters() 
{
	CObList*	pPrinters = NULL;
	POSITION	Pos = NULL;
	CString*	pPrinter = NULL;

	if((pPrinters = m_Printer.EnumPrinters()) != NULL)
	{
		Pos = pPrinters->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pPrinter = (CString*)pPrinters->GetNext(Pos)) != NULL)
			{
				m_ctrlPrinters.AddString(*pPrinter);
				delete pPrinter;
				pPrinter = NULL;
			}

		}
		
		delete pPrinters;			
	}

}

//==============================================================================
//
// 	Function Name:	COptions::FillTemplates()
//
// 	Description:	This function is called to fill the selection list with the
//					names of all available page templates
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::FillTemplates() 
{
	CTemplate* pTemplate;
	int		   i = 0;

	//	Clear the list box
	m_ctrlTemplates.ResetContent();

	//	Add each template to the list box
	pTemplate = m_Templates.GetFirstTemplate();
	while(pTemplate != 0)
	{
		//	Add the template description
		m_ctrlTemplates.AddString(pTemplate->m_strDescription);

		//	Set the object pointer
		m_ctrlTemplates.SetItemDataPtr(i++, (void*)pTemplate);

		//	Get the next template in the list
		pTemplate = m_Templates.GetNextTemplate();
	}
	
	//	Select the first template in the list
	m_ctrlTemplates.SetCurSel(0);
	OnTemplateChange();	
}

//==============================================================================
//
// 	Function Name:	COptions::Flush()
//
// 	Description:	This function is called to flush the print queue
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::Flush() 
{
	m_Queue.Lock();
	m_Queue.Flush(TRUE);
	m_Queue.Unlock();

	//	Update the controls
	SetControlStates();
}

//==============================================================================
//
// 	Function Name:	COptions::GetColumnsPerPage()
//
// 	Description:	This function is called to get the number of rows per page
//					defined by the current template.
//
// 	Returns:		The number of rows in each page
//
//	Notes:			None
//
//==============================================================================
short COptions::GetColumnsPerPage() 
{
	if(m_pTemplate != 0)
		return m_pTemplate->m_sColumns;
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	COptions::GetDefaultMask()
//
// 	Description:	This function is called to retrieve the default mask for the
//					template with the specified name
//
// 	Returns:		The default mask if found
//
//	Notes:			None
//
//==============================================================================
long COptions::GetDefaultMask(LPCSTR lpTemplate) 
{
	CTemplate* pTemplate;

	ASSERT(lpTemplate);

	if((pTemplate = m_Templates.Find(lpTemplate)) == 0)
		return 0;
	else
		return pTemplate->GetDefaultMask();
}

//==============================================================================
//
// 	Function Name:	COptions::GetDefaultPrinter()
//
// 	Description:	This function is called to get the name of the default
//					printer device.
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short COptions::GetDefaultPrinter(CString& rPrinter) 
{
	m_Printer.GetDefault(rPrinter);
	return TMPRINT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	COptions::GetEnabledMask()
//
// 	Description:	This function is called to retrieve the enabled mask for the
//					template with the specified name
//
// 	Returns:		The default mask if found
//
//	Notes:			None
//
//==============================================================================
long COptions::GetEnabledMask(LPCSTR lpTemplate) 
{
	CTemplate* pTemplate;

	ASSERT(lpTemplate);

	if((pTemplate = m_Templates.Find(lpTemplate)) == 0)
		return 0;
	else
		return pTemplate->GetEnabledMask();
}

//==============================================================================
//
// 	Function Name:	COptions::GetQueueCount()
//
// 	Description:	This function is called to get the current number of cells
//					in the queue.
//
// 	Returns:		The current queue count
//
//	Notes:			None
//
//==============================================================================
long COptions::GetQueueCount() 
{
	return m_Queue.GetCount();
}

//==============================================================================
//
// 	Function Name:	COptions::GetRowsPerPage()
//
// 	Description:	This function is called to get the number of rows per page
//					defined by the current template.
//
// 	Returns:		The number of rows in each page
//
//	Notes:			None
//
//==============================================================================
short COptions::GetRowsPerPage() 
{
	if(m_pTemplate != 0)
		return m_pTemplate->m_sRows;
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	COptions::GetTemplate()
//
// 	Description:	This function is called to get the template with the 
//					specified name
//
// 	Returns:		The template if found
//
//	Notes:			None
//
//==============================================================================
CTemplate* COptions::GetTemplate(LPCSTR lpTemplate) 
{
	ASSERT(lpTemplate);

	return m_Templates.Find(lpTemplate);
}

//==============================================================================
//
// 	Function Name:	COptions::GetTemplate()
//
// 	Description:	This function is called to get the current template object.
//
// 	Returns:		The current template
//
//	Notes:			None
//
//==============================================================================
CTemplate* COptions::GetTemplate() 
{
	return m_pTemplate;
}

//==============================================================================
//
// 	Function Name:	COptions::GetTemplates()
//
// 	Description:	This function is called to get the current list of templates
//
// 	Returns:		The current template list
//
//	Notes:			None
//
//==============================================================================
CTemplates* COptions::GetTemplates() 
{
	return &m_Templates;
}

//==============================================================================
//
// 	Function Name:	COptions::OnBorderColor()
//
// 	Description:	This function is called when the user clicks on the border
//					color button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnBorderColor() 
{
	CColorDialog Dialog(m_crPrintBorderColor, CC_ANYCOLOR, this);

	if(Dialog.DoModal() == IDOK)
	{
		m_crPrintBorderColor = Dialog.GetColor();
		m_ctrlBorderColor.SetColor(m_crPrintBorderColor);
		m_ctrlBorderColor.RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	COptions::OnCtlColor()
//
// 	Description:	This function is overloaded so that we can highlight the
//					status bar when PowerPoint printing is disabled
//
// 	Returns:		A handle to the background brush
//
//	Notes:			None
//
//==============================================================================
HBRUSH COptions::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a static text control?
	if((nCtlColor == CTLCOLOR_STATIC))
	{
		//	Is this the powerpoint status control
		if(pWnd->m_hWnd == m_ctrlLStatus)
		{
			//	Is the powerpoint viewer disabled?
			if(!m_bEnablePowerPoint)
			{
				pDC->SetTextColor(RGB(255,255,255));
				pDC->SetBkColor(RGB(255,0,0));
				return m_brRed;
			}
		}
	}
	return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	COptions::OnEndJob()
//
// 	Description:	This function handles notifications from the printer when
//					it terminates a print job.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnEndJob(BOOL bAborted) 
{
	//	Notify the control
	if(m_pTMPrint)
		m_pTMPrint->OnEndJob(bAborted);
}

//==============================================================================
//
// 	Function Name:	COptions::OnFonts()
//
// 	Description:	This function is called when the user clicks on the Fonts
//					button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnFonts() 
{
	LOGFONT	lfFont;

	ZeroMemory(&lfFont, sizeof(lfFont));
	lstrcpy(lfFont.lfFaceName, m_strBarcodeFont);

	CFontDialog	Dialog(&lfFont, CF_EFFECTS | CF_BOTH);
	if(Dialog.DoModal() == IDOK)
	{
		m_strBarcodeFont = Dialog.GetFaceName();

		//	Update the font name
		m_ctrlBarcodeFont.SetWindowText(m_strBarcodeFont);
	}
}

//==============================================================================
//
// 	Function Name:	COptions::OnHelp()
//
// 	Description:	This function is called by the framework when the user 
//					clicks on the help button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnHelp() 
{
	m_Help.Open(TMHELP_CMBARCODE);
}

//==============================================================================
//
// 	Function Name:	COptions::OnInitDialog()
//
// 	Description:	This function is called by the framework to initialize the
//					dialog box.
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL COptions::OnInitDialog() 
{
	//	Do the base class initialization
	CDialog::OnInitDialog();

	//	Fill the printer list box
	FillPrinters();

	//	Fill the template list box
	FillTemplates();

	//	Set the printer pointers
	m_Printer.SetEnablePowerPoint(m_bEnablePowerPoint);
	m_Printer.SetEnableDIBPrinting(m_bDIBPrintingEnabled);
	m_Printer.SetOptions(this);

	//	Initialize the TrialMax help engine
	m_Help.Initialize(m_hWnd);

	//	Set the status bar text
	SetStatusText();

	//	Select the active printer
	SetPrinter(m_strPrinter);

	//	Set the correct color for the border color button
	m_ctrlBorderColor.SetColor(m_crPrintBorderColor);

	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	COptions::OnPrint()
//
// 	Description:	This function is called by the framework when the user 
//					clicks on the Print button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnPrint() 
{
	//	Don't bother if nothing is in the queue or there is no active template
	if(m_Queue.IsEmpty() || m_pTemplate == 0)
		return;

	//	Update the class members with the current settings
	UpdateData(TRUE);

	//	Set the template runtime options
	m_pTemplate->m_bPrintImage		= m_bImage;
	m_pTemplate->m_bPrintBorder		= m_bPrintBorder;
	m_pTemplate->m_bPrintFullPath	= m_bIncludePath;
	m_pTemplate->m_bPageAsSeries	= m_bIncludeTotal;
	m_pTemplate->m_bAutoRotate		= m_bAutoRotate;

	m_pTemplate->SetPrintEnabled(TEMPLATE_BARCODE, m_bBarcode);
	m_pTemplate->SetPrintEnabled(TEMPLATE_GRAPHIC, m_bGraphic);
	m_pTemplate->SetPrintEnabled(TEMPLATE_FILENAME, m_bFilename);
	m_pTemplate->SetPrintEnabled(TEMPLATE_NAME, m_bName);
	m_pTemplate->SetPrintEnabled(TEMPLATE_PAGENUM, m_bPageNum);
	m_pTemplate->SetPrintEnabled(TEMPLATE_DEPONENT, m_bDeponent);
	m_pTemplate->SetPrintEnabled(TEMPLATE_FOREIGN_BARCODE, m_bForeignBarcode);
	m_pTemplate->SetPrintEnabled(TEMPLATE_SOURCE_BARCODE, m_bSourceBarcode);

	//	Print the job
	Print();

	//	Save the current options after printing
	SaveToIni();
}

//==============================================================================
//
// 	Function Name:	COptions::OnPrintImage()
//
// 	Description:	This function handles notifications from the printer when
//					it starts printing a new image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnPrintImage(long lImage, LPCSTR lpFilename) 
{
	//	Notify the control
	if(m_pTMPrint)
		m_pTMPrint->OnPrintImage(lImage, lpFilename);
}

//==============================================================================
//
// 	Function Name:	COptions::OnPrintPage()
//
// 	Description:	This function handles notifications from the printer when
//					it starts printing a new page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnPrintPage(long lPage) 
{
	//	Notify the control
	if(m_pTMPrint)
		m_pTMPrint->OnPrintPage(lPage);
}

//==============================================================================
//
// 	Function Name:	COptions::OnReload()
//
// 	Description:	This function is called by the framework when the user 
//					clicks on the Reload button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnReload() 
{
	//	Read in the list of templates
	RefreshTemplates();
}

//==============================================================================
//
// 	Function Name:	COptions::OnStartJob()
//
// 	Description:	This function handles notifications from the printer when
//					it starts a new print job.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnStartJob(LPCSTR lpPrinter, long lPages, long lImages,
						  CTemplate* pTemplate) 
{
	//	Notify the control
	if(m_pTMPrint)
		m_pTMPrint->OnStartJob(lpPrinter, lPages, lImages, pTemplate);
}

//==============================================================================
//
// 	Function Name:	COptions::OnTemplateChange()
//
// 	Description:	This function is called whenever the user selects a new
//					template from the list box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::OnTemplateChange() 
{
	int	iIndex;

	//	Get the current selection index
	if((iIndex = m_ctrlTemplates.GetCurSel()) == LB_ERR)
	{
		m_pTemplate = 0;
	}
	else
	{
		//	Get a pointer to the template object
		m_pTemplate = (CTemplate*)m_ctrlTemplates.GetItemDataPtr(iIndex);
	}

	//	Set the field selection controls to the appropriate state
	SetControlStates();
}

//==============================================================================
//
// 	Function Name:	COptions::Print()
//
// 	Description:	This function is called to print the contents of the queue
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			This function assumes the template options have already
//					been set by the caller.
//
//==============================================================================
short COptions::Print() 
{
	CString	strMsg;

	//	Don't bother if nothing is in the queue or there is no active template
	if(m_Queue.IsEmpty() || m_pTemplate == 0)
		return TMPRINT_NOERROR;

	//	Clear the abort flag
	m_bAborted = FALSE;

	//	Set the active printer
	m_Printer.SetName(m_strPrinter);
	if(!m_Printer.Attach())
	{
		strMsg = m_Printer.GetErrorMsg();
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMPRINT_ATTACH_FAILED, strMsg); 
		return TMPRINT_ATTACH_FAILED;
	}

	//	Set the active template in the printer
	m_Printer.SetTemplate(m_pTemplate);

	//	Set the job options
	m_Printer.SetBreakOnDocument(m_bForceNewPage);
	m_Printer.SetInsertSlipSheet(m_bInsertSlipSheet);
	m_Printer.SetUseSlideId(m_bUseSlideId);
	m_Printer.SetBarcodeFont(m_strBarcodeFont);
	m_Printer.SetSaveFormat(m_iSaveFormat);
	m_Printer.SetShowStatus(m_bShowStatus);
	m_Printer.SetPrintCallouts(m_bPrintCallouts);
	m_Printer.SetPrintCalloutBorders(m_bPrintCalloutBorders);
	m_Printer.SetPrintBorderColor(m_crPrintBorderColor);
	m_Printer.SetPrintBorderThickness(m_fPrintBorderThickness);
	m_Printer.SetCalloutFrameColor(m_sCalloutFrameColor);
	m_Printer.SetEnableDIBPrinting(m_bDIBPrintingEnabled);

	if(m_pTemplate->m_sOrientation == TEMPLATE_ORIENTATION_PORTRAIT)
		m_Printer.SetOrientation(TMPRINTER_ORIENTATION_PORTRAIT);
	else if(m_pTemplate->m_sOrientation == TEMPLATE_ORIENTATION_LANDSCAPE)
		m_Printer.SetOrientation(TMPRINTER_ORIENTATION_LANDSCAPE);
	else
		m_Printer.SetOrientation(TMPRINTER_ORIENTATION_DEVICE);

	//	Should we override the margins defined in the template?
	if(m_fLeftMargin >= 0.0)
		m_Printer.SetLeftMargin(m_fLeftMargin);
	else
		m_Printer.SetLeftMargin(m_pTemplate->m_fLeftMargin);
	if(m_fTopMargin >= 0.0)
		m_Printer.SetTopMargin(m_fTopMargin);
	else
		m_Printer.SetTopMargin(m_pTemplate->m_fTopMargin);

	if(m_strBarcodeCharacter.IsEmpty())
		m_Printer.SetBarcodeCharacter(0);
	else
		m_Printer.SetBarcodeCharacter(m_strBarcodeCharacter.GetAt(0));

	//	Start the print job
	if(m_Printer.Start(m_strJobName) == 0)
	{
		strMsg = m_Printer.GetErrorMsg();
		m_pErrors->Handle(0, IDS_TMPRINT_START_FAILED, strMsg);
		return TMPRINT_START_FAILED;
	} 

	//	Print all cells
	m_Queue.Lock();
	m_Printer.Print(&m_Queue, this, m_sCopies, m_bCollate);
	m_Queue.Unlock();

	//	End the job
	m_Printer.End();

	return TMPRINT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	COptions::ReadTemplates()
//
// 	Description:	This function will read all template specifications 
//					contained in the current ini file. If bFlush
//					is TRUE, all existing templates are destroyed first.
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function assumes the ini object has been set to the
//					correct section.
//
//==============================================================================
BOOL COptions::ReadTemplates(BOOL bFlush)
{
	//	Read the templates from the file
	if(m_Templates.ReadFile(m_Ini.strFileSpec, m_strTemplateSection, bFlush))
	{
		return TRUE;
	}
	else
	{
		if(m_pErrors)
			m_pErrors->Handle("", IDS_TMPRINT_TEMPLATEFILECLOSED);
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	COptions::RefreshTemplates()
//
// 	Description:	This function is called to refresh the list of templates
//					from the ini file.
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short COptions::RefreshTemplates() 
{
	//	Read in the list of templates
	ReadTemplates(TRUE);
		
	//	Update the list box
	FillTemplates();

	return TMPRINT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	COptions::SaveToIni()
//
// 	Description:	This function will save the current print options to the
//					ini file provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL COptions::SaveToIni()
{
	//	Do we have a valid filename and section?
	if(m_strIniFilespec.IsEmpty() || m_strIniSection.IsEmpty())
		return FALSE;

	//	Make sure the ini file is open
	if(!m_Ini.bFileFound)
	{
		//	Attempt to open the file
		if(!m_Ini.Open(m_strIniFilespec, m_strIniSection))
		{
			if(m_pErrors)
				m_pErrors->Handle("", IDS_TMPRINT_ININOTFOUND, m_Ini.strFileSpec);
			return FALSE;
		}
	}
	else
	{
		//	Line up on the section for the options
		m_Ini.SetSection(m_strIniSection);
	}

	//	Write the print options to the ini file
	m_Ini.WriteBool(INILINE_AUTOROTATE, m_bAutoRotate);
	m_Ini.WriteBool(INILINE_PRINTIMAGE, m_bImage);
	m_Ini.WriteBool(INILINE_PRINTBARCODE, m_bBarcode);
	m_Ini.WriteBool(INILINE_PRINTFOREIGNBARCODE, m_bForeignBarcode);
	m_Ini.WriteBool(INILINE_PRINTSOURCEBARCODE, m_bSourceBarcode);
	m_Ini.WriteBool(INILINE_PRINTGRAPHIC, m_bGraphic);
	m_Ini.WriteBool(INILINE_PRINTFILENAME, m_bFilename);
	m_Ini.WriteBool(INILINE_PRINTNAME, m_bName);
	m_Ini.WriteBool(INILINE_PRINTDEPONENT, m_bDeponent);
	m_Ini.WriteBool(INILINE_PRINTPAGENUM, m_bPageNum);
	m_Ini.WriteBool(INILINE_PRINTBORDER, m_bPrintBorder);
	m_Ini.WriteBool(INILINE_PRINTFULLPATH, m_bIncludePath);
	m_Ini.WriteBool(INILINE_PRINTPAGETOTAL, m_bIncludeTotal);
	m_Ini.WriteBool(INILINE_PRINTCALLOUTS, m_bPrintCallouts);
	m_Ini.WriteBool(INILINE_PRINTCALLOUTBORDERS, m_bPrintCalloutBorders);
	m_Ini.WriteBool(INILINE_COLLATE, m_bCollate);
	m_Ini.WriteLong(INILINE_COPIES, m_sCopies);
	m_Ini.WriteLong(INILINE_PRINTBORDERCOLOR, (long)m_crPrintBorderColor);
	m_Ini.WriteBool(INILINE_FORCENEWPAGE, m_bForceNewPage);
	m_Ini.WriteBool(INILINE_INSERTSLIPSHEET, m_bInsertSlipSheet);
	m_Ini.WriteBool(INILINE_USESLIDEIDS, m_bUseSlideId);
	m_Ini.WriteString(INILINE_BARCODECHARACTER, m_strBarcodeCharacter);
	m_Ini.WriteString(INILINE_BARCODEFONT, m_strBarcodeFont);
	m_Ini.WriteString(INILINE_JOBNAME, m_strJobName);
	m_Ini.WriteDouble(INILINE_LEFTMARGIN, m_fLeftMargin);
	m_Ini.WriteDouble(INILINE_TOPMARGIN, m_fTopMargin);
	m_Ini.WriteDouble(INILINE_PRINTBORDERTHICKNESS, m_fPrintBorderThickness);
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	COptions::SetPrintBorderColor()
//
// 	Description:	This function is called to set the color of the callout
//					borders.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetPrintBorderColor(COLORREF crColor) 
{
	m_crPrintBorderColor = crColor;

	if(IsWindow(m_ctrlBorderColor.m_hWnd))
	{
		m_ctrlBorderColor.SetColor(m_crPrintBorderColor);
		m_ctrlBorderColor.RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	COptions::SetPrintBorderThickness()
//
// 	Description:	This function will set BottomMargin option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetPrintBorderThickness(float fThickness) 
{
	//	Save the new option
	m_fPrintBorderThickness = fThickness;

	//	Update the control if it's available
	if(IsWindow(m_ctrlPrintBorderThickness.m_hWnd))
		UpdateData(FALSE);
}

//==============================================================================
//
// 	Function Name:	COptions::SetCalloutFrameColor()
//
// 	Description:	This function will set CalloutFrameColor option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetCalloutFrameColor(short sColor) 
{
	//	Save the new option
	m_sCalloutFrameColor = sColor;

	//	Not available on the options form
}

//==============================================================================
//
// 	Function Name:	COptions::SetCollate()
//
// 	Description:	This function will set Collate option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetCollate(BOOL bCollate) 
{
	//	Save the new option
	m_bCollate = bCollate;

	//	Update the control if it's available
	if(IsWindow(m_ctrlCollate.m_hWnd))
		m_ctrlCollate.SetCheck(m_bCollate);
}

//==============================================================================
//
// 	Function Name:	COptions::SetControlStates()
//
// 	Description:	This function will enable and disable the field selection
//					controls based on the current template selection.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetControlStates() 
{
		
	//	Enable/disable the field selection controls
	if(m_pTemplate)
	{
		m_ctrlAutoRotate.EnableWindow(m_pTemplate->m_bAutoRotateEnable);
		m_ctrlImage.EnableWindow(m_pTemplate->m_bImageEnable);

		m_ctrlName.EnableWindow(m_pTemplate->GetFieldEnabled(TEMPLATE_NAME));
		m_ctrlDeponent.EnableWindow(m_pTemplate->GetFieldEnabled(TEMPLATE_DEPONENT));
		m_ctrlBarcode.EnableWindow(m_pTemplate->GetFieldEnabled(TEMPLATE_BARCODE));
		m_ctrlForeignBarcode.EnableWindow(m_pTemplate->GetFieldEnabled(TEMPLATE_FOREIGN_BARCODE));
		m_ctrlSourceBarcode.EnableWindow(m_pTemplate->GetFieldEnabled(TEMPLATE_SOURCE_BARCODE));
		m_ctrlGraphic.EnableWindow(m_pTemplate->GetFieldEnabled(TEMPLATE_GRAPHIC));
		m_ctrlFilename.EnableWindow(m_pTemplate->GetFieldEnabled(TEMPLATE_FILENAME));
		m_ctrlPageNum.EnableWindow(m_pTemplate->GetFieldEnabled(TEMPLATE_PAGENUM));

		//	Set the template runtime options
		m_ctrlAutoRotate.SetCheck(m_pTemplate->m_bAutoRotate);
		m_ctrlImage.SetCheck(m_pTemplate->m_bPrintImage);
		m_ctrlPrintBorder.SetCheck(m_pTemplate->m_bPrintBorder);

		m_ctrlBarcode.SetCheck(m_pTemplate->GetPrintEnabled(TEMPLATE_BARCODE));
		m_ctrlGraphic.SetCheck(m_pTemplate->GetPrintEnabled(TEMPLATE_GRAPHIC));
		m_ctrlForeignBarcode.SetCheck(m_pTemplate->GetPrintEnabled(TEMPLATE_FOREIGN_BARCODE));
		m_ctrlSourceBarcode.SetCheck(m_pTemplate->GetPrintEnabled(TEMPLATE_SOURCE_BARCODE));
		m_ctrlFilename.SetCheck(m_pTemplate->GetPrintEnabled(TEMPLATE_FILENAME));
		m_ctrlName.SetCheck(m_pTemplate->GetPrintEnabled(TEMPLATE_NAME));
		m_ctrlPageNum.SetCheck(m_pTemplate->GetPrintEnabled(TEMPLATE_PAGENUM));
		m_ctrlDeponent.SetCheck(m_pTemplate->GetPrintEnabled(TEMPLATE_DEPONENT));
	}
	else
	{
		m_ctrlAutoRotate.EnableWindow(FALSE);
		m_ctrlImage.EnableWindow(FALSE);
		m_ctrlName.EnableWindow(FALSE);
		m_ctrlDeponent.EnableWindow(FALSE);
		m_ctrlBarcode.EnableWindow(FALSE);
		m_ctrlGraphic.EnableWindow(FALSE);
		m_ctrlFilename.EnableWindow(FALSE);
		m_ctrlPageNum.EnableWindow(FALSE);
		m_ctrlForeignBarcode.EnableWindow(FALSE);
		m_ctrlSourceBarcode.EnableWindow(FALSE);
	}

	//	Is there anything to print?
	m_ctrlPrint.EnableWindow((!m_Queue.IsEmpty()) && (m_pTemplate != 0));
}

//==============================================================================
//
// 	Function Name:	COptions::SelectPrinter()
//
// 	Description:	This method is called to select the printer to be used for
//					susequent print jobs
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL COptions::SelectPrinter() 
{
	CSelect Select(this);

	//	Initialize the dialog box
	Select.m_strPrinter = m_strPrinter;
	Select.m_iCopies    = m_sCopies;
	Select.m_bCollate   = m_bCollate;

	if(Select.DoModal() == IDOK)
	{
		SetPrinter(Select.m_strPrinter);
		SetCopies(Select.m_iCopies);
		SetCollate(Select.m_bCollate);
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	COptions::SetBarcodeCharacter()
//
// 	Description:	This function will set Barcode character option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetBarcodeCharacter(char cBarcode) 
{
	//	Save the new option
	m_strBarcodeCharacter.Format("%c", cBarcode);

	//	Update the control if it's available
	if(IsWindow(m_ctrlBarcodeCharacter.m_hWnd))
		m_ctrlBarcodeCharacter.SetWindowText(m_strBarcodeCharacter);
}

//==============================================================================
//
// 	Function Name:	COptions::SetBarcodeFont()
//
// 	Description:	This function will set Barcode font option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetBarcodeFont(LPCSTR lpFontName) 
{
	//	Save the new option
	if(lpFontName && lstrlen(lpFontName) > 0)
		m_strBarcodeFont = lpFontName;

	//	Update the control if it's available
	if(IsWindow(m_ctrlBarcodeFont.m_hWnd))
		m_ctrlBarcodeFont.SetWindowText(m_strBarcodeFont);
}

//==============================================================================
//
// 	Function Name:	COptions::SetCopies()
//
// 	Description:	This function will set Copies option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetCopies(int iCopies) 
{
	CString strCopies;

	if(iCopies <= 0)
		iCopies = 1;

	//	Save the new option
	m_sCopies = iCopies;

	//	Update the control if it's available
	if(IsWindow(m_ctrlCopies.m_hWnd))
	{
		strCopies.Format("%d", iCopies);
		m_ctrlCopies.SetWindowText(strCopies);
	}
}

//==============================================================================
//
// 	Function Name:	COptions::SetForceNewPage()
//
// 	Description:	This function will set ForceNewPage option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetForceNewPage(BOOL bForce) 
{
	//	Save the new option
	m_bForceNewPage = bForce;

	//	Update the control if it's available
	if(IsWindow(m_ctrlForceNewPage.m_hWnd))
		m_ctrlForceNewPage.SetCheck(m_bForceNewPage);
}

//==============================================================================
//
// 	Function Name:	COptions::SetFromIni()
//
// 	Description:	This function will set the current print options using the
//					information contained in the specified ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL COptions::SetFromIni(LPCSTR lpFilespec, LPCSTR lpSection)
{
	char szIniStr[256];

	//	Save the ini file information
	m_strIniFilespec = (lpFilespec == 0) ? "" : lpFilespec;
	m_strIniSection = (lpSection == 0) ? "" : lpSection;

	//	Attempt to open the file
	if(!m_Ini.Open(m_strIniFilespec, m_strIniSection))
	{
		if(m_pErrors)
			m_pErrors->Handle("", IDS_TMPRINT_ININOTFOUND, m_Ini.strFileSpec);
		return FALSE;
	}

	//	Read the print options from the ini file
	m_bAutoRotate = m_Ini.ReadBool(INILINE_AUTOROTATE, DEFAULT_AUTOROTATE);
	m_bImage = m_Ini.ReadBool(INILINE_PRINTIMAGE, DEFAULT_PRINTIMAGE);
	m_bBarcode = m_Ini.ReadBool(INILINE_PRINTBARCODE, DEFAULT_PRINTBARCODE);
	m_bForeignBarcode = m_Ini.ReadBool(INILINE_PRINTFOREIGNBARCODE, DEFAULT_PRINTFOREIGNBARCODE);
	m_bSourceBarcode = m_Ini.ReadBool(INILINE_PRINTSOURCEBARCODE, DEFAULT_PRINTSOURCEBARCODE);
	m_bGraphic = m_Ini.ReadBool(INILINE_PRINTGRAPHIC, DEFAULT_PRINTGRAPHIC);
	m_bFilename = m_Ini.ReadBool(INILINE_PRINTFILENAME, DEFAULT_PRINTFILENAME);
	m_bName = m_Ini.ReadBool(INILINE_PRINTNAME, DEFAULT_PRINTNAME);
	m_bDeponent = m_Ini.ReadBool(INILINE_PRINTDEPONENT, DEFAULT_PRINTDEPONENT);
	m_bPageNum = m_Ini.ReadBool(INILINE_PRINTPAGENUM, DEFAULT_PRINTPAGENUM);
	m_bPrintBorder = m_Ini.ReadBool(INILINE_PRINTBORDER, DEFAULT_PRINTCELLBORDER);
	m_bIncludePath = m_Ini.ReadBool(INILINE_PRINTFULLPATH, DEFAULT_PRINTFULLPATH);
	m_bIncludeTotal = m_Ini.ReadBool(INILINE_PRINTPAGETOTAL, DEFAULT_PRINTPAGETOTAL);
	m_bPrintCallouts = m_Ini.ReadBool(INILINE_PRINTCALLOUTS, DEFAULT_CALLOUTS);
	m_bPrintCalloutBorders = m_Ini.ReadBool(INILINE_PRINTCALLOUTBORDERS, DEFAULT_CALLOUTBORDERS);
	m_bCollate = m_Ini.ReadBool(INILINE_COLLATE, DEFAULT_COLLATE);
	m_bForceNewPage = m_Ini.ReadBool(INILINE_FORCENEWPAGE, DEFAULT_FORCENEWPAGE);
	m_bInsertSlipSheet = m_Ini.ReadBool(INILINE_INSERTSLIPSHEET, DEFAULT_INSERTSLIPSHEET);
	m_bUseSlideId = m_Ini.ReadBool(INILINE_USESLIDEIDS, DEFAULT_USESLIDEIDS);
	m_bShowStatus = m_Ini.ReadBool(INILINE_SHOWSTATUS, DEFAULT_SHOWSTATUS);
	m_sCopies = (short)m_Ini.ReadLong(INILINE_COPIES, DEFAULT_COPIES);
	m_fLeftMargin = (float)m_Ini.ReadDouble(INILINE_LEFTMARGIN, DEFAULT_LEFTMARGIN);
	m_fTopMargin = (float)m_Ini.ReadDouble(INILINE_TOPMARGIN, DEFAULT_TOPMARGIN);
	m_fPrintBorderThickness = (float)m_Ini.ReadDouble(INILINE_PRINTBORDERTHICKNESS, DEFAULT_PRINTBORDERTHICKNESS);
	m_crPrintBorderColor = (COLORREF)m_Ini.ReadLong(INILINE_PRINTBORDERCOLOR, DEFAULT_PRINTBORDERCOLOR);
	m_iSaveFormat = (int)m_Ini.ReadLong(INILINE_SAVEFORMAT, DEFAULT_SAVEFORMAT);
	m_Ini.ReadString(INILINE_BARCODECHARACTER, szIniStr, sizeof(szIniStr), 
					 DEFAULT_BARCODECHARACTER);
	m_strBarcodeCharacter = szIniStr;
	m_Ini.ReadString(INILINE_BARCODEFONT, szIniStr, sizeof(szIniStr), 
					 DEFAULT_BARCODEFONT);
	m_strBarcodeFont = szIniStr;
	m_Ini.ReadString(INILINE_JOBNAME, szIniStr, sizeof(szIniStr), DEFAULT_JOBNAME);
	m_strJobName = szIniStr;

	//	Read in the name of the section used to store the template specifications
	m_Ini.ReadString(INILINE_TEMPLATESECTION, szIniStr, sizeof(szIniStr),
					 DEFAULT_TEMPLATESECTION);
	m_strTemplateSection = szIniStr;
	m_strTemplateSection.TrimLeft();
	m_strTemplateSection.TrimRight();

	//	Read in the list of templates
	ReadTemplates(TRUE);
		
	//	Update the controls if the window exists
	if(IsWindow(m_hWnd))
	{
		UpdateData(FALSE);

		m_ctrlBorderColor.SetColor(m_crPrintBorderColor);

		//	Fill the template list box
		FillTemplates();
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	COptions::SetIncludePath()
//
// 	Description:	This function will set Include Path option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetIncludePath(BOOL bInclude) 
{
	//	Save the new option
	m_bIncludePath = bInclude;

	//	Update the control if it's available
	if(IsWindow(m_ctrlIncludePath.m_hWnd))
		m_ctrlIncludePath.SetCheck(bInclude);
}

//==============================================================================
//
// 	Function Name:	COptions::SetIncludeTotal()
//
// 	Description:	This function will set Include Total option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetIncludeTotal(BOOL bInclude) 
{
	//	Save the new option
	m_bIncludeTotal = bInclude;

	//	Update the control if it's available
	if(IsWindow(m_ctrlPageSeries.m_hWnd))
		m_ctrlPageSeries.SetCheck(bInclude);
}

//==============================================================================
//
// 	Function Name:	COptions::SetInsertSlipSheet()
//
// 	Description:	This function will set InsertSlipSheet option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetInsertSlipSheet(BOOL bInsert) 
{
	//	Save the new option
	m_bInsertSlipSheet = bInsert;

	//	Update the control if it's available
	if(IsWindow(m_ctrlInsertSlipSheet.m_hWnd))
		m_ctrlInsertSlipSheet.SetCheck(m_bInsertSlipSheet);
}

//==============================================================================
//
// 	Function Name:	COptions::SetJobName()
//
// 	Description:	This function will set Job Name option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetJobName(LPCSTR lpJobName) 
{
	//	Save the new option
	if(lpJobName != 0)
		m_strJobName = lpJobName;
	else
		m_strJobName = DEFAULT_JOBNAME;

	//	Update the control if it's available
	if(IsWindow(m_ctrlJobName.m_hWnd))
		m_ctrlJobName.SetWindowText(m_strJobName);
}

//==============================================================================
//
// 	Function Name:	COptions::SetLeftMargin()
//
// 	Description:	This function will set LeftMargin option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetLeftMargin(float fMargin) 
{
	//	Save the new option
	m_fLeftMargin = fMargin;

	//	Update the control if it's available
	if(IsWindow(m_ctrlLeftMargin.m_hWnd))
		UpdateData(FALSE);
}

//==============================================================================
//
// 	Function Name:	COptions::SetPrintCalloutBorders()
//
// 	Description:	This function will set PrintCalloutBorders option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetPrintCalloutBorders(BOOL bPrint) 
{
	//	Save the new option
	m_bPrintCalloutBorders = bPrint;

	//	Update the control if it's available
	if(IsWindow(m_ctrlPrintCalloutBorders.m_hWnd))
		m_ctrlPrintCalloutBorders.SetCheck(m_bPrintCalloutBorders);
}

//==============================================================================
//
// 	Function Name:	COptions::SetPrintCallouts()
//
// 	Description:	This function will set PrintCallouts option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetPrintCallouts(BOOL bPrint) 
{
	//	Save the new option
	m_bPrintCallouts = bPrint;

	//	Update the control if it's available
	if(IsWindow(m_ctrlPrintCallouts.m_hWnd))
		m_ctrlPrintCallouts.SetCheck(m_bPrintCallouts);
}

//==============================================================================
//
// 	Function Name:	COptions::SetPrinter()
//
// 	Description:	This function will set the active printer
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL COptions::SetPrinter(LPCSTR lpPrinter) 
{
	int		iIndex;
	BOOL	bReturn = TRUE;

	//	Should we set the printer to the system default?
	if(lpPrinter == 0 || lstrlen(lpPrinter) == 0)
	{
		GetDefaultPrinter(m_strPrinter);
		iIndex = m_ctrlPrinters.FindStringExact(-1, m_strPrinter);
	}
	else
	{
		//	Make sure the specified printer exists
		if((iIndex = m_ctrlPrinters.FindStringExact(-1, lpPrinter)) == LB_ERR)
		{
			//	Switch to default if we don't have a current selection
			if(m_strPrinter.IsEmpty())
				GetDefaultPrinter(m_strPrinter);

			if(!m_strPrinter.IsEmpty())
				iIndex = m_ctrlPrinters.FindStringExact(-1, m_strPrinter);
			
			bReturn = FALSE;
		}
		else
		{
			m_strPrinter = lpPrinter;
		}
	}

	m_ctrlPrinters.SetCurSel(iIndex);
	return bReturn;
}

//==============================================================================
//
// 	Function Name:	COptions::SetPrinter()
//
// 	Description:	This function will open the properties sheet for the
//					specified printer
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL COptions::SetPrinterProperties(HWND hWnd) 
{
	int iReturn = 0;

	if(hWnd == 0)
		hWnd = m_hWnd;

	//	Make sure the printer has the current values
	m_Printer.SetName(m_strPrinter);
	m_Printer.SetCopies(m_sCopies);
	m_Printer.SetCollate(m_bCollate);

	if(m_pTemplate != 0)
	{
		if(m_pTemplate->m_sOrientation == TEMPLATE_ORIENTATION_PORTRAIT)
			m_Printer.SetOrientation(TMPRINTER_ORIENTATION_PORTRAIT);
		else if(m_pTemplate->m_sOrientation == TEMPLATE_ORIENTATION_LANDSCAPE)
			m_Printer.SetOrientation(TMPRINTER_ORIENTATION_LANDSCAPE);
		else
			m_Printer.SetOrientation(TMPRINTER_ORIENTATION_DEVICE);
	}

/*
CString M;
M.Format("Printer = %s\nCopies = %d\nCollate = %d\n",
		 m_Printer.GetName(), m_Printer.GetCopies(), m_Printer.GetCollate());
MessageBox(M);
*/

	if((iReturn = m_Printer.SetProperties(m_hWnd)) < 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, m_Printer.GetErrorMsg()); 
		return FALSE;
	}
	else
	{
		//	Did the user make changes?
		if(iReturn == IDOK)
		{
			//	Update our local members
			SetPrinter(m_Printer.GetName());
			SetCopies(m_Printer.GetCopies());
			SetCollate(m_Printer.GetCollate());

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
// 	Function Name:	COptions::SetShowStatus()
//
// 	Description:	This function will set the ShowStatus option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetShowStatus(BOOL bShowStatus) 
{
	//	Save the new option
	m_bShowStatus = bShowStatus;

	//	Update the control if it's available
	if(IsWindow(m_ctrlShowStatus.m_hWnd))
		m_ctrlShowStatus.SetCheck(m_bShowStatus);
}

//==============================================================================
//
// 	Function Name:	COptions::SetStatusText()
//
// 	Description:	This function will set the text in the dialog box status bar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetStatusText() 
{
	//	Let the user know if the PowerPoint viewer is disabled
	if(!m_bEnablePowerPoint)
	{
		m_ctrlLStatus.SetWindowText("PowerPoint printing disabled");
	}
	else
	{
		m_ctrlLStatus.SetWindowText("PowerPoint printing enabled");
	}

	//	Make sure the PowerPoint status is displayed in the appropriate color
	m_ctrlLStatus.Invalidate();
}

//==============================================================================
//
// 	Function Name:	COptions::SetTemplates()
//
// 	Description:	This function is called to set the list of templates
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short COptions::SetTemplates(CTemplates* pTemplates) 
{
	POSITION	Pos;
	CTemplate*	pTemplate;

	//	Flush the existing list of templates
	m_Templates.Flush(TRUE);

	//	Add each template to the local list
	if(pTemplates)
	{
		Pos = pTemplates->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pTemplate = (CTemplate*)pTemplates->GetNext(Pos)) != 0)
			{
				m_Templates.Add(new CTemplate(*pTemplate));
			}
		}
	}
	 
	//	Update the list box
	if(IsWindow(m_hWnd))
	{
		//	Fill the template list box
		FillTemplates();
	}

	return TMPRINT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	COptions::SetTemplate()
//
// 	Description:	This function is called by the control to set the active
//					template for the print job
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL COptions::SetTemplate(CTemplate* pTemplate) 
{
	CTemplate*	pSelection;
	int			iIndex;

	ASSERT(pTemplate);

	//	Check to make sure this template is in the list
	//
	//	NOTE:	We don't use the pointer to locate the object because we may
	//			have made a copy of the caller's template object when we added
	//			it to the list
	if((pSelection = m_Templates.Find(pTemplate->m_strDescription)) == 0)
		return FALSE;

	//	Make this the active template
	m_pTemplate = pSelection;

	//	Update the list box
	if(IsWindow(m_ctrlTemplates.m_hWnd) && m_ctrlTemplates.IsWindowVisible())
	{
		if((iIndex = m_ctrlTemplates.FindStringExact(-1, pSelection->m_strDescription)) != LB_ERR)
		{
			m_ctrlTemplates.SetCurSel(iIndex);
			OnTemplateChange();
		}
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	COptions::SetTemplate()
//
// 	Description:	This function is called by the control to set the active
//					template for the print job
//
// 	Returns:		A pointer to the new template selection
//
//	Notes:			None
//
//==============================================================================
CTemplate* COptions::SetTemplate(LPCSTR lpTemplate) 
{
	CTemplate*	pTemplate;
	int			iIndex;

	ASSERT(lpTemplate);

	if((pTemplate = m_Templates.Find(lpTemplate)) == 0)
		return 0;

	//	Make this the active template
	m_pTemplate = pTemplate;

	//	Update the list box
	if(IsWindow(m_ctrlTemplates.m_hWnd) && m_ctrlTemplates.IsWindowVisible())
	{
		if((iIndex = m_ctrlTemplates.FindStringExact(-1, lpTemplate)) != LB_ERR)
		{
			m_ctrlTemplates.SetCurSel(iIndex);
			OnTemplateChange();
		}
	}

	return m_pTemplate;
}

//==============================================================================
//
// 	Function Name:	COptions::SetTopMargin()
//
// 	Description:	This function will set TopMargin option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetTopMargin(float fMargin) 
{
	//	Save the new option
	m_fTopMargin = fMargin;

	//	Update the control if it's available
	if(IsWindow(m_ctrlTopMargin.m_hWnd))
		UpdateData(FALSE);
}

//==============================================================================
//
// 	Function Name:	COptions::SetUseSlideId()
//
// 	Description:	This function will set UseSlideID option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COptions::SetUseSlideId(BOOL bUseSlideId) 
{
	//	Save the new option
	m_bUseSlideId = bUseSlideId;

	//	Update the control if it's available
	if(IsWindow(m_ctrlUseSlideId.m_hWnd))
		m_ctrlUseSlideId.SetCheck(m_bUseSlideId);
}


