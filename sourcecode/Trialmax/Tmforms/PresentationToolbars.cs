using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form is used to set the Presentation toolbar options</summary>
	public class CFPresentationToolbars : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		private AxTM_BARS6Lib.AxTMBars6 m_tmxBars;
		
		/// <summary>Name of the configuration file</summary>
		private string m_strFileSpec = "fti.ini";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFPresentationToolbars()
		{
			// Initialize the child controls
			InitializeComponent();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
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
		
		}// protected override void Dispose( bool disposing )

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFPresentationToolbars));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_tmxBars = new AxTM_BARS6Lib.AxTMBars6();
			((System.ComponentModel.ISupportInitialize)(this.m_tmxBars)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(416, 368);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(324, 368);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_tmxBars
			// 
			this.m_tmxBars.Enabled = true;
			this.m_tmxBars.Location = new System.Drawing.Point(4, 4);
			this.m_tmxBars.Name = "m_tmxBars";
			this.m_tmxBars.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_tmxBars.OcxState")));
			this.m_tmxBars.Size = new System.Drawing.Size(501, 364);
			this.m_tmxBars.TabIndex = 5;
			// 
			// CFPresentationToolbars
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(506, 399);
			this.Controls.Add(this.m_tmxBars);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFPresentationToolbars";
			this.Text = " Presentation Toolbars";
			this.Load += new System.EventHandler(this.OnLoad);
			((System.ComponentModel.ISupportInitialize)(this.m_tmxBars)).EndInit();
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method handles events fired when the user clicks on OK</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			m_tmxBars.Save();
			
			DialogResult = DialogResult.OK;
			this.Close();
		
		}// private void OnClickOK(object sender, System.EventArgs e)
		
		/// <summary>This method handles the forms Load event</summary>
		/// <param name="sender">The form object</param>
		/// <param name="e">The system event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			//	Initialize the properties
			m_tmxBars.IniFile = m_strFileSpec;
			m_tmxBars.Initialize();
		
		}// private void OnLoad(object sender, System.EventArgs e)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Fully qualified path to the configuration file</summary>
		/// </summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set { m_strFileSpec = value; }
		}
		
		#endregion Properties
	
	}// public class CFPresentationToolbars : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
