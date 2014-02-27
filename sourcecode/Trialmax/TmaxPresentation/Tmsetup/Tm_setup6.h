

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:30:43 2014
 */
/* Compiler settings for Tm_setup6.odl:
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


#ifndef __Tm_setup6_h__
#define __Tm_setup6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMSetup6_FWD_DEFINED__
#define ___DTMSetup6_FWD_DEFINED__
typedef interface _DTMSetup6 _DTMSetup6;

#endif 	/* ___DTMSetup6_FWD_DEFINED__ */


#ifndef ___DTMSetup6Events_FWD_DEFINED__
#define ___DTMSetup6Events_FWD_DEFINED__
typedef interface _DTMSetup6Events _DTMSetup6Events;

#endif 	/* ___DTMSetup6Events_FWD_DEFINED__ */


#ifndef __TMSetup6_FWD_DEFINED__
#define __TMSetup6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMSetup6 TMSetup6;
#else
typedef struct TMSetup6 TMSetup6;
#endif /* __cplusplus */

#endif 	/* __TMSetup6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_setup6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_setup6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_setup6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_SETUP6Lib_LIBRARY_DEFINED__
#define __TM_SETUP6Lib_LIBRARY_DEFINED__

/* library TM_SETUP6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_SETUP6Lib;

#ifndef ___DTMSetup6_DISPINTERFACE_DEFINED__
#define ___DTMSetup6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMSetup6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMSetup6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("CFA6855C-3EFF-4822-9EEB-BDA79EE880D8")
    _DTMSetup6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMSetup6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMSetup6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMSetup6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMSetup6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMSetup6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMSetup6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMSetup6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMSetup6 * This,
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
    } _DTMSetup6Vtbl;

    interface _DTMSetup6
    {
        CONST_VTBL struct _DTMSetup6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMSetup6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMSetup6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMSetup6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMSetup6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMSetup6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMSetup6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMSetup6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMSetup6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMSetup6Events_DISPINTERFACE_DEFINED__
#define ___DTMSetup6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMSetup6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMSetup6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("E43E0287-F454-402A-A05B-E2F79E71290D")
    _DTMSetup6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMSetup6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMSetup6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMSetup6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMSetup6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMSetup6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMSetup6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMSetup6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMSetup6Events * This,
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
    } _DTMSetup6EventsVtbl;

    interface _DTMSetup6Events
    {
        CONST_VTBL struct _DTMSetup6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMSetup6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMSetup6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMSetup6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMSetup6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMSetup6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMSetup6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMSetup6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMSetup6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMSetup6;

#ifdef __cplusplus

class DECLSPEC_UUID("B581682E-5CC0-4E50-BBBC-582D78677E5A")
TMSetup6;
#endif
#endif /* __TM_SETUP6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


