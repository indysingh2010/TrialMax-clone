using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form is used to resolve a MediaId conflict encountered at registration</summary>
	public class CFResolveConflict : CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Conflict text label</summary>
		private System.Windows.Forms.Label m_ctrlConflict;

		/// <summary>Conflict text label</summary>
		private System.Windows.Forms.Label m_ctrlConflictLabel;

		/// <summary>Static text control to display the source text</summary>
		private System.Windows.Forms.Label m_ctrlSource;
		
		/// <summary>Source path text label</summary>
		private System.Windows.Forms.Label m_ctrlSourceLabel;
		
		/// <summary>Resolution edit box</summary>
		private System.Windows.Forms.TextBox m_ctrlResolution;
		
		/// <summary>Resolution edit box label</summary>
		private System.Windows.Forms.Label m_ctrlResolutionLabel;
		
		/// <summary>Local member bound to MediaId property</summary>
		private bool m_bIsMediaId = true;
		
		/// <summary>Local member bound to Conflict property</summary>
		private string m_strConflict = "";
		
		/// <summary>Local member bound to Resolution property</summary>
		private string m_strResolution = "";
		
		/// <summary>Local member bound to Source property</summary>
		private string m_strSource = "";
		
		/// <summary>Resolve All button</summary>
		private System.Windows.Forms.Button m_ctrlResolveAll;
		
		/// <summary>Resolve button</summary>
		private System.Windows.Forms.Button m_ctrlResolve;
			
		/// <summary>Local member bound to Resolve property</summary>
		private bool m_bAutoResolve = false;
		
		/// <summary>Local member bound to ResolveAll property</summary>
		private bool m_bAutoResolveAll = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFResolveConflict()
		{
			InitializeComponent();
		}
		
		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>Clean up any resources being used</summary>
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_ctrlResolveAll = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlResolve = new System.Windows.Forms.Button();
			this.m_ctrlConflictLabel = new System.Windows.Forms.Label();
			this.m_ctrlSourceLabel = new System.Windows.Forms.Label();
			this.m_ctrlConflict = new System.Windows.Forms.Label();
			this.m_ctrlResolution = new System.Windows.Forms.TextBox();
			this.m_ctrlResolutionLabel = new System.Windows.Forms.Label();
			this.m_ctrlSource = new System.Windows.Forms.Label();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlResolveAll
			// 
			this.m_ctrlResolveAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlResolveAll.Location = new System.Drawing.Point(119, 172);
			this.m_ctrlResolveAll.Name = "m_ctrlResolveAll";
			this.m_ctrlResolveAll.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlResolveAll.TabIndex = 2;
			this.m_ctrlResolveAll.Text = "Auto &Resolve All";
			this.m_ctrlResolveAll.Click += new System.EventHandler(this.OnClickResolveAll);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.Location = new System.Drawing.Point(343, 172);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlOk.TabIndex = 4;
			this.m_ctrlOk.Text = "&Try This";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlResolve
			// 
			this.m_ctrlResolve.Location = new System.Drawing.Point(7, 172);
			this.m_ctrlResolve.Name = "m_ctrlResolve";
			this.m_ctrlResolve.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlResolve.TabIndex = 1;
			this.m_ctrlResolve.Text = "&Auto Resolve";
			this.m_ctrlResolve.Click += new System.EventHandler(this.OnClickResolve);
			// 
			// m_ctrlConflictLabel
			// 
			this.m_ctrlConflictLabel.Location = new System.Drawing.Point(4, 12);
			this.m_ctrlConflictLabel.Name = "m_ctrlConflictLabel";
			this.m_ctrlConflictLabel.Size = new System.Drawing.Size(444, 16);
			this.m_ctrlConflictLabel.TabIndex = 4;
			this.m_ctrlConflictLabel.Text = "Unable to assign:";
			// 
			// m_ctrlSourceLabel
			// 
			this.m_ctrlSourceLabel.Location = new System.Drawing.Point(4, 60);
			this.m_ctrlSourceLabel.Name = "m_ctrlSourceLabel";
			this.m_ctrlSourceLabel.Size = new System.Drawing.Size(444, 16);
			this.m_ctrlSourceLabel.TabIndex = 6;
			this.m_ctrlSourceLabel.Text = "as MediaId for:";
			// 
			// m_ctrlConflict
			// 
			this.m_ctrlConflict.Location = new System.Drawing.Point(4, 32);
			this.m_ctrlConflict.Name = "m_ctrlConflict";
			this.m_ctrlConflict.Size = new System.Drawing.Size(444, 16);
			this.m_ctrlConflict.TabIndex = 5;
			this.m_ctrlConflict.Text = "duplicate id goes here";
			// 
			// m_ctrlResolution
			// 
			this.m_ctrlResolution.Location = new System.Drawing.Point(4, 136);
			this.m_ctrlResolution.Name = "m_ctrlResolution";
			this.m_ctrlResolution.Size = new System.Drawing.Size(444, 20);
			this.m_ctrlResolution.TabIndex = 0;
			this.m_ctrlResolution.Text = "m_ctrlResolution";
			// 
			// m_ctrlResolutionLabel
			// 
			this.m_ctrlResolutionLabel.Location = new System.Drawing.Point(4, 116);
			this.m_ctrlResolutionLabel.Name = "m_ctrlResolutionLabel";
			this.m_ctrlResolutionLabel.Size = new System.Drawing.Size(444, 16);
			this.m_ctrlResolutionLabel.TabIndex = 8;
			this.m_ctrlResolutionLabel.Text = "Enter a unique id:";
			// 
			// m_ctrlSource
			// 
			this.m_ctrlSource.Location = new System.Drawing.Point(4, 80);
			this.m_ctrlSource.Name = "m_ctrlSource";
			this.m_ctrlSource.Size = new System.Drawing.Size(444, 28);
			this.m_ctrlSource.TabIndex = 7;
			this.m_ctrlSource.Text = "source path goes here";
			this.m_ctrlSource.UseMnemonic = false;
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Location = new System.Drawing.Point(231, 172);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(104, 23);
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "&Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// CFResolveConflict
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlResolveAll;
			this.ClientSize = new System.Drawing.Size(454, 203);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlSource);
			this.Controls.Add(this.m_ctrlResolutionLabel);
			this.Controls.Add(this.m_ctrlResolution);
			this.Controls.Add(this.m_ctrlConflict);
			this.Controls.Add(this.m_ctrlSourceLabel);
			this.Controls.Add(this.m_ctrlConflictLabel);
			this.Controls.Add(this.m_ctrlResolve);
			this.Controls.Add(this.m_ctrlResolveAll);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CFResolveConflict";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Registration Conflict !";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>This method handles the Load event fired when the form is being shown for the first time</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event argument object</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			m_ctrlResolution.Text = m_strResolution;
			m_ctrlConflict.Text = m_strConflict;
			m_ctrlSource.Text = CTmaxToolbox.FitPathToWidth(m_strSource, m_ctrlSource);
			
			//	Set the appropriate labels
			if(m_bIsMediaId == true)
			{
				m_ctrlSourceLabel.Text = "as Media ID for:";
				m_ctrlResolutionLabel.Text = "Enter a unique Media ID for the new record:";
			}
			else
			{
				m_ctrlSourceLabel.Text = "as Media Folder for:";
				m_ctrlResolutionLabel.Text = "Enter a unique folder name for the new media:";
			}
			
			//	Set focus to the resolution
			this.ActiveControl = m_ctrlResolution;
			m_ctrlResolution.Focus();
			m_ctrlResolution.SelectAll();

		}// protected void OnLoad(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on Resolve</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnClickResolve(object sender, System.EventArgs e)
		{
			m_bAutoResolve = true;
			DialogResult = DialogResult.OK;
			Close();
		}
		
		/// <summary>Called when the user clicks on Resolve All</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnClickResolveAll(object sender, System.EventArgs e)
		{
			m_bAutoResolveAll = true;
			DialogResult = DialogResult.OK;
			Close();
		}
		
		/// <summary>Called when the user clicks on Ok</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			//	Get the text specified by the user
			m_strResolution = m_ctrlResolution.Text;
			
			//	Must provide new text
			if(m_strResolution.Length > 0)
			{
				//	Make sure the text is different
				if(String.Compare(m_strResolution, m_strConflict, true) != 0)
				{
					//	Give it a try
					DialogResult = DialogResult.OK;
					Close();
				}
				else
				{
					if(m_bIsMediaId == true)
						Warn("You must provide a unique Media ID", m_ctrlResolution);
					else
						Warn("You must provide a unique folder name", m_ctrlResolution);
				}
				
			}
			else
			{
				Warn("You must provide the text for resolving the conflict", m_ctrlResolution);
			}
		
		}// protected void OnClickOk(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on Cancel</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion Protected Methods

		#region Properties
		
		/// <summary>True for MediaId conflict, False for Target Folder conflict</summary>
		public bool IsMediaId
		{
			get { return m_bIsMediaId; }
			set { m_bIsMediaId = value; }
		}
		
		/// <summary>The conflicting text</summary>
		public string Conflict
		{
			get { return m_strConflict; }
			set { m_strConflict = value; }
		}
		
		/// <summary>The resolution specified by the user</summary>
		public string Resolution
		{
			get { return m_strResolution; }
			set { m_strResolution = value; }
		}
		
		/// <summary>The text used to identify the source</summary>
		public string Source
		{
			get { return m_strSource; }
			set { m_strSource = value; }
		}
		
		/// <summary>True to perform automatic resolution of this conflict</summary>
		public bool AutoResolve
		{
			get { return m_bAutoResolve; }
		}
		
		/// <summary>True to perform automatic resolution of this and all subsequent conflicts</summary>
		public bool AutoResolveAll
		{
			get { return m_bAutoResolveAll; }
		}
		
		#endregion Public Properties
		
	}// public class CFResolveConflict : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Database
