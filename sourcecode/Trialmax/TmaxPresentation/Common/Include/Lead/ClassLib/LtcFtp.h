/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcFtp.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef _LEAD_INET_FTP_H_
#define _LEAD_INET_FTP_H_

class LInetFtp;
/*----------------------------------------------------------------------------+
| Class     : LInetFtp                                                        |
| Desc      :                                                                 |
| Return    :                                                                 | 
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : August 2000                                                     |
+----------------------------------------------------------------------------*/
//-----------------------------------------------------------------------------
class LWRP_EXPORT LInetFtp : public LBase
//-----------------------------------------------------------------------------
{
   LEAD_DECLAREOBJECT(LInetFtp);

private:
// Data Members
   HFTP     m_hInetFtp;

   static   L_VOID EXT_CALLBACK BrowseCS(L_TCHAR *pszFile, L_INT32 FileAttrib, L_FILETIME CreateTime,
                            L_FILETIME AccessTime, L_FILETIME LastWriteTime, L_UINT32 uFileSize,
                            L_VOID *pUserData);

protected:
   // Callback Function
   virtual  L_INT  BrowseCallBack(L_TCHAR *pszFile, L_INT32 uFileAttrib, L_FILETIME ftCreateTime,
                                  L_FILETIME ftLastAccessTime, L_FILETIME ftLastWriteTime, L_UINT32 uFileSize);

public:
// Member Functions
            LInetFtp();
            LInetFtp(L_TCHAR *pszServer, L_TCHAR *pszUserName = NULL,
                     L_TCHAR *pszPassword = NULL, L_INT nPort = 21);

   virtual  ~LInetFtp();
   L_INT    Connect(L_TCHAR *pszServer, L_TCHAR *pszUserName = NULL,
                    L_TCHAR *pszPassword = NULL, L_INT nPort = 21);

   L_VOID   Disconnect();
   L_INT    SendFile(L_TCHAR *pszLocal, L_TCHAR *pszRemote, L_UINT uSendAs);
   L_INT    GetFile(L_TCHAR *pszRemote, L_TCHAR *pszLocal, L_UINT uSendAs,
                    L_BOOL bOverwrite = TRUE);
   L_INT    RenameFile(L_TCHAR *pszOldName, L_TCHAR *pszNewName);
   L_INT    RemoveFile(L_TCHAR *pszRemote);

   L_INT    CreateDir(L_TCHAR *pszDirectory);
   L_INT    DeleteDir(L_TCHAR *pszDirectory);
   L_INT    GetCurrentDir(L_TCHAR *pszDirectory, L_UINT ulSize = MAX_PATH);
   L_INT    BrowseDir(L_TCHAR *pszSearch);
   L_INT    ChangeDir(L_TCHAR *pszDirectory);

   L_INT    SendBitmap(LBitmapBase *pBitmapBase, L_INT nFormat, L_INT nQFactor, L_TCHAR *pszRemote,
                       L_UINT uSendAs, pSAVEFILEOPTION pSaveFileOption = NULL);

   L_INT    SendBitmap(pBITMAPHANDLE pBitmap, L_INT nFormat, L_INT nBitsPerPixel,
                       L_INT nQFactor, L_TCHAR *pszRemote, L_UINT uSendAs,
                       pSAVEFILEOPTION pSaveFileOption = NULL);
};

#endif //_LEAD_INET_FTP_H_
/*================================================================= EOF =====*/
