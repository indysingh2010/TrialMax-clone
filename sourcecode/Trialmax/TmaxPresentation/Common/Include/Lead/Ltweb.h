/*************************************************************
   Ltweb.h
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTWEB_H)
#define LTWEB_H

#if !defined(L_LTWEB_API)
   #define L_LTWEB_API
#endif // #if !defined(L_LTWEB_API)

#include "ltkrn.h"
#include "ltfil.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

/****************************************************************
   Enums/defines/macros
****************************************************************/
typedef HGLOBAL HINET;
typedef HINET * pHINET;
typedef FILETIME L_FILETIME; //Specifies a FILETIME structure containing the time the file was created

typedef HGLOBAL HFTP;
typedef HFTP  * pHFTP;

#define FTP_FILE_ATTRIBUTE_ARCHIVE         FILE_ATTRIBUTE_ARCHIVE
#define FTP_FILE_ATTRIBUTE_COMPRESSED      FILE_ATTRIBUTE_COMPRESSED
#define FTP_FILE_ATTRIBUTE_DIRECTORY       FILE_ATTRIBUTE_DIRECTORY
#define FTP_FILE_ATTRIBUTE_ENCRYPTED       FILE_ATTRIBUTE_ENCRYPTED
#define FTP_FILE_ATTRIBUTE_HIDDEN          FILE_ATTRIBUTE_HIDDEN
#define FTP_FILE_ATTRIBUTE_NORMAL          FILE_ATTRIBUTE_NORMAL
#define FTP_FILE_ATTRIBUTE_OFFLINE         FILE_ATTRIBUTE_OFFLINE
#define FTP_FILE_ATTRIBUTE_READONLY        FILE_ATTRIBUTE_READONLY
#define FTP_FILE_ATTRIBUTE_REPARSE_POINT   FILE_ATTRIBUTE_REPARSE_POINT
#define FTP_FILE_ATTRIBUTE_SPARSE_FILE     FILE_ATTRIBUTE_SPARSE_FILE
#define FTP_FILE_ATTRIBUTE_SYSTEM          FILE_ATTRIBUTE_SYSTEM
#define FTP_FILE_ATTRIBUTE_TEMPORARY       FILE_ATTRIBUTE_TEMPORARY

#define SENDAS_ASCII    0
#define SENDAS_BINARY   1

#define HTTP_GET        0
#define HTTP_PUT        1
#define HTTP_POST       2

#define L_HTTP_STATUS_OK                  200 // request completed
#define L_HTTP_STATUS_CREATED             201 // object created, reason = new URI
#define L_HTTP_STATUS_ACCEPTED            202 // async completion (TBS)
#define L_HTTP_STATUS_PARTIAL             203 // partial completion
#define L_HTTP_STATUS_NO_CONTENT          204 // no info to return
#define L_HTTP_STATUS_RESET_CONTENT       205 // request completed, but clear form
#define L_HTTP_STATUS_PARTIAL_CONTENT     206 // partial GET furfilled

#define L_HTTP_STATUS_AMBIGUOUS           300 // server couldn't decide what to return
#define L_HTTP_STATUS_MOVED               301 // object permanently moved
#define L_HTTP_STATUS_REDIRECT            302 // object temporarily moved
#define L_HTTP_STATUS_REDIRECT_METHOD     303 // redirection w/ new access method
#define L_HTTP_STATUS_NOT_MODIFIED        304 // if-modified-since was not modified
#define L_HTTP_STATUS_USE_PROXY           305 // redirection to proxy, location header specifies proxy to use
#define L_HTTP_STATUS_REDIRECT_KEEP_VERB  307 // HTTP/1.1: keep same verb

#define L_HTTP_STATUS_BAD_REQUEST         400 // invalid syntax
#define L_HTTP_STATUS_DENIED              401 // access denied
#define L_HTTP_STATUS_PAYMENT_REQ         402 // payment required
#define L_HTTP_STATUS_FORBIDDEN           403 // request forbidden
#define L_HTTP_STATUS_NOT_FOUND           404 // object not found
#define L_HTTP_STATUS_BAD_METHOD          405 // method is not allowed
#define L_HTTP_STATUS_NONE_ACCEPTABLE     406 // no response acceptable to client found
#define L_HTTP_STATUS_PROXY_AUTH_REQ      407 // proxy authentication required
#define L_HTTP_STATUS_REQUEST_TIMEOUT     408 // server timed out waiting for request
#define L_HTTP_STATUS_CONFLICT            409 // user should resubmit with more info
#define L_HTTP_STATUS_GONE                410 // the resource is no longer available
#define L_HTTP_STATUS_LENGTH_REQUIRED     411 // the server refused to accept request w/o a length
#define L_HTTP_STATUS_PRECOND_FAILED      412 // precondition given in request failed
#define L_HTTP_STATUS_REQUEST_TOO_LARGE   413 // request entity was too large
#define L_HTTP_STATUS_URI_TOO_LONG        414 // request URI too long
#define L_HTTP_STATUS_UNSUPPORTED_MEDIA   415 // unsupported media type

#define L_HTTP_STATUS_SERVER_ERROR        500 // internal server error
#define L_HTTP_STATUS_NOT_SUPPORTED       501 // required not supported
#define L_HTTP_STATUS_BAD_GATEWAY         502 // error response received from gateway
#define L_HTTP_STATUS_SERVICE_UNAVAIL     503 // temporarily overloaded
#define L_HTTP_STATUS_GATEWAY_TIMEOUT     504 // timed out waiting for gateway
#define L_HTTP_STATUS_VERSION_NOT_SUP     505 // HTTP version not supported

/****************************************************************
   Classes/structures
****************************************************************/
typedef struct tagNAMEVALUEA
{
   L_CHAR *pszName;
   L_CHAR *pszValue;
} NAMEVALUEA, * pNAMEVALUEA;

#if defined(FOR_UNICODE)
typedef struct tagNAMEVALUE
{
   L_TCHAR *pszName;
   L_TCHAR *pszValue;
} NAMEVALUE, * pNAMEVALUE;
#else
typedef NAMEVALUEA NAMEVALUE;
typedef pNAMEVALUEA pNAMEVALUE;
#endif // #if defined(FOR_UNICODE)

typedef struct tagFTPINFO
{
   LPVOID hSession;
   LPVOID hFtpConnection;
} FTPINFO, * pFTPINFO;

typedef struct tagFILEDATA
{
   L_UINT     uFileAttributes;
   L_FILETIME ftCreationTime;
   L_FILETIME ftLastAccessTime;
   L_FILETIME ftLastWriteTime;
   L_UINT32   uFileSize;
} FILEDATA, *pFILEDATA;

/****************************************************************
   Callback typedefs
****************************************************************/

typedef L_INT (* FTPBROWSECALLBACKA)(L_CHAR *pszFile, pFILEDATA pFileData, L_VOID *pUserData);

#if defined(FOR_UNICODE)
typedef L_INT (* FTPBROWSECALLBACK)(L_TCHAR *pszFile, pFILEDATA pFileData, L_VOID *pUserData);
#else
typedef FTPBROWSECALLBACKA FTPBROWSECALLBACK;
#endif // #if defined(FOR_UNICODE)

/****************************************************************
   Function prototypes
****************************************************************/

// FTP functions
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpConnectA(L_CHAR *pszServer,L_INT iPort,L_CHAR *pszUserName,
                                   L_CHAR *pszPassword,pHFTP pFtp);
#if defined(FOR_UNICODE)
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpConnect(L_TCHAR *pszServer,L_INT iPort,L_TCHAR *pszUserName,
                                   L_TCHAR *pszPassword,pHFTP pFtp);
#else
#define L_InetFtpConnect L_InetFtpConnectA
#endif // #if defined(FOR_UNICODE)

L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpDisConnect(HFTP hFtp);

L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpSendFileA(HFTP hFtp,L_CHAR *pszLocal,L_CHAR *pszRemote,L_UINT uSendAs);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpChangeDirA(HFTP hFtp,L_CHAR *pszDirectory);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpGetFileA(HFTP hFtp, L_CHAR *pszRemote, L_CHAR *pszLocal,
                                   L_BOOL bOverwrite, L_UINT uSendAs);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpRenameFileA(HFTP hFtp, L_CHAR *pszOld, L_CHAR *pszNew);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpDeleteFileA(HFTP hFtp, L_CHAR *pszRemote);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpCreateDirA(HFTP hFtp, L_CHAR *pszRemoteDir);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpDeleteDirA(HFTP hFtp, L_CHAR *pszRemoteDir);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpGetCurrentDirA(HFTP hFtp, L_CHAR *pszRemoteDir, L_UINT32 ulSize);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpSendBitmapA(HFTP hFtp, pBITMAPHANDLE pBitmap, L_INT nFormat, 
                                      L_INT nBitsPerPixel, L_INT nQFactor, pSAVEFILEOPTION pSaveOptions,
                                      L_CHAR *pszRemote, L_UINT uSendAs);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpBrowseDirA(HFTP hFtp, L_CHAR *pszSearch,FTPBROWSECALLBACKA pfnCallback, L_VOID *pData);

#if defined(FOR_UNICODE)
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpSendFile(HFTP hFtp,L_TCHAR *pszLocal,L_TCHAR *pszRemote,L_UINT uSendAs);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpChangeDir(HFTP hFtp,L_TCHAR *pszDirectory);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpGetFile(HFTP hFtp, L_TCHAR *pszRemote, L_TCHAR *pszLocal,
                                   L_BOOL bOverwrite, L_UINT uSendAs);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpRenameFile(HFTP hFtp, L_TCHAR *pszOld, L_TCHAR *pszNew);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpDeleteFile(HFTP hFtp, L_TCHAR *pszRemote);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpCreateDir(HFTP hFtp, L_TCHAR *pszRemoteDir);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpDeleteDir(HFTP hFtp, L_TCHAR *pszRemoteDir);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpGetCurrentDir(HFTP hFtp, L_TCHAR *pszRemoteDir, L_UINT32 ulSize);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpSendBitmap(HFTP hFtp, pBITMAPHANDLE pBitmap, L_INT nFormat, 
                                      L_INT nBitsPerPixel, L_INT nQFactor, pSAVEFILEOPTION pSaveOptions,
                                      L_TCHAR *pszRemote, L_UINT uSendAs);
L_LTWEB_API L_INT EXT_FUNCTION L_InetFtpBrowseDir(HFTP hFtp, L_TCHAR *pszSearch,FTPBROWSECALLBACK pfnCallback, L_VOID *pData);
#else
#define L_InetFtpSendFile L_InetFtpSendFileA
#define L_InetFtpChangeDir L_InetFtpChangeDirA
#define L_InetFtpGetFile L_InetFtpGetFileA
#define L_InetFtpRenameFile L_InetFtpRenameFileA
#define L_InetFtpDeleteFile L_InetFtpDeleteFileA
#define L_InetFtpCreateDir L_InetFtpCreateDirA
#define L_InetFtpDeleteDir L_InetFtpDeleteDirA
#define L_InetFtpGetCurrentDir L_InetFtpGetCurrentDirA
#define L_InetFtpSendBitmap L_InetFtpSendBitmapA
#define L_InetFtpBrowseDir L_InetFtpBrowseDirA
#endif // #if defined(FOR_UNICODE)

// HTTP functions

L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpConnectA(L_CHAR *pszServer,L_INT iPort,L_CHAR *pszUserName,
                                    L_CHAR *pszPassword,pHINET pHttp);

#if defined(FOR_UNICODE)
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpConnect(L_TCHAR *pszServer,L_INT iPort,L_TCHAR *pszUserName,
                                    L_TCHAR *pszPassword,pHINET pHttp);
#else
#define L_InetHttpConnect L_InetHttpConnectA
#endif // #if defined(FOR_UNICODE)

L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpDisconnect(HINET hHttp);

L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpOpenRequestA(HINET hHttp,L_UINT uType,L_CHAR *pszTarget,
                                        L_CHAR *pszReferer,L_CHAR *pszVersion,
                                        L_UINT32 dwReserved);
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpOpenRequestExA(HINET hHttp,L_UINT uType,L_CHAR *pszTarget,
                                          L_CHAR *pszReferer,L_CHAR *pszVersion,
                                          L_UINT32 dwReserved, L_UINT32 uFlags);
#if defined(FOR_UNICODE)
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpOpenRequest(HINET hHttp,L_UINT uType,L_TCHAR *pszTarget,
                                        L_TCHAR *pszReferer,L_TCHAR *pszVersion,
                                        L_UINT32 dwReserved);
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpOpenRequestEx(HINET hHttp,L_UINT uType,L_TCHAR *pszTarget,
                                          L_TCHAR *pszReferer,L_TCHAR *pszVersion,
                                          L_UINT32 dwReserved, L_UINT32 uFlags);
#else
#define L_InetHttpOpenRequest L_InetHttpOpenRequestA
#define L_InetHttpOpenRequestEx L_InetHttpOpenRequestExA
#endif // #if defined(FOR_UNICODE)
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpCloseRequest(HINET hHttp);

L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpSendRequestA(HINET hHttp,L_CHAR *pszHeader,L_UINT32 ulHeaderSize,
                                        L_CHAR *pszOptional,L_UINT32 ulOptionalSize);
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpSendBitmapA(HINET hHttp,pBITMAPHANDLE pBitmap,L_INT nFormat,
                                       L_INT nBitsPerPixel,L_INT nQFactor,
                                       L_CHAR *pszContentType,pNAMEVALUEA pNameValue,
                                       pSAVEFILEOPTION pSaveOptions);
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpSendDataA(HINET hHttp,L_CHAR *pData, L_UINT32 uSize,
                                     L_CHAR *pszContentType,pNAMEVALUEA pNameValue);

L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpSendFormA(HINET hHttp,pNAMEVALUEA pNameValue,L_UINT uCount);

#if defined(FOR_UNICODE)
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpSendRequest(HINET hHttp,L_TCHAR *pszHeader,L_UINT32 ulHeaderSize,
                                        L_TCHAR *pszOptional,L_UINT32 ulOptionalSize);
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpSendBitmap(HINET hHttp,pBITMAPHANDLE pBitmap,L_INT nFormat,
                                       L_INT nBitsPerPixel,L_INT nQFactor,
                                       L_TCHAR *pszContentType,pNAMEVALUE pNameValue,
                                       pSAVEFILEOPTION pSaveOptions);
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpSendData(HINET hHttp,L_CHAR *pData, L_UINT32 uSize,
                                     L_TCHAR *pszContentType,pNAMEVALUE pNameValue);

L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpSendForm(HINET hHttp,pNAMEVALUE pNameValue,L_UINT uCount);
#else
#define L_InetHttpSendRequest L_InetHttpSendRequestA
#define L_InetHttpSendBitmap  L_InetHttpSendBitmapA
#define L_InetHttpSendData    L_InetHttpSendDataA
#define L_InetHttpSendForm    L_InetHttpSendFormA
#endif // #if defined(FOR_UNICODE)


L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpGetResponse(HINET hHttp,L_CHAR *pszData,L_UINT32 *ulSize);
L_LTWEB_API L_INT EXT_FUNCTION L_InetHttpGetServerStatus(HINET hHttp,L_UINT *uStatus);

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTWEB_H)