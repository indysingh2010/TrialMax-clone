using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace FTI.Trialmax.ActiveX
{
	/// <summary>
	/// This class serves as a wrapper for the Trialmax Tmpower ActiveX control
	/// </summary>
	public class CTmxPower : FTI.Trialmax.ActiveX.CTmxBase
	{
		#region Private Members
		
		/// <summary>Standard framework component container</summary>
		protected System.ComponentModel.IContainer components;
		private AxTM_POWER6Lib.AxTMPower6 m_ctrlTmpower;
		
		/// <summary>Background panel to act as container for child controls</summary>
		private System.Windows.Forms.Panel m_ctrlFillPanel;
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmxPower));
			this.m_ctrlFillPanel = new System.Windows.Forms.Panel();
			this.m_ctrlTmpower = new AxTM_POWER6Lib.AxTMPower6();
			this.m_ctrlFillPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTmpower)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlFillPanel
			// 
			this.m_ctrlFillPanel.Controls.Add(this.m_ctrlTmpower);
			this.m_ctrlFillPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_ctrlFillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlFillPanel.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlFillPanel.Name = "m_ctrlFillPanel";
			this.m_ctrlFillPanel.Size = new System.Drawing.Size(276, 150);
			this.m_ctrlFillPanel.TabIndex = 0;
			// 
			// m_ctrlTmpower
			// 
			this.m_ctrlTmpower.ContainingControl = this;
			this.m_ctrlTmpower.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlTmpower.Enabled = true;
			this.m_ctrlTmpower.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlTmpower.Name = "m_ctrlTmpower";
			this.m_ctrlTmpower.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_ctrlTmpower.OcxState")));
			this.m_ctrlTmpower.Size = new System.Drawing.Size(276, 150);
			this.m_ctrlTmpower.TabIndex = 0;
			this.m_ctrlTmpower.ViewFocus += new AxTM_POWER6Lib._DTMPower6Events_ViewFocusEventHandler(this.OnAxViewFocus);
			this.m_ctrlTmpower.AxError += new AxTM_POWER6Lib._DTMPower6Events_AxErrorEventHandler(this.OnAxError);
			this.m_ctrlTmpower.AxDiagnostic += new AxTM_POWER6Lib._DTMPower6Events_AxDiagnosticEventHandler(this.OnAxDiagnostic);
			// 
			// CTmxPower
			// 
			this.Controls.Add(this.m_ctrlFillPanel);
			this.Name = "CTmxPower";
			this.Size = new System.Drawing.Size(276, 150);
			this.m_ctrlFillPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTmpower)).EndInit();
			this.ResumeLayout(false);

		}
		
		#endregion Private Methods

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmxPower() : base()
		{
			m_strDescription = "TrialMax PowerPoint Viewer (tmpower)";
			m_tmaxEventSource.Name = m_strDescription;
			
			//	Initialize the child components
			InitializeComponent();
		}
		
		/// <summary>
		/// This method is called to perform the ActiveX initialization of the control
		/// </summary>
		/// <returns>true if successful</returns>
		public override bool AxInitialize()
		{
			//	Initialize the Tmpower control
			if(m_ctrlTmpower != null)
			{
				m_sAxError = m_ctrlTmpower.Initialize();
				m_bInitialized = m_ctrlTmpower.IsInitialized();
			}
			else
			{
				m_sAxError = -1;
				m_bInitialized = false;
			}
			
			if(m_bInitialized == true)
				FireStateChange(TmxStates.Unloaded);
				
			return m_bInitialized;
		}
		
		/// <summary>Called to shut down the ActiveX interfaces</summary>
		public override void AxTerminate()
		{
			base.AxTerminate();
			
			if(m_ctrlTmpower != null)
				m_ctrlTmpower.Close();
		}
		
		/// <summary>
		/// This method is called to determine if the specified file is viewable
		/// by the derived control
		/// </summary>
		/// <param name="strFilename">Name of file to be viewed if possible</param>
		/// <returns>true if viewable</returns>
		public override bool IsViewable(string strFilename)
		{
			string strExtension = System.IO.Path.GetExtension(strFilename);
			return CTmxPower.CheckExtension(strExtension);
		}
		
		/// <summary>This function is called to determine if files with the specified extension can be rendered by this control</summary>
		/// <param name="strExtension">File extension to be checked</param>
		/// <returns>true if viewable</returns>
		public static bool CheckExtension(string strExtension)
		{
			if((strExtension != null) && (strExtension.Length > 0))
			{
				//	Strip the leading period if found
				if(strExtension.StartsWith("."))
					strExtension = strExtension.Remove(0,1);
					
				switch(strExtension.ToLower())
				{
					case "ppt":
					case "pps":
					
						return true;
					
					default:
					
						break;
				}
				
			}
			
			return false;

		}
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="lStart">The slide to be viewed</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public override bool View(string strFilename, long lStart)
		{
			//	Are we actually unloading?
			if((strFilename == null) || (strFilename.Length == 0))
			{
				Unload();
				return true;
			}
			
			//	Do we have a valid control?
			if((m_ctrlTmpower == null) || (m_bInitialized == false)) return false;
			
			m_sAxError = m_ctrlTmpower.LoadFile(strFilename, (int)lStart, 0, -1);
			
			if(m_sAxError == 0)
			{
				m_strFilename = m_ctrlTmpower.GetFilename(-1);
				
				FireStateChange(TmxStates.Loaded);
				
				return true;
			}
			else
			{
				return false;
			}
				
		}//	Load(string strFilename)
		
		/// <summary>This method is called to unload the viewer</summary>
		public override void Unload()
		{
			//	Do we have a valid control?
			if((m_ctrlTmpower != null) && (m_bInitialized == true))
			{
				m_ctrlTmpower.Unload(-1);
			}
			m_strFilename = "";

			if(m_bInitialized == true)
				FireStateChange(TmxStates.Unloaded);
				
		}//	Unload()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		/// <param name="disposing">true if disposing of the object</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>This method is called to get the object's component container</summary>
		/// <returns>The viewer's component container</returns>
		protected override System.ComponentModel.IContainer GetComponentContainer()
		{
			return components;
		}

		#endregion Protected Methods

		#region Private Methods
		
		private void OnAxViewFocus(object sender, AxTM_POWER6Lib._DTMPower6Events_ViewFocusEvent e)
		{
			if(this.ParentForm != null)
				this.ParentForm.Select();
			else
				this.Select();
		}
		
		/// <summary>This method traps AxError events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxError(object sender, AxTM_POWER6Lib._DTMPower6Events_AxErrorEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireError(this, "TMPower", e.lpszMessage);	
		}

		/// <summary>This method traps AxDiagnostic events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxDiagnostic(object sender, AxTM_POWER6Lib._DTMPower6Events_AxDiagnosticEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireDiagnostic(this, e.lpszMethod, e.lpszMessage);	
		}
	
		#endregion Private Methods
		
		
	}//	class CTmxPower

}// namespace FTI.Trialmax.ActiveX
