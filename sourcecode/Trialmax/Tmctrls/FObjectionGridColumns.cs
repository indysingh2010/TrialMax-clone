using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>Form that allows users to choose the columns for an objections grid control</summary>
	public class CFObjectionGridColumns : Form
	{
		#region Private Members

		/// <summary>Component collection required by designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Group box for StatusTextMode selections</summary>
		private System.Windows.Forms.GroupBox m_ctrlColumnsGroup;

		/// <summary>Check list box for column selections</summary>
		private CheckedListBox m_ctrlColumns;
		private Button m_ctrlShowAll;

		/// <summary>Private member bound to GridCtrl property</summary>
		private FTI.Trialmax.Controls.CTmaxObjectionGridCtrl m_tmaxGridCtrl = null;

		#endregion Private Members

		#region Public Methods

		/// <summary>Default constructor</summary>
		public CFObjectionGridColumns()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

		}

		#endregion Public Methods

		#region Protected Methods

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
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFObjectionGridColumns));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlColumnsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlColumns = new System.Windows.Forms.CheckedListBox();
			this.m_ctrlShowAll = new System.Windows.Forms.Button();
			this.m_ctrlColumnsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(183, 199);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(103, 199);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 0;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlColumnsGroup
			// 
			this.m_ctrlColumnsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlColumnsGroup.Controls.Add(this.m_ctrlColumns);
			this.m_ctrlColumnsGroup.Location = new System.Drawing.Point(9, 8);
			this.m_ctrlColumnsGroup.Name = "m_ctrlColumnsGroup";
			this.m_ctrlColumnsGroup.Size = new System.Drawing.Size(247, 185);
			this.m_ctrlColumnsGroup.TabIndex = 0;
			this.m_ctrlColumnsGroup.TabStop = false;
			this.m_ctrlColumnsGroup.Text = "Fields";
			// 
			// m_ctrlColumns
			// 
			this.m_ctrlColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlColumns.CheckOnClick = true;
			this.m_ctrlColumns.FormattingEnabled = true;
			this.m_ctrlColumns.Location = new System.Drawing.Point(6, 19);
			this.m_ctrlColumns.Name = "m_ctrlColumns";
			this.m_ctrlColumns.Size = new System.Drawing.Size(235, 154);
			this.m_ctrlColumns.TabIndex = 0;
			// 
			// m_ctrlShowAll
			// 
			this.m_ctrlShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlShowAll.Location = new System.Drawing.Point(9, 199);
			this.m_ctrlShowAll.Name = "m_ctrlShowAll";
			this.m_ctrlShowAll.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlShowAll.TabIndex = 1;
			this.m_ctrlShowAll.Text = "&Show All";
			this.m_ctrlShowAll.Click += new System.EventHandler(this.OnClickShowAll);
			// 
			// CFObjectionGridColumns
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(268, 234);
			this.Controls.Add(this.m_ctrlShowAll);
			this.Controls.Add(this.m_ctrlColumnsGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFObjectionGridColumns";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Show / Hide Objection Fields";
			this.Load += new System.EventHandler(this.OnLoad);
			this.m_ctrlColumnsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion Protected Methods

		/// <summary>
		/// This method is called when the user clicks on OK
		/// </summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event argument object</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			try
			{
				for(int i = 0; i < m_ctrlColumns.Items.Count; i++)
				{
					((CTmaxObjectionGridColumn)(m_ctrlColumns.Items[i])).Visible = m_ctrlColumns.GetItemChecked(i);
				}
			}
			catch
			{
			}

		}// protected void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on ShowAll</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickShowAll(object sender, System.EventArgs e)
		{
			if((m_ctrlColumns != null) && (m_ctrlColumns.Items != null))
			{
				for(int i = 0; i < m_ctrlColumns.Items.Count; i++)
					m_ctrlColumns.SetItemChecked(i, true);
			}

		}// private void OnClickShowAll(object sender, System.EventArgs e)

		/// <summary>This method is called when the form is displayed the first time</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			FillColumns();

			m_ctrlOk.Enabled = (m_ctrlColumns.Items.Count > 0);

		}// private void OnLoad(object sender, System.EventArgs e)

		/// <summary>Called to populate the columns list box</summary>
		private void FillColumns()
		{
			if((m_tmaxGridCtrl != null) && (m_tmaxGridCtrl.Columns != null))
			{
				foreach(CTmaxObjectionGridColumn O in m_tmaxGridCtrl.Columns)
				{
					if(O.Configurable == true)
						m_ctrlColumns.Items.Add(O, O.Visible);
				}

			}// if((m_tmaxGridCtrl != null) && (m_tmaxGridCtrl.Columns != null))

		}// private void FillColumns()

		#region Properties

		/// <summary>The Objection grid control bound to the pane</summary>
		public CTmaxObjectionGridCtrl GridCtrl
		{
			get { return m_tmaxGridCtrl; }
			set { m_tmaxGridCtrl = value; }
		}

		#endregion Properties

	}// public class CFObjectionGridColumns

}// namespace FTI.Trialmax.Panes

