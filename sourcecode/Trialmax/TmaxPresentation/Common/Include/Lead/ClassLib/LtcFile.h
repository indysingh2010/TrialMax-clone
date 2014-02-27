/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcfile.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_FILE_H_
#define  _LEAD_FILE_H_

#ifndef WriteFileComment
#define WriteFileComment z001WriteFileComment
#endif 

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LFile                                                           |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 1998                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LBaseFile :public LBase
{
   LEAD_DECLAREOBJECT(LBaseFile);

   public:
      L_VOID *m_extLBaseFile;

   protected:

      static   L_INT    EXT_CALLBACK RedirectOpenCS(
                                       L_TCHAR *pFile,
                                       L_INT nMode,
                                       L_INT nShare,
                                       LBaseFile *pUserData
                                      );
      static   L_INT    EXT_CALLBACK RedirectCloseCS(L_INT FD,
                                       LBaseFile *pUserData);

      static   L_UINT   EXT_CALLBACK RedirectReadCS(
                                       L_INT FD,
                                       L_CHAR *pBuf,
                                       L_INT nCount,
                                       LBaseFile *pUserData
                                      );
      static   L_UINT    EXT_CALLBACK RedirectWriteCS(
                                          L_INT FD,
                                          L_CHAR *pBuf,
                                          L_INT nCount,
                                          LBaseFile *pUserData
                                       );
      static   L_INT32  EXT_CALLBACK RedirectSeekCS(
                                       L_INT FD,
                                       L_INT32 lnPos,
                                       L_INT nCount,
                                       LBaseFile *pUserData
                                      );

   protected:  

      virtual  L_INT    RedirectOpenCallBack(
                                             L_TCHAR *pFile,
                                             L_INT nMode,
                                             L_INT nShare
                                            );
      virtual  L_INT    RedirectCloseCallBack(L_INT FD);
      virtual  L_UINT   RedirectReadCallBack(
                                             L_INT FD,
                                             L_CHAR *pBuf,
                                             L_INT nCount
                                            );
      virtual  L_UINT   RedirectWriteCallBack(
                                                L_INT FD,
                                                L_CHAR *pBuf,
                                                L_INT nCount
                                             );
      virtual  L_INT32  RedirectSeekCallBack(
                                             L_INT FD,
                                             L_INT32 lnPos,
                                             L_INT nCount
                                            );

   protected:  
      LBitmapBase *           m_pBitmap;
      LVectorBase *           m_pVector;
      L_BOOL                  m_bEnableRedirectIOCB;
      L_UINT                  m_uEnableFlags;

   public:
      LBaseFile();
      virtual ~LBaseFile();

      L_UINT GetRedirectIOFlags();
      L_VOID SetRedirectIOFlags(L_UINT uEnableFlags=IO_REDIRECT_ALL);
      L_BOOL IsRedirectIOEnabled();
      L_BOOL EnableRedirectIO(L_BOOL bEnable);

      L_VOID SetBitmap(LBitmapBase * pBitmap);
      L_VOID SetVector(LVectorBase * pVector);
      static L_INT GetDefaultLoadFileOption(pLOADFILEOPTION pLoadFileOption, L_UINT uStructSize);
      static L_INT GetDefaultSaveFileOption(pSAVEFILEOPTION pSaveFileOption, L_UINT uStructSize);

      static  L_INT           GetFilterListInfo(pFILTERINFO *ppFilterList,
                                                L_UINT *pFilterCount);

      static  L_INT           GetFilterInfo(L_TCHAR * pFilterName,
                                             pFILTERINFO pFilterInfo,
                                             L_UINT uStructSize);

      static  L_INT           FreeFilterInfo(pFILTERINFO pFilterInfo, 
                                             L_UINT uFilterCount, 
                                             L_UINT uFlags);

      static  L_INT           SetFilterInfo(pFILTERINFO pFilterInfo, 
                                             L_UINT uFilterCount, 
                                             L_UINT uFlags);
};

/*----------------------------------------------------------------------------+
| Class     : LFile                                                           |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 27 may 1998                                                     |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LFile : public LBaseFile
{
   LEAD_DECLAREOBJECT(LFile);

   public:
      L_VOID *m_extLFile;
      
   protected:  
     L_TCHAR          m_szFileName[MAX_PATH+1];   
     L_BOOL          m_bFeedLoadStarted;
     LBitmapBase     m_FeedLoadBitmap;
     HGLOBAL         m_hFeedLoad;

   private:
      static L_INT  EXT_CALLBACK LoadFileTileCS(
                                    pFILEINFO  pFileInfo,
                                    pBITMAPHANDLE pBitmap,
                                    L_UCHAR  * pBuffer,
                                    L_UINT  uFlags,
                                    L_INT  nRow,
                                    L_INT nLines,
                                    L_VOID * pUserData
                                  );

      static L_INT  EXT_CALLBACK LoadFileCS(
                                 pFILEINFO pFileInfo,
                                 pBITMAPHANDLE pBitmap,
                                 L_UCHAR * pBuffer,
                                 L_UINT  uFlags,
                                 L_INT  nRow,
                                 L_INT  nLines,
                                 L_VOID * pUserData
                              );

      static L_INT  EXT_CALLBACK LoadFileOffsetCS(
                                       pFILEINFO pFileInfo,
                                       pBITMAPHANDLE pBitmap,
                                       L_UCHAR * pBuffer,
                                       L_UINT uFlags,
                                       L_INT nRow,
                                       L_INT nLines,
                                       L_VOID * pUserData
                                    );

      static L_INT  EXT_CALLBACK  FeedLoadCS(
                                 pFILEINFO pFileInfo,
                                 pBITMAPHANDLE pBitmap,
                                 L_UCHAR * pBuffer, 
                                 L_UINT uFlags,L_INT nRow,
                                 L_INT nLines,
                                 L_VOID * pUserData
                               );

      static L_INT  EXT_CALLBACK SaveFileOffsetCS(
                                       pBITMAPHANDLE pBitmap,
                                       L_UCHAR * pBuffer,
                                       L_UINT uRow,
                                       L_UINT uLines,
                                       L_VOID * pUserData
                                    );

      static L_INT  EXT_CALLBACK SaveFileCS(
                                 pBITMAPHANDLE pBitmap,
                                 L_UCHAR * pBuffer,
                                 L_UINT uRow,
                                 L_UINT uLines,
                                 L_VOID *  pUserData
                              );

      static L_INT  EXT_CALLBACK SaveFileTileCS(
                                    pBITMAPHANDLE pBitmap,
                                    L_UCHAR * pBuffer,
                                    L_UINT uRow,
                                    L_UINT uLines,
                                    L_VOID *  pUserData
                                  );

      static   L_INT  EXT_CALLBACK BrowseDirCS(
                                   pBITMAPHANDLE pBitmap,
                                   L_TCHAR * pszFilename,
                                   pFILEINFO pFileInfo,
                                   L_INT nStatus,
                                   L_INT nPercent,
                                   L_VOID *  pUserData
                                 );

      static   L_INT  EXT_CALLBACK TransformFileCS(L_UINT uMarker,
                                                   L_UINT uMarkerSize,
                                                   L_UCHAR * pMarkerData,
                                                   L_VOID * pUserData,
                                                   L_UINT uTransform,
                                                   LEADMARKERCALLBACK pfnLEADCallback,
                                                   L_VOID * pLEADUserData);

      static   L_INT EXT_CALLBACK CreateThumbnailCS(
                              pFILEINFO  pFileInfo,
                              pBITMAPHANDLE pBitmap,
                              L_UCHAR  * pBuffer,
                              L_UINT  uFlags,
                              L_INT  nRow,
                              L_INT nLines,
                              L_VOID * pUserData
                           );
   protected:     

      virtual    L_INT  LoadFileTileCallBack(
                                             pFILEINFO pFileInfo,
                                             LBitmapBase * pLBitmap,
                                             LBuffer * pLBuffer,
                                             L_UINT uFlags,
                                             L_INT nRow,
                                             L_INT nLines
                                            );

      virtual    L_INT  LoadFileCallBack(
                                          pFILEINFO pFileInfo,
                                          LBitmapBase * pLBitmap,
                                          LBuffer * pLBuffer,
                                          L_UINT uFlags,
                                          L_INT nRow,
                                          L_INT nLines
                                        );

      virtual    L_INT  LoadFileOffsetCallBack(
                                                pFILEINFO pFileInfo,
                                                LBitmapBase * pLBitmap,
                                                LBuffer * pLBuffer,
                                                L_UINT uFlags,
                                                L_INT nRow,
                                                L_INT nLines
                                              );

      virtual    L_INT  FeedLoadCallBack(
                                          pFILEINFO pFileInfo,
                                          LBitmapBase * pLBitmap,
                                          LBuffer * pLBuffer, 
                                          L_UINT uFlags,
                                          L_INT nRow,
                                          L_INT nLines
                                        );

      virtual    L_INT  SaveFileOffsetCallBack(
                                                LBitmapBase * pLBitmap,
                                                LBuffer * pLBuffer,
                                                L_UINT uRow,
                                                L_UINT uLines
                                              );

      virtual    L_INT  SaveFileCallBack(
                                          LBitmapBase * pLBitmap,
                                          LBuffer * pLBuffer,
                                          L_UINT uRow,
                                          L_UINT uLines
                                        );

      virtual    L_INT  SaveFileTileCallBack(
                                             LBitmapBase * pLBitmap,
                                             LBuffer * pLBuffer,
                                             L_UINT uRow,
                                             L_UINT uLines
                                           );

      virtual    L_INT  BrowseDirCallBack(
                                           LBitmapBase * pLBitmap,
                                           L_TCHAR * pszFilename,
                                           pFILEINFO pFileInfo,
                                           L_INT nStatus,
                                           L_INT nPercent
                                         );

      virtual    L_INT  TransformFileCallBack(L_UINT uMarker,
                                              L_UINT uMarkerSize,
                                              L_UCHAR * pMarkerData,
                                              L_UINT uTransform,
                                              LEADMARKERCALLBACK pfnLEADCallback,
                                              L_VOID * pLEADUserData);

      virtual    L_INT  CreateThumbnailCallBack(
                                             pFILEINFO pFileInfo,
                                             LBitmapBase * pLBitmap,
                                             LBuffer * pLBuffer,
                                             L_UINT uFlags,
                                             L_INT nRow,
                                             L_INT nLines
                                            );

   public : 
      LFile();
      LFile(LBitmapBase * pLBitmap);
      LFile(LBitmapBase * pLBitmap,L_TCHAR * pFileName);
      virtual ~LFile();

      L_BOOL        IsValid();

      L_TCHAR *     GetFileName();
      L_INT         GetFileName(L_TCHAR * pBuffer,L_UINT * puBuffSize);
      L_VOID        SetFileName(L_TCHAR * pFileName);
      L_VOID        SetBitmap(LBitmapBase* pBitmap);


      virtual L_INT DeletePage(L_INT nPage,L_UINT uFlags=0,pSAVEFILEOPTION pSaveFileOption=NULL);
      virtual L_INT ReadLoadResolutions(
                                    pDIMENSION pDimensions,
                                    L_INT * pDimensionCount,
                                    pLOADFILEOPTION pLoadFileOption=NULL
                                  );
      virtual L_INT ReadFileTransforms(   pFILETRANSFORMS pTransforms,
                                          pLOADFILEOPTION pLoadFileOption=NULL);
      virtual L_INT WriteFileTransforms(
                                          pFILETRANSFORMS pTransforms,
                                          L_INT  nFlags=0,
                                          pSAVEFILEOPTION pSaveFileOption=NULL
                                       );
      virtual L_INT StartFeedLoad(
                                    L_INT nBitsPerPixel=0,
                                    L_INT nOrder=ORDER_BGRORGRAY,
                                    L_UINT uFlags=LOADFILE_ALLOCATE|LOADFILE_STORE,
                                    pLOADFILEOPTION pLoadFileOption=NULL,
                                    pFILEINFO pFileInfo=NULL
                                    );
      virtual L_INT FeedLoad(LBuffer * pLBuffer);
      virtual L_INT StopFeedLoad();  
      virtual L_INT GetInfo(
                              pFILEINFO pFileInfo,
                              L_UINT uStructSize,
                              L_UINT uFlags=FILEINFO_TOTALPAGES,
                              pLOADFILEOPTION pLoadFileOption=NULL
                           ); 
      virtual L_INT GetCommentSize( L_UINT uType,
                                    L_UINT * puSize,
                                    pLOADFILEOPTION pLoadFileOption=NULL);
      virtual L_INT GetPCDResolution(pPCDINFO pPCDInfo);
      virtual L_INT Load(
                           L_INT nBitsPerPixel,
                           L_INT nOrder,
                           L_INT nPage,
                           pFILEINFO pFileInfo = NULL
                        );
      virtual L_INT Load(
                           L_INT nBitsPerPixel=0,
                           L_INT nOrder=ORDER_BGRORGRAY,
                           pLOADFILEOPTION pLoadFileOption = NULL,
                           pFILEINFO pFileInfo = NULL
                           );

      virtual L_INT LoadBitmapResize(L_INT nDestWidth,
                                     L_INT nDestHeight,
                                     L_INT nDestBits,
                                     L_UINT32 uFlags,
                                     L_INT nOrder,
                                     pLOADFILEOPTION pLoadOptions,
                                     pFILEINFO pFileInfo);

      virtual L_INT LoadBitmapList(
                                    LBitmapList * pLFileBitmapList,
                                    L_INT nBitsTo=0,
                                    L_INT nColorOrder=ORDER_BGRORGRAY,
                                    pLOADFILEOPTION pLoadFileOption = NULL,
                                    pFILEINFO pFileInfo = NULL
                                  );
      virtual L_INT LoadFile(
                              L_INT nBitsPerPixel,
                              L_INT nOrder,
                              L_UINT uFlags,
                              L_INT nPage,
                              pFILEINFO pFileInfo = NULL
                            );

      virtual L_INT LoadFile(
                              L_INT nBitsPerPixel=0,
                              L_INT nOrder=ORDER_BGRORGRAY,
                              L_UINT uFlags=LOADFILE_ALLOCATE | LOADFILE_STORE,
                              pLOADFILEOPTION pLoadFileOption = NULL,
                              pFILEINFO pFileInfo = NULL
                            );

      virtual L_INT LoadTile(
                              L_INT nCol,
                              L_INT nRow,
                              L_UINT uWidth,
                              L_UINT uHeight,
                              L_INT nBitsPerPixel=0,
                              L_INT nOrder=ORDER_BGRORGRAY, 
                              L_UINT uFlags=LOADFILE_ALLOCATE | LOADFILE_STORE,
                              pLOADFILEOPTION pLoadFileOption = NULL,
                              pFILEINFO pFileInfo = NULL
                            );
      virtual L_INT LoadOffset(
                                 L_HFILE fd,
                                 L_SSIZE_T nOffsetBegin,
                                 L_SSIZE_T nBytesToLoad,
                                 L_INT nBitsPerPixel=0,
                                 L_INT nOrder=ORDER_BGRORGRAY,
                                 L_UINT uFlags=LOADFILE_ALLOCATE|LOADFILE_STORE,
                                 pLOADFILEOPTION pLoadFileOption = NULL,
                                 pFILEINFO pFileInfo = NULL
                              );

      virtual L_INT LoadLayer(L_INT nBitsPerPixel,
                              L_INT nOrder,
                              L_INT nLayer,
                              pLAYERINFO pLayerInfo = NULL,
                              pLOADFILEOPTION pLoadOptions = NULL);


      virtual L_INT ReadComment( L_UINT uType,
                                 LBuffer * pLBuffer,
                                 pLOADFILEOPTION pLoadFileOption = NULL);
      virtual L_INT ReadCommentExt(
                                    L_UINT uType,
                                    pFILECOMMENTS  pComments,
                                    L_UCHAR * pBuffer,
                                    L_UINT *   puLength,
                                    pLOADFILEOPTION pLoadFileOption = NULL
                                  );
      virtual L_INT ReadStamp(pLOADFILEOPTION pLoadFileOption = NULL); 
      virtual L_INT ReadTag(
                              L_UINT16 uTag,
                              L_UINT16 * pType,
                              L_UINT * pCount,
                              LBuffer * pLBuffer,
                              pLOADFILEOPTION pLoadFileOption = NULL
                           ); 
      virtual L_INT Save(
                           L_INT nFormat,
                           L_INT nBitsPerPixel,
                           L_INT nQFactor,
                           L_INT nPageNumber,
                           L_UINT uFlags
                        );

      virtual L_INT Save(
                           L_INT nFormat,
                           L_INT nBitsPerPixel=0,
                           L_INT nQFactor=2,
                           L_UINT uFlags=MULTIPAGE_OPERATION_OVERWRITE,
                           pSAVEFILEOPTION pSaveFileOption = NULL
                        );

      virtual L_INT SaveBitmapList(
                                    LBitmapList * pList,
                                    L_INT nFormat,
                                    L_INT nBits=0,
                                    L_INT nQFactor=2,
                                    pSAVEFILEOPTION pSaveFileOption = NULL
                                  );
      virtual L_INT SaveFile(
                              L_INT nFormat,
                              L_INT nBitsPerPixel=0,
                              L_INT nQFactor=2,
                              L_UINT uFlags=0,
                              pSAVEFILEOPTION pSaveFileOption = NULL
                            );
      virtual L_INT SaveOffset(
                                 L_HFILE fd,
                                 L_SSIZE_T nOffsetBegin,
                                 L_SSIZE_T * pnSizeWritten,
                                 L_INT nFormat,
                                 L_INT nBitsPerPixel=0,
                                 L_INT nQFactor=2, 
                                 L_INT uFlags=0,
                                 pSAVEFILEOPTION pSaveFileOption = NULL
                              );
      virtual L_INT SaveTile( L_INT nCol,
                              L_INT nRow,
                              pSAVEFILEOPTION pSaveFileOption = NULL
                              );

      virtual L_INT SaveBitmapWithLayers(L_INT nFormat,
                                         L_INT nBitsPerPixel,
                                         L_INT nQFactor,
                                         LBitmapList * pLayers,
                                         pLAYERINFO pLayerInfo = NULL,
                                         L_INT nLayers = 0,
                                         pSAVEFILEOPTION pSaveOptions = NULL);

      virtual L_INT WriteComment(
                                    L_UINT uType,
                                    pFILECOMMENTS  pComments,
                                    pSAVEFILEOPTION pSaveFileOption = NULL
                                );
      
      virtual L_INT WriteTag(pSAVEFILEOPTION pSaveOptions);
      

      virtual L_INT z001WriteFileComment(pSAVEFILEOPTION pSaveFileOption=NULL);



      virtual L_INT WriteStamp(pSAVEFILEOPTION pSaveFileOption = NULL);
      static  L_INT FileConvert(
                                 L_TCHAR * pszFileDst,
                                 L_TCHAR * pszFileSrc,
                                 L_INT nType,
                                 L_INT nWidth,
                                 L_INT nHeight,
                                 L_INT nBitsPerPixel,
                                 L_INT nQFactor,
                                 pLOADFILEOPTION pLoadFileOption = NULL,
                                 pSAVEFILEOPTION pSaveFileOption = NULL,
                                 pFILEINFO pFileInfo = NULL
                               );
      static  L_INT GetProperQualityFactor(L_INT nFileFormat);


      virtual L_INT BrowseDir( L_TCHAR * pszPath,
                               L_TCHAR * pszFilter,
                               pTHUMBOPTIONS pThumbOptions,
                               L_BOOL bStopOnError=FALSE,
                               L_BOOL bIncludeSubDirs=FALSE,
                               L_BOOL bExpandMultipage=FALSE,
                               L_INT32 lSizeDisk=0,
                               L_INT32 lSizeMem=0);

      virtual L_INT ReadFileExtensions(pEXTENSIONLIST * ppExtensionList,
                                       pLOADFILEOPTION pLoadOptions = NULL);

      virtual L_INT FreeExtensions(pEXTENSIONLIST pExtensionList);

      virtual L_INT LoadExtensionStamp(pEXTENSIONLIST pExtensionList);

      virtual L_INT GetExtensionAudio(pEXTENSIONLIST pExtensionList,
                                      L_INT nStream,
                                      L_UCHAR ** ppBuffer,
                                      L_SIZE_T * puSize);

      virtual L_INT WriteMetaData(L_UINT uFlags, 
                                  pSAVEFILEOPTION pSaveFileOption);
      
      virtual L_INT TransformFile(L_TCHAR * pszFileDst,
                                  L_UINT uTransform,
                                  pLOADFILEOPTION pLoadOptions = NULL);
      
      virtual L_INT CreateThumbnail(         
                                    const pTHUMBOPTIONS pThumbOptions,
                                    pLOADFILEOPTION pLoadOptions,
                                    pFILEINFO pFileInfo );

   private:
      static L_INT EXT_CALLBACK EnumTagsCS(L_UINT16 uTag, 
                                             L_UINT16 uType, 
                                             L_UINT32 uCount,
                                             L_VOID * pUserData);

      static L_INT EXT_CALLBACK EnumGeoKeysCS(L_UINT16 uTag, 
                                                   L_UINT16 uType, 
                                                   L_UINT uCount,
                                                   L_VOID *pData,
                                                   L_VOID * pUserData);

      static L_INT EXT_CALLBACK LoadCMYKArrayCS(pFILEINFO pFileInfo, pBITMAPHANDLE pBitmap,
                                                    L_UCHAR * pBuffer, L_UINT uFlags,
                                                    L_INT nRow, L_INT nLines, L_VOID * pUserData);

      static L_INT EXT_CALLBACK SaveCMYKArrayCS(pBITMAPHANDLE pBitmap,
                                                      L_UCHAR * pBuffer,
                                                      L_UINT uRow,
                                                      L_UINT uLines,
                                                      L_VOID * pUserData);

   protected:
protected:
       virtual L_INT EnumTagsCallBack(L_UINT16 uTag, 
                                      L_UINT16 uType, 
                                      L_UINT32 uCount);
      
       virtual L_INT EnumGeoKeysCallBack(L_UINT16 uTag, 
                                         L_UINT16 uType, 
                                         L_UINT uCount,
                                         L_VOID *pData);

       virtual L_INT LoadCMYKArrayCallBack(pFILEINFO pFileInfo, pBITMAPHANDLE pBitmap,
                                           L_UCHAR * pBuffer, L_UINT uFlags,
                                           L_INT nRow, L_INT nLines);

       virtual L_INT SaveCMYKArrayCallBack(pBITMAPHANDLE pBitmap,
                                           L_UCHAR * pBuffer,
                                           L_UINT uRow,
                                           L_UINT uLines);

   public:
       L_INT ReadCommentOffset (L_HFILE             fd,
                                    L_SSIZE_T           nOffsetBegin,
                                    L_SSIZE_T           nBytesToLoad,
                                    L_UINT            uType,
                                    L_UCHAR     *     pComment,
                                    L_UINT            uLength,
                                    pLOADFILEOPTION   pLoadOptions);
      
       L_INT Compact (L_TCHAR * pszDstFile, 
                           L_UINT uPages, 
                           pLOADFILEOPTION pLoadFileOption, 
                           pSAVEFILEOPTION pSaveFileOption);
      
       L_INT EnumTags( L_UINT uFlags, pLOADFILEOPTION pLoadOptions);

       L_INT DeleteTag (L_INT nPage,
                        L_UINT uTag,
                        L_UINT uFlags,
                        pSAVEFILEOPTION pSaveOptions);

       L_INT SetGeoKey (L_UINT16 uTag, 
                        L_UINT uType, 
                        L_UINT uCount, 
                        L_VOID * pData);

       L_INT GetGeoKey (L_UINT16 uTag,
                        L_UINT * puType,
                        L_UINT * puCount,
                        L_VOID * pData);

       L_INT WriteGeoKey (pSAVEFILEOPTION pSaveOptions);

       L_INT ReadGeoKey (L_UINT16 uTag,
                              L_UINT * puType,
                              L_UINT * puCount,
                              L_VOID * pData,
                              pLOADFILEOPTION pLoadOptions);

       L_INT EnumGeoKeys(L_UINT uFlags,
                              pLOADFILEOPTION pLoadOptions);

       L_INT LoadCMYKArray(pBITMAPHANDLE * ppBitmapArray,
                              L_UINT uBitmapArrayCount, 
                              L_UINT uStructSize, 
                              L_INT nBitsPerPixel, 
                              L_UINT uFlags, 
                              pLOADFILEOPTION pLoadFileOption,
                              pFILEINFO pFileInfo);

       L_INT SaveCMYKArray(pBITMAPHANDLE *ppBitmapArray, 
                              L_UINT uBitmapArrayCount, 
                              L_INT nFormat, 
                              L_INT nBitsPerPixel, 
                              L_INT nQFactor,
                              L_UINT uFlags,
                              pSAVEFILEOPTION pSaveOptions);

       L_VOID * GetLoadInfoCallbackData ( L_VOID );

};

/*----------------------------------------------------------------------------+
| Class     : LMemoryFile                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 27 may 1998                                                     |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LMemoryFile :public LBaseFile
{
   LEAD_DECLAREOBJECT(LMemoryFile);

   public:
      L_VOID *m_extLMemoryFile;
      LBuffer * m_pLSaveBuffer;  // For the callback functionality of SaveBitmapBuffer and SaveFileBuffer
      
   private:  
      LBuffer     LCompressOutputBuffer;
      L_BOOL      bCompressStarted;

   private:  

      static L_INT  EXT_CALLBACK LoadMemoryCS(
                                    pFILEINFO pFileInfo,
                                    pBITMAPHANDLE pBitmap,
                                    L_UCHAR *pBuffer,
                                    L_UINT uFlags,
                                    L_INT nRow,
                                    L_INT nLines,
                                    L_VOID *pUserData
                                );

      static L_INT  EXT_CALLBACK CompressBufferCS(
                                       pBITMAPHANDLE pBitmap,
                                       L_UCHAR * pBuffer,
                                       L_SIZE_T uBytes,
                                       L_VOID * pUserData
                                    );

      static L_INT EXT_CALLBACK SaveBitmapBufferCS(L_SIZE_T uRequiredSize,
                                      L_UCHAR ** ppBuffer,
                                      L_SIZE_T * pdwBufferSize,
                                      L_VOID * pUserData);

      static L_INT EXT_CALLBACK SaveFileBufferCS(L_SIZE_T uRequiredSize,
                                    L_UCHAR ** ppBuffer,
                                    L_SIZE_T * pdwBufferSize,
                                    L_VOID * pUserData);

      static L_INT EXT_CALLBACK SaveFileCS(pBITMAPHANDLE pBitmap,
                              L_UCHAR * pBuffer,
                              L_UINT uRow,
                              L_UINT uLines,
                              L_VOID * pUserData);

   protected : 

      virtual L_INT LoadMemoryCallBack(
                                          pFILEINFO pFileInfo,
                                          LBitmapBase * pLBitmap,
                                          LBuffer * pLBuffer,
                                          L_UINT uFlags,
                                          L_INT nRow,
                                          L_INT nLines
                                      );

      virtual L_INT CompressBufferCallBack(
                                             LBitmapBase * pLBitmap,
                                             LBuffer * pLBuffer
                                          );

      virtual L_INT SaveBitmapBufferCallBack(LBuffer * pLBuffer, L_SIZE_T uRequiredSize);
      
      virtual L_INT SaveFileBufferCallBack(LBuffer * pLBuffer, L_SIZE_T uRequiredSize);

      virtual L_INT SaveFileCallBack(LBitmapBase * pLBitmap,
                                     LBuffer * pLBuffer,
                                     L_UINT uRow,
                                     L_UINT uLines);

   public : 
      LMemoryFile();
      LMemoryFile(LBitmapBase * pBitmap);
      virtual ~LMemoryFile();

      L_BOOL  IsValid();

      virtual L_INT  GetInfo(
                                    LBuffer& LMemoryBuffer,
                                    pFILEINFO pFileInfo,
                                    L_UINT uStructSize,
                                    L_UINT uFlags=FILEINFO_TOTALPAGES,
                                    pLOADFILEOPTION pLoadFileOption = NULL
                                   );
      virtual L_INT  Load(
                                 LBuffer& LMemoryBuffer,
                                 L_INT nBitsPerPixel=0,
                                 L_INT nOrder=ORDER_BGRORGRAY,
                                 pLOADFILEOPTION pLoadFileOption = NULL,
                                 pFILEINFO pFileInfo = NULL
                                 );
      virtual L_INT  LoadMemory(
                                 LBuffer& LMemoryBuffer,
                                 L_INT nBitsPerPixel=0,
                                 L_INT nOrder=ORDER_BGRORGRAY,
                                 L_UINT uFlags=LOADFILE_ALLOCATE|LOADFILE_STORE,
                                 pLOADFILEOPTION pLoadFileOption = NULL,
                                 pFILEINFO pFileInfo = NULL
                               );
      virtual L_INT  ReadComment(
                                 LBuffer& LMemoryBuffer,
                                 LBuffer * pLCommentBuffer,
                                 L_UINT uType,
                                 pLOADFILEOPTION pLoadFileOption = NULL
                              );
      virtual L_INT  ReadTag(
                                 LBuffer& LMemoryBuffer,
                                 LBuffer * pLTagDataBuffer,
                                 L_UINT16 uTag,
                                 L_UINT16 * pType,
                                 L_UINT * pCount,
                                 pLOADFILEOPTION pLoadFileOption = NULL
                               );
      virtual L_INT Save(
                           LBuffer * pLMemoryBuffer,
                           L_INT nFormat,
                           L_INT nBitsPerPixel=0,
                           L_INT nQFactor=2,
                           pSAVEFILEOPTION pSaveFileOption = NULL
                        );

      virtual L_INT SaveBitmapBuffer(LBuffer * pLBuffer,
                                     L_SIZE_T * puFileSize,
                                     L_INT nFormat,
                                     L_INT nBitsPerPixel,
                                     L_INT nQFactor,
                                     pSAVEFILEOPTION pSaveOptions);

      virtual L_INT SaveFileBuffer(LBuffer * pLBuffer,
                                   L_SIZE_T * puFileSize,
                                   L_INT nFormat,
                                   L_INT nBitsPerPixel,
                                   L_INT nQFactor,
                                   L_UINT uFlags,
                                   pSAVEFILEOPTION pSaveOptions);

      virtual L_INT StartCompressBuffer(
                                          L_UINT32 uInputBytes,
                                          L_UINT uOutputBytes,
                                          L_INT nOutputType,
                                          L_INT nQFactor=2,
                                          pSAVEFILEOPTION pSaveFileOption = NULL
                                       );
      virtual L_INT CompressBuffer(LBuffer * pLBuffer); 
      virtual L_INT EndCompressBuffer();

      static L_INT SetMemoryThresholds(L_INT nTiledThreshold,
                                         L_INT nMaxConvSize,
                                         L_INT nTileSize,
                                         L_INT nConvTiles,
                                         L_INT nConvBuffers,
                                         L_UINT uFlags);

      static L_VOID GetMemoryThresholds(L_INT *pnTiledThreshold,
                                          L_SSIZE_T *pnMaxConvSize,
                                          L_SSIZE_T *pnTileSize,
                                          L_INT *pnConvTiles,
                                          L_INT *pnConvBuffers);

      static L_INT SetBitmapMemoryInfo(pBITMAPHANDLE  pBitmap,
                                         L_UINT         uMemory,
                                         L_UINT         uTileSize,   
                                         L_UINT         uTotalTiles,
                                         L_UINT         uConvTiles,
                                         L_UINT         uMaxTileViews,
                                         L_UINT         uTileViews,
                                         L_UINT         uFlags);
      static L_INT SetBitmapMemoryInfo(LBitmapBase  *plBitmap,
                                         L_UINT         uMemory,
                                         L_UINT         uTileSize,   
                                         L_UINT         uTotalTiles,
                                         L_UINT         uConvTiles,
                                         L_UINT         uMaxTileViews,
                                         L_UINT         uTileViews,
                                         L_UINT         uFlags);

      static L_INT GetBitmapMemoryInfo( pBITMAPHANDLE  pBitmap,
                                          L_UINT *  puMemory,
                                          L_SSIZE_T *  puTileSize,
                                          L_UINT *  puTotalTiles,
                                          L_UINT *  puConvTiles,
                                          L_UINT *  puMaxTileViews,
                                          L_UINT *  puTileViews);
      static L_INT GetBitmapMemoryInfo( LBitmapBase  *plBitmap,
                                          L_UINT *  puMemory,
                                          L_SSIZE_T *  puTileSize,
                                          L_UINT *  puTotalTiles,
                                          L_UINT *  puConvTiles,
                                          L_UINT *  puMaxTileViews,
                                          L_UINT *  puTileViews);

};

#endif //_LEAD_FILE_H_
/*================================================================= EOF =====*/
