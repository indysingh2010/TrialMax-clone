using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFImportCodesDatabase : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 1;
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		private System.Windows.Forms.Label m_ctrlWarningLine2;
		private System.Windows.Forms.Label m_ctrlWarningLabel;
		private System.Windows.Forms.Label m_ctrlWarningLine1;
		
		/// <summary>Local member bound to ImportOptions property</summary>
		private CTmaxImportOptions m_tmaxImportOptions = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFImportCodesDatabase()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFImportCodesDatabase()

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
			//	Initialize all the child controls
			m_ctrlOk.Enabled = Exchange(false);
			
			base.OnLoad(e);
			
		}// protected override void OnLoad(EventArgs e)
		
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
				}
				else
				{
				}
				
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
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlWarningLine2 = new System.Windows.Forms.Label();
			this.m_ctrlWarningLabel = new System.Windows.Forms.Label();
			this.m_ctrlWarningLine1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(368, 48);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(280, 48);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 2;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlWarningLine2
			// 
			this.m_ctrlWarningLine2.BackColor = System.Drawing.Color.Red;
			this.m_ctrlWarningLine2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlWarningLine2.ForeColor = System.Drawing.Color.White;
			this.m_ctrlWarningLine2.Location = new System.Drawing.Point(96, 24);
			this.m_ctrlWarningLine2.Name = "m_ctrlWarningLine2";
			this.m_ctrlWarningLine2.Size = new System.Drawing.Size(360, 16);
			this.m_ctrlWarningLine2.TabIndex = 32;
			this.m_ctrlWarningLine2.Text = "with the contents of the selected database !";
			this.m_ctrlWarningLine2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlWarningLabel
			// 
			this.m_ctrlWarningLabel.BackColor = System.Drawing.Color.Red;
			this.m_ctrlWarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlWarningLabel.ForeColor = System.Drawing.Color.White;
			this.m_ctrlWarningLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlWarningLabel.Name = "m_ctrlWarningLabel";
			this.m_ctrlWarningLabel.Size = new System.Drawing.Size(88, 32);
			this.m_ctrlWarningLabel.TabIndex = 31;
			this.m_ctrlWarningLabel.Text = "WARNING !";
			this.m_ctrlWarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlWarningLine1
			// 
			this.m_ctrlWarningLine1.BackColor = System.Drawing.Color.Red;
			this.m_ctrlWarningLine1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlWarningLine1.ForeColor = System.Drawing.Color.White;
			this.m_ctrlWarningLine1.Location = new System.Drawing.Point(96, 8);
			this.m_ctrlWarningLine1.Name = "m_ctrlWarningLine1";
			this.m_ctrlWarningLine1.Size = new System.Drawing.Size(360, 16);
			this.m_ctrlWarningLine1.TabIndex = 30;
			this.m_ctrlWarningLine1.Text = "This operation will replace all existing fielded data ";
			this.m_ctrlWarningLine1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CFImportCodesDatabase
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(466, 79);
			this.Controls.Add(this.m_ctrlWarningLine2);
			this.Controls.Add(this.m_ctrlWarningLabel);
			this.Controls.Add(this.m_ctrlWarningLine1);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFImportCodesDatabase";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Import Fielded Data";
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
	
	}// public class CFImportCodesDatabase : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
