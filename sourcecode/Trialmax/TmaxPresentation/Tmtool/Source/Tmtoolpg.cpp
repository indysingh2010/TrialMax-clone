//==============================================================================
//
// File Name:	tmtoolpg.cpp
//
// Description:	This file contains member functions of the CTMToolProperties
//				class. 
//
// Functions:   CTMToolProperties::CTMToolProperties()
//				CTMToolProperties::DoDataExchange()
//				
// See Also:	tmtoolpg.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	04-21-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmtoolap.h>
#include <tmtoolpg.h>

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
IMPLEMENT_DYNCREATE(CTMToolProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMToolProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMToolProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMToolProperties, "TMTOOL6.TMToolPropPage.1",
	0x2ef0b985, 0x6302, 0x4ee2, 0xbd, 0xd2, 0xff, 0x7, 0xb9, 0x12, 0x12, 0xae)

//==============================================================================
//
// 	Function Name:	CTMToolProperties::CTMToolPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolProperties::CTMToolPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMTOOL_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMToolProperties::CTMToolProperties()
//
// 	Description:	This is the constructor for CTMToolProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMToolProperties::CTMToolProperties() 
				  :COlePropertyPage(IDD, IDS_TMTOOL_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMToolProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMToolProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMToolProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMToolProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}
