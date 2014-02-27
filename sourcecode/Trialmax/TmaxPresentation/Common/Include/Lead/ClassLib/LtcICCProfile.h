/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2005 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : LTCICCProfile.h                                                 |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_ICCPROFILE_H_
#define  _LEAD_ICCPROFILE_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LICCProfile                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : 14 November 2005                                                |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LICCProfile : public LBase
{

private:
   ICCPROFILEEXT m_ICCProfileExt;

protected:

public:
   LICCProfile();
   ~LICCProfile();

   L_INT Fill(L_UCHAR * pData, L_UINT uDataSize);
   L_INT Fill(L_TCHAR * pszFileName);

   L_INT Initialize();
   L_VOID Free();
   L_INT InitHeader();
   L_INT SetCMMType(L_IccInt32Number nCMMType);
   L_INT SetDeviceClass(ICCPROFILECLASS uDevClassSig);
   L_INT SetColorSpace(ICCCOLORSPACE uColorSpace);
   L_INT SetConnectionSpace(ICCCOLORSPACE uPCS);
   L_INT SetPrimaryPlatform(ICCPLATFORMSIGNATURE uPrimPlatform);
   L_INT SetFlags(L_IccUInt32Number uFlags);
   L_INT SetDevManufacturer(L_IccUInt32Number nDevManufacturer);
   L_INT SetDevModel(L_IccUInt32Number uDevModel);
   L_INT SetDeviceAttributes(L_IccUInt64Number uAttributes);
   L_INT SetRenderingIntent(ICCRENDERINGINTENT uRenderingIntent);
   L_INT SetCreator(L_IccInt32Number nCreator);
   L_INT SetDateTime(pICC_DATE_TIME_NUMBER pTime);
   L_INT SetTagData( L_UCHAR * pTagData, L_UINT uTagSig, L_UINT uTagTypeSig);

   L_INT GetTagData( L_UCHAR * pTagData, L_UINT32 uTagSignature);
   static L_INT CreateTagData( L_UCHAR * pDestTagData, 
                               L_UCHAR * pSrcTagData, 
                               L_UINT32  uTagTypeSig);

   L_INT DeleteTag( L_UINT32 uTagSig, L_UCHAR * pTag);
   L_INT GenerateFile( L_TCHAR * pszFileName);
   static L_UINT32 ConvertDoubleTo2bFixed2bNumber( L_DOUBLE dNumber);
   static L_DOUBLE Convert2bFixed2bNumberToDouble( L_UINT32 uNumber);
   L_INT GeneratePointer();
   L_UINT32 GetTagTypeSig(L_UINT32 uTagSig);
   static L_VOID FreeTagType(L_UCHAR * pTagType, L_UINT32 uTagTypeSig);

   L_INT GetParametricCurveNumberOfParameters(ICCFUNCTIONTYPE enFunctionType);
   static L_UINT16 DoubleToU8Fixed8Number(L_DOUBLE dNumber);
   static L_DOUBLE U8Fixed8NumberToDouble(L_UINT16 uNumber);
   L_INT ConvertCLUTToBuffer(L_UCHAR * pData, L_VOID * pIccCLUT, L_INT nPrecision, L_SSIZE_T nDataSize);
   L_INT ConvertCurveTypeToBuffer(L_UCHAR * pData, pICCTAG_CURVE_TYPE pIccTagCurveType);
   L_INT ConvertParametricCurveTypeToBuffer(L_UCHAR * pData, pICCTAG_PARAMETRIC_CURVE_TYPE pIccTagParametricCurveType);
   L_INT SetProfileId(pICCPROFILEEXT pICCProfile);
   L_INT Load(L_TCHAR * pszFilename, pLOADFILEOPTION pLoadOptions);
   L_INT Save(L_TCHAR * pszFilename, pSAVEFILEOPTION pSaveOptions);

   ICCPROFILEEXT* GetProfile();

};
#endif //_LEAD_ICCPROFILE_H_
/*================================================================= EOF =====*/
