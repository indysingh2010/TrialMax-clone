//==============================================================================
//
// File Name:	devcapsctrl.cpp
//
// Description:	This file contains the implementation of the CDevCapsCtrl class
//
// See Also:	devcapsctrl.h
//
// Copyright FTI Consulting - All Rights Reserved
//
//==============================================================================
//	Date		Revision    Description
//	06-15-2006	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <devcapsctrl.h>

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
BEGIN_MESSAGE_MAP(CDeviceCapsCtrl, CListCtrl)
	//{{AFX_MSG_MAP(CDeviceCapsCtrl)
		// NOTE - the ClassWizard will add and remove mapping macros here.
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::Add()
//
//	Parameters:		lpszName - name to appear in first column
//					iValue   - value to appear in second column
//					bYesNo	 - TRUE if iValue is boolean
//					bHex	 - TRUE if formatted as hex
//
// 	Return Value:	None
//
// 	Description:	Called to add a row to the list
//
//------------------------------------------------------------------------------
void CDeviceCapsCtrl::Add(LPCSTR lpszName, int iValue, BOOL bYesNo, BOOL bHex)
{
	CString	strValue = "";
	
	if(bYesNo == TRUE)
	{
		strValue = (iValue != 0) ? "Yes" : "No";
	}
	else if(bHex == TRUE)
	{
		strValue.Format("%d [0x%x]", iValue);
	}
	else
	{	
		strValue.Format("%d", iValue);
	}

	Add(lpszName, strValue);

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::Add()
//
//	Parameters:		lpszName  - name to appear in first column
//					lpszValue - value to appear in second column
//
// 	Return Value:	None
//
// 	Description:	Called to add a row to the list
//
//------------------------------------------------------------------------------
void CDeviceCapsCtrl::Add(LPCSTR lpszName, LPCSTR lpszValue)
{
	LV_ITEM	lvItem;
	int		iIndex = 0;
	
	lvItem.mask = LVIF_TEXT;
	lvItem.iItem = GetItemCount();
	lvItem.iSubItem = 0;
	lvItem.iImage = 0;
	lvItem.pszText = (LPSTR)lpszName;
	lvItem.lParam = 0;

	if((iIndex = InsertItem(&lvItem)) >= 0)
	{
		SetItemText(iIndex, 1, lpszValue);
	}

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::CDevCapsCtrl()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CDeviceCapsCtrl::CDeviceCapsCtrl()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::~CDevCapsCtrl()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CDeviceCapsCtrl::~CDeviceCapsCtrl()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::Clear()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Called to clear the contents of the list
//
//------------------------------------------------------------------------------
void CDeviceCapsCtrl::Clear()
{
	if(IsWindow(this->m_hWnd))
		this->DeleteAllItems();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::Fill()
//
//	Parameters:		pdc - the device context used to fill the list box
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to fill the list with the capabilities of the 
//					device context provided by the caller
//
//------------------------------------------------------------------------------
BOOL CDeviceCapsCtrl::Fill(CDC* pdc)
{
	//	Can't do it unless the window has been created
	if(!IsWindow(this->m_hWnd)) return FALSE;

	//	Clear the existing contents
	Clear();

	//	Refill the list
	if(pdc != NULL)
	{
		Add("HORZSIZE", pdc->GetDeviceCaps(HORZSIZE), FALSE);
		Add("VERTSIZE", pdc->GetDeviceCaps(VERTSIZE), FALSE);
		Add("HORZRES", pdc->GetDeviceCaps(HORZRES), FALSE);
		Add("VERTRES", pdc->GetDeviceCaps(VERTRES), FALSE);
		Add("LOGPIXELSX", pdc->GetDeviceCaps(LOGPIXELSX), FALSE);
		Add("LOGPIXELSY", pdc->GetDeviceCaps(LOGPIXELSY), FALSE);
		Add("PHYSICALOFFSETX", pdc->GetDeviceCaps(PHYSICALOFFSETX), FALSE);
		Add("PHYSICALOFFSETY", pdc->GetDeviceCaps(PHYSICALOFFSETY), FALSE);
		Add("PHYSICALWIDTH", pdc->GetDeviceCaps(PHYSICALWIDTH), FALSE);
		Add("PHYSICALHEIGHT", pdc->GetDeviceCaps(PHYSICALHEIGHT), FALSE);
	
		Add("BITSPIXEL", pdc->GetDeviceCaps(BITSPIXEL), FALSE);
		Add("PLANES", pdc->GetDeviceCaps(PLANES), FALSE);
		Add("NUMBRUSHES", pdc->GetDeviceCaps(NUMBRUSHES), FALSE);
		Add("NUMPENS", pdc->GetDeviceCaps(NUMPENS), FALSE);
		Add("NUMMARKERS", pdc->GetDeviceCaps(NUMMARKERS), FALSE);
		Add("NUMFONTS", pdc->GetDeviceCaps(NUMFONTS), FALSE);
		Add("NUMCOLORS", pdc->GetDeviceCaps(NUMCOLORS), FALSE);
		Add("PDEVICESIZE", pdc->GetDeviceCaps(PDEVICESIZE), FALSE);
		Add("ASPECTX", pdc->GetDeviceCaps(NUMFONTS), FALSE);
		Add("ASPECTY", pdc->GetDeviceCaps(NUMCOLORS), FALSE);
		Add("ASPECTXY", pdc->GetDeviceCaps(PDEVICESIZE), FALSE);

		Add("RASTERCAPS", pdc->GetDeviceCaps(RASTERCAPS), FALSE, TRUE);
		Add("RC_BITBLT", pdc->GetDeviceCaps(RASTERCAPS) & RC_BITBLT);
		Add("RC_BANDING", pdc->GetDeviceCaps(RASTERCAPS) & RC_BANDING);
		Add("RC_SCALING", pdc->GetDeviceCaps(RASTERCAPS) & RC_SCALING);
		Add("RC_BITMAP64", pdc->GetDeviceCaps(RASTERCAPS) & RC_BITMAP64);
		Add("RC_SAVEBITMAP", pdc->GetDeviceCaps(RASTERCAPS) & RC_SAVEBITMAP);
		Add("RC_DI_BITMAP", pdc->GetDeviceCaps(RASTERCAPS) & RC_DI_BITMAP);
		Add("RC_PALETTE", pdc->GetDeviceCaps(RASTERCAPS) & RC_PALETTE);
		Add("RC_DIBTODEV", pdc->GetDeviceCaps(RASTERCAPS) & RC_DIBTODEV);
		Add("RC_STRETCHBLT", pdc->GetDeviceCaps(RASTERCAPS) & RC_STRETCHBLT);
		Add("RC_STRETCHDIB", pdc->GetDeviceCaps(RASTERCAPS) & RC_STRETCHDIB);
		Add("RC_FLOODFILL", pdc->GetDeviceCaps(RASTERCAPS) & RC_FLOODFILL);

		ListView_SetColumnWidth(m_hWnd, DEVCAPSCTRL_COLUMN_NAME, -1);
		ListView_SetColumnWidth(m_hWnd, DEVCAPSCTRL_COLUMN_VALUE, -2);
	}

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::Fill()
//
//	Parameters:		hDevMode - handle to the device mode to be displayed
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to fill the list with the values in the DEVMODE
//					structure provided by the caller
//
//------------------------------------------------------------------------------
BOOL CDeviceCapsCtrl::Fill(HGLOBAL hDevMode)
{
	DEVMODE* pDevMode = NULL;

	//	Can't do it unless the window has been created
	if(!IsWindow(this->m_hWnd)) return FALSE;

	//	Clear the existing contents
	Clear();

	//	Get a pointer to the device mode
	if(hDevMode != NULL)
		pDevMode = (DEVMODE*)GlobalLock(hDevMode);

	//	Refill the list
	if(pDevMode != NULL)
	{
		Add("dmDeviceName", (LPCSTR)(pDevMode->dmDeviceName));
		Add("dmSpecVersion", pDevMode->dmSpecVersion, FALSE);
		Add("dmDriverVersion", pDevMode->dmDriverVersion, FALSE);
		Add("dmSize", pDevMode->dmSize, FALSE);
		Add("dmDriverExtra", pDevMode->dmDriverExtra, FALSE);
		Add("dmFields", pDevMode->dmFields, FALSE);

		Add("DM_ORIENTATION", pDevMode->dmFields & DM_ORIENTATION ? 1 : 0, FALSE);
		Add("DM_PAPERSIZE", pDevMode->dmFields & DM_PAPERSIZE ? 1 : 0, FALSE);
		Add("DM_PAPERLENGTH", pDevMode->dmFields & DM_PAPERLENGTH ? 1 : 0, FALSE);
		Add("DM_PAPERWIDTH", pDevMode->dmFields & DM_PAPERWIDTH ? 1 : 0, FALSE);
		Add("DM_SCALE", pDevMode->dmFields & DM_SCALE ? 1 : 0, FALSE);
		Add("DM_COPIES", pDevMode->dmFields & DM_COPIES ? 1 : 0, FALSE);
		Add("DM_DEFAULTSOURCE", pDevMode->dmFields & DM_DEFAULTSOURCE ? 1 : 0, FALSE);
		Add("DM_PRINTQUALITY", pDevMode->dmFields & DM_PRINTQUALITY ? 1 : 0, FALSE);
		Add("DM_COLOR", pDevMode->dmFields & DM_COLOR ? 1 : 0, FALSE);
		Add("DM_DUPLEX", pDevMode->dmFields & DM_DUPLEX ? 1 : 0, FALSE);
		Add("DM_YRESOLUTION", pDevMode->dmFields & DM_YRESOLUTION ? 1 : 0, FALSE);
		Add("DM_TTOPTION", pDevMode->dmFields & DM_TTOPTION ? 1 : 0, FALSE);
		Add("DM_COLLATE", pDevMode->dmFields & DM_COLLATE ? 1 : 0, FALSE);
		Add("DM_FORMNAME", pDevMode->dmFields & DM_FORMNAME ? 1 : 0, FALSE);
		Add("DM_LOGPIXELS", pDevMode->dmFields & DM_LOGPIXELS ? 1 : 0, FALSE);
		Add("DM_BITSPERPEL", pDevMode->dmFields & DM_BITSPERPEL ? 1 : 0, FALSE);
		Add("DM_PELSWIDTH", pDevMode->dmFields & DM_PELSWIDTH ? 1 : 0, FALSE);
		Add("DM_PELSHEIGHT", pDevMode->dmFields & DM_PELSHEIGHT ? 1 : 0, FALSE);
		Add("DM_DISPLAYFLAGS", pDevMode->dmFields & DM_DISPLAYFLAGS ? 1 : 0, FALSE);
		Add("DM_DISPLAYFREQUENCY", pDevMode->dmFields & DM_DISPLAYFREQUENCY ? 1 : 0, FALSE);

		Add("dmOrientation", pDevMode->dmOrientation == DMORIENT_PORTRAIT ? "Portrait" : "Landscape");
		Add("dmPaperSize", pDevMode->dmPaperSize, FALSE);
		Add("dmPaperLength", pDevMode->dmPaperLength, FALSE);
		Add("dmPaperWidth", pDevMode->dmPaperWidth, FALSE);
		Add("dmPosition.x", pDevMode->dmPosition.x, FALSE);
		Add("dmPosition.y", pDevMode->dmPosition.y, FALSE);
		Add("dmScale", pDevMode->dmScale, FALSE);
		Add("dmCopies", pDevMode->dmCopies, FALSE);
		Add("dmDefaultSource", pDevMode->dmDefaultSource, FALSE);
		Add("dmPrintQuality", GetPrintQuality((int)(pDevMode->dmPrintQuality)));
		Add("dmColor", pDevMode->dmColor, FALSE);
		Add("dmDuplex", pDevMode->dmDuplex, FALSE);
		Add("dmYResolution", pDevMode->dmYResolution, FALSE);
		Add("dmTTOption", pDevMode->dmTTOption, FALSE);
		Add("dmCollate", pDevMode->dmCollate, FALSE);
		Add("dmFormName", (LPCSTR)(pDevMode->dmFormName));
		Add("dmLogPixels", pDevMode->dmLogPixels, FALSE);
		Add("dmBitsPerPel", pDevMode->dmBitsPerPel, FALSE);
		Add("dmPelsWidth", pDevMode->dmPelsWidth, FALSE);
		Add("dmPelsHeight", pDevMode->dmPelsHeight, FALSE);
		Add("dmDisplayFlags", pDevMode->dmDisplayFlags, FALSE);
		Add("dmDisplayFrequency", pDevMode->dmDisplayFrequency, FALSE);

		ListView_SetColumnWidth(m_hWnd, DEVCAPSCTRL_COLUMN_NAME, -1);
		ListView_SetColumnWidth(m_hWnd, DEVCAPSCTRL_COLUMN_VALUE, -2);
	
		GlobalUnlock(pDevMode);
	}

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::GetPrintQuality()
//
//	Parameters:		iDevModePQ - value retrieved from the DEVMODE structure
//
// 	Return Value:	The print quality as a string
//
// 	Description:	Called to get the print quality as a string
//
//------------------------------------------------------------------------------
CString CDeviceCapsCtrl::GetPrintQuality(int iDevModePQ)
{
	CString strPQ = "";

	if(iDevModePQ > 0)
	{
		strPQ.Format("%d dpi", iDevModePQ);
	}
	else
	{
		switch(iDevModePQ)
		{
			case DMRES_HIGH:

				strPQ = "High";
				break;

			case DMRES_MEDIUM:

				strPQ = "Medium";
				break;

			case DMRES_LOW:

				strPQ = "Low";
				break;

			case DMRES_DRAFT:

				strPQ = "Draft";
				break;

			default:

				strPQ.Format("%d", iDevModePQ);
				break;

		}
	
	}

	return strPQ;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CDeviceCapsCtrl::Initialize()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Called to initialize the control
//
//------------------------------------------------------------------------------
void CDeviceCapsCtrl::Initialize(CDC* pdc)
{
	//	Insert the columns
	InsertColumn(DEVCAPSCTRL_COLUMN_NAME, "Name", LVCFMT_LEFT, -1);
	InsertColumn(DEVCAPSCTRL_COLUMN_VALUE, "Value", LVCFMT_RIGHT, -2);
}

