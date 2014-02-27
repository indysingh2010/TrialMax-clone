/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2001 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcMark.h                                                       |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_MARKER_H_
#define  _LEAD_MARKER_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LMarker                                                         |
| Desc      :                                                                 |
| Return    :                                                                 | 
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 2001                                                  |
+----------------------------------------------------------------------------*/

class LWRP_EXPORT LMarker : public LBase
{
   LEAD_DECLAREOBJECT(LMarker);
protected:
   L_HANDLE m_hMarkers;
   
private:
   static L_INT EXT_CALLBACK  EnumMarkersCS(L_UINT uMarker, L_UINT uMarkerSize, L_UCHAR *pMarkerData, L_VOID * pUserData, LEADMARKERCALLBACK pfnLEADCallback, L_VOID * pLEADUserData);
   LEADMARKERCALLBACK m_pfnLEADCallback;
   L_VOID * m_pLEADUserData;

protected:
   virtual L_INT EnumMarkersCallBack(L_UINT uMarker, L_UINT uMarkerSize, L_VOID *pMarkerData, LEADMARKERCALLBACK pfnLEADCallback, L_VOID * pLEADUserData);

public:
   LMarker();
   virtual ~LMarker();
   virtual L_INT Load(L_TCHAR *pszFilename, 
                      L_UINT uFlags);
   virtual L_INT Free();
   virtual L_INT SetHandle(HANDLE  *  phMarker,
                           L_BOOL bFreePrev);
   virtual L_INT SetAsGlobalMarkers(L_UINT uFlags);
   virtual L_INT GetGlobalMarkers(L_UINT uFlags);
   virtual L_INT Enum(L_UINT uFlags);
   virtual L_INT Delete(L_UINT uMarker, 
                        L_INT nCount);
   virtual L_INT Insert(L_UINT uIndex, 
                        L_UINT uMarker, 
                        L_UINT uMarkerSize, 
                        L_UCHAR *pMarkerData);
   virtual L_INT Create();
   virtual L_INT Copy(LMarker *pMarkerSrc);
   virtual L_INT Copy(HANDLE hMarkerSrc);
   virtual L_UINT GetCount();
   virtual L_INT GetMarker(L_UINT uIndex, 
                           L_UINT *puMarker, 
                           L_UINT *puMarkerSize, 
                           L_UCHAR *pMarkerData);
   virtual L_INT DeleteIndex(L_UINT uIndex);
   HANDLE GetHandle();

};

#endif // _LEAD_MARKER_H_
