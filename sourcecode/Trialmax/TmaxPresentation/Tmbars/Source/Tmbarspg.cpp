//==============================================================================
//
// File Name:	tmbarspg.cpp
//
// Description:	This file contains member functions of the CTMBarsProperties
//				class. 
//
// See Also:	tmbarspg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-09-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmbarsap.h>
#include <tmbarspg.h>

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
IMPLEMENT_DYNCREATE(CTMBarsProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMBarsProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMBarsProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMBarsProperties, "TMBARS6.TMBarsPropPage.1",
	0x889aaa4f, 0x1eb, 0x41c6, 0x9d, 0xa7, 0x54, 0xe2, 0x57, 0x5b, 0x94, 0x5a)

//==============================================================================
//
// 	Function Name:	CTMBarsProperties::CTMBarsPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMBarsProperties::CTMBarsPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMBARS_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMBarsProperties::CTMBarsProperties()
//
// 	Description:	This is the constructor for CTMBarsProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMBarsProperties::CTMBarsProperties() 
				  :COlePropertyPage(IDD, IDS_TMBARS_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMBarsProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMBarsProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMBarsProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMBarsProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

