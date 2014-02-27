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
	public class CFSetPropsVisible : CFTmaxBaseForm
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

		/// <summary>The form's Show All button</summary>
		private System.Windows.Forms.Button m_ctrlShowAll;

		/// <summary>The form's check list of properties</summary>
		private System.Windows.Forms.CheckedListBox m_ctrlProperties;

		/// <summary>Local member bound to Properties property</summary>
		private CTmaxProperties m_tmaxProperties = null;

		/// <summary>Local flag to indicate if changes were made</summary>
		private bool m_bModified = false;
		
		#endregion Private Members

		#region Public Methods

		public CFSetPropsVisible()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

		}// public CFSetPropsVisible()

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
		protected override void Dispose(bool disposing)
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
			bool	bSuccessful = false;
			int		iVisible = 0;

			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					if(this.Properties != null)
					{
						m_bModified = false;
						
						//	Update each of the properties
						for(int i = 0; i < m_ctrlProperties.Items.Count; i++)
						{
							if(m_tmaxProperties[i].Visible != m_ctrlProperties.GetItemChecked(i))
							{
								m_tmaxProperties[i].Visible = m_ctrlProperties.GetItemChecked(i);
								m_bModified = true;
							}
							
							if(m_tmaxProperties[i].Visible == true)
								iVisible += 1;

						}// for(int i = 0; i < m_ctrlProperties.Items.Count; i++)

						//	Must have at least one visible property
						if(iVisible == 0)
							return Warn("You must select at least one field to be visible.");
							
					}// if(this.Properties != null)

				}
				else
				{
					if(this.Properties != null)
					{
						foreach(CTmaxProperty O in this.Properties)
							m_ctrlProperties.Items.Add(O.Name, O.Visible);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFSetPropsVisible));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlProperties = new System.Windows.Forms.CheckedListBox();
			this.m_ctrlShowAll = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(182, 200);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(96, 200);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 2;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlProperties
			// 
			this.m_ctrlProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlProperties.CheckOnClick = true;
			this.m_ctrlProperties.IntegralHeight = false;
			this.m_ctrlProperties.Location = new System.Drawing.Point(9, 8);
			this.m_ctrlProperties.Name = "m_ctrlProperties";
			this.m_ctrlProperties.Size = new System.Drawing.Size(248, 184);
			this.m_ctrlProperties.TabIndex = 35;
			// 
			// m_ctrlShowAll
			// 
			this.m_ctrlShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlShowAll.Location = new System.Drawing.Point(9, 200);
			this.m_ctrlShowAll.Name = "m_ctrlShowAll";
			this.m_ctrlShowAll.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlShowAll.TabIndex = 36;
			this.m_ctrlShowAll.Text = "&Show All";
			this.m_ctrlShowAll.Click += new System.EventHandler(this.OnClickShowAll);
			// 
			// CFSetPropsVisible
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(266, 231);
			this.Controls.Add(this.m_ctrlShowAll);
			this.Controls.Add(this.m_ctrlProperties);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFSetPropsVisible";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Show / Hide Objection Fields";
			this.ResumeLayout(false);

		}

		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			//	Get the user settings
			if(Exchange(true) == true)
			{
				//	Close the form
				DialogResult = m_bModified ? DialogResult.OK : DialogResult.Cancel;
				this.Close();

			}// if(Exchange(true) == true)

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
			if((this.Properties != null) && (m_ctrlProperties.Items != null))
			{
				for(int i = 0; i < m_ctrlProperties.Items.Count; i++)
					m_ctrlProperties.SetItemChecked(i, true);
			}

		}// private void OnClickShowAll(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties

		/// <summary>The collection of properties displayed in the list</summary>
		public CTmaxProperties Properties
		{
			get { return m_tmaxProperties; }
			set { m_tmaxProperties = value; }
		}

		#endregion Properties

	}// public class CFSetPropsVisible : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
