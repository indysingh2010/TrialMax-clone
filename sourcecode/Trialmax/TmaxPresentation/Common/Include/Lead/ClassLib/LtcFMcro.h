/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright(c) 1991-2007 LEAD Technologies, Inc.                              |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : tcprnt.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_FUNCTIONS_MACROS_H_
#define  _LEAD_FUNCTIONS_MACROS_H_

/*----------------------------------------------------------------------------+
| MACROS                                                                      |
+----------------------------------------------------------------------------*/
#ifdef USE_POINTERS_TO_LEAD_FUNCTIONS

//-----------------------------------------------------------------------------
//--LTKRN.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPACCESSBITMAP(pBitmap) \
   ((pL_AccessBitmap )?pL_AccessBitmap(pBitmap):LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED))

#define L_WRPALLOCATEBITMAP(pBitmap,uMemory) \
   ((pL_AllocateBitmap )?pL_AllocateBitmap(pBitmap,uMemory):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPHEIGHT(pBitmap,nHeight) \
   ((pL_ChangeBitmapHeight )?pL_ChangeBitmapHeight(pBitmap,nHeight):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPVIEWPERSPECTIVE(pDstBitmap,pSrcBitmap,uStructSize,ViewPerspective) \
   ((pL_ChangeBitmapViewPerspective )?pL_ChangeBitmapViewPerspective(pDstBitmap,pSrcBitmap,uStructSize,ViewPerspective):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCHANGEFROMDIB(pBitmap,uStructSize,hDIB) \
   ((pL_ChangeFromDIB )?pL_ChangeFromDIB(pBitmap,uStructSize,hDIB):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCHANGETODIB(pBitmap,uType) \
   ((pL_ChangeToDIB )?pL_ChangeToDIB(pBitmap,uType):(LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED),(L_HDIB)0))

#define L_WRPCLEARBITMAP(pBitmap) \
   ((pL_ClearBitmap )?pL_ClearBitmap(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCLEARNEGATIVEPIXELS(pBitmap) \
   ((pL_ClearNegativePixels )?pL_ClearNegativePixels(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOLORRESBITMAP(pBitmapSrc,pBitmapDst,uStructSize,nBitsPerPixel,uFlags,pPalette,hPalette,uColors,pfnCallback,pUserData) \
   ((pL_ColorResBitmap )?pL_ColorResBitmap(pBitmapSrc,pBitmapDst,uStructSize,nBitsPerPixel,uFlags,pPalette,hPalette,uColors,pfnCallback,pUserData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOLORRESBITMAPLIST(hList,nBitsPerPixel,uFlags,pPalette,hPalette,uColors) \
   ((pL_ColorResBitmapList )?pL_ColorResBitmapList(hList,nBitsPerPixel,uFlags,pPalette,hPalette,uColors):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOLORRESBITMAPLISTEXT(hList,nBitsPerPixel,uFlags,pPalette,hPalette,uColors,pfnCallback,pUserData) \
   ((pL_ColorResBitmapListExt )?pL_ColorResBitmapListExt(hList,nBitsPerPixel,uFlags,pPalette,hPalette,uColors,pfnCallback,pUserData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOMPRESSROW(pRunBuffer,pBuffer,nCol,nWidth) \
   ((pL_CompressRow )?pL_CompressRow(pRunBuffer,pBuffer,nCol,nWidth):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOMPRESSROWS(pRunBuffer,pBuffer,nWidth,nRows) \
   ((pL_CompressRows )?pL_CompressRows(pRunBuffer,pBuffer,nWidth,nRows):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCONVERTBUFFER(pBuffer,nWidth,nBitsPerPixelSrc,nBitsPerPixelDst,nOrderSrc,nOrderDst,pPaletteSrc,pPaletteDst) \
   ((pL_ConvertBuffer )?pL_ConvertBuffer(pBuffer,nWidth,nBitsPerPixelSrc,nBitsPerPixelDst,nOrderSrc,nOrderDst,pPaletteSrc,pPaletteDst):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCONVERTBUFFEREXT(pBuffer,nWidth,nBitsPerPixelSrc,nBitsPerPixelDst,nOrderSrc,nOrderDst,pPaletteSrc,pPaletteDst,uFlags,uLowBit,uHighBit) \
   ((pL_ConvertBufferExt )?pL_ConvertBufferExt(pBuffer,nWidth,nBitsPerPixelSrc,nBitsPerPixelDst,nOrderSrc,nOrderDst,pPaletteSrc,pPaletteDst,uFlags,uLowBit,uHighBit):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCONVERTFROMDIB(pBitmap,uStructSize,pInfo,pBits) \
   ((pL_ConvertFromDIB )?pL_ConvertFromDIB(pBitmap,uStructSize,pInfo,pBits):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCONVERTTODIB(pBitmap,uType) \
   ((pL_ConvertToDIB )?pL_ConvertToDIB(pBitmap,uType):(LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED),(L_HGLOBAL)0))

#define L_WRPCOPYBITMAP(pBitmapDst,pBitmapSrc,uStructSize) \
   ((pL_CopyBitmap )?pL_CopyBitmap(pBitmapDst,pBitmapSrc,uStructSize):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOPYBITMAP2(pBitmapDst,pBitmapSrc,uStructSize,uMemory) \
   ((pL_CopyBitmap2 )?pL_CopyBitmap2(pBitmapDst,pBitmapSrc,uStructSize,uMemory):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOPYBITMAPDATA(pBitmapDst,pBitmapSrc) \
   ((pL_CopyBitmapData )?pL_CopyBitmapData(pBitmapDst,pBitmapSrc):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOPYBITMAPHANDLE(pBitmapDst,pBitmapSrc,uStructSize) \
   ((pL_CopyBitmapHandle )?pL_CopyBitmapHandle(pBitmapDst,pBitmapSrc,uStructSize):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOPYBITMAPLISTITEMS(phList,hList,uIndex,uCount) \
   ((pL_CopyBitmapListItems )?pL_CopyBitmapListItems(phList,hList,uIndex,uCount):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOPYBITMAPRECT(pBitmapDst,pBitmapSrc,uStructSize,nCol,nRow,uWidth,uHeight) \
   ((pL_CopyBitmapRect )?pL_CopyBitmapRect(pBitmapDst,pBitmapSrc,uStructSize,nCol,nRow,uWidth,uHeight):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOPYBITMAPRECT2(pBitmapDst,pBitmapSrc,uStructSize,nCol,nRow,uWidth,uHeight,uMemory) \
   ((pL_CopyBitmapRect2 )?pL_CopyBitmapRect2(pBitmapDst,pBitmapSrc,uStructSize,nCol,nRow,uWidth,uHeight,uMemory):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCREATEBITMAP(pBitmap,uStructSize,uMemory,uWidth,uHeight,uBitsPerPixel,uOrder,pPalette,uViewPerspective,pData,dwSize) \
   ((pL_CreateBitmap )?pL_CreateBitmap(pBitmap,uStructSize,uMemory,uWidth,uHeight,uBitsPerPixel,uOrder,pPalette,uViewPerspective,pData,dwSize):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCREATEBITMAPLIST(phList) \
   ((pL_CreateBitmapList )?pL_CreateBitmapList(phList):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCREATEUSERMATCHTABLE(pPalette,uColors) \
   ((pL_CreateUserMatchTable )?pL_CreateUserMatchTable(pPalette,uColors):(LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED),(L_UINT *)0))

#define L_WRPDEFAULTDITHERING(uMethod) \
   ((pL_DefaultDithering )?pL_DefaultDithering(uMethod):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPDELETEBITMAPLISTITEMS(hList,uIndex,uCount) \
   ((pL_DeleteBitmapListItems )?pL_DeleteBitmapListItems(hList,uIndex,uCount):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPDESTROYBITMAPLIST(hList) \
   ((pL_DestroyBitmapList )?pL_DestroyBitmapList(hList):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPDITHERLINE(pBitmap,pBufferSrc,pBufferDst) \
   ((pL_DitherLine )?pL_DitherLine(pBitmap,pBufferSrc,pBufferDst):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOPYBITMAPPALETTE(pDstBitmap,pSrcBitmap) \
   ((pL_CopyBitmapPalette )?pL_CopyBitmapPalette(pDstBitmap,pSrcBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPDUPBITMAPPALETTE(pBitmap) \
   ((pL_DupBitmapPalette )?pL_DupBitmapPalette(pBitmap):(LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED),(L_HPALETTE)0))

#define L_WRPDUPPALETTE(hPalette) \
   ((pL_DupPalette )?pL_DupPalette(hPalette):(LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED),(L_HPALETTE)0))

#define L_WRPEXPANDROW(pRunBuffer,pBuffer,nCol,nWidth) \
   ((pL_ExpandRow )?pL_ExpandRow(pRunBuffer,pBuffer,nCol,nWidth):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPEXPANDROWS(pRun,pBuffer,nWidth,nRows) \
   ((pL_ExpandRows )?pL_ExpandRows(pRun,pBuffer,nWidth,nRows):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPFILLBITMAP(pBitmap,crFill) \
   ((pL_FillBitmap )?pL_FillBitmap(pBitmap,crFill):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPFLIPBITMAP(pBitmap) \
   ((pL_FlipBitmap )?pL_FlipBitmap(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPFREEBITMAP(pBitmap) \
   ((pL_FreeBitmap )?pL_FreeBitmap(pBitmap):LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED))

#define L_WRPFREEUSERMATCHTABLE(pTable) \
   ((pL_FreeUserMatchTable )?pL_FreeUserMatchTable(pTable):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPCOLORS(pBitmap,nIndex,nCount,pPalette) \
   ((pL_GetBitmapColors )?pL_GetBitmapColors(pBitmap,nIndex,nCount,pPalette):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPLISTCOUNT(hList,puCount) \
   ((pL_GetBitmapListCount )?pL_GetBitmapListCount(hList,puCount):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPLISTITEM(hList,uIndex,pBitmap,uStructSize) \
   ((pL_GetBitmapListItem )?pL_GetBitmapListItem(hList,uIndex,pBitmap,uStructSize):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPROW(pBitmap,pBuffer,nRow,uBytes) \
   ((pL_GetBitmapRow )?pL_GetBitmapRow(pBitmap,pBuffer,nRow,uBytes):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPROWCOL(pBitmap,pBuffer,nRow,nCol,uBytes) \
   ((pL_GetBitmapRowCol )?pL_GetBitmapRowCol(pBitmap,pBuffer,nRow,nCol,uBytes):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPROWCOLCOMPRESSED(pBitmap,pWorkBuffer,pRunBuffer,nRow,nCol,uWidth) \
   ((pL_GetBitmapRowColCompressed )?pL_GetBitmapRowColCompressed(pBitmap,pWorkBuffer,pRunBuffer,nRow,nCol,uWidth):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPROWCOMPRESSED(pBitmap,pWorkBuffer,pRunBuffer,nRow,nLines) \
   ((pL_GetBitmapRowCompressed )?pL_GetBitmapRowCompressed(pBitmap,pWorkBuffer,pRunBuffer,nRow,nLines):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETFIXEDPALETTE(pPalette,nBitsPerPixel) \
   ((pL_GetFixedPalette )?pL_GetFixedPalette(pPalette,nBitsPerPixel):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETPIXELCOLOR(pBitmap,nRow,nCol) \
   ((pL_GetPixelColor )?pL_GetPixelColor(pBitmap,nRow,nCol):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETSTATUSCALLBACK(ppUserData) \
   ((pL_GetStatusCallBack )?pL_GetStatusCallBack(ppUserData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETSTATUSCALLBACK(pfnCallback,pUserData,pfnOldCallback,ppOldUserData) \
   ((pL_SetStatusCallBack )?pL_SetStatusCallBack(pfnCallback,pUserData,pfnOldCallback,ppOldUserData):LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED))

#define L_WRPGETCOPYSTATUSCALLBACK(ppUserData) \
   ((pL_GetCopyStatusCallBack )?pL_GetCopyStatusCallBack(ppUserData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETCOPYSTATUSCALLBACK(pfnCallback,pUserData,pOldFunction,ppUserData) \
   ((pL_SetCopyStatusCallBack )?pL_SetCopyStatusCallBack(pfnCallback,pUserData,pOldFunction,ppUserData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGRAYSCALEBITMAP(pBitmap,nBitsPerPixel) \
   ((pL_GrayScaleBitmap )?pL_GrayScaleBitmap(pBitmap,nBitsPerPixel):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPINITBITMAP(pBitmap,uStructSize,nWidth,nHeight,nBitsPerPixel) \
   ((pL_InitBitmap )?pL_InitBitmap(pBitmap,uStructSize,nWidth,nHeight,nBitsPerPixel):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPINSERTBITMAPLISTITEM(hList,uIndex,pBitmap) \
   ((pL_InsertBitmapListItem )?pL_InsertBitmapListItem(hList,uIndex,pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPISGRAYSCALEBITMAP(pBitmap) \
   ((pL_IsGrayScaleBitmap )?pL_IsGrayScaleBitmap(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPISSUPPORTLOCKED(uType) \
   ((pL_IsSupportLocked )?pL_IsSupportLocked(uType):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPOINTFROMBITMAP(pBitmap,ViewPerspective,px,py) \
   ((pL_PointFromBitmap )?pL_PointFromBitmap(pBitmap,ViewPerspective,px,py):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPOINTTOBITMAP(pBitmap,ViewPerspective,px,py) \
   ((pL_PointToBitmap )?pL_PointToBitmap(pBitmap,ViewPerspective,px,py):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPUTBITMAPCOLORS(pBitmap,nIndex,nCount,pPalette) \
   ((pL_PutBitmapColors )?pL_PutBitmapColors(pBitmap,nIndex,nCount,pPalette):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPUTBITMAPROW(pBitmap,pBuffer,nRow,uBytes) \
   ((pL_PutBitmapRow )?pL_PutBitmapRow(pBitmap,pBuffer,nRow,uBytes):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPUTBITMAPROWCOL(pBitmap,pBuffer,nRow,nCol,uBytes) \
   ((pL_PutBitmapRowCol )?pL_PutBitmapRowCol(pBitmap,pBuffer,nRow,nCol,uBytes):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPUTBITMAPROWCOLCOMPRESSED(pBitmap,pWorkBuffer,pRunBuffer,nRow,nCol,uWidth) \
   ((pL_PutBitmapRowColCompressed )?pL_PutBitmapRowColCompressed(pBitmap,pWorkBuffer,pRunBuffer,nRow,nCol,uWidth):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPUTBITMAPROWCOMPRESSED(pBitmap,pWorkBuffer,pRunBuffer,nRow,nLines) \
   ((pL_PutBitmapRowCompressed )?pL_PutBitmapRowCompressed(pBitmap,pWorkBuffer,pRunBuffer,nRow,nLines):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPUTPIXELCOLOR(pBitmap,nRow,nCol,crColor) \
   ((pL_PutPixelColor )?pL_PutPixelColor(pBitmap,nRow,nCol,crColor):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPRECTFROMBITMAP(pBitmap,ViewPerspective,lprc) \
   ((pL_RectFromBitmap )?pL_RectFromBitmap(pBitmap,ViewPerspective,lprc):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPRECTTOBITMAP(pBitmap,nViewPerspective,lprc) \
   ((pL_RectToBitmap )?pL_RectToBitmap(pBitmap,nViewPerspective,lprc):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPREDIRECTIO(pfnOpen,pfnRead,pfnWrite,pfnSeek,pfnClose,pUserData) \
   ((pL_RedirectIO )?pL_RedirectIO(pfnOpen,pfnRead,pfnWrite,pfnSeek,pfnClose,pUserData):LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED))

#define L_WRPRELEASEBITMAP(pBitmap) \
   ((pL_ReleaseBitmap )?pL_ReleaseBitmap(pBitmap):LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED))

#define L_WRPREMOVEBITMAPLISTITEM(hList,uIndex,pBitmap) \
   ((pL_RemoveBitmapListItem )?pL_RemoveBitmapListItem(hList,uIndex,pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPRESIZE(pBuffer,nRow,nBitsPerPixel,pXSize,pYSize,pResizeData) \
   ((pL_Resize )?pL_Resize(pBuffer,nRow,nBitsPerPixel,pXSize,pYSize,pResizeData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPRESIZEBITMAP(pBitmapSrc,pDestBitmap,uFlags) \
   ((pL_ResizeBitmap )?pL_ResizeBitmap(pBitmapSrc,pDestBitmap,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPREVERSEBITMAP(pBitmap) \
   ((pL_ReverseBitmap )?pL_ReverseBitmap(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPROTATEBITMAP(pBitmap,nAngle,uFlags,crFill) \
   ((pL_RotateBitmap )?pL_RotateBitmap(pBitmap,nAngle,uFlags,crFill):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPROTATEBITMAPVIEWPERSPECTIVE(pBitmap,nAngle) \
   ((pL_RotateBitmapViewPerspective )?pL_RotateBitmapViewPerspective(pBitmap,nAngle):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETBITMAPDATAPOINTER(pBitmap,pData,dwSize) \
   ((pL_SetBitmapDataPointer )?pL_SetBitmapDataPointer(pBitmap,pData,dwSize):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETBITMAPLISTITEM(hList,uIndex,pBitmap) \
   ((pL_SetBitmapListItem )?pL_SetBitmapListItem(hList,uIndex,pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETUSERMATCHTABLE(pTable) \
   ((pL_SetUserMatchTable )?pL_SetUserMatchTable(pTable):(LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED),(L_UINT *)0))

#define L_WRPSIZEBITMAP(pBitmap,nWidth,nHeight,uFlags) \
   ((pL_SizeBitmap )?pL_SizeBitmap(pBitmap,nWidth,nHeight,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSTARTDITHERING(pBitmap,pPalette,uColors) \
   ((pL_StartDithering )?pL_StartDithering(pBitmap,pPalette,uColors):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSTARTRESIZE(nOldWidth,nOldHeight,nNewWidth,nNewHeight,ppResizeData) \
   ((pL_StartResize )?pL_StartResize(nOldWidth,nOldHeight,nNewWidth,nNewHeight,ppResizeData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSTOPDITHERING(pBitmap) \
   ((pL_StopDithering )?pL_StopDithering(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSTOPRESIZE(pResizeData) \
   ((pL_StopResize )?pL_StopResize(pResizeData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPTRANSLATEBITMAPCOLOR(pBitmapDst,pBitmapSrc,crColor) \
   ((pL_TranslateBitmapColor )?pL_TranslateBitmapColor(pBitmapDst,pBitmapSrc,crColor):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPTOGGLEBITMAPCOMPRESSION(pBitmap) \
   ((pL_ToggleBitmapCompression )?pL_ToggleBitmapCompression(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPTRIMBITMAP(pBitmap,nCol,nRow,uWidth,uHeight) \
   ((pL_TrimBitmap )?pL_TrimBitmap(pBitmap,nCol,nRow,uWidth,uHeight):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPUNLOCKSUPPORT(uType,pKey) \
   ((pL_UnlockSupport )?pL_UnlockSupport(uType,pKey):LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED))

#define L_WRPKERNELHASEXPIRED() \
   ((pL_KernelHasExpired )?pL_KernelHasExpired():WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPFLIPBITMAPVIEWPERSPECTIVE(pBitmap) \
   ((pL_FlipBitmapViewPerspective )?pL_FlipBitmapViewPerspective(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPREVERSEBITMAPVIEWPERSPECTIVE(pBitmap) \
   ((pL_ReverseBitmapViewPerspective )?pL_ReverseBitmapViewPerspective(pBitmap):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSTARTRESIZEBITMAP(pBitmap,nNewWidth,nNewHeight,nNewBits,pPalette,nColors,uFlags,pfnCallback,pCallbackData,ppResizeData) \
   ((pL_StartResizeBitmap )?pL_StartResizeBitmap(pBitmap,nNewWidth,nNewHeight,nNewBits,pPalette,nColors,uFlags,pfnCallback,pCallbackData,ppResizeData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETRESIZEDROWCOL(pResizeData,pBuffer,nRow,nCol,uBytes) \
   ((pL_GetResizedRowCol )?pL_GetResizedRowCol(pResizeData,pBuffer,nRow,nCol,uBytes):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSTOPRESIZEBITMAP(pResizeData) \
   ((pL_StopResizeBitmap )?pL_StopResizeBitmap(pResizeData):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPMOVEBITMAPLISTITEMS(phList,hList,uIndex,uCount) \
   ((pL_MoveBitmapListItems )?pL_MoveBitmapListItems(phList,hList,uIndex,uCount):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPCOMPRESSION(pBitmap,nComp) \
   ((pL_ChangeBitmapCompression )?pL_ChangeBitmapCompression(pBitmap,nComp):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETPIXELDATA(pBitmap,pData,nRow,nCol,uBytes) \
   ((pL_GetPixelData )?pL_GetPixelData(pBitmap,pData,nRow,nCol,uBytes):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPPUTPIXELDATA(pBitmap,pData,nRow,nCol,uBytes) \
   ((pL_PutPixelData )?pL_PutPixelData(pBitmap,pData,nRow,nCol,uBytes):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETDEFAULTMEMORYTYPE(uMemory) \
   ((pL_SetDefaultMemoryType )?pL_SetDefaultMemoryType(uMemory):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETDEFAULTMEMORYTYPE(puMemory) \
   ((pL_GetDefaultMemoryType )?pL_GetDefaultMemoryType(puMemory):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETMEMORYTHRESHOLDS(nTiledThreshold,nMaxConvSize,nTileSize,nConvTiles,nConvBuffers,uFlags) \
   ((pL_SetMemoryThresholds )?pL_SetMemoryThresholds(nTiledThreshold,nMaxConvSize,nTileSize,nConvTiles,nConvBuffers,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETMEMORYTHRESHOLDS(pnTiledThreshold,pnMaxConvSize,pnTileSize,pnConvTiles,pnConvBuffers) \
   ((pL_GetMemoryThresholds )?pL_GetMemoryThresholds(pnTiledThreshold,pnMaxConvSize,pnTileSize,pnConvTiles,pnConvBuffers):LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED))

#define L_WRPSETBITMAPMEMORYINFO(pBitmap,uMemory,uTileSize,uTotalTiles,uConvTiles,uMaxTileViews,uTileViews,uFlags) \
   ((pL_SetBitmapMemoryInfo )?pL_SetBitmapMemoryInfo(pBitmap,uMemory,uTileSize,uTotalTiles,uConvTiles,uMaxTileViews,uTileViews,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPMEMORYINFO(pBitmap,puMemory,puTileSize,puTotalTiles,puConvTiles,puMaxTileViews,puTileViews) \
   ((pL_GetBitmapMemoryInfo )?pL_GetBitmapMemoryInfo(pBitmap,puMemory,puTileSize,puTotalTiles,puConvTiles,puMaxTileViews,puTileViews):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETTEMPDIRECTORY(pszTempDir) \
   ((pL_SetTempDirectory )?pL_SetTempDirectory(pszTempDir):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETTEMPDIRECTORY(pszTempDir,uSize) \
   ((pL_GetTempDirectory )?pL_GetTempDirectory(pszTempDir,uSize):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETBITMAPPALETTE(pBitmap,hPalette) \
   ((pL_SetBitmapPalette )?pL_SetBitmapPalette(pBitmap,hPalette):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSCRAMBLEBITMAP(pBitmap,nColStart,nRowStart,nWidth,nHeight,uKey,uFlags) \
   ((pL_ScrambleBitmap )?pL_ScrambleBitmap(pBitmap,nColStart,nRowStart,nWidth,nHeight,uKey,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCOMBINEBITMAPWARP(pBitmapDst,ptDstArray,pBitmapSrc,ptSrc,nSrcWidth,nSrcHeight,uFlags) \
   ((pL_CombineBitmapWarp )?pL_CombineBitmapWarp(pBitmapDst,ptDstArray,pBitmapSrc,ptSrc,nSrcWidth,nSrcHeight,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETOVERLAYBITMAP(pBitmap,nIndex,pOverlayBitmap,uFlags) \
   ((pL_SetOverlayBitmap )?pL_SetOverlayBitmap(pBitmap,nIndex,pOverlayBitmap,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETOVERLAYBITMAP(pBitmap,nIndex,pOverlayBitmap,uStructSize,uFlags) \
   ((pL_GetOverlayBitmap )?pL_GetOverlayBitmap(pBitmap,nIndex,pOverlayBitmap,uStructSize,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETOVERLAYATTRIBUTES(pBitmap,nIndex,pOverlayAttributes,uFlags) \
   ((pL_SetOverlayAttributes )?pL_SetOverlayAttributes(pBitmap,nIndex,pOverlayAttributes,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETOVERLAYATTRIBUTES(pBitmap,nIndex,pOverlayAttributes,uStructSize,uFlags) \
   ((pL_GetOverlayAttributes )?pL_GetOverlayAttributes(pBitmap,nIndex,pOverlayAttributes,uStructSize,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPUPDATEBITMAPOVERLAYBITS(pBitmap,nIndex,uFlags) \
   ((pL_UpdateBitmapOverlayBits )?pL_UpdateBitmapOverlayBits(pBitmap,nIndex,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETOVERLAYCOUNT(pBitmap,puCount,uFlags) \
   ((pL_GetOverlayCount )?pL_GetOverlayCount(pBitmap,puCount,uFlags):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCREATELEADDC(pBitmap) \
   ((pL_CreateLeadDC )?pL_CreateLeadDC(pBitmap):(LBase::RecordError(WRPERR_LTKRN_DLL_NOT_LOADED),(L_HDC)0))

#define L_WRPDELETELEADDC(hDC) \
   ((pL_DeleteLeadDC )?pL_DeleteLeadDC(hDC):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPGETBITMAPALPHA(pBitmap,pAlpha,uStructSize) \
   ((pL_GetBitmapAlpha )?pL_GetBitmapAlpha(pBitmap,pAlpha,uStructSize):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETBITMAPALPHA(pBitmap,pAlpha) \
   ((pL_SetBitmapAlpha )?pL_SetBitmapAlpha(pBitmap,pAlpha):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSETBITMAPALPHAVALUES(pBitmap,uAlpha) \
   ((pL_SetBitmapAlphaValues )?pL_SetBitmapAlphaValues(pBitmap,uAlpha):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPSHEARBITMAP(pBitmap,nAngle,fHorizontal,crFill) \
   ((pL_ShearBitmap )?pL_ShearBitmap(pBitmap,nAngle,fHorizontal,crFill):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPVERSIONINFO(pVersionInfo,uStructSize) \
   ((pL_VersionInfo )?pL_VersionInfo(pVersionInfo,uStructSize):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPHASEXPIRED() \
   ((pL_HasExpired )?pL_HasExpired():WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCREATEBITMAPLISTOPTPAL(hList, pPalette, puColors, ppMatchTable, pbGenerated) \
   ((pL_CreateBitmapListOptPal) ? pL_CreateBitmapListOptPal(hList, pPalette, puColors, ppMatchTable, pbGenerated):WRPERR_LTKRN_DLL_NOT_LOADED)

#define L_WRPCREATEGRAYSCALEBITMAP(pDstBitmap, pSrcBitmap, uBitsPerPixel) \
   ((pL_CreateGrayScaleBitmap )?pL_CreateGrayScaleBitmap(pDstBitmap, pSrcBitmap, uBitsPerPixel):WRPERR_LTKRN_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTIMG.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#if defined (LEADTOOLS_V16_OR_LATER)
#define L_WRPEDGEDETECTORBITMAP(pBitmap, uThreshold, uFilter, uFlags) \
      ((pL_EdgeDetectorBitmap )?pL_EdgeDetectorBitmap(pBitmap, uThreshold, uFilter, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPHISTOCONTRASTBITMAP(pBitmap, nChange, uFlags) \
      ((pL_HistoContrastBitmap )?pL_HistoContrastBitmap(pBitmap, nChange, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETMINMAXBITS(pBitmap, pLowBit, pHighBit, uFlags) \
      ((pL_GetMinMaxBits )?pL_GetMinMaxBits(pBitmap, pLowBit, pHighBit, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSTRETCHBITMAPINTENSITY(pBitmap, uFlags) \
      ((pL_StretchBitmapIntensity )?pL_StretchBitmapIntensity(pBitmap, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPSPATIALFILTERBITMAP(pBitmap, pFilter, uFlags) \
      ((pL_SpatialFilterBitmap )?pL_SpatialFilterBitmap(pBitmap, pFilter, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSOLARIZEBITMAP(pBitmap, uThreshold, uFlags) \
      ((pL_SolarizeBitmap )?pL_SolarizeBitmap(pBitmap, uThreshold, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPPOSTERIZEBITMAP(pBitmap, uLevels, uFlags) \
      ((pL_PosterizeBitmap )?pL_PosterizeBitmap(pBitmap, uLevels, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETMINMAXVAL(pBitmap, pMinVal, pMaxVal, uFlags) \
      ((pL_GetMinMaxVal )?pL_GetMinMaxVal(pBitmap, pMinVal, pMaxVal, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPINTENSITYDETECTBITMAP(pBitmap, uLow, uHigh, crInColor, crOutColor, uChannel, uFlags) \
      ((pL_IntensityDetectBitmap )?pL_IntensityDetectBitmap(pBitmap, uLow, uHigh, crInColor, crOutColor, uChannel, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPMAXFILTERBITMAP(pBitmap, uDim, uFlags) \
      ((pL_MaxFilterBitmap )?pL_MaxFilterBitmap(pBitmap, uDim, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPMEDIANFILTERBITMAP(pBitmap, uDim, uFlags) \
      ((pL_MedianFilterBitmap )?pL_MedianFilterBitmap(pBitmap, uDim, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSLICEBITMAP(pBitmap, pOptions, pnDeskewAngle, pfnCallback, pUserData, uFlags) \
      ((pL_SliceBitmap )?pL_SliceBitmap(pBitmap, pOptions, pnDeskewAngle, pfnCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPHOLESREMOVALBITMAPRGN(pBitmap, uFlags) \
      ((pL_HolesRemovalBitmapRgn )?pL_HolesRemovalBitmapRgn(pBitmap, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPUSERFILTERBITMAP(pBitmap, pFilter, uFlags) \
      ((pL_UserFilterBitmap )?pL_UserFilterBitmap(pBitmap, pFilter, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGAMMACORRECTBITMAP(pBitmap, uGamma, uFlags) \
      ((pL_GammaCorrectBitmap )?pL_GammaCorrectBitmap(pBitmap, uGamma, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPEMBOSSBITMAP(pBitmap, uDirection, uDepth, uFlags) \
      ((pL_EmbossBitmap )?pL_EmbossBitmap(pBitmap, uDirection, uDepth, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSMOOTHBITMAP(pBitmap, pSmooth, pfnCallback, pUserData, uFlags) \
      ((pL_SmoothBitmap )?pL_SmoothBitmap(pBitmap, pSmooth, pfnCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPLINEREMOVEBITMAP(pBitmap, pLineRemove, pfnCallback, pUserData, uFlags) \
      ((pL_LineRemoveBitmap )?pL_LineRemoveBitmap(pBitmap, pLineRemove, pfnCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPRAKEREMOVEBITMAP(pBitmap, bAuto, pComb, pDstRect, nRectCount, pCallback, pUserData, uFlags) \
   ((pL_RakeRemoveBitmap )?pL_RakeRemoveBitmap(pBitmap, bAuto, pComb, pDstRect, nRectCount, pCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPOBJECTCOUNTER(pBitmap, uCount, pCallback, pUserData, uFlags) \
   ((pL_ObjectCounter )?pL_ObjectCounter(pBitmap, uCount, pCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPBORDERREMOVEBITMAP(pBitmap, pBorderRemove, pfnCallback, pUserData, uFlags) \
      ((pL_BorderRemoveBitmap )?pL_BorderRemoveBitmap(pBitmap, pBorderRemove, pfnCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPINVERTEDTEXTBITMAP(pBitmap, pInvertedText, pfnCallback, pUserData, uFlags) \
      ((pL_InvertedTextBitmap )?pL_InvertedTextBitmap(pBitmap, pInvertedText, pfnCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPDOTREMOVEBITMAP(pBitmap, pDotRemove, pfnCallback, pUserData, uFlags) \
      ((pL_DotRemoveBitmap )?pL_DotRemoveBitmap(pBitmap, pDotRemove, pfnCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPHOLEPUNCHREMOVEBITMAP(pBitmap, pHolePunch, pfnCallback, pUserData, uFlags) \
      ((pL_HolePunchRemoveBitmap )?pL_HolePunchRemoveBitmap(pBitmap, pHolePunch, pfnCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSTAPLEREMOVEBITMAP(pBitmap, pStaplePunch, pfnCallback, pUserData, uFlags) \
      ((pL_StapleRemoveBitmap )?pL_StapleRemoveBitmap(pBitmap, pStaplePunch, pfnCallback, pUserData, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPDESPECKLEBITMAP(pBitmap, uFlags) \
      ((pL_DespeckleBitmap )?pL_DespeckleBitmap(pBitmap, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPHUE(pBitmap, nAngle, uFlags) \
      ((pL_ChangeBitmapHue )?pL_ChangeBitmapHue(pBitmap, nAngle, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPINTENSITY(pBitmap, nChange, uFlags) \
      ((pL_ChangeBitmapIntensity )?pL_ChangeBitmapIntensity(pBitmap, nChange, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPSATURATION(pBitmap, nChange, uFlags) \
      ((pL_ChangeBitmapSaturation )?pL_ChangeBitmapSaturation(pBitmap, nChange, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPINVERTBITMAP(pBitmap, uFlags) \
      ((pL_InvertBitmap )?pL_InvertBitmap(pBitmap, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPSHARPENBITMAP(pBitmap, nSharpness, uFlags) \
      ((pL_SharpenBitmap )?pL_SharpenBitmap(pBitmap, nSharpness, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETBITMAPCOLORCOUNT(pBitmap, puCount, uFlags) \
      ((pL_GetBitmapColorCount )?pL_GetBitmapColorCount(pBitmap, puCount, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETAUTOTRIMRECT(pBitmap, uThreshold, pRect, uFlags) \
      ((pL_GetAutoTrimRect)?pL_GetAutoTrimRect(pBitmap, uThreshold, pRect, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPCONTRAST(pBitmap, nChange, uFlags) \
      ((pL_ChangeBitmapContrast )?pL_ChangeBitmapContrast(pBitmap, nChange, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPBINARYFILTERBITMAP(pBitmap, pFilter, uFlags) \
      ((pL_BinaryFilterBitmap )?pL_BinaryFilterBitmap(pBitmap, pFilter, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPAVERAGEFILTERBITMAP(pBitmap, uDim, uFlags) \
      ((pL_AverageFilterBitmap )?pL_AverageFilterBitmap(pBitmap, uDim, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPAUTOTRIMBITMAP(pBitmap, uThreshold, uFlags) \
      ((pL_AutoTrimBitmap )?pL_AutoTrimBitmap(pBitmap, uThreshold, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPADDBITMAPNOISE(pBitmap, uRange, uChannel, uFlags) \
      ((pL_AddBitmapNoise )?pL_AddBitmapNoise(pBitmap, uRange, uChannel, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPFEATHERALPHABLENDBITMAP(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, pBitmapMask, nXMaskShift, nYMaskShift, uFlags) \
      ((pL_FeatherAlphaBlendBitmap )?pL_FeatherAlphaBlendBitmap(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, pBitmapMask, nXMaskShift, nYMaskShift, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPALPHABLENDBITMAP(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, nOpacity, uFlags) \
      ((pL_AlphaBlendBitmap )?pL_AlphaBlendBitmap(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, nOpacity, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETBITMAPCOLORCOUNT(pBitmap, puCount, uFlags) \
      ((pL_GetBitmapColorCount )?pL_GetBitmapColorCount(pBitmap, puCount, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOMBINEBITMAP(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, uFlag, uFlags1) \
      ((pL_CombineBitmap )?pL_CombineBitmap(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, uFlag, uFlags1):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPWINDBITMAP(pBitmap, uDim, nAngle, uOpacity, uFlags) \
      ((pL_WindBitmap )?pL_WindBitmap(pBitmap, uDim, nAngle, uOpacity, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPRADIALBLURBITMAP(pBitmap, uDim, uStress, CenterPt, uFlags) \
      ((pL_RadialBlurBitmap )?pL_RadialBlurBitmap(pBitmap, uDim, uStress, CenterPt, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPSWIRLBITMAP(pBitmap, nAngle, CenterPt, uFlags) \
      ((pL_SwirlBitmap )?pL_SwirlBitmap(pBitmap, nAngle, CenterPt, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPZOOMBLURBITMAP(pBitmap, uDim, uStress, CenterPt, uFlags) \
      ((pL_ZoomBlurBitmap )?pL_ZoomBlurBitmap(pBitmap, uDim, uStress, CenterPt, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPIMPRESSIONISTBITMAP(pBitmap, uHorzDim, uVertDim, uFlags) \
      ((pL_ImpressionistBitmap )?pL_ImpressionistBitmap(pBitmap, uHorzDim, uVertDim, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPREMOVEREDEYEBITMAP(pBitmap, rcNewColor, uThreshold, nLightness, uFlags) \
      ((pL_RemoveRedeyeBitmap )?pL_RemoveRedeyeBitmap(pBitmap, rcNewColor, uThreshold, nLightness, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPMINFILTERBITMAP(pBitmap, uDim, uFlags) \
      ((pL_MinFilterBitmap )?pL_MinFilterBitmap(pBitmap, uDim, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPMOSAICBITMAP(pBitmap, uDim, uFlags) \
      ((pL_MosaicBitmap )?pL_MosaicBitmap(pBitmap, uDim, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPLINEPROFILE(pBitmap, FirstPoint, SecondPoint,pRed,pGreen,pBlue, uFlags) \
      ((pL_LineProfile )?pL_LineProfile(pBitmap, FirstPoint, SecondPoint,pRed,pGreen,pBlue, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGRAYSCALEBITMAPEXT(pBitmap, RedFact, GreenFact, BlueFact, uFlags) \
      ((pL_GrayScaleBitmapExt )?pL_GrayScaleBitmapExt(pBitmap, RedFact, GreenFact, BlueFact, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPBALANCECOLORS(pBitmap, pRedFact, pGreenFact, pBlueFact, uFlags) \
      ((pL_BalanceColors )?pL_BalanceColors(pBitmap, pRedFact, pGreenFact, pBlueFact, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCONVERTTOCOLOREDGRAY(pBitmap, RedFact, GreenFact, BlueFact, RedGrayFact, GreenGrayFact, BlueGrayFact, uFlags) \
      ((pL_ConvertToColoredGray )?pL_ConvertToColoredGray(pBitmap, RedFact, GreenFact, BlueFact, RedGrayFact, GreenGrayFact, BlueGrayFact, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPANTIALIASBITMAP(pBitmap, uThreshold, uDim, uFilter, uFlags) \
      ((pL_AntiAliasBitmap )?pL_AntiAliasBitmap(pBitmap, uThreshold, uDim, uFilter, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPREMAPBITMAPHUE(pBitmap, pMask, pHTable, pSTable, pVTable, uLUTLen, uFlags) \
      ((pL_RemapBitmapHue )?pL_RemapBitmapHue(pBitmap, pMask, pHTable, pSTable, pVTable, uLUTLen, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPWINDOWLEVELBITMAP(pBitmap, nLowBit, nHighBit, pLUT, uLUTLength, nOrderDst, uFlags) \
      ((pL_WindowLevelBitmap )?pL_WindowLevelBitmap(pBitmap, nLowBit, nHighBit, pLUT, uLUTLength, nOrderDst, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPWINDOWLEVELBITMAPEXT(pBitmap, nLowBit, nHighBit, pLUT, uLUTLength, nOrderDst, uFlags) \
      ((pL_WindowLevelBitmapExt)?pL_WindowLevelBitmapExt(pBitmap, nLowBit, nHighBit, pLUT, uLUTLength, nOrderDst, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPGAUSSIANFILTERBITMAP(pBitmap, nRadius, uFlags) \
      ((pL_GaussianFilterBitmap )?pL_GaussianFilterBitmap(pBitmap, nRadius, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETUSERLOOKUPTABLE(pLookupTable, uLookupLen, apUserPoint, uUserPointCount, puPointCount, uFlags) \
      ((pL_GetUserLookUpTable )?pL_GetUserLookUpTable(pLookupTable, uLookupLen, apUserPoint, uUserPointCount, puPointCount, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCONVERTBITMAPSIGNEDTOUNSIGNED(pBitmap, uShift, uFlags) \
      ((pL_ConvertBitmapSignedToUnsigned )?pL_ConvertBitmapSignedToUnsigned(pBitmap, uShift, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPTEXTUREALPHABLENDBITMAP(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, pBitmapMask, nOpacity, pBitmapUnderlay, LPpOffset, uFlags) \
      ((pL_TextureAlphaBlendBitmap )?pL_TextureAlphaBlendBitmap(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, pBitmapMask, nOpacity, pBitmapUnderlay, LPpOffset, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPMULTIPLYBITMAP(pBitmap, uFactor, uFlags) \
      ((pL_MultiplyBitmap )?pL_MultiplyBitmap(pBitmap, uFactor, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPLOCALHISTOEQUALIZEBITMAP(pBitmap, nWidth, nHeight, nxExt, nyExt, uType, uSmooth, uFlags) \
      ((pL_LocalHistoEqualizeBitmap )?pL_LocalHistoEqualizeBitmap(pBitmap, nWidth, nHeight, nxExt, nyExt, uType, uSmooth, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPOILIFYBITMAP(pBitmap, uDim, uFlags) \
      ((pL_OilifyBitmap )?pL_OilifyBitmap(pBitmap, uDim, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCONTOURFILTERBITMAP(pBitmap, nThreshold, nDeltaDirection, nMaximumError, nOption, uFlags) \
      ((pL_ContourFilterBitmap )?pL_ContourFilterBitmap(pBitmap, nThreshold, nDeltaDirection, nMaximumError, nOption, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPMOTIONBLURBITMAP(pBitmap, uDim, nAngle, bUnidirectional, uFlags) \
      ((pL_MotionBlurBitmap )?pL_MotionBlurBitmap(pBitmap, uDim, nAngle, bUnidirectional, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPPICTURIZEBITMAPLIST(pBitmap, uCellWidth, uCellHeight, uLightnessFact, hList, uFlags) \
      ((pL_PicturizeBitmapList )?pL_PicturizeBitmapList(pBitmap, uCellWidth, uCellHeight, uLightnessFact, hList, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPPICTURIZEBITMAPSINGLE(pBitmap, pThumbBitmap, uCellWidth, uCellHeight, uLightnessFact, uFlags) \
      ((pL_PicturizeBitmapSingle )?pL_PicturizeBitmapSingle(pBitmap, pThumbBitmap, uCellWidth, uCellHeight, uLightnessFact, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPADDBORDER(pBitmap, pAddBorderInfo, uFlags) \
      ((pL_AddBorder )?pL_AddBorder(pBitmap, pAddBorderInfo, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPADDFRAME(pBitmap, pAddFrameInfo, uFlags) \
      ((pL_AddFrame )?pL_AddFrame(pBitmap, pAddFrameInfo, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPADDMESSAGETOBITMAP(pBitmap, pAddMesgInfo, uFlags) \
      ((pL_AddMessageToBitmap )?pL_AddMessageToBitmap(pBitmap, pAddMesgInfo, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPEXTRACTMESSAGEFROMBITMAP(pBitmap, pAddMesgInfo, uFlags) \
      ((pL_ExtractMessageFromBitmap )?pL_ExtractMessageFromBitmap(pBitmap, pAddMesgInfo, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPHALFTONEBITMAP(pBitmap, uType, nAngle, uDim, hList, uFlags) \
      ((pL_HalfToneBitmap )?pL_HalfToneBitmap(pBitmap, uType, nAngle, uDim, hList, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPBUMPMAPBITMAP(pBitmapDst, pBumpBitmap, pBumpData, uFlags) \
      ((pL_BumpMapBitmap )?pL_BumpMapBitmap(pBitmapDst, pBumpBitmap, pBumpData, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPGLOWFILTERBITMAP(pBitmap, uDim, uBright, uThreshold, uFlags) \
      ((pL_GlowFilterBitmap )?pL_GlowFilterBitmap(pBitmap, uDim, uBright, uThreshold, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPEDGEDETECTSTATISTICALBITMAP(pBitmap, uDim, uThreshold, crEdgeColor, crBkColor, uFlags) \
      ((pL_EdgeDetectStatisticalBitmap )?pL_EdgeDetectStatisticalBitmap(pBitmap, uDim, uThreshold, crEdgeColor, crBkColor, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPDESATURATEBITMAP(pBitmap, uFlags) \
      ((pL_DesaturateBitmap )?pL_DesaturateBitmap(pBitmap, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPSMOOTHEDGESBITMAP(pBitmap, nAmount, nThreshold, uFlags) \
      ((pL_SmoothEdgesBitmap )?pL_SmoothEdgesBitmap(pBitmap, nAmount, nThreshold, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPAUTOBINARYBITMAP(pBitmap, uFlags) \
      ((pL_AutoBinaryBitmap )?pL_AutoBinaryBitmap(pBitmap, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCHANNELMIX(pBitmap, pRedFactor, pGreenFactor, pBlueFactor, uFlags) \
      ((pL_ChannelMix )?pL_ChannelMix(pBitmap, pRedFactor, pGreenFactor, pBlueFactor, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPOCEANBITMAP(pBitmap, uAmplitude, uFrequency, bLowerTrnsp, uFlags) \
      ((pL_OceanBitmap )?pL_OceanBitmap(pBitmap, uAmplitude, uFrequency, bLowerTrnsp, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPLIGHTBITMAP(pBitmap, pLightInfo, uLightNo, uBright, uAmbient, crAmbientClr, uFlags) \
      ((pL_LightBitmap )?pL_LightBitmap(pBitmap, pLightInfo, uLightNo, uBright, uAmbient, crAmbientClr, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDRYBITMAP(pBitmap, uDim, uFlags) \
      ((pL_DryBitmap )?pL_DryBitmap(pBitmap, uDim, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDRAWSTARBITMAP(pBitmap, pStarInfo, uFlags) \
      ((pL_DrawStarBitmap )?pL_DrawStarBitmap(pBitmap, pStarInfo, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPFRQFILTERMASKBITMAP(pMaskBitmap, pFTArray, bOnOff, uFlags) \
      ((pL_FrqFilterMaskBitmap )?pL_FrqFilterMaskBitmap(pMaskBitmap, pFTArray, bOnOff, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPALLOCFTARRAY(pBitmap, ppFTArray, uStructSize, uFlags) \
      ((pL_AllocFTArray )?pL_AllocFTArray(pBitmap, ppFTArray, uStructSize, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPFREEFTARRAY(pFTArray, uFlags) \
      ((pL_FreeFTArray )?pL_FreeFTArray(pFTArray, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSKELETONBITMAP(pBitmap, uThreshold, uFlags) \
      ((pL_SkeletonBitmap )?pL_SkeletonBitmap(pBitmap, uThreshold, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSELECTIVECOLORBITMAP(pBitmap, pSelClr, uFlags) \
      ((pL_SelectiveColorBitmap )?pL_SelectiveColorBitmap(pBitmap, pSelClr, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETOBJECTINFO(pBitmap, puX, puY, pnAngle, puRoundness, bWeighted, uFlags) \
      ((pL_GetObjectInfo )?pL_GetObjectInfo(pBitmap, puX, puY, pnAngle, puRoundness, bWeighted, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETRGNPERIMETERLENGTH(pBitmap, pXForm, puLength, uFlags) \
      ((pL_GetRgnPerimeterLength )?pL_GetRgnPerimeterLength(pBitmap, pXForm, puLength, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETFERETSDIAMETER(pPoints, uSize, puFeretsDiameter, puFirstIndex, puSecondIndex, uFlags) \
      ((pL_GetFeretsDiameter )?pL_GetFeretsDiameter(pPoints, uSize, puFeretsDiameter, puFirstIndex, puSecondIndex, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCOLORREPLACEBITMAP(pBitmap, pColorReplace, uColorCount, nHue, nSaturation, nBrightness, uFlags) \
      ((pL_ColorReplaceBitmap )?pL_ColorReplaceBitmap(pBitmap, pColorReplace, uColorCount, nHue, nSaturation, nBrightness, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCHANGEHUESATINTBITMAP(pBitmap, nHue, nSaturation, nIntensity, pHsiData, uHsiDataCount, uFlags) \
      ((pL_ChangeHueSatIntBitmap )?pL_ChangeHueSatIntBitmap(pBitmap, nHue, nSaturation, nIntensity, pHsiData, uHsiDataCount, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETBITMAPSTATISTICSINFO(pBitmap, pStatisticsInfo, uChannel, uStart, uEnd, uFlags) \
      ((pL_GetBitmapStatisticsInfo )?pL_GetBitmapStatisticsInfo(pBitmap, pStatisticsInfo, uChannel, uStart, uEnd, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCOLORREPLACEWEIGHTSBITMAP(pBitmap, pColorReplace, uColorCount, uFlags) \
      ((pL_ColorReplaceWeightsBitmap )?pL_ColorReplaceWeightsBitmap(pBitmap, pColorReplace, uColorCount, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPDIRECTIONEDGESTATISTICALBITMAP(pBitmap, uDim, uThreshold, nAngle, crEdgeColor, crBkColor, uFlags) \
      ((pL_DirectionEdgeStatisticalBitmap )?pL_DirectionEdgeStatisticalBitmap(pBitmap, uDim, uThreshold, nAngle, crEdgeColor, crBkColor, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPMATHFUNCTIONBITMAP(pBitmap, uMType, uFactor, uFlags) \
      ((pL_MathFunctionBitmap )?pL_MathFunctionBitmap(pBitmap, uMType, uFactor, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOLORTHRESHOLDBITMAP(pBitmap, uColorSpace, pCompData, uFlags) \
      ((pL_ColorThresholdBitmap )?pL_ColorThresholdBitmap(pBitmap, uColorSpace, pCompData, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPREVEFFECTBITMAP(pBitmap, uLineSpace, uMaximumHeight, uFlags) \
      ((pL_RevEffectBitmap )?pL_RevEffectBitmap(pBitmap, uLineSpace, uMaximumHeight, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDYNAMICBINARYBITMAP(pBitmap, uDim, uLocalContrast, uFlags) \
      ((pL_DynamicBinaryBitmap )?pL_DynamicBinaryBitmap(pBitmap, uDim, uLocalContrast, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOLORINTENSITYBALANCE(pBitmap, pShadows, pMidTone, pHighLight, bLuminance, uFlags) \
      ((pL_ColorIntensityBalance )?pL_ColorIntensityBalance(pBitmap, pShadows, pMidTone, pHighLight, bLuminance, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPFUNCTIONALLIGHTBITMAP(pBitmap, pLightParams, uFlags) \
      ((pL_FunctionalLightBitmap )?pL_FunctionalLightBitmap(pBitmap, pLightParams, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPSHIFTBITMAPDATA(pDstBitmap, pSrcBitmap, uSrcLowBit, uSrcHighBit, uDstLowBit, uDstBitsPerPixel, uFlags) \
      ((pL_ShiftBitmapData )?pL_ShiftBitmapData(pDstBitmap, pSrcBitmap, uSrcLowBit, uSrcHighBit, uDstLowBit, uDstBitsPerPixel, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSELECTBITMAPDATA(pDstBitmap, pSrcBitmap, crColor, uSrcLowBit, uSrcHighBit, uThreshold, bCombine, uFlags) \
      ((pL_SelectBitmapData )?pL_SelectBitmapData(pDstBitmap, pSrcBitmap, crColor, uSrcLowBit, uSrcHighBit, uThreshold, bCombine, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCOLORIZEGRAYBITMAP(pDstBitmap, pSrcBitmap, pGrayColors, uCount, uFlags) \
      ((pL_ColorizeGrayBitmap )?pL_ColorizeGrayBitmap(pDstBitmap, pSrcBitmap, pGrayColors, uCount, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCONTBRIGHTINTBITMAP(pBitmap, nContrast, nBrightness, nIntensity, uFlags) \
      ((pL_ContBrightIntBitmap )?pL_ContBrightIntBitmap(pBitmap, nContrast, nBrightness, nIntensity, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPISREGMARKBITMAP(pBitmap, uType, uMinScale, uMaxScale, uWidth, uHeight, uFlags) \
      ((pL_IsRegMarkBitmap )?pL_IsRegMarkBitmap(pBitmap, uType, uMinScale, uMaxScale, uWidth, uHeight, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPGETMARKSCENTERMASSBITMAP(pBitmap, pMarkPoints, pMarkCMPoints, uMarksCount, uFlags) \
      ((pL_GetMarksCenterMassBitmap )?pL_GetMarksCenterMassBitmap(pBitmap, pMarkPoints, pMarkCMPoints, uMarksCount, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSEARCHREGMARKSBITMAP(pBitmap, pSearchMarks, uMarkCount, uFlags) \
      ((pL_SearchRegMarksBitmap )?pL_SearchRegMarksBitmap(pBitmap, pSearchMarks, uMarkCount, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPGETTRANSFORMATIONPARAMETERS(pBitmap, pRefPoints, pTrnsPoints, pnXTranslation, pnYTranslation, pnAngle, puXScale, puYScale, uFlags) \
      ((pL_GetTransformationParameters )?pL_GetTransformationParameters(pBitmap, pRefPoints, pTrnsPoints, pnXTranslation, pnYTranslation, pnAngle, puXScale, puYScale, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPFRAGMENTBITMAP(pBitmap, uOffset, uOpacity, uFlags) \
      ((pL_FragmentBitmap )?pL_FragmentBitmap(pBitmap, uOffset, uOpacity, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPVIGNETTEBITMAP(pBitmap, pVignetteInfo, uFlags) \
      ((pL_VignetteBitmap )?pL_VignetteBitmap(pBitmap, pVignetteInfo, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPMOSAICTILESBITMAP(pBitmap, pMosaicTilesInfo, uFlags) \
      ((pL_MosaicTilesBitmap )?pL_MosaicTilesBitmap(pBitmap, pMosaicTilesInfo, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPLASMAFILTERBITMAP(pBitmap,  pPlasmaInfo, uFlags) \
      ((pL_PlasmaFilterBitmap )?pL_PlasmaFilterBitmap(pBitmap,  pPlasmaInfo, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPADJUSTBITMAPTINT(pBitmap, nAngleA, nAngleB, uFlags) \
      ((pL_AdjustBitmapTint )?pL_AdjustBitmapTint(pBitmap, nAngleA, nAngleB, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOLOREDPENCILBITMAP(pBitmap, uRatio, uDim, uFlags) \
      ((pL_ColoredPencilBitmap )?pL_ColoredPencilBitmap(pBitmap, uRatio, uDim, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDIFFUSEGLOWBITMAP(pBitmap, nGlowAmount, uClearAmount, uSpreadAmount, uWhiteNoise, crGlowColor, uFlags) \
      ((pL_DiffuseGlowBitmap )?pL_DiffuseGlowBitmap(pBitmap, nGlowAmount, uClearAmount, uSpreadAmount, uWhiteNoise, crGlowColor, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPHIGHPASSFILTERBITMAP(pBitmap, uRadius, uOpacity, uFlags) \
      ((pL_HighPassFilterBitmap )?pL_HighPassFilterBitmap(pBitmap, uRadius, uOpacity, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCOLORHALFTONEBITMAP(pBitmap, uMaxRad, nCyanAngle, nMagentaAngle, nYellowAngle, nBlackAngle, uFlags) \
      ((pL_ColorHalfToneBitmap )?pL_ColorHalfToneBitmap(pBitmap, uMaxRad, nCyanAngle, nMagentaAngle, nYellowAngle, nBlackAngle, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCORRELATIONLISTBITMAP(pBitmap, hCorList, pPoints, puListIndex, uMaxPoints, puNumOfPoints, uXStep, uYStep, uThreshold, uFlags) \
      ((pL_CorrelationListBitmap )?pL_CorrelationListBitmap(pBitmap, hCorList, pPoints, puListIndex, uMaxPoints, puNumOfPoints, uXStep, uYStep, uThreshold, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSETKAUFMANNRGNBITMAP(pBitmap, pKaufmannProcBitmap, nRadius, nMinInput, nMaxInput, nRgnThreshold, ptRgnStart, bRemoveHoles, puPixelsCount, uCombineMode, uFlags) \
      ((pL_SetKaufmannRgnBitmap )?pL_SetKaufmannRgnBitmap(pBitmap, pKaufmannProcBitmap, nRadius, nMinInput, nMaxInput, nRgnThreshold, ptRgnStart, bRemoveHoles, puPixelsCount, uCombineMode, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSHIFTMINIMUMTOZERO(pBitmap, puShiftAmount, uFlags) \
      ((pL_ShiftMinimumToZero )?pL_ShiftMinimumToZero(pBitmap, puShiftAmount, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSHIFTZEROTONEGATIVE(pBitmap, nShiftAmount, nMinInput, nMaxInput, nMinOutput, nMaxOutput, uFlags) \
      ((pL_ShiftZeroToNegative )?pL_ShiftZeroToNegative(pBitmap, nShiftAmount, nMinInput, nMaxInput, nMinOutput, nMaxOutput, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPHISTOEQUALIZEBITMAP(pBitmap, uFlags) \
      ((pL_HistoEqualizeBitmap )?pL_HistoEqualizeBitmap(pBitmap, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPSWAPCOLORS(pBitmap, uFlags) \
      ((pL_SwapColors )?pL_SwapColors(pBitmap, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCYLINDRICALBITMAP(pBitmap, nValue, uType, uFlags) \
      ((pL_CylindricalBitmap )?pL_CylindricalBitmap(pBitmap, nValue, uType, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPCORRELATIONBITMAP(pBitmap, pCorBitmap, pPoints, uMaxPoints, puNumOfPoints, uXStep, uYStep, uThreshold, uFlags) \
      ((pL_CorrelationBitmap )?pL_CorrelationBitmap(pBitmap, pCorBitmap, pPoints, uMaxPoints, puNumOfPoints, uXStep, uYStep, uThreshold, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)


#else
#define L_WRPEDGEDETECTORBITMAP(pBitmap, uThreshold, uFilter) \
      ((pL_EdgeDetectorBitmap )?pL_EdgeDetectorBitmap(pBitmap, uThreshold, uFilter):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSLICEBITMAP(pBitmap, pOptions, pnDeskewAngle, pfnCallback, pUserData) \
      ((pL_SliceBitmap )?pL_SliceBitmap(pBitmap, pOptions, pnDeskewAngle, pfnCallback, pUserData):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPHOLESREMOVALBITMAPRGN(pBitmap) \
      ((pL_HolesRemovalBitmapRgn )?pL_HolesRemovalBitmapRgn(pBitmap):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPUSERFILTERBITMAP(pBitmap, pFilter) \
      ((pL_UserFilterBitmap )?pL_UserFilterBitmap(pBitmap, pFilter):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGAMMACORRECTBITMAP(pBitmap, uGamma) \
      ((pL_GammaCorrectBitmap )?pL_GammaCorrectBitmap(pBitmap, uGamma):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPEMBOSSBITMAP(pBitmap, uDirection, uDepth) \
      ((pL_EmbossBitmap )?pL_EmbossBitmap(pBitmap, uDirection, uDepth):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSMOOTHBITMAP(pBitmap, pSmooth, pfnCallback, pUserData) \
      ((pL_SmoothBitmap )?pL_SmoothBitmap(pBitmap, pSmooth, pfnCallback, pUserData):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPLINEREMOVEBITMAP(pBitmap, pLineRemove, pfnCallback, pUserData) \
      ((pL_LineRemoveBitmap )?pL_LineRemoveBitmap(pBitmap, pLineRemove, pfnCallback, pUserData):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPBORDERREMOVEBITMAP(pBitmap, pBorderRemove, pfnCallback, pUserData) \
      ((pL_BorderRemoveBitmap )?pL_BorderRemoveBitmap(pBitmap, pBorderRemove, pfnCallback, pUserData):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPINVERTEDTEXTBITMAP(pBitmap, pInvertedText, pfnCallback, pUserData) \
      ((pL_InvertedTextBitmap )?pL_InvertedTextBitmap(pBitmap, pInvertedText, pfnCallback, pUserData):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPDOTREMOVEBITMAP(pBitmap, pDotRemove, pfnCallback, pUserData) \
      ((pL_DotRemoveBitmap )?pL_DotRemoveBitmap(pBitmap, pDotRemove, pfnCallback, pUserData):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPHOLEPUNCHREMOVEBITMAP(pBitmap, pHolePunch, pfnCallback, pUserData) \
      ((pL_HolePunchRemoveBitmap )?pL_HolePunchRemoveBitmap(pBitmap, pHolePunch, pfnCallback, pUserData):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSTAPLEREMOVEBITMAP(pBitmap, pStaplePunch, pfnCallback, pUserData) \
      ((pL_StapleRemoveBitmap )?pL_StapleRemoveBitmap(pBitmap, pStaplePunch, pfnCallback, pUserData):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPDESPECKLEBITMAP(pBitmap) \
      ((pL_DespeckleBitmap )?pL_DespeckleBitmap(pBitmap):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPHUE(pBitmap, nAngle) \
      ((pL_ChangeBitmapHue )?pL_ChangeBitmapHue(pBitmap, nAngle):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPINTENSITY(pBitmap, nChange) \
      ((pL_ChangeBitmapIntensity )?pL_ChangeBitmapIntensity(pBitmap, nChange):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPSATURATION(pBitmap, nChange) \
      ((pL_ChangeBitmapSaturation )?pL_ChangeBitmapSaturation(pBitmap, nChange):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPINVERTBITMAP(pBitmap) \
      ((pL_InvertBitmap )?pL_InvertBitmap(pBitmap):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPSHARPENBITMAP(pBitmap, nSharpness) \
      ((pL_SharpenBitmap )?pL_SharpenBitmap(pBitmap, nSharpness):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETBITMAPCOLORCOUNT(pBitmap, puCount) \
      ((pL_GetBitmapColorCount )?pL_GetBitmapColorCount(pBitmap, puCount):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETAUTOTRIMRECT(pBitmap, uThreshold, pRect) \
      ((pL_GetAutoTrimRect)?pL_GetAutoTrimRect(pBitmap, uThreshold, pRect):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCHANGEBITMAPCONTRAST(pBitmap, nChange) \
      ((pL_ChangeBitmapContrast )?pL_ChangeBitmapContrast(pBitmap, nChange):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPBINARYFILTERBITMAP(pBitmap, pFilter) \
      ((pL_BinaryFilterBitmap )?pL_BinaryFilterBitmap(pBitmap, pFilter):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPAVERAGEFILTERBITMAP(pBitmap, uDim) \
      ((pL_AverageFilterBitmap )?pL_AverageFilterBitmap(pBitmap, uDim):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPAUTOTRIMBITMAP(pBitmap, uThreshold) \
      ((pL_AutoTrimBitmap )?pL_AutoTrimBitmap(pBitmap, uThreshold):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPADDBITMAPNOISE(pBitmap, uRange, uChannel) \
      ((pL_AddBitmapNoise )?pL_AddBitmapNoise(pBitmap, uRange, uChannel):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPFEATHERALPHABLENDBITMAP(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, pBitmapMask, nXMaskShift, nYMaskShift) \
      ((pL_FeatherAlphaBlendBitmap )?pL_FeatherAlphaBlendBitmap(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, pBitmapMask, nXMaskShift, nYMaskShift):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPALPHABLENDBITMAP(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, nOpacity) \
      ((pL_AlphaBlendBitmap )?pL_AlphaBlendBitmap(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, nOpacity):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETBITMAPCOLORCOUNT(pBitmap, puCount) \
      ((pL_GetBitmapColorCount )?pL_GetBitmapColorCount(pBitmap, puCount):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOMBINEBITMAP(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, uFlag) \
      ((pL_CombineBitmap )?pL_CombineBitmap(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, uFlag):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPHISTOCONTRASTBITMAP(pBitmap, nChange) \
      ((pL_HistoContrastBitmap )?pL_HistoContrastBitmap(pBitmap, nChange):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETMINMAXBITS(pBitmap, pLowBit, pHighBit) \
      ((pL_GetMinMaxBits )?pL_GetMinMaxBits(pBitmap, pLowBit, pHighBit):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSTRETCHBITMAPINTENSITY(pBitmap) \
      ((pL_StretchBitmapIntensity )?pL_StretchBitmapIntensity(pBitmap):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPSPATIALFILTERBITMAP(pBitmap, pFilter) \
      ((pL_SpatialFilterBitmap )?pL_SpatialFilterBitmap(pBitmap, pFilter):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSOLARIZEBITMAP(pBitmap, uThreshold) \
      ((pL_SolarizeBitmap )?pL_SolarizeBitmap(pBitmap, uThreshold):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPPOSTERIZEBITMAP(pBitmap, uLevels) \
      ((pL_PosterizeBitmap )?pL_PosterizeBitmap(pBitmap, uLevels):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETMINMAXVAL(pBitmap, pMinVal, pMaxVal) \
      ((pL_GetMinMaxVal )?pL_GetMinMaxVal(pBitmap, pMinVal, pMaxVal):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPINTENSITYDETECTBITMAP(pBitmap, uLow, uHigh, crInColor, crOutColor, uChannel) \
      ((pL_IntensityDetectBitmap )?pL_IntensityDetectBitmap(pBitmap, uLow, uHigh, crInColor, crOutColor, uChannel):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPMAXFILTERBITMAP(pBitmap, uDim) \
      ((pL_MaxFilterBitmap )?pL_MaxFilterBitmap(pBitmap, uDim):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPMEDIANFILTERBITMAP(pBitmap, uDim) \
      ((pL_MedianFilterBitmap )?pL_MedianFilterBitmap(pBitmap, uDim):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPWINDBITMAP(pBitmap, uDim, nAngle, uOpacity) \
      ((pL_WindBitmap )?pL_WindBitmap(pBitmap, uDim, nAngle, uOpacity):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPZOOMWAVEBITMAP(pBitmap, uAmplitude, uFrequency, nPhase, uZomFact, CenterPt, crFill, uFlag) \
      ((pL_ZoomWaveBitmap )?pL_ZoomWaveBitmap(pBitmap, uAmplitude, uFrequency, nPhase, uZomFact, CenterPt, crFill, uFlag):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPRADIALBLURBITMAP(pBitmap, uDim, uStress, CenterPt) \
      ((pL_RadialBlurBitmap )?pL_RadialBlurBitmap(pBitmap, uDim, uStress, CenterPt):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPSWIRLBITMAP(pBitmap, nAngle, CenterPt) \
      ((pL_SwirlBitmap )?pL_SwirlBitmap(pBitmap, nAngle, CenterPt):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPZOOMBLURBITMAP(pBitmap, uDim, uStress, CenterPt) \
      ((pL_ZoomBlurBitmap )?pL_ZoomBlurBitmap(pBitmap, uDim, uStress, CenterPt):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPIMPRESSIONISTBITMAP(pBitmap, uHorzDim, uVertDim) \
      ((pL_ImpressionistBitmap )?pL_ImpressionistBitmap(pBitmap, uHorzDim, uVertDim):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPREMOVEREDEYEBITMAP(pBitmap, rcNewColor, uThreshold, nLightness) \
      ((pL_RemoveRedeyeBitmap )?pL_RemoveRedeyeBitmap(pBitmap, rcNewColor, uThreshold, nLightness):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPMINFILTERBITMAP(pBitmap, uDim) \
      ((pL_MinFilterBitmap )?pL_MinFilterBitmap(pBitmap, uDim):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPMOSAICBITMAP(pBitmap, uDim) \
      ((pL_MosaicBitmap )?pL_MosaicBitmap(pBitmap, uDim):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPLINEPROFILE(pBitmap, FirstPoint, SecondPoint,pRed,pGreen,pBlue) \
      ((pL_LineProfile )?pL_LineProfile(pBitmap, FirstPoint, SecondPoint,pRed,pGreen,pBlue):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGRAYSCALEBITMAPEXT(pBitmap, RedFact, GreenFact, BlueFact) \
      ((pL_GrayScaleBitmapExt )?pL_GrayScaleBitmapExt(pBitmap, RedFact, GreenFact, BlueFact):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPBALANCECOLORS(pBitmap, pRedFact, pGreenFact, pBlueFact) \
      ((pL_BalanceColors )?pL_BalanceColors(pBitmap, pRedFact, pGreenFact, pBlueFact):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCONVERTTOCOLOREDGRAY(pBitmap, RedFact, GreenFact, BlueFact, RedGrayFact, GreenGrayFact, BlueGrayFact) \
      ((pL_ConvertToColoredGray )?pL_ConvertToColoredGray(pBitmap, RedFact, GreenFact, BlueFact, RedGrayFact, GreenGrayFact, BlueGrayFact):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPANTIALIASBITMAP(pBitmap, uThreshold, uDim, uFilter) \
      ((pL_AntiAliasBitmap )?pL_AntiAliasBitmap(pBitmap, uThreshold, uDim, uFilter):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPREMAPBITMAPHUE(pBitmap, pMask, pHTable, pSTable, pVTable, uLUTLen) \
      ((pL_RemapBitmapHue )?pL_RemapBitmapHue(pBitmap, pMask, pHTable, pSTable, pVTable, uLUTLen):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPWINDOWLEVELBITMAP(pBitmap, nLowBit, nHighBit, pLUT, uLUTLength, nOrderDst) \
      ((pL_WindowLevelBitmap )?pL_WindowLevelBitmap(pBitmap, nLowBit, nHighBit, pLUT, uLUTLength, nOrderDst):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPGAUSSIANFILTERBITMAP(pBitmap, nRadius) \
      ((pL_GaussianFilterBitmap )?pL_GaussianFilterBitmap(pBitmap, nRadius):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETUSERLOOKUPTABLE(pLookupTable, uLookupLen, apUserPoint, uUserPointCount, puPointCount) \
      ((pL_GetUserLookUpTable )?pL_GetUserLookUpTable(pLookupTable, uLookupLen, apUserPoint, uUserPointCount, puPointCount):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCONVERTBITMAPSIGNEDTOUNSIGNED(pBitmap, uShift) \
      ((pL_ConvertBitmapSignedToUnsigned )?pL_ConvertBitmapSignedToUnsigned(pBitmap, uShift):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPTEXTUREALPHABLENDBITMAP(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, pBitmapMask, nOpacity, pBitmapUnderlay, LPpOffset) \
      ((pL_TextureAlphaBlendBitmap )?pL_TextureAlphaBlendBitmap(pBitmapDst, nXDst, nYDst, nWidth, nHeight, pBitmapSrc, nXSrc, nYSrc, pBitmapMask, nOpacity, pBitmapUnderlay, LPpOffset):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPMULTIPLYBITMAP(pBitmap, uFactor) \
      ((pL_MultiplyBitmap )?pL_MultiplyBitmap(pBitmap, uFactor):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPLOCALHISTOEQUALIZEBITMAP(pBitmap, nWidth, nHeight, nxExt, nyExt, uType, uSmooth) \
      ((pL_LocalHistoEqualizeBitmap )?pL_LocalHistoEqualizeBitmap(pBitmap, nWidth, nHeight, nxExt, nyExt, uType, uSmooth):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPOILIFYBITMAP(pBitmap, uDim) \
      ((pL_OilifyBitmap )?pL_OilifyBitmap(pBitmap, uDim):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCONTOURFILTERBITMAP(pBitmap, nThreshold, nDeltaDirection, nMaximumError, nOption) \
      ((pL_ContourFilterBitmap )?pL_ContourFilterBitmap(pBitmap, nThreshold, nDeltaDirection, nMaximumError, nOption):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPMOTIONBLURBITMAP(pBitmap, uDim, nAngle, bUnidirectional) \
      ((pL_MotionBlurBitmap )?pL_MotionBlurBitmap(pBitmap, uDim, nAngle, bUnidirectional):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPPICTURIZEBITMAPLIST(pBitmap, uCellWidth, uCellHeight, uLightnessFact, hList) \
      ((pL_PicturizeBitmapList )?pL_PicturizeBitmapList(pBitmap, uCellWidth, uCellHeight, uLightnessFact, hList):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPPICTURIZEBITMAPSINGLE(pBitmap, pThumbBitmap, uCellWidth, uCellHeight, uLightnessFact) \
      ((pL_PicturizeBitmapSingle )?pL_PicturizeBitmapSingle(pBitmap, pThumbBitmap, uCellWidth, uCellHeight, uLightnessFact):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPADDBORDER(pBitmap, pAddBorderInfo) \
      ((pL_AddBorder )?pL_AddBorder(pBitmap, pAddBorderInfo):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPADDFRAME(pBitmap, pAddFrameInfo) \
      ((pL_AddFrame )?pL_AddFrame(pBitmap, pAddFrameInfo):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPADDMESSAGETOBITMAP(pBitmap, pAddMesgInfo) \
      ((pL_AddMessageToBitmap )?pL_AddMessageToBitmap(pBitmap, pAddMesgInfo):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPEXTRACTMESSAGEFROMBITMAP(pBitmap, pAddMesgInfo) \
      ((pL_ExtractMessageFromBitmap )?pL_ExtractMessageFromBitmap(pBitmap, pAddMesgInfo):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPHALFTONEBITMAP(pBitmap, uType, nAngle, uDim, hList) \
      ((pL_HalfToneBitmap )?pL_HalfToneBitmap(pBitmap, uType, nAngle, uDim, hList):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPBUMPMAPBITMAP(pBitmapDst, pBumpBitmap, pBumpData) \
      ((pL_BumpMapBitmap )?pL_BumpMapBitmap(pBitmapDst, pBumpBitmap, pBumpData):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPGLOWFILTERBITMAP(pBitmap, uDim, uBright, uThreshold) \
      ((pL_GlowFilterBitmap )?pL_GlowFilterBitmap(pBitmap, uDim, uBright, uThreshold):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPEDGEDETECTSTATISTICALBITMAP(pBitmap, uDim, uThreshold, crEdgeColor, crBkColor) \
      ((pL_EdgeDetectStatisticalBitmap )?pL_EdgeDetectStatisticalBitmap(pBitmap, uDim, uThreshold, crEdgeColor, crBkColor):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPDESATURATEBITMAP(pBitmap) \
      ((pL_DesaturateBitmap )?pL_DesaturateBitmap(pBitmap):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPSMOOTHEDGESBITMAP(pBitmap, nAmount, nThreshold) \
      ((pL_SmoothEdgesBitmap )?pL_SmoothEdgesBitmap(pBitmap, nAmount, nThreshold):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPAUTOBINARYBITMAP(pBitmap) \
      ((pL_AutoBinaryBitmap )?pL_AutoBinaryBitmap(pBitmap):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCHANNELMIX(pBitmap, pRedFactor, pGreenFactor, pBlueFactor) \
      ((pL_ChannelMix )?pL_ChannelMix(pBitmap, pRedFactor, pGreenFactor, pBlueFactor):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPOCEANBITMAP(pBitmap, uAmplitude, uFrequency, bLowerTrnsp) \
      ((pL_OceanBitmap )?pL_OceanBitmap(pBitmap, uAmplitude, uFrequency, bLowerTrnsp):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPLIGHTBITMAP(pBitmap, pLightInfo, uLightNo, uBright, uAmbient, crAmbientClr) \
      ((pL_LightBitmap )?pL_LightBitmap(pBitmap, pLightInfo, uLightNo, uBright, uAmbient, crAmbientClr):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDRYBITMAP(pBitmap, uDim) \
      ((pL_DryBitmap )?pL_DryBitmap(pBitmap, uDim):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDRAWSTARBITMAP(pBitmap, pStarInfo) \
      ((pL_DrawStarBitmap )?pL_DrawStarBitmap(pBitmap, pStarInfo):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPFRQFILTERMASKBITMAP(pMaskBitmap, pFTArray, bOnOff) \
      ((pL_FrqFilterMaskBitmap )?pL_FrqFilterMaskBitmap(pMaskBitmap, pFTArray, bOnOff):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPALLOCFTARRAY(pBitmap, ppFTArray, uStructSize) \
      ((pL_AllocFTArray )?pL_AllocFTArray(pBitmap, ppFTArray, uStructSize):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPFREEFTARRAY(pFTArray) \
      ((pL_FreeFTArray )?pL_FreeFTArray(pFTArray):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSKELETONBITMAP(pBitmap, uThreshold) \
      ((pL_SkeletonBitmap )?pL_SkeletonBitmap(pBitmap, uThreshold):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSELECTIVECOLORBITMAP(pBitmap, pSelClr) \
      ((pL_SelectiveColorBitmap )?pL_SelectiveColorBitmap(pBitmap, pSelClr):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETOBJECTINFO(pBitmap, puX, puY, pnAngle, puRoundness, bWeighted) \
      ((pL_GetObjectInfo )?pL_GetObjectInfo(pBitmap, puX, puY, pnAngle, puRoundness, bWeighted):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETRGNPERIMETERLENGTH(pBitmap, pXForm, puLength) \
      ((pL_GetRgnPerimeterLength )?pL_GetRgnPerimeterLength(pBitmap, pXForm, puLength):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETFERETSDIAMETER(pPoints, uSize, puFeretsDiameter, puFirstIndex, puSecondIndex) \
      ((pL_GetFeretsDiameter )?pL_GetFeretsDiameter(pPoints, uSize, puFeretsDiameter, puFirstIndex, puSecondIndex):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCOLORREPLACEBITMAP(pBitmap, pColorReplace, uColorCount, nHue, nSaturation, nBrightness) \
      ((pL_ColorReplaceBitmap )?pL_ColorReplaceBitmap(pBitmap, pColorReplace, uColorCount, nHue, nSaturation, nBrightness):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCHANGEHUESATINTBITMAP(pBitmap, nHue, nSaturation, nIntensity, pHsiData, uHsiDataCount) \
      ((pL_ChangeHueSatIntBitmap )?pL_ChangeHueSatIntBitmap(pBitmap, nHue, nSaturation, nIntensity, pHsiData, uHsiDataCount):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETBITMAPSTATISTICSINFO(pBitmap, pStatisticsInfo, uChannel, uStart, uEnd) \
      ((pL_GetBitmapStatisticsInfo )?pL_GetBitmapStatisticsInfo(pBitmap, pStatisticsInfo, uChannel, uStart, uEnd):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCOLORREPLACEWEIGHTSBITMAP(pBitmap, pColorReplace, uColorCount) \
      ((pL_ColorReplaceWeightsBitmap )?pL_ColorReplaceWeightsBitmap(pBitmap, pColorReplace, uColorCount):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPDIRECTIONEDGESTATISTICALBITMAP(pBitmap, uDim, uThreshold, nAngle, crEdgeColor, crBkColor) \
      ((pL_DirectionEdgeStatisticalBitmap )?pL_DirectionEdgeStatisticalBitmap(pBitmap, uDim, uThreshold, nAngle, crEdgeColor, crBkColor):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPMATHFUNCTIONBITMAP(pBitmap, uMType, uFactor) \
      ((pL_MathFunctionBitmap )?pL_MathFunctionBitmap(pBitmap, uMType, uFactor):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOLORTHRESHOLDBITMAP(pBitmap, uColorSpace, pCompData) \
      ((pL_ColorThresholdBitmap )?pL_ColorThresholdBitmap(pBitmap, uColorSpace, pCompData):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPREVEFFECTBITMAP(pBitmap, uLineSpace, uMaximumHeight) \
      ((pL_RevEffectBitmap )?pL_RevEffectBitmap(pBitmap, uLineSpace, uMaximumHeight):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDYNAMICBINARYBITMAP(pBitmap, uDim, uLocalContrast) \
      ((pL_DynamicBinaryBitmap )?pL_DynamicBinaryBitmap(pBitmap, uDim, uLocalContrast):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOLORINTENSITYBALANCE(pBitmap, pShadows, pMidTone, pHighLight, bLuminance) \
      ((pL_ColorIntensityBalance )?pL_ColorIntensityBalance(pBitmap, pShadows, pMidTone, pHighLight, bLuminance):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPFUNCTIONALLIGHTBITMAP(pBitmap, pLightParams) \
      ((pL_FunctionalLightBitmap )?pL_FunctionalLightBitmap(pBitmap, pLightParams):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPSHIFTBITMAPDATA(pDstBitmap, pSrcBitmap, uSrcLowBit, uSrcHighBit, uDstLowBit, uDstBitsPerPixel) \
      ((pL_ShiftBitmapData )?pL_ShiftBitmapData(pDstBitmap, pSrcBitmap, uSrcLowBit, uSrcHighBit, uDstLowBit, uDstBitsPerPixel):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSELECTBITMAPDATA(pDstBitmap, pSrcBitmap, crColor, uSrcLowBit, uSrcHighBit, uThreshold, bCombine) \
      ((pL_SelectBitmapData )?pL_SelectBitmapData(pDstBitmap, pSrcBitmap, crColor, uSrcLowBit, uSrcHighBit, uThreshold, bCombine):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCOLORIZEGRAYBITMAP(pDstBitmap, pSrcBitmap, pGrayColors, uCount) \
      ((pL_ColorizeGrayBitmap )?pL_ColorizeGrayBitmap(pDstBitmap, pSrcBitmap, pGrayColors, uCount):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCONTBRIGHTINTBITMAP(pBitmap, nContrast, nBrightness, nIntensity) \
      ((pL_ContBrightIntBitmap )?pL_ContBrightIntBitmap(pBitmap, nContrast, nBrightness, nIntensity):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPISREGMARKBITMAP(pBitmap, uType, uMinScale, uMaxScale, uWidth, uHeight) \
      ((pL_IsRegMarkBitmap )?pL_IsRegMarkBitmap(pBitmap, uType, uMinScale, uMaxScale, uWidth, uHeight):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPGETMARKSCENTERMASSBITMAP(pBitmap, pMarkPoints, pMarkCMPoints, uMarksCount) \
      ((pL_GetMarksCenterMassBitmap )?pL_GetMarksCenterMassBitmap(pBitmap, pMarkPoints, pMarkCMPoints, uMarksCount):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSEARCHREGMARKSBITMAP(pBitmap, pSearchMarks, uMarkCount) \
      ((pL_SearchRegMarksBitmap )?pL_SearchRegMarksBitmap(pBitmap, pSearchMarks, uMarkCount):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPGETTRANSFORMATIONPARAMETERS(pBitmap, pRefPoints, pTrnsPoints, pnXTranslation, pnYTranslation, pnAngle, puXScale, puYScale) \
      ((pL_GetTransformationParameters )?pL_GetTransformationParameters(pBitmap, pRefPoints, pTrnsPoints, pnXTranslation, pnYTranslation, pnAngle, puXScale, puYScale):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPFRAGMENTBITMAP(pBitmap, uOffset, uOpacity) \
      ((pL_FragmentBitmap )?pL_FragmentBitmap(pBitmap, uOffset, uOpacity):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPVIGNETTEBITMAP(pBitmap, pVignetteInfo) \
      ((pL_VignetteBitmap )?pL_VignetteBitmap(pBitmap, pVignetteInfo):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPMOSAICTILESBITMAP(pBitmap, pMosaicTilesInfo) \
      ((pL_MosaicTilesBitmap )?pL_MosaicTilesBitmap(pBitmap, pMosaicTilesInfo):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPLASMAFILTERBITMAP(pBitmap,  pPlasmaInfo) \
      ((pL_PlasmaFilterBitmap )?pL_PlasmaFilterBitmap(pBitmap,  pPlasmaInfo):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPADJUSTBITMAPTINT(pBitmap, nAngleA, nAngleB) \
      ((pL_AdjustBitmapTint )?pL_AdjustBitmapTint(pBitmap, nAngleA, nAngleB):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOLOREDPENCILBITMAP(pBitmap, uRatio, uDim) \
      ((pL_ColoredPencilBitmap )?pL_ColoredPencilBitmap(pBitmap, uRatio, uDim):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDIFFUSEGLOWBITMAP(pBitmap, nGlowAmount, uClearAmount, uSpreadAmount, uWhiteNoise, crGlowColor) \
      ((pL_DiffuseGlowBitmap )?pL_DiffuseGlowBitmap(pBitmap, nGlowAmount, uClearAmount, uSpreadAmount, uWhiteNoise, crGlowColor):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPHIGHPASSFILTERBITMAP(pBitmap, uRadius, uOpacity) \
      ((pL_HighPassFilterBitmap )?pL_HighPassFilterBitmap(pBitmap, uRadius, uOpacity):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCOLORHALFTONEBITMAP(pBitmap, uMaxRad, nCyanAngle, nMagentaAngle, nYellowAngle, nBlackAngle) \
      ((pL_ColorHalfToneBitmap )?pL_ColorHalfToneBitmap(pBitmap, uMaxRad, nCyanAngle, nMagentaAngle, nYellowAngle, nBlackAngle):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPCORRELATIONLISTBITMAP(pBitmap, hCorList, pPoints, puListIndex, uMaxPoints, puNumOfPoints, uXStep, uYStep, uThreshold) \
      ((pL_CorrelationListBitmap )?pL_CorrelationListBitmap(pBitmap, hCorList, pPoints, puListIndex, uMaxPoints, puNumOfPoints, uXStep, uYStep, uThreshold):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSETKAUFMANNRGNBITMAP(pBitmap, pKaufmannProcBitmap, nRadius, nMinInput, nMaxInput, nRgnThreshold, ptRgnStart, bRemoveHoles, puPixelsCount, uCombineMode) \
      ((pL_SetKaufmannRgnBitmap )?pL_SetKaufmannRgnBitmap(pBitmap, pKaufmannProcBitmap, nRadius, nMinInput, nMaxInput, nRgnThreshold, ptRgnStart, bRemoveHoles, puPixelsCount, uCombineMode):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSHIFTMINIMUMTOZERO(pBitmap, puShiftAmount) \
      ((pL_ShiftMinimumToZero )?pL_ShiftMinimumToZero(pBitmap, puShiftAmount):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSHIFTZEROTONEGATIVE(pBitmap, nShiftAmount, nMinInput, nMaxInput, nMinOutput, nMaxOutput) \
      ((pL_ShiftZeroToNegative )?pL_ShiftZeroToNegative(pBitmap, nShiftAmount, nMinInput, nMaxInput, nMinOutput, nMaxOutput):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPHISTOEQUALIZEBITMAP(pBitmap, nFlag) \
      ((pL_HistoEqualizeBitmap )?pL_HistoEqualizeBitmap(pBitmap, nFlag):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPSWAPCOLORS(pBitmap, nFlags) \
      ((pL_SwapColors )?pL_SwapColors(pBitmap, nFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCYLINDRICALBITMAP(pBitmap, nValue, uType) \
      ((pL_CylindricalBitmap )?pL_CylindricalBitmap(pBitmap, nValue, uType):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPCORRELATIONBITMAP(pBitmap, pCorBitmap, pPoints, uMaxPoints, puNumOfPoints, uXStep, uYStep, uThreshold) \
      ((pL_CorrelationBitmap )?pL_CorrelationBitmap(pBitmap, pCorBitmap, pPoints, uMaxPoints, puNumOfPoints, uXStep, uYStep, uThreshold):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#endif

#define L_WRPUNSHARPMASKBITMAP(pBitmap, nAmount, nRadius, nThreshold, uFlags) \
      ((pL_UnsharpMaskBitmap )?pL_UnsharpMaskBitmap(pBitmap, nAmount, nRadius, nThreshold, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPDESKEWBITMAP(pBitmap, pnAngle, crBack, uFlags) \
      ((pL_DeskewBitmap )?pL_DeskewBitmap(pBitmap, pnAngle, crBack, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPDESKEWCHECKBITMAP(pBitmap, pnAngle, crBack, uFlags) \
   ((pL_DeskewCheckBitmap )?pL_DeskewCheckBitmap(pBitmap, pnAngle, crBack, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPBLANKPAGEDETECTORBITMAP(pBitmap, bIsBlank, pAccuracy, PMargins, uFlags) \
      ((pL_BlankPageDetectorBitmap )?pL_BlankPageDetectorBitmap(pBitmap, bIsBlank, pAccuracy, PMargins, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPAUTOBINARIZEBITMAP(pBitmap, uFactor, uFlags) \
   ((pL_AutoBinarizeBitmap )?pL_AutoBinarizeBitmap(pBitmap, uFactor, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSTARTFASTMAGICWAND(pMagicWnd, m_pBitmapHandle, uFlags) \
   ((pL_StartFastMagicWandEngine )?pL_StartFastMagicWandEngine(pMagicWnd, m_pBitmapHandle, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPENDFASTMAGICWAND(MagicWnd, uFlags) \
   ((pL_EndFastMagicWandEngine )?pL_EndFastMagicWandEngine(MagicWnd, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPFASTMAGICWAND(MagicWnd, nTolerance, nXposition, nYposition, pObjectInfo, uFlags) \
   ((pL_FastMagicWand )?pL_FastMagicWand(MagicWnd, nTolerance, nXposition, nYposition, pObjectInfo, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPDELETEOBJECTINFO(pObjectInfo, uFlags) \
   ((pL_DeleteObjectInfo )?pL_DeleteObjectInfo(pObjectInfo, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPTISSUEEQUALIZEBITMAP(pBitmap, uFlags) \
   ((pL_TissueEqualizeBitmap )?pL_TissueEqualizeBitmap(pBitmap,  uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSIGMAFILTERBITMAP(pBitmap, nSize, nSigma, nThreshhold, bOutline,  uFlags) \
   ((pL_SigmaFilterBitmap )?pL_SigmaFilterBitmap(pBitmap, nSize, nSigma, nThreshhold, bOutline,  uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPAUTOSEGMENTBITMAP(pBitmap, prcRect, uFlags) \
   ((pL_AutoSegmentBitmap )?pL_AutoSegmentBitmap(pBitmap, prcRect, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPDESKEWBITMAPEXT(pBitmap, pnAngle, uAngleRange, uAngleResolution, crBack, uFlags) \
      ((pL_DeskewBitmapExt )?pL_DeskewBitmapExt(pBitmap, pnAngle, uAngleRange, uAngleResolution, crBack, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)


#define L_WRPRESIZEBITMAPRGN(pBitmap, uDim, uFlag,bAsFrame) \
      ((pL_ResizeBitmapRgn )?pL_ResizeBitmapRgn(pBitmap, uDim, uFlag,bAsFrame):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPDIGITALSUBTRACTBITMAP(pBitmap, pMaskBitmap, uFlags) \
      ((pL_DigitalSubtractBitmap )?pL_DigitalSubtractBitmap(pBitmap, pMaskBitmap, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCREATEFADEDMASK(pBitmap, pMaskBitmap, uStructSize, nLength, nFadeRate, nStepSize, nInflate, uFlag, nMaxGray, crTransparent) \
      ((pL_CreateFadedMask )?pL_CreateFadedMask(pBitmap, pMaskBitmap, uStructSize, nLength, nFadeRate, nStepSize, nInflate, uFlag, nMaxGray, crTransparent):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPPUZZLEEFFECTBITMAP(pBitmap, uXBlock, uYBlock, uRandomize, uFlags, crColor) \
      ((pL_PuzzleEffectBitmap )?pL_PuzzleEffectBitmap(pBitmap, uXBlock, uYBlock, uRandomize, uFlags, crColor):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDICEEFFECTBITMAP(pBitmap, uXBlock, uYBlock, uRandomize, uFlags, crColor) \
      ((pL_DiceEffectBitmap )?pL_DiceEffectBitmap(pBitmap, uXBlock, uYBlock, uRandomize, uFlags, crColor):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPICTURIZEBITMAP(pBitmap, pszDirName, uFlags, nCellWidth, nCellHeight, pfnCallback, pUserData) \
      ((pL_PicturizeBitmap )?pL_PicturizeBitmap(pBitmap, pszDirName, uFlags, nCellWidth, nCellHeight, pfnCallback, pUserData):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPREMAPBITMAPINTENSITY(pBitmap, pLUT, uLUTLen, uChannel) \
      ((pL_RemapBitmapIntensity )?pL_RemapBitmapIntensity(pBitmap, pLUT, uLUTLen, uChannel):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOLORMERGEBITMAP(pBitmap, ppBitmap, uStructSize, uFlags) \
      ((pL_ColorMergeBitmap )?pL_ColorMergeBitmap(pBitmap, ppBitmap, uStructSize, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCOLORSEPARATEBITMAP(pBitmap, ppBitmap, uStructSize, uFlags) \
      ((pL_ColorSeparateBitmap )?pL_ColorSeparateBitmap(pBitmap, ppBitmap, uStructSize, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETBITMAPHISTOGRAM(pBitmap, pHisto, uHistoLen, uFlags) \
      ((pL_GetBitmapHistogram )?pL_GetBitmapHistogram(pBitmap, pHisto, uHistoLen, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETFUNCTIONALLOOKUPTABLE(pLookupTable, uLookupLen, nStart, nEnd, nFactor, uFlags) \
      ((pL_GetFunctionalLookupTable )?pL_GetFunctionalLookupTable(pLookupTable, uLookupLen, nStart, nEnd, nFactor, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPGETCURVEPOINTS(pCurve, apUserPoint, uUserPointCount, puPointCount, uFlag) \
      ((pL_GetCurvePoints )?pL_GetCurvePoints(pCurve, apUserPoint, uUserPointCount, puPointCount, uFlag):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPADDBITMAPS(pBitmap, uStructSize, hList, uFlag) \
      ((pL_AddBitmaps )?pL_AddBitmaps(pBitmap, uStructSize, hList, uFlag):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPADDWEIGHTEDBITMAPS(pBitmap, uStructSize, hList, puFactor, uFlag) \
      ((pL_AddWeightedBitmaps )?pL_AddWeightedBitmaps(pBitmap, uStructSize, hList, puFactor, uFlag):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPPIXELATEBITMAP(pBitmap, uCellWidth, uCellHeight, uOpacity, CenterPt, uFlags) \
      ((pL_PixelateBitmap )?pL_PixelateBitmap(pBitmap, uCellWidth, uCellHeight, uOpacity, CenterPt, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPSPHERIZEBITMAP(pBitmap, nValue, CenterPt, crFill, uFlags) \
      ((pL_SpherizeBitmap )?pL_SpherizeBitmap(pBitmap, nValue, CenterPt, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPBENDINGBITMAP(pBitmap, nValue, CenterPt, crFill, uFlags) \
      ((pL_BendingBitmap )?pL_BendingBitmap(pBitmap, nValue, CenterPt, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPUNCHBITMAP(pBitmap, nValue, uStress, CenterPt, crFill, uFlags) \
      ((pL_PunchBitmap )?pL_PunchBitmap(pBitmap, nValue, uStress, CenterPt, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPOLARBITMAP(pBitmap, crFill, uFlags) \
      ((pL_PolarBitmap )?pL_PolarBitmap(pBitmap, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPRIPPLEBITMAP(pBitmap, uAmplitude, uFrequency, nPhase, uAttenuation, CenterPt, crFill, uFlag) \
      ((pL_RippleBitmap )?pL_RippleBitmap(pBitmap, uAmplitude, uFrequency, nPhase, uAttenuation, CenterPt, crFill, uFlag):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPFREEHANDWAVEBITMAP(pBitmap, pAmplitudes, uAmplitudesCount, uScale, uWaveLen, nAngle, crFill, uFlags) \
      ((pL_FreeHandWaveBitmap )?pL_FreeHandWaveBitmap(pBitmap, pAmplitudes, uAmplitudesCount, uScale, uWaveLen, nAngle, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPRADWAVEBITMAP(pBitmap, uAmplitude, uWaveLen, nPhase, pCenter, crFill, uFlag) \
      ((pL_RadWaveBitmap )?pL_RadWaveBitmap(pBitmap, uAmplitude, uWaveLen, nPhase, pCenter, crFill, uFlag):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPFREEHANDSHEARBITMAP(pBitmap, pAmplitudes, uAmplitudesCount, uScale, crFill, uFlags) \
      ((pL_FreeHandShearBitmap )?pL_FreeHandShearBitmap(pBitmap, pAmplitudes, uAmplitudesCount, uScale, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPZOOMWAVEBITMAP(pBitmap, uAmplitude, uFrequency, nPhase, uZomFact, pCenter, crFill, uFlag) \
         ((pL_ZoomWaveBitmap )?pL_ZoomWaveBitmap(pBitmap, uAmplitude, uFrequency, nPhase, uZomFact, pCenter, crFill, uFlag):WRPERR_LTIMGSFX_DLL_NOT_LOADED) \

#define L_WRPWAVEBITMAP(pBitmap, uAmplitude, uWaveLen, nAngle, uHorzFact, uVertFact, crFill, uFlags) \
      ((pL_WaveBitmap )?pL_WaveBitmap(pBitmap, uAmplitude, uWaveLen, nAngle, uHorzFact, uVertFact, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

//*** v14 functions **
#define L_WRPDEINTERLACEBITMAP(pBitmap, uFlags) \
      ((pL_DeinterlaceBitmap )?pL_DeinterlaceBitmap(pBitmap, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSAMPLETARGETBITMAP(pBitmap, crSample, crTarget, uFlags) \
      ((pL_SampleTargetBitmap )?pL_SampleTargetBitmap(pBitmap, crSample, crTarget, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPCUBISMBITMAP(pBitmap, uSpace, uLength, nBrightness, nAngle, crColor, uFlags) \
      ((pL_CubismBitmap )?pL_CubismBitmap(pBitmap, uSpace, uLength, nBrightness, nAngle, crColor, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPLIGHTCONTROLBITMAP(pBitmap, puLowerAvr, puAvrage, puUpperAvr, uFlag) \
      ((pL_LightControlBitmap )?pL_LightControlBitmap(pBitmap, puLowerAvr, puAvrage, puUpperAvr, uFlag):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGLASSEFFECTBITMAP(pBitmap, uCellWidth, uCellHeight, uFlags) \
      ((pL_GlassEffectBitmap )?pL_GlassEffectBitmap(pBitmap, uCellWidth, uCellHeight, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPLENSFLAREBITMAP(pBitmap, ptCenter, uBright, uFlag, crColor) \
      ((pL_LensFlareBitmap )?pL_LensFlareBitmap(pBitmap, ptCenter, uBright, uFlag, crColor):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPLANEBITMAP(pBitmap, ptCenterPoint, uZValue, nDistance, uPlaneOffset, nRepeat, nPydAngle, uStretch, uStartBright, uEndBright, uBrightLength, crBright, crFill, uFlags) \
      ((pL_PlaneBitmap )?pL_PlaneBitmap(pBitmap, ptCenterPoint, uZValue, nDistance, uPlaneOffset, nRepeat, nPydAngle, uStretch, uStartBright, uEndBright, uBrightLength, crBright, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPLANEBENDBITMAP(pBitmap, ptCenterPoint, uZValue, nDistance, uPlaneOffset, nRepeat, nPydAngle, uStretch, uBendFactor, uStartBright, uEndBright, uBrightLength, crBright, crFill, uFlags) \
      ((pL_PlaneBendBitmap )?pL_PlaneBendBitmap(pBitmap, ptCenterPoint, uZValue, nDistance, uPlaneOffset, nRepeat, nPydAngle, uStretch, uBendFactor, uStartBright, uEndBright, uBrightLength, crBright, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPTUNNELBITMAP(pBitmap, ptCenterPoint, uZValue, nDistance, uRad, nRepeat, nRotationOffset, uStretch, uStartBright, uEndBright, uBrightLength, crBright, crFill, uFlags) \
      ((pL_TunnelBitmap )?pL_TunnelBitmap(pBitmap, ptCenterPoint, uZValue, nDistance, uRad, nRepeat, nRotationOffset, uStretch, uStartBright, uEndBright, uBrightLength, crBright, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPFREERADBENDBITMAP(pBitmap, pnCurve, uCurveSize, uScale, CenterPt, crFill, uFlags) \
      ((pL_FreeRadBendBitmap )?pL_FreeRadBendBitmap(pBitmap, pnCurve, uCurveSize, uScale, CenterPt, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPFREEPLANEBENDBITMAP(pBitmap, pnCurve, uCurveSize, uScale, crFill, uFlags) \
      ((pL_FreePlaneBendBitmap )?pL_FreePlaneBendBitmap(pBitmap, pnCurve, uCurveSize, uScale, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPFFTBITMAP(pBitmap, pFTArray, uFlags) \
      ((pL_FFTBitmap )?pL_FFTBitmap(pBitmap, pFTArray, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPFTDISPLAYBITMAP(pBitmap, pFTArray, uFlags) \
      ((pL_FTDisplayBitmap )?pL_FTDisplayBitmap(pBitmap, pFTArray, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPDFTBITMAP(pBitmap, pFTArray, prcRange, uFlags) \
      ((pL_DFTBitmap )?pL_DFTBitmap(pBitmap, pFTArray, prcRange, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPFRQFILTERBITMAP(pFTArray, LPprcRange, uFlags) \
      ((pL_FrqFilterBitmap )?pL_FrqFilterBitmap(pFTArray, LPprcRange, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPGRAYSCALETODUOTONE(pBitmap, pNewColor, crColor, uFlags) \
      ((pL_GrayScaleToDuotone )?pL_GrayScaleToDuotone(pBitmap, pNewColor, crColor, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGRAYSCALETOMULTITONE(pBitmap, uToneType, uDistType, LPpColor, pGradient, uFlags) \
      ((pL_GrayScaleToMultitone )?pL_GrayScaleToMultitone(pBitmap, uToneType, uDistType, LPpColor, pGradient, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)




#define L_WRPCOLORLEVELBITMAP(pBitmap, pLvlClr, uFlags) \
      ((pL_ColorLevelBitmap )?pL_ColorLevelBitmap(pBitmap, pLvlClr, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPAUTOCOLORLEVELBITMAP(pBitmap, pLvlClr, uBlackClip, uWhiteClip, uFlags) \
      ((pL_AutoColorLevelBitmap )?pL_AutoColorLevelBitmap(pBitmap, pLvlClr, uBlackClip, uWhiteClip, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPGETRGNCONTOURPOINTS(pBitmap, pXForm, ppPoints, puSize, uFlag) \
      ((pL_GetRgnContourPoints )?pL_GetRgnContourPoints(pBitmap, pXForm, ppPoints, puSize,  uFlag):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPSEGMENTBITMAP(pBitmap, uThreshold, uFlag) \
      ((pL_SegmentBitmap )?pL_SegmentBitmap(pBitmap, uThreshold, uFlag):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPADDSHADOWBITMAP(pBitmap, uAngle, uThreshold, uFlags) \
      ((pL_AddShadowBitmap )?pL_AddShadowBitmap(pBitmap, uAngle, uThreshold, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPSUBTRACTBACKGROUNDBITMAP(pBitmap, uRollingBall, uShrinkSize, uBrightnessFactor, uFlags) \
      ((pL_SubtractBackgroundBitmap )?pL_SubtractBackgroundBitmap(pBitmap, uRollingBall, uShrinkSize, uBrightnessFactor, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPAPPLYMODALITYLUT(pBitmap, pLUT, pLUTDescriptor, uFlags) \
      ((pL_ApplyModalityLUT )?pL_ApplyModalityLUT(pBitmap, pLUT, pLUTDescriptor, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPAPPLYLINEARMODALITYLUT(pBitmap, fIntercept, fSlope, uFlags) \
      ((pL_ApplyLinearModalityLUT )?pL_ApplyLinearModalityLUT(pBitmap, fIntercept, fSlope, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPAPPLYVOILUT(pBitmap, pLUT, pLUTDescriptor, uFlags) \
      ((pL_ApplyVOILUT )?pL_ApplyVOILUT(pBitmap, pLUT, pLUTDescriptor, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPAPPLYLINEARVOILUT(pBitmap, fCenter, fWidth, uFlags) \
      ((pL_ApplyLinearVOILUT )?pL_ApplyLinearVOILUT(pBitmap, fCenter, fWidth, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPGETLINEARVOILUT(pBitmap, pCenter, pWidth, uFlags) \
      ((pL_GetLinearVOILUT )?pL_GetLinearVOILUT(pBitmap, pCenter, pWidth, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCOUNTLUTCOLORS(pLUT, ulLLUTLen, pNumberOfEntries, pFirstIndex, uFlags) \
      ((pL_CountLUTColors )?pL_CountLUTColors(pLUT, ulLLUTLen, pNumberOfEntries, pFirstIndex, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#ifdef LEADTOOLS_V16_OR_LATER
#define L_WRPCOUNTLUTCOLORSEXT(pLUT, ulLLUTLen, pNumberOfEntries, pFirstIndex, uFlags) \
      ((pL_CountLUTColorsExt)?pL_CountLUTColorsExt(pLUT, ulLLUTLen, pNumberOfEntries, pFirstIndex, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)
#endif // #ifdef LEADTOOLS_V16_OR_LATER

#define L_WRPADAPTIVECONTRASTBITMAP(pBitmap, uDim, uAmount, uFlag) \
      ((pL_AdaptiveContrastBitmap )?pL_AdaptiveContrastBitmap(pBitmap, uDim, uAmount, uFlag):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPAGINGBITMAP(pBitmap, uHScratchCount, uVScratchCount, uMaxScratchLen, uDustDensity, uPitsDensity, uMaxPitSize, crScratch, crDust, crPits, uFlags) \
      ((pL_AgingBitmap )?pL_AgingBitmap(pBitmap, uHScratchCount, uVScratchCount, uMaxScratchLen, uDustDensity, uPitsDensity, uMaxPitSize, crScratch, crDust, crPits, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPAPPLYMATHLOGICBITMAP(pBitmap, nFactor, uFlags) \
      ((pL_ApplyMathLogicBitmap )?pL_ApplyMathLogicBitmap(pBitmap, nFactor, uFlags):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPEDGEDETECTEFFECTBITMAP(pBitmap, uLevel, uThreshold, uFlags) \
      ((pL_EdgeDetectEffectBitmap )?pL_EdgeDetectEffectBitmap(pBitmap, uLevel, uThreshold, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPRINGEFFECTBITMAP(pBitmap, nXOrigin, nYOrigin, uRadius, uRingCount, uRandomize, crColor, nAngle, uFlags) \
      ((pL_RingEffectBitmap )?pL_RingEffectBitmap(pBitmap, nXOrigin, nYOrigin, uRadius, uRingCount, uRandomize, crColor, nAngle, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPMULTISCALEENHANCEMENTBITMAP(pBitmap, uContrast, uEdgeLevels, uEdgeCoeff, uLatitudeLevels, uLatitudeCoeff, uFlags) \
      ((pL_MultiScaleEnhancementBitmap )?pL_MultiScaleEnhancementBitmap(pBitmap, uContrast, uEdgeLevels, uEdgeCoeff, uLatitudeLevels, uLatitudeCoeff, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)


#define L_WRPAPPLYTRANSFORMATIONPARAMETERS(pBitmap, nXTranslation, nYTranslation, nAngle, uXScale, uYScale, uFlags) \
      ((pL_ApplyTransformationParameters )?pL_ApplyTransformationParameters(pBitmap, nXTranslation, nYTranslation, nAngle, uXScale, uYScale, uFlags):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCONVERTBITMAPUNSIGNEDTOSIGNED(pBitmap, uFlag) \
      ((pL_ConvertBitmapUnsignedToSigned )?pL_ConvertBitmapUnsignedToSigned(pBitmap, uFlag):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPOFFSETBITMAP(pBitmap, nHorizontalShift, nVerticalShift, crBackColor, uFlag) \
      ((pL_OffsetBitmap )?pL_OffsetBitmap(pBitmap, nHorizontalShift, nVerticalShift, crBackColor, uFlag):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPBRICKSTEXTUREBITMAP(pBitmap, uBricksWidth, uBricksHeight, uOffsetX, uOffsetY, uEdgeWidth, uMortarWidth, uShadeAngle, uRowDifference, uMortarRoughness, uMortarRoughnessEevenness, uBricksRoughness, uBricksRoughnessEevenness, crMortarColor, uFlags) \
      ((pL_BricksTextureBitmap )?pL_BricksTextureBitmap(pBitmap, uBricksWidth, uBricksHeight, uOffsetX, uOffsetY, uEdgeWidth, uMortarWidth, uShadeAngle, uRowDifference, uMortarRoughness, uMortarRoughnessEevenness, uBricksRoughness, uBricksRoughnessEevenness, crMortarColor, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPCANVASBITMAP(pBitmap, pThumbBitmap, uTransparency, uEmboss, nXOffset, nYOffset, uTilesOffset, uFlags) \
      ((pL_CanvasBitmap )?pL_CanvasBitmap(pBitmap, pThumbBitmap, uTransparency, uEmboss, nXOffset, nYOffset, uTilesOffset, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPCLOUDSBITMAP(pBitmap, uSeed, uFrequency, uDensity, uOpacity, cBackColor, crCloudsColor, uFlag) \
      ((pL_CloudsBitmap )?pL_CloudsBitmap(pBitmap, uSeed, uFrequency, uDensity, uOpacity, cBackColor, crCloudsColor, uFlag):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPROMANMOSAICBITMAP(pBitmap, uTileWidth, uTileHeight, uBorder, uShadowAngle, uShadowThresh, crColor, uFlags) \
      ((pL_RomanMosaicBitmap )?pL_RomanMosaicBitmap(pBitmap, uTileWidth, uTileHeight, uBorder, uShadowAngle, uShadowThresh, crColor, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPGAMMACORRECTBITMAPEXT(pBitmap, uGamma, uFlag) \
      ((pL_GammaCorrectBitmapExt )?pL_GammaCorrectBitmapExt(pBitmap, uGamma, uFlag):WRPERR_LTIMGCLR_DLL_NOT_LOADED)

#define L_WRPMASKCONVOLUTIONBITMAP(pBitmap, nAngle, uDepth, uHeight, uFlag) \
      ((pL_MaskConvolutionBitmap )?pL_MaskConvolutionBitmap(pBitmap, nAngle, uDepth, uHeight, uFlag):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPPERLINBITMAP(pBitmap, uSeed, uFrequency, uDensity, uOpacity, cBackColor, Perlin, nxCircle, nyCircle, nFreqLayout, nDenLayout, uFlag) \
      ((pL_PerlinBitmap )?pL_PerlinBitmap(pBitmap, uSeed, uFrequency, uDensity, uOpacity, cBackColor, Perlin, nxCircle, nyCircle, nFreqLayout, nDenLayout, uFlag):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPDISPLACEMAPBITMAP(pBitmap, pDisplacementMap, uHorzFact, uVertFact, crFill, uFlags) \
      ((pL_DisplaceMapBitmap )?pL_DisplaceMapBitmap(pBitmap, pDisplacementMap, uHorzFact, uVertFact, crFill, uFlags):WRPERR_LTIMGEFX_DLL_NOT_LOADED)

#define L_WRPZIGZAGBITMAP(pBitmap, uAmplitude, uAttenuation, uFrequency, nPhase, CenterPt, crFill, uFlags) \
      ((pL_ZigZagBitmap )?pL_ZigZagBitmap(pBitmap, uAmplitude, uAttenuation, uFrequency, nPhase, CenterPt, crFill, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPCOLOREDBALLSBITMAP(pBitmap, uNumBalls, uSize, uSizeVariation, nHighLightAng, crHighLight, crBkgColor, crShadingColor, pBallColors, uNumOfBallColors, uAvrBallClrOpacity, uBallClrOpacityVariation, uRipple, uFlags) \
      ((pL_ColoredBallsBitmap )?pL_ColoredBallsBitmap(pBitmap, uNumBalls, uSize, uSizeVariation, nHighLightAng, crHighLight, crBkgColor, crShadingColor, pBallColors, uNumOfBallColors, uAvrBallClrOpacity, uBallClrOpacityVariation, uRipple, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPERSPECTIVEBITMAP(pBitmap, pPoints, crBkgColor, uFlags) \
      ((pL_PerspectiveBitmap )?pL_PerspectiveBitmap(pBitmap, pPoints, crBkgColor, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPPOINTILLISTBITMAP(pBitmap, uSize, crColor, uFlag) \
      ((pL_PointillistBitmap )?pL_PointillistBitmap(pBitmap, uSize, crColor, uFlag):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPHALFTONEPATTERNBITMAP(pBitmap, uContrast, uRipple, uAngleContrast, uAngleRipple, nAngleOffset, crForGround, crBackGround, uFlag) \
      ((pL_HalfTonePatternBitmap )?pL_HalfTonePatternBitmap(pBitmap, uContrast, uRipple, uAngleContrast, uAngleRipple, nAngleOffset, crForGround, crBackGround, uFlag):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPSIZEBITMAPINTERPOLATE(pBitmap, nWidth, nHeight, uFlag) \
   ((pL_SizeBitmapInterpolate )?pL_SizeBitmapInterpolate(pBitmap, nWidth, nHeight, uFlag):WRPERR_LTIMGCOR_DLL_NOT_LOADED)

#define L_WRPCOLOREDPENCILBITMAPEXT(pBitmap, uSize, uStrength, uThreshold, uPencilRoughness, uStrokeLength, uPaperRoughness, nAngle, uFlags) \
   ((pL_ColoredPencilBitmapExt )?pL_ColoredPencilBitmapExt(pBitmap, uSize, uStrength, uThreshold, uPencilRoughness, uStrokeLength, uPaperRoughness, nAngle, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPINTELLIGENTUPSCALEBITMAP(pBitmap, pMaskBitmap, removeObjectColor, preserveObjectColor, newWidth, widthInsertionFactor, newHeight, heightInsertionFactor, InsertionOrder, uFlags) \
   ((pL_IntelligentUpScaleBitmap )?pL_IntelligentUpScaleBitmap(pBitmap, pMaskBitmap, removeObjectColor, preserveObjectColor, newWidth, widthInsertionFactor, newHeight, heightInsertionFactor, InsertionOrder, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

#define L_WRPINTELLIGENTDOWNSCALEBITMAP(pBitmap, pMaskBitmap, removeObjectColor, preserveObjectColor, newWidth, newHeight, carvingOrder, uFlags) \
   ((pL_IntelligentDownScaleBitmap )?pL_IntelligentDownScaleBitmap(pBitmap ,pMaskBitmap, removeObjectColor, preserveObjectColor, newWidth, newHeight, carvingOrder, uFlags):WRPERR_LTIMGSFX_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTDIS.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPAPPENDPLAYBACK(hPlayback,pBitmap) \
      ((pL_AppendPlayback )?pL_AppendPlayback(hPlayback,pBitmap):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCANCELPLAYBACKWAIT(hPlayback) \
      ((pL_CancelPlaybackWait )?pL_CancelPlaybackWait(hPlayback):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCHANGEFROMDDB(hDC,pBitmap,uStructSize,hBitmap,hPalette) \
      ((pL_ChangeFromDDB )?pL_ChangeFromDDB(hDC,pBitmap,uStructSize,hBitmap,hPalette):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCHANGETODDB(hDC,pBitmap) \
      ((pL_ChangeToDDB )?pL_ChangeToDDB(hDC,pBitmap):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HBITMAP)0))

#define L_WRPCLEARPLAYBACKUPDATERECT(hPlayback) \
      ((pL_ClearPlaybackUpdateRect )?pL_ClearPlaybackUpdateRect(hPlayback):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCLIPBOARDREADY() \
      ((pL_ClipboardReady )?pL_ClipboardReady():WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCONVERTCOLORSPACE(pBufferSrc,pBufferDst,nWidth,nFormatSrc,nFormatDst) \
      ((pL_ConvertColorSpace )?pL_ConvertColorSpace(pBufferSrc,pBufferDst,nWidth,nFormatSrc,nFormatDst):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCONVERTFROMDDB(hDC,pBitmap,uStructSize,hBitmap,hPalette) \
      ((pL_ConvertFromDDB )?pL_ConvertFromDDB(hDC,pBitmap,uStructSize,hBitmap,hPalette):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCONVERTTODDB(hDC,pBitmap) \
      ((pL_ConvertToDDB )?pL_ConvertToDDB(hDC,pBitmap):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HBITMAP)0))

#define L_WRPCOPYFROMCLIPBOARD(hWnd,pBitmap,uStructSize) \
      ((pL_CopyFromClipboard )?pL_CopyFromClipboard(hWnd,pBitmap,uStructSize):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCOPYTOCLIPBOARD(hWnd,pBitmap,uFlags) \
      ((pL_CopyToClipboard )?pL_CopyToClipboard(hWnd,pBitmap,uFlags):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCREATEPAINTPALETTE(hDC,pBitmap) \
   ((pL_CreatePaintPalette )?pL_CreatePaintPalette(hDC,pBitmap):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(HPALETTE)0))

#define L_WRPCREATEPLAYBACK(phPlayback,pBitmap,hList) \
      ((pL_CreatePlayback )?pL_CreatePlayback(phPlayback,pBitmap,hList):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPDESTROYPLAYBACK(hPlayback,phList) \
      ((pL_DestroyPlayback )?pL_DestroyPlayback(hPlayback,phList):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETDISPLAYMODE() \
      ((pL_GetDisplayMode )?pL_GetDisplayMode():WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETPAINTCONTRAST(pBitmap) \
      ((pL_GetPaintContrast )?pL_GetPaintContrast(pBitmap):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETPAINTGAMMA(pBitmap) \
      ((pL_GetPaintGamma )?pL_GetPaintGamma(pBitmap):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETPAINTINTENSITY(pBitmap) \
      ((pL_GetPaintIntensity )?pL_GetPaintIntensity(pBitmap):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETPLAYBACKDELAY(hPlayback,puDelay) \
      ((pL_GetPlaybackDelay )?pL_GetPlaybackDelay(hPlayback,puDelay):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETPLAYBACKINDEX(hPlayback,pnIndex) \
      ((pL_GetPlaybackIndex )?pL_GetPlaybackIndex(hPlayback,pnIndex):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETPLAYBACKSTATE(hPlayback,puState) \
      ((pL_GetPlaybackState )?pL_GetPlaybackState(hPlayback,puState):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETPLAYBACKUPDATERECT(hPlayback,prcUpdate,fClear) \
      ((pL_GetPlaybackUpdateRect )?pL_GetPlaybackUpdateRect(hPlayback,prcUpdate,fClear):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPPAINTDC(hDC,pBitmap,pSrc,pClipSrc,pDst,pClipDst,uROP3) \
      ((pL_PaintDC )?pL_PaintDC(hDC,pBitmap,pSrc,pClipSrc,pDst,pClipDst,uROP3):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPPAINTDCBUFFER(hDC,pBitmap,pSrc,pClipSrc,pDst,pClipDst,uROP3,pBuffer,nRow,nCount) \
      ((pL_PaintDCBuffer )?pL_PaintDCBuffer(hDC,pBitmap,pSrc,pClipSrc,pDst,pClipDst,uROP3,pBuffer,nRow,nCount):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPPROCESSPLAYBACK(hPlayback,puState) \
      ((pL_ProcessPlayback )?pL_ProcessPlayback(hPlayback,puState):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPRGBTOHSV(crColor) \
      ((pL_RGBtoHSV )?pL_RGBtoHSV(crColor):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPHSVTORGB(hsvColor) \
      ((pL_HSVtoRGB )?pL_HSVtoRGB(hsvColor):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETDISPLAYMODE(uFlagPos,uFlagSet) \
      ((pL_SetDisplayMode )?pL_SetDisplayMode(uFlagPos,uFlagSet):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETPAINTCONTRAST(pBitmap,nValue) \
      ((pL_SetPaintContrast )?pL_SetPaintContrast(pBitmap,nValue):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETPAINTGAMMA(pBitmap,uValue) \
      ((pL_SetPaintGamma )?pL_SetPaintGamma(pBitmap,uValue):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETPAINTINTENSITY(pBitmap,nValue) \
      ((pL_SetPaintIntensity )?pL_SetPaintIntensity(pBitmap,nValue):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETPLAYBACKINDEX(hPlayback,nIndex) \
      ((pL_SetPlaybackIndex )?pL_SetPlaybackIndex(hPlayback,nIndex):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPUNDERLAYBITMAP(pBitmapDst,pUnderlay,uFlags) \
      ((pL_UnderlayBitmap )?pL_UnderlayBitmap(pBitmapDst,pUnderlay,uFlags):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPVALIDATEPLAYBACKLINES(hPlayback,nRow,nLines) \
      ((pL_ValidatePlaybackLines )?pL_ValidatePlaybackLines(hPlayback,nRow,nLines):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPWINDOWLEVEL(pBitmap,nLowBit,nHighBit,pLUT,ulLUTLength,uFlags) \
      ((pL_WindowLevel )?pL_WindowLevel(pBitmap,nLowBit,nHighBit,pLUT,ulLUTLength,uFlags):WRPERR_LTDIS_DLL_NOT_LOADED)


#define L_WRPWINDOWLEVELFILLLUT(pLUT,ulLUTLen,crStart,crEnd,nLow,nHigh,uLowBit,uHighBit,nMinValue,nMaxValue,nFactor,uFlags) \
      ((pL_WindowLevelFillLUT )?pL_WindowLevelFillLUT(pLUT,ulLUTLen,crStart,crEnd,nLow,nHigh,uLowBit,uHighBit,nMinValue,nMaxValue,nFactor,uFlags):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPWINDOWLEVELFILLLUT2(pLUT,ulLUTLen,crStart,crEnd,nLow,nHigh,uLowBit,uHighBit,nMinValue,nMaxValue,nFactor,uFlags) \
      ((pL_WindowLevelFillLUT2 )?pL_WindowLevelFillLUT2(pLUT,ulLUTLen,crStart,crEnd,nLow,nHigh,uLowBit,uHighBit,nMinValue,nMaxValue,nFactor,uFlags):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETBITMAPCLIPSEGMENTS(pBitmap,nRow,pSegmentBuffer,puSegmentCount) \
      ((pL_GetBitmapClipSegments )?pL_GetBitmapClipSegments(pBitmap,nRow,pSegmentBuffer,puSegmentCount):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETBITMAPCLIPSEGMENTSMAX(pBitmap,puMaxSegments) \
      ((pL_GetBitmapClipSegmentsMax )?pL_GetBitmapClipSegmentsMax(pBitmap,puMaxSegments):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSTARTMAGGLASS(hWnd,pBitmap,prcDst,pMagGlassOptions,pfnCallback,pUserData) \
      ((pL_StartMagGlass )?pL_StartMagGlass(hWnd,pBitmap,prcDst,pMagGlassOptions,pfnCallback,pUserData):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSTOPMAGGLASS(hWnd) \
      ((pL_StopMagGlass )?pL_StopMagGlass(hWnd):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPUPDATEMAGGLASSRECT(hWnd,prcDst) \
      ((pL_UpdateMagGlassRect )?pL_UpdateMagGlassRect(hWnd,prcDst):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPUPDATEMAGGLASSPAINTFLAGS(hWnd,uPaintFlags) \
      ((pL_UpdateMagGlassPaintFlags )?pL_UpdateMagGlassPaintFlags(hWnd,uPaintFlags):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPUPDATEMAGGLASS(hWnd,pColor,pMaskPlane,nMaskPlaneStart,nMaskPlaneEnd,bUpdateBitmap) \
      ((pL_UpdateMagGlass )?pL_UpdateMagGlass(hWnd,pColor,pMaskPlane,nMaskPlaneStart,nMaskPlaneEnd,bUpdateBitmap):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPUPDATEMAGGLASSBITMAP(hWnd,pBitmap,bUpdateBitmap) \
      ((pL_UpdateMagGlassBitmap )?pL_UpdateMagGlassBitmap(hWnd,pBitmap,bUpdateBitmap):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPWINDOWHASMAGGLASS(hWnd) \
      ((pL_WindowHasMagGlass )?pL_WindowHasMagGlass(hWnd):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETMAGGLASSOWNERDRAWCALLBACK(hWnd,pfnCallback,pUserData) \
      ((pL_SetMagGlassOwnerDrawCallback )?pL_SetMagGlassOwnerDrawCallback(hWnd,pfnCallback,pUserData):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETMAGGLASSPAINTOPTIONS(hWnd,pMagGlassPaintOptions) \
      ((pL_SetMagGlassPaintOptions )?pL_SetMagGlassPaintOptions(hWnd,pMagGlassPaintOptions):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSHOWMAGGLASS(hWnd,bShowMagGlass) \
      ((pL_ShowMagGlass )?pL_ShowMagGlass(hWnd,bShowMagGlass):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETMAGGLASSPOS(hWnd,xPos,yPos) \
      ((pL_SetMagGlassPos )?pL_SetMagGlassPos(hWnd,xPos,yPos):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPUPDATEMAGGLASSSHAPE(hWnd,uMagGlassShape,hMagGlassRgn) \
      ((pL_UpdateMagGlassShape )?pL_UpdateMagGlassShape(hWnd,uMagGlassShape,hMagGlassRgn):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPPRINTBITMAP(hDC,pBitmap,nX,nY,nWidth,nHeight,fEndDoc) \
      ((pL_PrintBitmap )?pL_PrintBitmap(hDC,pBitmap,nX,nY,nWidth,nHeight,fEndDoc): (LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HDC)0))

#define L_WRPPRINTBITMAPFAST(hDC,pBitmap,nX,nY,nWidth,nHeight,fEndDoc) \
      ((pL_PrintBitmapFast )?pL_PrintBitmapFast(hDC,pBitmap,nX,nY,nWidth,nHeight,fEndDoc):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HDC)0))

#define L_WRPSCREENCAPTUREBITMAP(hDC,pBitmap,uStructSize,pRect) \
      ((pL_ScreenCaptureBitmap )?pL_ScreenCaptureBitmap(hDC,pBitmap,uStructSize,pRect):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPBITMAPHASRGN(pBitmap) \
      ((pL_BitmapHasRgn )?pL_BitmapHasRgn(pBitmap):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCREATEMASKFROMBITMAPRGN(pBitmap,pMask,uStructSize) \
      ((pL_CreateMaskFromBitmapRgn )?pL_CreateMaskFromBitmapRgn(pBitmap,pMask,uStructSize):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCURVETOBEZIER(pCurve,pOutPointCount,OutPoint) \
      ((pL_CurveToBezier )?pL_CurveToBezier(pCurve,pOutPointCount,OutPoint):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPFRAMEBITMAPRGN(hDC,pBitmap,pXForm,uType) \
      ((pL_FrameBitmapRgn )?pL_FrameBitmapRgn(hDC,pBitmap,pXForm,uType):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCOLORBITMAPRGN(hDC,pBitmap,pXForm,crRgnColor) \
      ((pL_ColorBitmapRgn )?pL_ColorBitmapRgn(hDC,pBitmap,pXForm,crRgnColor):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPFREEBITMAPRGN(pBitmap) \
      ((pL_FreeBitmapRgn )?pL_FreeBitmapRgn(pBitmap):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETBITMAPRGNAREA(pBitmap,puArea) \
      ((pL_GetBitmapRgnArea )?pL_GetBitmapRgnArea(pBitmap,puArea):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETBITMAPRGNBOUNDS(pBitmap,pXForm,pRect) \
      ((pL_GetBitmapRgnBounds )?pL_GetBitmapRgnBounds(pBitmap,pXForm,pRect):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETBITMAPRGNHANDLE(pBitmap,pXForm,phRgn) \
      ((pL_GetBitmapRgnHandle )?pL_GetBitmapRgnHandle(pBitmap,pXForm,phRgn):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPISPTINBITMAPRGN(pBitmap,nRow,nCol) \
      ((pL_IsPtInBitmapRgn )?pL_IsPtInBitmapRgn(pBitmap,nRow,nCol):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPOFFSETBITMAPRGN(pBitmap,nRowOffset,nColOffset) \
      ((pL_OffsetBitmapRgn )?pL_OffsetBitmapRgn(pBitmap,nRowOffset,nColOffset):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPPAINTRGNDC(hDC,pBitmap,pSrc,pClipSrc,pDst,pClipDst,uROP3) \
      ((pL_PaintRgnDC )?pL_PaintRgnDC(hDC,pBitmap,pSrc,pClipSrc,pDst,pClipDst,uROP3):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPPAINTRGNDCBUFFER(hDC,pBitmap,pSrc,pClipSrc,pDst,pClipDst,uROP3,pBuffer,nRow,nCount) \
      ((pL_PaintRgnDCBuffer )?pL_PaintRgnDCBuffer(hDC,pBitmap,pSrc,pClipSrc,pDst,pClipDst,uROP3,pBuffer,nRow,nCount):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNCOLOR(pBitmap,crColor,uCombineMode) \
      ((pL_SetBitmapRgnColor )?pL_SetBitmapRgnColor(pBitmap,crColor,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNCOLORHSVRANGE(pBitmap,hsvLower,hsvUpper,uCombineMode) \
      ((pL_SetBitmapRgnColorHSVRange )?pL_SetBitmapRgnColorHSVRange(pBitmap,hsvLower,hsvUpper,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNCOLORRGBRANGE(pBitmap,crLower,crUpper,uCombineMode) \
      ((pL_SetBitmapRgnColorRGBRange )?pL_SetBitmapRgnColorRGBRange(pBitmap,crLower,crUpper,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNMAGICWAND(pBitmap,x,y,crLowerTolerance,crUpperTolerance,uCombineMode) \
      ((pL_SetBitmapRgnMagicWand )?pL_SetBitmapRgnMagicWand(pBitmap,x,y,crLowerTolerance,crUpperTolerance,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNELLIPSE(pBitmap,pXForm,pRect,uCombineMode) \
      ((pL_SetBitmapRgnEllipse )?pL_SetBitmapRgnEllipse(pBitmap,pXForm,pRect,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNFROMMASK(pBitmap,pXForm,pMask,uCombineMode) \
      ((pL_SetBitmapRgnFromMask )?pL_SetBitmapRgnFromMask(pBitmap,pXForm,pMask,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNHANDLE(pBitmap,pXForm,hRgn,uCombineMode) \
      ((pL_SetBitmapRgnHandle )?pL_SetBitmapRgnHandle(pBitmap,pXForm,hRgn,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNPOLYGON(pBitmap,pXForm,pPoints,uPoints,uFillMode,uCombineMode) \
      ((pL_SetBitmapRgnPolygon )?pL_SetBitmapRgnPolygon(pBitmap,pXForm,pPoints,uPoints,uFillMode,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNRECT(pBitmap,pXForm,pRect,uCombineMode) \
      ((pL_SetBitmapRgnRect )?pL_SetBitmapRgnRect(pBitmap,pXForm,pRect,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNROUNDRECT(pBitmap,pXForm,pRect,nWidthEllipse,nHeightEllipse,uCombineMode) \
      ((pL_SetBitmapRgnRoundRect )?pL_SetBitmapRgnRoundRect(pBitmap,pXForm,pRect,nWidthEllipse,nHeightEllipse,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNCURVE(pBitmap,pXForm,pCurve,uCombineMode) \
      ((pL_SetBitmapRgnCurve )?pL_SetBitmapRgnCurve(pBitmap,pXForm,pCurve,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCREATEPANWINDOW(hWndParent,pBitmap,ulDisplayFlags,nLeft,nTop,nWidth,nHeight,pszClassName,hIcon,hCursor,bSysMenu,pfnPanCallback,pUserData) \
      ((pL_CreatePanWindow )?pL_CreatePanWindow(hWndParent,pBitmap,ulDisplayFlags,nLeft,nTop,nWidth,nHeight,pszClassName,hIcon,hCursor,bSysMenu,pfnPanCallback,pUserData):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPUPDATEPANWINDOW(hPanWindow,pBitmap,ulDisplayFlags,crPen,pszTitle,prcDst) \
      ((pL_UpdatePanWindow )?pL_UpdatePanWindow(hPanWindow,pBitmap,ulDisplayFlags,crPen,pszTitle,prcDst):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPDESTROYPANWINDOW(hPanWindow) \
      ((pL_DestroyPanWindow )?pL_DestroyPanWindow(hPanWindow):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETBITMAPRGNDATA(pBitmap,pXForm,nDataSize,pData) \
      ((pL_GetBitmapRgnData )?pL_GetBitmapRgnData(pBitmap,pXForm,nDataSize,pData):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNDATA(pBitmap,pXForm,nDataSize,pData,uCombineMode) \
      ((pL_SetBitmapRgnData )?pL_SetBitmapRgnData(pBitmap,pXForm,nDataSize,pData,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNCLIP(pBitmap,bClip) \
      ((pL_SetBitmapRgnClip )?pL_SetBitmapRgnClip(pBitmap,bClip):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETBITMAPRGNCLIP(pBitmap,pbClip) \
      ((pL_GetBitmapRgnClip )?pL_GetBitmapRgnClip(pBitmap,pbClip):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPGETBITMAPRGNBOUNDSCLIP(pBitmap,pXForm,pRect) \
      ((pL_GetBitmapRgnBoundsClip )?pL_GetBitmapRgnBoundsClip(pBitmap,pXForm,pRect):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCONVERTFROMWMF(pBitmap,uStructSize,hWmf,uWidth,uHeight) \
      ((pL_ConvertFromWMF )?pL_ConvertFromWMF(pBitmap,uStructSize,hWmf,uWidth,uHeight):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCHANGEFROMWMF(pBitmap,uStructSize,hWmf,uWidth,uHeight) \
      ((pL_ChangeFromWMF )?pL_ChangeFromWMF(pBitmap,uStructSize,hWmf,uWidth,uHeight):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCONVERTTOWMF(pBitmap) \
      ((pL_ConvertToWMF )?pL_ConvertToWMF(pBitmap):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HMETAFILE)0))

#define L_WRPCHANGETOWMF(pBitmap) \
      ((pL_ChangeToWMF )?pL_ChangeToWMF(pBitmap):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HMETAFILE)0))

#define L_WRPCONVERTFROMEMF(pBitmap,uStructSize,hWmf,uWidth,uHeight) \
      ((pL_ConvertFromEMF )?pL_ConvertFromEMF(pBitmap,uStructSize,hWmf,uWidth,uHeight):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCHANGEFROMEMF(pBitmap,uStructSize,hWmf,uWidth,uHeight) \
      ((pL_ChangeFromEMF )?pL_ChangeFromEMF(pBitmap,uStructSize,hWmf,uWidth,uHeight):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPCONVERTTOEMF(pBitmap) \
      ((pL_ConvertToEMF )?pL_ConvertToEMF(pBitmap):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HENHMETAFILE)0))

#define L_WRPCHANGETOEMF(pBitmap) \
      ((pL_ChangeToEMF )?pL_ChangeToEMF(pBitmap):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HENHMETAFILE)0))

#define L_WRPPAINTDCOVERLAY(hDC,pBitmap,nIndex,pSrc,pClipSrc,pDst,pClipDst,uROP3) \
      ((pL_PaintDCOverlay )?pL_PaintDCOverlay(hDC,pBitmap,nIndex,pSrc,pClipSrc,pDst,pClipDst,uROP3):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPDOUBLEBUFFERENABLE(hDoubleBufferHandle,bEnable) \
      ((pL_DoubleBufferEnable )?pL_DoubleBufferEnable(hDoubleBufferHandle,bEnable):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPDOUBLEBUFFERCREATEHANDLE(phDoubleBufferHandle) \
      ((pL_DoubleBufferCreateHandle )?pL_DoubleBufferCreateHandle(phDoubleBufferHandle):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPDOUBLEBUFFERDESTROYHANDLE(hDoubleBufferHandle) \
      ((pL_DoubleBufferDestroyHandle )?pL_DoubleBufferDestroyHandle(hDoubleBufferHandle):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPDOUBLEBUFFERBEGIN(hDoubleBufferHandle,hDC,cx,cy) \
      ((pL_DoubleBufferBegin )?pL_DoubleBufferBegin(hDoubleBufferHandle,hDC,cx,cy):(LBase::RecordError(WRPERR_LTDIS_DLL_NOT_LOADED),(L_HDC)0))

#define L_WRPDOUBLEBUFFEREND(hDoubleBufferHandle,hDC) \
      ((pL_DoubleBufferEnd )?pL_DoubleBufferEnd(hDoubleBufferHandle,hDC):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPSETBITMAPRGNBORDER(pBitmap,x,y,crBorderColor,crLowerTolerance,crUpperTolerance,uCombineMode) \
      ((pL_SetBitmapRgnBorder )?pL_SetBitmapRgnBorder(pBitmap,x,y,crBorderColor,crLowerTolerance,crUpperTolerance,uCombineMode):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPPAINTDCCMYKARRAY(hDC,ppBitmapArray,uBitmapArrayCount,pSrc,pClipSrc,pDst,pClipDst,uROP3,hClrHandle) \
      ((pL_PaintDCCMYKArray )?pL_PaintDCCMYKArray(hDC,ppBitmapArray,uBitmapArrayCount,pSrc,pClipSrc,pDst,pClipDst,uROP3,hClrHandle):WRPERR_LTDIS_DLL_NOT_LOADED)

#define L_WRPPRINTBITMAPGDIPLUS(hDC, pBitmap, nX, nY, nWidth, nHeight, uFlags) \
   ((pL_PrintBitmapGDIPlus )?pL_PrintBitmapGDIPlus(hDC, pBitmap, nX, nY, nWidth, nHeight, uFlags):WRPERR_LTDIS_DLL_NOT_LOADED)
//-----------------------------------------------------------------------------
//--LTFIL.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPCOMPRESSBUFFER(pBuffer) \
      ((pL_CompressBuffer )?pL_CompressBuffer(pBuffer):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPDELETEPAGE(pszFile, nPage, uFlags, pSaveOptions) \
      ((pL_DeletePage )?pL_DeletePage(pszFile, nPage, uFlags, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPENDCOMPRESSBUFFER() \
      ((pL_EndCompressBuffer )?pL_EndCompressBuffer():WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADLOADRESOLUTIONS(pszFile, pDimensions, pDimensionCount, pLoadOptions) \
      ((pL_ReadLoadResolutions )?pL_ReadLoadResolutions(pszFile, pDimensions, pDimensionCount, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPFEEDLOAD(hLoad, pBuffer, dwBufferSize) \
      ((pL_FeedLoad )?pL_FeedLoad(hLoad, pBuffer, dwBufferSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPFILECONVERT(pszFileSrc, pszFileDst, nType, nWidth, nHeight, nBitsPerPixel, nQFactor, pLoadOptions, pSaveOptions, pFileInfo) \
      ((pL_FileConvert )?pL_FileConvert(pszFileSrc, pszFileDst, nType, nWidth, nHeight, nBitsPerPixel, nQFactor, pLoadOptions, pSaveOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPFILEINFO(pszFile, pFileInfo, uStructSize, uFlags, pLoadOptions) \
      ((pL_FileInfo )?pL_FileInfo(pszFile, pFileInfo, uStructSize, uFlags, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPFILEINFOMEMORY(pBuffer, pFileInfo, uStructSize, nBufferSize, uFlags, pLoadOptions) \
      ((pL_FileInfoMemory )?pL_FileInfoMemory(pBuffer, pFileInfo, uStructSize, nBufferSize, uFlags, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETCOMMENT(uType, pComment, uLength) \
      ((pL_GetComment )?pL_GetComment(uType, pComment, uLength):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETLOADRESOLUTION(nFormat, pWidth, pHeight, pLoadOptions) \
      ((pL_GetLoadResolution )?pL_GetLoadResolution(nFormat, pWidth, pHeight, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETFILECOMMENTSIZE(pszFile, uType, uLength, pLoadOptions) \
      ((pL_GetFileCommentSize )?pL_GetFileCommentSize(pszFile, uType, uLength, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETTAG(uTag, pType, pCount, pData) \
      ((pL_GetTag )?pL_GetTag(uTag, pType, pCount, pData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADBITMAP(pszFile, pBitmap, uStructSize, nBitsPerPixel, nOrder, pLoadOptions, pFileInfo) \
      ((pL_LoadBitmap )?pL_LoadBitmap(pszFile, pBitmap, uStructSize, nBitsPerPixel, nOrder, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADBITMAPLIST(lpszFile, phList, nBitsTo, nColorOrder, pLoadOptions, pFileInfo) \
      ((pL_LoadBitmapList )?pL_LoadBitmapList(lpszFile, phList, nBitsTo, nColorOrder, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADBITMAPMEMORY(pBuffer, pBitmap, uStructSize, nBitsPerPixel, nOrder, nBufferSize, pLoadOptions, pFileInfo) \
      ((pL_LoadBitmapMemory )?pL_LoadBitmapMemory(pBuffer, pBitmap, uStructSize, nBitsPerPixel, nOrder, nBufferSize, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADFILE(pszFile, pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, pLoadOptions, pFileInfo) \
      ((pL_LoadFile )?pL_LoadFile(pszFile, pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADFILETILE(pszFile, pBitmap, uStructSize, nCol, nRow, uWidth, uHeight, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, pLoadOptions, pFileInfo) \
      ((pL_LoadFileTile )?pL_LoadFileTile(pszFile, pBitmap, uStructSize, nCol, nRow, uWidth, uHeight, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADMEMORYTILE(pBuffer, pBitmap, uStructSize, nCol, nRow, uWidth, uHeight, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, nBufferSize, pLoadOptions, pFileInfo) \
      ((pL_LoadMemoryTile )?pL_LoadMemoryTile(pBuffer, pBitmap, uStructSize, nCol, nRow, uWidth, uHeight, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, nBufferSize, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADFILEOFFSET(fd, nOffsetBegin, nBytesToLoad, pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, pLoadOptions, pFileInfo) \
      ((pL_LoadFileOffset )?pL_LoadFileOffset(fd, nOffsetBegin, nBytesToLoad, pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADMEMORY(pBuffer, pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, nBufferSize, pLoadOptions, pFileInfo) \
      ((pL_LoadMemory )?pL_LoadMemory(pBuffer, pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, nBufferSize, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILECOMMENT(pszFile, uType, pComment, uLength, pLoadOptions) \
      ((pL_ReadFileComment )?pL_ReadFileComment(pszFile, uType, pComment, uLength, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILECOMMENTEXT(pszFile, uType, pComments, pBuffer, uLength, pLoadOptions) \
      ((pL_ReadFileCommentExt )?pL_ReadFileCommentExt(pszFile, uType, pComments, pBuffer, uLength, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILECOMMENTMEMORY(pBuffer, uType, pComment, uLength, nBufferSize, pLoadOptions) \
      ((pL_ReadFileCommentMemory )?pL_ReadFileCommentMemory(pBuffer, uType, pComment, uLength, nBufferSize, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILETAG(pFile, uTag, pType, pCount, pData, pLoadOptions) \
      ((pL_ReadFileTag )?pL_ReadFileTag(pFile, uTag, pType, pCount, pData, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILETAGMEMORY(pBuffer, uTag, pType, pCount, pData, nBufferSize, pLoadOptions) \
      ((pL_ReadFileTagMemory )?pL_ReadFileTagMemory(pBuffer, uTag, pType, pCount, pData, nBufferSize, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILESTAMP(pszFile, pBitmap, uStructSize, pLoadOptions) \
      ((pL_ReadFileStamp )?pL_ReadFileStamp(pszFile, pBitmap, uStructSize, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEBITMAP(pszFile, pBitmap, nFormat, nBitsPerPixel, nQFactor, pSaveOptions) \
      ((pL_SaveBitmap )?pL_SaveBitmap(pszFile, pBitmap, nFormat, nBitsPerPixel, nQFactor, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEBITMAPBUFFER(pBuffer, uInitialBufferSize, puFinalFileSize, pBitmap, nFormat, nBitsPerPixel, nQFactor, pfnSaveBufferCB, pUserData, pSaveOptions) \
      ((pL_SaveBitmapBuffer )?pL_SaveBitmapBuffer(pBuffer, uInitialBufferSize, puFinalFileSize, pBitmap, nFormat, nBitsPerPixel, nQFactor, pfnSaveBufferCB, pUserData, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEBITMAPLIST(lpszFile, hList, nFormat, nBits, nQFactor, pSaveOptions) \
      ((pL_SaveBitmapList )?pL_SaveBitmapList(lpszFile, hList, nFormat, nBits, nQFactor, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEBITMAPMEMORY(phHandle, pBitmap, nFormat, nBitsPerPixel, nQFactor, puSize, pSaveOptions) \
      ((pL_SaveBitmapMemory )?pL_SaveBitmapMemory(phHandle, pBitmap, nFormat, nBitsPerPixel, nQFactor, puSize, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEFILE(pszFile, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pfnCallback, pUserData, pSaveOptions) \
      ((pL_SaveFile )?pL_SaveFile(pszFile, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pfnCallback, pUserData, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEFILEBUFFER(pBuffer, uInitialBufferSize, puFinalFileSize, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pfnFileSaveCB, pfnSaveBufferCB, pUserData, pSaveOptions) \
      ((pL_SaveFileBuffer )?pL_SaveFileBuffer(pBuffer, uInitialBufferSize, puFinalFileSize, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pfnFileSaveCB, pfnSaveBufferCB, pUserData, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEFILEMEMORY(hHandle, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pFunction, lpUserData, uSize, pSaveOptions) \
      ((pL_SaveFileMemory )?pL_SaveFileMemory(hHandle, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pFunction, lpUserData, uSize, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEFILETILE(pszFile, pBitmap, nCol, nRow, pfnCallback, pUserData, pSaveOptions) \
      ((pL_SaveFileTile )?pL_SaveFileTile(pszFile, pBitmap, nCol, nRow, pfnCallback, pUserData, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEFILEOFFSET(fd, nOffsetBegin, nSizeWritten, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pfnCallback, pUserData, pSaveOptions) \
      ((pL_SaveFileOffset )?pL_SaveFileOffset(fd, nOffsetBegin, nSizeWritten, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pfnCallback, pUserData, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETCOMMENT(uType, pComment, uLength) \
      ((pL_SetComment )?pL_SetComment(uType, pComment, uLength):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETLOADINFOCALLBACK(pfnCallback, pUserData) \
      ((pL_SetLoadInfoCallback )?pL_SetLoadInfoCallback(pfnCallback, pUserData):(LBase::RecordError(WRPERR_LTFIL_DLL_NOT_LOADED),(LOADINFOCALLBACK)0))

#define L_WRPGETLOADINFOCALLBACKDATA() \
      ((pL_GetLoadInfoCallbackData )?pL_GetLoadInfoCallbackData():(LBase::RecordError(WRPERR_LTFIL_DLL_NOT_LOADED),(LOADINFOCALLBACK)0))

#define L_WRPSETLOADRESOLUTION(nFormat, nWidth, nHeight) \
      ((pL_SetLoadResolution )?pL_SetLoadResolution(nFormat, nWidth, nHeight):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETTAG(uTag, uType, uCount, pData) \
      ((pL_SetTag )?pL_SetTag(uTag, uType, uCount, pData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSTARTCOMPRESSBUFFER(pBitmap, pfnCallback, uInputBytes, uOutputBytes, pOutputBuffer, nOutputType, nQFactor, pUserData, pSaveOptions) \
      ((pL_StartCompressBuffer )?pL_StartCompressBuffer(pBitmap, pfnCallback, uInputBytes, uOutputBytes, pOutputBuffer, nOutputType, nQFactor, pUserData, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSTARTFEEDLOAD(pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, phLoad, pLoadOptions, pFileInfo) \
      ((pL_StartFeedLoad )?pL_StartFeedLoad(pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnCallback, pUserData, phLoad, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSTOPFEEDLOAD(hLoad) \
      ((pL_StopFeedLoad )?pL_StopFeedLoad(hLoad):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPWRITEFILECOMMENTEXT(pszFile, uType, pComments, pSaveOptions) \
      ((pL_WriteFileCommentExt )?pL_WriteFileCommentExt(pszFile, uType, pComments, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPWRITEFILESTAMP(pszFile, pBitmap, pSaveOptions) \
      ((pL_WriteFileStamp )?pL_WriteFileStamp(pszFile, pBitmap, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETSAVERESOLUTION(uCount, pResolutions) \
      ((pL_SetSaveResolution )?pL_SetSaveResolution(uCount, pResolutions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETSAVERESOLUTION(puCount, pResolutions) \
      ((pL_GetSaveResolution )?pL_GetSaveResolution(puCount, pResolutions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETDEFAULTLOADFILEOPTION(pLoadOptions, uStructSize) \
      ((pL_GetDefaultLoadFileOption )?pL_GetDefaultLoadFileOption(pLoadOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETDEFAULTSAVEFILEOPTION(pSaveOptions, uStructSize) \
      ((pL_GetDefaultSaveFileOption )?pL_GetDefaultSaveFileOption(pSaveOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPWRITEFILETAG(pszFile, pSaveOptions) \
      ((pL_WriteFileTag )?pL_WriteFileTag(pszFile, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPWRITEFILECOMMENT(pszFile, pSaveOptions) \
      ((pL_WriteFileComment )?pL_WriteFileComment(pszFile, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPCREATETHUMBNAILFROMFILE(pszFile, pBitmap, uStructSize, pThumbOptions, pfnCallback, pUserData, pLoadOptions, pFileInfo) \
      ((pL_CreateThumbnailFromFile )?pL_CreateThumbnailFromFile(pszFile, pBitmap, uStructSize, pThumbOptions, pfnCallback, pUserData, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETJ2KOPTIONS(pOptions, uStructSize) \
      ((pL_GetJ2KOptions )?pL_GetJ2KOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETDEFAULTJ2KOPTIONS(pOptions, uStructSize) \
      ((pL_GetDefaultJ2KOptions )?pL_GetDefaultJ2KOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETJ2KOPTIONS(pOptions) \
      ((pL_SetJ2KOptions )?pL_SetJ2KOptions(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPMARKERCALLBACKPROXY(pfnCallback, uMarker, uMarkerSize, pMarkerData, pLEADUserData) \
      ((pL_MarkerCallbackProxy )?pL_MarkerCallbackProxy(pfnCallback, uMarker, uMarkerSize, pMarkerData, pLEADUserData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPTRANSFORMFILE(pszFileSrc, pszFileDst, uTransform, pfnCallback, pUserData, pLoadOptions) \
      ((pL_TransformFile )?pL_TransformFile(pszFileSrc, pszFileDst, uTransform, pfnCallback, pUserData, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILEEXTENSIONS(pszFile, ppExtensionList, pLoadOptions) \
      ((pL_ReadFileExtensions )?pL_ReadFileExtensions(pszFile, ppExtensionList, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPFREEEXTENSIONS(pExtensionList) \
      ((pL_FreeExtensions )?pL_FreeExtensions(pExtensionList):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADEXTENSIONSTAMP(pExtensionList, pBitmap, uStructSize) \
      ((pL_LoadExtensionStamp )?pL_LoadExtensionStamp(pExtensionList, pBitmap, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETEXTENSIONAUDIO(pExtensionList, nStream, ppBuffer, puSize) \
      ((pL_GetExtensionAudio )?pL_GetExtensionAudio(pExtensionList, nStream, ppBuffer, puSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSTARTDECOMPRESSBUFFER(phDecompress, pStartDecompressData) \
      ((pL_StartDecompressBuffer )?pL_StartDecompressBuffer(phDecompress, pStartDecompressData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSTOPDECOMPRESSBUFFER(hDecompress) \
      ((pL_StopDecompressBuffer )?pL_StopDecompressBuffer(hDecompress):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPDECOMPRESSBUFFER(hDecompress, pDecompressData) \
      ((pL_DecompressBuffer )?pL_DecompressBuffer(hDecompress, pDecompressData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPIGNOREFILTERS(pszFilters) \
      ((pL_IgnoreFilters )?pL_IgnoreFilters(pszFilters):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPPRELOADFILTERS(nFixedFilters, nCachedFilters, pszFilters) \
      ((pL_PreLoadFilters )?pL_PreLoadFilters(nFixedFilters, nCachedFilters, pszFilters):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETIGNOREFILTERS(pszFilters, uSize) \
      ((pL_GetIgnoreFilters )?pL_GetIgnoreFilters(pszFilters, uSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETPRELOADFILTERS(pszFilters, uSize, pnFixedFilters, pnCachedFilters) \
      ((pL_GetPreLoadFilters )?pL_GetPreLoadFilters(pszFilters, uSize, pnFixedFilters, pnCachedFilters):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETJBIG2OPTIONS(pOptions, uStructSize) \
      ((pL_GetJBIG2Options )?pL_GetJBIG2Options(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETJBIG2OPTIONS(pOptions) \
      ((pL_SetJBIG2Options )?pL_SetJBIG2Options(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPCREATEMARKERS(phMarkers) \
      ((pL_CreateMarkers )?pL_CreateMarkers(phMarkers):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADMARKERS(pszFilename, phMarkers, uFlags) \
      ((pL_LoadMarkers )?pL_LoadMarkers(pszFilename, phMarkers, uFlags):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPFREEMARKERS(hMarkers) \
      ((pL_FreeMarkers )?pL_FreeMarkers(hMarkers):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETMARKERS(hMarkers, uFlags) \
      ((pL_SetMarkers )?pL_SetMarkers(hMarkers, uFlags):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETMARKERS(phMarkers, uFlags) \
      ((pL_GetMarkers )?pL_GetMarkers(phMarkers, uFlags):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPENUMMARKERS(hMarkers, uFlags, pfnCallback, pUserData) \
      ((pL_EnumMarkers )?pL_EnumMarkers(hMarkers, uFlags, pfnCallback, pUserData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPDELETEMARKER(hMarkers, uMarker, nCount) \
      ((pL_DeleteMarker )?pL_DeleteMarker(hMarkers, uMarker, nCount):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPINSERTMARKER(hMarkers, uIndex, uMarker, uMarkerSize, pMarkerData) \
      ((pL_InsertMarker )?pL_InsertMarker(hMarkers, uIndex, uMarker, uMarkerSize, pMarkerData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPCOPYMARKERS(phMarkersDst, hMarkersSrc) \
      ((pL_CopyMarkers )?pL_CopyMarkers(phMarkersDst, hMarkersSrc):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETMARKERCOUNT(hMarkers, puCount) \
      ((pL_GetMarkerCount )?pL_GetMarkerCount(hMarkers, puCount):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETMARKER(hMarkers, uIndex, puMarker, puMarkerSize, pMarkerData) \
      ((pL_GetMarker )?pL_GetMarker(hMarkers, uIndex, puMarker, puMarkerSize, pMarkerData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPDELETEMARKERINDEX(hMarkers, uIndex) \
      ((pL_DeleteMarkerIndex )?pL_DeleteMarkerIndex(hMarkers, uIndex):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPWRITEFILEMETADATA(pFile, uFlags, pSaveFileOption) \
      ((pL_WriteFileMetaData )?pL_WriteFileMetaData(pFile, uFlags, pSaveFileOption):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSTARTSAVEDATA(ppStruct, pszFileName, nCompression, nPlanarConfig, nOrder, uWidth, uHeight, nBitsPerPixel, pPalette, uPaletteEntries, XResolution, YResolution, bSaveMulti, pSaveOptions) \
      ((pL_StartSaveData )?pL_StartSaveData(ppStruct, pszFileName, nCompression, nPlanarConfig, nOrder, uWidth, uHeight, nBitsPerPixel, pPalette, uPaletteEntries, XResolution, YResolution, bSaveMulti, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEDATA(pStruct, pDataBuffer, ulBytes) \
      ((pL_SaveData )?pL_SaveData(pStruct, pDataBuffer, ulBytes):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSTOPSAVEDATA(pStruct) \
      ((pL_StopSaveData )?pL_StopSaveData(pStruct):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETOVERLAYCALLBACK(pfnCallback, pUserData, uFlag) \
      ((pL_SetOverlayCallback )?pL_SetOverlayCallback(pfnCallback, pUserData, uFlag):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETOVERLAYCALLBACK(ppfnCallback, ppUserData, puFlag) \
      ((pL_GetOverlayCallback )?pL_GetOverlayCallback(ppfnCallback, ppUserData, puFlag):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETFILTERLISTINFO(ppFilterList, pFilterCount) \
      ((pL_GetFilterListInfo )?pL_GetFilterListInfo(ppFilterList, pFilterCount):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETFILTERINFO(pFilterName, pFilterInfo, uStructSize) \
      ((pL_GetFilterInfo )?pL_GetFilterInfo(pFilterName, pFilterInfo, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPFREEFILTERINFO(pFilterInfo, uFilterCount, uFlags) \
      ((pL_FreeFilterInfo )?pL_FreeFilterInfo(pFilterInfo, uFilterCount, uFlags):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETFILTERINFO(pFilterInfo, uFilterCount, uFlags) \
      ((pL_SetFilterInfo )?pL_SetFilterInfo(pFilterInfo, uFilterCount, uFlags):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETTXTOPTIONS(pTxtOptions, uStructSize) \
      ((pL_GetTXTOptions )?pL_GetTXTOptions(pTxtOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETTXTOPTIONS(pTxtOptions) \
      ((pL_SetTXTOptions )?pL_SetTXTOptions(pTxtOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPCOMPACTFILE(pszSrcFile, pszDstFile, uPages, pLoadFileOption, pSaveFileOption) \
      ((pL_CompactFile )?pL_CompactFile(pszSrcFile, pszDstFile, uPages, pLoadFileOption, pSaveFileOption):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADFILECMYKARRAY(pszFile, ppBitmapArray, uArrayCount, uStructSize, nBitsPerPixel, uFlags, pfnCallback, pUserData, pLoadFileOption, pFileInfo) \
      ((pL_LoadFileCMYKArray )?pL_LoadFileCMYKArray(pszFile, ppBitmapArray, uArrayCount, uStructSize, nBitsPerPixel, uFlags, pfnCallback, pUserData, pLoadFileOption, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEFILECMYKARRAY(pszFile, pBITMAPHANDLEppBitmapArray, uBitmapArrayCount, nFormat, nBitsPerPixel, nQFactor, uFlags, pfnCallback, pUserData, pSaveOptions) \
      ((pL_SaveFileCMYKArray )?pL_SaveFileCMYKArray(pszFile, pBITMAPHANDLEppBitmapArray, uBitmapArrayCount, nFormat, nBitsPerPixel, nQFactor, uFlags, pfnCallback, pUserData, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPENUMFILETAGS(pszFile, uFlags, pfnCallback, pUserData, pLoadOptions) \
      ((pL_EnumFileTags )?pL_EnumFileTags(pszFile, uFlags, pfnCallback, pUserData, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPDELETETAG(pszFile, nPage, uTag, uFlags, pSaveOptions) \
      ((pL_DeleteTag )?pL_DeleteTag(pszFile, nPage, uTag, uFlags, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETGEOKEY(uTag, uType, uCount, pData) \
      ((pL_SetGeoKey )?pL_SetGeoKey(uTag, uType, uCount, pData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETGEOKEY(uTag, puType, puCount, pData) \
      ((pL_GetGeoKey )?pL_GetGeoKey(uTag, puType, puCount, pData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPWRITEFILEGEOKEY(pszFile, pSaveOptions) \
      ((pL_WriteFileGeoKey )?pL_WriteFileGeoKey(pszFile, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILEGEOKEY(pszFile, uTag, puType, pCount, pData, pLoadOptions) \
      ((pL_ReadFileGeoKey )?pL_ReadFileGeoKey(pszFile, uTag, puType, pCount, pData, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPENUMFILEGEOKEYS(pszFile, uFlags, pfnCallback, pUserData, pLoadOptions) \
      ((pL_EnumFileGeoKeys )?pL_EnumFileGeoKeys(pszFile, uFlags, pfnCallback, pUserData, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILECOMMENTOFFSET(fd, nOffsetBegin, nBytesToLoad, uType, pComment, uLength, pLoadOptions) \
      ((pL_ReadFileCommentOffset )?pL_ReadFileCommentOffset(fd, nOffsetBegin, nBytesToLoad, uType, pComment, uLength, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETLOADSTATUS() \
      ((pL_GetLoadStatus )?pL_GetLoadStatus():WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPDECODEABIC(pInputData, nLength, ppOutputData, nAlign, nWidth, nHeight, bBiLevel) \
      ((pL_DecodeABIC )?pL_DecodeABIC(pInputData, nLength, ppOutputData, nAlign, nWidth, nHeight, bBiLevel):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPENCODEABIC(pInputData, nAlign, nWidth, nHeight, ppOutputData, pnLength, bBiLevel) \
      ((pL_EncodeABIC )?pL_EncodeABIC(pInputData, nAlign, nWidth, nHeight, ppOutputData, pnLength, bBiLevel):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETPNGTRNS(pData, uSize) \
      ((pL_SetPNGTRNS )?pL_SetPNGTRNS(pData, uSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETPNGTRNS(pData) \
      ((pL_GetPNGTRNS )?pL_GetPNGTRNS(pData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADBITMAPRESIZE(pszFile, pSmallBitmap, uStructSize, nDestWidth, nDestHeight, nDestBits, uFlags, nOrder, pLoadOptions, pFileInfo) \
      ((pL_LoadBitmapResize )?pL_LoadBitmapResize(pszFile, pSmallBitmap, uStructSize, nDestWidth, nDestHeight, nDestBits, uFlags, nOrder, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPREADFILETRANSFORMS(pszFile, pTransforms, pLoadOptions) \
      ((pL_ReadFileTransforms )?pL_ReadFileTransforms(pszFile, pTransforms, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPWRITEFILETRANSFORMS(pszFile, pTransforms, nFlags, pSaveOptions) \
      ((pL_WriteFileTransforms )?pL_WriteFileTransforms(pszFile, pTransforms, nFlags, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETPCDRESOLUTION(pszFile, pPCDInfo) \
      ((pL_GetPCDResolution )?pL_GetPCDResolution(pszFile, pPCDInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETWMFRESOLUTION(lpXResolution, lpYResolution) \
      ((pL_GetWMFResolution )?pL_GetWMFResolution(lpXResolution, lpYResolution):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETPCDRESOLUTION(nResolution) \
      ((pL_SetPCDResolution )?pL_SetPCDResolution(nResolution):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETWMFRESOLUTION(nXResolution, nYResolution) \
      ((pL_SetWMFResolution )?pL_SetWMFResolution(nXResolution, nYResolution):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVECUSTOMFILE(pszFilename, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pSaveOptions, pSaveCustomFileOption, pfnCallback, pUserData) \
      ((pL_SaveCustomFile )?pL_SaveCustomFile(pszFilename, pBitmap, nFormat, nBitsPerPixel, nQFactor, uFlags, pSaveOptions, pSaveCustomFileOption, pfnCallback, pUserData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADCUSTOMFILE(pszFile, pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnFileReadCallback, pFileReadCallbackUserData, pLoadOptions, pFileInfo, pLoadCustomFileOption, pfnLoadCustomFileCallback, pCustomCallbackUserData) \
      ((pL_LoadCustomFile )?pL_LoadCustomFile(pszFile, pBitmap, uStructSize, nBitsPerPixel, nOrder, uFlags, pfnFileReadCallback, pFileReadCallbackUserData, pLoadOptions, pFileInfo, pLoadCustomFileOption, pfnLoadCustomFileCallback, pCustomCallbackUserData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRP2DSETVIEWPORT(nWidth, nHeight) \
      ((pL_2DSetViewport )?pL_2DSetViewport(nWidth, nHeight):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRP2DGETVIEWPORT(pnWidth, pnHeight) \
      ((pL_2DGetViewport )?pL_2DGetViewport(pnWidth, pnHeight):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRP2DSETVIEWMODE(nViewMode) \
      ((pL_2DSetViewMode )?pL_2DSetViewMode(nViewMode):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRP2DGETVIEWMODE() \
      ((pL_2DGetViewMode )?pL_2DGetViewMode():WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECLOADFILE(pszFile, pVector, pLoadOptions, pFileInfo) \
      ((pL_VecLoadFile )?pL_VecLoadFile(pszFile, pVector, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECLOADMEMORY(pBuffer, pVector, nBufferSize, pLoadOptions, pFileInfo) \
      ((pL_VecLoadMemory )?pL_VecLoadMemory(pBuffer, pVector, nBufferSize, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECSTARTFEEDLOAD(pVector, phLoad, pLoadOptions, pFileInfo) \
      ((pL_VecStartFeedLoad )?pL_VecStartFeedLoad(pVector, phLoad, pLoadOptions, pFileInfo):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECFEEDLOAD(hLoad, pInBuffer, dwBufferSize) \
      ((pL_VecFeedLoad )?pL_VecFeedLoad(hLoad, pInBuffer, dwBufferSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECSTOPFEEDLOAD(hLoad) \
      ((pL_VecStopFeedLoad )?pL_VecStopFeedLoad(hLoad):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECSAVEFILE(pszFile, pVector, nFormat, pSaveOptions) \
      ((pL_VecSaveFile )?pL_VecSaveFile(pszFile, pVector, nFormat, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECSAVEMEMORY(hHandle, pVector, nFormat, uSize, pSaveOptions) \
      ((pL_VecSaveMemory )?pL_VecSaveMemory(hHandle, pVector, nFormat, uSize, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETPLTOPTIONS(pOptions, uStructSize) \
      ((pL_GetPLTOptions )?pL_GetPLTOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETPLTOPTIONS(pOptions) \
      ((pL_SetPLTOptions )?pL_SetPLTOptions(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETPDFINITDIR(pszInitDir, uBufSize) \
      ((pL_GetPDFInitDir )?pL_GetPDFInitDir(pszInitDir, uBufSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETPDFINITDIR(pszInitDir) \
      ((pL_SetPDFInitDir )?pL_SetPDFInitDir(pszInitDir):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETRTFOPTIONS(pOptions, uStructSize) \
      ((pL_GetRTFOptions )?pL_GetRTFOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETRTFOPTIONS(pOptions) \
      ((pL_SetRTFOptions )?pL_SetRTFOptions(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETPTKOPTIONS(pOptions, uStructSize) \
      ((pL_GetPTKOptions )?pL_GetPTKOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETPTKOPTIONS(pOptions) \
      ((pL_SetPTKOptions )?pL_SetPTKOptions(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETPDFOPTIONS(pOptions, uStructSize) \
      ((pL_GetPDFOptions )?pL_GetPDFOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETPDFOPTIONS(pOptions) \
      ((pL_SetPDFOptions )?pL_SetPDFOptions(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETPDFSAVEOPTIONS(pOptions, uStructSize) \
      ((pL_GetPDFSaveOptions )?pL_GetPDFSaveOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETPDFSAVEOPTIONS(pOptions) \
      ((pL_SetPDFSaveOptions )?pL_SetPDFSaveOptions(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETDJVOPTIONS(pOptions, uStructSize) \
      ((pL_GetDJVOptions )?pL_GetDJVOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETDJVOPTIONS(pOptions) \
      ((pL_SetDJVOptions )?pL_SetDJVOptions(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPLOADLAYER(pszFile, pBitmap, uStructSize, nBitsPerPixel, nOrder, nLayer, pLayerInfo, pLoadOptions) \
      ((pL_LoadLayer )?pL_LoadLayer(pszFile, pBitmap, uStructSize, nBitsPerPixel, nOrder, nLayer, pLayerInfo, pLoadOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSAVEBITMAPWITHLAYERS(pszFile, pBitmap, nFormat, nBitsPerPixel, nQFactor, hLayers, pLayerInfo, nLayers, pSaveOptions) \
      ((pL_SaveBitmapWithLayers )?pL_SaveBitmapWithLayers(pszFile, pBitmap, nFormat, nBitsPerPixel, nQFactor, hLayers, pLayerInfo, nLayers, pSaveOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETAUTOCADFILESCOLORSCHEME(dwFlags) \
      ((pL_GetAutoCADFilesColorScheme )?pL_GetAutoCADFilesColorScheme(dwFlags):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETAUTOCADFILESCOLORSCHEME(dwFlags) \
      ((pL_SetAutoCADFilesColorScheme )?pL_SetAutoCADFilesColorScheme(dwFlags):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECADDFONTMAPPER(pMapper, pUserData) \
      ((pL_VecAddFontMapper )?pL_VecAddFontMapper(pMapper, pUserData):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPVECREMOVEFONTMAPPER(pMapper) \
      ((pL_VecRemoveFontMapper )?pL_VecRemoveFontMapper(pMapper):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPSETXPSOPTIONS(pOptions) \
      ((pL_SetXPSOptions )?pL_SetXPSOptions(pOptions):WRPERR_LTFIL_DLL_NOT_LOADED)

#define L_WRPGETXPSOPTIONS(pOptions, uStructSize) \
      ((pL_GetXPSOptions )?pL_GetXPSOptions(pOptions, uStructSize):WRPERR_LTFIL_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTEFX.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPEFXPAINTTRANSITION(hDC, uTransition, crBack, crFore, uSteps, pDest, uEffect, uGrain, uDelay, nSpeed, nCycles, uPass, uMaxPass, fTransparency, crTransparency, uWandWidth, crWand, uROP) \
      ((pL_EfxPaintTransition )?pL_EfxPaintTransition(hDC, uTransition, crBack, crFore, uSteps, pDest, uEffect, uGrain, uDelay, nSpeed, nCycles, uPass, uMaxPass, fTransparency, crTransparency, uWandWidth, crWand, uROP):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXPAINTBITMAP(hDC, pBitmap, pSrc, pSrcClip, pDest, pDestClip, uEffect, uGrain, uDelay, nSpeed, nCycles, uPass, uMaxPass, fTransparency, crTransparency, uWandWidth, crWand, uROP) \
      ((pL_EfxPaintBitmap )?pL_EfxPaintBitmap(hDC, pBitmap, pSrc, pSrcClip, pDest, pDestClip, uEffect, uGrain, uDelay, nSpeed, nCycles, uPass, uMaxPass, fTransparency, crTransparency, uWandWidth, crWand, uROP):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXDRAWFRAME(hDC, pRect, uFlags, uFrameWidth, crFrame, uInnerWidth, crInner1, crInner2, uOuterWidth, crOuter1, crOuter2) \
      ((pL_EfxDrawFrame )?pL_EfxDrawFrame(hDC, pRect, uFlags, uFrameWidth, crFrame, uInnerWidth, crInner1, crInner2, uOuterWidth, crOuter1, crOuter2):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXTILERECT(hdcDest, prcDest, hdcSrc, prcSrc) \
      ((pL_EfxTileRect )?pL_EfxTileRect(hdcDest, prcDest, hdcSrc, prcSrc):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXGRADIENTFILLRECT(hDC, pRect, uStyle, crStart, crEnd, uSteps) \
      ((pL_EfxGradientFillRect )?pL_EfxGradientFillRect(hDC, pRect, uStyle, crStart, crEnd, uSteps):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXPATTERNFILLRECT(hDC, pRect, uStyle, crBack, crFore) \
      ((pL_EfxPatternFillRect )?pL_EfxPatternFillRect(hDC, pRect, uStyle, crBack, crFore):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXDRAW3DTEXT(hDC, pszText, pRect, uFlags, nXDepth, nYDepth, crText, crShadow, crHilite, hFont, hdcFore) \
      ((pL_EfxDraw3dText )?pL_EfxDraw3dText(hDC, pszText, pRect, uFlags, nXDepth, nYDepth, crText, crShadow, crHilite, hFont, hdcFore):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXEFFECTBLT(hdcDest, nXDest, nYDest, nWidth, nHeight, hdcSrc, nXSrc, nYSrc, uEffect, uGrain, uDelay, nSpeed, nCycles, uPass, uMaxPass, fTransparency, crTransparency, uWandWidth, crWand, uROP) \
      ((pL_EfxEffectBlt )?pL_EfxEffectBlt(hdcDest, nXDest, nYDest, nWidth, nHeight, hdcSrc, nXSrc, nYSrc, uEffect, uGrain, uDelay, nSpeed, nCycles, uPass, uMaxPass, fTransparency, crTransparency, uWandWidth, crWand, uROP):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPPAINTDCEFFECT(hDC, pBitmap, pSrc, pClipSrc, pDst, pClipDst, uROP3, uEffect) \
      ((pL_PaintDCEffect )?pL_PaintDCEffect(hDC, pBitmap, pSrc, pClipSrc, pDst, pClipDst, uROP3, uEffect):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPPAINTRGNDCEFFECT(hDC, pBitmap, pSrc, pClipSrc, pDst, pClipDst, uROP3, uEffect) \
      ((pL_PaintRgnDCEffect )?pL_PaintRgnDCEffect(hDC, pBitmap, pSrc, pClipSrc, pDst, pClipDst, uROP3, uEffect):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXDRAWROTATED3DTEXT(hDC, pszText, pRect, nAngle, uFlags, nXDepth, nYDepth, crText, crShadow, crHilite, hFont, hdcFore) \
      ((pL_EfxDrawRotated3dText )?pL_EfxDrawRotated3dText(hDC, pszText, pRect, nAngle, uFlags, nXDepth, nYDepth, crText, crShadow, crHilite, hFont, hdcFore):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPEFXDRAW3DSHAPE(hDC, uShape, pRect, crBack, hdcBack, prcBack, uBackStyle, crFill, uFillStyle, crBorder, uBorderStyle, uBorderWidth, crInnerHilite, crInnerShadow, uInnerStyle, uInnerWidth, crOuterHilite, crOuterShadow, uOuterStyle, uOuterWidth, nShadowX, nShadowY, crShadow, hRgn) \
      ((pL_EfxDraw3dShape )?pL_EfxDraw3dShape(hDC, uShape, pRect, crBack, hdcBack, prcBack, uBackStyle, crFill, uFillStyle, crBorder, uBorderStyle, uBorderWidth, crInnerHilite, crInnerShadow, uInnerStyle, uInnerWidth, crOuterHilite, crOuterShadow, uOuterStyle, uOuterWidth, nShadowX, nShadowY, crShadow, hRgn):WRPERR_LTANN_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTDLG.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPDLGINIT(uFlags) \
   ((pL_DlgInit) ? pL_DlgInit(uFlags) : WRPERR_LTDLGKRN_DLL_NOT_LOADED)

#define L_WRPDLGFREE() \
   ((pL_DlgFree) ? pL_DlgFree() : WRPERR_LTDLGKRN_DLL_NOT_LOADED)

#define L_WRPDLGSETFONT(hFont) \
   ((pL_DlgSetFont) ? pL_DlgSetFont(hFont) : (LBase::RecordError(WRPERR_LTDLGKRN_DLL_NOT_LOADED),(HFONT)NULL))

#define L_WRPDLGGETSTRINGLEN(uString, puLen) \
   ((pL_DlgGetStringLen) ? pL_DlgGetStringLen(uString, puLen) : WRPERR_LTDLGKRN_DLL_NOT_LOADED)

#define L_WRPDLGSETSTRING(uString,szString) \
   ((pL_DlgSetString) ? pL_DlgSetString(uString,szString) : WRPERR_LTDLGKRN_DLL_NOT_LOADED)

#define L_WRPDLGGETSTRING(uString,szString, sizeInWords) \
   ((pL_DlgGetString) ? pL_DlgGetString(uString,szString,sizeInWords) : WRPERR_LTDLGKRN_DLL_NOT_LOADED)

#define L_WRPDLGBALANCECOLORS(hWndOwner, pDlgParams) \
   ((pL_DlgBalanceColors) ? pL_DlgBalanceColors(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGCOLOREDGRAY(hWndOwner, pDlgParams) \
   ((pL_DlgColoredGray) ? pL_DlgColoredGray(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGGRAYSCALE(hWndOwner, pDlgParams) \
   ((pL_DlgGrayScale) ? pL_DlgGrayScale(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGREMAPINTENSITY(hWndOwner, pDlgParams) \
   ((pL_DlgRemapIntensity) ? pL_DlgRemapIntensity(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGREMAPHUE(hWndOwner, pDlgParams) \
   ((pL_DlgRemapHue) ? pL_DlgRemapHue(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGCUSTOMIZEPALETTE(hWndOwner, pDlgParams) \
   ((pL_DlgCustomizePalette) ? pL_DlgCustomizePalette(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGLOCALHISTOEQUALIZE(hWndOwner, pDlgParams) \
   ((pL_DlgLocalHistoEqualize) ? pL_DlgLocalHistoEqualize(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGINTENSITYDETECT(hWndOwner, pDlgParams) \
   ((pL_DlgIntensityDetect) ? pL_DlgIntensityDetect(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGSOLARIZE(hWndOwner, pDlgParams) \
   ((pL_DlgSolarize) ? pL_DlgSolarize(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGPOSTERIZE(hWndOwner, pDlgParams) \
   ((pL_DlgPosterize) ? pL_DlgPosterize(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGBRIGHTNESS(hWndOwner, pDlgParams) \
   ((pL_DlgBrightness) ? pL_DlgBrightness(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGCONTRAST(hWndOwner, pDlgParams) \
   ((pL_DlgContrast) ? pL_DlgContrast(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGHUE(hWndOwner, pDlgParams) \
   ((pL_DlgHue) ? pL_DlgHue(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGSATURATION(hWndOwner, pDlgParams) \
   ((pL_DlgSaturation) ? pL_DlgSaturation(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGGAMMAADJUSTMENT(hWndOwner, pDlgParams) \
   ((pL_DlgGammaAdjustment) ? pL_DlgGammaAdjustment(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGHALFTONE(hWndOwner, pDlgParams) \
   ((pL_DlgHalftone) ? pL_DlgHalftone(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGCOLORRES(hWndOwner, pDlgParams) \
   ((pL_DlgColorRes) ? pL_DlgColorRes(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGHISTOCONTRAST(hWndOwner, pDlgParams) \
   ((pL_DlgHistoContrast) ? pL_DlgHistoContrast(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGWINDOWLEVEL(hWndOwner, pDlgParams) \
   ((pL_DlgWindowLevel) ? pL_DlgWindowLevel(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGCOLOR(hWndOwner, pDlgParams) \
   ((pL_DlgColor) ? pL_DlgColor(hWndOwner, pDlgParams) : WRPERR_LTDLGCLR_DLL_NOT_LOADED)

#define L_WRPDLGMOTIONBLUR(hWndOwner, pDlgParams) \
   ((pL_DlgMotionBlur) ? pL_DlgMotionBlur(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGRADIALBLUR(hWndOwner, pDlgParams) \
   ((pL_DlgRadialBlur) ? pL_DlgRadialBlur(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGZOOMBLUR(hWndOwner, pDlgParams) \
   ((pL_DlgZoomBlur) ? pL_DlgZoomBlur(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGGAUSSIANBLUR(hWndOwner, pDlgParams) \
   ((pL_DlgGaussianBlur) ? pL_DlgGaussianBlur(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGANTIALIAS(hWndOwner, pDlgParams) \
   ((pL_DlgAntiAlias) ? pL_DlgAntiAlias(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGAVERAGE(hWndOwner, pDlgParams) \
   ((pL_DlgAverage) ? pL_DlgAverage(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGMEDIAN(hWndOwner, pDlgParams) \
   ((pL_DlgMedian) ? pL_DlgMedian(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGADDNOISE(hWndOwner, pDlgParams) \
   ((pL_DlgAddNoise) ? pL_DlgAddNoise(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGMAXFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgMaxFilter) ? pL_DlgMaxFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGMINFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgMinFilter) ? pL_DlgMinFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGSHARPEN(hWndOwner, pDlgParams) \
   ((pL_DlgSharpen) ? pL_DlgSharpen(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGSHIFTDIFFERENCEFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgShiftDifferenceFilter) ? pL_DlgShiftDifferenceFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGEMBOSS(hWndOwner, pDlgParams) \
   ((pL_DlgEmboss) ? pL_DlgEmboss(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGOILIFY(hWndOwner, pDlgParams) \
   ((pL_DlgOilify) ? pL_DlgOilify(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGMOSAIC(hWndOwner, pDlgParams) \
   ((pL_DlgMosaic) ? pL_DlgMosaic(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGEROSIONFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgErosionFilter) ? pL_DlgErosionFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGDILATIONFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgDilationFilter) ? pL_DlgDilationFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGCONTOURFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgContourFilter) ? pL_DlgContourFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGGRADIENTFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgGradientFilter) ? pL_DlgGradientFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGLAPLACIANFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgLaplacianFilter) ? pL_DlgLaplacianFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGSOBELFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgSobelFilter) ? pL_DlgSobelFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGPREWITTFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgPrewittFilter) ? pL_DlgPrewittFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGLINESEGMENTFILTER(hWndOwner, pDlgParams) \
   ((pL_DlgLineSegmentFilter) ? pL_DlgLineSegmentFilter(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGUNSHARPMASK(hWndOwner, pDlgParams) \
   ((pL_DlgUnsharpMask) ? pL_DlgUnsharpMask(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGMULTIPLY(hWndOwner, pDlgParams) \
   ((pL_DlgMultiply) ? pL_DlgMultiply(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGADDBITMAPS(hWndOwner, pDlgParams) \
   ((pL_DlgAddBitmaps) ? pL_DlgAddBitmaps(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGSTITCH(hWndOwner, pDlgParams) \
   ((pL_DlgStitch) ? pL_DlgStitch(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGFREEHANDWAVE(hWndOwner, pDlgParams) \
   ((pL_DlgFreeHandWave) ? pL_DlgFreeHandWave(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGWIND(hWndOwner, pDlgParams) \
   ((pL_DlgWind) ? pL_DlgWind(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGPOLAR(hWndOwner, pDlgParams) \
   ((pL_DlgPolar) ? pL_DlgPolar(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGZOOMWAVE(hWndOwner, pDlgParams) \
   ((pL_DlgZoomWave) ? pL_DlgZoomWave(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGRADIALWAVE(hWndOwner, pDlgParams) \
   ((pL_DlgRadialWave) ? pL_DlgRadialWave(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGSWIRL(hWndOwner, pDlgParams) \
   ((pL_DlgSwirl) ? pL_DlgSwirl(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGWAVE(hWndOwner, pDlgParams) \
   ((pL_DlgWave) ? pL_DlgWave(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGWAVESHEAR(hWndOwner, pDlgParams) \
   ((pL_DlgWaveShear) ? pL_DlgWaveShear(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGPUNCH(hWndOwner, pDlgParams) \
   ((pL_DlgPunch) ? pL_DlgPunch(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGRIPPLE(hWndOwner, pDlgParams) \
   ((pL_DlgRipple) ? pL_DlgRipple(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGBENDING(hWndOwner, pDlgParams) \
   ((pL_DlgBending) ? pL_DlgBending(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGCYLINDRICAL(hWndOwner, pDlgParams) \
   ((pL_DlgCylindrical) ? pL_DlgCylindrical(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGSPHERIZE(hWndOwner, pDlgParams) \
   ((pL_DlgSpherize) ? pL_DlgSpherize(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGIMPRESSIONIST(hWndOwner, pDlgParams) \
   ((pL_DlgImpressionist) ? pL_DlgImpressionist(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGPIXELATE(hWndOwner, pDlgParams) \
   ((pL_DlgPixelate) ? pL_DlgPixelate(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGEDGEDETECTOR(hWndOwner, pDlgParams) \
   ((pL_DlgEdgeDetector) ? pL_DlgEdgeDetector(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGUNDERLAY(hWndOwner, pDlgParams) \
   ((pL_DlgUnderlay) ? pL_DlgUnderlay(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGPICTURIZE(hWndOwner,  pDlgParams) \
   ((pL_DlgPicturize) ? pL_DlgPicturize(hWndOwner,  pDlgParams) : WRPERR_LTDLGIMGEFX_DLL_NOT_LOADED)

#define L_WRPDLGROTATE(hWndOwner, pDlgParams) \
   ((pL_DlgRotate) ? pL_DlgRotate(hWndOwner, pDlgParams) : WRPERR_LTDLGIMG_DLL_NOT_LOADED)

#define L_WRPDLGSHEAR(hWndOwner, pDlgParams) \
   ((pL_DlgShear) ? pL_DlgShear(hWndOwner, pDlgParams) : WRPERR_LTDLGIMG_DLL_NOT_LOADED)

#define L_WRPDLGRESIZE(hWndOwner, pDlgParams) \
   ((pL_DlgResize) ? pL_DlgResize(hWndOwner, pDlgParams) : WRPERR_LTDLGIMG_DLL_NOT_LOADED)

#define L_WRPDLGADDBORDER(hWndOwner, pDlgParams) \
   ((pL_DlgAddBorder) ? pL_DlgAddBorder(hWndOwner, pDlgParams) : WRPERR_LTDLGIMG_DLL_NOT_LOADED)

#define L_WRPDLGADDFRAME(hWndOwner, pDlgParams) \
   ((pL_DlgAddFrame) ? pL_DlgAddFrame(hWndOwner, pDlgParams) : WRPERR_LTDLGIMG_DLL_NOT_LOADED)

#define L_WRPDLGAUTOTRIM(hWndOwner, pDlgParams) \
   ((pL_DlgAutoTrim) ? pL_DlgAutoTrim(hWndOwner, pDlgParams) : WRPERR_LTDLGIMG_DLL_NOT_LOADED)

#define L_WRPDLGCANVASRESIZE(hWndOwner, pDlgParams) \
   ((pL_DlgCanvasResize) ? pL_DlgCanvasResize(hWndOwner, pDlgParams) : WRPERR_LTDLGIMG_DLL_NOT_LOADED)

#define L_WRPDLGHISTOGRAM(hWndOwner, pDlgParams) \
   ((pL_DlgHistogram) ? pL_DlgHistogram(hWndOwner, pDlgParams) : WRPERR_LTDLGIMG_DLL_NOT_LOADED)

//{{ Web dialogs C DLL's group - LTDlgWeb14?.dll
#define L_WRPDLGPNGWEBTUNER(hWndOwner, pDlgParams) \
   ((pL_DlgPNGWebTuner) ? pL_DlgPNGWebTuner(hWndOwner, pDlgParams) : WRPERR_LTDLGWEB_DLL_NOT_LOADED)

#define L_WRPDLGGIFWEBTUNER(hWndOwner, pDlgParams) \
   ((pL_DlgGIFWebTuner) ? pL_DlgGIFWebTuner(hWndOwner, pDlgParams) : WRPERR_LTDLGWEB_DLL_NOT_LOADED)

#define L_WRPDLGJPEGWEBTUNER(hWndOwner, pDlgParams) \
   ((pL_DlgJPEGWebTuner) ? pL_DlgJPEGWebTuner(hWndOwner, pDlgParams) : WRPERR_LTDLGWEB_DLL_NOT_LOADED)

#define L_WRPDLGHTMLMAPPER(hWndOwner, pDlgParams) \
   ((pL_DlgHTMLMapper) ? pL_DlgHTMLMapper(hWndOwner, pDlgParams) : WRPERR_LTDLGWEB_DLL_NOT_LOADED)

//{{ File dialogs C DLL's group - LTDlgFile14?.dll
#define L_WRPDLGGETDIRECTORY(hWndOwner, pDlgParams) \
   ((pL_DlgGetDirectory) ? pL_DlgGetDirectory(hWndOwner, pDlgParams) : WRPERR_LTDLGFILE_DLL_NOT_LOADED)

#define L_WRPDLGFILECONVERSION(hWndOwner, pDlgParams) \
   ((pL_DlgFileConversion) ? pL_DlgFileConversion(hWndOwner, pDlgParams) : WRPERR_LTDLGFILE_DLL_NOT_LOADED)

#define L_WRPDLGFILESASSOCIATION(hWndOwner, pDlgParams) \
   ((pL_DlgFilesAssociation) ? pL_DlgFilesAssociation(hWndOwner, pDlgParams) : WRPERR_LTDLGFILE_DLL_NOT_LOADED)

#define L_WRPDLGPRINTSTITCHEDIMAGES(hWndOwner, pDlgParams) \
   ((pL_DlgPrintStitchedImages) ? pL_DlgPrintStitchedImages(hWndOwner, pDlgParams) : WRPERR_LTDLGFILE_DLL_NOT_LOADED)

#define L_WRPDLGPRINTPREVIEW(hWndOwner, pDlgParams) \
   ((pL_DlgPrintPreview) ? pL_DlgPrintPreview(hWndOwner, pDlgParams) : WRPERR_LTDLGFILE_DLL_NOT_LOADED)

#define L_WRPDLGSAVE(hWndOwner, pOpenFileName, pDlgParams) \
   ((pL_DlgSave) ? pL_DlgSave(hWndOwner, pOpenFileName, pDlgParams) : WRPERR_LTDLGFILE_DLL_NOT_LOADED)

#define L_WRPDLGOPEN(hWndOwner,pOpenFileName,pDlgParams ) \
   ((pL_DlgOpen) ? pL_DlgOpen(hWndOwner,pOpenFileName,pDlgParams ) : WRPERR_LTDLGFILE_DLL_NOT_LOADED)

#define L_WRPDLGICCPROFILE( hWndOwner, pDlgParams ) \
   ((pL_DlgICCProfile) ? pL_DlgICCProfile( hWndOwner, pDlgParams ) : WRPERR_LTDLGFILE_DLL_NOT_LOADED)

//{{ Effects dialogs C DLL's group - LTDlgEfx14?.dll
#define L_WRPDLGGETSHAPE(hWndOwner, pDlgParams) \
   ((pL_DlgGetShape) ? pL_DlgGetShape(hWndOwner, pDlgParams) : WRPERR_LTDLGEFX_DLL_NOT_LOADED)

#define L_WRPDLGGETEFFECT(hWndOwner, pDlgParams) \
   ((pL_DlgGetEffect) ? pL_DlgGetEffect(hWndOwner, pDlgParams) : WRPERR_LTDLGEFX_DLL_NOT_LOADED)

#define L_WRPDLGGETTRANSITION(hWndOwner, pDlgParams) \
   ((pL_DlgGetTransition) ? pL_DlgGetTransition(hWndOwner, pDlgParams) : WRPERR_LTDLGEFX_DLL_NOT_LOADED)

#define L_WRPDLGGETGRADIENT(hWndOwner, pDlgParams) \
   ((pL_DlgGetGradient) ? pL_DlgGetGradient(hWndOwner, pDlgParams) : WRPERR_LTDLGEFX_DLL_NOT_LOADED)

#define L_WRPDLGGETTEXT(hWndOwner,  pDlgParams) \
   ((pL_DlgGetText) ? pL_DlgGetText(hWndOwner,  pDlgParams) : WRPERR_LTDLGEFX_DLL_NOT_LOADED)

//{{ Document Image dialogs C DLL's group - LTDlgImgDoc14?.dll
#define L_WRPDLGSMOOTH(hWndOwner, pDlgParams) \
   ((pL_DlgSmooth) ? pL_DlgSmooth(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGDOC_DLL_NOT_LOADED)

#define L_WRPDLGLINEREMOVE(hWndOwner, pDlgParams) \
   ((pL_DlgLineRemove) ? pL_DlgLineRemove(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGDOC_DLL_NOT_LOADED)

#define L_WRPDLGBORDERREMOVE(hWndOwner, pDlgParams) \
   ((pL_DlgBorderRemove) ? pL_DlgBorderRemove(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGDOC_DLL_NOT_LOADED)

#define L_WRPDLGINVERTEDTEXT(hWndOwner, pDlgParams) \
   ((pL_DlgInvertedText) ? pL_DlgInvertedText(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGDOC_DLL_NOT_LOADED)

#define L_WRPDLGDOTREMOVE(hWndOwner, pDlgParams) \
   ((pL_DlgDotRemove) ? pL_DlgDotRemove(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGDOC_DLL_NOT_LOADED)

#define L_WRPDLGHOLEPUNCHREMOVE(hWndOwner, pDlgParams) \
   ((pL_DlgHolePunchRemove) ? pL_DlgHolePunchRemove(hWndOwner, pDlgParams) : WRPERR_LTDLGIMGDOC_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTTW2.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPTWAININITSESSION(phSession,pAppData) \
      ((pL_TwainInitSession )?pL_TwainInitSession(phSession,pAppData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAININITSESSION2(phSession,pAppData,uFlags) \
      ((pL_TwainInitSession2 )?pL_TwainInitSession2(phSession,pAppData,uFlags):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINENDSESSION(phSession) \
      ((pL_TwainEndSession )?pL_TwainEndSession(phSession):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETPROPERTIES(hSession,pltProperties,uFlags,pfnCallBack,pUserData) \
      ((pL_TwainSetProperties )?pL_TwainSetProperties(hSession,pltProperties,uFlags,pfnCallBack,pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETPROPERTIES(hSession,pltProperties,uStructSize,uFlags) \
      ((pL_TwainGetProperties )?pL_TwainGetProperties(hSession,pltProperties,uStructSize,uFlags):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINACQUIRELIST(hSession,hBitmap,lpszTemplateFile,uFlags) \
   ((pL_TwainAcquireList )?pL_TwainAcquireList(hSession,hBitmap,lpszTemplateFile,uFlags):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINACQUIRE(hSession,pBitmap,uStructSize,pfnCallBack,uFlags,lpszTemplateFile,pUserData) \
      ((pL_TwainAcquire )?pL_TwainAcquire(hSession,pBitmap,uStructSize,pfnCallBack,uFlags,lpszTemplateFile,pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSELECTSOURCE(hSession,pltSource) \
      ((pL_TwainSelectSource )?pL_TwainSelectSource(hSession,pltSource):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINQUERYPROPERTY(hSession,uCapability,ppltProperty,uStructSize) \
      ((pL_TwainQueryProperty )?pL_TwainQueryProperty(hSession,uCapability,ppltProperty,uStructSize):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSTARTCAPSNEG(hSession) \
      ((pL_TwainStartCapsNeg )?pL_TwainStartCapsNeg(hSession):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINENDCAPSNEG(hSession) \
      ((pL_TwainEndCapsNeg )?pL_TwainEndCapsNeg(hSession):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETCAPABILITY(hSession,pCapability,uFlags) \
      ((pL_TwainSetCapability )?pL_TwainSetCapability(hSession,pCapability,uFlags):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETCAPABILITY(hSession,pCapability,uFlags) \
      ((pL_TwainGetCapability )?pL_TwainGetCapability(hSession,pCapability,uFlags):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINENUMCAPABILITIES(hSession,pfnCallBack,uFlags,pUserData) \
      ((pL_TwainEnumCapabilities )?pL_TwainEnumCapabilities(hSession,pfnCallBack,uFlags,pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINCREATENUMERICCONTAINERONEVALUE(pCapability,Type,uValue) \
      ((pL_TwainCreateNumericContainerOneValue )?pL_TwainCreateNumericContainerOneValue(pCapability,Type,uValue):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINCREATENUMERICCONTAINERRANGE(pCapability,Type,uMinValue,uMaxValue,uStepSize,uDefaultValue,uCurrentValue) \
      ((pL_TwainCreateNumericContainerRange )?pL_TwainCreateNumericContainerRange(pCapability,Type,uMinValue,uMaxValue,uStepSize,uDefaultValue,uCurrentValue):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINCREATENUMERICCONTAINERARRAY(pCapability,Type,uNumOfItems,pData) \
      ((pL_TwainCreateNumericContainerArray )?pL_TwainCreateNumericContainerArray(pCapability,Type,uNumOfItems,pData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINCREATENUMERICCONTAINERENUM(pCapability,Type,uNumOfItems,uCurrentIndex,uDefaultIndex,pData) \
      ((pL_TwainCreateNumericContainerEnum )?pL_TwainCreateNumericContainerEnum(pCapability,Type,uNumOfItems,uCurrentIndex,uDefaultIndex,pData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERVALUE(pCapability,nIndex,ppValue) \
      ((pL_TwainGetNumericContainerValue )?pL_TwainGetNumericContainerValue(pCapability,nIndex,ppValue):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINFREECONTAINER(pCapability) \
      ((pL_TwainFreeContainer )?pL_TwainFreeContainer(pCapability):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINFREEPROPQUERYSTRUCTURE(ppltProperty) \
      ((pL_TwainFreePropQueryStructure )?pL_TwainFreePropQueryStructure(ppltProperty):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINTEMPLATEDLG(hSession,lpszTemplateFile,pfnCallBack,pfnErCallBack,pUserData) \
      ((pL_TwainTemplateDlg )?pL_TwainTemplateDlg(hSession,lpszTemplateFile,pfnCallBack,pfnErCallBack,pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINOPENTEMPLATEFILE(hSession,phFile,lpszTemplateFile,uAccess) \
      ((pL_TwainOpenTemplateFile )?pL_TwainOpenTemplateFile(hSession,phFile,lpszTemplateFile,uAccess):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINADDCAPABILITYTOFILE(hSession,hFile,pCapability) \
      ((pL_TwainAddCapabilityToFile )?pL_TwainAddCapabilityToFile(hSession,hFile,pCapability):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETCAPABILITYFROMFILE(hSession,hFile,ppCapability,uIndex) \
      ((pL_TwainGetCapabilityFromFile )?pL_TwainGetCapabilityFromFile(hSession,hFile,ppCapability,uIndex):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMOFCAPSINFILE(hSession,hFile,puCapCount) \
      ((pL_TwainGetNumOfCapsInFile )?pL_TwainGetNumOfCapsInFile(hSession,hFile,puCapCount):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINCLOSETEMPLATEFILE(hSession,hFile) \
      ((pL_TwainCloseTemplateFile )?pL_TwainCloseTemplateFile(hSession,hFile):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETEXTENDEDIMAGEINFO(hSession,ptwExtImgInfo) \
      ((pL_TwainGetExtendedImageInfo )?pL_TwainGetExtendedImageInfo(hSession,ptwExtImgInfo):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINFREEEXTENDEDIMAGEINFOSTRUCTURE(pptwExtImgInfo) \
      ((pL_TwainFreeExtendedImageInfoStructure )?pL_TwainFreeExtendedImageInfoStructure(pptwExtImgInfo):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINLOCKCONTAINER(pCapability,ppContainer) \
      ((pL_TwainLockContainer )?pL_TwainLockContainer(pCapability,ppContainer):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINUNLOCKCONTAINER(pCapability) \
      ((pL_TwainUnlockContainer )?pL_TwainUnlockContainer(pCapability):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERITEMTYPE(pCapability,pnItemType) \
      ((pL_TwainGetNumericContainerItemType )?pL_TwainGetNumericContainerItemType(pCapability,pnItemType):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERINTVALUE(pCapability,nIndex,pnValue) \
      ((pL_TwainGetNumericContainerINTValue )?pL_TwainGetNumericContainerINTValue(pCapability,nIndex,pnValue):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERUINTVALUE(pCapability,nIndex,puValue) \
      ((pL_TwainGetNumericContainerUINTValue )?pL_TwainGetNumericContainerUINTValue(pCapability,nIndex,puValue):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERBOOLVALUE(pCapability,nIndex,pbValue) \
      ((pL_TwainGetNumericContainerBOOLValue )?pL_TwainGetNumericContainerBOOLValue(pCapability,nIndex,pbValue):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERFIX32VALUE(pCapability,nIndex,ptwFix) \
      ((pL_TwainGetNumericContainerFIX32Value )?pL_TwainGetNumericContainerFIX32Value(pCapability,nIndex,ptwFix):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERFRAMEVALUE(pCapability,nIndex,ptwFrame) \
      ((pL_TwainGetNumericContainerFRAMEValue )?pL_TwainGetNumericContainerFRAMEValue(pCapability,nIndex,ptwFrame):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERSTRINGVALUE(pCapability,nIndex,twString) \
      ((pL_TwainGetNumericContainerSTRINGValue )?pL_TwainGetNumericContainerSTRINGValue(pCapability,nIndex,twString):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETNUMERICCONTAINERUNICODEVALUE(pCapability,nIndex,twUniCode) \
      ((pL_TwainGetNumericContainerUNICODEValue )?pL_TwainGetNumericContainerUNICODEValue(pCapability,nIndex,twUniCode):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINACQUIREMULTI(hSession, pszBaseFileName,uFlags,uTransferMode,nFormat,nBitsPerPixel,bMultiPageFile,uUserBufSize,bUsePrefferedBuffer,pfnCallBack,pUserData) \
      ((pL_TwainAcquireMulti )?pL_TwainAcquireMulti(hSession, pszBaseFileName,uFlags,uTransferMode,nFormat,nBitsPerPixel,bMultiPageFile,uUserBufSize,bUsePrefferedBuffer,pfnCallBack,pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPISTWAINAVAILABLE(hWnd) \
      ((pL_IsTwainAvailable )?pL_IsTwainAvailable(hWnd):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINFINDFASTCONFIG(hSession,pszWorkingFolder,uFlags,nBitsPerPixel,nBufferIteration,pInFastConfigs,nInFastConfigsCount,ppTestConfigs,pnTestConfigsCount,pOutBestConfig,uStructSize,pfnCallBack,pUserData) \
      ((pL_TwainFindFastConfig )?pL_TwainFindFastConfig(hSession,pszWorkingFolder,uFlags,nBitsPerPixel,nBufferIteration,pInFastConfigs,nInFastConfigsCount,ppTestConfigs,pnTestConfigsCount,pOutBestConfig,uStructSize,pfnCallBack,pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETSCANCONFIGS(hSession,nBitsPerPixel,uTransferMode,nBufferIteration,ppFastConfig,uStructSize,pnFastConfigCount) \
      ((pL_TwainGetScanConfigs )?pL_TwainGetScanConfigs(hSession,nBitsPerPixel,uTransferMode,nBufferIteration,ppFastConfig,uStructSize,pnFastConfigCount):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINFREESCANCONFIG(hSession,ppFastConfig,nFastConfigCount) \
      ((pL_TwainFreeScanConfig )?pL_TwainFreeScanConfig(hSession,ppFastConfig,nFastConfigCount):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETSOURCES(hSession,pfnCallBack,uStructSize,uFlags,pUserData) \
      ((pL_TwainGetSources )?pL_TwainGetSources(hSession,pfnCallBack,uStructSize,uFlags,pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINENABLESHOWUSERINTERFACEONLY(hSession,bEnable) \
      ((pL_TwainEnableShowUserInterfaceOnly )?pL_TwainEnableShowUserInterfaceOnly(hSession,bEnable):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINCANCELACQUIRE(hSession) \
      ((pL_TwainCancelAcquire )?pL_TwainCancelAcquire(hSession):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINQUERYFILESYSTEM(hSession,FileMsg,pTwFile) \
      ((pL_TwainQueryFileSystem )?pL_TwainQueryFileSystem(hSession,FileMsg,pTwFile):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETJPEGCOMPRESSION(hSession,pTwJpegComp,uFlag) \
      ((pL_TwainGetJPEGCompression )?pL_TwainGetJPEGCompression(hSession,pTwJpegComp,uFlag):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETJPEGCOMPRESSION(hSession,pTwJpegComp,uFlag) \
      ((pL_TwainSetJPEGCompression )?pL_TwainSetJPEGCompression(hSession,pTwJpegComp,uFlag):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETTRANSFEROPTIONS(hSession,pTransferOpts) \
      ((pL_TwainSetTransferOptions )?pL_TwainSetTransferOptions(hSession,pTransferOpts):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETTRANSFEROPTIONS(hSession,pTransferOpts,uStructSize) \
      ((pL_TwainGetTransferOptions )?pL_TwainGetTransferOptions(hSession,pTransferOpts,uStructSize):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETSUPPORTEDTRANSFERMODE(hSession,pTransferModes) \
      ((pL_TwainGetSupportedTransferMode )?pL_TwainGetSupportedTransferMode(hSession,pTransferModes):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETRESOLUTION(hSession,pXRes,pYRes) \
      ((pL_TwainSetResolution )?pL_TwainSetResolution(hSession,pXRes,pYRes):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETRESOLUTION(hSession,pXRes,pYRes) \
      ((pL_TwainGetResolution )?pL_TwainGetResolution(hSession,pXRes,pYRes):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETIMAGEFRAME(hSession,pFrame) \
      ((pL_TwainSetImageFrame )?pL_TwainSetImageFrame(hSession,pFrame):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETIMAGEFRAME(hSession,pFrame) \
      ((pL_TwainGetImageFrame )?pL_TwainGetImageFrame(hSession,pFrame):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETIMAGEUNIT(hSession,nUnit) \
      ((pL_TwainSetImageUnit )?pL_TwainSetImageUnit(hSession,nUnit):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETIMAGEUNIT(hSession,pnUnit) \
      ((pL_TwainGetImageUnit )?pL_TwainGetImageUnit(hSession,pnUnit):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETIMAGEBITSPERPIXEL(hSession,nBitsPerPixel) \
      ((pL_TwainSetImageBitsPerPixel )?pL_TwainSetImageBitsPerPixel(hSession,nBitsPerPixel):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETIMAGEBITSPERPIXEL(hSession,pnBitsPerPixel) \
      ((pL_TwainGetImageBitsPerPixel )?pL_TwainGetImageBitsPerPixel(hSession,pnBitsPerPixel):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETIMAGEEFFECTS(hSession,ulFlags,pBrightness,pContrast,pHighlight) \
      ((pL_TwainSetImageEffects )?pL_TwainSetImageEffects(hSession,ulFlags,pBrightness,pContrast,pHighlight):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETIMAGEEFFECTS(hSession,ulFlags,pBrightness,pContrast,pHighlight) \
      ((pL_TwainGetImageEffects )?pL_TwainGetImageEffects(hSession,ulFlags,pBrightness,pContrast,pHighlight):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETACQUIREPAGEOPTIONS(hSession,nPaperSize,nPaperDirection) \
      ((pL_TwainSetAcquirePageOptions )?pL_TwainSetAcquirePageOptions(hSession,nPaperSize,nPaperDirection):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETACQUIREPAGEOPTIONS(hSession,pnPaperSize,pnPaperDirection) \
      ((pL_TwainGetAcquirePageOptions )?pL_TwainGetAcquirePageOptions(hSession,pnPaperSize,pnPaperDirection):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETRGBRESPONSE(hSession,pRgbResponse,nBitsPerPixel,uFlag) \
      ((pL_TwainSetRGBResponse )?pL_TwainSetRGBResponse(hSession,pRgbResponse,nBitsPerPixel,uFlag):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSHOWPROGRESS(hSession,bShow) \
      ((pL_TwainShowProgress )?pL_TwainShowProgress(hSession,bShow):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINENABLEDUPLEX(hSession,bEnableDuplex) \
      ((pL_TwainEnableDuplex )?pL_TwainEnableDuplex(hSession,bEnableDuplex):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETDUPLEXOPTIONS(hSession,pbEnableDuplex,pnDuplexMode) \
      ((pL_TwainGetDuplexOptions )?pL_TwainGetDuplexOptions(hSession,pbEnableDuplex,pnDuplexMode):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETMAXXFERCOUNT(hSession,nMaxXferCount) \
      ((pL_TwainSetMaxXferCount )?pL_TwainSetMaxXferCount(hSession,nMaxXferCount):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETMAXXFERCOUNT(hSession,pnMaxXferCount) \
      ((pL_TwainGetMaxXferCount )?pL_TwainGetMaxXferCount(hSession,pnMaxXferCount):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSTOPFEEDER(hSession) \
      ((pL_TwainStopFeeder )?pL_TwainStopFeeder(hSession):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETDEVICEEVENTCALLBACK(hSession, pfnCallBack, pUserData) \
      ((pL_TwainSetDeviceEventCallback)? pL_TwainSetDeviceEventCallback(hSession, pfnCallBack, pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETDEVICEEVENTDATA(hSession, pDeviceEvent) \
      ((pL_TwainGetDeviceEventData)?pL_TwainGetDeviceEventData(hSession, pDeviceEvent):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETDEVICEEVENTCAPABILITY(hSession, pDeviceCap) \
      ((pL_TwainSetDeviceEventCapability)?pL_TwainSetDeviceEventCapability(hSession, pDeviceCap):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINGETDEVICEEVENTCAPABILITY(hSession, pDeviceCap) \
      ((pL_TwainGetDeviceEventCapability)?pL_TwainGetDeviceEventCapability(hSession, pDeviceCap):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINRESETDEVICEEVENTCAPABILITY(hSession, pDeviceCap) \
      ((pL_TwainResetDeviceEventCapability)?pL_TwainResetDeviceEventCapability(hSession, pDeviceCap):WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINFASTACQUIRE(hSession, pszBaseFileName,uFlags,uTransferMode,nFormat,nBitsPerPixel,bMultiPageFile,uUserBufSize,bUsePrefferedBuffer,pfnCallBack,pUserData) \
      ((pL_TwainFastAcquire)?pL_TwainFastAcquire(hSession, pszBaseFileName,uFlags,uTransferMode,nFormat,nBitsPerPixel,bMultiPageFile,uUserBufSize,bUsePrefferedBuffer,pfnCallBack,pUserData):WRPERR_LTTWN_DLL_NOT_LOADED)

#if defined(LEADTOOLS_V16_OR_LATER)

#define L_WRPTWAINGETCUSTOMDSDATA(hSession, pCustomData, pszFileName) \
      ((pL_TwainGetCustomDSData) ? pL_TwainGetCustomDSData(hSession, pCustomData, pszFileName) : WRPERR_LTTWN_DLL_NOT_LOADED)

#define L_WRPTWAINSETCUSTOMDSDATA(hSession, pCustomData, pszFileName) \
      ((pL_TwainSetCustomDSData) ? pL_TwainSetCustomDSData(hSession, pCustomData, pszFileName) : WRPERR_LTTWN_DLL_NOT_LOADED)

#endif // #if defined(LEADTOOLS_V16_OR_LATER)

//-----------------------------------------------------------------------------
//--LTANN.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPANNBRINGTOFRONT(hObject) \
      ((pL_AnnBringToFront )?pL_AnnBringToFront(hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCLIPBOARDREADY(pfReady) \
      ((pL_AnnClipboardReady )?pL_AnnClipboardReady(pfReady):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCOPY(hSource, phDest) \
      ((pL_AnnCopy )?pL_AnnCopy(hSource, phDest):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCOPYFROMCLIPBOARD(hWnd, phContainer) \
      ((pL_AnnCopyFromClipboard )?pL_AnnCopyFromClipboard(hWnd, phContainer):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCOPYTOCLIPBOARD(hObject, uFormat, fSelected, fEmpty, fCheckMenu) \
      ((pL_AnnCopyToClipboard )?pL_AnnCopyToClipboard(hObject, uFormat, fSelected, fEmpty, fCheckMenu):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCUTTOCLIPBOARD(hObject, uFormat, fSelected, fEmpty, fCheckMenu) \
      ((pL_AnnCutToClipboard )?pL_AnnCutToClipboard(hObject, uFormat, fSelected, fEmpty, fCheckMenu):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCREATE(uObjectType, phObject) \
      ((pL_AnnCreate )?pL_AnnCreate(uObjectType, phObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCREATECONTAINER(hWnd, pRect, fVisible, phObject) \
      ((pL_AnnCreateContainer )?pL_AnnCreateContainer(hWnd, pRect, fVisible, phObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCREATEITEM(hContainer, uObjectType, fVisible, phObject) \
      ((pL_AnnCreateItem )?pL_AnnCreateItem(hContainer, uObjectType, fVisible, phObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCREATETOOLBAR(hwndParent, pPoint, uAlign, fVisible, phWnd, uButtons, pButtons) \
      ((pL_AnnCreateToolBar )?pL_AnnCreateToolBar(hwndParent, pPoint, uAlign, fVisible, phWnd, uButtons, pButtons):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDEFINE(hObject, pPoint, uState) \
      ((pL_AnnDefine )?pL_AnnDefine(hObject, pPoint, uState):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDELETEPAGEOFFSET(fd, nOffset, nPage) \
      ((pL_AnnDeletePageOffset )?pL_AnnDeletePageOffset(fd, nOffset, nPage):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDELETEPAGE(pFile, nPage) \
      ((pL_AnnDeletePage )?pL_AnnDeletePage(pFile, nPage):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDELETEPAGEMEMORY(hMem, puMemSize, nPage) \
      ((pL_AnnDeletePageMemory )?pL_AnnDeletePageMemory(hMem, puMemSize, nPage):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDESTROY(hObject, uFlags) \
      ((pL_AnnDestroy )?pL_AnnDestroy(hObject, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDRAW(hDC, prcInvalid, hObject) \
      ((pL_AnnDraw )?pL_AnnDraw(hDC, prcInvalid, hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNENUMERATE(hObject, pfnCallback, pUserData, uFlags, pUserList) \
      ((pL_AnnEnumerate )?pL_AnnEnumerate(hObject, pfnCallback, pUserData, uFlags, pUserList):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNFILEINFO(pszFile, pAnnFileInfo, uStructSize) \
      ((pL_AnnFileInfo )?pL_AnnFileInfo(pszFile, pAnnFileInfo, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNFILEINFOOFFSET(fd, pAnnFileInfo, uStructSize) \
      ((pL_AnnFileInfoOffset )?pL_AnnFileInfoOffset(fd, pAnnFileInfo, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNFILEINFOMEMORY(pMem, uMemSize, pAnnFileInfo, uStructSize) \
      ((pL_AnnFileInfoMemory )?pL_AnnFileInfoMemory(pMem, uMemSize, pAnnFileInfo, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNFLIP(hObject, pCenter, uFlags) \
      ((pL_AnnFlip )?pL_AnnFlip(hObject, pCenter, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETACTIVESTATE(hObject, puState) \
      ((pL_AnnGetActiveState )?pL_AnnGetActiveState(hObject, puState):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOCONTAINER(hObject, phContainer) \
      ((pL_AnnGetAutoContainer )?pL_AnnGetAutoContainer(hObject, phContainer):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTODRAWENABLE(hObject, pfEnable) \
      ((pL_AnnGetAutoDrawEnable )?pL_AnnGetAutoDrawEnable(hObject, pfEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOMENUENABLE(hObject, pfEnable) \
      ((pL_AnnGetAutoMenuEnable )?pL_AnnGetAutoMenuEnable(hObject, pfEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOTEXT(hObject, uItem, pText, puLen) \
      ((pL_AnnGetAutoText )?pL_AnnGetAutoText(hObject, uItem, pText, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOTEXTLEN(hObject, uItem, puLen) \
      ((pL_AnnGetAutoTextLen )?pL_AnnGetAutoTextLen(hObject, uItem, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETBACKCOLOR(hObject, pcrBack) \
      ((pL_AnnGetBackColor )?pL_AnnGetBackColor(hObject, pcrBack):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETBITMAP(hObject, pBitmap, uStructSize) \
      ((pL_AnnGetBitmap )?pL_AnnGetBitmap(hObject, pBitmap, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETBITMAPDPIX(hObject, pdDpiX) \
      ((pL_AnnGetBitmapDpiX )?pL_AnnGetBitmapDpiX(hObject, pdDpiX):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETBITMAPDPIY(hObject, pdDpiY) \
      ((pL_AnnGetBitmapDpiY )?pL_AnnGetBitmapDpiY(hObject, pdDpiY):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETBOUNDINGRECT(hObject, pRect, pRectName) \
      ((pL_AnnGetBoundingRect )?pL_AnnGetBoundingRect(hObject, pRect, pRectName):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETCONTAINER(hObject, phContainer) \
      ((pL_AnnGetContainer )?pL_AnnGetContainer(hObject, phContainer):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETDISTANCE(hObject, pdDistance, pdDistance2) \
      ((pL_AnnGetDistance )?pL_AnnGetDistance(hObject, pdDistance, pdDistance2):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETDPIX(hObject, pdDpiX) \
      ((pL_AnnGetDpiX )?pL_AnnGetDpiX(hObject, pdDpiX):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETDPIY(hObject, pdDpiY) \
      ((pL_AnnGetDpiY )?pL_AnnGetDpiY(hObject, pdDpiY):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFILLPATTERN(hObject, puFillPattern) \
      ((pL_AnnGetFillPattern )?pL_AnnGetFillPattern(hObject, puFillPattern):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFONTBOLD(hObject, pfFontBold) \
      ((pL_AnnGetFontBold )?pL_AnnGetFontBold(hObject, pfFontBold):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFONTITALIC(hObject, pfFontItalic) \
      ((pL_AnnGetFontItalic )?pL_AnnGetFontItalic(hObject, pfFontItalic):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFONTNAME(hObject, pFontName, puLen) \
      ((pL_AnnGetFontName )?pL_AnnGetFontName(hObject, pFontName, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFONTNAMELEN(hObject, puLen) \
      ((pL_AnnGetFontNameLen )?pL_AnnGetFontNameLen(hObject, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFONTSIZE(hObject, pdFontSize) \
      ((pL_AnnGetFontSize )?pL_AnnGetFontSize(hObject, pdFontSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFONTSTRIKETHROUGH(hObject, pfFontStrikeThrough) \
      ((pL_AnnGetFontStrikeThrough )?pL_AnnGetFontStrikeThrough(hObject, pfFontStrikeThrough):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFONTUNDERLINE(hObject, pfFontUnderline) \
      ((pL_AnnGetFontUnderline )?pL_AnnGetFontUnderline(hObject, pfFontUnderline):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFORECOLOR(hObject, pcrFore) \
      ((pL_AnnGetForeColor )?pL_AnnGetForeColor(hObject, pcrFore):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETGAUGELENGTH(hObject, pdLength) \
      ((pL_AnnGetGaugeLength )?pL_AnnGetGaugeLength(hObject, pdLength):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTICMARKLENGTH(hObject, pdLength) \
      ((pL_AnnGetTicMarkLength )?pL_AnnGetTicMarkLength(hObject, pdLength):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETHYPERLINK(hObject, puType, puMsg, pwParam, pLink, puLen) \
      ((pL_AnnGetHyperlink )?pL_AnnGetHyperlink(hObject, puType, puMsg, pwParam, pLink, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETHYPERLINKLEN(hObject, puLen) \
      ((pL_AnnGetHyperlinkLen )?pL_AnnGetHyperlinkLen(hObject, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETHYPERLINKMENUENABLE(hObject, pfEnable) \
      ((pL_AnnGetHyperlinkMenuEnable )?pL_AnnGetHyperlinkMenuEnable(hObject, pfEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETLINESTYLE(hObject, puLineStyle) \
      ((pL_AnnGetLineStyle )?pL_AnnGetLineStyle(hObject, puLineStyle):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETLINEWIDTH(hObject, pdLineWidth) \
      ((pL_AnnGetLineWidth )?pL_AnnGetLineWidth(hObject, pdLineWidth):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETLOCKED(hObject, pfLocked) \
      ((pL_AnnGetLocked )?pL_AnnGetLocked(hObject, pfLocked):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETOFFSETX(hObject, pdOffsetX) \
      ((pL_AnnGetOffsetX )?pL_AnnGetOffsetX(hObject, pdOffsetX):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETOFFSETY(hObject, pdOffsetY) \
      ((pL_AnnGetOffsetY )?pL_AnnGetOffsetY(hObject, pdOffsetY):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETPOINTCOUNT(hObject, puCount) \
      ((pL_AnnGetPointCount )?pL_AnnGetPointCount(hObject, puCount):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETPOINTS(hObject, pPoints) \
      ((pL_AnnGetPoints )?pL_AnnGetPoints(hObject, pPoints):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETPOLYFILLMODE(hObject, puPolyFillMode) \
      ((pL_AnnGetPolyFillMode )?pL_AnnGetPolyFillMode(hObject, puPolyFillMode):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETRECT(hObject, pRect, pRectName) \
      ((pL_AnnGetRect )?pL_AnnGetRect(hObject, pRect, pRectName):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETROP2(hObject, puRop2) \
      ((pL_AnnGetROP2 )?pL_AnnGetROP2(hObject, puRop2):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSCALARX(hObject, pdScalarX) \
      ((pL_AnnGetScalarX )?pL_AnnGetScalarX(hObject, pdScalarX):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSCALARY(hObject, pdScalarY) \
      ((pL_AnnGetScalarY )?pL_AnnGetScalarY(hObject, pdScalarY):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSELECTCOUNT(hObject, puCount) \
      ((pL_AnnGetSelectCount )?pL_AnnGetSelectCount(hObject, puCount):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSELECTED(hObject, pfSelected) \
      ((pL_AnnGetSelected )?pL_AnnGetSelected(hObject, pfSelected):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSELECTITEMS(hObject, pItems) \
      ((pL_AnnGetSelectItems )?pL_AnnGetSelectItems(hObject, pItems):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSELECTRECT(hObject, pRect) \
      ((pL_AnnGetSelectRect )?pL_AnnGetSelectRect(hObject, pRect):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTAG(hObject, puTag) \
      ((pL_AnnGetTag )?pL_AnnGetTag(hObject, puTag):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXT(hObject, pText, puLen) \
      ((pL_AnnGetText )?pL_AnnGetText(hObject, pText, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXTLEN(hObject, puLen) \
      ((pL_AnnGetTextLen )?pL_AnnGetTextLen(hObject, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXTALIGN(hObject, puTextAlign) \
      ((pL_AnnGetTextAlign )?pL_AnnGetTextAlign(hObject, puTextAlign):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXTROTATE(hObject, puTextRotate) \
      ((pL_AnnGetTextRotate )?pL_AnnGetTextRotate(hObject, puTextRotate):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXTPOINTERFIXED(hObject, pbPointerFixed) \
      ((pL_AnnGetTextPointerFixed )?pL_AnnGetTextPointerFixed(hObject, pbPointerFixed):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTEXTEXPANDTOKENS(hObject, bTextExpandTokens, uFlags) \
      ((pL_AnnSetTextExpandTokens )?pL_AnnSetTextExpandTokens(hObject, bTextExpandTokens, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXTEXPANDTOKENS(hObject, pbTextExpandTokens) \
      ((pL_AnnGetTextExpandTokens )?pL_AnnGetTextExpandTokens(hObject, pbTextExpandTokens):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTOOL(hObject, puTool) \
      ((pL_AnnGetTool )?pL_AnnGetTool(hObject, puTool):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTOOLBARBUTTONVISIBLE(hwndToolBar, uButton, pfVisible) \
      ((pL_AnnGetToolBarButtonVisible )?pL_AnnGetToolBarButtonVisible(hwndToolBar, uButton, pfVisible):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTOOLBARCHECKED(hwndToolBar, puChecked) \
      ((pL_AnnGetToolBarChecked )?pL_AnnGetToolBarChecked(hwndToolBar, puChecked):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTRANSPARENT(hObject, pbTransparent) \
      ((pL_AnnGetTransparent )?pL_AnnGetTransparent(hObject, pbTransparent):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTYPE(hObject, puObjectType) \
      ((pL_AnnGetType )?pL_AnnGetType(hObject, puObjectType):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTOPCONTAINER(hObject, phContainer) \
      ((pL_AnnGetTopContainer )?pL_AnnGetTopContainer(hObject, phContainer):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETUNIT(hObject, puUnit, pUnitAbbrev, puUnivAbbrevLen, puRulerPrecision) \
      ((pL_AnnGetUnit )?pL_AnnGetUnit(hObject, puUnit, pUnitAbbrev, puUnivAbbrevLen, puRulerPrecision):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETUNITLEN(hObject, puLen) \
      ((pL_AnnGetUnitLen )?pL_AnnGetUnitLen(hObject, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETUSERMODE(hObject, puMode) \
      ((pL_AnnGetUserMode )?pL_AnnGetUserMode(hObject, puMode):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETVISIBLE(hObject, pfVisible) \
      ((pL_AnnGetVisible )?pL_AnnGetVisible(hObject, pfVisible):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETWND(hObject, phWnd) \
      ((pL_AnnGetWnd )?pL_AnnGetWnd(hObject, phWnd):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNINSERT(hContainer, hObject, fStripContainer) \
      ((pL_AnnInsert )?pL_AnnInsert(hContainer, hObject, fStripContainer):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETITEM(hContainer, phItem) \
      ((pL_AnnGetItem )?pL_AnnGetItem(hContainer, phItem):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNLOAD(pFile, phObject, pLoadOptions) \
      ((pL_AnnLoad )?pL_AnnLoad(pFile, phObject, pLoadOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNLOADOFFSET(fd, nOffset, nLength, phObject, pLoadOptions) \
      ((pL_AnnLoadOffset )?pL_AnnLoadOffset(fd, nOffset, nLength, phObject, pLoadOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNLOADMEMORY(pMem, uMemSize, phObject, pLoadOptions) \
      ((pL_AnnLoadMemory )?pL_AnnLoadMemory(pMem, uMemSize, phObject, pLoadOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNLOCK(hObject,  pLockKey, uFlags) \
      ((pL_AnnLock )?pL_AnnLock(hObject,  pLockKey, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNMOVE(hObject, dDx, dDy, uFlags) \
      ((pL_AnnMove )?pL_AnnMove(hObject, dDx, dDy, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNPRINT(hDC, prcBounds, hObject) \
      ((pL_AnnPrint )?pL_AnnPrint(hDC, prcBounds, hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNREALIZE(pBitmap, prcBounds, hObject, fRedactOnly) \
      ((pL_AnnRealize )?pL_AnnRealize(pBitmap, prcBounds, hObject, fRedactOnly):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNRESIZE(hObject, dFactorX, dFactorY, pCenter, uFlags) \
      ((pL_AnnResize )?pL_AnnResize(hObject, dFactorX, dFactorY, pCenter, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNREVERSE(hObject, pCenter, uFlags) \
      ((pL_AnnReverse )?pL_AnnReverse(hObject, pCenter, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNREMOVE(hObject) \
      ((pL_AnnRemove )?pL_AnnRemove(hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNROTATE(hObject, dAngle, pCenter, uFlags) \
      ((pL_AnnRotate )?pL_AnnRotate(hObject, dAngle, pCenter, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSAVE(pFile, hObject, uFormat, fSelected, pSaveOptions) \
      ((pL_AnnSave )?pL_AnnSave(pFile, hObject, uFormat, fSelected, pSaveOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSAVEOFFSET(fd, nOffset, puSizeWritten, hObject, uFormat, fSelected, pSaveOptions) \
      ((pL_AnnSaveOffset )?pL_AnnSaveOffset(fd, nOffset, puSizeWritten, hObject, uFormat, fSelected, pSaveOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSAVEMEMORY(hObject, uFormat, fSelected, phMem, puMemSize, pSaveOptions) \
      ((pL_AnnSaveMemory )?pL_AnnSaveMemory(hObject, uFormat, fSelected, phMem, puMemSize, pSaveOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSAVETAG(hObject, uFormat, fSelected) \
      ((pL_AnnSaveTag )?pL_AnnSaveTag(hObject, uFormat, fSelected):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSELECTPOINT(hObject, pPoint) \
      ((pL_AnnSelectPoint )?pL_AnnSelectPoint(hObject, pPoint):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSELECTRECT(hObject, pRect) \
      ((pL_AnnSelectRect )?pL_AnnSelectRect(hObject, pRect):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSENDTOBACK(hObject) \
      ((pL_AnnSendToBack )?pL_AnnSendToBack(hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETACTIVESTATE(hObject, uState) \
      ((pL_AnnSetActiveState )?pL_AnnSetActiveState(hObject, uState):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOCONTAINER(hObject, hContainer) \
      ((pL_AnnSetAutoContainer )?pL_AnnSetAutoContainer(hObject, hContainer):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTODRAWENABLE(hObject, fEnable) \
      ((pL_AnnSetAutoDrawEnable )?pL_AnnSetAutoDrawEnable(hObject, fEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOMENUENABLE(hObject, fEnable) \
      ((pL_AnnSetAutoMenuEnable )?pL_AnnSetAutoMenuEnable(hObject, fEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOTEXT(hObject, uItem, pText) \
      ((pL_AnnSetAutoText )?pL_AnnSetAutoText(hObject, uItem, pText):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETBACKCOLOR(hObject, crBack, uFlags) \
      ((pL_AnnSetBackColor )?pL_AnnSetBackColor(hObject, crBack, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETBITMAP(hObject, pBitmap, uFlags) \
      ((pL_AnnSetBitmap )?pL_AnnSetBitmap(hObject, pBitmap, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETBITMAPDPIX(hObject, dDpiX, uFlags) \
      ((pL_AnnSetBitmapDpiX )?pL_AnnSetBitmapDpiX(hObject, dDpiX, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETBITMAPDPIY(hObject, dDpiY, uFlags) \
      ((pL_AnnSetBitmapDpiY )?pL_AnnSetBitmapDpiY(hObject, dDpiY, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETDPIX(hObject, dDpiX, uFlags) \
      ((pL_AnnSetDpiX )?pL_AnnSetDpiX(hObject, dDpiX, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETDPIY(hObject, dDpiY, uFlags) \
      ((pL_AnnSetDpiY )?pL_AnnSetDpiY(hObject, dDpiY, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFILLPATTERN(hObject, uFillPattern, uFlags) \
      ((pL_AnnSetFillPattern )?pL_AnnSetFillPattern(hObject, uFillPattern, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFONTBOLD(hObject, fFontBold, uFlags) \
      ((pL_AnnSetFontBold )?pL_AnnSetFontBold(hObject, fFontBold, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFONTITALIC(hObject, fFontItalic, uFlags) \
      ((pL_AnnSetFontItalic )?pL_AnnSetFontItalic(hObject, fFontItalic, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFONTNAME(hObject, pFontName, uFlags) \
      ((pL_AnnSetFontName )?pL_AnnSetFontName(hObject, pFontName, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFONTSIZE(hObject, dFontSize, uFlags) \
      ((pL_AnnSetFontSize )?pL_AnnSetFontSize(hObject, dFontSize, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFONTSTRIKETHROUGH(hObject, fFontStrikeThrough, uFlags) \
      ((pL_AnnSetFontStrikeThrough )?pL_AnnSetFontStrikeThrough(hObject, fFontStrikeThrough, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFONTUNDERLINE(hObject, fFontUnderline, uFlags) \
      ((pL_AnnSetFontUnderline )?pL_AnnSetFontUnderline(hObject, fFontUnderline, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFORECOLOR(hObject, crFore, uFlags) \
      ((pL_AnnSetForeColor )?pL_AnnSetForeColor(hObject, crFore, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETGAUGELENGTH(hObject, dLength, uFlags) \
      ((pL_AnnSetGaugeLength )?pL_AnnSetGaugeLength(hObject, dLength, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTICMARKLENGTH(hObject, dLength, uFlags) \
      ((pL_AnnSetTicMarkLength )?pL_AnnSetTicMarkLength(hObject, dLength, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETHYPERLINK(hObject, uType, uMsg, wParam, pLink, uFlags) \
      ((pL_AnnSetHyperlink )?pL_AnnSetHyperlink(hObject, uType, uMsg, wParam, pLink, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETHYPERLINKMENUENABLE(hObject, fEnable, uFlags) \
      ((pL_AnnSetHyperlinkMenuEnable )?pL_AnnSetHyperlinkMenuEnable(hObject, fEnable, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETLINESTYLE(hObject, uLineStyle, uFlags) \
      ((pL_AnnSetLineStyle )?pL_AnnSetLineStyle(hObject, uLineStyle, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETLINEWIDTH(hObject, dLineWidth, uFlags) \
      ((pL_AnnSetLineWidth )?pL_AnnSetLineWidth(hObject, dLineWidth, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETOFFSETX(hObject, dOffsetX, uFlags) \
      ((pL_AnnSetOffsetX )?pL_AnnSetOffsetX(hObject, dOffsetX, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETOFFSETY(hObject, dOffsetY, uFlags) \
      ((pL_AnnSetOffsetY )?pL_AnnSetOffsetY(hObject, dOffsetY, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETPOINTS(hObject, pPoints, uCount) \
      ((pL_AnnSetPoints )?pL_AnnSetPoints(hObject, pPoints, uCount):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETPOLYFILLMODE(hObject, uPolyFillMode, uFlags) \
      ((pL_AnnSetPolyFillMode )?pL_AnnSetPolyFillMode(hObject, uPolyFillMode, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETROP2(hObject, uROP2, uFlags) \
      ((pL_AnnSetROP2 )?pL_AnnSetROP2(hObject, uROP2, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETRECT(hObject, pRect) \
      ((pL_AnnSetRect )?pL_AnnSetRect(hObject, pRect):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETSELECTED(hObject, fSelected, uFlags) \
      ((pL_AnnSetSelected )?pL_AnnSetSelected(hObject, fSelected, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETSCALARX(hObject, dScalarX, uFlags) \
      ((pL_AnnSetScalarX )?pL_AnnSetScalarX(hObject, dScalarX, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETSCALARY(hObject, dScalarY, uFlags) \
      ((pL_AnnSetScalarY )?pL_AnnSetScalarY(hObject, dScalarY, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTAG(hObject, uTag, uFlags) \
      ((pL_AnnSetTag )?pL_AnnSetTag(hObject, uTag, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTEXT(hObject, pText, uFlags) \
      ((pL_AnnSetText )?pL_AnnSetText(hObject, pText, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTEXTALIGN(hObject, uTextAlign, uFlags) \
      ((pL_AnnSetTextAlign )?pL_AnnSetTextAlign(hObject, uTextAlign, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTEXTROTATE(hObject, uTextRotate, uFlags) \
      ((pL_AnnSetTextRotate )?pL_AnnSetTextRotate(hObject, uTextRotate, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTEXTPOINTERFIXED(hObject, bPointerFixed, uFlags) \
      ((pL_AnnSetTextPointerFixed )?pL_AnnSetTextPointerFixed(hObject, bPointerFixed, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTOOL(hObject, uTool) \
      ((pL_AnnSetTool )?pL_AnnSetTool(hObject, uTool):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTOOLBARBUTTONVISIBLE(hwndToolBar, uButton, fVisible) \
      ((pL_AnnSetToolBarButtonVisible )?pL_AnnSetToolBarButtonVisible(hwndToolBar, uButton, fVisible):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTOOLBARCHECKED(hwndToolBar, uChecked) \
      ((pL_AnnSetToolBarChecked )?pL_AnnSetToolBarChecked(hwndToolBar, uChecked):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTRANSPARENT(hObject, bTransparent, uFlags) \
      ((pL_AnnSetTransparent )?pL_AnnSetTransparent(hObject, bTransparent, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETUNDODEPTH(hObject, uLevels) \
      ((pL_AnnSetUndoDepth )?pL_AnnSetUndoDepth(hObject, uLevels):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETUNIT(hObject, uUnit,  pUnitAbbrev, uPrecision, uFlags) \
      ((pL_AnnSetUnit )?pL_AnnSetUnit(hObject, uUnit,  pUnitAbbrev, uPrecision, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETUSERMODE(hObject, uMode) \
      ((pL_AnnSetUserMode )?pL_AnnSetUserMode(hObject, uMode):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETVISIBLE(hObject, fVisible, uFlags, pUserList) \
      ((pL_AnnSetVisible )?pL_AnnSetVisible(hObject, fVisible, uFlags, pUserList):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETWND(hObject, hWnd) \
      ((pL_AnnSetWnd )?pL_AnnSetWnd(hObject, hWnd):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSHOWLOCKEDICON(hObject, bShow, uFlags) \
      ((pL_AnnShowLockedIcon )?pL_AnnShowLockedIcon(hObject, bShow, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNUNDO(hObject) \
      ((pL_AnnUndo )?pL_AnnUndo(hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNUNLOCK(hObject,  pUnlockKey, uFlags) \
      ((pL_AnnUnlock )?pL_AnnUnlock(hObject,  pUnlockKey, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNUNREALIZE(pBitmap, prcBounds, hObject, fSelected) \
      ((pL_AnnUnrealize )?pL_AnnUnrealize(pBitmap, prcBounds, hObject, fSelected):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETNODES(hObject, bShowNodes, uGapNodes, uFlags) \
      ((pL_AnnSetNodes )?pL_AnnSetNodes(hObject, bShowNodes, uGapNodes, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETNODES(hObject, pbShowNodes, puGapNodes) \
      ((pL_AnnGetNodes )?pL_AnnGetNodes(hObject, pbShowNodes, puGapNodes):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETPROTRACTOROPTIONS(hObject, bAcute, uUnit, pszAbbrev, uPrecision, dArcRadius, uFlags) \
      ((pL_AnnSetProtractorOptions )?pL_AnnSetProtractorOptions(hObject, bAcute, uUnit, pszAbbrev, uPrecision, dArcRadius, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETPROTRACTOROPTIONS(hObject, pbAcute, puUnit, puAbbrevLen, pszAbbrev, puPrecision, pdArcRadius) \
      ((pL_AnnGetProtractorOptions )?pL_AnnGetProtractorOptions(hObject, pbAcute, puUnit, puAbbrevLen, pszAbbrev, puPrecision, pdArcRadius):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETNAMEOPTIONS(hObject, pNameOptions, uStructSize) \
      ((pL_AnnGetNameOptions )?pL_AnnGetNameOptions(hObject, pNameOptions, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETNAMEOPTIONS(hObject, pNameOptions, uFlags) \
      ((pL_AnnSetNameOptions )?pL_AnnSetNameOptions(hObject, pNameOptions, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETSHOWFLAGS(hObject, uShowFlags, uFlags) \
      ((pL_AnnSetShowFlags )?pL_AnnSetShowFlags(hObject, uShowFlags, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSHOWFLAGS(hObject, puShowFlags) \
      ((pL_AnnGetShowFlags )?pL_AnnGetShowFlags(hObject, puShowFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETANGLE(hObject, pdAngle) \
      ((pL_AnnGetAngle )?pL_AnnGetAngle(hObject, pdAngle):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETMETAFILE(hObject, hMetafile, uType, uFlags) \
      ((pL_AnnSetMetafile )?pL_AnnSetMetafile(hObject, hMetafile, uType, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETMETAFILE(hObject, phMetafile) \
      ((pL_AnnGetMetafile )?pL_AnnGetMetafile(hObject, phMetafile):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSECONDARYMETAFILE(hObject, phMetafile) \
      ((pL_AnnGetSecondaryMetafile )?pL_AnnGetSecondaryMetafile(hObject, phMetafile):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETPREDEFINEDMETAFILE(uType, hMetafile) \
      ((pL_AnnSetPredefinedMetafile )?pL_AnnSetPredefinedMetafile(uType, hMetafile):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETPREDEFINEDMETAFILE(uType, phMetafile, pbEnhanced) \
      ((pL_AnnGetPredefinedMetafile )?pL_AnnGetPredefinedMetafile(uType, phMetafile, pbEnhanced):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETSECONDARYBITMAP(hObject, pBitmap, uFlags) \
      ((pL_AnnSetSecondaryBitmap )?pL_AnnSetSecondaryBitmap(hObject, pBitmap, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSECONDARYBITMAP(hObject, pBitmap, uStructSize) \
      ((pL_AnnGetSecondaryBitmap )?pL_AnnGetSecondaryBitmap(hObject, pBitmap, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOMENUITEMENABLE(hObject, nObjectType, uItem, uEnable, uFlags, pUserList) \
      ((pL_AnnSetAutoMenuItemEnable )?pL_AnnSetAutoMenuItemEnable(hObject, nObjectType, uItem, uEnable, uFlags, pUserList):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOMENUITEMENABLE(hObject, nObjectType, uItem, puEnable) \
      ((pL_AnnGetAutoMenuItemEnable )?pL_AnnGetAutoMenuItemEnable(hObject, nObjectType, uItem, puEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOMENUSTATE(hObject, nObjectType, pEnable, pEnableFlags, uBits, uFlags) \
      ((pL_AnnSetAutoMenuState )?pL_AnnSetAutoMenuState(hObject, nObjectType, pEnable, pEnableFlags, uBits, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOMENUSTATE(hObject, nObjectType, pEnable, pEnableFlags, uBits) \
      ((pL_AnnGetAutoMenuState )?pL_AnnGetAutoMenuState(hObject, nObjectType, pEnable, pEnableFlags, uBits):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETUSER(hObject, pOldUser, pNewUser, uFlags) \
      ((pL_AnnSetUser )?pL_AnnSetUser(hObject, pOldUser, pNewUser, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTOOLBARBUTTONS(hwndToolBar, pButtons, uButtons) \
      ((pL_AnnSetToolBarButtons )?pL_AnnSetToolBarButtons(hwndToolBar, pButtons, uButtons):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTOOLBARBUTTONS(hwndToolBar, pButtons, uStructSize, puButtons) \
      ((pL_AnnGetToolBarButtons )?pL_AnnGetToolBarButtons(hwndToolBar, pButtons, uStructSize, puButtons):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNFREETOOLBARBUTTONS(pButtons, uButtons) \
      ((pL_AnnFreeToolBarButtons )?pL_AnnFreeToolBarButtons(pButtons, uButtons):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTOOLBARINFO(hwndToolBar, pInfo, uStructSize) \
      ((pL_AnnGetToolBarInfo )?pL_AnnGetToolBarInfo(hwndToolBar, pInfo, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTOOLBARCOLUMNS(hwndToolBar, uColumns) \
      ((pL_AnnSetToolBarColumns )?pL_AnnSetToolBarColumns(hwndToolBar, uColumns):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTOOLBARROWS(hwndToolBar, uRows) \
      ((pL_AnnSetToolBarRows )?pL_AnnSetToolBarRows(hwndToolBar, uRows):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTODEFAULTS(hAutomation, hObject, uFlags) \
      ((pL_AnnSetAutoDefaults )?pL_AnnSetAutoDefaults(hAutomation, hObject, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTRANSPARENTCOLOR(hObject, crTransparent, uFlags) \
      ((pL_AnnSetTransparentColor )?pL_AnnSetTransparentColor(hObject, crTransparent, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTRANSPARENTCOLOR(hObject, pcrTransparent) \
      ((pL_AnnGetTransparentColor )?pL_AnnGetTransparentColor(hObject, pcrTransparent):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETUNDODEPTH(hObject, puUsedLevels, puMaxLevels) \
      ((pL_AnnGetUndoDepth )?pL_AnnGetUndoDepth(hObject, puUsedLevels, puMaxLevels):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGROUP(hObject, uFlags, pUserList) \
      ((pL_AnnGroup )?pL_AnnGroup(hObject, uFlags, pUserList):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNUNGROUP(hObject, uFlags, pUserList) \
      ((pL_AnnUngroup )?pL_AnnUngroup(hObject, uFlags, pUserList):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOOPTIONS(hObject, uFlags) \
      ((pL_AnnSetAutoOptions )?pL_AnnSetAutoOptions(hObject, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOOPTIONS(hObject, puFlags) \
      ((pL_AnnGetAutoOptions )?pL_AnnGetAutoOptions(hObject, puFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETOBJECTFROMTAG(hContainer, uFlags, uTag, phTagObject) \
      ((pL_AnnGetObjectFromTag )?pL_AnnGetObjectFromTag(hContainer, uFlags, uTag, phTagObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETRGNHANDLE(hObject, pXForm, phRgn) \
      ((pL_AnnGetRgnHandle )?pL_AnnGetRgnHandle(hObject, pXForm, phRgn):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAREA(hObject, puCount) \
      ((pL_AnnGetArea )?pL_AnnGetArea(hObject, puCount):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTODIALOGFONTSIZE(hObject, nFontSize) \
      ((pL_AnnSetAutoDialogFontSize )?pL_AnnSetAutoDialogFontSize(hObject, nFontSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTODIALOGFONTSIZE(hObject,  pnFontSize) \
      ((pL_AnnGetAutoDialogFontSize )?pL_AnnGetAutoDialogFontSize(hObject,  pnFontSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETGROUPING(hObject, bAutoGroup, uFlags) \
      ((pL_AnnSetGrouping )?pL_AnnSetGrouping(hObject, bAutoGroup, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETGROUPING(hObject,  pbAutoGroup) \
      ((pL_AnnGetGrouping )?pL_AnnGetGrouping(hObject,  pbAutoGroup):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOBACKCOLOR(hObject, uObjectType, crBack) \
      ((pL_AnnSetAutoBackColor )?pL_AnnSetAutoBackColor(hObject, uObjectType, crBack):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOBACKCOLOR(hObject, uObjectType, pcrBack) \
      ((pL_AnnGetAutoBackColor )?pL_AnnGetAutoBackColor(hObject, uObjectType, pcrBack):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNADDUNDONODE(hObject) \
      ((pL_AnnAddUndoNode )?pL_AnnAddUndoNode(hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOUNDOENABLE(hObject, bEnable) \
      ((pL_AnnSetAutoUndoEnable )?pL_AnnSetAutoUndoEnable(hObject, bEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOUNDOENABLE(hObject, pbEnable) \
      ((pL_AnnGetAutoUndoEnable )?pL_AnnGetAutoUndoEnable(hObject, pbEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTOOLBARPARENT(hwndToolBar, hwndParent) \
      ((pL_AnnSetToolBarParent )?pL_AnnSetToolBarParent(hwndToolBar, hwndParent):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETENCRYPTOPTIONS(hObject, pEncryptOptions, uFlags) \
      ((pL_AnnSetEncryptOptions )?pL_AnnSetEncryptOptions(hObject, pEncryptOptions, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETENCRYPTOPTIONS(hObject, pEncryptOptions) \
      ((pL_AnnGetEncryptOptions )?pL_AnnGetEncryptOptions(hObject, pEncryptOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNENCRYPTAPPLY(hObject, uEncryptFlags, uFlags) \
      ((pL_AnnEncryptApply )?pL_AnnEncryptApply(hObject, uEncryptFlags, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETPREDEFINEDBITMAP(uType, pBitmap) \
      ((pL_AnnSetPredefinedBitmap )?pL_AnnSetPredefinedBitmap(uType, pBitmap):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETPREDEFINEDBITMAP(uType, pBitmap, uStructSize) \
      ((pL_AnnGetPredefinedBitmap )?pL_AnnGetPredefinedBitmap(uType, pBitmap, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETPOINTOPTIONS(hObject, pPointOptions, uStructSize) \
      ((pL_AnnGetPointOptions )?pL_AnnGetPointOptions(hObject, pPointOptions, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETPOINTOPTIONS(hObject, pPointOptions, uFlags) \
      ((pL_AnnSetPointOptions )?pL_AnnSetPointOptions(hObject, pPointOptions, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

// Annotation #define HANDLE() function
#define L_WRPANNADDUSERHANDLE(hObject, pAnnHandle) \
      ((pL_AnnAddUserHandle )?pL_AnnAddUserHandle(hObject, pAnnHandle):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETUSERHANDLE(hObject, uIndex, pAnnHandle) \
      ((pL_AnnGetUserHandle )?pL_AnnGetUserHandle(hObject, uIndex, pAnnHandle):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETUSERHANDLES(hObject, pAnnHandle, pCount) \
      ((pL_AnnGetUserHandles )?pL_AnnGetUserHandles(hObject, pAnnHandle, pCount):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCHANGEUSERHANDLE(hObject, nIndex, pAnnHandle) \
      ((pL_AnnChangeUserHandle )?pL_AnnChangeUserHandle(hObject, nIndex, pAnnHandle):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDELETEUSERHANDLE(hObject, nIndex) \
      ((pL_AnnDeleteUserHandle )?pL_AnnDeleteUserHandle(hObject, nIndex):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNENUMERATEHANDLES(hObject, pfnCallback, pUserData) \
      ((pL_AnnEnumerateHandles )?pL_AnnEnumerateHandles(hObject, pfnCallback, pUserData):WRPERR_LTANN_DLL_NOT_LOADED)

// Misc functions
#define L_WRPANNDEBUG(hObject, pXForm) \
      ((pL_AnnDebug )?pL_AnnDebug(hObject, pXForm):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDUMPOBJECT(hObject) \
      ((pL_AnnDumpObject )?pL_AnnDumpObject(hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNHITTEST(hObject, pPoint, puResult, phObjectHit, pHitTestInfo, uStructSize) \
      ((pL_AnnHitTest)?pL_AnnHitTest(hObject, pPoint, puResult, phObjectHit, pHitTestInfo, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETROTATEANGLE(hObject, pdAngle) \
      ((pL_AnnGetRotateAngle )?pL_AnnGetRotateAngle(hObject, pdAngle):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNADJUSTPOINT(pptAnchor, pptMove, dAngle, nType) \
      ((pL_AnnAdjustPoint )?pL_AnnAdjustPoint(pptAnchor, pptMove, dAngle, nType):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCONVERT(hContainer, pPoints, pAnnPoints, nCount, nConvert) \
      ((pL_AnnConvert )?pL_AnnConvert(hContainer, pPoints, pAnnPoints, nCount, nConvert):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNRESTRICTCURSOR(hContainer, lpRect, pPoint, prcOldClip, bRestrictClient) \
      ((pL_AnnRestrictCursor )?pL_AnnRestrictCursor(hContainer, lpRect, pPoint, prcOldClip, bRestrictClient):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETRESTRICTTOCONTAINER(hObject, bRestrict, uFlags) \
      ((pL_AnnSetRestrictToContainer )?pL_AnnSetRestrictToContainer(hObject, bRestrict, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETRESTRICTTOCONTAINER(hObject,  pbRestrict) \
      ((pL_AnnGetRestrictToContainer )?pL_AnnGetRestrictToContainer(hObject,  pbRestrict):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDEFINE2(hObject, apt, uState) \
      ((pL_AnnDefine2 )?pL_AnnDefine2(hObject, apt, uState):WRPERR_LTANN_DLL_NOT_LOADED)

// Text Token Table Functions
#define L_WRPANNINSERTTEXTTOKENTABLE(hAutomation, pTextToken) \
      ((pL_AnnInsertTextTokenTable )?pL_AnnInsertTextTokenTable(hAutomation, pTextToken):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNENUMERATETEXTTOKENTABLE(hAutomation, pfnCallback, pUserData) \
      ((pL_AnnEnumerateTextTokenTable )?pL_AnnEnumerateTextTokenTable(hAutomation, pfnCallback, pUserData):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDELETETEXTTOKENTABLE(hAutomation, cToken) \
      ((pL_AnnDeleteTextTokenTable )?pL_AnnDeleteTextTokenTable(hAutomation, cToken):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCLEARTEXTTOKENTABLE(hAutomation) \
      ((pL_AnnClearTextTokenTable )?pL_AnnClearTextTokenTable(hAutomation):WRPERR_LTANN_DLL_NOT_LOADED)

// Fixed Annotation Functions
#define L_WRPANNSETNOSCROLL(hObject, bNoScroll, uFlags, pUserList) \
      ((pL_AnnSetNoScroll )?pL_AnnSetNoScroll(hObject, bNoScroll, uFlags, pUserList):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETNOSCROLL(hObject, pbNoScroll) \
      ((pL_AnnGetNoScroll )?pL_AnnGetNoScroll(hObject, pbNoScroll):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETNOZOOM(hObject, bNoZoom, uFlags, pUserList) \
      ((pL_AnnSetNoZoom )?pL_AnnSetNoZoom(hObject, bNoZoom, uFlags, pUserList):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETNOZOOM(hObject, pbNoZoom) \
      ((pL_AnnGetNoZoom )?pL_AnnGetNoZoom(hObject, pbNoZoom):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNENABLEFIXED(hObject, bEnable, uFlags) \
      ((pL_AnnEnableFixed )?pL_AnnEnableFixed(hObject, bEnable, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFIXED(hObject, pbFixed) \
      ((pL_AnnGetFixed )?pL_AnnGetFixed(hObject, pbFixed):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFIXED(hObject, bFixed, bAdjust, uFlags) \
      ((pL_AnnSetFixed )?pL_AnnSetFixed(hObject, bFixed, bAdjust, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNPUSHFIXEDSTATE(hObject, uFlags) \
      ((pL_AnnPushFixedState )?pL_AnnPushFixedState(hObject, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNPOPFIXEDSTATE(hObject, uFlags) \
      ((pL_AnnPopFixedState )?pL_AnnPopFixedState(hObject, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNISFIXEDINRECT(hObject, prc, pbFixedInRect, uFlags) \
      ((pL_AnnIsFixedInRect )?pL_AnnIsFixedInRect(hObject, prc, pbFixedInRect, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETDISTANCE2(hObject, puCount, pDistance, pTotalDistance, uStructSize) \
      ((pL_AnnGetDistance2 )?pL_AnnGetDistance2(hObject, puCount, pDistance, pTotalDistance, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNDUMPSMARTDISTANCE(sdSmartDistance, pszSmartDistance, puLength) \
      ((pL_AnnDumpSmartDistance )?pL_AnnDumpSmartDistance(sdSmartDistance, pszSmartDistance, puLength):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOCURSOR(hAutomation, uItem, hCursor) \
      ((pL_AnnSetAutoCursor )?pL_AnnSetAutoCursor(hAutomation, uItem, hCursor):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOCURSOR(hAutomation, uItem, phCursor) \
      ((pL_AnnGetAutoCursor )?pL_AnnGetAutoCursor(hAutomation, uItem, phCursor):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETUSERDATA(hObject, pUserData, uUserDataSize, uFlags) \
      ((pL_AnnSetUserData )?pL_AnnSetUserData(hObject, pUserData, uUserDataSize, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETUSERDATA(hObject, pUserData, puUserDataSize) \
      ((pL_AnnGetUserData )?pL_AnnGetUserData(hObject, pUserData, puUserDataSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTEXTRTF(hObject, uFormat, pText, uFlags) \
      ((pL_AnnSetTextRTF )?pL_AnnSetTextRTF(hObject, uFormat, pText, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXTRTF(hObject, uFormat, pText, puLen) \
      ((pL_AnnGetTextRTF )?pL_AnnGetTextRTF(hObject, uFormat, pText, puLen):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPDEBUGENABLEWORLDTRANSFORM(bEnable) \
      ((pL_DebugEnableWorldTransform )?pL_DebugEnableWorldTransform(bEnable):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPDEBUGSETGLOBAL(nValue) \
      ((pL_DebugSetGlobal )?pL_DebugSetGlobal(nValue):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPDEBUGGETGLOBAL() \
      ((pL_DebugGetGlobal )?pL_DebugGetGlobal():WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETLOCALE(nCategory, lpszLocale) \
      ((pL_AnnSetlocale )?pL_AnnSetlocale(nCategory, lpszLocale):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOHILIGHTPEN(hAutomation, crHilight) \
      ((pL_AnnSetAutoHilightPen )?pL_AnnSetAutoHilightPen(hAutomation, crHilight):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETOPTIONS(hAutomation, uOptions) \
      ((pL_AnnSetOptions )?pL_AnnSetOptions(hAutomation, uOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETOPTIONS(puOptions) \
      ((pL_AnnGetOptions )?pL_AnnGetOptions(puOptions):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETFILLMODE(hObject, puFillMode, pnAlpha) \
      ((pL_AnnGetFillMode )?pL_AnnGetFillMode(hObject, puFillMode, pnAlpha):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETFILLMODE(hObject, uFillMode, nAlpha, uFlags) \
      ((pL_AnnSetFillMode )?pL_AnnSetFillMode(hObject, uFillMode, nAlpha, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETROTATEOPTIONS(hObject, pRotateOptions, uStructSize) \
      ((pL_AnnGetRotateOptions )?pL_AnnGetRotateOptions(hObject, pRotateOptions, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETROTATEOPTIONS(hObject, pRotateOptions, uFlags) \
      ((pL_AnnSetRotateOptions )?pL_AnnSetRotateOptions(hObject, pRotateOptions, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNCALIBRATERULER(hObject, dCalibrateLength, uCalibrateUnit, dDpiRatioXtoY) \
      ((pL_AnnCalibrateRuler )?pL_AnnCalibrateRuler(hObject, dCalibrateLength, uCalibrateUnit, dDpiRatioXtoY):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNTEXTEDIT(hObject) \
      ((pL_AnnTextEdit )?pL_AnnTextEdit(hObject):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTEXTOPTIONS(hObject, pTextOptions, uFlags) \
      ((pL_AnnSetTextOptions )?pL_AnnSetTextOptions(hObject, pTextOptions, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXTOPTIONS(hObject, pTextOptions, uStructSize) \
      ((pL_AnnGetTextOptions )?pL_AnnGetTextOptions(hObject, pTextOptions, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETAUTOSNAPCURSOR(hAutomation, pbSnap) \
      ((pL_AnnGetAutoSnapCursor )?pL_AnnGetAutoSnapCursor(hAutomation, pbSnap):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETAUTOSNAPCURSOR(hAutomation, bSnap) \
      ((pL_AnnSetAutoSnapCursor )?pL_AnnSetAutoSnapCursor(hAutomation, bSnap):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETTEXTFIXEDSIZE(hObject, bTextFixedSize, uFlags) \
      ((pL_AnnSetTextFixedSize )?pL_AnnSetTextFixedSize(hObject, bTextFixedSize, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETTEXTFIXEDSIZE(hObject, pbTextFixedSize) \
      ((pL_AnnGetTextFixedSize )?pL_AnnGetTextFixedSize(hObject, pbTextFixedSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETLINEFIXEDWIDTH(hObject, pbLineFixedWidth) \
      ((pL_AnnGetLineFixedWidth )?pL_AnnGetLineFixedWidth(hObject, pbLineFixedWidth):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETLINEFIXEDWIDTH(hObject, bLineFixedWidth, uFlags) \
      ((pL_AnnSetLineFixedWidth )?pL_AnnSetLineFixedWidth(hObject, bLineFixedWidth, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETPOINTEROPTIONS(hObject, pOptions, uStructSize) \
      ((pL_AnnGetPointerOptions )?pL_AnnGetPointerOptions(hObject, pOptions, uStructSize):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETPOINTEROPTIONS(hObject, pOptions, uFlags) \
      ((pL_AnnSetPointerOptions )?pL_AnnSetPointerOptions(hObject, pOptions, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETRENDERMODE(hObject, uRenderMode, uFlags) \
      ((pL_AnnSetRenderMode )?pL_AnnSetRenderMode(hObject, uRenderMode, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNGETSHOWSTAMPBORDER(hObject, pbShowStampBorder) \
      ((pL_AnnGetShowStampBorder )?pL_AnnGetShowStampBorder(hObject, pbShowStampBorder):WRPERR_LTANN_DLL_NOT_LOADED)

#define L_WRPANNSETSHOWSTAMPBORDER(hObject, bShowStampBorder, uFlags) \
      ((pL_AnnSetShowStampBorder )?pL_AnnSetShowStampBorder(hObject, bShowStampBorder, uFlags):WRPERR_LTANN_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTSCR.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPSETCAPTUREOPTION(pOptions)   \
   ((pL_SetCaptureOption)? pL_SetCaptureOption(pOptions):WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPGETCAPTUREOPTION(pOptions, uStructSize) \
   ((pL_GetCaptureOption) ? pL_GetCaptureOption(pOptions, uStructSize) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREWINDOW(pBitmap, uBitmapStructSize, hWnd, wctCaptureType,pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureWindow) ? pL_CaptureWindow(pBitmap, uBitmapStructSize, hWnd, wctCaptureType,pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREACTIVEWINDOW(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureActiveWindow) ? pL_CaptureActiveWindow(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREACTIVECLIENT(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureActiveClient) ? pL_CaptureActiveClient(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREWALLPAPER(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureWallPaper) ? pL_CaptureWallPaper(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREFULLSCREEN(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureFullScreen) ? pL_CaptureFullScreen(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREMENUUNDERCURSOR(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureMenuUnderCursor) ? pL_CaptureMenuUnderCursor(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREWINDOWUNDERCURSOR(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureWindowUnderCursor) ? pL_CaptureWindowUnderCursor(pBitmap, uBitmapStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTURESELECTEDOBJECT(pBitmap, uBitmapStructSize, pObjectOptions, uOptionsStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback,pUserData) \
   ((pL_CaptureSelectedObject) ? pL_CaptureSelectedObject(pBitmap, uBitmapStructSize, pObjectOptions, uOptionsStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback,pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREAREA(pBitmap, uBitmapStructSize, pCaptureAreaOption, uOptionsStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureArea) ? pL_CaptureArea(pBitmap, uBitmapStructSize, pCaptureAreaOption, uOptionsStructSize, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREMOUSECURSOR(pBitmap, uBitmapStructSize, crFill, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) \
   ((pL_CaptureMouseCursor) ? pL_CaptureMouseCursor(pBitmap, uBitmapStructSize, crFill, pCaptureInfo, uInfoStructSize, pfnCaptureCallback, pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTURESETHOTKEYCALLBACK(pfnCaptureHotKeyCB,pUserData)  \
   ((pL_CaptureSetHotKeyCallback)? pL_CaptureSetHotKeyCallback(pfnCaptureHotKeyCB,pUserData):WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPSETCAPTUREOPTIONDLG(hwndOwner,uFlags,pOptions,pfnCallBack,pUserData) \
   ((pL_SetCaptureOptionDlg) ? pL_SetCaptureOptionDlg(hwndOwner,uFlags,pOptions,pfnCallBack,pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREAREAOPTIONDLG(hParentWnd,uFlags,pCaptureAreaOption,nUseDefault,pfnCallBack,pUserData) \
   ((pL_CaptureAreaOptionDlg) ? pL_CaptureAreaOptionDlg(hParentWnd,uFlags,pCaptureAreaOption,nUseDefault,pfnCallBack,pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREOBJECTOPTIONDLG(hParentWnd,uFlags,pObjectOptions,nUseDefault,pfnCallBack,pUserData) \
   ((pL_CaptureObjectOptionDlg) ? pL_CaptureObjectOptionDlg(hParentWnd,uFlags,pObjectOptions,nUseDefault,pfnCallBack,pUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPGETDEFAULTAREAOPTION(pCaptureAreaOption, uStructSize) \
   ((pL_GetDefaultAreaOption) ? pL_GetDefaultAreaOption(pCaptureAreaOption, uStructSize) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPGETDEFAULTOBJECTOPTION(pObjectOptions, uStructSize) \
   ((pL_GetDefaultObjectOption) ? pL_GetDefaultObjectOption(pObjectOptions, uStructSize) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPSTOPCAPTURE() \
   ((pL_StopCapture)? pL_StopCapture():WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREFROMEXEDLG(pBitmap,uBitmapStructSize,pszFileName,pTransparentColor,nResType,nDialogType,uFlags,pCaptureInfo,uInfoStructSize,pfnCaptureCallback,pUserData,pfnCallBack,pHlpUserData) \
   ((pL_CaptureFromExeDlg) ? pL_CaptureFromExeDlg(pBitmap,uBitmapStructSize,pszFileName,pTransparentColor,nResType,nDialogType,uFlags,pCaptureInfo,uInfoStructSize,pfnCaptureCallback,pUserData,pfnCallBack,pHlpUserData) : WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREFROMEXE(pBitmap, uBitmapStructSize,pszFileName,nResType,pResID,bCaptureByIndex,clrBackGnd,pfnCaptureCallback,pUserData)   \
   ((pL_CaptureFromExe)? pL_CaptureFromExe(pBitmap,uBitmapStructSize,pszFileName,nResType,pResID,bCaptureByIndex,clrBackGnd,pfnCaptureCallback,pUserData):WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPCAPTUREGETRESCOUNT(pszFileName,nResType,pnCount)  \
   ((pL_CaptureGetResCount)? pL_CaptureGetResCount(pszFileName,nResType,pnCount):WRPERR_LTSCR_DLL_NOT_LOADED)

#define L_WRPISCAPTUREACTIVE()   \
   ((pL_IsCaptureActive)? pL_IsCaptureActive():(LBase::RecordError(WRPERR_LTSCR_DLL_NOT_LOADED),FALSE))

//-----------------------------------------------------------------------------
//--LTNET.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPINETCONNECT(pszAddress, nPort, phComputer, pfnCallback, pUserData) \
      ((pL_InetConnect )?pL_InetConnect(pszAddress, nPort, phComputer, pfnCallback, pUserData):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSERVERINIT(nPort, phComputer, pfnCallback, pUserData) \
      ((pL_InetServerInit )?pL_InetServerInit(nPort, phComputer, pfnCallback, pUserData):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETCLOSE(hComputer, bGraceful) \
      ((pL_InetClose )?pL_InetClose(hComputer, bGraceful):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDDATA(hComputer, pBuffer, pulBufferLength, uDataType) \
      ((pL_InetSendData )?pL_InetSendData(hComputer, pBuffer, pulBufferLength, uDataType):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDMMDATA(hComputer, pBuffer, pulBufferLength) \
      ((pL_InetSendMMData )?pL_InetSendMMData(hComputer, pBuffer, pulBufferLength):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETREADDATA(hComputer, pBuffer, pulBufferLength) \
      ((pL_InetReadData )?pL_InetReadData(hComputer, pBuffer, pulBufferLength):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETGETHOSTNAME(hHost, pszName, nType, pulBufferLength) \
      ((pL_InetGetHostName )?pL_InetGetHostName(hHost, pszName, nType, pulBufferLength):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETACCEPTCONNECT(hHost, phRemote, pfnCallback, pUserData) \
      ((pL_InetAcceptConnect )?pL_InetAcceptConnect(hHost, phRemote, pfnCallback, pUserData):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDBITMAP(hComputer, pBitmap, nFormat, nBitsPerPixel, nQFactor, pulImageLength) \
      ((pL_InetSendBitmap )?pL_InetSendBitmap(hComputer, pBitmap, nFormat, nBitsPerPixel, nQFactor, pulImageLength):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETAUTOPROCESS(hComputer, bProcess) \
      ((pL_InetAutoProcess )?pL_InetAutoProcess(hComputer, bProcess):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDRAWDATA(hComputer, pBuffer, pulBufferLength) \
      ((pL_InetSendRawData )?pL_InetSendRawData(hComputer, pBuffer, pulBufferLength):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETGETQUEUESIZE(hComputer, pulLength) \
      ((pL_InetGetQueueSize )?pL_InetGetQueueSize(hComputer, pulLength):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETCLEARQUEUE(hComputer) \
      ((pL_InetClearQueue )?pL_InetClearQueue(hComputer):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSTARTUP() \
      ((pL_InetStartUp )?pL_InetStartUp():WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSHUTDOWN() \
      ((pL_InetShutDown )?pL_InetShutDown():WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSOUND(hComputer, pWaveFormatData, pWaveData, pdwDataSize) \
      ((pL_InetSendSound )?pL_InetSendSound(hComputer, pWaveFormatData, pWaveData, pdwDataSize):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETATTACHTOSOCKET(phComputer, hSocket, pfnCallback, pUserData) \
      ((pL_InetAttachToSocket )?pL_InetAttachToSocket(phComputer, hSocket, pfnCallback, pUserData):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETDETACHFROMSOCKET(hComputer, bWaitTillQueueEmpty, phSocket) \
      ((pL_InetDetachFromSocket )?pL_InetDetachFromSocket(hComputer, bWaitTillQueueEmpty, phSocket):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSETCALLBACK(hComputer, pfnCallback, pUserData) \
      ((pL_InetSetCallback )?pL_InetSetCallback(hComputer, pfnCallback, pUserData):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETGETCALLBACK(hComputer, ppfnCallback, ppUserData) \
      ((pL_InetGetCallback )?pL_InetGetCallback(hComputer, ppfnCallback, ppUserData):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETCREATEPACKET(phPacket, uExtra, pExtra, pszFormat) \
      ((pL_InetCreatePacket )?pL_InetCreatePacket(phPacket, uExtra, pExtra, pszFormat,:WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETCREATEPACKETFROMPARAMS(phPacket, uExtra, pExtra, uParama, pParams) \
      ((pL_InetCreatePacketFromParams )?pL_InetCreatePacketFromParams(phPacket, uExtra, pExtra, uParama, pParams):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETFREEPACKET(hPacket) \
      ((pL_InetFreePacket )?pL_InetFreePacket(hPacket):LBase::RecordError(WRPERR_LTNET_DLL_NOT_LOADED))

#define L_WRPINETSENDCMD(hComputer, uCommand, uCommandID, hPacket) \
      ((pL_InetSendCmd )?pL_InetSendCmd(hComputer, uCommand, uCommandID, hPacket):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDRSP(hComputer, uCommand, uCommandID, hPacket, nStatus) \
      ((pL_InetSendRsp )?pL_InetSendRsp(hComputer, uCommand, uCommandID, hPacket, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDLOADCMD(hComputer, uCommandID, pszFile, nBitsPerPixel, nOrder, uFlags) \
      ((pL_InetSendLoadCmd )?pL_InetSendLoadCmd(hComputer, uCommandID, pszFile, nBitsPerPixel, nOrder, uFlags):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDLOADRSP(hComputer, uCommandID, uBitmapID, uExtra, pExtra, nStatus) \
      ((pL_InetSendLoadRsp )?pL_InetSendLoadRsp(hComputer, uCommandID, uBitmapID, uExtra, pExtra, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSAVECMD(hComputer, uCommandID, pszFile, uBitmapID, nFormat, nBitsPerPixel, nQFactor, uFlags) \
      ((pL_InetSendSaveCmd )?pL_InetSendSaveCmd(hComputer, uCommandID, pszFile, uBitmapID, nFormat, nBitsPerPixel, nQFactor, uFlags):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSAVERSP(hComputer, uCommandID, uExtra, pExtra, nStatus) \
      ((pL_InetSendSaveRsp )?pL_InetSendSaveRsp(hComputer, uCommandID, uExtra, pExtra, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDCREATEWINCMD(hComputer, uCommandID, pszClassName, pszWindowName, ulFlags, nLeft, nTop, nWidth, nHeight, uParentID) \
      ((pL_InetSendCreateWinCmd )?pL_InetSendCreateWinCmd(hComputer, uCommandID, pszClassName, pszWindowName, ulFlags, nLeft, nTop, nWidth, nHeight, uParentID):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDCREATEWINRSP(hComputer, uCommandID, uWindowID, uLength, pExtraInfo, nStatus) \
      ((pL_InetSendCreateWinRsp )?pL_InetSendCreateWinRsp(hComputer, uCommandID, uWindowID, uLength, pExtraInfo, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSIZEWINCMD(hComputer, uCommandID, uWindowID, nLeft, nTop, nWidth, nHeight) \
      ((pL_InetSendSizeWinCmd )?pL_InetSendSizeWinCmd(hComputer, uCommandID, uWindowID, nLeft, nTop, nWidth, nHeight):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSIZEWINRSP(hComputer, uCommandID, uLength, pExtraInfo, nStatus) \
      ((pL_InetSendSizeWinRsp )?pL_InetSendSizeWinRsp(hComputer, uCommandID, uLength, pExtraInfo, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSHOWWINCMD(hComputer, uCommandID, uWindowID, nCmdShow) \
      ((pL_InetSendShowWinCmd )?pL_InetSendShowWinCmd(hComputer, uCommandID, uWindowID, nCmdShow):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSHOWWINRSP(hComputer, uCommandID, uLength, pExtraInfo, nStatus) \
      ((pL_InetSendShowWinRsp )?pL_InetSendShowWinRsp(hComputer, uCommandID, uLength, pExtraInfo, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDCLOSEWINCMD(hComputer, uCommandID, uWindowID) \
      ((pL_InetSendCloseWinCmd )?pL_InetSendCloseWinCmd(hComputer, uCommandID, uWindowID):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDCLOSEWINRSP(hComputer, uCommandID, uLength, pExtraInfo, nStatus) \
      ((pL_InetSendCloseWinRsp )?pL_InetSendCloseWinRsp(hComputer, uCommandID, uLength, pExtraInfo, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDFREEBITMAPCMD(hComputer, uCommandID, uBitmapID) \
      ((pL_InetSendFreeBitmapCmd )?pL_InetSendFreeBitmapCmd(hComputer, uCommandID, uBitmapID):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDFREEBITMAPRSP(hComputer, uCommandID, uLength, pExtraInfo, nStatus) \
      ((pL_InetSendFreeBitmapRsp )?pL_InetSendFreeBitmapRsp(hComputer, uCommandID, uLength, pExtraInfo, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSETRECTCMD(hComputer, uCommandID, uWindowID, nType, nLeft, nTop, nWidth, nHeight) \
      ((pL_InetSendSetRectCmd )?pL_InetSendSetRectCmd(hComputer, uCommandID, uWindowID, nType, nLeft, nTop, nWidth, nHeight):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDSETRECTRSP(hComputer, uCommandID, uLength, pExtraInfo, nStatus) \
      ((pL_InetSendSetRectRsp )?pL_InetSendSetRectRsp(hComputer, uCommandID, uLength, pExtraInfo, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSETCOMMANDCALLBACK(hComputer, pfnCallback, pUserData) \
      ((pL_InetSetCommandCallback )?pL_InetSetCommandCallback(hComputer, pfnCallback, pUserData):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSETRESPONSECALLBACK(hComputer, pfnCallback, pUserData) \
      ((pL_InetSetResponseCallback )?pL_InetSetResponseCallback(hComputer, pfnCallback, pUserData):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDATTACHBITMAPCMD(hComputer, uCommandID, uBitmapID, uWindowID) \
      ((pL_InetSendAttachBitmapCmd )?pL_InetSendAttachBitmapCmd(hComputer, uCommandID, uBitmapID, uWindowID):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDATTACHBITMAPRSP(hComputer, uCommandID, uExtra, pExtra, nStatus) \
      ((pL_InetSendAttachBitmapRsp )?pL_InetSendAttachBitmapRsp(hComputer, uCommandID, uExtra, pExtra, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDGETMAGGLASSDATACMD(hComputer, uCommandID, uBitmapID, nMaskPlaneSize, pMaskPlane, nMaskPlaneStart, nMaskPlaneEnd) \
      ((pL_InetSendGetMagGlassDataCmd )?pL_InetSendGetMagGlassDataCmd(hComputer, uCommandID, uBitmapID, nMaskPlaneSize, pMaskPlane, nMaskPlaneStart, nMaskPlaneEnd):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETSENDGETMAGGLASSDATARSP(hComputer, uCommandID, lColorSize, pColor, nMaskPlaneSize, pMaskPlane, nMaskPlaneStart, nMaskPlaneEnd, uExtra, pExtra, nStatus) \
      ((pL_InetSendGetMagGlassDataRsp )?pL_InetSendGetMagGlassDataRsp(hComputer, uCommandID, lColorSize, pColor, nMaskPlaneSize, pMaskPlane, nMaskPlaneStart, nMaskPlaneEnd, uExtra, pExtra, nStatus):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETGETMAGGLASSDATA(pBitmap, plColorSize, pColor, pMaskPlane, nMaskPlaneStart, nMaskPlaneEnd) \
      ((pL_InetGetMagGlassData )?pL_InetGetMagGlassData(pBitmap, plColorSize, pColor, pMaskPlane, nMaskPlaneStart, nMaskPlaneEnd):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETGETPARAMETERS( puParams, ppParams, pszFormat, pArgs) \
   ((pL_InetGetParameters)? pL_InetGetParameters( puParams, ppParams, pszFormat, pArgs):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETCOPYPARAMETERS( ppParams, uParams, pParams) \
   ((pL_InetCopyParameters)? pL_InetCopyParameters( ppParams, uParams, pParams):WRPERR_LTNET_DLL_NOT_LOADED)

#define L_WRPINETFREEPARAMETERS( pParams, nCount) \
   ((pL_InetFreeParameters)? pL_InetFreeParameters( pParams, nCount):(LBase::RecordError(WRPERR_LTNET_DLL_NOT_LOADED)))

//-----------------------------------------------------------------------------
//--LTWEB.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPINETFTPCONNECT(pszServer,iPort,pszUserName,pszPassword,pFtp) \
      ((pL_InetFtpConnect )?pL_InetFtpConnect(pszServer,iPort,pszUserName,pszPassword,pFtp):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPDISCONNECT(hFtp) \
      ((pL_InetFtpDisConnect )?pL_InetFtpDisConnect(hFtp):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPSENDFILE(hFtp,pszLocal,pszRemote,uSendAs) \
      ((pL_InetFtpSendFile )?pL_InetFtpSendFile(hFtp,pszLocal,pszRemote,uSendAs):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPCHANGEDIR(hFtp,pszDirectory) \
      ((pL_InetFtpChangeDir )?pL_InetFtpChangeDir(hFtp,pszDirectory):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPGETFILE(hFtp,pszRemote,pszLocal,bOverwrite,uSendAs) \
      ((pL_InetFtpGetFile )?pL_InetFtpGetFile(hFtp,pszRemote,pszLocal,bOverwrite,uSendAs):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPRENAMEFILE(hFtp,pszOld,pszNew) \
      ((pL_InetFtpRenameFile )?pL_InetFtpRenameFile(hFtp,pszOld,pszNew):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPDELETEFILE(hFtp,pszRemote) \
      ((pL_InetFtpDeleteFile )?pL_InetFtpDeleteFile(hFtp,pszRemote):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPCREATEDIR(hFtp,pszRemoteDir) \
      ((pL_InetFtpCreateDir )?pL_InetFtpCreateDir(hFtp,pszRemoteDir):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPDELETEDIR(hFtp,pszRemoteDir) \
      ((pL_InetFtpDeleteDir )?pL_InetFtpDeleteDir(hFtp,pszRemoteDir):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPGETCURRENTDIR(hFtp,pszRemoteDir,ulSize) \
      ((pL_InetFtpGetCurrentDir )?pL_InetFtpGetCurrentDir(hFtp,pszRemoteDir,ulSize):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPSENDBITMAP(hFtp,pBitmap,nFormat,nBitsPerPixel,nQFactor,pSaveOptions,pszRemote,uSendAs) \
      ((pL_InetFtpSendBitmap )?pL_InetFtpSendBitmap(hFtp,pBitmap,nFormat,nBitsPerPixel,nQFactor,pSaveOptions,pszRemote,uSendAs):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETFTPBROWSEDIR(hFtp,pszSearch,pfnCallback,pData) \
      ((pL_InetFtpBrowseDir )?pL_InetFtpBrowseDir(hFtp,pszSearch,pfnCallback,pData):WRPERR_LTWEB_DLL_NOT_LOADED)

// HTTP functions

#define L_WRPINETHTTPCONNECT(pszServer,iPort,pszUserName,pszPassword,pHttp) \
      ((pL_InetHttpConnect )?pL_InetHttpConnect(pszServer,iPort,pszUserName,pszPassword,pHttp):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPDISCONNECT(hHttp) \
      ((pL_InetHttpDisconnect )?pL_InetHttpDisconnect(hHttp):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPOPENREQUEST(hHttp,uType,pszTarget,pszReferer,pszVersion,dwReserved) \
      ((pL_InetHttpOpenRequest )?pL_InetHttpOpenRequest(hHttp,uType,pszTarget,pszReferer,pszVersion,dwReserved):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPOPENREQUESTEX(hHttp,uType,pszTarget,pszReferer,pszVersion,dwReserved,uFlags) \
      ((pL_InetHttpOpenRequestEx )?pL_InetHttpOpenRequestEx(hHttp,uType,pszTarget,pszReferer,pszVersion,dwReserved,uFlags):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPCLOSEREQUEST(hHttp) \
      ((pL_InetHttpCloseRequest )?pL_InetHttpCloseRequest(hHttp):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPSENDREQUEST(hHttp,pszHeader,ulHeaderSize,pszOptional,ulOptionalSize) \
      ((pL_InetHttpSendRequest )?pL_InetHttpSendRequest(hHttp,pszHeader,ulHeaderSize,pszOptional,ulOptionalSize):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPSENDBITMAP(hHttp,pBitmap,nFormat,nBitsPerPixel,nQFactor,pszContentType,pNameValue,pSaveOptions) \
      ((pL_InetHttpSendBitmap )?pL_InetHttpSendBitmap(hHttp,pBitmap,nFormat,nBitsPerPixel,nQFactor,pszContentType,pNameValue,pSaveOptions):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPSENDDATA(hHttp,pData,uSize,pszContentType,pNameValue) \
      ((pL_InetHttpSendData )?pL_InetHttpSendData(hHttp,pData,uSize,pszContentType,pNameValue):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPSENDFORM(hHttp,pNameValue,uCount) \
      ((pL_InetHttpSendForm )?pL_InetHttpSendForm(hHttp,pNameValue,uCount):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPGETRESPONSE(hHttp,pszData,ulSize) \
      ((pL_InetHttpGetResponse )?pL_InetHttpGetResponse(hHttp,pszData,ulSize):WRPERR_LTWEB_DLL_NOT_LOADED)

#define L_WRPINETHTTPGETSERVERSTATUS(hHttp,uStatus) \
      ((pL_InetHttpGetServerStatus )?pL_InetHttpGetServerStatus(hHttp,uStatus):WRPERR_LTWEB_DLL_NOT_LOADED)


//-----------------------------------------------------------------------------
//--LTTMB.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPBROWSEDIR(pszPath, pszFilter, pThumbOptions, bStopOnError, bIncludeSubDirs, bExpandMultipage, lSizeDisk, lSizeMem, pfnBrowseDirCB, pUserData) \
   ((pL_BrowseDir )?pL_BrowseDir(pszPath, pszFilter, pThumbOptions, bStopOnError, bIncludeSubDirs, bExpandMultipage, lSizeDisk, lSizeMem, pfnBrowseDirCB, pUserData):WRPERR_LTTMB_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTLST.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPUSEIMAGELISTCONTROL() \
   ((pL_UseImageListControl)? pL_UseImageListControl():(LBase::RecordError(WRPERR_LTLST_DLL_NOT_LOADED)))


#define L_WRPCREATEIMAGELISTCONTROL( dwStyle, x, y, nWidth, nHeight, hWndParent, nID, crBack) \
   ((pL_CreateImageListControl)? pL_CreateImageListControl( dwStyle, x, y, nWidth, nHeight, hWndParent, nID, crBack):(LBase::RecordError(WRPERR_LTLST_DLL_NOT_LOADED),(HWND)0))

//-----------------------------------------------------------------------------
//--LVKRN.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPVECDUPLICATEOBJECTDESCRIPTOR(nType, pObjectDescDst, pObjectDescSrc ) \
   ((pL_VecDuplicateObjectDescriptor)? pL_VecDuplicateObjectDescriptor( nType, pObjectDescDst, pObjectDescSrc ):WRPERR_LVKRN_DLL_NOT_LOADED)

//Do not remove the one above
/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  General functions.                                                   []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECINIT(pVector) \
   ((pL_VecInit )?pL_VecInit(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECFREE(pVector) \
   ((pL_VecFree )?pL_VecFree(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECEMPTY(pVector) \
   ((pL_VecEmpty )?pL_VecEmpty(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECISEMPTY(pVector) \
   ((pL_VecIsEmpty )?pL_VecIsEmpty(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCOPY(pDst, pSrc, dwFlags) \
   ((pL_VecCopy )?pL_VecCopy(pDst, pSrc, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETDISPLAYOPTIONS(pVector, pOptions) \
   ((pL_VecSetDisplayOptions )?pL_VecSetDisplayOptions(pVector, pOptions):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETDISPLAYOPTIONS(pVector, pOptions) \
   ((pL_VecGetDisplayOptions )?pL_VecGetDisplayOptions(pVector, pOptions):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECINVERTCOLORS(pVector) \
   ((pL_VecInvertColors )?pL_VecInvertColors(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETVIEWPORT(pVector, pViewport) \
   ((pL_VecSetViewport )?pL_VecSetViewport(pVector, pViewport):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETVIEWPORT(pVector, pViewport) \
   ((pL_VecGetViewport )?pL_VecGetViewport(pVector, pViewport):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETPAN(pVector, pPan) \
   ((pL_VecSetPan )?pL_VecSetPan(pVector, pPan):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETPAN(pVector, pPan) \
   ((pL_VecGetPan )?pL_VecGetPan(pVector, pPan):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECPAINT(hDC, pVector, bEraseBkgnd) \
   ((pL_VecPaint )?pL_VecPaint(hDC, pVector, bEraseBkgnd):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECREALIZE(pBitmap, pVetcor, bEraseBkgnd) \
   ((pL_VecRealize )?pL_VecRealize(pBitmap, pVetcor, bEraseBkgnd):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECPAINTDC(hDC, pVector, uWidth, uHeight, pSrc, pSrcClip, pDest, pDestClip, dwFlags) \
   ((pL_VecPaintDC )?pL_VecPaintDC(hDC, pVector, uWidth, uHeight, pSrc, pSrcClip, pDest, pDestClip, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECIS3D(pVector) \
   ((pL_VecIs3D )?pL_VecIs3D(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECISLOCKED(pVector) \
   ((pL_VecIsLocked )?pL_VecIsLocked(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETLOCKED(pVector, bLock) \
   ((pL_VecSetLocked )?pL_VecSetLocked(pVector, bLock):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETBACKGROUNDCOLOR(pVector, Color) \
   ((pL_VecSetBackgroundColor )?pL_VecSetBackgroundColor(pVector, Color):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETBACKGROUNDCOLOR(pVector) \
   ((pL_VecGetBackgroundColor )?pL_VecGetBackgroundColor(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECLOGICALTOPHYSICAL(pVector, pDst, pSrc) \
   ((pL_VecLogicalToPhysical )?pL_VecLogicalToPhysical(pVector, pDst, pSrc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECPHYSICALTOLOGICAL(pVector, pDst, pSrc) \
   ((pL_VecPhysicalToLogical )?pL_VecPhysicalToLogical(pVector, pDst, pSrc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETPALETTE(pVector, hPalette) \
   ((pL_VecSetPalette )?pL_VecSetPalette(pVector, hPalette):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETPALETTE(pVector) \
   ((pL_VecGetPalette )?pL_VecGetPalette(pVector):(LBase::RecordError(WRPERR_LVKRN_DLL_NOT_LOADED),(L_HPALETTE)0))

#define L_WRPVECSETVIEWMODE(pVector, nMode) \
   ((pL_VecSetViewMode )?pL_VecSetViewMode(pVector, nMode):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETVIEWMODE(pVector) \
   ((pL_VecGetViewMode )?pL_VecGetViewMode(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Transformation function.                                             []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECSETTRANSLATION(pVector, pTranslation, pObject, dwFlags) \
   ((pL_VecSetTranslation )?pL_VecSetTranslation(pVector, pTranslation, pObject, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETTRANSLATION(pVector, pTranslation) \
   ((pL_VecGetTranslation )?pL_VecGetTranslation(pVector, pTranslation):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETROTATION(pVector, pRotation, pObject, pOrigin, dwFlags) \
   ((pL_VecSetRotation )?pL_VecSetRotation(pVector, pRotation, pObject, pOrigin, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETROTATION(pVector, pRotation) \
   ((pL_VecGetRotation )?pL_VecGetRotation(pVector, pRotation):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETSCALE(pVector, pScale, pObject, pOrigin, dwFlags) \
   ((pL_VecSetScale )?pL_VecSetScale(pVector, pScale, pObject, pOrigin, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETSCALE(pVector, pScale) \
   ((pL_VecGetScale )?pL_VecGetScale(pVector, pScale):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETORIGIN(pVector, pOrigin) \
   ((pL_VecSetOrigin )?pL_VecSetOrigin(pVector, pOrigin):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETORIGIN(pVector, pOrigin) \
   ((pL_VecGetOrigin )?pL_VecGetOrigin(pVector, pOrigin):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECAPPLYTRANSFORMATION(pVector) \
   ((pL_VecApplyTransformation )?pL_VecApplyTransformation(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECZOOMRECT(pVector, pRect) \
   ((pL_VecZoomRect )?pL_VecZoomRect(pVector, pRect):WRPERR_LVKRN_DLL_NOT_LOADED)

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Attributes functions.                                                []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECSETBINDVERTICESMODE(pVector, nMode) \
   ((pL_VecSetBindVerticesMode )?pL_VecSetBindVerticesMode(pVector, nMode):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETBINDVERTICESMODE(pVector) \
   ((pL_VecGetBindVerticesMode )?pL_VecGetBindVerticesMode(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETPARALLELOGRAM(pVector, pMin, pMax) \
   ((pL_VecSetParallelogram )?pL_VecSetParallelogram(pVector, pMin, pMax):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETPARALLELOGRAM(pVector, pMin, pMax) \
   ((pL_VecGetParallelogram )?pL_VecGetParallelogram(pVector, pMin, pMax):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECENUMVERTICES(pVector, pEnumProc, pUserData, dwFlags) \
   ((pL_VecEnumVertices )?pL_VecEnumVertices(pVector, pEnumProc, pUserData, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Camera functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECSETCAMERA(pVector, pCamera) \
   ((pL_VecSetCamera )?pL_VecSetCamera(pVector, pCamera):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETCAMERA(pVector, pCamera) \
   ((pL_VecGetCamera )?pL_VecGetCamera(pVector, pCamera):WRPERR_LVKRN_DLL_NOT_LOADED)

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Metafile functions.                                                  []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECCONVERTTOWMF(hDC, pVector, pRect, uDPI) \
   ((pL_VecConvertToWMF )?pL_VecConvertToWMF(hDC, pVector, pRect, uDPI):(LBase::RecordError(WRPERR_LVKRN_DLL_NOT_LOADED),(L_HMETAFILE)0))

#define L_WRPVECCONVERTFROMWMF(hDC, pVector, hWMF) \
   ((pL_VecConvertFromWMF )?pL_VecConvertFromWMF(hDC, pVector, hWMF):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCONVERTTOEMF(hDC, pVector, pRect, uDPI) \
   ((pL_VecConvertToEMF )?pL_VecConvertToEMF(hDC, pVector, pRect, uDPI):(LBase::RecordError(WRPERR_LVKRN_DLL_NOT_LOADED),(L_HENHMETAFILE)0))

#define L_WRPVECCONVERTFROMEMF(hDC, pVector, hEMF) \
   ((pL_VecConvertFromEMF )?pL_VecConvertFromEMF(hDC, pVector, hEMF):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Engine functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECATTACHTOWINDOW(hWnd, pVector, dwFlags) \
   ((pL_VecAttachToWindow )?pL_VecAttachToWindow(hWnd, pVector, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Marker functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECSETMARKER(pVector, pMarker) \
   ((pL_VecSetMarker )?pL_VecSetMarker(pVector, pMarker):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETMARKER(pVector, pMarker) \
   ((pL_VecGetMarker )?pL_VecGetMarker(pVector, pMarker):WRPERR_LVKRN_DLL_NOT_LOADED)

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Unit functions.                                                      []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
/* Reserved for internal use */
#define L_WRPVECSETUNIT(pVector, pUnit) \
   ((pL_VecSetUnit )?pL_VecSetUnit(pVector, pUnit):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETUNIT(pVector, pUnit) \
   ((pL_VecGetUnit )?pL_VecGetUnit(pVector, pUnit):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCONVERTPOINTTOUNIT(pVector, pptDst, pptSrc, UnitToUse) \
   ((pL_VecConvertPointToUnit )?pL_VecConvertPointToUnit(pVector, pptDst, pptSrc, UnitToUse):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCONVERTPOINTFROMUNIT(pVector, pptDst, pptSrc, UnitToUse) \
   ((pL_VecConvertPointFromUnit )?pL_VecConvertPointFromUnit(pVector, pptDst, pptSrc, UnitToUse):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Hit test functions.                                                  []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECSETHITTEST(pVector, pHitTest) \
   ((pL_VecSetHitTest )?pL_VecSetHitTest(pVector, pHitTest):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETHITTEST(pVector, pHitTest) \
   ((pL_VecGetHitTest )?pL_VecGetHitTest(pVector, pHitTest):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECHITTEST(pVector, pPoint, pObject) \
   ((pL_VecHitTest )?pL_VecHitTest(pVector, pPoint, pObject):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Polygon functions.                                                   []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECSETPOLYGONMODE(pVector, nMode) \
   ((pL_VecSetPolygonMode )?pL_VecSetPolygonMode(pVector, nMode):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETPOLYGONMODE(pVector) \
   ((pL_VecGetPolygonMode )?pL_VecGetPolygonMode(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETUSELIGHTS(pVector) \
   ((pL_VecGetUseLights )?pL_VecGetUseLights(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETAMBIENTCOLOR(pVector, Color) \
   ((pL_VecSetAmbientColor )?pL_VecSetAmbientColor(pVector, Color):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETAMBIENTCOLOR(pVector) \
   ((pL_VecGetAmbientColor )?pL_VecGetAmbientColor(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Clipboard functions.                                                 []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECCLIPBOARDREADY() \
   ((pL_VecClipboardReady )?pL_VecClipboardReady():WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCOPYTOCLIPBOARD(hWnd, pVector, dwFlags) \
   ((pL_VecCopyToClipboard )?pL_VecCopyToClipboard(hWnd, pVector, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCOPYFROMCLIPBOARD(hWnd, pVector, dwFlags) \
   ((pL_VecCopyFromClipboard )?pL_VecCopyFromClipboard(hWnd, pVector, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Layer functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECADDLAYER(pVector, pLayerDesc, pLayer, dwFlags) \
   ((pL_VecAddLayer )?pL_VecAddLayer(pVector, pLayerDesc, pLayer, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDELETELAYER(pVector, pLayer) \
   ((pL_VecDeleteLayer )?pL_VecDeleteLayer(pVector, pLayer):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECEMPTYLAYER(pVector, pLayer) \
   ((pL_VecEmptyLayer )?pL_VecEmptyLayer(pVector, pLayer):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCOPYLAYER(pVectorDst, pLayerDst, pVectorSrc, pLayerSrc, dwFlags) \
   ((pL_VecCopyLayer )?pL_VecCopyLayer(pVectorDst, pLayerDst, pVectorSrc, pLayerSrc, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETLAYERBYNAME(pVector, pszName, pLayer) \
   ((pL_VecGetLayerByName )?pL_VecGetLayerByName(pVector, pszName, pLayer):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETLAYERCOUNT(pVector) \
   ((pL_VecGetLayerCount )?pL_VecGetLayerCount(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETLAYERBYINDEX(pVector, nIndex, pLayer) \
   ((pL_VecGetLayerByIndex )?pL_VecGetLayerByIndex(pVector, nIndex, pLayer):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETLAYER(pVector, pLayer, pLayerDesc) \
   ((pL_VecGetLayer )?pL_VecGetLayer(pVector, pLayer, pLayerDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECFREELAYER(pLayerDesc) \
   ((pL_VecFreeLayer )?pL_VecFreeLayer(pLayerDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETLAYER(pVector, pLayer, pLayerDesc) \
   ((pL_VecSetLayer )?pL_VecSetLayer(pVector, pLayer, pLayerDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETACTIVELAYER(pVector, pLayer) \
   ((pL_VecSetActiveLayer )?pL_VecSetActiveLayer(pVector, pLayer):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETACTIVELAYER(pVector, pLayer) \
   ((pL_VecGetActiveLayer )?pL_VecGetActiveLayer(pVector, pLayer):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECENUMLAYERS(pVector, pEnumProc, pUserData) \
   ((pL_VecEnumLayers )?pL_VecEnumLayers(pVector, pEnumProc, pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Group functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECADDGROUP(pVector, pGroupDesc, pGroup, dwFlags) \
   ((pL_VecAddGroup )?pL_VecAddGroup(pVector, pGroupDesc, pGroup, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDELETEGROUP(pVector, pGroup) \
   ((pL_VecDeleteGroup )?pL_VecDeleteGroup(pVector, pGroup):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDELETEGROUPCLONES(pVector, pGroup, dwFlags) \
   ((pL_VecDeleteGroupClones )?pL_VecDeleteGroupClones(pVector, pGroup, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECEMPTYGROUP(pVector, pGroup) \
   ((pL_VecEmptyGroup )?pL_VecEmptyGroup(pVector, pGroup):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCOPYGROUP(pVectorDst, pGroupDst, pVectorSrc, pGroupSrc, dwFlags) \
   ((pL_VecCopyGroup )?pL_VecCopyGroup(pVectorDst, pGroupDst, pVectorSrc, pGroupSrc, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETGROUPBYNAME(pVector, pszName, pGroup) \
   ((pL_VecGetGroupByName )?pL_VecGetGroupByName(pVector, pszName, pGroup):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETGROUPCOUNT(pVector) \
   ((pL_VecGetGroupCount )?pL_VecGetGroupCount(pVector):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETGROUPBYINDEX(pVector, nIndex, pGroup) \
   ((pL_VecGetGroupByIndex )?pL_VecGetGroupByIndex(pVector, nIndex, pGroup):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETGROUP(pVector, pGroup, pGroupDesc) \
   ((pL_VecGetGroup )?pL_VecGetGroup(pVector, pGroup, pGroupDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECFREEGROUP(pGroupDesc) \
   ((pL_VecFreeGroup )?pL_VecFreeGroup(pGroupDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETGROUP(pVector, pGroup, pGroupDesc) \
   ((pL_VecSetGroup )?pL_VecSetGroup(pVector, pGroup, pGroupDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECENUMGROUPS(pVector, pEnumProc, pUserData) \
   ((pL_VecEnumGroups )?pL_VecEnumGroups(pVector, pEnumProc, pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Object functions.                                                    []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECINITOBJECT(pObject) \
   ((pL_VecInitObject )?pL_VecInitObject(pObject):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECADDOBJECT(pVector, pLayer, nType, pObjectDesc, pNewObject) \
   ((pL_VecAddObject )?pL_VecAddObject(pVector, pLayer, nType, pObjectDesc, pNewObject):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDELETEOBJECT(pVector, pObject, dwFlags) \
   ((pL_VecDeleteObject )?pL_VecDeleteObject(pVector, pObject, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECCOPYOBJECT(pVectorDst, pLayerDst, pObjectDst, pVectorSrc, pObjectSrc) \
   ((pL_VecCopyObject )?pL_VecCopyObject(pVectorDst, pLayerDst, pObjectDst, pVectorSrc, pObjectSrc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETOBJECT(pVector, pObject, nType, pObjectDesc) \
   ((pL_VecGetObject )?pL_VecGetObject(pVector, pObject, nType, pObjectDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECFREEOBJECT(nType, pObjectDesc) \
   ((pL_VecFreeObject )?pL_VecFreeObject(nType, pObjectDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETOBJECT(pVector, pObject, nType, pObjectDesc) \
   ((pL_VecSetObject )?pL_VecSetObject(pVector, pObject, nType, pObjectDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECEXPLODEOBJECT(pVector, pObject, dwFlags) \
   ((pL_VecExplodeObject )?pL_VecExplodeObject(pVector, pObject, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETOBJECTPARALLELOGRAM(pVector, pObject, pMin, pMax, dwFlags) \
   ((pL_VecGetObjectParallelogram )?pL_VecGetObjectParallelogram(pVector, pObject, pMin, pMax, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETOBJECTRECT(pVector, pObject, pRect, dwFlags) \
   ((pL_VecGetObjectRect )?pL_VecGetObjectRect(pVector, pObject, pRect, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECISOBJECTINSIDEPARALLELOGRAM(pVector, pObject, pMin, pMax, dwFlags) \
   ((pL_VecIsObjectInsideParallelogram )?pL_VecIsObjectInsideParallelogram(pVector, pObject, pMin, pMax, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECISOBJECTINSIDERECT(pVector, pObject, pRect, dwFlags) \
   ((pL_VecIsObjectInsideRect )?pL_VecIsObjectInsideRect(pVector, pObject, pRect, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSELECTOBJECT(pVector, pObject, bSelect) \
   ((pL_VecSelectObject )?pL_VecSelectObject(pVector, pObject, bSelect):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECISOBJECTSELECTED(pVector, pObject) \
   ((pL_VecIsObjectSelected )?pL_VecIsObjectSelected(pVector, pObject):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECHIDEOBJECT(pVector, pObject, bHide) \
   ((pL_VecHideObject )?pL_VecHideObject(pVector, pObject, bHide):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECISOBJECTHIDDEN(pVector, pObject) \
   ((pL_VecIsObjectHidden )?pL_VecIsObjectHidden(pVector, pObject):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECENUMOBJECTS(pVector, pEnumProc, pUserData, dwFlags) \
   ((pL_VecEnumObjects )?pL_VecEnumObjects(pVector, pEnumProc, pUserData, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECENUMOBJECTSINLAYER(pVector, pLayer, pEnumProc, pUserData, dwFlags) \
   ((pL_VecEnumObjectsInLayer )?pL_VecEnumObjectsInLayer(pVector, pLayer, pEnumProc, pUserData, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETOBJECTATTRIBUTES(pVector, pObject, pnROP, pPen, pBrush, pFont, dwFlags) \
   ((pL_VecSetObjectAttributes )?pL_VecSetObjectAttributes(pVector, pObject, pnROP, pPen, pBrush, pFont, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETOBJECTATTRIBUTES(pVector, pObject, pnROP, pPen, pBrush, pFont) \
   ((pL_VecGetObjectAttributes )?pL_VecGetObjectAttributes(pVector, pObject, pnROP, pPen, pBrush, pFont):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECADDOBJECTTOGROUP(pVector, pGroup, nType, pObjectDesc, pNewObject) \
   ((pL_VecAddObjectToGroup )?pL_VecAddObjectToGroup(pVector, pGroup, nType, pObjectDesc, pNewObject):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECENUMOBJECTSINGROUP(pVector, pGroup, pEnumProc, pUserData, dwFlags) \
   ((pL_VecEnumObjectsInGroup )?pL_VecEnumObjectsInGroup(pVector, pGroup, pEnumProc, pUserData, dwFlags):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETOBJECTTOOLTIP(pVector, pObject, pszTooltip) \
   ((pL_VecSetObjectTooltip )?pL_VecSetObjectTooltip(pVector, pObject, pszTooltip):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETOBJECTTOOLTIP(pVector, pObject, pBuffer, uSize) \
   ((pL_VecGetObjectTooltip )?pL_VecGetObjectTooltip(pVector, pObject, pBuffer, uSize):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSHOWOBJECTTOOLTIP(pVector, pObject, pTooltipDesc) \
   ((pL_VecShowObjectTooltip )?pL_VecShowObjectTooltip(pVector, pObject, pTooltipDesc):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECHIDEOBJECTTOOLTIP(pVector, pObject) \
   ((pL_VecHideObjectTooltip )?pL_VecHideObjectTooltip(pVector, pObject):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETOBJECTVIEWCONTEXT(pVector, pObject, pMin, pMax) \
   ((pL_VecSetObjectViewContext )?pL_VecSetObjectViewContext(pVector, pObject, pMin, pMax):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETOBJECTVIEWCONTEXT(pVector, pObject, pMin, pMax) \
   ((pL_VecGetObjectViewContext )?pL_VecGetObjectViewContext(pVector, pObject, pMin, pMax):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECREMOVEOBJECTVIEWCONTEXT(pVector, pObject) \
   ((pL_VecRemoveObjectViewContext )?pL_VecRemoveObjectViewContext(pVector, pObject):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECADDHYPERLINK(pVector, pObject, pTarget) \
   ((pL_VecAddHyperlink )?pL_VecAddHyperlink(pVector, pObject, pTarget):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETHYPERLINK(pVector, pObject, uIndex, pTarget) \
   ((pL_VecSetHyperlink )?pL_VecSetHyperlink(pVector, pObject, uIndex, pTarget):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETHYPERLINK(pVector, pObject, uIndex, pTarget) \
   ((pL_VecGetHyperlink )?pL_VecGetHyperlink(pVector, pObject, uIndex, pTarget):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETHYPERLINKCOUNT(pVector, pObject) \
   ((pL_VecGetHyperlinkCount )?pL_VecGetHyperlinkCount(pVector, pObject):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGOTOHYPERLINK(pVector, pObject, uIndex) \
   ((pL_VecGotoHyperlink )?pL_VecGotoHyperlink(pVector, pObject, uIndex):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECSETOBJECTDESCRIPTION(pVector, pObject, pszTarget) \
   ((pL_VecSetObjectDescription )?pL_VecSetObjectDescription(pVector, pObject, pszTarget):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETOBJECTDESCRIPTION(pVector, pObject, pBuffer, uSize) \
   ((pL_VecGetObjectDescription )?pL_VecGetObjectDescription(pVector, pObject, pBuffer, uSize):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Event functions.                                                     []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECSETEVENTCALLBACK(pVector, pProc, pUserData, pOldProc, pOldUserData) \
   ((pL_VecSetEventCallback )?pL_VecSetEventCallback(pVector, pProc, pUserData, pOldProc, pOldUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECEVENT(pVector, pVectorEvent) \
   ((pL_VecEvent )?pL_VecEvent(pVector, pVectorEvent):WRPERR_LVKRN_DLL_NOT_LOADED)


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Font Substitution functions.                                         []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/
#define L_WRPVECSETFONTMAPPER(pVector, pVectorFontMapperCallback, pUserData) \
   ((pL_VecSetFontMapper )?pL_VecSetFontMapper(pVector, pVectorFontMapperCallback, pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECGETFONTMAPPER(pVector, pVectorFontMapperCallback) \
   ((pL_VecGetFontMapper )?pL_VecGetFontMapper(pVector, pVectorFontMapperCallback):WRPERR_LVKRN_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LVDLG.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPVECDLGROTATE( hWnd, pVector, pRotation, pOrigin, dwFlags, pfnHelpCallback, pUserData ) \
   ((pL_VecDlgRotate)? pL_VecDlgRotate( hWnd, pVector, pRotation, pOrigin, dwFlags, pfnHelpCallback, pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGSCALE(hWnd,pVector,pScale,pOrigin,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgScale )?pL_VecDlgScale(hWnd,pVector,pScale,pOrigin,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGTRANSLATE(hWnd,pVector,pTranslation,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgTranslate )?pL_VecDlgTranslate(hWnd,pVector,pTranslation,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGCAMERA(hWnd,pVector,pCamera,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgCamera )?pL_VecDlgCamera(hWnd,pVector,pCamera,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGRENDER(hWnd,pVector,pnPolygonMode,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgRender )?pL_VecDlgRender(hWnd,pVector,pnPolygonMode,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGVIEWMODE(hWnd,pVector,pnViewMode,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgViewMode )?pL_VecDlgViewMode(hWnd,pVector,pnViewMode,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGHITTEST(hWnd,pVector,pHitTest,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgHitTest )?pL_VecDlgHitTest(hWnd,pVector,pHitTest,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGEDITALLLAYERS(hWnd,pVector,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgEditAllLayers )?pL_VecDlgEditAllLayers(hWnd,pVector,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGNEWLAYER(hWnd,pVector,pLayerDesc,pLayer,pbActiveLayer,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgNewLayer )?pL_VecDlgNewLayer(hWnd,pVector,pLayerDesc,pLayer,pbActiveLayer,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGEDITLAYER(hWnd,pVector,pLayer,pLayerDesc,pbActiveLayer,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgEditLayer )?pL_VecDlgEditLayer(hWnd,pVector,pLayer,pLayerDesc,pbActiveLayer,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGEDITALLGROUPS(hWnd,pVector,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgEditAllGroups )?pL_VecDlgEditAllGroups(hWnd,pVector,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGNEWGROUP(hWnd,pVector,pGroupDesc,pGroup,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgNewGroup )?pL_VecDlgNewGroup(hWnd,pVector,pGroupDesc,pGroup,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGEDITGROUP(hWnd,pVector,pGroup,pGroupDesc,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgEditGroup )?pL_VecDlgEditGroup(hWnd,pVector,pGroup,pGroupDesc,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGNEWOBJECT(hWnd,pVector,pLayer,nType,pObjectDesc,pVectorObject,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgNewObject )?pL_VecDlgNewObject(hWnd,pVector,pLayer,nType,pObjectDesc,pVectorObject,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGEDITOBJECT(hWnd,pVector,pObject,nType,pObjectDesc,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgEditObject )?pL_VecDlgEditObject(hWnd,pVector,pObject,nType,pObjectDesc,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGOBJECTATTRIBUTES(hWnd,pVector,pObject,pbSelected,pnROP,pPen,pBrush,pFont,dwFlags,pfnCallback,pUserData) \
      ((pL_VecDlgObjectAttributes )?pL_VecDlgObjectAttributes(hWnd,pVector,pObject,pbSelected,pnROP,pPen,pBrush,pFont,dwFlags,pfnCallback,pUserData):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGGETSTRINGLEN(uString,puLen) \
      ((pL_VecDlgGetStringLen )?pL_VecDlgGetStringLen(uString,puLen):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGGETSTRING(uString,pszString) \
      ((pL_VecDlgGetString )?pL_VecDlgGetString(uString,pszString):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGSETSTRING(uString,pszString) \
      ((pL_VecDlgSetString )?pL_VecDlgSetString(uString,pszString):WRPERR_LVKRN_DLL_NOT_LOADED)

#define L_WRPVECDLGSETFONT(hFont) \
      ((pL_VecDlgSetFont )?pL_VecDlgSetFont(hFont):(LBase::RecordError(WRPERR_LVKRN_DLL_NOT_LOADED),(L_HFONT)0))

//-----------------------------------------------------------------------------
//--LTBAR.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPBARCODEREAD(pBitmap, prcSearch, ulSearchType, nUnits, ulFlags, nMultipleMaxCount, pBarCode1D, pBarCodePDF, pBarCodeColor, ppBarCodeData, uStructSize) \
      ((pL_BarCodeRead )?pL_BarCodeRead(pBitmap, prcSearch, ulSearchType, nUnits, ulFlags, nMultipleMaxCount, pBarCode1D, pBarCodePDF, pBarCodeColor, ppBarCodeData, uStructSize):WRPERR_LTBAR_DLL_NOT_LOADED)

#define L_WRPBARCODEWRITE(pBitmap, pBarCodeData, pBarCodeColor, ulFlags, pBarCode1D, pBarCodePDF, pBarCodeDM, pBarCodeQR, lprcSize) \
      ((pL_BarCodeWrite )?pL_BarCodeWrite(pBitmap, pBarCodeData, pBarCodeColor, ulFlags, pBarCode1D, pBarCodePDF, pBarCodeDM, pBarCodeQR, lprcSize):WRPERR_LTBAR_DLL_NOT_LOADED)

#define L_WRPBARCODEFREE(ppBarCodeData) \
      ((pL_BarCodeFree )?pL_BarCodeFree(ppBarCodeData):LBase::RecordError(WRPERR_LTBAR_DLL_NOT_LOADED))

#define L_WRPBARCODEISDUPLICATED(pBarCodeDataItem) \
      ((pL_BarCodeIsDuplicated )?pL_BarCodeIsDuplicated(pBarCodeDataItem):WRPERR_LTBAR_DLL_NOT_LOADED)

#define L_WRPBARCODEGETDUPLICATED(pBarCodeDataItem) \
      ((pL_BarCodeGetDuplicated )?pL_BarCodeGetDuplicated(pBarCodeDataItem):WRPERR_LTBAR_DLL_NOT_LOADED)

#define L_WRPBARCODEGETFIRSTDUPLICATED(pBarCodeData, nIndex) \
      ((pL_BarCodeGetFirstDuplicated )?pL_BarCodeGetFirstDuplicated(pBarCodeData, nIndex):WRPERR_LTBAR_DLL_NOT_LOADED)

#define L_WRPBARCODEGETNEXTDUPLICATED(pBarCodeData, nCurIndex) \
      ((pL_BarCodeGetNextDuplicated )?pL_BarCodeGetNextDuplicated(pBarCodeData, nCurIndex):WRPERR_LTBAR_DLL_NOT_LOADED)

#define L_WRPBARCODEINIT(nMajorType) \
      ((pL_BarCodeInit  )?pL_BarCodeInit (nMajorType):WRPERR_LTBAR_DLL_NOT_LOADED)

#define L_WRPBARCODEEXIT() \
      ((pL_BarCodeExit  )?pL_BarCodeExit():LBase::RecordError(WRPERR_LTBAR_DLL_NOT_LOADED))

#define L_WRPBARCODEVERSIONINFO(pBarCodeVersion, uStructSize) \
      ((pL_BarCodeVersionInfo )?pL_BarCodeVersionInfo(pBarCodeVersion, uStructSize):WRPERR_LTBAR_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTAUT.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
// General functions.
#define L_WRPAUTISVALID(pAutomation) \
      ((pL_AutIsValid )?pL_AutIsValid(pAutomation):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTINIT(ppAutomation) \
      ((pL_AutInit )?pL_AutInit(ppAutomation):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTCREATE(pAutomation, nMode, dwFlags) \
      ((pL_AutCreate )?pL_AutCreate(pAutomation, nMode, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTFREE(pAutomation) \
      ((pL_AutFree )?pL_AutFree(pAutomation):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTSETUNDOLEVEL(pAutomation, uLevel) \
      ((pL_AutSetUndoLevel )?pL_AutSetUndoLevel(pAutomation, uLevel):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTGETUNDOLEVEL(pAutomation, puLevel) \
      ((pL_AutGetUndoLevel )?pL_AutGetUndoLevel(pAutomation, puLevel):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTCANUNDO(pAutomation, pfCanUndo) \
      ((pL_AutCanUndo )?pL_AutCanUndo(pAutomation, pfCanUndo):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTCANREDO(pAutomation, pfCanRedo) \
      ((pL_AutCanRedo )?pL_AutCanRedo(pAutomation, pfCanRedo):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTUNDO(pAutomation, dwFlags) \
      ((pL_AutUndo )?pL_AutUndo(pAutomation, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTREDO(pAutomation, dwFlags) \
      ((pL_AutRedo )?pL_AutRedo(pAutomation, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTSETUNDOENABLED(pAutomation, bEnabled) \
      ((pL_AutSetUndoEnabled )?pL_AutSetUndoEnabled(pAutomation, bEnabled):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTADDUNDONODE(pAutomation, dwFlags) \
      ((pL_AutAddUndoNode )?pL_AutAddUndoNode(pAutomation, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTSELECT(pAutomation, nSelect, dwFlags) \
      ((pL_AutSelect )?pL_AutSelect(pAutomation, nSelect, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTCLIPBOARDDATAREADY(pAutomation, pfReady) \
      ((pL_AutClipboardDataReady )?pL_AutClipboardDataReady(pAutomation, pfReady):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTCUT(pAutomation, dwFlags) \
      ((pL_AutCut )?pL_AutCut(pAutomation, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTCOPY(pAutomation, dwFlags) \
      ((pL_AutCopy )?pL_AutCopy(pAutomation, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTPASTE(pAutomation, dwFlags) \
      ((pL_AutPaste )?pL_AutPaste(pAutomation, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTDELETE(pAutomation, dwFlags) \
      ((pL_AutDelete )?pL_AutDelete(pAutomation, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTPRINT(pAutomation, dwFlags) \
      ((pL_AutPrint )?pL_AutPrint(pAutomation, dwFlags):WRPERR_LTAUT_DLL_NOT_LOADED)

// Container Functions.
#define L_WRPAUTADDCONTAINER(pAutomation, pContainer , pModeData) \
      ((pL_AutAddContainer )?pL_AutAddContainer(pAutomation, pContainer , pModeData):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTREMOVECONTAINER(pAutomation, pContainer) \
      ((pL_AutRemoveContainer )?pL_AutRemoveContainer(pAutomation, pContainer):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTSETACTIVECONTAINER(pAutomation, pContainer) \
      ((pL_AutSetActiveContainer )?pL_AutSetActiveContainer(pAutomation, pContainer):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTGETACTIVECONTAINER(pAutomation, ppContainer) \
      ((pL_AutGetActiveContainer )?pL_AutGetActiveContainer(pAutomation, ppContainer):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTENUMCONTAINERS(pAutomation, pEnumProc, pUserData) \
      ((pL_AutEnumContainers )?pL_AutEnumContainers(pAutomation, pEnumProc, pUserData):WRPERR_LTAUT_DLL_NOT_LOADED)

// Painting Functionts.
#define L_WRPAUTSETPAINTPROPERTY(pAutomation, nGroup, pProperty) \
      ((pL_AutSetPaintProperty )?pL_AutSetPaintProperty(pAutomation, nGroup, pProperty):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTGETPAINTPROPERTY(pAutomation, nGroup, pProperty) \
      ((pL_AutGetPaintProperty )?pL_AutGetPaintProperty(pAutomation, nGroup, pProperty):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTSETPAINTBKCOLOR(pAutomation, rcBKColor) \
      ((pL_AutSetPaintBkColor )?pL_AutSetPaintBkColor(pAutomation, rcBKColor):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTGETPAINTBKCOLOR(pAutomation, prcBKColor) \
      ((pL_AutGetPaintBkColor )?pL_AutGetPaintBkColor(pAutomation, prcBKColor):WRPERR_LTAUT_DLL_NOT_LOADED)

// Vector Functions.
#define L_WRPAUTSETVECTORPROPERTY(pAutomation, pVectorObject) \
      ((pL_AutSetVectorProperty )?pL_AutSetVectorProperty(pAutomation, pVectorObject):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTGETVECTORPROPERTY(pAutomation, pVectorObject) \
      ((pL_AutGetVectorProperty )?pL_AutGetVectorProperty(pAutomation, pVectorObject):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTEDITVECTOROBJECT(pAutomation, pVectorObject) \
      ((pL_AutEditVectorObject )?pL_AutEditVectorObject(pAutomation, pVectorObject):WRPERR_LTAUT_DLL_NOT_LOADED)

//Toolbar Functions.
#define L_WRPAUTSETTOOLBAR(pAutomation, pToolbar) \
      ((pL_AutSetToolbar )?pL_AutSetToolbar(pAutomation, pToolbar):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTGETTOOLBAR(pAutomation, ppToolbar) \
      ((pL_AutGetToolbar )?pL_AutGetToolbar(pAutomation, ppToolbar):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTSETCURRENTTOOL(pAutomation, nTool) \
      ((pL_AutSetCurrentTool )?pL_AutSetCurrentTool(pAutomation, nTool):WRPERR_LTAUT_DLL_NOT_LOADED)

#define L_WRPAUTGETCURRENTTOOL(pAutomation, pnTool) \
      ((pL_AutGetCurrentTool )?pL_AutGetCurrentTool(pAutomation, pnTool):WRPERR_LTAUT_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTCON.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
 // general container operations funtions.
 #define L_WRPCONTAINERISVALID(pContainer) \
   ((pL_ContainerIsValid )?pL_ContainerIsValid(pContainer):WRPERR_LTCON_DLL_NOT_LOADED)

 #define L_WRPCONTAINERINIT(ppContainer) \
   ((pL_ContainerInit )?pL_ContainerInit(ppContainer):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERCREATE(pContainer, hwndOwner) \
   ((pL_ContainerCreate )?pL_ContainerCreate(pContainer, hwndOwner):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERFREE(pContainer) \
   ((pL_ContainerFree )?pL_ContainerFree(pContainer):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERUPDATE(pContainer, prcPaint) \
   ((pL_ContainerUpdate )?pL_ContainerUpdate(pContainer, prcPaint):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERRESET(pContainer) \
   ((pL_ContainerReset )?pL_ContainerReset(pContainer):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINEREDITOBJECT(pContainer, pObjectData) \
   ((pL_ContainerEditObject )?pL_ContainerEditObject(pContainer, pObjectData):WRPERR_LTCON_DLL_NOT_LOADED)
 
 // setting functions.
 #define L_WRPCONTAINERSETOWNER(pContainer, hWndOwner) \
   ((pL_ContainerSetOwner )?pL_ContainerSetOwner(pContainer, hWndOwner):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETMETRICS(pContainer, pMetrics) \
   ((pL_ContainerSetMetrics )?pL_ContainerSetMetrics(pContainer, pMetrics):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETOFFSET(pContainer, nXOffset, nYOffset, nZOffset) \
   ((pL_ContainerSetOffset )?pL_ContainerSetOffset(pContainer, nXOffset, nYOffset, nZOffset):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETSCALAR(pContainer, pvptScalarNum, pvptScalarDen) \
   ((pL_ContainerSetScalar )?pL_ContainerSetScalar(pContainer, pvptScalarNum, pvptScalarDen):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETOBJECTTYPE(pContainer, nObjectType) \
   ((pL_ContainerSetObjectType )?pL_ContainerSetObjectType(pContainer, nObjectType):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETOBJECTCURSOR(pContainer, nObjectType, hCursor) \
   ((pL_ContainerSetObjectCursor )?pL_ContainerSetObjectCursor(pContainer, nObjectType, hCursor):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETENABLED(pContainer, fEnable) \
   ((pL_ContainerSetEnabled )?pL_ContainerSetEnabled(pContainer, fEnable):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETCALLBACK(pContainer, pCallback, pUserData) \
   ((pL_ContainerSetCallback )?pL_ContainerSetCallback(pContainer, pCallback, pUserData):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETOWNERDRAW(pContainer, fOwnerDraw, dwFlags) \
   ((pL_ContainerSetOwnerDraw )?pL_ContainerSetOwnerDraw(pContainer, fOwnerDraw, dwFlags):WRPERR_LTCON_DLL_NOT_LOADED)
 
 // getting functions.
 #define L_WRPCONTAINERGETOWNER(pContainer, phwndOwner) \
   ((pL_ContainerGetOwner )?pL_ContainerGetOwner(pContainer, phwndOwner):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERGETMETRICS(pContainer, pMetrics) \
   ((pL_ContainerGetMetrics )?pL_ContainerGetMetrics(pContainer, pMetrics):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERGETOFFSET(pContainer, pnXOffset, pnYOffset, pnZOffset) \
   ((pL_ContainerGetOffset )?pL_ContainerGetOffset(pContainer, pnXOffset, pnYOffset, pnZOffset):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERGETSCALAR(pContainer, pvptScalarNum, pvptScalarDen) \
   ((pL_ContainerGetScalar )?pL_ContainerGetScalar(pContainer, pvptScalarNum, pvptScalarDen):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERGETOBJECTTYPE(pContainer, pnObjectType) \
   ((pL_ContainerGetObjectType )?pL_ContainerGetObjectType(pContainer, pnObjectType):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERGETOBJECTCURSOR(pContainer, nObjectType, phCursor) \
   ((pL_ContainerGetObjectCursor )?pL_ContainerGetObjectCursor(pContainer, nObjectType, phCursor):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERGETCALLBACK(pContainer, ppCallback, ppUserData) \
   ((pL_ContainerGetCallback )?pL_ContainerGetCallback(pContainer, ppCallback, ppUserData):WRPERR_LTCON_DLL_NOT_LOADED)
 
 // status query functions.
 #define L_WRPCONTAINERISENABLED(pContainer, pfEnabled) \
   ((pL_ContainerIsEnabled )?pL_ContainerIsEnabled(pContainer, pfEnabled):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERISOWNERDRAW(pContainer, pfOwnerDraw) \
   ((pL_ContainerIsOwnerDraw )?pL_ContainerIsOwnerDraw(pContainer, pfOwnerDraw):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERSETAUTOMATIONCALLBACK(pContainer, pAutomationCallback, pAutomationData) \
   ((pL_ContainerSetAutomationCallback )?pL_ContainerSetAutomationCallback(pContainer, pAutomationCallback, pAutomationData):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPSCREENTOCONTAINER(pContainer, pptPoint) \
   ((pL_ScreenToContainer )?pL_ScreenToContainer(pContainer, pptPoint):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERTOSCREEN(pContainer, pptPoint) \
   ((pL_ContainerToScreen )?pL_ContainerToScreen(pContainer, pptPoint):WRPERR_LTCON_DLL_NOT_LOADED)
 
 #define L_WRPCONTAINERENABLEUPDATE(pContainer, fEnableUpdate) \
   ((pL_ContainerEnableUpdate )?pL_ContainerEnableUpdate(pContainer, fEnableUpdate):WRPERR_LTCON_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTPNT.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPPNTISVALID(pPaint) \
      ((pL_PntIsValid )?pL_PntIsValid(pPaint):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTINIT(ppPaint) \
      ((pL_PntInit )?pL_PntInit(ppPaint):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTFREE(pPaint) \
      ((pL_PntFree )?pL_PntFree(pPaint):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTSETPROPERTY(pPaint,nGroup,pProperty) \
      ((pL_PntSetProperty )?pL_PntSetProperty(pPaint,nGroup,pProperty):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTGETPROPERTY(pPaint,nGroup,pProperty) \
      ((pL_PntGetProperty )?pL_PntGetProperty(pPaint,nGroup,pProperty):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTSETMETRICS(pPaint,UserDC,pBitmap,hRestrictionPalette) \
      ((pL_PntSetMetrics )?pL_PntSetMetrics(pPaint,UserDC,pBitmap,hRestrictionPalette):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTSETTRANSFORMATION(pPaint,pXForm) \
      ((pL_PntSetTransformation )?pL_PntSetTransformation(pPaint,pXForm):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTGETTRANSFORMATION(pPaint,pXForm) \
      ((pL_PntGetTransformation )?pL_PntGetTransformation(pPaint,pXForm):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTSETDCEXTENTS(pPaint,prcRect) \
      ((pL_PntSetDCExtents )?pL_PntSetDCExtents(pPaint,prcRect):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTGETDCEXTENTS(pPaint,prcRect) \
      ((pL_PntGetDCExtents )?pL_PntGetDCExtents(pPaint,prcRect):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTSETCLIPRGN(pPaint,hClipRng) \
      ((pL_PntSetClipRgn )?pL_PntSetClipRgn(pPaint,hClipRng):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTGETCLIPRGN(pPaint,phClipRng) \
      ((pL_PntGetClipRgn )?pL_PntGetClipRgn(pPaint,phClipRng):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTOFFSETCLIPRGN(pPaint,nDX,nDY) \
      ((pL_PntOffsetClipRgn )?pL_PntOffsetClipRgn(pPaint,nDX,nDY):WRPERR_LTPNT_DLL_NOT_LOADED)

// brush fucntions.
#define L_WRPPNTBRUSHMOVETO(pPaint,UserDC,nX,nY) \
      ((pL_PntBrushMoveTo )?pL_PntBrushMoveTo(pPaint,UserDC,nX,nY):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTBRUSHLINETO(pPaint,UserDC,nX,nY) \
      ((pL_PntBrushLineTo )?pL_PntBrushLineTo(pPaint,UserDC,nX,nY):WRPERR_LTPNT_DLL_NOT_LOADED)

// shape functions.
#define L_WRPPNTDRAWSHAPELINE(pPaint,UserDC,nXStart,nYStart,nEndX,nEndY) \
      ((pL_PntDrawShapeLine )?pL_PntDrawShapeLine(pPaint,UserDC,nXStart,nYStart,nEndX,nEndY):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTDRAWSHAPERECTANGLE(pPaint,UserDC,prcRect) \
      ((pL_PntDrawShapeRectangle )?pL_PntDrawShapeRectangle(pPaint,UserDC,prcRect):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTDRAWSHAPEROUNDRECT(pPaint,UserDC,prcRect) \
      ((pL_PntDrawShapeRoundRect )?pL_PntDrawShapeRoundRect(pPaint,UserDC,prcRect):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTDRAWSHAPEELLIPSE(pPaint,UserDC,prcRect) \
      ((pL_PntDrawShapeEllipse )?pL_PntDrawShapeEllipse(pPaint,UserDC,prcRect):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTDRAWSHAPEPOLYGON(pPaint,UserDC,pptPoints,nCount) \
      ((pL_PntDrawShapePolygon )?pL_PntDrawShapePolygon(pPaint,UserDC,pptPoints,nCount):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTDRAWSHAPEPOLYBEZIER(pPaint,UserDC,pptPoints,nCount) \
      ((pL_PntDrawShapePolyBezier )?pL_PntDrawShapePolyBezier(pPaint,UserDC,pptPoints,nCount):WRPERR_LTPNT_DLL_NOT_LOADED)

// region functions.
#define L_WRPPNTREGIONRECT(pPaint,UserDC,prcRect,phDestRgn) \
      ((pL_PntRegionRect )?pL_PntRegionRect(pPaint,UserDC,prcRect,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTREGIONROUNDRECT(pPaint,UserDC,prcRect,phDestRgn) \
      ((pL_PntRegionRoundRect )?pL_PntRegionRoundRect(pPaint,UserDC,prcRect,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTREGIONELLIPSE(pPaint,UserDC,prcRect,phDestRgn) \
      ((pL_PntRegionEllipse )?pL_PntRegionEllipse(pPaint,UserDC,prcRect,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTREGIONPOLYGON(pPaint,UserDC,pptPoints,nCount,phDestRgn) \
      ((pL_PntRegionPolygon )?pL_PntRegionPolygon(pPaint,UserDC,pptPoints,nCount,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTREGIONSURFACE(pPaint,UserDC,pptPoint,phDestRgn) \
      ((pL_PntRegionSurface )?pL_PntRegionSurface(pPaint,UserDC,pptPoint,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTREGIONBORDER(pPaint,UserDC,pptPoint,crBorderColor,phDestRgn) \
      ((pL_PntRegionBorder )?pL_PntRegionBorder(pPaint,UserDC,pptPoint,crBorderColor,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTREGIONCOLOR(pPaint,UserDC,crColor,phDestRgn) \
      ((pL_PntRegionColor )?pL_PntRegionColor(pPaint,UserDC,crColor,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTREGIONTRANSLATE(pPaint,dx,dy,phDestRgn) \
      ((pL_PntRegionTranslate )?pL_PntRegionTranslate(pPaint,dx,dy,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTREGIONSCALE(pPaint,nHScaleFactor,nVScaleFactor,nAlignment,phDestRgn) \
      ((pL_PntRegionScale )?pL_PntRegionScale(pPaint,nHScaleFactor,nVScaleFactor,nAlignment,phDestRgn):WRPERR_LTPNT_DLL_NOT_LOADED)

// fill functions.
#define L_WRPPNTFILLSURFACE(pPaint,UserDC,pptPoint) \
      ((pL_PntFillSurface )?pL_PntFillSurface(pPaint,UserDC,pptPoint):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTFILLBORDER(pPaint,UserDC,pptPoint,crBorderColor) \
      ((pL_PntFillBorder )?pL_PntFillBorder(pPaint,UserDC,pptPoint,crBorderColor):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTFILLCOLORREPLACE(pPaint,UserDC,crColor) \
      ((pL_PntFillColorReplace )?pL_PntFillColorReplace(pPaint,UserDC,crColor):WRPERR_LTPNT_DLL_NOT_LOADED)

// text functions.
#define L_WRPPNTAPPLYTEXT(pPaint,UserDC,prcRect) \
      ((pL_PntApplyText )?pL_PntApplyText(pPaint,UserDC,prcRect):WRPERR_LTPNT_DLL_NOT_LOADED)

// paint helping functions.
#define L_WRPPNTPICKCOLOR(pPaint, UserDC,nX,nY, pcrDestColor) \
      ((pL_PntPickColor )?pL_PntPickColor(pPaint, UserDC,nX,nY, pcrDestColor):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTSETCALLBACK(pPaint,pCallback,pUserData) \
      ((pL_PntSetCallback )?pL_PntSetCallback(pPaint,pCallback,pUserData):WRPERR_LTPNT_DLL_NOT_LOADED)

#define L_WRPPNTUPDATELEADDC(pPaint,pBitmap) \
      ((pL_PntUpdateLeadDC )?pL_PntUpdateLeadDC(pPaint,pBitmap):WRPERR_LTPNT_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTTLB.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
// general toolbar operations funtions.
#define L_TBISVALID(pToolbar) \
   ((pL_TBIsValid )?pL_TBIsValid(pToolbar):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBINIT(ppToolbar) \
   ((pL_TBInit )?pL_TBInit(ppToolbar):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBFREE(pToolbar) \
   ((pL_TBFree )?pL_TBFree(pToolbar):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBCREATE(pToolbar, hwndParent, szTitle, dwFlags) \
   ((pL_TBCreate )?pL_TBCreate(pToolbar, hwndParent, szTitle, dwFlags):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBFREETOOLBARINFO(pToolbar, pToolbarInfo) \
   ((pL_TBFreeToolbarInfo )?pL_TBFreeToolbarInfo(pToolbar, pToolbarInfo):WRPERR_LTTLB_DLL_NOT_LOADED)

// status query functions.
#define L_TBISVISIBLE(pToolbar, pfVisible) \
   ((pL_TBIsVisible )?pL_TBIsVisible(pToolbar, pfVisible):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBISBUTTONENABLED(pToolbar, uButtonID, pfEnable) \
   ((pL_TBIsButtonEnabled )?pL_TBIsButtonEnabled(pToolbar, uButtonID, pfEnable):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBISBUTTONVISIBLE(pToolbar, uButtonID, pfVisible) \
   ((pL_TBIsButtonVisible )?pL_TBIsButtonVisible(pToolbar, uButtonID, pfVisible):WRPERR_LTTLB_DLL_NOT_LOADED)

// setting functions.
#define L_TBSETVISIBLE(pToolbar, fVisible) \
   ((pL_TBSetVisible )?pL_TBSetVisible(pToolbar, fVisible):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETPOSITION(pToolbar, lpptPos, dwFlags) \
   ((pL_TBSetPosition )?pL_TBSetPosition(pToolbar, lpptPos, dwFlags):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETROWS(pToolbar, nRows) \
   ((pL_TBSetRows )?pL_TBSetRows(pToolbar, nRows):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETBUTTONCHECKED(pToolbar, uButtonID) \
   ((pL_TBSetButtonChecked )?pL_TBSetButtonChecked(pToolbar, uButtonID):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETBUTTONENABLED(pToolbar, uButtonID, fEnable) \
   ((pL_TBSetButtonEnabled )?pL_TBSetButtonEnabled(pToolbar, uButtonID, fEnable):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETBUTTONVISIBLE(pToolbar, uButtonID, fVisible) \
   ((pL_TBSetButtonVisible )?pL_TBSetButtonVisible(pToolbar, uButtonID, fVisible):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETTOOLBARINFO(pToolbar, pToolbarInfo) \
   ((pL_TBSetToolbarInfo )?pL_TBSetToolbarInfo(pToolbar, pToolbarInfo):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETCALLBACK(pToolbar, pCallback, pUserData) \
   ((pL_TBSetCallback )?pL_TBSetCallback(pToolbar, pCallback, pUserData):WRPERR_LTTLB_DLL_NOT_LOADED)

// getting functions.
#define L_TBGETPOSITION(pToolbar, lpptPos, dwFlags) \
   ((pL_TBGetPosition )?pL_TBGetPosition(pToolbar, lpptPos, dwFlags):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBGETROWS(pToolbar, pnRows) \
   ((pL_TBGetRows )?pL_TBGetRows(pToolbar, pnRows):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBGETBUTTONCHECKED(pToolbar, pnChecked) \
   ((pL_TBGetButtonChecked )?pL_TBGetButtonChecked(pToolbar, pnChecked):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBGETTOOLBARINFO(pToolbar, pToolbarInfo, uStructSize) \
   ((pL_TBGetToolbarInfo )?pL_TBGetToolbarInfo(pToolbar, pToolbarInfo, uStructSize):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBGETCALLBACK(pToolbar, ppCallback, ppUserData) \
   ((pL_TBGetCallback )?pL_TBGetCallback(pToolbar, ppCallback, ppUserData):WRPERR_LTTLB_DLL_NOT_LOADED)

// new functions
#define L_TBADDBUTTON(pToolbar, uButtonRefId, pButtonInfo, dwFlags) \
   ((pL_TBAddButton )?pL_TBAddButton(pToolbar, uButtonRefId, pButtonInfo, dwFlags):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBREMOVEBUTTON(pToolbar, uButtonId) \
   ((pL_TBRemoveButton )?pL_TBRemoveButton(pToolbar, uButtonId):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBGETBUTTONINFO(pToolbar, uButtonId, pButtonInfo, uStructSize) \
   ((pL_TBGetButtonInfo )?pL_TBGetButtonInfo(pToolbar, uButtonId, pButtonInfo, uStructSize):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETBUTTONINFO(pToolbar, uButtonId, pButtonInfo) \
   ((pL_TBSetButtonInfo )?pL_TBSetButtonInfo(pToolbar, uButtonId, pButtonInfo):WRPERR_LTTLB_DLL_NOT_LOADED)

#define L_TBSETAUTOMATIONCALLBACK(pToolbar, pAutomationCallback, pAutomationData) \
   ((pL_TBSetAutomationCallback )?pL_TBSetAutomationCallback(pToolbar, pAutomationCallback, pAutomationData):WRPERR_LTTLB_DLL_NOT_LOADED)


//-----------------------------------------------------------------------------
//--LTPDG.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPPNTDLGBRUSH( hWnd, pBrushDlgInfo )  \
   ((pL_PntDlgBrush )? pL_PntDlgBrush ( hWnd, pBrushDlgInfo ):WRPERR_LTPDG_DLL_NOT_LOADED)

#define L_WRPPNTDLGSHAPE( hWnd, pShapeDlgInfo )  \
   ((pL_PntDlgShape )? pL_PntDlgShape ( hWnd, pShapeDlgInfo ):WRPERR_LTPDG_DLL_NOT_LOADED)

#define L_WRPPNTDLGREGION( hWnd, pRegionDlgInfo)  \
   ((pL_PntDlgRegion)? pL_PntDlgRegion( hWnd, pRegionDlgInfo):WRPERR_LTPDG_DLL_NOT_LOADED)

#define L_WRPPNTDLGFILL( hWnd, pFillDlgInfo  )  \
   ((pL_PntDlgFill  )? pL_PntDlgFill  ( hWnd, pFillDlgInfo  ):WRPERR_LTPDG_DLL_NOT_LOADED)

#define L_WRPPNTDLGTEXT( hWnd, pTextDlgInfo  )  \
   ((pL_PntDlgText  )? pL_PntDlgText  ( hWnd, pTextDlgInfo  ):WRPERR_LTPDG_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTSGM.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------
// Initialize segmentation handle
#define L_WRPMRCSTARTBITMAPSEGMENTATION(phSegment,pBitmap,clrBackground,clrForeground) \
      ((pL_MrcStartBitmapSegmentation )?pL_MrcStartBitmapSegmentation(phSegment,pBitmap,clrBackground,clrForeground):WRPERR_LTSGM_DLL_NOT_LOADED)

// Free segmentation handle
#define L_WRPMRCSTOPBITMAPSEGMENTATION(hSegment) \
      ((pL_MrcStopBitmapSegmentation )?pL_MrcStopBitmapSegmentation(hSegment):WRPERR_LTSGM_DLL_NOT_LOADED)


// Break a bitmap into segments in other way
#define L_WRPMRCSEGMENTBITMAP(hSegment,pBitmap,pSegOption) \
      ((pL_MrcSegmentBitmap )?pL_MrcSegmentBitmap(hSegment,pBitmap,pSegOption):WRPERR_LTSGM_DLL_NOT_LOADED)

// Set a new segment in the segmentation handle 
#define L_WRPMRCCREATENEWSEGMENT(hSegment,pBitmap,pSegment) \
      ((pL_MrcCreateNewSegment )?pL_MrcCreateNewSegment(hSegment,pBitmap,pSegment):WRPERR_LTSGM_DLL_NOT_LOADED)

// Get all segments stored inside a segmentation handle
#define L_WRPMRCENUMSEGMENTS(hSegment,pEnumProc,pUserData,dwFlags) \
      ((pL_MrcEnumSegments )?pL_MrcEnumSegments(hSegment,pEnumProc,pUserData,dwFlags):WRPERR_LTSGM_DLL_NOT_LOADED)

// Update a certain segment in the segmentation handle 
#define L_WRPMRCSETSEGMENTDATA(hSegment,pBitmap,nSegId,pSegmentData) \
      ((pL_MrcSetSegmentData )?pL_MrcSetSegmentData(hSegment,pBitmap,nSegId,pSegmentData):WRPERR_LTSGM_DLL_NOT_LOADED)

#define L_WRPMRCDELETESEGMENT(hSegment,nSegId) \
      ((pL_MrcDeleteSegment )?pL_MrcDeleteSegment(hSegment,nSegId):WRPERR_LTSGM_DLL_NOT_LOADED)

#define L_WRPMRCCOMBINESEGMENTS(hSegment,nSegId1,nSegId2,uCombineFlags,uCombineFactor) \
      ((pL_MrcCombineSegments )?pL_MrcCombineSegments(hSegment,nSegId1,nSegId2,uCombineFlags,uCombineFactor):WRPERR_LTSGM_DLL_NOT_LOADED)

//------------Copy segmentation handle ---------------------------------------------//
#define L_WRPMRCCOPYSEGMENTATIONHANDLE(phSegmentDst,hSegmentSrc) \
      ((pL_MrcCopySegmentationHandle )?pL_MrcCopySegmentationHandle(phSegmentDst,hSegmentSrc):WRPERR_LTSGM_DLL_NOT_LOADED)

//------------Save a file as MRC ---------------------------------------------------//

#define L_WRPMRCSAVEBITMAP(hSegment,pBitmap,pCmpOption, pszFileName,pfnCallback,pUserData,nFormat,pSaveOptions) \
      ((pL_MrcSaveBitmap )?pL_MrcSaveBitmap(hSegment,pBitmap,pCmpOption, pszFileName,pfnCallback,pUserData,nFormat,pSaveOptions):WRPERR_LTSGM_DLL_NOT_LOADED)

#define L_WRPMRCSAVEBITMAPT44(hSegment,pBitmap,pCmpOption, pszFileName,pfnCallback,pUserData,nFormat,pSaveOptions) \
      ((pL_MrcSaveBitmapT44 )?pL_MrcSaveBitmapT44(hSegment,pBitmap,pCmpOption, pszFileName,pfnCallback,pUserData,nFormat,pSaveOptions):WRPERR_LTSGM_DLL_NOT_LOADED)

//------------Load an MRC file -----------------------------------------------------//

#define L_WRPMRCLOADBITMAP(pszFileName,pBitmap,uStructSize,nPageNo,pfnCallback,pUserData) \
      ((pL_MrcLoadBitmap )?pL_MrcLoadBitmap(pszFileName,pBitmap,uStructSize,nPageNo,pfnCallback,pUserData):WRPERR_LTSGM_DLL_NOT_LOADED)

#define L_WRPMRCGETPAGESCOUNT(pszFileName, pnPages) \
      ((pL_MrcGetPagesCount )?pL_MrcGetPagesCount(pszFileName, pnPages):WRPERR_LTSGM_DLL_NOT_LOADED)

//------------Load/Save segments to/from files -------------------------------------//
#define L_WRPMRCSAVESEGMENTATION(hSegment, pszFileName) \
      ((pL_MrcSaveSegmentation )?pL_MrcSaveSegmentation(hSegment, pszFileName):WRPERR_LTSGM_DLL_NOT_LOADED)

#define L_WRPMRCLOADSEGMENTATION(phSegment,pBitmap, pszFileName) \
      ((pL_MrcLoadSegmentation )?pL_MrcLoadSegmentation(phSegment,pBitmap, pszFileName):WRPERR_LTSGM_DLL_NOT_LOADED)

#define L_WRPMRCSAVEBITMAPLIST(hSegment,uhSegmentCount,hList,pCmpOption, pszFileName,nFormat) \
      ((pL_MrcSaveBitmapList)?pL_MrcSaveBitmapList(hSegment,uhSegmentCount,hList,pCmpOption, pszFileName,nFormat):WRPERR_LTSGM_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTZMV.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPWINDOWHASZOOMVIEW(hWnd) \
      ((pL_WindowHasZoomView )?pL_WindowHasZoomView(hWnd):WRPERR_LTZMV_DLL_NOT_LOADED)

#define L_WRPCREATEZOOMVIEW(hWnd, pBitmap, pZoomViewProps) \
      ((pL_CreateZoomView )?pL_CreateZoomView(hWnd, pBitmap, pZoomViewProps):WRPERR_LTZMV_DLL_NOT_LOADED)

#define L_WRPGETZOOMVIEWPROPS(hWnd, pZoomViewProps, uStructSize) \
      ((pL_GetZoomViewProps )?pL_GetZoomViewProps(hWnd, pZoomViewProps, uStructSize):WRPERR_LTZMV_DLL_NOT_LOADED)

#define L_WRPUPDATEZOOMVIEW(hWnd, pZoomViewProps) \
      ((pL_UpdateZoomView )?pL_UpdateZoomView(hWnd, pZoomViewProps):WRPERR_LTZMV_DLL_NOT_LOADED)

#define L_WRPDESTROYZOOMVIEW(hWnd, uIndex) \
      ((pL_DestroyZoomView )?pL_DestroyZoomView(hWnd, uIndex):WRPERR_LTZMV_DLL_NOT_LOADED)

#define L_WRPGETZOOMVIEWSCOUNT(hWnd, puCount) \
      ((pL_GetZoomViewsCount )?pL_GetZoomViewsCount(hWnd, puCount):WRPERR_LTZMV_DLL_NOT_LOADED)

#define L_WRPRENDERZOOMVIEW(hDC, hWnd) \
      ((pL_RenderZoomView )?pL_RenderZoomView(hDC, hWnd):WRPERR_LTZMV_DLL_NOT_LOADED)

#define L_WRPSTARTZOOMVIEWANNEDIT(hWnd, pZoomViewAnnEdit) \
      ((pL_StartZoomViewAnnEdit )?pL_StartZoomViewAnnEdit(hWnd, pZoomViewAnnEdit):WRPERR_LTZMV_DLL_NOT_LOADED)

#define L_WRPSTOPZOOMVIEWANNEDIT(hWnd) \
      ((pL_StopZoomViewAnnEdit )?pL_StopZoomViewAnnEdit(hWnd):WRPERR_LTZMV_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTIMGOPT.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

   #define L_WRPOPTGETDEFAULTOPTIONS(pOptImgOptions, uStructSize) \
      ((pL_OptGetDefaultOptions )?pL_OptGetDefaultOptions(pOptImgOptions, uStructSize):WRPERR_LTIMGOPT_DLL_NOT_LOADED)

   #define L_WRPOPTOPTIMIZEBUFFER(pOrgImgBuffer, uOrgImgBufferSize, phOptImgBuffer, puOptImgBufferSize, pOptImgOptions, pfnOptBufferCB, pUserData) \
      ((pL_OptOptimizeBuffer )?pL_OptOptimizeBuffer(pOrgImgBuffer, uOrgImgBufferSize, phOptImgBuffer, puOptImgBufferSize, pOptImgOptions, pfnOptBufferCB, pUserData):WRPERR_LTIMGOPT_DLL_NOT_LOADED)

   #define L_WRPOPTOPTIMIZEDIR(pszOrgDirPath, pszOptDirPath, pOptImgOptions, pszFilesExt, bIncludeSubDirs, pfnOptImgDirCB, pUserData) \
      ((pL_OptOptimizeDir )?pL_OptOptimizeDir(pszOrgDirPath, pszOptDirPath, pOptImgOptions, pszFilesExt, bIncludeSubDirs, pfnOptImgDirCB, pUserData):WRPERR_LTIMGOPT_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LPDFComp.H FUNCTIONS MACROS----------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPPDFCOMPINIT(phDocument,pCallback,pUserData) \
      ((pL_PdfCompInit )?pL_PdfCompInit(phDocument,pCallback,pUserData):WRPERR_LCMRC_DLL_NOT_LOADED)

#define L_WRPPDFCOMPFREE(hDocument) \
      ((pL_PdfCompFree )?pL_PdfCompFree(hDocument):WRPERR_LCMRC_DLL_NOT_LOADED)

#define L_WRPPDFCOMPWRITE(hDocument,pwszOutFile) \
      ((pL_PdfCompWrite )?pL_PdfCompWrite(hDocument,pwszOutFile):WRPERR_LCMRC_DLL_NOT_LOADED)

#define L_WRPPDFCOMPSETCOMPRESSION(hDocument,pCompression) \
      ((pL_PdfCompSetCompression )?pL_PdfCompSetCompression(hDocument,pCompression):WRPERR_LCMRC_DLL_NOT_LOADED)

#define L_WRPPDFCOMPINSERTMRC(hDocHandle,pBitmap,pPDFOptions) \
      ((pL_PdfCompInsertMRC )?pL_PdfCompInsertMRC(hDocHandle,pBitmap,pPDFOptions):WRPERR_LCMRC_DLL_NOT_LOADED)

#define L_WRPPDFCOMPINSERTNORMAL(hDocHandle,pBitmap) \
      ((pL_PdfCompInsertNormal )?pL_PdfCompInsertNormal(hDocHandle,pBitmap):WRPERR_LCMRC_DLL_NOT_LOADED)

#define L_WRPPDFCOMPINSERTSEGMENTS(hDocHandle,pBitmap,uSegmentCnt,pSegmentInfo,bIsThereBackGround,rgbBackGroundColor) \
      ((pL_PdfCompInsertSegments )?pL_PdfCompInsertSegments(hDocHandle,pBitmap,uSegmentCnt,pSegmentInfo,bIsThereBackGround,rgbBackGroundColor):WRPERR_LCMRC_DLL_NOT_LOADED)

//--LTIVW.H FUNCTIONS MACROS-----------------------------------------------
//-----------------------------------------------------------------------------
#define L_WRPDISPCONTAINERCREATE(hWndParent, lpRect, uFlags) \
   ((pL_DispContainerCreate) ? pL_DispContainerCreate(hWndParent, lpRect, uFlags) : (LBase::RecordError(WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED),(HWND)0))

#define L_WRPDISPCONTAINERGETWINDOWHANDLE(hCon, uFlags)\
   ((pL_DispContainerGetWindowHandle)? pL_DispContainerGetWindowHandle(hCon, uFlags): (LBase::RecordError(WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED),(HWND)0))

#define L_WRPDISPCONTAINERDESTROY(hCon , bCleanImages, uFlags)\
   ((pL_DispContainerDestroy)? pL_DispContainerDestroy(hCon , bCleanImages, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETPROPERTIES(hCon, pDispContainerProp, uFlags)\
   ((pL_DispContainerSetProperties )? pL_DispContainerSetProperties (hCon, pDispContainerProp, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETPROPERTIES(hCon, pDispContainerProp, uFlags)\
   ((pL_DispContainerGetProperties)? pL_DispContainerGetProperties(hCon, pDispContainerProp, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERINSERTCELL(hCon, nIndex, uFlags)\
   ((pL_DispContainerInsertCell)? pL_DispContainerInsertCell(hCon, nIndex, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERREMOVECELL(hCon, nCellIndex, bCleanImages ,uFlags)\
   ((pL_DispContainerRemoveCell)? pL_DispContainerRemoveCell(hCon, nCellIndex, bCleanImages ,uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLCOUNT(hCon, uFlags)\
   ((pL_DispContainerGetCellCount)? pL_DispContainerGetCellCount(hCon, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLWINDOWHANDLE(hCon, nCellIndex, uFlags)\
   ((pL_DispContainerGetCellWindowHandle)? pL_DispContainerGetCellWindowHandle(hCon, nCellIndex, uFlags):(LBase::RecordError(WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED),(HWND)0))

#define L_WRPDISPCONTAINERSETCELLBITMAPLIST(hCon, nCellIndex, hBitmapList, bCleanImages, uFlags)\
   ((pL_DispContainerSetCellBitmapList)? pL_DispContainerSetCellBitmapList(hCon, nCellIndex, hBitmapList, bCleanImages, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERADDACTION(hCon, nAction, uFlags)\
   ((pL_DispContainerAddAction)? pL_DispContainerAddAction(hCon, nAction, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETACTION(hCon, nAction, nMouseButton, uFlags)\
   ((pL_DispContainerSetAction)? pL_DispContainerSetAction(hCon, nAction, nMouseButton, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETCELLTAG(hCon, nCellIndex, uRow, uAlign, uType, pString, uFlags)\
   ((pL_DispContainerSetCellTag)? pL_DispContainerSetCellTag(hCon, nCellIndex, uRow, uAlign, uType, pString, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETCELLPROPERTIES(hCon, nCellIndex, pCellProperties, uFlags)\
   ((pL_DispContainerSetCellProperties)? pL_DispContainerSetCellProperties(hCon, nCellIndex, pCellProperties, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLPROPERTIES(hCon, nCellIndex, pCellProperties, uFlags)\
   ((pL_DispContainerGetCellProperties)? pL_DispContainerGetCellProperties(hCon, nCellIndex, pCellProperties, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLPOSITION(hCon, nCellIndex, puRow, puCol, uFlags)\
   ((pL_DispContainerGetCellPosition)? pL_DispContainerGetCellPosition(hCon, nCellIndex, puRow, puCol, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERREPOSITIONCELL(hCon, nCellIndex, nTargetIndex, bSwap, uFlags)\
   ((pL_DispContainerRepositionCell )? pL_DispContainerRepositionCell(hCon, nCellIndex, nTargetIndex, bSwap, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLBITMAPLIST(hCon, nCellIndex, phBitmapList, uFlags)\
   ((pL_DispContainerGetCellBitmapList)? pL_DispContainerGetCellBitmapList(hCon, nCellIndex, phBitmapList, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLBOUNDS(hCon, nCellIndex, lpRect, Flags)\
   ((pL_DispContainerGetCellBounds)? pL_DispContainerGetCellBounds(hCon, nCellIndex, lpRect, Flags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERFREEZECELL(hCon, nCellIndex, bFreeze, uFlags)\
   ((pL_DispContainerFreezeCell)? pL_DispContainerFreezeCell(hCon, nCellIndex, bFreeze, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETFIRSTVISIBLEROW(hCon, uRow, uFlags)\
   ((pL_DispContainerSetFirstVisibleRow)? pL_DispContainerSetFirstVisibleRow(hCon, uRow, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETFIRSTVISIBLEROW(hCon, puRow, uFlags)\
   ((pL_DispContainerGetFirstVisibleRow)? pL_DispContainerGetFirstVisibleRow(hCon, puRow, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETACTIONPROPERTIES(hCon, nAction, nCellIndex, nSubCellIndex, pActionProperties, uFlags)\
   ((pL_DispContainerSetActionProperties)? pL_DispContainerSetActionProperties(hCon, nAction, nCellIndex, nSubCellIndex, pActionProperties, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETACTIONPROPERTIES(hCon, nAction, nCellIndex, nSubCellIndex, pActionProperties, uFlags)\
   ((pL_DispContainerGetActionProperties)? pL_DispContainerGetActionProperties(hCon, nAction, nCellIndex, nSubCellIndex, pActionProperties, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERREMOVEACTION(hCon, nAction, uFlags)\
   ((pL_DispContainerRemoveAction)? pL_DispContainerRemoveAction(hCon, nAction, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETACTIONCOUNT(hCon, pnCount, uFlags)\
   ((pL_DispContainerGetActionCount)? pL_DispContainerGetActionCount(hCon, pnCount, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETKEYBOARDACTION(hCon, nAction, nButton, uKey, uFlags)\
   ((pL_DispContainerSetKeyboardAction)? pL_DispContainerSetKeyboardAction(hCon, nAction, nButton, uKey, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETBOUNDS(hCon, lpRect, uFlags)\
   ((pL_DispContainerSetBounds)? pL_DispContainerSetBounds(hCon, lpRect, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETBOUNDS(hCon, lpRect, uFlags)\
   ((pL_DispContainerGetBounds)? pL_DispContainerGetBounds(hCon, lpRect, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSELECTCELL(hCon, nCellIndex, bSelect, uFlags)\
   ((pL_DispContainerSelectCell)? pL_DispContainerSelectCell(hCon, nCellIndex, bSelect, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISCELLSELECTED(hCon, nCellIndex, uFlags)\
   ((pL_DispContainerIsCellSelected)? pL_DispContainerIsCellSelected(hCon, nCellIndex, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETHANDLE(hConWnd)\
   ((pL_DispContainerGetHandle)? pL_DispContainerGetHandle(hConWnd):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETKEYBOARDACTION(hCon, nAction, nMouseDirection, puVk, puModifiers, uFlags)\
   ((pL_DispContainerGetKeyboardAction)? pL_DispContainerGetKeyboardAction(hCon, nAction, nMouseDirection, puVk, puModifiers, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISCELLFROZEN(hCon, nCellIndex, uFlags)\
   ((pL_DispContainerIsCellFrozen)? pL_DispContainerIsCellFrozen(hCon, nCellIndex, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISACTIONACTIVE(hCon, nAction, uFlags)\
   ((pL_DispContainerIsActionActive)? pL_DispContainerIsActionActive(hCon, nAction, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETTAGCALLBACK(hCon, pfnCallBack, pUserData)\
   ((pL_DispContainerSetTagCallBack)? pL_DispContainerSetTagCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETACTIONCALLBACK(hCon, pfnCallBack, uFlags)\
   ((pL_DispContainerSetActionCallBack)? pL_DispContainerSetActionCallBack(hCon, pfnCallBack, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETTAGCALLBACK(hCon, ppfnCallBack, uFlags)\
   ((pL_DispContainerGetTagCallBack)? pL_DispContainerGetTagCallBack(hCon, ppfnCallBack, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETACTIONCALLBACK(hCon, ppfnCallBack, ppUserData)\
   ((pL_DispContainerGetActionCallBack)? pL_DispContainerGetActionCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETPAINTCALLBACK(hCon, pfnPaintCallBack, pUserData) \
   ((pL_DispContainerSetPaintCallBack )?pL_DispContainerSetPaintCallBack(hCon, pfnPaintCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETPAINTCALLBACK(hCon, ppfnPaintCallBack, ppUserData) \
   ((pL_DispContainerGetPaintCallBack )?pL_DispContainerGetPaintCallBack(hCon, ppfnPaintCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERREPAINTCELL(hCon, hWnd, nCellIndex, uFlags) \
   ((pL_DispContainerRepaintCell )?pL_DispContainerRepaintCell(hCon, hWnd, nCellIndex, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETBITMAPHANDLE(hCon, nCellIndex, nSubCellIndex, pBitmap, uFlags) \
   ((pL_DispContainerGetBitmapHandle )?pL_DispContainerGetBitmapHandle(hCon, nCellIndex, nSubCellIndex, pBitmap, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETBITMAPHANDLE(hCon, nCellIndex, nSubCellIndex, pBitmap, bRepaint, uFlags) \
   ((pL_DispContainerSetBitmapHandle )?pL_DispContainerSetBitmapHandle(hCon, nCellIndex, nSubCellIndex, pBitmap, bRepaint, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETRULERUNIT(hCon, uUnit, uFlags) \
   ((pL_DispContainerSetRulerUnit )?pL_DispContainerSetRulerUnit(hCon, uUnit, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERCALIBRATERULER(hCon, nCellIndex, nSubCellIndex, dLength, uUnit, uFlags) \
   ((pL_DispContainerCalibrateRuler )?pL_DispContainerCalibrateRuler(hCon, nCellIndex, nSubCellIndex, dLength, uUnit, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETACTIONBUTTON(hCon, nAction, pnMouseButton, puFlags) \
   ((pL_DispContainerGetActionButton )?pL_DispContainerGetActionButton(hCon, nAction, pnMouseButton, puFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERANNTORGN(hCon, nCellIndex, nSubCellIndex, uCombineMode, bDeleteAnn, uFlags) \
   ((pL_DispContainerAnnToRgn )?pL_DispContainerAnnToRgn(hCon, nCellIndex, nSubCellIndex, uCombineMode, bDeleteAnn, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISBUTTONVALID(hCon, nAction, nMouseButton, uFlags) \
   ((pL_DispContainerIsButtonValid )?pL_DispContainerIsButtonValid(hCon, nAction, nMouseButton, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSTARTANIMATION(hCon, nCellIndex, nStartFrame, nFrameCount, bAnimateAllSubCells, uFlags) \
   ((pL_DispContainerStartAnimation )?pL_DispContainerStartAnimation(hCon, nCellIndex, nStartFrame, nFrameCount, bAnimateAllSubCells, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETANIMATIONPROPERTIES(hCon, nCellIndex, pDisAnimationProps, uFlags) \
   ((pL_DispContainerSetAnimationProperties )?pL_DispContainerSetAnimationProperties(hCon, nCellIndex, pDisAnimationProps, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETANIMATIONPROPERTIES(hCon, nCellIndex, pDisAnimationProps, uFlags) \
   ((pL_DispContainerGetAnimationProperties )?pL_DispContainerGetAnimationProperties(hCon, nCellIndex, pDisAnimationProps, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSTOPANIMATION(hCon, nCellIndex, uFlags) \
   ((pL_DispContainerStopAnimation )?pL_DispContainerStopAnimation(hCon, nCellIndex, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSHOWTITLEBAR(hCon, uShow, uFlags) \
   ((pL_DispContainerShowTitlebar )?pL_DispContainerShowTitlebar(hCon, uShow, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETTITLEBARPROPERTIES(hCon, pDispContainerTitlebarProps, uFlags) \
   ((pL_DispContainerSetTitlebarProperties )?pL_DispContainerSetTitlebarProperties(hCon, pDispContainerTitlebarProps, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETTITLEBARPROPERTIES(hCon, pDispContainerTitlebarProps, uFlags) \
   ((pL_DispContainerGetTitlebarProperties )?pL_DispContainerGetTitlebarProperties(hCon, pDispContainerTitlebarProps, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETICONPROPERTIES(hCon, nIconIndex, pDispContainerIconProps, uFlags) \
   ((pL_DispContainerSetIconProperties )?pL_DispContainerSetIconProperties(hCon, nIconIndex, pDispContainerIconProps, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETICONPROPERTIES(hCon, nIconIndex, pDispContainerIconProps, uFlags) \
   ((pL_DispContainerGetIconProperties )?pL_DispContainerGetIconProperties(hCon, nIconIndex, pDispContainerIconProps, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERCHECKTITLEBARICON(hCon, nCellIndex, nSubCellIndex,nIconIndex, bCheck, uFlags) \
   ((pL_DispContainerCheckTitlebarIcon )?pL_DispContainerCheckTitlebarIcon(hCon, nCellIndex, nSubCellIndex,nIconIndex, bCheck, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISTITLEBARICONCHECKED(hCon, nCellIndex, nSubCellIndex, nIconIndex, uFlags) \
   ((pL_DispContainerIsTitlebarIconChecked )?pL_DispContainerIsTitlebarIconChecked(hCon, nCellIndex, nSubCellIndex, nIconIndex, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISCELLANIMATED(hCon, nCellIndex, uFlags) \
   ((pL_DispContainerIsCellAnimated )?pL_DispContainerIsCellAnimated(hCon, nCellIndex, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

// This function retrieves the viewer ruler unit
#define L_WRPDISPCONTAINERGETRULERUNIT(hCon, puUnit, uFlags) \
   ((pL_DispContainerGetRulerUnit )?pL_DispContainerGetRulerUnit(hCon, puUnit, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISTITLEBARENABLED(hCon, uFlags) \
   ((pL_DispContainerIsTitlebarEnabled )?pL_DispContainerIsTitlebarEnabled(hCon, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETSELECTEDANNOTATIONATTRIBUTES(hCon, nCellIndex, nSubCellIndex, pAnnAttributes, uFlags) \
  ((pL_DispContainerGetSelectedAnnotationAttributes )?pL_DispContainerGetSelectedAnnotationAttributes(hCon, nCellIndex, nSubCellIndex, pAnnAttributes, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)


#define L_WRPDISPCONTAINERHANDLEPALETTE(hCon, uMessage, wParam, uFlags) \
  ((pL_DispContainerHandlePalette)?pL_DispContainerHandlePalette(hCon, uMessage, wParam, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERUPDATECELLVIEW(hCon, hWnd, nCellIndex, uFlags) \
   ((pL_DispContainerUpdateCellView)? pL_DispContainerUpdateCellView(hCon, hWnd, nCellIndex, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

//New Medical Viewer Functionalities

#define L_WRPDISPCONTAINERFLIPANNOTATIONCONTAINER(hCon, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerFlipAnnotationContainer)?pL_DispContainerFlipAnnotationContainer(hCon, nCellIndex,nSubCellIndex,uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERREVERSEANNOTATIONCONTAINER(hCon, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerReverseAnnotationContainer)?pL_DispContainerReverseAnnotationContainer(hCon, nCellIndex,nSubCellIndex,uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERROTATEANNOTATIONCONTAINER(hCon, nCellIndex, nSubCellIndex, dAngle, uFlags) \
   ((pL_DispContainerRotateAnnotationContainer )?pL_DispContainerRotateAnnotationContainer(hCon, nCellIndex, nSubCellIndex, dAngle, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETANNOTATIONCALLBACK(hCon, pfnCallBack, pUserData) \
   ((pL_DispContainerSetAnnotationCallBack )?pL_DispContainerSetAnnotationCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETANNOTATIONCALLBACK(hCon, ppfnCallBack, ppUserData) \
   ((pL_DispContainerGetAnnotationCallBack )?pL_DispContainerGetAnnotationCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETREGIONCALLBACK(hCon, pfnCallBack, pUserData) \
   ((pL_DispContainerSetRegionCallBack )?pL_DispContainerSetRegionCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETREGIONCALLBACK(hCon, ppfnCallBack, ppUserData) \
   ((pL_DispContainerGetRegionCallBack )?pL_DispContainerGetRegionCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETANNOTATIONCREATEDCALLBACK(hCon, pfnCallBack, pUserData) \
   ((pL_DispContainerSetAnnotationCreatedCallBack )?pL_DispContainerSetAnnotationCreatedCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETANNOTATIONCREATEDCALLBACK(hCon, ppfnCallBack, ppUserData) \
   ((pL_DispContainerGetAnnotationCreatedCallBack )?pL_DispContainerGetAnnotationCreatedCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETACTIVESUBCELLCHANGEDCALLBACK(hCon, pfnCallBack, pUserData) \
   ((pL_DispContainerSetActiveSubCellChangedCallBack )?pL_DispContainerSetActiveSubCellChangedCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETACTIVESUBCELLCHANGEDCALLBACK(hCon, ppfnCallBack, ppUserData) \
   ((pL_DispContainerGetActiveSubCellChangedCallBack )?pL_DispContainerGetActiveSubCellChangedCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSAVEANNOTATION(hCon, pFileName, nCellIndex, nSubCellIndex, nStartPage, uFlags) \
   ((pL_DispContainerSaveAnnotation )?pL_DispContainerSaveAnnotation(hCon, pFileName, nCellIndex, nSubCellIndex, nStartPage, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERLOADANNOTATION(hCon, pFileName, nCellIndex, nSubCellIndex, nStartPage, uFlags) \
   ((pL_DispContainerLoadAnnotation )?pL_DispContainerLoadAnnotation(hCon, pFileName, nCellIndex, nSubCellIndex, nStartPage, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETBITMAPPIXEL(hCon, nCellIndex, nSubCellIndex, pSrcPoint, pBitmapPoint, uFlags) \
   ((pL_DispContainerGetBitmapPixel )?pL_DispContainerGetBitmapPixel(hCon, nCellIndex, nSubCellIndex, pSrcPoint, pBitmapPoint, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERLOADREGION(hCon, pFileName, nCellIndex, nSubCellIndex, nStartPage, uFlags) \
   ((pL_DispContainerLoadRegion )?pL_DispContainerLoadRegion(hCon, pFileName, nCellIndex, nSubCellIndex, nStartPage, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSAVEREGION(hCon, pFileName, nCellIndex, nSubCellIndex, nStartPage, uFlags) \
   ((pL_DispContainerSaveRegion )?pL_DispContainerSaveRegion(hCon, pFileName, nCellIndex, nSubCellIndex, nStartPage, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETMOUSECALLBACK(hCon, pfnCallBack, pUserData)\
   ((pL_DispContainerSetMouseCallBack)?pL_DispContainerSetMouseCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETMOUSECALLBACK(hCon, ppfnCallBack, ppUserData)\
   ((pL_DispContainerGetMouseCallBack)?pL_DispContainerGetMouseCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)


#define L_WRPDISPCONTAINERSETPREPAINTCALLBACK(hCon, pfnCallBack, pUserData)\
   ((pL_DispContainerSetPrePaintCallBack)?pL_DispContainerSetPrePaintCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETPOSTPAINTCALLBACK(hCon, pfnCallBack, pUserData)\
   ((pL_DispContainerSetPostPaintCallBack)?pL_DispContainerSetPostPaintCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETLOWMEMORYUSAGECALLBACK(hCon, pfnCallBack, pUserData)\
   ((pL_DispContainerSetLowMemoryUsageCallBack)?pL_DispContainerSetLowMemoryUsageCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETANIMATIONSTARTEDCALLBACK(hCon, pfnCallBack, pUserData)\
   ((pL_DispContainerSetAnimationStartedCallBack)?pL_DispContainerSetAnimationStartedCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETANIMATIONSTOPPEDCALLBACK(hCon, pfnCallBack, pUserData)\
   ((pL_DispContainerSetAnimationStoppedCallBack)?pL_DispContainerSetAnimationStoppedCallBack(hCon, pfnCallBack, pUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETPREPAINTCALLBACK(hCon, ppfnCallBack, ppUserData)\
   ((pL_DispContainerGetPrePaintCallBack)?pL_DispContainerGetPrePaintCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETPOSTPAINTCALLBACK(hCon, ppfnCallBack, ppUserData)\
   ((pL_DispContainerGetPostPaintCallBack)?pL_DispContainerGetPostPaintCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETLOWMEMORYUSAGECALLBACK(hCon, ppfnCallBack, ppUserData)\
   ((pL_DispContainerGetLowMemoryUsageCallBack)?pL_DispContainerGetLowMemoryUsageCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETANIMATIONSTARTEDCALLBACK(hCon, ppfnCallBack, ppUserData)\
   ((pL_DispContainerGetAnimationStartedCallBack)?pL_DispContainerGetAnimationStartedCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETANIMATIONSTOPPEDCALLBACK(hCon, ppfnCallBack, ppUserData)\
   ((pL_DispContainerGetAnimationStoppedCallBack)?pL_DispContainerGetAnimationStoppedCallBack(hCon, ppfnCallBack, ppUserData):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLREGIONHANDLE(m_hDispContainer, nCellIndex, nSubCellIndex, phRgn, uFlags)\
   ((pL_DispContainerGetCellRegionHandle)?pL_DispContainerGetCellRegionHandle(m_hDispContainer, nCellIndex, nSubCellIndex, phRgn, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETANNOTATIONCONTAINER(m_hDispContainer, nCellIndex, nSubCellIndex, PhAnnContainer, uFlags)\
   ((pL_DispContainerGetAnnotationContainer)?pL_DispContainerGetAnnotationContainer(m_hDispContainer, nCellIndex, nSubCellIndex, PhAnnContainer, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETANNOTATIONCONTAINER(m_hDispContainer, nCellIndex, nSubCellIndex, hAnnContainer, uFlags)\
   ((pL_DispContainerSetAnnotationContainer)?pL_DispContainerSetAnnotationContainer(m_hDispContainer, nCellIndex, nSubCellIndex, hAnnContainer, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETSUBCELLTAG(m_hDispContainer, nCellIndex, nSubCellIndex, uRow, uAlign, uType, pString, uFlags)\
   ((pL_DispContainerSetSubCellTag)?pL_DispContainerSetSubCellTag(m_hDispContainer, nCellIndex, nSubCellIndex, uRow, uAlign, uType, pString, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETSUBCELLTAG(m_hDispContainer, nCellIndex, nSubCellIndex, uRow,uAlign, pTagInfo, uFlags)\
   ((pL_DispContainerGetSubCellTag)?pL_DispContainerGetSubCellTag(m_hDispContainer, nCellIndex, nSubCellIndex, uRow,uAlign, pTagInfo, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINEREDITSUBCELLTAG( m_hDispContainer, nCellIndex, nSubCellIndex, uRow, uAlign, pTagInfo, uFlags)\
   ((pL_DispContainerEditSubCellTag)?pL_DispContainerEditSubCellTag( m_hDispContainer, nCellIndex, nSubCellIndex, uRow, uAlign, pTagInfo, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERDELETESUBCELLTAG(m_hDispContainer, nCellIndex, nSubCellIndex, uRow, uAlign, uFlags)\
   ((pL_DispContainerDeleteSubCellTag)?pL_DispContainerDeleteSubCellTag(m_hDispContainer, nCellIndex, nSubCellIndex, uRow, uAlign, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLTAG(m_hDispContainer, nCellIndex, uRow, uAlign, pTagInfo, uFlags)\
   ((pL_DispContainerGetCellTag)?pL_DispContainerGetCellTag(m_hDispContainer, nCellIndex, uRow, uAlign, pTagInfo, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERDELETECELLTAG(m_hDispContainer, nCellIndex, uRow, uAlign, uFlags)\
   ((pL_DispContainerDeleteCellTag)?pL_DispContainerDeleteCellTag(m_hDispContainer, nCellIndex, uRow, uAlign, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINEREDITCELLTAG(m_hDispContainer, nCellIndex, uRow, uAlign, pTagInfo, uFlags)\
   ((pL_DispContainerEditCellTag)?pL_DispContainerEditCellTag(m_hDispContainer, nCellIndex, uRow, uAlign, pTagInfo, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERPRINTCELL(m_hDispContainer, nCellIndex, uFlags)\
   ((pL_DispContainerPrintCell)?pL_DispContainerPrintCell(m_hDispContainer, nCellIndex, uFlags): (LBase::RecordError(WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED),(HBITMAP)0))

#define L_WRPDISPCONTAINERENABLECELLLOWMEMORYUSAGE(m_hDispContainer, nCellIndex, nHiddenCount, nFrameCount, pBitmapInfo, uFlags)\
   ((pL_DispContainerEnableCellLowMemoryUsage)?pL_DispContainerEnableCellLowMemoryUsage(m_hDispContainer, nCellIndex, nHiddenCount, nFrameCount, pBitmapInfo, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETREQUESTEDIMAGE(m_hDispContainer, nCellIndex, pBitmaps, pBitmapIndexes, nLength, uFlags)\
   ((pL_DispContainerSetRequestedImage)?pL_DispContainerSetRequestedImage(m_hDispContainer, nCellIndex, pBitmaps, pBitmapIndexes, nLength, uFlags):WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETCELLREGIONHANDLE(m_hDispContainer, nCellIndex, nSubCellIndex, hRgn, uCombineMode, uFlags)\
   ((pL_DispContainerSetCellRegionHandle)? pL_DispContainerSetCellRegionHandle(m_hDispContainer, nCellIndex, nSubCellIndex, hRgn, uCombineMode, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERFREEZESUBCELL(m_hDispContainer, nCellIndex, nSubCellIndex, bFreeze, uFlags)\
   ((pL_DispContainerFreezeSubCell)? pL_DispContainerFreezeSubCell(m_hDispContainer, nCellIndex, nSubCellIndex, bFreeze, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISSUBCELLFROZEN(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerIsSubCellFrozen)? pL_DispContainerIsSubCellFrozen(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLSCALEMODE(m_hDispContainer, nCellIndex, nSubCellIndex, puScaleMode, uFlags)\
   ((pL_DispContainerGetCellScaleMode)? pL_DispContainerGetCellScaleMode(m_hDispContainer, nCellIndex, nSubCellIndex, puScaleMode, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETCELLSCALE(m_hDispContainer, nCellIndex, nSubCellIndex, dScale, uFlags)\
   ((pL_DispContainerSetCellScale)? pL_DispContainerSetCellScale(m_hDispContainer, nCellIndex, nSubCellIndex, dScale, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETCELLSCALEMODE(m_hDispContainer, nCellIndex, nSubCellIndex, uScaleMode, uFlags)\
   ((pL_DispContainerSetCellScaleMode)? pL_DispContainerSetCellScaleMode(m_hDispContainer, nCellIndex, nSubCellIndex, uScaleMode, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETCELLSCALE(m_hDispContainer, nCellIndex, nSubCellIndex, pdScale, uFlags)\
   ((pL_DispContainerGetCellScale)? pL_DispContainerGetCellScale(m_hDispContainer, nCellIndex, nSubCellIndex, pdScale, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERPRINTSUBCELL(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerPrintSubCell)? pL_DispContainerPrintSubCell(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) : (LBase::RecordError(WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED),(HBITMAP)0))

#define L_WRPDISPCONTAINERCALIBRATECELL(m_hDispContainer, nCellIndex, nSubCellIndex, dLength, uUnit, dTargetLength, uTargetUnit, uFlags)\
   ((pL_DispContainerCalibrateCell)? pL_DispContainerCalibrateCell(m_hDispContainer, nCellIndex, nSubCellIndex, dLength, uUnit, dTargetLength, uTargetUnit, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETBITMAPLISTINFO(m_hDispContainer, nCellIndex, nSubCellIndex, pBitmapInfo, uFlags)\
   ((pL_DispContainerSetBitmapListInfo)? pL_DispContainerSetBitmapListInfo(m_hDispContainer, nCellIndex, nSubCellIndex, pBitmapInfo, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERSETDEFAULTWINDOWLEVELVALUES(m_hDispContainer, nCellIndex, nSubCellIndex, nWidth, nCenter, uFlags)\
   ((pL_DispContainerSetDefaultWindowLevelValues)? pL_DispContainerSetDefaultWindowLevelValues(m_hDispContainer, nCellIndex, nSubCellIndex, nWidth, nCenter, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETDEFAULTWINDOWLEVELVALUES(m_hDispContainer, nCellIndex, nSubCellIndex, pnWidth, pnCenter, uFlags)\
   ((pL_DispContainerGetDefaultWindowLevelValues)? pL_DispContainerGetDefaultWindowLevelValues(m_hDispContainer, nCellIndex, nSubCellIndex, pnWidth, pnCenter, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERRESETWINDOWLEVELVALUES(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerResetWindowLevelValues)? pL_DispContainerResetWindowLevelValues(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERINVERTBITMAP(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerInvertBitmap)? pL_DispContainerInvertBitmap(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERREVERSEBITMAP(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerReverseBitmap) ? pL_DispContainerReverseBitmap(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) : WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERFLIPBITMAP(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerFlipBitmap) ? pL_DispContainerFlipBitmap(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) : WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISBITMAPREVERSED(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerIsBitmapReversed) ? pL_DispContainerIsBitmapReversed(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) : WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISBITMAPFLIPPED(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerIsBitmapFlipped) ? pL_DispContainerIsBitmapFlipped(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) : WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERISBITMAPINVERTED(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerIsBitmapInverted)? pL_DispContainerIsBitmapInverted(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERROTATEBITMAPPERSPECTIVE(m_hDispContainer, nCellIndex, nSubCellIndex, nAngle, uFlags)\
   ((pL_DispContainerRotateBitmapPerspective)? pL_DispContainerRotateBitmapPerspective(m_hDispContainer, nCellIndex, nSubCellIndex, nAngle, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERGETROTATEBITMAPPERSPECTIVEANGLE(m_hDispContainer, nCellIndex, nSubCellIndex, pnAngle, uFlags)\
   ((pL_DispContainerGetRotateBitmapPerspectiveAngle)? pL_DispContainerGetRotateBitmapPerspectiveAngle(m_hDispContainer, nCellIndex, nSubCellIndex, pnAngle, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERREMOVEBITMAPREGION(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags)\
   ((pL_DispContainerRemoveBitmapRegion)? pL_DispContainerRemoveBitmapRegion(m_hDispContainer, nCellIndex, nSubCellIndex, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERBEGINUPDATE(m_hDispContainer, uFlags)\
   ((pL_DispContainerBeginUpdate)? pL_DispContainerBeginUpdate(m_hDispContainer, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)

#define L_WRPDISPCONTAINERENDUPDATE(m_hDispContainer, uFlags)\
   ((pL_DispContainerEndUpdate)? pL_DispContainerEndUpdate(m_hDispContainer, uFlags) :WRPERR_IMAGEVIEWERCONTROL_DLL_NOT_LOADED)



//-----------------------------------------------------------------------------
//--LTCLR.H FUNCTIONS MACROS------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPGETPARAMETRICCURVENUMBEROFPARAMETERS(enFunctionType) \
   ((pL_GetParametricCurveNumberOfParameters )?pL_GetParametricCurveNumberOfParameters(enFunctionType):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRINIT(pClrHandle, nSrcFormat, nDstFormat, pParams) \
   ((pL_ClrInit )?pL_ClrInit(pClrHandle, nSrcFormat, nDstFormat, pParams):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRSETCONVERSIONPARAMS(ClrHandle, pParams) \
   ((pL_ClrSetConversionParams )?pL_ClrSetConversionParams(ClrHandle, pParams):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRCONVERTDIRECT(nSrcFormat, nDstFormat, pSrcBuf, pDstBuf, nWidth, nHeight, nInAlign, nOutAlign) \
   ((pL_ClrConvertDirect )?pL_ClrConvertDirect(nSrcFormat, nDstFormat, pSrcBuf, pDstBuf, nWidth, nHeight, nInAlign, nOutAlign):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRCONVERTDIRECTTOBITMAP(nSrcFormat, nDstFormat, pSrcBuf, pBitmap, uStructSize, nWidth, nHeight, nInAlign, nOutAlign) \
   ((pL_ClrConvertDirectToBitmap )?pL_ClrConvertDirectToBitmap(nSrcFormat, nDstFormat, pSrcBuf, pBitmap, uStructSize, nWidth, nHeight, nInAlign, nOutAlign):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRCONVERT(ClrHandle, pSrc, pDst, nWidth, nHeight, nInAlign, nOutAlign) \
   ((pL_ClrConvert )?pL_ClrConvert(ClrHandle, pSrc, pDst, nWidth, nHeight, nInAlign, nOutAlign):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRCONVERTTOBITMAP(ClrHandle, pSrcBuf, pBitmap, uStructSize, nWidth, nHeight, nInAlign, nOutAlign) \
   ((pL_ClrConvertToBitmap )?pL_ClrConvertToBitmap(ClrHandle, pSrcBuf, pBitmap, uStructSize, nWidth, nHeight, nInAlign, nOutAlign):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRFREE(ClrHandle) \
   ((pL_ClrFree )?pL_ClrFree(ClrHandle):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRISVALID(ClrHandle) \
   ((pL_ClrIsValid )?pL_ClrIsValid(ClrHandle):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCLRDLG(nDlg, hWnd, pClrHandle, pParams) \
   ((pL_ClrDlg )?pL_ClrDlg(nDlg, hWnd, pClrHandle, pParams):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPFILLICCPROFILESTRUCTURE(pICCProfile, pData, uDataSize) \
   ((pL_FillICCProfileStructure )?pL_FillICCProfileStructure(pICCProfile, pData, uDataSize):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPFILLICCPROFILEFROMICCFILE(pszFileName, pICCProfile) \
   ((pL_FillICCProfileFromICCFile )?pL_FillICCProfileFromICCFile(pszFileName, pICCProfile):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPINITICCPROFILE(pICCProfile, uStructSize) \
   ((pL_InitICCProfile )?pL_InitICCProfile(pICCProfile, uStructSize):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPFREEICCPROFILE(pICCProfile) \
   ((pL_FreeICCProfile )?pL_FreeICCProfile(pICCProfile):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPINITICCHEADER(pICCProfile) \
   ((pL_InitICCHeader )?pL_InitICCHeader(pICCProfile):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCCMMTYPE(pICCProfile, nCMMType) \
   ((pL_SetICCCMMType )?pL_SetICCCMMType(pICCProfile, nCMMType):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCDEVICECLASS(pICCProfile, uDevClassSig) \
   ((pL_SetICCDeviceClass )?pL_SetICCDeviceClass(pICCProfile, uDevClassSig):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCCOLORSPACE(pICCProfile, uColorSpace) \
   ((pL_SetICCColorSpace )?pL_SetICCColorSpace(pICCProfile, uColorSpace):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCCONNECTIONSPACE(pICCProfile, uPCS) \
   ((pL_SetICCConnectionSpace )?pL_SetICCConnectionSpace(pICCProfile, uPCS):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCPRIMARYPLATFORM(pICCProfile, uPrimPlatform) \
   ((pL_SetICCPrimaryPlatform )?pL_SetICCPrimaryPlatform(pICCProfile, uPrimPlatform):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCFLAGS(pICCProfile, uFlags) \
   ((pL_SetICCFlags )?pL_SetICCFlags(pICCProfile, uFlags):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCDEVMANUFACTURER(pICCProfile, nDevManufacturer) \
   ((pL_SetICCDevManufacturer )?pL_SetICCDevManufacturer(pICCProfile, nDevManufacturer):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCDEVMODEL(pICCProfile, uDevModel) \
   ((pL_SetICCDevModel )?pL_SetICCDevModel(pICCProfile, uDevModel):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCDEVICEATTRIBUTES(pICCProfile, uAttributes) \
   ((pL_SetICCDeviceAttributes )?pL_SetICCDeviceAttributes(pICCProfile, uAttributes):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCRENDERINGINTENT(pICCProfile, uRenderingIntent) \
   ((pL_SetICCRenderingIntent )?pL_SetICCRenderingIntent(pICCProfile, uRenderingIntent):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCCREATOR(pICCProfile, nCreator) \
   ((pL_SetICCCreator )?pL_SetICCCreator(pICCProfile, nCreator):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCDATETIME(pICCProfile, pTime) \
   ((pL_SetICCDateTime )?pL_SetICCDateTime(pICCProfile, pTime):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCTAGDATA(pICCProfile, pTagData, uTagSig, uTagTypeSig) \
   ((pL_SetICCTagData )?pL_SetICCTagData(pICCProfile, pTagData, uTagSig, uTagTypeSig):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPGETICCTAGDATA(pICCProfile, pTagData, uTagSignature) \
   ((pL_GetICCTagData )?pL_GetICCTagData(pICCProfile, pTagData, uTagSignature):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCREATEICCTAGDATA(pDestTagData, pSrcTagData, uTagTypeSig) \
   ((pL_CreateICCTagData )?pL_CreateICCTagData(pDestTagData, pSrcTagData, uTagTypeSig):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPDELETEICCTAG(pICCProfile, uTagSig, pTag) \
   ((pL_DeleteICCTag )?pL_DeleteICCTag(pICCProfile, uTagSig, pTag):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPGENERATEICCFILE(pICCProfile, pszFileName) \
   ((pL_GenerateICCFile )?pL_GenerateICCFile(pICCProfile, pszFileName):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPDOUBLETO2BFIXED2BNUMBER(dNumber) \
   ((pL_DoubleTo2bFixed2bNumber )?pL_DoubleTo2bFixed2bNumber(dNumber):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRP2BFIXED2BNUMBERTODOUBLE(uNumber) \
   ((pL_2bFixed2bNumberToDouble )?pL_2bFixed2bNumberToDouble(uNumber):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPDOUBLETOU8FIXED8NUMBER(dNumber) \
   ((pL_DoubleToU8Fixed8Number )?pL_DoubleToU8Fixed8Number(dNumber):(L_UINT16)WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPU8FIXED8NUMBERTODOUBLE(uNumber) \
   ((pL_U8Fixed8NumberToDouble )?pL_U8Fixed8NumberToDouble(uNumber):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPGENERATEICCPOINTER(pICCProfile) \
   ((pL_GenerateICCPointer )?pL_GenerateICCPointer(pICCProfile):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPGETICCTAGTYPESIG(pICCProfile, uTagSig) \
   ((pL_GetICCTagTypeSig )?pL_GetICCTagTypeSig(pICCProfile, uTagSig):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPFREEICCTAGTYPE(pTagType, uTagTypeSig) \
   ((pL_FreeICCTagType )?pL_FreeICCTagType(pTagType, uTagTypeSig):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCONVERTCLUTTOBUFFER(pData, pIccCLUT, nPrecision, nDataSize) \
   ((pL_ConvertCLUTToBuffer )?pL_ConvertCLUTToBuffer(pData, pIccCLUT, nPrecision, nDataSize):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCONVERTCURVETYPETOBUFFER(pData, pIccTagCurveType) \
   ((pL_ConvertCurveTypeToBuffer )?pL_ConvertCurveTypeToBuffer(pData, pIccTagCurveType):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPCONVERTPARAMETRICCURVETYPETOBUFFER(pData, pIccTagParametricCurveType) \
   ((pL_ConvertParametricCurveTypeToBuffer )?pL_ConvertParametricCurveTypeToBuffer(pData, pIccTagParametricCurveType):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSETICCPROFILEID(pICCProfile) \
   ((pL_SetICCProfileId )?pL_SetICCProfileId(pICCProfile):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPLOADICCPROFILE(pszFilename, pICCProfile, pLoadOptions) \
   ((pL_LoadICCProfile )?pL_LoadICCProfile(pszFilename, pICCProfile, pLoadOptions):WRPERR_LTCLR_DLL_NOT_LOADED)

#define L_WRPSAVEICCPROFILE(pszFilename, pICCProfile, pSaveOptions) \
   ((pL_SaveICCProfile )?pL_SaveICCProfile(pszFilename, pICCProfile, pSaveOptions):WRPERR_LTCLR_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTNTF.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPNITFCREATE(phNitf, pszFileName) \
   ((pL_NITFCreate )?pL_NITFCreate(phNitf, pszFileName):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFDESTROY(hNitf) \
   ((pL_NITFDestroy )?pL_NITFDestroy(hNitf):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFGETSTATUS(hNitf) \
   ((pL_NITFGetStatus )?pL_NITFGetStatus(hNitf):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFSAVEFILE(hNitf, pszFileName) \
   ((pL_NITFSaveFile )?pL_NITFSaveFile(hNitf, pszFileName):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFAPPENDIMAGESEGMENT(hNitf, pBitmap, nFormat, nBpp, nQFactor) \
   ((pL_NITFAppendImageSegment )?pL_NITFAppendImageSegment(hNitf, pBitmap, nFormat, nBpp, nQFactor):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFAPPENDGRAPHICSEGMENT(hNitf, pVector, prcVecBounds) \
   ((pL_NITFAppendGraphicSegment )?pL_NITFAppendGraphicSegment(hNitf, pVector, prcVecBounds):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFAPPENDTEXTSEGMENT(hNitf, pszFileName) \
   ((pL_NITFAppendTextSegment )?pL_NITFAppendTextSegment(hNitf, pszFileName):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFSETVECTORHANDLE(hNitf, uIndex, pVector) \
   ((pL_NITFSetVectorHandle )?pL_NITFSetVectorHandle(hNitf, uIndex, pVector):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFGETVECTORHANDLE(hNitf, uIndex) \
   ((pL_NITFGetVectorHandle )?pL_NITFGetVectorHandle(hNitf, uIndex):(LBase::RecordError(WRPERR_LTNTF_DLL_NOT_LOADED),(pVECTORHANDLE)0))

#define L_WRPNITFGETNITFHEADER(hNitf, pNITFHeader) \
   ((pL_NITFGetNITFHeader  )?pL_NITFGetNITFHeader (hNitf, pNITFHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFSETNITFHEADER(hNitf, pNITFHeader) \
   ((pL_NITFSetNITFHeader  )?pL_NITFSetNITFHeader (hNitf, pNITFHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFGETGRAPHICHEADERCOUNT(hNitf) \
   ((pL_NITFGetGraphicHeaderCount )?pL_NITFGetGraphicHeaderCount(hNitf):(LBase::RecordError(WRPERR_LTNTF_DLL_NOT_LOADED), 0))

#define L_WRPNITFGETGRAPHICHEADER(hNitf, uIndex, pGraphicHeader) \
   ((pL_NITFGetGraphicHeader  )?pL_NITFGetGraphicHeader (hNitf, uIndex, pGraphicHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFSETGRAPHICHEADER(hNitf, uIndex, pGraphicsHeader) \
   ((pL_NITFSetGraphicHeader  )?pL_NITFSetGraphicHeader (hNitf, uIndex, pGraphicsHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFGETIMAGEHEADERCOUNT(hNitf) \
   ((pL_NITFGetImageHeaderCount )?pL_NITFGetImageHeaderCount(hNitf):(LBase::RecordError(WRPERR_LTNTF_DLL_NOT_LOADED), 0))

#define L_WRPNITFGETIMAGEHEADER(hNitf, uIndex, pImageHeader) \
   ((pL_NITFGetImageHeader  )?pL_NITFGetImageHeader (hNitf, uIndex, pImageHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFSETIMAGEHEADER(hNitf, uIndex, pImageHeader) \
   ((pL_NITFSetImageHeader  )?pL_NITFSetImageHeader (hNitf, uIndex, pImageHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFGETTEXTHEADERCOUNT(hNitf) \
   ((pL_NITFGetTextHeaderCount )?pL_NITFGetTextHeaderCount(hNitf):(LBase::RecordError(WRPERR_LTNTF_DLL_NOT_LOADED), 0))

#define L_WRPNITFGETTEXTHEADER(hNitf, uIndex, pTxtHeader) \
   ((pL_NITFGetTextHeader  )?pL_NITFGetTextHeader (hNitf, uIndex, pTxtHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFSETTEXTHEADER(hNitf, uIndex, pTxtHeader) \
   ((pL_NITFSetTextHeader  )?pL_NITFSetTextHeader (hNitf, uIndex, pTxtHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFGETTEXTSEGMENT(hNitf, uIndex, pTextBuffer, puBufferSize) \
   ((pL_NITFGetTextSegment  )?pL_NITFGetTextSegment (hNitf, uIndex, pTextBuffer, puBufferSize):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFFREENITFHEADER(pNITFHeader) \
   ((pL_NITFFreeNITFHeader  )?pL_NITFFreeNITFHeader (pNITFHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFFREEGRAPHICHEADER(pGraphicsHeader) \
   ((pL_NITFFreeGraphicHeader  )?pL_NITFFreeGraphicHeader (pGraphicsHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFFREEIMAGEHEADER(pImageHeader) \
   ((pL_NITFFreeImageHeader )?pL_NITFFreeImageHeader (pImageHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

#define L_WRPNITFFREETEXTHEADER(pTxtHeader) \
   ((pL_NITFFreeTextHeader  )?pL_NITFFreeTextHeader (pTxtHeader):WRPERR_LTNTF_DLL_NOT_LOADED)

//-----------------------------------------------------------------------------
//--LTWIA.H FUNCTIONS MACROS---------------------------------------------------
//-----------------------------------------------------------------------------

#define L_WRPWIAISAVAILABLE(uWiaVersion) \
   ((pL_WiaIsAvailable  )?pL_WiaIsAvailable (uWiaVersion):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAINITSESSION(uWiaVersion, pphSession) \
   ((pL_WiaInitSession  )?pL_WiaInitSession (uWiaVersion, pphSession):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAENDSESSION(hSession) \
   ((pL_WiaEndSession  )?pL_WiaEndSession (hSession):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAENUMDEVICES(hSession, pfnCallback, pUserData) \
   ((pL_WiaEnumDevices )?pL_WiaEnumDevices(hSession, pfnCallback, pUserData):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASELECTDEVICEDLG(hSession, hWndParent, uDeviceType, uFlags) \
   ((pL_WiaSelectDeviceDlg )?pL_WiaSelectDeviceDlg(hSession, hWndParent, uDeviceType, uFlags):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASELECTDEVICE(hSession, pszDeviceId) \
   ((pL_WiaSelectDevice )?pL_WiaSelectDevice(hSession, pszDeviceId):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAGETSELECTEDDEVICE(hSession) \
   ((pL_WiaGetSelectedDevice )?pL_WiaGetSelectedDevice(hSession):(LBase::RecordError(WRPERR_LTWIA_DLL_NOT_LOADED),0))

#define L_WRPWIAGETSELECTEDDEVICETYPE(hSession, puDeviceType) \
   ((pL_WiaGetSelectedDeviceType )?pL_WiaGetSelectedDeviceType(hSession, puDeviceType):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAGETPROPERTIES(hSession, pItem, pProperties) \
   ((pL_WiaGetProperties )?pL_WiaGetProperties(hSession, pItem, pProperties):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASETPROPERTIES(hSession, pItem, pProperties, pfnCallback, pUserData) \
   ((pL_WiaSetProperties )?pL_WiaSetProperties(hSession, pItem, pProperties, pfnCallback, pUserData):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAACQUIRE(hSession, hWndParent, uFlags, pSourceItem, pAcquireOptions, pnFilesCount, pppszFilePaths, pfnCallback, pUserData) \
   ((pL_WiaAcquire )?pL_WiaAcquire(hSession, hWndParent, uFlags, pSourceItem, pAcquireOptions, pnFilesCount, pppszFilePaths, pfnCallback, pUserData):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAACQUIRETOFILE(hSession, hWndParent, uFlags, pSourceItem, pAcquireOptions, pnFilesCount, pppszFilePaths, pfnCallback, pUserData) \
   ((pL_WiaAcquireToFile )?pL_WiaAcquireToFile(hSession, hWndParent, uFlags, pSourceItem, pAcquireOptions, pnFilesCount, pppszFilePaths, pfnCallback, pUserData):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAACQUIRESIMPLE(uWiaVersion, hWndParent, uDeviceType, uFlags, pSourceItem, pAcquireOptions, pnFilesCount, pppszFilePaths, pfnCallback, pUserData) \
   ((pL_WiaAcquireSimple )?pL_WiaAcquireSimple(uWiaVersion, hWndParent, uDeviceType, uFlags, pSourceItem, pAcquireOptions, pnFilesCount, pppszFilePaths, pfnCallback, pUserData):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASTARTVIDEOPREVIEW(hSession, hWndParent, bStretchToFitParent) \
   ((pL_WiaStartVideoPreview )?pL_WiaStartVideoPreview(hSession, hWndParent, bStretchToFitParent):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIARESIZEVIDEOPREVIEW(hSession, bStretchToFitParent) \
   ((pL_WiaResizeVideoPreview )?pL_WiaResizeVideoPreview(hSession, bStretchToFitParent):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAENDVIDEOPREVIEW(hSession) \
   ((pL_WiaEndVideoPreview )?pL_WiaEndVideoPreview(hSession):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAACQUIREIMAGEFROMVIDEO(hSession, pszFileName, puLength) \
   ((pL_WiaAcquireImageFromVideo )?pL_WiaAcquireImageFromVideo(hSession, pszFileName, puLength):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAISVIDEOPREVIEWAVAILABLE(hSession) \
   ((pL_WiaIsVideoPreviewAvailable )?pL_WiaIsVideoPreviewAvailable(hSession):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAGETROOTITEM(hSession, pItem, ppWiaRootItem) \
   ((pL_WiaGetRootItem )?pL_WiaGetRootItem(hSession, pItem, ppWiaRootItem):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAENUMCHILDITEMS(hSession, pWiaRootItem, pfnCallback, pUserData) \
   ((pL_WiaEnumChildItems )?pL_WiaEnumChildItems(hSession, pWiaRootItem, pfnCallback, pUserData):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAFREEITEM(hSession, pItem) \
   ((pL_WiaFreeItem )?pL_WiaFreeItem(hSession, pItem):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAGETPROPERTYLONG(hSession, pItem, pszID, uID, plValue) \
   ((pL_WiaGetPropertyLong )?pL_WiaGetPropertyLong(hSession, pItem, pszID, uID, plValue):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASETPROPERTYLONG(hSession, pItem, pszID, uID, lValue) \
   ((pL_WiaSetPropertyLong )?pL_WiaSetPropertyLong(hSession, pItem, pszID, uID, lValue):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAGETPROPERTYGUID(hSession, pItem, pszID, uID, pGuidValue) \
   ((pL_WiaGetPropertyGUID )?pL_WiaGetPropertyGUID(hSession, pItem, pszID, uID, pGuidValue):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASETPROPERTYGUID(hSession, pItem, pszID, uID, pGuidValue) \
   ((pL_WiaSetPropertyGUID )?pL_WiaSetPropertyGUID(hSession, pItem, pszID, uID, pGuidValue):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAGETPROPERTYSTRING(hSession, pItem, pszID, uID, pszValue, puLength) \
   ((pL_WiaGetPropertyString )?pL_WiaGetPropertyString(hSession, pItem, pszID, uID, pszValue, puLength):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASETPROPERTYSTRING(hSession, pItem, pszID, uID, pszValue) \
   ((pL_WiaSetPropertyString )?pL_WiaSetPropertyString(hSession, pItem, pszID, uID, pszValue):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAGETPROPERTYSYSTEMTIME(hSession, pItem, pszID, uID, pValue) \
   ((pL_WiaGetPropertySystemTime )?pL_WiaGetPropertySystemTime(hSession, pItem, pszID, uID, pValue):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASETPROPERTYSYSTEMTIME(hSession, pItem, pszID, uID, pValue) \
   ((pL_WiaSetPropertySystemTime )?pL_WiaSetPropertySystemTime(hSession, pItem, pszID, uID, pValue):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAGETPROPERTYBUFFER(hSession, pItem, pszID, uID, pValue, puSize) \
   ((pL_WiaGetPropertyBuffer )?pL_WiaGetPropertyBuffer(hSession, pItem, pszID, uID, pValue, puSize):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIASETPROPERTYBUFFER(hSession, pItem, pszID, uID, pValue, uSize) \
   ((pL_WiaSetPropertyBuffer )?pL_WiaSetPropertyBuffer(hSession, pItem, pszID, uID, pValue, uSize):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAENUMCAPABILITIES(hSession, pItem, uFlags, pfnCallBack, pUserData) \
   ((pL_WiaEnumCapabilities )?pL_WiaEnumCapabilities(hSession, pItem, uFlags, pfnCallBack, pUserData):WRPERR_LTWIA_DLL_NOT_LOADED)

#define L_WRPWIAENUMFORMATS(hSession, pItem, uFlags, pfnCallBack, pUserData) \
   ((pL_WiaEnumFormats )?pL_WiaEnumFormats(hSession, pItem, uFlags, pfnCallBack, pUserData):WRPERR_LTWIA_DLL_NOT_LOADED)

#endif //USE_POINTERS_TO_LEAD_FUNCTIONS

#endif //_LEAD_FUNCTIONS_MACROS_H_
/*================================================================= EOF =====*/
