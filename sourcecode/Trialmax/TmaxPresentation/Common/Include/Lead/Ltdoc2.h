/*************************************************************
   Ltdoc2.h - document recognition module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDOC2_H)
#define LTDOC2_H

#if !defined(L_LTDOC2_API)
   #define L_LTDOC2_API
#endif // #if !defined(L_LTDOC2_API)

#include "Lttyp.h"
#include "Lterr.h"

#include "Ltdocwrt.h"

#define L_HEADER_ENTRY
#include "Ltpck.h"

/****************************************************************
   Enums/defines/macros
****************************************************************/

typedef HANDLE L_HDOC2;

/* uFlags values of ZONEDATA2 structure */
/* input flags:*/
#define DOC2_ZONE_CHK_LANGDICT_PROHIBIT     0x00000001
#define DOC2_ZONE_CHK_USERDICT_PROHIBIT     0x00000002
#define DOC2_ZONE_CHK_CHECKCBF_PROHIBIT     0x00000004
#define DOC2_ZONE_CHK_VERTDICT_PROHIBIT     0x00000008
#define DOC2_ZONE_CHK_IGNORE_WHITESPACE     0x00000010
#define DOC2_ZONE_CHK_IGNORE_CASE           0x00000020
#define DOC2_ZONE_CHK_PASS_LINES            0x00000040
#define DOC2_ZONE_CHK_CORRECTION_DISABLED   0x00000080
#define DOC2_ZONE_CHK_INCLUDE_PUNCTUATION   0x00000100
#define DOC2_ZONE_CHK_CORRECT_PROPERNAMES   0x00000200
/* output flags:*/
#define DOC2_ZONE_CHK_LANGDICT_USED         0x00010000
#define DOC2_ZONE_CHK_USERDICT_USED         0x00020000
#define DOC2_ZONE_CHK_CHECKCBF_USED         0x00040000

// Character position flags
#define DOC2_CHAR_ENDOFLINE                 0x001
#define DOC2_CHAR_ENDOFPARA                 0x002
#define DOC2_CHAR_ENDOFWORD                 0x004
#define DOC2_CHAR_ENDOFZONE                 0x008
#define DOC2_CHAR_ENDOFPAGE                 0x010
#define DOC2_CHAR_ENDOFCELL                 0x020
#define DOC2_CHAR_ENDOFROW                  0x040
#define DOC2_CHAR_INTABLE                   0x080

// Font description flags
#define DOC2_FONT_ITALIC                    0x0001
#define DOC2_FONT_BOLD                      0x0002
#define DOC2_FONT_UNDERLINE                 0x0004
#define DOC2_FONT_SUBSCRIPT                 0x0008
#define DOC2_FONT_SUPERSCRIPT               0x0010
#define DOC2_FONT_SANSSERIF                 0x0020
#define DOC2_FONT_SERIF                     0x0040
#define DOC2_FONT_PROPORTIONAL              0x0080
#define DOC2_FONT_SMALLCAPS                 0x0100
#define DOC2_FONT_DROPCAP                   0x0200
#define DOC2_FONT_POPCAP                    0x0400
#define DOC2_FONT_INVERTED                  0x1000

// User dictionary mask
#define DOC2_USER_DICT_LITERAL              0x0000
#define DOC2_USER_DICT_REGULAR_EXPRESSION   0x0001

#define DOC2_MAX_UD_ITEM_LENGTH             64
#define DOC2_MAX_SECTION_NAME_LENGTH        17
#define DOC2_MAX_SEPARATOR_SIZE             256
#define DOC2_MAX_WORD_SIZE                  1000

#define DOC2_USE_FORMATTYPE_VALUE           0x0000
#define DOC2_USE_COMMON_PDF_SETTINGS        0x0002
#define DOC2_USE_COMMON_RTF_DOC_SETTINGS    0x0004
#define DOC2_USE_COMMON_TEXT_SETTINGS       0x0008

#define DOC2_SAVE_PAGE_RESULTS              0x001

typedef enum
{
   DOC2_ZONE_CHAR_FILTER_DEFAULT            = 0x0000,
   DOC2_ZONE_CHAR_FILTER_DIGIT              = 0x0001,
   DOC2_ZONE_CHAR_FILTER_UPPERCASE          = 0x0002,
   DOC2_ZONE_CHAR_FILTER_LOWERCASE          = 0x0004,
   DOC2_ZONE_CHAR_FILTER_PUNCTUATION        = 0x0008,
   DOC2_ZONE_CHAR_FILTER_MISCELLANEOUS      = 0x0010,
   DOC2_ZONE_CHAR_FILTER_PLUS               = 0x0020,
   DOC2_ZONE_CHAR_FILTER_ALL                = 0x0040,
   DOC2_ZONE_CHAR_FILTER_ALPHA              = 0x0080,
   DOC2_ZONE_CHAR_FILTER_NUMBERS            = 0x0100,
   DOC2_ZONE_CHAR_FILTER_OCRA               = 0x0200,
   DOC2_ZONE_CHAR_FILTER_USER_DICTIONARY    = 0x0400,
} DOC2_CHAR_FILTER;

typedef enum 
{
   DOC2_FILL_DEFAULT = 0,
   DOC2_FILL_OMNIFONT,
   DOC2_FILL_DRAFTDOT9,
   DOC2_FILL_OMR,
   DOC2_FILL_HANDPRINT,
   DOC2_FILL_DRAFTDOT24,
   DOC2_FILL_OCRA,
   DOC2_FILL_OCRB,
   DOC2_FILL_MICR,
   DOC2_FILL_DOTDIGIT,
   DOC2_FILL_DASHDIGIT,
   DOC2_FILL_NO_OCR,
   DOC2_FILL_ASIAN,
} DOC2_FILLMETHOD;

typedef enum
{
   DOC2_RECOGMODULE_AUTO = 0,
   DOC2_RECOGMODULE_MTEXT_OMNIFONT,
   DOC2_RECOGMODULE_MULTI_LINGUAL_OMNIFONT,
   DOC2_RECOGMODULE_DOT_MATRIX,
   DOC2_RECOGMODULE_OMR,
   DOC2_RECOGMODULE_HAND_PRINTED_NUMERAL,
   DOC2_RECOGMODULE_RER_PRINTED,
   DOC2_RECOGMODULE_MATRIX,
   DOC2_RECOGMODULE_OMNIFONT_PLUS2W,
   DOC2_RECOGMODULE_OMNIFONT_FRX,
   DOC2_RECOGMODULE_OMNIFONT_PLUS3W,
   DOC2_RECOGMODULE_ASIAN,
} DOC2_RECOGMODULE;

typedef enum
{
   // Basic Zone type
   DOC2_ZONE_FLOWTEXT = 0,
   DOC2_ZONE_TABLE,
   DOC2_ZONE_GRAPHIC,
   DOC2_ZONE_AUTO,
   DOC2_ZONE_VERTTEXT,
   DOC2_ZONE_LEFTTEXT,
   DOC2_ZONE_RIGHTTEXT,
} DOC2_ZONETYPE;

typedef enum
{
   DOC2_PARSE_AUTO = 0,
   DOC2_PARSE_LEGACY,
   DOC2_PARSE_STANDARD,
   DOC2_PARSE_FAST
} DOC2_PAGEPARSER;

typedef enum
{
   DOC2_LANG_ID_AUTO = -2,
   DOC2_LANG_ID_NO = -1,
   DOC2_LANG_ID_ENGLISH = 0,
   DOC2_LANG_ID_GERMAN,
   DOC2_LANG_ID_FRENCH,
   DOC2_LANG_ID_DUTCH,
   DOC2_LANG_ID_NORWEGIAN,
   DOC2_LANG_ID_SWEDISH,
   DOC2_LANG_ID_FINNISH,
   DOC2_LANG_ID_DANISH,
   DOC2_LANG_ID_ICELANDIC,
   DOC2_LANG_ID_PORTUGUESE,
   DOC2_LANG_ID_SPANISH,
   DOC2_LANG_ID_CATALAN,
   DOC2_LANG_ID_GALICIAN,
   DOC2_LANG_ID_ITALIAN,
   DOC2_LANG_ID_MALTESE,
   DOC2_LANG_ID_GREEK,
   DOC2_LANG_ID_POLISH,
   DOC2_LANG_ID_CZECH,
   DOC2_LANG_ID_SLOVAK,
   DOC2_LANG_ID_HUNGARIAN,
   DOC2_LANG_ID_SLOVENIAN,
   DOC2_LANG_ID_CROATIAN,
   DOC2_LANG_ID_ROMANIAN,
   DOC2_LANG_ID_ALBANIAN,
   DOC2_LANG_ID_TURKISH,
   DOC2_LANG_ID_ESTONIAN,
   DOC2_LANG_ID_LATVIAN,
   DOC2_LANG_ID_LITHUANIAN,
   DOC2_LANG_ID_ESPERANTO,
   DOC2_LANG_ID_SERBIAN_LATIN,
   DOC2_LANG_ID_SERBIAN,
   DOC2_LANG_ID_MACEDONIAN,
   DOC2_LANG_ID_MOLDAVIAN,
   DOC2_LANG_ID_BULGARIAN,
   DOC2_LANG_ID_BYELORUSSIAN,
   DOC2_LANG_ID_UKRAINIAN,
   DOC2_LANG_ID_RUSSIAN,
   DOC2_LANG_ID_CHECHEN,
   DOC2_LANG_ID_KABARDIAN,
   DOC2_LANG_ID_AFRIKAANS,
   DOC2_LANG_ID_AYMARA,
   DOC2_LANG_ID_BASQUE,
   DOC2_LANG_ID_BEMBA,
   DOC2_LANG_ID_BLACKFOOT,
   DOC2_LANG_ID_BRETON,
   DOC2_LANG_ID_BRAZILIAN,
   DOC2_LANG_ID_BUGOTU,
   DOC2_LANG_ID_CHAMORRO,
   DOC2_LANG_ID_CHUANA_TSWANA,
   DOC2_LANG_ID_CORSICAN,
   DOC2_LANG_ID_CROW,
   DOC2_LANG_ID_ESKIMO,
   DOC2_LANG_ID_FAROESE,
   DOC2_LANG_ID_FIJIAN,
   DOC2_LANG_ID_FRISIAN,
   DOC2_LANG_ID_FRIULIAN,
   DOC2_LANG_ID_GAELIC_IRISH,
   DOC2_LANG_ID_GAELIC_SCOTTISH,
   DOC2_LANG_ID_GANDA_LUGANDA,
   DOC2_LANG_ID_GUARANI,
   DOC2_LANG_ID_HANI,
   DOC2_LANG_ID_HAWAIIAN,
   DOC2_LANG_ID_IDO,
   DOC2_LANG_ID_INDONESIAN,
   DOC2_LANG_ID_INTERLINGUA,
   DOC2_LANG_ID_KASHUBIAN,
   DOC2_LANG_ID_KAWA,
   DOC2_LANG_ID_KIKUYU,
   DOC2_LANG_ID_KONGO,
   DOC2_LANG_ID_KPELLE,
   DOC2_LANG_ID_KURDISH,
   DOC2_LANG_ID_LATIN,
   DOC2_LANG_ID_LUBA,
   DOC2_LANG_ID_LUXEMBOURGIAN,
   DOC2_LANG_ID_MALAGASY,
   DOC2_LANG_ID_MALAY,
   DOC2_LANG_ID_MALINKE,
   DOC2_LANG_ID_MAORI,
   DOC2_LANG_ID_MAYAN,
   DOC2_LANG_ID_MIAO,
   DOC2_LANG_ID_MINANKABAW,
   DOC2_LANG_ID_MOHAWK,
   DOC2_LANG_ID_NAHUATL,
   DOC2_LANG_ID_NYANJA,
   DOC2_LANG_ID_OCCIDENTAL,
   DOC2_LANG_ID_OJIBWAY,
   DOC2_LANG_ID_PAPIAMENTO,
   DOC2_LANG_ID_PIDGIN_ENGLISH,
   DOC2_LANG_ID_PROVENCAL,
   DOC2_LANG_ID_QUECHUA,
   DOC2_LANG_ID_RHAETIC,
   DOC2_LANG_ID_ROMANY,
   DOC2_LANG_ID_RUANDA,
   DOC2_LANG_ID_RUNDI,
   DOC2_LANG_ID_SAMOAN,
   DOC2_LANG_ID_SARDINIAN,
   DOC2_LANG_ID_SHONA,
   DOC2_LANG_ID_SIOUX,
   DOC2_LANG_ID_SAMI,
   DOC2_LANG_ID_LULE_SAMI,
   DOC2_LANG_ID_NORTHERN_SAMI,
   DOC2_LANG_ID_SOUTHERN_SAMI,
   DOC2_LANG_ID_SOMALI,
   DOC2_LANG_ID_SOTHO_SUTO_SESUTO,
   DOC2_LANG_ID_SUNDANESE,
   DOC2_LANG_ID_SWAHILI,
   DOC2_LANG_ID_SWAZI,
   DOC2_LANG_ID_TAGALOG,
   DOC2_LANG_ID_TAHITIAN,
   DOC2_LANG_ID_TINPO,
   DOC2_LANG_ID_TONGAN,
   DOC2_LANG_ID_TUN,
   DOC2_LANG_ID_VISAYAN,
   DOC2_LANG_ID_WELSH,
   DOC2_LANG_ID_WEND_SORBIAN,
   DOC2_LANG_ID_WOLOF,
   DOC2_LANG_ID_XHOSA,
   DOC2_LANG_ID_ZAPOTEC,
   DOC2_LANG_ID_ZULU,
   DOC2_LANG_ID_JPN,
   DOC2_LANG_ID_CHS,
   DOC2_LANG_ID_CHT,
   DOC2_LANG_ID_KRN,
} DOC2_LANGIDS;

typedef enum
{
   DOC2_RECGMD_ACCURATE,
   DOC2_RECGMD_BALANCED,
   DOC2_RECGMD_FAST,
} DOC2_RECOGMODULE_TRADEOFF;

typedef enum
{
   DOC2_HAND_STYLE_EURO,
   DOC2_HAND_STYLE_US
} DOC2_HANDRECOG_STYLE;

typedef enum
{
   DOC2_OMR_SENSE_HIGHEST,
   DOC2_OMR_SENSE_HIGH,
   DOC2_OMR_SENSE_LOW,
   DOC2_OMR_SENSE_LOWEST,
} DOC2_OMRSENSE;

typedef enum
{
   DOC2_OMR_AUTO_FRAME,
   DOC2_OMR_WITHOUT_FRAME,
   DOC2_OMR_WITH_FRAME,
} DOC2_OMRFRAME;

typedef enum
{
   DOC2_FORMAT_LEVEL_AUTO = 0,
   DOC2_FORMAT_LEVEL_NONE,
   DOC2_FORMAT_LEVEL_RFP,
   DOC2_FORMAT_LEVEL_TRUEPAGE,
   DOC2_FORMAT_LEVEL_FLOWINGPAGE,
   DOC2_FORMAT_LEVEL_SPREADSHEET,
} DOC2_FORMATLEVEL;

typedef enum
{
   DOC2_PROC_LOADING_IMG = 0,
   DOC2_PROC_SAVING_IMG,
   DOC2_PROC_FIND_ZONES,
   DOC2_PROC_RECOGNIZE_MOR,
   DOC2_PROC_RECOGNITION,
   DOC2_PROC_SPELLING,
   DOC2_PROC_WRITE_OUTPUT_DOC,
   DOC2_PROC_WRITE_OUTPUT_IMG,
   DOC2_PROC_RECOGNITION3,
   DOC2_PROC_PROCESSING_IMG,
   DOC2_PROC_FORMATTING,
} DOC2_OCRPROCID;

typedef enum
{
   DOC2_VERIFY_IMPOSSIBLE = 0,
   DOC2_VERIFY_UNLIKELY,
   DOC2_VERIFY_UNRESOLVED,
   DOC2_VERIFY_POSSIBLE,
   DOC2_VERIFY_ACCEPT,
} DOC2_VERIFYCODE;

typedef enum
{
   DOC2_TEXT,
   DOC2_UTEXT,
   DOC2_FORMATTED_TEXT,
   DOC2_UFORMATTED_TEXT,
   DOC2_TEXT_LINEBREAKS,
   DOC2_UTEXT_LINEBREAKS,
   DOC2_TEXT_CSV,
   DOC2_TEXT_UCSV,
   DOC2_PDF,
   DOC2_PDF_IMAGE_SUBSTITUTES,
   DOC2_PDF_IMAGE_ON_TEXT,
   DOC2_PDF_EDITED,
   DOC2_XML,
   DOC2_HTML_3_2,
   DOC2_HTML_4_0,
   DOC2_RTF_6,
   DOC2_RTF_97,
   DOC2_RTF_2000,
   DOC2_RTF_WORD_2000,
   DOC2_WORD_2000,
   DOC2_WORD_97,
   DOC2_EXCEL_97,
   DOC2_EXCEL_2000,
   DOC2_PPT_97,
   DOC2_PUB_98,
   DOC2_MICROSOFT_READER,
   DOC2_WORDML,
   DOC2_WORDPERFECT_8,
   DOC2_WORDPERFECT_10,
   DOC2_WORDPAD,
   DOC2_INFOPATH,
   DOC2_EBOOK,
   DOC2_PDFA_IMAGE_ON_TEXT,
   DOC2_PDFA_TEXT_ONLY,
} DOC2_FORMATTYPE;

typedef enum
{
   DOC2_CODEPAGE_NONE = 0,
   DOC2_CODEPAGE_UNICODE,
   DOC2_CODEPAGE_WORDPERFECT,
   DOC2_CODEPAGE_437,
   DOC2_CODEPAGE_708,
   DOC2_CODEPAGE_709,
   DOC2_CODEPAGE_710,
   DOC2_CODEPAGE_711,
   DOC2_CODEPAGE_720,
   DOC2_CODEPAGE_819,
   DOC2_CODEPAGE_850,
   DOC2_CODEPAGE_852,
   DOC2_CODEPAGE_860,
   DOC2_CODEPAGE_862,
   DOC2_CODEPAGE_863,
   DOC2_CODEPAGE_864,
   DOC2_CODEPAGE_865,
   DOC2_CODEPAGE_866,
   DOC2_CODEPAGE_932,
   DOC2_CODEPAGE_1250,
   DOC2_CODEPAGE_1251,
   DOC2_CODEPAGE_1252,
   DOC2_CODEPAGE_1253,
   DOC2_CODEPAGE_1254,
   DOC2_CODEPAGE_1257,
   DOC2_CODEPAGE_DEF_CODEPAGE,
   DOC2_CODEPAGE_ROMAN_8,
   DOC2_CODEPAGE_GREEK_ELOT,
   DOC2_CODEPAGE_GREEK_MEMOTEK,
   DOC2_CODEPAGE_MAC,
   DOC2_CODEPAGE_MAC_INSO_LATIN_2,
   DOC2_CODEPAGE_MAC_CENTRAL_EU,
   DOC2_CODEPAGE_MAC_PRIMUS_CE,
   DOC2_CODEPAGE_WIN_ESPERANT,
   DOC2_CODEPAGE_HUN_CWI,
   DOC2_CODEPAGE_HUN_VENTURA,
   DOC2_CODEPAGE_IVKAM_C_S,
   DOC2_CODEPAGE_MAZOWIA_POLISH,
   DOC2_CODEPAGE_SLOVEN_CROAT,
   DOC2_CODEPAGE_TURKISH,
   DOC2_CODEPAGE_ICELANDIC,
   DOC2_CODEPAGE_MALTESE,
   DOC2_CODEPAGE_WINDOWS_OCR_MICR,
   DOC2_CODEPAGE_AUTO,
} DOC2_CODEPAGESETTING;

typedef enum
{
   DOC2_HF_NONE = 0,
   DOC2_HF_AUTO_FORMAT,
   DOC2_HF_BOXES,
   DOC2_HF_TABBEDBOX,
   DOC2_HF_TABBED,
   DOC2_HF_CONVERT,
} DOC2_HEADERFOOTERSETTING;

typedef enum
{
   DOC2_DPI_NONE = 0,
   DOC2_DPI_ORIGINAL,
   DOC2_DPI_72,
   DOC2_DPI_100,
   DOC2_DPI_150,
   DOC2_DPI_200,
   DOC2_DPI_300,
} DOC2_DPISETTING;

typedef enum
{
   DOC2_TABLE_NONE = 0,
   DOC2_TABLE_RETAIN,
   DOC2_TABLE_TO_TABBED,
   DOC2_TABLE_TO_SPACES,
} DOC2_TABLESETTING;

typedef enum
{
   DOC2_INDEX_NONE = 0,
   DOC2_INDEX_SIMPLEHTML,
   DOC2_INDEX_INFRAME,
} DOC2_HTMLINDEXPAGE;

typedef enum
{
   DOC2_PDFCOLORQUALITY_MIN = 0,
   DOC2_PDFCOLORQUALITY_GOOD,
   DOC2_PDFCOLORQUALITY_LOSSLESS,
} DOC2_PDFCOLORQUALITY;

typedef enum
{
   DOC2_OPTIMIZESIZE = 0,
   DOC2_OPTIMIZEQUALITY,
   DOC2_PDF15,
   DOC2_PDF14,
   DOC2_PDF13,
   DOC2_PDF12,
   DOC2_PDF11,
   DOC2_PDF10,
   DOC2_PDFA,
} DOC2_PDFCOMPATIBILITY;

typedef enum
{
   DOC2_PDF_SECURITY_NONE = 0,
   DOC2_PDF_SECURITY_40BITS,
   DOC2_PDF_SECURITY_128BITS,
} DOC2_PDFSECURITY;

typedef enum
{
   DOC2_PDF_MRC_NO = 0,
   DOC2_PDF_MRC_MIN,
   DOC2_PDF_MRC_GOOD,
   DOC2_PDF_MRC_LOSSLESS,
} DOC2_PDFMRC;

typedef enum
{
   DOC2_PDF_BPP_ORIGINAL = 0,
   DOC2_PDF_BPP_BLACKANDWHITE,
   DOC2_PDF_BPP_GRAYSCALE,
   DOC2_PDF_BPP_24BPP,
} DOC2_PDFBPP;

typedef enum
{
   DOC2_PDF_SIGTYPENONE = 0,
   DOC2_PDF_SIGTYPEPPKLITE,
   DOC2_PDF_SIGTYPEVERISIGN,
} DOC2_PDFSIGNATURE;

typedef enum
{
   DOC2_PB_NEVER = 0,
   DOC2_PB_ALWAYS,
   DOC2_PB_AUTO,
} DOC2_PAGEBREAKSETTING;

/****************************************************************
   Callback typedefs
****************************************************************/
typedef L_INT (pEXT_CALLBACK VERIFICATIONCALLBACKA2)(
                              L_INT nZoneIndex,
                              L_CHAR * pszWord,
                              DOC2_VERIFYCODE * pVerify,
                              L_VOID * pUserData);
#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK VERIFICATIONCALLBACK2)(
                              L_INT nZoneIndex,
                              L_TCHAR * pszWord,
                              DOC2_VERIFYCODE * pVerify,
                              L_VOID * pUserData);
#else
typedef VERIFICATIONCALLBACKA2 VERIFICATIONCALLBACK2;
#endif // #if defined(FOR_UNICODE)

/****************************************************************
   Classes/structures
****************************************************************/

typedef struct _tagProgressData2
{
   L_UINT      uStructSize;
   DOC2_OCRPROCID   Id;
   L_INT       nPercent;
} PROGRESSDATA2, * pPROGRESSDATA2;

typedef struct _tagZoneDataA2
{
   L_UINT               uStructSize;
   RECT                 rcArea;
   DOC2_FILLMETHOD      FillMethod;
   DOC2_RECOGMODULE     RecogModule;
   DOC2_CHAR_FILTER     CharFilter;
   DOC2_ZONETYPE        Type;
   L_UINT               uFlags;
   L_CHAR               szSection[DOC2_MAX_SECTION_NAME_LENGTH];
   VERIFICATIONCALLBACKA2 pfnCallback;
   L_VOID             * pUserData;
} ZONEDATAA2, * pZONEDATAA2;
#if defined(FOR_UNICODE)
typedef struct _tagZoneData2
{
   L_UINT               uStructSize;
   RECT                 rcArea;
   DOC2_FILLMETHOD      FillMethod;
   DOC2_RECOGMODULE     RecogModule;
   DOC2_CHAR_FILTER     CharFilter;
   DOC2_ZONETYPE        Type;
   L_UINT               uFlags;
   L_CHAR               szSection[DOC2_MAX_SECTION_NAME_LENGTH];
   VERIFICATIONCALLBACK2 pfnCallback;
   L_VOID             * pUserData;
} ZONEDATA2, * pZONEDATA2;
#else
typedef ZONEDATAA2 ZONEDATA2;
typedef pZONEDATAA2 pZONEDATA2;
#endif // #if defined(FOR_UNICODE)

typedef struct _tagAutoZoneOpts2
{
   L_UINT      uStructSize;
   DOC2_PAGEPARSER  Parser;
   L_BOOL      bEnableForceSingleColumn;
   L_BOOL      bDetectNonGridedTables;
} AUTOZONEOPTS2, * pAUTOZONEOPTS2;

typedef struct _tagCharOptions2
{
   L_UINT         uStructSize;
   L_WCHAR      * pszCharPlus;
   L_UINT         uCharPlusSize;
   L_WCHAR      * pszCharFilterPlus;
   L_UINT         uCharFilterPlusSize;
   DOC2_CHAR_FILTER    CharFilter;
} CHAROPTIONS2, * pCHAROPTIONS2;

typedef struct _tagMOROptions2
{
   L_UINT   uStructSize;
   L_BOOL   bEnableFax;
} MOROPTIONS2, * pMOROPTIONS2;

typedef struct _tagHandPrintOptions2
{
   L_UINT            uStructSize;
   DOC2_HANDRECOG_STYLE   style;
   L_BOOL            bSpace;
   L_INT             nCharHeight;
   L_INT             nCharWidth;
   L_INT             nCharSpace;
} HANDPRINTOPTIONS2, * pHANDPRINTOPTIONS2;

typedef struct _tagOMROptions2
{
   L_UINT         uStructSize;
   L_BOOL         bFill; /* Deprecated, do not use */
   DOC2_OMRFRAME  Frame;
   DOC2_OMRSENSE  Sense;
   L_WCHAR        FilledRecognitionChar;
   L_WCHAR        UnFilledRecognitionChar;
} OMROPTIONS2, * pOMROPTIONS2;

typedef struct _tagUserDictionaryA2
{
   L_UINT          uStructSize;
   L_CHAR        * pszFileName;
   L_CHAR        * pszDefSection;
} USERDICTIONARYA2, * pUSERDICTIONARYA2;

#if defined(FOR_UNICODE)
typedef struct _tagUserDictionary2
{
   L_UINT          uStructSize;
   L_TCHAR       * pszFileName;
   L_CHAR        * pszDefSection;
} USERDICTIONARY2, * pUSERDICTIONARY2;
#else
typedef USERDICTIONARYA2 USERDICTIONARY2;
typedef pUSERDICTIONARYA2 pUSERDICTIONARY2;
#endif // #if defined(FOR_UNICODE)

typedef struct _tagStatus2
{
   L_UINT   uStructSize;
   L_INT    nRecogChrCount;
   L_INT    nRecogWordCount;
   L_INT    nRejectChrCount;
   L_INT32  lRecogTime;
   L_INT32  lReadingTime;
   L_INT32  lPreprocTime;
   L_INT32  lDecompTime;
} STATUS2, *pSTATUS2;

typedef struct _tagResultOptions2
{
   L_UINT            uStructSize;
   DOC2_FORMATTYPE   Format;
   DOC2_FORMATLEVEL  FormatLevel;
   DOCWRTFORMAT      DocFormat;
} RESULTOPTIONS2, * pRESULTOPTIONS2;

typedef struct _tagTextFormatInfoA2
{
   L_UINT               uStructSize;
   L_CHAR *             pszName;
   L_CHAR *             pszDLLName;
   L_CHAR *             pszExtName;
} TEXTFORMATINFOA2, * pTEXTFORMATINFOA2;

#if defined(FOR_UNICODE)
typedef struct _tagTextFormatInfo2
{
   L_UINT               uStructSize;
   L_TCHAR *            pszName;
   L_TCHAR *            pszDLLName;
   L_TCHAR *            pszExtName;
} TEXTFORMATINFO2, * pTEXTFORMATINFO2;
#else
typedef TEXTFORMATINFOA2 TEXTFORMATINFO2;
typedef pTEXTFORMATINFOA2 pTEXTFORMATINFO2;
#endif// #if defined(FOR_UNICODE)

typedef struct _tagSpecialChar2
{
   L_UINT   uStructSize;
   L_WCHAR  chReject;
   L_WCHAR  chMissSym;
} SPECIALCHAR2, * pSPECIALCHAR2;

typedef struct _tagPageInfo2
{
   L_UINT   uStructSize;
   L_INT    nWidth;
   L_INT    nHeight;
   L_INT    nBitsPerPixel;
   L_BOOL   bPalette;
   L_UINT   uBytesPerLine;
   L_INT    nXRes;
   L_INT    nYRes;
} PAGEINFO2, * pPAGEINFO2;

typedef struct _tagRecognizeOpts2
{
   L_UINT   uStructSize;
   L_INT    nPageIndexStart;
   L_INT    nPagesCount;
   L_BOOL   bEnableSubSystem;
   L_BOOL   bEnableCorrection;
   DOC2_LANGIDS  SpellLangId;
} RECOGNIZEOPTS2, *pRECOGNIZEOPTS2;

typedef struct _tagRecogChars2
{
   L_UINT   uStructSize;
   RECT     rcArea;
   L_INT    nYOffset;
   L_WCHAR  wGuessCode;
   L_INT    nZoneIndex;
   L_INT    nCellIndex;
   L_INT    nConfidence;
   L_UINT   uFont;
   L_INT    nFontSize;
   L_INT    nCharFormat;
   DOC2_LANGIDS  Lang;
   DOC2_LANGIDS  Lang2;
   L_INT    nCapHeight;
   L_INT    nChoicesCount;
   L_INT    nSuggestionsCount;
   L_INT    nNextChoiceIndex;
   L_INT    nUnderLineWidthDot;
   L_INT    nUnderLineWidthGap;
   L_INT    nFGColorIndex;
   L_INT    nBGColorIndex;
} RECOGCHARS2, * pRECOGCHARS2;

typedef struct _tagRecogWords2
{
   L_UINT   uStructSize;
   L_WCHAR  szWord[DOC2_MAX_WORD_SIZE];
   RECT     rcWordArea;
   L_INT    nZoneIndex;
} RECOGWORDS2, * pRECOGWORDS2;

typedef struct _tageBookSettings
{
   L_UINT32             uStructSize;
   L_BOOL               bBullets;
   DOC2_CODEPAGESETTING      CpSetng;
   L_BOOL               bCrossrefs;
   DOC2_HEADERFOOTERSETTING  HFSetng;
   DOC2_DPISETTING           DpiSetng;
   DOC2_TABLESETTING         TableSetng;
} EBOOKSETTINGS, * pEBOOKSETTINGS;

typedef struct _tagExcel2000Settings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_CHAR * pszDocAuthor;
   L_CHAR * pszDocCategory;
   L_CHAR * pszDocComments;
   L_CHAR * pszDocCompany;
   L_CHAR * pszDocKeywords;
   L_CHAR * pszDocManager;
   L_CHAR * pszDocSubject;
   L_CHAR * pszDocTitle;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_CHAR * pszOverviewSheetName;
   L_BOOL bPageBreaks;
   L_BOOL bPageColor;
   L_CHAR * pszSheetNamePrefix;
} EXCEL2000SETTINGS, * pEXCEL2000SETTINGS;

typedef struct _tagExcel97Settings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_CHAR *pszDocAuthor;
   L_CHAR *pszDocCategory;
   L_CHAR *pszDocComments;
   L_CHAR *pszDocCompany;
   L_CHAR *pszDocKeywords;
   L_CHAR *pszDocManager;
   L_CHAR *pszDocSubject;
   L_CHAR *pszDocTitle;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_CHAR *pszOverviewSheetName;
   L_BOOL bPageBreaks;
   L_BOOL bPageColor;
   L_CHAR *pszSheetNamePrefix;
} EXCEL97SETTINGS, * pEXCEL97SETTINGS;

typedef struct _tagHtml32Settings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCrossrefs;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bHRBetweenSections;
   DOC2_HTMLINDEXPAGE IndexPage;
   L_BOOL bLineBreaks;
   L_CHAR * pszNavNextText;
   L_CHAR * pszNavPrevText;
   L_CHAR * pszNavTOCText;
   L_BOOL bPageBreaks;
   L_BOOL bPutItSubdirectory;
} HTML32SETTINGS, * pHTML32SETTINGS;

typedef struct _tagHtml40Settings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCharColors;
   L_BOOL bCharSpacing;
   L_BOOL bCrossrefs;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bHRBetweenSections;
   DOC2_HTMLINDEXPAGE IndexPage;
   L_BOOL bLineBreaks;
   L_CHAR * pszNavNextText;
   L_CHAR * pszNavPrevText;
   L_CHAR * pszNavTOCText;
   L_BOOL bPageBreaks;
   L_BOOL bPutItSubdirectory;
   L_BOOL bStyles;
} HTML40SETTINGS, * pHTML40SETTINGS;

typedef struct _tagInfoPathSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCrossrefs;
   DOC2_HEADERFOOTERSETTING HFSetng;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bRuleLines;
} INFOPATHSETTINGS, * pINFOPATHSETTINGS;

typedef struct _tagMSReaderSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCrossrefs;
   DOC2_HEADERFOOTERSETTING HFSetng;
   DOC2_DPISETTING DpiSetng;
   DOC2_TABLESETTING TableSetng;
} MSREADERSETTINGS, * pMSREADERSETTINGS;

typedef struct _tagPdfCommonSettings
{
   L_UINT32 uStructSize;
   L_BOOL bAdditionalFonts;
   L_CHAR * pszAppendFrom;
   L_CHAR * pszAppendFromPassword;
   DOC2_PDFCOLORQUALITY PdfClrQuality;
   DOC2_PDFCOMPATIBILITY PdfCompatible;
   L_BOOL bCompForContents;
   L_BOOL bCompForTTF;
   L_BOOL bUseFlate;
   L_BOOL bUseJBIG2;
   L_BOOL bUseJPEG2000;
   L_BOOL bUseLZWInsteadOfFlate;
   L_BOOL bDropCaps;
   L_BOOL bOutline;
   L_BOOL bOutlineNumbering;
   L_CHAR * pszOutlinePageName;
   L_BOOL bPDFFormVisuality;
   L_BOOL bEnableAdd;
   L_BOOL bEnableAssemble;
   L_BOOL bEnableCopy;
   L_BOOL bEnableExtract;
   L_BOOL bEnableForms;
   L_BOOL bEnableModify;
   L_BOOL bEnablePrint;
   L_BOOL bEnablePrintQ;
   L_CHAR * pszOwnerPassword;
   DOC2_PDFSECURITY Type;
   L_CHAR * pszUserPassword;
   DOC2_PDFBPP DOC2_PDFBPP;
   L_CHAR * pszCertificateDescription;
   L_CHAR * pszCertificateSHA1;
   DOC2_PDFSIGNATURE PdfSign;
   COLORREF crUrlBackground;
   COLORREF crUrlForeground;
   L_BOOL bURLForegroundDef;
   L_BOOL bURLUnderline;
   DOC2_PDFMRC DOC2_PDFMRC;
} PDFCOMMONSETTINGS, * pPDFCOMMONSETTINGS;

typedef struct _tagPdfSettings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bNeedTaggedInfo;
   L_BOOL bPDFThumbnail;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bPDFForms;
   L_BOOL bShowBackgroundImage;
   L_BOOL bShowText;
   L_BOOL bRuleLines;
   L_BOOL bShowSubstitutes;
   L_BOOL bURLBackgroundDef;
   L_BOOL bURLHighlight;
} PDFSETTINGS, * pPDFSETTINGS;

typedef struct _tagPdfEditedSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCrossrefs;
   L_BOOL bFieldCodes;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bNeedTaggedInfo;
   L_BOOL bPDFForms;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bRuleLines;
   L_BOOL bURLBackgroundDef;
   L_BOOL bURLHighlight;
} PDFEDITEDSETTINGS, * pPDFEDITEDSETTINGS;

typedef struct _tagPdfImageOnTextSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCrossrefs;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bNeedTaggedInfo;
   L_BOOL bPDFThumbnail;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bPDFForms;
   L_BOOL bShowBackgroundImage;
   L_BOOL bShowText;
   L_BOOL bRuleLines;
   L_BOOL bURLBackgroundDef;
   L_BOOL bURLHighlight;
} PDFIMAGEONTEXTSETTINGS, *pPDFIMAGEONTEXTSETTINGS;

typedef struct _tagPdfImageSubstSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCrossrefs;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bNeedTaggedInfo;
   L_BOOL bPDFThumbnail;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bPDFForms;
   L_BOOL bShowBackgroundImage;
   L_BOOL bShowText;
   L_BOOL bRuleLines;
   L_BOOL bURLBackgroundDef;
   L_BOOL bURLHighlight;
} PDFIMAGESUBSTSETTINGS, * pPDFIMAGESUBSTSETTINGS;

typedef struct _tagPowerPoint97Settings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCharColors;
   L_CHAR * pszDocAuthor;
   L_CHAR * pszDocCategory;
   L_CHAR * pszDocComments;
   L_CHAR * pszDocCompany;
   L_CHAR * pszDocKeywords;
   L_CHAR * pszDocManager;
   L_CHAR * pszDocSubject;
   L_CHAR * pszDocTitle;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bLineBreaks;
   DOC2_FORMATLEVEL OutputMode;
   L_BOOL bPageBreaks;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bStyles;
   DOC2_TABLESETTING TableSetng;
   L_BOOL bTabs;
} POWERPOINT97SETTINGS, * pPOWERPOINT97SETTINGS;

typedef struct _tagPublisher98Settings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCharColors;
   L_BOOL bCharScaling;
   L_BOOL bCharSpacing;
   L_CHAR * pszDocAuthor;
   L_CHAR * pszDocCategory;
   L_CHAR * pszDocComments;
   L_CHAR * pszDocCompany;
   L_CHAR * pszDocKeywords;
   L_CHAR * pszDocManager;
   L_CHAR * pszDocSubject;
   L_CHAR * pszDocTitle;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bLineBreaks;
   DOC2_FORMATLEVEL OutputMode;
   L_BOOL bPageBreaks;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bStyles;
   DOC2_TABLESETTING TableSetng;
   L_BOOL bTabs;
} PUBLISHER98SETTINGS, * pPUBLISHER98SETTINGS;

typedef struct _tagRtfDocWordMLSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCharColors;
   L_BOOL bCharScaling;
   L_BOOL bCharSpacing;
   L_BOOL bColumnBreaks;
   L_BOOL bConsolidatePages;
   L_CHAR * pszDocAuthor;
   L_CHAR * pszDocCategory;
   L_CHAR * pszDocComments;
   L_CHAR * pszDocCompany;
   L_CHAR * pszDocKeywords;
   L_CHAR * pszDocManager;
   L_CHAR * pszDocSubject;
   L_CHAR * pszDocTitle;
   L_BOOL bDropCaps;
   L_BOOL bFieldCodes;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bLineBreaks;
   DOC2_FORMATLEVEL OutputMode;
   DOC2_PAGEBREAKSETTING PgBreakValue;
   L_BOOL bPageColor;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bRuleLines;
   L_BOOL bStyles;
   DOC2_TABLESETTING TableSetng;
   L_BOOL bTabs;
} RTFDOCWORDMLSETTINGS, * pRTFDOCWORDMLSETTINGS;

typedef struct _tagRtf2000Settings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
} RTF2000SETTINGS, * pRTF2000SETTINGS;

typedef struct _tagRtf2000SWordSettings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
} RTF2000SWORDSETTINGS, * pRTF2000SWORDSETTINGS;

typedef struct _tagRtf6Settings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
} RTF6SETTINGS, * pRTF6SETTINGS;

typedef struct _tagRtf97Settings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
} RTF97SETTINGS, * pRTF97SETTINGS;

typedef struct _tagWord2000Settings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
} WORD2000SETTINGS, * pWORD2000SETTINGS;

typedef struct _tagWord97Settings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
} WORD97SETTINGS, * pWORD97SETTINGS;

typedef struct _tagWordMLSettings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
} WORDMLSETTINGS, * pWORDMLSETTINGS;

typedef struct _tagTextCommonSettings
{
   L_UINT32 uStructSize;
   L_BOOL bAppend;
   L_CHAR * pszAppendFrom;
   DOC2_HEADERFOOTERSETTING HFSetng;
} TEXTCOMMONSETTINGS, * pTEXTCOMMONSETTINGS;

typedef struct _tagTextCsvSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   DOC2_CODEPAGESETTING CpSetng;
   L_WCHAR * pszListSeparator;
   L_BOOL bUseOSListSeparator;
} TEXTCSVSETTINGS, * pTEXTCSVSETTINGS;

typedef struct _tagTextUCsvSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bIntelUnicodeByteOrder;
   L_WCHAR * pszListSeparator;
   L_BOOL bUseOSListSeparator;
} TEXTUCSVSETTINGS, * pTEXTUCSVSETTINGS;

typedef struct _tagTextFormattedSettings
{
   L_UINT32 uStructSize;
   DOC2_CODEPAGESETTING CpSetng;
   L_WCHAR * pszLineBreak;
   L_BOOL bLineBreaks;
   L_BOOL bPageBreaks;
   L_BOOL bPageMargins;
} TEXTFORMATTEDSETTINGS, * pTEXTFORMATTEDSETTINGS;

typedef struct _tagTextUFormattedSettings
{
   L_UINT32 uStructSize;
   L_BOOL bIntelUnicodeByteOrder;
   L_WCHAR * pszLineBreak;
   L_BOOL bLineBreaks;
   L_BOOL bPageBreaks;
   L_BOOL bPageMargins;
} TEXTUFORMATTEDSETTINGS, * pTEXTUFORMATTEDSETTINGS;

typedef struct _tagTextSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   DOC2_CODEPAGESETTING CpSetng;
   L_BOOL bConvertTabs;
   L_WCHAR * pszLineBreak;
   L_BOOL bLineBreaks;
   L_WCHAR * pszPageBreak;
   L_BOOL bPageBreaks;
} TEXTSETTINGS, * pTEXTSETTINGS;

typedef struct _tagTextLineBreaksSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   DOC2_CODEPAGESETTING CpSetng;
   L_BOOL bConvertTabs;
   L_WCHAR * pszLineBreak;
   L_BOOL bLineBreaks;
   L_WCHAR * pszPageBreak;
   L_BOOL bPageBreaks;
} TEXTLINEBREAKSSETTINGS, * pTEXTLINEBREAKSSETTINGS;

typedef struct _tagUTextSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bConvertTabs;
   L_BOOL bIntelUnicodeByteOrder;
   L_WCHAR * pszLineBreak;
   L_BOOL bLineBreaks;
   L_WCHAR * pszPageBreak;
   L_BOOL bPageBreaks;
} UTEXTSETTINGS, * pUTEXTSETTINGS;

typedef struct _tagUTextLineBreaksSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bConvertTabs;
   L_BOOL bIntelUnicodeByteOrder;
   L_WCHAR * pszLineBreak;
   L_BOOL bLineBreaks;
   L_WCHAR * pszPageBreak;
   L_BOOL bPageBreaks;
} UTEXTLINEBREAKSSETTINGS, * pUTEXTLINEBREAKSSETTINGS;

typedef struct _tagWordPadSettings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bCharColors;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bLineBreaks;
   DOC2_FORMATLEVEL OutputMode;
   L_BOOL bPageBreaks;
   DOC2_DPISETTING DpiSetng;
   DOC2_TABLESETTING TableSetng;
   L_BOOL bTabs;
} WORDPADSETTINGS, * pWORDPADSETTINGS;

typedef struct _tagWordPerfect10Settings
{
   L_UINT32 uStructSize;
   L_BOOL bCrossrefs;
   L_BOOL bFieldCodes;
   L_BOOL bRuleLines;
} WORDPERFECT10SETTINGS, * pWORDPERFECT10SETTINGS;

typedef struct _tagWordPerfect8Settings
{
   L_UINT32 uStructSize;
   L_BOOL bBullets;
   L_BOOL bColumnBreaks;
   L_BOOL bConsolidatePages;
   L_CHAR * pszDocAuthor;
   L_CHAR * pszDocCategory;
   L_CHAR * pszDocComments;
   L_CHAR * pszDocCompany;
   L_CHAR * pszDocKeywords;
   L_CHAR * pszDocManager;
   L_CHAR * pszDocSubject;
   L_CHAR * pszDocTitle;
   L_BOOL bDropCaps;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bLineBreaks;
   DOC2_FORMATLEVEL OutputMode;
   DOC2_PAGEBREAKSETTING PgBreakValue;
   L_BOOL bPageColor;
   DOC2_DPISETTING DpiSetng;
   L_BOOL bStyles;
   DOC2_TABLESETTING TableSetng;
   L_BOOL bTabs;
} WORDPERFECT8SETTINGS, * pWORDPERFECT8SETTINGS;

typedef struct _tagXMLSettings
{
   L_UINT32 uStructSize;
   L_CHAR * pszDocAuthor;
   L_CHAR * pszDocCategory;
   L_CHAR * pszDocComments;
   L_CHAR * pszDocCompany;
   L_CHAR * pszDocKeywords;
   L_CHAR * pszDocManager;
   L_CHAR * pszDocSubject;
   L_CHAR * pszDocTitle;
   DOC2_HEADERFOOTERSETTING HFSetng;
   L_BOOL bInsertCharacterCoordinates;
   L_WCHAR * pszSchemaLocation;
   L_BOOL bUseXsdSchema;
} XMLSETTINGS, * pXMLSETTINGS;

/****************************************************************
   Callback typedefs
****************************************************************/

typedef L_BOOL (pEXT_CALLBACK PROGRESSCALLBACK2)(
                              pPROGRESSDATA2 pProgressData,
                              L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK ENUMOUTPUTFILEFORMATS2)(
                              DOC2_FORMATTYPE Format,
                              L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK RECOGNIZESTATUSCALLBACK2)(
                              L_INT nRecogPage,
                              L_INT nError,
                              L_VOID * pUserData);

/****************************************************************
   Function prototypes
****************************************************************/

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2StartUpA(L_HDOC2 * phDoc, L_CHAR * pszEnginePath, L_BOOL bUseThunk);
#if defined(FOR_UNICODE)
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2StartUp(L_HDOC2 * phDoc, L_TCHAR * pszEnginePath, L_BOOL bUseThunk);
#else
#define L_Doc2StartUp L_Doc2StartUpA
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2ShutDown(L_HDOC2 * phDoc);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2LoadSettingsA(L_HDOC2 hDoc, L_CHAR * pszFileName);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SaveSettingsA(L_HDOC2 hDoc, L_CHAR * pszFileName);
#if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2LoadSettings(L_HDOC2 hDoc, L_TCHAR * pszFileName);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SaveSettings(L_HDOC2 hDoc, L_TCHAR * pszFileName);
#else
#define L_Doc2LoadSettings L_Doc2LoadSettingsA
#define L_Doc2SaveSettings L_Doc2SaveSettingsA
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetProgressCB(L_HDOC2 hDoc, PROGRESSCALLBACK2 pfnCallback, L_VOID * pUserData);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2AddPage(L_HDOC2 hDoc, pBITMAPHANDLE pBitmap, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetPageCount(L_HDOC2 hDoc, L_INT * pnPageCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2UpdatePage(L_HDOC2 hDoc, pBITMAPHANDLE pBitmap, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2RemovePage(L_HDOC2 hDoc, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2ExportPage(L_HDOC2 hDoc, pBITMAPHANDLE pBitmap, L_UINT uStructSize, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetPageInfo(L_HDOC2 hDoc, L_INT nPageIndex, pPAGEINFO2 pPageInfo, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2AddZoneA(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATAA2 pZoneData);
#if defined(FOR_UNICODE)
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2AddZone(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATA2 pZoneData);
#else
#define L_Doc2AddZone L_Doc2AddZoneA
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetZoneCount(L_HDOC2 hDoc, L_INT nPageIndex, L_INT * pnCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetZoneA(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATAA2 pZoneData, L_UINT uStructSize);
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2UpdateZoneA(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATAA2 pZoneData);
#if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetZone(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATA2 pZoneData, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2UpdateZone(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATA2 pZoneData);
#else
#define L_Doc2GetZone L_Doc2GetZoneA
#define L_Doc2UpdateZone L_Doc2UpdateZoneA
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2RemoveZone(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2ImportZonesA(L_HDOC2 hDoc, L_INT nPageIndex, L_CHAR * pszFileName);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2ExportZonesA(L_HDOC2 hDoc, L_INT nPageIndex, L_CHAR * pszFileName);
#if defined(FOR_UNICODE)
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2ImportZones(L_HDOC2 hDoc, L_INT nPageIndex, L_TCHAR * pszFileName);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2ExportZones(L_HDOC2 hDoc, L_INT nPageIndex, L_TCHAR * pszFileName);
#else
#define L_Doc2ImportZones L_Doc2ImportZonesA
#define L_Doc2ExportZones L_Doc2ExportZonesA
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2FindZones(L_HDOC2 hDoc, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetZoneOptions(L_HDOC2 hDoc, pAUTOZONEOPTS2 pZoneOpts);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetZoneOptions(L_HDOC2 hDoc, pAUTOZONEOPTS2 pZoneOpts, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetFillMethod(L_HDOC2 hDoc, DOC2_FILLMETHOD FillMethod);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetFillMethod(L_HDOC2 hDoc, DOC2_FILLMETHOD * pFillMethod);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2FindDefaultFillMethod(L_HDOC2 hDoc, L_INT nPageIndex, DOC2_FILLMETHOD * pFillMethod);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SelectLanguages(L_HDOC2 hDoc, DOC2_LANGIDS * pLangIds, L_INT nLangCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetSelectedLanguages(L_HDOC2 hDoc, DOC2_LANGIDS * * ppLangIds, L_INT * pnLangCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2FreeLanguages(L_HDOC2 hDoc, DOC2_LANGIDS * * ppLangIds);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetCharLangsOptions(L_HDOC2 hDoc, pCHAROPTIONS2 pCharOpts);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetCharLangsOptions(L_HDOC2 hDoc, pCHAROPTIONS2 pCharOpts, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetDefaultSpellLanguages(L_HDOC2 hDoc, DOC2_LANGIDS * * ppLangIds, L_INT *pnLangCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetRecognizeModuleTradeOff(L_HDOC2 hDoc, DOC2_RECOGMODULE_TRADEOFF TradeOff);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetRecognizeModuleTradeOff(L_HDOC2 hDoc, DOC2_RECOGMODULE_TRADEOFF *pTradeOff);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetMOROptions(L_HDOC2 hDoc, pMOROPTIONS2 pMOROpts);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetMOROptions(L_HDOC2 hDoc, pMOROPTIONS2 pMOROpts, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetHandPrintOptions(L_HDOC2 hDoc, pHANDPRINTOPTIONS2 pHandOpts);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetHandPrintOptions(L_HDOC2 hDoc, pHANDPRINTOPTIONS2 pHandOpts, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetOMROptions(L_HDOC2 hDoc, pOMROPTIONS2 pOMROpts);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetOMROptions(L_HDOC2 hDoc, pOMROPTIONS2 pOMROpts, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetUserDictionaryA(L_HDOC2 hDoc, pUSERDICTIONARYA2 pUDOpts, L_BOOL bCreateUD);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetUserDictionaryA(L_HDOC2 hDoc, pUSERDICTIONARYA2 pUDOpts, L_UINT uStructSize);
#if defined(FOR_UNICODE)
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetUserDictionary(L_HDOC2 hDoc, pUSERDICTIONARY2 pUDOpts, L_BOOL bCreateUD);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetUserDictionary(L_HDOC2 hDoc, pUSERDICTIONARY2 pUDOpts, L_UINT uStructSize);
#else
#define L_Doc2SetUserDictionary L_Doc2SetUserDictionaryA
#define L_Doc2GetUserDictionary L_Doc2GetUserDictionaryA
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_BOOL EXT_FUNCTION L_Doc2GetUserDictionaryState(L_HDOC2 hDoc);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetUserDictionarySection(L_HDOC2 hDoc, L_CHAR * pSection, L_INT nSize, L_BOOL bFirst);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetUserDictionarySectionItem(L_HDOC2 hDoc, L_CHAR * pSection, L_WCHAR * pItem, L_INT nSize, L_UINT32 * puMask, L_BOOL bFirst);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2AddItemToUserDictionary(L_HDOC2 hDoc, L_CHAR * pSection, L_WCHAR * pUDitem, L_UINT32 uMask);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2RemoveItemFromUserDictionary(L_HDOC2 hDoc, L_CHAR * pSection, L_WCHAR * pUDitem, L_UINT32 uMask);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetStatus(L_HDOC2 hDoc, pSTATUS2 pStatus, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2Recognize(L_HDOC2 hDoc, pRECOGNIZEOPTS2 pRecogOpts, RECOGNIZESTATUSCALLBACK2 pfnCallback, L_VOID * pUserData);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetRecognitionResultOptions(L_HDOC2 hDoc, pRESULTOPTIONS2 pResOpts);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetRecognitionResultOptions(L_HDOC2 hDoc, pRESULTOPTIONS2 pResOpts, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2EnumOutputFileFormats(L_HDOC2 hDoc, ENUMOUTPUTFILEFORMATS2 pfnCallback, L_VOID * pUserData);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetTextFormatInfoA(L_HDOC2 hDoc, DOC2_FORMATTYPE Format, pTEXTFORMATINFOA2 pFormatInfo, L_UINT uStructSize);
#if defined(FOR_UNICODE)
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetTextFormatInfo(L_HDOC2 hDoc, DOC2_FORMATTYPE Format, pTEXTFORMATINFO2 pFormatInfo, L_UINT uStructSize);
#else
#define L_Doc2GetTextFormatInfo L_Doc2GetTextFormatInfoA
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SaveResultsToFileA(L_HDOC2 hDoc, L_CHAR * pszFileName);
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SaveResultsToFile2A(L_HDOC2 hDoc, L_CHAR * pszFileName, L_UINT uFlags);

#if defined(FOR_UNICODE)
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SaveResultsToFile(L_HDOC2 hDoc, L_TCHAR * pszFileName);
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SaveResultsToFile2(L_HDOC2 hDoc, L_TCHAR * pszFileName, L_UINT uFlags);
#else
#define L_Doc2SaveResultsToFile L_Doc2SaveResultsToFileA
#define L_Doc2SaveResultsToFile2 L_Doc2SaveResultsToFile2A
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetSpecialChar(L_HDOC2 hDoc, pSPECIALCHAR2 pSpecialChar);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetSpecialChar(L_HDOC2 hDoc, pSPECIALCHAR2 pSpecialChar, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetRecognizedCharacters(L_HDOC2 hDoc, L_INT nPageIndex, pRECOGCHARS2 * ppRecogChars, L_INT32 * plCharsCount, L_UINT uStructSize);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetRecognizedCharacters(L_HDOC2 hDoc, L_INT nPageIndex, pRECOGCHARS2 pRecogChars, L_INT32 lCharsCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2FreeRecognizedCharacters(L_HDOC2 hDoc, pRECOGCHARS2 * ppRecogChars);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetRecognizedWords(L_HDOC2 hDoc, L_INT nPageIndex, pRECOGWORDS2 * ppRecogWords, L_UINT uStructSize, L_INT * pnWordsCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2FreeRecognizedWords(L_HDOC2 hDoc, pRECOGWORDS2 * ppRecogWords);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2AutoOrientPage(L_HDOC2 hDoc, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2DetectOrientationDegree(L_HDOC2 hDoc, L_INT nPageIndex, L_INT * pnRotate);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2LockPage(L_HDOC2 hDoc, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2UnlockPage(L_HDOC2 hDoc, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetActivePage(L_HDOC2 hDoc, L_INT nPageIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2DrawPage(L_HDOC2 hDoc, HDC hDC, L_INT nPageIndex, LPRECT pSrc, LPRECT pClipSrc, LPRECT pDst, LPRECT pClipDst, L_UINT32 uROP3, L_BOOL bShowZones);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SelectZoneByPoint(L_HDOC2 hDoc, HDC hDC, L_INT nPageIndex, POINT pt, L_INT * pnZoneIndex);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SelectZone(L_HDOC2 hDoc, HDC hDC, L_INT nPageIndex, L_INT nZoneIndex, L_BOOL bSelect);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2RemoveSelectedZone(L_HDOC2 hDoc, L_INT nPageIndex);

L_LTDOC2_API HPEN  EXT_FUNCTION L_Doc2SetZonePen(L_HDOC2 hDoc, HPEN hPen);

L_LTDOC2_API HPEN  EXT_FUNCTION L_Doc2SetSelectedZonePen(L_HDOC2 hDoc, HPEN hPen);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetSelectedZoneA(L_HDOC2 hDoc, L_INT nPageIndex, pZONEDATAA2 pZoneData, L_UINT uStructSize);
#if defined(FOR_UNICODE)
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetSelectedZone(L_HDOC2 hDoc, L_INT nPageIndex, pZONEDATA2 pZoneData, L_UINT uStructSize);
#else
#define L_Doc2GetSelectedZone L_Doc2GetSelectedZoneA
#endif // #if defined(FOR_UNICODE)

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2Zone(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, LPRECT lpArea);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2OffsetZones(L_HDOC2 hDoc, POINT pt);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetPaintZoomFactor(L_HDOC2 hDoc, L_INT nPageIndex, L_FLOAT fZoomFactor);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetPaintZoomFactor(L_HDOC2 hDoc, L_INT nPageIndex, L_FLOAT * pfZoomFactor);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2CleanupPages(L_HDOC2 hDoc, L_BOOL bAutoCleanup);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2CreateSettingsCollection(L_HDOC2 hDoc, L_INT nParentSid, L_INT * pnNewSid);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2DeleteSettingsCollection(L_HDOC2 hDoc, L_INT nSid);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetActiveSettingsCollection(L_HDOC2 hDoc, L_INT nSid);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetRecognizedCharactersColors(L_HDOC2 hDoc, L_INT nPageIndex, COLORREF * pClrs, L_INT * pnClrsCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetZoneNodes(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, PPOINT * ppPoints, L_INT *pnNodesCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2AddZoneRect(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, RECT * prc);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2ExcludeZoneRect(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, RECT * prc);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetZoneLayout(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, RECT * prc, L_INT nRectCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetZoneLayout(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, RECT ** pprc, L_INT * pnRectCount);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetZoneColor(L_HDOC2 hDoc, L_INT nPageIndex, L_INT nZoneIndex, COLORREF * pClr);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetOutputFormatSettings(L_HDOC2 hDoc, DOC2_FORMATTYPE formatType, L_UINT uFlags, L_VOID * pFormatSettings, L_UINT uStructSize);
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetOutputFormatSettings(L_HDOC2 hDoc, DOC2_FORMATTYPE formatType, L_UINT uFlags, L_VOID * pFormatSettings);

L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2SetDocumentWriterOptions(L_HDOC2 hDoc, L_VOID * pOptions, L_UINT uStructSize);
L_LTDOC2_API L_INT EXT_FUNCTION L_Doc2GetDocumentWriterOptions(L_HDOC2 hDoc, L_VOID * pOptions, L_UINT uStructSize);


#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDOC2_H)