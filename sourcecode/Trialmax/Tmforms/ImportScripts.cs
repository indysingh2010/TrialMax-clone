using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Shared.Win32;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFImportScripts : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_FILL_DELIMITERS_EX			= ERROR_TMAX_FORM_MAX + 1;
		protected const int ERROR_SET_DELIMITER_EX				= ERROR_TMAX_FORM_MAX + 2;
		protected const int ERROR_EXCHANGE_EX					= ERROR_TMAX_FORM_MAX + 3;
		protected const int ERROR_FILL_MERGE_DESIGNATIONS_EX	= ERROR_TMAX_FORM_MAX + 4;
		protected const int ERROR_SET_MERGE_DESIGNATIONS_EX		= ERROR_TMAX_FORM_MAX + 5;
		
		private const long WS_POPUP = 0x80000000;
		private const long TTS_BALLOON = 0x40;
		private const long TTS_NOFADE = 0x20;
		private const int GWL_STYLE = -16;
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Combox box for available delimiters</summary>
		private System.Windows.Forms.ComboBox m_ctrlDelimiters;
		
		/// <summary>Static text label for delimiters text box</summary>
		private System.Windows.Forms.Label m_ctrlDelimiterLabel;
		
		/// <summary>Text box to supply comment characters</summary>
		private System.Windows.Forms.TextBox m_ctrlCommentCharacters;
		
		/// <summary>Static text label for comment characters text box</summary>
		private System.Windows.Forms.Label m_ctrlCommentCharactersLabel;

		/// <summary>Label for combox box of available merge methods</summary>
		private System.Windows.Forms.Label m_ctrlMergeDesignationsLabel;

		/// <summary>Combox box for available merge methods</summary>
		private System.Windows.Forms.ComboBox m_ctrlMergeDesignations;

		/// <summary>The form's tool tip control</summary>
		private System.Windows.Forms.ToolTip m_ctrlToolTip;

		/// <summary>Check box to allow user to request record updates</summary>
		private System.Windows.Forms.CheckBox m_ctrlUpdate;

		/// <summary>Check box to allow user to request backups on update</summary>
		private System.Windows.Forms.CheckBox m_ctrlCreateBackup;

		/// <summary>Group box for file processing controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlProcessingGroup;

		/// <summary>Group box for record management controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlRecordsGroup;

		/// <summary>Check box to request application of registration options</summary>
		private System.Windows.Forms.CheckBox m_ctrlUseRegistrationOptions;

		/// <summary>Check box to allow user to set the value of the Ignore First Line merge option</summary>
		private CheckBox m_ctrlIgnoreFirstLine;

		/// <summary>Local member bound to ImportOptions property</summary>
		private CTmaxImportOptions m_tmaxImportOptions = null;

		/// <summary>Local member bound to HideRecords property</summary>
		private bool m_bHideRecords = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFImportScripts()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFImportScripts()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of delimiters.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active delimiter.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the form's properties: SetMembers = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of designation merge methods.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active designation merge method.");
		
		}// protected override void SetErrorStrings()

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
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			bool bSuccessful = false;
			
			//	Initialize all the child controls
			while(bSuccessful == false)
			{
				if(m_bHideRecords == true)
					HideRecordControls();
					
				if(FillDelimiters() == false)
					break;
					
				if(FillMergeDesignations() == false)
					break;
					
				if(Exchange(false) == false)
					break;
					
				OnClickUpdate(m_ctrlUpdate, System.EventArgs.Empty);
				
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			if(bSuccessful == false)
			{
				m_ctrlOk.Enabled = false;
				m_ctrlDelimiters.Enabled = false;
				m_ctrlDelimiterLabel.Enabled = false;
				m_ctrlCommentCharacters.Enabled = false;
				m_ctrlCommentCharactersLabel.Enabled = false;
				m_ctrlMergeDesignations.Enabled = false;
				m_ctrlMergeDesignationsLabel.Enabled = false;
			}
			else
			{
				SetBalloonStyle(m_ctrlToolTip);
				
				m_ctrlToolTip.SetToolTip(m_ctrlDelimiters, "Field delimiter character");
				m_ctrlToolTip.SetToolTip(m_ctrlDelimiterLabel, m_ctrlToolTip.GetToolTip(m_ctrlDelimiters));
				m_ctrlToolTip.SetToolTip(m_ctrlCommentCharacters, "Ignore lines that start with these characters");
				m_ctrlToolTip.SetToolTip(m_ctrlCommentCharactersLabel, m_ctrlToolTip.GetToolTip(m_ctrlCommentCharacters));
				m_ctrlToolTip.SetToolTip(m_ctrlUpdate, "Update scenes in existing script when MediaID already exists");
				m_ctrlToolTip.SetToolTip(m_ctrlCreateBackup, "Create a backup when updating an existing script");
				m_ctrlToolTip.SetToolTip(m_ctrlIgnoreFirstLine, "Ignore line 1 if missing when merging on page boundries");
				
				OnMergeDesignationsIndexChanged(m_ctrlMergeDesignations, System.EventArgs.Empty);
			}
				
			base.OnLoad(e);
			
		}// private void OnLoad(object sender, System.EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method handles events fired when the user changes the selection in the CRLF substitutions list box</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnMergeDesignationsIndexChanged(object sender, System.EventArgs e)
		{
			TmaxDesignationMergeMethods eMethod = TmaxDesignationMergeMethods.None;

			try 
			{ 
				//	Get the current selection
				eMethod = GetMergeDesignations();

				//	Set the appropriate tool tip text
				m_ctrlToolTip.SetToolTip(m_ctrlMergeDesignations, CTmaxImportOptions.GetToolTip(eMethod)); 
				m_ctrlToolTip.SetToolTip(m_ctrlMergeDesignationsLabel, m_ctrlToolTip.GetToolTip(m_ctrlMergeDesignations));

				//	Enable/disable the option to ignore the first line
				m_ctrlIgnoreFirstLine.Enabled = (eMethod == TmaxDesignationMergeMethods.AdjacentPages);
			}
			catch 
			{
			
			}

		}// private void OnMergeDesignationsIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method is called to populate the list of available delimiters</summary>
		/// <returns>true if successful</returns>
		private bool FillDelimiters()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Add each of the enumerated delimiter values
				Array aDelimiters = Enum.GetValues(typeof(TmaxImportDelimiters));
				foreach(TmaxImportDelimiters O in aDelimiters)
				{
					if(O != TmaxImportDelimiters.Expression)
						m_ctrlDelimiters.Items.Add(O);
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillDelimiters", m_tmaxErrorBuilder.Message(ERROR_FILL_DELIMITERS_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool FillDelimiters()
		
		/// <summary>This method is called to populate the list of available designation merge methods</summary>
		/// <returns>true if successful</returns>
		private bool FillMergeDesignations()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Add each of the enumerated delimiter values
				Array aMethods = Enum.GetValues(typeof(TmaxDesignationMergeMethods));
				foreach(TmaxDesignationMergeMethods O in aMethods)
				{
					m_ctrlMergeDesignations.Items.Add(CTmaxImportOptions.GetDisplayText(O));
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillMergeDesignations", m_tmaxErrorBuilder.Message(ERROR_FILL_MERGE_DESIGNATIONS_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool FillMergeDesignations()
		
		/// <summary>This method is called to set the active delimiter</summary>
		/// <param name="eDelimiter">the delimiter to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetDelimiter(TmaxImportDelimiters eDelimiter)
		{
			bool	bSuccessful = false;
			int		iIndex = 0;
			
			if(m_ctrlDelimiters.Items == null) return false;
			if(m_ctrlDelimiters.Items.Count == 0) return false;
			
			try
			{
				if((iIndex = m_ctrlDelimiters.FindStringExact(eDelimiter.ToString())) < 0)
					iIndex = 0;
					
				m_ctrlDelimiters.SelectedIndex = iIndex;
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDelimiter", m_tmaxErrorBuilder.Message(ERROR_SET_DELIMITER_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool SetDelimiter()
		
		/// <summary>This method is called to set the active designation merge method</summary>
		/// <param name="eMergeDesignations">the method to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetMergeDesignations(TmaxDesignationMergeMethods eMethod)
		{
			bool bSuccessful = false;
			
			if(m_ctrlMergeDesignations.Items == null) return false;
			if(m_ctrlMergeDesignations.Items.Count == 0) return false;
			
			try
			{
				if((int)eMethod <= m_ctrlMergeDesignations.Items.Count)
				{
					m_ctrlMergeDesignations.SelectedIndex = (int)eMethod;
					bSuccessful = true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetMergeDesignations", m_tmaxErrorBuilder.Message(ERROR_SET_MERGE_DESIGNATIONS_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool SetMergeDesignations(TmaxDesignationMergeMethods eMethod)
		
		/// <summary>This method is called to get the active delimiter</summary>
		/// <returns>the selected delimiter</returns>
		private TmaxImportDelimiters GetDelimiter()
		{
			TmaxImportDelimiters eDelimiter = TmaxImportDelimiters.Tab;
			
			if(m_ctrlDelimiters.Items == null) return eDelimiter;
			if(m_ctrlDelimiters.Items.Count == 0) return eDelimiter;
			
			try
			{
				if(m_ctrlDelimiters.SelectedItem != null)
					eDelimiter = (TmaxImportDelimiters)(m_ctrlDelimiters.SelectedItem);
			}
			catch
			{
			}
			
			return eDelimiter; 
			
		}// private TmaxImportDelimiters GetDelimiter()
		
		/// <summary>This method is called to get the active designation merge method</summary>
		/// <returns>the selected delimiter</returns>
		private TmaxDesignationMergeMethods GetMergeDesignations()
		{
			TmaxDesignationMergeMethods eMethod = TmaxDesignationMergeMethods.None;
			
			if(m_ctrlMergeDesignations.Items == null) return eMethod;
			if(m_ctrlMergeDesignations.Items.Count == 0) return eMethod;
			
			try
			{
				if(m_ctrlMergeDesignations.SelectedIndex >= 0)
					eMethod = (TmaxDesignationMergeMethods)(m_ctrlMergeDesignations.SelectedIndex);
			}
			catch
			{
			}
			
			return eMethod; 
			
		}// private TmaxDesignationMergeMethods GetMergeDesignations()
		
		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetMembers)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					m_tmaxImportOptions.CommentCharacters = m_ctrlCommentCharacters.Text;
					m_tmaxImportOptions.Delimiter = GetDelimiter();
					m_tmaxImportOptions.MergeDesignations = GetMergeDesignations();
					m_tmaxImportOptions.UpdateScripts = m_ctrlUpdate.Checked;
					m_tmaxImportOptions.CreateBackup = m_ctrlCreateBackup.Checked;
					m_tmaxImportOptions.UseRegistrationOptions = m_ctrlUseRegistrationOptions.Checked;
					m_tmaxImportOptions.IgnoreFirstLine = m_ctrlIgnoreFirstLine.Checked;
				}
				else
				{
					m_ctrlCommentCharacters.Text = m_tmaxImportOptions.CommentCharacters;
					m_ctrlUseRegistrationOptions.Checked = m_tmaxImportOptions.UseRegistrationOptions;
					m_ctrlIgnoreFirstLine.Checked = m_tmaxImportOptions.IgnoreFirstLine;
					SetDelimiter(m_tmaxImportOptions.Delimiter);
					SetMergeDesignations(m_tmaxImportOptions.MergeDesignations);
					m_ctrlUpdate.Checked = m_tmaxImportOptions.UpdateScripts;
					m_ctrlCreateBackup.Checked = m_tmaxImportOptions.CreateBackup;
						
				}// if(bSetMembers == true)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetMembers), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool Exchange(bool bSetMembers)
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFImportScripts));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlDelimiters = new System.Windows.Forms.ComboBox();
			this.m_ctrlDelimiterLabel = new System.Windows.Forms.Label();
			this.m_ctrlCommentCharacters = new System.Windows.Forms.TextBox();
			this.m_ctrlCommentCharactersLabel = new System.Windows.Forms.Label();
			this.m_ctrlMergeDesignationsLabel = new System.Windows.Forms.Label();
			this.m_ctrlMergeDesignations = new System.Windows.Forms.ComboBox();
			this.m_ctrlToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.m_ctrlUpdate = new System.Windows.Forms.CheckBox();
			this.m_ctrlCreateBackup = new System.Windows.Forms.CheckBox();
			this.m_ctrlProcessingGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlRecordsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlUseRegistrationOptions = new System.Windows.Forms.CheckBox();
			this.m_ctrlIgnoreFirstLine = new System.Windows.Forms.CheckBox();
			this.m_ctrlProcessingGroup.SuspendLayout();
			this.m_ctrlRecordsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(160, 321);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(76, 321);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 2;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlDelimiters
			// 
			this.m_ctrlDelimiters.Location = new System.Drawing.Point(10, 72);
			this.m_ctrlDelimiters.Name = "m_ctrlDelimiters";
			this.m_ctrlDelimiters.Size = new System.Drawing.Size(204, 21);
			this.m_ctrlDelimiters.TabIndex = 1;
			// 
			// m_ctrlDelimiterLabel
			// 
			this.m_ctrlDelimiterLabel.Location = new System.Drawing.Point(10, 56);
			this.m_ctrlDelimiterLabel.Name = "m_ctrlDelimiterLabel";
			this.m_ctrlDelimiterLabel.Size = new System.Drawing.Size(172, 16);
			this.m_ctrlDelimiterLabel.TabIndex = 12;
			this.m_ctrlDelimiterLabel.Text = "Delimiter";
			// 
			// m_ctrlCommentCharacters
			// 
			this.m_ctrlCommentCharacters.Location = new System.Drawing.Point(146, 28);
			this.m_ctrlCommentCharacters.Name = "m_ctrlCommentCharacters";
			this.m_ctrlCommentCharacters.Size = new System.Drawing.Size(64, 20);
			this.m_ctrlCommentCharacters.TabIndex = 0;
			// 
			// m_ctrlCommentCharactersLabel
			// 
			this.m_ctrlCommentCharactersLabel.Location = new System.Drawing.Point(10, 28);
			this.m_ctrlCommentCharactersLabel.Name = "m_ctrlCommentCharactersLabel";
			this.m_ctrlCommentCharactersLabel.Size = new System.Drawing.Size(132, 20);
			this.m_ctrlCommentCharactersLabel.TabIndex = 14;
			this.m_ctrlCommentCharactersLabel.Text = "Comment lines start with:";
			this.m_ctrlCommentCharactersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlMergeDesignationsLabel
			// 
			this.m_ctrlMergeDesignationsLabel.Location = new System.Drawing.Point(10, 104);
			this.m_ctrlMergeDesignationsLabel.Name = "m_ctrlMergeDesignationsLabel";
			this.m_ctrlMergeDesignationsLabel.Size = new System.Drawing.Size(164, 16);
			this.m_ctrlMergeDesignationsLabel.TabIndex = 16;
			this.m_ctrlMergeDesignationsLabel.Text = "Designation Merge Method";
			// 
			// m_ctrlMergeDesignations
			// 
			this.m_ctrlMergeDesignations.Location = new System.Drawing.Point(10, 120);
			this.m_ctrlMergeDesignations.Name = "m_ctrlMergeDesignations";
			this.m_ctrlMergeDesignations.Size = new System.Drawing.Size(204, 21);
			this.m_ctrlMergeDesignations.TabIndex = 2;
			this.m_ctrlMergeDesignations.SelectedIndexChanged += new System.EventHandler(this.OnMergeDesignationsIndexChanged);
			// 
			// m_ctrlUpdate
			// 
			this.m_ctrlUpdate.Location = new System.Drawing.Point(10, 50);
			this.m_ctrlUpdate.Name = "m_ctrlUpdate";
			this.m_ctrlUpdate.Size = new System.Drawing.Size(196, 20);
			this.m_ctrlUpdate.TabIndex = 1;
			this.m_ctrlUpdate.Text = "Update if MediaID already exists";
			this.m_ctrlUpdate.Click += new System.EventHandler(this.OnClickUpdate);
			// 
			// m_ctrlCreateBackup
			// 
			this.m_ctrlCreateBackup.Location = new System.Drawing.Point(10, 76);
			this.m_ctrlCreateBackup.Name = "m_ctrlCreateBackup";
			this.m_ctrlCreateBackup.Size = new System.Drawing.Size(164, 20);
			this.m_ctrlCreateBackup.TabIndex = 2;
			this.m_ctrlCreateBackup.Text = "Create backup on update";
			// 
			// m_ctrlProcessingGroup
			// 
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlIgnoreFirstLine);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlCommentCharactersLabel);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlCommentCharacters);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlDelimiterLabel);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlDelimiters);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlMergeDesignations);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlMergeDesignationsLabel);
			this.m_ctrlProcessingGroup.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlProcessingGroup.Name = "m_ctrlProcessingGroup";
			this.m_ctrlProcessingGroup.Size = new System.Drawing.Size(228, 185);
			this.m_ctrlProcessingGroup.TabIndex = 0;
			this.m_ctrlProcessingGroup.TabStop = false;
			this.m_ctrlProcessingGroup.Text = "File Processing";
			// 
			// m_ctrlRecordsGroup
			// 
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlUseRegistrationOptions);
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlUpdate);
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlCreateBackup);
			this.m_ctrlRecordsGroup.Location = new System.Drawing.Point(8, 206);
			this.m_ctrlRecordsGroup.Name = "m_ctrlRecordsGroup";
			this.m_ctrlRecordsGroup.Size = new System.Drawing.Size(228, 105);
			this.m_ctrlRecordsGroup.TabIndex = 1;
			this.m_ctrlRecordsGroup.TabStop = false;
			this.m_ctrlRecordsGroup.Text = "Records";
			// 
			// m_ctrlUseRegistrationOptions
			// 
			this.m_ctrlUseRegistrationOptions.Location = new System.Drawing.Point(10, 20);
			this.m_ctrlUseRegistrationOptions.Name = "m_ctrlUseRegistrationOptions";
			this.m_ctrlUseRegistrationOptions.Size = new System.Drawing.Size(214, 24);
			this.m_ctrlUseRegistrationOptions.TabIndex = 0;
			this.m_ctrlUseRegistrationOptions.Text = "Apply registration options to Media ID";
			// 
			// m_ctrlIgnoreFirstLine
			// 
			this.m_ctrlIgnoreFirstLine.Location = new System.Drawing.Point(10, 152);
			this.m_ctrlIgnoreFirstLine.Name = "m_ctrlIgnoreFirstLine";
			this.m_ctrlIgnoreFirstLine.Size = new System.Drawing.Size(192, 24);
			this.m_ctrlIgnoreFirstLine.TabIndex = 3;
			this.m_ctrlIgnoreFirstLine.Text = "Ignore Line 1 (if missing)";
			// 
			// CFImportScripts
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(246, 354);
			this.Controls.Add(this.m_ctrlRecordsGroup);
			this.Controls.Add(this.m_ctrlProcessingGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFImportScripts";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Import Script Options";
			this.m_ctrlProcessingGroup.ResumeLayout(false);
			this.m_ctrlProcessingGroup.PerformLayout();
			this.m_ctrlRecordsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			//	Get the user settings
			Exchange(true);
				
			//	Close the form
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

		/// <summary>This method is called when the user clicks on the Update check box</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickUpdate(object sender, System.EventArgs e)
		{
			if((m_ctrlCreateBackup != null) && (m_ctrlCreateBackup.IsDisposed == false))
			{
				try { m_ctrlCreateBackup.Enabled = m_ctrlUpdate.Checked; }
				catch {}
			}
		}

		/// <summary>This method will hide the controls used to set the record related options</summary>
		private void HideRecordControls()
		{
			try
			{
				m_ctrlOk.SetBounds(0, m_ctrlRecordsGroup.Top, 0, 0, BoundsSpecified.Y);
				m_ctrlCancel.SetBounds(0, m_ctrlRecordsGroup.Top, 0, 0, BoundsSpecified.Y);
			
				m_ctrlRecordsGroup.Visible = false;
				
				this.Size = new Size(this.Width, this.Height - m_ctrlRecordsGroup.Height);
			}
			catch
			{
			}
				
		}// private void HideRecords()

		#endregion Private Methods

		#region Properties
		
		/// <summary>The user defined export options</summary>
		public CTmaxImportOptions ImportOptions
		{
			get { return m_tmaxImportOptions;  }
			set { m_tmaxImportOptions = value; }
		}
		
		/// <summary>True to hide the record related options</summary>
		public bool HideRecords
		{
			get { return m_bHideRecords;  }
			set { m_bHideRecords = value; }
		}
		
		#endregion Properties
	
	}// public class CFImportScripts : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
