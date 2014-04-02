/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcWia.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_WIA_H_
#define  _LEAD_WIA_H_

#if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LWia                                                            |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 16 December 2007                                                |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LWia : public LScanner
{
   LEAD_DECLAREOBJECT(LWia);

private:
   HWIASESSION m_hSession;

private:
   static L_INT  EXT_CALLBACK WiaEnumDevicesCS(HWIASESSION hSession, pLWIADEVICEID pDeviceID, L_VOID * pUserData);
   static L_INT  EXT_CALLBACK WiaAcquireCS(HWIASESSION hSession, pBITMAPHANDLE pBitmap, L_TCHAR * pszFilename, L_UINT32 uPercent, L_UINT32 uFlags, L_VOID * pUserData);
   static L_INT  EXT_CALLBACK WiaAcquireFileCS(HWIASESSION hSession, L_TCHAR * pszFilename, L_UINT32 uPercent, L_UINT32 uFlags, L_VOID * pUserData);
   static L_INT  EXT_CALLBACK WiaEnumItemsCS(HWIASESSION hSession, L_INT nItemsCount, L_VOID * pItem, L_VOID * pUserData);
   static L_INT  EXT_CALLBACK WiaSetPropertiesCS(HWIASESSION hSession, L_INT PropertyID, L_INT nError, L_UINT uValueType, L_VOID * pValue, L_VOID * pUserData);
   static L_INT  EXT_CALLBACK WiaEnumCapabilitiesCS(HWIASESSION hSession, L_INT nCapsCount, pLWIACAPABILITY pCapability, L_VOID * pUserData);
   static L_INT  EXT_CALLBACK WiaEnumFormatsCS(HWIASESSION hSession, L_INT nFormatsCount, L_INT nTransferMode, GUID * pFormat, L_VOID * pUserData);

protected:
   virtual L_INT  EnumDevicesCallBack(pLWIADEVICEID pDeviceID);
   virtual L_INT  AcquireCallBack(pBITMAPHANDLE pBitmap, L_TCHAR * pszFilename, L_UINT32 uPercent, L_UINT32 uFlags);
   virtual L_INT  AcquireFileCallBack(L_TCHAR * pszFilename, L_UINT32 uPercent, L_UINT32 uFlags);
   virtual L_INT  EnumItemsCallBack(L_INT nItemsCount, L_VOID * pItem);
   virtual L_INT  SetPropertiesCallBack(L_INT PropertyID, L_INT nError, L_UINT uValueType, L_VOID * pValue);
   virtual L_INT  EnumCapabilitiesCallBack(L_INT nCapsCount, pLWIACAPABILITY pCapability);
   virtual L_INT  EnumFormatsCallBack(L_INT nFormatsCount, L_INT nTransferMode, GUID * pFormat);

public : 
   LWia();
   virtual ~LWia();

   virtual L_BOOL    IsAvailable(L_UINT uWiaVersion);
   virtual L_INT     InitSession(L_UINT uWiaVersion);
   virtual L_INT     EndSession();
   virtual L_INT     EnumDevices();
   virtual L_INT     SelectDeviceDlg(L_UINT32 uDeviceType, L_UINT32 uFlags);
   virtual L_INT     SelectDevice(L_TCHAR* pszDeviceId);
   virtual L_TCHAR*  GetSelectedDevice();
   virtual L_INT     GetSelectedDeviceType(L_UINT32 * puDeviceType);
   virtual L_INT     GetProperties(L_VOID * pItem, pLWIAPROPERTIES pProperties);
   virtual L_INT     SetProperties(L_VOID * pItem, pLWIAPROPERTIES pProperties);
   virtual L_INT     Acquire(L_UINT32 uFlags, L_VOID * pSourceItem, pLWIAACQUIREOPTIONS pAcquireOptions, L_INT * pnFilesCount, L_TCHAR *** pppszFilePaths);
   virtual L_INT     AcquireToFile(L_UINT32 uFlags, L_VOID * pSourceItem, pLWIAACQUIREOPTIONS pAcquireOptions, L_INT * pnFilesCount, L_TCHAR *** pppszFilePaths);
   virtual L_INT     AcquireSimple(L_UINT uWiaVersion, L_UINT32 uDeviceType, L_UINT32 uFlags, L_VOID * pSourceItem, pLWIAACQUIREOPTIONS pAcquireOptions, L_INT * pnFilesCount, L_TCHAR *** pppszFilePaths);
   virtual L_INT     StartVideoPreview(L_BOOL bStretchToFitParent);
   virtual L_INT     ResizeVideoPreview(L_BOOL bStretchToFitParent);
   virtual L_INT     EndVideoPreview();
   virtual L_INT     AcquireImageFromVideo(L_TCHAR* pszFileName, L_SIZE_T* puLength);
   virtual L_BOOL    IsVideoPreviewAvailable();
   virtual L_INT     GetRootItem(L_VOID * pItem, L_VOID ** ppWiaRootItem);
   virtual L_INT     EnumChildItems(L_VOID * pWiaRootItem);
   virtual L_INT     FreeItem(L_VOID * pItem);
   virtual L_INT     GetPropertyLong(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_INT32* plValue);
   virtual L_INT     SetPropertyLong(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_INT32 lValue);
   virtual L_INT     GetPropertyGUID(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, GUID* pGuidValue);
   virtual L_INT     SetPropertyGUID(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, GUID* pGuidValue);
   virtual L_INT     GetPropertyString(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_TCHAR* pszValue, L_SIZE_T* puLength);
   virtual L_INT     SetPropertyString(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_TCHAR* pszValue);
   virtual L_INT     GetPropertySystemTime(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, LPSYSTEMTIME pValue);
   virtual L_INT     SetPropertySystemTime(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, LPSYSTEMTIME pValue);
   virtual L_INT     GetPropertyBuffer(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_UCHAR* pValue, L_SIZE_T* puSize);
   virtual L_INT     SetPropertyBuffer(L_VOID * pItem, L_TCHAR* pszID, L_UINT32 uID, L_UCHAR* pValue, L_SIZE_T uSize);
   virtual L_INT     EnumCapabilities(L_VOID * pItem, L_UINT uFlags);
   virtual L_INT     EnumFormats(L_VOID * pItem, L_UINT uFlags);
};
#endif // #if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)

#endif //_LEAD_WIA_H_
/*================================================================= EOF =====*/