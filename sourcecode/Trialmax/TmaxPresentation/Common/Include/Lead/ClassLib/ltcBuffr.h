/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcbuffr.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_BUFFER_H_
#define  _LEAD_BUFFER_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     :LBuffer                                                          |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September  1998                                                 |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LBuffer :public LBase
{
   public:
      L_VOID *m_extLBuffer;
      
   private:  
      HGLOBAL           m_hData;
      L_UCHAR       *   m_pData;
      L_VOID        *   m_pResizeData; 
      L_SIZE_T          m_dwSize;
      L_BOOL            m_bResizeStart; 
      L_INT             m_nLockCount;
      L_BOOL            m_bValid;

   private:  
      L_VOID Initialize(L_SIZE_T dwSize);

   public : 
      LBuffer(); 
      LBuffer(DWORD_PTR dwSize); 
      LBuffer(L_TCHAR * lpStr); 
      LBuffer(L_VOID * lpData,DWORD_PTR dwSize); 
      LBuffer( LBuffer& LBufferSrc); 
      virtual ~LBuffer();

      L_INT              SetHandle(HGLOBAL * pMemHandle,L_SIZE_T dwSize=0);
      HGLOBAL            GetHandle();
      L_BOOL             IsValid();
      L_VOID  *          Lock();
      L_VOID             Unlock();
      L_INT              Fill(L_UCHAR cValue=0);
      L_INT              Free();          
      L_INT              Reallocate(L_SIZE_T dwSize);
      L_SIZE_T           GetSize();
      L_INT              Copy(L_TCHAR * lpStr); 
      L_INT              Copy(L_VOID * lpData,L_SIZE_T dwSize); 
      L_INT              Copy(LBuffer * pLBufferSrc);
      LBuffer&     operator=(LBuffer& LBufferSrc);
      LBuffer&     operator=(L_TCHAR * lpStr);

      virtual L_INT  CompressRow(LBuffer& LBufferSrc,
                                 L_INT  nCol,
                                 L_INT  nWidth); 
      virtual L_INT  CompressRows(LBuffer& LBufferSrc,
                                  L_UINT   nWidth,
                                  L_UINT   nRows);  
      virtual L_INT ConvertBuffer(L_INT  nWidth,
                                     L_INT  nBitsPerPixelSrc,
                                     L_INT  nBitsPerPixelDst,
                                     L_INT  nOrderSrc,
                                     L_INT  nOrderDst,
                                     LPRGBQUAD  pPaletteSrc,
                                     LPRGBQUAD  pPaletteDst,
                                     L_UINT  uFlags=0,
                                     L_INT   uLowBit=0,
                                     L_INT   uHighBit=0);
      virtual L_INT  ExpandRow( LBuffer& LBufferSrc,
                               L_UINT  nCol,
                               L_INT   nWidth); 
      virtual L_INT  ExpandRows(LBuffer& LBufferSrc,L_UINT  nWidth,
                                  L_UINT  nRows);
      virtual L_INT  ConvertColorSpace( LBuffer& LBufferSrc,
                                       L_INT  nWidth,
                                       L_INT  nFormatSrc,
                                       L_INT  nFormatDst);
      virtual L_INT   StartResize(L_INT  nOldWidth,
                                  L_INT  nOldHeight,
                                  L_INT  nNewWidth,
                                  L_INT  nNewHeight);
      virtual L_INT   Resize(L_INT  nRow,
                             L_INT  nBitsPerPixel,
                             L_INT *  pXSize,
                             L_INT *  pYSize);
      virtual L_INT StopResize(); 
};

#endif //_LEAD_BUFFER_H_
/*================================================================= EOF =====*/
