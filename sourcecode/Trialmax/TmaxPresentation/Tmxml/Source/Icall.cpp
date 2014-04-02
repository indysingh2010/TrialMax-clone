//==============================================================================
//
// File Name:	icall.cpp
//
// Description:	This file contains member functions of the CICallback class
//
// See Also:	icall.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	06-05-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <icall.h>
#include <download.h>
#include <xmlframe.h>

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
extern SDownload theDownload;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CICallback::AddRef()
//
// 	Description:	This function is called to attach to the interface
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG CICallback::AddRef()
{
	return 0;
}

//==============================================================================
//
// 	Function Name:	CICallback::CICallback()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CICallback::CICallback()
{
}

//==============================================================================
//
// 	Function Name:	CICallback::~CICallback()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CICallback::~CICallback()
{
}

//==============================================================================
//
// 	Function Name:	CICallback::GetBindInfo()
//
// 	Description:	This function is called by the binding interface to retrieve
//					the binding information.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::GetBindInfo(DWORD __RPC_FAR *pgrfBINDF, 
									 BINDINFO __RPC_FAR *pbindinfo)
{
	if(pbindinfo==NULL || pbindinfo->cbSize==0 || pgrfBINDF==NULL)
		return E_INVALIDARG;
	else
		return E_NOTIMPL;
}

//==============================================================================
//
// 	Function Name:	CICallback::GetPriority()
//
// 	Description:	This function is called to determine what priority should
//					be used for the download.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::GetPriority(LONG __RPC_FAR *pnPriority)
{
	HRESULT hResult = S_OK;

	if (pnPriority)
		*pnPriority = THREAD_PRIORITY_NORMAL;
	else
		hResult = E_INVALIDARG;

	return hResult;
}

//==============================================================================
//
// 	Function Name:	CICallback::OnDataAvailable()
//
// 	Description:	This function is called for asynchronous downloads when 
//					data is available on the stream.
//
// 	Returns:		E_NOTIMPL
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::OnDataAvailable(DWORD grfBSCF, DWORD dwSize,
								    FORMATETC __RPC_FAR *pformatetc,
								    STGMEDIUM __RPC_FAR *pstgmed)
{
	return E_NOTIMPL;
}

//==============================================================================
//
// 	Function Name:	CICallback::OnLowResource()
//
// 	Description:	This function is not implemented by IE
//
// 	Returns:		E_NOTIMPL
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::OnLowResource(DWORD dwReserved)
{
	return E_NOTIMPL;
}

//==============================================================================
//
// 	Function Name:	CICallback::OnObjectAvailable()
//
// 	Description:	This function is called by IE when the binding interface
//					is made available.
//
// 	Returns:		E_NOTIMPL
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::OnObjectAvailable(REFIID riid, IUnknown __RPC_FAR *punk)
{
	return E_NOTIMPL;
}

//==============================================================================
//
// 	Function Name:	CICallback::OnProgress()
//
// 	Description:	This function is called during the download progress to
//					report the status.
//
// 	Returns:		S_OK if downloading should continue
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::OnProgress(ULONG ulProgress, ULONG ulProgressMax,
							   ULONG ulStatusCode, LPCWSTR wszStatusText)
{

	switch(ulStatusCode)
	{
		case BINDSTATUS_BEGINDOWNLOADDATA:
		case BINDSTATUS_DOWNLOADINGDATA:
		
			//	Has the operation been aborted?
			if(theDownload.bAbort == TRUE)
				return E_ABORT;

			//	Update the global download information
			theDownload.ulProgress = ulProgress;
			theDownload.ulProgressMax = ulProgressMax;
			theDownload.strStatus = wszStatusText;

			//	Should we post a message?
			if((theDownload.hWnd != 0) && IsWindow(theDownload.hWnd))
			{
				PostMessage(theDownload.hWnd, WM_DOWNLOAD,  
							TMXML_DOWNLOAD_PROGRESS, theDownload.lParam);
			}
			
			break;

		case BINDSTATUS_ENDDOWNLOADDATA:

			break;

		default:

			break;
	}

	return S_OK;
}

//==============================================================================
//
// 	Function Name:	CICallback::OnStartBinding()
//
// 	Description:	This function is called when the process begins
//
// 	Returns:		E_NOTIMPL
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::OnStartBinding(DWORD dwReserved, IBinding __RPC_FAR *pib)
{
	return E_NOTIMPL;
}

//==============================================================================
//
// 	Function Name:	CICallback::OnStopBinding()
//
// 	Description:	This function is called when the operation is finished.
//
// 	Returns:		E_NOTIMPL
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::OnStopBinding(HRESULT hResult, LPCWSTR lpszError)
{
	return E_NOTIMPL;
}

//==============================================================================
//
// 	Function Name:	CICallback::QueryInterface()
//
// 	Description:	This function is called to query the callback interface.
//
// 	Returns:		E_NOTIMPL
//
//	Notes:			None
//
//==============================================================================
HRESULT CICallback::QueryInterface(REFIID riid, void __RPC_FAR *__RPC_FAR *ppvObject)
{
	return E_NOTIMPL;
}

//==============================================================================
//
// 	Function Name:	CICallback::Release()
//
// 	Description:	This function is called to dettach to the interface
//
// 	Returns:		Zero
//
//	Notes:			None
//
//==============================================================================
ULONG CICallback::Release()
{
	return 0;
}


