/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcDictn.h                                                      |
| DESC      :                                                                 |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_DICTIONARY_H_
#define  _LEAD_DICTIONARY_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/
#define ALLOCATED_DICTIONARY_SIZE    50

typedef struct _EXTDICTIONARY
{
   L_UINT            uStructSize;
   CRITICAL_SECTION  csLock;
} EXTDICTIONARY, *pEXTDICTIONARY;

/* Macro that prevents multiple threads from modifying class members at the same time */
#define GET_CRITICAL_SECTION_POINTER   (m_extLDictionary ? &(((pEXTDICTIONARY)m_extLDictionary)->csLock) : (CRITICAL_SECTION*)NULL)

/*----------------------------------------------------------------------------+
| Class     : LDictionary                                                     |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 1998                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LDictionary :public LBase
{
   public:
      L_VOID *m_extLDictionary;
      
   protected:
      L_VOID                    *m_hDictionary;
      L_VOID      *      *       m_pItem;
      L_INT                      m_nCount;
      L_INT                      m_nSize;

   public : 
      LDictionary();
      virtual ~LDictionary();

      L_INT                      GetCount();
      L_VOID      *              GetItem(L_INT nIndex);
      L_VOID                     AddItem(L_VOID      * pItem);
      L_VOID                     RemoveItem(L_VOID      * pItem);
      L_BOOL                     IsItem(L_VOID      * pItem);
};

/*----------------------------------------------------------------------------+
| Class     : LBitmapDictionary                                               |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : September 1998                                                  |
+----------------------------------------------------------------------------*/
class LWRP_EXPORT LBitmapDictionary :public LDictionary
{
   public:
      L_VOID *m_extLBitmapDictionary;
      
   public : 
      L_VOID               DisconnectBitmapList(LBitmapList      * pLBitmaplist);
};

#endif //_LEAD_DICTIONARY_H_
/*================================================================= EOF =====*/



