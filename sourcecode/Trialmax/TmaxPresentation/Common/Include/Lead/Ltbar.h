/*************************************************************
   Ltbar.h - barcode module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTBAR_H)
#define LTBAR_H

#if !defined(L_LTBAR_API)
   #define L_LTBAR_API
#endif // #if !defined(L_LTBAR_API)

#include "Lttyp.h"
#include "Lterr.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

/****************************************************************
   Enums/defines/macros
****************************************************************/
/* Barcode Status */
#define BARCODE_NORMAL                    1
#define BARCODE_ERRORCHECK                2

/* Barcode Colors Flags */
#define BARCODE_USECOLORS                 0x0200   /* for R/W */
#define BARCODE_TRANSPARENT               0x0400   /* for W only */

#define BARCODE_DM_FORCE_INVERT           0x00000800   /* for DM/R */
#define BARCODE_DM_FASTFIND_DISABLE       0x00001000   /* for DM/R */

#define BARCODE_RETURN_FOUR_POINTS        0x00002000   /* for R only */

/* Linear Barcode Flags */
#define BARCODE_JUSTIFYRIGHT              0x0010   /* For W only */
#define BARCODE_JUSTIFYHCENTER            0x0020   /* For W only */
#define BARCODE_RETURNCHECK               0x1000   /* for R only */

/* Barcode Read/Write Flags for 1D */
#define BARCODE_MARKERS                   0x0001
#define BARCODE_BLOCK_SEARCH              0x0002

#if defined(LEADTOOLS_V16_OR_LATER)
/* Barcode Write Flags for 1D */
#define BARCODE_1D_USE_XMOD               0x0100
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

/* Barcode Write Flags for CodeOne and PDF417 */
#define BARCODE_TRUNCATE                  0x0010
#define BARCODE_MSGAPPEND                 0x0020
#define BARCODE_INITREADER                0x0040
#define BARCODE_COLROWASLIMITS            0x0080
#define BARCODE_DISABLE_COMPRESSOPN       0x0100
#define BARCODE_MICRO_PDF417_MODE128      0x1000
#define BARCODE_MICRO_PDF417_LINKED       0x2000

/* Units of measurment in an image */
#define BARCODE_SCANLINES_PER_PIXELS      0  /* Use scanlines/pixels */
#define BARCODE_INCHES                    1  /* Use Inches, you must multiply one inch by 100 */
#define BARCODE_MILLIMETERS               2  /* Use Millimeters */

/* Barcode Direction, used for Read 1D and PDF417 */
#define BARCODE_DIR_LEFT_TO_RIGHT         0x001
#define BARCODE_DIR_RIGHT_TO_LEFT         0x002
#define BARCODE_DIR_TOP_TO_BOTTOM         0x004
#define BARCODE_DIR_BOTTOM_TO_TOP         0x008

#define BARCODE_DIR_SKEW                  0x010
#define BARCODE_DIR_HORIZONTAL            0x020
#define BARCODE_DIR_VERTICAL              0x040
#define BARCODE_DIR_DIAGONAL              0x080

/* Barcode Justification for Write CodeOne and PDF417 */
#define BARCODE_JUSTIFY_RIGHT             0x1000
#define BARCODE_JUSTIFY_H_CENTER          0x2000
#define BARCODE_JUSTIFY_BOTTOM            0x4000
#define BARCODE_JUSTIFY_V_CENTER          0x8000

/* PDF417 ECC LEVEL Constants */
#define BARCODE_PDF417_ECCLEVEL_0         0x0001
#define BARCODE_PDF417_ECCLEVEL_1         0x0002
#define BARCODE_PDF417_ECCLEVEL_2         0x0004
#define BARCODE_PDF417_ECCLEVEL_3         0x0008
#define BARCODE_PDF417_ECCLEVEL_4         0x0010
#define BARCODE_PDF417_ECCLEVEL_5         0x0020
#define BARCODE_PDF417_ECCLEVEL_6         0x0040
#define BARCODE_PDF417_ECCLEVEL_7         0x0080
#define BARCODE_PDF417_ECCLEVEL_8         0x0100
#define BARCODE_PDF417_ECCUSE_PERCENT     0x0FFF

/* PDF Read Flags */
#define BARCODE_RETURNCORRUPT                0x080 /* R / PDF417 */

#define BARCODE_PDF_READ_MODE_0              0x0000
#define BARCODE_PDF_READ_MODE_1              0x1000
#define BARCODE_PDF_READ_MODE_2              0x4000
#define BARCODE_PDF_READ_MODE_3_BASIC        0x5000
#define BARCODE_PDF_READ_MODE_3_EXTENDED     0x8000
#define BARCODE_MICRO_PDF_READ_BASIC         0x9000
#define BARCODE_MICRO_PDF_READ_EXTENDED      0xC000

#define BARCODE_PDF_READ_MACRO_OPTION_0      0x00010000
#define BARCODE_PDF_READ_MACRO_OPTION_1      0x00020000
#define BARCODE_PDF_READ_MACRO_OPTION_2      0x00040000
#define BARCODE_PDF_READ_MACRO_OPTION_3      0x00080000
#define BARCODE_PDF_READ_MACRO_OPTION_4      0x00100000
#define BARCODE_PDF_READ_MACRO_OPTION_5      0x00200000
#define BARCODE_PDF_READ_MACRO_OPTION_6      0x00400000
#define BARCODE_PDF_READ_MACRO_OPTION_79AZ   0x00800000

#define BARCODE_PDF_READ_RETURN_PARTIAL      0x01000000 /* R / PDF417 */
#define BARCODE_PDF_FASTREAD                 0x02000000
/* Barcode (1D) Read/Write Types */
#define BARCODE_1D_EAN_13                 0x80000001
#define BARCODE_1D_EAN_8                  0x80000002
#define BARCODE_1D_UPC_A                  0x80000004
#define BARCODE_1D_UPC_E                  0x80000008
#define BARCODE_1D_CODE_3_OF_9            0x80000010
#define BARCODE_1D_CODE_128               0x80000020
#define BARCODE_1D_CODE_I2_OF_5           0x80000040
#define BARCODE_1D_CODA_BAR               0x80000080
#define BARCODE_1D_UCCEAN_128             0x80000100
#define BARCODE_1D_CODE_93                0x80000200
#define BARCODE_1D_EANEXT_5	            0x80000400
#define BARCODE_1D_EANEXT_2               0x80000800
#define BARCODE_1D_MSI                    0x80001000
#define BARCODE_1D_CODE11                 0x80002000
#define BARCODE_1D_CODE_S25               0x80004000
#define BARCODE_1D_RSS14                  0x80008000
#define BARCODE_1D_RSS14_LIMITED          0x80010000
#define BARCODE_1D_RSS14_EXPANDED         0x80020000
#define BARCODE_1D_PATCH_CODE             0x04000001
#define BARCODE_1D_POST_NET               0x04000002
#define BARCODE_1D_PLANET                 0x04000004
#define BARCODE_1D_AUST_POST              0x04000008
#define BARCODE_1D_RM4SCC                 0x04000010
#define BARCODE_1D_RSS14_STACKED          0x04000020
#define BARCODE_1D_RSS14_EXP_STACKED      0x04000040
#define BARCODE_1D_USPS4BC                0x04000080

/* The following define used only for read any bar code (1D) type in the image */
#define BARCODE_1D_READ_ANYTYPE           0x8003FFFF
#define BARCODE_1D_READ_ANYTYPE_NO_RSS14  0x80007FFF

/*the following defines used to determine the MSI type for read and write*/
#define BARCODE_1D_TYPE_MSI_MOD10	      0x00000000
#define BARCODE_1D_TYPE_MSI_2MOD10        0x00000001
#define BARCODE_1D_TYPE_MSI_MOD11	      0x00000002
#define BARCODE_1D_TYPE_MSI_MOD11MOD10    0x00000003

/*the following defines used to determine the CODE11 type for read and write*/
#define BARCODE_1D_TYPE_CODE11_C	         0x00000000
#define BARCODE_1D_TYPE_CODE11_K          0x00000010

/*the following defines used to determine the speed of barcode read engine*/
#define BARCODE_1D_FAST	           0x00000000
#define BARCODE_1D_NORMAL          0x00000100

/*the following define used to set the linkage flag value to 1 for all RSS14 types write*/
#define BARCODE_1D_TYPE_RSS14_LINKAGE     0x00000001

/*the following define used to determine the type of RSS14 stacked write*/
#define BARCODE_1D_TYPE_RSS14_OMNI        0x00000002

/*the following defines used to determine the number of rows for RSS14 expanded stacked write*/
#define BARCODE_1D_TYPE_RSS14_EXP_1       0x00000004
#define BARCODE_1D_TYPE_RSS14_EXP_2       0x00000008
#define BARCODE_1D_TYPE_RSS14_EXP_3       0x0000000C
#define BARCODE_1D_TYPE_RSS14_EXP_4       0x00000010
#define BARCODE_1D_TYPE_RSS14_EXP_5       0x00000014
#define BARCODE_1D_TYPE_RSS14_EXP_6       0x00000018

/*the following defines used to determine the Australian Post type*/
#define BARCODE_AUS4STATE_CIF_C      	   0x00000000
#define BARCODE_AUS4STATE_CIF_N           0x00000040
#define BARCODE_AUS4STATE_CIF_S           0x00000080

/*the following define used to write the truncated version of RSS14*/
#define BARCODE_1D_TYPE_RSS14_TRUNCATED   0x00000100

/* Barcode CodeOne Read Types */
#define BARCODE_R_CODEONE_A_TO_H          0x40000200
#define BARCODE_R_CODEONE_T               0x40000400
#define BARCODE_R_CODEONE_S               0x40000800
#define BARCODE_R_CODEONE_ANYTYPE         0x40001000

/* Barcode PDF417 Read/Write Type */
#define BARCODE_PDF417                    0x20002000
/* Barcode MicroPDF417 Read/Write Type */
#define BARCODE_MICRO_PDF417              0x20000001

/* Code One Write/Return Read Type Sub Types */
#define BARCODE_CODEONE_DEF               0x40000001
#define BARCODE_CODEONE_TDEF              0x40000002
#define BARCODE_CODEONE_SDEF              0x40000004
#define BARCODE_CODEONE_A                 0x40000008
#define BARCODE_CODEONE_B                 0x40000010
#define BARCODE_CODEONE_C                 0x40000020
#define BARCODE_CODEONE_D                 0x40000040
#define BARCODE_CODEONE_E                 0x40000080
#define BARCODE_CODEONE_F                 0x40000100
#define BARCODE_CODEONE_G                 0x40000200
#define BARCODE_CODEONE_H                 0x40000400
#define BARCODE_CODEONE_T16               0x40000800
#define BARCODE_CODEONE_T32               0x40001000
#define BARCODE_CODEONE_T48               0x40002000
#define BARCODE_CODEONE_S10               0x40004000
#define BARCODE_CODEONE_S20               0x40008000
#define BARCODE_CODEONE_S30               0x40010000

/* Barcodes Major Types */
#define BARCODES_1D                       0x0001
#define BARCODES_2D_READ                  0x0002
#define BARCODES_2D_WRITE                 0x0004
#define BARCODES_PDF_READ                 0x0008
#define BARCODES_PDF_WRITE                0x0010
#define BARCODES_DATAMATRIX_READ          0x0020
#define BARCODES_DATAMATRIX_WRITE         0x0040
#define BARCODES_QR_READ                  0x0080
#define BARCODES_QR_WRITE                 0x0100
#define BARCODES_MICRO_PDF_READ           0x1000
#define BARCODES_MICRO_PDF_WRITE          0x2000

/* DataMatrix Write / Return Read Type Sub Types */
#define BARCODE_DM_DEF                    0x10000101 /* Use Default DataMatrix Size */
#define BARCODE_DM_10x10                  0x10000102
#define BARCODE_DM_12x12                  0x10000103
#define BARCODE_DM_14x14                  0x10000104
#define BARCODE_DM_16x16                  0x10000105
#define BARCODE_DM_18x18                  0x10000106
#define BARCODE_DM_20x20                  0x10000107
#define BARCODE_DM_22x22                  0x10000108
#define BARCODE_DM_24x24                  0x10000109
#define BARCODE_DM_26x26                  0x1000010A
#define BARCODE_DM_32x32                  0x1000010B
#define BARCODE_DM_36x36                  0x1000010C
#define BARCODE_DM_40x40                  0x1000010D
#define BARCODE_DM_44x44                  0x1000010E
#define BARCODE_DM_48x48                  0x1000010F
#define BARCODE_DM_52x52                  0x10000110
#define BARCODE_DM_64x64                  0x10000111
#define BARCODE_DM_72x72                  0x10000112
#define BARCODE_DM_80x80                  0x10000113
#define BARCODE_DM_88x88                  0x10000114
#define BARCODE_DM_96x96                  0x10000115
#define BARCODE_DM_104x104                0x10000116
#define BARCODE_DM_120x120                0x10000117
#define BARCODE_DM_132x132                0x10000118
#define BARCODE_DM_144x144                0x10000119
#define BARCODE_DM_8x18                   0x1000011A
#define BARCODE_DM_8x32                   0x1000011B
#define BARCODE_DM_12x26                  0x1000011C
#define BARCODE_DM_12x36                  0x1000011D
#define BARCODE_DM_16x36                  0x1000011E
#define BARCODE_DM_16x48                  0x1000011F
#define BARCODE_DM_WRITE_RECTANGLE        0x10000180 /* Write Rectangular Default Symbol */

/* DataMatrix Read General Types */
#define BARCODE_DM_READ_SQUARE            0x10000001
#define BARCODE_DM_READ_RECTANGLE         0x10000002
#define BARCODE_DM_READ_SMALL             0x10000004

/* QR Write / Return Read Sub Types */
#define BARCODE_QR_DEF                    0x08000300
#define BARCODE_QR_M2_1                   0x08000301
#define BARCODE_QR_M2_2                   0x08000302
#define BARCODE_QR_M2_3                   0x08000303
#define BARCODE_QR_M2_4                   0x08000304
#define BARCODE_QR_M2_5                   0x08000305
#define BARCODE_QR_M2_6                   0x08000306
#define BARCODE_QR_M2_7                   0x08000307
#define BARCODE_QR_M2_8                   0x08000308
#define BARCODE_QR_M2_9                   0x08000309
#define BARCODE_QR_M2_10                  0x0800030A
#define BARCODE_QR_M2_11                  0x0800030B
#define BARCODE_QR_M2_12                  0x0800030C
#define BARCODE_QR_M2_13                  0x0800030D
#define BARCODE_QR_M2_14                  0x0800030E
#define BARCODE_QR_M2_15                  0x0800030F
#define BARCODE_QR_M2_16                  0x08000310
#define BARCODE_QR_M2_17                  0x08000311
#define BARCODE_QR_M2_18                  0x08000312
#define BARCODE_QR_M2_19                  0x08000313
#define BARCODE_QR_M2_20                  0x08000314
#define BARCODE_QR_M2_21                  0x08000315
#define BARCODE_QR_M2_22                  0x08000316
#define BARCODE_QR_M2_23                  0x08000317
#define BARCODE_QR_M2_24                  0x08000318
#define BARCODE_QR_M2_25                  0x08000319
#define BARCODE_QR_M2_26                  0x0800031A
#define BARCODE_QR_M2_27                  0x0800031B
#define BARCODE_QR_M2_28                  0x0800031C
#define BARCODE_QR_M2_29                  0x0800031D
#define BARCODE_QR_M2_30                  0x0800031E
#define BARCODE_QR_M2_31                  0x0800031F
#define BARCODE_QR_M2_32                  0x08000320
#define BARCODE_QR_M2_33                  0x08000321
#define BARCODE_QR_M2_34                  0x08000322
#define BARCODE_QR_M2_35                  0x08000323
#define BARCODE_QR_M2_36                  0x08000324
#define BARCODE_QR_M2_37                  0x08000325
#define BARCODE_QR_M2_38                  0x08000326
#define BARCODE_QR_M2_39                  0x08000327
#define BARCODE_QR_M2_40                  0x08000328
#define BARCODE_QR_M1_1                   0x08000329
#define BARCODE_QR_M1_2                   0x0800032A
#define BARCODE_QR_M1_3                   0x0800032B
#define BARCODE_QR_M1_4                   0x0800032C
#define BARCODE_QR_M1_5                   0x0800032D
#define BARCODE_QR_M1_6                   0x0800032E
#define BARCODE_QR_M1_7                   0x0800032F
#define BARCODE_QR_M1_8                   0x08000330
#define BARCODE_QR_M1_9                   0x08000331
#define BARCODE_QR_M1_10                  0x08000332
#define BARCODE_QR_M1_11                  0x08000333
#define BARCODE_QR_M1_12                  0x08000334
#define BARCODE_QR_M1_13                  0x08000335
#define BARCODE_QR_M1_14                  0x08000336
#define BARCODE_QR_M1_DEF                 0x08000337

/* QR Read Type */
#define BARCODE_QR_CODE                   0x08000001

/* QR ECC LEVEL Constants */
#define BARCODE_QR_ECC_L                  0
#define BARCODE_QR_ECC_M                  1
#define BARCODE_QR_ECC_Q                  2
#define BARCODE_QR_ECC_H                  3

/****************************************************************
   Classes/structures
****************************************************************/

typedef struct _tagBarCodeData
{
   L_UINT            uStructSize;
   L_INT             nGroup;              // used only for CodeOne and PDF417
   L_UINT32          ulType;
   L_INT             nUnits;
   RECT              rcBarLocation;
   L_INT             nSizeofBarCodeData;
   L_CHAR *          pszBarCodeData;
   L_INT             nIndexDuplicate;     // for reading only
   L_INT             nTotalCount;         // for reading only
   L_INT             nDupCount;           // for reading only
   L_UINT            uFlags;
   L_UINT            uDataCode;
#if defined (LEADTOOLS_V16_OR_LATER)
   L_INT             nAngle;
#endif //#if defined (LEADTOOLS_V16_OR_LATER)
} BARCODEDATA, * pBARCODEDATA;

typedef struct _tagBarCodeColor
{
   L_UINT      uStructSize;
   DWORD       dwColorBar;
   DWORD       dwColorSpace;
} BARCODECOLOR, * pBARCODECOLOR;

typedef struct _tagBarCode1D
{
   L_UINT      uStructSize;
   L_BOOL      bOutShowText;              // for writing only
   L_INT       nDirection;                // for reading only
   L_BOOL      bErrorCheck;
   L_INT       nGranularity;
   L_INT       nMinLength;
   L_INT       nMaxLength;
   L_INT       nWhiteLines;
   L_UINT      uStd1DFlags;
   L_UINT      uAdvancedFlags; 
#if defined(LEADTOOLS_V16_OR_LATER)
   L_INT       nXModule;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
} BARCODE1D, * pBARCODE1D;

typedef struct _tagBarCodeReadPDF
{
   L_UINT      uStructSize;
   L_INT       nDirection;
} BARCODEREADPDF, * pBARCODEREADPDF;

typedef struct _tagBarCodeWriteCodeOne
{
   L_UINT      uStructSize;
   L_INT       nElementX;
   L_INT       nElementY;
   L_INT       nJustify;
} BARCODEWRITECODEONE, * pBARCODEWRITECODEONE;

typedef struct _tagBarCodeWritePDF
{
   L_UINT      uStructSize;
   L_UINT16    wEccPerc;
   L_UINT16    wEccLevel;
   L_UINT16    wAspectHeight;
   L_UINT16    wAspectWidth;
   L_UINT16    wModAspectRatio;
   L_UINT16    wColumns;
   L_UINT16    wRows;
   L_UINT16    wModule;
   L_INT       nJustify;
} BARCODEWRITEPDF, * pBARCODEWRITEPDF;

typedef struct _tagBarCodeVersion
{
   L_UINT      uStructSize;
   L_UCHAR     Product[60];
   L_INT       MajorNumber;
   L_INT       MinorNumber;
   L_UCHAR     Date[16];
   L_UCHAR     Time[16];
} BARCODEVERSION, * pBARCODEVERSION;

typedef struct _tagBARCODEWRITEDM
{
   L_UINT      uStructSize;
   L_UINT32    ulFlags;
   L_CHAR      cGroupNumber;
   L_CHAR      cGroupTotal;
   L_UCHAR     cFileIDLo;
   L_UCHAR     cFileIDHi;
   L_INT       nXModule;
} BARCODEWRITEDM, * pBARCODEWRITEDM;

typedef struct _tagBARCODEWRITEQR
{
   L_UINT      uStructSize;
   L_UINT32    ulFlags;
   L_INT       nGroupNumber;
   L_INT       nGroupTotal;
   L_INT       nEccLevel;
   L_INT       nXModule;
} BARCODEWRITEQR, * pBARCODEWRITEQR;

/****************************************************************
   Function prototypes
****************************************************************/

L_LTBAR_API L_INT EXT_FUNCTION L_BarCodeRead(pBITMAPHANDLE   pBitmap,
                                RECT *          prcSearch,
                                L_UINT32        ulSearchType,
                                L_INT           nUnits,
                                L_UINT32        ulFlags,
                                L_INT           nMultipleMaxCount,
                                pBARCODE1D      pBarCode1D,
                                pBARCODEREADPDF pBarCodePDF,
                                pBARCODECOLOR   pBarCodeColor,
                                pBARCODEDATA *  ppBarCodeData,
                                L_UINT          uStructSize);

L_LTBAR_API L_INT EXT_FUNCTION L_BarCodeWrite(pBITMAPHANDLE     pBitmap,
                                 pBARCODEDATA      pBarCodeData,
                                 pBARCODECOLOR     pBarCodeColor,
                                 L_UINT32          ulFlags,
                                 pBARCODE1D        pBarCode1D,
                                 pBARCODEWRITEPDF  pBarCodePDF,
                                 pBARCODEWRITEDM   pBarCodeDM,
                                 pBARCODEWRITEQR   pBarCodeQR,
                                 LPRECT            lprcSize);

L_LTBAR_API L_VOID EXT_FUNCTION L_BarCodeFree(pBARCODEDATA * ppBarCodeData);

L_LTBAR_API L_BOOL EXT_FUNCTION L_BarCodeIsDuplicated(pBARCODEDATA pBarCodeDataItem);

L_LTBAR_API L_INT EXT_FUNCTION L_BarCodeGetDuplicated(pBARCODEDATA pBarCodeDataItem);

L_LTBAR_API L_INT EXT_FUNCTION L_BarCodeGetFirstDuplicated(pBARCODEDATA pBarCodeData, L_INT nIndex);

L_LTBAR_API L_INT EXT_FUNCTION L_BarCodeGetNextDuplicated(pBARCODEDATA pBarCodeData, L_INT nCurIndex);

L_LTBAR_API L_INT EXT_FUNCTION L_BarCodeInit (L_INT nMajorType);

L_LTBAR_API L_VOID EXT_FUNCTION L_BarCodeExit (L_VOID);

L_LTBAR_API L_INT EXT_FUNCTION L_BarCodeVersionInfo(pBARCODEVERSION pBarCodeVersion, L_UINT uStructSize);

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTBAR_H)