//==============================================================================
//
// File Name:	grabpg.cpp
//
// Description:	This file contains member functions of the CTMGrabProperties
//				class. 
//
// See Also:	grabpg.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	12-27-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <grabapp.h>
#include <grabpg.h>

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
IMPLEMENT_DYNCREATE(CTMGrabProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMGrabProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMGrabProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMGrabProperties, "TMGRAB6.Tmgrab6PropPage.1",
	0x393cac89, 0xe366, 0x4a6e, 0xa9, 0xe4, 0xab, 0xd8, 0xbf, 0x44, 0xb1, 0x46)

//==============================================================================
//
// 	Function Name:	CTMGrabProperties::CTMGrabPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMGrabProperties::CTMGrabPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMGRAB_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMGrabProperties::CTMGrabProperties()
//
// 	Description:	This is the constructor for CTMGrabProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMGrabProperties::CTMGrabProperties() 
				   :COlePropertyPage(IDD, IDS_TMGRAB_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMGrabProperties)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMGrabProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMGrabProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMGrabProperties)
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

