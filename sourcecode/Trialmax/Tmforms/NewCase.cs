using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to create a new case</summary>
	public class CFNewCase : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Form cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Form OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Click to browse to new location</summary>
		private System.Windows.Forms.Button m_ctrlBrowse;
		
		/// <summary>Form's image strip used for button images</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Control used to display location of the case's parent folder</summary>
		private System.Windows.Forms.Label m_ctrlLocation;
		
		/// <summary>Location control label</summary>
		private System.Windows.Forms.Label m_ctrlLocationLabel;
		
		/// <summary>Name used to identify the new case</summary>
		private System.Windows.Forms.TextBox m_ctrlCaseName;
		
		/// <summary>Label for Name control</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strCaseName = "";
		
		/// <summary>Local member bound to Location property</summary>
		private string m_strParentFolder = "c:\\";
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFNewCase()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		/// <summary>This method will initialize the ParentFolder property using the specified case folder</summary>
		/// <param name="strCaseFolder">The case folder to be parsed</param>
		public void SetParentFromCase(string strCaseFolder)
		{
			char[]	acDelimiters = {'\\','/'};
			string	strPath = "";
			
			//	Make sure we have a valid path
			if((strCaseFolder == null) || (strCaseFolder.Length == 0)) return;
			
			//	Clear the existing parent
			m_strParentFolder = "";
			
			//	Strip the trailing separator if it exists
			strPath = strCaseFolder.TrimEnd(acDelimiters);

			//	Parse the path into it's component parts
			string[] aFolders = strPath.Split(acDelimiters);
			if((aFolders != null) && (aFolders.GetUpperBound(0) >= 0))
			{
				for(int i = 0; i < aFolders.GetUpperBound(0); i++)
				{
					m_strParentFolder += aFolders[i];
					if(m_strParentFolder.EndsWith("\\") == false &&
					   (m_strParentFolder.EndsWith("/") == false))
					{
						m_strParentFolder += "\\";
					}
				}
			
			}
			
			m_strParentFolder = m_strParentFolder.ToLower();

		}
		
		/// <summary>This method is called to get the fully qualified path to the new database folder</summary>
		/// <returns>The path to the new folder</returns>
		public string GetFolderSpec()
		{
			string strFolder = "";
			
			if((m_strParentFolder == null) || (m_strCaseName == null))
				return string.Empty;
				
			strFolder = m_strParentFolder;
				
			if(strFolder.EndsWith("\\") == false)
				strFolder += "\\";
				
			strFolder += m_strCaseName;
			
			return strFolder;
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

		/// <summary>Required method for Designer support</summary>
		protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFNewCase));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlLocation = new System.Windows.Forms.Label();
			this.m_ctrlBrowse = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlLocationLabel = new System.Windows.Forms.Label();
			this.m_ctrlCaseName = new System.Windows.Forms.TextBox();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(167, 104);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(67, 104);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 1;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlLocation
			// 
			this.m_ctrlLocation.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlLocation.Name = "m_ctrlLocation";
			this.m_ctrlLocation.Size = new System.Drawing.Size(264, 20);
			this.m_ctrlLocation.TabIndex = 12;
			this.m_ctrlLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlBrowse
			// 
			this.m_ctrlBrowse.Image = ((System.Drawing.Bitmap)(resources.GetObject("m_ctrlBrowse.Image")));
			this.m_ctrlBrowse.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowse.ImageIndex = 0;
			this.m_ctrlBrowse.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowse.Location = new System.Drawing.Point(276, 24);
			this.m_ctrlBrowse.Name = "m_ctrlBrowse";
			this.m_ctrlBrowse.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowse.TabIndex = 10;
			this.m_ctrlBrowse.Click += new System.EventHandler(this.OnBrowse);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlLocationLabel
			// 
			this.m_ctrlLocationLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlLocationLabel.Name = "m_ctrlLocationLabel";
			this.m_ctrlLocationLabel.Size = new System.Drawing.Size(224, 16);
			this.m_ctrlLocationLabel.TabIndex = 13;
			this.m_ctrlLocationLabel.Text = "Location:";
			// 
			// m_ctrlCaseName
			// 
			this.m_ctrlCaseName.Location = new System.Drawing.Point(52, 60);
			this.m_ctrlCaseName.Name = "m_ctrlCaseName";
			this.m_ctrlCaseName.Size = new System.Drawing.Size(248, 20);
			this.m_ctrlCaseName.TabIndex = 0;
			this.m_ctrlCaseName.Text = "";
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(8, 60);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(44, 20);
			this.m_ctrlNameLabel.TabIndex = 15;
			this.m_ctrlNameLabel.Text = "Name : ";
			this.m_ctrlNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFNewCase
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(308, 137);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_ctrlNameLabel,
																		  this.m_ctrlCaseName,
																		  this.m_ctrlLocationLabel,
																		  this.m_ctrlLocation,
																		  this.m_ctrlBrowse,
																		  this.m_ctrlCancel,
																		  this.m_ctrlOk});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFNewCase";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " New Case";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>Traps the event fired when the form gets loaded for the first time</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			if((m_strParentFolder != null) && (m_ctrlLocation != null))
				m_ctrlLocation.Text = m_strParentFolder;
				
			if((m_strCaseName != null) && (m_ctrlCaseName != null))
				m_ctrlCaseName.Text = m_strCaseName;
		}
	
		/// <summary>Traps the event fired when the user clicks on the Ok button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			string strFolderSpec = "";
			string strMsg = "";
			string strIllegal = "\\/*:?\"<>|'";
			
			//	Get the parent folder
			m_strParentFolder = m_ctrlLocation.Text;
			m_strParentFolder = m_strParentFolder.ToLower();
			
			//	Get the new name
			m_strCaseName = m_ctrlCaseName.Text.Trim();
			if(m_strCaseName.Length == 0)
			{
				MessageBox.Show("You must provide a name for the new case", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				m_ctrlCaseName.Focus();
				return;
			}
			
			if(m_strCaseName.IndexOfAny(strIllegal.ToCharArray()) >= 0)
			{
				strMsg = "Case names can not contain any of the following characters:\n" + strIllegal;
				MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				m_ctrlCaseName.Focus();
				m_ctrlCaseName.SelectAll();
				return;
			}
			
			//	Make sure we can create the folder
			strFolderSpec = GetFolderSpec();
			if(System.IO.Directory.Exists(strFolderSpec) == false)
			{
				try
				{
					System.IO.Directory.CreateDirectory(strFolderSpec);
				}
				catch
				{
					strMsg = String.Format("Unable to create the folder for the new case: {0}", strFolderSpec);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					m_ctrlCaseName.Focus();
					return;
				}
				
			}
			
			DialogResult = DialogResult.OK;
			Close();
		}
		
		/// <summary>Traps the event fired when the user clicks on the Browse button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnBrowse(object sender, System.EventArgs e)
		{
			FTI.Shared.CBrowseForFolder bff = new CBrowseForFolder();
			
			bff.Folder = m_ctrlLocation.Text;
			bff.Prompt = "Select the new location : ";
			bff.NoNewFolder = false;
			
			if(bff.ShowDialog(this.Handle) == DialogResult.OK)
			{
				m_ctrlLocation.Text = bff.Folder.ToLower();
				if((m_ctrlLocation.Text.EndsWith("\\") == false) &&
				   (m_ctrlLocation.Text.EndsWith("/") == false))
				{
					m_ctrlLocation.Text += "\\";
				}
			}
			
		}

		#endregion Protected Methods

		#region Properties
		
		/// <summary>Name used for the new case</summary>
		public string CaseName
		{
			get{ return m_strCaseName; }
			set{ m_strCaseName = value; }
		}
			
		/// <summary>Parent folder containing the case folder</summary>
		public string ParentFolder
		{
			get{ return m_strParentFolder; }
			set{ m_strParentFolder = value; }
		}
			
		#endregion Properties

	}// public class CFNewCase : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
