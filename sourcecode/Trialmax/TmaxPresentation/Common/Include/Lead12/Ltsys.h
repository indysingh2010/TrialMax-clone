/*
   LTSYS.H - operating system definition
   Copyright (c) 1991-2000 LEAD Technologies, Inc.
   All Rights Reserved.
*/

#ifndef _LTSYS_H_
#define _LTSYS_H_

#ifdef LINUX_DEFS
   #define FOR_LINUX
#elif defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined (_ALPHA_)
   #ifndef FOR_WIN32
      #define FOR_WIN32
   #endif                          /* FOR_WIN32 */
#endif

#endif

