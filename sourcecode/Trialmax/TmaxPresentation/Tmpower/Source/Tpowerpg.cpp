//==============================================================================
//
// File Name:	tpowerpg.cpp
//
// Description:	This file contains member functions of the CTMPowerProperties
//				class. 
//
// See Also:	tpowerpg.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-11-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tpowerap.h>
#include <tpowerpg.h>

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
IMPLEMENT_DYNCREATE(CTMPowerProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMPowerProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMPowerProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMPowerProperties, "TMPOWER6.TMPowerPropPage.1",
	0xe5437362, 0xe9e1, 0x47f8, 0x85, 0xd7, 0xec, 0xfd, 0x46, 0x7e, 0x82, 0x92)

//==============================================================================
//
// 	Function Name:	CTMPowerProperties::CTMPowerPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPowerProperties::CTMPowerPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMPOWER_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMPowerProperties::CTMPowerProperties()
//
// 	Description:	This is the constructor for CTMPowerProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMPowerProperties::CTMPowerProperties() :
	COlePropertyPage(IDD, IDS_TMPOWER_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMPowerProperties)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMPowerProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMPowerProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMPowerProperties)
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

