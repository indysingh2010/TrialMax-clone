//==============================================================================
//
// File Name:	tmstatpg.cpp
//
// Description:	This file contains member functions of the CTMStatProperties
//				class. 
//
// Functions:   CTMStatProperties::CTMStatProperties()
//				CTMStatProperties::DoDataExchange()
//				
// See Also:	tmstatpg.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-02-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmstatap.h>
#include <tmstatpg.h>

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
IMPLEMENT_DYNCREATE(CTMStatProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMStatProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMStatProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMStatProperties, "TMSTAT6.TMStatPropPage.1",
	0xec57c550, 0x2b80, 0x46bc, 0xb4, 0x66, 0xd0, 0x73, 0x82, 0x71, 0xb7, 0x62)

//==============================================================================
//
// 	Function Name:	CTMStatProperties::CTMStatPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMStatProperties::CTMStatPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMSTAT_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMStatProperties::CTMStatProperties()
//
// 	Description:	This is the constructor for CTMStatProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMStatProperties::CTMStatProperties() 
				  :COlePropertyPage(IDD, IDS_TMSTAT_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMStatProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMStatProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMStatProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMStatProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

