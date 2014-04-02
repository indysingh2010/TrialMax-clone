//==============================================================================
//
// File Name:	tmlpenpg.cpp
//
// Description:	This file contains member functions of the CTMLpenProperties
//				class. 
//
// Functions:   CTMLpenProperties::CTMLpenProperties()
//				CTMLpenProperties::DoDataExchange()
//				
// See Also:	tmlpenpg.h
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
#include <tmlpenap.h>
#include <tmlpenpg.h>

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
IMPLEMENT_DYNCREATE(CTMLpenProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMLpenProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMLpenProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMLpenProperties, "TMLPEN6.TMLpenPropPage.1",
	0x533d6d66, 0xf8b5, 0x4b04, 0xa8, 0x65, 0xe, 0x95, 0x1f, 0x1f, 0x4, 0x2c)

//==============================================================================
//
// 	Function Name:	CTMLpenProperties::CTMLpenPropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLpenProperties::CTMLpenPropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMLPEN_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMLpenProperties::CTMLpenProperties()
//
// 	Description:	This is the constructor for CTMLpenProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMLpenProperties::CTMLpenProperties() 
				  :COlePropertyPage(IDD, IDS_TMLPEN_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMLpenProperties)
	// NOTE: ClassWizard will add member initialization here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMLpenProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMLpenProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMLpenProperties)
	// NOTE: ClassWizard will add DDP, DDX, and DDV calls here
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

