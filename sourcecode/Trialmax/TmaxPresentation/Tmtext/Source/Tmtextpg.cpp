//==============================================================================
//
// File Name:	tmtextpg.cpp
//
// Description:	This file contains member functions of the CTMTextProperties
//				class. 
//
// See Also:	tmtextpg.h
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
#include <tmtextap.h>
#include <tmtextpg.h>

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
IMPLEMENT_DYNCREATE(CTMTextProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMTextProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMTextProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMTextProperties, "TMTEXT6.TMTextPropPage.1",
	0xea805940, 0x7eee, 0x4742, 0xa1, 0x46, 0x3c, 0x4d, 0x41, 0x60, 0x30, 0xdc)

//==============================================================================
//
// 	Function Name:	CTMTextProperties::CTMTextPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMTextProperties::CTMTextPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMTEXT_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMTextProperties::CTMTextProperties()
//
// 	Description:	This is the constructor for CTMTextProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMTextProperties::CTMTextProperties() 
				  :COlePropertyPage(IDD, IDS_TMTEXT_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMTextProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMTextProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMTextProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMTextProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

