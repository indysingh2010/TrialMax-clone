using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>Status form displayed while exporting PowerPoint slide</summary>
	public class CFExportSlide : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Information Icon</summary>
		private System.Windows.Forms.PictureBox m_ctrlInformationIcon;
		
		/// <summary>Label for presentation filename</summary>
		private System.Windows.Forms.Label m_ctrlPresentationLabel;
		
		/// <summary>Control to display presentation filename</summary>
		private System.Windows.Forms.Label m_ctrlPresentation;
		
		/// <summary>Label for export filename</summary>
		private System.Windows.Forms.Label m_ctrlFilename;
		
		/// <summary>Control to display export filename</summary>
		private System.Windows.Forms.Label m_ctrlFilenameLabel;
		
		/// <summary>Information Text</summary>
		private System.Windows.Forms.Label m_ctrlInformationText;
		
		/// <summary>Local member bound to Presentation property</summary>
		private string m_strPresentation = "";
		
		/// <summary>Local member bound to Export property</summary>
		private string m_strExport = "";
		
		#endregion Private Members
		
		/// <summary>Constructor</summary>
		public CFExportSlide()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

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
		
		/// <summary>Overloaded base class member called when the form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if(m_strPresentation.Length > 0)
				m_ctrlPresentation.Text = CTmaxToolbox.FitPathToWidth(m_strPresentation, m_ctrlPresentation);
			if(m_strExport.Length > 0)
				m_ctrlFilename.Text = CTmaxToolbox.FitPathToWidth(m_strExport, m_ctrlFilename);
			
			//	Do the base class processing
			base.OnLoad (e);
		}
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFExportSlide));
			this.m_ctrlInformationIcon = new System.Windows.Forms.PictureBox();
			this.m_ctrlInformationText = new System.Windows.Forms.Label();
			this.m_ctrlPresentationLabel = new System.Windows.Forms.Label();
			this.m_ctrlPresentation = new System.Windows.Forms.Label();
			this.m_ctrlFilename = new System.Windows.Forms.Label();
			this.m_ctrlFilenameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlInformationIcon
			// 
			this.m_ctrlInformationIcon.Image = ((System.Drawing.Image)(resources.GetObject("m_ctrlInformationIcon.Image")));
			this.m_ctrlInformationIcon.Location = new System.Drawing.Point(8, 4);
			this.m_ctrlInformationIcon.Name = "m_ctrlInformationIcon";
			this.m_ctrlInformationIcon.Size = new System.Drawing.Size(24, 24);
			this.m_ctrlInformationIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.m_ctrlInformationIcon.TabIndex = 0;
			this.m_ctrlInformationIcon.TabStop = false;
			// 
			// m_ctrlInformationText
			// 
			this.m_ctrlInformationText.Location = new System.Drawing.Point(52, 8);
			this.m_ctrlInformationText.Name = "m_ctrlInformationText";
			this.m_ctrlInformationText.Size = new System.Drawing.Size(320, 16);
			this.m_ctrlInformationText.TabIndex = 1;
			this.m_ctrlInformationText.Text = "Exporting PowerPoint Slide ....";
			// 
			// m_ctrlPresentationLabel
			// 
			this.m_ctrlPresentationLabel.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlPresentationLabel.Name = "m_ctrlPresentationLabel";
			this.m_ctrlPresentationLabel.Size = new System.Drawing.Size(92, 16);
			this.m_ctrlPresentationLabel.TabIndex = 2;
			this.m_ctrlPresentationLabel.Text = "Presentation:";
			// 
			// m_ctrlPresentation
			// 
			this.m_ctrlPresentation.Location = new System.Drawing.Point(104, 40);
			this.m_ctrlPresentation.Name = "m_ctrlPresentation";
			this.m_ctrlPresentation.Size = new System.Drawing.Size(292, 16);
			this.m_ctrlPresentation.TabIndex = 3;
			// 
			// m_ctrlFilename
			// 
			this.m_ctrlFilename.Location = new System.Drawing.Point(104, 60);
			this.m_ctrlFilename.Name = "m_ctrlFilename";
			this.m_ctrlFilename.Size = new System.Drawing.Size(292, 16);
			this.m_ctrlFilename.TabIndex = 5;
			// 
			// m_ctrlFilenameLabel
			// 
			this.m_ctrlFilenameLabel.Location = new System.Drawing.Point(8, 60);
			this.m_ctrlFilenameLabel.Name = "m_ctrlFilenameLabel";
			this.m_ctrlFilenameLabel.Size = new System.Drawing.Size(92, 16);
			this.m_ctrlFilenameLabel.TabIndex = 4;
			this.m_ctrlFilenameLabel.Text = "Export Filename:";
			// 
			// CFExportSlide
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 81);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlFilename);
			this.Controls.Add(this.m_ctrlFilenameLabel);
			this.Controls.Add(this.m_ctrlPresentation);
			this.Controls.Add(this.m_ctrlPresentationLabel);
			this.Controls.Add(this.m_ctrlInformationText);
			this.Controls.Add(this.m_ctrlInformationIcon);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CFExportSlide";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export";
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Name of the source presentation</summary>
		public string Presentation
		{
			get { return m_strPresentation; }
			set { m_strPresentation = value; }
		}
		
		/// <summary>Name of the source presentation</summary>
		public string Export
		{
			get { return m_strExport; }
			set { m_strExport = value; }
		}
		
		#endregion Properties
	
	}// public class CFExportSlide : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
