#if !defined(AFX_STDAFX_H__FEB40DFA_FA01_11D0_B002_008029EFD140__INCLUDED_)
#define AFX_STDAFX_H__FEB40DFA_FA01_11D0_B002_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

// stdafx.h : include file for standard system include files,
//      or project specific include files that are used frequently,
//      but are changed infrequently

#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

#include <tmtargetver.h>	// Target version identifiers for all TrialMax components


#include <afxctl.h>         // MFC support for ActiveX Controls
#include <afxcmn.h>			//	Windows common controls

// Lead Tools support
#define USE_DLG_COM
//#import "..\\..\\..\\..\\Bin\\CDLL\\Win32\\LtocxKrnDlgu.DLL" no_namespace, named_guids, exclude("WindowLevelFillLUTConstants")
//#import "..\\..\\..\\..\\Bin\\CDLL\\Win32\\LtocxImgDlgu.dll" no_namespace, named_guids
//#import "..\\..\\..\\..\\Bin\\CDLL\\Win32\\LtocxImgEfxDlgu.dll" no_namespace, named_guids
//#import "..\\..\\..\\..\\Bin\\CDLL\\Win32\\LtocxClrDlgu.dll" no_namespace, named_guids
//#import "..\\..\\..\\..\\Bin\\CDLL\\Win32\\LtocxFileDlgu.dll" no_namespace, named_guids
//#import "..\\..\\..\\..\\Bin\\CDLL\\Win32\\LtocxImgDocDlgu.dll" no_namespace, named_guids
//#import "..\\..\\..\\..\\Bin\\CDLL\\Win32\\LtocxWebDlgu.dll" no_namespace, named_guids,exclude("JPEGFormatConstants")
//#import "..\\..\\..\\..\\Bin\\CDLL\\Win32\\LtocxEfxDlgu.dll" no_namespace, named_guids
#include <ltkrn.h>
#include <l_ocx.h>

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__FEB40DFA_FA01_11D0_B002_008029EFD140__INCLUDED_)
