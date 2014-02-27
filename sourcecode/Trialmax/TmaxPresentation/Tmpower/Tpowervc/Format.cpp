// Format.cpp : implementation file
//

#include "stdafx.h"
#include "Tpowervc.h"
#include "Format.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CFormat dialog


CFormat::CFormat(CWnd* pParent /*=NULL*/)
	: CDialog(CFormat::IDD, pParent)
{
	//{{AFX_DATA_INIT(CFormat)
	m_iFormat = -1;
	//}}AFX_DATA_INIT
}


void CFormat::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CFormat)
	DDX_Control(pDX, IDC_FORMATS, m_ctrlFormats);
	DDX_LBIndex(pDX, IDC_FORMATS, m_iFormat);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CFormat, CDialog)
	//{{AFX_MSG_MAP(CFormat)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CFormat message handlers

BOOL CFormat::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_ctrlFormats.AddString("TIF - Tagged Image Format");
	m_ctrlFormats.AddString("WMF - Windows Meta File");
	m_ctrlFormats.AddString("BMP - Device Independent Bitmap");
	m_ctrlFormats.AddString("PNG - Portable Network Graphic");
	m_ctrlFormats.AddString("JPG - JPEG Interchange Format");
	m_ctrlFormats.AddString("GIF - Graphics Interchange Format");

	UpdateData(FALSE);
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
