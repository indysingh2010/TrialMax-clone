//==============================================================================
//
// File Name:	response.h
//
// Description:	This file contains the declaration of the CResponse class. This
//				class is used to create and manage a shared memory region for
//				submitting application command responses across process boundries. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-19-2002	1.00		Original Release
//==============================================================================
#if !defined(__RESPONSE_H__)
#define __RESPONSE_H__

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
	long		lError;
	long		lCommand;
	long		lPrimaryId;
	long		lSecondaryId;
	long		lTertiaryId;
	long		lQuaternaryId;
	long		lDisplayOrder;
	long		lBarcodeId;
	char		szCaseFolder[SHARED_MAXLEN_PATH];
	char		szSourceFileName[SHARED_MAXLEN_PATH];
	char		szSourceFilePath[SHARED_MAXLEN_PATH];
	char		szMediaId[SHARED_MAXLEN_MEDIA_ID];
	char		szBarcode[SHARED_MAXLEN_BARCODE];
}SResponse;

class CResponse : public CShared
{
	private:

		SResponse*			m_lpResponse;

	public:
	
							CResponse();
		virtual			   ~CResponse();

		BOOL				SetProperties(LPCSTR lpszName);
		BOOL				Open();
		BOOL				Close();
		long				Read(SResponse* lpResponse, BOOL bZeroWrites = TRUE);
		long				Write(SResponse* lpResponse, BOOL bZeroWrites = FALSE);

	protected:

};

#endif // __RESPONSE_H__
