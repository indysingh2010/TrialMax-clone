//==============================================================================
//
// File Name:	idxshow.h
//
// Description:	This file contains the declaration of the CIDXShow class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-26-98	1.00		Original Release
//==============================================================================
#if !defined(__IDXSHOW_H__)
#define __IDXSHOW_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <windows.h>
#include <mmsystem.h>
#include <amstream.h>	//	DX Media SDK Headers
#include <control.h>
#include <handler.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Maximum number of filters to be added to the filter graph before rendering
#define IDXSHOW_MAX_FILTERS			16

//	Default frame rate
#define IDXSHOW_FRAMERATE			29.97f
#define IDXSHOW_SECONDS_PER_FRAME	0.033333

//	This message is used for event notifications
#define WM_IDXSHOWNOTIFY			WM_USER+13

#define RELEASE_INTERFACE(x) { if (x) x->Release(); x = 0; }

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

typedef struct
{
	CString			strName;
	CLSID			ClsId;
}SFilter;

//	Forward declarations
class CPlayer;

class CIDXShow
{
	private:

		IGraphBuilder*			m_pIGraphBuilder;
		IBasicVideo*			m_pIBasicVideo;
		IBasicAudio*			m_pIBasicAudio;
		IMediaControl*			m_pIMediaControl;
		IMediaEventEx*			m_pIMediaEventEx;
		IMediaPosition*			m_pIMediaPosition;
		IMediaSeeking*			m_pIMediaSeeking;
		IVideoWindow*			m_pIVideoWindow;

		CPlayer*				m_pPlayer;
		CErrorHandler*			m_pErrors;
		HWND					m_hParent;
		HWND					m_hRenderer;
		RECT					m_rcMaxWnd;
		RECT					m_rcVideoPos;
		BOOL					m_bScale;
		BOOL					m_bAspect;
		BOOL					m_bDetachBeforeLoad;
		int						m_iSeekMode;

	public:

		//	These members are public to permit direct access from the
		//	player that owns the object
		SFilter*				m_paFilters;
		HRESULT					m_hResult;
		long					m_lId;
		float					m_fFrameRate;
		float					m_fAspectRatio; // Width/Height
		long					m_lWidth;
		long					m_lHeight;
		double					m_dDuration;
		int						m_iFilters;
	
								CIDXShow(CPlayer* pPlayer, long lId);
		virtual				   ~CIDXShow();

		//	Public access members
		BOOL					Initialize(HWND hParent, CErrorHandler* pErrors);
		BOOL					Render(LPCSTR lpFile);
		BOOL					Pause();
		BOOL					Stop();
		BOOL					SetDetachBeforeLoad(BOOL bDetach);
		BOOL					SetRate(short sRate);
		BOOL					SetVolume(short sVolume);
		BOOL					SetBalance(short sBalance);
		BOOL					SetMaxRect(RECT* pRect);
		BOOL					SetScaleProps(BOOL bScale, BOOL bAspect);
		BOOL					Show(BOOL bShow, BOOL bRefresh);
		BOOL					GetEvent(long* pCode);
		BOOL					GetState(long* pState);
		BOOL					GetPos(double* pPosition);
		BOOL					GetStartPos(double* pPosition);
		BOOL					GetStopPos(double* pPosition);
		BOOL					GetRange(double* pStart, double* pStop);
		BOOL					SetPos(double dPosition, BOOL bRun);
		BOOL					UpdatePos(BOOL bRun);
		BOOL					Play(double dFrom, double dTo);
		BOOL					Step(double dFrom, double dTo);
		BOOL					SetRange(double dStart, double dStop, BOOL bRun);
		BOOL					SetVideoPos();
		BOOL					SetSeekMode(int iMode);
		BOOL					IsVisible();
		BOOL					IsAudio();
		BOOL					CanSeekFrames();
		BOOL					GetFilters(CString& rFilters, long* pCount,
										   BOOL bVendorInfo = TRUE);
		BOOL					ConvertToFrames(double dSeconds, long* pConverted);  
		BOOL					ConvertToTime(long lFrame, double* pConverted); 
		void					Redraw();
		void					OnTimer();
		BITMAPINFOHEADER*		GetDIBitmap(long* pSize);
		HBITMAP					GetDDBitmap(int* pWidth, int* pHeight);
		RECT*					GetVideoRect();
		HWND					GetRenderWnd();
		LPVOID					GetInterface(short sInterface);
		double					GetDuration(LPCSTR lpFile);
	
	protected:

		void					ReleaseAll();
		void					HandleError(LPCSTR lpszFormat, ...);
		void					CalcVideoRect();
		void					ResetWindow();
		void					ShowState();
		BOOL					GetInterface(REFIID Id, void** pInterface);
		BOOL					GetInterfaces();
		BOOL					GetVideoProps();
		BOOL					Run();
		double					ConvertPosition(LONGLONG llPosition);
		LONGLONG				ConvertTime(double dSeconds);
		IGraphBuilder*			GetGraphBuilder();
};

#endif // !defined(__IDXSHOW_H__)
