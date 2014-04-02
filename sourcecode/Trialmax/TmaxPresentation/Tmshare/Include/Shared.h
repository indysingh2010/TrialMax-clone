//==============================================================================
//
// File Name:	shared.h
//
// Description:	This file contains the declaration of the CShared class. This
//				class is used to create and manage a shared memory region 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-11-2002	1.00		Original Release
//==============================================================================
#if !defined(__SHARED_H__)
#define __SHARED_H__

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define SHARED_MAXLEN_PATH		256
#define SHARED_MAXLEN_MEDIA_ID	256
#define SHARED_MAXLEN_BARCODE	(SHARED_MAXLEN_MEDIA_ID + 32)

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
typedef struct
{
	long	lWrites;
}SHeader;

class CShared
{
	private:

	public:
	
							CShared();
		virtual			   ~CShared();

		virtual LPBYTE		GetShared(){ return m_lpShared; }
		virtual LPCSTR		GetName(){ return m_strName; }
		virtual BOOL		GetFirst(){ return m_bFirst; }
		virtual BOOL		GetOpened(){ return (m_lpShared != 0); }
		virtual DWORD		GetSize(){ return m_dwSize; }
		virtual long		GetWrites();
	
		virtual BOOL		SetProperties(LPCSTR lpszName, DWORD dwSize);
		virtual BOOL		Open();
		virtual BOOL		Close();
		virtual long		Write(LPBYTE lpBuffer, BOOL bZeroWrites = FALSE);
		virtual long		Read(LPBYTE lpBuffer, BOOL bZeroWrites = TRUE);

	protected:
	
		LPBYTE				m_lpShared;	
		HANDLE				m_hShared;	
		HANDLE				m_hMutex;	
		BOOL				m_bFirst;	
		CString				m_strName;
		DWORD				m_dwSize;

		BOOL				Lock();
		BOOL				Unlock();
};

#endif // __SHARED_H__
