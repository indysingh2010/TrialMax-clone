// VKBDlg.cpp : implementation file
//

#include <stdafx.h>
#include <VKBDlg.h>
#include <atlimage.h>


// CVKBDlg dialog

IMPLEMENT_DYNAMIC(CVKBDlg, CDialog)

STARTUPINFO si;
PROCESS_INFORMATION pi;

CVKBDlg::CVKBDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CVKBDlg::IDD, pParent)
{

}

CVKBDlg::~CVKBDlg()
{
}

void CVKBDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CVKBDlg, CDialog)
	ON_WM_CREATE()
	ON_WM_PAINT()
	ON_WM_LBUTTONDOWN()
END_MESSAGE_MAP()


// CVKBDlg message handlers

void CVKBDlg::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here
	CreateProcess(NULL, "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe", 
		NULL, NULL, false, 0, NULL, NULL, &si, &pi);
}

int CVKBDlg::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;

	// Setting the Transparency of Virtual Keyboard
	SetWindowLong(this->m_hWnd, GWL_EXSTYLE, GetWindowLong(this->m_hWnd, GWL_EXSTYLE) | WS_EX_LAYERED);
	SetLayeredWindowAttributes(RGB(255,255,255), 140, LWA_ALPHA);

	return 0;
}

void CVKBDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);
}

void CVKBDlg::OnPaint()
{
	
	CBitmap cbm;
	cbm.LoadBitmapA(IDB_VKB);
	HBITMAP m_hBitmap = (HBITMAP) cbm;

	if (m_hBitmap == NULL)
	{
		AfxMessageBox("Error loading kb bitmap");
		return;
	}
	//Get information about the bitmap..
	BITMAP m_Bitmap;
	GetObject(m_hBitmap, sizeof(m_Bitmap), &m_Bitmap);	// Get info about the bitmap 
	// Put the bitmap into a memory device context
	CPaintDC dc(this);
	//get a memory dc object
	CDC dcMem;
	//create a compatible dc
	dcMem.CreateCompatibleDC(&dc);	// Select the bitmap into the in-memory DC
	//Select the bitmap into the dc
	CBitmap* pOldBitmap = dcMem.SelectObject(CBitmap::FromHandle(m_hBitmap));
	//Create a couple of region objects.
	CRgn crRgn, crRgnTmp;
	//create an empty region
	crRgn.CreateRectRgn(0, 0, 0, 0);
	//Create a region from a bitmap with transparency colour of Purple
	//COLORREF crTransparent = RGB(163,73,164);	
	COLORREF crTransparent = RGB(255, 255, 255);	
	int iX = 0;
	int iY = 0;
	for (iY = 0; iY < m_Bitmap.bmHeight; iY++)
	{
		do
		{
			//skip over transparent pixels at start of lines.
			while (iX <= m_Bitmap.bmWidth && dcMem.GetPixel(iX, iY) == crTransparent)
				iX++;
			//remember this pixel
			int iLeftX = iX;
			//now find first non transparent pixel
			while (iX <= m_Bitmap.bmWidth && dcMem.GetPixel(iX, iY) != crTransparent)
				++iX;
			//create a temp region on this info
			crRgnTmp.CreateRectRgn(iLeftX, iY, iX, iY+1);
			//combine into main region.
			crRgn.CombineRgn(&crRgn, &crRgnTmp, RGN_OR);
			//delete the temp region for next pass (otherwise you'll get an ASSERT)
			crRgnTmp.DeleteObject();
		} while(iX < m_Bitmap.bmWidth);
		iX = 0;
	}

	//Centre it on current desktop
	SetWindowRgn(crRgn, TRUE);

	// Free resources.
	dcMem.SelectObject(pOldBitmap);	// Put the original bitmap back (prevents memory leaks)
	dcMem.DeleteDC();
	crRgn.DeleteObject();

}

void CVKBDlg::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	CreateProcess(NULL, "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe", 
		NULL, NULL, false, 0, NULL, NULL, &si, &pi);
	CDialog::OnLButtonDown(nFlags, point);
}
