//==============================================================================
//
// File Name:	transcpt.h
//
// Description:	This file contains the declarations of the CTranscript and
//				CTranscripts classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-20-00	1.00		Original Release
//==============================================================================
#if !defined(__TRANSCPT_H__)
#define __TRANSCPT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CIXMLDOMDocument;
class CDesignation;

class CTranscript : public CObject
{
	private:

		CIXMLDOMDocument*	m_pXmlDocument;

	public:

		long				m_lTranscriptId;
		CString				m_strBaseFilename;
		CString				m_strCtxExtension;
		CString				m_strDbExtension;
		CString				m_strTranscriptName;
		CString				m_strRelativePath;
		CString				m_strDate;

		//	These members added for .NET
		long				m_lAttributes;
		long				m_lPrimaryMediaId;
		long				m_lFirstPL;
		long				m_lLastPL;
		long				m_lAliasId;
		CString				m_strAltBarcode;
		CString				m_strFilename;
		CString				m_strXmlFileSpec;
		BOOL				m_bLinked;
		short				m_sLinesPerPage;


							CTranscript();
		virtual			   ~CTranscript();

		//	XML file management operations
		void				Close();
		BOOL				Open(LPCSTR lpszXmlFileSpec);
		BOOL				GetText(long lSegmentId, CDesignation* pDesignation);

		UINT				MsgBox(HWND hWnd, LPCSTR lpszTitle);

	protected:

		CIXMLDOMDocument*	CreateXmlDocument();
};

//	Objects of this class are used to manage a list of CTranscript objects
class CTranscripts : public CObList
{
	private:

	public:

								CTranscripts();
		virtual				   ~CTranscripts();

		virtual BOOL			Add(CTranscript* pTranscript);
		virtual void			Flush(BOOL bDelete);
		virtual void			Remove(CTranscript* pTranscript, BOOL bDelete);
		virtual POSITION		Find(CTranscript* pTranscript);
		virtual CTranscript*	Find(long lId);
		virtual CTranscript*	FindByPrimary(long lPrimary);

		//	List iteration members
		virtual	CTranscript*	First();
		virtual	CTranscript*	Last();
		virtual	CTranscript*	Next();
		virtual	CTranscript*	Prev();

	protected:

		POSITION				m_NextPos;
		POSITION				m_PrevPos;

};

#endif // !defined(__TRANSCPT_H__)
