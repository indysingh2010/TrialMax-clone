/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcInet.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef _LEAD_INET_CAPTURE_H_
#define _LEAD_INET_CAPTURE_H_

/************************************************************************************
 *                                                                                  *
 *                            CONNECTION LIST FLAGS                                 *
 *                                                                                  *
 ************************************************************************************/

#define  COMPUTER_DESC     0x0001           /* Description column only  */
#define  COMPUTER_ADDRESS  0x0002           /* Address column only      */

#define  SERVER            0x0001           /* Server type */
#define  CLIENT            0x0002           /* Client type */

#include "commctrl.h"
#include "ltcDictn.h"
//#include "ltcMmcp.h"

/*----------------------------------------------------------------------------+
| FORWARDS                                                                     |
+----------------------------------------------------------------------------*/
class LInetPacket;

/*----------------------------------------------------------------------------+
| DEFINES                                                                     |
+----------------------------------------------------------------------------*/
/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/
class LInet;
/*----------------------------------------------------------------------------+
| Class     : LInet                                                           |
| Desc      :                                                                 |
| Return    :                                                                 | 
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : June 1999                                                       |
+----------------------------------------------------------------------------*/
//-----------------------------------------------------------------------------
class LWRP_EXPORT LInet : public LBase
//-----------------------------------------------------------------------------
{
   LEAD_DECLAREOBJECT(LInet);

   public:
      L_VOID *m_extLInet;

private:   
   static   L_UINT   m_uStartedCount;
   static   HICON    m_hDefSrvIcon;
   static   HICON    m_hDefClnIcon;

   L_BOOL            m_bStarted;
   L_BOOL            m_bAutoWnd;
   L_BOOL            m_bEnableAutoProcess;
   L_UINT            m_uType;
   L_UINT            m_uColumns;
   HTREEITEM         m_hRemoteItem;
   HTREEITEM         m_hTreeServer;
   HTREEITEM         m_hTreeClient;
   HWND              m_hCtrlWnd;
   HICON             m_hTempSrvIcon;
   HICON             m_hTempClnIcon;
   LDictionary       m_ConnectionList;
//   LMMCapture        m_MMCapture;
   L_CHAR           *m_pszName;
   L_CHAR           *m_pszIP;
   L_BOOL            m_bWndItem;
   LPWAVEFORMATDATA  m_pRemoteWaveFormat;
   LPWAVEFORMATDATA  m_pBadWaveFormat;
   L_UINT32          m_uWaveFormatSize;
   L_UINT32          m_uBadWaveFormatSize;

protected:
   L_COMP            m_hComputer;
   L_COMP            m_hServer;
   HWND              m_hImgWnd;

public:
   L_VOID            *m_pUserDataEx;

private:
   L_BOOL            IsServer();
   L_VOID            FillWnd(L_UINT uColumns);
   L_VOID            FillIcon(HICON hSrvIcon, HICON hClnIcon);
   L_VOID            InsertInWnd(HTREEITEM hOrder, LInet *plRemoteComp);
   L_VOID            RemoveFromWnd(HTREEITEM hOrder);
   L_UINT            GetType();
   L_INT             SetHandle(L_COMP hComputer);
   L_VOID            CloseServers(L_BOOL bGraceful);
   L_VOID            CloseClients(L_BOOL bGraceful);
   L_INT             PrepareTree();


   static   L_INT EXT_CALLBACK InetCS(L_COMP hConnection, L_INT nMessage, L_INT nError,
                         L_CHAR *pBuffer, L_SIZE_T ulSize, L_VOID *pUserData);

   static   L_VOID EXT_CALLBACK CommandCS(
      L_COMP      hComputer,   
      CMDTYPE     uCommand,
      L_UINT      uCommandID,
      L_INT       nError,
      L_UINT      uParams, 
      pPARAMETER  pParams, 
      L_UINT      uExtra,
      L_CHAR	 *pExtra,
      L_VOID	 *pUserData
      );

   static L_VOID EXT_CALLBACK ResponseCS(
   L_COMP      hComputer,
   CMDTYPE     uCommand,
   L_UINT      uCommandID,
   L_INT       nError,
   L_INT       nStatus,
   L_UINT      uParams, 
   pPARAMETER  pParams, 
   L_UINT      uExtra, 
   L_CHAR	  *pExtra,
   L_VOID	  *pUserData
);


protected:
   virtual  L_INT OnConnect(LInet *plConnection, L_INT nError);
   virtual  L_INT OnDataReady(LInet *plConnection, L_INT nError, L_CHAR *pBuffer, L_SIZE_T ulSize);
   virtual  L_INT OnClose(LInet *plConnection, L_INT nError);
   virtual  L_INT OnConnectRequest(LInet *plConnection, L_INT nError);
   virtual  L_INT OnDataSent(LInet *plConnection, L_INT nError);
   virtual  L_INT OnDataSending(LInet *plConnection, L_INT nError);
   virtual  L_INT OnDataStart(LInet *plConnection, L_INT nError);
   virtual  L_INT OnDataReceiving(LInet *plConnection, L_INT nError, L_CHAR *pBuffer, L_SIZE_T ulSize);
   virtual  L_INT OnImageReceived(LInet *plConnection, L_INT nError, LBitmapBase *pBitmap);
//   virtual  L_INT OnMMediaReceived(LInet *plConnection, L_INT nError, LMultimedia *pMedia);
//   virtual  L_INT OnSoundReceived(LInet *plConnection, L_INT nError,
//                                  LPWAVEFORMATDATA pWaveFormatData, L_UINT32 ulWaveFormatSize,
//                                  LPWAVEDATA pWaveData);
   virtual  L_INT OnUser1Received(LInet *plConnection, L_INT nError, L_CHAR *pBuffer, L_SIZE_T ulSize);
   virtual  L_INT OnUser2Received(LInet *plConnection, L_INT nError, L_CHAR *pBuffer, L_SIZE_T ulSize);
   virtual  L_INT OnUser3Received(LInet *plConnection, L_INT nError, L_CHAR *pBuffer, L_SIZE_T ulSize);
   virtual  L_INT OnUser4Received(LInet *plConnection, L_INT nError, L_CHAR *pBuffer, L_SIZE_T ulSize);

   virtual  L_INT CommandCallBack(LInet * plConnection,
                                  CMDTYPE     uCommand,
                                  L_UINT      uCommandID,
                                  L_INT       nError,
                                  L_UINT      uParams, 
                                  pPARAMETER  pParams, 
                                  L_UINT      uExtra,
                                  L_CHAR	 *pExtra);

   virtual L_INT ResponseCallBack(LInet * plConnection,
                                  CMDTYPE     uCommand,
                                  L_UINT      uCommandID,
                                  L_INT       nError,
                                  L_INT       nStatus,
                                  L_UINT      uParams, 
                                  pPARAMETER  pParams, 
                                  L_UINT      uExtra,
                                  L_CHAR	 *pExtra);
   
public:
   LInet();
   virtual        ~LInet();
   LInet&         operator=(LInet& LInetSrc);
   virtual  L_INT Connect(const L_CHAR *pszAddress, L_INT nPort);
   virtual  L_INT Close(LInet *plRemoteComp, L_BOOL bGraceful = FALSE);
   virtual  L_INT AcceptConnect(LInet *plRemoteComp);
   virtual  L_INT ServerInit(L_INT nPort);
   virtual  L_INT ServerShutdown();
   L_INT    CloseAll(L_UINT uType = SERVER | CLIENT, L_BOOL bGraceful = FALSE);
   L_CHAR   *GetHostName(L_INT nType = HOST_NAME_DESCRP);
   L_INT    StartUp (L_VOID);
   L_INT    ShutDown(L_VOID);
   L_INT    EnableAutoProcess(L_BOOL bProcess = TRUE);
   L_BOOL   IsAutoProcessEnabled();
   L_INT    ReadData(LInet *plRemoteComp, L_CHAR *pBuffer, L_UINT32 *pulBufferLength);
   L_INT    SendBitmap(LInet *plRemoteComp, pBITMAPHANDLE phBitmap, L_INT nFormat, L_INT nBitsPerPixel, L_INT nQFactor, L_SIZE_T *pulImageLength);
   
   L_INT    SendData(LInet *plRemoteComp, L_CHAR *pBuffer, L_SIZE_T *pulBufferLength, IDATATYPE uDataType);
   L_INT    SendMMData(LInet *plRemoteComp, L_CHAR *pBuffer, L_SIZE_T *pulBufferLength);
   L_INT    SendRawData(LInet *plRemoteComp, L_CHAR *pBuffer, L_SIZE_T *pulBufferLength);
   L_INT    SendSound(LInet *plRemoteComp, LPWAVEFORMATDATA pWaveFormatData, LPWAVEDATA pWaveData, L_SIZE_T *puldwDataSize);
   L_SIZE_T GetQueueSize();
   L_INT    ClearQueue();
   L_COMP   *GetHandle();
   L_BOOL   IsValid();
   L_INT    ExistsItem(LInet *plRemoteComp);
   LInet	*GetItem(L_INT uIndex);
   LInet	*GetItem(L_COMP* phComputer);
   LInet	*GetFirstItem(LInet *plRemoteComp = NULL);
   LInet	*GetLastItem(LInet *plRemoteComp = NULL);
   LInet	*GetNextItem(LInet *plRemoteComp, L_BOOL bByType = FALSE);
   LInet	*GetPrevItem(LInet *plRemoteComp, L_BOOL bByType = FALSE);
   L_UINT   GetItemsCount();
   L_INT    CreateWnd(HWND hParentWnd, pCONLISTOPTIONS pOptions, L_INT nID = 0,
					L_UINT32 uStyles = WS_VISIBLE, L_INT nX = 0, L_INT nY = 0,
					L_INT nCx = 320, L_INT nCy = 200);
   L_VOID   DestroyWnd();
   L_INT    ExpandWnd(L_BOOL bFlag = TRUE);
   L_BOOL   AddWndItem(LInet *plRemoteComp);
   L_VOID   EnableAutoWnd(L_BOOL bFlag = TRUE);
   L_BOOL   IsAutoWndEnabled();
   L_VOID   RemoveWndItem(LInet *plRemoteComp);
   L_VOID   RemoveAllWndItems();
   L_BOOL   IsWndItem(LInet *plConnection);
   L_VOID   SetImageWindow(HWND hWnd);
   
   //Version 12 functions
   L_INT    SendCmd( 
      LInet *plRemoteComp, 
      CMDTYPE uCommand,
      L_UINT uCommandID,
      LInetPacket *pInetPacket=NULL
      );
   
   L_INT    SendRsp(
      LInet *plRemoteComp, 
      CMDTYPE uCommand,
      L_UINT uCommandID,
      LInetPacket *pInetPacket,
      L_INT nStatus);
   
   L_INT    SendLoadCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_TCHAR *pszFile,
      L_INT nBitsPerPixel = 0,
      L_INT nOrder = ORDER_BGR,
      L_UINT uFlags = LOADFILE_ALLOCATE|LOADFILE_STORE);
   
   L_INT    SendLoadRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uBitmapID,
      L_UINT uExtra,
      L_CHAR *pExtra,
      L_INT nStatus);
   
   L_INT  SendCreateWinCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_TCHAR *pszClassName,
      L_TCHAR *pszWindowName,
      L_UINT ulFlags,
      L_INT nLeft,
      L_INT nTop,
      L_INT nWidth, 
      L_INT nHeight,
      L_UINT uParentID);
   
   L_INT  SendCreateWinRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      LONG_PTR uWindowID,
      L_UINT uLength,
      L_CHAR *pExtraInfo,
      L_INT nStatus);
   
   L_INT  SendAttachBitmapCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uBitmapID,
      LONG_PTR uWindowID);
   
   L_INT  SendAttachBitmapRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uExtra,
      L_CHAR *pExtra,
      L_INT nStatus);  

   L_INT  SendSaveCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_TCHAR *pszFile,
      L_UINT uBitmapID,
      L_INT nFormat,
      L_INT nBitsPerPixel = 0, 
      L_INT nQFactor = 2,
      L_UINT uFlags = SAVEFILE_OPTIMIZEDPALETTE);

   L_INT SendSaveRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uExtra,
      L_CHAR *pExtra,
      L_INT nStatus);

/** NEW **/

 L_INT SendSizeWinCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      LONG_PTR uWindowID,
      L_INT nLeft,
      L_INT nTop,
      L_INT nWidth,
      L_INT nHeight);
   
   L_INT SendSizeWinRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uLength,
      L_CHAR *pExtraInfo,
      L_INT nStatus);
   
   L_INT SendShowWinCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      LONG_PTR uWindowID,
      L_INT nCmdShow);
   
   L_INT SendShowWinRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uLength,
      L_CHAR *pExtraInfo,
      L_INT nStatus);
   
   L_INT SendCloseWinCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      LONG_PTR uWindowID);
   
   L_INT SendCloseWinRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uLength,
      L_CHAR *pExtraInfo,
      L_INT nStatus);
   
   L_INT SendFreeBitmapCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uBitmapID);
   
   L_INT SendFreeBitmapRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uLength,
      L_CHAR *pExtraInfo,
      L_INT nStatus);
   
   L_INT SendSetRectCmd(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      LONG_PTR uWindowID,
      RECTTYPE nType,
      L_INT nLeft,
      L_INT nTop,
      L_INT nWidth,
      L_INT nHeight);
   
   L_INT SendSetRectRsp(
      LInet *plRemoteComp,
      L_UINT uCommandID,
      L_UINT uLength,
      L_CHAR *pExtraInfo,
      L_INT nStatus);

  /** NEW **/

  };


/*----------------------------------------------------------------------------+
| Class     : LInetPacket                                                     |
| Desc      :                                                                 |
| Return    :                                                                 | 
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+*/
class LWRP_EXPORT LInetPacket : public LBase
{
   LEAD_DECLAREOBJECT(LInetPacket);

   public:
      L_VOID *m_extLInetPacket;

private:
   HINETPACK       m_hPacket;
   L_UINT          m_uExtra;
   L_VOID		  *m_pExtra;
   L_UINT          m_uParamCount;
   pPARAMETER      m_pParamsCopy;
   
public : 
   LInetPacket();
   LInetPacket(L_UINT uExtra, L_VOID *pExtra, L_CHAR *pszFormat, ...);
   ~LInetPacket();

   HINETPACK GetHandle();
   L_INT SetExtraData(L_UINT uExtra, L_VOID *pExtra);
   L_INT SetFormats(L_CHAR *pszFormat, ...);
};

#endif //_LEAD_INET_CAPTURE_H_


/*================================================================= EOF =====*/