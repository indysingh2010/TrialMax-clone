//==============================================================================
//
// File Name:	setline.cpp
//
// Description:	This file contains member functions of the CSetLine class.
//
// See Also:	setline.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-21-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <app.h>
#include <setline.h>
#include <transcpt.h>
#include <playlist.h>
#include <designat.h>

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
BEGIN_MESSAGE_MAP(CSetLine, CDialog)
	//{{AFX_MSG_MAP(CSetLine)
	ON_WM_CTLCOLOR()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CSetLine::AddTranscript()
//
// 	Description:	This function is called to add a transcript to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::AddTranscript(CTranscript* pTranscript)
{
	CString	strText;
	int		iIndex;

	ASSERT(pTranscript);
	if(pTranscript == 0)
		return;

	//	Format the list box text
	strText.Format("%s  %s", pTranscript->m_strTranscriptName, 
						     pTranscript->m_strDate);

	//	Now add it to the list box
	if((iIndex = m_ctrlTranscripts.AddString(strText)) != LB_ERR)
		m_ctrlTranscripts.SetItemData(iIndex, (DWORD)pTranscript);
}

//==============================================================================
//
// 	Function Name:	CSetLine::CSetLine()
//
// 	Description:	This is the constructor for CSetLine objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSetLine::CSetLine(CWnd* pParent) : CDialog(CSetLine::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSetLine)
	m_strLabel = _T("");
	m_iLine = 0;
	m_strMessage = _T("");
	m_iPage = 0;
	m_iTranscripts = -1;
	//}}AFX_DATA_INIT

	m_pTranscripts = 0;
	m_pPlaylist = 0;
	m_brBackGnd.CreateSolidBrush(RGB(0,0,0));
	m_brForeGnd.CreateSolidBrush(RGB(255,255,255));
	ZeroMemory(&m_rcPos, sizeof(m_rcPos));
}

//==============================================================================
//
// 	Function Name:	CSetLine::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and dialog box controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSetLine)
	DDX_Control(pDX, IDC_MESSAGE, m_ctrlMessage);
	DDX_Control(pDX, IDC_TRANSCRIPTS, m_ctrlTranscripts);
	DDX_Control(pDX, IDC_LINE, m_ctrlLine);
	DDX_Control(pDX, IDC_PAGE, m_ctrlPage);
	DDX_Text(pDX, IDC_LABEL, m_strLabel);
	DDX_Text(pDX, IDC_LINE, m_iLine);
	DDX_Text(pDX, IDC_MESSAGE, m_strMessage);
	DDX_Text(pDX, IDC_PAGE, m_iPage);
	DDX_CBIndex(pDX, IDC_TRANSCRIPTS, m_iTranscripts);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CSetLine::FillTranscripts()
//
// 	Description:	This function is called to fill the transcripts list box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::FillTranscripts()
{
	CTranscript*	pTranscript;
	CDesignation*	pDesignation;
	POSITION		Pos;

	//	Clear the existing entries
	m_ctrlTranscripts.ResetContent();
	m_iTranscripts = LB_ERR;

	//	We have to have a list of transcripts
	ASSERT(m_pTranscripts);
	if(m_pTranscripts == NULL)
		return;

	//	Do we have an active playlist?
	if(m_pPlaylist)
	{
		//	Get transcript associated with each designation in the playlist
		//
		//	NOTE: We use our own iterator so we don't mess up the local
		//		  iterator in the playlist
		Pos = m_pPlaylist->m_Designations.GetHeadPosition();
		while(Pos != NULL)
		{
			pDesignation = (CDesignation*)m_pPlaylist->m_Designations.GetNext(Pos);
			if(pDesignation != 0)
			{
				//	Is this transcript already in the list?
				if(GetIndex(pDesignation->m_lTranscriptId) == LB_ERR)
				{
					//	Add this transcript
					pTranscript = m_pTranscripts->Find(pDesignation->m_lTranscriptId);
					if(pTranscript)
						AddTranscript(pTranscript);
				}
			}
		}
	}
	else
	{
		//	Add all the transcripts to the list
		pTranscript = m_pTranscripts->First();
		while(pTranscript)
		{
			AddTranscript(pTranscript);
			pTranscript = m_pTranscripts->Next();
		}
	}

	//	Pre-select the specified transcript
	if(m_lTranscript > 0)
		m_iTranscripts = GetIndex(m_lTranscript);
	else
		m_iTranscripts = 0;
}

//==============================================================================
//
// 	Function Name:	CSetLine::GetIndex()
//
// 	Description:	This function is called to locate the index of the 
//					transcript with the id specified by the caller.
//
// 	Returns:		The index of the transcript if found
//
//	Notes:			None
//
//==============================================================================
int CSetLine::GetIndex(long lTranscriptId)
{
	int iCount = m_ctrlTranscripts.GetCount();
	CTranscript*	pTranscript;

	for(int i = 0; i < iCount; i++)
	{
		if((pTranscript = (CTranscript*)m_ctrlTranscripts.GetItemData(i)) != 0)
		{
			if(pTranscript->m_lTranscriptId == lTranscriptId)
				return i;
		}
	}

	return LB_ERR;
}

//==============================================================================
//
// 	Function Name:	CSetLine::MsgBox()
//
// 	Description:	This is a debugging aide to view the dialog values
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::MsgBox(LPCSTR lpszTitle)
{
	CString	strTemp = "";
	CString	strMsg = "";
	CString strTitle = "";
	
	strTemp.Format("Playlist: %s\n", m_pPlaylist != NULL ? m_pPlaylist->m_strName : "NULL");
	strMsg += strTemp;

	strTemp.Format("Transcripts: %ld\n", m_pTranscripts != NULL ? m_pTranscripts->GetCount() : -1);
	strMsg += strTemp;

	strTemp.Format("Transcript: %ld\n", m_lTranscript);
	strMsg += strTemp;

	strTemp.Format("Transcript Index: %d\n", m_iTranscripts);
	strMsg += strTemp;

	strTemp.Format("Page: %d\n", m_iPage);
	strMsg += strTemp;

	strTemp.Format("Line: %d\n", m_iLine);
	strMsg += strTemp;

	strTemp.Format("Message: %s\n", m_strMessage);
	strMsg += strTemp;

	if(lpszTitle != NULL)
		strTitle = lpszTitle;

	MessageBox(strMsg, strTitle, MB_ICONINFORMATION | MB_OK);
}

//==============================================================================
//
// 	Function Name:	CSetLine::OnCtlColor()
//
// 	Description:	This function traps all WM_CTLCOLOR messages sent to the
//					dialog box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HBRUSH CSetLine::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG || nCtlColor == CTLCOLOR_STATIC)
	{
		pDC->SetBkMode(TRANSPARENT);
		pDC->SetBkColor(RGB(0,0,0));
		pDC->SetTextColor(RGB(255,255,255));
		return (HBRUSH)m_brBackGnd;
	}
	else
		return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	CSetLine::CSetLine()
//
// 	Description:	This function is called to initialize the dialog box
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CSetLine::OnInitDialog() 
{
	//	Perform the base class initialization
	CDialog::OnInitDialog();
	
	//	Set the size and position if available
	if(!IsRectEmpty(&m_rcPos))
	{
		//	What is the minimum height?
		if((m_rcPos.bottom - m_rcPos.top) < MINIMUM_SETLINE_HEIGHT)
			m_rcPos.top = m_rcPos.bottom - MINIMUM_SETLINE_HEIGHT;

		MoveWindow(&m_rcPos);
	}
	
	//	Update the controls
	FillTranscripts();
	UpdateData(FALSE);

	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CSetLine::OnOK()
//
// 	Description:	This function is called when the user attempts to close the
//					dialog by pressing Enter
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::OnOK() 
{
	CTranscript* pTranscript = 0;

	//	Update the class members
	UpdateData(TRUE);

	//	Get the current transcript selection
	if(m_iTranscripts != LB_ERR)
		pTranscript = (CTranscript*)m_ctrlTranscripts.GetItemData(m_iTranscripts);

	//	Is this a valid page number?
	if(m_iPage <= 0)
	{
		m_strMessage = "Page numbers must be greater than 0";
		m_ctrlMessage.SetWindowText(m_strMessage);
		m_ctrlPage.SetFocus();
		return;
	}
	//	Is this a valid line number?
	else if(m_iLine <= 0)
	{
		m_strMessage = "Line numbers must be greater than 0";
		m_ctrlMessage.SetWindowText(m_strMessage);
		m_ctrlLine.SetFocus();
		return;
	}
	else
	{
		//	Save the transcript id
		if(pTranscript == 0)
			m_lTranscript = 0;
		else
			m_lTranscript = pTranscript->m_lTranscriptId;

		//	Close the dialog box
		CDialog::OnOK();
	}
}

//==============================================================================
//
// 	Function Name:	CSetLine::SetLabel()
//
// 	Description:	This function allows the caller to set the label text
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::SetLabel(LPCSTR lpLabel)
{
	if(lpLabel)
		m_strLabel = lpLabel;
	else
		m_strLabel.Empty();
}

//==============================================================================
//
// 	Function Name:	CSetLine::SetLine()
//
// 	Description:	This function allows the caller to set the line number used
//					to initialize the dialog box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::SetLine(int iLine)
{
	m_iLine = iLine;
}

//==============================================================================
//
// 	Function Name:	CSetLine::SetMessage()
//
// 	Description:	This function allows the caller to set the error message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::SetMessage(LPCSTR lpMessage)
{
	if(lpMessage)
		m_strMessage = lpMessage;
	else
		m_strMessage.Empty();
}

//==============================================================================
//
// 	Function Name:	CSetLine::SetPage()
//
// 	Description:	This function allows the caller to set the page number used
//					to initialize the dialog box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::SetPage(int iPage)
{
	m_iPage = iPage;
}

//==============================================================================
//
// 	Function Name:	CSetLine::SetPlaylist()
//
// 	Description:	This function allows the caller to set the active playlist
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::SetPlaylist(CPlaylist* pPlaylist)
{
	m_pPlaylist = pPlaylist;
}

//==============================================================================
//
// 	Function Name:	CSetLine::SetPos()
//
// 	Description:	This function is called to set the rectangle that will be
//					used to size and position the dialog on initialization
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::SetPos(RECT* pPos) 
{
	if(pPos)
		memcpy(&m_rcPos, pPos, sizeof(m_rcPos));
	else
		ZeroMemory(&m_rcPos, sizeof(m_rcPos));
}

//==============================================================================
//
// 	Function Name:	CSetLine::SetTranscript()
//
// 	Description:	This function allows the caller to set the id of the 
//					transcript used to initialize the dialog box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::SetTranscript(long lTranscriptId)
{
	m_lTranscript = lTranscriptId;
}

//==============================================================================
//
// 	Function Name:	CSetLine::SetTranscript()
//
// 	Description:	This function allows the caller to set the list of
//					transcripts managed by the database
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetLine::SetTranscripts(CTranscripts* pTranscripts)
{
	m_pTranscripts = pTranscripts;
}




