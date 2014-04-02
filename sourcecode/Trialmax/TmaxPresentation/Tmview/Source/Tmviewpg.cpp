//==============================================================================
//
// File Name:	tmviewpg.cpp
//
// Description:	This file contains member functions of the CTMViewProperties
//				class. 
//
// See Also:	tmviewpg.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-11-97	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>
#include <tmviewpg.h>

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
IMPLEMENT_DYNCREATE(CTMViewProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMViewProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMViewProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMViewProperties, "TMview6.TmviewPropPage.1",
	0xd8931e66, 0xc833, 0x4c77, 0xbd, 0x52, 0x43, 0xc9, 0xff, 0xa5, 0x4f, 0x4)

//==============================================================================
//
// 	Function Name:	CTMViewProperties::CTMViewPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewProperties::CTMViewPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TM_VIEW6_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMViewProperties::CTMViewProperties()
//
// 	Description:	This is the constructor for CTMViewProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMViewProperties::CTMViewProperties() 
				  :COlePropertyPage(IDD, IDS_TM_VIEW6_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMViewProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMViewProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMViewProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMViewProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

