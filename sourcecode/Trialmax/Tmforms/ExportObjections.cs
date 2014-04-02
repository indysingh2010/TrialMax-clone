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
	public class CFExportObjections : CFTmaxBaseForm
	{
		#region Constants

		/// <summary>Error message identifiers</summary>
		protected const int ERROR_FILL_DELIMITERS_EX = ERROR_TMAX_FORM_MAX + 1;
		protected const int ERROR_SET_DELIMITER_EX = ERROR_TMAX_FORM_MAX + 2;
		protected const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 3;
		protected const int ERROR_FILL_REPLACEMENTS_EX = ERROR_TMAX_FORM_MAX + 4;
		protected const int ERROR_SET_REPLACEMENT_EX = ERROR_TMAX_FORM_MAX + 5;

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

		/// <summary>Static text label for carraige return replacement list</summary>
		private System.Windows.Forms.Label m_ctrlCRLFReplacementsLabel;

		/// <summary>Combobox to select replacement characters for carraige returns</summary>
		private System.Windows.Forms.ComboBox m_ctrlCRLFReplacements;

		/// <summary>Static text label for user define carraige return replacement</summary>
		private System.Windows.Forms.Label m_ctrlUserCRLFLabel;

		/// <summary>User defined carraige return substitution characters</summary>
		private System.Windows.Forms.TextBox m_ctrlUserCRLF;

		/// <summary>Label for list of predefined delimiter characters</summary>
		private System.Windows.Forms.Label m_ctrlDelimiterLabel;

		/// <summary>Check box to request inclusion of sub-binders</summary>
		private System.Windows.Forms.CheckBox m_ctrlSubBinders;

		/// <summary>Check box to wrap text fields with quotes</summary>
		private System.Windows.Forms.CheckBox m_ctrlAddQuotes;

		/// <summary>Local member bound to ExportOptions property</summary>
		private CTmaxExportObjections m_tmaxExportOptions = null;

		#endregion Private Members

		#region Public Methods

		public CFExportObjections()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

		}// public CFExportObjections()

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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the CR/LF replacements list.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active CRLF replacement.");

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

				if(FillReplacements() == false)
					break;

				if(Exchange(false) == false)
					break;

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

		/// <summary>This method is called to set the active delimiter</summary>
		/// <param name="eDelimiter">the delimiter to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetDelimiter(TmaxExportDelimiters eDelimiter)
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
				m_tmaxEventSource.FireError(this, "SetDelimiter", m_tmaxErrorBuilder.Message(ERROR_SET_DELIMITER_EX), Ex);
			}

			return bSuccessful;

		}// private bool SetDelimiter()

		/// <summary>This method is called to set the active CRLF replacement</summary>
		/// <param name="eReplacement">the replacement to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetReplacement(TmaxExportCRLF eReplacement)
		{
			bool bSuccessful = false;
			int iIndex = 0;

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
					m_tmaxExportOptions.UserCRLF = m_ctrlUserCRLF.Text;
					m_tmaxExportOptions.Delimiter = GetDelimiter();
					m_tmaxExportOptions.CRLFReplacement = GetReplacement();
				}
				else
				{
					m_ctrlAddQuotes.Checked = m_tmaxExportOptions.AddQuotes;
					m_ctrlSubBinders.Checked = m_tmaxExportOptions.SubBinders;
					m_ctrlUserCRLF.Text = m_tmaxExportOptions.UserCRLF;

					SetDelimiter(m_tmaxExportOptions.Delimiter);
					SetReplacement(m_tmaxExportOptions.CRLFReplacement);

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFExportObjections));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlDelimiters = new System.Windows.Forms.ComboBox();
			this.m_ctrlCRLFReplacementsLabel = new System.Windows.Forms.Label();
			this.m_ctrlCRLFReplacements = new System.Windows.Forms.ComboBox();
			this.m_ctrlUserCRLFLabel = new System.Windows.Forms.Label();
			this.m_ctrlUserCRLF = new System.Windows.Forms.TextBox();
			this.m_ctrlSubBinders = new System.Windows.Forms.CheckBox();
			this.m_ctrlAddQuotes = new System.Windows.Forms.CheckBox();
			this.m_ctrlDelimiterLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(105, 208);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 6;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(16, 208);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 5;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlDelimiters
			// 
			this.m_ctrlDelimiters.Location = new System.Drawing.Point(16, 25);
			this.m_ctrlDelimiters.Name = "m_ctrlDelimiters";
			this.m_ctrlDelimiters.Size = new System.Drawing.Size(164, 21);
			this.m_ctrlDelimiters.TabIndex = 0;
			// 
			// m_ctrlCRLFReplacementsLabel
			// 
			this.m_ctrlCRLFReplacementsLabel.Location = new System.Drawing.Point(16, 57);
			this.m_ctrlCRLFReplacementsLabel.Name = "m_ctrlCRLFReplacementsLabel";
			this.m_ctrlCRLFReplacementsLabel.Size = new System.Drawing.Size(164, 16);
			this.m_ctrlCRLFReplacementsLabel.TabIndex = 17;
			this.m_ctrlCRLFReplacementsLabel.Text = "Replace Hard Returns With:";
			// 
			// m_ctrlCRLFReplacements
			// 
			this.m_ctrlCRLFReplacements.Location = new System.Drawing.Point(16, 73);
			this.m_ctrlCRLFReplacements.Name = "m_ctrlCRLFReplacements";
			this.m_ctrlCRLFReplacements.Size = new System.Drawing.Size(164, 21);
			this.m_ctrlCRLFReplacements.TabIndex = 1;
			this.m_ctrlCRLFReplacements.SelectedIndexChanged += new System.EventHandler(this.OnSelIndexChanged);
			// 
			// m_ctrlUserCRLFLabel
			// 
			this.m_ctrlUserCRLFLabel.Location = new System.Drawing.Point(15, 105);
			this.m_ctrlUserCRLFLabel.Name = "m_ctrlUserCRLFLabel";
			this.m_ctrlUserCRLFLabel.Size = new System.Drawing.Size(84, 16);
			this.m_ctrlUserCRLFLabel.TabIndex = 18;
			this.m_ctrlUserCRLFLabel.Text = "User Defined:";
			// 
			// m_ctrlUserCRLF
			// 
			this.m_ctrlUserCRLF.Location = new System.Drawing.Point(108, 101);
			this.m_ctrlUserCRLF.Name = "m_ctrlUserCRLF";
			this.m_ctrlUserCRLF.Size = new System.Drawing.Size(72, 20);
			this.m_ctrlUserCRLF.TabIndex = 2;
			// 
			// m_ctrlSubBinders
			// 
			this.m_ctrlSubBinders.Location = new System.Drawing.Point(16, 140);
			this.m_ctrlSubBinders.Name = "m_ctrlSubBinders";
			this.m_ctrlSubBinders.Size = new System.Drawing.Size(164, 24);
			this.m_ctrlSubBinders.TabIndex = 3;
			this.m_ctrlSubBinders.Text = "Include sub binders";
			// 
			// m_ctrlAddQuotes
			// 
			this.m_ctrlAddQuotes.Location = new System.Drawing.Point(16, 168);
			this.m_ctrlAddQuotes.Name = "m_ctrlAddQuotes";
			this.m_ctrlAddQuotes.Size = new System.Drawing.Size(164, 24);
			this.m_ctrlAddQuotes.TabIndex = 4;
			this.m_ctrlAddQuotes.Text = "Wrap text fields with quotes";
			// 
			// m_ctrlDelimiterLabel
			// 
			this.m_ctrlDelimiterLabel.Location = new System.Drawing.Point(16, 9);
			this.m_ctrlDelimiterLabel.Name = "m_ctrlDelimiterLabel";
			this.m_ctrlDelimiterLabel.Size = new System.Drawing.Size(164, 16);
			this.m_ctrlDelimiterLabel.TabIndex = 26;
			this.m_ctrlDelimiterLabel.Text = "Delimiters";
			// 
			// CFExportObjections
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(196, 243);
			this.Controls.Add(this.m_ctrlDelimiterLabel);
			this.Controls.Add(this.m_ctrlSubBinders);
			this.Controls.Add(this.m_ctrlAddQuotes);
			this.Controls.Add(this.m_ctrlUserCRLF);
			this.Controls.Add(this.m_ctrlUserCRLFLabel);
			this.Controls.Add(this.m_ctrlCRLFReplacementsLabel);
			this.Controls.Add(this.m_ctrlCRLFReplacements);
			this.Controls.Add(this.m_ctrlDelimiters);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFExportObjections";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Export Objections Options";
			this.ResumeLayout(false);
			this.PerformLayout();

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

		/// <summary>This method handles events fired when the user changes the selection in one of the list boxes</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnSelIndexChanged(object sender, System.EventArgs e)
		{
			//	Make sure the appropriate controls are enabled / disabled
			SetControlStates();

		}// private void OnSelIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method enables / disabled the controls based on current selections</summary>
		private void SetControlStates()
		{
			m_ctrlUserCRLF.Enabled = (GetReplacement() == TmaxExportCRLF.User);
			m_ctrlUserCRLFLabel.Enabled = m_ctrlUserCRLF.Enabled;

		}// private void SetControlStates()

		#endregion Private Methods

		#region Properties

		/// <summary>The user defined export options</summary>
		public CTmaxExportObjections ExportOptions
		{
			get { return m_tmaxExportOptions; }
			set { m_tmaxExportOptions = value; }
		}

		#endregion Properties

	}// public class CFExportObjections : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
