/*************************************************************
   Ltpck.h - pack header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#pragma warning(disable: 4103)  // alignment changed after including header, may be due to missing #pragma pack(pop)

#if defined(L_HEADER_ENTRY)

   #if defined(__cplusplus)
      extern "C"
      {
   #endif // #if defined(__cplusplus)

   #if !defined(RC_INVOKED)
      #pragma pack(1)
   #endif // #if !defined(RC_INVOKED)

#elif defined(L_HEADER_ENTRY8_)

   #if defined(__cplusplus)
      extern "C++"
      {
   #endif // #if defined(__cplusplus)

   #if !defined(RC_INVOKED)
      #pragma pack(8)
   #endif // #if !defined(RC_INVOKED)

#else

   #if !defined(RC_INVOKED)
      #pragma pack()
   #endif // #if !defined(RC_INVOKED)

   #if defined(__cplusplus)
      } // extern "C"
   #endif // #if defined(__cplusplus)

#endif
