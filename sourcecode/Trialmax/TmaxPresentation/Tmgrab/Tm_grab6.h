

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:28:51 2014
 */
/* Compiler settings for Tm_grab6.odl:
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


#ifndef __Tm_grab6_h__
#define __Tm_grab6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMGrab6_FWD_DEFINED__
#define ___DTMGrab6_FWD_DEFINED__
typedef interface _DTMGrab6 _DTMGrab6;

#endif 	/* ___DTMGrab6_FWD_DEFINED__ */


#ifndef ___DTMGrab6Events_FWD_DEFINED__
#define ___DTMGrab6Events_FWD_DEFINED__
typedef interface _DTMGrab6Events _DTMGrab6Events;

#endif 	/* ___DTMGrab6Events_FWD_DEFINED__ */


#ifndef __TMGrab6_FWD_DEFINED__
#define __TMGrab6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMGrab6 TMGrab6;
#else
typedef struct TMGrab6 TMGrab6;
#endif /* __cplusplus */

#endif 	/* __TMGrab6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_grab6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_grab6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_grab6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_GRAB6Lib_LIBRARY_DEFINED__
#define __TM_GRAB6Lib_LIBRARY_DEFINED__

/* library TM_GRAB6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_GRAB6Lib;

#ifndef ___DTMGrab6_DISPINTERFACE_DEFINED__
#define ___DTMGrab6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMGrab6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMGrab6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("7EB545C4-FB50-41EB-AA0F-DFC679F24680")
    _DTMGrab6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMGrab6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMGrab6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMGrab6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMGrab6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMGrab6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMGrab6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMGrab6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMGrab6 * This,
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
    } _DTMGrab6Vtbl;

    interface _DTMGrab6
    {
        CONST_VTBL struct _DTMGrab6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMGrab6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMGrab6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMGrab6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMGrab6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMGrab6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMGrab6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMGrab6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMGrab6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMGrab6Events_DISPINTERFACE_DEFINED__
#define ___DTMGrab6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMGrab6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMGrab6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("AB015BE7-6E4B-4991-9517-0EEF54F0B50A")
    _DTMGrab6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMGrab6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMGrab6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMGrab6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMGrab6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMGrab6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMGrab6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMGrab6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMGrab6Events * This,
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
    } _DTMGrab6EventsVtbl;

    interface _DTMGrab6Events
    {
        CONST_VTBL struct _DTMGrab6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMGrab6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMGrab6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMGrab6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMGrab6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMGrab6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMGrab6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMGrab6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMGrab6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMGrab6;

#ifdef __cplusplus

class DECLSPEC_UUID("4BA3488C-31EC-4619-9D96-1EFE592DD861")
TMGrab6;
#endif
#endif /* __TM_GRAB6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


