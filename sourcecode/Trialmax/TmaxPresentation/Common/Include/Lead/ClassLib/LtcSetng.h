/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcsetng.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_SETTINGS_H_
#define  _LEAD_SETTINGS_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LSettings                                                       |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 1998                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LSettings :public LBase 
{
   LEAD_DECLAREOBJECT(LSettings);

   public:
      L_VOID *m_extLSettings;
      
   public : 
      LSettings();
      virtual ~LSettings();

      static L_BOOL           IsSupportLocked(L_UINT uType);   
      static L_VOID           UnlockSupport(L_UINT uType,L_TCHAR * pKey);
};

/*----------------------------------------------------------------------------+
| Class     : LFileSetting                                                    |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 26 may 1998                                                     |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LFileSettings: public LSettings
{
   LEAD_DECLAREOBJECT(LFileSettings);

   public:
      L_VOID *m_extLFileSettings;
      
   public : 
      LFileSettings();
      virtual ~LFileSettings();

      static L_INT   GetWMFResolution(L_INT * lpXResolution, L_INT * lpYResolution);
      static L_INT   SetWMFResolution(L_INT nXResolution = 0, L_INT nYResolution = 0);
      static L_INT   IgnoreFilters(L_TCHAR * pszFilters); 
      static L_INT   GetPCDResolution(L_TCHAR * pszFile, pPCDINFO pPCDInfo);  
      static L_INT   SetPCDResolution(L_INT nResolution);  

      static L_INT   PreLoadFilters(
                                    L_INT nFixedFilters,
                                    L_INT nCachedFilters,
                                    L_TCHAR * pszFilters
                                   );  
      static L_INT   GetComment(
                                 L_UINT uType,
                                 L_UCHAR * pComment,
                                 L_UINT uLength
                               ); 
      static L_INT   SetComment(
                                 L_UINT uType,
                                 L_UCHAR * pComment,
                                 L_UINT  uLength
                               );  
      static L_INT   GetLoadResolution(
                                    L_INT nFormat,
                                    L_UINT32 * pWidth,
                                    L_UINT32 * pHeight,
                                    pLOADFILEOPTION pLoadFileOption=NULL
                                 ); 
      static L_INT   SetLoadResolution(
                                    L_INT nFormat,
                                    L_UINT32  nWidth,
                                    L_UINT32 nHeight
                                 );  
      static L_INT GetTag(
                              L_UINT16 uTag,
                              L_UINT16 * pType,
                              L_UINT * pCount,
                              L_VOID *  pData
                           );  
      static L_INT   SetTag(
                              L_UINT16 uTag,
                              L_UINT16 uType,
                              L_UINT32  uCount,
                              L_VOID * pData
                           );  

      static L_INT  SetSaveResolution (L_UINT uCount, 
                                       pDIMENSION pResolutions);

      static L_INT GetSaveResolution (L_UINT * puCount, pDIMENSION pResolutions);

      static L_INT SetViewport2D(L_INT nWidth, L_INT nHeight);
      static L_INT GetViewport2D(L_INT *pnWidth,L_INT *pnHeight);
      static L_INT SetViewMode2D(L_INT nViewMode);
      static L_INT GetViewMode2D();

      static L_INT GetJ2KOptions( pFILEJ2KOPTIONS pOptions, L_UINT uStructSize );
      static L_INT GetDefaultJ2KOptions( pFILEJ2KOPTIONS pOptions, L_UINT uStructSize  );
      static L_INT SetJ2KOptions( const pFILEJ2KOPTIONS pOptions);

      static L_INT GetPLTOptions( pFILEPLTOPTIONS pOptions, L_UINT uStructSize  );
      static L_INT SetPLTOptions( const pFILEPLTOPTIONS pOptions);

      static L_INT GetPDFOptions( pFILEPDFOPTIONS pOptions, L_UINT uStructSize  );
      static L_INT SetPDFOptions( const pFILEPDFOPTIONS pOptions);

      static L_INT GetRTFOptions( pFILERTFOPTIONS pOptions, L_UINT uStructSize  );
      static L_INT SetRTFOptions( const pFILERTFOPTIONS pOptions);
      
      static L_INT GetPTKOptions( pFILEPTKOPTIONS pOptions, L_UINT uStructSize );
      static L_INT SetPTKOptions( const pFILEPTKOPTIONS pOptions);
 
      static L_INT GetDJVOptions( pFILEDJVOPTIONS pOptions, L_UINT uStructSize );
      static L_INT SetDJVOptions( const pFILEDJVOPTIONS pOptions);

      static L_INT GetPDFSaveOptions( pFILEPDFSAVEOPTIONS pOptions, L_UINT uStructSize );
      static L_INT SetPDFSaveOptions( const pFILEPDFSAVEOPTIONS pOptions);

      static L_INT GetPDFInitDir( L_TCHAR * pszInitDir, L_UINT uBufSize );
      static L_INT SetPDFInitDir( L_TCHAR * pszInitDir );
      
      static L_INT GetAutoCADFilesColorScheme(L_UINT32 * dwFlags);
      static L_INT SetAutoCADFilesColorScheme(L_UINT32 dwFlags);

      static L_INT GetTempDirectory( L_TCHAR *pszTempDir, L_UINT uSize);
      static L_INT SetTempDirectory( L_TCHAR *pszTempDir);

      static L_SSIZE_T GetIgnoreFilters(L_TCHAR * pszFilters, L_SIZE_T uSize);
      static L_SSIZE_T GetPreLoadFilters(L_TCHAR * pszFilters,
                                     L_SIZE_T uSize,
                                     L_INT * pnFixedFilters,
                                     L_INT * pnCachedFilters);  

      static L_INT SetTXTOptions (const pFILETXTOPTIONS pOptions);
      static L_INT GetTXTOptions (pFILETXTOPTIONS pOptions, L_UINT uStructSize);

      static L_INT GetJBIG2Options ( pFILEJBIG2OPTIONS pOptions, L_UINT uStructSize );
      static L_INT SetJBIG2Options ( pFILEJBIG2OPTIONS pOptions);
      static L_INT GetXPSOptions( pFILEXPSOPTIONS pOptions, L_UINT uStructSize  );
      static L_INT SetXPSOptions( const pFILEXPSOPTIONS pOptions);
};

/*----------------------------------------------------------------------------+
| Class     : LBitmapSetting                                                  |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 26 may 1998                                                     |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LBitmapSettings: public LSettings
{
   LEAD_DECLAREOBJECT(LBitmapSettings);

   public:
      L_VOID *m_extLBitmapSettings;
      
   public : 
      LBitmapSettings();
      virtual ~LBitmapSettings();

      static L_INT            DefaultDithering(L_UINT uMethod);  
      static L_UINT32         GetDisplayMode();       
      static L_UINT32         SetDisplayMode(
                                             L_UINT32 uFlagPos,
                                             L_UINT32 uFlagSet
                                            );      
      static L_INT            FreeUserMatchTable(L_UINT * pTable); 
      static L_UINT *   SetUserMatchTable(L_UINT * pTable);  
      static L_UINT *   CreateUserMatchTable(
                                                   LPRGBQUAD pPalette,
                                                   L_UINT uColors
                                                  ); 
};      

#endif //_LEAD_SETTINGS_H_
/*================================================================= EOF =====*/
