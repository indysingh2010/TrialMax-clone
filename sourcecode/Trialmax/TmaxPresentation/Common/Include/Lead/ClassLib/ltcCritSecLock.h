/*----------------------------------------------------------------------------+
| LEADTOOLS for Windows -                                                     |
| Copyright (c) 1991-2009 LEAD Technologies, Inc.                             |
| All Rights Reserved.                                                        |
|-----------------------------------------------------------------------------|
| PROJECT   : LEAD wrappers                                                   |
| FILE NAME : ltcCritSecLock.h                                                |
| DESC      : Class for automatically locking a critical section.             |
+----------------------------------------------------------------------------*/

#ifndef  _LEAD_CRITSECLOCK_H_
#define  _LEAD_CRITSECLOCK_H_

/*----------------------------------------------------------------------------+
| CLASSES DECLARATION                                                         |
+----------------------------------------------------------------------------*/

/*----------------------------------------------------------------------------+
| Class     : LCritSecLock                                                    |
| Desc      :                                                                 |
| Notes     :                                                                 |
+-----------------------------------------------------------------------------+
| Date      : June 2009                                                       |
+----------------------------------------------------------------------------*/
class LCritSecLock
{
   private:
      CRITICAL_SECTION *m_pCriticalSection;

   public:
      LCritSecLock(CRITICAL_SECTION *pCriticalSection);
      virtual ~LCritSecLock();
};

#endif //_LEAD_CRITSECLOCK_H_
/*================================================================= EOF =====*/
