//==============================================================================
//
// File Name:	barpage.cpp
//
// Description:	This file contains member functions of the CBarPage class.
//
// See Also:	barpage.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-10-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmbarsap.h>
#include <barpage.h>
#include <tmbars.h>
#include <tmbadefs.h>

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
static short _aSorted[TMTB_MAXBUTTONS];

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CBarPage, CDialog)
	//{{AFX_MSG_MAP(CBarPage)
	ON_BN_CLICKED(IDC_TMTOOL_LOAD, OnLoad)
	ON_WM_TIMER()
	ON_WM_CLOSE()
	ON_CBN_DBLCLK(IDC_TMTOOL_TEMPLATES, OnDblClk)
	ON_BN_CLICKED(IDC_TMTB_AFTER, OnAfter)
	ON_BN_CLICKED(IDC_TMTB_BEFORE, OnBefore)
	ON_BN_CLICKED(IDC_TMTB_REMOVE, OnRemove)
	ON_BN_CLICKED(IDC_TMTOOL_SAVE, OnSave)
	ON_BN_CLICKED(IDC_TMTOOL_USE_MASTER, OnClickUseMaster)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CBarPage::CBarPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBarPage::CBarPage(CWnd* pParent, int iId)
	     :CDialog(CBarPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CBarPage)
	m_bFlat = FALSE;
	m_bFloat = FALSE;
	m_bStretch = FALSE;
	m_bVisible = FALSE;
	m_sSize = -1;
	m_bUseMaster = FALSE;
	//}}AFX_DATA_INIT

	m_iId = iId;
	m_strSection.Empty();
	m_pIni = 0;
	m_pErrors = 0;
	m_iImage   = -1;
	m_iToolbar = -1;

	//	Initialize the mask to include all buttons
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		m_aMap[i] = -1;
		m_aMask[i] = TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CBarPage::DoComparison()
//
// 	Description:	This function is called by Windows to sort the items in a
//					list.
//
// 	Returns:		> 0 if LPItem1 is greater than LPItem2
//					= 0 if LPItem1 equals LPItem2
//					< 0 if LPItem1 is less than LPItem2
//
//	Notes:			None
//
//==============================================================================
int CALLBACK CBarPage::DoComparison(LPARAM LPId1, LPARAM LPId2, 
									LPARAM LPNotUsed)
{
	if(_aSorted[LPId1] < _aSorted[LPId2])
		return -1;
	else if(_aSorted[LPId1] == _aSorted[LPId2])
		return 0;
	else
		return 1;
}

//==============================================================================
//
// 	Function Name:	CBarPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and the associated class members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CBarPage)
	DDX_Control(pDX, IDC_TOOLBAR_LABEL, m_ctrlToolbarLabel);
	DDX_Control(pDX, IDC_TEMPLATES_LABEL, m_ctrlTemplatesLabel);
	DDX_Control(pDX, IDC_PROPERTIES_GROUP, m_ctrlPropertiesGroup);
	DDX_Control(pDX, IDC_AVAILABLE_LABEL, m_ctrlAvailableLabel);
	DDX_Control(pDX, IDC_TMTOOL_USE_MASTER, m_ctrlUseMaster);
	DDX_Control(pDX, IDC_TMTOOL_VISIBLE, m_ctrlVisible);
	DDX_Control(pDX, IDC_TMTOOL_STRETCH, m_ctrlStretch);
	DDX_Control(pDX, IDC_TMTOOL_SMALL, m_ctrlSmall);
	DDX_Control(pDX, IDC_TMTOOL_MEDIUM, m_ctrlMedium);
	DDX_Control(pDX, IDC_TMTOOL_LARGE, m_ctrlLarge);
	DDX_Control(pDX, IDC_TMTOOL_FLOAT, m_ctrlFloat);
	DDX_Control(pDX, IDC_TMTOOL_FLAT, m_ctrlFlat);
	DDX_Control(pDX, IDC_TMTOOL, m_Toolbar);
	DDX_Control(pDX, IDC_TMTOOL_SAVE, m_ctrlSave);
	DDX_Control(pDX, IDC_TMTOOL_LOAD, m_ctrlLoad);
	DDX_Control(pDX, IDC_TMTOOL_TEMPLATES, m_ctrlTemplates);
	DDX_Control(pDX, IDC_TMTB_BEFORE, m_ctrlBefore);
	DDX_Control(pDX, IDC_TMTB_AFTER, m_ctrlAfter);
	DDX_Control(pDX, IDC_TMTB_REMOVE, m_ctrlRemove);
	DDX_Control(pDX, IDC_TMTB_TOOLBAR, m_ctrlToolbar);
	DDX_Control(pDX, IDC_TMTOOL_IMAGES, m_ctrlImages);
	DDX_Check(pDX, IDC_TMTOOL_FLAT, m_bFlat);
	DDX_Check(pDX, IDC_TMTOOL_FLOAT, m_bFloat);
	DDX_Check(pDX, IDC_TMTOOL_STRETCH, m_bStretch);
	DDX_Check(pDX, IDC_TMTOOL_VISIBLE, m_bVisible);
	DDX_Radio(pDX, IDC_TMTOOL_SMALL, m_sSize);
	DDX_Check(pDX, IDC_TMTOOL_USE_MASTER, m_bUseMaster);
	//}}AFX_DATA_MAP
	
	//	Update the button map
	if(pDX->m_bSaveAndValidate)
		UpdateMap();
}

//==============================================================================
//
// 	Function Name:	CBarPage::GetCurSel()
//
// 	Description:	This function will retrieve the index of the current 
//					selection in the list specified by the caller
//
// 	Returns:		The index of the current selection
//
//	Notes:			None
//
//==============================================================================
int CBarPage::GetCurSel(CListCtrl* pList)
{
	ASSERT(pList);

	return pList->GetNextItem(-1, LVNI_ALL | LVIS_SELECTED);
}

//==============================================================================
//
// 	Function Name:	CBarPage::IsMasterToolbar()
//
// 	Description:	Called to determine if this page is being used to configure
//					the master toolbar
//
// 	Returns:		TRUE if configuring the Documents toolbar
//
//	Notes:			None
//
//==============================================================================
BOOL CBarPage::IsMasterToolbar() 
{
	//	The documents toolbar is used as the master
	return (m_strSection.CompareNoCase(TMBARS_DOCUMENT_SECTION) == 0);
}

//==============================================================================
//
// 	Function Name:	CBarPage::LoadTemplate()
//
// 	Description:	This function will load the list specified template from
//					the ini file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::LoadTemplate(LPCSTR lpTemplate) 
{
	ASSERT(lpTemplate);
	ASSERT(m_pIni);

	if(m_pIni == 0)
		return;

	//	Set the ini file to the correct section
	m_pIni->SetSection(lpTemplate);

	m_bVisible = m_pIni->ReadBool(TMBARS_SHOW_LINE, DEFAULT_TMBARS_SHOW); 
	m_bFlat    = m_pIni->ReadBool(TMBARS_FLAT_LINE, DEFAULT_TMBARS_FLAT); 
	m_bStretch = m_pIni->ReadBool(TMBARS_STRETCH_LINE, DEFAULT_TMBARS_STRETCH); 
	m_bFloat   = !m_pIni->ReadBool(TMBARS_DOCK_LINE, DEFAULT_TMBARS_DOCK); 
	m_sSize	   = (short)m_pIni->ReadLong(TMBARS_SIZE_LINE, DEFAULT_TMBARS_SIZE);

	//	Now construct the button map
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
		m_aMap[i] = (short)m_pIni->ReadLong(i, -1);
}

//==============================================================================
//
// 	Function Name:	CBarPage::LoadTemplates()
//
// 	Description:	This function will load the list of toolbar templates from
//					the ini file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::LoadTemplates() 
{
	CString	strOldSection;
	CString	strTemplate;
	char	szTemplate[256];

	ASSERT(m_pIni);

	//	Set the ini file to the appropriate section
	m_pIni->SetSection(TMBARS_TEMPLATES_SECTION);

	//	Clear the current contents
	m_ctrlTemplates.ResetContent();

	//	Read the name of each template defined in the ini file
	for(int i = 1; ; i++)
	{
		m_pIni->ReadString(i, szTemplate, sizeof(szTemplate));
		if(lstrlen(szTemplate) == 0)
			break;

		//	Strip any leading or trailing whitespace
		strTemplate = szTemplate;
		strTemplate.TrimLeft();
		strTemplate.TrimRight();

		//	Add this template to the list
		m_ctrlTemplates.AddString(strTemplate);
	}

	//	Do we have any templates to choose from?
	if(m_ctrlTemplates.GetCount() == 0)
	{
		m_ctrlLoad.EnableWindow(FALSE);
	}
	else
	{
		m_ctrlTemplates.SetCurSel(0);
	}
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnAfter()
//
// 	Description:	This function will insert the button image selected in the
//					image list into the toolbar list at the location after
//					the current selection
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::OnAfter() 
{
	int		iItem;
	int		iItems;
	int		iInsert;
	int		i;
	int		iReplace = 0;
	short	sImage;
	short	sCommand;
	short	Replace[TMTB_MAXBUTTONS];

	//	Do we have an image selected in the list?
	if((iItem = GetCurSel(&m_ctrlImages)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}
	else
	{
		//	Get the image and command identifiers
		sCommand = (short)m_ctrlImages.GetItemData(iItem);
		sImage = m_Toolbar.GetImageIndex(sCommand);
	}

	//	Remove this image from the image list
	m_ctrlImages.DeleteItem(iItem);
	if(iItem >= m_ctrlImages.GetItemCount())
		iItem = m_ctrlImages.GetItemCount() - 1;
	m_ctrlImages.SetItemState(iItem, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlImages.EnsureVisible(iItem, FALSE);

	//	Get the index of the current selection in the toolbar list and the
	//	number of items in the list
	iItems = m_ctrlToolbar.GetItemCount();
	iInsert = GetCurSel(&m_ctrlToolbar) + 1;

	//	Delete all the items from the insertion point on
	for(iItem = iInsert; iItem < iItems; iItem++)
	{
		Replace[iReplace++] = (short)m_ctrlToolbar.GetItemData(iInsert);
		m_ctrlToolbar.DeleteItem(iInsert);
	}

	//	Add the new button
	m_ctrlToolbar.InsertItem(iInsert, m_Toolbar.GetButtonLabel(sCommand), 
						     sImage);
	m_ctrlToolbar.SetItemData(iInsert, (DWORD)sCommand);

	//	Where do we start putting back items?
	iItem = iInsert + 1;

	//	Now put the ones we removed back in the list
	for(i = 0; i < iReplace; i++)
	{
		sCommand = Replace[i];
		sImage = m_Toolbar.GetImageIndex(sCommand);
		m_ctrlToolbar.InsertItem(iItem, m_Toolbar.GetButtonLabel(sCommand), 
								 sImage);
		m_ctrlToolbar.SetItemData(iItem, (DWORD)sCommand);
		iItem++;
	} 
	
	m_ctrlToolbar.SetItemState(iInsert, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iInsert, FALSE);
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnBefore()
//
// 	Description:	This function will insert the button image selected in the
//					image list into the toolbar list at the location before
//					the current selection
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::OnBefore() 
{
	int		iItem;
	int		iItems;
	int		iInsert;
	int		i;
	int		iReplace = 0;
	short	sImage;
	short	sCommand;
	short	Replace[TMTB_MAXBUTTONS];

	//	Do we have an image selected in the list?
	if((iItem = GetCurSel(&m_ctrlImages)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}
	else
	{
		//	Get the command and image identifiers
		sCommand = (short)m_ctrlImages.GetItemData(iItem);
		sImage = m_Toolbar.GetImageIndex(sCommand);
	}

	//	Remove this image from the image list
	m_ctrlImages.DeleteItem(iItem);
	if(iItem >= m_ctrlImages.GetItemCount())
		iItem = m_ctrlImages.GetItemCount() - 1;
	m_ctrlImages.SetItemState(iItem, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlImages.EnsureVisible(iItem, FALSE);

	//	Get the index of the current selection in the toolbar list and the
	//	number of items in the list
	if((iInsert = GetCurSel(&m_ctrlToolbar)) < 0)
		iInsert = 0;
	iItems = m_ctrlToolbar.GetItemCount();

	//	Delete all the items from the insertion point on
	for(iItem = iInsert; iItem < iItems; iItem++)
	{
		Replace[iReplace++] = (short)m_ctrlToolbar.GetItemData(iInsert);
		m_ctrlToolbar.DeleteItem(iInsert);
	}

	//	Add the new button
	m_ctrlToolbar.InsertItem(iInsert, m_Toolbar.GetButtonLabel(sCommand), 
							 sImage);
	m_ctrlToolbar.SetItemData(iInsert, (DWORD)sCommand);

	//	Where do we start putting back items?
	iItem = iInsert + 1;

	//	Now put the ones we removed back in the list
	for(i = 0; i < iReplace; i++)
	{
		sCommand = Replace[i];
		sImage = m_Toolbar.GetImageIndex(sCommand);
		m_ctrlToolbar.InsertItem(iItem, m_Toolbar.GetButtonLabel(sCommand), 
								 sImage);
		m_ctrlToolbar.SetItemData(iItem, (DWORD)sCommand);
		iItem++;
	} 
	
	m_ctrlToolbar.SetItemState(iInsert, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iInsert, FALSE);
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnClickUseMaster()
//
// 	Description:	This function is called when the user clicks on the Use
//					Master check box 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::OnClickUseMaster() 
{
	ASSERT(IsMasterToolbar() == FALSE);

	m_bUseMaster = (m_ctrlUseMaster.GetCheck() != 0);

	SetControlStates();	
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnClose()
//
// 	Description:	This function is called when the page is closed.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::OnClose()
{
	//	Kill the timer 
	KillTimer(1);
	CDialog::OnClose();
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnDblClick()
//
// 	Description:	This function is called when the user double clicks on a
//					selection in the templates list box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::OnDblClk() 
{
	//	Load the selected template
	OnLoad();
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnInitDialog()
//
// 	Description:	This function is called by the framework to initialize the
//					dialog box.
//
// 	Returns:		TRUE for default focus
//
//	Notes:			None
//
//==============================================================================
BOOL CBarPage::OnInitDialog() 
{
	CBitmap	bmButtons;

	//	Perform base class initialization
	CDialog::OnInitDialog();
	
	m_Toolbar.GetSortOrder(_aSorted);

	//	Load the bitmaps for the image lists
	bmButtons.LoadBitmap(IDB_TBBITMAPS);
	m_Images.Create(24, 18, ILC_MASK | ILC_COLOR24, 0, 1);
	m_Images.Add(&bmButtons, RGB(255,0,255));  

	//	Attach the image list to the list controls
	m_ctrlImages.SetImageList(&m_Images, LVSIL_SMALL);
	m_ctrlToolbar.SetImageList(&m_Images, LVSIL_SMALL);

	//	Set up the controls
	ResetOptions();

	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnLoad()
//
// 	Description:	This function is called when the user clicks on the button
//					to load a toolbar template
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::OnLoad() 
{
	CString strTemplate;

	//	Get the name of the template to load
	m_ctrlTemplates.GetWindowText(strTemplate);
	
	//	Is the template name empty?
	strTemplate.TrimLeft();
	strTemplate.TrimRight();
	if(strTemplate.GetLength() == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMBARS_NOTEMPLATE);
		return;
	}

	//	Load the requested template
	LoadTemplate(strTemplate);

	//	Reset the toolbar options
	ResetOptions();
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnRemove()
//
// 	Description:	This function will remove the current selection from the
//					toolbar list and put it back in the image list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::OnRemove() 
{
	short	sImage;
	short	sCommand;
	int		iImages;
	int		iItem;

	//	Do we have an image selected in the toolbar?
	if((iItem = GetCurSel(&m_ctrlToolbar)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}

	//	Get the command id and image index for the selected image
	sCommand = (short)m_ctrlToolbar.GetItemData(iItem);
	sImage = m_Toolbar.GetImageIndex(sCommand);
	ASSERT(sImage >= 0);
	ASSERT(sCommand >= 0);
		
	//	Remove this item from the toolbar list
	m_ctrlToolbar.DeleteItem(iItem);
	if(iItem == m_ctrlToolbar.GetItemCount())
		iItem--;
	m_ctrlToolbar.SetItemState(iItem, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iItem, FALSE);

	//	Put it back in the image list
	iImages = m_ctrlImages.GetItemCount();
	m_ctrlImages.InsertItem(iImages, m_Toolbar.GetButtonLabel(sCommand), 
						    sImage);
	m_ctrlImages.SetItemData(iImages, (DWORD)sCommand);
	m_ctrlImages.SortItems(DoComparison, 0);
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnSave()
//
// 	Description:	This function will save the current settings as a toolbar
//					template.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::OnSave() 
{
	CString			strOldSection;
	CString			strTemplate;
	CDataExchange	Exchange(this, TRUE);
	int				i;
	char			szTemplate[256];

	ASSERT(m_pIni);
	if(m_pIni == 0)
		return;

	//	Get the name of the template to save
	m_ctrlTemplates.GetWindowText(strTemplate);
	
	//	Is the template name empty?
	strTemplate.TrimLeft();
	strTemplate.TrimRight();
	if(strTemplate.GetLength() == 0)
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_TMBARS_NOTEMPLATE);
		return;
	}

	//	Update the class members
	DDX_Check(&Exchange, IDC_TMTOOL_FLAT, m_bFlat);
	DDX_Check(&Exchange, IDC_TMTOOL_FLOAT, m_bFloat);
	DDX_Check(&Exchange, IDC_TMTOOL_STRETCH, m_bStretch);
	DDX_Check(&Exchange, IDC_TMTOOL_VISIBLE, m_bVisible);
	DDX_Radio(&Exchange, IDC_TMTOOL_SMALL, m_sSize);
	UpdateMap();

	//	Set the ini file to the correct section
	strOldSection = m_pIni->strSection;
	m_pIni->SetSection(strTemplate);

	//	Delete the existing information
	m_pIni->DeleteSection(strTemplate);

	//	Write the toolbar flags
	m_pIni->WriteBool(TMBARS_SHOW_LINE, m_bVisible); 
	m_pIni->WriteBool(TMBARS_FLAT_LINE, m_bFlat); 
	m_pIni->WriteBool(TMBARS_STRETCH_LINE, m_bStretch); 
	m_pIni->WriteBool(TMBARS_DOCK_LINE, !m_bFloat); 
	m_pIni->WriteLong(TMBARS_SIZE_LINE, m_sSize);

	//	Now write the button map
	for(i = 0; i < TMTB_MAXBUTTONS; i++)		
	{
		if(m_aMap[i] < 0)
			break;
		else
			m_pIni->WriteLong(i, m_aMap[i]);
	}

	//	Add this name to the list of templates if it's not already
	//	there
	if((i = m_ctrlTemplates.FindStringExact(-1, strTemplate)) == CB_ERR)
		i = m_ctrlTemplates.AddString(strTemplate);

	//	Select this template
	m_ctrlTemplates.SetCurSel(i);

	//	Update the list of templates in the ini file
	m_pIni->DeleteSection(TMBARS_TEMPLATES_SECTION);
	m_pIni->SetSection(TMBARS_TEMPLATES_SECTION);
	for(i = 0; i < m_ctrlTemplates.GetCount(); i++)
	{
		m_ctrlTemplates.GetLBText(i, strTemplate);
		lstrcpyn(szTemplate, strTemplate, sizeof(szTemplate));
		m_pIni->WriteString((i + 1), szTemplate);
	}

	//	Make sure the Load button is enabled
	m_ctrlLoad.EnableWindow(TRUE);

	//	Restore the ini file
	m_pIni->SetSection(strOldSection);
}

//==============================================================================
//
// 	Function Name:	CBarPage::OnTimer()
//
// 	Description:	This function handles all WM_TIMER messages.
//
// 	Returns:		None
//
//	Notes:			This page uses a timer to check the selections in the
//					list boxes on a periodic basis and enable/disable the
//					associated controls. This is easier than keeping track of
//					the selections with change notifications
//
//==============================================================================
void CBarPage::OnTimer(UINT nIDEvent) 
{
	//	Don't bother if using the master toolbar
	if(m_bUseMaster == FALSE)
	{
		//	Do we have an image selected?
		if(GetCurSel(&m_ctrlImages) < 0)
		{
			m_ctrlBefore.EnableWindow(FALSE);
			m_ctrlAfter.EnableWindow(FALSE);
		}
		else
		{
			m_ctrlBefore.EnableWindow(TRUE);
			m_ctrlAfter.EnableWindow(TRUE);
		}

		//	Do we have a toolbar button selected?
		if(GetCurSel(&m_ctrlToolbar) < 0)
		{
			m_ctrlRemove.EnableWindow(FALSE);
		}
		else
		{
			m_ctrlRemove.EnableWindow(TRUE);
		}

	}// if(m_bUseMaster == TRUE)
}

//==============================================================================
//
// 	Function Name:	CBarPage::ReadIniFile()
//
// 	Description:	This function is called by the control to initialize the 
//					page using the information stored in the ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::ReadIniFile(CTMIni* pIni) 
{
	ASSERT(pIni);
	
	//	Save the pointer to the ini file
	m_pIni = pIni;

	//	Stop here if we don't have an ini file
	if(m_pIni == 0)
		return;
	
	//	Set the ini file to the correct section
	pIni->SetSection(m_strSection);

	//	Get the toolbar flags
	m_bVisible = pIni->ReadBool(TMBARS_SHOW_LINE, DEFAULT_TMBARS_SHOW); 
	m_bFlat    = pIni->ReadBool(TMBARS_FLAT_LINE, DEFAULT_TMBARS_FLAT); 
	m_bStretch = pIni->ReadBool(TMBARS_STRETCH_LINE, DEFAULT_TMBARS_STRETCH); 
	m_bFloat   = !pIni->ReadBool(TMBARS_DOCK_LINE, DEFAULT_TMBARS_DOCK); 
	m_sSize	   = (short)pIni->ReadLong(TMBARS_SIZE_LINE, DEFAULT_TMBARS_SIZE);

	if(IsMasterToolbar() == FALSE)
		m_bUseMaster = pIni->ReadBool(TMBARS_USE_MASTER_LINE, DEFAULT_TMBARS_USE_MASTER);
	else
		m_bUseMaster = FALSE;

	//	Now construct the button map
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
		m_aMap[i] = (short)pIni->ReadLong(i, -1);

	//	Now load the template descriptors
	LoadTemplates();

	//	Set up the controls
	ResetOptions();
}

//==============================================================================
//
// 	Function Name:	CBarPage::ResetOptions()
//
// 	Description:	This function will reset the dialog box controls to reflect
//					the current options.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::ResetOptions() 
{
	CDataExchange	Exchange(this, FALSE);
	LV_FINDINFO		FindInfo;
	int				iTBList = 0;
	int				i;
	short			sImage;
	
	//	Prevent updates while we change the options
	KillTimer(1);

	//	Clear the image lists
	m_ctrlToolbar.DeleteAllItems();
	m_ctrlImages.DeleteAllItems();

	//	Set up the structure used to locate images
	FindInfo.flags  = LVFI_PARAM;
	FindInfo.psz    = 0;

	//	Place the images in the toolbar list
	for(i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		if(m_aMap[i] < 0)
			break;
				
		if(m_aMap[i] >= TMTB_MAXBUTTONS)
			continue;

		//	Should we mask out this button?
		if(!m_aMask[(m_aMap[i])])
			continue;

		sImage = m_Toolbar.GetImageIndex(m_aMap[i]);

		m_ctrlToolbar.InsertItem(i, m_Toolbar.GetButtonLabel(m_aMap[i]), 
							     sImage);
		m_ctrlToolbar.SetItemData(i, (DWORD)m_aMap[i]);
	}

	//	Now place the images in the general list if they aren't already part
	//	of the toolbar
	for(i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		FindInfo.lParam = i;
		if(m_ctrlToolbar.FindItem(&FindInfo) >= 0)
			continue;

		//	Should we mask out this button?
		if(!m_aMask[i])
			continue;

		m_ctrlImages.InsertItem(iTBList, m_Toolbar.GetButtonLabel(i), 
								m_Toolbar.GetImageIndex(i));
		m_ctrlImages.SetItemData(iTBList, (DWORD)i);
		iTBList++;
	}

	m_ctrlImages.SortItems(DoComparison, 0);

	m_ctrlImages.SetItemState(0, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.SetItemState(0, LVIS_SELECTED, LVIS_SELECTED);

	//	Reset the property controls
	DDX_Check(&Exchange, IDC_TMTOOL_FLAT, m_bFlat);
	DDX_Check(&Exchange, IDC_TMTOOL_FLOAT, m_bFloat);
	DDX_Check(&Exchange, IDC_TMTOOL_STRETCH, m_bStretch);
	DDX_Check(&Exchange, IDC_TMTOOL_VISIBLE, m_bVisible);
	DDX_Check(&Exchange, IDC_TMTOOL_USE_MASTER, m_bUseMaster);
	DDX_Radio(&Exchange, IDC_TMTOOL_SMALL, m_sSize);

	//	Enable / disable the controls
	SetControlStates();
	
	//	Start a timer to check the selections in each list
	SetTimer(1, 250, NULL);
}

//==============================================================================
//
// 	Function Name:	CBarPage::SetButtonMask()
//
// 	Description:	This function is called to set the mask appropriate for the
//					toolbar being configured by this page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::SetButtonMask(int iId) 
{
	//	Start by enabling all buttons
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
		m_aMask[i] = TRUE;

	//	Which toolbar is this page linked to?
	switch(iId)
	{
		case DOCUMENT_PAGE:
		case GRAPHIC_PAGE:
		case PLAYLIST_PAGE:
		case LINK_PAGE:
		case MOVIE_PAGE:
		case POWERPOINT_PAGE:
		default:

			//	These buttons are removed from all TrialMax toolbars
			m_aMask[TMTB_PAUSEMOVIE] = FALSE;
			m_aMask[TMTB_PAUSEDESIGNATION] = FALSE;
			m_aMask[TMTB_ENABLELINKS] = FALSE;
			m_aMask[TMTB_UNUSED1] = FALSE;
			m_aMask[TMTB_DELETEZAP] = FALSE;
			
			// hide color buttons except yellow because now we have color picker list
			m_aMask[TMTB_RED] = FALSE;
			m_aMask[TMTB_GREEN] = FALSE;
			m_aMask[TMTB_BLUE] = FALSE;
			m_aMask[TMTB_BLACK] = FALSE;
			m_aMask[TMTB_WHITE] = FALSE;
			m_aMask[TMTB_DARKRED] = FALSE;
			m_aMask[TMTB_DARKGREEN] = FALSE;
			m_aMask[TMTB_DARKBLUE] = FALSE;
			m_aMask[TMTB_LIGHTRED] = FALSE;
			m_aMask[TMTB_LIGHTGREEN] = FALSE;
			m_aMask[TMTB_LIGHTBLUE] = FALSE;

			break;
	}
}

//==============================================================================
//
// 	Function Name:	CBarPage::SetControlStates()
//
// 	Description:	Called to enable / disable the child controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::SetControlStates() 
{
	//	Is this the page for the master toolbar?
	if(IsMasterToolbar() == TRUE)
	{
		m_ctrlUseMaster.EnableWindow(FALSE);
		m_ctrlUseMaster.ShowWindow(SW_HIDE);
		m_bUseMaster = FALSE;
	}

	m_ctrlVisible.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlFloat.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlStretch.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlFlat.EnableWindow(m_bUseMaster == FALSE);

	m_ctrlSmall.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlMedium.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlLarge.EnableWindow(m_bUseMaster == FALSE);

	m_ctrlBefore.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlAfter.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlRemove.EnableWindow(m_bUseMaster == FALSE);

	m_ctrlSave.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlLoad.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlTemplates.EnableWindow(m_bUseMaster == FALSE);

	m_ctrlToolbar.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlImages.EnableWindow(m_bUseMaster == FALSE);

	m_ctrlAvailableLabel.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlToolbarLabel.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlTemplatesLabel.EnableWindow(m_bUseMaster == FALSE);
	m_ctrlPropertiesGroup.EnableWindow(m_bUseMaster == FALSE);

}

//==============================================================================
//
// 	Function Name:	CBarPage::SetHandler()
//
// 	Description:	This function is called to set the handler this page uses
//					to report errors
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::SetHandler(CErrorHandler* pHandler) 
{
	m_pErrors = pHandler;	
}

//==============================================================================
//
// 	Function Name:	CBarPage::SetId()
//
// 	Description:	This function is called to set the page id
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::SetId(int iId) 
{
	m_iId = iId;	
}

//==============================================================================
//
// 	Function Name:	CBarPage::SetSection()
//
// 	Description:	This function is called to set the section name the page
//					uses to manage the ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::SetSection(LPCSTR lpSection) 
{
	ASSERT(lpSection);
	m_strSection = lpSection;	
}

//==============================================================================
//
// 	Function Name:	CBarPage::UpdateMap()
//
// 	Description:	This function will reconstruct the button map using the
//					toolbar list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::UpdateMap()
{
	int iItems;

	//	How many items are in the list?
	iItems = m_ctrlToolbar.GetItemCount();
	if(iItems > TMTB_MAXBUTTONS)
		iItems = TMTB_MAXBUTTONS;

	//	Mark the end of the toolbar in the map
	if(iItems < TMTB_MAXBUTTONS)
		m_aMap[iItems] = -1;

	//	Rebuild the map
	for(int i = 0; i < iItems; i++)
		m_aMap[i] = (short)m_ctrlToolbar.GetItemData(i);
}

//==============================================================================
//
// 	Function Name:	CBarPage::WriteIniFile()
//
// 	Description:	This function is called by the control to write the new
//					toolbar information to the ini file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBarPage::WriteIniFile(CTMIni* pIni) 
{
	ASSERT(pIni);
	
	UpdateData(TRUE);

	//	Set the ini file to the correct section
	pIni->SetSection(m_strSection);

	//	Delete the existing information
	pIni->DeleteSection(m_strSection);

	//	Write the toolbar flags
	pIni->WriteBool(TMBARS_SHOW_LINE, m_bVisible); 
	pIni->WriteBool(TMBARS_FLAT_LINE, m_bFlat); 
	pIni->WriteBool(TMBARS_STRETCH_LINE, m_bStretch); 
	pIni->WriteBool(TMBARS_DOCK_LINE, !m_bFloat); 
	pIni->WriteLong(TMBARS_SIZE_LINE, m_sSize);
	pIni->WriteBool(TMBARS_USE_MASTER_LINE, m_bUseMaster);

	//	Now write the button map
	UpdateMap();
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)		
	{
		if(m_aMap[i] < 0)
			break;
		else
			pIni->WriteLong(i, m_aMap[i]);
	}
}


