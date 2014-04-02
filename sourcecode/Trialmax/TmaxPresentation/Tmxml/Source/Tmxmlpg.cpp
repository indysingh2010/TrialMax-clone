//==============================================================================
//
// File Name:	tmxmlpg.cpp
//
// Description:	This file contains member functions of the CTMXmlProperties
//				class. 
//
// Functions:   CTMXmlProperties::CTMXmlProperties()
//				CTMXmlProperties::DoDataExchange()
//				
// See Also:	tmxmlpg.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-02-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <tmxmlpg.h>

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
IMPLEMENT_DYNCREATE(CTMXmlProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMXmlProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMXmlProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMXmlProperties, "TMXML6.TMXml6PropPage.1",
	0x706f3d75, 0xfa07, 0x498b, 0x9a, 0x7, 0x4, 0x83, 0x79, 0xb, 0xd7, 0x67)

//==============================================================================
//
// 	Function Name:	CTMXmlProperties::CTMXmlPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMXmlProperties::CTMXmlPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMXML_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMXmlProperties::CTMXmlProperties()
//
// 	Description:	This is the constructor for CTMXmlProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMXmlProperties::CTMXmlProperties() 
				  :COlePropertyPage(IDD, IDS_TMXML_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMXmlProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMXmlProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMXmlProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMXmlProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

