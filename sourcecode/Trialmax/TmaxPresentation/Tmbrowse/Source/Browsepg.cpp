//==============================================================================
//
// File Name:	browsepg.cpp
//
// Description:	This file contains member functions of the CTMBrowseProperties
//				class. 
//
// See Also:	browsepg.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-09-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <browseapp.h>
#include <browsepg.h>

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
IMPLEMENT_DYNCREATE(CTMBrowseProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMBrowseProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMBrowseProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMBrowseProperties, "TMBROWSE6.Tmbrowse6PropPage.1",
	0xae05099c, 0xf266, 0x4323, 0x96, 0xfe, 0xb0, 0xca, 0xdd, 0x56, 0x55, 0x62)

//==============================================================================
//
// 	Function Name:	CTMBrowseProperties::CTMBrowsePropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMBrowseProperties::CTMBrowsePropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMBROWSE_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMBrowseProperties::CTMBrowseProperties()
//
// 	Description:	This is the constructor for CTMBrowseProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMBrowseProperties::CTMBrowseProperties() 
				   :COlePropertyPage(IDD, IDS_TMBROWSE_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMBrowseProperties)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMBrowseProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMBrowseProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMBrowseProperties)
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

