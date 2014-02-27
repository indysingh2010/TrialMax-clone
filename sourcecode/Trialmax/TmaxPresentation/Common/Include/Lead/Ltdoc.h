/*************************************************************
   Ltdoc.h - document recognition module header file
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDOC_H)
#define LTDOC_H

#if !defined(L_LTDOC_API)
   #define L_LTDOC_API
#endif // #if !defined(L_LTDOC_API)

#include "Lttyp.h"
#include "Lterr.h"

#if defined(LEADTOOLS_V16_OR_LATER)
#include "Ltdocwrt.h"
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

#define L_HEADER_ENTRY
#include "Ltpck.h"

/****************************************************************
   Enums/defines/macros
****************************************************************/

/* uFlags values of ZONEDATA structure */
/* input flags:*/
#define ZONE_CHK_LANGDICT_PROHIBIT     0x00000001
#define ZONE_CHK_USERDICT_PROHIBIT     0x00000002
#define ZONE_CHK_CHECKCBF_PROHIBIT     0x00000004
#define ZONE_CHK_VERTDICT_PROHIBIT     0x00000008
#define ZONE_CHK_IGNORE_WHITESPACE     0x00000010
#define ZONE_CHK_IGNORE_CASE           0x00000020
#define ZONE_CHK_PASS_LINES            0x00000040
#define ZONE_CHK_CORRECTION_DISABLED   0x00000080
#define ZONE_CHK_INCLUDE_PUNCTUATION   0x00000100
#define ZONE_CHK_CORRECT_PROPERNAMES   0x00000200
/* output flags:*/
#define ZONE_CHK_LANGDICT_USED         0x00010000
#define ZONE_CHK_USERDICT_USED         0x00020000
#define ZONE_CHK_CHECKCBF_USED         0x00040000

// Color flags
#define COLOR_BLUE                     0x001
#define COLOR_GREEN                    0x002
#define COLOR_RED                      0x004
#define COLOR_INTENSIVE                0x008
#define COLOR_NONE                     0x00F

// Character position flags
#define CHAR_ENDOFLINE                 0x001
#define CHAR_ENDOFPARA                 0x002
#define CHAR_ENDOFWORD                 0x004
#define CHAR_ENDOFZONE                 0x008
#define CHAR_ENDOFPAGE                 0x010
#define CHAR_ENDOFCELL                 0x020

// Font description flags
#define FONT_ITALIC                    0x001
#define FONT_BOLD                      0x002
#define FONT_UNDERLINE                 0x004
#define FONT_SUBSCRIPT                 0x008
#define FONT_SUPERSCRIPT               0x010
#define FONT_SANSSERIF                 0x020
#define FONT_SERIF                     0x040
#define FONT_PROPORTIONAL              0x080

// User dictionary mask
#define USER_DICT_LITERAL              0x0000
#define USER_DICT_REGULAR_EXPRESSION   0x0001

typedef enum
{
   ZONE_CHAR_FILTER_DEFAULT = 0,
   ZONE_CHAR_FILTER_DIGIT,
   ZONE_CHAR_FILTER_UPPERCASE,
   ZONE_CHAR_FILTER_LOWERCASE,
   ZONE_CHAR_FILTER_PUNCTUATION,
   ZONE_CHAR_FILTER_MISCELLANEOUS,
   ZONE_CHAR_FILTER_PLUS,
   ZONE_CHAR_FILTER_ALL,
   ZONE_CHAR_FILTER_ALPHA,
   ZONE_CHAR_FILTER_NUMBERS,
   ZONE_CHAR_FILTER_OCRA
} CHAR_FILTER;

typedef enum 
{
   FILL_DEFAULT = 0,
   FILL_OMNIFONT,
   FILL_DRAFTDOT9,
   FILL_OMR,
   FILL_HANDPRINT,
   FILL_DRAFTDOT24,
   FILL_OCRA,
   FILL_OCRB,
   FILL_MICR,
   FILL_DOTDIGIT,
   FILL_DASHDIGIT,
   FILL_NO_OCR,
} FILLMETHOD;

typedef enum
{
   RECOGMODULE_AUTO = 0,
   RECOGMODULE_MTEXT_OMNIFONT,
   RECOGMODULE_MULTI_LINGUAL_OMNIFONT,
   RECOGMODULE_DOT_MATRIX,
   RECOGMODULE_OMR,
   RECOGMODULE_HAND_PRINTED_NUMERAL,
   RECOGMODULE_RER_PRINTED,
   RECOGMODULE_MATRIX,
   RECOGMODULE_OMNIFONT_PLUS2W,
   RECOGMODULE_OMNIFONT_FRX,
   RECOGMODULE_OMNIFONT_PLUS3W,
} RECOGMODULE;

typedef enum
{
   // Basic Zone type
   ZONE_FLOWTEXT = 0,
   ZONE_TABLE,
   ZONE_GRAPHIC,

   // Further types created by the page-layout decomposition process: 
   ZONE_COLUMN,
   ZONE_HEADER,
   ZONE_FOOTER,
   ZONE_CAPTION,
   ZONE_TITLE,
   ZONE_OTHER,
   ZONE_SGRAPHIC,
   ZONE_HORZTEXT,
   ZONE_VERTTEXT,
} ZONETYPE;

typedef enum
{
   PARSE_AUTO = 0,
   PARSE_LEGACY,
   PARSE_STANDARD,
   PARSE_FAST
} PAGEPARSER;

typedef enum
{
   LANG_ID_AUTO = -2,
   LANG_ID_NO = -1,
   LANG_ID_ENGLISH = 0,
   LANG_ID_GERMAN,
   LANG_ID_FRENCH,
   LANG_ID_DUTCH,
   LANG_ID_NORWEGIAN,
   LANG_ID_SWEDISH,
   LANG_ID_FINNISH,
   LANG_ID_DANISH,
   LANG_ID_ICELANDIC,
   LANG_ID_PORTUGUESE,
   LANG_ID_SPANISH,
   LANG_ID_CATALAN,
   LANG_ID_ITALIAN,
   LANG_ID_MALTESE,
   LANG_ID_GREEK,
   LANG_ID_POLISH,
   LANG_ID_CZECH,
   LANG_ID_SLOVAK,
   LANG_ID_HUNGARIAN,
   LANG_ID_SLOVENIAN,
   LANG_ID_CROATIAN,
   LANG_ID_ROMANIAN,
   LANG_ID_ALBANIAN,
   LANG_ID_TURKISH,
   LANG_ID_ESTONIAN,
   LANG_ID_LATVIAN,
   LANG_ID_LITHUANIAN,
   LANG_ID_ESPERANTO,
   LANG_ID_SERBIAN,
   LANG_ID_MACEDONIAN,
   LANG_ID_MOLDAVIAN,
   LANG_ID_BULGARIAN,
   LANG_ID_BYELORUSSIAN,
   LANG_ID_UKRAINIAN,
   LANG_ID_RUSSIAN,
   LANG_ID_AFRIKAANS,
   LANG_ID_AYMARA,
   LANG_ID_BASQUE,
   LANG_ID_BEMBA,
   LANG_ID_BLACKFOOT,
   LANG_ID_BRETON,
   LANG_ID_BRAZILIAN,
   LANG_ID_BUGOTU,
   LANG_ID_CHAMORRO,
   LANG_ID_CHECHEN,
   LANG_ID_CHUANA_TSWANA,
   LANG_ID_CORSICAN,
   LANG_ID_CROW,
   LANG_ID_ESKIMO,
   LANG_ID_FAROESE,
   LANG_ID_FIJIAN,
   LANG_ID_FRISIAN,
   LANG_ID_FRIULIAN,
   LANG_ID_GAELIC_IRISH,
   LANG_ID_GAELIC_SCOTTISH,
   LANG_ID_GANDA_LUGANDA,
   LANG_ID_GUARANI,
   LANG_ID_HANI,
   LANG_ID_HAWAIIAN,
   LANG_ID_IDO,
   LANG_ID_INDONESIAN,
   LANG_ID_INTERLINGUA,
   LANG_ID_KABARDIAN,
   LANG_ID_KASUB,
   LANG_ID_KAWA,
   LANG_ID_KIKUYU,
   LANG_ID_KONGO,
   LANG_ID_KPELLE,
   LANG_ID_KURDISH,
   LANG_ID_LAPPISH,
   LANG_ID_LATIN,
   LANG_ID_LUBA,
   LANG_ID_LUXEMBOURGIAN,
   LANG_ID_MALAGASY,
   LANG_ID_MALAY,
   LANG_ID_MALINKE,
   LANG_ID_MAORI,
   LANG_ID_MAYAN,
   LANG_ID_MIAO,
   LANG_ID_MINANKABAW,
   LANG_ID_MOHAWK,
   LANG_ID_NAHUATL,
   LANG_ID_NYANJA,
   LANG_ID_OCCIDENTAL,
   LANG_ID_OJIBWAY,
   LANG_ID_PAPIAMENTO,
   LANG_ID_PIDGIN_ENGLISH,
   LANG_ID_PROVENCAL,
   LANG_ID_QUECHUA,
   LANG_ID_RHAETIC,
   LANG_ID_ROMANY,
   LANG_ID_RUANDA,
   LANG_ID_RUNDI,
   LANG_ID_SAMOAN,
   LANG_ID_SARDINIAN,
   LANG_ID_SHONA,
   LANG_ID_SIOUX,
   LANG_ID_SOMALI,
   LANG_ID_SOTHO_SUTO_SESUTO,
   LANG_ID_SUNDANESE,
   LANG_ID_SWAHILI,
   LANG_ID_SWAZI,
   LANG_ID_TAGALOG,
   LANG_ID_TAHITIAN,
   LANG_ID_TINPO,
   LANG_ID_TONGAN,
   LANG_ID_TUN,
   LANG_ID_VISAYAN,
   LANG_ID_WELSH,
   LANG_ID_WEND_SORBIAN,
   LANG_ID_WOLOF,
   LANG_ID_XHOSA,
   LANG_ID_ZAPOTEC,
   LANG_ID_ZULU,
   LANG_ID_GALICIAN,
   LANG_ID_SERBIAN_LATIN,
   LANG_ID_SAMI,
   LANG_ID_LULE_SAMI,
   LANG_ID_NORTHERN_SAMI,
   LANG_ID_SOUTHERN_SAMI,
} LANGIDS;

typedef enum
{
   RECGMD_ACCURATE,
   RECGMD_BALANCED,
   RECGMD_FAST,
} RECOGMODULE_TRADEOFF;

typedef enum
{
   HAND_STYLE_EURO,
   HAND_STYLE_US
} HANDRECOG_STYLE;

#if !defined(LEADTOOLS_V16_OR_LATER)
typedef enum
{
   OMR_SENSE_NORMAL,
   OMR_SENSE_LOW,
   OMR_SENSE_LOWER,
   OMR_SENSE_LOWEST,
} OMRSENSE;

typedef enum
{
   OMR_FRAME_AUTO,
   OMR_FRAME_NO,
   OMR_FRAME_YES,
} OMRFRAME;
#else
typedef enum
{
   OMR_SENSE_HIGHEST,
   OMR_SENSE_HIGH,
   OMR_SENSE_LOW,
   OMR_SENSE_LOWEST,
} OMRSENSE;

typedef enum
{
   OMR_AUTO_FRAME,
   OMR_WITHOUT_FRAME,
   OMR_WITH_FRAME,
} OMRFRAME;
#endif // #if !defined(LEADTOOLS_V16_OR_LATER)

typedef enum
{
   FORMAT_LEVEL_FULL = 0,
   FORMAT_LEVEL_PART,
   FORMAT_LEVEL_DROP,
   FORMAT_LEVEL_CUSTOM = 0xFFF,
} FORMATLEVEL;

typedef enum
{
   TABLE_METHOD_USETABS,
   TABLE_METHOD_USECELLS,
} TABLEMETHOD;

typedef enum
{
   SEL_NONE,
   SEL_AUTO,
   SEL_PREDEFINED
} SELECTOR;

typedef enum
{
   PAPER_TYPE_A4,
   PAPER_TYPE_A0,
   PAPER_TYPE_A1,
   PAPER_TYPE_A2,
   PAPER_TYPE_A3,
   PAPER_TYPE_A5,
   PAPER_TYPE_A6,
   PAPER_TYPE_A7,
   PAPER_TYPE_B0,
   PAPER_TYPE_B1,
   PAPER_TYPE_B2,
   PAPER_TYPE_B3,
   PAPER_TYPE_B4,
   PAPER_TYPE_B5,
   PAPER_TYPE_B6,
   PAPER_TYPE_B7,
   PAPER_TYPE_LETTER,
   PAPER_TYPE_LEGAL,
   PAPER_TYPE_EXECUTIVE,
   PAPER_TYPE_FROMIMAGEMAX,
} PAPERTYPE;

typedef enum
{
   ORIENT_PORTRAIT  = 0,
   ORIENT_LANDSCAPE,
} PAPERORIENTATION;

typedef enum
{
   LANG_OUT_NO_LANGUAGE,
   LANG_OUT_ALBANIAN,
   LANG_OUT_FRENCH,
   LANG_OUT_FRENCH_BELGIAN,
   LANG_OUT_NORWEGIAN_NYNORSK,
   LANG_OUT_POLISH,
   LANG_OUT_BAHASA,
   LANG_OUT_CATALAN,
   LANG_OUT_CROATO_SERBIAN_LATIN,
   LANG_OUT_CZECH,
   LANG_OUT_DANISH,
   LANG_OUT_DUTCH,
   LANG_OUT_DUTCH_BELGIAN,
   LANG_OUT_ENGLISH_AUSTRALIAN,
   LANG_OUT_ENGLISH_UK,
   LANG_OUT_ENGLISH_US,
   LANG_OUT_FINNISH,
   LANG_OUT_FRENCH_CANADIAN,
   LANG_OUT_FRENCH_SWISS,
   LANG_OUT_GERMAN,
   LANG_OUT_GERMAN_SWISS,
   LANG_OUT_GREEK,
   LANG_OUT_HUNGARIAN,
   LANG_OUT_ICELANDIC,
   LANG_OUT_ITALIAN,
   LANG_OUT_ITALIAN_SWISS,
   LANG_OUT_NORWEGIAN,
   LANG_OUT_PORTUGUESE_BRAZILIAN,
   LANG_OUT_PORTUGUESE,
   LANG_OUT_RHAETO_ROMANIC,
   LANG_OUT_ROMANIAN,
   LANG_OUT_SLOVAK,
   LANG_OUT_SPANISH_CASTILIAN,
   LANG_OUT_SPANISH_MEXICAN,
   LANG_OUT_SWEDISH,
   LANG_OUT_TURKISH,
   LANG_OUT_SERBO_CROATIAN_CYRILLIC,
   LANG_OUT_BULGARIAN,
   LANG_OUT_RUSSIAN,
} LANGUAGE_RESULT;

typedef enum
{
   COLOR_NONEGR,
   COLOR_BW,
   COLOR_GRAY,
   COLOR_TRUECOLOR,
   COLOR_ORIGINAL
} COLOR_RESULT;

typedef enum
{
   LINE_HALF,
   LINT_ONE,
   LINE_ONEANDHALF,
   LINE_DOUBLE,
} LINESPACING;

typedef enum
{
   ALIGNMENT_LEFTALIGNMENT = 0,
   ALIGNMENT_RIGHTALIGNMENT,
   ALIGNMENT_CENTERED,
   ALIGNMENT_JUSTIFIED
} ALIGNMENT;

typedef enum
{
   OUT_TYPE_PLAIN,
   OUT_TYPE_WORD_PROCESSOR,
   OUT_TYPE_TRUE_WORD_PROCESSOR,
   OUT_TYPE_TABLE,
   OUT_TYPE_UNKNOWN,
} FORMAT_OUTPUTTYPE;

typedef enum
{
   PROC_LOADING_IMG = 0,
   PROC_SAVING_IMG,
   PROC_FIND_ZONES,
   PROC_RECOGNIZE_MOR,
   PROC_RECOGNITION,
   PROC_SPELLING,
   PROC_SAVE_RECOG_RESULT,
   PROC_WRITE_OUTPUT_DOC,
   PROC_WRITE_OUTPUT_IMG,
   PROC_RECOGNITION3,
   PROC_PROCESSING_IMG,
} OCRPROCID;

typedef enum
{
   FONT_PROPORTSERIF = 1,
   FONT_PROPORTSANSSERIF,
   FONT_MONOSERIF,
   FONT_MONOSANSSERIF,
   FONT_PREDEFFONT,
} FONTDEFINE;

typedef enum
{
   VERIFY_IMPOSSIBLE = 0,
   VERIFY_UNLIKELY,
   VERIFY_UNRESOLVED,
   VERIFY_POSSIBLE,
   VERIFY_ACCEPT,
} VERIFYCODE;

typedef enum
{
   DOC_TEXT_STANDARD = 0,
   DOC_TEXT_SMART,
   DOC_TEXT_STRIPPED,
   DOC_TEXT_PLAIN,
   DOC_TEXT_COMMA_DELIMITED,
   DOC_TEXT_TAB_DELIMITED,
   DOC_REC_ASCII_FORMATTED,
   DOC_REC_ASCII_STANDARD,
   DOC_REC_ASCII_STANDARDEX,
   DOC_GENERAL_WORD_PROCESSOR,
   DOC_PDF,
   DOC_PDF_IMAGE_SUBSTITUTES,
   DOC_PDF_IMAGE_ON_TEXT,
   DOC_PDF_IMAGEONLY,
   DOC_PDF_EDITED,
   DOC_HTML_3_2,
   DOC_HTML_4_0,
   DOC_WORD_97_2000_XP,
   DOC_EXCEL_97_2000,
   DOC_WORDPERFECT_8,
   DOC_RTF,
   DOC_PPT_97_RTF,
   DOC_PUB_98_RTF,
   DOC_WORDPAD_RTF,
   DOC_RTF_WORD_2000,
   DOC_RTF_WORD_97,
   DOC_RTF_WORD_6_95,
   DOC_OPEN_EBOOK_1_0,
   DOC_XML,
   DOC_2G_TYPE_2,
   DOC_2G_TYPE_3,
   DOC_WORDPERFECT_9_10,
   DOC_MICROSOFT_READER,
   DOC_MICROSOFT_WORD_2003,
   DOC_REC_PDF_IMAGE_ON_TEXT,
   DOC_PDFA_IMAGE_ON_TEXT,
   DOC_PDFA_TEXT_ONLY,
} FORMAT_TYPE;

typedef HANDLE L_HDOC;

#define MAX_UD_ITEM_LENGTH       64
#define MAX_SECTION_NAME_LENGTH  17
#define MAX_SEPARATOR_SIZE       256
#define MAX_WORD_SIZE            1000

/****************************************************************
   Callback typedefs
****************************************************************/
typedef L_INT (pEXT_CALLBACK VERIFICATIONCALLBACKA)(
                              L_INT nZoneIndex,
                              L_CHAR * pszWord,
                              VERIFYCODE * pVerify,
                              L_VOID * pUserData);
#if defined(FOR_UNICODE)
typedef L_INT (pEXT_CALLBACK VERIFICATIONCALLBACK)(
                              L_INT nZoneIndex,
                              L_TCHAR * pszWord,
                              VERIFYCODE * pVerify,
                              L_VOID * pUserData);
#else
typedef VERIFICATIONCALLBACKA VERIFICATIONCALLBACK;
#endif // #if defined(FOR_UNICODE)

/****************************************************************
   Classes/structures
****************************************************************/

typedef struct _tagProgressData
{
   L_UINT      uStructSize;
   OCRPROCID   Id;
   L_INT       nPercent;
   L_INT       nAccecptCharCount;
   L_INT       nRejectedCharCount;
   L_INT32     lObjSize;
   L_UCHAR *   uObjData;
} PROGRESSDATA, * pPROGRESSDATA;

typedef struct _tagZoneDataA
{
   L_UINT               uStructSize;
   RECT                 rcArea;
   L_INT                Id;
   FILLMETHOD           FillMethod;
   RECOGMODULE          RecogModule;
   CHAR_FILTER          CharFilter;
   ZONETYPE             Type;
   L_UINT               uFlags;
   L_CHAR               szSection[MAX_SECTION_NAME_LENGTH];
   VERIFICATIONCALLBACKA pfnCallback;
   L_VOID             * pUserData;
} ZONEDATAA, * pZONEDATAA;
#if defined(FOR_UNICODE)
typedef struct _tagZoneData
{
   L_UINT               uStructSize;
   RECT                 rcArea;
   L_INT                Id;
   FILLMETHOD           FillMethod;
   RECOGMODULE          RecogModule;
   CHAR_FILTER          CharFilter;
   ZONETYPE             Type;
   L_UINT               uFlags;
   L_CHAR               szSection[MAX_SECTION_NAME_LENGTH];
   VERIFICATIONCALLBACK pfnCallback;
   L_VOID             * pUserData;
} ZONEDATA, * pZONEDATA;
#else
typedef ZONEDATAA ZONEDATA;
typedef pZONEDATAA pZONEDATA;
#endif // #if defined(FOR_UNICODE)

typedef struct _tagAutoZoneOpts
{
   L_UINT      uStructSize;
   PAGEPARSER  Parser;
   L_BOOL      bEnableForceSingleColumn;
   L_BOOL      bVisibleGridLines;
} AUTOZONEOPTS, * pAUTOZONEOPTS;

typedef struct _tagCharOptions
{
   L_UINT         uStructSize;
   L_WCHAR      * pszCharPlus;
   L_UINT         uCharPlusSize;
   L_WCHAR      * pszCharFilterPlus;
   L_UINT         uCharFilterPlusSize;
   CHAR_FILTER    CharFilter;
} CHAROPTIONS, * pCHAROPTIONS;

typedef struct _tagMOROptions
{
   L_UINT   uStructSize;
   L_BOOL   bEnableFax;
} MOROPTIONS, * pMOROPTIONS;

typedef struct _tagHandPrintOptions
{
   L_UINT            uStructSize;
   HANDRECOG_STYLE   style;
   L_BOOL            bSpace;
   L_INT             nCharHeight;
   L_INT             nCharWidth;
   L_INT             nCharSpace;
} HANDPRINTOPTIONS, * pHANDPRINTOPTIONS;

#if !defined(LEADTOOLS_V16_OR_LATER)
typedef struct _tagOMROptions
{
   L_UINT   uStructSize;
   L_BOOL   bFill;
   OMRFRAME Frame;
   OMRSENSE Sense;
} OMROPTIONS, * pOMROPTIONS;
#else
typedef struct _tagOMROptions
{
   L_UINT   uStructSize;
   L_BOOL   bFill; /* Deprecated, do not use */
   OMRFRAME Frame;
   OMRSENSE Sense;
   L_WCHAR  FilledRecognitionChar;
   L_WCHAR  UnFilledRecognitionChar;
} OMROPTIONS, * pOMROPTIONS;
#endif // #if !defined(LEADTOOLS_V16_OR_LATER)

typedef struct _tagUserDictionaryA
{
   L_UINT          uStructSize;
   L_CHAR        * pszFileName;
   L_CHAR        * pszDefSection;
} USERDICTIONARYA, * pUSERDICTIONARYA;

#if defined(FOR_UNICODE)
typedef struct _tagUserDictionary
{
   L_UINT          uStructSize;
   L_TCHAR       * pszFileName;
   L_CHAR        * pszDefSection;
} USERDICTIONARY, * pUSERDICTIONARY;
#else
typedef USERDICTIONARYA USERDICTIONARY;
typedef pUSERDICTIONARYA pUSERDICTIONARY;
#endif // #if defined(FOR_UNICODE)
typedef struct _tagStatus
{
   L_UINT   uStructSize;
   L_INT    nRecogChrCount;
   L_INT    nRecogWordCount;
   L_INT    nRejectChrCount;
   L_INT    nRejectWordCount;
   L_INT32  lRecogTime;
   L_INT32  lReadingTime;
   L_INT32  lPreprocTime;
   L_INT32  lDecompTime;
} STATUS, *pSTATUS;

typedef struct _tagDocumentOptions
{
   L_UINT            uStructSize;
   SELECTOR          PaperSize;
   PAPERTYPE         PaperType;
   PAPERORIENTATION  PaperOrientation;
   SELECTOR          Language;
   LANGUAGE_RESULT   ResLang;
   COLOR_RESULT      Color;
   L_BOOL            bEnableTextInBoxes;
   SELECTOR          Margins;
   RECT              rcMargin;
   L_BOOL            bEnablePageBreaks;
   TABLEMETHOD       TableMethod;
} DOCUMENTOPTIONS, * pDOCUMENTOPTIONS;

typedef struct _tagParagraphOptions
{
   L_UINT      uStructSize;
   SELECTOR    SelSpaceBefore;
   L_INT       nSpaceBefore;
   SELECTOR    SelParaIndent;
   L_INT       nFirstLineIndent;
   SELECTOR    SelLineSpacing;
   LINESPACING LineSpacing;
   SELECTOR    SelAlignment;
   ALIGNMENT   Alignment;
} PARAGRAPHOPTIONS, * pPARAGRAPHOPTIONS;

typedef struct _tagCharacterOptions
{
   L_UINT      uStructSize;
   SELECTOR    SelOutputFont;
   FONTDEFINE  FontNames[5];
   L_INT       nSize;
   L_BOOL      bBold;
   L_BOOL      bItalic;
   L_BOOL      bUnderline;
} CHARACTEROPTIONS, * pCHARACTEROPTIONS;

typedef struct _tagMarkOptions
{
   L_UINT   uStructSize;
   L_UINT   uColSuspCharacter;
   L_UINT   uColRejectionSymbol;
   L_UINT   uColMissingSymbol;
   L_UINT   uColSuspWord;
   L_WCHAR  szSepBeforeSuspCharacter[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepBeforeRejectionSymbol[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepBeforeMissingSymbol[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepStartofSuspWord[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepEndofSuspWord[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepStartofLine[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepEndofLine[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepStartofTableRow[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepEndofTableRow[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepStartofPara[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepEndofPara[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepStartofZone[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepEndofZone[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepStartofPage[MAX_SEPARATOR_SIZE];
   L_WCHAR  szSepEndofPage[MAX_SEPARATOR_SIZE];
} MARKOPTIONS, *pMARKOPTIONS;

typedef struct _tagResultOptions
{
   L_UINT            uStructSize;
   FORMAT_TYPE       Format;
   FORMATLEVEL       FormatLevel;
   DOCUMENTOPTIONS   DocOptions;
   PARAGRAPHOPTIONS  ParagOptions;
   CHARACTEROPTIONS  CharOptions;
   MARKOPTIONS       MarkOptions;
#if defined(LEADTOOLS_V16_OR_LATER)
   DOCWRTFORMAT      DocFormat;
#endif // #if defined(LEADTOOLS_V16_OR_LATER)
} RESULTOPTIONS, * pRESULTOPTIONS;

typedef struct _tagTextFormatInfoA
{
   L_UINT               uStructSize;
   L_CHAR *             pszName;
   L_CHAR *             pszDLLName;
   L_CHAR *             pszExtName;
   FORMAT_OUTPUTTYPE    Type;
} TEXTFORMATINFOA, * pTEXTFORMATINFOA;

#if defined(FOR_UNICODE)
typedef struct _tagTextFormatInfo
{
   L_UINT               uStructSize;
   L_TCHAR *            pszName;
   L_TCHAR *            pszDLLName;
   L_TCHAR *            pszExtName;
   FORMAT_OUTPUTTYPE    Type;
} TEXTFORMATINFO, * pTEXTFORMATINFO;
#else
typedef TEXTFORMATINFOA TEXTFORMATINFO;
typedef pTEXTFORMATINFOA pTEXTFORMATINFO;
#endif// #if defined(FOR_UNICODE)

typedef struct _tagSpecialChar
{
   L_UINT   uStructSize;
   L_WCHAR  chReject;
   L_WCHAR  chMissSym;
} SPECIALCHAR, * pSPECIALCHAR;

typedef struct _tagPageInfo
{
   L_UINT   uStructSize;
   L_INT    nWidth;
   L_INT    nHeight;
   L_INT    nBitsPerPixel;
} PAGEINFO, * pPAGEINFO;

typedef struct _tagRecognizeOptsA
{
   L_UINT   uStructSize;
   L_INT    nPageIndexStart;
   L_INT    nPagesCount;
   L_BOOL   bEnableSubSystem;
   L_BOOL   bEnableCorrection;
   LANGIDS  SpellLangId;
   L_CHAR  *pszFileName;
} RECOGNIZEOPTSA, *pRECOGNIZEOPTSA;

#if defined(FOR_UNICODE)
typedef struct _tagRecognizeOpts
{
   L_UINT   uStructSize;
   L_INT    nPageIndexStart;
   L_INT    nPagesCount;
   L_BOOL   bEnableSubSystem;
   L_BOOL   bEnableCorrection;
   LANGIDS  SpellLangId;
   L_TCHAR *pszFileName;
} RECOGNIZEOPTS, *pRECOGNIZEOPTS;
#else
typedef RECOGNIZEOPTSA RECOGNIZEOPTS;
typedef pRECOGNIZEOPTSA pRECOGNIZEOPTS;
#endif // #if defined(FOR_UNICODE)

typedef struct _tagRecogChars
{
   L_UINT   uStructSize;
   RECT     rcArea;
   L_INT    nYOffset;
   L_INT    nSpace;
   L_INT    nSpaceErr;
   L_WCHAR  wGuessCode;
   L_WCHAR  wGuessCode2;
   L_WCHAR  wGuessCode3;
   L_INT    nZoneIndex;
   L_INT    nCellIndex;
   L_INT    nConfidence;
   L_UINT   uFont;
   L_INT    nFontSize;
   L_INT    nCharFormat;
   LANGIDS  Lang;
   LANGIDS  Lang2;
} RECOGCHARS, * pRECOGCHARS;

typedef struct _tagRecogWords
{
   L_UINT   uStructSize;
   L_WCHAR  szWord[MAX_WORD_SIZE];
   RECT     rcWordArea;
   L_INT    nZoneIndex;
} RECOGWORDS, * pRECOGWORDS;

/****************************************************************
   Callback typedefs
****************************************************************/

typedef L_BOOL (pEXT_CALLBACK PROGRESSCALLBACK)(
                              pPROGRESSDATA pProgressData,
                              L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK ENUMOUTPUTFILEFORMATS)(
                              FORMAT_TYPE Format,
                              L_VOID * pUserData);

typedef L_INT (pEXT_CALLBACK RECOGNIZESTATUSCALLBACK)(
                              L_INT nRecogPage,
                              L_INT nError,
                              L_VOID * pUserData);

/****************************************************************
   Function prototypes
****************************************************************/

#if defined(LEADTOOLS_V16_OR_LATER)
L_LTDOC_API L_INT EXT_FUNCTION L_DocStartUpA(L_HDOC * phDoc, L_CHAR * pszEnginePath, L_BOOL bUseThunk);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocStartUp(L_HDOC * phDoc, L_TCHAR * pszEnginePath, L_BOOL bUseThunk);
#else
#define L_DocStartUp L_DocStartUpA
#endif // #if defined(FOR_UNICODE)
#else
L_LTDOC_API L_INT EXT_FUNCTION L_DocStartUpA(L_HDOC * phDoc, L_CHAR * pszEnginePath);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocStartUp(L_HDOC * phDoc, L_TCHAR * pszEnginePath);
#else
#define L_DocStartUp L_DocStartUpA
#endif // #if defined(FOR_UNICODE)
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

L_LTDOC_API L_INT EXT_FUNCTION L_DocShutDown(L_HDOC * phDoc);

L_LTDOC_API L_INT EXT_FUNCTION L_DocLoadSettingsA(L_HDOC hDoc, L_CHAR * pszFileName);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSaveSettingsA(L_HDOC hDoc, L_CHAR * pszFileName);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocLoadSettings(L_HDOC hDoc, L_TCHAR * pszFileName);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSaveSettings(L_HDOC hDoc, L_TCHAR * pszFileName);
#else
#define L_DocLoadSettings L_DocLoadSettingsA
#define L_DocSaveSettings L_DocSaveSettingsA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetProgressCB(L_HDOC hDoc, PROGRESSCALLBACK pfnCallback, L_VOID * pUserData);

L_LTDOC_API L_INT EXT_FUNCTION L_DocAddPage(L_HDOC hDoc, pBITMAPHANDLE pBitmap, L_INT nPageIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetPageCount(L_HDOC hDoc, L_INT * pnPageCount);

L_LTDOC_API L_INT EXT_FUNCTION L_DocUpdatePage(L_HDOC hDoc, pBITMAPHANDLE pBitmap, L_INT nPageIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocRemovePage(L_HDOC hDoc, L_INT nPageIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocExportPage(L_HDOC hDoc, pBITMAPHANDLE pBitmap, L_UINT uStructSize, L_INT nPageIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetPageInfo(L_HDOC hDoc, L_INT nPageIndex, pPAGEINFO pPageInfo, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocAddZoneA(L_HDOC hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATAA pZoneData);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocAddZone(L_HDOC hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATA pZoneData);
#else
#define L_DocAddZone L_DocAddZoneA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetZoneCount(L_HDOC hDoc, L_INT nPageIndex, L_INT * pnCount);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetZoneA(L_HDOC hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATAA pZoneData, L_UINT uStructSize);
L_LTDOC_API L_INT EXT_FUNCTION L_DocUpdateZoneA(L_HDOC hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATAA pZoneData);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocGetZone(L_HDOC hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATA pZoneData, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocUpdateZone(L_HDOC hDoc, L_INT nPageIndex, L_INT nZoneIndex, pZONEDATA pZoneData);
#else
#define L_DocGetZone L_DocGetZoneA
#define L_DocUpdateZone L_DocUpdateZoneA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_INT EXT_FUNCTION L_DocRemoveZone(L_HDOC hDoc, L_INT nPageIndex, L_INT nZoneIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocImportZonesA(L_HDOC hDoc, L_INT nPageIndex, L_CHAR * pszFileName);

L_LTDOC_API L_INT EXT_FUNCTION L_DocExportZonesA(L_HDOC hDoc, L_INT nPageIndex, L_CHAR * pszFileName);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocImportZones(L_HDOC hDoc, L_INT nPageIndex, L_TCHAR * pszFileName);

L_LTDOC_API L_INT EXT_FUNCTION L_DocExportZones(L_HDOC hDoc, L_INT nPageIndex, L_TCHAR * pszFileName);
#else
#define L_DocImportZones L_DocImportZonesA
#define L_DocExportZones L_DocExportZonesA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_INT EXT_FUNCTION L_DocFindZones(L_HDOC hDoc, L_INT nPageIndex, LPRECT pRect);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetZoneOptions(L_HDOC hDoc, pAUTOZONEOPTS pZoneOpts);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetZoneOptions(L_HDOC hDoc, pAUTOZONEOPTS pZoneOpts, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetFillMethod(L_HDOC hDoc, FILLMETHOD FillMethod);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetFillMethod(L_HDOC hDoc, FILLMETHOD * pFillMethod);

L_LTDOC_API L_INT EXT_FUNCTION L_DocFindDefaultFillMethod(L_HDOC hDoc, L_INT nPageIndex, FILLMETHOD * pFillMethod);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSelectLanguages(L_HDOC hDoc, LANGIDS * pLangIds, L_INT nLangCount);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetSelectedLanguages(L_HDOC hDoc, LANGIDS * * ppLangIds, L_INT * pnLangCount);

L_LTDOC_API L_INT EXT_FUNCTION L_DocFreeLanguages(L_HDOC hDoc, LANGIDS * * ppLangIds);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetCharLangsOptions(L_HDOC hDoc, pCHAROPTIONS pCharOpts);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetCharLangsOptions(L_HDOC hDoc, pCHAROPTIONS pCharOpts, L_UINT uStructSize);

L_LTDOC_API L_BOOL EXT_FUNCTION L_DocIsCharEnabled(L_HDOC hDoc, L_WCHAR ch);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetDefaultSpellLanguages(L_HDOC hDoc, LANGIDS * * ppLangIds, L_INT *pnLangCount);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetRecognizeModuleTradeOff(L_HDOC hDoc, RECOGMODULE_TRADEOFF TradeOff);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetRecognizeModuleTradeOff(L_HDOC hDoc, RECOGMODULE_TRADEOFF *pTradeOff);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetMOROptions(L_HDOC hDoc, pMOROPTIONS pMOROpts);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetMOROptions(L_HDOC hDoc, pMOROPTIONS pMOROpts, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetHandPrintOptions(L_HDOC hDoc, pHANDPRINTOPTIONS pHandOpts);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetHandPrintOptions(L_HDOC hDoc, pHANDPRINTOPTIONS pHandOpts, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetOMROptions(L_HDOC hDoc, pOMROPTIONS pOMROpts);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetOMROptions(L_HDOC hDoc, pOMROPTIONS pOMROpts, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetUserDictionaryA(L_HDOC hDoc, pUSERDICTIONARYA pUDOpts, L_BOOL bCreateUD);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetUserDictionaryA(L_HDOC hDoc, pUSERDICTIONARYA pUDOpts, L_UINT uStructSize);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocSetUserDictionary(L_HDOC hDoc, pUSERDICTIONARY pUDOpts, L_BOOL bCreateUD);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetUserDictionary(L_HDOC hDoc, pUSERDICTIONARY pUDOpts, L_UINT uStructSize);
#else
#define L_DocSetUserDictionary L_DocSetUserDictionaryA
#define L_DocGetUserDictionary L_DocGetUserDictionaryA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_BOOL EXT_FUNCTION L_DocGetUserDictionaryState(L_HDOC hDoc);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetUserDictionarySection(L_HDOC hDoc, L_CHAR * pSection, L_INT nSize, L_BOOL bFirst);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetUserDictionarySectionItem(L_HDOC hDoc, L_CHAR * pSection, L_WCHAR * pItem, L_INT nSize, L_UINT32 * puMask, L_BOOL bFirst);

L_LTDOC_API L_INT EXT_FUNCTION L_DocAddItemToUserDictionary(L_HDOC hDoc, L_CHAR * pSection, L_WCHAR * pUDitem, L_UINT32 uMask);

L_LTDOC_API L_INT EXT_FUNCTION L_DocRemoveItemFromUserDictionary(L_HDOC hDoc, L_CHAR * pSection, L_WCHAR * pUDitem, L_UINT32 uMask);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetStatus(L_HDOC hDoc, pSTATUS pStatus, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocRecognizeA(L_HDOC hDoc, pRECOGNIZEOPTSA pRecogOpts, RECOGNIZESTATUSCALLBACK pfnCallback, L_VOID * pUserData);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocRecognize(L_HDOC hDoc, pRECOGNIZEOPTS pRecogOpts, RECOGNIZESTATUSCALLBACK pfnCallback, L_VOID * pUserData);
#else
#define L_DocRecognize L_DocRecognizeA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_INT EXT_FUNCTION L_DocSaveResultsToMemory(L_HDOC hDoc, L_UCHAR **ppBuffer, L_INT32 * plSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocFreeMemoryResults(L_HDOC hDoc, L_UCHAR **ppBuffer);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetRecognitionResultOptions(L_HDOC hDoc, pRESULTOPTIONS pResOpts);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetRecognitionResultOptions(L_HDOC hDoc, pRESULTOPTIONS pResOpts, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocEnumOutputFileFormats(L_HDOC hDoc, ENUMOUTPUTFILEFORMATS pfnCallback, L_VOID * pUserData);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetTextFormatInfoA(L_HDOC hDoc, FORMAT_TYPE Format, pTEXTFORMATINFOA pFormatInfo, L_UINT uStructSize);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocGetTextFormatInfo(L_HDOC hDoc, FORMAT_TYPE Format, pTEXTFORMATINFO pFormatInfo, L_UINT uStructSize);
#else
#define L_DocGetTextFormatInfo L_DocGetTextFormatInfoA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_INT EXT_FUNCTION L_DocSaveResultsToFileA(L_HDOC hDoc, L_CHAR * pszFileName);

#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocSaveResultsToFile(L_HDOC hDoc, L_TCHAR * pszFileName);
#else
#define L_DocSaveResultsToFile L_DocSaveResultsToFileA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetSpecialChar(L_HDOC hDoc, pSPECIALCHAR pSpecialChar);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetSpecialChar(L_HDOC hDoc, pSPECIALCHAR pSpecialChar, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocLockPage(L_HDOC hDoc, L_INT nPageIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocUnlockPage(L_HDOC hDoc, L_INT nPageIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetActivePage(L_HDOC hDoc, L_INT nPageIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocDrawPage(L_HDOC hDoc, HDC hDC, L_INT nPageIndex, LPRECT pSrc, LPRECT pClipSrc, LPRECT pDst, LPRECT pClipDst, L_UINT32 uROP3, L_BOOL bShowZones);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSelectZoneByPoint(L_HDOC hDoc, HDC hDC, L_INT nPageIndex, POINT pt, L_INT * pnZoneIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSelectZone(L_HDOC hDoc, HDC hDC, L_INT nPageIndex, L_INT nZoneIndex, L_BOOL bSelect);

L_LTDOC_API L_INT EXT_FUNCTION L_DocRemoveSelectedZone(L_HDOC hDoc, L_INT nPageIndex);

L_LTDOC_API HPEN  EXT_FUNCTION L_DocSetZonePen(L_HDOC hDoc, HPEN hPen);

L_LTDOC_API HPEN  EXT_FUNCTION L_DocSetSelectedZonePen(L_HDOC hDoc, HPEN hPen);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetSelectedZoneA(L_HDOC hDoc, L_INT nPageIndex, pZONEDATAA pZoneData, L_UINT uStructSize);
#if defined(FOR_UNICODE)
L_LTDOC_API L_INT EXT_FUNCTION L_DocGetSelectedZone(L_HDOC hDoc, L_INT nPageIndex, pZONEDATA pZoneData, L_UINT uStructSize);
#else
#define L_DocGetSelectedZone L_DocGetSelectedZoneA
#endif // #if defined(FOR_UNICODE)

L_LTDOC_API L_INT EXT_FUNCTION L_DocZone(L_HDOC hDoc, L_INT nPageIndex, L_INT nZoneIndex, LPRECT lpArea);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetRecognizedCharacters(L_HDOC hDoc, L_INT nPageIndex, pRECOGCHARS * ppRecogChars, L_INT32 * plCharsCount, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetRecognizedCharacters(L_HDOC hDoc, L_INT nPageIndex, pRECOGCHARS pRecogChars, L_INT32 lCharsCount);

L_LTDOC_API L_INT EXT_FUNCTION L_DocFreeRecognizedCharacters(L_HDOC hDoc, pRECOGCHARS * ppRecogChars);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetRecognizedWords(L_HDOC hDoc, L_INT nPageIndex, pRECOGWORDS * ppRecogWords, L_UINT uStructSize, L_INT * pnWordsCount);

L_LTDOC_API L_INT EXT_FUNCTION L_DocFreeRecognizedWords(L_HDOC hDoc, pRECOGWORDS * ppRecogWords);

L_LTDOC_API L_INT EXT_FUNCTION L_DocOffsetZones(L_HDOC hDoc, POINT pt);

L_LTDOC_API L_INT EXT_FUNCTION L_DocAutoOrientPage(L_HDOC hDoc, L_INT nPageIndex);

L_LTDOC_API L_INT EXT_FUNCTION L_DocDetectOrientationDegree(L_HDOC hDoc, L_INT nPageIndex, L_INT * pnRotate);

L_LTDOC_API L_BOOL EXT_FUNCTION L_DocIsParallelRecognitionEnabled(L_HDOC hDoc);

L_LTDOC_API L_INT EXT_FUNCTION L_DocEnableParallelRecognition(L_HDOC hDoc, L_BOOL bEnable);

L_LTDOC_API L_INT EXT_FUNCTION L_DocSetPaintZoomFactor(L_HDOC hDoc, L_INT nPageIndex, L_FLOAT fZoomFactor);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetPaintZoomFactor(L_HDOC hDoc, L_INT nPageIndex, L_FLOAT * pfZoomFactor);

L_LTDOC_API L_INT EXT_FUNCTION L_DocCleanupPages(L_HDOC hDoc, L_BOOL bAutoCleanup);

#if defined(LEADTOOLS_V16_OR_LATER)
L_LTDOC_API L_INT EXT_FUNCTION L_DocSetDocumentWriterOptions(L_HDOC hDoc, L_VOID * pOptions, L_UINT uStructSize);

L_LTDOC_API L_INT EXT_FUNCTION L_DocGetDocumentWriterOptions(L_HDOC hDoc, L_VOID * pOptions, L_UINT uStructSize);
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

#undef L_HEADER_ENTRY
#include "Ltpck.h"

#endif // #if !defined(LTDOC_H)
