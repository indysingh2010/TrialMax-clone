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
	/// <summary>This form allows the user to set options for importing from XML files</summary>
	public class CFImportXmlScripts : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_FILL_DELIMITERS_EX = ERROR_TMAX_FORM_MAX + 1;
		private const int ERROR_SET_DELIMITER_EX = ERROR_TMAX_FORM_MAX + 2;
		private const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 3;
		private const int ERROR_FILL_MERGE_DESIGNATIONS_EX = ERROR_TMAX_FORM_MAX + 4;
		private const int ERROR_SET_MERGE_DESIGNATIONS_EX = ERROR_TMAX_FORM_MAX + 5;
		private const int ERROR_SET_OBJECTIONS_METHOD_EX = ERROR_TMAX_FORM_MAX + 6;
		
		private const long WS_POPUP = 0x80000000;
		private const long TTS_BALLOON = 0x40;
		private const long TTS_NOFADE = 0x20;
		private const int GWL_STYLE = -16;

		private const int OBJECTIONS_METHOD_IGNORE_ALL = 0;
		private const int OBJECTIONS_METHOD_UPDATE_EXISTING = 1;
		private const int OBJECTIONS_METHOD_IGNORE_EXISTING = 2;
		private const int OBJECTIONS_METHOD_ADD_ALL = 3;

		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

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

		/// <summary>Group box for objections controls</summary>
		private GroupBox m_ctrlObjectionsGroup;

		/// <summary>Check box to ignore objection GUID identifiers</summary>
		private CheckBox m_ctrlDiscardGUID;

		/// <summary>Radio button set used to select the objections method</summary>
		private Infragistics.Win.UltraWinEditors.UltraOptionSet m_ctrlObjectionMethod;

		/// <summary>Check box to enable addition of unidentified objection rulings</summary>
		private CheckBox m_ctrlAddObjectionRulings;

		/// <summary>Check box to enable addition of unidentified objection states</summary>
		private CheckBox m_ctrlAddObjectionStates;

		/// <summary>Check box to enable the option to ignore the first line when merging on page boundries</summary>
		private CheckBox m_ctrlIgnoreFirstLine;

		/// <summary>Local member bound to ImportOptions property</summary>
		private CTmaxImportOptions m_tmaxImportOptions = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFImportXmlScripts()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFImportXmlScripts()

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

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active objections method to %1");

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
				m_ctrlMergeDesignations.Enabled = false;
				m_ctrlMergeDesignationsLabel.Enabled = false;
				m_ctrlObjectionMethod.Enabled = false;
				m_ctrlDiscardGUID.Enabled = false;
				m_ctrlAddObjectionRulings.Enabled = false;
				m_ctrlAddObjectionStates.Enabled = false;
			}
			else
			{
				SetBalloonStyle(m_ctrlToolTip);
				
				m_ctrlToolTip.SetToolTip(m_ctrlUpdate, "Update scenes in existing script when MediaID already exists");
				m_ctrlToolTip.SetToolTip(m_ctrlCreateBackup, "Create a backup when updating an existing script");

				m_ctrlToolTip.SetToolTip(m_ctrlObjectionMethod, "Select method for importing objections contained in the file");
				m_ctrlToolTip.SetToolTip(m_ctrlDiscardGUID, "Discard objection GUID identifiers");
				m_ctrlToolTip.SetToolTip(m_ctrlAddObjectionRulings, "Add unidentified objection rulings to the database");
				m_ctrlToolTip.SetToolTip(m_ctrlAddObjectionStates, "Add unidentified objection status identifiers to the database");
				m_ctrlToolTip.SetToolTip(m_ctrlIgnoreFirstLine, "Ignore line 1 if missing when merging on page boundries");

				OnMergeDesignationsIndexChanged(m_ctrlMergeDesignations, System.EventArgs.Empty);

				SetControlStates();
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

		/// <summary>This method is called to set the active import method</summary>
		/// <param name="eMethod">the enumerated method identifier</param>
		/// <returns>true if successful</returns>
		private bool SetObjectionsMethod(TmaxImportObjectionMethods eMethod)
		{
			bool bSuccessful = false;

			try
			{
				switch(eMethod)
				{
					case TmaxImportObjectionMethods.AddAll:
						m_ctrlObjectionMethod.CheckedIndex = OBJECTIONS_METHOD_ADD_ALL;
						break;

					case TmaxImportObjectionMethods.IgnoreExisting:
						m_ctrlObjectionMethod.CheckedIndex = OBJECTIONS_METHOD_IGNORE_EXISTING;
						break;

					case TmaxImportObjectionMethods.IgnoreAll:
						m_ctrlObjectionMethod.CheckedIndex = OBJECTIONS_METHOD_IGNORE_ALL;
						break;

					case TmaxImportObjectionMethods.UpdateExisting:
					default:
						m_ctrlObjectionMethod.CheckedIndex = OBJECTIONS_METHOD_UPDATE_EXISTING;
						break;

				}// switch(eMethod)

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetObjectionsMethod", m_tmaxErrorBuilder.Message(ERROR_SET_OBJECTIONS_METHOD_EX, eMethod), Ex);
			}

			return bSuccessful;

		}// private bool SetObjectionsMethod(TmaxImportObjectionMethods eMethod)

		/// <summary>This method is called to get the selected import method</summary>
		/// <returns>The import method selected by the user</returns>
		private TmaxImportObjectionMethods GetObjectionsMethod()
		{
			TmaxImportObjectionMethods eMethod = TmaxImportObjectionMethods.UpdateExisting;

			try
			{
				if(m_ctrlObjectionMethod.CheckedIndex == OBJECTIONS_METHOD_ADD_ALL)
					eMethod = TmaxImportObjectionMethods.AddAll;
				else if(m_ctrlObjectionMethod.CheckedIndex == OBJECTIONS_METHOD_IGNORE_EXISTING)
					eMethod = TmaxImportObjectionMethods.IgnoreExisting;
				else if(m_ctrlObjectionMethod.CheckedIndex == OBJECTIONS_METHOD_IGNORE_ALL)
					eMethod = TmaxImportObjectionMethods.IgnoreAll;
				else
					eMethod = TmaxImportObjectionMethods.UpdateExisting;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetObjectionsMethod", Ex);
			}

			return eMethod;

		}// private TmaxImportObjectionMethods GetObjectionsMethod()

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
					m_tmaxImportOptions.MergeDesignations = GetMergeDesignations();
					m_tmaxImportOptions.UpdateScripts = m_ctrlUpdate.Checked;
					m_tmaxImportOptions.CreateBackup = m_ctrlCreateBackup.Checked;
					m_tmaxImportOptions.UseRegistrationOptions = m_ctrlUseRegistrationOptions.Checked;
					m_tmaxImportOptions.ObjectionsMethod = GetObjectionsMethod();
					m_tmaxImportOptions.AddObjectionStates = m_ctrlAddObjectionStates.Checked;
					m_tmaxImportOptions.AddObjectionRulings = m_ctrlAddObjectionRulings.Checked;
					m_tmaxImportOptions.DiscardObjectionsId = m_ctrlDiscardGUID.Checked;
					m_tmaxImportOptions.IgnoreFirstLine = m_ctrlIgnoreFirstLine.Checked;
				}
				else
				{
					m_ctrlUseRegistrationOptions.Checked = m_tmaxImportOptions.UseRegistrationOptions;
					SetMergeDesignations(m_tmaxImportOptions.MergeDesignations);
					m_ctrlUpdate.Checked = m_tmaxImportOptions.UpdateScripts;
					m_ctrlCreateBackup.Checked = m_tmaxImportOptions.CreateBackup;
					m_ctrlAddObjectionStates.Checked = m_tmaxImportOptions.AddObjectionStates;
					m_ctrlAddObjectionRulings.Checked = m_tmaxImportOptions.AddObjectionRulings;
					m_ctrlIgnoreFirstLine.Checked = m_tmaxImportOptions.IgnoreFirstLine;
					m_ctrlDiscardGUID.Checked = m_tmaxImportOptions.DiscardObjectionsId;
					SetObjectionsMethod(m_tmaxImportOptions.ObjectionsMethod);
						
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
			Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlMergeDesignationsLabel = new System.Windows.Forms.Label();
			this.m_ctrlMergeDesignations = new System.Windows.Forms.ComboBox();
			this.m_ctrlToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.m_ctrlUpdate = new System.Windows.Forms.CheckBox();
			this.m_ctrlCreateBackup = new System.Windows.Forms.CheckBox();
			this.m_ctrlProcessingGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlIgnoreFirstLine = new System.Windows.Forms.CheckBox();
			this.m_ctrlRecordsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlUseRegistrationOptions = new System.Windows.Forms.CheckBox();
			this.m_ctrlObjectionsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlDiscardGUID = new System.Windows.Forms.CheckBox();
			this.m_ctrlObjectionMethod = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
			this.m_ctrlAddObjectionRulings = new System.Windows.Forms.CheckBox();
			this.m_ctrlAddObjectionStates = new System.Windows.Forms.CheckBox();
			this.m_ctrlProcessingGroup.SuspendLayout();
			this.m_ctrlRecordsGroup.SuspendLayout();
			this.m_ctrlObjectionsGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlObjectionMethod)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(221, 412);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(137, 412);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlMergeDesignationsLabel
			// 
			this.m_ctrlMergeDesignationsLabel.Location = new System.Drawing.Point(12, 22);
			this.m_ctrlMergeDesignationsLabel.Name = "m_ctrlMergeDesignationsLabel";
			this.m_ctrlMergeDesignationsLabel.Size = new System.Drawing.Size(164, 16);
			this.m_ctrlMergeDesignationsLabel.TabIndex = 16;
			this.m_ctrlMergeDesignationsLabel.Text = "Designation Merge Method";
			// 
			// m_ctrlMergeDesignations
			// 
			this.m_ctrlMergeDesignations.Location = new System.Drawing.Point(12, 38);
			this.m_ctrlMergeDesignations.Name = "m_ctrlMergeDesignations";
			this.m_ctrlMergeDesignations.Size = new System.Drawing.Size(267, 21);
			this.m_ctrlMergeDesignations.TabIndex = 2;
			this.m_ctrlMergeDesignations.SelectedIndexChanged += new System.EventHandler(this.OnMergeDesignationsIndexChanged);
			// 
			// m_ctrlUpdate
			// 
			this.m_ctrlUpdate.Location = new System.Drawing.Point(12, 44);
			this.m_ctrlUpdate.Name = "m_ctrlUpdate";
			this.m_ctrlUpdate.Size = new System.Drawing.Size(196, 20);
			this.m_ctrlUpdate.TabIndex = 1;
			this.m_ctrlUpdate.Text = "Replace if MediaID already exists";
			this.m_ctrlUpdate.Click += new System.EventHandler(this.OnClickUpdate);
			// 
			// m_ctrlCreateBackup
			// 
			this.m_ctrlCreateBackup.Location = new System.Drawing.Point(12, 65);
			this.m_ctrlCreateBackup.Name = "m_ctrlCreateBackup";
			this.m_ctrlCreateBackup.Size = new System.Drawing.Size(164, 20);
			this.m_ctrlCreateBackup.TabIndex = 2;
			this.m_ctrlCreateBackup.Text = "Create backup on update";
			// 
			// m_ctrlProcessingGroup
			// 
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlIgnoreFirstLine);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlMergeDesignations);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlMergeDesignationsLabel);
			this.m_ctrlProcessingGroup.Location = new System.Drawing.Point(8, 7);
			this.m_ctrlProcessingGroup.Name = "m_ctrlProcessingGroup";
			this.m_ctrlProcessingGroup.Size = new System.Drawing.Size(294, 97);
			this.m_ctrlProcessingGroup.TabIndex = 0;
			this.m_ctrlProcessingGroup.TabStop = false;
			this.m_ctrlProcessingGroup.Text = "File Processing";
			// 
			// m_ctrlIgnoreFirstLine
			// 
			this.m_ctrlIgnoreFirstLine.Location = new System.Drawing.Point(12, 65);
			this.m_ctrlIgnoreFirstLine.Name = "m_ctrlIgnoreFirstLine";
			this.m_ctrlIgnoreFirstLine.Size = new System.Drawing.Size(192, 24);
			this.m_ctrlIgnoreFirstLine.TabIndex = 17;
			this.m_ctrlIgnoreFirstLine.Text = "Ignore Line 1 (if missing)";
			// 
			// m_ctrlRecordsGroup
			// 
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlUseRegistrationOptions);
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlUpdate);
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlCreateBackup);
			this.m_ctrlRecordsGroup.Location = new System.Drawing.Point(8, 110);
			this.m_ctrlRecordsGroup.Name = "m_ctrlRecordsGroup";
			this.m_ctrlRecordsGroup.Size = new System.Drawing.Size(294, 94);
			this.m_ctrlRecordsGroup.TabIndex = 1;
			this.m_ctrlRecordsGroup.TabStop = false;
			this.m_ctrlRecordsGroup.Text = "Scripts";
			// 
			// m_ctrlUseRegistrationOptions
			// 
			this.m_ctrlUseRegistrationOptions.Location = new System.Drawing.Point(12, 19);
			this.m_ctrlUseRegistrationOptions.Name = "m_ctrlUseRegistrationOptions";
			this.m_ctrlUseRegistrationOptions.Size = new System.Drawing.Size(216, 24);
			this.m_ctrlUseRegistrationOptions.TabIndex = 0;
			this.m_ctrlUseRegistrationOptions.Text = "Apply registration options to Media ID";
			// 
			// m_ctrlObjectionsGroup
			// 
			this.m_ctrlObjectionsGroup.Controls.Add(this.m_ctrlDiscardGUID);
			this.m_ctrlObjectionsGroup.Controls.Add(this.m_ctrlObjectionMethod);
			this.m_ctrlObjectionsGroup.Controls.Add(this.m_ctrlAddObjectionRulings);
			this.m_ctrlObjectionsGroup.Controls.Add(this.m_ctrlAddObjectionStates);
			this.m_ctrlObjectionsGroup.Location = new System.Drawing.Point(8, 211);
			this.m_ctrlObjectionsGroup.Name = "m_ctrlObjectionsGroup";
			this.m_ctrlObjectionsGroup.Size = new System.Drawing.Size(294, 195);
			this.m_ctrlObjectionsGroup.TabIndex = 2;
			this.m_ctrlObjectionsGroup.TabStop = false;
			this.m_ctrlObjectionsGroup.Text = "Objections";
			// 
			// m_ctrlDiscardGUID
			// 
			this.m_ctrlDiscardGUID.Location = new System.Drawing.Point(12, 103);
			this.m_ctrlDiscardGUID.Name = "m_ctrlDiscardGUID";
			this.m_ctrlDiscardGUID.Size = new System.Drawing.Size(276, 24);
			this.m_ctrlDiscardGUID.TabIndex = 1;
			this.m_ctrlDiscardGUID.Text = "Discard Global Unique Identifiers && Case Names";
			this.m_ctrlDiscardGUID.UseVisualStyleBackColor = true;
			// 
			// m_ctrlObjectionMethod
			// 
			this.m_ctrlObjectionMethod.AccessibleDescription = "Objections Method";
			this.m_ctrlObjectionMethod.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlObjectionMethod.CheckedIndex = 1;
			this.m_ctrlObjectionMethod.GlyphInfo = Infragistics.Win.UIElementDrawParams.StandardRadioButtonGlyphInfo;
			valueListItem5.DataValue = 0;
			valueListItem5.DisplayText = "Ignore All Objections";
			valueListItem6.DataValue = 1;
			valueListItem6.DisplayText = "Update Existing Objections";
			valueListItem7.DataValue = 2;
			valueListItem7.DisplayText = "Ignore Existing Objections";
			valueListItem8.DataValue = 3;
			valueListItem8.DisplayText = "Add All As New";
			this.m_ctrlObjectionMethod.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem5,
            valueListItem6,
            valueListItem7,
            valueListItem8});
			this.m_ctrlObjectionMethod.ItemSpacingVertical = 7;
			this.m_ctrlObjectionMethod.Location = new System.Drawing.Point(11, 19);
			this.m_ctrlObjectionMethod.Name = "m_ctrlObjectionMethod";
			this.m_ctrlObjectionMethod.Size = new System.Drawing.Size(207, 83);
			this.m_ctrlObjectionMethod.TabIndex = 0;
			this.m_ctrlObjectionMethod.Text = "Update Existing Objections";
			this.m_ctrlObjectionMethod.ValueChanged += new System.EventHandler(this.OnClickObjectionsMethod);
			// 
			// m_ctrlAddObjectionRulings
			// 
			this.m_ctrlAddObjectionRulings.Location = new System.Drawing.Point(12, 162);
			this.m_ctrlAddObjectionRulings.Name = "m_ctrlAddObjectionRulings";
			this.m_ctrlAddObjectionRulings.Size = new System.Drawing.Size(217, 20);
			this.m_ctrlAddObjectionRulings.TabIndex = 3;
			this.m_ctrlAddObjectionRulings.Text = "Add unknown ruling identifiers";
			// 
			// m_ctrlAddObjectionStates
			// 
			this.m_ctrlAddObjectionStates.Location = new System.Drawing.Point(12, 139);
			this.m_ctrlAddObjectionStates.Name = "m_ctrlAddObjectionStates";
			this.m_ctrlAddObjectionStates.Size = new System.Drawing.Size(217, 20);
			this.m_ctrlAddObjectionStates.TabIndex = 2;
			this.m_ctrlAddObjectionStates.Text = "Add unknown status identifiers";
			// 
			// CFImportXmlScripts
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(314, 444);
			this.Controls.Add(this.m_ctrlObjectionsGroup);
			this.Controls.Add(this.m_ctrlRecordsGroup);
			this.Controls.Add(this.m_ctrlProcessingGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFImportXmlScripts";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Import XML Script Options";
			this.m_ctrlProcessingGroup.ResumeLayout(false);
			this.m_ctrlRecordsGroup.ResumeLayout(false);
			this.m_ctrlObjectionsGroup.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlObjectionMethod)).EndInit();
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

		/// <summary>This method is called when the user clicks on one of the Objection methods radio buttons</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickObjectionsMethod(object sender, System.EventArgs e)
		{
			SetControlStates();
		}

		/// <summary>Called to enable/disable the child controls</summary>
		private void SetControlStates()
		{
			m_ctrlDiscardGUID.Enabled = (m_ctrlObjectionMethod.CheckedIndex == OBJECTIONS_METHOD_ADD_ALL);
			m_ctrlAddObjectionStates.Enabled = (m_ctrlObjectionMethod.CheckedIndex != OBJECTIONS_METHOD_IGNORE_ALL);
			m_ctrlAddObjectionRulings.Enabled = (m_ctrlObjectionMethod.CheckedIndex != OBJECTIONS_METHOD_IGNORE_ALL);
		}

		#endregion Private Methods

		#region Properties
		
		/// <summary>The user defined export options</summary>
		public CTmaxImportOptions ImportOptions
		{
			get { return m_tmaxImportOptions;  }
			set { m_tmaxImportOptions = value; }
		}
		
		#endregion Properties
	
	}// public class CFImportXmlScripts : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
