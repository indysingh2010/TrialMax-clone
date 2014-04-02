//==============================================================================
//
// File Name:	printer.cpp
//
// Description:	This file contains member functions of the CTMPrinter class.
//
// See Also:	printer.h 
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	04-29-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <printer.h>
#include <winspool.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CTMPrinter::Abort()
//
// 	Description:	This function is called to abort a print job.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::Abort()
{
	//	Do we have an ongoing print job?
	if(m_pDC != 0)
	{
		m_pDC->AbortDoc();

		//	Release the DC
		FreeDC();
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::Attach(LPCSTR lpName)
//
// 	Description:	This function attach to the specified printer.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::Attach()
{
	//	Are we already attached?
	if((m_strAttached.GetLength() > 0) && (m_hDevMode != 0))
	{
		//	Are we attached to the correct device?
		if(m_strAttached.CompareNoCase(m_strPrinter) == 0)
			return TRUE;
	}

	//	Should we use the default printer?
	if(m_strPrinter.GetLength() == 0)
	{
		if(!GetDefault(m_strPrinter))
		{
			HandleError(TMPRINTER_ERROR_NO_DEFAULT_PRINTER);
			return FALSE;
		}
	
	}

	//	Get the driver information for the specified printer
	if(GetDriver(m_strPrinter))
	{
		m_strAttached = m_strPrinter;
		
		//	Get the device mode for this printer
		if(GetDevMode() == FALSE)
		{
			HandleError(TMPRINTER_ERROR_GET_DEVICE_MODE_FAILED, (long)((LPCSTR)m_strPrinter));
			m_strAttached.Empty();
			return FALSE;
		}
		else
		{
			return TRUE;
		}

	}
	else
	{
		HandleError(TMPRINTER_ERROR_DRIVER_NOT_FOUND, (long)((LPCSTR)m_strPrinter));
		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CTMPrinter::CrackDevNames()
//
// 	Description:	This function is called to crack the device names structure
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::CrackDevNames() 
{
	char*		pString = NULL;
	DEVNAMES*	pDevNames = NULL;

	ASSERT(m_hDevNames);
	pDevNames = (DEVNAMES*)GlobalLock(m_hDevNames);
	ASSERT(pDevNames);

	if((pString = (char*)pDevNames) != NULL)
	{
		m_strDriver = (pString + pDevNames->wDriverOffset);
		m_strDevice = (pString + pDevNames->wDeviceOffset);
		m_strPort = (pString + pDevNames->wOutputOffset);	
	}

	GlobalUnlock(pDevNames);
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::CreateDC()
//
// 	Description:	This function is called to create the device context for the
//					print job.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::CreateDC() 
{
	char		szDriver[512];
	char*		pToken;
	DEVMODE*	pDevMode = NULL;
	BOOL		bSuccessful = FALSE;

	ASSERT(m_hDevMode != NULL);
	pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
	ASSERT(pDevMode != NULL);

	//	Free the existing DC
	FreeDC();

	//	We need to strip the file extension from the driver name
	lstrcpyn(szDriver, m_strDriver, sizeof(szDriver));
	if((pToken = strrchr(szDriver, '.')) != 0)
		*pToken = 0;

	//	Allocate a new device context
	m_pDC = new CDC();
	ASSERT(m_pDC);

	//	Create the device context.
	if(m_pDC->CreateDC(szDriver, m_strDevice, m_strPort, pDevMode))
	{
		m_pDC->SetMapMode(MM_TEXT);
		m_pDC->SetBkMode(OPAQUE);
		bSuccessful = TRUE;
	}
	else
	{
		delete m_pDC;
		m_pDC = NULL;

		HandleError(TMPRINTER_ERROR_CREATE_DC_FAILED, (long)((LPCSTR)szDriver));
		bSuccessful = FALSE;
	}

	GlobalUnlock(pDevMode);
	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::CTMPrinter()
//
// 	Description:	This is the constructor for CTMPrinter objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMPrinter::CTMPrinter()
{
	m_hDevMode = NULL;
	m_hDevNames = NULL;
	m_pDC = 0;
	m_iXDpi = 0;
	m_iYDpi = 0;
	m_iCopies = 1;
	m_bCollate = FALSE;
	m_fLeftMargin = 0.0f;
	m_fRightMargin = 0.0f;
	m_fTopMargin = 0.0f;
	m_fBottomMargin = 0.0f;
	m_iOrientation = TMPRINTER_ORIENTATION_DEVICE;
	m_strAttached.Empty();
	m_strDevice.Empty();
	m_strDriver.Empty();
	m_strPort.Empty();
	ZeroMemory(&m_rcMax, sizeof(m_rcMax));

	//	Initialize the error information
	HandleError(TMPRINTER_ERROR_NONE);
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::~CTMPrinter()
//
// 	Description:	This is the destructor for CTMPrinter objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMPrinter::~CTMPrinter()
{
	//	Make sure all the resources have been released
	Free();
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::End()
//
// 	Description:	This function is called to end a print job.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::End()
{
	//	Do we have an ongoing print job?
	if(m_pDC != 0)
	{
		m_pDC->EndDoc();

		//	Release the DC
		FreeDC();
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::EndPage()
//
// 	Description:	This function is called to end the current page
//
// 	Returns:		TRUE if the job should continue
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::EndPage()
{
	//	Do we have an ongoing print job?
	if(m_pDC != 0)
	{
		//	End the page
		return (m_pDC->EndPage() >= 0);
	}
	else
	{
		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::EnumPrinters()
//
// 	Description:	This function is called to enumerate the list of installed 
//					printers
//
// 	Returns:		A list of printers that meets the specified criteria
//
//	Notes:			None
//
//==============================================================================
CObList* CTMPrinter::EnumPrinters(DWORD dwFlags)
{
	OSVERSIONINFO		osVersion;
	DWORD				dwBytes;
	DWORD				dwReturn;
	LPPRINTER_INFO_4	pInfo4;
	LPPRINTER_INFO_5	pInfo5;
	CString*			pString = NULL;
	CObList*			pPrinters = new CObList();

	//	Get the Windows version information
	memset(&osVersion, 0, sizeof(osVersion));
	osVersion.dwOSVersionInfoSize = sizeof(osVersion);
	GetVersionEx(&osVersion);

	//	Is this NT/2000/XP ?
	if(osVersion.dwPlatformId == VER_PLATFORM_WIN32_NT)
	{
		//	First determine how many bytes we need
		::EnumPrinters(dwFlags, 0, 4, 0, 0, &dwBytes, &dwReturn);

		//	Allocate the memory required for the printer information
		if((pInfo4 = (LPPRINTER_INFO_4)LocalAlloc(LPTR, dwBytes)) != 0)
		{
			//	Enumerate the printers
			if(::EnumPrinters(dwFlags, 0, 4, (LPBYTE)pInfo4, dwBytes, &dwBytes, &dwReturn))
			{
				for(int i = 0; i < (int)dwReturn; i++)
				{
					pString = new CString(pInfo4[i].pPrinterName);
					pPrinters->AddTail((CObject*)pString);
				}
			}

			//	Deallocate the dynamic memory
			LocalFree(LocalHandle(pInfo4));
		}
	
	}
	else
	{
		//	First determine how many bytes we need
		::EnumPrinters(dwFlags, 0, 5, 0, 0, &dwBytes, &dwReturn);

		//	Allocate the memory required for the printer information
		if((pInfo5 = (LPPRINTER_INFO_5)LocalAlloc(LPTR, dwBytes)) != 0)
		{
			//	Enumerate the printers
			if(::EnumPrinters(dwFlags, 0, 5, (LPBYTE)pInfo5, dwBytes, &dwBytes, &dwReturn))
			{
				for(int i = 0; i < (int)dwReturn; i++)
				{
					pString = new CString(pInfo5[i].pPrinterName);
					pPrinters->AddTail((CObject*)pString);
				}
			}

			//	Deallocate the dynamic memory
			LocalFree(LocalHandle(pInfo5));
		}
	
	}

	if(pPrinters->GetCount() == 0)
	{
		delete pPrinters;
		pPrinters = NULL;
	}

	return pPrinters;

}

//==============================================================================
//
// 	Function Name:	CTMPrinter::Free()
//
// 	Description:	This function is called to free all resources
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::Free() 
{
	//	Deallocate the existing device mode and device names structures
	FREE_HANDLE(m_hDevMode);
	FREE_HANDLE(m_hDevNames);

	//	Reset the device names
	m_strDevice.Empty();
	m_strDriver.Empty();
	m_strPort.Empty();
	m_strAttached.Empty();
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::FreeDC()
//
// 	Description:	This function is called to free all resources
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::FreeDC() 
{
	if(m_pDC)
	{
		m_pDC->DeleteDC();
		delete m_pDC;
		m_pDC = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::GetNamedDC()
//
// 	Description:	This function is called to get a device context for the
//					specified printer
//
// 	Returns:		A pointer to the device context for the specified printer
//
//	Notes:			None
//
//==============================================================================
CDC* CTMPrinter::GetNamedDC(LPCSTR lpszPrinter)
{
	//	Did the caller specify a printer?
	if((lpszPrinter == NULL) || (lstrlen(lpszPrinter) == 0))
		return NULL;

	SetName(lpszPrinter);

	//	Make sure we are attached to the specified printer
	if(!Attach()) return NULL;

	CreateDC();

	return m_pDC;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::GetNamedDevMode()
//
// 	Description:	This function is called to get a device mode structure
//					for the specified printer
//
// 	Returns:		A handle to the DEVMODE for the specified printer
//
//	Notes:			None
//
//==============================================================================
HGLOBAL CTMPrinter::GetNamedDevMode(LPCSTR lpszPrinter)
{
	//	Did the caller specify a printer?
	if((lpszPrinter == NULL) || (lstrlen(lpszPrinter) == 0))
		return NULL;

	SetName(lpszPrinter);

	//	Attach to the requested printer
	Attach();

	return m_hDevMode;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::GetDefault()
//
// 	Description:	This function is called to get the name of the default 
//					printer.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::GetDefault(CString& rName) 
{
	char szName[256];

	if(GetDefault(szName, sizeof(szName)) == TRUE)
	{	
		rName = szName;
		return TRUE;
	}
	else
	{
		rName.Empty();
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::GetDefault()
//
// 	Description:	This function is called to get the name of the default
//					printer.
//
// 	Returns:		TRUE if found
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::GetDefault(LPSTR lpName, int iLength)
{
	OSVERSIONINFO	osVersion;
	HGLOBAL			hInfo = 0;
	PRINTER_INFO_2*	pInfo = 0;
	DWORD			dwBytes;
	DWORD			dwReturned;
	char			szPrinter[1024];
	char*			pToken;
	char*			pNext;

	ASSERT(lpName);
	ASSERT(iLength > 0);
	
	if((lpName != 0) && (iLength > 0))
	{
		memset(szPrinter, 0, sizeof(szPrinter));
		memset(lpName, 0, iLength);
	}
	else
	{
		return FALSE;
	}

	//	Get the Windows version information
	memset(&osVersion, 0, sizeof(osVersion));
	osVersion.dwOSVersionInfoSize = sizeof(osVersion);
	GetVersionEx(&osVersion);

	//	Is this NT/2000 ?
	if(osVersion.dwPlatformId == VER_PLATFORM_WIN32_NT)
	{
		//	Get the default printer from the registry
		GetProfileString("windows", "device", ",,,", szPrinter, sizeof(szPrinter));

		//	Parse the registry string: printername,drivername,portname
		if((pToken = strtok_s(szPrinter, ",", &pNext)) != 0)
		{	
			lstrcpyn(lpName, pToken, iLength);
		}
		else
		{
			return FALSE;
		}
			
	}
	else
	{
		//	Check to see how large a buffer we need to retrieve the printer
		//	information
		::EnumPrinters(PRINTER_ENUM_DEFAULT, NULL, 2, NULL, 0, &dwBytes, &dwReturned);

		if(dwBytes == 0)
		{
			return FALSE;
		}
		else
		{
			if((hInfo = GlobalAlloc(GHND, dwBytes)) != NULL)
				pInfo = (PRINTER_INFO_2*)GlobalLock(hInfo);
			if(pInfo == 0)
				return FALSE;

			if(!::EnumPrinters(PRINTER_ENUM_DEFAULT, NULL, 2, 
							 (LPBYTE)pInfo, dwBytes, &dwBytes, &dwReturned))
			{
				GlobalUnlock(pInfo);
				GlobalFree(hInfo);
				return FALSE;
			}
			else
			{
				//	Copy the printer name
				lstrcpyn(lpName, pInfo->pPrinterName, iLength);

				GlobalUnlock(pInfo);
				GlobalFree(hInfo);
			}
		}
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::GetDevMode()
//
// 	Description:	This function is called to get the device mode information
//					for the specified printer.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::GetDevMode()
{
	HANDLE		hPrinter = NULL;
	char		szPrinter[512];
	long		lSize;
	BOOL		bSuccessful = FALSE;
	DEVMODE*	pDevMode = NULL;

	//	Deallocate the existing device mode 
	FREE_HANDLE(m_hDevMode);

	//	Transfer the printer name to a working buffer
	//
	//	NOTE:	
	lstrcpyn(szPrinter, m_strAttached, sizeof(szPrinter));

	//	Open the specified printer device
	if(!OpenPrinter(szPrinter, &hPrinter, NULL))
		return FALSE;

	//	Determine the size of the buffer we need for the device mode 
	if((lSize = DocumentProperties(NULL, hPrinter, szPrinter, NULL, NULL, 0)) <= 0)
	{
		ClosePrinter(hPrinter);
		return FALSE;
	}
         
	//	Allocate a new buffer to hold the device mode data
	m_hDevMode = GlobalAlloc(GHND, lSize);
	ASSERT(m_hDevMode);
	pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
	ASSERT(pDevMode);	

	//	Get the default device mode data for this printer
	bSuccessful = (DocumentProperties(NULL, hPrinter, szPrinter, pDevMode, 
									  NULL, DM_OUT_BUFFER) >= 0);

	GlobalUnlock(pDevMode);

	if(bSuccessful == FALSE)
		FREE_HANDLE(m_hDevMode)

	ClosePrinter(hPrinter);
	
	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::GetDevModeProperties()
//
// 	Description:	This function is called to update the local class members
//					with the values stored in the device mode structure
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::GetDevModeProperties()
{
	DEVMODE* pDevMode = NULL;

	ASSERT(m_hDevMode != NULL);
	if(m_hDevMode == NULL) return;

	pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
	ASSERT(pDevMode != 0);

	//	Does this device allow us to set the orientation?
	if(pDevMode->dmFields & DM_ORIENTATION)
	{
		//	Are we supposed to be using the device orientation?
		if(m_iOrientation != TMPRINTER_ORIENTATION_DEVICE)
		{
			if(pDevMode->dmOrientation == DMORIENT_LANDSCAPE)
				m_iOrientation = TMPRINTER_ORIENTATION_LANDSCAPE;
			else
				m_iOrientation = TMPRINTER_ORIENTATION_PORTRAIT;

		}

	}

	//	Does this device allow us to set the number of copies?
	if(pDevMode->dmFields & DM_COPIES)
	{
		m_iCopies = pDevMode->dmCopies;
	}

	//	Does this device allow us to set the collate option?
	if(pDevMode->dmFields & DM_COLLATE)
	{
		m_bCollate = (pDevMode->dmCollate != 0);
	}

}

//==============================================================================
//
// 	Function Name:	CTMPrinter::GetDriver()
//
// 	Description:	This method is called to get the device driver information
//					for the specified printer
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::GetDriver(LPCSTR lpName) 
{
	DWORD				dwBytes;
	DWORD				dwReturn;
	LPPRINTER_INFO_2	pEnum;
	LPPRINTER_INFO_2	pPrinter = 0;
	DWORD				dwDriverName;
	DWORD				dwPrinterName;
	DWORD				dwPortName;
	DWORD				dwDevNames;
	int					iOffset;
	DEVNAMES*			pDevNames = NULL;

	//	First determine how many bytes we need to check all the printers
	::EnumPrinters(TMPRINTER_ENUM_FLAGS, 0, 2, 0, 0, &dwBytes,
	             &dwReturn);

	//	Allocate the memory required for the printer information
	if((pEnum = (LPPRINTER_INFO_2)LocalAlloc(LPTR, dwBytes)) == 0)
		return FALSE;

	//	Enumerate the printers
	if(::EnumPrinters(TMPRINTER_ENUM_FLAGS, 0, 2, (LPBYTE)pEnum, dwBytes,
	                &dwBytes, &dwReturn))
	{
		//	Check each printer
		for(int i = 0; i < (int)dwReturn; i++)
		{
			if(lstrcmpi(pEnum[i].pPrinterName, lpName) == 0)
			{
				pPrinter = &(pEnum[i]);
				break;
			}
		}
	}

	//	Were we unable to find the printer?
	if(pPrinter == 0)
	{
		//	Deallocate the dynamic memory
		LocalFree(LocalHandle(pEnum));
		return FALSE;
	}

	//	Free the existing device structures
	Free();

	//	Save the device name, driver name, and port name
	if(pPrinter->pPrinterName)
		m_strDevice = pPrinter->pPrinterName;
	if(pPrinter->pDriverName)
		m_strDriver = pPrinter->pDriverName;
	if(pPrinter->pPortName)
		m_strPort = pPrinter->pPortName;

	//	Compute the size required for the device name information
	if((pPrinter->pDriverName != 0) && (pPrinter->pPrinterName != 0) &&
	   (pPrinter->pPortName != 0))
	{
		dwDriverName = (lstrlen(pPrinter->pDriverName) + 1) * sizeof(TCHAR);
		dwPrinterName = (lstrlen(pPrinter->pPrinterName) + 1) * sizeof(TCHAR);
		dwPortName = (lstrlen(pPrinter->pPortName) + 1) * sizeof(TCHAR);
		dwDevNames = sizeof(DEVNAMES) + dwDriverName + dwPrinterName + dwPortName;

		//	Allocate memory required for the DevNames structure
		m_hDevNames = GlobalAlloc(GHND, dwDevNames);
		ASSERT(m_hDevNames);
		pDevNames = (DEVNAMES*)GlobalLock(m_hDevNames);
		ASSERT(pDevNames);
	
		//	Transfer the devnames information
		iOffset = sizeof(DEVNAMES);
		pDevNames->wDriverOffset = iOffset;
		memcpy((LPSTR)pDevNames + iOffset, pPrinter->pDriverName, dwDriverName);
		iOffset += dwDriverName;
		
		pDevNames->wDeviceOffset = iOffset;
		memcpy((LPSTR)pDevNames + iOffset, pPrinter->pPrinterName, dwPrinterName);
		iOffset += dwPrinterName;
		
		pDevNames->wOutputOffset = iOffset;
		memcpy((LPSTR)pDevNames + iOffset, pPrinter->pPortName, dwPortName);

		GlobalUnlock(pDevNames);
	}
		
	LocalFree(LocalHandle(pEnum));

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::HandleError()
//
// 	Description:	This function is called to handle internal printer errors
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::HandleError(int iError, long lParam1, long lParam2)
{
	//	Save the error identifier
	m_iError = iError;

	//	Format an error message
	switch(m_iError)
	{
		case TMPRINTER_ERROR_NO_DEFAULT_PRINTER:
			
			m_strErrorMsg = "Unable to retrieve default printer information";
			break;

		case TMPRINTER_ERROR_DRIVER_NOT_FOUND:
			
			m_strErrorMsg.Format("Unable to retrieve print driver information:\nDevice Name: %s",
								 (LPCSTR)lParam1);
			break;

		case TMPRINTER_ERROR_NOT_ATTACHED:

			m_strErrorMsg = "Unable to perform the operation - printer not attached to a device";
			break;

		case TMPRINTER_ERROR_CREATE_DC_FAILED:

			m_strErrorMsg.Format("System is unable to create the device context for the print job:\n\nDriver: %s\nDevice: %s\nPort: %s",
								 (LPCSTR)lParam1, m_strDevice, m_strPort);
			break;

		case TMPRINTER_ERROR_GET_DEVICE_MODE_FAILED:

			m_strErrorMsg.Format("System is unable to retrieve the device mode information for the print job:\n\nPrinter: %s\nDevice: %s\nPort: %s",
								 (LPCSTR)lParam1, m_strDevice, m_strPort);
			break;

		case TMPRINTER_ERROR_SET_MODE_PROPS_FAILED:

			m_strErrorMsg.Format("System is unable to set the device mode properties for the print job:\n\nPrinter: %s\nDevice: %s\nPort: %s",
								 m_strAttached, m_strDevice, m_strPort);
			break;

		case TMPRINTER_ERROR_START_JOB_FAILED:

			m_strErrorMsg.Format("The system is unable to start the print job:\n\nDriver: %s\nDevice: %s\nPort: %s",
								 m_strDriver, m_strDevice, m_strPort);
			break;

		case TMPRINTER_ERROR_OPEN_PRINTER_FAILED:

			m_strErrorMsg.Format("The attempt to open the printer connection failed:\n\nPrinter: %s",
								 (LPCSTR)lParam1);
			break;

		case TMPRINTER_ERROR_NONE:
		default:

			m_strErrorMsg = "No printer error";
			break;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::MessageBox()
//
// 	Description:	This function is provided as a debugging aid to display
//					a formatted message in a standard message box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::MessageBox(LPCSTR lpTitle, LPCSTR lpFormat, ...)
{
	char	szMessage[2048];
	CString	strTitle;

	if(lpTitle)
		strTitle = lpTitle;
	else
		strTitle = "TM Printer";

	//	Declare the variable list of arguements            
	va_list	Arguements;

	//	Insert the first variable arguement into the arguement list
	va_start(Arguements, lpFormat);

	//	Format the message
	vsprintf_s(szMessage, sizeof(szMessage), lpFormat, Arguements);

	//	Clean up the arguement list
	va_end(Arguements);

	//	Display the message
	::MessageBox(0, szMessage, strTitle, MB_OK);
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::Select()
//
// 	Description:	This function will open a print setup dialog that allows
//					the user to select the printer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::Select(CWnd* pParent)
{
	CString strPrinter;

	//	Attach to the current printer
	if(Attach())
	{
		//	Make sure the device mode is properly updated
		SetDevModeProperties(FALSE);
	}

	//	Allocate a new printer dialog 
	CPrintDialog Dlg(TRUE,  PD_NOPAGENUMS |
							PD_NOSELECTION |
							PD_ALLPAGES |
							PD_USEDEVMODECOPIESANDCOLLATE, pParent);

	//	Initialize with the current printer settings
	if(m_hDevMode)
		Dlg.m_pd.hDevMode  = m_hDevMode;
	if(m_hDevNames)
		Dlg.m_pd.hDevNames = m_hDevNames;

	if(Dlg.DoModal() == IDOK)
	{
		m_strAttached.Empty();
		
		m_strPrinter = Dlg.GetDeviceName();
		m_strDevice  = Dlg.GetDeviceName();
		m_strDriver  = Dlg.GetDriverName();
		m_strPort    = Dlg.GetPortName();

		//	Has the device mode changed?
		if((Dlg.m_pd.hDevMode != 0) && (Dlg.m_pd.hDevMode != m_hDevMode))
		{
			//	Free the existing device mode
			FREE_HANDLE(m_hDevMode)

			//	Now make the dialog device mode the active device mode
			m_hDevMode = Dlg.m_pd.hDevMode;
		}

		//	Has the device names structure changed?
		if((Dlg.m_pd.hDevNames != 0) && (Dlg.m_pd.hDevNames != m_hDevNames))
		{
			//	Free the existing device names
			FREE_HANDLE(m_hDevNames)

			//	Now make the dialog device mode the active device mode
			m_hDevNames = Dlg.m_pd.hDevNames;

		}

		//	Are we attached?
		if(m_hDevMode != NULL)
		{
			m_strAttached = m_strPrinter;

			GetDevModeProperties();
		}

	}
	else
	{
		//	Free the device mode and device names if allocated by the common dialog
		if((Dlg.m_pd.hDevMode != 0) && (Dlg.m_pd.hDevMode != m_hDevMode))
			FREE_HANDLE(Dlg.m_pd.hDevMode)
		if((Dlg.m_pd.hDevNames != 0) && (Dlg.m_pd.hDevNames != m_hDevNames))
			FREE_HANDLE(Dlg.m_pd.hDevNames)
	}

	//	Free all unused resources allocated by the common dialog
	if(Dlg.m_pd.hPrintTemplate)
		FREE_HANDLE(Dlg.m_pd.hPrintTemplate)	
	if(Dlg.m_pd.hSetupTemplate)
		FREE_HANDLE(Dlg.m_pd.hSetupTemplate)
	if(Dlg.m_pd.hDC)
		DeleteObject(Dlg.m_pd.hDC);

}

//==============================================================================
//
// 	Function Name:	CTMPrinter::SetProperties()
//
// 	Description:	This function is called to open the properties sheet for
//					the active printer
//
// 	Returns:		-1 on Error, IDOK if accepted, IDCANCEL if cancelled
//
//	Notes:			None
//
//==============================================================================
int CTMPrinter::SetProperties(HWND hWnd)
{
	int			iReturn = 0;
	HANDLE		hPrinter = NULL;
	char		szPrinter[1024];
	DEVMODE*	pDevMode = NULL;

	//	Make sure we are attached to the correct printer
	if(!Attach())
		return -1;

	ASSERT(m_hDevMode != NULL);

	//	Make sure the device mode properties are up to date
	if(SetDevModeProperties(FALSE) == FALSE)
	{
		HandleError(TMPRINTER_ERROR_SET_MODE_PROPS_FAILED);
		return -1;
	}

	//	Transfer the printer name to a working buffer
	lstrcpyn(szPrinter, m_strAttached, sizeof(szPrinter));

	//	Get a handle to the attached printer
	if(!OpenPrinter(szPrinter, &hPrinter, NULL))
	{
		HandleError(TMPRINTER_ERROR_OPEN_PRINTER_FAILED, (long)((LPCSTR)m_strAttached));
		return -1;
	}

	pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
	ASSERT(pDevMode != 0);

	//	Set the properties
	iReturn = DocumentProperties(NULL, hPrinter, szPrinter, pDevMode, 
								 pDevMode, DM_IN_PROMPT | DM_IN_BUFFER | DM_OUT_BUFFER);

	GlobalUnlock(pDevMode);

	//	Close the device and return
	ClosePrinter(hPrinter);
	
	//	Should we update the class members?
	if(iReturn == IDOK)
		GetDevModeProperties();

	return iReturn;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::SetDevModeOrientation()
//
// 	Description:	This function is called to set the device mode orientation
//					for the print job.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::SetDevModeOrientation()
{
	BOOL		bSuccessful = TRUE;
	DEVMODE*	pDevMode = NULL;

	ASSERT(m_hDevMode != NULL);

	//	Don't bother if we're using the device driver's configuration
	if(m_iOrientation == TMPRINTER_ORIENTATION_DEVICE) 
		return TRUE;

	pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
	ASSERT(pDevMode != NULL);

	//	Does this device allow us to set the orientation?
	if(pDevMode->dmFields & DM_ORIENTATION)
	{
		//	Are we supposed to use landscape orientation?
		if(m_iOrientation == TMPRINTER_ORIENTATION_LANDSCAPE)
		{
			//	Do we need to reset the orientation?
			if(pDevMode->dmOrientation != DMORIENT_LANDSCAPE)
			{
				pDevMode->dmOrientation = DMORIENT_LANDSCAPE;
			}
		}
		else
		{
			if(pDevMode->dmOrientation != DMORIENT_PORTRAIT)
			{
				pDevMode->dmOrientation = DMORIENT_PORTRAIT;
			}
		}
	
	}
	else
	{
		bSuccessful = FALSE;
	}

	GlobalUnlock(pDevMode);
	
	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::SetDevModeProperties()
//
// 	Description:	This function is called to set the device mode properties
//					for the print job.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::SetDevModeProperties(BOOL bPrinting)
{
	HANDLE		hPrinter = NULL;
	char		szPrinter[1024];
	BOOL		bSuccessful = TRUE;
	DEVMODE*	pDevMode = NULL;

	ASSERT(m_hDevMode != NULL);

	//	Transfer the printer name to a working buffer
	lstrcpyn(szPrinter, m_strAttached, sizeof(szPrinter));

	//	Get a handle to the attached printer
	if(!OpenPrinter(szPrinter, &hPrinter, NULL))
		return FALSE;

	pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
	ASSERT(pDevMode != 0);

	//	Does this device allow us to set the orientation?
	if(pDevMode->dmFields & DM_ORIENTATION)
	{
		//	Are we supposed to use landscape orientation?
		if(m_iOrientation == TMPRINTER_ORIENTATION_LANDSCAPE)
		{
			//	Do we need to reset the orientation?
			if(pDevMode->dmOrientation != DMORIENT_LANDSCAPE)
			{
				pDevMode->dmOrientation = DMORIENT_LANDSCAPE;
			}
		}
		else
		{
			if(pDevMode->dmOrientation != DMORIENT_PORTRAIT)
			{
				pDevMode->dmOrientation = DMORIENT_PORTRAIT;
			}
		}
	}

	//	Does this device allow us to set the number of copies?
	if(pDevMode->dmFields & DM_COPIES)
	{
		pDevMode->dmCopies = m_iCopies;
	}

	//	Does this device allow us to set the collate option?
	if(pDevMode->dmFields & DM_COLLATE)
	{
		pDevMode->dmCollate = m_bCollate;
	}

	//	Set the properties
	DocumentProperties(NULL, hPrinter, szPrinter, pDevMode, 
					   pDevMode, DM_IN_BUFFER | DM_OUT_BUFFER);

	GlobalUnlock(pDevMode);

	//	Close the device and return
	ClosePrinter(hPrinter);
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::SetDevModeScale()
//
// 	Description:	This function is called to set the device mode scale
//					for the print job.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::SetDevModeScale()
{
	BOOL		bSuccessful = TRUE;
	DEVMODE*	pDevMode = NULL;

	ASSERT(m_hDevMode != NULL);

	pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
	ASSERT(pDevMode != NULL);

CString strMsg;
CString strTemp;

pDevMode->dmFields |= DM_PRINTQUALITY;
pDevMode->dmPrintQuality = DMRES_LOW;

strTemp.Format("dmYResolution: %d\n", pDevMode->dmYResolution);
strMsg += strTemp;
strTemp.Format("dmPrintQuality: %d\n", pDevMode->dmPrintQuality);
strMsg += strTemp;

MessageBox(0, strMsg, "DEVMODE", MB_OK);


	pDevMode->dmFields |= DM_SCALE;
	//	Does this device allow us to set the scale?
	if(pDevMode->dmFields & DM_SCALE)
	{
		pDevMode->dmScale = 1000;	

	}
	else
	{
		MessageBox("NO SCALE SUPPORT", "");
		bSuccessful = FALSE;
	}

	GlobalUnlock(pDevMode);
	
	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::SetName()
//
// 	Description:	This function is called to set the name of the printer to
//					use for the print job
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::SetName(LPCSTR lpszName)
{
	if((lpszName != 0) && (lstrlen(lpszName) > 0))
	{
		m_strPrinter = lpszName;
	}
	else
	{
		m_strPrinter.Empty();
	}

}

//==============================================================================
//
// 	Function Name:	CTMPrinter::ShowDeviceInfo()
//
// 	Description:	This function is provided as a debugging aid to show the
//					current device information
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTMPrinter::ShowDeviceInfo()
{
	MessageBox("Device Info", 
			   "Attached: %s\nPrinter: %s\nDevice: %s\nDriver: %s\nPort: %s",
			   m_strAttached,
			   m_strPrinter,
			   m_strDevice,
			   m_strDriver,
			   m_strPort);			   
}

//==============================================================================
//
// 	Function Name:	CTMPrinter::Start()
//
// 	Description:	This function is called to start a print job.
//
// 	Returns:		A pointer to the device context for the new job
//
//	Notes:			None
//
//==============================================================================
CDC* CTMPrinter::Start(LPCSTR lpTitle)
{
	DOCINFO	DocInfo;
	char	szTitle[32];
	int		iWidth;
	int		iHeight;
	int		iLeft;
	int		iRight;
	int		iTop;
	int		iBottom;
	int		iXOffset;
	int		iYOffset;
	int		iHorzRes;
	int		iVertRes;

	//	Clear the current error information
	HandleError(TMPRINTER_ERROR_NONE);

	//	Make sure we are attached to the correct printer
	if(!Attach())
		return NULL;

	//	Make sure the orientation is correct
	SetDevModeOrientation();

	//	Make sure we have a device context
	if(!CreateDC())
		return 0;

	//	Get the pixels per inch in each direction
	m_iXDpi = m_pDC->GetDeviceCaps(LOGPIXELSX);
	m_iYDpi = m_pDC->GetDeviceCaps(LOGPIXELSY);

	//	Get the physical offsets for the device context. This is the distance
	//	between the edge of the physical page and the edge of the printable
	//	area on the page
	iXOffset = m_pDC->GetDeviceCaps(PHYSICALOFFSETX);
	iYOffset = m_pDC->GetDeviceCaps(PHYSICALOFFSETY);
	
	//	Get the total size of the physical page. This includes printable and
	//	non-printable areas
	iWidth   = m_pDC->GetDeviceCaps(PHYSICALWIDTH);
	iHeight  = m_pDC->GetDeviceCaps(PHYSICALHEIGHT);

	//	Get the total size of the printable area
	iHorzRes = m_pDC->GetDeviceCaps(HORZRES);
	iVertRes = m_pDC->GetDeviceCaps(VERTRES);

	//	Convert the margins from inches to pixels
	iLeft   = (int)((float)m_iXDpi * m_fLeftMargin);
	iTop    = (int)((float)m_iYDpi * m_fTopMargin);
	iRight  = (int)((float)m_iXDpi * m_fRightMargin);
	iBottom = (int)((float)m_iYDpi * m_fBottomMargin);

	//	Calculate the coordinates of the printable area
	//
	//	NOTE:	The physical offset defines the point at which 0,0 will
	//			appear on the printed page
	if((m_rcMax.left = iLeft - iXOffset) < 0)
		m_rcMax.left = 0;
	if((m_rcMax.top = iTop - iYOffset) < 0)
		m_rcMax.top = 0;
	if(iRight >= iXOffset)
		m_rcMax.right  = m_rcMax.left + m_pDC->GetDeviceCaps(HORZRES) - iRight;
	else
		m_rcMax.right  = m_rcMax.left + m_pDC->GetDeviceCaps(HORZRES) - iXOffset;
	if(iBottom >= iYOffset)
		m_rcMax.bottom = m_rcMax.top + m_pDC->GetDeviceCaps(VERTRES) - iBottom;
	else
		m_rcMax.bottom = m_rcMax.top + m_pDC->GetDeviceCaps(VERTRES) - iYOffset;

/*
CString M;
M.Format("XR: %d\nYR: %d\nPW: %d\nPH: %d\nHR: %d\nVR: %d\nXO: %d\nYO: %d\nLM: %d\nRM: %d\nTM: %d\nBM: %d\nLR: %d\nRR: %d\nTR: %d\nBR: %d\n",
		 m_iXDpi, m_iYDpi,
		 iWidth, iHeight,
		 iHorzRes, iVertRes,
		 iXOffset, iYOffset,
		 iLeft, iRight, iTop, iBottom,
		 m_rcMax.left, m_rcMax.right, m_rcMax.top, m_rcMax.bottom);
MessageBox(0, M, "", MB_OK);
*/

	//	The title is limited to 32 characters by Windows
	if(!lpTitle || lstrlen(lpTitle) == 0)
		lstrcpy(szTitle, "TrialMax Job");
	else
		lstrcpyn(szTitle, lpTitle, sizeof(szTitle));

	//	Initialize the document information structure
	::ZeroMemory(&DocInfo, sizeof(DocInfo));
	DocInfo.cbSize = sizeof(DocInfo);
	DocInfo.lpszDocName = szTitle;

	//	Start the print job
	if(m_pDC->StartDoc(&DocInfo) < 0)
	{
		FreeDC();
		HandleError(TMPRINTER_ERROR_START_JOB_FAILED);
		return 0;
	}
	else
	{
		return m_pDC;
	}

}

//==============================================================================
//
// 	Function Name:	CTMPrinter::StartPage()
//
// 	Description:	This function is called to start a new page
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrinter::StartPage()
{
	if(m_pDC != 0)
		return (m_pDC->StartPage() >= 0);
	else
		return FALSE;
}

/*
void CTMPrinter::Select(CWnd* pParent)
{
	CString strPrinter;

	//	Allocate a new printer dialog 
	CPrintDialog Dlg(TRUE, PD_DISABLEPRINTTOFILE |
							PD_HIDEPRINTTOFILE |
							PD_NOPAGENUMS |
							PD_NOSELECTION |
							PD_ALLPAGES |
							PD_USEDEVMODECOPIESANDCOLLATE, pParent);

	//	Initialize with the current printer settings
	if(m_hDevMode)
		Dlg.m_pd.hDevMode  = m_hDevMode;
	if(m_hDevNames)
		Dlg.m_pd.hDevNames = m_hDevNames;

	if(Dlg.DoModal() == IDOK)
	{
		m_strPrinter = Dlg.GetDeviceName();
		m_strDevice = Dlg.GetDeviceName();
		m_strDriver = Dlg.GetDriverName();
		m_strPort = Dlg.GetPortName();

ShowDeviceInfo();

		//	Has the device mode changed?
		if((Dlg.m_pd.hDevMode != 0) && (Dlg.m_pd.hDevMode != m_hDevMode))
		{
			//	Free the existing device mode
			FreeDevMode();

			//	Now make the dialog device mode the active device mode
			m_hDevMode = Dlg.m_pd.hDevMode;
			m_pDevMode = (DEVMODE*)GlobalLock(m_hDevMode);
			ASSERT(m_pDevMode);	
		}

		//	Has the device names structure changed?
		if((Dlg.m_pd.hDevNames != 0) && (Dlg.m_pd.hDevNames != m_hDevNames))
		{
			//	Free the existing device names
			FreeDevNames();

			//	Now make the dialog device mode the active device mode
			m_hDevNames = Dlg.m_pd.hDevNames;
			m_pDevNames = (DEVNAMES*)GlobalLock(m_hDevNames);
			ASSERT(m_pDevNames);	

		}
if(m_pDevNames != NULL)
{
			CrackDevNames();
			ShowDeviceInfo();
}
else
{
	MessageBox("Info", "NULL NAMES");
}

GetDriver(m_strPrinter);
ShowDeviceInfo();

		//	Are we attached?
		if(m_pDevMode != NULL)
			m_strAttached = m_strPrinter;
		else
			m_strAttached.Empty();

	}
	else
	{
		if((Dlg.m_pd.hDevMode != 0) && (Dlg.m_pd.hDevMode != m_hDevMode))
			GlobalFree(Dlg.m_pd.hDevMode);
		if((Dlg.m_pd.hDevNames != 0) && (Dlg.m_pd.hDevNames != m_hDevNames))
			GlobalFree(Dlg.m_pd.hDevNames);
	}

	if(Dlg.m_pd.hPrintTemplate)
		GlobalFree(Dlg.m_pd.hPrintTemplate);	
	if(Dlg.m_pd.hSetupTemplate)
		GlobalFree(Dlg.m_pd.hSetupTemplate);
	if(Dlg.m_pd.hDC)
		DeleteObject(Dlg.m_pd.hDC);

}

*/