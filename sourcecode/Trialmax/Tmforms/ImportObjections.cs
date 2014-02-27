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
	public class CFImportObjections : CFTmaxBaseForm
	{
		#region Constants

		/// <summary>Error message identifiers</summary>
		protected const int ERROR_FILL_DELIMITERS_EX = ERROR_TMAX_FORM_MAX + 1;
		protected const int ERROR_SET_DELIMITER_EX = ERROR_TMAX_FORM_MAX + 2;
		protected const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 3;
		protected const int ERROR_SET_METHOD_EX = ERROR_TMAX_FORM_MAX + 4;

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Group box for file processing controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlProcessingGroup;

		/// <summary>Group box for record management controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlRecordsGroup;

		/// <summary>Combox box for available delimiters</summary>
		private System.Windows.Forms.ComboBox m_ctrlDelimiters;

		/// <summary>Static text label for delimiters text box</summary>
		private System.Windows.Forms.Label m_ctrlDelimiterLabel;

		/// <summary>Text box to supply comment characters</summary>
		private System.Windows.Forms.TextBox m_ctrlCommentCharacters;

		/// <summary>Static text label for comment characters text box</summary>
		private System.Windows.Forms.Label m_ctrlCommentCharactersLabel;

		/// <summary>Check box to request addition of unknown state identifiers</summary>
		private System.Windows.Forms.CheckBox m_ctrlAddStates;

		/// <summary>Check box to request addition of unknown ruling identifiers</summary>
		private CheckBox m_ctrlAddRulings;

		/// <summary>Text box to supply the user defined CRLF replacement characters</summary>
		private System.Windows.Forms.TextBox m_ctrlUserCRLF;

		/// <summary>Static label for the text box to supply the user defined CRLF replacement characters</summary>
		private System.Windows.Forms.Label m_ctrlUserCRLFLabel;

		/// <summary>Static text label for list of CRLF substitution characters</summary>
		private System.Windows.Forms.Label m_ctrlCRLFSubstitutionsLabel;

		/// <summary>Drop list of CRLF substitution characters</summary>
		private System.Windows.Forms.ComboBox m_ctrlCRLFSubstitutions;

		/// <summary>Radio button set to select the update method for the operation</summary>
		private Infragistics.Win.UltraWinEditors.UltraOptionSet m_ctrlMethod;

		/// <summary>Check box used to select the Discard GUID option</summary>
		private CheckBox m_ctrlDiscardGUID;

		/// <summary>Local member bound to ImportOptions property</summary>
		private CTmaxImportOptions m_tmaxImportOptions = null;

		#endregion Private Members

		#region Public Methods

		public CFImportObjections()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

		}// public CFImportObjections()

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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active import method: Method = %1");

		}// protected override void SetErrorStrings()

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);

		}// protected override void Dispose( bool disposing )

		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			bool bSuccessful = false;

			//	Initialize all the child controls
			while(bSuccessful == false)
			{
				if(FillDelimiters() == false)
					break;

				if(FillSubstitutions() == false)
					break;

				if(Exchange(false) == false)
					break;

				OnSubstitutionIndexChanged(m_ctrlCRLFSubstitutions,System.EventArgs.Empty);

				bSuccessful = true;

			}// while(bSuccessful == false)

			if(bSuccessful == false)
			{
				m_ctrlOk.Enabled = false;
				m_ctrlDelimiters.Enabled = false;
				m_ctrlDelimiterLabel.Enabled = false;
				m_ctrlCommentCharacters.Enabled = false;
				m_ctrlCommentCharactersLabel.Enabled = false;
				m_ctrlUserCRLF.Enabled = false;
				m_ctrlUserCRLFLabel.Enabled = false;
				m_ctrlCRLFSubstitutions.Enabled = false;
				m_ctrlCRLFSubstitutionsLabel.Enabled = false;
				m_ctrlMethod.Enabled = false;
				m_ctrlDiscardGUID.Enabled = false;
				m_ctrlAddRulings.Enabled = false;
				m_ctrlAddStates.Enabled = false;
			}
			else
			{
				SetControlStates();
			}

			base.OnLoad(e);

		}// private void OnLoad(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Private Methods

		/// <summary>This method handles events fired when the user changes the selection in the CRLF substitutions list box</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnSubstitutionIndexChanged(object sender,System.EventArgs e)
		{
			m_ctrlUserCRLFLabel.Enabled = (GetSubstitution() == TmaxImportCRLF.User);
			m_ctrlUserCRLF.Enabled = m_ctrlUserCRLFLabel.Enabled;

		}// private void OnSelIndexChanged(object sender, System.EventArgs e)

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
				m_tmaxEventSource.FireError(this,"FillDelimiters",m_tmaxErrorBuilder.Message(ERROR_FILL_DELIMITERS_EX),Ex);
			}

			return bSuccessful;

		}// private bool FillDelimiters()

		/// <summary>This method is called to set the active delimiter</summary>
		/// <param name="eDelimiter">the delimiter to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetDelimiter(TmaxImportDelimiters eDelimiter)
		{
			bool bSuccessful = false;
			int iIndex = 0;

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
				m_tmaxEventSource.FireError(this,"SetDelimiter",m_tmaxErrorBuilder.Message(ERROR_SET_DELIMITER_EX),Ex);
			}

			return bSuccessful;

		}// private bool SetDelimiter()

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

		/// <summary>This method is called to set the active import method</summary>
		/// <param name="eMethod">the enumerated method identifier</param>
		/// <returns>true if successful</returns>
		private bool SetMethod(TmaxImportObjectionMethods eMethod)
		{
			bool bSuccessful = false;

			try
			{
				switch(eMethod)
				{
					case TmaxImportObjectionMethods.AddAll:
						m_ctrlMethod.CheckedIndex = 2;
						break;

					case TmaxImportObjectionMethods.IgnoreExisting:
						m_ctrlMethod.CheckedIndex = 1;
						break;

					case TmaxImportObjectionMethods.UpdateExisting:
					default:
						m_ctrlMethod.CheckedIndex = 0;
						break;

				}// switch(eMethod)

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetMethod", m_tmaxErrorBuilder.Message(ERROR_SET_METHOD_EX, eMethod), Ex);
			}

			return bSuccessful;

		}// private bool SetMethod(TmaxImportObjectionMethods eMethod)

		/// <summary>This method is called to get the selected import method</summary>
		/// <returns>The import method selected by the user</returns>
		private TmaxImportObjectionMethods GetMethod()
		{
			TmaxImportObjectionMethods eMethod = TmaxImportObjectionMethods.UpdateExisting;

			try
			{
				if(m_ctrlMethod.CheckedIndex == 2)
					eMethod = TmaxImportObjectionMethods.AddAll;
				else if(m_ctrlMethod.CheckedIndex == 1)
					eMethod = TmaxImportObjectionMethods.IgnoreExisting;
				else
					eMethod = TmaxImportObjectionMethods.UpdateExisting;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetMethod", Ex);
			}

			return eMethod;

		}// private TmaxImportObjectionMethods GetMethod()

		/// <summary>This method is called to populate the list of available CRLF Substitutions</summary>
		/// <returns>true if successful</returns>
		private bool FillSubstitutions()
		{
			bool bSuccessful = false;

			try
			{
				//	Add each of the enumerated delimiter values
				Array aSubstitutions = Enum.GetValues(typeof(TmaxImportCRLF));
				foreach(TmaxImportCRLF O in aSubstitutions)
				{
					m_ctrlCRLFSubstitutions.Items.Add(CTmaxImportOptions.GetDisplayText(O));
				}

				bSuccessful = true;
			}
			catch
			{
			}

			return bSuccessful;

		}// private bool FillSubstitutions()

		/// <summary>This method is called to get the active CRLF Substitution</summary>
		/// <returns>the selected replacement</returns>
		private TmaxImportCRLF GetSubstitution()
		{
			TmaxImportCRLF eSubstitution = TmaxImportCRLF.None;

			if(m_ctrlCRLFSubstitutions.Items == null) return eSubstitution;
			if(m_ctrlCRLFSubstitutions.Items.Count == 0) return eSubstitution;

			try
			{
				if(m_ctrlCRLFSubstitutions.SelectedIndex >= 0)
					eSubstitution = (TmaxImportCRLF)(m_ctrlCRLFSubstitutions.SelectedIndex);
			}
			catch
			{
			}

			return eSubstitution;

		}// private private TmaxImportCRLF GetSubstitution()

		/// <summary>This method is called to set the active CRLF replacement</summary>
		/// <param name="eSubstitution">the replacement to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetSubstitution(TmaxImportCRLF eSubstitution)
		{
			bool bSuccessful = false;
			int iIndex = 0;

			if(m_ctrlCRLFSubstitutions.Items == null) return false;
			if(m_ctrlCRLFSubstitutions.Items.Count == 0) return false;

			try
			{
				if((iIndex = m_ctrlCRLFSubstitutions.FindStringExact(CTmaxImportOptions.GetDisplayText(eSubstitution))) < 0)
					iIndex = 0;

				m_ctrlCRLFSubstitutions.SelectedIndex = iIndex;

				bSuccessful = true;
			}
			catch
			{
			}

			return bSuccessful;

		}// private bool SetSubstitution()

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
					m_tmaxImportOptions.UserCRLF = m_ctrlUserCRLF.Text;
					m_tmaxImportOptions.CRLFSubstitution = GetSubstitution();
					m_tmaxImportOptions.AddObjectionStates = m_ctrlAddStates.Checked;
					m_tmaxImportOptions.AddObjectionRulings = m_ctrlAddRulings.Checked;
					m_tmaxImportOptions.DiscardObjectionsId = m_ctrlDiscardGUID.Checked;
					m_tmaxImportOptions.ObjectionsMethod = GetMethod();
				}
				else
				{
					m_ctrlCommentCharacters.Text = m_tmaxImportOptions.CommentCharacters;
					SetDelimiter(m_tmaxImportOptions.Delimiter);
					SetSubstitution(m_tmaxImportOptions.CRLFSubstitution);
					m_ctrlUserCRLF.Text = m_tmaxImportOptions.UserCRLF;
					m_ctrlAddStates.Checked = m_tmaxImportOptions.AddObjectionStates;
					m_ctrlAddRulings.Checked = m_tmaxImportOptions.AddObjectionRulings;
					m_ctrlDiscardGUID.Checked = m_tmaxImportOptions.DiscardObjectionsId;

					SetMethod(m_tmaxImportOptions.ObjectionsMethod);

				}// if(bSetMembers == true)

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this,"Exchange",m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX,bSetMembers),Ex);
			}

			return bSuccessful;

		}// private bool Exchange(bool bSetMembers)

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlDelimiters = new System.Windows.Forms.ComboBox();
			this.m_ctrlDelimiterLabel = new System.Windows.Forms.Label();
			this.m_ctrlCommentCharacters = new System.Windows.Forms.TextBox();
			this.m_ctrlCommentCharactersLabel = new System.Windows.Forms.Label();
			this.m_ctrlAddStates = new System.Windows.Forms.CheckBox();
			this.m_ctrlProcessingGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlUserCRLFLabel = new System.Windows.Forms.Label();
			this.m_ctrlCRLFSubstitutionsLabel = new System.Windows.Forms.Label();
			this.m_ctrlCRLFSubstitutions = new System.Windows.Forms.ComboBox();
			this.m_ctrlUserCRLF = new System.Windows.Forms.TextBox();
			this.m_ctrlRecordsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlDiscardGUID = new System.Windows.Forms.CheckBox();
			this.m_ctrlMethod = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
			this.m_ctrlAddRulings = new System.Windows.Forms.CheckBox();
			this.m_ctrlProcessingGroup.SuspendLayout();
			this.m_ctrlRecordsGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlMethod)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(215, 369);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(131, 369);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 2;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlDelimiters
			// 
			this.m_ctrlDelimiters.Location = new System.Drawing.Point(12, 72);
			this.m_ctrlDelimiters.Name = "m_ctrlDelimiters";
			this.m_ctrlDelimiters.Size = new System.Drawing.Size(259, 21);
			this.m_ctrlDelimiters.TabIndex = 1;
			// 
			// m_ctrlDelimiterLabel
			// 
			this.m_ctrlDelimiterLabel.Location = new System.Drawing.Point(11, 56);
			this.m_ctrlDelimiterLabel.Name = "m_ctrlDelimiterLabel";
			this.m_ctrlDelimiterLabel.Size = new System.Drawing.Size(172, 16);
			this.m_ctrlDelimiterLabel.TabIndex = 12;
			this.m_ctrlDelimiterLabel.Text = "Delimiter";
			// 
			// m_ctrlCommentCharacters
			// 
			this.m_ctrlCommentCharacters.Location = new System.Drawing.Point(148, 28);
			this.m_ctrlCommentCharacters.Name = "m_ctrlCommentCharacters";
			this.m_ctrlCommentCharacters.Size = new System.Drawing.Size(123, 20);
			this.m_ctrlCommentCharacters.TabIndex = 0;
			// 
			// m_ctrlCommentCharactersLabel
			// 
			this.m_ctrlCommentCharactersLabel.Location = new System.Drawing.Point(11, 28);
			this.m_ctrlCommentCharactersLabel.Name = "m_ctrlCommentCharactersLabel";
			this.m_ctrlCommentCharactersLabel.Size = new System.Drawing.Size(132, 20);
			this.m_ctrlCommentCharactersLabel.TabIndex = 14;
			this.m_ctrlCommentCharactersLabel.Text = "Comment lines start with";
			this.m_ctrlCommentCharactersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAddStates
			// 
			this.m_ctrlAddStates.Location = new System.Drawing.Point(11, 126);
			this.m_ctrlAddStates.Name = "m_ctrlAddStates";
			this.m_ctrlAddStates.Size = new System.Drawing.Size(217, 20);
			this.m_ctrlAddStates.TabIndex = 2;
			this.m_ctrlAddStates.Text = "Add unknown status identifiers";
			// 
			// m_ctrlProcessingGroup
			// 
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlUserCRLFLabel);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlCRLFSubstitutionsLabel);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlCRLFSubstitutions);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlUserCRLF);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlCommentCharactersLabel);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlCommentCharacters);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlDelimiterLabel);
			this.m_ctrlProcessingGroup.Controls.Add(this.m_ctrlDelimiters);
			this.m_ctrlProcessingGroup.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlProcessingGroup.Name = "m_ctrlProcessingGroup";
			this.m_ctrlProcessingGroup.Size = new System.Drawing.Size(287, 156);
			this.m_ctrlProcessingGroup.TabIndex = 0;
			this.m_ctrlProcessingGroup.TabStop = false;
			this.m_ctrlProcessingGroup.Text = "File Processing";
			// 
			// m_ctrlUserCRLFLabel
			// 
			this.m_ctrlUserCRLFLabel.Location = new System.Drawing.Point(153, 103);
			this.m_ctrlUserCRLFLabel.Name = "m_ctrlUserCRLFLabel";
			this.m_ctrlUserCRLFLabel.Size = new System.Drawing.Size(76, 16);
			this.m_ctrlUserCRLFLabel.TabIndex = 26;
			this.m_ctrlUserCRLFLabel.Text = "User Defined";
			// 
			// m_ctrlCRLFSubstitutionsLabel
			// 
			this.m_ctrlCRLFSubstitutionsLabel.Location = new System.Drawing.Point(12, 103);
			this.m_ctrlCRLFSubstitutionsLabel.Name = "m_ctrlCRLFSubstitutionsLabel";
			this.m_ctrlCRLFSubstitutionsLabel.Size = new System.Drawing.Size(132, 16);
			this.m_ctrlCRLFSubstitutionsLabel.TabIndex = 25;
			this.m_ctrlCRLFSubstitutionsLabel.Text = "Replace with Hard Return";
			// 
			// m_ctrlCRLFSubstitutions
			// 
			this.m_ctrlCRLFSubstitutions.Location = new System.Drawing.Point(12, 119);
			this.m_ctrlCRLFSubstitutions.Name = "m_ctrlCRLFSubstitutions";
			this.m_ctrlCRLFSubstitutions.Size = new System.Drawing.Size(132, 21);
			this.m_ctrlCRLFSubstitutions.TabIndex = 2;
			// 
			// m_ctrlUserCRLF
			// 
			this.m_ctrlUserCRLF.Location = new System.Drawing.Point(153, 119);
			this.m_ctrlUserCRLF.Name = "m_ctrlUserCRLF";
			this.m_ctrlUserCRLF.Size = new System.Drawing.Size(118, 20);
			this.m_ctrlUserCRLF.TabIndex = 3;
			// 
			// m_ctrlRecordsGroup
			// 
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlDiscardGUID);
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlMethod);
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlAddRulings);
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlAddStates);
			this.m_ctrlRecordsGroup.Location = new System.Drawing.Point(8, 176);
			this.m_ctrlRecordsGroup.Name = "m_ctrlRecordsGroup";
			this.m_ctrlRecordsGroup.Size = new System.Drawing.Size(287, 181);
			this.m_ctrlRecordsGroup.TabIndex = 1;
			this.m_ctrlRecordsGroup.TabStop = false;
			this.m_ctrlRecordsGroup.Text = "Records";
			// 
			// m_ctrlDiscardGUID
			// 
			this.m_ctrlDiscardGUID.Location = new System.Drawing.Point(11, 88);
			this.m_ctrlDiscardGUID.Name = "m_ctrlDiscardGUID";
			this.m_ctrlDiscardGUID.Size = new System.Drawing.Size(270, 24);
			this.m_ctrlDiscardGUID.TabIndex = 1;
			this.m_ctrlDiscardGUID.Text = "Discard Global Unique Identifiers && Case Names";
			this.m_ctrlDiscardGUID.UseVisualStyleBackColor = true;
			// 
			// m_ctrlMethod
			// 
			this.m_ctrlMethod.AccessibleDescription = "Import Method";
			this.m_ctrlMethod.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlMethod.CheckedIndex = 0;
			this.m_ctrlMethod.GlyphInfo = Infragistics.Win.UIElementDrawParams.StandardRadioButtonGlyphInfo;
			valueListItem1.DataValue = 0;
			valueListItem1.DisplayText = "Update Existing";
			valueListItem2.DataValue = 1;
			valueListItem2.DisplayText = "Ignore Existing";
			valueListItem3.DataValue = 2;
			valueListItem3.DisplayText = "Add All As New";
			this.m_ctrlMethod.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
			this.m_ctrlMethod.ItemSpacingVertical = 8;
			this.m_ctrlMethod.Location = new System.Drawing.Point(10, 21);
			this.m_ctrlMethod.Name = "m_ctrlMethod";
			this.m_ctrlMethod.Size = new System.Drawing.Size(207, 65);
			this.m_ctrlMethod.TabIndex = 0;
			this.m_ctrlMethod.Text = "Update Existing";
			this.m_ctrlMethod.ValueChanged += new System.EventHandler(this.OnSelMethod);
			// 
			// m_ctrlAddRulings
			// 
			this.m_ctrlAddRulings.Location = new System.Drawing.Point(11, 149);
			this.m_ctrlAddRulings.Name = "m_ctrlAddRulings";
			this.m_ctrlAddRulings.Size = new System.Drawing.Size(217, 20);
			this.m_ctrlAddRulings.TabIndex = 3;
			this.m_ctrlAddRulings.Text = "Add unknown ruling identifiers";
			// 
			// CFImportObjections
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(307, 404);
			this.Controls.Add(this.m_ctrlRecordsGroup);
			this.Controls.Add(this.m_ctrlProcessingGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFImportObjections";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Import Objections Options";
			this.m_ctrlProcessingGroup.ResumeLayout(false);
			this.m_ctrlProcessingGroup.PerformLayout();
			this.m_ctrlRecordsGroup.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlMethod)).EndInit();
			this.ResumeLayout(false);

		}

		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender,System.EventArgs e)
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
		private void OnClickCancel(object sender,System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();

		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on one of the Update Method radio buttons</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnSelMethod(object sender, EventArgs e)
		{
			SetControlStates();
		}

		/// <summary>Called to enable/disable the child controls</summary>
		private void SetControlStates()
		{
			m_ctrlDiscardGUID.Enabled = (m_ctrlMethod.CheckedIndex == 2);
		}

		#endregion Private Methods

		#region Properties

		/// <summary>The user defined export options</summary>
		public CTmaxImportOptions ImportOptions
		{
			get { return m_tmaxImportOptions; }
			set { m_tmaxImportOptions = value; }
		}

		#endregion Properties

	}// public class CFImportObjections : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
