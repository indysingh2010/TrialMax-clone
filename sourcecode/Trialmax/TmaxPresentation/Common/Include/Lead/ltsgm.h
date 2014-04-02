/*
   ltsgm.h - LEAD Segmentation Module header file
   Copyright (c) 1990-2007 by LEAD Technologies, Inc.
   All Rights Reserved.
*/

#ifndef __LTSEG_H__
#define __LTSEG_H__

#if !defined(L_LTSGM_API)
   #define L_LTSGM_API
#endif // #if !defined(L_LTSGM_API)

#include "ltfil.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

/* Segment Type */
#define SEGTYPE_TEXT_1BIT_BW        0x01
#define SEGTYPE_TEXT_1BIT_COLOR     0x02
#define SEGTYPE_TEXT_2BIT_COLOR     0x03
#define SEGTYPE_GRAYSCALE_2BIT      0x04
#define SEGTYPE_GRAYSCALE_8BIT      0x05
#define SEGTYPE_PICTURE             0x06
#define SEGTYPE_BACKGROUND          0x07
#define SEGTYPE_ONECOLOR            0x08
#define SEGTYPE_TEXT_2BITBW         0x09

/* Coder Type */
// Mask compression
#define MRC_MASK_COMPRESSION_JBIG                 0x00
#define MRC_MASK_COMPRESSION_FAX_G4               0x01
#define MRC_MASK_COMPRESSION_FAX_G3_1D            0x02
#define MRC_MASK_COMPRESSION_FAX_G3_2D            0x03
#define MRC_PDF_ONEBIT_COMPRESSION_ZIP            0x04
#define MRC_PDF_ONEBIT_COMPRESSION_LZW            0x05
#define MRC_PDF_ONEBIT_COMPRESSION_CCITT_G3_1D    0x06
#define MRC_PDF_ONEBIT_COMPRESSION_CCITT_G3_2D    0x07
#define MRC_PDF_ONEBIT_COMPRESSION_CCITT_G4       0x08
#define MRC_PDF_ONEBIT_COMPRESSION_JBIG2          0x09

// Grayscale 2 bit segment compression                         
#define MRC_GRAYSCALE_COMPRESSION_JBIG_2BIT              0x00

// Text 2 bit colored segment compression                         
#define MRC_TEXT_COMPRESSION_JBIG_2BIT             0x00
#define MRC_TEXT_COMPRESSION_GIF_2BIT              0x01
#define MRC_PDF_TEXT_COMPRESSION_ZIP               0x02
#define MRC_PDF_TEXT_COMPRESSION_LZW               0x03

// Grayscale 8 bit segment compression                         
#define MRC_GRAYSCALE_COMPRESSION_LOSSLESS_CMW_8BIT         0x00
#define MRC_GRAYSCALE_COMPRESSION_GRAYSCALE_CMW_8BIT        0x01
#define MRC_GRAYSCALE_COMPRESSION_GRAYSCALE_CMP_8BIT        0x02
#define MRC_GRAYSCALE_COMPRESSION_LOSSLESS_JPEG_8BIT        0x03
#define MRC_GRAYSCALE_COMPRESSION_GRAYSCALE_JPEG_8BIT       0x04
#define MRC_GRAYSCALE_COMPRESSION_JPEG_PROGRESSIVE          0x05


// Picture segment compression                           
#define MRC_PICTURE_COMPRESSION_CMW                      0x00
#define MRC_PICTURE_COMPRESSION_LOSSLESS_CMW             0x01
#define MRC_PICTURE_COMPRESSION_CMP                      0x02
#define MRC_PICTURE_COMPRESSION_JPEG                     0x03
#define MRC_PICTURE_COMPRESSION_LOSSLESS_JPEG            0x04
#define MRC_PICTURE_COMPRESSION_JPEG_YUV422              0x05
#define MRC_PICTURE_COMPRESSION_JPEG_YUV411              0x06
#define MRC_PICTURE_COMPRESSION_JPEG_PROGRESSIVE         0x07
#define MRC_PICTURE_COMPRESSION_JPEG_PROGRESSIVE_YUV422  0x08
#define MRC_PICTURE_COMPRESSION_JPEG_PROGRESSIVE_YUV411  0x09
#define MRC_PDF_PICTURE_COMPRESSION_JPEG                 0x0A
#define MRC_PDF_PICTURE_COMPRESSION_YUV422               0x0B
#define MRC_PDF_PICTURE_COMPRESSION_YUV411               0x0C
#define MRC_PDF_PICTURE_COMPRESSION_PROGRESSIVE          0x0D
#define MRC_PDF_PICTURE_COMPRESSION_PROGRESSIVE_YUV422   0x0E
#define MRC_PDF_PICTURE_COMPRESSION_PROGRESSIVE_YUV411   0x0F
#define MRC_PDF_PICTURE_COMPRESSION_ZIP                  0x10
#define MRC_PDF_PICTURE_COMPRESSION_LZW                  0x11

/* segmentation handle */
#define HSEGMENTATION     HANDLE
#define pHSEGMENTATION    HSEGMENTATION *

/* Combination flags */
#define COMBINE_FORCE         0x00
#define COMBINE_FORCESIMILAR  0x01
#define COMBINE_TRY           0x02

#define SGM_FAVOR_ONEBIT   0x0000
#define SGM_FAVOR_TWOBIT   0x0001
#define SGM_FORCE_ONEBIT   0x0002
#define SGM_FORCE_TWOBIT   0x0003

#define SGM_WITHBKGRND      0x0000
#define SGM_WITHOUTBKGRND   0x0010

#if defined (LEADTOOLS_V16_OR_LATER)
#define SGM_NORMAL_SEGMENTATION                    0x0000
#define SGM_ADVANCED_FEATURE_BASED_SEGMENTATION    0x0100
#endif //LEADTOOLS_V16_OR_LATER

/* Images types */
#define IMAGETYPE_SCANNED            0x00
#define IMAGETYPE_COMPUTERGENERATED  0x01

#define L_MrcUpdateSegmentData         L_MrcSetSegmentData   
#define L_MrcEndBitmapSegmentation     L_MrcStopBitmapSegmentation

/* define segment info structure */
typedef struct _SEGMENTDATA
{
   L_UINT   uStructSize;
   RECT     rcBitmapSeg;
   L_UINT   uType;
}
SEGMENTDATA,  * pSEGMENTDATA;

typedef struct _tagSEGMENTEXTOPTIONS
{
   L_UINT   uStructSize;
   L_UINT   uCleanSize;
   L_UINT   uSegmentQuality;
   L_UINT   uColorThreshold;
   L_UINT   uBackGroundThreshold;
   L_UINT   uCombineThreshold;
   L_UINT   uFlags;
} SEGMENTEXTOPTIONS, * pSEGMENTEXTOPTIONS;

typedef struct _COMPRESSIONOPTIONS
{
   L_UINT   uStructSize;
   L_INT    nMaskCoder;
   L_INT    nPictureCoder;
   L_INT    nPictureQFactor;
   L_INT    nGrayscale2BitCoder;
   L_INT    nGrayscale8BitCoder;
   L_INT    nGrayscale8BitFactor;
   L_INT    nText2BitCoder;
}COMPRESSIONOPTIONS, *pCOMPRESSIONOPTIONS;



/*------------Callbacks-------------------------------------------------------------*/

typedef L_INT (pEXT_CALLBACK pMRCENUMSEGMENTSPROC)(HSEGMENTATION        hSegment, 
                                                   const pSEGMENTDATA   pSegment,
                                                   L_INT                nSegId,
                                                   L_VOID *        pUserData);

/*------------Bitmap Segmentation -------------------------------------------------*/

#if !defined(FOR_MANAGED) || defined(FOR_MANAGED_MRC)
// Initialize segmentation handle
L_LTSGM_API L_INT EXT_FUNCTION L_MrcStartBitmapSegmentation(pHSEGMENTATION phSegment,
                                                pBITMAPHANDLE  pBitmap,
                                                COLORREF       clrBackground,
                                                COLORREF       clrForeground);
// Free segmentation handle
L_LTSGM_API L_INT EXT_FUNCTION L_MrcStopBitmapSegmentation(HSEGMENTATION hSegment);


// Break a bitmap into segments in other way
L_LTSGM_API L_INT EXT_FUNCTION L_MrcSegmentBitmap(HSEGMENTATION              hSegment,
                                     pBITMAPHANDLE              pBitmap,
                                     pSEGMENTEXTOPTIONS         pSegOption);

// Set a new segment in the segmentation handle 
L_LTSGM_API L_INT EXT_FUNCTION L_MrcCreateNewSegment(HSEGMENTATION   hSegment,
                                         pBITMAPHANDLE   pBitmap,
                                         pSEGMENTDATA    pSegment);

// Get all segments stored inside a segmentation handle
L_LTSGM_API L_INT EXT_FUNCTION L_MrcEnumSegments(HSEGMENTATION              hSegment,
                                     pMRCENUMSEGMENTSPROC      pEnumProc,
                                     L_HANDLE                  pUserData,
                                     L_UINT32                  dwFlags);

// Update a certain segment in the segmentation handle 
L_LTSGM_API L_INT EXT_FUNCTION L_MrcSetSegmentData(HSEGMENTATION  hSegment,
                                       pBITMAPHANDLE  pBitmap, 
                                       L_INT          nSegId,
                                       pSEGMENTDATA   pSegmentData);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcDeleteSegment(HSEGMENTATION  hSegment,
                                      L_INT          nSegId);


L_LTSGM_API L_INT EXT_FUNCTION L_MrcCombineSegments(HSEGMENTATION  hSegment,
                                        L_INT          nSegId1,
                                        L_INT          nSegId2,
                                        L_UINT16       uCombineFlags,
                                        L_UINT16       uCombineFactor);

/*------------Copy segmentation handle ---------------------------------------------*/
L_LTSGM_API L_INT EXT_FUNCTION L_MrcCopySegmentationHandle(pHSEGMENTATION phSegmentDst,
                                               HSEGMENTATION  hSegmentSrc);

/*------------Save a file as MRC ---------------------------------------------------*/

L_LTSGM_API L_INT EXT_FUNCTION L_MrcSaveBitmapA(HSEGMENTATION         hSegment,
                                  pBITMAPHANDLE         pBitmap,
                                  pCOMPRESSIONOPTIONS   pCmpOption,
                                  L_CHAR*              pszFileName,
                                  FILESAVECALLBACK      pfnCallback,
                                  L_HANDLE              pUserData,
                                  L_INT                 nFormat,
                                  pSAVEFILEOPTION       pSaveOptions);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcSaveBitmapT44A(
                                      HSEGMENTATION           hSegment,
                                      pBITMAPHANDLE           pBitmap,
                                      pCOMPRESSIONOPTIONS     pCmpOption,
                                      L_CHAR*                pszFileName,
                                      FILESAVECALLBACK        pfnCallback,
                                      L_HANDLE                pUserData,
                                      L_INT                   nFormat,
                                      pSAVEFILEOPTION         pSaveOptions);
#if defined(FOR_UNICODE)
L_LTSGM_API L_INT EXT_FUNCTION L_MrcSaveBitmap(HSEGMENTATION         hSegment,
                                  pBITMAPHANDLE         pBitmap,
                                  pCOMPRESSIONOPTIONS   pCmpOption,
                                  L_TCHAR*              pszFileName,
                                  FILESAVECALLBACK      pfnCallback,
                                  L_HANDLE              pUserData,
                                  L_INT                 nFormat,
                                  pSAVEFILEOPTION       pSaveOptions);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcSaveBitmapT44(
                                      HSEGMENTATION           hSegment,
                                      pBITMAPHANDLE           pBitmap,
                                      pCOMPRESSIONOPTIONS     pCmpOption,
                                      L_TCHAR*                pszFileName,
                                      FILESAVECALLBACK        pfnCallback,
                                      L_HANDLE                pUserData,
                                      L_INT                   nFormat,
                                      pSAVEFILEOPTION         pSaveOptions);
#else
#define L_MrcSaveBitmap L_MrcSaveBitmapA
#define L_MrcSaveBitmapT44 L_MrcSaveBitmapT44A
#endif // #if defined(FOR_UNICODE)
/*------------Load an MRC file -----------------------------------------------------*/
L_LTSGM_API L_INT EXT_FUNCTION L_MrcLoadBitmapA(
                                   L_CHAR*          pszFileName,
                                   pBITMAPHANDLE     pBitmap,
                                   L_UINT            uStructSize,
                                   L_INT             nPageNo,
                                   FILEREADCALLBACK  pfnCallback,
                                   L_HANDLE          pUserData);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcGetPagesCountA( L_CHAR* pszFileName,
                                       L_INT* pnPages);

#if defined(FOR_UNICODE)
L_LTSGM_API L_INT EXT_FUNCTION L_MrcLoadBitmap(
                                   L_TCHAR*          pszFileName,
                                   pBITMAPHANDLE     pBitmap,
                                   L_UINT            uStructSize,
                                   L_INT             nPageNo,
                                   FILEREADCALLBACK  pfnCallback,
                                   L_HANDLE          pUserData);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcGetPagesCount( L_TCHAR* pszFileName,
                                       L_INT* pnPages);
#else
#define L_MrcLoadBitmap L_MrcLoadBitmapA
#define L_MrcGetPagesCount L_MrcGetPagesCountA
#endif // #if defined(FOR_UNICODE)

/*------------Load/Save segments to/from files -------------------------------------*/
L_LTSGM_API L_INT EXT_FUNCTION L_MrcSaveSegmentationA(HSEGMENTATION hSegment,
                                     L_CHAR* pszFileName);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcLoadSegmentationA(pHSEGMENTATION phSegment,
                                     pBITMAPHANDLE  pBitmap,
                                     L_CHAR* pszFileName);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcSaveBitmapListA(
                                       HSEGMENTATION  * hSegment,
                                       L_UINT                uhSegmentCount,
                                       HBITMAPLIST           hList,
                                       pCOMPRESSIONOPTIONS   pCmpOption,
                                       L_CHAR*              pszFileName,
                                       L_INT                 nFormat);

#if defined(FOR_UNICODE)
L_LTSGM_API L_INT EXT_FUNCTION L_MrcSaveSegmentation(HSEGMENTATION hSegment,
                                     L_TCHAR* pszFileName);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcLoadSegmentation(pHSEGMENTATION phSegment,
                                     pBITMAPHANDLE  pBitmap,
                                     L_TCHAR* pszFileName);

L_LTSGM_API L_INT EXT_FUNCTION L_MrcSaveBitmapList(
									            HSEGMENTATION  * hSegment,
                                       L_UINT                uhSegmentCount,
                                       HBITMAPLIST           hList,
                                       pCOMPRESSIONOPTIONS   pCmpOption,
                                       L_TCHAR*              pszFileName,
                                       L_INT                 nFormat);
#else
#define L_MrcSaveSegmentation L_MrcSaveSegmentationA
#define L_MrcLoadSegmentation L_MrcLoadSegmentationA
#define L_MrcSaveBitmapList   L_MrcSaveBitmapListA
#endif // #if defined(FOR_UNICODE)

#endif // #if !defined(FOR_MANAGED) || defined(FOR_MANAGED_MRC)
#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif //__LTSEG_H__
