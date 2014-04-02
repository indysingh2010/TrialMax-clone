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
	public class CFPathEditor : CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Browse button</summary>
		private System.Windows.Forms.Button m_ctrlBrowse;
		
		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Static text label for Linked Path</summary>
		private System.Windows.Forms.Label m_ctrlRelativePathLabel;
		
		/// <summary>Static text label for Registered From value</summary>
		private System.Windows.Forms.Label m_ctrlMediaRootLabel;
		
		/// <summary>Static text to display Media Path value</summary>
		private System.Windows.Forms.Label m_ctrlCurrentPath;
		
		/// <summary>Static text label for Media Path value</summary>
		private System.Windows.Forms.Label m_ctrlCurrentPathLabel;
		
		/// <summary>Image list control for button images</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Static text to display Registered From value</summary>
		private System.Windows.Forms.Label m_ctrlMediaRoot;
		
		/// <summary>Static text control to display user warning</summary>
		private System.Windows.Forms.Label m_ctrlWarning;
		
		/// <summary>Static text label for user warning</summary>
		private System.Windows.Forms.Label m_ctrlWarningLabel;
		
		/// <summary>Text edit box for linked path</summary>
		private System.Windows.Forms.TextBox m_ctrlRelativePath;
		
		/// <summary>Local member bound to Record property</summary>
		private CDxMediaRecord m_dxRecord = null;
		
		/// <summary>Local member bound to Database property</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member to store reference to primary record</summary>
		private CDxPrimary m_dxPrimary = null;
		
		/// <summary>Local member to store reference to secondary record</summary>
		private CDxSecondary m_dxSecondary = null;
		
		/// <summary>Local member to store the original RelativePath value</summary>
		private string m_strRelativePath = "";
		
		/// <summary>Local member to store the media root folder</summary>
		private string m_strMediaRoot = "";
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFPathEditor() : base()
		{
			// Initialize the child controls
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
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFPathEditor));
			this.m_ctrlRelativePathLabel = new System.Windows.Forms.Label();
			this.m_ctrlRelativePath = new System.Windows.Forms.TextBox();
			this.m_ctrlMediaRootLabel = new System.Windows.Forms.Label();
			this.m_ctrlCurrentPath = new System.Windows.Forms.Label();
			this.m_ctrlBrowse = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlCurrentPathLabel = new System.Windows.Forms.Label();
			this.m_ctrlMediaRoot = new System.Windows.Forms.Label();
			this.m_ctrlWarning = new System.Windows.Forms.Label();
			this.m_ctrlWarningLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlRelativePathLabel
			// 
			this.m_ctrlRelativePathLabel.Location = new System.Drawing.Point(8, 84);
			this.m_ctrlRelativePathLabel.Name = "m_ctrlRelativePathLabel";
			this.m_ctrlRelativePathLabel.Size = new System.Drawing.Size(96, 20);
			this.m_ctrlRelativePathLabel.TabIndex = 22;
			this.m_ctrlRelativePathLabel.Text = "Relative Path:";
			this.m_ctrlRelativePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlRelativePath
			// 
			this.m_ctrlRelativePath.Location = new System.Drawing.Point(108, 84);
			this.m_ctrlRelativePath.Name = "m_ctrlRelativePath";
			this.m_ctrlRelativePath.Size = new System.Drawing.Size(248, 20);
			this.m_ctrlRelativePath.TabIndex = 0;
			this.m_ctrlRelativePath.Text = "";
			// 
			// m_ctrlMediaRootLabel
			// 
			this.m_ctrlMediaRootLabel.Location = new System.Drawing.Point(8, 44);
			this.m_ctrlMediaRootLabel.Name = "m_ctrlMediaRootLabel";
			this.m_ctrlMediaRootLabel.Size = new System.Drawing.Size(96, 20);
			this.m_ctrlMediaRootLabel.TabIndex = 21;
			this.m_ctrlMediaRootLabel.Text = "Media Root:";
			// 
			// m_ctrlCurrentPath
			// 
			this.m_ctrlCurrentPath.Location = new System.Drawing.Point(108, 8);
			this.m_ctrlCurrentPath.Name = "m_ctrlCurrentPath";
			this.m_ctrlCurrentPath.Size = new System.Drawing.Size(276, 28);
			this.m_ctrlCurrentPath.TabIndex = 20;
			// 
			// m_ctrlBrowse
			// 
			this.m_ctrlBrowse.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBrowse.ImageIndex = 0;
			this.m_ctrlBrowse.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowse.Location = new System.Drawing.Point(360, 84);
			this.m_ctrlBrowse.Name = "m_ctrlBrowse";
			this.m_ctrlBrowse.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowse.TabIndex = 1;
			this.m_ctrlBrowse.Click += new System.EventHandler(this.OnClickBrowse);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(308, 148);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(224, 148);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlCurrentPathLabel
			// 
			this.m_ctrlCurrentPathLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlCurrentPathLabel.Name = "m_ctrlCurrentPathLabel";
			this.m_ctrlCurrentPathLabel.Size = new System.Drawing.Size(96, 20);
			this.m_ctrlCurrentPathLabel.TabIndex = 23;
			this.m_ctrlCurrentPathLabel.Text = "Current Path:";
			// 
			// m_ctrlMediaRoot
			// 
			this.m_ctrlMediaRoot.Location = new System.Drawing.Point(108, 44);
			this.m_ctrlMediaRoot.Name = "m_ctrlMediaRoot";
			this.m_ctrlMediaRoot.Size = new System.Drawing.Size(276, 28);
			this.m_ctrlMediaRoot.TabIndex = 24;
			// 
			// m_ctrlWarning
			// 
			this.m_ctrlWarning.BackColor = System.Drawing.Color.Red;
			this.m_ctrlWarning.ForeColor = System.Drawing.Color.White;
			this.m_ctrlWarning.Location = new System.Drawing.Point(108, 112);
			this.m_ctrlWarning.Name = "m_ctrlWarning";
			this.m_ctrlWarning.Size = new System.Drawing.Size(276, 28);
			this.m_ctrlWarning.TabIndex = 25;
			this.m_ctrlWarning.Text = "Changing the path will change the folder in which the media files should be store" +
				"d.";
			this.m_ctrlWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlWarningLabel
			// 
			this.m_ctrlWarningLabel.BackColor = System.Drawing.Color.Red;
			this.m_ctrlWarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlWarningLabel.ForeColor = System.Drawing.Color.White;
			this.m_ctrlWarningLabel.Location = new System.Drawing.Point(8, 112);
			this.m_ctrlWarningLabel.Name = "m_ctrlWarningLabel";
			this.m_ctrlWarningLabel.Size = new System.Drawing.Size(100, 28);
			this.m_ctrlWarningLabel.TabIndex = 26;
			this.m_ctrlWarningLabel.Text = "WARNING !";
			this.m_ctrlWarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// CFPathEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(392, 179);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlWarningLabel);
			this.Controls.Add(this.m_ctrlWarning);
			this.Controls.Add(this.m_ctrlMediaRoot);
			this.Controls.Add(this.m_ctrlCurrentPathLabel);
			this.Controls.Add(this.m_ctrlRelativePathLabel);
			this.Controls.Add(this.m_ctrlRelativePath);
			this.Controls.Add(this.m_ctrlMediaRootLabel);
			this.Controls.Add(this.m_ctrlCurrentPath);
			this.Controls.Add(this.m_ctrlBrowse);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFPathEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Path Editor";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			string strCurrent = "";
			
			if((m_dxRecord != null) && (m_tmaxDatabase != null))
			{
				if(m_dxRecord.GetMediaLevel() == TmaxMediaLevels.Primary)
				{
					m_dxPrimary = (CDxPrimary)m_dxRecord;
					m_dxSecondary = null;
				}
				else if(m_dxRecord.GetMediaLevel() == TmaxMediaLevels.Secondary)
				{
					m_dxSecondary = (CDxSecondary)m_dxRecord;
					m_dxPrimary = m_dxSecondary.Primary;
				}
				
			}
			
			//	Initialize the controls
			if(m_dxPrimary != null)
			{
				if(m_dxSecondary != null)
				{
					m_tmaxDatabase.GetFolderPaths(m_dxSecondary, ref strCurrent, ref m_strMediaRoot);
					
					if(m_dxSecondary.AliasId > 0)
						m_strRelativePath = m_tmaxDatabase.GetAliasedPath(m_dxSecondary);
					else
						m_strRelativePath = m_dxSecondary.RelativePath;
				}
				else
				{
					m_tmaxDatabase.GetFolderPaths(m_dxPrimary, ref strCurrent, ref m_strMediaRoot);
					
					if(m_dxPrimary.AliasId > 0)
						m_strRelativePath = m_tmaxDatabase.GetAliasedPath(m_dxPrimary);
					else
						m_strRelativePath = m_dxPrimary.RelativePath;
				}
				
				if((m_strRelativePath.Length > 0) && (m_strRelativePath.EndsWith("\\") == false))
					m_strRelativePath += "\\";
				m_strRelativePath = m_strRelativePath.ToLower();
				
				m_ctrlCurrentPath.Text = strCurrent;
				m_ctrlRelativePath.Text = m_strRelativePath;
				m_ctrlMediaRoot.Text = m_strMediaRoot;

			}
			else
			{			
				m_ctrlRelativePath.Enabled = false;
				m_ctrlRelativePathLabel.Enabled = false;
				m_ctrlOk.Enabled = false;
			}
			
		}// private void OnLoad(object sender, System.EventArgs e)
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			string strPath = "";
			
			//	Get the new path
			strPath = m_ctrlRelativePath.Text;
			
			if(strPath.Length > 0)
			{
				//	Should we append the drive separator
				if(strPath.Length == 1)
					strPath += ":\\";
				else if(strPath.EndsWith("\\") == false)
					strPath += "\\";
			}
			
			//	Has the path changed?
			if(String.Compare(strPath, m_strRelativePath, true) == 0)
			{
				DialogResult = DialogResult.Cancel;
				this.Close();
			}
			else
			{
				//	First verify the new path
				if(m_tmaxDatabase.VerifyUserPath(m_dxRecord, strPath, false) == true)
				{
					//	Set the new path
					if(m_tmaxDatabase.SetUserPath(m_dxRecord, strPath) == true)
					{
						//	Update the record
						FireCommand(TmaxCommands.Update, new CTmaxItem(m_dxRecord));
					}
					
					//	Close the dialog
					DialogResult = DialogResult.OK;
					this.Close();
				}
				
			}
		
		}// private void OnClickOk(object sender, System.EventArgs e)
		
		/// <summary>This method is called when the user clicks on Browse</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickBrowse(object sender, System.EventArgs e)
		{
			int iIndex = 0;
			FTI.Shared.CBrowseForFolder browser = new CBrowseForFolder();
			
			browser.Folder = m_ctrlCurrentPath.Text;
			browser.Prompt = "Select the new linked folder : ";
			browser.NoNewFolder = false;
			
			if(browser.ShowDialog(this.Handle) == DialogResult.OK)
			{
				m_ctrlRelativePath.Text = browser.Folder.ToLower();
				if((m_ctrlRelativePath.Text.EndsWith("\\") == false) &&
					(m_ctrlRelativePath.Text.EndsWith("/") == false))
				{
					m_ctrlRelativePath.Text += "\\";
				}
				
				//	Is this path relative to the default media root?
				if((iIndex = m_ctrlRelativePath.Text.IndexOf(m_strMediaRoot)) == 0)
				{
					m_ctrlRelativePath.Text = m_ctrlRelativePath.Text.Substring(m_strMediaRoot.Length);
				}
				
			}
			
		}// private void OnClickBrowse(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}
		
		/// <summary>The active database record</summary>
		public CDxMediaRecord Record
		{
			get { return m_dxRecord; }
			set { m_dxRecord = value; }
		}
		
		#endregion Properties

	}// public class CFPathEditor : CFTmaxBaseForm

}// namespace FTI.Trialmax.Panes
