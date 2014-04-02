/*************************************************************
   Lttyp.h - basic data types
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTTYP_H)
#define LTTYP_H

#include "Ltsys.h"
#include <windows.h>
#include <math.h>
#if defined(FOR_WINCE)
   //#include <fcntl.h>
   #define _O_RDONLY	0x0000	/* open for reading only */
   #define _O_WRONLY	0x0001	/* open for writing only */
   #define _O_RDWR 	0x0002	/* open for reading and writing */
   #define _O_APPEND	0x0008	/* writes done at eof */
   #define _O_CREAT	0x0100	/* create and open file */
   #define _O_TRUNC	0x0200	/* open and truncate */
   #define _O_BINARY	0x8000	/* file mode is binary (untranslated) */
   #define O_RDONLY	_O_RDONLY
   #define O_BINARY	_O_BINARY
   #define O_CREAT 	_O_CREAT
   #define O_TRUNC 	_O_TRUNC
   #define O_RDWR    _O_RDWR
   #define O_APPEND  _O_APPEND
   //#include <stat.h>
   #define _S_IREAD	0000400 	/* read permission, owner */
   #define _S_IWRITE	0000200 	/* write permission, owner */
   #define S_IREAD  _S_IREAD
   #define S_IWRITE _S_IWRITE
#else
   #include <fcntl.h>
   #include <sys\stat.h>
#endif // #if defined(FOR_WINCE)
#include <assert.h>
#include "ltcrt.h"

// needed for Windows Mobile
#if !defined(_MAX_FNAME)
#define _MAX_FNAME  256 /* max. length of file name component */
#define _MAX_EXT    256 /* max. length of extension component */
#endif // #if !defined(_MAX_FNAME)

#define L_HEADER_ENTRY
#include "Ltpck.h"

// Character types
typedef WCHAR           L_WCHAR; // 16-bit UNICODE character
typedef CHAR            L_CHAR;  // signed 8-bit integer
typedef UCHAR           L_UCHAR; // unsigned 8-bit integer

// Integer types
#if defined(INT8)
typedef INT8               L_INT8;     // 8-bit signed integer
typedef INT16              L_INT16;    // 16-bit signed integer
typedef INT32              L_INT32;    // 32-bit signed integer
typedef INT64              L_INT64;    // 64-bit signed integer
typedef UINT8              L_UINT8;    // 8-bit unsigned integer
typedef UINT16             L_UINT16;   // 16-bit unsigned integer
typedef UINT32             L_UINT32;   // 32-bit unsigned integer
typedef UINT64             L_UINT64;   // 64-bit unsigned integer
#else
typedef signed char        L_INT8;     // 8-bit signed integer
typedef signed short       L_INT16;    // 16-bit signed integer
typedef signed int         L_INT32;    // 32-bit signed integer
typedef signed __int64     L_INT64;    // 64-bit signed integer
typedef unsigned char      L_UINT8;    // 8-bit unsigned integer
typedef unsigned short     L_UINT16;   // 16-bit unsigned integer
typedef unsigned int       L_UINT32;   // 32-bit unsigned integer
typedef unsigned __int64   L_UINT64;   // 64-bit unsigned integer
#endif // #if defined(INT8)
typedef INT                L_INT;      // 32-bit signed integer
typedef UINT               L_UINT;     // 32-bit unsigned integer

// long and short
typedef LONG               L_LONG;
typedef ULONG              L_ULONG;
typedef USHORT             L_USHORT;

// Float types
typedef FLOAT              L_FLOAT;    // single precision 32-bit number (IEEE 754)

#if !defined(DOUBLE)
typedef double DOUBLE;
typedef DOUBLE             L_DOUBLE;   // double-precision 64-bit number (IEEE 754)
#endif // #if !defined(DOUBLE)

// 32/64 bit types
typedef INT_PTR            L_INT_PTR;
typedef UINT_PTR           L_UINT_PTR;

#if defined(LONG_PTR)
typedef LONG_PTR           L_LONG_PTR;
typedef ULONG_PTR          L_ULONG_PTR;
#else
   #if defined(_WIN64)
      typedef __int64            L_LONG_PTR;
      typedef unsigned __int64   L_ULONG_PTR;
   #else
      #if (_MSC_VER <=1200) || (_WIN32_WCE)
         typedef long          L_LONG_PTR;
         typedef unsigned long L_ULONG_PTR;
      #else   
         typedef _W64 long          L_LONG_PTR;
         typedef _W64 unsigned long L_ULONG_PTR;
      #endif
   #endif // #if defined(FOR_WIN64)
   typedef L_LONG_PTR            LONG_PTR;
   typedef L_ULONG_PTR           ULONG_PTR;
   typedef L_ULONG_PTR           DWORD_PTR;
#endif // #if defined(LONG_PTR)

#if defined(FOR_WIN64)
   #define L_UNALIGNED __unaligned
#else
   #define L_UNALIGNED
#endif // #if defined(FOR_WIN64)

typedef SIZE_T             L_SIZE_T;   // Maximum number of bytes to which a pointer can refer
typedef SSIZE_T            L_SSIZE_T;  // Signed L_SIZE_T

// Other types
typedef VOID               L_VOID;     // void
typedef L_UINT             L_BITFIELD; // bit field (in a structure of a class)

// Booleans
typedef BOOL               L_BOOL;     // boolean
#define L_TRUE    TRUE
#define L_FALSE   FALSE

typedef COLORREF           L_COLORREF;

// Unicode types
#if defined (_LEAD_FORCE_UNICODE_)
   typedef WCHAR            L_TCHAR;
#else
   typedef TCHAR            L_TCHAR;
#endif // #if defined (_LEAD_FORCE_UNICODE_)
#if !defined(L_TEXT)
#define L_TEXT          TEXT
#endif // #if !defined(L_TEXT)

// Handles
#define L_DECLARE_HANDLE   DECLARE_HANDLE

typedef HANDLE          L_HANDLE;
typedef HWND            L_HWND;
typedef HGDIOBJ         L_HGDIOBJ;
typedef HDC             L_HDC;
typedef HRGN            L_HRGN;
typedef HPALETTE        L_HPALETTE;
typedef HBITMAP         L_HBITMAP;
typedef HCURSOR         L_HCURSOR;
typedef HICON           L_HICON;
typedef HMETAFILE       L_HMETAFILE;
typedef HENHMETAFILE    L_HENHMETAFILE;
typedef HGLOBAL         L_HDIB;
typedef HINSTANCE       L_HINSTANCE;
typedef HPEN            L_HPEN;
typedef HFONT           L_HFONT;
typedef HBRUSH          L_HBRUSH;
typedef HGLOBAL         L_HGLOBAL;

L_DECLARE_HANDLE(L_HFILE);

#define L_INVALID_HANDLE_VALUE   INVALID_HANDLE_VALUE
#define L_INVALID_FILE           (L_HFILE)L_INVALID_HANDLE_VALUE

// Other defines
#if !defined(NULL)
   #define NULL 0
#endif // #if !defined(NULL)

#define L_MAXPATH    264   // 64-bit needs this to be multiple of 8

#if defined(__cplusplus)
    #define EXTERN_C extern "C"
#else
    #define EXTERN_C extern
#endif // #if defined(__cplusplus)

#define L_STDCALL       WINAPI
#define EXT_CALLBACK    WINAPI
#define pEXT_CALLBACK   EXT_CALLBACK*
#define EXT_FUNCTION    WINAPI
#define pEXT_FUNCTION   EXT_FUNCTION*

typedef WPARAM          L_WPARAM;
typedef LPARAM          L_LPARAM;
typedef LRESULT         L_LRESULT;

// Structures
typedef RGBQUAD            L_RGBQUAD;
typedef POINT              L_POINT;
typedef RECT               L_RECT;
typedef BITMAPINFOHEADER   L_BITMAPINFOHEADER;
typedef BITMAPINFO         L_BITMAPINFO;
typedef struct tagL_RGBQUAD16
{
   L_UINT16 rgbBlue;
   L_UINT16 rgbGreen;
   L_UINT16 rgbRed;
   L_UINT16 rgbReserved;
} L_RGBQUAD16;

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTTYP_H)
