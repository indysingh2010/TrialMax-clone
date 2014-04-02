/*************************************************************
   Ltsys.h - operating system definition
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTSYS_H)
#define LTSYS_H

#if defined(_LEAD_FORCE_UNICODE_)
   #define FOR_UNICODE
#endif // if defined(_LEAD_FORCE_UNICODE_)

#if defined(_UNICODE)
   // NOTE: if you use the non-UNICODE version of LEADTOOLS in a UNICODE app, then you must undefine this !
   #define FOR_UNICODE
   #if !defined(UNICODE)
      #error UNICODE and _UNICODE must be defined!!!
   #endif // #if !defined(UNICODE)
#endif // #if defined(_UNICODE)

#if defined(_WIN32_WCE)
   #define FOR_WINCE
#elif defined(WIN64) || defined(_WIN64)
   #define FOR_WIN64
#elif defined(WIN32) || defined(_WIN32_WCE)
   #define FOR_WIN32
#endif // #if defined(WIN64)

#if defined(_DEBUG)
   #define FOR_DEBUG
#endif // #if defined(_DEBUG)

#include "Ltver.h"

#endif // #if !defined(LTSYS_H)
