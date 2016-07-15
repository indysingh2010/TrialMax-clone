//==============================================================================
//
// File Name:	player.cpp
//
// Description:	This file contains member functions of the CPlayer class.
//
// Functions:   CPlayer::CaptureFrame()
//				CPlayer::CPlayer()
//				CPlayer::~CPlayer()
//				CPlayer::EnableSnapshots()
//				CPlayer::GetDDBitmap()
//				CPlayer::GetDIBitmap()
//				CPlayer::GetEvent()
//				CPlayer::GetInterface()
//				CPlayer::GetPos()
//				CPlayer::GetRenderWnd()
//				CPlayer::GetSnapshot()
//				CPlayer::GetStartPos()
//				CPlayer::GetState()
//				CPlayer::GetStopPos()
//				CPlayer::GetVideoRect()
//				CPlayer::Initialize()
//				CPlayer::IsLoaded()
//				CPlayer::IsVisible()
//				CPlayer::Load()
//				CPlayer::OnIDXSetPos()
//				CPlayer::OnIDXShow()
//				CPlayer::Pause()
//				CPlayer::Play()
//				CPlayer::Redraw()
//				CPlayer::Resize()
//				CPlayer::SetBalance()
//				CPlayer::SetPos()
//				CPlayer::SetRange()
//				CPlayer::SetRate()
//				CPlayer::SetScaleProps()
//				CPlayer::SetVolume()
//				CPlayer::Show()
//				CPlayer::Step()
//				CPlayer::Stop()
//				CPlayer::Switch()
//				CPlayer::Unload()
//				CPlayer::Update()
//
// See Also:	player.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-12-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <player.h>
#include <tmmovie.h>
#include <resource.h>
#include <tmmvdefs.h>
#include <snapshot.h>
#include <dshow.h>
#include <toolbox.h>

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

//==============================================================================
//
// 	Function Name:	CPlayer::AddFilter()
//
// 	Description:	This function is called to add the specified filter to
//					the list of filters added to the graph builder before
//					rendering.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::AddFilter(LPCSTR lpszName)
{
	CLSID	ClsId;
	int		iIndex;

	ASSERT(lpszName);
	ASSERT(lstrlen(lpszName) > 0);
	ASSERT(m_iFilters < IDXSHOW_MAX_FILTERS);

	//	Do we have room in the array?
	if(m_iFilters < IDXSHOW_MAX_FILTERS)
	{
		//	Find the first available slot
		for(iIndex = 0; iIndex < IDXSHOW_MAX_FILTERS; iIndex++)
		{
			if(m_aFilters[iIndex].strName.IsEmpty())
				break;
		}

		ASSERT(iIndex < IDXSHOW_MAX_FILTERS);
		if(iIndex >= IDXSHOW_MAX_FILTERS)
			return FALSE;

		//	Get the class id for this filter
		if(FindFilter(lpszName, &ClsId))
		{
			//	Store the filter information
			m_aFilters[iIndex].strName = lpszName;
			memcpy(&(m_aFilters[iIndex].ClsId), &ClsId, sizeof(CLSID));
			
			m_iFilters++;

			//	Notify the interface if it exists
			if(m_pIDXShow != 0)
				m_pIDXShow->m_iFilters = m_iFilters;

			return TRUE;
		}
		else
		{
			if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_FINDFILTERFAILED, lpszName);
			return FALSE;
		}
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CPlayer::Capture()
//
// 	Description:	This function is called to capture the current frame and
//					store it in the file provided by the caller
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Capture(CFile* pFile)
{
	BITMAPFILEHEADER	Header;
	BITMAPINFOHEADER*	pBitmap;
	long				lSize;

	ASSERT(pFile);
	ASSERT(pFile->m_hFile != CFile::hFileNull);

	if(!m_pIDXShow || !pFile || pFile->m_hFile == CFile::hFileNull)
		return FALSE;

	//	Get the DIB from the active interface
	if((pBitmap = m_pIDXShow->GetDIBitmap(&lSize)) == 0)
		return FALSE;

	//	Initialize the file header structure
	ZeroMemory(&Header, sizeof(Header));
	Header.bfType = *(WORD*)"BM";
	Header.bfSize = (sizeof(Header) + lSize);

	//	Write the header and bitmap to file
	pFile->Write(&Header, sizeof(Header));
	pFile->Write(pBitmap, lSize);

	//	Free the bitmap memory
	HeapFree(GetProcessHeap(), 0, pBitmap);
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::ConvertToFrames()
//
// 	Description:	This method is called to convert the seconds to frames based
//					on the current video's frame rate
//
// 	Returns:		The frame equivalent
//
//	Notes:			None
//
//==============================================================================
long CPlayer::ConvertToFrames(double dSeconds) 
{
	long lConverted = 0;

	if((m_pIDXShow != NULL) && (m_pIDXShow->ConvertToFrames(dSeconds, &lConverted) == TRUE))
	{
		return lConverted;
	}
	else
	{
		return ((long)((double)m_fDefaultRate * dSeconds));
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::ConvertToTime()
//
// 	Description:	This method is called to convert the specified frame to
//					it's equivalent time position in decimal seconds
//
// 	Returns:		The time equivalent
//
//	Notes:			None
//
//==============================================================================
double CPlayer::ConvertToTime(long lFrame) 
{
	double dConverted = 0;

	if((m_pIDXShow != NULL) && (m_pIDXShow->ConvertToTime(lFrame, &dConverted) == TRUE))
	{
		return dConverted;
	}
	else
	{
		if(m_fDefaultRate > 0)
			return ((double)lFrame / double(m_fDefaultRate));
		else
			return (double)0;
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::CPlayer
//
// 	Description:	This is the constructor for CPlayer objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPlayer::CPlayer()
{
	//	Initialize the local data
	m_pParent			 = 0;
	m_pSnapshot			 = 0;
	m_pErrors			 = 0;
	m_pIDXShow			 = 0;
	m_pIMapper			 = 0;
	m_lIDXShowId		 = 0;
	m_dMinTime			 = (double)0;
	m_dMaxTime			 = (double)0;
	m_lWidth			 = 640;
	m_lHeight			 = 480;
	m_iFilters			 = 0;
	m_iVideoSliderHeight = 0;
	m_sRate				 = 0;
	m_sVolume			 = 0;
	m_sBalance			 = 0;
	m_fAspectRatio		 = (float)m_lHeight / (float)m_lWidth;
	m_fDefaultRate		 = IDXSHOW_FRAMERATE;
	m_fFrameRate		 = IDXSHOW_FRAMERATE;
	m_bAdjRate			 = TRUE;
	m_bAdjVolume		 = TRUE;
	m_bAdjBalance		 = TRUE;
	m_bDetachBeforeLoad  = FALSE;
	m_bSnapshots	     = FALSE;
	m_bScale			 = TRUE;
	m_bAspect			 = TRUE;
	m_bHideTaskBar		 = FALSE;
	m_strFilename.Empty();
	memset(&m_rcMaxWnd, 0, sizeof(m_rcMaxWnd));

	//	Initialize members related to simulation
	m_bSimulating = FALSE;
	m_sSimState	= TMMOVIE_NOTREADY;
	m_dSimStartPosition	= 0.0;
	m_dSimStopPosition	= 0.0;
	m_dSimPosition = 0.0;
	m_ulSimLastTime = 0;
}

//==============================================================================
//
// 	Function Name:	CPlayer::~CPlayer
//
// 	Description:	This is the destructor for CPlayer objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPlayer::~CPlayer()
{
	//	Make sure the interfaces are shut down
	Unload();

	//	Release the static interfaces
	RELEASE_INTERFACE(m_pIMapper);

	//	Make sure the snapshot is destroyed
	if(m_pSnapshot)
		delete m_pSnapshot;
}

//==============================================================================
//
// 	Function Name:	CPlayer::EnableSnapshots()
//
// 	Description:	This function is called to set the flag that controls
//					the use of snapshots on file changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::EnableSnapshots(BOOL bEnable)
{
	m_bSnapshots = bEnable;
}

//==============================================================================
//
// 	Function Name:	CPlayer::EndSimulation()
//
// 	Description:	This function is called to end the playback simulation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::EndSimulation()
{
	if(m_bSimulating == TRUE)
	{
		m_bSimulating = FALSE;
		m_sSimState	= TMMOVIE_NOTREADY;
		m_dSimStartPosition	= 0.0;
		m_dSimStopPosition	= 0.0;
		m_dSimPosition = 0.0;
		m_ulSimLastTime = 0;
		m_strFilename.Empty();
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::FindFilter()
//
// 	Description:	This function will locate the specified filter and retrieve
//					its class identifier
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::FindFilter(LPCSTR lpszName, CLSID* pClsId)
{
    IEnumMoniker*	pIEnumerator = 0;
    IMoniker*		pIMoniker = 0;
    IPropertyBag*	pIPropBag = 0;
	CLSID			Clsid;
    HRESULT			hResult = S_OK;
    ULONG			ulFetched=0;
    VARIANT			vName = {0};
    VARIANT			vClsid;
	CString			strName;
	BOOL			bSucceeded = FALSE;

	ASSERT(lpszName);
	ASSERT(lstrlen(lpszName) > 0);
	ASSERT(pClsId);

	//	Make sure we have the filter mapper
	ASSERT(m_pIMapper);
	if(m_pIMapper == 0) return FALSE;

	// Enumerate all filters that match the specified criteria
    hResult = m_pIMapper->EnumMatchingFilters(&pIEnumerator,
											  0,                // Reserved
											  FALSE,			// Use exact match?
											  MERIT_UNLIKELY,	// Minimum merit
											  TRUE,				// At least one input pin?
											  0,				// Number of major type/subtype pairs for input
											  NULL,				// Array of major type/subtype pairs for input
											  NULL,             // Input medium
											  NULL,				// Input pin category
											  FALSE,			// Must be a renderer?
											  FALSE,			// At least one output pin
											  0,				// Number of major type/subtype pairs for output
											  NULL,				// Array of major type/subtype pairs for output
											  NULL,				// Output medium
											  NULL);			// Output pin category
    if(FAILED(hResult))
        return FALSE;
    
    // Enumerate all filters 
    pIEnumerator->Reset();
    while((hResult = pIEnumerator->Next(1, &pIMoniker, &ulFetched)) == S_OK)
    {
        ASSERT(pIMoniker);

        // Associate the moniker with a file
        hResult = pIMoniker->BindToStorage(0, 0, IID_IPropertyBag, 
                                          (void **)&pIPropBag);
        ASSERT(SUCCEEDED(hResult));
        ASSERT(pIPropBag);
        if(FAILED(hResult))
            continue;

        // Read filter name from property bag
        vName.vt = VT_BSTR;
        hResult = pIPropBag->Read(L"FriendlyName", &vName, 0);
        if(FAILED(hResult))
            continue;

		//	Is this the desired filter?
        strName = vName.bstrVal;
        SysFreeString(vName.bstrVal);
		if(lstrcmpi(strName, lpszName) == 0)
			break;
        
        // Clean up interfaces
        RELEASE_INTERFACE(pIPropBag);
        RELEASE_INTERFACE(pIMoniker);
    }   

	//	Did we find the filter?
	if(pIPropBag != 0)
	{
        vClsid.vt = VT_BSTR;

        // Read CLSID string from property bag
        if(SUCCEEDED(pIPropBag->Read(L"CLSID", &vClsid, 0)))
        {
            // Get the class identifier
            if(CLSIDFromString(vClsid.bstrVal, &Clsid) == S_OK)
            {
				memcpy(pClsId, &Clsid, sizeof(CLSID));
				bSucceeded = TRUE;
              
            }
            SysFreeString(vClsid.bstrVal);
        }

	}


	//	Clean up
    RELEASE_INTERFACE(pIPropBag);
    RELEASE_INTERFACE(pIMoniker);
	RELEASE_INTERFACE(pIEnumerator);

	return bSucceeded;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetActFilters()
//
// 	Description:	This function will retrieve a list of the filters used to
//					construct the current filter graph
//
// 	Returns:		TRUE if successful
//
//	Notes:			The filter list is returned as a CR delimited string
//
//==============================================================================
BOOL CPlayer::GetActFilters(CString& rFilters, long* pCount, BOOL bVendorInfo)
{
	//	Get the filter list
	if(m_pIDXShow)
		return m_pIDXShow->GetFilters(rFilters, pCount, bVendorInfo);
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetDDBitmap()
//
// 	Description:	This function is called to retrieve a DDB equivalent of the
//					current rendering
//
// 	Returns:		A handle to a standard DDB if successful
//
//	Notes:			None
//
//==============================================================================
HBITMAP CPlayer::GetDDBitmap(int* pWidth, int* pHeight)
{
	if(m_pIDXShow)
		return m_pIDXShow->GetDDBitmap(pWidth, pHeight);
	else
		return NULL;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetDIBitmap()
//
// 	Description:	This function is called to retrieve a DIB equivalent of the
//					current rendering
//
// 	Returns:		A pointer to the memory buffer containing the DIB
//
//	Notes:			The caller is responsible for deallocation of the buffer
//
//==============================================================================
BITMAPINFOHEADER* CPlayer::GetDIBitmap(long* pSize)
{
	if(m_pIDXShow)
		return m_pIDXShow->GetDIBitmap(pSize);
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetDuration()
//
// 	Description:	This function is called to get the duration of the 
//					specified file without actually playing it.
//
// 	Returns:		The duration in seconds
//
//	Notes:			None
//
//==============================================================================
double CPlayer::GetDuration(LPCSTR lpszFilename)
{
	CIDXShow*	pIDXShow = 0;
	double		dDuration = -1.0;

	ASSERT(lpszFilename);

	//	Allocate an IDXShow object to do the work
	if((pIDXShow = GetIDXShow()) != 0)
	{
		dDuration = pIDXShow->GetDuration(lpszFilename);
		delete pIDXShow;
	}

	return dDuration;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetEvent()
//
// 	Description:	This function is called to retrieve an event from the
//					media event interface associated with the interface object
//					specified by the caller
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::GetEvent(long lId, long* pCode)
{
	//	Are we looking for an event from the current interface
	if(m_pIDXShow && (m_pIDXShow->m_lId == lId))
		return m_pIDXShow->GetEvent(pCode);
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetFilterMapper()
//
// 	Description:	This function will attach to the system's filter mapper
//					interface
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::GetFilterMapper()
{
	HRESULT	hResult;

	//	Should only be called once
	if(m_pIMapper != 0) return TRUE;

	// Create the filter mapper that will be used for all queries    
	hResult = CoCreateInstance(CLSID_FilterMapper2, 
							   NULL, CLSCTX_INPROC, IID_IFilterMapper2, 
							   (void **)&m_pIMapper);
	if (FAILED(hResult))
	{
		if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_NOFILTERMAPPER);
		return FALSE;
	}
	else
	{
		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetIDXShow()
//
// 	Description:	This function will allocate and initialize a new IDXShow
//					interface object
//
// 	Returns:		The pointer to the new CIDXShow object if successful
//
//	Notes:			None
//
//==============================================================================
CIDXShow* CPlayer::GetIDXShow()
{
	CIDXShow* pIDXShow = 0;

	//	Allocate a new interface
	pIDXShow = new CIDXShow(this, m_lIDXShowId++);
	ASSERT(pIDXShow);

	//	Initialize the new interface
	if(!pIDXShow->Initialize(m_pParent->m_hWnd, m_pErrors))
	{
		DELETE_INTERFACE(pIDXShow);
		return 0;
	}

	//	Set the playback filters
	pIDXShow->m_paFilters = m_aFilters;
	pIDXShow->m_iFilters  = m_iFilters;

	//	Make sure the video is sized properly
	pIDXShow->SetMaxRect(&m_rcMaxWnd);
	pIDXShow->SetScaleProps(m_bScale, m_bAspect);
	pIDXShow->SetDetachBeforeLoad(m_bDetachBeforeLoad);
	pIDXShow->SetVideoSliderHeight(m_iVideoSliderHeight);

	return pIDXShow;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetInterface()
//
// 	Description:	This function will retrieve a pointer to the requested COM
//					interface
//
// 	Returns:		A pointer to the interface associated with the current
//					IDXShow object
//
//	Notes:			None
//
//==============================================================================
LPVOID CPlayer::GetInterface(short sInterface)
{
	//	Get the requested interface
	if(m_pIDXShow)
		return m_pIDXShow->GetInterface(sInterface);
	else
		return NULL;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetPos()
//
// 	Description:	This function is called to retrieve the current position.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::GetPos(double* pPosition)
{
	double dSimElapsed = 0;

	ASSERT(pPosition);

	//	Are we simulating playback?
	if(m_bSimulating == TRUE)
	{
		//	Adjust the position if playing
		if(m_sSimState == TMMOVIE_PLAYING)
		{
			ASSERT(m_ulSimLastTime > 0);
			if(m_ulSimLastTime <= 0) m_ulSimLastTime = GetTickCount(); // Just in case...

			//	Get the elapsed time since the last position check
			if((dSimElapsed = (double)(GetTickCount() - m_ulSimLastTime)) < 0)
				dSimElapsed = 0;
			m_ulSimLastTime = GetTickCount();

			//	Adjust the position by the amount of elapsed time in seconds
			m_dSimPosition += (dSimElapsed / 1000.0);

			if(m_dSimPosition >= m_dSimStopPosition)
			{
				m_sSimState = TMMOVIE_PAUSED;
				m_dSimPosition = m_dSimStopPosition;
			}

		}

		*pPosition = m_dSimPosition;
		return TRUE;
	}
	else
	{
		if(m_pIDXShow)
			return m_pIDXShow->GetPos(pPosition);
		else
			return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetRegFilters()
//
// 	Description:	This function will retrieve a list of all registered 
//					multimedia filters
//
// 	Returns:		TRUE if successful
//
//	Notes:			The filter list is returned as a CR delimited string
//
//==============================================================================
BOOL CPlayer::GetRegFilters(CString& rFilters, long* pCount)
{
	IGraphBuilder*	pIGraphBuilder;
	IMediaControl*	pIMediaControl;
	IAMCollection*	pIAMCollection;
	IRegFilterInfo*	pIRegFilterInfo;
	HRESULT			hResult;
	long			lCount;
	char			szName[256];
	BSTR			bszName;

	//	Create an instance of the graph builder interface
	hResult = CoCreateInstance(CLSID_FilterGraph, NULL, 
							   CLSCTX_INPROC_SERVER, 
						       IID_IGraphBuilder, 
							   (void **)&pIGraphBuilder);
	if(FAILED(hResult))
		return FALSE;

	//	Get a media control interface
	hResult = pIGraphBuilder->QueryInterface(IID_IMediaControl, 
											(void **)&pIMediaControl);
	if(FAILED(hResult))
	{
		pIGraphBuilder->Release();
		return FALSE;
	}

	//	Get a collection interface
	hResult = pIMediaControl->get_RegFilterCollection((IDispatch**)&pIAMCollection);
	if(FAILED(hResult))
	{
		pIMediaControl->Release();
		pIGraphBuilder->Release();
		return FALSE;
	}

	//	Get the number of registered filters
	hResult = pIAMCollection->get_Count(&lCount);
	if(FAILED(hResult))
	{
		pIAMCollection->Release();
		pIMediaControl->Release();
		pIGraphBuilder->Release();
		return FALSE;
	}

	//	Clear the filter list
	rFilters.Empty();

	//	Iterate the filter list
	for(int i = 0; i < lCount; i++)
	{
		//	Get the filter information interface for the next filter
		pIRegFilterInfo = 0;
		pIAMCollection->Item(i, (IUnknown**)&pIRegFilterInfo);

		if(pIRegFilterInfo == 0)
			continue;

		//	Get the filter name
		pIRegFilterInfo->get_Name(&bszName);

		//	Convert the name to null terminated string
		WideCharToMultiByte(CP_ACP, 0, bszName, -1, szName, 
						    sizeof(szName), 0, 0);

		//	Add this filter to the list
		rFilters += szName;
		rFilters += "\r";

		//	Release the information interface
		pIRegFilterInfo->Release();
	}

	//	Release the interfaces
	pIAMCollection->Release();
	pIMediaControl->Release();
	pIGraphBuilder->Release();

	//	Save the filter count
	if(pCount)
		*pCount = lCount;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetRenderWnd()
//
// 	Description:	This function is called to retrieve the handle of the 
//					current rendering window.
//
// 	Returns:		The handle of the rendering window. NULL if not found.
//
//	Notes:			None
//
//==============================================================================
HWND CPlayer::GetRenderWnd()
{
	if(m_pIDXShow)
		return m_pIDXShow->GetRenderWnd();
	else
		return NULL;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetSnapshot()
//
// 	Description:	This function is called to get a snapshot of the current
//					image displayed by the player
//
// 	Returns:		A pointer to the snapshot window if successful
//
//	Notes:			None
//
//==============================================================================
CSnapshot* CPlayer::GetSnapshot(BOOL bPopup) 
{
	CSnapshot*			pSnapshot;
	RECT				rcSnapshot;
	RECT*				pVideo;
	int					iWidth;
	int					iHeight;
	HBITMAP				hDDB;

	//	No snapshots while simulating playback
	if(m_bSimulating == TRUE) return NULL;

	//	Has a file been loaded ?
	if(!IsLoaded())
	{
		if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_NOTLOADED);
		return 0;
	}

	//	Allocate a new snapshot
	pSnapshot = new CSnapshot();
	ASSERT(pSnapshot);
	
	//	Create the snapshot window
	if(!pSnapshot->Create(m_pParent, bPopup))
	{
		if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_CREATESSFAILED);
		delete pSnapshot;
		return 0;
	}

	//	Get a device dependent bitmap
	if((hDDB = GetDDBitmap(&iWidth, &iHeight)) == 0)
	{
		if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_SSBITMAPFAILED);
		pSnapshot->SetDDBitmap(0, 0, 0);
	}
	else
	{
		//	Set the snapshot image
		pSnapshot->SetDDBitmap(hDDB, iWidth, iHeight);
	}

	//	Get the video window rectangle
	if((pVideo = GetVideoRect()) == 0)
	{
		m_pParent->GetClientRect(&rcSnapshot);
	}
	else
	{
		//	Make sure the snapshot window is properly sized and aligned
		rcSnapshot.top    = pVideo->top;
		rcSnapshot.left   = pVideo->left;
		rcSnapshot.bottom = rcSnapshot.top + pVideo->bottom;
		rcSnapshot.right  = rcSnapshot.left + pVideo->right;
	}

	//	Move the window into position
	pSnapshot->MoveWindow(&rcSnapshot);

	return pSnapshot;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetStartPos()
//
// 	Description:	This function is called to retrieve the start position.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::GetStartPos(double* pPosition)
{
	ASSERT(pPosition);

	if(m_bSimulating == TRUE)
	{
		*pPosition = m_dSimStartPosition;
		return TRUE;
	}
	else
	{
		if(m_pIDXShow)
			return m_pIDXShow->GetStartPos(pPosition);
		else
			return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::GetState()
//
// 	Description:	This function is called to retrieve the current player
//					state.
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function will translate DirectShow state identifiers
//					to TMMOVIE identifiers
//
//==============================================================================
BOOL CPlayer::GetState(short* pState)
{
	long lState;

	ASSERT(pState);

	if(m_bSimulating == TRUE)
	{
		*pState = m_sSimState;
		return TRUE;
	}
	else
	{
		//	Do we have an active interface?
		if(m_pIDXShow == 0 || !m_pIDXShow->GetState(&lState))
			return FALSE;

		//	Translate to TMMOVIE identifier
		switch(lState)
		{
			case State_Stopped:		*pState = TMMOVIE_STOPPED;
									break;

			case State_Paused:		*pState = TMMOVIE_PAUSED;
									break;

			case State_Running:		*pState = TMMOVIE_PLAYING;
									break;

			default:				return FALSE;
		}
		return TRUE;
	
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::GetStopPos()
//
// 	Description:	This function is called to retrieve the start position.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::GetStopPos(double* pPosition)
{
	ASSERT(pPosition);

	if(m_bSimulating == TRUE)
	{
		*pPosition = m_dSimStopPosition;
		return TRUE;
	}
	else
	{
		if(m_pIDXShow)
			return m_pIDXShow->GetStopPos(pPosition);
		else
			return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::GetUserFilters()
//
// 	Description:	This function will retrieve a list of all filters defined by
//					the user.
//
// 	Returns:		TRUE if successful
//
//	Notes:			The filter list is returned as a CR delimited string
//
//==============================================================================
BOOL CPlayer::GetUserFilters(CString& rFilters, long* pCount)
{
	//	Set the filter count
	if(pCount) *pCount = m_iFilters;

	//	Clear the filter list
	rFilters.Empty();

	//	Iterate the filter list
	for(int i = 0; i < IDXSHOW_MAX_FILTERS; i++)
	{
		if(m_aFilters[i].strName.GetLength() > 0)
		{
			rFilters += m_aFilters[i].strName;
			rFilters += "\r";
		}
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::GetVideoRect()
//
// 	Description:	This function is called to retrieve the current video
//					rectangle
//
// 	Returns:		A pointer to the current video rectangle. NULL otherwise.
//
//	Notes:			None
//
//==============================================================================
RECT* CPlayer::GetVideoRect()
{
	if(m_pIDXShow)
		return m_pIDXShow->GetVideoRect();
	else
		return NULL;
}

//==============================================================================
//
// 	Function Name:	CPlayer::HideTaskBar()
//
// 	Description:	This function is called to hide the task bar.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::HideTaskBar()
{
	//	Don't bother if we are not controlling the task bar
	if(m_bHideTaskBar)
	{
		CTMToolbox::SetTaskBarVisible(FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CPlayer::Initialize()
//
// 	Description:	This function will initialize the video player.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Initialize(CTMMovieCtrl* pParent, CErrorHandler* pErrors)
{
	HRESULT			hResult;
	IGraphBuilder*	pIGraphBuilder;

	ASSERT(pParent);
	ASSERT(pErrors);

	//	Save the pointer to the parent window and error handler
	m_pParent = pParent;
	m_pErrors = pErrors;

	//	Make sure we can get the graph builder interface
	hResult = CoCreateInstance(CLSID_FilterGraph, NULL, 
							   CLSCTX_INPROC_SERVER, 
							   IID_IGraphBuilder, 
								(void **)&pIGraphBuilder);

	//	Did the request fail
	if(!SUCCEEDED(hResult))
		return FALSE;

	if(pIGraphBuilder)
		pIGraphBuilder->Release();

	//	Get the system's filter mapper
	GetFilterMapper();

	//	Make sure the player is properly sized
	Resize();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::IsAudio()
//
// 	Description:	This function is called to see if the active file is audio
//					only
//
// 	Returns:		TRUE if audio only
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::IsAudio()
{
	if(m_pIDXShow)
		return m_pIDXShow->IsAudio();
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::IsLoaded()
//
// 	Description:	This function is called to see if a file is loaded and ready
//					for playback
//
// 	Returns:		TRUE if loaded
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::IsLoaded()
{
	if(m_bSimulating == TRUE)
		return TRUE;
	else
		return (m_pIDXShow != 0);
}

//==============================================================================
//
// 	Function Name:	CPlayer::IsVisible()
//
// 	Description:	This function is called to see if the active interface is
//					visible
//
// 	Returns:		TRUE if visible
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::IsVisible()
{
	if(m_pIDXShow)
		return m_pIDXShow->IsVisible();
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::Load()
//
// 	Description:	This function will load the file requested by the caller
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Load(LPCSTR lpFilename)
{
	CSnapshot* pSnapshot;

	ASSERT(lpFilename);
	ASSERT(m_pParent);

	//	End the current simulation
	EndSimulation();

	//	Is this already the active file?
	if(m_strFilename.CompareNoCase(lpFilename) == 0)
		return TRUE;

	//	Are we supposed to open a snapshot window?
	if(m_bSnapshots && m_pIDXShow && m_pIDXShow->IsVisible())
	{
		//	Pause the current playback
		Pause();

		//	Get a new snapshot
		if((pSnapshot = GetSnapshot()) != 0)
		{
			//	Show the new snapshot
			pSnapshot->ShowWindow(SW_SHOW);
			pSnapshot->RedrawWindow();
			pSnapshot->BringWindowToTop();

			//	Delete the existing snapshot
			if(m_pSnapshot)
				delete m_pSnapshot;

			//	Save the pointer to the new snapshot
			m_pSnapshot = pSnapshot;
		}
	}	

	//	Do we already have an active interface?
	if(!m_pIDXShow)
	{
		if((m_pIDXShow = GetIDXShow()) == 0)
			return FALSE;
	}

	//	Hide the system's task bar
	HideTaskBar();

	//	Load the file
	if(!m_pIDXShow->Render(lpFilename))
	{
		//ShowTaskBar();
		return FALSE;
	}
	
	//	Set the playback properties
	m_pIDXShow->SetRate(m_sRate);
	m_pIDXShow->SetVolume(m_sVolume);
	m_pIDXShow->SetBalance(m_sBalance);
	m_pIDXShow->SetVideoPos();

	//	Get the video properties
	m_dMinTime		= 0;
	m_dMaxTime		= m_pIDXShow->m_dDuration;
	m_lWidth		= m_pIDXShow->m_lWidth;
	m_lHeight		= m_pIDXShow->m_lHeight;
	m_fAspectRatio	= m_pIDXShow->m_fAspectRatio;
	m_fFrameRate	= m_pIDXShow->m_fFrameRate;
	m_strFilename   = lpFilename;
	
	//ShowTaskBar();
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::OnIDXSetPos()
//
// 	Description:	This function is called by the active CIDXShow object when
//					the video position is set
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::OnIDXSetPos()
{
	//	Notify the parent control
	if(m_pParent)
		m_pParent->OnPlayerSetPos();	
}

//==============================================================================
//
// 	Function Name:	CPlayer::OnIDXSetWnd()
//
// 	Description:	This function is called by the active CIDXShow object when
//					the video rendering window is set
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::OnIDXSetWnd()
{
	//	Notify the parent control
	if(m_pParent)
		m_pParent->OnPlayerSetWnd();	
}

//==============================================================================
//
// 	Function Name:	CPlayer::OnIDXShow()
//
// 	Description:	This function is called by the active CIDXShow object when
//					the video is shown or hidden
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::OnIDXShow(BOOL bShow)
{
	//	Notify the parent control
	if(m_pParent)
		m_pParent->OnPlayerShow(bShow);	
}

//==============================================================================
//
// 	Function Name:	CPlayer::Pause()
//
// 	Description:	This function will pause the current file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Pause()
{
	if(m_bSimulating == TRUE)
	{
		//	Get the current position
		GetPos(&m_dSimPosition);

		//	Stop simulated updates
		m_sSimState = TMMOVIE_PAUSED;
		m_ulSimLastTime = 0;

		return TRUE;
	}
	else
	{
		//	Pause the playback
		if(m_pIDXShow)
			return m_pIDXShow->Pause();
		else
			return TRUE;
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::Play()
//
// 	Description:	This function will play the current file from the specified
//					start frame to the specified stop frame
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Play(double dFrom, double dTo)
{
	if(m_bSimulating == TRUE)
	{
		//	Is the specified range valid?
		if(dFrom > dTo || dFrom < m_dMinTime || dTo > m_dMaxTime)
		{
			m_pParent->AddDebugMessage("CPlayer::Play() -> invalid range -> from = %f  to = %f", dFrom, dTo);

			return FALSE;
		}

		//	Set the playback range
		SetRange(dFrom, dTo);

		//	Start simulating playback
		m_ulSimLastTime = GetTickCount();
		m_sSimState = TMMOVIE_PLAYING;

		return TRUE;
	}
	else
	{
		//	Has a file been loaded ?
		if(!IsLoaded())
		{
			if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_NOTLOADED);
			return FALSE;
		}
		ASSERT(m_pIDXShow);

		//	Are the frame specifications valid?
		if(dFrom > dTo || dFrom < m_dMinTime || dTo > m_dMaxTime)
			return FALSE;

		//	Delete the existing snapshot
		DELETE_SNAPSHOT(m_pSnapshot);

		//	Play the file
		return m_pIDXShow->Play(dFrom, dTo);
	
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::Redraw()
//
// 	Description:	This function will redraw the video
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::Redraw()
{
	if(m_pIDXShow) 
		m_pIDXShow->Redraw();
}

//==============================================================================
//
// 	Function Name:	CPlayer::RemoveFilter()
//
// 	Description:	This function is called to remove a filter from the list of
//					filters added to the filter graph before rendering.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::RemoveFilter(LPCSTR lpszName)
{
	ASSERT(lpszName);
	ASSERT(lstrlen(lpszName) > 0);

	//	Do we have any filters in the array
	if(m_iFilters > 0)
	{
		//	Find the requested filter
		for(int i = 0; i < IDXSHOW_MAX_FILTERS; i++)
		{
			if((m_aFilters[i].strName).CompareNoCase(lpszName) == 0)
			{
				m_aFilters[i].strName.Empty();
				memset(&(m_aFilters[i].ClsId), 0, sizeof(CLSID));

				m_iFilters--;

				//	Notify the interface if it exists
				if(m_pIDXShow != 0)
					m_pIDXShow->m_iFilters = m_iFilters;

				return TRUE;
			}
		}

	}

	//	Must not have been able to find the filter
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::Resize()
//
// 	Description:	This function will resize the video to fit within the
//					parent's client area.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::Resize()
{
	//	Don't bother if we haven't been initialized yet
	if(!m_pParent)
		return;

	//	Get the dimensions of the parent's client rectangle
	m_pParent->GetClientRect(&m_rcMaxWnd);

	//	Resize the interfaces
	if(m_pIDXShow) 
		m_pIDXShow->SetMaxRect(&m_rcMaxWnd);
}

//==============================================================================
//
// 	Function Name:	CPlayer::SetBalance()
//
// 	Description:	This function will set the playback audio balance
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::SetBalance(short sBalance)
{
	//	Save the new balance
	m_sBalance = sBalance;

	//	Has a file been loaded ?
	if(!IsLoaded() || m_bSimulating)
		return m_bAdjBalance;

	if(m_pIDXShow != 0)
		m_bAdjBalance = m_pIDXShow->SetBalance(m_sBalance);

	return m_bAdjBalance;
}

//==============================================================================
//
// 	Function Name:	CPlayer::SetDetachBeforeLoad()
//
// 	Description:	This function will set the detach before load option of the
//					renderer
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::SetDetachBeforeLoad(BOOL bDetach)
{
	m_bDetachBeforeLoad = bDetach;
	if(m_pIDXShow)
		m_pIDXShow->SetDetachBeforeLoad(bDetach);
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::SetHideTaskBar()
//
// 	Description:	This function will set the detach before load option of the
//					renderer
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::SetHideTaskBar(BOOL bHide)
{
	m_bHideTaskBar = bHide;
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::SetPos()
//
// 	Description:	This function will set the current position
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::SetPos(double dPosition)
{
	//	Has a file been loaded ?
	if(!IsLoaded())
	{
		if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_NOTLOADED);
		return FALSE;
	}

	//	Make sure the position is within range
	if(dPosition < m_dMinTime)
		dPosition = m_dMinTime;
	else if(dPosition > m_dMaxTime)
		dPosition = m_dMaxTime;

	//	Are we simulating playback?
	if(m_bSimulating == TRUE)
	{
		//	Pause the playback
		Pause();

		//	Make this the current position
		m_dSimPosition = dPosition;

		//	Notify the parent control
		if(m_pParent)
			m_pParent->OnPlayerSetPos();	
		
		return TRUE;
	}
	else
	{
		//	Delete the existing snapshot
		DELETE_SNAPSHOT(m_pSnapshot);

		//	Do we have a valid interface? (WE SHOULD)
		ASSERT(m_pIDXShow);
		if(!m_pIDXShow)
			return FALSE;
 
		//	Set the current position
		if(!m_pIDXShow->SetPos(dPosition, FALSE))
			return FALSE;

		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CPlayer::SetRange()
//
// 	Description:	This function will set the start and stop positions of the
//					playback
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::SetRange(double dStart, double dStop)
{
	if(m_bSimulating == TRUE)
	{
		//	Pause the simulated playback
		Pause();

		//	Make sure the start frame is within range
		if((dStart < m_dMinTime) || (dStart >= m_dMaxTime))
			dStart = m_dMinTime;

		//	Make sure the stop frame is within range
		if((dStop <= m_dMinTime) || (dStop > m_dMaxTime))
			dStop = m_dMaxTime;

		//	Update the simulated positions
		m_dSimStartPosition = dStart;
		m_dSimStopPosition = dStop;
		m_dSimPosition = dStart;

		return TRUE;
	}
	else
	{
		//	Has a file been loaded ?
		if(!IsLoaded())
		{
			if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_NOTLOADED);
			return FALSE;
		}
		ASSERT(m_pIDXShow);

		//	Make sure the start frame is within range
		if((dStart < m_dMinTime) || (dStart >= m_dMaxTime))
		{
			m_pParent->AddDebugMessage("CPlayer::SetRange() -> Resetting start to minimum - Start = %f  Minimum = %f  Maximum = %f", dStart, m_dMinTime, m_dMaxTime);
			dStart = m_dMinTime;
		}

		//	Make sure the stop frame is within range
		if((dStop <= m_dMinTime) || (dStop > m_dMaxTime))
		{
			m_pParent->AddDebugMessage("CPlayer::SetRange() -> Resetting stop to maximum - Stop = %f  Minimum = %f  Maximum = %f", dStop, m_dMinTime, m_dMaxTime);
			dStop = m_dMaxTime;
		}

		return m_pIDXShow->SetRange(dStart, dStop, FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CPlayer::SetRate()
//
// 	Description:	This function will set the playback rate
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::SetRate(short sRate)
{
	//	Save the new rate
	m_sRate = sRate;

	//	Has a file been loaded ?
	if(!IsLoaded() || m_bSimulating)
		return m_bAdjRate;

	m_bAdjRate = m_pIDXShow->SetRate(m_sRate);
	
	return m_bAdjRate;
}

//==============================================================================
//
// 	Function Name:	CPlayer::SetScaleProps()
//
// 	Description:	This function will set the scaling properties for video
//					rendering
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::SetScaleProps(BOOL bScale, BOOL bAspect)
{
	//	Save the new properties
	m_bScale  = bScale;
	m_bAspect = bAspect;

	//	Has a file been loaded ?
	if(!IsLoaded() || m_bSimulating)
		return TRUE;

	return m_pIDXShow->SetScaleProps(bScale, bAspect);
}

//==============================================================================
//
// 	Function Name:	CPlayer::SetVolume()
//
// 	Description:	This function will set the playback volume
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::SetVolume(short sVolume)
{
	//	Save the new rate
	m_sVolume = sVolume;

	//	Has a file been loaded ?
	if(!IsLoaded() || m_bSimulating)
		return m_bAdjVolume;

	m_bAdjVolume = m_pIDXShow->SetVolume(m_sVolume);

	return m_bAdjVolume;
}

//==============================================================================
//
// 	Function Name:	CPlayer::Show()
//
// 	Description:	This function will set the visibility of the current
//					playback window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::Show(BOOL bShow, BOOL bRefresh)
{
	if(m_pIDXShow)
		m_pIDXShow->Show(bShow, bRefresh);

	//	Delete the existing snapshot
	DELETE_SNAPSHOT(m_pSnapshot);
}

//==============================================================================
//
// 	Function Name:	CPlayer::ShowTaskBar()
//
// 	Description:	This function is called to make the task bar visible.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlayer::ShowTaskBar()
{
	//	Don't bother if we are not controlling the task bar
	if(m_bHideTaskBar)
	{
		CTMToolbox::SetTaskBarVisible(TRUE);
	}
}

//==============================================================================
//
// 	Function Name:	CPlayer::Simulate()
//
// 	Description:	This function will simulate playback of the specified file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Simulate(LPCSTR lpFilename)
{
	ASSERT(lpFilename);

	//	Pause the current playback
	Pause();

	//	Unload the active video
	Unload();
	
	//	Get the video properties
	m_dMinTime		= 0;
	m_dMaxTime		= 3600000.0; // 1000 hrs - sounds large enough to me
	m_lWidth		= 320;
	m_lHeight		= 240;
	m_fAspectRatio	= (float)m_lWidth / (float)m_lHeight;
	m_fFrameRate	= m_fDefaultRate;
	m_strFilename   = lpFilename;
	m_sSimState		= TMMOVIE_READY;
	m_bSimulating	= TRUE;
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::Step()
//
// 	Description:	This function will play the video between the specified
//					frames
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Step(double dFrom, double dTo)
{
	//	Has a file been loaded ?
	if(!IsLoaded())
	{
		if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_NOTLOADED);
		return FALSE;
	}

	//	Are we simulating playback?
	if(m_bSimulating == TRUE)
	{
		return Play(dFrom, dTo);
	}
	else
	{
		//	Do we have a valid interface? (WE SHOULD)
		ASSERT(m_pIDXShow);
		if(!m_pIDXShow)
			return FALSE;
 
		//	Make sure the position is within range
		if(dFrom < m_dMinTime)
			dFrom = m_dMinTime;
		if(dTo > m_dMaxTime)
			dTo = m_dMaxTime;

		//	Delete the existing snapshot
		DELETE_SNAPSHOT(m_pSnapshot);

		//	Step the video
		return m_pIDXShow->Step(dFrom, dTo);
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::Stop()
//
// 	Description:	This function will stop the current playback
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Stop()
{
	if(m_bSimulating == TRUE)
	{
		//	Update the position
		GetPos(&m_dSimPosition);

		//	Stop simulated updates
		m_sSimState = TMMOVIE_STOPPED;
		m_ulSimLastTime = 0;

		return TRUE;
	}
	else
	{
		//	Stop the playback
		if(m_pIDXShow)
			return m_pIDXShow->Stop();
		else
			return TRUE;
	}

}

//==============================================================================
//
// 	Function Name:	CPlayer::Unload
//
// 	Description:	This function will unload all rendered files
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Unload()
{
	//	Do we have an active interface?
	if(m_pIDXShow)
	{
		//	Stop the current playback
		Stop();

		//	Make sure the rendering window is closed
		Show(FALSE, FALSE);

		//	Make sure the interfaces are shut down
		DELETE_INTERFACE(m_pIDXShow);
	}

	//	Clear the simulation values
	EndSimulation();

	//	Reset the video properties
	m_dMinTime		= 0;
	m_dMaxTime		= 0;
	m_lWidth		= 640;
	m_lHeight		= 480;
	m_fAspectRatio	= (float)m_lHeight / (float)m_lWidth;
	m_fFrameRate	= m_fDefaultRate;
	m_strFilename.Empty();
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPlayer::Update()
//
// 	Description:	This function is called to update the rendering window
//
// 	Returns:		TRUE if visible
//
//	Notes:			None
//
//==============================================================================
BOOL CPlayer::Update()
{
	//	Don't bother if not visible
	if(!IsVisible())
		return FALSE;

	if(m_pIDXShow)
		return m_pIDXShow->UpdatePos(FALSE);
	else
		return FALSE;
}

