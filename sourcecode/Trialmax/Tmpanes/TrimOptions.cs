using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Database;

namespace FTI.Trialmax.Panes
{
	/// <summary>This form allows the user to set the options for trimming the active database</summary>
	public class CFTrimOptions : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants

		/// <summary>Error message identifiers</summary>
		private const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 1;

		#endregion Constants

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

		/// <summary>Name used to identify the new case</summary>
		private System.Windows.Forms.TextBox m_ctrlCaseFolder;

		/// <summary>Label for Name control</summary>
		private System.Windows.Forms.Label m_ctrlCaseFolderLabel;

		private System.Windows.Forms.CheckBox m_ctrlKeepScripts;
		private System.Windows.Forms.CheckBox m_ctrlKeepTreatments;
		private System.Windows.Forms.GroupBox m_ctrlMediaGroup;
		private System.Windows.Forms.TextBox m_ctrlLastDocument;
		private System.Windows.Forms.Label m_ctrlLastLabel;
		private System.Windows.Forms.TextBox m_ctrlFirstDocument;
		private System.Windows.Forms.Label m_ctrlFirstLabel;
		private System.Windows.Forms.CheckBox m_ctrlUseMasterRange;

		/// <summary>Local member bound to TrimOptions property</summary>
		private CTmaxTrimOptions m_tmaxTrimOptions = null;

		/// <summary>Local member bound to SourceFolder property</summary>
		private string m_strSourceFolder = "";
		private FTI.Trialmax.Panes.CBinderTree m_ctrlBinders;
		private System.Windows.Forms.Label m_ctrlBindersLabel;
		private System.Windows.Forms.CheckBox m_ctrlKeepFirstPage;
		private System.Windows.Forms.GroupBox m_ctrlMasterGroup;
		private System.Windows.Forms.GroupBox m_ctrlOverrideGroup;
		private System.Windows.Forms.CheckBox m_ctrlUnusedMediaRecords;

		/// <summary>Local member bound to Database property</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = null;
		
		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CFTrimOptions()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			this.EventSource.Name = "Trim Options Form";
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>Traps the event fired when the form gets loaded for the first time</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			//	Set the control values
			Exchange(false);
			
			try
			{
				//	Preselect all binder nodes
				if(m_ctrlBinders.TreeCtrl.Nodes != null)
				{
					foreach(CTmaxBaseTreeNode O in m_ctrlBinders.TreeCtrl.Nodes)
						O.CheckedState = CheckState.Checked;
				}

			}
			catch
			{
			}

		}// protected void OnLoad(object sender, System.EventArgs e)

		/// <summary>Traps the event fired when the user clicks on the Ok button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			//	Set the class members
			if(Exchange(true) == true)
			{
				DialogResult = DialogResult.OK;
				Close();

			}// if(Exchange(true) == true)

		}// protected void OnClickOk(object sender, System.EventArgs e)

		/// <summary>Traps the event fired when the user clicks on the Browse button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnBrowse(object sender, System.EventArgs e)
		{
			FTI.Shared.CBrowseForFolder bff = new CBrowseForFolder();

			bff.Folder = m_ctrlCaseFolder.Text;
			bff.Prompt = "Select the trimmed folder : ";
			bff.NoNewFolder = false;

			if (bff.ShowDialog(this.Handle) == DialogResult.OK)
			{
				m_ctrlCaseFolder.Text = bff.Folder.ToLower();
				if ((m_ctrlCaseFolder.Text.EndsWith("\\") == false) &&
					(m_ctrlCaseFolder.Text.EndsWith("/") == false))
				{
					m_ctrlCaseFolder.Text += "\\";
				}
			}

		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if (m_tmaxErrorBuilder == null) return;
			if (m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Let the base class add its strings first
			base.SetErrorStrings();

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the class members with the child controls: SetMembers = %1");

		}// protected override void SetErrorStrings()

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFTrimOptions));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlBrowse = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlCaseFolder = new System.Windows.Forms.TextBox();
			this.m_ctrlCaseFolderLabel = new System.Windows.Forms.Label();
			this.m_ctrlKeepScripts = new System.Windows.Forms.CheckBox();
			this.m_ctrlKeepTreatments = new System.Windows.Forms.CheckBox();
			this.m_ctrlMediaGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlBinders = new FTI.Trialmax.Panes.CBinderTree();
			this.m_ctrlBindersLabel = new System.Windows.Forms.Label();
			this.m_ctrlMasterGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlKeepFirstPage = new System.Windows.Forms.CheckBox();
			this.m_ctrlLastDocument = new System.Windows.Forms.TextBox();
			this.m_ctrlLastLabel = new System.Windows.Forms.Label();
			this.m_ctrlFirstDocument = new System.Windows.Forms.TextBox();
			this.m_ctrlFirstLabel = new System.Windows.Forms.Label();
			this.m_ctrlUseMasterRange = new System.Windows.Forms.CheckBox();
			this.m_ctrlOverrideGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlUnusedMediaRecords = new System.Windows.Forms.CheckBox();
			this.m_ctrlMediaGroup.SuspendLayout();
			this.m_ctrlMasterGroup.SuspendLayout();
			this.m_ctrlOverrideGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(338, 326);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 5;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(248, 326);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 4;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlBrowse
			// 
			this.m_ctrlBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBrowse.ImageIndex = 0;
			this.m_ctrlBrowse.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowse.Location = new System.Drawing.Point(404, 10);
			this.m_ctrlBrowse.Name = "m_ctrlBrowse";
			this.m_ctrlBrowse.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowse.TabIndex = 1;
			this.m_ctrlBrowse.Click += new System.EventHandler(this.OnBrowse);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlImages.Images.SetKeyName(0, "");
			// 
			// m_ctrlCaseFolder
			// 
			this.m_ctrlCaseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseFolder.Location = new System.Drawing.Point(109, 10);
			this.m_ctrlCaseFolder.Name = "m_ctrlCaseFolder";
			this.m_ctrlCaseFolder.Size = new System.Drawing.Size(285, 20);
			this.m_ctrlCaseFolder.TabIndex = 0;
			// 
			// m_ctrlCaseFolderLabel
			// 
			this.m_ctrlCaseFolderLabel.Location = new System.Drawing.Point(11, 9);
			this.m_ctrlCaseFolderLabel.Name = "m_ctrlCaseFolderLabel";
			this.m_ctrlCaseFolderLabel.Size = new System.Drawing.Size(92, 20);
			this.m_ctrlCaseFolderLabel.TabIndex = 15;
			this.m_ctrlCaseFolderLabel.Text = "Trimmed Folder : ";
			this.m_ctrlCaseFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlKeepScripts
			// 
			this.m_ctrlKeepScripts.Location = new System.Drawing.Point(10, 24);
			this.m_ctrlKeepScripts.Name = "m_ctrlKeepScripts";
			this.m_ctrlKeepScripts.Size = new System.Drawing.Size(145, 17);
			this.m_ctrlKeepScripts.TabIndex = 0;
			this.m_ctrlKeepScripts.Text = "Keep All Scripts";
			// 
			// m_ctrlKeepTreatments
			// 
			this.m_ctrlKeepTreatments.Location = new System.Drawing.Point(10, 45);
			this.m_ctrlKeepTreatments.Name = "m_ctrlKeepTreatments";
			this.m_ctrlKeepTreatments.Size = new System.Drawing.Size(145, 17);
			this.m_ctrlKeepTreatments.TabIndex = 1;
			this.m_ctrlKeepTreatments.Text = "Keep All Treatments";
			// 
			// m_ctrlMediaGroup
			// 
			this.m_ctrlMediaGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMediaGroup.Controls.Add(this.m_ctrlKeepTreatments);
			this.m_ctrlMediaGroup.Controls.Add(this.m_ctrlKeepScripts);
			this.m_ctrlMediaGroup.Controls.Add(this.m_ctrlBinders);
			this.m_ctrlMediaGroup.Controls.Add(this.m_ctrlBindersLabel);
			this.m_ctrlMediaGroup.Location = new System.Drawing.Point(10, 40);
			this.m_ctrlMediaGroup.Name = "m_ctrlMediaGroup";
			this.m_ctrlMediaGroup.Size = new System.Drawing.Size(219, 309);
			this.m_ctrlMediaGroup.TabIndex = 2;
			this.m_ctrlMediaGroup.TabStop = false;
			this.m_ctrlMediaGroup.Text = "Media Options";
			// 
			// m_ctrlBinders
			// 
			this.m_ctrlBinders.PaneVisible = false;
			this.m_ctrlBinders.AllowDrop = true;
			this.m_ctrlBinders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBinders.ApplicationOptions = null;
			this.m_ctrlBinders.AsyncCommandArgs = null;
			this.m_ctrlBinders.CaseCodes = null;
			this.m_ctrlBinders.CaseOptions = null;
			this.m_ctrlBinders.CheckBoxes = true;
			this.m_ctrlBinders.Database = null;
			this.m_ctrlBinders.EnableContextMenu = false;
			this.m_ctrlBinders.EnableDragDrop = false;
			this.m_ctrlBinders.Filtered = null;
			this.m_ctrlBinders.Location = new System.Drawing.Point(10, 90);
			this.m_ctrlBinders.MediaTypes = null;
			this.m_ctrlBinders.Name = "m_ctrlBinders";
			this.m_ctrlBinders.PaneId = 0;
			this.m_ctrlBinders.PaneName = "";
			this.m_ctrlBinders.PresentationOptionsFilename = "";
			this.m_ctrlBinders.PrimaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.MediaId;
			this.m_ctrlBinders.QuaternaryTextMode = ((FTI.Shared.Trialmax.TmaxTextModes)((FTI.Shared.Trialmax.TmaxTextModes.Barcode | FTI.Shared.Trialmax.TmaxTextModes.Name)));
			this.m_ctrlBinders.RegistrationOptions = null;
			this.m_ctrlBinders.ReportManager = null;
			this.m_ctrlBinders.SecondaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_ctrlBinders.Size = new System.Drawing.Size(199, 209);
			this.m_ctrlBinders.SourceTypes = null;
			this.m_ctrlBinders.StationOptions = null;
			this.m_ctrlBinders.SuperNodeSize = 0;
			this.m_ctrlBinders.TabIndex = 2;
			this.m_ctrlBinders.TertiaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_ctrlBinders.TmaxClipboard = null;
			this.m_ctrlBinders.TmaxProductManager = null;
			this.m_ctrlBinders.TmaxRegistry = null;
			// 
			// m_ctrlBindersLabel
			// 
			this.m_ctrlBindersLabel.Location = new System.Drawing.Point(10, 70);
			this.m_ctrlBindersLabel.Name = "m_ctrlBindersLabel";
			this.m_ctrlBindersLabel.Size = new System.Drawing.Size(105, 20);
			this.m_ctrlBindersLabel.TabIndex = 17;
			this.m_ctrlBindersLabel.Text = "Select Binders : ";
			this.m_ctrlBindersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMasterGroup
			// 
			this.m_ctrlMasterGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMasterGroup.Controls.Add(this.m_ctrlKeepFirstPage);
			this.m_ctrlMasterGroup.Controls.Add(this.m_ctrlLastDocument);
			this.m_ctrlMasterGroup.Controls.Add(this.m_ctrlLastLabel);
			this.m_ctrlMasterGroup.Controls.Add(this.m_ctrlFirstDocument);
			this.m_ctrlMasterGroup.Controls.Add(this.m_ctrlFirstLabel);
			this.m_ctrlMasterGroup.Controls.Add(this.m_ctrlUseMasterRange);
			this.m_ctrlMasterGroup.Location = new System.Drawing.Point(239, 40);
			this.m_ctrlMasterGroup.Name = "m_ctrlMasterGroup";
			this.m_ctrlMasterGroup.Size = new System.Drawing.Size(192, 145);
			this.m_ctrlMasterGroup.TabIndex = 3;
			this.m_ctrlMasterGroup.TabStop = false;
			this.m_ctrlMasterGroup.Text = "Master Document Range";
			// 
			// m_ctrlKeepFirstPage
			// 
			this.m_ctrlKeepFirstPage.Location = new System.Drawing.Point(22, 50);
			this.m_ctrlKeepFirstPage.Name = "m_ctrlKeepFirstPage";
			this.m_ctrlKeepFirstPage.Size = new System.Drawing.Size(158, 17);
			this.m_ctrlKeepFirstPage.TabIndex = 29;
			this.m_ctrlKeepFirstPage.Text = "Always Keep First Page";
			// 
			// m_ctrlLastDocument
			// 
			this.m_ctrlLastDocument.Location = new System.Drawing.Point(55, 105);
			this.m_ctrlLastDocument.Name = "m_ctrlLastDocument";
			this.m_ctrlLastDocument.Size = new System.Drawing.Size(90, 20);
			this.m_ctrlLastDocument.TabIndex = 2;
			// 
			// m_ctrlLastLabel
			// 
			this.m_ctrlLastLabel.AutoSize = true;
			this.m_ctrlLastLabel.Location = new System.Drawing.Point(20, 110);
			this.m_ctrlLastLabel.Name = "m_ctrlLastLabel";
			this.m_ctrlLastLabel.Size = new System.Drawing.Size(30, 13);
			this.m_ctrlLastLabel.TabIndex = 28;
			this.m_ctrlLastLabel.Text = "Last:";
			// 
			// m_ctrlFirstDocument
			// 
			this.m_ctrlFirstDocument.Location = new System.Drawing.Point(55, 80);
			this.m_ctrlFirstDocument.Name = "m_ctrlFirstDocument";
			this.m_ctrlFirstDocument.Size = new System.Drawing.Size(90, 20);
			this.m_ctrlFirstDocument.TabIndex = 1;
			// 
			// m_ctrlFirstLabel
			// 
			this.m_ctrlFirstLabel.AutoSize = true;
			this.m_ctrlFirstLabel.Location = new System.Drawing.Point(20, 80);
			this.m_ctrlFirstLabel.Name = "m_ctrlFirstLabel";
			this.m_ctrlFirstLabel.Size = new System.Drawing.Size(29, 13);
			this.m_ctrlFirstLabel.TabIndex = 26;
			this.m_ctrlFirstLabel.Text = "First:";
			// 
			// m_ctrlUseMasterRange
			// 
			this.m_ctrlUseMasterRange.Location = new System.Drawing.Point(22, 24);
			this.m_ctrlUseMasterRange.Name = "m_ctrlUseMasterRange";
			this.m_ctrlUseMasterRange.Size = new System.Drawing.Size(135, 17);
			this.m_ctrlUseMasterRange.TabIndex = 0;
			this.m_ctrlUseMasterRange.Text = "Use Master Range";
			this.m_ctrlUseMasterRange.Click += new System.EventHandler(this.OnClickUseMaster);
			// 
			// m_ctrlOverrideGroup
			// 
			this.m_ctrlOverrideGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOverrideGroup.Controls.Add(this.m_ctrlUnusedMediaRecords);
			this.m_ctrlOverrideGroup.Location = new System.Drawing.Point(239, 195);
			this.m_ctrlOverrideGroup.Name = "m_ctrlOverrideGroup";
			this.m_ctrlOverrideGroup.Size = new System.Drawing.Size(192, 83);
			this.m_ctrlOverrideGroup.TabIndex = 16;
			this.m_ctrlOverrideGroup.TabStop = false;
			this.m_ctrlOverrideGroup.Text = "Override Options";
			// 
			// m_ctrlUnusedMediaRecords
			// 
			this.m_ctrlUnusedMediaRecords.Location = new System.Drawing.Point(22, 25);
			this.m_ctrlUnusedMediaRecords.Name = "m_ctrlUnusedMediaRecords";
			this.m_ctrlUnusedMediaRecords.Size = new System.Drawing.Size(153, 30);
			this.m_ctrlUnusedMediaRecords.TabIndex = 30;
			this.m_ctrlUnusedMediaRecords.Text = "Don\'t delete unused media records";
			// 
			// CFTrimOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(436, 357);
			this.Controls.Add(this.m_ctrlOverrideGroup);
			this.Controls.Add(this.m_ctrlMasterGroup);
			this.Controls.Add(this.m_ctrlMediaGroup);
			this.Controls.Add(this.m_ctrlCaseFolderLabel);
			this.Controls.Add(this.m_ctrlCaseFolder);
			this.Controls.Add(this.m_ctrlBrowse);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFTrimOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Trim Options";
			this.Load += new System.EventHandler(this.OnLoad);
			this.m_ctrlMediaGroup.ResumeLayout(false);
			this.m_ctrlMasterGroup.ResumeLayout(false);
			this.m_ctrlMasterGroup.PerformLayout();
			this.m_ctrlOverrideGroup.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion Protected Methods

		#region Private Methods

		/// <summary>Called to get the case folder specified by the user</summary>
		/// <returns>True if successful</returns>
		private bool GetCaseFolder()
		{
			bool	bSuccessful = false;
			string	strFolder = "";
			string	strFileSpec = "";

			if((m_strSourceFolder.Length > 0) && (m_strSourceFolder.EndsWith("\\") == false))
				m_strSourceFolder += "\\";

			while(bSuccessful == false)
			{
				//	Get the folder specified by the user
				strFolder = m_ctrlCaseFolder.Text;
				if(strFolder.Length == 0)
				{
					MessageBox.Show("You must specified a folder for the trimmed database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					m_ctrlCaseFolder.Focus();
					break;
				}

				//	Make sure this is not the source folder
				if(m_strSourceFolder.Length > 0)
				{
					if(strFolder.EndsWith("\\") == false)
						strFolder += "\\";
		
					if(String.Compare(m_strSourceFolder, strFolder, true) == 0)
					{
						MessageBox.Show("The trimmed database can not but put in the folder with the source database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						m_ctrlCaseFolder.Focus();
						break;
					}

				}// if(m_strSourceFolder.Length > 0)

				//	Does this folder exist?
				if(System.IO.Directory.Exists(strFolder) == false)
				{
					//	Attempt to create the folder
					try
					{
						System.IO.Directory.CreateDirectory(strFolder);
					}
					catch
					{
						MessageBox.Show("Unable to create " + strFolder + " to store the trimmed database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						m_ctrlCaseFolder.Focus();
						break;
					}

				}
				else
				{
					//	Check to see if this folder already contains a database
					strFileSpec = strFolder;
					if(strFileSpec.EndsWith("\\") == false)
						strFileSpec += "\\";
					strFileSpec += "_tmax_case.mdb";
	
					if(System.IO.File.Exists(strFileSpec) == true)
					{
						if(MessageBox.Show(strFolder + " already contains a TrialMax database. It will be overwritten with the trimmed database.\n\nDo you want to continue?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						{
							break;
						}
						else
						{
							try
							{
								System.IO.File.Delete(strFileSpec);
							}
							catch
							{
								MessageBox.Show("Unable to delete " + strFileSpec, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
								break;
							}

						}// if(MessageBox.Show(strFolder + " already contains a TrialMax database. It will be overwritten with the trimmed database.\n\nDo you want to continue?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)

					}// if(System.IO.File.Exists(strFileSpec) == true)
		
				}// if(System.IO.Directory.Exists(strFolder) == false)

				//	Store the folder
				m_tmaxTrimOptions.CaseFolder = strFolder;

				bSuccessful = true;

			}// while(bSuccessful == false)
	
			return bSuccessful;

		}// private bool GetCaseFolder()

		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetMembers)
		{
			bool	bSuccessful = false;
			long	lFirst = 0;
			long	lLast = 0;

			Debug.Assert(m_tmaxTrimOptions != null);
			if (m_tmaxTrimOptions == null) return false;

			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					GetBinderSelections(m_tmaxTrimOptions.BinderSelections, m_ctrlBinders.TreeCtrl.Nodes);

					//	Make sure the user has checked at least one media option
					if((m_ctrlKeepScripts.Checked == false) &&
						(m_ctrlKeepTreatments.Checked == false) &&
						(m_tmaxTrimOptions.BinderSelections.Count == 0 ))
					{
						MessageBox.Show("You must select at least one of the media options or binders", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}
	
					//	Get the folder for the trimmed database
					if(GetCaseFolder() == false)
						return false;
		
					//	Is the user requesting the master document range?
					if(m_ctrlUseMasterRange.Checked == true)
					{
						try { lFirst = System.Convert.ToInt64(m_ctrlFirstDocument.Text); }
						catch { lFirst = 0; }
						if(lFirst <= 0)	
							return Warn("You must supply a valid first document number if using the master range", m_ctrlFirstDocument);

						try { lLast = System.Convert.ToInt64(m_ctrlLastDocument.Text); }
						catch { lLast = 0; }
						if(lLast <= 0)
							return Warn("You must supply a valid last document number if using the master range", m_ctrlLastDocument);
		
						if(lLast < lFirst)
							return Warn("The last document number must be greater than or equal to the first document number", m_ctrlLastDocument);
		
						m_tmaxTrimOptions.UseMasterRange = true;
						m_tmaxTrimOptions.FirstDocument = lFirst;
						m_tmaxTrimOptions.LastDocument = lLast;

					}
					else
					{
						m_tmaxTrimOptions.UseMasterRange = false;
						m_tmaxTrimOptions.FirstDocument = 0;
						m_tmaxTrimOptions.LastDocument = 0;
					}
	
					//	Save the media options
					m_tmaxTrimOptions.KeepFirstPage = m_ctrlKeepFirstPage.Checked;
					m_tmaxTrimOptions.KeepScripts = m_ctrlKeepScripts.Checked;
					m_tmaxTrimOptions.KeepTreatments = m_ctrlKeepTreatments.Checked;
		
					m_tmaxTrimOptions.UnusedMediaRecords = m_ctrlUnusedMediaRecords.Checked;
				}
				else
				{
					m_ctrlBinders.Database = m_tmaxDatabase;
					m_ctrlCaseFolder.Text = m_tmaxTrimOptions.CaseFolder;
					m_ctrlKeepScripts.Checked = m_tmaxTrimOptions.KeepScripts;
					m_ctrlKeepTreatments.Checked = m_tmaxTrimOptions.KeepTreatments;
					m_ctrlKeepFirstPage.Checked = m_tmaxTrimOptions.KeepFirstPage;
					m_ctrlUseMasterRange.Checked = m_tmaxTrimOptions.UseMasterRange;
					m_ctrlUnusedMediaRecords.Checked = m_tmaxTrimOptions.UnusedMediaRecords;
	
					if(m_tmaxTrimOptions.FirstDocument > 0)
						m_ctrlFirstDocument.Text = m_tmaxTrimOptions.FirstDocument.ToString();
					else
						m_ctrlFirstDocument.Text = "";

					if(m_tmaxTrimOptions.LastDocument > 0)
						m_ctrlLastDocument.Text = m_tmaxTrimOptions.LastDocument.ToString();
					else
						m_ctrlLastDocument.Text = "";
	
					SetControlStates();
	
				}// if(bSetMembers == true)

				bSuccessful = true;
			}
			catch (System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetMembers), Ex);
			}

			return bSuccessful;

		}// private bool Exchange(bool bSetMembers)

		/// <summary>Called to enable disable the child controls</summary>
		private void SetControlStates()
		{
			if(m_ctrlUseMasterRange.Checked == true)
			{
				m_ctrlFirstLabel.Enabled = true;
				m_ctrlFirstDocument.Enabled = true;
				m_ctrlLastLabel.Enabled = true;
				m_ctrlLastDocument.Enabled = true;
				m_ctrlKeepFirstPage.Enabled = true;
			}
			else
			{
				m_ctrlFirstLabel.Enabled = false;
				m_ctrlFirstDocument.Enabled = false;
				m_ctrlLastLabel.Enabled = false;
				m_ctrlLastDocument.Enabled = false;
				m_ctrlKeepFirstPage.Enabled = false;
			}
			
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Binders != null) && (m_tmaxDatabase.Binders.Count > 0))
			{
				m_ctrlBindersLabel.Enabled = true;
				m_ctrlBinders.Enabled = true;
			}
			else
			{
				m_ctrlBindersLabel.Enabled = false;
				m_ctrlBinders.Enabled = false;
			}

		}// private void SetControlStates()

		/// <summary>Called when the user clicks on the Use Master Range check box</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		private void OnClickUseMaster(object sender, EventArgs e)
		{
			SetControlStates();
		}

		/// <summary>Called when the user clicks on the Keep Binders check box</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		private void OnClickKeepBinders(object sender, EventArgs e)
		{
			SetControlStates();
		}

		/// <summary>Called to get the binders selected by the user</summary>
		/// <param name="tmaxSelections">The collection in which to store the selected nodes</param>
		/// <param name="ultraNodes">The binder tree nodes to test</param>
		/// <returns>true if there is at least one media node in the specified collection</returns>
		private void GetBinderSelections(CTmaxItems tmaxSelections, Infragistics.Win.UltraWinTree.TreeNodesCollection ultraNodes)
		{
			CTmaxItem tmaxBinder = null;
			
			if((ultraNodes != null) && (ultraNodes.Count > 0))
			{
				foreach(CTmaxMediaTreeNode O in ultraNodes)
				{
					//	Don't bother if this is not a binder entry
					if(O.IBinder == null) continue;
					
					//	Add this node if it is a media record or a selected subbinder
					if((O.GetMediaRecord() != null) || (O.CheckedState == CheckState.Checked))
					{
						tmaxBinder = new CTmaxItem();
						
						if(O.GetMediaRecord() != null)
							tmaxBinder.SetRecord(O.GetMediaRecord());
						tmaxBinder.IBinderEntry = O.IBinder;
						
						//	Check the children if this node has been expanded and is not a media node
						if((O.IPrimary == null) && (O.Nodes != null) && (O.Nodes.Count > 0))
						{
							GetBinderSelections(tmaxBinder.SubItems, O.Nodes);
							
							//	Throw away if all children were turned off
							if(tmaxBinder.SubItems.Count == 0)
								tmaxBinder = null;
							
						}// if((O.Nodes != null) && (O.Nodes.Count > 0))

						//	Add to the caller's collection
						if(tmaxBinder != null)
							tmaxSelections.Add(tmaxBinder);						
					}
					
				}// foreach(CTmaxMediaTreeNode O in ultraNodes)
			
			}// if((ultraNodes != null) && (ultraNodes.Count > 0))
		
		}// private void GetBinderSelections(CTmaxItems tmaxSelections, Infragistics.Win.UltraWinTree.TreeNodesCollection ultraNodes)

/*
		private bool GetBinderSelections(CTmaxItems tmaxSelections, Infragistics.Win.UltraWinTree.TreeNodesCollection ultraNodes)
		{
			CTmaxItem	tmaxBinder = null;
			bool		bHasMedia = false;
			
			if(ultraNodes == null) return false;
			if(ultraNodes.Count == 0) return false;
			
			foreach(CTmaxMediaTreeNode O in ultraNodes)
			{
				//	Don't bother if this is not a pure binder entry
				if((O.IBinder == null) || (O.IPrimary != null))
				{
					if(O.IPrimary != null)
						bHasMedia = true;
				}
				else 
				{
					if(O.CheckedState == System.Windows.Forms.CheckState.Checked)
					{
						tmaxBinder = new CTmaxItem();
						tmaxBinder.IBinderEntry = O.IBinder;
						tmaxBinder.DataType = TmaxDataTypes.Binder;
						
						//	Check the children if this node has been expanded
						if((O.Nodes != null) && (O.Nodes.Count > 0))
						{
							if(GetBinderSelections(tmaxBinder.SubItems, O.Nodes) == false)
							{							
								//	Throw away if all children were turned off and it does
								//	not contain any media nodes
								if(tmaxBinder.SubItems.Count == 0)
									tmaxBinder = null;
							}
							
						}// if((O.Nodes != null) && (O.Nodes.Count > 0))

						//	Add to the caller's collection
						if(tmaxBinder != null)
							tmaxSelections.Add(tmaxBinder);						
					
					}// if(O.CheckedState == System.Windows.Forms.CheckState.Checked)
					
				}// if((O.IBinder == null) || (O.IPrimary != null)) 
					
			}// foreach(CTmaxMediaTreeNode O in ultraNodes)
			
			return bHasMedia;
		
		}// private void GetBinderSelections(CTmaxItems tmaxSelections, Infragistics.Win.UltraWinTree.TreeNodesCollection ultraNodes)
*/
		#endregion Private Methods

		#region Properties

		/// <summary>Options used to trim the active database</summary>
		public FTI.Shared.Trialmax.CTmaxTrimOptions Options
		{
			get { return m_tmaxTrimOptions; }
			set { m_tmaxTrimOptions = value; }
		}

		/// <summary>Path to the folder containing the source database</summary>
		public string SourceFolder
		{
			get { return m_strSourceFolder; }
			set { m_strSourceFolder = value; }
		}

		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}

		#endregion Properties

	}// public class CFTrimOptions : CFTmaxBaseForm

}// namespace FTI.Trialmax.Panes
