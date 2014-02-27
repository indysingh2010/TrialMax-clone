//==============================================================================
//
// File Name:	idxshow.cpp
//
// Description:	This file contains member functions of the CIDXShow class.
//
// Functions:   CIDXShow::CalcVideoRect()
//				CIDXShow::CIDXShow()
//				CIDXShow::~CIDXShow()
//				CIDXShow::GetDDBitmap()
//				CIDXShow::GetDIBitmap()
//				CIDXShow::GetEvent()
//				CIDXShow::GetFilters()
//				CIDXShow::GetInterface()
//				CIDXShow::GetInterfaces()
//				CIDXShow::GetPos()
//				CIDXShow::GetRenderWnd()
//				CIDXShow::GetStartPos()
//				CIDXShow::GetState()
//				CIDXShow::GetStopPos()
//				CIDXShow::GetVideoProps()
//				CIDXShow::GetVideoRect()
//				CIDXShow::HandleError()
//				CIDXShow::Initialize()
//				CIDXShow::IsVisible()
//				CIDXShow::Pause()
//				CIDXShow::Play()
//				CIDXShow::Redraw()
//				CIDXShow::ReleaseAll()
//				CIDXShow::Render()
//				CIDXShow::Run()
//				CIDXShow::SetBalance()
//				CIDXShow::SetMaxRect()
//				CIDXShow::SetPos()
//				CIDXShow::SetRange()
//				CIDXShow::SetRate()
//				CIDXShow::SetScaleProps()
//				CIDXShow::SetVideoPos()
//				CIDXShow::SetVolume()
//				CIDXShow::Show()
//				CIDXShow::ShowState()
//				CIDXShow::Step()
//				CIDXShow::Stop()
//				CIDXShow::UpdatePos()
//
// See Also:	idxshow.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-26-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <idxshow.h>
#include <math.h>
#include <tmmovie.h>
#include <tmmvdefs.h>
#include <dshow.h>

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
extern CTMMovieCtrl*	pControl;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CIDXShow::CalcVideoRect()
//
// 	Description:	This function will calculate the values used to define the
//					video rectangle
//
// 	Returns:		None
//
//	Notes:			We store the width and height in the video rectangle rather
//					than the bottom right coordinates
//
//==============================================================================
void CIDXShow::CalcVideoRect()
{
	float	fMaxWidth;
	float	fMaxHeight;
	float	fCx;
	float	fCy;
	float	fTop;
	float	fLeft;
	float	fWidth;
	float	fHeight;

	//	Calculate the dimensions and center point of the available viewport
	fMaxWidth  = (float)(m_rcMaxWnd.right - m_rcMaxWnd.left);
	fMaxHeight = (float)(m_rcMaxWnd.bottom - m_rcMaxWnd.top);
	fCx = (float)m_rcMaxWnd.left + (fMaxWidth / 2.0f);
	fCy = (float)m_rcMaxWnd.top + (fMaxHeight / 2.0f);

	//	Are we scaling the video?
	if(m_bScale)
	{
		//	Just in case !!!! (prevent divide by zero)
		ASSERT(m_fAspectRatio != 0);
		if(m_fAspectRatio == 0)
			m_fAspectRatio = 1.333333f;

		//	Are we maintaining the aspect ratio?
		if(m_bAspect)
		{
			//	Can we use the full available height?
			if((fMaxHeight * m_fAspectRatio) <= fMaxWidth)
			{
				fHeight = fMaxHeight;
				fWidth  = fHeight * m_fAspectRatio;

				fTop = 0;
				fLeft = fCx - (fWidth / 2.0f);
			}
			else
			{
				fWidth  = fMaxWidth;
				fHeight = fWidth / m_fAspectRatio;

				fLeft = 0;
				fTop = fCy - (fHeight / 2.0f);
			}

		}
		else
		{
			//	Resize to match the viewport
			fLeft = 0;
			fTop  = 0;
			fWidth = fMaxWidth;
			fHeight = fMaxHeight;
		}

	}
	else
	{
		//	We are not resizing the video
		fWidth = (float)m_lWidth;
		fHeight = (float)m_lHeight;

		//	Center the video
		fLeft = fCx - (fWidth / 2.0f);
		if(fLeft < 0)
			fLeft = 0;

		fTop = fCy - (fHeight / 2.0f);
		if(fTop < 0)
			fTop = 0;

	}

	//	Store the new position
	m_rcVideoPos.left   = (int)fLeft;
	m_rcVideoPos.top    = (int)fTop;
	m_rcVideoPos.right  = (int)fWidth;
	m_rcVideoPos.bottom = (int)fHeight;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::CanSeekFrames()
//
// 	Description:	This function is called to determine if the current 
//					interface supports frame based positioning
//
// 	Returns:		TRUE if capable of frame based positioning
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::CanSeekFrames()
{
	GUID Format = TIME_FORMAT_FRAME;

	//	Set the interface mode
	if((m_pIMediaSeeking != NULL) && (m_pIMediaSeeking->IsFormatSupported(&Format) == S_OK))
	{
		return TRUE;
	}
	else
	{
		return FALSE;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::CIDXShow
//
// 	Description:	This is the constructor for CIDXShow objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CIDXShow::CIDXShow(CPlayer* pPlayer, long lId)
{
	ASSERT(pPlayer);

	m_pIGraphBuilder		= 0;
	m_pIMediaSeeking		= 0;
	m_pIMediaPosition		= 0;
	m_pIMediaControl		= 0;
	m_pIMediaEventEx		= 0;
	m_pIVideoWindow			= 0;
	m_pIBasicVideo			= 0;
	m_pIBasicAudio			= 0;
	m_pPlayer				= pPlayer;
	m_paFilters				= 0;
	m_bScale				= TRUE;
	m_bAspect				= TRUE;
	m_bDetachBeforeLoad		= FALSE;
	m_hParent				= 0;
	m_hRenderer				= 0;
	m_pErrors				= 0;
	m_hResult				= 0;
	m_lId					= lId;
	m_lHeight				= 480;
	m_lWidth				= 640;
	m_iSeekMode				= TMMOVIE_SEEK_FRAMES;
	m_dDuration				= (double)0;
	m_iFilters				= 0;
	m_fAspectRatio			= (float)m_lWidth / (float)m_lHeight;
	m_fFrameRate			= IDXSHOW_FRAMERATE;
	memset(&m_rcMaxWnd, 0, sizeof(m_rcMaxWnd));
	memset(&m_rcVideoPos, 0, sizeof(m_rcVideoPos));
}

//==============================================================================
//
// 	Function Name:	CIDXShow::~CIDXShow
//
// 	Description:	This is the destructor for CIDXShow objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CIDXShow::~CIDXShow()
{
	//	Release the video window interface. We handle the video window interface
	//	differently from all others. See: ResetWindow()
	if(m_pIVideoWindow)
	{
		m_pIVideoWindow->put_Visible(OAFALSE);
		m_pIVideoWindow->put_Owner(NULL);
		RELEASE_INTERFACE(m_pIVideoWindow);
	}

	//	Make sure all the interfaces have been released
	ReleaseAll();
}

//==============================================================================
//
// 	Function Name:	CIDXShow::ConvertTime()
//
// 	Description:	This function is called to convert the double precision
//					seconds to units appropriate for the current seek mode.
//
// 	Returns:		The equivalent value in seek mode units
//
//	Notes:			None
//
//==============================================================================
LONGLONG CIDXShow::ConvertTime(double dSeconds)
{
	LONGLONG llPosition = (LONGLONG)0;

	//	What is the current seek mode?
	switch(m_iSeekMode)
	{
		case TMMOVIE_SEEK_FRAMES:

			ASSERT(m_fFrameRate > 0);
			llPosition = ((LONGLONG)((double)m_fFrameRate * dSeconds));
			break;

		case TMMOVIE_SEEK_TIME:

			//	Convert from seconds to 100 nanosecond units
			llPosition = ((LONGLONG)(dSeconds * (double)10000000));
			break;

		default:

			break;
	}

	return llPosition;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::ConvertPosition()
//
// 	Description:	This function is called to convert the specified position 
//					based on the current seek mode to double precision seconds
//
// 	Returns:		The equivalent value in seconds
//
//	Notes:			None
//
//==============================================================================
double CIDXShow::ConvertPosition(LONGLONG llPosition)
{
	double dReturn = (double)0;
	double dPosition = (double)llPosition;

	//	What is the current seek mode?
	switch(m_iSeekMode)
	{
		case TMMOVIE_SEEK_FRAMES:

			ASSERT(m_fFrameRate > 0);

			if(m_fFrameRate > 0)
			{
				dReturn = (dPosition / (double)m_fFrameRate);
			}
			break;

		case TMMOVIE_SEEK_TIME:

			//	Convert from 100 nanosecond units to seconds
			dReturn = (dPosition / (double)10000000);
			break;

		default:

			break;
	}

	return dReturn;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::ConvertTime()
//
// 	Description:	This method is called to convert the seconds to units based
//					on the current video's seek mode
//
// 	Returns:		The seek mode equivalent
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::ConvertToFrames(double dSeconds, long* pConverted) 
{
	GUID	Format;
	double	dFrames;

	ASSERT(pConverted != NULL);
	if(pConverted == NULL) return FALSE;

	if((m_pIMediaSeeking != 0) && (m_pIMediaSeeking->GetTimeFormat(&Format) == S_OK))
	{
		if(Format == TIME_FORMAT_FRAME)
		{
			dFrames = (dSeconds * (double)m_fFrameRate);

			//	Round the equivalent to the nearest frame
			if(dFrames < 0)
				*pConverted = ((long)(dFrames - 0.5));
			else
				*pConverted = ((long)(dFrames + 0.5));

			return TRUE;
		}

	}

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::ConvertToTime()
//
// 	Description:	This method is called to convert the specified frames to
//					double precision seconds
//
// 	Returns:		The equivalent value in seconds
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::ConvertToTime(long lFrame, double* pConverted) 
{
	GUID	Format;

	ASSERT(pConverted != NULL);
	if(pConverted == NULL) return FALSE;

	if((m_pIMediaSeeking != 0) && (m_pIMediaSeeking->GetTimeFormat(&Format) == S_OK))
	{
		if((Format == TIME_FORMAT_FRAME) && (m_fFrameRate > 0))
		{
			*pConverted = (double)lFrame / (double)m_fFrameRate;

			return TRUE;
		}

	}

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetDDBitmap()
//
// 	Description:	This function is called to get a DDB equivalent of the 
//					rendering window contents
//
// 	Returns:		A handle to the bitmap if successful 
//
//	Notes:			None
//
//==============================================================================
HBITMAP CIDXShow::GetDDBitmap(int* pWidth, int* pHeight)
{
	HDC		dcVideo;
	HDC		dcMem;
	HBITMAP	hBitmap;
	HBITMAP	hOldBitmap;
	RECT	rcVideo;
	int		iWidth;
	int		iHeight;

	//	Is the rendering window available and visible?
	if(m_hRenderer == 0 || !IsWindow(m_hRenderer))
		return NULL;	

	//	Get the client area of the rendering window
	GetClientRect(m_hRenderer, &rcVideo);

	//	Make sure we're not dealing with an empty rectangle
	if(IsRectEmpty(&rcVideo))
		return NULL;

	//	Create the device contexts we need to create the bitmap
	if((dcVideo = GetDC(m_hRenderer)) == NULL)
		return NULL;
	if((dcMem = CreateCompatibleDC(dcVideo)) == NULL)
	{
		DeleteDC(dcVideo);
		return NULL;
	}

	//	Calculate the width and height of the capture area
	iWidth  = rcVideo.right - rcVideo.left;
	iHeight = rcVideo.bottom - rcVideo.top;

	//	Create a bitmap compatible with the screen dc
	if((hBitmap = CreateCompatibleBitmap(dcVideo, iWidth, iHeight)) == NULL)
	{
		DeleteDC(dcVideo);
		DeleteDC(dcMem);
		return NULL;
	}

	//	Select the new bitmap into the memory dc
	hOldBitmap = (HBITMAP)SelectObject(dcMem, hBitmap);

	//	BitBlt the video area to the memory dc
	BitBlt(dcMem, 0, 0, iWidth, iHeight, dcVideo, 0,
		   0, SRCCOPY);

	//	Select the old bitmap back into the memory dc and retain the new bitmap
	hBitmap = (HBITMAP)SelectObject(dcMem, hOldBitmap);
	if(pWidth)	*pWidth  = iWidth;
	if(pHeight) *pHeight = iHeight;

	//	Clean up
	DeleteDC(dcVideo);
	DeleteDC(dcMem);

	return hBitmap;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetDIBitmap()
//
// 	Description:	This function is called to get a DIB equivalent of the data
//					present at the rendering filter
//
// 	Returns:		A pointer to the buffer containing the bitmap 
//
//	Notes:			The caller is responsible for deallocation of the image
//					buffer
//
//==============================================================================
BITMAPINFOHEADER* CIDXShow::GetDIBitmap(long* pSize)
{
	long			lSize;
	LPVOID			lpBitmap;
	
	//	Do we have the basic video interface?
	if(m_pIBasicVideo == 0)
		return 0;

	//	The playback has to be paused to get the bitmap
	Pause();

	//	How much memory are we going to need for the bitmap?
	m_hResult = m_pIBasicVideo->GetCurrentImage(&lSize, 0);
	if(!SUCCEEDED(m_hResult))
		return 0;

	//	Allocate the memory required for the image
	if((lpBitmap = HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, lSize)) == 0)
		return 0;

	//	Now get the image as a DIB
	m_hResult = m_pIBasicVideo->GetCurrentImage(&lSize, (long*)lpBitmap);
	if(!SUCCEEDED(m_hResult))
		return 0;

	//	Set the buffer size for the caller
	if(pSize) *pSize = lSize;

	return (BITMAPINFOHEADER*)lpBitmap;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetDuration()
//
// 	Description:	This function is called to get the duration of the specified
//					file.
//
// 	Returns:		The duration in seconds if successful, -1.0 on failure
//
//	Notes:			None
//
//==============================================================================
double CIDXShow::GetDuration(LPCSTR lpszFilename)
{
	WCHAR			wFile[MAX_PATH];
	IGraphBuilder*	pIGraphBuilder = 0;
	IMediaSeeking*	pIMediaSeeking = 0;
	double			dDuration = -1.0;
	GUID			Format = TIME_FORMAT_MEDIA_TIME;
	LONGLONG		llDuration;
	bool			bSuccessful = false;

	ASSERT(lpszFilename);

    //	Convert to wide character format
	MultiByteToWideChar(CP_ACP, 0, lpszFilename, -1, wFile, sizeof(wFile));

	while(bSuccessful == false)
	{
		//	Create an instance of the graph builder
		if((pIGraphBuilder = GetGraphBuilder()) == 0)
			break;

		//	Render the file requested by the caller
		m_hResult = pIGraphBuilder->RenderFile(wFile, NULL);
		if(!SUCCEEDED(m_hResult))
		{
			HandleError("GetDuration(%s)", lpszFilename);
			break;
		}

		//	Get the MediaSeeking interface
		m_hResult = pIGraphBuilder->QueryInterface(IID_IMediaSeeking, (void **)&pIMediaSeeking);
		if(!SUCCEEDED(m_hResult))
		{
			HandleError("GetDuration(%s)", lpszFilename);
			break;
		}

		//	Set the seek format to time based seeking
		m_hResult = pIMediaSeeking->SetTimeFormat(&Format);
		if(!SUCCEEDED(m_hResult))
		{
			HandleError("GetDuration(%s)", lpszFilename);
			break;
		}

		//	Get the duration of the file
		m_hResult = pIMediaSeeking->GetDuration(&llDuration);
		if(!SUCCEEDED(m_hResult))
		{
			HandleError("GetDuration(%s)", lpszFilename);
			break;
		}

		//	Convert from 100 nanosecond units to seconds
		dDuration = (((double)llDuration) / (double)10000000);
		bSuccessful = true;

	}

	RELEASE_INTERFACE(pIMediaSeeking);
	RELEASE_INTERFACE(pIGraphBuilder);

	return dDuration;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetEvent()
//
// 	Description:	This function is called to retrieve an event from the
//					media event interface
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetEvent(long* pCode)
{
	HRESULT hResult;
	long	lParam1;
	long	lParam2;

	ASSERT(pCode);

	//	Do we have the required interface?
	if(!m_pIMediaEventEx)
		return FALSE;

	hResult = m_pIMediaEventEx->GetEvent(pCode, &lParam1, &lParam2, 0);

	//	Are we out of events?
	if(!SUCCEEDED(hResult))
		return FALSE;

	//	Free the parameters associated with this event
	m_pIMediaEventEx->FreeEventParams(*pCode, lParam1, lParam2);
	return TRUE;

}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetFilters()
//
// 	Description:	This function is called to retrieve the list of filters 
//					currently in use by the graph manager. 
//
// 	Returns:		TRUE if successful
//
//	Notes:			The filter list is returned as a composite string. CR is
//					used to delimit filters in the string
//
//==============================================================================
BOOL CIDXShow::GetFilters(CString& rFilters, long* pCount, BOOL bVendorInfo)
{
	IAMCollection*	pIAMCollection;
	IFilterInfo*	pIFilterInfo;
	HRESULT			hResult;
	long			lCount;
	char			szName[256];
	char			szVendor[256];
	BSTR			bszVendor;
	BSTR			bszName;

	//	We have to have the media control interface
	if(!m_pIMediaControl)
		return FALSE;

	//	Get a collection interface
	hResult = m_pIMediaControl->get_FilterCollection((IDispatch**)&pIAMCollection);
	if(FAILED(hResult))
		return FALSE;

	//	Get the number of registered filters
	hResult = pIAMCollection->get_Count(&lCount);
	if(FAILED(hResult))
	{
		pIAMCollection->Release();
		return FALSE;
	}

	//	Clear the filter list
	rFilters.Empty();

	//	Iterate the collection
	for(int i = 0; i < lCount; i++)
	{
		//	Get the filter information interface for the next filter
		pIFilterInfo = 0;
		pIAMCollection->Item(i, (IUnknown**)&pIFilterInfo);

		if(pIFilterInfo == 0)
			continue;

		//	Get the filter name
		pIFilterInfo->get_Name(&bszName);

		//	Convert the name to null terminated string
		WideCharToMultiByte(CP_ACP, 0, bszName, -1, szName, 
						    sizeof(szName), 0, 0);

		//	Add it to the filter list
		rFilters += szName;
		rFilters += "\r";

		//	Do we want to include the vendor info?
		if(bVendorInfo)
		{
			pIFilterInfo->get_VendorInfo(&bszVendor);
			WideCharToMultiByte(CP_ACP, 0, bszVendor, -1, szVendor, 
								sizeof(szVendor), 0, 0);

			//	Add the vendor info
			rFilters += szVendor;
			rFilters += "\r";
		}

		//	Release the information interface
		pIFilterInfo->Release();
	}

	//	Release the collection
	pIAMCollection->Release();

	//	Set the filter count
	if(pCount)
		*pCount = lCount;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetGraphBuilder()
//
// 	Description:	This function will allocate and initialize a new graph
//					builder interface
//
// 	Returns:		Pointer to the new interface
//
//	Notes:			None
//
//==============================================================================
IGraphBuilder* CIDXShow::GetGraphBuilder()
{
	IGraphBuilder*	pIGraphBuilder = 0;
	IBaseFilter*	pIBaseFilter = 0;

	//	Create an instance of the graph builder
	m_hResult = CoCreateInstance(CLSID_FilterGraph, NULL, 
								 CLSCTX_INPROC_SERVER, 
							     IID_IGraphBuilder, 
								(void **)&pIGraphBuilder);
	//	Did the request fail
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("GetGraphBuilder");
		return 0;
	}
    ASSERT(pIGraphBuilder);

	//	Are we supposed to add user specified filters to the graph?
	if((m_paFilters != 0) && (m_iFilters > 0))
	{
		//	Add each of the filters specified by the player
		//
		//	NOTE:	The filter array is not packed so we have to check each element in 
		//			the array
		for(int i = 0; i < IDXSHOW_MAX_FILTERS; i++)
		{
			if(!m_paFilters[i].strName.IsEmpty())
			{
				//	Create an instance of this filter
				CoCreateInstance(m_paFilters[i].ClsId, 
								 NULL, CLSCTX_INPROC, IID_IBaseFilter, 
								 (void **)&pIBaseFilter);
				
				if(pIBaseFilter != 0)
				{
					//	Add the filter to the graph
					m_hResult = pIGraphBuilder->AddFilter(pIBaseFilter, 
														  m_paFilters[i].strName.AllocSysString());
					//	Were we successful?
					if(FAILED(m_hResult))
					{
						RELEASE_INTERFACE(pIBaseFilter);
						if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_ADDFILTERFAILED, 
														m_paFilters[i].strName);
					}
				}
				else
				{
					if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_CREATEFILTERFAILED, 
													m_paFilters[i].strName);
				}

				//	Remove from the list if unable to add this filter
				if((pIBaseFilter == 0) && (m_pPlayer != 0))
					m_pPlayer->RemoveFilter(m_paFilters[i].strName);
			
			}
		
		}

	}

	return pIGraphBuilder;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetInterface()
//
// 	Description:	This function will get the interface requested by the caller
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
LPVOID CIDXShow::GetInterface(short sInterface)
{
	//	Which interface does the caller want?
	switch(sInterface)
	{
		case TMMOVIE_IGRAPHBUILDER:		return m_pIGraphBuilder;
		case TMMOVIE_IBASICVIDEO:		return m_pIBasicVideo;
		case TMMOVIE_IBASICAUDIO:		return m_pIBasicAudio;
		case TMMOVIE_IMEDIACONTROL:		return m_pIMediaControl;
		case TMMOVIE_IMEDIAEVENTEX:		return m_pIMediaEventEx;
		case TMMOVIE_IMEDIAPOSITION:	return m_pIMediaPosition;
		case TMMOVIE_IMEDIASEEKING:		return m_pIMediaSeeking;
		case TMMOVIE_IVIDEOWINDOW:		return m_pIVideoWindow;
		default:						return NULL;
	}
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetInterface()
//
// 	Description:	This function will open the interface requested by the caller
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetInterface(REFIID Id, void** pInterface)
{
	//	We MUST already have the graph builder interface?
	ASSERT(m_pIGraphBuilder);

	//	Get the requested interface
	m_hResult = m_pIGraphBuilder->QueryInterface(Id, pInterface);

	//	Did the request succeed?
	if(SUCCEEDED(m_hResult))
		return TRUE;

	//	Display an error message
	HandleError("GetInterface");

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetInterfaces()
//
// 	Description:	This function will get the interfaces we need to control
//					the video playback
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetInterfaces()
{
    ASSERT(m_pIGraphBuilder);

	//	Get the media control interface
    if(!GetInterface(IID_IMediaControl, (void **)&m_pIMediaControl))
		return FALSE;

	//	Get the media position interface
    if(!GetInterface(IID_IMediaPosition, (void **)&m_pIMediaPosition))
		return FALSE;

	//	Get the media seeking interface
    if(!GetInterface(IID_IMediaSeeking, (void **)&m_pIMediaSeeking))
		return FALSE;

	//	Get the media events interface
    if(!GetInterface(IID_IMediaEventEx, (void **)&m_pIMediaEventEx))
		return FALSE;

	//	Get the basic video interface
    if(!GetInterface(IID_IBasicVideo, (void **)&m_pIBasicVideo))
		return FALSE;

	//	Get the basic audio interface
    if(!GetInterface(IID_IBasicAudio, (void **)&m_pIBasicAudio))
		return FALSE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetPos()
//
// 	Description:	This function is called to get the current position
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetPos(double* pPosition)
{
	HRESULT		hResult;
	LONGLONG	lPos;

	ASSERT(pPosition);

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;

	hResult = m_pIMediaSeeking->GetCurrentPosition(&lPos);

	//	Were we able to get the current position ?
	if(!SUCCEEDED(hResult))
		return FALSE;

	*pPosition = ConvertPosition(lPos);
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetRenderWnd()
//
// 	Description:	This function is called to get the handle of the video 
//					rendering window
//
// 	Returns:		The handle of the rendering window
//
//	Notes:			None
//
//==============================================================================
HWND CIDXShow::GetRenderWnd()
{
	return m_hRenderer;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetStartPos()
//
// 	Description:	This function is called to get the start position
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetStartPos(double* pPosition)
{
	LONGLONG	llStart;
	LONGLONG	llStop;

	ASSERT(pPosition);

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;

	//	Get the current positions
	m_hResult = m_pIMediaSeeking->GetPositions(&llStart, &llStop);

	//	Were we able to get the start position ?
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("GetStartPos()");
		return FALSE;
	}

	*pPosition = ConvertPosition(llStart);
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetState()
//
// 	Description:	This function is called to get the current state of the
//					media control interface
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetState(long* pState)
{
	HRESULT hResult;

	ASSERT(pState);

	//	Do we have the required interface?
	if(!m_pIMediaControl)
		return FALSE;

	hResult = m_pIMediaControl->GetState(100, pState);

	//	Were we able to get the current state?
	return (SUCCEEDED(hResult));
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetStopPos()
//
// 	Description:	This function is called to get the stop position
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetStopPos(double* pPosition)
{
	LONGLONG	llStart;
	LONGLONG	llStop;

	ASSERT(pPosition);

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;

	//	Get the current positions
	m_hResult = m_pIMediaSeeking->GetPositions(&llStart, &llStop);

	//	Were we able to get the start position ?
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("GetStopPos()");
		return FALSE;
	}

	*pPosition = ConvertPosition(llStop);
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetRange()
//
// 	Description:	This function is called to get the current playback range
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetRange(double* pStart, double* pStop)
{
	LONGLONG	llStart;
	LONGLONG	llStop;

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;

	//	Get the current positions
	m_hResult = m_pIMediaSeeking->GetPositions(&llStart, &llStop);

	//	Were we able to get the start position ?
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("GetRange()");
		return FALSE;
	}

	if(pStart != 0) *pStart = ConvertPosition(llStart);
	if(pStop != 0)  *pStop = ConvertPosition(llStop);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetVideoProps()
//
// 	Description:	This function will get the properties associated with the
//					currently rendered video
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::GetVideoProps()
{
    HRESULT		hResult;
	long		lWidth;
	long		lHeight;
	REFTIME		RefTime;
	BOOL		bReturn = TRUE;
	LONGLONG	llDuration;

	//	Set the default values
	m_lHeight    = 480;
	m_lWidth     = 640;
	m_fFrameRate = IDXSHOW_FRAMERATE;

	//	Don't bother if we don't have the required interfaces
	if(m_pIBasicVideo == 0 && m_pIMediaSeeking == 0)
		return FALSE;

	//	Get the length of the video
	hResult = m_pIMediaSeeking->GetDuration(&llDuration);
	if(SUCCEEDED(hResult))
	{
		m_dDuration = ConvertPosition(llDuration);
	}
	else
	{
		m_dDuration = (double)LONG_MAX;	//	Arbitraily large value
		bReturn = FALSE;
	}

	//	Get the frame rate for this video
	switch(m_iSeekMode)
	{
		case TMMOVIE_SEEK_FRAMES:

			hResult = m_pIBasicVideo->get_AvgTimePerFrame(&RefTime);
			if(SUCCEEDED(hResult) && (RefTime > 0))
			{
				m_fFrameRate = 1.0f / (float)RefTime;
			}
			else
			{
				ASSERT(FALSE);
				bReturn = FALSE;
			}
			break;

		case TMMOVIE_SEEK_TIME:
		default:

			m_fFrameRate = 0;
			break;

	}

	//	Get the size of the video 
	hResult = m_pIBasicVideo->GetVideoSize(&lWidth, &lHeight);
	if(SUCCEEDED(hResult))
	{
		m_lHeight = lHeight;
		m_lWidth  = lWidth;
	}
	else
		bReturn = FALSE;

	//	Compute the aspect ratio of the video
	m_fAspectRatio = (float)m_lWidth / (float)m_lHeight;

	return bReturn;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::GetVideoRect()
//
// 	Description:	This function is called to retrieve a pointer to the video
//					display rectangle.
//
// 	Returns:		A pointer to the video rectangle
//
//	Notes:			None
//
//==============================================================================
RECT* CIDXShow::GetVideoRect()
{
    return &m_rcVideoPos;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::HandleError()
//
// 	Description:	This function will handle DirectMedia errors
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CIDXShow::HandleError(LPCSTR lpszFormat, ...)
{
	char	szDxMsg[MAX_ERROR_TEXT_LEN];
	char	szPrefix[1024];
	CString	strErrorMsg;

	//	Do we have a valid error and handler?
	if(m_pErrors == 0 || SUCCEEDED(m_hResult))
		return;

	//	Is the error handler enabled?
	if(!m_pErrors->IsEnabled())
		return;

	memset(szDxMsg, 0, sizeof(szDxMsg));
	memset(szPrefix, 0, sizeof(szPrefix));

	//	Get the error DirectX message
	if(!AMGetErrorText(m_hResult, szDxMsg, sizeof(szDxMsg)))
		lstrcpy(szDxMsg, "An Undefined DirectShow Error Occurred");

	//	Is the caller providing a formatted prefix for the message?
	if((lpszFormat != 0) && (lstrlen(lpszFormat) > 0))
	{
		//	Declare the variable list of arguements            
		va_list	vaArgs;

		//	Insert the first variable arguement into the arguement list
		va_start(vaArgs, lpszFormat);

		//	Format the message
		vsprintf_s(szPrefix, sizeof(szPrefix), lpszFormat, vaArgs);

		//	Clean up the arguement list
		va_end(vaArgs);

	}

	if(lstrlen(szPrefix) > 0)
		strErrorMsg.Format("IDXShow::%s - %s", szPrefix, szDxMsg);
	else
		strErrorMsg.Format("IDXShow Error - %s", szDxMsg);
		
	m_pErrors->Handle("TMMovie::DirectShow Error", strErrorMsg);
}

//==============================================================================
//
// 	Function Name:	CIDXShow::Initialize()
//
// 	Description:	This function will initialize the object
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::Initialize(HWND hParent, CErrorHandler* pErrors)
{
	//	Save the parent window handle and error handler
	m_hParent = hParent;
	m_pErrors = pErrors;
	ASSERT(m_pErrors);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::IsAudio()
//
// 	Description:	This function is called to determine if the active file is
//					audio only
//
// 	Returns:		TRUE if audio only
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::IsAudio()
{
	//	Do we have an active graph?
	if(m_pIGraphBuilder != 0)
	{
		//	If no video window we assume this is an audio only file
		return (m_pIVideoWindow == 0);
	}
	else
	{
		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CIDXShow::IsVisible()
//
// 	Description:	This function is called to check the visibility of the
//					rendering window
//
// 	Returns:		TRUE if visible
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::IsVisible()
{
	long lVisible;

	//	Do we have the video window interface
	if(m_pIVideoWindow == 0)
		return FALSE;

	//	Get the current visibility
	m_hResult = m_pIVideoWindow->get_Visible(&lVisible);

	//	Did the request fail
	if(!SUCCEEDED(m_hResult))
		return FALSE;

	return (lVisible == OATRUE);	
}

//==============================================================================
//
// 	Function Name:	CIDXShow::OnTimer()
//
// 	Description:	This function is called to check the visibility of the
//					rendering window
//
// 	Returns:		TRUE if visible
//
//	Notes:			None
//
//==============================================================================
void CIDXShow::OnTimer()
{
}

//==============================================================================
//
// 	Function Name:	CIDXShow::Pause()
//
// 	Description:	This function will pause the playback
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::Pause()
{
	long lState = State_Stopped;

	//	Don't bother if we have got a file
	if(m_pIMediaControl == 0)
		return FALSE;

	//	Pause the file
	m_hResult = m_pIMediaControl->Pause();
	m_pIMediaControl->GetState(INFINITE, &lState);

	//	Did the request succeed?
	if(SUCCEEDED(m_hResult))
		return TRUE;

	//	Display an error message
	HandleError("Pause()");

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::Play()
//
// 	Description:	This function is called to play from the specified start
//					position to the specified stop position
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::Play(double dFrom, double dTo)
{
	LONGLONG	llStart = ConvertTime(dFrom);
	LONGLONG	llStop  = ConvertTime(dTo);

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;
	
	//	Get the current position
	m_hResult = m_pIMediaSeeking->SetPositions(&llStart, 
											   AM_SEEKING_AbsolutePositioning,
											   &llStop, 
											   AM_SEEKING_AbsolutePositioning | AM_SEEKING_Segment);
/*
	CString M;
	double dStart;
	double dStop;
	GetRange(&dStart, &dStop);
	M.Format("From: %f\nTo: %f\nStart: %f\nStop: %f\n", dFrom, dTo, dStart, dStop);
	MessageBox(0, M, "", MB_OK);
*/
	//	Did the request fail?
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("Play(%.4f, %.4f)", dFrom, dTo);
		return FALSE;
	}

	return Run();
}

//==============================================================================
//
// 	Function Name:	CIDXShow::Redraw()
//
// 	Description:	This function is called to redraw the rendering window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CIDXShow::Redraw()
{
	CWnd* pRenderer;

	//	Don't bother unless we have a visible window
	if(!m_hRenderer || !IsVisible())
		return;
	//	Get the window
	if((pRenderer = CWnd::FromHandle(m_hRenderer)) == 0)
		return;
	else
	{
		pRenderer->Invalidate();
		pRenderer->SendMessage(WM_PAINT, 0, 0);
	}		
}

//==============================================================================
//
// 	Function Name:	CIDXShow::ReleaseAll()
//
// 	Description:	This function will release all the COM interfaces.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CIDXShow::ReleaseAll()
{
	//	Stop the playback
	if(m_pIMediaControl)
		m_pIMediaControl->Stop();

	//	Release all the COM interfaces
	RELEASE_INTERFACE(m_pIMediaPosition);
	RELEASE_INTERFACE(m_pIMediaSeeking);
	RELEASE_INTERFACE(m_pIMediaControl);
	RELEASE_INTERFACE(m_pIMediaEventEx);
	RELEASE_INTERFACE(m_pIGraphBuilder);
	RELEASE_INTERFACE(m_pIBasicVideo);
	RELEASE_INTERFACE(m_pIBasicAudio);
}

//==============================================================================
//
// 	Function Name:	CIDXShow::Render()
//
// 	Description:	This function will render the video file provided by the
//					caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::Render(LPCSTR lpFile)
{
	WCHAR	wFile[MAX_PATH];
	int		iSeekMode;

	ASSERT(lpFile);

    //	Convert to wide character format
	MultiByteToWideChar(CP_ACP, 0, lpFile, -1, wFile, sizeof(wFile));

	//	Release the existing interfaces
	ReleaseAll();
	
	//	Create an instance of the graph builder
	if((m_pIGraphBuilder = GetGraphBuilder()) == 0)
		return FALSE;

	//	Should we detach the window before loading a new file?
	if(m_bDetachBeforeLoad && (m_pIVideoWindow != 0))
	{
		m_pIVideoWindow->put_Visible(OAFALSE);
		m_pIVideoWindow->put_Owner(NULL);
		RELEASE_INTERFACE(m_pIVideoWindow);
	}

	//	Render the file requested by the caller
	m_hResult = m_pIGraphBuilder->RenderFile(wFile, NULL);

	//	Did the request fail
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("Render(%s)", lpFile);
		return FALSE;
	}

	//	Get the interfaces we will need to control the playback
	if(!GetInterfaces())
		return FALSE;	

	ASSERT(m_pIMediaControl);
	ASSERT(m_pIMediaEventEx);
	ASSERT(m_pIMediaSeeking);
	ASSERT(m_pIMediaPosition);
	ASSERT(m_pIBasicVideo);
	ASSERT(m_pIBasicAudio);

	//	Inihibit redrawing of the control window while we set up the new
	//	control interfaces
	if(pControl) pControl->m_bDoDraw = FALSE;

	//	Set the media seeking interface to the appropriate mode
	iSeekMode = TMMOVIE_SEEK_TIME;
	SetSeekMode(iSeekMode);
	
    // Have the graph signal event via window callbacks for performance
    m_pIMediaEventEx->SetNotifyWindow((OAHWND)m_hParent, WM_IDXSHOWNOTIFY, m_lId);

	//	Reset the video window interface
	//
	//	NOTE:	This MUST be done BEFORE we pause the interface or else
	//			the interface will create and show a new window on the
	//			change of state
	ResetWindow();

    //	Wait for the data to be ready at the rendering pin
	m_pIMediaControl->Pause();
	m_pIMediaControl->StopWhenReady();

	//	Get the video's properties
	GetVideoProps();

	//	Set the initial playing range to the full file extents
	SetRange((double)0, m_dDuration, FALSE);
	
	//	Renable drawing of the control window
	if(pControl) pControl->m_bDoDraw = TRUE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::ResetWindow()
//
// 	Description:	This function will reset the rendering window interface
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CIDXShow::ResetWindow()
{
	CString	strCaption;
	BSTR	bstrCaption;
	HRESULT	hResult = NULL;

	//	Delete the existing interface if it exists
	//
	//	NOTE:	This MUST be done first in order to insure that systems with
	//			MPEG acceleration will ALWAYS use the accelerator for playback
	//			If we try to open another interface with an existing one still
	//			open, we will wind up with a non-accelerated rendering window
	//	Make sure the video window is destroyed
	if(m_pIVideoWindow)
	{
		m_pIVideoWindow->put_Visible(OAFALSE);
		m_pIVideoWindow->put_Owner(NULL);
		RELEASE_INTERFACE(m_pIVideoWindow);
	}
	
	//	Notify the player
	if(m_pPlayer)
		m_pPlayer->OnIDXSetWnd();
	
	//	Just in case ...
	ASSERT(m_pIGraphBuilder != 0); 
	if(m_pIGraphBuilder == 0) return;

	//	Get the video window interface from the graph builder
	if(SUCCEEDED(m_pIGraphBuilder->QueryInterface(IID_IVideoWindow, (void **)&m_pIVideoWindow)))
	{
		//	Is this interface supported?
		if(m_pIVideoWindow->put_Owner((OAHWND)m_hParent) == E_NOINTERFACE)
		{
			//	Assume this is an audio file
			RELEASE_INTERFACE(m_pIVideoWindow);
		}

	}
	else
	{
		m_pIVideoWindow = 0;
	}

	//	Stop here if not able to get a supported video interface
	if(m_pIVideoWindow == 0) return;

	//	Setup the playback window properties
	m_pIVideoWindow->put_WindowStyle(WS_CHILD|WS_CLIPSIBLINGS|WS_CLIPCHILDREN);
	m_pIVideoWindow->put_AutoShow(OAFALSE);
	m_pIVideoWindow->put_MessageDrain((OAHWND)m_hParent);

	//	Assign a unique caption to the rendering window
	strCaption.Format("CIDXShow%ld", m_lId);
	bstrCaption = strCaption.AllocSysString();
	m_pIVideoWindow->put_Caption(bstrCaption);
	SysFreeString(bstrCaption);

	//	Now use the caption to retrieve the handle of the rendering window
	if((m_hRenderer = FindWindowEx(m_hParent, NULL, NULL, strCaption)) != 0)
	{
		//	Size and position the new window
		SetVideoPos();
	}
	else
	{
		//	Notify the user
		if(m_pErrors) m_pErrors->Handle(0, IDS_TMMOVIE_FINDWINDOWFAILED);
	}

	//	Notify the player
	if(m_pPlayer)
		m_pPlayer->OnIDXSetWnd();
	
}

//==============================================================================
//
// 	Function Name:	CIDXShow::Run()
//
// 	Description:	This function will start the playback
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::Run()
{
	//	Don't bother if we have got a file
	if(m_pIMediaControl == 0)
		return FALSE;

	//	Play the file
	m_hResult = m_pIMediaControl->Run();

	//	Did the request succeed?
	if(SUCCEEDED(m_hResult))
		return TRUE;

	//	Display an error message
	HandleError("Run");

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetBalance()
//
// 	Description:	This function is called to set the playback audio balance
//
// 	Returns:		TRUE if successful
//
//	Notes:			The balance is specified by the caller as a range of -100 to
//					100 with 0 being normal
//
//==============================================================================
BOOL CIDXShow::SetBalance(short sBalance)
{
	HRESULT	hResult;
	long	lBalance;

	//	Do we have the required interface?
	if(!m_pIBasicAudio)
		return FALSE;

	//	Is the value within range?
	if(sBalance < -100)
		sBalance = -100;
	else if(sBalance > 100)
		sBalance = 100;

	//	Calculate the desired balance
	//
	//	The interface range is -10000 to 10000 
	lBalance = sBalance * 100;

	hResult = m_pIBasicAudio->put_Balance(lBalance);
	
	//	Did the request succeed?
	return (SUCCEEDED(m_hResult));
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetDetachBeforeLoad()
//
// 	Description:	This function is called to set the detach before load option
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::SetDetachBeforeLoad(BOOL bDetach)
{
	m_bDetachBeforeLoad = bDetach;
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetMaxRect()
//
// 	Description:	This function is called by the player to set the extents 
//					for the viewport
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::SetMaxRect(RECT* pRect)
{
	ASSERT(pRect);

	//	Save the new extents
	memcpy(&m_rcMaxWnd, pRect, sizeof(m_rcMaxWnd));

	//	Stop here if we have not yet loaded a file
	if(m_pIVideoWindow == 0)
		return TRUE;

	//	Set the video size and position
	return SetVideoPos();
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetPos()
//
// 	Description:	This function is called to set the current position
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::SetPos(double dPosition, BOOL bRun)
{
	LONGLONG	llPos  = ConvertTime(dPosition);
	LONGLONG	llStop = ConvertTime(dPosition);

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;
	
	//	Make sure the interface is paused.
	//
	//	We MUST do this here to make sure the frame is properly updated!
	Pause();

	//	Reset the stop frame to the last frame in the file
	m_hResult = m_pIMediaSeeking->SetPositions(&llPos, AM_SEEKING_AbsolutePositioning | AM_SEEKING_SeekToKeyFrame,
									           &llStop, AM_SEEKING_AbsolutePositioning | AM_SEEKING_SeekToKeyFrame);
	//	Notify the player
	if(m_pPlayer)
		m_pPlayer->OnIDXSetPos();
	
	//	Make sure the interface is stabalized
	Pause();

	//	Did the request fail?
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("SetPosition(%.4f, %s)", dPosition, bRun ? "TRUE" : "FALSE");
		return FALSE;
	}

	//	Run the video if requested
	if(bRun)
		return Run();
	else	
		return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetRange()
//
// 	Description:	This function is called to set the playback range
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::SetRange(double dStart, double dStop, BOOL bRun)
{
	LONGLONG	llStart = 0;
	LONGLONG	llStop  = 0;
	double		dCurrentStart = 0;
	double		dCurrentStop = 0;
	double		dPosition;

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;

	//	Check to see if we're already at the desired start position. If so, we
	//	have to bump the start position by one frame or else it will cause a
	//	problem with the timing on playlists
	GetPos(&dPosition);
	if((dStart > 0) && (dPosition == dStart))
	{
		if((dStart + IDXSHOW_SECONDS_PER_FRAME) < dStop)
			dStart += IDXSHOW_SECONDS_PER_FRAME;
	}

	llStart = ConvertTime(dStart);
	llStop  = ConvertTime(dStop);

	m_hResult = m_pIMediaSeeking->SetPositions(&llStart, 
											   AM_SEEKING_AbsolutePositioning,
											   &llStop, 
											   AM_SEEKING_AbsolutePositioning);
	//	Did the request fail?
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("SetRange(%.4f, %.4f, %s)", dStart, dStop, bRun ? "TRUE" : "FALSE");
		return FALSE;
	}

	//	Run the video if requested
	if((bRun == TRUE) && (dStart < dStop))
		return Run();
	else	
		return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetRate()
//
// 	Description:	This function is called to set the playback rate
//
// 	Returns:		TRUE if successful
//
//	Notes:			The rate is specified by the caller as a range of -100 to
//					100 with 0 being normal
//
//==============================================================================
BOOL CIDXShow::SetRate(short sRate)
{
	HRESULT	hResult;
	double	dRate;

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;

	//	Is the value within range?
	if(sRate < -100)
		sRate = -100;
	else if(sRate > 100)
		sRate = 100;

	//	Does the caller want normal speed?
	if(sRate == 0)
	{
		dRate = (double)1;
	}

	//	Does the caller want to speed up the video?
	else if(sRate > 0)
	{
		dRate = (0.02f * (double)sRate) + 1.0f;
	}
	else
	{
		dRate = (0.006667f * (double)sRate) + 1.0f;
	}
	
	hResult = m_pIMediaSeeking->SetRate(dRate);
	
	//	Did the request succeed?
	return (SUCCEEDED(hResult));
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetScaleProps()
//
// 	Description:	This function is called to set the scaling properties used
//					to size and position the video
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::SetScaleProps(BOOL bScale, BOOL bAspect)
{
	//	Save the new properties
	m_bScale  = bScale;
	m_bAspect = bAspect;

	//	Stop here if we have not yet loaded a file
	if(m_pIVideoWindow == 0)
		return TRUE;

	return SetVideoPos();
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetSeekMode()
//
// 	Description:	This function is called to set the mode used to cue the
//					video
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::SetSeekMode(int iMode)
{
	GUID Format;

	if(iMode == TMMOVIE_SEEK_FRAMES)
	{
		Format = TIME_FORMAT_FRAME;
	}
	else
	{
		Format = TIME_FORMAT_MEDIA_TIME;
	}

	//	Set the interface mode
	if(m_pIMediaSeeking)
	{
		if(m_pIMediaSeeking->IsFormatSupported(&Format) != S_OK)
		{
			ASSERT(iMode == TMMOVIE_SEEK_FRAMES);
			iMode = TMMOVIE_SEEK_TIME;
			Format = TIME_FORMAT_MEDIA_TIME;
			MessageBox(0,"Seek format not supported", "", MB_OK);
		}

		m_pIMediaSeeking->SetTimeFormat(&Format);
	}

	//	Save the new mode identifier
	m_iSeekMode = iMode;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetVideoPos()
//
// 	Description:	This function is called to position the video within the
//					available viewport
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::SetVideoPos()
{
	//	Don't bother if we have not yet loaded a file
	if(m_pIVideoWindow == 0)
		return TRUE;

	//	Calculate the new video rectangle
	CalcVideoRect();

	m_hResult = m_pIVideoWindow->SetWindowPosition((long)m_rcVideoPos.left,
												   (long)m_rcVideoPos.top,
												   (long)m_rcVideoPos.right,
												   (long)m_rcVideoPos.bottom);
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("SetVideoPos");
		return FALSE;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetVolume()
//
// 	Description:	This function is called to set the playback volume
//
// 	Returns:		TRUE if successful
//
//	Notes:			The volume is specified by the caller as a range of 0 to 100
//
//==============================================================================
BOOL CIDXShow::SetVolume(short sVolume)
{
	HRESULT	hResult;
	long	lVolume;

	//	Do we have the required interface?
	if(!m_pIBasicAudio)
		return FALSE;

	//	Is the value within range?
	if(sVolume < 0)
		sVolume = 0;
	else if(sVolume > 100)
		sVolume = 100;

	//	Calculate the desired volume
	//
	//	The interface range is -10000 to 0 with 0 being full volume. The scale
	//	is logarithmic
	lVolume = (long)pow(10.0f, (5.0f - ((float)sVolume / 25.0f)));
	lVolume *= -1;

	hResult = m_pIBasicAudio->put_Volume(lVolume);
	
	//	Did the request succeed?
	return (SUCCEEDED(m_hResult));
}

//==============================================================================
//
// 	Function Name:	CIDXShow::Show()
//
// 	Description:	This function will control the visibility of the playback
//					window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::Show(BOOL bShow, BOOL bRefresh)
{
	BOOL bIsVisible;

	//	Must have an active filter graph
	if(m_pIGraphBuilder == 0) return FALSE;

	//	Ignore the request if we are playing an audio file
	if(IsAudio() == TRUE) return TRUE;
	ASSERT(m_pIVideoWindow != 0);

	//	Get the current visibility
	bIsVisible = IsVisible();

	//	Set the visibility
	if(bShow && !bIsVisible)
	{
		m_hResult = m_pIVideoWindow->put_Visible(OATRUE);
		
		if(SUCCEEDED(m_hResult))
		{
			//	Activate the rendering window
			//m_pIVideoWindow->SetWindowForeground(-1);
			
			//	Do we need to refresh the display?
			if(bRefresh)
				UpdatePos(FALSE);

			//	Notify the player
			if(m_pPlayer)
				m_pPlayer->OnIDXShow(TRUE);
	
			return TRUE;
		}
		else
		{
			HandleError("Show(%s, %s)", bShow ? "TRUE" : "FALSE", bRefresh ? "TRUE" : "FALSE");
			return FALSE;
		}
	}
	else if(!bShow && bIsVisible)
	{
		//	Make sure we pause the playback first
		Pause();

		m_pIVideoWindow->put_Visible(OAFALSE);
		if(SUCCEEDED(m_hResult))
		{
			//	Notify the player
			if(m_pPlayer)
				m_pPlayer->OnIDXShow(FALSE);
	
			return TRUE;
		}
		else
		{
			HandleError("Show(%s, %s)", bShow ? "TRUE" : "FALSE", bRefresh ? "TRUE" : "FALSE");
			return FALSE;
		}
	}
	else
	{
		return TRUE;
	}

}

//==============================================================================
//
// 	Function Name:	CIDXShow::Step()
//
// 	Description:	This function is called to step from the specified start
//					position to the specified stop position
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::Step(double dFrom, double dTo)
{
	LONGLONG	llStart = ConvertTime(dFrom);
	LONGLONG	llStop  = ConvertTime(dTo);

	//	Do we have the required interface?
	if(!m_pIMediaSeeking)
		return FALSE;
	
	//	Get the current position
	m_hResult = m_pIMediaSeeking->SetPositions(&llStart, 
											   AM_SEEKING_AbsolutePositioning,
											   &llStop, 
											   AM_SEEKING_AbsolutePositioning);
	//	Did the request fail?
	if(!SUCCEEDED(m_hResult))
	{
		HandleError("Step(%.4f, %.4f)", dFrom, dTo);
		return FALSE;
	}

	return Run();
}

//==============================================================================
//
// 	Function Name:	CIDXShow::Stop()
//
// 	Description:	This function will stop the playback
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::Stop()
{
	//	Don't bother if we have got a file
	if(m_pIMediaControl == 0)
		return FALSE;

	//	Play the file
	m_hResult = m_pIMediaControl->Stop();

	//	Did the request succeed
	if(SUCCEEDED(m_hResult))
		return TRUE;

	HandleError("Stop");

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CIDXShow::ShowState()
//
// 	Description:	This function is called to display the current state of the
//					media control interface
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function is provided for debugging purposes
//
//==============================================================================
void CIDXShow::ShowState()
{
	HRESULT hResult;
	long	lState;

	//	Do we have the required interface?
	if(!m_pIMediaControl)
	{
		AfxMessageBox("NoControl");
		return;
	}

	hResult = m_pIMediaControl->GetState(0, &lState);

	//	Were we able to get the current state?
	if(hResult != S_OK)
		AfxMessageBox("Indeterminate");

	switch(lState)
	{
		case State_Stopped:		AfxMessageBox("Stopped");
								break;

		case State_Paused:		AfxMessageBox("Paused");
								break;

		case State_Running:		AfxMessageBox("Running");
								break;

		default:				AfxMessageBox("Unknown");
								break;
	}

}

//==============================================================================
//
// 	Function Name:	CIDXShow::SetPos()
//
// 	Description:	This function is called to set the current position
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CIDXShow::UpdatePos(BOOL bRun)
{
	double dCurrent;
	
	//	Get the current position
	if(!GetPos(&dCurrent))
		return FALSE;

	//	Reset the position
	return SetPos(dCurrent, bRun);
}



