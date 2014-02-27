//==============================================================================
//
// File Name:	template.h
//
// Description:	This file contains the declarations of the CTemplate and 
//				CTemplates classes. These classes are used to manage page
//				templates for barcode printing.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-25-98	1.00		Original Release
//==============================================================================
#if !defined(__TEMPLATE_H__)
#define __TEMPLATE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmini.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Indices of fields in the array
#define TEMPLATE_NAME						0
#define TEMPLATE_BARCODE					1
#define TEMPLATE_GRAPHIC					2
#define TEMPLATE_FILENAME					3
#define TEMPLATE_PAGENUM					4
#define TEMPLATE_DEPONENT					5
#define TEMPLATE_FOREIGN_BARCODE			6
#define TEMPLATE_SOURCE_BARCODE				7
#define TEMPLATE_MAX_FIXED_FIELDS			8

//	Template bitmask identifiers
#define TEMPLATE_BITMASK_NAME				0x00000001L
#define TEMPLATE_BITMASK_BARCODE			0x00000002L
#define TEMPLATE_BITMASK_GRAPHIC			0x00000004L
#define TEMPLATE_BITMASK_FILENAME			0x00000008L
#define TEMPLATE_BITMASK_PAGENUM			0x00000010L
#define TEMPLATE_BITMASK_DEPONENT			0x00000020L
#define TEMPLATE_BITMASK_IMAGE				0x00000040L
#define TEMPLATE_BITMASK_CELL_BORDER		0x00000080L
#define TEMPLATE_BITMASK_AUTO_ROTATE		0x00000100L
#define TEMPLATE_BITMASK_FOREIGN_BARCODE	0x00000200L
#define TEMPLATE_BITMASK_SOURCE_BARCODE		0x00000400L

//	Alignment identifiers
#define TEMPLATE_ALIGNCENTER				0
#define TEMPLATE_ALIGNTOP					1
#define TEMPLATE_ALIGNBOTTOM				2
#define TEMPLATE_ALIGNLEFT					3
#define	TEMPLATE_ALIGNRIGHT					4

//	Orientation identifiers
#define TEMPLATE_ORIENTATION_UNKNOWN		0
#define TEMPLATE_ORIENTATION_PORTRAIT		1
#define TEMPLATE_ORIENTATION_LANDSCAPE		2

//	INI line identifiers for template specifications
#define AUTOROTATE_LINE						"AutoRotate"
#define LANDSCAPE_LINE						"Landscape"
#define	TOPMARGIN_LINE						"TopMargin"
#define LEFTMARGIN_LINE						"LeftMargin"
#define ROWS_LINE							"Rows"
#define COLUMNS_LINE						"Columns"
#define CELLHEIGHT_LINE						"CellHeight"
#define CELLWIDTH_LINE						"CellWidth"
#define CELLSPACEWIDTH_LINE					"CellSpaceWidth"
#define CELLSPACEHEIGHT_LINE				"CellSpaceHeight"
#define CELLPADWIDTH_LINE					"CellPadWidth"
#define CELLPADHEIGHT_LINE					"CellPadHeight"
#define IMAGEPADWIDTH_LINE					"ImagePadWidth"
#define IMAGEPADHEIGHT_LINE					"ImagePadHeight"
#define IMAGEENABLE_LINE					"ImageEnable"
#define PRINTBORDER_LINE					"BorderEnable"
#define FIELDFONT_LINE						"Font"
#define FIELDENABLE_LINE					"Enable"
#define FIELDVALIGN_LINE					"VAlign"
#define FIELDHALIGN_LINE					"HAlign"
#define FIELDVPOS_LINE						"VPos"
#define FIELDHPOS_LINE						"HPos"
#define FIELDNAME_LINE						"Name"
#define FIELDTEXT_LINE						"Text"

//	Default template values
#define DEFAULT_TEMPLATE_AUTOROTATE			""
#define DEFAULT_TEMPLATE_LANDSCAPE			""
#define DEFAULT_TEMPLATE_TOPMARGIN			0.00f
#define DEFAULT_TEMPLATE_LEFTMARGIN			0.00f
#define DEFAULT_TEMPLATE_ROWS				2
#define DEFAULT_TEMPLATE_COLUMNS			2
#define DEFAULT_TEMPLATE_CELLHEIGHT			2.0f
#define DEFAULT_TEMPLATE_CELLWIDTH			2.0f
#define DEFAULT_TEMPLATE_CELLSPACEWIDTH		1.0f
#define DEFAULT_TEMPLATE_CELLSPACEHEIGHT	1.0f
#define DEFAULT_TEMPLATE_CELLPADWIDTH		0.25f
#define DEFAULT_TEMPLATE_CELLPADHEIGHT		0.25f
#define DEFAULT_TEMPLATE_IMAGEPADWIDTH		0.125f
#define DEFAULT_TEMPLATE_IMAGEPADHEIGHT		0.125f
#define DEFAULT_TEMPLATE_IMAGEENABLE		TRUE
#define DEFAULT_TEMPLATE_PRINTBORDER		TRUE
#define DEFAULT_TEMPLATE_FIELDFONT			8
#define DEFAULT_TEMPLATE_FIELDVALIGN		'C'
#define DEFAULT_TEMPLATE_FIELDHALIGN		'C'
#define DEFAULT_TEMPLATE_FIELDVPOS			0.25f
#define DEFAULT_TEMPLATE_FIELDHPOS			0.25f

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations of classes contained in this file
class CTemplate;
class CTemplates;

class CTemplateField : public CObject
{
	private:

	public:

		int				m_iId;
		short			m_sFont;
		short			m_sVAlign;
		short			m_sHAlign;
		float			m_fHPos;
		float			m_fVPos;
		BOOL			m_bEnable;
		BOOL			m_bDefault;
		BOOL			m_bPrint;
		CString			m_strName;
		CString			m_strPrefix;
		CString			m_strText;

						CTemplateField(int iId = 0, LPCSTR lpszPrefix = NULL, LPCSTR lpszName = NULL);
						CTemplateField(CTemplateField& rSource);
					   ~CTemplateField();

		void			Copy(CTemplateField& rSource);
		void			Reset();
		void			Initialize(int iId, LPCSTR lpszPrefix, LPCSTR lpszName = NULL);
		BOOL			Read(CTMIni& rIni);

		UINT			MsgBox(HWND hWnd);

	protected:

};

//	This class is used to manage a list of CTemplateField objects
class CTemplateFields : public CObList
{
	private:

	public:

								CTemplateFields();
		virtual				   ~CTemplateFields();

		BOOL					Add(CTemplateField* pField);
		void					Flush(BOOL bDelete);
		void					Remove(CTemplateField* pField, BOOL bDelete);
		POSITION				Find(CTemplateField* pField);
		CTemplateField*			Find(LPCSTR lpszName);
		CTemplateField*			Find(int iId);

		//	List iteration members
		CTemplateField*			First();
		CTemplateField*			Last();
		CTemplateField*			Next();
		CTemplateField*			Prev();

		void					Lock()	{ EnterCriticalSection(&m_Lock); }
		void					Unlock(){ LeaveCriticalSection(&m_Lock); }

	protected:

		CRITICAL_SECTION		m_Lock;
		POSITION				m_NextPos;
		POSITION				m_PrevPos;
};

//	This class is used to manage a page template for printing barcodes
class CTemplate : public CObject
{
	private:

	public:

		//	Template configuration options
		CTemplateField	m_aFixedFields[TEMPLATE_MAX_FIXED_FIELDS];
		CTemplateFields	m_aTextFields;
		CString			m_strDescription;
		float			m_fTopMargin;
		float			m_fLeftMargin;
		float			m_fCellHeight;
		float			m_fCellWidth;
		float			m_fCellSpaceWidth;
		float			m_fCellSpaceHeight;
		float			m_fCellPadWidth;
		float			m_fCellPadHeight;
		float			m_fImagePadWidth;
		float			m_fImagePadHeight;
		BOOL			m_bImageEnable;
		BOOL			m_bDefaultImage;
		BOOL			m_bDefaultBorder;
		BOOL			m_bAutoRotateEnable;
		BOOL			m_bDefaultAutoRotate;
		short			m_sRows;
		short			m_sColumns;
		short			m_sOrientation;

		//	Runtime job options
		BOOL			m_bPrintImage;
		BOOL			m_bPrintBorder;
		BOOL			m_bPrintFullPath;
		BOOL			m_bPageAsSeries;
		BOOL			m_bAutoRotate;

						CTemplate();
						CTemplate(CTemplate& rSource);
					   ~CTemplate();

		int				MsgBox(HWND hParent);
		void			Copy(CTemplate& rSource);
		BOOL			ReadFile(CTMIni& rIni, LPCSTR lpSection);

		BOOL			GetFieldEnabled(int iFixedField);
		BOOL			GetDefault(int iFixedField);
		BOOL			GetPrintEnabled(int iFixedField);
		void			SetPrintEnabled(int iFixedField, BOOL bEnabled);

		long			GetEnabledMask();
		long			GetDefaultMask();

		BOOL			operator < (const CTemplate& Compare);
		BOOL			operator == (const CTemplate& Compare);
	
	protected:

};

//	This class is used to manage a list of CTemplate objects
class CTemplates : public CObList
{
	private:

		POSITION		m_Pos;

	public:

						CTemplates();
					   ~CTemplates();

		void			Flush(BOOL bDeleteAll);
		void			Add(CTemplate* pTemplate);
		void			Remove(CTemplate* pTemplate, BOOL bDelete);
		void			MsgBox(HWND hParent);
		POSITION		Find(CTemplate* pTemplate);
		CTemplate*		Find(LPCSTR lpszTemplate);
		BOOL			ReadFile(LPCSTR lpFilename, LPCSTR lpSection, BOOL bFlush);

		//	List iteration routines
		CTemplate*		GetFirstTemplate();
		CTemplate*		GetLastTemplate();
		CTemplate*		GetNextTemplate();
		CTemplate*		GetPrevTemplate();

	protected:

};

#endif // !defined(__TEMPLATE_H__)
