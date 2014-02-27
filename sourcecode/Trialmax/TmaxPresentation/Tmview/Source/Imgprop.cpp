//==============================================================================
//
// File Name:	imgprop.cpp
//
// Description:	This file contains member functions of the CImageProperties
//				class. 
//
// See Also:	imgprop.h
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	03-10-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>
#include <imgprop.h>
#include <ltfil.h>

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
BEGIN_MESSAGE_MAP(CImageProperties, CDialog)
	//{{AFX_MSG_MAP(CImageProperties)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CImageProperties::CImageProperties()
//
// 	Description:	This is the constructor for CImageProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CImageProperties::CImageProperties(CWnd* pParent /*=NULL*/)
	: CDialog(CImageProperties::IDD, pParent)
{
	//{{AFX_DATA_INIT(CImageProperties)
	m_strFilename = _T("");
	m_strBitsPerPixel = _T("");
	m_strDiskSize = _T("");
	m_strRamSize = _T("");
	m_strDimInches = _T("");
	m_strDimPixels = _T("");
	m_strPage = _T("");
	m_strCompression = _T("");
	m_strImageType = _T("");
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CImageProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and dialog box controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CImageProperties::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CImageProperties)
	DDX_Text(pDX, IDC_FILENAME, m_strFilename);
	DDX_Text(pDX, IDC_BITSPERPIXEL, m_strBitsPerPixel);
	DDX_Text(pDX, IDC_SIZEDISK, m_strDiskSize);
	DDX_Text(pDX, IDC_SIZERAM, m_strRamSize);
	DDX_Text(pDX, IDC_DIM_INCHES, m_strDimInches);
	DDX_Text(pDX, IDC_DIM_PIXELS, m_strDimPixels);
	DDX_Text(pDX, IDC_PAGE, m_strPage);
	DDX_Text(pDX, IDC_COMPRESSION, m_strCompression);
	DDX_Text(pDX, IDC_IMAGE_TYPE, m_strImageType);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CImageProperties::SetImageInfo()
//
// 	Description:	This function is called to set the information for the
//					specified image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CImageProperties::SetImageInfo(STMVImageProperties* pProperties)
{
	ASSERT(pProperties);

	m_strFilename = pProperties->szFilename;
	m_strDimPixels = pProperties->szDimPixels;
	m_strDimInches = pProperties->szDimInches;
	m_strBitsPerPixel = pProperties->szBitsPerPixel;
	m_strDiskSize = pProperties->szDiskSize;
	m_strRamSize = pProperties->szRamSize;
	m_strPage = pProperties->szPage;
	m_strCompression = pProperties->szCompression;
	m_strImageType = pProperties->szType;

	if(IsWindow(m_hWnd))
		UpdateData(FALSE);
}

