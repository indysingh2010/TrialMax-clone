using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This form is used to add / edit pick lists</summary>
	public class CFPickListEditor : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Component collection required by forms designer</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Check box to allow the user to specify UserAdditions option</summary>
		private System.Windows.Forms.CheckBox m_ctrlUserAdditions;
		
		/// <summary>Check box to allow the user to specify CaseSensitive option</summary>
		private System.Windows.Forms.CheckBox m_ctrlCaseSensitive;
		
		/// <summary>Static text label for item name edit box</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;
		
		/// <summary>Edit box to allow the user to specify Item Name</summary>
		private System.Windows.Forms.TextBox m_ctrlName;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Private member bound to PickItem property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickItem = null;
		
		/// <summary>Private member bound to Edit property</summary>
		private bool m_bEdit = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFPickListEditor()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

		}
		
		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>Called when form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if(this.PickItem != null)
			{
				m_ctrlName.Text = this.PickItem.Name;
				
				switch(this.PickItem.Type)
				{
					case TmaxPickItemTypes.MultiLevel:
					
						m_ctrlCaseSensitive.Checked = this.PickItem.CaseSensitive;
						m_ctrlUserAdditions.Visible = false;
						break;
					
					case TmaxPickItemTypes.StringList:
					case TmaxPickItemTypes.Unknown:
					
						m_ctrlCaseSensitive.Checked = this.PickItem.CaseSensitive;
						m_ctrlUserAdditions.Checked = this.PickItem.UserAdditions;
						break;
					
					case TmaxPickItemTypes.Value:
					default:
					
						m_ctrlCaseSensitive.Visible = false;
						m_ctrlUserAdditions.Visible = false;
						break;
					
				}// switch(this.PickItem.Type)
				
			}
			else
			{
				m_ctrlName.Enabled = false;
				m_ctrlNameLabel.Enabled = false;
				m_ctrlCaseSensitive.Enabled = false;
				m_ctrlUserAdditions.Enabled = false;
				m_ctrlOK.Enabled = false;
			}
			
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

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
		
		/// <summary>Called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnOK(object sender, System.EventArgs e)
		{
			CTmaxPickItem tmaxDuplicate = null;
			
			//	Has the user supplied a name?
			if(m_ctrlName.Text.Length == 0)
			{
				Warn("You must supply a valid name for the operation", m_ctrlName);
				return;
			}
			
			//	Check for a duplicate
			if(this.PickItem.Parent != null)
			{
				tmaxDuplicate  = this.PickItem.Parent.FindChild(m_ctrlName.Text);
				if(ReferenceEquals(this.PickItem, tmaxDuplicate) == true)
					tmaxDuplicate = null; // Same item
			}
			
			//	Did we discover a duplicate item?
			if(tmaxDuplicate != null)
			{
				Warn("An item named " + m_ctrlName.Text + " is already in use", m_ctrlName);
			}
			else
			{
				this.PickItem.Name = m_ctrlName.Text;
				
				if(m_ctrlCaseSensitive.Visible == true)
					this.PickItem.CaseSensitive = m_ctrlCaseSensitive.Checked;
				if(m_ctrlUserAdditions.Visible == true)
					this.PickItem.UserAdditions = m_ctrlUserAdditions.Checked;
			
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			
		}// private void OnOK(object sender, System.EventArgs e)
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFPickListEditor));
			this.m_ctrlUserAdditions = new System.Windows.Forms.CheckBox();
			this.m_ctrlCaseSensitive = new System.Windows.Forms.CheckBox();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.TextBox();
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlUserAdditions
			// 
			this.m_ctrlUserAdditions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlUserAdditions.Location = new System.Drawing.Point(12, 80);
			this.m_ctrlUserAdditions.Name = "m_ctrlUserAdditions";
			this.m_ctrlUserAdditions.Size = new System.Drawing.Size(192, 24);
			this.m_ctrlUserAdditions.TabIndex = 2;
			this.m_ctrlUserAdditions.Text = "Allow User Additions";
			// 
			// m_ctrlCaseSensitive
			// 
			this.m_ctrlCaseSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseSensitive.Location = new System.Drawing.Point(12, 56);
			this.m_ctrlCaseSensitive.Name = "m_ctrlCaseSensitive";
			this.m_ctrlCaseSensitive.Size = new System.Drawing.Size(192, 24);
			this.m_ctrlCaseSensitive.TabIndex = 1;
			this.m_ctrlCaseSensitive.Text = "Case Sensitive";
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(12, 8);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(192, 16);
			this.m_ctrlNameLabel.TabIndex = 16;
			this.m_ctrlNameLabel.Text = "List / Value Name:";
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlName.Location = new System.Drawing.Point(12, 24);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(272, 20);
			this.m_ctrlName.TabIndex = 0;
			this.m_ctrlName.Text = "";
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Location = new System.Drawing.Point(120, 112);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 3;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnOK);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(208, 112);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// CFPickListEditor
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(296, 141);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.Controls.Add(this.m_ctrlUserAdditions);
			this.Controls.Add(this.m_ctrlCaseSensitive);
			this.Controls.Add(this.m_ctrlNameLabel);
			this.Controls.Add(this.m_ctrlName);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFPickListEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Pick List Editor";
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <param name="ctrlSelect">the control to select after the warning is displayed</param>
		protected virtual bool Warn(string strMsg, System.Windows.Forms.Control ctrlSelect)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation);
			
			if(ctrlSelect != null)
				ctrlSelect.Focus();	
				
			return false; // allows for cleaner code						
		
		}// protected virtual bool Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		
		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		protected virtual bool Warn(string strMsg)
		{
			return Warn(strMsg, null);			
		}
		
		#endregion Private Methods

		#region Properties
		
		//	The item being added / edited
		public FTI.Shared.Trialmax.CTmaxPickItem PickItem
		{
			get { return m_tmaxPickItem; }
			set { m_tmaxPickItem = value; }
		}
		
		//	True if PickItem is being edited
		public bool Edit
		{
			get { return m_bEdit; }
			set { m_bEdit = value; }
		}
		
		#endregion Properties
		
	}// public class CFPickListEditor : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Controls
