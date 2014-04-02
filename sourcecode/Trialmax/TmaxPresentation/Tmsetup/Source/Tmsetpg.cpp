//==============================================================================
//
// File Name:	tmsetpg.cpp
//
// Description:	This file contains member functions of the CTMSetupProperties
//				class. 
//
// See Also:	tmsetpg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <tmsetpg.h>

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
IMPLEMENT_DYNCREATE(CTMSetupProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMSetupProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMSetupProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMSetupProperties, "TMSETUP6.TMSetupPropPage.1",
	0x60266535, 0x7148, 0x470c, 0x82, 0x1d, 0x87, 0xac, 0xfb, 0x14, 0x40, 0x53)

//==============================================================================
//
// 	Function Name:	CTMSetupProperties::CTMSetupPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMSetupProperties::CTMSetupPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMSETUP_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMSetupProperties::CTMSetupProperties()
//
// 	Description:	This is the constructor for CTMSetupProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMSetupProperties::CTMSetupProperties() 
				  :COlePropertyPage(IDD, IDS_TMSETUP_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMSetupProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMSetupProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMSetupProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMSetupProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

