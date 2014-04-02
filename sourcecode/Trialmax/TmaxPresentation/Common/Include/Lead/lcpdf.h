/*-------------------------------------------------------------------------------------------*
 *  File       | lcpdf.h                                                                     *
 *  Product    | LEAD Compact PDF SDK v1.0
 *  Company    | LEAD Technologies, Inc.                                                     *
 *-------------|-----------------------------------------------------------------------------*
 *  Programmer |                                                                             *
 *  EMail      |                                                                             *
 *  Date       | 10/6/2004                                                                   *
 *-------------------------------------------------------------------------------------------*/
#ifndef __LEAD_COMPACT_PDF_
#define __LEAD_COMPACT_PDF_

#if !defined(L_LCPDF_API)
   #if !defined(FOR_MANAGED)
      #define L_LCPDF_API __declspec(dllimport)
   #else
      #define L_LCPDF_API
   #endif // #if !defined(FOR_MANAGED)
#endif // #if !defined(L_LCPDF_API)


#define L_HEADER_ENTRY
#include "Ltpck.h"


typedef HANDLE LCP_HANDLE;
typedef HANDLE LCPDEST_HANDLE;
typedef HANDLE LCPACTION_HANDLE;
typedef HANDLE LCPOUTLINE_HANDLE;
typedef HANDLE LCPANNOTATION_HANDLE;
typedef HANDLE LCPTEMPLATE_HANDLE;
typedef HANDLE LCPIMAGE_HANDLE;
typedef HANDLE LCPCOLORSPACE_HANDLE;


/* Annotation Flags */ 
#define  LCP_ANNOT_FLAG_INVISIBLE            0x00000001
#define  LCP_ANNOT_FLAG_HIDDEN               0x00000002
#define  LCP_ANNOT_FLAG_PRINT                0x00000004
#define  LCP_ANNOT_FLAG_NOZOOM               0x00000008
#define  LCP_ANNOT_FLAG_NOROTATE             0x00000010
#define  LCP_ANNOT_FLAG_NOVIEW               0x00000020
#define  LCP_ANNOT_FLAG_READONLY             0x00000040
#define  LCP_ANNOT_FLAG_TOGGLE_NO_VIEW       0x00000080

/* Data types */ 
typedef enum  tagLCPDFDOCVERSION
{
   LCPDFDOCVERSION_V1_2 = 0,
   LCPDFDOCVERSION_V1_3,
   LCPDFDOCVERSION_V1_4,
   LCPDFDOCVERSION_V1_5

} LCPDFDOCVERSION, *LPLCPDFDOCVERSION;

typedef enum  tagLCPDF_ALIGNMENT
{
   LCPDF_ALIGNMENT_LEFT = 0,
   LCPDF_ALIGNMENT_CENTER,
   LCPDF_ALIGNMENT_RIGHT

} LCPDF_ALIGNMENT, *LPLCPDF_ALIGNMENT;

typedef struct tagLCPDF_POINT
{
   L_FLOAT  x;
   L_FLOAT  y;

} LCPDF_POINT, *LPLCPDF_POINT;

typedef struct tagLCPDF_RECT
{
   L_FLOAT  left;
   L_FLOAT  lower;
   L_FLOAT  right;
   L_FLOAT  upper;

} LCPDF_RECT, *LPLCPDF_RECT;

typedef struct tagLCPDF_PAGECOLORINFO
{
   COLORREF  rgbClr;
   L_FLOAT   fWidth;
   L_BOOL    bSolid;

   L_INT     nDashes;   /* array count */ 
   L_INT    *pnDashes;

} LCPDF_PAGECOLORINFO, *LPLCPDF_PAGECOLORINFO;

typedef enum _PAGEMODE
{
   LCPDFDOCPAGEMODE_NONE = 0,  /* default - neither outline nor thumbnails will be visible */
   LCPDFDOCPAGEMODE_OUTLINES,  /* open the doc with outline visible */
   LCPDFDOCPAGEMODE_THUMBS,    /* open the doc with thumbnails visible */
   LCPDFDOCPAGEMODE_FULLSCREEN /* open the doc in full screen mode */

}LCPDFDOCPAGEMODE, *pLCPDFDOCPAGEMODE;

typedef enum _PAGELAYOUT
{
   LCPDFDOCPAGELAYOUT_SINGLE = 0, /* default - one page at a time */
   LCPDFDOCPAGELAYOUT_1COLUMN,   /* display pages in one column */
   LCPDFDOCPAGELAYOUT_2LCOLUMN,  /* 2-column display, with odd pages on the left */
   LCPDFDOCPAGELAYOUT_2RCOLUMN,  /* 2-column display, with odd pages on the right */

}LCPDFDOCPAGELAYOUT, *pLCPDFDOCPAGELAYOUT;

typedef enum
{
   LCPDFDOCMAGNIFICATION_DEFAULT = 0,
   LCPDFDOCMAGNIFICATION_FITVISIBLE,
   LCPDFDOCMAGNIFICATION_FITWIDTH,   
   LCPDFDOCMAGNIFICATION_FITINWINDOW,
   LCPDFDOCMAGNIFICATION_CUSTOM
      
}LCPDFDOCMAGNIFICATION;

typedef struct tagVIEWERPREF
{
   LCPDFDOCPAGELAYOUT  PageLayout;
   LCPDFDOCPAGEMODE    PageMode;
   L_BOOL             bHideToolbar;
   L_BOOL             bHideMenubar;
   L_BOOL             bHideWindowUI;
   L_INT              nInitialPage;           /* initial view at page */ 
   LCPDFDOCMAGNIFICATION magnification;
   L_INT              nCustomMagnification;

}LCPDFDOCVIEWERPREF, *pLCPDFDOCVIEWERPREF;

typedef struct tagLCPDFDOCSECURITY
{
   L_CHAR   *pszUserPassword;
   L_CHAR   *pszOwnerPassword;
   L_BOOL    b128bit;

   /* premissions */ 
   L_BOOL    bPrintEnabled;
   L_BOOL    bHighQuality;

   L_BOOL    bCopyEnabled;

   L_BOOL    bEditEnabled;   
   L_BOOL    bAnnotEnabled;
   L_BOOL    bFormFillEnabled;   
   L_BOOL    bAssemblyEnabled;

} LCPDFDOCSECURITY, *pLCPDFDOCSECURITY;

typedef enum tagLCPDFDOC_LINECAP
{
   LCPDFDOC_LINECAP_FLAT = 0,
   LCPDFDOC_LINECAP_ROUND,
   LCPDFDOC_LINECAP_SQUARE

} LCPDFDOC_LINECAP, *LPLCPDFDOC_LINECAP;

typedef enum tagLCPDFDOC_LINEJOIN
{
   LCPDFDOC_LINEJOIN_MITER = 0,
   LCPDFDOC_LINEJOIN_ROUND,
   LCPDFDOC_LINEJOIN_BEVEL

} LCPDFDOC_LINEJOIN, *LPLCPDFDOC_LINEJOIN;

typedef struct tagLCPDF_MATRIX
{
    L_FLOAT a; L_FLOAT b;		/*  a   b   0  */
    L_FLOAT c; L_FLOAT d;		/*  c   d   0  */
    L_FLOAT x; L_FLOAT y;		/*  x   y   1  */

} LCPDF_MATRIX, *LPLCPDF_MATRIX;



typedef enum tagLCPDF_FILLMETHOD
{
   LCPDF_FILLMETHOD_WINDING = 0,
   LCPDF_FILLMETHOD_ODDEVEN

} LCPDF_FILLMETHOD, *LPLCPDF_FILLMETHOD;

typedef enum tagLCPDF_COLORSPACES
{
   LCPDF_COLORSPACES_GRAY = 0,
   LCPDF_COLORSPACES_RGB,
   LCPDF_COLORSPACES_CMYK,
   LCPDF_COLORSPACES_INDEXED 

} LCPDF_COLORSPACES, *LPLCPDF_COLORSPACES;

typedef enum  tagLCPDF_TEXTRENDER_MODE
{
   LCPDF_TEXTRENDER_MODE_STROKE = 0,
   LCPDF_TEXTRENDER_MODE_FILL,
   LCPDF_TEXTRENDER_MODE_STROKEANDFILL

} LCPDF_TEXTRENDER_MODE, *LPLCPDF_TEXTRENDER_MODE;

typedef enum  tagLCPDF_COLORIMAGE_COMPRESSION
{
   LCPDF_COLORIMAGE_COMPRESSION_JPG = 0,
   LCPDF_COLORIMAGE_COMPRESSION_ZIP,
   LCPDF_COLORIMAGE_COMPRESSION_LZW

} LCPDF_COLORIMAGE_COMPRESSION, *LPLCPDF_COLORIMAGE_COMPRESSION;

typedef enum  tagLCPDF_GRAYIMAGE_COMPRESSION
{
   LCPDF_GRAYIMAGE_COMPRESSION_JPG = 0,
   LCPDF_GRAYIMAGE_COMPRESSION_ZIP,
   LCPDF_GRAYIMAGE_COMPRESSION_LZW

} LCPDF_GRAYIMAGE_COMPRESSION, *LPLCPDF_GRAYIMAGE_COMPRESSION;

typedef enum  tagLCPDF_MONOIMAGE_COMPRESSION
{
   LCPDF_MONOIMAGE_COMPRESSION_CCITT_G3_1D = 0,
   LCPDF_MONOIMAGE_COMPRESSION_CCITT_G3_2D,
   LCPDF_MONOIMAGE_COMPRESSION_CCITT_G4,
   LCPDF_MONOIMAGE_COMPRESSION_CCITT_RUNLENGTH,
   LCPDF_MONOIMAGE_COMPRESSION_CCITT_ZIP,
   LCPDF_MONOIMAGE_COMPRESSION_CCITT_JBIG2

} LCPDF_MONOIMAGE_COMPRESSION, *LPLCPDF_MONOIMAGE_COMPRESSION;

typedef enum  tagLCP_PAGEMODE
{
   LCP_PAGEMODE_USENONE = 0,
   LCP_PAGEMODE_USEOUTLINES,
   LCP_PAGEMODE_USETHUMBS,
   LCP_PAGEMODE_FULLSCREEN

} LCP_PAGEMODE, *LPLCP_PAGEMODE;


typedef enum  tagLCP_PAGELAYOUT
{
   LCP_PAGELAYOUT_SINGLEPAGE = 0,
   LCP_PAGELAYOUT_ONECOLUMN,
   LCP_PAGELAYOUT_TWOCOLUMNLEFT,
   LCP_PAGELAYOUT_TWOCOLUMNRIGHT

} LCP_PAGELAYOUT, *LPLCP_PAGELAYOUT;


typedef enum  tagLCP_NONFULLSCRN_PAGEMODE
{
   LCP_NONFULLSCRN_PAGEMODE_USENONE = 0,
   LCP_NONFULLSCRN_PAGEMODE_USEOUTLINES,
   LCP_NONFULLSCRN_PAGEMODE_USETHUMBS

} LCP_NONFULLSCRN_PAGEMODE, *LPLCP_NONFULLSCRN_PAGEMODE;

typedef  enum  tagLCP_PAGE_BOX_TYPES
{
   LCP_PAGE_BOX_TYPES_CROPBOX = 0,
   LCP_PAGE_BOX_TYPES_MEDIABOX,
   LCP_PAGE_BOX_TYPES_BLEEDBOX,
   LCP_PAGE_BOX_TYPES_TRIMBOX,
   LCP_PAGE_BOX_TYPES_ARTBOX

} LCP_PAGE_BOX_TYPES, *LPLCP_PAGE_BOX_TYPES;

typedef struct tagLCP_VIEWER_PREFERENCES
{
   L_BOOL  bHideToolbar;
   L_BOOL  bHideMenubar;
   L_BOOL  bHideWindowUI;
   L_BOOL  bFitWindow;
   L_BOOL  bCenterWindow;
   L_BOOL  bDisplayDocTitle;
   LCP_NONFULLSCRN_PAGEMODE nonFullScrnPageMode;
   L_BOOL  bR2LDirection;
   LCP_PAGE_BOX_TYPES  ViewArea;
   LCP_PAGE_BOX_TYPES  ClipArea;
   LCP_PAGE_BOX_TYPES  PrintArea;
   LCP_PAGE_BOX_TYPES  PrintClip;
   
} LCP_VIEWER_PREFERENCES, *LPLCP_VIEWER_PREFERENCES;

/* DEF:Annotation */ 
typedef enum  tagLCP_ANNOT_TYPE
{
         LCP_ANNOT_TYPE_TEXT = 0,
         LCP_ANNOT_TYPE_LINK,
/*[1.3]*/LCP_ANNOT_TYPE_FREETEXT,
/*[1.3]*/LCP_ANNOT_TYPE_LINE,
/*[1.3]*/LCP_ANNOT_TYPE_SQUARE,
/*[1.3]*/LCP_ANNOT_TYPE_CIRCLE,
/*[1.3]*/LCP_ANNOT_TYPE_HIGHLIGHT,
/*[1.3]*/LCP_ANNOT_TYPE_UNDERLINE,
/*[1.4]*/LCP_ANNOT_TYPE_SQUIGGLY,
/*[1.3]*/LCP_ANNOT_TYPE_STRIKEOUT,
/*[1.3]*/LCP_ANNOT_TYPE_STAMP,
/*[1.3]*/LCP_ANNOT_TYPE_INK,
/*[1.3]*/LCP_ANNOT_TYPE_POPUP,
/*[1.3]*/LCP_ANNOT_TYPE_FILEATTACHMENT,
/*[1.2]*/LCP_ANNOT_TYPE_SOUND,
/*[1.2]*/LCP_ANNOT_TYPE_MOVIE ,
/*[1.2]*/LCP_ANNOT_TYPE_WIDGET,
/*[1.4]*/LCP_ANNOT_TYPE_PRINTERMARK,
/*[1.3]*/LCP_ANNOT_TYPE_TRAPNET

} LCP_ANNOT_TYPE, *LPLCP_ANNOT_TYPE;


typedef enum tagLCP_ANNOT_BORDER_STYLE
{
   LCP_ANNOT_BORDER_STYLE_SOLID = 0,
   LCP_ANNOT_BORDER_STYLE_DASHED,
   LCP_ANNOT_BORDER_STYLE_BELEVED,
   LCP_ANNOT_BORDER_STYLE_INSET,
   LCP_ANNOT_BORDER_STYLE_UNDERLINE
} LCP_ANNOT_BORDER_STYLE, *LPLCP_ANNOT_BORDER_STYLE;

typedef enum tagLCP_ANNOT_ICON
{
   LCP_ANNOT_ICON_NOTE = 0,
   LCP_ANNOT_ICON_COMMENT,
   LCP_ANNOT_ICON_KEY,
   LCP_ANNOT_ICON_HELP,
   LCP_ANNOT_ICON_NEWPARAGRAPH,
   LCP_ANNOT_ICON_PARAGRAPH,
   LCP_ANNOT_ICON_INSERT

} LCP_ANNOT_ICON, *LPLCP_ANNOT_ICON;

typedef enum tagLCP_ANNOT_HIGHLIGHT
{
   LCP_ANNOT_HIGHLIGHT_NONE = 0,
   LCP_ANNOT_HIGHLIGHT_INVERT,
   LCP_ANNOT_HIGHLIGHT_OUTLINE,
   LCP_ANNOT_HIGHLIGHT_PUSH

} LCP_ANNOT_HIGHLIGHT, *LPLCP_ANNOT_HIGHLIGHT;

typedef enum  tagLCP_ANNOT_ALIGNMENT
{
   LCP_ANNOT_ALIGNMENT_LEFT = 0,
   LCP_ANNOT_ALIGNMENT_CENTER,
   LCP_ANNOT_ALIGNMENT_RIGHT

} LCP_ANNOT_ALIGNMENT, *LPLCP_ANNOT_ALIGNMENT;

typedef enum  tagLCP_ANNOT_LINEENDING
{
   LCP_ANNOT_LINEENDING_NONE = 0,
   LCP_ANNOT_LINEENDING_SQUARE,
   LCP_ANNOT_LINEENDING_CIRCLE,
   LCP_ANNOT_LINEENDING_DIAMOND,
   LCP_ANNOT_LINEENDING_OPENARROW,
   LCP_ANNOT_LINEENDING_CLOSEARROW

} LCP_ANNOT_LINEENDING, *LPLCP_ANNOT_LINEENDING;

typedef enum  tagLCP_ANNOT_STAMP_TYPE
{
   LCP_ANNOT_STAMP_TYPE_DRAFT = 0,
   LCP_ANNOT_STAMP_TYPE_APPROVED,
   LCP_ANNOT_STAMP_TYPE_EXPERIMENTAL,
   LCP_ANNOT_STAMP_TYPE_NOTAPPROVED,
   LCP_ANNOT_STAMP_TYPE_ASIS,
   LCP_ANNOT_STAMP_TYPE_EXPIRED,
   LCP_ANNOT_STAMP_TYPE_NOTFORPUBLICRELEASE,
   LCP_ANNOT_STAMP_TYPE_CONFIDENTIAL,
   LCP_ANNOT_STAMP_TYPE_FINAL,
   LCP_ANNOT_STAMP_TYPE_SOLD,
   LCP_ANNOT_STAMP_TYPE_DEPARTMENTAL,
   LCP_ANNOT_STAMP_TYPE_FORCOMMENT,
   LCP_ANNOT_STAMP_TYPE_TOPSECRET,   
   LCP_ANNOT_STAMP_TYPE_FORPUBLICRELEASE

} LCP_ANNOT_STAMP_TYPE, *LPLCP_ANNOT_STAMP_TYPE;

typedef enum tagLCP_ANNOT_ATTACHEMENT
{
   LCP_ANNOT_ATTACHEMENT_PUSHPIN = 0,
   LCP_ANNOT_ATTACHEMENT_GRAPH,
   LCP_ANNOT_ATTACHEMENT_PAPERCLIP,
   LCP_ANNOT_ATTACHEMENT_TAG

} LCP_ANNOT_ATTACHEMENT, *LPLCP_ANNOT_ATTACHEMENT;

typedef enum tagLCP_ANNOT_SOUND
{
   LCP_ANNOT_SOUND_SPEAKER = 0,
   LCP_ANNOT_SOUND_MICROPHONE

} LCP_ANNOT_SOUND, *LPLCP_ANNOT_SOUND;


typedef struct tagLCP_ANNOTATION_DATA
{
   L_WCHAR      *pwszContents;
   L_CHAR       *pszContents;
   LCPDF_RECT    rcRect;
   L_UINT32      dwFlags;

   LCP_ANNOT_BORDER_STYLE borderStyle;
   L_FLOAT       fBorderWidth;
   L_INT         nBorderDashes;
   L_INT        *pnBorderDashes;
   L_FLOAT       fXCorner;
   L_FLOAT       fYCorner;
   L_FLOAT       fWidth;

   COLORREF      rgbColor;
   L_FLOAT       fOpacity;

   L_WCHAR      *pwszLabel;
   L_CHAR       *pszLabel;

   LCPANNOTATION_HANDLE  hPopup;
   
   LCPACTION_HANDLE      hAction;   

   union 
   {
      struct
      {
         L_BOOL          bAnnOpen;
         LCP_ANNOT_ICON  icon;
      }Text;

      struct
      {
         LCPDEST_HANDLE  hDest;
         LCP_ANNOT_HIGHLIGHT highligh;
      }Link;

      struct
      {
         L_WCHAR  *pwszApp;
         L_CHAR   *pszApp;
         LCP_ANNOT_ALIGNMENT  align;
      }FreeText;

      struct
      {
         LCPDF_POINT   pt1;
         LCPDF_POINT   pt2;
         COLORREF      rgbColor;
         LCP_ANNOT_LINEENDING firstEnd;
         LCP_ANNOT_LINEENDING lastEnd;
      }Line;

      struct
      {
         COLORREF      rgbColor;         
      }Closed;

      struct
      {
         LCPDF_POINT pt1;
         LCPDF_POINT pt2;
         LCPDF_POINT pt3;
         LCPDF_POINT pt4;
      }Markup;

      struct
      {
         LCP_ANNOT_STAMP_TYPE  type;
      }Stamp;

      struct
      {
         L_UINT32     uCount;
         LCPDF_POINT *pPoints;
      }Ink;

      struct
      {
         LCPANNOTATION_HANDLE  hParent;
         L_BOOL                bOpen;
      }Popup;

      struct
      {
         L_WCHAR  *pwszPath;
         L_CHAR   *pszPath;
         LCP_ANNOT_ATTACHEMENT  icon;
      }Attachement;

      struct
      {
         L_WCHAR  *pwszPath;
         L_CHAR   *pszPath;
         LCP_ANNOT_SOUND  icon;
      }Sound;

      struct
      {
         L_WCHAR  *pwszPath;
         L_CHAR   *pszPath;
         L_BOOL    bPlay;
      }Movie;

      struct
      {
         LCP_ANNOT_HIGHLIGHT highlight;
      }Widget;

      
   }extra_setting;   

} LCP_ANNOTATION_DATA, *LPLCP_ANNOTATION_DATA;


typedef enum  tagLCP_ENCODING_TYPES
{
   LCP_ENCODING_ASCIIHEX = 0,
   LCP_ENCODING_ASCII85,
   LCP_ENCODING_LZW,
   LCP_ENCODING_FLATE,
   LCP_ENCODING_RUNLENGTH,
   LCP_ENCODING_CCITTFAX_G3_1D,
   LCP_ENCODING_CCITTFAX_G3_2D,
   LCP_ENCODING_CCITTFAX_G4,
   LCP_ENCODING_JBIG2,
   LCP_ENCODING_DCT,
   LCP_ENCODING_DCT_YUV422,
   LCP_ENCODING_DCT_YUV411,
   LCP_ENCODING_DCT_PROGRESSIVE,
   LCP_ENCODING_DCT_PROGRESSIVE_YUV422,
   LCP_ENCODING_DCT_PROGRESSIVE_YUV411

} LCP_ENCODING_TYPES, *LPLCP_ENCODING_TYPES;

typedef union tagLCP_ENCODING_PARAM
{
   struct
   {
      L_INT  nBPP;
      L_INT  nQFactor;
   } lzwParam, dctParam;

} LCP_ENCODING_PARAM, *LPLCP_ENCODING_PARAM;



/* functions */ 

L_LCPDF_API L_INT lcpInit( L_BOOL bCompressed, L_BOOL bAscii, LCP_HANDLE *phlcp );
L_LCPDF_API L_VOID lcpFree( LCP_HANDLE hlcp );

L_LCPDF_API L_INT lcpWriteMemory( LCP_HANDLE hlcp, L_UCHAR **ppData, L_UINT32 *pnLength );
L_LCPDF_API L_INT lcpWriteFile( LCP_HANDLE hlcp, HANDLE nLeadFileHandle );

L_LCPDF_API L_INT lcpSetDocInfo( LCP_HANDLE hlcp, L_CHAR *pszTitle, L_CHAR *pszSubject, L_CHAR *pszKeywords, L_CHAR *pszAuther, L_CHAR *pszCreater );
L_LCPDF_API L_INT lcpSetDocInfoW( LCP_HANDLE hlcp, L_WCHAR *pwszTitle, L_WCHAR *pwszSubject, L_WCHAR *pwszKeywords, L_WCHAR *pwszAuther, L_WCHAR *pwszCreater );

L_LCPDF_API L_INT lcpInsertPage( LCP_HANDLE hlcp, L_INT  nIndex, const LPLCPDF_RECT pRect );

L_LCPDF_API L_INT lcpAddRect( LCP_HANDLE  hlcp, LCPDF_RECT  Rect, COLORREF  rgbColor);

L_LCPDF_API L_INT lcpAddLeadImage( LCP_HANDLE hlcp, 
                                    pBITMAPHANDLE phBitmap, 
                                    const LPLCPDF_RECT pRect, 
                                    L_FLOAT angle, 
                                    const LPLCP_ENCODING_TYPES pEncTypes, 
                                    const LPLCP_ENCODING_PARAM pEncParam, 
                                    L_INT  uEncCount );
L_LCPDF_API L_INT lcpAddLeadImageInline( LCP_HANDLE hlcp, 
                                          pBITMAPHANDLE phBitmap, 
                                          const LPLCPDF_RECT pRect, 
                                          L_FLOAT angle, 
                                          const LPLCP_ENCODING_TYPES pEncTypes, 
                                          const LPLCP_ENCODING_PARAM pEncParam, 
                                          L_INT  uEncCount );


#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif
/* EOF */ 