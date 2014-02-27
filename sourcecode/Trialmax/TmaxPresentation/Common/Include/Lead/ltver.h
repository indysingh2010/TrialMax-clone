/*************************************************************
   Ltver.h - LEADTOOLS version definition
   Copyright (c) 1991-2008 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTVER_H)
#define LTVER_H

#if defined(LTV15_CONFIG)
#define LTVER_   1500
#define L_VER_DESIGNATOR
#elif defined(LTV16_CONFIG)
#define LTVER_   1600
#define L_VER_DESIGNATOR
#else
#error LEADTOOLS Vxx_CONFIG not found!
#endif // #if defined(LTV15_CONFIG)

#if LTVER_ >= 1600
#define LEADTOOLS_V16_OR_LATER
#endif

#endif // #if !defined(LTVER_H)
