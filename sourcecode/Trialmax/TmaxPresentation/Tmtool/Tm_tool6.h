

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:31:51 2014
 */
/* Compiler settings for Tm_tool6.odl:
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


#ifndef __Tm_tool6_h__
#define __Tm_tool6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMTool6_FWD_DEFINED__
#define ___DTMTool6_FWD_DEFINED__
typedef interface _DTMTool6 _DTMTool6;

#endif 	/* ___DTMTool6_FWD_DEFINED__ */


#ifndef ___DTMTool6Events_FWD_DEFINED__
#define ___DTMTool6Events_FWD_DEFINED__
typedef interface _DTMTool6Events _DTMTool6Events;

#endif 	/* ___DTMTool6Events_FWD_DEFINED__ */


#ifndef __TMTool6_FWD_DEFINED__
#define __TMTool6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMTool6 TMTool6;
#else
typedef struct TMTool6 TMTool6;
#endif /* __cplusplus */

#endif 	/* __TMTool6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_tool6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_tool6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_tool6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_TOOL6Lib_LIBRARY_DEFINED__
#define __TM_TOOL6Lib_LIBRARY_DEFINED__

/* library TM_TOOL6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_TOOL6Lib;

#ifndef ___DTMTool6_DISPINTERFACE_DEFINED__
#define ___DTMTool6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMTool6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMTool6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("3CBDF435-832A-4A4D-8005-B732170641F7")
    _DTMTool6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMTool6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMTool6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMTool6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMTool6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMTool6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMTool6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMTool6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMTool6 * This,
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
    } _DTMTool6Vtbl;

    interface _DTMTool6
    {
        CONST_VTBL struct _DTMTool6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMTool6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMTool6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMTool6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMTool6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMTool6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMTool6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMTool6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMTool6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMTool6Events_DISPINTERFACE_DEFINED__
#define ___DTMTool6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMTool6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMTool6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("9259569A-473C-416A-A780-FC655CFDE923")
    _DTMTool6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMTool6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMTool6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMTool6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMTool6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMTool6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMTool6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMTool6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMTool6Events * This,
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
    } _DTMTool6EventsVtbl;

    interface _DTMTool6Events
    {
        CONST_VTBL struct _DTMTool6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMTool6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMTool6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMTool6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMTool6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMTool6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMTool6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMTool6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMTool6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMTool6;

#ifdef __cplusplus

class DECLSPEC_UUID("2341B5A2-769B-49CC-8652-B8914992AFB1")
TMTool6;
#endif
#endif /* __TM_TOOL6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


