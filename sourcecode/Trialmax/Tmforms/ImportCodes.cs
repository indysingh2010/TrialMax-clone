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

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFImportCodes : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_FILL_DELIMITERS_EX		= ERROR_TMAX_FORM_MAX + 1;
		protected const int ERROR_SET_DELIMITER_EX			= ERROR_TMAX_FORM_MAX + 2;
		protected const int ERROR_EXCHANGE_EX				= ERROR_TMAX_FORM_MAX + 3;
		protected const int ERROR_FILL_CONCATENATORS_EX		= ERROR_TMAX_FORM_MAX + 4;
		protected const int ERROR_SET_CONCATENATOR_EX		= ERROR_TMAX_FORM_MAX + 5;
		
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
		
		/// <summary>Static text label for delimiters text box</summary>
		private System.Windows.Forms.Label m_ctrlDelimiterLabel;
		
		/// <summary>Check box to select Overwrite Existing option</summary>
		private System.Windows.Forms.CheckBox m_ctrlOverwriteExisting;
		
		/// <summary>Check box to select Use Registration Options option</summary>
		private System.Windows.Forms.CheckBox m_ctrlUseRegistrationOptions;
		
		/// <summary>Text box to supply comment characters</summary>
		private System.Windows.Forms.TextBox m_ctrlCommentCharacters;
		
		/// <summary>Static text label for comment characters text box</summary>
		private System.Windows.Forms.Label m_ctrlCommentCharactersLabel;
		
		/// <summary>Text box to allow user to define the regular expression</summary>
		private System.Windows.Forms.TextBox m_ctrlExpression;
		
		/// <summary>Static text label for Expression text box</summary>
		private System.Windows.Forms.Label m_ctrlExpressionLabel;
		
		/// <summary>Text box to supply the user defined CRLF replacement characters</summary>
		private System.Windows.Forms.TextBox m_ctrlUserCRLF;
		
		/// <summary>Static label for the text box to supply the user defined CRLF replacement characters</summary>
		private System.Windows.Forms.Label m_ctrlUserCRLFLabel;
		
		/// <summary>Static text label for list of CRLF substitution characters</summary>
		private System.Windows.Forms.Label m_ctrlCRLFSubstitutionsLabel;
		
		/// <summary>Drop list of CRLF substitution characters</summary>
		private System.Windows.Forms.ComboBox m_ctrlCRLFSubstitutions;
		
		/// <summary>Group box for file processing options</summary>
		private System.Windows.Forms.GroupBox m_ctrlFileProcessingGroup;
		
		/// <summary>Group box for record related options</summary>
		private System.Windows.Forms.GroupBox m_ctrlRecordsGroup;
		
		/// <summary>Text box to supply the user defined Concatenator characters</summary>
		private System.Windows.Forms.TextBox m_ctrlUserConcatenator;
		
		/// <summary>Static text label for text box to supply the user defined Concatenator characters</summary>
		private System.Windows.Forms.Label m_ctrlUserConcatenatorLabel;
		
		/// <summary>Static text label for list of Concatenator characters</summary>
		private System.Windows.Forms.Label m_ctrlConcatenatorsLabel;
		
		/// <summary>Drop list of Concatenator characters</summary>
		private System.Windows.Forms.ComboBox m_ctrlConcatenators;
		
		/// <summary>Check box to request splitting of concatenated fields</summary>
		private System.Windows.Forms.CheckBox m_ctrlSplitConcatenated;

		/// <summary>Local member bound to ImportOptions property</summary>
		private CTmaxImportOptions m_tmaxImportOptions = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFImportCodes()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFImportCodes()

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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of concatenator characters.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active concatenator character.");
		
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
				if(FillDelimiters() == false)
					break;
					
				if(FillSubstitutions() == false)
					break;
					
				if(FillConcatenators() == false)
					break;
					
				if(Exchange(false) == false)
					break;
					
				OnSubstitutionIndexChanged(m_ctrlCRLFSubstitutions, System.EventArgs.Empty);
				OnSetConcatenateOption(m_ctrlSplitConcatenated, System.EventArgs.Empty);
				
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			if(bSuccessful == false)
			{
				m_ctrlOk.Enabled = false;
				m_ctrlExpression.Enabled = false;
				m_ctrlExpressionLabel.Enabled = false;
				m_ctrlUseRegistrationOptions.Enabled = false;
				m_ctrlOverwriteExisting.Enabled = false;
				m_ctrlDelimiters.Enabled = false;
				m_ctrlDelimiterLabel.Enabled = false;
				m_ctrlCommentCharacters.Enabled = false;
				m_ctrlCommentCharactersLabel.Enabled = false;
			}
			
			base.OnLoad(e);
			
		}// private void OnLoad(object sender, System.EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method handles events fired when the user changes the selection in the CRLF substitutions list box</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnSetConcatenateOption(object sender, System.EventArgs e)
		{
			m_ctrlConcatenators.Enabled = (m_ctrlSplitConcatenated.Checked == true);
			m_ctrlConcatenatorsLabel.Enabled	= m_ctrlConcatenators.Enabled;
			m_ctrlUserConcatenator.Enabled = ((m_ctrlConcatenators.Enabled == true) && (GetConcatenator() == TmaxImportConcatenators.User));
			m_ctrlUserConcatenatorLabel.Enabled	= m_ctrlUserConcatenator.Enabled;
			
		}// private void OnSetConcatenateOption(object sender, System.EventArgs e)

		/// <summary>This method handles events fired when the user changes the selection in the CRLF substitutions list box</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnSubstitutionIndexChanged(object sender, System.EventArgs e)
		{
			m_ctrlUserCRLFLabel.Enabled = (GetSubstitution() == TmaxImportCRLF.User);
			m_ctrlUserCRLF.Enabled = m_ctrlUserCRLFLabel.Enabled;
			
		}// private void OnSelIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method is called to populate the list of available Concatenators</summary>
		/// <returns>true if successful</returns>
		private bool FillConcatenators()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Add each of the enumerated delimiter values
				Array aConcatenators = Enum.GetValues(typeof(TmaxImportConcatenators));
				foreach(TmaxImportConcatenators O in aConcatenators)
				{
					m_ctrlConcatenators.Items.Add(O.ToString());
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillConcatenators", m_tmaxErrorBuilder.Message(ERROR_FILL_CONCATENATORS_EX), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool FillConcatenators()
		
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
		
		/// <summary>This method is called to get the active Concatenator</summary>
		/// <returns>the selected concatenator</returns>
		private TmaxImportConcatenators GetConcatenator()
		{
			TmaxImportConcatenators eConcatenator = TmaxImportConcatenators.Comma;
			
			if(m_ctrlConcatenators.Items == null) return eConcatenator;
			if(m_ctrlConcatenators.Items.Count == 0) return eConcatenator;
			
			try
			{
				if(m_ctrlConcatenators.SelectedIndex >= 0)
					eConcatenator = (TmaxImportConcatenators)(m_ctrlConcatenators.SelectedIndex);
			}
			catch
			{
			}
			
			return eConcatenator; 
			
		}// private TmaxImportConcatenators GetConcatenator()
		
		/// <summary>This method is called to set the active CRLF replacement</summary>
		/// <param name="eConcatenator">the replacement to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetConcatenator(TmaxImportConcatenators eConcatenator)
		{
			bool	bSuccessful = false;
			int		iIndex = 0;
			
			if(m_ctrlConcatenators.Items == null) return false;
			if(m_ctrlConcatenators.Items.Count == 0) return false;
			
			try
			{
				if((iIndex = m_ctrlConcatenators.FindStringExact(eConcatenator.ToString())) < 0)
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
		
		/// <summary>This method is called to set the active CRLF replacement</summary>
		/// <param name="eSubstitution">the replacement to be selected</param>
		/// <returns>true if successful</returns>
		private bool SetSubstitution(TmaxImportCRLF eSubstitution)
		{
			bool	bSuccessful = false;
			int		iIndex = 0;
			
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
					m_tmaxImportOptions.OverwriteCodes = m_ctrlOverwriteExisting.Checked;
					m_tmaxImportOptions.UseRegistrationOptions = m_ctrlUseRegistrationOptions.Checked;
					m_tmaxImportOptions.CommentCharacters = m_ctrlCommentCharacters.Text;
					m_tmaxImportOptions.Expression = m_ctrlExpression.Text;
					
					m_tmaxImportOptions.Delimiter = GetDelimiter();

					m_tmaxImportOptions.UserCRLF = m_ctrlUserCRLF.Text;
					m_tmaxImportOptions.CRLFSubstitution = GetSubstitution();

					m_tmaxImportOptions.SplitConcatenated = m_ctrlSplitConcatenated.Checked;
					m_tmaxImportOptions.Concatenator = GetConcatenator();
					m_tmaxImportOptions.UserConcatenator = m_ctrlUserConcatenator.Text;
				}
				else
				{
					m_ctrlOverwriteExisting.Checked = m_tmaxImportOptions.OverwriteCodes;
					m_ctrlUseRegistrationOptions.Checked = m_tmaxImportOptions.UseRegistrationOptions;
					m_ctrlCommentCharacters.Text = m_tmaxImportOptions.CommentCharacters;
					m_ctrlExpression.Text = m_tmaxImportOptions.Expression;
					m_ctrlUserCRLF.Text = m_tmaxImportOptions.UserCRLF;
					m_ctrlSplitConcatenated.Checked = m_tmaxImportOptions.SplitConcatenated;
					m_ctrlUserConcatenator.Text = m_tmaxImportOptions.UserConcatenator;
							
					SetSubstitution(m_tmaxImportOptions.CRLFSubstitution);
					SetDelimiter(m_tmaxImportOptions.Delimiter);
					SetConcatenator(m_tmaxImportOptions.Concatenator);
					
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFImportCodes));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlDelimiters = new System.Windows.Forms.ComboBox();
			this.m_ctrlDelimiterLabel = new System.Windows.Forms.Label();
			this.m_ctrlOverwriteExisting = new System.Windows.Forms.CheckBox();
			this.m_ctrlUseRegistrationOptions = new System.Windows.Forms.CheckBox();
			this.m_ctrlCommentCharacters = new System.Windows.Forms.TextBox();
			this.m_ctrlCommentCharactersLabel = new System.Windows.Forms.Label();
			this.m_ctrlExpression = new System.Windows.Forms.TextBox();
			this.m_ctrlExpressionLabel = new System.Windows.Forms.Label();
			this.m_ctrlUserCRLF = new System.Windows.Forms.TextBox();
			this.m_ctrlUserCRLFLabel = new System.Windows.Forms.Label();
			this.m_ctrlCRLFSubstitutionsLabel = new System.Windows.Forms.Label();
			this.m_ctrlCRLFSubstitutions = new System.Windows.Forms.ComboBox();
			this.m_ctrlFileProcessingGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlUserConcatenator = new System.Windows.Forms.TextBox();
			this.m_ctrlUserConcatenatorLabel = new System.Windows.Forms.Label();
			this.m_ctrlConcatenatorsLabel = new System.Windows.Forms.Label();
			this.m_ctrlConcatenators = new System.Windows.Forms.ComboBox();
			this.m_ctrlSplitConcatenated = new System.Windows.Forms.CheckBox();
			this.m_ctrlRecordsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlFileProcessingGroup.SuspendLayout();
			this.m_ctrlRecordsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(464, 240);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(380, 240);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 2;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlDelimiters
			// 
			this.m_ctrlDelimiters.Location = new System.Drawing.Point(196, 36);
			this.m_ctrlDelimiters.Name = "m_ctrlDelimiters";
			this.m_ctrlDelimiters.Size = new System.Drawing.Size(144, 21);
			this.m_ctrlDelimiters.TabIndex = 2;
			this.m_ctrlDelimiters.SelectedIndexChanged += new System.EventHandler(this.OnDelimiterIndexChanged);
			// 
			// m_ctrlDelimiterLabel
			// 
			this.m_ctrlDelimiterLabel.Location = new System.Drawing.Point(196, 20);
			this.m_ctrlDelimiterLabel.Name = "m_ctrlDelimiterLabel";
			this.m_ctrlDelimiterLabel.Size = new System.Drawing.Size(144, 16);
			this.m_ctrlDelimiterLabel.TabIndex = 12;
			this.m_ctrlDelimiterLabel.Text = "Delimiter";
			// 
			// m_ctrlOverwriteExisting
			// 
			this.m_ctrlOverwriteExisting.Location = new System.Drawing.Point(12, 52);
			this.m_ctrlOverwriteExisting.Name = "m_ctrlOverwriteExisting";
			this.m_ctrlOverwriteExisting.Size = new System.Drawing.Size(216, 24);
			this.m_ctrlOverwriteExisting.TabIndex = 1;
			this.m_ctrlOverwriteExisting.Text = "Overwrite existing fields";
			// 
			// m_ctrlUseRegistrationOptions
			// 
			this.m_ctrlUseRegistrationOptions.Location = new System.Drawing.Point(12, 24);
			this.m_ctrlUseRegistrationOptions.Name = "m_ctrlUseRegistrationOptions";
			this.m_ctrlUseRegistrationOptions.Size = new System.Drawing.Size(216, 24);
			this.m_ctrlUseRegistrationOptions.TabIndex = 0;
			this.m_ctrlUseRegistrationOptions.Text = "Apply registration options to Media ID";
			// 
			// m_ctrlCommentCharacters
			// 
			this.m_ctrlCommentCharacters.Location = new System.Drawing.Point(140, 37);
			this.m_ctrlCommentCharacters.Name = "m_ctrlCommentCharacters";
			this.m_ctrlCommentCharacters.Size = new System.Drawing.Size(48, 20);
			this.m_ctrlCommentCharacters.TabIndex = 0;
			this.m_ctrlCommentCharacters.Text = "";
			// 
			// m_ctrlCommentCharactersLabel
			// 
			this.m_ctrlCommentCharactersLabel.Location = new System.Drawing.Point(8, 37);
			this.m_ctrlCommentCharactersLabel.Name = "m_ctrlCommentCharactersLabel";
			this.m_ctrlCommentCharactersLabel.Size = new System.Drawing.Size(132, 20);
			this.m_ctrlCommentCharactersLabel.TabIndex = 14;
			this.m_ctrlCommentCharactersLabel.Text = "Comment lines start with:";
			this.m_ctrlCommentCharactersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlExpression
			// 
			this.m_ctrlExpression.Location = new System.Drawing.Point(352, 36);
			this.m_ctrlExpression.Name = "m_ctrlExpression";
			this.m_ctrlExpression.Size = new System.Drawing.Size(176, 20);
			this.m_ctrlExpression.TabIndex = 3;
			this.m_ctrlExpression.Text = "";
			// 
			// m_ctrlExpressionLabel
			// 
			this.m_ctrlExpressionLabel.Location = new System.Drawing.Point(352, 20);
			this.m_ctrlExpressionLabel.Name = "m_ctrlExpressionLabel";
			this.m_ctrlExpressionLabel.Size = new System.Drawing.Size(176, 16);
			this.m_ctrlExpressionLabel.TabIndex = 16;
			this.m_ctrlExpressionLabel.Text = "Regular Expression";
			// 
			// m_ctrlUserCRLF
			// 
			this.m_ctrlUserCRLF.Location = new System.Drawing.Point(352, 132);
			this.m_ctrlUserCRLF.Name = "m_ctrlUserCRLF";
			this.m_ctrlUserCRLF.Size = new System.Drawing.Size(92, 20);
			this.m_ctrlUserCRLF.TabIndex = 7;
			this.m_ctrlUserCRLF.Text = "";
			// 
			// m_ctrlUserCRLFLabel
			// 
			this.m_ctrlUserCRLFLabel.Location = new System.Drawing.Point(352, 116);
			this.m_ctrlUserCRLFLabel.Name = "m_ctrlUserCRLFLabel";
			this.m_ctrlUserCRLFLabel.Size = new System.Drawing.Size(76, 16);
			this.m_ctrlUserCRLFLabel.TabIndex = 22;
			this.m_ctrlUserCRLFLabel.Text = "User Defined:";
			// 
			// m_ctrlCRLFSubstitutionsLabel
			// 
			this.m_ctrlCRLFSubstitutionsLabel.Location = new System.Drawing.Point(196, 116);
			this.m_ctrlCRLFSubstitutionsLabel.Name = "m_ctrlCRLFSubstitutionsLabel";
			this.m_ctrlCRLFSubstitutionsLabel.Size = new System.Drawing.Size(144, 16);
			this.m_ctrlCRLFSubstitutionsLabel.TabIndex = 21;
			this.m_ctrlCRLFSubstitutionsLabel.Text = "Replace with Hard Return:";
			// 
			// m_ctrlCRLFSubstitutions
			// 
			this.m_ctrlCRLFSubstitutions.Location = new System.Drawing.Point(196, 132);
			this.m_ctrlCRLFSubstitutions.Name = "m_ctrlCRLFSubstitutions";
			this.m_ctrlCRLFSubstitutions.Size = new System.Drawing.Size(144, 21);
			this.m_ctrlCRLFSubstitutions.TabIndex = 6;
			this.m_ctrlCRLFSubstitutions.SelectedIndexChanged += new System.EventHandler(this.OnSubstitutionIndexChanged);
			// 
			// m_ctrlFileProcessingGroup
			// 
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlUserConcatenator);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlUserConcatenatorLabel);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlConcatenatorsLabel);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlConcatenators);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlSplitConcatenated);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlUserCRLFLabel);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlCRLFSubstitutionsLabel);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlCRLFSubstitutions);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlDelimiters);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlDelimiterLabel);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlExpression);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlExpressionLabel);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlUserCRLF);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlCommentCharacters);
			this.m_ctrlFileProcessingGroup.Controls.Add(this.m_ctrlCommentCharactersLabel);
			this.m_ctrlFileProcessingGroup.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlFileProcessingGroup.Name = "m_ctrlFileProcessingGroup";
			this.m_ctrlFileProcessingGroup.Size = new System.Drawing.Size(540, 168);
			this.m_ctrlFileProcessingGroup.TabIndex = 0;
			this.m_ctrlFileProcessingGroup.TabStop = false;
			this.m_ctrlFileProcessingGroup.Text = "Text Processing";
			// 
			// m_ctrlUserConcatenator
			// 
			this.m_ctrlUserConcatenator.Location = new System.Drawing.Point(352, 84);
			this.m_ctrlUserConcatenator.Name = "m_ctrlUserConcatenator";
			this.m_ctrlUserConcatenator.Size = new System.Drawing.Size(92, 20);
			this.m_ctrlUserConcatenator.TabIndex = 5;
			this.m_ctrlUserConcatenator.Text = "";
			// 
			// m_ctrlUserConcatenatorLabel
			// 
			this.m_ctrlUserConcatenatorLabel.Location = new System.Drawing.Point(352, 68);
			this.m_ctrlUserConcatenatorLabel.Name = "m_ctrlUserConcatenatorLabel";
			this.m_ctrlUserConcatenatorLabel.Size = new System.Drawing.Size(84, 16);
			this.m_ctrlUserConcatenatorLabel.TabIndex = 29;
			this.m_ctrlUserConcatenatorLabel.Text = "User Defined:";
			// 
			// m_ctrlConcatenatorsLabel
			// 
			this.m_ctrlConcatenatorsLabel.Location = new System.Drawing.Point(196, 68);
			this.m_ctrlConcatenatorsLabel.Name = "m_ctrlConcatenatorsLabel";
			this.m_ctrlConcatenatorsLabel.Size = new System.Drawing.Size(144, 16);
			this.m_ctrlConcatenatorsLabel.TabIndex = 28;
			this.m_ctrlConcatenatorsLabel.Text = "Split Character:";
			// 
			// m_ctrlConcatenators
			// 
			this.m_ctrlConcatenators.Location = new System.Drawing.Point(196, 84);
			this.m_ctrlConcatenators.Name = "m_ctrlConcatenators";
			this.m_ctrlConcatenators.Size = new System.Drawing.Size(144, 21);
			this.m_ctrlConcatenators.TabIndex = 4;
			this.m_ctrlConcatenators.SelectedIndexChanged += new System.EventHandler(this.OnSetConcatenateOption);
			// 
			// m_ctrlSplitConcatenated
			// 
			this.m_ctrlSplitConcatenated.Location = new System.Drawing.Point(12, 84);
			this.m_ctrlSplitConcatenated.Name = "m_ctrlSplitConcatenated";
			this.m_ctrlSplitConcatenated.Size = new System.Drawing.Size(176, 24);
			this.m_ctrlSplitConcatenated.TabIndex = 1;
			this.m_ctrlSplitConcatenated.Text = "Split Concatenated Text";
			this.m_ctrlSplitConcatenated.Click += new System.EventHandler(this.OnSetConcatenateOption);
			// 
			// m_ctrlRecordsGroup
			// 
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlOverwriteExisting);
			this.m_ctrlRecordsGroup.Controls.Add(this.m_ctrlUseRegistrationOptions);
			this.m_ctrlRecordsGroup.Location = new System.Drawing.Point(8, 180);
			this.m_ctrlRecordsGroup.Name = "m_ctrlRecordsGroup";
			this.m_ctrlRecordsGroup.Size = new System.Drawing.Size(356, 84);
			this.m_ctrlRecordsGroup.TabIndex = 1;
			this.m_ctrlRecordsGroup.TabStop = false;
			this.m_ctrlRecordsGroup.Text = "Records";
			// 
			// CFImportCodes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(554, 271);
			this.Controls.Add(this.m_ctrlRecordsGroup);
			this.Controls.Add(this.m_ctrlFileProcessingGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFImportCodes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Import Fielded Data Options";
			this.m_ctrlFileProcessingGroup.ResumeLayout(false);
			this.m_ctrlRecordsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			if(CheckValues() == true)
			{
				//	Get the user settings
				Exchange(true);
				
				//	Close the form
				DialogResult = DialogResult.OK;
				this.Close();
			
			}// if(CheckValues() == true)
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on a new selection in the Delimiters combo box</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnDelimiterIndexChanged(object sender, System.EventArgs e)
		{
			TmaxImportDelimiters eDelimiter = TmaxImportDelimiters.Tab;
			
			try { eDelimiter = (TmaxImportDelimiters)(m_ctrlDelimiters.SelectedIndex); }
			catch{}
			
			m_ctrlExpressionLabel.Enabled = (eDelimiter == TmaxImportDelimiters.Expression);
			m_ctrlExpression.Enabled = (eDelimiter == TmaxImportDelimiters.Expression);
			
		}// private void OnDelimiterIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method is called to verify that the user values are valid</summary>
		/// <returns>true if valid</returns>
		private bool CheckValues()
		{
			//	Nothing to check if not using regular expresssions
			if(GetDelimiter() != TmaxImportDelimiters.Expression) return true;
			
			//	MUST provide an expression
			if(m_ctrlExpression.Text.Length == 0)
			{
				MessageBox.Show("You must provide a valid regular expression", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				m_ctrlExpression.Focus();
				return false;
			}
			
			//	Make sure this is a valid expression
			try
			{
				Regex regEx = new System.Text.RegularExpressions.Regex(m_ctrlExpression.Text);
				return true;
			}
			catch
			{
				MessageBox.Show(m_ctrlExpression.Text + " is not a valid regular expression", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			
		}// private bool CheckValues()

		#endregion Private Methods

		#region Properties
		
		/// <summary>The user defined export options</summary>
		public CTmaxImportOptions ImportOptions
		{
			get { return m_tmaxImportOptions;  }
			set { m_tmaxImportOptions = value; }
		}
		
		#endregion Properties
	
	}// public class CFImportCodes : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
