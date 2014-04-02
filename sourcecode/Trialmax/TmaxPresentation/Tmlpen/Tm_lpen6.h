

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:28:53 2014
 */
/* Compiler settings for Tm_lpen6.odl:
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


#ifndef __Tm_lpen6_h__
#define __Tm_lpen6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMLpen6_FWD_DEFINED__
#define ___DTMLpen6_FWD_DEFINED__
typedef interface _DTMLpen6 _DTMLpen6;

#endif 	/* ___DTMLpen6_FWD_DEFINED__ */


#ifndef ___DTMLpen6Events_FWD_DEFINED__
#define ___DTMLpen6Events_FWD_DEFINED__
typedef interface _DTMLpen6Events _DTMLpen6Events;

#endif 	/* ___DTMLpen6Events_FWD_DEFINED__ */


#ifndef __TMLpen6_FWD_DEFINED__
#define __TMLpen6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMLpen6 TMLpen6;
#else
typedef struct TMLpen6 TMLpen6;
#endif /* __cplusplus */

#endif 	/* __TMLpen6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_lpen6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_lpen6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_lpen6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_LPEN6Lib_LIBRARY_DEFINED__
#define __TM_LPEN6Lib_LIBRARY_DEFINED__

/* library TM_LPEN6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_LPEN6Lib;

#ifndef ___DTMLpen6_DISPINTERFACE_DEFINED__
#define ___DTMLpen6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMLpen6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMLpen6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("32470B00-8288-4443-AAE1-426DC93FFAA5")
    _DTMLpen6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMLpen6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMLpen6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMLpen6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMLpen6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMLpen6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMLpen6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMLpen6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMLpen6 * This,
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
    } _DTMLpen6Vtbl;

    interface _DTMLpen6
    {
        CONST_VTBL struct _DTMLpen6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMLpen6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMLpen6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMLpen6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMLpen6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMLpen6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMLpen6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMLpen6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMLpen6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMLpen6Events_DISPINTERFACE_DEFINED__
#define ___DTMLpen6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMLpen6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMLpen6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("33D55C15-0F74-45EC-80F9-BF9A9CD3FA62")
    _DTMLpen6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMLpen6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMLpen6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMLpen6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMLpen6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMLpen6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMLpen6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMLpen6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMLpen6Events * This,
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
    } _DTMLpen6EventsVtbl;

    interface _DTMLpen6Events
    {
        CONST_VTBL struct _DTMLpen6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMLpen6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMLpen6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMLpen6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMLpen6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMLpen6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMLpen6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMLpen6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMLpen6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMLpen6;

#ifdef __cplusplus

class DECLSPEC_UUID("7EFCBDC0-F749-4574-8DC1-2E5575DD9808")
TMLpen6;
#endif
#endif /* __TM_LPEN6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


