/*************************************************************
   LTTMB.H - LEAD Thumbnail Browser module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/
   
#if !defined(LTTMB_H)
#define LTTMB_H

#if !defined(L_LTTMB_API)
   #define L_LTTMB_API
#endif // #if !defined(L_LTTMB_API)

#include "Ltkrn.h"
#include "Ltimg.h"
#include "Ltfil.h"
#include <winerror.h>

#define L_HEADER_ENTRY
#include "ltpck.h"

/****************************************************************
   Callback prototypes
****************************************************************/
typedef L_INT (pEXT_CALLBACK BROWSEDIRCALLBACKA)(
   pBITMAPHANDLE pBitmap, 
   L_CHAR* pszFilename, 
   pFILEINFOA pFileInfo, 
   L_INT nStatusCode, 
   L_INT nPercent, 
   L_VOID* pUserData);

#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK BROWSEDIRCALLBACK)(
   pBITMAPHANDLE pBitmap, 
   L_TCHAR* pszFilename, 
   pFILEINFO pFileInfo, 
   L_INT nStatusCode, 
   L_INT nPercent, 
   L_VOID* pUserData);
#else
typedef BROWSEDIRCALLBACKA BROWSEDIRCALLBACK;
#endif // #if defined(FOR_UNICODE)

/****************************************************************
   Enums/defines/Errors
****************************************************************/
#define BROWSE_LOADING 2 /* used to indicate image is being loaded */
#define BROWSE_SKIPPED 3 /* used to indicate image was skipped b/c
                            it was larger than the specified range */
#define BROWSE_PRELOAD 4 /* used to indicate image is about to be
                            loaded */

/****************************************************************
   Function prototypes
****************************************************************/
L_LTTMB_API L_INT EXT_FUNCTION L_BrowseDirA( 
   L_CHAR* pszPath,
   L_CHAR* pszFilter,
   pTHUMBOPTIONS pThumbOptions,
   L_BOOL bStopOnError,
   L_BOOL bIncludeSubDirs,
   L_BOOL bExpandMultipage,
   L_SSIZE_T lSizeDisk,
   L_SSIZE_T lSizeMem,
   BROWSEDIRCALLBACKA pfnBrowseDirCB,
   L_VOID* pUserData );
#if defined(FOR_UNICODE)
L_LTTMB_API L_INT EXT_FUNCTION L_BrowseDir( 
   L_TCHAR* pszPath,
   L_TCHAR* pszFilter,
   pTHUMBOPTIONS pThumbOptions,
   L_BOOL bStopOnError,
   L_BOOL bIncludeSubDirs,
   L_BOOL bExpandMultipage,
   L_SSIZE_T lSizeDisk,
   L_SSIZE_T lSizeMem,
   BROWSEDIRCALLBACK pfnBrowseDirCB,
   L_VOID* pUserData );
#else
#define L_BrowseDir L_BrowseDirA
#endif // #if defined(FOR_UNICODE)

#undef L_HEADER_ENTRY
#include "ltpck.h"

#endif // #if !defined(LTTMB_H)



