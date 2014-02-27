using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;

namespace FTI.Trialmax.Controls
{
	/// <summary>This control implements a custom property grid control</summary>
	public class CTmaxPropGridCtrl : CTmaxBaseCtrl
	{
		#region Constants
		
		/// <summary>Column names</summary>
		private const string CATEGORIES_COLUMN_ID		= "Id";
		private const string CATEGORIES_COLUMN_NAME		= "Name";
		private const string CATEGORIES_COLUMN_TAG		= "Tag";
		
		private const string PROPERTIES_COLUMN_CATEGORY	= "Category";
		private const string PROPERTIES_COLUMN_ID		= "ID";
		private const string PROPERTIES_COLUMN_NAME		= "Name";
		private const string PROPERTIES_COLUMN_VALUE	= "Value";
		private const string PROPERTIES_COLUMN_TAG		= "Tag";
		private const string PROPERTIES_COLUMN_INTERFACE= "IProperty";
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_CREATE_CATEGORIES_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_CREATE_PROPERTIES_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_CREATE_DATA_SOURCE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_INITIALIZE_EX					= ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_ON_INITIALIZE_LAYOUT_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_CATEGORY_NOT_FOUND				= ERROR_TMAX_BASE_CONTROL_MAX + 6;
		private const int ERROR_ADD_EX							= ERROR_TMAX_BASE_CONTROL_MAX + 7;
		private const int ERROR_FIRE_BEFORE_UPDATE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 8;
		private const int ERROR_FIRE_AFTER_UPDATE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 9;
		private const int ERROR_SET_COLUMNS_EX					= ERROR_TMAX_BASE_CONTROL_MAX + 10;
		private const int ERROR_GET_SELECTED_EX					= ERROR_TMAX_BASE_CONTROL_MAX + 11;
		private const int ERROR_GET_ARGS_EX						= ERROR_TMAX_BASE_CONTROL_MAX + 12;
		private const int ERROR_GET_PROPERTY_ROW_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 13;
		private const int ERROR_DELETE_DATA_ROW_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 14;
		private const int ERROR_DELETE_GRID_ROW_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 15;
		private const int ERROR_CREATE_EDITORS_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 16;
		private const int ERROR_SORT_EX							= ERROR_TMAX_BASE_CONTROL_MAX + 17;
		private const int ERROR_FILL_EDITOR_EX					= ERROR_TMAX_BASE_CONTROL_MAX + 18;
		private const int ERROR_SET_SELECTION_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 19;
		private const int ERROR_FIRE_GET_DROP_LIST_VALUES_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 20;
		private const int ERROR_GET_EDITOR_ROW_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 21;
		
		private const int PROP_EDITOR_BOOLEAN			= 0;
		private const int PROP_EDITOR_DATE				= 1;
		private const int PROP_EDITOR_INTEGER			= 2;
		private const int PROP_EDITOR_DOUBLE			= 3;
		private const int PROP_EDITOR_TEXT				= 4;
		private const int PROP_EDITOR_TEXT_WITH_BUTTON	= 5;
		private const int PROP_EDITOR_DROP_LIST			= 6;
		private const int PROP_EDITOR_COMBO_BOX			= 7;
		private const int PROP_EDITOR_MULTI_LEVEL		= 8;
		private const int MAX_PROP_EDITORS				= 9;
		
		private const string CUSTOM_BUTTON_KEY =		"CUSTOM";
		private const string COPY_BUTTON_KEY =			"COPY";
		
		private const int MINIMUM_MULTILEVEL_EDITOR_WIDTH = 180;
		private const int MINIMUM_DROP_LIST_ITEMS = 8;
		
		public const System.Windows.Forms.Keys PROPGRID_ADD_HOTKEY = Keys.A;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Infragistics ultra grid control used to create the grid</summary>
		private Infragistics.Win.UltraWinGrid.UltraGrid m_ctrlUltraGrid = null;

		/// <summary>Owner supplied array of objects to define the categories</summary>
		private System.Collections.ICollection m_ICategories = null;

		/// <summary>Local member used as the data source for the grid</summary>
		private System.Data.DataSet m_dsSource = null;

		/// <summary>Local member used to store category names</summary>
		private System.Data.DataTable m_dtCategories = null;

		/// <summary>Local member used to store properties</summary>
		private System.Data.DataTable m_dtProperties = null;

		/// <summary>Images assigned to cell and editor buttons</summary>
		private System.Windows.Forms.ImageList m_ctrlImages = null;

		/// <summary>Local flag to control column sizing</summary>
		private bool m_bResizeColumns = true;

		/// <summary>Local member bound to DropLisBoolean</summary>
		private bool m_bDropListBoolean = true;

		/// <summary>Local member bound to SortOn property</summary>
		private long m_lSortOn = 0;

		/// <summary>Array of type-specific editors to be assigned to Value cells</summary>
		private EmbeddableEditorBase[] m_aEditors = new EmbeddableEditorBase[MAX_PROP_EDITORS];

		/// <summary>Local member to access the drop button for the custom MultiLevel editor</summary>
		private Infragistics.Win.UltraWinEditors.DropDownEditorButton m_multiLevelButton = null;

		/// <summary>Local member to keep track of the cell bound to the custom multi-level editor</summary>
		private Infragistics.Win.UltraWinGrid.UltraGridCell m_multiLevelCell = null;

		/// <summary>The property bound to the last active editor</summary>
		private ITmaxPropGridCtrl m_IEditorProperty = null;

		/// <summary>Text stored in the last active editor</summary>
		private string m_strEditorText = "";

		/// <summary>Text selected in the last active editor</summary>
		private string m_strEditorSelection = "";

		/// <summary>Start index of the text selected in the last active editor</summary>
		private long m_lEditorSelStart = -1;

		//---------------------------------------------------------------------------
		//	Infragistics CellMultiLine vs. AutoComplete bug
		//---------------------------------------------------------------------------
		//
		//	These members are used to fix a bug in the Infragistics combobox editor. 
		//	If the user drops the list, highlights an item with the mouse, and hits Enter,
		//	the list closes as it should but the highlighted item is not selected. We have
		//	to implement that ourselves.
		private int m_iComboEditorIndex = -1;
		private int	m_iComboEditorSelection = -1;
		private FTI.Trialmax.Controls.CTmaxMultiLevelEditorCtrl m_ctrlMultiLevelEditor;

		//---------------------------------------------------------------------------
		//	Infragistics CellMultiLine vs. AutoComplete bug
		//---------------------------------------------------------------------------
		//	This flag is used to enable/disable AutoComplete mode on drop list editors
		//
		//	NOTE:	We have to do this because the CellMultiLine feature breaks
		//			autocomplete processing. If Infragistics gets this fixed we can
		//			enable AutoEdit. Otherwise, if we enable AutoEdit we have to 
		//			dynamically turn CellMultiLine on and off (see SetMultiLineEnabled).
		//			This has the undesired effect of shrinking and expanding grid cells containing
		//			multiple lines of text when we enter and exit drop list editors
		private bool INFRAGISTICS_MULTILINE_BUG_ENABLE_AUTOCOMPLETE = true;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This is the delegate used to handle events fired by this control</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Event arguments</param>
		public delegate void TmaxPropGridHandler(object sender, CTmaxPropGridArgs e);
		
		/// <summary>This event is fired before the value of a property is updated</summary>
		public event TmaxPropGridHandler BeforeUpdate;
		
		/// <summary>This event is fired after the value of a property is updated</summary>
		public event TmaxPropGridHandler AfterUpdate;

		/// <summary>This event is fired when the user presses the hotkey to add a new property</summary>
		public event TmaxPropGridHandler AddKeyPress;

		/// <summary>This event is fired when the user clicks on the Editor button for a custom property</summary>
		public event TmaxPropGridHandler ClickEditorButton;

		/// <summary>This event is fired by the control to request the collection of values for populating a drop list</summary>
		public event TmaxPropGridHandler GetDropListValues;

		/// <summary>This event is fired by the control to propagate mouse down events</summary>
		public event System.Windows.Forms.MouseEventHandler GridMouseDown;

		/// <summary>This event is fired by the control to propagate mouse up events</summary>
		public event System.Windows.Forms.MouseEventHandler GridMouseUp;

		/// <summary>Constructor</summary>
		public CTmaxPropGridCtrl() : base()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "TrialMax Property Grid Control";
			
			//	Initialize the child controls
			InitializeComponent();
			
		}// public CTmaxPropGridCtrl() : base()

		/// <summary>This method is called to initialize the grid to display properties associated with the specified owner type</summary>
		/// <param name="ICategories">An optional list of objects that define the categories</param>
		/// <returns>true if successful</returns>
		/// <remarks>The ToString() member of each object is called to display the category</remarks>
		public bool Initialize(System.Collections.ICollection ICategories)
		{
			try
			{
				//	Create the editors
				CreateEditors();
		
				//	Create the data source
				CreateDataSource(ICategories);
				
				//	Assign the data source to the grid
				if(m_dsSource != null)
					this.UltraGridCtrl.DataSource = m_dsSource;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
			
				//	Clean up
				FreeDataSource();
			}
			
			return (m_dsSource != null);
		
		}// public bool Initialize(ITmaxPropGridCtrl IOwner)
		
		/// <summary>This method is called to add a property to the grid</summary>
		/// <param name="IProperty">The interface to the property being added</param>
		/// <returns>true if successful</returns>
		public bool Add(ITmaxPropGridCtrl IProperty)
		{
			DataRow row = null;
			
			Debug.Assert(IProperty != null);
			if(IProperty == null) return false;
			
			Debug.Assert(m_dtProperties != null, "Control not initialized");
			if(m_dtProperties == null) return false;
			
			try
			{
				//	Create a new row for this property
				row = m_dtProperties.NewRow();

				//	Set the column values
				SetColumns(row, IProperty);
				
				//	Add to the table
				m_dtProperties.Rows.Add(row);
		
				
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX, IProperty.GetName()), Ex);
				return false;
			}
		
		}// public bool Add(ITmaxPropGridCtrl IProperty)
		
		/// <summary>This method is called to make the specified property the selected row</summary>
		/// <param name="IProperty">The interface to the property being selected</param>
		/// <param name="bEditor">True to open the editor for the row</param>
		/// <returns>true if successful</returns>
		public bool SetSelection(ITmaxPropGridCtrl IProperty, bool bEditor)
		{
			UltraGridRow row = null;
			
			try
			{
				m_ctrlUltraGrid.Selected.Rows.Clear();
				
				if(IProperty != null)
				{
					if((row = GetPropertyRow(IProperty)) != null)
					{
						row.Selected = true;
						
						if(bEditor == true)
						{
							try
							{
								m_ctrlUltraGrid.ActiveCell = row.Cells[PROPERTIES_COLUMN_VALUE];
								
								switch(GetEditorEnum(row))
								{
									case TmaxPropGridEditors.Boolean:
									case TmaxPropGridEditors.Combobox:
									case TmaxPropGridEditors.DropList:
									
										m_ctrlUltraGrid.PerformAction(UltraGridAction.EnterEditModeAndDropdown);
										break;
										
									default:
									
										m_ctrlUltraGrid.PerformAction(UltraGridAction.EnterEditMode);
										break;
										
								}// switch(GetEditorEnum(row))
								
							}
							catch
							{
							}
							
						}// if(bEditor == true)
						
					}// if((row = GetPropertyRow(IProperty)) != null)
					
				}// if(IProperty != null)
		
				return true;
			
			}
			catch(System.Exception Ex)
			{
				if(IProperty != null)
					m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX, IProperty.GetName()), Ex);
				else
					m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX, "NULL"), Ex);
				return false;
			}
		
		}// public bool Add(ITmaxPropGridCtrl IProperty)
		
		/// <summary>This method is called to add the specified properties to the grid</summary>
		/// <param name="IProperties">The collection of properties to be added</param>
		/// <param name="bClear">True to clear the existing properties</param>
		/// <returns>true if successful</returns>
		public bool Add(ICollection IProperties, bool bClear)
		{
			bool bSuccessful = true;
			
			//	Are we supposed to clear the existing properties?
			if(bClear)
				Clear(false);

			//	Now add the new properties
			if((IProperties != null) && (IProperties.Count > 0))
			{
				//	Don't resize columns until we're done
				m_bResizeColumns = false;
				
				//	Add each of the properties
				foreach(ITmaxPropGridCtrl O in IProperties)
				{
					if(Add(O) == false)
						bSuccessful = false;				
				}
				
				//	Resize the columns now that all properties have been added
				SetColumnWidths();
				m_bResizeColumns = true;
				
			}// if((IProperties != null) && (IProperties.Count > 0))
			
			return bSuccessful;
		
		}// public bool Add(ICollection IProperties, bool bClear)
		
		/// <summary>This method will use the interface to update the row with the specified Id</summary>
		/// <param name="IProperty">The interface reassigned to the row</param>
		/// <param name="lId">The id used to locate the row</param>
		/// <returns>true if successful</returns>
		public bool Update(ITmaxPropGridCtrl IProperty, long lId)
		{
			DataRow dr = null;
			bool	bSuccessful = false;
			
			//	Locate the row with the specified Id
			if(lId > 0)
			{
				if((dr = GetSourceRow(lId)) == null) return false;
				
				//	This allows the caller to update the property interface
				bSuccessful = SetColumns(dr, IProperty);
			}	
			else
			{
				bSuccessful = Update(IProperty);
			}
			
			return bSuccessful;

		}// public bool Update(ITmaxPropGridCtrl IProperty, long lId)
		
		/// <summary>This method will update the row bound to the specified interface</summary>
		/// <param name="IProperty">The interface bound to the row</param>
		/// <returns>true if successful</returns>
		public bool Update(ITmaxPropGridCtrl IProperty)
		{
			DataRow dr = null;
			bool	bSuccessful = true;
			
			if(m_dtProperties == null) return false;
			if(m_dtProperties.Rows == null) return false;
			
			//	Does the caller want to update all properties?
			if(IProperty == null)
			{
				foreach(DataRow O in m_dtProperties.Rows)
				{
					if((IProperty = (ITmaxPropGridCtrl)(O[PROPERTIES_COLUMN_INTERFACE])) != null)
					{
						try
						{
							if((O[PROPERTIES_COLUMN_NAME] == null) || (O[PROPERTIES_COLUMN_NAME].ToString() != IProperty.GetName()))
								O[PROPERTIES_COLUMN_NAME]	= IProperty.GetName();
							if((O[PROPERTIES_COLUMN_VALUE] == null) || (O[PROPERTIES_COLUMN_VALUE].ToString() != IProperty.GetValue()))
								O[PROPERTIES_COLUMN_VALUE]	= IProperty.GetValue();
						}
						catch
						{
						}
						
					}// if((IProperty = (ITmaxPropGridCtrl)(O[PROPERTIES_COLUMN_INTERFACE])) != null)
				
				}// foreach(DataRow O in m_dtProperties.Rows)
			
			}
			else
			{	
				if((dr = GetSourceRow(IProperty)) != null)
				{
					bSuccessful = SetColumns(dr, IProperty);
				}
				
			}// if(IProperty == null)
			
			return bSuccessful;
		
		}// public bool Update(ITmaxPropGridCtrl IProperty)
		
		/// <summary>This method is called to end the active update</summary>
		/// <param name="bCancel">True to cancel changes that may have been made</param>
		/// <returns>true if successful</returns>
		public bool EndUserUpdate(bool bCancel)
		{
			bool bSuccessful = true;
			
			//	Are we in edit mode?
			if((m_ctrlUltraGrid.CurrentState & UltraGridState.InEdit) != 0)
			{
				//	Are we supposed to cancel all changes?
				if(bCancel == true)
				{
					try { m_ctrlUltraGrid.PerformAction(UltraGridAction.UndoCell); }
					catch { bSuccessful = false; }
				}
					
				//	Get out of edit mode
				try { m_ctrlUltraGrid.PerformAction(UltraGridAction.ExitEditMode); }
				catch {}
				
			}// if((m_ctrlUltraGrid.CurrentState & UltraGridState.InEdit) != 0)
			
			return bSuccessful;
		
		}// public bool EndUserUpdate(bool bCancel)
		
		/// <summary>This method will use the interface to delete the row with the specified Id</summary>
		/// <param name="lId">The id used to locate the row</param>
		/// <returns>true if successful</returns>
		public bool Delete(long lId)
		{
			UltraGridRow row = null;
			
			if((row = GetPropertyRow(lId)) != null)
			{
				return Delete(row);
			}
			else
			{
				m_tmaxEventSource.FireDiagnostic(this, "Delete", "Unable to locate data row with id = " + lId.ToString());
				return false;
			}

		}// public bool Delete(long lId)
		
		/// <summary>This method will delete the row bound to the specified interface</summary>
		/// <param name="IProperty">The interface bound to the row</param>
		/// <returns>true if successful</returns>
		public bool Delete(ITmaxPropGridCtrl IProperty)
		{
			UltraGridRow row = null;
			
			//	Get the requested row in the data source
			if((row = GetPropertyRow(IProperty)) != null)
			{
				return Delete(row);
			}
			else
			{
				m_tmaxEventSource.FireDiagnostic(this, "Delete", "Unable to locate data row with specified Interface");
				return false;
			}

		}// public bool Delete(ITmaxPropGridCtrl IProperty)

		/// <summary>This method is called to get the property that is selected in the grid</summary>
		/// <param name="strValue">The text that appears in the value cell</param>
		/// <param name="bIncludeEditMode">True to consider the row selected if in edit mode</param>
		/// <returns>The selected property, null if none or more than one selected</returns>
		public ITmaxPropGridCtrl GetSelection(ref string strValue, bool bIncludeEditMode)
		{
			ArrayList aSelected = new ArrayList();
			ITmaxPropGridCtrl IProperty = null;
			UltraGridRow row = null;

			if(GetSelected(aSelected, bIncludeEditMode) == 1)
			{
				IProperty = (ITmaxPropGridCtrl)(aSelected[0]);
				if((row = GetPropertyRow(IProperty)) != null)
					strValue = row.Cells[PROPERTIES_COLUMN_VALUE].Text;
			}

			return IProperty;

		}// public ITmaxPropGridCtrl GetSelection(ref string strValue, bool bIncludeEditMode)

		/// <summary>This method is called to get the property that is selected in the grid</summary>
		/// <param name="strValue">The text that appears in the value cell</param>
		/// <param name="bIncludeEditMode">True to consider the row selected if in edit mode</param>
		/// <returns>The selected property, null if none or more than one selected</returns>
		public ITmaxPropGridCtrl GetSelection(ref string strValue)
		{
			return GetSelection(ref strValue, false);
		}

		/// <summary>This method is called to get the property that is selected in the grid</summary>
		/// <param name="bIncludeEditMode">True to consider the row selected if in edit mode</param>
		/// <returns>The selected property, null if none or more than one selected</returns>
		public ITmaxPropGridCtrl GetSelection(bool bIncludeEditMode)
		{
			string strText = "";
			return GetSelection(ref strText, bIncludeEditMode);
		}

		/// <summary>This method is called to get the property that is selected in the grid</summary>
		/// <returns>The selected property, null if none or more than one selected</returns>
		public ITmaxPropGridCtrl GetSelection()
		{
			return GetSelection(false);
		}

		/// <summary>This method is called to get the collection of selected properties</summary>
		/// <param name="aSelected">The collection in which to store the properties</param>
		/// <param name="bIncludeEditMode">True to consider the row selected if in edit mode</param>
		/// <returns>The total number of selected properties</returns>
		public int GetSelected(ArrayList aSelected, bool bIncludeEditMode)
		{
			return GetSelected(aSelected, null, bIncludeEditMode);
		}
		
		/// <summary>This method is called to get the collection of selected properties</summary>
		/// <param name="bIncludeEditMode">True to consider the row selected if in edit mode</param>
		/// <returns>The collection of selected properties if any</returns>
		public ArrayList GetSelected(bool bIncludeEditMode)
		{
			ArrayList aSelected = new ArrayList();
			
			if(GetSelected(aSelected, bIncludeEditMode) > 0)
				return aSelected;
			else
				return null;

		}// public ArrayList GetSelected(bool bIncludeEditMode)

		/// <summary>This method is called to get the collection of selected properties</summary>
		/// <returns>The collection of selected properties if any</returns>
		public ArrayList GetSelected()
		{
			return GetSelected(false);
		}

		/// <summary>This method is called to get the total number of selections</summary>
		/// <param name="bIncludeEditMode">True to consider the row selected if in edit mode</param>
		/// <returns>The number of selected rows</returns>
		public int GetSelectedCount(bool bIncludeEditMode)
		{
			return GetSelected(null, bIncludeEditMode);				
		}

		/// <summary>This method is called to get the total number of selections</summary>
		/// <returns>The number of selected rows</returns>
		public int GetSelectedCount()
		{
			return GetSelected(null, false);
		}

		/// <summary>This method is called to get the property being edited and it's current text</summary>
		/// <param name="strText">The total text</param>
		/// <param name="strSelected">The selected text</param>
		/// <param name="lIndex">The index where the selection starts</param>
		/// <returns>The property being edited</returns>
		public ITmaxPropGridCtrl GetEditorProps(ref string strText, ref string strSelected, ref long lIndex)
		{
			return GetEditorProps(null, ref strText, ref strSelected, ref lIndex);
		}
		
		/// <summary>This method is called to get the property being edited and it's current text</summary>
		/// <param name="strText">The total text</param>
		/// <param name="strSelected">The selected text</param>
		/// <returns>The property being edited</returns>
		public ITmaxPropGridCtrl GetEditorProps(ref string strText, ref string strSelected)
		{
			long lIndex = 0;
			return GetEditorProps(ref strText, ref strSelected, ref lIndex);
		}
		
		/// <summary>This method is called to get the property being edited and it's current text</summary>
		/// <param name="strText">The total text</param>
		/// <returns>The property being edited</returns>
		public ITmaxPropGridCtrl GetEditorProps(ref string strText)
		{
			long	lIndex = 0;
			string	strSelected = "";
			
			return GetEditorProps(ref strText, ref strSelected, ref lIndex);
		}
		
		/// <summary>This method is called to get the property being edited and it's current text</summary>
		/// <returns>The property being edited</returns>
		public ITmaxPropGridCtrl GetEditorProps()
		{
			long	lIndex = 0;
			string	strSelected = "";
			string	strText = "";
			return GetEditorProps(ref strText, ref strSelected, ref lIndex);
		}
		
		/// <summary>This method is called to sort the objects in the grid</summary>
		/// <returns>true if successful</returns>
		public bool Sort()
		{
			UltraGridColumn column = null;
			
			if(m_ctrlUltraGrid == null) return false;
			if(m_ctrlUltraGrid.IsDisposed == true) return false;
			if(m_ctrlUltraGrid.Rows == null) return false;
			if(m_ctrlUltraGrid.Rows.Count == 0) return false;
			if(m_dtProperties == null) return false;
			if(m_dtProperties.Rows == null) return false;
			if(m_dtProperties.Rows.Count == 0) return false;
			
			try
			{
				//	Get the column we plan to sort on
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_INTERFACE)) == null)
					return false;
					
				if(column.SortComparer == null)
					column.SortComparer = new CTmaxPropGridComparer(this.SortOn);
				column.SortIndicator = SortIndicator.Ascending;
				
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Sort", m_tmaxErrorBuilder.Message(ERROR_SORT_EX), Ex);
				return false;
			}
		
		}// public bool Sort()
		
		/// <summary>This method is called to set the column widths</summary>
		public void SetColumnWidths()
		{
			UltraGridColumn	column = null;
			int	iMaxWidth;
			
			if(m_dtProperties == null) return;
			if(m_dtProperties.Rows == null) return ;
			if(m_dtProperties.Rows.Count == 0) return;
			
			try
			{
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_ID)) != null)
				{
					if(column.Hidden == false)
					{
						iMaxWidth = column.CalculateAutoResizeWidth(PerformAutoSizeType.AllRowsInBand, true);
						column.MinWidth = iMaxWidth;
						column.MaxWidth = iMaxWidth;
					}
					
				}
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_NAME)) != null)
				{
					iMaxWidth = column.CalculateAutoResizeWidth(PerformAutoSizeType.AllRowsInBand, true);
					
					//	This prevents Infragistics from raising an exception 
					//	if Min > Max
					if(column.MaxWidth >= iMaxWidth)
					{
						//	Set the minimum first then the maximum
						column.MinWidth = iMaxWidth;
						column.MaxWidth = iMaxWidth;
					}
					else
					{
						//	Set the maximum first
						column.MaxWidth = iMaxWidth;
						column.MinWidth = iMaxWidth;
					}
					
				}

//				if((column = GetPropertyColumn(PROPERTIES_COLUMN_VALUE)) != null)
//				{
//					column.PerformAutoResize();
//					
//				}
				// NOTE: Because we set the AutoFitColumns = true, the last column (VALUE) will
				//		 automatically size to use the rest of the window
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetColumnWidths", Ex);
			}

		}// public void SetColumnWidths()
		
		/// <summary>This method is called to clear the grid</summary>
		///	<param name="bAll">true to clear all properties and categories</param>
		public void Clear(bool bAll)
		{
			try
			{
				//	Are we clearing the entire grid?
				if(bAll == true)
				{
					FreeDataSource();
				}
				else
				{
					//	Clear only the properties
					if((m_dtProperties != null) && (m_dtProperties.Rows != null))
					{
						if(m_dtProperties.Rows.Count > 0)
							m_dtProperties.Rows.Clear();
					}
				
				}// if(bAll == true)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Clear", Ex);
			}
		
		}// public void Clear()

		#endregion Public Methods
		
		#region Protected Methods
		
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
		
		}// protected override void Dispose(bool disposing)

		/// <summary>Required method for Designer support</summary>
		protected new void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTmaxPropGridCtrl));
			this.m_ctrlUltraGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlMultiLevelEditor = new FTI.Trialmax.Controls.CTmaxMultiLevelEditorCtrl();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlUltraGrid
			// 
			this.m_ctrlUltraGrid.Cursor = System.Windows.Forms.Cursors.Default;
			appearance1.BackColor = System.Drawing.Color.White;
			this.m_ctrlUltraGrid.DisplayLayout.Appearance = appearance1;
			ultraGridBand1.ColHeadersVisible = false;
			this.m_ctrlUltraGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
			this.m_ctrlUltraGrid.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
			appearance2.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.m_ctrlUltraGrid.DisplayLayout.Override.CellAppearance = appearance2;
			appearance3.BackColor = System.Drawing.Color.White;
			appearance3.BorderColor = System.Drawing.SystemColors.Control;
			this.m_ctrlUltraGrid.DisplayLayout.Override.RowAppearance = appearance3;
			this.m_ctrlUltraGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraGrid.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFree;
			this.m_ctrlUltraGrid.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.Control;
			this.m_ctrlUltraGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
			this.m_ctrlUltraGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.m_ctrlUltraGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlUltraGrid.ImageList = this.m_ctrlImages;
			this.m_ctrlUltraGrid.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlUltraGrid.Name = "m_ctrlUltraGrid";
			this.m_ctrlUltraGrid.Size = new System.Drawing.Size(260, 308);
			this.m_ctrlUltraGrid.TabIndex = 0;
			this.m_ctrlUltraGrid.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.OnInitializeRow);
			this.m_ctrlUltraGrid.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.OnBeforeCellUpdate);
			this.m_ctrlUltraGrid.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.OnBeforeEnterEditMode);
			this.m_ctrlUltraGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnUltraMouseDown);
			this.m_ctrlUltraGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnUltraMouseUp);
			this.m_ctrlUltraGrid.AfterExitEditMode += new System.EventHandler(this.OnAfterExitEditMode);
			this.m_ctrlUltraGrid.DoubleClick += new System.EventHandler(this.OnUltraDoubleClick);
			this.m_ctrlUltraGrid.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.OnBeforeCellListDropDown);
			this.m_ctrlUltraGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.OnInitializeLayout);
			this.m_ctrlUltraGrid.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.OnClickCellButton);
			this.m_ctrlUltraGrid.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.OnBeforeExitEditMode);
			this.m_ctrlUltraGrid.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.OnAfterCellListCloseUp);
			this.m_ctrlUltraGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnUltraKeyDown);
			this.m_ctrlUltraGrid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnUltraKeyPress);
			this.m_ctrlUltraGrid.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.OnAfterCellUpdate);
			this.m_ctrlUltraGrid.BeforeAutoSizeEdit += new Infragistics.Win.UltraWinGrid.CancelableAutoSizeEditEventHandler(this.OnBeforeAutoSizeEdit);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlImages.Images.SetKeyName(0, "");
			this.m_ctrlImages.Images.SetKeyName(1, "");
			// 
			// m_ctrlMultiLevelEditor
			// 
			this.m_ctrlMultiLevelEditor.IPropGridCtrl = null;
			this.m_ctrlMultiLevelEditor.Location = new System.Drawing.Point(15, 31);
			this.m_ctrlMultiLevelEditor.MultiLevel = null;
			this.m_ctrlMultiLevelEditor.Name = "m_ctrlMultiLevelEditor";
			this.m_ctrlMultiLevelEditor.PaneId = 0;
			this.m_ctrlMultiLevelEditor.Size = new System.Drawing.Size(135, 214);
			this.m_ctrlMultiLevelEditor.TabIndex = 1;
			this.m_ctrlMultiLevelEditor.UserAdditions = true;
			this.m_ctrlMultiLevelEditor.Value = null;
			// 
			// CTmaxPropGridCtrl
			// 
			this.Controls.Add(this.m_ctrlMultiLevelEditor);
			this.Controls.Add(this.m_ctrlUltraGrid);
			this.Name = "CTmaxPropGridCtrl";
			this.Size = new System.Drawing.Size(260, 308);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraGrid)).EndInit();
			this.ResumeLayout(false);

		}// protected new void InitializeComponent()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		override protected void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Let the base class add it's strings first
			base.SetErrorStrings();
			
			//	Now add the strings for this class
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while creating the categories table");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while creating the properties table");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while creating the data source");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the property grid");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the grid's layout");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the category for the property to be added: property name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding the property to the grid: property name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the BeforeUpdate event");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the AfterUpdate event");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while setting the column values: property name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the current selections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the event arguments");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while searching for a property row");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while deleting the data row from the Properties table");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while deleting the row from the Properties grid");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the property editors");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to sort the objects in the grid.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the drop down list: property name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the current selection: property name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the event to retrieve the drop list values");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to locate the row currently in edit mode.");

		}// override protected void SetErrorStrings()

		#endregion Protected Methods
		
		#region Private Methods

		/// <summary>This method is called to get the editor properties associated with the specified row</summary>
		/// <param name="row">The row that owns the editor</param>
		/// <param name="strText">The total text</param>
		/// <param name="strSelected">The selected text</param>
		/// <param name="lIndex">The index where the selection starts</param>
		/// <returns>The property being edited</returns>
		private ITmaxPropGridCtrl GetEditorProps(UltraGridRow row, ref string strText, ref string strSelected, ref long lIndex)
		{
			ITmaxPropGridCtrl tmaxProperty = null;

			try
			{
				//	Do we need to locate the active row?
				if(row == null)
					row = GetEditorRow(null);
					
				//	Get the row bound to the specified property
				if(row != null)
				{
					if((row.Cells != null) && (row.Cells[PROPERTIES_COLUMN_VALUE] != null))
					{
						strText = row.Cells[PROPERTIES_COLUMN_VALUE].Text;
						strSelected = row.Cells[PROPERTIES_COLUMN_VALUE].SelText;
						lIndex = row.Cells[PROPERTIES_COLUMN_VALUE].SelStart;

					}// if((row.Cells != null) && (row.Cells[PROPERTIES_COLUMN_VALUE] != null))

					tmaxProperty = (ITmaxPropGridCtrl)(row.Cells[PROPERTIES_COLUMN_INTERFACE].Value);

				}
				else
				{
					//	Assign the values saved when the edit session ended
					tmaxProperty = m_IEditorProperty;
					strText = m_strEditorText;
					strSelected = m_strEditorSelection;
					lIndex = m_lEditorSelStart;

				}// // if((row = GetEditorRow()) != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetEditorProps", Ex);
			}

			return tmaxProperty;

		}// public ITmaxPropGridCtrl GetEditorProps(UltraGridRow row, ref string strText, ref string strSelected, ref long lIndex)

		/// <summary>This method will release the data source and reset the local members</summary>
		private void FreeDataSource()
		{
			try
			{
				m_ctrlUltraGrid.DataSource = null;
				
				if(m_dsSource != null)
					m_dsSource.Clear();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FreeDataSource", Ex);
			}
			finally
			{
				m_dsSource = null;
				m_dtCategories = null;
				m_dtProperties = null;
				m_ICategories = null;
				m_IEditorProperty = null;
				m_strEditorText = "";
				m_strEditorSelection = "";
				m_lEditorSelStart = -1;
			}
			
		}// private void FreeDataSource()
		
		/// <summary>This method will create a new data source for the grid</summary>
		/// <param name="ICategories">The collection of categories for the new source</param>
		/// <returns>The data set to be assigned as the source for the grid control</returns>
		private System.Data.DataSet CreateDataSource(System.Collections.ICollection ICategories)
		{
			//	Release the existing data source
			FreeDataSource();
			
			try
			{
				//	Save the categories collection
				m_ICategories = ICategories;
				
				//	Create the data tables
				CreateCategoriesTable();
				CreatePropertiesTable();
				
				//	Nothing to do if no properties
				if(m_dtProperties == null) return null;
				
				//	Create the source data set
				m_dsSource = new System.Data.DataSet();
					
				//	Do we have a categories table?
				if(m_dtCategories != null)
				{
					//	Add the categories and properties tables
					m_dsSource.Tables.Add(m_dtCategories);
					m_dsSource.Tables.Add(m_dtProperties);
					
					//	Establish the relationship between the two tables
					DataRelation drBind = new DataRelation("CatProp", m_dtCategories.Columns["Id"], m_dtProperties.Columns["Category"]);
					m_dsSource.Relations.Add(drBind);

				}
				else
				{
					//	All we have to do is add the properties table
					m_dsSource.Tables.Add(m_dtProperties);
				
				}// if(m_dtCategories != null)
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDataSource", m_tmaxErrorBuilder.Message(ERROR_CREATE_DATA_SOURCE_EX), Ex);
				FreeDataSource();
			}
			
			return m_dsSource;
		
		}// private System.Data.DataSet CreateDataSource()
		
		/// <summary>This method will create the table of categories to be included in the data set</summary>
		/// <returns>The new Categories table</returns>
		private System.Data.DataTable CreateCategoriesTable()
		{
			int i = 1;
			
			Debug.Assert(m_dtCategories == null);
			
			try
			{
				//	Nothing to do if no categories defined
				if((m_ICategories == null) || (m_ICategories.Count == 0))
					return null;

				//	Create a data table for the categories
				m_dtCategories = new DataTable("Categories");
				
				//	Add the Id columns
				m_dtCategories.Columns.Add(CATEGORIES_COLUMN_ID, typeof(Int32));
				m_dtCategories.Columns.Add(CATEGORIES_COLUMN_NAME, typeof(string));
				m_dtCategories.Columns.Add(CATEGORIES_COLUMN_TAG, typeof(object));
				
				//	Set the primary key for the categories
				DataColumn[] dtKeys = { m_dtCategories.Columns["Id"] };
				m_dtCategories.PrimaryKey = dtKeys;
				
				//	Add rows for each category
				foreach(object O in m_ICategories)
				{
					m_dtCategories.Rows.Add(new object[] { i, O.ToString(), O } );
					i++;
				}
				
				//	Make sure we added at least one category
				if((m_dtCategories.Rows == null) || (m_dtCategories.Rows.Count == 0))
					m_dtCategories = null;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateCategoriesTable", m_tmaxErrorBuilder.Message(ERROR_CREATE_CATEGORIES_EX), Ex);
			}
			
			return m_dtCategories;
			
		}// private System.Data.DataTable CreateCategoriesTable()
		
		/// <summary>This method will create the table of properties to be included in the data set</summary>
		/// <returns>The new Properties table</returns>
		private System.Data.DataTable CreatePropertiesTable()
		{
			Debug.Assert(m_dtProperties == null);
			
			try
			{
				//	Create a data table for the categories
				m_dtProperties = new DataTable("Properties");
				
				//	Do we need a category column?
				m_dtProperties.Columns.Add(PROPERTIES_COLUMN_CATEGORY, typeof(Int32));
				m_dtProperties.Columns.Add(PROPERTIES_COLUMN_ID, typeof(long));
				m_dtProperties.Columns.Add(PROPERTIES_COLUMN_NAME, typeof(string));
				m_dtProperties.Columns.Add(PROPERTIES_COLUMN_VALUE, typeof(string));
				m_dtProperties.Columns.Add(PROPERTIES_COLUMN_TAG, typeof(object));
				m_dtProperties.Columns.Add(PROPERTIES_COLUMN_INTERFACE, typeof(object));

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreatePropertiesTable", m_tmaxErrorBuilder.Message(ERROR_CREATE_PROPERTIES_EX), Ex);
			}
			
			return m_dtProperties;
			
		}// private System.Data.DataTable CreatePropertiesTable()
		
		/// <summary>This method handles events fired by the grid when it initializes a new row</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnInitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			EmbeddableEditorBase eeb = null;

			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnInitializeRow", "Initialize Row -> Reinitialize = " + e.ReInitialize.ToString());

				//	Is this a property row
				if(IsPropertiesRow(e.Row) == true)
				{
					//	Prevent the user from tabbing to the Name column
					e.Row.Cells[PROPERTIES_COLUMN_NAME].TabStop = DefaultableBoolean.False;

					if(GetEditorEnum(e.Row) != TmaxPropGridEditors.None)
					{
						//	Assign the editor
						if((eeb = GetEditor(e.Row)) != null)
						{
							e.Row.Cells[PROPERTIES_COLUMN_VALUE].Editor = eeb;
							
//							if(GetEditorEnum(e.Row) == TmaxPropGridEditors.Boolean)
//							{
//								e.Row.Cells[PROPERTIES_COLUMN_VALUE].Style = ColumnStyle.DropDownValidate;
//							}
						
//							if(GetEditorEnum(e.Row) == TmaxPropGridEditors.DropList)
//							{
//								e.Row.Cells[PROPERTIES_COLUMN_VALUE].Style = ColumnStyle.DropDown;
//							}
						
						}
						
					}
					else
					{
						e.Row.Cells[PROPERTIES_COLUMN_VALUE].Activation = Activation.ActivateOnly;
						e.Row.Cells[PROPERTIES_COLUMN_NAME].Appearance.ForeColor = System.Drawing.SystemColors.ControlDark;
						e.Row.Cells[PROPERTIES_COLUMN_VALUE].Appearance.ForeColor = System.Drawing.SystemColors.ControlDark;

						e.Row.Cells[PROPERTIES_COLUMN_VALUE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
						e.Row.Cells[PROPERTIES_COLUMN_VALUE].ButtonAppearance.Image = 0;
						
					}	
					
					if(m_bResizeColumns == true)
						SetColumnWidths();
				
				}// if(IsPropertiesRow(e.Row) == true)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnInitializeRow", Ex);
			}
			
		}// private void OnInitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)

		/// <summary>This method handles events fired by the grid when it attempts to initialize its layout</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnInitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			UltraGridColumn column = null;
			UltraGridBand	band = null;
			
			//this.Columns[0].AutoEdit = false;
			//this.Columns[0].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

			try
			{

				//	Hide the band headers
				foreach(UltraGridBand O in e.Layout.Bands)
				{
					O.HeaderVisible = false;
					O.ColHeadersVisible = false;
				}
			
				//	Set the properties for columns in the Categories band
				if(this.HasCategories)
				{
					if((column = GetCategoryColumn(CATEGORIES_COLUMN_ID)) != null)
						column.Hidden = true;
					if((column = GetCategoryColumn(CATEGORIES_COLUMN_TAG)) != null)
						column.Hidden = true;
					
					if((column = GetCategoryColumn(CATEGORIES_COLUMN_NAME)) != null)
					{
						column.CellActivation = Activation.Disabled;						
						column.CellAppearance.BackColorDisabled = System.Drawing.SystemColors.Control;
						column.CellAppearance.ForeColorDisabled = System.Drawing.SystemColors.ControlText;
					}
					
					if((band = GetPropertiesBand()) != null)
						band.Indentation = 0;
				}
				
				//m_ctrlUltraGrid.DisplayLayout.AutoFitColumns = true;
				m_ctrlUltraGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
					
				//	Set the properties for columns in the Properties band
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_CATEGORY)) != null)
					column.Hidden = true;
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_TAG)) != null)
					column.Hidden = true;
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_INTERFACE)) != null)
					column.Hidden = true;
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_ID)) != null)
					column.Hidden = true;
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_NAME)) != null)
				{
					// DEPRECATED IN NETADVANTAGE 8.3
					//column.AutoEdit = false;
					
					column.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
					column.CellActivation = Activation.NoEdit;
					column.MinWidth = 10;
				}
				
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_VALUE)) != null)
				{
					column.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
					column.AutoSizeEdit = DefaultableBoolean.True;
					column.CellMultiLine = DefaultableBoolean.True;
					
					//---------------------------------------------------------------------------
					//	See description of INFRAGISTICS_MULTILINE_BUG_ENABLE_AUTOCOMPLETE
					//---------------------------------------------------------------------------
					
					// DEPRECATED IN NETADVANTAGE 8.3
					//column.AutoEdit = INFRAGISTICS_MULTILINE_BUG_ENABLE_AUTOCOMPLETE;

					if(INFRAGISTICS_MULTILINE_BUG_ENABLE_AUTOCOMPLETE == false)
						column.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
					else
						column.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
				
				}
				e.Layout.Override.RowSizing = RowSizing.AutoFree;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnInitializeLayout", m_tmaxErrorBuilder.Message(ERROR_ON_INITIALIZE_LAYOUT_EX), Ex);
			
				//	Clean up
				FreeDataSource();
			}

		}// private void OnInitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
	
		/// <summary>This method enabled/disabled multiline editing</summary>
		/// <param name="bEnabled">True to enable multiline editing</param>
		private void SetMultilineEnabled(bool bEnabled)
		{
			UltraGridColumn column = null;

			try
			{
				if((column = GetPropertyColumn(PROPERTIES_COLUMN_VALUE)) != null)
				{
					if(bEnabled == true)
					{
						column.CellMultiLine = DefaultableBoolean.True;
						//m_ctrlUltraGrid.DisplayLayout.Override.RowSizing = RowSizing.AutoFixed;
					}
					else
					{
						//m_ctrlUltraGrid.DisplayLayout.Override.RowSizing = RowSizing.Fixed;
						column.CellMultiLine = DefaultableBoolean.False;
					}
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetMultilineEnabled", Ex);
			}

		}// private void SetMultilineEnabled(bool bEnabled)
	
		/// <summary>This method handles events fired by the grid after it updates the value in a cell</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			m_tmaxEventSource.FireDiagnostic(this, "OnAfterCellUpdate", "After Cell Update");
			
			//	Fire the event to inform the owner
			FireAfterUpdate(e.Cell);
			
			if(GetEditorEnum(e.Cell.Row) == TmaxPropGridEditors.Memo)
				e.Cell.Row.PerformAutoSize();
		
		}// private void OnAfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)

		/// <summary>This method handles events fired by the grid before it updates the value in a cell</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnBeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
		{
			m_tmaxEventSource.FireDiagnostic(this, "OnBeforeCellUpdate", "Before Cell Update");

			//	Cancel if unable to perform the update
			e.Cancel = (PerformUpdate(e.Cell, e.NewValue, null, false) == false);
		}

		/// <summary>This method handles events fired by the grid before it updates the value in a cell</summary>
		/// <param name="cell">The cell that is being edited</param>
		/// <param name="newValue">The new value being assigned to the cell</param>
		/// <param name="tmaxPickItem">The pick list item required for multilevel updates</param>
		/// <returns>true if successful</returns>
		private bool PerformUpdate(UltraGridCell cell, object newValue, CTmaxPickItem tmaxPickItem, bool bSuppressAddConfirmation)
		{
			CTmaxPropGridArgs	Args = null;
			string				strMsg = "";
			bool				bSuccessful = true;
			
			//	Fire the notification event
			Args = FireBeforeUpdate(cell, newValue, tmaxPickItem, bSuppressAddConfirmation);
			
			//	Is it OK to continue?
			if((Args != null) && (Args.Cancel == false))
			{
				//	Set the value of the property
				if(Args.IProperty != null)
				{
					//	Is this a multilevel value?
					if(Args.PickList != null)
						bSuccessful = Args.IProperty.SetValue(Args.PickList, Args.Value);
					else
						bSuccessful = Args.IProperty.SetValue(Args.Value);
						
					if(bSuccessful == false)
					{
						strMsg = String.Format("Unable to set {0} to {1}", Args.IProperty.GetName(), Args.Value);
						MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					
				}// if(Args.IProperty != null)
					
			}
			else
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
		
		}// private bool PerformUpdate(UltraGridCell cell, object newValue, CTmaxPickItem tmaxPickItem)

		/// <summary>This method handles events fired by the grid before it drops the multi-level child control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnBeforeDropMultiLevel(object sender, Infragistics.Win.UltraWinEditors.BeforeEditorButtonDropDownEventArgs e)
		{
			ITmaxPropGridCtrl	IProperty = null;
			CTmaxCaseCode		tmaxCaseCode = null;
			int					iWidth = 0;
			
			try
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnBeforeDropMultiLevel", "Before MultiLevel");

				//	The current context should be the grid cell
				if((m_multiLevelCell = (UltraGridCell)(e.Context)) != null)
				{
					//	Get the property interface bound to this cell
					if((IProperty = GetPropertyInterface(m_multiLevelCell)) != null)
					{
						//	Get the case code bound to this property
						if((tmaxCaseCode = IProperty.GetCaseCode()) != null)
						{
							//	Set the width to use the full column
							if((iWidth = GetPropertyColumnWidth(PROPERTIES_COLUMN_VALUE)) < MINIMUM_MULTILEVEL_EDITOR_WIDTH)
								iWidth = MINIMUM_MULTILEVEL_EDITOR_WIDTH;
								
							m_ctrlMultiLevelEditor.Width = iWidth;
							m_ctrlMultiLevelEditor.IPropGridCtrl = IProperty;
							m_ctrlMultiLevelEditor.MultiLevel = tmaxCaseCode.PickList;
							m_ctrlMultiLevelEditor.Value = IProperty.GetPickItem();
							m_ctrlMultiLevelEditor.OnBeforeDropDown();
						}
						else
						{
							e.Cancel = true;
							m_tmaxEventSource.FireDiagnostic(this, "OnBeforeDropMultiLevel", "No case code available");
						}
							
					}
					else
					{
						e.Cancel = true;
						m_tmaxEventSource.FireDiagnostic(this, "OnBeforeDropMultiLevel", "No property available");
					
					}// if((IProperty = GetPropertyInterface(m_multiLevelCell)) != null)
				
				}// if((m_multiLevelCell = (UltraGridCell)(e.Context)) != null)
				else
				{
					e.Cancel = true;
					m_tmaxEventSource.FireDiagnostic(this, "OnBeforeDropMultiLevel", "No editor cell available");
				}

			}
			catch(System.Exception Ex)
			{
				e.Cancel = true;
				m_tmaxEventSource.FireDiagnostic(this, "OnBeforeDropMultiLevel", Ex);
			}

		}// private void OnBeforeDropMultiLevel(object sender, Infragistics.Win.UltraWinEditors.BeforeEditorButtonDropDownEventArgs e)

		/// <summary>This method handles events fired by the grid after it closes the multi-level child control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAfterCloseMultiLevel(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			try
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAfterCloseMultiLevel", "After MultiLevel");
			}
			catch (System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAfterCloseMultiLevel", Ex);
			}

		}// private void OnAfterCloseMultiLevel(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)

		/// <summary>This method handles events fired by the multilevel child control when the user finishes the operation</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnMultiLevelFinished(object sender, System.EventArgs e)
		{
			bool				bClose = true;
			ITmaxPropGridCtrl	IProperty = null;
			
			try
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnMultiLevelFinished", "Finished MultiLevel");

				//	Did the user assign a new value?
				if(m_ctrlMultiLevelEditor.Cancelled == false)
				{
					//	Set the new value
					bClose = PerformUpdate(m_multiLevelCell, m_ctrlMultiLevelEditor.Value, m_ctrlMultiLevelEditor.ParentList, true);
					
				}
				
				//	Should we close the editor?
				if(bClose == true)
				{
					//	Fire the event to inform the owner
					FireAfterUpdate(m_multiLevelCell);
			
					//	Update the contents displayed by the grid
					if((IProperty = GetPropertyInterface(m_multiLevelCell)) != null)
					{
						Update(IProperty);
						((EditorWithText)(m_aEditors[PROP_EDITOR_MULTI_LEVEL])).TextBox.Text = IProperty.GetValue();
					}
					
					//	Close the custom editor
					this.Focus();
					m_multiLevelButton.CloseUp();
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnMultiLevelFinished", Ex);
			}

		}// private void OnMultiLevelFinished(object sender, System.EventArgs e)

		/// <summary>This method is called to get the specified category column</summary>
		/// <param name="strName">The name of the desired column</param>
		///	<returns>The column object with the specified name</returns>
		private UltraGridColumn GetCategoryColumn(string strName)
		{
			UltraGridColumn column = null;
			UltraGridBand	band = null;
			
			try
			{
				//	Get the band containing the categories
				if((band = GetCategoriesBand()) != null)
					column = band.Columns[strName];
			}
			catch(System.Exception Ex)
			{
				FTI.Shared.Trialmax.CTmaxDiagnosticArgs.CDiagnosticItems items = new FTI.Shared.Trialmax.CTmaxDiagnosticArgs.CDiagnosticItems();
				items.Add(new CTmaxDiagnosticArgs.CDiagnosticItem("Name", strName));
				
				m_tmaxEventSource.FireDiagnostic(this, "GetCategoryColumn", "EXCEPTION", items, Ex);
			}
			
			return column;
		
		}// private UltraGridColumn GetCategoryColumn(string strName)
	
		/// <summary>This method is called to get the specified property column</summary>
		/// <param name="strName">The name of the desired column</param>
		///	<returns>The column object with the specified name</returns>
		private UltraGridColumn GetPropertyColumn(string strName)
		{
			UltraGridColumn column = null;
			UltraGridBand	band = null;
			
			try
			{
				//	Get the band containing the properties
				if((band = GetPropertiesBand()) != null)
				{
					column = band.Columns[strName];
				}

			}
			catch(System.Exception Ex)
			{
				FTI.Shared.Trialmax.CTmaxDiagnosticArgs.CDiagnosticItems items = new FTI.Shared.Trialmax.CTmaxDiagnosticArgs.CDiagnosticItems();
				items.Add(new CTmaxDiagnosticArgs.CDiagnosticItem("Name", strName));
				
				m_tmaxEventSource.FireDiagnostic(this, "GetPropertyColumn", "EXCEPTION", items, Ex);
			}
			
			return column;
		
		}// private UltraGridColumn GetPropertyColumn(string strName)
	
		/// <summary>This method is called to get the width of the specified property column</summary>
		/// <param name="strName">The name of the desired column</param>
		///	<returns>The display width in pixels</returns>
		private int GetPropertyColumnWidth(string strName)
		{
			UltraGridColumn column = null;
			int				iWidth = 0;
			
			try
			{
				//	Get the desired column
				if((column = GetPropertyColumn(strName)) != null)
				{
					iWidth = column.CellSizeResolved.Width;
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetPropertyColumnWidth", Ex);
			}
			
			return iWidth;
		
		}// private int GetPropertyColumnWidth(string strName)
	
		/// <summary>This method is called to get the specified property row</summary>
		/// <param name="IProperty">The interface object associated with the row</param>
		///	<param name="rows">The collection of rows to be searched</param>
		///	<returns>The row object with the specified interface</returns>
		private UltraGridRow GetPropertyRow(ITmaxPropGridCtrl IProperty, RowsCollection rows)
		{
			UltraGridRow row = null;
			
			//	Use the root collection if not specified by the caller
			if(rows == null)
				rows = this.UltraGridCtrl.Rows;
			
			Debug.Assert(rows != null);
			if(rows == null) return null;
			
			try
			{
				foreach(UltraGridRow O in rows)
				{
					//	Is this a property row?
					if(O.Cells.Exists(PROPERTIES_COLUMN_INTERFACE) == true)
					{
						//	Is this the one we're looking for?
						if(ReferenceEquals(O.Cells[PROPERTIES_COLUMN_INTERFACE].Value, IProperty) == true)
							return O;
					}
					else
					{
						//	Does this row have any child rows
						if((O.ChildBands != null) && (O.ChildBands.Count > 0))
						{
							foreach(UltraGridChildBand childBand in O.ChildBands)
							{
								if(childBand.Rows != null)
								{
									if((row = GetPropertyRow(IProperty, childBand.Rows)) != null)
										return row;
								}
							}
							
						}// if((O.ChildBands != null) && (O.ChildBands.Count > 0)
						
					}// if(O.Cells.Exists(PROPERTIES_COLUMN_INTERFACE) == true)

				}// foreach(UltraGridRow O in rows)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetPropertyRow", m_tmaxErrorBuilder.Message(ERROR_GET_PROPERTY_ROW_EX), Ex);
			}
			
			return null;
			
		}// private UltraGridRow GetPropertyRow(ITmaxPropGridCtrl IProperty, RowsCollection rows)
	
		/// <summary>This method is called to get the specified property row</summary>
		/// <param name="IProperty">The interface object associated with the row</param>
		///	<returns>The row object with the specified interface</returns>
		private UltraGridRow GetPropertyRow(ITmaxPropGridCtrl IProperty)
		{
			return GetPropertyRow(IProperty, null);
		}
	
		/// <summary>This method is called to get the specified property row</summary>
		/// <param name="lId">The Id of the desired row</param>
		///	<param name="rows">The collection of rows to be searched</param>
		///	<returns>The row object with the specified Id</returns>
		private UltraGridRow GetPropertyRow(long lId, RowsCollection rows)
		{
			UltraGridRow row = null;
			
			//	Use the root collection if not specified by the caller
			if(rows == null)
				rows = this.UltraGridCtrl.Rows;
			
			Debug.Assert(rows != null);
			if(rows == null) return null;
			
			try
			{
				foreach(UltraGridRow O in rows)
				{
					//	Is this a property row?
					if(O.Cells.Exists(PROPERTIES_COLUMN_ID) == true)
					{
						//	Is this the one we're looking for?
						if(lId == (long)(O.Cells[PROPERTIES_COLUMN_ID].Value))
							return O;
					}
					else
					{
						//	Does this row have any child rows
						if((O.ChildBands != null) && (O.ChildBands.Count > 0))
						{
							foreach(UltraGridChildBand childBand in O.ChildBands)
							{
								if(childBand.Rows != null)
								{
									if((row = GetPropertyRow(lId, childBand.Rows)) != null)
										return row;
								}
								
							}
							
						}// if((O.ChildBands != null) && (O.ChildBands.Count > 0)
						
					}// if(O.Cells.Exists(PROPERTIES_COLUMN_ID) == true)

				}// foreach(UltraGridRow O in rows)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetPropertyRow", m_tmaxErrorBuilder.Message(ERROR_GET_PROPERTY_ROW_EX), Ex);
			}
			
			return null;
			
		}// private UltraGridRow GetPropertyRow(long lId, RowsCollection rows)
	
		/// <summary>This method is called to get the specified property row</summary>
		/// <param name="lId">The Id of the desired row</param>
		///	<returns>The row object with the specified Id</returns>
		private UltraGridRow GetPropertyRow(long lId)
		{
			return GetPropertyRow(lId, null);
		}
	
		/// <summary>Called to get the property interface bound to the specified row</summary>
		/// <param name="row">The grid row to which the interface is bound</param>
		/// <returns>The property interface bound to the specified row</returns>
		private ITmaxPropGridCtrl GetPropertyInterface(UltraGridRow row)
		{
			try
			{
				return ((ITmaxPropGridCtrl)(row.Cells[PROPERTIES_COLUMN_INTERFACE].Value));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetPropertyInterface(row)", Ex);
				return null;
			}

		}// private ITmaxPropGridCtrl GetPropertyInterface(UltraGridRow row)

		/// <summary>Called to get the property interface bound to row containing the specified cell</summary>
		/// <param name="cell">The grid cell that belongs to the desired row</param>
		/// <returns>The property interface bound to the row that owns the specified cell</returns>
		private ITmaxPropGridCtrl GetPropertyInterface(UltraGridCell cell)
		{
			try
			{
				Debug.Assert(cell.Row != null);
				return GetPropertyInterface(cell.Row);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetPropertyInterface(cell)", Ex);
				return null;
			}

		}// private ITmaxPropGridCtrl GetPropertyInterface(UltraGridCell cell)

		/// <summary>This method is called to get the band containing the properties</summary>
		///	<returns>The band containing the properties</returns>
		private UltraGridBand GetPropertiesBand()
		{
			try
			{
				if(m_ctrlUltraGrid == null) return null;
				if(m_ctrlUltraGrid.DisplayLayout == null) return null;
				if(m_ctrlUltraGrid.DisplayLayout.Bands == null) return null;
				if(m_ctrlUltraGrid.DisplayLayout.Bands.Count == 0) return null;
				
				//	Are we using upper level categories?
				if(this.HasCategories)
					return m_ctrlUltraGrid.DisplayLayout.Bands[1];
				else
					return m_ctrlUltraGrid.DisplayLayout.Bands[0];

			}
			catch
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetPropertiesBand", "Exception");
				return null;
			}
		
		}// private UltraGridBand GetPropertiesBand()
	
		/// <summary>This method is called to get the band containing the categories</summary>
		///	<returns>The band containing the categories</returns>
		private UltraGridBand GetCategoriesBand()
		{
			try
			{
				if(this.HasCategories == false) return null;
				if(m_ctrlUltraGrid == null) return null;
				if(m_ctrlUltraGrid.DisplayLayout == null) return null;
				if(m_ctrlUltraGrid.DisplayLayout.Bands == null) return null;
				if(m_ctrlUltraGrid.DisplayLayout.Bands.Count == 0) return null;
				
				return m_ctrlUltraGrid.DisplayLayout.Bands[0];

			}
			catch
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetCategoriesBand", "Exception");
				return null;
			}
		
		}// private UltraGridBand GetCategoriesBand()
	
		/// <summary>This method is called to determine if the specified band is the Properties band</summary>
		///	<returns>True if the band specified by the caller is the properites band</returns>
		private bool IsPropertiesBand(UltraGridBand band)
		{
			UltraGridBand props = GetPropertiesBand();
			
			if(band == null) return false;
			if(props == null) return false;
			
			return ReferenceEquals(band, props);
			
		}// private bool IsPropertiesBand(UltraGridBand band)
	
		/// <summary>This method is called to determine if the specified row represents a Property</summary>
		///	<returns>True if the row specified by the caller is associated with an entry in the Properties table</returns>
		private bool IsPropertiesRow(UltraGridRow row)
		{
			bool bIsProperty = false;
			
			try
			{
				//	To be a property row it must have the interface column
				//
				//	NOTE: This does not necessarily mean the interface has been assigned
				bIsProperty = row.Cells.Exists(PROPERTIES_COLUMN_INTERFACE);
			}
			catch
			{
			}
			
			return bIsProperty;
			
		}// private bool IsPropertiesRow(UltraGridRow row)
	
		/// <summary>This method is called to determine if the specified cell is associated with a Property</summary>
		///	<returns>True if the cell specified by the caller is associated with an entry in the Properties table</returns>
		private bool IsPropertiesCell(UltraGridCell cell)
		{
			if(cell == null) return false;
			if(cell.Row == null) return false;
			
			//	Is the parent row bound to a property?
			return IsPropertiesRow(cell.Row);
			
		}// private bool IsPropertiesCell(UltraGridCell cell)
	
		/// <summary>This method is called to get the Id of the category to which this property belongs</summary>
		/// <param name="IProperty">The property that belongs to one of the categories</param>
		/// <returns>The id of the property if found</returns>
		private int GetCategoryId(ITmaxPropGridCtrl IProperty)
		{
			int		iCategory = -1;
			object	propCategory = null;
			
			Debug.Assert(IProperty != null);
			if(IProperty == null) return -1;
			
			try
			{
				//	Are we using categories?
				if(this.HasCategories == false) return -1;
				
				//	Get the category to which this property belongs
				if((propCategory = IProperty.GetCategory()) == null) return -1;

				//	Locate the category with the same name
				foreach(DataRow O in m_dtCategories.Rows)
				{
					if(String.Compare(propCategory.ToString(), (O[CATEGORIES_COLUMN_NAME]).ToString(), true) == 0)
					{
						iCategory = System.Convert.ToInt32(O[CATEGORIES_COLUMN_ID]);
					}
							
				}// foreach(DataRow O in m_dtCategories.Rows)

			}
			catch
			{
			}
			
			return iCategory;
		
		}// private int GetCategoryId(ITmaxPropGridCtrl IProperty)
		
		/// <summary>This method gets the value for the specified property row as a text string</summary>
		/// <param name="row">The desired property row</param>
		/// <returns>The current value as a text string</returns>
		private string GetValue(UltraGridRow row)
		{
			if((row != null) && (row.Cells != null) && (row.Cells.Exists(PROPERTIES_COLUMN_VALUE) == true))
				return row.Cells[PROPERTIES_COLUMN_VALUE].Text;
			else
				return "";
		
		}// private string GetValue(UltraGridRow row)
		
		/// <summary>This method gets the value for the specified property row as a text string</summary>
		/// <param name="dr">The desired property row</param>
		/// <returns>The current value as a text string</returns>
		private string GetValue(DataRow dr)
		{
			if((dr != null) && (dr[PROPERTIES_COLUMN_VALUE] != null))
				return dr[PROPERTIES_COLUMN_VALUE].ToString();
			else
				return "";
		
		}// private string GetValue(DataRow dr)
		
		/// <summary>This method gets the name for the specified property row as a text string</summary>
		/// <param name="dr">The desired property row</param>
		/// <returns>The current value as a text string</returns>
		private string GetName(DataRow dr)
		{
			if((dr != null) && (dr[PROPERTIES_COLUMN_NAME] != null))
				return dr[PROPERTIES_COLUMN_NAME].ToString();
			else
				return "";
		
		}// private string GetName(DataRow dr)
		
		/// <summary>This method gets the enumerated editor assigned to the specified row</summary>
		/// <param name="row">The desired property row</param>
		/// <returns>The enumerated editor identifier</returns>
		private TmaxPropGridEditors GetEditorEnum(UltraGridRow row)
		{
			ITmaxPropGridCtrl		IProperty = null;
			TmaxPropGridEditors		eEditor = TmaxPropGridEditors.None;
			
			try
			{
				//	Get the properties interface for this row
				if(IsPropertiesRow(row) == true)
					IProperty = ((ITmaxPropGridCtrl)(row.Cells[PROPERTIES_COLUMN_INTERFACE].Value));
				
				//	Get the editor enumeration
				if(IProperty != null)
					eEditor = IProperty.GetEditor();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetEditorEnum", Ex);
			}
			
			return eEditor;
			
		}// private TmaxPropGridEditors GetEditorEnum(UltraGridRow row)
		
		/// <summary>This method gets the editor object for the specified row</summary>
		/// <param name="row">The desired property row</param>
		/// <returns>The editor suitable for the specified row</returns>
		private EmbeddableEditorBase GetEditor(UltraGridRow row)
		{
			try
			{
				return GetEditor(GetEditorEnum(row));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetEditor", Ex);
			}
			
			return null;
			
		}// private EmbeddableEditorBase GetEditor(UltraGridRow row)
		
		/// <summary>This method gets the editor object for the specified property editor type</summary>
		/// <param name="row">The enumerated property grid editor type</param>
		/// <returns>The editor associated with the specified type</returns>
		private EmbeddableEditorBase GetEditor(TmaxPropGridEditors eEditor)
		{
			EmbeddableEditorBase eeb = null;
			
			switch(eEditor)
			{
				case TmaxPropGridEditors.Double:
							
					eeb = m_aEditors[PROP_EDITOR_DOUBLE];
					break;
								
				case TmaxPropGridEditors.Integer:
							
					eeb = m_aEditors[PROP_EDITOR_INTEGER];
					break;
								
				case TmaxPropGridEditors.Date:
							
					eeb = m_aEditors[PROP_EDITOR_DATE];
					break;
								
				case TmaxPropGridEditors.Boolean:
							
					eeb = m_aEditors[PROP_EDITOR_BOOLEAN];
					break;
								
				case TmaxPropGridEditors.Text:
				case TmaxPropGridEditors.None:
							
					eeb = m_aEditors[PROP_EDITOR_TEXT];
					break;
								
				case TmaxPropGridEditors.Custom:
							
					eeb = m_aEditors[PROP_EDITOR_TEXT_WITH_BUTTON];
					break;
								
				case TmaxPropGridEditors.DropList:
						
					eeb = m_aEditors[PROP_EDITOR_DROP_LIST];
					break;
								
				case TmaxPropGridEditors.Combobox:
							
					eeb = m_aEditors[PROP_EDITOR_COMBO_BOX];
					break;
								
				case TmaxPropGridEditors.MultiLevel:
							
					eeb = m_aEditors[PROP_EDITOR_MULTI_LEVEL];
					break;
								
				case TmaxPropGridEditors.Memo:
				default:
							
					break;

			}// switch(eEditor)

			return eeb;
			
		}// private EmbeddableEditorBase GetEditor(TmaxPropGridEditors eEditor)
		
		/// <summary>This method gets the interface object associated with the specified row</summary>
		/// <param name="row">The desired property row</param>
		/// <returns>The bounded interface object</returns>
		private ITmaxPropGridCtrl GetInterface(UltraGridRow row)
		{
			ITmaxPropGridCtrl IProperty = null;
			
			try
			{
				//	Get the properties interface for this row
				if(IsPropertiesRow(row) == true)
					IProperty = ((ITmaxPropGridCtrl)(row.Cells[PROPERTIES_COLUMN_INTERFACE].Value));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetInterface", row.Cells[PROPERTIES_COLUMN_VALUE].Text + " -> Ex: " + Ex.ToString());
			}
			
			return IProperty;
			
		}// private ITmaxPropGridCtrl GetInterface(UltraGridRow row)
		
		/// <summary>This method fires the control's BeforeUpdate event</summary>
		/// <param name="cell">The cell being updated</param>
		/// <param name="newValue">The new value being assigned to the property</param>
		/// <param name="tmaxPickItem">the pick item required for multi-level updates</param>
		///	<returns>The arguements passed with the event</returns>
		private CTmaxPropGridArgs FireBeforeUpdate(UltraGridCell cell, object newValue, CTmaxPickItem tmaxPickItem, bool bSuppressAddConfirmation)
		{
			CTmaxPropGridArgs Args = null;
			
			Debug.Assert(cell != null);
			if(cell == null) return null;

			try
			{
				Debug.Assert(cell.Row != null);
				if(cell.Row == null) return null;
		
				//	Allocate and intialize the arguments for the event
				if((Args = GetArgs(cell.Row)) == null) return null;
				
				//	Assign the values required to set the new value
				if(newValue != null)
					Args.Value = newValue.ToString();
				
				Args.PickList = tmaxPickItem;
				Args.SupressConfirmations = bSuppressAddConfirmation;
					
				//	Fire the event
				if(BeforeUpdate != null)
					BeforeUpdate(this, Args);
				
				return Args;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireBeforeUpdate", m_tmaxErrorBuilder.Message(ERROR_FIRE_BEFORE_UPDATE_EX), Ex);
				return null;
			}
			
		}// private CTmaxPropGridArgs FireBeforeUpdate(UltraGridRow row)

		/// <summary>This method fires the control's ClickEditorButton event</summary>
		/// <param name="cell">The cell being updated</param>
		///	<returns>The arguements passed with the event</returns>
		private CTmaxPropGridArgs FireClickEditorButton(UltraGridCell cell)
		{
			CTmaxPropGridArgs Args = null;
			
			Debug.Assert(cell != null);
			if(cell == null) return null;

			try
			{
				Debug.Assert(cell.Row != null);
				if(cell.Row == null) return null;
			
				//	Allocate and intialize the arguments for the event
				if((Args = GetArgs(cell.Row)) == null) return null;
				
				//	Fire the event
				if(ClickEditorButton != null)
					ClickEditorButton(this, Args);
				
				return Args;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireClickEditorButton", m_tmaxErrorBuilder.Message(ERROR_FIRE_BEFORE_UPDATE_EX), Ex);
				return null;
			}
			
		}// private CTmaxPropGridArgs FireClickEditorButton(UltraGridRow row)

		/// <summary>This method fires the control's AfterUpdate event</summary>
		/// <param name="cell">The cell that has been updated</param>
		///	<returns>The arguements passed with the event</returns>
		private CTmaxPropGridArgs FireAfterUpdate(UltraGridCell cell)
		{
			CTmaxPropGridArgs Args = null;
			
			Debug.Assert(cell != null);
			if(cell == null) return null;
			
			try
			{
				//	Get the row that owns this cell
				Debug.Assert(cell.Row != null);
				if(cell.Row == null) return null;

				if((Args = GetArgs(cell.Row)) == null) return null;
				
				//	Override the default value
				if(cell.OriginalValue != null)
					Args.Value = cell.OriginalValue.ToString();
				else
					Args.Value = "";
				
				//	Fire the event
				if(AfterUpdate != null)
					AfterUpdate(this, Args);
				
				return Args;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireAfterUpdate", m_tmaxErrorBuilder.Message(ERROR_FIRE_AFTER_UPDATE_EX), Ex);
				return null;
			}
			
		}// private CTmaxPropGridArgs FireAfterUpdate(UltraGridRow row)

		/// <summary>This method fires the control's AddKeyPress event</summary>
		/// <param name="cell">The cell that has been updated</param>
		///	<returns>The arguements passed with the event</returns>
		private CTmaxPropGridArgs FireAddKeyPress()
		{
			UltraGridRow		row = null;
			CTmaxPropGridArgs	Args = null;
			ITmaxPropGridCtrl	tmaxSelection = null;

			try
			{
				//	Don't bother if nobody is interested
				if(AddKeyPress == null) return null;
				
				//	Must have a selection
				if((tmaxSelection = GetSelection(true)) != null)
				{
					//	Get the row that owns this property
					if((row = GetPropertyRow(tmaxSelection)) != null)
					{
						//	Get the arguments for the event
						if((Args = GetArgs(row)) != null)
						{
							m_tmaxEventSource.FireDiagnostic(this, "FireAddPress", "Fired AddKeyPress -> " + tmaxSelection.GetName());

							//	Fire the event
							AddKeyPress(this, Args);
						}

					}// if((row = GetPropertyRow(tmaxSelection)) != null)

				}
				else
				{
					m_tmaxEventSource.FireDiagnostic(this, "FireAddPress", "AddPress NOT Fired - NO SELECTION");
				
				}// if((tmaxSelection = GetSelection()) != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireAfterUpdate", m_tmaxErrorBuilder.Message(ERROR_FIRE_AFTER_UPDATE_EX), Ex);
				Args = null;
			}
			
			return Args;

		}// private CTmaxPropGridArgs FireAddKeyPress()

		/// <summary>This method fires the control's AfterUpdate event</summary>
		/// <param name="IProperty">The property that we need the drop list for</param>
		///	<returns>The arguements passed with the event</returns>
		private ICollection FireGetDropListValues(ITmaxPropGridCtrl IProperty)
		{
			CTmaxPropGridArgs Args = null;

			Debug.Assert(IProperty != null);
			if(IProperty == null) return null;

			try
			{
				//	Allocate and intialize the arguments for the event
				Args = new CTmaxPropGridArgs();
				Args.IProperty = IProperty;

				//	Fire the event
				if(GetDropListValues != null)
					GetDropListValues(this, Args);

				return Args.IDropListValues;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireAfterUpdate", m_tmaxErrorBuilder.Message(ERROR_FIRE_AFTER_UPDATE_EX), Ex);
				return null;
			}

		}// private ICollection FireGetDropListValues(UltraGridRow row)

		/// <summary>This method allocates and initializes an event argument object using the specified row</summary>
		/// <param name="row">The property row to use to initialize the objec</param>
		///	<returns>The arguements to be passed with the event</returns>
		private CTmaxPropGridArgs GetArgs(UltraGridRow row)
		{
			CTmaxPropGridArgs Args = null;
			
			Debug.Assert(row != null);
			if(row == null) return null;
			
			try
			{
				//	Allocate and intialize the arguments for the event
				Args = new CTmaxPropGridArgs();
				
				Args.Cancel = false;
				Args.IProperty = (ITmaxPropGridCtrl)(row.Cells[PROPERTIES_COLUMN_INTERFACE].Value);
				Args.Id = (long)(row.Cells[PROPERTIES_COLUMN_ID].Value);
				Args.Tag = row.Cells[PROPERTIES_COLUMN_TAG].Value;
				Args.Value = GetValue(row);
				
				return Args;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetArgs", m_tmaxErrorBuilder.Message(ERROR_GET_ARGS_EX), Ex);
				return null;
			}
			
		}// private CTmaxPropGridArgs GetArgs(UltraGridRow row)

		/// <summary>This method is called to get the specified row from the Properties table</summary>
		/// <param name="lId">The Id of the desired row</param>
		///	<returns>The row object with the specified Id</returns>
		private System.Data.DataRow GetSourceRow(long lId)
		{
			if(m_dtProperties == null) return null;
			if(m_dtProperties.Rows == null) return null;
			if(m_dtProperties.Rows.Count == 0) return null;
			
			foreach(DataRow O in m_dtProperties.Rows)
			{
				if((long)(O[PROPERTIES_COLUMN_ID]) == lId)
					return O;
			}
			
			return null;
						
		}// private System.Data.DataRow GetSourceRow(long lId)
	
		/// <summary>This method is called to get the specified row from the Properties table</summary>
		/// <param name="IProperty">The Interface of the desired row</param>
		///	<returns>The row object with the specified Id</returns>
		private System.Data.DataRow GetSourceRow(ITmaxPropGridCtrl IProperty)
		{
			if(m_dtProperties == null) return null;
			if(m_dtProperties.Rows == null) return null;
			if(m_dtProperties.Rows.Count == 0) return null;
			
			foreach(DataRow O in m_dtProperties.Rows)
			{
				if(ReferenceEquals(O[PROPERTIES_COLUMN_INTERFACE], IProperty) == true)
					return O;
			}
			
			return null;
						
		}// private System.Data.DataRow GetSourceRow(ITmaxPropGridCtrl IProperty)
	
		/// <summary>This method is called to update the columns of the specified data row</summary>
		/// <param name="dr">The data row to be updated</param>
		/// <param name="IProperty">The interface to the property bound to the row</param>
		/// <returns>true if successful</returns>
		private bool SetColumns(DataRow dr, ITmaxPropGridCtrl IProperty)
		{
			int iCategory = -1;
			
			Debug.Assert(dr != null);
			if(dr == null) return false;
			
			Debug.Assert(IProperty != null);
			if(IProperty == null) return false;
			
			Debug.Assert(m_dtProperties != null);
			if(m_dtProperties == null) return false;
			
			//	Get the category id for this property
			iCategory = GetCategoryId(IProperty);
			if((iCategory < 0) && (this.HasCategories == true))
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_CATEGORY_NOT_FOUND, IProperty.GetName()));
				return false;
			}
			
			try
			{
				//	Set the column values
				dr[PROPERTIES_COLUMN_CATEGORY]  = iCategory;
				dr[PROPERTIES_COLUMN_ID]		= IProperty.GetId();
				dr[PROPERTIES_COLUMN_NAME]		= IProperty.GetName();
				dr[PROPERTIES_COLUMN_VALUE]		= IProperty.GetValue();
				dr[PROPERTIES_COLUMN_TAG]		= IProperty.GetTag();
				dr[PROPERTIES_COLUMN_INTERFACE]	= IProperty;
	
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetColumns", m_tmaxErrorBuilder.Message(ERROR_SET_COLUMNS_EX, IProperty.GetName()), Ex);
				return false;
			}
		
		}// private bool SetColumns(DataRow dr, ITmaxPropGridCtrl IProperty)
		
		/// <summary>This method is called to get the collection of selected properties</summary>
		/// <param name="aSelected">The collection in which to store the properties</param>
		/// <param name="rows">The collection of rows to be searched</param>
		/// <param name="bIncludeEditMode">True to consider the row selected if in edit mode</param>
		/// <returns>The total number of selected properties</returns>
		private int GetSelected(ArrayList aSelected, RowsCollection rows, bool bIncludeEditMode)
		{
			if(m_ctrlUltraGrid == null) return 0;
			if(m_ctrlUltraGrid.IsDisposed == true) return 0;
			if(m_ctrlUltraGrid.Rows == null) return 0;
			if(m_ctrlUltraGrid.Rows.Count == 0) return 0;
			if(m_dtProperties == null) return 0;
			if(m_dtProperties.Rows == null) return 0;
			if(m_dtProperties.Rows.Count == 0) return 0;
			
			try
			{
				//	Do we need to allocate a collection?
				if(aSelected == null)
					aSelected = new ArrayList();
				else
					aSelected.Clear();
					
				//	Should we use the root collection?
				if(rows == null)
					rows = m_ctrlUltraGrid.Rows;
					
				foreach(UltraGridRow O in rows)
				{
					//	Is this a property row?
					if(IsPropertiesRow(O) == true)
					{
						//	Is this row selected?
						if(IsSelected(O, bIncludeEditMode) == true)
							aSelected.Add(O.Cells[PROPERTIES_COLUMN_INTERFACE].Value);
					}
					else
					{
						//	Does this row have any child rows
						if((O.ChildBands != null) && (O.ChildBands.Count > 0))
						{
							foreach(UltraGridChildBand childBand in O.ChildBands)
							{
								if(childBand.Rows != null)
									GetSelected(aSelected, childBand.Rows, bIncludeEditMode);
							}
							
						}// if((O.ChildBands != null) && (O.ChildBands.Count > 0)
						
					}// if(O.Cells.Exists(PROPERTIES_COLUMN_INTERFACE) == true)

				}// foreach(UltraGridRow O in rows)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSelected", m_tmaxErrorBuilder.Message(ERROR_GET_SELECTED_EX), Ex);
			}
			
			if(aSelected != null)
				return aSelected.Count;
			else
				return 0;
			
		}// private int GetSelected(ArrayList aSelected, RowCollection rows)
		
		/// <summary>This method is called to get the row that is in edit mode</summary>
		/// <param name="rows">The collection of rows to be searched</param>
		/// <returns>The row that is in edit mode</returns>
		private UltraGridRow GetEditorRow(RowsCollection rows)
		{
			UltraGridRow editRow = null;
			
			if(m_ctrlUltraGrid == null) return null;
			if(m_ctrlUltraGrid.IsDisposed == true) return null;
			if(m_ctrlUltraGrid.Rows == null) return null;
			if(m_ctrlUltraGrid.Rows.Count == 0) return null;
			if(m_dtProperties == null) return null;
			if(m_dtProperties.Rows == null) return null;
			if(m_dtProperties.Rows.Count == 0) return null;
			
			try
			{
				//	Should we use the root collection?
				if(rows == null)
					rows = m_ctrlUltraGrid.Rows;
					
				foreach(UltraGridRow O in rows)
				{
					//	Is this a property row?
					if(IsPropertiesRow(O) == true)
					{
						//	Is this row in edit mode?
						if(O.Cells[PROPERTIES_COLUMN_VALUE].IsInEditMode == true)
							return O;
					}
					else
					{
						//	Does this row have any child rows
						if((O.ChildBands != null) && (O.ChildBands.Count > 0))
						{
							foreach(UltraGridChildBand childBand in O.ChildBands)
							{
								if(childBand.Rows != null)
									if((editRow = GetEditorRow(childBand.Rows)) != null)
										return editRow;
							}
							
						}// if((O.ChildBands != null) && (O.ChildBands.Count > 0)
						
					}// if(IsPropertiesRow(O) == true)

				}// foreach(UltraGridRow O in rows)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetEditorRow", m_tmaxErrorBuilder.Message(ERROR_GET_EDITOR_ROW_EX), Ex);
			}
			
			return null; // Not found
			
		}// private UltraGridRow GetEditorRow(RowsCollection rows)
		
		/// <summary>This method is called to fill the value list using the specified property interface</summary>
		/// <param name="valueList">The value list to be populated</param>
		/// <param name="IProperty">The interface to the property bound to the row</param>
		/// <param name="strSelection">The value to be selected in the list</param>
		/// <returns>true if successful</returns>
		private bool FillEditor(ValueList valueList, ITmaxPropGridCtrl IProperty, string strSelection)
		{
			ICollection IValues = null;
			bool		bSuccessful = false;
			object		objSelection = null;
			
			Debug.Assert(valueList != null);
			if(valueList == null) return false;
			Debug.Assert(valueList.ValueListItems != null);
			if(valueList.ValueListItems == null) return false;
			
			Debug.Assert(IProperty != null);
			if(IProperty == null) return false;
			
			Debug.Assert(m_dtProperties != null);
			if(m_dtProperties == null) return false;
			
			try
			{
				//	Clear the existing values
				valueList.ResetValueListItems();
				
				//	First try to get the values from an attached object
				if(GetDropListValues != null)
					IValues = FireGetDropListValues(IProperty);
				if(IValues == null)
					IValues = IProperty.GetDropListValues();
				
				if(IValues != null)
				{
					//	Add each of the values to the list
					foreach(object O in IValues)
					{
						valueList.ValueListItems.Add(O, O.ToString());
						
						if((objSelection == null) && (strSelection != null) && (strSelection.Length > 0))
						{
							if(String.Compare(O.ToString(), strSelection, false) == 0)
							{
								objSelection = O;
							}
						
						}// if((objSelection == null) && (strSelection != null) && (strSelection.Length > 0))
						
					}// foreach(object O in IValues)
				
					if(objSelection != null)
					{
						int iIndex = valueList.ValueListItems.IndexOf(objSelection);
						if(iIndex >= 0)
							valueList.SelectedIndex = iIndex;
					}
				
					bSuccessful = true;
				}
				
				try
				{
					//	Size the drop list
					if(valueList.ValueListItems.Count <= MINIMUM_DROP_LIST_ITEMS)
						valueList.MaxDropDownItems = MINIMUM_DROP_LIST_ITEMS;
					else
						valueList.MaxDropDownItems = valueList.ValueListItems.Count;
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "FillEditor", Ex);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillEditor", m_tmaxErrorBuilder.Message(ERROR_FILL_EDITOR_EX, IProperty.GetName()), Ex);
			}
			
			return bSuccessful;
		
		}// private bool FillEditor(ValueList valueList, ITmaxPropGridCtrl IProperty, string strSelection)
		
		/// <summary>This method is called to fill the value list for the specified row</summary>
		/// <param name="valueList">The value list to be populated</param>
		/// <param name="row">The row that owns the value list</param>
		/// <param name="strSelection">The value to be selected in the list</param>
		/// <returns>true if successful</returns>
		private bool FillEditor(ValueList valueList, UltraGridRow row, string strSelection)
		{
			ITmaxPropGridCtrl IProperty = null;
			
			Debug.Assert(valueList != null);
			if(valueList == null) return false;
			Debug.Assert(valueList.ValueListItems != null);
			if(valueList.ValueListItems == null) return false;
			
			Debug.Assert(row != null);
			if(row == null) return false;
			
			if((IProperty = (ITmaxPropGridCtrl)(row.Cells[PROPERTIES_COLUMN_INTERFACE].Value)) != null)
				return FillEditor(valueList, IProperty, strSelection);
			else
				return false;
		
		}// private bool FillEditor(ValueList valueList, UltraGridRow row, string strSelection)
		
		/// <summary>This method is called to determine if the specified property row is selected</summary>
		/// <param name="aSelected">The row being checked</param>
		/// <param name="bIncludeEditMode">True to consider the row selected if in edit mode</param>
		/// <returns>true if selected</returns>
		private bool IsSelected(UltraGridRow row, bool bIncludeEditMode)
		{
			bool bSelected = false;
			
			if(m_ctrlUltraGrid == null) return false;
			if(m_ctrlUltraGrid.IsDisposed == true) return false;
			if(m_ctrlUltraGrid.Rows == null) return false;
			if(m_ctrlUltraGrid.Rows.Count == 0) return false;
			if(m_dtProperties == null) return false;
			if(m_dtProperties.Rows == null) return false;
			if(m_dtProperties.Rows.Count == 0) return false;
			if(row == null) return false;
			
			try
			{
				//	Is this a property row?
				if(IsPropertiesRow(row) == true)
				{
					//	Is this row selected?
					if(row.Selected == true)
						bSelected = true;
					else if(row.Cells[PROPERTIES_COLUMN_NAME].Selected == true)
						bSelected = true;
					else if(row.Cells[PROPERTIES_COLUMN_VALUE].Selected == true)
						bSelected = true;
					else if((bIncludeEditMode == true) && (row.Cells[PROPERTIES_COLUMN_VALUE].IsInEditMode == true))
						bSelected = true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "IsSelected", Ex);
			}
			
			return bSelected;
			
		}// private bool IsSelected(UltraGridRow row)
		
		/// <summary>This method will delete the specified row in the Properties table</summary>
		/// <param name="dr">The data row to be deleted</param>
		/// <returns>true if successful</returns>
		private bool Delete(DataRow dr)
		{
			try
			{
				m_ctrlUltraGrid.PerformAction(UltraGridAction.ExitEditMode);
				dr.Delete();
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Delete", m_tmaxErrorBuilder.Message(ERROR_DELETE_DATA_ROW_EX), Ex);
				return false;
			}			

		}// private bool Delete(DataRow dr)
		
		/// <summary>This method will delete the specified row in the grid</summary>
		/// <param name="row">The row to be deleted</param>
		/// <returns>true if successful</returns>
		private bool Delete(UltraGridRow row)
		{
			try
			{
				m_ctrlUltraGrid.PerformAction(UltraGridAction.ExitEditMode);
				row.Delete(false);
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Delete", m_tmaxErrorBuilder.Message(ERROR_DELETE_GRID_ROW_EX), Ex);
				return false;
			}			

		}// private bool Delete(UltraGridRow row)
		
		/// <summary>This method is called to initialize the array of editors</summary>
		/// <returns>true if successful</returns>
		private bool CreateEditors()
		{
			DefaultEditorOwnerSettings	settings = null;
			CTmaxEditorWithText			tmaxTextEditor = null;
			CTmaxEditorWithCombo		tmaxComboEditor = null;
			CTmaxEditorWithNumbers		tmaxNumericEditor = null;
			EditorButton				customButton = null;
						
			Debug.Assert(m_aEditors != null);
			if(m_aEditors == null) return false;			
	
			try
			{
				// Add the editor for floating point values
				settings = new DefaultEditorOwnerSettings();
				settings.DataType = typeof(double);
				tmaxNumericEditor = new CTmaxEditorWithNumbers(new DefaultEditorOwner(settings));
				tmaxNumericEditor.AllowDecimal = true;
				m_aEditors[PROP_EDITOR_DOUBLE] = tmaxNumericEditor;			
				
				// Add the editor for integer values
				settings = new DefaultEditorOwnerSettings();
				settings.DataType = typeof(int);
				tmaxNumericEditor = new CTmaxEditorWithNumbers(new DefaultEditorOwner(settings));
				tmaxNumericEditor.AllowDecimal = false;
				m_aEditors[PROP_EDITOR_INTEGER] = tmaxNumericEditor;			

				// Add the editor for date values
				settings = new DefaultEditorOwnerSettings( );
				settings.DataType = typeof(DateTime);
				settings.MaskInput = "mm/dd/yyyy";
				m_aEditors[PROP_EDITOR_DATE] = new DateTimeEditor(new DefaultEditorOwner(settings) );
				
				// Add the editor for boolean values
				if(m_bDropListBoolean == false)
				{
					settings = new DefaultEditorOwnerSettings();
					settings.DataType = typeof(bool);
					m_aEditors[PROP_EDITOR_BOOLEAN] = new CheckEditor(new DefaultEditorOwner(settings));
					((CheckEditor)m_aEditors[PROP_EDITOR_BOOLEAN]).ThreeState = true;
				
				}
				else
				{
					settings = new DefaultEditorOwnerSettings( );
		
					ValueList valueList = new ValueList( );
					valueList.ValueListItems.Add( "", "Unassigned" );
					valueList.ValueListItems.Add( "true", "True" );
					valueList.ValueListItems.Add( "false", "False" );
					settings.ValueList = valueList;
					settings.DataType = typeof(string);
					m_aEditors[PROP_EDITOR_BOOLEAN] = new EditorWithCombo( new DefaultEditorOwner( settings ) );
				}
				
				// Add the editor for max length text values (non-memo)
				settings = new DefaultEditorOwnerSettings();
				settings.DataType = typeof(string);
				settings.MaxLength = 255;
				tmaxTextEditor = new CTmaxEditorWithText(new DefaultEditorOwner(settings));
				m_tmaxEventSource.Attach(tmaxTextEditor.EventSource);
				tmaxTextEditor.EventSource.Name = "Single-line Editor";
				tmaxTextEditor.ReadOnly = false;
				tmaxTextEditor.MultiLine = false;
				m_aEditors[PROP_EDITOR_TEXT] = tmaxTextEditor;
				
				// Add the editor for custom property editing
				settings = new DefaultEditorOwnerSettings();
				settings.DataType = typeof(string);
				settings.MaxLength = 255;
				customButton = new EditorButton(CUSTOM_BUTTON_KEY);
				customButton.Appearance.Image = 1;
				tmaxTextEditor = new CTmaxEditorWithText(new DefaultEditorOwner(settings) );
				tmaxTextEditor.EventSource.Name = "Custom Text Editor";
				tmaxTextEditor.ReadOnly = false;
				tmaxTextEditor.MultiLine = false;
				
				tmaxTextEditor.ButtonsRight.Add(customButton);
				tmaxTextEditor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.OnClickEditorButton);

				m_aEditors[PROP_EDITOR_TEXT_WITH_BUTTON] = tmaxTextEditor;

				settings = new DefaultEditorOwnerSettings( );
				settings.ValueList = new ValueList();
				settings.DataType = typeof(string);
				tmaxComboEditor = new CTmaxEditorWithCombo(new DefaultEditorOwner(settings));
				tmaxComboEditor.TextBox.ReadOnly = true;
				tmaxComboEditor.TextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
				m_aEditors[PROP_EDITOR_DROP_LIST] = tmaxComboEditor;
				
				settings = new DefaultEditorOwnerSettings( );
				settings.ValueList = new ValueList();
				settings.DataType = typeof(string);
				tmaxComboEditor = new CTmaxEditorWithCombo(new DefaultEditorOwner(settings));
				tmaxComboEditor.TextBox.ReadOnly = false;
				tmaxComboEditor.TextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
				m_aEditors[PROP_EDITOR_COMBO_BOX] = tmaxComboEditor;
				
				// Add the editor for multi-level pick list codes
				settings = new DefaultEditorOwnerSettings();
				settings.DataType = typeof(string);
				settings.MaxLength = 255;
			
				tmaxTextEditor = new CTmaxEditorWithText(new DefaultEditorOwner(settings) );
				tmaxTextEditor.EventSource.Name = "Multi-Level Pick List Editor";
				tmaxTextEditor.ReadOnly = true;
				tmaxTextEditor.MultiLine = false;
				m_aEditors[PROP_EDITOR_MULTI_LEVEL] = tmaxTextEditor;
				
				//	Create the drop button to trigger the custom multi-level editor
				m_multiLevelButton = new Infragistics.Win.UltraWinEditors.DropDownEditorButton();
				m_multiLevelButton.Control = this.m_ctrlMultiLevelEditor;
				m_multiLevelButton.BeforeDropDown += new Infragistics.Win.UltraWinEditors.BeforeEditorButtonDropDownEventHandler(this.OnBeforeDropMultiLevel);
				m_multiLevelButton.AfterCloseUp += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.OnAfterCloseMultiLevel);
				tmaxTextEditor.ButtonsRight.Add(m_multiLevelButton); //add the button to the editor control
				
				//	Trap the event fired by the multi-level editor when the user finishes
				m_ctrlMultiLevelEditor.Finished += new System.EventHandler(this.OnMultiLevelFinished);
				
				//	Attach event handlers
				foreach(EmbeddableEditorBase O in m_aEditors)
				{
					O.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEditorKeyDownX);
					O.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnEditorKeyPress);
				}
				
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateEditors", m_tmaxErrorBuilder.Message(ERROR_CREATE_EDITORS_EX), Ex);
				return false;
			}
		
		}// private bool CreateEditors()
		
		/// <summary>This method handles events fired by the grid before it enters EDIT mode</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnBeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
		{
			EditorWithCombo		comboBox = null;
			TmaxPropGridEditors	eEditor = TmaxPropGridEditors.None;
			
			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnBeforeEnterEditMode", "Before Enter Edit");

				//	Reset the members used to track the value when the user hits Enter to close a drop list
				m_iComboEditorIndex = -1;
				m_iComboEditorSelection = -1;
				
				//	Make sure the text editor does not accept multiline
				//
				//	NOTE:	This works around an apparent bug in which the editor ALWAYS
				//			accepts CR/LF when CellMultiline is TRUE. Even if a custom 
				//			editor is attached to the cell
				if(m_aEditors[PROP_EDITOR_TEXT] != null)
					((EditorWithText)(m_aEditors[PROP_EDITOR_TEXT])).AcceptsReturn = false;
				if(m_aEditors[PROP_EDITOR_TEXT_WITH_BUTTON] != null)
					((EditorWithText)(m_aEditors[PROP_EDITOR_TEXT_WITH_BUTTON])).AcceptsReturn = false;
				if(m_aEditors[PROP_EDITOR_COMBO_BOX] != null)
					((EditorWithCombo)(m_aEditors[PROP_EDITOR_COMBO_BOX])).AcceptsReturn = false;
				if(m_aEditors[PROP_EDITOR_DROP_LIST] != null)
					((EditorWithCombo)(m_aEditors[PROP_EDITOR_DROP_LIST])).AcceptsReturn = false;
				if(m_aEditors[PROP_EDITOR_BOOLEAN] != null)
					((EditorWithCombo)(m_aEditors[PROP_EDITOR_BOOLEAN])).AcceptsReturn = false;
				if(m_aEditors[PROP_EDITOR_MULTI_LEVEL] != null)
				{
					((EditorWithText)(m_aEditors[PROP_EDITOR_MULTI_LEVEL])).AcceptsReturn = false;
				}
			
				if((m_ctrlUltraGrid.ActiveCell != null) && (m_ctrlUltraGrid.ActiveCell.Row != null))
				{
					//	Which editor is in use for this cell?
					eEditor = GetEditorEnum(m_ctrlUltraGrid.ActiveCell.Row);
					switch(eEditor)
					{
						case TmaxPropGridEditors.Combobox:
						case TmaxPropGridEditors.DropList:
						case TmaxPropGridEditors.Boolean:
						
							//---------------------------------------------------------------------------
							//	See description of INFRAGISTICS_MULTILINE_BUG_ENABLE_AUTOCOMPLETE
							//---------------------------------------------------------------------------
							if(INFRAGISTICS_MULTILINE_BUG_ENABLE_AUTOCOMPLETE == true)
								SetMultilineEnabled(false);
							
							if(eEditor != TmaxPropGridEditors.Boolean)
							{
								try { comboBox = (EditorWithCombo)(m_ctrlUltraGrid.ActiveCell.Editor); }
								catch {}
							}
							break;
						
					}// switch(eEditor)
				
					if((comboBox != null) && (comboBox.ValueList != null))
					{
						FillEditor((ValueList)(comboBox.ValueList), m_ctrlUltraGrid.ActiveCell.Row, m_ctrlUltraGrid.ActiveCell.Text);
						comboBox.TextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
						//m_tmaxEventSource.FireDiagnostic(this, "OnBeforeEnterEdit", "Filled: " + comboBox.ValueList.ItemCount.ToString());
					}
				
				}// if((e.Cell != null) && (e.Cell.Row != null))
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnBeforeEnterEditMode", Ex);
			}
						
		}// private void OnBeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)

		/// <summary>This method handles events fired by the grid when the user clicks on an editor button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickEditorButton(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnClickEditorButton", "Click Editor Button");

			//	The context provided with the event should be a cell in the grid
			if(e.Context != null)
			{
				try
				{
					UltraGridCell cell = (UltraGridCell)(e.Context);
					FireClickEditorButton(cell);
					
					//	This forces the text in the cell to update if it
					//	was changed by the custom editor
					m_ctrlUltraGrid.PerformAction(UltraGridAction.ExitEditMode);				
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "OnClickEditorButton", Ex);
				}
				
			}// if(e.Context != null)

		}// private void OnClickEditorButton(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)

		/// <summary>This method handles events fired by the grid when the user clicks on an editor button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnClickCellButton", "Click Cell Button");

			if ((e.Cell != null) && (e.Cell.Row != null))
				SendToClipboard(e.Cell.Row);

		}// private void OnClickCellButton(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)

		/// <summary>This method handles events fired by the grid before it creates an autosizing edit window</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnBeforeAutoSizeEdit(object sender, Infragistics.Win.UltraWinGrid.CancelableAutoSizeEditEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnBeforeAutoSizeEdit", "Before AutoSize Edit");

			if (m_ctrlUltraGrid.ActiveCell != null)
			{
				//	Cancel the autosize for everything except memo
				if(GetEditorEnum(m_ctrlUltraGrid.ActiveCell.Row) != TmaxPropGridEditors.Memo)
					e.Cancel = true;

			}// if(m_ctrlUltraGrid.ActiveCell != null)
		
		}// private void OnBeforeAutoSizeEdit(object sender, Infragistics.Win.UltraWinGrid.CancelableAutoSizeEditEventArgs e)

		/// <summary>This method handles MouseDown events fired by the grid control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnUltraMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			UltraGridCell cell = null;

			//m_tmaxEventSource.FireDiagnostic(this, "OnUltraMouseDown", "Ultra Mouse Down");

			//	Bubble the event
			//
			//	NOTE:	This is done BEFORE changing the Selected state so that the
			//			owner can retrieve selected text before taking the row out of edit mode
			try 
			{ 
				if(GridMouseDown != null)
					GridMouseDown(this, e);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnUltraMouseDown", Ex); 
			}

			//	Is this the right mouse button?
			if(e.Button == MouseButtons.Right)
			{
				if((cell = GetCell(e.X, e.Y)) != null)
				{
					//	Select this row if it's a property row
					if(IsPropertiesRow(cell.Row) == true)
					{
						if(IsSelected(cell.Row, false) == false)
						{
							m_ctrlUltraGrid.Selected.Rows.Clear();
							cell.Row.Selected = true;
						}
							
					}// if(IsPropertiesRow(cell.Row) == true)
					
				}// if(cell != null)
				
			}// if(e.Button = MouseButtons.Right)	

		}

		/// <summary>This method handles MouseUp events fired by the grid control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnUltraMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnUltraMouseUp", "Ultra Mouse Up");

			//	Bubble the event
			try
			{
				if(GridMouseUp != null)
					GridMouseUp(this, e);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnMouseUp", Ex);
			}

		}// private void OnUltraMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This method handles DoubleClick events fired by the grid control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnUltraDoubleClick(object sender, System.EventArgs e)
		{
			UltraGridCell	cell = null;

			//m_tmaxEventSource.FireDiagnostic(this, "OnUltraMouseDoubleClick", "Ultra Mouse DblClick");

			//	Get the current mouse position and convert to client coordinates
			Point Pos = new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
			Pos = this.PointToClient(Pos);
			
			if((cell = GetCell(Pos.X, Pos.Y)) != null)
				SendToClipboard(cell.Row);

		}// private void OnUltraDoubleClick(object sender, System.EventArgs e)

		/// <summary>This gets the cell at the specified location within the client area</summary>
		/// <param name="iX">The X position in client coordinates</param>
		/// <param name="iY">The Y position in client coordinates</param>
		private UltraGridCell GetCell(int iX, int iY)
		{
			UIElement		uiElement = null;
			UltraGridCell	cell = null;
				
			try
			{
				//	Retrieve the UIElement at the specified location
				uiElement = m_ctrlUltraGrid.DisplayLayout.UIElement.ElementFromPoint(new Point(iX, iY));
			
				//	Use the UI element to retrieve the cell
				if(uiElement != null)
					cell = (UltraGridCell)(uiElement.GetContext(typeof(UltraGridCell)));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetCell", Ex);
			}
			
			return cell;
		
		}// private UltraGridCell GetCell(int iX, int iY)

		/// <summary>This method will send the value assigned to the specified row to the clipboard</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private bool SendToClipboard(UltraGridRow row)
		{
			string			strValue = "";
			bool			bSuccessful = false;
			UltraGridCell	cell = null;
				
			if(row == null) return false;
			
			try
			{
				//	Get the cell containing the value
				if((row != null) && (IsPropertiesRow(row) == true))
					if((cell = row.Cells[PROPERTIES_COLUMN_VALUE]) == null) return false;
					
				//	Is the user editing this cell?
				if(cell.IsInEditMode == true)
				{
					//	Get the selected text (if any)
					if(cell.SelLength > 0)
					{
						try { strValue = cell.SelText; }
						catch {}
					}
				
				}// if(cell.IsInEditMode == true)
						
				//	Get the complete text if nothing is selected
				if(strValue.Length == 0)
					strValue = row.Cells[PROPERTIES_COLUMN_VALUE].Text;
					
				//	Copy the value to the clipboard
				if(strValue.Length > 0)
				{
					System.Windows.Forms.Clipboard.SetDataObject(strValue, false);
					bSuccessful = true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SendToClipboard", Ex);
			}
			
			return bSuccessful;
			
		}// private bool SendToClipboard(UltraGridRow row)

		/// <summary>Called when the drop list for an editor is opened</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnBeforeCellListDropDown(object sender, Infragistics.Win.UltraWinGrid.CancelableCellEventArgs e)
		{
			m_tmaxEventSource.FireDiagnostic(this, "OnBeforeCellListDropDown", "Before Cell List Drop");

			//	Reset the members used to track the value when the user hits Enter to close a drop list
			//
			//	NOTE:	We do this because the Infragistics editor does not update the
			//			text box to match the drop list selection on Enter if the user
			//			changes the selection with the mouse
			m_iComboEditorIndex = -1;
			m_iComboEditorSelection = -1;
			
		}// private void OnBeforeCellListDropDown(object sender, Infragistics.Win.UltraWinGrid.CancelableCellEventArgs e)

		/// <summary>Called when the drop list for an editor is closed</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAfterCellListCloseUp", "Cell List Close Up");

				//	Update the text box if the user closed the list by hitting Enter
				//
				//	NOTE:	We do this because the Infragistics editor does not update the
				//			text box to match the drop list selection on Enter if the user
				//			changes the selection with the mouse
				if((m_iComboEditorIndex >= 0) && (m_iComboEditorSelection >= 0))
				{
					EditorWithCombo comboEditor = (EditorWithCombo)(m_aEditors[m_iComboEditorIndex]);
					
					if((comboEditor != null) && (comboEditor.IsInEditMode == true))
					{
						comboEditor.ValueList.SelectedItemIndex = m_iComboEditorSelection;
					}
				
				}// if((m_iComboEditorIndex >= 0) && (m_strComboEditorText.Length > 0))
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAfterCellListCloseUp", Ex);
			}
			
		}// private void OnAfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)

		/// <summary>Called when the user presses a key in an editor</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnEditorKeyDownX(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnEditorKeyDown", "Editor Key Down -> Key = " + e.KeyCode.ToString());

				//	Did the user hit the Enter key?
				if(e.KeyCode == System.Windows.Forms.Keys.Enter)
				{
					//	Is this one of our combobox style editors?
					//
					//	NOTE:	We do this because the Infragistics editor does not update the
					//			text box to match the drop list selection on Enter if the user
					//			changes the selection with the mouse
					if(ReferenceEquals(m_aEditors[PROP_EDITOR_COMBO_BOX], sender) == true)
						m_iComboEditorIndex = PROP_EDITOR_COMBO_BOX;
					else if(ReferenceEquals(m_aEditors[PROP_EDITOR_DROP_LIST], sender) == true)
						m_iComboEditorIndex = PROP_EDITOR_DROP_LIST;
					else if(ReferenceEquals(m_aEditors[PROP_EDITOR_BOOLEAN], sender) == true)
						m_iComboEditorIndex = PROP_EDITOR_BOOLEAN;
					else
						m_iComboEditorIndex = -1;

					if(m_iComboEditorIndex >= 0)
					{
						EditorWithCombo comboEditor = (EditorWithCombo)(m_ctrlUltraGrid.ActiveCell.Editor);

						if((comboEditor.IsDroppedDown == true) && (comboEditor.ValueList != null))
						{
							m_iComboEditorSelection = comboEditor.ValueList.SelectedItemIndex;

						}// if((comboEditor.IsDroppedDown == true) && (comboEditor.ValueList != null))
						else
						{
							m_iComboEditorIndex = -1;
						}

					}// if(m_iComboEditorIndex >= 0)
				
				}
				else
				{
					//if((CTmaxToolbox.IsControlKeyPressed() == true) && (e.KeyCode != Keys.ControlKey))
					//    m_tmaxEventSource.FireDiagnostic(this, "OnEditorKeyDown", "Editor Key Down -> Key = " + e.KeyCode.ToString());
					
					//	Check to see if this is a hotkey
					//
					//	NOTE:	Turns out we don't have to do this here. The UltraKeyDown gets
					//			fired even if we're in an editor
					//CheckHotkey(e);
				
				}// if(e.KeyCode == System.Windows.Forms.Keys.Enter)
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnEditorKeyDown", Ex);
			}
			
		}// private void OnEditorKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)

		/// <summary>Handles KeyPress events fired by the grid</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnEditorKeyPress(object sender, KeyPressEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnEditorKeyPress", "OnEditorKeyPress -> Key = " + e.KeyChar.ToString() + " : " + System.Convert.ToByte(e.KeyChar).ToString());

			//	Is this the hotkey to add a new property to the grid?
			//
			//	NOTE: 1 is the ASCII code for Control+A (see ASCII table)
			if(e.KeyChar == 1)
			{
				e.Handled = true; // This suppresses the default MessageBeep(0)
			}

		}// private void OnEditorKeyPress(object sender, KeyPressEventArgs e)

		/// <summary>Called when the user presses a key in the grid</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnUltraKeyDown(object sender, KeyEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnUltraKeyDown", "OnUltraKeyDown -> Key = " + e.KeyCode.ToString());
			
			//	Check to see if this is a hotkey
			if((m_dsSource != null) && (m_ctrlUltraGrid.Rows != null))
			{
				//if((CTmaxToolbox.IsControlKeyPressed() == true) && (e.KeyCode != Keys.ControlKey))
				//    m_tmaxEventSource.FireDiagnostic(this, "OnUltraKeyDown", "Ultra Key Down -> Key = " + e.KeyCode.ToString());
					
				CheckHotkey(e);

			}// if((m_dsSource != null) && (m_ctrlUltraGrid.Rows != null))

		}// private void OnUltraKeyDown(object sender, KeyEventArgs e)

		/// <summary>Handles KeyPress events fired by the grid</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnUltraKeyPress(object sender, KeyPressEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnUltraKeyPress", "OnUltraKeyPress -> Key = " + e.KeyChar.ToString() + " : " + System.Convert.ToByte(e.KeyChar).ToString());

			//	Is this the hotkey to add a new property to the grid?
			//
			//	NOTE: 1 is the ASCII code for Control+A (see ASCII table)
			if(e.KeyChar == 1)
			{
				e.Handled = true; // This suppresses the default MessageBeep(0)
			}

		}// private void OnUltraKeyPress(object sender, KeyPressEventArgs e)

		/// <summary>Called to determine if the user key press is a predefined hotkey</summary>
		/// <param name="e">The keyboard event arguments</param>
		private bool CheckHotkey(KeyEventArgs e)
		{
			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "CheckHotkey", "CheckHotKey -> Key = " + e.KeyCode.ToString());

				//	Is the user pressing the control key
				if(CTmaxToolbox.IsControlKeyPressed() == true)
				{
					//	Is this the hotkey to add a new property?
					if(e.KeyCode == PROPGRID_ADD_HOTKEY)
					{
						e.Handled = true;
						FireAddKeyPress();
					}

				}// if(CTmaxToolbox.IsControlKeyPressed() == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "CheckHotkey", Ex);
			}
			
			return e.Handled;

		}// private bool CheckHotkey(KeyEventArgs e)

		/// <summary>Called when the user finishes editing a cell</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAfterExitEditMode(object sender, System.EventArgs e)
		{
			m_tmaxEventSource.FireDiagnostic(this, "OnAfterExitEditMode", "After Exit Edit Mode");

			//---------------------------------------------------------------------------
			//	See description of INFRAGISTICS_MULTILINE_BUG_ENABLE_AUTOCOMPLETE
			//---------------------------------------------------------------------------
			if(INFRAGISTICS_MULTILINE_BUG_ENABLE_AUTOCOMPLETE == true)
				SetMultilineEnabled(true);
		
		}// private void OnAfterExitEditMode(object sender, System.EventArgs e)

		/// <summary>Called before ending edit mode</summary>
		/// <param name="sender">The object that fired the event</param>
		/// <param name="e">The event arguments</param>
		private void OnBeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
		{
			UltraGridRow row = null;

			try
			{
				//	Get the row bound to the specified property
				if((row = GetEditorRow(null)) != null)
				{
					//	Store the editor state before exiting
					m_IEditorProperty = GetEditorProps(row, ref m_strEditorText, ref m_strEditorSelection, ref m_lEditorSelStart);
				}
				else
				{
					//	Clear the values
					m_IEditorProperty = null;
					m_strEditorText = "";
					m_strEditorSelection = "";
					m_lEditorSelStart = -1;

				}// if((row = GetEditorRow()) != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnBeforeExitEditMode", Ex);
			}

		}// private void OnBeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>The underlying Infragistics UltraGrid control</summary>
		public Infragistics.Win.UltraWinGrid.UltraGrid UltraGridCtrl
		{
			get { return m_ctrlUltraGrid; }
		}
		
		/// <summary>The collection of category objects used to initialize the control</summary>
		public System.Collections.ICollection ICategories
		{
			get { return m_ICategories; }
		}
		
		/// <summary>True if current data source uses upper level categories</summary>
		public bool HasCategories
		{
			get { return ((m_dtCategories != null) && (m_dtCategories.Rows != null) && (m_dtCategories.Rows.Count > 0)); }
		}
		
		/// <summary>True to use a drop down list as the boolean editor</summary>
		public bool DropListBoolean
		{
			get { return m_bDropListBoolean; }
			set { m_bDropListBoolean = value; }
		}
		
		/// <summary>Owner supplied identifier for sorting</summary>
		public long SortOn
		{
			get { return m_lSortOn; }
			set { m_lSortOn = value; }
		}
			
		#endregion Properties
		
		#region Nested Classes
		
		/// <summary>This class manages the custom comparer for sorting objects in the grid</summary>
		public class CTmaxPropGridComparer : IComparer
		{
			#region Private Members
			
			/// <summary>Local member bound to SortOn property</summary>
			private long m_lSortOn = 0;

			#endregion Private Members
			
			#region Public Methods
			
			/// <summary>Constructor</summary>
			public CTmaxPropGridComparer()
			{
			}
			
			/// <summary>Constructor</summary>
			/// <param name="lSortOn">Sort mode identifier</param>
			public CTmaxPropGridComparer(long lSortOn)
			{
				m_lSortOn = lSortOn;
			}
			
			/// <summary>This method compares the values in two cells</summary>
			/// <param name="x">The first cell</param>
			/// <param name="y">The second cell</param>
			/// <returns>0 if equal, -1 if x less than y, 1 if x > y</returns>
			int IComparer.Compare(object x, object y)
			{
				int					iReturn = 0;
				UltraGridCell		cellx = null;
				UltraGridCell		celly = null;
				ITmaxPropGridCtrl	Ix = null;
				ITmaxPropGridCtrl	Iy = null;
				
				try
				{
					cellx = (UltraGridCell)x;
					celly = (UltraGridCell)y;
					
					//	Get the interface object stored in each cell
					if(cellx != null)
						Ix = (ITmaxPropGridCtrl)(cellx.Value);
					if(celly != null)
						Iy = (ITmaxPropGridCtrl)(celly.Value);
						
					if((Ix != null) && (Iy != null))
					{
						//	Are these the same objects?
						if(ReferenceEquals(Ix, Iy) == true)
						{
							iReturn = 0;
						}
						else
						{
							iReturn = Ix.Compare(Iy, this.SortOn);
						}
						
					}// if((Ix != null) && (Iy != null))
					
				}
				catch
				{
				}
				
				return iReturn;
				
			}// public int IComparer.Compare(object x, object y)
			
			#endregion Public Methods
			
			#region Properties
			
			/// <summary>Owner supplied identifier for sorting</summary>
			public long SortOn
			{
				get { return m_lSortOn; }
				set { m_lSortOn = value; }
			}
			
			#endregion Properties
			
		}// public class CTmaxPropGridComparer : IComparer
		
		/// <summary>This class is used to encapsulate the arguments for an event fired by this control</summary>
		public class CTmaxPropGridArgs
		{
			#region Private Members
		
			/// <summary>Local class member bound to IProperty property</summary>
			private FTI.Shared.Trialmax.ITmaxPropGridCtrl m_IProperty = null;

			/// <summary>Local class member bound to Value property</summary>
			private string m_strValue = null;
			
			/// <summary>Local class member bound to Tag property</summary>
			private object m_userTag = null;
			
			/// <summary>Local class member bound to Cancel property</summary>
			private bool m_bCancel = false;
			
			/// <summary>Local class member bound to SupressConfirmations property</summary>
			private bool m_bSuppressConfirmations = false;
			
			/// <summary>Local class member bound to Id property</summary>
			private long m_lId = 0;
			
			/// <summary>Local class member bound to PickList property</summary>
			private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickList = null;

			/// <summary>Local class member bound to IDropListValues property</summary>
			private System.Collections.ICollection m_IDropListValues = null;

			#endregion Private Members
		
			#region Public Methods
		
			/// <summary>Constructor</summary>
			public CTmaxPropGridArgs() 
			{
			}

			#endregion Public Methods
		
			#region Properties
		
			/// <summary>The property interface associated with the argument</summary>
			public FTI.Shared.Trialmax.ITmaxPropGridCtrl IProperty
			{
				get { return m_IProperty; }
				set { m_IProperty = value; }
			}

			/// <summary>True to cancel the pending operation</summary>
			public bool Cancel
			{
				get { return m_bCancel; }
				set { m_bCancel = value; }
			}

			/// <summary>The Value related to the event</summary>
			/// <remarks>If event == BeforeUpdate, Value is the new value</remarks>
			/// <remarks>If event == AfterUpdate, Value is the old value</remarks>
			public string Value
			{
				get { return m_strValue; }
				set { m_strValue = value; }
			}

			/// <summary>The user supplied tag fired with the event</summary>
			public object Tag
			{
				get { return m_userTag; }
				set { m_userTag = value; }
			}

			/// <summary>The user supplied id assigned to the object</summary>
			public long Id
			{
				get { return m_lId; }
				set { m_lId = value; }
			}

			/// <summary>The pick list required for MultiLevel objects</summary>
			public FTI.Shared.Trialmax.CTmaxPickItem PickList
			{
				get { return m_tmaxPickList; }
				set { m_tmaxPickList = value; }
			}

			/// <summary>The collection of values used to populate the requested drop list</summary>
			public System.Collections.ICollection IDropListValues
			{
				get { return m_IDropListValues; }
				set { m_IDropListValues = value; }
			}

			/// <summary>True to suppress confirmation forms</summary>
			public bool SupressConfirmations
			{
				get { return m_bSuppressConfirmations; }
				set { m_bSuppressConfirmations = value; }
			}

			#endregion Properties
		
		}// public class CTmaxPropGridArgs
		
		/// <summary>This class is used to encapsulate the arguments for an event fired by this control</summary>
		public class CTmaxEditorWithCombo : EditorWithCombo
		{
			#region Public Methods
		
			/// <summary>Constructor</summary>
			public CTmaxEditorWithCombo(EmbeddableEditorOwnerBase defaultOwner) : base(defaultOwner)
			{
			}

			#endregion Public Methods
		
			#region Protected Methods
		
			protected override void InternalOnEmbeddableTextBoxListenerKeyDown(KeyEventArgs e)
			{
			
base.InternalOnEmbeddableTextBoxListenerKeyDown (e);
return;
/*
				if((e.KeyData == Keys.Enter) && (this.ValueList != null) && (this.ValueList.IsDroppedDown == true))
				{
					if(this.ValueList.SelectedItemIndex >= 0)
					{
//						try { ((IValueListOwner)this).OnSelectionChangeCommitted(); }
//						catch {}

						this.EditorValue = this.ValueList.GetValue( this.ValueList.SelectedItemIndex );
//						this.ExitEditMode(true, true);

						//	Raise the editor's SelectionChanged event
						this.RaiseSelectionChangedEvent();
						FTI.Shared.Win32.User.MessageBeep(0);
					}
					//this.ExitEditMode(true, true);
					//this.CloseUp();
					
//					if(this.ValueList.IsDroppedDown == true)
//					{
//						MessageBox.Show(this.ValueList.SelectedItemIndex.ToString());
//						this.CloseUp();
//
//						//	BF 3.18.02
//						//	Since the ValueList's DropDown doesn't know the ENTER
//						//	key was pressed, it also doesn't know that it should commit
//						//	the selection change, so we must emulate that by setting
//						//	the SelectedIndex property, as well as the textbox text
//						if(this.ValueList.SelectedItemIndex >= 0)
//						{
//							IValueListOwner listOwner = this as IValueListOwner;
//							listOwner.OnSelectionChangeCommitted();
//							//						MessageBox.Show(this.ValueList.SelectedItemIndex.ToString());
//							if ( this.ValueList != null )
//							{
//								//	BF 11.27.02		UWE370
//								//	We don't want to fire the events when we do this
//								try
//								{
//									this.suppressEventFiring = true;
//									this.ValueList.SelectedItemIndex = this.lastSelectedItemIndex;
//								}
//								finally
//								{
//									this.suppressEventFiring = false;
//								}
//							}
//
//							//	BF 6.6.02
//							//	Call the IValueListOwner's OnSelectionChangeCommitted method,
//							//	which will call UpdateTextBoxContents, SelectAllTextBoxText, and
//							//	fire the SelectionChangeCommitted and ValueChanged events
//							IValueListOwner listOwner = this as IValueListOwner;
//							listOwner.OnSelectionChangeCommitted();
//
//
//						}// if(this.lastSelectedItemIndex >= 0)
//					
//					}// if(this.IsDroppedDown == true)


					e.Handled = true;
				}
				else
				{
					base.InternalOnEmbeddableTextBoxListenerKeyDown (e);
			
				}// if(e.KeyData == Keys.Enter)
*/				
			}// protected override void InternalOnEmbeddableTextBoxListenerKeyDown(KeyEventArgs e)

			#endregion Protected Methods
		
			#region Properties
		

			#endregion Properties
		
		}// public class CTmaxEditorWithCombo
		
		/// <summary>This class is used to encapsulate the arguments for an event fired by this control</summary>
		public class CTmaxEditorWithText : EditorWithText, IEmbeddableTextBoxListener
		{
			#region Private Members

			/// <summary>Local member bound to EventSource property</summary>
			protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

			/// <summary>Local member bound to ReadOnly property</summary>
			private bool m_bReadOnly = false;

			/// <summary>Local member bound to MultiLine property</summary>
			private bool m_bMultiLine = true;

			#endregion Private Members
			
			#region Public Methods
		
			/// <summary>Constructor</summary>
			public CTmaxEditorWithText(EmbeddableEditorOwnerBase defaultOwner) : base(defaultOwner)
			{
			}

			#endregion Public Methods
		
			#region Protected Methods
			
			/// <summary>Listener implementation to eat keystrokes if read-only</summary>
			/// <param name="e">Keypress event arguments</param>
			void IEmbeddableTextBoxListener.OnKeyPress(KeyPressEventArgs e)
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnKeyPress", "key = " + e.KeyChar.ToString());
				
				if(this.ReadOnly == true)
				{
					FTI.Shared.Win32.User.MessageBeep(0);
					e.Handled = true;
				}
				else if(this.MultiLine == false)
				{
					//	Suppress the Enter key
					if(e.KeyChar == '\x0d')
					{
						FTI.Shared.Win32.User.MessageBeep(0);
						e.Handled = true;
					}

				}// else if(this.MultiLine == false)

			}// void IEmbeddableTextBoxListener.OnKeyPress(KeyPressEventArgs e)
			
			/// <summary>Listener implementation to eat keystrokes if read-only</summary>
			/// <param name="e">Keypress event arguments</param>
			void IEmbeddableTextBoxListener.OnKeyDown(KeyEventArgs e)
			{
				if(this.ReadOnly == true)
				{
					switch(e.KeyCode)
					{
						case Keys.Delete:
						case Keys.Enter:
						case Keys.Back:

							//m_tmaxEventSource.FireDiagnostic(this, "OnKeyDown", "Trapped -> " + e.KeyCode.ToString());
							FTI.Shared.Win32.User.MessageBeep(0);
							e.Handled = true;
							break;

					}// switch(e.KeyCode)

				}// if(this.ReadOnly == true)
			
			}// void IEmbeddableTextBoxListener.OnKeyDown(KeyEventArgs e)

			#endregion Protected Methods
		
			#region Properties

			/// <summary>Event source interface for error and diagnostic events</summary>
			public FTI.Shared.Trialmax.CTmaxEventSource EventSource
			{
				get { return m_tmaxEventSource; }
			}
			
			/// <summary>True if the text is read only</summary>
			public bool ReadOnly
			{
				get{ return m_bReadOnly; }
				set{ m_bReadOnly = value; }
			}

			/// <summary>True if multiple lines are allowed</summary>
			public bool MultiLine
			{
				get { return m_bMultiLine; }
				set { m_bMultiLine = value; }
			}

			#endregion Properties

		}// public class CTmaxEditorWithText : EditorWithText, IEmbeddableTextBoxListener

		/// <summary>This class is used to implement a numeric editor</summary>
		public class CTmaxEditorWithNumbers : EditorWithText, IEmbeddableTextBoxListener
		{
			#region Private Members

			/// <summary>Local member bound to AllowNegative property</summary>
			private bool m_bAllowNegative = true;

			/// <summary>Local member bound to AllowDecimal property</summary>
			private bool m_bAllowDecimal = true;

			#endregion Private Members

			#region Public Methods

			/// <summary>Constructor</summary>
			public CTmaxEditorWithNumbers(EmbeddableEditorOwnerBase defaultOwner)
				: base(defaultOwner)
			{
			}

			#endregion Public Methods

			#region Protected Methods

			/// <summary>Listener implementation to eat invalid keystrokes</summary>
			/// <param name="e">Keypress event arguments</param>
			void IEmbeddableTextBoxListener.OnKeyPress(KeyPressEventArgs e)
			{
				switch(e.KeyChar)
				{
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					case '\x08': // backspace

						e.Handled = false;
						break;

					case '-':

						e.Handled = !m_bAllowNegative;
						break;

					case '.':

						e.Handled = !m_bAllowDecimal;
						break;

					case '\x0D': // carraige return

						e.Handled = true;
						break;

					default:

						FTI.Shared.Win32.User.MessageBeep(0);
						e.Handled = true;
						break;

				}// switch(e.KeyChar)

			}// void IEmbeddableTextBoxListener.OnKeyPress(KeyPressEventArgs e)

			#endregion Protected Methods

			#region Properties

			/// <summary>True if negative numbers allowed</summary>
			public bool AllowNegative
			{
				get { return m_bAllowNegative; }
				set { m_bAllowNegative = value; }
			}

			/// <summary>True if decimal numbers allowed</summary>
			public bool AllowDecimal
			{
				get { return m_bAllowDecimal; }
				set { m_bAllowDecimal = value; }
			}

			#endregion Properties

		}

		#endregion Nested Classes

	}// public class CTmaxPropGridCtrl : CTmaxBaseCtrl
	 
}// namespace FTI.Trialmax.Controls
