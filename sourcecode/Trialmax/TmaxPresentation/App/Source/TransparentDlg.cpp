// TransparentDlg.cpp : implementation file
//

#include "stdafx.h"
#include "TransparentDlg.h"


// TransparentDlg dialog

IMPLEMENT_DYNAMIC(TransparentDlg, CDialog)

TransparentDlg::TransparentDlg(CWnd* pParent /*=NULL*/)
	: CDialog(TransparentDlg::IDD, pParent)
{

}

TransparentDlg::~TransparentDlg()
{
}

void TransparentDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(TransparentDlg, CDialog)
	ON_WM_CREATE()
	ON_WM_LBUTTONDOWN()
//	ON_WM_PAINT()
END_MESSAGE_MAP()


// TransparentDlg message handlers

int TransparentDlg::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  Add your specialized creation code here

	return 0;
}

void TransparentDlg::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	
	ShellExecute( NULL, "open", "TabTip.exe", NULL, NULL, SW_SHOWNORMAL );
	CDialog::OnLButtonDown(nFlags, point);
}

//void TransparentDlg::OnPaint()
//{
//	//CPaintDC dc(this); // device context for painting
//	//// TODO: Add your message handler code here
//	//// Do not call CDialog::OnPaint() for painting messages
//
//
//	//CBitmap cbm;
//	//cbm.LoadBitmapA(IDB_VKB);
//	//HBITMAP m_hBitmap = (HBITMAP) cbm;
//
//	//if (m_hBitmap == NULL)
//	//{
//	//	AfxMessageBox("Error loading kb bitmap");
//	//	return;
//	//}
//	////Get information about the bitmap..
//	//BITMAP m_Bitmap;
//	//GetObject(m_hBitmap, sizeof(m_Bitmap), &m_Bitmap);	// Get info about the bitmap 
//	//// Put the bitmap into a memory device context
//	//CPaintDC dc(this);
//	////get a memory dc object
//	//CDC dcMem;
//	////create a compatible dc
//	//dcMem.CreateCompatibleDC(&dc);	// Select the bitmap into the in-memory DC
//	////Select the bitmap into the dc
//	//CBitmap* pOldBitmap = dcMem.SelectObject(CBitmap::FromHandle(m_hBitmap));
//	////Create a couple of region objects.
//	//CRgn crRgn, crRgnTmp;
//	////create an empty region
//	//crRgn.CreateRectRgn(0, 0, 0, 0);
//	////Create a region from a bitmap with transparency colour of Purple
//	////COLORREF crTransparent = RGB(163,73,164);	
//	//COLORREF crTransparent = RGB(255, 255, 255);	
//	//int iX = 0;
//	//int iY = 0;
//	//for (iY = 0; iY < m_Bitmap.bmHeight; iY++)
//	//{
//	//	do
//	//	{
//	//		//skip over transparent pixels at start of lines.
//	//		while (iX <= m_Bitmap.bmWidth && dcMem.GetPixel(iX, iY) == crTransparent)
//	//			iX++;
//	//		//remember this pixel
//	//		int iLeftX = iX;
//	//		//now find first non transparent pixel
//	//		while (iX <= m_Bitmap.bmWidth && dcMem.GetPixel(iX, iY) != crTransparent)
//	//			++iX;
//	//		//create a temp region on this info
//	//		crRgnTmp.CreateRectRgn(iLeftX, iY, iX, iY+1);
//	//		//combine into main region.
//	//		crRgn.CombineRgn(&crRgn, &crRgnTmp, RGN_OR);
//	//		//delete the temp region for next pass (otherwise you'll get an ASSERT)
//	//		crRgnTmp.DeleteObject();
//	//	} while(iX < m_Bitmap.bmWidth);
//	//	iX = 0;
//	//}
//	////Centre it on current desktop
//	//SetWindowRgn(crRgn, TRUE);
//	////CRect rect;
//	////this->GetWindowRect(&rect);
//	////iX = (rect.Width()) / 2 - (m_Bitmap.GetBitmapDimension().cx / 2);
//	////iY = (rect.Height()) / 2 - (m_Bitmap.GetBitmapDimension().cy / 2);
//	////SetWindowPos(&wndTopMost, iX, iY, m_Bitmap.GetBitmapDimension().cx, m_Bitmap.GetBitmapDimension().cy, NULL); 
//
//	//// Free resources.
//	//dcMem.SelectObject(pOldBitmap);	// Put the original bitmap back (prevents memory leaks)
//	//dcMem.DeleteDC();
//	//crRgn.DeleteObject();
//
//
//
//
//
//
//
//
//
//
//
//
//}
