#if !defined(AFX_ANNTOOLBAR_H__31A112D0_5F5B_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_ANNTOOLBAR_H__31A112D0_5F5B_11D5_8F0A_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.

/////////////////////////////////////////////////////////////////////////////
// CAnnToolbar wrapper class

class CAnnToolbar : public COleDispatchDriver
{
public:
	CAnnToolbar() {}		// Calls COleDispatchDriver default constructor
	CAnnToolbar(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CAnnToolbar(const CAnnToolbar& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:
	BOOL GetVisible();
	void SetVisible(BOOL);
	short GetTool();
	void SetTool(short);
	BOOL GetAutoSetTool();
	void SetAutoSetTool(BOOL);
	short GetButtonCount();
	void SetButtonCount(short);
	short GetButtonColumns();
	void SetButtonColumns(short);
	short GetButtonRows();
	void SetButtonRows(short);
	long GetHWnd();
	void SetHWnd(long);

// Operations
public:
	short Create(float x, float y, short iAlign);
	BOOL GetButtonVisible(short nButton);
	void SetButtonVisible(short nButton, BOOL bNewValue);
	short GetButtonTool(short iButtonIndex);
	void SetButtonTool(short iButtonIndex, short nNewValue);
	short GetButtonToolCount(short iButtonIndex);
	void SetButtonToolCount(short iButtonIndex, short nNewValue);
	short GetButtonMenuTool(short iButtonIndex, short iToolIndex);
	void SetButtonMenuTool(short iButtonIndex, short iToolIndex, short nNewValue);
	short GetButtonMenuTextID(short iButtonIndex, short iToolIndex);
	void SetButtonMenuTextID(short iButtonIndex, short iToolIndex, short nNewValue);
	CString GetButtonMenuText(short iButtonIndex, short iToolIndex);
	void SetButtonMenuText(short iButtonIndex, short iToolIndex, LPCTSTR lpszNewValue);
	long GetButtonBitmapDown(short iButtonIndex);
	void SetButtonBitmapDown(short iButtonIndex, long nNewValue);
	long GetButtonBitmapUp(short iButtonIndex);
	void SetButtonBitmapUp(short iButtonIndex, long nNewValue);
	short GetButtonToolTipTextID(short iButtonIndex);
	void SetButtonToolTipTextID(short iButtonIndex, short nNewValue);
	CString GetButtonToolTipText(short iButtonIndex);
	void SetButtonToolTipText(short iButtonIndex, LPCTSTR lpszNewValue);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ANNTOOLBAR_H__31A112D0_5F5B_11D5_8F0A_00802966F8C1__INCLUDED_)
