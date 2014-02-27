//==============================================================================
//
// File Name:	tmoviepg.cpp
//
// Description:	This file contains member functions of the CTMMovieProperties
//				class. 
//
// Functions:   CTMMovieProperties::CTMMovieProperties()
//				CTMMovieProperties::DoDataExchange()
//				
// See Also:	tmoviepg.h
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
#include <tmovieap.h>
#include <tmoviepg.h>

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
IMPLEMENT_DYNCREATE(CTMMovieProperties, COlePropertyPage)

BEGIN_MESSAGE_MAP(CTMMovieProperties, COlePropertyPage)
	//{{AFX_MSG_MAP(CTMMovieProperties)
	// NOTE - ClassWizard will add and remove message map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/* Replace 5 */
IMPLEMENT_OLECREATE_EX(CTMMovieProperties, "TMMOVIE6.TMMoviePropPage.1",
	0xdb2a439a, 0xbb7b, 0x4f29, 0xbd, 0xca, 0x71, 0x74, 0x4f, 0x7e, 0x7d, 0xbd)


//==============================================================================
//
// 	Function Name:	CTMMovieProperties::CTMMoviePropertiesFactory::UpdateRegistry()
//
// 	Description:	This function will add/remove entries in the system 
//					registry for this class.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieProperties::CTMMoviePropertiesFactory::UpdateRegistry(BOOL bRegister)
{
	if (bRegister)
		return AfxOleRegisterPropertyPageClass(AfxGetInstanceHandle(),
			m_clsid, IDS_TMMOVIE_PPG);
	else
		return AfxOleUnregisterClass(m_clsid, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMMovieProperties::CTMMovieProperties()
//
// 	Description:	This is the constructor for CTMMovieProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMMovieProperties::CTMMovieProperties() :
	COlePropertyPage(IDD, IDS_TMMOVIE_PPG_CAPTION)
{
	//{{AFX_DATA_INIT(CTMMovieProperties)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CTMMovieProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					CTMMovieProperties object and the dialog interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieProperties::DoDataExchange(CDataExchange* pDX)
{
	//{{AFX_DATA_MAP(CTMMovieProperties)
	//}}AFX_DATA_MAP
	DDP_PostProcessing(pDX);
}

