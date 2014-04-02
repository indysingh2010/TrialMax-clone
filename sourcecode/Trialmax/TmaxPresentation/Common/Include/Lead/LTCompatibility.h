#ifndef __LTCOMPATIBILITY__H__
#define __LTCOMPATIBILITY__H__

//sb: VC++ 6.0 does not like inline from C code sometimes (eg: H263 decoder)
#ifndef INLINE
#ifdef __cplusplus
#define INLINE inline
#else
#define INLINE
#endif // #ifdef __cplusplus
#endif // #ifndef INLINE

#ifndef FOR_UNIX

//...........................................................................................
// SUPPORTED ENVIROMENTS
//...........................................................................................
#if _MSC_VER < 1400 && _MSC_VER >= 1300
   //#error "We only support VC6 and VS 2005"
#endif//_MSC_VER

//global includes
#include <tchar.h>

#pragma warning(push)
#pragma warning(disable: 4035 4068)

#include <windows.h> // for UNREFERENCED_PARAMETER

#pragma warning(pop)

//global defines
#ifndef _countof
#define _countof(a) (sizeof(a) / sizeof(*a)) 
#endif

#ifndef GWLP_USERDATA
#define GWLP_USERDATA GWL_USERDATA
#endif // #ifndef GWLP_USERDATA

#ifndef GWLP_WNDPROC
#define GWLP_WNDPROC GWL_WNDPROC
#endif // #ifndef GWLP_WNDPROC

#ifndef GCLP_HCURSOR
#define GCLP_HCURSOR GCL_HCURSOR
#endif // #ifndef GCLP_HCURSOR

#ifndef GCLP_HBRBACKGROUND
#define GCLP_HBRBACKGROUND GCL_HBRBACKGROUND
#endif

//...........................................................................................
// MOBILE PLATFORM INCLUDES
//...........................................................................................
#ifdef UNDER_CE
#include <streams.h>
//#include <cprop.h>
#include <altcecrt.h>
#include "CEInterfaces.h"
#endif

//...........................................................................................
// FOR DEPRECATED FUNCTIONS ON VISUAL STUDIO VER < 8
//...........................................................................................

//...........................................................................................
//    TCHAR DEPRECATED FUNCTIONS (1)
//...........................................................................................
#if _MSC_VER < 1300 || UNDER_CE// 1200 == VC++ 6.0, 1200-1202 == eVC++4, Windows Mobile Platform

//macros for the functions with same parameters
#ifndef _stprintf_s
#ifdef UNICODE
#define _stprintf_s swprintf_s
#else
#define _stprintf_s sprintf_s
#endif // #ifdef UNICODE
#endif // #ifndef _stprintf_s

#endif

/*sb: The Windows Mobile 5.0 PocketPC configurations has UNDER_CE defined and yet defines _tcscpy_s
      You need to restrict this check further more for your CE configurations */
#if (_MSC_VER < 1400) || (defined(_WIN32_WCE) && _WIN32_WCE < 0x500)// 1200 == VC++ 6.0, 1200-1202 == eVC++4, Windows Mobile Platform

//mapping macros:

//macros for the functions with different parameters
#undef _tcscpy_s 
#define _tcscpy_s(_Dst, _SizeInChars, _Src ) _tcscpy(_Dst, _Src)

#undef _tcscat_s 
#define _tcscat_s(_Dst, _SizeInChars, _Src ) _tcscat(_Dst, _Src)

#endif//_MSC_VER 

#if _MSC_VER < 1300 || (defined(_WIN32_WCE) && _WIN32_WCE < 0x500)// 1200 == VC++ 6.0, 1200-1202 == eVC++4, Windows Mobile Platform

#include <stdlib.h>

#undef _ltot_s 
#define _ltot_s(Val, _Dst, Size, _Radix ) _ltot ( Val, _Dst, _Radix )

#endif//_MSC_VER 

//...........................................................................................
//    TCHAR DEPRECATED FUNCTIONS (2)
//...........................................................................................
#if _MSC_VER < 1300 // 1200 == VC++ 6.0, 1200-1202 == eVC++4

#include <stdlib.h>

//mapping macros:
#ifndef _tsplitpath_s 
#define _tsplitpath_s(_Path,_Drive,_Drive_len,_Dir,_Dir_len,_Fname,_Fname_len,_Ext,_Ext_len) _tsplitpath(_Path, _Drive, _Dir, _Fname, _Ext)
#endif

#endif//_MSC_VER

//...........................................................................................
//    TCHAR DEPRECATED FUNCTIONS (3)
//...........................................................................................
#ifdef UNDER_CE // Mobile

//mapping macros:
#ifndef _tsplitpath_s 
#define _tsplitpath_s LT_SPLIT_PATH
#endif

#ifndef _MAX_DRIVE
#define _MAX_DRIVE  3   /* max. length of drive component */
#endif

#ifndef _MAX_DIR    
#define _MAX_DIR    256 /* max. length of path component */
#endif

#ifndef _MAX_FNAME 
#define _MAX_FNAME  256 /* max. length of file name component */
#endif

#ifndef _MAX_EXT    
#define _MAX_EXT    256 /* max. length of extension component */
#endif

//_tsplitpath_s (_splitpath):
INLINE int __cdecl LT_SPLIT_PATH ( const TCHAR *path, 
                                  TCHAR *drive, size_t /*_Drive_len*/, 
                                  TCHAR *dir, size_t /*_Dir_len*/, 
                                  TCHAR *fname, size_t /*_Fname_len*/, 
                                  TCHAR *ext, size_t /*_Ext_len*/)
{
      register TCHAR *p;
      TCHAR *last_slash = NULL, *dot = NULL;
      unsigned len;

      /* extract drive and :, if any */
      if ((_tcslen(path) >= (_MAX_DRIVE - 2)) && (*(path + _MAX_DRIVE - 2) == _T(':'))) {
         if (drive) {
            _tcsncpy(drive, path, _MAX_DRIVE - 1);
            *(drive + _MAX_DRIVE-1) = _T('\0');
         }
         path += _MAX_DRIVE - 1;
      }
      else if (drive) {
         *drive = _T('\0');
      }

      /* extract path string, if any.  Path now points to the first character
      * of the path, if any, or the filename or extension, if no path was
      * specified.  Scan ahead for the last occurence, if any, of a '/' or
      * '\' path separator character.  If none is found, there is no path.
      * We will also note the last '.' character found, if any, to aid in
      * handling the extension.
      */

      for (last_slash = NULL, p = (TCHAR *)path; *p; p++) {
#ifdef _MBCS
         if (_ISLEADBYTE (*p))
            p++;
         else {
#endif  /* _MBCS */
            if (*p == _T('/') || *p == _T('\\'))
               /* point to one beyond for later copy */
               last_slash = p + 1;
            else if (*p == _T('.'))
               dot = p;
#ifdef _MBCS
         }
#endif  /* _MBCS */
      }

      if (last_slash) {

         /* found a path - copy up through last_slash or max. characters
         * allowed, whichever is smaller
         */

         if (dir) {
            len = __min(((char *)last_slash - (char *)path) / sizeof(TCHAR),
               (_MAX_DIR - 1));
            _tcsncpy(dir, path, len);
            *(dir + len) = _T('\0');
         }
         path = last_slash;
      }
      else if (dir) {

         /* no path found */

         *dir = _T('\0');
      }

      /* extract file name and extension, if any.  Path now points to the
      * first character of the file name, if any, or the extension if no
      * file name was given.  Dot points to the '.' beginning the extension,
      * if any.
      */

      if (dot && (dot >= path)) {
         /* found the marker for an extension - copy the file name up to
         * the '.'.
         */
         if (fname) {
            len = __min(((char *)dot - (char *)path) / sizeof(TCHAR),
               (_MAX_FNAME - 1));
            _tcsncpy(fname, path, len);
            *(fname + len) = _T('\0');
         }
         /* now we can get the extension - remember that p still points
         * to the terminating nul character of path.
         */
         if (ext) {
            len = __min(((char *)p - (char *)dot) / sizeof(TCHAR),
               (_MAX_EXT - 1));
            _tcsncpy(ext, dot, len);
            *(ext + len) = _T('\0');
         }
      }
      else {
         /* found no extension, give empty extension and copy rest of
         * string into fname.
         */
         if (fname) {
            len = __min(((char *)p - (char *)path) / sizeof(TCHAR),
               (_MAX_FNAME - 1));
            _tcsncpy(fname, path, len);
            *(fname + len) = _T('\0');
         }
         if (ext) {
            *ext = _T('\0');
         }
      }

   return 0 ;
}

#define vswprintf_s(_Dst, _SizeInWords, _Format, _ArgList) vswprintf(_Dst, _Format, _ArgList)
#define _tcstok_s(strToken, strDelimit, context) _tcstok(strToken, strDelimit)
#define strtok_s(strToken, strDelimit, context) strtok(strToken, strDelimit)

#define _vstprintf_s(BUT, OUTBUF, FORMAT, ARGLIST) _vstprintf(BUT, FORMAT, ARGLIST)
#define vsprintf_s(BUT, OUTBUF, FORMAT, ARGLIST)   vsprintf(BUT, FORMAT, ARGLIST)
int __cdecl swprintf_s(__out_ecount_z(_SizeInWords) wchar_t * _Dst, __in size_t _SizeInWords, __in_z __format_string const wchar_t * _Format, ...);
int __cdecl sprintf_s(__out_ecount_z(_SizeInBytes) char * _Dst, __in size_t _SizeInBytes, __in_z __format_string const char * _Format, ...);

#define _tcsncat_s(_Destination, _Destination_size_chars, _Source, _Count) _tcsncat(_Destination, _Source, _Count)

#define _expand   realloc

#endif//UNDER_CE

//...........................................................................................
//    TIME DEPRECATED FUNCTIONS 
//...........................................................................................
#if (_MSC_VER < 1400) || UNDER_CE// 1200 == VC++ 6.0, 1200-1202 == eVC++4, Windows Mobile Platform

#include <time.h>

#ifndef  localtime_s 
#define localtime_s LT_LOCAL_TIME
#endif

//localtime_s (localtime):
static __inline int LT_LOCAL_TIME ( struct tm * _Tm, const time_t * _Time )
{
   _Tm = localtime ( _Time ) ;

   return 0 ;
}

#endif//_MSC_VER || UNDER_CE


//...........................................................................................
// MOBILE PLATFORM FUNCTIONS
//...........................................................................................

//...........................................................................................
//ShellExecute 
//...........................................................................................
#include <Wtypes.h>

#ifdef UNICODE
#define L_ShellExecute L_ShellExecuteW
#else
#define L_ShellExecute L_ShellExecuteA
#endif // #ifdef UNICODE

HINSTANCE WINAPI L_ShellExecuteA(HWND hwnd,
                          LPCSTR lpOperation,
                          LPCSTR lpFile,
                          LPCSTR lpParameters,
                          LPCSTR lpDirectory,
                          INT nShowCmd);
HINSTANCE WINAPI L_ShellExecuteW(HWND hwnd,
                          LPCWSTR lpOperation,
                          LPCWSTR lpFile,
                          LPCWSTR lpParameters,
                          LPCWSTR lpDirectory,
                          INT nShowCmd);

//...........................................................................................
// abort()
//...........................................................................................
#ifdef UNDER_CE
INLINE void abort(void) { exit(3) ; }
#endif

//...........................................................................................
// MOBILE PLATFORM MACROS & DEFINES
//...........................................................................................
#ifdef UNDER_CE
   // HRESULT_FROM_WIN32 converts ERROR_SUCCESS to a success code, but in
   // our use of HRESULT_FROM_WIN32, it typically means a function failed
   // to call SetLastError(), and we still want a failure code.
   //
   #define AmHresultFromWin32(x) (MAKE_HRESULT(SEVERITY_ERROR, FACILITY_WIN32, x))

   static HRESULT AmGetLastErrorToHResult() 
   {
      DWORD dwLastError = GetLastError();
      if(dwLastError != 0) 
      {
         return AmHresultFromWin32(dwLastError);
      }
      else 
      {
         return E_FAIL;
      }
   }

   // 773c9ac0-3274-11d0-B724-00aa006c1A01            MEDIASUBTYPE_ARGB32
   DEFINE_GUID(MEDIASUBTYPE_ARGB32, 0x773c9ac0, 0x3274, 0x11d0, 0xb7, 0x24, 0x0, 0xaa, 0x0, 0x6c, 0x1a, 0x1 ) ;
#endif

//...........................................................................................
// MOBILE PLATFORM INTERFACES
//...........................................................................................
#ifdef UNDER_CE
typedef /* [public][public][public][public][public][public] */ struct __MIDL___MIDL_itf_strmif_0133_0003
 {
    CLSID clsMedium;
    DWORD dw1;
    DWORD dw2;
 } 	REGPINMEDIUM;


enum __MIDL___MIDL_itf_strmif_0133_0004
{	REG_PINFLAG_B_ZERO	= 0x1,
   REG_PINFLAG_B_RENDERER	= 0x2,
   REG_PINFLAG_B_MANY	= 0x4,
   REG_PINFLAG_B_OUTPUT	= 0x8
} ;

typedef /* [public][public][public] */ struct __MIDL___MIDL_itf_strmif_0133_0005
{
   DWORD dwFlags;
   UINT cInstances;
   UINT nMediaTypes;
   /* [size_is] */ const REGPINTYPES *lpMediaType;
   UINT nMediums;
   /* [size_is] */ const REGPINMEDIUM *lpMedium;
   const CLSID *clsPinCategory;
} 	REGFILTERPINS2;

typedef /* [public][public] */ struct __MIDL___MIDL_itf_strmif_0133_0006
{
   DWORD dwVersion;
   DWORD dwMerit;
   /* [switch_type][switch_is] */ union 
   {
      /* [case()] */ struct 
      {
         ULONG cPins;
         /* [size_is] */ const REGFILTERPINS *rgPins;
      } 	;
      /* [case()] */ struct 
      {
         ULONG cPins2;
         /* [size_is] */ const REGFILTERPINS2 *rgPins2;
      } 	;
      /* [default] */  /* Empty union arm */ 
   } 	;
} 	REGFILTER2;

EXTERN_C const IID IID_IFilterMapper2;

MIDL_INTERFACE("b79bb0b0-33c1-11d1-abe1-00a0c905f375")
IFilterMapper2 : public IUnknown
{
public:
   virtual HRESULT STDMETHODCALLTYPE CreateCategory( 
      /* [in] */ REFCLSID clsidCategory,
      /* [in] */ DWORD dwCategoryMerit,
      /* [in] */ LPCWSTR Description) = 0;

   virtual HRESULT STDMETHODCALLTYPE UnregisterFilter( 
      /* [in] */ const CLSID *pclsidCategory,
      /* [in] */ const OLECHAR *szInstance,
      /* [in] */ REFCLSID Filter) = 0;

   virtual HRESULT STDMETHODCALLTYPE RegisterFilter( 
      /* [in] */ REFCLSID clsidFilter,
      /* [in] */ LPCWSTR Name,
      /* [out][in] */ IMoniker **ppMoniker,
      /* [in] */ const CLSID *pclsidCategory,
      /* [in] */ const OLECHAR *szInstance,
      /* [in] */ const REGFILTER2 *prf2) = 0;

   virtual HRESULT STDMETHODCALLTYPE EnumMatchingFilters( 
      /* [out] */ IEnumMoniker **ppEnum,
      /* [in] */ DWORD dwFlags,
      /* [in] */ BOOL bExactMatch,
      /* [in] */ DWORD dwMerit,
      /* [in] */ BOOL bInputNeeded,
      /* [in] */ DWORD cInputTypes,
      /* [size_is] */ const GUID *pInputTypes,
      /* [in] */ const REGPINMEDIUM *pMedIn,
      /* [in] */ const CLSID *pPinCategoryIn,
      /* [in] */ BOOL bRender,
      /* [in] */ BOOL bOutputNeeded,
      /* [in] */ DWORD cOutputTypes,
      /* [size_is] */ const GUID *pOutputTypes,
      /* [in] */ const REGPINMEDIUM *pMedOut,
      /* [in] */ const CLSID *pPinCategoryOut) = 0;

};
#endif

#ifdef UNDER_CE
/* Mobile defines */
#define BI_RLE8       1L   // should have been in wingdi.h
#define BI_RLE4       2L   // should have been in wingdi.h

#ifndef GWLP_WNDPROC
  #define GWLP_WNDPROC        (-4)
#endif
#ifndef GWLP_HINSTANCE
  #define GWLP_HINSTANCE      (-6)
#endif
#ifndef GWLP_HWNDPARENT
  #define GWLP_HWNDPARENT     (-8)
#endif
#ifndef GWLP_USERDATA
  #define GWLP_USERDATA       (-21)
#endif
#ifndef GWLP_ID
  #define GWLP_ID             (-12)
#endif
#ifndef DWLP_MSGRESULT
  #define DWLP_MSGRESULT  0
#endif
#ifndef DWLP_DLGPROC 
  #define DWLP_DLGPROC    DWLP_MSGRESULT + sizeof(LRESULT)
#endif
#ifndef DWLP_USER
  #define DWLP_USER       DWLP_DLGPROC + sizeof(DLGPROC)
#endif

#ifndef RDW_FRAME
#define RDW_FRAME          0
#endif // #ifndef RDW_FRAME

#ifndef OUT_CHARACTER_PRECIS
#define OUT_CHARACTER_PRECIS  0
#endif // #ifndef OUT_CHARACTER_PRECIS

/* Mobile typedefs */
typedef CONST BYTE * LPCBYTE;

DWORD WINAPI SearchPath(
  LPCTSTR lpPath,      // search path
  LPCTSTR lpFileName,  // file name
  LPCTSTR lpExtension, // file extension
  DWORD nBufferLength, // size of buffer
  LPTSTR lpBuffer,     // found file name buffer
  LPTSTR *lpFilePart   // file component
);

#define MAKEPOINTS(l)       (*((POINTS FAR *)&(l)))

#define WS_EX_CONTROLPARENT   0

BOOL WINAPI DestroyCursor(__in HCURSOR hCursor);
BOOL WINAPI GdiFlush(void);

#define SetLastErrorEx(dwErrCode, dwType) SetLastError(dwErrCode)

//#define MkParseDisplayName MkParseDisplayNameEx // in URLMon.dll
#define MkParseDisplayName L_ParseDisplayName   // my version of the function

EXTERN_C int GetDIBits(HDC hdc, HBITMAP hbmp, UINT uStartScan,
              UINT cScanLines, LPVOID lpvBits,
              LPBITMAPINFO lpbi, UINT uUsage);

EXTERN_C int WINAPI SetDIBits(__in_opt HDC hdc1, __in HBITMAP hbm1, __in UINT start, __in UINT cLines, __in CONST VOID *lpBits, __in CONST BITMAPINFO * lpbmi, __in UINT ColorUse);

EXTERN_C BOOL  WINAPI TextOut( __in HDC hdc, __in int x, __in int y, __in_ecount(c) LPCWSTR lpString, __in int c);

EXTERN_C DWORD WINAPI GetPrivateProfileString
(
   LPCWSTR lpAppName,
   LPCWSTR lpKeyName,
   LPCWSTR lpDefault,
   LPWSTR lpReturnedString,
   DWORD nSize,
   LPCWSTR lpFileName
);

EXTERN_C BOOL WINAPI WritePrivateProfileString
(
    __in_opt LPCWSTR lpAppName,
    __in_opt LPCWSTR lpKeyName,
    __in_opt LPCWSTR lpString,
    __in_opt LPCWSTR lpFileName
);

#define GET_DIB_BYTES(pixels) ((((pixels) + 31) & ~31) >> 3)

#define CDEF_DEVMON_FILTER  0x0080

/* Create DirectShow filter enumerator */
STDAPI L_CreateDevEnum(ICreateDevEnum **ppCreateDevEnum);
/* Parse display names for filter monikers we create */
STDAPI L_ParseDisplayName(IBindCtx *pbc, LPCWSTR pszDisplayName, ULONG *pchEaten, IMoniker **pMkOut);

#endif // #ifdef UNDER_CE

EXTERN_C HRESULT STDAPIVCALLTYPE L_DeleteKey(HKEY hKey, LPCTSTR pszSubKey);

#endif // #ifndef FOR_UNIX

#endif // #ifndef __LTCOMPATIBILITY__H__
