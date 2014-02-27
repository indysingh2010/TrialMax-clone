/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : tcprnt.h                                                        |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_WRAPPER_DEFINES_H_
#define  _LEAD_WRAPPER_DEFINES_H_

/*----------------------------------------------------------------------------+
| DEFINES                                                                     |
+----------------------------------------------------------------------------*/
#ifdef   _LEAD_WRAPPER_
   #define  LWRP_EXPORT __declspec(dllexport)
   #define L_WRAPPER_MEMSET         L_MEMSET
#else
   #define  LWRP_EXPORT __declspec(dllimport)
   #define L_WRAPPER_MEMSET         memset
#endif

//--FOR LOADING LEAD LIBRARIES-------------------------------------------------
#define  LT_KRN         0x00000001
#define  LT_DIS         0x00000002
#define  LT_FIL         0x00000004
#define  LT_IMG         0x00000008
#define  LT_EFX         0x00000010
#define  LT_WIA         0x00000020
#define  LT_TWN         0x00000040
#define  LT_SCR         0x00000080
#define  LT_ANN         0x00000100
#define  LT_CAP         0x00000200
#define  LT_NET         0x00000400
#define  LT_TMB         0x00001000
#define  LT_LST         0x00002000
#define  LV_KRN         0x00004000
#define  LV_DLG         0x00008000
#define  LT_BAR         0x00010000
#define  LD_KRN         0x00020000
#define  LT_AUT         0x00040000
#define  LT_CON         0x00080000
#define  LT_PNT         0x00100000
#define  LT_NTF         0x00200000
#define  LT_TLB         0x00400000
#define  LT_PDG         0x00800000
#define  LT_WEB         0x01000000
#define  LT_SGM         0x02000000
#define  LT_DLG         0x04000000
#define  LT_ZMV         0x08000000
#define  LT_IMGOPT      0x10000000
#define  LC_MRC         0x20000000
#define  LT_IMGVIEWER   0x40000000
#define  LT_CLR         0x80000000

// Sub Category
#define  LT_DLGIMGEFX   0x00000001
#define  LT_DLGEFX      0x00000002
#define  LT_DLGFILE     0x00000004
#define  LT_DLGIMG      0x00000008
#define  LT_DLGIMGDOC   0x00000010
#define  LT_DLGCLR      0x00000020
#define  LT_DLGKRN      0x00000040
#define  LT_DLGWEB      0x00000080

#define  LT_ALL_DLG     \
         LT_DLGIMGEFX|\
         LT_DLGEFX|\
         LT_DLGFILE|\
         LT_DLGIMG|\
         LT_DLGIMGDOC|\
         LT_DLGCLR|\
         LT_DLGKRN|\
         LT_DLGWEB

// Sub Category
#define  LT_IMGCOR   0x00000001
#define  LT_IMGCLR   0x00000002
#define  LT_IMGSFX   0x00000004
#define  LT_IMGEFX   0x00000008

#define  LT_ALL_IMG     \
         LT_IMGCOR|\
         LT_IMGCLR|\
         LT_IMGSFX|\
         LT_IMGEFX

#define  LT_ALL_LEADLIB                   \
         LT_KRN|                          \
         LT_DIS|LT_FIL|LT_IMG|LT_EFX|     \
         LT_DLG|\
         LT_TWN|LT_SCR|     \
         LT_ANN|LT_CAP|LT_NET|     \
         LT_TMB|LT_LST|LV_KRN|LV_DLG|     \
         LT_BAR|LD_KRN|LT_AUT|LT_CON|     \
         LT_NTF |LT_TLB|LT_PDG|LT_WEB| LT_SGM | LT_ZMV | LT_WIA

//--FOR REDIRECTING I/O CALLBACKS----------------------------------------------
#define  IO_OPEN              0x0001
#define  IO_CLOSE             0x0002
#define  IO_READ              0x0004
#define  IO_WRITE             0x0008
#define  IO_SEEK              0x0010
#define  IO_REDIRECT_ALL  IO_OPEN|IO_CLOSE|IO_READ|IO_WRITE|IO_SEEK


//--FOR BITMAP WINDOW LEAD SPECIFIC STYLE---------------------------------------
#define L_BS_PROCESSKEYBOARD  0x00000002
#define L_BS_3DAPPEARANCE     0x00000008
#define L_BS_CENTER           0x00000010

//--FOR LEAD SPESIFIC MESSAGE---------------------------------------------------
#define WM_LEAD               WM_USER+200
//--FOR LEAD SPESIFIC ERROR-----------------------------------------------------
#define LEAD_LAST_ERROR       0

//--FOR BITMAP WINDOW-----------------------------------------------------------
#define COPY2CB_FLOATER       0x8000

//--FOR ANNOTATION WINDOW-------------------------------------------------------
#define COPY2CB_ANNOBJECTS    0x4000

#define pWRPEXT_CALLBACK pEXT_FUNCTION

#ifndef FOR_BORLAND
#if _MSC_VER <= 1200
   
   # if !defined (GCLP_HCURSOR)
      #define GCLP_HCURSOR        GCL_HCURSOR
   #endif  // # if !defined (GCLP_HCURSOR)
   
   # if !defined (GCLP_HBRBACKGROUND)   
      #define GCLP_HBRBACKGROUND  GCL_HBRBACKGROUND
   #endif  // # if !defined (GCLP_HBRBACKGROUND)
   
   # if !defined (GWLP_WNDPROC)   
      #define GWLP_WNDPROC  GWL_WNDPROC
   #endif  // # if !defined (GWLP_WNDPROC)

   # if !defined (GWLP_HWNDPARENT)   
      #define GWLP_HWNDPARENT      GWL_HWNDPARENT
   #endif  // # if !defined (GWLP_HWNDPARENT)

   # if !defined (GWLP_USERDATA)   
      #define GWLP_USERDATA      GWL_USERDATA
   #endif  // # if !defined (GWLP_USERDATA)

   # if !defined (GWLP_ID)   
      #define GWLP_ID      GWL_ID
   #endif  // # if !defined (GWLP_USERDATA)

   #define errno_t             int

#endif // #if _MSC_VER <= 1200
#endif // FOR_BORLAND

#undef SetWindowLongPtr
#undef GetWindowLongPtr

#ifndef FOR_BORLAND
#if defined(FOR_WIN64)
   #if defined(FOR_UNICODE)
   #define L_SETWINDOWLONGPTR(hWnd, nIndex, dwNewLong) SetWindowLongPtrW(hWnd, nIndex, (LONG_PTR)dwNewLong)
   #define L_GETWINDOWLONGPTR(hWnd, nIndex)            GetWindowLongPtrW(hWnd, nIndex)
   #else
   #define L_SETWINDOWLONGPTR(hWnd, nIndex, dwNewLong) SetWindowLongPtrA(hWnd, nIndex, (LONG_PTR)dwNewLong)
   #define L_GETWINDOWLONGPTR(hWnd, nIndex)            GetWindowLongPtrA(hWnd, nIndex)
   #endif // #if defined(FOR_UNICODE)
#else
   #if defined(FOR_UNICODE)
      #if _MSC_VER <= 1200
         #define L_SETWINDOWLONGPTR(hWnd, nIndex, dwNewLong) SetWindowLongW(hWnd, nIndex, PtrToLong(dwNewLong))
         #define L_GETWINDOWLONGPTR(hWnd, nIndex)            GetWindowLongW(hWnd, nIndex)
      #else
         #define L_SETWINDOWLONGPTR(hWnd, nIndex, dwNewLong) (LONG_PTR)SetWindowLongPtrW(hWnd, nIndex, PtrToLong(dwNewLong))
         #define L_GETWINDOWLONGPTR(hWnd, nIndex)            (LONG_PTR)GetWindowLongPtrW(hWnd, nIndex)
      #endif  // #if _MSC_VER <= 1200
   #else
      #if _MSC_VER <= 1200      
         #define L_SETWINDOWLONGPTR(hWnd, nIndex, dwNewLong) SetWindowLongA(hWnd, nIndex, PtrToLong(dwNewLong))
         #define L_GETWINDOWLONGPTR(hWnd, nIndex)            GetWindowLongA(hWnd, nIndex)
      #else
         #define L_SETWINDOWLONGPTR(hWnd, nIndex, dwNewLong) (LONG_PTR)SetWindowLongPtrA(hWnd, nIndex, PtrToLong(dwNewLong))
         #define L_GETWINDOWLONGPTR(hWnd, nIndex)            (LONG_PTR)GetWindowLongPtrA(hWnd, nIndex)
      #endif  // #if _MSC_VER <= 1200
   #endif // #if defined(FOR_UNICODE)
#endif
#endif //FOR_BORLAND
#endif //_LEAD_WRAPPER_DEFINES_H_
/*================================================================= EOF =====*/
