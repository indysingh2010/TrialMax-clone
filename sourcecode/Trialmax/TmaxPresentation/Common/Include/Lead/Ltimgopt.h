//==============================================================================
//
//  LTIMGOPT : Header file.
//
//  Copyright (C) 1990, 2007 LEAD Technologies, Inc.
//  All rights reserved.
//
//==============================================================================
#if !defined LTIMGOPT_H
#define LTIMGOPT_H

#if !defined(L_LTIMGOPT_API)
   #define L_LTIMGOPT_API
#endif // #if !defined(L_LTIMGOPT_API)

//============= INCLUDES =======================================================

#define L_HEADER_ENTRY
#include "ltpck.h"

//============= CONSTANTS ======================================================

// L_OptimizeImageDir Status defines

#define OPTIMIZE_DIR_PRE_OPTIMIZINGIMAGE  2 // The Image optimizer is going to optimize the image.
#define OPTIMIZE_DIR_OPTIMIZINGIMAGE      3 // The Image Optimizer optimizing the image.

//============= ENUMS ======================================================
// JPEG Color Space enum
typedef enum tagJPEGCOLORSPACE
{
   JPEG_COLORSPACE_411   = 0,  // Saving with YUV 4:1:1 color space.
   JPEG_COLORSPACE_422   = 1   // Saving with YUV 4:2:2 color space.

} JPEGCOLORSPACE, *pJPEGCOLORSPACE;

//============= STRUCTRUES ======================================================
// Optimized Image options structure, if the user wants to save with custom options
typedef struct _tagOPTIMIZEIMAGEOPTIONS
{
   L_UINT         uStructSize;   /* Size of the structure */

   L_UINT         uJPEGQFactor;  /* QFactor value to optimize with JPEG images (2-55) */

   L_UINT         uPNGQFactor;   /* QFactor value to optimize with PNG images (0-9) */

   JPEGCOLORSPACE JPEGColorSpace;/* Color space to optimize the JPEG images with */

   L_INT          nPercent;      /* percent of redundancy value (0-100) */

   L_UINT         uDistance;     /* 0 means no additional reduction
                                    1 to 255 further reduction based on the color distance */

   L_BOOL         bPickSamePalette; /* If the user wants to generate same palette for all image frames*/

} OPTIMIZEIMAGEOPTIONS, *pOPTIMIZEIMAGEOPTIONS;

/* A data to be passed to the callback user function to inform him about the image being processed*/
typedef struct _tagOPTIMIZEIMAGEDIRINFOA
{
   L_UINT                  uStructSize;            /* Size of the structure */
   L_CHAR                  szOrgFileName[MAX_PATH];       /* always holds the input filename */
   L_CHAR                  szOptFileName[MAX_PATH];       /* always holds the output filename */
   L_INT                   nStatusCode;            /* holds SUCCESS /error code/ Or Current Status */
   L_INT                   nFilePercent;           /* The Completion percentage of the operation for the currect processed file (1-100) */	
   L_INT                   nTotalPercent;          /* The Completion percentage of the operation for the whole directory images (1-100) */
   L_INT                   nTotalFolderFilesCount; /* The Total Number of files to be process */  
   pFILEINFOA              pFileInfo;              /* Always has the fileinfo structure data */
   pOPTIMIZEIMAGEOPTIONS   pOptImgOptions;         /* Copy of the original options passed */
}OPTIMIZEIMAGEDIRINFOA, *pOPTIMIZEIMAGEDIRINFOA;

#if defined(FOR_UNICODE)
typedef struct _tagOPTIMIZEIMAGEDIRINFO
{
   L_UINT                  uStructSize;            /* Size of the structure */
   L_TCHAR                 szOrgFileName[MAX_PATH];       /* always holds the input filename */
   L_TCHAR                 szOptFileName[MAX_PATH];       /* always holds the output filename */
   L_INT                   nStatusCode;            /* holds SUCCESS /error code/ Or Current Status */
   L_INT                   nFilePercent;           /* The Completion percentage of the operation for the currect processed file (1-100) */	
   L_INT                   nTotalPercent;          /* The Completion percentage of the operation for the whole directory images (1-100) */
   L_INT                   nTotalFolderFilesCount; /* The Total Number of files to be process */  
   pFILEINFO               pFileInfo;              /* Always has the fileinfo structure data */
   pOPTIMIZEIMAGEOPTIONS   pOptImgOptions;         /* Copy of the original options passed */
}OPTIMIZEIMAGEDIRINFO, *pOPTIMIZEIMAGEDIRINFO;
#else
typedef OPTIMIZEIMAGEDIRINFOA OPTIMIZEIMAGEDIRINFO;
typedef pOPTIMIZEIMAGEDIRINFOA pOPTIMIZEIMAGEDIRINFO;
#endif // #if defined(FOR_UNICODE)

//============= CALLBACKs ======================================================

typedef L_INT (pEXT_CALLBACK OPTIMIZEBUFFERCALLBACK)(L_INT nPercent, // Optimization Completion Percentage (0..100).
                                        L_VOID * pUserData);// pointer to additional parameters.

typedef L_INT (pEXT_CALLBACK OPTIMIZEIMAGEDIRCALLBACKA)(pOPTIMIZEIMAGEDIRINFOA  pOptImgDirCBInfo, // Pointer to IMAGEEVENTDATA.
                                          L_VOID * pUserData); // pointer to additional parameters.
#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK OPTIMIZEIMAGEDIRCALLBACK)(pOPTIMIZEIMAGEDIRINFO  pOptImgDirCBInfo, // Pointer to IMAGEEVENTDATA.
                                          L_VOID * pUserData); // pointer to additional parameters.
#else
typedef OPTIMIZEIMAGEDIRCALLBACKA OPTIMIZEIMAGEDIRCALLBACK;
typedef pOPTIMIZEIMAGEDIRINFOA pOPTIMIZEIMAGEDIRINFO;
#endif // #if defined(FOR_UNICODE)

//============= FUNCTIONS ======================================================
   
   /* 
      Get the default optimization options 
      The default options are:
      pOptimizeImageOptions->uStructSize = sizeof(OPTIMIZEIMAGEOPTIONS)
      pOptimizeImageOptions->uJPEGQFactor = 35;
      pOptimizeImageOptions->uPNGQFactor = 9;
      pOptimizeImageOptions->JPEGColorSpace = JPEG_COLORSPACE_411;
      pOptimizeImageOptions->nPercent = 10;
      pOptimizeImageOptions->uDistance = 8;
      pOptimizeImageOptions->bPickSamePalette = FALSE;
   */
   L_LTIMGOPT_API L_INT EXT_FUNCTION L_OptGetDefaultOptions(
      pOPTIMIZEIMAGEOPTIONS pOptImgOptions,
      L_UINT uStructSize);

   /*
      Optimize the image buffer (pOrgImgData) and update the phOptImgData and puOptImgDataSize 
      parameters by the optimized image buffer and buffer size.
   */
   L_LTIMGOPT_API L_INT EXT_FUNCTION L_OptOptimizeBuffer(
      L_UCHAR *         pOrgImgBuffer,          // Original Image Data.
      L_SIZE_T          uOrgImgBufferSize,      // Original Image Data size in mem in bytes.
      HGLOBAL *         phOptImgBuffer,         // Optimized Image Data.
      L_SIZE_T *        puOptImgBufferSize,     // Optimized Image Data size in mem in bytes.
      pOPTIMIZEIMAGEOPTIONS   pOptImgOptions,// Pointer to OPTIMIZEIMAGEOPTIONS structure.
      OPTIMIZEBUFFERCALLBACK  pfnOptBufferCB,   // Optional Callback.
      L_VOID *          pUserData);           // Optional Callback User data.

   /*
      Optimize a directory full of the supported images and saves the optimized images to 
      antoher output directory.
   */
   L_LTIMGOPT_API L_INT EXT_FUNCTION L_OptOptimizeDirA(
      L_CHAR *                   pszOrgDirPath, // Original Images Dir Path.
      L_CHAR *                   pszOptDirPath, // Optimized Images Dir Path.
      pOPTIMIZEIMAGEOPTIONS      pOptImgOptions, // Pointer to OPTIMIZEIMAGEOPTIONS structure.
      L_CHAR *                   pszFilesExt, // Indicates the files extensions to be optimized.
      L_BOOL                     bIncludeSubDirs, // True/False to recurse sub directories. 
      OPTIMIZEIMAGEDIRCALLBACKA  pfnOptImgDirCB, // Optional Callback.
      L_VOID *                   pUserData); // Optional Callback user data.

#if defined(FOR_UNICODE)
   L_LTIMGOPT_API L_INT EXT_FUNCTION L_OptOptimizeDir(
      L_TCHAR *                  pszOrgDirPath, // Original Images Dir Path.
      L_TCHAR *                  pszOptDirPath, // Optimized Images Dir Path.
      pOPTIMIZEIMAGEOPTIONS      pOptImgOptions, // Pointer to OPTIMIZEIMAGEOPTIONS structure.
      L_TCHAR *                  pszFilesExt, // Indicates the files extensions to be optimized.
      L_BOOL                     bIncludeSubDirs, // True/False to recurse sub directories. 
      OPTIMIZEIMAGEDIRCALLBACK   pfnOptImgDirCB, // Optional Callback.
      L_VOID *                   pUserData); // Optional Callback user data.

#else
#define L_OptOptimizeDir L_OptOptimizeDirA
#endif // #if defined(FOR_UNICODE)

#undef L_HEADER_ENTRY
#include "ltpck.h"

#endif  // LTIMGOPT_H

/*.End.Of.File.........................................................................*/