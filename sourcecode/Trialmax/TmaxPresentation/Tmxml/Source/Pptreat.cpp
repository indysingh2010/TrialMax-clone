//==============================================================================
//
// File Name:	pptreat.cpp
//
// Description:	This file contains member functions of the CPPTreatment class
//
// See Also:	pptreat.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	07-15-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <pptreat.h>

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
IMPLEMENT_DYNCREATE(CPPTreatment, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPTreatment, CPropertyPage)
	//{{AFX_MSG_MAP(CPPTreatment)
	ON_BN_CLICKED(IDC_SAVE_REQUEST, OnSaveRequest)
	ON_BN_CLICKED(IDC_SAVE_RESPONSE, OnSaveResponse)
	ON_BN_CLICKED(IDC_POST, OnPost)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPTreatment::CPPTreatment()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPTreatment::CPPTreatment() : CPropertyPage(CPPTreatment::IDD)
{
	//{{AFX_DATA_INIT(CPPTreatment)
	m_strTarget = _T("");
	//}}AFX_DATA_INIT

	m_pPutTreatment = 0;
}

//==============================================================================
//
// 	Function Name:	CPPTreatment::~CPPTreatment()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPTreatment::~CPPTreatment()
{
}

//==============================================================================
//
// 	Function Name:	CPPTreatment::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTreatment::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPTreatment)
	DDX_Control(pDX, IDC_RESPONSE_CODE, m_ctrlResponseCode);
	DDX_Control(pDX, IDC_TARGET_LABEL, m_ctrlTargetLabel);
	DDX_Control(pDX, IDC_SAVE_RESPONSE, m_ctrlSaveResponse);
	DDX_Control(pDX, IDC_SAVE_REQUEST, m_ctrlSaveRequest);
	DDX_Control(pDX, IDC_RESPONSE, m_ctrlResponse);
	DDX_Control(pDX, IDC_REQUEST_LABEL, m_ctrlRequestLabel);
	DDX_Control(pDX, IDC_POST, m_ctrlPost);
	DDX_Control(pDX, IDC_HEADERS, m_ctrlHeaders);
	DDX_Control(pDX, IDC_TARGET_URL, m_ctrlTarget);
	DDX_Control(pDX, IDC_REQUEST, m_ctrlRequest);
	DDX_Text(pDX, IDC_TARGET_URL, m_strTarget);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPPTreatment::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPTreatment::OnInitDialog() 
{
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	if((m_pPutTreatment != 0) && (m_pPutTreatment->lpRequest != 0))
	{
		//	Set the control values
		Refresh();
	}
	else
	{
		//	Disable the controls
		m_ctrlTargetLabel.EnableWindow(FALSE);
		m_ctrlSaveResponse.EnableWindow(FALSE);
		m_ctrlSaveRequest.EnableWindow(FALSE);
		m_ctrlResponseCode.EnableWindow(FALSE);
		m_ctrlResponse.EnableWindow(FALSE);
		m_ctrlRequestLabel.EnableWindow(FALSE);
		m_ctrlPost.EnableWindow(FALSE);
		m_ctrlHeaders.EnableWindow(FALSE);
		m_ctrlTarget.EnableWindow(FALSE);
		m_ctrlRequest.EnableWindow(FALSE);
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPPTreatment::OnPost()
//
// 	Description:	This function is called when the user clicks on Post
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTreatment::OnPost() 
{
	//	Get the target url
	UpdateData(TRUE);
	if(m_strTarget.GetLength() == 0)
	{
		//	User Mr-Phil's debugging script as the default
		m_strTarget = "http://www.aitstuff.com/go/envv.cgi";
		m_ctrlTarget.SetWindowText(m_strTarget);
	}

	//	Notify the frame 
	if(m_pXmlFrame)
	{
		if(m_pXmlFrame->OnPostRequest(m_strTarget))
		{
			//	Refresh the controls
			Refresh();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CPPTreatment::OnSaveRequest()
//
// 	Description:	This function is called when the user clicks on Save Request
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTreatment::OnSaveRequest() 
{
	CFileDialog	FileDlg(FALSE);
	char		szFilter[] = "Text Files (*.txt)\0*.txt\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	ASSERT(m_pPutTreatment);
	ASSERT(m_pPutTreatment->lpRequest);
	if((m_pPutTreatment == 0) || (m_pPutTreatment->lpRequest == 0))
		return;

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_LONGNAMES | OFN_NONETWORKBUTTON;
	if(FileDlg.DoModal() == IDOK)
	{
		FILE* fptr = NULL;
		fopen_s(&fptr, szFilename, "wb");

		if(fptr != NULL)
		{
			fwrite(m_pPutTreatment->lpRequest, m_pPutTreatment->dwLength, 1, fptr);
			fclose(fptr);
		}
		else
		{
			CString strError;
			strError.Format("Unable to open %s", szFilename);
			MessageBox(strError, "Error", MB_ICONEXCLAMATION | MB_OK);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CPPTreatment::OnSaveResponse()
//
// 	Description:	This function is called when the user clicks on 
//					Save Response
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTreatment::OnSaveResponse() 
{
	CFileDialog	FileDlg(FALSE);
	char		szFilter[] = "HTML Files (*.htm)\0*.htm\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	ASSERT(m_pPutTreatment);
	if(m_pPutTreatment == 0)
		return;

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_LONGNAMES | OFN_NONETWORKBUTTON;
	if(FileDlg.DoModal() == IDOK)
	{
		FILE* fptr = NULL;
		fopen_s(&fptr, szFilename, "wt");

		if(fptr != NULL)
		{
			fprintf(fptr, "%s\r\n\r\n%s", m_pPutTreatment->strResponseHeader,
										  m_pPutTreatment->strResponse);
			fclose(fptr);
		}
		else
		{
			CString strError;
			strError.Format("Unable to open %s", szFilename);
			MessageBox(strError, "Error", MB_ICONEXCLAMATION | MB_OK);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CPPTreatment::Refresh()
//
// 	Description:	This function is called to refresh the controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTreatment::Refresh() 
{
	LPBYTE	lpRequest;
	CString	strCode;

	if((m_pPutTreatment != 0) && (m_pPutTreatment->lpRequest != 0) &&
	   (m_pPutTreatment->dwLength > 0))
	{
		//	Make a copy of the request
		lpRequest = (LPBYTE)HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, 
									  m_pPutTreatment->dwLength + 1);
		if(lpRequest)
		{
			memcpy(lpRequest, m_pPutTreatment->lpRequest, m_pPutTreatment->dwLength);

			//	Replace any NULL character in the request
			for(DWORD i = 0; i < m_pPutTreatment->dwLength; i++)
			{
				if(lpRequest[i] == 0)
					lpRequest[i] = '_';
			}

			m_ctrlRequest.SetWindowText((LPCSTR)lpRequest);

			HeapFree(GetProcessHeap(), 0, lpRequest);
		}
		else
		{
			m_ctrlRequest.SetWindowText("");
		}

		//	Update the response information
		strCode.Format("Response: %lu", m_pPutTreatment->dwStatusCode);
		m_ctrlResponseCode.SetWindowText(strCode);
		m_ctrlHeaders.SetWindowText(m_pPutTreatment->strResponseHeader);
		m_ctrlResponse.SetWindowText(m_pPutTreatment->strResponse);
	}
	else
	{
		m_ctrlRequest.SetWindowText("");
		m_ctrlHeaders.SetWindowText("");
		m_ctrlResponse.SetWindowText("");
	}
}


