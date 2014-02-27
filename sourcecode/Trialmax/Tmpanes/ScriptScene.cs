using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Panes
{
	/// <summary>User control class to display a scene object from a TrialMax media script</summary>
	public class CScriptScene : System.Windows.Forms.UserControl
	{
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member bounded to Secondary property</summary>
		private FTI.Trialmax.Database.CDxSecondary m_dxSecondary = null;
		
		/// <summary>Local class member bound to SelectBorderSize property</summary>
		private int m_iSelectBorderSize = 5;

		/// <summary>Local class member bound to TextHeight property</summary>
		private int m_iTextHeight = 0;

		/// <summary>Local member bound to StatusText property</summary>
		private string m_strStatusText = "";

		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";

		/// <summary>Local member bound to ZapSourceFileSpec property</summary>
		private string m_strZapSourceFileSpec = "";

		/// <summary>Local class member bound to Failed property</summary>
		private bool m_bFailed = false;
		
		/// <summary>Member bound to Viewer property</summary>
		private FTI.Trialmax.Controls.CTmaxViewerCtrl m_ctrlViewer = null;
		
		/// <summary>Status bar control</summary>
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatusBar;
		private System.Windows.Forms.Label m_ctrlMessage;

		/// <summary>Local class member bound to Selected property</summary>
		private bool m_bSelected = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="dxSecondary">Secondary exchange object associated with the scene</param>
		public CScriptScene(CDxSecondary dxSecondary)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			m_dxSecondary = dxSecondary;
		
			m_tmaxEventSource.Name = "CScriptScene";
			
			//	Get the height required to display the text
			m_iTextHeight = m_ctrlStatusBar.Height;
			
			//	Set the initial size and positions of the child controls
			RecalcLayout();
		
			if(m_ctrlStatusBar != null)
				m_ctrlStatusBar.Text = StatusText;
		}
		
		/// <summary>This method is called to set the size and position</summary>
		/// <param name="rcPos">A rectangle that defines the window bounds</param>
		public void SetPos(Rectangle rcPos)
		{
			//	Do we need to resize?
			if((rcPos.Width != this.Width) || (rcPos.Height != this.Height))
			{
				SetBounds(rcPos.Left, rcPos.Top, rcPos.Width, rcPos.Height);
			}
			else
			{
				this.Location = new Point(rcPos.Left, rcPos.Top);
			}
			
		}// public void SetPos(Rectangle rcPos) 
		
		/// <summary>This method is called to get the height required to display the viewer at the specified ascpect ratio</summary>
		/// <param name="iWidth">The maximum available width</param>
		/// <param name="fAspectRatio">The desired aspect ratio</param>
		/// <returns>The height for the requested aspect ratio</returns>
		public int GetPreferredHeight(int iWidth, float fAspectRatio)
		{
			int iHeight = 0;
			
			//	Make sure the caller specified a valid aspect ratio
			if(fAspectRatio <= 0) return 0;
			if(iWidth <= 0) return 0;
			
			iHeight = (int)((float)iWidth / fAspectRatio);
			
			//	Now allow for the text descriptor
			iHeight += m_iTextHeight;
			
			return iHeight;
			
		}// public int GetPreferredHeight(int iWidth, float fAspectRatio)
		
		/// <summary>This method is called to get the height required to display the viewer at the default 4/3 aspect ratio</summary>
		/// <param name="iWidth">The maximum available width</param>
		/// <returns>The height for the requested aspect ratio</returns>
		public int GetPreferredHeight(int iWidth)
		{
			
			return GetPreferredHeight(iWidth, 4.0f / 3.0f);
			
		}// public int GetPreferredHeight(int iWidth)
		
		/// <summary>This function is called to allocate and initialize the child viewer control</summary>
		///	<param name="strFileSpec">The path to the file to be loaded in the viewer</param>
		/// <returns>true if successful</returns>
		public bool Initialize(string strFileSpec)
		{
			try
			{
				m_strFileSpec = strFileSpec;
				
				//	Does the file exist?
				if(System.IO.File.Exists(m_strFileSpec) == true)
				{
					this.m_ctrlViewer = new FTI.Trialmax.Controls.CTmaxViewerCtrl();
					
					//this.m_ctrlViewer.Visible = false;
					
					//	Attach to the viewer's event source
					m_tmaxEventSource.Attach(this.m_ctrlViewer.EventSource);
					this.m_ctrlViewer.EventSource.MouseDownEvent += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
					this.m_ctrlViewer.EventSource.MouseUpEvent += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
					this.m_ctrlViewer.EventSource.MouseMoveEvent += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
					this.m_ctrlViewer.EventSource.MouseDblClickEvent += new System.Windows.Forms.MouseEventHandler(this.OnMouseDblClick);
					
					//	Set the properties
					//this.m_ctrlViewer.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(192)));
					this.m_ctrlViewer.BackColor = System.Drawing.SystemColors.Control;
					this.m_ctrlViewer.Location = new System.Drawing.Point(0, 0);
					this.m_ctrlViewer.Name = "m_ctrlViewer";
					this.m_ctrlViewer.TabIndex = 3;
					this.m_ctrlViewer.Size = new System.Drawing.Size(1, 1);
					this.m_ctrlViewer.ShowToolbar = false;
					
					//	Attach to the viewer events
					this.m_ctrlViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
					this.m_ctrlViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
					this.m_ctrlViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);

					//	Add to the control collection
					this.Controls.Add(m_ctrlViewer);
					
					//	Perform the viewer initialization
					if(this.m_ctrlViewer.Initialize(strFileSpec) == true)
					{			
						m_bFailed = false;
					
						//	Make sure it is properly sized
						RecalcLayout();
					}
					else
					{
						m_ctrlMessage.Text = ("Unable to initialize viewer for: " + m_strFileSpec.ToLower());
						m_bFailed = true;
					}
				
				}
				else
				{
					m_ctrlMessage.Text = ("File not found: " + m_strFileSpec.ToLower());
					m_bFailed = true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetViewer", "Exception raised while trying to set the viewer: " + Ex.ToString());
				m_ctrlViewer = null;
				m_bFailed = true;
			}
		
			if(m_bFailed)
			{
				RecalcLayout();
				m_ctrlMessage.Visible = true;
			}
			
			return !m_bFailed;	
			
		}// public bool Initialize()
		
		/// <summary>This method is called to shut down the scene rendering control</summary>
		public void Terminate()
		{
			if(m_ctrlViewer != null && (m_ctrlViewer.IsDisposed != true))
			{
				m_ctrlViewer.Terminate();
			}
			
		}// public void Terminate()
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be played</param>
		/// <param name="dStart">The value at which to start the playback</param>
		/// <param name="dStop">The value at which to stop the playback</param>
		/// <returns>true if successful</returns>
		public bool Play(string strFilename, double dStart, double dStop)
		{
			if(m_ctrlViewer != null && (m_ctrlViewer.IsDisposed == false))
			{
				if(m_ctrlViewer.Play(strFilename, dStart, dStop) == true)
				{
					//	Make sure the control is visible
					if(m_ctrlViewer.Visible == false)
						m_ctrlViewer.Visible = true;
				
					return true;
				}
			}
			
			return false;
			
		}// public bool Play(string strFilename, double dStart, double dStop)
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be played</param>
		/// <returns>true if successful</returns>
		public bool View(string strFilename)
		{
			if(m_ctrlViewer != null && (m_ctrlViewer.IsDisposed == false))
			{
				m_ctrlViewer.ZapSourceFile = m_strZapSourceFileSpec;
				
				if(m_ctrlViewer.View(strFilename) == true)
				{
					//	Make sure the control is visible
					if(m_ctrlViewer.Visible == false)
						m_ctrlViewer.Visible = true;
				
					return true;
				}
			}
			
			return false;
			
		}// public bool Play(string strFilename, double dStart, double dStop)
		
		#endregion Public Methods

		#region Protected Methods
		
		protected override void DefWndProc(ref Message m)
		{
			string strText = "";
			
			if(m.Msg == FTI.Shared.Win32.User.WM_PAINT)
			{
				strText = "WM_PAINT";
			}
			else if(m.Msg == FTI.Shared.Win32.User.WM_NCPAINT)
			{
				strText = "WM_NCPAINT";
			}
			else if(m.Msg == FTI.Shared.Win32.User.WM_ERASEBKGND)
			{
				strText = "WM_ERASEBKGND";
			}
			else
			{
				strText = m.Msg.ToString();
			}

			//m_tmaxEventSource.FireDiagnostic(this, "DEF", "SCENE DWP: " + strText);
			base.DefWndProc(ref m);
		}
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				m_ctrlViewer = null;
			}
			base.Dispose( disposing );
		}

		/// <summary>Required method for form designer support</summary>
		protected void InitializeComponent()
		{
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			this.m_ctrlStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
			this.m_ctrlMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlStatusBar
			// 
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			this.m_ctrlStatusBar.Appearance = appearance1;
			this.m_ctrlStatusBar.Dock = System.Windows.Forms.DockStyle.None;
			this.m_ctrlStatusBar.Location = new System.Drawing.Point(8, 136);
			this.m_ctrlStatusBar.Name = "m_ctrlStatusBar";
			this.m_ctrlStatusBar.Size = new System.Drawing.Size(160, 20);
			this.m_ctrlStatusBar.TabIndex = 5;
			this.m_ctrlStatusBar.WrapText = false;
			this.m_ctrlStatusBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
			this.m_ctrlStatusBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
			this.m_ctrlStatusBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
			// 
			// m_ctrlMessage
			// 
			this.m_ctrlMessage.Location = new System.Drawing.Point(24, 36);
			this.m_ctrlMessage.Name = "m_ctrlMessage";
			this.m_ctrlMessage.TabIndex = 6;
			this.m_ctrlMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_ctrlMessage.Visible = false;
			this.m_ctrlMessage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
			this.m_ctrlMessage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
			this.m_ctrlMessage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
			// 
			// CScriptScene
			// 
			this.Controls.Add(this.m_ctrlMessage);
			this.Controls.Add(this.m_ctrlStatusBar);
			this.Name = "CScriptScene";
			this.Size = new System.Drawing.Size(176, 168);
			this.Resize += new System.EventHandler(this.OnResize);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
			this.ResumeLayout(false);

		}
		
		/// <summary>This function handles all Resize events</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event parameters - no data</param>
		protected void OnResize(object sender, System.EventArgs e)
		{
			//	Set the size and position of the child controls
			RecalcLayout();
		}

		/// <summary>This function is called to resize and reposition the child controls</summary>
		protected void RecalcLayout()
		{
			int iHeight;
			int	iWidth;
			
			//	Make sure the child controls have been created
			if(m_ctrlStatusBar == null) return;
			
			//	Make sure the selection border size is valid
			if(m_iSelectBorderSize < 0) 
				m_iSelectBorderSize = 0;
				
			//	Have we got the text height
			if(m_iTextHeight <= 0)
				if((m_iTextHeight = m_ctrlStatusBar.Height) <= 0) return;
			
			try
			{
				//	What is the available width for the controls?
				if((iWidth = this.Width - (2 * m_iSelectBorderSize)) <= 0)
					return;
				
				//	Set the size and position of the text control
				m_ctrlStatusBar.SetBounds(m_iSelectBorderSize, this.Height - (m_iTextHeight + m_iSelectBorderSize),
										  iWidth, m_iTextHeight);
									 
				//	What is the height of the viewer?
				if((iHeight = m_ctrlStatusBar.Top - m_iSelectBorderSize) < 0)
					iHeight = 0;

				//	Set the size and position of the viewer
				if((m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false))
				{				
					m_ctrlViewer.SetBounds(m_iSelectBorderSize, m_iSelectBorderSize,
										   iWidth, iHeight);
					//m_ctrlViewer.BringToFront();
				}
				if((m_ctrlMessage != null) && (m_ctrlMessage.IsDisposed == false))
				{				
					m_ctrlMessage.SetBounds(m_iSelectBorderSize, m_iSelectBorderSize,
						iWidth, iHeight);
					m_ctrlMessage.BringToFront();
				}
				
				
				//	Refresh
				//this.Refresh();
				//this.Invalidate(false);				
			}
			catch
			{
			}
		
		}// RecalcLayout()

		/// <summary>This method traps and handles all MouseDown events</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">Mouse event arguments</param>
		protected void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Propagate the event
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseDown(this, GetMouseArgs(e));
		}

		/// <summary>This method traps and handles all MouseUp events</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">Mouse event arguments</param>
		protected void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Propagate the event
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseUp(this, GetMouseArgs(e));
		}

		/// <summary>This method traps and handles all MouseMove events</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">Mouse event arguments</param>
		protected void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Propagate the event
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseMove(this, GetMouseArgs(e));
		}

		/// <summary>This method traps and handles all MouseDblClick events</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">Mouse event arguments</param>
		protected void OnMouseDblClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Propagate the event
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseDblClick(this, GetMouseArgs(e));
		}

		/// <summary>This method copies the caller's arguement but replaces the coordinates with screen coordinates</summary>
		/// <param name="e">Mouse event arguments</param>
		/// <returns>Converted mouse arguments</returns>
		protected System.Windows.Forms.MouseEventArgs GetMouseArgs(System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.MouseEventArgs Args = null;
			
			Args = new MouseEventArgs(e.Button, 
				e.Clicks,
				System.Windows.Forms.Cursor.Position.X,
				System.Windows.Forms.Cursor.Position.Y,
				e.Delta);
				
			return Args;
		
		}

		/// <summary>This method handles events fired by the background panel when it needs to be painted</summary>
		/// <param name="e">The system paint arguments</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnPaint", "Scene Paint: " + StatusText);
			
			try
			{
				DrawBorder(e.Graphics, false);
			}
			catch
			{
			}
			
		}// protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)

		/// <summary>This method handles events fired by the background panel when it needs to be painted</summary>
		/// <param name="e">The system paint arguments</param>
		protected override void OnInvalidated(System.Windows.Forms.InvalidateEventArgs e)
		{
			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnInvalidate", "Scene Invalidate");
			}
			catch
			{
			}
			
		}// private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)

		#endregion Protected Methods

		/// <summary>This method will draw the control's border</summary>
		/// <param name="gdi">The graphics object to draw into</param>
		/// <param name="bErase">True to erase the border</param>
		private void DrawBorder(System.Drawing.Graphics gdi, bool bErase)
		{
			System.Drawing.Pen pen;
			int iWidth = 0;
			int iHeight = 0;
			
			if((this != null) && (this.IsDisposed == false))
			{
				iWidth = this.Width - 1;
				iHeight = this.Height - 1;
			}
			else
			{
				return;
			}
			
			//	Are we erasing the border?
			if(bErase == true)
			{
				if((pen = new System.Drawing.Pen(SystemColors.Control, m_iSelectBorderSize * 2)) != null)
				{
					gdi.DrawRectangle(pen, 0, 0, iWidth, iHeight);
				}
			}
			else
			{	
				if(m_bSelected == true)
				{
					// Create the pen needed to draw the selection border
					//
					//	NOTE:	By doubling the width we need we don't have to worry about adjusting the
					//			rectangle to allow for the pen thickness
					if((pen = new System.Drawing.Pen(SystemColors.ControlText, m_iSelectBorderSize * 2)) != null)
					{
						gdi.DrawRectangle(pen, 0, 0, iWidth, iHeight);
					}
				}
				else
				{
					//	Draw a normal border
					gdi.DrawRectangle(SystemPens.ControlText, 0, 0, iWidth, iHeight);
				}
				
			}
			
		}// private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)

		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }

		}// EventSource property
		
		/// <summary>Flag to indicate if attempt to create the viewer failed</summary>
		public bool Failed
		{
			get { return m_bFailed; }
			set { m_bFailed = value; }
			
		}// Failed property
		
		/// <summary>TrialMax media viewer control</summary>
		public FTI.Trialmax.Controls.CTmaxViewerCtrl Viewer
		{
			get { return m_ctrlViewer; }
			
		}// Viewer property
		
		/// <summary>Secondary record exchange object associated with the scene</summary>
		public FTI.Trialmax.Database.CDxSecondary Secondary
		{
			get { return m_dxSecondary; }
			set { m_dxSecondary = value; }
			
		}// Secondary property
		
		/// <summary>Test to be displayed in the status bar</summary>
		public string StatusText
		{
			get
			{
				return m_strStatusText;
			}
			set
			{
				m_strStatusText = value;
				
				if((m_ctrlStatusBar != null) && (m_ctrlStatusBar.IsDisposed == false))
					m_ctrlStatusBar.Text = m_strStatusText;
			}
			
		}// StatusText property
		
		/// <summary>Size in pixels of the selection border</summary>
		public int SelectBorderSize
		{
			get
			{
				return m_iSelectBorderSize;
			}
			set
			{
				m_iSelectBorderSize = value;
				
				RecalcLayout();
			}
			
		}// SelectBorderSize property
		
		/// <summary>Size in pixels required for the text description</summary>
		public int TextHeight
		{
			get { return m_iTextHeight; }
			
		}// TextHeight property
		
		/// <summary>The file to be viewed in the scene</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set { m_strFileSpec = value; }
			
		}// FileSpec property
		
		/// <summary>The is the path to the source image when viewing a treatment</summary>
		public string ZapSourceFileSpec
		{
			get { return m_strZapSourceFileSpec; }
			set { m_strZapSourceFileSpec = value; }
			
		}// ZapSourceFileSpec property
		
		/// <summary>True if the viewer is loaded</summary>
		public bool IsLoaded
		{
			get
			{
				if(m_ctrlViewer != null)
				{
					if(m_ctrlViewer.Filename != null)
						return (m_ctrlViewer.Filename.Length > 0);
						
				}
				return false;
			}
			
		}// IsLoaded property
		
		/// <summary>Indicates if the control is currently selected</summary>
		public bool Selected
		{
			get
			{
				return m_bSelected;
			}
			set
			{
				// Only act on the new value if the m_bSelected state is changing
				if(m_bSelected != value)
				{
					m_bSelected = value;

					//      Force repainting of the background panel if it's visible
					if(this.Visible == true)
					{
						Invalidate(false);
					}
				}
			}
                        
		}// m_bSelected property
		
		#endregion Properties

	}// public class CScriptScene : System.Windows.Forms.UserControl

	/// <summary>This class is used to manage a dynamic list of CScriptScene objects</summary>
	public class CScriptScenes : FTI.Shared.Trialmax.CTmaxSortedArrayList
	{
		#region Events
		
		/// <summary>This event is fired when the selection(s) is about to change</summary>
		public event SelectionChangingHandler SelectionChanging;
		
		/// <summary>This event is fired when the selection(s) have changed</summary>
		public event SelectionChangedHandler SelectionChanged;
		
		/// <summary>This is the delegate used to handle all SelectionChanging events</summary>
		/// <param name="objSender">Object firing the event</param>
		public delegate void SelectionChangingHandler(object objSender);
		
		/// <summary>This is the delegate used to handle all SelectionChanged events</summary>
		/// <param name="objSender">Object firing the event</param>
		public delegate void SelectionChangedHandler(object objSender);
			
		#endregion Events
		
		#region Private Members
		
		/// <summary>Local member bounded to Primary property</summary>
		FTI.Trialmax.Database.CDxPrimary m_dxPrimary = null;
				
		/// <summary>Local member to keep track of selected scenes</summary>
		System.Collections.ArrayList m_aSelections = new System.Collections.ArrayList();
				
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CScriptScenes()
		{
			//	Set up the sorter
			this.Comparer = new CScriptSceneSorter();
			this.KeepSorted = true;
			this.EventSource.Attach(this.Sorter.EventSource);
		}

		/// <summary>This method allows the caller to add a new object to the list</summary>
		/// <param name="oScene">Object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CScriptScene Add(CScriptScene oScene)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(oScene as object);

				//	Daisy chain the event triggers
				this.EventSource.Attach(oScene.EventSource);
				
				return oScene;
			}
			catch
			{
				return null;
			}
			
		}// Add(CScriptScene oScene)

		/// <summary>This method is called to remove the specified object from the collection</summary>
		/// <param name="oScene">The object to be removed</param>
		public void Remove(CScriptScene oScene)
		{
			try
			{
				if(m_aSelections.Contains(oScene) == true)
					m_aSelections.Remove(oScene);
					
				// Use base class to process actual collection operation
				base.Remove(oScene as object);
			}
			catch
			{
			}
		
		}// public void Remove(CScriptScene oScene)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="oScene">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CScriptScene oScene)
		{
			// Use base class to process actual collection operation
			return base.Contains(oScene as object);
		
		}// public bool Contains(CScriptScene oScene)

		/// <summary>This method is called to clear all objects from the collection</summary>
		public override void Clear()
		{
			//	Clear the local selections
			if(m_aSelections != null)
				m_aSelections.Clear();
				
			//	Do the base class processing
			base.Clear();
		
		}// public override void Clear()

		/// <summary>Gets the object at the specified index</summary>
		/// <returns>The object at the requested index</returns>
		public new CScriptScene GetAt(int index)
		{
			return (base.GetAt(index) as CScriptScene);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>Object at the specified index</returns>
		public new CScriptScene this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return GetAt(index);
			}
		
		}// public CScriptScene this[int index]

		/// <summary>This method is called to get the index of the scene associated with the specified secondary exchange record</summary>
		/// <param name="dxSecondary">The secondary exchange record associated with the scene</param>
		///	<returns>The scene index if found</returns>
		public int GetIndex(CDxSecondary dxSecondary)
		{
			CScriptScene Scene = null;
			
			for(int i = 0; i < Count; i++)
			{
				if((Scene = (CScriptScene)GetAt(i)) != null)
				{
					if(ReferenceEquals(Scene.Secondary, dxSecondary) == true)
						return i;
				}
				
			}
			
			//	Must not have found the scene
			return -1;	
			
		}// public int GetIndex(CDxSecondary dxSecondary)

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CScriptScene value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>This method is called to select the specified object from the collection</summary>
		/// <param name="oScene">The object to be selected</param>
		/// <param name="bSingle">true for single selection</param>
		public void Select(CScriptScene oScene, bool bSingle)
		{
			FireSelectionChanging();
			
			//	Clear the current selections if this is single select
			if(bSingle == true)
				m_aSelections.Clear();
				
			foreach(CScriptScene O in this)
			{
				if((oScene != null) && (ReferenceEquals(oScene, O) == true))
				{
					O.Selected = true;
					
					if(m_aSelections.Contains(O) == false)
						m_aSelections.Add(O);
					
					if(bSingle == false)
						break;
				}
				else
				{
					if((oScene == null) || (bSingle == true))
						O.Selected = false;
				}
				
			}
			
			FireSelectionChanged();
		
		}// public void Select(CScriptScene oScene, bool bSingle)

		/// <summary>This method is called to select the specified object and clear all others</summary>
		/// <param name="oScene">The object to be selected</param>
		public void Select(CScriptScene oScene)
		{
			Select(oScene, true);
			
		}// public void Remove(CScriptScene oScene)

		/// <summary>This method is called to select the specified object from the collection</summary>
		/// <param name="iScene">The index of the object to be selected</param>
		/// <param name="bSingle">true for single selection</param>
		public void Select(int iScene, bool bSingle)
		{
			if((iScene >= 0) && (iScene < this.Count))
				Select(GetAt(iScene), bSingle);
			else
				Select((CScriptScene)null);	
		
		}// public void Select(int iScene, bool bSingle)

		/// <summary>This method is called to select the specified object and clear all others</summary>
		/// <param name="iScene">The index of the object to be selected</param>
		public void Select(int iScene)
		{
			if((iScene >= 0) && (iScene < this.Count))
				Select(GetAt(iScene), true);
			else
				Select((CScriptScene)null);	
		
		}// public void Select(int iScene, bool bSingle)

		/// <summary>This method is called to select a range of objects</summary>
		/// <param name="iFrom">Index of first object to be selected</param>
		/// <param name="iTo">Index of last object to be selected</param>
		/// <param name="bClearOthers">true to deselect all outside the range</param>
		public void Select(int iFrom, int iTo, bool bClearOthers)
		{
			Debug.Assert(iFrom <= iTo);

			FireSelectionChanging();
			
			if(bClearOthers)
				m_aSelections.Clear();
				
			for(int i = 0; i < this.Count; i++)
			{
				if((i >= iFrom) && (i <= iTo))
				{
					this[i].Selected = true;
					
					if(m_aSelections.Contains(this[i]) == false)
						m_aSelections.Add(this[i]);
				}
				else
				{
					if(bClearOthers == true)
						this[i].Selected = false;
				}
				
			}
			
			FireSelectionChanged();
		
		}// public void Select(int iFrom, int iTo, bool bClearOthers)

		/// <summary>This method is called to retrieve the first selected scene in the collection</summary>
		///	<returns>The first selected scene</returns>
		public CScriptScene GetSelection()
		{
			if(m_aSelections.Count > 0)
			{
				return (m_aSelections[0] as CScriptScene);
			}
			else
			{
				//	Shouldn't have to do this but....
				foreach(CScriptScene O in this)
				{
					if(O.Selected == true)
						return O;
				}
			}
		
			return null;
			
		}// public CScriptScene GetSelection()

		/// <summary>This method is called to populate the specified collection with all the selected scenes</summary>
		/// <param name="aScenes">The collection to be populated</param>
		///	<returns>The total number of selections</returns>
		public int GetSelections(CScriptScenes aScenes)
		{
			Debug.Assert(aScenes != null);
			if(aScenes == null) return 0;
			
			//	Clear the existing objects
			aScenes.Clear();
			
			foreach(CScriptScene O in m_aSelections)
			{
				//	Just in case...
				if((O.Selected == true) && (this.Contains(O) == true))
					aScenes.Add(O);
			}
		
			return aScenes.Count;
			
		}// public void GetSelections(CScriptScenes aScenes)

		/// <summary>This method is called to get a collection of selected scenes</summary>
		///	<returns>The collection of selected scenes</returns>
		public CScriptScenes GetSelections()
		{
			CScriptScenes aScenes = new CScriptScenes();
			
			//	Maintain the order in which the scenes were selected
			aScenes.KeepSorted = false;
			
			//	Get the current selections
			GetSelections(aScenes);
			
			if(aScenes.Count > 0)
				return aScenes;
			else
				return null;
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to fire the SelectionChanging event</summary>
		protected void FireSelectionChanging()
		{
			if(SelectionChanging != null)
				SelectionChanging(this);
		}
		
		/// <summary>This method is called to fire the SelectionChanged event</summary>
		protected void FireSelectionChanged()
		{
			if(SelectionChanged != null)
				SelectionChanged(this);
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Sorter used to sort the nodes in the collection</summary>
		///
		///	<remarks>This property allows the owner to reference the base class Comparer property as a script sorter object</remarks>
		public CScriptSceneSorter Sorter
		{
			get
			{
				return (Comparer as CScriptSceneSorter);
			}
			set
			{
				Comparer = value;
			}
		
		}// Sorter property
		
		/// <summary>Primary record exchange object associated with the script</summary>
		public FTI.Trialmax.Database.CDxPrimary Primary
		{
			get
			{
				return m_dxPrimary;
			}
			set
			{
				m_dxPrimary = value;
				
			}
			
		}// Primary property
		
		#endregion Properties
		
	}//	public class CScriptScenes
		
	/// <summary>
	/// Objects of this class are used to sort nodes in the tree
	/// </summary>
	public class CScriptSceneSorter : IComparer
	{
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_eventSource = new CTmaxEventSource();

		#endregion Private Members
		
		#region Private Methods
		

		#endregion Private Methods
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CScriptSceneSorter()
		{
			//	Set the default event source name
			m_eventSource.Name = "Scene Sorter";
		}
		
		/// <summary>
		/// This method is called to compare the local property values to the values assigned to the specified sorter object
		/// </summary>
		/// <param name="tmaxSorter">The object to be compared</param>
		/// <returns>true if both objects are equivalent</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
			
		/// <summary>
		/// This function is called to compare two nodes in the tree
		/// </summary>
		/// <param name="x">First node to be compared</param>
		/// <param name="y">Second node to be compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		int IComparer.Compare(object x, object y) 
		{
			return Compare((CScriptScene)x, (CScriptScene)y);
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to compare two scenes in the script</summary>
		/// <param name="Scene1">First scene to be compared</param>
		/// <param name="Scene2">Second scene to be compared</param>
		/// <returns>-1 if Scene1 less than Scene2, 0 if equal, 1 if greater than</returns>
		protected virtual int Compare(CScriptScene Scene1, CScriptScene Scene2)
		{
			try
			{
				//	Check for equality first
				//
				//	NOTE:	.NET raises and exception if we don't return 0 for
				//			equal objects
				if(ReferenceEquals(Scene1, Scene2) == true)
				{
					return 0;
				}
				else
				{
					if((Scene1.Secondary != null) && (Scene2.Secondary != null))
					{
						if(Scene1.Secondary.GetDisplayOrder() < Scene2.Secondary.GetDisplayOrder())
							return -1;
						else if(Scene1.Secondary.GetDisplayOrder() == Scene2.Secondary.GetDisplayOrder())
							return 0;
						else
							return 1;
					}
					else
					{
						return 0;
					}
					
				}
		
			}
			catch(System.Exception Ex)
			{
				m_eventSource.FireError(this, "Compare", "Exception:", Ex);
				return 0;
			}
			
		
		}//protected virtual int Compare(CScriptScene Scene1, CScriptScene Scene2)
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get
			{
				return m_eventSource;
			}
			
		}// EventSource property

		#endregion Properties
		
	}// class CScriptSceneSorter
	
}// namespace FTI.Trialmax.Panes
