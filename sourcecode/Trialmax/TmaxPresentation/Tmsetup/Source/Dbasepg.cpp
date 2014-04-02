//==============================================================================
//
// File Name:	dbasepg.cpp
//
// Description:	This file contains member functions of the CDatabasePage class.
//
// See Also:	dbasepg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <dbasepg.h>
#include <shlobj.h>

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
BEGIN_MESSAGE_MAP(CDatabasePage, CSetupPage)
	//{{AFX_MSG_MAP(CDatabasePage)
	ON_BN_CLICKED(IDC_BROWSEFOLDER, OnBrowseFolder)
	ON_LBN_SELCHANGE(IDC_CASES, OnSelChange)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CDatabasePage::CDatabasePage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDatabasePage::CDatabasePage(CWnd* pParent) : CSetupPage(CDatabasePage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDatabasePage)
	m_bEnableErrors = FALSE;
	m_bEnablePowerPoint = FALSE;
	m_strCurrent = _T("");
	//}}AFX_DATA_INIT

	m_iCases = 0;
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDatabasePage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDatabasePage)
	DDX_Control(pDX, IDC_CURRENT, m_ctrlCurrent);
	DDX_Control(pDX, IDC_CASES, m_ctrlCases);
	DDX_Check(pDX, IDC_ENABLEERRORS, m_bEnableErrors);
	DDX_Check(pDX, IDC_ENABLEPOWERPOINT, m_bEnablePowerPoint);
	DDX_Text(pDX, IDC_CURRENT, m_strCurrent);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::FillListBox()
//
// 	Description:	This function is called to fill the list box with the names
//					of the most recent case selections.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDatabasePage::FillListBox()
{
	int iIndex = LB_ERR;

	//	Clear the existing cases
	m_ctrlCases.ResetContent();

	//	Now add the most recent selections
	for(int i = 0; i < m_iCases; i++)
	{
		m_ctrlCases.AddString(m_aCases[i]);
	}

	//	Set the current selection
	if(!m_strCurrent.IsEmpty())
		iIndex = m_ctrlCases.FindStringExact(-1, m_strCurrent);

	if(iIndex != LB_ERR)
		m_ctrlCases.SetCurSel(iIndex);
	else
		m_ctrlCases.SetCurSel(0);
	OnSelChange();
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::FindFolder()
//
// 	Description:	This function checks to see if the specified folder exists.
//
// 	Returns:		TRUE if the folder exists.
//
//	Notes:			None
//
//==============================================================================
BOOL CDatabasePage::FindFolder(LPCSTR lpFolder)
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;

	ASSERT(lpFolder);

	if((hFind = FindFirstFile(lpFolder, &FindData)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		FindClose(hFind);
		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::InsertFolder()
//
// 	Description:	This function is called to insert a folder into the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDatabasePage::InsertFolder(LPCSTR lpFolder)
{
	int i;

	//	Is there room in the list?
	if(m_iCases < DBPAGE_MAX_CASES)
	{
		//	Move all the existing cases down
		for(i = m_iCases; i > 0; i--)
			m_aCases[i] = m_aCases[i - 1];

		//	Add the new case
		m_aCases[0] = lpFolder;
		m_iCases++;

		//	Insert the case at the top of the list
		m_ctrlCases.InsertString(0, m_aCases[0]);
	}
	else
	{
		//	Move all the existing cases down
		for(i = (DBPAGE_MAX_CASES - 1); i > 0; i--)
			m_aCases[i] = m_aCases[i - 1];

		//	Add the new case
		m_aCases[0] = lpFolder;
		m_iCases = DBPAGE_MAX_CASES;

		//	Remove the last case
		m_ctrlCases.DeleteString(DBPAGE_MAX_CASES - 1);

		//	Insert the case at the top of the list
		m_ctrlCases.InsertString(0, m_aCases[0]);
	}

	//	Select the new folder
	m_ctrlCases.SetCurSel(0);
	OnSelChange();
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::OnBrowseFolder()
//
// 	Description:	This function opens a dialog that allows the user to select
//					the root folder for the database.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDatabasePage::OnBrowseFolder() 
{
	BROWSEINFO		BrowseInfo;
	IMalloc*		pMalloc;
	LPITEMIDLIST	pItemIDList;
	TCHAR			szFolder[MAX_PATH];
	CString			strFolder;
	int				iIndex;
	
	//	Initialize the browse information
	memset(&BrowseInfo, 0, sizeof(BrowseInfo));
	BrowseInfo.hwndOwner = m_hWnd;
	BrowseInfo.lpszTitle = "Select the case folder";
	BrowseInfo.ulFlags = BIF_RETURNONLYFSDIRS;
	BrowseInfo.pszDisplayName = szFolder;

	//	Open the browser dialog
	if((pItemIDList = SHBrowseForFolder(&BrowseInfo)) == NULL)
		return;

	//	Translate the folder's display name to a path specification
	if(!SHGetPathFromIDList(pItemIDList, szFolder))
		return;

	// Convert to lower case so we can do exact comparisons
	strFolder = szFolder;
	strFolder.MakeLower();

	//	Is this folder already in the list?
	if((iIndex = m_ctrlCases.FindStringExact(-1, strFolder)) != LB_ERR)
	{
		m_ctrlCases.SetCurSel(iIndex);
		OnSelChange();
	}
	else
	{
		//	Add this case to the list
		InsertFolder(strFolder);
	}

	//	Delete the PIDL using the shells task allocator
	if(SHGetMalloc(&pMalloc) != NOERROR)
		return;

	//	Free the memory returned by the shell
	if(pMalloc)
	{
		pMalloc->Free(pItemIDList);
		pMalloc->Release();
	}
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::OnInitDialog()
//
// 	Description:	This function handles initialization of the dialog box
//					controls.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDatabasePage::OnInitDialog() 
{
	//	Perform base class initialization
	CDialog::OnInitDialog();
	
	//	Load the bitmaps for the browse buttons
	m_btnFolder.AutoLoad(IDC_BROWSEFOLDER, this);

	//	Fill the list box and set the current selection
	FillListBox();

	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::OnSelChange()
//
// 	Description:	This function is called when the user selects a new case
//					from the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDatabasePage::OnSelChange() 
{
	int iIndex;
	
	if((iIndex = m_ctrlCases.GetCurSel()) != LB_ERR)
	{
		m_ctrlCases.GetText(iIndex, m_strCurrent);	
	}
	else
	{
		m_strCurrent.Empty();
	}

	m_ctrlCurrent.SetWindowText(m_strCurrent);
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::ReadOptions()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDatabasePage::ReadOptions(CTMIni& rIni)
{
	SDatabaseOptions	Options;
	char				szIniStr[512];

	//	Read the options from the ini file
	rIni.ReadDatabaseOptions(&Options);

	//	Set the class members
	m_bEnableErrors = Options.bEnableErrors;
	m_bEnablePowerPoint = Options.bEnablePowerPoint;
	m_strCurrent = Options.strFolder;

	//	The last case should always appear first in the list of cases
	if(!m_strCurrent.IsEmpty())
	{
		//	Convert to lower case
		m_strCurrent.MakeLower();

		//	Does the case folder still exist?
		if(!FindFolder(m_strCurrent))
		{
			m_strCurrent.Empty();
			m_iCases = 0;
		}
		else
		{
			m_aCases[0] = m_strCurrent;
			m_iCases = 1;
		}
	}

	//	Now build the list of most recent cases
	rIni.SetSection(PRESENTATION_CASES_SECTION);
	for(int i = 0; m_iCases < DBPAGE_MAX_CASES; i++)
	{
		rIni.ReadString(i, szIniStr, sizeof(szIniStr));

		//	Have we run out of cases?
		if(lstrlen(szIniStr) == 0)
			break;

		//	Does this folder still exist on the machine?
		if(FindFolder(szIniStr))
		{
			//	Add this case to the list as long as it's not the current case
			if(m_strCurrent.CompareNoCase(szIniStr))
			{
				m_aCases[m_iCases] = szIniStr;
				m_aCases[m_iCases].MakeLower();
				m_iCases++;
			}
		}
	}

	//	Update the controls
	if(IsWindow(m_hWnd))
	{
		UpdateData(FALSE);
		FillListBox();
	}
}

//==============================================================================
//
// 	Function Name:	CDatabasePage::WriteOptions()
//
// 	Description:	This function is called to write the page options to the
//					ini file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDatabasePage::WriteOptions(CTMIni& rIni)
{
	SDatabaseOptions Options;

	//	Refresh the class members
	UpdateData(TRUE);

	//	Fill the transfer structure
	Options.bEnableErrors = m_bEnableErrors;
	Options.bEnablePowerPoint = m_bEnablePowerPoint;
	Options.strFolder = m_strCurrent;

	//	Write the options to the ini file
	rIni.WriteDatabaseOptions(&Options);

	//	Now write the list of most recent cases
	rIni.SetSection(PRESENTATION_CASES_SECTION);
	rIni.DeleteSection(PRESENTATION_CASES_SECTION);
	for(int i = 0; i < m_iCases; i++)
	{
		rIni.WriteString(i, m_aCases[i]);
	}

	return TRUE;
}


