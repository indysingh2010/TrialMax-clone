/*************************************************************
   Ltdlgkrn.h - Common Dialogs module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDLG_KRN_H)
#define LTDLG_KRN_H

#if !defined(L_LTDLG_API)
   #define L_LTDLG_API
#endif

#include "Ltkrn.h"
#include "Ltimg.h"
#include "Ltfil.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

//.............................................................................
// enums, defines and structures
//.............................................................................

// Flags For L_DlgInit
#define DLG_INIT_COLOR     0x00000001

//.............................................................................
// enums, defines and structures
//.............................................................................

//.............................................................................
// Functions
//.............................................................................

L_LTDLG_API L_INT EXT_FUNCTION L_DlgInit ( L_UINT32 uFlags ) ;
L_LTDLG_API L_INT EXT_FUNCTION L_DlgFree ( ) ;

L_LTDLG_API HFONT EXT_FUNCTION L_DlgSetFont      ( HFONT hFont ) ;
L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetStringLen ( L_UINT32 uString, L_UINT * puLen ) ;
L_LTDLG_API L_INT EXT_FUNCTION L_DlgSetStringA   ( L_UINT32 uString, L_CHAR * pszString ) ;
L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetStringA   ( L_UINT32 uString, L_CHAR * pszString, L_SIZE_T sizeInWords ) ;
#if defined(FOR_UNICODE)
L_LTDLG_API L_INT EXT_FUNCTION L_DlgSetString    ( L_UINT32 uString, L_TCHAR * pszString ) ;
L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetString    ( L_UINT32 uString, L_TCHAR * pszString, L_SIZE_T sizeInWords ) ;
#else
#define L_DlgSetString L_DlgSetStringA
#define L_DlgGetString L_DlgGetStringA
#endif // #if defined(FOR_UNICODE)
//.............................................................................
// Functions
//.............................................................................

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDLG_KRN_H)
