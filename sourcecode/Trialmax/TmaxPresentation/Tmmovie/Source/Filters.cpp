//==============================================================================
//
// File Name:	filters.cpp
//
// Description:	This file contains member functions of the CFilterInfo class.
//
// Functions:   CFilterInfo::CFilterInfo()
//				CFilterInfo::DoDataExchange()
//				CFilterInfo::InitActive()
//				CFilterInfo::InitRegistered()
//				CFilterInfo::OnAdvanced()
//				CFilterInfo::OnInitDialog()
//
// See Also:	filters.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-04-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmmovie.h>
#include <filters.h>

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
BEGIN_MESSAGE_MAP(CFilterInfo, CDialog)
	//{{AFX_MSG_MAP(CFilterInfo)
	ON_BN_CLICKED(IDC_ADVANCED, OnAdvanced)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CFilterInfo::CFilterInfo()
//
// 	Description:	This is the constructor for CFilterInfo objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFilterInfo::CFilterInfo(CWnd* pParent) : CDialog(CFilterInfo::IDD, pParent)
{
	//{{AFX_DATA_INIT(CFilterInfo)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	m_pIGraphBuilder = 0;
	m_pIMediaControl = 0;
}

//==============================================================================
//
// 	Function Name:	CFilterInfo::~CFilterInfo()
//
// 	Description:	This is the destructor for CFilterInfo objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFilterInfo::~CFilterInfo()
{
	//	Release the COM interfaces
	//if(m_pIGraphBuilder) m_pIGraphBuilder->Release(); m_pIGraphBuilder = 0;
	//if(m_pIMediaControl) m_pIMediaControl->Release(); m_pIMediaControl = 0;
}


//==============================================================================
//
// 	Function Name:	CFilterInfo::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFilterInfo::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CFilterInfo)
	DDX_Control(pDX, IDC_ADVANCED, m_Advanced);
	DDX_Control(pDX, IDC_ACTFILTERS, m_Active);
	DDX_Control(pDX, IDC_REGFILTERS, m_Registered);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CFilterInfo::InitActive()
//
// 	Description:	This function will initialize the controls used to display
//					information about the active filters
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFilterInfo::InitActive() 
{
	LPTSTR	lpFilters;
	char*	pFilter;
	char*	pNext;
	int		i = 0;

	//	Set up the active filters list
	m_Active.InsertColumn(0, "Active Filters", LVCFMT_LEFT, 200);
	m_Active.InsertColumn(1, "Vendor Info", LVCFMT_LEFT, 200);

	//	Get a pointer to the filter string
	lpFilters = m_strActFilters.GetBuffer(m_strActFilters.GetLength() + 1);
	if(!lpFilters)
		return;

	//	Add an entry for each filter
	pFilter = strtok_s(lpFilters, "\r", &pNext);
	while(pFilter)
	{
		m_Active.InsertItem(i, pFilter);
		
		//	Add the vendor info
		if((pFilter = strtok_s(NULL, "\r", &pNext)) != 0)
		{
			m_Active.SetItemText(i, 1, pFilter);
			pFilter = strtok_s(NULL, "\r", &pNext);
		}
		
		i++;
	}

	//	Release the string buffer
	m_strActFilters.ReleaseBuffer();

	//	Enable the active filter controls if we have any
	if(i > 0)
	{
		m_Active.EnableWindow(TRUE);
		m_Advanced.EnableWindow(TRUE);
	}
}

//==============================================================================
//
// 	Function Name:	CFilterInfo::InitRegistered()
//
// 	Description:	This function will initialize the controls used to display
//					information about the registered filters
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFilterInfo::InitRegistered() 
{
	LPTSTR	lpFilters;
	char*	pFilter;
	char*	pNext;
	int		i = 0;

	//	Set up the registered filters list
	m_Registered.InsertColumn(0, "Registered Filters", LVCFMT_LEFT, 200);

	//	Get a pointer to the filter string
	lpFilters = m_strRegFilters.GetBuffer(m_strRegFilters.GetLength() + 1);
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
	m_strRegFilters.ReleaseBuffer();
}

//==============================================================================
//
// 	Function Name:	CFilterInfo::OnAdvanced()
//
// 	Description:	This function is called to display the properties for the
//					current selection in the active filters list box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFilterInfo::OnAdvanced() 
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
				   "Error", MB_ICONINFORMATION | MB_OK);
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
		MessageBox("No properties available for this filter", "Error",
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
// 	Function Name:	CFilterInfo::OnInitDialog()
//
// 	Description:	This function is called by the framework to initialize the
//					dialog box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CFilterInfo::OnInitDialog() 
{
	//	Perform the base class processing
	CDialog::OnInitDialog();
	
	//	Do we have any active filters?
	if(m_pIMediaControl && m_lActFilters > 0)
		m_Advanced.EnableWindow(TRUE);
	else
		m_Advanced.EnableWindow(FALSE);

	//	Initialize the registered filters
	InitRegistered();

	//	Initialize the active filters
	InitActive();
	
	return TRUE;  
}


