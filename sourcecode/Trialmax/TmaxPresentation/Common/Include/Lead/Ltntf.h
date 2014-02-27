//
//   LTNTF.H - LEAD National Imagery Transmission Format header file
//   Copyright (c) 1991-2009 LEAD Technologies, Inc.
//   All Rights Reserved.

#if !defined (LTNTF_H)
#define LTNTF_H

#if !defined(L_LTNTF_API)
   #define L_LTNTF_API
#endif // #if !defined(L_LTNTF_API)

#include "Lttyp.h"
#include "ltkrn.h"
#include "lvkrn.h"

#define L_HEADER_ENTRY
#include "ltpck.h"


typedef HANDLE HNITF;
typedef HANDLE * pHNITF;

// enums and defines
////////////////////////////////////////////////////
#define NITF_FILE_EMPTY   0x0001
#define NITF_FILE_VALID   0x0002


// structures
////////////////////////////////////////////////////
typedef struct tagNITFDATE
{
   L_UCHAR   CC[2];
   L_UCHAR   YY[2];
   L_UCHAR   MM[2];
   L_UCHAR   DD[2];
}  NITFDATE, * pNITFDATE;

typedef struct tagNITFTIME
{
   L_UCHAR   hh[2];
   L_UCHAR   mm[2];
   L_UCHAR   ss[2];
}  NITFTIME, * pNITFTIME;

typedef struct tagNITFDATETIME
{
   NITFDATE    Date;
   NITFTIME	Time;
}  NITFDATETIME, * pNITFDATETIME;
 
typedef struct tagNITFHEADER
{
   L_CHAR      *     pFHDR;
   L_UINT            uCLEVEL;
   L_CHAR      *     pSTYPE;
   L_CHAR      *     pOSTAID;
   L_CHAR      *     pFTITLE;
   L_CHAR      *     pFSCLAS;
   L_CHAR      *     pFSCLSY;
   L_CHAR      *     pFSCODE;
   L_CHAR      *     pFSCTLH;
   L_CHAR      *     pFSREL;
   L_CHAR      *     pFSDCTP;
   L_CHAR      *     pFSDCXM;
   L_CHAR      *     pFSDG;
   L_CHAR      *     pFSCLTX;
   L_CHAR      *     pFSCATP;
   L_CHAR      *     pFSCAUT;
   L_CHAR      *     pFSCRSN;
   L_CHAR      *     pFSCTLN;
   L_CHAR      *     pONAME;
   L_CHAR      *     pOPHONE;

   L_UINT            uHL;
   L_UINT            uFL;
   L_UINT            uFSCOP;
   L_UINT            uFSCPYS;
   L_UINT            uENCRYP;
   
   L_UINT            uNUMI;
   L_UINT      *     puLISH;
   L_UINT      *     puLI;

   L_UINT            uNUMS;
   L_UINT      *     puLSSH;
   L_UINT      *     puLS;
   
   L_UINT            uNUMX;

   L_UINT            uNUMT;
   L_UINT      *     puLTSH;
   L_UINT      *     puLT;

   L_UINT            uNUMDES;
   L_UINT      *     puLDSH;
   L_UINT      *     puLD;

   L_UINT            uNUMRES;
   L_UINT      *     puLRESH;
   L_UINT      *     puLRE;
   L_UINT            uUDHDL;
   L_UINT            uUDHOFL;
   L_UCHAR     *     pUDHD;

   L_UINT            uXHDL;
   L_UINT            uXHDLOFL;

   L_UCHAR     *     pXHD;

   COLORREF          FBKGC;

   NITFDATE          FSSRDT;
   NITFDATE          FSDGDT;
   NITFDATE          FSDCDT;
   NITFDATETIME      FDT;

   L_FLOAT           fFVER;

}  NITFHEADER, *pNITFHEADER;

typedef struct tagGRAPHICHEADER
{
   L_CHAR   *  pSY; 
   L_CHAR   *  pSID;
   L_CHAR   *  pSNAME;  
   L_CHAR   *  pSSCLAS; 
   L_CHAR   *  pSSCLSY; 
   L_CHAR   *  pSSCODE; 
   L_CHAR   *  pSSCTLH; 
   L_CHAR   *  pSSREL;  
   L_CHAR   *  pSSDCTP; 

   NITFDATE    SSDCDT;  

   L_CHAR   *  pSSDCXM; 
   L_CHAR   *  pSSDG;   

   NITFDATE    SSDGDT;
   L_CHAR   *  pSSCLTX;  
   L_CHAR   *  pSSCATP; 
   L_CHAR   *  pSSCAUT;
   L_CHAR   *  pSSCRSN; 
   NITFDATE    SSSRDT;

   L_CHAR   *  pSSCTLN;   
   L_UINT      uENCRYP; 
   
   L_CHAR   *  pSFMT; 
   L_UINT      uSSTRUCT;
   L_UINT      uSDLVL;
   L_UINT      uSALVL;

   L_INT       nSLOCRow;
   L_INT       nSLOCCol;
   L_INT       nSBND1Row;
   L_INT       nSBND1Col;
   
   L_CHAR   *  pSCOLOR;
   L_INT       nSBND2Row;
   L_INT       nSBND2Col;

   L_UINT      uSRES2;

   L_UINT      uSXSHDL;
   L_UINT      uSXSOFL;
   L_UCHAR   *  pSXSHD;
}  GRAPHICHEADER, *pGRAPHICHEADER;

typedef struct tagIMAGEHEADER
{
   L_CHAR   *     pIM;
   L_CHAR   *     pIID1;

   NITFDATETIME  IDATIM;

   L_CHAR   *  pTGTID;
   L_CHAR   *  pIID2;
   L_CHAR   *  pISCLAS;
   L_CHAR   *  pISCLSY;
   L_CHAR   *  pISCODE;
   L_CHAR   *  pISCTLH;
   L_CHAR   *  pISREL;
   L_CHAR   *  pISDCTP;

   NITFDATE    ISDCDT;

   L_CHAR   *  pISDCXM;
   L_CHAR   *  pISDG;

   NITFDATE    ISDGDT;

   L_CHAR   *  pISCLTX;
   L_CHAR   *  pISCATP;
   L_CHAR   *  pISCAUT;
   L_CHAR   *  pISCRSN;

   NITFDATE    ISSRDT;

   L_CHAR   *  pISCTLN;

   L_UINT      uENCRYP;

   L_CHAR   *  pISORCE;

   L_UINT      uNROWS;
   L_UINT      uNCOLS;

   L_CHAR   *  pPVTYPE;
   L_CHAR   *  pIREP;
   L_CHAR   *  pICAT;

   L_UINT      uABPP;
   L_CHAR   *  pPJUST;
   L_CHAR   *  pICORDS;
   L_CHAR   *  pIGEOLO;

   L_UINT      uNICOM;
   L_CHAR   ** pICOM;

   L_CHAR   *  pIC;
   L_CHAR   *  pCOMRAT;

   L_UINT      uNBANDS;
   L_UINT      uXBANDS;

   L_CHAR   **  pIREPBAND;
   L_CHAR   **  pISUBCAT;
   L_CHAR   **  pIFCN;
   L_CHAR   **  pIMFLTN;

   L_UINT       uISYNC;

   L_CHAR   *  pIMODE;
   L_UINT      uNBPR;
   L_UINT      uNBPC;
   L_UINT      uNPPBH;
   L_UINT      uNPPBV;
   L_UINT      uNBPP;
   L_UINT      uIDLVL;
   L_UINT      uIALVL;
   L_INT       nILOCRow;
   L_INT       nILOCCol;
   L_CHAR   *  pIMAG;

   L_UINT      uUDIDL;
   L_UINT      uUDOFL;
   L_UCHAR *   pUDID;

   L_UINT      uIXSHDL;
   L_UINT      uIXSOFL;
   L_UCHAR  *  pIXSHD;
   L_UINT      uIMDATOFF;
   L_UINT16    uBMRLNTH;
   L_UINT16    uTMRLNTH;

   L_UINT16    uTPXCDLNTH;
   L_UCHAR   * pTPXCD;

}  IMAGEHEADER, *pIMAGEHEADER;

typedef struct tagNITFTXTHEADER 
{
   L_CHAR   *  pTE;
   L_CHAR   *  pTEXTID;

   L_UINT       uTXTALVL;

   NITFDATETIME    TXTDT;

   L_CHAR   *  pTXTITL; 
   L_CHAR   *  pTSCLAS;
   L_CHAR   *  pTSCLSY;
   L_CHAR   *  pTSCODE;
   L_CHAR   *  pTSCTLH;
   L_CHAR   *  pTSREL;
   L_CHAR   *  pTSDCTP;

   NITFDATE    TSDCDT;
   L_CHAR   *  pTSDCXM;
   L_CHAR   *  pTSDG;
   NITFDATE    TSDGDT;
   L_CHAR   *  pTSCLTX;
   L_CHAR   *  pTSCATP;
   L_CHAR   *  pTSCAUT;
   L_CHAR   *  pTSCRSN;

   NITFDATE    TSSRDT;
   L_CHAR   *  pTSCTLN;

   L_UINT      uENCRYP;
   L_CHAR   *  pTXTFMT;

   L_UINT      uTXSHDL;
   L_UINT      uTXSOFL;
   L_UCHAR   *  pTXSHD;
}  TXTHEADER, * pTXTHEADER;

/****************************************************************
   Function prototypes
****************************************************************/

L_LTNTF_API L_INT EXT_FUNCTION L_NITFCreateA(pHNITF phNitf, L_CHAR * pszFileName);

#if defined(FOR_UNICODE)
L_LTNTF_API L_INT EXT_FUNCTION L_NITFCreate(pHNITF phNitf, L_TCHAR * pszFileName);
#else
#define L_NITFCreate L_NITFCreateA
#endif // #if defined(FOR_UNICODE)

L_LTNTF_API L_INT EXT_FUNCTION L_NITFDestroy(pHNITF phNitf);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetStatus (HNITF hNitf);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFSaveFileA(HNITF hNitf, L_CHAR * pszFileName);
#if defined(FOR_UNICODE)
L_LTNTF_API L_INT EXT_FUNCTION L_NITFSaveFile(HNITF hNitf, L_TCHAR * pszFileName);
#else
#define L_NITFSaveFile L_NITFSaveFileA
#endif // #if defined(FOR_UNICODE)

L_LTNTF_API L_INT EXT_FUNCTION L_NITFAppendImageSegment(HNITF hNitf, pBITMAPHANDLE pBitmap, L_INT nFormat, L_INT nBpp, L_INT nQFactor);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFAppendGraphicSegment(HNITF hNitf, pVECTORHANDLE pVector, LPRECT prcVecBounds);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFAppendTextSegmentA(HNITF hNitf, L_CHAR* pszFileName);
#if defined(FOR_UNICODE)
L_LTNTF_API L_INT EXT_FUNCTION L_NITFAppendTextSegment(HNITF hNitf, L_TCHAR* pszFileName);
#else
#define L_NITFAppendTextSegment L_NITFAppendTextSegmentA
#endif // #if defined(FOR_UNICODE)

L_LTNTF_API L_INT EXT_FUNCTION L_NITFSetVectorHandle(HNITF hNitf, L_UINT32 uIndex, pVECTORHANDLE pVector);
L_LTNTF_API pVECTORHANDLE EXT_FUNCTION L_NITFGetVectorHandle(HNITF hNitf, L_UINT32 uIndex);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetNITFHeader (HNITF hNitf, pNITFHEADER pNITFHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFSetNITFHeader (HNITF hNitf, pNITFHEADER pNITFHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetGraphicHeaderCount(HNITF hNitf);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetGraphicHeader (HNITF hNitf, L_UINT uIndex, pGRAPHICHEADER pGraphicHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFSetGraphicHeader (HNITF hNitf, L_UINT uIndex, pGRAPHICHEADER pGraphicsHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetImageHeaderCount(HNITF hNitf);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetImageHeader (HNITF hNitf, L_UINT uIndex, pIMAGEHEADER pImageHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFSetImageHeader (HNITF hNitf, L_UINT uIndex, pIMAGEHEADER  pImageHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetTextHeaderCount(HNITF hNitf);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetTextHeader (HNITF hNitf, L_UINT uIndex, pTXTHEADER pTxtHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFSetTextHeader (HNITF hNitf, L_UINT uIndex, pTXTHEADER pTxtHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFGetTextSegment (HNITF hNitf, L_UINT uIndex, L_CHAR* pTextBuffer, L_UINT *puBufferSize);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFFreeNITFHeader (pNITFHEADER pNITFHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFFreeGraphicHeader (pGRAPHICHEADER pGraphicHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFFreeImageHeader(pIMAGEHEADER pImageHeader);
L_LTNTF_API L_INT EXT_FUNCTION L_NITFFreeTextHeader(pTXTHEADER pTxtHeader);

#undef L_HEADER_ENTRY
#include "ltpck.h"

#endif //LTNTF_H
