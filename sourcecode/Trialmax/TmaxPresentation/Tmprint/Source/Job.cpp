//==============================================================================
//
// File Name:	tmprint.cpp
//
// Description:	This file contains member functions of the CJob class.
//
// See Also:	job.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-29-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <job.h>
#include <tmview.h>
#include <tmvdefs.h>
#include <tmppdefs.h>
#include <tmpower.h>
#include <prtstat.h>
#include <cell.h>
#include <options.h>
#include <tmprdefs.h>
#include <tmppdefs.h>
#include <tmprntap.h>

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
static CPrintStatus*	theStatus = 0;
static COptions*		theOptions = 0;	
extern CTMPrintApp NEAR theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CJob::AbortTMPrint()
//
// 	Description:	This is the callback passed to Windows when a print job
//					is started.
//
// 	Returns:		TRUE if printing should continue
//
//	Notes:			None
//
//==============================================================================
BOOL CALLBACK CJob::AbortTMPrint(HDC hdc, int iCode)
{
	MSG Msg;
	
	if(theStatus)
		SetFocus(theStatus->m_hWnd);

	while(::PeekMessage(&Msg, NULL, 0, 0, PM_NOREMOVE))
	{
		AfxGetThread()->PumpMessage();
	}
	
	//	Did the user cancel the operation?
	if(theStatus && theStatus->GetAbortJob())
		return FALSE;
	else if(theOptions && theOptions->GetAborted())
		return FALSE;
	else
		return TRUE;	
}

//==============================================================================
//
// 	Function Name:	CJob::CJob()
//
// 	Description:	This is the constructor for CJob objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CJob::CJob() : CTMPrinter()
{
	m_pTemplate		= NULL;
	m_pOptions		= NULL;
	m_iPgWidth		= 0;
	m_iPgHeight		= 0;
	m_iRows			= 0;
	m_iCols			= 0;
	m_iCellWidth	= 0;
	m_iCellHeight	= 0;
	m_iCellSpaceX	= 0;
	m_iCellSpaceY	= 0;
	m_iCellPadX		= 0;
	m_iCellPadY		= 0;
	m_iImagePadX	= 0;
	m_iImagePadY	= 0;
	m_iCells		= 0;
	m_iPages		= 0;
	m_iSaveFormat	= 0;
	m_lImage		= 0;
	m_lImages		= 0;
	m_cBarcode		= 0;
	m_bEnablePowerPoint = FALSE;
	m_bEnableDIBPrinting = TRUE;
	m_bBreakOnDocument = FALSE;
	m_bInsertSlipSheet = FALSE;
	m_bShowStatus = TRUE;
	m_bPrintCallouts = TRUE;
	m_bPrintCalloutBorders = TRUE;
	m_bAutoRotateImage = FALSE;
	m_sCalloutFrameColor = 0;
	m_fPrintBorderThickness = 0.0;
	m_crPrintBorderColor = RGB(0x00,0x00,0x00);
	m_strBarcodeFont = TMPRINT_BARCODEFONT;
}

//==============================================================================
//
// 	Function Name:	CJob::CJob()
//
// 	Description:	This is the destructor for CJob objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CJob::~CJob()
{
	//	Delete the status dialog if it exists
	CloseStatus();

	//	Flush any cells from the local list without deleting the objects
	m_Page.Flush(FALSE);
}

//==============================================================================
//
// 	Function Name:	CJob::CloseStatus()
//
// 	Description:	This function is called to close the status dialog
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::CloseStatus()
{
	//	Delete the status dialog if it exists
	if(theStatus)
	{
		theStatus->Terminate();
		delete theStatus;
		theStatus = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CJob::DrawText()
//
// 	Description:	This function draws the text provided by the caller into
//					the printer dc using the options defined in the template 
//					field descriptor. The text is placed within the cell passed 
//					by the caller.
//
// 	Returns:		None
//
//	Notes:			This function assumes the desired font has been selected
//					into the dc before it's called.
//
//==============================================================================
void CJob::DrawText(LPCSTR lpText, CTemplateField* pField, 
					RECT* pCellRect, RECT* pRect)
{
	CSize			Size;
	UINT			uFormat = DT_SINGLELINE;
	int				iX;
	int				iY;
	int				iCx;
	int				iCy;

	ASSERT(m_pDC);
	ASSERT(lpText);
	ASSERT(pField);
	ASSERT(pCellRect);
	ASSERT(pRect);
	
	//	Get the size of the rectangle required to draw the text
	GetTextExtentPoint32(m_pDC->GetSafeHdc(), lpText, lstrlen(lpText), &Size);

	//	Convert the coordinates provided by the caller to pixels
	iX = (int)(pField->m_fHPos * (float)m_iXDpi);
	iY = (int)(pField->m_fVPos * (float)m_iYDpi);

	//	Get the center points of the cell
	iCx = ((pCellRect->right - pCellRect->left) / 2) + pCellRect->left;
	iCy = ((pCellRect->bottom - pCellRect->top) / 2) + pCellRect->top;

	//	What horizontal alignment to we want to use?
	switch(pField->m_sHAlign)
	{
		case TEMPLATE_ALIGNLEFT:	uFormat |= DT_LEFT;
									pRect->left = pCellRect->left + iX;
									pRect->right = pRect->left + Size.cx;
									break;
		case TEMPLATE_ALIGNRIGHT:	uFormat |= DT_RIGHT;
									pRect->right = pCellRect->right - iX;
									pRect->left = pRect->right - Size.cx;
									break;
		case TEMPLATE_ALIGNCENTER:	
		default:					uFormat |= DT_CENTER;
									pRect->left = (iCx - (Size.cx / 2)) + iX;
									pRect->right = pRect->left + Size.cx;
									break;
	}

	//	What verticle alignment to we want to use?
	switch(pField->m_sVAlign)
	{
		case TEMPLATE_ALIGNTOP:		uFormat |= DT_TOP;
									pRect->top = pCellRect->top + iY;
									pRect->bottom = pRect->top + Size.cy;
									break;
		case TEMPLATE_ALIGNBOTTOM:	uFormat |= DT_BOTTOM;
									pRect->bottom = pCellRect->bottom - iY;
									pRect->top = pRect->bottom - Size.cy;
									break;
		case TEMPLATE_ALIGNCENTER:	
		default:					uFormat |= DT_VCENTER;
									pRect->top = (iCy - (Size.cy / 2)) + iY;
									pRect->bottom = pRect->top + Size.cy;
									break;
	}

	//	Draw the text
	m_pDC->DrawText(lpText, pRect, uFormat);
}

//==============================================================================
//
// 	Function Name:	CJob::FindFile()
//
// 	Description:	This function checks to see if the file exists.
//
// 	Returns:		TRUE if the file exists.
//
//	Notes:			None
//
//==============================================================================
BOOL CJob::FindFile(LPCSTR lpszFilename)
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;

	if((hFind = FindFirstFile(lpszFilename, &FindData)) == INVALID_HANDLE_VALUE)
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
// 	Function Name:	CJob::GetPageExtents()
//
// 	Description:	This function will use the active page template to calculate
//					the extents used to print a cell page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::GetPageExtents()
{
	ASSERT(m_pTemplate);

	//	Get the page extents in pixels
	m_iPgWidth  = m_rcMax.right - m_rcMax.left;
	m_iPgHeight = m_rcMax.bottom - m_rcMax.top;

	//	How many rows and columns does this template specify
	m_iRows = m_pTemplate->m_sRows;
	m_iCols = m_pTemplate->m_sColumns;
	m_iCellsPerPage = m_iRows * m_iCols;
	
	//	All information for an individual cell is contained within a cell
	m_iCellWidth = (int)(m_pTemplate->m_fCellWidth * (float)m_iXDpi);
	m_iCellHeight = (int)(m_pTemplate->m_fCellHeight * (float)m_iYDpi);

	//	Cell spacing refers to the margin that exists between adjacent cells
	m_iCellSpaceX = (int)(m_pTemplate->m_fCellSpaceWidth * (float)m_iXDpi);
	m_iCellSpaceY = (int)(m_pTemplate->m_fCellSpaceHeight * (float)m_iYDpi);

	//	Cell padding refers to the margin that exists between the outside
	//	edges of a cell and the text/image within the cell
	m_iCellPadX = (int)(m_pTemplate->m_fCellPadWidth * (float)m_iXDpi);
	m_iCellPadY = (int)(m_pTemplate->m_fCellPadHeight * (float)m_iYDpi);

	//	Image padding refers to the margin that exists between the edges
	//	of the image and other fields inside the cell
	m_iImagePadX = (int)(m_pTemplate->m_fImagePadWidth * (float)m_iXDpi);
	m_iImagePadY = (int)(m_pTemplate->m_fImagePadHeight * (float)m_iYDpi);

	//	Get the total page count
	ASSERT(m_pQueue);
	ASSERT(m_iCellsPerPage > 0);
	if(m_iCells == 0)
	{
		m_iPages = 0;
	}
	else
	{
		//	How many pages do we need?
		m_iPages = (int)((double)m_iCells / (double)m_iCellsPerPage);

		//	Allow an extra page if not evenly divisible
		if(m_iCells % m_iCellsPerPage)
			m_iPages++;
	}
}

//==============================================================================
//
// 	Function Name:	CJob::Print()
//
// 	Description:	This function will use the current page template to print
//					the cells provided by the caller.
//
// 	Returns:		TMPRINTER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
int CJob::Print(CCells* pCells, CWnd* pParent, short sCopies, BOOL bCollate)
{
	CCell*	pCell;
	int		i;
	int		iJobLoops;
	int		iPageLoops;
	int		iPage = 1;
	BOOL	bAbortJob = FALSE;
	CString	strMaster;
	BOOL	bPowerPoint;
	BOOL	bSlipSheet = FALSE;

	ASSERT(pCells);
	m_pQueue = pCells;

	//	Don't bother if there isn't anything in the list
	if((m_iCells = m_pQueue->GetCount()) == 0)
		return TMPRINTER_NOERROR;

	//	We should have the device context if the user called Start()
	if(m_pDC == 0)
		return TMPRINTER_NOTOPEN;

	//	We have to have a valid template
	if(!m_pTemplate)
		return TMPRINTER_NOTEMPLATE;

	//	Are we supposed to automatically rotate the image?
	if((m_pTemplate->m_bAutoRotateEnable == TRUE) && (m_pTemplate->m_bAutoRotate == TRUE))
		m_bAutoRotateImage = TRUE;
	else
		m_bAutoRotateImage = FALSE;

	//	Get the page extents for this job
	GetPageExtents();

	//	Delete the status dialog if it exists
	CloseStatus();

	//	How many total images are going to be printed?
	if(sCopies <= 1)
		m_lImages = m_iCells;
	else
		m_lImages = m_iCells * sCopies;
	m_lImage = 0;

	//	Do we need the PowerPoint viewer?
	if(m_bEnablePowerPoint && m_pQueue->UsesPowerPoint())
		bPowerPoint = TRUE;
	else
		bPowerPoint = FALSE;

	//	Open a status dialog
	theStatus = new CPrintStatus(this, m_iPages, m_lImages, bPowerPoint, pParent);

	//	Should we make the status dialog visible?
	if(m_bShowStatus)
	{
		::SetFocus(theStatus->m_hWnd);

		//	Keep the status window centered within the client area of the 
		//	control window
		//
		//	NOTE:	We use the control window instead of the Options dialog window
		//			because the Options dialog may be invisible in which case the
		//			control window will not be the same size as the Options window
		if(m_pOptions != 0)
			theStatus->CenterWindow(m_pOptions->GetParent());
		
		//	Make the window visible
		theStatus->ShowWindow(SW_SHOW);
		theStatus->BringWindowToTop();
	}

	//	Notify the options dialog that we are about to start printing
	if(m_pOptions)
		m_pOptions->OnStartJob(m_strAttached, m_iPages, m_lImages, m_pTemplate);

	//	Are we printing only one copy?
	if(sCopies <= 1)
	{
		iJobLoops = 1;
		iPageLoops = 1;
	}
	else
	{
		//	Are we collating?
		if(bCollate)
		{
			iJobLoops = sCopies;
			iPageLoops = 1;
		}
		else
		{
			iJobLoops = 1;
			iPageLoops = sCopies;
		}
	}
	
	//	How may times to we need to print the job
	for(int j = 0; ((j < iJobLoops) && !bAbortJob); j++)
	{
		//	Reset the page count for multiple copies
		iPage = 1;

		//	Get the first cell object
		pCell = m_pQueue->First();

		//	Print each page of the job
		while(pCell != 0 && !bAbortJob)
		{
			//	Flush any cells from the local list without deleting the objects
			m_Page.Flush(FALSE);

			//	Now fill the local list with those required for the next page
			for(i = 0; i < m_iCellsPerPage; i++)
			{
				m_Page.Add(pCell);
				strMaster = pCell->m_strMasterId;
			
				//	Stop here if we've run out of cells
				if((pCell = m_pQueue->Next()) == NULL)
					break;

				//	Are we breaking across documents?
				//
				//	NOTE:	Inserting slip sheets imples that we are breaking across 
				//			documents
				if(m_bBreakOnDocument || m_bInsertSlipSheet)
				{
					//	Is the next cell associated with a new document?
					if(pCell->m_strMasterId.CompareNoCase(strMaster) != 0)
					{
						//	Should we insert a slip sheet?
						if(m_bInsertSlipSheet)
						{
							//	Force printing of a slip sheet after this page
							//	gets printed
							bSlipSheet = TRUE;
						}

						//	This finishes this page
						break;

					}// if(pCell->m_strMasterId.CompareNoCase(strMaster) != 0)
				
				}// if(m_bBreakOnDocument || m_bInsertSlipSheet)
			
			}// for(i = 0; i < m_iCellsPerPage; i++)

			//	Print this page if there are any cells in the local list
			if(!m_Page.IsEmpty() && !bAbortJob)
			{
				//	Notify the options dialog
				if(m_pOptions)
					m_pOptions->OnPrintPage(iPage);

				//	Notify the status box
				if(theStatus)
					theStatus->SetPage(iPage++);

				for(int p = 0; ((p < iPageLoops) && !bAbortJob); p++)
					bAbortJob = PrintPage();

				//	Should we print a slip sheet?
				if(bSlipSheet && !bAbortJob)
				{
					bAbortJob = PrintSlipSheet();
					bSlipSheet = FALSE;
				}
			
			}// if(!m_Page.IsEmpty())		
		
		}// while(pCell != 0 && !bAbortJob)
	
	}// for(int j = 0; ((j < iJobLoops) && !bAbortJob); j++)

	//if(!bAbortJob)
		//m_pDC->EndDoc();

	//	Delete the status box
	CloseStatus();

	//if(bAbortJob)
		//m_pDC->AbortDoc();

	//	Notify the options dialog
	if(m_pOptions)
		m_pOptions->OnEndJob(bAbortJob);

	//	End the document
	if(bAbortJob)
		m_pDC->AbortDoc();
	else
		m_pDC->EndDoc();

	//	Remove all cells from the local list without deleting the objects
	m_Page.Flush(FALSE);

	return TMPRINTER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CJob::PrintCell()
//
// 	Description:	This function prints an individual cell on the page at the
//					top/left position specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::PrintCell(int iLeft, int iTop, CCell* pCell)
{
	RECT			rcCell;
	RECT			rcText;
	RECT			rcImage;
	RECT			rcGraphic;
	CString			strFilespec;
	CTemplateField*	pField = NULL;
	CCellField*		pCellField = NULL;

	ASSERT(m_pDC);
	ASSERT(pCell);
	ASSERT(m_pTemplate);

	//	Assemble the full file specification for the filename
	strFilespec = pCell->m_strPath;
	if(strFilespec.Right(1) != "\\")
		strFilespec += "\\";
	strFilespec += pCell->GetText(TEMPLATE_FILENAME);

	//	Notify the options dialog
	if(m_pOptions)
		m_pOptions->OnPrintImage(m_lImage, strFilespec);

	//	Set up the rectangle that defines this cell
	rcCell.left   = iLeft;
	rcCell.top    = iTop;
	rcCell.right  = iLeft + m_iCellWidth;
	rcCell.bottom = iTop + m_iCellHeight;

	//	Draw the border for this cell if enabled
	if(m_pTemplate->m_bPrintBorder)
		m_pDC->Rectangle(&rcCell);

	//	Now adjust the cell to allow for padding. This becomes our printable
	//	region
	rcCell.left   += m_iCellPadX;
	rcCell.top    += m_iCellPadY;
	rcCell.right  -= m_iCellPadX;
	rcCell.bottom -= m_iCellPadY;

	//	Initialize the rectangle for the image to match that of the cell
	rcImage.left   = rcCell.left; 
	rcImage.top    = rcCell.top; 
	rcImage.right  = rcCell.right; 
	rcImage.bottom = rcCell.bottom; 

	//	Print the fixed fields
	for(int i = 0; i < TEMPLATE_MAX_FIXED_FIELDS; i++)
	{
		//	Should we print this field?
		if(m_pTemplate->GetPrintEnabled(i) == TRUE)
		{
			//	Is this the barcode graphic?
			if(i == TEMPLATE_GRAPHIC)
			{
				PrintGraphic(pCell, &(m_pTemplate->m_aFixedFields[i]), &rcCell, &rcGraphic);

				//	Adjust the image rectangle
				SubtractRect(&rcImage, &rcGraphic);
			}
			else
			{
				PrintText(pCell->GetText(i, m_pTemplate), &(m_pTemplate->m_aFixedFields[i]), &rcCell, &rcText);
				
				//	Adjust the image rectangle
				SubtractRect(&rcImage, &rcText);
			}
		
		}// if(m_pTemplate->GetPrintEnabled(i) == TRUE)

	}// for(int i = 0; i < TEMPLATE_MAX_FIXED_FIELDS; i++)

	//	Print the text fields
	pField = m_pTemplate->m_aTextFields.First();
	while(pField != NULL)
	{
		if(pField->m_bPrint == TRUE)
		{
			//	Do we have the text to be printed?
			//
			//	First look to see if text has been provided through the cell descriptor.
			//	If not, use default text assigned to the field if available
			if((pCellField = pCell->m_aTextFields.Find(pField->m_iId)) != NULL)
				PrintText(pCellField->m_strText, pField, &rcCell, &rcText);
			else if(pField->m_strText.GetLength() > 0)
				PrintText(pField->m_strText, pField, &rcCell, &rcText);
				
		}

		pField = m_pTemplate->m_aTextFields.Next();
	}

	//	Do we need to print the image?
	if(m_pTemplate->m_bImageEnable && 
	   m_pTemplate->m_bPrintImage && 
	   pCell->m_sType != CELL_PLAYLIST && 
	   pCell->m_sType != CELL_DESIGNATION &&
	   pCell->m_sType != CELL_MOVIE)
	{
		//	Adjust the image rectangle to allow for padding. 
		rcImage.left   += m_iImagePadX;
		rcImage.top    += m_iImagePadY;
		rcImage.right  -= m_iImagePadX;
		rcImage.bottom -= m_iImagePadY;

		//	Print the image using the appropriate viewer
		if(pCell->m_sType == CELL_POWERPOINT)
		{
			//	Has the caller provided an exported image?
			if((pCell->m_strTreatmentImage.GetLength() > 0) &&
			   (FindFile(pCell->m_strTreatmentImage) == TRUE))
			{
				//	Just print the exported image
				PrintTMView(&rcImage, pCell->m_strTreatmentImage, 0, TRUE, 0, "", "", 0);
			}
			else
			{
				PrintTMPower(&rcImage, strFilespec, pCell->m_lPage);
			}
		}
		else
		{
			PrintTMView(&rcImage, strFilespec, pCell->m_lTertiaryId, TRUE,
						pCell->m_strTreatmentImage, pCell->m_strSiblingFileSpec,
						pCell->m_strSiblingImage, pCell->m_lFlags);
		}
	}

	//	Update the status box
	if(theStatus)
	{
		SetFocus(theStatus->m_hWnd);
		//theStatus->StepProgress(1);
	}
}

//==============================================================================
//
// 	Function Name:	CJob::PrintGraphic()
//
// 	Description:	This function will print the graphic representation of the
//					cell.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::PrintGraphic(CCell* pCell, CTemplateField* pField, 
						RECT* pCellRect, RECT* pRect)
{
	LOGFONT			LogFont;
	CFont			Font;
	CFont*			pOldFont;
	CString			strCell;
	CString			strBarcode;

	ASSERT(m_pDC);
	ASSERT(pCell);
	ASSERT(pField);
	ASSERT(pCell);
	ASSERT(pRect);

	//	Which barcode string should we use?
	strBarcode = pCell->GetText(TEMPLATE_GRAPHIC);
	if(strBarcode.GetLength() == 0)
		strBarcode = pCell->GetText(TEMPLATE_BARCODE);

	//	Format the text to be read by a barcode scanner
	if(strBarcode.GetLength() > 0)
	{
		if(m_cBarcode == 0)
			strCell.Format("*%s*", strBarcode);
		else
			strCell.Format("*%c%s*", m_cBarcode, strBarcode);

		//	Convert to all upper case
		strCell.MakeUpper();
	}

	//	Initialize the logical font structure
	LogFont.lfHeight = (int)((float)pField->m_sFont / 72.0f * (float)m_iYDpi);                	
	LogFont.lfWidth  = 0;				      	
	LogFont.lfEscapement = 0;                 	
	LogFont.lfOrientation = 0;                	
	LogFont.lfWeight = FW_NORMAL;
	LogFont.lfItalic = 0;
	LogFont.lfUnderline = 0;
	LogFont.lfStrikeOut = 0;
	LogFont.lfCharSet		= ANSI_CHARSET;
	LogFont.lfOutPrecision	= OUT_TT_PRECIS;
	LogFont.lfClipPrecision 	= CLIP_DEFAULT_PRECIS;
	LogFont.lfQuality		= PROOF_QUALITY;
	LogFont.lfPitchAndFamily = DEFAULT_PITCH | FF_DONTCARE;
	lstrcpy(LogFont.lfFaceName, m_strBarcodeFont);
	
	//	Create the new font and select it into the dc
	Font.CreateFontIndirect(&LogFont);
	pOldFont = m_pDC->SelectObject(&Font);

	//	Draw the caller's text
	DrawText(strCell, pField, pCellRect, pRect);

	//	Select the old font back into the dc
	m_pDC->SelectObject(pOldFont);
}

//==============================================================================
//
// 	Function Name:	CJob::PrintPage()
//
// 	Description:	This function prints a page of the current job.
//
// 	Returns:		TRUE if the job should be aborted
//
//	Notes:			The local Cells list should be filled with the cells
//					for this page before this function is called.
//
//==============================================================================
BOOL CJob::PrintPage()
{
	CCell*	pCell;
	int		iLeft = m_rcMax.left;
	int		iTop  = m_rcMax.top;
	MSG		Msg;

	ASSERT(m_pDC);

	//	Tell the device to start a new page
	StartPage();

	//	Get the first cell for this page
	pCell = m_Page.First();

	//	Print the objects in row major form
	for(int i = 0; i < m_iRows && pCell; i++)
	{
		//	Reposition on the first column
		iLeft = m_rcMax.left;

		for(int j = 0; j < m_iCols; j++)
		{
			//	Process pending messages
			while(::PeekMessage(&Msg, NULL, 0, 0, PM_NOREMOVE))
			{
				AfxGetThread()->PumpMessage();
			}
	
			//	Did the user cancel the operation?
			if(theStatus && theStatus->GetAbortJob())
				return TRUE;
			if(m_pOptions && m_pOptions->GetAborted())
				return TRUE;

			//	Update the status dialog
			if(theStatus)
				theStatus->SetImage(++m_lImage);

			//	Print this cell
			PrintCell(iLeft, iTop, pCell);

			//	Get the next cell
			if((pCell = m_Page.Next()) == 0)
				break;

			//	Adjust for the next column
			iLeft += (m_iCellWidth + m_iCellSpaceX);
		}
			
		//	Adjust the top of the next row
		iTop += (m_iCellHeight + m_iCellSpaceY);

	}

	//	End this page and tell the caller if the job should be aborted
	return (!EndPage());
}

//==============================================================================
//
// 	Function Name:	CJob::PrintSlipSheet()
//
// 	Description:	This function prints a slip sheet (blank) page
//
// 	Returns:		TRUE if the job should be aborted
//
//	Notes:			None
//
//==============================================================================
BOOL CJob::PrintSlipSheet()
{
	MSG Msg;

	ASSERT(m_pDC);

//MessageBox("", "slip sheet");
//return FALSE;

	//	Tell the device to start a new page
	StartPage();

	//	Process pending messages
	while(::PeekMessage(&Msg, NULL, 0, 0, PM_NOREMOVE))
	{
		AfxGetThread()->PumpMessage();
	}

	//	Did the user cancel the operation?
	if(theStatus && theStatus->GetAbortJob())
		return TRUE;
	if(m_pOptions && m_pOptions->GetAborted())
		return TRUE;

	//	End this page and tell the caller if the job should be aborted
	return (!EndPage());
}

//==============================================================================
//
// 	Function Name:	CJob::PrintText()
//
// 	Description:	This function prints the text provided by the caller using
//					the options defined in the template field descriptor. The
//					text is placed within the cell passed by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::PrintText(LPCSTR lpText, CTemplateField* pField, 
					 RECT* pCellRect, RECT* pRect)
{
	LOGFONT			LogFont;
	CFont			Font;
	CFont*			pOldFont;

	ASSERT(m_pDC);
	ASSERT(lpText);
	ASSERT(pField);
	ASSERT(pCellRect);
	ASSERT(pRect);

	//	Initialize the logical font structure
	LogFont.lfHeight = (int)((float)pField->m_sFont / 72.0f * (float)m_iYDpi);                	
	LogFont.lfWidth  = 0;				      	
	LogFont.lfEscapement = 0;                 	
	LogFont.lfOrientation = 0;                	
	LogFont.lfWeight = FW_NORMAL;
	LogFont.lfItalic = 0;
	LogFont.lfUnderline = 0;
	LogFont.lfStrikeOut = 0;
	LogFont.lfCharSet		= ANSI_CHARSET;
	LogFont.lfOutPrecision	= OUT_TT_PRECIS;
	LogFont.lfClipPrecision 	= CLIP_DEFAULT_PRECIS;
	LogFont.lfQuality		= PROOF_QUALITY;
	LogFont.lfPitchAndFamily = DEFAULT_PITCH | FF_DONTCARE;
	lstrcpy(LogFont.lfFaceName, "ARIAL");
	
	//	Create the new font and select it into the dc
	Font.CreateFontIndirect(&LogFont);
	pOldFont = m_pDC->SelectObject(&Font);

	//	Draw the caller's text
	DrawText(lpText, pField, pCellRect, pRect);

	//	Select the old font back into the dc
	m_pDC->SelectObject(pOldFont);
}

//==============================================================================
//
// 	Function Name:	CJob::PrintTMPower()
//
// 	Description:	This function will print the image specified by the caller
//					within the rectangle specified by the caller using the
//					TMPower control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::PrintTMPower(RECT* pImage, LPCSTR lpFilename, long lSlide)
{
	char szPath[MAX_PATH];
	char szFilespec[MAX_PATH];

	ASSERT(pImage);
	ASSERT(lpFilename);
	ASSERT(m_pDC);

	//	Don't bother if we don't have a viewer
	if(theStatus == 0)
		return;

	//	Don't bother if the PowerPoint viewer is not initialized
	if(!theStatus->PowerPointInitialized())
		return;

	//	Notify the status dialog
	//
	//	NOTE:	We have to do the notification BEFORE loading the file to make
	//			sure the control is visible. Otherwise we will get an error
	//			when we try to advance to a specific slide
	if(theStatus)
		theStatus->OnLoadViewer(TRUE);

	//	Load the file into the viewer
	if(!theStatus->LoadSlide(lpFilename, lSlide, m_bUseSlideId))
		return;

	if(m_bShowStatus)
	{
		::SetFocus(theStatus->m_hWnd);
		theApp.DoWaitCursor(-1);
	}

	//	Assemble the specification for a temporary file
	GetTempPath(sizeof(szPath), szPath);
	GetTempFileName(szPath, "", 0, szFilespec);
	
	//	Append the appropriate extenstion
	switch(m_iSaveFormat)
	{
		case TMPOWER_TIF:	lstrcat(szFilespec, ".tif");
							break;
		case TMPOWER_WMF:	lstrcat(szFilespec, ".wmf");
							break;
		case TMPOWER_PNG:	lstrcat(szFilespec, ".png");
							break;
		case TMPOWER_BMP:	lstrcat(szFilespec, ".bmp");
							break;
		case TMPOWER_GIF:	lstrcat(szFilespec, ".gif");
							break;
		case TMPOWER_JPG:
		default:			lstrcat(szFilespec, ".jpg");
							break;
	}

	//	Now save the current slide
	if(!theStatus->SaveSlide(szFilespec, m_iSaveFormat))
	{
		return;
	}

	//	Use the TMView control to print the temporary file
	PrintTMView(pImage, szFilespec, 0, FALSE, 0, "", "", 0);

	//	Delete the temporary file
	_unlink(szFilespec);
}

//==============================================================================
//
// 	Function Name:	CJob::PrintTMView()
//
// 	Description:	This function will print the image specified by the caller
//					within the rectangle specified by the caller using the
//					TMView control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::PrintTMView(RECT* pImage, LPCSTR lpFilename, long lTertiary,
					   BOOL bNotify, LPCSTR lpTreatmentSource,
					   LPCSTR lpSibling, LPCSTR lpSiblingImage, long lFlags)
{
	BOOL bFullImage;

	ASSERT(pImage);
	ASSERT(lpFilename);
	ASSERT(m_pDC);

	//	Don't bother if we don't have a viewer
	if(!theStatus)
		return;

	//	Load the file into the viewer
	if(lTertiary > 0)
	{
		//	Print only the visible portion of the image for treatments
		bFullImage = FALSE;

		if(theStatus->LoadZap(lpFilename, lpTreatmentSource, lpSibling, lpSiblingImage, lFlags) == FALSE)
			return;
	}
	else
	{
		bFullImage = TRUE;

		if(theStatus->LoadImage(lpFilename) == FALSE)
			return;
	}

	//	Notify the status dialog
	if(bNotify)
		theStatus->OnLoadViewer(FALSE);

	//	Print the image
	theStatus->PrintEx(bFullImage, m_pDC, pImage->left, pImage->top,
					   (pImage->right - pImage->left),
					   (pImage->bottom - pImage->top), m_bAutoRotateImage);
}

//==============================================================================
//
// 	Function Name:	CJob::SetBarcodeCharacter()
//
// 	Description:	This function allows the caller to set the character
//					inserted in front of barcode graphics. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetBarcodeCharacter(char cBarcode)
{
	m_cBarcode = cBarcode;
}

//==============================================================================
//
// 	Function Name:	CJob::SetBarcodeFont()
//
// 	Description:	This function allows the caller to set the name of the font
//					used to print barcode graphics.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetBarcodeFont(LPCSTR lpFontName)
{
	if(lpFontName && lstrlen(lpFontName) > 0)
		m_strBarcodeFont = lpFontName;
}

//==============================================================================
//
// 	Function Name:	CJob::SetBreakOnDocument()
//
// 	Description:	This function allows the caller to set the BreakOnDocument 
//					option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetBreakOnDocument(BOOL bBreak)
{
	m_bBreakOnDocument = bBreak;
}

//==============================================================================
//
// 	Function Name:	CJob::SetCalloutFrameColor()
//
// 	Description:	This function allows the caller to set the  
//					CalloutFrameColor option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetCalloutFrameColor(short sColor)
{
	m_sCalloutFrameColor = sColor;
}

//==============================================================================
//
// 	Function Name:	CJob::SetDevModeProperties()
//
// 	Description:	This function is called to set the device mode properties
//					for the print job.
//
// 	Returns:		TRUE if successful
//
//	Notes:			We override the base class processing to perform custom
//					processing when printing
//
//==============================================================================
BOOL CJob::SetDevModeProperties(BOOL bPrinting)
{
	DEVMODE* pDevMode = NULL;

	//	Do the base class processing first
	if(CTMPrinter::SetDevModeProperties(bPrinting) == FALSE)
		return FALSE;

	//	The print logic handles copies and collation when printing
	if(bPrinting)
	{
		ASSERT(m_hDevMode != NULL);
		pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
		ASSERT(pDevMode);

		if(pDevMode->dmFields & DM_COPIES)
			pDevMode->dmCopies = 1;

		if(pDevMode->dmFields & DM_COLLATE)
			pDevMode->dmCollate = FALSE;
	}

	GlobalUnlock(pDevMode);
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CJob::SetEnableDIBPrinting()
//
// 	Description:	This function allows the caller to set the DIBPrinting 
//					support option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetEnableDIBPrinting(BOOL bEnable)
{
	m_bEnableDIBPrinting = bEnable;
}

//==============================================================================
//
// 	Function Name:	CJob::SetEnablePowerPoint()
//
// 	Description:	This function allows the caller to set the PowerPoint 
//					support option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetEnablePowerPoint(BOOL bEnable)
{
	m_bEnablePowerPoint = bEnable;
}

//==============================================================================
//
// 	Function Name:	CJob::SetInsertSlipSheet()
//
// 	Description:	This function allows the caller to set the InsertSlipSheet 
//					option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetInsertSlipSheet(BOOL bInsert)
{
	m_bInsertSlipSheet = bInsert;
}

//==============================================================================
//
// 	Function Name:	CJob::SetOptions()
//
// 	Description:	This function allows the caller to set the Options dialog
//					used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetOptions(COptions* pOptions)
{
	m_pOptions = pOptions;
	theOptions = pOptions;
}

//==============================================================================
//
// 	Function Name:	CJob::SetPrintBorderColor()
//
// 	Description:	This function allows the caller to set the  
//					PrintBorderColor option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetPrintBorderColor(COLORREF crColor)
{
	m_crPrintBorderColor = crColor;
}

//==============================================================================
//
// 	Function Name:	CJob::SetPrintBorderThickness()
//
// 	Description:	This function allows the caller to set the  
//					PrintBorderThickness option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetPrintBorderThickness(float fThickness)
{
	m_fPrintBorderThickness = fThickness;
}

//==============================================================================
//
// 	Function Name:	CJob::SetPrintCalloutBorders()
//
// 	Description:	This function allows the caller to set the  
//					PrintCalloutBorders option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetPrintCalloutBorders(BOOL bPrint)
{
	m_bPrintCalloutBorders = bPrint;
}

//==============================================================================
//
// 	Function Name:	CJob::SetPrintCallouts()
//
// 	Description:	This function allows the caller to set the  
//					PrintCallouts option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetPrintCallouts(BOOL bPrint)
{
	m_bPrintCallouts = bPrint;
}

//==============================================================================
//
// 	Function Name:	CJob::SetSaveFormat()
//
// 	Description:	This function allows the caller to set the identifier to 
//					determine the format of the intermediate file used to print
//					PowerPoint slides
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetSaveFormat(int iFormat)
{
	m_iSaveFormat = iFormat;
}

//==============================================================================
//
// 	Function Name:	CJob::SetShowStatus()
//
// 	Description:	This function allows the caller to set the ShowStatus 
//					option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetShowStatus(BOOL bShowStatus)
{
	m_bShowStatus = bShowStatus;
}

//==============================================================================
//
// 	Function Name:	CJob::SetTemplate()
//
// 	Description:	This function allows the caller to set the template used
//					for print operations. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetTemplate(CTemplate* pTemplate)
{
	m_pTemplate = pTemplate;
}

//==============================================================================
//
// 	Function Name:	CJob::SetUseSlideId()
//
// 	Description:	This function allows the caller to set the UseSlideId 
//					option used during print operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::SetUseSlideId(BOOL bUseSlideId)
{
	m_bUseSlideId = bUseSlideId;
}

//==============================================================================
//
// 	Function Name:	CJob::ShowExtents()
//
// 	Description:	This function is provided as a debugging aide. It will 
//					display the current extents in a standard message box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CJob::ShowExtents(HWND hWnd)
{
	CString M;
	CString T;

	M.Empty();

	T.Format("Logical Width = %.3f (pix/in)\n", (float)m_iXDpi); M += T;
	T.Format("Logical Height = %.3f (pix/in)\n", (float)m_iYDpi); M += T;
	T.Format("Page Width = %d\n", m_iPgWidth); M += T;
	T.Format("Page Height = %d\n", m_iPgHeight); M += T;
	T.Format("Page Left = %d\n", m_rcMax.left); M += T;
	T.Format("Page Top = %d\n", m_rcMax.top); M += T;
	T.Format("Rows = %d\n", m_iRows); M += T;
	T.Format("Columns = %d\n", m_iCols); M += T;
	T.Format("Cells per page = %d\n", m_iCellsPerPage); M += T;
	T.Format("Cell Width = %d\n", m_iCellWidth); M += T;
	T.Format("Cell Height = %d\n", m_iCellHeight); M += T;
	T.Format("Cell Space X = %d\n", m_iCellSpaceX); M += T;
	T.Format("Cell Space Y = %d\n", m_iCellSpaceY); M += T;
	T.Format("Cell Pad X = %d\n", m_iCellPadX); M += T;
	T.Format("Cell Pad Y = %d\n", m_iCellPadY); M += T;
	T.Format("Image Pad X = %d\n", m_iImagePadX); M += T;
	T.Format("Image Pad Y = %d\n", m_iImagePadY); M += T;

	::MessageBox(hWnd, M, "Page Extents", MB_ICONINFORMATION | MB_OK);
}

//==============================================================================
//
// 	Function Name:	CJob::SubtractRect()
//
// 	Description:	This function will subtract the second rectangle from the
//					first.
//
// 	Returns:		None
//
//	Notes:			This function assumes the rectangle being subtracted covers
//					the full width of the first rectangle. Only the overall
//					height of the first rectangle will change.
//
//==============================================================================
void CJob::SubtractRect(RECT* pRect1, RECT* pRect2)
{
	int	iAbove;
	int	iBelow;
	int	iTop;
	int iBottom;

	ASSERT(pRect1);
	ASSERT(pRect2);

	//	Compute the unused space above and below the rectangle being subtracted
	iAbove = pRect2->top - pRect1->top;
	iBelow = pRect1->bottom - pRect2->bottom;

	//	Does the rectangle being subtracted completely overlap the source
	if(iAbove <= 0 && iBelow <= 0)
	{
		iTop    = 0;
		iBottom = 0;
	}

	//	Is there more room above than below?
	else if(iAbove > iBelow)
	{
		iTop = pRect1->top;
		
		//	Is the rectangle being subtracted completeley outside?
		if(pRect2->top > pRect1->bottom)
			iBottom = pRect1->bottom;
		else
			iBottom = pRect2->top;
	}

	//	There must be more room below 
	else
	{
		iBottom = pRect1->bottom;

		//	Is the rectangle being subtracted completely outside?
		if(pRect2->bottom < pRect1->top)
			iTop = pRect1->top;
		else
			iTop = pRect2->bottom;
	}

	//	Reset the target rectangle
	pRect1->top    = iTop;
	pRect1->bottom = iBottom;
}

