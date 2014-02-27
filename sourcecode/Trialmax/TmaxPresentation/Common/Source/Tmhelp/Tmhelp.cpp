//==============================================================================
//
// File Name:	tmhelp.cpp
//
// Description:	This file contains member functions of the CTMHelp class.
//
// See Also:	tmhelp.h
//
//==============================================================================
//	Date		Revision    Description
//	10-30-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmhelp.h>
#include <direct.h>		// getcwd()
#include <casemngr.hh>
#include <tmax2.hh>
#include <trialmax.hh>

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

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CTMHelp::CTMHelp()
//
// 	Description:	This is the constructor for CTMHelp objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMHelp::CTMHelp()
{
	m_hParent  = 0;
	m_strFilespec = TMHELP_FILENAME;
}

//==============================================================================
//
// 	Function Name:	CTMHelp::~CTMHelp()
//
// 	Description:	This is the destructor for CTMHelp objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMHelp::~CTMHelp()
{
	WinHelp(m_hParent, m_strFilespec, HELP_QUIT, 0);
}

//==============================================================================
//
// 	Function Name:	CTMHelp::Initialize()
//
// 	Description:	This function initializes the Trialmax help engine
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMHelp::Initialize(HWND hParent)
{
    char szDirectory[512];

    //	Get the current working directory
    _getcwd(szDirectory, sizeof(szDirectory));
	
	//	Assemble the help file specification
	m_strFilespec = szDirectory;
	if(m_strFilespec.Right(1) != "\\")
		m_strFilespec += "\\";
	m_strFilespec += TMHELP_FILENAME;

	//	Save the parent window
	m_hParent = hParent;
}

//==============================================================================
//
// 	Function Name:	CTMHelp::Open()
//
// 	Description:	This function will open the Trialmax help file to the
//					requested section
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMHelp::Open(short sContext)
{
	switch(sContext)
	{
		case TMHELP_CMCASE:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_Cases_Page);
			break;

		case TMHELP_CMPARTY:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_Parties_Page);
			break;

		case TMHELP_CMDEPONENT:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_Deponents_Page);
			break;

		case TMHELP_CMDEPOSITION:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_Depositions_Page);
			break;

		case TMHELP_CMPLAYLIST:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_Playlist_Page);
			break;

		case TMHELP_CMTUNE:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_Tune_Page);
			break;

		case TMHELP_CMREGISTER:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_Registration_page);
			break;

		case TMHELP_CMVIEW:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_View_Page);
			break;

		case TMHELP_CMBARCODE:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_Print_Page);
			break;

		case TMHELP_TMOVERVIEW:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_Overview);
			break;

		case TMHELP_TMTOOLBAR:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_Toolbar_configuration);
			break;

		case TMHELP_TMDATABASE:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_Database_options);
			break;

		case TMHELP_TMGRAPHICS:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_Graphics_options);
			break;

		case TMHELP_TMVIDEO:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_Video_options);
			break;

		case TMHELP_TMDIAGNOSTICS:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_Diagnostics_options);
			break;

		case TMHELP_TMABOUT:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_About_options);
			break;

		case TMHELP_TMDIRECTX:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_DirectX_Options);
			break;

		case TMHELP_CMSYSTEM:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, CM_System_Page);
			break;

		case TMHELP_TMSYSTEM:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_System_Options);
			break;

		case TMHELP_TMTEXT:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTEXT, TMax_Text_Options);
			break;

		default:

			WinHelp(m_hParent, m_strFilespec, HELP_CONTENTS, 0);
			break;
	}
}



