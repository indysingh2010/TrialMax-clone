/*
   LTPCK.H - pack header file
   Copyright (c) 1991-2000 LEAD Technologies, Inc.
   All Rights Reserved.
*/

#ifndef FOR_LINUX

#ifdef _HEADER_ENTRY_

   #ifdef __cplusplus
      extern "C"
      {
   #endif

   #if defined(__BORLANDC__) && defined(FOR_WIN)
      #pragma option -a1
   #else
      #ifndef RC_INVOKED
         #ifdef __BORLANDC__
            #pragma warn -pck
         #else
            #pragma warning(disable: 4103)
         #endif
         #pragma pack(1)
      #endif                          /* RC_INVOKED */
   #endif                          /* __BORLANDC__ */

#elif defined(_HEADER_ENTRY_8_)

   #ifdef __cplusplus
      extern "C++"
      {
   #endif

   #if defined(__BORLANDC__) && defined(FOR_WIN)
      #pragma option -a8
   #else
      #ifndef RC_INVOKED
         #ifdef __BORLANDC__
            #pragma warn -pck
         #else
            #pragma warning(disable: 4103)
         #endif
         #pragma pack(8)
      #endif                          /* RC_INVOKED */
   #endif                          /* __BORLANDC__ */

#else

   #if defined(__BORLANDC__) && defined(FOR_WIN)
      #pragma option -a.
   #else
      #ifndef RC_INVOKED
         #pragma pack()
      #endif                          /* RC_INVOKED */
   #endif                          /* __BORLANDC__ */

   #ifdef __cplusplus
      } // extern "C"
   #endif

#endif

#endif   // FOR_LINUX
