using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.ActiveX;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinToolbars;

namespace FTI.Trialmax.Controls
{
	/// <summary>Control used to play video designations</summary>
	public class CTmaxVideoPlayerCtrl : CTmaxVideoBaseCtrl
	{
		#region Constants
		
		/// <summary>Command identifiers</summary>
		protected enum TmaxVideoPlayerCommands
		{
			Invalid = 0,
			PlayPause,
			Back5,
			Back1,
			StepBack,
			Preview,
			StepFwd,
			Fwd1,
			Fwd5,
			Apply,
		}
		
		protected const double VIDEO_CUE_LARGE		= 5.00;
		protected const double VIDEO_CUE_SMALL		= 1.00;
		protected const double VIDEO_CUE_STEP		= 0.10;
		protected const double VIDEO_CUE_TOLERANCE	= 0.033333;
		
		protected const int ERROR_SET_TUNE_MODE_EX		= 0;
		protected const int ERROR_FIRE_VIDEO_EVENT_EX	= 1;
		protected const int ERROR_SET_POSITION_EX		= 2;
		
		#endregion Constants
		
		#region Private Members

		private System.ComponentModel.IContainer components;
		
		/// <summary>Local member bound to EventSource property</summary>
		protected TmaxVideoCtrlTuneModes m_eTuneMode = TmaxVideoCtrlTuneModes.None;

		/// <summary>Image list used for toolbar buttons</summary>
		protected System.Windows.Forms.ImageList m_ctrlImages;

		/// <summary>Infragistics library toolbar manager</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlToolbarManager;

		/// <summary>Left hand docking zone used by toolbar manager</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _TmaxVideoCtrl_Toolbars_Dock_Area_Left;

		/// <summary>Right hand docking zone used by toolbar manager</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _TmaxVideoCtrl_Toolbars_Dock_Area_Right;

		/// <summary>Top docking zone used by toolbar manager</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _TmaxVideoCtrl_Toolbars_Dock_Area_Top;

		/// <summary>Bottom docking zone used by toolbar manager</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _TmaxVideoCtrl_Toolbars_Dock_Area_Bottom;

		/// <summary>Media viewer control used to play video</summary>
		protected FTI.Trialmax.Controls.CTmaxViewerCtrl m_ctrlViewer;

		/// <summary>Background fill panel used to manage docking</summary>
		protected System.Windows.Forms.Panel m_ctrlPanel;

		/// <summary>Local member to keep track of active transcript element</summary>
		private int m_iTranscript = -1;
		
		/// <summary>Local member to keep track of active link element</summary>
		private int m_iLink = -1;
		
		/// <summary>Static text control to display current transcript text</summary>
		private System.Windows.Forms.Label m_ctrlTranscriptText;
		
		/// <summary>Local member bound to ShowPosition property</summary>
		protected bool m_bShowPosition = true;
		
		/// <summary>Local member bound to AllowApply property</summary>
		protected bool m_bAllowApply = true;
		
		/// <summary>Local member bound to ShowTranscript property</summary>
		protected bool m_bShowTranscript = true;
		
		/// <summary>Local member bound to ShowTuner property</summary>
		protected bool m_bShowTuner = true;
		
		/// <summary>Local member to indicate if we are previewing</summary>
		protected bool m_bPreviewing = false;
		
		/// <summary>Status bar used to display position information</summary>
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlPosition;
		
		/// <summary>Local member bound to Position property</summary>
		protected double m_dPosition = (double)-1;
		
		/// <summary>Local member bound to PreviewPeriod property</summary>
		protected double m_dPreviewPeriod = 0;
		
		/// <summary>Local member to keep track of return position after previewing</summary>
		protected double m_dPreviewReturn = -1;
		
		#endregion Protected Members

		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoPlayerCtrl() : base()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Video Player Control";
			
			//	Initialize the child controls
			InitializeComponent();
			
			//	Attach to the viewer's event interfaces
			m_ctrlViewer.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.m_tmaxEventSource.OnError);
			m_ctrlViewer.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.m_tmaxEventSource.OnDiagnostic);
			m_ctrlViewer.TmxEvent += new FTI.Trialmax.ActiveX.TmxEventHandler(this.OnTmxEvent);
		}

		/// <summary>This method is called to set the control properties</summary>
		/// <param name="strFileSpec">The fully qualified file specification used to set property values</param>
		/// <param name="xmlDesignation">The designation used to set property values</param>
		/// <returns>true if successful</returns>
		public override bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		{
			//	Are we unloading?
			if((strFileSpec == null) || (strFileSpec.Length == 0))
			{
				return Unload();
			}
			else
			{
				//	Save the new values
				m_strFileSpec    = strFileSpec;
				m_xmlDesignation = xmlDesignation;

				//	Reset the playback values
				m_dPosition = (double)-1;
				m_iTranscript = -1;
				m_iLink = -1;
				m_xmlLink = null;

				//	Load the file
				//
				//	NOTE:	This only starts the playback if PlayOnLoad==true
				if(Play() == false)
					return false;
					
				//	Should we start playing?
				if(m_bPlayingScript)
				{
					return m_ctrlViewer.Play();
				}
				else
				{
					return true;
				}
			}

		}// public virtual bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to set the control properties</summary>
		/// <param name="xmlLink">The link used to set property values</param>
		/// <returns>true if successful</returns>
		public override bool SetProperties(CXmlLink xmlLink)
		{
			m_xmlLink = xmlLink;
			
			//	Update the control states
			return OnAttributesChanged(xmlLink);
		}
		
		/// <summary>This method is called when the attributes associated with the active designation have changed</summary>
		/// <param name="xmlDesignation">The designation who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public override bool OnAttributesChanged(CXmlDesignation xmlDesignation)
		{
			Debug.Assert(ReferenceEquals(m_xmlDesignation, xmlDesignation) == true);
			if(ReferenceEquals(m_xmlDesignation, xmlDesignation) == false) return false;
			
			//	Refresh the controls
			RefreshAll();
			
			//	Make sure the transcript text didn't change
			if((m_iTranscript >= 0) && (m_xmlDesignation != null))
			{
				if(m_ctrlTranscriptText.Text != m_xmlDesignation.Transcripts[m_iTranscript].GetFormattedText())
					m_ctrlTranscriptText.Text = m_xmlDesignation.Transcripts[m_iTranscript].GetFormattedText();
			}
			else
			{
				m_ctrlTranscriptText.Text = "";
			}
			
			return true;
		
		}// public override bool OnAttributesChanged(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called when the attributes associated with the active designation have changed</summary>
		/// <param name="xmlLink">The designation who's attributes have changed</param>
		/// <returns>true if successful</returns>
		public override bool OnAttributesChanged(CXmlLink xmlLink)
		{
			//	Refresh the controls
			RefreshAll();
			
			//	Make sure we are properly positioned 
			//
			//	NOTE:	The link's start position may have changed on 
			//			the link's page so this will keep things in sync
			if((m_xmlLink != null) && (m_eTuneMode == TmaxVideoCtrlTuneModes.Link))
			{
				SetTuneMode(TmaxVideoCtrlTuneModes.Link);
			}
				
			return true;
		
		}// public override bool OnAttributesChanged(CXmlLink xmlLink)
		
		/// <summary>This method handles all video events fired by the player and tune bar</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">event arguments</param>
		public override void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
		{
			//	Which event?
			switch(e.EventId)
			{
				case TmaxVideoCtrlEvents.SetMode:
				
					SetTuneMode(e.TuneMode);
					break;
					
				case TmaxVideoCtrlEvents.SetPreviewPeriod:
				
					if(e.PreviewPeriod > 0)
						PreviewPeriod = e.PreviewPeriod;
					break;
					
				case TmaxVideoCtrlEvents.QueryPlayerPosition:
				
					e.Position = PlayerPosition;
					e.QueryHandled = true;
					break;
					
				default:
				
					break;
					
			}
			
		}// protected void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
	
		/// <summary>This method is called when the user wants to start playing a script</summary>
		/// <returns>true if successful</returns>
		public override bool StartScript()
		{
			m_bPlayingScript = true;
			RefreshAll();
			return true;
							
		}// public virtual bool StartScript()
		
		/// <summary>This method is called when the user wants to stop playing a script</summary>
		/// <returns>true if successful</returns>
		public override bool StopScript()
		{
			m_ctrlViewer.Pause();
			m_bPlayingScript = false;
			RefreshAll();
			return true;
			
		}// public virtual bool StopScript()
		
		/// <summary>This function is called to play the specified designation</summary>
		/// <param name="strFileSpec">The file to be played</param>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <returns>true if successful</returns>
		public bool Play(string strFileSpec, double dStart, double dStop)
		{
			Debug.Assert(strFileSpec != null);
			Debug.Assert(strFileSpec.Length > 0);
			Debug.Assert(m_ctrlViewer != null);
			Debug.Assert(m_ctrlViewer.IsDisposed == false);
			if(strFileSpec == null) return false;
			if(strFileSpec.Length == 0) return false;
			if(m_ctrlViewer == null) return false;
			if(m_ctrlViewer.IsDisposed == true) return false;
			
			//	Are we previewing?
			if(m_bPreviewing == true)
			{
				m_ctrlViewer.Pause();
				m_bPreviewing = false;
			}
			
			//	Reset the local members
			m_dPosition = (double)-1;
			m_iTranscript = -1;
			m_iLink = -1;

			if(m_ctrlViewer.Play(strFileSpec, dStart, dStop, false) == true)
			{
				m_strFileSpec = strFileSpec;
				m_dPosition   = m_ctrlViewer.Position;
			}
			
			//	Refresh all the control values
			RefreshAll();			
			
			return (m_dPosition >= 0);
				
		}// public bool Play(string strFileSpec, double dStart, double dStop)

		/// <summary>This function is called to play the specified designation</summary>
		/// <param name="xmlDesignation">The designation used to define the extents</param>
		/// <returns>true if successful</returns>
		public bool Play(CXmlDesignation xmlDesignation)
		{
			//	Set the extents
			SetProperties(m_strFileSpec, xmlDesignation);
			
			// NOTE: This assumes FileSpec has been set
			return Play(m_strFileSpec, StartPosition, StopPosition);
		}
			
		/// <summary>This function is called to play the video defined by the local properties</summary>
		/// <returns>true if successful</returns>
		public bool Play()
		{
			return Play(StartPosition, StopPosition);
		}

		/// <summary>This function is called to play the current file</summary>
		/// <param name="dStart">The start position</param>
		/// <param name="dStop">The stop position</param>
		/// <returns>true if successful</returns>
		public bool Play(double dStart, double dStop)
		{
			if(m_ctrlViewer == null) return false;
			if(m_ctrlViewer.IsDisposed == true) return false;
			
			//	Are we previewing?
			if(m_bPreviewing == true)
			{
				m_ctrlViewer.Pause();
				m_bPreviewing = false;
			}
				
			//	Do we have a file specification?
			//
			//	NOTE:	The viewer is smart enough not to reload the file if it's
			//			already loaded
			if((m_strFileSpec != null) && (m_strFileSpec.Length > 0))
			{
				return Play(m_strFileSpec, dStart, dStop);
			}
			else
			{
				return false;
			}
		
		}// public bool Play(double dStart, double dStop)
			
		/// <summary>This function is called to set the tune mode</summary>
		/// <param name="eMode">The new tune mode</param>
		/// <returns>true if successful</returns>
		public void SetTuneMode(TmaxVideoCtrlTuneModes eMode)
		{
			//	Update the property value
			m_eTuneMode = eMode;
			
			//	Stop here if not loaded
			if(m_ctrlViewer.IsLoaded() == false) return;
			
			//	Are we previewing?
			if(m_bPreviewing == true)
			{
				m_ctrlViewer.Pause();
				m_bPreviewing = false;
			}
			
			try
			{
				//	Should we cue the video?
				switch(eMode)
				{
					case TmaxVideoCtrlTuneModes.Start:
					
						if(StartPosition >= 0)
							m_ctrlViewer.Cue(TmxCueModes.Absolute, StartPosition, false);
						break;
						
					case TmaxVideoCtrlTuneModes.Stop:
					
						if(StopPosition >= 0)
							m_ctrlViewer.Cue(TmxCueModes.Absolute, StopPosition, false);
						break;
						
					case TmaxVideoCtrlTuneModes.Link:
					
						if(LinkPosition >= 0)
							m_ctrlViewer.Cue(TmxCueModes.Absolute, LinkPosition, false);
						break;
						
					default:
					
						break;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetTuneMode", m_tmaxErrorBuilder.Message(ERROR_SET_TUNE_MODE_EX, eMode), Ex);
			}
			
			SetCommandStates();
			
		}// public void SetTuneMode(TmaxVideoCtrlTuneModes eMode)
		
		/// <summary>This function is called to set the current position</summary>
		/// <param name="dPosition">The desired position</param>
		/// <returns>true if successful</returns>
		public bool SetPosition(double dPosition)
		{
			bool bSuccessful = false;
			
			//	Stop here if not loaded
			if(m_ctrlViewer.IsLoaded() == false) return false;
			
			//	Are we previewing?
			if(m_bPreviewing == true)
			{
				m_ctrlViewer.Pause();
				m_bPreviewing = false;
			}
			
			try
			{
				//	Is the specified position within range?
				if((dPosition >= m_ctrlViewer.MinimumPosition) && (dPosition <= m_ctrlViewer.MaximumPosition))
				{
					m_ctrlViewer.Cue(TmxCueModes.Absolute, dPosition, false);

					SetCommandStates();

					bSuccessful = true;
					
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetPosition", m_tmaxErrorBuilder.Message(ERROR_SET_POSITION_EX, dPosition), Ex);
			}
			
			return bSuccessful;
			
		}// public bool SetPosition(double dPosition)
		
		/// <summary>This function is called to stop the playback<summary>
		public void Stop()
		{
			if(m_ctrlViewer != null)
				m_ctrlViewer.Stop();		
				
			SetCommandStates();
		}

		/// <summary>This function is called to resume the playback<summary>
		public void Resume()
		{
			if(m_ctrlViewer != null)
				m_ctrlViewer.Resume();		
				
			SetCommandStates();
		}

        ///// <summary>
        ///// Get the duration of the provided file name
        ///// </summary>
        ///// <param name="strFileName">Absolute path of the video file</param>
        ///// <returns>Duration of the video file</returns>
        //public double GetDuration(string strFileName)
        //{
        //    try
        //    {
        //        return m_ctrlViewer.GetDuration(strFileName);
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}// public double GetDuration(string strFileName)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to refresh all the controls</summary>
		protected void RefreshAll()
		{
			UpdateTranscriptBar();
			UpdatePlayerLink();
			UpdatePositionBar();				
			SetCommandStates();
			
		}// public void RefreshAll()
		
		/// <summary>This function is called to unload the video<summary>
		protected bool Unload()
		{
			try
			{
				if((m_ctrlViewer != null) && (m_ctrlViewer.IsLoaded() == true))
					m_ctrlViewer.Unload();
			}
			catch
			{
			}
			
			m_strFileSpec = "";
			m_xmlDesignation = null;
			m_dPosition = (double)-1;
			m_iTranscript = -1;
			m_iLink = -1;
			
			//	Reset all the visual controls
			return OnAttributesChanged(m_xmlDesignation);
		
		}// public void Unload()

		/// <summary>This method handles the PlayPause command</summary>
		protected void OnCmdPlayPause()
		{
			Debug.Assert(m_ctrlViewer != null);
			Debug.Assert(m_ctrlViewer.IsDisposed == false);

			//	Is the viewer playing?
			if(m_ctrlViewer.State == TmxStates.Playing)
			{
				//	Pause the playback
				m_ctrlViewer.Pause();
			}
			else
			{
				//	Are we at the end of the range?
				if(m_ctrlViewer.Position >= (m_xmlDesignation.Stop - VIDEO_CUE_TOLERANCE))
				{
					//	Are we tuning video?
					if(m_eTuneMode == TmaxVideoCtrlTuneModes.None)
					{
						//	Cue to the start before we play
						m_ctrlViewer.Cue(TmxCueModes.Start, 0, true);
					}
					else
					{
						m_ctrlViewer.Play();
					}
					
				}
				else
				{
					m_ctrlViewer.Resume();
				}
				
			}
		
			SetCommandStates();

		}// protected void OnCmdPlayPause()
		
		/// <summary>This method translates a TmxStates enumeration to a TmaxVideoCtrlStates enumeration</summary>
		/// <param name="eTmxState">The valid TmxStates enumeration</param>
		/// <returns>The equivalent TmaxVideoCtrlStates enumeration</returns>
		protected TmaxVideoCtrlStates TranslateTmxState(TmxStates eTmxState)
		{
			switch(eTmxState)
			{
				case TmxStates.Playing:		return TmaxVideoCtrlStates.Playing;
				case TmxStates.Paused:		return TmaxVideoCtrlStates.Paused;
				case TmxStates.Stopped:		return TmaxVideoCtrlStates.Stopped;
				case TmxStates.Loaded:		return TmaxVideoCtrlStates.Loaded;
				case TmxStates.Unitialized:
				case TmxStates.Unloaded:	return TmaxVideoCtrlStates.NotReady;
				default:					return TmaxVideoCtrlStates.Invalid;
			}
			
		}// protected TmaxVideoCtrlStates TranslateTmxState(TmxStates eTmxState)
		
		/// <summary>This method handles all cue related commands</summary>
		/// <param name="eCommand">The video command enumerator</param>
		protected void OnCmdCue(TmaxVideoPlayerCommands eCommand)
		{
			double dTime = 0;
			
			Debug.Assert(m_ctrlViewer != null);
			Debug.Assert(m_ctrlViewer.IsDisposed == false);

			//	Make sure we have the current position and verify that
			//	we are not too close to the start
			m_dPosition = m_ctrlViewer.Position;
			if(GetCommandEnabled(eCommand) == false) return;
			
			//	Get the time associated with this command
			if((dTime = GetTime(eCommand)) != 0)
			{
				//	If stepping forward we want to use a special command that
				//	will permit us to hear the audio while the video is cued
				if(eCommand == TmaxVideoPlayerCommands.StepFwd)
					m_ctrlViewer.Step(dTime);
				else
					m_ctrlViewer.Cue(TmxCueModes.Relative, dTime, false);
				
				SetCommandStates();
			}
		
		}// protected void OnCmdCue(TmaxVideoPlayerCommands eCommand)
		
		/// <summary>This method handles the Preview command</summary>
		protected void OnCmdPreview()
		{
			double	dFrom;
			double	dTo;
			
			//	What tune mode are we in?
			if(m_eTuneMode == TmaxVideoCtrlTuneModes.Start)
			{
				dFrom = m_ctrlViewer.Position;
				dTo   = dFrom + m_dPreviewPeriod;
				
				if(dTo > m_ctrlViewer.MaximumPosition)
					dTo = m_ctrlViewer.MaximumPosition;
					
				m_dPreviewReturn = dFrom;
			}
			else
			{
				dTo = m_ctrlViewer.Position;
				dFrom = dTo - m_dPreviewPeriod;
				
				if(dFrom < m_ctrlViewer.MinimumPosition)
					dFrom = m_ctrlViewer.MinimumPosition;

				m_dPreviewReturn = -1;
			}
			
			//	Use Step() so that we don't effect the stored
			//	start and stop positions
			m_ctrlViewer.Step(dFrom, dTo);
			
			m_bPreviewing = true;
			SetCommandStates();
			
		}// protected void OnCmdPreview()
		
		/// <summary>This method handles the Apply command</summary>
		protected void OnCmdApply()
		{
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			if(m_xmlDesignation != null)
			{				
				//	Fire the video event
				Args.EventId = TmaxVideoCtrlEvents.Apply;
				FireTmaxVideoCtrlEvent(Args);
			}
			
		}// protected void OnCmdApply()
		
		/// <summary>This method is called to get the time period associated with the specified command</summary>
		/// <param name="eCommand">The video command enumerator</param>
		/// <returns>The time in seconds</returns>
		protected double GetTime(TmaxVideoPlayerCommands eCommand)
		{
			double dTime = 0;
			double dCompare = 0;
			
			switch(eCommand)
			{
				case TmaxVideoPlayerCommands.Back5:	
				
					dCompare = (m_eTuneMode == TmaxVideoCtrlTuneModes.None) ? StartPosition : m_ctrlViewer.MinimumPosition;
					
					if(m_dPosition > dCompare)
					{
						if((m_dPosition - dCompare) >= VIDEO_CUE_LARGE)
							dTime = (VIDEO_CUE_LARGE * -1.0);
						else
							dTime = ((m_dPosition - dCompare) * -1.0);
					}
					break;
											
				case TmaxVideoPlayerCommands.Fwd5:	
				
					dCompare = (m_eTuneMode == TmaxVideoCtrlTuneModes.None) ? StopPosition : m_ctrlViewer.MaximumPosition;
					
					if(m_dPosition < dCompare)
					{
						if((dCompare - m_dPosition) >= VIDEO_CUE_LARGE)
							dTime = VIDEO_CUE_LARGE;
						else
							dTime = (dCompare - m_dPosition);
					}
					break;
											
				case TmaxVideoPlayerCommands.Back1:	
				
					dCompare = (m_eTuneMode == TmaxVideoCtrlTuneModes.None) ? StartPosition : m_ctrlViewer.MinimumPosition;
					
					if(m_dPosition > dCompare)
					{
						if((m_dPosition - dCompare) >= VIDEO_CUE_SMALL)
							dTime = (VIDEO_CUE_SMALL * -1.0);
						else
							dTime = ((m_dPosition - dCompare) * -1.0);
					}
					break;
											
				case TmaxVideoPlayerCommands.Fwd1:	
				
					dCompare = (m_eTuneMode == TmaxVideoCtrlTuneModes.None) ? StopPosition : m_ctrlViewer.MaximumPosition;
					
					if(m_dPosition < dCompare)
					{
						if((dCompare - m_dPosition) >= VIDEO_CUE_SMALL)
							dTime = VIDEO_CUE_SMALL;
						else
							dTime = (dCompare - m_dPosition);
					}
					break;
											
				case TmaxVideoPlayerCommands.StepBack:	
				
					dCompare = (m_eTuneMode == TmaxVideoCtrlTuneModes.None) ? StartPosition : m_ctrlViewer.MinimumPosition;
					
					if(m_dPosition > dCompare)
					{
						if((m_dPosition - dCompare) >= VIDEO_CUE_STEP)
							dTime = (VIDEO_CUE_STEP * -1.0);
						else
							dTime = ((m_dPosition - dCompare) * -1.0);
					}
					break;
											
				case TmaxVideoPlayerCommands.StepFwd:	
				
					dCompare = (m_eTuneMode == TmaxVideoCtrlTuneModes.None) ? StopPosition : m_ctrlViewer.MaximumPosition;
					
					if(m_dPosition < dCompare)
					{
						if((dCompare - m_dPosition) >= VIDEO_CUE_STEP)
							dTime = VIDEO_CUE_STEP;
						else
							dTime = (dCompare - m_dPosition);
					}
					break;
											
				default:						
					
					Debug.Assert(false);
					break;
			}
			
			return dTime;
		
		}// protected double GetTime(TmaxVideoPlayerCommands eCommand)
		
		/// <summary>This method handles all TmxEvents trapped by this control</summary>
		/// <param name="O">The object sending the event</param>
		/// <param name="Args">Tmx event arguments</param>
		protected void OnTmxEvent(object O, FTI.Trialmax.ActiveX.CTmxEventArgs Args)
		{
			//	What type of event?
			switch(Args.Event)
			{
				case TmxEvents.StateChange:
				
					OnTmxStateChange();
					break;
					
				case TmxEvents.PositionChange:
					
					OnTmxPositionChange();
					break;
					
				default:
				
					break;
			}
		
		}// protected void OnTmxEvent(object O, FTI.Trialmax.ActiveX.CTmxEventArgs Args)
			
		/// <summary>This method is called when the player's position changes<summary>
		protected void OnTmxPositionChange()
		{
			if(m_ctrlViewer == null) return;
			if(m_ctrlViewer.IsDisposed == true) return;
			if(m_ctrlTranscriptText == null) return;
			if(m_ctrlTranscriptText.IsDisposed == true) return;
			
			//	Update the local position value
			m_dPosition = m_ctrlViewer.Position;
			
			//	Make sure the application has the latest position
			FirePositionChanged(m_dPosition);
		
			//	Refresh the controls
			RefreshAll();
		
		}// protected void OnTmxPositionChange()
		
		/// <summary>This function is called when the player's state changes<summary>
		protected void OnTmxStateChange()
		{
			//	Do we need to reset the preview mode?
			if(m_ctrlViewer.State != TmxStates.Playing)
			{
				if(m_bPreviewing)
				{
					m_bPreviewing = false;
					if(m_dPreviewReturn > 0)
					{
						m_ctrlViewer.Cue(TmxCueModes.Absolute, m_dPreviewReturn, false);
					}
				}
				
							
			}
			
			//	Have we reached the end of a designation?
			if((m_ctrlViewer.State == TmxStates.Paused) ||
			   (m_ctrlViewer.State == TmxStates.Stopped))
			{
				if(m_xmlDesignation != null)
				{
					if(System.Math.Abs(PlayerPosition - m_xmlDesignation.Stop) <= VIDEO_CUE_TOLERANCE)
					{
						FireDesignationComplete(m_bPlayingScript);
					}							
				}
			}
			
			//	Notify the system
			FireStateChanged(m_ctrlViewer.State);
			
			//	Update the command states
			SetCommandStates();
		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the tuner mode. mode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing a video event. event = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the video position: seconds = %1");

		}// protected void SetErrorStrings()

		/// <summary>This method is called enable/disable the command buttons</summary>
		protected void SetCommandStates()
		{
			TmaxVideoPlayerCommands eCommand;
			
			Debug.Assert(m_ctrlToolbarManager != null);
			Debug.Assert(m_ctrlToolbarManager.Tools != null);
			if((m_ctrlToolbarManager == null) ||( m_ctrlToolbarManager.Tools == null)) return;
			
			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ctrlToolbarManager.Tools)
			{
				if(O.Key == null) 
					continue;

				if((eCommand = GetCommand(O.Key)) == TmaxVideoPlayerCommands.Invalid)
					continue;
					
				try
				{
					//	Should the command button be visible?
					if(GetCommandVisible(eCommand) == true)
					{
						O.SharedProps.Visible = true;
						
						//	Should the command be enabled?
						O.SharedProps.Enabled = GetCommandEnabled(eCommand);
					}
					else
					{
						O.SharedProps.Visible = false;
					}
					
					//	Is this the PlayPause button?
					if(eCommand == TmaxVideoPlayerCommands.PlayPause)
					{
						//	Is video playing?
						if(m_ctrlViewer.State == TmxStates.Playing)
						{
							O.SharedProps.AppearancesSmall.Appearance.Image = 1;
							O.SharedProps.ToolTipText = "Pause";
						}
						else
						{
							O.SharedProps.AppearancesSmall.Appearance.Image = 0;
							O.SharedProps.ToolTipText = "Play";
						}
						
					}// if(eCommand == TmaxVideoPlayerCommands.PlayPause)
					
					//	Has a link position been specified?
					if(LinkPosition >= 0)
					{
						m_ctrlPosition.Panels[2].Visible = true;
					}
					else
					{
						m_ctrlPosition.Panels[2].Visible = false;
					}
				}
				catch
				{
				}

			}// foreach(ToolBase ultraTool in m_ctrlToolbarManager.Tools			
		
		}// protected void SetCommandStates()

		/// <summary>This method is called to update the position bar information using the current position<summary>
		protected void UpdatePositionBar()
		{
			if(m_ctrlViewer == null) return;
			if(m_ctrlViewer.IsDisposed == true) return;
			if(m_ctrlPosition == null) return;
			if(m_ctrlPosition.IsDisposed == true) return;
			
			//	Don't bother if turned off
			if(m_bShowPosition == false) return;
			
			//	Update the associated panel in the position bar
			if(PlayerPosition >= 0)
				m_ctrlPosition.Panels[0].Text = CTmaxToolbox.SecondsToString(PlayerPosition, 1);
			else
				m_ctrlPosition.Panels[0].Text = "";
			
			//	Are we on one of the end points?
			if(PlayerPosition > 0)
			{
				if(System.Math.Abs(PlayerPosition - StartPosition) <= VIDEO_CUE_TOLERANCE)
				{
					m_ctrlPosition.Panels[1].Enabled = true;
					m_ctrlPosition.Panels[3].Enabled = false;
				}
				else if(System.Math.Abs(PlayerPosition - StopPosition) <= VIDEO_CUE_TOLERANCE)
				{
					m_ctrlPosition.Panels[1].Enabled = false;
					m_ctrlPosition.Panels[3].Enabled = true;
				}
				else
				{
					m_ctrlPosition.Panels[1].Enabled = false;
					m_ctrlPosition.Panels[3].Enabled = false;
				}

				//	NOTE: Link position could be the same as start or stop
				if((LinkPosition >= 0) && (System.Math.Abs(PlayerPosition - LinkPosition) <= VIDEO_CUE_TOLERANCE))
				{
					m_ctrlPosition.Panels[2].Enabled = true;
				}
				else
				{
					m_ctrlPosition.Panels[2].Enabled = false;
				}
				
			}
			else
			{
				m_ctrlPosition.Panels[1].Enabled = false;
				m_ctrlPosition.Panels[2].Enabled = false;
				m_ctrlPosition.Panels[3].Enabled = false;
			}
			
		}// protected void UpdatePositionBar()
		
		/// <summary>This method is called to update the transcript text for the current position<summary>
		protected void UpdateTranscriptBar()
		{
			int	iTranscript = -1;
			CXmlTranscript xmlTranscript = null;
			
			if(m_ctrlViewer == null) return;
			if(m_ctrlViewer.IsDisposed == true) return;
			if(m_ctrlTranscriptText == null) return;
			if(m_ctrlTranscriptText.IsDisposed == true) return;
			
			//	Do we have any transcript information?
			if((m_xmlDesignation != null) && (m_xmlDesignation.Transcripts != null))
			{
				//	Get the transcript for the current position
				iTranscript = m_xmlDesignation.Transcripts.Locate(m_iTranscript, m_dPosition, false);
			}
			
			//	Has the text changed?
			if(m_iTranscript != iTranscript)
			{
				m_iTranscript = iTranscript;
				
				//	Set the display text
				if(m_iTranscript >= 0)
					xmlTranscript = m_xmlDesignation.Transcripts[m_iTranscript];
					
				if(xmlTranscript != null)
					m_ctrlTranscriptText.Text = xmlTranscript.GetFormattedText();
				else
					m_ctrlTranscriptText.Text = "";
					
				//	Fire the event
				FireTranscriptChanged(xmlTranscript, m_iTranscript, m_bPlayingScript);

			}
		
		}// protected void UpdateTranscriptBar()
		
		/// <summary>This method is called to update the xml link for the current position<summary>
		protected void UpdatePlayerLink()
		{
			int			iLink = -1;
			CXmlLink	xmlLink = null;
			
			//	Do we have any link information?
			if((m_xmlDesignation != null) && (m_xmlDesignation.Links != null))
			{
				//	Get the transcript for the current position
				iLink = m_xmlDesignation.Links.Locate(m_iLink, m_dPosition, false);
			}
			
			//	Has the text changed?
			if(m_iLink != iLink)
			{
				m_iLink = iLink;
				
				if(m_iLink >= 0)
					xmlLink = m_xmlDesignation.Links[m_iLink];
					
				//	Fire the event
				FireLinkChanged(xmlLink, m_iLink, m_bPlayingScript);

			}
		
		}// protected void UpdatePlayerLink()
		
		/// <summary>
		/// This method is called internally to convert the key passed in
		///	an Infragistics event to its associated command enumeration
		/// </summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated command</returns>
		protected TmaxVideoPlayerCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(TmaxVideoPlayerCommands));
				
				foreach(TmaxVideoPlayerCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return TmaxVideoPlayerCommands.Invalid;
		}
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The command enumeration</param>
		/// <returns>true if command should be enabled</returns>
		protected virtual bool GetCommandEnabled(TmaxVideoPlayerCommands eCommand)
		{
			//	Do we have a valid viewer control?
			if((m_ctrlViewer == null) || (m_ctrlViewer.IsDisposed == true))
				return false;
				
			//	Has the viewer been loaded?
			if(m_ctrlViewer.IsLoaded() == false) return false;
			
			//	Are we playing a script?
			if(m_bPlayingScript == true) return false;
			
			//	Do we have a valid position?
			if(m_dPosition < 0) return false;
			
			//	Are we previewing
			if(m_bPreviewing == true) return false;
			
			//	What is the command?
			switch(eCommand)
			{
				case TmaxVideoPlayerCommands.PlayPause:
				
					//	Just need to be loaded
					return true;
				
				case TmaxVideoPlayerCommands.Back5:
				case TmaxVideoPlayerCommands.Back1:
				case TmaxVideoPlayerCommands.StepBack:

					//	Are we tuning the video
					if(m_eTuneMode != TmaxVideoCtrlTuneModes.None)
					{
						if(m_dPosition <= 0) return false;
					}
					else
					{
						//	Are we before the start of the range?
						if(m_dPosition <= (StartPosition + VIDEO_CUE_TOLERANCE))
							return false;
					}
						
					//	All conditions have been satisfied
					return true;
					
				case TmaxVideoPlayerCommands.StepFwd:
				case TmaxVideoPlayerCommands.Fwd1:
				case TmaxVideoPlayerCommands.Fwd5:
					
					//	Are we tuning the video?
					if(m_eTuneMode != TmaxVideoCtrlTuneModes.None)
					{
					}
					else
					{
						//	Are we after the end of the range?
						if(m_dPosition >= (StopPosition - VIDEO_CUE_TOLERANCE))
							return false;
					}
						
					//	All conditions have been satisfied
					return true;
					
				case TmaxVideoPlayerCommands.Preview:
				
					if(m_eTuneMode == TmaxVideoCtrlTuneModes.None) return false;
					if(m_dPreviewPeriod <= 0) return false;
					if(GetModePosition(m_eTuneMode) < 0) return false;
					
					return true;
					
				case TmaxVideoPlayerCommands.Apply:
				
					if(m_xmlDesignation == null) return false;
					if((m_xmlDesignation.HasText == true) && 
					   (m_xmlDesignation.GetSynchronized(false) == false)) return false;
					   
					//	Everything is OK
					return true;
					
				default:
				
					break;
			}	
			
			return false;
		
		}// protected virtual bool GetCommandEnabled(TmaxVideoPlayerCommands eCommand, int iSelections)
		
		/// <summary>This method is called to determine if the specified command should be visible</summary>
		/// <param name="eCommand">The command enumeration</param>
		/// <returns>true if command should be visible</returns>
		protected bool GetCommandVisible(TmaxVideoPlayerCommands eCommand)
		{
			if(eCommand == TmaxVideoPlayerCommands.Preview)
			{
				return (PreviewPeriod > 0);
			}
			else if(eCommand == TmaxVideoPlayerCommands.Apply)
			{
				return (m_bAllowApply == true);
			}
			else
			{
				return true;
			}
		
		}// protected virtual bool GetCommandVisible(TmaxVideoPlayerCommands eCommand, int iSelections)
		
		/// <summary>This function will retrieve the tool with the specified key from the toolbar manager</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		/// <returns>Infragistic base class tool object</returns>
		protected ToolBase GetUltraTool(string strKey)
		{
			ToolBase Tool = null;
					
			try
			{
				if(m_ctrlToolbarManager != null)
				{
					Tool = m_ctrlToolbarManager.Tools[strKey];
				}
			
			}
			catch(System.Exception )
			{
				//FireError("GetUltraTool", m_tmaxErrorBuilder.Message(ERROR_GET_ULTRA_TOOL_EX, strKey), Ex);
			}
			
			return Tool;
		
		}// GetUltraTool()
				
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
				
			base.Dispose(disposing);
		}
		
		/// <summary>This method fires the event to inform the system when the player position changes</summary>
		/// <param name="dPosition">The position to be provided with the event</param>
		protected void FirePositionChanged(double dPosition)
		{
			//	Create the event argument
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			Args.EventId  = TmaxVideoCtrlEvents.PlayerPositionChanged;
			Args.Position = dPosition;
			
			//	Fire the event
			FireTmaxVideoCtrlEvent(Args);
		
		}// protected void FirePositionChanged(double dPosition)

		/// <summary>This method fires the event to inform the system when the player state changes</summary>
		/// <param name="eState">The current TmaxVideoState value</param>
		protected void FireStateChanged(TmaxVideoCtrlStates eState)
		{
			//	Create the event argument
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			Args.EventId  = TmaxVideoCtrlEvents.PlayerStateChanged;
			Args.State    = eState;
			Args.Position = m_dPosition;
			
			//	Fire the event
			FireTmaxVideoCtrlEvent(Args);
		
		}// protected void FirePositionChanged(double dPosition)

		/// <summary>This method fires the event to inform the system when the player state changes</summary>
		/// <param name="eTmxState">The current TmxStates value</param>
		protected void FireStateChanged(TmxStates eTmxState)
		{
			TmaxVideoCtrlStates eState = TranslateTmxState(eTmxState);
			
			if(eState != TmaxVideoCtrlStates.Invalid)
				FireStateChanged(eState);
		
		}// protected void FirePositionChanged(double dPosition)

		/// <summary>This method fires the event to inform the system when the transcript associated with the player position changes</summary>
		/// <param name="xmlTranscript">The active transcript node</param>
		/// <param name="iTranscript">The index of the active transcript</param>
		/// <param name="bScript">True to fire script playback event instead of Player event</param>
		protected void FireTranscriptChanged(CXmlTranscript xmlTranscript, int iTranscript, bool bScript)
		{
			//	Create the event argument
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			//	Should we fire the script playback event?
			if(bScript == true)
				Args.EventId = TmaxVideoCtrlEvents.ScriptTranscriptChanged;
			else
				Args.EventId = TmaxVideoCtrlEvents.PlayerTranscriptChanged;
			
			Args.Position        = m_dPosition;
			Args.XmlDesignation  = m_xmlDesignation;
			Args.TranscriptIndex = iTranscript;
			Args.XmlTranscript   = xmlTranscript;
			
			//	Fire the event
			FireTmaxVideoCtrlEvent(Args);
		
		}// protected void FireTranscriptChanged(CXmlTranscript xmlTranscript, int iTranscript)

		/// <summary>This method fires the event to inform the system when the transcript associated with the player position changes</summary>
		/// <param name="xmlLink">The active link node</param>
		/// <param name="iLink">The index of the active link</param>
		/// <param name="bScript">True to fire script playback event instead of Player event</param>
		protected void FireLinkChanged(CXmlLink xmlLink, int iLink, bool bScript)
		{
			//	Create the event argument
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			if(bScript == true)
				Args.EventId = TmaxVideoCtrlEvents.ScriptLinkChanged;
			else
				Args.EventId = TmaxVideoCtrlEvents.PlayerLinkChanged;
			
			Args.Position		 = m_dPosition;
			Args.XmlDesignation  = m_xmlDesignation;
			Args.LinkIndex       = iLink;
			Args.XmlLink         = xmlLink;
			
			//	Fire the event
			FireTmaxVideoCtrlEvent(Args);
		
		}// protected void FireLinkChanged(CXmlLink xmlLink, int iLink)

		/// <summary>This method fires the designation complete event</summary>
		/// <param name="bScript">True to fire script playback event instead of Player event</param>
		protected void FireDesignationComplete(bool bScript)
		{
			//	Create the event argument
			CTmaxVideoCtrlEventArgs Args = new CTmaxVideoCtrlEventArgs();
			
			if(bScript == true)
				Args.EventId = TmaxVideoCtrlEvents.ScriptDesignationComplete;
			else
				Args.EventId = TmaxVideoCtrlEvents.PlayerDesignationComplete;
			
			Args.Position		 = m_dPosition;
			Args.XmlDesignation  = m_xmlDesignation;
			
			//	Fire the event
			FireTmaxVideoCtrlEvent(Args);
		
		}// protected void FireDesignationComplete(bool bScript)

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected new void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxVideoPlayerCtrl));
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("SpringLabels");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayPause");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Separator");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Back5");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Back1");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StepBack");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Separator");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preview");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Apply");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Separator");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StepFwd");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Fwd1");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Fwd5");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("SpringLabels");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayPause");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Back5");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Back1");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StepBack");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preview");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("StepFwd");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Fwd1");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Fwd5");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("SpringLabels");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Separator");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Apply");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Apply");
			Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
			Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel2 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel3 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel4 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel5 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlPanel = new System.Windows.Forms.Panel();
			this.m_ctrlPosition = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
			this.m_ctrlTranscriptText = new System.Windows.Forms.Label();
			this.m_ctrlViewer = new FTI.Trialmax.Controls.CTmaxViewerCtrl();
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).BeginInit();
			this.m_ctrlPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlToolbarManager
			// 
			this.m_ctrlToolbarManager.DockWithinContainer = this;
			this.m_ctrlToolbarManager.ImageListSmall = this.m_ctrlImages;
			this.m_ctrlToolbarManager.ShowFullMenusDelay = 500;
			this.m_ctrlToolbarManager.ShowQuickCustomizeButton = false;
			this.m_ctrlToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			ultraToolbar1.DockedRow = 0;
			ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.ShowInToolbarList = false;
			ultraToolbar1.Text = "MainToolbar";
			ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							  labelTool1,
																							  buttonTool1,
																							  buttonTool2,
																							  buttonTool3,
																							  buttonTool4,
																							  buttonTool5,
																							  buttonTool6,
																							  buttonTool7,
																							  buttonTool8,
																							  buttonTool9,
																							  buttonTool10,
																							  buttonTool11,
																							  buttonTool12,
																							  labelTool2});
			this.m_ctrlToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
																												 ultraToolbar1});
			this.m_ctrlToolbarManager.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlToolbarManager.ToolbarSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlToolbarManager.ToolbarSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlToolbarManager.ToolbarSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlToolbarManager.ToolbarSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlToolbarManager.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlToolbarManager.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlToolbarManager.ToolbarSettings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			this.m_ctrlToolbarManager.ToolbarSettings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
			appearance1.Image = 0;
			buttonTool13.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool13.SharedProps.Caption = "Play";
			appearance2.Image = 2;
			buttonTool14.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool14.SharedProps.Caption = "Back 5 sec";
			appearance3.Image = 3;
			buttonTool15.SharedProps.AppearancesSmall.Appearance = appearance3;
			buttonTool15.SharedProps.Caption = "Back 1 sec";
			appearance4.Image = 4;
			buttonTool16.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool16.SharedProps.Caption = "Step Back";
			appearance5.Image = 5;
			buttonTool17.SharedProps.AppearancesSmall.Appearance = appearance5;
			buttonTool17.SharedProps.Caption = "Preview";
			appearance6.Image = 6;
			buttonTool18.SharedProps.AppearancesSmall.Appearance = appearance6;
			buttonTool18.SharedProps.Caption = "Step Fwd";
			appearance7.Image = 7;
			buttonTool19.SharedProps.AppearancesSmall.Appearance = appearance7;
			buttonTool19.SharedProps.Caption = "Fwd 1 sec";
			appearance8.Image = 8;
			buttonTool20.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool20.SharedProps.Caption = "Fwd 5 sec";
			labelTool3.SharedProps.MinWidth = 1;
			labelTool3.SharedProps.Spring = true;
			buttonTool21.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			buttonTool21.SharedProps.Enabled = false;
			appearance9.Image = 9;
			buttonTool22.SharedProps.AppearancesSmall.Appearance = appearance9;
			buttonTool22.SharedProps.Caption = "Apply Changes";
			popupMenuTool1.SharedProps.Caption = "ContextMenu";
			popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							   buttonTool23});
			this.m_ctrlToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																										  buttonTool13,
																										  buttonTool14,
																										  buttonTool15,
																										  buttonTool16,
																										  buttonTool17,
																										  buttonTool18,
																										  buttonTool19,
																										  buttonTool20,
																										  labelTool3,
																										  buttonTool21,
																										  buttonTool22,
																										  popupMenuTool1});
			this.m_ctrlToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			this.m_ctrlToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			// 
			// m_ctrlPanel
			// 
			this.m_ctrlPanel.Controls.Add(this.m_ctrlPosition);
			this.m_ctrlPanel.Controls.Add(this.m_ctrlTranscriptText);
			this.m_ctrlPanel.Controls.Add(this.m_ctrlViewer);
			this.m_ctrlPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_ctrlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPanel.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlPanel.Name = "m_ctrlPanel";
			this.m_ctrlPanel.Size = new System.Drawing.Size(320, 181);
			this.m_ctrlPanel.TabIndex = 0;
			// 
			// m_ctrlPosition
			// 
			this.m_ctrlPosition.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlPosition.BorderStylePanel = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlPosition.Dock = System.Windows.Forms.DockStyle.None;
			this.m_ctrlPosition.Location = new System.Drawing.Point(4, 152);
			this.m_ctrlPosition.Name = "m_ctrlPosition";
			ultraStatusPanel1.MinWidth = 75;
			ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
			ultraStatusPanel1.Text = "00:00:00.0";
			ultraStatusPanel1.Width = 75;
			appearance10.BackColor = System.Drawing.SystemColors.Highlight;
			appearance10.BackColorDisabled = System.Drawing.SystemColors.Control;
			appearance10.ForeColor = System.Drawing.SystemColors.HighlightText;
			appearance10.ForeColorDisabled = System.Drawing.SystemColors.GrayText;
			appearance10.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraStatusPanel2.Appearance = appearance10;
			ultraStatusPanel2.Text = "Start";
			ultraStatusPanel2.Width = 75;
			appearance11.BackColor = System.Drawing.SystemColors.Highlight;
			appearance11.BackColorDisabled = System.Drawing.SystemColors.Control;
			appearance11.ForeColor = System.Drawing.SystemColors.HighlightText;
			appearance11.ForeColorDisabled = System.Drawing.SystemColors.GrayText;
			appearance11.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraStatusPanel3.Appearance = appearance11;
			ultraStatusPanel3.Text = "Link";
			ultraStatusPanel3.Width = 75;
			appearance12.BackColor = System.Drawing.SystemColors.Highlight;
			appearance12.BackColorDisabled = System.Drawing.SystemColors.Control;
			appearance12.ForeColor = System.Drawing.SystemColors.HighlightText;
			appearance12.ForeColorDisabled = System.Drawing.SystemColors.GrayText;
			appearance12.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraStatusPanel4.Appearance = appearance12;
			ultraStatusPanel4.Text = "Stop";
			ultraStatusPanel4.Width = 75;
			ultraStatusPanel5.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
			this.m_ctrlPosition.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
																											  ultraStatusPanel1,
																											  ultraStatusPanel2,
																											  ultraStatusPanel3,
																											  ultraStatusPanel4,
																											  ultraStatusPanel5});
			this.m_ctrlPosition.Size = new System.Drawing.Size(312, 23);
			this.m_ctrlPosition.TabIndex = 3;
			this.m_ctrlPosition.WrapText = false;
			// 
			// m_ctrlTranscriptText
			// 
			this.m_ctrlTranscriptText.Location = new System.Drawing.Point(8, 128);
			this.m_ctrlTranscriptText.Name = "m_ctrlTranscriptText";
			this.m_ctrlTranscriptText.Size = new System.Drawing.Size(284, 16);
			this.m_ctrlTranscriptText.TabIndex = 1;
			this.m_ctrlTranscriptText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_ctrlTranscriptText.UseMnemonic = false;
			// 
			// m_ctrlViewer
			// 
			this.m_ctrlViewer.AxAutoSave = false;
			this.m_ctrlViewer.AxIniFilename = "";
			this.m_ctrlViewer.AxIniSection = "";
			this.m_ctrlViewer.BackColor = System.Drawing.Color.Black;
			this.m_ctrlViewer.Location = new System.Drawing.Point(4, 4);
			this.m_ctrlViewer.LockVideoRange = false;
			this.m_ctrlViewer.Name = "m_ctrlViewer";
			this.m_ctrlViewer.PlayOnLoad = true;
			this.m_ctrlViewer.ShowToolbar = false;
			this.m_ctrlViewer.Size = new System.Drawing.Size(312, 112);
			this.m_ctrlViewer.TabIndex = 0;
			this.m_ctrlViewer.UseScreenRatio = false;
			this.m_ctrlViewer.ZapSourceFile = "";
			// 
			// _TmaxVideoCtrl_Toolbars_Dock_Area_Left
			// 
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left.Name = "_TmaxVideoCtrl_Toolbars_Dock_Area_Left";
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 181);
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _TmaxVideoCtrl_Toolbars_Dock_Area_Right
			// 
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(320, 0);
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right.Name = "_TmaxVideoCtrl_Toolbars_Dock_Area_Right";
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 181);
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _TmaxVideoCtrl_Toolbars_Dock_Area_Top
			// 
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top.Name = "_TmaxVideoCtrl_Toolbars_Dock_Area_Top";
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(320, 0);
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _TmaxVideoCtrl_Toolbars_Dock_Area_Bottom
			// 
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 181);
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom.Name = "_TmaxVideoCtrl_Toolbars_Dock_Area_Bottom";
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(320, 27);
			this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// CTmaxVideoPlayerCtrl
			// 
			this.Controls.Add(this.m_ctrlPanel);
			this.Controls.Add(this._TmaxVideoCtrl_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._TmaxVideoCtrl_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._TmaxVideoCtrl_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._TmaxVideoCtrl_Toolbars_Dock_Area_Bottom);
			this.Name = "CTmaxVideoPlayerCtrl";
			this.Size = new System.Drawing.Size(320, 208);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).EndInit();
			this.m_ctrlPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected override void RecalcLayout()
		{
			int iBottom = 0;
			
			if(m_ctrlViewer == null) return;
			if(m_ctrlViewer.IsDisposed == true) return;
			if(m_ctrlPanel == null) return;
			if(m_ctrlPanel.IsDisposed == true) return;

			iBottom = m_ctrlPanel.Height;
			
			if((m_bShowPosition == true) && 
				(m_ctrlPosition != null) && 
				(m_ctrlPosition.IsDisposed != true))
			{
				m_ctrlPosition.Location = new System.Drawing.Point(0, iBottom - m_ctrlPosition.Height);
				m_ctrlPosition.Size = new System.Drawing.Size(m_ctrlPanel.Width, m_ctrlPosition.Height);		
				iBottom = m_ctrlPosition.Top;
			}
			
			if((m_bShowTranscript == true) && 
			   (m_ctrlTranscriptText != null) && 
			   (m_ctrlTranscriptText.IsDisposed != true))
			{
				m_ctrlTranscriptText.Location = new System.Drawing.Point(0, iBottom - m_ctrlTranscriptText.Height);
				m_ctrlTranscriptText.Size = new System.Drawing.Size(m_ctrlPanel.Width, m_ctrlTranscriptText.Height);		
				iBottom = m_ctrlTranscriptText.Top;
			}
			
			m_ctrlViewer.Location = new System.Drawing.Point(0, 0);
			m_ctrlViewer.Size = new System.Drawing.Size(m_ctrlPanel.Width, iBottom);		
			
		}// private void RecalcLayout()
		
		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		protected void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			TmaxVideoPlayerCommands eCommand = TmaxVideoPlayerCommands.Invalid;
			
			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);
				
			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case TmaxVideoPlayerCommands.PlayPause:
					
						OnCmdPlayPause();
						break;
						
					case TmaxVideoPlayerCommands.Preview:
					
						OnCmdPreview();
						break;
						
					case TmaxVideoPlayerCommands.Back5:
					case TmaxVideoPlayerCommands.Back1:
					case TmaxVideoPlayerCommands.StepBack:
					case TmaxVideoPlayerCommands.StepFwd:
					case TmaxVideoPlayerCommands.Fwd1:
					case TmaxVideoPlayerCommands.Fwd5:
					
						OnCmdCue(eCommand);
						break;
						
					case TmaxVideoPlayerCommands.Apply:
					
						OnCmdApply();
						break;
						
					default:
					
						break;
				
				}// switch(eCommand)

			}
			catch
			{
				//MessageBox.Show(Ex.ToString());
				//Debug.Assert(false);
			}
		
		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>This function is notify the control that the parent window has been moved</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		public override void OnParentMoved()
		{
			//	Notify the viewer that the parent window has been moved
			if((m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false))
				m_ctrlViewer.OnParentMoved();
		}
		
		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			if(m_ctrlTranscriptText != null)
				m_ctrlTranscriptText.Visible = m_bShowTranscript;
				
			if(m_ctrlPosition != null)
				m_ctrlPosition.Visible = m_bShowPosition;
				
			//	Assign the context menu if we are using the Apply command
			if(m_bAllowApply == true)
			{
				this.m_ctrlToolbarManager.SetContextMenuUltra(this, "ContextMenu");
			}
			
			//	Do the base class processing
			base.OnLoad(e);
		
		}// protected override void OnLoad(System.EventArgs e)

		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		protected void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		#endregion Protected Methods

		#region Properties
		
		/// <summary>Current tune mode as reflected by the button state</summary>
		public TmaxVideoCtrlTuneModes TuneMode
		{
			get	{ return m_eTuneMode; }
			set { SetTuneMode(value); }
			
		}// TuneMode property

		/// <summary>Current playback position</summary>
		public double PlayerPosition
		{
			get { return m_dPosition; }
			set { m_dPosition = value;}
		
		}// PlayerPosition
		
		/// <summary>Make the apply command visible</summary>
		public bool AllowApply
		{
			get { return m_bAllowApply; }
			set { m_bAllowApply = value;}
		
		}// AllowApply
		
		/// <summary>Time for previewing video from current position</summary>
		public double PreviewPeriod
		{
			get { return m_dPreviewPeriod; }
			set { m_dPreviewPeriod = value;
			      SetCommandStates();
			    }
		
		}// PreviewPeriod
		
		/// <summary>Flag to indicate if position bar should be visible</summary>
		public bool ShowPosition
		{
			get 
			{ 
				return m_bShowPosition; 
			}
			set 
			{ 
				m_bShowPosition = value;
				
				if((m_ctrlPosition != null) && (m_ctrlPosition.IsDisposed == false))
				{
					m_ctrlPosition.Visible = m_bShowPosition;
					RecalcLayout();
				}
			}
		
		}// ShowPosition
		
		/// <summary>Flag to indicate if transcript bar should be visible</summary>
		public bool ShowTranscript
		{
			get 
			{ 
				return m_bShowTranscript; 
			}
			set 
			{ 
				m_bShowTranscript = value;
				
				if((m_ctrlTranscriptText != null) && (m_ctrlTranscriptText.IsDisposed == false))
				{
					m_ctrlTranscriptText.Visible = m_bShowTranscript;
					RecalcLayout();
				}
			}
		
		}// ShowTranscript
		
		// True to automatically start playback when loading a file
		public bool PlayOnLoad
		{
			get
			{ 
				if(m_ctrlViewer != null)
					return m_ctrlViewer.PlayOnLoad;
				else
					return false;
			}
			set
			{ 
				if(m_ctrlViewer != null)
					m_ctrlViewer.PlayOnLoad = value;
			}
		
		}// PlayOnLoad
		
		/// <summary>Enables simulated playback if unable to locate the video file</summary>
		public bool EnableSimulation
		{
			get
			{ 
				if(m_ctrlViewer != null)
					return m_ctrlViewer.EnablePlayerSimulation;
				else
					return false;
			}
			set
			{ 
				if(m_ctrlViewer != null)
					m_ctrlViewer.EnablePlayerSimulation = value;
			}
		
		}// EnableSimulation
		
		/// <summary>Text displayed during simulated playback</summary>
		public string SimulationText
		{
			get
			{ 
				if(m_ctrlViewer != null)
					return m_ctrlViewer.SimulationText;
				else
					return "";
			}
			set
			{ 
				if(m_ctrlViewer != null)
					m_ctrlViewer.SimulationText = value;
			}
		
		}// SimulationText
		
		#endregion Properties
		
	}// public class CTmaxVideoCtrl : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.Controls

