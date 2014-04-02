/*************************************************************
   Ltkrn.h - LEADTOOLS runtime library
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTKRN_H)
#define LTKRN_H

#if !defined(L_LTKRN_API)
   #define L_LTKRN_API
#endif // #if !defined(L_LTKRN_API)

#if !defined(LEAD_DEFINES_ONLY)
#include "Lttyp.h"
#include "Lterr.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

#include "Ltlck.h"
#endif // !defined(LEAD_DEFINES_ONLY)

/****************************************************************
   Enums/defines/macros
****************************************************************/

// flags for L_CombineBitmapWarp
#define CBW_BILINEAR    0x0001
#define CBW_RESERVED1   0x0002
#define CBW_RESERVED2   0x0004
#define CBW_RESERVED3   0x0008
#define CBW_RESERVED4   0x0010
#define CBW_RESERVED5   0x0020
#define CBW_RESERVED6   0x0040
#define CBW_RESERVED7   0x0080
#define CBW_RESERVED8   0x0100
#define CBW_RESERVED9   0x0200
#define CBW_ALLFLAGS    (CBW_BILINEAR | CBW_RESERVED1 | CBW_RESERVED2 | CBW_RESERVED3 | \
                        CBW_RESERVED4 | CBW_RESERVED5 | CBW_RESERVED6 | CBW_RESERVED7 | \
                        CBW_RESERVED8 | CBW_RESERVED9)

// flags for L_ScrambleBitmap
#define SB_ENCRYPT      0x0001
#define SB_DECRYPT      0x0002
#define SB_INTERSECT    0x0004
#define SB_RESERVED3    0x0008
#define SB_RESERVED4    0x0010
#define SB_RESERVED5    0x0020
#define SB_RESERVED6    0x0040
#define SB_RESERVED7    0x0080
#define SB_RESERVED8    0x0100
#define SB_RESERVED9    0x0200
#define SB_ALLFLAGS     (SB_ENCRYPT   | SB_DECRYPT   | SB_INTERSECT | SB_RESERVED3 | \
                         SB_RESERVED4 | SB_RESERVED5 | SB_RESERVED6 | SB_RESERVED7 | \
                         SB_RESERVED8 | SB_RESERVED9)

// flags for L_ColorResBitmap
#define CRF_FIXEDPALETTE            0x00000001  // versus custom palette
#define CRF_OPTIMIZEDPALETTE        0x00000002  // versus fixed palette
#define CRF_IDENTITYPALETTE         0x00000008  // versus ALL 256 colors
#define CRF_USERPALETTE             0x00000010  // user generated palette
#define CRF_FASTMATCHPALETTE        0x00000020  // With user, for fast color match
#define CRF_NETSCAPEPALETTE         0x00000040  // use netscape fixed palette
#define CRF_BYTEORDERBGR            0x00000004  // versus RGB
#define CRF_BYTEORDERRGB            0x00000000  // versus BGR
#define CRF_BYTEORDERGRAY           0x00000080  // Convert to grayscale (12 & 16 bit only)
#define CRF_PALETTEOPTIONS          0x000003FF  // Mask to find palette option
#define CRF_USERHPALETTE            0x00000100  // user generated palette
#define CRF_SLOWMATCH               0x00000200  // use with CRF_OPTIMIZEDPALETTE to use
                                                // a slower but more accurate match
#define CRF_FAVORPURECOLORS         0x00000400  // Give colors that are very numerous palette entries
                                                // of their own
#define CRF_BYTEORDERROMM           0x00000800  // ROMM color space (BGR wide gamut)
#define CRF_USELUT                  0x00001000  // (Internal flag) Use the grayscale LUT
#define CRF_EXACTCOLORS             0x00002000  // (Internal flag) The output palette has all the colors in the bitmap. No dithering is needed.

#define CRF_NODITHERING             0x00000000  // No Dithering
#define CRF_FLOYDSTEINDITHERING     0x00010000  // Floyd Stein Dithering
#define CRF_STUCKIDITHERING         0x00020000  // Stucki Dithering
#define CRF_BURKESDITHERING         0x00030000  // Burkes Dithering
#define CRF_SIERRADITHERING         0x00040000  // Sierra Dithering
#define CRF_STEVENSONARCEDITHERING  0x00050000  // Stevenson Arce Dithering
#define CRF_JARVISDITHERING         0x00060000  // Jarvis Dithering
#define CRF_ORDEREDDITHERING        0x00070000  // Ordered Dithering
#define CRF_CLUSTEREDDITHERING      0x00080000  // Clustered Dithering
#define CRF_DITHERINGOPTIONS        0x00FF0000  // Mask to find dither option

#define CRF_ALPHAINIT               0x01000000  // initialize alpha channel to 0xFF

#define RGB_RESERVED 0x01 // Do not map to the entry
#define RGB_EMPTY    0x02 // The entry is empty


// useful define for copying the source to the destination
#define L_SRC_COPY      (CB_DST_0 | CB_SRC_NOP | CB_OP_OR)

// return values of L_IsGrayScaleBitmap
#define GRAY_NO               0  // not grayscale
#define GRAY_ORDEREDNORMAL    1  // contains a normally ordered grayscale palette
#define GRAY_ORDEREDINVERSE   2  // contains a reverse ordered grayscale palette
#define GRAY_NOTORDERED       3  // contains a non-ordered grayscale palette

// flags for L_VersionInfo
#define TOOLKIT_LEVEL_DOCUMENT   0x00  // Document Toolkit
#define TOOLKIT_LEVEL_RASTER     0x01  // Raster Toolkit
#define TOOLKIT_LEVEL_MEDICAL    0x02  // Medical Toolkit
#define TOOLKIT_LEVEL_EVAL       0x03  // Evaluation Toolkit
#define TOOLKIT_LEVEL_NAG        0x04  // Nag Toolkit

// Dithering methods. To be used for DitheringMethod in BITMAPHANDLE.
#define NO_DITHERING                0
#define FLOYD_STEIN_DITHERING       1
#define STUCKI_DITHERING            2
#define BURKES_DITHERING            3
#define SIERRA_DITHERING            4
#define STEVENSON_ARCE_DITHERING    5
#define JARVIS_DITHERING            6
#define ORDERED_DITHERING           7
#define CLUSTERED_DITHERING         8
#define LAST_DITHERING              8

// bitmap data colors order
#define ORDER_RGB                0  // RGB ordering
#define ORDER_BGR                1  // BGR ordering
#define ORDER_GRAY               2  // Gray pixels
#define ORDER_RGBORGRAY          3  // Valid only for L_LoadXXX functions - gray or RGB
#define ORDER_BGRORGRAY          4  // Valid only for L_LoadXXX functions - gray or BGR
#define ORDER_ROMM               5  // BGR Wide gamut format
#define ORDER_BGRORGRAYORROMM    6  // Valid only for L_LoadXXX functions - gray or BGR or ROMM

// view perspectives
#define TOP_LEFT        1  // Top-left of image is first in memory
#define BOTTOM_LEFT     4  // Bottom-left of image is first in memory

#define TOP_RIGHT       2  // Top-right of image is first in memory
#define BOTTOM_LEFT180  2  // TOP_RIGHT is BOTTOM_LEFT rotated clockwise by 180 degrees
#define BOTTOM_RIGHT    3  // Bottom-right of image is first in memory
#define TOP_LEFT180     3  // BOTTOM_RIGHT is TOP_LEFT rotated clockwise by 180 degrees

#define RIGHT_TOP       6  // First row is the right side, first column is top side
#define TOP_LEFT90      6  // RIGHT_TOP is TOP_LEFT rotated clockwise by 90 degrees 
#define LEFT_BOTTOM     8  // First row is the left side, first column is top side
#define TOP_LEFT270     8  // LEFT_BOTTOM is TOP_LEFT rotated clockwise by 270 degrees 

#define LEFT_TOP        5  // First row is the left side, first column is the top side
#define BOTTOM_LEFT90   5  // LEFT_TOP is BOTTOM_LEFT rotated clockwise by 90 degrees
#define RIGHT_BOTTOM    7  // First row is the right side, first column is the bottom side
#define BOTTOM_LEFT270  7  // RIGHT_BOTTOM is BOTTOM_LEFT rotated clockwise by 270 degrees

// memory allocation flags
#define TYPE_CONV             0x0001   // Use conventional memory first
#define TYPE_USER             0x0002   // Create a bitmap where the user maintains the data pointer
#define TYPE_TILED            0x0004   // Create tiled bitmap first
#define TYPE_NOTILED          0x0008   // Do not use tiled bitmaps
#define TYPE_DISK             0x0080   // Use Disk (if possible)
#define TYPE_NODISK           0x0100   // Do not use disk
#define TYPE_COMPRESSED       0x0200   // keep 1 bit images compressed in memory
#define TYPE_SUPERCOMPRESSED  0x0400   // keep 24 bit images compressed in memory - slow access

// flags for L_SizeBitmap and L_ResizeBitmap
#define SIZE_NORMAL        0x0000
#define SIZE_FAVORBLACK    0x0001
#define SIZE_RESAMPLE      0x0002
#define SIZE_BICUBIC       0x0004

// flags for L_ConvertBufferExt
#define CVT_SRCGRAY     0x0001
#define CVT_DSTGRAY     0x0002
#define CVT_SRCUSEBITS  0x0004
#define CVT_DSTUSEBITS  0x0008
#define CVT_USEALPHA    0x0010
#define CVT_ALPHAINIT   0x0020

// flags for L_RotateBitmap
#define ROTATE_RESIZE      0x0001
#define ROTATE_RESAMPLE    0x0002
#define ROTATE_BICUBIC     0x0004
#define ROTATE_OFFCENTER   0x0008   // reserved for internal use

// flags for L_StartResizeBitmap
//-------- ColorRes flags
#define RES_ORDERRGB                0x00000000
#define RES_ORDERBGR                0x00000004
#define RES_ORDERGRAY               0x00000080
#define RES_ORDERROMM               0x00000800

//-------- Resize flags
#define RES_NORMAL                  0x00000000
#define RES_RESAMPLE                0x00000010
#define RES_BICUBIC                 0x00000020

//-------- Dithering flags
#define RES_NODITHERING             CRF_NODITHERING            //  0x00000000 No Dithering
#define RES_FLOYDSTEINDITHERING     CRF_FLOYDSTEINDITHERING    //  0x00010000 Floyd Stein Dithering
#define RES_STUCKIDITHERING         CRF_STUCKIDITHERING        //  0x00020000 Stucki Dithering
#define RES_BURKESDITHERING         CRF_BURKESDITHERING        //  0x00030000 Burkes Dithering
#define RES_SIERRADITHERING         CRF_SIERRADITHERING        //  0x00040000 Sierra Dithering
#define RES_STEVENSONARCEDITHERING  CRF_STEVENSONARCEDITHERING //  0x00050000 Stevenson Arce Dithering
#define RES_JARVISDITHERING         CRF_JARVISDITHERING        //  0x00060000 Jarvis Dithering
#define RES_ORDEREDDITHERING        CRF_ORDEREDDITHERING       //  0x00070000 Ordered Dithering
#define RES_CLUSTEREDDITHERING      CRF_CLUSTEREDDITHERING     //  0x00080000 Clustered Dithering
#define RES_DITHERINGOPTIONS        CRF_DITHERINGOPTIONS       //  0x00FF0000 Mask to find dither option

#define RES_USELUT                  CRF_USELUT                 // 0x00001000 (Internal flag) Use the grayscale LUT

// constants for L_ConvertToDIB
#define DIB_BITMAPINFOHEADER  0
#define DIB_BITMAPV4HEADER    1
#define DIB_BITMAPV5HEADER    2

// compression types for L_ChangeBitmapCompression
#define COMP_NONE    0
#define COMP_RLE     1
#define COMP_SUPER   2

// Flags for L_SetBitmapMemoryInfo
#define SETMEM_MEMORY         0x0001
#define SETMEM_TILESIZE       0x0002
#define SETMEM_TOTALTILES     0x0004
#define SETMEM_CONVTILES      0x0008
#define SETMEM_MAXTILEVIEWS   0x0010
#define SETMEM_TILEVIEWS      0x0020

// Flags for L_SetMemoryThresholds
#define MEM_TILEDTHRESHOLD    0x0001
#define MEM_MAXCONVSIZE       0x0002
#define MEM_TILESIZE          0x0004
#define MEM_CONVTILES         0x0008
#define MEM_CONVBUFFERS       0x0010
#define MEM_ALL               0x001F

#define MAX_OVERLAYS 16 // the maximum number of overlays

// Flags which make up OVERLAYATTRIBUTES.uFlags
#define OVERLAY_AUTOPAINT     0x00000001  // if set, this overlay will be painted automatically
                                          // when the bitmap is painted using L_PaintDC functions
#define OVERLAY_AUTOPROCESS   0x00000002  // if set, the overlay bitmap and position 
                                          // are automatically adjusted when the main bitmap is:
                                          // resized (zoomed in), scrolled, rotated, 
                                          // flipped, reversed, sheared, trimmed
#define OVERLAY_USEBITPLANE   0x00000004  // the overlay should be embedded inside the image data
#define OVERLAY_RGNCREATED    0x80000000  // Internal Flag

// Flags for L_SetOverlayAttributes and L_GetOverlayAttributes
#define OVERLAYATTRIBUTES_ORIGIN    0x0001   // Indicates the overlay origin should be retrieved/set.
#define OVERLAYATTRIBUTES_COLOR     0x0002   // Indicates the color should be retrieved/set.
#define OVERLAYATTRIBUTES_FLAGS     0x0004   // Indicates the flags in the OVERLAYATTRIBUTES.uFlags should
                                             // be retrieved/set.
#define OVERLAYATTRIBUTES_BITINDEX  0x0008   // Indicates the bit position should be retrieved/set.
#define OVERLAYATTRIBUTES_DICOM     0x0010   // Indicates that DICOM-related attributes should be retrieved/set.

// Flags for L_SetOverlayBitmap and L_GetOverlayBitmap
#define OVERLAY_COPY    0x0000   // if set, a copy is made
#define OVERLAY_NOCOPY  0x0001   // if set, the copy is not made. both pOverlayBitmap and the
                                 // overlay bitmap will point to the same bitmap
#define OVERLAY_MOVE    0x0003   // if set, pOverlayBitmap is moved to/from the overlay list. 
                                 //  This is recommended over OVERLAY_NOCOPY

// Flags for L_UpdateBitmapOverlayBits
#define SETOVERLAYBITS_FROMOVERLAY  0x0001
#define SETOVERLAYBITS_FROMBITMAP   0x0002
#define SETOVERLAYBITS_CLEAR        0x0004

#define DICOM_VR_LO_LENGTH 64
#define DICOM_VR_UI_LENGTH 64
#define DICOM_VR_CS_LENGTH 16

#if !defined(LEAD_DEFINES_ONLY)

// helper macros for dealing with rotated bitmap view perspectives
#define ISBASIC(pBitmap) ((pBitmap)->ViewPerspective == TOP_LEFT || (pBitmap)->ViewPerspective == BOTTOM_LEFT)
#define ISROTATED(pBitmap) ((pBitmap)->ViewPerspective == TOP_LEFT90 || (pBitmap)->ViewPerspective == TOP_LEFT270 || (pBitmap)->ViewPerspective == BOTTOM_LEFT90 || (pBitmap)->ViewPerspective == BOTTOM_LEFT270)
#define ISFLIPPED(pBitmap) ((pBitmap)->ViewPerspective == BOTTOM_LEFT || (pBitmap)->ViewPerspective == BOTTOM_LEFT90 || (pBitmap)->ViewPerspective == BOTTOM_LEFT180 || (pBitmap)->ViewPerspective == BOTTOM_LEFT270)
#define BITMAPWIDTH(pBitmap) (ISROTATED(pBitmap)?(pBitmap)->Height:(pBitmap)->Width)
#define BITMAPHEIGHT(pBitmap) (ISROTATED(pBitmap)?(pBitmap)->Width:(pBitmap)->Height)

// helper macro to determine whether a bitmap is 12 or 16 bit grayscale
#define ISGRAY(pBitmap) ((pBitmap)->Order == ORDER_GRAY)
#define ISGRAY32(pBitmap) (((pBitmap)->Order == ORDER_GRAY) && ((pBitmap)->BitsPerPixel == 32))

// flag that you can use to detect when a pixel color is 16 bit grayscale
#define COLORREF_GRAY16              0x04000000L
#define L_IS_COLORREF_GRAY16(clr)    ((clr & 0xFF000000) == COLORREF_GRAY16)
#define L_IS_COLORREF_INDEXED1(clr)  ((clr & 0xFF000000) == 0x01000000L)
#define L_IS_COLORREF_INDEXED2(clr)  ((clr & 0xFF000000) == 0x80000000L)

// playback handle
L_DECLARE_HANDLE(HPLAYBACK);
typedef HPLAYBACK* pHPLAYBACK;

// bitmap list handle
L_DECLARE_HANDLE(HBITMAPLIST);
typedef HBITMAPLIST* pHBITMAPLIST;

#define VECTOR_SUBSTITUTE_FONT_FOUND   0x0001

/****************************************************************
   Classes/structures
****************************************************************/

typedef struct _DICOMLUTDESCRIPTOR
{
   L_UINT uStructSize;
   L_UINT uNumberOfEntries;
   L_INT nFirstStoredPixelValueMapped; // Signed short or unsigned short, integer can hold either
   L_UINT uEntryBits;
}
DICOMLUTDESCRIPTOR, *pDICOMLUTDESCRIPTOR;

typedef struct _OVERLAYATTRIBUTESA
{
   L_UINT uStructSize;                                // The size of the structure (use sizeof(OVERLAYATTRIBUTES))
   L_POINT ptOrigin;                                  // The top-left offset (in pixels) for the overlay bitmap
   L_COLORREF crColor;                                // The color in which the bits with a value of 1 should be displayed
   L_UINT uFlags;                                     // One or more of the OVERLAY_XXX flags
   L_UINT16 uBitPosition;                             // The associated bit plane
   L_UINT16 uRows;                                    // Number of rows in overlay.
   L_UINT16 uColumns;                                 // Number of columns in overlay.
   L_CHAR szType[DICOM_VR_CS_LENGTH + 1];             // Overlay type
   L_UINT16 uBitsAllocated;                           // Number of Bits Allocated in the Overlay.
   L_CHAR szDescription[DICOM_VR_LO_LENGTH + 1];      // User-defined comments about the overlay.
   L_CHAR szSubtype[DICOM_VR_LO_LENGTH + 1];          // Defined term which identifies the intended purpose of the Overlay Type.
   L_CHAR szLabel[DICOM_VR_LO_LENGTH + 1];            // A user defined text string which may be used to label or name this overlay.
   L_INT32 nROIArea;                                  // Number of pixels in ROI area.
   L_DOUBLE fROIMean;                                 // ROI Mean.
   L_DOUBLE fROIStandardDeviation;                    // ROI standard deviation.
   L_INT32 nNumFramesInOverlay;                       // Number of Frames in Overlay
   L_UINT16 uImageFrameOrigin;                        // Frame number of Multi-frame Image to which this overlay applies
   L_CHAR szActivationLayer[DICOM_VR_CS_LENGTH + 1];  // The layer in which the overlay shall be displayed.
}
OVERLAYATTRIBUTESA, *pOVERLAYATTRIBUTESA;

#if defined(FOR_UNICODE)
typedef struct _OVERLAYATTRIBUTES
{
   L_UINT uStructSize;                                // The size of the structure (use sizeof(OVERLAYATTRIBUTES))
   L_POINT ptOrigin;                                  // The top-left offset (in pixels) for the overlay bitmap
   L_COLORREF crColor;                                // The color in which the bits with a value of 1 should be displayed
   L_UINT uFlags;                                     // One or more of the OVERLAY_XXX flags
   L_UINT16 uBitPosition;                             // The associated bit plane
   L_UINT16 uRows;                                    // Number of rows in overlay.
   L_UINT16 uColumns;                                 // Number of columns in overlay.
   L_TCHAR szType[DICOM_VR_CS_LENGTH + 1];            // Overlay type
   L_UINT16 uBitsAllocated;                           // Number of Bits Allocated in the Overlay.
   L_TCHAR szDescription[DICOM_VR_LO_LENGTH + 1];     // User-defined comments about the overlay.
   L_TCHAR szSubtype[DICOM_VR_LO_LENGTH + 1];         // Defined term which identifies the intended purpose of the Overlay Type.
   L_TCHAR szLabel[DICOM_VR_LO_LENGTH + 1];           // A user defined text string which may be used to label or name this overlay.
   L_INT32 nROIArea;                                  // Number of pixels in ROI area.
   L_DOUBLE fROIMean;                                 // ROI Mean.
   L_DOUBLE fROIStandardDeviation;                    // ROI standard deviation.
   L_INT32 nNumFramesInOverlay;                       // Number of Frames in Overlay
   L_UINT16 uImageFrameOrigin;                        // Frame number of Multi-frame Image to which this overlay applies
   L_TCHAR szActivationLayer[DICOM_VR_CS_LENGTH + 1]; // The layer in which the overlay shall be displayed.
}
OVERLAYATTRIBUTES, *pOVERLAYATTRIBUTES;
#else
typedef OVERLAYATTRIBUTESA OVERLAYATTRIBUTES;
typedef pOVERLAYATTRIBUTESA pOVERLAYATTRIBUTES;
#endif // #if defined(FOR_UNICODE)

typedef struct _BITMAPHANDLE
{
   L_UINT uStructSize;

   union
   {
      struct
      {
         L_HANDLE hData;  // internal use
         L_VOID **ppData; // internal use
      }
      Compressed;

      struct
      {
         L_HANDLE hData;  // Internal use
         L_UCHAR* pData;   // Internal use
      }
      Windows;

      struct
      {
         L_HFILE fd;         // Internal use
         L_UINT Available; // Internal use
      }
      File;

      #if !defined(FOR_WINCE)
      struct
      {
         L_HANDLE hData;  // Internal use
         L_UCHAR* pData;   // Internal use
      }
      Tiled;
      #endif //#if !defined(FOR_WINCE)

      struct
      {
         L_HANDLE hData;  // internal use
         L_VOID* pData;    // internal use
      }
      SuperCompressed;
   }
   Addr;

   L_INT Width;                 // Image width
   L_INT Height;                // Image height
   L_INT BitsPerPixel;          // Number of bits per pixel
   L_UINT BytesPerLine;         // Bytes per line, rounded by four
   L_SIZE_T CurrentPos;         // Internal use
   L_INT CurrentRow;            // Internal use

   struct _BitFlags
   {
      L_BITFIELD Allocated:1;             // Is the bitmap allocated?
      L_BITFIELD ConventionalMemory:1;    // allocated in conventional memory
      L_BITFIELD DiskMemory:1;            // allocated on disk
      L_BITFIELD ProgressiveAvailable:1;  // progressive format
      L_BITFIELD LockedMemory:1;          // Is memory locked in windows
      L_BITFIELD Interlaced:1;            // Interlaced format
      L_BITFIELD WaitUserInput:1;         // Playback wait for user input
      L_BITFIELD Transparency:1;          // Playback uses transparency
      L_BITFIELD Compressed:1;            // 1 bit RLE compressed data
      L_BITFIELD Signed:1;                // the pixel values are signed
      L_BITFIELD Mirror:1;                // this is a mirror of another image
      L_BITFIELD UseLUT:1;                // use pLUT for image processing
      L_BITFIELD Global:1;                // hData is HGLOBAL
      L_BITFIELD UsePaintLUT:1;           // Use PaintGamma, PaintContrast, PaintIntensity
      L_BITFIELD Tiled:1;                 // Bitmap is stored as tiles
      L_BITFIELD SuperCompressed:1;       // Super compressed data
      L_BITFIELD NoClip:1;                // Do not clip region
      L_BITFIELD Dummy:15;
   }
   Flags;

   L_INT nColors;                // No. of possible colors in the bitmap
   L_RGBQUAD* pPalette;          // Pointer to a L_RGBQUAD array containing the palette

   L_INT ViewPerspective;        // Is TOP_LEFT, BOTTOM_RIGHT, ...
   L_INT Order;                  // Is it ORDER_RGB, ORDER_BGR
   L_SIZE_T Size;                // Size of the bitmap
   L_VOID* pDitherData;          // Internal use
   L_UINT DitheringMethod;       // Which dithering method used
   #if !defined(FOR_WINCE)
   L_VOID* pRgnInfo;             // Internal use
   #endif // #if !defined(FOR_WINCE)
   L_INT XResolution;            // Bitmaps X resolution (DPI)
   L_INT YResolution;            // Bitmaps Y resolution (DPI)
   L_INT Left;                   // Left playback offset
   L_INT Top;                    // Top playback offset
   L_UINT Delay;                 // Playback delay (ms)
   L_COLORREF Background;        // Playback background color
   L_COLORREF Transparency;      // Playback transparent color
   L_UINT DisposalMethod;        // Playback disposal method
   L_INT LowBit;                 // low bit set by L_WindowLevelBitmap
   L_INT HighBit;                // high bit set by L_WindowLevelBitmap
   L_RGBQUAD* pLUT;              // LUT set by L_WindowLevel or L_WindowLevelBitmap
   L_UINT LUTLength;             // no of entries pointed by LUT
   L_SIZE_T DataOffset;          // offset into hData
   L_INT MinVal;                 // min val for a grayscale bitmap
   L_INT MaxVal;                 // max val for a grayscale bitmap
   L_INT PaintLowBit;            // low bit set by L_WindowLevelBitmap
   L_INT PaintHighBit;           // high bit set by L_WindowLevelBitmap
   L_UINT PaintGamma;            // Paint gamma value to be used during painting
   L_INT PaintContrast;          // Paint contrast value to be used during painting
   L_INT PaintIntensity;         // Paint intensity value to be used during painting
   L_UCHAR* pPaintLUT;           // Paint LUT generated from PaintIntensity, PaintGamma, PaintIntensity
   L_VOID* pOverlaysInfo;        // Internal use
   L_INT OriginalFormat;         // Original file format of this bitmap

#if defined(LEADTOOLS_V16_OR_LATER)
#if !defined(FOR_WINCE)
   L_VOID* pDicomData;           // used by ltdic to store DICOM specific data
   L_SIZE_T uDicomDataLength;    // size of DICOM specific data
   L_UINT32 LUTLength16;         // number of entries pointed by LUT16
   L_RGBQUAD16 *pLUT16;          // 16-bit LUT set by L_WindowLevelExt or L_WindowLevelBitmapExt
   L_UINT16 *pPaintLUT16;        // 16-bit paint LUT generated from PaintIntensity, PaintGamma, PaintIntensity
   CRITICAL_SECTION* pCriticalSection;
   L_UINT64 Size64;              // Size of the bitmap data in bytes; use instead of Size
   L_UINT64 CurrentPos64;        // Internal use
#endif // #if !defined(FOR_WINCE)
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
   L_UINT32 Reserved;            // reserved field, do not use
}
BITMAPHANDLE, *pBITMAPHANDLE;

typedef struct _BITMAPHANDLEOLD
{
   L_UINT uStructSize;

   union
   {
      struct
      {
         L_HANDLE hData;  // internal use
         L_VOID **ppData; // internal use
      }
      Compressed;

      struct
      {
         L_HANDLE hData;  // Internal use
         L_UCHAR* pData;   // Internal use
      }
      Windows;

      struct
      {
         L_HFILE fd;         // Internal use
         L_UINT Available; // Internal use
      }
      File;

      #if !defined(FOR_WINCE)
      struct
      {
         L_HANDLE hData;  // Internal use
         L_UCHAR* pData;   // Internal use
      }
      Tiled;
      #endif //#if !defined(FOR_WINCE)

      struct
      {
         L_HANDLE hData;  // internal use
         L_VOID* pData;    // internal use
      }
      SuperCompressed;
   }
   Addr;

   L_INT Width;                 // Image width
   L_INT Height;                // Image height
   L_INT BitsPerPixel;          // Number of bits per pixel
   L_UINT BytesPerLine;         // Bytes per line, rounded by four
   L_SIZE_T CurrentPos;         // Internal use
   L_INT CurrentRow;            // Internal use

   struct _Flags
   {
      L_BITFIELD Allocated:1;             // Is the bitmap allocated?
      L_BITFIELD ConventionalMemory:1;    // allocated in conventional memory
      L_BITFIELD DiskMemory:1;            // allocated on disk
      L_BITFIELD ProgressiveAvailable:1;  // progressive format
      L_BITFIELD LockedMemory:1;          // Is memory locked in windows
      L_BITFIELD Interlaced:1;            // Interlaced format
      L_BITFIELD WaitUserInput:1;         // Playback wait for user input
      L_BITFIELD Transparency:1;          // Playback uses transparency
      L_BITFIELD Compressed:1;            // 1 bit RLE compressed data
      L_BITFIELD Signed:1;                // the pixel values are signed
      L_BITFIELD Mirror:1;                // this is a mirror of another image
      L_BITFIELD UseLUT:1;                // use pLUT for image processing
      L_BITFIELD Global:1;                // hData is HGLOBAL
      L_BITFIELD UsePaintLUT:1;           // Use PaintGamma, PaintContrast, PaintIntensity
      L_BITFIELD Tiled:1;                 // Bitmap is stored as tiles
      L_BITFIELD SuperCompressed:1;       // Super compressed data
      L_BITFIELD NoClip:1;                // Do not clip region
      L_BITFIELD Dummy:15;
   }
   Flags;

   L_INT nColors;                // No. of possible colors in the bitmap
   L_RGBQUAD* pPalette;          // Pointer to a L_RGBQUAD array containing the palette

   L_INT ViewPerspective;        // Is TOP_LEFT, BOTTOM_RIGHT, ...
   L_INT Order;                  // Is it ORDER_RGB, ORDER_BGR
   L_SIZE_T Size;                // Size of the bitmap
   L_VOID* pDitherData;          // Internal use
   L_UINT DitheringMethod;       // Which dithering method used
   #if !defined(FOR_WINCE)
   L_VOID* pRgnInfo;             // Internal use
   #endif // #if !defined(FOR_WINCE)
   L_INT XResolution;            // Bitmaps X resolution (DPI)
   L_INT YResolution;            // Bitmaps Y resolution (DPI)
   L_INT Left;                   // Left playback offset
   L_INT Top;                    // Top playback offset
   L_UINT Delay;                 // Playback delay (ms)
   L_COLORREF Background;        // Playback background color
   L_COLORREF Transparency;      // Playback transparent color
   L_UINT DisposalMethod;        // Playback disposal method
   L_INT LowBit;                 // low bit set by L_WindowLevelBitmap
   L_INT HighBit;                // high bit set by L_WindowLevelBitmap
   L_RGBQUAD* pLUT;              // LUT set by L_WindowLevel or L_WindowLevelBitmap
   L_UINT LUTLength;             // no of entries pointed by LUT
   L_SIZE_T DataOffset;          // offset into hData
   L_INT MinVal;                 // min val for a grayscale bitmap
   L_INT MaxVal;                 // max val for a grayscale bitmap
   L_INT PaintLowBit;            // low bit set by L_WindowLevelBitmap
   L_INT PaintHighBit;           // high bit set by L_WindowLevelBitmap
   L_UINT PaintGamma;            // Paint gamma value to be used during painting
   L_INT PaintContrast;          // Paint contrast value to be used during painting
   L_INT PaintIntensity;         // Paint intensity value to be used during painting
   L_UCHAR* pPaintLUT;           // Paint LUT generated from PaintIntensity, PaintGamma, PaintIntensity
   L_VOID* pOverlaysInfo;        // Internal use
   L_INT OriginalFormat;         // Original file format of this bitmap

#if defined(LEADTOOLS_V16_OR_LATER)
#if !defined(FOR_WINCE)
   L_VOID* pDicomData;           // used by ltdic to store DICOM specific data
   L_SIZE_T uDicomDataLength;    // size of DICOM specific data
#endif // #if !defined(FOR_WINCE)
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
   L_UINT32 Reserved;            // reserved field, do not use
}
BITMAPHANDLEOLD, *pBITMAPHANDLEOLD;

#if !defined(FOR_WINCE)

// version structure
typedef struct _VERSIONINFO
{
   L_UINT uStructSize;
   L_UCHAR Product[60]; // Product Name
   L_INT Level;         // Toolkit Level (Raster, Document, Medical, etc.)
   L_INT MajorNumber;   // Major Version Number
   L_INT MinorNumber;   // Minor Version Number
   L_UCHAR Date[16];    // Date of the Build
   L_UCHAR Time[16];    // Time of the Build
}
VERSIONINFO, *pVERSIONINFO;
#endif // #if !defined(FOR_WINCE)

/****************************************************************
   Callback typedefs
****************************************************************/

typedef L_INT (pEXT_CALLBACK COLORRESCALLBACK)(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_INT nLines,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK COLORRESLISTCALLBACK)(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_UINT uIndex,
   L_UINT uLine,
   L_INT nLines,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK STATUSCALLBACK)(
   L_INT nPercentComplete,
   L_VOID* pUserData);

typedef L_HFILE (pEXT_CALLBACK REDIRECTOPENA)(
   L_CHAR* pFile,
   L_INT nMode,
   L_INT nShare,
   L_VOID* pUserData);

#if defined(FOR_UNICODE)
typedef L_HFILE (pEXT_CALLBACK REDIRECTOPEN)(
   L_TCHAR* pFile,
   L_INT nMode,
   L_INT nShare,
   L_VOID* pUserData);
#else
typedef REDIRECTOPENA REDIRECTOPEN;
#endif // #if defined(FOR_UNICODE)

typedef L_UINT (pEXT_CALLBACK REDIRECTREAD)(
   L_HFILE nFd,
   L_UCHAR* pBuf,
   L_UINT uCount,
   L_VOID* pUserData);

typedef L_UINT (pEXT_CALLBACK REDIRECTWRITE)(
   L_HFILE nFd,
   L_UCHAR* pBuf,
   L_UINT uCount,
   L_VOID* pUserData);

typedef L_SSIZE_T (pEXT_CALLBACK REDIRECTSEEK)(
   L_HFILE nFd,
   L_SSIZE_T nPos,
   L_INT nOrigin,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK REDIRECTCLOSE)(
   L_HFILE nFd,
   L_VOID* pUserData);

typedef L_INT (pEXT_CALLBACK RESIZECALLBACK)(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_INT nRow,
   L_INT nCol,
   L_SIZE_T uBytes,
   L_VOID* pUserData);

/****************************************************************
   Function prototypes
****************************************************************/
#if !defined(FOR_MANAGED) || defined(FOR_MANAGED_KERNEL)

L_LTKRN_API L_VOID EXT_FUNCTION L_AccessBitmap(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_AllocateBitmap(
   pBITMAPHANDLE pBitmap,
   L_UINT uMemory);

L_LTKRN_API L_INT EXT_FUNCTION L_ChangeBitmapHeight(
   pBITMAPHANDLE pBitmap,
   L_INT nHeight);

L_LTKRN_API L_INT EXT_FUNCTION L_ChangeBitmapViewPerspective(
   pBITMAPHANDLE pDstBitmap,
   pBITMAPHANDLE pSrcBitmap,
   L_UINT uStructSize,
   L_INT ViewPerspective);

L_LTKRN_API L_INT EXT_FUNCTION L_ChangeFromDIB(
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_HDIB hDIB);

L_LTKRN_API L_HDIB EXT_FUNCTION L_ChangeToDIB(
   pBITMAPHANDLE pBitmap,
   L_UINT uType);

L_LTKRN_API L_INT EXT_FUNCTION L_ClearBitmap(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_ClearNegativePixels(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_ColorResBitmap(
   pBITMAPHANDLE pBitmapSrc,
   pBITMAPHANDLE pBitmapDst,
   L_UINT uStructSize,
   L_INT nBitsPerPixel,
   L_UINT uFlags,
   L_RGBQUAD* pPalette,
   L_HPALETTE hPalette,
   L_UINT uColors,
   COLORRESCALLBACK pfnCallback,
   L_VOID* pUserData);

L_LTKRN_API L_INT EXT_FUNCTION L_ColorResBitmapList(
   HBITMAPLIST hList,
   L_INT nBitsPerPixel,
   L_UINT uFlags,
   L_RGBQUAD* pPalette,
   L_HPALETTE hPalette,
   L_UINT uColors);

L_LTKRN_API L_INT EXT_FUNCTION L_ColorResBitmapListExt(
   HBITMAPLIST hList,
   L_INT nBitsPerPixel,
   L_UINT uFlags,
   L_RGBQUAD* pPalette,
   L_HPALETTE hPalette,
   L_UINT uColors,
   COLORRESLISTCALLBACK pfnCallback,
   L_VOID* pUserData);

L_LTKRN_API L_INT EXT_FUNCTION L_CompressRow(
   L_UINT16* pRunBuffer,
   L_UCHAR* pBuffer,
   L_INT nCol,
   L_INT nWidth);

L_LTKRN_API L_INT EXT_FUNCTION L_CompressRows(
   L_UINT16* pRunBuffer,
   L_UCHAR* pBuffer,
   L_UINT nWidth,
   L_UINT nRows);

L_LTKRN_API L_INT EXT_FUNCTION L_ConvertBuffer(
   L_UCHAR* pBuffer,
   L_INT nWidth,
   L_INT nBitsPerPixelSrc,
   L_INT nBitsPerPixelDst,
   L_INT nOrderSrc,
   L_INT nOrderDst,
   L_RGBQUAD* pPaletteSrc,
   L_RGBQUAD* pPaletteDst);

L_LTKRN_API L_INT EXT_FUNCTION L_ConvertBufferExt(
   L_UCHAR* pBuffer,
   L_INT nWidth,
   L_INT nBitsPerPixelSrc,
   L_INT nBitsPerPixelDst,
   L_INT nOrderSrc,
   L_INT nOrderDst,
   L_RGBQUAD* pPaletteSrc,
   L_RGBQUAD* pPaletteDst,
   L_UINT uFlags,
   L_INT uLowBit,
   L_INT uHighBit);

#if defined(LEADTOOLS_V16_OR_LATER) && !defined(FOR_WINCE)
L_LTKRN_API L_INT EXT_FUNCTION L_ConvertBufferExt2(
   L_UCHAR* pBuffer,
   L_INT nWidth,
   L_INT nBitsPerPixelSrc,
   L_INT nBitsPerPixelDst,
   L_INT nOrderSrc,
   L_INT nOrderDst,
   L_RGBQUAD* pPaletteSrc,
   L_RGBQUAD* pPaletteDst,
   L_RGBQUAD16* pPaletteSrc16,
   L_RGBQUAD16* pPaletteDst16,
   L_UINT uFlags,
   L_INT uLowBit,
   L_INT uHighBit);
#endif // #if defined(LEADTOOLS_V16_OR_LATER) && !defined(FOR_WINCE)

L_LTKRN_API L_INT EXT_FUNCTION L_ConvertFromDIB(
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_BITMAPINFO* pInfo,
   L_UCHAR* pBits);

L_LTKRN_API L_HDIB EXT_FUNCTION L_ConvertToDIB(
   pBITMAPHANDLE pBitmap,
   L_UINT uType);

L_LTKRN_API L_INT EXT_FUNCTION L_CopyBitmap(
   pBITMAPHANDLE pBitmapDst,
   pBITMAPHANDLE pBitmapSrc,
   L_UINT uStructSize);

L_LTKRN_API L_INT EXT_FUNCTION L_CopyBitmap2(
   pBITMAPHANDLE pBitmapDst,
   pBITMAPHANDLE pBitmapSrc,
   L_UINT uStructSize,
   L_UINT uMemory);

L_LTKRN_API L_INT EXT_FUNCTION L_CopyBitmapData(
   pBITMAPHANDLE pBitmapDst,
   pBITMAPHANDLE pBitmapSrc);

L_LTKRN_API L_INT EXT_FUNCTION L_CopyBitmapHandle(
   pBITMAPHANDLE pBitmapDst,
   pBITMAPHANDLE pBitmapSrc,
   L_UINT uStructSize);

L_LTKRN_API L_INT EXT_FUNCTION L_CopyBitmapListItems(
   pHBITMAPLIST phList,
   HBITMAPLIST hList,
   L_UINT uIndex,
   L_UINT uCount);

L_LTKRN_API L_INT EXT_FUNCTION L_CopyBitmapRect(
   pBITMAPHANDLE pBitmapDst,
   pBITMAPHANDLE pBitmapSrc,
   L_UINT uStructSize,
   L_INT nCol,
   L_INT nRow,
   L_UINT uWidth,
   L_UINT uHeight);

L_LTKRN_API L_INT EXT_FUNCTION L_CopyBitmapRect2(
   pBITMAPHANDLE pBitmapDst,
   pBITMAPHANDLE pBitmapSrc,
   L_UINT uStructSize,
   L_INT nCol,
   L_INT nRow,
   L_UINT uWidth,
   L_UINT uHeight,
   L_UINT uMemory);

L_LTKRN_API L_INT EXT_FUNCTION L_CreateBitmap(
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_UINT uMemory,
   L_UINT uWidth,
   L_UINT uHeight,
   L_UINT uBitsPerPixel,
   L_UINT uOrder,
   L_RGBQUAD* pPalette,
   L_UINT uViewPerspective,
   L_UCHAR* pData,
   L_SIZE_T dwSize);

L_LTKRN_API L_INT EXT_FUNCTION L_CreateBitmapList(
   pHBITMAPLIST phList);

L_LTKRN_API L_UINT* EXT_FUNCTION L_CreateUserMatchTable(
   L_RGBQUAD* pPalette,
   L_UINT uColors);

L_LTKRN_API L_INT EXT_FUNCTION L_DefaultDithering(
   L_UINT uMethod);

L_LTKRN_API L_INT EXT_FUNCTION L_DeleteBitmapListItems(
   HBITMAPLIST hList,
   L_UINT uIndex,
   L_UINT uCount);

L_LTKRN_API L_INT EXT_FUNCTION L_DestroyBitmapList(
   HBITMAPLIST hList);

L_LTKRN_API L_INT EXT_FUNCTION L_DitherLine(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBufferSrc,
   L_UCHAR* pBufferDst);

L_LTKRN_API L_INT EXT_FUNCTION L_CopyBitmapPalette(
   pBITMAPHANDLE pDstBitmap,
   pBITMAPHANDLE pSrcBitmap);

L_LTKRN_API L_HPALETTE EXT_FUNCTION L_DupBitmapPalette(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_HPALETTE EXT_FUNCTION L_DupPalette(
   L_HPALETTE hPalette);

L_LTKRN_API L_INT EXT_FUNCTION L_ExpandRow(
   L_UINT16* pRunBuffer,
   L_UCHAR* pBuffer,
   L_UINT nCol,
   L_INT nWidth);

L_LTKRN_API L_INT EXT_FUNCTION L_ExpandRows(
   L_UINT16* pRun,
   L_UCHAR* pBuffer,
   L_UINT nWidth,
   L_UINT nRows);

L_LTKRN_API L_INT EXT_FUNCTION L_FillBitmap(
   pBITMAPHANDLE pBitmap,
   L_COLORREF crFill);

L_LTKRN_API L_INT EXT_FUNCTION L_FlipBitmap(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_VOID EXT_FUNCTION L_FreeBitmap(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_FreeUserMatchTable(
   L_UINT* pTable);

L_LTKRN_API L_INT EXT_FUNCTION L_GetBitmapColors(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   L_INT nCount,
   L_RGBQUAD* pPalette);

L_LTKRN_API L_INT EXT_FUNCTION L_GetBitmapListCount(
   HBITMAPLIST hList,
   L_UINT* puCount);

L_LTKRN_API L_INT EXT_FUNCTION L_GetBitmapListItem(
   HBITMAPLIST hList,
   L_UINT uIndex,
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize);

L_LTKRN_API L_SSIZE_T EXT_FUNCTION L_GetBitmapRow(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_INT nRow,
   L_SIZE_T uBytes);

L_LTKRN_API L_SSIZE_T EXT_FUNCTION L_GetBitmapRowCol(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_INT nRow,
   L_INT nCol,
   L_SIZE_T uBytes);

L_LTKRN_API L_SSIZE_T EXT_FUNCTION L_GetBitmapRowColCompressed(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pWorkBuffer,
   L_UINT16* pRunBuffer,
   L_INT nRow,
   L_INT nCol,
   L_SIZE_T uWidth);

L_LTKRN_API L_INT EXT_FUNCTION L_GetBitmapRowCompressed(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pWorkBuffer,
   L_UINT16* pRunBuffer,
   L_UINT nRow,
   L_UINT nLines);

L_LTKRN_API L_INT EXT_FUNCTION L_GetFixedPalette(
   L_RGBQUAD* pPalette,
   L_INT nBitsPerPixel);

L_LTKRN_API L_COLORREF EXT_FUNCTION L_GetPixelColor(
   pBITMAPHANDLE pBitmap,
   L_INT nRow,
   L_INT nCol);

L_LTKRN_API STATUSCALLBACK EXT_FUNCTION L_GetStatusCallBack(
   L_VOID** ppUserData);

L_LTKRN_API L_VOID EXT_FUNCTION L_SetStatusCallBack(
   STATUSCALLBACK pfnCallback,
   L_VOID* pUserData,
   STATUSCALLBACK* pfnOldCallback,
   L_VOID** ppOldUserData);

L_LTKRN_API STATUSCALLBACK EXT_FUNCTION L_GetCopyStatusCallBack(
   L_VOID** ppUserData);

L_LTKRN_API L_VOID EXT_FUNCTION L_SetCopyStatusCallBack(
   STATUSCALLBACK pfnCallback,
   L_VOID* pUserData,
   STATUSCALLBACK* pOldFunction,
   L_VOID** ppUserData);

L_LTKRN_API L_INT EXT_FUNCTION L_GrayScaleBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nBitsPerPixel);

L_LTKRN_API L_INT EXT_FUNCTION L_InitBitmap(
   pBITMAPHANDLE pBitmap,
   L_UINT uStructSize,
   L_INT nWidth,
   L_INT nHeight,
   L_INT nBitsPerPixel);

L_LTKRN_API L_INT EXT_FUNCTION L_InsertBitmapListItem(
   HBITMAPLIST hList,
   L_UINT uIndex,
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_IsGrayScaleBitmap(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_BOOL EXT_FUNCTION L_IsSupportLocked(
   L_UINT uType);

L_LTKRN_API L_INT EXT_FUNCTION L_PointFromBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT ViewPerspective,
   L_INT* px,
   L_INT* py);

L_LTKRN_API L_INT EXT_FUNCTION L_PointToBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT ViewPerspective,
   L_INT* px,
   L_INT* py);

L_LTKRN_API L_INT EXT_FUNCTION L_PutBitmapColors(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   L_INT nCount,
   L_RGBQUAD* pPalette);

L_LTKRN_API L_SSIZE_T EXT_FUNCTION L_PutBitmapRow(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_INT nRow,
   L_SIZE_T uBytes);

L_LTKRN_API L_SSIZE_T EXT_FUNCTION L_PutBitmapRowCol(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pBuffer,
   L_INT nRow,
   L_INT nCol,
   L_SIZE_T uBytes);

L_LTKRN_API L_SSIZE_T EXT_FUNCTION L_PutBitmapRowColCompressed(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pWorkBuffer,
   L_UINT16* pRunBuffer,
   L_INT nRow,
   L_INT nCol,
   L_UINT uWidth);

L_LTKRN_API L_INT EXT_FUNCTION L_PutBitmapRowCompressed(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pWorkBuffer,
   L_UINT16* pRunBuffer,
   L_UINT nRow,
   L_UINT nLines);

L_LTKRN_API L_INT EXT_FUNCTION L_PutPixelColor(
   pBITMAPHANDLE pBitmap,
   L_INT nRow,
   L_INT nCol,
   L_COLORREF crColor);

L_LTKRN_API L_INT EXT_FUNCTION L_RectFromBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT ViewPerspective,
   L_RECT* lprc);

L_LTKRN_API L_INT EXT_FUNCTION L_RectToBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nViewPerspective,
   L_RECT* lprc);

L_LTKRN_API L_VOID EXT_FUNCTION L_RedirectIOA(
   REDIRECTOPENA pfnOpen,
   REDIRECTREAD pfnRead,
   REDIRECTWRITE pfnWrite,
   REDIRECTSEEK pfnSeek,
   REDIRECTCLOSE pfnClose,
   L_VOID* pUserData);

#if defined(FOR_UNICODE)
L_LTKRN_API L_VOID EXT_FUNCTION L_RedirectIO(
   REDIRECTOPEN pfnOpen,
   REDIRECTREAD pfnRead,
   REDIRECTWRITE pfnWrite,
   REDIRECTSEEK pfnSeek,
   REDIRECTCLOSE pfnClose,
   L_VOID* pUserData);
#else
#define L_RedirectIO L_RedirectIOA
#endif // #if defined(FOR_UNICODE)

L_LTKRN_API L_VOID EXT_FUNCTION L_ReleaseBitmap(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_RemoveBitmapListItem(
   HBITMAPLIST hList,
   L_UINT uIndex,
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_Resize(
   L_UCHAR* pBuffer,
   L_INT nRow,
   L_INT nBitsPerPixel,
   L_INT* pXSize,
   L_INT* pYSize,
   L_VOID* pResizeData);

L_LTKRN_API L_INT EXT_FUNCTION L_ResizeBitmap(
   pBITMAPHANDLE pBitmapSrc,
   pBITMAPHANDLE pDestBitmap,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_ReverseBitmap(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_RotateBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nAngle,
   L_UINT uFlags,
   L_COLORREF crFill);

L_LTKRN_API L_INT EXT_FUNCTION L_RotateBitmapViewPerspective(
   pBITMAPHANDLE pBitmap,
   L_INT nAngle);

L_LTKRN_API L_INT EXT_FUNCTION L_SetBitmapDataPointer(
   pBITMAPHANDLE pBitmap,
   L_UCHAR* pData,
   L_SIZE_T dwSize);

L_LTKRN_API L_INT EXT_FUNCTION L_SetBitmapListItem(
   HBITMAPLIST hList,
   L_UINT uIndex,
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_UINT* EXT_FUNCTION L_SetUserMatchTable(
   L_UINT* pTable);

L_LTKRN_API L_INT EXT_FUNCTION L_SizeBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nWidth,
   L_INT nHeight,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_StartDithering(
   pBITMAPHANDLE pBitmap,
   L_RGBQUAD* pPalette,
   L_UINT uColors);

L_LTKRN_API L_INT EXT_FUNCTION L_StartResize(
   L_INT nOldWidth,
   L_INT nOldHeight,
   L_INT nNewWidth,
   L_INT nNewHeight,
   L_VOID** ppResizeData);

L_LTKRN_API L_INT EXT_FUNCTION L_StopDithering(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_StopResize(
   L_VOID* pResizeData);

L_LTKRN_API L_COLORREF EXT_FUNCTION L_TranslateBitmapColor(
   pBITMAPHANDLE pBitmapDst,
   pBITMAPHANDLE pBitmapSrc,
   L_COLORREF crColor);

L_LTKRN_API L_INT EXT_FUNCTION L_ToggleBitmapCompression(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_TrimBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nCol,
   L_INT nRow,
   L_UINT uWidth,
   L_UINT uHeight);

L_LTKRN_API L_VOID EXT_FUNCTION L_UnlockSupportA(
   L_UINT uType,
   L_CHAR* pKey);

#if defined(FOR_UNICODE)
L_LTKRN_API L_VOID EXT_FUNCTION L_UnlockSupport(
   L_UINT uType,
   L_TCHAR* pKey);
#else
#define L_UnlockSupport L_UnlockSupportA
#endif // #if defined(FOR_UNICODE)

L_LTKRN_API L_BOOL EXT_FUNCTION L_KernelHasExpired(
   L_VOID);

L_LTKRN_API L_INT EXT_FUNCTION L_FlipBitmapViewPerspective(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_ReverseBitmapViewPerspective(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_INT EXT_FUNCTION L_StartResizeBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nNewWidth,
   L_INT nNewHeight,
   L_INT nNewBits,
   L_RGBQUAD* pPalette,
   L_INT nColors,
   L_UINT uFlags,
   RESIZECALLBACK pfnCallback,
   L_VOID* pCallbackData,
   L_VOID** ppResizeData);

L_LTKRN_API L_INT EXT_FUNCTION L_GetResizedRowCol(
   L_VOID* pResizeData,
   L_UCHAR* pBuffer,
   L_INT nRow,
   L_INT nCol,
   L_SIZE_T uBytes);

L_LTKRN_API L_INT EXT_FUNCTION L_StopResizeBitmap(
   L_VOID* pResizeData);

L_LTKRN_API L_INT EXT_FUNCTION L_MoveBitmapListItems(
   pHBITMAPLIST phList,
   HBITMAPLIST hList,
   L_UINT uIndex,
   L_UINT uCount);

#if defined(FOR_LIB)
L_LTKRN_API L_BOOL EXT_FUNCTION L_InitLEADTOOLS(
   L_HINSTANCE hInstance);

L_LTKRN_API L_VOID EXT_FUNCTION L_QuitLEADTOOLS(
   L_BOOL bProcess);
#endif // #if defined(FOR_LIB)

L_LTKRN_API L_INT EXT_FUNCTION L_ChangeBitmapCompression(
   pBITMAPHANDLE pBitmap,
   L_INT nComp);

L_LTKRN_API L_INT EXT_FUNCTION L_GetPixelData(
   pBITMAPHANDLE pBitmap,
   L_VOID*pData,
   L_INT nRow,
   L_INT nCol,
   L_SIZE_T uBytes);

L_LTKRN_API L_INT EXT_FUNCTION L_PutPixelData(
   pBITMAPHANDLE pBitmap,
   L_VOID* pData,
   L_INT nRow,
   L_INT nCol,
   L_SIZE_T uBytes);

L_LTKRN_API L_INT EXT_FUNCTION L_SetDefaultMemoryType(
   L_UINT uMemory);

L_LTKRN_API L_INT EXT_FUNCTION L_GetDefaultMemoryType(
   L_UINT* puMemory);

L_LTKRN_API L_INT EXT_FUNCTION L_SetMemoryThresholds(
   L_INT nTiledThreshold,
   L_SSIZE_T nMaxConvSize,
   L_SSIZE_T nTileSize,
   L_INT nConvTiles,
   L_INT nConvBuffers,
   L_UINT uFlags);

L_LTKRN_API L_VOID EXT_FUNCTION L_GetMemoryThresholds(
   L_INT* pnTiledThreshold,
   L_SSIZE_T* pnMaxConvSize,
   L_SSIZE_T* pnTileSize,
   L_INT* pnConvTiles,
   L_INT* pnConvBuffers);

L_LTKRN_API L_INT EXT_FUNCTION L_SetBitmapMemoryInfo(
   pBITMAPHANDLE pBitmap,
   L_UINT uMemory,
   L_SSIZE_T uTileSize,
   L_UINT uTotalTiles,
   L_UINT uConvTiles,
   L_UINT uMaxTileViews,
   L_UINT uTileViews,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_GetBitmapMemoryInfo(
   pBITMAPHANDLE pBitmap,
   L_UINT* puMemory,
   L_SSIZE_T* puTileSize,
   L_UINT* puTotalTiles,
   L_UINT* puConvTiles,
   L_UINT* puMaxTileViews,
   L_UINT* puTileViews);

L_LTKRN_API L_INT EXT_FUNCTION L_SetTempDirectoryA(
   L_CHAR* pszTempDir);

L_LTKRN_API L_INT EXT_FUNCTION L_GetTempDirectoryA(
   L_CHAR* pszTempDir,
   L_SIZE_T uSize);

#if defined(FOR_UNICODE)
L_LTKRN_API L_INT EXT_FUNCTION L_SetTempDirectory(
   L_TCHAR* pszTempDir);

L_LTKRN_API L_INT EXT_FUNCTION L_GetTempDirectory(
   L_TCHAR* pszTempDir,
   L_SIZE_T uSize);
#else
#define L_SetTempDirectory L_SetTempDirectoryA
#define L_GetTempDirectory L_GetTempDirectoryA
#endif // #if defined(FOR_UNICODE)

L_LTKRN_API L_INT EXT_FUNCTION L_SetBitmapPalette(
   pBITMAPHANDLE pBitmap,
   L_HPALETTE hPalette);

L_LTKRN_API L_INT EXT_FUNCTION L_ScrambleBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nColStart,
   L_INT nRowStart,
   L_INT nWidth,
   L_INT nHeight,
   L_UINT uKey,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_CombineBitmapWarp(
   pBITMAPHANDLE pBitmapDst,
   L_POINT ptDstArray[],
   pBITMAPHANDLE pBitmapSrc,
   L_POINT ptSrc,
   L_INT nSrcWidth,
   L_INT nSrcHeight,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_SetOverlayBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   pBITMAPHANDLE pOverlayBitmap,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_GetOverlayBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   pBITMAPHANDLE pOverlayBitmap,
   L_UINT uStructSize,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_SetOverlayAttributesA(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   pOVERLAYATTRIBUTESA pOverlayAttributes,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_GetOverlayAttributesA(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   pOVERLAYATTRIBUTESA pOverlayAttributes,
   L_UINT uStructSize,
   L_UINT uFlags);

#if defined(FOR_UNICODE)
L_LTKRN_API L_INT EXT_FUNCTION L_SetOverlayAttributes(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   pOVERLAYATTRIBUTES pOverlayAttributes,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_GetOverlayAttributes(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   pOVERLAYATTRIBUTES pOverlayAttributes,
   L_UINT uStructSize,
   L_UINT uFlags);
#else
#define L_SetOverlayAttributes L_SetOverlayAttributesA
#define L_GetOverlayAttributes L_GetOverlayAttributesA
#endif // #if defined(FOR_UNICODE)

L_LTKRN_API L_INT EXT_FUNCTION L_UpdateBitmapOverlayBits(
   pBITMAPHANDLE pBitmap,
   L_INT nIndex,
   L_UINT uFlags);

L_LTKRN_API L_INT EXT_FUNCTION L_GetOverlayCount(
   pBITMAPHANDLE pBitmap,
   L_UINT*puCount,
   L_UINT uFlags);

// These functions not ported to Windows CE
#if !defined(FOR_WINCE)
L_LTKRN_API L_HDC EXT_FUNCTION L_CreateLeadDC(
   pBITMAPHANDLE pBitmap);

L_LTKRN_API L_BOOL EXT_FUNCTION L_DeleteLeadDC(
   L_HDC hDC);

L_LTKRN_API L_INT EXT_FUNCTION L_GetBitmapAlpha(
   pBITMAPHANDLE pBitmap,
   pBITMAPHANDLE pAlpha,
   L_UINT uStructSize);

L_LTKRN_API L_INT EXT_FUNCTION L_SetBitmapAlpha(
   pBITMAPHANDLE pBitmap,
   pBITMAPHANDLE pAlpha);

L_LTKRN_API L_INT EXT_FUNCTION L_SetBitmapAlphaValues(
   pBITMAPHANDLE pBitmap,
   L_UINT16 uAlpha);

L_LTKRN_API L_INT EXT_FUNCTION L_ShearBitmap(
   pBITMAPHANDLE pBitmap,
   L_INT nAngle,
   L_BOOL fHorizontal,
   L_COLORREF crFill);

L_LTKRN_API L_INT EXT_FUNCTION L_VersionInfo(
   pVERSIONINFO pVersionInfo,
   L_UINT uStructSize);

L_LTKRN_API L_INT EXT_FUNCTION L_CreateGrayScaleBitmap(pBITMAPHANDLE pDstBitmap, pBITMAPHANDLE pSrcBitmap, L_INT uBitsPerPixel);

L_LTKRN_API L_INT EXT_FUNCTION L_SetBitmapExtraData(pBITMAPHANDLE pBitmap, L_UINT uTag, L_UINT uDataSize, L_UCHAR *pData);

L_LTKRN_API L_INT EXT_FUNCTION L_GetBitmapExtraData(pBITMAPHANDLE pBitmap, L_UINT uTag, L_UINT *puDataSize, L_UCHAR *pData);
#endif // #if !defined(FOR_WINCE)

L_LTKRN_API L_BOOL EXT_FUNCTION L_HasExpired(
   L_VOID);

#endif // #if !defined(FOR_MANAGED) || defined(FOR_MANAGED_KERNEL)
#endif // !defined(LEAD_DEFINES_ONLY)
#undef L_HEADER_ENTRY
#include "ltpck.h"

#endif // #if !defined(LTKRN_H)
