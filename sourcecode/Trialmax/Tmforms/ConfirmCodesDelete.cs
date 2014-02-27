using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form is used to prompt the user before deleting a record</summary>
	public class CFConfirmCodesDelete : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Yes button</summary>
		private System.Windows.Forms.Button m_ctrlYes;

		/// <summary>No button</summary>
		private System.Windows.Forms.Button m_ctrlNo;

		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Static text control to display record name</summary>
		private System.Windows.Forms.Label m_ctrlName;

		/// <summary>Yes To All pushbutton</summary>
		private System.Windows.Forms.Button m_ctrlYesToAll;

		/// <summary>Static text label for the name</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;

		/// <summary>Static text to display the name of record being deleted</summary>
		private System.Windows.Forms.Label m_ctrlMessage;

		/// <summary>List of fielded data (codes) being deleted</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlCodes;

		/// <summary>List of case codes being deleted</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlCaseCodes;

		/// <summary>Static text label for case codes list</summary>
		private System.Windows.Forms.Label m_ctrlCaseCodesLabel;

		/// <summary>Static text label for fielded data list</summary>
		private System.Windows.Forms.Label m_ctrlCodesLabel;

		/// <summary>Private member bound to DeleteName property</summary>
		private string m_strDeleteName = "";
		
		/// <summary>Private member bound to ICodes property</summary>
		private IList m_ICodes = null;
		
		/// <summary>Private member bound to ICaseCodes property</summary>
		private IList m_ICaseCodes = null;
		
		/// <summary>Private member bound to CodesDisplayMode property</summary>
		private int m_iCodesDisplayMode = 0;
		
		/// <summary>Private member bound to CaseCodesDisplayMode property</summary>
		private int m_iCaseCodesDisplayMode = 0;
		private System.Windows.Forms.Label m_ctrlQuestion;
		
		/// <summary>Local member bound to YesToAll property</summary>
		private bool m_bYesToAll = false;

		#endregion Private Members
		
		#region Public Methods

		/// <summary>Default constructor</summary>
		public CFConfirmCodesDelete()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		/// <param name="disposing">true if disposing of the form</param>
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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		protected void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFConfirmCodesDelete));
			this.m_ctrlYes = new System.Windows.Forms.Button();
			this.m_ctrlNo = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlMessage = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.Label();
			this.m_ctrlYesToAll = new System.Windows.Forms.Button();
			this.m_ctrlCodes = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlCodesLabel = new System.Windows.Forms.Label();
			this.m_ctrlCaseCodesLabel = new System.Windows.Forms.Label();
			this.m_ctrlCaseCodes = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlQuestion = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlYes
			// 
			this.m_ctrlYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.m_ctrlYes.Location = new System.Drawing.Point(216, 272);
			this.m_ctrlYes.Name = "m_ctrlYes";
			this.m_ctrlYes.TabIndex = 0;
			this.m_ctrlYes.Text = "&Yes";
			// 
			// m_ctrlNo
			// 
			this.m_ctrlNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlNo.Location = new System.Drawing.Point(304, 272);
			this.m_ctrlNo.Name = "m_ctrlNo";
			this.m_ctrlNo.TabIndex = 1;
			this.m_ctrlNo.Text = "&No";
			this.m_ctrlNo.Click += new System.EventHandler(this.OnClickNo);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(88, 272);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.Text = "&Cancel";
			this.m_ctrlCancel.Visible = false;
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(7, 8);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(129, 12);
			this.m_ctrlNameLabel.TabIndex = 3;
			this.m_ctrlNameLabel.Text = "You are about to delete:";
			this.m_ctrlNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMessage
			// 
			this.m_ctrlMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMessage.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlMessage.Name = "m_ctrlMessage";
			this.m_ctrlMessage.Size = new System.Drawing.Size(378, 32);
			this.m_ctrlMessage.TabIndex = 5;
			this.m_ctrlMessage.Text = "Records shown in this list reference the one you are attempting to delete. If you" +
				" delete this record they will also be deleted.";
			this.m_ctrlMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlName.Location = new System.Drawing.Point(136, 8);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(248, 12);
			this.m_ctrlName.TabIndex = 9;
			this.m_ctrlName.Text = "name goes here";
			this.m_ctrlName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlYesToAll
			// 
			this.m_ctrlYesToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlYesToAll.Location = new System.Drawing.Point(16, 272);
			this.m_ctrlYesToAll.Name = "m_ctrlYesToAll";
			this.m_ctrlYesToAll.TabIndex = 13;
			this.m_ctrlYesToAll.Text = "Yes To &All";
			this.m_ctrlYesToAll.Visible = false;
			this.m_ctrlYesToAll.Click += new System.EventHandler(this.OnClickYesToAll);
			// 
			// m_ctrlCodes
			// 
			this.m_ctrlCodes.AddTop = false;
			this.m_ctrlCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCodes.AutoResizeColumns = false;
			this.m_ctrlCodes.ClearOnDblClick = false;
			this.m_ctrlCodes.DisplayMode = 0;
			this.m_ctrlCodes.HideSelection = true;
			this.m_ctrlCodes.Location = new System.Drawing.Point(8, 76);
			this.m_ctrlCodes.MaxRows = 0;
			this.m_ctrlCodes.Name = "m_ctrlCodes";
			this.m_ctrlCodes.OwnerImages = null;
			this.m_ctrlCodes.PaneId = 0;
			this.m_ctrlCodes.SelectedIndex = -1;
			this.m_ctrlCodes.ShowHeaders = true;
			this.m_ctrlCodes.ShowImage = false;
			this.m_ctrlCodes.Size = new System.Drawing.Size(376, 88);
			this.m_ctrlCodes.TabIndex = 14;
			// 
			// m_ctrlCodesLabel
			// 
			this.m_ctrlCodesLabel.Location = new System.Drawing.Point(8, 64);
			this.m_ctrlCodesLabel.Name = "m_ctrlCodesLabel";
			this.m_ctrlCodesLabel.Size = new System.Drawing.Size(129, 12);
			this.m_ctrlCodesLabel.TabIndex = 15;
			this.m_ctrlCodesLabel.Text = "Fielded Data:";
			this.m_ctrlCodesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCaseCodesLabel
			// 
			this.m_ctrlCaseCodesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlCaseCodesLabel.Location = new System.Drawing.Point(8, 168);
			this.m_ctrlCaseCodesLabel.Name = "m_ctrlCaseCodesLabel";
			this.m_ctrlCaseCodesLabel.Size = new System.Drawing.Size(129, 12);
			this.m_ctrlCaseCodesLabel.TabIndex = 17;
			this.m_ctrlCaseCodesLabel.Text = "Fields:";
			this.m_ctrlCaseCodesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCaseCodes
			// 
			this.m_ctrlCaseCodes.AddTop = false;
			this.m_ctrlCaseCodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseCodes.AutoResizeColumns = false;
			this.m_ctrlCaseCodes.ClearOnDblClick = false;
			this.m_ctrlCaseCodes.DisplayMode = 0;
			this.m_ctrlCaseCodes.HideSelection = true;
			this.m_ctrlCaseCodes.Location = new System.Drawing.Point(8, 180);
			this.m_ctrlCaseCodes.MaxRows = 0;
			this.m_ctrlCaseCodes.Name = "m_ctrlCaseCodes";
			this.m_ctrlCaseCodes.OwnerImages = null;
			this.m_ctrlCaseCodes.PaneId = 0;
			this.m_ctrlCaseCodes.SelectedIndex = -1;
			this.m_ctrlCaseCodes.ShowHeaders = true;
			this.m_ctrlCaseCodes.ShowImage = false;
			this.m_ctrlCaseCodes.Size = new System.Drawing.Size(376, 64);
			this.m_ctrlCaseCodes.TabIndex = 16;
			// 
			// m_ctrlQuestion
			// 
			this.m_ctrlQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlQuestion.Location = new System.Drawing.Point(8, 248);
			this.m_ctrlQuestion.Name = "m_ctrlQuestion";
			this.m_ctrlQuestion.Size = new System.Drawing.Size(352, 12);
			this.m_ctrlQuestion.TabIndex = 18;
			this.m_ctrlQuestion.Text = "Do you want to continue ?";
			this.m_ctrlQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// CFConfirmCodesDelete
			// 
			this.AcceptButton = this.m_ctrlYes;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(392, 301);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlQuestion);
			this.Controls.Add(this.m_ctrlCaseCodesLabel);
			this.Controls.Add(this.m_ctrlCaseCodes);
			this.Controls.Add(this.m_ctrlCodesLabel);
			this.Controls.Add(this.m_ctrlCodes);
			this.Controls.Add(this.m_ctrlYesToAll);
			this.Controls.Add(this.m_ctrlName);
			this.Controls.Add(this.m_ctrlMessage);
			this.Controls.Add(this.m_ctrlNameLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlNo);
			this.Controls.Add(this.m_ctrlYes);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFConfirmCodesDelete";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Confirm Removal";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}

		/// <summary>This method is called when the user clicks on NO</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnClickNo(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Close();
		}
		
		/// <summary>This method is called when the user clicks on Yes To All</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickYesToAll(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			this.m_bYesToAll = true;
			this.Close();
		}

		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			m_ctrlName.Text = m_strDeleteName;
			m_ctrlMessage.Text = String.Format("Records shown here will also be deleted because they reference {0}", m_strDeleteName);
			
			if((this.ICodes != null) && (this.ICodes.Count > 0))
			{
				m_ctrlCodes.DisplayMode = this.CodesDisplayMode;
				
				foreach(object O in this.ICodes)
				{
					//	We really just want the first object
					m_ctrlCodes.Initialize((ITmaxListViewCtrl)O);
					break;
				}
				
				m_ctrlCodes.Add(this.ICodes, true);
			
			}// if((this.ICodes != null) && (this.ICodes.Count > 0))
			
			if((this.ICaseCodes != null) && (this.ICaseCodes.Count > 0))
			{
				m_ctrlCaseCodes.DisplayMode = this.CaseCodesDisplayMode;
				
				foreach(object O in this.ICaseCodes)
				{
					//	We really just want the first object
					m_ctrlCaseCodes.Initialize((ITmaxListViewCtrl)O);
					break;
				}
				
				m_ctrlCaseCodes.Add(this.ICaseCodes, true);
			
			}// if((this.ICaseCodes != null) && (this.ICaseCodes.Count > 0))
			else
			{
				m_ctrlCodes.SetBounds(0, 0, 0, m_ctrlCaseCodes.Bottom - m_ctrlCodes.Top, BoundsSpecified.Height);
				
				m_ctrlCaseCodesLabel.Enabled = false;
				m_ctrlCaseCodesLabel.Visible = false;
				m_ctrlCaseCodes.Enabled = false;
				m_ctrlCaseCodes.Visible = false;
				
			}
			
			//	Get their attention
			FTI.Shared.Win32.User.MessageBeep(0);
			
		}// protected void OnLoad(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Properties
		
		/// <summary>True to confirm all subsequent deletions</summary>
		public bool YesToAll
		{
			get { return m_bYesToAll;	}
			set { m_bYesToAll = value;	}
			
		}// YesToAll property
			
		/// <summary>Name of the case code or pick list being deleted</summary>
		public string DeleteName
		{
			get { return m_strDeleteName;	}
			set { m_strDeleteName = value;	}
		}
			
		/// <summary>Collection of fielded data (codes) being deleted</summary>
		public IList ICodes
		{
			get { return m_ICodes;	}
			set { m_ICodes = value;	}
		}
			
		/// <summary>Collection of case codes being deleted</summary>
		public IList ICaseCodes
		{
			get { return m_ICaseCodes;	}
			set { m_ICaseCodes = value;	}
		}
			
		/// <summary>Display mode used to populate codes list box</summary>
		public int CodesDisplayMode
		{
			get { return m_iCodesDisplayMode;	}
			set { m_iCodesDisplayMode = value;	}
		}
			
		/// <summary>Display mode used to populate case codes list box</summary>
		public int CaseCodesDisplayMode
		{
			get { return m_iCaseCodesDisplayMode;	}
			set { m_iCaseCodesDisplayMode = value;	}
		}
			
		#endregion Properties

	
	}// public class CFConfirmCodesDelete : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms

