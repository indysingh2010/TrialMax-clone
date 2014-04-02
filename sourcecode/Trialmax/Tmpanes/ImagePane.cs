using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace FTI.Trialmax.Panes
{
	public class CImagePane : FTI.Trialmax.Panes.CBasePane
	{
		#region Private data
		
		/// <summary>Required designer variable</BR></summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Control used to display filename if file not found</BR></summary>
		private Label m_ctrlLabel;
		
		/// <summary>Full path to file to be displayed</BR></summary>
		private string m_strImageFilename;
		
		/// <summary>Bitmap object being displayed</BR></summary>
		private Bitmap m_Bitmap;
		
		/// <summary>Picture box used to display the image</BR></summary>
		private System.Windows.Forms.PictureBox m_ctrlImage;
		
		/// <summary>Scaling mode applied to the image</BR></summary>
		private short m_sImageScaling;
		
		#endregion Private data

		/// <summary>Default constructor</BR></summary>
		public CImagePane() : base()
		{
			m_ctrlImage = new System.Windows.Forms.PictureBox();

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}


		#region Component Designer generated code
		
		/// <summary>Clean up all resources being used</BR></summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>Designer uses this function to initialize child controls</BR></summary>
		protected override void InitializeComponent()
		{
			this.m_ctrlImage = new System.Windows.Forms.PictureBox();
			this.m_ctrlLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlImage
			// 
			this.m_ctrlImage.Location = new System.Drawing.Point(16, 48);
			this.m_ctrlImage.Name = "m_ctrlImage";
			this.m_ctrlImage.Size = new System.Drawing.Size(120, 88);
			this.m_ctrlImage.TabIndex = 1;
			this.m_ctrlImage.TabStop = false;
			this.m_ctrlImage.Click += new System.EventHandler(this.OnClick);
			
			// 
			// m_ctrlLabel
			// 
			this.m_ctrlLabel.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlLabel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.m_ctrlLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlLabel.Name = "m_ctrlLabel";
			this.m_ctrlLabel.Size = new System.Drawing.Size(136, 24);
			this.m_ctrlLabel.TabIndex = 2;
			this.m_ctrlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CImagePane
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_ctrlLabel,
																		  this.m_ctrlImage});
			this.Name = "CImagePane";
			this.ResumeLayout(false);

		}
		#endregion
		
		#region Properties

		/// <summary>Name (including path) of file to be displayed</BR></summary>
		[ Editor (typeof(FileNameEditor),typeof(UITypeEditor)) ]
		public string ImageFilename
		{
			get
			{
				return m_strImageFilename;
			}
			set
			{
				//	Save the new filename
				m_strImageFilename = value;

				//	Load the image
				LoadImage();
			}
		}

		/// <summary>Scaling method applied to image</BR></summary>
		///	<remarks>0 = Normal (no resize, upper left
		///			 <BR>1 = Centered (no resize, centered in pane)
		///			 <BR>2 = Stretched (resize to use full area)
		/// </remarks>
		public short ImageScaling
		{
			get
			{
				return m_sImageScaling;
			}
			set
			{
				//	Store the new value
				m_sImageScaling = value;
				
				//	Scale using the new value
				SetScaling();
			}
		}
		
		#endregion Properties
		
		#region Protected Methods
		
		/// <summary>Loads the file into the picture box</BR></summary>
		protected void LoadImage()
		{
			//	Nothing to do if no image control
			if(m_ctrlImage == null) return;

			//	Destroy the existing bitmap
			if(m_Bitmap != null)
				m_Bitmap.Dispose();

			if((m_strImageFilename == null) || (m_strImageFilename.Length == 0))
			{
				m_ctrlImage.Image = null;
			}
			else
			{
				if(System.IO.File.Exists(m_strImageFilename) == false)
				{
					m_ctrlImage.Image = null;
				}
				else
				{
					m_Bitmap = new Bitmap(m_strImageFilename);
					m_ctrlImage.Image = (Image)m_Bitmap;
				}
			}

			if(m_ctrlImage.Image == null)
				m_ctrlImage.Hide();
			else
				m_ctrlImage.Show();

			//	Set the label text
			if(m_ctrlLabel != null)
			{
				m_ctrlLabel.Text = m_strImageFilename;
				if(m_ctrlImage.Image != null)
					m_ctrlLabel.Hide();
				else
					m_ctrlLabel.Show();
			}

		}

		/// <summary>Applies the scaling mode to the image</BR></summary>
		protected void SetScaling()
		{
			//	Nothing to do if no image control
			if(m_ctrlImage == null) return;

			switch(m_sImageScaling)
			{
				case 1:
				
					m_ctrlImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
					break;
					
				case 2:
				
					m_ctrlImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
					break;
					
				default:
				
					m_ctrlImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
					break;
					
			}

		}

		/// <summary>Recalculates layout of the child controls</BR></summary>
		protected override void RecalcLayout()
		{
			if(m_ctrlLabel != null)
			{
				m_ctrlLabel.Left = 1;
				m_ctrlLabel.Top = 1;
				m_ctrlLabel.Width = this.Width - 2;
				m_ctrlLabel.Height = this.Height - 2;
			}
			if(m_ctrlImage != null)
			{
				m_ctrlImage.Left = 1;
				m_ctrlImage.Top = 1;
				m_ctrlImage.Width = this.Width - 2;
				m_ctrlImage.Height = this.Height - 2;
			}
		}

		#endregion Protected Methods

		#region Event Handlers
		
		#endregion Event Handlers

		private void OnClick(object sender, System.EventArgs e)
		{
		}

	}
}

