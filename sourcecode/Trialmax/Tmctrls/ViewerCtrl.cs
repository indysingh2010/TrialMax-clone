using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.ActiveX;

namespace FTI.Trialmax.Controls
{
	/// <summary>
	///	This class creates a control that manages all available Trialmax viewers 
	/// </summary>
	public class CTmaxViewerCtrl : System.Windows.Forms.UserControl
	{
		/// <summary>ActiveX viewer events (see TmxEvents enumeration)</summary>
		public event FTI.Trialmax.ActiveX.TmxEventHandler TmxEvent;
        public event FTI.Trialmax.ActiveX.TmxEventHandler OnRequestPresentation;
		
		#region Constants
		
		private const int ERROR_NO_VIEWER			= 0;
		private const int ERROR_AXINITIALIZE_FAILED	= 1;
		private const int ERROR_AXINITIALIZE_EX		= 2;
		private const int ERROR_VIEW_LOAD_FAILED	= 3;
		private const int ERROR_VIEW_EX				= 4;
		private const int ERROR_CREATE_VIEWER_EX	= 5;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to ShowToolbar property</summary>
		protected bool m_bShowToolbar = false;

		/// <summary>Local member bound to UseScreenRatio property</summary>
		protected bool m_bUseScreenRatio = false;

		/// <summary>Local member bound to LockVideoRange property</summary>
		protected bool m_bLockVideoRange = false;

		/// <summary>Local member bound to ZapSourceFile property</summary>
		protected string m_strZapSourceFile = "";

		/// <summary>Local member accessed by the AxIniFilename property</summary>
		protected string m_strAxIniFilename = "";
		
		/// <summary>Local member accessed by the AxIniSection property</summary>
		protected string m_strAxIniSection = "";
		
		/// <summary>Local member bound to AxAutoSave</summary>
		private bool m_bAxAutoSave = false;
		
		/// <summary>Dynamic array of available viewers</summary>
		private CTmxBase[] m_aViewers = new CTmxBase[(int)TmaxMediaViewers.MaxViewers];
		
		/// <summary>The active viewer</summary>
		private CTmxBase m_tmaxViewer = null;
		
		/// <summary>Local member bound to PlayOnLoad property</summary>
		private bool m_bPlayOnLoad = false;
		
		/// <summary>Local member bound to EnablePlayerSimulation property</summary>
		private bool m_bEnablePlayerSimulation = false;
		
		/// <summary>Local member bound to EnableToolbar property</summary>
		private bool m_bEnableToolbar = true;
		
		/// <summary>Local member bound to SimulationText property</summary>
		private string m_strSimulationText = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxViewerCtrl()
		{
			//	Populate the error builder
			SetErrorStrings();

			//	Initialize the child windows
			InitializeComponent();
			
			m_tmaxEventSource.Name = this.Name;
		}
		
		/// <summary>This method is called to initialize the control</summary>
		/// <param name="bCreateViewers">true to create the media viewers</param>
		///	<returns>true if successful</returns>
		/// <remarks>If not created at initialization, the viewers will be created on demand</remarks>
		public bool Initialize(bool bCreateViewers)
		{
			bool bSuccessful = true;
			
			//	Do we need to create the viewers?
			if(bCreateViewers == true)
			{
				if(CreateViewer(TmaxMediaViewers.Tmview) == null)
					bSuccessful = false;
					
				if(CreateViewer(TmaxMediaViewers.Tmmovie) == null)
					bSuccessful = false;
					
				if(CreateViewer(TmaxMediaViewers.Tmpower) == null)
					bSuccessful = false;
			}
				
			return bSuccessful;
			
		}// public bool CreateViewers()

		/// <summary>This method is called to initialize the control</summary>
		/// <param name="strFilename">The filename to be viewed</param>
		///	<returns>true if successful</returns>
		public bool Initialize(string strFilename)
		{
			return (GetViewer(strFilename) != null);
			
		}// public bool Initialize(string strFilename)

		/// <summary>This method is called to shut down the viewer</summary>
		public void Terminate()
		{
			//	Unload the active viewer
			Unload();
			
			//	Shut down the ActiveX interfaces
			AxTerminate();
			
		}// public void Terminate()
		
		/// <summary>This method is called to determine if the specified file can be viewed</summary>
		/// <param name="strFilename">The desired file</param>
		/// <returns>true if the file can be viewed</returns>
		public bool IsViewable(string strFilename)
		{
			return (GetViewer(strFilename) != null);
		}
		
		/// <summary>This method is called to determine if there is a file loaded in the viewer</summary>
		/// <returns>true if loaded</returns>
		public bool IsLoaded()
		{
			if(m_tmaxViewer != null)
				return (m_tmaxViewer.Filename.Length > 0);
			else
				return false;
		}
		
		/// <summary>This method is called to process an active viewer request</summary>
		/// <param name="tmxRequest">The request arguments</param>
		/// <returns>true if processed successfully</returns>
		public bool ProcessRequest(CTmxRequest tmxRequest)
		{
			if((m_tmaxViewer != null) && (m_tmaxViewer.IsDisposed == false))
				return m_tmaxViewer.ProcessRequest(tmxRequest);
			else
				return false;
				
		}// public bool ProcessRequest(CTmxRequest tmxRequest)
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="lPage">The page at which to start viewing (different meaning for different viewers)</param>
		/// <param name="bShowToolbar">True to show the viewer toolbar</param>
		/// <returns>true if successful</returns>
		public bool View(string strFilename, long lPage, bool bShowToolbar)
		{
			CTmxBase tmaxViewer = null;

			//	Should we unload?
			if((strFilename == null) || (strFilename.Length == 0))
			{
				Unload();
				return true;
			}

			//	Locate the appropriate viewer
			if((tmaxViewer = GetViewer(strFilename)) != null)
			{
				//	Make sure the viewer is initialized
				if(tmaxViewer.Initialized == false)
				{
					if(AxInitialize(tmaxViewer) == false)
						return false;
				}
						
				try
				{
					//	Set the toolbar state
					if(tmaxViewer.ShowToolbar != bShowToolbar)
						tmaxViewer.ShowToolbar = bShowToolbar;
						
					//	Make this the active viewer
					//
					//	NOTE:	This must be done before loading the viewer
					//			to make sure its visible before loading the
					//			file
					SetActiveViewer(tmaxViewer);
					
					return tmaxViewer.View(strFilename, lPage);

				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "View", m_tmaxErrorBuilder.Message(ERROR_VIEW_EX, strFilename), Ex);
				}
			
			}
			else
			{
				m_tmaxEventSource.FireError(this, "View", m_tmaxErrorBuilder.Message(ERROR_NO_VIEWER, strFilename));

			}// if((tmaxViewer = GetViewer(strFilename)) != null)
			
			//	Must have been an error
			return false;
				
		}// View(string strFilename)
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="lPage">The value at which to start viewing (different meaning for different viewers)</param>
		/// <returns>true if successful</returns>
		public bool View(string strFilename, long lPage)
		{
			return View(strFilename, lPage, m_bShowToolbar);
		}	
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="bShowToolbar">True to show the viewer toolbar</param>
		/// <returns>true if successful</returns>
		public bool View(string strFilename, bool bShowToolbar)
		{
			return View(strFilename, 0, bShowToolbar);
		}	
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <returns>true if successful</returns>
		public bool View(string strFilename)
		{
			return View(strFilename, 0, m_bShowToolbar);
		}	
		
		//	This method is called to unload the active viewer
		public void Unload()
		{
			if(m_tmaxViewer != null)
			{
				m_tmaxViewer.Unload();
				SetActiveViewer(null);
			}
			
		}//	Unload()
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="dStart">The value at which to start the playback</param>
		/// <param name="dStop">The value at which to stop the playback</param>
		/// <param name="bShowToolbar">True to show the viewer toolbar</param>
		/// <returns>true if successful</returns>
		public bool Play(string strFilename, double dStart, double dStop, bool bShowToolbar)
		{
			CTmxBase tmaxViewer = null;
			
			//	Should we unload?
			if((strFilename == null) || (strFilename.Length == 0))
			{
				Unload();
				return true;
			}

			//	Locate the appropriate viewer
			if((tmaxViewer = GetViewer(strFilename)) != null)
			{
				//	Make sure the viewer is initialized
				if(tmaxViewer.Initialized == false)
				{
					if(AxInitialize(tmaxViewer) == false)
						return false;
				}
						
				try
				{
					//	Set the toolbar state
					if(tmaxViewer.ShowToolbar != bShowToolbar)
						tmaxViewer.ShowToolbar = bShowToolbar;
						
					if(tmaxViewer.Play(strFilename, dStart, dStop, m_bPlayOnLoad) == true)
					{
						SetActiveViewer(tmaxViewer);
						
						//	Make sure we are on the start position if not playing
						if(m_bPlayOnLoad == false)
						{
							Cue(TmxCueModes.Start);
						}
							
						return true;
					}
					else
					{
						m_tmaxEventSource.FireError(this, "Play", m_tmaxErrorBuilder.Message(ERROR_VIEW_LOAD_FAILED, tmaxViewer.Description, strFilename, tmaxViewer.AxError));
					}
					
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "Play", m_tmaxErrorBuilder.Message(ERROR_VIEW_EX, strFilename), Ex);
				}
			
			}
			else
			{
				m_tmaxEventSource.FireError(this, "Play", m_tmaxErrorBuilder.Message(ERROR_NO_VIEWER, strFilename));
				
			}// if((tmaxPlayer = GetPlayer(strFilename)) != null)
			
			//	Must have been an error
			return false;
				
		}// Play(string strFilename)
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="dStart">The value at which to start the playback</param>
		/// <param name="dStop">The value at which to stop the playback</param>
		/// <returns>true if successful</returns>
		public bool Play(string strFilename, double dStart, double dStop)
		{
			return Play(strFilename, dStart, dStop, m_bShowToolbar);
		}
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="bShowToolbar">True to show the viewer toolbar</param>
		/// <returns>true if successful</returns>
		public bool Play(string strFilename, bool bShowToolbar)
		{
			return Play(strFilename, 0, 0, bShowToolbar);
		}
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <returns>true if successful</returns>
		public bool Play(string strFilename)
		{
			return Play(strFilename, 0, 0, m_bShowToolbar);
		}
		
		/// <summary>This function is called play the file loaded in the viewer</summary>
		/// <param name="dStart">The value at which to start viewing (different meaning for different viewers)</param>
		/// <param name="dStop">The value at which to stop viewing (different meaning for different viewers)</param>
		/// <returns>true if successful</returns>
		public bool Play(double dStart, double dStop)
		{
			if(m_tmaxViewer != null)
				return m_tmaxViewer.Play(dStart, dStop);
			else
				return false;

		}// public bool Play(double dStart, double dStop)
		
		/// <summary>This function is called play the file loaded in the viewer</summary>
		/// <returns>true if successful</returns>
		public bool Play()
		{
			if(m_tmaxViewer != null)
				return m_tmaxViewer.Play();
			else
				return false;

		}// public bool Play()
		
		/// <summary>This function is called to step the active file from the start position to the stop position</summary>
		/// <param name="dStart">The value at which to start from</param>
		/// <param name="dStop">The value at which to step to</param>
		/// <returns>true if successful</returns>
		public bool Step(double dStart, double dStop)
		{
			if(m_tmaxViewer != null)
				return m_tmaxViewer.Step(dStart, dStop);
			else
				return false;

		}// public bool Step(double dStart, double dStop)
		
		/// <summary>This function is called to step the active file from the current position the specified amount of time</summary>
		/// <param name="dTime">The time to step</param>
		/// <returns>true if successful</returns>
		public bool Step(double dTime)
		{
			if(m_tmaxViewer != null)
				return m_tmaxViewer.Step(dTime);
			else
				return false;

		}// public bool Step(double dTime)
		
		/// <summary>This function is called stop the current playback</summary>
		/// <returns>true if successful</returns>
		public bool Stop()
		{
			if((m_tmaxViewer != null) && (m_tmaxViewer.IsDisposed == false))
				return m_tmaxViewer.Stop();
			else
				return false;
		
		}// public bool Stop()
		
		/// <summary>This function is called stop the current playback</summary>
		/// <returns>true if successful</returns>
		public bool Pause()
		{
			CTmxRequest tmxRequest = null;
			
			try
			{
				tmxRequest = new CTmxRequest(TmxActions.Pause);
				return ProcessRequest(tmxRequest);
			}
			catch
			{
			}
			return false;
		
		}// public bool Pause()
		
		/// <summary>This function is called resume the current playback</summary>
		/// <returns>true if successful</returns>
		public bool Resume()
		{
			if((m_tmaxViewer != null) && (m_tmaxViewer.IsDisposed == false))
				return m_tmaxViewer.Resume();
			else
				return false;
		
		}// public bool Pause()
		
		/// <summary>This function is called cue the playback</summary>
		/// <param name="eMode">The enumerated cue mode</param>
		/// <param name="dPosition">The new position</param>
		/// <param name="bResume">true to resume playback</param>
		/// <returns>true if successful</returns>
		public bool Cue(TmxCueModes eMode, double dPosition, bool bResume)
		{
			if((m_tmaxViewer != null) && (m_tmaxViewer.IsDisposed == false))
				return m_tmaxViewer.Cue(eMode, dPosition, bResume);
			else
				return false;
		
		}// public bool Cue(TmxCueModes eMode, double dPosition, bool bResume)

        /// <summary>
        /// Get the duration of the provided file name
        /// </summary>
        /// <param name="strFileName">Absolute path of the video file</param>
        /// <returns>Duration of the video file</returns>
        public double GetDuration(string strFileName)
        {
            return ((CTmxMovie)m_aViewers[(int)TmaxMediaViewers.Tmmovie]).GetDuration(strFileName);
        }// public double GetDuration(string strFileName)

		/// <summary>This function is called to cue the active file</summary>
		/// <param name="eMode">The enumerated cue mode</param>
		/// <param name="dPosition">The new position</param>
		/// <returns>true if successful</returns>
		public virtual bool Cue(TmxCueModes eMode, double dPosition)
		{
			return Cue(eMode, dPosition, false);
		}
		
		/// <summary>This function is called to cue the active file to the specified position</summary>
		/// <param name="dPosition">The new position</param>
		/// <returns>true if successful</returns>
		public virtual bool Cue(double dPosition)
		{
			return Cue(TmxCueModes.Absolute, dPosition, false);
		}
		
		/// <summary>This function is called to cue the active file</summary>
		/// <param name="eMode">The enumerated cue mode</param>
		/// <returns>true if successful</returns>
		public virtual bool Cue(TmxCueModes eMode)
		{
			return Cue(eMode, 0, false);
		}
		
		/// <summary>This method handles all KeyDown notifications from the application</summary>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <returns>true if processed</returns>
		public virtual bool OnKeyDown(Keys eKey, Keys eModifiers)
		{
			if((m_tmaxViewer != null) && (m_tmaxViewer.IsDisposed == false))
				return m_tmaxViewer.OnKeyDown(eKey, eModifiers);
			else
				return false;
		}
		
		/// <summary>This function is called to set the index and total used for navigator commands</summary>
		/// <param name="iPosition">The current position</param>
		/// <param name="iTotal">The total number of navigator positions</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool SetNavigatorPosition(int iPosition, int iTotal)
		{
			if((m_tmaxViewer != null) && (m_tmaxViewer.IsDisposed == false))
				return m_tmaxViewer.SetNavigatorPosition(iPosition, iTotal);
			else
				return false;
		}
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>Performs initialization of child components</summary>
		private void InitializeComponent()
		{
			// 
			// CTmaxViewerCtrl
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Name = "CTmaxViewerCtrl";
			this.Size = new System.Drawing.Size(212, 148);

		}
		
		/// <summary>This method is called to initialize the specified viewer's ActiveX interface</summary>
		/// <returns>true if successfully initialized</returns>
		private bool AxInitialize(CTmxBase tmxViewer)
		{
			bool bSuccessful = true;
			
			try
			{
				if(tmxViewer.AxInitialize() == false)
				{
					m_tmaxEventSource.FireError(this, "AxInitialize", m_tmaxErrorBuilder.Message(ERROR_AXINITIALIZE_FAILED, tmxViewer.Description));
					bSuccessful = false;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AxInitialize", m_tmaxErrorBuilder.Message(ERROR_AXINITIALIZE_EX, tmxViewer.Description), Ex);
				bSuccessful = false;
			}
				
			return bSuccessful;
						
		}// public bool AxInitialize()
		
		/// <summary>This method is called to shut down each viewer's ActiveX interface</summary>
		private void AxTerminate()
		{
			//	Initialize each of the ActiveX controls
			for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
			{
				if(m_aViewers[i] != null)
				{
					try
					{
						m_aViewers[i].AxTerminate();
					}
					catch
					{
					}
				
				}// if(m_aViewers[i] != null)
				
			}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
						
		}// private void AxTerminate()
		
		/// <summary>This method is called to set the properties of all the child ActiveX controls</summary>
		public void AxSetProperties()
		{
			//	Notify each of the ActiveX controls
			for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
			{
				if(m_aViewers[i] != null)
				{
					try
					{
						m_aViewers[i].AxSetProperties();
					}
					catch
					{
					}
				
				}// if(m_aViewers[i] != null)
				
			}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
						
		}// private void AxSetProperties()
		
		/// <summary>This method is called to save the properties of all the child ActiveX controls</summary>
		public void AxSaveProperties()
		{
			//	Notify each of the ActiveX controls
			for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
			{
				if(m_aViewers[i] != null)
				{
					try
					{
						m_aViewers[i].AxSaveProperties();
					}
					catch
					{
					}
				
				}// if(m_aViewers[i] != null)
				
			}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
						
		}// private void AxSaveProperties()
		
		/// <summary>This method is called to create the specified viewer</summary>
		/// <param name="eViewer">The enumerated viewer identifier</param>
		/// <returns>The newly created viewer</returns>
		private CTmxBase CreateViewer(TmaxMediaViewers eViewer)
		{
			CTmxBase tmaxViewer = null;
			
			//	Does this viewer already exist?
			if(m_aViewers[(int)eViewer] != null)
				return m_aViewers[(int)eViewer];
				
			try
			{
				//	Which viewer are we supposed to create?
				switch(eViewer)
				{
					case TmaxMediaViewers.Tmview:
					
						tmaxViewer = (CTmxBase)(new CTmxView());
						
						//	Set custom TmxView properties
						((CTmxView)tmaxViewer).UseScreenRatio = m_bUseScreenRatio;
						((CTmxView)tmaxViewer).ZapSourceFile = m_strZapSourceFile;
						
						break;
						
					case TmaxMediaViewers.Tmmovie:
					
						tmaxViewer = (CTmxBase)(new CTmxMovie());
						
						//	Set custom TmxMovie properties
						((CTmxMovie)tmaxViewer).LockRange = m_bLockVideoRange;
						((CTmxMovie)tmaxViewer).EnableSimulation = m_bEnablePlayerSimulation;
						((CTmxMovie)tmaxViewer).SimulationText = m_strSimulationText;
						
						break;
						
					case TmaxMediaViewers.Tmpower:
					
						tmaxViewer = (CTmxBase)(new CTmxPower());
						break;
						
				}
				
				tmaxViewer.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(m_tmaxEventSource.OnDiagnostic);
				tmaxViewer.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(m_tmaxEventSource.OnError);

				tmaxViewer.Visible = false;
				tmaxViewer.IniFilename = AxIniFilename;
				tmaxViewer.IniSection = AxIniSection;
				tmaxViewer.AxAutoSave = AxAutoSave;
				tmaxViewer.AxError = ((short)(0));
				tmaxViewer.Bounds = new System.Drawing.Rectangle(0, 0, this.Width, this.Height);
				tmaxViewer.Name = eViewer.ToString();
				tmaxViewer.ShowToolbar = m_bShowToolbar;
				tmaxViewer.EnableToolbar = m_bEnableToolbar;
				
				this.Controls.Add(tmaxViewer);
				
				tmaxViewer.EventSource.MouseDblClickEvent += new System.Windows.Forms.MouseEventHandler(m_tmaxEventSource.OnMouseDblClick);
				tmaxViewer.EventSource.MouseDownEvent += new System.Windows.Forms.MouseEventHandler(m_tmaxEventSource.OnMouseDown);
				tmaxViewer.EventSource.MouseUpEvent += new System.Windows.Forms.MouseEventHandler(m_tmaxEventSource.OnMouseUp);
				tmaxViewer.EventSource.MouseMoveEvent += new System.Windows.Forms.MouseEventHandler(m_tmaxEventSource.OnMouseMove);
				tmaxViewer.TmxEvent += new FTI.Trialmax.ActiveX.TmxEventHandler(this.OnTmxEvent);

                tmaxViewer.OnRequestPresentation += new TmxEventHandler(tmaxViewer_OnRequestPresentation);

				//	Do the ActiveX initialization
				tmaxViewer.AxInitialize();
				
				m_aViewers[(int)eViewer] = tmaxViewer;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateViewer", m_tmaxErrorBuilder.Message(ERROR_CREATE_VIEWER_EX, eViewer), Ex);
				tmaxViewer = null;
			}
			
			return tmaxViewer;
			
		}// CreateViewer(TmaxMediaViewers eViewer)

        void tmaxViewer_OnRequestPresentation(object objSender, CTmxEventArgs Args)
        {
            if (OnRequestPresentation != null)
                OnRequestPresentation(objSender, Args);
        }
		
		/// <summary>This method is called to locate the appropriate viewer for the specified file</summary>
		/// <param name="strFilename">The file to be viewed</param>
		/// <returns>The viewer associated with the specified file</returns>
		private CTmxBase GetViewer(string strFilename)
		{
			string strExtension = System.IO.Path.GetExtension(strFilename);

			//	Locate the appropriate viewer
			if(CTmxView.CheckExtension(strExtension) == true)
				return CreateViewer(TmaxMediaViewers.Tmview);
			else if(CTmxMovie.CheckExtension(strExtension) == true)
				return CreateViewer(TmaxMediaViewers.Tmmovie);
			else if(CTmxPower.CheckExtension(strExtension) == true)
				return CreateViewer(TmaxMediaViewers.Tmpower);
			else
				return null;
				
		}// GetViewer(string strFilename)
		
		/// <summary>This method is called to set the size and position of the viewer</summary>
		/// <param name="eViewer">The viewer to be positioned</param>
		private void SetPosition(CTmxBase tmxViewer)
		{
			Debug.Assert(tmxViewer != null);
			if(tmxViewer == null) return;
			
			if((this.Height <= 0) || (this.Width <= 0)) return;
			
			//	Move the viewer window
			tmxViewer.SetBounds(0, 0, this.Width, this.Height, BoundsSpecified.All);

			//	Automatically rescale callouts if this is the Tmview control
			if(m_aViewers[(int)TmaxMediaViewers.Tmview] != null)
			{
				if(ReferenceEquals(tmxViewer, m_aViewers[(int)TmaxMediaViewers.Tmview]) == true)
				{
					if(((CTmxView)tmxViewer).CalloutCount > 0)
					{
						try
						{
							((CTmxView)tmxViewer).RescaleCallouts();
						}
						catch
						{
						}
						
					}
				}
			
			}
			else if(m_aViewers[(int)TmaxMediaViewers.Tmmovie] != null)
			{
				if(ReferenceEquals(tmxViewer, m_aViewers[(int)TmaxMediaViewers.Tmmovie]) == true)
				{
					((CTmxMovie)tmxViewer).OnParentMoved();			
				}
			
			}
				

		}// private void SetPosition(CTmxBase tmxViewer)
		
		/// <summary>This method is called to activate the specified viewer</summary>
		/// <param name="objViewer">The viewer object to be activated</param>
		private void SetActiveViewer(CTmxBase tmaxViewer)
		{
			//	Has the viewer changed?
			if(m_tmaxViewer != null)
			{
				if((tmaxViewer != null) && (ReferenceEquals(tmaxViewer, m_tmaxViewer) == true))
				{
					return;
				}
			}
			else
			{
				if(tmaxViewer == null) return;
			}
			
			//	Make sure the new viewer is visible
			if(tmaxViewer != null)
			{
				SetPosition(tmaxViewer);
				tmaxViewer.Visible = true;
				tmaxViewer.BringToFront();
				tmaxViewer.Activate();
			}
			
			//	Deactivate the current viewer
			if(m_tmaxViewer != null)
			{
				m_tmaxViewer.Deactivate();
				m_tmaxViewer.Visible = false;
			}
			
			m_tmaxViewer = tmaxViewer;
			
		}// SetActiveViewer(CTmxBase tmaxViewer)
		
		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		private void RecalcLayout()
		{
			if(m_aViewers != null)
			{
				//	Check each of the viewers
				for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
				{
					if(m_aViewers[i] != null)
					{
						SetPosition(m_aViewers[i]);
							
					}// if(m_aViewers[i] != null)
					
				}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
			}
			
		}// private void RecalcLayout()
		
		#endregion Private Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected void SetErrorStrings()
		{
			if((m_tmaxErrorBuilder != null) && (m_tmaxErrorBuilder.FormatStrings != null))
			{
				m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate an appropriate viewer for the specified file: %1");
				m_tmaxErrorBuilder.FormatStrings.Add("Unable to initialize the ActiveX interface for %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the ActiveX interface for %1");
				m_tmaxErrorBuilder.FormatStrings.Add("%1 was unable to load %2. ActiveX error # = %3");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while loading %1 into the media viewer.");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the %1 viewer.");
			}

		}// protected void SetErrorStrings()

		/// <summary>This function handles all Resize events</BR></summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnResize(System.EventArgs e)
		{
			//	Perform the base class processing
			base.OnResize(e);
			
			//	Make sure the viewer's are properly sized
			RecalcLayout();
		}

		/// <summary>This function is notify the control that the parent window has been moved</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		public virtual void OnParentMoved()
		{
			//	Notify each of the viewers
			for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
			{
				if(m_aViewers[i] != null)
				{
					m_aViewers[i].OnParentMoved();
							
				}// if(m_aViewers[i] != null)
					
			}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
		}
		
		/// <summary>This method handles all TmxEvents fired by the ActiveX viewers</summary>
		/// <param name="O">The object sending the event</param>
		/// <param name="Args">The event arguments</param>
		protected virtual void OnTmxEvent(object O, CTmxEventArgs Args)
		{
			//	Propagate the event
			if((TmxEvent != null) && (Args != null))
			{
				TmxEvent(O, Args);
			}
			
		}// protected virtual void OnTmxEvent(object O, CTmxEventArgs Args)
		
		/// <summary>Clean up all resources being used</BR></summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing == true)
			{
				//	Release all references to the viewers
				for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
				{
					m_aViewers[i] = null;
				}
			
			}
			base.Dispose(disposing);
		}
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
			
		}// EventSource property

		/// <summary>INI file containing the ActiveX configuration information</summary>
		public string AxIniFilename
		{
			get
			{
				return m_strAxIniFilename;
			}
			set
			{
				m_strAxIniFilename = value;
				
				//	Notify each viewer
				for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
				{
					if(m_aViewers[i] != null)
					{
						try
						{
							m_aViewers[i].IniFilename = m_strAxIniFilename;
						}
						catch
						{
						}
				
					}// if(m_aViewers[i] != null)
				
				}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
						
			}
		}
		
		/// <summary>INI section containing the ActiveX configuration information</summary>
		public string AxIniSection
		{
			get
			{
				return m_strAxIniSection;
			}
			set
			{
				m_strAxIniSection = value;
				
				//	Notify each viewer
				for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
				{
					if(m_aViewers[i] != null)
					{
						try
						{
							m_aViewers[i].IniSection = m_strAxIniSection;
						}
						catch
						{
						}
				
					}// if(m_aViewers[i] != null)
				
				}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
						
			}
		}
		
		/// <summary>This property controls whether or not the ActiveX control automatically updates its configuration file when necessary</summary>
		public bool AxAutoSave
		{
			get
			{
				return m_bAxAutoSave;
			}
			set
			{
				m_bAxAutoSave = value;
				
				//	Notify each viewer
				for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
				{
					if(m_aViewers[i] != null)
					{
						try
						{
							m_aViewers[i].AxAutoSave = m_bAxAutoSave;
						}
						catch
						{
						}
				
					}// if(m_aViewers[i] != null)
				
				}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
						
			}
		
		}
		
		/// </summary>This property is used to specify the file to be loaded in the viewer</summary>
		public string Filename
		{
			get
			{
				if(m_tmaxViewer != null)
				{
					return System.IO.Path.GetFileName(m_tmaxViewer.Filename);
				}
				else
				{
					return "";
				}
			}
		
		}// Filename
		
		/// </summary>Exposes the State of the active viewer</summary>
		public TmxStates State
		{
			get
			{
				if(m_tmaxViewer != null)
				{
					return m_tmaxViewer.TmxState;
				}
				else
				{
					return TmxStates.Invalid;
				}
			}
		
		}// State
		
		/// </summary>Exposes the playback position of the active viewer</summary>
		public double Position
		{
			get
			{
				if(m_tmaxViewer != null)
				{
					return m_tmaxViewer.TmxPosition;
				}
				else
				{
					return (double)-1;
				}
			}
		
		}// Position
		
		/// </summary>Exposes the minimum playback position of the active viewer</summary>
		public double MinimumPosition
		{
			get
			{
				if(m_tmaxViewer != null)
				{
					return m_tmaxViewer.TmxMinPosition;
				}
				else
				{
					return (double)-1;
				}
			}
		
		}// Position
		
		/// </summary>Exposes the minimum playback position of the active viewer</summary>
		public double MaximumPosition
		{
			get
			{
				if(m_tmaxViewer != null)
				{
					return m_tmaxViewer.TmxMaxPosition;
				}
				else
				{
					return (double)-1;
				}
			}
		
		}// Position
		
		/// </summary>True to automatically start playback when loading a file</summary>
		public bool PlayOnLoad
		{
			get{ return m_bPlayOnLoad; }
			set{ m_bPlayOnLoad = value; }
		
		}// PlayOnLoad
		
		/// <summary>True to force TMView to used default 4/3 screen ratio</summary>
		public bool UseScreenRatio
		{
			get
			{
				return m_bUseScreenRatio;
			}
			set
			{
				m_bUseScreenRatio = value;
				
				if(m_aViewers[(int)TmaxMediaViewers.Tmview] != null)
					((CTmxView)m_aViewers[(int)TmaxMediaViewers.Tmview]).UseScreenRatio = m_bUseScreenRatio;
			}
		
		}// UseScreenRatio
		
		/// <summary>File to use as source file when loading a treatment</summary>
		public string ZapSourceFile
		{
			get
			{
				return m_strZapSourceFile;
			}
			set
			{
				m_strZapSourceFile = value;
				
				if(m_aViewers[(int)TmaxMediaViewers.Tmview] != null)
					((CTmxView)m_aViewers[(int)TmaxMediaViewers.Tmview]).ZapSourceFile = m_strZapSourceFile;
			}
		
		}// ZapSourceFile
		
		/// <summary>This property is controls the visibility of the viewer's toolbar</summary>
		public bool ShowToolbar
		{
			get
			{
				return m_bShowToolbar;
			}
			set
			{
				m_bShowToolbar = value;
				
				//	Notify each viewer
				for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
				{
					if(m_aViewers[i] != null)
					{
						try
						{
							m_aViewers[i].ShowToolbar = m_bShowToolbar;
						}
						catch
						{
						}
				
					}// if(m_aViewers[i] != null)
				
				}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
						
			}
		
		}// ShowToolbar
		
		/// <summary>This property is enables/disables the viewer's toolbar</summary>
		public bool EnableToolbar
		{
			get
			{
				return m_bEnableToolbar;
			}
			set
			{
				m_bEnableToolbar = value;
				
				//	Notify each viewer
				for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
				{
					if(m_aViewers[i] != null)
					{
						try
						{
							m_aViewers[i].EnableToolbar = m_bEnableToolbar;
						}
						catch
						{
						}
				
					}// if(m_aViewers[i] != null)
				
				}// for(int i = 0; i < (int)TmaxMediaViewers.MaxViewers; i++)
						
			}
		
		}// EnableToolbar
		
		/// <summary>True to confine playback range to Start/Stop positions</summary>
		public bool LockVideoRange
		{
			get
			{
				return m_bLockVideoRange;
			}
			set
			{
				m_bLockVideoRange = value;
				
				if(m_aViewers[(int)TmaxMediaViewers.Tmmovie] != null)
					((CTmxMovie)m_aViewers[(int)TmaxMediaViewers.Tmmovie]).LockRange = m_bLockVideoRange;
			}
		
		}// LockVideoRange
		
		/// <summary>True to enable simulated playback if the video file does not exist</summary>
		public bool EnablePlayerSimulation
		{
			get
			{
				return m_bEnablePlayerSimulation;
			}
			set
			{
				m_bEnablePlayerSimulation = value;
				
				if(m_aViewers[(int)TmaxMediaViewers.Tmmovie] != null)
					((CTmxMovie)m_aViewers[(int)TmaxMediaViewers.Tmmovie]).EnableSimulation = m_bEnablePlayerSimulation;
			}
		
		}// EnablePlayerSimulation
		
		/// <summary>Text displayed during simulated playback</summary>
		public string SimulationText
		{
			get
			{
				return m_strSimulationText;
			}
			set
			{
				m_strSimulationText = value;
				
				if(m_aViewers[(int)TmaxMediaViewers.Tmmovie] != null)
					((CTmxMovie)m_aViewers[(int)TmaxMediaViewers.Tmmovie]).SimulationText = m_strSimulationText;
			}
		
		}// SimulationText
		
		/// </summary>This is the fully qualified path of the current file
		public string Path
		{
			get
			{
				if(m_tmaxViewer != null)
				{
					return m_tmaxViewer.Filename;
				}
				else
				{
					return "";
				}
			}
		
		}// Path
		
		/// </summary>Exposes the position used by the viewer's navigation controls</summary>
		public int NavigatorPosition
		{
			get
			{
				if(m_tmaxViewer != null)
				{
					return m_tmaxViewer.NavigatorPosition;
				}
				else
				{
					return -1;
				}
			}
		
		}
		
		/// </summary>Exposes the total positions used by the viewer's navigation controls</summary>
		public int NavigatorTotal
		{
			get
			{
				if(m_tmaxViewer != null)
				{
					return m_tmaxViewer.NavigatorTotal;
				}
				else
				{
					return 0;
				}
			}
		
		}
		
		#endregion Properties
		
	}//	class CTmaxViewerCtrl
	
	public enum TmaxMediaViewers
	{
		Tmview = 0,
		Tmpower,
		Tmmovie,
		MaxViewers,
	}
		
}// namespace FTI.Trialmax.Controls
