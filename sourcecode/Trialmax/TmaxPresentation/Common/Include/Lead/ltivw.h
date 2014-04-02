/*
   LTIVW.H - LEAD Image viewer header file
   Copyright (c) 1991-2005 LEAD Technologies, Inc.
   All Rights Reserved.
*/

#if !defined(INC_LTIMAGVIEWER_H)
#define INC_LTIMAGVIEWER_H

#if !defined(L_LTIVW_API)
   #define L_LTIVW_API
#endif // #if !defined(L_LTIVW_API)

#include "ltkrn.h"
#include "ltdis.h"
#include "ltefx.h"
#include "ltimg.h"
#include "ltann.h"

#define L_RGN_REMOVE 8

#define L_HEADER_ENTRY
#include "ltpck.h"

/* The window class for the container control */
#define LTCONTAINERCLASS TEXT("LEADCONTAINER32")

/* The container handle */
typedef HANDLE HDISPCONTAINER;

typedef struct tagDISPCONTAINERBITMAPINFO
{
   L_UINT uStructSize;
   L_UINT uWidth;
   L_UINT uHeight;
   L_UINT uXResolution;
   L_UINT uYResolution;
}
DISPCONTAINERBITMAPINFO, * pDISPCONTAINERBITMAPINFO;


typedef struct tagDISPANIMATIONPROPS
{
   L_UINT uStructSize;
   L_INT  nFrames;
   L_UINT uInterval;
   L_UINT uFlags;
}
DISPANIMATIONPROPS, * pDISPANIMATIONPROPS;

typedef struct tagDISPREGIONPROPS
{
   L_BOOL  bCreateFromCenter;
   L_UINT  uOperation;
}
DISPREGIONPROPS, * pDISPREGIONPROPS;

typedef struct tagDISPANNOTATIONPROPS
{
   L_UINT    uStructSize;
   COLORREF  crAnnotationColor;
   L_UINT    bSimpleRuler;
   L_BOOL    bCreateFromCenter;
   L_UINT    uFlags;
   COLORREF  crForeColor;
}
DISPANNOTATIONPROPS, * pDISPANNOTATIONPROPS;

typedef struct tagDISPCONTAINERPROPERTIES
{
   L_UINT   uStructSize;
   L_UINT   uMask;
   L_UINT   uMask1;
   L_UINT   uNumRows;
   L_UINT   uNumCols;
   COLORREF crEmptyCellBackGroundColor;
   COLORREF crBackground;
   COLORREF crText;
   COLORREF crShadow;
   COLORREF crRulerIn;
   COLORREF crRulerout;
   COLORREF crActiveBorderColor;
   COLORREF crNonActiveBorderColor;
   COLORREF crActiveSubCellBorderColor;
   HCURSOR  hDefaultCursor;
   HCURSOR  hRszVertCursor;
   HCURSOR  hRszHorzCursor;
   HCURSOR  hRszBothCursor;
   HCURSOR  hAnnMoveCursor;
   HCURSOR  hAnnSelectCursor;
   HCURSOR  hAnnDefaultCursor;
   HCURSOR  hRegionDefaultCursor;
   L_BOOL   bAutoScroll;
   L_UINT   uCellsMaintenance;
   L_BOOL   bShowFreezeText;
   L_UINT   uBorderStyle;
   L_UINT   uTextQuality;
   L_UINT   uRulerStyle;
   L_UINT   uSplitterStyle;
   L_UINT   bUseExtraSplitters;
   L_BOOL   bCustomSplitterColor;
   COLORREF crSplitterColor;
   L_UINT   uPaintingMethod;
   L_BOOL   bShowContainerScroll;
   L_BOOL   bShowCellScroll;
   L_UINT   uOverlayTextSize;
   COLORREF crRegionBorderColor1;
   COLORREF crRegionBorderColor2;
   L_BOOL   bInteractiveInterpolation;
}
DISPCONTAINERPROPERTIES, * pDISPCONTAINERPROPERTIES;

typedef struct tagDISPCELLPROPERTIES
{
   L_UINT   uStructSize;
   L_UINT   uMask;
   L_UINT   uShowRuler;
   L_UINT   uShowTags;
   L_UINT   uNumRows;
   L_UINT   uNumCols;
   L_BOOL   bOnMove;
   L_BOOL   bIndividual;
   L_BOOL   bIsFit;
}
DISPCELLPROPERTIES, * pDISPCELLPROPERTIES;

typedef struct tagDISPCELLTAGINFOA
{
   L_UINT    uStructSize;
   L_UINT    uMask;
   L_UINT    uPosition;
   L_UINT    uAlign;
   L_CHAR *  szText;
   L_UINT    uTagType;
}
DISPCELLTAGINFOA, * pDISPCELLTAGINFOA;

#if defined(FOR_UNICODE)
typedef struct tagDISPCELLTAGINFO
{
   L_UINT    uStructSize;
   L_UINT    uMask;
   L_UINT    uPosition;
   L_UINT    uAlign;
   L_TCHAR * szText;
   L_UINT    uTagType;
}
DISPCELLTAGINFO, * pDISPCELLTAGINFO;
#else
typedef DISPCELLTAGINFOA DISPCELLTAGINFO;
typedef pDISPCELLTAGINFOA pDISPCELLTAGINFO;
#endif // #if defined(FOR_UNICODE)

typedef struct tagDISPCONTAINERACTIONPROPS
{
   L_UINT   uStructSize;
   HCURSOR  hCursor;
   L_INT    nChange;
   L_BOOL   bCircularMouseMove;
}
DISPCONTAINERACTIONPROPS, * pDISPCONTAINERACTIONPROPS;

typedef struct tagDISPWLEVELACTIONPROPS
{
   DISPCONTAINERACTIONPROPS DispContainerActionProps;
   COLORREF                 rgbColorStart;
   COLORREF                 rgbColorEnd;
   L_INT                    nWidth;
   L_INT                    nCenter;
   L_UINT                   uFillType;
   L_BOOL                   bRelativeSensitivity;
}
DISPWLEVELACTIONPROPS, * pDISPWLEVELACTIONPROPS;

typedef struct tagDISPALPHAACTIONPROPS
{
   DISPCONTAINERACTIONPROPS DispContainerActionProps;
   L_INT                    nFactor;
}
DISPALPHAACTIONPROPS, * pDISPALPHAACTIONPROPS;

typedef struct tagDISPMAGACTIONPROPS
{
   DISPCONTAINERACTIONPROPS DispContainerActionProps;
   L_INT                    nWidth;
   L_INT                    nHeight;
   L_INT                    nZoom;
   COLORREF                 clrPen;
   L_BOOL                   bEllipse;
   L_INT                    nBorderSize;
   L_BOOL                   b3D;
   L_INT                    nCrosshair;
}
DISPMAGACTIONPROPS, * pDISPMAGACTIONPROPS;

typedef struct tagDISPOFFSETACTIONPROPS
{
   DISPCONTAINERACTIONPROPS DispContainerActionProps;
   L_INT                    nXOffset;
   L_INT                    nYOffset;
   L_BOOL                   bRelativeSensitivity;
}
DISPOFFSETACTIONPROPS, * pDISPOFFSETACTIONPROPS;

typedef struct tagDISPNUDGETOOLACTIONPROPS
{
   L_UINT      uStructSize;
   L_UINT      uWidth;
   L_UINT      uHeight;
   L_UINT      uShape;
}
DISPNUDGETOOLACTIONPROPS, * pDISPNUDGETOOLACTIONPROPS;


typedef struct tagDISPSCALEACTIONPROPS
{
   DISPCONTAINERACTIONPROPS DispContainerActionProps;
   L_UINT                   uScaleType;
   L_UINT                   uScale;
}
DISPSCALEACTIONPROPS, * pDISPSCALEACTIONPROPS;

typedef struct tagDISPROTATE3DPLANEACTIONPROPS
{
   DISPCONTAINERACTIONPROPS DispContainerActionProps;
   L_UINT uRotationAxis;
   L_UINT uRotationSpace;
}
DISPROTATE3DPLANEACTIONPROPS, * pDISPROTATE3DPLANEACTIONPROPS;


typedef struct tagDISPSTACKACTIONPROPS
{
   DISPCONTAINERACTIONPROPS DispContainerActionProps;
   L_INT                    nScrollValue;
   L_INT                    nActiveSubCell;
}
DISPSTACKACTIONPROPS, * pDISPSTACKACTIONPROPS;

typedef struct tagDISPOWNERACTIONPROPS
{
   DISPCONTAINERACTIONPROPS DispContainerActionProps;
}
DISPOWNERACTIONPROPS, * pDISPOWNERACTIONPROPS;

typedef  struct tagDISPCONTAINERTITLEBARPROPS
{
   L_UINT   uStructSize;
   L_UINT   uMask;
   COLORREF crColor;
   L_BOOL   bCustomTitlebarColor;
}
DISPCONTAINERTITLEBARPROPS, * pDISPCONTAINERTITLEBARPROPS;

typedef  struct tagDISPCONTAINERTITLEBARICONPROPS
{
   L_UINT   uStructSize;
   L_UINT   uMask;
   L_BOOL   bVisible;
   L_BOOL   bReadOnly;
   COLORREF crColor;
   COLORREF crColorPressed;
   COLORREF crColorHover;
}
DISPCONTAINERTITLEBARICONPROPS, * pDISPCONTAINERTITLEBARICONPROPS;

typedef struct tagDISPCONTAINERANNATTRIBS
{
   L_UINT uStructSize;
   L_UINT uType;
}
DISPCONTAINERANNATTRIBS, * pDISPCONTAINERANNATTRIBS;

typedef struct tagDISPCONTAINERCELLINFO
{
   L_UINT uStructSize;
   L_HDC  hDC;
   L_INT  nCellIndex;
   L_INT  nSubCellIndex;
   RECT   rcRect;
   RECT   rcBitmapRect;
   L_INT  nX;
   L_INT  nY;
}
DISPCONTAINERCELLINFO, * pDISPCONTAINERCELLINFO;

#define MAX_BUTTON_NUM 7


#define CONTAINER_DONTHANDLEPALETTE             0x00000000
#define CONTAINER_HANDLEPALETTE                 0x00000001

#define CONTAINER_ACTION_NONE                   0
#define CONTAINER_ACTION_WINDOWLEVEL            1
#define CONTAINER_ACTION_SCALE                  2
#define CONTAINER_ACTION_OFFSET                 3
#define CONTAINER_ACTION_STACK                  4
#define CONTAINER_ACTION_MAG                    5
#define CONTAINER_ACTION_ALPHA                  6
#define CONTAINER_ACTION_ANNOTATION_RULER       7
#define CONTAINER_ACTION_ANNOTATION_ANGLE       8
#define CONTAINER_ACTION_ANNOTATION_TEXT        9
#define CONTAINER_ACTION_ANNOTATION_ARROW       10
#define CONTAINER_ACTION_ANNOTATION_RECTANGLE   11
#define CONTAINER_ACTION_ANNOTATION_ELLIPSE     12
#define CONTAINER_ACTION_ANNOTATION_HILITE      13
#define CONTAINER_ACTION_REGION_RECTANGLE       14
#define CONTAINER_ACTION_REGION_ELLIPSE         15
#define CONTAINER_ACTION_REGION_FREEHAND        16
#define CONTAINER_ACTION_REGION_POLYGON         17
#define CONTAINER_ACTION_REGION_MAGICWAND       18
#define CONTAINER_ACTION_REGION_COLORRANGE      19
#define CONTAINER_ACTION_REGION_CIRCLE          20
#define CONTAINER_ACTION_REGION_SQUARE          21
#define CONTAINER_ACTION_REGION_NUDGETOOL       22
#define CONTAINER_ACTION_REGION_SHRINKTOOL      23
#define CONTAINER_ACTION_REFERENCELINE          24
#define CONTAINER_ACTION_ROTATE3DOBJECT         25
#define CONTAINER_ACTION_TRANSLATE3DOBJECT      26
#define CONTAINER_ACTION_SCALE3DOBJECT          27
#define CONTAINER_ACTION_ROTATE3DCAMERA         28
#define CONTAINER_ACTION_TRANSLATE3DCAMERA      29
#define CONTAINER_ACTION_ZOOMCAMERA             30
#define CONTAINER_ACTION_TRANSLATEPLANE         31
#define CONTAINER_ACTION_ROTATEPLANE            32

#define CONTAINER_FILE_CREATE                   0
#define CONTAINER_FILE_APPEND                   1
#define CONTAINER_FILE_REPLACE                  2
#define CONTAINER_FILE_INSERT                   3

#define DISPWIN_ALIGN_TOPLEFT                   0
#define DISPWIN_ALIGN_LEFTCENTER                1
#define DISPWIN_ALIGN_BOTTOMLEFT                2
#define DISPWIN_ALIGN_TOPCENTER                 3
#define DISPWIN_ALIGN_BOTTOMCENTER              4
#define DISPWIN_ALIGN_TOPRIGHT                  5
#define DISPWIN_ALIGN_RIGHTCENTER               6
#define DISPWIN_ALIGN_BOTTOMRIGHT               7

#define DISPWIN_TYPE_USERDATA                   0
#define DISPWIN_TYPE_SCALE                      1
#define DISPWIN_TYPE_WLCENTERWIDTH              2
#define DISPWIN_TYPE_FIELDOFVIEW                3
#define DISPWIN_TYPE_OWNERDRAW                  4
#define DISPWIN_TYPE_FRAME                      5
#define DISPWIN_TYPE_RULERUNIT                  6
#define DISPWIN_TYPE_LEFTORIENTATION            7
#define DISPWIN_TYPE_RIGHTORIENTATION           8
#define DISPWIN_TYPE_TOPORIENTATION             9
#define DISPWIN_TYPE_BOTTOMORIENTATION          10
#define DISPWIN_TYPE_OFFSET                     11


#define CONTAINER_CALIBRATERULER_BOTH           0
#define CONTAINER_CALIBRATERULER_VERTICAL       1
#define CONTAINER_CALIBRATERULER_HORIZONTAL     2

#define DCPF_ALL                       0xFFFFFFFF
#define DCPF_NUMROWS                   0x00000001
#define DCPF_NUMCOLS                   0x00000002
#define DCPF_EMPTYCELLBACKGROUNDCOLOR  0x00000004
#define DCPF_BACKGROUNDCOLOR           0x00000008
#define DCPF_TEXTCOLOR                 0x00000010
#define DCPF_SHADOWCOLOR               0x00000020
#define DCPF_RULERINCOLOR              0x00000040
#define DCPF_RULEROUTCOLOR             0x00000080
#define DCPF_ACTIVEBORDERCOLOR         0x00000100
#define DCPF_NONACTIVEBORDERCOLOR      0x00000200
#define DCPF_ACTIVESUBCELLBORDERCOLOR  0x00000400
#define DCPF_DEFAULTCURSOR             0x00000800
#define DCPF_VERTCURSOR                0x00001000
#define DCPF_HORZCURSOR                0x00002000
#define DCPF_BOTHCURSOR                0x00004000
#define DCPF_ANNDEFCURSOR              0x00008000
#define DCPF_ANNMOVCURSOR              0x00010000
#define DCPF_ANNSELCURSOR              0x00020000
#define DCPF_RGNDEFCURSOR              0x00040000
#define DCPF_AUTOSCROLL                0x00080000
#define DCPF_CELLSMAINTENANCE          0x00100000
#define DCPF_SHOWFREEZETEXT            0x00200000
#define DCPF_BORDERSTYLE               0x00400000
#define DCPF_TEXTQUALITY               0x00800000
#define DCPF_RULERSTYLE                0x01000000
#define DCPF_SPLITTERSTYLE             0x02000000
#define DCPF_SPLITTERCOLOR             0x04000000
#define DCPF_PAINTMETHOD               0x08000000
#define DCPF_USEEXTRASPLITTERS         0x10000000
#define DCPF_SHOWVIEWERSCROLL          0x20000000
#define DCPF_SHOWCELLSCROLL            0x40000000
#define DCPF_OVERLAYTEXTSIZE           0x80000000

#define DCPF1_ALL                      0x00000007
#define DCPF1_REGIONCOLOR1             0x00000001
#define DCPF1_REGIONCOLOR2             0x00000002
#define DCPF1_INTERACTIVEINTERPOLATION 0x00000004

#define DCTF_ALL                       0x0000000F
#define DCTF_POSITION                  0x00000001
#define DCTF_ALIGN                     0x00000002
#define DCTF_TYPE                      0x00000004
#define DCTF_TEXT                      0x00000008

#define DCTITLEBAR_ALL                 0x00000001
#define DCTITLEBAR_COLOR               0x00000001

#define DCPF_RULERSTYLE_INVERT         0x00000000
#define DCPF_RULERSTYLE_BORDERED       0x00000001

#define DCPF_CELLSMAINTENANCE_SIZE     0x00000000
#define DCPF_CELLSMAINTENANCE_POS      0x00000001

#define DCACTION_ACTIVEONLY            0x00000000
#define DCACTION_SELECTED              0x00000001
#define DCACTION_ALLCELLS              0x00000002

#define DCACTION_REALTIME              0x00000000
#define DCACTION_ONRELEASE             0x00000010

#define DCANNOTATION_NONE              0x00000000
#define DCANNOTATION_DROPSHADOW        0x00000010

#define DCANNOTATION_APPLYTO_SELECTED  0x00000000
#define DCANNOTATION_APPLYTO_ALLOBJECT 0x00000100
#define DCANNOTATION_APPLYTO_ALL       0x00000200

#define DCANNOTATION_APPLYCOLOR_FORNEW 0x00001000

#define CONTAINER_RULERUNIT_INCHES            0
#define CONTAINER_RULERUNIT_FEET              1
#define CONTAINER_RULERUNIT_MICROMETERS       2
#define CONTAINER_RULERUNIT_MILLIMETERS       3
#define CONTAINER_RULERUNIT_CENTIMETERS       4
#define CONTAINER_RULERUNIT_METERS            5

#define CONTAINER_BORDERSIZE_NONE             0
#define CONTAINER_BORDERSIZE_THIN             1
#define CONTAINER_BORDERSIZE_THICK            2

#define CONTAINER_NUDGETOOLSHAPE_RECTANGLE    0
#define CONTAINER_NUDGETOOLSHAPE_ELLIPSE      1
#define CONTAINER_NUDGETOOLSHAPE_SLASH        2
#define CONTAINER_NUDGETOOLSHAPE_BACKSLASH    3

enum DISPCONTAINER_MOUSEBUTTONS
{
   CONTAINER_MOUSE_BUTTON_NONE,
   CONTAINER_MOUSE_BUTTON_LEFT,
   CONTAINER_MOUSE_BUTTON_RIGHT,
   CONTAINER_MOUSE_BUTTON_MIDDLE,
   CONTAINER_MOUSE_WHEEL,
   CONTAINER_MOUSE_BUTTON_XBUTTON1, //the first XButton
   CONTAINER_MOUSE_BUTTON_XBUTTON2, //the second XButton
};

#define DCCELLPF_ALL                        0x0000007F
#define DCCELLPF_SHOWRULER                  0x00000001
#define DCCELLPF_SHOWTAGS                   0x00000002
#define DCCELLPF_ROWS                       0x00000004
#define DCCELLPF_COLS                       0x00000008
#define DCCELLPF_APPLYONMOVE                0x00000010
#define DCCELLPF_ALLOWINDIVIDUALWINDOWLEVEL 0x00000020
#define DCCELLPF_IMAGE_FIT                  0x00000040

#define DCCELLPF_SHOWRULER_NONE             0x00000000
#define DCCELLPF_SHOWRULER_BOTH             0x00000001
#define DCCELLPF_SHOWRULER_VERT             0x00000002
#define DCCELLPF_SHOWRULER_HORZ             0x00000003

#define DCCELLPF_SHOWTAGS_SHOW              0x00000000
#define DCCELLPF_SHOWTAGS_HIDE              0x00000001

#define CONTAINER_MOUSEMOVE_UP              0x00000000
#define CONTAINER_MOUSEMOVE_DOWN            0x00000001
#define CONTAINER_MOUSEMOVE_LEFT            0x00000002
#define CONTAINER_MOUSEMOVE_RIGHT           0x00000003
#define CONTAINER_TOGGLE_CIRCLE_ELLIPSE     0x00000004
#define CONTAINER_TOGGLE_SQUARE_RECTANGLE   0x00000004
#define CONTAINER_TOGGLE_EDGE_CENTER        0x00000005

#define CONTAINER_SCALEMODE_NORMAL          0x00000000
#define CONTAINER_SCALEMODE_FIT             0x00000001
#define CONTAINER_SCALEMODE_FITWIDTH        0x00000002
#define CONTAINER_SCALEMODE_FITHEIGHT       0x00000003

#define CONTAINER_ACTION_CONTAINERLEVEL     0x00000000
#define CONTAINER_ACTION_CELLLEVEL          0x00000001

#define CONTAINER_KEY_SHIFT                 0x00000001
#define CONTAINER_KEY_CTRL                  0x00000002
#define CONTAINER_KEY_ALT                   0x00000004

#define CONTAINER_ANIMATION_SHOW_ALL            -1
#define CONTAINER_ANIMATION_SHOW_ODD             0
#define CONTAINER_ANIMATION_SHOW_EVEN            1

#define CONTAINER_ANIMATION_PAINT_NORMAL        0x00000000
#define CONTAINER_ANIMATION_PAINT_RESAMPLE      0x00000001
#define CONTAINER_ANIMATION_PAINT_BICUBIC       0x00000002

#define CONTAINER_ANIMATION_SHOW_ANNOTATION     0x00000010

#define CONTAINER_ANIMATION_SHOW_REGION         0x00000100

#define CONTAINER_ANIMATION_DIRECTION_FORWARD   0x00000000
#define CONTAINER_ANIMATION_DIRECTION_BACKWARD  0x00001000

#define CONTAINER_ANIMATION_LOOP_SEQUENCE       0x00000000
#define CONTAINER_ANIMATION_LOOP_SWEEP          0x00010000
#define CONTAINER_ANIMATION_LOOP_SHUFFLE        0x00020000

#define CONTAINER_ANIMATION_PLAYONSELECTION     0x00100000

#define DCTITLEBAR_ICONPROPS_ALL                0x0000001F
#define DCTITLEBAR_ICONPROPS_COLOR              0x00000001
#define DCTITLEBAR_ICONPROPS_COLORHOVER         0x00000002
#define DCTITLEBAR_ICONPROPS_COLORPRESSED       0x00000004
#define DCTITLEBAR_ICONPROPS_SHOW               0x00000008
#define DCTITLEBAR_ICONPROPS_READONLY           0x00000010

#define CONTAINER_SETIMAGE_INSERT               0x00000000
#define CONTAINER_SETIMAGE_REPLACE              0x00000001

#define CONTAINER_PRINTCELL_ALL                 0x0000001F
#define CONTAINER_PRINTCELL_REGION              0x00000001
#define CONTAINER_PRINTCELL_ANNOTATION          0x00000002
#define CONTAINER_PRINTCELL_BORDERS             0x00000004
#define CONTAINER_PRINTCELL_TAG                 0x00000008
#define CONTAINER_PRINTCELL_RULERS              0x00000010

#define CONTAINER_PRINTCELL_EXPLODED            0x00000020


typedef L_INT (pEXT_CALLBACK DISPCONTAINERANIMATIONSTARTEDCALLBACK) (L_INT    nCellIndex,
                                                                     L_VOID * pAnimationStartedUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERANIMATIONSTOPPEDCALLBACK) (L_INT    nCellIndex,
                                                                     L_VOID * pAnimationStoppedUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERFRAMEREQUESTEDCALLBACK)    (L_INT    nCellIndex,
                                                                      L_UINT * puFramesRequested,
                                                                      L_UINT   uLength,
                                                                      L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERPREPAINTCALLBACK)          (pDISPCONTAINERCELLINFO pCellInfo,
                                                                      L_VOID *               pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERPOSTPAINTCALLBACK)         (pDISPCONTAINERCELLINFO pCellInfo,
                                                                      L_VOID *               pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERMOUSECALLBACK)             (L_UINT                 uMessage,
                                                                      pDISPCONTAINERCELLINFO pCellInfo,
                                                                      L_VOID *               pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERACTIVESUBCELLCHANGED)      (L_INT    nCellIndex,
                                                                      L_INT    nSubCellIndex,
                                                                      L_INT    nPreviousSubCellIndex,
                                                                      L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERANNOTATIONCREATEDCALLBACK) (L_INT    nCellIndex,
                                                                      L_INT    nSubCellIndex,
                                                                      L_UINT   uAnnotationType,
                                                                      L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERREGIONCALLBACK) (HRGN     hRgn,
                                                           L_INT    nCellIndex,
                                                           L_INT    nSubCellIndex,
                                                           L_UINT   uOperation,
                                                           L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERANNOTATIONCALLBACK) (L_UINT   uMessage,
                                                               L_INT    nX,
                                                               L_INT    nY,
                                                               L_INT    nCellIndex,
                                                               L_INT    nSubCellIndex,
                                                               L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERPAINTCALLBACK)  (HDC    hMemDC,
                                                           LPRECT lpRect,
                                                           L_INT  nCellIndex,
                                                           L_INT  nSubCellIndex,
                                                           L_VOID *  pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERACTIONCALLBACK) (L_INT               nCellIndex,
                                                           HBITMAPLIST * phBitmapList,
                                                           L_UINT              uCount,
                                                           L_INT               nAction,
                                                           L_UINT              uMessage,
                                                           L_UINT              wParam,
                                                           POINT *       ptMousePos,
                                                           L_VOID *      pUserData);

typedef L_INT (pEXT_CALLBACK DISPCONTAINERTAGCALLBACK)    (L_INT          nCellIndex,
                                                           HDC            hDC,
                                                           RECT *   lpRect,
                                                           L_VOID * pUserData);

/* External prototypes */
L_LTIVW_API HDISPCONTAINER EXT_FUNCTION L_DispContainerCreate    (HWND   hWndParent,
                                                      LPRECT lpRect,
                                                      L_UINT uFlags);

L_LTIVW_API HDISPCONTAINER EXT_FUNCTION L_DispContainerGetHandle (HWND hConWnd);

L_LTIVW_API HWND EXT_FUNCTION L_DispContainerGetWindowHandle     (HDISPCONTAINER hCon,
                                                      L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerDestroy            (HDISPCONTAINER hCon, 
                                                     L_BOOL         bCleanImages,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetProperties      (HDISPCONTAINER            hCon,
                                                     pDISPCONTAINERPROPERTIES  pDispContainerProp,
                                                     L_UINT                    uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetProperties      (HDISPCONTAINER            hCon,
                                                     pDISPCONTAINERPROPERTIES pDispContainerProp,
                                                     L_UINT                    uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerInsertCell         (HDISPCONTAINER hCon, 
                                                     L_INT          nCellIndex, 
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerRemoveCell         (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_BOOL         bCleanImage, 
                                                      L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellCount       (HDISPCONTAINER hCon,
                                                     L_UINT         uFlags);

L_LTIVW_API HWND EXT_FUNCTION L_DispContainerGetCellWindowHandle (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetCellBitmapList  (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     HBITMAPLIST    hBitmapList,
                                                     L_BOOL         bCleanImage, 
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerAddAction          (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetAction          (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_INT          nMouseButton,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetCellTagA        (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_UINT         uRow,
                                                     L_UINT         uAlign,
                                                     L_UINT         uType,
                                                     LPSTR          pString,
                                                     L_UINT         uFlags);

L_LTIVW_API L_VOID EXT_FUNCTION L_UseContainerControl            (L_VOID);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetCellProperties  (HDISPCONTAINER       hCon,
                                                     L_INT                nCellIndex,
                                                     pDISPCELLPROPERTIES  pCellProperties,
                                                     L_UINT               uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellProperties  (HDISPCONTAINER       hCon,
                                                     L_INT                nCellIndex,
                                                     pDISPCELLPROPERTIES  pCellProperties,
                                                     L_UINT               uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellPosition    (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_UINT * puRow,
                                                     L_UINT * puCol,
                                                     L_UINT         uFlags);

// This function changes the cell position
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerRepositionCell     (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_INT          nTargetIndex,
                                                     L_BOOL         bSwap,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellBitmapList  (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     pHBITMAPLIST   phBitmapList,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellBounds      (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     LPRECT         lpRect,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerFreezeCell         (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_BOOL         bFreeze,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetFirstVisibleRow (HDISPCONTAINER hCon,
                                                     L_UINT         uRow,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetFirstVisibleRow (HDISPCONTAINER hCon,
                                                     L_UINT *       uRow,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetActionProperties(HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_INT          nCellIndex,
                                                     L_INT          nSubCellIndex,
                                                     L_VOID *       pActionProperties,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetActionProperties(HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_INT          nCellIndex,
                                                     L_INT          nSubCellIndex,
                                                     L_VOID *       pActionProperties,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerRemoveAction       (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetActionCount     (HDISPCONTAINER hCon,
                                                      L_INT *  nCount,
                                                      L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetKeyboardAction  (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_INT          nButton, 
                                                     L_UINT         uKey,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetBounds          (HDISPCONTAINER hCon,
                                                     RECT *   lpRect,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetBounds          (HDISPCONTAINER hCon,
                                                     LPRECT         lpRect,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSelectCell         (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_BOOL         bSelect,
                                                     L_UINT         uFlags);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerIsCellSelected    (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetTagCallBack     (HDISPCONTAINER           hCon,
                                                     DISPCONTAINERTAGCALLBACK pfnCallBack,
                                                     L_VOID         *   pUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetActionCallBack  (HDISPCONTAINER              hCon,
                                                     DISPCONTAINERACTIONCALLBACK pfnCallBack,
                                                     L_VOID         *      pUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetTagCallBack     (HDISPCONTAINER                   hCon,
                                                     DISPCONTAINERTAGCALLBACK * ppfnCallBack,
                                                     LPVOID *                   ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetActionCallBack  (HDISPCONTAINER                      hCon,
                                                     DISPCONTAINERACTIONCALLBACK * ppfnCallBack,
                                                     LPVOID                      * ppUserData);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerIsActionActive    (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetKeyboardAction  (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_INT          nMouseDirection,
                                                     L_UINT * puVk,
                                                     L_UINT * puModifiers,
                                                     L_UINT         uFlags);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerIsCellFrozen      (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetPaintCallBack   (HDISPCONTAINER              hCon,
                                                     DISPCONTAINERPAINTCALLBACK  pfnPaintCallBack,
                                                     L_VOID         *      pUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetPaintCallBack   (HDISPCONTAINER                      hCon,
                                                     DISPCONTAINERPAINTCALLBACK  * ppfnPaintCallBack,
                                                     LPVOID                      * ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerRepaintCell        (HDISPCONTAINER hCon,
                                                     HWND           hWnd,
                                                     L_INT          nCellIndex,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetBitmapHandle    (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_INT          nSubCellIndex,
                                                     pBITMAPHANDLE  pBitmap,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetBitmapHandle    (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_INT          nSubCellIndex,
                                                     pBITMAPHANDLE  pBitmap,
                                                     L_BOOL         bRepaint,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetRulerUnit       (HDISPCONTAINER hCon,
                                                     L_UINT         uUnit,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerCalibrateRuler     (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_INT          nSubCellIndex,
                                                     L_DOUBLE       dLength,
                                                     L_UINT         uUnit,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetActionButton   (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_INT  * pnMouseButton,
                                                     L_UINT * puFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerAnnToRgn          (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_INT          nSubCellIndex,
                                                     L_UINT         uCombineMode,
                                                     L_BOOL         bDeleteAnn,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerIsButtonValid     (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_INT          nMouseButton,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerIsKeyValid        (HDISPCONTAINER hCon,
                                                     L_INT          nAction,
                                                     L_INT          nButton,
                                                     L_UINT         uKey,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerStartAnimation    (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_INT          nStartFrame,
                                                     L_INT          nFrameCount,
                                                     L_BOOL         bAnimateAllSubCells,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetAnimationProperties (HDISPCONTAINER       hCon,
                                                          L_INT                nCellIndex,
                                                          pDISPANIMATIONPROPS  pDisAnimationProps,
                                                          L_UINT               uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetAnimationProperties (HDISPCONTAINER       hCon,
                                                          L_INT                nCellIndex,
                                                          pDISPANIMATIONPROPS  pDisAnimationProps,
                                                          L_UINT               uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerStopAnimation          (HDISPCONTAINER hCon,
                                                          L_INT          nCellIndex,
                                                          L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerShowTitlebar           (HDISPCONTAINER hCon,
                                                          L_UINT         uShow,
                                                          L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetTitlebarProperties  (HDISPCONTAINER              hCon,
                                                          pDISPCONTAINERTITLEBARPROPS pDispContainerTitlebarProps,
                                                          L_UINT                      uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetTitlebarProperties  (HDISPCONTAINER              hCon,
                                                          pDISPCONTAINERTITLEBARPROPS pDispContainerTitlebarProps,
                                                          L_UINT                      uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetIconProperties      (HDISPCONTAINER                  hCon,
                                                          L_INT                           nIconIndex,
                                                          pDISPCONTAINERTITLEBARICONPROPS pDispContainerIconProps,
                                                          L_UINT                          uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetIconProperties      (HDISPCONTAINER                  hCon,
                                                          L_INT                           nIconIndex,
                                                          pDISPCONTAINERTITLEBARICONPROPS pDispContainerIconProps,
                                                          L_UINT                          uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerCheckTitlebarIcon      (HDISPCONTAINER hCon,
                                                          L_INT          nCellIndex,
                                                          L_INT          nSubCellIndex,
                                                          L_INT          nIconIndex,
                                                          L_BOOL         bCheck,
                                                          L_UINT         uFlags);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerIsTitlebarIconChecked  (HDISPCONTAINER hCon,
                                                          L_INT          nCellIndex,
                                                          L_INT          nSubCellIndex,
                                                          L_INT          nIconIndex,
                                                          L_UINT         uFlags);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerIsCellAnimated        (HDISPCONTAINER hCon,
                                                          L_INT          nCellIndex,
                                                          L_UINT         uFlags);

// This function retrieves the viewer ruler unit
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetRulerUnit           (HDISPCONTAINER hCon, 
                                                          L_UINT * puUnit,
                                                          L_UINT         uFlags);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerIsTitlebarEnabled     (HDISPCONTAINER hCon,
                                                          L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetSelectedAnnotationAttributes (HDISPCONTAINER           hCon,
                                                                  L_INT                    nCellIndex,
                                                                  L_INT                    nSubCellIndex,
                                                                  pDISPCONTAINERANNATTRIBS pAnnAttributes,
                                                                  L_UINT                   uFlags);


L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerReverseAnnotationContainer(HDISPCONTAINER hCon,
                                                                         L_INT          nCellIndex,
                                                                         L_INT          nSubCellIndex,
                                                                         L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerFlipAnnotationContainer(HDISPCONTAINER hCon,
                                                                      L_INT          nCellIndex,
                                                                      L_INT          nSubCellIndex,
                                                                      L_UINT         uFlags);

// New functionalities
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerRotateAnnotationContainer(HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_DOUBLE       dAngle,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetAnnotationCallBack(HDISPCONTAINER                  hCon,
                                                                    DISPCONTAINERANNOTATIONCALLBACK pfnCallBack,
                                                                    L_VOID         *                pUserData);

// This function gets the callback for the annotation click.
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetAnnotationCallBack(HDISPCONTAINER                    hCon,
                                                                    DISPCONTAINERANNOTATIONCALLBACK * ppfnCallBack,
                                                                    LPVOID *                          ppUserData);

// This function sets the callback for the region.
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetRegionCallBack(HDISPCONTAINER              hCon,
                                                                DISPCONTAINERREGIONCALLBACK pfnCallBack,
                                                                L_VOID *                    pUserData);
// This function gets the callback for the region.
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetRegionCallBack(HDISPCONTAINER                hCon,
                                                                DISPCONTAINERREGIONCALLBACK * ppfnCallBack,
                                                                LPVOID *                      ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetAnnotationCreatedCallBack(HDISPCONTAINER                         hCon,
                                                                           DISPCONTAINERANNOTATIONCREATEDCALLBACK pfnCallBack,
                                                                           L_VOID *                               pUserData);

// This function gets the callback for the annnotaion creation.
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetAnnotationCreatedCallBack(HDISPCONTAINER                           hCon,
                                                                           DISPCONTAINERANNOTATIONCREATEDCALLBACK * ppfnCallBack,
                                                                           LPVOID *                                 ppUserData);

// This function sets the callback for when the active sub cell changed.
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetActiveSubCellChangedCallBack(HDISPCONTAINER                    hCon,
                                                                              DISPCONTAINERACTIVESUBCELLCHANGED pfnCallBack,
                                                                              L_VOID *                          pUserData);

// This function gets the callback for when the active sub cell changed.
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetActiveSubCellChangedCallBack(HDISPCONTAINER                      hCon,
                                                                              DISPCONTAINERACTIVESUBCELLCHANGED * ppfnCallBack,
                                                                              LPVOID *                            ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetBitmapPixel(HDISPCONTAINER hCon,
                                                             L_INT          nCellIndex,
                                                             L_INT          nSubCellIndex,
                                                             LPPOINT        pSrcPoint,
                                                             LPPOINT        pBitmapPoint,
                                                             L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSaveAnnotationA(HDISPCONTAINER hCon,
                                                              LPSTR          pFileName,
                                                              L_INT          nCellIndex,
                                                              L_INT          nSubCellIndex,
                                                              L_INT          nStartPage,
                                                              L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerLoadAnnotationA(HDISPCONTAINER hCon,
                                                              LPSTR          pFileName,
                                                              L_INT          nCellIndex,
                                                              L_INT          nSubCellIndex,
                                                              L_INT          nStartPage,
                                                              L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerLoadRegionA(HDISPCONTAINER hCon,
                                                          LPSTR          pFileName,
                                                          L_INT          nCellIndex,
                                                          L_INT          nSubCellIndex,
                                                          L_INT          nStartPage,
                                                          L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSaveRegionA(HDISPCONTAINER hCon,
                                                          LPSTR          pFileName,
                                                          L_INT          nCellIndex,
                                                          L_INT          nSubCellIndex,
                                                          L_INT          nStartPage,
                                                          L_UINT         uFlags);


L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetCellRegionHandle(HDISPCONTAINER hCon,
                                                                  L_INT          nCellIndex,
                                                                  L_INT          nSubCellIndex,
                                                                  L_HRGN         hRgn,
                                                                  L_UINT         uCombineMode,
                                                                  L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellRegionHandle(HDISPCONTAINER hCon,
                                                                  L_INT          nCellIndex,
                                                                  L_INT          nSubCellIndex,
                                                                  L_HRGN *       phRgn,
                                                                  L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetAnnotationContainer(HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_INT          nSubCellIndex,
                                                                     HANNOBJECT *   PhAnnContainer,
                                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetAnnotationContainer(HDISPCONTAINER hCon,
                                                                     L_INT          nCellIndex,
                                                                     L_INT          nSubCellIndex,
                                                                     HANNOBJECT     hAnnContainer,
                                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetMouseCallBack    (HDISPCONTAINER             hCon,
                                                                   DISPCONTAINERMOUSECALLBACK pfnCallBack,
                                                                   L_VOID         *           pUserData);

// This function sets the callback for the annnotaion creation.
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetMouseCallBack    (HDISPCONTAINER               hCon,
                                                                   DISPCONTAINERMOUSECALLBACK * ppfnCallBack,
                                                                   LPVOID *                     ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetSubCellTagA(HDISPCONTAINER hCon,
                                                            L_INT          nCellIndex,
                                                            L_INT          nSubCellIndex,
                                                            L_UINT         uRow,
                                                            L_UINT         uAlign,
                                                            L_UINT         uType,
                                                            LPSTR          pString,
                                                            L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetSubCellTag(HDISPCONTAINER     hCon,
                                                            L_INT              nCellIndex,
                                                            L_INT              nSubCellIndex,
                                                            L_UINT             uRow,
                                                            L_UINT             uAlign,
                                                            DISPCELLTAGINFO *  pTagInfo,
                                                            L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerEditSubCellTag(HDISPCONTAINER     hCon,
                                                             L_INT              nCellIndex,
                                                             L_INT              nSubCellIndex,
                                                             L_UINT             uRow,
                                                             L_UINT             uAlign,
                                                             DISPCELLTAGINFO *  pTagInfo,
                                                             L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerDeleteSubCellTag(HDISPCONTAINER     hCon,
                                                               L_INT              nCellIndex,
                                                               L_INT              nSubCellIndex,
                                                               L_UINT             uRow,
                                                               L_UINT             uAlign,
                                                               L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellTag        (HDISPCONTAINER     hCon,
                                                                 L_INT              nCellIndex,
                                                                 L_UINT             uRow,
                                                                 L_UINT             uAlign,
                                                                 DISPCELLTAGINFO *  pTagInfo,
                                                                 L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerDeleteCellTag     (HDISPCONTAINER     hCon,
                                                                 L_INT              nCellIndex,
                                                                 L_UINT             uRow,
                                                                 L_UINT             uAlign,
                                                                 L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerEditCellTag       (HDISPCONTAINER     hCon,
                                                                 L_INT              nCellIndex,
                                                                 L_UINT             uRow,
                                                                 L_UINT             uAlign,
                                                                 DISPCELLTAGINFO *  pTagInfo,
                                                                 L_UINT             uFlags);

L_LTIVW_API HBITMAP EXT_FUNCTION L_DispContainerPrintCell       (HDISPCONTAINER hCon,
                                                                 L_INT          nCellIndex,
                                                                 L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetPrePaintCallBack  (HDISPCONTAINER                 hCon,
                                                                    DISPCONTAINERPREPAINTCALLBACK  pfnCallBack,
                                                                    LPVOID                         pUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetPrePaintCallBack  (HDISPCONTAINER                  hCon,
                                                                    DISPCONTAINERPREPAINTCALLBACK * ppfnCallBack,
                                                                    LPVOID *                        ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetPostPaintCallBack (HDISPCONTAINER                 hCon,
                                                                    DISPCONTAINERPOSTPAINTCALLBACK pfnCallBack,
                                                                    LPVOID                         pUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetPostPaintCallBack (HDISPCONTAINER                   hCon,
                                                                    DISPCONTAINERPOSTPAINTCALLBACK * ppfnCallBack,
                                                                    LPVOID *                         ppUserData);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerHandlePalette           (HDISPCONTAINER hCon,
                                                                        L_UINT         uMessage,
                                                                        WPARAM         wParam,
                                                                        L_UINT         uFlags);


#if defined (LEADTOOLS_V16_OR_LATER)

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerEnableCellLowMemoryUsage(HDISPCONTAINER hCon,
                                                                       L_INT          nCellIndex,
                                                                       L_INT          nHiddenCount,
                                                                       L_INT          nFrameCount,
                                                                       DISPCONTAINERBITMAPINFO * pBitmapInfo,
                                                                       L_BOOL         bEnable,
                                                                       L_UINT         uFlags);


L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetLowMemoryUsageCallBack(HDISPCONTAINER                      hCon,
                                                                        DISPCONTAINERFRAMEREQUESTEDCALLBACK pfnCallBack,
                                                                        LPVOID                              pUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetLowMemoryUsageCallBack(HDISPCONTAINER                        hCon,
                                                                        DISPCONTAINERFRAMEREQUESTEDCALLBACK * ppfnCallBack,
                                                                        LPVOID *                              ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetRequestedImage        (HDISPCONTAINER   hCon,
                                                                        L_INT            nCellIndex,
                                                                        pBITMAPHANDLE    pBitmaps,
                                                                        L_INT          * pBitmapIndexes,
                                                                        L_INT            nLength,
                                                                        L_UINT           uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerFreezeSubCell            (HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_BOOL         bFreeze,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerIsSubCellFrozen         (HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellScaleMode         (HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_UINT *       puScaleMode,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetCellScale             (HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_DOUBLE       dScale,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetCellScaleMode         (HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_UINT         uScaleMode,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetCellScale             (HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_DOUBLE *     pdScale,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerUpdateCellView           (HDISPCONTAINER hCon,
                                                                        HWND           hWnd,
                                                                        L_INT          nCellIndex,
                                                                        L_UINT         uFlags);

L_LTIVW_API HBITMAP EXT_FUNCTION L_DispContainerPrintSubCell           (HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerCalibrateCell            (HDISPCONTAINER hCon,
                                                                        L_INT          nCellIndex,
                                                                        L_INT          nSubCellIndex,
                                                                        L_DOUBLE       dLength,
                                                                        L_UINT         uUnit,
                                                                        L_DOUBLE       dTargetLength,
                                                                        L_UINT         uTargetUnit,
                                                                        L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetBitmapListInfo        (HDISPCONTAINER            hCon,
                                                                        L_INT                     nCellIndex,
                                                                        L_INT                     nSubCellIndex,
                                                                        DISPCONTAINERBITMAPINFO * pBitmapInfo,
                                                                        L_UINT                    uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetAnimationStartedCallBack (HDISPCONTAINER                        hCon,
                                                                           DISPCONTAINERANIMATIONSTARTEDCALLBACK pfnCallBack,
                                                                           LPVOID                                pUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetAnimationStoppedCallBack (HDISPCONTAINER                       hCon,
                                                                           DISPCONTAINERANIMATIONSTOPPEDCALLBACK pfnCallBack,
                                                                           LPVOID                                pUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetAnimationStartedCallBack (HDISPCONTAINER                          hCon,
                                                                           DISPCONTAINERANIMATIONSTARTEDCALLBACK * ppfnCallBack,
                                                                           LPVOID *                                ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetAnimationStoppedCallBack (HDISPCONTAINER                          hCon,
                                                                            DISPCONTAINERANIMATIONSTOPPEDCALLBACK * ppfnCallBack,
                                                                            LPVOID *                                ppUserData);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetDefaultWindowLevelValues (HDISPCONTAINER     hCon,
                                                                           L_INT              nCellIndex,
                                                                           L_INT              nSubCellIndex,
                                                                           L_INT              nWidth,
                                                                           L_INT              nCenter,
                                                                           L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetDefaultWindowLevelValues (HDISPCONTAINER     hCon,
                                                                           L_INT              nCellIndex,
                                                                           L_INT              nSubCellIndex,
                                                                           L_INT *            pnWidth,
                                                                           L_INT *            pnCenter,
                                                                           L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerResetWindowLevelValues      (HDISPCONTAINER     hCon,
                                                                           L_INT              nCellIndex,
                                                                           L_INT              nSubCellIndex,
                                                                           L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerInvertBitmap                (HDISPCONTAINER     hCon,
                                                                           L_INT              nCellIndex,
                                                                           L_INT              nSubCellIndex,
                                                                           L_UINT             uFlags);

L_LTIVW_API L_BOOL EXT_FUNCTION L_DispContainerIsBitmapInverted           (HDISPCONTAINER     hCon,
                                                                           L_INT              nCellIndex,
                                                                           L_INT              nSubCellIndex,
                                                                           L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerRotateBitmapPerspective     (HDISPCONTAINER     hCon,
                                                                           L_INT              nCellIndex,
                                                                           L_INT              nSubCellIndex,
                                                                           L_INT              nAngle,
                                                                           L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerGetRotateBitmapPerspectiveAngle(HDISPCONTAINER     hCon,
                                                                              L_INT              nCellIndex,
                                                                              L_INT              nSubCellIndex,
                                                                              L_INT *            pnAngle,
                                                                              L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerRemoveBitmapRegion            (HDISPCONTAINER     hCon,
                                                                             L_INT              nCellIndex,
                                                                             L_INT              nSubCellIndex,
                                                                             L_UINT             uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerBeginUpdate                   (HDISPCONTAINER         hCon,
                                                                             L_UINT                 uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerEndUpdate                     (HDISPCONTAINER         hCon,
                                                                             L_UINT                 uFlags);
#endif //LEADTOOLS_V16_OR_LATER

#if defined(FOR_UNICODE)
L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetCellTag         (HDISPCONTAINER hCon,
                                                     L_INT          nCellIndex,
                                                     L_UINT         uRow,
                                                     L_UINT         uAlign,
                                                     L_UINT         uType,
                                                     LPTSTR         pString,
                                                     L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSaveAnnotation(HDISPCONTAINER hCon,
                                                             LPTSTR         pFileName,
                                                             L_INT          nCellIndex,
                                                             L_INT          nSubCellIndex,
                                                             L_INT          nStartPage,
                                                             L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerLoadAnnotation(HDISPCONTAINER hCon,
                                                             LPTSTR         pFileName,
                                                             L_INT          nCellIndex,
                                                             L_INT          nSubCellIndex,
                                                             L_INT          nStartPage,
                                                             L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerLoadRegion    (HDISPCONTAINER hCon,
                                                             LPTSTR         pFileName,
                                                             L_INT          nCellIndex,
                                                             L_INT          nSubCellIndex,
                                                             L_INT          nStartPage,
                                                             L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSaveRegion    (HDISPCONTAINER hCon,
                                                             LPTSTR         pFileName,
                                                             L_INT          nCellIndex,
                                                             L_INT          nSubCellIndex,
                                                             L_INT          nStartPage,
                                                             L_UINT         uFlags);

L_LTIVW_API L_INT EXT_FUNCTION L_DispContainerSetSubCellTag(HDISPCONTAINER hCon,
                                                            L_INT          nCellIndex,
                                                            L_INT          nSubCellIndex,
                                                            L_UINT         uRow,
                                                            L_UINT         uAlign,
                                                            L_UINT         uType,
                                                            LPTSTR         pString,
                                                            L_UINT         uFlags);

#else
#define L_DispContainerSetSubCellTag L_DispContainerSetSubCellTagA
#define L_DispContainerSetCellTag L_DispContainerSetCellTagA
#define L_DispContainerSaveRegion L_DispContainerSaveRegionA
#define L_DispContainerLoadRegion L_DispContainerLoadRegionA
#define L_DispContainerSaveAnnotation L_DispContainerSaveAnnotationA
#define L_DispContainerLoadAnnotation L_DispContainerLoadAnnotationA
#endif // #if defined(FOR_UNICODE)


#undef L_HEADER_ENTRY
#include "ltpck.h"

#endif
