/*-------------------------------------------------------------------------------------------*
 *  File       | lpdfComp.h                                                                  *
 *  Company    | LEAD Technologies, Inc.                                                     *
 *-------------|-----------------------------------------------------------------------------*
 *  Programmer |                                                                             *
 *  EMail      |                                                                             *
 *  Date       | 5/16/2005                                                                   *
 *-------------------------------------------------------------------------------------------*/
#if !defined(__LCP_MRC_)
#define __LCP_MRC_

#if !defined(L_LCMRC_API)
   #define L_LCMRC_API
#endif // #if !defined(L_LCMRC_API)

#define L_HEADER_ENTRY
#include "ltpck.h"

/* flags */ 

/* ...flags for PDFCOMPRESSION */
#define  PDFCOMP_1BITCOMPTYPE_ENABLED           0x00000001      /* comp1Bit field is valid */ 
#define  PDFCOMP_2BITCOMPTYPE_ENABLED           0x00000002      /* comp2Bit field is valid */ 
#define  PDFCOMP_PICTURECOMPTYPE_ENABLED        0x00000004      /* compPicture field is valid */ 

/* ...flags for PDFCOMPOPTIONS */ 
#define  PDFCOMP_FAVOR_ONEBIT                   0x00000000      /* favor 1 bit segments while doing the segmentation */ 
#define  PDFCOMP_FAVOR_TWOBIT                   0x00000001      /* favor 2 bit segments while doing the segmentation */ 

#define  PDFCOMP_FORCE_ONEBIT                   0x00000002
#define  PDFCOMP_FORCE_TWOBIT                   0x00000003

#define  PDFCOMP_WITH_BACKGROUND                0x00000000      /* detect the background color of the image being segmented */ 
#define  PDFCOMP_WITHOUT_BACKGROUND             0x00000010

typedef HANDLE LCPDF_HANDLE;

typedef struct _SEGMENTINFO
{
   RECT        rcSegmentRect;
   L_UINT      uSegmentType;
   COLORREF    rgbColors[4];
   L_UINT      uColorsCount;
} SEGMENTINFO,  *LPSEGMENTINFO;

/* segmentation callback */ 
typedef L_INT (pEXT_CALLBACK pPDFCOMP_IMAGECALLBACK)(

   LCPDF_HANDLE
   hDocument,
   L_INT nPage,
   LPSEGMENTINFO
   pSegment,
   HANDLE UserData );

typedef enum  tagPDFCOMP_1BITCOMPTYPE
{
   PDFCOMP_1BITCOMPTYPE_ZIP = 0,
   PDFCOMP_1BITCOMPTYPE_LZW,
   PDFCOMP_1BITCOMPTYPE_CCITT_G3_1D,
   PDFCOMP_1BITCOMPTYPE_CCITT_G3_2D,
   PDFCOMP_1BITCOMPTYPE_CCITT_G4,
   PDFCOMP_1BITCOMPTYPE_JBIG2

} PDFCOMP_1BITCOMPTYPE, *LPPDFCOMP_1BITCOMPTYPE;

typedef enum  tagPDFCOMP_2BITCOMPTYPE
{
   PDFCOMP_2BITCOMPTYPE_ZIP = 0,
   PDFCOMP_2BITCOMPTYPE_LZW,   

} PDFCOMP_2BITCOMPTYPE, *LPPDFCOMP_2BITCOMPTYPE;


typedef enum  tagPDFCOMP_PICTURECOMPTYPE
{
   PDFCOMP_PICTURECOMPTYPE_JPEG = 0,
   PDFCOMP_PICTURECOMPTYPE_JPEG_YUV422,
   PDFCOMP_PICTURECOMPTYPE_JPEG_YUV411,
   PDFCOMP_PICTURECOMPTYPE_JPEG_PROGRESSIVE,
   PDFCOMP_PICTURECOMPTYPE_JPEG_PROGRESSIVE_YUV422,
   PDFCOMP_PICTURECOMPTYPE_JPEG_PROGRESSIVE_YUV411,
   PDFCOMP_PICTURECOMPTYPE_ZIP,
   PDFCOMP_PICTURECOMPTYPE_LZW

} PDFCOMP_PICTURECOMPTYPE, *LPPDFCOMP_PICTURECOMPTYPE;

typedef struct tagPDFCOMPRESSION
{
   L_UINT   uStructSize;

   L_UINT32  dwFlags;
   PDFCOMP_1BITCOMPTYPE  comp1Bit;
   PDFCOMP_2BITCOMPTYPE  comp2Bit;
   PDFCOMP_PICTURECOMPTYPE  compPicture;
   L_INT nQFactor;
   

} PDFCOMPRESSION, *LPPDFCOMPRESSION;


typedef enum tagPDFCOMP_IMAGEQUALITY
{
   PDFCOMP_IMAGEQUALITY_UNKNOWN = 0,
   PDFCOMP_IMAGEQUALITY_NOISY,
   PDFCOMP_IMAGEQUALITY_SCANNED,
   PDFCOMP_IMAGEQUALITY_PRINTED,
   PDFCOMP_IMAGEQUALITY_COMPUTER_GENERATED,
   PDFCOMP_IMAGEQUALITY_PHOTO,
   PDFCOMP_IMAGEQUALITY_USER

} PDFCOMP_IMAGEQUALITY, *LPPDFCOMP_IMAGEQUALITY;

typedef enum tagPDFCOMP_OUTPUTQUALITY
{
   PDFCOMP_OUTPUTQUALITY_AUTO = 0,
   PDFCOMP_OUTPUTQUALITY_POOR,
   PDFCOMP_OUTPUTQUALITY_AVERAGE,
   PDFCOMP_OUTPUTQUALITY_GOOD,
   PDFCOMP_OUTPUTQUALITY_EXCELLENT,
   PDFCOMP_OUTPUTQUALITY_USER

} PDFCOMP_OUTPUTQUALITY, *LPPDFCOMP_OUTPUTQUALITY;


typedef struct _PDFCOMPOPTIONS
{
   L_UINT   uStructSize;

   L_UINT32  dwFlags;

   PDFCOMP_IMAGEQUALITY  imageQuality;
   PDFCOMP_OUTPUTQUALITY outputQuality;

   L_UINT   uCleanSize;            //perform clean opertion it range from 1 to 10, 0 mean skip clean stage.
   L_UINT   uSegmentQuality;       //Threshold value that dicide how much segement quality should be taken to accept it as picture type or not. it is range from 0 to 100.
   L_UINT   uColorThreshold;       //The minimum distance between colors used to combine them(used for define the segments).
   L_UINT   uBackGroundThreshold;  //Threshold value that specify   the noise quantity inside the image.
   L_UINT   uCombineThreshold;     //The minimum distance between colors used to combine them (used for combine segments).

} PDFCOMPOPTIONS, * LPPDFCOMPOPTIONS;




L_LCMRC_API L_INT EXT_FUNCTION L_PdfCompInit( LCPDF_HANDLE *phDocument, pPDFCOMP_IMAGECALLBACK pCallback, HANDLE UserData );
L_LCMRC_API L_VOID EXT_FUNCTION L_PdfCompFree( LCPDF_HANDLE hDocument );

L_LCMRC_API L_INT EXT_FUNCTION L_PdfCompWriteA( LCPDF_HANDLE hDocument, L_CHAR  *pwszOutFile );
#if defined(FOR_UNICODE)
L_LCMRC_API L_INT EXT_FUNCTION L_PdfCompWrite( LCPDF_HANDLE hDocument, L_TCHAR  *pwszOutFile );
#else
#define L_PdfCompWrite L_PdfCompWriteA
#endif // #if defined(FOR_UNICODE)
L_LCMRC_API L_INT EXT_FUNCTION L_PdfCompSetCompression( LCPDF_HANDLE hDocument, LPPDFCOMPRESSION pCompression );

L_LCMRC_API L_INT EXT_FUNCTION L_PdfCompInsertMRC( LCPDF_HANDLE hDocHandle, pBITMAPHANDLE pBitmap, LPPDFCOMPOPTIONS pPDFOptions );
L_LCMRC_API L_INT EXT_FUNCTION L_PdfCompInsertNormal( LCPDF_HANDLE hDocHandle, pBITMAPHANDLE pBitmap );
L_LCMRC_API L_INT EXT_FUNCTION L_PdfCompInsertSegments( LCPDF_HANDLE hDocHandle, pBITMAPHANDLE pBitmap, L_UINT uSegmentCnt, LPSEGMENTINFO pSegmentInfo, L_BOOL bIsThereBackGround, COLORREF rgbBackGroundColor );

#undef L_HEADER_ENTRY
#include "ltpck.h"

#endif

/* EOF */ 