/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1990, 1998 by LEAD Technologies, Inc.                         |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : tcprnt.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_WRAPPER_TYPEDEF_H_
#define  _LEAD_WRAPPER_TYPEDEF_H_

/*----------------------------------------------------------------------------+
| TYPEDEFS                                                                    |
+----------------------------------------------------------------------------*/

//--BITMAPSETTINGS-------------------------------------------------------------
typedef struct tagBITMAPSETTINGS
{
   L_UINT      uDitheringMethod;
   //L_INT       nPaintContrast;
   //L_UINT      uPaintGamma;
   //L_INT       nPaintIntensity;
   L_UINT32    uDisplayModeFlagPos;
   L_UINT32    uDisplayModeFlagSet;
} BITMAPSETTINGS, * pBITMAPSETTINGS;
#define soBITMAPSETTINGS sizeof(BITMAPSETTINGS)
//--LPLEADCREATEOBJECT---------------------------------------------------------
typedef LBase* (*LPLEADCREATEOBJECT) (L_VOID);

//--NOTIFICATION MESSAGES STRUCTURES-------------------------------------------
typedef struct tagMOUSEDATA
{  
   HWND                 hWnd;
   union
   {
      LBitmapWindow * pBitmapWindowClass;
      LVectorWindow * pVectorWindowClass;
   } Class;
   L_UINT32             fwKeys;
   L_INT16              xPos;
   L_INT16              yPos;
} MOUSEDATA, * pMOUSEDATA;
#define soMOUSEDATA sizeof(MOUSEDATA)

typedef struct tagKEYDATA
{
   HWND                 hWnd;
   union
   {
      LBitmapWindow * pBitmapWindowClass;
      LVectorWindow * pVectorWindowClass;
   } Class;
   L_INT32              nVirtKey;
   L_INT32              lKeyData;
}KEYDATA, * pKEYDATA;
#define soKEYDATA sizeof(KEYDATA)

typedef struct tagSIZEDATA
{
   HWND                 hWnd;
   union
   {
      LBitmapWindow * pBitmapWindowClass;
      LVectorWindow * pVectorWindowClass;
   } Class;
   L_INT                fwSizeType;
   L_INT16              nWidth;
   L_INT16              nHeight;
}SIZEDATA, * pSIZEDATA;
#define soSIZEDATA sizeof(SIZEDATA)

typedef struct tagPANWINDOWDATA
{
   HWND                 hBitmapWindow;
   LBitmapWindow * pClass;
   HWND                 hPanWindow;
   L_UINT               uFlag;
   LPRECT               prcPan;
}PANWINDOWDATA, * pPANWINDOWDATA;
#define soPANWINDOWDATA sizeof(PANWINDOWDATA)

typedef struct tagZOOMDATA
{
   HWND                 hWnd;
   union
   {
      LBitmapWindow * pBitmapWindowClass;
      LVectorWindow * pVectorWindowClass;
   } Class;
   L_UINT               uZoomPercent;
}ZOOMDATA, * pZOOMDATA;
#define soZOOMDATA sizeof(ZOOMDATA)

typedef struct tagVECTORSCALEDATA
{
   HWND                 hWnd;
   LVectorWindow * pVectorWindowClass;
   VECTORPOINT          ScaleFactor;
}VECTORSCALEDATA, * pVECTORSCALEDATA;
#define soVECTORSCALEDATA sizeof(VECTORSCALEDATA)

typedef struct tagPAINTEFFECTDATA
{
   HWND                 hBitmapWindow;
   LBitmapWindow * pClass;
   L_UINT               uPass;
   L_UINT               uType;
}PAINTEFFECTDATA, * pPAINTEFFECTDATA;
#define soPAINTEFFECTDATA sizeof(PAINTEFFECTDATA)

typedef struct tagDRAGDROPDATA
{
   HWND                 hWnd;
   union
   {
      LBitmapWindow * pBitmapWindowClass;
      LVectorWindow * pVectorWindowClass;
   } Class;
   L_UINT               uFilesCount;
   L_UINT               uFileNumber;
   L_TCHAR *        pszFileName;
}DRAGDROPDATA, * pDRAGDROPDATA;
#define soDRAGDROPDATA sizeof(DRAGDROPDATA)

typedef struct tagREGIONDATA
{
   HWND                 hBitmapWindow;
   LBitmapWindow * pClass;
   L_INT                nModificationType;
}REGIONDATA, * pREGIONDATA;
#define soREGIONDATA sizeof(REGIONDATA)

typedef struct tagANNEVENTDATA
{
   HWND                 hBitmapAnnWindow;
   LAnnotationWindow * pClass;
   HANNOBJECT           hAnnObject;
}ANNEVENTDATA, * pANNEVENTDATA;
#define soANNEVENTDATA sizeof(ANNEVENTDATA)

typedef struct tagANIMATIONEVENTDATA
{
   HWND                       hAnimationWindow;
   LAnimationWindow *    pClass;
   L_UINT                     nEvent;
   L_UINT                     nFrameNumber;
} ANIMATIONEVENTDATA, * pANIMATIONEVENTDATA;
#define soANIMATIONEVENTDATA  sizeof(ANIMATIONEVENTDATA)

typedef struct tagSTATUSDATA
{
   DWORD   dwID;
   LBase*  pObject;
}STATUSDATA, * pSTATUSDATA;

typedef struct tagCONLISTOPTIONS
{
   L_UINT   uColumns;      /* Columns at the control (Description & Adderss)  */
   HICON    hServerIcon;   /* Server icon                                     */
   HICON    hClientIcon;   /* Client icon                                     */
} CONLISTOPTIONS, *pCONLISTOPTIONS;

typedef struct tagBarCodeReadOpt
{
   L_UINT32       ulSearchType;
   L_INT          nUnits;
   L_UINT32       ulFlags;
   L_INT          nMultipleMax;
   RECT           rcSearch;
   L_BOOL         bUseRgn;
   BARCODECOLOR   BarColor;
} BARCODEREADOPT, * pBARCODEREADOPT;

#endif //_LEAD_WRAPPER_TYPEDEF_H_
/*================================================================= EOF =====*/
