//==============================================================================
//
// File Name:	request.h
//
// Description:	This file contains the declaration of the CRequest class. This
//				class is used to create and manage a shared memory region for
//				submitting application requests across process boundries. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-19-2002	1.00		Original Release
//==============================================================================
#if !defined(__REQUEST_H__)
#define __REQUEST_H__

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <shared.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
typedef struct 
{
	long		lCommand;
	long		lPrimaryId;
	long		lSecondaryId;
	long		lTertiaryId;
	long		lQuaternaryId;
	long		lDisplayOrder;
	long		lBarcodeId;
	long		lPageNumber;
	short		sLineNumber;
	char		szCaseFolder[SHARED_MAXLEN_PATH];
	char		szSourceFileName[SHARED_MAXLEN_PATH];
	char		szSourceFilePath[SHARED_MAXLEN_PATH];
	char		szMediaId[SHARED_MAXLEN_MEDIA_ID];
	char		szBarcode[SHARED_MAXLEN_BARCODE];
}SRequest;

class CRequest : public CShared
{
	private:

		SRequest*			m_lpRequest;

	public:
	
							CRequest();
		virtual			   ~CRequest();

		//	Overloaded base class members
		BOOL				SetProperties(LPCSTR lpszName);
		BOOL				Open();
		BOOL				Close();
		long				Read(SRequest* lpRequest, BOOL bZeroWrites = TRUE);
		long				Write(SRequest* lpRequest, BOOL bZeroWrites = FALSE);

	protected:

};

#endif // __REQUEST_H__
