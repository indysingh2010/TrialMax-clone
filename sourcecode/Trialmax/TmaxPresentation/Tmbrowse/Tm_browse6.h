

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0595 */
/* at Tue Jan 28 14:28:08 2014
 */
/* Compiler settings for Tm_browse6.odl:
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


#ifndef __Tm_browse6_h__
#define __Tm_browse6_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef ___DTMBrowse6_FWD_DEFINED__
#define ___DTMBrowse6_FWD_DEFINED__
typedef interface _DTMBrowse6 _DTMBrowse6;

#endif 	/* ___DTMBrowse6_FWD_DEFINED__ */


#ifndef ___DTMBrowse6Events_FWD_DEFINED__
#define ___DTMBrowse6Events_FWD_DEFINED__
typedef interface _DTMBrowse6Events _DTMBrowse6Events;

#endif 	/* ___DTMBrowse6Events_FWD_DEFINED__ */


#ifndef __TMBrowse6_FWD_DEFINED__
#define __TMBrowse6_FWD_DEFINED__

#ifdef __cplusplus
typedef class TMBrowse6 TMBrowse6;
#else
typedef struct TMBrowse6 TMBrowse6;
#endif /* __cplusplus */

#endif 	/* __TMBrowse6_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_Tm_browse6_0000_0000 */
/* [local] */ 

#pragma once
#pragma region Desktop Family
#pragma endregion


extern RPC_IF_HANDLE __MIDL_itf_Tm_browse6_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_Tm_browse6_0000_0000_v0_0_s_ifspec;


#ifndef __TM_BROWSE6Lib_LIBRARY_DEFINED__
#define __TM_BROWSE6Lib_LIBRARY_DEFINED__

/* library TM_BROWSE6Lib */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_TM_BROWSE6Lib;

#ifndef ___DTMBrowse6_DISPINTERFACE_DEFINED__
#define ___DTMBrowse6_DISPINTERFACE_DEFINED__

/* dispinterface _DTMBrowse6 */
/* [hidden][helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMBrowse6;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("8D68471B-F666-42D4-8EE8-A72DD2B5EAE5")
    _DTMBrowse6 : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMBrowse6Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMBrowse6 * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMBrowse6 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMBrowse6 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMBrowse6 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMBrowse6 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMBrowse6 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMBrowse6 * This,
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
    } _DTMBrowse6Vtbl;

    interface _DTMBrowse6
    {
        CONST_VTBL struct _DTMBrowse6Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMBrowse6_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMBrowse6_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMBrowse6_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMBrowse6_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMBrowse6_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMBrowse6_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMBrowse6_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMBrowse6_DISPINTERFACE_DEFINED__ */


#ifndef ___DTMBrowse6Events_DISPINTERFACE_DEFINED__
#define ___DTMBrowse6Events_DISPINTERFACE_DEFINED__

/* dispinterface _DTMBrowse6Events */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DTMBrowse6Events;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("9021DA4C-E3EA-4CCC-A39D-7B5022FC52F5")
    _DTMBrowse6Events : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DTMBrowse6EventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DTMBrowse6Events * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DTMBrowse6Events * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DTMBrowse6Events * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DTMBrowse6Events * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DTMBrowse6Events * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DTMBrowse6Events * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DTMBrowse6Events * This,
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
    } _DTMBrowse6EventsVtbl;

    interface _DTMBrowse6Events
    {
        CONST_VTBL struct _DTMBrowse6EventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DTMBrowse6Events_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DTMBrowse6Events_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DTMBrowse6Events_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DTMBrowse6Events_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DTMBrowse6Events_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DTMBrowse6Events_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DTMBrowse6Events_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DTMBrowse6Events_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_TMBrowse6;

#ifdef __cplusplus

class DECLSPEC_UUID("1B964711-19A0-4696-9489-008829D87D7E")
TMBrowse6;
#endif
#endif /* __TM_BROWSE6Lib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


