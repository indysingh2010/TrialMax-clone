

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:31:30 2014
 */
/* Compiler settings for Tm_stat6.odl:
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


#ifndef __Tm_stat6_h__
#define __Tm_stat6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMStat6_FWD_DEFINED__
#define ___DTMStat6_FWD_DEFINED__
typedef interface _DTMStat6 _DTMStat6;

#endif 	/* ___DTMStat6_FWD_DEFINED__ */


#ifndef ___DTMStat6Events_FWD_DEFINED__
#define ___DTMStat6Events_FWD_DEFINED__
typedef interface _DTMStat6Events _DTMStat6Events;

#endif 	/* ___DTMStat6Events_FWD_DEFINED__ */


#ifndef __TMStat6_FWD_DEFINED__
#define __TMStat6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMStat6 TMStat6;
#else
typedef struct TMStat6 TMStat6;
#endif /* __cplusplus */

#endif 	/* __TMStat6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_stat6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_stat6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_stat6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_STAT6Lib_LIBRARY_DEFINED__
#define __TM_STAT6Lib_LIBRARY_DEFINED__

/* library TM_STAT6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_STAT6Lib;

#ifndef ___DTMStat6_DISPINTERFACE_DEFINED__
#define ___DTMStat6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMStat6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMStat6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("B6554C6C-D285-42ED-8433-B098EC38084B")
    _DTMStat6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMStat6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMStat6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMStat6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMStat6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMStat6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMStat6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMStat6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMStat6 * This,
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
    } _DTMStat6Vtbl;

    interface _DTMStat6
    {
        CONST_VTBL struct _DTMStat6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMStat6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMStat6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMStat6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMStat6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMStat6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMStat6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMStat6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMStat6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMStat6Events_DISPINTERFACE_DEFINED__
#define ___DTMStat6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMStat6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMStat6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("CD88DA40-2EAB-44E3-BB80-2D4D223A01DE")
    _DTMStat6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMStat6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMStat6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMStat6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMStat6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMStat6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMStat6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMStat6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMStat6Events * This,
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
    } _DTMStat6EventsVtbl;

    interface _DTMStat6Events
    {
        CONST_VTBL struct _DTMStat6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMStat6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMStat6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMStat6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMStat6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMStat6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMStat6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMStat6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMStat6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMStat6;

#ifdef __cplusplus

class DECLSPEC_UUID("0C69F0D1-9BB0-4DB0-A600-D98621E8D8B3")
TMStat6;
#endif
#endif /* __TM_STAT6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


