#if !defined(AFX_TMSTAT_H__E547AA1B_5222_447A_97E6_F8D5AF96085F__INCLUDED_)
#define AFX_TMSTAT_H__E547AA1B_5222_447A_97E6_F8D5AF96085F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


// Dispatch interfaces referenced by this interface
class COleFont;

/////////////////////////////////////////////////////////////////////////////
// CTMStat wrapper class

class CTMStat : public CWnd
{
protected:
	DECLARE_DYNCREATE(CTMStat)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0xc69f0d1, 0x9bb0, 0x4db0, { 0xa6, 0x0, 0xd9, 0x86, 0x21, 0xe8, 0xd8, 0xb3 } };
		return clsid;
	}
	virtual BOOL Create(LPCTSTR lpszClassName,
		LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect,
		CWnd* pParentWnd, UINT nID,
		CCreateContext* pContext = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID); }

    BOOL Create(LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect, CWnd* pParentWnd, UINT nID,
		CFile* pPersist = NULL, BOOL bStorage = FALSE,
		BSTR bstrLicKey = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID,
		pPersist, bStorage, bstrLicKey); }

// Attributes
public:
	BOOL GetAutoInit();
	void SetAutoInit(BOOL);
	short GetVerBuild();
	void SetVerBuild(short);
	short GetVerMajor();
	void SetVerMajor(short);
	short GetVerMinor();
	void SetVerMinor(short);
	BOOL GetEnableErrors();
	void SetEnableErrors(BOOL);
	OLE_COLOR GetBackColor();
	void SetBackColor(OLE_COLOR);
	COleFont GetFont();
	void SetFont(LPDISPATCH);
	OLE_COLOR GetForeColor();
	void SetForeColor(OLE_COLOR);
	short GetAppearance();
	void SetAppearance(short);
	BOOL GetAutosizeFont();
	void SetAutosizeFont(BOOL);
	CString GetStatusText();
	void SetStatusText(LPCTSTR);
	short GetLeftMargin();
	void SetLeftMargin(short);
	short GetRightMargin();
	void SetRightMargin(short);
	short GetTopMargin();
	void SetTopMargin(short);
	short GetBottomMargin();
	void SetBottomMargin(short);
	CString GetVerTextLong();
	void SetVerTextLong(LPCTSTR);
	double GetPlaylistTime();
	void SetPlaylistTime(double);
	double GetElapsedPlaylist();
	void SetElapsedPlaylist(double);
	double GetDesignationTime();
	void SetDesignationTime(double);
	double GetElapsedDesignation();
	void SetElapsedDesignation(double);
	short GetMode();
	void SetMode(short);
	long GetDesignationCount();
	void SetDesignationCount(long);
	long GetDesignationIndex();
	void SetDesignationIndex(long);
	long GetTextLine();
	void SetTextLine(long);
	long GetTextPage();
	void SetTextPage(long);
	short GetVerQEF();
	void SetVerQEF(short);
	CString GetVerTextShort();
	void SetVerTextShort(LPCTSTR);
	CString GetVerBuildDate();
	void SetVerBuildDate(LPCTSTR);

// Operations
public:
	short Initialize();
	short SetPlaylistInfo(long lInfo);
	CString GetClassIdString();
	CString GetRegisteredPath();
	void AboutBox();
	LONG GetStatusBarWidth(void);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMSTAT_H__E547AA1B_5222_447A_97E6_F8D5AF96085F__INCLUDED_)
