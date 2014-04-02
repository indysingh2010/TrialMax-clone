// Rectangle.cpp : implementation file
//

#include "stdafx.h"
#include "Tmviewvc.h"
#include "Rectangle.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CRectangle dialog


CRectangle::CRectangle(CWnd* pParent /*=NULL*/)
	: CDialog(CRectangle::IDD, pParent)
{
	//{{AFX_DATA_INIT(CRectangle)
	m_iBottom = 965;
	m_iLeft = 20;
	m_iRight = 2438;
	m_bLocked = FALSE;
	m_iTop = 238;
	m_iTransparency = 1;
	//}}AFX_DATA_INIT

	m_crColor = RGB(0x00,0x00,0xFF);
}


void CRectangle::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CRectangle)
	DDX_Control(pDX, IDC_COLOR, m_ctrlColor);
	DDX_Text(pDX, IDC_BOTTOM, m_iBottom);
	DDX_Text(pDX, IDC_LEFT, m_iLeft);
	DDX_Text(pDX, IDC_RIGHT, m_iRight);
	DDX_Check(pDX, IDC_LOCKED, m_bLocked);
	DDX_Text(pDX, IDC_TOP, m_iTop);
	DDX_CBIndex(pDX, IDC_TRANSPARANCY, m_iTransparency);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CRectangle, CDialog)
	//{{AFX_MSG_MAP(CRectangle)
	ON_BN_CLICKED(IDC_COLOR, OnColor)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRectangle message handlers

BOOL CRectangle::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	m_ctrlColor.SetColor(m_crColor);
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CRectangle::OnColor() 
{
	CColorDialog Dialog(m_crColor, CC_PREVENTFULLOPEN | CC_SOLIDCOLOR, this);
	if(Dialog.DoModal() == IDOK)
	{
		m_crColor = Dialog.GetColor();
		m_ctrlColor.SetColor(m_crColor);
		m_ctrlColor.RedrawWindow();
	}
}
