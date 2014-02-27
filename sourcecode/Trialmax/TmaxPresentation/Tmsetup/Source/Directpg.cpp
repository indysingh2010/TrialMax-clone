//==============================================================================
//
// File Name:	directpg.cpp
//
// Description:	This file contains member functions of the CDirectxPage class.
//
// See Also:	directpg.h
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
#include <tmsetup.h>
#include <directpg.h>
#include <tmmovie.h>

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
extern CTMSetupCtrl* theControl;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CDirectxPage, CSetupPage)
	//{{AFX_MSG_MAP(CDirectxPage)
	ON_BN_CLICKED(IDC_PROPERTIES, OnProperties)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CDirectxPage::CDirectxPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDirectxPage::CDirectxPage(CWnd* pParent) : CSetupPage(CDirectxPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDirectxPage)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	m_pIMediaControl = 0;
	m_strActive.Empty();
}

//==============================================================================
//
// 	Function Name:	CDirectxPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDirectxPage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDirectxPage)
	DDX_Control(pDX, IDC_REGISTERED, m_Registered);
	DDX_Control(pDX, IDC_ACTIVE, m_Active);
	DDX_Control(pDX, IDC_PROPERTIES, m_Properties);
	DDX_Control(pDX, IDC_TMMOVIE, m_TMMovie);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CDirectxPage::InitActive()
//
// 	Description:	This function will initialize the controls used to display
//					information about the active filters
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDirectxPage::InitActive() 
{
	LPTSTR	lpFilters;
	char*	pFilter;
	char*	pNext;
	int		i = 0;

	//	Flush the list
	m_Active.DeleteAllItems();

	//	Do we have any active filters?
	if((m_pIMediaControl == 0) || m_strActive.IsEmpty())
	{
		m_Properties.EnableWindow(FALSE);
		return;
	}
	else
	{
		m_Properties.EnableWindow(TRUE);

	}

	//	Get a pointer to the filter string
	lpFilters = m_strActive.GetBuffer(m_strActive.GetLength() + 1);
	if(!lpFilters)
		return;

	//	Add an entry for each filter
	pFilter = strtok_s(lpFilters, "\r", &pNext);
	while(pFilter)
	{
		m_Active.InsertItem(i++, pFilter);
		pFilter = strtok_s(NULL, "\r", &pNext);
	}

	//	Release the string buffer
	m_strActive.ReleaseBuffer();
}

//==============================================================================
//
// 	Function Name:	CDirectxPage::InitRegistered()
//
// 	Description:	This function will initialize the controls used to display
//					information about the registered filters
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDirectxPage::InitRegistered() 
{
	CString		strRegistered;
	long		lRegistered;
	LPTSTR		lpFilters;
	char*		pFilter;
	char*		pNext;
	int			i = 0;

	//	Get the string of registered filters from the TMMovie control
	strRegistered = m_TMMovie.GetRegFilters(&lRegistered);

	//	Set up the registered filters list
	m_Registered.InsertColumn(0, "", LVCFMT_LEFT, 200);

	//	Get a pointer to the filter string
	lpFilters = strRegistered.GetBuffer(strRegistered.GetLength() + 1);
	if(!lpFilters)
		return;

	//	Add an entry for each filter
	pFilter = strtok_s(lpFilters, "\r", &pNext);
	while(pFilter)
	{
		m_Registered.InsertItem(i++, pFilter);
		pFilter = strtok_s(NULL, "\r", &pNext);
	}

	//	Release the string buffer
	strRegistered.ReleaseBuffer();
}

//==============================================================================
//
// 	Function Name:	CDirectxPage::OnInitDialog()
//
// 	Description:	This function is called when the page is created
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDirectxPage::OnInitDialog() 
{
	//	Perform the base class processing
	CDialog::OnInitDialog();
	
	//	Initialize the registered filters
	InitRegistered();

	//	Initialize the active filters
	m_Active.InsertColumn(0, "", LVCFMT_LEFT, 200);
	InitActive();
	
	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CDirectxPage::OnProperties()
//
// 	Description:	This function is called to display the properties for the
//					current selection in the active filters list box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDirectxPage::OnProperties() 
{
	HRESULT					hResult;
	IBaseFilter*			pIFilter;
	IFilterGraph*			pIGraph;
	ISpecifyPropertyPages*	pIPages;
	CAUUID					Pages;
	int						iItem;
	CString					strFilter;
	BSTR					bszFilter;

	//	Do we have the required interfaces?
	if(!m_pIMediaControl)
		return;

	//	Initialize the item information structure
	if((iItem = m_Active.GetNextItem(-1, LVNI_ALL | LVIS_SELECTED)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		MessageBox("You must select a filter from the active filters list",
				   "Properties", MB_ICONINFORMATION | MB_OK);
		return;
	}
	strFilter = m_Active.GetItemText(iItem, 0);
	bszFilter = strFilter.AllocSysString();

	//	Get a pointer to the filter graph interface
	hResult = m_pIMediaControl->QueryInterface(IID_IFilterGraph,
											  (void**)&pIGraph);
	if(FAILED(hResult))
		return;

	//	Find the filter interface
	hResult = pIGraph->FindFilterByName(bszFilter, &pIFilter);

	//	Release the filter graph interface
	pIGraph->Release();
	
	//	Were we able to get the filter interface?
	if(FAILED(hResult))
		return;

	//	Get the property pages interface for this filter
	hResult = pIFilter->QueryInterface(IID_ISpecifyPropertyPages,
									  (void**)&pIPages);
	if(FAILED(hResult))
	{
		MessageBeep(0xFFFFFFFF);
		MessageBox("No properties available for this filter", "Properties",
					MB_ICONINFORMATION | MB_OK);
		pIFilter->Release();
		return;
	}

	//	Get the property page information
	if(SUCCEEDED(pIPages->GetPages(&Pages)))
	{
		//	Display the property pages
		OleCreatePropertyFrame(	m_hWnd,
								0, 0,
								bszFilter,
								1,
								(LPUNKNOWN*)&pIFilter,
								Pages.cElems,
								Pages.pElems,
								NULL,
								0,
								NULL);
	}

	//	Clean up
	CoTaskMemFree(Pages.pElems);
	pIPages->Release();
	pIFilter->Release();
}

//==============================================================================
//
// 	Function Name:	CDirectxPage::SetActiveFilters()
//
// 	Description:	This function is called to set the active filters and
//					media control interface use to display active multimedia 
//					filters
//
// 	Returns:		None
//
//	Notes:			It is assumed that the active filters and media control
//					interface are obtained from an active TMMovie control.
//
//==============================================================================
void CDirectxPage::SetActiveFilters(LPCTSTR lpFilters, LPUNKNOWN lpMediaControl) 
{
	if(lpFilters)
		m_strActive = lpFilters;
	else
		m_strActive.Empty();

	m_pIMediaControl = (IMediaControl*)lpMediaControl;

	//	Has the window been created yet?
	if(IsWindow(m_hWnd))
		InitActive();
}

