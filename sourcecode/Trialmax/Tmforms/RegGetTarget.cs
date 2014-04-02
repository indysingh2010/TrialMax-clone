using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>Form used to prompt the user for the target folder for registration</summary>
	public class CFRegGetTarget : CFTmaxBaseForm
	{
		#region Private Members

		/// <summary>Form designer component container</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Control to display user prompt</summary>
		private System.Windows.Forms.Label m_ctrlMessage;

		/// <summary>Local member bound to MediaType property</summary>
		private TmaxMediaTypes m_eMediaType = TmaxMediaTypes.Unknown;
		
		/// <summary>Local member bound to Target property</summary>
		private string m_strTarget = "";
		
		/// <summary>Local member bound to UserOverride property</summary>
		private string m_strUserOverride = "";
		
		/// <summary>Local member bound to QueryTarget property</summary>
		private string m_strQueryTarget = "";
		
		/// <summary>Local member bound to QueryMediaId property</summary>
		private string m_strQueryMediaId = "";
		
		/// <summary>Local member bound to Abort property</summary>
		private bool m_bAbort = false;
		
		/// <summary>Group box control for response options</summary>
		private System.Windows.Forms.GroupBox m_ctrlOptionsGroup;
		
		/// <summary>Radio button control to request replacement of existing files</summary>
		private System.Windows.Forms.RadioButton m_ctrlReplace;
		
		/// <summary>Radio button control to request replacement of existing files</summary>
		private System.Windows.Forms.RadioButton m_ctrlOverride;
		
		/// <summary>Text control used to specify an override target name</summary>
		private System.Windows.Forms.RadioButton m_ctrlSkip;
		
		/// <summary>Radio button control to abort the registration</summary>
		private System.Windows.Forms.RadioButton m_ctrlAbort;
		
		/// <summary>Text label for the Replace radio button</summary>
		private System.Windows.Forms.Label m_ctrlReplaceLabel;
		
		/// <summary>Text label for the Override radio button</summary>
		private System.Windows.Forms.Label m_ctrlOverrideLabel;
		
		/// <summary>Text label for the Skip radio button</summary>
		private System.Windows.Forms.Label m_ctrlSkipLabel;
		
		/// <summary>Text label for the Abort radio button</summary>
		private System.Windows.Forms.Label m_ctrlAbortLabel;
		
		/// <summary>Text label for the Override name text box</summary>
		private System.Windows.Forms.Label m_ctrlOverrideNameLabel;
		
		/// <summary>Edit box for the Override name to be used</summary>
		private System.Windows.Forms.TextBox m_ctrlOverrideName;
		
		/// <summary>Text label to display the media id of the target's owner</summary>
		private System.Windows.Forms.Label m_ctrlMediaId;
		
		/// <summary>Text label to display the target path</summary>
		private System.Windows.Forms.Label m_ctrlTarget;
		
		/// <summary>Label for the target path control</summary>
		private System.Windows.Forms.Label m_ctrlTargetLabel;
		
		/// <summary>Label for the target path owner control</summary>
		private System.Windows.Forms.Label m_ctrlOwnerLabel;
		
		/// <summary>Local member bound to IsFolder property</summary>
		private bool m_bIsFolder = false;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This event is fired to check the validity of an overridden registration target</summary>
		public event System.EventHandler QueryRegTarget;
		
		/// <summary>Constructor</summary>
		public CFRegGetTarget() : base()
		{
			// Required for Windows Form Designer support
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

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFRegGetTarget));
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlMessage = new System.Windows.Forms.Label();
			this.m_ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlOverrideName = new System.Windows.Forms.TextBox();
			this.m_ctrlOverrideNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlAbortLabel = new System.Windows.Forms.Label();
			this.m_ctrlSkipLabel = new System.Windows.Forms.Label();
			this.m_ctrlOverrideLabel = new System.Windows.Forms.Label();
			this.m_ctrlReplaceLabel = new System.Windows.Forms.Label();
			this.m_ctrlAbort = new System.Windows.Forms.RadioButton();
			this.m_ctrlSkip = new System.Windows.Forms.RadioButton();
			this.m_ctrlOverride = new System.Windows.Forms.RadioButton();
			this.m_ctrlReplace = new System.Windows.Forms.RadioButton();
			this.m_ctrlMediaId = new System.Windows.Forms.Label();
			this.m_ctrlTarget = new System.Windows.Forms.Label();
			this.m_ctrlTargetLabel = new System.Windows.Forms.Label();
			this.m_ctrlOwnerLabel = new System.Windows.Forms.Label();
			this.m_ctrlOptionsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(324, 304);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlMessage
			// 
			this.m_ctrlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMessage.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlMessage.Name = "m_ctrlMessage";
			this.m_ctrlMessage.Size = new System.Drawing.Size(388, 36);
			this.m_ctrlMessage.TabIndex = 8;
			// 
			// m_ctrlOptionsGroup
			// 
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlOverrideName);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlOverrideNameLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlAbortLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlSkipLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlOverrideLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlReplaceLabel);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlAbort);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlSkip);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlOverride);
			this.m_ctrlOptionsGroup.Controls.Add(this.m_ctrlReplace);
			this.m_ctrlOptionsGroup.Location = new System.Drawing.Point(8, 116);
			this.m_ctrlOptionsGroup.Name = "m_ctrlOptionsGroup";
			this.m_ctrlOptionsGroup.Size = new System.Drawing.Size(392, 180);
			this.m_ctrlOptionsGroup.TabIndex = 0;
			this.m_ctrlOptionsGroup.TabStop = false;
			this.m_ctrlOptionsGroup.Text = "Options";
			// 
			// m_ctrlOverrideName
			// 
			this.m_ctrlOverrideName.Location = new System.Drawing.Point(100, 148);
			this.m_ctrlOverrideName.Name = "m_ctrlOverrideName";
			this.m_ctrlOverrideName.Size = new System.Drawing.Size(284, 20);
			this.m_ctrlOverrideName.TabIndex = 9;
			this.m_ctrlOverrideName.Text = "";
			// 
			// m_ctrlOverrideNameLabel
			// 
			this.m_ctrlOverrideNameLabel.Location = new System.Drawing.Point(12, 148);
			this.m_ctrlOverrideNameLabel.Name = "m_ctrlOverrideNameLabel";
			this.m_ctrlOverrideNameLabel.Size = new System.Drawing.Size(88, 20);
			this.m_ctrlOverrideNameLabel.TabIndex = 8;
			this.m_ctrlOverrideNameLabel.Text = "Override With:";
			this.m_ctrlOverrideNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAbortLabel
			// 
			this.m_ctrlAbortLabel.Location = new System.Drawing.Point(92, 112);
			this.m_ctrlAbortLabel.Name = "m_ctrlAbortLabel";
			this.m_ctrlAbortLabel.Size = new System.Drawing.Size(292, 23);
			this.m_ctrlAbortLabel.TabIndex = 7;
			this.m_ctrlAbortLabel.Text = "Abort the registration operation";
			this.m_ctrlAbortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlSkipLabel
			// 
			this.m_ctrlSkipLabel.Location = new System.Drawing.Point(92, 84);
			this.m_ctrlSkipLabel.Name = "m_ctrlSkipLabel";
			this.m_ctrlSkipLabel.Size = new System.Drawing.Size(292, 23);
			this.m_ctrlSkipLabel.TabIndex = 6;
			this.m_ctrlSkipLabel.Text = "Skip registration of this record";
			this.m_ctrlSkipLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlOverrideLabel
			// 
			this.m_ctrlOverrideLabel.Location = new System.Drawing.Point(92, 56);
			this.m_ctrlOverrideLabel.Name = "m_ctrlOverrideLabel";
			this.m_ctrlOverrideLabel.Size = new System.Drawing.Size(292, 23);
			this.m_ctrlOverrideLabel.TabIndex = 5;
			this.m_ctrlOverrideLabel.Text = "Override default target name with the name I specify";
			this.m_ctrlOverrideLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlReplaceLabel
			// 
			this.m_ctrlReplaceLabel.Location = new System.Drawing.Point(92, 28);
			this.m_ctrlReplaceLabel.Name = "m_ctrlReplaceLabel";
			this.m_ctrlReplaceLabel.Size = new System.Drawing.Size(292, 23);
			this.m_ctrlReplaceLabel.TabIndex = 4;
			this.m_ctrlReplaceLabel.Text = "Replace existing source files with these source files";
			this.m_ctrlReplaceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAbort
			// 
			this.m_ctrlAbort.Location = new System.Drawing.Point(12, 112);
			this.m_ctrlAbort.Name = "m_ctrlAbort";
			this.m_ctrlAbort.Size = new System.Drawing.Size(80, 24);
			this.m_ctrlAbort.TabIndex = 3;
			this.m_ctrlAbort.Text = "Abort";
			this.m_ctrlAbort.Click += new System.EventHandler(this.OnClickOption);
			// 
			// m_ctrlSkip
			// 
			this.m_ctrlSkip.Location = new System.Drawing.Point(12, 84);
			this.m_ctrlSkip.Name = "m_ctrlSkip";
			this.m_ctrlSkip.Size = new System.Drawing.Size(80, 24);
			this.m_ctrlSkip.TabIndex = 2;
			this.m_ctrlSkip.Text = "Skip";
			this.m_ctrlSkip.Click += new System.EventHandler(this.OnClickOption);
			// 
			// m_ctrlOverride
			// 
			this.m_ctrlOverride.Location = new System.Drawing.Point(12, 56);
			this.m_ctrlOverride.Name = "m_ctrlOverride";
			this.m_ctrlOverride.Size = new System.Drawing.Size(80, 24);
			this.m_ctrlOverride.TabIndex = 1;
			this.m_ctrlOverride.Text = "Override";
			this.m_ctrlOverride.Click += new System.EventHandler(this.OnClickOption);
			// 
			// m_ctrlReplace
			// 
			this.m_ctrlReplace.Location = new System.Drawing.Point(12, 28);
			this.m_ctrlReplace.Name = "m_ctrlReplace";
			this.m_ctrlReplace.Size = new System.Drawing.Size(80, 24);
			this.m_ctrlReplace.TabIndex = 0;
			this.m_ctrlReplace.Text = "Replace";
			this.m_ctrlReplace.Click += new System.EventHandler(this.OnClickOption);
			// 
			// m_ctrlMediaId
			// 
			this.m_ctrlMediaId.Location = new System.Drawing.Point(72, 80);
			this.m_ctrlMediaId.Name = "m_ctrlMediaId";
			this.m_ctrlMediaId.Size = new System.Drawing.Size(324, 23);
			this.m_ctrlMediaId.TabIndex = 11;
			this.m_ctrlMediaId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTarget
			// 
			this.m_ctrlTarget.Location = new System.Drawing.Point(72, 52);
			this.m_ctrlTarget.Name = "m_ctrlTarget";
			this.m_ctrlTarget.Size = new System.Drawing.Size(324, 23);
			this.m_ctrlTarget.TabIndex = 10;
			this.m_ctrlTarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTargetLabel
			// 
			this.m_ctrlTargetLabel.Location = new System.Drawing.Point(8, 52);
			this.m_ctrlTargetLabel.Name = "m_ctrlTargetLabel";
			this.m_ctrlTargetLabel.Size = new System.Drawing.Size(60, 23);
			this.m_ctrlTargetLabel.TabIndex = 12;
			this.m_ctrlTargetLabel.Text = "Target:";
			this.m_ctrlTargetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlOwnerLabel
			// 
			this.m_ctrlOwnerLabel.Location = new System.Drawing.Point(8, 80);
			this.m_ctrlOwnerLabel.Name = "m_ctrlOwnerLabel";
			this.m_ctrlOwnerLabel.Size = new System.Drawing.Size(60, 23);
			this.m_ctrlOwnerLabel.TabIndex = 13;
			this.m_ctrlOwnerLabel.Text = "Owner:";
			this.m_ctrlOwnerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFRegGetTarget
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(406, 335);
			this.Controls.Add(this.m_ctrlOwnerLabel);
			this.Controls.Add(this.m_ctrlTargetLabel);
			this.Controls.Add(this.m_ctrlOptionsGroup);
			this.Controls.Add(this.m_ctrlMessage);
			this.Controls.Add(this.m_ctrlOk);
			this.Controls.Add(this.m_ctrlMediaId);
			this.Controls.Add(this.m_ctrlTarget);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFRegGetTarget";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Registration Warning";
			this.Load += new System.EventHandler(this.OnLoad);
			this.m_ctrlOptionsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			//	Initialize the controls
			SetMessage();
			m_ctrlTarget.Text = CTmaxToolbox.FitPathToWidth(this.Target, m_ctrlTarget);
			m_ctrlReplace.Checked = true;
			OnClickOption(m_ctrlReplace, System.EventArgs.Empty);
		
		}// private void OnLoad(object sender, System.EventArgs e)
		
		/// <summary>This method handles events fired when the user clicks one of the Options radio buttons</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOption(object sender, System.EventArgs e)
		{
			m_ctrlOverrideName.Enabled = m_ctrlOverride.Checked;
			m_ctrlOverrideNameLabel.Enabled = m_ctrlOverride.Checked;
		
		}// private void OnClickOption(object sender, System.EventArgs e)
		
		/// <summary>This method formats the message that gets displayed when the form opens</summary>
		private void SetMessage()
		{
			string strMsg = "";
			
			Debug.Assert(m_ctrlMessage != null);
			if(m_ctrlMessage == null) return;
			Debug.Assert(m_ctrlMessage.IsDisposed == false);
			if(m_ctrlMessage.IsDisposed == true) return;
			
			if(this.IsFolder == true)
				strMsg = "The target folder for registering this " + this.MediaType.ToString() + " is already owned by a registered media record";
			else
				strMsg = "The target file for registering this " + this.MediaType.ToString() + " is already owned by a registered media record";
		
			m_ctrlMessage.Text = strMsg;
			
		}// private void SetMessage()
		
		/// <summary>Called to check the user override value</summary>
		/// <returns>True if the value is acceptable</returns>
		private bool CheckOverride()
		{
			bool bSuccessful = false;
			
			this.UserOverride = m_ctrlOverrideName.Text;
			
			//	Must have provided a value
			if(this.UserOverride.Length == 0)
			{
				MessageBox.Show("You must provide a name to override the default target", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			
			//	Should we query the owner?
			if(QueryRegTarget != null)
			{
				//	Clear the query media id
				this.QueryMediaId = "";
				
				if(SetQueryTarget() == true)
				{
					//	Fire the event
					QueryRegTarget(this, System.EventArgs.Empty);
					
					if(this.QueryMediaId.Length > 0)
					{
						string strMsg = String.Format("{0} is already registered by {1}", this.QueryTarget, this.QueryMediaId);
						MessageBox.Show(strMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						bSuccessful = true;
					}
					
				}// if(SetQueryTarget() == true)
				
			}// if(QueryRegTarget != null)
			
			return bSuccessful;
		
		}// private bool CheckOverride()
		
		/// <summary>Called to set the target path for the query event</summary>
		/// <returns>True if successful</returns>
		private bool SetQueryTarget()
		{
			int iIndex = 0;
			
			//	Is this a folder we are dealing with?
			if(this.IsFolder == true)
			{
				m_strQueryTarget = m_strTarget;
				if(m_strQueryTarget.EndsWith("\\") == true)
					m_strQueryTarget = m_strQueryTarget.Substring(0, m_strQueryTarget.Length - 1);
					
				if((iIndex = m_strQueryTarget.LastIndexOf("\\")) >= 0)
					m_strQueryTarget = m_strQueryTarget.Substring(0, iIndex + 1);
					
				m_strQueryTarget += this.UserOverride;
			}
			else
			{
				//	Strip the extension if set by the user
				if(System.IO.Path.HasExtension(this.UserOverride) == true)
				{
					this.UserOverride = System.IO.Path.GetFileNameWithoutExtension(this.UserOverride);
					m_ctrlOverrideName.Text = this.UserOverride;
				}
			
				m_strQueryTarget = System.IO.Path.GetDirectoryName(m_strTarget);
				if(m_strQueryTarget.EndsWith("\\") == false)
					m_strQueryTarget += "\\";
					
				m_strQueryTarget += this.UserOverride;
				
				if(System.IO.Path.HasExtension(m_strTarget) == true)
					m_strQueryTarget += System.IO.Path.GetExtension(m_strTarget);
			}
			
			return true;
			
		}
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			//	Which option has been selected
			if(m_ctrlReplace.Checked == true)
			{
				this.UserOverride = "";
				DialogResult = DialogResult.OK;
			}
			else if(m_ctrlOverride.Checked == true)
			{
				if(CheckOverride() == true)
				{
					DialogResult = DialogResult.OK;
				}
				else
				{
					m_ctrlOverrideName.Focus();
					return;
				}
				
			}
			else if(m_ctrlSkip.Checked == true)
			{
				this.Abort = false;
				DialogResult = DialogResult.Cancel;
			}
			else if(m_ctrlAbort.Checked == true)
			{
				this.Abort = true;
				DialogResult = DialogResult.Cancel;
			}
			else
			{
				MessageBox.Show("You must select one of the available options");
				return;
			}
					
			this.Close();
		
		}// private void OnClickOK(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>True if Target is a folder, False if it's a filename</summary>
		public bool IsFolder
		{
			get { return m_bIsFolder; }
			set { m_bIsFolder = value; }
		}
		
		/// <summary>True if the user wants to abort the registration</summary>
		public bool Abort
		{
			get { return m_bAbort; }
			set { m_bAbort = value; }
		}
		
		/// <summary>The target filename/folder</summary>
		public string Target
		{
			get { return m_strTarget; }
			set { m_strTarget = value; }
		}
		
		/// <summary>The target filename/folder provided for the QueryRegTarget event</summary>
		public string QueryTarget
		{
			get { return m_strQueryTarget; }
			set { m_strQueryTarget = value; }
		}
		
		/// <summary>MediaId set as response to a QueryRegTarget event</summary>
		public string QueryMediaId
		{
			get { return m_strQueryMediaId; }
			set { m_strQueryMediaId = value; }
		}
		
		/// <summary>The user specified name for overriding the default target name</summary>
		public string UserOverride
		{
			get { return m_strUserOverride; }
			set { m_strUserOverride = value; }
		}
		
		/// <summary>Type of media being registered</summary>
		public TmaxMediaTypes MediaType
		{
			get { return m_eMediaType; }
			set { m_eMediaType = value; }
		}
		
		#endregion Properties
		
	}// public class CFRegGetTarget : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
