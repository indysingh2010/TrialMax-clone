#if !defined(LTCRT_H)
#define LTCRT_H

#pragma warning(disable: 4710)   // 'inline function' : function not expanded
#if (_MSC_VER <=1200) || (_MSC_VER == 1310)
#if defined(__cplusplus)
   #include<stdio.h>
   #include <time.h>
   #include <Tchar.h>

#if !defined (_countof)
   #define _countof(p) sizeof(p)/sizeof(p[0])
#endif // (_countof)
   #define errno_t int

   static inline int sprintf_s(char * p1, size_t sizeOfBuffer, const char * format, ...)
   {
      UNREFERENCED_PARAMETER(sizeOfBuffer);
      int nRet= 0;
      va_list args;
      va_start(args,format);
      nRet = vsprintf(p1,format,args);
      strcpy(p1 + nRet, "\0");
      va_end(args);
      return nRet;
   }

   static inline int sprintf_s(char * p1, const char * format, ...)
   {
      int nRet= 0;
      va_list args;
      va_start(args,format);
      nRet = vsprintf(p1,format,args);
      strcpy(p1 + nRet, "\0");
      va_end(args);
      return nRet;
   }

   static inline int vsprintf_s(char * p1, size_t sizeOfBuffer, const char * format, ...)
   {
      UNREFERENCED_PARAMETER(sizeOfBuffer);
      int nRet= 0;
      va_list args;
      va_start(args,format);
      nRet = vsprintf(p1,format,args);
      strcpy(p1 + nRet, "\0");
      va_end(args);
      return nRet;
   }

   static inline int vsprintf_s(char * p1, const char * format, ...)
   {
      int nRet= 0;
      va_list args;
      va_start(args,format);
      nRet = vsprintf(p1,format,va_arg(args, char* ));
      strcpy(p1 + nRet, "\0");
      va_end(args);
      return nRet;
   }

   static inline int _fcvt_s(char* buffer, size_t sizeInBytes, double value, int count, int *dec, int *sign)
   {
      UNREFERENCED_PARAMETER(sizeInBytes);
      buffer = _fcvt(value, count, dec, sign);
      return (buffer != NULL)?  TRUE : FALSE;
   }

   static inline int _gcvt_s(char *buffer,  size_t sizeInBytes, double value, int digits)
   {
      UNREFERENCED_PARAMETER(sizeInBytes);
      _gcvt(value, digits, buffer);
      return (buffer != NULL)?  TRUE : FALSE;
   }
   

   static inline int vswprintf_s(wchar_t * p1, size_t sizeOfBuffer, const wchar_t * format, va_list args)
   {
      int nRet = 0;
      UNREFERENCED_PARAMETER(sizeOfBuffer);
      nRet = vswprintf(p1,format,args);
      wcscpy(p1 + nRet, L"\0");
      return nRet;
   }

   static inline int vswprintf_s(wchar_t * p1, const wchar_t * format, va_list args)
   {
      int nRet= 0;
      nRet = vswprintf(p1,format,args);
      wcscpy(p1 + nRet, L"\0");
      return nRet;
   }

   static inline int swprintf_s(wchar_t * p1, size_t sizeOfBuffer, const wchar_t * format, ...)
   {
      int nRet= 0;
      va_list args;
      va_start(args,format);
      nRet = vswprintf_s(p1, sizeOfBuffer, format, args);
      wcscpy(p1 + nRet, L"\0");
      va_end(args);
      return nRet;
   }

   static inline int swprintf_s(wchar_t * p1, const wchar_t * format, ...)
   {
      int nRet= 0;
      va_list args;
      va_start(args,format);
      nRet = vswprintf_s(p1,format,args);
      wcscpy(p1 + nRet, L"\0");
      va_end(args);
      return nRet;
   }

   static inline errno_t _itoa_s(int value, char *buffer, size_t sizeInCharacters, int radix)
   {
      UNREFERENCED_PARAMETER(sizeInCharacters);
      itoa(value, buffer, radix);
      return 1;
   }

   static inline errno_t _itoa_s(int value, char *buffer, int radix)
   {
      itoa(value, buffer, radix);
      return 1;
   }

   static inline errno_t _itow_s(int value,wchar_t *buffer,size_t sizeInCharacters,int radix)
   {
      UNREFERENCED_PARAMETER(sizeInCharacters);
      _itow(value, buffer, radix);
      return 1;
   }

   static inline errno_t _itow_s(int value,wchar_t *buffer, int radix)
   {
      _itow(value, buffer, radix);
      return 1;
   }

   static inline errno_t _ui64toa_s(unsigned __int64 value, char *buffer, size_t sizeInCharacters, int radix)
   {
      UNREFERENCED_PARAMETER(sizeInCharacters);
      _ui64toa(value, buffer, radix);
      return 1;
   }

   static inline errno_t _ui64tow_s(unsigned __int64 value, wchar_t *buffer, size_t sizeInCharacters, int radix)
   {
      UNREFERENCED_PARAMETER(sizeInCharacters);
      _ui64tow(value, buffer, radix);
      return 1;
   }

   static inline errno_t _ui64toa_s(unsigned __int64 value, char *buffer, int radix)
   {
      _ui64toa(value, buffer, radix);
      return 1;
   }

   static inline errno_t _ui64tow_s(unsigned __int64 value, wchar_t *buffer, int radix)
   {
      _ui64tow(value, buffer, radix);
      return 1;
   }

   static inline errno_t localtime_s(struct tm* _tm,const time_t *time)
   {
      _tm = localtime(time);
      return 1;
   }

   static inline char *strtok_s(char *strToken,const char *strDelimit,char **context)
   {
      UNREFERENCED_PARAMETER(context);
      return strtok(strToken, strDelimit);
   }

   static inline wchar_t *wcstok_s(wchar_t *strToken,const wchar_t *strDelimit,wchar_t **context)
   {
      UNREFERENCED_PARAMETER(context);
      return wcstok(strToken, strDelimit);
   }

   static inline errno_t strcpy_s(char *strDestination, size_t sizeInBytes, const char *strSource)
   {
      UNREFERENCED_PARAMETER(sizeInBytes);
      return strcpy(strDestination, strSource) ? TRUE : FALSE;
   }
   
   static inline errno_t strcpy_s(char *strDestination, const char *strSource)
   {
      return strcpy(strDestination, strSource) ? TRUE : FALSE;
   }

   static inline errno_t wcscpy_s(wchar_t *strDestination, size_t sizeInWords, const wchar_t *strSource)
   {
      UNREFERENCED_PARAMETER(sizeInWords);
      return wcscpy(strDestination, strSource) ? TRUE : FALSE;
   }

   static inline errno_t wcscpy_s(wchar_t *strDestination, const wchar_t *strSource)
   {
      return wcscpy(strDestination, strSource) ? TRUE : FALSE;
   }

   static inline errno_t strcat_s(char *strDestination, size_t sizeInBytes, const char *strSource)
   {
      UNREFERENCED_PARAMETER(sizeInBytes);
      return strcat(strDestination, strSource) ? TRUE : FALSE;
   }
   
   static inline errno_t strcat_s(char *strDestination, const char *strSource)
   {
      return strcat(strDestination, strSource) ? TRUE : FALSE;
   }

   static inline errno_t wcscat_s(wchar_t *strDestination, size_t sizeInWords, const wchar_t *strSource)
   {
      UNREFERENCED_PARAMETER(sizeInWords);
      return wcscat(strDestination, strSource) ? TRUE : FALSE;
   }

   static inline errno_t wcscat_s(wchar_t *strDestination, const wchar_t *strSource)
   {
      return wcscat(strDestination, strSource) ? TRUE : FALSE;
   }

# if (_MSC_VER <=1200)
   static inline double _wtof(const wchar_t *str)
   {
      char    *pmbbuf   = (char *)malloc(MB_CUR_MAX);

      wcstombs(pmbbuf, str, MB_CUR_MAX);
      
      return atof(pmbbuf);
   }
#endif // (_MSC_VER <=1200)
 
   static inline errno_t mbstowcs_s(size_t *pConvertedChars, wchar_t *wcstr, size_t sizeInWords, const char *mbstr, size_t count)
   {
      UNREFERENCED_PARAMETER(pConvertedChars);
      UNREFERENCED_PARAMETER(sizeInWords);
      
      return mbstowcs(wcstr, mbstr, count);
   }

   static inline errno_t mbstowcs_s(size_t *pConvertedChars, wchar_t *wcstr, const char *mbstr, size_t count)
   {
      UNREFERENCED_PARAMETER(pConvertedChars);
      
      return mbstowcs(wcstr, mbstr, count);
   }

  
   #define strncpy_s(p1,p2,p3,p4)                        strncpy(p1,p3,p4)
   #define strupr_s(p1,p2)                               strupr(p1)
   #define _splitpath_s(p1,p2,p3,p4,p5,p6,p7,p8,p9)      _splitpath(p1, p2, p4, p6, p8)
   #define _makepath_s(p1,p2,p3,p4,p5,p6)                _makepath_s(p1 ,p3, p4, p5, p6)
   #define sscanf_s                                      sscanf


   #define wcsncpy_s(p1,p2,p3,p4)                        wcsncpy(p1,p3,p4)
   #define _wcsupr_s(p1,p2)                              _wcsupr(p1)
   #define _wsplitpath_s(p1,p2,p3,p4,p5,p6,p7,p8,p9)     _wsplitpath(p1, p2, p4, p6, p8)
   #define _wmakepath_s(p1,p2,p3,p4,p5,p6)               _wmakepath(p1 ,p3, p4, p5, p6)

   #if defined(_UNICODE)
      #define _tsplitpath_s(p1,p2,p3,p4,p5,p6,p7,p8,p9)     _wsplitpath(p1, p2, p4, p6, p8)
      #define _tcscpy_s                                     wcscpy_s
      #define _tcscat_s                                     wcscat_s
      #define _tcsupr_s(p1,p2)                              (_wcsupr(p1) != NULL) ? TRUE : FALSE
      #define _stprintf_s                                   swprintf_s
      #define _tcsncpy_s(p1,p2,p3,p4)                       wcsncpy_s(p1,p2,p3,p4) != NULL ? TRUE : FALSE
      #define _tcslwr_s(p1,p2)                              (_wcsupr(p1) != NULL) ? TRUE : FALSE
      #define _tmakepath_s(p1,p2,p3,p4,p5,p6)               _wmakepath(p1 ,p3, p4, p5, p6)
      #define _itot_s                                       _itow_s
      #define _tcstok_s                                     wcstok_s
      #define _ui64tot_s                                    _ui64tow_s
      #define _tstof                                        _wtof
      #define _wcslwr_s(p1,p2)                               (_wcslwr(p1)!= NULL) ? TRUE : FALSE
      #define _snwprintf_s(p1,p2,p3,p4)                     _snwprintf(p1,p3,p4)
      #define _vstprintf_s                                   vswprintf_s
   #else
      #define _tsplitpath_s(p1,p2,p3,p4,p5,p6,p7,p8,p9)     _splitpath(p1, p2, p4, p6, p8)
      #define _tcscpy_s                                     strcpy_s
      #define _tcscat_s                                     strcat_s
      #define _tcsupr_s(p1,p2)                              (strupr(p1) != NULL) ? TRUE : FALSE
      #define _stprintf_s                                   sprintf_s
      #define _tcslwr_s(p1,p2)                              (strlwr(p1) != NULL) ? TRUE : FALSE
      #define _itot_s                                       _itoa_s
      #define _tcstok_s                                     strtok_s
      #define _ui64tot_s                                    _ui64toa_s
      #define _tstof                                        atof
      #define _tmakepath_s(p1,p2,p3,p4,p5,p6)               _makepath(p1 ,p3, p4, p5, p6)
      #define _vstprintf_s                                  vsprintf_s
   #endif

# if(_MSC_VER <=1200)
   #define _tstoi64                                          _ttoi64
   #define _tstoi                                           _ttoi
#endif // # if(_MSC_VER <=1200)

   #define wcstombs_s(p1,p2,p3,p4,p5)                       UNREFERENCED_PARAMETER(p1); UNREFERENCED_PARAMETER(p3); wcstombs(p2,p4,p5);
#endif
#endif // #if defined(__cplusplus)
#endif // #if !defined(LTCRT_H)
