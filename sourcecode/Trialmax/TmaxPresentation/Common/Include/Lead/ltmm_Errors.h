#ifndef __ltmm_Errors_H__
#define __ltmm_Errors_H__

// Last error used = 0x80050026

// #define E_EVAL_EXPIRED       _HRESULT_TYPEDEF_(0x80050010) /* (In ILMDSKernel2.h) The eval has expired */
#define LTMM_E_INVALID_DATA   _HRESULT_TYPEDEF_(0x80050013) /* Invalid data */
#define LTMM_E_INVALID_HEADER _HRESULT_TYPEDEF_(0x80050014) /* Invalid packed data */
#define LTMM_E_DISABLED       _HRESULT_TYPEDEF_(0x80050015) /* This feature is disabled */
#define LTMM_E_INVALID_VERSION _HRESULT_TYPEDEF_(0x80050016) /* This version is not supported */
#define LTMM_E_STILLIMAGE_LOCKED _HRESULT_TYPEDEF_(0x80050017) /* The LEADTOOLS still image toolkit needs to be unlocked */
// #define E_APP_ALREADY_REGISTERED       _HRESULT_TYPEDEF_(0x80050018) /* (In ILMDSKernel2.h) An app with the same name is already registered */
// #define E_APP_NOT_FOUND =  _HRESULT_TYPEDEF_0x80050019 /* (In ILMDSKernel2.h) The app has not been found */
#define LTMM_E_STILLIMAGE_MISSING   _HRESULT_TYPEDEF_(0x8005001A) /* One or more raster LEADTOOLS modules are missing */

/* The next two defines are used to return a LEADTOOLS DICOM error. Unlike LEADTOOLS still image errors, DICOM errors are positive.
   The error is (<HRESULT_CODE> - LTMM_E_DICOM_ERROR_FIRST). 
   The error code range is 0x8005E000 - 0x800EFFF. So, if the HRESULT is 0x8005E002, then the LEADTOOLS DICOM error is '-2' (not enough memory). */
#define LTMM_E_DICOM_ERROR_FIRST  _HRESULT_TYPEDEF_(0x8005E000) 
#define LTMM_E_DICOM_ERROR_LAST   _HRESULT_TYPEDEF_(0x8005EFFF)

/* The next two defines are used to return a LEADTOOLS still image error code. 
   The error is - (<HRESULT_CODE> - LTMM_E_STILLIMAGE_ERROR_FIRST). 
   The error code range is 0x8005F000 - 0x8005FFF. 
   So, if the HRESULT is 0x8005F009, then the LEADTOOLS error is '-9' (error loading file). */
#define LTMM_E_STILLIMAGE_ERROR_FIRST  _HRESULT_TYPEDEF_(0x8005F000) 
#define LTMM_E_STILLIMAGE_ERROR_LAST   _HRESULT_TYPEDEF_(0x8005FFFF)


#define LTMM_E_DVR_DISCARDED_DATA      _HRESULT_TYPEDEF_(0x80050020) /* You are trying to read discarded data */
#define LTMM_E_DVR_NO_RECORDINGS       _HRESULT_TYPEDEF_(0x80050021) /* There are no recordings in the stub file */
#define LTMM_E_DVR_CORRUPT_STUB_FILE   _HRESULT_TYPEDEF_(0x80050022) /* The stub file is corrupt */
#define LTMM_E_DVR_STOPPED             _HRESULT_TYPEDEF_(0x80050023) /* The DVR has stopped writing data */
#define LTMM_S_DVR_WAITING             _HRESULT_TYPEDEF_(0x00050024) /* The DVR source is waiting for data to be written to disk */
#define LTMM_E_DVR_CHANGING_ATTRIBUTES _HRESULT_TYPEDEF_(0x80050025) /* The DVR sink is changing attributes */
#define LTMM_E_DVR_SUSPENDED           _HRESULT_TYPEDEF_(0x80050026) /* Reading for current sample is temporarily suspended */

#endif // #ifndef __ltmm_Errors_H__
