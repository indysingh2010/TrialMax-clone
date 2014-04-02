// Movievw.cpp : implementation of the CMovieView class
//

#include "stdafx.h"
#include "Tmovievc.h"

#include "Moviedoc.h"
#include "Movievw.h"
#include <tmmvdefs.h>
#include <childfrm.h>
#include "Mainfrm.h"
#include "filters.h"
#include <frames.h>


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

extern CMainFrame* pMainFrame;


IMPLEMENT_DYNCREATE(CMovieView, CFormView)

BEGIN_MESSAGE_MAP(CMovieView, CFormView)
	//{{AFX_MSG_MAP(CMovieView)
	ON_WM_SIZE()
	ON_COMMAND(ID_SETFILENAME, OnSetfilename)
	ON_COMMAND(ID_TM_PAUSE, OnTmPause)
	ON_UPDATE_COMMAND_UI(ID_TM_PAUSE, OnUpdateTmPause)
	ON_COMMAND(ID_TM_PLAY, OnTmPlay)
	ON_UPDATE_COMMAND_UI(ID_TM_PLAY, OnUpdateTmPlay)
	ON_COMMAND(ID_TM_RESUME, OnTmResume)
	ON_UPDATE_COMMAND_UI(ID_TM_RESUME, OnUpdateTmResume)
	ON_COMMAND(ID_TM_SHOW, OnTmShow)
	ON_COMMAND(ID_TM_STOP, OnTmStop)
	ON_UPDATE_COMMAND_UI(ID_TM_STOP, OnUpdateTmStop)
	ON_COMMAND(ID_TM_AUTOPLAY, OnTmAutoplay)
	ON_UPDATE_COMMAND_UI(ID_TM_AUTOPLAY, OnUpdateTmAutoplay)
	ON_COMMAND(ID_TM_UNLOAD, OnTmUnload)
	ON_WM_CREATE()
	ON_COMMAND(ID_TM_FWD1, OnTmFwd1)
	ON_COMMAND(ID_TM_FWD150, OnTmFwd150)
	ON_COMMAND(ID_TM_FWD30, OnTmFwd30)
	ON_COMMAND(ID_TM_REV1, OnTmRev1)
	ON_COMMAND(ID_TM_REV150, OnTmRev150)
	ON_COMMAND(ID_TM_REV30, OnTmRev30)
	ON_COMMAND(ID_TM_CUEFIRST, OnTmCuefirst)
	ON_COMMAND(ID_TM_CUELAST, OnTmCuelast)
	ON_COMMAND(ID_TM_RESUMECUE, OnTmResumecue)
	ON_UPDATE_COMMAND_UI(ID_TM_RESUMECUE, OnUpdateTmResumecue)
	ON_COMMAND(ID_TM_AUTOSHOW, OnTmAutoshow)
	ON_UPDATE_COMMAND_UI(ID_TM_AUTOSHOW, OnUpdateTmAutoshow)
	ON_COMMAND(ID_TM_CUEABSOLUTE, OnTmCueabsolute)
	ON_COMMAND(ID_TM_VIDEOPROPS, OnTmVideoprops)
	ON_COMMAND(ID_TM_KEEPASPECT, OnTmKeepaspect)
	ON_UPDATE_COMMAND_UI(ID_TM_KEEPASPECT, OnUpdateTmKeepaspect)
	ON_COMMAND(ID_TM_SCALE, OnTmScale)
	ON_UPDATE_COMMAND_UI(ID_TM_SCALE, OnUpdateTmScale)
	ON_COMMAND(ID_TM_SOFTER, OnTmSofter)
	ON_UPDATE_COMMAND_UI(ID_TM_SOFTER, OnUpdateTmSofter)
	ON_COMMAND(ID_TM_SLOWER, OnTmSlower)
	ON_UPDATE_COMMAND_UI(ID_TM_SLOWER, OnUpdateTmSlower)
	ON_COMMAND(ID_TM_RIGHT, OnTmRight)
	ON_UPDATE_COMMAND_UI(ID_TM_RIGHT, OnUpdateTmRight)
	ON_COMMAND(ID_TM_LOUDER, OnTmLouder)
	ON_UPDATE_COMMAND_UI(ID_TM_LOUDER, OnUpdateTmLouder)
	ON_COMMAND(ID_TM_LEFT, OnTmLeft)
	ON_UPDATE_COMMAND_UI(ID_TM_LEFT, OnUpdateTmLeft)
	ON_COMMAND(ID_TM_FASTER, OnTmFaster)
	ON_UPDATE_COMMAND_UI(ID_TM_FASTER, OnUpdateTmFaster)
	ON_COMMAND(ID_TM_GETRESOLUTION, OnTmGetresolution)
	ON_COMMAND(ID_TM_RANGE, OnTmRange)
	ON_COMMAND(ID_TM_STEP, OnTmStep)
	ON_COMMAND(ID_TM_UPDATEVIDEO, OnTmUpdatevideo)
	ON_COMMAND(ID_TM_SNAPSHOT, OnTmSnapshot)
	ON_COMMAND(ID_TM_CAPTURE, OnTmCapture)
	ON_COMMAND(ID_TM_DIBSNAPS, OnTmDibsnaps)
	ON_UPDATE_COMMAND_UI(ID_TM_DIBSNAPS, OnUpdateTmDibsnaps)
	ON_COMMAND(ID_TM_FILTERPROPS, OnTmFilterprops)
	ON_COMMAND(ID_TM_OVERLAYVISIBLE, OnTmOverlayvisible)
	ON_UPDATE_COMMAND_UI(ID_TM_OVERLAYVISIBLE, OnUpdateTmOverlayvisible)
	ON_COMMAND(ID_TM_SETOVERLAY, OnTmSetoverlay)
	ON_COMMAND(ID_TM_CLEAROVERLAY, OnTmClearoverlay)
	ON_COMMAND(ID_CLASS_ID, OnClassId)
	ON_COMMAND(ID_REGISTERED_PATH, OnRegisteredPath)
	ON_COMMAND(ID_TM_DETACHBEFORELOAD, OnTmDetachbeforeload)
	ON_UPDATE_COMMAND_UI(ID_TM_DETACHBEFORELOAD, OnUpdateTmDetachbeforeload)
	ON_COMMAND(ID_TM_SET_USER_FILTERS, OnTmSetUserFilters)
	ON_COMMAND(ID_TM_SHOWAUDIOIMAGE, OnTmShowaudioimage)
	ON_UPDATE_COMMAND_UI(ID_TM_SHOWAUDIOIMAGE, OnUpdateTmShowaudioimage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CMovieView construction/destruction

CMovieView::CMovieView()
	: CFormView(CMovieView::IDD)
{
	//{{AFX_DATA_INIT(CMovieView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	m_bResumeCue = FALSE;
}

CMovieView::~CMovieView()
{
}

void CMovieView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CMovieView)
	DDX_Control(pDX, IDC_TMMOVIECTRL1, m_Movie);
	//}}AFX_DATA_MAP
}

BOOL CMovieView::PreCreateWindow(CREATESTRUCT& cs)
{
	return CFormView::PreCreateWindow(cs);
}

/////////////////////////////////////////////////////////////////////////////
// CMovieView diagnostics

#ifdef _DEBUG
void CMovieView::AssertValid() const
{
	CFormView::AssertValid();
}

void CMovieView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CMovieDoc* CMovieView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CMovieDoc)));
	return (CMovieDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMovieView message handlers

void CMovieView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	if(IsWindow(m_Movie.m_hWnd))
		m_Movie.MoveWindow(0, 0, cx, cy);
}

void CMovieView::OnInitialUpdate() 
{
	CFormView::OnInitialUpdate();
	
	if(IsWindow(m_Movie.m_hWnd))
	{
		CRect Rect;
		GetClientRect(Rect);
		m_Movie.MoveWindow(Rect);
	}
	
	OnStateChangeTmmoviectrl1(m_Movie.GetState());	

	CString strFile = GetDocument()->GetPathName();
	if(strFile.IsEmpty())
		return;
	else
		m_Movie.SetFilename(strFile);
	
}

void CMovieView::OnSetfilename() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "MPEG (*.mpg,*.mpeg)\0*.mpg;*.mpeg\0AVI (*.avi)\0*.avi\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES;
	if(FileDlg.DoModal() == IDOK)
		m_Movie.SetFilename(szFilename);

}

void CMovieView::OnTmPause() 
{
	m_Movie.Pause();
}

void CMovieView::OnUpdateTmPause(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.IsLoaded());
}

void CMovieView::OnTmPlay() 
{
	m_Movie.Play();
}

void CMovieView::OnUpdateTmPlay(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.IsLoaded());
}

void CMovieView::OnTmResume() 
{
	m_Movie.Resume();
}

void CMovieView::OnUpdateTmResume(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.IsLoaded());
}

void CMovieView::OnTmShow() 
{
	m_Movie.ShowVideo(!m_Movie.IsVideoVisible());
}

void CMovieView::OnTmStop() 
{
	m_Movie.Stop();
}

void CMovieView::OnUpdateTmStop(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.IsReady());
}

void CMovieView::OnTmAutoplay() 
{
	m_Movie.SetAutoPlay(!m_Movie.GetAutoPlay());
}

void CMovieView::OnUpdateTmAutoplay(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_Movie.GetAutoPlay());
}

void CMovieView::OnTmUnload() 
{
	m_Movie.Unload();
	
}

BEGIN_EVENTSINK_MAP(CMovieView, CFormView)
    //{{AFX_EVENTSINK_MAP(CMovieView)
	ON_EVENT(CMovieView, IDC_TMMOVIECTRL1, 1 /* FileChange */, OnFileChangeTmmoviectrl1, VTS_BSTR)
	ON_EVENT(CMovieView, IDC_TMMOVIECTRL1, 2 /* StateChange */, OnStateChangeTmmoviectrl1, VTS_I2)
	ON_EVENT(CMovieView, IDC_TMMOVIECTRL1, 4 /* PlaybackError */, OnPlaybackErrorTmmoviectrl1, VTS_I4 VTS_BOOL)
	ON_EVENT(CMovieView, IDC_TMMOVIECTRL1, 5 /* PlaybackComplete */, OnPlaybackCompleteTmmoviectrl1, VTS_NONE)
	ON_EVENT(CMovieView, IDC_TMMOVIECTRL1, 14 /* PositionChange */, OnPositionChangeTmmoviectrl1, VTS_R8)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CMovieView::OnFileChangeTmmoviectrl1(LPCTSTR lpFilename) 
{
	GetParent()->SetWindowText(lpFilename);
	if(m_pStatus)
	{
		m_pStatus->SetPaneText(STATUS_STATE_PANE, "");
		m_pStatus->SetPaneText(STATUS_POSITION_PANE, "");
		m_pStatus->SetPaneText(STATUS_EVENT_PANE, "");
	}
	
}

void CMovieView::OnStateChangeTmmoviectrl1(short sState) 
{
	if(m_pStatus == 0 || !IsWindow(m_pStatus->m_hWnd))
		return;

	switch(sState)
	{
		case TMMOVIE_NOTREADY:			

			m_pStatus->SetPaneText(STATUS_STATE_PANE, "Not Ready");
			break;
	
		case TMMOVIE_READY:			

			m_pStatus->SetPaneText(STATUS_STATE_PANE, "Ready");
			break;
	
		case TMMOVIE_PLAYING:			

			m_pStatus->SetPaneText(STATUS_STATE_PANE, "Playing");
			m_pStatus->SetPaneText(STATUS_EVENT_PANE, "");
			break;
	
		case TMMOVIE_PAUSED:			

			m_pStatus->SetPaneText(STATUS_STATE_PANE, "Paused");
			break;
	
		case TMMOVIE_STOPPED:			

			m_pStatus->SetPaneText(STATUS_STATE_PANE, "Stopped");
			break;
	
		default:		

			m_pStatus->SetPaneText(STATUS_STATE_PANE, "Unknown");
			break;
	}
}

int CMovieView::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	CChildFrame* pFrame;
	
	if(CFormView::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	pFrame = (CChildFrame*)GetParent();
	if(pFrame)
		m_pStatus = &(pFrame->m_wndStatusBar);
	ASSERT(m_pStatus);
	
	return 0;
}

void CMovieView::OnTmFwd1() 
{
	m_Movie.Cue(TMMCUE_RELATIVE, 0.3, m_bResumeCue);
}

void CMovieView::OnTmFwd150() 
{
	m_Movie.Cue(TMMCUE_RELATIVE, 5.0f, m_bResumeCue);
}

void CMovieView::OnTmFwd30() 
{
	m_Movie.Cue(TMMCUE_RELATIVE, 1.0f, m_bResumeCue);
}

void CMovieView::OnTmRev1() 
{
	m_Movie.Cue(TMMCUE_RELATIVE, -0.3, m_bResumeCue);
}

void CMovieView::OnTmRev150() 
{
	m_Movie.Cue(TMMCUE_RELATIVE, -5.0f, m_bResumeCue);
}

void CMovieView::OnTmRev30() 
{
	m_Movie.Cue(TMMCUE_RELATIVE, -1.0f, m_bResumeCue);
}

void CMovieView::OnTmCuefirst() 
{
	m_Movie.Cue(TMMCUE_FIRST, 0, m_bResumeCue);
}

void CMovieView::OnTmCuelast() 
{
	m_Movie.Cue(TMMCUE_LAST, 0, m_bResumeCue);
}

void CMovieView::OnTmResumecue() 
{
	m_bResumeCue = !m_bResumeCue;
	
}

void CMovieView::OnUpdateTmResumecue(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_bResumeCue);
}

void CMovieView::OnTmAutoshow() 
{
	m_Movie.SetAutoShow(!m_Movie.GetAutoShow());
}

void CMovieView::OnUpdateTmAutoshow(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_Movie.GetAutoShow());
}

void CMovieView::OnTmCueabsolute() 
{
	long lPos = 0;

	if(pMainFrame)
		lPos = pMainFrame->GetCurrent();

	m_Movie.Cue(TMMCUE_ABSOLUTE, lPos, m_bResumeCue);
}

void CMovieView::OnTmVideoprops() 
{
	m_Movie.ShowVideoProps();
}

void CMovieView::OnTmKeepaspect() 
{
	m_Movie.SetKeepAspect(!m_Movie.GetKeepAspect());	
}

void CMovieView::OnUpdateTmKeepaspect(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_Movie.GetKeepAspect());	
}

void CMovieView::OnTmScale() 
{
	m_Movie.SetScaleVideo(!m_Movie.GetScaleVideo());
}

void CMovieView::OnUpdateTmScale(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_Movie.GetScaleVideo());
}

void CMovieView::OnTmSofter() 
{
	m_Movie.SetVolume(m_Movie.GetVolume() - 5);	
}

void CMovieView::OnUpdateTmSofter(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.CanSetVolume() && m_Movie.GetVolume() > 0);	
}

void CMovieView::OnTmSlower() 
{
	m_Movie.SetRate(m_Movie.GetRate() - 5);
}

void CMovieView::OnUpdateTmSlower(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.CanSetRate() && m_Movie.GetRate() > -100);	
}

void CMovieView::OnTmRight() 
{
	m_Movie.SetBalance(m_Movie.GetBalance() + 5);	
}

void CMovieView::OnUpdateTmRight(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.CanSetBalance() && m_Movie.GetBalance() < 100);	
}

void CMovieView::OnTmLouder() 
{
	m_Movie.SetVolume(m_Movie.GetVolume() + 5);	
}

void CMovieView::OnUpdateTmLouder(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.CanSetVolume() && m_Movie.GetVolume() < 100);	
}

void CMovieView::OnTmLeft() 
{
	m_Movie.SetBalance(m_Movie.GetBalance() - 5);	
}

void CMovieView::OnUpdateTmLeft(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.CanSetBalance() && m_Movie.GetBalance() > -100);	
}

void CMovieView::OnTmFaster() 
{
	m_Movie.SetRate(m_Movie.GetRate() + 5);	
}

void CMovieView::OnUpdateTmFaster(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Movie.CanSetRate() && m_Movie.GetRate() < 100);	
}

void CMovieView::OnPlaybackErrorTmmoviectrl1(long lError, BOOL bStopped) 
{
	if(m_pStatus == 0)
		return;

	CString strText;
	if(bStopped)
		strText.Format("Error: %ld - stopped");
	else
		strText.Format("Error: %ld - playing");

	m_pStatus->SetPaneText(STATUS_EVENT_PANE, strText);
	
}

void CMovieView::OnPlaybackCompleteTmmoviectrl1() 
{
	if(m_pStatus)
		m_pStatus->SetPaneText(STATUS_EVENT_PANE, "Playback Complete");
}

void CMovieView::OnTmGetresolution() 
{
	short sUpdate = m_Movie.GetUpdateRate();
	float fRate = m_Movie.GetFrameRate();
	short sResolution = m_Movie.GetResolution();

	CString Info;
	Info.Format("Update Rate: %d\nFrame Rate: %.2f\nResolution: %d",
				sUpdate, fRate, sResolution);
	MessageBox(Info, "Frame Resolution", MB_ICONINFORMATION | MB_OK);
	
}

void CMovieView::OnTmRange() 
{
MessageBox("CORRECT FOR TMMOVIE 6 TIME BASED POSITIONING");
/*
	CFrames Frames;

	Frames.m_lStart = m_Movie.GetStartFrame();
	Frames.m_lStop  = m_Movie.GetStopFrame();

	if(Frames.DoModal() == IDOK)
		m_Movie.SetRange(Frames.m_lStart, Frames.m_lStop);
*/	
}

void CMovieView::OnTmStep() 
{
	CFrames Frames;

	Frames.m_lStart = 0;
	Frames.m_lStop  = 0;

	if(Frames.DoModal() == IDOK)
		m_Movie.Step(Frames.m_lStart, Frames.m_lStop);
	
}

void CMovieView::OnTmUpdatevideo() 
{
	m_Movie.Update();
}

void CMovieView::OnTmSnapshot() 
{
	m_Movie.ShowSnapshot();
}

void CMovieView::OnTmCapture() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "Bitmaps (*.bmp)\0*.bmp\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_LONGNAMES;
	if(FileDlg.DoModal() == IDOK)
		m_Movie.Capture(szFilename, TRUE);
}

void CMovieView::OnTmDibsnaps() 
{
	m_Movie.SetUseSnapshots(!m_Movie.GetUseSnapshots());	
}

void CMovieView::OnUpdateTmDibsnaps(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_Movie.GetUseSnapshots());
}

void CMovieView::OnTmFilterprops() 
{
	m_Movie.ShowFilterInfo();
}

void CMovieView::OnTmOverlayvisible() 
{
	m_Movie.SetOverlayVisible(!m_Movie.GetOverlayVisible());	
}

void CMovieView::OnUpdateTmOverlayvisible(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_Movie.GetOverlayVisible());
}

void CMovieView::OnTmSetoverlay() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "Bitmaps (*.bmp)\0*.bmp\0Tiff (*.tif)\0*.tif\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES;
	if(FileDlg.DoModal() == IDOK)
		m_Movie.SetOverlayFile(szFilename);
}

void CMovieView::OnTmClearoverlay() 
{
	m_Movie.SetOverlayFile(0);	
}



void CMovieView::OnClassId() 
{
	CString Class = m_Movie.GetClassIdString();
	MessageBox(Class);	
}

void CMovieView::OnRegisteredPath() 
{
	CString Path = m_Movie.GetRegisteredPath();
	MessageBox(Path);	
}

void CMovieView::OnTmDetachbeforeload() 
{
	m_Movie.SetDetachBeforeLoad(!m_Movie.GetDetachBeforeLoad());	
}

void CMovieView::OnUpdateTmDetachbeforeload(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_Movie.GetDetachBeforeLoad());
}

void CMovieView::OnTmSetUserFilters() 
{
	CFilters Filters;
	Filters.SetTMMovie(&m_Movie);
	Filters.DoModal();
}

void CMovieView::OnPositionChangeTmmoviectrl1(double dPosition) 
{
	CString Pos;
	Pos.Format("Position: %f", dPosition);
	if(m_pStatus && IsWindow(m_pStatus->m_hWnd))
		m_pStatus->SetPaneText(STATUS_POSITION_PANE, Pos);
}

void CMovieView::OnTmShowaudioimage() 
{
	m_Movie.SetShowAudioImage(!m_Movie.GetShowAudioImage());	
}

void CMovieView::OnUpdateTmShowaudioimage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_Movie.GetShowAudioImage());
}
