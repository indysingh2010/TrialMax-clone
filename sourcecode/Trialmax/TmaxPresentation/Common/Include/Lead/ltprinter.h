/*************************************************************
   LtPrinter.h - LEADTOOLS runtime library
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTPRINTER_H)
#define LTPRINTER_H

#if !defined(L_LTPRINTER_API)
   #define L_LTPRINTER_API
#endif // #if !defined(L_LTPRINTER_API)

#include "Lttyp.h"
#include "Lterr.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

/****************************************************************
   Callback typedefs
****************************************************************/
typedef L_INT (pEXT_CALLBACK PRNEMFRGSPROC)(L_WCHAR * pszPrinterName,
                                            HGLOBAL  hMem,     // Should be freed using the Windows GlobalFree function
                                            L_UINT   uSize,
                                            L_VOID * pData);

typedef L_INT (pEXT_CALLBACK PRNJOBINFOPROC)(L_WCHAR *  pszPrinterName,
                                             DWORD    dwJobID,
                                             DWORD    dwFlags,
                                             L_VOID * pData);

/****************************************************************
   Enums/defines/macros
****************************************************************/

#ifndef L_MAX_PATH
#define L_MAX_PATH 260
#endif

#define MAX_STRING          255
#define MAX_PRINTER_NAME    256

// Support Types for ePrint
#define PRN_SUPPORT_DOCUMENT  1 // DOCUMENT ....
#define PRN_SUPPORT_EVAL      2 // Evaluation Copy.

#define PRN_JOB_START         0x00000001L
#define PRN_JOB_END           0x00000002L

typedef struct _tagPrnPrinterInfoA
{
   L_UINT   uStructSize;        /* Size of the structure */
   L_CHAR  *pszPrinterName;     /* Printer Name  */
   L_CHAR  *pszDriverName;      /* Driver Name   */
   L_CHAR  *pszMonitorName;     /* Monitor Name  */
   L_CHAR  *pszPortName;        /* Port Name     */
   L_CHAR  *pszProductName;     /* Product Name  */
   L_CHAR  *pszRegistryKey;     /* Reg Subkey on HKCU\Software to store Printer Information*/
   L_CHAR  *pszRootDir;         /* Root Directory*/
   L_CHAR  *pszHelpFile;        /* Help File Name*/
   L_CHAR  *pszPassword;        /* password */
   L_CHAR  *pszUrl;             /* URL to redirect user when printer is locked */
   L_CHAR  *pszPrinterExe;      /* Printer Exe Path*/
   L_CHAR  *pszAboutString;     /* About String*/
   L_CHAR  *pszAboutIcon;       /* About Icon Path*/
} PRNPRINTERINFOA, * pPRNPRINTERINFOA;

#if defined(FOR_UNICODE)
typedef struct _tagPrnPrinterInfo
{
   L_UINT   uStructSize;         /* Size of the structure */
   L_TCHAR  *pszPrinterName;     /* Printer Name  */
   L_TCHAR  *pszDriverName;      /* Driver Name   */
   L_TCHAR  *pszMonitorName;     /* Monitor Name  */
   L_TCHAR  *pszPortName;        /* Port Name     */
   L_TCHAR  *pszProductName;     /* Product Name  */
   L_TCHAR  *pszRegistryKey;     /* Reg Subkey on HKCU\Software to store Printer Information*/
   L_TCHAR  *pszRootDir;         /* Root Directory*/
   L_TCHAR  *pszHelpFile;        /* Help File Name*/
   L_TCHAR  *pszPassword;        /* password */
   L_TCHAR  *pszUrl;             /* URL to redirect user when printer is locked */
   L_TCHAR  *pszPrinterExe;      /* Printer Exe Path*/
   L_TCHAR  *pszAboutString;     /* About String*/
   L_TCHAR  *pszAboutIcon;       /* About Icon Path*/
} PRNPRINTERINFO, * pPRNPRINTERINFO;
#else
typedef PRNPRINTERINFOA PRNPRINTERINFO;
typedef pPRNPRINTERINFOA pPRNPRINTERINFO;
#endif // #if defined(FOR_UNICODE)


typedef struct _tagPrnPrinterSpecifications
{
   L_UINT   uStructSize;
   L_UINT   uPaperID;            // If the paper is a predefined paper; then the IDs
                                 // range will be: 1 - 41.
                                 // If the paper is a custom paper; then the ID will
                                 // be: >= DMPAPER_USER + 200
   L_CHAR  szPaperSizeName[ MAX_STRING ];
   L_DOUBLE dPaperWidth;
   L_DOUBLE dPaperHeight;
   L_BOOL   bDimensionsInInches;
   L_BOOL   bPortraitOrient;
   L_CHAR  szMarginsPrinter[ MAX_PRINTER_NAME ];
   L_INT    nPrintQuality;
   L_INT    nYResolution;
} PRNPRINTERSPECIFICATIONS, * pPRNPRINTERSPECIFICATIONS;

/****************************************************************
   Function prototypes
****************************************************************/
L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnLockPrinterA( L_CHAR * pszPrinterName, L_CHAR * pszPassword );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnUnlockPrinterA( L_CHAR * pszPrinterName, L_CHAR * pszPassword );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnIsPrinterLockedA( L_CHAR * pszPrinterName, L_BOOL * pbLocked );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnInstallPrinterA( PRNPRINTERINFOA * pPrnInfo, L_UINT32 uFlags);

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnUninstallPrinterA( PRNPRINTERINFOA  * pPrnInfo );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnRegisterEMFCallbackA( L_CHAR * pszPrinterName, PRNEMFRGSPROC fnEMFCallback, L_VOID * pData);

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnRegisterJobCallbackA( L_CHAR * pszPrinterName, PRNJOBINFOPROC fnJobInfoCallBack, L_VOID * pData);

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnUnRegisterEMFCallbackA( L_CHAR * pszPrinterName );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnUnRegisterJobCallbackA( L_CHAR * pszPrinterName );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnSetPrinterSpecificationsA( L_CHAR * pszPrinterName, PRNPRINTERSPECIFICATIONS * pSpecifications );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnGetPrinterSpecificationsA( L_CHAR * pszPrinterName, PRNPRINTERSPECIFICATIONS * pSpecifications );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnIsLeadtoolsPrinterA( L_CHAR  * pszPrinterName,L_BOOL* pbLeadtoolsPrinter);

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnCancelPrintedJobA( L_CHAR  * pszPrinterName,L_INT jobID);

#if defined(FOR_UNICODE)
L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnLockPrinter( L_TCHAR * pszPrinterName, L_TCHAR * pszPassword );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnUnlockPrinter( L_TCHAR * pszPrinterName, L_TCHAR * pszPassword );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnIsPrinterLocked( L_TCHAR * pszPrinterName, L_BOOL * pbLocked );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnInstallPrinter( PRNPRINTERINFO * pPrnInfo, L_UINT32 uFlags);

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnUninstallPrinter( PRNPRINTERINFO  * pPrnInfo );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnRegisterEMFCallback( L_TCHAR * pszPrinterName, PRNEMFRGSPROC fnEMFCallback, L_VOID * pData);

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnRegisterJobCallback( L_TCHAR * pszPrinterName, PRNJOBINFOPROC fnJobInfoCallBack, L_VOID * pData);

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnUnRegisterEMFCallback( L_TCHAR * pszPrinterName );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnUnRegisterJobCallback( L_TCHAR * pszPrinterName );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnSetPrinterSpecifications( L_TCHAR * pszPrinterName, PRNPRINTERSPECIFICATIONS * pSpecifications );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnGetPrinterSpecifications( L_TCHAR * pszPrinterName, PRNPRINTERSPECIFICATIONS * pSpecifications );

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnIsLeadtoolsPrinter( L_TCHAR  * pszPrinterName,L_BOOL* pbLeadtoolsPrinter);

L_LTPRINTER_API L_INT EXT_FUNCTION L_PrnCancelPrintedJob( L_TCHAR  * pszPrinterName,L_INT jobID);
#else
#define L_PrnLockPrinter                 L_PrnLockPrinterA

#define L_PrnUnlockPrinter               L_PrnUnlockPrinterA

#define L_PrnIsPrinterLocked             L_PrnIsPrinterLockedA

#define L_PrnInstallPrinter              L_PrnInstallPrinterA

#define L_PrnUninstallPrinter            L_PrnUninstallPrinterA

#define L_PrnRegisterEMFCallback         L_PrnRegisterEMFCallbackA

#define L_PrnRegisterJobCallback         L_PrnRegisterJobCallbackA

#define L_PrnUnRegisterEMFCallback       L_PrnUnRegisterEMFCallbackA

#define L_PrnUnRegisterJobCallback       L_PrnUnRegisterJobCallbackA

#define L_PrnSetPrinterSpecifications    L_PrnSetPrinterSpecificationsA

#define L_PrnGetPrinterSpecifications    L_PrnGetPrinterSpecificationsA

#define L_PrnIsLeadtoolsPrinter          L_PrnIsLeadtoolsPrinterA

#define L_PrnCancelPrintedJob          L_PrnCancelPrintedJobA
#endif // #if defined(FOR_UNICODE)


#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTPRINTER_H)
