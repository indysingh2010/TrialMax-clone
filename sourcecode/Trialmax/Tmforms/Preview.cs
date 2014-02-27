using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Forms
{
	/// <summary>This forms contains a TrialMax preview control used to preview video designations and clips</summary>
	public class CFPreview : CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>TrialMax video previewing control</summary>
		private FTI.Trialmax.Controls.CTmaxVideoPlayerCtrl m_ctrlPlayer;
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Local member bound to XmlDesignation property</summary>
		private FTI.Shared.Xml.CXmlDesignation m_xmlDesignation = null;
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		/// <summary>Local member to store a flag to indicate that the form has been loaded</summary>
		private bool m_bLoaded = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFPreview()
		{
			InitializeComponent();

			//	Attach to the player's event source
			m_ctrlPlayer.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.m_tmaxEventSource.OnError);
			m_ctrlPlayer.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.m_tmaxEventSource.OnDiagnostic);
		}
		
		/// <summary>This method allows the caller to set the FileSpec and Designation properties</summary>
		/// <param name="strFileSpec">The new FileSpec value</param>
		/// <param name="xmlDesignation">The new XmlDesignation value</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		{
			m_strFileSpec = strFileSpec;
			m_xmlDesignation = xmlDesignation;
			
			if(m_bLoaded == true)
				return m_ctrlPlayer.SetProperties(strFileSpec, xmlDesignation);
			else
				return true;
		
		}// public bool SetProperties(string strFileSpec, CXmlDesignation xmlDesignation)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called when the window is moved</summary>
		/// <param name="e">system event arguments</param>
		protected override void OnMove(System.EventArgs e)
		{
			base.OnMove(e);
			
			//	Notify the player
			if((m_ctrlPlayer != null) && (m_ctrlPlayer.IsDisposed == false))
				m_ctrlPlayer.OnParentMoved();
		
		}// protected override void OnMove(System.EventArgs e)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFPreview));
			this.m_ctrlPlayer = new FTI.Trialmax.Controls.CTmaxVideoPlayerCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlPlayer
			// 
			this.m_ctrlPlayer.AllowApply = false;
			this.m_ctrlPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPlayer.LinkPosition = -1;
			this.m_ctrlPlayer.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlPlayer.Name = "m_ctrlPlayer";
			this.m_ctrlPlayer.PlayerPosition = -1;
			this.m_ctrlPlayer.PlayOnLoad = true;
			this.m_ctrlPlayer.PreviewPeriod = 0;
			this.m_ctrlPlayer.ShowPosition = true;
			this.m_ctrlPlayer.ShowTranscript = true;
			this.m_ctrlPlayer.Size = new System.Drawing.Size(348, 325);
			this.m_ctrlPlayer.StartPosition = -1;
			this.m_ctrlPlayer.StopPosition = -1;
			this.m_ctrlPlayer.TabIndex = 0;
			this.m_ctrlPlayer.TuneMode = FTI.Trialmax.Controls.TmaxVideoCtrlTuneModes.None;
			// 
			// CFPreview
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(348, 325);
			this.Controls.Add(this.m_ctrlPlayer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFPreview";
			this.Text = "  Preview";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.OnClosing);
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>This function handles all Load events</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event parameters - no data</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			
			
			//	Did the caller provide the file and designation?
			if((FileSpec.Length > 0) && (XmlDesignation != null))
			{
				m_ctrlPlayer.SetProperties(FileSpec, XmlDesignation);
			}
		
			m_bLoaded = true;
			
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

		}// private void OnLoad(object sender, System.EventArgs e)
		
		/// <summary>This function handles all Closing events</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event parameters - no data</param>
		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if(m_ctrlPlayer != null)
				{
					m_ctrlPlayer.Stop();
					m_ctrlPlayer.SetProperties("", null);
				}
			}
			catch
			{
			}

		}// private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>Media viewer component</summary>
		public FTI.Trialmax.Controls.CTmaxVideoPlayerCtrl Player
		{
			get	{ return m_ctrlPlayer; }
		}

		/// <summary>Fully qualified path to the video file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
		}
		
		/// <summary>The XML designation being previewed</summary>
		public CXmlDesignation XmlDesignation
		{
			get	{ return m_xmlDesignation; }
		}		
		
		#endregion Properties
		
	
	}// public class CFPreview : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
