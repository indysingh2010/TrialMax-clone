/*************************************************************
   Ltocrpdf_.h - internal file module library
   Copyright (c) 1991-2008 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTOCRDOCWRT__H )
#define LTOCRDOCWRT__H 

#if !defined(L_LTDOCWRT_API)
   #define L_LTDOCWRT_API
#endif // #if !defined(L_LTPDF_API)

#define L_HEADER_ENTRY
#include "Ltpck.h"

typedef HANDLE DOCUMENTWRITER_HANDLE;


typedef struct _DOCWRTPAGE
{
   L_UINT          uStructSize;
   HENHMETAFILE    hEmf;
   pBITMAPHANDLE   pOverlayBitmap; //for Pdf file only
   L_DOUBLE        *pdwTextScale; //For Pdf Image over text only
}DOCWRTPAGE, *pDOCWRTPAGE;

typedef  enum _DOCWRTFORMAT
{
   DOCUMENTFORMAT_USER = -1,
   DOCUMENTFORMAT_LTD = 0,
   DOCUMENTFORMAT_PDF = 1,
   DOCUMENTFORMAT_DOC,
   DOCUMENTFORMAT_RTF,
   DOCUMENTFORMAT_HTM,
   DOCUMENTFORMAT_TXT,
   DOCUMENTFORMAT_EMF,
   DOCUMENTFORMAT_XPS,
   DOCUMENTFORMAT_DOCX,
 }DOCWRTFORMAT, *pDOCWRTFORMAT;

typedef enum _DOCWRTPDFPROFILE
{
   DOCWRTPDFPROFILE_PDF = 0, //default, save version 1.4 Pdf document
   DOCWRTPDFPROFILE_PDFA,    //save PdfA document
#if defined(LEADTOOLS_V16_OR_LATER)
   DOCWRTPDFPROFILE_PDF12,   //save version 1.2 Pdf document
   DOCWRTPDFPROFILE_PDF13,   //save version 1.3 Pdf document 
   DOCWRTPDFPROFILE_PDF15    //save version 1.5 Pdf document
#endif //#if defined(LEADTOOLS_V16_OR_LATER)
}DOCWRTPDFPROFILE, *pDOCWRTPDFPROFILE;

typedef enum _DOCWRTPDFENCRYPTIONMODE
{
   DOCWRTPDFENCRYPTIONMODE_RC40BIT = 0,
   DOCWRTPDFENCRYPTIONMODE_RC128BIT,
}DOCWRTPDFENCRYPTIONMODE, *pDOCWRTPDFENCRYPTIONMODE;

typedef enum _DOCWRTTXTTYPE
{
   DOCWRTTXTTYPE_ANSI = 0,
   DOCWRTTXTTYPE_UNICODE,
   DOCWRTTXTTYPE_UNICODE_BIGENDIAN

}DOCWRTTXTTYPE, *pDOCWRTTXTTYPE;

typedef enum _DOCWRTFONTEMBED
{
   DOCWRTFONTEMBED_NOEMBED = 0,     //Do not embed fonts
   DOCWRTFONTEMBED_AUTO,            // Automatic embedding
   DOCWRTFONTEMBED_FORCE,           //force embedding for no-embedding license fonts
   DOCWRTFONTEMBED_ALL              // all fonts in the document should be embedded
}DOCWRTFONTEMBED, *pDOCWRTFONTEMBED;

typedef enum _DOCWRTHTMTYPE
{
   DOCWRTHTMTYPE_IECOMPATIBLE = 0,     //IE5 and above
   DOCWRTHTMTYPE_NETSCAPEOMPATIBLE,    //Netscape 4.6, 6.0+
   DOCWRTHTMTYPE_IENETSCAPEOMPATIBLE   //IE5 and above, Netscape 4.6, 6.0+

}DOCWRTHTMTYPE, *pDOCWRTHTMTYPE;

typedef enum _DOCWRTPAGERESTRICTION
{
   DOCWRTPAGERESTRICTION_DEFAULT = 0,
   DOCWRTPAGERESTRICTION_RELAXED,
}DOCWRTPAGERESTRICTION, *pDOCWRTPAGERESTRICTION;

typedef struct _DOCWRTOPTIONS
{
   L_UINT                uStructSize;            // Structure size
#if defined(LEADTOOLS_V16_OR_LATER)
   DOCWRTPAGERESTRICTION PageRestriction;        // Page restriction      
   L_DOUBLE              dEmptyPageWidth;        // Empty page width in inches
   L_DOUBLE              dEmptyPageHeight;       // Empty page height in inches
   L_INT                 nEmptyPageResolution;   // Empty page resolution
   L_BOOL                bMaintainAspectRatio;   // Maintain the aspect ratio of original emf file.
   L_INT                 nDocumentResolution;    // Document resolution, 0 means keep the original emf resolution
#endif //#if defined(LEADTOOLS_V16_OR_LATER)
}DOCWRTOPTIONS, *pDOCWRTOPTIONS;

typedef struct _DOCWRTLTDOPTIONS
{
   DOCWRTOPTIONS     Options;             // Options
   L_VOID*           pReserved;
}DOCWRTLTDOPTIONS, *pDOCWRTLTDOPTIONS;

typedef struct _DOCWRTEMFOPTIONS
{
   DOCWRTOPTIONS     Options;             // Options
   L_VOID*           pReserved;
}DOCWRTEMFOPTIONS, *pDOCWRTEMFOPTIONS;


typedef struct _DOCWRTXPSOPTIONS
{
   DOCWRTOPTIONS     Options;             // Options
   L_VOID*           pReserved;
   L_UINT32          uFlags;              // Flags - reserved
}DOCWRTXPSOPTIONS, *pDOCWRTXPSOPTIONS;


typedef struct _DOCWRTDOCXOPTIONS
{
   DOCWRTOPTIONS     Options;             // Options
   L_BOOL            bFramed;             // TRUE means to use framed text
   L_UINT32          uFlags;              // Flags - reserved
}DOCWRTDOCXOPTIONS, *pDOCWRTDOCXOPTIONS;

typedef struct _DOCWRTTXTOPTIONS
{
   DOCWRTOPTIONS     Options;             // Options
   DOCWRTTXTTYPE     Type;                // Text file type
   L_BOOL            bAddPageNumber;      // TRUE means to add page number to saved file
   L_BOOL            bAddPageBreak;       // TRUE means to add page break at end of each page
   L_BOOL            bFormatted;          // If TRUE the saved file will maintain the same format for original document
   L_UINT32          uFlags;              // Flags
}DOCWRTTXTOPTIONS, *pDOCWRTTXTOPTIONS;


typedef struct _DOCWRTPDFOPTIONS
{
   DOCWRTOPTIONS           Options;            // Options
   DOCWRTPDFPROFILE        PdfProfile;         // Pdf profile mode
   DOCWRTFONTEMBED         FontEmbed;          // Font Embedding mode
   L_BOOL                  bImageOverText;     // Image over text flag
   L_UINT32                uFlags;             // Flags
#if defined(LEADTOOLS_V16_OR_LATER)
   L_BOOL                  bLinearized;        // Linearization flag, for fast web view

   L_WCHAR                *pwszTitle;          // Pdf document title
   L_WCHAR                *pwszSubject;        // Pdf document subject
   L_WCHAR                *pwszKeywords;       // Pdf document keywords
   L_WCHAR                *pwszAuthor;         // Pdf document author

   L_BOOL                  bProtected;         // TRUE means the document will be saved with password protection
   L_CHAR                 *pszUserPassword;    // User password
   L_CHAR                 *pszOwnerPassword;   // Owner password
   DOCWRTPDFENCRYPTIONMODE EncryptionMode;
   L_BOOL                  bPrintEnabled;
   L_BOOL                  bHighQualityPrintEnabled;
   L_BOOL                  bCopyEnabled;
   L_BOOL                  bEditEnabled;
   L_BOOL                  bAnnotationsEnabled;
   L_BOOL                  bAssemblyEnabled;
#endif //#if defined(LEADTOOLS_V16_OR_LATER)
} DOCWRTPDFOPTIONS, *pDOCWRTPDFOPTIONS;

typedef struct _DOCWRTDOCOPTIONS
{
   DOCWRTOPTIONS      Options;            // Options
   L_BOOL             bFramed;            //TRUE means to use framed text
   L_UINT32           uFlags;             // Flags

}DOCWRTDOCOPTIONS, *pDOCWRTDOCOPTIONS;

typedef struct _DOCWRTRTFOPTIONS
{
   DOCWRTOPTIONS      Options;            // Options
   L_BOOL             bFramed;            //TRUE means to use framed text
   L_UINT32           uFlags;             // Flags

}DOCWRTRTFOPTIONS,*pDOCWRTRTFOPTIONS;

typedef struct _DOCWRTHTMOPTIONS
{
   DOCWRTOPTIONS      Options;               // Options
   DOCWRTHTMTYPE      Type;                  // Html document type
   DOCWRTFONTEMBED    FontEmbed;             // Font Embedding mode
   L_BOOL             bUseBackgroundColor;   // TRUE means to use the rgbBackground as background color at saved Html document
   COLORREF           rgbBackground;         // Background color
   L_UINT32           uFlags;                // Flags 
}DOCWRTHTMOPTIONS,*pDOCWRTHTMOPTIONS;


L_LTDOCWRT_API L_INT EXT_FUNCTION L_DocWriterInitA( DOCUMENTWRITER_HANDLE *phDocument, L_CHAR *pszFileName, DOCWRTFORMAT Format, L_VOID* pDocOptions, STATUSCALLBACK pfnStatusCallback, L_VOID *pUserData );
L_LTDOCWRT_API L_INT EXT_FUNCTION L_DocWriterAddPageA( DOCUMENTWRITER_HANDLE hDocument, pDOCWRTPAGE pPage );
L_LTDOCWRT_API L_INT EXT_FUNCTION L_DocWriterFinishA( DOCUMENTWRITER_HANDLE hDocument );
L_LTDOCWRT_API L_INT EXT_FUNCTION L_DocWriterConvertA( L_CHAR* pszLtdFileName, L_CHAR *pszFileName, DOCWRTFORMAT Format, L_VOID* pDocOptions, STATUSCALLBACK pfnStatusCallback, L_VOID *pUserData );
#if defined(FOR_UNICODE)
L_LTDOCWRT_API L_INT EXT_FUNCTION L_DocWriterInit( DOCUMENTWRITER_HANDLE *phDocument, L_WCHAR *pszFileName, DOCWRTFORMAT Format, L_VOID* pDocOptions, STATUSCALLBACK pfnStatusCallback, L_VOID *pUserData );
L_LTDOCWRT_API L_INT EXT_FUNCTION L_DocWriterAddPage( DOCUMENTWRITER_HANDLE hDocument, pDOCWRTPAGE pPage );
L_LTDOCWRT_API L_INT EXT_FUNCTION L_DocWriterFinish( DOCUMENTWRITER_HANDLE hDocument );
L_LTDOCWRT_API L_INT EXT_FUNCTION L_DocWriterConvert(L_WCHAR* pszLtdFileName, L_WCHAR *pszFileName, DOCWRTFORMAT Format, L_VOID* pDocOptions, STATUSCALLBACK pfnStatusCallback, L_VOID *pUserData );
#else
#define L_DocWriterInit       L_DocWriterInitA
#define L_DocWriterAddPage    L_DocWriterAddPageA
#define L_DocWriterFinish     L_DocWriterFinishA
#define L_DocWriterConvert    L_DocWriterConvertA

#endif // #if defined(FOR_UNICODE)

#if defined(LEADTOOLS_V16_OR_LATER)
L_INT EXT_FUNCTION L_DocWriterUpdateMetaFileResolution(L_HENHMETAFILE hEmfSrc, L_INT xResolution, L_INT yResolution, L_HENHMETAFILE* phEmfDest);
#endif //#if defined(LEADTOOLS_V16_OR_LATER)

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTOCRDOCWRT__H)
