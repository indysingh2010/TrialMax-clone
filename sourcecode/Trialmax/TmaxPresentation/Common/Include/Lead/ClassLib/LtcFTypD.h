/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : tcprnt.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_FUNCTIONS_TYPEDEFINES_H_
#define  _LEAD_FUNCTIONS_TYPEDEFINES_H_

/*----------------------------------------------------------------------------+
| MACROS                                                                      |
+----------------------------------------------------------------------------*/
#ifdef USE_POINTERS_TO_LEAD_FUNCTIONS

//-----------------------------------------------------------------------------
//--LTKRN.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

typedef L_VOID ( pWRPEXT_CALLBACK pL_ACCESSBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_ALLOCATEBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_UINT uMemory);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPHEIGHT)(
               pBITMAPHANDLE pBitmap,
               L_INT nHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPVIEWPERSPECTIVE)(
               pBITMAPHANDLE pDstBitmap,
               pBITMAPHANDLE pSrcBitmap,
               L_UINT uStructSize,
               L_INT ViewPerspective);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEFROMDIB)(
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_HDIB hDIB);

typedef L_HDIB ( pWRPEXT_CALLBACK pL_CHANGETODIB)(
               pBITMAPHANDLE pBitmap,
               L_UINT uType);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLEARBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLEARNEGATIVEPIXELS)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORRESBITMAP)(
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

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORRESBITMAPLIST)(
               HBITMAPLIST hList,
               L_INT nBitsPerPixel,
               L_UINT uFlags,
               L_RGBQUAD* pPalette,
               L_HPALETTE hPalette,
               L_UINT uColors);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORRESBITMAPLISTEXT)(
               HBITMAPLIST hList,
               L_INT nBitsPerPixel,
               L_UINT uFlags,
               L_RGBQUAD* pPalette,
               L_HPALETTE hPalette,
               L_UINT uColors,
               COLORRESLISTCALLBACK pfnCallback,
               L_VOID* pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_COMPRESSROW)(
               L_UINT16* pRunBuffer,
               L_UCHAR* pBuffer,
               L_INT nCol,
               L_INT nWidth);

typedef L_INT ( pWRPEXT_CALLBACK pL_COMPRESSROWS)(
               L_UINT16* pRunBuffer,
               L_UCHAR* pBuffer,
               L_UINT nWidth,
               L_UINT nRows);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTBUFFER)(
               L_UCHAR* pBuffer,
               L_INT nWidth,
               L_INT nBitsPerPixelSrc,
               L_INT nBitsPerPixelDst,
               L_INT nOrderSrc,
               L_INT nOrderDst,
               L_RGBQUAD* pPaletteSrc,
               L_RGBQUAD* pPaletteDst);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTBUFFEREXT)(
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

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTFROMDIB)(
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_BITMAPINFO* pInfo,
               L_UCHAR* pBits);

typedef L_HDIB ( pWRPEXT_CALLBACK pL_CONVERTTODIB)(
               pBITMAPHANDLE pBitmap,
               L_UINT uType);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYBITMAP)(
               pBITMAPHANDLE pBitmapDst,
               pBITMAPHANDLE pBitmapSrc,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYBITMAP2)(
               pBITMAPHANDLE pBitmapDst,
               pBITMAPHANDLE pBitmapSrc,
               L_UINT uStructSize,
               L_UINT uMemory);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYBITMAPDATA)(
               pBITMAPHANDLE pBitmapDst,
               pBITMAPHANDLE pBitmapSrc);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYBITMAPHANDLE)(
               pBITMAPHANDLE pBitmapDst,
               pBITMAPHANDLE pBitmapSrc,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYBITMAPLISTITEMS)(
               pHBITMAPLIST phList,
               HBITMAPLIST hList,
               L_UINT uIndex,
               L_UINT uCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYBITMAPRECT)(
               pBITMAPHANDLE pBitmapDst,
               pBITMAPHANDLE pBitmapSrc,
               L_UINT uStructSize,
               L_INT nCol,
               L_INT nRow,
               L_UINT uWidth,
               L_UINT uHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYBITMAPRECT2)(
               pBITMAPHANDLE pBitmapDst,
               pBITMAPHANDLE pBitmapSrc,
               L_UINT uStructSize,
               L_INT nCol,
               L_INT nRow,
               L_UINT uWidth,
               L_UINT uHeight,
               L_UINT uMemory);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEBITMAP)(
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

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEBITMAPLIST)(
               pHBITMAPLIST phList);

typedef L_UINT*( pWRPEXT_CALLBACK pL_CREATEUSERMATCHTABLE)(
               L_RGBQUAD* pPalette,
               L_UINT uColors);

typedef L_INT ( pWRPEXT_CALLBACK pL_DEFAULTDITHERING)(
               L_UINT uMethod);

typedef L_INT ( pWRPEXT_CALLBACK pL_DELETEBITMAPLISTITEMS)(
               HBITMAPLIST hList,
               L_UINT uIndex,
               L_UINT uCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESTROYBITMAPLIST)(
               HBITMAPLIST hList);

typedef L_INT ( pWRPEXT_CALLBACK pL_DITHERLINE)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pBufferSrc,
               L_UCHAR* pBufferDst);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYBITMAPPALETTE)(
               pBITMAPHANDLE pDstBitmap,
               pBITMAPHANDLE pSrcBitmap);

typedef L_HPALETTE ( pWRPEXT_CALLBACK pL_DUPBITMAPPALETTE)(
               pBITMAPHANDLE pBitmap);

typedef L_HPALETTE ( pWRPEXT_CALLBACK pL_DUPPALETTE)(
               L_HPALETTE hPalette);

typedef L_INT ( pWRPEXT_CALLBACK pL_EXPANDROW)(
               L_UINT16* pRunBuffer,
               L_UCHAR* pBuffer,
               L_UINT nCol,
               L_INT nWidth);

typedef L_INT ( pWRPEXT_CALLBACK pL_EXPANDROWS)(
               L_UINT16* pRun,
               L_UCHAR* pBuffer,
               L_UINT nWidth,
               L_UINT nRows);

typedef L_INT ( pWRPEXT_CALLBACK pL_FILLBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_COLORREF crFill);

typedef L_INT ( pWRPEXT_CALLBACK pL_FLIPBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_VOID ( pWRPEXT_CALLBACK pL_FREEBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEUSERMATCHTABLE)(
               L_UINT* pTable);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPCOLORS)(
               pBITMAPHANDLE pBitmap,
               L_INT nIndex,
               L_INT nCount,
               L_RGBQUAD* pPalette);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPLISTCOUNT)(
               HBITMAPLIST hList,
               L_UINT* puCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPLISTITEM)(
               HBITMAPLIST hList,
               L_UINT uIndex,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize);

typedef L_SSIZE_T ( pWRPEXT_CALLBACK pL_GETBITMAPROW)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pBuffer,
               L_INT nRow,
               L_SIZE_T uBytes);

typedef L_SSIZE_T ( pWRPEXT_CALLBACK pL_GETBITMAPROWCOL)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pBuffer,
               L_INT nRow,
               L_INT nCol,
               L_SIZE_T uBytes);

typedef L_SSIZE_T ( pWRPEXT_CALLBACK pL_GETBITMAPROWCOLCOMPRESSED)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pWorkBuffer,
               L_UINT16* pRunBuffer,
               L_INT nRow,
               L_INT nCol,
               L_SIZE_T uWidth);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPROWCOMPRESSED)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pWorkBuffer,
               L_UINT16* pRunBuffer,
               L_UINT nRow,
               L_UINT nLines);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETFIXEDPALETTE)(
               L_RGBQUAD* pPalette,
               L_INT nBitsPerPixel);

typedef L_COLORREF ( pWRPEXT_CALLBACK pL_GETPIXELCOLOR)(
               pBITMAPHANDLE pBitmap,
               L_INT nRow,
               L_INT nCol);

typedef STATUSCALLBACK ( pWRPEXT_CALLBACK pL_GETSTATUSCALLBACK)(
               L_VOID** ppUserData);

typedef L_VOID ( pWRPEXT_CALLBACK pL_SETSTATUSCALLBACK)(
               STATUSCALLBACK pfnCallback,
               L_VOID* pUserData,
               STATUSCALLBACK* pfnOldCallback,
               L_VOID** ppOldUserData);

typedef STATUSCALLBACK ( pWRPEXT_CALLBACK pL_GETCOPYSTATUSCALLBACK)(
               L_VOID** ppUserData);

typedef L_VOID ( pWRPEXT_CALLBACK pL_SETCOPYSTATUSCALLBACK)(
               STATUSCALLBACK pfnCallback,
               L_VOID* pUserData,
               STATUSCALLBACK* pOldFunction,
               L_VOID** ppUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_GRAYSCALEBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_INT nBitsPerPixel);

typedef L_INT ( pWRPEXT_CALLBACK pL_INITBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nWidth,
               L_INT nHeight,
               L_INT nBitsPerPixel);

typedef L_INT ( pWRPEXT_CALLBACK pL_INSERTBITMAPLISTITEM)(
               HBITMAPLIST hList,
               L_UINT uIndex,
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_ISGRAYSCALEBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_ISSUPPORTLOCKED)(
               L_UINT uType);

typedef L_INT ( pWRPEXT_CALLBACK pL_POINTFROMBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_INT ViewPerspective,
               L_INT* px,
               L_INT* py);

typedef L_INT ( pWRPEXT_CALLBACK pL_POINTTOBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_INT ViewPerspective,
               L_INT* px,
               L_INT* py);

typedef L_INT ( pWRPEXT_CALLBACK pL_PUTBITMAPCOLORS)(
               pBITMAPHANDLE pBitmap,
               L_INT nIndex,
               L_INT nCount,
               L_RGBQUAD* pPalette);

typedef L_SSIZE_T ( pWRPEXT_CALLBACK pL_PUTBITMAPROW)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pBuffer,
               L_INT nRow,
               L_SIZE_T uBytes);

typedef L_SSIZE_T ( pWRPEXT_CALLBACK pL_PUTBITMAPROWCOL)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pBuffer,
               L_INT nRow,
               L_INT nCol,
               L_SIZE_T uBytes);

typedef L_SSIZE_T ( pWRPEXT_CALLBACK pL_PUTBITMAPROWCOLCOMPRESSED)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pWorkBuffer,
               L_UINT16* pRunBuffer,
               L_INT nRow,
               L_INT nCol,
               L_UINT uWidth);

typedef L_INT ( pWRPEXT_CALLBACK pL_PUTBITMAPROWCOMPRESSED)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pWorkBuffer,
               L_UINT16* pRunBuffer,
               L_UINT nRow,
               L_UINT nLines);

typedef L_INT ( pWRPEXT_CALLBACK pL_PUTPIXELCOLOR)(
               pBITMAPHANDLE pBitmap,
               L_INT nRow,
               L_INT nCol,
               L_COLORREF crColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_RECTFROMBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_INT ViewPerspective,
               L_RECT* lprc);

typedef L_INT ( pWRPEXT_CALLBACK pL_RECTTOBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_INT nViewPerspective,
               L_RECT* lprc);

typedef L_VOID ( pWRPEXT_CALLBACK pL_REDIRECTIO)(
               REDIRECTOPEN pfnOpen,
               REDIRECTREAD pfnRead,
               REDIRECTWRITE pfnWrite,
               REDIRECTSEEK pfnSeek,
               REDIRECTCLOSE pfnClose,
               L_VOID* pUserData);

typedef L_VOID ( pWRPEXT_CALLBACK pL_RELEASEBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_REMOVEBITMAPLISTITEM)(
               HBITMAPLIST hList,
               L_UINT uIndex,
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_RESIZE)(
               L_UCHAR* pBuffer,
               L_INT nRow,
               L_INT nBitsPerPixel,
               L_INT* pXSize,
               L_INT* pYSize,
               L_VOID* pResizeData);

typedef L_INT ( pWRPEXT_CALLBACK pL_RESIZEBITMAP)(
               pBITMAPHANDLE pBitmapSrc,
               pBITMAPHANDLE pDestBitmap,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_REVERSEBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_ROTATEBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_INT nAngle,
               L_UINT uFlags,
               L_COLORREF crFill);

typedef L_INT ( pWRPEXT_CALLBACK pL_ROTATEBITMAPVIEWPERSPECTIVE)(
               pBITMAPHANDLE pBitmap,
               L_INT nAngle);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPDATAPOINTER)(
               pBITMAPHANDLE pBitmap,
               L_UCHAR* pData,
               L_SIZE_T dwSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPLISTITEM)(
               HBITMAPLIST hList,
               L_UINT uIndex,
               pBITMAPHANDLE pBitmap);

typedef L_UINT* ( pWRPEXT_CALLBACK pL_SETUSERMATCHTABLE)(
               L_UINT* pTable);

typedef L_INT ( pWRPEXT_CALLBACK pL_SIZEBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_INT nWidth,
               L_INT nHeight,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTDITHERING)(
               pBITMAPHANDLE pBitmap,
               L_RGBQUAD* pPalette,
               L_UINT uColors);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTRESIZE)(
               L_INT nOldWidth,
               L_INT nOldHeight,
               L_INT nNewWidth,
               L_INT nNewHeight,
               L_VOID** ppResizeData);

typedef L_INT ( pWRPEXT_CALLBACK pL_STOPDITHERING)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_STOPRESIZE)(
               L_VOID* pResizeData);

typedef L_COLORREF ( pWRPEXT_CALLBACK pL_TRANSLATEBITMAPCOLOR)(
               pBITMAPHANDLE pBitmapDst,
               pBITMAPHANDLE pBitmapSrc,
               L_COLORREF crColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_TOGGLEBITMAPCOMPRESSION)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_TRIMBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_INT nCol,
               L_INT nRow,
               L_UINT uWidth,
               L_UINT uHeight);

typedef L_VOID ( pWRPEXT_CALLBACK pL_UNLOCKSUPPORT)(
               L_UINT uType,
               L_TCHAR* pKey);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_KERNELHASEXPIRED)(
               L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_FLIPBITMAPVIEWPERSPECTIVE)(
         pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_REVERSEBITMAPVIEWPERSPECTIVE)(
         pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTRESIZEBITMAP)(
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

typedef L_INT ( pWRPEXT_CALLBACK pL_GETRESIZEDROWCOL)(
         L_VOID* pResizeData,
         L_UCHAR* pBuffer,
         L_INT nRow,
         L_INT nCol,
         L_SIZE_T uBytes);

typedef L_INT ( pWRPEXT_CALLBACK pL_STOPRESIZEBITMAP)(
         L_VOID* pResizeData);

typedef L_INT ( pWRPEXT_CALLBACK pL_MOVEBITMAPLISTITEMS)(
         pHBITMAPLIST phList,
         HBITMAPLIST hList,
         L_UINT uIndex,
         L_UINT uCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPCOMPRESSION)(
         pBITMAPHANDLE pBitmap,
         L_INT nComp);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPIXELDATA)(
         pBITMAPHANDLE pBitmap,
         L_VOID*pData,
         L_INT nRow,
         L_INT nCol,
         L_SIZE_T uBytes);

typedef L_INT ( pWRPEXT_CALLBACK pL_PUTPIXELDATA)(
         pBITMAPHANDLE pBitmap,
         L_VOID* pData,
         L_INT nRow,
         L_INT nCol,
         L_SIZE_T uBytes);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETDEFAULTMEMORYTYPE)(
         L_UINT uMemory);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETDEFAULTMEMORYTYPE)(
         L_UINT* puMemory);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETMEMORYTHRESHOLDS)(
         L_INT nTiledThreshold,
         L_SSIZE_T nMaxConvSize,
         L_SSIZE_T nTileSize,
         L_INT nConvTiles,
         L_INT nConvBuffers,
         L_UINT uFlags);

typedef L_VOID ( pWRPEXT_CALLBACK pL_GETMEMORYTHRESHOLDS)(
         L_INT* pnTiledThreshold,
         L_SSIZE_T* pnMaxConvSize,
         L_SSIZE_T* pnTileSize,
         L_INT* pnConvTiles,
         L_INT* pnConvBuffers);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPMEMORYINFO)(
         pBITMAPHANDLE pBitmap,
         L_UINT uMemory,
         L_SSIZE_T uTileSize,
         L_UINT uTotalTiles,
         L_UINT uConvTiles,
         L_UINT uMaxTileViews,
         L_UINT uTileViews,
         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPMEMORYINFO)(
         pBITMAPHANDLE pBitmap,
         L_UINT* puMemory,
         L_SSIZE_T* puTileSize,
         L_UINT* puTotalTiles,
         L_UINT* puConvTiles,
         L_UINT* puMaxTileViews,
         L_UINT* puTileViews);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETTEMPDIRECTORY)(
         L_TCHAR* pszTempDir);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETTEMPDIRECTORY)(
         L_TCHAR* pszTempDir,
         L_SIZE_T uSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPPALETTE)(
         pBITMAPHANDLE pBitmap,
         L_HPALETTE hPalette);

typedef L_INT ( pWRPEXT_CALLBACK pL_SCRAMBLEBITMAP)(
         pBITMAPHANDLE pBitmap,
         L_INT nColStart,
         L_INT nRowStart,
         L_INT nWidth,
         L_INT nHeight,
         L_UINT uKey,
         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COMBINEBITMAPWARP)(
         pBITMAPHANDLE pBitmapDst,
         L_POINT ptDstArray[],
         pBITMAPHANDLE pBitmapSrc,
         L_POINT ptSrc,
         L_INT nSrcWidth,
         L_INT nSrcHeight,
         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETOVERLAYBITMAP)(
         pBITMAPHANDLE pBitmap,
         L_INT nIndex,
         pBITMAPHANDLE pOverlayBitmap,
         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETOVERLAYBITMAP)(
         pBITMAPHANDLE pBitmap,
         L_INT nIndex,
         pBITMAPHANDLE pOverlayBitmap,
         L_UINT uStructSize,
         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETOVERLAYATTRIBUTES)(
         pBITMAPHANDLE pBitmap,
         L_INT nIndex,
         pOVERLAYATTRIBUTES pOverlayAttributes,
         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETOVERLAYATTRIBUTES)(
         pBITMAPHANDLE pBitmap,
         L_INT nIndex,
         pOVERLAYATTRIBUTES pOverlayAttributes,
         L_UINT uStructSize,
         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_UPDATEBITMAPOVERLAYBITS)(
         pBITMAPHANDLE pBitmap,
         L_INT nIndex,
         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETOVERLAYCOUNT)(
         pBITMAPHANDLE pBitmap,
         L_UINT*puCount,
         L_UINT uFlags);

typedef L_HDC ( pWRPEXT_CALLBACK pL_CREATELEADDC)(
         pBITMAPHANDLE pBitmap);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_DELETELEADDC)(
         L_HDC hDC);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPALPHA)(
         pBITMAPHANDLE pBitmap,
         pBITMAPHANDLE pAlpha,
         L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPALPHA)(
         pBITMAPHANDLE pBitmap,
         pBITMAPHANDLE pAlpha);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPALPHAVALUES)(
         pBITMAPHANDLE pBitmap,
         L_UINT16 uAlpha);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHEARBITMAP)(
         pBITMAPHANDLE pBitmap,
         L_INT nAngle,
         L_BOOL fHorizontal,
         L_COLORREF crFill);

typedef L_INT ( pWRPEXT_CALLBACK pL_VERSIONINFO)(
         pVERSIONINFO pVersionInfo,
         L_UINT uStructSize);

typedef L_BOOL( pWRPEXT_CALLBACK pL_HASEXPIRED)(
         L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEBITMAPLISTOPTPAL)(HBITMAPLIST hList,
                                 LPRGBQUAD pPalette,
                                 L_UINT *puColors,
                                 L_UINT * * ppMatchTable,
                                 L_BOOL *pbGenerated);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEGRAYSCALEBITMAP)(pBITMAPHANDLE pDstBitmap, pBITMAPHANDLE pSrcBitmap, L_INT uBitsPerPixel);
//-----------------------------------------------------------------------------
//--LTIMG.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

#if defined (LEADTOOLS_V16_OR_LATER)
typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPCONTRAST)(
            pBITMAPHANDLE  pBitmap,
            L_INT          nChange,
            L_UINT32       uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPHUE)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nAngle,
               L_UINT32       uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPINTENSITY)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nChange,
               L_UINT32       uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPSATURATION)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nChange, 
               L_UINT32       uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_INVERTBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_UINT32      uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHARPENBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nSharpness, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPCOLORCOUNT)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *puCount, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETAUTOTRIMRECT)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uThreshold,
               RECT           *pRect, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_AUTOTRIMBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uThreshold, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESPECKLEBITMAP)(
               pBITMAPHANDLE pBitmap, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_EDGEDETECTORBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nThreshold,
               L_UINT         uFilter, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_INTENSITYDETECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uLow,
               L_UINT         uHigh,
               COLORREF       crInColor,
               COLORREF       crOutColor,
               L_UINT         uChannel, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_AVERAGEFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_BINARYFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBINARYFLT     pFilter, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COMBINEBITMAP)(
               pBITMAPHANDLE  pBitmapDst,
               L_INT          nXDst,
               L_INT          nYDst,
               L_INT          nWidth,
               L_INT          nHeight,
               pBITMAPHANDLE  pBitmapSrc,
               L_INT          nXSrc,
               L_INT          nYSrc,
               L_UINT         uFlag, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MINFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MAXFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADDBITMAPNOISE)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uRange,
               L_UINT         uChannel, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_EMBOSSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDirection,
               L_UINT         uDepth, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GAMMACORRECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uGamma, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMINMAXBITS)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pLowBit,
               L_INT          *pHighBit, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMINMAXVAL)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pMinVal,
               L_INT          *pMaxVal, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_HISTOCONTRASTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nChange, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MEDIANFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MOSAICBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_POSTERIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uLevels, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_LINEPROFILE)(
               pBITMAPHANDLE  pBitmap,
               POINT          FirstPoint,
               POINT          SecondPoint,
               L_INT          **pRed,
               L_INT          **pGreen,
               L_INT          **pBlue, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GRAYSCALEBITMAPEXT)(
               pBITMAPHANDLE  pBitmap,
               L_INT          RedFact,
               L_INT          GreenFact,
               L_INT          BlueFact, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SWAPCOLORS)(
               pBITMAPHANDLE  pBitmap, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_BALANCECOLORS)(
               pBITMAPHANDLE  pBitmap,
               BALANCING      *pRedFact,
               BALANCING      *pGreenFact,
               BALANCING      *pBlueFact, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTTOCOLOREDGRAY)(
               pBITMAPHANDLE  pBitmap,
               L_INT          RedFact,
               L_INT          GreenFact,
               L_INT          BlueFact,
               L_INT          RedGrayFact,
               L_INT          GreenGrayFact,
               L_INT          BlueGrayFact, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_HISTOEQUALIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ALPHABLENDBITMAP)(
               pBITMAPHANDLE pBitmapDst,
               L_INT          nXDst,
               L_INT          nYDst,
               L_INT          nWidth,
               L_INT          nHeight,
               pBITMAPHANDLE  pBitmapSrc,
               L_INT          nXSrc,
               L_INT          nYSrc,
               L_INT          nOpacity, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANTIALIASBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nThreshold,
               L_UINT         uDim,
               L_UINT         uFilter, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETUSERLOOKUPTABLE)(
               L_UINT         *pLookupTable,
               L_UINT         uLookupLen,
               POINT          *apUserPoint,
               L_UINT         uUserPointCount,
               L_UINT         *puPointCount, 
               L_UINT32 uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTBITMAPSIGNEDTOUNSIGNED)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uShift, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_TEXTUREALPHABLENDBITMAP)(
               pBITMAPHANDLE pBitmapDst,
               L_INT          nXDst,
               L_INT          nYDst,
               L_INT          nWidth,
               L_INT          nHeight,
               pBITMAPHANDLE  pBitmapSrc,
               L_INT          nXSrc,
               L_INT          nYSrc,
               pBITMAPHANDLE  pBitmapMask,
               L_INT          nOpacity,
               pBITMAPHANDLE  pBitmapUnderlay,
               LPPOINT        pOffset, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_REMAPBITMAPHUE)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *pMask,
               L_UINT         *pHTable,
               L_UINT         *pSTable,
               L_UINT         *pVTable,
               L_UINT         uLUTLen, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MULTIPLYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uFactor, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOCALHISTOEQUALIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nWidth,
               L_INT          nHeight,
               L_INT          nxExt,
               L_INT          nyExt,
               L_UINT         uType,
               L_UINT         uSmooth, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SOLARIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uThreshold, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SPATIALFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSPATIALFLT    pFilter, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_STRETCHBITMAPINTENSITY)(
               pBITMAPHANDLE  pBitmap, 
               L_UINT32 uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_WINDOWLEVELBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nLowBit,
               L_INT          nHighBit,
               RGBQUAD        *pLUT,
               L_UINT         uLUTLength,
               L_INT          nOrderDst, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_WINDOWLEVELBITMAPEXT)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nLowBit,
               L_INT          nHighBit,
               L_RGBQUAD16    *pLUT,
               L_UINT         uLUTLength,
               L_INT          nOrderDst, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GAUSSIANFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nRadius, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SMOOTHBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSMOOTH        pSmooth,
               SMOOTHCALLBACK pfnCallback,
               L_VOID         *pUserData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_LINEREMOVEBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pLINEREMOVE          pLineRemove,
               LINEREMOVECALLBACK   pfnCallback,
               L_VOID               *pUserData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_RAKEREMOVEBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_BOOL bAuto,
               pRAKEREMOVE pComb,
               RECT* pDstRect,
               L_INT nRectCount,
               RAKEREMOVECALLBACK pCallback,
               L_VOID   *pUserData,
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_OBJECTCOUNTER)(
               pBITMAPHANDLE pBitmap, 
               L_UINT *uCount, 
               OBJECTCOUNTERCALLBACK pCallback, 
               L_VOID *pUserData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_BORDERREMOVEBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pBORDERREMOVE        pBorderRemove,
               BORDERREMOVECALLBACK pfnCallback,
               L_VOID               *pUserData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_INVERTEDTEXTBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pINVERTEDTEXT        pInvertedText,
               INVERTEDTEXTCALLBACK pfnCallback,
               L_VOID               *pUserData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DOTREMOVEBITMAP)(
               pBITMAPHANDLE     pBitmap,
               pDOTREMOVE        pDotRemove,
               DOTREMOVECALLBACK pfnCallback,
               L_VOID            *pUserData, 
               L_UINT32 uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_HOLEPUNCHREMOVEBITMAP)(
               pBITMAPHANDLE           pBitmap,
               pHOLEPUNCH              pHolePunch,
               HOLEPUNCHREMOVECALLBACK pfnCallback,
               L_VOID                  *pUserData, 
               L_UINT32 uFlags);


            // L_StapleRemoveBitmap reserved for future use
typedef L_INT ( pWRPEXT_CALLBACK pL_STAPLEREMOVEBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pSTAPLE              pStaplePunch,
               STAPLEREMOVECALLBACK pfnCallback,
               L_VOID               *pUserData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_OILIFYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONTOURFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT16        nThreshold,
               L_INT16        nDeltaDirection,
               L_INT16        nMaximumError,
               L_INT          nOption, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MOTIONBLURBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_INT          nAngle,
               L_BOOL         bUnidirectional, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_PICTURIZEBITMAPLIST)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uCellWidth,
               L_UINT         uCellHeight,
               L_UINT         uLightnessFact,
               HBITMAPLIST    hList, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_PICTURIZEBITMAPSINGLE)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pThumbBitmap,
               L_UINT         uCellWidth,
               L_UINT         uCellHeight,
               L_UINT         uLightnessFact, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADDBORDER)(
               pBITMAPHANDLE  pBitmap,
               pADDBORDERINFO pAddBorderInfo, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADDFRAME)(
               pBITMAPHANDLE  pBitmap,
               pADDFRAMEINFO  pAddFrameInfo, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADDMESSAGETOBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pADDMESGINFO   pAddMesgInfo, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_EXTRACTMESSAGEFROMBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pADDMESGINFO   pAddMesgInfo, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CYLINDRICALBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nValue,
               L_UINT         uType, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_RADIALBLURBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uStress,
               POINT          CenterPt, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SWIRLBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nAngle,
               POINT          CenterPt, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ZOOMBLURBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uStress,
               POINT          CenterPt, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_IMPRESSIONISTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uHorzDim,
               L_UINT         uVertDim, 
               L_UINT32 uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_WINDBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_INT          nAngle,
               L_UINT         uOpacity, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_REMOVEREDEYEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               COLORREF       rcNewColor,
               L_UINT         uThreshold,
               L_INT          nLightness, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_HALFTONEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uType,
               L_INT32        nAngle,
               L_UINT         uDim,
               HBITMAPLIST    hList, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_HOLESREMOVALBITMAPRGN)(
               pBITMAPHANDLE  pBitmap, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_BUMPMAPBITMAP)(
               pBITMAPHANDLE  pBitmapDst,
               pBITMAPHANDLE  pBumpBitmap,
               pBUMPDATA      pBumpData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GLOWFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uBright,
               L_UINT         uThreshold, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_EDGEDETECTSTATISTICALBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_INT          nThreshold,
               COLORREF       crEdgeColor,
               COLORREF       crBkColor, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESATURATEBITMAP)(
               pBITMAPHANDLE  pBitmap, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SMOOTHEDGESBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         nAmount,
               L_UINT         nThreshold, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_AUTOBINARYBITMAP)(
               pBITMAPHANDLE  pBitmap, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANNELMIX)(
               pBITMAPHANDLE  pBitmap,
               COLORDATA      *pRedFactor,
               COLORDATA      *pGreenFactor,
               COLORDATA      *pBlueFactor, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_OCEANBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uAmplitude,
               L_UINT         uFrequency,
               L_BOOL         bLowerTrnsp, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_LIGHTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pLIGHTINFO     pLightInfo,
               L_UINT         uLightNo,
               L_UINT         uBright,
               L_UINT         uAmbient,
               COLORREF       crAmbientClr, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DRYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DRAWSTARBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSTARINFO      pStarInfo, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FRQFILTERMASKBITMAP)(
               pBITMAPHANDLE  pMaskBitmap,
               pFTARRAY       pFTArray,
               L_BOOL         bOnOff, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ALLOCFTARRAY)(
               pBITMAPHANDLE  pBitmap,
               pFTARRAY       *ppFTArray,
               L_UINT         uStructSize, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEFTARRAY)(
               pFTARRAY pFTArray, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SKELETONBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nThreshold, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SELECTIVECOLORBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSELCLR        pSelClr, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CORRELATIONBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pCorBitmap,
               POINT          *pPoints,
               L_UINT         uMaxPoints,
               L_UINT         *puNumOfPoints,
               L_UINT         uXStep,
               L_UINT         uYStep,
               L_UINT          uThreshold, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETOBJECTINFO)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *puX,
               L_UINT         *puY,
               L_INT          *pnAngle,
               L_UINT         *puRoundness,
               L_BOOL         bWeighted, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETRGNPERIMETERLENGTH)(
               pBITMAPHANDLE  pBitmap,
               pRGNXFORM      pXForm,
               L_SIZE_T      *puLength, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETFERETSDIAMETER)(
               POINT    *pPoints,
               L_UINT   uSize,
               L_UINT   *puFeretsDiameter,
               L_UINT   *puFirstIndex,
               L_UINT   *puSecondIndex, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORREPLACEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pCOLORREPLACE  pColorReplace,
               L_UINT         uColorCount,
               L_INT          nHue,
               L_INT          nSaturation,
               L_INT          nBrightness, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEHUESATINTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nHue,
               L_INT          nSaturation,
               L_INT          nIntensity,
               pHSIDATA       pHsiData,
               L_UINT         uHsiDataCount, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPSTATISTICSINFO)(
               pBITMAPHANDLE     pBitmap,
               pSTATISTICSINFO   pStatisticsInfo,
               L_UINT            uChannel,
               L_UINT            uStart,
               L_UINT            uEnd, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_USERFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pUSERFLT       pFilter, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORREPLACEWEIGHTSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pCOLORREPLACE  pColorReplace,
               L_UINT         uColorCount, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DIRECTIONEDGESTATISTICALBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_INT          nThreshold,
               L_INT          nAngle,
               COLORREF       crEdgeColor,
               COLORREF       crBkColor, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MATHFUNCTIONBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uMType,
               L_UINT         uFactor, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORTHRESHOLDBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uColorSpace,
               pCOMPDATA      pCompData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_REVEFFECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uLineSpace,
               L_UINT         uMaximumHeight, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DYNAMICBINARYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uLocalContrast, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORINTENSITYBALANCE)(
               pBITMAPHANDLE  pBitmap,
               pBALANCEDATA   pShadows,
               pBALANCEDATA   pMidTone,
               pBALANCEDATA   pHighLight,
               L_BOOL         bLuminance, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FUNCTIONALLIGHTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pLIGHTPARAMS   pLightParams, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHIFTBITMAPDATA)(
               pBITMAPHANDLE  pDstBitmap,
               pBITMAPHANDLE  pSrcBitmap,
               L_UINT         uSrcLowBit,
               L_UINT         uSrcHighBit,
               L_UINT         uDstLowBit,
               L_UINT         uDstBitsPerPixel, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SELECTBITMAPDATA)(
               pBITMAPHANDLE  pDstBitmap,
               pBITMAPHANDLE  pSrcBitmap,
               COLORREF       crColor,
               L_UINT         uSrcLowBit,
               L_UINT         uSrcHighBit,
               L_UINT         uThreshold,
               L_BOOL         bCombine, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORIZEGRAYBITMAP)(
               pBITMAPHANDLE  pDstBitmap,
               pBITMAPHANDLE  pSrcBitmap,
               pLTGRAYCOLOR   pGrayColors,
               L_UINT         uCount, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONTBRIGHTINTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nContrast,
               L_INT          nBrightness,
               L_INT          nIntensity, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ISREGMARKBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uType,
               L_UINT         uMinScale,
               L_UINT         uMaxScale,
               L_UINT         uWidth,
               L_UINT         uHeight, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMARKSCENTERMASSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               POINT          *pMarkPoints,
               POINT          *pMarkCMPoints,
               L_UINT         uMarksCount, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SEARCHREGMARKSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSEARCHMARKS   pSearchMarks,
               L_UINT         uMarkCount, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETTRANSFORMATIONPARAMETERS)(
               pBITMAPHANDLE  pBitmap,
               POINT          *pRefPoints,
               POINT          *pTrnsPoints,
               L_INT          *pnXTranslation,
               L_INT          *pnYTranslation,
               L_INT          *pnAngle,
               L_UINT         *puXScale,
               L_UINT         *puYScale, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FRAGMENTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uOffset,
               L_UINT         uOpacity, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_VIGNETTEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pVIGNETTEINFO  pVignetteInfo, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MOSAICTILESBITMAP)(
               pBITMAPHANDLE     pBitmap,
               pMOSAICTILESINFO  pMosaicTilesInfo, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_PLASMAFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pPLASMAINFO    pPlasmaInfo, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADJUSTBITMAPTINT)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nAngleA,
               L_INT          nAngleB, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLOREDPENCILBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uRatio,
               L_UINT         uDim, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DIFFUSEGLOWBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nGlowAmount,
               L_UINT         uClearAmount,
               L_UINT         uSpreadAmount,
               L_UINT         uWhiteNoise,
               COLORREF       crGlowColor, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_HIGHPASSFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uRadius,
               L_UINT         uOpacity, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORHALFTONEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uMaxRad,
               L_INT          nCyanAngle,
               L_INT          nMagentaAngle,
               L_INT          nYellowAngle,
               L_INT          nBlackAngle, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CORRELATIONLISTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               HBITMAPLIST    hCorList,
               POINT          *pPoints,
               L_UINT         *puListIndex,
               L_UINT         uMaxPoints,
               L_UINT         *puNumOfPoints,
               L_UINT         uXStep,
               L_UINT         uYStep,
               L_UINT         uThreshold, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETKAUFMANNRGNBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pKaufmannProcBitmap,
               L_INT          nRadius,
               L_INT          nMinInput,
               L_INT          nMaxInput,
               L_INT          nRgnThreshold,
               POINT          ptRgnStart,
               L_BOOL         bRemoveHoles,
               L_SIZE_T      *puPixelsCount,
               L_UINT         uCombineMode, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SLICEBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pSLICEBITMAPOPTIONS  pOptions,
               L_INT                *pnDeskewAngle,
               BITMAPSLICECALLBACK  pfnCallback,
               L_VOID               *pUserData, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHIFTMINIMUMTOZERO)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *puShiftAmount, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHIFTZEROTONEGATIVE)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nShiftAmount,
               L_INT          nMinInput,
               L_INT          nMaxInput,
               L_INT          nMinOutput,
               L_INT          nMaxOutput, 
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FEATHERALPHABLENDBITMAP)(
               pBITMAPHANDLE  pBitmapDst,
               L_INT          nXDst,
               L_INT          nYDst,
               L_INT          nWidth,
               L_INT          nHeight,
               pBITMAPHANDLE  pBitmapSrc,
               L_INT          nXSrc,
               L_INT          nYSrc,
               pBITMAPHANDLE  pBitmapMask,
               L_INT          nXMaskShift,
               L_INT          nYMaskShift, 
               L_UINT32 uFlags);

#else
typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPCONTRAST)(
            pBITMAPHANDLE  pBitmap,
            L_INT          nChange);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPHUE)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nAngle);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPINTENSITY)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nChange);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEBITMAPSATURATION)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nChange);

typedef L_INT ( pWRPEXT_CALLBACK pL_INVERTBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHARPENBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nSharpness);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPCOLORCOUNT)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *puCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETAUTOTRIMRECT)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uThreshold,
               RECT           *pRect);

typedef L_INT ( pWRPEXT_CALLBACK pL_AUTOTRIMBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uThreshold);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESPECKLEBITMAP)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_EDGEDETECTORBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nThreshold,
               L_UINT         uFilter);

typedef L_INT ( pWRPEXT_CALLBACK pL_INTENSITYDETECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uLow,
               L_UINT         uHigh,
               COLORREF       crInColor,
               COLORREF       crOutColor,
               L_UINT         uChannel);

typedef L_INT ( pWRPEXT_CALLBACK pL_AVERAGEFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim);

typedef L_INT ( pWRPEXT_CALLBACK pL_BINARYFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBINARYFLT     pFilter);

typedef L_INT ( pWRPEXT_CALLBACK pL_COMBINEBITMAP)(
               pBITMAPHANDLE  pBitmapDst,
               L_INT          nXDst,
               L_INT          nYDst,
               L_INT          nWidth,
               L_INT          nHeight,
               pBITMAPHANDLE  pBitmapSrc,
               L_INT          nXSrc,
               L_INT          nYSrc,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_MINFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim);

typedef L_INT ( pWRPEXT_CALLBACK pL_MAXFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADDBITMAPNOISE)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uRange,
               L_UINT         uChannel);

typedef L_INT ( pWRPEXT_CALLBACK pL_EMBOSSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDirection,
               L_UINT         uDepth);

typedef L_INT ( pWRPEXT_CALLBACK pL_GAMMACORRECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uGamma);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMINMAXBITS)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pLowBit,
               L_INT          *pHighBit);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMINMAXVAL)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pMinVal,
               L_INT          *pMaxVal);

typedef L_INT ( pWRPEXT_CALLBACK pL_HISTOCONTRASTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nChange);

typedef L_INT ( pWRPEXT_CALLBACK pL_MEDIANFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim);

typedef L_INT ( pWRPEXT_CALLBACK pL_MOSAICBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim);

typedef L_INT ( pWRPEXT_CALLBACK pL_POSTERIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uLevels);

typedef L_INT ( pWRPEXT_CALLBACK pL_LINEPROFILE)(
               pBITMAPHANDLE  pBitmap,
               POINT          FirstPoint,
               POINT          SecondPoint,
               L_INT          **pRed,
               L_INT          **pGreen,
               L_INT          **pBlue);

typedef L_INT ( pWRPEXT_CALLBACK pL_GRAYSCALEBITMAPEXT)(
               pBITMAPHANDLE  pBitmap,
               L_INT          RedFact,
               L_INT          GreenFact,
               L_INT          BlueFact);

typedef L_INT ( pWRPEXT_CALLBACK pL_SWAPCOLORS)(
               pBITMAPHANDLE pBitmap, L_INT nFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_BALANCECOLORS)(
               pBITMAPHANDLE  pBitmap,
               BALANCING      *pRedFact,
               BALANCING      *pGreenFact,
               BALANCING      *pBlueFact);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTTOCOLOREDGRAY)(
               pBITMAPHANDLE  pBitmap,
               L_INT          RedFact,
               L_INT          GreenFact,
               L_INT          BlueFact,
               L_INT          RedGrayFact,
               L_INT          GreenGrayFact,
               L_INT          BlueGrayFact);

typedef L_INT ( pWRPEXT_CALLBACK pL_HISTOEQUALIZEBITMAP)(
               pBITMAPHANDLE pBitmap,L_INT nFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_ALPHABLENDBITMAP)(
               pBITMAPHANDLE pBitmapDst,
               L_INT          nXDst,
               L_INT          nYDst,
               L_INT          nWidth,
               L_INT          nHeight,
               pBITMAPHANDLE  pBitmapSrc,
               L_INT          nXSrc,
               L_INT          nYSrc,
               L_INT          nOpacity);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANTIALIASBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nThreshold,
               L_UINT         uDim,
               L_UINT         uFilter);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETUSERLOOKUPTABLE)(
               L_UINT         *pLookupTable,
               L_UINT         uLookupLen,
               POINT          *apUserPoint,
               L_UINT         uUserPointCount,
               L_UINT         *puPointCount);
typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTBITMAPSIGNEDTOUNSIGNED)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uShift);

typedef L_INT ( pWRPEXT_CALLBACK pL_TEXTUREALPHABLENDBITMAP)(
               pBITMAPHANDLE pBitmapDst,
               L_INT          nXDst,
               L_INT          nYDst,
               L_INT          nWidth,
               L_INT          nHeight,
               pBITMAPHANDLE  pBitmapSrc,
               L_INT          nXSrc,
               L_INT          nYSrc,
               pBITMAPHANDLE  pBitmapMask,
               L_INT          nOpacity,
               pBITMAPHANDLE  pBitmapUnderlay,
               LPPOINT        pOffset);

typedef L_INT ( pWRPEXT_CALLBACK pL_REMAPBITMAPHUE)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *pMask,
               L_UINT         *pHTable,
               L_UINT         *pSTable,
               L_UINT         *pVTable,
               L_UINT         uLUTLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_MULTIPLYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uFactor);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOCALHISTOEQUALIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nWidth,
               L_INT          nHeight,
               L_INT          nxExt,
               L_INT          nyExt,
               L_UINT         uType,
               L_UINT         uSmooth);
typedef L_INT ( pWRPEXT_CALLBACK pL_SOLARIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uThreshold);

typedef L_INT ( pWRPEXT_CALLBACK pL_SPATIALFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSPATIALFLT    pFilter);

typedef L_INT ( pWRPEXT_CALLBACK pL_STRETCHBITMAPINTENSITY)(
               pBITMAPHANDLE  pBitmap);


typedef L_INT ( pWRPEXT_CALLBACK pL_WINDOWLEVELBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nLowBit,
               L_INT          nHighBit,
               RGBQUAD        *pLUT,
               L_UINT         uLUTLength,
               L_INT          nOrderDst);

typedef L_INT ( pWRPEXT_CALLBACK pL_GAUSSIANFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nRadius);

typedef L_INT ( pWRPEXT_CALLBACK pL_SMOOTHBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSMOOTH        pSmooth,
               SMOOTHCALLBACK pfnCallback,
               L_VOID         *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_LINEREMOVEBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pLINEREMOVE          pLineRemove,
               LINEREMOVECALLBACK   pfnCallback,
               L_VOID               *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_BORDERREMOVEBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pBORDERREMOVE        pBorderRemove,
               BORDERREMOVECALLBACK pfnCallback,
               L_VOID               *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_INVERTEDTEXTBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pINVERTEDTEXT        pInvertedText,
               INVERTEDTEXTCALLBACK pfnCallback,
               L_VOID               *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_DOTREMOVEBITMAP)(
               pBITMAPHANDLE     pBitmap,
               pDOTREMOVE        pDotRemove,
               DOTREMOVECALLBACK pfnCallback,
               L_VOID            *pUserData);


typedef L_INT ( pWRPEXT_CALLBACK pL_HOLEPUNCHREMOVEBITMAP)(
               pBITMAPHANDLE           pBitmap,
               pHOLEPUNCH              pHolePunch,
               HOLEPUNCHREMOVECALLBACK pfnCallback,
               L_VOID                  *pUserData);


            // L_StapleRemoveBitmap reserved for future use
typedef L_INT ( pWRPEXT_CALLBACK pL_STAPLEREMOVEBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pSTAPLE              pStaplePunch,
               STAPLEREMOVECALLBACK pfnCallback,
               L_VOID               *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_OILIFYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim);
typedef L_INT ( pWRPEXT_CALLBACK pL_CONTOURFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT16        nThreshold,
               L_INT16        nDeltaDirection,
               L_INT16        nMaximumError,
               L_INT          nOption);
typedef L_INT ( pWRPEXT_CALLBACK pL_MOTIONBLURBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_INT          nAngle,
               L_BOOL         bUnidirectional);
typedef L_INT ( pWRPEXT_CALLBACK pL_PICTURIZEBITMAPLIST)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uCellWidth,
               L_UINT         uCellHeight,
               L_UINT         uLightnessFact,
               HBITMAPLIST    hList);

typedef L_INT ( pWRPEXT_CALLBACK pL_PICTURIZEBITMAPSINGLE)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pThumbBitmap,
               L_UINT         uCellWidth,
               L_UINT         uCellHeight,
               L_UINT         uLightnessFact);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADDBORDER)(
               pBITMAPHANDLE  pBitmap,
               pADDBORDERINFO pAddBorderInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADDFRAME)(
               pBITMAPHANDLE  pBitmap,
               pADDFRAMEINFO  pAddFrameInfo);
typedef L_INT ( pWRPEXT_CALLBACK pL_ADDMESSAGETOBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pADDMESGINFO   pAddMesgInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_EXTRACTMESSAGEFROMBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pADDMESGINFO   pAddMesgInfo);
typedef L_INT ( pWRPEXT_CALLBACK pL_CYLINDRICALBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nValue,
               L_UINT         uType);
typedef L_INT ( pWRPEXT_CALLBACK pL_RADIALBLURBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uStress,
               POINT          CenterPt);
typedef L_INT ( pWRPEXT_CALLBACK pL_SWIRLBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nAngle,
               POINT          CenterPt);

typedef L_INT ( pWRPEXT_CALLBACK pL_ZOOMBLURBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uStress,
               POINT          CenterPt);
typedef L_INT ( pWRPEXT_CALLBACK pL_IMPRESSIONISTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uHorzDim,
               L_UINT         uVertDim);
typedef L_INT ( pWRPEXT_CALLBACK pL_WINDBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_INT          nAngle,
               L_UINT         uOpacity);
typedef L_INT ( pWRPEXT_CALLBACK pL_REMOVEREDEYEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               COLORREF       rcNewColor,
               L_UINT         uThreshold,
               L_INT          nLightness);
typedef L_INT ( pWRPEXT_CALLBACK pL_HALFTONEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uType,
               L_INT32        nAngle,
               L_UINT         uDim,
               HBITMAPLIST    hList);

typedef L_INT ( pWRPEXT_CALLBACK pL_HOLESREMOVALBITMAPRGN)(
               pBITMAPHANDLE  pBitmap);
typedef L_INT ( pWRPEXT_CALLBACK pL_BUMPMAPBITMAP)(
               pBITMAPHANDLE  pBitmapDst,
               pBITMAPHANDLE  pBumpBitmap,
               pBUMPDATA      pBumpData);

typedef L_INT ( pWRPEXT_CALLBACK pL_GLOWFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uBright,
               L_UINT         uThreshold);

typedef L_INT ( pWRPEXT_CALLBACK pL_EDGEDETECTSTATISTICALBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_INT          nThreshold,
               COLORREF       crEdgeColor,
               COLORREF       crBkColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESATURATEBITMAP)(
               pBITMAPHANDLE  pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_SMOOTHEDGESBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         nAmount,
               L_UINT         nThreshold);

typedef L_INT ( pWRPEXT_CALLBACK pL_AUTOBINARYBITMAP)(
               pBITMAPHANDLE  pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANNELMIX)(
               pBITMAPHANDLE  pBitmap,
               COLORDATA      *pRedFactor,
               COLORDATA      *pGreenFactor,
               COLORDATA      *pBlueFactor);
typedef L_INT ( pWRPEXT_CALLBACK pL_OCEANBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uAmplitude,
               L_UINT         uFrequency,
               L_BOOL         bLowerTrnsp);

typedef L_INT ( pWRPEXT_CALLBACK pL_LIGHTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pLIGHTINFO     pLightInfo,
               L_UINT         uLightNo,
               L_UINT         uBright,
               L_UINT         uAmbient,
               COLORREF       crAmbientClr);

typedef L_INT ( pWRPEXT_CALLBACK pL_DRYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim);

typedef L_INT ( pWRPEXT_CALLBACK pL_DRAWSTARBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSTARINFO      pStarInfo);
typedef L_INT ( pWRPEXT_CALLBACK pL_FRQFILTERMASKBITMAP)(
               pBITMAPHANDLE  pMaskBitmap,
               pFTARRAY       pFTArray,
               L_BOOL         bOnOff);

typedef L_INT ( pWRPEXT_CALLBACK pL_ALLOCFTARRAY)(
               pBITMAPHANDLE  pBitmap,
               pFTARRAY       *ppFTArray,
               L_UINT         uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEFTARRAY)(
               pFTARRAY pFTArray);
typedef L_INT ( pWRPEXT_CALLBACK pL_SKELETONBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nThreshold);
typedef L_INT ( pWRPEXT_CALLBACK pL_SELECTIVECOLORBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSELCLR        pSelClr);
typedef L_INT ( pWRPEXT_CALLBACK pL_CORRELATIONBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pCorBitmap,
               POINT          *pPoints,
               L_UINT         uMaxPoints,
               L_UINT         *puNumOfPoints,
               L_UINT         uXStep,
               L_UINT         uYStep,
               L_UINT          uThreshold);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETOBJECTINFO)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *puX,
               L_UINT         *puY,
               L_INT          *pnAngle,
               L_UINT         *puRoundness,
               L_BOOL         bWeighted);
typedef L_INT ( pWRPEXT_CALLBACK pL_GETRGNPERIMETERLENGTH)(
               pBITMAPHANDLE  pBitmap,
               pRGNXFORM      pXForm,
               L_SIZE_T      *puLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETFERETSDIAMETER)(
               POINT    *pPoints,
               L_UINT   uSize,
               L_UINT   *puFeretsDiameter,
               L_UINT   *puFirstIndex,
               L_UINT   *puSecondIndex);
typedef L_INT ( pWRPEXT_CALLBACK pL_COLORREPLACEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pCOLORREPLACE  pColorReplace,
               L_UINT         uColorCount,
               L_INT          nHue,
               L_INT          nSaturation,
               L_INT          nBrightness);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEHUESATINTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nHue,
               L_INT          nSaturation,
               L_INT          nIntensity,
               pHSIDATA       pHsiData,
               L_UINT         uHsiDataCount);
typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPSTATISTICSINFO)(
               pBITMAPHANDLE     pBitmap,
               pSTATISTICSINFO   pStatisticsInfo,
               L_UINT            uChannel,
               L_UINT            uStart,
               L_UINT            uEnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_USERFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pUSERFLT       pFilter);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORREPLACEWEIGHTSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pCOLORREPLACE  pColorReplace,
               L_UINT         uColorCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_DIRECTIONEDGESTATISTICALBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_INT          nThreshold,
               L_INT          nAngle,
               COLORREF       crEdgeColor,
               COLORREF       crBkColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_MATHFUNCTIONBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uMType,
               L_UINT         uFactor);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORTHRESHOLDBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uColorSpace,
               pCOMPDATA      pCompData);

typedef L_INT ( pWRPEXT_CALLBACK pL_REVEFFECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uLineSpace,
               L_UINT         uMaximumHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_DYNAMICBINARYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uLocalContrast);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORINTENSITYBALANCE)(
               pBITMAPHANDLE  pBitmap,
               pBALANCEDATA   pShadows,
               pBALANCEDATA   pMidTone,
               pBALANCEDATA   pHighLight,
               L_BOOL         bLuminance);

typedef L_INT ( pWRPEXT_CALLBACK pL_FUNCTIONALLIGHTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pLIGHTPARAMS   pLightParams);
typedef L_INT ( pWRPEXT_CALLBACK pL_SHIFTBITMAPDATA)(
               pBITMAPHANDLE  pDstBitmap,
               pBITMAPHANDLE  pSrcBitmap,
               L_UINT         uSrcLowBit,
               L_UINT         uSrcHighBit,
               L_UINT         uDstLowBit,
               L_UINT         uDstBitsPerPixel);
typedef L_INT ( pWRPEXT_CALLBACK pL_SELECTBITMAPDATA)(
               pBITMAPHANDLE  pDstBitmap,
               pBITMAPHANDLE  pSrcBitmap,
               COLORREF       crColor,
               L_UINT         uSrcLowBit,
               L_UINT         uSrcHighBit,
               L_UINT         uThreshold,
               L_BOOL         bCombine);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORIZEGRAYBITMAP)(
               pBITMAPHANDLE  pDstBitmap,
               pBITMAPHANDLE  pSrcBitmap,
               pLTGRAYCOLOR   pGrayColors,
               L_UINT         uCount);
typedef L_INT ( pWRPEXT_CALLBACK pL_CONTBRIGHTINTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nContrast,
               L_INT          nBrightness,
               L_INT          nIntensity);

typedef L_INT ( pWRPEXT_CALLBACK pL_ISREGMARKBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uType,
               L_UINT         uMinScale,
               L_UINT         uMaxScale,
               L_UINT         uWidth,
               L_UINT         uHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMARKSCENTERMASSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               POINT          *pMarkPoints,
               POINT          *pMarkCMPoints,
               L_UINT         uMarksCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_SEARCHREGMARKSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pSEARCHMARKS   pSearchMarks,
               L_UINT         uMarkCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETTRANSFORMATIONPARAMETERS)(
               pBITMAPHANDLE  pBitmap,
               POINT          *pRefPoints,
               POINT          *pTrnsPoints,
               L_INT          *pnXTranslation,
               L_INT          *pnYTranslation,
               L_INT          *pnAngle,
               L_UINT         *puXScale,
               L_UINT         *puYScale);
typedef L_INT ( pWRPEXT_CALLBACK pL_FRAGMENTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uOffset,
               L_UINT         uOpacity);
typedef L_INT ( pWRPEXT_CALLBACK pL_VIGNETTEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pVIGNETTEINFO  pVignetteInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_MOSAICTILESBITMAP)(
               pBITMAPHANDLE     pBitmap,
               pMOSAICTILESINFO  pMosaicTilesInfo);
typedef L_INT ( pWRPEXT_CALLBACK pL_PLASMAFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pPLASMAINFO    pPlasmaInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADJUSTBITMAPTINT)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nAngleA,
               L_INT          nAngleB);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLOREDPENCILBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uRatio,
               L_UINT         uDim);
typedef L_INT ( pWRPEXT_CALLBACK pL_DIFFUSEGLOWBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nGlowAmount,
               L_UINT         uClearAmount,
               L_UINT         uSpreadAmount,
               L_UINT         uWhiteNoise,
               COLORREF       crGlowColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_HIGHPASSFILTERBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uRadius,
               L_UINT         uOpacity);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORHALFTONEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uMaxRad,
               L_INT          nCyanAngle,
               L_INT          nMagentaAngle,
               L_INT          nYellowAngle,
               L_INT          nBlackAngle);
typedef L_INT ( pWRPEXT_CALLBACK pL_CORRELATIONLISTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               HBITMAPLIST    hCorList,
               POINT          *pPoints,
               L_UINT         *puListIndex,
               L_UINT         uMaxPoints,
               L_UINT         *puNumOfPoints,
               L_UINT         uXStep,
               L_UINT         uYStep,
               L_UINT         uThreshold);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETKAUFMANNRGNBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pKaufmannProcBitmap,
               L_INT          nRadius,
               L_INT          nMinInput,
               L_INT          nMaxInput,
               L_INT          nRgnThreshold,
               POINT          ptRgnStart,
               L_BOOL         bRemoveHoles,
               L_SIZE_T      *puPixelsCount,
               L_UINT         uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SLICEBITMAP)(
               pBITMAPHANDLE        pBitmap,
               pSLICEBITMAPOPTIONS  pOptions,
               L_INT                *pnDeskewAngle,
               BITMAPSLICECALLBACK  pfnCallback,
               L_VOID               *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHIFTMINIMUMTOZERO)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *puShiftAmount);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHIFTZEROTONEGATIVE)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nShiftAmount,
               L_INT          nMinInput,
               L_INT          nMaxInput,
               L_INT          nMinOutput,
               L_INT          nMaxOutput);

typedef L_INT ( pWRPEXT_CALLBACK pL_FEATHERALPHABLENDBITMAP)(
               pBITMAPHANDLE  pBitmapDst,
               L_INT          nXDst,
               L_INT          nYDst,
               L_INT          nWidth,
               L_INT          nHeight,
               pBITMAPHANDLE  pBitmapSrc,
               L_INT          nXSrc,
               L_INT          nYSrc,
               pBITMAPHANDLE  pBitmapMask,
               L_INT          nXMaskShift,
               L_INT          nYMaskShift);

#endif


typedef L_INT ( pWRPEXT_CALLBACK pL_REMAPBITMAPINTENSITY)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pLUT,
               L_UINT         uLUTLen,
               L_UINT         uFlags);



typedef L_INT ( pWRPEXT_CALLBACK pL_UNSHARPMASKBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nAmount,
               L_INT          nRadius,
               L_INT          nThreshold,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESKEWBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pnAngle,
               COLORREF       crBack,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESKEWCHECKBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pnAngle,
               COLORREF       crBack,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_BLANKPAGEDETECTORBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_BOOL          *bIsBlank,
               L_UINT          *pAccuracy,
               pPAGEMARGINS    PMargins,
               L_UINT          uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_AUTOBINARIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uFactor,
               L_UINT          uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_STARTFASTMAGICWANDENGINE)(
               MAGICWANDHANDLE* pMagicWnd,
               pBITMAPHANDLE LeadBitmap,
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ENDFASTMAGICWANDENGINE)(
               MAGICWANDHANDLE MagicWnd,
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FASTMAGICWAND)(
               MAGICWANDHANDLE MagicWnd,
               L_INT nTolerance,
               L_INT nXposition,
               L_INT nYposition,
               pOBJECTINFO pObjectInfo,
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DELETEOBJECTINFO)(
               pOBJECTINFO pObjectInfo,
               L_UINT32 uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_TISSUEEQUALIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT          uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SIGMAFILTERBITMAP)(
               pBITMAPHANDLE pBitmap, 
               L_UINT nSize,
               L_UINT nSigma,
               L_FLOAT nThreshhold,                               
               L_BOOL bOutline,  
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_AUTOSEGMENTBITMAP)(
               pBITMAPHANDLE pBitmap,
               L_RECT * prcRect,
               L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESKEWBITMAPEXT)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pnAngle,
               L_UINT         uAngleRange,
               L_UINT         uAngleResolution,
               COLORREF       crBack,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORMERGEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  *ppBitmap,
               L_UINT         uStructSize,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORSEPARATEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  *ppBitmap,
               L_UINT         uStructSize,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPHISTOGRAM)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *pHisto,
               L_SIZE_T       uHistoLen,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_GETFUNCTIONALLOOKUPTABLE)(
               L_INT   *pLookupTable,
               L_UINT   uLookupLen,
               L_INT    nStart,
               L_INT    nEnd,
               L_INT    nFactor,
               L_UINT   uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_GETCURVEPOINTS)(
               L_INT    *pCurve,
               POINT    *apUserPoint,
               L_UINT   uUserPointCount,
               L_UINT   *puPointCount,
               L_UINT   uFlag);


typedef L_INT ( pWRPEXT_CALLBACK pL_PICTURIZEBITMAP)(
               pBITMAPHANDLE     pBitmap,
               L_TCHAR           *pszDirName,
               L_UINT            uFlags,
               L_INT             nCellWidth,
               L_INT             nCellHeight,
               PICTURIZECALLBACK pfnCallback,
               L_VOID            *pUserData);


typedef L_INT ( pWRPEXT_CALLBACK pL_RESIZEBITMAPRGN)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uFlag,
               L_BOOL         bAsFrame);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEFADEDMASK)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pMaskBitmap,
               L_UINT         uStructSize,
               L_INT          nLength,
               L_INT          nFadeRate,
               L_INT          nStepSize,
               L_INT          nInflate,
               L_UINT         uFlag,
               L_INT          nMaxGray,
               COLORREF       crTransparent);

typedef L_INT ( pWRPEXT_CALLBACK pL_ADDBITMAPS)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uStructSize,
               HBITMAPLIST    hList,
               L_UINT         uFlag);



typedef L_INT ( pWRPEXT_CALLBACK pL_ADDWEIGHTEDBITMAPS)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uStructSize,
               HBITMAPLIST    hList,
               L_UINT         *puFactor,
               L_UINT         uFlag);


typedef L_INT ( pWRPEXT_CALLBACK pL_PIXELATEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uCellWidth,
               L_UINT         uCellHeight,
               L_UINT         uOpacity,
               POINT          CenterPt,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SPHERIZEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nValue,
               POINT          CenterPt,
               COLORREF       crFill,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_BENDINGBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nValue,
               POINT          CenterPt,
               COLORREF       crFill,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_PUNCHBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nValue,
               L_UINT         uStress,
               POINT          CenterPt,
               COLORREF       crFill,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_POLARBITMAP)(
               pBITMAPHANDLE  pBitmap,
               COLORREF       crFill,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_RIPPLEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uAmplitude,
               L_UINT         uFrequency,
               L_INT          nPhase,
               L_UINT         uAttenuation,
               POINT          CenterPt,
               COLORREF       crFill,
               L_UINT         uFlag);


typedef L_INT ( pWRPEXT_CALLBACK pL_FREEHANDWAVEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pAmplitudes,
               L_UINT         uAmplitudesCount,
               L_UINT         uScale,
               L_UINT         uWaveLen,
               L_INT          nAngle,
               COLORREF       crFill,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_RADWAVEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uAmplitude,
               L_UINT         uWaveLen,
               L_INT          nPhase,
               POINT          pCenter,
               COLORREF       crFill,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEHANDSHEARBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pAmplitudes,
               L_UINT         uAmplitudesCount,
               L_UINT         uScale,
               COLORREF       crFill,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_WAVEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uAmplitude,
               L_UINT         uWaveLen,
               L_INT          nAngle,
               L_UINT         uHorzFact,
               L_UINT         uVertFact,
               COLORREF       crFill,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_ZOOMWAVEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uAmplitude,
               L_UINT         uFrequency,
               L_INT          nPhase,
               L_UINT         uZomFact,
               POINT          CenterPt,
               COLORREF       crFill,
               L_UINT         uFlag);


            //*** v14 functions ***
typedef L_INT ( pWRPEXT_CALLBACK pL_DEINTERLACEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAMPLETARGETBITMAP)(
               pBITMAPHANDLE  pBitmap,
               COLORREF       crSample,
               COLORREF       crTarget,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_CUBISMBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uSpace,
               L_UINT         uLength,
               L_INT          nBrightness,
               L_INT          nAngle,
               COLORREF       crColor,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_LIGHTCONTROLBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         *puLowerAvr,
               L_UINT         *puAvrage,
               L_UINT         *puUpperAvr,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_GLASSEFFECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uCellWidth,
               L_UINT         uCellHeight,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_LENSFLAREBITMAP)(
               pBITMAPHANDLE  pBitmap,
               POINT          ptCenter,
               L_UINT         uBright,
               L_UINT         uFlag,
               COLORREF       crColor);


typedef L_INT ( pWRPEXT_CALLBACK pL_PLANEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               POINT          ptCenterPoint,
               L_UINT         uZValue,
               L_INT          nDistance,
               L_UINT         uPlaneOffset,
               L_INT          nRepeat,
               L_INT          nPydAngle,
               L_UINT         uStretch,
               L_UINT         uStartBright,
               L_UINT         uEndBright,
               L_UINT         uBrightLength,
               COLORREF       crBright,
               COLORREF       crFill,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_PLANEBENDBITMAP)(
               pBITMAPHANDLE  pBitmap,
               POINT          ptCenterPoint,
               L_UINT         uZValue,
               L_INT          nDistance,
               L_UINT         uPlaneOffset,
               L_INT          nRepeat,
               L_INT          nPydAngle,
               L_UINT         uStretch,
               L_UINT         uBendFactor,
               L_UINT         uStartBright,
               L_UINT         uEndBright,
               L_UINT         uBrightLength,
               COLORREF       crBright,
               COLORREF       crFill,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_TUNNELBITMAP)(
               pBITMAPHANDLE  pBitmap,
               POINT          ptCenterPoint,
               L_UINT         uZValue,
               L_INT          nDistance,
               L_UINT         uRad,
               L_INT          nRepeat,
               L_INT          nRotationOffset,
               L_UINT         uStretch,
               L_UINT         uStartBright,
               L_UINT         uEndBright,
               L_UINT         uBrightLength,
               COLORREF       crBright,
               COLORREF       crFill,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREERADBENDBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pnCurve,
               L_UINT         uCurveSize,
               L_UINT         uScale,
               POINT          CenterPt,
               COLORREF       crFill,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEPLANEBENDBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          *pnCurve,
               L_UINT         uCurveSize,
               L_UINT         uScale,
               COLORREF       crFill,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_FFTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pFTARRAY       pFTArray,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FTDISPLAYBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pFTARRAY       pFTArray,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_DFTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pFTARRAY       pFTArray,
               RECT           *prcRange,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FRQFILTERBITMAP)(
               pFTARRAY pFTArray,
               LPRECT   prcRange,
               L_UINT   uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_GRAYSCALETODUOTONE)(
               pBITMAPHANDLE  pBitmap,
               LPRGBQUAD      pNewColor,
               COLORREF       crColor,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GRAYSCALETOMULTITONE)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uToneType,
               L_UINT         uDistType,
               LPCOLORREF     pColor,
               LPRGBQUAD      *pGradient,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_COLORLEVELBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pLVLCLR        pLvlClr,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_AUTOCOLORLEVELBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pLVLCLR        pLvlClr,
               L_UINT         uBlackClip,
               L_UINT         uWhiteClip,
               L_UINT         uFlags);



typedef L_INT ( pWRPEXT_CALLBACK pL_GETRGNCONTOURPOINTS)(
               pBITMAPHANDLE  pBitmap,
               pRGNXFORM      pXForm,
               POINT          **ppPoints,
               L_UINT         *puSize,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_SEGMENTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uThreshold,
               L_UINT         uFlag);




typedef L_INT ( pWRPEXT_CALLBACK pL_ADDSHADOWBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uAngle,
               L_UINT         uThreshold,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SUBTRACTBACKGROUNDBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uRollingBall,
               L_UINT         uShrinkSize,
               L_UINT         uBrightnessFactor,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_APPLYMODALITYLUT)(
               pBITMAPHANDLE        pBitmap,
               L_UINT16             *pLUT,
               pDICOMLUTDESCRIPTOR  pLUTDescriptor,
               L_UINT               uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_APPLYLINEARMODALITYLUT)(
               pBITMAPHANDLE  pBitmap,
               L_DOUBLE       fIntercept,
               L_DOUBLE       fSlope,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_APPLYVOILUT)(
               pBITMAPHANDLE        pBitmap,
               L_UINT16             *pLUT,
               pDICOMLUTDESCRIPTOR  pLUTDescriptor,
               L_UINT               uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_APPLYLINEARVOILUT)(
               pBITMAPHANDLE  pBitmap,
               L_DOUBLE       fCenter,
               L_DOUBLE       fWidth,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETLINEARVOILUT)(
               pBITMAPHANDLE  pBitmap,
               L_DOUBLE       *pCenter,
               L_DOUBLE       *pWidth,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_COUNTLUTCOLORS)(
               RGBQUAD  *pLUT,
               L_UINT   ulLLUTLen,
               L_UINT   *pNumberOfEntries,
               L_INT    *pFirstIndex,
               L_UINT   uFlags);

#ifdef LEADTOOLS_V16_OR_LATER
typedef L_INT ( pWRPEXT_CALLBACK pL_COUNTLUTCOLORSEXT)(
               L_RGBQUAD16 *pLUT,
               L_UINT   ulLLUTLen,
               L_UINT   *pNumberOfEntries,
               L_INT    *pFirstIndex,
               L_UINT   uFlags);
#endif // #ifdef LEADTOOLS_V16_OR_LATER

typedef L_INT ( pWRPEXT_CALLBACK pL_ADAPTIVECONTRASTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uDim,
               L_UINT         uAmount,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_AGINGBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uHScratchCount,
               L_UINT         uVScratchCount,
               L_UINT         uMaxScratchLen,
               L_UINT         uDustDensity,
               L_UINT         uPitsDensity,
               L_UINT         uMaxPitSize,
               COLORREF       crScratch,
               COLORREF       crDust,
               COLORREF       crPits,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_APPLYMATHLOGICBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nFactor,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_EDGEDETECTEFFECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uLevel,
               L_UINT         uThreshold,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_DICEEFFECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uXBlock,
               L_UINT         uYBlock,
               L_UINT         uRandomize,
               L_UINT         uFlags,
               COLORREF       crColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_PUZZLEEFFECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uXBlock,
               L_UINT         uYBlock,
               L_UINT         uRandomize,
               L_UINT         uFlags,
               COLORREF       crColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_RINGEFFECTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nXOrigin,
               L_INT          nYOrigin,
               L_UINT         uRadius,
               L_UINT         uRingCount,
               L_UINT         uRandomize,
               COLORREF       crColor,
               L_INT          nAngle,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_MULTISCALEENHANCEMENTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uContrast,
               L_UINT         uEdgeLevels,
               L_UINT         uEdgeCoeff,
               L_UINT         uLatitudeLevels,
               L_UINT         uLatitudeCoeff,
               L_UINT         uFlags);



typedef L_INT ( pWRPEXT_CALLBACK pL_DIGITALSUBTRACTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pMaskBitmap,
               L_UINT         uFlags);



typedef L_INT ( pWRPEXT_CALLBACK pL_APPLYTRANSFORMATIONPARAMETERS)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nXTranslation,
               L_INT          nYTranslation,
               L_INT          nAngle,
               L_UINT         uXScale,
               L_UINT         uYScale,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTBITMAPUNSIGNEDTOSIGNED)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_OFFSETBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nHorizontalShift,
               L_INT          nVerticalShift,
               COLORREF       crBackColor,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_BRICKSTEXTUREBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uBricksWidth,
               L_UINT         uBricksHeight,
               L_UINT         uOffsetX,
               L_UINT         uOffsetY,
               L_UINT         uEdgeWidth,
               L_UINT         uMortarWidth,
               L_UINT         uShadeAngle,
               L_UINT         uRowDifference,
               L_UINT         uMortarRoughness,
               L_UINT         uMortarRoughnessEevenness,
               L_UINT         uBricksRoughness,
               L_UINT         uBricksRoughnessEevenness,
               COLORREF       crMortarColor,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_CANVASBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pThumbBitmap,
               L_UINT         uTransparency,
               L_UINT         uEmboss,
               L_INT          nXOffset,
               L_INT          nYOffset,
               L_UINT         uTilesOffset,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_CLOUDSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uSeed,
               L_UINT         uFrequency,
               L_UINT         uDensity,
               L_UINT         uOpacity,
               COLORREF       cBackColor,
               COLORREF       crCloudsColor,
               L_UINT         uFlag);


typedef L_INT ( pWRPEXT_CALLBACK pL_ROMANMOSAICBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uTileWidth,
               L_UINT         uTileHeight,
               L_UINT         uBorder,
               L_UINT         uShadowAngle,
               L_UINT         uShadowThresh,
               COLORREF       crColor,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GAMMACORRECTBITMAPEXT)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uGamma,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_MASKCONVOLUTIONBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_INT          nAngle,
               L_UINT         uDepth,
               L_UINT         uHeight,
               L_UINT         uFlag);


typedef L_INT ( pWRPEXT_CALLBACK pL_PERLINBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uSeed,
               L_UINT         uFrequency,
               L_UINT         uDensity,
               L_UINT         uOpacity,
               COLORREF       cBackColor,
               COLORREF       Perlin,
               L_INT          nxCircle,
               L_INT          nyCircle,
               L_INT          nFreqLayout,
               L_INT          nDenLayout,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_DISPLACEMAPBITMAP)(
               pBITMAPHANDLE  pBitmap,
               pBITMAPHANDLE  pDisplacementMap,
               L_UINT         uHorzFact,
               L_UINT         uVertFact,
               COLORREF       crFill,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ZIGZAGBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uAmplitude,
               L_UINT         uAttenuation,
               L_UINT         uFrequency,
               L_INT          nPhase,
               POINT          CenterPt,
               COLORREF       crFill,
               L_UINT         uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_COLOREDBALLSBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uNumBalls,
               L_UINT         uSize,
               L_UINT         uSizeVariation,
               L_INT          nHighLightAng,
               COLORREF       crHighLight,
               COLORREF       crBkgColor,
               COLORREF       crShadingColor,
               COLORREF       *pBallColors,
               L_UINT         uNumOfBallColors,
               L_UINT         uAvrBallClrOpacity,
               L_UINT         uBallClrOpacityVariation,
               L_UINT         uRipple,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_PERSPECTIVEBITMAP)(
               pBITMAPHANDLE  pBitmap,
               POINT          *pPoints,
               COLORREF       crBkgColor,
               L_UINT         uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_POINTILLISTBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uSize,
               COLORREF       crColor,
               L_UINT         uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_HALFTONEPATTERNBITMAP)(
               pBITMAPHANDLE  pBitmap,
               L_UINT         uContrast,
               L_UINT         uRipple,
               L_UINT         uAngleContrast,
               L_UINT         uAngleRipple,
               L_INT          nAngleOffset,
               COLORREF       crForGround,
               COLORREF       crBackGround,
               L_UINT         uFlag);


typedef L_INT ( pWRPEXT_CALLBACK pL_SIZEBITMAPINTERPOLATE)(pBITMAPHANDLE pBitmap, 
                                                        L_INT nWidth, 
                                                        L_INT nHeight, 
                                                        L_INT uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLOREDPENCILBITMAPEXT)(
   pBITMAPHANDLE pBitmap,
   L_UINT uSize,
   L_UINT uStrength,
   L_UINT uThreshold,
   L_UINT uPencilRoughness,
   L_UINT uStrokeLength,
   L_UINT uPaperRoughness,
   L_INT  nAngle,
   L_UINT uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_INTELLIGENTUPSCALEBITMAP)(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pMaskBitmap,
   COLORREF       removeObjectColor,
   COLORREF       preserveObjectColor,
   L_INT          newWidth,
   L_INT          widthInsertionFactor,
   L_INT          newHeight,
   L_INT          heightInsertionFactor,
   L_INT          InsertionOrder,
   L_UINT32       uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_INTELLIGENTDOWNSCALEBITMAP)(
   pBITMAPHANDLE  pBitmap,
   pBITMAPHANDLE  pMaskBitmap,
   COLORREF       removeObjectColor,
   COLORREF       preserveObjectColor,
   L_INT          newWidth,
   L_INT          newHeight,
   L_INT          carvingOrder,
   L_UINT32       uFlags);

//-----------------------------------------------------------------------------
//--LTDIS.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------


typedef L_INT ( pWRPEXT_CALLBACK pL_APPENDPLAYBACK)(
               HPLAYBACK hPlayback,
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_CANCELPLAYBACKWAIT)(
               HPLAYBACK hPlayback);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEFROMDDB)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_HBITMAP hBitmap,
               L_HPALETTE hPalette);

typedef L_HBITMAP ( pWRPEXT_CALLBACK pL_CHANGETODDB)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLEARPLAYBACKUPDATERECT)(
               HPLAYBACK hPlayback);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_CLIPBOARDREADY)(
               L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTCOLORSPACE)(
               L_UCHAR* pBufferSrc,
               L_UCHAR* pBufferDst,
               L_INT nWidth,
               L_INT nFormatSrc,
               L_INT nFormatDst);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTFROMDDB)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_HBITMAP hBitmap,
               L_HPALETTE hPalette);

typedef L_HBITMAP ( pWRPEXT_CALLBACK pL_CONVERTTODDB)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_COPYFROMCLIPBOARD)(
               L_HWND hWnd,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYTOCLIPBOARD)(
               L_HWND hWnd,
               pBITMAPHANDLE pBitmap,
               L_UINT uFlags);

typedef L_HPALETTE ( pWRPEXT_CALLBACK pL_CREATEPAINTPALETTE)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEPLAYBACK)(
               pHPLAYBACK phPlayback,
               pBITMAPHANDLE pBitmap,
               HBITMAPLIST hList);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESTROYPLAYBACK)(
               HPLAYBACK hPlayback,
               pHBITMAPLIST phList);

typedef L_UINT ( pWRPEXT_CALLBACK pL_GETDISPLAYMODE)(
               L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPAINTCONTRAST)(
               pBITMAPHANDLE pBitmap);

typedef L_UINT ( pWRPEXT_CALLBACK pL_GETPAINTGAMMA)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPAINTINTENSITY)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPLAYBACKDELAY)(
               HPLAYBACK hPlayback,
               L_UINT *puDelay);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPLAYBACKINDEX)(
               HPLAYBACK hPlayback,
               L_INT *pnIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPLAYBACKSTATE)(
               HPLAYBACK hPlayback,
               L_UINT *puState);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPLAYBACKUPDATERECT)(
               HPLAYBACK hPlayback,
               L_RECT* prcUpdate,
               L_BOOL fClear);

typedef L_INT ( pWRPEXT_CALLBACK pL_PAINTDC)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_RECT* pSrc,
               L_RECT* pClipSrc,
               L_RECT* pDst,
               L_RECT* pClipDst,
               L_UINT32 uROP3);

typedef L_INT ( pWRPEXT_CALLBACK pL_PAINTDCBUFFER)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_RECT* pSrc,
               L_RECT* pClipSrc,
               L_RECT* pDst,
               L_RECT* pClipDst,
               L_UINT32 uROP3,
               L_UCHAR* pBuffer,
               L_INT nRow,
               L_INT nCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_PROCESSPLAYBACK)(
               HPLAYBACK hPlayback,
               L_UINT* puState);

typedef HSVREF ( pWRPEXT_CALLBACK pL_RGBTOHSV)(
               L_COLORREF crColor);

typedef L_COLORREF ( pWRPEXT_CALLBACK pL_HSVTORGB)(
               HSVREF hsvColor);

typedef L_UINT ( pWRPEXT_CALLBACK pL_SETDISPLAYMODE)(
               L_UINT uFlagPos,
               L_UINT uFlagSet);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPAINTCONTRAST)(
               pBITMAPHANDLE pBitmap,
               L_INT nValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPAINTGAMMA)(
               pBITMAPHANDLE pBitmap,
               L_UINT uValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPAINTINTENSITY)(
               pBITMAPHANDLE pBitmap,
               L_INT nValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPLAYBACKINDEX)(
               HPLAYBACK hPlayback,
               L_INT nIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_UNDERLAYBITMAP)(
               pBITMAPHANDLE pBitmapDst,
               pBITMAPHANDLE pUnderlay,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_VALIDATEPLAYBACKLINES)(
               HPLAYBACK hPlayback,
               L_INT nRow,
               L_INT nLines);

typedef L_INT ( pWRPEXT_CALLBACK pL_WINDOWLEVEL)(
               pBITMAPHANDLE pBitmap,
               L_INT nLowBit,
               L_INT nHighBit,
               L_RGBQUAD* pLUT,
               L_UINT ulLUTLength,
               L_UINT uFlags);

            // new WindowLevel dialog helper function
typedef L_INT ( pWRPEXT_CALLBACK pL_WINDOWLEVELFILLLUT)(
               L_RGBQUAD* pLUT,
               L_UINT ulLUTLen,
               L_COLORREF crStart,
               L_COLORREF crEnd,
               L_INT nLow,
               L_INT nHigh,
               L_UINT uLowBit,
               L_UINT uHighBit,
               L_INT nMinValue,
               L_INT nMaxValue,
               L_INT nFactor,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_WINDOWLEVELFILLLUT2)(
               L_RGBQUAD* pLUT,
               L_UINT ulLUTLen,
               L_COLORREF crStart,
               L_COLORREF crEnd,
               L_INT nLow,
               L_INT nHigh,
               L_UINT uLowBit,
               L_UINT uHighBit,
               L_INT nMinValue,
               L_INT nMaxValue,
               L_INT nFactor,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPCLIPSEGMENTS)(
               pBITMAPHANDLE pBitmap,
               L_INT nRow,
               L_UINT* pSegmentBuffer,
               L_UINT* puSegmentCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPCLIPSEGMENTSMAX)(
               pBITMAPHANDLE pBitmap,
               L_UINT* puMaxSegments);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTMAGGLASS)(
               L_HWND hWnd,
               pBITMAPHANDLE pBitmap,
               L_RECT* prcDst,
               MAGGLASSOPTIONS* pMagGlassOptions,
               MAGGLASSCALLBACK pfnCallback,
               L_VOID* pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_STOPMAGGLASS)(
               L_HWND hWnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_UPDATEMAGGLASSRECT)(
               L_HWND hWnd,
               L_RECT* prcDst);

typedef L_INT ( pWRPEXT_CALLBACK pL_UPDATEMAGGLASSPAINTFLAGS)(
               L_HWND hWnd,
               L_UINT uPaintFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_UPDATEMAGGLASS)(
               L_HWND hWnd,
               L_COLORREF* pColor,
               L_UCHAR* pMaskPlane,
               L_INT nMaskPlaneStart,
               L_INT nMaskPlaneEnd,
               L_BOOL bUpdateBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_UPDATEMAGGLASSBITMAP)(
               L_HWND hWnd,
               pBITMAPHANDLE pBitmap,
               L_BOOL bUpdateBitmap);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_WINDOWHASMAGGLASS)(
               L_HWND hWnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETMAGGLASSOWNERDRAWCALLBACK)(
               L_HWND hWnd,
               MAGGLASSOWNERDRAWCALLBACK pfnCallback,
               L_VOID* pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETMAGGLASSPAINTOPTIONS)(
               L_HWND hWnd,
               pMAGGLASSPAINTOPTIONS pMagGlassPaintOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SHOWMAGGLASS)(
               L_HWND hWnd,
               L_BOOL bShowMagGlass);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETMAGGLASSPOS)(
               L_HWND hWnd,
               L_INT xPos,
               L_INT yPos);

typedef L_INT ( pWRPEXT_CALLBACK pL_UPDATEMAGGLASSSHAPE)(
               L_HWND hWnd,
               L_UINT uMagGlassShape,
               L_HRGN hMagGlassRgn);

typedef L_HDC ( pWRPEXT_CALLBACK pL_PRINTBITMAP)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_INT nX,
               L_INT nY,
               L_INT nWidth,
               L_INT nHeight,
               L_BOOL fEndDoc);

typedef L_HDC ( pWRPEXT_CALLBACK pL_PRINTBITMAPFAST)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_INT nX,
               L_INT nY,
               L_INT nWidth,
               L_INT nHeight,
               L_BOOL fEndDoc);

typedef L_INT ( pWRPEXT_CALLBACK pL_SCREENCAPTUREBITMAP)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_RECT* pRect);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_BITMAPHASRGN)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEMASKFROMBITMAPRGN)(
               pBITMAPHANDLE pBitmap,
               pBITMAPHANDLE pMask,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_CURVETOBEZIER)(
               pCURVE pCurve,
               L_INT* pOutPointCount,
               L_POINT *OutPoint);

typedef L_INT ( pWRPEXT_CALLBACK pL_FRAMEBITMAPRGN)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_UINT uType);

typedef L_INT ( pWRPEXT_CALLBACK pL_COLORBITMAPRGN)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_COLORREF crRgnColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEBITMAPRGN)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPRGNAREA)(
               pBITMAPHANDLE pBitmap,
               L_SIZE_T* puArea);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPRGNBOUNDS)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_RECT* pRect);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPRGNHANDLE)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_HRGN* phRgn);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_ISPTINBITMAPRGN)(
               pBITMAPHANDLE pBitmap,
               L_INT nRow,
               L_INT nCol);

typedef L_INT ( pWRPEXT_CALLBACK pL_OFFSETBITMAPRGN)(
               pBITMAPHANDLE pBitmap,
               L_INT nRowOffset,
               L_INT nColOffset);

typedef L_INT ( pWRPEXT_CALLBACK pL_PAINTRGNDC)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_RECT* pSrc,
               L_RECT* pClipSrc,
               L_RECT* pDst,
               L_RECT* pClipDst,
               L_UINT32 uROP3);

typedef L_INT ( pWRPEXT_CALLBACK pL_PAINTRGNDCBUFFER)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_RECT* pSrc,
               L_RECT* pClipSrc,
               L_RECT* pDst,
               L_RECT* pClipDst,
               L_UINT32 uROP3,
               L_UCHAR* pBuffer,
               L_INT nRow,
               L_INT nCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNCOLOR)(
               pBITMAPHANDLE pBitmap,
               L_COLORREF crColor,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNCOLORHSVRANGE)(
               pBITMAPHANDLE pBitmap,
               HSVREF hsvLower,
               HSVREF hsvUpper,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNCOLORRGBRANGE)(
               pBITMAPHANDLE pBitmap,
               L_COLORREF crLower,
               L_COLORREF crUpper,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNMAGICWAND)(
               pBITMAPHANDLE pBitmap,
               L_INT x,
               L_INT y,
               L_COLORREF crLowerTolerance,
               L_COLORREF crUpperTolerance,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNELLIPSE)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_RECT* pRect,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNFROMMASK)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               pBITMAPHANDLE pMask,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNHANDLE)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_HRGN hRgn,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNPOLYGON)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_POINT* pPoints,
               L_UINT uPoints,
               L_UINT uFillMode,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNRECT)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_RECT* pRect,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNROUNDRECT)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_RECT* pRect,
               L_INT nWidthEllipse,
               L_INT nHeightEllipse,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNCURVE)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               pCURVE pCurve,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEPANWINDOW)(
               L_HWND hWndParent,
               pBITMAPHANDLE pBitmap,
               L_UINT ulDisplayFlags,
               L_INT nLeft,
               L_INT nTop,
               L_INT nWidth,
               L_INT nHeight,
               L_TCHAR* pszClassName,
               L_HICON hIcon,
               L_HCURSOR hCursor,
               L_BOOL bSysMenu,
               PANWNDCALLBACK pfnPanCallback,
               L_VOID* pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_UPDATEPANWINDOW)(
               L_HWND hPanWindow,
               pBITMAPHANDLE pBitmap,
               L_UINT ulDisplayFlags,
               L_COLORREF crPen,
               L_TCHAR* pszTitle,
               L_RECT* prcDst);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESTROYPANWINDOW)(
               L_HWND hPanWindow);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPRGNDATA)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_INT nDataSize,
               L_VOID* pData);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNDATA)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_INT nDataSize,
               L_VOID* pData,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNCLIP)(
               pBITMAPHANDLE pBitmap,
               L_BOOL bClip);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPRGNCLIP)(
               pBITMAPHANDLE pBitmap,
               L_BOOL* pbClip);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETBITMAPRGNBOUNDSCLIP)(
               pBITMAPHANDLE pBitmap,
               pRGNXFORM pXForm,
               L_RECT* pRect);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTFROMWMF)(
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_HMETAFILE hWmf,
               L_UINT uWidth,
               L_UINT uHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEFROMWMF)(
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_HMETAFILE hWmf,
               L_UINT uWidth,
               L_UINT uHeight);

typedef L_HMETAFILE ( pWRPEXT_CALLBACK pL_CONVERTTOWMF)(
               pBITMAPHANDLE pBitmap);

typedef L_HMETAFILE ( pWRPEXT_CALLBACK pL_CHANGETOWMF)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTFROMEMF)(
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_HENHMETAFILE hWmf,
               L_UINT uWidth,
               L_UINT uHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_CHANGEFROMEMF)(
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_HENHMETAFILE hWmf,
               L_UINT uWidth,
               L_UINT uHeight);

typedef L_HENHMETAFILE ( pWRPEXT_CALLBACK pL_CONVERTTOEMF)(
               pBITMAPHANDLE pBitmap);

typedef L_HENHMETAFILE ( pWRPEXT_CALLBACK pL_CHANGETOEMF)(
               pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_PAINTDCOVERLAY)(
               L_HDC hDC,
               pBITMAPHANDLE pBitmap,
               L_INT nIndex,
               L_RECT* pSrc,
               L_RECT* pClipSrc,
               L_RECT* pDst,
               L_RECT* pClipDst,
               L_UINT32 uROP3);

typedef L_INT ( pWRPEXT_CALLBACK pL_DOUBLEBUFFERENABLE)(
               L_HANDLE hDoubleBufferHandle,
               L_BOOL bEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_DOUBLEBUFFERCREATEHANDLE)(
               L_HANDLE* phDoubleBufferHandle);

typedef L_INT ( pWRPEXT_CALLBACK pL_DOUBLEBUFFERDESTROYHANDLE)(
               L_HANDLE hDoubleBufferHandle);

typedef L_HDC ( pWRPEXT_CALLBACK pL_DOUBLEBUFFERBEGIN)(
               L_HANDLE hDoubleBufferHandle,
               L_HDC hDC,
               L_INT cx,
               L_INT cy);

typedef L_INT ( pWRPEXT_CALLBACK pL_DOUBLEBUFFEREND)(
               L_HANDLE hDoubleBufferHandle,
               L_HDC hDC);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETBITMAPRGNBORDER)(
               pBITMAPHANDLE pBitmap,
               L_INT x,
               L_INT y,
               L_COLORREF crBorderColor,
               L_COLORREF crLowerTolerance,
               L_COLORREF crUpperTolerance,
               L_UINT uCombineMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_PAINTDCCMYKARRAY)(
               L_HDC hDC,
               pBITMAPHANDLE* ppBitmapArray,
               L_UINT uBitmapArrayCount,
               L_RECT* pSrc,
               L_RECT* pClipSrc,
               L_RECT* pDst,
               L_RECT* pClipDst,
               L_UINT32 uROP3,
               L_HANDLE hClrHandle);

typedef L_INT ( pWRPEXT_CALLBACK pL_PRINTBITMAPGDIPLUS )(L_HDC hDC, pBITMAPHANDLE pBitmap, L_INT nX, L_INT nY, L_INT nWidth, L_INT nHeight, L_UINT32 uFlags);
//-----------------------------------------------------------------------------
//--LTFIL.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

typedef L_INT ( pWRPEXT_CALLBACK pL_COMPRESSBUFFER)(
               L_UCHAR* pBuffer);

typedef L_INT ( pWRPEXT_CALLBACK pL_DELETEPAGE)(
               L_TCHAR* pszFile,
               L_INT nPage,
               L_UINT uFlags,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ENDCOMPRESSBUFFER)(
               L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_READLOADRESOLUTIONS)(
               L_TCHAR* pszFile,
               pDIMENSION pDimensions,
               L_INT* pDimensionCount,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_FEEDLOAD)(
               L_HGLOBAL hLoad,
               L_UCHAR* pBuffer,
               L_SIZE_T dwBufferSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_FILECONVERT)(
               L_TCHAR* pszFileSrc,
               L_TCHAR* pszFileDst,
               L_INT nType,
               L_INT nWidth,
               L_INT nHeight,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               pLOADFILEOPTION pLoadOptions,
               pSAVEFILEOPTION pSaveOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_FILEINFO)(
               L_TCHAR* pszFile,
               pFILEINFO pFileInfo,
               L_UINT uStructSize,
               L_UINT uFlags,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_FILEINFOMEMORY)(
               L_UCHAR* pBuffer,
               pFILEINFO pFileInfo,
               L_UINT uStructSize,
               L_SSIZE_T nBufferSize,
               L_UINT uFlags,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETCOMMENT)(
               L_UINT uType,
               L_UCHAR* pComment,
               L_UINT uLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETLOADRESOLUTION)(
               L_INT nFormat,
               L_UINT* pWidth,
               L_UINT* pHeight,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETFILECOMMENTSIZE)(
               L_TCHAR* pszFile,
               L_UINT uType,
               L_UINT* uLength,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETTAG)(
               L_UINT16 uTag,
               L_UINT16* pType,
               L_UINT* pCount,
               L_VOID* pData);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADBITMAP)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADBITMAPLIST)(
               L_TCHAR*lpszFile,
               pHBITMAPLIST phList,
               L_INT nBitsTo,
               L_INT nColorOrder,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADBITMAPMEMORY)(
               L_UCHAR* pBuffer,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_SSIZE_T nBufferSize,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADFILE)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_UINT uFlags,
               FILEREADCALLBACK pfnCallback,
               L_VOID* pUserData,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADFILETILE)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nCol,
               L_INT nRow,
               L_UINT uWidth,
               L_UINT uHeight,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_UINT uFlags,
               FILEREADCALLBACK pfnCallback,
               L_VOID* pUserData,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADMEMORYTILE)(
               L_UCHAR*   pBuffer,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nCol,
               L_INT nRow,
               L_UINT uWidth,
               L_UINT uHeight,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_UINT uFlags,
               FILEREADCALLBACK pfnCallback,
               L_VOID* pUserData,
               L_SSIZE_T nBufferSize,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADFILEOFFSET)(
               L_HFILE fd,
               L_SSIZE_T nOffsetBegin,
               L_SSIZE_T nBytesToLoad,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_UINT uFlags,
               FILEREADCALLBACK pfnCallback,
               L_VOID* pUserData,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADMEMORY)(
               L_UCHAR* pBuffer,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_UINT uFlags,
               FILEREADCALLBACK pfnCallback,
               L_VOID* pUserData,
               L_SSIZE_T nBufferSize,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILECOMMENT)(
               L_TCHAR* pszFile,
               L_UINT uType,
               L_UCHAR* pComment,
               L_UINT uLength,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILECOMMENTEXT)(
               L_TCHAR* pszFile,
               L_UINT uType,
               pFILECOMMENTS pComments,
               L_UCHAR* pBuffer,
               L_UINT* uLength,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILECOMMENTMEMORY)(
               L_UCHAR* pBuffer,
               L_UINT uType,
               L_UCHAR* pComment,
               L_UINT uLength,
               L_SSIZE_T nBufferSize,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILETAG)(
               L_TCHAR* pFile,
               L_UINT16 uTag,
               L_UINT16* pType,
               L_UINT* pCount,
               L_VOID* pData,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILETAGMEMORY)(
               L_UCHAR* pBuffer,
               L_UINT16 uTag,
               L_UINT16* pType,
               L_UINT* pCount,
               L_VOID* pData,
               L_SSIZE_T nBufferSize,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILESTAMP)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEBITMAP)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEBITMAPBUFFER)(
               L_UCHAR* pBuffer,
               L_SIZE_T uInitialBufferSize,
               L_SIZE_T* puFinalFileSize,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               SAVEBUFFERCALLBACK pfnSaveBufferCB,
               L_VOID* pUserData,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEBITMAPLIST)(
               L_TCHAR* lpszFile,
               HBITMAPLIST hList,
               L_INT nFormat,
               L_INT nBits,
               L_INT nQFactor,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEBITMAPMEMORY)(
               L_HGLOBAL* phHandle,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               L_SIZE_T* puSize,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEFILE)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               L_UINT uFlags,
               FILESAVECALLBACK pfnCallback,
               L_VOID* pUserData,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEFILEBUFFER)(
               L_UCHAR* pBuffer,
               L_SIZE_T uInitialBufferSize,
               L_SIZE_T* puFinalFileSize,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               L_UINT uFlags,
               FILESAVECALLBACK pfnFileSaveCB,
               SAVEBUFFERCALLBACK pfnSaveBufferCB,
               L_VOID* pUserData,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEFILEMEMORY)(
               L_HANDLE* hHandle,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               L_UINT uFlags,
               FILESAVECALLBACK pFunction,
               L_VOID* lpUserData,
               L_SIZE_T* uSize,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEFILETILE)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_INT nCol,
               L_INT nRow,
               FILESAVECALLBACK pfnCallback,
               L_VOID* pUserData,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEFILEOFFSET)(
               L_HFILE fd,
               L_SSIZE_T nOffsetBegin,
               L_SSIZE_T* nSizeWritten,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               L_UINT uFlags,
               FILESAVECALLBACK pfnCallback,
               L_VOID* pUserData,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETCOMMENT)(
               L_UINT uType,
               L_UCHAR* pComment,
               L_UINT uLength);

typedef LOADINFOCALLBACK ( pWRPEXT_CALLBACK pL_SETLOADINFOCALLBACK)(
               LOADINFOCALLBACK pfnCallback,
               L_VOID* pUserData);

typedef L_VOID* ( pWRPEXT_CALLBACK pL_GETLOADINFOCALLBACKDATA)(
               L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETLOADRESOLUTION)(
               L_INT nFormat,
               L_UINT nWidth,
               L_UINT nHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETTAG)(
               L_UINT16 uTag,
               L_UINT16 uType,
               L_SIZE_T uCount,
               L_VOID* pData);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTCOMPRESSBUFFER)(
               pBITMAPHANDLE pBitmap,
               COMPBUFFCALLBACK pfnCallback,
               L_SIZE_T uInputBytes,
               L_SIZE_T uOutputBytes,
               L_UCHAR* pOutputBuffer,
               L_INT nOutputType,
               L_INT nQFactor,
               L_VOID* pUserData,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTFEEDLOAD)(
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_UINT uFlags,
               FILEREADCALLBACK pfnCallback,
               L_VOID* pUserData,
               L_HGLOBAL* phLoad,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_STOPFEEDLOAD)(
               L_HGLOBAL hLoad);

typedef L_INT ( pWRPEXT_CALLBACK pL_WRITEFILECOMMENTEXT)(
               L_TCHAR*pszFile,
               L_UINT uType,
               pFILECOMMENTS pComments,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_WRITEFILESTAMP)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETSAVERESOLUTION)(
               L_UINT uCount,
               pDIMENSION pResolutions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETSAVERESOLUTION)(
               L_UINT* puCount,
               pDIMENSION pResolutions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETDEFAULTLOADFILEOPTION)(
               pLOADFILEOPTION pLoadOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETDEFAULTSAVEFILEOPTION)(
               pSAVEFILEOPTION pSaveOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_WRITEFILETAG)(
               L_TCHAR* pszFile,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_WRITEFILECOMMENT)(
               L_TCHAR* pszFile,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATETHUMBNAILFROMFILE)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               pTHUMBOPTIONS pThumbOptions,
               FILEREADCALLBACK pfnCallback,
               L_VOID* pUserData,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETJ2KOPTIONS)(
               pFILEJ2KOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETDEFAULTJ2KOPTIONS)(
               pFILEJ2KOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETJ2KOPTIONS)(
               pFILEJ2KOPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_MARKERCALLBACKPROXY)(
               LEADMARKERCALLBACK pfnCallback,
               L_UINT uMarker,
               L_UINT uMarkerSize,
               L_UCHAR* pMarkerData,
               L_VOID* pLEADUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_TRANSFORMFILE)(
               L_TCHAR* pszFileSrc,
               L_TCHAR* pszFileDst,
               L_UINT uTransform,
               TRANSFORMFILECALLBACK pfnCallback,
               L_VOID* pUserData,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILEEXTENSIONS)(
               L_TCHAR* pszFile,
               pEXTENSIONLIST* ppExtensionList,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEEXTENSIONS)(
               pEXTENSIONLIST pExtensionList);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADEXTENSIONSTAMP)(
               pEXTENSIONLIST pExtensionList,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETEXTENSIONAUDIO)(
               pEXTENSIONLIST pExtensionList,
               L_INT nStream,
               L_UCHAR** ppBuffer,
               L_SIZE_T* puSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTDECOMPRESSBUFFER)(
               L_HGLOBAL *phDecompress,
               pSTARTDECOMPRESSDATA pStartDecompressData);

typedef L_INT ( pWRPEXT_CALLBACK pL_STOPDECOMPRESSBUFFER)(
               L_HGLOBAL hDecompress);

typedef L_INT ( pWRPEXT_CALLBACK pL_DECOMPRESSBUFFER)(
               L_HGLOBAL hDecompress,
               pDECOMPRESSDATA pDecompressData);

typedef L_INT ( pWRPEXT_CALLBACK pL_IGNOREFILTERS)(
               L_TCHAR* pszFilters);

typedef L_INT ( pWRPEXT_CALLBACK pL_PRELOADFILTERS)(
               L_INT nFixedFilters,
               L_INT nCachedFilters,
               L_TCHAR* pszFilters);

typedef L_SSIZE_T ( pWRPEXT_CALLBACK pL_GETIGNOREFILTERS)(
               L_TCHAR* pszFilters,
               L_SIZE_T uSize);

typedef L_SSIZE_T ( pWRPEXT_CALLBACK pL_GETPRELOADFILTERS)(
               L_TCHAR* pszFilters,
               L_SIZE_T uSize,
               L_INT* pnFixedFilters,
               L_INT* pnCachedFilters);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETJBIG2OPTIONS)(
               pFILEJBIG2OPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETJBIG2OPTIONS)(
               pFILEJBIG2OPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEMARKERS)(
               L_HANDLE* phMarkers);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADMARKERS)(
               L_TCHAR* pszFilename,
               L_VOID** phMarkers,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEMARKERS)(
               L_VOID* hMarkers);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETMARKERS)(
               L_VOID* hMarkers,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMARKERS)(
               L_VOID** phMarkers,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ENUMMARKERS)(
               L_VOID* hMarkers,
               L_UINT uFlags,
               ENUMMARKERSCALLBACK pfnCallback,
               L_VOID* pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_DELETEMARKER)(
               L_VOID* hMarkers,
               L_UINT uMarker,
               L_INT nCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_INSERTMARKER)(
               L_VOID* hMarkers,
               L_UINT uIndex,
               L_UINT uMarker,
               L_UINT uMarkerSize,
               L_UCHAR* pMarkerData);

typedef L_INT ( pWRPEXT_CALLBACK pL_COPYMARKERS)(
               L_VOID** phMarkersDst,
               L_HANDLE hMarkersSrc);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMARKERCOUNT)(
               L_VOID* hMarkers,
               L_UINT* puCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETMARKER)(
               L_VOID* hMarkers,
               L_UINT uIndex,
               L_UINT* puMarker,
               L_UINT* puMarkerSize,
               L_UCHAR* pMarkerData);

typedef L_INT ( pWRPEXT_CALLBACK pL_DELETEMARKERINDEX)(
               L_VOID* hMarkers,
               L_UINT uIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_WRITEFILEMETADATA)(
               L_TCHAR* pFile,
               L_UINT uFlags,
               pSAVEFILEOPTION pSaveFileOption);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTSAVEDATA)(
               L_VOID** ppStruct,
               L_TCHAR* pszFileName,
               L_INT nCompression,
               L_INT nPlanarConfig,
               L_INT nOrder,           // ORDER_BGR or ORDER_RGB
               L_UINT uWidth,
               L_UINT uHeight,
               L_INT nBitsPerPixel,
               L_RGBQUAD* pPalette,
               L_UINT uPaletteEntries,
               L_INT XResolution,
               L_INT YResolution,
               L_BOOL bSaveMulti,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEDATA)(
               L_VOID* pStruct,
               L_UCHAR* pDataBuffer,
               L_SIZE_T ulBytes);

typedef L_INT ( pWRPEXT_CALLBACK pL_STOPSAVEDATA)(
               L_VOID* pStruct);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETOVERLAYCALLBACK)(
               OVERLAYCALLBACK pfnCallback,
               L_VOID* pUserData,
               L_UINT uFlags
            );

typedef L_INT ( pWRPEXT_CALLBACK pL_GETOVERLAYCALLBACK)(
               OVERLAYCALLBACK *ppfnCallback,
               L_VOID** ppUserData,
               L_UINT* puFlags
            );

typedef L_INT ( pWRPEXT_CALLBACK pL_GETFILTERLISTINFO)(
               pFILTERINFO* ppFilterList,
               L_UINT* pFilterCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETFILTERINFO)(
               L_TCHAR* pFilterName,
               pFILTERINFO pFilterInfo,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_FREEFILTERINFO)(
               pFILTERINFO pFilterInfo,
               L_UINT uFilterCount,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETFILTERINFO)(
               pFILTERINFO pFilterInfo,
               L_UINT uFilterCount,
               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETTXTOPTIONS)(
               pFILETXTOPTIONS pTxtOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETTXTOPTIONS)(
               pFILETXTOPTIONS pTxtOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_COMPACTFILE)(
               L_TCHAR* pszSrcFile,
               L_TCHAR* pszDstFile,
               L_UINT uPages,
               pLOADFILEOPTION pLoadFileOption,
               pSAVEFILEOPTION pSaveFileOption);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADFILECMYKARRAY)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE* ppBitmapArray,
               L_UINT uArrayCount,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_UINT uFlags,
               FILEREADCALLBACK pfnCallback,
               L_VOID* pUserData,
               pLOADFILEOPTION pLoadFileOption,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEFILECMYKARRAY)(
               L_TCHAR* pszFile, 
               pBITMAPHANDLE* ppBitmapArray,
               L_UINT uBitmapArrayCount,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               L_UINT uFlags,
               FILESAVECALLBACK pfnCallback,
               L_VOID* pUserData,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ENUMFILETAGS)(
               L_TCHAR* pszFile,
               L_UINT uFlags,
               ENUMTAGSCALLBACK pfnCallback,
               L_VOID* pUserData,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_DELETETAG)(
               L_TCHAR* pszFile,
               L_INT nPage,
               L_UINT uTag,
               L_UINT uFlags,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETGEOKEY)(
               L_UINT16 uTag,
               L_UINT uType,
               L_UINT uCount,
               L_VOID* pData);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETGEOKEY)(
               L_UINT16 uTag,
               L_UINT* puType,
               L_UINT* puCount,
               L_VOID* pData);

typedef L_INT ( pWRPEXT_CALLBACK pL_WRITEFILEGEOKEY)(
               L_TCHAR* pszFile,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILEGEOKEY)(
               L_TCHAR* pszFile,
               L_UINT16 uTag,
               L_UINT* puType,
               L_UINT* pCount,
               L_VOID* pData,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ENUMFILEGEOKEYS)(
               L_TCHAR* pszFile,
               L_UINT uFlags,
               ENUMGEOKEYSCALLBACK pfnCallback,
               L_VOID* pUserData,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILECOMMENTOFFSET)(
               L_HFILE fd,
               L_SSIZE_T nOffsetBegin,
               L_SSIZE_T nBytesToLoad,
               L_UINT uType,
               L_UCHAR* pComment,
               L_UINT uLength,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETLOADSTATUS)(L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_DECODEABIC)(
               L_UCHAR *pInputData,
               L_SSIZE_T nLength,
               L_UCHAR **ppOutputData,
               L_INT nAlign,
               L_INT nWidth,
               L_INT nHeight,
               L_BOOL bBiLevel);

typedef L_INT ( pWRPEXT_CALLBACK pL_ENCODEABIC)(
               L_UCHAR* pInputData,
               L_INT nAlign,
               L_INT nWidth,
               L_INT nHeight,
               L_UCHAR** ppOutputData,
               L_SSIZE_T* pnLength,
               L_BOOL bBiLevel);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPNGTRNS)( L_UCHAR * pData, L_UINT uSize );
typedef L_UINT ( pWRPEXT_CALLBACK pL_GETPNGTRNS)( L_UCHAR * pData );

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADBITMAPRESIZE)(
               L_TCHAR* pszFile,             // name of the file to load 
               pBITMAPHANDLE pSmallBitmap,   // pointer to the target bitmap handle 
               L_UINT uStructSize,
               L_INT nDestWidth,             // new width of the image 
               L_INT nDestHeight,            // new height of the image 
               L_INT nDestBits,              // new bits per pixel for the image 
               L_UINT uFlags,                // SIZE_NORMAL, SIZE_RESAMPLE SIZE_BICUBIC   
               L_INT nOrder,                 // color order for 16-, 24-, 32-, 48, and 64-bit bitmaps 
               pLOADFILEOPTION pLoadOptions, // pointer to optional extended load options 
               pFILEINFO pFileInfo);         // pointer to a structure 

typedef L_INT ( pWRPEXT_CALLBACK pL_READFILETRANSFORMS)(
               L_TCHAR* pszFile,
               pFILETRANSFORMS pTransforms,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_WRITEFILETRANSFORMS)(
               L_TCHAR* pszFile,
               pFILETRANSFORMS pTransforms,
               L_INT nFlags,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPCDRESOLUTION)(
               L_TCHAR* pszFile,
               pPCDINFO pPCDInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETWMFRESOLUTION)(
               L_INT* lpXResolution,
               L_INT* lpYResolution);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPCDRESOLUTION)(
               L_INT nResolution);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETWMFRESOLUTION)(
               L_INT nXResolution,
               L_INT nYResolution);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVECUSTOMFILE)(
               L_TCHAR* pszFilename,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               L_UINT uFlags,
               pSAVEFILEOPTION pSaveOptions,
               pSAVECUSTOMFILEOPTION pSaveCustomFileOption,
               SAVECUSTOMFILECALLBACK pfnCallback,
               L_VOID* pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADCUSTOMFILE)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_UINT uFlags,
               FILEREADCALLBACK pfnFileReadCallback,
               L_VOID* pFileReadCallbackUserData,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo,
               pLOADCUSTOMFILEOPTION pLoadCustomFileOption,
               LOADCUSTOMFILECALLBACK pfnLoadCustomFileCallback,
               L_VOID* pCustomCallbackUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_2DSETVIEWPORT)(
               L_INT nWidth,
               L_INT nHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_2DGETVIEWPORT)(
               L_INT* pnWidth,
               L_INT* pnHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_2DSETVIEWMODE)(
               L_INT nViewMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_2DGETVIEWMODE)(
               L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECLOADFILE)(
               L_TCHAR* pszFile,
               pVECTORHANDLE pVector,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECLOADMEMORY)(
               L_UCHAR* pBuffer,
               pVECTORHANDLE pVector,
               L_SSIZE_T nBufferSize,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSTARTFEEDLOAD)(
               pVECTORHANDLE pVector,
               L_HANDLE*phLoad,
               pLOADFILEOPTION pLoadOptions,
               pFILEINFO pFileInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECFEEDLOAD)(
               L_HANDLE hLoad,
               L_UCHAR* pInBuffer,
               L_SIZE_T dwBufferSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSTOPFEEDLOAD)(
               L_HANDLE hLoad);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSAVEFILE)(
               L_TCHAR* pszFile,
               pVECTORHANDLE pVector,
               L_INT nFormat,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSAVEMEMORY)(
               L_HANDLE*hHandle,
               pVECTORHANDLE pVector,
               L_INT nFormat,
               L_SIZE_T* uSize,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPLTOPTIONS)(
               pFILEPLTOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPLTOPTIONS)(
               pFILEPLTOPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPDFINITDIR)(
               L_TCHAR* pszInitDir,
               L_SIZE_T uBufSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPDFINITDIR)(
               L_TCHAR* pszInitDir);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETRTFOPTIONS)(
               pFILERTFOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETRTFOPTIONS)(
               pFILERTFOPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPTKOPTIONS)(
               pFILEPTKOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPTKOPTIONS)(
               pFILEPTKOPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPDFOPTIONS)(
               pFILEPDFOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPDFOPTIONS)(
               pFILEPDFOPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETPDFSAVEOPTIONS)(
               pFILEPDFSAVEOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETPDFSAVEOPTIONS)(
               pFILEPDFSAVEOPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETDJVOPTIONS)(
               pFILEDJVOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETDJVOPTIONS)(
               pFILEDJVOPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADLAYER)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nBitsPerPixel,
               L_INT nOrder,
               L_INT nLayer,
               pLAYERINFO pLayerInfo,
               pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEBITMAPWITHLAYERS)(
               L_TCHAR* pszFile,
               pBITMAPHANDLE pBitmap,
               L_INT nFormat,
               L_INT nBitsPerPixel,
               L_INT nQFactor,
               HBITMAPLIST hLayers,
               LAYERINFO* pLayerInfo,
               L_INT nLayers,
               pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETAUTOCADFILESCOLORSCHEME)(
               L_UINT* dwFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETAUTOCADFILESCOLORSCHEME)(
               L_UINT dwFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECADDFONTMAPPER)(
               VECTORFONTMAPPERCALLBACK pMapper,
               L_VOID* pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECREMOVEFONTMAPPER)(
               VECTORFONTMAPPERCALLBACK pMapper);


typedef L_INT ( pWRPEXT_CALLBACK pL_GETXPSOPTIONS)(
               pFILEXPSOPTIONS pOptions,
               L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETXPSOPTIONS)(
               pFILEXPSOPTIONS pOptions);

//-----------------------------------------------------------------------------
//--LTEFX.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_EFXPAINTTRANSITION )(HDC hDC,
                                     L_UINT uTransition,
                                     COLORREF crBack,
                                     COLORREF crFore,
                                     L_UINT uSteps,
                                     RECT *pDest,
                                     L_UINT uEffect,
                                     L_UINT uGrain,
                                     L_UINT uDelay,
                                     L_INT nSpeed,
                                     L_INT nCycles,
                                     L_UINT uPass,
                                     L_UINT uMaxPass,
                                     L_BOOL fTransparency,
                                     COLORREF crTransparency,
                                     L_UINT uWandWidth,
                                     COLORREF crWand,
                                     L_UINT32 uROP);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXPAINTBITMAP )(HDC hDC,
                                     pBITMAPHANDLE pBitmap,
                                     RECT *pSrc,
                                     RECT *pSrcClip,
                                     RECT *pDest,
                                     RECT *pDestClip,
                                     L_UINT uEffect,
                                     L_UINT uGrain,
                                     L_UINT uDelay,
                                     L_INT nSpeed,
                                     L_INT nCycles,
                                     L_UINT uPass,
                                     L_UINT uMaxPass,
                                     L_BOOL fTransparency,
                                     COLORREF crTransparency,
                                     L_UINT uWandWidth,
                                     COLORREF crWand,
                                     L_UINT32 uROP);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXDRAWFRAME)(HDC hDC,
                                  RECT *pRect,
                                  L_UINT uFlags,
                                  L_UINT uFrameWidth,
                                  COLORREF crFrame,
                                  L_UINT uInnerWidth,
                                  COLORREF crInner1,
                                  COLORREF crInner2,
                                  L_UINT uOuterWidth,
                                  COLORREF crOuter1,
                                  COLORREF crOuter2);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXTILERECT)(HDC hdcDest, RECT *prcDest, HDC hdcSrc,
                                 RECT *prcSrc);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXGRADIENTFILLRECT)(HDC hDC, RECT *pRect,
                                         L_UINT uStyle, COLORREF crStart,
                                         COLORREF crEnd, L_UINT uSteps);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXPATTERNFILLRECT)(HDC hDC, RECT *pRect,
                                         L_UINT uStyle, COLORREF crBack,
                                         COLORREF crFore);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXDRAW3DTEXT)(HDC hDC,
                                 L_TCHAR * pszText,
                                 RECT *pRect,
                                 L_UINT uFlags,
                                 L_INT nXDepth, L_INT nYDepth,
                                 COLORREF crText,
                                 COLORREF crShadow,
                                 COLORREF crHilite,
                                 HFONT hFont,
                                 HDC hdcFore);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXEFFECTBLT)(HDC hdcDest,
                                  L_INT nXDest,
                                  L_INT nYDest,
                                  L_INT nWidth,
                                  L_INT nHeight,
                                  HDC hdcSrc,
                                  L_INT nXSrc,
                                  L_INT nYSrc,
                                  L_UINT uEffect,
                                  L_UINT uGrain,
                                  L_UINT uDelay,
                                  L_INT nSpeed,
                                  L_INT nCycles,
                                  L_UINT uPass,
                                  L_UINT uMaxPass,
                                  L_BOOL fTransparency,
                                  COLORREF crTransparency,
                                  L_UINT uWandWidth,
                                  COLORREF crWand,
                                  L_UINT32 uROP);


typedef L_INT ( pWRPEXT_CALLBACK pL_PAINTDCEFFECT )(HDC hDC,
                                     pBITMAPHANDLE pBitmap,
                                     LPRECT pSrc,
                                     LPRECT pClipSrc,
                                     LPRECT pDst,
                                     LPRECT pClipDst,
                                     L_UINT32 uROP3,
                                     L_UINT uEffect);

typedef L_INT ( pWRPEXT_CALLBACK pL_PAINTRGNDCEFFECT )(HDC hDC,
                                       pBITMAPHANDLE pBitmap,
                                       LPRECT pSrc,
                                       LPRECT pClipSrc,
                                       LPRECT pDst,
                                       LPRECT pClipDst,
                                       L_UINT32 uROP3,
                                       L_UINT uEffect);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXDRAWROTATED3DTEXT)(HDC hDC,
                                 L_TCHAR *pszText,
                                 RECT *pRect,
                                 L_INT nAngle,
                                 L_UINT uFlags,
                                 L_INT nXDepth, L_INT nYDepth,
                                 COLORREF crText,
                                 COLORREF crShadow,
                                 COLORREF crHilite,
                                 HFONT hFont,
                                 HDC hdcFore);

typedef L_INT ( pWRPEXT_CALLBACK pL_EFXDRAW3DSHAPE)(HDC hDC,
                                 L_UINT uShape,
                                 RECT *pRect,
                                 COLORREF crBack,
                                 HDC hdcBack,
                                 RECT *prcBack,
                                 L_UINT uBackStyle,
                                 COLORREF crFill,
                                 L_UINT uFillStyle,
                                 COLORREF crBorder,
                                 L_UINT uBorderStyle,
                                 L_UINT uBorderWidth,
                                 COLORREF crInnerHilite,
                                 COLORREF crInnerShadow,
                                 L_UINT uInnerStyle,
                                 L_UINT uInnerWidth,
                                 COLORREF crOuterHilite,
                                 COLORREF crOuterShadow,
                                 L_UINT uOuterStyle,
                                 L_UINT uOuterWidth,
                                 L_INT nShadowX,
                                 L_INT nShadowY,
                                 COLORREF crShadow,
                                 HRGN hRgn);


//-----------------------------------------------------------------------------
//--LTDLG.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT (pWRPEXT_CALLBACK pL_DLGINIT) (L_UINT32 uFlags);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGFREE) ();

typedef HFONT (pWRPEXT_CALLBACK pL_DLGSETFONT) (HFONT hFont);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGETSTRINGLEN) (L_UINT32 uString, L_UINT * puLen);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSETSTRING) (L_UINT32 uString, L_TCHAR * szString);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGETSTRING) (L_UINT32 uString, L_TCHAR * szString, L_SIZE_T sizeInWords);

//{{ Color dialogs C DLL's group - LTDlgClr14?.dll
typedef L_INT (pWRPEXT_CALLBACK pL_DLGBALANCECOLORS) (HWND hWndOwner, LPBALANCECOLORSDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGCOLOREDGRAY) (HWND hWndOwner, LPCOLOREDGRAYDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGRAYSCALE) (HWND hWndOwner, LPGRAYSCALEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGREMAPINTENSITY) (HWND hWndOwner, LPREMAPINTENSITYDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGREMAPHUE) (HWND hWndOwner, LPREMAPHUEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGCUSTOMIZEPALETTE) (HWND hWndOwner, LPCUSTOMIZEPALETTEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGLOCALHISTOEQUALIZE) (HWND hWndOwner, LPLOCALHISTOEQUALIZEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGINTENSITYDETECT) (HWND hWndOwner, LPINTENSITYDETECTDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSOLARIZE) (HWND hWndOwner, LPSOLARIZEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGPOSTERIZE) (HWND hWndOwner, LPPOSTERIZEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGBRIGHTNESS) (HWND hWndOwner, LPBRIGHTNESSDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGCONTRAST) (HWND hWndOwner, LPCONTRASTDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGHUE) (HWND hWndOwner, LPHUEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSATURATION) (HWND hWndOwner, LPSATURATIONDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGAMMAADJUSTMENT) (HWND hWndOwner, LPGAMMAADJUSTMENTDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGHALFTONE) (HWND hWndOwner, LPHALFTONEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGCOLORRES) (HWND hWndOwner, LPCOLORRESDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGHISTOCONTRAST) (HWND hWndOwner, LPHISTOCONTRASTDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGWINDOWLEVEL) (HWND hWndOwner, LPWINDOWLEVELDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGCOLOR) (HWND hWndOwner, LPCOLORDLGPARAMS pDlgParams);

//{{ Image Effects dialogs C DLL's group - LTDlgImgEfx14?.dll
typedef L_INT (pWRPEXT_CALLBACK pL_DLGMOTIONBLUR) (HWND hWndOwner, LPMOTIONBLURDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGRADIALBLUR) (HWND hWndOwner, LPRADIALBLURDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGZOOMBLUR) (HWND hWndOwner, LPZOOMBLURDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGAUSSIANBLUR) (HWND hWndOwner, LPGAUSSIANBLURDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGANTIALIAS) (HWND hWndOwner, LPANTIALIASDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGAVERAGE) (HWND hWndOwner, LPAVERAGEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGMEDIAN) (HWND hWndOwner, LPMEDIANDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGADDNOISE) (HWND hWndOwner, LPADDNOISEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGMAXFILTER) (HWND hWndOwner, LPMAXFILTERDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGMINFILTER) (HWND hWndOwner, LPMINFILTERDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSHARPEN) (HWND hWndOwner, LPSHARPENDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSHIFTDIFFERENCEFILTER) (HWND hWndOwner, LPSHIFTDIFFERENCEFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGEMBOSS) (HWND hWndOwner, LPEMBOSSDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGOILIFY) (HWND hWndOwner, LPOILIFYDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGMOSAIC) (HWND hWndOwner, LPMOSAICDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGEROSIONFILTER) (HWND hWndOwner, LPEROSIONFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGDILATIONFILTER) (HWND hWndOwner, LPDILATIONFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGCONTOURFILTER) (HWND hWndOwner, LPCONTOURFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGRADIENTFILTER) (HWND hWndOwner, LPGRADIENTFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGLAPLACIANFILTER) (HWND hWndOwner, LPLAPLACIANFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSOBELFILTER) (HWND hWndOwner, LPSOBELFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGPREWITTFILTER) (HWND hWndOwner, LPPREWITTFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGLINESEGMENTFILTER) (HWND hWndOwner, LPLINESEGMENTFILTERDLGPARAMS  pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGUNSHARPMASK) (HWND hWndOwner, LPUNSHARPMASKDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGMULTIPLY) (HWND hWndOwner, LPMULTIPLYDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGADDBITMAPS) (HWND hWndOwner, LPADDBITMAPSDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSTITCH) (HWND hWndOwner, LPSTITCHDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGFREEHANDWAVE) (HWND hWndOwner, LPFREEHANDWAVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGWIND) (HWND hWndOwner, LPWINDDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGPOLAR) (HWND hWndOwner, LPPOLARDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGZOOMWAVE) (HWND hWndOwner, LPZOOMWAVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGRADIALWAVE) (HWND hWndOwner, LPRADIALWAVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSWIRL) (HWND hWndOwner, LPSWIRLDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGWAVE) (HWND hWndOwner, LPWAVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGWAVESHEAR) (HWND hWndOwner, LPWAVESHEARDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGPUNCH) (HWND hWndOwner, LPPUNCHDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGRIPPLE) (HWND hWndOwner, LPRIPPLEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGBENDING) (HWND hWndOwner, LPBENDINGDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGCYLINDRICAL) (HWND hWndOwner, LPCYLINDRICALDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSPHERIZE) (HWND hWndOwner, LPSPHERIZEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGIMPRESSIONIST) (HWND hWndOwner, LPIMPRESSIONISTDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGPIXELATE) (HWND hWndOwner, LPPIXELATEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGEDGEDETECTOR) (HWND hWndOwner, LPEDGEDETECTORDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGUNDERLAY) (HWND hWndOwner, LPUNDERLAYDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGPICTURIZE) (HWND hWndOwner, LPPICTURIZEDLGPARAMS pDlgParams);

//{{ Image dialogs  C DLL's group - LTDlgImg14?.dll
typedef L_INT (pWRPEXT_CALLBACK pL_DLGROTATE) (HWND hWndOwner, LPROTATEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSHEAR) (HWND hWndOwner, LPSHEARDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGRESIZE) (HWND hWndOwner, LPRESIZEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGADDBORDER) (HWND hWndOwner, LPADDBORDERDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGADDFRAME) (HWND hWndOwner, LPADDFRAMEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGAUTOTRIM) (HWND hWndOwner, LPAUTOTRIMDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGCANVASRESIZE) (HWND hWndOwner, LPCANVASRESIZEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGHISTOGRAM) (HWND hWndOwner, LPHISTOGRAMDLGPARAMS pDlgParams);


//{{ Web dialogs C DLL's group - LTDlgWeb14?.dll
typedef L_INT (pWRPEXT_CALLBACK pL_DLGPNGWEBTUNER) (HWND hWndOwner, LPPNGWEBTUNERDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGIFWEBTUNER) (HWND hWndOwner, LPGIFWEBTUNERDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGJPEGWEBTUNER) (HWND hWndOwner, LPJPEGWEBTUNERDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGHTMLMAPPER) (HWND hWndOwner, LPHTMLMAPPERDLGPARAMS pDlgParams);

//{{ File dialogs C DLL's group - LTDlgFile14?.dll
typedef L_INT (pWRPEXT_CALLBACK pL_DLGGETDIRECTORY) (HWND hWndOwner, LPGETDIRECTORYDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGFILECONVERSION) (HWND hWndOwner, LPFILECONVERSIONDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGFILESASSOCIATION) (HWND hWndOwner, LPFILESASSOCIATIONDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGPRINTSTITCHEDIMAGES) (HWND hWndOwner, LPPRINTSTITCHEDIMAGESDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGPRINTPREVIEW) (HWND hWndOwner,LPPRINTPREVIEWDLGPARAMS pDlgParams);

#if defined(FOR_UNICODE)
typedef L_INT (pWRPEXT_CALLBACK pL_DLGSAVE) (HWND hWndOwner, LPOPENFILENAMEW pOpenFileName, LPSAVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGOPEN) (HWND hWndOwner,LPOPENFILENAMEW pOpenFileName,LPOPENDLGPARAMS pDlgParams );
#else
typedef L_INT (pWRPEXT_CALLBACK pL_DLGSAVE) (HWND hWndOwner, LPOPENFILENAMEA pOpenFileName, LPSAVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGOPEN) (HWND hWndOwner,LPOPENFILENAMEA pOpenFileName,LPOPENDLGPARAMS pDlgParams );
#endif //#if defined(FOR_UNICODE)

typedef L_INT (pWRPEXT_CALLBACK pL_DLGICCPROFILE) ( HWND hWndOwner, LPICCPROFILEDLGPARAMS  pDlgParams);

//{{ Effects dialogs C DLL's group - LTDlgEfx14?.dll
typedef L_INT (pWRPEXT_CALLBACK pL_DLGGETSHAPE) (HWND hWndOwner, LPSHAPEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGETEFFECT) (HWND hWndOwner, LPEFFECTDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGETTRANSITION) (HWND hWndOwner, LPTRANSITIONDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGETGRADIENT) (HWND hWndOwner, LPGRADIENTDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGGETTEXT) (HWND hWndOwner, LPTEXTDLGPARAMS pDlgParams);

//{{ Document Image dialogs C DLL's group - LTDlgImgDoc14?.dll

typedef L_INT (pWRPEXT_CALLBACK pL_DLGSMOOTH) (HWND hWndOwner, LPSMOOTHDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGLINEREMOVE) (HWND hWndOwner, LPLINEREMOVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGBORDERREMOVE) (HWND hWndOwner, LPBORDERREMOVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGINVERTEDTEXT) (HWND hWndOwner, LPINVERTEDTEXTDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGDOTREMOVE) (HWND hWndOwner, LPDOTREMOVEDLGPARAMS pDlgParams);

typedef L_INT (pWRPEXT_CALLBACK pL_DLGHOLEPUNCHREMOVE) (HWND hWndOwner, LPHOLEPUNCHREMOVEDLGPARAMS pDlgParams);

//-----------------------------------------------------------------------------
//--LTTWN.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAININITSESSION )(pHTWAINSESSION phSession, pAPPLICATIONDATA pAppData);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAININITSESSION2)(pHTWAINSESSION phSession, pAPPLICATIONDATA pAppData, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINENDSESSION )(pHTWAINSESSION phSession);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETPROPERTIES )(HTWAINSESSION hSession, pLTWAINPROPERTIES pltProperties, L_UINT uFlags, LTWAINSETPROPERTYCALLBACK pfnCallBack, L_VOID * pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETPROPERTIES )(HTWAINSESSION hSession, pLTWAINPROPERTIES pltProperties, L_UINT uStructSize, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINACQUIRELIST )(HTWAINSESSION hSession, HBITMAPLIST hBitmap, L_TCHAR * lpszTemplateFile, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINACQUIRE )(HTWAINSESSION hSession, pBITMAPHANDLE pBitmap, L_UINT uStructSize, LTWAINBITMAPCALLBACK pfnCallBack, L_UINT uFlags, L_TCHAR * lpszTemplateFile, L_VOID * pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSELECTSOURCE )(HTWAINSESSION hSession, pLTWAINSOURCE pltSource);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINQUERYPROPERTY )(HTWAINSESSION hSession, L_UINT uCapability, pLTWAINPROPERTYQUERY* ppltProperty, L_UINT uStructSize);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSTARTCAPSNEG )(HTWAINSESSION hSession);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINENDCAPSNEG )(HTWAINSESSION hSession);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETCAPABILITY )(HTWAINSESSION hSession, pTW_CAPABILITY pCapability, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETCAPABILITY )(HTWAINSESSION hSession, pTW_CAPABILITY pCapability, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINENUMCAPABILITIES )(HTWAINSESSION hSession, LTWAINCAPABILITYCALLBACK pfnCallBack, L_UINT uFlags, L_VOID * pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINCREATENUMERICCONTAINERONEVALUE )(TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uValue);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINCREATENUMERICCONTAINERRANGE )(TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uMinValue, L_UINT32 uMaxValue, L_UINT32 uStepSize, L_UINT32 uDefaultValue, L_UINT32 uCurrentValue);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINCREATENUMERICCONTAINERARRAY )(TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uNumOfItems, L_VOID * pData);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINCREATENUMERICCONTAINERENUM )(TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uNumOfItems, L_UINT32 uCurrentIndex, L_UINT32 uDefaultIndex, L_VOID * pData);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERVALUE )(TW_CAPABILITY * pCapability, L_INT nIndex, L_VOID ** ppValue);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINFREECONTAINER )(TW_CAPABILITY * pCapability);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINFREEPROPQUERYSTRUCTURE )(pLTWAINPROPERTYQUERY * ppltProperty);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINTEMPLATEDLG )(HTWAINSESSION hSession, L_TCHAR * lpszTemplateFile, LTWAINSAVECAPCALLBACK pfnCallBack, LTWAINSAVEERRORCALLBACK pfnErCallBack, L_VOID * pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINOPENTEMPLATEFILE )(HTWAINSESSION hSession, HTWAINTEMPLATEFILE * phFile, L_TCHAR * lpszTemplateFile, L_UINT uAccess);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINADDCAPABILITYTOFILE )(HTWAINSESSION hSession, HTWAINTEMPLATEFILE hFile, pTW_CAPABILITY pCapability);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETCAPABILITYFROMFILE )(HTWAINSESSION hSession, HTWAINTEMPLATEFILE hFile, pTW_CAPABILITY * ppCapability, L_UINT uIndex);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMOFCAPSINFILE )(HTWAINSESSION hSession, HTWAINTEMPLATEFILE hFile, L_UINT * puCapCount);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINCLOSETEMPLATEFILE )(HTWAINSESSION hSession, HTWAINTEMPLATEFILE hFile);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETEXTENDEDIMAGEINFO )(HTWAINSESSION hSession, TW_EXTIMAGEINFO * ptwExtImgInfo);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINFREEEXTENDEDIMAGEINFOSTRUCTURE )(TW_EXTIMAGEINFO ** pptwExtImgInfo);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINLOCKCONTAINER )(TW_CAPABILITY * pCapability, void ** ppContainer);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINUNLOCKCONTAINER )(TW_CAPABILITY * pCapability);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERITEMTYPE )(TW_CAPABILITY * pCapability, L_INT * pnItemType);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERINTVALUE )(TW_CAPABILITY * pCapability, L_INT nIndex, L_INT * pnValue);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERUINTVALUE )(TW_CAPABILITY * pCapability, L_INT nIndex, L_UINT * puValue);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERBOOLVALUE )(TW_CAPABILITY * pCapability, L_INT nIndex, L_BOOL * pbValue);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERFIX32VALUE )(TW_CAPABILITY * pCapability, L_INT nIndex, TW_FIX32 * ptwFix);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERFRAMEVALUE )(TW_CAPABILITY * pCapability, L_INT nIndex, TW_FRAME * ptwFrame);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERSTRINGVALUE )(TW_CAPABILITY * pCapability, L_INT nIndex, TW_STR1024 twString);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETNUMERICCONTAINERUNICODEVALUE )(TW_CAPABILITY * pCapability, L_INT nIndex, TW_UNI512 twUniCode);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINACQUIREMULTI)(HTWAINSESSION         hSession,
                                      L_TCHAR *             pszBaseFileName,
                                      L_UINT                uFlags,
                                      L_UINT                uTransferMode,
                                      L_INT                 nFormat,
                                      L_INT                 nBitsPerPixel,
                                      L_BOOL                bMultiPageFile,
                                      L_UINT32              uUserBufSize,
                                      L_BOOL                bUsePrefferedBuffer,
                                      LTWAINACQUIRECALLBACK pfnCallBack,
                                      L_VOID              * pUserData);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_ISTWAINAVAILABLE)(HWND hWnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINFINDFASTCONFIG)(HTWAINSESSION        hSession,
                                        L_TCHAR *            pszWorkingFolder,
                                        L_UINT               uFlags,
                                        L_INT                nBitsPerPixel,
                                        L_INT                nBufferIteration,
                                        pFASTCONFIG          pInFastConfigs,
                                        L_INT                nInFastConfigsCount,
                                        pFASTCONFIG        * ppTestConfigs,
                                        L_INT              * pnTestConfigsCount,
                                        pFASTCONFIG          pOutBestConfig,
                                        L_UINT               uStructSize,
                                        LTWAINFINDFASTCONFIG pfnCallBack,
                                        L_VOID             * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETSCANCONFIGS)(HTWAINSESSION   hSession,
                                        L_INT           nBitsPerPixel,
                                        L_UINT          uTransferMode,
                                        L_INT           nBufferIteration,
                                        pFASTCONFIG    *ppFastConfig,
                                        L_UINT          uStructSize,
                                        L_INT          *pnFastConfigCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINFREESCANCONFIG)(HTWAINSESSION   hSession,
                                        pFASTCONFIG    *ppFastConfig,
                                        L_INT           nFastConfigCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETSOURCES )(HTWAINSESSION            hSession,
                                     LTWAINSOURCEINFOCALLBACK pfnCallBack,
                                     L_UINT                   uStructSize,
                                     L_UINT                   uFlags,
                                     L_VOID                 * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINENABLESHOWUSERINTERFACEONLY)(HTWAINSESSION  hSession,
                                                     L_BOOL         bEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINCANCELACQUIRE)(HTWAINSESSION hSession);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINQUERYFILESYSTEM)(HTWAINSESSION hSession, FILESYSTEMMSG FileMsg, pTW_FILESYSTEM pTwFile);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETJPEGCOMPRESSION)(HTWAINSESSION hSession, pTW_JPEGCOMPRESSION pTwJpegComp, L_UINT uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETJPEGCOMPRESSION)(HTWAINSESSION hSession, pTW_JPEGCOMPRESSION pTwJpegComp, L_UINT uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETTRANSFEROPTIONS)(HTWAINSESSION hSession, pTRANSFEROPTIONS pTransferOpts);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETTRANSFEROPTIONS)(HTWAINSESSION hSession, pTRANSFEROPTIONS pTransferOpts, L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETSUPPORTEDTRANSFERMODE)(HTWAINSESSION hSession, L_UINT * pTransferModes);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETRESOLUTION)(HTWAINSESSION hSession, pTW_FIX32 pXRes, pTW_FIX32 pYRes);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETRESOLUTION)(HTWAINSESSION hSession, pTW_FIX32 pXRes, pTW_FIX32 pYRes);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETIMAGEFRAME)(HTWAINSESSION hSession, pTW_FRAME pFrame);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETIMAGEFRAME)(HTWAINSESSION hSession, pTW_FRAME pFrame);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETIMAGEUNIT)(HTWAINSESSION hSession, L_INT nUnit);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETIMAGEUNIT)(HTWAINSESSION hSession, L_INT * pnUnit);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETIMAGEBITSPERPIXEL)(HTWAINSESSION hSession, L_INT nBitsPerPixel);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETIMAGEBITSPERPIXEL)(HTWAINSESSION hSession, L_INT * pnBitsPerPixel);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETIMAGEEFFECTS)(HTWAINSESSION hSession, L_UINT32 ulFlags, pTW_FIX32 pBrightness, pTW_FIX32 pContrast, pTW_FIX32 pHighlight);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETIMAGEEFFECTS)(HTWAINSESSION hSession, L_UINT32 ulFlags, pTW_FIX32 pBrightness, pTW_FIX32 pContrast, pTW_FIX32 pHighlight);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETACQUIREPAGEOPTIONS)(HTWAINSESSION hSession, L_INT nPaperSize, L_INT nPaperDirection);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETACQUIREPAGEOPTIONS)(HTWAINSESSION hSession, L_INT * pnPaperSize, L_INT * pnPaperDirection);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETRGBRESPONSE)(HTWAINSESSION hSession, pTW_RGBRESPONSE pRgbResponse, L_INT nBitsPerPixel, L_UINT uFlag);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSHOWPROGRESS)(HTWAINSESSION hSession, L_BOOL bShow);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINENABLEDUPLEX)(HTWAINSESSION hSession, L_BOOL bEnableDuplex);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETDUPLEXOPTIONS)(HTWAINSESSION hSession, L_BOOL * pbEnableDuplex, L_INT * pnDuplexMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETMAXXFERCOUNT)(HTWAINSESSION hSession, L_INT nMaxXferCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETMAXXFERCOUNT)(HTWAINSESSION hSession, L_INT * pnMaxXferCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSTOPFEEDER)(HTWAINSESSION hSession);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETDEVICEEVENTCALLBACK)(HTWAINSESSION hSession, LTWAINDEVICEEVENTCALLBACK pfnCallBack, L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETDEVICEEVENTDATA)(HTWAINSESSION hSession, pTW_DEVICEEVENT pDeviceEvent);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETDEVICEEVENTCAPABILITY)(HTWAINSESSION hSession, pTW_CAPABILITY pDeviceCap);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETDEVICEEVENTCAPABILITY)(HTWAINSESSION hSession, pTW_CAPABILITY pDeviceCap);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINRESETDEVICEEVENTCAPABILITY)(HTWAINSESSION hSession, pTW_CAPABILITY pDeviceCap);

typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINFASTACQUIRE)(HTWAINSESSION         hSession,
                                      L_TCHAR *             pszBaseFileName,
                                      L_UINT                uFlags,
                                      L_UINT                uTransferMode,
                                      L_INT                 nFormat,
                                      L_INT                 nBitsPerPixel,
                                      L_BOOL                bMultiPageFile,
                                      L_UINT32              uUserBufSize,
                                      L_BOOL                bUsePrefferedBuffer,
                                      LTWAINACQUIRECALLBACK pfnCallBack,
                                      L_VOID              * pUserData);


#if defined(LEADTOOLS_V16_OR_LATER)
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINGETCUSTOMDSDATA )(HTWAINSESSION hSession, pTW_CUSTOMDSDATA pCustomData, L_TCHAR * pszFileName);
typedef L_INT ( pWRPEXT_CALLBACK pL_TWAINSETCUSTOMDSDATA )(HTWAINSESSION hSession, pTW_CUSTOMDSDATA pCustomData, L_TCHAR * pszFileName);
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

//-----------------------------------------------------------------------------
//--LTANN.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNBRINGTOFRONT )(HANNOBJECT hObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCLIPBOARDREADY)(L_BOOL *pfReady);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCOPY)(HANNOBJECT hSource,
                             pHANNOBJECT phDest);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCOPYFROMCLIPBOARD)(HWND hWnd,
                                          pHANNOBJECT phContainer);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCOPYTOCLIPBOARD)(HANNOBJECT hObject,
                                        L_UINT uFormat,
                                        L_BOOL fSelected,
                                        L_BOOL fEmpty,
                                        L_BOOL fCheckMenu);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCUTTOCLIPBOARD)(HANNOBJECT hObject,
                                       L_UINT uFormat,
                                       L_BOOL fSelected,
                                       L_BOOL fEmpty,
                                       L_BOOL fCheckMenu);


typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCREATE )(L_UINT uObjectType,
                                pHANNOBJECT phObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCREATECONTAINER )(HWND hWnd,
                                         pANNRECT pRect,
                                         L_BOOL fVisible,
                                         pHANNOBJECT phObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCREATEITEM )(HANNOBJECT hContainer,
                                    L_UINT uObjectType,
                                    L_BOOL fVisible,
                                    pHANNOBJECT phObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCREATETOOLBAR)(HWND hwndParent,
                                      LPPOINT pPoint,
                                      L_UINT uAlign,
                                      L_BOOL fVisible,
                                      HWND *phWnd,
                                      L_UINT uButtons,
                                      pANNBUTTON pButtons);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDEFINE )(HANNOBJECT hObject,
                                LPPOINT pPoint,
                                L_UINT uState);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDELETEPAGEOFFSET)(L_HFILE fd, L_SSIZE_T nOffset, L_INT32 nPage);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDELETEPAGE)(L_TCHAR *pFile, 
                                   L_INT32 nPage);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDELETEPAGEMEMORY )(HGLOBAL hMem, L_SIZE_T *puMemSize, L_INT32 nPage);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDESTROY )(HANNOBJECT hObject,
                                 L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDRAW )(HDC hDC,
                              LPRECT prcInvalid,
                              HANNOBJECT hObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNENUMERATE )(HANNOBJECT hObject,
                                   ANNENUMCALLBACK pfnCallback,
                                   L_VOID *pUserData,
                                   L_UINT uFlags,
                                   L_TCHAR *pUserList);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNFILEINFO )(L_TCHAR *pszFile, 
                                  pANNFILEINFO pAnnFileInfo,
                                  L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNFILEINFOOFFSET )(L_HFILE fd, 
                                        pANNFILEINFO pAnnFileInfo,
                                        L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNFILEINFOMEMORY )(L_UCHAR *pMem, 
                                        L_SIZE_T uMemSize, 
                                        pANNFILEINFO pAnnFileInfo,
                                        L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNFLIP)(HANNOBJECT hObject,
                             pANNPOINT pCenter,
                             L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETACTIVESTATE )(HANNOBJECT hObject,
                                        L_UINT *puState);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOCONTAINER )(HANNOBJECT hObject,
                                          pHANNOBJECT phContainer);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTODRAWENABLE )(HANNOBJECT hObject,
                                           L_BOOL *pfEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOMENUENABLE )(HANNOBJECT hObject,
                                           L_BOOL *pfEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOTEXT )(HANNOBJECT hObject,
                                     L_UINT uItem,
                                     L_TCHAR *pText,
                                     L_SIZE_T *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOTEXTLEN )(HANNOBJECT hObject,
                                        L_UINT uItem,
                                        L_SIZE_T *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETBACKCOLOR )(HANNOBJECT hObject,
                                      COLORREF *pcrBack);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETBITMAP )(HANNOBJECT hObject,
                                   pBITMAPHANDLE pBitmap,
                                   L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETBITMAPDPIX)(HANNOBJECT hObject,
                                      L_DOUBLE *pdDpiX);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETBITMAPDPIY)(HANNOBJECT hObject,
                                      L_DOUBLE *pdDpiY);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETBOUNDINGRECT )(HANNOBJECT hObject, 
                                         LPRECT pRect, 
                                         LPRECT pRectName);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETCONTAINER )(HANNOBJECT hObject,
                                       pHANNOBJECT phContainer);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETDISTANCE)(HANNOBJECT hObject,
                                    L_DOUBLE *pdDistance,
                                    L_DOUBLE *pdDistance2);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETDPIX )(HANNOBJECT hObject,
                                 L_DOUBLE *pdDpiX);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETDPIY )(HANNOBJECT hObject,
                                 L_DOUBLE *pdDpiY);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFILLPATTERN )(HANNOBJECT hObject,
                                        L_UINT *puFillPattern);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFONTBOLD )(HANNOBJECT hObject,
                                     L_BOOL *pfFontBold);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFONTITALIC )(HANNOBJECT hObject,
                                       L_BOOL *pfFontItalic);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFONTNAME )(HANNOBJECT hObject,
                                     L_TCHAR *pFontName,
                                     L_UINT *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFONTNAMELEN )(HANNOBJECT hObject,
                                        L_UINT *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFONTSIZE )(HANNOBJECT hObject,
                                     L_DOUBLE *pdFontSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFONTSTRIKETHROUGH )(HANNOBJECT hObject,
                                              L_BOOL *pfFontStrikeThrough);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFONTUNDERLINE )(HANNOBJECT hObject,
                                          L_BOOL *pfFontUnderline);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFORECOLOR )(HANNOBJECT hObject,
                                      COLORREF *pcrFore);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETGAUGELENGTH)(HANNOBJECT hObject,
                                       L_DOUBLE *pdLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTICMARKLENGTH)(HANNOBJECT hObject,
                                         L_DOUBLE *pdLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETHYPERLINK )(HANNOBJECT hObject,
                                      L_UINT *puType,
                                      L_UINT *puMsg,
                                      WPARAM *pwParam,
                                      L_TCHAR *pLink,
                                      L_SIZE_T *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETHYPERLINKLEN )(HANNOBJECT hObject,
                                         L_SIZE_T *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETHYPERLINKMENUENABLE)(HANNOBJECT hObject,
                                               L_BOOL *pfEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETLINESTYLE )(HANNOBJECT hObject,
                                      L_UINT *puLineStyle);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETLINEWIDTH )(HANNOBJECT hObject,
                                      L_DOUBLE *pdLineWidth);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETLOCKED )(HANNOBJECT hObject, 
                                   L_BOOL *pfLocked);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETOFFSETX )(HANNOBJECT hObject,
                                    L_DOUBLE *pdOffsetX);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETOFFSETY )(HANNOBJECT hObject,
                                    L_DOUBLE *pdOffsetY);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPOINTCOUNT )(HANNOBJECT hObject,
                                       L_UINT *puCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPOINTS )(HANNOBJECT hObject,
                                   pANNPOINT pPoints);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPOLYFILLMODE )(HANNOBJECT hObject,
                                         L_UINT *puPolyFillMode);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETRECT )(HANNOBJECT hObject, 
                                 pANNRECT pRect, 
                                 pANNRECT pRectName);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETROP2 )(HANNOBJECT hObject,
                                 L_UINT *puRop2);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSCALARX )(HANNOBJECT hObject,
                                    L_DOUBLE *pdScalarX);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSCALARY )(HANNOBJECT hObject,
                                    L_DOUBLE *pdScalarY);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSELECTCOUNT )(HANNOBJECT hObject,
                                        L_UINT *puCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSELECTED )(HANNOBJECT hObject,
                                     L_BOOL *pfSelected);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSELECTITEMS)(HANNOBJECT hObject, 
                                       pHANNOBJECT pItems);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSELECTRECT)(HANNOBJECT hObject,
                                      LPRECT pRect);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTAG )(HANNOBJECT hObject,
                                L_UINT32 *puTag);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXT )(HANNOBJECT hObject,
                                 L_TCHAR *pText,
                                 L_SIZE_T *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTLEN )(HANNOBJECT hObject,
                                    L_SIZE_T *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTALIGN)(HANNOBJECT hObject, L_UINT *puTextAlign);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTROTATE)(HANNOBJECT hObject, L_UINT *puTextRotate);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTPOINTERFIXED)(HANNOBJECT hObject, L_BOOL *pbPointerFixed);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXTEXPANDTOKENS)(HANNOBJECT hObject, L_BOOL bTextExpandTokens, L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTEXPANDTOKENS)(HANNOBJECT hObject, L_BOOL *pbTextExpandTokens);


typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTOOL )(HANNOBJECT hObject,
                                 L_UINT *puTool);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTOOLBARBUTTONVISIBLE)(HWND hwndToolBar,
                                                L_UINT uButton,
                                                L_BOOL *pfVisible);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTOOLBARCHECKED )(HWND hwndToolBar,
                                           L_UINT *puChecked);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTRANSPARENT )(HANNOBJECT hObject,
                                        L_BOOL *pbTransparent);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTYPE )(HANNOBJECT hObject,
                                 L_UINT *puObjectType);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTOPCONTAINER )(HANNOBJECT hObject,
                                         pHANNOBJECT phContainer);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETUNIT)(HANNOBJECT hObject,
                                L_UINT    *puUnit,
                                L_TCHAR   *pUnitAbbrev,
                                L_SIZE_T  *puUnivAbbrevLen,
                                L_UINT    *puRulerPrecision
                                );

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETUNITLEN)(HANNOBJECT hObject,
                                   L_SIZE_T *puLen);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETUSERMODE )(HANNOBJECT hObject,
                                     L_UINT *puMode);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETVISIBLE )(HANNOBJECT hObject,
                                    L_BOOL *pfVisible);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETWND )(HANNOBJECT hObject,
                                HWND *phWnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNINSERT )(HANNOBJECT hContainer,
                                HANNOBJECT hObject,
                                L_BOOL fStripContainer);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETITEM )(HANNOBJECT hContainer,
                                 pHANNOBJECT phItem);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNLOAD )(L_TCHAR *pFile,
                              pHANNOBJECT phObject,
                              pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNLOADOFFSET )(L_HFILE fd,
                                    L_SSIZE_T nOffset,
                                    L_SIZE_T nLength,
                                    pHANNOBJECT phObject,
                                    pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNLOADMEMORY )(L_UCHAR *pMem,
                                    L_SIZE_T uMemSize,
                                    pHANNOBJECT phObject,
                                    pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNLOCK )(HANNOBJECT hObject, 
                              L_TCHAR * pLockKey, 
                              L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNMOVE)(HANNOBJECT hObject,
                             L_DOUBLE dDx,
                             L_DOUBLE dDy,
                             L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNPRINT)(HDC hDC,
                              LPRECT prcBounds,
                              HANNOBJECT hObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNREALIZE)(pBITMAPHANDLE pBitmap,
                                LPRECT prcBounds,
                                HANNOBJECT hObject,
                                L_BOOL fRedactOnly);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNRESIZE)(HANNOBJECT hObject,
                               L_DOUBLE dFactorX,
                               L_DOUBLE dFactorY,
                               pANNPOINT pCenter,
                               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNREVERSE)(HANNOBJECT hObject,
                                pANNPOINT pCenter,
                                L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNREMOVE )(HANNOBJECT hObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNROTATE)(HANNOBJECT hObject,
                               L_DOUBLE dAngle,
                               pANNPOINT pCenter,
                               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSAVE )(L_TCHAR *pFile,
                              HANNOBJECT hObject,
                              L_UINT uFormat,
                              L_BOOL fSelected,
                              pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSAVEOFFSET )(L_HFILE fd,
                                    L_SSIZE_T nOffset,
                                    L_SIZE_T *puSizeWritten,
                                    HANNOBJECT hObject,
                                    L_UINT uFormat,
                                    L_BOOL fSelected,
                                    pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSAVEMEMORY )(HANNOBJECT hObject,
                                    L_UINT uFormat,
                                    L_BOOL fSelected,
                                    HGLOBAL *phMem,
                                    L_SIZE_T *puMemSize,
                                    pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSAVETAG)(HANNOBJECT hObject,
                                L_UINT uFormat,
                                L_BOOL fSelected);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSELECTPOINT)(HANNOBJECT hObject,
                                    LPPOINT pPoint);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSELECTRECT)(HANNOBJECT hObject,
                                   LPRECT pRect);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSENDTOBACK )(HANNOBJECT hObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETACTIVESTATE )(HANNOBJECT hObject,
                                        L_UINT uState);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOCONTAINER )(HANNOBJECT hObject,
                                          HANNOBJECT hContainer);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTODRAWENABLE )(HANNOBJECT hObject,
                                           L_BOOL fEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOMENUENABLE )(HANNOBJECT hObject,
                                           L_BOOL fEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOTEXT )(HANNOBJECT hObject,
                                     L_UINT uItem,
                                     L_TCHAR *pText);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETBACKCOLOR )(HANNOBJECT hObject,
                                      COLORREF crBack,
                                      L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETBITMAP )(HANNOBJECT hObject,
                                   pBITMAPHANDLE pBitmap,
                                   L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETBITMAPDPIX)(HANNOBJECT hObject,
                                      L_DOUBLE  dDpiX,
                                      L_UINT    uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETBITMAPDPIY)(HANNOBJECT hObject,
                                      L_DOUBLE  dDpiY,
                                      L_UINT    uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETDPIX)(HANNOBJECT hObject,
                                L_DOUBLE dDpiX,
                                L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETDPIY)(HANNOBJECT hObject,
                                L_DOUBLE dDpiY,
                                L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFILLPATTERN )(HANNOBJECT hObject,
                                        L_UINT uFillPattern,
                                        L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFONTBOLD )(HANNOBJECT hObject,
                                     L_BOOL fFontBold,
                                     L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFONTITALIC )(HANNOBJECT hObject,
                                       L_BOOL fFontItalic,
                                       L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFONTNAME )(HANNOBJECT hObject,
                                     L_TCHAR *pFontName,
                                     L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFONTSIZE )(HANNOBJECT hObject,
                                     L_DOUBLE dFontSize,
                                     L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFONTSTRIKETHROUGH )(HANNOBJECT hObject,
                                              L_BOOL fFontStrikeThrough,
                                              L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFONTUNDERLINE )(HANNOBJECT hObject,
                                          L_BOOL fFontUnderline,
                                          L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFORECOLOR )(HANNOBJECT hObject,
                                      COLORREF crFore,
                                      L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETGAUGELENGTH)(HANNOBJECT hObject,
                                       L_DOUBLE dLength,
                                       L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTICMARKLENGTH)(HANNOBJECT hObject,
                                         L_DOUBLE dLength,
                                         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETHYPERLINK )(HANNOBJECT hObject,
                                      L_UINT uType,
                                      L_UINT uMsg,
                                      WPARAM wParam,
                                      L_TCHAR *pLink,
                                      L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETHYPERLINKMENUENABLE)(HANNOBJECT hObject,
                                               L_BOOL fEnable,
                                               L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETLINESTYLE )(HANNOBJECT hObject,
                                      L_UINT uLineStyle,
                                      L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETLINEWIDTH )(HANNOBJECT hObject,
                                      L_DOUBLE dLineWidth,
                                      L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETOFFSETX)(HANNOBJECT hObject,
                                   L_DOUBLE dOffsetX,
                                   L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETOFFSETY)(HANNOBJECT hObject,
                                   L_DOUBLE dOffsetY,
                                   L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETPOINTS )(HANNOBJECT hObject,
                                   pANNPOINT pPoints,
                                   L_UINT uCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETPOLYFILLMODE )(HANNOBJECT hObject,
                                         L_UINT uPolyFillMode,
                                         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETROP2 )(HANNOBJECT hObject,
                                 L_UINT uROP2,
                                 L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETRECT )(HANNOBJECT hObject,
                                 pANNRECT pRect);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETSELECTED )(HANNOBJECT hObject,
                                     L_BOOL fSelected,
                                     L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETSCALARX)(HANNOBJECT hObject,
                                   L_DOUBLE dScalarX,
                                   L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETSCALARY)(HANNOBJECT hObject,
                                   L_DOUBLE dScalarY,
                                   L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTAG )(HANNOBJECT hObject,
                                L_UINT32 uTag,
                                L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXT )(HANNOBJECT hObject,
                                 L_TCHAR *pText,
                                 L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXTALIGN)(HANNOBJECT hObject, L_UINT uTextAlign, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXTROTATE)(HANNOBJECT hObject, L_UINT uTextRotate, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXTPOINTERFIXED)(HANNOBJECT hObject, L_BOOL bPointerFixed, L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTOOL )(HANNOBJECT hObject,
                                 L_UINT uTool);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTOOLBARBUTTONVISIBLE)(HWND hwndToolBar,
                                  L_UINT uButton,
                                  L_BOOL fVisible);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTOOLBARCHECKED)(HWND hwndToolBar,
                                  L_UINT uChecked);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTRANSPARENT )(HANNOBJECT hObject,
                                      L_BOOL bTransparent,
                                      L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETUNDODEPTH )(HANNOBJECT hObject,
                                      L_UINT uLevels);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETUNIT)(HANNOBJECT hObject,
                                L_UINT uUnit,
                                L_TCHAR * pUnitAbbrev,
                                L_UINT uPrecision,
                                L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETUSERMODE )(HANNOBJECT hObject,
                                      L_UINT uMode);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETVISIBLE )(HANNOBJECT hObject,
                                     L_BOOL fVisible,
                                     L_UINT uFlags,
                                     L_TCHAR *pUserList);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETWND )(HANNOBJECT hObject,
                                 HWND hWnd);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSHOWLOCKEDICON )(HANNOBJECT hObject,
                                     L_BOOL bShow,
                                     L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNUNDO )(HANNOBJECT hObject);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNUNLOCK )(HANNOBJECT hObject, 
                              L_TCHAR * pUnlockKey, 
                              L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNUNREALIZE)(pBITMAPHANDLE pBitmap, 
                                LPRECT prcBounds, 
                                HANNOBJECT hObject, 
                                L_BOOL fSelected);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETNODES)(HANNOBJECT hObject, 
                                 L_BOOL bShowNodes, 
                                 L_UINT uGapNodes, 
                                 L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETNODES)(HANNOBJECT hObject, 
                                 L_BOOL *pbShowNodes, 
                                 L_UINT *puGapNodes);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETPROTRACTOROPTIONS)(HANNOBJECT hObject, 
                                             L_BOOL  bAcute,
                                             L_UINT  uUnit,
                                             L_TCHAR *pszAbbrev,
                                             L_UINT  uPrecision,
                                             L_DOUBLE dArcRadius,
                                             L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPROTRACTOROPTIONS)(HANNOBJECT hObject,
                                             L_BOOL  *pbAcute,
                                             L_UINT  *puUnit,
                                             L_SIZE_T *puAbbrevLen,
                                             L_TCHAR *pszAbbrev,
                                             L_UINT  *puPrecision,
                                             L_DOUBLE *pdArcRadius);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETNAMEOPTIONS)(HANNOBJECT hObject, pANNNAMEOPTIONS pNameOptions, L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETNAMEOPTIONS)(HANNOBJECT    hObject, 
                                       pANNNAMEOPTIONS pNameOptions, 
                                       L_UINT        uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETSHOWFLAGS )(HANNOBJECT hObject,
                                       L_UINT uShowFlags,
                                       L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSHOWFLAGS )(HANNOBJECT hObject,
                                       L_UINT *puShowFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETANGLE)(HANNOBJECT hObject,
                                 L_DOUBLE *pdAngle);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETMETAFILE )(HANNOBJECT hObject, 
                                     HMETAFILE hMetafile, 
                                     L_UINT uType, 
                                     L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETMETAFILE )(HANNOBJECT hObject, 
                                     HMETAFILE *phMetafile);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSECONDARYMETAFILE )(HANNOBJECT hObject, 
                                              HMETAFILE *phMetafile);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETPREDEFINEDMETAFILE)(L_UINT uType, 
                                              HMETAFILE hMetafile);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPREDEFINEDMETAFILE)(L_UINT uType, 
                                              HMETAFILE *phMetafile,
                                              L_BOOL *pbEnhanced);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETSECONDARYBITMAP )(HANNOBJECT hObject, 
                                            pBITMAPHANDLE pBitmap, 
                                            L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSECONDARYBITMAP )(HANNOBJECT hObject, 
                                            pBITMAPHANDLE pBitmap,
                                            L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOMENUITEMENABLE)(HANNOBJECT hObject, 
                                              L_INT nObjectType, 
                                              L_UINT uItem, 
                                              L_UINT uEnable, 
                                              L_UINT uFlags,
                                              L_TCHAR *pUserList);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOMENUITEMENABLE)(HANNOBJECT hObject, 
                                              L_INT nObjectType, 
                                              L_UINT uItem, 
                                              L_UINT *puEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOMENUSTATE)(HANNOBJECT hObject, 
                                         L_INT nObjectType, 
                                         L_UCHAR *pEnable, 
                                         L_UCHAR *pEnableFlags, 
                                         L_UINT uBits, 
                                         L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOMENUSTATE)(HANNOBJECT hObject, 
                                         L_INT nObjectType, 
                                         L_UCHAR *pEnable, 
                                         L_UCHAR *pEnableFlags, 
                                         L_UINT uBits);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETUSER)(HANNOBJECT hObject, 
                                L_TCHAR *pOldUser, 
                                L_TCHAR *pNewUser, 
                                L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTOOLBARBUTTONS)(HWND hwndToolBar,
                                          pANNBUTTON pButtons,
                                          L_UINT uButtons);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTOOLBARBUTTONS)(HWND hwndToolBar,
                                          pANNBUTTON pButtons,
                                          L_UINT uStructSize,
                                          L_UINT *puButtons);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNFREETOOLBARBUTTONS)(pANNBUTTON pButtons,
                                           L_UINT uButtons);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTOOLBARINFO)(HWND hwndToolBar,
                                       pANNTOOLBARINFO pInfo,
                                       L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTOOLBARCOLUMNS)(HWND hwndToolBar,
                                          L_UINT uColumns);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTOOLBARROWS)(HWND hwndToolBar,
                                          L_UINT uRows);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTODEFAULTS)(HANNOBJECT hAutomation, 
                                        HANNOBJECT hObject, 
                                        L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTRANSPARENTCOLOR )(HANNOBJECT hObject, 
                                             COLORREF crTransparent, 
                                             L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTRANSPARENTCOLOR )(HANNOBJECT hObject, 
                                             COLORREF *pcrTransparent);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETUNDODEPTH )(HANNOBJECT hObject,
                                      L_UINT *puUsedLevels,
                                      L_UINT *puMaxLevels);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGROUP )(HANNOBJECT hObject, 
                               L_UINT uFlags, 
                               L_TCHAR *pUserList);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNUNGROUP )(HANNOBJECT hObject, 
                               L_UINT uFlags, 
                               L_TCHAR *pUserList);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOOPTIONS )(HANNOBJECT hObject,
                                        L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOOPTIONS )(HANNOBJECT hObject,
                                        L_UINT *puFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETOBJECTFROMTAG )(HANNOBJECT hContainer,
                                          L_UINT uFlags,
                                          L_UINT32 uTag,
                                          pHANNOBJECT phTagObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETRGNHANDLE)(HANNOBJECT hObject,
                                    pRGNXFORM pXForm,
                                    HRGN *phRgn);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAREA)(HANNOBJECT hObject, 
                                L_SIZE_T *puCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTODIALOGFONTSIZE)(HANNOBJECT hObject,
                                        L_INT   nFontSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTODIALOGFONTSIZE)(HANNOBJECT hObject,
                                        L_INT * pnFontSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETGROUPING)(HANNOBJECT hObject,
                                    L_BOOL bAutoGroup,
                                    L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETGROUPING)(HANNOBJECT hObject,
                                    L_BOOL * pbAutoGroup);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOBACKCOLOR)(HANNOBJECT hObject,
                                         L_UINT uObjectType,
                                         COLORREF crBack);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOBACKCOLOR)(HANNOBJECT hObject,
                                         L_UINT uObjectType,
                                         COLORREF *pcrBack);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNADDUNDONODE)(HANNOBJECT hObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOUNDOENABLE)(HANNOBJECT hObject,
                                          L_BOOL      bEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOUNDOENABLE)(HANNOBJECT hObject,
                                          L_BOOL *pbEnable);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTOOLBARPARENT)(HWND hwndToolBar, HWND hwndParent);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETENCRYPTOPTIONS )(HANNOBJECT hObject, 
                                           pANNENCRYPTOPTIONS pEncryptOptions,
                                           L_UINT uFlags);


typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETENCRYPTOPTIONS)(HANNOBJECT hObject,
                                          pANNENCRYPTOPTIONS pEncryptOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNENCRYPTAPPLY)(HANNOBJECT    hObject, 
                                     L_UINT uEncryptFlags, 
                                     L_UINT        uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETPREDEFINEDBITMAP)(L_UINT uType, 
                                            pBITMAPHANDLE pBitmap);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPREDEFINEDBITMAP)(L_UINT uType, 
                                            pBITMAPHANDLE pBitmap,
                                            L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPOINTOPTIONS)(HANNOBJECT hObject,
                                        pANNPOINTOPTIONS pPointOptions,
                                        L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETPOINTOPTIONS )(HANNOBJECT    hObject, 
                                         pANNPOINTOPTIONS pPointOptions, 
                                         L_UINT        uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNADDUSERHANDLE)(HANNOBJECT hObject,
                                      pANNHANDLE pAnnHandle);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETUSERHANDLE)(HANNOBJECT hObject,
                                      L_INT32 uIndex,
                                      pANNHANDLE pAnnHandle);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETUSERHANDLES)(HANNOBJECT hObject,
                                       pANNHANDLE pAnnHandle,
                                       L_UINT    *pCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCHANGEUSERHANDLE)(HANNOBJECT hObject,
                                         L_INT32 nIndex,
                                         pANNHANDLE pAnnHandle);



typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDELETEUSERHANDLE)(HANNOBJECT hObject,
                                         L_INT32 nIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNENUMERATEHANDLES)(HANNOBJECT hObject, 
                                         ANNENUMHANDLECALLBACK pfnCallback, 
                                         L_VOID *pUserData);

// Misc functions

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDEBUG)(HANNOBJECT hObject, LPXFORM pXForm);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDUMPOBJECT)(HANNOBJECT hObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNHITTEST)(HANNOBJECT hObject,
                                   LPPOINT pPoint,
                                   L_UINT *puResult,
                                   pHANNOBJECT phObjectHit,
                                   pANNHITTESTINFO pHitTestInfo,
                                   L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETROTATEANGLE)(HANNOBJECT hObject, L_DOUBLE *pdAngle);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNADJUSTPOINT)(pANNPOINT pptAnchor, pANNPOINT pptMove, L_DOUBLE dAngle, L_INT nType);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCONVERT)(HANNOBJECT hContainer, LPPOINT pPoints, pANNPOINT pAnnPoints, L_INT nCount, L_INT nConvert);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNRESTRICTCURSOR)(HANNOBJECT hContainer, LPRECT lpRect, LPPOINT pPoint, LPRECT prcOldClip, L_BOOL bRestrictClient);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETRESTRICTTOCONTAINER)(HANNOBJECT hObject, L_BOOL bRestrict, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETRESTRICTTOCONTAINER)(HANNOBJECT hObject, L_BOOL * pbRestrict);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDEFINE2)(HANNOBJECT hObject, pANNPOINT apt, L_UINT uState);

// Text Token Table Functions
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNINSERTTEXTTOKENTABLE)(HANNOBJECT hAutomation, pANNTEXTTOKEN pTextToken);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNENUMERATETEXTTOKENTABLE)(HANNOBJECT hAutomation, ANNENUMTEXTTOKENTABLECALLBACK pfnCallback,L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDELETETEXTTOKENTABLE)(HANNOBJECT hAutomation, L_TCHAR cToken);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCLEARTEXTTOKENTABLE)(HANNOBJECT hAutomation);

// Fixed Annotation Functions
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETNOSCROLL)(HANNOBJECT hObject, L_BOOL bNoScroll, L_UINT uFlags, L_TCHAR *pUserList);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETNOSCROLL)(HANNOBJECT hObject, L_BOOL *pbNoScroll);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETNOZOOM)(HANNOBJECT hObject, L_BOOL bNoZoom, L_UINT uFlags, L_TCHAR *pUserList);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETNOZOOM)(HANNOBJECT hObject, L_BOOL *pbNoZoom);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNENABLEFIXED)(HANNOBJECT hObject, L_BOOL bEnable, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFIXED)(HANNOBJECT hObject, L_BOOL *pbFixed);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFIXED)(HANNOBJECT hObject, L_BOOL bFixed, L_BOOL bAdjust, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNPUSHFIXEDSTATE)(HANNOBJECT hObject, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNPOPFIXEDSTATE)(HANNOBJECT hObject, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNISFIXEDINRECT)(HANNOBJECT hObject,
                                    LPRECT prc, 
                                    L_BOOL *pbFixedInRect,
                                    L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETDISTANCE2)(HANNOBJECT          hObject,
                                     L_UINT             *puCount,
                                     pANNSMARTDISTANCE   pDistance,
                                     pANNSMARTDISTANCE   pTotalDistance,
                                     L_UINT              uStructSize);

typedef L_VOID ( pWRPEXT_CALLBACK pL_ANNDUMPSMARTDISTANCE)(ANNSMARTDISTANCE sdSmartDistance, L_TCHAR *pszSmartDistance, L_UINT *puLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOCURSOR)(HANNOBJECT hAutomation, L_UINT uItem, HCURSOR hCursor);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOCURSOR)(HANNOBJECT hAutomation, L_UINT uItem, HCURSOR *phCursor);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETUSERDATA)(HANNOBJECT hObject, L_UCHAR *pUserData, L_UINT uUserDataSize, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETUSERDATA)(HANNOBJECT hObject, L_UCHAR *pUserData, L_UINT *puUserDataSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXTRTF)(HANNOBJECT hObject, L_UINT uFormat, L_TCHAR *pText, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTRTF)(HANNOBJECT hObject, L_UINT uFormat, L_TCHAR *pText, L_SIZE_T  *puLen);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_DEBUGENABLEWORLDTRANSFORM)(L_BOOL bEnable);


typedef L_VOID ( pWRPEXT_CALLBACK pL_DEBUGSETGLOBAL)(L_INT nValue);
typedef L_INT ( pWRPEXT_CALLBACK pL_DEBUGGETGLOBAL)();

typedef L_TCHAR * ( pWRPEXT_CALLBACK pL_ANNSETLOCALE)(L_INT nCategory, const L_TCHAR *lpszLocale);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOHILIGHTPEN)(HANNOBJECT hAutomation, COLORREF crHilight);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETOPTIONS)(HANNOBJECT hAutomation, L_UINT uOptions);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETOPTIONS)(L_UINT *puOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFILLMODE )(HANNOBJECT hObject,
                                     L_UINT *puFillMode,
                                     L_INT *pnAlpha);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFILLMODE )(HANNOBJECT hObject,
                                     L_UINT uFillMode,
                                     L_INT  nAlpha,
                                     L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETROTATEOPTIONS)(HANNOBJECT hObject, pANNROTATEOPTIONS pRotateOptions, L_UINT uStructSize);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETROTATEOPTIONS)(HANNOBJECT hObject, pANNROTATEOPTIONS pRotateOptions, L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCALIBRATERULER)(HANNOBJECT hObject, L_DOUBLE dCalibrateLength, L_UINT uCalibrateUnit, L_DOUBLE dDpiRatioXtoY);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNTEXTEDIT)(HANNOBJECT hObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXTOPTIONS)(HANNOBJECT hObject, pANNTEXTOPTIONS pTextOptions, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTOPTIONS)(HANNOBJECT hObject, pANNTEXTOPTIONS pTextOptions, L_UINT uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETAUTOSNAPCURSOR)(HANNOBJECT hAutomation, L_BOOL *pbSnap);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETAUTOSNAPCURSOR)(HANNOBJECT hAutomation, L_BOOL bSnap);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXTFIXEDSIZE)(HANNOBJECT hObject, L_BOOL bTextFixedSize, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTFIXEDSIZE)(HANNOBJECT hObject, L_BOOL *pbTextFixedSize);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETLINEFIXEDWIDTH)(HANNOBJECT hObject, L_BOOL *pbLineFixedWidth);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETLINEFIXEDWIDTH)(HANNOBJECT hObject, L_BOOL bLineFixedWidth, L_UINT uFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPOINTEROPTIONS)(HANNOBJECT hObject, pANNPOINTEROPTIONS pOptions, L_UINT uStructSize);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETPOINTEROPTIONS)(HANNOBJECT hObject, pANNPOINTEROPTIONS pOptions, L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETRENDERMODE)(HANNOBJECT hObject, L_UINT32 uRenderMode, L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETSHOWSTAMPBORDER)(HANNOBJECT hObject, L_BOOL *pbShowStampBorder);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETSHOWSTAMPBORDER)(HANNOBJECT hObject, L_BOOL bShowStampBorder, L_UINT uFlags);

//-----------------------------------------------------------------------------
//--LTSCR.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT (pWRPEXT_CALLBACK pL_SETCAPTUREOPTION)(pLEADCAPTUREOPTION );
typedef L_INT (pWRPEXT_CALLBACK pL_GETCAPTUREOPTION) (pLEADCAPTUREOPTION pOptions, L_UINT uStructSize);

typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREWINDOW) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, HWND hWnd, WINDOWCAPTURETYPE wctCaptureType,pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREACTIVEWINDOW) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREACTIVECLIENT) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);

typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREWALLPAPER) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREFULLSCREEN) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREMENUUNDERCURSOR) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREWINDOWUNDERCURSOR) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);

typedef L_INT (pWRPEXT_CALLBACK pL_CAPTURESELECTEDOBJECT) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, pLEADCAPTUREOBJECTOPTION pObjectOptions, L_UINT uOptionsStructSize, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback,L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREAREA) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, pLEADCAPTUREAREAOPTION pCaptureAreaOption, L_UINT uOptionsStructSize, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);

typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREMOUSECURSOR) (pBITMAPHANDLE pBitmap, L_UINT uBitmapStructSize, COLORREF crFill, pLEADCAPTUREINFO pCaptureInfo, L_UINT uInfoStructSize, CAPTURECALLBACK pfnCaptureCallback, L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTURESETHOTKEYCALLBACK)(CAPTUREHOTKEYCALLBACK , L_VOID * );

typedef L_INT (pWRPEXT_CALLBACK pL_SETCAPTUREOPTIONDLG) (HWND hwndOwner,L_UINT uFlags,pLEADCAPTUREOPTION pOptions,LTSCRHELPCB pfnCallBack,L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREAREAOPTIONDLG) (HWND hParentWnd,L_UINT uFlags,pLEADCAPTUREAREAOPTION pCaptureAreaOption,L_INT nUseDefault,LTSCRHELPCB pfnCallBack,L_VOID * pUserData);
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREOBJECTOPTIONDLG) (HWND hParentWnd,L_UINT uFlags,pLEADCAPTUREOBJECTOPTION pObjectOptions,L_INT nUseDefault,LTSCRHELPCB pfnCallBack,L_VOID * pUserData);

typedef L_INT (pWRPEXT_CALLBACK pL_GETDEFAULTAREAOPTION) (pLEADCAPTUREAREAOPTION pCaptureAreaOption, L_UINT uStructSize);
typedef L_INT (pWRPEXT_CALLBACK pL_GETDEFAULTOBJECTOPTION) (pLEADCAPTUREOBJECTOPTION pObjectOptions, L_UINT uStructSize);
typedef L_INT (pWRPEXT_CALLBACK pL_STOPCAPTURE)();

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//                          C A P T U R E       E X E       F U N C T I O N S
//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREFROMEXEDLG) (pBITMAPHANDLE pBitmap,L_UINT uBitmapStructSize,L_TCHAR * pszFileName,LPCOLORREF pTransparentColor,L_INT nResType,L_INT nDialogType,L_UINT uFlags,pLEADCAPTUREINFO pCaptureInfo,L_UINT uInfoStructSize,CAPTURECALLBACK pfnCaptureCallback,L_VOID * pUserData,LTSCRHELPCB pfnCallBack,L_VOID * pHlpUserData);

typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREFROMEXE)
(
   pBITMAPHANDLE ,
   L_UINT ,
   L_TCHAR * ,
   L_INT ,
   L_TCHAR * ,
   L_BOOL ,
   COLORREF ,
   CAPTURECALLBACK ,
   L_VOID * 
);

typedef L_INT (pWRPEXT_CALLBACK pL_CAPTUREGETRESCOUNT)
(
   L_TCHAR * ,
   L_INT ,        
   L_INT32 * 
);


typedef L_BOOL (pWRPEXT_CALLBACK pL_ISCAPTUREACTIVE)();

//-----------------------------------------------------------------------------
//--LTNET.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
/************************************************************************************
 *                                                                                  *
 *                  INTERNET FUNCTIONS                                              *
 *                                                                                  *
 ************************************************************************************/
typedef L_INT ( pWRPEXT_CALLBACK pL_INETCONNECT)(L_CHAR *pszAddress,
                                             L_INT nPort,
                                             L_COMP *phComputer,
                                             INETCALLBACK pfnCallback,
                                             L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSERVERINIT)(L_INT nPort,
                                                L_COMP *phComputer,
                                                INETCALLBACK pfnCallback,
                                                L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETCLOSE)(L_COMP hComputer, L_BOOL bGraceful);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDDATA)(L_COMP hComputer, 
                                              L_CHAR *pBuffer, 
                                              L_SIZE_T *pulBufferLength,
                                              IDATATYPE uDataType);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDMMDATA)(L_COMP hComputer, 
                                                L_CHAR *pBuffer, 
                                                L_SIZE_T *pulBufferLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETREADDATA)(L_COMP hComputer, 
                                              L_CHAR *pBuffer, 
                                              L_UINT32 *pulBufferLength);


typedef L_INT ( pWRPEXT_CALLBACK pL_INETGETHOSTNAME)(L_COMP hHost,
                                                 L_CHAR *pszName,
                                                 L_INT nType,
                                                 L_SIZE_T *pulBufferLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETACCEPTCONNECT)(L_COMP hHost, 
                                                   L_COMP *phRemote,
                                                   INETCALLBACK pfnCallback,
                                                   L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDBITMAP)(L_COMP hComputer, 
                                                pBITMAPHANDLE pBitmap,
                                                L_INT nFormat, 
                                                L_INT nBitsPerPixel, 
                                                L_INT nQFactor,
                                                L_SIZE_T *pulImageLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETAUTOPROCESS)(L_COMP hComputer,
                                                 L_BOOL bProcess);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDRAWDATA)(L_COMP hComputer, 
                                                 L_CHAR *pBuffer,
                                                 L_SIZE_T *pulBufferLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETGETQUEUESIZE)(L_COMP hComputer, 
                                                  L_SIZE_T *pulLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETCLEARQUEUE)(L_COMP hComputer);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSTARTUP)(void);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSHUTDOWN)(void);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSOUND)(L_COMP hComputer, 
                                               LPWAVEFORMATDATA pWaveFormatData, 
                                               LPWAVEDATA pWaveData, 
                                               L_SIZE_T *pdwDataSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETATTACHTOSOCKET)(L_COMP *phComputer,
                                                   SOCKET hSocket,
                                                   INETCALLBACK pfnCallback,
                                                   L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETDETACHFROMSOCKET)(L_COMP hComputer,
                                                      L_BOOL bWaitTillQueueEmpty,
                                                      SOCKET *phSocket);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSETCALLBACK)(L_COMP hComputer,
                                                 INETCALLBACK  pfnCallback,
                                                 L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETGETCALLBACK)(L_COMP hComputer,
                                                 INETCALLBACK *ppfnCallback,
                                                 L_VOID * * ppUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETCREATEPACKET)(pHINETPACK phPacket,
                                                   L_UINT uExtra,
                                                   L_VOID *pExtra,
                                                   L_CHAR *pszFormat,
                                                   ...);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETCREATEPACKETFROMPARAMS)(pHINETPACK phPacket,
                                                   L_UINT uExtra,
                                                   L_VOID *pExtra,
                                                   L_UINT uParama,
                                                   pPARAMETER pParams);

typedef L_VOID ( pWRPEXT_CALLBACK pL_INETFREEPACKET)(HINETPACK hPacket);


typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDCMD)(L_COMP hComputer, 
                                             CMDTYPE uCommand,
                                             L_UINT uCommandID,
                                             HINETPACK hPacket);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDRSP)(L_COMP hComputer, 
                                             CMDTYPE uCommand,
                                             L_UINT uCommandID,
                                             HINETPACK hPacket,
                                             L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDLOADCMD)(L_COMP hComputer,
                                                 L_UINT uCommandID,
                                                 L_TCHAR *pszFile,
                                                 L_INT nBitsPerPixel,
                                                 L_INT nOrder,
                                                 L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDLOADRSP)(L_COMP hComputer,
                                                 L_UINT uCommandID,
                                                 L_UINT uBitmapID,
                                                 L_UINT uExtra,
                                                 L_CHAR *pExtra,
                                                 L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSAVECMD)(L_COMP hComputer,
                                                 L_UINT uCommandID,
                                                 L_TCHAR *pszFile,
                                                 L_UINT uBitmapID,
                                                 L_INT nFormat,
                                                 L_INT nBitsPerPixel, 
                                                 L_INT nQFactor,
                                                 L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSAVERSP)(L_COMP hComputer,
                                                  L_UINT uCommandID,
                                                  L_UINT uExtra,
                                                  L_CHAR *pExtra,
                                                  L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDCREATEWINCMD)(L_COMP hComputer,
                                                      L_UINT uCommandID,
                                                      L_TCHAR *pszClassName,
                                                      L_TCHAR *pszWindowName,
                                                      L_UINT ulFlags,
                                                      L_INT nLeft,
                                                      L_INT nTop,
                                                      L_INT nWidth, 
                                                      L_INT nHeight,
                                                      L_UINT uParentID);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDCREATEWINRSP)(L_COMP hComputer,
                                                      L_UINT uCommandID,
                                                      LONG_PTR uWindowID,
                                                      L_UINT uLength,
                                                      L_CHAR *pExtraInfo,
                                                      L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSIZEWINCMD)(L_COMP hComputer,
                                                    L_UINT uCommandID,
                                                    LONG_PTR uWindowID,
                                                    L_INT nLeft,
                                                    L_INT nTop,
                                                    L_INT nWidth,
                                                    L_INT nHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSIZEWINRSP)(L_COMP hComputer,
                                                    L_UINT uCommandID,
                                                    L_UINT uLength,
                                                    L_CHAR *pExtraInfo,
                                                    L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSHOWWINCMD)(L_COMP hComputer,
                                                    L_UINT uCommandID,
                                                    LONG_PTR uWindowID,
                                                    L_INT nCmdShow);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSHOWWINRSP)(L_COMP hComputer,
                                                    L_UINT uCommandID,
                                                    L_UINT uLength,
                                                    L_CHAR *pExtraInfo,
                                                    L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDCLOSEWINCMD)(L_COMP hComputer,
                                                     L_UINT uCommandID,
                                                     LONG_PTR uWindowID);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDCLOSEWINRSP)(L_COMP hComputer,
                                                     L_UINT uCommandID,
                                                     L_UINT uLength,
                                                     L_CHAR *pExtraInfo,
                                                     L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDFREEBITMAPCMD)(L_COMP hComputer,
                                                     L_UINT uCommandID,
                                                     L_UINT uBitmapID);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDFREEBITMAPRSP)(L_COMP hComputer,
                                                     L_UINT uCommandID,
                                                     L_UINT uLength,
                                                     L_CHAR *pExtraInfo,
                                                     L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSETRECTCMD)(L_COMP hComputer,
                                                    L_UINT uCommandID,
                                                    LONG_PTR uWindowID,
                                                    RECTTYPE nType,
                                                    L_INT nLeft,
                                                    L_INT nTop,
                                                    L_INT nWidth,
                                                    L_INT nHeight);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDSETRECTRSP)(L_COMP hComputer,
                                                    L_UINT uCommandID,
                                                    L_UINT uLength,
                                                    L_CHAR *pExtraInfo,
                                                    L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSETCOMMANDCALLBACK)(L_COMP hComputer, 
                                                        INETCOMMANDCALLBACK pfnCallback,
                                                        L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSETRESPONSECALLBACK)(L_COMP hComputer,
                                                         INETRESPONSECALLBACK pfnCallback,
                                                         L_VOID *pUserData);


typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDATTACHBITMAPCMD)(L_COMP hComputer,
                                                         L_UINT uCommandID,
                                                         L_UINT uBitmapID,
                                                         LONG_PTR uWindowID);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDATTACHBITMAPRSP)(L_COMP hComputer,
                                                         L_UINT uCommandID,
                                                         L_UINT uExtra,
                                                         L_CHAR *pExtra,
                                                         L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDGETMAGGLASSDATACMD)(L_COMP hComputer,
                                                            L_UINT uCommandID,
                                                            L_UINT uBitmapID,
                                                            L_UINT32 nMaskPlaneSize,
                                                            L_UCHAR *pMaskPlane,
                                                            L_INT nMaskPlaneStart,
                                                            L_INT nMaskPlaneEnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETSENDGETMAGGLASSDATARSP)(L_COMP hComputer,
                                                            L_UINT uCommandID,
                                                            L_UINT32 lColorSize,
                                                            COLORREF *pColor,
                                                            L_UINT32 nMaskPlaneSize,
                                                            L_UCHAR *pMaskPlane,
                                                            L_INT nMaskPlaneStart,
                                                            L_INT nMaskPlaneEnd,
                                                            L_UINT uExtra,
                                                            L_CHAR *pExtra,
                                                            L_INT nStatus);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETGETMAGGLASSDATA)(pBITMAPHANDLE pBitmap,
                                                     L_UINT32 *plColorSize,
                                                     COLORREF *pColor,
                                                     L_UCHAR *pMaskPlane,
                                                     L_INT nMaskPlaneStart,
                                                     L_INT nMaskPlaneEnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETGETPARAMETERS)(L_UINT *puParams, 
                                       pPARAMETER *ppParams, 
                                       L_CHAR *pszFormat, 
                                       va_list pArgs);
typedef L_INT ( pWRPEXT_CALLBACK pL_INETCOPYPARAMETERS)(pPARAMETER *ppParams, 
                                        L_UINT uParams, 
                                        pPARAMETER pParams);

typedef L_VOID ( pWRPEXT_CALLBACK pL_INETFREEPARAMETERS)(pPARAMETER pParams, L_INT nCount);

//-----------------------------------------------------------------------------
//--LTWEB.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPCONNECT)(L_TCHAR *pszServer,L_INT iPort,L_TCHAR *pszUserName,
                                   L_TCHAR *pszPassword,pHFTP pFtp);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPDISCONNECT)(HFTP hFtp);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPSENDFILE)(HFTP hFtp,L_TCHAR *pszLocal,L_TCHAR *pszRemote,L_UINT uSendAs);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPCHANGEDIR)(HFTP hFtp,L_TCHAR *pszDirectory);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPGETFILE)(HFTP hFtp, L_TCHAR *pszRemote, L_TCHAR *pszLocal,
                                   L_BOOL bOverwrite, L_UINT uSendAs);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPRENAMEFILE)(HFTP hFtp, L_TCHAR *pszOld, L_TCHAR *pszNew);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPDELETEFILE)(HFTP hFtp, L_TCHAR *pszRemote);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPCREATEDIR)(HFTP hFtp, L_TCHAR *pszRemoteDir);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPDELETEDIR)(HFTP hFtp, L_TCHAR *pszRemoteDir);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPGETCURRENTDIR)(HFTP hFtp, L_TCHAR *pszRemoteDir, L_UINT32 ulSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPSENDBITMAP)(HFTP hFtp, pBITMAPHANDLE pBitmap, L_INT nFormat, 
                                      L_INT nBitsPerPixel, L_INT nQFactor, pSAVEFILEOPTION pSaveOptions,
                                      L_TCHAR *pszRemote, L_UINT uSendAs);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETFTPBROWSEDIR)(HFTP hFtp, L_TCHAR *pszSearch,FTPBROWSECALLBACK pfnCallback, L_VOID *pData);

// HTTP functions

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPCONNECT)(L_TCHAR *pszServer,L_INT iPort,L_TCHAR *pszUserName,
                                    L_TCHAR *pszPassword,pHINET pHttp);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPDISCONNECT)(HINET hHttp);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPOPENREQUEST)(HINET hHttp,L_UINT uType,L_TCHAR *pszTarget,
                                        L_TCHAR *pszReferer,L_TCHAR *pszVersion,
                                        L_UINT32 dwReserved);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPOPENREQUESTEX)(HINET hHttp,L_UINT uType,L_TCHAR *pszTarget,
                                          L_TCHAR *pszReferer,L_TCHAR *pszVersion,
                                          L_UINT32 dwReserved, L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPCLOSEREQUEST)(HINET hHttp);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPSENDREQUEST)(HINET hHttp,L_TCHAR *pszHeader,L_UINT32 ulHeaderSize,
                                        L_TCHAR *pszOptional,L_UINT32 ulOptionalSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPSENDBITMAP)(HINET hHttp,pBITMAPHANDLE pBitmap,L_INT nFormat,
                                       L_INT nBitsPerPixel,L_INT nQFactor,
                                       L_TCHAR *pszContentType,pNAMEVALUE pNameValue,
                                       pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPSENDDATA)(HINET hHttp,L_CHAR *pData, L_UINT32 uSize,
                                     L_TCHAR *pszContentType,pNAMEVALUE pNameValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPSENDFORM)(HINET hHttp,pNAMEVALUE pNameValue,L_UINT uCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPGETRESPONSE)(HINET hHttp,L_CHAR *pszData,L_UINT32 *ulSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_INETHTTPGETSERVERSTATUS)(HINET hHttp,L_UINT *uStatus);

//-----------------------------------------------------------------------------
//--LTTMB.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

typedef L_INT ( pWRPEXT_CALLBACK pL_BROWSEDIR)( 
               L_TCHAR* pszPath,
               L_TCHAR* pszFilter,
               pTHUMBOPTIONS pThumbOptions,
               L_BOOL bStopOnError,
               L_BOOL bIncludeSubDirs,
               L_BOOL bExpandMultipage,
               L_SSIZE_T lSizeDisk,
               L_SSIZE_T lSizeMem,
               BROWSEDIRCALLBACK pfnBrowseDirCB,
               L_VOID* pUserData );

//-----------------------------------------------------------------------------
//--LTLST.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_VOID (pWRPEXT_CALLBACK pL_USEIMAGELISTCONTROL )(L_VOID);

typedef HWND ( pWRPEXT_CALLBACK pL_CREATEIMAGELISTCONTROL)(DWORD dwStyle,
                                                          L_INT x,
                                                          L_INT y,
                                                          L_INT nWidth,
                                                          L_INT nHeight,
                                                          HWND hWndParent,
                                                          L_INT nID,
                                                          COLORREF crBack);

//-----------------------------------------------------------------------------
//--LVKRN.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDUPLICATEOBJECTDESCRIPTOR)( L_INT, L_VOID *, const L_VOID * );
//Do not remove the one above

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  General functions.                                                   []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECINIT)(pVECTORHANDLE pVector);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECFREE)(pVECTORHANDLE pVector);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECEMPTY)(pVECTORHANDLE pVector);
typedef L_BOOL ( pWRPEXT_CALLBACK pL_VECISEMPTY)(const pVECTORHANDLE pVector);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCOPY)(pVECTORHANDLE pDst, const pVECTORHANDLE pSrc, L_UINT32 dwFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETDISPLAYOPTIONS)(pVECTORHANDLE pVector, const pVECTOR_DISPLAY_OPTIONS pOptions);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETDISPLAYOPTIONS)(pVECTORHANDLE pVector, pVECTOR_DISPLAY_OPTIONS pOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECINVERTCOLORS)(pVECTORHANDLE pVector);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETVIEWPORT)(pVECTORHANDLE pVector, const RECT *pViewport);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETVIEWPORT)(const pVECTORHANDLE pVector, RECT *pViewport);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETPAN)(pVECTORHANDLE pVector, const POINT *pPan);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETPAN)(const pVECTORHANDLE pVector, POINT *pPan);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECPAINT)(HDC hDC, const pVECTORHANDLE pVector, L_BOOL bEraseBkgnd);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECREALIZE)(pBITMAPHANDLE pBitmap, const pVECTORHANDLE pVetcor, L_BOOL bEraseBkgnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECPAINTDC)(HDC hDC, const pVECTORHANDLE pVector, L_UINT uWidth, L_UINT uHeight, const RECT *pSrc, const RECT *pSrcClip, const RECT *pDest, const RECT *pDestClip, L_UINT32 dwFlags);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_VECIS3D)(const pVECTORHANDLE pVector);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_VECISLOCKED)(const pVECTORHANDLE pVector);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETLOCKED)(pVECTORHANDLE pVector, L_BOOL bLock);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETBACKGROUNDCOLOR)(pVECTORHANDLE pVector, COLORREF Color);
typedef COLORREF ( pWRPEXT_CALLBACK pL_VECGETBACKGROUNDCOLOR)(const pVECTORHANDLE pVector);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECLOGICALTOPHYSICAL)(const pVECTORHANDLE pVector, POINT *pDst, const pVECTORPOINT pSrc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECPHYSICALTOLOGICAL)(const pVECTORHANDLE pVector, pVECTORPOINT pDst, const POINT *pSrc);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETPALETTE)(pVECTORHANDLE pVector, HPALETTE hPalette);
typedef HPALETTE ( pWRPEXT_CALLBACK pL_VECGETPALETTE)(const pVECTORHANDLE pVector);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETVIEWMODE)(pVECTORHANDLE pVector, L_INT nMode);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETVIEWMODE)(const pVECTORHANDLE pVector);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Transformation function.                                             []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETTRANSLATION)(pVECTORHANDLE pVector, const pVECTORPOINT pTranslation, pVECTOROBJECT pObject, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETTRANSLATION)(const pVECTORHANDLE pVector, pVECTORPOINT pTranslation);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETROTATION)(pVECTORHANDLE pVector, const pVECTORPOINT pRotation, pVECTOROBJECT pObject, const pVECTORPOINT pOrigin, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETROTATION)(const pVECTORHANDLE pVector, pVECTORPOINT pRotation);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETSCALE)(pVECTORHANDLE pVector, const pVECTORPOINT pScale, pVECTOROBJECT pObject, const pVECTORPOINT pOrigin, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETSCALE)(const pVECTORHANDLE pVector, pVECTORPOINT pScale);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETORIGIN)(pVECTORHANDLE pVector, const pVECTORPOINT pOrigin);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETORIGIN)(const pVECTORHANDLE pVector, pVECTORPOINT pOrigin);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECAPPLYTRANSFORMATION)(pVECTORHANDLE pVector);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECZOOMRECT)(pVECTORHANDLE pVector, const RECT *pRect);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Attributes functions.                                                []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETBINDVERTICESMODE)(pVECTORHANDLE pVector, L_INT nMode);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETBINDVERTICESMODE)(const pVECTORHANDLE pVector);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETPARALLELOGRAM)(pVECTORHANDLE pVector, const pVECTORPOINT pMin, const pVECTORPOINT pMax);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETPARALLELOGRAM)(const pVECTORHANDLE pVector, pVECTORPOINT pMin, pVECTORPOINT pMax);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECENUMVERTICES)(pVECTORHANDLE pVector, pVECTORENUMVERTICESPROC pEnumProc, L_VOID *pUserData, L_UINT32 dwFlags);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Camera functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETCAMERA)(pVECTORHANDLE pVector, const pVECTORCAMERA pCamera);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETCAMERA)(const pVECTORHANDLE pVector, pVECTORCAMERA pCamera);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Metafile functions.                                                  []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef HMETAFILE ( pWRPEXT_CALLBACK pL_VECCONVERTTOWMF)(HDC hDC, const pVECTORHANDLE pVector, const RECT *pRect, L_UINT uDPI);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCONVERTFROMWMF)(HDC hDC, pVECTORHANDLE pVector, HMETAFILE hWMF);

typedef HENHMETAFILE ( pWRPEXT_CALLBACK pL_VECCONVERTTOEMF)(HDC hDC, const pVECTORHANDLE pVector, const RECT *pRect, L_UINT uDPI);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCONVERTFROMEMF)(HDC hDC, pVECTORHANDLE pVector, HENHMETAFILE hEMF);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Engine functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECATTACHTOWINDOW)(HWND hWnd, pVECTORHANDLE pVector, L_UINT32 dwFlags);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Marker functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETMARKER)(pVECTORHANDLE pVector, const pVECTORMARKER pMarker);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETMARKER)(const pVECTORHANDLE pVector, pVECTORMARKER pMarker);

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Unit functions.                                                      []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
/* Reserved for internal use */
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETUNIT)(pVECTORHANDLE pVector, const pVECTORUNIT pUnit);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETUNIT)(const pVECTORHANDLE pVector, pVECTORUNIT pUnit);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCONVERTPOINTTOUNIT)(const pVECTORHANDLE pVector, pVECTORPOINT pptDst, const pVECTORPOINT pptSrc, pVECTORUNIT UnitToUse);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCONVERTPOINTFROMUNIT)(const pVECTORHANDLE pVector, pVECTORPOINT pptDst, const pVECTORPOINT pptSrc, pVECTORUNIT UnitToUse);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Hit test functions.                                                  []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETHITTEST)(pVECTORHANDLE pVector, const pVECTORHITTEST pHitTest);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETHITTEST)(const pVECTORHANDLE pVector, pVECTORHITTEST pHitTest);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECHITTEST)(const pVECTORHANDLE pVector, const POINT *pPoint, pVECTOROBJECT pObject);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Polygon functions.                                                   []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETPOLYGONMODE)(pVECTORHANDLE pVector, L_INT nMode);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETPOLYGONMODE)(const pVECTORHANDLE pVector);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETAMBIENTCOLOR)(pVECTORHANDLE pVector, COLORREF Color);
typedef COLORREF ( pWRPEXT_CALLBACK pL_VECGETAMBIENTCOLOR)(const pVECTORHANDLE pVector);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Clipboard functions.                                                 []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_BOOL ( pWRPEXT_CALLBACK pL_VECCLIPBOARDREADY)(L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECCOPYTOCLIPBOARD)(HWND hWnd, const pVECTORHANDLE pVector, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCOPYFROMCLIPBOARD)(HWND hWnd, pVECTORHANDLE pVector, L_UINT32 dwFlags);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Layer functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECADDLAYER)(pVECTORHANDLE pVector, const pVECTORLAYERDESC pLayerDesc, pVECTORLAYER pLayer, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDELETELAYER)(pVECTORHANDLE pVector, const pVECTORLAYER pLayer);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECEMPTYLAYER)(pVECTORHANDLE pVector, const pVECTORLAYER pLayer);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCOPYLAYER)(pVECTORHANDLE pVectorDst, const pVECTORLAYER pLayerDst, const pVECTORHANDLE pVectorSrc, const pVECTORLAYER pLayerSrc, L_UINT32 dwFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETLAYERBYNAME)(const pVECTORHANDLE pVector, const L_TCHAR *pszName, pVECTORLAYER pLayer);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETLAYERCOUNT)(const pVECTORHANDLE pVector);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETLAYERBYINDEX)(const pVECTORHANDLE pVector, L_INT nIndex, pVECTORLAYER pLayer);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETLAYER)(const pVECTORHANDLE pVector, const pVECTORLAYER pLayer, pVECTORLAYERDESC pLayerDesc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECFREELAYER)(pVECTORLAYERDESC pLayerDesc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETLAYER)(pVECTORHANDLE pVector, const pVECTORLAYER pLayer, const pVECTORLAYERDESC pLayerDesc);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETACTIVELAYER)(pVECTORHANDLE pVector, const pVECTORLAYER pLayer);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETACTIVELAYER)(const pVECTORHANDLE pVector, pVECTORLAYER pLayer);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECENUMLAYERS)(pVECTORHANDLE pVector, pVECTORENUMLAYERSPROC pEnumProc, L_VOID *pUserData);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Group functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECADDGROUP)(pVECTORHANDLE pVector, const pVECTORGROUPDESC pGroupDesc, pVECTORGROUP pGroup, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDELETEGROUP)(pVECTORHANDLE pVector, const pVECTORGROUP pGroup);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDELETEGROUPCLONES)(pVECTORHANDLE pVector, const pVECTORGROUP pGroup, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECEMPTYGROUP)(pVECTORHANDLE pVector, const pVECTORGROUP pGroup);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCOPYGROUP)(pVECTORHANDLE pVectorDst, const pVECTORGROUP pGroupDst, const pVECTORHANDLE pVectorSrc, const pVECTORGROUP pGroupSrc, L_UINT32 dwFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETGROUPBYNAME)(const pVECTORHANDLE pVector, const L_TCHAR *pszName, pVECTORGROUP pGroup);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETGROUPCOUNT)(const pVECTORHANDLE pVector);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETGROUPBYINDEX)(const pVECTORHANDLE pVector, L_INT nIndex, pVECTORGROUP pGroup);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETGROUP)(const pVECTORHANDLE pVector, const pVECTORGROUP pGroup, pVECTORGROUPDESC pGroupDesc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECFREEGROUP)(pVECTORGROUPDESC pGroupDesc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETGROUP)(pVECTORHANDLE pVector, const pVECTORGROUP pGroup, const pVECTORGROUPDESC pGroupDesc);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECENUMGROUPS)(pVECTORHANDLE pVector, pVECTORENUMGROUPSPROC pEnumProc, L_VOID *pUserData);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Object functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECINITOBJECT)(pVECTOROBJECT pObject);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECADDOBJECT)(pVECTORHANDLE pVector, const pVECTORLAYER pLayer, L_INT nType, const L_VOID *pObjectDesc, pVECTOROBJECT pNewObject);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDELETEOBJECT)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECCOPYOBJECT)(pVECTORHANDLE pVectorDst, const pVECTORLAYER pLayerDst, pVECTOROBJECT pObjectDst, const pVECTORHANDLE pVectorSrc, const pVECTOROBJECT pObjectSrc);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETOBJECT)(const pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_INT nType, L_VOID *pObjectDesc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECFREEOBJECT)(L_INT nType, L_VOID *pObjectDesc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETOBJECT)(pVECTORHANDLE pVector, pVECTOROBJECT pObject, L_INT nType, const L_VOID *pObjectDesc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECEXPLODEOBJECT)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETOBJECTPARALLELOGRAM)(const pVECTORHANDLE pVector, const pVECTOROBJECT pObject, pVECTORPOINT pMin, pVECTORPOINT pMax, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETOBJECTRECT)(const pVECTORHANDLE pVector, const pVECTOROBJECT pObject, RECT *pRect, L_UINT32 dwFlags);
typedef L_BOOL ( pWRPEXT_CALLBACK pL_VECISOBJECTINSIDEPARALLELOGRAM)(const pVECTORHANDLE pVector, const pVECTOROBJECT pObject, const pVECTORPOINT pMin, const pVECTORPOINT pMax, L_UINT32 dwFlags);
typedef L_BOOL ( pWRPEXT_CALLBACK pL_VECISOBJECTINSIDERECT)(const pVECTORHANDLE pVector, const pVECTOROBJECT pObject, const RECT *pRect, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSELECTOBJECT)(const pVECTORHANDLE pVector, pVECTOROBJECT pObject, L_BOOL bSelect);
typedef L_BOOL ( pWRPEXT_CALLBACK pL_VECISOBJECTSELECTED)(const pVECTORHANDLE pVector, const pVECTOROBJECT pObject);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECHIDEOBJECT)(const pVECTORHANDLE pVector, pVECTOROBJECT pObject, L_BOOL bHide);
typedef L_BOOL ( pWRPEXT_CALLBACK pL_VECISOBJECTHIDDEN)(const pVECTORHANDLE pVector, const pVECTOROBJECT pObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECENUMOBJECTS)(pVECTORHANDLE pVector, pVECTORENUMOBJECTSPROC pEnumProc, L_VOID *pUserData, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECENUMOBJECTSINLAYER)(pVECTORHANDLE pVector, const pVECTORLAYER pLayer, pVECTORENUMOBJECTSPROC pEnumProc, L_VOID *pUserData, L_UINT32 dwFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETOBJECTATTRIBUTES)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, const L_INT *pnROP, const pVECTORPEN pPen, const pVECTORBRUSH pBrush, const pVECTORFONT pFont, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETOBJECTATTRIBUTES)(const pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_INT *pnROP, pVECTORPEN pPen, pVECTORBRUSH pBrush, pVECTORFONT pFont);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECADDOBJECTTOGROUP)(pVECTORHANDLE pVector, const pVECTORGROUP pGroup, L_INT nType, const L_VOID *pObjectDesc, pVECTOROBJECT pNewObject);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECENUMOBJECTSINGROUP)(pVECTORHANDLE pVector, const pVECTORGROUP pGroup, pVECTORENUMOBJECTSPROC pEnumProc, L_VOID *pUserData, L_UINT32 dwFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETOBJECTTOOLTIP)(pVECTORHANDLE pVector, pVECTOROBJECT pObject, L_TCHAR *pszTooltip);
typedef L_UINT32 ( pWRPEXT_CALLBACK pL_VECGETOBJECTTOOLTIP)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_TCHAR *pBuffer, L_UINT32 uSize);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSHOWOBJECTTOOLTIP)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, pVECTORTOOLTIPDESC pTooltipDesc);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECHIDEOBJECTTOOLTIP)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETOBJECTVIEWCONTEXT)(pVECTORHANDLE pVector, pVECTOROBJECT pObject, const pVECTORPOINT pMin, const pVECTORPOINT pMax);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETOBJECTVIEWCONTEXT)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, pVECTORPOINT pMin, pVECTORPOINT pMax);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECREMOVEOBJECTVIEWCONTEXT)(pVECTORHANDLE pVector, pVECTOROBJECT pObject);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECADDHYPERLINK)(pVECTORHANDLE pVector, pVECTOROBJECT pObject, pVECTORLINKDESC pTarget);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETHYPERLINK)(pVECTORHANDLE pVector, pVECTOROBJECT pObject, L_UINT32 uIndex, pVECTORLINKDESC pTarget);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETHYPERLINK)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_UINT32 uIndex, pVECTORLINKDESC pTarget);
typedef L_UINT32 ( pWRPEXT_CALLBACK pL_VECGETHYPERLINKCOUNT)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGOTOHYPERLINK)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_UINT32 uIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETOBJECTDESCRIPTION)(pVECTORHANDLE pVector, pVECTOROBJECT pObject, L_TCHAR *pszTarget);
typedef L_UINT32 ( pWRPEXT_CALLBACK pL_VECGETOBJECTDESCRIPTION)(pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_TCHAR *pBuffer, L_UINT32 uSize);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Event functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETEVENTCALLBACK)(pVECTORHANDLE pVector, pVECTOREVENTPROC pProc, L_VOID *pUserData, ppVECTOREVENTPROC pOldProc, L_VOID **pOldUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECEVENT)(pVECTORHANDLE, pVECTOREVENT);


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Font Substitution functions.                                         []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
typedef L_INT ( pWRPEXT_CALLBACK pL_VECSETFONTMAPPER)(pVECTORHANDLE, pVECTORFONTMAPPERCALLBACK, L_VOID *);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECGETFONTMAPPER)(pVECTORHANDLE, pVECTORFONTMAPPERCALLBACK *);


//-----------------------------------------------------------------------------
//--LVDLG.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGROTATE)(HWND hWnd, pVECTORHANDLE pVector, pVECTORPOINT pRotation, const pVECTORPOINT pOrigin, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGSCALE)(HWND hWnd, pVECTORHANDLE pVector, pVECTORPOINT pScale, const pVECTORPOINT pOrigin, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGTRANSLATE)(HWND hWnd, pVECTORHANDLE pVector, pVECTORPOINT pTranslation, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGCAMERA)(HWND hWnd, pVECTORHANDLE pVector, pVECTORCAMERA pCamera, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGRENDER)(HWND hWnd, pVECTORHANDLE pVector, L_INT  *pnPolygonMode, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGVIEWMODE)(HWND hWnd, pVECTORHANDLE pVector, L_INT  *pnViewMode, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGHITTEST)(HWND hWnd, pVECTORHANDLE pVector, pVECTORHITTEST pHitTest, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGEDITALLLAYERS)(HWND hWnd, pVECTORHANDLE pVector, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGNEWLAYER)(HWND hWnd, pVECTORHANDLE pVector, pVECTORLAYERDESC pLayerDesc, pVECTORLAYER pLayer, L_BOOL  *pbActiveLayer, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGEDITLAYER)(HWND hWnd, pVECTORHANDLE pVector, pVECTORLAYER pLayer, pVECTORLAYERDESC pLayerDesc, L_BOOL  *pbActiveLayer, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGEDITALLGROUPS)(HWND hWnd, pVECTORHANDLE pVector, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGNEWGROUP)(HWND hWnd, pVECTORHANDLE pVector, pVECTORGROUPDESC pGroupDesc, pVECTORGROUP pGroup, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGEDITGROUP)(HWND hWnd, pVECTORHANDLE pVector, pVECTORGROUP pGroup, pVECTORGROUPDESC pGroupDesc, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGNEWOBJECT)(HWND hWnd, pVECTORHANDLE pVector, const pVECTORLAYER pLayer, L_INT nType, L_VOID  *pObjectDesc, pVECTOROBJECT, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGEDITOBJECT)(HWND hWnd, pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_INT nType, L_VOID  *pObjectDesc, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGOBJECTATTRIBUTES)(HWND hWnd, pVECTORHANDLE pVector, const pVECTOROBJECT pObject, L_BOOL *pbSelected, L_INT *pnROP, pVECTORPEN pPen, pVECTORBRUSH pBrush, pVECTORFONT pFont, L_UINT32 dwFlags, LVCOMMDLGHELPCB pfnCallback, L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGGETSTRINGLEN)(L_UINT32 uString, L_UINT  *puLen);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGGETSTRING)(L_UINT32 uString, L_TCHAR  *pszString);
typedef L_INT ( pWRPEXT_CALLBACK pL_VECDLGSETSTRING)(L_UINT32 uString, const L_TCHAR  *pszString);
typedef HFONT ( pWRPEXT_CALLBACK pL_VECDLGSETFONT)(HFONT hFont);

//-----------------------------------------------------------------------------
//--LTBAR.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_BARCODEREAD)(pBITMAPHANDLE   pBitmap,
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

typedef L_INT ( pWRPEXT_CALLBACK pL_BARCODEWRITE)(pBITMAPHANDLE     pBitmap,
                                                   pBARCODEDATA      pBarCodeData,
                                                   pBARCODECOLOR     pBarCodeColor,
                                                   L_UINT32          ulFlags,
                                                   pBARCODE1D        pBarCode1D,
                                                   pBARCODEWRITEPDF  pBarCodePDF,
                                                   pBARCODEWRITEDM   pBarCodeDM,
                                                   pBARCODEWRITEQR   pBarCodeQR,
                                                   LPRECT            lprcSize);

typedef L_VOID ( pWRPEXT_CALLBACK pL_BARCODEFREE)(pBARCODEDATA * ppBarCodeData);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_BARCODEISDUPLICATED)(pBARCODEDATA pBarCodeDataItem);

typedef L_INT ( pWRPEXT_CALLBACK pL_BARCODEGETDUPLICATED)(pBARCODEDATA pBarCodeDataItem);

typedef L_INT ( pWRPEXT_CALLBACK pL_BARCODEGETFIRSTDUPLICATED)(pBARCODEDATA pBarCodeData, L_INT nIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_BARCODEGETNEXTDUPLICATED)(pBARCODEDATA pBarCodeData, L_INT nCurIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_BARCODEINIT )(L_INT nMajorType);

typedef L_VOID ( pWRPEXT_CALLBACK pL_BARCODEEXIT )(L_VOID);

typedef L_INT ( pWRPEXT_CALLBACK pL_BARCODEVERSIONINFO)(pBARCODEVERSION pBarCodeVersion, L_UINT uStructSize);

//-----------------------------------------------------------------------------
//--LTAUT.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

// General functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTISVALID)(pAUTOMATIONHANDLE pAutomation);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTINIT)(ppAUTOMATIONHANDLE ppAutomation);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTCREATE)(pAUTOMATIONHANDLE pAutomation, AUTOMATIONMODE nMode, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTFREE)(pAUTOMATIONHANDLE pAutomation);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSETUNDOLEVEL)(pAUTOMATIONHANDLE pAutomation, L_UINT uLevel);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTGETUNDOLEVEL)(pAUTOMATIONHANDLE pAutomation, L_UINT *puLevel);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTCANUNDO)(pAUTOMATIONHANDLE pAutomation, L_BOOL *pfCanUndo);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTCANREDO)(pAUTOMATIONHANDLE pAutomation, L_BOOL *pfCanRedo);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTUNDO)(pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTREDO)(pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSETUNDOENABLED)(pAUTOMATIONHANDLE pAutomation, L_BOOL bEnabled);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTADDUNDONODE)(pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSELECT)(pAUTOMATIONHANDLE pAutomation, AUTOMATIONSELECT nSelect, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTCLIPBOARDDATAREADY)(pAUTOMATIONHANDLE pAutomation, L_BOOL *pfReady);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTCUT)(pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTCOPY)(pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTPASTE)(pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTDELETE)(pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTPRINT)(pAUTOMATIONHANDLE pAutomation, L_UINT32 dwFlags);

// Container Functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTADDCONTAINER)(pAUTOMATIONHANDLE pAutomation, pCONTAINERHANDLE pContainer, L_VOID *pModeData);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTREMOVECONTAINER)(pAUTOMATIONHANDLE pAutomation, pCONTAINERHANDLE pContainer);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSETACTIVECONTAINER)(pAUTOMATIONHANDLE pAutomation, pCONTAINERHANDLE pContainer);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTGETACTIVECONTAINER)(pAUTOMATIONHANDLE pAutomation, pCONTAINERHANDLE *ppContainer);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTENUMCONTAINERS)(pAUTOMATIONHANDLE pAutomation, pAUTOMATIONENUMCONTAINERPROC pEnumProc, L_VOID *pUserData);

// Painting Functionts.
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSETPAINTPROPERTY)(pAUTOMATIONHANDLE pAutomation, PAINTGROUP nGroup, const L_VOID *pProperty);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTGETPAINTPROPERTY)(pAUTOMATIONHANDLE pAutomation, PAINTGROUP nGroup, L_VOID *pProperty);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSETPAINTBKCOLOR)(pAUTOMATIONHANDLE pAutomation, COLORREF rcBKColor);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTGETPAINTBKCOLOR)(pAUTOMATIONHANDLE pAutomation, COLORREF *prcBKColor);

// Vector Functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSETVECTORPROPERTY)(pAUTOMATIONHANDLE pAutomation, const pAUTOMATIONVECTORPROPERTIES pAutomationVectorprOperties);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTGETVECTORPROPERTY)(const pAUTOMATIONHANDLE pAutomation, pAUTOMATIONVECTORPROPERTIES pAutomationVectorprOperties);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTEDITVECTOROBJECT)(pAUTOMATIONHANDLE pAutomation, const pVECTOROBJECT pVectorObject);

//Toolbar Functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSETTOOLBAR)(pAUTOMATIONHANDLE pAutomation, pTOOLBARHANDLE pToolbar);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTGETTOOLBAR)(pAUTOMATIONHANDLE pAutomation, pTOOLBARHANDLE *ppToolbar);

typedef L_INT ( pWRPEXT_CALLBACK pL_AUTSETCURRENTTOOL)(pAUTOMATIONHANDLE pAutomation, L_INT nTool);
typedef L_INT ( pWRPEXT_CALLBACK pL_AUTGETCURRENTTOOL)(pAUTOMATIONHANDLE pAutomation, L_INT *pnTool);

//-----------------------------------------------------------------------------
//--LTCON.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
 // general container operations funtions.
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERISVALID)(pCONTAINERHANDLE pContainer);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERINIT)(ppCONTAINERHANDLE ppContainer);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERCREATE)(pCONTAINERHANDLE pContainer, HWND hwndOwner);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERFREE)(pCONTAINERHANDLE pContainer);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERUPDATE)(pCONTAINERHANDLE pContainer, LPRECT prcPaint);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERRESET)(pCONTAINERHANDLE pContainer);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINEREDITOBJECT)(pCONTAINERHANDLE pContainer, const pCONTAINEROBJECTDATA pObjectData);
 
 // setting functions.
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETOWNER)(pCONTAINERHANDLE pContainer, HWND hWndOwner);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETMETRICS)(pCONTAINERHANDLE pContainer, pCONTAINERMETRICS pMetrics);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETOFFSET)(pCONTAINERHANDLE pContainer, L_INT nXOffset, L_INT nYOffset, L_INT nZOffset);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETSCALAR)(pCONTAINERHANDLE pContainer, pVECTORPOINT pvptScalarNum, pVECTORPOINT pvptScalarDen);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETOBJECTTYPE)(pCONTAINERHANDLE pContainer, CONTAINEROBJECTTYPE nObjectType);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETOBJECTCURSOR)(pCONTAINERHANDLE pContainer, CONTAINEROBJECTTYPE nObjectType, HCURSOR hCursor);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETENABLED)(pCONTAINERHANDLE pContainer, L_BOOL fEnable);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETCALLBACK)(pCONTAINERHANDLE pContainer, const pCONTAINERCALLBACK pCallback, L_VOID *pUserData);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETOWNERDRAW)(pCONTAINERHANDLE pContainer, L_BOOL fOwnerDraw, L_UINT32 dwFlags);
 
 // getting functions.
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERGETOWNER)(pCONTAINERHANDLE pContainer, HWND* phwndOwner);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERGETMETRICS)(pCONTAINERHANDLE pContainer, pCONTAINERMETRICS pMetrics);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERGETOFFSET)(pCONTAINERHANDLE pContainer, L_INT *pnXOffset, L_INT *pnYOffset, L_INT *pnZOffset);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERGETSCALAR)(pCONTAINERHANDLE pContainer, pVECTORPOINT pvptScalarNum, pVECTORPOINT pvptScalarDen);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERGETOBJECTTYPE)(pCONTAINERHANDLE pContainer, pCONTAINEROBJECTTYPE pnObjectType);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERGETOBJECTCURSOR)(pCONTAINERHANDLE pContainer, CONTAINEROBJECTTYPE nObjectType, HCURSOR* phCursor);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERGETCALLBACK)(pCONTAINERHANDLE pContainer, pCONTAINERCALLBACK *ppCallback, L_VOID **ppUserData);
 
 // status query functions.
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERISENABLED)(pCONTAINERHANDLE pContainer, L_BOOL* pfEnabled);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERISOWNERDRAW)(pCONTAINERHANDLE pContainer, L_BOOL* pfOwnerDraw);
 
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERSETAUTOMATIONCALLBACK)(pCONTAINERHANDLE pContainer, const pCONTAINERCALLBACK pAutomationCallback, L_VOID *pAutomationData);
 typedef L_INT ( pWRPEXT_CALLBACK pL_SCREENTOCONTAINER)(pCONTAINERHANDLE pContainer, LPPOINT pptPoint);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERTOSCREEN)(pCONTAINERHANDLE pContainer, LPPOINT pptPoint);
 typedef L_INT ( pWRPEXT_CALLBACK pL_CONTAINERENABLEUPDATE)(pCONTAINERHANDLE pContainer, L_BOOL fEnableUpdate);

//-----------------------------------------------------------------------------
//--LTPNT.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTISVALID)(pPAINTHANDLE pPaint);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTINIT)(ppPAINTHANDLE ppPaint);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTFREE)(pPAINTHANDLE pPaint);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTSETPROPERTY)(pPAINTHANDLE pPaint, PAINTGROUP nGroup, const L_VOID *pProperty);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTGETPROPERTY)(pPAINTHANDLE pPaint, PAINTGROUP nGroup, const L_VOID *pProperty);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTSETMETRICS)(pPAINTHANDLE pPaint, HDC UserDC, pBITMAPHANDLE pBitmap, HPALETTE hRestrictionPalette);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTSETTRANSFORMATION)(pPAINTHANDLE pPaint, pPAINTXFORM pXForm);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTGETTRANSFORMATION)(pPAINTHANDLE pPaint, pPAINTXFORM pXForm);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTSETDCEXTENTS)(pPAINTHANDLE pPaint, const LPRECT prcRect);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTGETDCEXTENTS)(pPAINTHANDLE pPaint, LPRECT prcRect);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTSETCLIPRGN)(pPAINTHANDLE pPaint, HRGN hClipRng);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTGETCLIPRGN)(pPAINTHANDLE pPaint, pHRGN phClipRng);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTOFFSETCLIPRGN)(pPAINTHANDLE pPaint, L_INT nDX, L_INT nDY);

// brush fucntions.
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTBRUSHMOVETO)(pPAINTHANDLE pPaint, HDC UserDC, L_INT nX, L_INT nY);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTBRUSHLINETO)(pPAINTHANDLE pPaint, HDC UserDC, L_INT nX, L_INT nY);

// shape functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDRAWSHAPELINE)(pPAINTHANDLE pPaint, HDC UserDC, L_INT nXStart, L_INT nYStart, L_INT nEndX, L_INT nEndY);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDRAWSHAPERECTANGLE)(pPAINTHANDLE pPaint, HDC UserDC, const LPRECT prcRect);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDRAWSHAPEROUNDRECT)(pPAINTHANDLE pPaint, HDC UserDC, const LPRECT prcRect);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDRAWSHAPEELLIPSE)(pPAINTHANDLE pPaint, HDC UserDC, const LPRECT prcRect);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDRAWSHAPEPOLYGON)(pPAINTHANDLE pPaint, HDC UserDC, const LPPOINT pptPoints, L_INT nCount);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDRAWSHAPEPOLYBEZIER)(pPAINTHANDLE pPaint, HDC UserDC, const LPPOINT pptPoints, L_INT nCount);

// region functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONRECT)(pPAINTHANDLE pPaint, HDC UserDC, const LPRECT prcRect, pHRGN phDestRgn);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONROUNDRECT)(pPAINTHANDLE pPaint, HDC UserDC, const LPRECT prcRect, pHRGN phDestRgn);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONELLIPSE)(pPAINTHANDLE pPaint, HDC UserDC, const LPRECT prcRect, pHRGN phDestRgn);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONPOLYGON)(pPAINTHANDLE pPaint, HDC UserDC, const LPPOINT pptPoints, L_INT nCount, pHRGN phDestRgn);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONSURFACE)(pPAINTHANDLE pPaint, HDC UserDC, const LPPOINT pptPoint, pHRGN phDestRgn);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONBORDER)(pPAINTHANDLE pPaint, HDC UserDC, const LPPOINT pptPoint, const COLORREF crBorderColor, pHRGN phDestRgn);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONCOLOR)(pPAINTHANDLE pPaint, HDC UserDC, const COLORREF crColor, pHRGN phDestRgn);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONTRANSLATE)(pPAINTHANDLE pPaint, L_INT dx, L_INT dy, pHRGN phDestRgn);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTREGIONSCALE)(pPAINTHANDLE pPaint, L_INT nHScaleFactor, L_INT nVScaleFactor, PAINTALIGNMENT nAlignment, pHRGN phDestRgn);

// fill functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTFILLSURFACE)(pPAINTHANDLE pPaint, HDC UserDC, const LPPOINT pptPoint);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTFILLBORDER)(pPAINTHANDLE pPaint, HDC UserDC, const LPPOINT pptPoint, const COLORREF crBorderColor);
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTFILLCOLORREPLACE)(pPAINTHANDLE pPaint, HDC UserDC, const COLORREF crColor);

// text functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTAPPLYTEXT)(pPAINTHANDLE pPaint, HDC UserDC, const LPRECT prcRect);

// paint helping functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTPICKCOLOR)(pPAINTHANDLE pPaint, HDC UserDC, L_INT nX, L_INT nY, COLORREF* pcrDestColor);

typedef L_INT ( pWRPEXT_CALLBACK pL_PNTSETCALLBACK)(pPAINTHANDLE pPaint, pPAINTCALLBACK pCallback, L_VOID *pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_PNTUPDATELEADDC)(pPAINTHANDLE pPaint, 
                                                   pBITMAPHANDLE pBitmap);

//-----------------------------------------------------------------------------
//--LTTLB.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

// general toolbar operations funtions.
typedef L_INT ( pWRPEXT_CALLBACK pL_TBISVALID)(pTOOLBARHANDLE pToolbar);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBINIT)(ppTOOLBARHANDLE ppToolbar);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBFREE)(pTOOLBARHANDLE pToolbar);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBCREATE)(pTOOLBARHANDLE pToolbar, HWND hwndParent, L_TCHAR * szTitle, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBFREETOOLBARINFO)(pTOOLBARHANDLE pToolbar, pLTOOLBARINFO pToolbarInfo);

// status query functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_TBISVISIBLE)(pTOOLBARHANDLE pToolbar, L_BOOL *pfVisible);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBISBUTTONENABLED)(pTOOLBARHANDLE pToolbar, L_UINT uButtonID, L_BOOL *pfEnable);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBISBUTTONVISIBLE)(pTOOLBARHANDLE pToolbar, L_UINT uButtonID, L_BOOL *pfVisible);

// setting functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETVISIBLE)(pTOOLBARHANDLE pToolbar, L_BOOL fVisible);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETPOSITION)(pTOOLBARHANDLE pToolbar, LPPOINT lpptPos, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETROWS)(pTOOLBARHANDLE pToolbar, L_INT nRows);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETBUTTONCHECKED)(pTOOLBARHANDLE pToolbar, L_UINT uButtonID);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETBUTTONENABLED)(pTOOLBARHANDLE pToolbar, L_UINT uButtonID, L_BOOL fEnable);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETBUTTONVISIBLE)(pTOOLBARHANDLE pToolbar, L_UINT uButtonID, L_BOOL fVisible);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETTOOLBARINFO)(pTOOLBARHANDLE pToolbar, pLTOOLBARINFO pToolbarInfo);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETCALLBACK)(pTOOLBARHANDLE pToolbar, const pTOOLBARCALLBACK pCallback, L_VOID *pUserData);

// getting functions.
typedef L_INT ( pWRPEXT_CALLBACK pL_TBGETPOSITION)(pTOOLBARHANDLE pToolbar, LPPOINT lpptPos, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBGETROWS)(pTOOLBARHANDLE pToolbar, L_INT *pnRows);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBGETBUTTONCHECKED)(pTOOLBARHANDLE pToolbar, L_INT *pnChecked);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBGETTOOLBARINFO)(pTOOLBARHANDLE pToolbar, pLTOOLBARINFO pToolbarInfo, L_UINT uStructSize);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBGETCALLBACK)(pTOOLBARHANDLE pToolbar, pTOOLBARCALLBACK *ppCallback, L_VOID **ppUserData);

// new functions
typedef L_INT ( pWRPEXT_CALLBACK pL_TBADDBUTTON)(pTOOLBARHANDLE pToolbar, L_UINT uButtonRefId, const pLBUTTONINFO pButtonInfo, L_UINT32 dwFlags);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBREMOVEBUTTON)(pTOOLBARHANDLE pToolbar, L_UINT uButtonId);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBGETBUTTONINFO)(const pTOOLBARHANDLE pToolbar, L_UINT uButtonId, pLBUTTONINFO pButtonInfo, L_UINT uStructSize);
typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETBUTTONINFO)(pTOOLBARHANDLE pToolbar, L_UINT uButtonId, const pLBUTTONINFO pButtonInfo);

typedef L_INT ( pWRPEXT_CALLBACK pL_TBSETAUTOMATIONCALLBACK)(pTOOLBARHANDLE pToolbar, const pTOOLBARCALLBACK pAutomationCallback, L_VOID *pAutomationData);

//-----------------------------------------------------------------------------
//--LTPDG.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDLGBRUSH )( HWND hWnd, pPAINTDLGBRUSHINFO  pBrushDlgInfo ) ;
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDLGSHAPE )( HWND hWnd, pPAINTDLGSHAPEINFO  pShapeDlgInfo ) ;
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDLGREGION)( HWND hWnd, pPAINTDLGREGIONINFO pRegionDlgInfo) ;
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDLGFILL  )( HWND hWnd, pPAINTDLGFILLINFO   pFillDlgInfo  ) ;
typedef L_INT ( pWRPEXT_CALLBACK pL_PNTDLGTEXT  )( HWND hWnd, pPAINTDLGTEXTINFO   pTextDlgInfo  ) ;

//-----------------------------------------------------------------------------
//--LTSGM.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
// Initialize segmentation handle
typedef L_INT ( pWRPEXT_CALLBACK pL_MRCSTARTBITMAPSEGMENTATION)(pHSEGMENTATION phSegment,
                                                            pBITMAPHANDLE  pBitmap,
                                                            COLORREF       clrBackground,
                                                            COLORREF       clrForeground);
// Free segmentation handle
typedef L_INT ( pWRPEXT_CALLBACK pL_MRCSTOPBITMAPSEGMENTATION)(HSEGMENTATION hSegment);


// Break a bitmap into segments in other way
typedef L_INT ( pWRPEXT_CALLBACK pL_MRCSEGMENTBITMAP)(HSEGMENTATION              hSegment,
                                                     pBITMAPHANDLE              pBitmap,
                                                     pSEGMENTEXTOPTIONS         pSegOption);

// Set a new segment in the segmentation handle 
typedef L_INT ( pWRPEXT_CALLBACK pL_MRCCREATENEWSEGMENT)(HSEGMENTATION   hSegment,
                                                     pBITMAPHANDLE   pBitmap,
                                                     pSEGMENTDATA    pSegment);

// Get all segments stored inside a segmentation handle
typedef L_INT ( pWRPEXT_CALLBACK pL_MRCENUMSEGMENTS)(HSEGMENTATION              hSegment,
                                                 pMRCENUMSEGMENTSPROC      pEnumProc,
                                                 L_HANDLE                  pUserData,
                                                 L_UINT32                  dwFlags);

// Update a certain segment in the segmentation handle 
typedef L_INT ( pWRPEXT_CALLBACK pL_MRCSETSEGMENTDATA)(HSEGMENTATION  hSegment,
                                                   pBITMAPHANDLE  pBitmap, 
                                                   L_INT          nSegId,
                                                   pSEGMENTDATA   pSegmentData);

typedef L_INT ( pWRPEXT_CALLBACK pL_MRCDELETESEGMENT)(HSEGMENTATION  hSegment,
                                                  L_INT          nSegId);


typedef L_INT ( pWRPEXT_CALLBACK pL_MRCCOMBINESEGMENTS)(HSEGMENTATION  hSegment,
                                                    L_INT          nSegId1,
                                                    L_INT          nSegId2,
                                                    L_UINT16       uCombineFlags,
                                                    L_UINT16       uCombineFactor);

/*------------Copy segmentation handle ---------------------------------------------*/
typedef L_INT ( pWRPEXT_CALLBACK pL_MRCCOPYSEGMENTATIONHANDLE)(pHSEGMENTATION phSegmentDst,
                                                           HSEGMENTATION  hSegmentSrc);

/*------------Save a file as MRC ---------------------------------------------------*/

typedef L_INT ( pWRPEXT_CALLBACK pL_MRCSAVEBITMAP)(HSEGMENTATION         hSegment,
                                              pBITMAPHANDLE         pBitmap,
                                              pCOMPRESSIONOPTIONS   pCmpOption,
                                              L_TCHAR*              pszFileName,
                                              FILESAVECALLBACK      pfnCallback,
                                              L_HANDLE              pUserData,
                                              L_INT                 nFormat,
                                              pSAVEFILEOPTION       pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_MRCSAVEBITMAPT44)(
                                                  HSEGMENTATION           hSegment,
                                                  pBITMAPHANDLE           pBitmap,
                                                  pCOMPRESSIONOPTIONS     pCmpOption,
                                                  L_TCHAR*                pszFileName,
                                                  FILESAVECALLBACK        pfnCallback,
                                                  L_HANDLE                pUserData,
                                                  L_INT                   nFormat,
                                                  pSAVEFILEOPTION         pSaveOptions);

/*------------Load an MRC file -----------------------------------------------------*/

typedef L_INT ( pWRPEXT_CALLBACK pL_MRCLOADBITMAP)(
                                               L_TCHAR*          pszFileName,
                                               pBITMAPHANDLE     pBitmap,
                                               L_UINT            uStructSize,
                                               L_INT             nPageNo,
                                               FILEREADCALLBACK  pfnCallback,
                                               L_HANDLE          pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_MRCGETPAGESCOUNT)( L_TCHAR* pszFileName,
                                                   L_INT* pnPages);


/*------------Load/Save segments to/from files -------------------------------------*/
typedef L_INT ( pWRPEXT_CALLBACK pL_MRCSAVESEGMENTATION)(HSEGMENTATION hSegment,
                                                 L_TCHAR* pszFileName);

typedef L_INT ( pWRPEXT_CALLBACK pL_MRCLOADSEGMENTATION)(pHSEGMENTATION phSegment,
                                                 pBITMAPHANDLE  pBitmap,
                                                 L_TCHAR* pszFileName);

typedef L_INT ( pWRPEXT_CALLBACK pL_MRCSAVEBITMAPLIST)(
                                                   HSEGMENTATION  * hSegment,
                                                   L_UINT                uhSegmentCount,
                                                   HBITMAPLIST           hList,
                                                   pCOMPRESSIONOPTIONS   pCmpOption,
                                                   L_TCHAR*              pszFileName,
                                                   L_INT                 nFormat);

//-----------------------------------------------------------------------------
//--LTZMV.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_BOOL ( pWRPEXT_CALLBACK pL_WINDOWHASZOOMVIEW)(HWND hWnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEZOOMVIEW)(HWND hWnd,
                                    pBITMAPHANDLE pBitmap,
                                    pZOOMVIEWPROPS pZoomViewProps);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETZOOMVIEWPROPS)(HWND hWnd,
                                      pZOOMVIEWPROPS pZoomViewProps,
                                      L_UINT32 uStructSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_UPDATEZOOMVIEW)(HWND hWnd,
                                    pZOOMVIEWPROPS pZoomViewProps);

typedef L_INT ( pWRPEXT_CALLBACK pL_DESTROYZOOMVIEW)(HWND hWnd, L_UINT uIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETZOOMVIEWSCOUNT)(HWND hWnd, L_UINT *puCount);

typedef L_INT ( pWRPEXT_CALLBACK pL_RENDERZOOMVIEW)(HDC hDC, HWND hWnd);

typedef L_INT ( pWRPEXT_CALLBACK pL_STARTZOOMVIEWANNEDIT)(HWND hWnd, pZOOMVIEWANNEDIT pZoomViewAnnEdit);

typedef L_INT ( pWRPEXT_CALLBACK pL_STOPZOOMVIEWANNEDIT)(HWND hWnd);

//-----------------------------------------------------------------------------
//--LTIMGOPT.H FUNCTIONS PROTOTYPES--------------------------------------------
//-----------------------------------------------------------------------------
   typedef L_INT ( * pL_OPTGETDEFAULTOPTIONS)(
                  pOPTIMIZEIMAGEOPTIONS pOptImgOptions,
                  L_UINT uStructSize);

   typedef L_INT ( * pL_OPTOPTIMIZEBUFFER)(
                  L_UCHAR *         pOrgImgBuffer,
                  L_SIZE_T          uOrgImgBufferSize,
                  HGLOBAL *         phOptImgBuffer,
                  L_SIZE_T *        puOptImgBufferSize,
                  pOPTIMIZEIMAGEOPTIONS   pOptImgOptions,
                  OPTIMIZEBUFFERCALLBACK  pfnOptBufferCB,
                  L_VOID *          pUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_OPTOPTIMIZEDIR)(
                  L_TCHAR *                  pszOrgDirPath, 
                  L_TCHAR *                  pszOptDirPath, 
                  pOPTIMIZEIMAGEOPTIONS      pOptImgOptions,
                  L_TCHAR *                  pszFilesExt, 
                  L_BOOL                     bIncludeSubDirs, 
                  OPTIMIZEIMAGEDIRCALLBACK   pfnOptImgDirCB, 
                  L_VOID *                   pUserData);

//-----------------------------------------------------------------------------
//--LPDFComp.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_PDFCOMPINIT)( LCPDF_HANDLE *phDocument, pPDFCOMP_IMAGECALLBACK pCallback, HANDLE pUserData );
typedef L_VOID ( pWRPEXT_CALLBACK pL_PDFCOMPFREE)( LCPDF_HANDLE hDocument );

typedef L_INT ( pWRPEXT_CALLBACK pL_PDFCOMPWRITE)( LCPDF_HANDLE hDocument, L_TCHAR  *pwszOutFile );

typedef L_INT ( pWRPEXT_CALLBACK pL_PDFCOMPSETCOMPRESSION)( LCPDF_HANDLE hDocument, LPPDFCOMPRESSION pCompression );

typedef L_INT ( pWRPEXT_CALLBACK pL_PDFCOMPINSERTMRC)( LCPDF_HANDLE hDocHandle, pBITMAPHANDLE pBitmap, LPPDFCOMPOPTIONS pPDFOptions );
typedef L_INT ( pWRPEXT_CALLBACK pL_PDFCOMPINSERTNORMAL)( LCPDF_HANDLE hDocHandle, pBITMAPHANDLE pBitmap );
typedef L_INT ( pWRPEXT_CALLBACK pL_PDFCOMPINSERTSEGMENTS)( LCPDF_HANDLE hDocHandle, pBITMAPHANDLE pBitmap, L_UINT uSegmentCnt, LPSEGMENTINFO pSegmentInfo, L_BOOL bIsThereBackGround, COLORREF rgbBackGroundColor );

//-----------------------------------------------------------------------------
//--LTIVW.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
typedef  HDISPCONTAINER ( pWRPEXT_CALLBACK pL_DISPCONTAINERCREATE)      (HWND                  hWndParent,
                                                                     LPRECT                lpRect,
                                                                     L_UINT                uFlags);

typedef  HDISPCONTAINER ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETHANDLE)   (HWND hConWnd);

typedef  HWND ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETWINDOWHANDLE)       (HDISPCONTAINER hCon,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERDESTROY)              (HDISPCONTAINER hCon, 
                                                                     L_BOOL         bCleanImages,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETPROPERTIES)        (HDISPCONTAINER            hCon,
                                                                     pDISPCONTAINERPROPERTIES  pDispContainerProp,
                                                                     L_UINT                    uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETPROPERTIES)        (HDISPCONTAINER            hCon,
                                                                     pDISPCONTAINERPROPERTIES pDispContainerProp,
                                                                     L_UINT                    uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERINSERTCELL)           (HDISPCONTAINER hCon, 
                                                                     L_INT          nCellIndex, 
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERREMOVECELL)           (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_BOOL         bCleanImage, 
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLCOUNT)         (HDISPCONTAINER hCon,
                                                                     L_UINT         uFlags);

typedef  HWND ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLWINDOWHANDLE)   (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETCELLBITMAPLIST)    (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     HBITMAPLIST    hBitmapList,
                                                                     L_BOOL         bCleanImage, 
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERADDACTION)            (HDISPCONTAINER hCon,
                                                                     L_INT          nAction,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETACTION)           (HDISPCONTAINER hCon,
                                                                     L_INT          nAction,
                                                                     L_INT          nMouseButton,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETCELLTAG)           (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_UINT         uRow,
                                                                     L_UINT         uAlign,
                                                                     L_UINT         uType,
                                                                     LPTSTR          pString,
                                                                     L_UINT         uFlags);


typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETCELLPROPERTIES)    (HDISPCONTAINER       hCon,
                                                                     L_INT                nCellIndex,
                                                                     pDISPCELLPROPERTIES  pCellProperties,
                                                                     L_UINT               uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLPROPERTIES)    (HDISPCONTAINER       hCon,
                                                                     L_INT                nCellIndex,
                                                                     pDISPCELLPROPERTIES  pCellProperties,
                                                                     L_UINT               uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLPOSITION)      (HDISPCONTAINER hCon,
                                                                      L_INT          nCellIndex,
                                                                      L_UINT * puRow,
                                                                      L_UINT * puCol,
                                                                      L_UINT         uFlags);
                                                   
typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERREPOSITIONCELL)       (HDISPCONTAINER hCon,
                                                                      L_INT          nCellIndex,
                                                                      L_INT          nTargetIndex,
                                                                      L_BOOL         bSwap,
                                                                      L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLBITMAPLIST)    (HDISPCONTAINER hCon,
                                                                      L_INT          nCellIndex,
                                                                      pHBITMAPLIST   phBitmapList,
                                                                      L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLBOUNDS)        (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     LPRECT         lpRect,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERFREEZECELL)           (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_BOOL         bFreeze,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETFIRSTVISIBLEROW)   (HDISPCONTAINER hCon,
                                                                     L_UINT         uRow,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETFIRSTVISIBLEROW)   (HDISPCONTAINER hCon,
                                                                     L_UINT * uRow,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETACTIONPROPERTIES)  (HDISPCONTAINER hCon,
                                                                     L_INT          nAction,
                                                                     L_INT          nCellIndex,
                                                                     L_INT          nSubCellIndex,
                                                                     L_VOID *       pActionProperties,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETACTIONPROPERTIES)  (HDISPCONTAINER hCon,
                                                                     L_INT          nAction,
                                                                     L_INT          nCellIndex,
                                                                     L_INT          nSubCellIndex,
                                                                     L_VOID *       pActionProperties,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERREMOVEACTION)         (HDISPCONTAINER hCon,
                                                                     L_INT          nAction,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETACTIONCOUNT)       (HDISPCONTAINER hCon,
                                                                     L_INT *  nCount,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETKEYBOARDACTION)    (HDISPCONTAINER hCon,
                                                                     L_INT          nAction,
                                                                     L_INT          nButton, 
                                                                     L_UINT         uKey,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETBOUNDS)            (HDISPCONTAINER hCon,
                                                                     RECT *   lpRect,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETBOUNDS)            (HDISPCONTAINER hCon,
                                                                     LPRECT         lpRect,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSELECTCELL)           (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_BOOL         bSelect,
                                                                     L_UINT         uFlags);

typedef  L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISCELLSELECTED)      (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETTAGCALLBACK)       (HDISPCONTAINER           hCon,
                                                                     DISPCONTAINERTAGCALLBACK pfnCallBack,
                                                                     L_VOID         *   pUserData);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETACTIONCALLBACK)    (HDISPCONTAINER              hCon,
                                                                     DISPCONTAINERACTIONCALLBACK pfnCallBack,
                                                                     L_VOID         *      pUserData);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETTAGCALLBACK)       (HDISPCONTAINER                   hCon,
                                                                     DISPCONTAINERTAGCALLBACK * ppfnCallBack,
                                                                     LPVOID *                   ppUserData);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETACTIONCALLBACK)    (HDISPCONTAINER                      hCon,
                                                                     DISPCONTAINERACTIONCALLBACK * ppfnCallBack,
                                                                     LPVOID                      * ppUserData);

typedef  L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISACTIONACTIVE)      (HDISPCONTAINER hCon,
                                                                     L_INT          nAction,
                                                                     L_UINT         uFlags);

typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETKEYBOARDACTION)   (HDISPCONTAINER hCon,
                                                                     L_INT          nAction,
                                                                     L_INT          nMouseDirection,
                                                                     L_UINT * puVk,
                                                                     L_UINT * puModifiers,
                                                                     L_UINT         uFlags);

typedef  L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISCELLFROZEN)       (HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETPAINTCALLBACK   )(HDISPCONTAINER              hCon,
                                                                       DISPCONTAINERPAINTCALLBACK  pfnPaintCallBack,
                                                                       L_VOID         *      pUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETPAINTCALLBACK   )(HDISPCONTAINER                      hCon,
                                                                       DISPCONTAINERPAINTCALLBACK  * ppfnPaintCallBack,
                                                                       LPVOID                      * ppUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERREPAINTCELL        )(HDISPCONTAINER hCon,
                                                                       HWND           hWnd,
                                                                       L_INT          nCellIndex,
                                                                       L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETBITMAPHANDLE    )(HDISPCONTAINER hCon,
                                                                       L_INT          nCellIndex,
                                                                       L_INT          nSubCellIndex,
                                                                       pBITMAPHANDLE  pBitmap,
                                                                       L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETBITMAPHANDLE    )(HDISPCONTAINER hCon,
                                                                       L_INT          nCellIndex,
                                                                       L_INT          nSubCellIndex,
                                                                       pBITMAPHANDLE  pBitmap,
                                                                       L_BOOL         bRepaint,
                                                                       L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETRULERUNIT       )(HDISPCONTAINER hCon,
                                                                       L_UINT         uUnit,
                                                                       L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERCALIBRATERULER     )(HDISPCONTAINER hCon,
                                                                       L_INT          nCellIndex,
                                                                       L_INT          nSubCellIndex,
                                                                       L_DOUBLE       dLength,
                                                                       L_UINT         uUnit,
                                                                       L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETACTIONBUTTON   )(HDISPCONTAINER hCon,
                                                                       L_INT          nAction,
                                                                       L_INT  * pnMouseButton,
                                                                       L_UINT * puFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERANNTORGN          )(HDISPCONTAINER hCon,
                                                                       L_INT          nCellIndex,
                                                                       L_INT          nSubCellIndex,
                                                                       L_UINT         uCombineMode,
                                                                       L_BOOL         bDeleteAnn,
                                                                       L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERISBUTTONVALID     )(HDISPCONTAINER hCon,
                                                                       L_INT          nAction,
                                                                       L_INT          nMouseButton,
                                                                       L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSTARTANIMATION    )(HDISPCONTAINER hCon,
                                                                       L_INT          nCellIndex,
                                                                       L_INT          nStartFrame,
                                                                       L_INT          nFrameCount,
                                                                       L_BOOL         bAnimateAllSubCells,
                                                                       L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETANIMATIONPROPERTIES )(HDISPCONTAINER       hCon,
                                                                            L_INT                nCellIndex,
                                                                            pDISPANIMATIONPROPS  pDisAnimationProps,
                                                                            L_UINT               uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETANIMATIONPROPERTIES )(HDISPCONTAINER       hCon,
                                                                            L_INT                nCellIndex,
                                                                            pDISPANIMATIONPROPS  pDisAnimationProps,
                                                                            L_UINT               uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSTOPANIMATION          )(HDISPCONTAINER hCon,
                                                                            L_INT          nCellIndex,
                                                                            L_UINT         uFlags);



   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSHOWTITLEBAR           )(HDISPCONTAINER hCon,
                                                                            L_UINT         uShow,
                                                                            L_UINT         uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETTITLEBARPROPERTIES  )(HDISPCONTAINER              hCon,
                                                                            pDISPCONTAINERTITLEBARPROPS pDispContainerTitlebarProps,
                                                                            L_UINT                      uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETTITLEBARPROPERTIES  )(HDISPCONTAINER              hCon,
                                                                            pDISPCONTAINERTITLEBARPROPS pDispContainerTitlebarProps,
                                                                            L_UINT                      uFlags);


   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETICONPROPERTIES      )(HDISPCONTAINER                  hCon,
                                                                            L_INT                           nIconIndex,
                                                                            pDISPCONTAINERTITLEBARICONPROPS pDispContainerIconProps,
                                                                            L_UINT                          uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETICONPROPERTIES      )(HDISPCONTAINER                  hCon,
                                                                            L_INT                           nIconIndex,
                                                                            pDISPCONTAINERTITLEBARICONPROPS pDispContainerIconProps,
                                                                            L_UINT                          uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERCHECKTITLEBARICON      )(HDISPCONTAINER hCon,
                                                                            L_INT          nCellIndex,
                                                                            L_INT          nSubCellIndex,
                                                                            L_INT          nIconIndex,
                                                                            L_BOOL         bCheck,
                                                                            L_UINT         uFlags);

   typedef L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISTITLEBARICONCHECKED  )(HDISPCONTAINER hCon,
                                                                            L_INT          nCellIndex,
                                                                            L_INT          nSubCellIndex,
                                                                            L_INT          nIconIndex,
                                                                            L_UINT         uFlags);

   typedef L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISCELLANIMATED        )(HDISPCONTAINER hCon,
                                                                            L_INT          nCellIndex,
                                                                            L_UINT         uFlags);

                  // This function retrieves the viewer ruler unit
   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETRULERUNIT           )(HDISPCONTAINER hCon, 
                                                                            L_UINT * puUnit,
                                                                            L_UINT         uFlags);

   typedef L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISTITLEBARENABLED     )(HDISPCONTAINER hCon,
                                                                            L_UINT         uFlags);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETSELECTEDANNOTATIONATTRIBUTES )(HDISPCONTAINER           hCon,
                                                                                       L_INT                    nCellIndex,
                                                                                       L_INT                    nSubCellIndex,
                                                                                       pDISPCONTAINERANNATTRIBS pAnnAttributes,
                                                                                       L_UINT                   uFlags);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERHANDLEPALETTE )   (HDISPCONTAINER hCon,
                                                                        L_UINT         uMessage,
                                                                        WPARAM         wParam,
                                                                        L_UINT         uFlags);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERUPDATECELLVIEW )  (HDISPCONTAINER hCon,
                                                                        HWND           hWnd,
                                                                        L_INT          nCellIndex,
                                                                        L_UINT         uFlags);

   //New Medical Viewer Functionalities
   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERROTATEANNOTATIONCONTAINER )(HDISPCONTAINER hCon,
                                                                                 L_INT          nCellIndex,
                                                                                 L_INT          nSubCellIndex,
                                                                                 L_DOUBLE       dAngle,
                                                                                 L_UINT         uFlags);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETANNOTATIONCALLBACK )(HDISPCONTAINER                  hCon,
                                                                             DISPCONTAINERANNOTATIONCALLBACK pfnCallBack,
                                                                             L_VOID *                        pUserData);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETANNOTATIONCALLBACK )(HDISPCONTAINER                    hCon,
                                                                             DISPCONTAINERANNOTATIONCALLBACK * ppfnCallBack,
                                                                             LPVOID *                          ppUserData);
   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETREGIONCALLBACK )(HDISPCONTAINER              hCon,
                                                                         DISPCONTAINERREGIONCALLBACK pfnCallBack,
                                                                         L_VOID *                    pUserData);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETREGIONCALLBACK )(HDISPCONTAINER                hCon,
                                                                         DISPCONTAINERREGIONCALLBACK * ppfnCallBack,
                                                                         LPVOID *                      ppUserData);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETANNOTATIONCREATEDCALLBACK )(HDISPCONTAINER                         hCon,
                                                                                    DISPCONTAINERANNOTATIONCREATEDCALLBACK pfnCallBack,
                                                                                    L_VOID *                               pUserData);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETANNOTATIONCREATEDCALLBACK )(HDISPCONTAINER                           hCon,
                                                                                    DISPCONTAINERANNOTATIONCREATEDCALLBACK * ppfnCallBack,
                                                                                    LPVOID *                                 ppUserData);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETACTIVESUBCELLCHANGEDCALLBACK )(HDISPCONTAINER                    hCon,
                                                                                       DISPCONTAINERACTIVESUBCELLCHANGED pfnCallBack,
                                                                                       L_VOID *                          pUserData);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETACTIVESUBCELLCHANGEDCALLBACK )(HDISPCONTAINER                      hCon,
                                                                                       DISPCONTAINERACTIVESUBCELLCHANGED * ppfnCallBack,
                                                                                       LPVOID *                            ppUserData);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSAVEANNOTATION )(HDISPCONTAINER hCon,
                                                                     LPTSTR         pFileName,
                                                                     L_INT          nCellIndex,
                                                                     L_INT          nSubCellIndex,
                                                                     L_INT          nStartPage,
                                                                     L_UINT         uFlags);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERLOADANNOTATION )(HDISPCONTAINER hCon,
                                                                     LPTSTR         pFileName,
                                                                     L_INT          nCellIndex,
                                                                     L_INT          nSubCellIndex,
                                                                     L_INT          nStartPage,
                                                                     L_UINT         uFlags);


   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETBITMAPPIXEL )(HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_INT          nSubCellIndex,
                                                                     LPPOINT        pSrcPoint,
                                                                     LPPOINT        pBitmapPoint,
                                                                     L_UINT         uFlags);

   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERLOADREGION )(HDISPCONTAINER hCon,
                                                                  LPTSTR         pFileName,
                                                                  L_INT          nCellIndex,
                                                                  L_INT          nSubCellIndex,
                                                                  L_INT          nStartPage,
                                                                  L_UINT         uFlags);
   
   typedef  L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSAVEREGION )(HDISPCONTAINER hCon,
                                                                  LPTSTR         pFileName,
                                                                  L_INT          nCellIndex,
                                                                  L_INT          nSubCellIndex,
                                                                  L_INT          nStartPage,
                                                                  L_UINT         uFlags);

//More Medical Viewer Functionalities
   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETCELLREGIONHANDLE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_HRGN hRgn,
      L_UINT uCombineMode,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLREGIONHANDLE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_HRGN * phRgn,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETANNOTATIONCONTAINER )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex, 
      HANNOBJECT * PhAnnContainer,
      L_UINT uFlags);
   
   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETANNOTATIONCONTAINER )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      HANNOBJECT hAnnContainer,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETMOUSECALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERMOUSECALLBACK pfnCallBack,
      L_VOID * pUserData);

   // This function sets the callback for the annnotaion creation.
   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETMOUSECALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERMOUSECALLBACK pfnCallBack,
      L_VOID * pUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETSUBCELLTAG )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uRow,
      L_UINT uAlign,
      L_UINT uType,
      LPTSTR pString,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETSUBCELLTAG )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uRow,
      L_UINT uAlign,
      DISPCELLTAGINFO * pTagInfo,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINEREDITSUBCELLTAG )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uRow,
      L_UINT uAlign,
      DISPCELLTAGINFO * pTagInfo,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERDELETESUBCELLTAG )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uRow,
      L_UINT uAlign,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLTAG )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_UINT uRow,
      L_UINT uAlign,
      DISPCELLTAGINFO * pTagInfo,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERDELETECELLTAG )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_UINT uRow,
      L_UINT uAlign,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINEREDITCELLTAG )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_UINT uRow,
      L_UINT uAlign,
      DISPCELLTAGINFO * pTagInfo,
      L_UINT uFlags);

   typedef HBITMAP ( pWRPEXT_CALLBACK pL_DISPCONTAINERPRINTCELL )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETPREPAINTCALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERPREPAINTCALLBACK pfnCallBack,
      LPVOID pUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETPREPAINTCALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERPREPAINTCALLBACK * ppfnCallBack,
      LPVOID * ppUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETPOSTPAINTCALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERPOSTPAINTCALLBACK pfnCallBack,
      LPVOID pUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETPOSTPAINTCALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERPOSTPAINTCALLBACK * ppfnCallBack,
      LPVOID * ppUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERENABLECELLLOWMEMORYUSAGE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nHiddenCount,
      L_INT nFrameCount,
      DISPCONTAINERBITMAPINFO * pBitmapInfo,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETLOWMEMORYUSAGECALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERFRAMEREQUESTEDCALLBACK pfnCallBack,
      LPVOID pUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETLOWMEMORYUSAGECALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERFRAMEREQUESTEDCALLBACK * ppfnCallBack,
      LPVOID * ppUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETREQUESTEDIMAGE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      pBITMAPHANDLE pBitmaps,
      L_INT * pBitmapIndexes,
      L_INT nLength,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERFREEZESUBCELL )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_BOOL bFreeze,
      L_UINT uFlags);

   typedef L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISSUBCELLFROZEN )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLSCALEMODE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT * puScaleMode,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETCELLSCALE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_DOUBLE dScale,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETCELLSCALEMODE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uScaleMode,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLSCALE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_DOUBLE * pdScale,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERUPDATECELLVIEW )(HDISPCONTAINER hCon,
      HWND hWnd,
      L_INT nCellIndex,
      L_UINT uFlags);

   typedef HBITMAP ( pWRPEXT_CALLBACK pL_DISPCONTAINERPRINTSUBCELL )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERCALIBRATECELL )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_DOUBLE dLength,
      L_UINT uUnit,
      L_DOUBLE dTargetLength,
      L_UINT uTargetUnit,
      L_UINT uFlags);

   typedef L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERHANDLEPALETTE )(HDISPCONTAINER hCon,
      L_UINT uMessage,
      WPARAM wParam,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETBITMAPLISTINFO )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      DISPCONTAINERBITMAPINFO * pBitmapInfo,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETANIMATIONSTARTEDCALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERANIMATIONSTARTEDCALLBACK pfnCallBack,
      LPVOID pUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETANIMATIONSTOPPEDCALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERANIMATIONSTOPPEDCALLBACK pfnCallBack,
      LPVOID pUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETANIMATIONSTARTEDCALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERANIMATIONSTARTEDCALLBACK * ppfnCallBack,
      LPVOID * ppUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETANIMATIONSTOPPEDCALLBACK )(HDISPCONTAINER hCon,
      DISPCONTAINERANIMATIONSTOPPEDCALLBACK * ppfnCallBack,
      LPVOID * ppUserData);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERSETDEFAULTWINDOWLEVELVALUES )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_INT nWidth,
      L_INT nCenter,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETDEFAULTWINDOWLEVELVALUES )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_INT * pnWidth,
      L_INT * pnCenter,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERRESETWINDOWLEVELVALUES )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);
//
   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERREVERSEBITMAP )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERFLIPBITMAP)(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISBITMAPFLIPPED )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISBITMAPREVERSED)(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);
//
   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERINVERTBITMAP )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_BOOL ( pWRPEXT_CALLBACK pL_DISPCONTAINERISBITMAPINVERTED )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_INT (pWRPEXT_CALLBACK pL_DISPCONTAINERFLIPANNOTATIONCONTAINER)(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_INT (pWRPEXT_CALLBACK pL_DISPCONTAINERREVERSEANNOTATIONCONTAINER)(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERROTATEBITMAPPERSPECTIVE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_INT nAngle,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERGETROTATEBITMAPPERSPECTIVEANGLE )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_INT * pnAngle,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERREMOVEBITMAPREGION )(HDISPCONTAINER hCon,
      L_INT nCellIndex,
      L_INT nSubCellIndex,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERBEGINUPDATE )(HDISPCONTAINER hCon,
      L_UINT uFlags);

   typedef L_INT ( pWRPEXT_CALLBACK pL_DISPCONTAINERENDUPDATE )(HDISPCONTAINER hCon,
      L_UINT uFlags);

   typedef L_INT (pWRPEXT_CALLBACK pL_DISPCONTAINERGETCELLREGIONHANDLE)(HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_HRGN *       phRgn,
                                                                        L_UINT         uFlags);



//-----------------------------------------------------------------------------
//--LTCLR.H FUNCTIONS PROTOTYPES---------------------------------------
//-----------------------------------------------------------------------------
typedef L_INT ( pWRPEXT_CALLBACK pL_GETPARAMETRICCURVENUMBEROFPARAMETERS)(ICCFUNCTIONTYPE enFunctionType);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRINIT)(
               L_HANDLE *pClrHandle,
               L_INT nSrcFormat,
               L_INT nDstFormat,
               LPCONVERSION_PARAMS pParams);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRSETCONVERSIONPARAMS)(
               L_HANDLE ClrHandle,
               LPCONVERSION_PARAMS pParams);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRCONVERTDIRECT)(
               L_INT nSrcFormat,
               L_INT nDstFormat,
               L_UCHAR* pSrcBuf,
               L_UCHAR* pDstBuf,
               L_INT nWidth,
               L_INT nHeight,
               L_INT nInAlign,
               L_INT nOutAlign);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRCONVERTDIRECTTOBITMAP)(
               L_INT nSrcFormat,
               L_INT nDstFormat,
               L_UCHAR* pSrcBuf,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nWidth,
               L_INT nHeight,
               L_INT nInAlign,
               L_INT nOutAlign);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRCONVERT)(
               L_HANDLE ClrHandle,
               L_UCHAR* pSrc,
               L_UCHAR* pDst,
               L_INT nWidth,
               L_INT nHeight,
               L_INT nInAlign,
               L_INT nOutAlign);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRCONVERTTOBITMAP)(
               L_HANDLE ClrHandle,
               L_UCHAR* pSrcBuf,
               pBITMAPHANDLE pBitmap,
               L_UINT uStructSize,
               L_INT nWidth,
               L_INT nHeight,
               L_INT nInAlign,
               L_INT nOutAlign);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRFREE)(
               L_HANDLE ClrHandle);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRISVALID)(
               L_HANDLE ClrHandle);

typedef L_INT ( pWRPEXT_CALLBACK pL_CLRDLG)(
               L_INT  nDlg,
               L_HANDLE hWnd,
               L_HANDLE *pClrHandle,
               LPCONVERSION_PARAMS pParams);

typedef L_INT ( pWRPEXT_CALLBACK pL_FILLICCPROFILESTRUCTURE)(pICCPROFILEEXT pICCProfile,
                                                         L_UCHAR     * pData,
                                                         L_SIZE_T      uDataSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_FILLICCPROFILEFROMICCFILE)(L_TCHAR * pszFileName,
                                                           pICCPROFILEEXT  pICCProfile);

typedef L_INT ( pWRPEXT_CALLBACK pL_INITICCPROFILE)(pICCPROFILEEXT pICCProfile, L_UINT uStructSize);

typedef L_VOID ( pWRPEXT_CALLBACK pL_FREEICCPROFILE)(pICCPROFILEEXT pICCProfile);

typedef L_INT ( pWRPEXT_CALLBACK pL_INITICCHEADER)(pICCPROFILEEXT pICCProfile);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCCMMTYPE)(pICCPROFILEEXT pICCProfile, L_IccInt32Number nCMMType);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCDEVICECLASS)(pICCPROFILEEXT pICCProfile, ICCPROFILECLASS uDevClassSig);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCCOLORSPACE)(pICCPROFILEEXT pICCProfile, ICCCOLORSPACE uColorSpace);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCCONNECTIONSPACE)(pICCPROFILEEXT pICCProfile, ICCCOLORSPACE uPCS);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCPRIMARYPLATFORM)(pICCPROFILEEXT pICCProfile, ICCPLATFORMSIGNATURE uPrimPlatform);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCFLAGS)(pICCPROFILEEXT pICCProfile, L_IccUInt32Number uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCDEVMANUFACTURER)(pICCPROFILEEXT pICCProfile, L_IccInt32Number nDevManufacturer);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCDEVMODEL)(pICCPROFILEEXT pICCProfile, L_IccUInt32Number uDevModel);
           
typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCDEVICEATTRIBUTES)(pICCPROFILEEXT pICCProfile, L_IccUInt64Number uAttributes);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCRENDERINGINTENT)(pICCPROFILEEXT pICCProfile, ICCRENDERINGINTENT uRenderingIntent);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCCREATOR)(pICCPROFILEEXT pICCProfile, L_IccInt32Number nCreator);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCDATETIME)(pICCPROFILEEXT pICCProfile, pICC_DATE_TIME_NUMBER pTime);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCTAGDATA)(pICCPROFILEEXT  pICCProfile, 
                                               L_UCHAR * pTagData, 
                                               L_UINT          uTagSig, 
                                               L_UINT          uTagTypeSig);

typedef L_INT ( pWRPEXT_CALLBACK pL_GETICCTAGDATA)(pICCPROFILEEXT     pICCProfile, 
                                               L_UCHAR *    pTagData, 
                                               L_UINT32           uTagSignature);

typedef L_INT ( pWRPEXT_CALLBACK pL_CREATEICCTAGDATA)(L_UCHAR * pDestTagData, 
                                                  L_UCHAR * pSrcTagData, 
                                                  L_UINT32        uTagTypeSig);

typedef L_INT ( pWRPEXT_CALLBACK pL_DELETEICCTAG)(pICCPROFILEEXT pICCProfile, L_UINT32 uTagSig, L_UCHAR * pTag);

typedef L_INT ( pWRPEXT_CALLBACK pL_GENERATEICCFILE)(pICCPROFILEEXT pICCProfile, L_TCHAR * pszFileName);

typedef L_UINT32 ( pWRPEXT_CALLBACK pL_DOUBLETO2BFIXED2BNUMBER)(L_DOUBLE dNumber);

typedef L_DOUBLE ( pWRPEXT_CALLBACK pL_2BFIXED2BNUMBERTODOUBLE)(L_UINT32 uNumber);

typedef L_UINT16 ( pWRPEXT_CALLBACK pL_DOUBLETOU8FIXED8NUMBER)(L_DOUBLE dNumber);

typedef L_DOUBLE ( pWRPEXT_CALLBACK pL_U8FIXED8NUMBERTODOUBLE)(L_UINT16 uNumber);

typedef L_INT ( pWRPEXT_CALLBACK pL_GENERATEICCPOINTER)(pICCPROFILEEXT pICCProfile);

typedef L_UINT32 ( pWRPEXT_CALLBACK pL_GETICCTAGTYPESIG)(pICCPROFILEEXT pICCProfile, L_UINT32 uTagSig);

typedef L_VOID ( pWRPEXT_CALLBACK pL_FREEICCTAGTYPE)(L_UCHAR * pTagType, L_UINT32 uTagTypeSig);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTCLUTTOBUFFER)(L_UCHAR * pData, L_VOID * pIccCLUT, L_INT nPrecision, L_SSIZE_T nDataSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTCURVETYPETOBUFFER)(L_UCHAR * pData, pICCTAG_CURVE_TYPE pIccTagCurveType);

typedef L_INT ( pWRPEXT_CALLBACK pL_CONVERTPARAMETRICCURVETYPETOBUFFER)(L_UCHAR * pData, pICCTAG_PARAMETRIC_CURVE_TYPE pIccTagParametricCurveType);

typedef L_INT ( pWRPEXT_CALLBACK pL_SETICCPROFILEID)(pICCPROFILEEXT pICCProfile);

typedef L_INT ( pWRPEXT_CALLBACK pL_LOADICCPROFILE)(L_TCHAR * pszFilename, pICCPROFILEEXT pICCProfile, pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_SAVEICCPROFILE)(L_TCHAR*  pszFilename, pICCPROFILEEXT  pICCProfile, pSAVEFILEOPTION pSaveOptions);

//-----------------------------------------------------------------------------
//--LTNTF.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFCREATE)(pHNITF phNitf, L_TCHAR * pszFileName);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFDESTROY)(HNITF hNitf);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETSTATUS )(HNITF hNitf);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFSAVEFILE)(HNITF hNitf, L_TCHAR * pszFileName);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFAPPENDIMAGESEGMENT)(HNITF hNitf, pBITMAPHANDLE pBitmap, L_INT nFormat, L_INT nBpp, L_INT nQFactor);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFAPPENDGRAPHICSEGMENT)(HNITF hNitf, pVECTORHANDLE pVector, LPRECT prcVecBounds);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFAPPENDTEXTSEGMENT)(HNITF hNitf, L_TCHAR* pszFileName);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFSETVECTORHANDLE)(HNITF hNitf, L_UINT32 uIndex, pVECTORHANDLE pVector);

typedef pVECTORHANDLE ( pWRPEXT_CALLBACK pL_NITFGETVECTORHANDLE)(HNITF hNitf, L_UINT32 uIndex);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETNITFHEADER )(HNITF hNitf, pNITFHEADER pNITFHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFSETNITFHEADER )(HNITF hNitf, pNITFHEADER pNITFHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETGRAPHICHEADERCOUNT)(HNITF hNitf);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETGRAPHICHEADER )(HNITF hNitf, L_UINT uIndex, pGRAPHICHEADER pGraphicHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFSETGRAPHICHEADER )(HNITF hNitf, L_UINT uIndex, pGRAPHICHEADER pGraphicsHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETIMAGEHEADERCOUNT)(HNITF hNitf);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETIMAGEHEADER )(HNITF hNitf, L_UINT uIndex, pIMAGEHEADER pImageHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFSETIMAGEHEADER )(HNITF hNitf, L_UINT uIndex, pIMAGEHEADER  pImageHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETTEXTHEADERCOUNT)(HNITF hNitf);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETTEXTHEADER )(HNITF hNitf, L_UINT uIndex, pTXTHEADER pTxtHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFSETTEXTHEADER )(HNITF hNitf, L_UINT uIndex, pTXTHEADER pTxtHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFGETTEXTSEGMENT )(HNITF hNitf, L_UINT uIndex, L_CHAR* pTextBuffer, L_UINT *puBufferSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFFREENITFHEADER )(pNITFHEADER pNITFHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFFREEGRAPHICHEADER )(pGRAPHICHEADER pGraphicsHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFFREEIMAGEHEADER )(pIMAGEHEADER pImageHeader);

typedef L_INT ( pWRPEXT_CALLBACK pL_NITFFREETEXTHEADER )(pTXTHEADER pTxtHeader);


//-----------------------------------------------------------------------------
//--LTWIA.H FUNCTIONS PROTOTYPES-----------------------------------------------
//-----------------------------------------------------------------------------
#if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)

typedef L_BOOL ( pWRPEXT_CALLBACK pL_WIAISAVAILABLE )(L_UINT uWiaVersion);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAINITSESSION )(L_UINT uWiaVersion, pHWIASESSION phSession);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAENDSESSION )(HWIASESSION hSession);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAENUMDEVICES)(HWIASESSION hSession, LWIAENUMDEVICESCALLBACK pfnCallback, L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASELECTDEVICEDLG)(HWIASESSION hSession, HWND hWndParent, L_UINT32 uDeviceType, L_UINT32 uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASELECTDEVICE)(HWIASESSION hSession, L_TCHAR* pszDeviceId);

typedef L_TCHAR* ( pWRPEXT_CALLBACK pL_WIAGETSELECTEDDEVICE)(HWIASESSION hSession);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAGETSELECTEDDEVICETYPE)(HWIASESSION hSession, L_UINT32 * puDeviceType);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAGETPROPERTIES)(HWIASESSION hSession, L_VOID * pItem, pLWIAPROPERTIES pProperties);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASETPROPERTIES)(HWIASESSION hSession, L_VOID * pItem, pLWIAPROPERTIES pProperties, LWIASETPROPERTIESCALLBACK pfnCallback, L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAACQUIRE)(HWIASESSION hSession, HWND hWndParent, L_UINT32 uFlags, L_VOID * pSourceItem, pLWIAACQUIREOPTIONS pAcquireOptions, L_INT * pnFilesCount, L_TCHAR *** pppszFilePaths, LWIAACQUIRECALLBACK pfnCallback, L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAACQUIRETOFILE)(HWIASESSION hSession, HWND hWndParent, L_UINT32 uFlags, L_VOID * pSourceItem, pLWIAACQUIREOPTIONS pAcquireOptions, L_INT * pnFilesCount, L_TCHAR *** pppszFilePaths, LWIAACQUIREFILECALLBACK pfnCallback, L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAACQUIRESIMPLE)(L_UINT uWiaVersion, HWND hWndParent, L_UINT32 uDeviceType, L_UINT32 uFlags, L_VOID * pSourceItem, pLWIAACQUIREOPTIONS pAcquireOptions, L_INT * pnFilesCount, L_TCHAR *** pppszFilePaths, LWIAACQUIRECALLBACK pfnCallback, L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASTARTVIDEOPREVIEW)(HWIASESSION hSession, HWND hWndParent, L_BOOL bStretchToFitParent);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIARESIZEVIDEOPREVIEW)(HWIASESSION hSession, L_BOOL bStretchToFitParent);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAENDVIDEOPREVIEW)(HWIASESSION hSession);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAACQUIREIMAGEFROMVIDEO)(HWIASESSION hSession, L_TCHAR* pszFileName, L_SIZE_T* puLength);

typedef L_BOOL ( pWRPEXT_CALLBACK pL_WIAISVIDEOPREVIEWAVAILABLE)(HWIASESSION hSession);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAGETROOTITEM)(HWIASESSION hSession, L_VOID * pItem, L_VOID ** ppWiaRootItem);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAENUMCHILDITEMS)(HWIASESSION hSession, L_VOID * pWiaRootItem, LWIAENUMITEMSCALLBACK pfnCallback, L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAFREEITEM)(HWIASESSION hSession, L_VOID * pItem);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAGETPROPERTYLONG)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_INT32* plValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASETPROPERTYLONG)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_INT32 lValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAGETPROPERTYGUID)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, GUID* pGuidValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASETPROPERTYGUID)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, GUID* pGuidValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAGETPROPERTYSTRING)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_TCHAR* pszValue, L_SIZE_T* puLength);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASETPROPERTYSTRING)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_TCHAR* pszValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAGETPROPERTYSYSTEMTIME)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, LPSYSTEMTIME pValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASETPROPERTYSYSTEMTIME)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, LPSYSTEMTIME pValue);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAGETPROPERTYBUFFER)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_UCHAR* pValue, L_SIZE_T* puSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIASETPROPERTYBUFFER)(HWIASESSION hSession, L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_UCHAR* pValue, L_SIZE_T uSize);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAENUMCAPABILITIES)(HWIASESSION hSession, L_VOID * pItem, L_UINT uFlags, LWIAENUMCAPABILITIESCALLBACK pfnCallBack, L_VOID * pUserData);

typedef L_INT ( pWRPEXT_CALLBACK pL_WIAENUMFORMATS)(HWIASESSION hSession, L_VOID * pItem, L_UINT uFlags, LWIAENUMFORMATSCALLBACK pfnCallBack, L_VOID * pUserData);

#endif //#if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)

#endif //USE_POINTERS_TO_LEAD_FUNCTIONS

#endif //_LEAD_FUNCTIONS_TYPEDEFINES_H_
/*================================================================= EOF =====*/
