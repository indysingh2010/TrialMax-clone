//==============================================================================
//
// File Name:	tmprntpg.cpp
//
// Description:	This file contains member functions of the CTMPrintProperties
//				class. 
//
// See Also:	tmprntpg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-13-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmprntap.h>
#include <tmprntpg.h>

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
IMPLEMENT_DYNCREATE(CTMPrintProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMPrintProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMPrintProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMPrintProperties, "TMPRINT6.TMPrintPropPage.1",
	0xe917b2fa, 0xefb7, 0x4b52, 0x8b, 0x57, 0x88, 0x77, 0, 0x51, 0x3, 0xeb)

//==============================================================================
//
// 	Function Name:	CTMPrintProperties::CTMPrintPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrintProperties::CTMPrintPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMPRINT_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMPrintProperties::CTMPrintProperties()
//
// 	Description:	This is the constructor for CTMPrintProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMPrintProperties::CTMPrintProperties() 
				  :COlePropertyPage(IDD, IDS_TMPRINT_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMPrintProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMPrintProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMPrintProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMPrintProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

