

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:28:06 2014
 */
/* Compiler settings for Tm_bars6.odl:
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


#ifndef __Tm_bars6_h__
#define __Tm_bars6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMBars6_FWD_DEFINED__
#define ___DTMBars6_FWD_DEFINED__
typedef interface _DTMBars6 _DTMBars6;

#endif 	/* ___DTMBars6_FWD_DEFINED__ */


#ifndef ___DTMBars6Events_FWD_DEFINED__
#define ___DTMBars6Events_FWD_DEFINED__
typedef interface _DTMBars6Events _DTMBars6Events;

#endif 	/* ___DTMBars6Events_FWD_DEFINED__ */


#ifndef __TMBars6_FWD_DEFINED__
#define __TMBars6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMBars6 TMBars6;
#else
typedef struct TMBars6 TMBars6;
#endif /* __cplusplus */

#endif 	/* __TMBars6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_bars6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_bars6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_bars6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_BARS6Lib_LIBRARY_DEFINED__
#define __TM_BARS6Lib_LIBRARY_DEFINED__

/* library TM_BARS6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_BARS6Lib;

#ifndef ___DTMBars6_DISPINTERFACE_DEFINED__
#define ___DTMBars6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMBars6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMBars6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("22E390AD-0E5E-42A2-9070-AD2665C2520E")
    _DTMBars6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMBars6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMBars6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMBars6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMBars6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMBars6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMBars6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMBars6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMBars6 * This,
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
    } _DTMBars6Vtbl;

    interface _DTMBars6
    {
        CONST_VTBL struct _DTMBars6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMBars6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMBars6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMBars6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMBars6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMBars6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMBars6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMBars6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMBars6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMBars6Events_DISPINTERFACE_DEFINED__
#define ___DTMBars6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMBars6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMBars6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("5AEECE8F-EC7D-427D-8728-67E15172538C")
    _DTMBars6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMBars6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMBars6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMBars6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMBars6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMBars6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMBars6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMBars6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMBars6Events * This,
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
    } _DTMBars6EventsVtbl;

    interface _DTMBars6Events
    {
        CONST_VTBL struct _DTMBars6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMBars6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMBars6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMBars6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMBars6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMBars6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMBars6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMBars6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMBars6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMBars6;

#ifdef __cplusplus

class DECLSPEC_UUID("5284E5B7-9E77-4200-9E9F-D5F22CB40F2C")
TMBars6;
#endif
#endif /* __TM_BARS6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


