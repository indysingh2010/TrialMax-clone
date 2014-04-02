//==============================================================================
//
// File Name:	filedata.h
//
// Description:	This file contains declarations of the CFileData and 
//				CFileDataList classes. These classes are used by the CFileCtrl
//				and CDriveCtrl classes to collect file information.
//
// Author:		Kenneth Moore
//
//==============================================================================
//	Date		Revision    Description
//	07-09-97	1.00		Original Release
//==============================================================================
#if !defined(__FILEDATA_H__)
#define __FILEDATA_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <afxtempl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Sort orders
#define FILESORT_NONE		0
#define FILESORT_NAME		1
#define FILESORT_SIZE		2
#define FILESORT_DATE		3
#define FILESORT_EXTENSION	4

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CFileData : public CObject
{
	private:
	
		CString				m_strFilespec;

	public:

		HIMAGELIST			m_hImageList;
		HICON				m_hIcon;
		CString				m_strParent;
		CString				m_strFileName;
		CString				m_strExtension;
		CString				m_strAlternate;
		CString				m_strDisplayName;
		CString				m_strTypeName;
		DWORD				m_dwAttributes;
		DWORD				m_dwFileSizeHigh;
		DWORD				m_dwFileSizeLow;
		FILETIME			m_ftCreated;
		FILETIME			m_ftLastWrite;
		FILETIME			m_ftLastAccess;
		int					m_iIcon;
		int					m_iSortField;

							CFileData(LPCSTR lpParent,
									  WIN32_FIND_DATA* pFindData = 0,
									  int iSortField = FILESORT_NONE);
							CFileData(CFileData& FD);
		BOOL				GetShellInfo(BOOL bSmallIcon, BOOL bOpenIcon = 0);
		BOOL				GetIconIndex(BOOL bSmallIcon, BOOL bOpenIcon = 0);
		BOOL				GetIconHandle(BOOL bSmallIcon,BOOL bOpenIcon = 0);
		LPCSTR				GetFilespec();

		BOOL				operator < (const CFileData& fdCompare);
		BOOL				operator == (const CFileData& fdCompare);

	#ifdef _DEBUG
	void Show(HWND hParent = 0);
	#endif

	protected:

};

class CFileDataList : public CObList
{
	private:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

	public:

		BOOL				m_bAscending;


							CFileDataList(BOOL bAscending = TRUE,
										  int iBlockSize = 10);

		void				Flush(BOOL bDeleteAll);
		void				Add(CFileData* pData);

		//	List iteration routines
		CFileData*			GetFirst();
		CFileData*			GetLast();
		CFileData*			GetNext();
		CFileData*			GetPrev();

	protected:

};


#endif // !defined(__FILEDATA_H__)
