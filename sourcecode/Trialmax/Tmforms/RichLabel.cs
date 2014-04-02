using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>Form used to display rich text format (RTF) files and resources</summary>
	public class CFRichLabel : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Pushbutton used to close the form</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>Custom TrialMax control for displaying rich text format</summary>
		private FTI.Trialmax.Controls.CTmaxRichLabelCtrl m_ctrlRichLabel;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFRichLabel()
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
		
		}// protected override void Dispose( bool disposing )
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFRichLabel));
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlRichLabel = new FTI.Trialmax.Controls.CTmaxRichLabelCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOK.Location = new System.Drawing.Point(440, 308);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 0;
			this.m_ctrlOK.Text = "&OK";
			// 
			// m_ctrlRichLabel
			// 
			this.m_ctrlRichLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlRichLabel.BackColor = System.Drawing.SystemColors.Window;
			this.m_ctrlRichLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_ctrlRichLabel.FileSpec = "";
			this.m_ctrlRichLabel.Location = new System.Drawing.Point(4, 4);
			this.m_ctrlRichLabel.Name = "m_ctrlRichLabel";
			this.m_ctrlRichLabel.Size = new System.Drawing.Size(516, 296);
			this.m_ctrlRichLabel.TabIndex = 1;
			// 
			// CFRichLabel
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(524, 341);
			this.Controls.Add(this.m_ctrlRichLabel);
			this.Controls.Add(this.m_ctrlOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFRichLabel";
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Fully qualified path to the RTF file to be displayed in the control</summary>
		public string FileSpec
		{
			get { return m_ctrlRichLabel.FileSpec; }
			set { m_ctrlRichLabel.FileSpec = value; }
		}
		
		/// <summary>Stream containing RTF text used to fill the control</summary>
		public System.IO.Stream IOStream
		{
			get { return m_ctrlRichLabel.IOStream; }
			set { m_ctrlRichLabel.IOStream = value; }
		}
		
		#endregion Properties

	}// public class CFRichLabel : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
