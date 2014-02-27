/*************************************************************
   Ltdlgfile.h - Common Dialogs module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDLG_FILE_H)
#define LTDLG_FILE_H

#if !defined(L_LTDLG_API)
   #if !defined(FOR_MANAGED)
      #define L_LTDLG_API __declspec(dllimport)
   #else
      #define L_LTDLG_API
   #endif // #if !defined(FOR_MANAGED)
#endif

#include "Ltkrn.h"
#include "Ltimg.h"
#include "Ltfil.h"
#include "Ltclr.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

//.............................................................................
// enums, defines and structures
//.............................................................................
enum 
{
   DLG_FF_SAVE_LEAD = 0,
   DLG_FF_SAVE_JPEG,
   DLG_FF_SAVE_CMW,
   DLG_FF_SAVE_J2K,
   DLG_FF_SAVE_TIFF,
   DLG_FF_SAVE_GIF,
   DLG_FF_SAVE_PNG,

   // Alphabetical Order                         
   // +++ "A" +++                               
   DLG_FF_SAVE_ANI,

   // +++ "C" +++                               
   DLG_FF_SAVE_CALS,
   DLG_FF_SAVE_CIN,
   DLG_FF_SAVE_CLP,
   DLG_FF_SAVE_WMZ,

   // +++ "D" +++        

   DLG_FF_SAVE_DICOMGRAY,
   DLG_FF_SAVE_DICOMCOLOR,
   DLG_FF_SAVE_CUT,

   // +++ "E" +++                               
   DLG_FF_SAVE_ECW,
   DLG_FF_SAVE_EMF,
   DLG_FF_SAVE_EPS,
   DLG_FF_SAVE_EXIF,

   // +++ "F" +++                               
   DLG_FF_SAVE_FAX,
   DLG_FF_SAVE_FIT,
   DLG_FF_SAVE_FLC,
   DLG_FF_SAVE_FPX,

   // +++ "G" +++                               
   DLG_FF_SAVE_IMG,
   DLG_FF_SAVE_GEOTIFF,

   // +++ "I" +++                               
   DLG_FF_SAVE_IFF,
   DLG_FF_SAVE_ITG,

   // +++ "J" +++                               
   DLG_FF_SAVE_JBIG,

   // +++ "M" +++                               
   DLG_FF_SAVE_MAC,
   DLG_FF_SAVE_PCT,
   DLG_FF_SAVE_MODCA,
#ifndef FOR_WIN64
   DLG_FF_SAVE_AWD,
#endif //FOR_WIN64
   DLG_FF_SAVE_MSP,

   // +++ "O" +++                               
   DLG_FF_SAVE_OS2BMP,

   // +++ "P" +++                               
   DLG_FF_SAVE_PBM,
   DLG_FF_SAVE_PCX,
   DLG_FF_SAVE_PDF,
   DLG_FF_SAVE_PGM,
   DLG_FF_SAVE_PPM,
   DLG_FF_SAVE_PSD,
   DLG_FF_SAVE_PSP,

   // +++ "R" +++                               
   DLG_FF_SAVE_RAW,
   DLG_FF_SAVE_RAWICA,

   // +++ "S" +++                               
   DLG_FF_SAVE_SCT,
   DLG_FF_SAVE_SGI,
   DLG_FF_SAVE_SFF,
   DLG_FF_SAVE_RAS,

   // +++ "T" +++                               
   DLG_FF_SAVE_TGA,
   DLG_FF_SAVE_TIFX,

   // +++ "W" +++                               
   DLG_FF_SAVE_WFX,
   DLG_FF_SAVE_BMP,
   DLG_FF_SAVE_CUR,
   DLG_FF_SAVE_ICO,
   DLG_FF_SAVE_WBMP,
   DLG_FF_SAVE_WMF,
   DLG_FF_SAVE_WPG,

   // +++ "X" +++                               
   DLG_FF_SAVE_XBM,
   DLG_FF_SAVE_SMP,
   DLG_FF_SAVE_XPM,
   DLG_FF_SAVE_XWD,

   // Types added after the release
   DLG_FF_SAVE_MRC,
   DLG_FF_SAVE_ABC,
   
   DLG_FF_SAVE_JBIG2,
   DLG_FF_SAVE_ABIC,

   DLG_FF_SAVE_HDP,

   DLG_FF_SAVE_PNG_ICO,
   DLG_FF_SAVE_XPS,

   DLG_FF_SAVE_MNG,

   DLG_FF_SAVE_COUNT
} ;


//.............................................................................
// Files Sub-format Flags define
//.............................................................................

// FileSave sub-format values        
// LEAD CMP file subtypes (8,24-bit) 
enum 
{
   DLG_FF_SAVE_SUB_CMP_NONPROGRESSIVE = 0x0001,
   DLG_FF_SAVE_SUB_CMP_PROGRESSIVE    = 0x0002
} ;

// JPEG file subtypes (24-bit)       
enum 
{
   DLG_FF_SAVE_SUB_JPEG24_YUV_444  = 0x0001,
   DLG_FF_SAVE_SUB_JPEG24_YUV_422  = 0x0002,
   DLG_FF_SAVE_SUB_JPEG24_YUV_411  = 0x0004,
   DLG_FF_SAVE_SUB_JPEG24_PROG_444 = 0x0008,
   DLG_FF_SAVE_SUB_JPEG24_PROG_422 = 0x0010,
   DLG_FF_SAVE_SUB_JPEG24_PROG_411 = 0x0020,
   DLG_FF_SAVE_SUB_JPEG24_LOSSLESS = 0x0040,
   DLG_FF_SAVE_SUB_JPEG24_LAB_444  = 0x0080,
   DLG_FF_SAVE_SUB_JPEG24_LAB_422  = 0x0100,
   DLG_FF_SAVE_SUB_JPEG24_LAB_411  = 0x0200
} ;

// JPEG file subtypes (12-bit)       
enum 
{
   DLG_FF_SAVE_SUB_JPEG12_YUV_400  = 0x0001,
   DLG_FF_SAVE_SUB_JPEG12_LOSSLESS = 0x0002
} ;

// JPEG file subtypes (8-bit)        
enum 
{
   DLG_FF_SAVE_SUB_JPEG8_YUV_400  = 0x0001,
   DLG_FF_SAVE_SUB_JPEG8_PROG_400 = 0x0002,
   DLG_FF_SAVE_SUB_JPEG8_LOSSLESS = 0x0004
} ;

// JPEG 2000 file subtypes (8,12,16,24, 32-bit)
enum 
{
   DLG_FF_SAVE_SUB_J2K_STREAM = 0x0001,
   DLG_FF_SAVE_SUB_J2K_JP2    = 0x0002,
   DLG_FF_SAVE_SUB_J2K_JPX    = 0x0003,
} ;

// CALS file subtypes (1-bit)       
enum 
{
   DLG_FF_SAVE_SUB_CALS  = 0x0001,
   DLG_FF_SAVE_SUB_CALS2 = 0x0002,
   DLG_FF_SAVE_SUB_CALS3 = 0x0004,
   DLG_FF_SAVE_SUB_CALS4 = 0x0008
} ;

// CLP file subtypes (8-bit)       
enum 
{
   DLG_FF_SAVE_SUB_CLP_UNCOMPRESSED = 0x0001,
   DLG_FF_SAVE_SUB_CLP_RLE          = 0x0002
} ;

// DICOM file subtypes (8-bit)       
enum 
{
   DLG_FF_SAVE_SUB_DICOM8_GRAY               = 0x0001,
   DLG_FF_SAVE_SUB_DICOM8_COLOR              = 0x0002,
   DLG_FF_SAVE_SUB_DICOM8_RLE_GRAY           = 0x0004,
   DLG_FF_SAVE_SUB_DICOM8_RLE_COLOR          = 0x0008,
   DLG_FF_SAVE_SUB_DICOM8_JPEG_GRAY          = 0x0010,
   DLG_FF_SAVE_SUB_DICOM8_J2K_GRAY_LOSSLESS  = 0x0020,
   DLG_FF_SAVE_SUB_DICOM8_J2K_GRAY           = 0x0040

} ;

// DICOM 16-bit file subtypes 
enum
{
   DLG_FF_SAVE_SUB_DICOM16_GRAY               = 0x0001,
   DLG_FF_SAVE_SUB_DICOM16_RLE_GRAY           = 0x0002,
   DLG_FF_SAVE_SUB_DICOM16_LOSSLESSJPEG_GRAY  = 0x0004,
   DLG_FF_SAVE_SUB_DICOM16_J2K_GRAY_LOSSLESS  = 0x0008,
   DLG_FF_SAVE_SUB_DICOM16_J2K_GRAY           = 0x0010
} ;

// DICOM 24-bit file subtypes 
enum
{
   DLG_FF_SAVE_SUB_DICOM24_COLOR              = 0x0001,
   DLG_FF_SAVE_SUB_DICOM24_RLE_COLOR          = 0x0002,
   DLG_FF_SAVE_SUB_DICOM24_LOSSLESSJPEG_COLOR = 0x0004,
   DLG_FF_SAVE_SUB_DICOM24_JPEG_COLOR         = 0x0008,
   DLG_FF_SAVE_SUB_DICOM24_J2K_GRAY_LOSSLESS  = 0x0010,
   DLG_FF_SAVE_SUB_DICOM24_J2K_GRAY           = 0x0020
} ;

// RAW FAX file subtypes 
enum 
{
   DLG_FF_SAVE_SUB_FAX_G3_1D = 0x0001,
   DLG_FF_SAVE_SUB_FAX_G3_2D = 0x0002,
   DLG_FF_SAVE_SUB_FAX_G4    = 0x0004
} ;

// MODCA:IOCA 1-bit file subtypes 
enum
{
   DLG_FF_SAVE_SUB_ICA_G3_1D        = 0x0001,
   DLG_FF_SAVE_SUB_ICA_G3_2D        = 0x0002,
   DLG_FF_SAVE_SUB_ICA_G4           = 0x0004,
   DLG_FF_SAVE_SUB_ICA_IBM_MMR      = 0x0008,
   DLG_FF_SAVE_SUB_ICA_UNCOMPRESSED = 0x0010,
   DLG_FF_SAVE_SUB_ICA_ABIC         = 0x0020
} ;

// MODCA:IOCA 4-bit file subtypes 
enum
{
   DLG_FF_SAVE_SUB_ICA4_ABIC = 0x0001
} ;

// RAWICA file subtypes 
enum
{
   DLG_FF_SAVE_SUB_RAWICA_G3_1D        = 0x0001,
   DLG_FF_SAVE_SUB_RAWICA_G3_2D        = 0x0002,
   DLG_FF_SAVE_SUB_RAWICA_G4           = 0x0004,
   DLG_FF_SAVE_SUB_RAWICA_IBM_MMR      = 0x0008,
   DLG_FF_SAVE_SUB_RAWICA_UNCOMPRESSED = 0x0010
} ;

// SMP file subtypes 
enum
{
   DLG_FF_SAVE_SUB_SMP_CCITT_GROUP3_1D  = 0x0001,
   DLG_FF_SAVE_SUB_SMP_CCITT_GROUP3_2D  = 0x0002,
   DLG_FF_SAVE_SUB_SMP_CCITT_GROUP4     = 0x0004,
   DLG_FF_SAVE_SUB_SMP_UNCOMPRESSED     = 0x0008
} ;

// EXIF file subtypes                
enum 
{
   DLG_FF_SAVE_SUB_EXIF_UNCOMPRESSEDRGB = 0x0001,
   DLG_FF_SAVE_SUB_EXIF_UNCOMPRESSEDYCC = 0x0002,
   DLG_FF_SAVE_SUB_EXIF_JPEG_411        = 0x0004,
   DLG_FF_SAVE_SUB_EXIF_JPEG_422        = 0x0008
} ;

// FPX file subtypes                 
enum 
{
   DLG_FF_SAVE_SUB_FPX_UNCOMPRESSED = 0x0001,
   DLG_FF_SAVE_SUB_FPX_SINGLECOLOR  = 0x0002,
   DLG_FF_SAVE_SUB_FPX_DEFQFACTOR   = 0x0004,
   DLG_FF_SAVE_SUB_FPX_SPECQFACTOR  = 0x0008
} ;

// GIF file subtypes                 
enum 
{
   DLG_FF_SAVE_SUB_GIF_INTERLACED89A    = 0x0001,
   DLG_FF_SAVE_SUB_GIF_NONINTERLACED89A = 0x0002
} ;

// OS/2 BMP file subtypes
enum 
{
   DLG_FF_SAVE_SUB_OS2_VER1 = 0x0001,
   DLG_FF_SAVE_SUB_OS2_VER2 = 0x0002
} ;

// TIFF file subtypes (1-bit)
enum 
{
   DLG_FF_SAVE_SUB_TIFF1_CCITT           = 0x0001,
   DLG_FF_SAVE_SUB_TIFF1_CCITT_G3_1D     = 0x0002,
   DLG_FF_SAVE_SUB_TIFF1_CCITT_G3_2D     = 0x0004,
   DLG_FF_SAVE_SUB_TIFF1_CCITT_G4        = 0x0008,
   DLG_FF_SAVE_SUB_TIFF1_UNCOMPRESSEDRGB = 0x0010,
   DLG_FF_SAVE_SUB_TIFF1_RLERGB          = 0x0020,
   DLG_FF_SAVE_SUB_TIFF1_LZWRGB          = 0x0040,
   DLG_FF_SAVE_SUB_TIFF1_JBIG            = 0x0080,
   DLG_FF_SAVE_SUB_TIFF1_JBIG2           = 0x0100,
   DLG_FF_SAVE_SUB_TIFF1_ABC             = 0x0200,
   DLG_FF_SAVE_SUB_TIFF1_ABIC            = 0x0400
} ;

// TIFF file subtypes 4-bit) 
enum 
{
   DLG_FF_SAVE_SUB_TIFF4_UNCOMPRESSED = 0x0001,
   DLG_FF_SAVE_SUB_TIFF4_RLE          = 0x0002,
   DLG_FF_SAVE_SUB_TIFF4_LZW          = 0x0004,
   DLG_FF_SAVE_SUB_TIFF4_JBIG         = 0x0008,
   DLG_FF_SAVE_SUB_TIFF4_ABIC         = 0x0010
} ;

// TIFF file subtypes (2,3,5,6,7-bit) 
enum 
{
   DLG_FF_SAVE_SUB_TIFFOTHER_UNCOMPRESSED = 0x0001,
   DLG_FF_SAVE_SUB_TIFFOTHER_RLE          = 0x0002,
   DLG_FF_SAVE_SUB_TIFFOTHER_LZW          = 0x0004,
   DLG_FF_SAVE_SUB_TIFFOTHER_JBIG         = 0x0008
} ;

// TIFF file subtypes (8-bit)        
enum 
{
   DLG_FF_SAVE_SUB_TIFF8_UNCOMPRESSEDRGB = 0x0001,
   DLG_FF_SAVE_SUB_TIFF8_RLERGB          = 0x0002,
   DLG_FF_SAVE_SUB_TIFF8_JPEG_GRAY_YCC   = 0x0004,
   DLG_FF_SAVE_SUB_TIFF8_LOSSLESSJPEG    = 0x0008,
   DLG_FF_SAVE_SUB_TIFF8_LZWRGB          = 0x0010,
   DLG_FF_SAVE_SUB_TIFF8_JBIG            = 0x0020,
   DLG_FF_SAVE_SUB_TIFF8_CMP             = 0x0040,
   DLG_FF_SAVE_SUB_TIFF8_J2K             = 0x0080,
   DLG_FF_SAVE_SUB_TIFF8_CMW             = 0x0100
} ;

// TIFF file subtypes (12-bit)
enum 
{
   DLG_FF_SAVE_SUB_TIFF12_UNCOMPRESSED  = 0x0001,
   DLG_FF_SAVE_SUB_TIFF12_RLE           = 0x0002,
   DLG_FF_SAVE_SUB_TIFF12_LOSSLESSJPEG  = 0x0004,
   DLG_FF_SAVE_SUB_TIFF12_JPEG_GRAY_YCC = 0x0008,
   DLG_FF_SAVE_SUB_TIFF12_LZW           = 0x0010,
   DLG_FF_SAVE_SUB_TIFF12_J2K           = 0x0020,
   DLG_FF_SAVE_SUB_TIFF12_CMW           = 0x0040
} ;

// TIFF file subtypes (16-bit)
enum 
{
   DLG_FF_SAVE_SUB_TIFF16_UNCOMPRESSED = 0x0001,
   DLG_FF_SAVE_SUB_TIFF16_RLE          = 0x0002,
   DLG_FF_SAVE_SUB_TIFF16_LOSSLESSJPEG = 0x0004,
   DLG_FF_SAVE_SUB_TIFF16_LZW          = 0x0008,
   DLG_FF_SAVE_SUB_TIFF16_J2K          = 0x0010,
   DLG_FF_SAVE_SUB_TIFF16_CMW          = 0x0020
} ;

// TIFF file subtypes (24-bit)       
enum 
{
   DLG_FF_SAVE_SUB_TIFF24_UNCOMPRESSEDRGB    = 0x00000001,
   DLG_FF_SAVE_SUB_TIFF24_UNCOMPRESSEDCMYK   = 0x00000002,
   DLG_FF_SAVE_SUB_TIFF24_UNCOMPRESSEDYCC    = 0x00000004,
   DLG_FF_SAVE_SUB_TIFF24_RLERGB             = 0x00000008,
   DLG_FF_SAVE_SUB_TIFF24_RLECMYK            = 0x00000010,
   DLG_FF_SAVE_SUB_TIFF24_RLEYCC             = 0x00000020,
   DLG_FF_SAVE_SUB_TIFF24_JPEG_YCC_444       = 0x00000040,
   DLG_FF_SAVE_SUB_TIFF24_JPEG_YCC_422       = 0x00000080,
   DLG_FF_SAVE_SUB_TIFF24_JPEG_YCC_411       = 0x00000100,
   DLG_FF_SAVE_SUB_TIFF24_LOSSLESSJPEG       = 0x00000200,
   DLG_FF_SAVE_SUB_TIFF24_LZWRGB             = 0x00000400,
   DLG_FF_SAVE_SUB_TIFF24_LZWCMYK            = 0x00000800,
   DLG_FF_SAVE_SUB_TIFF24_LZWYCC             = 0x00001000,
   DLG_FF_SAVE_SUB_TIFF24_CMP_NONPROGRESSIVE = 0x00002000,
   DLG_FF_SAVE_SUB_TIFF24_CMP_PROGRESSIVE    = 0x00004000,
   DLG_FF_SAVE_SUB_TIFF24_J2K                = 0x00008000,
   DLG_FF_SAVE_SUB_TIFF24_CMW                = 0x00010000,
   
   //Subtypes added after the release 
   DLG_FF_SAVE_SUB_TIFF24_LEAD_MRC           = 0x00020000,
   DLG_FF_SAVE_SUB_TIFF24_MRC                = 0x00040000

} ;

// TIFF file subtypes (32-bit)       
enum 
{
   DLG_FF_SAVE_SUB_TIFF32_UNCOMPRESSEDRGB  = 0x0001,
   DLG_FF_SAVE_SUB_TIFF32_UNCOMPRESSEDCMYK = 0x0002,
   DLG_FF_SAVE_SUB_TIFF32_RLERGB           = 0x0004,
   DLG_FF_SAVE_SUB_TIFF32_RLECMYK          = 0x0008,
   DLG_FF_SAVE_SUB_TIFF32_LZWRGB           = 0x0010,
   DLG_FF_SAVE_SUB_TIFF32_LZWCMYK          = 0x0020
} ;

// TIFF file subtypes (48-bit)       
enum 
{
   DLG_FF_SAVE_SUB_TIFF48_UNCOMPRESSEDRGB = 0x0001,
   DLG_FF_SAVE_SUB_TIFF48_LZWRGB          = 0x0002
} ;


// TIFF file subtypes (64-bit) 
enum 
{    
   DLG_FF_SAVE_SUB_TIFF64_UNCOMPRESSEDRGB = 0x0001,
   DLG_FF_SAVE_SUB_TIFF64_LZWRGB          = 0x0002
} ;

// BMP file subtypes (4 and 8-bit only) 
enum 
{
   DLG_FF_SAVE_SUB_BMP_UNCOMPRESSED  = 0x0001,
   DLG_FF_SAVE_SUB_BMP_RLECOMPRESSED = 0x0002
} ;

// WFX file subtypes                 
enum 
{
   DLG_FF_SAVE_SUB_WFX_CCITT_G3_1D = 0x0001,
   DLG_FF_SAVE_SUB_WFX_CCITT_G4    = 0x0002
} ;

// PPM file subtypes                 
enum 
{
   DLG_FF_SAVE_SUB_PPM_ASCII  = 0x0001,
   DLG_FF_SAVE_SUB_PPM_BINARY = 0x0002
} ;

// PGM file subtypes 
enum 
{
   DLG_FF_SAVE_SUB_PGM_ASCII  = 0x0001,
   DLG_FF_SAVE_SUB_PGM_BINARY = 0x0002
} ;

// PBM file subtypes 
enum 
{
   DLG_FF_SAVE_SUB_PBM_ASCII  = 0x0001,
   DLG_FF_SAVE_SUB_PBM_BINARY = 0x0002
} ;

// IFF file subtypes 
// All BitsPerPixel have subtypes 
enum 
{
   DLG_FF_SAVE_SUB_IFF_ILBM_UNCOMPRESSED = 0x0001,
   DLG_FF_SAVE_SUB_IFF_ILBM_RLE          = 0x0002,
   DLG_FF_SAVE_SUB_IFF_CAT_UNCOMPRESSED  = 0x0004,
   DLG_FF_SAVE_SUB_IFF_CAT_RLE           = 0x0008
} ;

// SGI file subtypes 
// All BitsPerPixel have subtypes 
enum 
{
   DLG_FF_SAVE_SUB_SGI_UNCOMPRESSED  = 0x0001,
   DLG_FF_SAVE_SUB_SGI_RLECOMPRESSED = 0x0002
} ;

// PDF file subtypes (1-bit)
enum 
{
   DLG_FF_SAVE_SUB_PDF1_UNCOMPRESSED  = 0x0001,
   DLG_FF_SAVE_SUB_PDF1_CCITT_G3_1D   = 0x0002,
   DLG_FF_SAVE_SUB_PDF1_CCITT_G3_2D   = 0x0004,
   DLG_FF_SAVE_SUB_PDF1_CCITT_G4      = 0x0008,
   DLG_FF_SAVE_SUB_PDF1_RAS_PDF_LZW   = 0x0010,
   DLG_FF_SAVE_SUB_PDF1_RAS_PDF_JBIG2 = 0x0020
} ;

// PDF file subtypes (2-bit)
enum 
{
   DLG_FF_SAVE_SUB_PDF2_RAS_PDF_LZW = 0x0001
} ;

// PDF file subtypes (4-bit)
enum 
{
   DLG_FF_SAVE_SUB_PDF4_RAS_PDF_LZW = 0x0001
} ;

// PDF file subtypes (8-bit)       
enum 
{
   DLG_FF_SAVE_SUB_PDF8_UNCOMPRESSED = 0x0001,
   DLG_FF_SAVE_SUB_PDF8_LOSSLESSJPEG = 0x0002,
   DLG_FF_SAVE_SUB_PDF8_RAS_PDF_LZW  = 0x0004
} ;

// PDF file subtypes (24-bit)       
enum 
{
   DLG_FF_SAVE_SUB_PDF24_UNCOMPRESSED = 0x0001,
   DLG_FF_SAVE_SUB_PDF24_YUV_444      = 0x0002,
   DLG_FF_SAVE_SUB_PDF24_YUV_422      = 0x0004,
   DLG_FF_SAVE_SUB_PDF24_YUV_411      = 0x0008,
   DLG_FF_SAVE_SUB_PDF24_RAS_PDF_LZW  = 0x0010,
   DLG_FF_SAVE_SUB_PDF24_PDF_LEAD_MRC = 0x0020
} ;

// TGA
enum
{
   DLG_FF_SAVE_SUB_TGA_RLE          = 0x0001,
   DLG_FF_SAVE_SUB_TGA_UNCOMPRESSED = 0x0002
} ;

// SUN Ras file sub types (1, 4, 8, 24, 32 bit)
enum
{
   DLG_FF_SAVE_SUB_RAS_RLE          = 0x0001,
   DLG_FF_SAVE_SUB_RAS_UNCOMPRESSED = 0x0002
} ;

// PSP file subtypes (1, 4, 8, 24 bit )
enum
{
   DLG_FF_SAVE_SUB_PSP_RLE          = 0x0001,
   DLG_FF_SAVE_SUB_PSP_UNCOMPRESSED = 0x0002
} ;

// TIFX file subtypes (1 bit)
enum
{
   DLG_FF_SAVE_SUB_TIFX_CCITT_G4     = 0x0001,
   DLG_FF_SAVE_SUB_TIFX_CCITT_G3_1D  = 0x0002,
   DLG_FF_SAVE_SUB_TIFX_CCITT_G3_2D  = 0x0004,
   DLG_FF_SAVE_SUB_TIFX_JBIG         = 0x0008
} ;

// SVG file subtypes
enum
{
   DLG_FF_SAVE_SUB_SVG_EXTERNAL = 0x0001,
   DLG_FF_SAVE_SUB_SVG_EMBEDDED = 0x0002
} ;

// ITG file subtypes
enum
{
   DLG_FF_SAVE_SUB_ITG_RLE      = 0x0001,
   DLG_FF_SAVE_SUB_ITG_CCITT_G4 = 0x0002
} ;

// XWD file subtypes
enum
{
   DLG_FF_SAVE_SUB_XWD_VERSION10 = 0x0001,
   DLG_FF_SAVE_SUB_XWD_VERSION11 = 0x0002
} ;

///////////////////////////////////
// Subtypes added after the release 
///////////////////////////////////

// MRC file subtypes
enum
{
   DLG_FF_SAVE_SUB_LEAD_MRC = 0x0001,
   DLG_FF_SAVE_SUB_MRC = 0x0002
} ;

// Overwrite initial value
enum
{
   DLG_FILECONVERSION_OVERWRITE_ASK,
   DLG_FILECONVERSION_OVERWRITE_SKIP,
   DLG_FILECONVERSION_OVERWRITE_REPLACE,
   DLG_FILECONVERSION_OVERWRITE_RENAME
} ;

enum
{
   DLG_FF_SAVE_SUB_HDP_GRAY = 0x0001,
   DLG_FF_SAVE_SUB_HDP_CMYK = 0x0002
} ;

// possible values for Save Multi-page Options 
enum 
{
   MULTIPAGE_OPERATION_OVERWRITE = 1,
   MULTIPAGE_OPERATION_APPEND,
   MULTIPAGE_OPERATION_REPLACE,
   MULTIPAGE_OPERATION_INSERT
} ;

typedef enum 
{
   DLG_VIEW_LIST,
   DLG_VIEW_ICONS,
   DLG_VIEW_TILES,
   DLG_VIEW_THUMBNAILS,
} INITIALVIEWENUM;

typedef enum
{
   DLG_PDF_PROFILE_DEFAULT,
   DLG_PDF_PROFILE_PDFA,
   DLG_PDF_PROFILE_PDF_V14,
   DLG_PDF_PROFILE_PDF_V15,
} PDFPROFILESENUM;


// Flags For L_DlgFileConversion ( ... ) 
#define DLG_FILECONVERSION_SHOW_CONTEXTHELP    0x00000001
#define DLG_FILECONVERSION_SHOW_PREVIEW        0x00000002
#define DLG_FILECONVERSION_SHOW_LOADOPTIONS    0x00000004  
#define DLG_FILECONVERSION_SHOW_FILEINFO       0x00000008  
#define DLG_FILECONVERSION_SHOW_PREVIEW_PAGES  0x00000010  
#define DLG_FILECONVERSION_SHOW_RESIZE         0x00000020
#define DLG_FILECONVERSION_SHOW_ROTATE         0x00000040
#define DLG_FILECONVERSION_SHOW_NAMINGTEMPLATE 0x00000080
// Flags for Overwrite combo content
#define DLG_FILECONVERSION_SHOW_OVERWRITE                   0x00000100 
                                                                       
#define DLG_FILECONVERSION_SHOW_OVERWRITE_ASK               0x00000200
#define DLG_FILECONVERSION_SHOW_OVERWRITE_SKIP              0x00000400
#define DLG_FILECONVERSION_SHOW_OVERWRITE_REPLACE           0x00000800
#define DLG_FILECONVERSION_SHOW_OVERWRITE_RENAME            0x00001000
#define DLG_FILECONVERSION_SHOW_OVERWRITE_ALL               0x00001E00 
                                                               
#define DLG_FILECONVERSION_SHOW_ADD                         0x00002000
#define DLG_FILECONVERSION_SHOW_ADDFOLDER                   0x00004000
#define DLG_FILECONVERSION_SHOW_REMOVE                      0x00008000
#define DLG_FILECONVERSION_SHOW_SELECTALL                   0x00010000
#define DLG_FILECONVERSION_SHOW_DELETEORIGINAL              0x00020000
#define DLG_FILECONVERSION_SHOW_NEWFORMATSUPDATES           0x00040000

// Flags for L_DlgFilesAssociation ( ... )
#define DLG_FILESASSOCIATION_SHOW_CONTEXTHELP        0x00000001

// flags for L_DlgOpen() 
#define DLG_OPEN_SHOW_CONTEXTHELP             0x00000001  
#define DLG_OPEN_SHOW_PROGRESSIVE             0x00000002  
#define DLG_OPEN_SHOW_MULTIPAGE               0x00000004  
#define DLG_OPEN_SHOW_LOADROTATED             0x00000008  
#define DLG_OPEN_SHOW_LOADCOMPRESSED          0x00000010  
#define DLG_OPEN_SHOW_FILEINFO                0x00000020  
#define DLG_OPEN_SHOW_PREVIEW                 0x00000040  
#define DLG_OPEN_SHOW_DELPAGE                 0x00000080  
#define DLG_OPEN_SHOW_LOADOPTIONS             0x00000100  
#define DLG_OPEN_SHOW_RASTEROPTIONS           0x00000200  
#define DLG_OPEN_SHOW_PDFOPTIONS              0x00000400  
#define DLG_OPEN_SHOW_VECTOROPTIONS           0x00000800  

#define DLG_OPEN_VIEWTOTALPAGES               0x00010000  
#define DLG_OPEN_ENABLESIZING                 0x00020000  
#define DLG_OPEN_NOFILEMUSTEXIST              0x00040000  
#define DLG_OPEN_NOPATHMUSTEXIST              0x00080000  
#define DLG_OPEN_USEFILESTAMP                 0x00100000  
#define DLG_OPEN_LOADBITMAP                   0x00200000  
#define DLG_OPEN_GENERATETHUMBNAIL            0x00400000  

#define DLG_OPEN_ALWAYSLOADCOMPRESSED         0x00800000  
#define DLG_OPEN_ALWAYSLOADROTATED            0x01000000
#define DLG_OPEN_LOADANY                      0x04000000
#define DLG_OPEN_SHOW_XPSOPTIONS              0x08000000  
#if defined(LEADTOOLS_V16_OR_LATER)
#define DLG_OPEN_SHOW_XLSOPTIONS               0x10000000
#define DLG_OPEN_SHOW_RASTERIZEDOCUMENTOPTIONS 0x20000000
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
// flags for L_DlgSave() 
#define DLG_SAVE_AUTOPROCESS                        0x00000001 
#define DLG_SAVE_SHOW_CONTEXTHELP                   0x00000002
#define DLG_SAVE_SHOW_FILEOPTIONS_PROGRESSIVE       0x00000004 
#define DLG_SAVE_SHOW_FILEOPTIONS_MULTIPAGE         0x00000008 
#define DLG_SAVE_SHOW_FILEOPTIONS_STAMP             0x00000010 
#define DLG_SAVE_SHOW_FILEOPTIONS_QFACTOR           0x00000020 
#define DLG_SAVE_SHOW_FILEOPTIONS_J2KOPTIONS        0x00000040 
#define DLG_SAVE_SHOW_FILEOPTIONS_BASICJ2KOPTIONS   0x00000080 
#define DLG_SAVE_ENABLESIZING                       0x00000100 
#define DLG_SAVE_SHOW_FILEOPTIONS_JBIG2OPTIONS      0x00000200
#if defined(LEADTOOLS_V16_OR_LATER)
#define DLG_SAVE_SHOW_FILEOPTIONS_PDFPROFILES       0x00000400
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

// Flags for L_DlgICCProfileDlg()
#define DLG_ICCPROFILE_SHOW_CONTEXTHELP 0x00000001  
#define DLG_ICCPROFILE_SHOW_SAVE        0x00000002
#define DLG_ICCPROFILE_SHOW_LOAD        0x00000004

typedef struct _GETDIRECTORYDLGPARAMSA
{
   L_UINT          uStructSize ;          
   L_CHAR *        pszDirectory ; 
   L_INT           nBuffSize ; 
   L_CHAR *        pszFilter ;
   L_INT           nFilterIndex ;
   L_CHAR *        pszTitle ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} GETDIRECTORYDLGPARAMSA, * LPGETDIRECTORYDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _GETDIRECTORYDLGPARAMS
{
   L_UINT          uStructSize ;          
   L_TCHAR *       pszDirectory ; 
   L_INT           nBuffSize ; 
   L_TCHAR *       pszFilter ;
   L_INT           nFilterIndex ;
   L_TCHAR *       pszTitle ; 
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} GETDIRECTORYDLGPARAMS, * LPGETDIRECTORYDLGPARAMS ;
#else
typedef GETDIRECTORYDLGPARAMSA GETDIRECTORYDLGPARAMS;
typedef LPGETDIRECTORYDLGPARAMSA LPGETDIRECTORYDLGPARAMS;
#endif // #if defined(FOR_UNICODE)
typedef struct _FILESAVEFORMATBPP
{
   L_UINT  uStructSize ; 
   L_INT32 nFormatBpp ;  
   L_UINT  uSubFormats ; 

} FILESAVEFORMATBPP, * LPFILESAVEFORMATBPP ;

typedef struct _FILESAVEFORMAT
{
   L_UINT               uStructSize  ;            
   L_INT32              nFormat ;           
   L_INT                nBppCount ;         
   LPFILESAVEFORMATBPP  pFileSaveFormatBpp; 

} FILESAVEFORMAT, * LPFILESAVEFORMAT ;

typedef struct _FILECONVERSIONDLGPARAMSA
{
   L_UINT           uStructSize ;                          
   L_UINT           uOverwrite ;                     
   LPFILESAVEFORMAT pFileFormats ;                   
   L_INT32          nFileFormatsCount ;              
   L_CHAR           szDestPath [ L_MAXPATH ] ;        
   L_CHAR           szAddSrcFilePath [ L_MAXPATH ] ;  
   L_BOOL           bUseLogReport ;                  
   L_BOOL           bRemoveSrcFile ;                 
   L_BOOL           bShowFullPath ;                  
   L_BOOL           bUseOriginalFolder ;             
   L_CHAR  *        pszSrcFileList ;                  
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} FILECONVERSIONDLGPARAMSA, * LPFILECONVERSIONDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _FILECONVERSIONDLGPARAMS
{
   L_UINT           uStructSize ;                          
   L_UINT           uOverwrite ;                     
   LPFILESAVEFORMAT pFileFormats ;                   
   L_INT32          nFileFormatsCount ;              
   L_TCHAR          szDestPath [ L_MAXPATH ] ;        
   L_TCHAR          szAddSrcFilePath [ L_MAXPATH ] ;  
   L_BOOL           bUseLogReport ;                  
   L_BOOL           bRemoveSrcFile ;                 
   L_BOOL           bShowFullPath ;                  
   L_BOOL           bUseOriginalFolder ;             
   L_TCHAR  *       pszSrcFileList ;                  
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} FILECONVERSIONDLGPARAMS, * LPFILECONVERSIONDLGPARAMS ;
#else
typedef FILECONVERSIONDLGPARAMSA FILECONVERSIONDLGPARAMS;
typedef LPFILECONVERSIONDLGPARAMSA LPFILECONVERSIONDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

typedef struct _FILESASSOCIATIONDLGPARAMSA
{
   L_UINT           uStructSize ;
   L_CHAR *         pszFormats ; 
   L_CHAR *         pszSelectedExt ;
   L_CHAR *         pszServerAppName ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} FILESASSOCIATIONDLGPARAMSA, * LPFILESASSOCIATIONDLGPARAMSA ;

typedef struct _PRINTSTITCHEDIMAGESDLGPARAMSA
{
   L_UINT            uStructSize ;       
   HICON             hWindowIcon ;
   LPDLGBITMAPLISTA  pBitmapList ;
   HGLOBAL           hDevMode ;
   HGLOBAL           hDevNames ;
   RECT              rcMargins ;
   RECT              rcMinMargins ;
   L_INT             nCmdShow ;
   LPPOINT           pptPosition ;   
   LTCOMMDLGHELPCB   pfnHelpCallback ; 
   L_VOID  *         pHelpCallBackUserData ;

} PRINTSTITCHEDIMAGESDLGPARAMSA, * LPPRINTSTITCHEDIMAGESDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _FILESASSOCIATIONDLGPARAMS
{
   L_UINT           uStructSize ;
   L_TCHAR *        pszFormats ; 
   L_TCHAR *        pszSelectedExt ;
   L_TCHAR *        pszServerAppName ; 
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *         pHelpCallBackUserData ;

} FILESASSOCIATIONDLGPARAMS, * LPFILESASSOCIATIONDLGPARAMS ;

typedef struct _PRINTSTITCHEDIMAGESDLGPARAMS
{
   L_UINT            uStructSize ;       
   HICON             hWindowIcon ;
   LPDLGBITMAPLIST   pBitmapList ;
   HGLOBAL           hDevMode ;
   HGLOBAL           hDevNames ;
   RECT              rcMargins ;
   RECT              rcMinMargins ;
   L_INT             nCmdShow ;
   LPPOINT           pptPosition ;   
   LTCOMMDLGHELPCB   pfnHelpCallback ; 
   L_VOID  *         pHelpCallBackUserData ;

} PRINTSTITCHEDIMAGESDLGPARAMS, * LPPRINTSTITCHEDIMAGESDLGPARAMS ;

#else
typedef FILESASSOCIATIONDLGPARAMSA FILESASSOCIATIONDLGPARAMS;
typedef LPFILESASSOCIATIONDLGPARAMSA LPFILESASSOCIATIONDLGPARAMS;
typedef PRINTSTITCHEDIMAGESDLGPARAMSA PRINTSTITCHEDIMAGESDLGPARAMS;
typedef LPPRINTSTITCHEDIMAGESDLGPARAMSA LPPRINTSTITCHEDIMAGESDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

typedef struct _PRINTPREVIEWDLGPARAMS
{
   L_UINT          uStructSize ;    
   pBITMAPHANDLE   pBitmap ;        
   HICON           hWindowIcon ;
   HGLOBAL         hDevMode ;
   HGLOBAL         hDevNames ;
   RECT            rcMargins ;      
   RECT            rcMinMargins ;   
   L_INT           nCmdShow ;
   LPPOINT         pptPosition ;   
   LTCOMMDLGHELPCB pfnHelpCallback ; 
   L_VOID *        pHelpCallBackUserData ;

} PRINTPREVIEWDLGPARAMS, * LPPRINTPREVIEWDLGPARAMS ;

typedef struct _SAVEDLGPARAMSA
{
   L_UINT           uStructSize  ;                    
   L_CHAR           szFileName [ L_MAXPATH ] ;  
   pBITMAPHANDLE    pBitmap ;                   
   L_INT            nBitsPerPixel ;             
   L_INT            nFormat ;                   
   L_INT            nQFactor ;                  
   L_UINT           uSaveMulti ;                
   L_BOOL           bSaveInterlaced ;           
   L_INT            nPasses ;                   
   L_BOOL           bSaveWithStamp ;            
   L_INT            nStampBits ;                
   L_INT            nStampWidth ;               
   L_INT            nStampHeight ;              
   L_INT            nPageNumber ;               
   L_UINT           uFileTypeIndex ;            
   L_UINT           uSubTypeIndex ;             
   FILEJ2KOPTIONS   FileJ2KOptions ;
   LPFILESAVEFORMAT pFileFormats ;              
   L_UINT           uFileFormatsCount ;         
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *          pHelpCallBackUserData ;
   FILEJBIG2OPTIONS FileJBIG2Options ;
   INITIALVIEWENUM   InitialView;
#if defined(LEADTOOLS_V16_OR_LATER)
   PDFPROFILESENUM   PDFProfiles;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
} SAVEDLGPARAMSA, * LPSAVEDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _SAVEDLGPARAMS
{
   L_UINT           uStructSize  ;                    
   L_TCHAR          szFileName [ L_MAXPATH ] ;  
   pBITMAPHANDLE    pBitmap ;                   
   L_INT            nBitsPerPixel ;             
   L_INT            nFormat ;                   
   L_INT            nQFactor ;                  
   L_UINT           uSaveMulti ;                
   L_BOOL           bSaveInterlaced ;           
   L_INT            nPasses ;                   
   L_BOOL           bSaveWithStamp ;            
   L_INT            nStampBits ;                
   L_INT            nStampWidth ;               
   L_INT            nStampHeight ;              
   L_INT            nPageNumber ;               
   L_UINT           uFileTypeIndex ;            
   L_UINT           uSubTypeIndex ;             
   FILEJ2KOPTIONS   FileJ2KOptions ;
   LPFILESAVEFORMAT pFileFormats ;              
   L_UINT           uFileFormatsCount ;         
   L_UINT32         uDlgFlags ; 
   LPPOINT          pptPosition ;   
   LTCOMMDLGHELPCB  pfnHelpCallback ; 
   L_VOID *          pHelpCallBackUserData ;
   FILEJBIG2OPTIONS FileJBIG2Options ;
   INITIALVIEWENUM   InitialView;
#if defined(LEADTOOLS_V16_OR_LATER)
   PDFPROFILESENUM   PDFProfiles;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

} SAVEDLGPARAMS, * LPSAVEDLGPARAMS ;
#else
typedef SAVEDLGPARAMSA SAVEDLGPARAMS;
typedef LPSAVEDLGPARAMSA LPSAVEDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

// FILE_PCD
typedef struct _OPENDLGOPTIONS_PCD 
{
   L_UINT  uStructSize ;      
   L_INT nResolution ;  

} OPENDLGOPTIONS_PCD, * LPOPENDLGOPTIONS_PCD ;


// FILE_PDF
typedef struct _OPENDLGOPTIONS_PDF 
{
   L_UINT  uStructSize ;               
   FILEPDFOPTIONS PDFOptions ;   

} OPENDLGOPTIONS_PDF, * LPOPENDLGOPTIONS_PDF ;

// FILE_FPX, FILE_FPX_SINGLE_COLOR, FILE_FPX_JPEG, FILE_FPX_JPEG_QFACTOR, 
// FILE_CMW, FILE_JBIG, FILE_JP2, FILE_J2K, 
typedef struct _OPENDLGOPTIONS_RASTERMISC 
{
   L_UINT    uStructSize ;          
   L_UINT32  uXResolution ;   
   L_UINT32  uYResolution ;   

} OPENDLGOPTIONS_RASTERMISC, * LPOPENDLGOPTIONS_RASTERMISC ;

// FILE_WMF, FILE_EMF
typedef struct _OPENDLGOPTIONS_METAFILE 
{
   L_UINT   uStructSize ;     
   L_INT    nXResolution ; 
   L_INT    nYResolution ; 

} OPENDLGOPTIONS_METAFILE, * LPOPENDLGOPTIONS_METAFILE ;

// FILE_DWF, FILE_CGM, FILE_CMX, FILE_PCL, FILE_VECTOR_DUMP, FILE_PCT, FILE_DRW, 
// FILE_INTERGRAPH_VECTOR, FILE_GERBER, FILE_SHP, FILE_SVG, FILE_VWPG
typedef struct _OPENDLGOPTIONS_VECTORMISCA
{
   L_UINT  uStructSize ;                  
   L_INT   nViewportWidth ;         
   L_INT   nViewportHeight ;        
   L_UINT  uViewportMode ;          
   L_CHAR szFont [ LF_FACESIZE ] ; 

} OPENDLGOPTIONS_VECTORMISCA, * LPOPENDLGOPTIONS_VECTORMISCA ;

// FILE_DXF, FILE_DXF_R13, FILE_DWG
typedef struct _OPENDLGOPTIONS_DXFA
{
   L_UINT   uStructSize ;                    
   L_INT    nViewportWidth ;           
   L_INT    nViewportHeight ;          
   L_UINT   uViewportMode ;            
   L_CHAR   szFont [ LF_FACESIZE ] ;   
   L_UINT32 uAutoCADColorScheme ;     

} OPENDLGOPTIONS_DXFA, * LPOPENDLGOPTIONS_DXFA ;

// PLT
typedef struct _OPENDLGOPTIONS_PLTA 
{
   L_UINT         uStructSize ;                    
   L_INT          nViewportWidth ;           
   L_INT          nViewportHeight ;          
   L_UINT         uViewportMode ;            
   L_CHAR         szFont [ LF_FACESIZE ] ;   
   FILEPLTOPTIONS PLTOptions ;               
 
} OPENDLGOPTIONS_PLTA, * LPOPENDLGOPTIONS_PLTA ;
#if defined(FOR_UNICODE)
// FILE_DWF, FILE_CGM, FILE_CMX, FILE_PCL, FILE_VECTOR_DUMP, FILE_PCT, FILE_DRW, 
// FILE_INTERGRAPH_VECTOR, FILE_GERBER, FILE_SHP, FILE_SVG, FILE_VWPG
typedef struct _OPENDLGOPTIONS_VECTORMISC
{
   L_UINT  uStructSize ;                  
   L_INT   nViewportWidth ;         
   L_INT   nViewportHeight ;        
   L_UINT  uViewportMode ;          
   L_TCHAR szFont [ LF_FACESIZE ] ; 

} OPENDLGOPTIONS_VECTORMISC, * LPOPENDLGOPTIONS_VECTORMISC ;

// FILE_DXF, FILE_DXF_R13, FILE_DWG
typedef struct _OPENDLGOPTIONS_DXF
{
   L_UINT   uStructSize ;                    
   L_INT    nViewportWidth ;           
   L_INT    nViewportHeight ;          
   L_UINT   uViewportMode ;            
   L_TCHAR  szFont [ LF_FACESIZE ] ;   
   L_UINT32 uAutoCADColorScheme ;     

} OPENDLGOPTIONS_DXF, * LPOPENDLGOPTIONS_DXF ;

// PLT
typedef struct _OPENDLGOPTIONS_PLT 
{
   L_UINT         uStructSize ;                    
   L_INT          nViewportWidth ;           
   L_INT          nViewportHeight ;          
   L_UINT         uViewportMode ;            
   L_TCHAR        szFont [ LF_FACESIZE ] ;   
   FILEPLTOPTIONS PLTOptions ;               
 
} OPENDLGOPTIONS_PLT, * LPOPENDLGOPTIONS_PLT ;
#else
typedef OPENDLGOPTIONS_PLTA OPENDLGOPTIONS_PLT;
typedef LPOPENDLGOPTIONS_PLTA LPOPENDLGOPTIONS_PLT;
typedef OPENDLGOPTIONS_DXFA OPENDLGOPTIONS_DXF;
typedef LPOPENDLGOPTIONS_DXFA LPOPENDLGOPTIONS_DXF;
typedef OPENDLGOPTIONS_VECTORMISCA OPENDLGOPTIONS_VECTORMISC;
typedef LPOPENDLGOPTIONS_VECTORMISCA LPOPENDLGOPTIONS_VECTORMISC;
#endif // #if defined(FOR_UNICODE)

typedef struct _OPENDLGOPTIONS_XPS 
{
   L_UINT  uStructSize ;               
   FILEXPSOPTIONS XPSOptions ;   

} OPENDLGOPTIONS_XPS, * LPOPENDLGOPTIONS_XPS ;

#if defined(LEADTOOLS_V16_OR_LATER)
typedef struct _OPENDLGOPTIONS_XLS 
{
   L_UINT  uStructSize ;
   FILEXLSOPTIONS XLSOptions ;
} OPENDLGOPTIONS_XLS, * LPOPENDLGOPTIONS_XLS ;

#endif // #if defined(LEADTOOLS_V16_OR_LATER)

typedef struct _OPENDLGOPTIONS
{
   L_UINT  uStructSize ;    
   L_INT   nType ;    
                      
   LPVOID  pOptions ; 

} OPENDLGOPTIONS, * LPOPENDLGOPTIONS ;

typedef struct _OPENDLGFILEDATAA
{  
   L_UINT         uStructSize ;                     
   L_CHAR         szFileName [ L_MAXPATH ] ;  
   pBITMAPHANDLE  pBitmap ;             
   pBITMAPHANDLE  pThumbnail ;          
   pFILEINFOA     pFileInfo ;               
   L_INT          nPageNumber ;                 
   L_INT          nPasses ;                     
   L_BOOL         bLoadCompressed ;            
   L_BOOL         bLoadRotated ;               
   OPENDLGOPTIONS FileOptions ;        
#if defined(LEADTOOLS_V16_OR_LATER)
   RASTERIZEDOCOPTIONS RasterizeDocOptions;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
} OPENDLGFILEDATAA, * LPOPENDLGFILEDATAA ;

#if defined(FOR_UNICODE)
typedef struct _OPENDLGFILEDATA
{  
   L_UINT         uStructSize ;                     
   L_TCHAR        szFileName [ L_MAXPATH ] ;  
   pBITMAPHANDLE  pBitmap ;             
   pBITMAPHANDLE  pThumbnail ;          
   pFILEINFO      pFileInfo ;               
   L_INT          nPageNumber ;                 
   L_INT          nPasses ;                     
   L_BOOL         bLoadCompressed ;            
   L_BOOL         bLoadRotated ;               
   OPENDLGOPTIONS FileOptions ;        
#if defined(LEADTOOLS_V16_OR_LATER)
   RASTERIZEDOCOPTIONS RasterizeDocOptions;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
} OPENDLGFILEDATA, * LPOPENDLGFILEDATA ;
#else
typedef OPENDLGFILEDATAA OPENDLGFILEDATA;
typedef LPOPENDLGFILEDATAA LPOPENDLGFILEDATA;
#endif // #if defined(FOR_UNICODE)

// OPEN dialog file(s) loading callback
typedef L_INT ( pEXT_CALLBACK OPENDLGFILELOADCALLBACKA ) ( LPOPENDLGFILEDATAA lpFileData, 
                                                          L_INT             nTotalPercent,
                                                          L_INT             nFilePercent,
                                                          L_VOID  *         lpUserData ) ;
#if defined(FOR_UNICODE)
typedef L_INT ( pEXT_CALLBACK OPENDLGFILELOADCALLBACK ) ( LPOPENDLGFILEDATA lpFileData, 
                                                          L_INT             nTotalPercent,
                                                          L_INT             nFilePercent,
                                                          L_VOID  *         lpUserData ) ;
#else
typedef OPENDLGFILELOADCALLBACKA OPENDLGFILELOADCALLBACK;
#endif // #if defined(FOR_UNICODE)
typedef struct _OPENDLGPARAMSA
{
   L_UINT                     uStructSize ;                  
   LPOPENDLGFILEDATAA         pFileData ;                    
   L_INT                      nNumOfFiles ;                  
   L_BOOL                     bPreviewEnabled ;              
   L_BOOL                     bShowLoadOptions ;             
   LTCOMMDLGFILEPREVIEWDATACB pfnFilePreviewData ;
   L_VOID *                   pFilePreviewCallBackUserData ;
   LTCOMMDLGIMAGEFILEINFOCB   pfnImageFileInfo ;
   L_VOID *                   pImageFileInfoCallBackUserData ;
   OPENDLGFILELOADCALLBACKA   pfnFileLoadCallback ;          
   L_VOID *                   pFileLoadCallbackUserData ;    
   L_UINT32                   uDlgFlags ;                    
   LPPOINT                    pptPosition ;   
   LTCOMMDLGHELPCB            pfnHelpCallback ;              
   L_VOID *                   pHelpCallBackUserData ;
   INITIALVIEWENUM            InitialView;

} OPENDLGPARAMSA, * LPOPENDLGPARAMSA ;

typedef struct _ICCPROFILEDLGPARAMSA
{
   L_UINT            uStructSize ;
   ICCPROFILEEXT     ICCProfile ;
   L_CHAR            szLoadFile [ L_MAXPATH ] ;
   L_CHAR            szSaveFile [ L_MAXPATH ] ;
   L_UINT32          uDlgFlags ;                    
   LPPOINT           pptPosition ;   
   LTCOMMDLGHELPCB   pfnHelpCallback ;              
   L_VOID *          pHelpCallBackUserData ;        

} ICCPROFILEDLGPARAMSA, * LPICCPROFILEDLGPARAMSA ;

#if defined(FOR_UNICODE)
typedef struct _OPENDLGPARAMS 
{
   L_UINT                     uStructSize ;                  
   LPOPENDLGFILEDATA          pFileData ;                    
   L_INT                      nNumOfFiles ;                  
   L_BOOL                     bPreviewEnabled ;              
   L_BOOL                     bShowLoadOptions ;             
   LTCOMMDLGFILEPREVIEWDATACB pfnFilePreviewData ;
   L_VOID *                   pFilePreviewCallBackUserData ;
   LTCOMMDLGIMAGEFILEINFOCB   pfnImageFileInfo ;
   L_VOID *                   pImageFileInfoCallBackUserData ;
   OPENDLGFILELOADCALLBACK    pfnFileLoadCallback ;          
   L_VOID *                   pFileLoadCallbackUserData ;    
   L_UINT32                   uDlgFlags ;                    
   LPPOINT                    pptPosition ;   
   LTCOMMDLGHELPCB            pfnHelpCallback ;              
   L_VOID *                   pHelpCallBackUserData ;
   INITIALVIEWENUM            InitialView;
} OPENDLGPARAMS, * LPOPENDLGPARAMS ;

typedef struct _ICCPROFILEDLGPARAMS
{
   L_UINT            uStructSize ;
   ICCPROFILEEXT     ICCProfile ;
   L_TCHAR           szLoadFile [ L_MAXPATH ] ;
   L_TCHAR           szSaveFile [ L_MAXPATH ] ;
   L_UINT32          uDlgFlags ;                    
   LPPOINT           pptPosition ;   
   LTCOMMDLGHELPCB   pfnHelpCallback ;              
   L_VOID *          pHelpCallBackUserData ;        

} ICCPROFILEDLGPARAMS, * LPICCPROFILEDLGPARAMS ;
#else
typedef OPENDLGPARAMSA OPENDLGPARAMS;
typedef LPOPENDLGPARAMSA LPOPENDLGPARAMS;
typedef ICCPROFILEDLGPARAMSA ICCPROFILEDLGPARAMS;
typedef LPICCPROFILEDLGPARAMSA LPICCPROFILEDLGPARAMS;
#endif // #if defined(FOR_UNICODE)

//.............................................................................
// enums, defines and structures
//.............................................................................


//.............................................................................
// Functions
//.............................................................................

L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetDirectoryA ( HWND hWndOwner,
                                       LPGETDIRECTORYDLGPARAMSA pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgFileConversionA ( HWND hWndOwner,
                                         LPFILECONVERSIONDLGPARAMSA pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgFilesAssociationA ( HWND hWndOwner, 
                                           LPFILESASSOCIATIONDLGPARAMSA pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPrintStitchedImagesA ( HWND hWndOwner,
                                              LPPRINTSTITCHEDIMAGESDLGPARAMSA pDlgParams ) ;

#if defined(FOR_UNICODE)
L_LTDLG_API L_INT EXT_FUNCTION L_DlgGetDirectory ( HWND hWndOwner,
                                       LPGETDIRECTORYDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgFileConversion ( HWND hWndOwner,
                                         LPFILECONVERSIONDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgFilesAssociation ( HWND hWndOwner, 
                                           LPFILESASSOCIATIONDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPrintStitchedImages ( HWND hWndOwner,
                                              LPPRINTSTITCHEDIMAGESDLGPARAMS pDlgParams ) ;
#else
#define L_DlgGetDirectory L_DlgGetDirectoryA
#define L_DlgFileConversion   L_DlgFileConversionA
#define L_DlgFilesAssociation L_DlgFilesAssociationA
#define L_DlgPrintStitchedImages L_DlgPrintStitchedImagesA
#endif // #if defined(FOR_UNICODE)

L_LTDLG_API L_INT EXT_FUNCTION L_DlgPrintPreview ( HWND hWndOwner,
                                       LPPRINTPREVIEWDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgSaveA ( HWND hWndOwner,
                               LPOPENFILENAMEA pOpenFileName,
                               LPSAVEDLGPARAMSA pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgOpenA ( HWND hWndOwner,
                               LPOPENFILENAMEA pOpenFileName,
                               LPOPENDLGPARAMSA pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgICCProfileA ( HWND hWndOwner,
                                     LPICCPROFILEDLGPARAMSA pDlgParams ) ;

#if defined(FOR_UNICODE)
L_LTDLG_API L_INT EXT_FUNCTION L_DlgSave ( HWND hWndOwner,
                               LPOPENFILENAMEW pOpenFileName,
                               LPSAVEDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgOpen ( HWND hWndOwner,
                               LPOPENFILENAMEW pOpenFileName,
                               LPOPENDLGPARAMS pDlgParams ) ;

L_LTDLG_API L_INT EXT_FUNCTION L_DlgICCProfile ( HWND hWndOwner,
                                     LPICCPROFILEDLGPARAMS pDlgParams ) ;
#else
#define L_DlgSave L_DlgSaveA
#define L_DlgOpen L_DlgOpenA
#define L_DlgICCProfile L_DlgICCProfileA
#endif // #if defined(FOR_UNICODE)
//.............................................................................
// Functions
//.............................................................................

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDLG_FILE_H)
