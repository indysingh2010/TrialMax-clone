

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:29:17 2014
 */
/* Compiler settings for Tm_movie6.odl:
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


#ifndef __Tm_movie6_h__
#define __Tm_movie6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMMovie6_FWD_DEFINED__
#define ___DTMMovie6_FWD_DEFINED__
typedef interface _DTMMovie6 _DTMMovie6;

#endif 	/* ___DTMMovie6_FWD_DEFINED__ */


#ifndef ___DTMMovie6Events_FWD_DEFINED__
#define ___DTMMovie6Events_FWD_DEFINED__
typedef interface _DTMMovie6Events _DTMMovie6Events;

#endif 	/* ___DTMMovie6Events_FWD_DEFINED__ */


#ifndef __TMMovie6_FWD_DEFINED__
#define __TMMovie6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMMovie6 TMMovie6;
#else
typedef struct TMMovie6 TMMovie6;
#endif /* __cplusplus */

#endif 	/* __TMMovie6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_movie6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_movie6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_movie6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_MOVIE6Lib_LIBRARY_DEFINED__
#define __TM_MOVIE6Lib_LIBRARY_DEFINED__

/* library TM_MOVIE6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_MOVIE6Lib;

#ifndef ___DTMMovie6_DISPINTERFACE_DEFINED__
#define ___DTMMovie6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMMovie6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMMovie6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("2199ECB7-D80F-4188-9F90-929320B77E5C")
    _DTMMovie6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMMovie6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMMovie6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMMovie6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMMovie6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMMovie6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMMovie6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMMovie6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMMovie6 * This,
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
    } _DTMMovie6Vtbl;

    interface _DTMMovie6
    {
        CONST_VTBL struct _DTMMovie6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMMovie6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMMovie6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMMovie6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMMovie6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMMovie6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMMovie6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMMovie6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMMovie6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMMovie6Events_DISPINTERFACE_DEFINED__
#define ___DTMMovie6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMMovie6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMMovie6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("3084C1B8-A25F-4923-9B54-D73DEB761780")
    _DTMMovie6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMMovie6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMMovie6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMMovie6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMMovie6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMMovie6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMMovie6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMMovie6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMMovie6Events * This,
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
    } _DTMMovie6EventsVtbl;

    interface _DTMMovie6Events
    {
        CONST_VTBL struct _DTMMovie6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMMovie6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMMovie6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMMovie6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMMovie6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMMovie6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMMovie6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMMovie6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMMovie6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMMovie6;

#ifdef __cplusplus

class DECLSPEC_UUID("D71D2494-B9CA-401F-8E24-1815E077CE64")
TMMovie6;
#endif
#endif /* __TM_MOVIE6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


