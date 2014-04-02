using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>
	/// Summary description for CFTestEditCtrl.
	/// </summary>
	public class CFTestEditCtrl : System.Windows.Forms.Form
	{
		private FTI.Trialmax.Controls.CTmaxEditorCtrl m_ctrlTmaxEdit;
		private System.Windows.Forms.ListBox m_ctrlTypes;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor ultraNumericEditor1;
		private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor1;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditor1;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComboEditor1;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor m_ctrlTextWithDrop;
		private FTI.Trialmax.Controls.CTmaxMultiLevelEditorCtrl m_ctrlPickListEditor;
		private CTmaxPickItem m_tmaxMultiLevel = null;
		private CTmaxPickItem m_tmaxStringList = null;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CFTestEditCtrl()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			CreateMultiLevel();
			m_ctrlTmaxEdit.MultiLevel = m_tmaxMultiLevel;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
			this.m_ctrlTmaxEdit = new FTI.Trialmax.Controls.CTmaxEditorCtrl();
			this.m_ctrlTypes = new System.Windows.Forms.ListBox();
			this.ultraNumericEditor1 = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
			this.ultraDateTimeEditor1 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
			this.ultraTextEditor1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.ultraComboEditor1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
			this.m_ctrlTextWithDrop = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.m_ctrlPickListEditor = new FTI.Trialmax.Controls.CTmaxMultiLevelEditorCtrl();
			((System.ComponentModel.ISupportInitialize)(this.ultraNumericEditor1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraComboEditor1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTextWithDrop)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlTmaxEdit
			// 
			this.m_ctrlTmaxEdit.BackColor = System.Drawing.Color.Blue;
			this.m_ctrlTmaxEdit.DropListValues = null;
			this.m_ctrlTmaxEdit.FalseText = "False";
			this.m_ctrlTmaxEdit.Location = new System.Drawing.Point(20, 124);
			this.m_ctrlTmaxEdit.MaxTextLength = 255;
			this.m_ctrlTmaxEdit.MemoAsText = false;
			this.m_ctrlTmaxEdit.Name = "m_ctrlTmaxEdit";
			this.m_ctrlTmaxEdit.PaneId = 0;
			this.m_ctrlTmaxEdit.Size = new System.Drawing.Size(248, 84);
			this.m_ctrlTmaxEdit.TabIndex = 0;
			this.m_ctrlTmaxEdit.TrueText = "True";
			this.m_ctrlTmaxEdit.Type = FTI.Trialmax.Controls.TmaxEditorCtrlTypes.Text;
			this.m_ctrlTmaxEdit.UserAdditions = false;
			this.m_ctrlTmaxEdit.Value = "";
			// 
			// m_ctrlTypes
			// 
			this.m_ctrlTypes.Items.AddRange(new object[] {
															 "Text",
															 "Memo",
															 "Boolean",
															 "Date",
															 "Decimal",
															 "Integer",
															 "DropList",
															 "PickList"});
			this.m_ctrlTypes.Location = new System.Drawing.Point(16, 24);
			this.m_ctrlTypes.Name = "m_ctrlTypes";
			this.m_ctrlTypes.Size = new System.Drawing.Size(252, 95);
			this.m_ctrlTypes.TabIndex = 1;
			this.m_ctrlTypes.SelectedIndexChanged += new System.EventHandler(this.OnTypeChanged);
			// 
			// ultraNumericEditor1
			// 
			this.ultraNumericEditor1.Location = new System.Drawing.Point(12, 224);
			this.ultraNumericEditor1.MaskInput = "{LOC}-nnnnnnnnnn.nnnnnn";
			this.ultraNumericEditor1.Name = "ultraNumericEditor1";
			this.ultraNumericEditor1.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
			this.ultraNumericEditor1.Size = new System.Drawing.Size(100, 21);
			this.ultraNumericEditor1.TabIndex = 2;
			// 
			// ultraDateTimeEditor1
			// 
			this.ultraDateTimeEditor1.Dock = System.Windows.Forms.DockStyle.Top;
			this.ultraDateTimeEditor1.Location = new System.Drawing.Point(0, 0);
			this.ultraDateTimeEditor1.Name = "ultraDateTimeEditor1";
			this.ultraDateTimeEditor1.Size = new System.Drawing.Size(324, 21);
			this.ultraDateTimeEditor1.TabIndex = 3;
			// 
			// ultraTextEditor1
			// 
			this.ultraTextEditor1.AcceptsReturn = true;
			this.ultraTextEditor1.AllowDrop = true;
			this.ultraTextEditor1.Location = new System.Drawing.Point(12, 252);
			this.ultraTextEditor1.Multiline = true;
			this.ultraTextEditor1.Name = "ultraTextEditor1";
			this.ultraTextEditor1.ShowOverflowIndicator = true;
			this.ultraTextEditor1.Size = new System.Drawing.Size(100, 21);
			this.ultraTextEditor1.TabIndex = 4;
			this.ultraTextEditor1.Text = "ultraTextEditor1";
			// 
			// ultraComboEditor1
			// 
			this.ultraComboEditor1.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
			valueListItem1.DataValue = "True";
			valueListItem2.DataValue = "False";
			this.ultraComboEditor1.Items.Add(valueListItem1);
			this.ultraComboEditor1.Items.Add(valueListItem2);
			this.ultraComboEditor1.Location = new System.Drawing.Point(124, 224);
			this.ultraComboEditor1.Name = "ultraComboEditor1";
			this.ultraComboEditor1.TabIndex = 5;
			// 
			// m_ctrlTextWithDrop
			// 
			this.m_ctrlTextWithDrop.AcceptsReturn = true;
			this.m_ctrlTextWithDrop.AllowDrop = true;
			this.m_ctrlTextWithDrop.Location = new System.Drawing.Point(124, 256);
			this.m_ctrlTextWithDrop.Multiline = true;
			this.m_ctrlTextWithDrop.Name = "m_ctrlTextWithDrop";
			this.m_ctrlTextWithDrop.ShowOverflowIndicator = true;
			this.m_ctrlTextWithDrop.Size = new System.Drawing.Size(128, 21);
			this.m_ctrlTextWithDrop.TabIndex = 6;
			this.m_ctrlTextWithDrop.Text = "ultraTextEditor2";
			// 
			// m_ctrlPickListEditor
			// 
			this.m_ctrlPickListEditor.Location = new System.Drawing.Point(24, 288);
			this.m_ctrlPickListEditor.Name = "m_ctrlPickListEditor";
			this.m_ctrlPickListEditor.PaneId = 0;
			this.m_ctrlPickListEditor.Size = new System.Drawing.Size(224, 80);
			this.m_ctrlPickListEditor.TabIndex = 7;
			// 
			// CFTestEditCtrl
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(324, 385);
			this.Controls.Add(this.m_ctrlPickListEditor);
			this.Controls.Add(this.m_ctrlTextWithDrop);
			this.Controls.Add(this.ultraComboEditor1);
			this.Controls.Add(this.ultraTextEditor1);
			this.Controls.Add(this.ultraDateTimeEditor1);
			this.Controls.Add(this.ultraNumericEditor1);
			this.Controls.Add(this.m_ctrlTypes);
			this.Controls.Add(this.m_ctrlTmaxEdit);
			this.Name = "CFTestEditCtrl";
			this.Text = "CFTestEditCtrl";
			((System.ComponentModel.ISupportInitialize)(this.ultraNumericEditor1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraComboEditor1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTextWithDrop)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			m_ctrlTypes.SelectedIndex = 0;
			base.OnLoad (e);

			//Set up the "Custom Control"
			Infragistics.Win.UltraWinEditors.DropDownEditorButton b = new Infragistics.Win.UltraWinEditors.DropDownEditorButton(); //Dropdown Editor Button
			b.Control = this.m_ctrlPickListEditor;
			this.m_ctrlTextWithDrop.ButtonsRight.Add(b); //add the button to the editor control
			
		}

		private void OnTypeChanged(object sender, System.EventArgs e)
		{
			if(m_ctrlTypes.SelectedIndex >= 0)
				m_ctrlTmaxEdit.Type = (TmaxEditorCtrlTypes)(m_ctrlTypes.SelectedIndex);
		}
		
		private void CreateMultiLevel()
		{
			CTmaxPickItem tmaxPickItem = null;
			
			m_tmaxMultiLevel = new CTmaxPickItem();
			m_tmaxMultiLevel.ParentId = 0;
			m_tmaxMultiLevel.UniqueId = 1;
			m_tmaxMultiLevel.Type = TmaxPickItemTypes.MultiLevel;
			m_tmaxMultiLevel.Name = "ML1";
			
			m_tmaxStringList = new CTmaxPickItem();
			m_tmaxStringList.ParentId = 1;
			m_tmaxStringList.UniqueId = 11;
			m_tmaxStringList.Type = TmaxPickItemTypes.StringList;
			m_tmaxStringList.Name = "ML1-SL1";
			m_tmaxStringList.Parent = m_tmaxMultiLevel;
			m_tmaxMultiLevel.Children.Add(m_tmaxStringList);
			
			tmaxPickItem = new CTmaxPickItem();
			tmaxPickItem.ParentId = 11;
			tmaxPickItem.UniqueId = 111;
			tmaxPickItem.Type = TmaxPickItemTypes.Value;
			tmaxPickItem.Name = "ML1-SL1-V1";
			tmaxPickItem.Parent = m_tmaxStringList;
			m_tmaxStringList.Children.Add(tmaxPickItem);
			
			tmaxPickItem = new CTmaxPickItem();
			tmaxPickItem.ParentId = 11;
			tmaxPickItem.UniqueId = 112;
			tmaxPickItem.Type = TmaxPickItemTypes.Value;
			tmaxPickItem.Name = "ML1-SL1-V2";
			tmaxPickItem.Parent = m_tmaxStringList;
			m_tmaxStringList.Children.Add(tmaxPickItem);
			
			
			m_tmaxStringList = new CTmaxPickItem();
			m_tmaxStringList.ParentId = 1;
			m_tmaxStringList.UniqueId = 12;
			m_tmaxStringList.Type = TmaxPickItemTypes.StringList;
			m_tmaxStringList.Name = "ML1-SL2";
			m_tmaxStringList.Parent = m_tmaxMultiLevel;
			m_tmaxMultiLevel.Children.Add(m_tmaxStringList);
			
			tmaxPickItem = new CTmaxPickItem();
			tmaxPickItem.ParentId = 12;
			tmaxPickItem.UniqueId = 121;
			tmaxPickItem.Type = TmaxPickItemTypes.Value;
			tmaxPickItem.Name = "ML1-SL2-V1";
			tmaxPickItem.Parent = m_tmaxStringList;
			m_tmaxStringList.Children.Add(tmaxPickItem);
			
			tmaxPickItem = new CTmaxPickItem();
			tmaxPickItem.ParentId = 12;
			tmaxPickItem.UniqueId = 122;
			tmaxPickItem.Type = TmaxPickItemTypes.Value;
			tmaxPickItem.Name = "ML1-SL2-V2";
			tmaxPickItem.Parent = m_tmaxStringList;
			m_tmaxStringList.Children.Add(tmaxPickItem);
			
			
		}
	}
}
