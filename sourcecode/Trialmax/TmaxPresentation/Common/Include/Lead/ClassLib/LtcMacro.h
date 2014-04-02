/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcMacro.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_WRAPPER_MACROS_H_
#define  _LEAD_WRAPPER_MACROS_H_

/*----------------------------------------------------------------------------+
| MACROS                                                                      |
+----------------------------------------------------------------------------*/

//--LEAD OBJECT IMPLEMENTATION-------------------------------------------------
#define LEAD_DECLAREOBJECT(ClassName)                                \
   public:                                                           \
      static LBase* LEAD_Create##ClassName()

#include "ltcBase.h"

#define LEAD_IMPLEMENTOBJECT(ClassName)                              \
   LBase* ClassName::LEAD_Create##ClassName()                        \
   {                                                                 \
      return new ClassName;                                              \
   }

//--MACROS USED BY PLAYBACK CLASS BITMAP MAPPING-------------------------------
#define LEAD_DECLARE_PLAYBACK_BITMAP()                               \
   public:                                                           \
      virtual LBitmapBase* GetBitmap()

#define LEAD_INIT_PLAYBACK_BITMAP(PlayBackClassName,BitmapClassName) \
LBitmapBase*   PlayBackClassName::GetBitmap()                        \
{                                                                    \
   return LEAD_GetBitmapObject(BITMAP_MEMBER_PLAYBACK,this,BitmapClassName::LEAD_Create##BitmapClassName); \
}

//--MACROS USED BY PLAYBACK CLASS BITMAP MAPPING-------------------------------
#define LEAD_DECLARE_LIST_BITMAP()                                   \
   private:                                                          \
      virtual LBitmapBase *GetBitmap()

#define LEAD_INIT_LIST_BITMAP(BitmapListClassName,BitmapClassName)   \
LBitmapBase *  BitmapListClassName::GetBitmap()                      \
{                                                                    \
   return LEAD_GetBitmapObject(BITMAP_MEMBER_BITMAPLIST,this,BitmapClassName::LEAD_Create##BitmapClassName); \
}
//--MACROS USED BY LBITMAP CLASS MAPING----------------------------------------
#define LEAD_INIT_CLASS(nType,ClassName)     \
         pLEADCreateObject[nType]=ClassName::LEAD_Create##ClassName;

#define LEAD_INIT_LFile(ClassName)           \
         LEAD_INIT_CLASS(LEAD_OBJECT_LFILE,ClassName)  

#define LEAD_INIT_LTwain(ClassName)          \
         LEAD_INIT_CLASS(LEAD_OBJECT_LTWAIN,ClassName)

#define LEAD_INIT_LPrint(ClassName)          \
         LEAD_INIT_CLASS(LEAD_OBJECT_LPRINT,ClassName)

#define LEAD_INIT_LPaint(ClassName)          \
         LEAD_INIT_CLASS(LEAD_OBJECT_LPAINT,ClassName)

#define LEAD_INIT_LDialogBase(ClassName)         \
         LEAD_INIT_CLASS(LEAD_OBJECT_LDIALOGBASE,ClassName)

#define LEAD_INIT_LDialogColor(ClassName)         \
         LEAD_INIT_CLASS(LEAD_OBJECT_LDIALOGCOLOR,ClassName)

#define LEAD_INIT_LDialogEffect(ClassName)         \
         LEAD_INIT_CLASS(LEAD_OBJECT_LDIALOGEFFECT,ClassName)

#define LEAD_INIT_LDialogImage(ClassName)         \
         LEAD_INIT_CLASS(LEAD_OBJECT_LDIALOGIMAGE,ClassName)

#define LEAD_INIT_LDialogWeb(ClassName)         \
         LEAD_INIT_CLASS(LEAD_OBJECT_LDIALOGWEB,ClassName)

#define LEAD_INIT_LDialogImageEffect(ClassName)         \
         LEAD_INIT_CLASS(LEAD_OBJECT_LDIALOGIMAGEEFFECT,ClassName)

#define LEAD_INIT_LDialogDocument(ClassName)         \
         LEAD_INIT_CLASS(LEAD_OBJECT_LDIALOGDOCUMENT,ClassName)

#define LEAD_INIT_LDialogFile(ClassName)         \
         LEAD_INIT_CLASS(LEAD_OBJECT_LDIALOGFILE,ClassName)

#define LEAD_INIT_LBitmapRgn(ClassName)      \
         LEAD_INIT_CLASS(LEAD_OBJECT_LBITMAPRGN,ClassName)

#define LEAD_INIT_LMemoryFile(ClassName)     \
         LEAD_INIT_CLASS(LEAD_OBJECT_LMEMORYFILE,ClassName)

#define LEAD_INIT_LPaintEffect(ClassName)    \
         LEAD_INIT_CLASS(LEAD_OBJECT_LPAINTEFFECT,ClassName)

#define LEAD_INIT_LScreenCapture(ClassName)  \
         LEAD_INIT_CLASS(LEAD_OBJECT_LSCREENCAPTURE,ClassName)

#define LEAD_INIT_LVectorDialog(ClassName)  \
         LEAD_INIT_CLASS(LEAD_OBJECT_LVECTORDIALOG,ClassName)

#define LEAD_INIT_LVectorFile(ClassName)  \
         LEAD_INIT_CLASS(LEAD_OBJECT_LVECTORFILE,ClassName)

#define LEAD_INIT_LVectorMemoryFile(ClassName)  \
         LEAD_INIT_CLASS(LEAD_OBJECT_LVECTORMEMORYFILE,ClassName)

#define LEAD_INIT_LVectorLayer(ClassName)  \
         LEAD_INIT_CLASS(LEAD_OBJECT_LVECTORLAYER,ClassName)

#define LEAD_INIT_LVectorObject(ClassName)  \
         LEAD_INIT_CLASS(LEAD_OBJECT_LVECTOROBJECT,ClassName)

#define LEAD_INIT_LBarCode(ClassName)  \
         LEAD_INIT_CLASS(LEAD_OBJECT_LBARCODE,ClassName)

#define LEAD_INIT_LMarker(ClassName)  \
         LEAD_INIT_CLASS(LEAD_OBJECT_LMARKER,ClassName)

#define LEAD_DESTROYCLASS(pClass)            \
   if(pClass)                                \
   {                                         \
      delete pClass;                         \
      pClass=0;                              \
   }

//--DECLARE CLASS MAP FOR LBITMAP CLASS----------------------------------------
#define LEAD_DECLARE_CLASS_MAP()                                                             \
   public:                                      \
      virtual LDialogBase*       DialogBase();    \
      virtual LDialogImage*      DialogImage();    \
      virtual LDialogColor*      DialogColor();    \
      virtual LDialogEffect*     DialogEffect();   \
      virtual LDialogDocument*   DialogDocument(); \
      virtual LDialogWeb*        DialogWeb();      \
      virtual LDialogImageEffect* DialogImageEffect();      \
      virtual LDialogFile*       DialogFile();     \
      virtual LBitmapRgn*        Region();         \
      virtual LPaint*            Paint();          \
      virtual LPaintEffect*      PaintEffect();    \
      virtual LTwain*            Twain();          \
      virtual LMemoryFile*       MemoryFile();     \
      virtual LFile*             File();           \
      virtual LPrint*            Print();          \
      virtual LScreenCapture*    ScreenCapture();  \
      virtual LVectorDialog*     VectorDialog();   \
      virtual LVectorFile*       VectorFile();     \
      virtual LVectorMemoryFile* VectorMemoryFile(); \
      virtual LVectorLayer*      VectorLayer(); \
      virtual LVectorObject*     VectorObject(); \
      virtual LBarCode*          BarCode();        \
      virtual LMarker*           Marker();        \
      static  LBase*          LEAD_ObjectManager(LWRAPPEROBJECTTYPE nType,LBase* This,LPLEADCREATEOBJECT* pCreateObj)


//--START OBJECT MAP FOR LBITMAP CLASS-------------------------------------------
#define LEAD_START_CLASS_MAP(ClassName,ClassParent)                                       \
LBarCode* ClassName::BarCode()                                                            \
{                                                                                         \
   return (LBarCode*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LBARCODE,this,0);          \
}                                                                                         \
LMarker* ClassName::Marker()                                                            \
{                                                                                         \
   return (LMarker*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LMARKER,this,0);          \
}                                                                                         \
LDialogBase* ClassName::DialogBase()                                                              \
{                                                                                         \
   return (LDialogBase*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LDIALOGBASE,this,0);            \
}                                                                                         \
LDialogImage* ClassName::DialogImage()                                                              \
{                                                                                         \
   return (LDialogImage*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LDIALOGIMAGE,this,0);            \
}                                                                                         \
LDialogEffect* ClassName::DialogEffect()                                                              \
{                                                                                         \
   return (LDialogEffect*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LDIALOGEFFECT,this,0);            \
}                                                                                         \
LDialogColor* ClassName::DialogColor()                                                              \
{                                                                                         \
   return (LDialogColor*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LDIALOGCOLOR,this,0);            \
}                                                                                         \
LDialogDocument* ClassName::DialogDocument()                                                              \
{                                                                                         \
   return (LDialogDocument*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LDIALOGDOCUMENT,this,0);            \
}                                                                                         \
LDialogFile* ClassName::DialogFile()                                                              \
{                                                                                         \
   return (LDialogFile*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LDIALOGFILE,this,0);            \
}                                                                                         \
LDialogWeb* ClassName::DialogWeb()                                                              \
{                                                                                         \
   return (LDialogWeb*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LDIALOGWEB,this,0);            \
}                                                                                         \
LDialogImageEffect* ClassName::DialogImageEffect()                                                              \
{                                                                                         \
   return (LDialogImageEffect*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LDIALOGIMAGEEFFECT,this,0);            \
}                                                                                         \
LBitmapRgn* ClassName::Region()                                                           \
{                                                                                         \
   return  (LBitmapRgn*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LBITMAPRGN,this,0);     \
}                                                                                         \
LPaint* ClassName::Paint()                                                                \
{                                                                                         \
   return  (LPaint*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LPAINT,this,0);             \
}                                                                                         \
LPaintEffect* ClassName::PaintEffect()                                                    \
{                                                                                         \
   return (LPaintEffect*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LPAINTEFFECT,this,0);  \
}                                                                                         \
LTwain* ClassName::Twain()                                                                \
{                                                                                         \
   return  (LTwain*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LTWAIN,this,0);             \
}                                                                                         \
LMemoryFile* ClassName::MemoryFile()                                                      \
{                                                                                         \
   return (LMemoryFile*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LMEMORYFILE,this,0);    \
}                                                                                         \
LFile* ClassName::File()                                                                  \
{                                                                                         \
   return (LFile*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LFILE,this,0);                \
}                                                                                         \
LPrint* ClassName::Print()                                                                \
{                                                                                         \
   return (LPrint*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LPRINT,this,0);              \
}                                                                                         \
LScreenCapture* ClassName::ScreenCapture()                                                \
{                                                                                         \
   return (LScreenCapture*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LSCREENCAPTURE,this,0);                \
}                                                                                         \
LVectorDialog* ClassName::VectorDialog()  \
{                                         \
return (LVectorDialog*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LVECTORDIALOG,this,0); \
}                                         \
LVectorFile* ClassName::VectorFile()  \
{                                         \
return (LVectorFile*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LVECTORFILE,this,0); \
}                                         \
LVectorMemoryFile* ClassName::VectorMemoryFile()  \
{                                         \
return (LVectorMemoryFile*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LVECTORMEMORYFILE,this,0); \
}                                         \
LVectorLayer* ClassName::VectorLayer()  \
{                                         \
return (LVectorLayer*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LVECTORLAYER,this,0); \
}                                         \
LVectorObject* ClassName::VectorObject()  \
{                                         \
return (LVectorObject*)ClassName::LEAD_ObjectManager(LEAD_OBJECT_LVECTORMEMORYFILE,this,0); \
}                                         \
LBase* ClassName::LEAD_ObjectManager(LWRAPPEROBJECTTYPE nType,LBase* This,LPLEADCREATEOBJECT* pCreateObj)   \
{                                                                                         \
   static LPLEADCREATEOBJECT  pLEADCreateObject[LEAD_OBJECT_LAST];                        \
   static L_BOOL              bInitialized  = 0 ;                                         \
   if(!bInitialized)                                                                      \
   {                                                                                      \
      bInitialized=TRUE;                                                                  \
      L_WRAPPER_MEMSET(pLEADCreateObject,0,sizeof(pLEADCreateObject));

//--END OBJECT MAP FOR LBITMAP CLASS-------------------------------------------
#define LEAD_END_CLASS_MAP(ClassName,ClassParent)                                         \
   }                                                                                      \
   if(pCreateObj!=0)                                                                      \
   {                                                                                      \
      if(pLEADCreateObject[nType]==0)                                                     \
         return (ClassParent::LEAD_ObjectManager(nType,This,pCreateObj));                 \
      else return LEAD_GetObject(nType,This,*pCreateObj=pLEADCreateObject[nType]);        \
   }                                                                                      \
   else                                                                                   \
   {                                                                                      \
      if(pLEADCreateObject[nType]==0)                                                     \
         return (ClassParent::LEAD_ObjectManager(nType,This,&pLEADCreateObject[nType]));  \
      else return LEAD_GetObject(nType,This,pLEADCreateObject[nType]);                    \
   }                                                                                      \
}

//--START/END CHANGING FOR LBITMAP CLASS (USED FROM OUTSIDE THE BITMAP CLASS)--
#define START_BITMAP_CHANGING(pLBitmap,nNotifyCode,nNotifyCatigory)              \
   {                                                                             \
      L_INT nDummy;                                                              \
      if(LDictionary_IsBitmap(pLBitmap))                                         \
         if(pLBitmap->IsChangeNotificationEnabled())                             \
         {                                                                       \
            nDummy=pLBitmap->StartChanging(nNotifyCode,nNotifyCatigory);         \
            if(nDummy!=SUCCESS)                                                  \
               LEAD_ERROR_RETURN(nDummy);                                        \
         }                                                                       \
   }

#define END_BITMAP_CHANGING(pLBitmap,nNotifyCode,nNotifyCatigory,nRetCode)       \
   if(LDictionary_IsBitmap(pLBitmap))                                            \
      if(pLBitmap->IsChangeNotificationEnabled())                                \
         if((nRetCode==SUCCESS)||(pLBitmap->IsAlwaysEndNotification()))          \
            pLBitmap->EndChanging(nNotifyCode,nNotifyCatigory,nRetCode)

//--START/END CHANGING FOR LBITMAP CLASS (USED WITHEN THE BITMAP CLASS)--------
#define START_CHANGING(nNotifyCode,nNotifyCatigory)                              \
   START_BITMAP_CHANGING(this,nNotifyCode,nNotifyCatigory)

#define END_CHANGING(nNotifyCode,nNotifyCatigory,nRetCode)                       \
   END_BITMAP_CHANGING(this,nNotifyCode,nNotifyCatigory,nRetCode)

#define SET_BITMAP_FILE_NAME(pBitmap,pNewFileName)                               \
   if(LDictionary_IsBitmap(pBitmap)&&pBitmap->IsFileNameEnabled())                      \
      pBitmap->SetFileName(pNewFileName)

#define GET_BITMAP_FILE_NAME(pBitmap,pFileNameBuff,nBuffSize)                    \
   if(LDictionary_IsBitmap(pBitmap)&&pBitmap->IsFileNameEnabled())                      \
      pBitmap->GetFileName(pFileNameBuff,&nBuffSize)

//--CHECKS FOR USING CALLBACK FUNCTIONS----------------------------------------
#define CHECK_CALLBACK(pCallBackFunc)                          \
   ((IsCallBackEnabled())? pCallBackFunc:NULL)

#define RESET_STATUS_CALLBACK()                                \
   {                                                           \
      LStatusDictionary.RemoveItem(&sdData);                   \
      if(LStatusDictionary.GetCount()<=0)                      \
         L_WRPSETSTATUSCALLBACK(lpfnOldStatusCB, pOldData, NULL, NULL); \
   }

#define CHECK_STATUS_CALLBACK(pBitmap)                         \
   STATUSDATA sdData;                                          \
   STATUSCALLBACK lpfnOldStatusCB = NULL;                      \
   L_VOID      * pOldData = NULL;                              \
  if(IsStatusCallBackEnabled())                                \
   {                                                           \
      LBase*   pObjectForStatusCB;                             \
      if(                                                      \
         LDictionary_IsBitmap(pBitmap)&&                       \
         pBitmap->IsRedirectStatusCallBackEnabled()            \
        )                                                      \
         pObjectForStatusCB=(LBase*)pBitmap;                   \
      else                                                     \
         pObjectForStatusCB=(LBase*)this;                      \
      sdData.dwID    = GetCurrentThreadId();                   \
      sdData.pObject = pObjectForStatusCB;                     \
      LStatusDictionary.AddItem(&sdData);                      \
      L_WRPSETSTATUSCALLBACK(StatusCS, pObjectForStatusCB,&lpfnOldStatusCB, &pOldData);       \
   }                                                           \
   else                                                        \
      RESET_STATUS_CALLBACK()


#define RESET_OVERLAY_CALLBACK()                                \
   {                                                           \
   L_WRPSETOVERLAYCALLBACK(lpfnOldStatusCB, pOldData, uOverLay_Flags); \
   }

#define CHECK_OVERLAY_CALLBACK(pBitmap)                         \
   OVERLAYCALLBACK lpfnOldStatusCB = NULL;                      \
   L_VOID       * pOldData = NULL;                              \
   L_UINT uOverLay_Flags = 0;                                   \
  if(IsOverlayCallBackEnabled())                                \
   {                                                           \
      LBase*   pObjectForStatusCB;                             \
      pObjectForStatusCB=(LBase*)pBitmap;                      \
      L_WRPGETOVERLAYCALLBACK(&lpfnOldStatusCB, &pOldData,&uOverLay_Flags);       \
      L_WRPSETOVERLAYCALLBACK(OverlayCS, pObjectForStatusCB,OVERLAY_CALLLOAD);       \
   }                                                           \
   else                                                        \
      RESET_OVERLAY_CALLBACK()

#define RESET_LOAD_INFO_CALLBACK()                             \
   {                                                           \
      L_WRPSETLOADINFOCALLBACK(0, NULL);                       \
   }

#define CHECK_LOAD_INFO_CALLBACK()                             \
   if(IsLoadInfoCallBackEnabled())                             \
   {                                                           \
      L_WRPSETLOADINFOCALLBACK(LoadInfoCS, this);              \
   }                                                           \
   else                                                        \
      RESET_LOAD_INFO_CALLBACK()

#define RESET_REDIRECT_IO()                                    \
   {                                                           \
      L_WRPREDIRECTIO(0,0,0,0,0,NULL);                            \
   }

#define CHECK_REDIRECT_IO()                                    \
   if(IsRedirectIOEnabled()&&(m_uEnableFlags&IO_REDIRECT_ALL)) \
   {                                                           \
      L_WRPREDIRECTIO(                                            \
                     (m_uEnableFlags&IO_OPEN) ? (REDIRECTOPEN)RedirectOpenCS :0,\
                     (m_uEnableFlags&IO_READ) ? (REDIRECTREAD)RedirectReadCS :0,\
                     (m_uEnableFlags&IO_WRITE)? (REDIRECTWRITE)RedirectWriteCS:0,\
                     (m_uEnableFlags&IO_SEEK) ? (REDIRECTSEEK)RedirectSeekCS :0,\
                     (m_uEnableFlags&IO_CLOSE)? (REDIRECTCLOSE)RedirectCloseCS:0,\
                     this                                      \
                   );                                          \
   }                                                           \
   else                                                        \
      RESET_REDIRECT_IO()

//--LEAD DEBUG MACROS----------------------------------------------------------
#define LErrorMessage(x,y,z) // change this please

#ifdef   LEAD_TRACE
   #define  LEAD_ASSERTION(bExp)    LErrorMessage((bExp),__FILE__,__LINE__);
#else
   #define  LEAD_ASSERTION(bExp) 
#endif

#define  LEAD_VALIDATION(bExp)      LErrorMessage((bExp),__FILE__,__LINE__);

//--LEAD ERROR HANDLING MACROS-------------------------------------------------
#define LEAD_ERROR(nError) LBase::RecordError(nError)

#define LEAD_ERROR_RETURN(nError)                                 \
   {                                                              \
      L_INT nTempError22=nError;                                  \
      LEAD_ERROR(nTempError22);                                   \
      return(nTempError22);                                       \
   } 

//--FOR BITMAPLIST CHECKING----------------------------------------------------
#define IF_BITMAPLIST_ITEM_RETURN_ERROR()                         \
   if(m_pBitmapList)                                              \
   {                                                              \
      if(!IsBitmapListLocked())                                   \
      {                                                           \
         m_pBitmapList=0;                                         \
         L_WRAPPER_MEMSET(GetHandle(),0,sizeof(BITMAPHANDLE));              \
      }                                                           \
      LEAD_ERROR_RETURN(WRPERR_BITMAPLIST_ITEM_OPERATION_ERROR);  \
   }

#define RESTORE_BITMAPLIST_ITEM()                                 \
   if(m_pBitmapList)                                              \
   {                                                              \
      L_WRPSETBITMAPLISTITEM(                                     \
                           m_pBitmapList->GetHandle(),            \
                           m_BitmapListItemIndex,                 \
                           GetHandle()                            \
                         );                                       \
      if(!IsBitmapListLocked())                                   \
      {                                                           \
         m_pBitmapList=0;                                         \
         L_WRAPPER_MEMSET(GetHandle(),0,sizeof(BITMAPHANDLE));              \
      }                                                           \
   }
//--FOR Rectangles-------------------------------------------------------------
#define GETRECTPTR(r)                                             \
   (((r.right-r.left)&&(r.bottom-r.top))?&r:NULL) 

//--FOR Dictionary-------------------------------------------------------------
#define LDictionary_GetBitmapCount() \
        BitmapDictionary.GetCount()

#define LDictionary_GetBitmap(nIndex) \
        (LBitmapBase *)BitmapDictionary.GetItem(nIndex)

#define LDictionary_DisconnectBitmapList(pLBitmaplist) \
        BitmapDictionary.DisconnectBitmapList(pLBitmaplist)

#define LDictionary_IsBitmap(pLBitmap)\
        BitmapDictionary.IsItem(pLBitmap)

//--FOR NOTIFYING PARENT WINDOW-------------------------------------------------
#define NOTIFY_PARENT(xMsg,xlParam)    \
   if(m_bCreateAsControl==TRUE)        \
      SendMessage(GetParent(m_hWnd),xMsg,L_GETWINDOWLONGPTR(m_hWnd,GWL_ID),xlParam)

//--FOR NOTIFYING PARENT WINDOW-------------------------------------------------
#ifndef max
   #define max(x,y)                    (((x)>(y))? (x):(y))
#endif

#ifndef min
   #define min(x,y)                    (((x)<(y))? (x):(y))
#endif

#ifndef abs
   #define abs(x)                      (((x)<0)  ?-(x):(x))
#endif

#define UNUSED_PARAMETER(XX)           XX=XX

#endif //_LEAD_WRAPPER_MACROS_H_
/*================================================================= EOF =====*/
