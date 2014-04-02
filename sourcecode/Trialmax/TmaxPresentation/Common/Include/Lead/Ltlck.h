/*************************************************************
   Ltlck.h - Support constants
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTCLK_H)
#define LTCLK_H

#define L_SUPPORT_DOCUMENT                      0
#define L_SUPPORT_RESERVED1                     1
#define L_SUPPORT_RESERVED2                     2
#define L_SUPPORT_OCR                           3
#define L_SUPPORT_MEDICAL                       4
#define L_SUPPORT_VECTOR                        5
#define L_SUPPORT_MEDICAL_NET                   6
#define L_SUPPORT_BARCODES_1D                   7
#define L_SUPPORT_BARCODES_2D_READ              8
#define L_SUPPORT_BARCODES_2D_WRITE             9
#define L_SUPPORT_BARCODES_PDF_READ             10
#define L_SUPPORT_BARCODES_PDF_WRITE            11
#define L_SUPPORT_PDF                           12
#define L_SUPPORT_J2K                           13
#define L_SUPPORT_CMW                           14
#define L_SUPPORT_DICOM                         15
#define L_SUPPORT_EXTGRAY                       16
#define L_SUPPORT_BITONAL                       17
#define L_SUPPORT_PDF_SAVE                      18
#define L_SUPPORT_OCR_PDF_OUTPUT                19
#define L_SUPPORT_BARCODES_DATAMATRIX_READ      20
#define L_SUPPORT_BARCODES_DATAMATRIX_WRITE     21
#define L_SUPPORT_LTPRO                         22
#define L_SUPPORT_OCR_ASIAN                     23
#define L_SUPPORT_ICR                           24
#define L_SUPPORT_OMR                           L_SUPPORT_LEAD_OMR
#define L_SUPPORT_ABC                           26
#define L_SUPPORT_BARCODES_QR_READ              27
#define L_SUPPORT_BARCODES_QR_WRITE             28
#define L_SUPPORT_BARCODES_1D_SILVER            29
#define L_SUPPORT_MOBILE                        30
#define L_SUPPORT_JBIG2                         31
#define L_SUPPORT_ABIC_READ                     32
#define L_SUPPORT_ABIC_SAVE                     33
#define L_SUPPORT_PDF_READ                      34
#define L_SUPPORT_PDF_ADVANCED                  35
#define L_SUPPORT_NITF                          36
#define L_SUPPORT_OCR_ARABIC                    37
// unlocks for v16
#define L_SUPPORT_START_V16_OR_LATER                  38
#define L_SUPPORT_JPIP                                38
#define L_SUPPORT_OCR_PLUS                            L_SUPPORT_OCR
#define L_SUPPORT_OCR_PLUS_PDF_OUTPUT                 L_SUPPORT_OCR_PDF_OUTPUT
#define L_SUPPORT_ICR_PLUS                            L_SUPPORT_ICR
#define L_SUPPORT_OMR_PLUS                            L_SUPPORT_OMR
#define L_SUPPORT_OCR_PLUS_PDF_LEAD_OUTPUT            39
#define L_SUPPORT_OCR_PROFESSIONAL                    40
#define L_SUPPORT_OCR_PROFESSIONAL_PDF_OUTPUT         41
#define L_SUPPORT_ICR_PROFESSIONAL                    42
#define L_SUPPORT_OMR_PROFESSIONAL                    L_SUPPORT_LEAD_OMR
#define L_SUPPORT_OCR_PROFESSIONAL_PDF_LEAD_OUTPUT    44
#define L_SUPPORT_OCR_PROFESSIONAL_ASIAN              L_SUPPORT_OCR_ASIAN
#define L_SUPPORT_OCR_ADVANTAGE                       45
#define L_SUPPORT_OCR_ADVANTAGE_PDF_LEAD_OUTPUT       46
#define L_SUPPORT_FORMS                               47
#define L_SUPPORT_MEDICAL_SERVER                      48
#define L_SUPPORT_PRINT_DRIVER                        49
#define L_SUPPORT_DOCUMENT_WRITERS                    50
#define L_SUPPORT_LEAD_OMR                            51
#define L_SUPPORT_MEDIA_WRITER                        52
#define L_SUPPORT_DOCUMENT_WRITERS_PDF                53
#define L_SUPPORT_MEDICAL_3D                          54
#define L_SUPPORT_OCR_ARABIC_PDF_LEAD_OUTPUT          55
#define L_SUPPORT_PRINT_DRIVER_SERVER                 56

#define L_SUPPORT_MAX                                 56


#define ISBARCODELOCKED() \
L_IsSupportLocked(L_SUPPORT_BARCODES_1D)                 && \
L_IsSupportLocked(L_SUPPORT_BARCODES_2D_READ)            && \
L_IsSupportLocked(L_SUPPORT_BARCODES_2D_WRITE)           && \
L_IsSupportLocked(L_SUPPORT_BARCODES_PDF_READ)           && \
L_IsSupportLocked(L_SUPPORT_BARCODES_PDF_WRITE)          && \
L_IsSupportLocked(L_SUPPORT_BARCODES_DATAMATRIX_READ)    && \
L_IsSupportLocked(L_SUPPORT_BARCODES_DATAMATRIX_WRITE)   && \
L_IsSupportLocked(L_SUPPORT_BARCODES_QR_READ)            && \
L_IsSupportLocked(L_SUPPORT_BARCODES_QR_WRITE)           && \
L_IsSupportLocked(L_SUPPORT_BARCODES_1D_SILVER)

#endif // #if !defined(LTCLK_H)
