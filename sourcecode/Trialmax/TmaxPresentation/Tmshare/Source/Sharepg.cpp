//==============================================================================
//
// File Name:	sharepg.cpp
//
// Description:	This file contains member functions of the CTMShareProperties
//				class. 
//
// See Also:	sharepg.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-05-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <shareapp.h>
#include <sharepg.h>

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
IMPLEMENT_DYNCREATE(CTMShareProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMShareProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMShareProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMShareProperties, "TMSHARE6.Tmshare6PropPage.1",
	0xe74d322c, 0x25c8, 0x41f3, 0xbd, 0x37, 0x48, 0x7e, 0xa1, 0x72, 0xb6, 0xd2)

//==============================================================================
//
// 	Function Name:	CTMShareProperties::CTMSharePropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMShareProperties::CTMSharePropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMSHARE_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMShareProperties::CTMShareProperties()
//
// 	Description:	This is the constructor for CTMShareProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMShareProperties::CTMShareProperties() 
				   :COlePropertyPage(IDD, IDS_TMSHARE_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMShareProperties)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMShareProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMShareProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMShareProperties)
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

