//==============================================================================
//
// File Name:	tmmovie.h
//
// Description:	This file contains the declaration of the CTMMovieCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	04-21-98	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMMOVIE_H__828750F2_0139_11D2_B1BD_008029EFD140__INCLUDED_)
#define AFX_TMMOVIE_H__828750F2_0139_11D2_B1BD_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <afxcmn.h>
#include <handler.h>
#include <tmini.h>
#include <player.h>
#include <playlist.h>
#include <snapshot.h>
#include <overlay.h>
#include <textline.h>
#include <objsafe.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define LIGOS_SPLITTER_KEY		"HKEY_CLASSES_ROOT\\CLSID\\{083863F1-70DE-11d0-BD40-00A0C911CE86}\\Instance\\{CB51EFC1-40D6-11D3-B265-00A0C9A3A56F}"
#define LIGOS_DECODER_KEY		"HKEY_CLASSES_ROOT\\CLSID\\{083863F1-70DE-11d0-BD40-00A0C911CE86}\\Instance\\{CB51EFC2-40D6-11D3-B265-00A0C9A3A56F}"

//	This message is used for event notifications
#define WM_ERROR_NOTIFICATION	WM_USER + 256

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMMovieCtrl : public COleControl
{
	private:
	
							DECLARE_DYNCREATE(CTMMovieCtrl)

		CPlayer				m_Player;
		CTextLines			m_Lines;
		COverlay*			m_pOverlay;
		RECT				m_rcOverlay;
		CTMIni				m_Ini;
		CTMVersion			m_tmVersion;
		CErrorHandler		m_Errors;
		CPlaylist*			m_pPlaylist;
		CDesignation*		m_pDesignation;
		CLink*				m_pLink;
		CTextLine*			m_pLine;
		short				m_sState;
		short				m_sPlaylistState;
		int					m_iVideoSliderHeight;
		long				m_lPlaylistStart;
		long				m_lPlaylistStop;
		double				m_dPosition;
		double				m_dMinimumCue;
		double				m_dMaximumCue;
		double				m_dTimePos;
		UINT				m_uTimer;
		BOOL				m_bPlayDesignation;
		BOOL				m_bNotify;
		BOOL				m_bLastFrame;
		BOOL				m_bEnforceRange;
		HBITMAP				m_hAudioBitmap;
		BITMAP				m_bmAudioInfo;
		CString				m_strDebugFilename;
		CSliderCtrl *		m_sldVideoSliderControl;
	
	public:
	
		BOOL				m_bDoDraw;

							CTMMovieCtrl();
						   ~CTMMovieCtrl();

		LONG				OnWMErrorNotification(WPARAM wParam, LPARAM lParam);
		
		//	CPlayer notifications
		void				OnPlayerSetPos();
		void				OnPlayerShow(BOOL bShow);
		void				OnPlayerSetWnd();

		void				AddDebugMessage(LPCSTR lpFormat, ...);
		void				SendDebug(LPCSTR lpMsg1, LPCSTR lpMsg2, 
									  short sSleep);

	protected:

		void				GetRegistration();
		void				ProcessEvent(long lEvent);
		void				SetState(short sState);
		void				StopTimer();
		void				StartTimer();
		void				UpdatePos();
		void				SetFilters();
		void				SetPlaylistState(short sState);
		void				SetPlaylistPosition(double dPosition);
		void				SetPlaylistLink(double dPosition);
		void				SetPlaylistLine(double dPosition);
		void				CalcPlaylistStep(long* pIndex, double* pPosition,
											 double dStep);
		void				ResetPlaylistTimes(double dStart);
		void				ResetDesignationTimes(double dPosition);
		void				OnEvComplete();
		void				OnEvError(long lError, BOOL bStopped);
		void				UpdateElapsedTimes();
		void				ResizeToRatio(CRect& rRect, float fRatio);
		void				DrawAudio(CDC* pdc, CRect& rcBounds);
		void				GetAudioBitmap();
		void				ReleaseAudioBitmap();
		void				SetDebugFilename();
		BOOL				CheckVersion(DWORD dwVersion);
		BOOL				FindFile(LPCSTR lpFile, BOOL bPrompt);
		BOOL				SetOverlayPos(RECT* pRect);
		short				LoadDesignation(double dPosition);
		short				UpdateState();
		short				GetControlKeys();
	
	//--------------------------------------------------------------------
	//	Dispatch interface for implementing object safety
	//
	//	This is required to eliminate warnings when the control is invoked
	//	directly from a browser
	DECLARE_INTERFACE_MAP()

	BEGIN_INTERFACE_PART(ObjSafety, IObjectSafety)
		STDMETHOD_(HRESULT, GetInterfaceSafetyOptions) ( 
            /* [in] */ REFIID riid,
            /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
            /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions
		);
        
        STDMETHOD_(HRESULT, SetInterfaceSafetyOptions) ( 
            /* [in] */ REFIID riid,
            /* [in] */ DWORD dwOptionSetMask,
            /* [in] */ DWORD dwEnabledOptions
		);
	END_INTERFACE_PART(ObjSafety);
	//--------------------------------------------------------------------

	//	The remainder of this declaration is maintained by Class Wizard

	public:
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTMMovieCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();
	virtual void OnBackColorChanged();
	protected:
	virtual LRESULT DefWindowProc(UINT message, WPARAM wParam, LPARAM lParam);
	HRESULT OnActivateInPlace(BOOL bUIActivate, LPMSG pMsg);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMMovieCtrl)		// Class factory and guid
	DECLARE_OLETYPELIB(CTMMovieCtrl)		// GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMMovieCtrl)		// Property page IDs
	DECLARE_OLECTLTYPE(CTMMovieCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMMovieCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnRButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	//{{AFX_DISPATCH(CTMMovieCtrl)
	double m_dPlaylistTime;
	double m_dElapsedDesignation;
	double m_dElapsedPlaylist;
	double m_dDesignationTime;
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	CString m_strIniFile;
	afx_msg void OnIniFileChanged();
	BOOL m_bAutoPlay;
	afx_msg void OnAutoPlayChanged();
	BOOL m_bScaleVideo;
	afx_msg void OnScaleVideoChanged();
	CString m_strFilename;
	afx_msg void OnFilenameChanged();
	short m_sUpdateRate;
	afx_msg void OnUpdateRateChanged();
	BOOL m_bAutoShow;
	afx_msg void OnAutoShowChanged();
	BOOL m_bKeepAspect;
	afx_msg void OnKeepAspectChanged();
	short m_sBalance;
	afx_msg void OnBalanceChanged();
	short m_sRate;
	afx_msg void OnRateChanged();
	short m_sVolume;
	afx_msg void OnVolumeChanged();
	BOOL m_bUseSnapshots;
	afx_msg void OnUseSnapshotsChanged();
	CString m_strOverlay;
	afx_msg void OnOverlayFileChanged();
	BOOL m_bOverlayVisible;
	afx_msg void OnOverlayVisibleChanged();
	double m_dStartPosition;
	afx_msg void OnStartPositionChanged();
	double m_dStopPosition;
	afx_msg void OnStopPositionChanged();
	BOOL m_bEnableAxErrors;
	afx_msg void OnEnableAxErrorsChanged();
	afx_msg BSTR GetVerTextLong();
	afx_msg short Unload();
	afx_msg short Initialize();
	afx_msg short Play();
	afx_msg short Pause();
	afx_msg short Stop();
	afx_msg short Resume();
	afx_msg BOOL IsReady();
	afx_msg short GetState();
	afx_msg float GetFrameRate();
	afx_msg short GetSrcWidth();
	afx_msg short GetSrcHeight();
	afx_msg short ShowVideoProps();
	afx_msg short CheckType(LPCTSTR lpFilename);
	afx_msg short GetPlaylistState();
	afx_msg short GetType();
	afx_msg void ShowVideo(BOOL bShow);
	afx_msg BOOL IsVideoVisible();
	afx_msg BOOL CanSetVolume();
	afx_msg BOOL CanSetBalance();
	afx_msg BOOL CanSetRate();
	afx_msg BOOL IsLoaded();
	afx_msg short Update();
	afx_msg short GetResolution();
	afx_msg OLE_HANDLE ShowSnapshot();
	afx_msg short Capture(LPCTSTR lpFilespec, BOOL bResume);
	afx_msg BSTR GetRegFilters(long FAR* pCount);
	afx_msg void ShowFilterInfo();
	afx_msg BSTR GetActFilters(BOOL bVendorInfo, long FAR* pCount);
	afx_msg LPUNKNOWN GetInterface(short sInterface);
	afx_msg void SetDefaultRate(double dFrameRate);
	afx_msg double GetDefaultRate();
	afx_msg short SetPlaylistRange(long lStart, long lStop);
	afx_msg BSTR GetClassIdString();
	afx_msg BSTR GetRegisteredPath();
	afx_msg short AddFilter(LPCTSTR lpszName);
	afx_msg short RemoveFilter(LPCTSTR lpszName);
	afx_msg BSTR GetUserFilters(long FAR* pCount);
	afx_msg double GetMinTime();
	afx_msg double GetMaxTime();
	afx_msg double GetPosition();
	afx_msg long ConvertToFrames(double dSeconds);
	afx_msg short SetMaxCuePosition(double dPosition);
	afx_msg short SetMinCuePosition(double dPosition);
	afx_msg short SetRange(double dStart, double dStop);
	afx_msg short Cue(short sType, double dSeconds, BOOL bResume);
	afx_msg short Load(LPCTSTR lpszFilename, double dStart, double dStop, BOOL bPlay);
	afx_msg short Step(double dFrom, double dTo);
	afx_msg double ConvertToTime(long lFrame);
	afx_msg short CuePlaylist(short sType, double dSeconds, BOOL bResume, BOOL bPlayToEnd);
	afx_msg short PlayPlaylist(long pPlaylist, long lStart, long lStop, double dPosition);
	afx_msg short CueDesignation(long lDesignation, double dPosition, short bResume);
	afx_msg double GetDuration(LPCTSTR lpszFilename);
	afx_msg short UpdateScreenPosition();
	afx_msg BOOL GetIsAudio();
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 5.1
	BOOL m_bDetachBeforeLoad;
	afx_msg void OnDetachBeforeLoadChanged();

	//	Added in rev 5.2
	BOOL m_bHideTaskBar;
	afx_msg void OnHideTaskBarChanged();

	//	Added in rev 6.1
	BOOL m_bEnableSimulation;
	afx_msg void OnEnableSimulationChanged();
	afx_msg void OnSimulationTextChanged();
	CString m_strSimulationText;

	//	Added in rev 6.1.0
	BOOL m_bShowAudioImage;
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg short GetVerBuild();
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();
	afx_msg void OnShowAudioImageChanged();

	//{{AFX_EVENT(CTMMovieCtrl)
	void FireFileChange(LPCTSTR lpFilename)
		{FireEvent(eventidFileChange,EVENT_PARAM(VTS_BSTR), lpFilename);}
	void FireStateChange(short sState)
		{FireEvent(eventidStateChange,EVENT_PARAM(VTS_I2), sState);}
	void FirePlaylistState(short sState)
		{FireEvent(eventidPlaylistState,EVENT_PARAM(VTS_I2), sState);}
	void FirePlaybackError(long lError, BOOL bStopped)
		{FireEvent(eventidPlaybackError,EVENT_PARAM(VTS_I4  VTS_BOOL), lError, bStopped);}
	void FirePlaybackComplete()
		{FireEvent(eventidPlaybackComplete,EVENT_PARAM(VTS_NONE));}
	void FireDebugMessage(LPCTSTR lpMsg1, LPCTSTR lpMsg2)
		{FireEvent(eventidDebugMessage,EVENT_PARAM(VTS_BSTR  VTS_BSTR), lpMsg1, lpMsg2);}
	void FireLineChange(long pLine)
		{FireEvent(eventidLineChange,EVENT_PARAM(VTS_I4), pLine);}
	void FirePlaylistTime(double dTime)
		{FireEvent(eventidPlaylistTime,EVENT_PARAM(VTS_R8), dTime);}
	void FireDesignationTime(double dTime)
		{FireEvent(eventidDesignationTime,EVENT_PARAM(VTS_R8), dTime);}
	void FireElapsedTimes(double dPlaylist, double dDesignation)
		{FireEvent(eventidElapsedTimes,EVENT_PARAM(VTS_R8  VTS_R8), dPlaylist, dDesignation);}
	void FireDesignationChange(long lId, long lOrder)
		{FireEvent(eventidDesignationChange,EVENT_PARAM(VTS_I4  VTS_I4), lId, lOrder);}
	void FireLinkChange(LPCTSTR lpszBarcode, long lId, long lFlags)
		{FireEvent(eventidLinkChange,EVENT_PARAM(VTS_BSTR  VTS_I4  VTS_I4), lpszBarcode, lId, lFlags);}
	void FireMouseDblClick(short sButtton, OLE_XPOS_PIXELS x, OLE_YPOS_PIXELS y)
		{FireEvent(eventidMouseDblClick,EVENT_PARAM(VTS_I2  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS), sButtton, x, y);}
	void FirePositionChange(double dPosition)
		{FireEvent(eventidPositionChange,EVENT_PARAM(VTS_R8), dPosition);}
	void FireAxError(LPCTSTR lpszMessage)
		{FireEvent(eventidAxError,EVENT_PARAM(VTS_BSTR), lpszMessage);}
	void FireAxDiagnostic(LPCTSTR lpszMethod, LPCTSTR lpszMessage)
		{FireEvent(eventidAxDiagnostic,EVENT_PARAM(VTS_BSTR  VTS_BSTR), lpszMethod, lpszMessage);}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMMovieCtrl)
	dispidAutoInit = 5L,
	dispidEnableErrors = 6L,
	dispidIniFile = 7L,
	dispidAutoPlay = 8L,
	dispidScaleVideo = 9L,
	dispidFilename = 10L,
	dispidUpdateRate = 11L,
	dispidAutoShow = 12L,
	dispidKeepAspect = 13L,
	dispidBalance = 14L,
	dispidRate = 15L,
	dispidVolume = 16L,
	dispidUseSnapshots = 17L,
	dispidOverlayFile = 18L,
	dispidOverlayVisible = 19L,
	dispidVerTextLong = 23L,
	dispidPlaylistTime = 1L,
	dispidElapsedDesignation = 2L,
	dispidElapsedPlaylist = 3L,
	dispidDesignationTime = 4L,
	dispidStartPosition = 20L,
	dispidStopPosition = 21L,
	dispidEnableAxErrors = 22L,
	dispidUnload = 24L,
	dispidInitialize = 25L,
	dispidPlay = 26L,
	dispidPause = 27L,
	dispidStop = 28L,
	dispidResume = 29L,
	dispidIsReady = 30L,
	dispidGetState = 31L,
	dispidGetFrameRate = 32L,
	dispidGetSrcWidth = 33L,
	dispidGetSrcHeight = 34L,
	dispidShowVideoProps = 35L,
	dispidCheckType = 36L,
	dispidGetPlaylistState = 37L,
	dispidGetType = 38L,
	dispidShowVideo = 39L,
	dispidIsVideoVisible = 40L,
	dispidCanSetVolume = 41L,
	dispidCanSetBalance = 42L,
	dispidCanSetRate = 43L,
	dispidIsLoaded = 44L,
	dispidUpdate = 45L,
	dispidGetResolution = 46L,
	dispidShowSnapshot = 47L,
	dispidCapture = 48L,
	dispidGetRegFilters = 49L,
	dispidShowFilterInfo = 50L,
	dispidGetActFilters = 51L,
	dispidGetInterface = 52L,
	dispidSetDefaultRate = 53L,
	dispidGetDefaultRate = 54L,
	dispidSetPlaylistRange = 55L,
	dispidGetClassIdString = 56L,
	dispidGetRegisteredPath = 57L,
	dispidAddFilter = 58L,
	dispidRemoveFilter = 59L,
	dispidGetUserFilters = 60L,
	dispidGetMinTime = 61L,
	dispidGetMaxTime = 62L,
	dispidGetPosition = 63L,
	dispidConvertToFrames = 64L,
	dispidSetMaxCuePosition = 65L,
	dispidSetMinCuePosition = 66L,
	dispidSetRange = 67L,
	dispidCue = 68L,
	dispidLoad = 69L,
	dispidStep = 70L,
	dispidConvertToTime = 71L,
	dispidCuePlaylist = 72L,
	dispidPlayPlaylist = 73L,
	dispidCueDesignation = 74L,
	dispidGetDuration = 75L,
	dispidUpdateScreenPosition = 76L,
	dispidGetIsAudio = 77L,
	eventidFileChange = 1L,
	eventidStateChange = 2L,
	eventidPlaylistState = 3L,
	eventidPlaybackError = 4L,
	eventidPlaybackComplete = 5L,
	eventidDebugMessage = 6L,
	eventidLineChange = 7L,
	eventidPlaylistTime = 8L,
	eventidDesignationTime = 9L,
	eventidElapsedTimes = 10L,
	eventidDesignationChange = 11L,
	eventidLinkChange = 12L,
	eventidMouseDblClick = 13L,
	eventidPositionChange = 14L,
	eventidAxError = 15L,
	eventidAxDiagnostic = 16L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMMOVIE_H__828750F2_0139_11D2_B1BD_008029EFD140__INCLUDED)
