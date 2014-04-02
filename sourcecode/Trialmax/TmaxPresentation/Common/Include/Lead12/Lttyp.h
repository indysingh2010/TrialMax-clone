/*
   LTTYP.H - basic data types
   Copyright (c) 1991-2000 LEAD Technologies, Inc.
   All Rights Reserved.
*/

#ifndef _LTTYP_H_
#define _LTTYP_H_
#ifdef FOR_LINUX
#include "windows.h"
#endif
#include "ltsys.h"

#define _HEADER_ENTRY_
#include "ltpck.h"

typedef char L_CHAR;            /* signed 8-bit integer */
typedef unsigned char L_UCHAR;  /* unsigned 8-bit integer */
typedef short L_INT16;          /* signed 16-bit integer */
typedef unsigned short L_UINT16;/* unsigned 16-bit integer */
typedef long L_INT32;           /* signed 32-bit integer */
typedef unsigned long L_UINT32; /* unsigned 32-bit integer */
typedef int L_INT;              /* signed integer 16-bits or larger */
typedef unsigned L_UINT;        /* unsigned integer 16-bits or larger */
typedef void L_VOID;            /* void */
typedef L_UINT L_BOOL;          /* boolean */
typedef L_UINT L_BITFIELD;      /* bit field */
typedef float  L_FLOAT;         /* floating point number */
typedef double L_DOUBLE;        /* double precision floating point number */
typedef RGBQUAD L_RGBQUAD;      /* RGBQUAD structure */

#ifdef FOR_WIN
   #define L_FAR           _far    /* segmented memory attribute < 64K */
   #define L_HUGE          _huge   /* segmented memory attribute > 64K */
   #define L_EXPORT        _export /* export attribute */
   #define L_PASCAL        _pascal /* pascal calling convention */
#elif defined(FOR_WIN32) || defined(FOR_LINUX)
   #define L_FAR                   /* segmented memory attribute < 64K */
   #define L_HUGE                  /* segmented memory attribute > 64K */
   #define L_EXPORT                /* export attribute */
   #define L_PASCAL                /* pascal calling convention */
#endif

#if defined(FOR_WIN) || defined(FOR_WIN32)
   #define EXT_FUNCTION    WINAPI
   #define pEXT_FUNCTION   EXT_FUNCTION L_FAR *
#ifndef FOR_LINUX
   #define EXT_CALLBACK    _cdecl L_FAR
#else
   #define EXT_CALLBACK
#endif
   #define pEXT_CALLBACK   EXT_CALLBACK L_FAR *
#endif                          /* defined(FOR_WIN) || defined(FOR_WIN32) */

#define L_MAXPATH       260

#undef _HEADER_ENTRY_
#include "ltpck.h"

#endif                          /* _LTTYP_H_ */
