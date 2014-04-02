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
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFExportCodes : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_FILL_AVAILABLE_EX			= ERROR_TMAX_FORM_MAX + 1;
		protected const int ERROR_FILL_EXPORTED_EX			= ERROR_TMAX_FORM_MAX + 2;
		protected const int ERROR_FILL_DELIMITERS_EX		= ERROR_TMAX_FORM_MAX + 3;
		protected const int ERROR_SET_DELIMITER_EX			= ERROR_TMAX_FORM_MAX + 4;
		protected const int ERROR_EXCHANGE_EX				= ERROR_TMAX_FORM_MAX + 5;
		protected const int ERROR_INSERT_EX					= ERROR_TMAX_FORM_MAX + 6;
		protected const int ERROR_REMOVE_EX					= ERROR_TMAX_FORM_MAX + 7;
		protected const int ERROR_FILL_REPLACEMENTS_EX		= ERROR_TMAX_FORM_MAX + 8;
		protected const int ERROR_SET_REPLACEMENT_EX		= ERROR_TMAX_FORM_MAX + 9;
		protected const int ERROR_FILL_CONCATENATORS_EX		= ERROR_TMAX_FORM_MAX + 10;
		protected const int ERROR_SET_CONCATENATOR_EX		= ERROR_TMAX_FORM_MAX + 11;
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Combox box for available delimiters</summary>
		private System.Windows.Forms.ComboBox m_ctrlDelimiters;
		
		/// <summary>True to use default value if unassigned</summary>
		private System.Windows.Forms.CheckBox m_ctrlUseDefaults;
		
		/// <summary>Static text label for carraige return replacement list</summary>
		private System.Windows.Forms.Label m_ctrlCRLFReplacementsLabel;
		
		/// <summary>Combobox to select replacement characters for carraige returns</summary>
		private System.Windows.Forms.ComboBox m_ctrlCRLFReplacements;
		
		/// <summary>Static text label for user define carraige return replacement</summary>
		private System.Windows.Forms.Label m_ctrlUserCRLFLabel;
		
		/// <summary>User defined carraige return substitution characters</summary>
		private System.Windows.Forms.TextBox m_ctrlUserCRLF;
		
		/// <summary>Custom control to build the list of columns to be exported</summary>
		private FTI.Trialmax.Controls.CTmaxExportColumnsCtrl m_ctrlExportColumns;
		
		/// <summary>Label for list of predefined delimiter characters</summary>
		private System.Windows.Forms.Label m_ctrlDelimiterLabel;
		
		/// <summary>Check box to request exporting of column headers</summary>
		private System.Windows.Forms.CheckBox m_ctrlColumnHeaders;
		
		/// <summary>Check box to request inclusion of sub-binders</summary>
		private System.Windows.Forms.CheckBox m_ctrlSubBinders;
		
		/// <summary>Check box to wrap text fields with quotes</summary>
		private System.Windows.Forms.CheckBox m_ctrlAddQuotes;
		
		/// <summary>Local member to store a collection of available fields</summary>
		private CTmaxExportColumns m_tmaxAvailable = new CTmaxExportColumns();
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bound to ExportOptions property</summary>
		private CTmaxExportOptions m_tmaxExportOptions = null;
		private System.Windows.Forms.TextBox m_ctrlUserConcatenator;
		private System.Windows.Forms.Label m_ctrlUserConcatenatorLabel;
		private System.Windows.Forms.Label m_ctrlConcatenatorsLabel;
		private System.Windows.Forms.ComboBox m_ctrlConcatenators;
		private System.Windows.Forms.CheckBox m_ctrlConcatenate;
		
		/// <summary>Local member bound to ExportAsDatabase property</summary>
		private bool m_bExportAsDatabase = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFExportCodes()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFExportCodes()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of available columns.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of export columns.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of delimiters.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active delimiter.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the form's properties: SetMembers = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to insert the selected field.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to remove the selected field.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the CR/LF replacements list.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active CRLF replacement.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the concatenators list.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active concatenator.");
		
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
				m_ctrlExportColumns.CaseCodes = this.CaseCodes;
				m_ctrlExportColumns.ExportColumns = m_tmaxExportOptions.Columns;
				
				if(this.m_bExportAsDatabase == true)
					this.Text = "Export to Database";
				else
					this.Text = "Export to Text";
					
				if(FillDelimiters() == false)
					break;
					
				if(FillReplacements() == false)
					break;
					
				if(FillConcatenators() == false)
					break;
					
				if(Exchange(false) == false)
					break;
					
				if(this.ExportAsDatabase == true)
					HideTextControls();
					
				SetControlStates();
				
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			if(bSuccessful == false)
			{
				m_ctrlOk.Enabled = false;
			}
			
			base.OnLoad(e);
			
		}// private void OnLoad(object sender, System.EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to populate the list of available delimiters</summary>
		/// <returns>true if successful</returns>
		private bool FillDelimiters()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Add each of the enumerated delimiter values
				Array aDelimiters = Enum.GetValues(typeof(TmaxExportDelimiters));
				foreach(TmaxExportDelimiters O in aDelimiters)
				{
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
		
		/// <summary>This method is called to populate the list of available CRLF Replacements</summary>
		/// <returns>true if successful</returns>
		private bool FillReplacements()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Add each of the enumerated delimiter values
				Array aReplacements = Enum.GetValues(typeof(TmaxExportCRLF));
				foreach(TmaxExportCRLF O in aReplacements)
				{
					m_ctrlCRLFReplacements.Items.Add(CTmaxExportOptions.GetDisplayText(O));
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillReplacements", m_tmaxErrorBuilder.Message(ERROR_FILL_REPLACEMENTS_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool FillDelimiters()
		
		/// <summary>This method is called to populate the list of available Concatenators</summary>
		/// <returns>true if successful</returns>
		private bool FillConcatenators()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Add each of the enumerated delimiter values
				Array aConcatenators = Enum.GetValues(typeof(TmaxExportConcatenators));
				foreach(TmaxExportConcatenators O in aConcatenators)
				{
					m_ctrlConcatenators.Items.Add(CTmaxExportOptions.GetDisplayText(O));
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillConcatenators", m_tmaxErrorBuilder.Message(ERROR_FILL_CONCATENATORS_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool FillDelimiters()
		
		/// <summary>This method is called to set the active delimiter</summary>
		/// <param name="eDelimiter">the delimiter to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetDelimiter(TmaxExportDelimiters eDelimiter)
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
		
		/// <summary>This method is called to set the active CRLF replacement</summary>
		/// <param name="eReplacement">the replacement to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetReplacement(TmaxExportCRLF eReplacement)
		{
			bool	bSuccessful = false;
			int		iIndex = 0;
			
			if(m_ctrlCRLFReplacements.Items == null) return false;
			if(m_ctrlCRLFReplacements.Items.Count == 0) return false;
			
			try
			{
				if((iIndex = m_ctrlCRLFReplacements.FindStringExact(CTmaxExportOptions.GetDisplayText(eReplacement))) < 0)
					iIndex = 0;
					
				m_ctrlCRLFReplacements.SelectedIndex = iIndex;
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetReplacement", m_tmaxErrorBuilder.Message(ERROR_SET_REPLACEMENT_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool SetReplacement()
		
		/// <summary>This method is called to set the active CRLF replacement</summary>
		/// <param name="eConcatenator">the replacement to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetConcatenator(TmaxExportConcatenators eConcatenator)
		{
			bool	bSuccessful = false;
			int		iIndex = 0;
			
			if(m_ctrlConcatenators.Items == null) return false;
			if(m_ctrlConcatenators.Items.Count == 0) return false;
			
			try
			{
				if((iIndex = m_ctrlConcatenators.FindStringExact(CTmaxExportOptions.GetDisplayText(eConcatenator))) < 0)
					iIndex = 0;
					
				m_ctrlConcatenators.SelectedIndex = iIndex;
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetConcatenator", m_tmaxErrorBuilder.Message(ERROR_SET_CONCATENATOR_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool SetConcatenator()
		
		/// <summary>This method is called to get the active delimiter</summary>
		/// <returns>the selected delimiter</returns>
		private TmaxExportDelimiters GetDelimiter()
		{
			TmaxExportDelimiters eDelimiter = TmaxExportDelimiters.Tab;
			
			if(m_ctrlDelimiters.Items == null) return eDelimiter;
			if(m_ctrlDelimiters.Items.Count == 0) return eDelimiter;
			
			try
			{
				if(m_ctrlDelimiters.SelectedItem != null)
					eDelimiter = (TmaxExportDelimiters)(m_ctrlDelimiters.SelectedItem);
			}
			catch
			{
			}
			
			return eDelimiter; 
			
		}// private TmaxExportDelimiters GetDelimiter()
		
		/// <summary>This method is called to get the active CRLF Replacement</summary>
		/// <returns>the selected replacement</returns>
		private TmaxExportCRLF GetReplacement()
		{
			TmaxExportCRLF eReplacement = TmaxExportCRLF.None;
			
			if(m_ctrlCRLFReplacements.Items == null) return eReplacement;
			if(m_ctrlCRLFReplacements.Items.Count == 0) return eReplacement;
			
			try
			{
				if(m_ctrlCRLFReplacements.SelectedIndex >= 0)
					eReplacement = (TmaxExportCRLF)(m_ctrlCRLFReplacements.SelectedIndex);
			}
			catch
			{
			}
			
			return eReplacement; 
			
		}// private TmaxExportCRLF GetReplacement()
		
		/// <summary>This method is called to get the active Concatenator</summary>
		/// <returns>the selected concatenator</returns>
		private TmaxExportConcatenators GetConcatenator()
		{
			TmaxExportConcatenators eConcatenator = TmaxExportConcatenators.Comma;
			
			if(m_ctrlConcatenators.Items == null) return eConcatenator;
			if(m_ctrlConcatenators.Items.Count == 0) return eConcatenator;
			
			try
			{
				if(m_ctrlConcatenators.SelectedIndex >= 0)
					eConcatenator = (TmaxExportConcatenators)(m_ctrlConcatenators.SelectedIndex);
			}
			catch
			{
			}
			
			return eConcatenator; 
			
		}// private TmaxExportConcatenators GetConcatenator()
		
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
					m_tmaxExportOptions.AddQuotes = m_ctrlAddQuotes.Checked;
					m_tmaxExportOptions.SubBinders = m_ctrlSubBinders.Checked;
					m_tmaxExportOptions.ColumnHeaders = m_ctrlColumnHeaders.Checked;
					m_tmaxExportOptions.UseDefaults = m_ctrlUseDefaults.Checked;
					m_tmaxExportOptions.UserCRLF = m_ctrlUserCRLF.Text;
					m_tmaxExportOptions.Delimiter = GetDelimiter();
					m_tmaxExportOptions.CRLFReplacement = GetReplacement();
					m_tmaxExportOptions.Concatenate = m_ctrlConcatenate.Checked;
					m_tmaxExportOptions.Concatenator = GetConcatenator();
					m_tmaxExportOptions.UserConcatenator = m_ctrlUserConcatenator.Text;

				}
				else
				{
					m_ctrlAddQuotes.Checked = m_tmaxExportOptions.AddQuotes;
					m_ctrlSubBinders.Checked = m_tmaxExportOptions.SubBinders;
					m_ctrlColumnHeaders.Checked = m_tmaxExportOptions.ColumnHeaders;
					m_ctrlUseDefaults.Checked = m_tmaxExportOptions.UseDefaults;
					m_ctrlUserCRLF.Text = m_tmaxExportOptions.UserCRLF;
					m_ctrlConcatenate.Checked = m_tmaxExportOptions.Concatenate;
					m_ctrlUserConcatenator.Text = m_tmaxExportOptions.UserConcatenator;
							
					SetDelimiter(m_tmaxExportOptions.Delimiter);
					SetReplacement(m_tmaxExportOptions.CRLFReplacement);
					SetConcatenator(m_tmaxExportOptions.Concatenator);
				
				}// if(bSetMembers == true)
				
				//	Exchange the exported columns
				m_ctrlExportColumns.Exchange(bSetMembers);
				
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
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlDelimiters = new System.Windows.Forms.ComboBox();
			this.m_ctrlUseDefaults = new System.Windows.Forms.CheckBox();
			this.m_ctrlCRLFReplacementsLabel = new System.Windows.Forms.Label();
			this.m_ctrlCRLFReplacements = new System.Windows.Forms.ComboBox();
			this.m_ctrlUserCRLFLabel = new System.Windows.Forms.Label();
			this.m_ctrlUserCRLF = new System.Windows.Forms.TextBox();
			this.m_ctrlColumnHeaders = new System.Windows.Forms.CheckBox();
			this.m_ctrlSubBinders = new System.Windows.Forms.CheckBox();
			this.m_ctrlAddQuotes = new System.Windows.Forms.CheckBox();
			this.m_ctrlExportColumns = new FTI.Trialmax.Controls.CTmaxExportColumnsCtrl();
			this.m_ctrlDelimiterLabel = new System.Windows.Forms.Label();
			this.m_ctrlUserConcatenator = new System.Windows.Forms.TextBox();
			this.m_ctrlUserConcatenatorLabel = new System.Windows.Forms.Label();
			this.m_ctrlConcatenatorsLabel = new System.Windows.Forms.Label();
			this.m_ctrlConcatenators = new System.Windows.Forms.ComboBox();
			this.m_ctrlConcatenate = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(452, 368);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 12;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(360, 368);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 11;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlDelimiters
			// 
			this.m_ctrlDelimiters.Location = new System.Drawing.Point(364, 256);
			this.m_ctrlDelimiters.Name = "m_ctrlDelimiters";
			this.m_ctrlDelimiters.Size = new System.Drawing.Size(168, 21);
			this.m_ctrlDelimiters.TabIndex = 7;
			// 
			// m_ctrlUseDefaults
			// 
			this.m_ctrlUseDefaults.Location = new System.Drawing.Point(144, 348);
			this.m_ctrlUseDefaults.Name = "m_ctrlUseDefaults";
			this.m_ctrlUseDefaults.Size = new System.Drawing.Size(196, 24);
			this.m_ctrlUseDefaults.TabIndex = 13;
			this.m_ctrlUseDefaults.Text = "Use default value if unassigned";
			this.m_ctrlUseDefaults.Visible = false;
			// 
			// m_ctrlCRLFReplacementsLabel
			// 
			this.m_ctrlCRLFReplacementsLabel.Location = new System.Drawing.Point(364, 288);
			this.m_ctrlCRLFReplacementsLabel.Name = "m_ctrlCRLFReplacementsLabel";
			this.m_ctrlCRLFReplacementsLabel.Size = new System.Drawing.Size(164, 16);
			this.m_ctrlCRLFReplacementsLabel.TabIndex = 17;
			this.m_ctrlCRLFReplacementsLabel.Text = "Replace Hard Returns With:";
			// 
			// m_ctrlCRLFReplacements
			// 
			this.m_ctrlCRLFReplacements.Location = new System.Drawing.Point(364, 304);
			this.m_ctrlCRLFReplacements.Name = "m_ctrlCRLFReplacements";
			this.m_ctrlCRLFReplacements.Size = new System.Drawing.Size(168, 21);
			this.m_ctrlCRLFReplacements.TabIndex = 8;
			this.m_ctrlCRLFReplacements.SelectedIndexChanged += new System.EventHandler(this.OnSelIndexChanged);
			// 
			// m_ctrlUserCRLFLabel
			// 
			this.m_ctrlUserCRLFLabel.Location = new System.Drawing.Point(364, 336);
			this.m_ctrlUserCRLFLabel.Name = "m_ctrlUserCRLFLabel";
			this.m_ctrlUserCRLFLabel.Size = new System.Drawing.Size(84, 16);
			this.m_ctrlUserCRLFLabel.TabIndex = 9;
			this.m_ctrlUserCRLFLabel.Text = "User Defined:";
			// 
			// m_ctrlUserCRLF
			// 
			this.m_ctrlUserCRLF.Location = new System.Drawing.Point(448, 332);
			this.m_ctrlUserCRLF.Name = "m_ctrlUserCRLF";
			this.m_ctrlUserCRLF.Size = new System.Drawing.Size(80, 20);
			this.m_ctrlUserCRLF.TabIndex = 10;
			this.m_ctrlUserCRLF.Text = "";
			// 
			// m_ctrlColumnHeaders
			// 
			this.m_ctrlColumnHeaders.Location = new System.Drawing.Point(196, 320);
			this.m_ctrlColumnHeaders.Name = "m_ctrlColumnHeaders";
			this.m_ctrlColumnHeaders.Size = new System.Drawing.Size(164, 24);
			this.m_ctrlColumnHeaders.TabIndex = 6;
			this.m_ctrlColumnHeaders.Text = "Add column headers";
			// 
			// m_ctrlSubBinders
			// 
			this.m_ctrlSubBinders.Location = new System.Drawing.Point(196, 256);
			this.m_ctrlSubBinders.Name = "m_ctrlSubBinders";
			this.m_ctrlSubBinders.Size = new System.Drawing.Size(164, 24);
			this.m_ctrlSubBinders.TabIndex = 4;
			this.m_ctrlSubBinders.Text = "Include sub binders";
			// 
			// m_ctrlAddQuotes
			// 
			this.m_ctrlAddQuotes.Location = new System.Drawing.Point(196, 286);
			this.m_ctrlAddQuotes.Name = "m_ctrlAddQuotes";
			this.m_ctrlAddQuotes.Size = new System.Drawing.Size(164, 24);
			this.m_ctrlAddQuotes.TabIndex = 5;
			this.m_ctrlAddQuotes.Text = "Wrap text fields with quotes";
			// 
			// m_ctrlExportColumns
			// 
			this.m_ctrlExportColumns.CaseCodes = null;
			this.m_ctrlExportColumns.ExportColumns = null;
			this.m_ctrlExportColumns.Location = new System.Drawing.Point(1, -4);
			this.m_ctrlExportColumns.Name = "m_ctrlExportColumns";
			this.m_ctrlExportColumns.PaneId = 0;
			this.m_ctrlExportColumns.Size = new System.Drawing.Size(536, 248);
			this.m_ctrlExportColumns.TabIndex = 0;
			// 
			// m_ctrlDelimiterLabel
			// 
			this.m_ctrlDelimiterLabel.Location = new System.Drawing.Point(364, 240);
			this.m_ctrlDelimiterLabel.Name = "m_ctrlDelimiterLabel";
			this.m_ctrlDelimiterLabel.Size = new System.Drawing.Size(164, 16);
			this.m_ctrlDelimiterLabel.TabIndex = 26;
			this.m_ctrlDelimiterLabel.Text = "Delimiters";
			// 
			// m_ctrlUserConcatenator
			// 
			this.m_ctrlUserConcatenator.Location = new System.Drawing.Point(96, 332);
			this.m_ctrlUserConcatenator.Name = "m_ctrlUserConcatenator";
			this.m_ctrlUserConcatenator.Size = new System.Drawing.Size(84, 20);
			this.m_ctrlUserConcatenator.TabIndex = 3;
			this.m_ctrlUserConcatenator.Text = "";
			// 
			// m_ctrlUserConcatenatorLabel
			// 
			this.m_ctrlUserConcatenatorLabel.Location = new System.Drawing.Point(12, 336);
			this.m_ctrlUserConcatenatorLabel.Name = "m_ctrlUserConcatenatorLabel";
			this.m_ctrlUserConcatenatorLabel.Size = new System.Drawing.Size(84, 16);
			this.m_ctrlUserConcatenatorLabel.TabIndex = 2;
			this.m_ctrlUserConcatenatorLabel.Text = "User Defined:";
			// 
			// m_ctrlConcatenatorsLabel
			// 
			this.m_ctrlConcatenatorsLabel.Location = new System.Drawing.Point(12, 288);
			this.m_ctrlConcatenatorsLabel.Name = "m_ctrlConcatenatorsLabel";
			this.m_ctrlConcatenatorsLabel.Size = new System.Drawing.Size(164, 16);
			this.m_ctrlConcatenatorsLabel.TabIndex = 31;
			this.m_ctrlConcatenatorsLabel.Text = "Concatenate With:";
			// 
			// m_ctrlConcatenators
			// 
			this.m_ctrlConcatenators.Location = new System.Drawing.Point(12, 304);
			this.m_ctrlConcatenators.Name = "m_ctrlConcatenators";
			this.m_ctrlConcatenators.Size = new System.Drawing.Size(169, 21);
			this.m_ctrlConcatenators.TabIndex = 1;
			this.m_ctrlConcatenators.SelectedIndexChanged += new System.EventHandler(this.OnSelIndexChanged);
			// 
			// m_ctrlConcatenate
			// 
			this.m_ctrlConcatenate.Location = new System.Drawing.Point(12, 256);
			this.m_ctrlConcatenate.Name = "m_ctrlConcatenate";
			this.m_ctrlConcatenate.Size = new System.Drawing.Size(164, 24);
			this.m_ctrlConcatenate.TabIndex = 0;
			this.m_ctrlConcatenate.Text = "Concatenate Multiple Fields";
			this.m_ctrlConcatenate.Click += new System.EventHandler(this.OnClickConcatenate);
			// 
			// CFExportCodes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(538, 399);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlUserConcatenator);
			this.Controls.Add(this.m_ctrlUserCRLF);
			this.Controls.Add(this.m_ctrlUserConcatenatorLabel);
			this.Controls.Add(this.m_ctrlConcatenatorsLabel);
			this.Controls.Add(this.m_ctrlConcatenators);
			this.Controls.Add(this.m_ctrlConcatenate);
			this.Controls.Add(this.m_ctrlDelimiterLabel);
			this.Controls.Add(this.m_ctrlExportColumns);
			this.Controls.Add(this.m_ctrlColumnHeaders);
			this.Controls.Add(this.m_ctrlSubBinders);
			this.Controls.Add(this.m_ctrlAddQuotes);
			this.Controls.Add(this.m_ctrlUserCRLFLabel);
			this.Controls.Add(this.m_ctrlCRLFReplacementsLabel);
			this.Controls.Add(this.m_ctrlCRLFReplacements);
			this.Controls.Add(this.m_ctrlUseDefaults);
			this.Controls.Add(this.m_ctrlDelimiters);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFExportCodes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export Options";
			this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			//	There must be at least one column
			if(m_ctrlExportColumns.ExportCount == 0)
			{
				MessageBox.Show("You must specify at least one field to be exported", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
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

		/// <summary>This method handles events fired when the user changes the selection in one of the list boxes</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnSelIndexChanged(object sender, System.EventArgs e)
		{
			//	Make sure the appropriate controls are enabled / disabled
			SetControlStates();
			
		}// private void OnSelIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method handles events fired when the user changes the selection in one of the list boxes</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnClickConcatenate(object sender, System.EventArgs e)
		{
			//	Make sure the appropriate controls are enabled / disabled
			SetControlStates();
		}

		/// <summary>This method enables / disabled the controls based on current selections</summary>
		private void SetControlStates()
		{
			m_ctrlUserCRLF.Enabled = (GetReplacement() == TmaxExportCRLF.User);
			m_ctrlUserCRLFLabel.Enabled	= m_ctrlUserCRLF.Enabled;
			m_ctrlConcatenators.Enabled = (m_ctrlConcatenate.Checked == true);
			m_ctrlConcatenatorsLabel.Enabled	= m_ctrlConcatenators.Enabled;
			m_ctrlUserConcatenator.Enabled = ((m_ctrlConcatenators.Enabled == true) && (GetConcatenator() == TmaxExportConcatenators.User));
			m_ctrlUserConcatenatorLabel.Enabled	= m_ctrlUserConcatenator.Enabled;
		
		}// private void SetControlStates()

		/// <summary>This method hides the controls not required for exporting to database</summary>
		private void HideTextControls()
		{
			int iDelta = 0;
			
			try
			{
				m_ctrlAddQuotes.Visible = false;
				m_ctrlColumnHeaders.Visible = false;
				
				m_ctrlDelimiters.Visible = false;
				m_ctrlDelimiterLabel.Visible = false;

				m_ctrlCRLFReplacements.Visible = false;
				m_ctrlCRLFReplacementsLabel.Visible = false;
				m_ctrlUserCRLF.Visible = false;
				m_ctrlUserCRLFLabel.Visible = false;
			
//				m_ctrlConcatenate.Visible = false;
//				m_ctrlConcatenators.Visible = false;
//				m_ctrlConcatenatorsLabel.Visible = false;
//				m_ctrlUserConcatenator.Visible = false;
//				m_ctrlUserConcatenatorLabel.Visible = false;

				//	Keep track of how far we move the Ok button up
				iDelta = m_ctrlOk.Top - m_ctrlUserCRLF.Top;
				
				m_ctrlOk.SetBounds(0, m_ctrlUserCRLF.Top, 0, 0, BoundsSpecified.Y);
				m_ctrlCancel.SetBounds(0, m_ctrlUserCRLF.Top, 0, 0, BoundsSpecified.Y);
			
				m_ctrlSubBinders.SetBounds(m_ctrlDelimiters.Left, 0, 0, 0, BoundsSpecified.X);

				//	Resize the form
				this.Height = this.Height - iDelta;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "HideTextControls", Ex);
			}
			
		}// private void HideTextControls()

		#endregion Private Methods

		#region Properties
		
		/// <summary>True if exporting to database instead of text</summary>
		public bool ExportAsDatabase
		{
			get { return m_bExportAsDatabase;  }
			set { m_bExportAsDatabase = value; }
		}
		
		/// <summary>The application's collection of meta field objects</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes;  }
			set { m_tmaxCaseCodes = value; }
		}
		
		/// <summary>The user defined export options</summary>
		public CTmaxExportOptions ExportOptions
		{
			get { return m_tmaxExportOptions;  }
			set { m_tmaxExportOptions = value; }
		}
		
		#endregion Properties
	
	}// public class CFExportCodes : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
