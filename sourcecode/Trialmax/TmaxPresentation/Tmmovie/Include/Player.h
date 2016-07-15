//==============================================================================
//
// File Name:	player.h
//
// Description:	This file contains the declaration of the CPlayer class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-12-98	1.00		Original Release
//==============================================================================
#if !defined(__PLAYER_H__)
#define __PLAYER_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <idxshow.h>
#include <handler.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define DELETE_INTERFACE(x) { if (x) delete x; x = 0; }
#define DELETE_SNAPSHOT(x)	{ if (x) delete x; x = 0; }

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMMovieCtrl;
class CSnapshot;

class CPlayer
{
	private:

		IFilterMapper2*		m_pIMapper;
		SFilter				m_aFilters[IDXSHOW_MAX_FILTERS];
		CTMMovieCtrl*		m_pParent;
		CSnapshot*			m_pSnapshot;
		CErrorHandler*		m_pErrors;
		CIDXShow*			m_pIDXShow;
		RECT				m_rcMaxWnd;
		long				m_lIDXShowId;
		BOOL				m_bAspect;
		BOOL				m_bScale;
		BOOL				m_bDetachBeforeLoad;
		BOOL				m_bHideTaskBar;
		short				m_sRate;
		short				m_sVolume;
		short				m_sBalance;
		int					m_iFilters;

		//	These members are used to simulate playback
		BOOL				m_bSimulating;
		double				m_dSimPosition;
		double				m_dSimStartPosition;
		double				m_dSimStopPosition;
		short				m_sSimState;
		ULONG				m_ulSimLastTime;

	public:
	
		//	These members are public to permit direct access from the
		//	control object
		float				m_fDefaultRate;
		float				m_fFrameRate;
		float				m_fAspectRatio; // Height / Width
		long				m_lWidth;
		long				m_lHeight;
		double				m_dMinTime;
		double				m_dMaxTime;
		int					m_iVideoSliderHeight;
		BOOL				m_bAdjRate;
		BOOL				m_bAdjVolume;
		BOOL				m_bAdjBalance;
		BOOL				m_bSnapshots;
		CString				m_strFilename;

							CPlayer();
		virtual			   ~CPlayer();

		//	Public access members
		void				Show(BOOL bShow, BOOL bRefresh);
		void				Resize();
		void				EnableSnapshots(BOOL bEnable);
		void				Redraw();
		void				SetVideoSliderHeight(int height) { m_iVideoSliderHeight = height; }
		long				ConvertToFrames(double dSeconds); 
		double				ConvertToTime(long lFrame);
		double				GetDuration(LPCSTR lpszFilename); 
		BOOL				GetSimulating(){ return m_bSimulating; }
		BOOL				Initialize(CTMMovieCtrl* pParent, 
									   CErrorHandler* pErrors);
		BOOL				AddFilter(LPCSTR lpszName);
		BOOL				RemoveFilter(LPCSTR lpszName);
		BOOL				Load(LPCSTR lpFilename);
		BOOL				Simulate(LPCSTR lpFilename);
		BOOL				IsAudio();
		BOOL				IsLoaded();
		BOOL				IsVisible();
		BOOL				Play(double dFrom, double dTo);
		BOOL				Pause();
		BOOL				Stop();
		BOOL				Unload();
		BOOL				GetEvent(long lId, long* pCode);
		BOOL				GetState(short *pState);
		BOOL				GetPos(double* pPosition);
		BOOL				GetStartPos(double* pPosition);
		BOOL				GetStopPos(double* pPosition);
		BOOL				SetPos(double dPosition);
		BOOL				SetRate(short sRate);
		BOOL				SetVolume(short sVolume);
		BOOL				SetBalance(short sBalance);
		BOOL				SetRange(double dStart, double dStop);
		BOOL				Step(double dFrom, double dTo);
		BOOL				SetScaleProps(BOOL bScale, BOOL bAspect);
		BOOL				SetDetachBeforeLoad(BOOL bDetach);
		BOOL				SetHideTaskBar(BOOL bHide);
		BOOL				Update();
		BOOL				Capture(CFile* pFile);
		BOOL				GetActFilters(CString& rFilters, long* pCount,
										  BOOL bVendorInfo);
		BOOL				GetRegFilters(CString& rFilters, long* pCount);
		BOOL				GetUserFilters(CString& rFilters, long* pCount);
		RECT*				GetVideoRect();
		HWND				GetRenderWnd();
		HBITMAP				GetDDBitmap(int* pWidth, int* pHeight);
		BITMAPINFOHEADER*	GetDIBitmap(long* pSize);
		CSnapshot*			GetSnapshot(BOOL bPopup = FALSE);
		LPVOID				GetInterface(short sInterface);

		//	IDXShow notifications
		void				OnIDXSetPos();
		void				OnIDXShow(BOOL bShow);
		void				OnIDXSetWnd();


	protected:

		BOOL				FindFilter(LPCSTR lpszName, CLSID* pClsId);
		BOOL				GetFilterMapper();
		void				ShowTaskBar();
		void				HideTaskBar();
		void				EndSimulation();
		CIDXShow*			GetIDXShow();
};

#endif // !defined(__PLAYER_H__)
