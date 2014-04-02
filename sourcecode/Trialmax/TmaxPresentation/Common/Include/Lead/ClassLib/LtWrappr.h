/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME :                                                                 |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/
#ifndef  _LEAD_WRAPPER_H_
#define  _LEAD_WRAPPER_H_

/*---------------------------------------------------------------+
| LEAD WRAPPER CLASSES PROTOTYPES                                |
+---------------------------------------------------------------*/
class LBitmapDictionary;
class LDictionary;
class LBase;
class LBitmapBase;
class LInet;
class LDraw;
class LAnnToolBar;
class LInetServer;
class LPaint;
class LAnnotation;
class LInetClient;
class LPaintEffect;
class LAnnAutomation;
class LScanner;
#if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)
class LWia;
#endif // #if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)
class LAnnContainer;
class LBitmap;
class LTwain;
class LAnnText;
class LBitmapWindow;
class LAnnStamp;
class LAnimationWindow;
class LBaseFile;
class LAnnLine;
class LAnnotationWindow;
class LDialogBase;
class LDialogImage;
class LDialogColor;
class LDialogWeb;
class LDialogImageEffect;
class LDialogEffect;
class LDialogDocument;
class LDialogFile;
class LFile;
class LAnnPolyline;
class LMemoryFile;
class LAnnPolygon;
class LSettings;
class LAnnFreeHand;
class LBitmapSettings;
class LAnnRectangle;
class LFileSettings;
class LAnnEllipse;
class LPrint;
class LAnnPointer;
class LAnnButton;
class LAnnTextPointer;
class LAnnEncrypt;
class LAnnRTF;
class LAnnCurve;
class LAnnCurveClosed;
class LAnnPolyRuler;
class LBitmapRgn;
class LAnnHilite;
class LPlayBack;
class LAnnRedact;
class LChange;
class LAnnAudioClip;
class LBuffer;
class LDoubleBuffer;
class LAnnHotSpot;
class LScreenCapture;
class LAnnRuler;
class LBitmapList;
class LAnnNote;
class LImageListControl;
class LVectorBase; 
class LVectorDialog;
class LVectorFile;
class LVectorMemoryFile;
class LVectorLayer;
class LVectorObject;
class LVectorGroup;
class LVectorWindow;
class LBarCode;
class LMarker;
class LToolbar;
class LContainer;
class LAutomation;

class LInetHttp;
class LInetFtp;

class LRasterPaint;
class LRasterDialog;
class LRasterPaintWindow;

class LSegment;
class LOptimize;
class LPDFCompressor;
class LImageViewer;
class LICCProfile;
class LColor;
class LNITFFile;
class LCritSecLock;

/*----------------------------------------------------------------------------+
| INCLUDES                                                                    |
+----------------------------------------------------------------------------*/
//--STANDARD INCLUDE FILES-----------------------------------------------------
#ifndef STRICT
   #define STRICT
#endif

#if _WIN32_WINNT < 0x0400
#define _WIN32_WINNT 0x0400
#endif

#include <windows.h>

typedef COLORREF                       L_COLOR;

//--LEAD INCLUDE FILES---------------------------------------------------------
//Comment this line to compile and link the LEAD Class Library
//directly with LEAD C DLL's
#define USE_POINTERS_TO_LEAD_FUNCTIONS

#ifdef USE_POINTERS_TO_LEAD_FUNCTIONS

   #include "..\ltsys.h"
   #include "..\lttyp.h"
   #include "..\lterr.h"
   #include "..\ltkrn.h"
   #include "..\ltdis.h"
   #include "..\ltimg.h"
   #include "..\lvkrn.h"
   #include "..\ltfil.h"
   #include "..\lttwn.h"
#if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)
   #include "..\ltwia.h"
#endif // #if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)
   #include "..\ltlck.h"
   #include "..\ltkey.h"
   #include "..\ltann.h"
   #include "..\ltefx.h"
   #include "..\ltscr.h"
   #include "..\ltnet.h"
   #include "..\ltdlg.h"
   #include "..\lttmb.h"
   #include "..\ltlst.h"
   #include "..\lvkrn.h"
   #include "..\lvdlg.h"
   #include "..\ltbar.h"

   #include "..\ltTlb.h"
   #include "..\ltCon.h"
   #include "..\ltPnt.h"
   #include "..\ltAut.h"
   #include "..\ltWeb.h"
   #include "..\Ltsgm.h"
   #include "..\Ltzmv.h"
   #include "..\ltimgopt.h"
   #include "..\lpdfComp.h"
   #include "..\LtNtf.h"
   #include "..\ltivw.h"


   #include "..\ltPdg.h"


#else // ! USE_POINTERS_TO_LEAD_FUNCTIONS
   #include "..\..\include\l_bitmap.h"
   #include "..\..\include\ltIsi.h"
   #include "..\..\include\ltScr.h"
   #include "..\..\include\ltNet.h"
#endif //USE_POINTERS_TO_LEAD_FUNCTIONS

#define L_HEADER_ENTRY8_
#include "..\ltpck.h"

//--LEAD WRAPPER INCLUDE FILES-------------------------------------------------
#include "ltcDefs.h"
#include "ltcWrpEr.h"
#include "ltcEnums.h"
#include "ltcTypDf.h"
#include "ltcFTypD.h"
#include "ltcExtrn.h"
#include "ltcFMcro.h"

//--FRIEND FUNCTIONS ----------------------------------------------------------
LWRP_EXPORT LBitmapBase *  LEAD_GetBitmapObject(LWRAPPERBITMAPMEMBER nClassType,LBase * This,LPLEADCREATEOBJECT pCreateObj);
LWRP_EXPORT LBase *        LEAD_GetObject(LWRAPPEROBJECTTYPE nType,LBase * This,LPLEADCREATEOBJECT pCreateObj);

//--LEAD WRAPPER CLASSES HEADER FILES------------------------------------------
#include "ltcMacro.h"
#include "ltcBase.h"
#include "ltcBBase.h"
#include "ltcBtmp.h"
#include "ltcBList.h"
#include "ltcBuffr.h"
#include "ltcDBufr.h"
#include "ltcChng.h"

#include "ltcDlg.h"
#include "ltcDlgImg.h"
#include "ltcDlgEfx.h"
#include "ltcDlgWeb.h"
#include "ltcDlgClr.h"
#include "ltcDlgDoc.h"
#include "ltcDlgFile.h"
#include "ltcdlgImgEfx.h"

#include "ltcDraw.h"
#include "ltcFile.h"

#include "ltcPlybk.h"
#include "ltcPrnt.h"
#include "ltcRgn.h"
#include "ltcScnnr.h"
#if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)
#include "ltcWia.h"
#endif // #if defined(LEADTOOLS_V16_OR_LATER) && !defined(IGNORE_CLASSLIB_WIA)
#include "ltcSetng.h"
#include "ltcScr.h"
#include "ltcAnn.h"
#include "ltcInet.h"
#include "ltcDictn.h"
#include "ltcBWnd.h"
#include "ltcAnim.h"
#include "ltcAnnWn.h"

#include "ltcImLst.h"

#include "ltcVBase.h"
#include "ltcVDlg.h"
#include "ltcVFile.h"
#include "ltcVMFil.h"
#include "ltcVWnd.h"
#include "ltcVLayr.h"
#include "ltcVGrp.h"
#include "ltcVObj.h"

#include "ltcBar.h"
#include "ltcMark.h"
#include "ltcTb.h"
#include "ltcCon.h"
#include "ltcAut.h"
#include "ltcHttp.h"
#include "ltcFtp.h"
#include "ltcSeg.h"
#include "ltcRst.h"
#include "ltcRstDlg.h"
#include "ltcRstWd.h"
#include "ltcimgopt.h"
#include "ltcpdfcomp.h"
#include "ltcNitf.h"
#include "ltcImgViewer.h"
#include "ltcICCProfile.h"
#include "ltcColor.h"
#include "ltcCritSecLock.h"   // for LCritSecLock

#define WRPUNLOCKSUPPORT() \
   LSettings::UnlockSupport(L_SUPPORT_DOCUMENT, L_KEY_DOCUMENT); \
   LSettings::UnlockSupport(L_SUPPORT_OCR, L_KEY_OCR); \
   LSettings::UnlockSupport(L_SUPPORT_MEDICAL, L_KEY_MEDICAL); \
   LSettings::UnlockSupport(L_SUPPORT_MEDICAL_NET, L_KEY_MEDICAL_NET); \
   LSettings::UnlockSupport(L_SUPPORT_VECTOR, L_KEY_VECTOR); \
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_1D, L_KEY_BARCODES_1D); \
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_2D_READ, L_KEY_BARCODES_2D_READ); \
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_2D_WRITE, L_KEY_BARCODES_2D_WRITE); \
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_PDF_READ, L_KEY_BARCODES_PDF_READ); \
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_PDF_WRITE, L_KEY_BARCODES_PDF_WRITE); \
   LSettings::UnlockSupport(L_SUPPORT_VECTOR, L_KEY_VECTOR); \
   LSettings::UnlockSupport(L_SUPPORT_PDF, L_KEY_PDF);\
   LSettings::UnlockSupport(L_SUPPORT_J2K, L_KEY_J2K);\
   LSettings::UnlockSupport(L_SUPPORT_CMW, L_KEY_CMW);\
   LSettings::UnlockSupport(L_SUPPORT_DICOM, L_KEY_DICOM);\
   LSettings::UnlockSupport(L_SUPPORT_EXTGRAY, L_KEY_EXTGRAY);\
   LSettings::UnlockSupport(L_SUPPORT_BITONAL, L_KEY_BITONAL);\
   LSettings::UnlockSupport(L_SUPPORT_PDF_SAVE, L_KEY_PDF_SAVE);\
   LSettings::UnlockSupport(L_SUPPORT_OCR_PDF_OUTPUT, L_KEY_OCR_PDF_OUTPUT);\
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_DATAMATRIX_READ, L_KEY_BARCODES_DATAMATRIX_READ); \
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_DATAMATRIX_WRITE, L_KEY_BARCODES_DATAMATRIX_WRITE); \
   LSettings::UnlockSupport(L_SUPPORT_LTPRO, L_KEY_LTPRO); \
   LSettings::UnlockSupport(L_SUPPORT_ICR, L_KEY_ICR); \
   LSettings::UnlockSupport(L_SUPPORT_OMR, L_KEY_OMR); \
   LSettings::UnlockSupport(L_SUPPORT_ABC, L_KEY_ABC);\
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_QR_READ, L_KEY_BARCODES_QR_READ); \
   LSettings::UnlockSupport(L_SUPPORT_BARCODES_QR_WRITE, L_KEY_BARCODES_QR_WRITE); \
   LSettings::UnlockSupport(L_SUPPORT_MOBILE, L_KEY_MOBILE); \
   LSettings::UnlockSupport(L_SUPPORT_JBIG2, L_KEY_JBIG2); \
   LSettings::UnlockSupport(L_SUPPORT_ABIC_READ, L_KEY_ABIC_READ); \
   LSettings::UnlockSupport(L_SUPPORT_ABIC_SAVE, L_KEY_ABIC_SAVE); \
   LSettings::UnlockSupport(L_SUPPORT_PDF_READ, L_KEY_PDF_READ);\
   LSettings::UnlockSupport(L_SUPPORT_PDF_ADVANCED, L_KEY_PDF_ADVANCED);\
   LSettings::UnlockSupport(L_SUPPORT_NITF, L_KEY_NITF);\

#undef L_HEADER_ENTRY8_
#include "..\ltpck.h"

#define MYATAN2(x,y) atan2((L_DOUBLE)x, (L_DOUBLE)y)
/*================================================================= EOF =====*/
#endif //_LEAD_WRAPPER_H_
