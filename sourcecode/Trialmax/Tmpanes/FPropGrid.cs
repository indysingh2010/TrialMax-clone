using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Win32;
using FTI.Shared.Trialmax;
using FTI.Trialmax;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Panes
{
	/// <summary>This form allows the user to edit the LinkedPath value of a media record</summary>
	public class CFPropGrid : CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Local member bound to Database property</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>TrialMax property grid control</summary>
		private FTI.Trialmax.Controls.CTmaxPropGridCtrl m_ctrlGrid;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFPropGrid() : base()
		{
			// Initialize the child controls
			InitializeComponent();
			
			m_tmaxEventSource.Attach(m_ctrlGrid.EventSource);
			m_ctrlGrid.Initialize(null);
		}

		/// <summary>This method is called to add the specified properties to the grid</summary>
		/// <param name="IProperties">The collection of properties to be added</param>
		/// <returns>true if successful</returns>
		public bool Fill(ICollection IProperties)
		{
			bool bSuccessful = false;
			
			try
			{
				bSuccessful = m_ctrlGrid.Add(IProperties, true);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Fill", Ex);
			}
			
			return bSuccessful;
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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlGrid = new FTI.Trialmax.Controls.CTmaxPropGridCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(237, 314);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlGrid
			// 
			this.m_ctrlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlGrid.DropListBoolean = true;
			this.m_ctrlGrid.Location = new System.Drawing.Point(12, 12);
			this.m_ctrlGrid.Name = "m_ctrlGrid";
			this.m_ctrlGrid.PaneId = 0;
			this.m_ctrlGrid.Size = new System.Drawing.Size(297, 294);
			this.m_ctrlGrid.SortOn = ((long)(0));
			this.m_ctrlGrid.TabIndex = 4;
			// 
			// CFPropGrid
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(321, 345);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlGrid);
			this.Controls.Add(this.m_ctrlOk);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFPropGrid";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Property Grid Test Form";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
					
		}// private void OnLoad(object sender, System.EventArgs e)
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			//	Close the dialog
			DialogResult = DialogResult.OK;
			this.Close();
		
		}// private void OnClickOk(object sender, System.EventArgs e)
		

		#endregion Private Methods

		#region Properties
		
		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}
		
		#endregion Properties

	}// public class CFPropGrid : CFTmaxBaseForm

}// namespace FTI.Trialmax.Panes
