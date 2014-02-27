

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0500 */
/* at Thu Oct 15 11:07:14 2009
 */
/* Compiler settings for ..\DSEvalKernel2\DSKernel.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

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


#ifndef __ILMDSKernel2_h__
#define __ILMDSKernel2_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ILMDSKernel2v1_FWD_DEFINED__
#define __ILMDSKernel2v1_FWD_DEFINED__
typedef interface ILMDSKernel2v1 ILMDSKernel2v1;
#endif 	/* __ILMDSKernel2v1_FWD_DEFINED__ */


#ifndef __ILMDSKernel2_FWD_DEFINED__
#define __ILMDSKernel2_FWD_DEFINED__
typedef interface ILMDSKernel2 ILMDSKernel2;
#endif 	/* __ILMDSKernel2_FWD_DEFINED__ */


#ifndef __LMDSKernel2_FWD_DEFINED__
#define __LMDSKernel2_FWD_DEFINED__

#ifdef __cplusplus
typedef class LMDSKernel2 LMDSKernel2;
#else
typedef struct LMDSKernel2 LMDSKernel2;
#endif /* __cplusplus */

#endif 	/* __LMDSKernel2_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 



#ifndef __LMDSKernelLib2_LIBRARY_DEFINED__
#define __LMDSKernelLib2_LIBRARY_DEFINED__

/* library LMDSKernelLib2 */
/* [helpstring][version][uuid] */ 

#ifndef __DSKernel2_H__
#define __DSKernel2_H__
static const CLSID CLSID_LMDSKernel2       = {0xE2B7DDA9, 0x38C5, 0x11D5, {0x91, 0xF6, 0x00, 0x10, 0x4B, 0xDB, 0x8F, 0xF9}};
static const IID IID_ILMDSKernel2v1       = {0xE2B7DDA8, 0x38C5, 0x11D5, {0x91, 0xF6, 0x00, 0x10, 0x4B, 0xDB, 0x8F, 0xF9}};
static const IID IID_ILMDSKernel2          = {0xE2B7DD8F, 0x38C5, 0x11D5, {0x91, 0xF6, 0x00, 0x10, 0x4B, 0xDB, 0x8F, 0xF9}};
static const GUID LIBID_LMDSKernelLib2     = {0xE2B7DDA7, 0x38C5, 0x11D5, {0x91, 0xF6, 0x00, 0x10, 0x4B, 0xDB, 0x8F, 0xF9}};
#endif 
typedef /* [v1_enum] */ 
enum RegistrationStateConstants2
    {	REGSTATE_EXPIRED	= 0x1,
	REGSTATE_RELEASE	= 0x2,
	REGSTATE_EVAL	= 0x3,
	REGSTATE_MM	= 0x4,
	REGSTATE_LOCKED	= 0x10000000
    } 	RegistrationStateConstants2;

typedef /* [v1_enum] */ 
enum RegistrationFlagsConstants2
    {	REGFLAGS_COMPUTER	= 0x1,
	REGFLAGS_APP	= 0x2
    } 	RegistrationFlagsConstants2;

typedef /* [v1_enum] */ 
enum UnLockFlagsConstants2
    {	UNLOCK_COMPUTER	= 0x1,
	UNLOCK_APP_ONCE	= 0x2,
	UNLOCK_APP_ALWAYS	= 0x3,
	UNLOCK_APP_PATH	= 0x4,
	UNLOCK_MM	= 0x80000000
    } 	UnLockFlagsConstants2;

typedef /* [v1_enum] */ 
enum LockFlagsConstants2
    {	LOCK_COMPUTER	= 0x1,
	LOCK_APP	= 0x2,
	LOCK_APP_PATH	= 0x4
    } 	LockFlagsConstants2;

typedef /* [v1_enum] */ 
enum ErrorCodeConstants2
    {	E_EVAL_EXPIRED	= 0x80050010,
	E_APP_ALREADY_REGISTERED	= 0x80050018,
	E_APP_NOT_FOUND	= 0x80050019
    } 	ErrorCodeConstants2;


EXTERN_C const IID LIBID_LMDSKernelLib2;

#ifndef __ILMDSKernel2v1_INTERFACE_DEFINED__
#define __ILMDSKernel2v1_INTERFACE_DEFINED__

/* interface ILMDSKernel2v1 */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_ILMDSKernel2v1;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("E2B7DDA8-38C5-11D5-91F6-00104BDB8FF9")
    ILMDSKernel2v1 : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE UnlockModule( 
            /* [in] */ BSTR Key,
            /* [in] */ long Flags,
            /* [in] */ BSTR AppID) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE LockModules( 
            /* [in] */ BSTR AppID,
            /* [in] */ long Flags) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetModuleLockState( 
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *State) = 0;
        
        virtual /* [helpstring][hidden][id] */ HRESULT STDMETHODCALLTYPE SetEvalExpiryDate( 
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [in] */ long lYear,
            /* [in] */ long lMonth,
            /* [in] */ long lDay,
            /* [in] */ VARIANT_BOOL vbReset) = 0;
        
        virtual /* [helpstring][hidden][id] */ HRESULT STDMETHODCALLTYPE SetEvalExpiryDuration( 
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [in] */ long lDays,
            /* [in] */ VARIANT_BOOL vbReset) = 0;
        
        virtual /* [helpstring][hidden][id] */ HRESULT STDMETHODCALLTYPE GetEvalExpiryDate( 
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *plYear,
            /* [out] */ long *plMonth,
            /* [out] */ long *plDay) = 0;
        
        virtual /* [helpstring][hidden][id] */ HRESULT STDMETHODCALLTYPE GetModuleInfo( 
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *plLastUsedYear,
            /* [out] */ long *plLastUsedMonth,
            /* [out] */ long *plLastUsedDay,
            /* [out] */ long *plExpiryYear,
            /* [out] */ long *plExpiryMonth,
            /* [out] */ long *plExpiryDay,
            /* [out] */ long *plReleaseCount,
            /* [out] */ long *plMMCount,
            /* [out] */ long *plModuleState) = 0;
        
        virtual /* [helpstring][hidden][id] */ HRESULT STDMETHODCALLTYPE LDSKernelService1( 
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3) = 0;
        
        virtual /* [helpstring][hidden][id] */ HRESULT STDMETHODCALLTYPE LDSKernelService3( 
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3) = 0;
        
        virtual /* [helpstring][hidden][id] */ HRESULT STDMETHODCALLTYPE LDSKernelService2( 
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ILMDSKernel2v1Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ILMDSKernel2v1 * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ILMDSKernel2v1 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ILMDSKernel2v1 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ILMDSKernel2v1 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ILMDSKernel2v1 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ILMDSKernel2v1 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ILMDSKernel2v1 * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *UnlockModule )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long Flags,
            /* [in] */ BSTR AppID);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *LockModules )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR AppID,
            /* [in] */ long Flags);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetModuleLockState )( 
            ILMDSKernel2v1 * This,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *State);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *SetEvalExpiryDate )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [in] */ long lYear,
            /* [in] */ long lMonth,
            /* [in] */ long lDay,
            /* [in] */ VARIANT_BOOL vbReset);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *SetEvalExpiryDuration )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [in] */ long lDays,
            /* [in] */ VARIANT_BOOL vbReset);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *GetEvalExpiryDate )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *plYear,
            /* [out] */ long *plMonth,
            /* [out] */ long *plDay);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *GetModuleInfo )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *plLastUsedYear,
            /* [out] */ long *plLastUsedMonth,
            /* [out] */ long *plLastUsedDay,
            /* [out] */ long *plExpiryYear,
            /* [out] */ long *plExpiryMonth,
            /* [out] */ long *plExpiryDay,
            /* [out] */ long *plReleaseCount,
            /* [out] */ long *plMMCount,
            /* [out] */ long *plModuleState);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *LDSKernelService1 )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *LDSKernelService3 )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *LDSKernelService2 )( 
            ILMDSKernel2v1 * This,
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3);
        
        END_INTERFACE
    } ILMDSKernel2v1Vtbl;

    interface ILMDSKernel2v1
    {
        CONST_VTBL struct ILMDSKernel2v1Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ILMDSKernel2v1_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ILMDSKernel2v1_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ILMDSKernel2v1_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ILMDSKernel2v1_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ILMDSKernel2v1_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ILMDSKernel2v1_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ILMDSKernel2v1_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ILMDSKernel2v1_UnlockModule(This,Key,Flags,AppID)	\
    ( (This)->lpVtbl -> UnlockModule(This,Key,Flags,AppID) ) 

#define ILMDSKernel2v1_LockModules(This,AppID,Flags)	\
    ( (This)->lpVtbl -> LockModules(This,AppID,Flags) ) 

#define ILMDSKernel2v1_GetModuleLockState(This,lCategory,lModuleID,State)	\
    ( (This)->lpVtbl -> GetModuleLockState(This,lCategory,lModuleID,State) ) 

#define ILMDSKernel2v1_SetEvalExpiryDate(This,Key,lCategory,lModuleID,lYear,lMonth,lDay,vbReset)	\
    ( (This)->lpVtbl -> SetEvalExpiryDate(This,Key,lCategory,lModuleID,lYear,lMonth,lDay,vbReset) ) 

#define ILMDSKernel2v1_SetEvalExpiryDuration(This,Key,lCategory,lModuleID,lDays,vbReset)	\
    ( (This)->lpVtbl -> SetEvalExpiryDuration(This,Key,lCategory,lModuleID,lDays,vbReset) ) 

#define ILMDSKernel2v1_GetEvalExpiryDate(This,Key,lCategory,lModuleID,plYear,plMonth,plDay)	\
    ( (This)->lpVtbl -> GetEvalExpiryDate(This,Key,lCategory,lModuleID,plYear,plMonth,plDay) ) 

#define ILMDSKernel2v1_GetModuleInfo(This,Key,lCategory,lModuleID,plLastUsedYear,plLastUsedMonth,plLastUsedDay,plExpiryYear,plExpiryMonth,plExpiryDay,plReleaseCount,plMMCount,plModuleState)	\
    ( (This)->lpVtbl -> GetModuleInfo(This,Key,lCategory,lModuleID,plLastUsedYear,plLastUsedMonth,plLastUsedDay,plExpiryYear,plExpiryMonth,plExpiryDay,plReleaseCount,plMMCount,plModuleState) ) 

#define ILMDSKernel2v1_LDSKernelService1(This,Key,plParm1,plParm2,plParm3)	\
    ( (This)->lpVtbl -> LDSKernelService1(This,Key,plParm1,plParm2,plParm3) ) 

#define ILMDSKernel2v1_LDSKernelService3(This,Key,plParm1,plParm2,plParm3)	\
    ( (This)->lpVtbl -> LDSKernelService3(This,Key,plParm1,plParm2,plParm3) ) 

#define ILMDSKernel2v1_LDSKernelService2(This,Key,plParm1,plParm2,plParm3)	\
    ( (This)->lpVtbl -> LDSKernelService2(This,Key,plParm1,plParm2,plParm3) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ILMDSKernel2v1_INTERFACE_DEFINED__ */


#ifndef __ILMDSKernel2_INTERFACE_DEFINED__
#define __ILMDSKernel2_INTERFACE_DEFINED__

/* interface ILMDSKernel2 */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_ILMDSKernel2;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("E2B7DD8F-38C5-11D5-91F6-00104BDB8FF9")
    ILMDSKernel2 : public ILMDSKernel2v1
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetModuleLockState2( 
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *State,
            /* [out] */ long *Flags) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ILMDSKernel2Vtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ILMDSKernel2 * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ILMDSKernel2 * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ILMDSKernel2 * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ILMDSKernel2 * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ILMDSKernel2 * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ILMDSKernel2 * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ILMDSKernel2 * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *UnlockModule )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long Flags,
            /* [in] */ BSTR AppID);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *LockModules )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR AppID,
            /* [in] */ long Flags);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetModuleLockState )( 
            ILMDSKernel2 * This,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *State);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *SetEvalExpiryDate )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [in] */ long lYear,
            /* [in] */ long lMonth,
            /* [in] */ long lDay,
            /* [in] */ VARIANT_BOOL vbReset);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *SetEvalExpiryDuration )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [in] */ long lDays,
            /* [in] */ VARIANT_BOOL vbReset);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *GetEvalExpiryDate )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *plYear,
            /* [out] */ long *plMonth,
            /* [out] */ long *plDay);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *GetModuleInfo )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR Key,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *plLastUsedYear,
            /* [out] */ long *plLastUsedMonth,
            /* [out] */ long *plLastUsedDay,
            /* [out] */ long *plExpiryYear,
            /* [out] */ long *plExpiryMonth,
            /* [out] */ long *plExpiryDay,
            /* [out] */ long *plReleaseCount,
            /* [out] */ long *plMMCount,
            /* [out] */ long *plModuleState);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *LDSKernelService1 )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *LDSKernelService3 )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3);
        
        /* [helpstring][hidden][id] */ HRESULT ( STDMETHODCALLTYPE *LDSKernelService2 )( 
            ILMDSKernel2 * This,
            /* [in] */ BSTR Key,
            /* [out] */ long *plParm1,
            /* [out] */ long *plParm2,
            /* [out] */ long *plParm3);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetModuleLockState2 )( 
            ILMDSKernel2 * This,
            /* [in] */ long lCategory,
            /* [in] */ long lModuleID,
            /* [out] */ long *State,
            /* [out] */ long *Flags);
        
        END_INTERFACE
    } ILMDSKernel2Vtbl;

    interface ILMDSKernel2
    {
        CONST_VTBL struct ILMDSKernel2Vtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ILMDSKernel2_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ILMDSKernel2_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ILMDSKernel2_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ILMDSKernel2_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ILMDSKernel2_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ILMDSKernel2_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ILMDSKernel2_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ILMDSKernel2_UnlockModule(This,Key,Flags,AppID)	\
    ( (This)->lpVtbl -> UnlockModule(This,Key,Flags,AppID) ) 

#define ILMDSKernel2_LockModules(This,AppID,Flags)	\
    ( (This)->lpVtbl -> LockModules(This,AppID,Flags) ) 

#define ILMDSKernel2_GetModuleLockState(This,lCategory,lModuleID,State)	\
    ( (This)->lpVtbl -> GetModuleLockState(This,lCategory,lModuleID,State) ) 

#define ILMDSKernel2_SetEvalExpiryDate(This,Key,lCategory,lModuleID,lYear,lMonth,lDay,vbReset)	\
    ( (This)->lpVtbl -> SetEvalExpiryDate(This,Key,lCategory,lModuleID,lYear,lMonth,lDay,vbReset) ) 

#define ILMDSKernel2_SetEvalExpiryDuration(This,Key,lCategory,lModuleID,lDays,vbReset)	\
    ( (This)->lpVtbl -> SetEvalExpiryDuration(This,Key,lCategory,lModuleID,lDays,vbReset) ) 

#define ILMDSKernel2_GetEvalExpiryDate(This,Key,lCategory,lModuleID,plYear,plMonth,plDay)	\
    ( (This)->lpVtbl -> GetEvalExpiryDate(This,Key,lCategory,lModuleID,plYear,plMonth,plDay) ) 

#define ILMDSKernel2_GetModuleInfo(This,Key,lCategory,lModuleID,plLastUsedYear,plLastUsedMonth,plLastUsedDay,plExpiryYear,plExpiryMonth,plExpiryDay,plReleaseCount,plMMCount,plModuleState)	\
    ( (This)->lpVtbl -> GetModuleInfo(This,Key,lCategory,lModuleID,plLastUsedYear,plLastUsedMonth,plLastUsedDay,plExpiryYear,plExpiryMonth,plExpiryDay,plReleaseCount,plMMCount,plModuleState) ) 

#define ILMDSKernel2_LDSKernelService1(This,Key,plParm1,plParm2,plParm3)	\
    ( (This)->lpVtbl -> LDSKernelService1(This,Key,plParm1,plParm2,plParm3) ) 

#define ILMDSKernel2_LDSKernelService3(This,Key,plParm1,plParm2,plParm3)	\
    ( (This)->lpVtbl -> LDSKernelService3(This,Key,plParm1,plParm2,plParm3) ) 

#define ILMDSKernel2_LDSKernelService2(This,Key,plParm1,plParm2,plParm3)	\
    ( (This)->lpVtbl -> LDSKernelService2(This,Key,plParm1,plParm2,plParm3) ) 


#define ILMDSKernel2_GetModuleLockState2(This,lCategory,lModuleID,State,Flags)	\
    ( (This)->lpVtbl -> GetModuleLockState2(This,lCategory,lModuleID,State,Flags) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ILMDSKernel2_INTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_LMDSKernel2;

#ifdef __cplusplus

class DECLSPEC_UUID("E2B7DDA9-38C5-11D5-91F6-00104BDB8FF9")
LMDSKernel2;
#endif
#endif /* __LMDSKernelLib2_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


