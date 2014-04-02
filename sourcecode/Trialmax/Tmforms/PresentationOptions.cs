using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form is used to set the Presentation application options</summary>
	public class CFPresentationOptions : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>TMSetup control</summary>
		private AxTM_SETUP6Lib.AxTMSetup6 m_tmxSetup;
		
		/// <summary>Collection of control versions</summary>
		private ArrayList m_aVersions = null;
		
		/// <summary>Name of the configuration file</summary>
		private string m_strFileSpec = "fti.ini";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFPresentationOptions()
		{
			// Initialize the child controls
			InitializeComponent();
		}

		/// <summary>This method is called to get the collection of ActiveX version information</summary>
		/// <returns>The collection of version information descriptors</returns>
		public ArrayList GetAxVersions(bool bIncludeTmaxPresentation)
		{
			//	Make sure the path to Presentation has been set
			if(bIncludeTmaxPresentation == true)
				SetPresentationFileSpec();
			
			//	Do we need to retrieve the version information?
			if(m_aVersions == null)
			{
				try
				{
					m_aVersions = new ArrayList();
					
					m_tmxSetup.EnumAxVersions();
					
				}
				catch
				{
				}
				
			}
			
			return m_aVersions;
			
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
				
				if(m_aVersions != null)
					m_aVersions.Clear();
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFPresentationOptions));
			this.m_tmxSetup = new AxTM_SETUP6Lib.AxTMSetup6();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.m_tmxSetup)).BeginInit();
			this.SuspendLayout();
			// 
			// m_tmxSetup
			// 
			this.m_tmxSetup.Enabled = true;
			this.m_tmxSetup.Location = new System.Drawing.Point(4, 4);
			this.m_tmxSetup.Name = "m_tmxSetup";
			this.m_tmxSetup.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_tmxSetup.OcxState")));
			this.m_tmxSetup.Size = new System.Drawing.Size(491, 392);
			this.m_tmxSetup.TabIndex = 0;
			this.m_tmxSetup.AxVersion += new AxTM_SETUP6Lib._DTMSetup6Events_AxVersionEventHandler(this.OnAxVersion);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(408, 393);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(324, 393);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// CFPresentationOptions
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(500, 422);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Controls.Add(this.m_tmxSetup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFPresentationOptions";
			this.Text = " Presentation Options";
			this.Load += new System.EventHandler(this.OnLoad);
			((System.ComponentModel.ISupportInitialize)(this.m_tmxSetup)).EndInit();
			this.ResumeLayout(false);

		}
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method handles events fired when the user clicks on OK</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			m_tmxSetup.Save();
			
			DialogResult = DialogResult.OK;
			this.Close();
		
		}// private void OnClickOK(object sender, System.EventArgs e)
		
		/// <summary>This method handles events fired by the TMSetup control when it is enumerating the control versions</summary>
		/// <param name="sender">The TMSetup control</param>
		/// <param name="e">Event arguments</param>
		private void OnAxVersion(object sender, AxTM_SETUP6Lib._DTMSetup6Events_AxVersionEvent e)
		{
			FTI.Shared.CBaseVersion tmaxVersion = null;
			
			//	Add a new version to the collection
			if(m_aVersions != null)
			{
				tmaxVersion = new CBaseVersion();
				tmaxVersion.Major = e.sMajorVer;
				tmaxVersion.Minor = e.sMinorVer;
				tmaxVersion.QEF = e.sQEF;
				tmaxVersion.Build = e.sBuild;
				tmaxVersion.Location = e.lpszPath;
				tmaxVersion.Description = e.lpszDescription;
				tmaxVersion.Title = e.lpszName;
				tmaxVersion.SetVersionText(true);
				
				m_aVersions.Add(tmaxVersion);
			}

		}// private void OnAxVersion(object sender, AxTM_SETUP6Lib._DTMSetup6Events_AxVersionEvent e)

		/// <summary>This method handles the forms Load event</summary>
		/// <param name="sender">The form object</param>
		/// <param name="e">The system event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			//	Initialize the properties
			m_tmxSetup.IniFile = m_strFileSpec;
			SetPresentationFileSpec();
			
			m_tmxSetup.Initialize();
		
		}// private void OnLoad(object sender, System.EventArgs e)
		
		/// <summary>This method sets the path to the TmaxPresentation application</summary>
		private void SetPresentationFileSpec()
		{
			string strFolder = "";
			
			if(m_tmxSetup == null) return;
			if(m_tmxSetup.IsDisposed == true) return;
			
			//	Get the folder for the Manager application
			strFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			if((strFolder.Length > 0) && (strFolder.EndsWith("\\") == false))
				strFolder += "\\";
				
			//	Assume Presentation is in the same folder
			m_tmxSetup.PresentationFileSpec = (strFolder + "tmaxPresentation.exe");
		
		}// private void SetPresentationFileSpec()
		
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
	
	}// public class CFPresentationOptions : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
