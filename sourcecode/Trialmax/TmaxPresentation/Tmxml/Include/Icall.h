//==============================================================================
//
// File Name:	icall.h
//
// Description:	This file contains the declarations of the CICallback class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	06-04-01	1.00		Original Release
//==============================================================================
#if !defined(__ICALL_H__)
#define __ICALL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CDownload;
class CXmlFrame;

//	This class is used to implement a custom status callback to be used during
//	file downloads

#pragma warning(disable:4100)   // disable warnings about unreferenced params

class CICallback : public IBindStatusCallback  
{
	private:

	public:
	
							CICallback();
						   ~CICallback();

    //	Overloaded IBindStatusCallback methods. The only method called by IE is
	//	OnProgress() so the others just return E_NOTIMPL
    STDMETHOD(OnStartBinding)(
        /* [in] */ DWORD dwReserved,
        /* [in] */ IBinding __RPC_FAR *pib);

    STDMETHOD(GetPriority)(
        /* [out] */ LONG __RPC_FAR *pnPriority);

    STDMETHOD(OnLowResource)(
        /* [in] */ DWORD dwReserved);

    STDMETHOD(OnProgress)(
        /* [in] */ ULONG ulProgress,
        /* [in] */ ULONG ulProgressMax,
        /* [in] */ ULONG ulStatusCode,
        /* [in] */ LPCWSTR wszStatusText);

    STDMETHOD(OnStopBinding)(
        /* [in] */ HRESULT hresult,
        /* [unique][in] */ LPCWSTR lpszError);

    STDMETHOD(GetBindInfo)(
        /* [out] */ DWORD __RPC_FAR *pgrfBINDF,
        /* [unique][out][in] */ BINDINFO __RPC_FAR *pbindinfo);
 
    STDMETHOD(OnDataAvailable)(
        /* [in] */ DWORD grfBSCF,
        /* [in] */ DWORD dwSize,
        /* [in] */ FORMATETC __RPC_FAR *pformatetc,
        /* [in] */ STGMEDIUM __RPC_FAR *pstgmed);

    STDMETHOD(OnObjectAvailable)(
        /* [in] */ REFIID riid,
        /* [iid_is][in] */ IUnknown __RPC_FAR *punk);

    //	Overloaded IUnknown methods.  IE never calls any of these methods, since
    //	the caller owns the IBindStatusCallback interface, so the methods all
    //	return zero/E_NOTIMPL.

    STDMETHOD_(ULONG,AddRef)();
	STDMETHOD_(ULONG,Release)();

    STDMETHOD(QueryInterface)(
    /* [in] */ REFIID riid,
    /* [iid_is][out] */ void __RPC_FAR *__RPC_FAR *ppvObject);
};

#pragma warning(default:4100)

#endif // !defined(__ICALL_H__)
