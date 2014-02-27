/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcscnnr.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_SCANNER_H_
#define  _LEAD_SCANNER_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LScanner                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 1998                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LScanner: public LBase
{
   LEAD_DECLAREOBJECT(LScanner);

   public:
      L_VOID *m_extLScanner;
      
   protected :
      LBitmapBase	   *   m_pBitmap;
      HWND                 m_hWnd;

   public : 
      LScanner();
      virtual ~LScanner();

      L_BOOL         IsValid();
      virtual L_VOID SetWindow(HWND hWnd);
      virtual L_VOID SetBitmap(LBitmapBase * pBitmap);
};

/*----------------------------------------------------------------------------+
| Class     : LScanner                                                        |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 26 may 1998                                                     |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LTwain : public LScanner
{
   LEAD_DECLAREOBJECT(LTwain);

   public:
      HTWAINSESSION m_hSession;
      
   private:  
      static L_INT  EXT_CALLBACK TwainBitmapCS(HTWAINSESSION hSession,pBITMAPHANDLE pBitmap, L_VOID * pUserData);
      static L_INT  EXT_CALLBACK TwainCapabilityCS(HTWAINSESSION hSession,L_UINT uCap, pTW_CAPABILITY pCapability, L_VOID * pUserData);
      static L_INT  EXT_CALLBACK TwainSetPropertyCS(HTWAINSESSION hSession,L_UINT uCap, L_INT nStatus, L_VOID * pValue, L_VOID * pUserData);
      static L_INT  EXT_CALLBACK TwainSaveCapCS(HTWAINSESSION hSession,pTW_CAPABILITY pCapability, L_VOID * pUserData);
      static L_INT  EXT_CALLBACK TwainSaveErrorCS(HTWAINSESSION hSession,pTW_CAPABILITY pCapability, L_UINT uError, L_VOID * pUserData);
      static L_INT  EXT_CALLBACK TwainFindFastConfigCS(HTWAINSESSION hSession,pFASTCONFIG pResConfig, L_VOID *pUserData);
      static L_INT  EXT_CALLBACK TwainSourceInfoCS(HTWAINSESSION hSession,pLTWAINSOURCEINFO pSourceInfo, L_VOID * pUserData);
      static L_VOID EXT_CALLBACK TwainAcquireCS(HTWAINSESSION hSession,L_INT nPage, L_TCHAR * pszFileName, L_BOOL bFinishScan, L_VOID *pUserData);

   protected:
      virtual L_INT  BitmapCallBack(pBITMAPHANDLE pBitmap);
      virtual L_INT  CapabilityCallBack(L_UINT uCap, pTW_CAPABILITY pCapability);
      virtual L_INT  SetPropertyCallBack(L_UINT uCap, L_INT nStatus, L_VOID * pValue);
      virtual L_INT  SaveCapCallBack(pTW_CAPABILITY pCapability);
      virtual L_INT  SaveErrorCallBack(pTW_CAPABILITY pCapability, L_UINT uError);
      virtual L_INT  FindFastConfigCallBack(pFASTCONFIG pResConfig);
      virtual L_INT  SourceInfoCallBack(pLTWAINSOURCEINFO pSourceInfo);
      virtual L_VOID AcquireCallBack(L_INT nPage, L_TCHAR * pszFileName, L_BOOL bFinishScan);

   public : 
      LTwain();
      virtual ~LTwain();

      virtual L_INT InitSession(pAPPLICATIONDATA pAppData);
      virtual L_INT EndSession();
      virtual L_INT SetProperties(pLTWAINPROPERTIES pltProperties, L_UINT uFlags);
      virtual L_INT GetProperties(pLTWAINPROPERTIES pltProperties, L_UINT uStructSize, L_UINT uFlags);
      virtual L_INT AcquireList(HBITMAPLIST hBitmap, L_TCHAR * lpszTemplateFile, L_UINT uFlags);
      virtual L_INT AcquireList(LBitmapList *BitmapList, L_TCHAR * lpszTemplateFile, L_UINT uFlags);
      virtual L_INT Acquire(pBITMAPHANDLE pBitmap, L_UINT uStructSize,  L_UINT uFlags, L_TCHAR * lpszTemplateFile);
      virtual L_INT Acquire(LBitmap *Bitmap, L_UINT uStructSize,L_UINT uFlags, L_TCHAR * lpszTemplateFile);
      virtual L_INT SelectSource(pLTWAINSOURCE pltSource);
      virtual L_INT QueryProperty(L_UINT uCapability, pLTWAINPROPERTYQUERY* ppltProperty, L_UINT uStructSize);
      virtual L_INT SetCapability(pTW_CAPABILITY pCapability, L_UINT uFlags);            
      virtual L_INT GetCapability(pTW_CAPABILITY pCapability, L_UINT uFlags);      
      virtual L_INT EnumCapabilities(L_UINT uFlags);      
      virtual L_INT TemplateDlg(L_TCHAR * lpszTemplateFile);
      virtual L_INT OpenTemplateFile(HTWAINTEMPLATEFILE * phFile, L_TCHAR * lpszTemplateFile, L_UINT uAccess);      
      virtual L_INT AddCapabilityToFile(HTWAINTEMPLATEFILE hFile, pTW_CAPABILITY pCapability);      
      virtual L_INT GetCapabilityFromFile(HTWAINTEMPLATEFILE hFile, pTW_CAPABILITY * ppCapability, L_UINT uIndex);
      virtual L_UINT GetNumOfCapsInFile(HTWAINTEMPLATEFILE hFile);
      virtual L_INT CloseTemplateFile(HTWAINTEMPLATEFILE hFile);
      virtual L_INT GetExtendedImageInfo(TW_EXTIMAGEINFO * ptwExtImgInfo);
      virtual L_INT AcquireMulti(L_TCHAR * pszBaseFileName,L_UINT uFlags,L_UINT uTransferMode,L_INT nFormat,L_INT nBitsPerPixel,L_BOOL bMultiPageFile,L_UINT32 uUserBufSize,L_BOOL bUsePrefferedBuffer);
      virtual L_INT FindFastConfig(L_TCHAR * pszWorkingFolder,L_UINT uFlags,L_INT nBitsPerPixel,L_INT nBufferIteration,pFASTCONFIG pInFastConfigs,L_INT nInFastConfigsCount,pFASTCONFIG* ppTestConfigs,L_INT* pnTestConfigsCount,pFASTCONFIG pOutBestConfig, L_UINT uStructSize);
      virtual L_INT GetScanConfigs(L_INT nBitsPerPixel,L_UINT uTransferMode,L_INT nBufferIteration,pFASTCONFIG* ppFastConfig, L_UINT uStructSize,L_INT* pnFastConfigCount);
      virtual L_INT FreeScanConfig(pFASTCONFIG *ppFastConfig,L_INT nFastConfigCount);
      virtual L_INT GetSources(L_UINT uFlags, L_UINT uStructSize);
      
      virtual L_INT CreateNumericContainerOneValue(TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uValue);
      virtual L_INT CreateNumericContainerRange(TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uMinValue, L_UINT32 uMaxValue, L_UINT32 uStepSize, L_UINT32 uDefaultValue, L_UINT32 uCurrentValue);
      virtual L_INT CreateNumericContainerArray(TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uNumOfItems, L_VOID * pData);
      virtual L_INT CreateNumericContainerEnum(TW_CAPABILITY * pCapability, LTWAINNUMERICTYPE Type, L_UINT32 uNumOfItems, L_UINT32 uCurrentIndex, L_UINT32 uDefaultIndex, L_VOID * pData);
      virtual L_INT GetNumericContainerValue(TW_CAPABILITY * pCapability, L_INT nIndex, L_VOID ** ppValue);
      virtual L_INT FreeContainer(TW_CAPABILITY * pCapability);
      virtual L_INT FreePropQueryStructure(pLTWAINPROPERTYQUERY * ppltProperty);
      virtual L_INT FreeExtendedImageInfoStructure(TW_EXTIMAGEINFO ** pptwExtImgInfo);
      virtual L_INT LockContainer(TW_CAPABILITY * pCapability, void ** ppContainer);
      virtual L_INT UnlockContainer(TW_CAPABILITY * pCapability);
      virtual L_INT GetNumericContainerItemType(TW_CAPABILITY * pCapability, L_INT * pnItemType);
      virtual L_INT GetNumericContainerINTValue(TW_CAPABILITY * pCapability, L_INT nIndex, L_INT * pnValue);
      virtual L_INT GetNumericContainerUINTValue(TW_CAPABILITY * pCapability, L_INT nIndex, L_UINT * puValue);
      virtual L_INT GetNumericContainerBOOLValue(TW_CAPABILITY * pCapability, L_INT nIndex, L_BOOL * pbValue);
      virtual L_INT GetNumericContainerFIX32Value(TW_CAPABILITY * pCapability, L_INT nIndex, TW_FIX32 * ptwFix);
      virtual L_INT GetNumericContainerFRAMEValue(TW_CAPABILITY * pCapability, L_INT nIndex, TW_FRAME * ptwFrame);
      virtual L_INT GetNumericContainerSTRINGValue(TW_CAPABILITY * pCapability, L_INT nIndex, TW_STR1024 twString);
#if defined(FOR_UNICODE)
      virtual L_INT GetNumericContainerUNICODEValue(TW_CAPABILITY * pCapability, L_INT nIndex, TW_UNI512 twUniCode);
#endif //#if defined(FOR_UNICODE)

      virtual L_BOOL IsAvailable();
      L_INT FastAcquire(L_TCHAR * pszBaseFileName,L_UINT uFlags,L_UINT uTransferMode,L_INT nFormat,L_INT nBitsPerPixel,L_BOOL bMultiPageFile,L_UINT32 uUserBufSize,L_BOOL bUsePrefferedBuffer);

   private:
      L_INT StartCapsNeg();
      L_INT EndCapsNeg();

   public:
       L_INT CancelAcquire();
       L_INT QueryFileSystem(FILESYSTEMMSG FileMsg, pTW_FILESYSTEM pTwFile);
       L_INT GetJPEGCompression(pTW_JPEGCOMPRESSION pTwJpegComp, L_UINT uFlag);
       L_INT SetJPEGCompression(pTW_JPEGCOMPRESSION pTwJpegComp, L_UINT uFlag);

       L_INT SetTransferOptions(pTRANSFEROPTIONS pTransferOpts);
       L_INT GetTransferOptions(pTRANSFEROPTIONS pTransferOpts, L_UINT uStructSize);
       L_INT GetSupportedTransferMode(L_UINT * pTransferModes);
       L_INT SetResolution(pTW_FIX32 pXRes, pTW_FIX32 pYRes);
       L_INT GetResolution(pTW_FIX32 pXRes, pTW_FIX32 pYRes);
       L_INT SetImageFrame(pTW_FRAME pFrame);
       L_INT GetImageFrame(pTW_FRAME pFrame);
       L_INT SetImageUnit(L_INT nUnit);
       L_INT GetImageUnit(L_INT * pnUnit);
       L_INT SetImageBitsPerPixel(L_INT nBitsPerPixel);
       L_INT GetImageBitsPerPixel(L_INT * pnBitsPerPixel);
       L_INT SetImageEffects(L_UINT32 ulFlags, pTW_FIX32 pBrightness, pTW_FIX32 pContrast, pTW_FIX32 pHighlight);
       L_INT GetImageEffects(L_UINT32 ulFlags, pTW_FIX32 pBrightness, pTW_FIX32 pContrast, pTW_FIX32 pHighlight);
       L_INT SetAcquirePageOptions(L_INT nPaperSize, L_INT nPaperDirection);
       L_INT GetAcquirePageOptions(L_INT * pnPaperSize, L_INT * pnPaperDirection);
       L_INT SetRGBResponse(pTW_RGBRESPONSE pRgbResponse, L_INT nBitsPerPixel, L_UINT uFlag);
       L_INT ShowProgress(L_BOOL bShow);
       L_INT EnableDuplex(L_BOOL bEnableDuplex);
       L_INT GetDuplexOptions(L_BOOL * pbEnableDuplex, L_INT * pnDuplexMode);
       L_INT SetMaxXferCount(L_INT nMaxXferCount);
       L_INT GetMaxXferCount(L_INT * pnMaxXferCount);
       L_INT StopFeeder();

       L_INT GetDeviceEventData(pTW_DEVICEEVENT pDeviceEvent);
       L_INT SetDeviceEventCapability(pTW_CAPABILITY pDeviceCap);
       L_INT GetDeviceEventCapability(pTW_CAPABILITY pDeviceCap);
       L_INT ResetDeviceEventCapability(pTW_CAPABILITY pDeviceCap);

#if defined(LEADTOOLS_V16_OR_LATER)
      L_INT GetCustomDSData(pTW_CUSTOMDSDATA pCustomData, L_TCHAR * pszFileName);
      L_INT SetCustomDSData(pTW_CUSTOMDSDATA pCustomData, L_TCHAR * pszFileName);
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

};
#endif //_LEAD_SCANNER_H_
/*================================================================= EOF =====*/



