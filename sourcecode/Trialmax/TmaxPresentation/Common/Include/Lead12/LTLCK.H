/*
   LTLCK.H - support unlock key defines
   Copyright (c) 1991-2000 LEAD Technologies, Inc.
   All Rights Reserved.
*/

#ifndef _LTLCK_H_
#define _LTLCK_H_

#include "ltkey.h"

#define UNLOCKSUPPORT() \
{ \
   L_UnlockSupport(L_SUPPORT_DOCUMENT, L_KEY_DOCUMENT); \
   L_UnlockSupport(L_SUPPORT_GIFLZW, L_KEY_GIFLZW); \
   L_UnlockSupport(L_SUPPORT_TIFLZW, L_KEY_TIFLZW); \
   L_UnlockSupport(L_SUPPORT_FPXEXTENSIONS, L_KEY_FPXEXTENSIONS); \
   L_UnlockSupport(L_SUPPORT_OCR, L_KEY_OCR); \
   L_UnlockSupport(L_SUPPORT_MULTIMEDIA, L_KEY_MULTIMEDIA); \
   L_UnlockSupport(L_SUPPORT_MEDICAL, L_KEY_MEDICAL); \
   L_UnlockSupport(L_SUPPORT_MEDICAL_NET, L_KEY_MEDICAL_NET); \
   L_UnlockSupport(L_SUPPORT_JBIG, L_KEY_JBIG); \
   L_UnlockSupport(L_SUPPORT_VECTOR, L_KEY_VECTOR); \
   L_UnlockSupport(L_SUPPORT_BARCODES_1D, L_KEY_BARCODES_1D); \
   L_UnlockSupport(L_SUPPORT_BARCODES_2D_READ, L_KEY_BARCODES_2D_READ); \
   L_UnlockSupport(L_SUPPORT_BARCODES_2D_WRITE, L_KEY_BARCODES_2D_WRITE); \
   L_UnlockSupport(L_SUPPORT_BARCODES_PDF_READ, L_KEY_BARCODES_PDF_READ); \
   L_UnlockSupport(L_SUPPORT_BARCODES_PDF_WRITE, L_KEY_BARCODES_PDF_WRITE); \
   L_UnlockSupport(L_SUPPORT_INTERNET, L_KEY_INTERNET); \
   L_UnlockSupport(L_SUPPORT_VECTOR, L_KEY_VECTOR); \
   L_UnlockSupport(L_SUPPORT_VECTOR_DWG, L_KEY_VECTOR_DWG); \
   L_UnlockSupport(L_SUPPORT_VECTOR_DXF, L_KEY_VECTOR_DXF); \
   L_UnlockSupport(L_SUPPORT_VECTOR_MISC, L_KEY_VECTOR_MISC); \
   L_UnlockSupport(L_SUPPORT_VECTOR_ALL, L_KEY_VECTOR_ALL); \
   L_UnlockSupport(L_SUPPORT_VECTOR_DWF, L_KEY_VECTOR_DWF); \
   L_UnlockSupport(L_SUPPORT_PDF, L_KEY_PDF);\
   L_UnlockSupport(L_SUPPORT_VECTOR_VIEW, L_KEY_VECTOR_VIEW);\
}

#endif                          /* _LTLCK_H_ */
