/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2003 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcdbufr.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DOUBLEBUFFER_H_
#define  _LEAD_DOUBLEBUFFER_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     :LDoubleBuffer                                                          |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : November  2003                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LDoubleBuffer :public LBase
{
   LEAD_DECLAREOBJECT(LDoubleBuffer);
   public:
      L_VOID *m_extLDoubleBuffer;
      
   private:  
      L_HANDLE m_hDoubleBufferHandle;
      L_BOOL m_bEnableDoubleBuffer;

   private:  
      L_VOID Initialize();

   public : 
      LDoubleBuffer(); 
      virtual ~LDoubleBuffer();
      L_HANDLE* GetHandle();
      L_INT SetHandle(L_HANDLE  *hDoubleBufferHandle, L_BOOL bFreePrev);
      
      virtual L_BOOL EnableDoubleBuffer(L_BOOL bEnable);
      virtual L_INT CreateHandle();
      virtual L_INT DestroyHandle();
      virtual HDC   Begin(HDC hDC, L_INT cx, L_INT cy);
      virtual L_INT End(HDC hDC);
      virtual L_BOOL IsDoubleBufferEnabled();
};

#endif //_LEAD_BUFFER_H_
/*================================================================= EOF =====*/
