using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Shared.Win32;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFImportBinders : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 1;
		
		private const long WS_POPUP = 0x80000000;
		private const long TTS_BALLOON = 0x40;
		private const long TTS_NOFADE = 0x20;
		private const int GWL_STYLE = -16;
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Text box to supply comment characters</summary>
		private System.Windows.Forms.TextBox m_ctrlCommentCharacters;
		
		/// <summary>Static text label for comment characters text box</summary>
		private System.Windows.Forms.Label m_ctrlCommentCharactersLabel;

		/// <summary>The form's tool tip control</summary>
		private System.Windows.Forms.ToolTip m_ctrlToolTip;

		/// <summary>Group box for file processing controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlProcessingGroup;

		/// <summary>Group box for record management controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlRecordsGroup;

		/// <summary>Check box to request application of registration options</summary>
		private System.Windows.Forms.CheckBox m_ctrlUseRegistrationOptions;

		/// <summary>Local member bound to ImportOptions property</summary>
		private CTmaxImportOptions m_tmaxImportOptions = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFImportBinders()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFImportBinders()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the form's properties: SetMembers = %1");
		
		}// protected override void SetErrorStrings()

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
		
		}// protected override void Dispose( bool disposing )
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			bool bSuccessful = false;
			
			//	Initialize all the child controls
			while(bSuccessful == false)
			{
				if(Exchange(false) == false)
					break;
					
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			if(bSuccessful == false)
			{
				m_ctrlOk.Enabled = false;
				m_ctrlCommentCharacters.Enabled = false;
				m_ctrlCommentCharactersLabel.Enabled = false;
			}
			else
			{
				SetBalloonStyle(m_ctrlToolTip);
				
				m_ctrlToolTip.SetToolTip(m_ctrlCommentCharacters, "Ignore lines that start with these characters");
				m_ctrlToolTip.SetToolTip(m_ctrlCommentCharactersLabel, m_ctrlToolTip.GetToolTip(m_ctrlCommentCharacters));
				m_ctrlToolTip.SetToolTip(m_ctrlUseRegistrationOptions, "Apply registration options to MediaID values stored in the file");
			}
				
			base.OnLoad(e);
			
		}// private void OnLoad(object sender, System.EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetMembers)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					m_tmaxImportOptions.CommentCharacters = m_ctrlCommentCharacters.Text;
					m_tmaxImportOptions.UseRegistrationOptions = m_ctrlUseRegistrationOptions.Checked;
				}
				else
				{
					m_ctrlCommentCharacters.Text = m_tmaxImportOptions.CommentCharacters;
					m_ctrlUseRegistrationOptions.Checked = m_tmaxImportOptions.UseRegistrationOptions;
						
				}// if(bSetMembers == true)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetMembers), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool Exchange(bool bSetMembers)
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlCommentCharacters = new System.Windows.Forms.TextBox();
			this.m_ctrlCommentCharactersLabel = new System.Windows.Forms.Label();
			this.m_ctrlToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.m_ctrlProcessingGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlRecordsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlUseRegistrationOptions = new System.Windows.Forms.CheckBox();
			this.m_ctrlProcessingGroup.SuspendLayout();
			this.m_ctrlRecordsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(160, 160);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(75, 160);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlCommentCharacters
			// 
			this.m_ctrlCommentCharacters.Location = new System.Drawing.Point(148, 28);
			this.m_ctrlCommentCharacters.Name = "m_ctrlCommentCharacters";
			this.m_ctrlCommentCharacters.Size = new System.Drawing.Size(64, 20);
			this.m_ctrlCommentCharacters.TabIndex = 0;
			this.m_ctrlCommentCharacters.Text = "";
			// 
			// m_ctrlCommentCharactersLabel
			// 
			this.m_ctrlCommentCharactersLabel.Location = new System.Drawing.Point(12, 28);
			this.m_ctrlCommentCharactersLabel.Name = "m_ctrlCommentCharactersLabel";
			this.m_ctrlCommentCharactersLabel.Size = new System.Drawing.Size(132, 20);
			this.m_ctrlCommentCharactersLabel.TabIndex = 14;
			this.m_ctrlCommentCharactersLabel.Text = "Comment lines start with:";
			this.m_ctrlCommentCharactersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlProcessingGroup
			// 
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlCommentCharactersLabel);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlCommentCharacters);
			this.m_ctrlProcessingGroup.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlProcessingGroup.Name = "m_ctrlProcessingGroup";
			this.m_ctrlProcessingGroup.Size = new System.Drawing.Size(228, 63);
			this.m_ctrlProcessingGroup.TabIndex = 19;
			this.m_ctrlProcessingGroup.TabStop = false;
			this.m_ctrlProcessingGroup.Text = "File Processing";
			// 
			// m_ctrlRecordsGroup
			// 
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlUseRegistrationOptions);
			this.m_ctrlRecordsGroup.Location = new System.Drawing.Point(8, 80);
			this.m_ctrlRecordsGroup.Name = "m_ctrlRecordsGroup";
			this.m_ctrlRecordsGroup.Size = new System.Drawing.Size(228, 64);
			this.m_ctrlRecordsGroup.TabIndex = 20;
			this.m_ctrlRecordsGroup.TabStop = false;
			this.m_ctrlRecordsGroup.Text = "Records";
			// 
			// m_ctrlUseRegistrationOptions
			// 
			this.m_ctrlUseRegistrationOptions.Location = new System.Drawing.Point(8, 28);
			this.m_ctrlUseRegistrationOptions.Name = "m_ctrlUseRegistrationOptions";
			this.m_ctrlUseRegistrationOptions.Size = new System.Drawing.Size(216, 24);
			this.m_ctrlUseRegistrationOptions.TabIndex = 0;
			this.m_ctrlUseRegistrationOptions.Text = "Apply registration options to Media ID";
			// 
			// CFImportBinders
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(246, 190);
			this.Controls.Add(this.m_ctrlRecordsGroup);
			this.Controls.Add(this.m_ctrlProcessingGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFImportBinders";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Import Binder Options";
			this.m_ctrlProcessingGroup.ResumeLayout(false);
			this.m_ctrlRecordsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			//	Get the user settings
			Exchange(true);
				
			//	Close the form
			DialogResult = DialogResult.OK;
			this.Close();
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickCancel(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>The user defined export options</summary>
		public CTmaxImportOptions ImportOptions
		{
			get { return m_tmaxImportOptions;  }
			set { m_tmaxImportOptions = value; }
		}
		
		#endregion Properties
	
	}// public class CFImportBinders : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
