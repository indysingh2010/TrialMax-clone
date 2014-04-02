/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcHttp.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef _LEAD_INET_HTTP_H_
#define _LEAD_INET_HTTP_H_

class LInetHttp;
/*----------------------------------------------------------------------------+
| Class     : LInetHttp                                                       |
| Desc      :                                                                 |
| Return    :                                                                 | 
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2000                                                     |
+----------------------------------------------------------------------------*/
//-----------------------------------------------------------------------------
class LWRP_EXPORT LInetHttp : public LBase
//-----------------------------------------------------------------------------
{
   LEAD_DECLAREOBJECT(LInetHttp);

// Data Members
private:

   HINET          m_hInetHttp;
   L_BOOL         m_bOpenReq;

// Member Functions
public:
            LInetHttp();
            LInetHttp(L_TCHAR *pszServer, L_TCHAR *pszUserName = NULL, L_TCHAR *pszPassword = NULL,
                      L_INT nPort = 80);

   virtual  ~LInetHttp();
   L_INT    Connect(L_TCHAR *pszServer, L_TCHAR *pszUserName = NULL, L_TCHAR *pszPassword = NULL,
                    L_INT nPort = 80);

   L_VOID   Disconnect();
   L_INT    OpenRequest(L_UINT uType, L_TCHAR *pszTarget, L_UINT dwFlags = 0, L_TCHAR *pszReferer = NULL,
                        L_TCHAR *pszVersion = NULL);

   L_VOID   CloseRequest();
   L_INT    SendBitmap(LBitmapBase *pBitmapBase, L_INT nFormat, L_INT nQFactor, L_TCHAR *pszContentType,
                       pNAMEVALUE pNameValue, pSAVEFILEOPTION pSaveFileOption = NULL);

   L_INT    SendBitmap(pBITMAPHANDLE pBitmap, L_INT nFormat, L_INT nBitsPerPixel, L_INT nQFactor,
                       L_TCHAR *pszContentType, pNAMEVALUE pNameValue, pSAVEFILEOPTION pSaveFileOption = NULL);

   L_INT    SendData(LBuffer *pData, L_TCHAR *pszContentType, pNAMEVALUE pNameValue);
   L_INT    SendData(L_CHAR *pszData, L_UINT uSize, L_TCHAR *pszContentType, pNAMEVALUE pNameValue);
   L_INT    SendForm(pNAMEVALUE pNameValue, L_UINT uCount);
   L_INT    SendRequest(L_TCHAR *pszHeader, L_UINT32 ulHeaderSize, L_TCHAR *pszOptional,
                        L_UINT32 ulOptionalSize);
   
   L_INT    GetResponse(LBuffer *pData);
   L_UINT   GetServerStatus();
};

#endif //_LEAD_INET_HTTP_H_
/*================================================================= EOF =====*/
