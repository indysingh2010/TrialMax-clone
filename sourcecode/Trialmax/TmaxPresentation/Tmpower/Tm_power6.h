

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:29:35 2014
 */
/* Compiler settings for Tm_power6.odl:
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


#ifndef __Tm_power6_h__
#define __Tm_power6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMPower6_FWD_DEFINED__
#define ___DTMPower6_FWD_DEFINED__
typedef interface _DTMPower6 _DTMPower6;

#endif 	/* ___DTMPower6_FWD_DEFINED__ */


#ifndef ___DTMPower6Events_FWD_DEFINED__
#define ___DTMPower6Events_FWD_DEFINED__
typedef interface _DTMPower6Events _DTMPower6Events;

#endif 	/* ___DTMPower6Events_FWD_DEFINED__ */


#ifndef __TMPower6_FWD_DEFINED__
#define __TMPower6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMPower6 TMPower6;
#else
typedef struct TMPower6 TMPower6;
#endif /* __cplusplus */

#endif 	/* __TMPower6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_power6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_power6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_power6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_POWER6Lib_LIBRARY_DEFINED__
#define __TM_POWER6Lib_LIBRARY_DEFINED__

/* library TM_POWER6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_POWER6Lib;

#ifndef ___DTMPower6_DISPINTERFACE_DEFINED__
#define ___DTMPower6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMPower6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMPower6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("0D6CBE90-9DD2-4AC9-91A7-199A3E5ED3F0")
    _DTMPower6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMPower6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMPower6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMPower6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMPower6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMPower6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMPower6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMPower6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMPower6 * This,
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
    } _DTMPower6Vtbl;

    interface _DTMPower6
    {
        CONST_VTBL struct _DTMPower6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMPower6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMPower6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMPower6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMPower6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMPower6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMPower6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMPower6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMPower6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMPower6Events_DISPINTERFACE_DEFINED__
#define ___DTMPower6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMPower6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMPower6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("BBFECF38-3DE1-498B-9D96-946C40ED2AC8")
    _DTMPower6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMPower6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMPower6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMPower6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMPower6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMPower6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMPower6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMPower6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMPower6Events * This,
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
    } _DTMPower6EventsVtbl;

    interface _DTMPower6Events
    {
        CONST_VTBL struct _DTMPower6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMPower6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMPower6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMPower6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMPower6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMPower6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMPower6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMPower6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMPower6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMPower6;

#ifdef __cplusplus

class DECLSPEC_UUID("BD138FDB-21B2-4CF1-8175-A94182FED781")
TMPower6;
#endif
#endif /* __TM_POWER6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


