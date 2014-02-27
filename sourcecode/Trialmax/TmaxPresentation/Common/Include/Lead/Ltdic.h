/*************************************************************
   Ltdic.h - LEADTOOLS dicom library
   Copyright (c) 1991-2009 LEAD Technologies, Inc.
   All Rights Reserved.
*************************************************************/

#if !defined(LTDIC_H)
#define LTDIC_H

#if defined(__cplusplus)
   #if defined(_LEAD_DICOM_NO_EXP_IMP)
      #define  DICOM_EXPORT
   #else
      #if defined(_LEAD_DICOM_)
         #define  DICOM_EXPORT __declspec(dllexport)
      #else
         #define  DICOM_EXPORT __declspec(dllimport)
      #endif   
   #endif // #if defined(_LEAD_DICOM_NO_EXP_IMP)
#endif // #if defined(__cplusplus)

#if !defined(L_LTDIC_API)
   #define L_LTDIC_API DICOM_EXPORT
#endif // #if !defined(L_LTDIC_API)

#if !defined(L_LTDIC_CLASS)
   #define L_LTDIC_CLASS DICOM_EXPORT
#endif // #if !defined(L_LTDIC_CLASS)

#include "Ltfil.h"
#if !defined(FOR_WINCE)
#include "Ltann.h"
#endif // #if !defined(FOR_WINCE)

#define pWRPEXT_CALLBACK pEXT_FUNCTION

#include "ltdicErrors.h"

#if defined(LEADTOOLS_V16_OR_LATER)
#pragma pack(8)
#endif // #if defined(LEADTOOLS_V16_OR_LATER)


//For extended debug functionality
#define DEBUG_MODE_ONSEND_SHOW_EXTENDED_INFO    0x00000001
#define DEBUG_MODE_ONRECEIVE_SHOW_EXTENDED_INFO 0x00000002


// macro which checks the structure size without checking for NULL pointer
#define DICOM_CHECK_SIZE(PSTRUCTPTR) if((PSTRUCTPTR)->uStructSize != sizeof(*PSTRUCTPTR)) return DICOM_ERROR_INVALID_STRUCT_SIZE
#define DICOM_CHECK_SIZE2(PSTRUCTPTR, USTRUCTSIZE) if(USTRUCTSIZE != sizeof(*PSTRUCTPTR)) return DICOM_ERROR_INVALID_STRUCT_SIZE
// macro which checks the size of the structure. If the pointer is NULL, it returns DICOM_ERROR_NULL_PTR
#define DICOM_SAFE_CHECK_SIZE(PSTRUCTPTR) \
   if(!PSTRUCTPTR)                  \
      return DICOM_ERROR_NULL_PTR;        \
   CHECK_SIZE(PSTRUCTPTR)
#define DICOM_SAFE_CHECK_SIZE_TYPE(PSTRUCTPTR, retType) \
   if(!PSTRUCTPTR)                  \
   return (retType)DICOM_ERROR_NULL_PTR;    \
   CHECK_SIZE_TYPE(PSTRUCTPTR, retType)

// Added for 64-bit types - or 8 bytes values, used in encryption, as keys etc
// and for TLS primitive data
typedef const L_CHAR* L_PCSTR;
typedef const L_TCHAR* L_PCTSTR;
typedef unsigned __int64 L_UINT64;
typedef __int64 L_INT64;
struct ssl_st;
typedef struct ssl_st SSL;

struct ssl_ctx_st;
typedef struct ssl_ctx_st SSL_CTX;
typedef SSL_CTX L_SSL_CTX;

#define L_SSL_OP_NO_SSLv2              0x01000000L
#define L_SSL_OP_NO_SSLv3              0x02000000L
#define L_SSL_OP_NO_TLSv1              0x04000000L
#define L_SSL_OP_ALL                   0x000FFFFFL

// flags
#define FLAG_SSL_CTX_PRECREATE_METHOD_TYPE   0x001
#define FLAG_SSL_CTX_PRECREATE_ALL           0x001

#define L_SSL_VERIFY_NONE                 0x00
#define L_SSL_VERIFY_PEER                 0x01
#define L_SSL_VERIFY_FAIL_IF_NO_PEER_CERT 0x02
#define L_SSL_VERIFY_CLIENT_ONCE          0x04
#define L_SSL_VERIFY_ALL                  0x07

#define FLAG_SSL_CTX_CREATE_METHOD_TYPE      0x001
#define FLAG_SSL_CTX_CREATE_VERIFY_DEPTH     0x002
#define FLAG_SSL_CTX_CREATE_VERIFY_MODE      0x004
#define FLAG_SSL_CTX_CREATE_CAFILE           0x008
#define FLAG_SSL_CTX_CREATE_OPTIONS          0x010
#define FLAG_SSL_CTX_CREATE_ALL              0x01F

typedef struct tagSSL_CTX_CREATE
{
   L_UINT        uStructSize;
   L_UINT32      uFlags;
   L_INT         nMethodTypeSSL;  //TYPE_SSLV2_METHOD, TYPE_SSLV3_METHOD, TYPE_TLSV1_METHOD, TYPE_SSLV23_METHOD
   L_TCHAR       *pszCAfile;
   L_UINT        uVerifyMode;
   L_INT         nVerifyDepth;
   L_INT         nOptions;        //L_SSL_OP_NO_SSLv2, L_SSL_OP_NO_SSLv3, L_SSL_OP_NO_TLSv1
   L_INT         nSuccess;        // DICOM_SUCCESS, or reason for failure
   L_INT         nReserved1;
   L_INT         nReserved2;
} L_SSL_CTX_CREATE, *pL_SSL_CTX_CREATE;


struct x509_store_st;
typedef struct x509_store_ctx_st X509_STORE_CTX;
typedef int (*TLS_CERT_VERIFY_CALLBACK)(int ok,X509_STORE_CTX *ctx);

// end of added for 64bit types and TLS primitives


// added for secure extension

#define DICOM_SECURE_NONE  0xABCD0000
#define DICOM_SECURE_ISCL  0xABCD0001
#define DICOM_SECURE_TLS   0xABCD0002

// end of added for secure extension

// added for TLS complance

enum _L_CIPHERSUITE
{
   TLS_DHE_RSA_WITH_DES_CBC_SHA = 0x12,
   TLS_DHE_RSA_WITH_3DES_EDE_CBC_SHA = 0x13,  // mandatory standard
};

typedef enum _L_CIPHERSUITE L_CIPHERSUITE;

enum _L_ENCRYPTION_METHOD
{
   L_CRYPT_DES   = 0x01,
   L_CRYPT_3DES  = 0x02
};

typedef enum _L_ENCRYPTION_METHOD L_ENCRYPTION_METHOD;

enum _L_MAC_METHOD
{
   L_MAC_SHA1  = 0x10
};

typedef enum _L_MAC_METHOD L_MAC_METHOD;

enum _L_MUTUAL_AUTH_METHOD
{
   L_MUTUALAUTH_RSA = 0x20
};

typedef enum _L_MUTUAL_AUTH_METHOD L_MUTUAL_AUTH_METHOD;

enum _L_KEY_EXCHANGE_METHOD
{
   L_KEYEXCHANGE_RSA_SIGNED_DHE = 0x40
};

typedef enum _L_KEY_EXCHANGE_METHOD L_KEY_EXCHANGE_METHOD;

enum _SSL_METHOD_TYPE 
{
   TYPE_SSLV2_METHOD    = 0x01, 
   TYPE_SSLV3_METHOD    = 0x02, 
   TYPE_TLSV1_METHOD    = 0x03, 
   TYPE_SSLV23_METHOD   = 0x04,
};

typedef enum _SSL_METHOD_TYPE SSL_METHOD_TYPE;




#if !defined(FOR_BORLAND)
typedef enum _DICOM_TLS_ERRORS DICOM_TLS_ERRORS;
#endif // #if !defined(FOR_BORLAND)

#define L_TLS_FILETYPE_PEM    1
#define L_TLS_FILETYPE_ASN1   2

// use either SSL_VERIFY_NONE or SSL_VERIFY_PEER, the last 2 options
// are 'ored' with SSL_VERIFY_PEER if they are desired
#define L_TLS_VERIFY_NONE        0x00
#define L_TLS_VERIFY_PEER        0x01
#define L_TLS_VERIFY_FAIL_IF_NO_PEER_CERT 0x02
#define L_TLS_VERIFY_CLIENT_ONCE    0x04

extern L_INT nVerifyCertDepth;
extern L_INT nVerifyCertError;
// end of added for TLS


// added for ISCL compliance 

#define DC_MSG_IN_QUEUE    0xDC00

#define DESCBC_CHAIN_IVEC_OVER_SIGNATURE  1
#define DESCBC_CHAIN_IVEC_OVER_MESSAGES   1


// Magical constants:


#define DICOM_ISCL_MAX_PACKET_SIZE        (8160)     // set with manual values
#define DICOM_ISCL_MAX_MESSAGE_SIZE       (1048576)   // set with manual values


      // values specified in ISCL standard
#define DICOM_ISCL_ACK                    0x00000000
#define DICOM_ISCL_NAK                    0xFFFFFFFF

#define DICOM_ISCL_MUTUAL_AUTH_3P4W       0x00000000  // three pass four way mutual auth

#define DICOM_ISCL_ENCRYPT_NONE           0x00000000
#define DICOM_ISCL_ENCRYPT_DESCBC         0x00001212

#define DICOM_ISCL_MAC_NONE               0x00000000
#define DICOM_ISCL_MAC_MD5                0x00001441
#define DICOM_ISCL_MAC_DESMAC             0x00004001

#define DICOM_ISCL_LINE_CONNECTION_CHECK_RQ     0x00110001  // RQ - request, RP - response
#define DICOM_ISCL_LINE_CONNECTION_CHECK_RP     0x00110003
#define DICOM_ISCL_MUTUAL_AUTH_RQ               0x00120001
#define DICOM_ISCL_MUTUAL_AUTH_RP               0x00120003
#define DICOM_ISCL_MUTUAL_AUTH_PASS1_NOTIF      0x00130002
#define DICOM_ISCL_MUTUAL_AUTH_PASS2_NOTIF      0x00140002
#define DICOM_ISCL_MUTUAL_AUTH_PASS3_NOTIF      0x00150002
#define DICOM_ISCL_MUTUAL_AUTH_COMPLETION_NOTIF 0x00160002
#define DICOM_ISCL_MESSAGE_TRANSMISSION_RQ      0x00200001
#define DICOM_ISCL_MESSAGE_TRANSMISSION_RP      0x00200003
#define DICOM_ISCL_RNDNO_FOR_SESSION_KEY_RQ     0x00210001
#define DICOM_ISCL_RNDNO_FOR_SESSION_KEY_RP     0x00210003
#define DICOM_ISCL_MESSAGE_TRANSMISSION_NOTIF   0x00200002
#define DICOM_ISCL_MAC_TRANSMISSION_NOTIF       0x00230002
#define DICOM_ISCL_THROUGH_MODE_TRANSMISSION_NOTIF 0x00260002
#define DICOM_ISCL_LINE_DISCONNECTION_RQ        0x00FF0001
#define DICOM_ISCL_LINE_DISCONNECTION_RP        0x00FF0003


enum DICOM_ISCLAuthKeyPair
{
   DICOM_ISCL_AUTH_KEY_PAIR1 = 0x00000001,
   DICOM_ISCL_AUTH_KEY_PAIR2 = 0x00000002,
   DICOM_ISCL_AUTH_KEY_PAIR3 = 0x00000003,
   DICOM_ISCL_AUTH_KEY_PAIR4 = 0x00000004,
   DICOM_ISCL_AUTH_KEY_PAIR5 = 0x00000005,
   DICOM_ISCL_AUTH_KEY_PAIR6 = 0x00000006,
   DICOM_ISCL_AUTH_KEY_PAIR7 = 0x00000007,
   DICOM_ISCL_AUTH_KEY_PAIR8 = 0x00000008
};

enum DICOM_ISCLCryptKeyPair
{
   DICOM_ISCL_CRYPT_KEY_PAIR1 = 0x00000001,
   DICOM_ISCL_CRYPT_KEY_PAIR2 = 0x00000002,
   DICOM_ISCL_CRYPT_KEY_PAIR3 = 0x00000003,
   DICOM_ISCL_CRYPT_KEY_PAIR4 = 0x00000004,
   DICOM_ISCL_CRYPT_KEY_PAIR5 = 0x00000005,
   DICOM_ISCL_CRYPT_KEY_PAIR6 = 0x00000006,
   DICOM_ISCL_CRYPT_KEY_PAIR7 = 0x00000007,
   DICOM_ISCL_CRYPT_KEY_PAIR8 = 0x00000008,
   DICOM_ISCL_UNIQUE_SESSION_KEYS = 0x00000010
};

      // header of ISCL messages
struct DICOM_ISCLMessageHeader
{
   L_UINT32 Indicator;    // not used
   L_UINT32 MessageId;    // type of message
   L_UINT32 nDataLength;  // number of bytes in message (excluding header)
   L_UINT32 Option;       // options belonging to the message block
   L_UINT32 timeStamp;    // not used
   L_UINT32 nErrNo;       // not used
   L_UINT32 stuff1, stuff2;  // to align msg header to 32 bytes, not used
};




// end of added for ISCL compliance


//============= TYPES ==========================================================

//============= VARIABLES ======================================================

//============= CLASS ==========================================================

#define DICOM_FILE_OPEN          0x0000
#define DICOM_FILE_CREATE        0x0001
#define DICOM_FILE_READ          0x0002
#define DICOM_FILE_WRITE         0x0004
#define DICOM_FILE_TEMPORARY     0x0008
#define DICOM_FILE_MEMORY        0x0010
#define DICOM_FILE_REDIRECT      0x0020
#define DICOM_FILE_CLOSE         0x0040
#define DICOM_FILE_DELETE        0x0080

#define DICOM_FILE_USER_MEMORY   0x1000 // Internal Use
#define DICOM_FILE_READ_ONLY     0x2000 // Internal Use




#define DICOM_FILE_BEGIN      0
#define DICOM_FILE_CURRENT    1
#define DICOM_FILE_END        2

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomFile
{
public:
   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   LDicomFile();
   ~LDicomFile();

   L_BOOL   Open      (L_TCHAR *pszName, L_UINT16 nMode);
   L_VOID   Close     (L_BOOL bComplete);
   L_BOOL   Read      (L_VOID *pBuffer, L_UINT32 nLength);
   L_BOOL   Write     (L_VOID *pBuffer, L_UINT32 nLength);
   L_BOOL   Seek      (L_SSIZE_T nOffset, L_UINT16 nWhere);
   L_SIZE_T Tell      ();
   L_VOID   SetHandle (L_HFILE hFile, L_UINT16 nMode);
   L_BOOL   SetMemoryBuffer(L_UCHAR *pBuffer, L_SIZE_T uLength, L_BOOL bReadonly);

   //---------------------------------------------------------------------------
   // Private Functions
   //---------------------------------------------------------------------------
private:
   L_TCHAR    m_szName[_MAX_PATH];
   L_HFILE   m_hFile;
   L_UINT16  m_nMode;
   L_SIZE_T  m_nLength;
   L_SSIZE_T m_nOffset;
   L_UCHAR  *m_pBuffer;
};

#endif // #if defined(__cplusplus)

//============= CLASS ==========================================================

#define GENERICLINK  pDICOMLINK pParent;     \
                     pDICOMLINK pFirstChild; \
                     pDICOMLINK pLastChild;  \
                     pDICOMLINK pPrev;       \
                     pDICOMLINK pNext;       \

typedef struct _DICOMLINK *pDICOMLINK;

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomTree
{
public:
   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   LDicomTree();
   ~LDicomTree();

   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   pDICOMLINK  Insert    (pDICOMLINK pNeighbor, L_UINT32 nSize, L_UINT16 nFlags);

   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   L_VOID      Delete    (pDICOMLINK pLink);

   //---------------------------------------------------------------------------
   // Iteration Functions
   //---------------------------------------------------------------------------
   pDICOMLINK  GetRoot   (pDICOMLINK pLink);
   pDICOMLINK  GetParent (pDICOMLINK pLink);
   pDICOMLINK  GetChild  (pDICOMLINK pLink);
   pDICOMLINK  GetFirst  (pDICOMLINK pLink, L_BOOL bTree);
   pDICOMLINK  GetLast   (pDICOMLINK pLink, L_BOOL bTree);
   pDICOMLINK  GetPrev   (pDICOMLINK pLink, L_BOOL bTree);
   pDICOMLINK  GetNext   (pDICOMLINK pLink, L_BOOL bTree);
   L_UINT32    GetLevel  (pDICOMLINK pLink);
   L_UINT32    GetCount  (pDICOMLINK pLink, L_BOOL bTree);

   //---------------------------------------------------------------------------
   // Searching Functions
   //---------------------------------------------------------------------------
   pDICOMLINK  FindIndex (pDICOMLINK pLink, L_BOOL bTree, L_UINT32 nIndex);
   L_BOOL      Exists    (pDICOMLINK pLink);
   pDICOMLINK  Verify    ();

   //---------------------------------------------------------------------------
   // Private Functions
   //---------------------------------------------------------------------------
protected:
   pDICOMLINK m_pFirstChild;
   pDICOMLINK m_pLastChild;
};

#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
class L_LTDIC_CLASS LDicomTreeEx : public LDicomTree
{
public:
   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   LDicomTreeEx();
   ~LDicomTreeEx();
   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   pDICOMLINK  Insert    (pDICOMLINK pNeighbor, L_UINT32 nSize, L_UINT16 nFlags);
   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   L_VOID      Delete    (pDICOMLINK pLink);
};
#endif // #if defined(__cplusplus)

//============= CLASS ==========================================================
#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)

//---------------------------------------------------------------------------
// Initializing Functions
//---------------------------------------------------------------------------
#define DICOM_ENGINE_STARTUP_NO_IOD_TABLE             0x0001U
#define DICOM_ENGINE_STARTUP_NO_CONTEXT_GROUP_TABLE   0x0002U

L_LTDIC_API L_VOID EXT_FUNCTION L_DicomEngineStartup(L_UINT16 uFlags);
L_LTDIC_API L_VOID EXT_FUNCTION L_DicomEngineShutdown(L_VOID);

#if defined(__cplusplus)
}
#endif // #if defined(__cplusplus)

#define VR_AE   0x4145U    // Application Entity
#define VR_AS   0x4153U    // Age String
#define VR_AT   0x4154U    // Attribute Tag
#define VR_CS   0x4353U    // Code String
#define VR_DA   0x4441U    // Date
#define VR_DS   0x4453U    // Decimal String
#define VR_DT   0x4454U    // Date Time
#define VR_FD   0x4644U    // Floating Point Double
#define VR_FL   0x464CU    // Floating Point Single
#define VR_IS   0x4953U    // Integer String
#define VR_LO   0x4C4FU    // Long String
#define VR_LT   0x4C54U    // Long Text
#define VR_OB   0x4F42U    // Other Byte String
#define VR_OW   0x4F57U    // Other Word String
#define VR_PN   0x504EU    // Person Name
#define VR_SH   0x5348U    // Short String
#define VR_SL   0x534CU    // Signed Long
#define VR_SQ   0x5351U    // Sequence of Items
#define VR_SS   0x5353U    // Signed Short
#define VR_ST   0x5354U    // Short Text
#define VR_TM   0x544DU    // Time
#define VR_UI   0x5549U    // Unique Identifier
#define VR_UL   0x554CU    // Unsigned Long
#define VR_UN   0x554EU    // Unknown
#define VR_US   0x5553U    // Unsigned Short
#define VR_UT   0x5554U    // Unlimited Text
#define VR_OF   0x4F46U    // Other Float Strings

enum
{
   VR_FIXED = 0,           // DICOMVR.nLength bytes fixed
   VR_MAXIMUM,             // DICOMVR.nLength bytes maximum
   VR_MAXIMUM_GROUP,       // DICOMVR.nLength maximum per component group
   VR_ANY,                 // DICOMVR.nLength any length valid for any of the other DICOM Value Representations
   VR_NOT_APPLICABLE,      // DICOMVR.nLength not applicable
   VR_MAX,
};

#define VR_BINARY  0x0100  // Binary value
#define VR_STRING  0x0200  // String value (The character '\' is used as the delimiter between values for multiple data elements)
#define VR_TEXT    0x0400  // Text value (Data Elements with this VR shall not be multi-valued)

//typedef struct _DICOMVR DICOMVR, *pDICOMVR;
typedef struct _DICOMVR
{
   GENERICLINK             // Reserved - internally used only

   L_UINT16  nCode;        // Code (VR_AE, VR_AS, ...)
   L_TCHAR   *pszName;      // Name ("Application Entity", "Age String", ...)
   L_UINT32  nLength;      // Length
   L_UINT16  nRestrict;    // Restriction applied to the length
   L_UINT16  nUnitSize;    // The size for the smallest item
} DICOMVR, *pDICOMVR;

//DICOM Character Sets
enum 
{
   DICOM_CHARACTER_SET_DEFAULT               =0 ,
   DICOM_CHARACTER_SET_LATIN_ALPHABET_NO_1      ,
   DICOM_CHARACTER_SET_LATIN_ALPHABET_NO_2      ,
   DICOM_CHARACTER_SET_LATIN_ALPHABET_NO_3      ,
   DICOM_CHARACTER_SET_LATIN_ALPHABET_NO_4      ,
   DICOM_CHARACTER_SET_CYRILLIC                 ,
   DICOM_CHARACTER_SET_ARABIC                   ,
   DICOM_CHARACTER_SET_GREEK                    ,
   DICOM_CHARACTER_SET_HEBREW                   ,
   DICOM_CHARACTER_SET_LATIN_ALPHABET_NO_5      ,
   DICOM_CHARACTER_SET_JAPANESE_JIS_X_0201      ,
   DICOM_CHARACTER_SET_THAI                     ,
   DICOM_CHARACTER_SET_KOREAN                   ,
   DICOM_CHARACTER_SET_UNICODE_IN_UTF8          ,
   DICOM_CHARACTER_SET_GB18030
};

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomVR
{
public:
   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   static L_VOID   Default   ();

   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   static pDICOMVR Insert    (L_UINT16 nCode, L_TCHAR *pszName, L_UINT32 nLength, L_UINT16 nRestrict, L_UINT16 nUnitSize);

   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   static pDICOMVR Delete    (pDICOMVR pVR);
   static L_VOID   Reset     ();

   //---------------------------------------------------------------------------
   // Iteration Functions
   //---------------------------------------------------------------------------
   static pDICOMVR GetFirst  ();
   static pDICOMVR GetLast   ();
   static pDICOMVR GetPrev   (pDICOMVR pVR);
   static pDICOMVR GetNext   (pDICOMVR pVR);
   static L_UINT32 GetCount  ();
   static L_BOOL   Exists    (pDICOMVR pVR);

   //---------------------------------------------------------------------------
   // Searching Functions
   //---------------------------------------------------------------------------
   static pDICOMVR Find      (L_UINT16 nCode);
   static pDICOMVR FindIndex (L_UINT32 nIndex);

   //---------------------------------------------------------------------------
   // Modification Functions
   //---------------------------------------------------------------------------
   static L_BOOL   SetName   (pDICOMVR pVR, L_TCHAR *pszName);

   //---------------------------------------------------------------------------
   // Private Functions
   //---------------------------------------------------------------------------
private:
   static LDicomTreeEx m_InfoVR;
};

#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)

//---------------------------------------------------------------------------
// Initializing Functions
//---------------------------------------------------------------------------
L_LTDIC_API L_VOID   EXT_FUNCTION L_DicomDefaultVR   ();

//---------------------------------------------------------------------------
// Insertion Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMVR EXT_FUNCTION L_DicomInsertVR    (L_UINT16 nCode, L_TCHAR *pszName, L_UINT32 nLength, L_UINT16 nRestrict, L_UINT16 nUnitSize);

//---------------------------------------------------------------------------
// Deletion Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMVR EXT_FUNCTION L_DicomDeleteVR    (pDICOMVR pVR);
L_LTDIC_API L_VOID   EXT_FUNCTION L_DicomResetVR     ();

//---------------------------------------------------------------------------
// Iteration Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMVR EXT_FUNCTION L_DicomGetFirstVR  ();
L_LTDIC_API pDICOMVR EXT_FUNCTION L_DicomGetLastVR   ();
L_LTDIC_API pDICOMVR EXT_FUNCTION L_DicomGetPrevVR   (pDICOMVR pVR);
L_LTDIC_API pDICOMVR EXT_FUNCTION L_DicomGetNextVR   (pDICOMVR pVR);
L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetCountVR  ();
L_LTDIC_API L_BOOL   EXT_FUNCTION L_DicomExistsVR    (pDICOMVR pVR);

//---------------------------------------------------------------------------
// Searching Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMVR EXT_FUNCTION L_DicomFindVR      (L_UINT16 nCode);
L_LTDIC_API pDICOMVR EXT_FUNCTION L_DicomFindIndexVR (L_UINT32 nIndex);

//---------------------------------------------------------------------------
// Modification Functions
//---------------------------------------------------------------------------
L_LTDIC_API L_BOOL   EXT_FUNCTION L_DicomSetNameVR   (pDICOMVR pVR, L_TCHAR *pszName);

#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)


//============= CLASS ==========================================================
#include "ltdicUID.h"

enum
{
   UID_TYPE_OTHER,         // Unknown
   UID_TYPE_TRANSFER1,     // Transfer Syntax - Uncompressed Image
   UID_TYPE_TRANSFER2,     // Transfer Syntax - Compressed Image
   UID_TYPE_CLASS,         // SOP Class
   UID_TYPE_META_CLASS,    // Meta SOP Class
   UID_TYPE_INSTANCE,      // SOP Instance
   UID_TYPE_APPLICATION,   // Application Context Name

   UID_TYPE_FRAME_OF_REFERENCE, // Frame of reference
   UID_TYPE_LDAP_OID,      //
};

//typedef struct _DICOMUID DICOMUID, *pDICOMUID;
typedef struct _DICOMUID
{
   GENERICLINK          // Reserved - internally used only

   L_TCHAR  *pszCode;    // Code
   L_TCHAR  *pszName;    // Name
   L_INT    nType;      // Type
} DICOMUID, *pDICOMUID;

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomUID
{
public:
   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   static L_VOID    Default   ();

   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   static pDICOMUID Insert    (L_TCHAR *pszCode, L_TCHAR *pszName, L_INT nType);

   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   static pDICOMUID Delete    (pDICOMUID pUID);
   static L_VOID    Reset     ();

   //---------------------------------------------------------------------------
   // Iteration Functions
   //---------------------------------------------------------------------------
   static pDICOMUID GetFirst  ();
   static pDICOMUID GetLast   ();
   static pDICOMUID GetPrev   (pDICOMUID pUID);
   static pDICOMUID GetNext   (pDICOMUID pUID);
   static L_UINT32  GetCount  ();
   static L_BOOL    Exists    (pDICOMUID pUID);

   //---------------------------------------------------------------------------
   // Searching Functions
   //---------------------------------------------------------------------------
   static pDICOMUID Find      (L_TCHAR *pszCode);
   static pDICOMUID FindIndex (L_UINT32 nIndex);

   //---------------------------------------------------------------------------
   // Modification Functions
   //---------------------------------------------------------------------------
   static L_BOOL    SetName   (pDICOMUID pUID, L_TCHAR *pszName);

   //---------------------------------------------------------------------------
   // Private Functions
   //---------------------------------------------------------------------------
private:
   static LDicomTreeEx m_InfoUID;
};

#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)

//---------------------------------------------------------------------------
// Initializing Functions
//---------------------------------------------------------------------------
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomDefaultUID   ();

//---------------------------------------------------------------------------
// Insertion Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMUID EXT_FUNCTION L_DicomInsertUID    (L_TCHAR *pszCode, L_TCHAR *pszName, L_INT nType);

//---------------------------------------------------------------------------
// Deletion Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMUID EXT_FUNCTION L_DicomDeleteUID    (pDICOMUID pUID);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomResetUID     ();

//---------------------------------------------------------------------------
// Iteration Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMUID EXT_FUNCTION L_DicomGetFirstUID  ();
L_LTDIC_API pDICOMUID EXT_FUNCTION L_DicomGetLastUID   ();
L_LTDIC_API pDICOMUID EXT_FUNCTION L_DicomGetPrevUID   (pDICOMUID pUID);
L_LTDIC_API pDICOMUID EXT_FUNCTION L_DicomGetNextUID   (pDICOMUID pUID);
L_LTDIC_API L_UINT32  EXT_FUNCTION L_DicomGetCountUID  ();
L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomExistsUID    (pDICOMUID pUID);

//---------------------------------------------------------------------------
// Searching Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMUID EXT_FUNCTION L_DicomFindUID      (L_TCHAR *pszCode);
L_LTDIC_API pDICOMUID EXT_FUNCTION L_DicomFindIndexUID (L_UINT32 nIndex);

//---------------------------------------------------------------------------
// Modification Functions
//---------------------------------------------------------------------------
L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomSetNameUID   (pDICOMUID pUID, L_TCHAR *pszName);

#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)

//============= CLASS ==========================================================

//typedef struct _DICOMTAG DICOMTAG, *pDICOMTAG;
typedef struct _DICOMTAG
{
   GENERICLINK          // Reserved - internally used only

   L_UINT32  nCode;     // Code
   L_UINT32  nMask;     // Mask (for multiple-elements specifies the same entry in the table)
   L_TCHAR   *pszName;   // Name
   L_UINT16  nVR;       // Value Representation
   L_UINT32  nMinVM;    // Minimum Value Multiplicity
   L_INT32   nMaxVM;    // Maximum Value Multiplicity
   L_UINT32  nDivideVM; // Value that should divide the Value Multiplicity
} DICOMTAG, *pDICOMTAG;

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomTag
{
public:
   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   static L_VOID    Default   ();

   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   static pDICOMTAG Insert    (L_UINT32 nCode, L_UINT32 nMask, L_TCHAR *pszName, L_UINT16 nVR, L_UINT32 nMinVM, L_UINT32 nMaxVM, L_UINT32 nDivideVM);

   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   static pDICOMTAG Delete    (pDICOMTAG pTag);
   static L_VOID    Reset     ();

   //---------------------------------------------------------------------------
   // Iteration Functions
   //---------------------------------------------------------------------------
   static pDICOMTAG GetFirst  ();
   static pDICOMTAG GetLast   ();
   static pDICOMTAG GetPrev   (pDICOMTAG pTag);
   static pDICOMTAG GetNext   (pDICOMTAG pTag);
   static L_UINT32  GetCount  ();
   static L_BOOL    Exists    (pDICOMTAG pTag);

   //---------------------------------------------------------------------------
   // Searching Functions
   //---------------------------------------------------------------------------
   static pDICOMTAG Find      (L_UINT32 nCode);
   static pDICOMTAG FindIndex (L_UINT32 nIndex);

   //---------------------------------------------------------------------------
   // Modification Functions
   //---------------------------------------------------------------------------
   static L_BOOL    SetName   (pDICOMTAG pTag, L_TCHAR *pszName);

   //---------------------------------------------------------------------------
   // Private Functions
   //---------------------------------------------------------------------------
private:
   static LDicomTreeEx m_InfoTag;
};

#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)

//---------------------------------------------------------------------------
// Initializing Functions
//---------------------------------------------------------------------------
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomDefaultTag   ();

//---------------------------------------------------------------------------
// Insertion Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMTAG EXT_FUNCTION L_DicomInsertTag    (L_UINT32 nCode, L_UINT32 nMask, L_TCHAR *pszName, L_UINT16 nVR, L_UINT32 nMinVM, L_UINT32 nMaxVM, L_UINT32 nDivideVM);

//---------------------------------------------------------------------------
// Deletion Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMTAG EXT_FUNCTION L_DicomDeleteTag    (pDICOMTAG pTag);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomResetTag     ();

//---------------------------------------------------------------------------
// Iteration Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMTAG EXT_FUNCTION L_DicomGetFirstTag  ();
L_LTDIC_API pDICOMTAG EXT_FUNCTION L_DicomGetLastTag   ();
L_LTDIC_API pDICOMTAG EXT_FUNCTION L_DicomGetPrevTag   (pDICOMTAG pTag);
L_LTDIC_API pDICOMTAG EXT_FUNCTION L_DicomGetNextTag   (pDICOMTAG pTag);
L_LTDIC_API L_UINT32  EXT_FUNCTION L_DicomGetCountTag  ();
L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomExistsTag    (pDICOMTAG pTag);

//---------------------------------------------------------------------------
// Searching Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMTAG EXT_FUNCTION L_DicomFindTag      (L_UINT32 nCode);
L_LTDIC_API pDICOMTAG EXT_FUNCTION L_DicomFindIndexTag (L_UINT32 nIndex);

//---------------------------------------------------------------------------
// Modification Functions
//---------------------------------------------------------------------------
L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomSetNameTag   (pDICOMTAG pTag, L_TCHAR *pszName);

#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)

#include "ltdicTags.h"

// For backward compatability
#define TAG_UNDEFINED_TYPE                                  0x000000000L
#define TAG_CONTRIBUTION_DATETIME                           TAG_CONTRIBUTION_DATE_TIME
#define TAG_END_MESSAGE_SET                                 TAG_END_MESSAGE_ID
#define TAG_NORMAL_REVERSE                                  TAG_NOR_REV
#define TAG_OLD_MAGNIFICATION_TYPE                          TAG_COMMAND_MAGNIFICATION_TYPE
#define TAG_NUCLEAR_MEDICINE_SERIES_TYPE_RETIRED            TAG_NUCLEAR_MEDICINE_SERIES_TYPE
#define TAG_ADMITTING_DIAGNOSIS_CODE_SEQUENCE               TAG_ADMITTING_DIAGNOSES_CODE_SEQUENCE
#define TAG_REFERENCED_STUDY_COMPONENT_SEQUENCE             TAG_REFERENCED_PERFORMED_PROCEDURE_STEP_SEQUENCE
#define TAG_TRANSDUCER_POSITION_RETIRED                     TAG_TRANSDUCER_POSITION
#define TAG_TRANSDUCER_ORIENTATION_RETIRED                  TAG_TRANSDUCER_ORIENTATION
#define TAG_ANATOMIC_STRUCTURE_RETIRED                      TAG_ANATOMIC_STRUCTURE
#define TAG_CONTRAST_ALLERGIES                              TAG_ALLERGIES
#define TAG_INTERVENTION_DRUG_CODE_SEQUENCE                 TAG_INTERVENTION_DRUG_SEQUENCE
#define TAG_RADIONUCLIDE_RETIRED                            TAG_RADIONUCLIDE
#define TAG_ENERGY_WINDOW_CENTERLINE_RETIRED                TAG_ENERGY_WINDOW_CENTERLINE
#define TAG_ENERGY_WINDOW_TOTAL_WIDTH_RETIRED               TAG_ENERGY_WINDOW_TOTAL_WIDTH
#define TAG_INTERVENTIONAL_THERAPY_SEQUENCE                 TAG_INTERVENTION_SEQUENCE
#define TAG_INTERVENTIONAL_STATUS                           TAG_INTERVENTION_STATUS
#define TAG_EFFECTIVE_SERIES_DURATION                       TAG_EFFECTIVE_DURATION
#define TAG_SYNCHRONIZATION_FRAME_OF_REFERENCE              0x0018106BUL
#define TAG_SECONDARY_CAPTURE_DEVICE_MANUFACTURER           TAG_SECONDARY_CAPTURE_DEVICE_MANUFACTURERS
#define TAG_HARDCOPY_DEVICE_MANFUACTURER_MODEL_NAME         TAG_HARDCOPY_DEVICE_MANUFACTURER_MODEL_NAME
#define TAG_FRAMING_TYPE                                    TAG_CARDIAC_FRAMING_TYPE
#define TAG_ROTATION_OFFSET_RETIRED                         TAG_ROTATION_OFFSET
#define TAG_IMAGE_AREA_DOSE_PRODUCT                         TAG_IMAGE_AND_FLUOROSCOPY_AREA_DOSE_PRODUCT
#define TAG_RECEIVING_COIL                                  TAG_RECEIVE_COIL_NAME
#define TAG_TRANSMITTING_COIL                               TAG_TRANSMIT_COIL_NAME
#define TAG_PHASE_ENCODING_DIRECTION                        TAG_IN_PLANE_PHASE_ENCODING_DIRECTION
#define TAG_THERMAL_INDEX                                   TAG_BONE_THERMAL_INDEX
#define TAG_REGION_LOCATION_MIN_X_0                         TAG_REGION_LOCATION_MIN_X0
#define TAG_REGION_LOCATION_MIN_Y_0                         TAG_REGION_LOCATION_MIN_Y0
#define TAG_REGION_LOCATION_MAX_X_1                         TAG_REGION_LOCATION_MAX_X1
#define TAG_REGION_LOCATION_MAX_Y_1                         TAG_REGION_LOCATION_MAX_Y1
#define TAG_REFERENCE_PIXEL_X_0                             TAG_REFERENCE_PIXEL_X0
#define TAG_REFERENCE_PIXEL_Y_0                             TAG_REFERENCE_PIXEL_Y0
#define TAG_TM_LINE_POSITION_X_0                            TAG_TM_LINE_POSITION_X0
#define TAG_TM_LINE_POSITION_Y_0                            TAG_TM_LINE_POSITION_Y0
#define TAG_TM_LINE_POSITION_X_1                            TAG_TM_LINE_POSITION_X1
#define TAG_TM_LINE_POSITION_Y_1                            TAG_TM_LINE_POSITION_Y1
#define TAG_DETECTOR_ACTIVE_DIMENSIONS                      TAG_DETECTOR_ACTIVE_DIMENSION
#define TAG_EXPOSURE_TIME_IN_NANO_S                         TAG_EXPOSURE_TIME_IN_US
#define TAG_XRAY_TUBE_CURRENT_IN_NANO_A                     TAG_X_RAY_TUBE_CURRENT_IN_UA
#define TAG_CARDIAC_TRIGGER_SEQUENCE                        TAG_CARDIAC_SYNCHRONIZATION_SEQUENCE
#define TAG_CHEMICAL_SHIFTS_MINIMUM_INTEGRATION_LIMIT       TAG_CHEMICAL_SHIFTS_MINIMUM_INTEGRATION_LIMIT_IN_HZ
#define TAG_CHEMICAL_SHIFTS_MAXIMUM_INTEGRATION_LIMIT       TAG_CHEMICAL_SHIFTS_MAXIMUM_INTEGRATION_LIMIT_IN_HZ
#define TAG_ISOTOPE_NUMBER_RETIRED                          TAG_ISOTOPE_NUMBER
#define TAG_PHASE_NUMBER_RETIRED                            TAG_PHASE_NUMBER
#define TAG_INTERVAL_NUMBER_RETIRED                         TAG_INTERVAL_NUMBER
#define TAG_TIME_SLOT_NUMBER_RETIRED                        TAG_TIME_SLOT_NUMBER
#define TAG_ANGLE_NUMBER_RETIRED                            TAG_ANGLE_NUMBER
#define TAG_TRIGGER_DELAY_TIME                              TAG_NOMINAL_CARDIAC_TRIGGER_DELAY_TIME
#define TAG_BIPLANE_ACQUISITION_SEQUENCE                    TAG_BI_PLANE_ACQUISITION_SEQUENCE
#define TAG_MASK_POINTER_RETIRED                            TAG_MASK_POINTER
#define TAG_WAVEFORM_SAMPLE_VALUE_REPRESENTATION            0x003A0103UL
#define TAG_DIFFERENTIAL_CHANNEL_SOURCE_MODIFIERS           0x003A020BUL
#define TAG_SCHEDULED_ACTION_ITEM_CODE_SEQUENCE             TAG_SCHEDULED_PROTOCOL_CODE_SEQUENCE
#define TAG_REFERENCED_STANDALONE_SOP_INSTANCE_SEQUENCE     TAG_REFERENCED_NON_IMAGE_COMPOSITE_SOP_INSTANCE_SEQUENCE
#define TAG_PERFORMED_ACTION_ITEM_SEQUENCE                  TAG_PERFORMED_PROTOCOL_CODE_SEQUENCE
#define TAG_COMMENTS_ON_THE_PERFORMED_PROCEDURE_STEPS       TAG_COMMENTS_ON_THE_PERFORMED_PROCEDURE_STEP
#define TAG_BILLING_SUPPLIES_AND_DEVICES_SEQUENCE_RETIRED   TAG_BILLING_SUPPLIES_AND_DEVICES_SEQUENCE
#define TAG_PLACER_ORDER_NUMBER_PROCEDURE_RETIRED           TAG_PLACER_ORDER_NUMBER_PROCEDURE
#define TAG_FILLER_ORDER_NUMBER_PROCEDURE_RETIRED           TAG_FILLER_ORDER_NUMBER_PROCEDURE
#define TAG_PERSONS_ADDRESS                                 TAG_PERSON_ADDRESS
#define TAG_PERSONS_TELEPHONE_NUMBERS                       TAG_PERSON_TELEPHONE_NUMBERS
#define TAG_INPUT_INFORMATIONSEQUENCE                       TAG_INPUT_INFORMATION_SEQUENCE
#define TAG_INTER_MARKER_DISTANCE                           TAG_INTERMARKER_DISTANCE
#define TAG_TOPIC_KEY_WORDS                                 TAG_TOPIC_KEYWORDS
#define TAG_REFERENCED_VOI_LUT_BOX_SEQUENCE_RETIRED         TAG_REFERENCED_VOI_LUT_BOX_SEQUENCE
#define TAG_OVERLAY_MODE_RETIRED                            TAG_OVERLAY_MODE
#define TAG_THRESHOLD_DENSITY_RETIRED                       TAG_THRESHOLD_DENSITY
#define TAG_REFERENCED_PRINT_JOB_SEQUENCE_2120              TAG_REFERENCED_PRINT_JOB_SEQUENCE
#define TAG_TREATMENT_INTENT                                TAG_PLAN_INTENT
#define TAG_NUMBER_OF_FRACTIONS_PER_DAY                     TAG_NUMBER_OF_FRACTION_PATTERN_DIGITS_PER_DAY
#define TAG_HIGHDOSE_TECHNIQUE_TYPE                         TAG_HIGH_DOSE_TECHNIQUE_TYPE
#define TAG_AIR_KERMA_RATE_REFERENCE_DATE                   TAG_SOURCE_STRENGTH_REFERENCE_DATE
#define TAG_AIR_KERMA_RATE_REFERENCE_TIME                   TAG_SOURCE_STRENGTH_REFERENCE_TIME
#define TAG_REFERENCED_OVERLAY_SEQUENCE_50XX                TAG_CURVE_REFERENCED_OVERLAY_SEQUENCE
#define TAG_LEAD_BITMAP                                     TAG_PIXEL_DATA

#define TAG_REFERENCED_OVERLAY_GROUP                        TAG_CURVE_REFERENCED_OVERLAY_GROUP




//============= CLASS ==========================================================

enum
{
   IOD_TYPE_CLASS=0,             // Class type
   IOD_TYPE_MODULE,              // Module type
   IOD_TYPE_ELEMENT,             // Element type
   IOD_TYPE_MAX,
};

enum
{
   IOD_USAGE_M    = 0,                  // Mandatory IOD   - M - (U - Unique for key)
   IOD_USAGE_C    = 1,                  // Conditional IOD - C - (R - Required for key)
   IOD_USAGE_U    = 2,                  // Optional IOD    - U - (O - Optional for key)
   IOD_USAGE_1    = 3,                  // Mandatory IOD   - 1  (type 1)
   IOD_USAGE_1C   = 4,                  // Conditional IOD - 1C (type 1)
   IOD_USAGE_2    = 5,                  // Mandatory IOD   - 2  (type 2)
   IOD_USAGE_2C   = 6,                  // Conditional IOD - 2C (type 2)
   IOD_USAGE_3    = 7,                  // Optional IOD    - 3  (type 3)
   IOD_USAGE_MAX  = 8,
};

//typedef struct _DICOMIOD DICOMIOD, *pDICOMIOD;
typedef struct _DICOMIOD
{
   GENERICLINK                   // Reserved - internally used only

   L_UINT32  nCode;              // Code (CLASS_CR_IMAGE, MODULE_PATIENT, TAG_STUDY_TIME, ...)
   L_TCHAR   *pszName;           // Name
   L_UCHAR   nType;              // Type (IOD_TYPE_CLASS, IOD_TYPE_MODULE, IOD_TYPE_ELEMENT)
   L_UINT16  nUsage;             // Usage (IOD_USAGE_M, IOD_USAGE_C, ...) 
   L_TCHAR   *pszDescription;    // Description
} DICOMIOD, *pDICOMIOD;

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomIOD
{
public:
   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   static L_VOID    Default         ();

   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   static pDICOMIOD Insert          (pDICOMIOD pNeighbor, L_BOOL bChild, L_UINT32 nCode, L_TCHAR  *pszName, L_UCHAR nType, L_UINT16 nUsage, L_TCHAR *pszDescription);

   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   static pDICOMIOD Delete          (pDICOMIOD pIOD);
   static L_VOID    Reset           ();

   //---------------------------------------------------------------------------
   // Iteration Functions
   //---------------------------------------------------------------------------
   static pDICOMIOD GetRoot         (pDICOMIOD pIOD);
   static pDICOMIOD GetParent       (pDICOMIOD pIOD);
   static pDICOMIOD GetChild        (pDICOMIOD pIOD);
   static pDICOMIOD GetFirst        (pDICOMIOD pIOD, L_BOOL bTree);
   static pDICOMIOD GetLast         (pDICOMIOD pIOD, L_BOOL bTree);
   static pDICOMIOD GetPrev         (pDICOMIOD pIOD, L_BOOL bTree);
   static pDICOMIOD GetNext         (pDICOMIOD pIOD, L_BOOL bTree);
   static L_UINT32  GetCountModule  (L_UINT32 nClass);
   static L_BOOL    Exists          (pDICOMIOD pIOD);

   //---------------------------------------------------------------------------
   // Searching Functions
   //---------------------------------------------------------------------------
   static pDICOMIOD Find            (pDICOMIOD pIOD, L_UINT32 nCode, L_UCHAR nType, L_BOOL bTree);
   static pDICOMIOD FindClass       (L_UINT32 nClass);
   static pDICOMIOD FindModule      (L_UINT32 nClass, L_UINT32 nModule);
   static pDICOMIOD FindIndexModule (L_UINT32 nClass, L_UINT32 nIndex);

   //---------------------------------------------------------------------------
   // Modification Functions
   //---------------------------------------------------------------------------
   static L_BOOL    SetName         (pDICOMIOD pIOD, L_TCHAR *pszName);
   static L_BOOL    SetDescription  (pDICOMIOD pIOD, L_TCHAR *pszDescription);

   //---------------------------------------------------------------------------
   // Private Functions
   //---------------------------------------------------------------------------
private:
   static LDicomTreeEx m_InfoIOD;
   
   static L_VOID    Default         (pDICOMIOD pIOD, L_VOID *pReference);
   static pDICOMIOD FindInClass     (pDICOMIOD pIOD, L_UINT32 nCode, L_UCHAR nType);

#if defined(LEADTOOLS_V16_OR_LATER)
   static L_BOOL m_bInitialized;
#endif
public:
   static L_VOID SetInitialized(L_BOOL bValue);
};

#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)


// For use with L_DicomSetMemoryAllocation
#define MEMORY_FAIL   (0)
#define MEMORY_FAR    (1)
#define MEMORY_GLOBAL (2)


//---------------------------------------------------------------------------
// Initializing Functions
//---------------------------------------------------------------------------
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomDefaultIOD         ();
L_LTDIC_API L_UINT16  EXT_FUNCTION L_DicomSetMemoryAllocation(L_UINT16 nType);

//---------------------------------------------------------------------------
// Insertion Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomInsertIOD          (pDICOMIOD pNeighbor, L_BOOL bChild, L_UINT32 nCode, L_TCHAR  *pszName, L_UCHAR nType, L_UINT16 nUsage, L_TCHAR *pszDescription);

//---------------------------------------------------------------------------
// Deletion Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomDeleteIOD          (pDICOMIOD pIOD);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomResetIOD           ();

//---------------------------------------------------------------------------
// Iteration Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomGetRootIOD         (pDICOMIOD pIOD);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomGetParentIOD       (pDICOMIOD pIOD);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomGetChildIOD        (pDICOMIOD pIOD);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomGetFirstIOD        (pDICOMIOD pIOD, L_BOOL bTree);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomGetLastIOD         (pDICOMIOD pIOD, L_BOOL bTree);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomGetPrevIOD         (pDICOMIOD pIOD, L_BOOL bTree);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomGetNextIOD         (pDICOMIOD pIOD, L_BOOL bTree);
L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomExistsIOD          (pDICOMIOD pIOD);
L_LTDIC_API L_UINT32  EXT_FUNCTION L_DicomGetCountModuleIOD  (L_UINT32 nClass);

//---------------------------------------------------------------------------
// Searching Functions
//---------------------------------------------------------------------------
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomFindIOD            (pDICOMIOD pIOD, L_UINT32 nCode, L_UCHAR nType, L_BOOL bTree);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomFindClassIOD       (L_UINT32 nClass);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomFindModuleIOD      (L_UINT32 nClass, L_UINT32 nModule);
L_LTDIC_API pDICOMIOD EXT_FUNCTION L_DicomFindIndexModuleIOD (L_UINT32 nClass, L_UINT32 nIndex);

//---------------------------------------------------------------------------
// Modification Functions
//---------------------------------------------------------------------------
L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomSetNameIOD         (pDICOMIOD pIOD, L_TCHAR *pszName);
L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomSetDescriptionIOD  (pDICOMIOD pIOD, L_TCHAR *pszDescription);

#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)

enum
{
   CLASS_CR_IMAGE_STORAGE                                              =    0,
   CLASS_CT_IMAGE_STORAGE                                              =    1,
   CLASS_MR_IMAGE_STORAGE                                              =    2,
   CLASS_NM_IMAGE_STORAGE                                              =    3,
   CLASS_NM_IMAGE_STORAGE_RETIRED                                      =    4,
   CLASS_US_IMAGE_STORAGE                                              =    5,
   CLASS_US_IMAGE_STORAGE_RETIRED                                      =    6,
   CLASS_US_MULTI_FRAME_IMAGE_STORAGE                                  =    7,
   CLASS_US_MULTI_FRAME_IMAGE_STORAGE_RETIRED                          =    8,
   CLASS_SC_IMAGE_STORAGE                                              =    9,
   CLASS_STANDALONE_OVERLAY_STORAGE                                    =   10,
   CLASS_STANDALONE_CURVE_STORAGE                                      =   11,
   CLASS_BASIC_STUDY_DESCRIPTOR                                        =   12,
   CLASS_STANDALONE_MODALITY_LUT_STORAGE                               =   13,
   CLASS_STANDALONE_VOI_LUT_STORAGE                                    =   14,
   CLASS_XA_IMAGE_STORAGE                                              =   15,
   CLASS_XA_BIPLANE_IMAGE_STORAGE_RETIRED                              =   16,
   CLASS_XRF_IMAGE_STORAGE                                             =   17,
   CLASS_RT_IMAGE_STORAGE                                              =   18,
   CLASS_RT_DOSE_STORAGE                                               =   19,
   CLASS_RT_STRUCTURE_SET_STORAGE                                      =   20,
   CLASS_RT_PLAN_STORAGE                                               =   21,
   CLASS_PET_IMAGE_STORAGE                                             =   22,
   CLASS_STANDALONE_PET_CURVE_STORAGE                                  =   23,
   CLASS_STORED_PRINT_STORAGE                                          =   24,
   CLASS_HC_GRAYSCALE_IMAGE_STORAGE                                    =   25,
   CLASS_HC_COLOR_IMAGE_STORAGE                                        =   26,
   CLASS_DX_IMAGE_STORAGE_PRESENTATION                                 =   27,
   CLASS_DX_IMAGE_STORAGE_PROCESSING                                   =   28,
   CLASS_DX_MAMMOGRAPHY_IMAGE_STORAGE_PRESENTATION                     =   29,
   CLASS_DX_MAMMOGRAPHY_IMAGE_STORAGE_PROCESSING                       =   30,
   CLASS_DX_INTRAORAL_IMAGE_STORAGE_PRESENTATION                       =   31,
   CLASS_DX_INTRAORAL_IMAGE_STORAGE_PROCESSING                         =   32,
   CLASS_RT_BEAMS_TREATMENT_RECORD_STORAGE                             =   33,
   CLASS_RT_BRACHY_TREATMENT_RECORD_STORAGE                            =   34,
   CLASS_RT_TREATMENT_SUMMARY_RECORD_STORAGE                           =   35,
   CLASS_VL_ENDOSCOPIC_IMAGE_STORAGE                                   =   36,
   CLASS_VL_MICROSCOPIC_IMAGE_STORAGE                                  =   37,
   CLASS_VL_SLIDE_COORDINATES_MICROSCOPIC_IMAGE_STORAGE                =   38,
   CLASS_VL_PHOTOGRAPHIC_IMAGE_STORAGE                                 =   39,
   
   CLASS_PATIENT                                                       =   40,
   CLASS_VISIT                                                         =   41,
   CLASS_STUDY                                                         =   42,
   CLASS_STUDY_COMPONENT                                               =   43,
   CLASS_RESULTS                                                       =   44,
   CLASS_INTERPRETATION                                                =   45,
   CLASS_BASIC_FILM_SESSION                                            =   46,
   CLASS_BASIC_FILM_BOX                                                =   47,
   CLASS_BASIC_GRAYSCALE_IMAGE_BOX                                     =   48,
   CLASS_BASIC_COLOR_IMAGE_BOX                                         =   49,
   CLASS_BASIC_ANNOTATION_BOX                                          =   50,
   CLASS_PRINT_JOB                                                     =   51,
   CLASS_PRINTER                                                       =   52,
   CLASS_VOI_LUT_BOX_RETIRED                                           =   53,
   CLASS_IMAGE_OVERLAY_BOX_RETIRED                                     =   54,
   CLASS_STORAGE_COMMITMENT_PUSH_MODEL                                 =   55,
   CLASS_STORAGE_COMMITMENT_PULL_MODEL                                 =   56,
   CLASS_PRINT_QUEUE                                                   =   57,
   CLASS_MODALITY_PERFORMED_PROCEDURE_STEP                             =   58,
   CLASS_PRESENTATION_LUT                                              =   59,
   CLASS_PULL_PRINT_REQUEST                                            =   60,
   CLASS_PATIENT_META                                                  =   61,
   CLASS_STUDY_META                                                    =   62,
   CLASS_RESULTS_META                                                  =   63,
   CLASS_BASIC_GRAYSCALE_PRINT_META                                    =   64,
   CLASS_BASIC_COLOR_PRINT_META                                        =   65,
   CLASS_REFERENCED_GRAYSCALE_PRINT_META_RETIRED                       =   66,
   CLASS_REFERENCED_COLOR_PRINT_META_RETIRED                           =   67,
   CLASS_PULL_STORED_PRINT_META                                        =   68,
   CLASS_PRINTER_CONFIGURATION                                         =   69,
   CLASS_BASIC_PRINT_IMAGE_OVERLAY_BOX                                 =   70,
   CLASS_BASIC_DIRECTORY                                               =   71,
   CLASS_PATIENT_ROOT_QUERY_PATIENT                                    =   72,
   CLASS_PATIENT_ROOT_QUERY_STUDY                                      =   73,
   CLASS_PATIENT_ROOT_QUERY_SERIES                                     =   74,
   CLASS_PATIENT_ROOT_QUERY_IMAGE                                      =   75,
   CLASS_STUDY_ROOT_QUERY_STUDY                                        =   76,
   CLASS_STUDY_ROOT_QUERY_SERIES                                       =   77,
   CLASS_STUDY_ROOT_QUERY_IMAGE                                        =   78,
   CLASS_PATIENT_STUDY_QUERY_PATIENT                                   =   79,
   CLASS_PATIENT_STUDY_QUERY_STUDY                                     =   80,

   CLASS_BASIC_TEXT_SR                                                 =   81,
   CLASS_ENHANCED_SR                                                   =   82,
   CLASS_COMPREHENSIVE_SR                                              =   83,
   CLASS_MODALITY_WORKLIST                                             =   84,
   CLASS_GRAYSCALE_SOFTCOPY_PRESENTATION_STATE                         =   85,
   CLASS_BASIC_VOICE_AUDIO                                             =   86,
   CLASS_12_LEAD_ECG                                                   =   87,
   CLASS_GENERAL_ECG                                                   =   88,
   CLASS_AMBULATORY_ECG                                                =   89,
   CLASS_HEMODYNAMIC                                                   =   90,
   CLASS_BASIC_CARDIAC_EP                                              =   91,
   CLASS_ENHANCED_MR_IMAGE_STORAGE                                     =   92,
   CLASS_MR_SPECTROSCOPY_STORAGE                                       =   93,
   CLASS_RAW_DATA_STORAGE                                              =   94,
   CLASS_SC_MULTI_FRAME_SINGLE_BIT_IMAGE_STORAGE                       =   95,
   CLASS_SC_MULTI_FRAME_GRAYSCALE_BYTE_IMAGE_STORAGE                   =   96,
   CLASS_SC_MULTI_FRAME_GRAYSCALE_WORD_IMAGE_STORAGE                   =   97,
   CLASS_SC_MULTI_FRAME_TRUE_COLOR_IMAGE_STORAGE                       =   98,
   CLASS_GENERAL_PURPOSE_SCHEDULED_PROCEDURE_STEP                      =   99,
   CLASS_GENERAL_PURPOSE_PERFORMED_PROCEDURE_STEP                      =  100,
   CLASS_GENERAL_PURPOSE_WORKLIST_MANAGEMENT_META                      =  101,
   CLASS_KEY_OBJECT_SELECTION_DOCUMENT                                 =  102,
   CLASS_MAMMOGRAPHY_CAD_SR                                            =  103,
   CLASS_CHEST_CAD_SR                                                  =  104,
   CLASS_GENERAL_PURPOSE_WORKLIST                                      =  105,
   CLASS_OPHTHALMIC_8_BIT_PHOTOGRAPHY_IMAGE_STORAGE                    =  106,
   CLASS_OPHTHALMIC_16_BIT_PHOTOGRAPHY_IMAGE_STORAGE                   =  107,
   CLASS_STEREOMETRIC_RELATIONSHIP_STORAGE                             =  108,
   CLASS_VIDEO_ENDOSCOPIC_IMAGE_STORAGE                                =  109,
   CLASS_VIDEO_MICROSCOPIC_IMAGE_STORAGE                               =  110,
   CLASS_VIDEO_PHOTOGRAPHIC_IMAGE_STORAGE                              =  111,

   CLASS_UNDEFINED                                                     =  112,
#if !defined(LEADTOOLS_V16_OR_LATER)
   CLASS_MAX                                                           =  113,
#endif


#if defined(LEADTOOLS_V16_OR_LATER)
   CLASS_PSEUDO_COLOR_SOFTCOPY_PRESENTATION_STATE_STORAGE              =  113,
   CLASS_BLENDING_SOFTCOPY_PRESENTATION_STATE_STORAGE                  =  114,

   CLASS_PROCEDURE_LOG_STORAGE                                         =  115,
   CLASS_X_RAY_RADIATION_DOSE_SR_STORAGE                               =  116,
   CLASS_ENHANCED_CT_IMAGE_STORAGE                                     =  117,
   CLASS_SPATIAL_REGISTRATION_STORAGE                                  =  118,
   CLASS_DEFORMABLE_SPATIAL_REGISTRATION_STORAGE                       =  119,
   CLASS_SPATIAL_FIDUCIALS_STORAGE                                     =  120,

   CLASS_HANGING_PROTOCOL_STORAGE                                      =  121,
   CLASS_ENCAPSULATED_PDF_STORAGE                                      =  122,
   CLASS_ENCAPSULATED_CDA_STORAGE                                      =  123,
   CLASS_REAL_WORLD_VALUE_MAPPING_STORAGE                              =  124,
   CLASS_ENHANCED_XA_IMAGE_STORAGE                                     =  125,
   CLASS_ENHANCED_XRF_IMAGE_STORAGE                                    =  126,
   CLASS_RT_ION_PLAN_STORAGE                                           =  127,
   CLASS_RT_ION_BEAMS_TREATMENT_RECORD_STORAGE                         =  128,
   CLASS_SEGMENTATION_STORAGE                                          =  129,
   CLASS_OPHTHALMIC_TOMOGRAPHY_IMAGE_STORAGE                           =  130,
   CLASS_X_RAY_3D_ANGIOGRAPHIC_IMAGE_STORAGE                           =  131,
   CLASS_X_RAY_3D_CRANIOFACIAL_IMAGE_STORAGE                           =  132,
   CLASS_BASIC_IMAGE_BOX                                               =  133,
   CLASS_INSTANCE_AVAILABILITY_NOTIFICATION                            =  134,
   CLASS_MEDIA_CREATION_MANAGEMENT                                     =  135,
   CLASS_ENHANCED_PET_IMAGE_STORAGE                                    =  136,

   CLASS_LENSOMETRY_MEASUREMENTS_STORAGE                               =  137,
   CLASS_AUTOREFRACTION_MEASUREMENTS_STORAGE                           =  138,
   CLASS_KERATOMETRY_MEASUREMENTS_STORAGE                              =  139,
   CLASS_SUBJECTIVE_REFRACTION_MEASUREMENTS_STORAGE                    =  140,
   CLASS_VISUAL_ACUITY_MEASUREMENTS_STORAGE                            =  141,
   CLASS_SPECTACLE_PRESCRIPTION_REPORT_STORAGE                         =  142,

   // Part IV
   CLASS_SUBSTANCE_ADMINISTRATION_LOGGING                              =  143,
   CLASS_GENERAL_RELEVANT_PATIENT_INFORMATION_QUERY                    =  144,
   CLASS_HANGING_PROTOCOL_INFORMATION_MODEL_FIND                       =  145,
   CLASS_PRODUCT_CHARACTERISTICS_QUERY                                 =  146,
   CLASS_SUBSTANCE_APPROVAL_QUERY                                      =  147,
   CLASS_BASIC_STRUCTURED_DISPLAY_STORAGE                              =  148,


   CLASS_ENHANCED_US_VOLUME_STORAGE                                    =  149,                     
   CLASS_ARTERIAL_PULSE_WAVEFORM_STORAGE                               =  150,                
   CLASS_RESPIRATORY_WAVEFORM_STORAGE                                  =  151,                   
   CLASS_GENERAL_AUDIO_WAVEFORM_STORAGE                                =  152, 
   CLASS_BREAST_TOMOSYNTHESIS_IMAGE_STORAGE                            =  153,
   CLASS_COLON_CAD_SR_STORAGE                                          =  154,
   CLASS_SURFACE_SEGMENTATION_STORAGE                                  =  155,
   CLASS_COLOR_PALETTE_STORAGE                                         =  156,
   CLASS_ENHANCED_MR_COLOR_IMAGE_STORAGE                               =  157,

   CLASS_MAX                                                           =  158,

   // For v15 backwards compatibility
   CLASS_ENHANCED_SR_STORAGE                                           =  CLASS_ENHANCED_SR,
   CLASS_BASIC_TEXT_SR_STORAGE                                         =  CLASS_BASIC_TEXT_SR,
   CLASS_COMPREHENSIVE_SR_STORAGE                                      =  CLASS_COMPREHENSIVE_SR,
   CLASS_BASIC_VOICE_AUDIO_STORAGE                                     =  CLASS_BASIC_VOICE_AUDIO,
   CLASS_12_LEAD_ECG_STORAGE                                           =  CLASS_12_LEAD_ECG,
   CLASS_GENERAL_ECG_STORAGE                                           =  CLASS_GENERAL_ECG,
   CLASS_AMBULATORY_ECG_STORAGE                                        =  CLASS_AMBULATORY_ECG,
   CLASS_HEMODYNAMIC_STORAGE                                           =  CLASS_HEMODYNAMIC,
   CLASS_BASIC_CARDIAC_EP_STORAGE                                      =  CLASS_BASIC_CARDIAC_EP,
   CLASS_KEY_OBJECT_SELECTION_DOCUMENT_STORAGE                         =  CLASS_KEY_OBJECT_SELECTION_DOCUMENT,
   CLASS_MAMMOGRAPHY_CAD_SR_STORAGE                                    =  CLASS_MAMMOGRAPHY_CAD_SR,
   CLASS_CHEST_CAD_SR_STORAGE                                          =  CLASS_CHEST_CAD_SR,
   CLASS_OPHTHALMIC_PHOTOGRAPHY_8_BIT_IMAGE_STORAGE                    =  CLASS_OPHTHALMIC_8_BIT_PHOTOGRAPHY_IMAGE_STORAGE,
   CLASS_OPHTHALMIC_PHOTOGRAPHY_16_BIT_IMAGE_STORAGE                   =  CLASS_OPHTHALMIC_16_BIT_PHOTOGRAPHY_IMAGE_STORAGE,

#endif
};


#define CLASS_UNKNOWN  65535

enum
{
   MODULE_PATIENT                                                      =    0,
   MODULE_GENERAL_STUDY                                                =    1,
   MODULE_PATIENT_STUDY                                                =    2,
   MODULE_GENERAL_SERIES                                               =    3,
   MODULE_CR_SERIES                                                    =    4,
   MODULE_GENERAL_EQUIPMENT                                            =    5,
   MODULE_GENERAL_IMAGE                                                =    6,
   MODULE_IMAGE_PIXEL                                                  =    7,
   MODULE_CONTRAST_BOLUS                                               =    8,
   MODULE_CR_IMAGE                                                     =    9,
   MODULE_OVERLAY_PLANE                                                =   10,
   MODULE_CURVE                                                        =   11,
   MODULE_MODALITY_LUT                                                 =   12,
   MODULE_VOI_LUT                                                      =   13,
   MODULE_SOP_COMMON                                                   =   14,
   MODULE_FRAME_OF_REFERENCE                                           =   15,
   MODULE_IMAGE_PLANE                                                  =   16,
   MODULE_CT_IMAGE                                                     =   17,
   MODULE_MR_IMAGE                                                     =   18,
   MODULE_NM_PET_PATIENT_ORIENTATION                                   =   19,
   MODULE_NM_IMAGE_PIXEL                                               =   20,
   MODULE_MULTI_FRAME                                                  =   21,
   MODULE_NM_MULTI_FRAME                                               =   22,
   MODULE_NM_IMAGE                                                     =   23,
   MODULE_NM_ISOTOPE                                                   =   24,
   MODULE_NM_DETECTOR                                                  =   25,
   MODULE_NM_TOMO_ACQUISITION                                          =   26,
   MODULE_NM_MULTI_GATED_ACQUISITION                                   =   27,
   MODULE_NM_PHASE                                                     =   28,
   MODULE_NM_RECONSTRUCTION                                            =   29,
   MODULE_MULTI_FRAME_OVERLAY                                          =   30,
   MODULE_US_FRAME_OF_REFERENCE                                        =   31,   // v16 not used -- retired
   MODULE_PALETTE_COLOR_LOOKUP_TABLE                                   =   32,
   MODULE_PALETTE_COLOR_LOOCKUP_TABLE                                  = MODULE_PALETTE_COLOR_LOOKUP_TABLE,
   MODULE_US_REGION_CALIBRATION                                        =   33,
   MODULE_US_IMAGE                                                     =   34,
   MODULE_CURVE_IDENTIFICATION                                         =   35,
   MODULE_AUDIO                                                        =   36,
   MODULE_CINE                                                         =   37,
   MODULE_SC_EQUIPMENT                                                 =   38,
   MODULE_SC_IMAGE                                                     =   39,
   MODULE_OVERLAY_IDENTIFICATION                                       =   40,
   MODULE_PATIENT_SUMMARY                                              =   41,
   MODULE_STUDY_CONTENT                                                =   42,
   MODULE_LUT_IDENTIFICATION                                           =   43,
   MODULE_FRAME_POINTERS                                               =   44,
   MODULE_MASK                                                         =   45,
   MODULE_DISPLAY_SHUTTER                                              =   46,
   MODULE_DEVICE                                                       =   47,
   MODULE_THERAPY                                                      =   48,
   MODULE_XA_IMAGE                                                     =   49,
   MODULE_XA_ACQUISITION                                               =   50,
   MODULE_XA_COLLIMATOR                                                =   51,
   MODULE_XA_TABLE                                                     =   52,
   MODULE_XA_POSITIONER                                                =   53,
   MODULE_BIPLANE_SEQUENCE                                             =   54,
   MODULE_BIPLANE_OVERLAY                                              =   55,
   MODULE_BIPLANE_IMAGE                                                =   56,
   MODULE_XRF_POSITIONER                                               =   57,
   MODULE_X_RAY_TOMO_ACQUISITION                                        =   58,   //misnamed
   MODULE_XRF_TOMO_ACQUISITION                                         = MODULE_X_RAY_TOMO_ACQUISITION,
   MODULE_RT_SERIES                                                    =   59,
   MODULE_RT_IMAGE                                                     =   60,
   MODULE_APPROVAL                                                     =   61,
   MODULE_RT_DOSE                                                      =   62,
   MODULE_RT_DVH                                                       =   63,
   MODULE_STRUCTURE_SET                                                =   64,
   MODULE_ROI_CONTOUR                                                  =   65,
   MODULE_RT_DOSE_ROI                                                  =   66,
   MODULE_RT_ROI_OBSERVATIONS                                          =   67,   // misnamed
   MODULE_RT_OBSERVATIONS                                              = MODULE_RT_ROI_OBSERVATIONS,
   MODULE_RT_GENERAL_PLAN                                              =   68,
   MODULE_RT_PRESCRIPTION                                              =   69,
   MODULE_RT_TOLERANCE_TABLES                                          =   70,
   MODULE_RT_PATIENT_SETUP                                             =   71,
   MODULE_RT_FRACTION_SCHEME                                           =   72,
   MODULE_RT_BEAMS                                                     =   73,
   MODULE_RT_BRACHY_APPLICATION_SETUPS                                 =   74,
   MODULE_PET_SERIES                                                   =   75,
   MODULE_PET_ISOTOPE                                                  =   76,
   MODULE_PET_MULTIGATED_ACQUISITION                                   =   77,
   MODULE_PET_IMAGE                                                    =   78,
   MODULE_PET_CURVE                                                    =   79,
   MODULE_PRINTER_CHARACTERISTICS                                      =   80,
   MODULE_FILM_BOX                                                     =   81,
   MODULE_IMAGE_BOX_LIST                                               =   82,
   MODULE_ANNOTATION_LIST                                              =   83,
   MODULE_IMAGE_OVERLAY_BOX_LIST                                       =   84,
   MODULE_PRESENTATION_LUT_LIST                                        =   85,
   MODULE_HC_EQUIPMENT                                                 =   86,
   MODULE_HC_GRAYSCALE_IMAGE                                           =   87,
   MODULE_HC_COLOR_IMAGE                                               =   88,
   MODULE_PATIENT_RELATIONSHIP                                         =   89,
   MODULE_PATIENT_IDENTIFICATION                                       =   90,
   MODULE_PATIENT_DEMOGRAPHIC                                          =   91,
   MODULE_PATIENT_MEDICAL                                              =   92,
   MODULE_VISIT_RELATIONSHIP                                           =   93,
   MODULE_VISIT_IDENTIFICATION                                         =   94,
   MODULE_VISIT_ADMISSION                                              =   95,
   MODULE_VISIT_STATUS                                                 =   96,
   MODULE_VISIT_DISCHARGE                                              =   97,
   MODULE_VISIT_SCHEDULING                                             =   98,
   MODULE_STUDY_RELATIONSHIP                                           =   99,
   MODULE_STUDY_IDENTIFICATION                                         =  100,
   MODULE_STUDY_CLASSIFICATION                                         =  101,
   MODULE_STUDY_SCHEDULING                                             =  102,
   MODULE_STUDY_ACQUISITION                                            =  103,
   MODULE_STUDY_READ                                                   =  104,
   MODULE_STUDY_COMPONENT_RELATIONSHIP                                 =  105,
   MODULE_STUDY_COMPONENT_ACQUISITION                                  =  106,
   MODULE_STUDY_COMPONENT                                              =  107,
   MODULE_RESULTS_RELATIONSHIP                                         =  108,
   MODULE_RESULTS_IDENTIFICATION                                       =  109,
   MODULE_RESULTS_IMPRESSIONS                                          =  110,
   MODULE_INTERPRETATION_RELATIONSHIP                                  =  111,
   MODULE_INTERPRETATION_IDENTIFICATION                                =  112,
   MODULE_INTERPRETATION_STATE                                         =  113,
   MODULE_INTERPRETATION_RECORDING                                     =  114,
   MODULE_INTERPRETATION_TRANSCRIPTION                                 =  115,
   MODULE_INTERPRETATION_APPROVAL                                      =  116,
   MODULE_BASIC_FILM_SESSION_PRESENTATION                              =  117,
   MODULE_BASIC_FILM_SESSION_RELATIONSHIP                              =  118,
   MODULE_BASIC_FILM_BOX_PRESENTATION                                  =  119,
   MODULE_BASIC_FILM_BOX_RELATIONSHIP                                  =  120,
   MODULE_IMAGE_BOX_PRESENTATION_GRAYSCALE                             =  121,
   MODULE_IMAGE_BOX_PRESENTATION_COLOR                                 =  122,
   MODULE_IMAGE_BOX_RELATIONSHIP                                       =  123,
   MODULE_BASIC_ANNOTATION_PRESENTATION                                =  124,
   MODULE_PRINT_JOB                                                    =  125,
   MODULE_PRINTER                                                      =  126,
   MODULE_IMAGE_OVERLAY_BOX_PRESENTATION                               =  127,
   MODULE_IMAGE_OVERLAY_BOX_RELATIONSHIP                               =  128,
   MODULE_STORAGE_COMMITMENT                                           =  129,
   MODULE_GENERAL_QUEUE                                                =  130,
   MODULE_PRINT_QUEUE                                                  =  131,
   MODULE_PERFORMED_PROCEDURE_STEP_RELATIONSHIP                        =  132,
   MODULE_PERFORMED_PROCEDURE_STEP_INFORMATION                         =  133,
   MODULE_IMAGE_ACQUISITION_RESULTS                                    =  134,
   MODULE_RADIATION_DOSE                                               =  135,
   MODULE_BILLING_AND_MATERIAL_MANAGEMENT_CODES                        =  136,
   MODULE_PRESENTATION_LUT                                             =  137,
   MODULE_PRINT_REQUEST                                                =  138,
   MODULE_PATIENT_RELATIONSHIP_META                                    =  139,   // v16 not used, v15 not used
   MODULE_NM_SERIES_RETIRED                                            =  140,
   MODULE_NM_EQUIPMENT_RETIRED                                         =  141,
   MODULE_NM_IMAGE_RETIRED                                             =  142,
   MODULE_NM_SPECT_ACQUISITION_IMAGE_RETIRED                           =  143,
   MODULE_NM_MULTI_GATED_ACQUISITION_IMAGE_RETIRED                     =  144,
   MODULE_US_FRAME_OF_REFERENCE_RETIRED                                =  145,
   MODULE_US_REGION_CALIBRATION_RETIRED                                =  146,
   MODULE_US_IMAGE_RETIRED                                             =  147,
   MODULE_FILESET_IDENTIFICATION                                       =  148,
   MODULE_DIRECTORY_INFORMATION                                        =  149,
   MODULE_PATIENT_KEY                                                  =  150,
   MODULE_STUDY_KEY                                                    =  151,
   MODULE_SERIES_KEY                                                   =  152,
   MODULE_IMAGE_KEY                                                    =  153,
   MODULE_OVERLAY_KEY                                                  =  154,
   MODULE_MODALITY_LUT_KEY                                             =  155,
   MODULE_VOI_LUT_KEY                                                  =  156,
   MODULE_CURVE_KEY                                                    =  157,
   MODULE_STORED_PRINT_KEY                                             =  158,
   MODULE_RT_DOSE_KEY                                                  =  159,
   MODULE_RT_STRUCTURE_SET_KEY                                         =  160,
   MODULE_RT_PLAN_KEY                                                  =  161,
   MODULE_RT_TREATMENT_RECORD_KEY                                      =  162,
   MODULE_TOPIC_KEY                                                    =  163,
   MODULE_VISIT_KEY                                                    =  164,
   MODULE_RESULTS_KEY                                                  =  165,
   MODULE_INTERPRETATION_KEY                                           =  166,
   MODULE_STUDY_COMPONENT_KEY                                          =  167,
   MODULE_PRIVATE_KEY                                                  =  168,
   MODULE_SPECIMEN_IDENTIFICATION                                      =  169,
   MODULE_DX_SERIES                                                    =  170,
   MODULE_DX_ANATOMY_IMAGED                                            =  171,
   MODULE_DX_IMAGE                                                     =  172,
   MODULE_DX_DETECTOR                                                  =  173,
   MODULE_DX_POSITIONING                                               =  174,
   MODULE_ACQUISITION_CONTEXT                                          =  175,
   MODULE_XA_ACQUISITION_DOSE                                          =  176,   
   MODULE_XA_GENERATION                                                =  177,   
   MODULE_XA_FILTRATION                                                =  178,   
   MODULE_XA_GRID                                                      =  179,   
   MODULE_IMAGE_HISTOGRAM                                              =  180,
   MODULE_MAMMOGRAPHY_SERIES                                           =  181,
   MODULE_MAMMOGRAPHY_IMAGE                                            =  182,
   MODULE_INTRAORAL_SERIES                                             =  183,   
   MODULE_INTRAORAL_IMAGE                                              =  184,   
   MODULE_RT_GENERAL_TREATMENT_RECORD                                  =  185,
   MODULE_RT_TREATMENT_MACHINE_RECORD                                  =  186,
   MODULE_MEASURED_DOSE_REFERENCE_RECORD                               =  187,
   MODULE_CALCULATED_DOSE_REFERENCE_RECORD                             =  188,
   MODULE_RT_BEAMS_SESSION_RECORD                                      =  189,
   MODULE_RT_TREATMENT_SUMMARY_RECORD                                  =  190,
   MODULE_RT_BRACHY_SESSION_RECORD                                     =  191,
   MODULE_VL_IMAGE                                                     =  192,
   MODULE_SLIDE_COORDINATES                                            =  193,
   MODULE_BASIC_PRINT_IMAGE_OVERLAY_BOX                                =  194,
   MODULE_PRINTER_CONFIGURATION                                        =  195,
   MODULE_SR_DOCUMENT_SERIES                                           =  196,
   MODULE_SR_DOCUMENT_GENERAL                                          =  197,
   MODULE_SR_DOCUMENT_CONTENT                                          =  198,
   MODULE_SR_DOCUMENT_KEY                                              =  199,
   MODULE_SCHEDULED_PROCEDURE_STEP                                     =  200,
   MODULE_REQUESTED_PROCEDURE                                          =  201,
   MODULE_IMAGING_SERVICE_REQUEST                                      =  202,
   MODULE_PRESENTATION_SERIES                                          =  203,
   MODULE_PRESENTATION_STATE                                           =  204,   
   MODULE_BITMAP_DISPLAY_SHUTTER                                       =  205,
   MODULE_OVERLAY_CURVE_ACTIVATION                                     =  206,   
   MODULE_DISPLAYED_AREA                                               =  207,
   MODULE_GRAPHIC_ANNOTATION                                           =  208,
   MODULE_SPATIAL_TRANSFORMATION                                       =  209,
   MODULE_GRAPHIC_LAYER                                                =  210,
   MODULE_SOFTCOPY_VOI_LUT                                             =  211,
   MODULE_SOFTCOPY_PRESENTATION_LUT                                    =  212,
   MODULE_SYNCHRONIZATION                                              =  213,
   MODULE_WAVEFORM_IDENTIFICATION                                      =  214,
   MODULE_WAVEFORM                                                     =  215,
   MODULE_WAVEFORM_ANNOTATION                                          =  216,
   MODULE_PRESENTATION_KEY                                             =  217,
   MODULE_WAVEFORM_KEY                                                 =  218,
   MODULE_CLINICAL_TRIAL_SUBJECT                                       =  219,
   MODULE_CLINICAL_TRIAL_STUDY                                         =  220,
   MODULE_CLINICAL_TRIAL_SERIES                                        =  221,
   MODULE_MULTI_FRAME_FUNCTIONAL_GROUPS                                =  222,
   MODULE_MULTI_FRAME_DIMENSION                                        =  223,
   MODULE_CARDIAC_SYNCHRONIZATION                                      =  224,
   MODULE_RESPIRATORY_SYNCHRONIZATION                                  =  225,
   MODULE_BULK_MOTION_SYNCHRONIZATION                                  =  226,
   MODULE_SUPPLEMENTAL_PALETTE_COLOR_LOOKUP_TABLE                      =  227,
   MODULE_ENHANCED_MR_IMAGE                                            =  228,
   MODULE_MR_PULSE_SEQUENCE                                            =  229,
   MODULE_MR_SPECTROSCOPY                                              =  230,
   MODULE_MR_SPECTROSCOPY_PULSE_SEQUENCE                               =  231,
   MODULE_MR_SPECTROSCOPY_DATA                                         =  232,
   MODULE_RAW_DATA                                                     =  233,
   MODULE_SC_MULTI_FRAME_IMAGE                                         =  234,
   MODULE_SC_MULTI_FRAME_VECTOR                                        =  235,
   MODULE_KEY_OBJECT_DOCUMENT_SERIES                                   =  236,
   MODULE_KEY_OBJECT_DOCUMENT                                          =  237,
   MODULE_GENERAL_PURPOSE_SCHEDULED_PROCEDURE_STEP_RELATIONSHIP        =  238,
   MODULE_GENERAL_PURPOSE_SCHEDULED_PROCEDURE_STEP_INFORMATION         =  239,
   MODULE_GENERAL_PURPOSE_PERFORMED_PROCEDURE_STEP_RELATIONSHIP        =  240,
   MODULE_GENERAL_PURPOSE_PERFORMED_PROCEDURE_STEP_INFORMATION         =  241,
   MODULE_GENERAL_PURPOSE_RESULTS                                      =  242,
   MODULE_KEY_OBJECT_DOCUMENT_KEY                                      =  243,
   MODULE_OPHTHALMIC_PHOTOGRAPHY_SERIES                                =  244,
   MODULE_OPHTHALMIC_PHOTOGRAPHY_IMAGE                                 =  245,
   MODULE_OPHTHALMIC_PHOTOGRAPHIC_PARAMETERS                           =  246,
   MODULE_OPHTHALMIC_PHOTOGRAPHY_ACQUISITION_PARAMETERS                =  247,
   MODULE_OCULAR_REGION_IMAGED                                         =  248,
   MODULE_STEREOMETRIC_SERIES                                          =  249,
   MODULE_STEREOMETRIC_RELATIONSHIP                                    =  250,   
   MODULE_ENHANCED_CONTRAST_BOLUS                                      =  251,
   MODULE_COMMON_INSTANCE_REFERENCE                                    =  252,
   MODULE_UNDEFINED                                                    =  253,   

#if !defined(LEADTOOLS_V16_OR_LATER)
   MODULE_MAX                                                          =  254,
#endif

#if defined(LEADTOOLS_V16_OR_LATER)
   MODULE_INTERVENTION                                                  =  254,
   MODULE_PRESENTATION_STATE_RELATIONSHIP                               =  255,
   MODULE_PRESENTATION_STATE_SHUTTER                                    =  256,
   MODULE_PRESENTATION_STATE_MASK                                       =  257,
   MODULE_ICC_PROFILE                                                   =  258,
   MODULE_PRESENTATION_STATE_BLENDING                                   =  259,
   MODULE_MR_SERIES                                                     =  260,
   MODULE_ENHANCED_GENERAL_EQUIPMENT                                    =  261,
   MODULE_CT_SERIES                                                     =  262,
   MODULE_ENHANCED_CT_IMAGE                                             =  263,
   MODULE_SPATIAL_REGISTRATION_SERIES                                   =  264,
   MODULE_SPATIAL_REGISTRATION                                          =  265,
   MODULE_DEFORMABLE_SPATIAL_REGISTRATION                               =  266,
   MODULE_SPATIAL_FIDUCIALS_SERIES                                      =  267,
   MODULE_SPATIAL_FIDUCIALS                                             =  268,
   MODULE_HANGING_PROTOCOL_DEFINITION                                   =  269,
   MODULE_HANGING_PROTOCOL_ENVIRONMENT                                  =  270,
   MODULE_HANGING_PROTOCOL_DISPLAY                                      =  271,
   MODULE_ENCAPSULATED_DOCUMENT_SERIES                                  =  272,
   MODULE_ENCAPSULATED_DOCUMENT                                         =  273,
   MODULE_REAL_WORLD_VALUE_MAPPING_SERIES                               =  274,
   MODULE_REAL_WORLD_VALUE_MAPPING                                      =  275,
   MODULE_XA_XRF_SERIES                                                 =  276,
   MODULE_ENHANCED_XA_XRF_IMAGE                                         =  277,
   MODULE_XA_XRF_ACQUISITION                                            =  278,
   MODULE_X_RAY_IMAGE_INTENSIFIER                                       =  279,
   MODULE_X_RAY_DETECTOR                                                =  280,
   MODULE_XA_XRF_MULTI_FRAME_PRESENTATION                               =  281,
   MODULE_RT_ION_TOLERANCE_TABLES                                       =  282,
   MODULE_RT_ION_BEAMS                                                  =  283,
   MODULE_RT_ION_BEAMS_SESSION_RECORD                                   =  284,
   MODULE_SEGMENTATION_SERIES                                           =  285,
   MODULE_SEGMENTATION_IMAGE                                            =  286,
   MODULE_OPHTHALMIC_TOMOGRAPHY_SERIES                                  =  287,
   MODULE_OPHTHALMIC_TOMOGRAPHY_IMAGE                                   =  288,
   MODULE_OPHTHALMIC_TOMOGRAPHY_ACQUISITION_PARAMETERS                  =  289,
   MODULE_OPHTHALMIC_TOMOGRAPHY_PARAMETERS                              =  290,
   MODULE_ENHANCED_SERIES                                               =  291,
   MODULE_PATIENT_ORIENTATION                                           =  292,
   MODULE_IMAGE_EQUIPMENT_COORDINATE_RELATIONSHIP                       =  293,
   MODULE_X_RAY_3D_IMAGE                                                =  294,
   MODULE_X_RAY_3D_ANGIOGRAPHIC_IMAGE_CONTRIBUTING_SOURCES              =  295,
   MODULE_X_RAY_3D_ANGIOGRAPHIC_ACQUISITION                             =  296,
   MODULE_X_RAY_3D_RECONSTRUCTION                                       =  297,
   MODULE_X_RAY_3D_CRANIOFACIAL_IMAGE_CONTRIBUTING_SOURCES              =  298,
   MODULE_X_RAY_3D_CRANIOFACIAL_ACQUISITION                             =  299,
   MODULE_ENHANCED_PET_SERIES                                           =  300,
   MODULE_ENHANCED_PET_ISOTOPE_MODULE                                   =  301,
   MODULE_ENHANCED_PET_ACQUISITION                                      =  302,
   MODULE_ENHANCED_PET_IMAGE                                            =  303,

   // sup117_ft.doc
   // sup130_ft.doc
   MODULE_LENSOMETRY_MEASUREMENTS_SERIES                                =  304,
   MODULE_GENERAL_OPHTHALMIC_REFRACTIVE_MEASUREMENTS                    =  305,
   MODULE_LENSOMETRY_MEASUREMENTS                                       =  306,
   MODULE_AUTOREFRACTION_MEASUREMENTS_SERIES                            =  307,
   MODULE_AUTOREFRACTION_MEASUREMENTS                                   =  308,
   MODULE_KERATOMETRY_MEASUREMENTS_SERIES                               =  309,
   MODULE_KERATOMETRY_MEASUREMENTS                                      =  310,
   MODULE_SUBJECTIVE_REFRACTION_MEASUREMENTS_SERIES                     =  311,
   MODULE_SUBJECTIVE_REFRACTION_MEASUREMENTS                            =  312,
   MODULE_VISUAL_ACUITY_MEASUREMENTS_SERIES                             =  313,
   MODULE_VISUAL_ACUITY_MEASUREMENTS                                    =  314,

   // Part4
   MODULE_SUBSTANCE_ADMINISTRATION_LOGGING                              =  315,
   MODULE_MODALITY_HANGING_PROTOCOL                                     =  316,
   MODULE_PRODUCT_CHARACTERISTICS                                       =  317,
   MODULE_SUBSTANCE_APPROVAL_PATIENT                                    =  318,
   MODULE_SUBSTANCE_APPROVAL_VISIT                                      =  319,
   MODULE_SUBSTANCE_APPROVAL_PRODUCT                                    =  320,
   MODULE_SUBSTANCE_ADMINISTRATION                                      =  321,
   MODULE_SUBSTANCE_APPROVAL                                            =  322,

      // New Modules begin here
   MODULE_BASIC_FILM_BOX_PRESENTATION_MODULE                            =  323,
   MODULE_IMAGE_BOX_PRESENTATION_MODULE                                 =  324,
   MODULE_SOP_COMMON_INFORMATION                                        =  325,
   MODULE_INSTANCE_AVAILABILITY_NOTIFICATION                            =  326,
   MODULE_MEDIA_CREATION_MANAGEMENT                                     =  327,

   // New MODULE_XXXX_KEY
   MODULE_SPECTROSCOPY_KEY                                              =  328,
   MODULE_RAW_DATA_KEY                                                  =  329,
   MODULE_REGISTRATION_KEY                                              =  330,
   MODULE_FIDUCIAL_KEY                                                  =  331,
   MODULE_HANGING_PROTOCOL_KEY                                          =  332,
   MODULE_ENCAPSULATED_DOCUMENT_KEY                                     =  333,
   MODULE_HL7_STRUCTURED_DOCUMENT_KEY                                   =  334,
   MODULE_REAL_WORLD_VALUE_MAPPING_KEY                                  =  335,
   MODULE_STEREOMETRIC_RELATIONSHIP_KEY                                 =  336,


   MODULE_STRUCTURED_DISPLAY                                            =  337,
   MODULE_STRUCTURED_DISPLAY_IMAGE_BOX                                  =  338,
   MODULE_STRUCTURED_DISPLAY_ANNOTATION                                 =  339,

   // All 2008 supplements go here
   MODULE_ENHANCED_US_SERIES                                            =  340,
   MODULE_ULTRASOUND_FRAME_OF_REFERENCE                                 =  341,
   MODULE_ENHANCED_PALETTE_COLOR_LOOKUP_TABLE                           =  342,
   MODULE_ENHANCED_US_IMAGE                                             =  343,
   MODULE_IVUS_IMAGE                                                    =  344, 
   MODULE_EXCLUDED_INTERVALS                                            =  345,
   MODULE_SPECIMEN                                                      =  346,
   MODULE_ENHANCED_MAMMOGRAPHY_SERIES                                   =  347,
   MODULE_BREAST_TOMOSYNTHESIS_CONTRIBUTING_SOURCES                     =  348,
   MODULE_BREAST_TOMOSYNTHESIS_ACQUISITION                              =  349,
   MODULE_BREAST_VIEW                                                   =  350,
   MODULE_SURFACE_SEGMENTATION                                          =  351,
   MODULE_SURFACE_MESH                                                  =  352,
   MODULE_COLOR_PALETTE_DEFINITION                                      =  353,
   MODULE_MAX                                                           =  354,


   // For v15 backward compatibility
   MODULE_BASIC_ANNOTATION_PRESENTATION_MODULE                         =  MODULE_BASIC_ANNOTATION_PRESENTATION,
   MODULE_PRINT_JOB_MODULE                                             =  MODULE_PRINT_JOB,
   MODULE_PRESENTATION_LUT_MODULE                                      =  MODULE_PRESENTATION_LUT,
   MODULE_X_RAY_ACQUISITION_DOSE                                       =  MODULE_XA_ACQUISITION_DOSE,
   MODULE_X_RAY_GENERATION                                             =  MODULE_XA_GENERATION,
   MODULE_X_RAY_FILTRATION                                             =  MODULE_XA_FILTRATION,
   MODULE_X_RAY_GRID                                                   =  MODULE_XA_GRID,
   MODULE_INTRA_ORAL_SERIES                                            =  MODULE_INTRAORAL_SERIES,
   MODULE_PRESENTATION_STATE_IDENTIFICATION                            =  MODULE_PRESENTATION_STATE,
   MODULE_OVERLAY_ACTIVATION                                           =  MODULE_OVERLAY_CURVE_ACTIVATION,
   MODULE_STEREOMETRIC_RELATIONSHIP_MODULE                             =  MODULE_STEREOMETRIC_RELATIONSHIP,
   MODULE_INTRA_ORAL_IMAGE                                             =  MODULE_INTRAORAL_IMAGE,
   MODULE_PET_MULTI_GATED_ACQUISITION                                  =  MODULE_PET_MULTIGATED_ACQUISITION,
   MODULE_PRINTER_MODULE                                               =  MODULE_PRINTER,
   MODULE_X_RAY_IMAGE                                                  =  MODULE_XA_IMAGE,
   MODULE_X_RAY_ACQUISITION                                            =  MODULE_XA_ACQUISITION,
   MODULE_X_RAY_COLLIMATOR                                             =  MODULE_XA_COLLIMATOR,
   MODULE_X_RAY_TABLE                                                  =  MODULE_XA_TABLE,
   MODULE_PALETTE_COLOR_LUT                                            =  MODULE_PALETTE_COLOR_LOOKUP_TABLE,
   MODULE_COMMON_INSTANCE_REFERENCE_MODULE                             =  MODULE_COMMON_INSTANCE_REFERENCE,
   MODULE_MULTI_FRAME_DIMENSION_MODULE                                 =  MODULE_MULTI_FRAME_DIMENSION,
   MODULE_X_RAY_TOMOGRAPHY_ACQUISITION                                 =  MODULE_XRF_TOMO_ACQUISITION,








#endif
};


//============= CLASS ==========================================================

#define MAKETAG(nGroup,nElement) ((L_UINT32)((((L_UINT32)nGroup) << 16) | nElement))
#define GETGROUP(nTag)           ((L_UINT16)(nTag >> 16))
#define GETELEMENT(nTag)         ((L_UINT16)(nTag & 0xFFFF))

#define DS_METAHEADER_PRESENT             0x0001U
#define DS_METAHEADER_ABSENT              0x0002U
#define DS_LITTLE_ENDIAN                  0x0004U
#define DS_BIG_ENDIAN                     0x0008U
#define DS_IMPLICIT_VR                    0x0010U
#define DS_EXPLICIT_VR                    0x0020U
#define DS_GROUP_LENGTHS                  0x0040U
#define DS_LENGTH_EXPLICIT                0x0080U
#define DS_EXCLUDE_METAHEADER_GROUP       0x0100U
#define DS_LOAD_CLOSE                     0x0200U
#define DS_KEEP_PIXEL_DATA_INTACT         0x0400U
#define DS_LOAD_STORE_OFFSETS             0x0800U
#define DS_ADD_MANDATORY_ELEMENTS_ONLY    0x1000U

#if defined(LEADTOOLS_V16_OR_LATER)
#define DS_ADD_MANDATORY_MODULES_ONLY     0x2000U
#endif
enum
{
   IMAGE_COMPRESSION_NONE,
   IMAGE_COMPRESSION_RLE,
   IMAGE_COMPRESSION_JPEG_LOSSLESS,
   IMAGE_COMPRESSION_JPEG_LOSSY,
   IMAGE_COMPRESSION_JPEG_LS_LOSSLESS,
   IMAGE_COMPRESSION_JPEG_LS_LOSSY,
   IMAGE_COMPRESSION_J2K_LOSSLESS,
   IMAGE_COMPRESSION_J2K_LOSSY,
   IMAGE_COMPRESSION_MPEG2,
   IMAGE_COMPRESSION_MPEG2_HD,
};


enum
{
   IMAGE_PHOTOMETRIC_MONOCHROME1,
   IMAGE_PHOTOMETRIC_MONOCHROME2,
   IMAGE_PHOTOMETRIC_PALETTE_COLOR,
   IMAGE_PHOTOMETRIC_RGB,
   IMAGE_PHOTOMETRIC_ARGB,
   IMAGE_PHOTOMETRIC_CMYK,
   IMAGE_PHOTOMETRIC_YBR_FULL_422,
   IMAGE_PHOTOMETRIC_YBR_FULL,
   IMAGE_PHOTOMETRIC_YBR_RCT,
   IMAGE_PHOTOMETRIC_YBR_ICT
};



#define DS_PREAMBLE_LENGTH             128
#define DS_PREFIX_LENGTH               4
#define DS_PREFIX                      "DICM"

#define ELEMENT_LENGTH_MAX             0xFFFFFFFFUL
#define ELEMENT_INDEX_MAX              0xFFFFFFFFUL

//typedef struct _DICOMELEMENT DICOMELEMENT, *pDICOMELEMENT;
typedef struct _DICOMELEMENT
{
   GENERICLINK           // Reserved - internally used only

   L_UINT32    nTag;      // Data Element Tag
   L_UINT16    nVR;       // Value Representation
   L_UINT32    nLength;
   
   L_UINT32    nVM;           // Reserved - internally used only
   L_UCHAR     *pValue;       // Reserved - internally used only
   L_UINT32    nAttach;       // Reserved - internally used only
   L_UINT32    *pOffset;      // Reserved - internally used only
   L_VOID      **pFile;       // Reserved - internally used only
   L_UINT32    *pSize;        // Reserved - internally used only
   L_UINT32    nCount;        // Reserved - internally used only
   L_TCHAR     *pStringValue; // Reserved - internally used only
} DICOMELEMENT, *pDICOMELEMENT;

typedef struct _DICOMELEMENTOFFSET
{
   GENERICLINK    // Reserved - internally used only
   pDICOMELEMENT  pElement;
   L_UINT32       nOffset;   // Reserved - internally used only   
} DICOMELEMENTOFFSET, *pDICOMELEMENTOFFSET;


//typedef struct _DICOMMODULE DICOMMODULE, *pDICOMMODULE;
typedef struct _DICOMMODULE
{
   L_UINT32      nModule;         // Module code
   L_UINT32      nCount;          // Number of elements
   pDICOMELEMENT pElement[1000];  // Pointers to elements
} DICOMMODULE, *pDICOMMODULE;

#define VALUE_AGE_DAYS   'D'
#define VALUE_AGE_WEEKS  'W'
#define VALUE_AGE_MONTHS 'M'
#define VALUE_AGE_YEARS  'Y'

typedef struct _VALUEAGE
{
   L_UINT16    nNumber;       // A value
   L_UCHAR     nReference;    // Reference of age (VALUE_AGE_DAYS, VALUE_AGE_WEEKS, ...)
} VALUEAGE, *  pVALUEAGE;

typedef struct _VALUEDATE
{
   L_UINT16 nYear;         // Year
   L_UINT16 nMonth;        // Month
   L_UINT16 nDay;          // Day
} VALUEDATE, *pVALUEDATE;

#define VALUE_RANGE_NONE   0 //only Date1 is valid, represents single date, not a range
#define VALUE_RANGE_LOWER  1 //only Date1 is valid, represents lower range, "1/1/01-"
#define VALUE_RANGE_UPPER  2 //only Date1 is valid, represents upper range, "-9/9/01"
#define VALUE_RANGE_BOTH   3 //Date1 & Date2 valid, represents full range,  "1/1/01-9/9/01"


typedef struct _VALUETIME
{
   L_UINT16 nHours;        // Hours
   L_UINT16 nMinutes;      // Minutes
   L_UINT16 nSeconds;      // Seconds
   L_UINT32 nFractions;    // Fractional Second
} VALUETIME, *pVALUETIME;

typedef struct _VALUEDATETIME
{
   L_UINT16 nYear;         // Year
   L_UINT16 nMonth;        // Month
   L_UINT16 nDay;          // Day
   L_UINT16 nHours;        // Hours
   L_UINT16 nMinutes;      // Minutes
   L_UINT16 nSeconds;      // Seconds
   L_UINT32 nFractions;    // Fractional Second
   L_INT32  nOffset;       // Optional suffix for plus/minus offset from Coordinated Universal Time
} VALUEDATETIME, *pVALUEDATETIME;

typedef struct _VALUEDATERANGE
{
   L_UINT32 nFlags;        // Flags
   VALUEDATE Date1;        // Date1
   VALUEDATE Date2;        // Date2
} VALUEDATERANGE, *pVALUEDATERANGE;

typedef struct _VALUETIMERANGE
{
   L_UINT32 nFlags;        // Flags
   VALUETIME Time1;        // Time1
   VALUETIME Time2;        // Time2
} VALUETIMERANGE, *pVALUETIMERANGE;

typedef struct _VALUEDATETIMERANGE
{
   L_UINT32 nFlags;         // Flags
   VALUEDATETIME DateTime1; // DateTime1
   VALUEDATETIME DateTime2; // DateTime2
} VALUEDATETIMERANGE, *pVALUEDATETIMERANGE;

typedef struct _DICOMIMAGE
{
   L_INT32  nCompression;
   L_TCHAR  szPhotometric[30];
   L_INT32  nPhotometric;
   L_UINT32 nSamplesPerPixel;
   L_INT32  nRows;
   L_INT32  nColumns;
   L_UINT32 nBitsAllocated;
   L_UINT32 nBitsStored;
   L_UINT32 nHighBit;
   L_INT32  nPixelRepresentation;
   L_INT32  nPlanarConfiguration;
   L_INT32  nResolutionX;
   L_INT32  nResolutionY;
   L_INT32  nSmallestImagePixelValue;
   L_BOOL   bSmallestImagePixelValue;
   L_INT32  nLargestImagePixelValue;
   L_BOOL   bLargestImagePixelValue;
   L_UINT32 nRedEntries;
   L_UINT32 nRedFirst;
   L_UINT32 nRedBits;
   L_UINT32 nGreenEntries;
   L_UINT32 nGreenFirst;
   L_UINT32 nGreenBits;
   L_UINT32 nBlueEntries;
   L_UINT32 nBlueFirst;
   L_UINT32 nBlueBits;
   L_UINT32 nPaletteEntries;
   L_UINT32 nPaletteFirst;
   L_INT32  nBitsPerPixel;
   L_BOOL   bGray;
   L_UINT32 nFrames;
} DICOMIMAGE, *pDICOMIMAGE;

typedef struct _IMAGEINFO IMAGEINFO, *pIMAGEINFO;

typedef struct _DICOMNETDEBUGINFOONSEND
{
   L_INT       nSize ;// The size of this structure
   L_INT       nError;// error code
   L_UCHAR     nType ;// type of message/data sent
   L_UINT32    nBytes;// number of bytes of data sent
   L_UCHAR     *pSentData;//Data itself
}DICOMNETDEBUGINFOONSEND, *pDICOMNETDEBUGINFOONSEND;

// Data Commands
#define COMMAND_C_STORE           0x0001
#define COMMAND_C_FIND            0x0020
#define COMMAND_C_GET             0x0010
#define COMMAND_C_MOVE            0x0021
#define COMMAND_C_CANCEL          0x0FFF
#define COMMAND_C_ECHO            0x0030
#define COMMAND_N_REPORT          0x0100
#define COMMAND_N_GET             0x0110
#define COMMAND_N_SET             0x0120
#define COMMAND_N_ACTION          0x0130
#define COMMAND_N_CREATE          0x0140
#define COMMAND_N_DELETE          0x0150

#define COMMAND_REQUEST           0x0000
#define COMMAND_RESPONSE          0x8000

// Values for the element TAG_DATA_SET_TYPE
#define COMMAND_DATASET_PRESENT    0x0000
#define COMMAND_DATASET_IDENTIFIER 0x0102
#define COMMAND_DATASET_ABSENT     0x0101

// Values for the element TAG_STATUS
#define COMMAND_STATUS_SUCCESS                           0x0000   // Success
#define COMMAND_STATUS_CANCEL                            0xFE00   // Cancel
#define COMMAND_STATUS_ATTRIBUTE_LIST_ERROR              0x0107   // Attribute list error
#define COMMAND_STATUS_ATTRIBUTE_OUT_OF_RANGE            0x0116   // Attribute Value Out of Range
#define COMMAND_STATUS_CLASS_NOT_SUPPORTED               0x0122   // Class not supported
#define COMMAND_STATUS_CLASE_INSTANCE_CONFLICT           0x0119   // Class-instance conflict
#define COMMAND_STATUS_DUPLICATE_INSTANCE                0x0111   // Duplicate instance
#define COMMAND_STATUS_DUPLICATE_INVOCATION              0x0210   // Duplicate invocation
#define COMMAND_STATUS_INVALID_ARGUMENT_VALUE            0x0115   // Invalid argument value
#define COMMAND_STATUS_INVALID_ATTRIBUTE_VALUE           0x0106   // Invalid attribute value
#define COMMAND_STATUS_INVALID_OBJECT_INSTANCE           0x0117   // Ivalid object instance
#define COMMAND_STATUS_MISSING_ATTRIBUTE                 0x0120   // Missing attribute
#define COMMAND_STATUS_MISSING_ATTRIBUTE_VALUE           0x0121   // Missing attribute value
#define COMMAND_STATUS_MISTYPED_ARGUMENT                 0x0212   // Mistyped argument
#define COMMAND_STATUS_NO_SUCH_ARGUMENT                  0x0114   // No such argument
#define COMMAND_STATUS_NO_SUCH_ATTRIBUTE                 0x0105   // No such attribute
#define COMMAND_STATUS_NO_SUCH_EVENT_TYPE                0x0113   // No such event type
#define COMMAND_STATUS_NO_SUCH_OBJECT_INSTANCE           0x0112   // No such object instance
#define COMMAND_STATUS_NO_SUCH_CLASS                     0x0118   // No such class
#define COMMAND_STATUS_PROCESSING_FAILURE                0x0110   // Proccesing failure
#define COMMAND_STATUS_RESOURCE_LIMITATION               0x0213   // Resource limitation
#define COMMAND_STATUS_UNRECOGNIZED_OPERATION            0x0211   // Unrecognized operation
#define COMMAND_STATUS_REFUSED_OUT_OF_RESOURCES          0xA700   // Out of Resources
#define COMMAND_STATUS_REFUSED_UNABLE_CALCULATE_MATCHES  0xA701   // Out of Resources - unable to calculate number of matches
#define COMMAND_STATUS_REFUSED_UNABLE_PERFORM_SUB_OPS    0xA702   // Out of Resources - Unable to perform sub-operations
#define COMMAND_STATUS_REFUSED_MOVE_DESTINATION_UNKNOWN  0xA801   // Move Destination Unknown
#define COMMAND_STATUS_FAILURE                           0xC001   // Failure
#define COMMAND_STATUS_RESERVED2                         0xC002   // Reserved for future use
#define COMMAND_STATUS_RESERVED3                         0xC003   // Reserved for future use
#define COMMAND_STATUS_RESERVED4                         0xC004   // Reserved for future use
#define COMMAND_STATUS_WARNING                           0xB000   // Sub-operations Complete - One or more failures
#define COMMAND_STATUS_PENDING                           0xFF00   // Matches are continuing - Current Match is supplied and any Optional Keys were supported in the same manner as Required Keys.
#define COMMAND_STATUS_PENDING_WARNING                   0xFF01   // Matches are continuing - Warning that one or more Optional Keys were not supported for existence and/or matching for this Identifier.

// Values for the element TAG_PRIORITY
#define COMMAND_PRIORITY_LOW            0x0002
#define COMMAND_PRIORITY_MEDIUM         0x0000
#define COMMAND_PRIORITY_HIGH           0x0001


// Values for CONFORMANCECALLBACK
#define CALLBACK_ERROR_UNKNOWN_CLASS    0x0001
#define CALLBACK_ERROR_UNKNOWN_TAG      0x0002
#define CALLBACK_ERROR_UNKNOWN_VR       0x0004
#define CALLBACK_ERROR_WRONG_VR         0x0008
#define CALLBACK_ERROR_MIN_VM           0x0010
#define CALLBACK_ERROR_MAX_VM           0x0020
#define CALLBACK_ERROR_DIVIDE_VM        0x0040
#define CALLBACK_ERROR_IMAGE            0x0080
#define CALLBACK_ERROR_ELEMENT          0x0100
#define CALLBACK_ERROR_ELEMENT_EXISTS   0x0200
#define CALLBACK_ERROR_MEMORY           0x0400


#if !defined(HANNOBJECT)
typedef HANDLE HANNOBJECT;
#endif // #if !defined(HANNOBJECT)

#if !defined(pHANNOBJECT)
typedef HANNOBJECT* pHANNOBJECT;
#endif // #if !defined(pHANNOBJECT)

typedef L_BOOL (pEXT_CALLBACK CONFORMANCECALLBACK) (pDICOMELEMENT pElement, L_UINT16 nFlags, L_VOID *pUserData);
typedef L_BOOL (pEXT_CALLBACK COPYDSCALLBACK) (pDICOMELEMENT pElement, L_UINT16 nFlags, L_VOID *pUserData);

typedef L_VOID *HDICOMDS;
typedef L_VOID *HDICOMWAVEFORMGROUP;
typedef L_VOID *HDICOMWAVEFORMCHANNEL;

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSAVEMEMORY )(HANNOBJECT hObject,
                                    L_UINT uFormat,
                                    L_BOOL fSelected,
                                    HGLOBAL *phMem,
                                    L_SIZE_T *puMemSize,
                                    pSAVEFILEOPTION pSaveOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNLOADMEMORY )(L_UCHAR *pMem,
                                    L_SIZE_T uMemSize,
                                    pHANNOBJECT phObject,
                                    pLOADFILEOPTION pLoadOptions);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDELETEPAGEMEMORY )(HGLOBAL hMem, L_SIZE_T *puMemSize, L_INT32 nPage);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPOINTS )(HANNOBJECT hObject,
                                   pANNPOINT pPoints);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETPOINTCOUNT )(HANNOBJECT hObject,
                                       L_UINT *puCount);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETFILLMODE )(HANNOBJECT hObject,
                                     L_UINT *puFillMode,
                                     L_INT *pnAlpha);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXTLEN )(HANNOBJECT hObject,
                                    L_SIZE_T *puLen);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTEXT )(HANNOBJECT hObject,
                                 L_TCHAR *pText,
                                 L_SIZE_T *puLen);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETRECT )(HANNOBJECT hObject, 
                                 pANNRECT pRect, 
                                 pANNRECT pRectName);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNGETTYPE )(HANNOBJECT hObject,
                                 L_UINT *puObjectType);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNCREATE )(L_UINT uObjectType,
                                pHANNOBJECT phObject);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETPOINTS )(HANNOBJECT hObject,
                                   pANNPOINT pPoints,
                                   L_UINT uCount);
typedef L_INT ( pWRPEXT_CALLBACK pL_ANNDEFINE )(HANNOBJECT hObject,
                                LPPOINT pPoint,
                                L_UINT uState);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETRECT )(HANNOBJECT hObject,
                                 pANNRECT pRect);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETFILLMODE )(HANNOBJECT hObject,
                                     L_UINT uFillMode,
                                     L_INT nAlpha,
                                     L_UINT uFlags);

typedef L_INT ( pWRPEXT_CALLBACK pL_ANNSETTEXT )(HANNOBJECT hObject,
                                 L_TCHAR *pText,
                                 L_UINT uFlags);

typedef L_INT (pEXT_FUNCTION pDicomL_ApplyModalityLUT)(pBITMAPHANDLE pBitmap,L_UINT16* pLUT,pDICOMLUTDESCRIPTOR pLUTDescriptor,L_UINT uFlags);
typedef L_INT (pEXT_FUNCTION pDicomL_ApplyLinearModalityLUT)(pBITMAPHANDLE pBitmap,L_DOUBLE fIntercept,L_DOUBLE fSlope,L_UINT uFlags);
typedef L_INT (pEXT_FUNCTION pDicomL_ApplyVOILUT)(pBITMAPHANDLE pBitmap,L_UINT16* pLUT,pDICOMLUTDESCRIPTOR  pLUTDescriptor,L_UINT uFlags);
typedef L_INT (pEXT_FUNCTION pDicomL_ApplyLinearVOILUT)(pBITMAPHANDLE  pBitmap,L_DOUBLE fCenter,L_DOUBLE fWidth,L_UINT uFlags);
typedef L_INT (pEXT_FUNCTION pDicomL_GetLinearVOILUT)(pBITMAPHANDLE pBitmap,L_DOUBLE *pCenter,L_DOUBLE *pWidth,L_UINT uFlags);
typedef L_INT (pEXT_FUNCTION pDicomL_CountLUTColors)(RGBQUAD* pLUT,L_UINT32 ulLLUTLen,L_UINT *pNumberOfEntries,L_INT *pFirstIndex,L_UINT uFlags);
#if defined (LEADTOOLS_V16_OR_LATER)
typedef L_INT (pEXT_FUNCTION pDicomL_CountLUTColorsExt)(L_RGBQUAD16* pLUT,L_UINT32 ulLLUTLen,L_UINT *pNumberOfEntries,L_INT *pFirstIndex,L_UINT uFlags);
#endif // #if defined (LEADTOOLS_V16_OR_LATER)

#if defined (LEADTOOLS_V16_OR_LATER)
typedef L_INT (pEXT_FUNCTION pDicomL_GetMinMaxVal)(pBITMAPHANDLE pBitmap,L_INT* pMinVal,L_INT* pMaxVal,L_UINT32 uFlags);
typedef L_INT (pEXT_FUNCTION pDicomL_ShiftBitmapData)(pBITMAPHANDLE pDstBitmap, pBITMAPHANDLE pSrcBitmap, L_UINT uSrcLowBit, L_UINT uSrcHighBit, L_UINT uDstLowBit, L_UINT uDstBitsPerPixel, L_UINT32 uFlags);
#else
typedef L_INT (pEXT_FUNCTION pDicomL_GetMinMaxVal)(pBITMAPHANDLE pBitmap,L_INT* pMinVal,L_INT* pMaxVal);
typedef L_INT (pEXT_FUNCTION pDicomL_ShiftBitmapData)(pBITMAPHANDLE  pDstBitmap, pBITMAPHANDLE pSrcBitmap, L_UINT uSrcLowBit, L_UINT uSrcHighBit, L_UINT uDstLowBit, L_UINT uDstBitsPerPixel);
#endif

typedef enum tagDICOM_TRANSFER_SYNTAXES
{
   TRANSFER_SYNTAX_IMPLICIT_VR_LITTLE_ENDIAN ,//1.2.840.10008.1.2
   TRANSFER_SYNTAX_EXPLICIT_VR_LITTLE_ENDIAN ,//1.2.840.10008.1.2.1
   TRANSFER_SYNTAX_EXPLICIT_VR_BIG_ENDIAN    ,//1.2.840.10008.1.2.2
   TRANSFER_SYNTAX_RLE_LOSSLESS              ,//1.2.840.10008.1.2.5
   TRANSFER_SYNTAX_JPEG_BASELINE_1           ,//1.2.840.10008.1.2.4.50
   TRANSFER_SYNTAX_JPEG_EXTENDED_2_4         ,//1.2.840.10008.1.2.4.51
   TRANSFER_SYNTAX_JPEG_LOSSLESS_NONHIER_14  ,//1.2.840.10008.1.2.4.57
   TRANSFER_SYNTAX_JPEG_LOSSLESS_NONHIER_14B ,//1.2.840.10008.1.2.4.70
   TRANSFER_SYNTAX_JPEG2000_LOSSLESS_ONLY    ,//1.2.840.10008.1.2.4.90
   TRANSFER_SYNTAX_JPEG2000                   //1.2.840.10008.1.2.4.91  
}
DICOM_TRANSFER_SYNTAXES, *pDICOM_TRANSFER_SYNTAXES;


typedef enum tagDICOMLUTDESCRIPTORTYPE
{
   DICOMLUTDESCRIPTORTYPE_MODALITY        ,
   DICOMLUTDESCRIPTORTYPE_VOI             ,
   DICOMLUTDESCRIPTORTYPE_PALETTECOLOR       
}
DICOMLUTDESCRIPTORTYPE, *pDICOMLUTDESCRIPTORTYPE;


typedef struct tagDICOMMLUTATTRIBS
{
   L_UINT                  uStructSize;
   L_BOOL                  bIsModalityLUTSequence;
   DICOMLUTDESCRIPTOR      LUTDescriptor;
   L_TCHAR                 pszLUTExplanation[DICOM_VR_LO_LENGTH+1];
   L_TCHAR                 szModalityLUTType[DICOM_VR_LO_LENGTH+1];
   L_BOOL                  bIsRescaleSlopeIntercept;
   L_DOUBLE                fRescaleIntercept;
   L_DOUBLE                fRescaleSlope;
   L_TCHAR                 szRescaleType[DICOM_VR_LO_LENGTH+1];   
}DICOMMLUTATTRIBS , * pDICOMMLUTATTRIBS;


/*Palette Color LUT related types*/


typedef struct tagDICOMPALCOLORLUTATTRIBS
{
   L_UINT               uStructSize;
   DICOMLUTDESCRIPTOR   RedLUTDescriptor;
   DICOMLUTDESCRIPTOR   GreenLUTDescriptor;
   DICOMLUTDESCRIPTOR   BlueLUTDescriptor;
   L_TCHAR              szUID[DICOM_VR_UI_LENGTH+1];
   L_BOOL               bIsSegmented;
}DICOMPALCOLORLUTATTRIBS , * pDICOMPALCOLORLUTATTRIBS;

typedef enum tagDICOMPALETTECOLORLUTTYPE
{
   DICOMPALETTECOLORLUTTYPE_RED    ,
   DICOMPALETTECOLORLUTTYPE_GREEN  ,
   DICOMPALETTECOLORLUTTYPE_BLUE   
} DICOMPALETTECOLORLUTTYPE, *pDICOMPALETTECOLORLUTTYPE;


/*VOI LUT related types*/

typedef struct tagDICOMWINDOWATTRIBS
{
   L_UINT      uStructSize;
   L_DOUBLE    fWindowCenter ; // Window Center
   L_DOUBLE    fWindowWidth  ; // Window Width
   L_TCHAR     pszWindowCWExplanation[DICOM_VR_LO_LENGTH+1];
      
}DICOMWINDOWATTRIBS , * pDICOMWINDOWATTRIBS;


typedef struct tagDICOMVOILUTATTRIBS
{
   L_UINT               uStructSize;
   DICOMLUTDESCRIPTOR   LUTDescriptor;
   L_TCHAR              pszLUTExplanation[DICOM_VR_LO_LENGTH+1];   
}DICOMVOILUTATTRIBS , * pDICOMVOILUTATTRIBS;


// Flags used with SetOverlayAttributes and L_DicomSetOverlayAttributes
#define SET_OVERLAY_ATTRIB_NO_OVERRIDE    0x0001


// Flags for GetImage and GetImageList 
#define DICOM_GETIMAGE_AUTO_LOAD_OVERLAYS       0x0001
#define DICOM_GETIMAGE_AUTO_APPLY_MODALITY_LUT  0x0002
#define DICOM_GETIMAGE_AUTO_APPLY_VOI_LUT       0x0004
#define DICOM_GETIMAGE_ALLOW_RANGE_EXPANSION    0x0008 //deprecated, do not use
#define DICOM_GETIMAGE_AUTO_SCALE_MODALITY_LUT  0x0010
#define DICOM_GETIMAGE_AUTO_SCALE_VOI_LUT       0x0020

// Flags for SetImage
#define DICOM_SETIMAGE_AUTO_SAVE_OVERLAYS             0x00000001
#define DICOM_SETIMAGE_AUTO_SET_VOI_LUT               0x00000002
#define DICOM_SETIMAGE_MINIMIZE_JPEG_SIZE             0x00000004
#define DICOM_SETIMAGE_KEEP_LUTS_INTACT               0x20000000 // Internal flag

#define DICOM_SETIMAGE_MFG_OVERWRITE_SHARED           0x00000008
#define DICOM_SETIMAGE_MFG_VOI_LUT_PER_FRAME          0x00000010
#define DICOM_SETIMAGE_MFG_VOI_LUT_SHARED             0x00000020
#define DICOM_SETIMAGE_MFG_MODALITY_LUT_PER_FRAME     0x00000040
#define DICOM_SETIMAGE_MFG_MODALITY_LUT_SHARED        0x00000080


//Flags for ChangeTransferSyntax
#define DICOM_CHANGETRAN_MINIMIZE_JPEG_SIZE           0x00000001

// DICOM Annotation Types

#define DICANN_TYPE_POINT           1
#define DICANN_TYPE_POLYLINE        2
#define DICANN_TYPE_INTERPOLATED    3
#define DICANN_TYPE_CIRCLE          4
#define DICANN_TYPE_ELLIPSE         5

// DICOM Annotation Units Relativity
#define DICANN_UNIT_PIXEL           1
#define DICANN_UNIT_DISPLAY         2

// Text Annotation Justification
#define DICANN_TEXT_LEFT            0    // 'LEFT'
#define DICANN_TEXT_RIGHT           1    // 'RIGHT'
#define DICANN_TEXT_CENTER          2    // 'CENTER'

// Displayed Area Size Mode
#define DICANN_SIZEMODE_SCALETOFIT  0    // 'SCALE TO FIT'
#define DICANN_SIZEMODE_TRUESIZE    1    // 'TRUE SIZE'
#define DICANN_SIZEMODE_MAGNIFY     2    // 'MAGNIFY'

// Conversion Types
typedef enum tagLEADANNOBJCONVERSIONTYPE
{
   LEADANNOBJCONVERSIONTYPE_GRAPHIC ,
   LEADANNOBJCONVERSIONTYPE_TEXT    
} LEADANNOBJCONVERSIONTYPE, *pLEADANNOBJCONVERSIONTYPE;

// DICOM Annotation Point
typedef struct tagDICOMANNPOINT
{
   L_FLOAT fX;
   L_FLOAT fY;
} DICOMANNPOINT, *pDICOMANNPOINT;

// Presentation State Module structure
typedef struct tagDICOMPRESSTATEINFO
{
   L_UINT     uStructSize;
   L_INT32    nInstanceNumber;
   L_TCHAR    *pszPresLabel;
   L_TCHAR    *pszPresDescription;
   pVALUEDATE pPresCreationDate;
   pVALUETIME pPresCreationTime;
   L_TCHAR    *pszPresCreator;
} DICOMPRESSTATEINFO, *pDICOMPRESSTATEINFO;


// Graphic Layer Module structure
typedef struct tagDICOMGRAPHICLAYER
{
   L_UINT    uStructSize;
   L_TCHAR   *pszLayerName;
   L_INT32   nLayerOrder;
   L_INT16*  puGrayscale;
   L_INT16*  pRGBLayerColors;
   L_TCHAR   *pszLayerDescription;
} DICOMGRAPHICLAYER, *pDICOMGRAPHICLAYER;

// Graphic Annotation Object structure
typedef struct tagDICOMGRAPHICOBJECT
{
   L_UINT   uStructSize;
   L_TCHAR* pszLayerName;
   L_UINT   uType;
   L_UINT   uUnits;
   L_BOOL   bFilled;
   L_INT16  nPointCount;
   pDICOMANNPOINT  pAnnPoints;
} DICOMGRAPHICOBJECT, *pDICOMGRAPHICOBJECT;

// Text Annotation Object structure
typedef struct tagDICOMTEXTOBJECT
{
   L_UINT      uStructSize;
   L_TCHAR*    pszLayerName;
   L_TCHAR*    pszTextValue;
   L_FLOAT*    pTLHCorner;
   L_FLOAT*    pBRHCorner;
   L_UINT      uBoundingBoxUnits;
   L_UINT      uTextJustification; 
   L_FLOAT*    pAnchorPoint;
   L_UINT      uAnchorPointUnits;
   L_BOOL      bAnchorPointVisible;
} DICOMTEXTOBJECT, *pDICOMTEXTOBJECT;

typedef L_UINT16(pEXT_CALLBACK pCONVERTLEADANNOBJTODICOMANNPROC)( const pDICOMGRAPHICOBJECT pGraphicObject,
                                                                  const pDICOMTEXTOBJECT pTextObject,L_VOID *pUserData);


#define _MAX_DICOM_OVERLAYS 16


// MAC Algorithms
#define DICOM_MAC_ALGORITHM_RIPEMD160   0
#define DICOM_MAC_ALGORITHM_SHA1        1
#define DICOM_MAC_ALGORITHM_MD5         2

// Digital Signature Security Profiles
#define DICOM_SECURITY_PROFILE_NONE                0
#define DICOM_SECURITY_PROFILE_BASE_RSA            1
#define DICOM_SECURITY_PROFILE_CREATOR_RSA         2
#define DICOM_SECURITY_PROFILE_AUTHORIZATION_RSA   3

// Formats when saving a digital certificate
#define DICOM_CERTIFICATE_FORMAT_PEM   0
#define DICOM_CERTIFICATE_FORMAT_DER   1

#if defined(__cplusplus)

#if !defined(FOR_WINCE)
class L_LTDIC_CLASS LDicomNet;
class L_LTDIC_CLASS LDicomWaveformGroup;
class L_LTDIC_CLASS LDicomWaveformChannel;
#endif // #if !defined(FOR_WINCE)

// An Item in a Code Sequence Attribute
typedef struct tagDICOMCODESEQUENCEITEM
{
   L_UINT uStructSize;
   
   // Basic Coded Entry Attributes
   L_TCHAR*       pszCodeValue;
   L_TCHAR*       pszCodingSchemeDesignator;
   L_TCHAR*       pszCodingSchemeVersion;
   L_TCHAR*       pszCodeMeaning;
   
   // Enhanced Encoding Mode Attributes
   L_TCHAR*       pszContextIdentifier;
   L_TCHAR*       pszMappingResource;
   pVALUEDATETIME pContextGroupVersion;
   pVALUEDATETIME pContextGroupLocalVersion;
   L_TCHAR*       pszContextGroupExtensionCreatorUID;

} DICOMCODESEQUENCEITEM, * pDICOMCODESEQUENCEITEM;

#if defined(LEADTOOLS_V16_OR_LATER)
   //---------------------------------------------------------------------------
   // Encapsulated Document types
   //---------------------------------------------------------------------------
   enum 
   {
      ENCAPSULATED_DOCUMENT_UNKNOWN = 0,
      ENCAPSULATED_DOCUMENT_PDF     = 1,
      ENCAPSULATED_DOCUMENT_CDA     = 2,
   };

   typedef struct _DICOMENCAPSULATEDDOCUMENT
   {
      L_UINT                     uStructSize;
      L_UINT16                   uType;                                 // ENCAPSULATED_DOCUMENT_PDF or ENCAPSULATED_DOCUMENT_CDA
      L_INT32                    nInstanceNumber;                      // (0020,0013)    Type1    VR_IS    1  IntegerString
      pVALUEDATE                 pContentDate;                          // (0008,0023)    Type2    VR_DA    1  Date
      pVALUETIME                 pContentTime;                          // (0008,0033)    Type2    VR_TM    1  Time
      pVALUEDATETIME             pAcquisitionDateTime;                  // (0008,002A)    Type2    VR_DT    1  DateTime
      L_TCHAR                   *pszBurnedInAnnotation;                 // (0028,0301)    Type1    VR_CS    1 CodeString   "YES" or "NO"
      L_TCHAR                   *pszDocumentTitle;                      // (0042,0010)    Type2    VR_ST    1 ShortText
      L_TCHAR                   *pszVerificationFlag;                   // (0040,A493)    Type3  VR_CS    1 CodeStrin "UNVERIFIED" or "VERIFIED"
      L_TCHAR                   *pszHL7InstanceIdentifier;              // (0040,E001)    Type1C   VR_ST   1 ShortText
      L_TCHAR                   *pszMIMETypeOfEncapsulatedDocument;     // (0042,0012)    Type1    VR_LO   1 LongString
      L_TCHAR                   *pszListOfMIMETypes;                    // (0042,0014)    Type1C   VR_LO   1.FFFFF  LongString
      L_UINT32                   nListOfMIMETypesCount;                 // Number of NULL terminated strings in 'pszListOfMIMETypes'
   } DICOMENCAPSULATEDDOCUMENT, *pDICOMENCAPSULATEDDOCUMENT;
#endif

class L_LTDIC_CLASS LDicomDS
{
public:
   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   LDicomDS(L_TCHAR *pszPath=NULL);
   ~LDicomDS();

   L_VOID          InitDS            (L_UINT32 nClass, L_UINT16 nFlags);
   L_VOID          GetInfoDS         (L_UINT32 *pnClass, L_UINT16 *pnFlags);

   L_VOID          InitCS            (L_UINT16 nCommand, L_BOOL bRequest);
   L_VOID          GetInfoCS         (L_UINT16 *pnCommand, L_BOOL *pbRequest);


   //---------------------------------------------------------------------------
   // Input and Output Functions
   //---------------------------------------------------------------------------
   L_UINT16        LoadDS            (L_TCHAR *pszName, L_UINT16 nFlags);
   L_UINT16        LoadDSMemory      (L_UCHAR *pBuffer, L_UINT32 uBufferSize, L_UINT16 nFlags);
   L_UINT16        SaveDS            (L_TCHAR *pszName, L_UINT16 nFlags);
   L_UINT16        CopyDS            (pDICOMELEMENT pDstParent, LDicomDS *pSrcDS, pDICOMELEMENT pSrcParent);

#if defined (LEADTOOLS_V16_OR_LATER)
   L_UINT16        CopyDS            (pDICOMELEMENT pDstParent, LDicomDS *pSrcDS, pDICOMELEMENT pSrcParent, COPYDSCALLBACK pfnCallback, L_VOID *pUserData);
#endif


   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   pDICOMELEMENT   InsertElement     (pDICOMELEMENT pNeighbor, L_BOOL bChild, L_UINT32 nTag, L_UINT16 nVR, L_BOOL bSequence, L_UINT32 nIndex);
   pDICOMMODULE    InsertModule      (L_UINT32 nModule, L_BOOL bOptional);
   pDICOMELEMENT   InsertKey         (pDICOMELEMENT pParent, L_TCHAR *pszKey, L_BOOL bOptional);

   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   pDICOMELEMENT   DeleteElement     (pDICOMELEMENT pElement);
   L_VOID          DeleteModule      (L_UINT32 nModule);
   L_VOID          DeleteKey         (pDICOMELEMENT pElement);
   L_VOID          ResetDS           ();

   //---------------------------------------------------------------------------
   // Iteration Functions
   //---------------------------------------------------------------------------
   pDICOMELEMENT   GetRootElement    (pDICOMELEMENT pElement);
   pDICOMELEMENT   GetParentElement  (pDICOMELEMENT pElement);
   pDICOMELEMENT   GetChildElement   (pDICOMELEMENT pElement, L_BOOL bVolatile);
   pDICOMELEMENT   GetFirstElement   (pDICOMELEMENT pElement, L_BOOL bTree, L_BOOL bVolatile);
   pDICOMELEMENT   GetLastElement    (pDICOMELEMENT pElement, L_BOOL bTree, L_BOOL bVolatile);
   pDICOMELEMENT   GetPrevElement    (pDICOMELEMENT pElement, L_BOOL bTree, L_BOOL bVolatile);
   pDICOMELEMENT   GetNextElement    (pDICOMELEMENT pElement, L_BOOL bTree, L_BOOL bVolatile);
   L_UINT32        GetLevelElement   (pDICOMELEMENT pElement);
   L_UINT32        GetCountModule    ();
   L_BOOL          ExistsElement     (pDICOMELEMENT pElement);
   L_BOOL          IsVolatileElement (pDICOMELEMENT pElement);
   L_BOOL          ConformanceDS     (CONFORMANCECALLBACK pfnCallback, L_VOID *pUserData);
   L_VOID          SetDebugDS        (CONFORMANCECALLBACK pfnCallback, L_VOID *pUserData);

   pDICOMELEMENT   GetRootKey        (pDICOMELEMENT pElement);
   pDICOMELEMENT   GetParentKey      (pDICOMELEMENT pElement);
   pDICOMELEMENT   GetChildKey       (pDICOMELEMENT pElement);
   pDICOMELEMENT   GetFirstKey       (pDICOMELEMENT pElement, L_BOOL bTree);
   pDICOMELEMENT   GetLastKey        (pDICOMELEMENT pElement, L_BOOL bTree);
   pDICOMELEMENT   GetPrevKey        (pDICOMELEMENT pElement, L_BOOL bTree);
   pDICOMELEMENT   GetNextKey        (pDICOMELEMENT pElement, L_BOOL bTree);
   L_TCHAR         *GetValueKey       (pDICOMELEMENT pElement);

   //---------------------------------------------------------------------------
   // Searching Functions
   //---------------------------------------------------------------------------
   pDICOMELEMENT   FindFirstElement  (pDICOMELEMENT pElement, L_UINT32 nTag, L_BOOL bTree);
   pDICOMELEMENT   FindLastElement   (pDICOMELEMENT pElement, L_UINT32 nTag, L_BOOL bTree);
   pDICOMELEMENT   FindPrevElement   (pDICOMELEMENT pElement, L_BOOL bTree);
   pDICOMELEMENT   FindNextElement   (pDICOMELEMENT pElement, L_BOOL bTree);
   pDICOMMODULE    FindModule        (L_UINT32 nModule);
   pDICOMMODULE    FindIndexModule   (L_UINT32 nIndex);

   pDICOMELEMENT   FindFirstKey      (pDICOMELEMENT pElement, L_TCHAR *pszKey, L_BOOL bTree);
   pDICOMELEMENT   FindLastKey       (pDICOMELEMENT pElement, L_TCHAR *pszKey, L_BOOL bTree);
   pDICOMELEMENT   FindPrevKey       (pDICOMELEMENT pElement, L_BOOL bTree);
   pDICOMELEMENT   FindNextKey       (pDICOMELEMENT pElement, L_BOOL bTree);

   //---------------------------------------------------------------------------
   // Retrieval Functions
   //---------------------------------------------------------------------------
   L_VOID          GetPreamble       (L_UCHAR *pPreamble, L_UINT16 nLength);
   L_UINT32        GetCountValue     (pDICOMELEMENT pElement);
   L_VOID          FreeValue         (pDICOMELEMENT pElement);
   L_BOOL          GetBinaryValue    (pDICOMELEMENT pElement, L_VOID *pValue,  L_UINT32 nLength);
   L_UCHAR         *GetCharValue     (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_INT16        *GetShortValue     (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_INT32        *GetLongValue      (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);   
   L_UINT32        GetLong64Value    (pDICOMELEMENT pElement, L_INT64 *pValue, L_UINT32 nIndex,L_UINT32 nCount);
   L_FLOAT        *GetFloatValue     (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_DOUBLE       *GetDoubleValue    (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_CHAR         *GetStringValueA   (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_TCHAR        *GetStringValue    (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);

#if defined(_UNICODE)
   L_UINT32        ConvertStringValue(L_CHAR *Source, L_UINT32 SrcSizeInBytes, L_TCHAR *Destination, L_UINT32 DestSizeInWords);
#endif // #if defined(_UNICODE)
   pVALUEAGE       GetAgeValue       (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   pVALUEDATE      GetDateValue      (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);

   L_INT           GetDateRangeValue (pDICOMELEMENT pElement, L_UINT32 nIndex, pVALUEDATERANGE pValue);
   L_INT16         GetTimeRangeValue (pDICOMELEMENT pElement, L_UINT32 nIndex, pVALUETIMERANGE pValue);
   L_INT16         GetDateTimeRangeValue (pDICOMELEMENT pElement, L_UINT32 nIndex, pVALUEDATETIMERANGE pValue);


   pVALUETIME      GetTimeValue      (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   pVALUEDATETIME  GetDateTimeValue  (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_UINT32        GetConvertValue   (pDICOMELEMENT pElement, L_TCHAR *Destination, L_UINT32 DestSizeInWords);

   L_UINT32        GetCountImage     (pDICOMELEMENT pElement);
   L_UINT16        GetInfoImage      (pDICOMELEMENT pElement, pDICOMIMAGE pInfo, L_UINT32 nIndex);
   L_UINT16        GetImage          (pDICOMELEMENT pElement, pBITMAPHANDLE pBitmap,L_UINT uStructSize ,L_UINT32 nIndex, L_INT32 nBitsPerPixel, L_INT32 nOrder, L_UINT uFlags ,FILEREADCALLBACK pfnCallback, L_VOID *pUserData);
   L_UINT16        GetImageList      (pDICOMELEMENT pElement, HBITMAPLIST hList, L_UINT32 nIndex, L_UINT32 nCount, L_INT32 nBitsPerPixel, L_INT32 nOrder,L_UINT uFlags );

   L_BOOL          GetKeepPixelDataIntactFlag();

   L_UINT16        GetJ2KOptions( pFILEJ2KOPTIONS pOptions, L_INT nSize );
   L_UINT16        GetDefaultJ2KOptions( pFILEJ2KOPTIONS pOptions, L_INT nSize );

   L_UINT32        GetElementOffset(pDICOMELEMENT pElement);

   

   
   //---------------------------------------------------------------------------
   // Waveform Functions
   //---------------------------------------------------------------------------
   L_UINT32 GetWaveformGroupCount();
   L_UINT16 GetWaveformGroup(L_UINT32 uIndex, LDicomWaveformGroup* pWaveformGroup);
   L_UINT16 DeleteWaveformGroup(L_UINT32 uIndex, L_UINT16 uReserved);
   L_UINT16 AddWaveformGroup(LDicomWaveformGroup* pWaveformGroup, L_UINT16 uFlags, L_UINT32 uIndex = ELEMENT_INDEX_MAX);
   //---------------------------------------------------------------------------
   // Modification Functions
   //---------------------------------------------------------------------------
   L_VOID          SetPreamble       (L_UCHAR *pPreamble, L_UINT16 nLength);
   L_BOOL          SetBinaryValue    (pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nLength);
   L_BOOL          SetCharValue      (pDICOMELEMENT pElement, L_UCHAR *pValue, L_UINT32 nCount);
   L_BOOL          SetShortValue     (pDICOMELEMENT pElement, L_INT16 *pValue, L_UINT32 nCount);
   L_BOOL          SetLongValue      (pDICOMELEMENT pElement, L_INT32 *pValue, L_UINT32 nCount);
   L_BOOL          SetFloatValue     (pDICOMELEMENT pElement, L_FLOAT *pValue, L_UINT32 nCount);
   L_BOOL          SetDoubleValue    (pDICOMELEMENT pElement, L_DOUBLE *pValue, L_UINT32 nCount);
   L_BOOL          SetStringValue    (pDICOMELEMENT pElement, L_TCHAR *pValue, L_UINT32 nCount, L_UINT32 uCharacterSet);
   L_BOOL          SetAgeValue       (pDICOMELEMENT pElement, pVALUEAGE pValue, L_UINT32 nCount);
   L_BOOL          SetDateValue      (pDICOMELEMENT pElement, pVALUEDATE pValue, L_UINT32 nCount);
   L_BOOL          SetTimeValue      (pDICOMELEMENT pElement, pVALUETIME pValue, L_UINT32 nCount);
   L_BOOL          SetDateTimeValue  (pDICOMELEMENT pElement, pVALUEDATETIME pValue, L_UINT32 nCount);
   L_BOOL          SetDateRangeValue      (pDICOMELEMENT pElement, pVALUEDATERANGE pValue, L_UINT32 nCount);
   L_BOOL          SetTimeRangeValue      (pDICOMELEMENT pElement, pVALUETIMERANGE pValue, L_UINT32 nCount);
   L_BOOL          SetDateTimeRangeValue  (pDICOMELEMENT pElement, pVALUEDATETIMERANGE pValue, L_UINT32 nCount);
   L_BOOL          SetConvertValue   (pDICOMELEMENT pElement, L_TCHAR *strText, L_UINT32 nCount);
   L_UINT16        InsertImage       (pDICOMELEMENT pElement, pBITMAPHANDLE pBitmap, L_UINT32 nIndex, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor,L_UINT uFlags, FILESAVECALLBACK pfnCallback, L_VOID *pUserData);
   L_UINT16        InsertImageList   (pDICOMELEMENT pElement, HBITMAPLIST hList, L_UINT32 nIndex, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor, L_UINT uFlags );
   L_UINT16        SetImage          (pDICOMELEMENT pElement, pBITMAPHANDLE pBitmap, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor, L_UINT uFlags , FILESAVECALLBACK pfnCallback, L_VOID *pUserData);
   L_UINT16        SetImageList      (pDICOMELEMENT pElement, HBITMAPLIST hList, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor,L_UINT uFlags );
   L_UINT16        DeleteImage       (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
#if !defined(FOR_WINCE)
   L_UINT16        ChangeTransferSyntax(L_TCHAR *pszUID,L_INT32 nQFactor/*For lossy*/,L_UINT32 uFlags);   
#endif // #if !defined(FOR_WINCE)
   L_VOID          SetKeepPixelDataIntactFlag(L_BOOL bSet);
   
   L_UINT16        SetJ2KOptions( const pFILEJ2KOPTIONS pOptions);


   //---------------------------------------------------------------------------
   // Digital Signatures
   //---------------------------------------------------------------------------

   L_UINT32       GetSignaturesCount(pDICOMELEMENT pItem);
   pDICOMELEMENT  GetSignature(pDICOMELEMENT pItem, L_UINT32 uIndex);
   pDICOMELEMENT  FindSignature(const L_TCHAR* pszSignatureUID);

   L_TCHAR*       GetSignatureUID(pDICOMELEMENT pSignatureItem);
   pVALUEDATETIME GetSignatureDateTime(pDICOMELEMENT pSignatureItem);
   L_UINT16       SaveCertificate(pDICOMELEMENT pSignatureItem, const L_TCHAR* pszFilename, L_UINT16 uFormat = DICOM_CERTIFICATE_FORMAT_PEM);
   L_UINT32       GetSignedElementsCount(pDICOMELEMENT pSignatureItem);
   pDICOMELEMENT  GetSignedElement(pDICOMELEMENT pSignatureItem, L_UINT32 uIndex);
   L_TCHAR*       GetMacTransferSyntax(pDICOMELEMENT pSignatureItem);
   L_TCHAR*       GetMacAlgorithm(pDICOMELEMENT pSignatureItem);

   L_VOID         DeleteSignature(pDICOMELEMENT pSignatureItem);
   L_UINT16       VerifySignature(pDICOMELEMENT pSignatureItem, L_UINT16 uReserved = 0);
   L_UINT16       CreateSignature(pDICOMELEMENT  pItem,
                                  const L_TCHAR*        pszPrivateKeyFile,
                                  const L_TCHAR*        pszCertificateFile,
                                  const L_TCHAR*        pszPassword,
                                  pDICOMELEMENT* ppSignatureItem = NULL,
                                  const L_TCHAR*        pszMacTransferSyntax = NULL,
                                  L_UINT16       uMacAlgorithm = DICOM_MAC_ALGORITHM_RIPEMD160,
                                  L_UINT32*      pElementsToSign = NULL,
                                  L_UINT32       uCount = 0,
                                  L_UINT16       uSecurityProfile = DICOM_SECURITY_PROFILE_NONE,
                                  L_UINT16       uReserved = 0);


   //---------------------------------------------------------------------------
   // LUT Manipulation Functions
   //---------------------------------------------------------------------------
   
   //Modality LUT
   L_UINT16 GetModalityLUTData(L_UINT16 * pLUTData,L_UINT uDataSize,L_UINT uFlags);
#if defined(LEADTOOLS_V16_OR_LATER)
   L_UINT16 GetModalityLUTAttributes(L_UINT32 uFrameIndex, pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT uStructSize,L_UINT uFlags);
   L_UINT16 SetModalityLUT(L_UINT32 uFrameIndex, pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT16 *pLUTData,L_UINT uDataSize,L_UINT uFlags);
   L_UINT16 DeleteModalityLUT(L_UINT32 uFrameIndex, L_UINT uFlags);
#else
   L_UINT16 GetModalityLUTAttributes(pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT uStructSize,L_UINT uFlags);
   L_UINT16 SetModalityLUT(pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT16 *pLUTData,L_UINT uDataSize,L_UINT uFlags);
   L_UINT16 DeleteModalityLUT(L_UINT uFlags);
#endif


   //Palette Color LUT
   L_UINT16 GetPaletteColorLUTAttributes(pDICOMPALCOLORLUTATTRIBS pPaletteColorLUTAttributes,L_UINT uStructSize,L_UINT uFlags);
   L_UINT16 GetPaletteColorLUTData(L_UINT16 * pLUTData,L_UINT uDataSize,DICOMPALETTECOLORLUTTYPE PaletteColorLUTType,L_UINT uFlags);
   L_UINT16 SetPaletteColorLUTAttributes(pDICOMPALCOLORLUTATTRIBS pPaletteColorLUTAttributes,L_UINT uFlags);
   L_UINT16 SetPaletteColorLUTData(L_UINT16 * pLUTData,L_UINT uDataSize,DICOMPALETTECOLORLUTTYPE PaletteColorLUTType,L_UINT uFlags);
   L_UINT16 DeletePaletteColorLUT(L_UINT uFlags);


   // VOI LUT
#if defined(LEADTOOLS_V16_OR_LATER) 
   L_UINT16 GetWindowCount (L_UINT32 uFrameIndex, L_UINT *pCount);
   L_UINT16 GetWindow      (L_UINT32 uFrameIndex, L_UINT uWindowIndex ,pDICOMWINDOWATTRIBS pWindowAttributes ,  L_UINT uStructSize,L_UINT uFlags);
   L_UINT16 SetWindow      (L_UINT32 uFrameIndex, L_UINT uWindowIndex ,pDICOMWINDOWATTRIBS pWindowAttributes ,  L_UINT uFlags);
   L_UINT16 DeleteWindow   (L_UINT32 uFrameIndex, L_UINT uFlags);
#else
   L_UINT16 GetWindowCount (L_UINT *pCount);
   L_UINT16 GetWindow      (L_UINT uWindowIndex ,pDICOMWINDOWATTRIBS pWindowAttributes ,  L_UINT uStructSize,L_UINT uFlags);
   L_UINT16 SetWindow      (L_UINT uWindowIndex ,pDICOMWINDOWATTRIBS pWindowAttributes ,  L_UINT uFlags);
   L_UINT16 DeleteWindow   (L_UINT uFlags);
#endif


public:
   L_UINT16 GetVOILUTCount (L_UINT * pCount);
   L_UINT16 GetVOILUT      (L_UINT uVOILUTIndex ,pDICOMVOILUTATTRIBS pVOILUTAttributes ,  L_UINT uStructSize,L_UINT uFlags);
   L_UINT16 SetVOILUT      (L_UINT uVOILUTIndex ,pDICOMVOILUTATTRIBS pVOILUTAttributes ,  L_UINT16 * pLUTData,L_UINT uDataSize,L_UINT uFlags);
   L_UINT16 GetVOILUTData  (L_UINT uVOILUTIndex ,L_UINT16 * pLUTData,  L_UINT uDataSize,L_UINT uFlags);
   L_UINT16 DeleteVOILUT   (L_UINT uFlags);

   
   //---------------------------------------------------------------------------
   // Overlay Manipulation Functions
   //---------------------------------------------------------------------------
   L_UINT16 GetOverlayCount            (L_UINT * pCount);
   L_UINT16 GetOverlayAttributes       (L_UINT uOverlayIndex ,pOVERLAYATTRIBUTES    pOverlayAttributes   ,  L_UINT   uStructSize,L_INT *pGroupNumber,L_BOOL * pIsOverlayInDataset , L_UINT uFlags);
   L_UINT16 GetOverlayActivationLayer  (L_UINT uOverlayIndex ,L_TCHAR *             pActivationLayer     ,  L_UINT   uLength);
   L_UINT16 GetOverlayBitmap           (L_UINT uOverlayIndex ,pBITMAPHANDLE         pBitmap              ,  L_UINT   uStructSize,L_UINT uFlags);   
   L_UINT16 GetOverlayBitmapList       (L_UINT uOverlayIndex ,HBITMAPLIST           hList                ,  L_UINT32 uOverlayFrameIndex, L_UINT32 uCount,L_UINT uFlags);
   L_UINT16 SetOverlayAttributes       (L_UINT uOverlayIndex ,pOVERLAYATTRIBUTES    pOverlayAttributes,L_UINT uFlags);
   L_UINT16 SetOverlayBitmap           (L_UINT uOverlayIndex ,pBITMAPHANDLE         pBitmap,L_UINT uFlags);
   L_UINT16 SetOverlayBitmapList       (L_UINT uOverlayIndex ,HBITMAPLIST           hList,L_UINT uFlags);
   L_UINT16 DeleteOverlay              (L_UINT uOverlayIndex ,L_UINT uFlags);

   //---------------------------------------------------------------------------
   // Annotation Functions
   //---------------------------------------------------------------------------
#if !defined(FOR_MANAGED)

   L_UINT16        AnnSave           (HANNOBJECT      hAnnContainer,
                                      L_UINT          uFormat,
                                      L_BOOL          bSelected,
                                      pSAVEFILEOPTION pSaveOption,
                                      L_INT           nIndex,
                                      L_UINT32       *pnPrivateCreatorTag
                                      );

   L_UINT16        AnnLoad           (pHANNOBJECT     phAnnContainer,
                                      L_INT           nIndex,
                                      pLOADFILEOPTION pLoadOptions
                                     );


   L_UINT16        AnnCount         (L_INT *pFileIndices, L_UINT32 *pnPrivateCreatorTag);

   L_UINT16        AnnDelete        (L_INT nIndex, L_INT nPage); //nIndex: Pass -1 to delete the LEAD Private tag and all files
                                                                 //nPage:   Pass -1 to delete the entire file
                                                                 //         Pass >0 to delete the page

   L_VOID          LoadAnnDLL        ();

   L_UINT16 ConvertLEADAnnObjToDicomAnnObjs(HANNOBJECT hAnnObject, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uFlags);
   virtual L_UINT16 OnConvertLEADAnnObjToDicomAnnObj(const pDICOMGRAPHICOBJECT pGraphicObject, const pDICOMTEXTOBJECT pTextObject);
   L_UINT16 ConvertDicomAnnObjToLEADAnnObj(pHANNOBJECT phAnnObject, pDICOMGRAPHICOBJECT pGraphicObject = NULL, pDICOMTEXTOBJECT pTextObject= NULL);

#endif
   //---------------------------------------------------------------------------
   // Presentation State Module functions 
   //---------------------------------------------------------------------------   
   L_UINT16 SetPresStateInfo(pDICOMPRESSTATEINFO pPresState);
   L_UINT16 GetPresStateInfo(pDICOMPRESSTATEINFO pPresState, L_UINT uStructSize);
   L_UINT16 AddPresStateImageRefByFileName(L_TCHAR* pszImageFileName, L_INT32* pFrameNumbers = NULL, L_UINT uFramesCount = 0);
   L_UINT16 AddPresStateImageRefByDS(LDicomDS* pDS, L_INT32* pFrameNumbers = NULL, L_UINT uFramesCount = 0);
   L_UINT16 RemovePresStateImageRefBySOPInstance(L_TCHAR* pszSOPInstanceUID);
   L_UINT16 RemoveAllPresStateImageRefs(L_VOID);
   L_TCHAR*  GetPresStateImageRefSOPInstance(pDICOMELEMENT pRefSeriesSQItem, L_UINT uImageIndex);
   L_UINT16 GetPresStateImageRefCount(pDICOMELEMENT pRefSeriesSQItem, L_UINT* pCount);
   pDICOMELEMENT FindFirstPresStateRefSeriesItem();
   pDICOMELEMENT FindNextPresStateRefSeriesItem(pDICOMELEMENT pRefSeriesItem);
   pDICOMELEMENT GetPresStateImageRefBySOPInstance(L_TCHAR* pszSOPInstanceUID);   
   //---------------------------------------------------------------------------
   // Graphic Layer Module functions
   //---------------------------------------------------------------------------    
   L_UINT16 CreateLayer(pDICOMGRAPHICLAYER pGraphicLayer, L_UINT* pLayerIndex);
   L_UINT16 GetLayerInfo(L_UINT uLayerIndex, pDICOMGRAPHICLAYER pGraphicLayer, L_UINT uStructSize);
   L_UINT16 SetLayerInfo(L_UINT uLayerIndex, pDICOMGRAPHICLAYER pGraphicLayer);
   L_UINT16 RemoveLayerByIndex(L_UINT uLayerIndex,L_BOOL bAnnSequence);
   L_UINT16 RemoveLayerByName(L_TCHAR* pszLayerName,L_BOOL bAnnSequence);
   L_UINT16 RemoveAllLayers(L_BOOL bAnnSequence);
   L_UINT16 GetLayerCount(L_UINT* pCount);
   L_UINT16 GetLayerIndex(L_TCHAR* pszLayerName, L_INT* pLayerIndex);
   L_UINT16 GetLayerGraphicObjectCount(pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_UINT16 RemoveLayerGraphicObjects(pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16 GetLayerTextObjectCount(pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_UINT16 RemoveLayerTextObjects(pDICOMELEMENT pGraphicAnnSQItem);
   pDICOMELEMENT GetLayerElementByIndex(L_UINT uLayerIndex);
   pDICOMELEMENT GetLayerElementByName(L_TCHAR* pszLayerName);   
   //---------------------------------------------------------------------------
   // Graphic Annotation Module functions 
   //---------------------------------------------------------------------------          
   pDICOMELEMENT FindFirstGraphicAnnSQItem();
   pDICOMELEMENT FindNextGraphicAnnSQItem(pDICOMELEMENT pRefSeriesItem);
   L_TCHAR* GetLayerName(pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16 SetLayerName(pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszLayerName);
   L_UINT16 CreateGraphicAnnSQItem(L_UINT32 nIndex, L_TCHAR* pszLayerName);
   // Annotation Referenced Image Sequence Functions 
   L_UINT16 AddLayerImageRef(pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszImageSOPInstance);
   L_UINT16 GetLayerImageRefCount(pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_TCHAR*  GetLayerImageRefSOPInstance(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uImageIndex);
   L_UINT16 RemoveImageRefFromLayer(pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszImageSOPInstance);
   L_UINT16 RemoveAllImageRefsFromLayer(pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16 RemoveAllImageRefFromAllLayers();
   pDICOMELEMENT GetLayerImageRefElement(pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszImageSOPInstance);
   // Graphic Annotation Objects functions
   L_UINT16 CreateGraphicObject(pDICOMELEMENT pGraphicAnnSQItem, pDICOMGRAPHICOBJECT pGraphicObject, L_BOOL bCheckLayer = FALSE);
   L_UINT16 RemoveGraphicObject(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uGraphicObjIndex);
   L_UINT16 GetGraphicObjectInfo(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uGraphicObjectIndex, pDICOMGRAPHICOBJECT pGraphicObject, L_UINT uStructSize);
   L_UINT16 SetGraphicObjectInfo(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uGraphicObjectIndex, pDICOMGRAPHICOBJECT pGraphicObject);
   L_UINT16 GetGraphicObjectCount(pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_UINT16 RemoveAllGraphicObjects(pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16 GetGraphicObjPointCount(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uGraphicObjIndex, L_UINT* pPointsCount);
   pDICOMELEMENT GetGraphicObjElement(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uObjIndex);
   // Text Annotation Objects function
   L_UINT16 CreateTextObject(pDICOMELEMENT pGraphicAnnSQItem, pDICOMTEXTOBJECT pTextObject, L_BOOL bCheckLayer= FALSE);
   L_UINT16 RemoveTextObject(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uTextObjIndex);
   L_UINT16 GetTextObjectInfo(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uTextObjectIndex, pDICOMTEXTOBJECT pTextObject, L_UINT uStructSize);
   L_UINT16 SetTextObjectInfo(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uTextObjectIndex, pDICOMTEXTOBJECT pTextObject);
   L_UINT16 GetTextObjectCount(pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_UINT16 RemoveAllTextObjects(pDICOMELEMENT pGraphicAnnSQItem);
   pDICOMELEMENT GetTextObjElement(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uObjIndex);
   //L_UINT16 GetAutoScaleParams(L_BOOL *pbAutoScaleApplied, L_DOUBLE *pdAutoScaleSlope, L_DOUBLE *pdAutoScaleIntercept, L_DOUBLE *pdWindowCenter, L_DOUBLE *pdWindowWidth);

#if defined(LEADTOOLS_V16_OR_LATER)
   //---------------------------------------------------------------------------
   // Private Element methods
   //---------------------------------------------------------------------------
   L_UINT16 CreatePrivateCreatorDataElement(pDICOMELEMENT pElement, L_UINT16 uElementGroup, L_UINT16 uElementNumber, L_TCHAR *pszIdCode, pDICOMELEMENT *ppPrivateCreatorDataElement);
   L_UINT16 GetNextUnusedPrivateTag(pDICOMELEMENT pPrivateCreatorDataElement, L_UINT32 *puTag);
   pDICOMELEMENT FindFirstPrivateCreatorDataElement(pDICOMELEMENT pElement, L_BOOL bTree, L_TCHAR *pszIdCode, L_UINT16 uElementGroup);
   pDICOMELEMENT FindNextPrivateCreatorDataElement(pDICOMELEMENT pElement, L_BOOL bTree, L_TCHAR *pszIdCode, L_UINT16 uElementGroup);
   pDICOMELEMENT FindFirstPrivateElement(pDICOMELEMENT pPrivateCreatorDataElement);
   pDICOMELEMENT FindNextPrivateElement(pDICOMELEMENT pElement, pDICOMELEMENT pPrivateCreatorDataElement);

   //---------------------------------------------------------------------------
   // Encapsulated Document methods
   //---------------------------------------------------------------------------
   L_UINT16 GetEncapsulatedDocument(pDICOMELEMENT pElement, L_BOOL bChild, L_TCHAR *pszFileDocument, pDICOMENCAPSULATEDDOCUMENT pEncapsulatedDocument, pDICOMCODESEQUENCEITEM pConceptNameCodeSequence);
   L_UINT16 SetEncapsulatedDocument(pDICOMELEMENT pElement, L_BOOL bChild, L_TCHAR *pszFileDocument, pDICOMENCAPSULATEDDOCUMENT pEncapsulatedDocument, pDICOMCODESEQUENCEITEM pConceptNameCodeSequence);
#endif


   //---------------------------------------------------------------------------
   // (Undocumented Functions and Internal use only)
   //---------------------------------------------------------------------------
   L_BOOL          IsBadPixelData(L_UINT32 * pBadCount,L_UINT32 * pGoodCount);
   L_UINT16        LoadDS            (LDicomFile *pFile, L_UINT16 nFlags, L_BOOL bVerify);
   L_UINT16        SaveDS            (LDicomFile *pFile, L_UINT16 nFlags);
   L_UINT32        FindGetValue      (pDICOMELEMENT pElement, L_UINT32 nTag, L_VOID *pValue, L_UINT32 nIndex, L_UINT32 nCount, L_INT16 nType);
   L_LTDIC_API friend L_UINT32       EXT_FUNCTION L_DicomFindGetValue      (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nTag, L_VOID *pValue, L_UINT32 nIndex, L_UINT32 nCount, L_INT16 nType);
   pDICOMELEMENT   FindSetValue      (pDICOMELEMENT Element, L_UINT32 nTag, L_VOID *pValue, L_UINT32 nCount, L_INT16 nType);
   L_LTDIC_API friend L_BOOL EXT_FUNCTION L_DicomInsertUncompressedFrame(LDicomDS *pDS , pDICOMELEMENT pPixelDataElement, L_VOID *pFrameBuffer, L_UINT32 nLength,L_UINT32 nFrameIndex);   
   static HINSTANCE  m_hInstance;
   L_VOID SetAnnConversionCallback(pCONVERTLEADANNOBJTODICOMANNPROC pDICOMAnnConversionProc,L_VOID* pDICOMAnnConversionUserData);
   L_BOOL InsertBinaryValue(pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nOffset, L_UINT32 nLength);
   L_BOOL GetBinaryValue(pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nOffset, L_UINT32 nLength);
   L_UINT32        GetConvertValueA  (pDICOMELEMENT pElement, L_CHAR *pszText);
   L_BOOL          SetStringValueA    (pDICOMELEMENT pElement, L_CHAR *pValue, L_UINT32 nCount);
   L_BOOL          SetConvertValueA   (pDICOMELEMENT pElement, L_CHAR *pszText, L_UINT32 nCount);
   L_UINT          UpdateSpecificCharacterSet();
private:
   LDicomTree           m_Tree;
   L_UINT16             m_nFlags;
   L_VOID *             m_pPrivateData;
   L_CHAR               m_szClass[UID_MAX_SIZE+1];
   L_CHAR               m_pPreamble[DS_PREAMBLE_LENGTH];
   LDicomFile          *m_pInput;
   LDicomFile           m_MyInput;
   LDicomFile           m_MyScratch;
   L_BOOL               m_bConformance;
   DICOMMODULE          m_Module;
   CONFORMANCECALLBACK  m_pfnDebug;
   L_VOID              *m_pDebug;

   L_INT                *m_pCharacterSets;
   L_INT                m_nCharacterSetCount;

   static L_BOOL IsCmpFunctionOk(L_CHAR* functionName);
   static L_INT CallFltLoadBuffer(L_UCHAR* pInput, L_SIZE_T nLength, L_UCHAR* pOutput, L_INT nFormat, L_INT32 nWidth, L_INT32 nHeight, L_UINT nBitsPerPixel, L_VOID* pLeadHdr, L_UINT uFlags);
   static L_INT CallFltSaveBuffer(L_UCHAR* pInput, L_VOID* pbiInput, L_UCHAR* pOutput, L_SIZE_T* pdwSize, L_INT nQFactor, L_UINT uFlags, L_UINT nFormat, L_UCHAR* pTopBuffer, L_INT nTopBufferHeight);

   static L_BOOL IsImgCorFunctionOk(L_CHAR* functionName);
   static L_INT CallApplyModalityLUT(pBITMAPHANDLE pBitmap, L_UINT16 *pLUT, pDICOMLUTDESCRIPTOR pLUTDescriptor, L_UINT uFlags);
   static L_INT CallApplyLinearModalityLUT(pBITMAPHANDLE pBitmap, L_DOUBLE fIntercept, L_DOUBLE fSlope, L_UINT uFlags);
   static L_INT CallApplyVOILUT(pBITMAPHANDLE pBitmap, L_UINT16 *pLUT, pDICOMLUTDESCRIPTOR pLUTDescriptor, L_UINT uFlags);
   static L_INT CallApplyLinearVOILUT(pBITMAPHANDLE pBitmap, L_DOUBLE fCenter, L_DOUBLE fWidth, L_UINT uFlags);
   static L_INT CallGetMinMaxVal(pBITMAPHANDLE pBitmap, L_INT *pMinVal, L_INT *pMaxVal);
   static L_INT CallGetLinearVOILUT(pBITMAPHANDLE pBitmap, L_DOUBLE *pCenter, L_DOUBLE *pWidth, L_UINT uFlags);
   static L_INT CallCountLUTColors(RGBQUAD *pLUT, L_UINT ulLLUTLen, L_UINT *pNumberOfEntries, L_INT *pFirstIndex, L_UINT uFlags);
#ifdef CAN_HAVE_LUT16
   static L_INT CallCountLUTColorsExt(L_RGBQUAD16 *pLUT, L_UINT ulLLUTLen, L_UINT *pNumberOfEntries, L_INT *pFirstIndex, L_UINT uFlags);
#endif // #ifdef CAN_HAVE_LUT16
   static L_INT CallShiftBitmapData(pBITMAPHANDLE  pDstBitmap, pBITMAPHANDLE  pSrcBitmap, L_UINT uSrcLowBit, L_UINT uSrcHighBit, L_UINT uDstLowBit, L_UINT uDstBitsPerPixel);
   FLTJ2KDECOMPRESSFRAME   m_fltJ2KDecompressFrame;

   HINSTANCE               m_hANN;
   pL_ANNLOADMEMORY        pfnAnnLoadMemory;
   pL_ANNSAVEMEMORY        pfnAnnSaveMemory;
   pL_ANNDELETEPAGEMEMORY  pfnAnnDeletePageMemory;   
   pL_ANNGETPOINTS         m_pfnAnnGetPoints;
   pL_ANNGETPOINTCOUNT     m_pfnAnnGetPointCount;
   pL_ANNGETFILLMODE       m_pfnAnnGetFillMode;
   pL_ANNGETTEXTLEN        m_pfnAnnGetTextLen;
   pL_ANNGETTEXT           m_pfnAnnGetText;
   pL_ANNGETRECT           m_pfnAnnGetRect;
   pL_ANNGETTYPE           m_pfnAnnGetType;
   pL_ANNCREATE            m_pfnAnnCreate;
   pL_ANNSETPOINTS         m_pfnAnnSetPoints;
   pL_ANNDEFINE            m_pfnAnnDefine;
   pL_ANNSETRECT           m_pfnAnnSetRect;
   pL_ANNSETFILLMODE       m_pfnAnnSetFillMode;
   pL_ANNSETTEXT           m_pfnAnnSetText;

   pCONVERTLEADANNOBJTODICOMANNPROC m_pDICOMAnnConversionProc;
   L_VOID*                          m_pDICOMAnnConversionUserData;   



   L_VOID        ResetDS                (L_BOOL bClose);
   L_UINT16      CopyTreeDS             (pDICOMELEMENT pDstParent, LDicomDS *pSrcDS, pDICOMELEMENT pSrcParent);

#if defined (LEADTOOLS_V16_OR_LATER)
   L_UINT16      CopyTreeDS             (pDICOMELEMENT pDstParent, LDicomDS *pSrcDS, pDICOMELEMENT pSrcParent, COPYDSCALLBACK pfnCallback, L_VOID *pUserData);
#endif

   
   L_UINT16      GetDefaultVR           (pDICOMELEMENT pElement, L_UINT32 nTag, L_BOOL bParent);
   L_UINT16      GetSizeVR              (L_UINT16 nVR);
   L_BOOL        VerifyVR               (pDICOMELEMENT pElement, L_UINT16 nType);
   L_BOOL        GetTS                  (L_UINT16 *pnFlags, L_INT32 *pnCompression, L_CHAR *pszValue);
   L_BOOL        SetTS                  (L_UINT16 nFlags, L_INT32 nCompression);
   L_BOOL        GetClass               (L_UINT32 *pnClass, L_CHAR *pszUID);
   L_BOOL        SetClass               (L_UINT32 nClass);

   L_BOOL        VerifyTypeValue        (pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 *pnCount, L_UINT16 nType);
   L_BOOL        GetTypeValue           (pDICOMELEMENT pElement);
   L_BOOL        SetTypeValue           (pDICOMELEMENT pElement, L_CHAR *pValue, L_UINT32 nCount, L_UINT32 uFlags=0);

   L_BOOL        GetFileBinaryValue     (pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nOffset, L_UINT32 nLength);
   L_BOOL        SetFileBinaryValue     (pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nOffset, L_UINT32 nLength);
   L_BOOL        InsertFileBinaryValue  (pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nOffset, L_UINT32 nLength);
   L_BOOL        DeleteFileBinaryValue  (pDICOMELEMENT pElement, L_UINT32 nOffset, L_UINT32 nLength);
   L_VOID        ConvertFileBinaryValue (L_VOID *pBuffer, L_UINT32 nLength, L_UINT16 nVR, L_BOOL bLittleEndian);
   L_UINT32      GetFileBinaryIndex     (pDICOMELEMENT pElement, L_UINT32 *nOffset);
   L_BOOL        CopyFileBinaryValue    (pDICOMELEMENT pElement, L_UINT32 nOffset1, L_UINT32 nLength, L_UINT16 nVR, L_BOOL bLittleEndian1, LDicomFile *pFile2, L_UINT32 nOffset2, L_BOOL bLittleEndian2);


   L_VOID        DelSpaces              (L_CHAR *pszText, L_UINT32 nLength);
   L_VOID        DelSpaces              (L_CHAR *pszText);

   L_UINT32      GetLengthSequence      (pDICOMELEMENT pElement, L_UINT16 nFlags);
   L_UINT32      GetLengthGroup         (pDICOMELEMENT pElement, L_UINT16 nFlags);
   L_UINT32      GetLengthElement       (L_UINT32 nTag, L_UINT16 nVR, L_UINT32 nLength, L_BOOL bLittleEndian, L_BOOL bExplicitVR);
   L_UINT32      GetOffsetElement       (pDICOMELEMENT pElement, L_UINT16 nFlags);

   L_UINT16      ReadSequence           (LDicomFile *pInput, pDICOMELEMENT hParent, L_UINT16 nFlags, L_UINT32 *pnLength);
   L_UINT16      WriteSequence          (LDicomFile *pOutput, pDICOMELEMENT pElement, L_UINT16 nFlags);

   L_UINT16      ReadElement            (LDicomFile *pInput, L_UINT32 *pnTag, L_UINT16 *pnVR, L_UINT32 *pnLength, L_BOOL bLittleEndian, L_BOOL bExplicitVR);
   L_UINT16      WriteElement           (LDicomFile *pOutput, L_UINT32 nTag, L_UINT16 nVR, L_UINT32 nLength, L_BOOL bLittleEndian, L_BOOL bExplicitVR);

   L_BOOL        Read                   (LDicomFile *pInput, L_VOID *pBuffer, L_UINT32 nLength, L_UINT16 nVR, L_BOOL bLittleEndian);
   L_BOOL        Write                  (LDicomFile *pOutput, L_VOID *pBuffer, L_UINT32 nLength, L_UINT16 nVR, L_BOOL bLittleEndian);

   L_BOOL        InsertModule           (pDICOMELEMENT pParent, L_UINT32 nModule, L_UINT32 nIndex, L_BOOL bOptional);
   L_VOID        InsertModule           (pDICOMELEMENT pParentElement, pDICOMIOD pParentIOD, L_BOOL bOptional);
   pDICOMIOD     VerifyModule           (pDICOMELEMENT pParent, L_UINT32 nModule);
   L_UINT32      GetCountElementIOD     (pDICOMIOD pIOD, L_BOOL bOptional);

   pDICOMELEMENT GetItemKey             (pDICOMELEMENT pElement);
   pDICOMELEMENT FindAttachKey          (pDICOMELEMENT pElement, L_UINT32 nTag);
   L_VOID        RefreshKeys            ();
   L_UINT32      GetAvailableKey        ();
   L_VOID        InsertModuleKey        (pDICOMELEMENT pParentElement, pDICOMIOD pParentIOD, L_BOOL bOptional);

   L_BOOL        ComputeResolutionFromTag(pDICOMELEMENT pElement, L_UINT32 uTag, pIMAGEINFO pInfo);
   L_UINT16      GetInfoImage           (pDICOMELEMENT pElement, pIMAGEINFO pInfo, L_UINT32 nIndex, L_BOOL bPalette);
   L_UINT16      SetInfoImage           (pDICOMELEMENT pElement, pIMAGEINFO pInfo, pBITMAPHANDLE pBitmap, L_UINT32 uFrameIndex, L_INT32 nPhotometric, L_BOOL bSave,L_UINT uFlags);
   L_VOID        FreeInfoImage          (pIMAGEINFO pInfo);
   L_UINT16      DecodeSegmentedPaletteColorLUTData(pIMAGEINFO pInfo);
   L_UINT16      DecodeSegmentedPaletteColorLUTData(L_UINT16 *pPaletteBuffer,L_UINT32  uNumEntries,L_UINT32  uSegmentedDataSize);   

   L_UINT16      LoadImage              (pDICOMELEMENT pElement, pBITMAPHANDLE pBitmap,L_UINT uStructSize ,L_UINT32 nIndex, L_INT32 nBitsPerPixel, L_INT32 nOrder, L_UINT uFlags ,FILEREADCALLBACK pfnCallback, L_VOID *pUserData);
   L_UINT16      SetPaintParams         (pBITMAPHANDLE pBitmap,IMAGEINFO * pImageInfo);
   //L_BOOL        CanApplyModalityLUT    (pBITMAPHANDLE  pBitmap,L_BOOL VOILUTWillBeApplied,L_INT nOriginalBitmapMinVal,L_INT nOriginalBitmapMaxVal, L_UINT32 uFrameIndex);
   L_UINT16      ApplyModalityLUT       (pBITMAPHANDLE pBitmap,L_UINT uStructSize ,IMAGEINFO * pImageInfo,L_INT nOriginalBitmapMinVal,L_INT nOriginalBitmapMaxVal,L_UINT uApplyModalityLUTFlags);
   L_UINT16      ApplyVOILUT            (pBITMAPHANDLE pBitmap,L_UINT uStructSize ,IMAGEINFO * pImageInfo, L_UINT uFlags);   
   L_UINT16      SaveImage              (pDICOMELEMENT pElement, pBITMAPHANDLE pBitmap, L_UINT32 nIndex, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor, L_UINT uFlags,FILESAVECALLBACK pfnCallback, L_VOID *pUserData);
   L_UINT16      SetVOIlUT              (L_UINT32      uFrameIndex,pDICOMELEMENT pPixelDataEle,pBITMAPHANDLE pBitmap,L_UINT        uFlags);
   L_UINT16      SetNonLinearVOILUT     (L_UINT32 uFrameIndex, pDICOMELEMENT pElement,pBITMAPHANDLE pBitmap);
   L_UINT16      DeleteImage            (pDICOMELEMENT pElement, L_UINT32 nIndex);
   L_UINT16      RefreshImage           (pDICOMELEMENT pElement);

   L_BOOL        IsElementAffectedByCharacterSet(pDICOMELEMENT pElement);

   L_UINT16      DecodeNONE             (pIMAGEINFO pInfo, L_UCHAR *pImage);
   L_UINT16      DecodeRLE              (pIMAGEINFO pInfo, L_UCHAR *pImage);
   L_UINT16      DecodeJPEG             (pIMAGEINFO pInfo, L_UCHAR *pImage);
   L_UINT16      DecodeJ2K              (pIMAGEINFO pInfo, L_UCHAR *pImage,L_UINT32 uImageBuffSize);
   L_UINT16      DecodeJustLibJ2K       (pIMAGEINFO pInfo, L_UCHAR *pImage,L_UINT32 uImageBuffSize);
   L_UINT16      EncodeNONE             (L_UCHAR** ppImage, pBITMAPHANDLE pBitmap, L_UINT32 *pnLength);
   L_UINT16      EncodeRLE              (L_UCHAR** ppImage, pBITMAPHANDLE pBitmap, L_UINT32 *pnLength);
   L_UINT16      EncodeJPEG             (L_UCHAR** ppImage, pBITMAPHANDLE pBitmap, L_INT32 nQFactor, L_UINT32 *pnLength);
   L_UINT16      EncodeJ2K              (L_UCHAR** ppImage, pBITMAPHANDLE pBitmap, L_INT32 nQFactor, L_UINT32 *pnLength,L_BOOL bLossless);

   L_VOID        ConvertYUVtoRGB        (L_UCHAR *pImage, L_UINT32 nPlaneSize, L_BOOL bPlanar);
   L_VOID        Convert12to16          (L_UCHAR *pImage, L_INT nWidth, L_INT nHeight);
   //L_UINT16      GenerateGrayLUT        (pBITMAPHANDLE pBitmap, L_UINT32 nBitsStored, L_UINT32 nHighBit, L_UINT32 nMinVal, L_UINT32 nMaxVal);
   HPALETTE      CreateColorPalette     (L_RGBQUAD *pPalette, L_UINT32 nColors);
   HPALETTE      CreateGrayPalette      (L_UINT32 nBitsPerPixel);

   L_BOOL        CreatePrivateCreatorElement(pDICOMELEMENT pElement, L_UINT32 uTag);
   L_BOOL        IsAnnPrivateCreatorElement(pDICOMELEMENT pElement);
   pDICOMELEMENT FindAnnPrivateCreatorElement(L_CHAR *pszPrivateString);
   L_INT         GetXRangeValue(pDICOMELEMENT pElement, L_UINT32 nIndex, L_VOID * pValue, L_UINT16 nType, L_UINT32 nCount);
   L_UINT16      CompressPixelData(L_INT32 nCompression,L_INT32 nQFactor,L_BOOL bKeepPixelDataIntact);   
   L_UINT16      UncompressPixelDataElement(L_BOOL bKeepPixelDataIntact);
   L_UINT16      ChangeTransferSyntax(DICOM_TRANSFER_SYNTAXES NewTransferSyntax,L_INT32 nQFactor,L_BOOL bKeepPixelDataIntact,L_UINT32 uFlags);
   L_UINT16      FixBadElements();
   L_UINT16      ValidateJ2KOptions ( const pFILEJ2KOPTIONS pOptions );

   L_UINT16      AddIODSpecificElements(pDICOMELEMENT pParent,pIMAGEINFO pInfo);

   //---------------------------------------------------------------------------
   // Internal LUT Manipulation Functions
   //---------------------------------------------------------------------------
   
   // Modality LUT
   L_UINT16      ResetModalityLUTAttributes(pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT uStructSize);
   L_UINT16      GetLUTDescriptor(pDICOMELEMENT pParentLUTSequenceItem,L_INT nTag,pDICOMLUTDESCRIPTOR pLUTDescriptor,DICOMLUTDESCRIPTORTYPE DescriptorType);
   L_UINT16      SetModalityLUTDescriptor(pDICOMELEMENT pParentLUTSequenceItem,pDICOMLUTDESCRIPTOR pLUTDescriptor);
   L_UINT16      SetLUTDescriptor(pDICOMELEMENT pNeighborElement,pDICOMLUTDESCRIPTOR pLUTDescriptor, L_INT nTag);
   L_UINT16      VerifyModalityLUT(pDICOMMLUTATTRIBS pModalityLUTAttributes);
   L_UINT16      Set8BitLUT(pDICOMELEMENT pLUTDataElement,L_UINT16 *pLUTData,L_UINT uDataSize);
   L_UINT16      InsertModalityLUTSequence(pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT16 *pLUTData,L_UINT uDataSize);

   L_UINT32      SearchModalityLUT(pDICOMELEMENT pNeighbor, pDICOMMLUTATTRIBS pModalityLUTAttributes);


   // v16

  pDICOMELEMENT CreateMultiFrameFunctionalGroupElement(L_UINT32 uTagMultiframeFunctionalGroup, L_UINT32 uElementSequence, L_UINT32 uElement, L_UINT uItemIndex, L_UINT uFlag);
  pDICOMELEMENT FindFunctionalGroupElement(L_UINT32 uTagMultiframeFunctionalGroup, L_UINT32 uElementSequence, L_UINT32 uElement, L_UINT uItemIndex);
  L_UINT32 SearchWindowCenterWidth(pDICOMELEMENT pNeighbor, L_UINT uWindowIndex, pDICOMWINDOWATTRIBS  pWindowAttributes);

#if defined(LEADTOOLS_V16_OR_LATER) 
   L_INT32       GetItemCount(pDICOMELEMENT pParent);
   pDICOMELEMENT GetItem(pDICOMELEMENT pParent, L_INT nItemIndex);
   pDICOMELEMENT FindChildElement(pDICOMELEMENT pParent, L_UINT32 uTag);
   pDICOMELEMENT FindMultiFrameFunctionalGroupElement(L_UINT32 uElementSequence, L_UINT32 uElement, L_UINT uItemIndex);
   L_BOOL        CanAddMultiFrameFunctionalGroup();
   pDICOMELEMENT FindMultiFrameFunctionGroupVOILUT(L_UINT32 uFrameIndex);
   pDICOMELEMENT FindMultiFrameFunctionGroupModalityLUT(L_UINT32 uFrameIndex);
   L_VOID        GetMultiFrameFunctionGroupLutElements(L_UINT32 uFlags, L_UINT32 uFrameIndex, pDICOMELEMENT *ppElementVOILUT, L_BOOL *pbSharedExistingVOILUT, pDICOMELEMENT *ppElementModalityLUT, L_BOOL *pbSharedExistingModalityLUT);
   L_UINT32      FindFunctionalGroupWindowCenterWidth(L_UINT32 uTagMultiframeFunctionalGroup, L_UINT uItemIndex, pDICOMWINDOWATTRIBS pWindowAttributes, L_UINT uWindowIndex);
   pDICOMELEMENT GetSequenceItem(pDICOMELEMENT pSequence, L_UINT uItem, L_UINT32 uTag);
   L_UINT32      FindFunctionalGroupModalityLUT(L_UINT32 uTagMultiframeFunctionalGroup, L_UINT uItemIndex, pDICOMMLUTATTRIBS pModalityLUTAttributes);
   //L_UINT16      GetModalityLUTAttributesMultiFrame(pDICOMMLUTATTRIBS pModalityLUTAttributes ,L_UINT uStructSize ,L_UINT uFlags, L_INT nFrameIndex);
   L_BOOL        IsSharedMultiFrameFunctionalGroup(pDICOMELEMENT pElement);
   L_BOOL        IsPerFrameMultiFrameFunctionalGroup(pDICOMELEMENT pElement);

   L_BOOL        InsertItemPerFrameMultiFrameFunctionalGroup(L_INT nIndex);
   L_BOOL        DeleteItemPerFrameMultiFrameFunctionalGroup(L_INT nItemFirst, L_INT nItemLast);
   L_BOOL        DeleteMultiFrameSequence(L_UINT32 uElementSequence, L_UINT32 uFrameIndex);
#endif




   // Palette Color
   L_UINT16      ResetPaletteColorLUTAttributes(pDICOMPALCOLORLUTATTRIBS pPaletteColorLUTAttributes,L_UINT uStructSize);
   L_UINT16      GetPaletteColorDescriptor(pDICOMELEMENT pPaletteColorDescriptorElement,pDICOMLUTDESCRIPTOR pLUTDescriptor);
   L_UINT16      VerifyPaletteColorLUT(pDICOMPALCOLORLUTATTRIBS pPaletteColorLUTAttributes);
      

   // VOI LUT 
   pDICOMELEMENT GetVOILUTItem(pDICOMELEMENT pVOILUTSequence,L_UINT uVOILUTIndex);
   L_UINT16      UpdateWindowElement(pDICOMELEMENT pElement, L_UINT uWindowIndex,L_DOUBLE fValue,L_INT nTag);
   L_UINT16      DeleteVOILUTItemElements(pDICOMELEMENT pVOILUTItem,L_INT nTagsToDelete[]);
   L_UINT16      InsertVOILUTItemElements(pDICOMELEMENT pVOILUTItem,L_INT nTagsToAdd[],L_UINT uItemCount,L_BOOL bUseVROW);

   //---------------------------------------------------------------------------
   // Internal Overlay Manipulation Functions
   //---------------------------------------------------------------------------
   L_INT    GetOverlayGroupMaskByIndex(L_UINT uOverlayIndex, L_BOOL *pIsActivationLayerOnly);
   L_INT    GetOverlayGroupMaskByIndexActLayer(L_UINT uOverlayIndex);
   L_UINT16 GetOverlayBitmap  (L_UINT uOverlayIndex   ,pBITMAPHANDLE  pBitmap ,L_UINT uStructSize,L_UINT uOverlayFrameIndex,L_UINT uFlags);
   L_UINT16 SetOverlayBitmap  (L_UINT uOverlayIndex   ,pBITMAPHANDLE  pBitmap ,L_UINT uOverlayFrameIndex , L_UINT uFlags);
   L_UINT16 LoadOverlaysIntoBitmap(pBITMAPHANDLE pBitmap,L_UINT uStructSize,L_UINT uFlags);
   L_UINT16 SaveOverlaysFromBitmap(pBITMAPHANDLE pBitmap,L_UINT uFlags);
   L_UINT16 DeleteAllOverlays(L_UINT uFlags);
   
   //---------------------------------------------------------------------------
   // Internal Waveform Functions
   //--------------------------------------------------------------------------- 
   L_UINT16 GetChannelStatusWritten(L_UINT16 uStatus, L_UINT* puIncludedCount, L_CHAR* pszStatusWritten);

   //---------------------------------------------------------------------------
   // Internal Presentation State Functions
   //--------------------------------------------------------------------------- 
   pDICOMELEMENT  GetPresStateImageRefElement(pDICOMELEMENT pRefSeriesSQItem, L_UINT uImageIndex);
   L_VOID         SetTagInstanceNumber(L_INT32 nInstance);
   L_VOID         SetStringTagValue(L_UINT32 uTag, L_TCHAR* pszValue);
   L_BOOL         IsEmptySequence(pDICOMELEMENT pElementSequence);
   L_BOOL         ValidImage(L_TCHAR* pszImageFileName);
   L_BOOL         ValidImage(LDicomDS* pDS);
   L_VOID         GetNewImageInfo(L_CHAR* pszClassUID, L_CHAR* pszInstanceUID, L_CHAR* pszSeriesUID, LDicomDS* pDS);
   L_VOID         GetNewImageInfo(L_CHAR* pszClassUID, L_CHAR* pszInstanceUID, L_CHAR* pszSeriesUID, L_TCHAR* pszImageFileName);
   L_TCHAR*       GetGraphicObjLayerName(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uObjIndex);
   L_TCHAR*       GetTextObjLayerName(pDICOMELEMENT pGraphicAnnSQItem, L_UINT uObjIndex); 
   pDICOMELEMENT  AnnSequenceOfLayer(L_CHAR* pszLayerName);
   pDICOMELEMENT  CopyImageToLayer(pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszImageSOPInstance);
   L_TCHAR*       GetFirstAnnSequenceLayerName();
   L_TCHAR*       GetAnnSequenceLayerName(pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       RemoveImageRefFromAllAnnotationSequences(L_CHAR* pszSOPInstanceUID);
   L_UINT16       ConvertLEADtoDICOMPoints(pANNPOINT pLeadPoints, pDICOMANNPOINT pDicPoints, L_UINT uLeadPointsCount);
   L_UINT16       ConvertLineObject(HANNOBJECT hLineObj, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertRectangleObject(HANNOBJECT hRectangle, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertEllipseObject(HANNOBJECT hEllipse, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertPolylineObject(HANNOBJECT hPolyLine, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertPolygoneObject(HANNOBJECT hPolygone, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertPointerObject(HANNOBJECT hPointer, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertFreehandObject(HANNOBJECT hFreeHand, pDICOMELEMENT pGraphicAnnSQItem);
   L_INT          GetAnnText(HANNOBJECT hText, L_CHAR* pszAnnText, L_UINT uTextLength);
   L_UINT16       ConvertTextObject(HANNOBJECT hText, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertProtractorObject(HANNOBJECT hProtractor, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertPointObject(HANNOBJECT hPointAnn, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertInterpolatedLineObject(HANNOBJECT hInterpolatedLine, pDICOMELEMENT pGraphicAnnSQItem, L_BOOL bFillMode);
   L_UINT16       ConvertTextPointerObject(HANNOBJECT hText, pDICOMELEMENT pGraphicAnnSQItem);
   L_UINT16       ConvertDICOMPointIntoLEADObject(pDICOMGRAPHICOBJECT pGraphicObject, pHANNOBJECT phAnnObject);
   L_UINT16       ConvertDICOMPolylineIntoLEADObject(pDICOMGRAPHICOBJECT pGraphicObject, pHANNOBJECT phAnnObject);   
   L_UINT16       ConvertDICOMEllipseIntoLEADObject(pDICOMGRAPHICOBJECT pGraphicObject, pHANNOBJECT phAnnObject);
   L_UINT16       ConvertDICOMCircleIntoLEADObject(pDICOMGRAPHICOBJECT pGraphicObject, pHANNOBJECT phAnnObject);   
   L_UINT16       GetEllipseBounding(pDICOMGRAPHICOBJECT pGraphicObject, L_FLOAT* pfMaxY, L_FLOAT* pfMinY, L_FLOAT* pfMaxX, L_FLOAT* pfMinX);
   L_UINT16       GetCircleRadius(pDICOMGRAPHICOBJECT pGraphicObject, L_FLOAT* pfRadius);
   L_UINT16       GetObjPointsCountBeforeConversion(HANNOBJECT hAnnObject, L_UINT* puPointsCount);   
   L_UINT16       ConvertDICOMInterPolatedLineIntoLEADObject(pDICOMGRAPHICOBJECT pGraphicObject, pHANNOBJECT phAnnObject);

   L_TCHAR        *GetStringValue    (pDICOMELEMENT pElement, L_UINT32 nIndex);
   L_UINT16       RefreshCharacterSet();


public:
   // (LEAD's Internal use only)
   L_UINT16      GetFrameBinaryData(pDICOMELEMENT pPixelDataElement,L_INT nFrameIndex,L_UCHAR *pFrameDataBuffer,L_UINT *puFrameDataSize);
private:
   L_UINT16 AutoScaleModalityLUT(pDICOMMLUTATTRIBS pModalityLUTAttributes, pBITMAPHANDLE pBitmap, L_UINT uFlags);
   L_UINT16 GetModalityLUTAttributes(L_UINT32 uFrameIndex, pDICOMMLUTATTRIBS pModalityLUTAttributes, L_UINT uStructSize, pBITMAPHANDLE pBitmap, L_UINT uFlags);
};

#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)

   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API HDICOMDS        EXT_FUNCTION L_DicomCreateDS          (L_TCHAR *pszPath);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomFreeDS            (HDICOMDS hDS);

   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomInitDS            (HDICOMDS hDS, L_UINT32 nClass, L_UINT16 nFlags);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomGetInfoDS         (HDICOMDS hDS, L_UINT32 *pnClass, L_UINT16 *pnFlags);

   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomInitCS            (HDICOMDS hDS, L_UINT16 nCommand, L_BOOL bRequest);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomGetInfoCS         (HDICOMDS hDS, L_UINT16 *pnCommand, L_BOOL *pbRequest);

   //---------------------------------------------------------------------------
   // Input and Output Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomLoadDS            (HDICOMDS hDS, L_TCHAR *pszName, L_UINT16 nFlags);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomLoadDSMemory      (HDICOMDS hDS, L_UCHAR *pBuffer, L_UINT32 uBufferSize, L_UINT16 nFlags);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomSaveDS            (HDICOMDS hDS, L_TCHAR *pszName, L_UINT16 nFlags);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomCopyDS            (HDICOMDS hDstDS, pDICOMELEMENT pDstParent, HDICOMDS hSrcDS, pDICOMELEMENT pSrcParent);
#if defined(LEADTOOLS_V16_OR_LATER)
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomCopyDSExt         (HDICOMDS hDstDS, pDICOMELEMENT pDstParent, HDICOMDS hSrcDS, pDICOMELEMENT pSrcParent, COPYDSCALLBACK pfnCallback, L_VOID *pUserData);
#endif
   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomInsertElement     (HDICOMDS hDS, pDICOMELEMENT pNeighbor, L_BOOL bChild, L_UINT32 nTag, L_UINT16 nVR, L_BOOL bSequence, L_UINT32 nIndex);
   L_LTDIC_API pDICOMMODULE    EXT_FUNCTION L_DicomInsertModule      (HDICOMDS hDS, L_UINT32 nModule, L_BOOL bOptional);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomInsertKey         (HDICOMDS hDS, pDICOMELEMENT pParent, L_TCHAR *pszKey, L_BOOL bOptional);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomInsertBinaryValue      (HDICOMDS hDS, pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nOffset, L_UINT32 nLength);

   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomDeleteElement     (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomDeleteModule      (HDICOMDS hDS, L_UINT32 nModule);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomDeleteKey         (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomResetDS           (HDICOMDS hDS);

   //---------------------------------------------------------------------------
   // Iteration Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetRootElement    (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetParentElement  (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetChildElement   (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bVolatile);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetFirstElement   (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree, L_BOOL bVolatile);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetLastElement    (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree, L_BOOL bVolatile);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetPrevElement    (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree, L_BOOL bVolatile);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetNextElement    (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree, L_BOOL bVolatile);
   L_LTDIC_API L_UINT32        EXT_FUNCTION L_DicomGetLevelElement   (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API L_UINT32        EXT_FUNCTION L_DicomGetCountModule    (HDICOMDS hDS);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomExistsElement     (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomIsVolatileElement (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomConformanceDS     (HDICOMDS hDS, CONFORMANCECALLBACK pfnCallback, L_VOID *pUserData);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomSetDebugDS        (HDICOMDS hDS, CONFORMANCECALLBACK pfnCallback, L_VOID *pUserData);

   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetRootKey        (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetParentKey      (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetChildKey       (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetFirstKey       (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetLastKey        (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetPrevKey        (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomGetNextKey        (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree);
   L_LTDIC_API L_TCHAR*        EXT_FUNCTION L_DicomGetValueKey       (HDICOMDS hDS, pDICOMELEMENT pElement);

   //---------------------------------------------------------------------------
   // Searching Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindFirstElement  (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nTag, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindLastElement   (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nTag, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindPrevElement   (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindNextElement   (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree);
   L_LTDIC_API pDICOMMODULE    EXT_FUNCTION L_DicomFindModule        (HDICOMDS hDS, L_UINT32 nModule);
   L_LTDIC_API pDICOMMODULE    EXT_FUNCTION L_DicomFindIndexModule   (HDICOMDS hDS, L_UINT32 nIndex);

   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindFirstKey      (HDICOMDS hDS, pDICOMELEMENT pElement, L_TCHAR *pszKey, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindLastKey       (HDICOMDS hDS, pDICOMELEMENT pElement, L_TCHAR *pszKey, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindPrevKey       (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindNextKey       (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomIsBadPixelData    (HDICOMDS hDS,L_UINT32 * pBadCount,L_UINT32 * pGoodCount);

   //---------------------------------------------------------------------------
   // Retrieval Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomGetPreamble       (HDICOMDS hDS, L_UCHAR *pPreamble, L_UINT16 nLength);
   L_LTDIC_API L_UINT32        EXT_FUNCTION L_DicomGetCountValue     (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomFreeValue         (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomGetBinaryValue    (HDICOMDS hDS, pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nLength);
   L_LTDIC_API L_UCHAR*        EXT_FUNCTION L_DicomGetCharValue      (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API L_INT16*        EXT_FUNCTION L_DicomGetShortValue     (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API L_INT32*        EXT_FUNCTION L_DicomGetLongValue      (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API L_UINT32        EXT_FUNCTION L_DicomGetLong64Value    (HDICOMDS hDS, pDICOMELEMENT pElement, L_INT64 *pValue, L_UINT32 nIndex,L_UINT32 nCount);
   L_LTDIC_API L_FLOAT*        EXT_FUNCTION L_DicomGetFloatValue     (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API L_DOUBLE*       EXT_FUNCTION L_DicomGetDoubleValue    (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API L_CHAR*         EXT_FUNCTION L_DicomGetStringValueA   (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API L_TCHAR*        EXT_FUNCTION L_DicomGetStringValue    (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API pVALUEAGE       EXT_FUNCTION L_DicomGetAgeValue       (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API pVALUEDATE      EXT_FUNCTION L_DicomGetDateValue      (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API pVALUETIME      EXT_FUNCTION L_DicomGetTimeValue      (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API pVALUEDATETIME  EXT_FUNCTION L_DicomGetDateTimeValue  (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);   
   L_LTDIC_API L_UINT32        EXT_FUNCTION L_DicomGetConvertValue   (HDICOMDS hDS, pDICOMELEMENT pElement, L_TCHAR *Destination, L_UINT32 DestSizeInWords);
   L_LTDIC_API L_UINT32        EXT_FUNCTION L_DicomGetCountImage     (HDICOMDS hDS, pDICOMELEMENT pElement);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomGetInfoImage      (HDICOMDS hDS, pDICOMELEMENT pElement, pDICOMIMAGE pInfo, L_UINT32 nIndex);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomGetImage          (HDICOMDS hDS, pDICOMELEMENT pElement, pBITMAPHANDLE pBitmap,L_UINT uStructSize ,L_UINT32 nIndex, L_INT32 nBitsPerPixel, L_INT32 nOrder, L_UINT uFlags , FILEREADCALLBACK pfnCallback, L_VOID *pUserData);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomGetImageList      (HDICOMDS hDS, pDICOMELEMENT pElement, HBITMAPLIST hList, L_UINT32 nIndex, L_UINT32 nCount, L_INT32 nBitsPerPixel, L_INT32 nOrder,L_UINT uFlags);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomGetKeepPixelDataIntactFlag(HDICOMDS hDS);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomGetJ2KOptions( HDICOMDS hDS,pFILEJ2KOPTIONS pOptions, L_INT nSize );
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomGetDefaultJ2KOptions( HDICOMDS hDS,pFILEJ2KOPTIONS pOptions, L_INT nSize );

   //---------------------------------------------------------------------------
   // Modification Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomSetPreamble       (HDICOMDS hDS, L_UCHAR *pPreamble, L_UINT16 nLength);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetBinaryValue    (HDICOMDS hDS, pDICOMELEMENT pElement, L_VOID *pValue, L_UINT32 nLength);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetCharValue      (HDICOMDS hDS, pDICOMELEMENT pElement, L_UCHAR *pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetShortValue     (HDICOMDS hDS, pDICOMELEMENT pElement, L_INT16 *pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetLongValue      (HDICOMDS hDS, pDICOMELEMENT pElement, L_INT32 *pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetFloatValue     (HDICOMDS hDS, pDICOMELEMENT pElement, L_FLOAT *pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetDoubleValue    (HDICOMDS hDS, pDICOMELEMENT pElement, L_DOUBLE *pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetStringValue    (HDICOMDS hDS, pDICOMELEMENT pElement, L_TCHAR *pValue, L_UINT32 nCount, L_UINT32 uCharacterSet);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetAgeValue       (HDICOMDS hDS, pDICOMELEMENT pElement, pVALUEAGE pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetDateValue      (HDICOMDS hDS, pDICOMELEMENT pElement, pVALUEDATE pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetTimeValue      (HDICOMDS hDS, pDICOMELEMENT pElement, pVALUETIME pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetDateTimeValue  (HDICOMDS hDS, pDICOMELEMENT pElement, pVALUEDATETIME pValue, L_UINT32 nCount);
   L_LTDIC_API L_BOOL          EXT_FUNCTION L_DicomSetConvertValue   (HDICOMDS hDS, pDICOMELEMENT pElement, L_TCHAR *strText, L_UINT32 nCount);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomInsertImage       (HDICOMDS hDS, pDICOMELEMENT pElement, pBITMAPHANDLE pBitmap, L_UINT32 nIndex, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor,L_UINT uFlags ,FILESAVECALLBACK pfnCallback, L_VOID *pUserData);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomInsertImageList   (HDICOMDS hDS, pDICOMELEMENT pElement, HBITMAPLIST hList, L_UINT32 nIndex, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor, L_UINT uFlags );
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomSetImage          (HDICOMDS hDS, pDICOMELEMENT pElement, pBITMAPHANDLE pBitmap, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor,L_UINT uFlags, FILESAVECALLBACK pfnCallback, L_VOID *pUserData);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomSetImageList      (HDICOMDS hDS, pDICOMELEMENT pElement, HBITMAPLIST hList, L_INT32 nCompression, L_INT32 nPhotometric, L_INT32 nBitsPerPixel, L_INT32 nQFactor,L_UINT uFlags);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomDeleteImage       (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT32 nIndex, L_UINT32 nCount);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomChangeTransferSyntax(HDICOMDS hDS, L_TCHAR *pszUID,L_INT32 nQFactor/*For lossy*/,L_UINT32 uFlags);
   L_LTDIC_API L_VOID          EXT_FUNCTION L_DicomSetKeepPixelDataIntactFlag(HDICOMDS hDS, L_BOOL bSet);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomSetJ2KOptions( HDICOMDS hDS,const pFILEJ2KOPTIONS pOptions);

   //---------------------------------------------------------------------------
   // Overlay Manipulation Functions
   //---------------------------------------------------------------------------

   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetOverlayCount            (HDICOMDS hDS ,L_UINT * pCount);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetOverlayAttributes       (HDICOMDS hDS ,L_UINT uOverlayIndex ,pOVERLAYATTRIBUTES    pOverlayAttributes   ,  L_UINT   uStructSize,L_INT *pGroupNumber,L_BOOL * pIsOverlayInDataset , L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetOverlayActivationLayer  (HDICOMDS hDS ,L_UINT uOverlayIndex ,L_TCHAR *             pActivationLayer     ,  L_UINT   uLength);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetOverlayBitmap           (HDICOMDS hDS ,L_UINT uOverlayIndex ,pBITMAPHANDLE         pBitmap              ,  L_UINT   uStructSize,L_UINT uFlags);   
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetOverlayBitmapList       (HDICOMDS hDS ,L_UINT uOverlayIndex ,HBITMAPLIST           hList                ,  L_UINT32 uOverlayFrameIndex, L_UINT32 uCount,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetOverlayAttributes       (HDICOMDS hDS ,L_UINT uOverlayIndex ,pOVERLAYATTRIBUTES    pOverlayAttributes,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetOverlayBitmap           (HDICOMDS hDS ,L_UINT uOverlayIndex ,pBITMAPHANDLE         pBitmap,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetOverlayBitmapList       (HDICOMDS hDS ,L_UINT uOverlayIndex ,HBITMAPLIST           hList,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomDeleteOverlay              (HDICOMDS hDS ,L_UINT uOverlayIndex ,L_UINT uFlags);

   
   //---------------------------------------------------------------------------
   // Digital signatures
   //---------------------------------------------------------------------------

   L_LTDIC_API L_UINT32       EXT_FUNCTION L_DicomGetSignaturesCount    (HDICOMDS hDS ,pDICOMELEMENT pItem);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomGetSignature          (HDICOMDS hDS ,pDICOMELEMENT pItem, L_UINT32 uIndex);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomFindSignature         (HDICOMDS hDS ,const L_TCHAR* pszSignatureUID);
   L_LTDIC_API L_TCHAR*       EXT_FUNCTION L_DicomGetSignatureUID       (HDICOMDS hDS ,pDICOMELEMENT pSignatureItem);
   L_LTDIC_API pVALUEDATETIME EXT_FUNCTION L_DicomGetSignatureDateTime  (HDICOMDS hDS ,pDICOMELEMENT pSignatureItem);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomSaveCertificate       (HDICOMDS hDS ,pDICOMELEMENT pSignatureItem, const L_TCHAR* pszFilename, L_UINT16 uFormat);
   L_LTDIC_API L_UINT32       EXT_FUNCTION L_DicomGetSignedElementsCount(HDICOMDS hDS ,pDICOMELEMENT pSignatureItem);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomGetSignedElement      (HDICOMDS hDS ,pDICOMELEMENT pSignatureItem, L_UINT32 uIndex);
   L_LTDIC_API L_TCHAR*       EXT_FUNCTION L_DicomGetMacTransferSyntax  (HDICOMDS hDS ,pDICOMELEMENT pSignatureItem);
   L_LTDIC_API L_TCHAR*       EXT_FUNCTION L_DicomGetMacAlgorithm       (HDICOMDS hDS ,pDICOMELEMENT pSignatureItem);
   L_LTDIC_API L_VOID         EXT_FUNCTION L_DicomDeleteSignature       (HDICOMDS hDS ,pDICOMELEMENT pSignatureItem);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomVerifySignature       (HDICOMDS hDS ,pDICOMELEMENT pSignatureItem, L_UINT16 uReserved);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomCreateSignature       (HDICOMDS hDS,
                                                               pDICOMELEMENT  pItem,
                                                               const L_TCHAR*        pszPrivateKeyFile,
                                                               const L_TCHAR*        pszCertificateFile,
                                                               const L_TCHAR*        pszPassword,
                                                               pDICOMELEMENT* ppSignatureItem,
                                                               const L_TCHAR*        pszMacTransferSyntax,
                                                               L_UINT16       uMacAlgorithm,
                                                               L_UINT32*      pElementsToSign,
                                                               L_UINT32       uCount,
                                                               L_UINT16       uSecurityProfile,
                                                               L_UINT16       uReserved);



   //---------------------------------------------------------------------------
   // Waveform Functions
   //---------------------------------------------------------------------------

   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetWaveformGroupCount (HDICOMDS hDS );
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetWaveformGroup      (HDICOMDS hDS ,L_UINT32 uIndex, HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomDeleteWaveformGroup   (HDICOMDS hDS ,L_UINT32 uIndex, L_UINT16 uReserved);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomAddWaveformGroup      (HDICOMDS hDS ,HDICOMWAVEFORMGROUP hDICOMWaveFormGroup, L_UINT16 uFlags, L_UINT32 uIndex);

   
   //---------------------------------------------------------------------------
   // LUT Manipulation Functions
   //---------------------------------------------------------------------------

   //Modality LUT
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetModalityLUTData             (HDICOMDS hDS ,L_UINT16 * pLUTData,L_UINT uDataSize,L_UINT uFlags);
#if defined(LEADTOOLS_V16_OR_LATER)
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetModalityLUTAttributes       (HDICOMDS hDS , L_UINT32 uFrameIndex, pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT uStructSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetModalityLUT                 (HDICOMDS hDS ,L_UINT32 uFrameIndex, pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT16 *pLUTData,L_UINT uDataSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomDeleteModalityLUT              (HDICOMDS hDS ,L_UINT32 uFrameIndex, L_UINT uFlags);
#else
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetModalityLUTAttributes       (HDICOMDS hDS ,pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT uStructSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetModalityLUT                 (HDICOMDS hDS ,pDICOMMLUTATTRIBS pModalityLUTAttributes,L_UINT16 *pLUTData,L_UINT uDataSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomDeleteModalityLUT              (HDICOMDS hDS ,L_UINT uFlags);
#endif

   //Palette Color LUT
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetPaletteColorLUTAttributes   (HDICOMDS hDS ,pDICOMPALCOLORLUTATTRIBS pPaletteColorLUTAttributes,L_UINT uStructSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetPaletteColorLUTData         (HDICOMDS hDS ,L_UINT16 * pLUTData,L_UINT uDataSize,DICOMPALETTECOLORLUTTYPE PaletteColorLUTType,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetPaletteColorLUTAttributes   (HDICOMDS hDS ,pDICOMPALCOLORLUTATTRIBS pPaletteColorLUTAttributes,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetPaletteColorLUTData         (HDICOMDS hDS ,L_UINT16 * pLUTData,L_UINT uDataSize,DICOMPALETTECOLORLUTTYPE PaletteColorLUTType,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomDeletePaletteColorLUT          (HDICOMDS hDS ,L_UINT uFlags);   
   
   // VOI LUT
#if defined(LEADTOOLS_V16_OR_LATER)
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetWindowCount                 (HDICOMDS hDS ,L_UINT32 uFrameIndex, L_UINT* pCount);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetWindow                      (HDICOMDS hDS ,L_UINT32 uFrameIndex, L_UINT uWindowIndex ,pDICOMWINDOWATTRIBS pWindowAttributes,  L_UINT uStructSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetWindow                      (HDICOMDS hDS ,L_UINT32 uFrameIndex, L_UINT uWindowIndex ,pDICOMWINDOWATTRIBS pWindowAttributes ,  L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomDeleteWindow                   (HDICOMDS hDS ,L_UINT32 uFrameIndex, L_UINT uFlags);
#else
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetWindowCount                 (HDICOMDS hDS ,L_UINT * pCount);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetWindow                      (HDICOMDS hDS ,L_UINT uWindowIndex ,pDICOMWINDOWATTRIBS pWindowAttributes ,  L_UINT uStructSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetWindow                      (HDICOMDS hDS ,L_UINT uWindowIndex ,pDICOMWINDOWATTRIBS pWindowAttributes ,  L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomDeleteWindow                   (HDICOMDS hDS ,L_UINT uFlags);
#endif


   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetVOILUTCount                 (HDICOMDS hDS ,L_UINT * pCount);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetVOILUT                      (HDICOMDS hDS ,L_UINT uVOILUTIndex ,pDICOMVOILUTATTRIBS pVOILUTAttributes ,  L_UINT uStructSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomSetVOILUT                      (HDICOMDS hDS ,L_UINT uVOILUTIndex ,pDICOMVOILUTATTRIBS pVOILUTAttributes ,  L_UINT16 * pLUTData,L_UINT uDataSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomGetVOILUTData                  (HDICOMDS hDS ,L_UINT uVOILUTIndex ,L_UINT16 * pLUTData                      ,  L_UINT uDataSize,L_UINT uFlags);
   L_LTDIC_API L_UINT16 EXT_FUNCTION L_DicomDeleteVOILUT                   (HDICOMDS hDS ,L_UINT uFlags);
   
   
   //---------------------------------------------------------------------------
   // Presentation State Functions
   //--------------------------------------------------------------------------- 
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomSetPresStateInfo(HDICOMDS hDS, pDICOMPRESSTATEINFO pPresState);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetPresStateInfo(HDICOMDS hDS, pDICOMPRESSTATEINFO pPresState, L_UINT uStructSize);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomAddPresStateImageRefByFileName(HDICOMDS hDS, L_TCHAR* pszImageFileName, L_INT32* pFrameNumbers , L_UINT uFramesCount);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomAddPresStateImageRefByDS(HDICOMDS hDS, HDICOMDS hRefImageDS, L_INT32* FrameNumbers, L_UINT uFramesCount);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemovePresStateImageRefBySOPInstance(HDICOMDS hDS, L_TCHAR* pszSOPInstanceUID);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveAllPresStateImageRefs(HDICOMDS hDS);
   L_LTDIC_API L_TCHAR*       EXT_FUNCTION L_DicomGetPresStateImageRefSOPInstance(HDICOMDS hDS, pDICOMELEMENT pRefSeriesSQItem, L_UINT uImageIndex);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetPresStateImageRefCount(HDICOMDS hDS, pDICOMELEMENT pRefSeriesSQItem, L_UINT* pCount);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomFindFirstPresStateRefSeriesItem(HDICOMDS hDS);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomFindNextPresStateRefSeriesItem(HDICOMDS hDS, pDICOMELEMENT pRefSeriesItem);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomGetPresStateImageRefBySOPInstance(HDICOMDS hDS, L_TCHAR* pszSOPInstanceUID);      
   //---------------------------------------------------------------------------
   // Graphic Layer Module functions
   //---------------------------------------------------------------------------    
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomCreateLayer(HDICOMDS hDS, pDICOMGRAPHICLAYER pGraphicLayer, L_UINT* pLayerIndex);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetLayerInfo(HDICOMDS hDS, L_UINT uLayerIndex, pDICOMGRAPHICLAYER pGraphicLayer, L_UINT uStructSize);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomSetLayerInfo(HDICOMDS hDS, L_UINT uLayerIndex, pDICOMGRAPHICLAYER pGraphicLayer);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveLayerByIndex(HDICOMDS hDS, L_UINT uLayerIndex,L_BOOL bAnnSequence);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveLayerByName(HDICOMDS hDS, L_TCHAR* pszLayerName,L_BOOL bAnnSequence);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveAllLayers(HDICOMDS hDS, L_BOOL bAnnSequence);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetLayerCount(HDICOMDS hDS, L_UINT* pCount);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetLayerIndex(HDICOMDS hDS, L_TCHAR* pszLayerName, L_INT* pLayerIndex);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetLayerGraphicObjectCount(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveLayerGraphicObjects(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetLayerTextObjectCount(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveLayerTextObjects(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomGetLayerElementByIndex(HDICOMDS hDS, L_UINT uLayerIndex);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomGetLayerElementByName(HDICOMDS hDS, L_TCHAR* pszLayerName);
   //---------------------------------------------------------------------------
   // Graphic Annotation Module functions 
   //---------------------------------------------------------------------------          
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomFindFirstGraphicAnnSQItem(HDICOMDS hDS);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomFindNextGraphicAnnSQItem(HDICOMDS hDS, pDICOMELEMENT pRefSeriesItem);
   L_LTDIC_API L_TCHAR*       EXT_FUNCTION L_DicomGetLayerName(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomSetLayerName(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszLayerName);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomCreateGraphicAnnSQItem(HDICOMDS hDS, L_UINT32 nIndex, L_TCHAR* pszLayerName);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomAddLayerImageRef(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszImageSOPInstance);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetLayerImageRefCount(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_LTDIC_API L_TCHAR*       EXT_FUNCTION L_DicomGetLayerImageRefSOPInstance(HDICOMDS hDS,pDICOMELEMENT pGraphicAnnSQItem, L_UINT uImageIndex);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveImageRefFromLayer(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszImageSOPInstance);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveAllImageRefsFromLayer(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveAllImageRefFromAllLayers(HDICOMDS hDS);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomGetLayerImageRefElement(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_TCHAR* pszImageSOPInstance);
   //---------------------------------------------------------------------------
   // Graphic Annotation Objects functions
   //---------------------------------------------------------------------------
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomCreateGraphicObject(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, pDICOMGRAPHICOBJECT pGraphicObject, L_BOOL bCheckLayer);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveGraphicObject(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uGraphicObjIndex);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetGraphicObjectInfo(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uGraphicObjectIndex, pDICOMGRAPHICOBJECT pGraphicObject, L_UINT uStructSize);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomSetGraphicObjectInfo(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uGraphicObjectIndex, pDICOMGRAPHICOBJECT pGraphicObject);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetGraphicObjectCount(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT* nCount);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveAllGraphicObjects(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetGraphicObjPointCount(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uGraphicObjIndex, L_UINT* pPointsCount);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomGetGraphicObjElement(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uObjIndex);
   //---------------------------------------------------------------------------
   // Text Annotation Objects function
   //---------------------------------------------------------------------------
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomCreateTextObject(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, pDICOMTEXTOBJECT pTextObject, L_BOOL bCheckLayer);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveTextObject(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uTextObjIndex);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetTextObjectInfo(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uTextObjectIndex, pDICOMTEXTOBJECT pTextObject, L_UINT uStructSize);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomSetTextObjectInfo(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uTextObjectIndex, pDICOMTEXTOBJECT pTextObject);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomGetTextObjectCount(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT* pCount);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomRemoveAllTextObjects(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem);
   L_LTDIC_API pDICOMELEMENT  EXT_FUNCTION L_DicomGetTextObjElement(HDICOMDS hDS, pDICOMELEMENT pGraphicAnnSQItem, L_UINT uObjIndex);

   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomConvertLEADAnnObjToDicomAnnObjs(  HDICOMDS hDS,HANNOBJECT hAnnObject,pDICOMELEMENT pGraphicAnnSQItem,pCONVERTLEADANNOBJTODICOMANNPROC pConversionProc,L_VOID* pUserData,L_UINT uFlags);
   L_LTDIC_API L_UINT16       EXT_FUNCTION L_DicomConvertDicomAnnObjToLEADAnnObj(HDICOMDS hDS, pHANNOBJECT phAnnObject, pDICOMGRAPHICOBJECT pGraphicObject, pDICOMTEXTOBJECT pTextObject);

#if defined(LEADTOOLS_V16_OR_LATER)
   //---------------------------------------------------------------------------
   // Private Element methods
   //---------------------------------------------------------------------------
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomCreatePrivateCreatorDataElement     (HDICOMDS hDS, pDICOMELEMENT pElement, L_UINT16 uElementGroup, L_UINT16 uElementNumber, L_TCHAR *pszIdCode, pDICOMELEMENT *ppPrivateCreatorDataElement);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomGetNextUnusedPrivateTag             (HDICOMDS hDS, pDICOMELEMENT pPrivateCreatorDataElement, L_UINT32 *puTag);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindFirstPrivateCreatorDataElement  (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree, L_TCHAR *pszIdCode, L_UINT16 uElementGroup);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindNextPrivateCreatorDataElement   (HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bTree, L_TCHAR *pszIdCode, L_UINT16 uElementGroup);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindFirstPrivateElement             (HDICOMDS hDS, pDICOMELEMENT pPrivateCreatorDataElement);
   L_LTDIC_API pDICOMELEMENT   EXT_FUNCTION L_DicomFindNextPrivateElement              (HDICOMDS hDS, pDICOMELEMENT pElement, pDICOMELEMENT pPrivateCreatorDataElement);

   //---------------------------------------------------------------------------
   // Encapsulate Document Functions
   //---------------------------------------------------------------------------
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomGetEncapsulatedDocument(HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bChild, L_TCHAR *pszFileDocument, pDICOMENCAPSULATEDDOCUMENT pEncapsulatedDocument, pDICOMCODESEQUENCEITEM pConceptNameCodeSequence);
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomSetEncapsulatedDocument(HDICOMDS hDS, pDICOMELEMENT pElement, L_BOOL bChild, L_TCHAR *pszFileDocument, pDICOMENCAPSULATEDDOCUMENT pEncapsulatedDocument, pDICOMCODESEQUENCEITEM pConceptNameCodeSequence);

#endif

   //---------------------------------------------------------------------------
   // Annotation Functions
   //---------------------------------------------------------------------------
#if !defined(FOR_MANAGED)
   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomAnnSave           (HDICOMDS hDS,
                                                          HANNOBJECT      hAnnContainer,
                                                          L_UINT          uFormat,
                                                          L_BOOL          bSelected,
                                                          pSAVEFILEOPTION pSaveOption,
                                                          L_INT           nIndex,
                                                          L_UINT32       *pnPrivateCreatorTag
                                                          );

   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomAnnLoad           (HDICOMDS hDS,
                                                          pHANNOBJECT     phAnnContainer,
                                                          L_INT           nIndex,
                                                          pLOADFILEOPTION pLoadOptions
                                                          );


   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomAnnCount         (HDICOMDS hDS, L_INT *pFileIndices, L_UINT32 *pnPrivateCreatorTag);

   L_LTDIC_API L_UINT16        EXT_FUNCTION L_DicomAnnDelete        (HDICOMDS hDS, L_INT nIndex, L_INT nPage ); //nIndex: Pass -1 to delete the LEAD Private tag and all files
                                                                                                    //nPage:   Pass -1 to delete the entire file
                                                                                                    //         Pass >0 to delete the page
#endif
#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)

//============= CLASS ==========================================================
// Channel Status (003A,0205)
#define DICOM_CHANNEL_STATUS_OK             0x01
#define DICOM_CHANNEL_STATUS_TEST_DATA      0x02
#define DICOM_CHANNEL_STATUS_DISCONNECTED   0x04
#define DICOM_CHANNEL_STATUS_QUESTIONABLE   0x08
#define DICOM_CHANNEL_STATUS_INVALID        0x10
#define DICOM_CHANNEL_STATUS_UNCALIBRATED   0x20
#define DICOM_CHANNEL_STATUS_UNZEROED       0x40

// Waveform Sample Interpretation (5400,1006)
#define DICOM_SAMPLE_INTERPRETATION_SS   0
#define DICOM_SAMPLE_INTERPRETATION_US   1
#define DICOM_SAMPLE_INTERPRETATION_SB   2
#define DICOM_SAMPLE_INTERPRETATION_UB   3
#define DICOM_SAMPLE_INTERPRETATION_MB   4
#define DICOM_SAMPLE_INTERPRETATION_AB   5

// Waveform Originality (003A,0004)
#define DICOM_WAVEFORM_ORIGINALITY_ORIGINAL   0
#define DICOM_WAVEFORM_ORIGINALITY_DERIVED    1

// Temporal Range Type (0040,A130)
#define DICOM_TEMPORAL_RANGE_TYPE_UNDEFINED        0
#define DICOM_TEMPORAL_RANGE_TYPE_POINT            1
#define DICOM_TEMPORAL_RANGE_TYPE_MULTIPOINT       2
#define DICOM_TEMPORAL_RANGE_TYPE_SEGMENT          3
#define DICOM_TEMPORAL_RANGE_TYPE_MULTISEGMENT     4
#define DICOM_TEMPORAL_RANGE_TYPE_BEGIN            5
#define DICOM_TEMPORAL_RANGE_TYPE_END              6



// Waveform Annotation
typedef struct tagDICOMWAVEFORMANNOTATION
{
   L_UINT uStructSize;
   
   // Annotation Value
   L_TCHAR*               pszUnformattedTextValue;
   pDICOMCODESEQUENCEITEM pCodedName;
   pDICOMCODESEQUENCEITEM pCodedValue;
   L_DOUBLE*              pNumericValue;
   L_UINT32               uNumericValueCount;
   pDICOMCODESEQUENCEITEM pMeasurementUnits;
   
   // Annotation Temporal Range
   L_UINT16       uTemporalRangeType;
   L_UINT32*      pRefSamplePositions;
   L_DOUBLE*      pRefTimeOffsets;
   pVALUEDATETIME pRefDatetime;
   L_UINT32       uTemporalPointCount;
   
   L_UINT16*      puAnnGroupNumber;
   
} DICOMWAVEFORMANNOTATION, * pDICOMWAVEFORMANNOTATION;


#if defined(__cplusplus)
#if !defined(FOR_WINCE)
class L_LTDIC_CLASS LDicomWaveformGroup;
#endif // #if !defined(FOR_WINCE)

class L_LTDIC_CLASS LDicomWaveformChannel
{
   public:
   LDicomWaveformChannel();
   virtual ~LDicomWaveformChannel();
   
   LDicomWaveformGroup* GetWaveformGroup();
   L_UINT32 GetIndex();
   
   L_UINT32 SetChannelSamples8(L_UCHAR* pSamples, L_UINT32 uCount);
   L_UINT32 SetChannelSamples16(L_INT16* pSamples, L_UINT32 uCount);
   L_UINT32 SetChannelSamples32(L_INT32* pSamples, L_UINT32 uCount);
   L_INT32* GetChannelSamples(L_UINT32* puNumberOfSamples);
   
   L_UINT16 SetChannelSource(pDICOMCODESEQUENCEITEM pChannelSource);
   pDICOMCODESEQUENCEITEM GetChannelSource();
   L_UINT16 SetChannelSensitivity(L_BOOL bInclude,
                                  L_DOUBLE dChannelSensitivity,
                                  pDICOMCODESEQUENCEITEM pChannelSensitivityUnits,
                                  L_DOUBLE dChannelSensitivityCF = 1.0,
                                  L_DOUBLE dChannelBaseline = 0.0);
   L_UINT16 GetChannelSensitivity(L_BOOL* pbIncluded,
                                  L_DOUBLE* pdChannelSensitivity,
                                  pDICOMCODESEQUENCEITEM pChannelSensitivityUnits,
                                  L_UINT uStructSize,
                                  L_DOUBLE* pdChannelSensitivityCF,
                                  L_DOUBLE* pdChannelBaseline);
   L_VOID   SetChannelStatus(L_UINT16 uStatus);
   L_UINT16 GetChannelStatus();
   L_BOOL   SetChannelTimeSkew(L_DOUBLE dTimeSkew);
   L_BOOL   GetChannelTimeSkew(L_DOUBLE* pdTimeSkew);
   L_BOOL   SetChannelSampleSkew(L_DOUBLE dSampleSkew);
   L_BOOL   GetChannelSampleSkew(L_DOUBLE* pdSampleSkew);
   
   L_BOOL   SetWaveformChannelNumber(L_BOOL bInclude, L_INT32 nChannelNumber);
   L_BOOL   GetWaveformChannelNumber(L_INT32* pnChannelNumber);
   L_BOOL   SetChannelLabel(L_TCHAR* pszLabel);
   L_TCHAR* GetChannelLabel();
   L_BOOL   SetChannelOffset(L_BOOL bInclude, L_DOUBLE dChannelOffset);
   L_BOOL   GetChannelOffset(L_DOUBLE* pdChannelOffset);
   L_BOOL   SetFilterLowFrequency(L_BOOL bInclude, L_DOUBLE dLowFrequency);
   L_BOOL   GetFilterLowFrequency(L_DOUBLE* pdLowFrequency);
   L_BOOL   SetFilterHighFrequency(L_BOOL bInclude, L_DOUBLE dHighFrequency);
   L_BOOL   GetFilterHighFrequency(L_DOUBLE* pdHighFrequency);
   L_BOOL   SetNotchFilterFrequency(L_BOOL bInclude, L_DOUBLE dFrequency);
   L_BOOL   GetNotchFilterFrequency(L_DOUBLE* pdFrequency);
   L_BOOL   SetNotchFilterBandwidth(L_BOOL bInclude, L_DOUBLE dBandwidth);
   L_BOOL   GetNotchFilterBandwidth(L_DOUBLE* pdBandwidth);
   L_BOOL   SetChannelMinimumValue(L_BOOL bInclude, L_INT32 nMinValue);
   L_BOOL   GetChannelMinimumValue(L_INT32* pnMinValue);
   L_BOOL   SetChannelMaximumValue(L_BOOL bInclude, L_INT32 nMaxValue);
   L_BOOL   GetChannelMaximumValue(L_INT32* pnMaxValue);
   
   // Annotations
   L_UINT32 GetAnnotationCount();
   pDICOMWAVEFORMANNOTATION GetAnnotation(L_UINT32 uIndex);
   L_UINT16 AddAnnotation(pDICOMWAVEFORMANNOTATION pAnnotation);
   L_UINT32 DeleteAnnotation(L_UINT32 uIndex);
   
private:
   class CWaveformAnnotation
   {
   public:
      CWaveformAnnotation()
      {
         m_pItem = NULL;
         m_pNextAnnotation = NULL;

         m_Annotation.uStructSize = sizeof(DICOMWAVEFORMANNOTATION);
         m_Annotation.pszUnformattedTextValue = NULL;
         m_Annotation.pCodedName = NULL;
         m_Annotation.pCodedValue = NULL;
         m_Annotation.pNumericValue = NULL;
         m_Annotation.uNumericValueCount = 0;
         m_Annotation.pMeasurementUnits = NULL;
         m_Annotation.uTemporalRangeType = 0;
         m_Annotation.pRefSamplePositions = NULL;
         m_Annotation.pRefTimeOffsets = NULL;
         m_Annotation.pRefDatetime = NULL;
         m_Annotation.uTemporalPointCount = 0;
         m_Annotation.puAnnGroupNumber = NULL;
      }
      
      DICOMWAVEFORMANNOTATION m_Annotation;
      pDICOMELEMENT           m_pItem;
      CWaveformAnnotation*    m_pNextAnnotation;
   };
   
   LDicomWaveformGroup*  m_pParentGroup;
   L_INT32*              m_piChannelSamples;
   L_UINT32              m_uChannelIndex;
   L_UINT16              m_uChannelStatus;
   DICOMCODESEQUENCEITEM m_ChannelSource;
   LDicomDS              m_ChannelDS;
   CWaveformAnnotation*  m_pAnnotations;
   
   L_VOID FreeChannelSource();
   L_BOOL SetCodeSequenceItem(pDICOMELEMENT pAnnotationItem,
                              L_UINT32 uCodeSequenceTag,
                              pDICOMCODESEQUENCEITEM pDstItem,
                              pDICOMCODESEQUENCEITEM pSrcItem);
   
   friend class LDicomWaveformGroup;
};
#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)
   
   L_LTDIC_API HDICOMWAVEFORMCHANNEL       EXT_FUNCTION L_DicomChannelCreate();
   L_LTDIC_API L_VOID                      EXT_FUNCTION L_DicomChannelFree(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel);
   L_LTDIC_API HDICOMWAVEFORMGROUP         EXT_FUNCTION L_DicomChannelGetWaveformGroup (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel);
   L_LTDIC_API L_UINT32                    EXT_FUNCTION L_DicomChannelGetIndex (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel);
   L_LTDIC_API L_UINT32                    EXT_FUNCTION L_DicomChannelSetSamples8(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_UCHAR* pSamples, L_UINT32 uCount);
   L_LTDIC_API L_UINT32                    EXT_FUNCTION L_DicomChannelSetSamples16(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_INT16* pSamples, L_UINT32 uCount);
   L_LTDIC_API L_UINT32                    EXT_FUNCTION L_DicomChannelSetSamples32(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_INT32* pSamples, L_UINT32 uCount);
   L_LTDIC_API L_INT32*                    EXT_FUNCTION L_DicomChannelGetSamples(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_UINT32* puNumberOfSamples);
   L_LTDIC_API L_UINT16                    EXT_FUNCTION L_DicomChannelSetSource (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,pDICOMCODESEQUENCEITEM pChannelSource);
   L_LTDIC_API pDICOMCODESEQUENCEITEM      EXT_FUNCTION L_DicomChannelGetSource (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel);
   L_LTDIC_API L_UINT16                    EXT_FUNCTION L_DicomChannelSetSensitivity (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude,L_DOUBLE dChannelSensitivity,pDICOMCODESEQUENCEITEM pChannelSensitivityUnits,L_DOUBLE dChannelSensitivityCF ,L_DOUBLE dChannelBaseline);
   L_LTDIC_API L_UINT16                    EXT_FUNCTION L_DicomChannelGetSensitivity (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL* pbIncluded,L_DOUBLE* pdChannelSensitivity,pDICOMCODESEQUENCEITEM pChannelSensitivityUnits,L_UINT uStructSize,L_DOUBLE* pdChannelSensitivityCF,L_DOUBLE* pdChannelBaseline);
   L_LTDIC_API L_VOID                      EXT_FUNCTION L_DicomChannelSetStatus (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_UINT16 uStatus);
   L_LTDIC_API L_UINT16                    EXT_FUNCTION L_DicomChannelGetStatus (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetTimeSkew(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE dTimeSkew);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetTimeSkew(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE* pdTimeSkew);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetSampleSkew(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE dSampleSkew);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetSampleSkew(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE* pdSampleSkew);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetNumber (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude, L_INT32 nChannelNumber);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetNumber (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_INT32* pnChannelNumber);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetLabel(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_TCHAR* pszLabel);
   L_LTDIC_API L_TCHAR*                    EXT_FUNCTION L_DicomChannelGetLabel(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetOffset (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude, L_DOUBLE dChannelOffset);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetOffset (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE* pdChannelOffset);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetFilterLowFrequency (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude, L_DOUBLE dLowFrequency);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetFilterLowFrequency (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE* pdLowFrequency);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetFilterHighFrequency(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude, L_DOUBLE dHighFrequency);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetFilterHighFrequency(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE* pdHighFrequency);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetNotchFilterFrequency(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude, L_DOUBLE dFrequency);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetNotchFilterFrequency(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE* pdFrequency);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetNotchFilterBandwidth(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude, L_DOUBLE dBandwidth);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetNotchFilterBandwidth(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_DOUBLE* pdBandwidth);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetMinimumValue(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude, L_INT32 nMinValue);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetMinimumValue(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_INT32* pnMinValue);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelSetMaximumValue(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_BOOL bInclude, L_INT32 nMaxValue);
   L_LTDIC_API L_BOOL                      EXT_FUNCTION L_DicomChannelGetMaximumValue(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_INT32* pnMaxValue);
   L_LTDIC_API L_UINT32                    EXT_FUNCTION L_DicomChannelGetAnnotationCount(HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel);
   L_LTDIC_API pDICOMWAVEFORMANNOTATION    EXT_FUNCTION L_DicomChannelGetAnnotation (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_UINT32 uIndex);
   L_LTDIC_API L_UINT16                    EXT_FUNCTION L_DicomChannelAddAnnotation (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,pDICOMWAVEFORMANNOTATION pAnnotation);
   L_LTDIC_API L_UINT32                    EXT_FUNCTION L_DicomChannelDeleteAnnotation (HDICOMWAVEFORMCHANNEL hDICOMWaveFormChannel,L_UINT32 uIndex);


#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)


//============= CLASS ==========================================================
#if defined(__cplusplus)
class L_LTDIC_CLASS LDicomWaveformGroup
{
   public:
   LDicomWaveformGroup();
   virtual ~LDicomWaveformGroup();
   
   L_VOID   Reset();
   
   L_UINT32 GetNumberOfChannels();
   LDicomWaveformChannel* GetChannel(L_UINT32 uIndex);
   LDicomWaveformChannel* AddChannel(L_UINT32 uIndex = ELEMENT_INDEX_MAX);
   L_UINT32 DeleteChannel(L_UINT32 uIndex);
   L_BOOL   SetNumberOfSamplesPerChannel(L_UINT32 uNumberOfSamples);
   L_UINT32 GetNumberOfSamplesPerChannel();
   L_VOID   SetSamplingFrequency(L_DOUBLE dFrequency);
   L_DOUBLE GetSamplingFrequency();
   L_BOOL   SetSampleInterpretation(L_UINT16 uInterpretation);
   L_UINT16 GetSampleInterpretation();
   
   L_BOOL   SetMultiplexGroupTimeOffset(L_BOOL bInclude, L_DOUBLE dOffset);
   L_BOOL   GetMultiplexGroupTimeOffset(L_DOUBLE* pdOffset);
   L_BOOL   SetTriggerTimeOffset(L_BOOL bInclude, L_DOUBLE dOffset);
   L_BOOL   GetTriggerTimeOffset(L_DOUBLE* pdOffset);
   L_BOOL   SetTriggerSamplePosition(L_BOOL bInclude, L_UINT32 uSamplePosition);
   L_BOOL   GetTriggerSamplePosition(L_UINT32* puSamplePosition);
   L_VOID   SetWaveformOriginality(L_UINT16 uOriginality);
   L_UINT16 GetWaveformOriginality();
   L_BOOL   SetMultiplexGroupLabel(L_TCHAR* pszLabel);
   L_TCHAR* GetMultiplexGroupLabel();
   L_BOOL   SetWaveformPaddingValue(L_BOOL bInclude, L_INT32 nPaddingValue);
   L_BOOL   GetWaveformPaddingValue(L_INT32* pnPaddingValue);
   
   L_UINT16 LoadAudio(L_TCHAR* pszFilename, L_UINT16 uFlags = 0);
   L_UINT16 SaveAudio(L_TCHAR* pszFilename, L_UINT16 uFlags = 0);
   
private:
   struct CHANNEL
   {
      LDicomWaveformChannel m_Channel;
      CHANNEL*              m_pNextChannel;
   };
   
   struct WAVEDATA
   {
      L_CHAR*  pData;
      L_UINT32 uDataLength;
   };
   
   CHANNEL* m_pChannels;
   LDicomDS m_GroupDS;
   L_UINT32 m_uSamplesPerChannelCount;
   L_DOUBLE m_dSamplingFrequency;
   L_UINT16 m_uInterpretation;
   L_UINT16 m_uOriginality;
   
   L_BOOL   IsChangeAllowed(L_UINT16 uNewInterpretation, L_UINT* puConversionType);
   L_VOID   UpdateIndexes();
   L_UINT16 ConvertWaveData(L_UINT16 nConversionType, L_VOID* pOldData, L_CHAR* pNewData, L_UINT32 uDataLength, L_BOOL b16BitData = TRUE);
   L_UCHAR  Linear2Alaw(L_INT16 iVal);
   L_UCHAR  Linear2Ulaw(L_INT16 iVal);
   L_INT16  Alaw2Linear(L_UCHAR byVal);
   L_INT16  Ulaw2Linear(L_UCHAR byVal);
   L_UCHAR  Alaw2Ulaw(L_UCHAR byVal);
   L_UCHAR  Ulaw2Alaw(L_UCHAR byVal);
};
#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)
   L_LTDIC_API HDICOMWAVEFORMGROUP     EXT_FUNCTION L_DicomWaveGrpCreate();
   L_LTDIC_API L_VOID                  EXT_FUNCTION L_DicomWaveGrpFree(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API L_VOID                  EXT_FUNCTION L_DicomWaveGrpReset(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API L_UINT32                EXT_FUNCTION L_DicomWaveGrpGetNumberOfChannels(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API HDICOMWAVEFORMCHANNEL   EXT_FUNCTION L_DicomWaveGrpGetChannel  (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_UINT32 uIndex);
   L_LTDIC_API HDICOMWAVEFORMCHANNEL   EXT_FUNCTION L_DicomWaveGrpAddChannel  (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_UINT32 uIndex);
   L_LTDIC_API L_UINT32                EXT_FUNCTION L_DicomWaveGrpDeleteChannel(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_UINT32 uIndex);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpSetNumberOfSamplesPerChannel(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_UINT32 uNumberOfSamples);
   L_LTDIC_API L_UINT32                EXT_FUNCTION L_DicomWaveGrpGetNumberOfSamplesPerChannel(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API L_VOID                  EXT_FUNCTION L_DicomWaveGrpSetSamplingFrequency   (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_DOUBLE dFrequency);
   L_LTDIC_API L_DOUBLE                EXT_FUNCTION L_DicomWaveGrpGetSamplingFrequency   (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpSetSampleInterpretation(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_UINT16 uInterpretation);
   L_LTDIC_API L_UINT16                EXT_FUNCTION L_DicomWaveGrpGetSampleInterpretation(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpSetMultiplexGroupTimeOffset (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_BOOL bInclude, L_DOUBLE dOffset);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpGetMultiplexGroupTimeOffset (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_DOUBLE* pdOffset);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpSetTriggerTimeOffset   (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_BOOL bInclude, L_DOUBLE dOffset);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpGetTriggerTimeOffset   (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_DOUBLE* pdOffset);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpSetTriggerSamplePosition    (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_BOOL bInclude, L_UINT32 uSamplePosition);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpGetTriggerSamplePosition    (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_UINT32* puSamplePosition);
   L_LTDIC_API L_VOID                  EXT_FUNCTION L_DicomWaveGrpSetOriginality (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_UINT16 uOriginality);
   L_LTDIC_API L_UINT16                EXT_FUNCTION L_DicomWaveGrpGetOriginality (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpSetMultiplexGroupLabel (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_TCHAR* pszLabel);
   L_LTDIC_API L_TCHAR*                EXT_FUNCTION L_DicomWaveGrpGetMultiplexGroupLabel (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpSetPaddingValue(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_BOOL bInclude, L_INT32 nPaddingValue);
   L_LTDIC_API L_BOOL                  EXT_FUNCTION L_DicomWaveGrpGetPaddingValue(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_INT32* pnPaddingValue);
   L_LTDIC_API L_UINT16                EXT_FUNCTION L_DicomWaveGrpLoadAudio(HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_TCHAR* pszFilename, L_UINT16 uFlags);
   L_LTDIC_API L_UINT16                EXT_FUNCTION L_DicomWaveGrpSaveAudio (HDICOMWAVEFORMGROUP hDICOMWaveFormGroup,L_TCHAR* pszFilename, L_UINT16 uFlags);

#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)


//============= CLASS ==========================================================
// Directory options structure
typedef struct tagDICOMDIROPTIONS
{
   L_INT    nSize;
   L_BOOL   bIncludeSubfolders;
   L_UINT32 uFlags;

} DICOMDIROPTIONS, * pDICOMDIROPTIONS;

// Flags of the Directory options structure
#define DICOMDIR_REJECT_INVALID_FILEID          0x01
#define DICOMDIR_INSERT_ICON_IMAGE_SEQUENCE     0x02

// A status code
#define DICOMDIR_INSERTDICOMFILE_PREADD   300
#define DICOMDIR_INSERTDICOMFILE_FAILURE  350


#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomDir : public LDicomDS
{
public:

   //---------------------------------------------------------------------------
   // Initializing Functions
   //---------------------------------------------------------------------------
   LDicomDir(L_TCHAR* pszDICOMDIRDstFolder = NULL, L_TCHAR* pszPath = NULL);
   virtual ~LDicomDir();

   //---------------------------------------------------------------------------
   // Input and Output Functions
   //---------------------------------------------------------------------------
   L_UINT16 LoadDS(L_TCHAR* pszName, L_UINT16 nFlags);
   L_UINT16 SaveDicomDir(L_UINT16 uFlags = 0);   
   //---------------------------------------------------------------------------
   // Deletion Functions
   //---------------------------------------------------------------------------
   L_UINT16 ResetDicomDir(const L_TCHAR* pszDICOMDIRDstFolder);

   //---------------------------------------------------------------------------
   // Insertion Functions
   //---------------------------------------------------------------------------
   L_UINT16 InsertDicomFile(L_TCHAR* pszFileName);
   L_UINT16 InsertDicomDS(LDicomDS& DataSet, L_TCHAR* pszFileName);

   //---------------------------------------------------------------------------
   // Modification Functions
   //---------------------------------------------------------------------------
   L_UINT16 SetFileSetID(L_TCHAR* pszFileSetID);
   L_UINT16 SetDescriptorFile(L_TCHAR* pszFileName, L_TCHAR* pszCharSet);
   L_UINT16 SetOptions(const pDICOMDIROPTIONS pOptions);

   //---------------------------------------------------------------------------
   // Retrieval Functions
   //---------------------------------------------------------------------------
   L_UINT16 GetOptions(pDICOMDIROPTIONS pOptions, L_INT nSize) const;

   //---------------------------------------------------------------------------
   // Overridables
   //---------------------------------------------------------------------------   
   virtual L_UINT16 OnInsertDicomFile(const L_TCHAR* pszFileName,
                                      LDicomDS* pDataSet,
                                      L_UINT16 uStatus);

   //---------------------------------------------------------------------------
   // Private
   //---------------------------------------------------------------------------
private:
   L_TCHAR*        m_pszDICOMDIRDstFolder;
   DICOMDIROPTIONS m_Options;
   L_BOOL          m_bEnableSearchFolder;

   pDICOMELEMENT FindDirectoryRecord(pDICOMELEMENT pReferencingDR, L_INT nDRType,
                                     const L_TCHAR* pszIdentifier);
   L_VOID        SetSelectionKey(pDICOMELEMENT pDR, LDicomDS& DataSet, L_UINT32 uTag,
                                 L_UINT16 uVR, L_BOOL bEnableDelete = FALSE);
   L_VOID        DeleteDRElement(pDICOMELEMENT pDR, L_UINT32 uTag);
   L_UINT16      GetRelativePath(const L_TCHAR* pszFileName, L_TCHAR** ppszFName) const;
   L_BOOL        CheckFileID(const L_TCHAR* pszFileID) const;
   L_VOID        ResetOptions();
   L_UINT16      SearchFolder(const L_TCHAR* pszFolderName, L_BOOL bIncludeSubfolders);
   L_UINT16      InsertIconImageSequence(pDICOMELEMENT pImageKey, LDicomDS& SourceDataSet);
   L_BOOL        IsDataSetAlreadyPresent(LDicomDS &ds);

};

#endif // #if defined(__cplusplus)
//============= CLASS ==========================================================

#define PDU_UNKNOWN                     0x00

#define PDU_ASSOCIATE_REQUEST           0x01
#define PDU_ASSOCIATE_ACCEPT            0x02
#define PDU_ASSOCIATE_REJECT            0x03
#define PDU_DATA_TRANSFER               0x04
#define PDU_RELEASE_REQUEST             0x05
#define PDU_RELEASE_RESPONSE            0x06
#define PDU_ABORT                       0x07

#define PDU_ROLE_NON_SUPPORT               0
#define PDU_ROLE_SUPPORT                   1

#define PDU_MAX_UID_SIZE                  64
#define PDU_MAX_TITLE_SIZE                64
#define PDU_MAX_VERSION_SIZE              16
#define PDU_MIN_MAXIMUM_LENGTH             8

// Associate Accept
#define PDU_ACCEPT_RESULT_SUCCESS           0  // Acceptance
#define PDU_ACCEPT_RESULT_USER_REJECT       1  // User rejection
#define PDU_ACCEPT_RESULT_PROVIDER_REJECT   2  // No reason (provider rejection)
#define PDU_ACCEPT_RESULT_ABSTRACT_SYNTAX   3  // Abstract syntax not supported (provider rejection)
#define PDU_ACCEPT_RESULT_TRANSFER_SYNTAX   4  // Transfer syntaxes not supported (provider rejection)

// Associate Reject
#define PDU_REJECT_RESULT_PERMANENT         1  // Rejected permanent
#define PDU_REJECT_RESULT_TRANSIENT         2  // Rejected transient

#define PDU_REJECT_SOURCE_USER              1  // DICOM UL service-user
#define PDU_REJECT_SOURCE_PROVIDER1         2  // DICOM UL service-provider (ASCE related function)
#define PDU_REJECT_SOURCE_PROVIDER2         3  // DICOM UL service-provider (Presentation related function)

#define PDU_REJECT_REASON_UNKNOWN           1  // No reason given (User - Provider1)
#define PDU_REJECT_REASON_APPLICATION       2  // Application context name not supported (User)
#define PDU_REJECT_REASON_CALLING           3  // Calling AE Title not recognized (User)
#define PDU_REJECT_REASON_CALLED            7  // Called AE Title not recognized (User)

#define PDU_REJECT_REASON_VERSION           2  // Protocol version not supported (Provider1)

#define PDU_REJECT_REASON_CONGESTION        1  // Temporary congestion (Provider2)
#define PDU_REJECT_REASON_LIMIT             2  // Local limit exceeded (Provider2)

// Abort
#define PDU_ABORT_SOURCE_USER               0  // Service user
#define PDU_ABORT_SOURCE_PROVIDER           2  // Service provider

#define PDU_ABORT_REASON_UNKNOWN            0  // Reason no specified (Provider)
#define PDU_ABORT_REASON_UNRECOGNIZED       1  // Unrecognized PDU (Provider)
#define PDU_ABORT_REASON_UNEXPECTED         2  // Unexpected PDU (Provider)
#define PDU_ABORT_REASON_UNRECOGNIZED_PARAM 4  // Unrecognized PDU parameter (Provider)
#define PDU_ABORT_REASON_UNEXPECTED_PARAM   5  // Unexpected PDU parameter (Provider)
#define PDU_ABORT_REASON_INVALID_PARAM      6  // Invalid PDU parameter value (Provider)

//============= CLASS ==========================================================
#if !defined(FOR_WINCE)

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomAssociate
{
public:
   LDicomAssociate(L_BOOL bRequest);
   ~LDicomAssociate();

   L_VOID    Reset                  (L_BOOL bRequest);
   L_VOID    Default                ();

   L_BOOL    IsRequest              ();
   L_VOID    SetRequest             (L_BOOL bRequest);

   L_UINT16  GetVersion             ();
   L_INT     SetVersion             (L_UINT16 nVersion);

   L_VOID    GetCalled              (L_TCHAR *strCalled, L_UINT32 SizeInWords);
   L_INT     SetCalled              (L_TCHAR *pszName);

   L_VOID    GetCalling             (L_TCHAR *strCalling, L_UINT32 SizeInWords);
   L_INT     SetCalling             (L_TCHAR *pszName);

   L_VOID    GetApplication         (L_TCHAR *strApplication, L_UINT32 SizeInWords);
   L_INT     SetApplication         (L_TCHAR *pszUID);

   L_INT     GetPresentationCount   ();
   L_UCHAR   GetPresentation        (L_INT nIndex);
   L_INT     SetPresentation        (L_INT nIndex, L_UCHAR nID);
   L_INT     AddPresentation        (L_UCHAR nID, L_UCHAR nResult, L_TCHAR *pszAbstract);
   L_VOID    DelPresentation        (L_UCHAR nID);

   L_UCHAR   GetResult              (L_UCHAR nID);
   L_INT     SetResult              (L_UCHAR nID, L_UCHAR nResult);

   L_VOID    GetAbstract            (L_UCHAR nID, L_TCHAR *strAbstract, L_UINT32 SizeInWords);
   L_INT     SetAbstract            (L_UCHAR nID, L_TCHAR *pszUID);
   L_UCHAR   FindAbstract           (L_TCHAR *pszUID);
   L_UCHAR   FindNextAbstract       (L_UCHAR nID, L_TCHAR *pszUID);
   L_INT     GetAbstractCount       (L_TCHAR *pszUID);



   L_INT     GetTransferCount       (L_UCHAR nID);
   L_VOID    GetTransfer            (L_UCHAR nID, L_INT nIndex, L_TCHAR *strTransfer, L_UINT32 SizeInWords);
   L_INT     SetTransfer            (L_UCHAR nID, L_INT nIndex, L_TCHAR *pszUID);
   L_INT     AddTransfer            (L_UCHAR nID, L_TCHAR *pszUID);
   L_VOID    DelTransfer            (L_UCHAR nID, L_INT nIndex);

   L_BOOL    IsRoleSelect           (L_UCHAR nID);
   L_UCHAR   GetUserRole            (L_UCHAR nID);
   L_UCHAR   GetProviderRole        (L_UCHAR nID);
   L_INT     SetRoleSelect          (L_UCHAR nID, L_BOOL bEnabled, L_UCHAR nUser, L_UCHAR nProvider);

   L_UINT32  GetLengthExtended      (L_UCHAR nID);
   L_UCHAR  *GetExtended            (L_UCHAR nID);
   L_INT     SetExtended            (L_UCHAR nID, L_UCHAR *pData, L_UINT32 nLength);

   L_BOOL    IsMaxLength            ();
   L_UINT32  GetMaxLength           ();
   L_INT     SetMaxLength           (L_BOOL bEnabled, L_UINT32 nLength);

   L_BOOL    IsImplementClass       ();
   L_VOID    GetImplementClass      (L_TCHAR *strImplementClass, L_UINT32 SizeInWords);
   L_INT     SetImplementClass      (L_BOOL bEnabled, L_TCHAR *pszUID);

   L_BOOL    IsAsyncOperations      ();
   L_UINT16  GetInvokedOperations   ();
   L_UINT16  GetPerformedOperations ();
   L_INT     SetAsyncOperations     (L_BOOL bEnabled, L_UINT16 nInvoked, L_UINT16 nPerformed);

   L_BOOL    IsImplementVersion     ();
   L_VOID    GetImplementVersion    (L_TCHAR *strImplementVersion, L_UINT32 SizeInWords);
   L_INT     SetImplementVersion    (L_BOOL bEnabled, L_TCHAR *pszVersion);

   L_INT     GetUserInfoCount       ();
   L_UCHAR   GetTypeUserInfo        (L_INT nIndex);
   L_UINT16  GetLengthUserInfo      (L_INT nIndex);
   L_UCHAR   *GetDataUserInfo       (L_INT nIndex);
   L_INT     SetUserInfo            (L_INT nIndex, L_UCHAR nType, L_UCHAR *pData, L_UINT16 nLength);
   L_INT     AddUserInfo            (L_UCHAR nType, L_UCHAR *pData, L_UINT16 nLength);
   L_VOID    DelUserInfo            (L_INT nIndex);

   //Internal Use only
   L_CHAR   *GetAbstractA            (L_UCHAR nID);
   L_INT     SetAbstractA            (L_UCHAR nID, L_CHAR *pszUID);
   L_UCHAR   FindAbstractA           (L_CHAR *pszUID);
   L_UCHAR   FindNextAbstractA       (L_UCHAR nID, L_CHAR *pszUID);
   L_INT     GetAbstractCountA       (L_CHAR *pszUID);
   L_INT     SetTransferA            (L_UCHAR nID, L_INT nIndex, L_CHAR *pszUID);
   L_CHAR   *GetImplementClassA      ();
   L_INT     SetImplementClassA      (L_BOOL bEnabled, L_CHAR *pszUID);
   L_CHAR   *GetImplementVersionA    ();
   L_INT     SetImplementVersionA    (L_BOOL bEnabled, L_CHAR *pszVersion);
private:
   friend class LDicomNet;

   L_BOOL     m_bRequest;
   L_UINT16   m_nVersion;
   L_CHAR     m_szCalled[PDU_MAX_TITLE_SIZE+1];
   L_CHAR     m_szCalling[PDU_MAX_TITLE_SIZE+1];
   L_CHAR     m_szApplication[PDU_MAX_UID_SIZE+1];

   L_BOOL     m_bMaxLength;
   L_UINT32   m_nMaxLength;

   L_BOOL     m_bImplementClass;
   L_CHAR     m_szImplementClass[PDU_MAX_UID_SIZE+1];

   L_BOOL     m_bAsyncOperations;
   L_UINT16   m_nInvokedOperations;
   L_UINT16   m_nPerformedOperations;

   L_BOOL     m_bImplementVersion;
   L_CHAR     m_szImplementVersion[PDU_MAX_VERSION_SIZE+1];

   LDicomTree m_Presentation;
   LDicomTree m_RoleSelect;
   LDicomTree m_UserInfo;

   L_INT     GetBinary        (L_CHAR *pBuffer, L_UINT32 *pnLength);
   L_INT     SetBinary        (L_CHAR *pBuffer, L_UINT32 *pnLength, LDicomAssociate *pDicomAssociate);
   L_VOID   *FindPresentation (L_UCHAR nID);
   L_VOID    DelSpaces        (L_CHAR *pszText, L_UINT32 nLength);

   L_INT     AddTransferA      (L_UCHAR nID, L_CHAR *pszUID);
   L_INT     AddPresentationA  (L_UCHAR nID, L_UCHAR nResult, L_CHAR *pszAbstract);
   L_CHAR   *GetTransferA      (L_UCHAR nID, L_INT nIndex);
};

#endif // #if defined(__cplusplus)

typedef L_VOID *HDICOMPDU;

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)

L_LTDIC_API HDICOMPDU EXT_FUNCTION L_DicomCreateAssociate        (L_BOOL bRequest);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomFreeAssociate          (HDICOMPDU hPDU);

L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomResetAssociate         (HDICOMPDU hPDU, L_BOOL bRequest);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomDefaultAssociate       (HDICOMPDU hPDU);

L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsRequest              (HDICOMPDU hPDU);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomSetRequest             (HDICOMPDU hPDU, L_BOOL bRequest);

L_LTDIC_API L_UINT16  EXT_FUNCTION L_DicomGetVersion             (HDICOMPDU hPDU);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetVersion             (HDICOMPDU hPDU, L_UINT16 nVersion);

L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomGetCalled              (HDICOMPDU hPDU,L_TCHAR *strCalled, L_UINT32 SizeInWords);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetCalled              (HDICOMPDU hPDU, L_TCHAR *pszName);

L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomGetCalling             (HDICOMPDU hPDU,L_TCHAR *strCalling, L_UINT32 SizeInWords);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetCalling             (HDICOMPDU hPDU, L_TCHAR *pszName);

L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomGetApplication         (HDICOMPDU hPDU,L_TCHAR *strApplication, L_UINT32 SizeInWords);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetApplication         (HDICOMPDU hPDU, L_TCHAR *pszUID);

L_LTDIC_API L_INT     EXT_FUNCTION L_DicomGetPresentationCount   (HDICOMPDU hPDU);
L_LTDIC_API L_UCHAR   EXT_FUNCTION L_DicomGetPresentation        (HDICOMPDU hPDU, L_INT nIndex);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetPresentation        (HDICOMPDU hPDU, L_INT nIndex, L_UCHAR nID);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomAddPresentation        (HDICOMPDU hPDU, L_UCHAR nID, L_UCHAR nResult, L_TCHAR *pszAbstract);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomDelPresentation        (HDICOMPDU hPDU, L_UCHAR nID);

L_LTDIC_API L_UCHAR   EXT_FUNCTION L_DicomGetResult              (HDICOMPDU hPDU, L_UCHAR nID);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetResult              (HDICOMPDU hPDU, L_UCHAR nID, L_UCHAR nResult);

L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomGetAbstract            (HDICOMPDU hPDU, L_UCHAR nID, L_TCHAR *strAbstract, L_UINT32 SizeInWords);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetAbstract            (HDICOMPDU hPDU, L_UCHAR nID, L_TCHAR *pszUID);
L_LTDIC_API L_UCHAR   EXT_FUNCTION L_DicomFindAbstract           (HDICOMPDU hPDU, L_TCHAR *pszUID);

L_LTDIC_API L_UCHAR   EXT_FUNCTION L_DicomFindNextAbstract       (HDICOMPDU hPDU, L_UCHAR nID, L_TCHAR *pszUID);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomGetAbstractCount       (HDICOMPDU hPDU, L_TCHAR *pszUID);


L_LTDIC_API L_INT     EXT_FUNCTION L_DicomGetTransferCount       (HDICOMPDU hPDU, L_UCHAR nID);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomGetTransfer            (HDICOMPDU hPDU, L_UCHAR nID, L_INT nIndex, L_TCHAR *strTransfer, L_UINT32 SizeInWords);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetTransfer            (HDICOMPDU hPDU, L_UCHAR nID, L_INT nIndex, L_TCHAR *pszUID);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomAddTransfer            (HDICOMPDU hPDU, L_UCHAR nID, L_TCHAR *pszUID);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomDelTransfer            (HDICOMPDU hPDU, L_UCHAR nID, L_INT nIndex);

L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsRoleSelect           (HDICOMPDU hPDU, L_UCHAR nID);
L_LTDIC_API L_UCHAR   EXT_FUNCTION L_DicomGetUserRole            (HDICOMPDU hPDU, L_UCHAR nID);
L_LTDIC_API L_UCHAR   EXT_FUNCTION L_DicomGetProviderRole        (HDICOMPDU hPDU, L_UCHAR nID);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetRoleSelect          (HDICOMPDU hPDU, L_UCHAR nID, L_BOOL bEnabled, L_UCHAR nUser, L_UCHAR nProvider);

L_LTDIC_API L_UINT32  EXT_FUNCTION L_DicomGetLengthExtended      (HDICOMPDU hPDU, L_UCHAR nID);
L_LTDIC_API L_UCHAR*  EXT_FUNCTION L_DicomGetExtended            (HDICOMPDU hPDU, L_UCHAR nID);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetExtended            (HDICOMPDU hPDU, L_UCHAR nID, L_UCHAR *pData, L_UINT32 nLength);

L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsMaxLength            (HDICOMPDU hPDU);
L_LTDIC_API L_UINT32  EXT_FUNCTION L_DicomGetMaxLength           (HDICOMPDU hPDU);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetMaxLength           (HDICOMPDU hPDU, L_BOOL bEnabled, L_UINT32 nLength);

L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsImplementClass       (HDICOMPDU hPDU);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomGetImplementClass      (HDICOMPDU hPDU, L_TCHAR *strImplementClass, L_UINT32 SizeInWords);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetImplementClass      (HDICOMPDU hPDU, L_BOOL bEnabled, L_TCHAR *pszUID);

L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsAsyncOperations      (HDICOMPDU hPDU);
L_LTDIC_API L_UINT16  EXT_FUNCTION L_DicomGetInvokedOperations   (HDICOMPDU hPDU);
L_LTDIC_API L_UINT16  EXT_FUNCTION L_DicomGetPerformedOperations (HDICOMPDU hPDU);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetAsyncOperations     (HDICOMPDU hPDU, L_BOOL bEnabled, L_UINT16 nInvoked, L_UINT16 nPerformed);

L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsImplementVersion     (HDICOMPDU hPDU);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomGetImplementVersion    (HDICOMPDU hPDU,L_TCHAR *strImplementVersion, L_UINT32 SizeInWords);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetImplementVersion    (HDICOMPDU hPDU, L_BOOL bEnabled, L_TCHAR *pszVersion);

L_LTDIC_API L_INT     EXT_FUNCTION L_DicomGetUserInfoCount       (HDICOMPDU hPDU);
L_LTDIC_API L_UCHAR   EXT_FUNCTION L_DicomGetTypeUserInfo        (HDICOMPDU hPDU, L_INT nIndex);
L_LTDIC_API L_UINT16  EXT_FUNCTION L_DicomGetLengthUserInfo      (HDICOMPDU hPDU, L_INT nIndex);
L_LTDIC_API L_UCHAR*  EXT_FUNCTION L_DicomGetDataUserInfo       (HDICOMPDU hPDU, L_INT nIndex);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSetUserInfo            (HDICOMPDU hPDU, L_INT nIndex, L_UCHAR nType, L_UCHAR *pData, L_UINT16 nLength);
L_LTDIC_API L_INT     EXT_FUNCTION L_DicomAddUserInfo            (HDICOMPDU hPDU, L_UCHAR nType, L_UCHAR *pData, L_UINT16 nLength);
L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomDelUserInfo            (HDICOMPDU hPDU, L_INT nIndex);

#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)
#endif // #if !defined(FOR_WINCE)

//============= CLASS ==========================================================
#if !defined(FOR_WINCE)

#define NET_MAX_ADDRESS_SIZE  16

typedef L_VOID *HDICOMNET;

typedef L_VOID (pEXT_CALLBACK CONNECTCALLBACK)                 (HDICOMNET hNet, L_INT nError, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK ACCEPTCALLBACK)                  (HDICOMNET hNet, L_INT nError, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK CLOSECALLBACK)                   (HDICOMNET hNet, L_INT nError, HDICOMNET hPeer, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECALLBACK)                 (HDICOMNET hNet, L_INT nError, L_UCHAR nType, L_UCHAR *pBuffer, L_UINT32 nBytes, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK SENDCALLBACK)                    (HDICOMNET hNet, L_INT nError, L_UCHAR nType, L_UINT32 nBytes, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK SENDEXTCALLBACK)                 (HDICOMNET hNet, pDICOMNETDEBUGINFOONSEND pDicomNetDebugInfoOnsend, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVEASSOCIATEREQUESTCALLBACK) (HDICOMNET hNet, HDICOMPDU hPDU, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVEASSOCIATEACCEPTCALLBACK)  (HDICOMNET hNet, HDICOMPDU hPDU, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVEASSOCIATEREJECTCALLBACK)  (HDICOMNET hNet, L_UCHAR nResult, L_UCHAR nSource, L_UCHAR nReason, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVEDATACALLBACK)             (HDICOMNET hNet, L_UCHAR nPresentationID, HDICOMDS hCS, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVERELEASEREQUESTCALLBACK)   (HDICOMNET hNet, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVERELEASERESPONSECALLBACK)  (HDICOMNET hNet, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVEABORTCALLBACK)            (HDICOMNET hNet, L_UCHAR nSource, L_UCHAR nReason, L_VOID *pUserData);

typedef L_VOID (pEXT_CALLBACK RECEIVECSTOREREQUESTCALLBACK)    (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nPriority, L_TCHAR *pszMoveAE, L_UINT16 nMoveMessageID, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECSTORERESPONSECALLBACK)   (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECFINDREQUESTCALLBACK)     (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECFINDRESPONSECALLBACK)    (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECGETREQUESTCALLBACK)      (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECGETRESPONSECALLBACK)     (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_UINT16 nRemaining, L_UINT16 nCompleted, L_UINT16 nFailed, L_UINT16 nWarning, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECMOVEREQUESTCALLBACK)     (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, L_TCHAR *pszMoveAE, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECMOVERESPONSECALLBACK)    (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_UINT16 nRemaining, L_UINT16 nCompleted, L_UINT16 nFailed, L_UINT16 nWarning, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECCANCELREQUESTCALLBACK)   (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECECHOREQUESTCALLBACK)     (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVECECHORESPONSECALLBACK)    (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENREPORTREQUESTCALLBACK)   (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nEvent, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENREPORTRESPONSECALLBACK)  (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_UINT16 nEvent, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENGETREQUESTCALLBACK)      (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT32 *pnAttribute, L_UINT32 nCount, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENGETRESPONSECALLBACK)     (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENSETREQUESTCALLBACK)      (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENSETRESPONSECALLBACK)     (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENACTIONREQUESTCALLBACK)   (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nAction, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENACTIONRESPONSECALLBACK)  (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_UINT16 nAction, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENCREATEREQUESTCALLBACK)   (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENCREATERESPONSECALLBACK)  (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, HDICOMDS hDS, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENDELETEREQUESTCALLBACK)   (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVENDELETERESPONSECALLBACK)  (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_VOID *pUserData);
typedef L_VOID (pEXT_CALLBACK RECEIVEUNKNOWNCALLBACK)          (HDICOMNET hNet, L_UCHAR nPresentationID, HDICOMDS hCS, HDICOMDS hDS, L_VOID *pUserData);

typedef L_UINT32 (pEXT_CALLBACK GETCHALLENGECALLBACK)          (HDICOMNET hNet, L_UINT64 *nChallenge, L_UINT64 nParameter, L_VOID *pUserData);
typedef L_UINT32 (pEXT_CALLBACK INTERNALAUTHENTICATECALLBACK)  (HDICOMNET hNet, L_UINT64 nChallenge, L_UINT64 *nResponse, L_UINT64 nParameter, L_VOID *pUserData);
typedef L_UINT32 (pEXT_CALLBACK EXTERNALAUTHENTICATECALLBACK)  (HDICOMNET hNet, L_UINT64 nChallenge, L_UINT64 nResponse, L_UINT64 nParameter, L_VOID *pUserData);
typedef L_VOID   (pEXT_CALLBACK ONSECURELINKREADY)             (HDICOMNET hNet, L_UINT32 nError, L_VOID *pUserData);
typedef L_VOID   (pEXT_CALLBACK ONNONSECURESENDISCL)           (HDICOMNET hNet, L_INT nError, L_UCHAR nType, L_UINT32 nLength, L_VOID *pUserData);
typedef L_VOID   (pEXT_CALLBACK ONNONSECURERECEIVEDISCL)       (HDICOMNET hNet, L_INT nError, L_CHAR *nType, L_UINT32 nLength, L_VOID *pUserData);
//debug only
typedef L_VOID   (pEXT_CALLBACK RECEIVEDISCLPACKET)            (HDICOMNET hNet, L_INT nError, L_UCHAR *pBuffer, L_UINT32 nBytes, L_VOID *pUserData);


typedef struct _DICOMNETCALLBACK
{
   CONNECTCALLBACK                  pfnConnect;
   ACCEPTCALLBACK                   pfnAccept; 
   CLOSECALLBACK                    pfnClose;  
   RECEIVECALLBACK                  pfnReceive;
   SENDCALLBACK                     pfnSend;      
   RECEIVEASSOCIATEREQUESTCALLBACK  pfnReceiveAssociateRequest;
   RECEIVEASSOCIATEACCEPTCALLBACK   pfnReceiveAssociateAccept; 
   RECEIVEASSOCIATEREJECTCALLBACK   pfnReceiveAssociateReject; 
   RECEIVEDATACALLBACK              pfnReceiveData;            
   RECEIVERELEASEREQUESTCALLBACK    pfnReceiveReleaseRequest;  
   RECEIVERELEASERESPONSECALLBACK   pfnReceiveReleaseResponse; 
   RECEIVEABORTCALLBACK             pfnReceiveAbort;           
   RECEIVECSTOREREQUESTCALLBACK     pfnReceiveCStoreRequest;   
   RECEIVECSTORERESPONSECALLBACK    pfnReceiveCStoreResponse;  
   RECEIVECFINDREQUESTCALLBACK      pfnReceiveCFindRequest;    
   RECEIVECFINDRESPONSECALLBACK     pfnReceiveCFindResponse;   
   RECEIVECGETREQUESTCALLBACK       pfnReceiveCGetRequest;     
   RECEIVECGETRESPONSECALLBACK      pfnReceiveCGetResponse;    
   RECEIVECMOVEREQUESTCALLBACK      pfnReceiveCMoveRequest;    
   RECEIVECMOVERESPONSECALLBACK     pfnReceiveCMoveResponse;   
   RECEIVECCANCELREQUESTCALLBACK    pfnReceiveCCancelRequest;  
   RECEIVECECHOREQUESTCALLBACK      pfnReceiveCEchoRequest;    
   RECEIVECECHORESPONSECALLBACK     pfnReceiveCEchoResponse;   
   RECEIVENREPORTREQUESTCALLBACK    pfnReceiveNReportRequest;  
   RECEIVENREPORTRESPONSECALLBACK   pfnReceiveNReportResponse; 
   RECEIVENGETREQUESTCALLBACK       pfnReceiveNGetRequest;     
   RECEIVENGETRESPONSECALLBACK      pfnReceiveNGetResponse;    
   RECEIVENSETREQUESTCALLBACK       pfnReceiveNSetRequest;     
   RECEIVENSETRESPONSECALLBACK      pfnReceiveNSetResponse;    
   RECEIVENACTIONREQUESTCALLBACK    pfnReceiveNActionRequest;  
   RECEIVENACTIONRESPONSECALLBACK   pfnReceiveNActionResponse; 
   RECEIVENCREATEREQUESTCALLBACK    pfnReceiveNCreateRequest;  
   RECEIVENCREATERESPONSECALLBACK   pfnReceiveNCreateResponse; 
   RECEIVENDELETEREQUESTCALLBACK    pfnReceiveNDeleteRequest;  
   RECEIVENDELETERESPONSECALLBACK   pfnReceiveNDeleteResponse; 
   RECEIVEUNKNOWNCALLBACK           pfnReceiveUnknown; 
   L_VOID                          *pUserData;
// added for ISCL compliance
   GETCHALLENGECALLBACK             pfnGetChallenge;
   INTERNALAUTHENTICATECALLBACK     pfnInternalAuthenticate;
   EXTERNALAUTHENTICATECALLBACK     pfnExternalAuthenticate;
   ONSECURELINKREADY                pfnOnSecureLinkReady;      // used by TLS also
   ONNONSECURESENDISCL              pfnOnNonSecureSendISCL;
   ONNONSECURERECEIVEDISCL          pfnOnNonSecureReceivedISCL;
// debug only
   RECEIVEDISCLPACKET               pfnOnReceivedISCLPacket;
// end of added for ISCL compliance

} DICOMNETCALLBACK, *pDICOMNETCALLBACK;


typedef L_INT (pEXT_CALLBACK PRIVATEKEYPASSWORD)           (HDICOMNET hNet, L_TCHAR *pszPassword, L_INT nSize, L_INT nFlag, L_VOID *pUserData);

typedef struct _DICOMNETCALLBACKEXT
{
   L_UINT                           uStructSize;
   PRIVATEKEYPASSWORD               pfnPrivateKeyPassword;  L_VOID *pUserDataPrivateKeyPassword;
} DICOMNETCALLBACKEXT, *pDICOMNETCALLBACKEXT;

#if defined(__cplusplus)

typedef struct _NETLINK NETLINK, *pNETLINK;
typedef struct _ISCLInternalData ISCLInternalData, *pISCLInternalData;

#if defined(LEADTOOLS_V16_OR_LATER)
typedef struct tagDICOMSOCKETOPTIONS
{
   L_UINT uStructSize;
   L_INT  nSendBufferSize;
   L_INT  nReceiveBufferSize;
   L_BOOL bNoDelay;
   L_UINT uReserved1;
   L_UINT uReserved2;
} DICOMSOCKETOPTIONS, *pDICOMSOCKETOPTIONS;
#endif

typedef struct _LDICOMNETPRIVATEDATA
{
   L_INT    nSize;
   L_BOOL   m_bEnable;

   // 0 - normal, 
   // or DEBUG_MODE_ONRECEIVE_SHOW_EXTENDED_INFO, 
   // or DEBUG_MODE_ONSEND_SHOW_EXTENDED_INFO
   L_UINT32 m_DebugModeFlags;

   L_BOOL   m_bUseExtendedOnSend;
   L_BOOL   m_bUseExtendedOnReceive;
   DICOMNETDEBUGINFOONSEND m_DebugInfoOnSend;

   L_UINT32 m_nMethodTypeSSL;       // SSL_METHOD object type
   L_INT    m_nCTX_Options;         //L_SSL_OP_NO_SSLv2, L_SSL_OP_NO_SSLv3, L_SSL_OP_NO_TLSv1
   L_VOID  *m_pfnCTX_VerifyCallback;


   // Callbacks for API
   DICOMNETCALLBACKEXT m_CallbackExt;

   L_BOOL   m_bEnableOptimizedSend ;

#if defined(LEADTOOLS_V16_OR_LATER)
   DICOMSOCKETOPTIONS m_SocketOptions;
#endif



}LDICOMNETPRIVATEDATA , * pLDICOMNETPRIVATEDATA;

class L_LTDIC_CLASS LDicomNet
{
public:

   pNETLINK m_pNetMessageQueue;
   L_VOID** m_ppNetMessageTracker;
   L_SIZE_T m_NetMessageCount;
   L_SIZE_T m_NetMessageTrackerSize;
   pISCLInternalData m_pISCLIntData;

// added for security - used by TLS and ISCL
   virtual L_VOID OnSecureLinkReady(L_UINT32 nError);  // can be 0 - OK and non-zero -  failure

// added for TLS compliance

   L_CIPHERSUITE  GetCipherFromIndexTLS(L_UINT32 nIndex);
   L_UINT32       SetCipherToIndexTLS(L_UINT32 nIndex, L_CIPHERSUITE cipher);

   L_CIPHERSUITE  GetCiphersuiteTLS();

   L_UINT32       GetEncryptionAlgorithmTLS(L_CIPHERSUITE cipher);
   L_UINT32       GetAuthenticationAlgorithmTLS(L_CIPHERSUITE cipher);
   L_UINT32       GetIntegrityAlgorithmTLS (L_CIPHERSUITE cipher);
   L_UINT32       GetKeyExchangeAlgorithmTLS(L_CIPHERSUITE cipher);

   L_UINT32       GetEncryptKeyLengthTLS (L_CIPHERSUITE cipher);
   L_UINT32       GetMutualAuthKeyLengthTLS (L_CIPHERSUITE cipher);
   L_UINT32       SetClientCertificateTLS(L_TCHAR *pszPathToCertificate, L_UINT32 certType, L_TCHAR *pszPathToKeyFile);
      // certType can be L_TLS_FILETYPE_PEM (clear text) or L_TLS_FILETYPE_ASN1 (binary)
      // the client have zero or one certificate
   L_UINT32       SetServerCertificateTLS(L_TCHAR *pszPathToCertificate, L_UINT32 certType, L_TCHAR *pszPathToKeyFile);
      // the server can have a chain of certificates in a single file
      // certType can be L_TLS_FILETYPE_PEM (clear text) or L_TLS_FILETYPE_ASN1 (binary)
      // AT LEAST ONE certificate is required for a TLS server

   virtual L_BOOL SetVerifyTLS(L_UINT32 nMode, TLS_CERT_VERIFY_CALLBACK verifyCallback);

// not to be documented
   L_UINT32       GetTLSStatus();
   L_VOID         SetTLSStatus(L_UINT32 nStatus);
      // 0 - non connected
      // 1 - client during TLS handshake
      // 2 - server during TLS_handshake
      // 3 - connected
// end of added for TLS

// added  for ISCL compliance - 18.09.2001
   // interface for ISCL operations:

   L_UINT32 SetMaxCommBlockLengthISCL(L_UINT32 nMaxCommBlockLength);   // maximum communication block size, there are performance
         // advantages if a comm block of ISCL is included into a TCP packet
   L_UINT32 GetCommBlockLengthISCL();     // return the comm block size negociated
   L_UINT32 SetMaxMessageLengthISCL(L_UINT32 nMsgLength);     // the maximum message length that can be processed - 1MB, 2MB and so on

   L_UINT32 SetMutualAuthAlgISCL(L_UINT32 mutualAuthMode);       // only 4way 3pass
   L_UINT32 SetDefaultEncryptionISCL(L_UINT32 EncryptionMode);   // bulk data encryption algorithm
   L_UINT32 SetDefaultSigningISCL(L_UINT32 SignMode);            // message authentication algorithm
   L_UINT32 GetPeerEncryptionISCL();   // can be checked to see what algorithm of communication the sender tries to use
   L_UINT32 GetPeerMACISCL();      // ATTN: if the crypt/MAC algorithm differs, the message is dropped and the GetPeerEncryption()/GetPeerMAC() will tell me
                                    // the algorithms of crypt/MAC used by the client

   L_UINT32 GetErrorSecure();       // returns the m_nISCLError in ISCL case, or m_nTLSError in case of TLS

   virtual L_UINT32 GetChallengeISCL(L_UINT64 *nChallenge, L_UINT64 nParameter);
   virtual L_UINT32 InternalAuthenticateISCL(L_UINT64 nChallenge, L_UINT64 *nResponse, L_UINT64 nParameter);
   virtual L_UINT32 ExternalAuthenticateISCL(L_UINT64 nChallenge, L_UINT64 nResponse, L_UINT64 nParameter);
         // functions described in ISO 7816-4


   L_UINT32 SetAuthDataISCL(L_VOID *pBuffer, L_UINT32 nLength);    // nLength = 1...128
   L_UINT32 GetPeerAuthDataISCL(L_VOID *pBuffer, L_UINT32 *nLength);
         // if nLength not between 1 and 128 bytes, error
         // used to set or obtain the authentication data exchanged during the mutual authentication (connectISCL/acceptISCL)

   L_UINT32 GetPeerRequestedMessageLengthISCL();      // used if local end refuse to receive
         // peer mesage because message size bigger than local end max
         // example: a server can use max 1MB message, but a client could try to send a 16 MB message

   L_UINT32 SetMutualAuthKeyISCL(L_UINT32 nIndex, L_UINT64 key);      // key used during mutual authentication
            // it looks unused for the V1.00 of ISCL
   L_UINT32 SetIndexForMutualAuthISCL(L_UINT32 nIndex);
            // index from 1 to 8 for the mutual auth key - see the remark 2 lines up
   L_UINT32 SetEncryptKeyISCL(L_UINT32 nIndex, L_UINT64 key);
            // index from 1 to 8 for the encryption key - used to load communication keys
   L_UINT32 SetIndexForEncryptISCL(L_UINT32 nIndex);
            // index from 1 to 8 to select the encryption key


   L_BOOL   IsISCLQueueEmpty();

   L_UINT32 GetIndexForEncryptISCL();
   L_UINT32 GetIndexForMutualAuthISCL();
   L_UINT32 GetStatusISCL();                          // return the internal status of the object.
         // Description of internal status will be added in docs, see also the source

   L_INT    CloseForced(L_BOOL bForced);

   L_INT32  SendNonSecureISCL(L_UCHAR *pBuffer, L_UINT32 nBytes);
   virtual  L_VOID OnNonSecureSendISCL(L_INT nError, L_UCHAR nType, L_UINT32 nLength);
   virtual  L_VOID OnNonSecureReceivedISCL(L_UINT32 nError, L_UCHAR *pBuffer, L_UINT32 nLength);

   virtual  L_VOID OnReceivedISCLPacket(L_INT nError, L_UCHAR *pBuffer, L_UINT32 nBytes);  // error code, buffer and length

   L_UINT32 GetSecureMode();

// end of added for ISCL

   LDicomNet(L_TCHAR *pszPath, L_INT32 nMode);
      // modified for security: Added L_INT32 nMode parameter of constructor.
      // Values for nMode: DICOM_SECURE_ISCL, DICOM_SECURE_TLS, DICOM_SECURE_NONE
      // if a bad value is supplied, security will be DICOM_SECURE_NONE

   LDicomNet::LDicomNet(L_TCHAR *pszPath, L_INT32 nMode, L_BOOL bReserved);


   virtual ~LDicomNet();

   static L_INT     StartUp                    ();
   static L_VOID    ShutDown                   ();

   L_INT            Connect                    (L_TCHAR *pszHostAddress, L_UINT nHostPort, L_TCHAR *pszPeerAddress, L_UINT nPeerPort);
   L_INT            Listen                     (L_TCHAR *pszHostAddress, L_UINT nHostPort, L_INT nNbPeers);
   L_INT            Accept                     (LDicomNet *pNet);
   L_VOID           Close                      ();
   
   L_INT            SendAssociateRequest       (LDicomAssociate *pPDU);
   L_INT            SendAssociateAccept        (LDicomAssociate *pPDU);
   L_INT            SendAssociateReject        (L_UCHAR nResult, L_UCHAR nSource, L_UCHAR nReason);
   L_INT            SendData                   (L_UCHAR nPresentationID, LDicomDS *pCS, LDicomDS *pDS);
   L_INT            SendReleaseRequest         ();
   L_INT            SendReleaseResponse        ();
   L_INT            SendAbort                  (L_UCHAR nSource, L_UCHAR nReason);

   L_INT            SendCStoreRequest          (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nPriority, L_TCHAR *pszMoveAE, L_UINT16 nMoveMessageID, LDicomDS *pDS);
   L_INT            SendCStoreResponse         (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus);
   L_INT            SendCFindRequest           (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, LDicomDS *pDS);
   L_INT            SendCFindResponse          (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, LDicomDS *pDS);
   L_INT            SendCGetRequest            (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, LDicomDS *pDS);
   L_INT            SendCGetResponse           (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_UINT16 nRemaining, L_UINT16 nCompleted, L_UINT16 nFailed, L_UINT16 nWarning, LDicomDS *pDS);
   L_INT            SendCMoveRequest           (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, L_TCHAR *pszMoveAE, LDicomDS *pDS);
   L_INT            SendCMoveResponse          (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_UINT16 nRemaining, L_UINT16 nCompleted, L_UINT16 nFailed, L_UINT16 nWarning, LDicomDS *pDS);
   L_INT            SendCCancelRequest         (L_UCHAR nPresentationID, L_UINT16 nMessageID);
   L_INT            SendCEchoRequest           (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass);
   L_INT            SendCEchoResponse          (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus);

   L_INT            SendNReportRequest         (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nEvent, LDicomDS *pDS);
   L_INT            SendNReportResponse        (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_UINT16 nEvent, LDicomDS *pDS);
   L_INT            SendNGetRequest            (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT32 *pnAttribute, L_UINT16 nCount);
   L_INT            SendNGetResponse           (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, LDicomDS *pDS);
   L_INT            SendNSetRequest            (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, LDicomDS *pDS);
   L_INT            SendNSetResponse           (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, LDicomDS *pDS);
   L_INT            SendNActionRequest         (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nAction, LDicomDS *pDS);
   L_INT            SendNActionResponse        (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_UINT16 nAction, LDicomDS *pDS);
   L_INT            SendNCreateRequest         (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, LDicomDS *pDS);
   L_INT            SendNCreateResponse        (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, LDicomDS *pDS);
   L_INT            SendNDeleteRequest         (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance);
   L_INT            SendNDeleteResponse        (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus);

   L_VOID           EnableOptimizedSend        (L_BOOL bEnable);

   L_BOOL           IsActivated                ();
   L_BOOL           IsConnected                ();
   L_BOOL           IsAssociated               ();

   L_UINT32         GetQueueSend               ();

   LDicomNet       *GetServer                  ();
   L_UINT32         GetClientCount             ();
   LDicomNet       *GetClient                  (L_UINT nIndex);

   L_INT            GetHostInfo                (L_TCHAR *pszAddress, L_UINT32 AddressSizeInWords, L_UINT *pnPort);
   L_INT            GetPeerInfo                (L_TCHAR *pszAddress, L_UINT32 AddressSizeInWords, L_UINT *pnPort);

   LDicomAssociate *GetAssociate               ();

   virtual L_VOID   OnConnect                  (L_INT nError);
   virtual L_VOID   OnAccept                   (L_INT nError);
   virtual L_VOID   OnClose                    (L_INT nError, LDicomNet *pNet);
   virtual L_VOID   OnReceive                  (L_INT nError, L_UCHAR nType, L_UCHAR *pBuffer, L_UINT32 nBytes);
   virtual L_VOID   OnSend                     (L_INT nError, L_UCHAR nType, L_UINT32 nBytes);

   virtual L_VOID   OnReceiveAssociateRequest  (LDicomAssociate *pPDU);
   virtual L_VOID   OnReceiveAssociateAccept   (LDicomAssociate *pPDU);
   virtual L_VOID   OnReceiveAssociateReject   (L_UCHAR nResult, L_UCHAR nSource, L_UCHAR nReason);
   virtual L_VOID   OnReceiveData              (L_UCHAR nPresentationID, LDicomDS *pCS, LDicomDS *pDS);
   virtual L_VOID   OnReceiveReleaseRequest    ();
   virtual L_VOID   OnReceiveReleaseResponse   ();
   virtual L_VOID   OnReceiveAbort             (L_UCHAR nSource, L_UCHAR nReason);

   virtual L_VOID   OnReceiveCStoreRequest     (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nPriority, L_TCHAR *pszMoveAE, L_UINT16 nMoveMessageID, LDicomDS *pDS);
   virtual L_VOID   OnReceiveCStoreResponse    (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus);
   virtual L_VOID   OnReceiveCFindRequest      (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, LDicomDS *pDS);
   virtual L_VOID   OnReceiveCFindResponse     (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, LDicomDS *pDS);
   virtual L_VOID   OnReceiveCGetRequest       (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, LDicomDS *pDS);
   virtual L_VOID   OnReceiveCGetResponse      (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_UINT16 nRemaining, L_UINT16 nCompleted, L_UINT16 nFailed, L_UINT16 nWarning, LDicomDS *pDS);
   virtual L_VOID   OnReceiveCMoveRequest      (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, L_TCHAR *pszMoveAE, LDicomDS *pDS);
   virtual L_VOID   OnReceiveCMoveResponse     (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_UINT16 nRemaining, L_UINT16 nCompleted, L_UINT16 nFailed, L_UINT16 nWarning, LDicomDS *pDS);
   virtual L_VOID   OnReceiveCCancelRequest    (L_UCHAR nPresentationID, L_UINT16 nMessageID);
   virtual L_VOID   OnReceiveCEchoRequest      (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass);
   virtual L_VOID   OnReceiveCEchoResponse     (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus);
   virtual L_VOID   OnReceiveNReportRequest    (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nEvent, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNReportResponse   (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_UINT16 nEvent, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNGetRequest       (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT32 *pnAttribute, L_UINT16 nCount);
   virtual L_VOID   OnReceiveNGetResponse      (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNSetRequest       (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNSetResponse      (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNActionRequest    (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nAction, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNActionResponse   (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_UINT16 nAction, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNCreateRequest    (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNCreateResponse   (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, LDicomDS *pDS);
   virtual L_VOID   OnReceiveNDeleteRequest    (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance);
   virtual L_VOID   OnReceiveNDeleteResponse   (L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus);
   virtual L_VOID   OnReceiveUnknown           (L_UCHAR nPresentationID, LDicomDS *pCS, LDicomDS *pDS);

   L_SSL_CTX  *GetCTX();
   L_INT32   CTXSetOptions(L_UINT uOptions);
   L_INT     SetSLLMethodType(L_INT nMethodType);


   //---------------------------------------------------------------------------
   // Private Functions (Internal use only)
   //---------------------------------------------------------------------------
   L_VOID           SetCallback                (pDICOMNETCALLBACK pCallback);
   L_VOID           SetCallbackExt             (pDICOMNETCALLBACKEXT pCallbackExt);

   L_UINT32 SetDebugMode(L_UINT32 nDebugMode);  // returns the old debug mode
   virtual L_VOID   OnSendExt(pDICOMNETDEBUGINFOONSEND pDicomNetDebugInfoOnsend);
   virtual L_VOID OnCreateCTX(L_SSL_CTX *pCtx);
   virtual L_INT  OnPrivateKeyPassword(L_TCHAR *pszPassword, L_INT nSize, L_INT nFlag);
   L_VOID   FreeISCLData(L_BOOL bFreeISCLMessages);
   L_VOID   TrackISCLMessage(L_VOID* pMessage);
   L_VOID   FreeISCLMessages(L_BOOL bFreeAll);
   

 
private:
   L_BOOL                  m_bConnected;
   L_BOOL                  m_bAssociated;
   pLDICOMNETPRIVATEDATA   m_pPrivateData;
   LDicomAssociate         m_AssociateRQ;
   LDicomAssociate         m_AssociateAC;
   LDicomFile              m_SendFile;
   LDicomDS                m_SendCS;
   LDicomFile              m_ReceiveFileCS;
   LDicomFile              m_ReceiveFileDS;
   LDicomDS                m_ReceiveCS;
   LDicomDS                m_ReceiveDS;
   //L_CHAR                  m_szPath[_MAX_PATH];
   DICOMNETCALLBACK        m_Callback;
   static HINSTANCE        m_hLibrary;
   static HINSTANCE        m_hLibrary2;
   static HINSTANCE        m_hLibCry;
   static HINSTANCE        m_hLibTLS;

   L_UINT16 GetTS       (L_BOOL bCommand, L_CHAR *pszUID);
   L_INT    Send        (L_CHAR *pBuffer, L_UINT32 nBytes);
   L_VOID   Receive     (L_INT nError, L_UCHAR nType, L_UCHAR *pBuffer, L_UINT32 nBytes);
   L_VOID   ReceiveData (L_UCHAR nPresentationID, LDicomDS *pCS, LDicomDS *pDS);
   L_INT    SendData    (L_BOOL bCommand, L_UCHAR nPresentationID, LDicomDS *pDS);

   static L_INT   GetError(L_UINT32 nError);
   friend LRESULT CALLBACK L_NetWindowCallback (HWND hWnd, UINT nMessage, WPARAM wParam, LPARAM lParam);   
// added for ISCL compliance
   // private members:
   L_UINT32 m_nTLSError;
   L_BOOL   m_bInTransmission;    // set when a transmission is processed
                              // not set when the ISCL layer is ready to accept another job (send/receive)
         // differs from m_Status: m_Status keep his value after exiting (w/ error) from a transmit job)

//   L_BOOL   m_bIsServer;      // to know if I call OnAccept or OnConnect
   L_UINT32 m_SecurityStandard;     // choose between DICOM_ISCL, DICOM_TLS and DICOM_NO_SECURITY
   L_UCHAR  m_AuthData[128];
   L_UCHAR  m_PeerAuthData[128];
   L_UINT32 m_nAuthDataLength, m_nPeerAuthDataLength;
   L_UINT32 m_nDelay_ms;    // wait time before timeout error, in milliseconds - not usable w/ LEADtk

   L_UINT32 m_nMaxCommBlockLength;   // max acceptable, automagically = 8kB
   L_UINT32 m_nCommBlockLength;        // negociated block size
   L_UINT32 m_nMaxMessageLength;     // the size of the message the object (server or client) is able to send/receive
   L_UINT32 m_nPeerRequestedMsgLength; // the size the other comm end tried to send


   L_UINT32 m_CryptAlg, m_PeerCryptAlg;
   L_UINT32 m_MACAlg, m_PeerMACAlg;   // DESMAC or MD5 or none. MUST be the same for communication

   L_UINT32 m_Status, m_StatusOld;    // internal status (communication wise) and previous status

   L_UINT32 m_mutualAuthAlg; // only 4way 3pass is defined in standard
   L_UINT64 m_AuthKey[8];    // the keys for  mutual auth
   L_UINT64 m_EncryptKey[8]; // the keys used for encryption
   L_UINT32 m_nAuthKeyId, m_nEncryptKeyId;  // 0-based indexes
            // m_nEncryptKeyId is KEY_ONE to KEY_EIGHT or UNIQUE_KEYS
            // ATTN: if uniques keys, then every new message is sent with a key that is negociated at that moment
   L_UINT64 m_sessionKey;         // this is the key used for en/decryption. It is initialized to the correct key
         // or takes the value negociated during a "Random number for generating session key" request/response

   L_BOOL   m_bUseAutoChangeKeys;

   L_UINT32 m_nRequestedSendLength;
   L_UINT32 m_nCurrentSendLength;
   L_UCHAR  m_cCurrentType;

   L_UINT32 m_nISCLError;  // used because the function of treating messages does not return values
   L_UINT64 m_n64ServerChallenge, m_n64ServerResponse, m_n64ClientChallenge, m_n64ClientResponse;
         // even if looks weird, the server generates n64ChallengeServer and n64ResponseClient
         // so I check the n64ChServer and n64RespServer if match - it is a n64RespForServerChallenge



   L_UCHAR m_ivec[8];      // m_ivec is the initialisation vector for encryption with DES CBC
            // it is set at 0 in connect and accept. If the DESCBC_CHAIN_IVEC_OVER_SIGNATURE or DESCBC_CHAIN_IVEC_OVER_MAC are
            // NOT set, the ivec is set to 0 after every encrypt operation

   L_UCHAR m_sSignature[24];     // I don't use dynamic allocated memory
   L_UCHAR m_sEncryptedSignature[24];

   L_CHAR *m_pRecvEncryptedData, *m_pRecvDecryptedData;
   L_UINT32 m_nRequestedDataLength, m_nRecvDataIndex, m_nRecvDecryptedLength;

// private (implementation) methods:

   L_UINT32 Sign(L_VOID *pBuffer, L_UINT32 nLength, L_VOID *pSign, L_UINT32 *nOutLength);
         // generates a MAC code for the data from pBuffer using the current MAC method
         // the length of the MAc is 64 bits for DESMAC and 128 bits for MD5


   L_UINT32 CryptSendDataISCL(L_CHAR *pBuffer, L_UINT32 nLength);
         // gets data from pBuffer and sent it thu network, encrypted w/ current rypting algorithm
   L_UINT32 CryptSendMACISCL(L_CHAR *pBuffer, L_UINT32 nLength);
         // send a MAC for data in pBuffer, crypted w/ current algorithm

   L_UINT32 Get32bDataFromOffset(L_UCHAR *pBuffer, L_UINT32 nOffset);
   L_UINT64 Get64bDataFromOffset(L_UCHAR *pBuffer, L_UINT32 nOffset);

   L_INT32  SendRaw(L_CHAR *pBuffer, L_UINT32 nBytes); // just like Send in non secure mode

   L_UINT32 Encrypt (L_VOID *pBufferIn,
                        L_UINT32 nLength,
                        L_VOID *pBufferOut,
                        L_UINT32 *nLengthOut
                       );
         // add padding to pBufferIn, encrypt nLength bytes from pBufferIn
         // and write the crypted msg into pBufferOut. nLengthOut is the size of crypted msg
         // if called with pBufferOut == NULL, then return the required length
         // also when nLengthOut is too small
         // returns DICOM_SUCCES if OK, not 0 if nLengthLut too small
         // nLengthOut is updated to the necessary size
         // ivec is the initialisation vector. It is used to chain the previous state for the DES CBC encryption algorithm
         // ivec is a 8 bytes array

   L_UINT32 Decrypt (L_VOID *pBufferIn,
                        L_UINT32 nLength,
                        L_VOID *pBufferOut,
                        L_UINT32 *nLengthOut
                       );
         // ATTN: MiDecrypt uses an BufferOut capable to contain a number of
         // blocks (is longer with at most 8 bytes than the actual decrypted message)
         // returns DICOM_SUCCESS if data decrypted and padding removed. Update nLengthOut to correct length of decrypted message
         // return error if decryption failed (could not clear the padding from the end)
         // ivec is the initialisation vector. It is used to chain the previous state for the DES CBC encryption algorithm
         // ivec is a 8 bytes array

   L_UINT32 MD5Sign(L_VOID *pBuffer, L_UINT32 nLength, L_VOID *pSign);
      // creates a message authentication code of the L_VOID *pBuffer, put it
      //    into the pSign and returns the length of sign
      // ATTN: as I know the sign method, I know the length of MAC - 16 bytes
   L_UINT32 DESMACSign(L_VOID *pBuffer, L_UINT32 nLength, L_VOID *pSign);
      // this time only 64-bit (8 bytes) data
      // DESMAC use the default key for sign. MD5 does not use key for sign, but at the end
      // the message digest is encrypted with the current encryption key and algorithm (if any)


   L_INT32 LineReset();


   L_INT32  LineConnectionCheckRequest ();
   L_INT32  LineConnectionCheckResponse(L_UINT32 status, L_UINT32 nISCLCommBlockLength);

   L_INT32  MutualAuthRequest          (L_UINT32 mutualAuthMethod,
                                        L_UINT32 mutualKeyPairId
                                       );
   L_INT32  MutualAuthResponse(L_UINT32 status);
   L_INT32  MutualAuthPass1Notification(L_UINT32 status,
                                        L_UINT64 nChallenge,
                                        L_UINT32 nRetGCh
                                       );
   L_INT32  MutualAuthPass2Notification(
                                        L_UINT32 status,
                                        L_UINT64 nResponseCode,
                                        L_UINT64 nChallenge2,
                                        L_UINT32 nOption = 0   // result from GetCh or InternalAuth
                                                                // 0 is OK
                                       );
   L_INT32  MutualAuthPass3Notification(
                                        L_UINT32 status,
                                        L_UINT64 nResponseCode2,
                                        L_UINT32 nOption = 0   // result code from ExtAuth or IntAuth
                                                                // 0 is OK
                                       );
   L_INT32  MutualAuthCompletion(L_UINT32 nAuthCode);
   L_INT32  SendTransmissionRequest    (L_UINT32 EncryptMethod, 
                                        L_UINT32 nEncryptKeyId,
                                        L_UINT32 MessageAuthMethod,
                                        L_UINT32 nMessageLength
                                       );
   L_INT32  SendTransmissionResponse   (L_UINT32 acknoledgeCode,
                                        L_UINT32 nMessageLength
                                       );
   L_INT32  RandomNoForSessionKeyRequest(
                                        L_UINT64 nRndNoForSessionKey,
                                        L_UINT32 nResultCode
                                       );
   L_INT32  RandomNoForSessionKeyResponse(L_UINT32 nResultCode);

   L_INT32  MsgTransmitNotification    (L_VOID *rawData,
                                        L_UINT32 nSplitMsgLength
                                       );

   L_INT32  MsgAuthCode                (L_VOID *pAuthCode,
                                        L_UINT32 nAuthCodeLength
                                       );

   L_INT32  ThroughModeTransmission    (L_VOID *rawData,
                                        L_UINT32 nMessageLength  // is not limited by comm block size
                                       );

   L_INT32  LineDisconnectionRequest();
   L_INT32  LineDisconnectionResponse(L_UINT32 status);

   L_INT32  SetHeader                  (L_VOID *pBuffer,
                                        L_UINT32 indicator,
                                        L_UINT32 messageId,
                                        L_UINT32 nDataLength,
                                        L_UINT32 option,
                                        L_UINT32 timeStamp,
                                        L_UINT32 errNo,
                                        L_UINT32 stuff32_1 = 0,
                                        L_UINT32 stuff32_2 = 0
                                       );

   L_INT32  CreateEmptyMessage(L_VOID **pBuffer, L_UINT32 nLength);
      // allocates a area of size 32+nLength (header and data);

   L_INT32  CopyMessageData(L_VOID *pPacket, L_UINT32 nOffset, L_VOID *pData, L_UINT32 nLength);
      // copy raw data into message data area starting from offset bytes

   L_INT32  OnISCLPacketSend(L_INT nError, L_UINT32 nMsgId, L_UINT32 nLength);

// end of added for ISCL

// added for TLS compliance

   L_SSL_CTX *m_pCtx;
   L_CIPHERSUITE m_ciphersuites[16];
   L_CIPHERSUITE m_ActualCiphersuite;
   L_VOID InitializePrivateData();
   L_INT UpdateCTX(L_SSL_CTX_CREATE *pCtx);

   static L_INT VerifyCB(int ok, X509_STORE_CTX *store);
   static L_INT PrivateKeyPasswordCB(L_CHAR *pszPassword, L_INT nSize, L_INT nFlag, L_VOID *userdata);

public:
// not to be documented
   L_BOOL   m_bISCLConnected;
   L_UINT32 m_nTLSVerifyMode;
   SSL *m_TLSConn;
   static LDicomNet *m_pNetTLS[512];
   HWND             m_hWnd;
   L_VOID SetIsSecureConnected(L_BOOL bIsConnected);
   L_BOOL GetIsSecureConnected();

   L_VOID SetHandshakeDebug(L_BOOL isDebug, L_VOID (*myfn)(LDicomNet *pDic, const char *TLS_STATUS, int ret));

// end of not to be documented
// end of added for TLS

   
protected:
   void LDicomNet::Initialize(L_TCHAR *pszPath, L_INT32 nMode);

public:
   L_INT Initialize(L_TCHAR *pszPath, L_INT32 nMode, pL_SSL_CTX_CREATE pCtxCreate);

#if defined(LEADTOOLS_V16_OR_LATER)
   L_INT GetDefaultSocketOptions(pDICOMSOCKETOPTIONS pOptions, L_UINT uStructSize);
   L_INT GetSocketOptions(pDICOMSOCKETOPTIONS pOptions, L_UINT uStructSize);
   L_INT SetSocketOptions(pDICOMSOCKETOPTIONS pOptions);
#endif
};

#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)
// modified for security
   // added parameter nMode
   L_LTDIC_API HDICOMNET EXT_FUNCTION L_DicomCreateNet                  (L_TCHAR *pszPath, L_INT32 nMode);
   L_LTDIC_API HDICOMNET EXT_FUNCTION L_DicomCreateNetExt               (L_TCHAR *pszPath, L_INT32 nMode, pL_SSL_CTX_CREATE pCtxCreate);
// end of modified for security


// added for ISCL compliance
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetMaxCommBlockLengthISCL(HDICOMNET hNet, L_UINT32 nCommBlockLength);   // maximum communication block size, there are performance
         // advantages if a comm block of ISCL is included into a TCP packet
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetCommBlockLengthISCL(HDICOMNET hNet);     // return the comm block size negociated
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetMaxMessageLengthISCL(HDICOMNET hNet, L_UINT32 nMsgLength);     // the maximum message length that can be processed - 1MB, 2MB and so on

   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetMutualAuthAlgISCL(HDICOMNET hNet, L_UINT32 mutualAuthMode);       // only 4way 3pass
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetDefaultEncryptionISCL(HDICOMNET hNet, L_UINT32 EncryptionMode);   // bulk data encryption algorithm
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetDefaultSigningISCL(HDICOMNET hNet, L_UINT32 SignMode);            // message authentication algorithm
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetPeerEncryptionISCL(HDICOMNET hNet);   // can be checked to see what algorithm of communication the sender tries to use
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetPeerMACISCL(HDICOMNET hNet);      // ATTN: if the crypt/MAC algorithm differs, the message is dropped and the GetPeerEncryption()/GetPeerMAC() will tell me
                                    // the algorithms of crypt/MAC used by the client

   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetErrorSecure(HDICOMNET hNet);       // returns the m_nISCLError in ISCL case, or m_nTLSError in case of TLS



         // functions described in ISO 7816-4


   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetAuthDataISCL(HDICOMNET hNet, L_VOID *pBuffer, L_UINT32 nLength);    // nLength = 1...128
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetPeerAuthDataISCL(HDICOMNET hNet, L_VOID *pBuffer, L_UINT32 *nLength);
         // if nLength not between 1 and 128 bytes, error
         // used to set or obtain the authentication data exchanged during the mutual authentication (connectISCL/acceptISCL)

   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetPeerRequestedMessageLengthISCL(HDICOMNET hNet);      // used if local end refuse to receive
         // peer mesage because message size bigger than local end max
         // example: a server can use max 1MB message, but a client could try to send a 16 MB message

   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetMutualAuthKeyISCL(HDICOMNET hNet, L_UINT32 nIndex, L_UINT64 nKey);      // key used during mutual authentication
            // it looks unused for the V1.00 of ISCL
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetIndexForMutualAuthISCL(HDICOMNET hNet, L_UINT32 nIndex);
            // index from 1 to 8 for the mutual auth key - see the remark 2 lines up
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetEncryptKeyISCL(HDICOMNET hNet, L_UINT32 nIndex, L_UINT64 key);
            // index from 1 to 8 for the encryption key - used to load communication keys
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomSetIndexForEncryptISCL(HDICOMNET hNet, L_UINT32 nIndex);
            // index from 1 to 8 to select the encryption key
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetIndexForEncryptISCL(HDICOMNET hNet);
   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetIndexForMutualAuthISCL(HDICOMNET hNet);

   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetStatusISCL(HDICOMNET hNet);                          // return the internal status of the object.
         // Description of internal status will be added in docs, see also the source

   L_LTDIC_API L_BOOL   EXT_FUNCTION L_DicomIsISCLQueueEmpty(HDICOMNET hNet);

   L_LTDIC_API L_INT    EXT_FUNCTION L_DicomCloseForced(HDICOMNET hNet, L_BOOL bForced);

   L_LTDIC_API L_INT32  EXT_FUNCTION L_DicomSendNonSecureISCL(HDICOMNET hNet, L_UCHAR *pBuffer, L_UINT32 nBytes);

   L_LTDIC_API L_UINT32 EXT_FUNCTION L_DicomGetSecureMode(HDICOMNET hNet);
// end of added for ISCL compliance

// added for TLS compliance
   L_LTDIC_API L_CIPHERSUITE EXT_FUNCTION L_DicomGetCipherFromIndexTLS(HDICOMNET hNet, L_UINT32 nIndex);
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomSetCipherToIndexTLS(HDICOMNET hNet, L_UINT32 nIndex, L_CIPHERSUITE cipher);
   L_LTDIC_API L_CIPHERSUITE EXT_FUNCTION L_DicomGetCiphersuiteTLS(HDICOMNET hNet);
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomGetEncryptionAlgorithmTLS(HDICOMNET hNet, L_CIPHERSUITE cipher);   
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomGetAuthenticationAlgorithmTLS(HDICOMNET hNet, L_CIPHERSUITE cipher);   
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomGetIntegrityAlgorithmTLS(HDICOMNET hNet, L_CIPHERSUITE cipher);   
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomGetKeyExchangeAlgorithmTLS(HDICOMNET hNet, L_CIPHERSUITE cipher);   
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomGetEncryptKeyLengthTLS(HDICOMNET hNet, L_CIPHERSUITE cipher);
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomGetMutualAuthKeyLengthTLS(HDICOMNET hNet, L_CIPHERSUITE cipher);
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomSetClientCertificateTLS(HDICOMNET hNet, L_TCHAR *pszPathToCertificateFile, L_UINT32 nCertType, L_TCHAR *pszPathToKeyFile);
   L_LTDIC_API L_UINT32      EXT_FUNCTION L_DicomSetServerCertificateTLS(HDICOMNET hNet, L_TCHAR *pszPathToCertificateFile, L_UINT32 nCertType, L_TCHAR *pszPathToKeyFile);
   L_LTDIC_API L_VOID        EXT_FUNCTION L_DicomSetIsSecureConnected(HDICOMNET hNet, L_BOOL bIsConnected);
   L_LTDIC_API L_BOOL        EXT_FUNCTION L_DicomGetIsSecureConnected(HDICOMNET hNet);

// end of added for TLS compliance

   L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomFreeNet                    (HDICOMNET hNet);

   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomStartUp                    ();
   L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomShutDown                   ();

   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomConnect                    (HDICOMNET hNet, L_TCHAR *pszHostAddress, L_UINT nHostPort, L_TCHAR *pszPeerAddress, L_UINT nPeerPort);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomListen                     (HDICOMNET hNet, L_TCHAR *pszHostAddress, L_UINT nHostPort, L_INT nNbPeers);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomAccept                     (HDICOMNET hNet, HDICOMNET hPeer);
   L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomClose                      (HDICOMNET hNet);
   
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendAssociateRequest       (HDICOMNET hNet, HDICOMPDU hPDU);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendAssociateAccept        (HDICOMNET hNet, HDICOMPDU hPDU);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendAssociateReject        (HDICOMNET hNet, L_UCHAR nResult, L_UCHAR nSource, L_UCHAR nReason);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendData                   (HDICOMNET hNet, L_UCHAR nPresentationID, HDICOMDS hCS, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendReleaseRequest         (HDICOMNET hNet);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendReleaseResponse        (HDICOMNET hNet);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendAbort                  (HDICOMNET hNet, L_UCHAR nSource, L_UCHAR nReason);

   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCStoreRequest          (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nPriority, L_TCHAR *pszMoveAE, L_UINT16 nMoveMessageID, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCStoreResponse         (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCFindRequest           (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCFindResponse          (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCGetRequest            (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCGetResponse           (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_UINT16 nRemaining, L_UINT16 nCompleted, L_UINT16 nFailed, L_UINT16 nWarning, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCMoveRequest           (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nPriority, L_TCHAR *pszMoveAE, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCMoveResponse          (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus, L_UINT16 nRemaining, L_UINT16 nCompleted, L_UINT16 nFailed, L_UINT16 nWarning, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCCancelRequest         (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCEchoRequest           (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendCEchoResponse          (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_UINT16 nStatus);

   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNReportRequest         (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nEvent, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNReportResponse        (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_UINT16 nEvent, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNGetRequest            (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT32 *pnAttribute, L_UINT16 nCount);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNGetResponse           (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNSetRequest            (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNSetResponse           (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNActionRequest         (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nAction, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNActionResponse        (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, L_UINT16 nAction, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNCreateRequest         (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNCreateResponse        (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, HDICOMDS hDS);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNDeleteRequest         (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomSendNDeleteResponse        (HDICOMNET hNet, L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR *pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus);

   L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsActivated                (HDICOMNET hNet);
   L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsConnected                (HDICOMNET hNet);
   L_LTDIC_API L_BOOL    EXT_FUNCTION L_DicomIsAssociated               (HDICOMNET hNet);

   L_LTDIC_API L_UINT32  EXT_FUNCTION L_DicomGetQueueSend               (HDICOMNET hNet);

   L_LTDIC_API HDICOMNET EXT_FUNCTION L_DicomGetServer                  (HDICOMNET hNet);
   L_LTDIC_API L_UINT32  EXT_FUNCTION L_DicomGetClientCount             (HDICOMNET hNet);
   L_LTDIC_API HDICOMNET EXT_FUNCTION L_DicomGetClient                  (HDICOMNET hNet, L_UINT nIndex);

   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomGetHostInfo                (HDICOMNET hNet, L_TCHAR *pszAddress, L_UINT32 AddressSizeInWords, L_UINT *pnPort);
   L_LTDIC_API L_INT     EXT_FUNCTION L_DicomGetPeerInfo                (HDICOMNET hNet, L_TCHAR *pszAddress, L_UINT32 AddressSizeInWords, L_UINT *pnPort);

   L_LTDIC_API HDICOMPDU EXT_FUNCTION L_DicomGetAssociate               (HDICOMNET hNet);

   L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomSetCallback                (HDICOMNET hNet, pDICOMNETCALLBACK pCallback);
   L_LTDIC_API L_VOID    EXT_FUNCTION L_DicomSetCallbackExt             (HDICOMNET hNet, pDICOMNETCALLBACKEXT pCallbackExt);

#if defined(LEADTOOLS_V16_OR_LATER)
   L_LTDIC_API L_INT    EXT_FUNCTION L_DicomSetSocketOptions            (HDICOMNET hNet, pDICOMSOCKETOPTIONS pOptions);
   L_LTDIC_API L_INT    EXT_FUNCTION L_DicomGetSocketOptions            (HDICOMNET hNet, pDICOMSOCKETOPTIONS pOptions, L_UINT uStructSize);
   L_LTDIC_API L_INT    EXT_FUNCTION L_DicomGetDefaultSocketOptions     (HDICOMNET hNet, pDICOMSOCKETOPTIONS pOptions, L_UINT uStructSize);
#endif


#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)


//============= CLASS ==========================================================
// SOP/Meta SOP Classes
#define PRINTSCU_BASIC_GRAYSCALE_PM_META_SOP_CLASS           0x01
#define PRINTSCU_BASIC_COLOR_PM_META_SOP_CLASS               0x02
#define PRINTSCU_PULL_STORED_PM_META_SOP_CLASS               0x04
#define PRINTSCU_BASIC_ANNOTATION_BOX_SOP_CLASS              0x08
#define PRINTSCU_BASIC_PRINT_IMAGE_OVERLAY_BOX_SOP_CLASS     0x10
#define PRINTSCU_PRESENTATION_LUT_SOP_CLASS                  0x20
#define PRINTSCU_PRINT_JOB_SOP_CLASS                         0x40
#define PRINTSCU_PRINTER_CONFIGURATION_RETRIEVAL_SOP_CLASS   0x80

// Statuses
#define PRINTSCU_STATUS_CONNECT                                 1
#define PRINTSCU_STATUS_SEND_ASSOCIATE_RQ                       2
#define PRINTSCU_STATUS_RECEIVE_ASSOCIATE_RJ                    3
#define PRINTSCU_STATUS_RECEIVE_ASSOCIATE_AC                    4
#define PRINTSCU_STATUS_SEND_RELEASE_RQ                         5
#define PRINTSCU_STATUS_CLOSE                                   6
#define PRINTSCU_STATUS_RECEIVE_RELEASE_RP                      7
#define PRINTSCU_STATUS_RECEIVE_RELEASE_RQ                      8
#define PRINTSCU_STATUS_SEND_RELEASE_RP                         9
#define PRINTSCU_STATUS_RECEIVE_ABORT                           10
#define PRINTSCU_STATUS_SEND_CREATE_FILM_SESSION_RQ             11
#define PRINTSCU_STATUS_RECEIVE_CREATE_FILM_SESSION_RSP         12
#define PRINTSCU_STATUS_SEND_UPDATE_FILM_SESSION_RQ             13
#define PRINTSCU_STATUS_RECEIVE_UPDATE_FILM_SESSION_RSP         14
#define PRINTSCU_STATUS_SEND_PRINT_FILM_SESSION_RQ              15
#define PRINTSCU_STATUS_RECEIVE_PRINT_FILM_SESSION_RSP          16
#define PRINTSCU_STATUS_SEND_DELETE_FILM_SESSION_RQ             17
#define PRINTSCU_STATUS_RECEIVE_DELETE_FILM_SESSION_RSP         18
#define PRINTSCU_STATUS_SEND_CREATE_FILM_BOX_RQ                 19
#define PRINTSCU_STATUS_RECEIVE_CREATE_FILM_BOX_RSP             20
#define PRINTSCU_STATUS_SEND_UPDATE_FILM_BOX_RQ                 21
#define PRINTSCU_STATUS_RECEIVE_UPDATE_FILM_BOX_RSP             22
#define PRINTSCU_STATUS_SEND_PRINT_FILM_BOX_RQ                  23
#define PRINTSCU_STATUS_RECEIVE_PRINT_FILM_BOX_RSP              24
#define PRINTSCU_STATUS_SEND_DELETE_FILM_BOX_RQ                 25
#define PRINTSCU_STATUS_RECEIVE_DELETE_FILM_BOX_RSP             26
#define PRINTSCU_STATUS_SEND_UPDATE_IMAGE_BOX_RQ                27
#define PRINTSCU_STATUS_RECEIVE_UPDATE_IMAGE_BOX_RSP            28
#define PRINTSCU_STATUS_SEND_UPDATE_ANNOTATION_BOX_RQ           29
#define PRINTSCU_STATUS_RECEIVE_UPDATE_ANNOTATION_BOX_RSP       30
#define PRINTSCU_STATUS_SEND_GET_PRINTER_INFO_RQ                31
#define PRINTSCU_STATUS_RECEIVE_GET_PRINTER_INFO_RSP            32
#define PRINTSCU_STATUS_SEND_GET_PRINT_JOB_INFO_RQ              33
#define PRINTSCU_STATUS_RECEIVE_GET_PRINT_JOB_INFO_RSP          34
#define PRINTSCU_STATUS_SEND_GET_PRINTER_CONFIG_RQ              35
#define PRINTSCU_STATUS_RECEIVE_GET_PRINTER_CONFIG_RSP          36
#define PRINTSCU_STATUS_SEND_CREATE_PRESENTATION_LUT_RQ         37
#define PRINTSCU_STATUS_RECEIVE_CREATE_PRESENTATION_LUT_RSP     38
#define PRINTSCU_STATUS_SEND_DELETE_PRESENTATION_LUT_RQ         39
#define PRINTSCU_STATUS_RECEIVE_DELETE_PRESENTATION_LUT_RSP     40
#define PRINTSCU_STATUS_SEND_CREATE_OVERLAY_BOX_RQ              41
#define PRINTSCU_STATUS_RECEIVE_CREATE_OVERLAY_BOX_RSP          42
#define PRINTSCU_STATUS_SEND_UPDATE_OVERLAY_BOX_RQ              43
#define PRINTSCU_STATUS_RECEIVE_UPDATE_OVERLAY_BOX_RSP          44
#define PRINTSCU_STATUS_SEND_DELETE_OVERLAY_BOX_RQ              45
#define PRINTSCU_STATUS_RECEIVE_DELETE_OVERLAY_BOX_RSP          46
#define PRINTSCU_STATUS_SEND_CREATE_PULL_PRINT_REQUEST_RQ       47
#define PRINTSCU_STATUS_RECEIVE_CREATE_PULL_PRINT_REQUEST_RSP   48
#define PRINTSCU_STATUS_SEND_PRINT_PULL_PRINT_REQUEST_RQ        49
#define PRINTSCU_STATUS_RECEIVE_PRINT_PULL_PRINT_REQUEST_RSP    50
#define PRINTSCU_STATUS_SEND_DELETE_PULL_PRINT_REQUEST_RQ       51
#define PRINTSCU_STATUS_RECEIVE_DELETE_PULL_PRINT_REQUEST_RSP   52


#define MAX_SIZE   UID_MAX_SIZE + 1


// --- Structures --- //

// Basic Film Session Parameters
typedef struct tagFILMSESSIONPARAMETERS
{
   L_UINT  uStructSize;
   L_INT32 nNumberOfCopies;
   L_PCTSTR pszPrintPriority;
   L_PCTSTR pszMediumType;
   L_PCTSTR pszFilmDestination;
   L_PCTSTR pszFilmSessionLabel;
   L_INT32 nMemoryAllocation;
   L_PCTSTR pszOwnerID;

} FILMSESSIONPARAMETERS, * pFILMSESSIONPARAMETERS;

// Basic Film Box Parameters
typedef struct tagFILMBOXPARAMETERS
{
   L_UINT  uStructSize;
   L_PCTSTR pszImageDisplayFormat;
   L_PCTSTR pszFilmOrientation;
   L_PCTSTR pszFilmSizeID;
   L_PCTSTR pszMagnificationType;
   L_INT32 nMaxDensity;
   L_PCTSTR pszConfigurationInformation;
   L_PCTSTR pszAnnotationDisplayFormatID;
   L_PCTSTR pszSmoothingType;
   L_PCTSTR pszBorderDensity;
   L_PCTSTR pszEmptyImageDensity;
   L_INT32 nMinDensity;
   L_PCTSTR pszTrim;
   L_INT32 nIllumination;
   L_INT32 nReflectedAmbientLight;
   L_PCTSTR pszRequestedResolutionID;

} FILMBOXPARAMETERS, * pFILMBOXPARAMETERS;

// Basic Grayscale/Color Image Box Parameters
typedef struct tagIMAGEBOXPARAMETERS
{
   L_UINT   uStructSize;
   L_INT32  nImagePosition;
   L_PCTSTR  pszPolarity;
   L_PCTSTR  pszMagnificationType;
   L_PCTSTR  pszSmoothingType;
   L_INT32  nMinDensity;
   L_INT32  nMaxDensity;
   L_PCTSTR  pszConfigurationInformation;
   L_DOUBLE dRequestedImageSize;
   L_PCTSTR  pszRequestedDecimateCropBehavior;

} IMAGEBOXPARAMETERS, * pIMAGEBOXPARAMETERS;

// Printer Report Info
typedef struct tagPRINTERREPORTINFO
{
   L_PCTSTR pszPrinterStatusInfo;
   L_PCTSTR pszFilmDestination;
   L_PCTSTR pszPrinterName;

} PRINTERREPORTINFO, * pPRINTERREPORTINFO;

// Printer Info
typedef struct tagPRINTERINFO
{
   L_UINT uStructSize;
   L_TCHAR szPrinterStatus[MAX_SIZE];
   L_TCHAR szPrinterStatusInfo[MAX_SIZE];
   L_TCHAR szPrinterName[MAX_SIZE];
   L_TCHAR szManufacturer[MAX_SIZE];
   L_TCHAR szManufacturerModelName[MAX_SIZE];
   L_TCHAR szDeviceSerialNumber[MAX_SIZE];
   L_TCHAR szSoftwareVersions[MAX_SIZE];
   L_TCHAR szDateOfLastCalibration[MAX_SIZE];
   L_TCHAR szTimeOfLastCalibration[MAX_SIZE];

} PRINTERINFO, * pPRINTERINFO;

// Print Job Report Info
typedef struct tagPRINTJOBREPORTINFO
{
   L_PCTSTR pszExecutionStatusInfo;
   L_PCTSTR pszPrintJobID;
   L_PCTSTR pszFilmSessionLabel;
   L_PCTSTR pszPrinterName;

} PRINTJOBREPORTINFO, * pPRINTJOBREPORTINFO;

// Print Job Info
typedef struct tagPRINTJOBINFO
{
   L_UINT uStructSize;
   L_TCHAR szExecutionStatus[MAX_SIZE];
   L_TCHAR szExecutionStatusInfo[MAX_SIZE];
   L_TCHAR szPrintPriority[MAX_SIZE];
   L_TCHAR szCreationDate[MAX_SIZE];
   L_TCHAR szCreationTime[MAX_SIZE];
   L_TCHAR szPrinterName[MAX_SIZE];
   L_TCHAR szOriginator[MAX_SIZE];

} PRINTJOBINFO, * pPRINTJOBINFO;

// Basic Print Image Overlay Box Parameters
typedef struct tagOVERLAYBOXPARAMETERS
{
   L_UINT  uStructSize;
   L_INT16 nOverlayOriginRow;
   L_INT16 nOverlayOriginColumn;
   L_PCTSTR pszOverlayOrImageMagnification;
   L_INT32 nMagnifyToNumberOfColumns;
   L_PCTSTR pszOverlayMagnificationType;
   L_PCTSTR pszOverlayForegroundDensity;
   L_PCTSTR pszOverlayBackgroundDensity;
   L_PCTSTR pszOverlaySmoothingType;

} OVERLAYBOXPARAMETERS, * pOVERLAYBOXPARAMETERS;

// Stored Print Storage SOP Instance Info
typedef struct tagSTOREDPRINTSTORAGEINSTANCEINFO
{
   L_UINT  uStructSize;
   L_PCTSTR pszRetrieveAETitle;
   L_PCTSTR pszReferencedSOPInstanceUID;
   L_PCTSTR pszStudyInstanceUID;
   L_PCTSTR pszSeriesInstanceUID;
   L_PCTSTR pszPatientID;

} STOREDPRINTSTORAGEINSTANCEINFO, * pSTOREDPRINTSTORAGEINSTANCEINFO;

// Pull Print Request Parameters
typedef struct tagPULLPRINTREQUESTPARAMETERS
{
   L_UINT  uStructSize;
   L_INT32 nNumberOfCopies;
   L_PCTSTR pszPrintPriority;
   L_PCTSTR pszMediumType;
   L_PCTSTR pszFilmDestination;
   L_PCTSTR pszColorImagePrintingFlag;
   L_PCTSTR pszAnnotationFlag;
   L_PCTSTR pszImageOverlayFlag;
   L_PCTSTR pszPresentationLUTFlag;
   L_PCTSTR pszImageBoxPresentationLUTFlag;
   L_PCTSTR pszConfigurationInformation;
   L_PCTSTR pszFilmSessionLabel;
   L_INT32 nMemoryAllocation;
   L_PCTSTR pszCollationFlag;
   L_INT32 nIllumination;
   L_INT32 nReflectedAmbientLight;
   L_PCTSTR pszOwnerID;

} PULLPRINTREQUESTPARAMETERS, * pPULLPRINTREQUESTPARAMETERS;

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomPrintSCU : public LDicomNet
{
public:
   LDicomDS m_PrinterConfiguration;
  
public:
   LDicomPrintSCU(L_TCHAR * pszPath = NULL);
   virtual ~LDicomPrintSCU();

   virtual L_VOID OnStatus(L_UINT16 uStatus, L_UINT16 uOperationStatus);
   L_VOID SetTimeout(L_UINT16 uTimeout);

   // Association Functions
   L_INT  Associate(const L_TCHAR * pszPrintScpIP, L_UINT uPrintScpPort, const L_TCHAR * pszCalledTitle, const L_TCHAR * pszCallingTitle, L_UINT16 uSupportedClasses);
   L_INT  Associate(const L_TCHAR * pszPrintScuIP, L_UINT uPrintScuPort, const L_TCHAR * pszPrintScpIP, L_UINT uPrintScpPort, const L_TCHAR *  pszCalledTitle, const L_TCHAR * pszCallingTitle, L_UINT16 uSupportedClasses);
   L_VOID GetAssociateRejectInfo(L_UCHAR* pnResult, L_UCHAR* pnSource, L_UCHAR* pnReason) const;
   L_BOOL IsClassSupported(L_UINT16 uClass);
   L_INT  Release();
   L_VOID GetAbortInfo(L_UCHAR* pnSource, L_UCHAR* pnReason) const;

   L_UINT16 GetLastOperationStatus() const;

   // Basic Film Session Functions
   L_INT   CreateFilmSession(const pFILMSESSIONPARAMETERS pParameters, L_BOOL bColorPrintManagement = FALSE);
   const   L_TCHAR* GetFilmSessionInstanceUID() const;
   L_INT   UpdateFilmSession(const pFILMSESSIONPARAMETERS pParameters);
   L_INT   PrintFilmSession();
   L_INT   DeleteFilmSession();
   L_INT   GetDefaultFilmSessionParameters(pFILMSESSIONPARAMETERS pParameters, L_UINT uStructSize) const;

   // Basic Film Box Functions
   L_INT   CreateFilmBox(const pFILMBOXPARAMETERS pParameters, const L_TCHAR * pszRefPresLUTInstanceUID = NULL);
   const   L_TCHAR*  GetFilmBoxInstanceUID() const;
   L_INT   UpdateFilmBox(const pFILMBOXPARAMETERS pParameters, const L_TCHAR * pszRefPresLUTInstanceUID = NULL);
   L_INT   PrintFilmBox();
   L_INT   DeleteFilmBox();
   L_INT   GetDefaultFilmBoxParameters(pFILMBOXPARAMETERS pParameters, L_UINT uStructSize) const;

   // Basic Grayscale/Color Image Box Functions
   L_UINT32 GetImageBoxesCount() const;
   L_PCTSTR GetImageBoxInstanceUID(L_UINT32 uIndex) const;
   L_INT    UpdateImageBox(const L_TCHAR * pszImageBoxInstanceUID, LDicomDS* pImage,
                           const pIMAGEBOXPARAMETERS pParameters,
                           const L_TCHAR * pszRefImageOverlayBoxInstanceUID = NULL,
                           const L_TCHAR * pszRefPresLUTInstanceUID = NULL);
   L_INT    GetDefaultImageBoxParameters(pIMAGEBOXPARAMETERS pParameters, L_UINT uStructSize) const;
   L_VOID   FreeImageBoxesInstanceUIDs();

   // Printer Functions
   virtual L_VOID OnPrinterReport(L_UINT16 uEventTypeID, const pPRINTERREPORTINFO pReportInfo);
   L_INT GetPrinterInfo(const pPRINTERINFO pRequiredPrinterInfo, L_BOOL bBasicPrintManagement, L_BOOL bColorPrintManagement);
   L_INT GetPrinterInfo(pPRINTERINFO pPrinterInfo, L_UINT uStructSize) const;

   // Basic Annotation Box Functions
   L_UINT32 GetAnnotationBoxesCount() const;
   L_PCTSTR  GetAnnotationBoxInstanceUID(L_UINT32 uIndex) const;
   L_INT    UpdateAnnotationBox(const L_TCHAR * pszAnnotationBoxInstanceUID, L_UINT16 uAnnotationPosition, const L_TCHAR * pszTextString);
   L_VOID   FreeAnnotationBoxesInstanceUIDs();

   // Presentation LUT Functions
   L_INT   CreatePresentationLUT(LDicomDS* pPresentationLUT, const L_TCHAR * pszPresentationLUTShape = NULL);
   const   L_TCHAR* GetPresentationLUTInstanceUID() const;
   L_INT   DeletePresentationLUT(const L_TCHAR *  pszPresentationLUTInstanceUID);

   // Basic Print Image Overlay Box Functions
   L_INT   CreateOverlayBox(LDicomDS* pOverlay, const pOVERLAYBOXPARAMETERS pParameters);
   const   L_TCHAR* GetOverlayBoxInstanceUID() const;
   L_INT   UpdateOverlayBox(const L_TCHAR * pszOverlayBoxInstanceUID, LDicomDS* pOverlay, const pOVERLAYBOXPARAMETERS pParameters, L_BOOL bUpdateOverlayOrigin);
   L_INT   DeleteOverlayBox(const L_TCHAR * pszOverlayBoxInstanceUID);
   L_INT   GetDefaultOverlayBoxParameters(pOVERLAYBOXPARAMETERS pParameters, L_UINT uStructSize) const;

   // Pull Print Request Functions
   L_INT   CreatePullPrintRequest(const pSTOREDPRINTSTORAGEINSTANCEINFO InstancesInfo,
                                  L_UINT32 uInstancesCount,
                                  const pPULLPRINTREQUESTPARAMETERS pParameters);
   const   L_TCHAR* GetPullPrintRequestInstanceUID() const;
   L_INT   PrintPullPrintRequestSession();
   L_INT   DeletePullPrintRequest();
   L_INT   GetDefaultPullPrintRequestParameters(pPULLPRINTREQUESTPARAMETERS pParameters, L_UINT uStructSize) const;

   // Print Job Functions
   const   L_TCHAR* GetPrintJobInstanceUID() const;
   virtual L_VOID OnPrintJobReport(const L_TCHAR * pszPrintJobInstanceUID, L_UINT16 uEventTypeID, const pPRINTJOBREPORTINFO pReportInfo);
   L_INT GetPrintJobInfo(const L_TCHAR * pszPrintJobInstanceUID, const pPRINTJOBINFO pRequiredPrintJobInfo = NULL);
   L_INT GetPrintJobInfo(pPRINTJOBINFO pPrintJobInfo, L_UINT uStructSize) const;

   // Printer Configuration Retrieval Functions (the retrieved Printer Configuration is stored
   // in m_PrinterConfiguration)
   L_INT  GetPrinterConfiguration();

private:
   HANDLE   m_hOpCompleteEvent;
   L_INT    m_nOperationResult;
   L_UINT16 m_uTimeout; // In seconds

   L_TCHAR  m_szCalledTitle[MAX_SIZE];
   L_TCHAR  m_szCallingTitle[MAX_SIZE];

   L_UINT16 m_uClassesSupportedByScu; // Specified when calling LDicomPrintSCU::Associate

   static struct ClassesInfo
   {
      L_UINT16 uClass;
      L_CHAR*  pszClassUID;

   } m_Classes[];

   struct
   {
      L_UCHAR m_nResult;
      L_UCHAR m_nSource;
      L_UCHAR m_nReason;

   } m_AssociateRejectInfo;

   struct
   {
      L_UCHAR m_nSource;
      L_UCHAR m_nReason;

   } m_AbortInfo;

   L_UINT16 m_uMessageID;
   L_BOOL   m_bColorPrint;
   L_UINT16 m_uOutstandingOperation; // Note: We operate synchronously
   L_UINT16 m_uLastOperationStatus;

   L_TCHAR m_szFilmSessionInstanceUID[MAX_SIZE];
   L_TCHAR m_szFilmBoxInstanceUID[MAX_SIZE];
   L_TCHAR m_szPrintJobInstanceUID[MAX_SIZE];
   L_TCHAR m_szPresentationLUTInstanceUID[MAX_SIZE];
   L_TCHAR m_szOverlayBoxInstanceUID[MAX_SIZE];
   L_TCHAR m_szPullPrintRequestInstanceUID[MAX_SIZE];

   class CInstanceUID
   {
   public:
      L_TCHAR        m_szInstanceUID[MAX_SIZE];
      CInstanceUID* m_pNextInstanceUID;

      CInstanceUID(L_PCTSTR pszInstanceUID = NULL);
      ~CInstanceUID();
   };
   CInstanceUID* m_ImageBoxInstancesUIDs;
   CInstanceUID* m_AnnotationBoxInstancesUIDs;

   PRINTERINFO  m_PrinterInfo;
   PRINTJOBINFO m_PrintJobInfo;

private:
   L_VOID OnConnect(L_INT nError);
   L_VOID OnReceiveAssociateReject(L_UCHAR nResult, L_UCHAR nSource, L_UCHAR nReason);
   L_VOID OnReceiveAssociateAccept(LDicomAssociate* pPDU);
   L_VOID OnReceiveReleaseResponse();
   L_VOID OnReceiveReleaseRequest();
   L_VOID OnReceiveAbort(L_UCHAR nSource, L_UCHAR nReason);
   L_VOID OnReceiveNCreateResponse(L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR* pszClass, L_TCHAR* pszInstance, L_UINT16 nStatus, LDicomDS* pDS);
   L_VOID OnReceiveNSetResponse(L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR* pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus, LDicomDS* pDS);
   L_VOID OnReceiveNActionResponse(L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR* pszClass, L_TCHAR* pszInstance, L_UINT16 nStatus, L_UINT16 nAction, LDicomDS* pDS);
   L_VOID OnReceiveNDeleteResponse(L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR* pszClass, L_TCHAR *pszInstance, L_UINT16 nStatus);
   L_VOID OnReceiveNReportRequest(L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR* pszClass, L_TCHAR* pszInstance, L_UINT16 nEvent, LDicomDS* pDS);
   L_VOID OnReceiveNGetResponse(L_UCHAR nPresentationID, L_UINT16 nMessageID, L_TCHAR* pszClass, L_TCHAR* pszInstance, L_UINT16 nStatus, LDicomDS* pDS);

   L_INT   WaitForOperation();
   L_VOID  Reset();
   L_UCHAR GetPresentationContextID(L_UINT16 uClass);
   L_BOOL  SetAttribute(LDicomDS& List, L_UINT32 uTag, L_PCTSTR pszValue) const;
   L_BOOL  SetAttribute(LDicomDS& List, L_UINT32 uTag, L_INT32 nValue) const;
   L_BOOL  SetAttribute(LDicomDS& List, L_UINT32 uTag, L_DOUBLE dValue) const;
   L_BOOL  InsertReferencedSequence(LDicomDS& DataSet, L_UINT32 uRefSeqTag, const L_TCHAR * pszRefSOPClassUID, const L_TCHAR * pszRefSOPInstanceUID) const;
   L_VOID  StorePrintJobInstanceUID(LDicomDS& ActionReply);
   L_INT   SetImage(LDicomDS& ModificationList, LDicomDS* pImage, L_BOOL bGrayscale) const;
};

#endif // #if defined(__cplusplus)

//============= CLASS ==========================================================
// Context IDs
#define CID_2       L_TEXT("CID 2")
#define CID_4       L_TEXT("CID 4")
#define CID_5       L_TEXT("CID 5")
#define CID_6       L_TEXT("CID 6")
#define CID_7       L_TEXT("CID 7")
#define CID_8       L_TEXT("CID 8")
#define CID_9       L_TEXT("CID 9")
#define CID_10      L_TEXT("CID 10")
#define CID_11      L_TEXT("CID 11")
#define CID_12      L_TEXT("CID 12")
#define CID_18      L_TEXT("CID 18")
#define CID_19      L_TEXT("CID 19")
#define CID_20      L_TEXT("CID 20")
#define CID_21      L_TEXT("CID 21")
#define CID_23      L_TEXT("CID 23")
#define CID_25      L_TEXT("CID 25")
#define CID_26      L_TEXT("CID 26")
#define CID_29      L_TEXT("CID 29")
#define CID_30      L_TEXT("CID 30")
#define CID_42      L_TEXT("CID 42")
#define CID_82      L_TEXT("CID 82")
#define CID_244     L_TEXT("CID 244")
#define CID_3000    L_TEXT("CID 3000")
#define CID_3001    L_TEXT("CID 3001")
#define CID_3003    L_TEXT("CID 3003")
#define CID_3010    L_TEXT("CID 3010")
#define CID_3011    L_TEXT("CID 3011")
#define CID_3014    L_TEXT("CID 3014")
#define CID_3015    L_TEXT("CID 3015")
#define CID_3019    L_TEXT("CID 3019")
#define CID_3082    L_TEXT("CID 3082")
#define CID_3090    L_TEXT("CID 3090")
#define CID_3101    L_TEXT("CID 3101")
#define CID_3240    L_TEXT("CID 3240")
#define CID_3241    L_TEXT("CID 3241")
#define CID_3250    L_TEXT("CID 3250")
#define CID_3254    L_TEXT("CID 3254")
#define CID_3261    L_TEXT("CID 3261")
#define CID_3262    L_TEXT("CID 3262")
#define CID_3263    L_TEXT("CID 3263")
#define CID_3264    L_TEXT("CID 3264")
#define CID_3271    L_TEXT("CID 3271")
#define CID_3335    L_TEXT("CID 3335")
#define CID_3337    L_TEXT("CID 3337")
#define CID_3339    L_TEXT("CID 3339")
#define CID_4009    L_TEXT("CID 4009")
#define CID_4010    L_TEXT("CID 4010")
#define CID_4011    L_TEXT("CID 4011")
#define CID_4012    L_TEXT("CID 4012")
#define CID_4013    L_TEXT("CID 4013")
#define CID_4014    L_TEXT("CID 4014")
#define CID_4015    L_TEXT("CID 4015")
#define CID_4016    L_TEXT("CID 4016")
#define CID_4017    L_TEXT("CID 4017")
#define CID_4018    L_TEXT("CID 4018")
#define CID_4019    L_TEXT("CID 4019")
#define CID_4020    L_TEXT("CID 4020")
#define CID_4021    L_TEXT("CID 4021")
#define CID_4030    L_TEXT("CID 4030")
#define CID_4031    L_TEXT("CID 4031")
#define CID_4200    L_TEXT("CID 4200")
#define CID_4201    L_TEXT("CID 4201")
#define CID_4202    L_TEXT("CID 4202")
#define CID_4203    L_TEXT("CID 4203")
#define CID_4204    L_TEXT("CID 4204")
#define CID_4205    L_TEXT("CID 4205")
#define CID_4206    L_TEXT("CID 4206")
#define CID_4207    L_TEXT("CID 4207")
#define CID_4208    L_TEXT("CID 4208")
#define CID_4209    L_TEXT("CID 4209")
#define CID_5000    L_TEXT("CID 5000")
#define CID_5001    L_TEXT("CID 5001")
#define CID_6000    L_TEXT("CID 6000")
#define CID_6001    L_TEXT("CID 6001")
#define CID_6002    L_TEXT("CID 6002")
#define CID_6003    L_TEXT("CID 6003")
#define CID_6004    L_TEXT("CID 6004")
#define CID_6005    L_TEXT("CID 6005")
#define CID_6006    L_TEXT("CID 6006")
#define CID_6007    L_TEXT("CID 6007")
#define CID_6008    L_TEXT("CID 6008")
#define CID_6009    L_TEXT("CID 6009")
#define CID_6010    L_TEXT("CID 6010")
#define CID_6011    L_TEXT("CID 6011")
#define CID_6012    L_TEXT("CID 6012")
#define CID_6013    L_TEXT("CID 6013")
#define CID_6014    L_TEXT("CID 6014")
#define CID_6015    L_TEXT("CID 6015")
#define CID_6016    L_TEXT("CID 6016")
#define CID_6017    L_TEXT("CID 6017")
#define CID_6018    L_TEXT("CID 6018")
#define CID_6019    L_TEXT("CID 6019")
#define CID_6020    L_TEXT("CID 6020")
#define CID_6021    L_TEXT("CID 6021")
#define CID_6022    L_TEXT("CID 6022")
#define CID_6023    L_TEXT("CID 6023")
#define CID_6024    L_TEXT("CID 6024")
#define CID_6025    L_TEXT("CID 6025")
#define CID_6026    L_TEXT("CID 6026")
#define CID_6027    L_TEXT("CID 6027")
#define CID_6028    L_TEXT("CID 6028")
#define CID_6029    L_TEXT("CID 6029")
#define CID_6030    L_TEXT("CID 6030")
#define CID_6031    L_TEXT("CID 6031")
#define CID_6032    L_TEXT("CID 6032")
#define CID_6033    L_TEXT("CID 6033")
#define CID_6034    L_TEXT("CID 6034")
#define CID_6035    L_TEXT("CID 6035")
#define CID_6036    L_TEXT("CID 6036")
#define CID_6037    L_TEXT("CID 6037")
#define CID_6038    L_TEXT("CID 6038")
#define CID_6039    L_TEXT("CID 6039")
#define CID_6040    L_TEXT("CID 6040")
#define CID_6041    L_TEXT("CID 6041")
#define CID_6042    L_TEXT("CID 6042")
#define CID_6043    L_TEXT("CID 6043")
#define CID_6044    L_TEXT("CID 6044")
#define CID_6045    L_TEXT("CID 6045")
#define CID_6046    L_TEXT("CID 6046")
#define CID_6047    L_TEXT("CID 6047")
#define CID_6100    L_TEXT("CID 6100")
#define CID_6101    L_TEXT("CID 6101")
#define CID_6102    L_TEXT("CID 6102")
#define CID_6103    L_TEXT("CID 6103")
#define CID_6104    L_TEXT("CID 6104")
#define CID_6105    L_TEXT("CID 6105")
#define CID_6106    L_TEXT("CID 6106")
#define CID_6107    L_TEXT("CID 6107")
#define CID_6108    L_TEXT("CID 6108")
#define CID_6109    L_TEXT("CID 6109")
#define CID_6110    L_TEXT("CID 6110")
#define CID_6111    L_TEXT("CID 6111")
#define CID_6112    L_TEXT("CID 6112")
#define CID_6113    L_TEXT("CID 6113")
#define CID_6114    L_TEXT("CID 6114")
#define CID_6115    L_TEXT("CID 6115")
#define CID_6116    L_TEXT("CID 6116")
#define CID_6117    L_TEXT("CID 6117")
#define CID_6118    L_TEXT("CID 6118")
#define CID_6119    L_TEXT("CID 6119")
#define CID_6120    L_TEXT("CID 6120")
#define CID_6121    L_TEXT("CID 6121")
#define CID_6122    L_TEXT("CID 6122")
#define CID_6123    L_TEXT("CID 6123")
#define CID_6124    L_TEXT("CID 6124")
#define CID_6125    L_TEXT("CID 6125")
#define CID_6126    L_TEXT("CID 6126")
#define CID_6127    L_TEXT("CID 6127")
#define CID_6128    L_TEXT("CID 6128")
#define CID_6129    L_TEXT("CID 6129")
#define CID_6130    L_TEXT("CID 6130")
#define CID_6131    L_TEXT("CID 6131")
#define CID_6132    L_TEXT("CID 6132")
#define CID_6133    L_TEXT("CID 6133")
#define CID_6134    L_TEXT("CID 6134")
#define CID_6135    L_TEXT("CID 6135")
#define CID_6136    L_TEXT("CID 6136")
#define CID_6137    L_TEXT("CID 6137")
#define CID_6138    L_TEXT("CID 6138")
#define CID_6139    L_TEXT("CID 6139")
#define CID_6140    L_TEXT("CID 6140")
#define CID_6141    L_TEXT("CID 6141")
#define CID_6142    L_TEXT("CID 6142")
#define CID_6143    L_TEXT("CID 6143")
#define CID_6144    L_TEXT("CID 6144")
#define CID_6145    L_TEXT("CID 6145")
#define CID_7000    L_TEXT("CID 7000")
#define CID_7001    L_TEXT("CID 7001")
#define CID_7002    L_TEXT("CID 7002")
#define CID_7003    L_TEXT("CID 7003")
#define CID_7004    L_TEXT("CID 7004")
#define CID_7005    L_TEXT("CID 7005")
#define CID_7010    L_TEXT("CID 7010")
#define CID_7011    L_TEXT("CID 7011")
#define CID_7012    L_TEXT("CID 7012")
#define CID_7201    L_TEXT("CID 7201")
#define CID_7202    L_TEXT("CID 7202")
#define CID_7203    L_TEXT("CID 7203")
#define CID_7210    L_TEXT("CID 7210")
#define CID_7452    L_TEXT("CID 7452")
#define CID_7453    L_TEXT("CID 7453")
#define CID_7454    L_TEXT("CID 7454")
#define CID_7455    L_TEXT("CID 7455")
#define CID_7456    L_TEXT("CID 7456")
#define CID_7460    L_TEXT("CID 7460")
#define CID_7461    L_TEXT("CID 7461")
#define CID_7462    L_TEXT("CID 7462")
#define CID_7470    L_TEXT("CID 7470")
#define CID_7471    L_TEXT("CID 7471")
#define CID_7472    L_TEXT("CID 7472")
#define CID_9231    L_TEXT("CID 9231")
#define CID_9232    L_TEXT("CID 9232")
#define CID_9300    L_TEXT("CID 9300")
#define CID_12001   L_TEXT("CID 12001")
#define CID_12002   L_TEXT("CID 12002")
#define CID_12140   L_TEXT("CID 12140")
#define CID_12141   L_TEXT("CID 12141")

// Context Group
typedef struct tagDICOMCONTEXTGROUP
{
   GENERICLINK

   L_TCHAR*       pszContextIdentifier;
   L_TCHAR*       pszName;
   L_BOOL         bExtensible;
   pVALUEDATETIME pContextGroupVersion;

} DICOMCONTEXTGROUP, * pDICOMCONTEXTGROUP;

// Coded Concept
typedef struct tagDICOMCODEDCONCEPT
{
   GENERICLINK

   L_TCHAR*        pszCodingSchemeDesignator;
   L_TCHAR*        pszCodingSchemeVersion;
   L_TCHAR*        pszCodeValue;
   L_TCHAR*        pszCodeMeaning;

   pVALUEDATETIME pContextGroupLocalVersion;
   L_TCHAR*        pszContextGroupExtensionCreatorUID;

} DICOMCODEDCONCEPT, * pDICOMCODEDCONCEPT;

#define DICOM_CONTEXTGROUP_DISALLOW_DUPLICATES   0x01

#if defined(__cplusplus)

class L_LTDIC_CLASS LDicomContextGroup
{
public:   
   // Context Groups

   static L_UINT16           Load(L_TCHAR* pszContextID = NULL);
   static L_VOID             Reset();

   static pDICOMCONTEXTGROUP GetFirst();
   static pDICOMCONTEXTGROUP GetLast();
   static pDICOMCONTEXTGROUP GetNext(pDICOMCONTEXTGROUP pContextGroup);
   static pDICOMCONTEXTGROUP GetPrev(pDICOMCONTEXTGROUP pContextGroup);

   static L_UINT32           GetCount();
   static pDICOMCONTEXTGROUP Find(L_TCHAR* pszContextID);
   static pDICOMCONTEXTGROUP FindIndex(L_UINT32 uIndex);

   static pDICOMCONTEXTGROUP Insert(L_TCHAR* pszContextIdentifier,
                                    L_TCHAR* pszName,
                                    L_BOOL bExtensible,
                                    pVALUEDATETIME pContextGroupVersion,
                                    L_UINT16 uFlags = 0);
   static pDICOMCONTEXTGROUP Delete(pDICOMCONTEXTGROUP pContextGroup);
   static L_BOOL             Default(pDICOMCONTEXTGROUP pContextGroup);
   static L_BOOL             Exists(pDICOMCONTEXTGROUP pContextGroup);

   // Coded Concepts

   static pDICOMCODEDCONCEPT GetFirstCodedConcept(pDICOMCONTEXTGROUP pContextGroup);
   static pDICOMCODEDCONCEPT GetLastCodedConcept(pDICOMCONTEXTGROUP pContextGroup);
   static pDICOMCODEDCONCEPT GetNextCodedConcept(pDICOMCODEDCONCEPT pCodedConcept);
   static pDICOMCODEDCONCEPT GetPrevCodedConcept(pDICOMCODEDCONCEPT pCodedConcept);
   static pDICOMCONTEXTGROUP GetContextGroup(pDICOMCODEDCONCEPT pCodedConcept);

   static L_UINT32           GetCountCodedConcept(pDICOMCONTEXTGROUP pContextGroup);
   static pDICOMCODEDCONCEPT FindCodedConcept(pDICOMCONTEXTGROUP pContextGroup,
                                              L_TCHAR* pszCodingSchemeDesignator,
                                              L_TCHAR* pszCodeValue);
   static pDICOMCODEDCONCEPT FindIndexCodedConcept(pDICOMCONTEXTGROUP pContextGroup,
                                                   L_UINT32 uIndex);

   static L_BOOL             SetCodeMeaning(pDICOMCODEDCONCEPT pCodedConcept,
                                            L_TCHAR* pszCodeMeaning);
   static pDICOMCODEDCONCEPT InsertCodedConcept(pDICOMCONTEXTGROUP pContextGroup,
                                                L_TCHAR* pszCodingSchemeDesignator,
                                                L_TCHAR* pszCodingSchemeVersion,
                                                L_TCHAR* pszCodeValue,
                                                L_TCHAR* pszCodeMeaning,
                                                pVALUEDATETIME pContextGroupLocalVersion = NULL,
                                                L_TCHAR* pszContextGroupExtensionCreatorUID = NULL,
                                                L_UINT16 uFlags = 0);
   static pDICOMCODEDCONCEPT DeleteCodedConcept(pDICOMCODEDCONCEPT pCodedConcept);
   static L_BOOL             ExistsCodedConcept(pDICOMCODEDCONCEPT pCodedConcept);

private:
   static LDicomTreeEx m_ContextGroupsTable;

   static L_BOOL LoadCodedConcepts(L_UINT32 uContextGroupIndex,
                                   pDICOMCONTEXTGROUP pContextGroup = NULL,
                                   L_BOOL bDeleteIfFailed = TRUE);

#if defined(LEADTOOLS_V16_OR_LATER)
   static L_BOOL m_bInitialized;
#endif
public:
   static L_VOID SetInitialized(L_BOOL bValue);
};

#endif // #if defined(__cplusplus)

#if defined(__cplusplus)
extern "C"
{
#endif // #if defined(__cplusplus)

   // Context Groups

   L_LTDIC_API L_UINT16           EXT_FUNCTION L_DicomLoadContextGroup(L_TCHAR* pszContextID);
   L_LTDIC_API L_VOID             EXT_FUNCTION L_DicomResetContextGroup();

   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomGetFirstContextGroup();
   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomGetLastContextGroup();
   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomGetNextContextGroup(pDICOMCONTEXTGROUP pContextGroup);
   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomGetPrevContextGroup(pDICOMCONTEXTGROUP pContextGroup);

   L_LTDIC_API L_UINT32           EXT_FUNCTION L_DicomGetCountContextGroup();
   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomFindContextGroup(L_TCHAR* pszContextID);
   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomFindIndexContextGroup(L_UINT32 uIndex);

   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomInsertContextGroup(L_TCHAR* pszContextIdentifier, L_TCHAR* pszName, L_BOOL bExtensible, pVALUEDATETIME pContextGroupVersion,L_UINT16 uFlags);
   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomDeleteContextGroup(pDICOMCONTEXTGROUP pContextGroup);
   L_LTDIC_API L_BOOL             EXT_FUNCTION L_DicomDefaultContextGroup(pDICOMCONTEXTGROUP pContextGroup);
   L_LTDIC_API L_BOOL             EXT_FUNCTION L_DicomExistsContextGroup(pDICOMCONTEXTGROUP pContextGroup);

   // Coded Concepts

   L_LTDIC_API pDICOMCODEDCONCEPT EXT_FUNCTION L_DicomGetFirstCodedConcept(pDICOMCONTEXTGROUP pContextGroup);
   L_LTDIC_API pDICOMCODEDCONCEPT EXT_FUNCTION L_DicomGetLastCodedConcept(pDICOMCONTEXTGROUP pContextGroup);
   L_LTDIC_API pDICOMCODEDCONCEPT EXT_FUNCTION L_DicomGetNextCodedConcept(pDICOMCODEDCONCEPT pCodedConcept);
   L_LTDIC_API pDICOMCODEDCONCEPT EXT_FUNCTION L_DicomGetPrevCodedConcept(pDICOMCODEDCONCEPT pCodedConcept);
   L_LTDIC_API pDICOMCONTEXTGROUP EXT_FUNCTION L_DicomGetCodedConceptGroup(pDICOMCODEDCONCEPT pCodedConcept);

   L_LTDIC_API L_UINT32           EXT_FUNCTION L_DicomGetCountCodedConcept(pDICOMCONTEXTGROUP pContextGroup);
   L_LTDIC_API pDICOMCODEDCONCEPT EXT_FUNCTION L_DicomFindCodedConcept(pDICOMCONTEXTGROUP pContextGroup, L_TCHAR* pszCodingSchemeDesignator, L_TCHAR* pszCodeValue);
   L_LTDIC_API pDICOMCODEDCONCEPT EXT_FUNCTION L_DicomFindIndexCodedConcept(pDICOMCONTEXTGROUP pContextGroup, L_UINT32 uIndex);

   L_LTDIC_API L_BOOL             EXT_FUNCTION L_DicomSetCodedConceptCodeMeaning(pDICOMCODEDCONCEPT pCodedConcept, L_TCHAR* pszCodeMeaning);
   L_LTDIC_API pDICOMCODEDCONCEPT EXT_FUNCTION L_DicomInsertCodedConcept(pDICOMCONTEXTGROUP pContextGroup, L_TCHAR* pszCodingSchemeDesignator, L_TCHAR* pszCodingSchemeVersion, L_TCHAR* pszCodeValue, L_TCHAR* pszCodeMeaning, pVALUEDATETIME pContextGroupLocalVersion, L_TCHAR* pszContextGroupExtensionCreatorUID,L_UINT16 uFlags);
   L_LTDIC_API pDICOMCODEDCONCEPT EXT_FUNCTION L_DicomDeleteCodedConcept(pDICOMCODEDCONCEPT pCodedConcept);
   L_LTDIC_API L_BOOL             EXT_FUNCTION L_DicomExistsCodedConcept(pDICOMCODEDCONCEPT pCodedConcept);

#if defined(__cplusplus)
};
#endif // #if defined(__cplusplus)

#endif // #if !defined(FOR_WINCE)

#if defined(LEADTOOLS_V16_OR_LATER)
#pragma pack()
#endif // #if defined(LEADTOOLS_V16_OR_LATER)

#endif // #if !defined(LTDIC_H)
