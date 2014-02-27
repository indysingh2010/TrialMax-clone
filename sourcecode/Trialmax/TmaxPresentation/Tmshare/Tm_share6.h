

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:30:56 2014
 */
/* Compiler settings for Tm_share6.odl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.00.0595 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __Tm_share6_h__
#define __Tm_share6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMShare6_FWD_DEFINED__
#define ___DTMShare6_FWD_DEFINED__
typedef interface _DTMShare6 _DTMShare6;

#endif 	/* ___DTMShare6_FWD_DEFINED__ */


#ifndef ___DTMShare6Events_FWD_DEFINED__
#define ___DTMShare6Events_FWD_DEFINED__
typedef interface _DTMShare6Events _DTMShare6Events;

#endif 	/* ___DTMShare6Events_FWD_DEFINED__ */


#ifndef __TMShare6_FWD_DEFINED__
#define __TMShare6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMShare6 TMShare6;
#else
typedef struct TMShare6 TMShare6;
#endif /* __cplusplus */

#endif 	/* __TMShare6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_share6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_share6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_share6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_SHARE6Lib_LIBRARY_DEFINED__
#define __TM_SHARE6Lib_LIBRARY_DEFINED__

/* library TM_SHARE6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_SHARE6Lib;

#ifndef ___DTMShare6_DISPINTERFACE_DEFINED__
#define ___DTMShare6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMShare6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMShare6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("B3A5F2E5-42DD-4D3F-A7B5-F5A0B7693565")
    _DTMShare6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMShare6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMShare6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMShare6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMShare6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMShare6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMShare6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMShare6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMShare6 * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } _DTMShare6Vtbl;

    interface _DTMShare6
    {
        CONST_VTBL struct _DTMShare6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMShare6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMShare6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMShare6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMShare6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMShare6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMShare6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMShare6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMShare6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMShare6Events_DISPINTERFACE_DEFINED__
#define ___DTMShare6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMShare6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMShare6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("3CA9FF70-E3F1-4CEE-954A-918400F0F7A6")
    _DTMShare6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMShare6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMShare6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMShare6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMShare6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMShare6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMShare6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMShare6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMShare6Events * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } _DTMShare6EventsVtbl;

    interface _DTMShare6Events
    {
        CONST_VTBL struct _DTMShare6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMShare6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMShare6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMShare6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMShare6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMShare6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMShare6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMShare6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMShare6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMShare6;

#ifdef __cplusplus

class DECLSPEC_UUID("CB5D5073-AB77-45F6-B728-1808DDC80026")
TMShare6;
#endif
#endif /* __TM_SHARE6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


