/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  LEAD LDKRN.                                                          []*/
/*[]                                                                       []*/
/*[]                                                                       []*/
/*[]  Copyright (c) 1991-2000  by LEAD Technologies, Inc.                  []*/
/*[]  All Rights Reserved.                                                 []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/

#ifndef _LDKRN_H_
#define _LDKRN_H_

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]                    INCLUDES                                           []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/

#ifndef STRICT
   #define STRICT
#endif

#include "lttyp.h"
#include "lterr.h"
#include "ltkrn.h"
#include "lvkrn.h"

#define _HEADER_ENTRY_
#include "ltpck.h"

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]                    DEFINES                                            []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/

// edit mode defines 
#define   DOCEDIT_MODE_PREVIEW          0                // preview mode
#define   DOCEDIT_MODE_EDIT             1                // edit mode

// callback command defines 
#define   DOCEDIT_CMD_UPDATECOMMANDS    0                // update command 

// editor status values 
#define   DOCEDIT_SUPPORTED	         0x1               // the feature is supported 
#define   DOCEDIT_ENABLED	            0x2               // the feature is enabled   
#define   DOCEDIT_ISON	               0x4               // the feature is set on  
#define   DOCEDIT_ISOFF                0x8               // the feature is set off

// justify valuse
#define   DOCEDIT_JUSTIFYCENTER        0                  // Justify center
#define   DOCEDIT_JUSTIFYRIGHT         1                  // Justify right  
#define   DOCEDIT_JUSTIFYLEFT          2                  // Justify left

#define  DOC_MAX_PASSWORD_LEN          64   
// Events of the event CallBack
typedef enum
{
   DOC_EVENT_GETPASSWORD = 500   
}DOCUMENT_EVENT;

// Object alignement with adjacent text
typedef enum 
{
   DOCEDIT_OBJTEXTALIGN_ABSBOTTOM =0,
   DOCEDIT_OBJTEXTALIGN_ABSMIDDLE   ,
   DOCEDIT_OBJTEXTALIGN_BASELINE    ,
   DOCEDIT_OBJTEXTALIGN_BOTTOM      ,
   DOCEDIT_OBJTEXTALIGN_LEFT        ,
   DOCEDIT_OBJTEXTALIGN_MIDDLE      ,
   DOCEDIT_OBJTEXTALIGN_RIGHT       ,
   DOCEDIT_OBJTEXTALIGN_TEXTTOP     ,
   DOCEDIT_OBJTEXTALIGN_TOP         
} 
DOCEDIT_OBJTEXTALIGN;


/* Definitions */
#define DOCEDIT_DLG_AUTO_PROCESS                (0x80000000)


/* Dialog String Indices */
enum
{
   /* Generic control strings */
   DOCEDIT_DLGSTR_OK,
   DOCEDIT_DLGSTR_CANCEL,
   DOCEDIT_DLGSTR_HELP,

   /* Insert Vector/Raster dialog */
   DOCEDIT_DLGSTR_INSERT_RASTER_CAPTION,
   DOCEDIT_DLGSTR_INSERT_VECTOR_CAPTION,
   DOCEDIT_DLGSTR_INSERT_BUTTON_BROWSE,
   DOCEDIT_DLGSTR_INSERT_LABEL_WIDTH,
   DOCEDIT_DLGSTR_INSERT_LABEL_HEIGHT,
   DOCEDIT_DLGSTR_INSERT_LABEL_ALIGN,
   DOCEDIT_DLGSTR_INSERT_LABEL_HSPACE,
   DOCEDIT_DLGSTR_INSERT_LABEL_VSPACE,

   /* Insert dialog align combo */
   DOCEDIT_DLGSTR_INSERT_ALIGN_ABSBOTTOM,
   DOCEDIT_DLGSTR_INSERT_ALIGN_ABSMIDDLE,
   DOCEDIT_DLGSTR_INSERT_ALIGN_BASELINE,
   DOCEDIT_DLGSTR_INSERT_ALIGN_BOTTOM,
   DOCEDIT_DLGSTR_INSERT_ALIGN_LEFT,
   DOCEDIT_DLGSTR_INSERT_ALIGN_MIDDLE,
   DOCEDIT_DLGSTR_INSERT_ALIGN_RIGHT,
   DOCEDIT_DLGSTR_INSERT_ALIGN_TEXTTOP,
   DOCEDIT_DLGSTR_INSERT_ALIGN_TOP,

   /* Messages */
   DOCEDIT_DLGSTR_ERROR_NO_MEMORY,
   DOCEDIT_DLGSTR_ERROR_NO_BITMAP,
   DOCEDIT_DLGSTR_ERROR_MEMORY_TOO_LOW,
   DOCEDIT_DLGSTR_ERROR_FILE_LSEEK,
   DOCEDIT_DLGSTR_ERROR_FILE_WRITE,
   DOCEDIT_DLGSTR_ERROR_FILE_GONE,
   DOCEDIT_DLGSTR_ERROR_FILE_READ,
   DOCEDIT_DLGSTR_ERROR_INV_FILENAME,
   DOCEDIT_DLGSTR_ERROR_FILE_FORMAT,
   DOCEDIT_DLGSTR_ERROR_FILENOTFOUND,
   DOCEDIT_DLGSTR_ERROR_INV_RANGE,
   DOCEDIT_DLGSTR_ERROR_IMAGE_TYPE,

   DOCEDIT_DLGSTR_ERROR_INV_PARAMETER,
   DOCEDIT_DLGSTR_ERROR_FILE_OPEN,
   DOCEDIT_DLGSTR_ERROR_UNKNOWN_COMP,
   DOCEDIT_DLGSTR_ERROR_FEATURE_NOT_SUPPORTED,
   DOCEDIT_DLGSTR_ERROR_NOT_256_COLOR,

   DOCEDIT_DLGSTR_ERROR_DOC_NOT_INITIALIZED,
   DOCEDIT_DLGSTR_ERROR_DOC_HANDLE,
   DOCEDIT_DLGSTR_ERROR_DOC_EMPTY,
   DOCEDIT_DLGSTR_ERROR_DOC_INVALID_FONT,
   DOCEDIT_DLGSTR_ERROR_DOC_INVALID_PAGE,
   DOCEDIT_DLGSTR_ERROR_DOC_INVALID_RULE,
   DOCEDIT_DLGSTR_ERROR_DOC_INVALID_ZONE,
   DOCEDIT_DLGSTR_ERROR_DOC_TYPE_ZONE,
   DOCEDIT_DLGSTR_ERROR_DOC_INVALID_COLUMN,
   DOCEDIT_DLGSTR_ERROR_DOC_INVALID_LINE,
   DOCEDIT_DLGSTR_ERROR_DOC_INVALID_WORD,

   DOCEDIT_DLGSTR_ERROR_VECTOR_IS_LOCKED,
   DOCEDIT_DLGSTR_ERROR_VECTOR_IS_EMPTY,
   DOCEDIT_DLGSTR_ERROR_VECTOR_LAYER_NOT_FOUND,
   DOCEDIT_DLGSTR_ERROR_VECTOR_LAYER_IS_LOCKED,
   DOCEDIT_DLGSTR_ERROR_VECTOR_LAYER_ALREADY_EXISTS,
   DOCEDIT_DLGSTR_ERROR_VECTOR_OBJECT_NOT_FOUND,
   DOCEDIT_DLGSTR_ERROR_VECTOR_INVALID_OBJECT_TYPE,
   DOCEDIT_DLGSTR_ERROR_VECTOR_PEN_NOT_FOUND,
   DOCEDIT_DLGSTR_ERROR_VECTOR_BRUSH_NOT_FOUND,
   DOCEDIT_DLGSTR_ERROR_VECTOR_FONT_NOT_FOUND,
   DOCEDIT_DLGSTR_ERROR_VECTOR_BITMAP_NOT_FOUND,
   DOCEDIT_DLGSTR_ERROR_VECTOR_POINT_NOT_FOUND,
   DOCEDIT_DLGSTR_ERROR_VECTOR_CANT_ADD_TEXT,
   DOCEDIT_DLGSTR_ERROR_VECTOR_GROUP_NOT_FOUND,
   DOCEDIT_DLGSTR_ERROR_VECTOR_GROUP_ALREADY_EXISTS,

   DOCEDIT_DLGSTR_ERROR_DOCUMENT_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_MULTIMEDIA_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_MEDICAL_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_JBIG_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_JBIG_FILTER_MISSING,
   DOCEDIT_DLGSTR_ERROR_VECTOR_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_VECTOR_DXF_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_VECTOR_DWG_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_VECTOR_MISC_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_TAG_MISSING,
   DOCEDIT_DLGSTR_ERROR_VECTOR_DWF_NOT_ENABLED,
   DOCEDIT_DLGSTR_ERROR_PDF_NOT_ENABLED,

   DOCEDIT_DLGSTR_ERROR_CANNOTLOADBITMAP,
   DOCEDIT_DLGSTR_ERROR_CANNOTLOADVECTOR,

   /* Context Menu Items*/   
   DOCEDIT_DLGSTR_MENU_CUT,
   DOCEDIT_DLGSTR_MENU_COPY,
   DOCEDIT_DLGSTR_MENU_PASTE,
   DOCEDIT_DLGSTR_MENU_SELECTALL,
   DOCEDIT_DLGSTR_MENU_PRINT,
   DOCEDIT_DLGSTR_MENU_DELETE,

   DOCEDIT_DLGSTR_MAX
};


/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]                    TYPES                                              []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/

// Document Handle
typedef struct _DOCHANDLE
{
   L_VOID * pHandleData;
} DOCHANDLE , * pDOCHANDLE ;

// table structure
typedef  struct TAG_DocEditTbale
{
   L_UINT    uRows;              // number of table rows
   L_UINT    uColumns;           // number of table coloumns
   L_UINT    uCellWidth;         // cell width 
   L_UINT    uBorderSize;        // border size

}  DOCEDITTABLE,*pDOCEDITTABLE;

// valid format structure
typedef  struct TAG_VALIDFORMAT
{
   L_INT     nNumber;            // format numbers 
   L_CHAR ** ppFormat;           // array of valid format.

}  VALIDFORMAT,*pVALIDFORMAT;


// command structure
typedef  struct TAG_COMMANDDATA
{
   L_INT    nCmdID;              // command ID 
   L_CHAR*  pCommand;            // text of command 
   L_VOID*  pUserData;           // user data.

}  COMMANDDATA,*pCOMMANDDATA;


// Events Callback 
typedef L_INT (pEXT_CALLBACK pDOCEVENTCALLBACK)(pDOCHANDLE pDocHandle,L_UINT uEventType,L_VOID *pEventData,L_VOID *pUserData );

// Structure passed when document Event is DOC_EVENT_GETPASSWORD
typedef struct _DOCPASSWORDEVENT
{
   L_INT    nSize;
   L_CHAR   pszPassword[DOC_MAX_PASSWORD_LEN];
   
} DOCPASSWORDEVENTDATA, *pDOCPASSWORDEVENTDATA;

typedef struct _DOCPAGEMARGINS
{
   L_INT    nSize;
   L_FLOAT  fMarginLeft;
   L_FLOAT  fMarginRight;
   L_FLOAT  fMarginTop;
   L_FLOAT  fMarginBottom;
} DOCPAGEMARGINS , * pDOCPAGEMARGINS;


typedef struct _DOCEDITINSERTOPTIONS
{
   L_INT                nWidth;
   L_INT                nHeight;
   DOCEDIT_OBJTEXTALIGN ObjAlign;
   L_INT                nHSpace;
   L_INT                nVSpace;
}
DOCEDITINSERTOPTIONS, L_FAR *pDOCEDITINSERTOPTIONS;

/*[]-----------------------------------------------------------------------[]*/
/*[]                                                                       []*/
/*[]  Functions.                                                           []*/
/*[]                                                                       []*/
/*[]-----------------------------------------------------------------------[]*/

L_INT    EXT_FUNCTION L_DocInit(pDOCHANDLE pDoc);
L_VOID   EXT_FUNCTION L_DocFree(pDOCHANDLE pDoc);
L_INT    EXT_FUNCTION L_DocSetImagesPathName(LPCSTR lpPathName);
L_INT    EXT_FUNCTION L_DocGetImagesPathName(LPTSTR lpPathName);
L_INT    EXT_FUNCTION L_DocSetVectorPathName(LPCSTR lpPathName);
L_INT    EXT_FUNCTION L_DocGetVectorPathName(LPTSTR lpPathName);
L_INT    EXT_FUNCTION L_DocSetEventCallBack(pDOCHANDLE pDoc,pDOCEVENTCALLBACK pDocEventCallback,L_VOID *pUserData);
pDOCEVENTCALLBACK EXT_FUNCTION L_DocGetEventCallBack(pDOCHANDLE pDoc,L_VOID **ppUserData);

typedef L_INT ( pEXT_CALLBACK pCOMMANDCALLBACK)(pCOMMANDDATA pCommandData);

L_INT  EXT_FUNCTION L_DocEditSetMode(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_INT nEditMode);
L_INT  EXT_FUNCTION L_DocEditCopy(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditCut(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditDelete(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditPaste(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditRedo(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditSelectAll(pDOCHANDLE pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditUndo(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditRemoveFormat(pDOCHANDLE pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditBold(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
//L_INT  EXT_FUNCTION L_DocEditDlgFont(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditGetFontName(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_CHAR* pFontName,L_INT nSize);
L_INT  EXT_FUNCTION L_DocEditSetFontName(pDOCHANDLE  pDoc,L_UINT32* puStatusMode, L_CHAR* pFontName);
L_INT  EXT_FUNCTION L_DocEditGetFontSize(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_INT *pnSize);
L_INT  EXT_FUNCTION L_DocEditSetFontSize(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_INT nSize);
L_INT  EXT_FUNCTION L_DocEditItalic(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);  
L_INT  EXT_FUNCTION L_DocEditUnderLine(pDOCHANDLE  pDoc,L_UINT32* puStatusMode); 
L_INT  EXT_FUNCTION L_DocEditCreateHyperLink(pDOCHANDLE  pDoc,L_UINT32* puStatusMode, L_CHAR* pLink);
L_INT  EXT_FUNCTION L_DocEditGetHyperLink(pDOCHANDLE   pDoc,L_UINT32* puStatusMode, L_CHAR* pLink,L_INT nSize);
L_INT  EXT_FUNCTION L_DocEditUnlink(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
//L_INT  EXT_FUNCTION L_DocEditInsertImage(pDOCHANDLE   pDoc,L_UINT32* puStatusMode,L_CHAR * pImageName); 
//L_INT  EXT_FUNCTION L_DocEditDlgCreateImage(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_CHAR * pImageName,L_INT nSize); 
L_INT  EXT_FUNCTION L_DocEditSetBackColor(pDOCHANDLE  pDoc,L_UINT32* puStatusMode ,COLORREF cColor);
L_INT  EXT_FUNCTION L_DocEditGetBackColor(pDOCHANDLE  pDoc,L_UINT32* puStatusMode, COLORREF* pcColor);
L_INT  EXT_FUNCTION L_DocEditGetForeColor(pDOCHANDLE  pDoc,L_UINT32* puStatusMode, COLORREF* pcColor);
L_INT  EXT_FUNCTION L_DocEditSetForeColor(pDOCHANDLE  pDoc,L_UINT32* puStatusMode, COLORREF cColor);
L_INT  EXT_FUNCTION L_DocEditDeleteCells(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditDeleteCols(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditDeleteRows(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditInsertCell(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditInsertCol(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditInsertRow(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditInsertTable(pDOCHANDLE  pDoc, L_UINT32* puStatusMode,pDOCEDITTABLE pTable);
L_INT  EXT_FUNCTION L_DocEditMergeCells(pDOCHANDLE pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditSplitCell(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditPrint(pDOCHANDLE  pDoc,L_UINT32* puStatusMode); 
L_INT  EXT_FUNCTION L_DocEditDlgPrint(pDOCHANDLE    pDoc, L_UINT32* puStatusMode); 
L_INT  EXT_FUNCTION L_DocEditJustify(pDOCHANDLE pDoc,L_UINT32* puStatusMode,L_INT nJustify);
L_INT  EXT_FUNCTION L_DocEditShowBorders(pDOCHANDLE   pDoc,L_UINT32* puStatusMode,L_BOOL bBorder);
L_INT  EXT_FUNCTION L_DocEditDlgFind(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditBullets(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditNumbers(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditIndentParagraph(pDOCHANDLE  pDoc, L_UINT32* puStatusMode,L_BOOL bIndent);
L_INT  EXT_FUNCTION L_DocEditSetWndColor(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,COLORREF Color);
L_INT  EXT_FUNCTION L_DocEditGetWndColor(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,COLORREF* pColor);
L_INT  EXT_FUNCTION L_DocEditSetFormat(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_CHAR * pFormat);
L_INT  EXT_FUNCTION L_DocEditGetFormat(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_CHAR * pFormat,L_INT nSize);
L_INT  EXT_FUNCTION L_DocEditGetValidFormat(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,pVALIDFORMAT  pValidFormat);
L_INT  EXT_FUNCTION L_DocEditSetCommandCallBack(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,pCOMMANDCALLBACK pCommandCallBack,L_VOID * pUserData);
L_INT  EXT_FUNCTION L_DocEditGetWnd(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,HWND * phWnd);
HWND   EXT_FUNCTION L_DocEditCreateWnd(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,HWND hWndParent,RECT* pRect);
L_INT  EXT_FUNCTION L_DocEditPreTranslateMessage(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,MSG* pMsg);
L_INT  EXT_FUNCTION L_DocEditDestroyWnd(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT  EXT_FUNCTION L_DocEditSetPageWidth(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_INT  nWidth);
L_INT  EXT_FUNCTION L_DocEditGetPageWidth(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_INT* pnWidth);

L_INT EXT_FUNCTION L_DocEditSetPageMargins(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,pDOCPAGEMARGINS pDocPageMargins);
L_INT EXT_FUNCTION L_DocEditGetPageMargins(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,pDOCPAGEMARGINS pDocPageMargins);

L_INT  EXT_FUNCTION L_DocEditIsDirty(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,L_BOOL* pbDirty);
L_INT  EXT_FUNCTION L_DocEditRealize(pDOCHANDLE  pDoc,L_UINT32* puStatusMode,pBITMAPHANDLE pBitmapHandle);




L_INT EXT_FUNCTION L_DocEditAddVectorDrawing(pDOCHANDLE              pDoc,
                                             L_UINT32*               puStatusMode,
                                             pVECTORHANDLE           pVectorHandle,
                                             L_INT                   nWidth,
                                             L_INT                   nHeight,
                                             DOCEDIT_OBJTEXTALIGN    ObjAlign,
                                             L_INT                   nHSpace,
                                             L_INT                   nVSpace);
L_INT EXT_FUNCTION L_DocEditAddRasterImage( pDOCHANDLE           pDoc,
                                             L_UINT32*            puStatusMode,
                                             pBITMAPHANDLE        pBitmap,
                                             L_INT                nWidth,
                                             L_INT                nHeight,
                                             DOCEDIT_OBJTEXTALIGN ObjAlign,
                                             L_INT                nHSpace,
                                             L_INT                nVSpace);

L_INT EXT_FUNCTION L_DocDlgInsertRaster( HWND hWnd, pDOCHANDLE pDocument, pBITMAPHANDLE pBitmap, pDOCEDITINSERTOPTIONS pOptions, L_UINT32 dwFlags );
L_INT EXT_FUNCTION L_DocDlgInsertVector( HWND hWnd, pDOCHANDLE pDocument, pVECTORHANDLE pVector, pDOCEDITINSERTOPTIONS pOptions, L_UINT32 dwFlags );
L_INT EXT_FUNCTION L_DocDlgFind(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);
L_INT EXT_FUNCTION L_DocDlgSetFindDlgIcon(HICON hNewIcon);
HICON EXT_FUNCTION L_DocDlgGetFindDlgIcon();
L_INT EXT_FUNCTION L_DocDlgFont(pDOCHANDLE  pDoc,L_UINT32* puStatusMode);





L_INT EXT_FUNCTION L_DocDlgGetStringLen( L_UINT32 uString, L_UINT L_FAR *puLen );
L_INT EXT_FUNCTION L_DocDlgGetString( L_UINT32 uString, L_CHAR L_FAR *pszString );
L_INT EXT_FUNCTION L_DocDlgSetString( L_UINT32 uString, const L_CHAR L_FAR *pszString );
HFONT EXT_FUNCTION L_DocDlgSetFont( HFONT hFont );


#undef _HEADER_ENTRY_
#include "ltpck.h"

#endif   // _LDKRN_H_
