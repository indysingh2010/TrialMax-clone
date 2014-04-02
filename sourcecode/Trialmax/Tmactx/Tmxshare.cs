using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using FTI.Shared.Trialmax;

namespace FTI.Trialmax.ActiveX
{
	/// <summary>This control manages the shared communications between the Manager and Presentation applications</summary>
	public class CTmxShare : FTI.Trialmax.ActiveX.CTmxBase
	{
		/// <summary>Tmshare command request</summary>
		public event TmxShareEventHandler TmxShareRequestEvent;
		public event TmxShareEventHandler TmxShareResponseEvent;
		
		const int TMSHARE_COMMAND_NONE = 0;
		const int TMSHARE_COMMAND_LOAD = 1;
		const int TMSHARE_COMMAND_ADD_TREATMENT = 2;
		const int TMSHARE_COMMAND_ADD_TO_BINDER = 3;
		const int TMSHARE_COMMAND_UPDATE_TREATMENT = 4;
		
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>TrialMax Tm_share control instance</summary>
		private AxTM_SHARE6Lib.AxTMShare6 m_tmxShare = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmxShare() : base()
		{
			m_strDescription = "TrialMax Application Share Control (tmshare)";
			m_tmaxEventSource.Name = m_strDescription;
			
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		/// <summary>This method is called to launch presentation</summary>
		/// <returns>True if successful</returns>
		public bool Execute()
		{
			if(m_tmxShare != null)
				return (m_tmxShare.Execute() == 0);
			else
				return false;	
		}
		
		/// <summary>This method is called to send a request to presentation</summary>
		/// <returns>True if successful</returns>
		public bool Open()
		{
			if(m_tmxShare != null)
			{
				m_tmxShare.Command = TMSHARE_COMMAND_LOAD;
				return (m_tmxShare.SetRequest(0) == 0);
			}
			else
			{
				return false;	
			}
		}
		
		/// <summary>This method is called to send a response to presentation</summary>
		/// <returns>True if successful</returns>
		public bool Respond()
		{
			if(m_tmxShare != null)
			{
				return (m_tmxShare.SetResponse() == 0);
			}
			else
			{
				return false;	
			}
		}
		
		/// <summary>This method is called to send a request to presentation</summary>
		/// <returns>True if successful</returns>
		public bool Request()
		{
			if(m_tmxShare != null)
			{
				return (m_tmxShare.SetRequest(0) == 0);
			}
			else
			{
				return false;	
			}
		}
		
		/// <summary>
		/// This method is called to perform the ActiveX initialization of the control
		/// </summary>
		/// <returns>true if successful</returns>
		public override bool AxInitialize()
		{
			try
			{
				if(m_tmxShare != null)
					m_bInitialized = (m_tmxShare.Initialize() == 0);
			}
			catch
			{
				m_bInitialized = false;
			}			
			
			return m_bInitialized;
		}
		
		/// <summary>
		/// This method is called to perform the ActiveX termination of the control
		/// </summary>
		public override void AxTerminate()
		{
			if(m_tmxShare != null)
				m_tmxShare.Terminate();
			m_bInitialized = false;
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method converts the .NET command enumeration to a valid ActiveX command</summary>
		/// <param name="eCommand">The desired .NET enumeration</param>
		/// <returns>The equivalent ActiveX command identifier</returns>
		private int TranslateCommand(TmxShareCommands eCommand)
		{
			switch(eCommand)
			{
				case TmxShareCommands.Open:
					return TMSHARE_COMMAND_LOAD;
					
				case TmxShareCommands.AddTreatment:
					return TMSHARE_COMMAND_ADD_TREATMENT;
					
				case TmxShareCommands.AddToBinder:
					return TMSHARE_COMMAND_ADD_TO_BINDER;

				case TmxShareCommands.UpdateTreatment:
					return TMSHARE_COMMAND_UPDATE_TREATMENT;

				case TmxShareCommands.None:
				default:
					return TMSHARE_COMMAND_NONE;
			}
			
		}// private int TranslateCommand(TmshareCommands eCommand)
		
		/// <summary>This method converts the specified ActiveX command identifier to its equivalent .NET enumeration</summary>
		/// <param name="iAxCommand">The desired ActiveX identifier</param>
		/// <returns>The equivalent .NET enumeration</returns>
		private TmxShareCommands TranslateCommand(int iAxCommand)
		{
			switch(iAxCommand)
			{
				case TMSHARE_COMMAND_LOAD:
					return TmxShareCommands.Open;
					
				case TMSHARE_COMMAND_ADD_TREATMENT:
					return TmxShareCommands.AddTreatment;
					
				case TMSHARE_COMMAND_ADD_TO_BINDER:
					return TmxShareCommands.AddToBinder;

				case TMSHARE_COMMAND_UPDATE_TREATMENT:
					return TmxShareCommands.UpdateTreatment;

				case TMSHARE_COMMAND_NONE:
				default:
					return TmxShareCommands.None;
			}
			
		}// private TmxShareCommands TranslateCommand(int iAxCommand)
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmxShare));
			this.m_tmxShare = new AxTM_SHARE6Lib.AxTMShare6();
			((System.ComponentModel.ISupportInitialize)(this.m_tmxShare)).BeginInit();
			this.SuspendLayout();
			// 
			// m_tmxShare
			// 
			this.m_tmxShare.Enabled = true;
			this.m_tmxShare.Location = new System.Drawing.Point(12, 8);
			this.m_tmxShare.Name = "m_tmxShare";
			this.m_tmxShare.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_tmxShare.OcxState")));
			this.m_tmxShare.Size = new System.Drawing.Size(180, 40);
			this.m_tmxShare.TabIndex = 0;
			this.m_tmxShare.CommandResponse += new System.EventHandler(this.OnAxCommandResponse);
			this.m_tmxShare.AxError += new AxTM_SHARE6Lib._DTMShare6Events_AxErrorEventHandler(this.OnAxError);
			this.m_tmxShare.CommandRequest += new System.EventHandler(this.OnAxCommandRequest);
			this.m_tmxShare.AxDiagnostic += new AxTM_SHARE6Lib._DTMShare6Events_AxDiagnosticEventHandler(this.OnAxDiagnostic);
			// 
			// CTmxShare
			// 
			this.Controls.Add(this.m_tmxShare);
			this.Name = "CTmxShare";
			this.Size = new System.Drawing.Size(200, 56);
			((System.ComponentModel.ISupportInitialize)(this.m_tmxShare)).EndInit();
			this.ResumeLayout(false);

		}
		
		/// <summary>This method traps AxError events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxError(object sender, AxTM_SHARE6Lib._DTMShare6Events_AxErrorEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireError(this, "TMShare", e.lpszMessage);	
		}

		/// <summary>This method traps AxDiagnostic events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxDiagnostic(object sender, AxTM_SHARE6Lib._DTMShare6Events_AxDiagnosticEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireDiagnostic(this, e.lpszMethod, e.lpszMessage);	
		}

		/// <summary>This method traps CommandRequest events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxCommandRequest(object sender, System.EventArgs e)
		{
			if(TmxShareRequestEvent != null)
			{
				try
				{ 
					if(m_tmxShare.GetRequest() == 0)
					{
						TmxShareRequestEvent(sender);
					}
				}
				catch
				{
				}
				
			}
		}
		
		/// <summary>This method traps CommandResponse events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxCommandResponse(object sender, System.EventArgs e)
		{
			if(TmxShareResponseEvent != null)
			{
				try
				{
					if(m_tmxShare.GetResponse() == 0)
						TmxShareResponseEvent(sender);
				}
				catch
				{
				}
				
			}
		}
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The folder containing the applications</summary>
		public string AppFolder
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.AppFolder; 
				else
					return "";
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.AppFolder = value; 
			}
			
		}
		
		/// <summary>The fully qualifed path to the database folder</summary>
		public string CaseFolder
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.CaseFolder;
				else
					return "";
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.CaseFolder = value; 
			}
			
		}
		
		/// <summary>The barcode to be acted on</summary>
		public string Barcode
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.Barcode;
				else
					return "";
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.Barcode = value; 
			}
			
		}
		
		/// <summary>The name of the file associated with the event</summary>
		public string SourceFileName
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.SourceFileName;
				else
					return "";
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.SourceFileName = value; 
			}
			
		}
		
		/// <summary>The path to the file associated with the event</summary>
		public string SourceFilePath
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.SourceFilePath;
				else
					return "";
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.SourceFilePath = value; 
			}
			
		}
		
		/// <summary>The unique id of the new treatment</summary>
		public int PrimaryId
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.PrimaryId;
				else
					return 0;
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.PrimaryId = value; 
			}
			
		}
		
		/// <summary>The unique id of the new treatment</summary>
		public int SecondaryId
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.SecondaryId;
				else
					return 0;
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.SecondaryId = value; 
			}
			
		}
		
		/// <summary>The unique id of the new treatment</summary>
		public int TertiaryId
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.TertiaryId;
				else
					return 0;
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.TertiaryId = value; 
			}
			
		}
		
		/// <summary>The unique id of the new treatment</summary>
		public int QuaternaryId
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.QuaternaryId;
				else
					return 0;
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.QuaternaryId = value; 
			}
			
		}
		
		/// <summary>The unique id of the new treatment</summary>
		public int BarcodeId
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.BarcodeId;
				else
					return 0;
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.BarcodeId = value; 
			}
			
		}
		
		/// <summary>The unique id of the new treatment</summary>
		public int DisplayOrder
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.DisplayOrder;
				else
					return 0;
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.DisplayOrder = value; 
			}
			
		}

		/// <summary>The page number to start deposition playback</summary>
		public int PageNumber
		{
			get
			{
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.PageNumber;
				else
					return 0;
			}
			set
			{
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.PageNumber = value;
			}

		}

		/// <summary>The page number to start deposition playback</summary>
		public short LineNumber
		{
			get
			{
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					return m_tmxShare.LineNumber;
				else
					return 0;
			}
			set
			{
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.LineNumber = value;
			}

		}

		/// <summary>The id of the command associated with the request/response</summary>
		public TmxShareCommands Command
		{
			get 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
				{
					try
					{
						return TranslateCommand(m_tmxShare.Command);
					}
					catch
					{
						return TmxShareCommands.None;
					}
				}
				else
				{
					return TmxShareCommands.None;
				}
			}
			set 
			{ 
				if((m_tmxShare != null) && (m_tmxShare.IsDisposed == false))
					m_tmxShare.Command = TranslateCommand(value); 
			}
			
		}
		
		#endregion Properties

	}// public class CTmxShare : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.ActiveX
