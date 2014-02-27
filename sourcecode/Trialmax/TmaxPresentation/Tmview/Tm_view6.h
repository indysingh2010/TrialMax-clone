

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:34:07 2014
 */
/* Compiler settings for Tm_view6.odl:
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


#ifndef __Tm_view6_h__
#define __Tm_view6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTm_view6_FWD_DEFINED__
#define ___DTm_view6_FWD_DEFINED__
typedef interface _DTm_view6 _DTm_view6;

#endif 	/* ___DTm_view6_FWD_DEFINED__ */


#ifndef ___DTm_view6Events_FWD_DEFINED__
#define ___DTm_view6Events_FWD_DEFINED__
typedef interface _DTm_view6Events _DTm_view6Events;

#endif 	/* ___DTm_view6Events_FWD_DEFINED__ */


#ifndef __Tm_view6_FWD_DEFINED__
#define __Tm_view6_FWD_DEFINED__

#ifdef __cplusplus
typedef class Tm_view6 Tm_view6;
#else
typedef struct Tm_view6 Tm_view6;
#endif /* __cplusplus */

#endif 	/* __Tm_view6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_view6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_view6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_view6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_VIEW6Lib_LIBRARY_DEFINED__
#define __TM_VIEW6Lib_LIBRARY_DEFINED__

/* library TM_VIEW6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_VIEW6Lib;

#ifndef ___DTm_view6_DISPINTERFACE_DEFINED__
#define ___DTm_view6_DISPINTERFACE_DEFINED__

/* dispinterface _DTm_view6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTm_view6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("B6EE6D22-30F7-434B-96D2-DFE4A99E246A")
    _DTm_view6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTm_view6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTm_view6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTm_view6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTm_view6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTm_view6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTm_view6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTm_view6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTm_view6 * This,
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
    } _DTm_view6Vtbl;

    interface _DTm_view6
    {
        CONST_VTBL struct _DTm_view6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTm_view6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTm_view6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTm_view6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTm_view6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTm_view6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTm_view6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTm_view6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTm_view6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTm_view6Events_DISPINTERFACE_DEFINED__
#define ___DTm_view6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTm_view6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTm_view6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("A46EC24D-5EE5-45EB-AAEA-8DE5A1EA8D22")
    _DTm_view6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTm_view6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTm_view6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTm_view6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTm_view6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTm_view6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTm_view6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTm_view6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTm_view6Events * This,
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
    } _DTm_view6EventsVtbl;

    interface _DTm_view6Events
    {
        CONST_VTBL struct _DTm_view6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTm_view6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTm_view6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTm_view6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTm_view6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTm_view6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTm_view6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTm_view6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTm_view6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_Tm_view6;

#ifdef __cplusplus

class DECLSPEC_UUID("5A3A9FC9-D747-4B92-9106-A32C7E6E84A3")
Tm_view6;
#endif
#endif /* __TM_VIEW6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


