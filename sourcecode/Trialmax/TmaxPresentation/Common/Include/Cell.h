//==============================================================================
//
// File Name:	cell.h
//
// Description:	This file contains the declarations of the CCell and CCells
//				classes.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-19-99	1.00		Original Release
//==============================================================================
#if !defined(__CELL_H__)
#define __CELL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <template.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Cell type identifiers
#define CELL_DOCUMENT				0
#define CELL_GRAPHIC				1
#define CELL_MOVIE					2
#define CELL_PLAYLIST				3
#define CELL_DESIGNATION			4
#define CELL_POWERPOINT				5

#define CELL_TYPECHAR_DOCUMENT		'T'
#define CELL_TYPECHAR_GRAPHIC		'G'
#define CELL_TYPECHAR_MOVIE			'M'
#define CELL_TYPECHAR_PLAYLIST		'L'
#define CELL_TYPECHAR_POWERPOINT	'P'
#define CELL_TYPECHAR_DESIGNATION	'D'

//	Parameter identifiers for string based initialization
#define PRIMARY_LABEL				"Primary"			//	Primary id
#define SECONDARY_LABEL				"Secondary"			//	Secondary id
#define TERTIARY_LABEL				"Tertiary"			//	Tertiary label
#define BARCODE_LABEL				"Barcode"			//	Barcode string
#define FOREIGN_BARCODE_LABEL		"Foreign"			//	Foreign barcode string
#define SOURCE_BARCODE_LABEL		"Source"			//	Source barcode string
#define GRAPHIC_BARCODE_LABEL		"Graphic"			//	Graphic barcode string
#define FILENAME_LABEL				"Filename"			//	Filename 
#define NAME_LABEL					"Name"				//	Name (description)
#define PATH_LABEL					"Path"				//	File path (no filename)
#define DEPONENT_LABEL				"Deponent"			//	Deponent name
#define TYPE_LABEL					"Type"				//	Type identifier
#define PAGE_LABEL					"Page"				//	Page number
#define PAGES_LABEL					"Pages"				//	Page count 
#define TREATMENTIMAGE_LABEL		"TreatmentImage"	//	Treatment source image
#define SIBLINGFILESPEC_LABEL		"SiblingPath"		//	Path to sibling treatment zap file
#define SIBLINGIMAGE_LABEL			"SiblingImage"		//	Sibling treatment source image
#define FLAGS_LABEL					"Flags"				//	Bit-packed flags
#define TEXTFIELD_LABEL				"TF"				//	Text field prefix

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CCellField : public CObject
{
	private:

	public:

		int				m_iId;
		CString			m_strText;

						CCellField(int iId = 0, LPCSTR lpszText = NULL);
					   ~CCellField();

	protected:

};

//	This class is used to manage a list of CCellField objects
class CCellFields : public CObList
{
	private:

	public:

								CCellFields();
		virtual				   ~CCellFields();

		BOOL					Add(CCellField* pField);
		void					Flush(BOOL bDelete);
		void					Remove(CCellField* pField, BOOL bDelete);
		POSITION				Find(CCellField* pField);
		CCellField*				Find(int iId);

		//	List iteration members
		CCellField*				First();
		CCellField*				Last();
		CCellField*				Next();
		CCellField*				Prev();

		void					Lock()	{ EnterCriticalSection(&m_Lock); }
		void					Unlock(){ LeaveCriticalSection(&m_Lock); }

	protected:

		CRITICAL_SECTION		m_Lock;
		POSITION				m_NextPos;
		POSITION				m_PrevPos;
};

class CCell : public CObject
{
	private:

	public:

		CString			m_aFixedFields[TEMPLATE_MAX_FIXED_FIELDS];
		CCellFields		m_aTextFields;

		CString			m_strString;
		CString			m_strPath;
		CString			m_strTreatmentImage;
		CString			m_strMasterId;
		CString			m_strSecondId;
		CString			m_strSiblingFileSpec;
		CString			m_strSiblingImage;
		long			m_lTertiaryId;
		long			m_lPage;
		long			m_lPages;
		long			m_lFlags;
		short			m_sType;

						CCell(LPSTR lpString = 0);
					   ~CCell();

		BOOL			SetFromString(LPSTR lpString);
		UINT			MsgBox(HWND hWnd);

		CString			GetText(int iFixedField, CTemplate* pTemplate = NULL);

	protected:

		BOOL			SetMember(LPCSTR lpParam);

};

//	This class is used to manage a list of CCell objects
class CCells : public CObList
{
	private:

	public:

								CCells();
		virtual				   ~CCells();

		BOOL					Add(CCell* pCell);
		BOOL					UsesPowerPoint();
		void					Flush(BOOL bDelete);
		void					Remove(CCell* pCell, BOOL bDelete);
		POSITION				Find(CCell* pCell);

		//	List iteration members
		CCell*					First();
		CCell*					Last();
		CCell*					Next();
		CCell*					Prev();

		void					Lock()	{ EnterCriticalSection(&m_Lock); }
		void					Unlock(){ LeaveCriticalSection(&m_Lock); }

		UINT					MsgBox(HWND hWnd);

	protected:

		CRITICAL_SECTION		m_Lock;
		POSITION				m_NextPos;
		POSITION				m_PrevPos;
};

#endif // !defined(__CELL_H__)
