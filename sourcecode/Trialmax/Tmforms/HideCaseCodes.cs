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
	public class CFHideCaseCodes : CFTmaxBaseForm
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
		private System.Windows.Forms.Button m_ctrlShowAll;
		private System.Windows.Forms.CheckedListBox m_ctrlCaseCodes;
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFHideCaseCodes()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFHideCaseCodes()

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
					if(this.CaseCodes != null)
					{
						for(int i = 0; i < m_ctrlCaseCodes.Items.Count; i++)
							((CTmaxCaseCode)(m_ctrlCaseCodes.Items[i])).Hidden = (m_ctrlCaseCodes.GetItemChecked(i) == false);
					}

				}
				else
				{
					if(this.CaseCodes != null)
					{
						foreach(CTmaxCaseCode O in this.CaseCodes)
							m_ctrlCaseCodes.Items.Add(O, !O.Hidden);
					}
						
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFHideCaseCodes));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlCaseCodes = new System.Windows.Forms.CheckedListBox();
			this.m_ctrlShowAll = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(206, 200);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(120, 200);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 2;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlCaseCodes
			// 
			this.m_ctrlCaseCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseCodes.CheckOnClick = true;
			this.m_ctrlCaseCodes.IntegralHeight = false;
			this.m_ctrlCaseCodes.Location = new System.Drawing.Point(9, 8);
			this.m_ctrlCaseCodes.Name = "m_ctrlCaseCodes";
			this.m_ctrlCaseCodes.Size = new System.Drawing.Size(272, 184);
			this.m_ctrlCaseCodes.TabIndex = 35;
			// 
			// m_ctrlShowAll
			// 
			this.m_ctrlShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlShowAll.Location = new System.Drawing.Point(9, 200);
			this.m_ctrlShowAll.Name = "m_ctrlShowAll";
			this.m_ctrlShowAll.TabIndex = 36;
			this.m_ctrlShowAll.Text = "&Show All";
			this.m_ctrlShowAll.Click += new System.EventHandler(this.OnClickShowAll);
			// 
			// CFHideCaseCodes
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(290, 231);
			this.Controls.Add(this.m_ctrlShowAll);
			this.Controls.Add(this.m_ctrlCaseCodes);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFHideCaseCodes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Show / Hide Data Fields";
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

		/// <summary>This method is called when the user clicks on ShowAll</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickShowAll(object sender, System.EventArgs e)
		{
			if((this.CaseCodes != null) && (m_ctrlCaseCodes.Items != null))
			{
				for(int i = 0; i < m_ctrlCaseCodes.Items.Count; i++)
					m_ctrlCaseCodes.SetItemChecked(i, true);
			}
		
		}// private void OnClickCancel(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>The application's collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes;  }
			set { m_tmaxCaseCodes = value; }
		}
		
		#endregion Properties
	
	}// public class CFHideCaseCodes : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
