using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;

namespace FTI.Trialmax.Panes
{
	/// <summary>This form allows the user to set the highlighter information</summary>
	public class CFSetHighlighters : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Constants
		
		private int MAX_HIGHLIGHTER_ROWS = 7;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>Array of group names used to initialize group selectors</summary>
		private string [] m_aGroupNames = null;
		
		/// <summary>Array of modified records</summary>
		private CDxMediaRecords m_dxModified = new CDxMediaRecords();
		
		/// <summary>Array of group name combobox controls</summary>
		private ArrayList m_aGroupCombos = new ArrayList();
		
		/// <summary>Array of color picker controls</summary>
		private ArrayList m_aColorButtons = new ArrayList();
		
		/// <summary>Array of name text edit controls</summary>
		private ArrayList m_aNameEditors = new ArrayList();
		
		/// <summary>Groups combobox for highlighter 1</summary>
		private Infragistics.Win.UltraWinGrid.UltraCombo m_ctrlGroups1;
		
		/// <summary>Groups combobox for highlighter 2</summary>
		private Infragistics.Win.UltraWinGrid.UltraCombo m_ctrlGroups2;
		
		/// <summary>Groups combobox for highlighter 3</summary>
		private Infragistics.Win.UltraWinGrid.UltraCombo m_ctrlGroups3;
		
		/// <summary>Groups combobox for highlighter 4</summary>
		private Infragistics.Win.UltraWinGrid.UltraCombo m_ctrlGroups4;
		
		/// <summary>Groups combobox for highlighter 5</summary>
		private Infragistics.Win.UltraWinGrid.UltraCombo m_ctrlGroups5;
		
		/// <summary>Groups combobox for highlighter 6</summary>
		private Infragistics.Win.UltraWinGrid.UltraCombo m_ctrlGroups6;
		
		/// <summary>Groups combobox for highlighter 7</summary>
		private Infragistics.Win.UltraWinGrid.UltraCombo m_ctrlGroups7;
		
		/// <summary>Name editor for highlighter 1</summary>
		private Infragistics.Win.UltraWinEditors.UltraTextEditor m_ctrlName1;
		
		/// <summary>Name editor for highlighter 2</summary>
		private Infragistics.Win.UltraWinEditors.UltraTextEditor m_ctrlName2;
		
		/// <summary>Name editor for highlighter 3</summary>
		private Infragistics.Win.UltraWinEditors.UltraTextEditor m_ctrlName3;
		
		/// <summary>Name editor for highlighter 4</summary>
		private Infragistics.Win.UltraWinEditors.UltraTextEditor m_ctrlName4;
		
		/// <summary>Name editor for highlighter 5</summary>
		private Infragistics.Win.UltraWinEditors.UltraTextEditor m_ctrlName5;
		
		/// <summary>Name editor for highlighter 6</summary>
		private Infragistics.Win.UltraWinEditors.UltraTextEditor m_ctrlName6;
		
		/// <summary>Name editor for highlighter 7</summary>
		private Infragistics.Win.UltraWinEditors.UltraTextEditor m_ctrlName7;
		
		/// <summary>Color button for highlighter 1</summary>
		private System.Windows.Forms.Button m_ctrlColorButton1;
		
		/// <summary>Color button for highlighter 2</summary>
		private System.Windows.Forms.Button m_ctrlColorButton2;
		
		/// <summary>Color button for highlighter 3</summary>
		private System.Windows.Forms.Button m_ctrlColorButton4;
		
		/// <summary>Color button for highlighter 4</summary>
		private System.Windows.Forms.Button m_ctrlColorButton3;
		
		/// <summary>Color button for highlighter 5</summary>
		private System.Windows.Forms.Button m_ctrlColorButton7;
		
		/// <summary>Color button for highlighter 6</summary>
		private System.Windows.Forms.Button m_ctrlColorButton6;
		
		/// <summary>Color button for highlighter 7</summary>
		private System.Windows.Forms.Button m_ctrlColorButton5;
		
		/// <summary>Color selection dialog box</summary>
		private System.Windows.Forms.ColorDialog m_ctrlGetColor;
		
		/// <summary>The database containing the highlighter information</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFSetHighlighters() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			//	Initialize the group descriptors
			m_aGroupNames = new string[] {"Plaintiff","Defendant","Other" };
			
			//	Initilize the group controls 
			m_aGroupCombos.Add(m_ctrlGroups1);
			m_aGroupCombos.Add(m_ctrlGroups2);
			m_aGroupCombos.Add(m_ctrlGroups3);
			m_aGroupCombos.Add(m_ctrlGroups4);
			m_aGroupCombos.Add(m_ctrlGroups5);
			m_aGroupCombos.Add(m_ctrlGroups6);
			m_aGroupCombos.Add(m_ctrlGroups7);
			
			foreach(UltraCombo O in m_aGroupCombos)
				O.DataSource = m_aGroupNames;

			//	Initilize the color picker controls 
			m_aColorButtons.Add(m_ctrlColorButton1);
			m_aColorButtons.Add(m_ctrlColorButton2);
			m_aColorButtons.Add(m_ctrlColorButton3);
			m_aColorButtons.Add(m_ctrlColorButton4);
			m_aColorButtons.Add(m_ctrlColorButton5);
			m_aColorButtons.Add(m_ctrlColorButton6);
			m_aColorButtons.Add(m_ctrlColorButton7);
			
			//	Initilize the name text editor controls 
			m_aNameEditors.Add(m_ctrlName1);
			m_aNameEditors.Add(m_ctrlName2);
			m_aNameEditors.Add(m_ctrlName3);
			m_aNameEditors.Add(m_ctrlName4);
			m_aNameEditors.Add(m_ctrlName5);
			m_aNameEditors.Add(m_ctrlName6);
			m_aNameEditors.Add(m_ctrlName7);
			
		}// public CFSetHighlighters() : base()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to set the control values for the specified row of controls</summary>
		/// <param name="iIndex">The zero-based row index</param>
		protected void SetControls(int iIndex)
		{
			CDxHighlighter	dxHighlighter = null;
			Button			colorButton = (Button)m_aColorButtons[iIndex];
			UltraTextEditor	ultraName   = (UltraTextEditor)m_aNameEditors[iIndex];
			UltraCombo		ultraGroups = (UltraCombo)m_aGroupCombos[iIndex];
			int				iGroup = -1;
			
			//	Get the highlighter for this row
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			{
				if(iIndex <= m_tmaxDatabase.Highlighters.Count)
					dxHighlighter = m_tmaxDatabase.Highlighters[iIndex];
			}
			
			if(dxHighlighter != null)
			{
				iGroup = (int)dxHighlighter.Group;
				if((iGroup >= 0) && (iGroup < 3)) 
					ultraGroups.SelectedRow = ultraGroups.Rows[iGroup];
					
				colorButton.BackColor = dxHighlighter.SysColor;
				ultraName.Text = dxHighlighter.Name;
			}
			else
			{
				ultraGroups.Enabled = false;
				colorButton.BackColor = System.Drawing.SystemColors.Control;
				colorButton.Enabled = false;
				ultraName.Text = "";
				ultraName.Enabled = false;
			}
			
		}// protected void SetControls(int iIndex)
		
		/// <summary>This method is called to set the properties for the specified highlighter</summary>
		/// <param name="iIndex">The zero-based row index</param>
		protected void SetProps(int iIndex)
		{
			CDxHighlighter			dxHighlighter = null;
			Button					colorButton = (Button)m_aColorButtons[iIndex];
			UltraTextEditor			ultraName = (UltraTextEditor)m_aNameEditors[iIndex];
			UltraCombo				ultraGroups = (UltraCombo)m_aGroupCombos[iIndex];
			bool					bModified = false;
			TmaxHighlighterGroups	eGroup = TmaxHighlighterGroups.Other;
			
			//	Get the highlighter for this row
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			{
				if(iIndex <= m_tmaxDatabase.Highlighters.Count)
					dxHighlighter = m_tmaxDatabase.Highlighters[iIndex];
			}
			
			if(dxHighlighter == null) return;
			
			if(ultraGroups.SelectedRow != null)
				eGroup = (TmaxHighlighterGroups)ultraGroups.SelectedRow.Index;
			
			if(eGroup != dxHighlighter.Group)
			{
				dxHighlighter.Group = eGroup;
				bModified = true;
			}
			if(colorButton.BackColor != dxHighlighter.SysColor)
			{	
				dxHighlighter.SysColor = colorButton.BackColor;
				bModified = true;
			}
			if(ultraName.Text != dxHighlighter.Name)
			{	
				dxHighlighter.Name = ultraName.Text;
				bModified = true;
			}
			
			if(bModified == true)
			{
				m_tmaxDatabase.Highlighters.Update(dxHighlighter);
				m_dxModified.AddList(dxHighlighter);
			}
			
		}// protected void SetProps(int iIndex)
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				
				if(m_dxModified != null)
				{
					m_dxModified.Clear();
				}
				if(m_aNameEditors != null)
				{
					m_aNameEditors.Clear();
				}
				if(m_aColorButtons != null)
				{
					m_aColorButtons.Clear();
				}
				if(m_aGroupCombos != null)
				{
					m_aGroupCombos.Clear();
				}
			
			}
			base.Dispose( disposing );
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			//	Initialize all the controls
			for(int i = 0; i < MAX_HIGHLIGHTER_ROWS; i++)
				SetControls(i);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFSetHighlighters));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlName1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.m_ctrlGroups1 = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.m_ctrlGroups2 = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.m_ctrlGroups4 = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.m_ctrlGroups3 = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.m_ctrlGroups7 = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.m_ctrlGroups6 = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.m_ctrlGroups5 = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.m_ctrlName2 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.m_ctrlName4 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.m_ctrlName3 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.m_ctrlName7 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.m_ctrlName6 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.m_ctrlName5 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.m_ctrlColorButton1 = new System.Windows.Forms.Button();
			this.m_ctrlColorButton2 = new System.Windows.Forms.Button();
			this.m_ctrlColorButton4 = new System.Windows.Forms.Button();
			this.m_ctrlColorButton3 = new System.Windows.Forms.Button();
			this.m_ctrlColorButton7 = new System.Windows.Forms.Button();
			this.m_ctrlColorButton6 = new System.Windows.Forms.Button();
			this.m_ctrlColorButton5 = new System.Windows.Forms.Button();
			this.m_ctrlGetColor = new System.Windows.Forms.ColorDialog();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups5)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(284, 200);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 22;
			this.m_ctrlCancel.TabStop = false;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOK.Location = new System.Drawing.Point(196, 200);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 21;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlName1
			// 
			this.m_ctrlName1.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlName1.Location = new System.Drawing.Point(164, 8);
			this.m_ctrlName1.Name = "m_ctrlName1";
			this.m_ctrlName1.Size = new System.Drawing.Size(196, 21);
			this.m_ctrlName1.TabIndex = 2;
			// 
			// m_ctrlGroups1
			// 
			ultraGridBand1.ColHeadersVisible = false;
			ultraGridBand1.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.RowSpacingAfter = 0;
			ultraGridBand1.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups1.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.m_ctrlGroups1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlGroups1.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
			this.m_ctrlGroups1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups1.DisplayLayout.Override.RowSpacingAfter = 0;
			this.m_ctrlGroups1.DisplayLayout.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups1.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
			this.m_ctrlGroups1.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlGroups1.DisplayMember = "";
			this.m_ctrlGroups1.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlGroups1.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.m_ctrlGroups1.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlGroups1.Name = "m_ctrlGroups1";
			this.m_ctrlGroups1.Size = new System.Drawing.Size(108, 21);
			this.m_ctrlGroups1.TabIndex = 0;
			this.m_ctrlGroups1.ValueMember = "";
			// 
			// m_ctrlGroups2
			// 
			ultraGridBand2.ColHeadersVisible = false;
			ultraGridBand2.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand2.Override.RowSpacingAfter = 0;
			ultraGridBand2.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups2.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
			this.m_ctrlGroups2.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups2.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlGroups2.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
			this.m_ctrlGroups2.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups2.DisplayLayout.Override.RowSpacingAfter = 0;
			this.m_ctrlGroups2.DisplayLayout.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups2.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
			this.m_ctrlGroups2.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlGroups2.DisplayMember = "";
			this.m_ctrlGroups2.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlGroups2.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.m_ctrlGroups2.Location = new System.Drawing.Point(8, 35);
			this.m_ctrlGroups2.Name = "m_ctrlGroups2";
			this.m_ctrlGroups2.Size = new System.Drawing.Size(108, 21);
			this.m_ctrlGroups2.TabIndex = 3;
			this.m_ctrlGroups2.ValueMember = "";
			// 
			// m_ctrlGroups4
			// 
			ultraGridBand3.ColHeadersVisible = false;
			ultraGridBand3.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand3.Override.RowSpacingAfter = 0;
			ultraGridBand3.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups4.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
			this.m_ctrlGroups4.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups4.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlGroups4.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
			this.m_ctrlGroups4.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups4.DisplayLayout.Override.RowSpacingAfter = 0;
			this.m_ctrlGroups4.DisplayLayout.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups4.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
			this.m_ctrlGroups4.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlGroups4.DisplayMember = "";
			this.m_ctrlGroups4.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlGroups4.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.m_ctrlGroups4.Location = new System.Drawing.Point(8, 89);
			this.m_ctrlGroups4.Name = "m_ctrlGroups4";
			this.m_ctrlGroups4.Size = new System.Drawing.Size(108, 21);
			this.m_ctrlGroups4.TabIndex = 9;
			this.m_ctrlGroups4.ValueMember = "";
			// 
			// m_ctrlGroups3
			// 
			ultraGridBand4.ColHeadersVisible = false;
			ultraGridBand4.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand4.Override.RowSpacingAfter = 0;
			ultraGridBand4.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups3.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
			this.m_ctrlGroups3.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups3.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlGroups3.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
			this.m_ctrlGroups3.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups3.DisplayLayout.Override.RowSpacingAfter = 0;
			this.m_ctrlGroups3.DisplayLayout.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups3.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
			this.m_ctrlGroups3.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlGroups3.DisplayMember = "";
			this.m_ctrlGroups3.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlGroups3.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.m_ctrlGroups3.Location = new System.Drawing.Point(8, 62);
			this.m_ctrlGroups3.Name = "m_ctrlGroups3";
			this.m_ctrlGroups3.Size = new System.Drawing.Size(108, 21);
			this.m_ctrlGroups3.TabIndex = 6;
			this.m_ctrlGroups3.ValueMember = "";
			// 
			// m_ctrlGroups7
			// 
			ultraGridBand5.ColHeadersVisible = false;
			ultraGridBand5.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand5.Override.RowSpacingAfter = 0;
			ultraGridBand5.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups7.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
			this.m_ctrlGroups7.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups7.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlGroups7.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
			this.m_ctrlGroups7.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups7.DisplayLayout.Override.RowSpacingAfter = 0;
			this.m_ctrlGroups7.DisplayLayout.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups7.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
			this.m_ctrlGroups7.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlGroups7.DisplayMember = "";
			this.m_ctrlGroups7.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlGroups7.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.m_ctrlGroups7.Location = new System.Drawing.Point(8, 170);
			this.m_ctrlGroups7.Name = "m_ctrlGroups7";
			this.m_ctrlGroups7.Size = new System.Drawing.Size(108, 21);
			this.m_ctrlGroups7.TabIndex = 18;
			this.m_ctrlGroups7.ValueMember = "";
			// 
			// m_ctrlGroups6
			// 
			ultraGridBand6.ColHeadersVisible = false;
			ultraGridBand6.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand6.Override.RowSpacingAfter = 0;
			ultraGridBand6.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups6.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
			this.m_ctrlGroups6.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups6.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlGroups6.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
			this.m_ctrlGroups6.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups6.DisplayLayout.Override.RowSpacingAfter = 0;
			this.m_ctrlGroups6.DisplayLayout.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups6.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
			this.m_ctrlGroups6.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlGroups6.DisplayMember = "";
			this.m_ctrlGroups6.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlGroups6.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.m_ctrlGroups6.Location = new System.Drawing.Point(8, 143);
			this.m_ctrlGroups6.Name = "m_ctrlGroups6";
			this.m_ctrlGroups6.Size = new System.Drawing.Size(108, 21);
			this.m_ctrlGroups6.TabIndex = 15;
			this.m_ctrlGroups6.ValueMember = "";
			// 
			// m_ctrlGroups5
			// 
			ultraGridBand7.ColHeadersVisible = false;
			ultraGridBand7.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand7.Override.RowSpacingAfter = 0;
			ultraGridBand7.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups5.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
			this.m_ctrlGroups5.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups5.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlGroups5.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
			this.m_ctrlGroups5.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlGroups5.DisplayLayout.Override.RowSpacingAfter = 0;
			this.m_ctrlGroups5.DisplayLayout.Override.RowSpacingBefore = 0;
			this.m_ctrlGroups5.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
			this.m_ctrlGroups5.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlGroups5.DisplayMember = "";
			this.m_ctrlGroups5.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlGroups5.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.m_ctrlGroups5.Location = new System.Drawing.Point(8, 116);
			this.m_ctrlGroups5.Name = "m_ctrlGroups5";
			this.m_ctrlGroups5.Size = new System.Drawing.Size(108, 21);
			this.m_ctrlGroups5.TabIndex = 12;
			this.m_ctrlGroups5.ValueMember = "";
			// 
			// m_ctrlName2
			// 
			this.m_ctrlName2.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlName2.Location = new System.Drawing.Point(164, 35);
			this.m_ctrlName2.Name = "m_ctrlName2";
			this.m_ctrlName2.Size = new System.Drawing.Size(196, 21);
			this.m_ctrlName2.TabIndex = 5;
			// 
			// m_ctrlName4
			// 
			this.m_ctrlName4.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlName4.Location = new System.Drawing.Point(164, 89);
			this.m_ctrlName4.Name = "m_ctrlName4";
			this.m_ctrlName4.Size = new System.Drawing.Size(196, 21);
			this.m_ctrlName4.TabIndex = 11;
			// 
			// m_ctrlName3
			// 
			this.m_ctrlName3.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlName3.Location = new System.Drawing.Point(164, 62);
			this.m_ctrlName3.Name = "m_ctrlName3";
			this.m_ctrlName3.Size = new System.Drawing.Size(196, 21);
			this.m_ctrlName3.TabIndex = 8;
			// 
			// m_ctrlName7
			// 
			this.m_ctrlName7.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlName7.Location = new System.Drawing.Point(164, 170);
			this.m_ctrlName7.Name = "m_ctrlName7";
			this.m_ctrlName7.Size = new System.Drawing.Size(196, 21);
			this.m_ctrlName7.TabIndex = 20;
			// 
			// m_ctrlName6
			// 
			this.m_ctrlName6.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlName6.Location = new System.Drawing.Point(164, 143);
			this.m_ctrlName6.Name = "m_ctrlName6";
			this.m_ctrlName6.Size = new System.Drawing.Size(196, 21);
			this.m_ctrlName6.TabIndex = 17;
			// 
			// m_ctrlName5
			// 
			this.m_ctrlName5.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2003;
			this.m_ctrlName5.Location = new System.Drawing.Point(164, 116);
			this.m_ctrlName5.Name = "m_ctrlName5";
			this.m_ctrlName5.Size = new System.Drawing.Size(196, 21);
			this.m_ctrlName5.TabIndex = 14;
			// 
			// m_ctrlColorButton1
			// 
			this.m_ctrlColorButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlColorButton1.Location = new System.Drawing.Point(124, 8);
			this.m_ctrlColorButton1.Name = "m_ctrlColorButton1";
			this.m_ctrlColorButton1.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlColorButton1.TabIndex = 1;
			this.m_ctrlColorButton1.Click += new System.EventHandler(this.OnClickColor);
			// 
			// m_ctrlColorButton2
			// 
			this.m_ctrlColorButton2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlColorButton2.Location = new System.Drawing.Point(124, 35);
			this.m_ctrlColorButton2.Name = "m_ctrlColorButton2";
			this.m_ctrlColorButton2.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlColorButton2.TabIndex = 4;
			this.m_ctrlColorButton2.Click += new System.EventHandler(this.OnClickColor);
			// 
			// m_ctrlColorButton4
			// 
			this.m_ctrlColorButton4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlColorButton4.Location = new System.Drawing.Point(124, 89);
			this.m_ctrlColorButton4.Name = "m_ctrlColorButton4";
			this.m_ctrlColorButton4.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlColorButton4.TabIndex = 10;
			this.m_ctrlColorButton4.Click += new System.EventHandler(this.OnClickColor);
			// 
			// m_ctrlColorButton3
			// 
			this.m_ctrlColorButton3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlColorButton3.Location = new System.Drawing.Point(124, 62);
			this.m_ctrlColorButton3.Name = "m_ctrlColorButton3";
			this.m_ctrlColorButton3.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlColorButton3.TabIndex = 7;
			this.m_ctrlColorButton3.Click += new System.EventHandler(this.OnClickColor);
			// 
			// m_ctrlColorButton7
			// 
			this.m_ctrlColorButton7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlColorButton7.Location = new System.Drawing.Point(124, 170);
			this.m_ctrlColorButton7.Name = "m_ctrlColorButton7";
			this.m_ctrlColorButton7.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlColorButton7.TabIndex = 19;
			this.m_ctrlColorButton7.Click += new System.EventHandler(this.OnClickColor);
			// 
			// m_ctrlColorButton6
			// 
			this.m_ctrlColorButton6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlColorButton6.Location = new System.Drawing.Point(124, 143);
			this.m_ctrlColorButton6.Name = "m_ctrlColorButton6";
			this.m_ctrlColorButton6.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlColorButton6.TabIndex = 16;
			this.m_ctrlColorButton6.Click += new System.EventHandler(this.OnClickColor);
			// 
			// m_ctrlColorButton5
			// 
			this.m_ctrlColorButton5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlColorButton5.Location = new System.Drawing.Point(124, 116);
			this.m_ctrlColorButton5.Name = "m_ctrlColorButton5";
			this.m_ctrlColorButton5.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlColorButton5.TabIndex = 13;
			this.m_ctrlColorButton5.Click += new System.EventHandler(this.OnClickColor);
			// 
			// m_ctrlGetColor
			// 
			this.m_ctrlGetColor.AnyColor = true;
			this.m_ctrlGetColor.FullOpen = true;
			// 
			// CFSetHighlighters
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(366, 231);
			this.Controls.Add(this.m_ctrlColorButton7);
			this.Controls.Add(this.m_ctrlColorButton6);
			this.Controls.Add(this.m_ctrlColorButton5);
			this.Controls.Add(this.m_ctrlColorButton4);
			this.Controls.Add(this.m_ctrlColorButton3);
			this.Controls.Add(this.m_ctrlColorButton2);
			this.Controls.Add(this.m_ctrlColorButton1);
			this.Controls.Add(this.m_ctrlName7);
			this.Controls.Add(this.m_ctrlName6);
			this.Controls.Add(this.m_ctrlName5);
			this.Controls.Add(this.m_ctrlName4);
			this.Controls.Add(this.m_ctrlName3);
			this.Controls.Add(this.m_ctrlName2);
			this.Controls.Add(this.m_ctrlGroups7);
			this.Controls.Add(this.m_ctrlGroups6);
			this.Controls.Add(this.m_ctrlGroups5);
			this.Controls.Add(this.m_ctrlGroups4);
			this.Controls.Add(this.m_ctrlGroups3);
			this.Controls.Add(this.m_ctrlGroups2);
			this.Controls.Add(this.m_ctrlGroups1);
			this.Controls.Add(this.m_ctrlName1);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFSetHighlighters";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Highlighters";
			this.Load += new System.EventHandler(this.OnLoad);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlGroups5)).EndInit();
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			//	Set all the property values
			for(int i = 0; i < MAX_HIGHLIGHTER_ROWS; i++)
				SetProps(i);
		
			//	Make it look as though the user canceled if no changes were made
			DialogResult = (m_dxModified.Count > 0) ? DialogResult.OK : DialogResult.Cancel;
			
			this.Close();
		}
	
		/// <summary>This method is called when the user clicks on one of the color buttons</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickColor(object sender, System.EventArgs e)
		{
			Button colorButton = (Button)sender;
			
			m_ctrlGetColor.Color = colorButton.BackColor;
			if(m_ctrlGetColor.ShowDialog() == DialogResult.OK)
				colorButton.BackColor = m_ctrlGetColor.Color;
		}

		#endregion Private Methods

		#region Properties
		
		/// <summary>The database containing the highlighter records</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}
		
		/// <summary>The collection of modified records</summary>
		public CDxMediaRecords Modified
		{
			get { return m_dxModified; }
		}
		
		#endregion Properties

	}// public class CFSetHighlighters : FTI.Trialmax.Forms.CFTmaxBaseForm

}// namespace FTI.Trialmax.Panes
