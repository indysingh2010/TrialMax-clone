//==============================================================================
//
// File Name:	pbtool.cpp
//
// Description:	This file contains member functions of the CPBToolbar class.
//
// See Also:	pbtool.h
//
//==============================================================================
//	Date		Revision    Description
//	06-19-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <pbtool.h>
#include <tmtbdefs.h>
#include <pagebar.h>

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
const char _szPBToolMask[TMTB_MAXBUTTONS + 1] = 
{
'0',	// TMTB_CONFIG              
'0',	// TMTB_UNUSED1
'0',	// TMTB_CONFIGTOOLBARS      
'0',	// TMTB_CLEAR               
'0',	// TMTB_ROTATECW            
'0',	// TMTB_ROTATECCW           
'0',	// TMTB_NORMAL              
'0',	// TMTB_ZOOM                
'0',	// TMTB_ZOOMWIDTH           
'0',	// TMTB_PAN             
'0',	// TMTB_CALLOUT         
'0',	// TMTB_DRAWTOOL            
'0',	// TMTB_HIGHLIGHT           
'0',	// TMTB_REDACT              
'0',	// TMTB_ERASE               
'1',	// TMTB_FIRSTPAGE           
'1',	// TMTB_PREVPAGE            
'1',	// TMTB_NEXTPAGE            
'1',	// TMTB_LASTPAGE            
'0',	// TMTB_SAVEZAP         
'0',	// TMTB_FIRSTZAP            
'0',	// TMTB_PREVZAP         
'0',	// TMTB_NEXTZAP         
'0',	// TMTB_LASTZAP         
'0',	// TMTB_STARTMOVIE          
'0',	// TMTB_BACKMOVIE           
'0',	// TMTB_PAUSEMOVIE          
'0',	// TMTB_PLAYMOVIE           
'0',	// TMTB_FWDMOVIE            
'0',	// TMTB_ENDMOVIE            
'0',	// TMTB_FIRSTDESIGNATION    
'0',	// TMTB_BACKDESIGNATION 
'0',	// TMTB_PREVDESIGNATION 
'0',	// TMTB_STARTDESIGNATION    
'0',	// TMTB_PAUSEDESIGNATION    
'0',	// TMTB_PLAYDESIGNATION 
'0',	// TMTB_NEXTDESIGNATION 
'0',	// TMTB_FWDDESIGNATION      
'0',	// TMTB_LASTDESIGNATION 
'0',	// TMTB_PRINT               
'0',	// TMTB_SPLITPANE           
'0',	// TMTB_SINGLEPANE          
'0',	// TMTB_DISABLELINKS        
'0',	// TMTB_ENABLELINKS     
'0',	// TMTB_EXIT               
'0',	// TMTB_RED             
'0',	// TMTB_GREEN               
'0',	// TMTB_BLUE                
'0',	// TMTB_YELLOW              
'0',	// TMTB_BLACK               
'0',	// TMTB_WHITE               
'0',	// TMTB_PLAYTHROUGH               
'0',	// TMTB_CUEPGLNCURRENT               
'0',	// TMTB_CUEPGLNNEXT               
'0',	// TMTB_DELETEANN               
'0',	// TMTB_SELECT   
'0',	// TMTB_TEXT              
'0',	// TMTB_SELECTTOOL             
'0',	// TMTB_FREEHAND               
'0',	// TMTB_LINE              
'0',	// TMTB_ARROW               
'0',	// TMTB_ELLIPSE               
'0',	// TMTB_RECTANGLE               
'0',	// TMTB_FILLEDELLIPSE               
'0',	// TMTB_FILLEDRECTANGLE 
'0',	// TMTB_FULLSCREEN
'0',	// TMTB_STATUSBAR
'0',	// TMTB_UNUSED2
'0',	// TMTB_DARKRED             
'0',	// TMTB_DARKGREEN               
'0',	// TMTB_DARKBLUE                
'0',	// TMTB_LIGHTRED              
'0',	// TMTB_LIGHTGREEN              
'0',	// TMTB_LIGHTBLUE               
'0',	// TMTB_POLYLINE               
'0',	// TMTB_POLYGON 
'0',	// TMTB_ANNTEXT 
'0',	// TMTB_UPDATEZAP 
'0',	// TMTB_DELETEZAP
'0',	// TMTB_ZOOMRESTRICTED
0,		// NULL TERMINATION
};

short _aPBToolMap[TMTB_MAXBUTTONS] = 
{
TMTB_FIRSTPAGE, 
TMTB_PREVPAGE,
TMTB_NEXTPAGE,
TMTB_LASTPAGE, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1,
-1,
-1, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1,  
-1,  
-1,
-1, 
-1,
-1, 
-1,
-1, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1,
-1,
-1, 
-1, 
-1,
-1,
-1,  
-1,
-1,  
-1, 
-1, 
-1, 
-1,
-1,
-1, 
-1, 
-1, 
-1, 
-1, 
-1, 
-1,       
-1,          
-1,       
-1,     
-1,
-1,
-1,    
-1,    
-1,
-1, 
-1,   
-1,     
-1,         
-1,
-1,
-1,
-1,
-1,  
-1,     
-1,     
-1,   
-1,     
-1,     
-1,    
-1,
-1,
};

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CPBToolbar, CPBBand)
	//{{AFX_MSG_MAP(CPBToolbar)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CPBToolbar, CPBBand)
    //{{AFX_EVENTSINK_MAP(CPBToolbar)
	ON_EVENT(CPBToolbar, IDC_TOOLBAR, 1 /* ButtonClick */, OnButtonClick, VTS_I2 VTS_BOOL)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

//==============================================================================
//
// 	Function Name:	CPBToolbar::CPBToolbar()
//
// 	Description:	This is the constructor for CPBToolbar objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPBToolbar::CPBToolbar(CPageBar* pPageBar) : CPBBand(pPageBar, CPBToolbar::IDD)
{
	//{{AFX_DATA_INIT(CPBToolbar)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CPBToolbar::~CPBToolbar()
//
// 	Description:	This is the destructor for CPBToolbar objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPBToolbar::~CPBToolbar()
{
}

//==============================================================================
//
// 	Function Name:	CPBToolbar::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBToolbar::DoDataExchange(CDataExchange* pDX)
{
	CPBBand::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPBToolbar)
	DDX_Control(pDX, IDC_TOOLBAR, m_ctrlToolbar);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPBToolbar::EnableButton()
//
// 	Description:	This function is called to set the enable/disable the
//					specified toolbar button.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPBToolbar::EnableButton(short sId, BOOL bEnabled)
{
	//	Make sure the toolbar has been created
	if(!IsWindow(m_ctrlToolbar.m_hWnd)) 
		return FALSE;
	else
		return (m_ctrlToolbar.EnableButton(sId, bEnabled) == TMTB_NOERROR);
}

//==============================================================================
//
// 	Function Name:	CPBToolbar::GetInitialWidth()
//
// 	Description:	This function is called to get the initial width of this
//					band.
//
// 	Returns:		The initial width in pixels
//
//	Notes:			None
//
//==============================================================================
int CPBToolbar::GetInitialWidth()
{
	if(m_iInitialWidth <= 0)
	{
		if(IsWindow(m_ctrlToolbar.m_hWnd))
			return m_ctrlToolbar.GetBarWidth() + 10;
		else
			return 150;
	}
	else
	{
		return m_iInitialWidth;
	}
}

//==============================================================================
//
// 	Function Name:	CPBToolbar::GetToolbarHeight()
//
// 	Description:	This function is called to get the height required for the
//					toolbar.
//
// 	Returns:		The height in pixels
//
//	Notes:			None
//
//==============================================================================
int CPBToolbar::GetToolbarHeight()
{
	if(IsWindow(m_ctrlToolbar.m_hWnd))
		return m_ctrlToolbar.GetBarHeight();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CPBToolbar::OnButtonClick()
//
// 	Description:	This function handles the event fired by the toolbar when
//					the user clicks on a button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBToolbar::OnButtonClick(short sId, BOOL bChecked) 
{
	//	Notify the rebar window
	if(m_pPageBar)
		m_pPageBar->OnButtonClick(sId, bChecked);	
}

//==============================================================================
//
// 	Function Name:	CPBToolbar::OnInitDialog()
//
// 	Description:	This function traps all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPBToolbar::OnInitDialog() 
{
	//	Do the base class processing
	CPBBand::OnInitDialog();
	
	//	Initialize the toolbar
	m_ctrlToolbar.SetButtonMask(_szPBToolMask);
	m_ctrlToolbar.SetButtonMap(_aPBToolMap);
	if(m_ctrlToolbar.Initialize() != TMTB_NOERROR)
		return FALSE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPBToolbar::RecalcLayout()
//
// 	Description:	This function is called to recalculate the size and 
//					position of controls in the band.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBToolbar::RecalcLayout() 
{
	//	Do the base class processing
	CPBBand::RecalcLayout();
	
	//	Has the toolbar window been created yet?
	if(IsWindow(m_ctrlToolbar.m_hWnd))
	{
		m_ctrlToolbar.MoveWindow(&m_rcWnd);
		m_ctrlToolbar.ResetFrame();

	}	

}

//==============================================================================
//
// 	Function Name:	CPBToolbar::SetToolbarProps()
//
// 	Description:	This function is called to set the toolbar properties to
//					match those of the specified source toolbar.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPBToolbar::SetToolbarProps(CTMTool& rSource)
{
	//	Make sure the toolbar has been created
	if(!IsWindow(m_ctrlToolbar.m_hWnd)) return FALSE;

	//	Copy the required property values
	m_ctrlToolbar.SetButtonSize(rSource.GetButtonSize());
	m_ctrlToolbar.SetStyle(rSource.GetStyle());

	//	Rebuild the toolbar
	return (m_ctrlToolbar.Reset() == TMTB_NOERROR);
}


