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
	/// <summary>This form allows the user to define the active primary media filter</summary>
	public class CFFilterBuilder : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_FILL_CODES_EX				= ERROR_TMAX_FORM_MAX + 1;
		protected const int ERROR_FILL_TERMS_EX				= ERROR_TMAX_FORM_MAX + 2;
		protected const int ERROR_SET_CANCEL_FILTER_EX		= ERROR_TMAX_FORM_MAX + 3;
		protected const int ERROR_SELECT_OPERATOR_EX		= ERROR_TMAX_FORM_MAX + 4;
		protected const int ERROR_FILL_COMPARISONS_EX		= ERROR_TMAX_FORM_MAX + 5;
		protected const int ERROR_INITIALIZE_FILTER_GRID_EX	= ERROR_TMAX_FORM_MAX + 6;
		protected const int ERROR_EXCHANGE_EX				= ERROR_TMAX_FORM_MAX + 7;
		protected const int ERROR_ON_ADD_EX					= ERROR_TMAX_FORM_MAX + 8;
		protected const int ERROR_UPDATE_TERM_EX			= ERROR_TMAX_FORM_MAX + 9;
		protected const int ERROR_ACTIVATE_EX				= ERROR_TMAX_FORM_MAX + 10;
		
		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Ok button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>The custom grid used to display the filter terms</summary>
		private FTI.Trialmax.Controls.CTmaxFilterGridCtrl m_ctrlFilterGrid;

		/// <summary>The custom grid used to display the active case codes</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlCodes;

		/// <summary>The AND radio button used to set the filter operand</summary>
		private System.Windows.Forms.RadioButton m_ctrlAND;

		/// <summary>The OR radio button used to set the filter operand</summary>
		private System.Windows.Forms.RadioButton m_ctrlOR;

		/// <summary>The combobox control used to display the comparisons available for the active code</summary>
		private System.Windows.Forms.ComboBox m_ctrlComparisons;

		/// <summary>Local member bound to CaseCodes property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCodes m_tmaxCaseCodes = null;

		/// <summary>Local member bound to PickLists property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickLists = null;

		/// <summary>Local member to keep track of the active case code</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCode m_tmaxCaseCode = null;

		/// <summary>Local member to keep track of the active filter term</summary>
		private FTI.Shared.Trialmax.CTmaxFilterTerm m_tmaxFilterTerm = null;

		/// <summary>Local member bound to Filter property</summary>
		private FTI.Shared.Trialmax.CTmaxFilter m_tmaxFilter = null;

		/// <summary>Local member bound to Filter property</summary>
		private FTI.Shared.Trialmax.CTmaxFilter m_tmaxCancelFilter = null;
		
		/// <summary>Local member to manage available comparisons</summary>
		private System.Collections.ArrayList m_aComparisons = new ArrayList();
		
		/// <summary>Local member to keep track of comparisons</summary>
		private FTI.Shared.Trialmax.TmaxCodeTypes m_eComparisonType = TmaxCodeTypes.Unknown;
		
		/// <summary>Pushbutton to add a new filter term</summary>
		private System.Windows.Forms.Button m_ctrlAdd;
		
		/// <summary>Pushbutton to delete the existing filter term</summary>
		private System.Windows.Forms.Button m_ctrlDelete;
		
		/// <summary>Static text label for available operators</summary>
		private System.Windows.Forms.Label m_ctrlOperatorsLabel;
		
		/// <summary>Group box for available operators</summary>
		private System.Windows.Forms.GroupBox m_ctrlOperatorsGroup;
		
		/// <summary>Check box to allow user to select the NOT modifier</summary>
		private System.Windows.Forms.CheckBox m_ctrlModifier;
		
		/// <summary>Group box for filter terms controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlTermsGroup;
		
		/// <summary>Static text label for case codes list box</summary>
		private System.Windows.Forms.Label m_ctrlCodesLabel;
		
		/// <summary>Static text label for filter term modifier</summary>
		private System.Windows.Forms.Label m_ctrlModifierLabel;
		
		/// <summary>Static text label for filter term comparison</summary>
		private System.Windows.Forms.Label m_ctrlComparisonLabel;
		
		/// <summary>Pushbutton to clear all terms</summary>
		private System.Windows.Forms.Button m_ctrlClearAll;
		
		/// <summary>Static text label for Primary MediaID/Name value</summary>
		private System.Windows.Forms.Label m_ctrlPrimaryNameLabel;
		
		/// <summary>Edit box for Primary MediaID/Name value</summary>
		private System.Windows.Forms.TextBox m_ctrlPrimaryName;
		
		/// <summary>Pushbutton to view the resultant SQL statement</summary>
		private System.Windows.Forms.Button m_ctrlViewSQL;
		
		/// <summary>Pushbutton to apply the changes</summary>
		private System.Windows.Forms.Button m_ctrlApply;
		
		/// <summary>Static text label for filter term Value edit box</summary>
		private System.Windows.Forms.Label m_ctrlValueLabel;
		
		/// <summary>Pushbutton to clear the existing list of terms</summary>
		private System.Windows.Forms.Button m_ctrlClearTerms;
		
		/// <summary>Edit control for setting a term's value</summary>
		private FTI.Trialmax.Controls.CTmaxEditorCtrl m_ctrlValue;
		
		/// <summary>Check box to set AllTextFields property</summary>
		private System.Windows.Forms.CheckBox m_ctrlAllTextFields;
		
		/// <summary>Check box to request only treated documents</summary>
		private System.Windows.Forms.CheckBox m_ctrlIfTreated;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFFilterBuilder()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Filter Builder";
			m_tmaxEventSource.Attach(m_ctrlFilterGrid.EventSource);
			m_tmaxEventSource.Attach(m_ctrlValue.EventSource);
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

		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnClickOK(object sender, System.EventArgs e)
		{
			//	Don't need the cancel filter
			if(m_tmaxCancelFilter != null)
			{
				m_tmaxCancelFilter.Clear();
				m_tmaxCancelFilter = null;
			}
			
			//	Make sure to update the current selection
//			if(m_tmaxFilterTerm != null)
//				Update(m_tmaxFilterTerm, true, false);
				
			//	Get the primary media options
			ExchangePrimaryOptions(true);
			
			//	Close the dialog
			this.DialogResult = DialogResult.OK;
			this.Close();
		
		}// protected void OnClickOK(object sender, System.EventArgs e)
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnClickCancel(object sender, System.EventArgs e)
		{
			//	Get rid of our working filter
			if(m_tmaxFilter != null)
			{
				m_tmaxFilter.Clear();
				m_tmaxFilter = null;
			}
			
			//	Restore the original filter
			m_tmaxFilter = m_tmaxCancelFilter;
			
			//	Close the dialog
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// protected void OnClickCancel(object sender, System.EventArgs e)
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickApply(object sender, System.EventArgs e)
		{
			//	Make sure the active term has been updated
			if(m_tmaxFilterTerm != null)
				Update(m_tmaxFilterTerm, false, true);
					
			SetControlStates();
			
		}// private void OnClickApply(object sender, System.EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of case codes.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of filter terms.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the filter for cancellation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the filter operator controls.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of conditions.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the filter grid.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the filter term data: bSetMembers = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the new filter term.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the filter term.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to activate the filter term.");

		}// protected override void SetErrorStrings()

		/// <summary>This method is called when the form is loaded the first time</summary>
		/// <param name="e">system event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			bool bSuccessful = false;
			
			//	Perform all required initialization
			while(bSuccessful == false)
			{
				//	Make sure we can restore if cancelled
				if(SetCancelFilter() == false)
					break;

				//	Initialize the case codes list box
				if(FillCodes() == false)
					break;
					
				//	Initialize the filter grid control
				if(InitializeFilterGrid() == false)
					break;
					
				//	Initialize the operator radio buttons
				SelectOperator();
				
				//	Set the Primary Media options
				ExchangePrimaryOptions(false);
				
				//	Set the initial selection
				if(m_tmaxFilter.Terms.Count > 0)
					m_ctrlFilterGrid.SetSelected(m_tmaxFilter.Terms[0], false);
				else if(m_tmaxCaseCodes.Count > 0)
					m_ctrlCodes.SetSelected(m_tmaxCaseCodes[0], false);
					
				//	Set the maximum text length
				m_ctrlValue.MaxTextLength = CTmaxCaseCodes.CASE_CODES_MAX_TEXT_LENGTH;
				
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			SetControlStates();
			
			//	Do the base class processing
			base.OnLoad(e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>Required method for designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFFilterBuilder));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlOperatorsLabel = new System.Windows.Forms.Label();
			this.m_ctrlFilterGrid = new FTI.Trialmax.Controls.CTmaxFilterGridCtrl();
			this.m_ctrlCodes = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlAND = new System.Windows.Forms.RadioButton();
			this.m_ctrlOR = new System.Windows.Forms.RadioButton();
			this.m_ctrlComparisons = new System.Windows.Forms.ComboBox();
			this.m_ctrlAdd = new System.Windows.Forms.Button();
			this.m_ctrlDelete = new System.Windows.Forms.Button();
			this.m_ctrlClearAll = new System.Windows.Forms.Button();
			this.m_ctrlOperatorsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlPrimaryNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlPrimaryName = new System.Windows.Forms.TextBox();
			this.m_ctrlAllTextFields = new System.Windows.Forms.CheckBox();
			this.m_ctrlIfTreated = new System.Windows.Forms.CheckBox();
			this.m_ctrlModifier = new System.Windows.Forms.CheckBox();
			this.m_ctrlTermsGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlValue = new FTI.Trialmax.Controls.CTmaxEditorCtrl();
			this.m_ctrlApply = new System.Windows.Forms.Button();
			this.m_ctrlValueLabel = new System.Windows.Forms.Label();
			this.m_ctrlComparisonLabel = new System.Windows.Forms.Label();
			this.m_ctrlModifierLabel = new System.Windows.Forms.Label();
			this.m_ctrlCodesLabel = new System.Windows.Forms.Label();
			this.m_ctrlViewSQL = new System.Windows.Forms.Button();
			this.m_ctrlClearTerms = new System.Windows.Forms.Button();
			this.m_ctrlOperatorsGroup.SuspendLayout();
			this.m_ctrlTermsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(426, 420);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(80, 23);
			this.m_ctrlCancel.TabIndex = 8;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(338, 420);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(80, 23);
			this.m_ctrlOk.TabIndex = 7;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlOperatorsLabel
			// 
			this.m_ctrlOperatorsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOperatorsLabel.Location = new System.Drawing.Point(260, 24);
			this.m_ctrlOperatorsLabel.Name = "m_ctrlOperatorsLabel";
			this.m_ctrlOperatorsLabel.Size = new System.Drawing.Size(132, 20);
			this.m_ctrlOperatorsLabel.TabIndex = 9;
			this.m_ctrlOperatorsLabel.Text = "Combine All Terms With:";
			this.m_ctrlOperatorsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlFilterGrid
			// 
			this.m_ctrlFilterGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlFilterGrid.Filter = null;
			this.m_ctrlFilterGrid.Location = new System.Drawing.Point(8, 288);
			this.m_ctrlFilterGrid.Name = "m_ctrlFilterGrid";
			this.m_ctrlFilterGrid.PaneId = 0;
			this.m_ctrlFilterGrid.Size = new System.Drawing.Size(502, 124);
			this.m_ctrlFilterGrid.TabIndex = 3;
			this.m_ctrlFilterGrid.SelectedIndexChanged += new System.EventHandler(this.OnTermSelIndexChanged);
			// 
			// m_ctrlCodes
			// 
			this.m_ctrlCodes.AddTop = false;
			this.m_ctrlCodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCodes.AutoResizeColumns = true;
			this.m_ctrlCodes.ClearOnDblClick = false;
			this.m_ctrlCodes.DisplayMode = 0;
			this.m_ctrlCodes.HideSelection = false;
			this.m_ctrlCodes.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlCodes.MaxRows = 0;
			this.m_ctrlCodes.Name = "m_ctrlCodes";
			this.m_ctrlCodes.OwnerImages = null;
			this.m_ctrlCodes.PaneId = 0;
			this.m_ctrlCodes.SelectedIndex = -1;
			this.m_ctrlCodes.ShowHeaders = true;
			this.m_ctrlCodes.ShowImage = false;
			this.m_ctrlCodes.Size = new System.Drawing.Size(232, 128);
			this.m_ctrlCodes.TabIndex = 0;
			this.m_ctrlCodes.SelectedIndexChanged += new System.EventHandler(this.OnCodesSelIndexChanged);
			// 
			// m_ctrlAND
			// 
			this.m_ctrlAND.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAND.Location = new System.Drawing.Point(392, 24);
			this.m_ctrlAND.Name = "m_ctrlAND";
			this.m_ctrlAND.Size = new System.Drawing.Size(56, 20);
			this.m_ctrlAND.TabIndex = 0;
			this.m_ctrlAND.Text = "AND";
			this.m_ctrlAND.CheckedChanged += new System.EventHandler(this.OnOperatorChanged);
			// 
			// m_ctrlOR
			// 
			this.m_ctrlOR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOR.Location = new System.Drawing.Point(444, 24);
			this.m_ctrlOR.Name = "m_ctrlOR";
			this.m_ctrlOR.Size = new System.Drawing.Size(48, 20);
			this.m_ctrlOR.TabIndex = 1;
			this.m_ctrlOR.Text = "OR";
			this.m_ctrlOR.CheckedChanged += new System.EventHandler(this.OnOperatorChanged);
			// 
			// m_ctrlComparisons
			// 
			this.m_ctrlComparisons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlComparisons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlComparisons.Location = new System.Drawing.Point(290, 40);
			this.m_ctrlComparisons.Name = "m_ctrlComparisons";
			this.m_ctrlComparisons.Size = new System.Drawing.Size(204, 21);
			this.m_ctrlComparisons.TabIndex = 1;
			this.m_ctrlComparisons.SelectedIndexChanged += new System.EventHandler(this.OnComparisonsSelIndexChanged);
			// 
			// m_ctrlAdd
			// 
			this.m_ctrlAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAdd.Location = new System.Drawing.Point(256, 144);
			this.m_ctrlAdd.Name = "m_ctrlAdd";
			this.m_ctrlAdd.TabIndex = 3;
			this.m_ctrlAdd.Text = "&Add";
			this.m_ctrlAdd.Click += new System.EventHandler(this.OnAdd);
			// 
			// m_ctrlDelete
			// 
			this.m_ctrlDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlDelete.Location = new System.Drawing.Point(416, 144);
			this.m_ctrlDelete.Name = "m_ctrlDelete";
			this.m_ctrlDelete.TabIndex = 5;
			this.m_ctrlDelete.Text = "&Delete";
			this.m_ctrlDelete.Click += new System.EventHandler(this.OnDelete);
			// 
			// m_ctrlClearAll
			// 
			this.m_ctrlClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlClearAll.Location = new System.Drawing.Point(100, 420);
			this.m_ctrlClearAll.Name = "m_ctrlClearAll";
			this.m_ctrlClearAll.Size = new System.Drawing.Size(80, 23);
			this.m_ctrlClearAll.TabIndex = 5;
			this.m_ctrlClearAll.Text = "&Clear All";
			this.m_ctrlClearAll.Click += new System.EventHandler(this.OnClearAll);
			// 
			// m_ctrlOperatorsGroup
			// 
			this.m_ctrlOperatorsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOperatorsGroup.Controls.Add(this.m_ctrlOperatorsLabel);
			this.m_ctrlOperatorsGroup.Controls.Add(this.m_ctrlOR);
			this.m_ctrlOperatorsGroup.Controls.Add(this.m_ctrlAND);
			this.m_ctrlOperatorsGroup.Controls.Add(this.m_ctrlPrimaryNameLabel);
			this.m_ctrlOperatorsGroup.Controls.Add(this.m_ctrlPrimaryName);
			this.m_ctrlOperatorsGroup.Controls.Add(this.m_ctrlAllTextFields);
			this.m_ctrlOperatorsGroup.Controls.Add(this.m_ctrlIfTreated);
			this.m_ctrlOperatorsGroup.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlOperatorsGroup.Name = "m_ctrlOperatorsGroup";
			this.m_ctrlOperatorsGroup.Size = new System.Drawing.Size(500, 88);
			this.m_ctrlOperatorsGroup.TabIndex = 1;
			this.m_ctrlOperatorsGroup.TabStop = false;
			this.m_ctrlOperatorsGroup.Text = "Properties";
			// 
			// m_ctrlPrimaryNameLabel
			// 
			this.m_ctrlPrimaryNameLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlPrimaryNameLabel.Name = "m_ctrlPrimaryNameLabel";
			this.m_ctrlPrimaryNameLabel.Size = new System.Drawing.Size(228, 20);
			this.m_ctrlPrimaryNameLabel.TabIndex = 9;
			this.m_ctrlPrimaryNameLabel.Text = "MediaID or Name Contains:";
			this.m_ctrlPrimaryNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlPrimaryName
			// 
			this.m_ctrlPrimaryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPrimaryName.Location = new System.Drawing.Point(8, 48);
			this.m_ctrlPrimaryName.Name = "m_ctrlPrimaryName";
			this.m_ctrlPrimaryName.Size = new System.Drawing.Size(228, 20);
			this.m_ctrlPrimaryName.TabIndex = 0;
			this.m_ctrlPrimaryName.Text = "";
			// 
			// m_ctrlAllTextFields
			// 
			this.m_ctrlAllTextFields.Location = new System.Drawing.Point(16, 72);
			this.m_ctrlAllTextFields.Name = "m_ctrlAllTextFields";
			this.m_ctrlAllTextFields.Size = new System.Drawing.Size(204, 8);
			this.m_ctrlAllTextFields.TabIndex = 10;
			this.m_ctrlAllTextFields.Text = "Search All Text Fields";
			this.m_ctrlAllTextFields.Visible = false;
			// 
			// m_ctrlIfTreated
			// 
			this.m_ctrlIfTreated.Location = new System.Drawing.Point(260, 48);
			this.m_ctrlIfTreated.Name = "m_ctrlIfTreated";
			this.m_ctrlIfTreated.Size = new System.Drawing.Size(224, 20);
			this.m_ctrlIfTreated.TabIndex = 1;
			this.m_ctrlIfTreated.Text = "Only Treated Documents";
			// 
			// m_ctrlModifier
			// 
			this.m_ctrlModifier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlModifier.Location = new System.Drawing.Point(258, 40);
			this.m_ctrlModifier.Name = "m_ctrlModifier";
			this.m_ctrlModifier.Size = new System.Drawing.Size(24, 24);
			this.m_ctrlModifier.TabIndex = 20;
			// 
			// m_ctrlTermsGroup
			// 
			this.m_ctrlTermsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlValue);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlApply);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlValueLabel);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlComparisonLabel);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlModifierLabel);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlCodesLabel);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlModifier);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlComparisons);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlCodes);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlDelete);
			this.m_ctrlTermsGroup.Controls.Add(this.m_ctrlAdd);
			this.m_ctrlTermsGroup.Location = new System.Drawing.Point(8, 104);
			this.m_ctrlTermsGroup.Name = "m_ctrlTermsGroup";
			this.m_ctrlTermsGroup.Size = new System.Drawing.Size(502, 176);
			this.m_ctrlTermsGroup.TabIndex = 2;
			this.m_ctrlTermsGroup.TabStop = false;
			this.m_ctrlTermsGroup.Text = "Filter Terms";
			// 
			// m_ctrlValue
			// 
			this.m_ctrlValue.DropListValues = null;
			this.m_ctrlValue.FalseText = "False";
			this.m_ctrlValue.Location = new System.Drawing.Point(256, 92);
			this.m_ctrlValue.MaxTextLength = 255;
			this.m_ctrlValue.MemoAsText = false;
			this.m_ctrlValue.MultiLevel = null;
			this.m_ctrlValue.MultiLevelSelection = null;
			this.m_ctrlValue.Name = "m_ctrlValue";
			this.m_ctrlValue.PaneId = 0;
			this.m_ctrlValue.Size = new System.Drawing.Size(236, 48);
			this.m_ctrlValue.TabIndex = 25;
			this.m_ctrlValue.TrueText = "True";
			this.m_ctrlValue.Type = FTI.Trialmax.Controls.TmaxEditorCtrlTypes.Text;
			this.m_ctrlValue.UserAdditions = false;
			this.m_ctrlValue.Value = "";
			// 
			// m_ctrlApply
			// 
			this.m_ctrlApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlApply.Location = new System.Drawing.Point(336, 144);
			this.m_ctrlApply.Name = "m_ctrlApply";
			this.m_ctrlApply.TabIndex = 4;
			this.m_ctrlApply.Text = "A&pply";
			this.m_ctrlApply.Click += new System.EventHandler(this.OnClickApply);
			// 
			// m_ctrlValueLabel
			// 
			this.m_ctrlValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlValueLabel.Location = new System.Drawing.Point(254, 72);
			this.m_ctrlValueLabel.Name = "m_ctrlValueLabel";
			this.m_ctrlValueLabel.Size = new System.Drawing.Size(144, 16);
			this.m_ctrlValueLabel.TabIndex = 24;
			this.m_ctrlValueLabel.Text = "Value";
			// 
			// m_ctrlComparisonLabel
			// 
			this.m_ctrlComparisonLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlComparisonLabel.Location = new System.Drawing.Point(294, 24);
			this.m_ctrlComparisonLabel.Name = "m_ctrlComparisonLabel";
			this.m_ctrlComparisonLabel.Size = new System.Drawing.Size(140, 16);
			this.m_ctrlComparisonLabel.TabIndex = 23;
			this.m_ctrlComparisonLabel.Text = "Condition";
			// 
			// m_ctrlModifierLabel
			// 
			this.m_ctrlModifierLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlModifierLabel.Location = new System.Drawing.Point(250, 24);
			this.m_ctrlModifierLabel.Name = "m_ctrlModifierLabel";
			this.m_ctrlModifierLabel.Size = new System.Drawing.Size(32, 16);
			this.m_ctrlModifierLabel.TabIndex = 22;
			this.m_ctrlModifierLabel.Text = "NOT";
			this.m_ctrlModifierLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// m_ctrlCodesLabel
			// 
			this.m_ctrlCodesLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlCodesLabel.Name = "m_ctrlCodesLabel";
			this.m_ctrlCodesLabel.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlCodesLabel.TabIndex = 21;
			this.m_ctrlCodesLabel.Text = "Data Field:";
			// 
			// m_ctrlViewSQL
			// 
			this.m_ctrlViewSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlViewSQL.Location = new System.Drawing.Point(188, 420);
			this.m_ctrlViewSQL.Name = "m_ctrlViewSQL";
			this.m_ctrlViewSQL.Size = new System.Drawing.Size(80, 23);
			this.m_ctrlViewSQL.TabIndex = 6;
			this.m_ctrlViewSQL.Text = "&View SQL";
			this.m_ctrlViewSQL.Click += new System.EventHandler(this.OnViewSQL);
			// 
			// m_ctrlClearTerms
			// 
			this.m_ctrlClearTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlClearTerms.Location = new System.Drawing.Point(12, 420);
			this.m_ctrlClearTerms.Name = "m_ctrlClearTerms";
			this.m_ctrlClearTerms.Size = new System.Drawing.Size(80, 23);
			this.m_ctrlClearTerms.TabIndex = 4;
			this.m_ctrlClearTerms.Text = "Clear &Terms";
			this.m_ctrlClearTerms.Click += new System.EventHandler(this.OnClearTerms);
			// 
			// CFFilterBuilder
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(516, 449);
			this.Controls.Add(this.m_ctrlClearTerms);
			this.Controls.Add(this.m_ctrlViewSQL);
			this.Controls.Add(this.m_ctrlOperatorsGroup);
			this.Controls.Add(this.m_ctrlFilterGrid);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Controls.Add(this.m_ctrlTermsGroup);
			this.Controls.Add(this.m_ctrlClearAll);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFFilterBuilder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Filter Builder";
			this.m_ctrlOperatorsGroup.ResumeLayout(false);
			this.m_ctrlTermsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Called to populate the list of case codes</summary>
		/// <returns>true if successful</returns>
		private bool FillCodes()
		{
			bool bSuccessful = false;
			
			if(m_tmaxCaseCodes == null) return false;
			if(m_tmaxCaseCodes.Count == 0) return false;
			
			try
			{
				//	Initialize the list box to display case codes
				m_ctrlCodes.Initialize(new CTmaxCaseCode());
				
				//	Fill the list box
				m_ctrlCodes.Add(m_tmaxCaseCodes, true);

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillCodes", m_tmaxErrorBuilder.Message(ERROR_FILL_CODES_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool FillCodes()
		
		/// <summary>Called to populate the list of possible comparisons</summary>
		/// <returns>true if successful</returns>
		private bool FillComparisons()
		{
			bool					bSuccessful = false;
			TmaxCodeTypes			eType = TmaxCodeTypes.Unknown;
			TmaxFilterComparisons	eComparison = TmaxFilterComparisons.Equals;
			
			try
			{
				//	Get the type of active case code
				if(m_tmaxCaseCode != null)
					eType = m_tmaxCaseCode.Type;
					
				//	Don't bother if the type has not changed
				if(eType == m_eComparisonType) return true;
				
				//	Store the current selection if there is one
				eComparison = GetComparison();
				
				//	Clear the current comparisons
				m_aComparisons.Clear();
				m_ctrlComparisons.Items.Clear();
				m_eComparisonType = TmaxCodeTypes.Unknown;
							
				//	What type of code are we dealing with?
				switch(eType)
				{
					case TmaxCodeTypes.Boolean:
					case TmaxCodeTypes.PickList:

						m_aComparisons.Add(TmaxFilterComparisons.AnyValue);
						m_aComparisons.Add(TmaxFilterComparisons.Equals);
						break;

					case TmaxCodeTypes.Integer:
					case TmaxCodeTypes.Decimal:
					case TmaxCodeTypes.Date:
					
						m_aComparisons.Add(TmaxFilterComparisons.AnyValue);
						m_aComparisons.Add(TmaxFilterComparisons.LessThan);
						m_aComparisons.Add(TmaxFilterComparisons.LessThanEquals);
						m_aComparisons.Add(TmaxFilterComparisons.Equals);
						m_aComparisons.Add(TmaxFilterComparisons.GreaterThanEquals);
						m_aComparisons.Add(TmaxFilterComparisons.GreaterThan);
						break;
						
					case TmaxCodeTypes.Memo:
					case TmaxCodeTypes.Text:
					default:
					
						m_aComparisons.Add(TmaxFilterComparisons.AnyValue);
						m_aComparisons.Add(TmaxFilterComparisons.Equals);
						m_aComparisons.Add(TmaxFilterComparisons.StartsWith);
						m_aComparisons.Add(TmaxFilterComparisons.Contains);
						m_aComparisons.Add(TmaxFilterComparisons.EndsWith);
						break;
						
				}// switch(m_tmaxCaseCode.Type)
				
				m_eComparisonType = eType;

				//	Put the available comparisons in the list
				foreach(TmaxFilterComparisons O in m_aComparisons)
					m_ctrlComparisons.Items.Add(CTmaxFilter.GetDisplayText(O));
					
				//	Set the selection
				SetComparison(eComparison);
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillComparisons", m_tmaxErrorBuilder.Message(ERROR_FILL_COMPARISONS_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool FillComparisons()
		
		/// <summary>Called to get the current comparison selection</summary>
		/// <returns>The enumerated comparison</returns>
		private TmaxFilterComparisons GetComparison()
		{
			TmaxFilterComparisons	eComparison = TmaxFilterComparisons.Equals;
			int						iIndex = 0;
			
			try
			{
				//	Get the index of the selected comparison
				iIndex = m_ctrlComparisons.SelectedIndex;
				
				if((iIndex >= 0) && (iIndex < m_aComparisons.Count))
				{
					eComparison = (TmaxFilterComparisons)(m_aComparisons[iIndex]);
				}
				
			}
			catch
			{
			}
			
			return eComparison;
			
		}// private bool FillComparisons()
		
		/// <summary>Called to get set current comparison selection</summary>
		/// <param name="eComparison">The enumerated comparison identifier</param>
		/// <returns>True if successful</returns>
		private bool SetComparison(TmaxFilterComparisons eComparison)
		{
			int iIndex = -1;
			bool bSuccessful = false;
			
			if(m_ctrlComparisons.Items == null) return false;
			if(m_ctrlComparisons.Items.Count == 0) return false;
			
			try
			{
				//	Get the index of the requested comparison
				if(m_aComparisons != null)
					iIndex = m_aComparisons.IndexOf(eComparison);
				
				if((iIndex < 0) || (iIndex >= m_ctrlComparisons.Items.Count))
					iIndex = 0;

				m_ctrlComparisons.SelectedIndex = iIndex;
				
				bSuccessful = true;
				
			}
			catch
			{
			}
			
			return bSuccessful;
			
		}// private bool SetComparison(TmaxFilterComparions eComparison)
		
		/// <summary>Called to create a backup filter so that we can restore when the user cancels</summary>
		/// <returns>true if successful</returns>
		private bool SetCancelFilter()
		{
			bool bSuccessful = false;
			
			if(m_tmaxFilter == null) return false;
			
			try
			{
				//	Use the original filter to restore if cancelled
				m_tmaxCancelFilter = m_tmaxFilter;
				
				//	Make a copy of the original to be used by this form
				m_tmaxFilter = new CTmaxFilter();
				
				//	Copy the properties and terms in the source filter
				m_tmaxFilter.Copy(m_tmaxCancelFilter);
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetCancelFilter", m_tmaxErrorBuilder.Message(ERROR_SET_CANCEL_FILTER_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool FillCodes()
		
		/// <summary>Called to initialize the filter grid control</summary>
		/// <returns>true if successful</returns>
		private bool InitializeFilterGrid()
		{
			bool bSuccessful = false;
			
			if(m_tmaxFilter == null) return false;
			
			try
			{
				//	Set the filter displayed by the grid
				bSuccessful = m_ctrlFilterGrid.SetFilter(m_tmaxFilter);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeFilterGrid", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_FILTER_GRID_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool InitializeFilterGrid()
		
		/// <summary>This method selects the appropriate operator button</summary>
		/// <returns>true if successful</returns>
		private bool SelectOperator()
		{
			bool bSuccessful = false;
			
			if(m_tmaxFilter == null) return false;
			
			try
			{
				m_ctrlOR.Checked = (m_tmaxFilter.Operator == TmaxFilterOperators.OR);
				m_ctrlAND.Checked = (m_ctrlOR.Checked == false);
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SelectOperator", m_tmaxErrorBuilder.Message(ERROR_SELECT_OPERATOR_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool SelectOperator()
		
		/// <summary>This method is called when the active case code has changed</summary>
		/// <param name="bSynchronize">true to synchronize the Codes list box</param>
		/// <returns>true if no error occurs</returns>
		private bool OnCaseCodeChanged(bool bSynchronize)
		{
			TmaxEditorCtrlTypes	eEditType = TmaxEditorCtrlTypes.Text;
			bool				bSuccessful = false;
					
			try
			{
				//	Make sure the appropriate comparisons are available
				FillComparisons();
							
				//	Do we need to connect the pick list?
				if((m_tmaxCaseCode.Type == TmaxCodeTypes.PickList) && (m_tmaxCaseCode.PickList == null))
				{
					if((this.PickLists != null) && (m_tmaxCaseCode.PickListId > 0))
					{
						m_tmaxCaseCode.PickList = this.PickLists.FindList(m_tmaxCaseCode.PickListId);
					}
				}

				//	What editor should we be using?
				if(m_tmaxCaseCode != null)
					eEditType = m_ctrlValue.TranslateType(m_tmaxCaseCode);

				//	Should we change types?
				if(eEditType != m_ctrlValue.Type)
				{
					m_ctrlValue.Type = eEditType;
				}
				
				if(m_ctrlValue.Type == TmaxEditorCtrlTypes.Boolean)
				{
					m_ctrlValue.SetValue("true");
					m_ctrlModifier.Checked = false;
				}
				else if(m_ctrlValue.Type == TmaxEditorCtrlTypes.DropList)
				{
					if((m_tmaxCaseCode.PickList != null) && (m_tmaxCaseCode.PickList.Children != null))
					{
						m_ctrlValue.DropListValues = m_tmaxCaseCode.PickList.Children;
					}
					
				}
				else if(m_ctrlValue.Type == TmaxEditorCtrlTypes.MultiLevel)
				{
					if((m_tmaxCaseCode.PickList != null) && (m_tmaxCaseCode.PickList.Children != null))
					{
						m_ctrlValue.MultiLevel = m_tmaxCaseCode.PickList;
					}
					
				}
				else
				{
					m_ctrlComparisons.Enabled = (m_tmaxCaseCode != null);
				}
				
				if(bSynchronize == true)
					m_ctrlCodes.SetSelected(m_tmaxCaseCode, true);
					
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillComparisons", m_tmaxErrorBuilder.Message(ERROR_FILL_COMPARISONS_EX), Ex);
			}
			
			SetControlStates();
			
			return bSuccessful;
		
		}// private void OnCaseCodeChanged(bool bSynchronize)
		
		/// <summary>This method is called to set the active filter term</summary>
		/// <param name="tmaxTerm">The term to be activated</param>
		/// <param name="bSynchronize">true to synchronize the filter grid list box</param>
		/// <returns>true if no error occurs</returns>
		private bool Activate(CTmaxFilterTerm tmaxTerm, bool bSynchronize)
		{
			bool bSuccessful = false;
						
			try
			{
				//	Activate the caller's term
				m_tmaxFilterTerm = tmaxTerm;
				
				//	Set the child controls
				Exchange(tmaxTerm, false);

				if(bSynchronize == true)
					m_ctrlFilterGrid.SetSelected(tmaxTerm, true);
					
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Activate", m_tmaxErrorBuilder.Message(ERROR_ACTIVATE_EX), Ex);
			}
			
			SetControlStates();
			
			return bSuccessful;
		
		}// private bool Activate(CTmaxFilterTerm tmaxTerm, bool bSynchronize)
		
		/// <summary>This method is called to update the active filter term</summary>
		/// <param name="tmaxTerm">The term to be updated</param>
		/// <param name="bSilent">true to suppress warnings</param>
		/// <param name="bGrid">True to update the filter grid</param>
		/// <returns>true if no error occurs</returns>
		private bool Update(CTmaxFilterTerm tmaxTerm, bool bSilent, bool bGrid)
		{
			bool bSuccessful = false;
						
			try
			{
				//	Do we have an active filter term?
				if(tmaxTerm == null) return false;
				
				//	Check the current values
				if(CheckValues(bSilent) == true)
				{
					//	Get the new values
					if(Exchange(tmaxTerm, true) == true)
					{
						bSuccessful = true;
						
						//	Should we notify the grid?
						if(bGrid == true)
							m_ctrlFilterGrid.Update(tmaxTerm);
					}
				
				}// if(CheckValues(bSilent) == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Update", m_tmaxErrorBuilder.Message(ERROR_UPDATE_TERM_EX), Ex);
			}
			
			return bSuccessful;
		
		}// private bool Update(CTmaxFilterTerm tmaxTerm, bool bSilent, bool bGrid)
		
		/// <summary>Handles events fired when one of the Operator radio buttons check state changes</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnOperatorChanged(object sender, System.EventArgs e)
		{
			//	Update the active filter
			if(m_tmaxFilter != null)
			{
				TmaxFilterOperators Operator = TmaxFilterOperators.AND;
				
				if(m_ctrlOR.Checked == true)
					Operator = TmaxFilterOperators.OR;
				
				//	Has the value changed?
				if(m_tmaxFilter.Operator != Operator)
				{
					m_tmaxFilter.Operator = Operator;
					
					//	Notify the grid
					m_ctrlFilterGrid.UpdateOperator();
				}
			
			}// if(m_tmaxFilter != null)

		}// private void OnOperatorChanged(object sender, System.EventArgs e)

		/// <summary>This method handles events fired by the Codes list box when the selection changes</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnCodesSelIndexChanged(object sender, System.EventArgs e)
		{
			//	Get the index of the current selection
			if(m_ctrlCodes.GetSelected() != null)
			{
				m_tmaxCaseCode = (CTmaxCaseCode)(m_ctrlCodes.GetSelected());
				OnCaseCodeChanged(false);
			}
			
		}// private void OnCodesSelIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method handles events fired by the Comparisons list box when the selection changes</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnComparisonsSelIndexChanged(object sender, System.EventArgs e)
		{
			//	Update the control states
			SetControlStates();
			
		}// private void OnComparisonsSelIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method handles events fired by the Term list box when the selection changes</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnTermSelIndexChanged(object sender, System.EventArgs e)
		{
			//	Activate the selected filter term
			Activate(m_ctrlFilterGrid.GetSelected(), false);
			
		}// private void OnTermSelIndexChanged(object sender, System.EventArgs e)

		/// <summary>This method handles events fired when the user clicks on the Clear All button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClearAll(object sender, System.EventArgs e)
		{
			try
			{
				
				//	Reset the primary media members
				m_ctrlPrimaryName.Text = "";
				m_ctrlIfTreated.Checked = false;

				//	Clear the filter and it's grid
				OnClearTerms(sender, e);
					
			}
			catch
			{
				SetControlStates();
			}
			
		}// private void OnClearAll(object sender, System.EventArgs e)

		/// <summary>This method handles events fired when the user clicks on the Clear Terms button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClearTerms(object sender, System.EventArgs e)
		{
			try
			{
				
				//	Reset the local members
				m_tmaxFilterTerm = null;
				m_tmaxCaseCode = null;
				m_ctrlValue.SetValue("");

				//	Clear the filter and it's grid
				if(m_tmaxFilter != null)
					m_tmaxFilter.Clear();
				m_ctrlFilterGrid.SetFilter(m_tmaxFilter);
				
				//	Select the first case code
				if((m_tmaxCaseCodes != null) && (m_tmaxCaseCodes.Count > 0))
					m_ctrlCodes.SetSelected(m_tmaxCaseCodes[0], false);
					
			}
			catch
			{
			}
		
			SetControlStates();
			
		}// private void OnClearTerms(object sender, System.EventArgs e)

		/// <summary>This method handles events fired when the user clicks on the Add button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnAdd(object sender, System.EventArgs e)
		{
			CTmaxFilterTerm tmaxTerm = null;
			
			//	Make sure we have all the values
			if(CheckValues(false) == false) return;

			try
			{
				//	Allocate a new filter term and set it's values
				tmaxTerm = new CTmaxFilterTerm();
				
				//	Set the filter term properties
				if(Exchange(tmaxTerm, true) == true)
				{				
					//	Add to the active filter
					if(m_tmaxFilter.Add(tmaxTerm) != null)
					{
						//	Add to the grid
						m_ctrlFilterGrid.Add(tmaxTerm);
						
						//	Make the new term the current selection
						m_tmaxFilterTerm = null; // This prevents attempt to update
						m_ctrlFilterGrid.SetSelected(tmaxTerm, false);
					}
				
				}// if(Exchange(tmaxTerm, true) == true)
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAdd", m_tmaxErrorBuilder.Message(ERROR_ON_ADD_EX), Ex);
			}
		
			SetControlStates();
			
		}// private void OnAdd(object sender, System.EventArgs e)
		
		/// <summary>This method handles events fired when the user clicks on the Delete button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnDelete(object sender, System.EventArgs e)
		{
			int				iIndex = -1;
			CTmaxFilterTerm	tmaxDelete = null;
			
			//	Don't bother if no term selected
			if((tmaxDelete = m_tmaxFilterTerm) == null) 
				return;
			
			try
			{
				//	Get this item's index in the filter/grid
				if((iIndex = m_tmaxFilter.Terms.IndexOf(tmaxDelete)) < 0)
					return;
					
				//	Prevent any attempt to update
				m_tmaxFilterTerm = null;

				//	Remove this object from the grid
				m_ctrlFilterGrid.Remove(tmaxDelete);
				
				//	Remove it from the filter
				m_tmaxFilter.Terms.RemoveAt(iIndex);
				
				//	Do we still have terms in the filter?
				if(m_tmaxFilter.Terms.Count > 0)
				{
					if((iIndex >= 0) && (iIndex < m_tmaxFilter.Terms.Count))
						m_ctrlFilterGrid.SetSelectedIndex(iIndex, false);
					else
						m_ctrlFilterGrid.SetSelectedIndex(iIndex - 1, false);
				}
				else
				{
					//	Reset the local members
					m_tmaxFilterTerm = null;
					m_tmaxCaseCode = null;
					m_ctrlValue.SetValue("");
				
					//	Select the first case code
					if((m_tmaxCaseCodes != null) && (m_tmaxCaseCodes.Count > 0))
						m_ctrlCodes.SetSelected(m_tmaxCaseCodes[0], false);
				}		
			
			}
			catch
			{
			}
		
			SetControlStates();
			
		}// private void OnDelete(object sender, System.EventArgs e)
		
		/// <summary>This method is called to make sure the current values are valid</summary>
		/// <param name="bSilent">true to inhibit the warning message</param>
		/// <returns>true if all values are good</returns>
		private bool CheckValues(bool bSilent)
		{
			string strValue = "";
			bool	bValid = false;
			
			//	Make sure we have a valid code selection
			if(m_tmaxCaseCode == null)
				m_tmaxCaseCode = (CTmaxCaseCode)(m_ctrlCodes.GetSelected());
			if(m_tmaxCaseCode == null)
			{
				if(bSilent == false)
					Warn("You must select a valid data field from the list", null);
				return false;
			}
				
			//	Should we check the value?
			if(GetComparison() != TmaxFilterComparisons.AnyValue)
			{
				//	Make sure the user has supplied a value for this term
				strValue = m_ctrlValue.Value;
				if((strValue == null) || (strValue.Length == 0))
				{
					if(bSilent == false)
						Warn("You must supply a value for the filter term.", m_ctrlValue);
					return false;
				}
				
				//	Is this a pick list?
				if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
				{
					//	Make sure we have the pick list
					if((m_tmaxCaseCode.PickList == null) && (this.PickLists != null))
						m_tmaxCaseCode.PickList = this.PickLists.FindList(m_tmaxCaseCode.PickListId);
					
					if(m_tmaxCaseCode.PickList != null)
					{
						if(m_tmaxCaseCode.IsMultiLevel == true)
							bValid = (m_ctrlValue.MultiLevelSelection != null);
						else if(m_tmaxCaseCode.PickList.FindChild(strValue) != null)
							bValid = true;
					}
						
				}
				else
				{
					//	Is this value valid for the selected code
					if(m_tmaxCaseCode.IsValid(strValue) == true)
						bValid = true;
				
				}// if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
			
			}// if(GetComparison() != TmaxFilterComparisons.AnyValue)
			else
			{
				//	Any value is allowed
				bValid = true;
			}
			
			if((bSilent == false) && (bValid == false))
				Warn("The specified value is not valid for the selected data type", m_ctrlValue);

			return bValid;					
		
		}// private bool CheckValues()
		
		/// <summary>Called to exchange the values related to Primary Media options</summary>
		/// <returns>true if successful</returns>
		private bool ExchangePrimaryOptions(bool bSetMembers)
		{
			bool bSuccessful = false;
			
			if(m_tmaxFilter == null) return false;
			
			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					m_tmaxFilter.IfTreated = m_ctrlIfTreated.Checked;
					m_tmaxFilter.NameTerm.Value = m_ctrlPrimaryName.Text;
//					m_tmaxFilter.AllTextFields = m_ctrlAllTextFields.Checked;
					m_tmaxFilter.AllTextFields = false;
				}
				else
				{
					m_ctrlPrimaryName.Text = m_tmaxFilter.NameTerm.Value;
//					m_ctrlAllTextFields.Checked = m_tmaxFilter.AllTextFields;
					m_ctrlAllTextFields.Checked = false;
					m_ctrlIfTreated.Checked = m_tmaxFilter.IfTreated;
				}

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "ExchangePrimaryOptions", Ex);
			}
			
			return bSuccessful;
			
		}// private bool ExchangePrimaryOptions(bool bSetMembers)
		
		/// <summary>This method is called to exchange data between the active filter term and the child controls</summary>
		/// <param name="tmaxTerm">The term to use for the operation</param>
		/// <param name="bSetTerm">true to set the term members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(CTmaxFilterTerm tmaxTerm, bool bSetTerm)
		{
			bool bSuccessful = true;
			
			try
			{
				//	Set the filter term members?
				if(bSetTerm == true)
				{
					//	Make sure we have a valid term
					Debug.Assert(tmaxTerm != null);
					if(tmaxTerm == null) return false;

					tmaxTerm.CaseCode   = (CTmaxCaseCode)(m_ctrlCodes.GetSelected());
					tmaxTerm.Comparison = GetComparison();
					
					if(tmaxTerm.Comparison != TmaxFilterComparisons.AnyValue)
					{
						tmaxTerm.Value = m_ctrlValue.Value;
						tmaxTerm.MultiLevelSelection = m_ctrlValue.MultiLevelSelection;
					}
					else
					{
						tmaxTerm.Value = "";
						tmaxTerm.MultiLevelSelection = null;
					}
					
					if((m_ctrlModifier.Enabled == true) && (m_ctrlModifier.Checked == true))
						tmaxTerm.Modifier = TmaxFilterModifiers.NOT;
					else
						tmaxTerm.Modifier = TmaxFilterModifiers.None;
				}
				else
				{
					//	Is this a boolean term?
					if((tmaxTerm.CaseCode != null) && (tmaxTerm.CaseCode.Type == TmaxCodeTypes.Boolean))					
					{
						//	Is the user selecting a value?
						if(tmaxTerm.Comparison != TmaxFilterComparisons.AnyValue)
						{
							//	Do not allow the NOT modifier when specifying a value for the term
							if(tmaxTerm.Modifier == TmaxFilterModifiers.NOT)
							{
								tmaxTerm.Modifier = TmaxFilterModifiers.None;
								if((tmaxTerm.Value.Length == 0) || (CTmaxToolbox.StringToBool(tmaxTerm.Value) == false))
									tmaxTerm.Value = "True";// Instead of NOT false
								else
									tmaxTerm.Value = "False";// Instead of NOT true
							}
						
						}// if(tmaxTerm.Comparison != TmaxFilterComparisons.AnyValue)
						
					}// if((tmaxTerm.CaseCode != null) && (tmaxTerm.CaseCode.Type == TmaxCodeTypes.Boolean))
					
					//	Has the active case code changed?
					if(ReferenceEquals(m_tmaxCaseCode, tmaxTerm.CaseCode) == false)
						m_ctrlCodes.SetSelected(tmaxTerm.CaseCode, false);
						
					SetComparison(tmaxTerm.Comparison);
					m_ctrlModifier.Checked = (tmaxTerm.Modifier != TmaxFilterModifiers.None);
					m_ctrlValue.MultiLevelSelection = tmaxTerm.MultiLevelSelection;
					m_ctrlValue.SetValue(tmaxTerm.Value);
					
				}// if(bSetTerm == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetTerm), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// private bool Exchange(bool bSetTerm)
		
		private void OnViewSQL(object sender, System.EventArgs e)
		{
			System.IO.StreamWriter	stream = null;
			string					strFilename = "";
			string					strSQL = "";
			string					strMsg = "";
			
			Debug.Assert(m_tmaxFilter != null);
			if(m_tmaxFilter == null) return;
			
			//	Make sure to update the current selection
			if(m_tmaxFilterTerm != null)
				Update(m_tmaxFilterTerm, true, false);
				
			//	Get the name of the primary media to be included
			ExchangePrimaryOptions(true);
			
			//	Get the SQL statement
			strSQL = m_tmaxFilter.GetSQL(true);
			if(strSQL.Length == 0)
			{
				MessageBox.Show("No filter defined");
				return;
			}
			
			//	Get a temporary filename
			strFilename = System.IO.Path.GetTempFileName();
			strFilename = System.IO.Path.ChangeExtension(strFilename, "txt");
			
			//	Open the temporary file
			try
			{
				stream = new System.IO.StreamWriter(strFilename, false);
			}
			catch
			{
				//	Just use a message box to notify the user
				strMsg = String.Format("Unable to open {0} to save the SQL:\n\n{1}", strFilename, strSQL);
				MessageBox.Show(strMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
		
			//	Write the SQL to file
			stream.WriteLine(strSQL);
			stream.Close();
			
			//	Execute the viewer
			FTI.Shared.Win32.Shell.ShellExecute(this.Handle, "open", strFilename, "", "", FTI.Shared.Win32.User.SW_NORMAL);
			
		}// private void OnViewSQL(object sender, System.EventArgs e)

		/// <summary>This method enables / disabled the controls based on current selections</summary>
		private void SetControlStates()
		{
			m_ctrlApply.Enabled		 = (m_tmaxFilterTerm != null);
			m_ctrlDelete.Enabled	 = (m_tmaxFilterTerm != null);
			m_ctrlAdd.Enabled		 = (m_tmaxCaseCode != null);
			m_ctrlClearTerms.Enabled = (m_tmaxFilter.Terms.Count > 0);
			m_ctrlClearAll.Enabled	 = ((m_ctrlClearTerms.Enabled == true) || (m_ctrlPrimaryName.Text.Length > 0) || (m_ctrlIfTreated.Checked == true));
			
			if(m_tmaxCaseCode != null)
			{
				//	Is the user requesting all values?
				if(GetComparison() == TmaxFilterComparisons.AnyValue)
				{
					m_ctrlValueLabel.Enabled = false;
					m_ctrlValue.Enabled = false;
					m_ctrlModifier.Enabled = true;
				}
				else
				{
					m_ctrlValueLabel.Enabled = true;
					m_ctrlValue.Enabled = true;
				
					if(m_tmaxCaseCode.Type == TmaxCodeTypes.Boolean)
					{
						m_ctrlModifier.Enabled = false;
					}
					else if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
					{
						if((m_tmaxCaseCode.PickList != null) && (m_tmaxCaseCode.PickList.Children != null))
						{
							m_ctrlModifier.Enabled = (m_tmaxCaseCode.PickList.Children.Count > 0);
						}
						else
						{
							m_ctrlModifier.Enabled = false;
						}
					
					}
					else
					{
						m_ctrlModifier.Enabled = true;
					}
				
				}// if(GetComparison() == TmaxFilterComparisons.AnyValue)
				
			}
			else
			{
				m_ctrlValueLabel.Enabled = false;
				m_ctrlValue.Enabled = false;
				m_ctrlModifier.Enabled = false;
			
			}// if(m_tmaxCaseCode != null)
			
		}// private void SetControlStates()

		#endregion Private Methods

		#region Properties
		
		/// <summary>The collection of case codes for the active case</summary>
		public FTI.Shared.Trialmax.CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes; }
			set { m_tmaxCaseCodes = value; }
		}
		
		/// <summary>The collection of pick lists for the active case</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists; }
			set { m_tmaxPickLists = value; }
		}
		
		/// <summary>The filter to be configured</summary>
		public FTI.Shared.Trialmax.CTmaxFilter Filter
		{
			get { return m_tmaxFilter; }
			set { m_tmaxFilter = value; }
		}
		
		#endregion Properties

	}// public class CFFilterBuilder : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Forms
