//
//   LTZMV.H - LEAD Zoom View module header file
//   Copyright (c) 1991-2009 LEAD Technologies, Inc.
//   All Rights Reserved.


#if !defined (LTZMV_H)
#define LTZMV_H

#if !defined(L_LTZMV_API)
   #define L_LTZMV_API
#endif // #if !defined(L_LTZMV_API)

#include "ltkrn.h"
#include "ltdis.h"
#include "LTANN.h"

#define L_HEADER_ENTRY
#include "ltpck.h"

// enums and defines
////////////////////////////////////////////////////

#define ZOOMVIEWBORDERSTYLE_NONE       0x0000 /* no border */
#define ZOOMVIEWBORDERSTYLE_SIMPLE     0x0001 /* simple, single-pixel border */
#define ZOOMVIEWBORDERSTYLE_3D         0x0002 /* 3-D look */
#define ZOOMVIEWBORDERSTYLE_TEAROUT    0x0004 /* torn out look */
#define ZOOMVIEWBORDERSTYLE_ROUNDED    0x0008 /* rectangle with rounded corners */ 

#define SOURCEBORDERSTYLE_SIMPLE       0x0000 /* simple, single-pixel border */
#define SOURCEBORDERSTYLE_3D           0x0001 /* 3-D look*/


#define ZOOMVIEWSTATE_NONE                   (0)
#define ZOOMVIEWSTATE_ANNOTATION_EDIT_MODE   (1)   

// structures
////////////////////////////////////////////////////

typedef struct tagZOOMVIEWPROPS
{
   L_UINT                   uStructSize;
   L_UINT                   uIndex;
   RECT                     rcSrc;
   RECT                     rcDst;
   RECT                     rcView;
   L_INT                    nZoom;
   L_BOOL                   bForceDst;
   L_BOOL                   bEnabled;
   L_UINT32                 uZoomViewBorderStyle;
   COLORREF                 crZoomViewBorder;
   L_INT                    nZoomViewPenStyle;
   HRGN                     hBorderRgn;
   L_UINT32                 uSrcBorderStyle;
   COLORREF                 crSrcBorder;
   L_INT                    nSrcPenStyle;
   L_BOOL                   bCallouts;
   COLORREF                 crCallout;
   L_INT                    nCalloutPenStyle;
   HANNOBJECT               hAnnContainer;
   L_INT                    nSrcPenWidth;
   L_INT                    nZoomViewPenWidth;
   L_INT                    nCalloutPenWidth;   
} ZOOMVIEWPROPS, * pZOOMVIEWPROPS;



// Used in L_StartZoomViewAnnEdit
typedef struct tagZOOMVIEWANNEDIT
{
   L_UINT                  uStructSize;
   HANNOBJECT              hAnnAutomation;
   HWND                    hWndAnnToolbar;
   L_INT32                 nReserved;
} ZOOMVIEWANNEDIT, *pZOOMVIEWANNEDIT;


/****************************************************************
   Function prototypes
****************************************************************/

L_LTZMV_API L_BOOL EXT_FUNCTION L_WindowHasZoomView(HWND hWnd);

L_LTZMV_API L_INT EXT_FUNCTION L_CreateZoomView(HWND hWnd,
                                    pBITMAPHANDLE pBitmap,
                                    pZOOMVIEWPROPS pZoomViewProps);

L_LTZMV_API L_INT EXT_FUNCTION L_GetZoomViewProps(HWND hWnd,
                                      pZOOMVIEWPROPS pZoomViewProps,
                                      L_UINT32 uStructSize);

L_LTZMV_API L_INT EXT_FUNCTION L_UpdateZoomView(HWND hWnd,
                                    pZOOMVIEWPROPS pZoomViewProps);

L_LTZMV_API L_INT EXT_FUNCTION L_DestroyZoomView(HWND hWnd, L_UINT uIndex);

L_LTZMV_API L_INT EXT_FUNCTION L_GetZoomViewsCount(HWND hWnd, L_UINT *puCount);

L_LTZMV_API L_INT EXT_FUNCTION L_RenderZoomView(HDC hDC, HWND hWnd);

L_LTZMV_API L_INT EXT_FUNCTION L_StartZoomViewAnnEdit(HWND hWnd, pZOOMVIEWANNEDIT pZoomViewAnnEdit);

L_LTZMV_API L_INT EXT_FUNCTION L_StopZoomViewAnnEdit(HWND hWnd);

#undef L_HEADER_ENTRY
#include "ltpck.h"


#endif //LTZMV_H
