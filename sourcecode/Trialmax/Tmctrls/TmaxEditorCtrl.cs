using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class creates a grid-style control for viewing the terms in a TrialMax database filter</summary>
	public class CTmaxEditorCtrl : CTmaxBaseCtrl
	{
		#region Constants
		
		private int BOOLEAN_TRUE_INDEX			= 0;
		private int BOOLEAN_FALSE_INDEX			= 1;
				
		//	Infragistics Editor indexes
		private const int ULTRA_TEXT_EDITOR			= 0;
		private const int ULTRA_MEMO_EDITOR			= 1;
		private const int ULTRA_BOOLEAN_EDITOR		= 2;
		private const int ULTRA_DATE_EDITOR			= 3;
		private const int ULTRA_DECIMAL_EDITOR		= 4;
		private const int ULTRA_INTEGER_EDITOR		= 5;
		private const int ULTRA_DROP_LIST_EDITOR	= 6;
		private const int ULTRA_MULTI_LEVEL_EDITOR	= 7;
		private const int MAX_ULTRA_EDITORS			= 8;
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_CREATE_EDITORS_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_SET_TYPE_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_SET_VALUE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_SET_DROP_LIST_VALUES_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		
		private const int MINIMUM_MULTILEVEL_EDITOR_WIDTH  = 120;
		private const int MINIMUM_MULTILEVEL_EDITOR_HEIGHT = 140;
		
		#endregion Constants
		
		#region Private Members
		
		private UltraControlBase[] m_aUltraEditors = new UltraControlBase[MAX_ULTRA_EDITORS];
		
		/// <summary>Local member bound to Type property</summary>
		private TmaxEditorCtrlTypes m_eType = TmaxEditorCtrlTypes.Text;
		
		/// <summary>Custom editor used for multi-level data types</summary>
		private FTI.Trialmax.Controls.CTmaxMultiLevelEditorCtrl m_ctrlMultiLevelEditor;

		/// <summary>Local member to access the drop button for the custom MultiLevel editor</summary>
		private Infragistics.Win.UltraWinEditors.DropDownEditorButton m_multiLevelButton = null;

		/// <summary>Local member bound to MultiLevel property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxMultiLevel = null;
		
		/// <summary>Local member bound to MultiLevelSelection property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxMultiLevelSelection = null;
		
		/// <summary>Local member bound to Value property</summary>
		private string m_strValue = "";
		
		/// <summary>Local member bound to MaxTextLength property</summary>
		private int m_iMaxTextLength = 255;
		
		/// <summary>Local member bound to FalseText property</summary>
		private string m_strFalseText = "False";
		
		/// <summary>Local member bound to TrueText property</summary>
		private string m_strTrueText = "True";
		
		/// <summary>Local member bound to DropListValues property</summary>
		private ICollection m_IDropListValues = null;
		
		/// <summary>Local member bound to UserAdditions property</summary>
		private bool m_bUserAdditions = false;
		
		/// <summary>Local member bound to MemoAsText property</summary>
		private bool m_bMemoAsText = false;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxEditorCtrl() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();

			//	Set the default event source name
			m_tmaxEventSource.Name = "TrialMax Edit Control";
			
		}// public CTmaxEditorCtrl() : base()
		
		/// <summary>This method is called to set the data type</summary>
		/// <param name="e">The enumerated data type identifier</param>
		/// <returns>true if successsful</returns>
		public bool SetType(TmaxEditorCtrlTypes e)
		{
			int		iEditor = -1;
			bool	bSuccessful = false;
			bool	bSetFocus = false;
			
			try
			{
				if(this.ContainsFocus)
					bSetFocus = true;
					
				//	Should we block multi-line memo fields?
				if(e == TmaxEditorCtrlTypes.Memo)
				{
					if(m_bMemoAsText == true)
						e = TmaxEditorCtrlTypes.Text;
				}
				
				//	What is the index of the new editor?
				iEditor = GetEditorIndex(e);
				
				//	Save the new type
				m_eType = e;
				
				//	Set the default value before making the editor visible
				if(m_eType == TmaxEditorCtrlTypes.Boolean)
					SetValue("False");
				else
					SetValue("");
					
				//	Set the visibility
				for(int i = 0; i < MAX_ULTRA_EDITORS; i++)
				{
					try
					{
						if(m_aUltraEditors[i] != null)
							m_aUltraEditors[i].Visible = (i == iEditor);
						
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireDiagnostic(this, "SetType", Ex);
					}
						
				}// for(int i = 0; i < MAX_ULTRA_EDITORS; i++)
				
				if(bSetFocus == true)
				{
					try { m_aUltraEditors[iEditor].Focus(); }
					catch {};
				}
				
				bSuccessful = true;	
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetType", m_tmaxErrorBuilder.Message(ERROR_SET_TYPE_EX, e), Ex);
				bSuccessful = false;
			}
			
			//	Make sure the controls are properly sized
			RecalcLayout();
			
			return bSuccessful;
		
		}// public bool SetType(TmaxEditorCtrlTypes e)
		
		/// <summary>This method is called to to translate the case code type to an editor type</summary>
		/// <returns>The enumerated type identifier of the appropriate editor</returns>
		public TmaxEditorCtrlTypes TranslateType(CTmaxCaseCode tmaxCaseCode)
		{
			TmaxEditorCtrlTypes eType = TmaxEditorCtrlTypes.Text;
			
			switch(tmaxCaseCode.Type)
			{
				case TmaxCodeTypes.Memo:
						
					if(m_bMemoAsText == false)
						eType = TmaxEditorCtrlTypes.Memo;
					else
						eType = TmaxEditorCtrlTypes.Text;
					break;
							
				case TmaxCodeTypes.Boolean:
						
					eType = TmaxEditorCtrlTypes.Boolean;
					break;
							
				case TmaxCodeTypes.Integer:
						
					eType = TmaxEditorCtrlTypes.Integer;
					break;
							
				case TmaxCodeTypes.Decimal:
						
					eType = TmaxEditorCtrlTypes.Decimal;
					break;
							
				case TmaxCodeTypes.Date:
						
					eType = TmaxEditorCtrlTypes.Date;
					break;
							
				case TmaxCodeTypes.PickList:
						
					if(tmaxCaseCode.IsMultiLevel == true)
						eType = TmaxEditorCtrlTypes.MultiLevel;
					else
						eType = TmaxEditorCtrlTypes.DropList;
					break;
							
				case TmaxCodeTypes.Text:
				default:
						
					eType = TmaxEditorCtrlTypes.Text;
					break;
						
			}// switch(eCodeType)
			
			return eType;
		
		}// public TmaxEditorCtrlTypes TranslateType(TmaxCodeTypes eCodeType)
		
		/// <summary>This method is called to get the appropriate editor for the active case code</summary>
		/// <returns>The enumerated type identifier of the appropriate editor</returns>
		public bool SetType(CTmaxCaseCode tmaxCaseCode)
		{
			return SetType(TranslateType(tmaxCaseCode));
		
		}// public bool SetType(CTmaxCaseCode tmaxCaseCode)
		
		/// <summary>This method is called to set the current value</summary>
		/// <param name="strValue">The current value as a string</param>
		/// <returns>true if successsful</returns>
		public bool SetValue(string strValue)
		{
			UltraControlBase	editor = null;
			bool				bSuccessful = true;
			int					iIndex = -1;
			
			//	Update the local class member
			if(strValue != null)
				m_strValue = strValue;
			else
				m_strValue = "";
				
			try
			{
				//	Get the active editor
				if((editor = GetEditor()) == null) 
					return false;
				
				switch(m_eType)
				{
					case TmaxEditorCtrlTypes.DropList:
					
						if(((UltraComboEditor)editor).Items.Count > 0)
						{
							if(m_strValue.Length > 0)
								iIndex = ((UltraComboEditor)editor).FindStringExact(m_strValue);
								
							((UltraComboEditor)editor).SelectedIndex = iIndex;
						}
						break;
						
					case TmaxEditorCtrlTypes.Boolean:
					
						if(m_strValue.Length > 0)
						{
							if(CTmaxToolbox.StringToBool(m_strValue) == true)
								((UltraComboEditor)editor).SelectedIndex = BOOLEAN_TRUE_INDEX;
							else
								((UltraComboEditor)editor).SelectedIndex = BOOLEAN_FALSE_INDEX;
						}
						else
						{
							((UltraComboEditor)editor).SelectedIndex = -1;
						}
						break;
						
					default:
					
						editor.Text = m_strValue;
						break;
						
				}// switch(m_eType)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetValue", m_tmaxErrorBuilder.Message(ERROR_SET_VALUE_EX, m_strValue), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
		
		}// public bool SetValue(string strValue)
		
		/// <summary>This method is called to get the current value as text</summary>
		/// <returns>The current value as a string</returns>
		public string GetValue()
		{
			UltraControlBase editor = null;
			
			//	Clear the local class member
			m_strValue = "";
				
			try
			{
				//	Get the active editor
				if((editor = GetEditor()) != null)
				{
					m_strValue = editor.Text;
					
				}// if((editor = GetEditor()) != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetValue", Ex);
			}
			
			return m_strValue;
		
		}// public string GetValue()
		
		/// <summary>This method is called to set the values displayed by the drop list editor</summary>
		/// <param name="IValues">The collection of values</param>
		/// <returns>true if successsful</returns>
		public bool SetDropListValues(ICollection IValues)
		{
			UltraComboEditor	editor = null;
			bool				bSuccessful = false;
			
			try
			{
				m_IDropListValues = IValues;

//				if(IValues != null)
//					m_tmaxEventSource.FireDiagnostic(this, "SetDropListValues", IValues.Count.ToString() + " drop list values");
//				else
//					m_tmaxEventSource.FireDiagnostic(this, "SetDropListValues", "NULL drop list values");
				
				//	Get the drop list editor
				if(m_aUltraEditors != null)
					editor = (UltraComboEditor)(m_aUltraEditors[ULTRA_DROP_LIST_EDITOR]);
				
				if((editor != null) && (editor.Items != null))
				{
					//	Clear the existing items
					editor.Items.Clear();
					
					if((m_IDropListValues != null) && (m_IDropListValues.Count > 0))
					{
						foreach(object O in m_IDropListValues)
							editor.Items.Add(O);
					}
				
					bSuccessful = true;	
				
				}// if(editor.Items != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDropListValues", m_tmaxErrorBuilder.Message(ERROR_SET_DROP_LIST_VALUES_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
		
		}// public bool SetDropListValues(ICollection IValues)
		
		/// <summary>This method is called to set the option to allow the user to add values to a drop list collection</summary>
		/// <param name="bAllowed">True to allow user additions</param>
		/// <returns>true if successsful</returns>
		public bool SetUserAdditions(bool bAllowed)
		{
			UltraComboEditor	editor = null;
			bool				bSuccessful = false;
			
			try
			{
				m_bUserAdditions = bAllowed;
				
				//	Get the drop list editor
				if(m_aUltraEditors != null)
					editor = (UltraComboEditor)(m_aUltraEditors[ULTRA_DROP_LIST_EDITOR]);
				
				if((editor != null) && (editor.Items != null))
				{
					if(m_bUserAdditions)
						editor.DropDownStyle = DropDownStyle.DropDown;
					else
						editor.DropDownStyle = DropDownStyle.DropDownList;
					
					bSuccessful = true;	
				
				}
			
			}
			catch
			{
			}
			
			return bSuccessful;
		
		}// public bool SetUserAdditions(bool bAllowed)
		
		/// <summary>Called to set the user selection for multi-level pick list data types</summary>
		/// <param name="tmaxSelection">The current selection</param>
		/// <returns>true if successsful</returns>
		public bool SetMultiLevelSelection(CTmaxPickItem tmaxSelection)
		{
			if((m_tmaxMultiLevelSelection = tmaxSelection) != null)
				return SetValue(m_tmaxMultiLevelSelection.Name);
			else
				return SetValue("");
		
		}// public bool SetMultiLevelSelection(CTmaxPickItem tmaxSelection)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnGotFocus(EventArgs e)
		{
			UltraControlBase editor = null;
			
			//	Pass focus to the active editor
			if((editor = GetEditor()) != null)
				editor.Focus();
			else
				base.OnGotFocus(e);
		
		}// protected override void OnGotFocus(EventArgs e)

		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Create the type specific editors
			CreateEditors();
			
			SetType(m_eType);
			
			SetValue(m_strValue);
			
			//	Populate the drop list editor
			if(this.DropListValues != null)
				SetDropListValues(this.DropListValues);
			
			//	Perform the base class processing first
			base.OnLoad(e);
			
		}// protected override void OnLoad(System.EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Do the base class processing first
			base.SetErrorStrings();
			
			//	Add placeholders for the reserved strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the editor controls.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the data type: type = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the current value: value = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to populate the drop down list editor.");
			
		}// protected void SetErrorStrings()

		/// <summary>Required by form designer</summary>
		override protected void InitializeComponent()
		{
			this.m_ctrlMultiLevelEditor = new FTI.Trialmax.Controls.CTmaxMultiLevelEditorCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlMultiLevelEditor
			// 
			this.m_ctrlMultiLevelEditor.MultiLevel = null;
			this.m_ctrlMultiLevelEditor.IPropGridCtrl = null;
			this.m_ctrlMultiLevelEditor.Location = new System.Drawing.Point(80, 0);
			this.m_ctrlMultiLevelEditor.Name = "m_ctrlMultiLevelEditor";
			this.m_ctrlMultiLevelEditor.PaneId = 0;
			this.m_ctrlMultiLevelEditor.Size = new System.Drawing.Size(56, 24);
			this.m_ctrlMultiLevelEditor.TabIndex = 0;
			this.m_ctrlMultiLevelEditor.Value = null;
			// 
			// CTmaxEditorCtrl
			// 
			this.Controls.Add(this.m_ctrlMultiLevelEditor);
			this.Name = "CTmaxEditorCtrl";
			this.Size = new System.Drawing.Size(192, 24);
			this.ResumeLayout(false);

		}// protected void InitializeComponent()
		
		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing == true)
			{
				if(m_aUltraEditors != null)
				{
					foreach(UltraControlBase O in m_aUltraEditors)
					{
						try { O.Dispose(); }
						catch {}
					}
					
					m_aUltraEditors = null;
				}
			}
			base.Dispose(disposing);
		
		}// protected override void Dispose(bool disposing)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Called to create the individual editors used by this control</summary>
		/// <returns>true if successful</returns>
		private bool CreateEditors()
		{
			UltraNumericEditor		numericEditor = null;
			UltraTextEditor			textEditor = null;
			UltraDateTimeEditor		dateEditor = null;
			UltraComboEditor		boolEditor = null;
			UltraComboEditor		dropListEditor = null;
			ValueListItem			vlItem = null;
			bool					bSuccessful = true;
					
			try
			{
				textEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
				textEditor.AcceptsReturn = false;
				textEditor.Multiline = false;
				textEditor.TabStop = false;
				textEditor.Name = TmaxEditorCtrlTypes.Text.ToString() + "Editor";;
				textEditor.ShowOverflowIndicator = true;
				textEditor.Dock = System.Windows.Forms.DockStyle.Top;
				textEditor.MaxLength = m_iMaxTextLength;
				this.Controls.Add(textEditor);
				m_aUltraEditors[ULTRA_TEXT_EDITOR] = textEditor;

				textEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
				textEditor.AcceptsReturn = true;
				textEditor.Multiline = true;
				textEditor.TabStop = false;
				textEditor.Name = TmaxEditorCtrlTypes.Memo.ToString() + "Editor";;
				textEditor.ShowOverflowIndicator = true;
				textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
				this.Controls.Add(textEditor);
				m_aUltraEditors[ULTRA_MEMO_EDITOR] = textEditor;

				dateEditor = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
				dateEditor.Name = TmaxEditorCtrlTypes.Date.ToString() + "Editor";
				dateEditor.Dock = System.Windows.Forms.DockStyle.Top;
				this.Controls.Add(dateEditor);
				m_aUltraEditors[ULTRA_DATE_EDITOR] = dateEditor;

				boolEditor = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
				boolEditor.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
				vlItem = new Infragistics.Win.ValueListItem();
				vlItem.DataValue = m_strTrueText;
				boolEditor.Items.Add(vlItem);
				vlItem = new Infragistics.Win.ValueListItem();
				vlItem.DataValue = m_strFalseText;
				boolEditor.Items.Add(vlItem);
				boolEditor.SelectedIndex = BOOLEAN_FALSE_INDEX;
				boolEditor.Name = TmaxEditorCtrlTypes.Boolean.ToString() + "Editor";
				boolEditor.Dock = System.Windows.Forms.DockStyle.Top;
				this.Controls.Add(boolEditor);
				m_aUltraEditors[ULTRA_BOOLEAN_EDITOR] = boolEditor;

				numericEditor = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
				numericEditor.Name = TmaxEditorCtrlTypes.Integer.ToString() + "Editor";
				numericEditor.TabStop = false;
				numericEditor.NumericType = NumericType.Integer;
				numericEditor.Dock = System.Windows.Forms.DockStyle.Top;
				numericEditor.MaskInput = "-nnnnnnnnn";
				numericEditor.Nullable = true;
				numericEditor.NullText = "";
				numericEditor.PromptChar = '_';
				numericEditor.DisplayStyle = EmbeddableElementDisplayStyle.Standard;
				numericEditor.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
				this.Controls.Add(numericEditor);
				m_aUltraEditors[ULTRA_INTEGER_EDITOR] = numericEditor;
				
				numericEditor = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
				numericEditor.Name = TmaxEditorCtrlTypes.Decimal.ToString() + "Editor";
				numericEditor.TabStop = false;
				numericEditor.NumericType = NumericType.Double;
				numericEditor.Nullable = true;
				numericEditor.NullText = "";
				numericEditor.PromptChar = '_';
				numericEditor.DisplayStyle = EmbeddableElementDisplayStyle.Standard;
				numericEditor.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
				numericEditor.MaskInput = "-nnnnnnnnnnnn.nnnn";
				numericEditor.Dock = System.Windows.Forms.DockStyle.Top;
				this.Controls.Add(numericEditor);
				m_aUltraEditors[ULTRA_DECIMAL_EDITOR] = numericEditor;
			
				dropListEditor = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
				dropListEditor.Name = TmaxEditorCtrlTypes.DropList.ToString() + "Editor";
				dropListEditor.Dock = System.Windows.Forms.DockStyle.Top;
				if(m_bUserAdditions)
					dropListEditor.DropDownStyle = DropDownStyle.DropDown;
				else
					dropListEditor.DropDownStyle = DropDownStyle.DropDownList;
				this.Controls.Add(dropListEditor);
				m_aUltraEditors[ULTRA_DROP_LIST_EDITOR] = dropListEditor;

				textEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
				textEditor.AcceptsReturn = false;
				textEditor.Multiline = false;
				textEditor.TabStop = false;
				textEditor.Name = TmaxEditorCtrlTypes.MultiLevel.ToString() + "Editor";;
				textEditor.ShowOverflowIndicator = false;
				textEditor.Dock = System.Windows.Forms.DockStyle.Top;
				textEditor.MaxLength = 255;
				//textEditor.ReadOnly = true;
				textEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnMultiLevelKey);
				textEditor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnMultiLevelKey);
				textEditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnMultiLevelKeyPress);
				//				textEditor.Appearance.BackColor = System.Drawing.SystemColors.Window;
//				textEditor.Appearance.BackColorDisabled = textEditor.Appearance.BackColor;
				
				//	Create the drop button to trigger the custom multi-level editor
				m_multiLevelButton = new Infragistics.Win.UltraWinEditors.DropDownEditorButton();
				m_multiLevelButton.Control = this.m_ctrlMultiLevelEditor;
				m_multiLevelButton.BeforeDropDown += new Infragistics.Win.UltraWinEditors.BeforeEditorButtonDropDownEventHandler(this.OnBeforeDropMultiLevel);
				textEditor.ButtonsRight.Add(m_multiLevelButton); //add the button to the editor control
	
				this.Controls.Add(textEditor);
				m_aUltraEditors[ULTRA_MULTI_LEVEL_EDITOR] = textEditor;
				
				//	Trap the event fired by the multi-level editor when the user finishes
				m_ctrlMultiLevelEditor.Finished += new System.EventHandler(this.OnMultiLevelFinished);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateEditors", m_tmaxErrorBuilder.Message(ERROR_CREATE_EDITORS_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// private bool CreateEditors()
		
		/// <summary>Called to get the index of the editor for the specified control type</summary>
		/// <param name="eType">The enumerated type identifier</param>
		/// <returns>The index of the associated editor control</returns>
		private int GetEditorIndex(TmaxEditorCtrlTypes eType)
		{
			int iIndex = -1;
			
			try
			{
				iIndex = (int)eType;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetEditorIndex", Ex);
			}
			
			return iIndex;
			
		}// private int GetEditorIndex(TmaxEditorCtrlTypes eType)
		
		/// <summary>Called to get the active editor</summary>
		/// <returns>The active editor</returns>
		private UltraControlBase GetEditor()
		{
			int iIndex = -1;
			
			try
			{
				if((iIndex = GetEditorIndex(m_eType)) >= 0)
				{
					return m_aUltraEditors[iIndex];
				}
				else
				{
					m_tmaxEventSource.FireDiagnostic(this, "GetEditor", "Invalid Editor Type: " + m_eType.ToString());
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetEditor", Ex);
			}
			
			return null;
			
		}// private UltraControlBase GetEditor()
		
		/// <summary>Set the maximum number of characters for text fields</summary>
		/// <param name="iMaxLength">The maximum number of characters</param>
		/// <returns>true if successful</returns>
		private bool SetMaxTextLength(int iMaxLength)
		{
			bool bSuccessful = true;
			
			//	Store the maximum length
			if(iMaxLength > 0)
				m_iMaxTextLength = iMaxLength;
			else
				return false;
				
			try
			{
				//	Update the editor
				if(m_aUltraEditors[ULTRA_TEXT_EDITOR] != null)
					((UltraTextEditor)m_aUltraEditors[ULTRA_TEXT_EDITOR]).MaxLength = m_iMaxTextLength;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetMaxTextLength", Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// private bool SetMaxTextLength(int iMaxLength)
		
		/// <summary>Set the text displayed for the boolean value</summary>
		/// <param name="strText">The text to be displayed</param>
		/// <param name="bFalseText">The text is for boolean == false</param>
		/// <returns>true if successful</returns>
		private bool SetBooleanText(string strText, bool bFalseText)
		{
			int					iIndex = -1;
			bool				bSuccessful = true;
			UltraComboEditor	boolEditor = null;
			
			if((strText == null) || (strText.Length == 0)) return false;
			
			//	Store the new text
			if(bFalseText == true)
			{
				m_strFalseText = strText;
				iIndex = BOOLEAN_FALSE_INDEX;
			}
			else
			{
				m_strTrueText = strText;
				iIndex = BOOLEAN_TRUE_INDEX;
			}
				
			try
			{
				//	Update the editor
				if((boolEditor = (UltraComboEditor)(m_aUltraEditors[ULTRA_BOOLEAN_EDITOR])) != null)
				{
					if((boolEditor.Items != null) && (boolEditor.Items.Count > iIndex))
					{
						boolEditor.Items[iIndex].DisplayText = strText;
					}	
				
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetMaxTextLength", Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// private bool SetMaxTextLength(int iMaxLength)
		
		/// <summary>This method handles events fired by the grid before it drops the multi-level child control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnBeforeDropMultiLevel(object sender, Infragistics.Win.UltraWinEditors.BeforeEditorButtonDropDownEventArgs e)
		{
			int				iWidth = 0;
			int				iHeight = 0;

			try
			{
				//	We have to have a valid MultiLevel
				if(this.MultiLevel != null)
				{
					//	What size should the multi-level editor be?
					if((iWidth = this.Width) < MINIMUM_MULTILEVEL_EDITOR_WIDTH)
						iWidth = MINIMUM_MULTILEVEL_EDITOR_WIDTH;
					if((iHeight = this.Height) < MINIMUM_MULTILEVEL_EDITOR_HEIGHT)
						iHeight = MINIMUM_MULTILEVEL_EDITOR_HEIGHT;
								
					//	Initialize the control
					m_ctrlMultiLevelEditor.Width = iWidth;
					m_ctrlMultiLevelEditor.Height = iHeight;
					m_ctrlMultiLevelEditor.UserAdditions = this.UserAdditions;
					m_ctrlMultiLevelEditor.MultiLevel = this.MultiLevel;
					m_ctrlMultiLevelEditor.Value = this.MultiLevelSelection;
					m_ctrlMultiLevelEditor.OnBeforeDropDown();
				
				}
				else
				{
					e.Cancel = true;
					m_tmaxEventSource.FireDiagnostic(this, "OnBeforeDropMultiLevel", "No multi-level pick list available");
				}

			}
			catch(System.Exception Ex)
			{
				e.Cancel = true;
				m_tmaxEventSource.FireDiagnostic(this, "OnBeforeDropMultiLevel", Ex);
			}

		}// private void OnBeforeDropMultiLevel(object sender, Infragistics.Win.UltraWinEditors.BeforeEditorButtonDropDownEventArgs e)

		/// <summary>This method handles events fired by the multilevel child control when the user finishes the operation</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnMultiLevelFinished(object sender, System.EventArgs e)
		{
			try
			{
				//	Did the user assign a new value?
				if(m_ctrlMultiLevelEditor.Cancelled == false)
				{
					this.MultiLevelSelection = m_ctrlMultiLevelEditor.Value;
					if(this.MultiLevelSelection != null)
					{
						SetValue(this.MultiLevelSelection.Name);
					}
					else
					{
						SetValue("");
					}
					
				}
				
				//	Close the editor
				this.Focus();
				m_multiLevelButton.CloseUp();				
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnMultiLevelFinished", Ex);
			}

		}// private void OnMultiLevelFinished(object sender, System.EventArgs e)

		/// <summary>Called when the user presses a key in the multilevel editor's text box</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnMultiLevelKey(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Tab:
				case Keys.Enter:
				case Keys.Left:
				case Keys.Right:
					break;
					
				default:
					e.Handled = true;//	Eat the keystroke
					break;
			}
			
		}// private void OnMultiLevelKey(object sender, System.Windows.Forms.KeyEventArgs e)

		/// <summary>Called when the user presses a key in the multilevel editor's text box</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnMultiLevelKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			//	Eat the event (it's only called for character keys)
			e.Handled = true;
		}

		#endregion Private Methods

		#region Properties

		/// <summary>The enumerated data type identifier</summary>
		public TmaxEditorCtrlTypes Type
		{
			get { return m_eType; }
			set { SetType(value); }
		}
		
		/// <summary>The current value as text</summary>
		public string Value
		{
			get { return GetValue(); }
			set { SetValue(value); }
		}
		
		/// <summary>The text displayed for boolean False</summary>
		public string FalseText
		{
			get { return m_strFalseText; }
			set { SetBooleanText(value, true); }
		}
		
		/// <summary>The text displayed for boolean True</summary>
		public string TrueText
		{
			get { return m_strTrueText; }
			set { SetBooleanText(value, false); }
		}
		
		/// <summary>The maximum length allowed for Text type</summary>
		public int MaxTextLength
		{
			get { return m_iMaxTextLength; }
			set { SetMaxTextLength(value); }
		}
		
		/// <summary>The collection of values used to populate the drop list editor</summary>
		public ICollection DropListValues
		{
			get { return m_IDropListValues; }
			set { SetDropListValues(value); }
		}
		
		/// <summary>True if the user is allowed to add values to the value list</summary>
		public bool UserAdditions
		{
			get { return m_bUserAdditions; }
			set { SetUserAdditions(value); }
		}
		
		/// <summary>True to treat memo types as single line text</summary>
		public bool MemoAsText
		{
			get { return m_bMemoAsText; }
			set { m_bMemoAsText = value; }
		}
		
		/// <summary>Top level pick list from which the multi-level selection must be made</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem MultiLevel
		{
			get { return m_tmaxMultiLevel; }
			set { m_tmaxMultiLevel = value; }
		}
		
		/// <summary>The user's selection in the active multilevel</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem MultiLevelSelection
		{
			get { return m_tmaxMultiLevelSelection; }
			set { SetMultiLevelSelection(value); }
		}
		
		#endregion Properties
		
	}// class CTmaxEditorCtrl
	
	public enum TmaxEditorCtrlTypes
	{
		Text = 0,
		Memo,
		Boolean,
		Date,
		Decimal,
		Integer,
		DropList,
		MultiLevel,
	}
	
}// namespace FTI.Trialmax.Controls

