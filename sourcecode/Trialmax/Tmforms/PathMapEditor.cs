using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Forms
{
	/// <summary>Form that allows the user to configure the case options</summary>
	public class CFPathMapEditor : CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Local member used to manage the child path editors</summary>
		private ArrayList m_aPathEditors = new ArrayList();
		
		/// <summary>Local member bound to PathMaps property</summary>
		private CTmaxPathMaps m_tmaxPathMaps = null;
		
		/// <summary>Local member bound to PathMap property</summary>
		private CTmaxPathMap m_tmaxPathMap = null;
		
		/// <summary>Local member bound to CaseFolder property</summary>
		private string m_strCaseFolder = "";
		
		/// <summary>Local member bound to ShowWarning property</summary>
		private bool m_bShowWarning = true;
		
		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>The edit box used to enter the map name</summary>
		private System.Windows.Forms.TextBox m_ctrlName;

		/// <summary>The label for the map Name edit box</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;
		
		/// <summary>The label for the warning text</summary>
		private System.Windows.Forms.Label m_ctrlWarningLabel;
		
		/// <summary>Warning text line 1</summary>
		private System.Windows.Forms.Label m_ctrlWarningLine1;
		
		/// <summary>Warning text line 2</summary>
		private System.Windows.Forms.Label m_ctrlWarningLine2;
		
		/// <summary>Group box for the child path override editors</summary>
		private System.Windows.Forms.GroupBox m_ctrlFoldersGroup;
		
		/// <summary>Path override editor for Videos</summary>
		private FTI.Trialmax.Controls.CTmaxPathOverrideCtrl m_ctrlVideos;
		
		/// <summary>Path override editor for Recordings</summary>
		private FTI.Trialmax.Controls.CTmaxPathOverrideCtrl m_ctrlRecordings;
		
		/// <summary>Path override editor for PowerPoint</summary>
		private FTI.Trialmax.Controls.CTmaxPathOverrideCtrl m_ctrlPowerPoints;
		
		/// <summary>Path override editor for Documents</summary>
		private FTI.Trialmax.Controls.CTmaxPathOverrideCtrl m_ctrlDocuments;

        /// <summary>Path override editor for Objections</summary>
        private CTmaxPathOverrideCtrl m_ctrlObjections;
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFPathMapEditor()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			//	Store the editors in an array to make processing easier
			m_aPathEditors.Add(m_ctrlDocuments);
			m_aPathEditors.Add(m_ctrlPowerPoints);
			m_aPathEditors.Add(m_ctrlRecordings);
			m_aPathEditors.Add(m_ctrlVideos);
            m_aPathEditors.Add(m_ctrlObjections);
			
			//	Connect the editors' event sources to the local event source
			foreach(CTmaxPathOverrideCtrl O in m_aPathEditors)
			{
				O.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.m_tmaxEventSource.OnError);
				O.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.m_tmaxEventSource.OnDiagnostic);
			}
		
		}// public CFPathMapEditor()

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
				
				if(m_aPathEditors != null)
				{
					m_aPathEditors.Clear();
					m_aPathEditors = null;
				}
			}
			base.Dispose( disposing );
		
		}// protected override void Dispose( bool disposing )
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFPathMapEditor));
            this.m_ctrlCancel = new System.Windows.Forms.Button();
            this.m_ctrlOk = new System.Windows.Forms.Button();
            this.m_ctrlNameLabel = new System.Windows.Forms.Label();
            this.m_ctrlName = new System.Windows.Forms.TextBox();
            this.m_ctrlWarningLabel = new System.Windows.Forms.Label();
            this.m_ctrlWarningLine1 = new System.Windows.Forms.Label();
            this.m_ctrlWarningLine2 = new System.Windows.Forms.Label();
            this.m_ctrlFoldersGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlVideos = new FTI.Trialmax.Controls.CTmaxPathOverrideCtrl();
            this.m_ctrlRecordings = new FTI.Trialmax.Controls.CTmaxPathOverrideCtrl();
            this.m_ctrlPowerPoints = new FTI.Trialmax.Controls.CTmaxPathOverrideCtrl();
            this.m_ctrlDocuments = new FTI.Trialmax.Controls.CTmaxPathOverrideCtrl();
            this.m_ctrlObjections = new FTI.Trialmax.Controls.CTmaxPathOverrideCtrl();
            this.m_ctrlFoldersGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ctrlCancel
            // 
            this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlCancel.Location = new System.Drawing.Point(364,276);
            this.m_ctrlCancel.Name = "m_ctrlCancel";
            this.m_ctrlCancel.Size = new System.Drawing.Size(75,23);
            this.m_ctrlCancel.TabIndex = 3;
            this.m_ctrlCancel.Text = "  &Cancel";
            this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
            // 
            // m_ctrlOk
            // 
            this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlOk.Location = new System.Drawing.Point(284,276);
            this.m_ctrlOk.Name = "m_ctrlOk";
            this.m_ctrlOk.Size = new System.Drawing.Size(75,23);
            this.m_ctrlOk.TabIndex = 2;
            this.m_ctrlOk.Text = "&OK";
            this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
            // 
            // m_ctrlNameLabel
            // 
            this.m_ctrlNameLabel.Location = new System.Drawing.Point(16,8);
            this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
            this.m_ctrlNameLabel.Size = new System.Drawing.Size(112,20);
            this.m_ctrlNameLabel.TabIndex = 11;
            this.m_ctrlNameLabel.Text = "Path Map Name:";
            this.m_ctrlNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlName
            // 
            this.m_ctrlName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlName.Location = new System.Drawing.Point(132,8);
            this.m_ctrlName.Name = "m_ctrlName";
            this.m_ctrlName.Size = new System.Drawing.Size(304,20);
            this.m_ctrlName.TabIndex = 0;
            // 
            // m_ctrlWarningLabel
            // 
            this.m_ctrlWarningLabel.BackColor = System.Drawing.Color.Red;
            this.m_ctrlWarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif",9.75F,((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))),System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.m_ctrlWarningLabel.ForeColor = System.Drawing.Color.White;
            this.m_ctrlWarningLabel.Location = new System.Drawing.Point(8,234);
            this.m_ctrlWarningLabel.Name = "m_ctrlWarningLabel";
            this.m_ctrlWarningLabel.Size = new System.Drawing.Size(88,32);
            this.m_ctrlWarningLabel.TabIndex = 28;
            this.m_ctrlWarningLabel.Text = "WARNING !";
            this.m_ctrlWarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_ctrlWarningLine1
            // 
            this.m_ctrlWarningLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlWarningLine1.BackColor = System.Drawing.Color.Red;
            this.m_ctrlWarningLine1.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.m_ctrlWarningLine1.ForeColor = System.Drawing.Color.White;
            this.m_ctrlWarningLine1.Location = new System.Drawing.Point(92,234);
            this.m_ctrlWarningLine1.Name = "m_ctrlWarningLine1";
            this.m_ctrlWarningLine1.Size = new System.Drawing.Size(348,16);
            this.m_ctrlWarningLine1.TabIndex = 27;
            this.m_ctrlWarningLine1.Text = "Editing paths in a map affects all machines using that map !";
            this.m_ctrlWarningLine1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_ctrlWarningLine2
            // 
            this.m_ctrlWarningLine2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlWarningLine2.BackColor = System.Drawing.Color.Red;
            this.m_ctrlWarningLine2.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.m_ctrlWarningLine2.ForeColor = System.Drawing.Color.White;
            this.m_ctrlWarningLine2.Location = new System.Drawing.Point(92,250);
            this.m_ctrlWarningLine2.Name = "m_ctrlWarningLine2";
            this.m_ctrlWarningLine2.Size = new System.Drawing.Size(348,16);
            this.m_ctrlWarningLine2.TabIndex = 29;
            this.m_ctrlWarningLine2.Text = "Changes are saved when you click OK";
            this.m_ctrlWarningLine2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_ctrlFoldersGroup
            // 
            this.m_ctrlFoldersGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlFoldersGroup.Controls.Add(this.m_ctrlObjections);
            this.m_ctrlFoldersGroup.Controls.Add(this.m_ctrlVideos);
            this.m_ctrlFoldersGroup.Controls.Add(this.m_ctrlRecordings);
            this.m_ctrlFoldersGroup.Controls.Add(this.m_ctrlPowerPoints);
            this.m_ctrlFoldersGroup.Controls.Add(this.m_ctrlDocuments);
            this.m_ctrlFoldersGroup.Location = new System.Drawing.Point(8,40);
            this.m_ctrlFoldersGroup.Name = "m_ctrlFoldersGroup";
            this.m_ctrlFoldersGroup.Size = new System.Drawing.Size(432,188);
            this.m_ctrlFoldersGroup.TabIndex = 1;
            this.m_ctrlFoldersGroup.TabStop = false;
            this.m_ctrlFoldersGroup.Text = "Case Folders";
            // 
            // m_ctrlVideos
            // 
            this.m_ctrlVideos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlVideos.CasePath = "";
            this.m_ctrlVideos.Folder = FTI.Shared.Trialmax.TmaxCaseFolders.Videos;
            this.m_ctrlVideos.Location = new System.Drawing.Point(8,117);
            this.m_ctrlVideos.Name = "m_ctrlVideos";
            this.m_ctrlVideos.Override = "";
            this.m_ctrlVideos.Size = new System.Drawing.Size(420,28);
            this.m_ctrlVideos.TabIndex = 3;
            this.m_ctrlVideos.Tag = "";
            // 
            // m_ctrlRecordings
            // 
            this.m_ctrlRecordings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlRecordings.CasePath = "";
            this.m_ctrlRecordings.Folder = FTI.Shared.Trialmax.TmaxCaseFolders.Recordings;
            this.m_ctrlRecordings.Location = new System.Drawing.Point(8,86);
            this.m_ctrlRecordings.Name = "m_ctrlRecordings";
            this.m_ctrlRecordings.Override = "";
            this.m_ctrlRecordings.Size = new System.Drawing.Size(420,28);
            this.m_ctrlRecordings.TabIndex = 2;
            this.m_ctrlRecordings.Tag = "";
            // 
            // m_ctrlPowerPoints
            // 
            this.m_ctrlPowerPoints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlPowerPoints.CasePath = "";
            this.m_ctrlPowerPoints.Folder = FTI.Shared.Trialmax.TmaxCaseFolders.PowerPoints;
            this.m_ctrlPowerPoints.Location = new System.Drawing.Point(8,55);
            this.m_ctrlPowerPoints.Name = "m_ctrlPowerPoints";
            this.m_ctrlPowerPoints.Override = "";
            this.m_ctrlPowerPoints.Size = new System.Drawing.Size(420,28);
            this.m_ctrlPowerPoints.TabIndex = 1;
            this.m_ctrlPowerPoints.Tag = "";
            // 
            // m_ctrlDocuments
            // 
            this.m_ctrlDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlDocuments.CasePath = "";
            this.m_ctrlDocuments.Folder = FTI.Shared.Trialmax.TmaxCaseFolders.Documents;
            this.m_ctrlDocuments.Location = new System.Drawing.Point(8,24);
            this.m_ctrlDocuments.Name = "m_ctrlDocuments";
            this.m_ctrlDocuments.Override = "";
            this.m_ctrlDocuments.Size = new System.Drawing.Size(420,28);
            this.m_ctrlDocuments.TabIndex = 0;
            this.m_ctrlDocuments.Tag = "";
            // 
            // m_ctrlObjections
            // 
            this.m_ctrlObjections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlObjections.CasePath = "";
            this.m_ctrlObjections.Folder = FTI.Shared.Trialmax.TmaxCaseFolders.Objections;
            this.m_ctrlObjections.Location = new System.Drawing.Point(8,148);
            this.m_ctrlObjections.Name = "m_ctrlObjections";
            this.m_ctrlObjections.Override = "";
            this.m_ctrlObjections.Size = new System.Drawing.Size(420,28);
            this.m_ctrlObjections.TabIndex = 4;
            this.m_ctrlObjections.Tag = "";
            // 
            // CFPathMapEditor
            // 
            this.AcceptButton = this.m_ctrlOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
            this.CancelButton = this.m_ctrlCancel;
            this.ClientSize = new System.Drawing.Size(448,308);
            this.Controls.Add(this.m_ctrlFoldersGroup);
            this.Controls.Add(this.m_ctrlWarningLine2);
            this.Controls.Add(this.m_ctrlWarningLabel);
            this.Controls.Add(this.m_ctrlWarningLine1);
            this.Controls.Add(this.m_ctrlName);
            this.Controls.Add(this.m_ctrlNameLabel);
            this.Controls.Add(this.m_ctrlCancel);
            this.Controls.Add(this.m_ctrlOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFPathMapEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Path Map Editor";
            this.m_ctrlFoldersGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if((m_tmaxPathMaps == null) || (m_tmaxPathMap == null) || (m_tmaxPathMap.CasePaths == null))
			{
				m_ctrlOk.Enabled = false;
				m_ctrlNameLabel.Enabled = false;
				m_ctrlName.Enabled = false;
				m_ctrlWarningLabel.Visible = false;
				m_ctrlWarningLine1.Visible = false;
				m_ctrlWarningLine2.Visible = false;
				
				//	Disable all the editors
				foreach(CTmaxPathOverrideCtrl O in m_aPathEditors)
					O.Enabled = false;
			}
			else
			{
				//	Initialize the name
				m_ctrlName.Text = m_tmaxPathMap.Name;
				
				m_ctrlWarningLabel.Visible = m_bShowWarning;
				m_ctrlWarningLine1.Visible = m_bShowWarning;
				m_ctrlWarningLine2.Visible = m_bShowWarning;
			
			}// if((m_tmaxPathMaps == null) || (m_tmaxPathMap == null) || (m_tmaxPathMap.CasePaths == null))
		
			base.OnLoad(e);
			
		}// private void OnLoad(object sender, System.EventArgs e)
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			Debug.Assert(m_tmaxPathMaps != null);
			Debug.Assert(m_tmaxPathMap != null);
			if((m_tmaxPathMaps == null) || (m_tmaxPathMap == null))
			{
				DialogResult = DialogResult.Cancel;
				this.Close();
				return;
			}
			
			//	Check the name for the map
			if(CheckName() == false) return;

			//	Check each of the editors
			foreach(CTmaxPathOverrideCtrl O in m_aPathEditors)
				if(CheckEditor(O) == false) return;
							
			//	Set the values
			m_tmaxPathMap.Name = m_ctrlName.Text;
			foreach(CTmaxPathOverrideCtrl O in m_aPathEditors)
				GetEditor(O);
				
			DialogResult = DialogResult.OK;
			this.Close();
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called to determine if the name entered by the user is valid</summary>
		/// <returns>True if valid</returns>
		private bool CheckName()
		{
			//	Did the caller provide a name
			if(m_ctrlName.Text.Length == 0)
				return Warn("You must specify a valid name for the path map.");
			
			//	See if the name already exists
			foreach(CTmaxPathMap O in m_tmaxPathMaps)
			{
				if(String.Compare(O.Name, m_ctrlName.Text, true) == 0)
				{
					if(ReferenceEquals(m_tmaxPathMap, O) == false)
					{
						return Warn(O.Name + " is already assigned to map #" + O.Id.ToString());
					}
					
				}// if(String.Compare(O.Name, m_ctrlName.Text, true) == 0)
				
			}// foreach(CTmaxPathMap O in m_tmaxPathMaps)
			
			return true;
		}
		
		/// <summary>This method is called to determine if the override path associated with the specified editor is valid</summary>
		/// <param name="tmaxEditor">The path override control to check</param>
		/// <returns>True if valid</returns>
		private bool CheckEditor(CTmaxPathOverrideCtrl tmaxEditor)
		{
			//	Is there an override path for this editor?
			if(tmaxEditor.Override.Length > 0)
			{
				//	Prompt for confirmation if the folder does not exist
				if(System.IO.Directory.Exists(tmaxEditor.Override) == false)
				{
					string strMsg = "The override folder specified for ";
					strMsg += tmaxEditor.Folder.ToString();
					strMsg += " does not exist\n\n";
					strMsg += tmaxEditor.Override;
					strMsg += "\n\nDo you want to continue?";
					
					if(MessageBox.Show(strMsg, "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.No)
						return false;
				}
			
			}
			
			return true;
		
		}// private bool CheckEditor(CTmaxPathOverrideCtrl tmaxEditor)
		
		/// <summary>This method sets the override path for the specified editor</summary>
		/// <param name="tmaxEditor">The desired editor control</param>
		private void SetEditor(CTmaxPathOverrideCtrl tmaxEditor)
		{
			CTmaxOption tmaxOption = null;
			
			//	Get the case path for this control
			if((m_tmaxPathMap != null) && (m_tmaxPathMap.CasePaths != null))
				tmaxOption = m_tmaxPathMap.CasePaths.Find(tmaxEditor.Folder.ToString());
				
			if(tmaxOption != null)
			{
				//	Set the default case folder
				tmaxEditor.CasePath = CaseFolder;
				if(CaseFolder.Length > 0)
				{
					tmaxEditor.CasePath = CaseFolder;
					if(tmaxEditor.CasePath.EndsWith("\\") == false)
						tmaxEditor.CasePath += "\\";
				}
				tmaxEditor.CasePath += ("_tmax_" + tmaxEditor.Folder.ToString());
				tmaxEditor.CasePath = tmaxEditor.CasePath.ToLower();

				//	Set the override path
				tmaxEditor.Override = tmaxOption.Value.ToString();
				tmaxEditor.Enabled = true;
			}
			else
			{
				tmaxEditor.Override = "";
				tmaxEditor.CasePath = "";
				tmaxEditor.Enabled = false;
			}
			
		}// private void SetEditor(CTmaxPathOverrideCtrl tmaxEditor)
		
		/// <summary>This method gets the override path for the specified editor</summary>
		/// <param name="tmaxEditor">The desired editor control</param>
		private void GetEditor(CTmaxPathOverrideCtrl tmaxEditor)
		{
			CTmaxOption tmaxOption = null;
			
			try
			{
				//	Get the case path for this control
				if((m_tmaxPathMap != null) && (m_tmaxPathMap.CasePaths != null))
					tmaxOption = m_tmaxPathMap.CasePaths.Find(tmaxEditor.Folder.ToString());
					
				//	Set the override path
				if(tmaxOption != null)
					tmaxOption.Value = tmaxEditor.Override;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetEditor", Ex);
			}
			
		}// private void GetEditor(CTmaxPathOverrideCtrl tmaxEditor)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The application's collection of path maps</summary>
		public CTmaxPathMaps PathMaps
		{
			get { return m_tmaxPathMaps;  }
			set { m_tmaxPathMaps = value; }
		}
		
		/// <summary>The path map being edited</summary>
		public CTmaxPathMap PathMap
		{
			get { return m_tmaxPathMap;  }
			set 
			{ 
				m_tmaxPathMap = value; 
				
				//	Initialize the path editors
				foreach(CTmaxPathOverrideCtrl O in m_aPathEditors)
				{
					if(O.IsDisposed == false)
						SetEditor(O);
				}

			}
		
		}
		
		/// <summary>True to show the warning text</summary>
		public bool ShowWarning
		{
			get { return m_bShowWarning;  }
			set { m_bShowWarning = value; }
		}
		
		/// <summary>Folder containing the active case</summary>
		public string CaseFolder
		{
			get { return m_strCaseFolder;  }
			set { m_strCaseFolder = value; }
		}
		
		#endregion Properties
	
	}// public class CFPathMapEditor : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
