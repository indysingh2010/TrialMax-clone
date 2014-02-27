//==============================================================================
//
// File Name:	ppimage.cpp
//
// Description:	This file contains member functions of the CPPImage class
//
// See Also:	ppimage.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-20-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <ppimage.h>

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
IMPLEMENT_DYNCREATE(CPPImage, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPImage, CPropertyPage)
	//{{AFX_MSG_MAP(CPPImage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPImage::CPPImage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPImage::CPPImage() : CPropertyPage(CPPImage::IDD)
{
	//{{AFX_DATA_INIT(CPPImage)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CPPImage::~CPPImage()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPImage::~CPPImage()
{
}

//==============================================================================
//
// 	Function Name:	CPPImage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPImage::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPImage)
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
// 	Function Name:	CPPImage::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPImage::OnInitDialog() 
{
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPPImage::SetImageProperties()
//
// 	Description:	This function is called to set the information for the
//					image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPImage::SetImageProperties(STMVImageProperties* pProperties)
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




